using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Web.Models.ViewModels.Pnc
{
    /// <summary>
    /// ViewModel para formulario de creación de PNC
    /// Incluye dropdown lists y validaciones
    /// </summary>
    public class CrearPncVM
    {
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

        // Causas a agregar en el mismo formulario (dinámico con JS)
        public List<string> Causas { get; set; } = new();

        // Datos lookup (autocompletes)
        public string? NombreEstudio { get; set; }
        public int? IdEstudio { get; set; }
        public int? IdTrabajo { get; set; }
        public int? IdUnidad { get; set; }
    }

    /// <summary>
    /// ViewModel para formulario de agregar causa a PNC existente
    /// </summary>
    public class AgregarCausaPncVM
    {
        [Required]
        public int IdPNC { get; set; }

        [Required(ErrorMessage = "La causa raíz es requerida")]
        [Display(Name = "Causa Raíz")]
        public string CausaRaiz { get; set; } = string.Empty;

        // Información del PNC para mostrar en el formulario
        public string? JobBook { get; set; }
        public string? DescripcionPNC { get; set; }
    }

    /// <summary>
    /// ViewModel para formulario de agregar acción a causa
    /// </summary>
    public class AgregarAccionPncVM
    {
        [Required]
        public int IdPNC { get; set; }

        [Required]
        public int IdCausa { get; set; }

        [Required(ErrorMessage = "El tipo de acción es requerido")]
        [Display(Name = "Tipo de Acción")]
        public int TipoAccion { get; set; }

        [Required(ErrorMessage = "La acción es requerida")]
        [Display(Name = "Acción a Ejecutar")]
        public string Accion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha planeada es requerida")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Planeada")]
        public DateTime FechaPlaneada { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el responsable de la acción")]
        [Display(Name = "Responsable Acción")]
        public int IdResponsableAccion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el responsable del seguimiento")]
        [Display(Name = "Responsable Seguimiento")]
        public int IdResponsableSeguimiento { get; set; }

        // Información del PNC y Causa para mostrar
        public string? JobBook { get; set; }
        public string? CausaRaiz { get; set; }
    }

    /// <summary>
    /// ViewModel para cerrar acción (registrar ejecución)
    /// </summary>
    public class CerrarAccionPncVM
    {
        [Required]
        public int IdAccion { get; set; }

        [Required(ErrorMessage = "La fecha de ejecución es requerida")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Ejecución")]
        public DateTime FechaEjecucion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La evidencia de cierre es requerida")]
        [Display(Name = "Evidencia de Cierre")]
        public string EvidenciaCierre { get; set; } = string.Empty;
    }
}
