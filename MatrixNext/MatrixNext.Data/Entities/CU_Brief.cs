using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Data.Entities
{
    [Table("CU_Brief")]
    public class CU_Brief
    {
        [Column("Id")]
        public long Id { get; set; }

        [Column("Cliente")]
        public string? Cliente { get; set; }

        [Column("ClienteId")]
        public long? ClienteId { get; set; }

        [Column("Contacto")]
        public string? Contacto { get; set; }

        [Column("Titulo")]
        public string? Titulo { get; set; }

        [Column("Antecedentes")]
        public string? Antecedentes { get; set; }

        [Column("Objetivos")]
        public string? Objetivos { get; set; }

        [Column("ActionStandars")]
        public string? ActionStandars { get; set; }

        [Column("Metodologia")]
        public string? Metodologia { get; set; }

        [Column("Viabilidad")]
        public bool? Viabilidad { get; set; }

        [Column("FechaViabilidad")]
        public DateTime? FechaViabilidad { get; set; }

        [Column("MarcaViabilidad")]
        public long? MarcaViabilidad { get; set; }

        [Column("GerenteCuentas")]
        public long? GerenteCuentas { get; set; }

        [Column("Unidad")]
        public int? Unidad { get; set; }

        [Column("Fecha")]
        public DateTime? Fecha { get; set; }

        [Column("MarcaCategoria")]
        public string? MarcaCategoria { get; set; }

        [Column("TipoBrief")]
        public int? TipoBrief { get; set; }

        [Column("NewClient")]
        public bool? NewClient { get; set; }

        [Column("O1")]
        public string? O1 { get; set; }

        [Column("O2")]
        public string? O2 { get; set; }

        [Column("O3")]
        public string? O3 { get; set; }

        [Column("O4")]
        public string? O4 { get; set; }

        [Column("O5")]
        public string? O5 { get; set; }

        [Column("O6")]
        public string? O6 { get; set; }

        [Column("O7")]
        public string? O7 { get; set; }

        [Column("D1")]
        public string? D1 { get; set; }

        [Column("D2")]
        public string? D2 { get; set; }

        [Column("D3")]
        public string? D3 { get; set; }

        [Column("C1")]
        public string? C1 { get; set; }

        [Column("C2")]
        public string? C2 { get; set; }

        [Column("C3")]
        public string? C3 { get; set; }

        [Column("C4")]
        public string? C4 { get; set; }

        [Column("C5")]
        public string? C5 { get; set; }

        [Column("M1")]
        public string? M1 { get; set; }

        [Column("M2")]
        public string? M2 { get; set; }

        [Column("M3")]
        public string? M3 { get; set; }

        [Column("DI1")]
        public string? DI1 { get; set; }

        [Column("DI2")]
        public string? DI2 { get; set; }

        [Column("DI3")]
        public string? DI3 { get; set; }

        [Column("DI4")]
        public string? DI4 { get; set; }

        [Column("DI5")]
        public string? DI5 { get; set; }

        [Column("DI6")]
        public string? DI6 { get; set; }

        [Column("DI7")]
        public string? DI7 { get; set; }

        [Column("DI8")]
        public string? DI8 { get; set; }

        [Column("DI9")]
        public string? DI9 { get; set; }

        [Column("DI10")]
        public string? DI10 { get; set; }

        [Column("DI11")]
        public string? DI11 { get; set; }

        [Column("DI12")]
        public string? DI12 { get; set; }

        [Column("DI13")]
        public string? DI13 { get; set; }

        [Column("DI14")]
        public string? DI14 { get; set; }

        [Column("DI15")]
        public string? DI15 { get; set; }

        [Column("DI16")]
        public string? DI16 { get; set; }

        [Column("DI17")]
        public string? DI17 { get; set; }

        [Column("DI18")]
        public string? DI18 { get; set; }
    }
}
