namespace MatrixNext.Web.Models.CORE
{
    /// <summary>
    /// Entidad CORE_WorkFlow
    /// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md (creación de tareas en WorkFlow)
    /// Ref: VALIDACION_BASE_DATOS.md § 1.5 (parámetros SP)
    /// </summary>
    public class WorkFlow : BaseEntity
    {
        public long IdTrabajo { get; set; }
        
        public long IdTarea { get; set; }
        
        public int IdTipoHilo { get; set; }
        
        public string? Estado { get; set; } = "Creada"; // Creada, EnProgreso, Completada, Anulada
        
        public int Prioridad { get; set; } = 1; // 1=Normal, 2=Alta, 3=Baja
        
        public DateTime? FechaVencimiento { get; set; }
        
        public string? Observaciones { get; set; }
        
        // Relaciones
        public virtual ICollection<WorkFlowUsuarioAsignado> UsuariosAsignados { get; set; } = new List<WorkFlowUsuarioAsignado>();
        
        public virtual ICollection<ObservacionTarea> Observaciones_Log { get; set; } = new List<ObservacionTarea>();
        
        public virtual ICollection<TareaPrevia> TareasPrevias { get; set; } = new List<TareaPrevia>();
    }
}
