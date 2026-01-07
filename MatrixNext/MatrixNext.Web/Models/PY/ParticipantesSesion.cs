using System;
using MatrixNext.Web.Models;

namespace MatrixNext.Web.Models.PY
{
    /// <summary>
    /// Entidad intermedia que relaciona participantes (muestras) con sesiones.
    /// Permite registrar la asistencia de un participante a una sesión específica.
    /// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 1.1 (SesionesCuali.aspx.vb)
    /// </summary>
    public class ParticipantesSesion : BaseEntity
    {
        /// <summary>FK a SesionesCuali</summary>
        public long IdSesion { get; set; }

        /// <summary>FK a MuestrasCuali</summary>
        public long IdMuestra { get; set; }

        /// <summary>Asistencia: Asistió, No Asistió, Canceló, Reprogramó</summary>
        public string Asistencia { get; set; }

        /// <summary>Hora de llegada real</summary>
        public DateTime? HoraLlegada { get; set; }

        /// <summary>Hora de salida real</summary>
        public DateTime? HoraSalida { get; set; }

        /// <summary>Observaciones sobre el participante durante la sesión</summary>
        public string Observaciones { get; set; }

        /// <summary>Calidad de respuestas del participante: Excelente, Buena, Regular, Pobre</summary>
        public string CalidadRespuestas { get; set; }

        /// <summary>Motivo si no asistió</summary>
        public string MotivoInasistencia { get; set; }

        /// <summary>Indica si está activo el registro</summary>
        public bool Activo { get; set; } = true;

        // Navegación
        /// <summary>Sesión en la que participó</summary>
        public virtual SesionesCuali Sesion { get; set; }

        /// <summary>Muestra/Participante</summary>
        public virtual MuestrasCuali Muestra { get; set; }
    }
}
