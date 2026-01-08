using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MatrixNext.Web.Services.OP;

namespace MatrixNext.Web.ViewModels.OP;

public sealed class OpCargaFormModel
{
    [Display(Name = "Tipo de carga")]
    public OpCargaTipo Tipo { get; set; } = OpCargaTipo.CatiRMC;

    [DataType(DataType.Upload)]
    [Display(Name = "Archivo Excel (.xls/.xlsx)")]
    public IFormFile? Archivo { get; set; }
}
