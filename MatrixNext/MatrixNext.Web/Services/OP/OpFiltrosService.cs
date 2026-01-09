using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Services.OP.Models;
using MatrixNext.Web.Services.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Implementación del servicio de filtros
/// Ref: ANALISIS_OP_CUALITATIVO_FASE4_FLUJOS2Y3.md § 3.2
/// </summary>
public class OpFiltrosService : IOpFiltrosService
{
    private readonly MatrixDbContext _context;
    private readonly string _connectionString;
    private readonly ILogger<OpFiltrosService> _logger;
    private readonly IExportService _exportService;

    public OpFiltrosService(
        MatrixDbContext context,
        IConfiguration configuration,
        ILogger<OpFiltrosService> logger,
        IExportService exportService)
    {
        _context = context;
        _connectionString = configuration.GetConnectionString("MatrixDb")!;
        _logger = logger;
        _exportService = exportService;
    }

    public async Task<(bool Success, FiltroConfigVm Data, string Error)> ObtenerConfiguracionFiltroAsync(
        long trabajoId, int tipoFiltro)
    {
        try
        {
            // Ref: DisenarFiltros.aspx.vb líneas 45-89 (cargarPreguntasFiltro)
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@TrabajoId", trabajoId);
            parameters.Add("@TipoFiltro", tipoFiltro);

            var config = new FiltroConfigVm
            {
                TrabajoId = trabajoId,
                TipoFiltro = tipoFiltro
            };

            // Obtener preguntas
            var preguntas = await connection.QueryAsync<PreguntaFiltroVm>(
                "OP_ObtenerPreguntasFiltro",
                parameters,
                commandType: CommandType.StoredProcedure);

            config.Preguntas = preguntas.ToList();

            // Obtener opciones para preguntas de selección/multi
            foreach (var pregunta in config.Preguntas.Where(p => p.TipoPregunta == 3 || p.TipoPregunta == 4))
            {
                var opciones = await connection.QueryAsync<OpcionPreguntaVm>(
                    "OP_ObtenerOpcionesPregunta",
                    new { PreguntaId = pregunta.Id },
                    commandType: CommandType.StoredProcedure);
                
                pregunta.Opciones = opciones.ToList();
            }

            return (true, config, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo configuración filtro trabajo {TrabajoId}, tipo {TipoFiltro}", 
                trabajoId, tipoFiltro);
            return (false, null!, ex.Message);
        }
    }

