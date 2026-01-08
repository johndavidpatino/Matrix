using MatrixNext.Web.ViewModels.OP;

namespace MatrixNext.Web.Services.OP;

public interface IOpProduccionService
{
    Task<IReadOnlyList<UnidadDto>> ObtenerUnidadesAsync(long? identificacion, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ActividadDto>> ObtenerActividadesAsync(int? unidad, int? actividad, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JbeDto>> ObtenerJbeAsync(int tipo, string? busqueda, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProduccionRowViewModel>> ObtenerProduccionAsync(DateTime? fechaInicio, DateTime? fechaFin, string? identificacion, int? unidad, CancellationToken cancellationToken = default);
    Task<bool> GuardarRegistroAsync(GuardarRegistroRequest request, CancellationToken cancellationToken = default);
    Task<ProduccionSummary> ObtenerResumenGeneralAsync(CancellationToken cancellationToken = default);
}
