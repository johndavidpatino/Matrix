using System;
using System.Collections.Generic;

namespace MatrixNext.Data.Modules.CU.Models
{
    public class EstudioListItemViewModel
    {
        public long Id { get; set; }
        public long PropuestaId { get; set; }
        public string? JobBook { get; set; }
        public string? Nombre { get; set; }
        public double? Valor { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaTerminacion { get; set; }
        public string? Estado { get; set; }
    }

    public class EstudioViewModel
    {
        public long Id { get; set; }
        public long PropuestaId { get; set; }
        public string? JobBook { get; set; }
        public string? Nombre { get; set; }
        public double? Valor { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaTerminacion { get; set; }
        public DateTime? FechaInicioCampo { get; set; }
        public byte? Anticipo { get; set; }
        public byte? Saldo { get; set; }
        public short? Plazo { get; set; }
        public byte? DocumentoSoporte { get; set; }
        public byte? TiempoRetencionAnnos { get; set; }
        public string? Observaciones { get; set; }
    }

    public class EstudioFormViewModel
    {
        public EstudioViewModel Estudio { get; set; } = new EstudioViewModel();
        public IEnumerable<CatalogoItem<byte>> DocumentosSoporte { get; set; } = Array.Empty<CatalogoItem<byte>>();
    }

    public class EstudiosIndexViewModel
    {
        public List<EstudioListItemViewModel> Estudios { get; set; } = new List<EstudioListItemViewModel>();
        public long? IdPropuesta { get; set; }
        public JobBookContextViewModel? Contexto { get; set; }
    }
}
