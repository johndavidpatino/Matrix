using MatrixNext.Web.Models.PY;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Services.PY
{
    public interface IAsignacionesProyectosService
    {
        /// <summary>
        /// Obtiene asignaciones existentes con filtro opcional por proyecto
        /// </summary>
        Task<List<AsignacionProyecto>> ObtenerAsignacionesAsync(long? idProyecto);

        /// <summary>
        /// Obtiene proyectos sin gerente asignado o para reasignación
        /// </summary>
        Task<ResultVM<List<dynamic>>> ObtenerProyectosXAsignarAsync(int idUnidad, long idUsuario);

        /// <summary>
        /// Obtiene proyectos para reasignación
        /// </summary>
        Task<ResultVM<List<dynamic>>> ObtenerProyectosXReasignarAsync(int idUnidad, string? filtroNombre, long idUsuario);

        /// <summary>
        /// Obtiene gerentes de proyectos disponibles para asignación
        /// </summary>
        Task<ResultVM<List<dynamic>>> ObtenerGerentesDisponiblesAsync(int idUnidad, long idUsuario);

        /// <summary>
        /// Asigna un gerente a un proyecto
        /// </summary>
        Task<ResultVM<bool>> AsignarGerenteAsync(long idProyecto, long idGerenteProyecto, long idUsuarioActual, string? observaciones = null);

        /// <summary>
        /// Reasigna un gerente a un proyecto
        /// </summary>
        Task<ResultVM<bool>> ReasignarGerenteAsync(long idProyecto, long idGerenteNuevo, long idUsuarioActual, string? observaciones = null);

        /// <summary>
        /// Obtiene historial de asignaciones de un proyecto
        /// </summary>
        Task<ResultVM<List<AsignacionProyecto>>> ObtenerHistorialAsync(long idProyecto, long idUsuario);

        /// <summary>
        /// Valida que el usuario actual tenga permiso para asignar/reasignar
        /// </summary>
        Task<ResultVM<bool>> ValidarPermisosAsync(long idUsuario);
    }
}
