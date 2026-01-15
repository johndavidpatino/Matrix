using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.SGC
{
    /// <summary>
    /// DTO para Informe del Auditor
    /// Mapea desde SGC_AI_Auditorias_InformeAuditorEntity
    /// </summary>
    public class SGCAuditoriaInformeDto
    {
        public int Id { get; set; }
        public int SGC_AI_AuditoriaId { get; set; }
        public DateTime FechaAuditoria { get; set; }

        [Required(ErrorMessage = "Las fortalezas son requeridas")]
        [StringLength(2000, ErrorMessage = "Máximo 2000 caracteres")]
        public string Fortalezas { get; set; }

        public string ArchivoInformeAuditoriaNombre { get; set; }
        public string ArchivoInformeAuditoriaId { get; set; }
        public int ArchivoInformeAuditoriaTamanoBytes { get; set; }
        public DateTime FechaRegistro { get; set; }
        public long UsuarioRegistra { get; set; }

        // Relaciones
        public List<SGCAuditadoDto> Auditados { get; set; } = new();
        public List<SGCHallazgoDto> Hallazgos { get; set; } = new();
    }

    /// <summary>
    /// DTO para crear/enviar informe auditor
    /// </summary>
    public class SGCAuditoriaInformeCreateDto
    {
        public int AuditoriaId { get; set; }

        [Required(ErrorMessage = "La fecha de auditoría es requerida")]
        public DateTime FechaAuditoria { get; set; }

        [Required(ErrorMessage = "Las fortalezas son requeridas")]
        [StringLength(2000, ErrorMessage = "Máximo 2000 caracteres")]
        public string Fortalezas { get; set; }

        [Required(ErrorMessage = "Debe registrar al menos un auditado")]
        public List<int> AuditadosIds { get; set; } = new();

        [Required(ErrorMessage = "Debe registrar al menos un hallazgo")]
        public List<SGCHallazgoCreateDto> Hallazgos { get; set; } = new();

        /// <summary>
        /// Archivo en base64 (si aplica)
        /// </summary>
        public string ArchivoBase64 { get; set; }
        public string ArchivoNombre { get; set; }
    }
}
