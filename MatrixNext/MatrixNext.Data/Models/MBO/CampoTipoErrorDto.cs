namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para catálogo de tipos de error de campo
/// Mapea resultados del SP: MBO_CampoTiposErrorGet
/// </summary>
public class CampoTipoErrorDto
{
    /// <summary>ID del tipo de error</summary>
    public int IdTipoError { get; set; }

    /// <summary>Código del tipo de error</summary>
    public string Codigo { get; set; } = string.Empty;

    /// <summary>Descripción del tipo de error</summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>Severidad del error (1=Crítico, 2=Mayor, 3=Menor)</summary>
    public int Severidad { get; set; }

    /// <summary>Activo</summary>
    public bool Activo { get; set; }

    /// <summary>Descripción de la severidad</summary>
    public string SeveridadTexto => Severidad switch
    {
        1 => "Crítico",
        2 => "Mayor",
        3 => "Menor",
        _ => "Desconocido"
    };
}
