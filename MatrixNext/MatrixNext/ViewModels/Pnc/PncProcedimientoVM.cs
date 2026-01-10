using System.ComponentModel.DataAnnotations;

namespace MatrixNext.ViewModels.Pnc
{
    /// <summary>
    /// ViewModel para catálogo de procedimientos
    /// Origen: PNC_Procedimientos
    /// Usado en sistema avanzado PNC_Productos
    /// </summary>
    public class PncProcedimientoVM
    {
        public byte Id { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [Display(Name = "Procedimiento")]
        public string Descripcion { get; set; } = string.Empty;
    }
}