    public async Task<(bool Success, long PreguntaId, string Error)> AgregarPreguntaFiltroAsync(
        long trabajoId, int tipoFiltro, PreguntaFiltroVm pregunta, long usuarioId)
    {
        try
        {
            // Ref: DisenarFiltros.aspx.vb líneas 321-459 (btnAgregarPregunta_Click)
            using var connection = new SqlConnection(_connectionString);
            using var transaction = connection.BeginTransaction();

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", trabajoId);
                parameters.Add("@TipoFiltro", tipoFiltro);
                parameters.Add("@TipoPregunta", pregunta.TipoPregunta);
                parameters.Add("@TextoPregunta", pregunta.TextoPregunta);
                parameters.Add("@Obligatoria", pregunta.Obligatoria);
                parameters.Add("@Orden", pregunta.Orden);
                parameters.Add("@CreadoPor", usuarioId);
                parameters.Add("@PreguntaId", dbType: DbType.Int64, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(
                    "OP_InsertarPreguntaFiltro",
                    parameters,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);

                var preguntaId = parameters.Get<long>("@PreguntaId");

                // Insertar opciones si aplica
                if (pregunta.Opciones?.Count > 0)
                {
                    foreach (var opcion in pregunta.Opciones)
                    {
                        await connection.ExecuteAsync(
                            @"INSERT INTO OP_OpcionesPreguntaFiltro (PreguntaId, Texto, Orden)
                              VALUES (@PreguntaId, @Texto, @Orden)",
                            new { PreguntaId = preguntaId, opcion.Texto, opcion.Orden },
                            transaction: transaction);
                    }
                }

                transaction.Commit();
                return (true, preguntaId, string.Empty);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error agregando pregunta filtro trabajo {TrabajoId}", trabajoId);
            return (false, 0, ex.Message);
        }
    }

    public async Task<(bool Success, string Error)> EliminarPreguntaFiltroAsync(
        long preguntaId, long usuarioId)
    {
        try
        {
            // Ref: DisenarFiltros.aspx.vb líneas 493-517 (btnEliminar_Click)
            using var connection = new SqlConnection(_connectionString);
            
            await connection.ExecuteAsync(
                "DELETE FROM OP_PreguntasFiltro WHERE Id = @PreguntaId",
                new { PreguntaId = preguntaId });

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando pregunta {PreguntaId}", preguntaId);
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string Error)> ActualizarPreguntaFiltroAsync(
        long preguntaId, PreguntaFiltroVm pregunta, long usuarioId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            await connection.ExecuteAsync(
                @"UPDATE OP_PreguntasFiltro 
                  SET TextoPregunta = @TextoPregunta,
                      Obligatoria = @Obligatoria,
                      Orden = @Orden
                  WHERE Id = @PreguntaId",
                new { PreguntaId = preguntaId, pregunta.TextoPregunta, pregunta.Obligatoria, pregunta.Orden });

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando pregunta {PreguntaId}", preguntaId);
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string LinkVisualizacion, string Error)> GenerarLinkVisualizacionAsync(
        long trabajoId, int tipoFiltro)
    {
        try
        {
            // Ref: DisenarFiltros.aspx.vb líneas 519-546 (GenerarLink)
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var link = $"/OP/VisualizadorFiltros?t={trabajoId}&tipo={tipoFiltro}&token={token}";
            
            return (true, link, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generando link visualización trabajo {TrabajoId}", trabajoId);
            return (false, string.Empty, ex.Message);
        }
    }

    public async Task<(bool Success, List<RespuestaFiltroVm> Data, string Error)> ObtenerRespuestasFiltroAsync(
        long trabajoId, int tipoFiltro, string estado = null)
    {
        try
        {
            // Ref: AprobacionesFiltros.aspx.vb líneas 28-91
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@TrabajoId", trabajoId);
            parameters.Add("@TipoFiltro", tipoFiltro);
            parameters.Add("@Estado", estado);

            var respuestas = await connection.QueryAsync<RespuestaFiltroVm>(
                "OP_ObtenerRespuestasFiltro",
                parameters,
                commandType: CommandType.StoredProcedure);

            return (true, respuestas.ToList(), string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo respuestas filtro trabajo {TrabajoId}", trabajoId);
            return (false, new List<RespuestaFiltroVm>(), ex.Message);
        }
    }

    public async Task<(bool Success, string Error)> AprobarRespuestasFiltroAsync(
        List<long> respuestasIds, long usuarioId, string observaciones = null)
    {
        try
        {
            // Ref: AprobacionesFiltros.aspx.vb líneas 143-188 (btnAprobar_Click)
            using var connection = new SqlConnection(_connectionString);
            
            foreach (var respuestaId in respuestasIds)
            {
                await connection.ExecuteAsync(
                    @"UPDATE OP_RespuestasFiltro 
                      SET Estado = 'Aprobada',
                          FechaAprobacion = GETDATE(),
                          AprobadoPor = @UsuarioId,
                          ObservacionesAprobacion = @Observaciones
                      WHERE Id = @RespuestaId",
                    new { RespuestaId = respuestaId, UsuarioId = usuarioId, Observaciones = observaciones ?? string.Empty });

                // Log de auditoría
                await connection.ExecuteAsync(
                    @"INSERT INTO OP_LogRespuestas_Filtro (RespuestaId, Accion, UsuarioId, Fecha, Observaciones)
                      VALUES (@RespuestaId, 'Aprobada', @UsuarioId, GETDATE(), @Observaciones)",
                    new { RespuestaId = respuestaId, UsuarioId = usuarioId, Observaciones = observaciones ?? string.Empty });
            }

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error aprobando respuestas filtro");
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string Error)> RechazarRespuestasFiltroAsync(
        List<long> respuestasIds, long usuarioId, string observaciones)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            foreach (var respuestaId in respuestasIds)
            {
                await connection.ExecuteAsync(
                    @"UPDATE OP_RespuestasFiltro 
                      SET Estado = 'Rechazada',
                          FechaAprobacion = GETDATE(),
                          AprobadoPor = @UsuarioId,
                          ObservacionesAprobacion = @Observaciones
                      WHERE Id = @RespuestaId",
                    new { RespuestaId = respuestaId, UsuarioId = usuarioId, Observaciones = observaciones });

                // Log de auditoría
                await connection.ExecuteAsync(
                    @"INSERT INTO OP_LogRespuestas_Filtro (RespuestaId, Accion, UsuarioId, Fecha, Observaciones)
                      VALUES (@RespuestaId, 'Rechazada', @UsuarioId, GETDATE(), @Observaciones)",
                    new { RespuestaId = respuestaId, UsuarioId = usuarioId, Observaciones = observaciones });
            }

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rechazando respuestas filtro");
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, byte[] Data, string Error)> ExportarRespuestasExcelAsync(
        long trabajoId, int tipoFiltro)
    {
        try
        {
            // Ref: AprobacionesFiltros.aspx.vb líneas 237-270 (SP REP_OP_Respuestas_Filtro)
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@TrabajoId", trabajoId);
            parameters.Add("@TipoFiltro", tipoFiltro);

            var data = await connection.QueryAsync<dynamic>(
                "REP_OP_Respuestas_Filtro",
                parameters,
                commandType: CommandType.StoredProcedure);

                var excelBytes = await _exportService.ExportarExcelAsync(
                data.ToList(),
                $"Respuestas_Filtro_{trabajoId}_{tipoFiltro}_{DateTime.Now:yyyyMMdd}");

            return (true, excelBytes, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando respuestas Excel trabajo {TrabajoId}", trabajoId);
            return (false, Array.Empty<byte>(), ex.Message);
        }
    }
}
