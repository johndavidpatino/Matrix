using MatrixNext.Web.Services.OP.Models;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Servicio para administración de planillas de moderación e informes cualitativos
/// Ref: AdministracionRegistroPlanillas.aspx + RegistroPlanillasCualitativo.aspx
/// </summary>
public interface IOpPlanillasModeracionService
{
    /// <summary>
    /// Obtener planillas con filtros y paginación
    /// </summary>
    /// <param name="tipoPlantilla">"Moderacion" o "Informes"</param>
    /// <param name="idEstado">1=EnEspera, 2=Aprobado, 3=NoAprobado, null=Todos</param>
    Task<(bool success, List<PlanillaListItemVm> data, int totalRecords, string error)> ObtenerPlanillasAsync(
        string? tipoPlantilla,
        short? idEstado,
        int pageIndex = 0,
        int pageSize = 25);

    /// <summary>
    /// Obtener planilla de moderación por ID
    /// </summary>
    Task<(bool success, PlanillaModeracionVm? data, string error)> ObtenerPlanillaModeracionAsync(long idPlanilla);

    /// <summary>
    /// Obtener planilla de informes por ID
    /// </summary>
    Task<(bool success, PlanillaInformeVm? data, string error)> ObtenerPlanillaInformeAsync(long idPlanilla);

    /// <summary>
    /// Guardar planilla de moderación (INSERT o UPDATE)
    /// </summary>
    Task<(bool success, long idPlanilla, string error)> GuardarPlanillaModeracionAsync(
        PlanillaModeracionVm model,
        long usuarioId);

    /// <summary>
    /// Guardar planilla de informes (INSERT o UPDATE)
    /// </summary>
    Task<(bool success, long idPlanilla, string error)> GuardarPlanillaInformeAsync(
        PlanillaInformeVm model,
        long usuarioId);

    /// <summary>
    /// Aprobar planilla (moderación o informe)
    /// </summary>
    Task<(bool success, string error)> AprobarPlanillaAsync(
        long idPlanilla,
        string tipoPlantilla,
        long usuarioId,
        string? observaciones = null);

    /// <summary>
    /// Rechazar planilla (moderación o informe)
    /// </summary>
    Task<(bool success, string error)> RechazarPlanillaAsync(
        long idPlanilla,
        string tipoPlantilla,
        long usuarioId,
        string observaciones); // Observaciones requeridas en rechazo

    /// <summary>
    /// Exportar planillas a Excel
    /// </summary>
    Task<byte[]> ExportarPlanillasExcelAsync(
        string? tipoPlantilla,
        short? idEstado);

    /// <summary>
    /// Buscar JobBooks por término de búsqueda
    /// </summary>
    Task<List<JobBookSearchVm>> BuscarJobBooksAsync(string termino);

    /// <summary>
    /// Obtener listado de moderadores disponibles
    /// </summary>
    Task<List<ModeradorVm>> ObtenerModeradoresAsync();

    /// <summary>
    /// Obtener técnicas cualitativas por tipo
    /// </summary>
    /// <param name="tipoTecnica">Filtro opcional por tipo</param>
    Task<List<TecnicaVm>> ObtenerTecnicasAsync(string? tipoTecnica = null);
}
