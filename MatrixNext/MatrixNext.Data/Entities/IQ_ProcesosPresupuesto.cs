using System;

namespace MatrixNext.Data.Entities
{
    public class IQ_ProcesosPresupuesto
    {
        public long IdPropuesta { get; set; }
        public int ParAlternativa { get; set; }
        public int MetCodigo { get; set; }
        public int ProcCodigo { get; set; }
        public double? Porcentaje { get; set; }
        public int ParNacional { get; set; }

        public virtual IQ_Procesos? IQ_Procesos { get; set; }
        public virtual IQ_Parametros? IQ_Parametros { get; set; }
    }
}
