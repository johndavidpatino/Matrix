using System;

namespace MatrixNext.Data.Entities
{
    public class IQ_DatosGeneralesPresupuesto
    {
        public long IdPropuesta { get; set; }
        public int ParAlternativa { get; set; }
        public string? Descripcion { get; set; }
        public string? Observaciones { get; set; }
        public int DiasCampo { get; set; }
        public int? DiasDiseno { get; set; }
        public int? DiasProcesamiento { get; set; }
        public int? DiasInformes { get; set; }
        public int? Anticipo { get; set; }
        public int? Saldo { get; set; }
        public int? Plazo { get; set; }
        public float? TasaCambio { get; set; }
        public int? NumeroMediciones { get; set; }
        public int? MesesMediciones { get; set; }
        public byte? TipoPresupuesto { get; set; }
        public string? NoIQuote { get; set; }
    }
}
