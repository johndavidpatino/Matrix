using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Data.Entities
{
    [Table("CU_Estudios")]
    public class CU_Estudios
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("JobBook")]
        public string? JobBook { get; set; }

        [Column("PropuestaId")]
        public long PropuestaId { get; set; }

        [Column("Nombre")]
        public string? Nombre { get; set; }

        [Column("Valor")]
        public double? Valor { get; set; }

        [Column("FechaInicio")]
        public DateTime? FechaInicio { get; set; }

        [Column("FechaTerminacion")]
        public DateTime? FechaTerminacion { get; set; }

        [Column("FechaInicioCampo")]
        public DateTime? FechaInicioCampo { get; set; }

        [Column("Anticipo")]
        public byte? Anticipo { get; set; }

        [Column("Saldo")]
        public byte? Saldo { get; set; }

        [Column("Plazo")]
        public short? Plazo { get; set; }

        [Column("DocumentoSoporte")]
        public byte? DocumentoSoporte { get; set; }

        [Column("TiempoRetencionAnnos")]
        public byte? TiempoRetencionAnnos { get; set; }

        [Column("GerenteCuentas")]
        public long? GerenteCuentas { get; set; }

        [Column("Estado")]
        public byte? Estado { get; set; }

        [Column("Observaciones")]
        public string? Observaciones { get; set; }

        [Column("FormaPago")]
        public string? FormaPago { get; set; }

        [Column("PlazoPago")]
        public string? PlazoPago { get; set; }
    }
}
