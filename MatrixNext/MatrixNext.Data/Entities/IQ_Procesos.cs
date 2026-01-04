using System.Collections.Generic;

namespace MatrixNext.Data.Entities
{
    public class IQ_Procesos
    {
        public int ProcCodigo { get; set; }
        public string? ProcDescripcion { get; set; }

        public virtual ICollection<IQ_TecnicaProcesos> IQ_TecnicaProcesos { get; set; }
            = new HashSet<IQ_TecnicaProcesos>();

        public virtual ICollection<IQ_ProcesosPresupuesto> IQ_ProcesosPresupuesto { get; set; }
            = new HashSet<IQ_ProcesosPresupuesto>();
    }
}
