using MatrixNext.Data.Modules.TH.Contratistas.Adapters;
using MatrixNext.Data.Modules.TH.Contratistas.Models;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Modules.TH.Contratistas.Services;

/// <summary>
/// Interfaz para el servicio de Contratistas
/// </summary>
public interface IContratistaService
{
    // Consultas
    Task<IEnumerable<ContratistaDto>> ObtenerContratistasAsync(BuscarContratistasParams parametros);
    Task<ContratistaDto?> ObtenerContratistaPorIdAsync(long identificacion);
    
    // CRUD Contratistas
    Task<(bool Success, string Message)> GuardarContratistaAsync(ContratistaCreateEditDto dto, long usuarioId);
    Task<(bool Success, string Message)> ActualizarEstadoAsync(long identificacion, int estado, long usuarioId);
    
    // Servicios de Contratista
    Task<IEnumerable<ContratistaServicioDto>> ObtenerServiciosContratistaAsync(long identificacion);
    Task<(bool Success, string Message)> AgregarServicioAsync(ContratistaServicioCreateDto dto, long usuarioId);
    Task<(bool Success, string Message)> ActualizarEstadoServicioAsync(long id, bool estado, long contratistaId, long usuarioId);
    
    // Log
    Task<IEnumerable<ContratistaLogDto>> ObtenerLogContratistasAsync(long? contratistaId, string? nombre);
    
    // Combos
    Task<IEnumerable<EstadoContratistaDto>> ObtenerEstadosAsync();
    Task<IEnumerable<ServicioContratistaComboDto>> ObtenerServiciosComboAsync();
    Task<IEnumerable<ClasificacionContratistaDto>> ObtenerClasificacionesAsync();
    Task<IEnumerable<CiudadComboDto>> ObtenerCiudadesAsync();
}

/// <summary>
/// Implementación del servicio de Contratistas
/// </summary>
public class ContratistaService : IContratistaService
{
    private readonly IContratistaAdapter _adapter;
    private readonly ILogger<ContratistaService> _logger;

