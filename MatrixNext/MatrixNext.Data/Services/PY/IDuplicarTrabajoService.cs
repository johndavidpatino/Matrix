using MatrixNext.Data.DTOs.PY;

namespace MatrixNext.Data.Services.PY;

public interface IDuplicarTrabajoService
{
    Task<DuplicarTrabajoViewModel> PrepararViewModelAsync(long idTrabajo);
    Task<(bool success, string message, long? idNuevo)> DuplicarTrabajoAsync(DuplicarTrabajoDto dto, long userId);
}
