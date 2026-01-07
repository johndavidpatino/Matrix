using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Services.CORE
{
    public interface IIndicadoresCumplimientoService
    {
        Task<ResultVM<IndicadoresResumenDTO>> ObtenerResumenIndicadoresAsync();
        Task<ResultVM<List<IndicadorPorGerenteDTO>>> ObtenerIndicadoresPorGerenteAsync();
        Task<ResultVM<List<IndicadorPorTipoHiloDTO>>> ObtenerIndicadoresPorTipoHiloAsync();
    }

    public class IndicadoresResumenDTO
    {
        public decimal PorcentajeCumplimiento { get; set; }
        public decimal PorcentajeAtrasadas { get; set; }
        public int TotalTareasCompletadas { get; set; }
        public int TotalTareasAtrasadas { get; set; }
        public decimal PromedioDiasCompletacion { get; set; }
    }

    public class IndicadorPorGerenteDTO
    {
        public long IdGerenteProyectos { get; set; }
        public string? NombreGerente { get; set; }
        public int TotalTareas { get; set; }
        public int TareasCompletadas { get; set; }
        public decimal PorcentajeCumplimiento { get; set; }
        public int TareasAtrasadas { get; set; }
    }

    public class IndicadorPorTipoHiloDTO
    {
        public int IdTipoHilo { get; set; }
        public string? NombreTipoHilo { get; set; }
        public int TotalTareas { get; set; }
        public int TareasCompletadas { get; set; }
        public decimal PorcentajeCumplimiento { get; set; }
        public int TareasAtrasadas { get; set; }
    }
}
