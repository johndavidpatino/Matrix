using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.OP;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MatrixNext.Web.Services.OP
{
    /// <summary>
    /// Implementación del servicio de gestión de muestra por ciudad
    /// </summary>
    public class OpMuestraService : IOpMuestraService
    {
        private readonly MatrixDbContext _dbContext;
        private readonly ILogger<OpMuestraService> _logger;

        public OpMuestraService(MatrixDbContext dbContext, ILogger<OpMuestraService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<MuestraCiudadListItemVM>> ObtenerMuestraPorTrabajoAsync(long trabajoId)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                // Query con JOIN a Divipola para obtener nombres de departamento y ciudad
                var muestras = await connection.QueryAsync<MuestraCiudadDto>(@"
                    SELECT 
                        m.Id,
                        d.DivDeptoNombre AS Departamento,
                        d.DivMuniNombre AS Ciudad,
                        m.CiudadId,
                        m.Cantidad,
                        m.FechaInicio,
                        m.FechaFin,
                        CONCAT(u.Nombres, ' ', u.Apellidos) AS CoordinadorNombre
                    FROM OP_MuestraTrabajos m
                    LEFT JOIN C_Divipola d ON m.CiudadId = d.DivMuniCodigo
                    LEFT JOIN TH_Personas u ON m.Coordinador = u.IdPersona
                    WHERE m.TrabajoId = @TrabajoId
                    ORDER BY d.DivMuniNombre",
                    new { TrabajoId = trabajoId });

                return muestras.Select(m => new MuestraCiudadListItemVM
                {
                    Id = m.Id,
                    Departamento = m.Departamento,
                    Ciudad = m.Ciudad,
                    CiudadId = m.CiudadId,
                    Cantidad = m.Cantidad,
                    FechaInicio = m.FechaInicio,
                    FechaFin = m.FechaFin,
                    CoordinadorNombre = m.CoordinadorNombre
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener muestra del trabajo {TrabajoId}", trabajoId);
                return new List<MuestraCiudadListItemVM>();
            }
        }

        public async Task<MuestraCiudadVM?> ObtenerMuestraPorIdAsync(long idMuestra)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                var muestra = await connection.QueryFirstOrDefaultAsync<MuestraCiudadDto>(@"
                    SELECT Id, TrabajoId, CiudadId, Cantidad, FechaInicio, FechaFin, Coordinador AS CoordinadorId
                    FROM OP_MuestraTrabajos
                    WHERE Id = @IdMuestra",
                    new { IdMuestra = idMuestra });

                if (muestra == null)
                    return null;

                return new MuestraCiudadVM
                {
                    Id = muestra.Id,
                    TrabajoId = muestra.TrabajoId,
                    CiudadId = muestra.CiudadId ?? 0,
                    Cantidad = muestra.Cantidad,
                    FechaInicio = muestra.FechaInicio,
                    FechaFin = muestra.FechaFin,
                    CoordinadorId = muestra.CoordinadorId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener muestra {IdMuestra}", idMuestra);
                return null;
            }
        }

        public async Task<double> ObtenerMuestraPorCiudadAsync(long trabajoId, int ciudadId)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                var cantidad = await connection.QueryFirstOrDefaultAsync<double?>(@"
                    SELECT Cantidad
                    FROM OP_MuestraTrabajos
                    WHERE TrabajoId = @TrabajoId AND CiudadId = @CiudadId",
                    new { TrabajoId = trabajoId, CiudadId = ciudadId });

                return cantidad ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener muestra de ciudad {CiudadId} en trabajo {TrabajoId}", 
                    ciudadId, trabajoId);
                return 0;
            }
        }

        public async Task<long> GuardarMuestraAsync(MuestraCiudadVM model)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                if (model.Id.HasValue && model.Id.Value > 0)
                {
                    // Actualizar muestra existente
                    await connection.ExecuteAsync(@"
                        UPDATE OP_MuestraTrabajos
                        SET Cantidad = @Cantidad,
                            FechaInicio = @FechaInicio,
                            FechaFin = @FechaFin,
                            Coordinador = @CoordinadorId
                        WHERE Id = @Id",
                        new
                        {
                            model.Id,
                            model.Cantidad,
                            model.FechaInicio,
                            model.FechaFin,
                            model.CoordinadorId
                        });

                    _logger.LogInformation("Muestra {Id} actualizada para trabajo {TrabajoId}, ciudad {CiudadId}",
                        model.Id, model.TrabajoId, model.CiudadId);

                    return model.Id.Value;
                }
                else
                {
                    // Insertar nueva muestra
                    var id = await connection.QuerySingleAsync<long>(@"
                        INSERT INTO OP_MuestraTrabajos (TrabajoId, CiudadId, Cantidad, FechaInicio, FechaFin, Coordinador)
                        VALUES (@TrabajoId, @CiudadId, @Cantidad, @FechaInicio, @FechaFin, @CoordinadorId);
                        SELECT CAST(SCOPE_IDENTITY() as bigint);",
                        new
                        {
                            model.TrabajoId,
                            model.CiudadId,
                            model.Cantidad,
                            model.FechaInicio,
                            model.FechaFin,
                            model.CoordinadorId
                        });

                    _logger.LogInformation("Muestra {Id} creada para trabajo {TrabajoId}, ciudad {CiudadId}",
                        id, model.TrabajoId, model.CiudadId);

                    return id;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar muestra para trabajo {TrabajoId}", model.TrabajoId);
                throw;
            }
        }

        public async Task<bool> ActualizarFechasConPlaneacionAsync(ActualizarFechasMuestraVM model)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                await using var transaction = await connection.BeginTransactionAsync();
                try
                {
                    // 1. Actualizar fechas en OP_MuestraTrabajos
                    await connection.ExecuteAsync(@"
                        UPDATE OP_MuestraTrabajos
                        SET FechaInicio = @FechaInicio,
                            FechaFin = @FechaFin
                        WHERE Id = @IdMuestra",
                        new
                        {
                            model.IdMuestra,
                            model.FechaInicio,
                            model.FechaFin
                        },
                        transaction: transaction);

                    // 2. Ejecutar SP de auto-planeación
                    // Mapea a OP_AjusteProduccionAutoCiudad
                    await connection.ExecuteAsync(
                        "OP_AjusteProduccionAutoCiudad",
                        new
                        {
                            IdMuestra = model.IdMuestra,
                            lun = model.Lunes,
                            mar = model.Martes,
                            mie = model.Miercoles,
                            jue = model.Jueves,
                            vie = model.Viernes,
                            sab = model.Sabado,
                            dom = model.Domingo,
                            fest = !model.ExcluirFestivos // SP usa 'fest' para incluir festivos
                        },
                        transaction: transaction,
                        commandType: CommandType.StoredProcedure);

                    await transaction.CommitAsync();

                    _logger.LogInformation("Fechas y planeación actualizadas para muestra {IdMuestra}", model.IdMuestra);
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar fechas con planeación para muestra {IdMuestra}", model.IdMuestra);
                return false;
            }
        }

        public async Task<bool> EliminarMuestraAsync(long idMuestra)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                var rowsAffected = await connection.ExecuteAsync(@"
                    DELETE FROM OP_MuestraTrabajos
                    WHERE Id = @IdMuestra",
                    new { IdMuestra = idMuestra });

                if (rowsAffected > 0)
                {
                    _logger.LogInformation("Muestra {IdMuestra} eliminada correctamente", idMuestra);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar muestra {IdMuestra}", idMuestra);
                return false;
            }
        }

        public async Task<double> CalcularTotalMuestraAsync(long trabajoId)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                var total = await connection.QueryFirstOrDefaultAsync<double?>(@"
                    SELECT SUM(Cantidad)
                    FROM OP_MuestraTrabajos
                    WHERE TrabajoId = @TrabajoId",
                    new { TrabajoId = trabajoId });

                return total ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al calcular total de muestra del trabajo {TrabajoId}", trabajoId);
                return 0;
            }
        }

        #region DTOs Internos

        private class MuestraCiudadDto
        {
            public long Id { get; set; }
            public long TrabajoId { get; set; }
            public int? CiudadId { get; set; }
            public string? Departamento { get; set; }
            public string? Ciudad { get; set; }
            public double Cantidad { get; set; }
            public DateTime? FechaInicio { get; set; }
            public DateTime? FechaFin { get; set; }
            public long? CoordinadorId { get; set; }
            public string? CoordinadorNombre { get; set; }
        }

        #endregion
    }
}
