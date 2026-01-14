using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Detalles del cuestionario para una cotización
    /// Mapea desde Excel Entradas: duracion, penetracion, flags de procesos
    /// </summary>
    [Table("eq_questionnaire")]
    public class EqQuestionnaire
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuoteHeaderId { get; set; }

        [ForeignKey(nameof(QuoteHeaderId))]
        public virtual EqQuoteHeader? QuoteHeader { get; set; }

        [Range(5, 60)]
        public int DuracionMinutos { get; set; }

        [StringLength(100)]
        public string? PenetracionLabel { get; set; } // Mas82, 75-82, etc.

        public decimal? PenetracionValor { get; set; }

        public int PreguntasAbiertas { get; set; } = 0;

        public int PreguntasAbiertasMultiples { get; set; } = 0;

        [StringLength(500)]
        public string? OtrosProcesos { get; set; }

        public bool TopLine { get; set; }

        [StringLength(50)]
        public string? DataCleaning { get; set; } // Total, Parcial, No

        public bool ASCII { get; set; }

        public bool ScriptReclutamiento { get; set; }

        public bool Scripting { get; set; }

        [StringLength(50)]
        public string? TipoScript { get; set; } // Nuevo, Duplicado, Reutilizacion

        public bool Codificacion { get; set; }

        public bool Procesamiento { get; set; }

        public int NumProcesamientos { get; set; } = 1;

        public bool ProcesoEstadistico { get; set; }

        [StringLength(50)]
        public string? ClasePrueba { get; set; } // Monodica, Monodica secuencial, No aplica

        public bool Refrigeracion { get; set; }

        public decimal? CompraProducto { get; set; }

        [StringLength(100)]
        public string? EtiquetadoTipo { get; set; } // blind, sin blind, etc.

        public bool Embalaje { get; set; }

        public int ProductosATestear { get; set; } = 1;

        public int ProductosPorRespondiente { get; set; } = 1;

        public int PatinadoresPorCiudad { get; set; } = 0;

        public bool Siembra { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}

