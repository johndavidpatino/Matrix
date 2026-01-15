/// <summary>
/// Service para Maestro de Documentos (GD_MaestroDocumentos)
/// Tipos: 1=ConstrucciÃ³n, 2=ActualizaciÃ³n, 3=AnulaciÃ³n
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md Â§ Sprint 12.3.5
/// </summary>
namespace MatrixNext.Data.Services.GD
{
    using MatrixNext.Data.DTOs.GD;
    using MatrixNext.Data.Adapters.GD;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMaestroDocumentoService
    {
        Task<IEnumerable<MaestroDocumentoDto>> ObtenerMaestrosAsync(long? idProceso = null, bool? activos = null);
        Task<MaestroDocumentoDto> ObtenerMaestroAsync(long idMaestro);
        Task<(bool exitoso, string mensaje, long idMaestro)> CrearMaestroTipo1ConstruccionAsync(MaestroTipo1ConstruccionDto maestro);
        Task<(bool exitoso, string mensaje, long idMaestro)> CrearMaestroTipo2ActualizacionAsync(MaestroTipo2ActualizacionDto maestro);
        Task<(bool exitoso, string mensaje)> AnularMaestroTipo3Async(MaestroTipo3AnulacionDto anulacion);
        Task<(bool exitoso, string mensaje)> ActualizarMaestroAsync(MaestroDocumentoDto maestro);
        Task<(bool exitoso, string mensaje)> DesactivarMaestroAsync(long idMaestro, long usuarioId);
        Task<ResumenMaestrosDto> ObtenerResumenMaestrosAsync();
    }

    public class MaestroDocumentoService : IMaestroDocumentoService
    {
        private readonly IMaestroDocumentoAdapter _adapter;
        private readonly ILogger<MaestroDocumentoService> _logger;

        public MaestroDocumentoService(IMaestroDocumentoAdapter adapter, ILogger<MaestroDocumentoService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<IEnumerable<MaestroDocumentoDto>> ObtenerMaestrosAsync(long? idProceso = null, bool? activos = null)
        {
            try
            {
                return await _adapter.ObtenerMaestrosAsync(idProceso, activos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo maestros. IdProceso: {IdProceso}", idProceso);
                throw;
            }
        }

        public async Task<MaestroDocumentoDto> ObtenerMaestroAsync(long idMaestro)
        {
            try
            {
                return await _adapter.ObtenerMaestroAsync(idMaestro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo maestro. IdMaestro: {IdMaestro}", idMaestro);
                throw;
            }
        }

        public async Task<(bool exitoso, string mensaje, long idMaestro)> CrearMaestroTipo1ConstruccionAsync(MaestroTipo1ConstruccionDto maestro)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(maestro.NombreDocumento))
                    return (false, "Nombre del documento es obligatorio", 0);

                if (maestro.IdProceso <= 0)
                    return (false, "Proceso es obligatorio", 0);

                if (maestro.RegistradoPor <= 0)
                    return (false, "Usuario registrado es obligatorio", 0);

                if (maestro.TiempoRetencionAnios <= 0)
                    return (false, "Tiempo de retenciÃ³n debe ser mayor a 0", 0);

                // Crear maestro Tipo 1
                var idMaestro = await _adapter.CrearMaestroTipo1ConstruccionAsync(maestro);

                _logger.LogInformation("Maestro Tipo 1 creado exitosamente. IdMaestro: {IdMaestro}, Nombre: {Nombre}", 
                    idMaestro, maestro.NombreDocumento);

                return (true, $"Maestro '{maestro.NombreDocumento}' creado exitosamente", idMaestro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando maestro Tipo 1. Nombre: {Nombre}", maestro.NombreDocumento);
                return (false, "Error al crear el maestro", 0);
            }
        }

        public async Task<(bool exitoso, string mensaje, long idMaestro)> CrearMaestroTipo2ActualizacionAsync(MaestroTipo2ActualizacionDto maestro)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(maestro.NombreDocumento))
                    return (false, "Nombre del documento es obligatorio", 0);

                if (maestro.IdMaestroExistente <= 0)
                    return (false, "Debe seleccionar un maestro existente para actualizar", 0);

                if (string.IsNullOrWhiteSpace(maestro.MotivoCambio))
                    return (false, "Motivo del cambio es obligatorio", 0);

                // Verificar que el maestro existente existe
                var maestroExistente = await _adapter.ObtenerMaestroAsync(maestro.IdMaestroExistente);
                if (maestroExistente == null)
                    return (false, "El maestro a actualizar no existe", 0);

                // Crear nueva versiÃ³n
                maestro.MaestroExistenteNombre = maestroExistente.NombreDocumento;
                var idMaestro = await _adapter.CrearMaestroTipo2ActualizacionAsync(maestro);

