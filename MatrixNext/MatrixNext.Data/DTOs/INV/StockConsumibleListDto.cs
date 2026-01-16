namespace MatrixNext.Data.DTOs.INV
{
    /// <summary>
    /// DTO para mostrar stock de consumibles en listados/grids.
    /// </summary>
    public class StockConsumibleListDto
    {
        public long Id { get; set; }
        public long IdConsumible { get; set; }
        public string Articulo { get; set; } = string.Empty;
        public string? TipoProducto { get; set; }
        public string? Producto { get; set; }
        public long? NumeroVale { get; set; }
        public DateTime Fecha { get; set; }
        public short TipoMovimiento { get; set; }
        public string TipoMovimientoNombre { get; set; } = string.Empty;
        public short? Estado { get; set; }
        public string? EstadoNombre { get; set; }
        public string? JobBookCodigo { get; set; }
        public string? JobBookNombre { get; set; }
        public string? Ciudad { get; set; }
        public long? Valor { get; set; }
        public long? Total { get; set; }
        public long? Disponible { get; set; }
        public long? IdUsuarioAsignado { get; set; }
        public string? UsuarioAsignado { get; set; }
        public string? Cargo { get; set; }
        public bool Legalizado { get; set; }
        public string? Observaciones { get; set; }
        public long UsuarioRegistra { get; set; }
        public string? UsuarioRegistraNombre { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string? NombreConsumible { get; set; } // Nombre del consumible
        public long? Cantidad { get; set; } // Cantidad del movimiento
        public long IdMovimiento { get; set; } // ID único del movimiento
        public string? NombreUsuario { get; set; } // Nombre del usuario que registra

        // Propiedades calculadas para UI
        public string TipoMovimientoClass => TipoMovimiento == 1 ? "success" : "warning"; // 1=Entrada, 2=Salida
        public string LegalizadoTexto => Legalizado ? "Sí" : "No";
        public string LegalizadoClass => Legalizado ? "success" : "warning";
    }
}
