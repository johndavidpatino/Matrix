using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Costos unitarios e insumos por NSE para reclutamiento, obsequios, transporte, etc.
    /// Mapea desde Excel Valores Insumos reclutamiento
    /// </summary>
    [Table("eq_cost_insumos")]
    public class EqCostInsumos
    {
        [Key]
        public int Id { get; set; }

        [Range(1, 6)]
        public int NSE { get; set; }

        public decimal Reclutamiento { get; set; }

        public decimal Obsequio { get; set; }

        public decimal Productividad { get; set; }

        public decimal Dias { get; set; }

        public decimal Supervisores { get; set; }

        public decimal Logistica { get; set; }

        public decimal TransporteEncuestador { get; set; }

        public decimal TransporteSupervisor { get; set; }

        public decimal ValorEnvio1erKilo { get; set; }

        public decimal ValorKiloAdicional { get; set; }

        public decimal SeguroPct { get; set; }

        public decimal ValorMinDeclarar { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}
