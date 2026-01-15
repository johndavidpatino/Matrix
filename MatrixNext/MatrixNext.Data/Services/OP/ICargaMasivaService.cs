/// <summary>
/// Interface para servicio de procesamiento de carga masiva
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.7
/// </summary>
namespace MatrixNext.Data.Services.OP;

using MatrixNext.Data.Models.OP;

public interface ICargaMasivaService
{
    /// <summary>
    /// Procesa archivo Excel para CATI, validando estructura y contenido
    /// </summary>
    Task<ResumenCargaMasivaDto> ProcesarCatiRmcAsync(Stream archivoStream, string nombreArchivo, long usuarioId, bool ejecutar = false);

    /// <summary>
    /// Procesa archivo Excel para Planillas, validando estructura y contenido
    /// </summary>
    Task<ResumenCargaMasivaDto> ProcesarPlanillasAsync(Stream archivoStream, string nombreArchivo, long usuarioId, bool ejecutar = false);

    /// <summary>
    /// Extrae datos del archivo Excel a lista de DTOs
    /// </summary>
    Task<List<T>> ExtraerDatosExcelAsync<T>(Stream archivoStream, string nombreHoja) where T : class;

    /// <summary>
    /// Valida todas las filas extraídas del archivo
    /// </summary>
    Task<ResumenCargaMasivaDto> ValidarFilasAsync<T>(List<T> filas, string tipoCarga, long usuarioId) where T : class;
}
