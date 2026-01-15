using MatrixNext.Data.DTOs.ES;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Adapters.ES
{
    /// <summary>
    /// Interface para acceso a datos de Brief Diseño Muestral
    /// </summary>
    public interface IESBriefDisenoMuestralAdapter
    {
        /// <summary>
        /// Obtiene todos los briefs de diseño muestral
        /// SP: ES_BriefDisenoMuestral_Get
        /// </summary>
        Task<IEnumerable<ESBriefDisenoMuestralOutputDto>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene briefs de diseño muestral por propuesta
        /// SP: ES_BriefDisenoMuestral_Get con filtro
        /// </summary>
        Task<IEnumerable<ESBriefDisenoMuestralOutputDto>> ObtenerPorPropuestaAsync(long propuestaId);

        /// <summary>
        /// Obtiene briefs de diseño muestral pendientes (sin diseño)
        /// </summary>
        Task<IEnumerable<ESBriefDisenoMuestralOutputDto>> ObtenerPendientesAsync();

        /// <summary>
        /// Obtiene un brief por ID
        /// </summary>
        Task<ESBriefDisenoMuestralOutputDto> ObtenerPorIdAsync(long id);

        /// <summary>
        /// Crea un nuevo brief de diseño muestral
        /// SP: ES_BriefDisenoMuestral_Add
        /// </summary>
        Task<long> CrearAsync(ESBriefDisenoMuestralInputDto dto, long usuarioId);

        /// <summary>
        /// Actualiza un brief de diseño muestral
        /// SP: ES_BriefDisenoMuestral_Edit
        /// </summary>
        Task ActualizarAsync(long id, ESBriefDisenoMuestralInputDto dto);

        /// <summary>
        /// Elimina un brief de diseño muestral
        /// SP: ES_BriefDisenoMuestral_Del
        /// </summary>
        Task EliminarAsync(long id);
    }
}
