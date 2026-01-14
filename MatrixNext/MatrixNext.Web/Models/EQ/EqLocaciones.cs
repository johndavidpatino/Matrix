using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Tarifas de locaciones por ciudad y otros parametros
    /// Mapea desde Excel Valores Insumos reclutamiento: tarifa base, con gross, dias base
    /// </summary>
    [Table("eq_locaciones")]
    public class EqLocaciones
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Ciudad { get; set; }

        public decimal TarifaBase { get; set; }

        public decimal? TarifaConGross { get; set; }

        public int DiasBase { get; set; } = 1;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}

