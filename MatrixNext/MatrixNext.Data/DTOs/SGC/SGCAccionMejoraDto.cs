using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.SGC
{
    /// <summary>
    /// DTO para Acción de Mejora
    /// Mapea desde ACM_AccionesMejora tabla
    /// </summary>
    public class SGCAccionMejoraDto
    {
        public int AccionMejoraId { get; set; }

        [Required(ErrorMessage = "La descripción de acción es requerida")]
        [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
        public string DescripcionAccion { get; set; }

        [Required(ErrorMessage = "La fecha del incidente es requerida")]
        public DateTime FechaIncidente { get; set; }

        [Required(ErrorMessage = "El usuario que reporta es requerido")]
        public long UsuarioReporta { get; set; }

        public string NombreUsuarioReporta { get; set; }

        [Required(ErrorMessage = "El proceso es requerido")]
        public int ProcesoId { get; set; }

        public string NombreProceso { get; set; }

        [Required(ErrorMessage = "El usuario responsable es requerido")]
        public long UsuarioResponsable { get; set; }

        public string NombreUsuarioResponsable { get; set; }

        [StringLength(1000)]
        public string Descripcion { get; set; }

        [StringLength(1000)]
        public string Correccion { get; set; }

        public int? FuenteNoConformidadId { get; set; }
        public int? FuenteId { get; set; }

        // Relaciones
        public List<SGCCausaDto> Causas { get; set; } = new();
        public List<SGCPlanAccionDto> PlanesAccion { get; set; } = new();

        // Campos de auditoría
        public bool IsDeleted { get; set; }
        public DateTime FechaRegistro { get; set; }
        public long RegistradoPor { get; set; }
    }

    /// <summary>
    /// DTO para crear acción de mejora
    /// </summary>
    public class SGCAccionMejoraCreateDto
    {
        [Required(ErrorMessage = "La descripción de acción es requerida")]
        [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
        public string DescripcionAccion { get; set; }

        [Required(ErrorMessage = "La fecha del incidente es requerida")]
        public DateTime FechaIncidente { get; set; }

        [Required(ErrorMessage = "El usuario que reporta es requerido")]
        public long UsuarioReporta { get; set; }

        [Required(ErrorMessage = "El proceso es requerido")]
        public int ProcesoId { get; set; }

        [Required(ErrorMessage = "El usuario responsable es requerido")]
        public long UsuarioResponsable { get; set; }

        [StringLength(1000)]
        public string Descripcion { get; set; }

        [StringLength(1000)]
        public string Correccion { get; set; }

        public int? FuenteNoConformidadId { get; set; }
        public int? FuenteId { get; set; }

        // Relaciones al crear
        public List<SGCCausaCreateDto> Causas { get; set; } = new();
        public List<SGCPlanAccionCreateDto> PlanesAccion { get; set; } = new();
    }

    /// <summary>
    /// DTO para actualizar acción de mejora
    /// </summary>
    public class SGCAccionMejoraUpdateDto
    {
        public int AccionMejoraId { get; set; }

        [Required(ErrorMessage = "La descripción de acción es requerida")]
        [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
        public string DescripcionAccion { get; set; }

        [StringLength(1000)]
        public string Descripcion { get; set; }

        [StringLength(1000)]
        public string Correccion { get; set; }

        [Required(ErrorMessage = "El usuario responsable es requerido")]
        public long UsuarioResponsable { get; set; }

        public int? FuenteNoConformidadId { get; set; }
        public int? FuenteId { get; set; }
    }
}
