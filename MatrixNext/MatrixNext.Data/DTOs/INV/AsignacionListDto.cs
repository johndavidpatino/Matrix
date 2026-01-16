namespace MatrixNext.Data.DTOs.INV
{
    /// <summary>
    /// DTO para mostrar asignaciones en listados/grids con datos denormalizados.
    /// </summary>
    public class AsignacionListDto
    {
        public long Id { get; set; }
        public long IdActivoFijo { get; set; }
        public string Articulo { get; set; } = string.Empty;
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Serial { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public string? JobBookCodigo { get; set; }
        public string? JobBookNombre { get; set; }
        public string? Ciudad { get; set; }
        public string? EstadoTablet { get; set; }
        public long IdUsuarioAsignado { get; set; }
        public string UsuarioAsignado { get; set; } = string.Empty;
        public string? Cargo { get; set; }
        public string? Observacion { get; set; }
        public string? Sede { get; set; }
        public string? GrupoUnidad { get; set; }
        public string? Unidad { get; set; }
        public long UsuarioRegistra { get; set; }
        public string? UsuarioRegistraNombre { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string? PlacaActivo { get; set; } // Placa del activo
        public string? TipoArticulo { get; set; } // Tipo del artículo
        public string? NombreUsuario { get; set; } // Nombre del usuario asignado
        public long IdAsignacion { get; set; } // ID de la asignación
        public DateTime? FechaDevolucion { get; set; } // Fecha cuando fue devuelto (null si activo)
        public string? Observaciones { get; set; }

        // Propiedades calculadas para UI
        public string EstadoTabletClass => EstadoTablet?.ToLower() switch
        {
            "activa" => "success",
            "inactiva" => "danger",
            "en mantenimiento" => "warning",
            _ => "secondary"
        };
    }
}
