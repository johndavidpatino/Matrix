namespace MatrixNext.Web.Models.PY
{
    /// <summary>
    /// Entidad PY_Proyectos
    /// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 6 (Proyecto.vb 30+ métodos)
    /// Ref: VALIDACION_BASE_DATOS.md § 1.1 (parámetros SP)
    /// </summary>
    public class Proyecto : BaseEntity
    {
        public string? Nombre { get; set; }
        
        public string? Descripcion { get; set; }
        
        public long IdGerenteProyectos { get; set; }
        
        public long IdUnidad { get; set; }
        
        public int Estado { get; set; } = 1; // 1=Nuevo, 2=EnProgreso, 3=Cerrado
        
        public string? JobBook { get; set; }
        
        // Relaciones
        public virtual ICollection<Trabajo> Trabajos { get; set; } = new List<Trabajo>();
    }
}
