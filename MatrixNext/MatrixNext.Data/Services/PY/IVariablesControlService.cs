using MatrixNext.Data.DTOs.PY;

namespace MatrixNext.Data.Services.PY;

public interface IVariablesControlService
{
    // CRUD
    Task<VariablesControlViewModel> PrepararViewModelAsync(long idTrabajo, string? tipoEvaluado = null);
    Task<(bool success, string message, long id)> CrearVariableControlAsync(VariableControlDto dto, long userId);
    Task<VariableControlDto?> ObtenerVariableControlAsync(long id);
    Task<List<VariableControlDto>> ObtenerVariablesControlPorTrabajoAsync(long idTrabajo, string? tipoEvaluado = null);
    
    // Reportes
    Task<List<ReporteVariableControlDto>> ObtenerReporteVariablesControlAsync(VariablesControlFiltrosDto filtros);
    Task<List<ReporteVariableControlPorMesDto>> ObtenerReporteVariablesControlPorMesAsync(VariablesControlFiltrosDto filtros);
    Task<Dictionary<long, string>> ObtenerEmpleadosConEvaluacionAsync();
    
    // Export
    Task<byte[]> ExportarReporteExcelAsync(VariablesControlFiltrosDto filtros, string tipoReporte);
}
