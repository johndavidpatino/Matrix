using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Services.PY
{
    /// <summary>
    /// Interface para servicio de Dashboard de Proyectos y Trabajos
    /// Proporciona métricas y reportes para gestión operacional
    /// </summary>
    public interface IDashboardService
    {
        /// <summary>
        /// Obtiene resumen general de trabajos agrupados por estado
        /// </summary>
        Task<ResultVM<DashboardResumenDTO>> ObtenerResumenGeneralAsync(int? idUnidad = null);

        /// <summary>
        /// Obtiene trabajos agrupados por gerente de proyectos
        /// </summary>
        Task<ResultVM<List<TrabajosPorGerenteDTO>>> ObtenerTrabajosPorGerenteAsync(
            int? idUnidad = null,
            long? idGerenteProyectos = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null);

        /// <summary>
        /// Obtiene trabajos agrupados por estado
        /// </summary>
        Task<ResultVM<List<TrabajosPorEstadoDTO>>> ObtenerTrabajosPorEstadoAsync(
            int? idUnidad = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null);

        /// <summary>
        /// Obtiene detalle de trabajos con filtros avanzados
        /// </summary>
        Task<ResultVM<List<TrabajoDetalleDTO>>> ObtenerDetalleTrabajosAsync(
            int? idUnidad = null,
            long? idGerenteProyectos = null,
            int? estado = null,
            string? busqueda = null,
            int page = 1,
            int pageSize = 20);
    }

    #region DTOs Dashboard

    public class DashboardResumenDTO
    {
        public int TotalProyectos { get; set; }
        public int TotalTrabajos { get; set; }
        public int TrabajosActivos { get; set; }
        public int TrabajosCerrados { get; set; }
        public int TrabajosAtrasados { get; set; }
        public Dictionary<string, int> TrabajosPorEstado { get; set; } = new();
        public Dictionary<string, int> TrabajosPorUnidad { get; set; } = new();
    }

    public class TrabajosPorGerenteDTO
    {
        public long IdGerenteProyectos { get; set; }
        public string? NombreGerente { get; set; }
        public int TotalTrabajos { get; set; }
        public int TrabajosActivos { get; set; }
        public int TrabajosCompletados { get; set; }
        public int TrabajosAtrasados { get; set; }
    }

    public class TrabajosPorEstadoDTO
    {
        public int IdEstado { get; set; }
        public string? NombreEstado { get; set; }
        public int CantidadTrabajos { get; set; }
        public decimal PorcentajeTotal { get; set; }
    }

    public class TrabajoDetalleDTO
    {
        public long Id { get; set; }
        public string? JobBook { get; set; }
        public string? Nombre { get; set; }
        public long? IdProyecto { get; set; }
        public string? NombreProyecto { get; set; }
        public long? IdGerenteProyectos { get; set; }
        public string? NombreGerente { get; set; }
        public int? IdUnidad { get; set; }
        public string? NombreUnidad { get; set; }
        public int Estado { get; set; }
        public string? NombreEstado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaInicioCampo { get; set; }
        public DateTime? FechaFinalizacionCampo { get; set; }
        public bool Atrasado { get; set; }
        public int? DiasAtraso { get; set; }
    }

    #endregion
}
