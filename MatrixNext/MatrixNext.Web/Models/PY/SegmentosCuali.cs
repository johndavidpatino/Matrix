using System;
using System.Collections.Generic;
using MatrixNext.Web.Models;

namespace MatrixNext.Web.Models.PY
{
    /// <summary>
    /// Entidad para representar segmentos de población en trabajos cualitativos.
    /// Ejemplo: Edad 18-30, Estrato 3-4, Usuarios de telefonía celular
    /// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 1.1 (SegmentosCuali.aspx.vb)
    /// </summary>
    public class SegmentosCuali : BaseEntity
    {
        /// <summary>FK a TrabajosCuali</summary>
        public long IdTrabajoCuali { get; set; }

        /// <summary>Nombre descriptivo del segmento</summary>
        public string? Nombre { get; set; }

        /// <summary>Descripción detallada de características del segmento</summary>
        public string? Descripcion { get; set; }

        /// <summary>Número de participantes a entrevistar en este segmento</summary>
        public int NumeroParticipantes { get; set; }

        /// <summary>Cuota mínima (por lógica de muestreo)</summary>
        public int? CuotaMinima { get; set; }

        /// <summary>Cuota máxima (por lógica de muestreo)</summary>
        public int? CuotaMaxima { get; set; }

        /// <summary>Descripción de criterios de inclusión</summary>
        public string? CriteriosInclusion { get; set; }

        /// <summary>Descripción de criterios de exclusión</summary>
        public string? CriteriosExclusion { get; set; }

        /// <summary>Notas sobre el segmento</summary>
        public string? Notas { get; set; }

        /// <summary>Orden de presentación/importancia del segmento</summary>
        public int? Orden { get; set; }

        // Navegación
        /// <summary>Trabajo cualitativo al que pertenece</summary>
        public virtual TrabajosCuali? TrabajoCuali { get; set; }

        /// <summary>Muestras asociadas a este segmento</summary>
        public virtual ICollection<MuestrasCuali> Muestras { get; set; } = new List<MuestrasCuali>();

        /// <summary>Entrevistadores asignados a este segmento</summary>
        public virtual ICollection<EntrevistadorasCuali> Entrevistadores { get; set; } = new List<EntrevistadorasCuali>();
    }
}
