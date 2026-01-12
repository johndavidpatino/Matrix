using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Web.Models.ViewModels.Pnc
{
    /// <summary>
    /// ViewModel para listado de PNC (grid de resultados)
    /// Origen: SP PNC_ObtenerProductoNoConforme
    /// </summary>
    public class ProductoNoConformeListadoVM
    {
        public int Id { get; set; }

        [Display(Name = "JobBook")]
        public string JobBook { get; set; } = string.Empty;

        [Display(Name = "Estudio")]
        public string? NombreEstudio { get; set; }

        [Display(Name = "Cliente")]
        public string? NombreCliente { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Reclamo")]
        public DateTime FechaReclamo { get; set; }

        [Display(Name = "Reporta")]
        public string? NombreReporta { get; set; }

        [Display(Name = "Fuente")]
        public string? FuenteReclamo { get; set; }

        [Display(Name = "Categoría")]
        public string? Categoria { get; set; }

        [Display(Name = "Estado")]
        public string Estado => Cerrado ? "Cerrado" : "Abierto";

        public bool Cerrado { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Cierre")]
        public DateTime? FechaCierre { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(100)]
        public string DescripcionCorta { get; set; } = string.Empty;

        // Indicadores
        public int TotalCausas { get; set; }
        public int TotalAcciones { get; set; }
        public int AccionesPendientes { get; set; }

        public string EstadoClass => Cerrado ? "success" : (AccionesPendientes > 0 ? "warning" : "info");
    }
}
