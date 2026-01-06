namespace MatrixNext.Web.Services
{
    /// <summary>
    /// Interface para auditoría de cambios
    /// Ref: MATRIZ_PERMISOS_ROLES.md § 6 (CORE_ObservacionesTareas)
    /// </summary>
    public interface IAuditoriaService
    {
        Task LogearAsync(AuditoriaVM auditoria);
    }

    public class AuditoriaVM
    {
        public string Entidad { get; set; }
        public long? EntidadId { get; set; }
        public string Accion { get; set; } // "Create", "Update", "Delete", "Upload", "Download"
        public string Detalles { get; set; }
        public string RutaArchivo { get; set; }
        public long? IdUsuario { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
