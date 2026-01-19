namespace MatrixNext.Data.Modules.US.Unidades.Models;

/// <summary>
/// DTO para Unidades
/// Ref: WebMatrix/US_Usuarios/Unidades.aspx
/// SP: US_Unidades_Get, US_Unidades_Add, US_Unidades_Edit, US_Unidades_Del
/// </summary>
public class UnidadDto
{
    public int Id { get; set; }
    public string Unidad { get; set; } = string.Empty;
    public int? IdGrupoUnidad { get; set; }
    public string? GrupoUnidadNombre { get; set; }
}
