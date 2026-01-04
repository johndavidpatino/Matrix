using System;

namespace MatrixNext.Data.Entities
{
    public class IQ_CostoActividades
    {
        public long IdPropuesta { get; set; }
        public int MetCodigo { get; set; }
        public int ActCodigo { get; set; }
        public decimal CaCosto { get; set; }
        public int? CaUnidades { get; set; }
        public string? CaDescripcionUnidades { get; set; }
        public int ParAlternativa { get; set; }
        public int ParNacional { get; set; }
        public int? Horas { get; set; }

        public virtual IQ_Parametros? IQ_Parametros { get; set; }
    }
}
