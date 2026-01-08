using System;

namespace MatrixNext.Web.Models.OP.Dtos
{
    /// <summary>
    /// DTO para representar un registro de actividad de producción.
    /// </summary>
    public class RegistroProduccionDto
    {
        /// <summary>ID del registro (si es edición)</summary>
        public int? IdRegistro { get; set; }

        /// <summary>ID de la unidad/área seleccionada</summary>
        public int UnidadId { get; set; }

        /// <summary>Nombre de la unidad para display</summary>
        public string UnidadNombre { get; set; }

        /// <summary>ID de la actividad seleccionada</summary>
        public int ActividadId { get; set; }

        /// <summary>Nombre de la actividad para display</summary>
        public string ActividadNombre { get; set; }

        /// <summary>ID de la subactividad seleccionada</summary>
        public int SubactividadId { get; set; }

        /// <summary>Nombre de la subactividad para display</summary>
        public string SubactividadNombre { get; set; }

        /// <summary>ID del JobBook seleccionado (JBE/JBI/CC)</summary>
        public int? JobBookId { get; set; }

        /// <summary>Código del JobBook (para display)</summary>
        public string JobBookCodigo { get; set; }

        /// <summary>Cantidad registrada (ej: 10 encuestas, 5 horas)</summary>
        public int Cantidad { get; set; }

        /// <summary>Hora de inicio (formato HH:mm)</summary>
        public string HoraInicio { get; set; }

        /// <summary>Hora de fin (formato HH:mm)</summary>
        public string HoraFin { get; set; }

        /// <summary>Fecha del registro (YYYY-MM-DD)</summary>
        public string Fecha { get; set; }

        /// <summary>Observaciones adicionales</summary>
        public string Observaciones { get; set; }

        /// <summary>ID del usuario que realiza el registro (del claim)</summary>
        public int UsuarioId { get; set; }

        /// <summary>Fecha/hora de creación del registro</summary>
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
