using System;
using System.Collections.Generic;

namespace MatrixNext.Data.Modules.CU.Models
{
    public class CatalogoItem<T>
    {
        public T Id { get; set; } = default!;
        public string? Nombre { get; set; }
    }

    public class PropuestaListItemViewModel
    {
        public long Id { get; set; }
        public string? Titulo { get; set; }
        public string? Cliente { get; set; }
        public string? Estado { get; set; }
        public string? Probabilidad { get; set; }
        public string? JobBook { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public long BriefId { get; set; }
    }

    public class PropuestaViewModel
    {
        public long Id { get; set; }
        public long BriefId { get; set; }
        public string? Titulo { get; set; }
        public decimal? ProbabilidadId { get; set; }
        public byte? EstadoId { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public DateTime? FechaAprob { get; set; }
        public short? RazonNoAprobId { get; set; }
        public string? JobBook { get; set; }
        public bool Internacional { get; set; }
        public bool Tracking { get; set; }
        public byte? Anticipo { get; set; }
        public byte? Saldo { get; set; }
        public short? Plazo { get; set; }
        public DateTime? FechaInicioCampo { get; set; }
        public string? RequestHabeasData { get; set; }
    }

    public class PropuestaFormViewModel
    {
        public PropuestaViewModel Propuesta { get; set; } = new PropuestaViewModel();
        public IEnumerable<CatalogoItem<byte>> Estados { get; set; } = Array.Empty<CatalogoItem<byte>>();
        public IEnumerable<CatalogoItem<decimal>> Probabilidades { get; set; } = Array.Empty<CatalogoItem<decimal>>();
        public IEnumerable<CatalogoItem<short>> Razones { get; set; } = Array.Empty<CatalogoItem<short>>();
    }

    public class PropuestasIndexViewModel
    {
        public List<PropuestaListItemViewModel> Propuestas { get; set; } = new List<PropuestaListItemViewModel>();
        public IEnumerable<CatalogoItem<byte>> Estados { get; set; } = Array.Empty<CatalogoItem<byte>>();
        public byte? EstadoFiltro { get; set; }
        public long? IdBriefContext { get; set; }
        public long? IdPropuestaContext { get; set; }
    }

    public class ObservacionViewModel
    {
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        public string? Observacion { get; set; }
        public string? Usuario { get; set; }
    }
}
