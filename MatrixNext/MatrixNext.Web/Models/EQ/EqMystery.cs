using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Detalles de estudios Mystery/Shopper
    /// Mapea desde Excel MYSTERY sheet: tipos de visita, olas, desplazamientos
    /// </summary>
    [Table("eq_mystery")]
    public class EqMystery
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuoteHeaderId { get; set; }

        [ForeignKey(nameof(QuoteHeaderId))]
        public virtual EqQuoteHeader? QuoteHeader { get; set; }

        [Range(1, 3)]
        public int TipoVisita { get; set; } // 1, 2, 3

        [StringLength(100)]
        public string? Complejidad { get; set; }

        public int NumOlas { get; set; } = 1;

        public decimal? Desplazamientos { get; set; }

        public decimal? Tanques { get; set; }

        public decimal? Alertas { get; set; }

        public decimal? EdicionVideo { get; set; }

        public decimal? AlquilerEquipos { get; set; }

        public decimal? CompraDispositivos { get; set; }

        public decimal? Seguimiento { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}

