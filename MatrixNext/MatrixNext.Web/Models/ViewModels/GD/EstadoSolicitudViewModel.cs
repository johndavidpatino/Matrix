using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Web.Models.ViewModels.GD
{
    public class EstadoSolicitudViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }
    }
}
