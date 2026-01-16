namespace MatrixNext.Data.DTOs.INV
{
    /// <summary>
    /// DTO para mostrar artículos en listados/grids con datos denormalizados.
    /// </summary>
    public class RegistroArticuloListDto
    {
        public long Id { get; set; }
        public long IdTipoArticulo { get; set; }
        public string TipoArticulo { get; set; } = string.Empty;
        public long IdArticulo { get; set; }
        public string? Articulo { get; set; } = string.Empty;
        public DateTime? FechaCompra { get; set; }
        public string? JobBookCodigo { get; set; }
        public string? JobBookNombre { get; set; }
        public long? ValorUnitario { get; set; }
        public long? IdEstado { get; set; }
        public string? Estado { get; set; }
        public string? Descripcion { get; set; }
        public string? Symphony { get; set; }
        public long? IdFisico { get; set; }
        public string? Sede { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Serial { get; set; }
        public string? NombreEquipo { get; set; }
        public bool Asignado { get; set; }
        public long? Cantidad { get; set; }
        public long? IdUsuarioAsignado { get; set; }
        public string? UsuarioAsignado { get; set; }
        public string? Placa { get; set; } // Placa del activo
        public DateTime? FechaModificacion { get; set; }

        // Propiedades calculadas para UI
        public string AsignadoTexto => Asignado ? "Sí" : "No";
        public string AsignadoClass => Asignado ? "success" : "warning";
        public string EstadoClass => IdEstado switch
        {
            1 => "success",  // Activo
            2 => "warning",  // En mantenimiento
            3 => "danger",   // Inactivo
            4 => "info",     // En renta
            _ => "secondary"
        };
    }
}
