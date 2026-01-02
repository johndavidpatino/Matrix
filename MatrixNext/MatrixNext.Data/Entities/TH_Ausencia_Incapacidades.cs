using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Data.Entities
{
    [Table("TH_Ausencia_Incapacidades")]
    public class TH_Ausencia_Incapacidades
    {
        [Column("idSolicitudAusencia")]
        public int IdSolicitudAusencia { get; set; }

        [Column("EntidadConsulta")]
        public byte? EntidadConsulta { get; set; }

        [Column("IPS")]
        public string? IPS { get; set; }

        [Column("RegistroMedico")]
        public string? RegistroMedico { get; set; }

        [Column("TipoIncapacidad")]
        public byte? TipoIncapacidad { get; set; }

        [Column("ClaseAusencia")]
        public byte? ClaseAusencia { get; set; }

        [Column("SOAT")]
        public byte? SOAT { get; set; }

        [Column("FechaAccidenteTrabajo")]
        public DateTime? FechaAccidenteTrabajo { get; set; }

        [Column("Comentarios")]
        public string? Comentarios { get; set; }

        [Column("CIE")]
        public string? CIE { get; set; }
    }
}
