using MatrixNext.Web.Models.PY;
using MatrixNext.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.PY
{
    /// <summary>
    /// Interface para gestión de trabajos cualitativos.
    /// Proporciona operaciones CRUD y consultas específicas para trabajos de investigación cualitativa.
    /// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 1.1 (TrabajosCualitativos.aspx.vb)
    /// </summary>
    public interface ITrabajosCualiService
    {
        /// <summary>Obtiene todos los trabajos cualitativos de un proyecto</summary>
        Task<List<TrabajosCuali>> ObtenerPorProyectoAsync(long idProyecto);

        /// <summary>Obtiene un trabajo cualitativo por ID</summary>
        Task<TrabajosCuali> ObtenerPorIdAsync(long id);

        /// <summary>Obtiene trabajos cualitativos filtrados por estado</summary>
        Task<List<TrabajosCuali>> ObtenerPorEstadoAsync(string estado);

        /// <summary>Obtiene trabajos cualitativos asignados a un coordinador</summary>
        Task<List<TrabajosCuali>> ObtenerPorCoordinadorAsync(long idCoordinador);

        /// <summary>Crea un nuevo trabajo cualitativo</summary>
        Task<ResultVM<long>> CrearAsync(TrabajosCuali trabajo, long idUsuario);

        /// <summary>Actualiza un trabajo cualitativo existente</summary>
        Task<ResultVM<bool>> ActualizarAsync(TrabajosCuali trabajo, long idUsuario);

        /// <summary>Cambia el estado de un trabajo cualitativo</summary>
        Task<ResultVM<bool>> CambiarEstadoAsync(long idTrabajo, string nuevoEstado, long idUsuario, string observacion = null);

        /// <summary>Elimina (soft delete) un trabajo cualitativo</summary>
        Task<ResultVM<bool>> EliminarAsync(long idTrabajo, long idUsuario);

        /// <summary>Duplica un trabajo cualitativo con sus segmentos</summary>
        Task<ResultVM<long>> DuplicarAsync(long idTrabajoOriginal, string nuevoNombre, long idUsuario);

        /// <summary>Valida si un trabajo cualitativo puede ser eliminado</summary>
        Task<ResultVM<bool>> ValidarEliminacionAsync(long idTrabajo);
    }
}
