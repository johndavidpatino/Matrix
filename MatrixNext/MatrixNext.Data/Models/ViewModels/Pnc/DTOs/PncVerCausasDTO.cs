namespace MatrixNext.Data.Models.ViewModels.Pnc.DTOs
{
    /// <summary>
    /// DTO para resultado del SP PNC_ProductoNoConformeCausas_Get
    /// o vista PNC_VerProductoNoConformeCausas
    /// </summary>
    public class PncVerCausasDTO
    {
        public int Id { get; set; }
        public int IdPNC { get; set; }
        public string? CausaRaiz { get; set; }
        
        // Información del PNC padre (del join)
        public string? JobBook { get; set; }
        public string? DescripcionPNC { get; set; }
    }
}
