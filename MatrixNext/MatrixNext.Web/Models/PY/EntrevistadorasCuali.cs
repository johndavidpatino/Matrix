using System;
using System.Collections.Generic;
using MatrixNext.Web.Models;

namespace MatrixNext.Web.Models.PY
{
    /// <summary>
    /// Entidad para registrar entrevistadores/moderadores asignados a trabajos cualitativos.
    /// Un entrevistador puede estar asignado a múltiples segmentos/trabajos.
    /// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 1.1 (coordinación de entrevistadores)
    /// </summary>
    public class EntrevistadorasCuali : BaseEntity
    {
        /// <summary>FK a TrabajosCuali (trabajo al que está asignado)</summary>
        public long IdTrabajoCuali { get; set; }

        /// <summary>FK a SegmentosCuali (segmento específico, si aplica)</summary>
        public long? IdSegmento { get; set; }

        /// <summary>FK a tabla de usuarios (quien hace la entrevista)</summary>
        public long IdUsuario { get; set; }

        /// <summary>Nombre del entrevistador (denormalizado para reportes)</summary>
        public string? NombreCompleto { get; set; }

        /// <summary>Teléfono de contacto del entrevistador</summary>
        public string? Telefono { get; set; }

        /// <summary>Email de contacto del entrevistador</summary>
        public string? Email { get; set; }

        /// <summary>Especialidad o entrenamiento: Moderador, Entrevistador In-Depth, Entrevistador Grupal</summary>
        public string? Especialidad { get; set; }

        /// <summary>Número de entrevistas asignadas</summary>
        public int NumeroEntrevistasAsignadas { get; set; } = 0;

        /// <summary>Número de entrevistas completadas</summary>
        public int NumeroEntrevistasCompletadas { get; set; } = 0;

        /// <summary>Porcentaje de cumplimiento estimado</summary>
        public decimal? PorcentajeCumplimiento { get; set; }

        /// <summary>Fecha de inicio de asignación</summary>
        public DateTime FechaAsignacion { get; set; }

        /// <summary>Fecha estimada de término</summary>
        public DateTime? FechaTermino { get; set; }

        /// <summary>Estado de la asignación: Asignado, En Ejecución, Completado, Cancelado</summary>
        public string? Estado { get; set; } = "Asignado";

        /// <summary>Nivel de experiencia: Junior, Senior, Experto</summary>
        public string? NivelExperiencia { get; set; }

        /// <summary>Disponibilidad: Disponible, Ocupado, No Disponible</summary>
        public string? Disponibilidad { get; set; } = "Disponible";

        /// <summary>Notas o restricciones para este entrevistador</summary>
        public string? Notas { get; set; }

        // Navegación
        /// <summary>Trabajo cualitativo al que está asignado</summary>
        public virtual TrabajosCuali? TrabajoCuali { get; set; }

        /// <summary>Segmento específico (si aplica)</summary>
        public virtual SegmentosCuali? Segmento { get; set; }

        /// <summary>Muestras/entrevistas realizadas por este entrevistador</summary>
        public virtual ICollection<MuestrasCuali> Muestras { get; set; } = new List<MuestrasCuali>();
    }
}
