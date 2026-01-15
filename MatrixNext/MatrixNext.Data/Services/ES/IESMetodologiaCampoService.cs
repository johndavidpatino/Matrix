using MatrixNext.Data.DTOs.ES;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.ES
{
    public interface IESMetodologiaCampoService
    {
        Task<IEnumerable<ESMetodologiaCampoOutputDto>> ObtenerTodosAsync();
        Task<IEnumerable<ESMetodologiaCampoOutputDto>> ObtenerPorTrabajoAsync(long trabajoId);
        Task<IEnumerable<ESMetodologiaCampoOutputDto>> ObtenerPendientesAsync();
        Task<ESMetodologiaCampoOutputDto> ObtenerPorIdAsync(long id);
        Task<(bool Success, string Message, long Id)> CrearAsync(ESMetodologiaCampoInputDto dto, long usuarioId);
        Task<(bool Success, string Message)> ActualizarAsync(long id, ESMetodologiaCampoInputDto dto);
        Task<(bool Success, string Message)> EliminarAsync(long id);
    }
}
