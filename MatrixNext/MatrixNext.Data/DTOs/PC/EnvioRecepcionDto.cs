using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.PC
{
    /// <summary>
    /// DTO para flujo de envío/recepción de productos
    /// </summary>
    public class EnvioRecepcionDto
    {
        public int Id { get; set; }

        [Display(Name = "Producto")]
        public string Producto { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; }

        [Display(Name = "Unidad Origen")]
        public string UnidadOrigen { get; set; } = string.Empty;

        [Display(Name = "Unidad Destino")]
        public string UnidadDestino { get; set; } = string.Empty;

        [Display(Name = "Fecha Envío")]
        public DateTime? FechaEnvio { get; set; }

        [Display(Name = "Fecha Recepción")]
        public DateTime? FechaRecepcion { get; set; }

        [Required(ErrorMessage = "Las observaciones son requeridas para recepción")]
        [StringLength(1000, ErrorMessage = "Las observaciones no pueden exceder 1000 caracteres")]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        /// <summary>
        /// Usuario que recibe (solo para recepción)
        /// </summary>
        public int? RecibeUsuarioId { get; set; }
    }
}
