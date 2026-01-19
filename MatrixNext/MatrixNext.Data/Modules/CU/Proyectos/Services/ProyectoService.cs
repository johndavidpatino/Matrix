using Microsoft.Extensions.Logging;
using MatrixNext.Data.Modules.CU.Proyectos.Adapters;
using MatrixNext.Data.Modules.CU.Proyectos.Models;

namespace MatrixNext.Data.Modules.CU.Proyectos.Services;

/// <summary>
/// Interface para el servicio de Proyectos
/// </summary>
public interface IProyectoService
{
    Task<IEnumerable<ProyectoDto>> ObtenerProyectosAsync(ProyectoBusquedaParams? filtros = null);
    Task<ProyectoDto?> ObtenerProyectoPorIdAsync(long id);
    Task<IEnumerable<ProyectoDto>> ObtenerProyectosPorEstudioAsync(long estudioId);
    Task<(bool Success, string Message, long Id)> CrearProyectoAsync(ProyectoCreateEditDto dto);
    Task<(bool Success, string Message)> ActualizarProyectoAsync(ProyectoCreateEditDto dto);
    Task<(bool Success, string Message)> ActualizarGerenteProyectoAsync(long id, long gerenteProyectos);
    Task<ProyectosIndexViewModel> PrepararViewModelAsync(long estudioId);
}

/// <summary>
/// Servicio de Proyectos - Lógica de negocio
/// </summary>
public class ProyectoService : IProyectoService
{
    private readonly IProyectoAdapter _adapter;
    private readonly ILogger<ProyectoService> _logger;

    public ProyectoService(IProyectoAdapter adapter, ILogger<ProyectoService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    public async Task<IEnumerable<ProyectoDto>> ObtenerProyectosAsync(ProyectoBusquedaParams? filtros = null)
    {
        return await _adapter.ObtenerProyectosAsync(filtros);
    }

    public async Task<ProyectoDto?> ObtenerProyectoPorIdAsync(long id)
    {
        return await _adapter.ObtenerProyectoPorIdAsync(id);
    }

    public async Task<IEnumerable<ProyectoDto>> ObtenerProyectosPorEstudioAsync(long estudioId)
    {
        return await _adapter.ObtenerProyectosPorEstudioAsync(estudioId);
    }

    public async Task<(bool Success, string Message, long Id)> CrearProyectoAsync(ProyectoCreateEditDto dto)
    {
        try
        {
            // Validar JobBook
            if (string.IsNullOrWhiteSpace(dto.JobBook))
            {
                return (false, "El JobBook es requerido", 0);
            }

            // Validar que no termine en 00 (según WebMatrix)
            if (dto.JobBook.EndsWith("00"))
            {
                return (false, "Debe escribir un número de JobBook válido antes de continuar", 0);
            }

            // Verificar si ya existe un proyecto con el mismo JobBook
            var existente = await _adapter.ObtenerProyectoPorJobBookAsync(dto.JobBook);
            if (existente != null)
            {
                return (false, $"Ya existe un proyecto con el JobBook {dto.JobBook}", 0);
            }

            var id = await _adapter.CrearProyectoAsync(dto);
            
            _logger.LogInformation("Proyecto creado exitosamente: {ProyectoId}, JobBook: {JobBook}", id, dto.JobBook);
            return (true, "Proyecto creado exitosamente", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando proyecto. Dto: {@Dto}", dto);
            return (false, "Error al crear el proyecto", 0);
        }
    }

    public async Task<(bool Success, string Message)> ActualizarProyectoAsync(ProyectoCreateEditDto dto)
    {
        try
        {
            // Validar que existe
            var existente = await _adapter.ObtenerProyectoPorIdAsync(dto.Id);
            if (existente == null)
            {
                return (false, "El proyecto no existe");
            }

            // Validar JobBook
            if (string.IsNullOrWhiteSpace(dto.JobBook))
            {
                return (false, "El JobBook es requerido");
            }

            if (dto.JobBook.EndsWith("00"))
            {
                return (false, "Debe escribir un número de JobBook válido antes de continuar");
            }

            // Verificar si otro proyecto ya tiene este JobBook
            var otroProyecto = await _adapter.ObtenerProyectoPorJobBookAsync(dto.JobBook);
            if (otroProyecto != null && otroProyecto.Id != dto.Id)
            {
                return (false, $"Ya existe otro proyecto con el JobBook {dto.JobBook}");
            }

            await _adapter.ActualizarProyectoAsync(dto);
            
            _logger.LogInformation("Proyecto actualizado: {ProyectoId}", dto.Id);
            return (true, "Proyecto actualizado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando proyecto {ProyectoId}", dto.Id);
            return (false, "Error al actualizar el proyecto");
        }
    }

    public async Task<(bool Success, string Message)> ActualizarGerenteProyectoAsync(long id, long gerenteProyectos)
    {
        try
        {
            var existente = await _adapter.ObtenerProyectoPorIdAsync(id);
            if (existente == null)
            {
                return (false, "El proyecto no existe");
            }

            await _adapter.ActualizarGerenteProyectoAsync(id, gerenteProyectos);
            
            _logger.LogInformation("Gerente actualizado para proyecto {ProyectoId}: {GerenteId}", id, gerenteProyectos);
            return (true, "Gerente de proyecto actualizado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando gerente del proyecto {ProyectoId}", id);
            return (false, "Error al actualizar el gerente del proyecto");
        }
    }

    public async Task<ProyectosIndexViewModel> PrepararViewModelAsync(long estudioId)
    {
        var proyectos = await _adapter.ObtenerProyectosPorEstudioAsync(estudioId);
        var tiposProyecto = await _adapter.ObtenerTiposProyectoAsync();
        var unidades = await _adapter.ObtenerUnidadesAsync();

        return new ProyectosIndexViewModel
        {
            EstudioId = estudioId,
            Proyectos = proyectos,
            TiposProyecto = tiposProyecto,
            Unidades = unidades
        };
    }
}
