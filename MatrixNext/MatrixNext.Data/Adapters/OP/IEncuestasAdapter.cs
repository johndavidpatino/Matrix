using MatrixNext.Data.Models.OP;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Adapters.OP
{
    /// <summary>
    /// Interfaz para acceso a datos de encuestas (activación/anulación)
    /// </summary>
    public interface IEncuestasAdapter
    {
        /// <summary>
        /// Obtiene lista de encuestas anuladas por trabajo
        /// </summary>
        Task<IEnumerable<EncuestaAnuladaDto>> ObtenerEncuestasAnuladasAsync(long trabajoId);

        /// <summary>
        /// Verifica si una encuesta está anulada
        /// </summary>
        Task<bool> ExisteEncuestaAnuladaAsync(long trabajoId, long numeroEncuesta);

        /// <summary>
        /// Verifica si una encuesta está anulada en gestión de campo
        /// </summary>
        Task<bool> ExisteEncuestaAnuladaGestionCampoAsync(long trabajoId, long numeroEncuesta);

        /// <summary>
        /// Anula una encuesta
        /// </summary>
        Task<long> AnularEncuestaAsync(EncuestaAnuladaDto dto);

        /// <summary>
        /// Anula una encuesta en gestión de campo
        /// </summary>
        Task AnularEncuestaGestionCampoAsync(long trabajoId, long numeroEncuesta, string observacion);

        /// <summary>
        /// Activa (elimina anulación) de una encuesta
        /// </summary>
        Task ActivarEncuestaAsync(long numeroEncuesta, long trabajoId);

        /// <summary>
        /// Actualiza gestión de campo al activar una encuesta
        /// </summary>
        Task ActualizarGestionCampoActivacionAsync(long trabajoId, long numeroEncuesta, string observacion, long usuarioId);
    }
}
