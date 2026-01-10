using System.ComponentModel.DataAnnotations;

namespace MatrixNext.ViewModels.Pnc
{
    /// <summary>
    /// ViewModel para catálogo de tipos de acción
    /// Origen: PNC_TiposDeAccion
    /// Valores: 1=Inmediata, 2=Correctiva, 3=Preventiva
    /// </summary>
    public class PncTipoAccionVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres")]
        [Display(Name = "Tipo de Acción")]
        public string Accion { get; set; } = string.Empty;
    }

    /// <summary>
    /// Enum para tipos de acción (referencia del legacy)
    /// </summary>
    public enum TipoAccionEnum
    {
        [Display(Name = "Inmediata")]
        Inmediata = 1,

        [Display(Name = "Correctiva")]
        Correctiva = 2,

        [Display(Name = "Preventiva")]
        Preventiva = 3
    }
}
