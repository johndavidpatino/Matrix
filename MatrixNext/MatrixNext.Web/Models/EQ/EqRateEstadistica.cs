using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Catalogo de servicios estadisticos adicionales
    /// Mapea desde Excel Tarifario Estadistica: horas, precio_ref, factor_escala, lead_time
    /// </summary>
    [Table("eq_rate_estadistica")]
    public class EqRateEstadistica
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Categoria { get; set; }

        [Required]
        [StringLength(200)]
        public string Servicio { get; set; }

        public decimal? HorasEstimadas { get; set; }

        public decimal PrecioRef2024 { get; set; }

        public decimal? FactorEscala { get; set; }

        [StringLength(100)]
        public string LeadTime { get; set; }

        [StringLength(500)]
        public string Ejemplos { get; set; }

        public decimal? FactorEconomiaEscala { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}
