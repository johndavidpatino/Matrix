/// <summary>
/// Servicio de supervisión telefónica con validaciones
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.10
/// </summary>
namespace MatrixNext.Data.Services.OP;

using MatrixNext.Data.Adapters.OP;
using MatrixNext.Data.Models.OP;
using Microsoft.Extensions.Logging;

public class SupervisionService : ISupervisionService
{
    private readonly ISupervisionAdapter _adapter;
    private readonly ILogger<SupervisionService> _logger;

    public SupervisionService(ISupervisionAdapter adapter, ILogger<SupervisionService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    public async Task<List<SupervisionTelefonicaDto>> ObtenerSupervisionesAsync(FiltrosSupervisionDto filtros, long usuarioId)
    {
        try
        {
            if (!await _adapter.ValidarPermisoSupervisionAsync(usuarioId))
            {
                _logger.LogWarning("Usuario {UserId} sin permiso 157 para ver supervisiones", usuarioId);
                return new List<SupervisionTelefonicaDto>();
            }

            var supervisiones = await _adapter.ObtenerSupervisionesAsync(filtros);
            _logger.LogInformation("Usuario {UserId} consultó {Count} supervisiones", usuarioId, supervisiones.Count);
            return supervisiones;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo supervisiones. Usuario: {UserId}", usuarioId);
            throw;
        }
    }

    public async Task<ResumenSupervisionDto> ObtenerResumenAsync(long idTrabajo, DateTime? fechaInicio, DateTime? fechaFin, long usuarioId)
    {
        try
        {
            if (!await _adapter.ValidarPermisoSupervisionAsync(usuarioId))
            {
                _logger.LogWarning("Usuario {UserId} sin permiso para ver resumen supervisión", usuarioId);
                return new ResumenSupervisionDto();
            }

            var resumen = await _adapter.ObtenerResumenAsync(idTrabajo, fechaInicio, fechaFin);
            _logger.LogInformation("Usuario {UserId} consultó resumen supervisión trabajo {IdTrabajo}", usuarioId, idTrabajo);
            return resumen;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo resumen supervisión. Trabajo: {IdTrabajo}, Usuario: {UserId}",
                idTrabajo, usuarioId);
            throw;
        }
    }

    public async Task<(bool Success, string Message, long IdSupervision)> RegistrarSupervisionAsync(
        RegistroSupervisionDto registro, 
        long usuarioId)
    {
        try
        {
            // Validación 1: Permiso
            if (!await _adapter.ValidarPermisoSupervisionAsync(usuarioId))
            {
                _logger.LogWarning("Usuario {UserId} sin permiso 157 para registrar supervisión", usuarioId);
                return (false, "No tiene permiso para registrar supervisiones telefónicas", 0);
            }

            // Validación 2: Checklist no vacío
            if (registro.Checklist == null || registro.Checklist.Count == 0)
            {
                return (false, "El checklist de evaluación no puede estar vacío", 0);
            }

            // Validación 3: Número de encuesta requerido
            if (string.IsNullOrWhiteSpace(registro.NumeroEncuesta))
            {
                return (false, "El número de encuesta es requerido", 0);
            }

            // Validación 4: Operador y supervisor diferentes
            if (registro.IdOperador == registro.IdSupervisor)
            {
                return (false, "El operador y el supervisor deben ser personas diferentes", 0);
            }

            // Registrar supervisión
            var idSupervision = await _adapter.RegistrarSupervisionAsync(registro);
            
            _logger.LogInformation("Supervisión {Id} registrada por usuario {UserId}. Operador: {IdOp}, Supervisor: {IdSup}",
                idSupervision, usuarioId, registro.IdOperador, registro.IdSupervisor);
            
            return (true, $"Supervisión registrada exitosamente. ID: {idSupervision}", idSupervision);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registrando supervisión. Usuario: {UserId}", usuarioId);
            return (false, "Error al registrar la supervisión", 0);
        }
    }

    public async Task<List<ChecklistSupervisionDto>> ObtenerChecklistAsync(long idSupervision, long usuarioId)
    {
        try
        {
            if (!await _adapter.ValidarPermisoSupervisionAsync(usuarioId))
            {
                _logger.LogWarning("Usuario {UserId} sin permiso para ver checklist", usuarioId);
                return new List<ChecklistSupervisionDto>();
            }

            var checklist = await _adapter.ObtenerChecklistAsync(idSupervision);
            _logger.LogInformation("Usuario {UserId} consultó checklist de supervisión {Id}", usuarioId, idSupervision);
            return checklist;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo checklist. Supervisión: {Id}, Usuario: {UserId}",
                idSupervision, usuarioId);
            throw;
        }
    }

    public async Task<List<CatalogoSupervisionDto>> ObtenerCatalogosAsync(string tipo, long usuarioId, long? idTrabajo = null)
    {
        try
        {
            if (!await _adapter.ValidarPermisoSupervisionAsync(usuarioId))
            {
                _logger.LogWarning("Usuario {UserId} sin permiso para ver catálogos supervisión", usuarioId);
                return new List<CatalogoSupervisionDto>();
            }

            List<CatalogoSupervisionDto> catalogo;
            
            if (tipo.Equals("Operadores", StringComparison.OrdinalIgnoreCase))
            {
                catalogo = await _adapter.ObtenerOperadoresActivosAsync(idTrabajo);
            }
            else if (tipo.Equals("Supervisores", StringComparison.OrdinalIgnoreCase))
            {
                catalogo = await _adapter.ObtenerSupervisoresActivosAsync();
            }
            else
            {
                _logger.LogWarning("Tipo de catálogo inválido: {Tipo}", tipo);
                return new List<CatalogoSupervisionDto>();
            }

            _logger.LogInformation("Usuario {UserId} consultó catálogo {Tipo}: {Count} registros",
                usuarioId, tipo, catalogo.Count);
            return catalogo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo catálogo {Tipo}. Usuario: {UserId}", tipo, usuarioId);
            throw;
        }
    }
}
