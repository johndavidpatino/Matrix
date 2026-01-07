using System;
using System.Collections.Generic;
using MatrixNext.Web.Models;

namespace MatrixNext.Web.Models.PY
{
    /// <summary>
    /// Entidad para registrar sesiones de recolección de datos en trabajos cualitativos.
    /// Una sesión puede ser: Focus Group, Sesión de Entrevistas, In-Home Visit, etc.
    /// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 1.1 (Sesiones.aspx.vb)
    /// </summary>
    public class SesionesCuali : BaseEntity
    {
        /// <summary>FK a TrabajosCuali</summary>
        public long IdTrabajoCuali { get; set; }

        /// <summary>FK a SegmentosCuali (si la sesión es para un segmento específico)</summary>
        public long? IdSegmento { get; set; }

        /// <summary>Nombre o descripción de la sesión</summary>
        public string Nombre { get; set; }

        /// <summary>Tipo de sesión: Focus Group, Entrevista In-Depth, IDH, Grupo de Discusión</summary>
        public string Tipo { get; set; }

        /// <summary>Fecha programada de la sesión</summary>
        public DateTime FechaProgramada { get; set; }

        /// <summary>Fecha en que se realizó la sesión (real)</summary>
        public DateTime? FechaEjecucion { get; set; }

        /// <summary>Hora de inicio programada (formato HH:mm)</summary>
        public string HoraInicio { get; set; }

        /// <summary>Hora de fin programada (formato HH:mm)</summary>
        public string HoraFin { get; set; }

        /// <summary>Duración estimada en minutos</summary>
        public int? DuracionEstimada { get; set; }

        /// <summary>Duración real en minutos</summary>
        public int? DuracionReal { get; set; }

        /// <summary>Ubicación física de la sesión (dirección, sala, etc.)</summary>
        public string Ubicacion { get; set; }

        /// <summary>Moderador o facilitador de la sesión</summary>
        public string Moderador { get; set; }

        /// <summary>Número de participantes planeados</summary>
        public int? NumeroParticipantesPlaneado { get; set; }

        /// <summary>Número de participantes que asistieron</summary>
        public int? NumeroParticipantesReal { get; set; }

        /// <summary>Estado: Planeada, Ejecutada, Cancelada, Reprogramada</summary>
        public string Estado { get; set; } = "Planeada";

        /// <summary>Observaciones sobre la ejecución de la sesión</summary>
        public string Observaciones { get; set; }

        /// <summary>URL o ruta del archivo de grabación (audio/video)</summary>
        public string UrlGrabacion { get; set; }

        /// <summary>Notas adicionales</summary>
        public string Notas { get; set; }

        /// <summary>Indica si la sesión está activa</summary>
        public bool Activo { get; set; } = true;

        // Navegación
        /// <summary>Trabajo cualitativo al que pertenece la sesión</summary>
        public virtual TrabajosCuali TrabajoCuali { get; set; }

        /// <summary>Segmento específico de la sesión (si aplica)</summary>
        public virtual SegmentosCuali Segmento { get; set; }

        /// <summary>Participantes en esta sesión</summary>
        public virtual ICollection<ParticipantesSesion> Participantes { get; set; } = new List<ParticipantesSesion>();
    }
}
