using System;
using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.Models.OP
{
    /// <summary>
    /// DTO para revisión IPS por tarea
    /// Mapea a OP_IPS_Revision_Get_Result
    /// </summary>
    public class IpsRevisionDto
    {
        [Display(Name = "ID")]
        public long Id { get; set; }

        [Display(Name = "Trabajo")]
        public long TrabajoId { get; set; }

        [Display(Name = "Pregunta")]
        [Required(ErrorMessage = "La pregunta es requerida")]
        public string Pregunta { get; set; } = string.Empty;

        [Display(Name = "Observación")]
        [StringLength(1000, ErrorMessage = "La observación no puede exceder 1000 caracteres")]
        public string? Observacion { get; set; }

        [Display(Name = "Descripción Observación")]
        [StringLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
        public string? DescripcionObservacion { get; set; }

        [Display(Name = "Respuesta Programador")]
        [StringLength(2000, ErrorMessage = "La respuesta no puede exceder 2000 caracteres")]
        public string? RespuestaProgramador { get; set; }

        [Display(Name = "Tipo de Tarea")]
        public string? TipoTarea { get; set; }

        [Display(Name = "Registrado Por")]
        public string? RegistradoPor { get; set; }

        [Display(Name = "Fecha Registro")]
        public DateTime? FechaRegistro { get; set; }

        [Display(Name = "Modificado Por")]
        public string? ModificadoPor { get; set; }

        [Display(Name = "Fecha Modificación")]
        public DateTime? FechaModificacion { get; set; }
    }

    /// <summary>
    /// DTO para crear/actualizar revisión IPS
    /// </summary>
    public class IpsRevisionCreateUpdateDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "El trabajo es requerido")]
        public long TrabajoId { get; set; }

        [Required(ErrorMessage = "La pregunta es requerida")]
        [StringLength(500, ErrorMessage = "La pregunta no puede exceder 500 caracteres")]
        public string Pregunta { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "La observación no puede exceder 1000 caracteres")]
        public string? Observacion { get; set; }

        [StringLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
        public string? DescripcionObservacion { get; set; }

        [StringLength(2000, ErrorMessage = "La respuesta no puede exceder 2000 caracteres")]
        public string? RespuestaProgramador { get; set; }

        [Display(Name = "Tipo de Tarea")]
        public string? TipoTarea { get; set; }
    }
}
