using System;

namespace MatrixNext.Data.Entities
{
    public class IQ_ControlCostos
    {
        public long IdPropuesta { get; set; }
        public int ParAlternativa { get; set; }
        public int MetCodigo { get; set; }
        public int ParNacional { get; set; }
        public int ID { get; set; }
        public int Consecutivo { get; set; }
        public decimal ValorAutorizado { get; set; }
        public decimal? ValorEjecutado { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal? Usuario { get; set; }
        public string? Observacion { get; set; }
        public decimal? ValorProduccion { get; set; }
    }
}
