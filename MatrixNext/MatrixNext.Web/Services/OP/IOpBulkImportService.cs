namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Interfaz para servicio de importación bulk de muestras
/// Ref: WebMatrix Bulk Upload functionality + ClosedXML para Excel
/// Sprint 6 Fase 6: Bulk Import
/// </summary>
public interface IOpBulkImportService
{
    /// <summary>
    /// Validar estructura de archivo Excel/CSV antes de procesar
    /// Retorna lista de errores encontrados (vacía si es válido)
    /// </summary>
    Task<(bool Valid, List<string> Errors)> ValidarArchivoAsync(Stream archivoStream, string nombreArchivo);

    /// <summary>
    /// Procesar y importar muestras desde Excel/CSV
    /// Retorna cantidad de registros procesados y errores
    /// </summary>
    Task<(bool Success, int Insertados, int Errores, List<string> Mensajes)> ImportarMuestrasAsync(
        Stream archivoStream, string nombreArchivo, long trabajoId, long usuarioId);

    /// <summary>
    /// Generar archivo plantilla Excel con estructura correcta
    /// </summary>
    Task<byte[]> GenerarPlantillaExcelAsync();

    /// <summary>
    /// Obtener historial de imports por trabajo
    /// </summary>
    Task<List<ImportHistorialVm>> ObtenerHistorialImportsAsync(long trabajoId);
}

public class ImportHistorialVm
{
    public long Id { get; set; }
    public long TrabajoId { get; set; }
    public string NombreArchivo { get; set; } = string.Empty;
    public int RegistrosProcessados { get; set; }
    public int RegistrosExitosos { get; set; }
    public int RegistrosError { get; set; }
    public DateTime FechaImport { get; set; }
    public string UsuarioId { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
}
