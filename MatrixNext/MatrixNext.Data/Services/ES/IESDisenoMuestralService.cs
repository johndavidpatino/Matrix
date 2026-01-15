using MatrixNext.Data.DTOs.ES;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.ES
{
    public interface IESDisenoMuestralService
    {
        Task<IEnumerable<ESDisenoMuestralOutputDto>> ObtenerTodosAsync();
        Task<IEnumerable<ESDisenoMuestralOutputDto>> ObtenerPorBriefAsync(long briefId);
        Task<ESDisenoMuestralOutputDto> ObtenerPorIdAsync(long id);
        Task<(bool Success, string Message, long Id)> CrearAsync(ESDisenoMuestralInputDto dto);
        Task<(bool Success, string Message)> ActualizarAsync(long id, ESDisenoMuestralInputDto dto);
        Task<(bool Success, string Message)> EliminarAsync(long id);
    }
}
