using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.PC
{
    /// <summary>
    /// DTO para entrada/edición de productos internos
    /// </summary>
    public class ProductoInternoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El proyecto es requerido")]
        [Display(Name = "Proyecto")]
        public int ProyectoId { get; set; }

        [Display(Name = "Fecha Envío")]
        public DateTime? FechaEnvio { get; set; }

        [Required(ErrorMessage = "La unidad que envía es requerida")]
        [Display(Name = "Unidad Envía")]
        public int UnidadEnvia { get; set; }

        [Required(ErrorMessage = "La unidad que recibe es requerida")]
        [Display(Name = "Unidad Recibe")]
        public int UnidadRecibe { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es requerido")]
        [Display(Name = "Tipo Movimiento")]
        public int Tipo { get; set; }

        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [StringLength(200, ErrorMessage = "El nombre del producto no puede exceder 200 caracteres")]
        [Display(Name = "Producto")]
        public string Producto { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(0.01, 999999.99, ErrorMessage = "La cantidad debe ser mayor a 0")]
        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; }

        [Required(ErrorMessage = "El usuario que envía es requerido")]
        [Display(Name = "Envía")]
        public int Envia { get; set; }

        [Display(Name = "Recibe")]
        public int? Recibe { get; set; }

        [Display(Name = "Fecha Recepción")]
        public DateTime? FechaRecepcion { get; set; }

        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }
    }
}
