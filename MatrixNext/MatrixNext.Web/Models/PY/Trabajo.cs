namespace MatrixNext.Web.Models.PY
{
    /// <summary>
    /// Entidad PY_Trabajo
    /// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 7 (Trabajo.vb 50+ métodos)
    /// Ref: VALIDACION_BASE_DATOS.md § 1.2 (parámetros SP)
    /// </summary>
    public class Trabajo : BaseEntity
    {
        public long IdProyecto { get; set; }
        
        public string Nombre { get; set; }
        
        public string Descripcion { get; set; }
        
        public int IdMetodologia { get; set; }
        
        public int IdTipoProyecto { get; set; }
        
        public string JobBook { get; set; }
        
        public int Estado { get; set; } = 1; // 1=Nuevo, 2=Enviado, 3=Cerrado, 11=Anulado
        
        public long? IdCoordinador { get; set; }
        
        public DateTime? FechaEnvio { get; set; }
        
        public DateTime? FechaCierre { get; set; }
        
        // Relaciones
        public virtual Proyecto Proyecto { get; set; }
        
        public virtual ICollection<VariableControl> VariablesControl { get; set; } = new List<VariableControl>();
    }
}
