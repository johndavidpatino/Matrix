namespace MatrixNext.Data.Modules.US.RolesPermisos.Models;

/// <summary>
/// DTO para RolPermiso (tabla US_RolesPermisos)
/// </summary>
public class RolPermisoDto
{
    public int Id { get; set; }
    public int PermisoId { get; set; }
    public int RolId { get; set; }
    
    // Propiedades de navegación para display
    public string? NombreRol { get; set; }
    public string? NombrePermiso { get; set; }
}

/// <summary>
/// DTO para crear asignación rol-permiso
/// </summary>
public class RolPermisoCreateDto
{
    public int PermisoId { get; set; }
    public int RolId { get; set; }
}

/// <summary>
/// DTO para combo de Roles
/// </summary>
public class RolComboDto
{
    public int Id { get; set; }
    public string Rol { get; set; } = string.Empty;
}

/// <summary>
/// DTO para combo de Permisos
/// </summary>
public class PermisoComboDto
{
    public int Id { get; set; }
    public string Permiso { get; set; } = string.Empty;
}
