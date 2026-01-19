namespace MatrixNext.Data.Modules.US.TipoGrupoUnidad.Models;

/// <summary>
/// DTO para Tipo de Grupo de Unidad
/// Ref: WebMatrix/US_Usuarios/TipoGrupoUnidad.aspx - Permiso 89
/// SP: US_TipoGrupoUnidad_Get, US_TipoGrupoUnidad_Add, US_TipoGrupoUnidad_Edit, US_TipoGrupoUnidad_Del
/// </summary>
public class TipoGrupoUnidadDto
{
    public int Id { get; set; }
    public string TipoGrupoUnidad { get; set; } = string.Empty;
}
