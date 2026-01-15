using System;
using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.Models.OP
{
    /// <summary>
    /// DTO para planillas cuantitativas con aprobación/rechazo
    /// Mapea a resultado del SP: OP_CuantiPlanillas_GET
    /// </summary>
    public class PlanillaAprobacionDto
    {
        [Display(Name = "ID")]
        public long Id { get; set; }

        [Display(Name = "Trabajo")]
        public long TrabajoId { get; set; }

        [Display(Name = "Nombre Trabajo")]
        public string? NombreTrabajo { get; set; }

        [Display(Name = "Fecha Ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Display(Name = "Moneda")]
        public string? Moneda { get; set; }

        [Display(Name = "Monto")]
        [DataType(DataType.Currency)]
        public decimal Monto { get; set; }

        [Display(Name = "Moneda Planilla")]
        public string? MonedaPlanilla { get; set; }

        [Display(Name = "Monto Planilla")]
        [DataType(DataType.Currency)]
        public decimal MontoPlanilla { get; set; }

        [Display(Name = "Monto Autorizado")]
        [DataType(DataType.Currency)]
        public decimal MontoAutorizado { get; set; }

        [Display(Name = "Usuario")]
        public long? UsuarioId { get; set; }

        [Display(Name = "Coordinador")]
        public string? NombreCoordinador { get; set; }

        [Display(Name = "Estado")]
        public string? Estado { get; set; }

        [Display(Name = "Revisado")]
        public bool Revisado { get; set; }

        [Display(Name = "PMO")]
        public long? PMO { get; set; }

        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        // Información de auditoría
        [Display(Name = "Registrado Por")]
        public string? RegistradoPor { get; set; }

        [Display(Name = "Fecha Registro")]
        public DateTime? FechaRegistro { get; set; }

        [Display(Name = "Modificado Por")]
        public string? ModificadoPor { get; set; }

        [Display(Name = "Fecha Modificación")]
        public DateTime? FechaModificacion { get; set; }
    }

    /// <summary>
    /// DTO para aprobación de planilla
    /// </summary>
    public class AprobacionPlanillaDto
    {
        [Required(ErrorMessage = "El ID de la planilla es requerido")]
        public long PlanillaId { get; set; }

        public long IdPlanilla
        {
            get => PlanillaId;
            set => PlanillaId = value;
        }

        [Required(ErrorMessage = "El monto autorizado es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        [Display(Name = "Monto Autorizado")]
        [DataType(DataType.Currency)]
        public decimal MontoAutorizado { get; set; }

        public long AprobadoPor { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para rechazo de planilla
    /// </summary>
    public class RechazoPlanillaDto
    {
        [Required(ErrorMessage = "El ID de la planilla es requerido")]
        public long PlanillaId { get; set; }

        [Required(ErrorMessage = "El motivo del rechazo es requerido")]
        [StringLength(500, ErrorMessage = "El motivo no puede exceder 500 caracteres")]
        [Display(Name = "Motivo del Rechazo")]
        public string Motivo { get; set; } = string.Empty;
    }
}
