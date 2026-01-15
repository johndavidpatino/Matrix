/// <summary>
/// Interface para procesamiento de carga masiva de datos CATI y Planillas
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.7
/// </summary>
namespace MatrixNext.Data.Adapters.OP;

using MatrixNext.Data.Models.OP;

public interface ICargaMasivaAdapter
{
    /// <summary>
    /// Valida estructura de columnas del archivo Excel para CATI
    /// </summary>
    Task<(bool Valido, List<string> Errores)> ValidarColumnasExcelCatiAsync(List<string> columnasActuales);

    /// <summary>
    /// Valida estructura de columnas del archivo Excel para Planillas
    /// </summary>
    Task<(bool Valido, List<string> Errores)> ValidarColumnasExcelPlanillasAsync(List<string> columnasActuales);

    /// <summary>
    /// Valida una fila de datos CATI contra reglas de negocio
    /// </summary>
    Task<ResultadoValidacionFilaDto> ValidarFilaCatiAsync(CargaCatiRmcDto fila, int numFila);

    /// <summary>
    /// Valida una fila de datos Planilla contra reglas de negocio (corte 16-15, festivos)
    /// </summary>
    Task<ResultadoValidacionFilaDto> ValidarFilaPlanillaAsync(CargaPlanillaDto fila, int numFila);

    /// <summary>
    /// Obtiene lista de fechas festivas para validar descansos
    /// </summary>
    Task<List<DateTime>> ObtenerFestivosAsync(int año);

    /// <summary>
    /// Obtiene lista de domingos para validar descansos en Planillas
    /// </summary>
    Task<List<DateTime>> ObtenerDomingosAsync(int año);

    /// <summary>
    /// Inserta datos CATI validados en tabla temporal
    /// </summary>
    Task<int> InsertarCatiRmcAsync(List<CargaCatiRmcDto> datos, long usuarioId);

    /// <summary>
    /// Inserta datos Planilla validados en tabla temporal
    /// </summary>
    Task<int> InsertarPlanillasAsync(List<CargaPlanillaDto> datos, long usuarioId);

    /// <summary>
    /// Calcula cuota de corte 16-15 (quincena)
    /// </summary>
    Task<int> CalcularCorte16_15Async(DateTime fecha);
}
