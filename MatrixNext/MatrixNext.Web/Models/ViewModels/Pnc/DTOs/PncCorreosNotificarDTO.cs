namespace MatrixNext.Web.Models.ViewModels.Pnc.DTOs
{
    /// <summary>
    /// DTO para resultado del SP PNC_Productos_CorreosNotificar
    /// Devuelve emails de personas a notificar
    /// </summary>
    public class PncCorreosNotificarDTO
    {
        public long IdUsuario { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Email { get; set; }
        public string? TipoNotificacion { get; set; }  // "Reporta", "Responsable", "Seguimiento", "CC"
    }
}
