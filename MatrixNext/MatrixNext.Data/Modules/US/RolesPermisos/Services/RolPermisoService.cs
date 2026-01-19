using MatrixNext.Data.Modules.US.RolesPermisos.Adapters;
using MatrixNext.Data.Modules.US.RolesPermisos.Models;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Modules.US.RolesPermisos.Services;

/// <summary>
/// Servicio para RolesPermisos
/// Ref: CoreProject/Clases/US/RolesPermisos.vb
/// </summary>
public interface IRolPermisoService
{
    Task<IEnumerable<RolPermisoDto>> ObtenerRolesPermisosAsync(int permisoId);
    Task<(bool Success, string Message)> GuardarRolPermisoAsync(RolPermisoCreateDto dto);
    Task<(bool Success, string Message)> EliminarRolPermisoAsync(int permisoId, int rolId);
    Task<IEnumerable<RolComboDto>> ObtenerRolesComboAsync();
    Task<IEnumerable<PermisoComboDto>> ObtenerPermisosComboAsync();
}

public class RolPermisoService : IRolPermisoService
{
    private readonly IRolPermisoAdapter _adapter;
    private readonly ILogger<RolPermisoService> _logger;

    public RolPermisoService(IRolPermisoAdapter adapter, ILogger<RolPermisoService> logger)
    {
        _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<RolPermisoDto>> ObtenerRolesPermisosAsync(int permisoId)
    {
        try
        {
            if (permisoId <= 0)
                return Enumerable.Empty<RolPermisoDto>();

            return await _adapter.ObtenerRolesPermisosAsync(permisoId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener roles permisos. PermisoId: {PermisoId}", permisoId);
            return Enumerable.Empty<RolPermisoDto>();
        }
    }

    public async Task<(bool Success, string Message)> GuardarRolPermisoAsync(RolPermisoCreateDto dto)
    {
        try
        {
            if (dto.PermisoId <= 0)
                return (false, "El permiso es requerido");

            if (dto.RolId <= 0)
                return (false, "El rol es requerido");

            // Verificar si ya existe la asignación
            var existentes = await _adapter.ObtenerRolesPermisosAsync(dto.PermisoId);
            if (existentes.Any(rp => rp.RolId == dto.RolId))
                return (false, "El rol ya está asignado a este permiso");

            var result = await _adapter.GuardarRolPermisoAsync(dto.PermisoId, dto.RolId);

            _logger.LogInformation("Rol permiso guardado. PermisoId: {PermisoId}, RolId: {RolId}", dto.PermisoId, dto.RolId);
            return (true, "Rol asignado correctamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar rol permiso. PermisoId: {PermisoId}, RolId: {RolId}", dto.PermisoId, dto.RolId);
            return (false, "Error al asignar el rol. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, string Message)> EliminarRolPermisoAsync(int permisoId, int rolId)
    {
        try
        {
            if (permisoId <= 0 || rolId <= 0)
                return (false, "Parámetros inválidos");

            await _adapter.EliminarRolPermisoAsync(permisoId, rolId);

            _logger.LogInformation("Rol permiso eliminado. PermisoId: {PermisoId}, RolId: {RolId}", permisoId, rolId);
            return (true, "Asignación eliminada correctamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar rol permiso. PermisoId: {PermisoId}, RolId: {RolId}", permisoId, rolId);
            return (false, "Error al eliminar la asignación. Por favor intente nuevamente.");
        }
    }

    public async Task<IEnumerable<RolComboDto>> ObtenerRolesComboAsync()
    {
        try
        {
            return await _adapter.ObtenerRolesComboAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener roles combo");
            return Enumerable.Empty<RolComboDto>();
        }
    }

    public async Task<IEnumerable<PermisoComboDto>> ObtenerPermisosComboAsync()
    {
        try
        {
            return await _adapter.ObtenerPermisosComboAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener permisos combo");
            return Enumerable.Empty<PermisoComboDto>();
        }
    }
}
