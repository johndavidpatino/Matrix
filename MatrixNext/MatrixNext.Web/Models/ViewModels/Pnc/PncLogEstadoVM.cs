using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Web.Models.ViewModels.Pnc
{
    /// <summary>
    /// ViewModel para log de cambios de estado del PNC
    /// Origen: PNC_Productos_Log (sistema avanzado)
    /// Auditoría de cambios
    /// </summary>
    public class PncLogEstadoVM
    {
        public int Id { get; set; }

        [Display(Name = "PNC")]
        public int IdProducto { get; set; }

        [Display(Name = "Estado Anterior")]
        public string? EstadoAnterior { get; set; }

        [Display(Name = "Estado Nuevo")]
        public string? EstadoNuevo { get; set; }

        [Display(Name = "Fecha Cambio")]
        [DataType(DataType.DateTime)]
        public DateTime FechaCambio { get; set; }

        [Display(Name = "Usuario")]
        public long IdUsuario { get; set; }

        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        // Navegación
        public string? NombreUsuario { get; set; }
    }

    /// <summary>
    /// Estados del PNC (del enum legacy EEstados)
    /// </summary>
    public enum EstadoPncLegacyEnum
    {
        [Display(Name = "Enviado")]
        Enviado = 1,

        [Display(Name = "Actualizado")]
        Actualizado = 2,

        [Display(Name = "Anulado")]
        Anulado = 3,

        [Display(Name = "Eliminado")]
        Eliminado = 4,

        [Display(Name = "Aceptado")]
        Aceptado = 5,

        [Display(Name = "Rechazado")]
        Rechazado = 6,

        [Display(Name = "Causa Registrada")]
        CausaRegistrada = 7
    }
}
