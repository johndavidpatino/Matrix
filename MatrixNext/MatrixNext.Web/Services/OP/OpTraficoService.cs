using MatrixNext.Web.Models.PY;
using MatrixNext.Web.Services.OP.Models;
using MatrixNext.Web.Services.PY;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Services.OP;

public class OpTraficoService : IOpTraficoService
{
    private readonly IOpTraficoDataAdapter _adapter;
    private readonly ITrabajosService _trabajos;
    private readonly ILogger<OpTraficoService> _logger;

    public OpTraficoService(IOpTraficoDataAdapter adapter, ITrabajosService trabajos, ILogger<OpTraficoService> logger)
    {
        _adapter = adapter;
        _trabajos = trabajos;
        _logger = logger;
    }

    public async Task<OpTraficoSummary> ObtenerResumenAsync(long? trabajoId = null)
    {
        var filtros = new FiltrosVM { PageNumber = 1, PageSize = 50 };
        var trabajosResult = await _trabajos.ListarAsync(filtros);
        var trabajos = trabajosResult.Items;

        var selectedTrabajo = trabajos.FirstOrDefault(t => t.Id == trabajoId) ?? trabajos.FirstOrDefault();
        var traffic = selectedTrabajo is null
            ? Array.Empty<TraficoCiudadDto>()
            : await _adapter.ObtenerCiudadesPorTrabajoAsync(selectedTrabajo.Id);

        var summary = new OpTraficoSummary(
            GeneratedAt: DateTime.UtcNow,
            SelectedTrabajoId: selectedTrabajo?.Id,
            SelectedTrabajoName: selectedTrabajo?.Nombre,
            Trabajos: trabajos,
            Ciudades: traffic);

        _logger.LogDebug("Resumen de trafico generado para trabajo {trabajoId}", summary.SelectedTrabajoId);
        return summary;
    }
}
