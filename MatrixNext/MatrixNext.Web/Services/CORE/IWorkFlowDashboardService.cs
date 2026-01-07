using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Services.CORE
{
    /// <summary>
    /// Interface para servicio de Dashboard de Tráfico de Tareas (WorkFlow)
    /// Proporciona métricas de carga operacional y estado de procesos
    /// </summary>
    public interface IWorkFlowDashboardService
    {
        /// <summary>
        /// Obtiene resumen general de tareas por estado
        /// </summary>
        Task<ResultVM<WorkFlowResumenDTO>> ObtenerResumenGeneralAsync();

        /// <summary>
        /// Obtiene tareas agrupadas por estado con información de atraso
        /// </summary>
        Task<ResultVM<List<TareasPorEstadoDTO>>> ObtenerTareasPorEstadoAsync(
            int? idTipoHilo = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null);

        /// <summary>
        /// Obtiene tareas agrupadas por prioridad
        /// </summary>
        Task<ResultVM<List<TareasPorPrioridadDTO>>> ObtenerTareasPorPrioridadAsync(
            int? idTipoHilo = null);

        /// <summary>
        /// Obtiene tareas próximas a vencer (alarma de vencimiento)
        /// </summary>
        Task<ResultVM<List<TareaProximaAVencerDTO>>> ObtenerTareasProximasAVencerAsync(
            int diasAlerta = 3);

        /// <summary>
        /// Obtiene detalle de tareas con filtros avanzados
        /// </summary>
        Task<ResultVM<List<TareaDetalleDTO>>> ObtenerDetalleTareasAsync(
            int? idTipoHilo = null,
            string? estado = null,
            int? prioridad = null,
            string? busqueda = null,
            int page = 1,
            int pageSize = 20);

        /// <summary>
        /// Obtiene tareas asignadas a usuarios específicos
        /// </summary>
        Task<ResultVM<List<TareasPorUsuarioDTO>>> ObtenerTareasPorUsuarioAsync();
    }

    #region DTOs Dashboard

    public class WorkFlowResumenDTO
    {
        public int TotalTareas { get; set; }
        public int TareasActivas { get; set; }
        public int TareasCompletadas { get; set; }
        public int TareasAnuladas { get; set; }
        public int TareasAtrasadas { get; set; }
        public int TareasProximasAvencer { get; set; }
        public Dictionary<string, int> TareasPorEstado { get; set; } = new();
        public Dictionary<int, int> TareasPorPrioridad { get; set; } = new();
    }

    public class TareasPorEstadoDTO
    {
        public string? Estado { get; set; }
        public int CantidadTareas { get; set; }
        public decimal PorcentajeTotal { get; set; }
        public int TareasAtrasadas { get; set; }
        public int TareasProximasAvencer { get; set; }
    }

    public class TareasPorPrioridadDTO
    {
        public int Prioridad { get; set; }
        public string? NombrePrioridad { get; set; }
        public int CantidadTareas { get; set; }
        public int TareasAtrasadas { get; set; }
    }

    public class TareaProximaAVencerDTO
    {
        public long Id { get; set; }
        public string? TareaNombre { get; set; }
        public long? IdTrabajo { get; set; }
        public string? TrabajoNombre { get; set; }
        public string? Estado { get; set; }
        public int Prioridad { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int? DiasHastaVencer { get; set; }
        public bool UrgenteCritica { get; set; }
        public int UsuariosAsignados { get; set; }
    }

    public class TareaDetalleDTO
    {
        public long Id { get; set; }
        public string? TareaNombre { get; set; }
        public long? IdTrabajo { get; set; }
        public string? TrabajoNombre { get; set; }
        public long IdTarea { get; set; }
        public int IdTipoHilo { get; set; }
        public string? Estado { get; set; }
        public int Prioridad { get; set; }
        public string? NombrePrioridad { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public bool Atrasada { get; set; }
        public int? DiasAtraso { get; set; }
        public int UsuariosAsignados { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    public class TareasPorUsuarioDTO
    {
        public long IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public int TotalAsignaciones { get; set; }
        public int TareasActivas { get; set; }
        public int TareasCompletadas { get; set; }
        public int TareasAtrasadas { get; set; }
        public int TareasAlta { get; set; }
    }

    #endregion
}
