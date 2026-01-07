namespace MatrixNext.Web.Models.CORE
{
    /// <summary>
    /// Entidad CORE_WorkFlow_TareasPrevias
    /// Relación de precedencias: una tarea require otra completada antes
    /// Ref: VALIDACION_BASE_DATOS.md § 1.5
    /// Ref: MATRIZ_PERMISOS_ROLES.md § 5.2 (GrafoAciclico)
    /// </summary>
    public class TareaPrevía : BaseEntity
    {
        public long IdTarea { get; set; }
        
        public long? IdTareaPreviaRequerida { get; set; }
        
        public int Orden { get; set; } = 1; // Orden de ejecución si hay múltiples
        
        // Relaciones
        public virtual WorkFlow? Tarea { get; set; }
        
        public virtual WorkFlow? TareaPreviaRequerida { get; set; }
    }
}
