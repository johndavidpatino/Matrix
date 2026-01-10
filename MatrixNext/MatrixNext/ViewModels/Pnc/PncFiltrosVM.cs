using System.ComponentModel.DataAnnotations;

namespace MatrixNext.ViewModels.Pnc
{
    /// <summary>
    /// ViewModel para filtros de búsqueda de PNC
    /// Usado en Index para filtrar grid
    /// </summary>
    public class PncFiltrosVM
    {
        [Display(Name = "JobBook")]
        [StringLength(15)]
        public string? JobBook { get; set; }

        [Display(Name = "Estudio")]
        public string? NombreEstudio { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Desde")]
        public DateTime? FechaDesde { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Hasta")]
        public DateTime? FechaHasta { get; set; }

        [Display(Name = "Fuente Reclamo")]
        public int? IdFuenteReclamo { get; set; }

        [Display(Name = "Categoría")]
        public int? IdCategoria { get; set; }

        [Display(Name = "Estado")]
        public EstadoPncEnum? Estado { get; set; }

        [Display(Name = "Reporta")]
        public long? IdReporta { get; set; }

        [Display(Name = "Solo Vencidos")]
        public bool SoloVencidos { get; set; } = false;

        // Para paginación
        public int PaginaActual { get; set; } = 1;
        public int RegistrosPorPagina { get; set; } = 20;

        // Para resultados
        public List<ProductoNoConformeListadoVM> Resultados { get; set; } = new();
        public int TotalRegistros { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / RegistrosPorPagina);
    }

    /// <summary>
    /// Enum para filtro de estado
    /// </summary>
    public enum EstadoPncEnum
    {
        [Display(Name = "Todos")]
        Todos = 0,

        [Display(Name = "Abiertos")]
        Abiertos = 1,

        [Display(Name = "Cerrados")]
        Cerrados = 2
    }
}
