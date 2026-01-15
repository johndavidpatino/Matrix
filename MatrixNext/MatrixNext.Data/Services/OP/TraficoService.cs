/// <summary>
/// Servicio de tráfico de encuestas con validaciones de negocio
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.9
/// </summary>
namespace MatrixNext.Data.Services.OP;

using MatrixNext.Data.Adapters.OP;
using MatrixNext.Data.Models.OP;
using Microsoft.Extensions.Logging;

public class TraficoService : ITraficoService
{
    private readonly ITraficoAdapter _adapter;
    private readonly ILogger<TraficoService> _logger;

    public TraficoService(ITraficoAdapter adapter, ILogger<TraficoService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    public async Task<List<TraficoEncuestaDto>> ObtenerMovimientosAsync(FiltrosTraficoDto filtros, long usuarioId)
    {
        try
        {
            var movimientos = await _adapter.ObtenerMovimientosAsync(filtros);
            _logger.LogInformation("Usuario {UserId} consultó {Count} movimientos de tráfico", usuarioId, movimientos.Count);
            return movimientos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo movimientos. Usuario: {UserId}", usuarioId);
            throw;
        }
    }

    public async Task<ResumenTraficoDto> ObtenerResumenUnidadAsync(int idUnidad, long? idTrabajo, long usuarioId)
    {
        try
        {
            if (!await _adapter.ValidarPermisoUnidadAsync(usuarioId, idUnidad))
            {
                _logger.LogWarning("Usuario {UserId} sin permiso para ver resumen de unidad {IdUnidad}", usuarioId, idUnidad);
                return new ResumenTraficoDto { IdUnidad = idUnidad, NombreUnidad = "Sin acceso" };
            }

            var resumen = await _adapter.ObtenerResumenPorUnidadAsync(idUnidad, idTrabajo);
            _logger.LogInformation("Usuario {UserId} consultó resumen unidad {IdUnidad}", usuarioId, idUnidad);
            return resumen;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo resumen unidad {IdUnidad}. Usuario: {UserId}", idUnidad, usuarioId);
            throw;
        }
    }

    public async Task<(bool Success, string Message, long IdMovimiento)> EnviarEncuestasAsync(EnvioEncuestasDto envio, long usuarioId)
    {
        try
        {
            // Validación 1: Permiso para unidad origen
            if (!await _adapter.ValidarPermisoUnidadAsync(usuarioId, envio.IdUnidadOrigen))
            {
                _logger.LogWarning("Usuario {UserId} sin permiso para enviar desde unidad {IdUnidad}", 
                    usuarioId, envio.IdUnidadOrigen);
                return (false, "No tiene permiso para enviar desde esta unidad", 0);
            }

            // Validación 2: Cantidad disponible
            if (!await _adapter.ValidarCantidadDisponibleAsync(envio.IdTrabajo, envio.IdUnidadOrigen, envio.Cantidad))
            {
                _logger.LogWarning("Cantidad insuficiente para envío. Trabajo: {IdTrabajo}, Unidad: {IdUnidad}, Cantidad: {Cantidad}",
                    envio.IdTrabajo, envio.IdUnidadOrigen, envio.Cantidad);
                return (false, "Cantidad de encuestas no disponible en la unidad de origen", 0);
            }

            // Validación 3: Ciudad requerida para RMC (unidad 119/120)
            if (envio.IdUnidadDestino == 119 || envio.IdUnidadDestino == 120)
            {
                if (string.IsNullOrWhiteSpace(envio.Ciudad))
                {
                    return (false, "La ciudad es requerida para envíos a RMC", 0);
                }
            }

            // Registrar envío
            var idMovimiento = await _adapter.RegistrarEnvioAsync(envio);
            _logger.LogInformation("Envío registrado exitosamente. Id: {Id}, Usuario: {UserId}", idMovimiento, usuarioId);
            return (true, $"Envío registrado exitosamente. ID: {idMovimiento}", idMovimiento);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enviando encuestas. Usuario: {UserId}", usuarioId);
            return (false, "Error al registrar el envío", 0);
        }
    }

    public async Task<(bool Success, string Message)> RecibirEncuestasAsync(RecepcionEncuestasDto recepcion, long usuarioId)
    {
        try
        {
            // Validación: Observaciones requeridas si hay discrepancia
            // (Esta validación se puede mejorar consultando el movimiento primero)

            var resultado = await _adapter.RegistrarRecepcionAsync(recepcion);
            
            if (resultado)
            {
                _logger.LogInformation("Recepción registrada. Movimiento: {Id}, Usuario: {UserId}", 
                    recepcion.IdMovimiento, usuarioId);
                return (true, "Recepción registrada exitosamente");
            }
            else
            {
                _logger.LogWarning("Recepción no pudo ser registrada. Movimiento: {Id}", recepcion.IdMovimiento);
                return (false, "El movimiento no pudo ser recibido (estado inválido)");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recibiendo encuestas. Movimiento: {Id}, Usuario: {UserId}", 
                recepcion.IdMovimiento, usuarioId);
            return (false, "Error al registrar la recepción");
        }
    }

    public async Task<(bool Success, string Message)> DevolverEncuestasAsync(DevolucionEncuestasDto devolucion, long usuarioId)
    {
        try
        {
            // Validación: Motivo de devolución requerido
            if (string.IsNullOrWhiteSpace(devolucion.MotivoDevolucion))
            {
                return (false, "El motivo de devolución es requerido");
            }

            var resultado = await _adapter.RegistrarDevolucionAsync(devolucion);
            
            if (resultado)
            {
                _logger.LogInformation("Devolución registrada. Movimiento: {Id}, Usuario: {UserId}", 
                    devolucion.IdMovimiento, usuarioId);
                return (true, "Devolución registrada exitosamente");
            }
            else
            {
                return (false, "La devolución no pudo ser registrada");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error devolviendo encuestas. Movimiento: {Id}, Usuario: {UserId}",
                devolucion.IdMovimiento, usuarioId);
            return (false, "Error al registrar la devolución");
        }
    }

    public async Task<List<PersonalTraficoDto>> ObtenerPersonalAsignadoAsync(long idMovimiento, long usuarioId)
    {
        try
        {
            var personal = await _adapter.ObtenerPersonalAsignadoAsync(idMovimiento);
            _logger.LogInformation("Usuario {UserId} consultó personal asignado a movimiento {Id}", usuarioId, idMovimiento);
            return personal;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo personal asignado. Movimiento: {Id}, Usuario: {UserId}", 
                idMovimiento, usuarioId);
            throw;
        }
    }

