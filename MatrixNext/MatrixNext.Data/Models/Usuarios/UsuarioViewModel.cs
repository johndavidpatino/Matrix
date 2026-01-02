using System;
using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.Models.Usuarios;

/// <summary>
/// ViewModel para listado de usuarios
/// </summary>
public class UsuarioListViewModel
{
    public long Id { get; set; }
    
    [Display(Name = "Usuario")]
    public string NombreUsuario { get; set; }

    [Display(Name = "Nombre Completo")]
    public string NombreCompleto { get; set; }

    [Display(Name = "Email")]
    [EmailAddress]
    public string Email { get; set; }

    [Display(Name = "Activo")]
    public bool Activo { get; set; }

    [Display(Name = "Fecha Creación")]
    public DateTime? FechaCreacion { get; set; }
}

/// <summary>
/// ViewModel para crear/editar usuario
/// </summary>
public class UsuarioFormViewModel
{
    public long? Id { get; set; }

    [Required(ErrorMessage = "El usuario es requerido")]
    [Display(Name = "Usuario")]
    [StringLength(50)]
    public string NombreUsuario { get; set; }

    [Required(ErrorMessage = "Los nombres son requeridos")]
    [Display(Name = "Nombres")]
    [StringLength(100)]
    public string Nombres { get; set; }

    [Required(ErrorMessage = "Los apellidos son requeridos")]
    [Display(Name = "Apellidos")]
    [StringLength(100)]
    public string Apellidos { get; set; }

    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100)]
    public string Email { get; set; }

    [Display(Name = "Contraseña")]
    [DataType(DataType.Password)]
    [StringLength(255)]
    public string Password { get; set; }

    [Display(Name = "Confirmar Contraseña")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmPassword { get; set; }

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;

    public bool IsNew => Id == null || Id == 0;
}

/// <summary>
/// ViewModel para Rol (utilizado en detalle de usuario)
/// </summary>
public class RolViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
}

/// <summary>
/// ViewModel para Unidad (utilizado en detalle de usuario)
/// </summary>
public class UnidadViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
}

/// <summary>
/// ViewModel para detalle de usuario
/// </summary>
public class UsuarioDetailViewModel
{
    public int Id { get; set; }

    public string NombreUsuario { get; set; }

    public string Nombres { get; set; }

    public string Apellidos { get; set; }

    public string Email { get; set; }

    public bool Activo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string NombreCompleto => $"{Nombres} {Apellidos}";

    public List<RolViewModel> Roles { get; set; } = new List<RolViewModel>();

    public List<UnidadViewModel> Unidades { get; set; } = new List<UnidadViewModel>();
}
