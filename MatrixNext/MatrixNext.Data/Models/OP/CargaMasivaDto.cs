/// <summary>
/// DTOs para procesamiento de carga masiva de datos CATI y Planillas
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.7
/// </summary>
namespace MatrixNext.Data.Models.OP;

/// <summary>
/// Datos de entrada para validación de carga CATI RMC
/// </summary>
public class CargaCatiRmcDto
{
    public long TrabajoId { get; set; }
    public int Res_Numero { get; set; }
    public string Per_NumIdentificacionEncu { get; set; }
    public string? Per_NumIdentificacionSup { get; set; }
    public string? Res_IDM { get; set; }
    public string? Res_Ciudad { get; set; }
    public DateTime? Res_Fecha { get; set; }
    public string? TipoSupervision { get; set; }
    public string TipoActividad { get; set; } // Enum: Implementación, InstruccionarioRespondido, InstruccionarioCorregido, Supervisión
}

/// <summary>
/// Datos de entrada para validación de carga Planillas
/// </summary>
public class CargaPlanillaDto
{
    public long IdTrabajo { get; set; }
    public long IdEmpleado { get; set; }
    public DateTime Fecha { get; set; }
    public int Cantidad { get; set; }
    public string? TipoProductividad { get; set; } // Encuestas, Llamadas, etc.
    public string? Observaciones { get; set; }
}

/// <summary>
/// Resultado de validación de fila
/// </summary>
public class ResultadoValidacionFilaDto
{
    public int NumeroFila { get; set; }
    public bool EsValida { get; set; }
    public List<string> Errores { get; set; } = new();
    public string? Advertencia { get; set; }
}

/// <summary>
/// Resumen del procesamiento de carga masiva
/// </summary>
public class ResumenCargaMasivaDto
{
    public string TipoCarga { get; set; } // "CATI" o "Planillas"
    public int TotalFilas { get; set; }
    public int FilasValidas { get; set; }
    public int FilasRechazadas { get; set; }
    public List<ResultadoValidacionFilaDto> Validaciones { get; set; } = new();
    public DateTime FechaCarga { get; set; }
    public long UsuarioId { get; set; }
    public string NombreArchivo { get; set; }
    public long BytesArchivo { get; set; }
}

/// <summary>
/// Configuración de validaciones por tipo de carga (leído de appsettings)
/// </summary>
public class ConfiguracionCargaMasivaDto
{
    public int MaximoFilasPermitidas { get; set; } = 10000;
    public int MaximoBytesArchivo { get; set; } = 5242880; // 5 MB
    public List<string> ExtensionesPermitidas { get; set; } = new() { ".xls", ".xlsx" };
    public bool PermitirDuplicados { get; set; } = false;
    public bool ValidarFestivos { get; set; } = true;
    public bool CalcularCorte16_15 { get; set; } = true;
}
