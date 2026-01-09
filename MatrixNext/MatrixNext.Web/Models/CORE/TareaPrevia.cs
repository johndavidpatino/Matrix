namespace MatrixNext.Web.Models.CORE
{
    /// <summary>
    /// Entidad CORE_TareasPrevias
    /// RelaciÃ³n de precedencias: una tarea require otra completada antes
    /// Ref: VALIDACION_BASE_DATOS.md Â§ 1.5
    /// Ref: MATRIZ_PERMISOS_ROLES.md Â§ 5.2 (GrafoAciclico)
    /// </summary>
    public class TareaPrevia : BaseEntity
    {
        public long IdTarea { get; set; }
        
        public long? IdTareaPreviaRequerida { get; set; }
        
        public int Orden { get; set; } = 1; // Orden de ejecuciÃ³n si hay mÃºltiples
        
        // Relaciones
        public virtual WorkFlow? Tarea { get; set; }
        
        public virtual WorkFlow? TareaPreviaRequerida { get; set; }
    }
}
