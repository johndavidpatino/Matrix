namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para registro de errores de campo
/// Mapea resultados del SP: MBO_CampoErroresGet
/// </summary>
public class CampoErrorDto
{
    /// <summary>ID del error</summary>
    public int IdError { get; set; }

    /// <summary>ID del trabajo</summary>
    public int IdTrabajo { get; set; }

    /// <summary>Nombre del trabajo</summary>
    public string? NombreTrabajo { get; set; }

    /// <summary>ID del encuestador</summary>
    public int IdEncuestador { get; set; }

    /// <summary>Nombre del encuestador</summary>
    public string NombreEncuestador { get; set; } = string.Empty;

    /// <summary>Código del encuestador</summary>
    public string CodigoEncuestador { get; set; } = string.Empty;

    /// <summary>ID de la ciudad</summary>
    public int IdCiudad { get; set; }

    /// <summary>Nombre de la ciudad</summary>
    public string NombreCiudad { get; set; } = string.Empty;

    /// <summary>Fecha de la encuesta</summary>
    public DateTime FechaEncuesta { get; set; }

    /// <summary>Número de la encuesta</summary>
    public string NumeroEncuesta { get; set; } = string.Empty;

    /// <summary>ID del tipo de error</summary>
    public int IdTipoError { get; set; }

    /// <summary>Descripción del tipo de error</summary>
    public string TipoError { get; set; } = string.Empty;

    /// <summary>Severidad del error (1=Crítico, 2=Mayor, 3=Menor)</summary>
    public int SeveridadError { get; set; }

    /// <summary>Descripción de la severidad</summary>
    public string SeveridadTexto => SeveridadError switch
    {
        1 => "Crítico",
        2 => "Mayor",
        3 => "Menor",
        _ => "Desconocido"
    };

    /// <summary>Observaciones del error</summary>
    public string? Observaciones { get; set; }

    /// <summary>Acción correctiva tomada</summary>
    public string? AccionCorrectiva { get; set; }

    /// <summary>Fecha de registro</summary>
    public DateTime FechaRegistro { get; set; }

    /// <summary>Usuario que registró</summary>
    public int RegistradoPor { get; set; }

    /// <summary>Nombre del usuario que registró</summary>
    public string? NombreUsuarioRegistro { get; set; }

    /// <summary>Fecha de modificación</summary>
    public DateTime? FechaModificacion { get; set; }

    /// <summary>Usuario que modificó</summary>
    public int? ModificadoPor { get; set; }

    /// <summary>Estado del error (Pendiente/Corregido/Descartado)</summary>
    public string Estado { get; set; } = "Pendiente";

    /// <summary>Clase CSS para badge de severidad</summary>
    public string SeveridadClass => SeveridadError switch
    {
        1 => "danger",
        2 => "warning",
        3 => "info",
        _ => "secondary"
    };
}
