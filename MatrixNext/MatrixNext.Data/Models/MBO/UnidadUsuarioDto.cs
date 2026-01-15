namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para unidades asociadas a un usuario
/// Mapea resultado del SP MBO_ObtenerUnidadesUsuario
/// </summary>
public class UnidadUsuarioDto
{
    public int IdUnidad { get; set; }
    public string Sigla { get; set; } = string.Empty;
    public string NombreUnidad { get; set; } = string.Empty;
}
