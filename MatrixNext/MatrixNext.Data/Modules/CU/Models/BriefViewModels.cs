using System;
using System.Collections.Generic;

namespace MatrixNext.Data.Modules.CU.Models
{
    public class BriefViewModel
    {
        public long Id { get; set; }
        public DateTime? Fecha { get; set; }
        public string? MarcaCategoria { get; set; }
        public string? Cliente { get; set; }
        public string? Contacto { get; set; }
        public string? Titulo { get; set; }
        public string? Antecedentes { get; set; }
        public string? Objetivos { get; set; }
        public string? ActionStandars { get; set; }
        public string? Metodologia { get; set; }
        public int? Unidad { get; set; }
        public bool? Viabilidad { get; set; }
        public DateTime? FechaViabilidad { get; set; }
        public bool NewClient { get; set; }
        public string? O1 { get; set; }
        public string? O2 { get; set; }
        public string? O3 { get; set; }
        public string? O4 { get; set; }
        public string? O5 { get; set; }
        public string? O6 { get; set; }
        public string? O7 { get; set; }
        public string? D1 { get; set; }
        public string? D2 { get; set; }
        public string? D3 { get; set; }
        public string? C1 { get; set; }
        public string? C2 { get; set; }
        public string? C3 { get; set; }
        public string? C4 { get; set; }
        public string? C5 { get; set; }
        public string? M1 { get; set; }
        public string? M2 { get; set; }
        public string? M3 { get; set; }
        public string? DI1 { get; set; }
        public string? DI2 { get; set; }
        public string? DI3 { get; set; }
        public string? DI4 { get; set; }
        public string? DI5 { get; set; }
        public string? DI6 { get; set; }
        public string? DI7 { get; set; }
        public string? DI8 { get; set; }
        public string? DI9 { get; set; }
        public string? DI10 { get; set; }
        public string? DI11 { get; set; }
        public string? DI12 { get; set; }
        public string? DI13 { get; set; }
        public string? DI14 { get; set; }
        public string? DI15 { get; set; }
        public string? DI16 { get; set; }
        public string? DI17 { get; set; }
        public string? DI18 { get; set; }
    }

    public class BriefFormViewModel
    {
        public BriefViewModel Brief { get; set; } = new BriefViewModel();
        public IEnumerable<UnidadViewModel> Unidades { get; set; } = Array.Empty<UnidadViewModel>();
        public JobBookContextViewModel? Contexto { get; set; }
    }

    public class UnidadViewModel
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
    }

    // TODO-P0-03: ViewModel para clonación de Brief
    public class ClonarBriefViewModel
    {
        public long IdBrief { get; set; }
        public string? TituloOriginal { get; set; }
        public int IdUnidad { get; set; }
        public string? NuevoNombre { get; set; }
        public IEnumerable<UnidadViewModel> Unidades { get; set; } = Array.Empty<UnidadViewModel>();
    }
}