                _logger.LogInformation("Maestro Tipo 2 creado exitosamente. IdMaestro: {IdMaestro}, IdMaestroExistente: {IdMaestroExistente}, VersiÃ³n: {Version}", 
                    idMaestro, maestro.IdMaestroExistente, maestro.VersionNumero);

                return (true, $"Nueva versiÃ³n '{maestro.VersionNumero}' del maestro creada exitosamente", idMaestro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando maestro Tipo 2. IdMaestroExistente: {IdMaestroExistente}", maestro.IdMaestroExistente);
                return (false, "Error al crear la nueva versiÃ³n", 0);
            }
        }

        public async Task<(bool exitoso, string mensaje)> AnularMaestroTipo3Async(MaestroTipo3AnulacionDto anulacion)
        {
            try
            {
                // Validaciones
                if (anulacion.IdMaestroAnular <= 0)
                    return (false, "Maestro a anular es obligatorio");

                if (string.IsNullOrWhiteSpace(anulacion.MotivoAnulacion))
                    return (false, "Motivo de anulaciÃ³n es obligatorio");

                if (anulacion.UsuarioAnulacion <= 0)
                    return (false, "Usuario de anulaciÃ³n es obligatorio");

                // Verificar que el maestro existe y estÃ¡ activo
                var maestro = await _adapter.ObtenerMaestroAsync(anulacion.IdMaestroAnular);
                if (maestro == null)
                    return (false, "El maestro a anular no existe");

                if (!maestro.Activo)
                    return (false, "El maestro ya estÃ¡ inactivo");

                // Ejecutar anulaciÃ³n
                var resultado = await _adapter.AnularMaestroTipo3Async(anulacion);

                if (!resultado)
                    return (false, "Error al anular el maestro");

                _logger.LogInformation("Maestro Tipo 3 anulado exitosamente. IdMaestro: {IdMaestro}, Motivo: {Motivo}", 
                    anulacion.IdMaestroAnular, anulacion.MotivoAnulacion);

                return (true, $"Maestro '{maestro.NombreDocumento}' anulado exitosamente. Motivo: {anulacion.MotivoAnulacion}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error anulando maestro Tipo 3. IdMaestro: {IdMaestro}", anulacion.IdMaestroAnular);
                return (false, "Error al anular el maestro");
            }
        }

        public async Task<(bool exitoso, string mensaje)> ActualizarMaestroAsync(MaestroDocumentoDto maestro)
        {
            try
            {
                // Validaciones
                if (maestro.IdMaestro <= 0)
                    return (false, "Maestro a actualizar es obligatorio");

                if (string.IsNullOrWhiteSpace(maestro.NombreDocumento))
                    return (false, "Nombre del documento es obligatorio");

                // Verificar que el maestro existe
                var maestroExistente = await _adapter.ObtenerMaestroAsync(maestro.IdMaestro);
                if (maestroExistente == null)
                    return (false, "El maestro a actualizar no existe");

                // Actualizar
                var resultado = await _adapter.ActualizarMaestroAsync(maestro);

                if (!resultado)
                    return (false, "No se realizaron cambios");

                _logger.LogInformation("Maestro actualizado exitosamente. IdMaestro: {IdMaestro}", maestro.IdMaestro);

                return (true, "Maestro actualizado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando maestro. IdMaestro: {IdMaestro}", maestro.IdMaestro);
                return (false, "Error al actualizar el maestro");
            }
        }

        public async Task<(bool exitoso, string mensaje)> DesactivarMaestroAsync(long idMaestro, long usuarioId)
        {
            try
            {
                // Validaciones
                if (idMaestro <= 0)
                    return (false, "Maestro es obligatorio");

                if (usuarioId <= 0)
                    return (false, "Usuario es obligatorio");

                // Verificar que el maestro existe y estÃ¡ activo
                var maestro = await _adapter.ObtenerMaestroAsync(idMaestro);
                if (maestro == null)
                    return (false, "El maestro no existe");

                if (!maestro.Activo)
                    return (false, "El maestro ya estÃ¡ inactivo");

                // Desactivar
                var resultado = await _adapter.DesactivarMaestroAsync(idMaestro, usuarioId);

                if (!resultado)
                    return (false, "Error al desactivar el maestro");

                _logger.LogInformation("Maestro desactivado exitosamente. IdMaestro: {IdMaestro}", idMaestro);

                return (true, "Maestro desactivado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error desactivando maestro. IdMaestro: {IdMaestro}", idMaestro);
                return (false, "Error al desactivar el maestro");
            }
        }

        public async Task<ResumenMaestrosDto> ObtenerResumenMaestrosAsync()
        {
            try
            {
                return await _adapter.ObtenerResumenMaestrosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo resumen de maestros");
                throw;
            }
        }
    }
}


