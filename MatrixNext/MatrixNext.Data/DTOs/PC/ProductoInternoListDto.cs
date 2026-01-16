namespace MatrixNext.Data.DTOs.PC
{
    /// <summary>
    /// DTO para listado de productos internos con información de joins
    /// </summary>
    public class ProductoInternoListDto
    {
        public int Id { get; set; }
        public int ProyectoId { get; set; }
        public string ProyectoNombre { get; set; } = string.Empty;
        public DateTime? FechaEnvio { get; set; }
        public int UnidadEnvia { get; set; }
        public string UnidadEnviaNombre { get; set; } = string.Empty;
        public int UnidadRecibe { get; set; }
        public string UnidadRecibeNombre { get; set; } = string.Empty;
        public int Tipo { get; set; }
        public string TipoNombre { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public int Envia { get; set; }
        public string EnviaNombre { get; set; } = string.Empty;
        public int? Recibe { get; set; }
        public string? RecibeNombre { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public string? Observaciones { get; set; }

        /// <summary>
        /// Estado calculado: Pendiente o Recibido
        /// </summary>
        public string Estado => FechaRecepcion.HasValue ? "Recibido" : "Pendiente";

        /// <summary>
        /// Clase CSS para badge de estado
        /// </summary>
        public string EstadoClass => FechaRecepcion.HasValue ? "success" : "warning";
    }
}
