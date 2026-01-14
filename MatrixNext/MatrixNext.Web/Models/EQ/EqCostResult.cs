using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Resultados de costos calculados para una cotización
    /// Almacena breakdown por rubro, totales, márgenes (GM, PB+RMF, OP)
    /// </summary>
    [Table("eq_cost_result")]
    public class EqCostResult
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuoteHeaderId { get; set; }

        [ForeignKey(nameof(QuoteHeaderId))]
        public virtual EqQuoteHeader? QuoteHeader { get; set; }

        [StringLength(10)]
        public string? Moneda { get; set; } = "COP";

        // Rubros principales
        public decimal CostoCampo { get; set; }

        public decimal CostoCalidad { get; set; }

        public decimal Viaticos { get; set; }

        public decimal Incentivos { get; set; }

        public decimal Insumos { get; set; }

        public decimal Logistica { get; set; }

        public decimal StaffOps { get; set; }

        public decimal Estadistica { get; set; }

        public decimal Scripting { get; set; }

        public decimal DataCleaning { get; set; }

        public decimal TopLines { get; set; }

        public decimal Procesamiento { get; set; }

        public decimal Harmoni { get; set; }

        public decimal Graficacion { get; set; }

        public decimal CompraProducto { get; set; }

        public decimal Tablets { get; set; }

        // Totales
        public decimal CostoDirectoTotal { get; set; }

        public decimal CostoConIncentivos { get; set; }

        public decimal DirectCostOps { get; set; }

        // Márgenes
        public decimal GM { get; set; }

        public decimal PB_RMF { get; set; }

        public decimal ProfTime { get; set; }

        public decimal OP { get; set; }

        public decimal PctOP { get; set; }

        // Unitarios
        public decimal AOTUnitario { get; set; }

        public decimal AOTTotal { get; set; }

        public DateTime FechaCalculo { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}

