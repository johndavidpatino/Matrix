using System.Linq;
using MatrixNext.Web.Models.PY;
using MatrixNext.Web.Services.PY;
using MatrixNext.Web.Services.OP.Models;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Services.OP;

public class OpPortalService : IOpPortalService
{
    private readonly ITrabajosService _trabajos;
    private readonly IMetodologiasLookupService _metodologias;
    private readonly ILogger<OpPortalService> _logger;

    public OpPortalService(
        ITrabajosService trabajos,
        IMetodologiasLookupService metodologias,
        ILogger<OpPortalService> logger)
    {
        _trabajos = trabajos;
        _metodologias = metodologias;
        _logger = logger;
    }

    public async Task<OpPortalSnapshot> ObtenerPortalAsync(FiltrosVM filtros, long? idProyecto = null)
    {
        filtros ??= new FiltrosVM();

        var trabajos = await _trabajos.ListarAsync(filtros, idProyecto);
        var metodologias = await _metodologias.ObtenerMapaMetodologiasAsync();

        var estados = trabajos.Items
            .GroupBy(x => x.Estado)
            .ToDictionary(g => g.Key, g => g.Count());

        var snapshot = new OpPortalSnapshot(
            GeneratedAt: DateTime.UtcNow,
            Trabajos: trabajos,
            EstadoCounts: estados,
            MetodologiasMap: metodologias);

        _logger.LogDebug("Portal OP_Cuantitativo snapshot generado con {count} trabajos", trabajos.TotalCount);

        return snapshot;
    }
}
