using MatrixNext.Data.Modules.US.Unidades.Adapters;
using MatrixNext.Data.Modules.US.Unidades.Models;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Modules.US.Unidades.Services;

public interface IUnidadService
{
    Task<IEnumerable<UnidadDto>> ObtenerTodosAsync(string? filtro = null, int? idGrupoUnidad = null);
    Task<UnidadDto?> ObtenerPorIdAsync(int id);
    Task<(bool Success, string Message)> GuardarAsync(UnidadDto dto);
    Task<(bool Success, string Message)> EditarAsync(UnidadDto dto);
    Task<(bool Success, string Message)> EliminarAsync(int id);
}

public class UnidadService : IUnidadService
{
    private readonly IUnidadAdapter _adapter;
    private readonly ILogger<UnidadService> _logger;

    public UnidadService(IUnidadAdapter adapter, ILogger<UnidadService> logger)
    {
        _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<UnidadDto>> ObtenerTodosAsync(string? filtro = null, int? idGrupoUnidad = null)
    {
        try
        {
            return await _adapter.ObtenerTodosAsync(filtro, idGrupoUnidad);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener unidades");
            return Enumerable.Empty<UnidadDto>();
        }
    }

    public async Task<UnidadDto?> ObtenerPorIdAsync(int id)
    {
        try
        {
            return await _adapter.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener unidad por Id: {Id}", id);
            return null;
        }
    }

    public async Task<(bool Success, string Message)> GuardarAsync(UnidadDto dto)
    {
        try
        {
            if (dto.Id <= 0)
                return (false, "El Id de la unidad es requerido");

            if (string.IsNullOrWhiteSpace(dto.Unidad))
                return (false, "El nombre de la unidad es requerido");

            await _adapter.GuardarAsync(dto.Id, dto.Unidad.Trim(), dto.IdGrupoUnidad);

            _logger.LogInformation("Unidad guardada exitosamente. Id: {Id}", dto.Id);
            return (true, "Unidad guardada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar unidad. Id: {Id}", dto.Id);
            return (false, "Error al guardar la unidad. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, string Message)> EditarAsync(UnidadDto dto)
    {
        try
        {
            if (dto.Id <= 0)
                return (false, "El Id de la unidad es requerido");

            if (string.IsNullOrWhiteSpace(dto.Unidad))
                return (false, "El nombre de la unidad es requerido");

            var existente = await _adapter.ObtenerPorIdAsync(dto.Id);
            if (existente == null)
                return (false, "La unidad no existe");

            await _adapter.EditarAsync(dto.Id, dto.Unidad.Trim(), dto.IdGrupoUnidad);

            _logger.LogInformation("Unidad editada exitosamente. Id: {Id}", dto.Id);
            return (true, "Unidad actualizada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al editar unidad. Id: {Id}", dto.Id);
            return (false, "Error al actualizar la unidad. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, string Message)> EliminarAsync(int id)
    {
        try
        {
            if (id <= 0)
                return (false, "El Id de la unidad es requerido");

            var existente = await _adapter.ObtenerPorIdAsync(id);
            if (existente == null)
                return (false, "La unidad no existe");

            await _adapter.EliminarAsync(id);

            _logger.LogInformation("Unidad eliminada exitosamente. Id: {Id}", id);
            return (true, "Unidad eliminada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar unidad. Id: {Id}", id);
            return (false, "Error al eliminar la unidad. Por favor intente nuevamente.");
        }
    }
}