    public ContratistaService(IContratistaAdapter adapter, ILogger<ContratistaService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    #region Consultas

    public async Task<IEnumerable<ContratistaDto>> ObtenerContratistasAsync(BuscarContratistasParams parametros)
    {
        return await _adapter.ObtenerContratistasAsync(parametros);
    }

    public async Task<ContratistaDto?> ObtenerContratistaPorIdAsync(long identificacion)
    {
        return await _adapter.ObtenerContratistaPorIdAsync(identificacion);
    }

    #endregion

    #region CRUD Contratistas

    public async Task<(bool Success, string Message)> GuardarContratistaAsync(ContratistaCreateEditDto dto, long usuarioId)
    {
        try
        {
            // Validaciones
            if (dto.Estado <= 0)
            {
                return (false, "Seleccione el estado del contratista");
            }
            
            if (dto.NumeroSymphony <= 0)
            {
                return (false, "Ingrese el número de Symphony");
            }
            
            if (dto.CiudadId <= 0)
            {
                return (false, "Seleccione la ciudad del contratista");
            }
            
            if (dto.Clasificacion <= 0)
            {
                return (false, "Seleccione la clasificación del contratista");
            }

            if (dto.EsActualizacion)
            {
                // Obtener datos actuales para log
                var contratistaActual = await _adapter.ObtenerContratistaPorIdAsync(dto.Identificacion);
                
                await _adapter.ActualizarContratistaAsync(dto);
                
                // Registrar cambios en log
                await RegistrarCambiosLogAsync(contratistaActual, dto, usuarioId);
                
                _logger.LogInformation("Contratista {Identificacion} actualizado por usuario {UserId}", dto.Identificacion, usuarioId);
                return (true, "Contratista actualizado exitosamente");
            }
            else
            {
                // Verificar si ya existe
                var existe = await _adapter.ExisteContratistaAsync(dto.Identificacion);
                if (existe)
                {
                    return (false, "Ya existe un contratista con esa identificación");
                }
                
                await _adapter.GuardarContratistaAsync(dto);
                
                // Log de creación
                await _adapter.AgregarLogContratistaAsync(dto.Identificacion, "Contratista creado", usuarioId);
                
                _logger.LogInformation("Contratista {Identificacion} creado por usuario {UserId}", dto.Identificacion, usuarioId);
                return (true, "Contratista guardado exitosamente");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar contratista {Identificacion}. UserId: {UserId}", dto.Identificacion, usuarioId);
            return (false, "Error al guardar el contratista. Por favor intente nuevamente.");
        }
    }

    private async Task RegistrarCambiosLogAsync(ContratistaDto? actual, ContratistaCreateEditDto nuevo, long usuarioId)
    {
        if (actual == null) return;
        
        // Comparar y registrar cambios
        if (actual.Nombre != nuevo.Nombre)
        {
            await _adapter.AgregarLogContratistaAsync(nuevo.Identificacion, 
                $"Nombre actualizado: {actual.Nombre} -> {nuevo.Nombre}", usuarioId);
        }
        
        if (actual.Clasificacion != nuevo.Clasificacion)
        {
            await _adapter.AgregarLogContratistaAsync(nuevo.Identificacion, 
                $"Clasificación actualizada: {actual.Clasificacion} -> {nuevo.Clasificacion}", usuarioId);
        }
        
        if (actual.Direccion != nuevo.Direccion)
        {
            await _adapter.AgregarLogContratistaAsync(nuevo.Identificacion, 
                $"Dirección actualizada: {actual.Direccion} -> {nuevo.Direccion}", usuarioId);
        }
        
        if (actual.Email != nuevo.Email)
        {
            await _adapter.AgregarLogContratistaAsync(nuevo.Identificacion, 
                $"Correo actualizado: {actual.Email} -> {nuevo.Email}", usuarioId);
        }
        
        if (actual.NumeroSymphony != nuevo.NumeroSymphony)
        {
            await _adapter.AgregarLogContratistaAsync(nuevo.Identificacion, 
                $"Symphony actualizado: {actual.NumeroSymphony} -> {nuevo.NumeroSymphony}", usuarioId);
        }
        
        if (actual.Telefono != nuevo.Telefono)
        {
            await _adapter.AgregarLogContratistaAsync(nuevo.Identificacion, 
                $"Teléfono actualizado: {actual.Telefono} -> {nuevo.Telefono}", usuarioId);
        }
        
        if (actual.Estado != nuevo.Estado)
        {
            var estadoTexto = nuevo.Estado switch
            {
                1 => "Activado",
                2 => "Inactivado",
                3 => "Retirado",
                _ => nuevo.Estado.ToString()
            };
            await _adapter.AgregarLogContratistaAsync(nuevo.Identificacion, 
                $"Estado actualizado: {estadoTexto}", usuarioId);
        }
    }

    public async Task<(bool Success, string Message)> ActualizarEstadoAsync(long identificacion, int estado, long usuarioId)
    {
        try
        {
            await _adapter.ActualizarEstadoContratistaAsync(identificacion, estado);
            
            var estadoTexto = estado switch
            {
                1 => "Activado",
                2 => "Inactivado",
                3 => "Retirado",
                _ => estado.ToString()
            };
            
            await _adapter.AgregarLogContratistaAsync(identificacion, $"Estado actualizado: {estadoTexto}", usuarioId);
            
            _logger.LogInformation("Estado de contratista {Identificacion} actualizado a {Estado} por usuario {UserId}", 
                identificacion, estado, usuarioId);
            return (true, "Estado actualizado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando estado de contratista {Identificacion}", identificacion);
            return (false, "Error al actualizar el estado");
        }
    }

    #endregion

    #region Servicios de Contratista

    public async Task<IEnumerable<ContratistaServicioDto>> ObtenerServiciosContratistaAsync(long identificacion)
    {
        return await _adapter.ObtenerServiciosContratistaAsync(identificacion);
    }

    public async Task<(bool Success, string Message)> AgregarServicioAsync(ContratistaServicioCreateDto dto, long usuarioId)
    {
        try
        {
            await _adapter.AgregarServicioContratistaAsync(dto);
            await _adapter.AgregarLogContratistaAsync(dto.ContratistaId, 
                $"Servicio agregado: {dto.NombreServicio} (ID: {dto.ServicioId})", usuarioId);
            
            return (true, "Servicio agregado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error agregando servicio a contratista {ContratistaId}", dto.ContratistaId);
            return (false, "Error al agregar el servicio");
        }
    }

    public async Task<(bool Success, string Message)> ActualizarEstadoServicioAsync(long id, bool estado, long contratistaId, long usuarioId)
    {
        try
        {
            await _adapter.ActualizarEstadoServicioAsync(id, estado);
            await _adapter.AgregarLogContratistaAsync(contratistaId, 
                $"Estado de servicio {id} actualizado: {(estado ? "Activo" : "Inactivo")}", usuarioId);
            
            return (true, "Estado del servicio actualizado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando estado de servicio {Id}", id);
            return (false, "Error al actualizar el estado del servicio");
        }
    }

    #endregion

    #region Log

    public async Task<IEnumerable<ContratistaLogDto>> ObtenerLogContratistasAsync(long? contratistaId, string? nombre)
    {
        return await _adapter.ObtenerLogContratistasAsync(contratistaId, nombre);
    }

    #endregion

    #region Combos

    public async Task<IEnumerable<EstadoContratistaDto>> ObtenerEstadosAsync()
    {
        return await _adapter.ObtenerEstadosAsync();
    }

    public async Task<IEnumerable<ServicioContratistaComboDto>> ObtenerServiciosComboAsync()
    {
        return await _adapter.ObtenerServiciosComboAsync(null);
    }

    public async Task<IEnumerable<ClasificacionContratistaDto>> ObtenerClasificacionesAsync()
    {
        return await _adapter.ObtenerClasificacionesAsync();
    }

    public async Task<IEnumerable<CiudadComboDto>> ObtenerCiudadesAsync()
    {
        return await _adapter.ObtenerCiudadesAsync();
    }

    #endregion
}
