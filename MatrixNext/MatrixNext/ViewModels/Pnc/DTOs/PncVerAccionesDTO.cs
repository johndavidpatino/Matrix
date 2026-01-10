namespace MatrixNext.ViewModels.Pnc.DTOs
{
    /// <summary>
    /// DTO para resultado del SP PNC_ProductoNoConformeAcciones_Get
    /// o vista PNC_VerProductoNoConformeDetalle
    /// </summary>
    public class PncVerAccionesDTO
    {
        public int Id { get; set; }
        public int IdPNC { get; set; }
        public int IdCausa { get; set; }
        public int? TipoAccion { get; set; }
        public string? Accion { get; set; }
        public DateTime? FechaPlaneada { get; set; }
        public DateTime? FechaEjecucion { get; set; }
        public int? IdResponsableAccion { get; set; }
        public int? IdResponsableSeguimiento { get; set; }
        public string? EvidenciaCierre { get; set; }
        public bool? PermiteActualizar { get; set; }

        // Campos calculados del SP (joins)
        public string? CausaRaiz { get; set; }
        public string? NombreTipoAccion { get; set; }
        public string? NombreResponsableAccion { get; set; }
        public string? NombreResponsableSeguimiento { get; set; }
    }
}
