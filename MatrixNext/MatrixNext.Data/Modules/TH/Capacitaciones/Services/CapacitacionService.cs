using Microsoft.Extensions.Logging;
using MatrixNext.Data.Modules.TH.Capacitaciones.Adapters;
using MatrixNext.Data.Modules.TH.Capacitaciones.Models;

namespace MatrixNext.Data.Modules.TH.Capacitaciones.Services;

public interface ICapacitacionService
{
    Task<IEnumerable<CapacitacionDto>> ObtenerCapacitacionesAsync(long? trabajoId = null);
    Task<CapacitacionDto?> ObtenerCapacitacionPorIdAsync(long id);
    Task<(bool success, string message, long id)> GuardarCapacitacionAsync(CapacitacionCreateEditDto dto);
    Task<(bool success, string message)> EliminarCapacitacionAsync(long id);
    Task<(bool success, string message, long id)> CrearRefuerzoAsync(long capacitacionId);
    
    // Participantes
    Task<IEnumerable<CapacitacionParticipanteDto>> ObtenerParticipantesAsync(long capacitacionId);
    Task<(bool success, string message)> AgregarParticipanteAsync(CapacitacionParticipanteCreateDto dto);
    Task<(bool success, string message)> ActualizarParticipanteAsync(CapacitacionParticipanteUpdateDto dto);
    Task<(bool success, string message)> EliminarParticipanteAsync(long participanteId);
    
    // Búsqueda
    Task<IEnumerable<PersonaCapacitacionDto>> BuscarPersonasAsync(BuscarPersonasCapacitacionParams parametros);
    
    // Combos
    Task<IEnumerable<ResponsableComboDto>> ObtenerResponsablesAsync();
}

public class CapacitacionService : ICapacitacionService
{
    private readonly ICapacitacionAdapter _adapter;
    private readonly ILogger<CapacitacionService> _logger;

    public CapacitacionService(ICapacitacionAdapter adapter, ILogger<CapacitacionService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    public async Task<IEnumerable<CapacitacionDto>> ObtenerCapacitacionesAsync(long? trabajoId = null)
    {
        try
        {
            return await _adapter.ObtenerCapacitacionesAsync(trabajoId: trabajoId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener capacitaciones. TrabajoId: {TrabajoId}", trabajoId);
            return Enumerable.Empty<CapacitacionDto>();
        }
    }

    public async Task<CapacitacionDto?> ObtenerCapacitacionPorIdAsync(long id)
    {
        try
        {
            return await _adapter.ObtenerCapacitacionPorIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener capacitación por ID: {Id}", id);
            return null;
        }
    }

    public async Task<(bool success, string message, long id)> GuardarCapacitacionAsync(CapacitacionCreateEditDto dto)
    {
        try
        {
            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(dto.Ubicacion))
                return (false, "La ubicación es requerida", 0);

            if (dto.Fecha == default)
                return (false, "La fecha es requerida", 0);

            if (dto.Duracion <= 0)
                return (false, "La duración debe ser mayor a 0", 0);

            if (string.IsNullOrWhiteSpace(dto.Actividad))
                return (false, "La actividad es requerida", 0);

            if (dto.ResponsableId <= 0)
                return (false, "El responsable es requerido", 0);

            var id = await _adapter.GuardarCapacitacionAsync(dto);
            
            var accion = dto.Id > 0 ? "actualizada" : "creada";
            _logger.LogInformation("Capacitación {Accion}. ID: {Id}", accion, id);
            
            return (true, $"Capacitación {accion} exitosamente", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar capacitación. ID: {Id}", dto.Id);
            return (false, "Error al guardar la capacitación", 0);
        }
    }

    public async Task<(bool success, string message)> EliminarCapacitacionAsync(long id)
    {
        try
        {
            // Verificar que existe
            var capacitacion = await _adapter.ObtenerCapacitacionPorIdAsync(id);
            if (capacitacion == null)
                return (false, "La capacitación no existe");

            await _adapter.EliminarCapacitacionAsync(id);
            
            _logger.LogInformation("Capacitación eliminada. ID: {Id}", id);
            return (true, "Capacitación eliminada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar capacitación. ID: {Id}", id);
            return (false, "Error al eliminar la capacitación");
        }
    }

    public async Task<(bool success, string message, long id)> CrearRefuerzoAsync(long capacitacionId)
    {
        try
        {
            // Verificar que existe la capacitación original
            var capacitacion = await _adapter.ObtenerCapacitacionPorIdAsync(capacitacionId);
            if (capacitacion == null)
                return (false, "La capacitación original no existe", 0);

            var nuevoId = await _adapter.CrearRefuerzoAsync(capacitacionId);
            
            _logger.LogInformation("Refuerzo creado. ID original: {IdOriginal}, Nuevo ID: {NuevoId}", 
                capacitacionId, nuevoId);
            
            return (true, "Refuerzo creado exitosamente", nuevoId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear refuerzo. CapacitacionId: {CapacitacionId}", capacitacionId);
            return (false, "Error al crear el refuerzo", 0);
        }
    }

    public async Task<IEnumerable<CapacitacionParticipanteDto>> ObtenerParticipantesAsync(long capacitacionId)
    {
        try
        {
            return await _adapter.ObtenerParticipantesAsync(capacitacionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener participantes. CapacitacionId: {CapacitacionId}", capacitacionId);
            return Enumerable.Empty<CapacitacionParticipanteDto>();
        }
    }

    public async Task<(bool success, string message)> AgregarParticipanteAsync(CapacitacionParticipanteCreateDto dto)
    {
        try
        {
            if (dto.CapacitacionId <= 0)
                return (false, "La capacitación es requerida");

            if (dto.ParticipanteId <= 0)
                return (false, "El participante es requerido");

            await _adapter.AgregarParticipanteAsync(dto);
            
            _logger.LogInformation("Participante agregado. CapacitacionId: {CapacitacionId}, ParticipanteId: {ParticipanteId}", 
                dto.CapacitacionId, dto.ParticipanteId);
            
            return (true, "Participante agregado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar participante. CapacitacionId: {CapacitacionId}", dto.CapacitacionId);
            return (false, "Error al agregar el participante");
        }
    }

    public async Task<(bool success, string message)> ActualizarParticipanteAsync(CapacitacionParticipanteUpdateDto dto)
    {
        try
        {
            await _adapter.ActualizarParticipanteAsync(dto);
            
            _logger.LogInformation("Participante actualizado. ID: {Id}", dto.Id);
            return (true, "Participante actualizado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar participante. ID: {Id}", dto.Id);
            return (false, "Error al actualizar el participante");
        }
    }

    public async Task<(bool success, string message)> EliminarParticipanteAsync(long participanteId)
    {
        try
        {
            await _adapter.EliminarParticipanteAsync(participanteId);
            
            _logger.LogInformation("Participante eliminado. ID: {Id}", participanteId);
            return (true, "Participante eliminado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar participante. ID: {Id}", participanteId);
            return (false, "Error al eliminar el participante");
        }
    }

    public async Task<IEnumerable<PersonaCapacitacionDto>> BuscarPersonasAsync(BuscarPersonasCapacitacionParams parametros)
    {
        try
        {
            return await _adapter.BuscarPersonasAsync(parametros);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar personas para capacitación");
            return Enumerable.Empty<PersonaCapacitacionDto>();
        }
    }

    public async Task<IEnumerable<ResponsableComboDto>> ObtenerResponsablesAsync()
    {
        try
        {
            return await _adapter.ObtenerResponsablesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener responsables");
            return Enumerable.Empty<ResponsableComboDto>();
        }
    }
}
