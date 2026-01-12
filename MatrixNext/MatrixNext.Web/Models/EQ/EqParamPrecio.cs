using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Matriz de precios base por duracion y penetracion para cada metodologia
    /// Mapea desde Excel Parametros y Precios bases: F2F, CATI, ONLINE, AUTO
    /// </summary>
    [Table("eq_param_precio")]
    public class EqParamPrecio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoMetodologia { get; set; } // F2F, CATI, ONLINE, AUTO

        [StringLength(100)]
        public string PenetracionRango { get; set; } // Mas82, 75-82, 67-74, 55-66, 46-54, 37-45

        [Range(5, 60)]
        public int DuracionMin { get; set; }

        public decimal ValorPerfil { get; set; }

        public decimal ValorCoord { get; set; }

        public decimal ValorTotal { get; set; }

        [StringLength(50)]
        public string Version { get; set; }

        public DateTime VigentDesde { get; set; }

        public DateTime? VigentHasta { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}
