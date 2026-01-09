using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Implementación del servicio de Ficha Cuantitativa
/// </summary>
public class OpFichaService : IOpFichaService
{
    private readonly MatrixDbContext _context;
    private readonly ILogger<OpFichaService> _logger;

    public OpFichaService(MatrixDbContext context, ILogger<OpFichaService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<FichaCuantitativaVM?> ObtenerPorTrabajoAsync(long trabajoId)
    {
        try
        {
            var sql = @"
                SELECT TOP 1
                    Id,
                    IdTrabajo,
                    GrupoObjetivo,
                    MarcoMuestral,
                    ISNULL(RequerimientosEspeciales, '') AS Incentivos,
                    ISNULL(RegalosCliente, '') AS RegaloClientes,
                    ISNULL(CompraIpsos, '') AS CompraIpsos,
                    ISNULL(OtrasObservaciones, '') AS Observaciones
                FROM OP_FichaCuantitativo
                WHERE IdTrabajo = @TrabajoId";

            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            var result = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { TrabajoId = trabajoId });

            if (result == null)
                return null;

            // Obtener Habeas Data desde la propuesta si existe
            var habeasData = await ObtenerHabeasDataDePropuestaAsync(trabajoId);

            return new FichaCuantitativaVM
            {
                Id = result.Id,
                IdTrabajo = result.IdTrabajo,
                Incentivos = result.Incentivos,
                RegaloClientes = result.RegaloClientes,
                CompraIpsos = result.CompraIpsos,
                HabeasData = habeasData ?? string.Empty,
                GrupoObjetivo = result.GrupoObjetivo ?? string.Empty,
                MarcoMuestral = result.MarcoMuestral ?? string.Empty,
                Observaciones = result.Observaciones
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener ficha cuantitativa para trabajo {TrabajoId}", trabajoId);
            return null;
        }
    }

    public async Task<long> GuardarAsync(FichaCuantitativaVM model, long usuarioId)
    {
        try
        {
            long fichaId;

            if (model.Id.HasValue && model.Id.Value > 0)
            {
                // Actualizar existente usando SP
                var sql = @"
                    EXEC OP_FichaCuantitativo_Edit 
                        @Id, @TrabajoId, @GrupoObjetivo, NULL, @MarcoMuestral, NULL, NULL, NULL, NULL, 
                        @RequerimientosEspeciales, @OtrasObservaciones, NULL, NULL, @RegalosCliente, @CompraIpsos, NULL";

                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                await connection.ExecuteAsync(sql, new
                {
                    Id = model.Id.Value,
                    TrabajoId = model.IdTrabajo,
                    GrupoObjetivo = model.GrupoObjetivo ?? string.Empty,
                    MarcoMuestral = model.MarcoMuestral ?? string.Empty,
                    RequerimientosEspeciales = model.Incentivos ?? string.Empty,
                    OtrasObservaciones = model.Observaciones ?? string.Empty,
                    RegalosCliente = model.RegaloClientes ?? string.Empty,
                    CompraIpsos = model.CompraIpsos ?? string.Empty
                });

                fichaId = model.Id.Value;
            }
            else
            {
                // Insertar nuevo usando SP
                var sql = @"
                    EXEC OP_FichaCuantitativo_Add 
                        @TrabajoId, @GrupoObjetivo, NULL, @MarcoMuestral, NULL, NULL, NULL, NULL, 
                        @RequerimientosEspeciales, @OtrasObservaciones, NULL, NULL, @RegalosCliente, @CompraIpsos, NULL";

                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                var result = await connection.QuerySingleAsync<long>(sql, new
                {
                    TrabajoId = model.IdTrabajo,
                    GrupoObjetivo = model.GrupoObjetivo ?? string.Empty,
                    MarcoMuestral = model.MarcoMuestral ?? string.Empty,
                    RequerimientosEspeciales = model.Incentivos ?? string.Empty,
                    OtrasObservaciones = model.Observaciones ?? string.Empty,
                    RegalosCliente = model.RegaloClientes ?? string.Empty,
                    CompraIpsos = model.CompraIpsos ?? string.Empty
                });

                fichaId = result;
            }

            _logger.LogInformation("Ficha cuantitativa guardada: ID {FichaId}, Trabajo {TrabajoId}, Usuario {UsuarioId}",
                fichaId, model.IdTrabajo, usuarioId);

            return fichaId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar ficha cuantitativa para trabajo {TrabajoId}", model.IdTrabajo);
            throw;
        }
    }

    public async Task SincronizarHabeasDataAsync(long trabajoId, string habeasData)
    {
        try
        {
            // Obtener el ID de proyecto asociado al trabajo
            var proyectoId = await ObtenerIdProyectoPorTrabajoAsync(trabajoId);
            
            if (!proyectoId.HasValue)
            {
                _logger.LogWarning("No se encontró proyecto asociado al trabajo {TrabajoId} para sincronizar Habeas Data", trabajoId);
                return;
            }

            // Actualizar Habeas Data en la propuesta usando SP
            var sql = "EXEC CU_Propuestas_Edit_HabeasData @Id, @RequestHabeasData";

            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            await connection.ExecuteAsync(sql, new
            {
                Id = proyectoId.Value,
                RequestHabeasData = habeasData ?? string.Empty
            });

            _logger.LogInformation("Habeas Data sincronizado: Trabajo {TrabajoId}, Proyecto {ProyectoId}", trabajoId, proyectoId.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al sincronizar Habeas Data para trabajo {TrabajoId}", trabajoId);
            throw;
        }
    }

    public async Task<long?> ObtenerIdProyectoPorTrabajoAsync(long trabajoId)
    {
        try
        {
            var sql = "SELECT IdProyecto FROM PY_Trabajo WHERE Id = @TrabajoId";

            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            var proyectoId = await connection.QuerySingleOrDefaultAsync<long?>(sql, new { TrabajoId = trabajoId });

            return proyectoId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener ID de proyecto para trabajo {TrabajoId}", trabajoId);
            return null;
        }
    }

    /// <summary>
    /// Obtiene el Habeas Data desde la tabla de propuestas
    /// </summary>
    private async Task<string?> ObtenerHabeasDataDePropuestaAsync(long trabajoId)
    {
        try
        {
            var sql = @"
                SELECT p.RequestHabeasData
                FROM CU_Propuestas p
                INNER JOIN PY_Trabajo t ON t.IdProyecto = p.Id
                WHERE t.Id = @TrabajoId";

            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            var habeasData = await connection.QuerySingleOrDefaultAsync<string>(sql, new { TrabajoId = trabajoId });

            return habeasData;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo obtener Habeas Data de propuesta para trabajo {TrabajoId}", trabajoId);
            return null;
        }
    }
}
