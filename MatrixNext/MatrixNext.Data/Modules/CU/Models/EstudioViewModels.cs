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
        
        // TODO-P0-02: Presupuestos seleccionados
        public List<long> PresupuestosSeleccionados { get; set; } = new List<long>();
    }

    public class EstudioFormViewModel
    {
        public EstudioViewModel Estudio { get; set; } = new EstudioViewModel();
        public IEnumerable<CatalogoItem<byte>> DocumentosSoporte { get; set; } = Array.Empty<CatalogoItem<byte>>();
        
        // TODO-P0-02: Lista de presupuestos aprobados disponibles
        public List<PresupuestoAprobadoViewModel> PresupuestosAprobados { get; set; } = new List<PresupuestoAprobadoViewModel>();
    }

    public class EstudiosIndexViewModel
    {
        public List<EstudioListItemViewModel> Estudios { get; set; } = new List<EstudioListItemViewModel>();
        public long? IdPropuesta { get; set; }
        public JobBookContextViewModel? Contexto { get; set; }
    }

    // TODO-P0-02: ViewModels para presupuestos
    public class PresupuestoAprobadoViewModel
    {
        public long Id { get; set; }
        public int Alternativa { get; set; }
        public double Valor { get; set; }
        public string? Metodologia { get; set; }
        public string? Estado { get; set; }
    }

    public class PresupuestoAsignadoViewModel
    {
        public long Id { get; set; }
        public int Alternativa { get; set; }
        public double Valor { get; set; }
        public string? Metodologia { get; set; }
    }
}
