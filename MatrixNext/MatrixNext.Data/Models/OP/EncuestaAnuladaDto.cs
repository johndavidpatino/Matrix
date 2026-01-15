using System;
using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.Models.OP
{
    /// <summary>
    /// DTO para encuestas anuladas
    /// </summary>
    public class EncuestaAnuladaDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "El trabajo es requerido")]
        public long TrabajoId { get; set; }

        [Required(ErrorMessage = "El número de encuesta es requerido")]
        [Display(Name = "Número de Encuesta")]
        public long NumeroEncuesta { get; set; }

        [Required(ErrorMessage = "La observación es requerida")]
        [StringLength(500, ErrorMessage = "La observación no puede exceder 500 caracteres")]
        [Display(Name = "Observación")]
        public string Observacion { get; set; } = string.Empty;

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Usuario")]
        public long UsuarioId { get; set; }

        [Display(Name = "Unidad")]
        public long UnidadId { get; set; }

        // Campos de visualización
        public string? NombreTrabajo { get; set; }
        public string? NombreUsuario { get; set; }
        public string? NombreUnidad { get; set; }
    }

    /// <summary>
    /// DTO para activación de encuestas
    /// </summary>
    public class ActivacionEncuestaDto
    {
        [Required(ErrorMessage = "El trabajo es requerido")]
        public long TrabajoId { get; set; }

        [Required(ErrorMessage = "El número de encuesta es requerido")]
        [Display(Name = "Número de Encuesta")]
        public long NumeroEncuesta { get; set; }

        [Required(ErrorMessage = "La observación es requerida")]
        [StringLength(500, ErrorMessage = "La observación no puede exceder 500 caracteres")]
        [Display(Name = "Observación")]
        public string Observacion { get; set; } = string.Empty;

        // Campos de visualización
        public string? NombreTrabajo { get; set; }
    }
}
