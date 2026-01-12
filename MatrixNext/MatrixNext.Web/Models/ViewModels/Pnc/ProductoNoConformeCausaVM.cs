using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Web.Models.ViewModels.Pnc
{
    /// <summary>
    /// ViewModel para causas raíz del Producto No Conforme
    /// Origen: PNC_ProductoNoConformeCausas
    /// Relación: 1 PNC → N Causas
    /// </summary>
    public class ProductoNoConformeCausaVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El IdPNC es requerido")]
        [Display(Name = "PNC")]
        public int IdPNC { get; set; }

        [Required(ErrorMessage = "La causa raíz es requerida")]
        [Display(Name = "Causa Raíz")]
        public string CausaRaiz { get; set; } = string.Empty;

        // Navegación
        public ProductoNoConformeVM? Pnc { get; set; }

        // Lista de acciones por causa
        public List<ProductoNoConformeAccionVM>? Acciones { get; set; }
    }
}
