using System.Collections.Generic;

namespace MatrixNext.Data.Modules.CU.Models
{
    public class JobBookSearchViewModel
    {
        public string? Titulo { get; set; }
        public string? JobBook { get; set; }
        public long? IdPropuesta { get; set; }
        public int TypeSearch { get; set; } = 1;
        public List<JobBookResultViewModel> Resultados { get; set; } = new List<JobBookResultViewModel>();
    }

    public class JobBookResultViewModel
    {
        public long IdBrief { get; set; }
        public long IdPropuesta { get; set; }
        public long IdEstudio { get; set; }
        public string? Cliente { get; set; }
        public string? Titulo { get; set; }
        public string? Estado { get; set; }
        public string? GerenteCuentas { get; set; }
        public long? GerenteCuentasID { get; set; }
        public int? IdUnidad { get; set; }
        public string? Unidad { get; set; }
        public bool? Viabilidad { get; set; }
        public string? NumJobBook { get; set; }
        public string? MarcaCategoria { get; set; }
    }

    public class JobBookContextViewModel
    {
        public long IdBrief { get; set; }
        public long IdPropuesta { get; set; }
        public long IdEstudio { get; set; }
        public string? Cliente { get; set; }
        public string? Titulo { get; set; }
        public string? NumJobBook { get; set; }
    }
}
