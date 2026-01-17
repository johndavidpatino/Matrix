/// <summary>
/// Servicio consolidado de productividad multi-roles
/// Unifica lógica de PMO, Coordinador, Campo y MyS
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.8
/// </summary>
namespace MatrixNext.Data.Services.OP;

using MatrixNext.Data.Adapters.OP;
using MatrixNext.Data.Models.OP;
using Microsoft.Extensions.Logging;

public class ProductividadConsolidadoService : IProductividadConsolidadoService
{
    private readonly IProductividadAdapter _adapter;
    private readonly ILogger<ProductividadConsolidadoService> _logger;

    public ProductividadConsolidadoService(
        IProductividadAdapter adapter,
        ILogger<ProductividadConsolidadoService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    public async Task<List<ProductividadPlanillaDto>> ObtenerPlanillasAsync(
        FiltrosProductividadDto filtros, 
        long usuarioId)
    {
        try
        {
            // 1. Obtener permisos y rol del usuario
            var permisos = await _adapter.ObtenerPermisosUsuarioAsync(usuarioId);
            
            if (permisos.RolActual == "Sin permisos" || permisos.RolActual == "Error")
            {
                _logger.LogWarning("Usuario {UserId} sin permisos para ver productividad", usuarioId);
                return new List<ProductividadPlanillaDto>();
            }

            // 2. Obtener planillas según rol
            var planillas = await _adapter.ObtenerPlanillasPorRolAsync(filtros, permisos.RolActual, usuarioId);
            
            _logger.LogInformation("Obtenidas {Count} planillas para usuario {UserId} con rol {Rol}", 
                planillas.Count, usuarioId, permisos.RolActual);
            
            return planillas;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo planillas para usuario {UserId}", usuarioId);
            throw;
        }
    }

    public async Task<ResumenProductividadDto> ObtenerResumenProductividadAsync(
        int año, 
        int mes, 
        int corte, 
        long? idTrabajo, 
        long usuarioId)
    {
        try
        {
            // Validar permisos
            var permisos = await _adapter.ObtenerPermisosUsuarioAsync(usuarioId);
            
            if (permisos.RolActual == "Sin permisos")
            {
                _logger.LogWarning("Usuario {UserId} sin permisos para ver resumen productividad", usuarioId);
                return new ResumenProductividadDto { Año = año, Mes = mes, Corte = corte };
            }

            // Obtener resumen
            var resumen = await _adapter.ObtenerResumenAsync(año, mes, corte, idTrabajo);
            
            _logger.LogInformation("Resumen productividad obtenido para usuario {UserId}. Periodo: {Año}/{Mes} Corte {Corte}",
                usuarioId, año, mes, corte);
            
            return resumen;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo resumen productividad. Usuario: {UserId}, Periodo: {Año}/{Mes} Corte {Corte}",
                usuarioId, año, mes, corte);
            throw;
        }
    }

