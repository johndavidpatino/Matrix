namespace MatrixNext.Web.Models.CORE
{
    /// <summary>
    /// Entidad CORE_ObservacionesTareas
    /// Auditoría: Quién cambió qué, cuándo
    /// Ref: MATRIZ_PERMISOS_ROLES.md § 6.1
    /// Ref: VALIDACION_BASE_DATOS.md § 3.1
    /// </summary>
    public class ObservacionTarea : BaseEntity
    {
        public long IdWorkFlow { get; set; }
        
        public long IdUsuario { get; set; }
        
        public string? Observacion { get; set; }
        
        public string? TipoOperacion { get; set; } // "CambioEstado", "Anulacion", "ComentarioGeneral"
        
        // Relaciones
        public virtual WorkFlow? WorkFlow { get; set; }
    }
}
