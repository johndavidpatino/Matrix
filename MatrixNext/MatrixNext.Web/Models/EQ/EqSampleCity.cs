using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Distribucion de muestra por ciudad y NSE
    /// Mapea desde Excel Entradas: ciudades, muestra, NSE, envios
    /// </summary>
    [Table("eq_sample_city")]
    public class EqSampleCity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuoteHeaderId { get; set; }

        [ForeignKey(nameof(QuoteHeaderId))]
        public virtual EqQuoteHeader? QuoteHeader { get; set; }

        [Required]
        [StringLength(100)]
        public string? Ciudad { get; set; }

        public bool Activa { get; set; } = true;

        public int MuestraTotal { get; set; }

        public int NSE1 { get; set; } = 0;
        public int NSE2 { get; set; } = 0;
        public int NSE3 { get; set; } = 0;
        public int NSE4 { get; set; } = 0;
        public int NSE5 { get; set; } = 0;
        public int NSE6 { get; set; } = 0;

        [StringLength(50)]
        public string? MetodologiaTecnicaReferenciada { get; set; }

        public decimal SobreMuestraPct { get; set; } = 0;

        public decimal? PesoProductoGramos { get; set; }

        public bool EnvioCiudades { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}

