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
            // NOTA: Tablas OP_Filtros y OP_Preguntas_Filtro existen; los SP de carga no están migrados.
            // Funcionalidad de filtros dinámicos requiere migrar los SP legacy antes de habilitarla.
            // Por ahora retorna configuración vacía (placeholder).
            
            var config = new FiltroConfigVm
            {
                TrabajoId = trabajoId,
                TipoFiltro = tipoFiltro,
                Preguntas = new List<PreguntaFiltroVm>()
            };

            _logger.LogWarning("Filtros dinámicos no disponibles - SPs de filtros no migrados. TrabajoId: {TrabajoId}", trabajoId);
            return (true, config, "Funcionalidad de filtros en construcción");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo configuración filtro trabajo {TrabajoId}, tipo {TipoFiltro}", 
                trabajoId, tipoFiltro);
            return (false, null!, "Error al obtener configuración del filtro. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, long PreguntaId, string Error)> AgregarPreguntaFiltroAsync(
        long trabajoId, int tipoFiltro, PreguntaFiltroVm pregunta, long usuarioId)
    {
        try
        {
            // Ref: DisenarFiltros.aspx.vb líneas 321-459 (btnAgregarPregunta_Click)
            // NOTA: Tablas OP_Filtros y OP_Preguntas_Filtro existen; SP OP_InsertarPreguntaFiltro no está migrado.
            _logger.LogWarning("Agregación de preguntas de filtro no disponible - SP no migrado");
            return (false, 0, "Funcionalidad de filtros dinámicos no disponible");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error agregando pregunta filtro trabajo {TrabajoId}", trabajoId);
            return (false, 0, "Error al agregar pregunta. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, string Error)> EliminarPreguntaFiltroAsync(
        long preguntaId, long usuarioId)
    {
        try
        {
            // Ref: DisenarFiltros.aspx.vb líneas 493-517 (btnEliminar_Click)
            // NOTA: Tabla real es OP_Preguntas_Filtro; SP de eliminación no está migrado.
            _logger.LogWarning("Eliminación de preguntas de filtro no disponible");
            return (false, "Funcionalidad no disponible");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando pregunta {PreguntaId}", preguntaId);
            return (false, "Error al eliminar pregunta. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, string Error)> ActualizarPreguntaFiltroAsync(
        long preguntaId, PreguntaFiltroVm pregunta, long usuarioId)
    {
        try
        {
            // NOTA: Tabla real es OP_Preguntas_Filtro; SP de actualización no está migrado.
            _logger.LogWarning("Actualización de preguntas de filtro no disponible");
            return (false, "Funcionalidad no disponible");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando pregunta {PreguntaId}", preguntaId);
            return (false, "Error al actualizar pregunta. Por favor intente nuevamente.");
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
            return (false, string.Empty, "Error al generar link de visualización. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, List<RespuestaFiltroVm> Data, string Error)> ObtenerRespuestasFiltroAsync(
        long trabajoId, int tipoFiltro, string? estado = null)
    {
        try
        {
            // NOTA: SP OP_ObtenerRespuestasFiltro no existe en BD
            // Usar SELECT directo de tabla OP_RespuestasFiltro si existe
            _logger.LogWarning("[OpFiltros] ObtenerRespuestasFiltro: SP no existe. TrabajoId: {TrabajoId}", trabajoId);
            
            using var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT * FROM OP_RespuestasFiltro 
                        WHERE TrabajoId = @TrabajoId 
                        AND (@TipoFiltro IS NULL OR TipoFiltro = @TipoFiltro)
                        AND (@Estado IS NULL OR Estado = @Estado)";
            
            var respuestas = await connection.QueryAsync<RespuestaFiltroVm>(
                sql,
                new { TrabajoId = trabajoId, TipoFiltro = tipoFiltro, Estado = estado });

            return (true, respuestas.ToList(), string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo respuestas filtro trabajo {TrabajoId}", trabajoId);
            return (false, new List<RespuestaFiltroVm>(), "Error al obtener respuestas del filtro. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, string Error)> AprobarRespuestasFiltroAsync(
        List<long> respuestasIds, long usuarioId, string? observaciones = null)
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
            return (false, "Error al aprobar respuestas. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, string Error)> RechazarRespuestasFiltroAsync(
        List<long> respuestasIds, long usuarioId, string? observaciones)
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
            return (false, "Error al rechazar respuestas. Por favor intente nuevamente.");
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
            return (false, Array.Empty<byte>(), "Error al exportar respuestas a Excel. Por favor intente nuevamente.");
        }
    }
}
