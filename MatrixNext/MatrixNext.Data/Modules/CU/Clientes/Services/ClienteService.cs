using Microsoft.Extensions.Logging;
using MatrixNext.Data.Modules.CU.Clientes.Models;
using MatrixNext.Data.Modules.CU.Clientes.Adapters;

namespace MatrixNext.Data.Modules.CU.Clientes.Services;

/// <summary>
/// Interface del servicio de Clientes y Contactos
/// </summary>
public interface IClienteService
{
    // Clientes
    Task<IEnumerable<ClienteDto>> ObtenerClientesAsync(ClienteBusquedaParams? filtros = null);
    Task<ClienteDto?> ObtenerClientePorIdAsync(long id);
    Task<(bool Success, string Message, long Id)> CrearClienteAsync(ClienteCreateEditDto dto, int userId);
    Task<(bool Success, string Message)> ActualizarClienteAsync(ClienteCreateEditDto dto, int userId);

    // Contactos
    Task<IEnumerable<ContactoDto>> ObtenerContactosPorClienteAsync(long idCliente);
    Task<ContactoDto?> ObtenerContactoPorIdAsync(long id);
    Task<(bool Success, string Message, long Id)> CrearContactoAsync(ContactoCreateEditDto dto, int userId);
    Task<(bool Success, string Message)> ActualizarContactoAsync(ContactoCreateEditDto dto, int userId);

    // Catálogos
    Task<IEnumerable<PaisDto>> ObtenerPaisesAsync();
    Task<IEnumerable<DepartamentoDto>> ObtenerDepartamentosPorPaisAsync(int idPais);
    Task<IEnumerable<CiudadDto>> ObtenerCiudadesPorDepartamentoAsync(int idDepartamento);
    Task<IEnumerable<SectorDto>> ObtenerSectoresAsync();
    Task<IEnumerable<TipoClienteDto>> ObtenerTiposClienteAsync();
}

/// <summary>
/// Servicio de Clientes y Contactos - Lógica de negocio
/// </summary>
public class ClienteService : IClienteService
{
    private readonly IClienteAdapter _adapter;
    private readonly ILogger<ClienteService> _logger;

    public ClienteService(IClienteAdapter adapter, ILogger<ClienteService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    #region Clientes

    public async Task<IEnumerable<ClienteDto>> ObtenerClientesAsync(ClienteBusquedaParams? filtros = null)
    {
        return await _adapter.ObtenerClientesAsync(filtros);
    }

    public async Task<ClienteDto?> ObtenerClientePorIdAsync(long id)
    {
        return await _adapter.ObtenerClientePorIdAsync(id);
    }

    public async Task<(bool Success, string Message, long Id)> CrearClienteAsync(ClienteCreateEditDto dto, int userId)
    {
        try
        {
            // Validar que anticipo + saldo = 100
            if (dto.Anticipo + dto.Saldo != 100)
            {
                return (false, "El anticipo más el saldo debe sumar 100", 0);
            }

            // Validar razón social no vacía
            if (string.IsNullOrWhiteSpace(dto.RazonSocial))
            {
                return (false, "La razón social es requerida", 0);
            }

            var id = await _adapter.CrearClienteAsync(dto);

            _logger.LogInformation("Cliente {ClienteId} creado por usuario {UserId}", id, userId);
            return (true, "Cliente creado exitosamente", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear cliente. UserId: {UserId}, Dto: {@Dto}", userId, dto);
            return (false, "Error al crear el cliente", 0);
        }
    }

    public async Task<(bool Success, string Message)> ActualizarClienteAsync(ClienteCreateEditDto dto, int userId)
    {
        try
        {
            // Validar que existe
            if (dto.Id == null || dto.Id <= 0)
            {
                return (false, "ID de cliente inválido");
            }

            // Validar que anticipo + saldo = 100
            if (dto.Anticipo + dto.Saldo != 100)
            {
                return (false, "El anticipo más el saldo debe sumar 100");
            }

            var clienteExistente = await _adapter.ObtenerClientePorIdAsync(dto.Id.Value);
            if (clienteExistente == null)
            {
                return (false, "Cliente no encontrado");
            }

            await _adapter.ActualizarClienteAsync(dto);

            _logger.LogInformation("Cliente {ClienteId} actualizado por usuario {UserId}", dto.Id, userId);
            return (true, "Cliente actualizado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar cliente {ClienteId}. UserId: {UserId}", dto.Id, userId);
            return (false, "Error al actualizar el cliente");
        }
    }

    #endregion

    #region Contactos

    public async Task<IEnumerable<ContactoDto>> ObtenerContactosPorClienteAsync(long idCliente)
    {
        return await _adapter.ObtenerContactosPorClienteAsync(idCliente);
    }

    public async Task<ContactoDto?> ObtenerContactoPorIdAsync(long id)
    {
        return await _adapter.ObtenerContactoPorIdAsync(id);
    }

    public async Task<(bool Success, string Message, long Id)> CrearContactoAsync(ContactoCreateEditDto dto, int userId)
    {
        try
        {
            // Validar que el cliente existe
            var cliente = await _adapter.ObtenerClientePorIdAsync(dto.IdCliente);
            if (cliente == null)
            {
                return (false, "El cliente especificado no existe", 0);
            }

            // Validar nombre no vacío
            if (string.IsNullOrWhiteSpace(dto.Nombre))
            {
                return (false, "El nombre del contacto es requerido", 0);
            }

            var id = await _adapter.CrearContactoAsync(dto);

            _logger.LogInformation("Contacto {ContactoId} creado para cliente {ClienteId} por usuario {UserId}", 
                id, dto.IdCliente, userId);
            return (true, "Contacto creado exitosamente", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear contacto. UserId: {UserId}, Dto: {@Dto}", userId, dto);
            return (false, "Error al crear el contacto", 0);
        }
    }

    public async Task<(bool Success, string Message)> ActualizarContactoAsync(ContactoCreateEditDto dto, int userId)
    {
        try
        {
            // Validar que existe
            if (dto.Id == null || dto.Id <= 0)
            {
                return (false, "ID de contacto inválido");
            }

            var contactoExistente = await _adapter.ObtenerContactoPorIdAsync(dto.Id.Value);
            if (contactoExistente == null)
            {
                return (false, "Contacto no encontrado");
            }

            await _adapter.ActualizarContactoAsync(dto);

            _logger.LogInformation("Contacto {ContactoId} actualizado por usuario {UserId}", dto.Id, userId);
            return (true, "Contacto actualizado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar contacto {ContactoId}. UserId: {UserId}", dto.Id, userId);
            return (false, "Error al actualizar el contacto");
        }
    }

    #endregion

    #region Catálogos

    public async Task<IEnumerable<PaisDto>> ObtenerPaisesAsync()
    {
        return await _adapter.ObtenerPaisesAsync();
    }

    public async Task<IEnumerable<DepartamentoDto>> ObtenerDepartamentosPorPaisAsync(int idPais)
    {
        return await _adapter.ObtenerDepartamentosPorPaisAsync(idPais);
    }

    public async Task<IEnumerable<CiudadDto>> ObtenerCiudadesPorDepartamentoAsync(int idDepartamento)
    {
        return await _adapter.ObtenerCiudadesPorDepartamentoAsync(idDepartamento);
    }

    public async Task<IEnumerable<SectorDto>> ObtenerSectoresAsync()
    {
        return await _adapter.ObtenerSectoresAsync();
    }

    public async Task<IEnumerable<TipoClienteDto>> ObtenerTiposClienteAsync()
    {
        return await _adapter.ObtenerTiposClienteAsync();
    }

    #endregion
}
