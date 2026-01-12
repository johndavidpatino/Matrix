using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.Models.ViewModels.Pnc
{
    /// <summary>
    /// ViewModel maestro para Producto No Conforme (registro/edición)
    /// Origen: PNC_ProductoNoConforme
    /// ISO 9001 - Sistema de Gestión de Calidad
    /// </summary>
    public class ProductoNoConformeVM
    {
        public int Id { get; set; }

        [Display(Name = "Estudio")]
        public int? IdEstudio { get; set; }

        [Display(Name = "Trabajo")]
        public int? IdTrabajo { get; set; }

        [Required(ErrorMessage = "El JobBook es requerido")]
        [StringLength(15, ErrorMessage = "Máximo 15 caracteres")]
        [Display(Name = "JobBook")]
        public string JobBook { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha del reclamo es requerida")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Reclamo")]
        public DateTime FechaReclamo { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Debe seleccionar quién reporta")]
        [Display(Name = "Reporta")]
        public long IdReporta { get; set; }

        [Display(Name = "Unidad")]
        public int? IdUnidad { get; set; }

        [Display(Name = "Cliente Externo")]
        public long? IdClienteExterno { get; set; }

        [Required(ErrorMessage = "La fuente del reclamo es requerida")]
        [Display(Name = "Fuente Reclamo")]
        public int FuenteReclamo { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        [Display(Name = "Categoría")]
        public int Categoria { get; set; }

        [Display(Name = "Tarea")]
        public int? Tarea { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [Display(Name = "Descripción del Problema")]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "Cerrado")]
        public bool Cerrado { get; set; } = false;

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Cierre")]
        public DateTime? FechaCierre { get; set; }

        public long Usuario { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaGrabacion { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? FechaActualizacion { get; set; }

        // Navegación (calculada)
        public string? NombreEstudio { get; set; }
        public string? NombreReporta { get; set; }
        public string? NombreUnidad { get; set; }
        public string? NombreCliente { get; set; }
        public string? DescripcionFuenteReclamo { get; set; }
        public string? DescripcionCategoria { get; set; }
    }
}
