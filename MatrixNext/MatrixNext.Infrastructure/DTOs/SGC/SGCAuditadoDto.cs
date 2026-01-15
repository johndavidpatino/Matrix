using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Infrastructure.DTOs.SGC
{
    /// <summary>
    /// DTO para Persona Auditada en una Auditoría
    /// Mapea desde SGC_AI_AuditadoResult
    /// </summary>
    public class SGCAuditadoDto
    {
        public int Id { get; set; }
        public int AuditadoId { get; set; }
        public int SGC_AI_AuditoriaId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }

        public string NombreCompleto => $"{Nombres} {Apellidos}";
    }

    /// <summary>
    /// DTO para crear/asignar auditado
    /// </summary>
    public class SGCAuditadoCreateDto
    {
        [Required(ErrorMessage = "El auditado es requerido")]
        public int AuditadoId { get; set; }
    }
}
