using System.ComponentModel.DataAnnotations;

namespace MatrixNext.ViewModels.Pnc
{
    /// <summary>
    /// ViewModel para catálogo de categorías PNC
    /// Origen: PNC_Categorias (ISO 9001)
    /// </summary>
    public class PncCategoriaVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(72, ErrorMessage = "Máximo 72 caracteres")]
        [Display(Name = "Categoría")]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "Unidad")]
        public int? IdUnidad { get; set; }

        [Display(Name = "Rol")]
        public int? IdRol { get; set; }

        // Navegación
        public string? NombreUnidad { get; set; }
        public string? NombreRol { get; set; }
    }
}
