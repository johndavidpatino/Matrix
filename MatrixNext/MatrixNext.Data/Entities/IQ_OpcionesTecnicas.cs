using System.Collections.Generic;

namespace MatrixNext.Data.Entities
{
    public class IQ_OpcionesTecnicas
    {
        public int TecCodigo { get; set; }
        public int IdOpcion { get; set; }
        public string? DescOpcion { get; set; }
        public string? NombreVariable { get; set; }

        public virtual ICollection<IQ_OpcionesAplicadas> IQ_OpcionesAplicadas { get; set; }
            = new HashSet<IQ_OpcionesAplicadas>();
    }
}
