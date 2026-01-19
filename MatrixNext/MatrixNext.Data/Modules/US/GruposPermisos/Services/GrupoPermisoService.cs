using MatrixNext.Data.Modules.US.GruposPermisos.Adapters;
using MatrixNext.Data.Modules.US.GruposPermisos.Models;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Modules.US.GruposPermisos.Services;

/// <summary>
/// Servicio para Grupos de Permisos
/// Ref: CoreProject/Clases/US/GrupoPermisos.vb
/// </summary>
public interface IGrupoPermisoService
{
    Task<IEnumerable<GrupoPermisoDto>> ObtenerTodosAsync(string? filtro = null);
    Task<GrupoPermisoDto?> ObtenerPorIdAsync(int id);
    Task<(bool Success, string Message)> GuardarAsync(GrupoPermisoDto dto);
    Task<(bool Success, string Message)> EditarAsync(GrupoPermisoDto dto);
    Task<(bool Success, string Message)> EliminarAsync(int id);
}

public class GrupoPermisoService : IGrupoPermisoService
{
    private readonly IGrupoPermisoAdapter _adapter;
    private readonly ILogger<GrupoPermisoService> _logger;

    public GrupoPermisoService(IGrupoPermisoAdapter adapter, ILogger<GrupoPermisoService> logger)
    {
        _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<GrupoPermisoDto>> ObtenerTodosAsync(string? filtro = null)
    {
        try
        {
            return await _adapter.ObtenerGruposPermisosAsync(filtro);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener grupos de permisos");
            return Enumerable.Empty<GrupoPermisoDto>();
        }
    }

    public async Task<GrupoPermisoDto?> ObtenerPorIdAsync(int id)
    {
        try
        {
            return await _adapter.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener grupo de permisos por Id: {Id}", id);
            return null;
        }
    }

    public async Task<(bool Success, string Message)> GuardarAsync(GrupoPermisoDto dto)
    {
        try
        {
            // Validaciones de negocio
            if (dto.Id <= 0)
                return (false, "El Id del grupo de permisos es requerido");

            if (string.IsNullOrWhiteSpace(dto.GrupoPermisos))
                return (false, "El nombre del grupo de permisos es requerido");

            await _adapter.GuardarAsync(dto.Id, dto.GrupoPermisos.Trim());

            _logger.LogInformation("Grupo de permisos guardado exitosamente. Id: {Id}", dto.Id);
            return (true, "Grupo de permisos guardado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar grupo de permisos. Id: {Id}", dto.Id);
            return (false, "Error al guardar el grupo de permisos. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, string Message)> EditarAsync(GrupoPermisoDto dto)
    {
        try
        {
            // Validaciones de negocio
            if (dto.Id <= 0)
                return (false, "El Id del grupo de permisos es requerido");

            if (string.IsNullOrWhiteSpace(dto.GrupoPermisos))
                return (false, "El nombre del grupo de permisos es requerido");

            // Verificar que existe
            var existente = await _adapter.ObtenerPorIdAsync(dto.Id);
            if (existente == null)
                return (false, "El grupo de permisos no existe");

            await _adapter.EditarAsync(dto.Id, dto.GrupoPermisos.Trim());

            _logger.LogInformation("Grupo de permisos editado exitosamente. Id: {Id}", dto.Id);
            return (true, "Grupo de permisos actualizado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al editar grupo de permisos. Id: {Id}", dto.Id);
            return (false, "Error al actualizar el grupo de permisos. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, string Message)> EliminarAsync(int id)
    {
        try
        {
            if (id <= 0)
                return (false, "El Id del grupo de permisos es requerido");

            // Verificar que existe
            var existente = await _adapter.ObtenerPorIdAsync(id);
            if (existente == null)
                return (false, "El grupo de permisos no existe");

            await _adapter.EliminarAsync(id);

            _logger.LogInformation("Grupo de permisos eliminado exitosamente. Id: {Id}", id);
            return (true, "Grupo de permisos eliminado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar grupo de permisos. Id: {Id}", id);
            return (false, "Error al eliminar el grupo de permisos. Por favor intente nuevamente.");
        }
    }
}
