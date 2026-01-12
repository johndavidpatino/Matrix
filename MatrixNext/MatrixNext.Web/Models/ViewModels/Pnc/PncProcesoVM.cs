using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Web.Models.ViewModels.Pnc
{
    /// <summary>
    /// ViewModel para catálogo de procesos
    /// Origen: PNC_Procesos
    /// Usado en sistema avanzado PNC_Productos
    /// </summary>
    public class PncProcesoVM
    {
        public byte Id { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [Display(Name = "Proceso")]
        public string Descripcion { get; set; } = string.Empty;
    }
}
