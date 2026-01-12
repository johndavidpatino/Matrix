using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.Models.ViewModels.Pnc
{
    /// <summary>
    /// ViewModel para acciones correctivas/preventivas del PNC
    /// Origen: PNC_ProductoNoConformeAcciones
    /// Relación: 1 Causa → N Acciones
    /// Tipos: 1=Inmediata (OBLIGATORIA), 2=Correctiva, 3=Preventiva
    /// </summary>
    public class ProductoNoConformeAccionVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El IdPNC es requerido")]
        [Display(Name = "PNC")]
        public int IdPNC { get; set; }

        [Required(ErrorMessage = "El IdCausa es requerido")]
        [Display(Name = "Causa")]
        public int IdCausa { get; set; }

        [Required(ErrorMessage = "El tipo de acción es requerido")]
        [Display(Name = "Tipo de Acción")]
        public int TipoAccion { get; set; }

        [Required(ErrorMessage = "La acción es requerida")]
        [Display(Name = "Acción a Ejecutar")]
        public string Accion { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Planeada")]
        public DateTime? FechaPlaneada { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Ejecución")]
        public DateTime? FechaEjecucion { get; set; }

        [Display(Name = "Responsable Acción")]
        public int? IdResponsableAccion { get; set; }

        [Display(Name = "Responsable Seguimiento")]
        public int? IdResponsableSeguimiento { get; set; }

        [Display(Name = "Evidencia de Cierre")]
        public string? EvidenciaCierre { get; set; }

        [Display(Name = "Permite Actualizar")]
        public bool? PermiteActualizar { get; set; }

        // Navegación
        public ProductoNoConformeCausaVM? Causa { get; set; }
        public string? NombreTipoAccion { get; set; }
        public string? NombreResponsableAccion { get; set; }
        public string? NombreResponsableSeguimiento { get; set; }

        // Propiedades calculadas
        public bool EstaVencida => FechaPlaneada.HasValue 
            && FechaPlaneada.Value < DateTime.Now 
            && !FechaEjecucion.HasValue;

        public bool EstaProximaAVencer => FechaPlaneada.HasValue 
            && (FechaPlaneada.Value - DateTime.Now).Days <= 3 
            && !FechaEjecucion.HasValue;

        public bool EstaEjecutada => FechaEjecucion.HasValue;
    }
}
