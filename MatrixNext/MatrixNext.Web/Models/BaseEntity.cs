namespace MatrixNext.Web.Models
{
    /// <summary>
    /// Clase base para todas las entidades del sistema
    /// Ref: ESPECIFICACION_COMPONENTES_COMPARTIDOS.md § 5 (BaseVM)
    /// </summary>
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        
        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
        
        public long UsuarioCreacion { get; set; }
        
        public long UsuarioModificacion { get; set; }
        
        public bool Activo { get; set; } = true;
    }
}
