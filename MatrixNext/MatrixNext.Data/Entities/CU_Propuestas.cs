using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Data.Entities
{
    [Table("CU_Propuestas")]
    public class CU_Propuestas
    {
        [Column("Id")]
        public long Id { get; set; }

        [Column("Titulo")]
        public string? Titulo { get; set; }

        [Column("Brief")]
        public long Brief { get; set; }

        [Column("TipoId")]
        public byte? TipoId { get; set; }

        [Column("ProbabilidadId")]
        public decimal? ProbabilidadId { get; set; }

        [Column("EstadoId")]
        public byte? EstadoId { get; set; }

        [Column("FechaEnvio")]
        public DateTime? FechaEnvio { get; set; }

        [Column("FechaAprob")]
        public DateTime? FechaAprob { get; set; }

        [Column("RazonNoAprobId")]
        public short? RazonNoAprobId { get; set; }

        [Column("JobBook")]
        public string? JobBook { get; set; }

        [Column("Internacional")]
        public bool? Internacional { get; set; }

        [Column("Anticipo")]
        public byte? Anticipo { get; set; }

        [Column("Saldo")]
        public byte? Saldo { get; set; }

        [Column("Plazo")]
        public short? Plazo { get; set; }

        [Column("FechaInicioCampo")]
        public DateTime? FechaInicioCampo { get; set; }

        [Column("RequestHabeasData")]
        public string? RequestHabeasData { get; set; }

        [Column("Tracking")]
        public bool? Tracking { get; set; }

        [Column("OrigenId")]
        public byte? OrigenId { get; set; }

        [Column("FormaEnvio")]
        public string? FormaEnvio { get; set; }
    }
}
