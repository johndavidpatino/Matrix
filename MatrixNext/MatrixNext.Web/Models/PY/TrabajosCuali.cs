using System;
using System.Collections.Generic;
using MatrixNext.Web.Models;

namespace MatrixNext.Web.Models.PY
{
    /// <summary>
    /// Entidad para representar trabajos cualitativos dentro de un proyecto.
    /// Heredada de Sprint 2: PY_Trabajos con especialización para estudios cualitativos.
    /// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 1.4 (TrabajosCualitativos.aspx.vb)
    /// </summary>
    public class TrabajosCuali : BaseEntity
    {
        /// <summary>FK a PY_Proyectos</summary>
        public long IdProyecto { get; set; }

        /// <summary>FK a PY_Trabajos (relación al trabajo cuantitativo principal, si aplica)</summary>
        public long? IdTrabajoRelacionado { get; set; }

        /// <summary>Nombre del trabajo cualitativo</summary>
        public string Nombre { get; set; }

        /// <summary>Descripción del trabajo</summary>
        public string Descripcion { get; set; }

        /// <summary>Estado del trabajo: Creado, En Ejecución, Completado, Cancelado</summary>
        public string Estado { get; set; } = "Creado";

        /// <summary>JobBook o código interno del trabajo</summary>
        public string JobBook { get; set; }

        /// <summary>FK a usuario coordinador asignado</summary>
        public long? IdCoordinador { get; set; }

        /// <summary>FK a usuario gerente del proyecto</summary>
        public long? IdGerenteProyecto { get; set; }

        /// <summary>Fecha límite para completar el trabajo</summary>
        public DateTime? FechaVencimiento { get; set; }

        /// <summary>Presupuesto estimado para el trabajo</summary>
        public decimal? PresupuestoEstimado { get; set; }

        /// <summary>Tipo de estudio cualitativo: Focus Group, Entrevista In-Depth, IDH, etc.</summary>
        public string TipoEstudio { get; set; }

        /// <summary>Número de participantes esperados</summary>
        public int? NumeroParticipantesEstimado { get; set; }

        /// <summary>Ciudad o zona geográfica</summary>
        public string Ubicacion { get; set; }

        /// <summary>Notas adicionales del trabajo</summary>
        public string Notas { get; set; }

        /// <summary>Indica si el trabajo está activo o no</summary>
        public bool Activo { get; set; } = true;

        // Navegación
        /// <summary>Proyecto al que pertenece el trabajo</summary>
        public virtual Proyecto Proyecto { get; set; }

        /// <summary>Segmentos definidos para este trabajo cualitativo</summary>
        public virtual ICollection<SegmentosCuali> Segmentos { get; set; } = new List<SegmentosCuali>();

        /// <summary>Sesiones de recolección de datos</summary>
        public virtual ICollection<SesionesCuali> Sesiones { get; set; } = new List<SesionesCuali>();

        /// <summary>Muestras para este trabajo</summary>
        public virtual ICollection<MuestrasCuali> Muestras { get; set; } = new List<MuestrasCuali>();
    }
}
