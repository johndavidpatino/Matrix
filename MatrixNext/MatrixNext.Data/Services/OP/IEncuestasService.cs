using MatrixNext.Data.Models.OP;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.OP
{
    /// <summary>
    /// Interfaz para servicio de gestión de encuestas (activación/anulación)
    /// </summary>
    public interface IEncuestasService
    {
        /// <summary>
        /// Obtiene lista de encuestas anuladas por trabajo
        /// </summary>
        Task<IEnumerable<EncuestaAnuladaDto>> ObtenerEncuestasAnuladasAsync(long trabajoId);

        /// <summary>
        /// Anula una encuesta con validaciones de negocio
        /// </summary>
        Task<(bool Success, string Message, long Id)> AnularEncuestaAsync(EncuestaAnuladaDto dto, long usuarioId, long unidadId);

        /// <summary>
        /// Activa una encuesta (elimina anulación) con validaciones
        /// </summary>
        Task<(bool Success, string Message)> ActivarEncuestaAsync(long trabajoId, long numeroEncuesta, string observacion, long usuarioId);
    }
}
