using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Tarifas de valor hora por nivel OPS (L3-L8) y alternativas
    /// Mapea desde Excel Valor Hora - Alternativas: base_cost, overhead, loaded, billing rates
    /// </summary>
    [Table("eq_valor_hora_ops")]
    public class EqValorHoraOps
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string? Nivel { get; set; } // L1, L2, L3, L4, L5, L6, L7, L8

        [StringLength(50)]
        public string? Alternativa { get; set; } // 2022, 2023, 2023_alt1, 2023_alt2

        public decimal BaseCostRate { get; set; }

        public decimal OverheadRate { get; set; }

        public decimal LoadedCostRate { get; set; }

        public decimal BillingRate { get; set; }

        public DateTime VigentDesde { get; set; }

        public DateTime? VigentHasta { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}

