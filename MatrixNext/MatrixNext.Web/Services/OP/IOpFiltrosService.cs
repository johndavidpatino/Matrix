using MatrixNext.Web.Services.OP.Models;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Servicio para gestión de filtros de reclutamiento y asistencia
/// Ref: ANALISIS_OP_CUALITATIVO_FASE4_FLUJOS2Y3.md § 3.2
/// Tareas: OP-F01, OP-F02 (Filtros)
/// </summary>
public interface IOpFiltrosService
{
    /// <summary>
    /// Obtiene configuración de filtro existente
    /// Ref: DisenarFiltros.aspx.vb líneas 45-89 (cargarPreguntasFiltro)
    /// </summary>
    Task<(bool Success, FiltroConfigVm Data, string Error)> ObtenerConfiguracionFiltroAsync(
        long trabajoId, int tipoFiltro);

    /// <summary>
    /// Agrega pregunta a filtro (dinámico según tipo)
    /// Ref: DisenarFiltros.aspx.vb líneas 321-459 (btnAgregarPregunta_Click)
    /// </summary>
    Task<(bool Success, long PreguntaId, string Error)> AgregarPreguntaFiltroAsync(
        long trabajoId, int tipoFiltro, PreguntaFiltroVm pregunta, long usuarioId);

    /// <summary>
    /// Elimina pregunta de filtro
    /// Ref: DisenarFiltros.aspx.vb líneas 493-517 (btnEliminar_Click)
    /// </summary>
    Task<(bool Success, string Error)> EliminarPreguntaFiltroAsync(
        long preguntaId, long usuarioId);

    /// <summary>
    /// Actualiza pregunta existente
    /// Ref: DisenarFiltros.aspx.vb líneas 461-491 (btnActualizar_Click)
    /// </summary>
    Task<(bool Success, string Error)> ActualizarPreguntaFiltroAsync(
        long preguntaId, PreguntaFiltroVm pregunta, long usuarioId);

    /// <summary>
    /// Genera link de visualización del filtro
    /// Ref: DisenarFiltros.aspx.vb líneas 519-546 (GenerarLink)
    /// </summary>
    Task<(bool Success, string LinkVisualizacion, string Error)> GenerarLinkVisualizacionAsync(
        long trabajoId, int tipoFiltro);

    /// <summary>
    /// Obtiene respuestas de filtro para aprobación
    /// Ref: AprobacionesFiltros.aspx.vb líneas 28-91 (Page_Load, cargarRespuestas)
    /// </summary>
    Task<(bool Success, List<RespuestaFiltroVm> Data, string Error)> ObtenerRespuestasFiltroAsync(
        long trabajoId, int tipoFiltro, string? estado = null);

    /// <summary>
    /// Aprueba respuestas de filtro
    /// Ref: AprobacionesFiltros.aspx.vb líneas 143-188 (btnAprobar_Click)
    /// </summary>
    Task<(bool Success, string Error)> AprobarRespuestasFiltroAsync(
        List<long> respuestasIds, long usuarioId, string? observaciones = null);

    /// <summary>
    /// Rechaza respuestas de filtro
    /// Ref: AprobacionesFiltros.aspx.vb líneas 190-235 (btnRechazar_Click)
    /// </summary>
    Task<(bool Success, string Error)> RechazarRespuestasFiltroAsync(
        List<long> respuestasIds, long usuarioId, string? observaciones);

    /// <summary>
    /// Exporta respuestas a Excel (SP REP_OP_Respuestas_Filtro)
    /// Ref: AprobacionesFiltros.aspx.vb líneas 237-270 (btnExportarExcel_Click)
    /// </summary>
    Task<(bool Success, byte[] Data, string Error)> ExportarRespuestasExcelAsync(
        long trabajoId, int tipoFiltro);
}
