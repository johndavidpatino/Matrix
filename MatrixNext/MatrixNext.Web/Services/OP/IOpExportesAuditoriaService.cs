namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Data transfer object for export audit record
/// Ref: S4-004 implementation
/// </summary>
public sealed record OpExportAuditoriaDto
{
    public long IdExporte { get; init; }
    public long TrabajoId { get; init; }
    public string Tipo { get; init; } = string.Empty;
    public long? Usuario { get; init; }
    public DateTime FechaExportacion { get; init; }
    public string RutaArchivo { get; init; } = string.Empty;
    public string NombreArchivo { get; init; } = string.Empty;
    public long? TamanoBytes { get; init; }
    public bool Exitoso { get; init; }
    public string? MensajeError { get; init; }
    public DateTime? FechaProgramadaLimpieza { get; init; }
}

/// <summary>
/// Service interface for export audit logging
/// Tracks all Excel exports for compliance and cleanup
/// Ref: ESPECIFICACION_COMPONENTES_COMPARTIDOS.md § 4.3 (Auditoría de Exportaciones)
/// </summary>
public interface IOpExportesAuditoriaService
{
    /// <summary>
    /// Register successful export in audit table
    /// </summary>
    Task<long> RegistrarExportacionAsync(
        long trabajoId,
        string tipo,
        long? usuario,
        string rutaArchivo,
        string nombreArchivo,
        long? tamanoBytes);

    /// <summary>
    /// Register failed export attempt
    /// </summary>
    Task<long> RegistrarErrorExportacionAsync(
        long trabajoId,
        string tipo,
        long? usuario,
        string mensajeError);

    /// <summary>
    /// Get all exports for a specific work order
    /// </summary>
    Task<List<OpExportAuditoriaDto>> ObtenerExportacionesPorTrabajoAsync(long trabajoId);

    /// <summary>
    /// Get exports within date range
    /// </summary>
    Task<List<OpExportAuditoriaDto>> ObtenerExportacionesPorFechaAsync(DateTime desde, DateTime hasta);

    /// <summary>
    /// Get exports pending cleanup (older than 30 days)
    /// </summary>
    Task<List<OpExportAuditoriaDto>> ObtenerExportacionesPendienteLimpiezaAsync();

    /// <summary>
    /// Mark export as cleaned and delete physical file
    /// </summary>
    Task<bool> LimpiarExportacionAsync(long idExporte);

    /// <summary>
    /// Batch cleanup of old exports
    /// </summary>
    Task<int> LimpiarExportacionesAntiguasAsync(int diasRetension = 30);

    /// <summary>
    /// Get total statistics for exports
    /// </summary>
    Task<(int Total, int Exitosos, int Fallidos, long TamanoTotalBytes)> ObtenerEstadisticasAsync();
}
