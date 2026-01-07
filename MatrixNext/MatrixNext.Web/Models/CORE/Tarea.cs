namespace MatrixNext.Web.Models.CORE
{
    /// <summary>
    /// Entidad CORE_Tareas - Catálogo de tipos de tareas (plantillas)
    /// Ref: MIGRACION_CORE.md § Fase 1 - Configuración
    /// Ref: CoreProject/CORE_Tareas.vb
    /// </summary>
    public class Tarea : BaseEntity
    {
        /// <summary>
        /// Nombre de la tarea (tipo de tarea)
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Días mínimos antes de poder iniciar (desde inicio del hilo)
        /// </summary>
        public long? NoEmpiezaAntesDe { get; set; }

        /// <summary>
        /// Días mínimos antes de poder terminar
        /// </summary>
        public long? NoTerminaAntesDe { get; set; }

        /// <summary>
        /// Tiempo promedio estimado en días para completar esta tarea
        /// </summary>
        public short? TiempoPromedioDias { get; set; }

        /// <summary>
        /// Indica si la tarea requiere estimación de tiempo
        /// </summary>
        public bool? RequiereEstimacion { get; set; }

        /// <summary>
        /// ID del rol responsable de estimar
        /// </summary>
        public long? RolEstima { get; set; }

        /// <summary>
        /// ID de la unidad que ejecuta la tarea
        /// </summary>
        public long? UnidadEjecuta { get; set; }

        /// <summary>
        /// ID de la unidad que recibe el resultado
        /// </summary>
        public long? UnidadRecibe { get; set; }

        /// <summary>
        /// ID del rol que ejecuta la tarea
        /// </summary>
        public long? RolEjecuta { get; set; }

        /// <summary>
        /// Indica si la tarea es visible en listados
        /// </summary>
        public bool? Visible { get; set; } = true;

        /// <summary>
        /// Orden de presentación en listados
        /// </summary>
        public long? Orden { get; set; }
    }
}
