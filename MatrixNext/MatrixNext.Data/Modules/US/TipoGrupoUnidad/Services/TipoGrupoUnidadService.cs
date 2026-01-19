using MatrixNext.Data.Modules.US.TipoGrupoUnidad.Adapters;
using MatrixNext.Data.Modules.US.TipoGrupoUnidad.Models;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Modules.US.TipoGrupoUnidad.Services;

/// <summary>
/// Servicio para Tipo de Grupo de Unidad
/// Ref: CoreProject/Clases/US/TipoGrupoUnidad.vb
/// </summary>
public interface ITipoGrupoUnidadService
{
    Task<IEnumerable<TipoGrupoUnidadDto>> ObtenerTodosAsync(string? filtro = null);
    Task<TipoGrupoUnidadDto?> ObtenerPorIdAsync(int id);
    Task<(bool Success, string Message)> GuardarAsync(TipoGrupoUnidadDto dto);
    Task<(bool Success, string Message)> EditarAsync(TipoGrupoUnidadDto dto);
    Task<(bool Success, string Message)> EliminarAsync(int id);
}

public class TipoGrupoUnidadService : ITipoGrupoUnidadService
{
    private readonly ITipoGrupoUnidadAdapter _adapter;
    private readonly ILogger<TipoGrupoUnidadService> _logger;

    public TipoGrupoUnidadService(ITipoGrupoUnidadAdapter adapter, ILogger<TipoGrupoUnidadService> logger)
    {
        _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TipoGrupoUnidadDto>> ObtenerTodosAsync(string? filtro = null)
    {
        try
        {
            return await _adapter.ObtenerTodosAsync(filtro);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tipos de grupo de unidad");
            return Enumerable.Empty<TipoGrupoUnidadDto>();
        }
    }

    public async Task<TipoGrupoUnidadDto?> ObtenerPorIdAsync(int id)
    {
        try
        {
            return await _adapter.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tipo de grupo de unidad por Id: {Id}", id);
            return null;
        }
    }

    public async Task<(bool Success, string Message)> GuardarAsync(TipoGrupoUnidadDto dto)
    {
        try
        {
            if (dto.Id <= 0)
                return (false, "El Id del tipo de grupo de unidad es requerido");

            if (string.IsNullOrWhiteSpace(dto.TipoGrupoUnidad))
                return (false, "El nombre del tipo de grupo de unidad es requerido");

            await _adapter.GuardarAsync(dto.Id, dto.TipoGrupoUnidad.Trim());

            _logger.LogInformation("Tipo de grupo de unidad guardado exitosamente. Id: {Id}", dto.Id);
            return (true, "Tipo de grupo de unidad guardado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar tipo de grupo de unidad. Id: {Id}", dto.Id);
            return (false, "Error al guardar el tipo de grupo de unidad. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, string Message)> EditarAsync(TipoGrupoUnidadDto dto)
    {
        try
        {
            if (dto.Id <= 0)
                return (false, "El Id del tipo de grupo de unidad es requerido");

            if (string.IsNullOrWhiteSpace(dto.TipoGrupoUnidad))
                return (false, "El nombre del tipo de grupo de unidad es requerido");

            var existente = await _adapter.ObtenerPorIdAsync(dto.Id);
            if (existente == null)
                return (false, "El tipo de grupo de unidad no existe");

            await _adapter.EditarAsync(dto.Id, dto.TipoGrupoUnidad.Trim());

            _logger.LogInformation("Tipo de grupo de unidad editado exitosamente. Id: {Id}", dto.Id);
            return (true, "Tipo de grupo de unidad actualizado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al editar tipo de grupo de unidad. Id: {Id}", dto.Id);
            return (false, "Error al actualizar el tipo de grupo de unidad. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, string Message)> EliminarAsync(int id)
    {
        try
        {
            if (id <= 0)
                return (false, "El Id del tipo de grupo de unidad es requerido");

            var existente = await _adapter.ObtenerPorIdAsync(id);
            if (existente == null)
                return (false, "El tipo de grupo de unidad no existe");

            await _adapter.EliminarAsync(id);

            _logger.LogInformation("Tipo de grupo de unidad eliminado exitosamente. Id: {Id}", id);
            return (true, "Tipo de grupo de unidad eliminado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar tipo de grupo de unidad. Id: {Id}", id);
            return (false, "Error al eliminar el tipo de grupo de unidad. Por favor intente nuevamente.");
        }
    }
}
