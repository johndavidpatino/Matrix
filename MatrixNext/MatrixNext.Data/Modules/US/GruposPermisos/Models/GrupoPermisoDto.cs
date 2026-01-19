namespace MatrixNext.Data.Modules.US.GruposPermisos.Models;

/// <summary>
/// DTO para Grupos de Permisos
/// Ref: CoreProject/GruposPermisos_Result + WebMatrix/US_Usuarios/GruposPermisos.aspx
/// </summary>
public class GrupoPermisoDto
{
    public int Id { get; set; }
    public string GrupoPermisos { get; set; } = string.Empty;
}

/// <summary>
/// DTO para crear/editar Grupo de Permisos
/// </summary>
public class GrupoPermisoInputDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}
