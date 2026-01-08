using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Web.Models.OP
{
    /// <summary>
    /// ViewModel para listado de estimaciones por ciudad de un trabajo
    /// </summary>
    public class EstimacionCiudadListItemVM
    {
        public long Id { get; set; }
        public string? Ciudad { get; set; }
        public DateTime? FechaEstimacion { get; set; }
        public string? UsuarioNombre { get; set; }
        public string? Observaciones { get; set; }
        public bool Activa { get; set; }
        public bool Bloqueada { get; set; }
    }

    /// <summary>
    /// ViewModel para detalle de una estimación con planeación diaria
    /// </summary>
    public class EstimacionDetalleVM
    {
        public long IdEstimacion { get; set; }
        public long IdTrabajo { get; set; }
        public int CiudadId { get; set; }
        public string? CiudadNombre { get; set; }
        public DateTime? FechaEstimacion { get; set; }
        public string? Observaciones { get; set; }
        public bool Activa { get; set; }
        public bool Bloqueada { get; set; }
        
        /// <summary>
        /// Planeación diaria (fecha, cantidad estimada)
        /// </summary>
        public List<PlaneacionDiaVM> PlaneacionDias { get; set; } = new();
    }

    /// <summary>
    /// ViewModel para un día de planeación
    /// </summary>
    public class PlaneacionDiaVM
    {
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        [Range(0, 9999, ErrorMessage = "La cantidad debe estar entre 0 y 9999")]
        public short Cantidad { get; set; }
    }

    /// <summary>
    /// ViewModel para crear nueva estimación por ciudad
    /// </summary>
    public class CrearEstimacionVM
    {
        [Required(ErrorMessage = "El trabajo es requerido")]
        public long TrabajoId { get; set; }

        [Required(ErrorMessage = "La ciudad es requerida")]
        public int CiudadId { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string? Observaciones { get; set; }

        // Días de la semana incluidos en planeación
        public bool Lunes { get; set; } = true;
        public bool Martes { get; set; } = true;
        public bool Miercoles { get; set; } = true;
        public bool Jueves { get; set; } = true;
        public bool Viernes { get; set; } = true;
        public bool Sabado { get; set; }
        public bool Domingo { get; set; }
        
        /// <summary>
        /// Excluir días festivos de la planeación automática
        /// </summary>
        public bool ExcluirFestivos { get; set; } = true;
    }

    /// <summary>
    /// ViewModel para activar una estimación
    /// </summary>
    public class ActivarEstimacionVM
    {
        public long IdEstimacion { get; set; }
        public long IdTrabajo { get; set; }
    }
}
