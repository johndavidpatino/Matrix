using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Infrastructure.DTOs.SGC
{
    /// <summary>
    /// DTO para Auditoría Interna - Lectura
    /// Mapea desde SGC_AuditoriaInternaEntity
    /// </summary>
    public class SGCAuditoriaDto
    {
        public int Id { get; set; }
        public long AuditorId { get; set; }
        public string NombreAuditor { get; set; }
        public string AreaAuditada { get; set; }
        public string ProcesoAuditado { get; set; }
        public DateTime FechaLimiteAuditoria { get; set; }
        public DateTime FechaRegistro { get; set; }
        public long UsuarioRegistraId { get; set; }
        
        /// <summary>
        /// Estados: 20=Creada, 30=Diligenciada, 40=Aprobada, 50=Cerrada
        /// </summary>
        public byte SGC_AI_EstadoId { get; set; }
        public string SGC_AI_EstadoAuditoria { get; set; }

        /// <summary>
        /// Normativas a auditar (separadas por |)
        /// Ej: "1;ISO 9001|2;ISO 14001"
        /// </summary>
        public string SGC_NormativasAAuditar { get; set; }

        /// <summary>
        /// Tipos de auditoría (separadas por |)
        /// Ej: "1;Sistemas|2;Procesos"
        /// </summary>
        public string SGC_AI_Tipos { get; set; }

        public int RowNum { get; set; }
        public int TotalRows { get; set; }
    }

    /// <summary>
    /// DTO para Auditoría Interna - Creación
    /// </summary>
    public class SGCAuditoriaCreateDto
    {
        [Required(ErrorMessage = "El auditor es requerido")]
        public long AuditorId { get; set; }

        [Required(ErrorMessage = "El área auditada es requerida")]
        [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
        public string AreaAuditada { get; set; }

        [Required(ErrorMessage = "El proceso auditado es requerido")]
        [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
        public string ProcesoAuditado { get; set; }

        [Required(ErrorMessage = "La fecha límite es requerida")]
        public DateTime FechaLimiteAuditoria { get; set; }

        [Required(ErrorMessage = "Debe seleccionar al menos una normativa")]
        public List<short> NormativasAAuditar { get; set; } = new();

        [Required(ErrorMessage = "Debe seleccionar al menos un tipo de auditoría")]
        public List<short> TiposAuditoria { get; set; } = new();
    }

    /// <summary>
    /// DTO para Auditoría Interna - Actualización Estado
    /// </summary>
    public class SGCAuditoriaUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nuevo estado es requerido")]
        public byte EstadoId { get; set; }
    }
}