    public async Task<(bool Success, string Message)> AprobarPlanillasAsync(
        List<AprobacionPlanillaDto> aprobaciones, 
        long usuarioId)
    {
        try
        {
            // Validar permisos
            var permisos = await _adapter.ObtenerPermisosUsuarioAsync(usuarioId);
            
            if (!permisos.PuedeAprobar)
            {
                _logger.LogWarning("Usuario {UserId} sin permiso para aprobar planillas", usuarioId);
                return (false, "No tiene permiso para aprobar planillas");
            }

            int aprobadas = 0;
            var errores = new List<string>();

            foreach (var aprobacion in aprobaciones)
            {
                try
                {
                    // Validar monto autorizado (no puede ser mayor al reportado)
                    // Esta validación se puede implementar consultando la planilla primero
                    
                    var resultado = await _adapter.AprobarPlanillaAsync(aprobacion);
                    
                    if (resultado)
                    {
                        aprobadas++;
                        _logger.LogInformation("Planilla {Id} aprobada por usuario {UserId}", 
                            aprobacion.IdPlanilla, usuarioId);
                    }
                    else
                    {
                        errores.Add($"Planilla {aprobacion.IdPlanilla} no pudo ser aprobada (posiblemente ya no está pendiente)");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error aprobando planilla {Id}", aprobacion.IdPlanilla);
                    errores.Add($"Error en planilla {aprobacion.IdPlanilla}. Por favor intente nuevamente.");
                }
            }

            if (aprobadas == aprobaciones.Count)
            {
                return (true, $"{aprobadas} planilla(s) aprobada(s) exitosamente");
            }
            else if (aprobadas > 0)
            {
                return (true, $"{aprobadas}/{aprobaciones.Count} planilla(s) aprobada(s). Errores: {string.Join(", ", errores)}");
            }
            else
            {
                return (false, $"No se aprobaron planillas. Errores: {string.Join(", ", errores)}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en proceso de aprobación masiva. Usuario: {UserId}", usuarioId);
            return (false, "Error al aprobar planillas");
        }
    }

    public async Task<(bool Success, string Message)> RechazarPlanillaAsync(
        long idPlanilla, 
        string observaciones, 
        long usuarioId)
    {
        try
        {
            // Validar permisos
            var permisos = await _adapter.ObtenerPermisosUsuarioAsync(usuarioId);
            
            if (!permisos.PuedeRechazar)
            {
                _logger.LogWarning("Usuario {UserId} sin permiso para rechazar planillas", usuarioId);
                return (false, "No tiene permiso para rechazar planillas");
            }

            // Validar observaciones
            if (string.IsNullOrWhiteSpace(observaciones))
            {
                return (false, "Las observaciones son requeridas para rechazar una planilla");
            }

            // Rechazar planilla
            var resultado = await _adapter.RechazarPlanillaAsync(idPlanilla, observaciones, usuarioId);
            
            if (resultado)
            {
                _logger.LogInformation("Planilla {Id} rechazada por usuario {UserId}", idPlanilla, usuarioId);
                return (true, "Planilla rechazada exitosamente");
            }
            else
            {
                _logger.LogWarning("Planilla {Id} no pudo ser rechazada (posiblemente ya no está pendiente)", idPlanilla);
                return (false, "La planilla no pudo ser rechazada (posiblemente ya fue procesada)");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rechazando planilla {Id}. Usuario: {UserId}", idPlanilla, usuarioId);
            return (false, "Error al rechazar la planilla");
        }
    }

    public async Task<bool> PuedeRealizarAccionAsync(long usuarioId, long idPlanilla, string accion)
    {
        try
        {
            // Esta implementación requeriría obtener el IdTrabajo de la planilla primero
            // Para simplificar, validamos solo los permisos generales del usuario
            var permisos = await _adapter.ObtenerPermisosUsuarioAsync(usuarioId);
            
            return accion switch
            {
                "Aprobar" => permisos.PuedeAprobar,
                "Rechazar" => permisos.PuedeRechazar,
                "Editar" => permisos.PuedeEditar,
                "Ver" => true, // Todos pueden ver sus propias planillas
                _ => false
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando permiso. Usuario: {UserId}, Planilla: {Planilla}, Accion: {Accion}",
                usuarioId, idPlanilla, accion);
            return false;
        }
    }

    public async Task<PermisosProductividadDto> ObtenerPermisosYRolAsync(long usuarioId)
    {
        try
        {
            var permisos = await _adapter.ObtenerPermisosUsuarioAsync(usuarioId);
            
            _logger.LogInformation("Permisos obtenidos para usuario {UserId}: Rol={Rol}", 
                usuarioId, permisos.RolActual);
            
            return permisos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo permisos para usuario {UserId}", usuarioId);
            throw;
        }
    }

    public async Task<List<dynamic>> ObtenerTrabajosDisponiblesAsync(long usuarioId)
    {
        try
        {
            // Obtener rol del usuario
            var permisos = await _adapter.ObtenerPermisosUsuarioAsync(usuarioId);
            
            if (permisos.RolActual == "Sin permisos")
            {
                _logger.LogWarning("Usuario {UserId} sin permisos para ver trabajos", usuarioId);
                return new List<dynamic>();
            }

            // Obtener trabajos según rol
            var trabajos = await _adapter.ObtenerTrabajosAsignadosAsync(usuarioId, permisos.RolActual);
            
            _logger.LogInformation("Obtenidos {Count} trabajos disponibles para usuario {UserId} con rol {Rol}",
                trabajos.Count, usuarioId, permisos.RolActual);
            
            return trabajos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo trabajos disponibles para usuario {UserId}", usuarioId);
            return new List<dynamic>();
        }
    }
}
