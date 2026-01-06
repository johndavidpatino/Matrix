namespace MatrixNext.Web.Models.PY
{
    /// <summary>
    /// Entidad PY_Variables_Control
    /// Ref: VALIDACION_BASE_DATOS.md § 1.2
    /// </summary>
    public class VariableControl : BaseEntity
    {
        public long IdTrabajo { get; set; }
        
        public string Nombre { get; set; }
        
        public string Valor { get; set; }
        
        public int TipoDato { get; set; } // 1=Texto, 2=Número, 3=Fecha
        
        // Relaciones
        public virtual Trabajo Trabajo { get; set; }
    }
}
