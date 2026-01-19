using Microsoft.Extensions.Logging;
using MatrixNext.Data.Modules.CU.TrabajosCuentas.Adapters;
using MatrixNext.Data.Modules.CU.TrabajosCuentas.Models;

namespace MatrixNext.Data.Modules.CU.TrabajosCuentas.Services;

/// <summary>
/// Interface para el servicio de Trabajos de Cuenta
/// </summary>
public interface ITrabajoCuentaService
{
    Task<IEnumerable<TrabajoCuentaDto>> ObtenerTrabajosAsync(TrabajoCuentaBusquedaParams? filtros = null);
    Task<TrabajoCuentaDto?> ObtenerTrabajoPorIdAsync(long id);
    Task<IEnumerable<TrabajoCuentaDto>> ObtenerTrabajosPorEstudioAsync(long estudioId);
    Task<TrabajosCuentasIndexViewModel> PrepararViewModelAsync(long estudioId);
}

/// <summary>
/// Servicio de Trabajos de Cuenta - Lógica de negocio
/// </summary>
public class TrabajoCuentaService : ITrabajoCuentaService
{
    private readonly ITrabajoCuentaAdapter _adapter;
    private readonly ILogger<TrabajoCuentaService> _logger;

    public TrabajoCuentaService(ITrabajoCuentaAdapter adapter, ILogger<TrabajoCuentaService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    public async Task<IEnumerable<TrabajoCuentaDto>> ObtenerTrabajosAsync(TrabajoCuentaBusquedaParams? filtros = null)
    {
        return await _adapter.ObtenerTrabajosAsync(filtros);
    }

    public async Task<TrabajoCuentaDto?> ObtenerTrabajoPorIdAsync(long id)
    {
        return await _adapter.ObtenerTrabajoPorIdAsync(id);
    }

    public async Task<IEnumerable<TrabajoCuentaDto>> ObtenerTrabajosPorEstudioAsync(long estudioId)
    {
        return await _adapter.ObtenerTrabajosPorEstudioAsync(estudioId);
    }

    public async Task<TrabajosCuentasIndexViewModel> PrepararViewModelAsync(long estudioId)
    {
        var trabajos = await _adapter.ObtenerTrabajosPorEstudioAsync(estudioId);
        var estados = await _adapter.ObtenerEstadosTrabajoAsync();

        return new TrabajosCuentasIndexViewModel
        {
            EstudioId = estudioId,
            Trabajos = trabajos,
            Estados = estados
        };
    }
}
