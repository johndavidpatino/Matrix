using MatrixNext.Data.DTOs.ES;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Adapters.ES
{
    /// <summary>
    /// Interface para acceso a datos de Metodología de Campo
    /// </summary>
    public interface IESMetodologiaCampoAdapter
    {
        /// <summary>
        /// Obtiene todas las metodologías
        /// SP: ES_MetodologiaCampo_Get
        /// </summary>
        Task<IEnumerable<ESMetodologiaCampoOutputDto>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene metodologías por trabajo
        /// </summary>
        Task<IEnumerable<ESMetodologiaCampoOutputDto>> ObtenerPorTrabajoAsync(long trabajoId);

        /// <summary>
        /// Obtiene metodologías pendientes de aprobación
        /// </summary>
        Task<IEnumerable<ESMetodologiaCampoOutputDto>> ObtenerPendientesAsync();

        /// <summary>
        /// Obtiene una metodología por ID
        /// </summary>
        Task<ESMetodologiaCampoOutputDto> ObtenerPorIdAsync(long id);

        /// <summary>
        /// Crea una nueva metodología de campo
        /// SP: ES_MetodologiaCampo_Add
        /// </summary>
        Task<long> CrearAsync(ESMetodologiaCampoInputDto dto, long usuarioId);

        /// <summary>
        /// Actualiza una metodología de campo
        /// SP: ES_MetodologiaCampo_Edit
        /// </summary>
        Task ActualizarAsync(long id, ESMetodologiaCampoInputDto dto);

        /// <summary>
        /// Elimina una metodología de campo
        /// SP: ES_MetodologiaCampo_Del
        /// </summary>
        Task EliminarAsync(long id);

        /// <summary>
        /// Obtiene número de versiones de metodología por trabajo
        /// SP: ES_MetodologiaCampo_NumVersiones
        /// </summary>
        Task<int> ObtenerNumeroVersionesAsync(long trabajoId);
    }
}
