using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Web.Models.ViewModels.Pnc
{
    /// <summary>
    /// ViewModel para datos de notificación de PNC por email
    /// Usado por IEmailQueueService para enviar notificaciones
    /// </summary>
    public class PncNotificacionVM
    {
        [Required]
        public int IdPNC { get; set; }

        [Required]
        public string JobBook { get; set; } = string.Empty;

        public string? NombreEstudio { get; set; }

        [Required]
        public TipoNotificacionPncEnum TipoNotificacion { get; set; }

        [Required]
        public List<string> EmailsDestinatarios { get; set; } = new();

        public List<string> EmailsCopia { get; set; } = new();

        public string Asunto { get; set; } = string.Empty;

        public string CuerpoMensaje { get; set; } = string.Empty;

        // Datos adicionales para el template
        public string? DescripcionPNC { get; set; }
        public DateTime? FechaReclamo { get; set; }
        public string? NombreReporta { get; set; }
        public string? CausaRaiz { get; set; }
        public string? AccionDescripcion { get; set; }
        public DateTime? FechaPlaneada { get; set; }
        public string? NombreResponsable { get; set; }
    }

    /// <summary>
    /// Tipos de notificación para PNC
    /// </summary>
    public enum TipoNotificacionPncEnum
    {
        [Display(Name = "Nuevo PNC Registrado")]
        NuevoPNC = 1,

        [Display(Name = "Causa Registrada")]
        CausaRegistrada = 2,

        [Display(Name = "Acción Asignada")]
        AccionAsignada = 3,

        [Display(Name = "Acción Próxima a Vencer")]
        AccionProximaVencer = 4,

        [Display(Name = "Acción Vencida")]
        AccionVencida = 5,

        [Display(Name = "PNC Cerrado")]
        PNCCerrado = 6
    }
}
