using MatrixNext.Data.DTOs.ES;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Adapters.ES
{
    /// <summary>
    /// Interface para acceso a datos de Diseño Muestral
    /// </summary>
    public interface IESDisenoMuestralAdapter
    {
        /// <summary>
        /// Obtiene todos los diseños muestrales
        /// SP: ES_DisenoMuestral_Get
        /// </summary>
        Task<IEnumerable<ESDisenoMuestralOutputDto>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene diseños muestrales por brief
        /// </summary>
        Task<IEnumerable<ESDisenoMuestralOutputDto>> ObtenerPorBriefAsync(long briefId);

        /// <summary>
        /// Obtiene un diseño muestral por ID
        /// </summary>
        Task<ESDisenoMuestralOutputDto> ObtenerPorIdAsync(long id);

        /// <summary>
        /// Crea un nuevo diseño muestral
        /// SP: ES_DisenoMuestral_Add
        /// </summary>
        Task<long> CrearAsync(ESDisenoMuestralInputDto dto);

        /// <summary>
        /// Actualiza un diseño muestral
        /// SP: ES_DisenoMuestral_Edit
        /// </summary>
        Task ActualizarAsync(long id, ESDisenoMuestralInputDto dto);

        /// <summary>
        /// Elimina un diseño muestral
        /// SP: ES_DisenoMuestral_Del
        /// </summary>
        Task EliminarAsync(long id);
    }
}
