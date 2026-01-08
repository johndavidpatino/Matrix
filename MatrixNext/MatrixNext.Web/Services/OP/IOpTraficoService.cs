using MatrixNext.Web.Services.OP.Models;

namespace MatrixNext.Web.Services.OP;

public interface IOpTraficoService
{
    Task<OpTraficoSummary> ObtenerResumenAsync(long? trabajoId = null);
}
