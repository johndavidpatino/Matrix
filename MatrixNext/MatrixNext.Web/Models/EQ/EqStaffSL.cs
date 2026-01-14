using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Staff OPS/SL por nivel (L3-L7) para una cotización
    /// Mapea desde Excel Entradas tabla Staff SL: horas presupuestadas y tarifas
    /// </summary>
    [Table("eq_staff_sl")]
    public class EqStaffSL
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuoteHeaderId { get; set; }

        [ForeignKey(nameof(QuoteHeaderId))]
        public virtual EqQuoteHeader? QuoteHeader { get; set; }

        [Required]
        [StringLength(10)]
        public string? Nivel { get; set; } // L3, L4, L5, L6, L7

        public decimal HorasMinimas { get; set; }

        public decimal HorasPresupuestadas { get; set; }

        public decimal TarifaNivel { get; set; }

        public decimal ValorTotal { get; set; } // Calculado: HorasPresupuestadas * TarifaNivel

        [StringLength(100)]
        public string? Fuente { get; set; } // Tabla Horas

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}

