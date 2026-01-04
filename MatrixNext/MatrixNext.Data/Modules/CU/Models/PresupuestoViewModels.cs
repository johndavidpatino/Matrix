using System.Collections.Generic;

namespace MatrixNext.Data.Modules.CU.Models
{
    public class AlternativaViewModel
    {
        public long IdPropuesta { get; set; }
        public int ParAlternativa { get; set; }
        public string? Descripcion { get; set; }
        public string? Observaciones { get; set; }
        public int DiasCampo { get; set; }
        public int? DiasDiseno { get; set; }
        public int? DiasInformes { get; set; }
        public int? DiasProcesamiento { get; set; }
        public int? NumeroMediciones { get; set; }
        public int? MesesMediciones { get; set; }
        public byte? TipoPresupuesto { get; set; }
        public string? NoIQuote { get; set; }
        public int? TotalPresupuestos { get; set; }
        public decimal? ValorTotal { get; set; }
    }

    public class PresupuestoIndexViewModel
    {
        public long IdPropuesta { get; set; }
        public JobBookContextViewModel? JobBookContext { get; set; }
        public List<AlternativaViewModel> Alternativas { get; set; } = new List<AlternativaViewModel>();
    }

    public class EditarAlternativaViewModel
    {
        public long IdPropuesta { get; set; }
        public int ParAlternativa { get; set; }
        public string? Descripcion { get; set; }
        public string? Observaciones { get; set; }
        public int DiasCampo { get; set; }
        public int? DiasDiseno { get; set; }
        public int? DiasProcesamiento { get; set; }
        public int? DiasInformes { get; set; }
        public int? NumeroMediciones { get; set; }
        public int? MesesMediciones { get; set; }
        public byte? TipoPresupuesto { get; set; }
        public string? NoIQuote { get; set; }
    }
}
