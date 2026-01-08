using MatrixNext.Web.Services.OP.Models;

namespace MatrixNext.Web.Services.OP;

public interface IOpTraficoDataAdapter
{
    Task<IReadOnlyCollection<TraficoCiudadDto>> ObtenerCiudadesPorTrabajoAsync(long trabajoId, CancellationToken cancellationToken = default);
}
