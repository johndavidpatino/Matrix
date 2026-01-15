/// <summary>
/// Service para distribución de entrevistas, variables de control e InHome visits
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.2.1-12.2.3
/// </summary>
namespace MatrixNext.Data.Services.PY;

using MatrixNext.Data.Adapters.PY;
using MatrixNext.Data.Models.PY;
using Microsoft.Extensions.Logging;

public class DistribucionService : IDistribucionService
{
    private readonly IDistribucionAdapter _adapter;
    private readonly ILogger<DistribucionService> _logger;

    public DistribucionService(IDistribucionAdapter adapter, ILogger<DistribucionService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    // ===== SPRINT 12.2.1: Distribución de Entrevistas =====
    
    public async Task<(bool success, string message)> DistribuirEntrevistasAsync(DistribuirPorUnidadDto distribucion, long usuarioId)
    {
        try
        {
            // Validar que haya unidades para distribuir
            if (distribucion.Unidades == null || !distribucion.Unidades.Any())
            {
                _logger.LogWarning("Intento de distribuir sin unidades. Trabajo: {IdTrabajo}", distribucion.IdTrabajo);
                return (false, "Debe incluir al menos una unidad para distribuir");
            }

            // Calcular suma total de la distribución
            var sumaTotal = distribucion.Unidades.Sum(u => u.Cantidad);

            // Validar que la suma coincida con la muestra
            var esValida = await _adapter.ValidarSumaDistribucionAsync(distribucion.IdTrabajo, sumaTotal);
            if (!esValida)
            {
                _logger.LogWarning("Suma de distribución no coincide con muestra. Trabajo: {IdTrabajo}, Suma: {Suma}",
                    distribucion.IdTrabajo, sumaTotal);
                return (false, $"La suma de la distribución ({sumaTotal}) no coincide con la muestra total del trabajo");
            }

            // Validar cantidades positivas
            if (distribucion.Unidades.Any(u => u.Cantidad <= 0))
            {
                _logger.LogWarning("Distribución con cantidades no válidas. Trabajo: {IdTrabajo}", distribucion.IdTrabajo);
                return (false, "Todas las cantidades deben ser mayores a cero");
            }

            // Asignar auditoría
            distribucion.AsignadoPor = usuarioId;

            // Distribuir
            await _adapter.DistribuirPorUnidadAsync(distribucion);

            _logger.LogInformation("Distribución exitosa. Trabajo: {IdTrabajo}, Usuario: {UserId}, Unidades: {Count}",
                distribucion.IdTrabajo, usuarioId, distribucion.Unidades.Count);
            return (true, "Distribución registrada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error distribuyendo entrevistas. Trabajo: {IdTrabajo}, Usuario: {UserId}",
                distribucion.IdTrabajo, usuarioId);
            return (false, "Error al distribuir las entrevistas");
        }
    }

    public async Task<ResumenDistribucionDto> ObtenerResumenDistribucionAsync(long idTrabajo)
    {
        try
        {
            var resumen = await _adapter.ObtenerResumenAsync(idTrabajo);
            return resumen;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo resumen distribución. Trabajo: {IdTrabajo}", idTrabajo);
            throw;
        }
    }

    public async Task<List<DistribucionEntrevistaDto>> ObtenerDistribucionesAsync(long idTrabajo)
    {
        try
        {
            return await _adapter.ObtenerDistribucionesAsync(idTrabajo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo distribuciones. Trabajo: {IdTrabajo}", idTrabajo);
            throw;
        }
    }

    public async Task<List<CuotaDistribucionDto>> ObtenerCuotasAsync(long idDistribucion)
    {
        try
        {
            return await _adapter.ObtenerCuotasAsync(idDistribucion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo cuotas. Distribución: {IdDistribucion}", idDistribucion);
            throw;
        }
    }

    // ===== SPRINT 12.2.2: Variables de Control =====
    
    public async Task<List<VariableControlDto>> ObtenerVariablesControlAsync(long idTrabajo)
    {
        try
        {
            return await _adapter.ObtenerVariablesControlAsync(idTrabajo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo variables de control. Trabajo: {IdTrabajo}", idTrabajo);
            throw;
        }
    }

    public async Task<(bool success, string message, long id)> CrearVariableControlAsync(VariableControlDto variable, long usuarioId)
    {
        try
        {
            // Validar nombre variable
            if (string.IsNullOrWhiteSpace(variable.NombreVariable))
            {
                _logger.LogWarning("Intento de crear variable sin nombre. Usuario: {UserId}", usuarioId);
                return (false, "El nombre de la variable es obligatorio", 0);
            }

            // Validar tipo de dato
            var tiposValidos = new[] { "Numérico", "Texto", "Rango", "Lista" };
            if (!tiposValidos.Contains(variable.TipoDato))
            {
                _logger.LogWarning("Tipo de dato no válido: {Tipo}. Usuario: {UserId}", variable.TipoDato, usuarioId);
                return (false, $"Tipo de dato no válido. Valores permitidos: {string.Join(", ", tiposValidos)}", 0);
            }

            // Validar rangos si es tipo Numérico o Rango
            if ((variable.TipoDato == "Numérico" || variable.TipoDato == "Rango") &&
                variable.ValorMinimo.HasValue && variable.ValorMaximo.HasValue &&
                variable.ValorMinimo > variable.ValorMaximo)
            {
                _logger.LogWarning("Rango inválido: Min {Min} > Max {Max}. Usuario: {UserId}",
                    variable.ValorMinimo, variable.ValorMaximo, usuarioId);
                return (false, "El valor mínimo no puede ser mayor al valor máximo", 0);
            }

            // Asignar auditoría
            variable.RegistradoPor = usuarioId;

            // Crear
            var id = await _adapter.CrearVariableControlAsync(variable);

            _logger.LogInformation("Variable de control {Id} creada: {Nombre}. Usuario: {UserId}",
                id, variable.NombreVariable, usuarioId);
            return (true, "Variable de control creada exitosamente", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando variable de control. Usuario: {UserId}", usuarioId);
            return (false, "Error al crear la variable de control", 0);
        }
    }

    public async Task<(bool success, string message)> ActualizarVariableControlAsync(VariableControlDto variable, long usuarioId)
    {
        try
        {
            // Validaciones similares a crear
            if (string.IsNullOrWhiteSpace(variable.NombreVariable))
            {
                return (false, "El nombre de la variable es obligatorio");
            }

            // Actualizar
            var actualizado = await _adapter.ActualizarVariableControlAsync(variable);

            if (actualizado)
            {
                _logger.LogInformation("Variable de control {Id} actualizada. Usuario: {UserId}", variable.IdVariable, usuarioId);
                return (true, "Variable de control actualizada exitosamente");
            }
            else
            {
                _logger.LogWarning("Variable de control {Id} no encontrada. Usuario: {UserId}", variable.IdVariable, usuarioId);
                return (false, "Variable de control no encontrada");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando variable de control {Id}. Usuario: {UserId}", variable.IdVariable, usuarioId);
            return (false, "Error al actualizar la variable de control");
        }
    }

    public async Task<(bool success, string message)> EliminarVariableControlAsync(long idVariable, long usuarioId)
    {
        try
        {
            var eliminado = await _adapter.EliminarVariableControlAsync(idVariable);

            if (eliminado)
            {
                _logger.LogInformation("Variable de control {Id} eliminada. Usuario: {UserId}", idVariable, usuarioId);
                return (true, "Variable de control eliminada exitosamente");
            }
            else
            {
                _logger.LogWarning("Variable de control {Id} no encontrada. Usuario: {UserId}", idVariable, usuarioId);
                return (false, "Variable de control no encontrada");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando variable de control {Id}. Usuario: {UserId}", idVariable, usuarioId);
            return (false, "Error al eliminar la variable de control");
        }
    }

    // ===== SPRINT 12.2.3: InHome Visit =====
    
    public async Task<List<InHomeVisitDto>> ObtenerInHomeVisitsAsync(long idTrabajo)
    {
        try
        {
            return await _adapter.ObtenerInHomeVisitsAsync(idTrabajo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo InHome visits. Trabajo: {IdTrabajo}", idTrabajo);
            throw;
        }
    }

    public async Task<(bool success, string message, long id)> CrearInHomeVisitAsync(InHomeVisitDto visita, long usuarioId)
    {
        try
        {
            // Validar lugar visita
            if (string.IsNullOrWhiteSpace(visita.LugarVisita))
            {
                _logger.LogWarning("Intento de crear visita sin lugar. Usuario: {UserId}", usuarioId);
                return (false, "El lugar de la visita es obligatorio", 0);
            }

            // Validar fecha programada futura
            if (visita.FechaProgramada < DateTime.Now.Date)
            {
                _logger.LogWarning("Fecha programada en el pasado: {Fecha}. Usuario: {UserId}", visita.FechaProgramada, usuarioId);
                return (false, "La fecha programada debe ser futura", 0);
            }

            // Validar cantidad participantes
            if (visita.CantidadParticipantes <= 0)
            {
                _logger.LogWarning("Cantidad de participantes inválida: {Cantidad}. Usuario: {UserId}", visita.CantidadParticipantes, usuarioId);
                return (false, "La cantidad de participantes debe ser mayor a cero", 0);
            }

            // Asignar auditoría
            visita.RegistradoPor = usuarioId;

            // Crear
            var id = await _adapter.CrearInHomeVisitAsync(visita);

            _logger.LogInformation("InHome visit {Id} creada: {Lugar} - {Fecha}. Usuario: {UserId}",
                id, visita.LugarVisita, visita.FechaProgramada, usuarioId);
            return (true, "Visita InHome creada exitosamente", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando InHome visit. Usuario: {UserId}", usuarioId);
            return (false, "Error al crear la visita InHome", 0);
        }
    }

    public async Task<(bool success, string message)> ActualizarInHomeVisitAsync(InHomeVisitDto visita, long usuarioId)
    {
        try
        {
            // Validaciones similares a crear
            if (string.IsNullOrWhiteSpace(visita.LugarVisita))
            {
                return (false, "El lugar de la visita es obligatorio");
            }

            // Actualizar
            var actualizado = await _adapter.ActualizarInHomeVisitAsync(visita);

            if (actualizado)
            {
                _logger.LogInformation("InHome visit {Id} actualizada. Usuario: {UserId}", visita.IdVisita, usuarioId);
                return (true, "Visita InHome actualizada exitosamente");
            }
            else
            {
                _logger.LogWarning("InHome visit {Id} no encontrada. Usuario: {UserId}", visita.IdVisita, usuarioId);
                return (false, "Visita InHome no encontrada");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando InHome visit {Id}. Usuario: {UserId}", visita.IdVisita, usuarioId);
            return (false, "Error al actualizar la visita InHome");
        }
    }

    public async Task<(bool success, string message)> CambiarEstadoVisitaAsync(long idVisita, string nuevoEstado, long usuarioId)
    {
        try
        {
            // Validar estado
            var estadosValidos = new[] { "Programada", "Realizada", "Cancelada", "Reprogramada" };
            if (!estadosValidos.Contains(nuevoEstado))
            {
                _logger.LogWarning("Estado no válido: {Estado}. Usuario: {UserId}", nuevoEstado, usuarioId);
                return (false, $"Estado no válido. Valores permitidos: {string.Join(", ", estadosValidos)}");
            }

            // Cambiar estado
            var actualizado = await _adapter.CambiarEstadoVisitaAsync(idVisita, nuevoEstado, usuarioId);

            if (actualizado)
            {
                _logger.LogInformation("Estado InHome visit {Id} cambiado a {Estado}. Usuario: {UserId}",
                    idVisita, nuevoEstado, usuarioId);
                return (true, $"Estado cambiado a {nuevoEstado} exitosamente");
            }
            else
            {
                _logger.LogWarning("InHome visit {Id} no encontrada. Usuario: {UserId}", idVisita, usuarioId);
                return (false, "Visita InHome no encontrada");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cambiando estado InHome visit {Id}. Usuario: {UserId}", idVisita, usuarioId);
            return (false, "Error al cambiar el estado de la visita");
        }
    }
}
