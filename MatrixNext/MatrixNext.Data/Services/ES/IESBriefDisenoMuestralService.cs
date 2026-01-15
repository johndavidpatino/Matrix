using MatrixNext.Data.DTOs.ES;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.ES
{
    /// <summary>
    /// Interface para servicio de Brief Diseño Muestral
    /// Contiene lógica de negocio y validaciones
    /// </summary>
    public interface IESBriefDisenoMuestralService
    {
        Task<IEnumerable<ESBriefDisenoMuestralOutputDto>> ObtenerTodosAsync();
        Task<IEnumerable<ESBriefDisenoMuestralOutputDto>> ObtenerPorPropuestaAsync(long propuestaId);
        Task<IEnumerable<ESBriefDisenoMuestralOutputDto>> ObtenerPendientesAsync();
        Task<ESBriefDisenoMuestralOutputDto> ObtenerPorIdAsync(long id);
        Task<(bool Success, string Message, long Id)> CrearAsync(ESBriefDisenoMuestralInputDto dto, long usuarioId);
        Task<(bool Success, string Message)> ActualizarAsync(long id, ESBriefDisenoMuestralInputDto dto);
        Task<(bool Success, string Message)> EliminarAsync(long id);
    }
}
