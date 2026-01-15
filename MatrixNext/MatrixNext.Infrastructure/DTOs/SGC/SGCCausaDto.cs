using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Infrastructure.DTOs.SGC
{
    /// <summary>
    /// DTO para Causa en Acción de Mejora
    /// </summary>
    public class SGCCausaDto
    {
        public int CausaId { get; set; }
        public int AccionMejoraId { get; set; }

        [Required(ErrorMessage = "La descripción de causa es requerida")]
        [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
        public string DescripcionCausa { get; set; }
    }

    /// <summary>
    /// DTO para crear causa
    /// </summary>
    public class SGCCausaCreateDto
    {
        [Required(ErrorMessage = "La descripción de causa es requerida")]
        [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
        public string DescripcionCausa { get; set; }
    }
}
