using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Metodologias de recoleccion para una cotización
    /// Mapea desde Excel Entradas: tecnicas (F2F, CATI, ONLINE, AUTO)
    /// </summary>
    [Table("eq_methodology")]
    public class EqMethodology
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuoteHeaderId { get; set; }

        [ForeignKey(nameof(QuoteHeaderId))]
        public virtual EqQuoteHeader QuoteHeader { get; set; }

        [StringLength(50)]
        public string MetodologiaRecoleccion { get; set; } // Hogares, Empresas, etc.

        [StringLength(50)]
        public string Tecnica1Tipo { get; set; }

        public bool Tecnica1Flag { get; set; }

        [StringLength(50)]
        public string Tecnica2Tipo { get; set; }

        public bool Tecnica2Flag { get; set; }

        [StringLength(50)]
        public string Tecnica3Tipo { get; set; }

        public bool Tecnica3Flag { get; set; }

        [StringLength(50)]
        public string BaseDatos { get; set; } // No requiere, Comprar, Cliente

        [StringLength(100)]
        public string IncidenciaLabel { get; set; }

        public decimal? IncidenciaValor { get; set; }

        public bool MetodologiasMix { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}
