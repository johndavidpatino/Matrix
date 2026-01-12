using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Encabezado de cotización EasyQuote
    /// Mapea desde Excel Entradas: propuesta, cliente, SL, metodologias
    /// </summary>
    [Table("eq_quote_header")]
    public class EqQuoteHeader
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string PropuestaNombre { get; set; }

        [Required]
        [StringLength(500)]
        public string GrupoObjetivo { get; set; }

        [Required]
        [StringLength(500)]
        public string Cliente { get; set; }

        public DateTime? FechaAprobacionEstimada { get; set; }

        public DateTime? FechaCampo { get; set; }

        [StringLength(50)]
        public string ProbabilidadAprobacion { get; set; } // Alta, Media, Baja

        [Required]
        [StringLength(50)]
        public string SL { get; set; } // Nivel de la propuesta

        [Required]
        [StringLength(50)]
        public string MetodologiaSL { get; set; } // F2F, CATI, ONLINE, AUTO, MYSTERY, SHOPPER

        [Required]
        [StringLength(50)]
        public string RecordDetail { get; set; }

        [StringLength(100)]
        public string CategoriaProducto { get; set; } // Otro, Bebidas, etc.

        public decimal? ValorProveedorExterno { get; set; }

        public decimal? ValorProveedorInternacional { get; set; }

        public decimal? ValorGMU { get; set; }

        [StringLength(1000)]
        public string Notas { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;

        // Relaciones
        public virtual ICollection<EqQuestionnaire> Questionnaires { get; set; } = new List<EqQuestionnaire>();
        public virtual ICollection<EqMethodology> Methodologies { get; set; } = new List<EqMethodology>();
        public virtual ICollection<EqSampleCity> SampleCities { get; set; } = new List<EqSampleCity>();
        public virtual ICollection<EqMystery> Mysteries { get; set; } = new List<EqMystery>();
        public virtual ICollection<EqStaffSL> StaffSL { get; set; } = new List<EqStaffSL>();
        public virtual EqCostResult CostResult { get; set; }
    }
}