    public async Task<(bool Success, string Message)> AsignarPersonalAsync(AsignacionPersonalDto asignacion, long usuarioId)
    {
        try
        {
            // Validación: Cantidad asignada debe ser positiva
            if (asignacion.CantidadAsignada <= 0)
            {
                return (false, "La cantidad asignada debe ser mayor a 0");
            }

            // Validación: Cargo válido
            var cargosValidos = new[] { "Encuestador", "Supervisor", "Crítico", "Digitador", "RMC" };
            if (!cargosValidos.Contains(asignacion.Cargo))
            {
                return (false, $"Cargo inválido. Válidos: {string.Join(", ", cargosValidos)}");
            }

            var resultado = await _adapter.AsignarPersonalAsync(asignacion);
            
            if (resultado)
            {
                _logger.LogInformation("Personal asignado. Movimiento: {IdMov}, Empleado: {IdEmp}, Usuario: {UserId}",
                    asignacion.IdMovimiento, asignacion.IdEmpleado, usuarioId);
                return (true, "Personal asignado exitosamente");
            }
            else
            {
                return (false, "No se pudo asignar el personal");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error asignando personal. Movimiento: {Id}, Usuario: {UserId}",
                asignacion.IdMovimiento, usuarioId);
            return (false, "Error al asignar el personal");
        }
    }
}
