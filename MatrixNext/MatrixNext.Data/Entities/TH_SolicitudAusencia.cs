using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Data.Entities
{
    [Table("TH_SolicitudAusencia")]
    public class TH_SolicitudAusencia
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("idEmpleado")]
        public long IdEmpleado { get; set; }

        [Column("FiniCausacion")]
        public DateTime? FiniCausacion { get; set; }

        [Column("FFinCausacion")]
        public DateTime? FFinCausacion { get; set; }

        [Column("FInicio")]
        public DateTime? FInicio { get; set; }

        [Column("FFin")]
        public DateTime? FFin { get; set; }

        [Column("DiasCalendario")]
        public short? DiasCalendario { get; set; }

        [Column("DiasLaborales")]
        public byte? DiasLaborales { get; set; }

        [Column("Tipo")]
        public byte? Tipo { get; set; }

        [Column("Estado")]
        public byte? Estado { get; set; }

        [Column("AprobadoPor")]
        public long? AprobadoPor { get; set; }

        [Column("FechaAprobacion")]
        public DateTime? FechaAprobacion { get; set; }

        [Column("VoBo1")]
        public long? VoBo1 { get; set; }

        [Column("FechaVoBo1")]
        public DateTime? FechaVoBo1 { get; set; }

        [Column("VoBo2")]
        public long? VoBo2 { get; set; }

        [Column("FechaVoBo2")]
        public DateTime? FechaVoBo2 { get; set; }

        [Column("VoBo3")]
        public long? VoBo3 { get; set; }

        [Column("FechaVoBo3")]
        public DateTime? FechaVoBo3 { get; set; }

        [Column("RegistradoPor")]
        public long? RegistradoPor { get; set; }

        [Column("FechaRegistro")]
        public DateTime? FechaRegistro { get; set; }

        [Column("ObservacionesSolicitud")]
        public string? ObservacionesSolicitud { get; set; }

        [Column("ObservacionesAprobacion")]
        public string? ObservacionesAprobacion { get; set; }
    }
}
