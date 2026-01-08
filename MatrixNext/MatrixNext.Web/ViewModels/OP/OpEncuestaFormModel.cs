using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Web.ViewModels.OP;

public sealed class OpEncuestaFormModel
{
    [Required]
    [Display(Name = "Trabajo")]
    public long TrabajoId { get; set; }

    [Required]
    [Display(Name = "Encuesta")]
    public decimal NumeroEncuesta { get; set; }

    [Display(Name = "Observación")]
    public string Observacion { get; set; } = string.Empty;
}
