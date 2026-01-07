namespace MatrixNext.Web.Models.CORE
{
    /// <summary>
    /// Entidad CORE_WorkFlow_UsuariosAsignados
    /// Relación N:N entre tareas y usuarios responsables
    /// Ref: VALIDACION_BASE_DATOS.md § 1.5
    /// </summary>
    public class WorkFlowUsuarioAsignado : BaseEntity
    {
        public long IdWorkFlow { get; set; }
        
        public long IdUsuario { get; set; }
        
        public string? Rol { get; set; } // "Responsable", "Observador", etc.
        
        public DateTime? FechaAsignacion { get; set; } = DateTime.UtcNow;
        
        // Relaciones
        public virtual WorkFlow? WorkFlow { get; set; }
    }
}
