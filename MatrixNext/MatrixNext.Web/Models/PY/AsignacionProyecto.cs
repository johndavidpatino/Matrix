using MatrixNext.Web.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.PY
{
    /// <summary>
    /// Registra asignación de proyectos a gerentes de proyectos
    /// </summary>
    [Table("PY_AsignacionProyectos", Schema = "dbo")]
    public class AsignacionProyecto : BaseEntity
    {
        /// <summary>
        /// Proyecto siendo asignado
        /// </summary>
        [Required]
        public long IdProyecto { get; set; }

        /// <summary>
        /// Gerente de proyectos asignado (ID de usuario)
        /// </summary>
        [Required]
        public long IdGerenteProyecto { get; set; }

        /// <summary>
        /// Nombre del gerente para auditoría
        /// </summary>
        [MaxLength(200)]
        public string? NombreGerenteProyecto { get; set; }

        /// <summary>
        /// Fecha de la asignación
        /// </summary>
        public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Tipo de asignación: Inicial, Reasignación
        /// </summary>
        [MaxLength(50)]
        public string? TipoAsignacion { get; set; } = "Inicial";

        /// <summary>
        /// Observaciones sobre la asignación
        /// </summary>
        public string? Observaciones { get; set; }

        /// <summary>
        /// Gerente anterior (si es reasignación)
        /// </summary>
        public long? IdGerentePrevio { get; set; }

        /// <summary>
        /// Nombre del gerente anterior para auditoría
        /// </summary>
        [MaxLength(200)]
        public string? NombreGerentePrevio { get; set; }

        // Navegación
        [ForeignKey(nameof(IdProyecto))]
        public virtual Proyecto? Proyecto { get; set; }
    }
}
