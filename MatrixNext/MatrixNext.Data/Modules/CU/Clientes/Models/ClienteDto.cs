using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.Modules.CU.Clientes.Models;

/// <summary>
/// DTO principal de Cliente
/// </summary>
public class ClienteDto
{
    public long Id { get; set; }
    public decimal? Nit { get; set; }
    public string? GrupoEconomico { get; set; }
    public string RazonSocial { get; set; } = string.Empty;
    public int? IdCiudad { get; set; }
    public string? CiudadNombre { get; set; }
    public string? DepartamentoNombre { get; set; }
    public string? PaisNombre { get; set; }
    public string? Apodo { get; set; }
    public int? IdTipoCliente { get; set; }
    public string? TipoClienteNombre { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? IdSector { get; set; }
    public string? SectorNombre { get; set; }
    public short Anticipo { get; set; }
    public short Saldo { get; set; }
    public short Plazo { get; set; }
}

/// <summary>
/// DTO para crear/editar cliente
/// </summary>
public class ClienteCreateEditDto
{
    public long? Id { get; set; }

    [Required(ErrorMessage = "El NIT es requerido")]
    public decimal Nit { get; set; }

    public string? GrupoEconomico { get; set; }

    [Required(ErrorMessage = "La razón social es requerida")]
    [StringLength(200, ErrorMessage = "La razón social no puede exceder 200 caracteres")]
    public string RazonSocial { get; set; } = string.Empty;

    public int? IdCiudad { get; set; }
    
    public string? Apodo { get; set; }

    public int? IdTipoCliente { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? IdSector { get; set; }

    [Range(0, 100, ErrorMessage = "El anticipo debe estar entre 0 y 100")]
    public short Anticipo { get; set; } = 50;

    [Range(0, 100, ErrorMessage = "El saldo debe estar entre 0 y 100")]
    public short Saldo { get; set; } = 50;

    [Range(0, 365, ErrorMessage = "El plazo debe estar entre 0 y 365 días")]
    public short Plazo { get; set; } = 30;
}

/// <summary>
/// Parámetros de búsqueda de clientes
/// </summary>
public class ClienteBusquedaParams
{
    public string? Buscar { get; set; }
    public int? IdCiudad { get; set; }
    public string? IdSector { get; set; }
    public int? IdTipoCliente { get; set; }
}

/// <summary>
/// DTO principal de Contacto
/// </summary>
public class ContactoDto
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Celular { get; set; }
    public string? Email { get; set; }
    public string? Cargo { get; set; }
    public bool Activo { get; set; }
    public long IdCliente { get; set; }
    public string? ClienteNombre { get; set; }
}

/// <summary>
/// DTO para crear/editar contacto
/// </summary>
public class ContactoCreateEditDto
{
    public long? Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public string? Celular { get; set; }

    [EmailAddress(ErrorMessage = "El email no es válido")]
    public string? Email { get; set; }

    public string? Cargo { get; set; }

    public bool Activo { get; set; } = true;

    [Required(ErrorMessage = "El cliente es requerido")]
    public long IdCliente { get; set; }
}

/// <summary>
/// DTO para catálogos geográficos
/// </summary>
public class PaisDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class DepartamentoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int IdPais { get; set; }
}

public class CiudadDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int IdDepartamento { get; set; }
}

public class SectorDto
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
}

public class TipoClienteDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}
