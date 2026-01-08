using MatrixNext.Web.Models.PY;
using MatrixNext.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.PY
{
    /// <summary>
    /// Interface para gestión de segmentos de población en trabajos cualitativos.
    /// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 1.2 (PY_SegmentosCuali_Get)
    /// </summary>
    public interface ISegmentosCualiService
    {
        /// <summary>Obtiene todos los segmentos de un trabajo cualitativo</summary>
        Task<List<SegmentosCuali>> ObtenerPorTrabajoAsync(long idTrabajoCuali);

        /// <summary>Obtiene un segmento por ID</summary>
        Task<SegmentosCuali?> ObtenerPorIdAsync(long id);

        /// <summary>Crea un nuevo segmento</summary>
        Task<ResultVM<long>> CrearAsync(SegmentosCuali segmento, long idUsuario);

        /// <summary>Actualiza un segmento existente</summary>
        Task<ResultVM<bool>> ActualizarAsync(SegmentosCuali segmento, long idUsuario);

        /// <summary>Elimina (soft delete) un segmento</summary>
        Task<ResultVM<bool>> EliminarAsync(long idSegmento, long idUsuario);

        /// <summary>Duplica un segmento</summary>
        Task<ResultVM<long>> DuplicarAsync(long idSegmentoOriginal, long idUsuario);

        /// <summary>Obtiene el total de participantes requeridos por trabajo</summary>
        Task<int> ObtenerTotalParticipantesPorTrabajoAsync(long idTrabajoCuali);
    }
}
