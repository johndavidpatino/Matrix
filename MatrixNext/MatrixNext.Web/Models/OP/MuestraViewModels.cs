using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Web.Models.OP
{
    /// <summary>
    /// ViewModel para listado de muestra por ciudad de un trabajo
    /// </summary>
    public class MuestraCiudadListItemVM
    {
        public long Id { get; set; }
        public string? Departamento { get; set; }
        public string? Ciudad { get; set; }
        public int? CiudadId { get; set; }
        public double Cantidad { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? CoordinadorNombre { get; set; }
    }

    /// <summary>
    /// ViewModel para agregar/editar muestra de una ciudad
    /// </summary>
    public class MuestraCiudadVM
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "El trabajo es requerido")]
        public long TrabajoId { get; set; }

        [Required(ErrorMessage = "La ciudad es requerida")]
        public int CiudadId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, 999999, ErrorMessage = "La cantidad debe estar entre 1 y 999,999")]
        public double Cantidad { get; set; }

        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public long? CoordinadorId { get; set; }
    }

    /// <summary>
    /// ViewModel para actualizar fechas de muestra con auto-planeación
    /// </summary>
    public class ActualizarFechasMuestraVM
    {
        [Required]
        public long IdMuestra { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es requerida")]
        public DateTime FechaFin { get; set; }

        // Días de la semana incluidos en auto-planeación
        public bool Lunes { get; set; } = true;
        public bool Martes { get; set; } = true;
        public bool Miercoles { get; set; } = true;
        public bool Jueves { get; set; } = true;
        public bool Viernes { get; set; } = true;
        public bool Sabado { get; set; }
        public bool Domingo { get; set; }

        /// <summary>
        /// Excluir días festivos de la auto-planeación
        /// </summary>
        public bool ExcluirFestivos { get; set; } = true;
    }
}
