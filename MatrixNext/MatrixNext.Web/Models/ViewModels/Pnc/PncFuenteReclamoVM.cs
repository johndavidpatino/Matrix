using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Web.Models.ViewModels.Pnc
{
    /// <summary>
    /// ViewModel para catálogo de fuentes de reclamo
    /// Origen: PNC_FuenteReclamo
    /// Ejemplos: Cliente Externo, Cliente Interno, Auditoría Interna, Auditoría Externa
    /// </summary>
    public class PncFuenteReclamoVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(30, ErrorMessage = "Máximo 30 caracteres")]
        [Display(Name = "Fuente de Reclamo")]
        public string Descripcion { get; set; } = string.Empty;
    }
}
