using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.SGC
{
    /// <summary>
    /// DTO para Plan de Acción
    /// </summary>
    public class SGCPlanAccionDto
    {
        public int PlanAccionId { get; set; }
        public int AccionMejoraId { get; set; }

        [Required(ErrorMessage = "La descripción del plan es requerida")]
        [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
        public string DescripcionPlan { get; set; }

        public DateTime FechaPlaneado { get; set; }
        public DateTime? FechaEjecutado { get; set; }

        /// <summary>
        /// Porcentaje o evaluación de eficacia: 0-100% o "Alto", "Medio", "Bajo"
        /// </summary>
        [StringLength(50)]
        public string EficaciaPlan { get; set; }

        public DateTime? FechaRevision { get; set; }

        public int DiosRestantes => CalcularDiasRestantes();

        private int CalcularDiasRestantes()
        {
            if (FechaEjecutado.HasValue)
                return 0;
            
            return (int)(FechaPlaneado - DateTime.Now).TotalDays;
        }
    }

    /// <summary>
    /// DTO para crear plan de acción
    /// </summary>
    public class SGCPlanAccionCreateDto
    {
        [Required(ErrorMessage = "La descripción del plan es requerida")]
        [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
        public string DescripcionPlan { get; set; }

        [Required(ErrorMessage = "La fecha planeada es requerida")]
        public DateTime FechaPlaneado { get; set; }
    }

    /// <summary>
    /// DTO para actualizar plan de acción
    /// </summary>
    public class SGCPlanAccionUpdateDto
    {
        public int PlanAccionId { get; set; }

        [Required(ErrorMessage = "La descripción del plan es requerida")]
        [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
        public string DescripcionPlan { get; set; }

        [Required(ErrorMessage = "La fecha planeada es requerida")]
        public DateTime FechaPlaneado { get; set; }

        public DateTime? FechaEjecutado { get; set; }

        [StringLength(50)]
        public string EficaciaPlan { get; set; }

        public DateTime? FechaRevision { get; set; }
    }
}
