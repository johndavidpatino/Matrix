using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.OP;
using MatrixNext.Data.Models.OP;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.OP
{
    /// <summary>
    /// Servicio para gestión de revisiones IPS por tarea
    /// </summary>
    public class IpsRevisionService : IIpsRevisionService
    {
        private readonly IIpsRevisionAdapter _adapter;
        private readonly ILogger<IpsRevisionService> _logger;

        public IpsRevisionService(IIpsRevisionAdapter adapter, ILogger<IpsRevisionService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<IEnumerable<IpsRevisionDto>> ObtenerRevisionesAsync(long trabajoId)
        {
            try
            {
                return await _adapter.ObtenerRevisionesAsync(trabajoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo revisiones IPS para trabajo {TrabajoId}", trabajoId);
                throw;
            }
        }

        public async Task<IpsRevisionDto?> ObtenerRevisionAsync(long revisionId)
        {
            try
            {
                return await _adapter.ObtenerRevisionAsync(revisionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo revisión IPS {RevisionId}", revisionId);
                throw;
            }
        }

        public async Task<(bool Success, string Message, long Id)> CrearRevisionAsync(IpsRevisionCreateUpdateDto dto, long usuarioId)
        {
            try
            {
                // Validar datos
                if (string.IsNullOrWhiteSpace(dto.Pregunta))
                {
                    return (false, "La pregunta es requerida", 0);
                }

                var id = await _adapter.CrearRevisionAsync(dto, usuarioId);

                _logger.LogInformation("Revisión IPS creada exitosamente. ID: {Id}, Trabajo: {TrabajoId}, Usuario: {UsuarioId}", 
                    id, dto.TrabajoId, usuarioId);

                return (true, "Revisión IPS creada correctamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando revisión IPS. Trabajo: {TrabajoId}, Usuario: {UsuarioId}", 
                    dto.TrabajoId, usuarioId);
                return (false, "Error al crear la revisión IPS. Por favor intente nuevamente.", 0);
            }
        }

        public async Task<(bool Success, string Message)> ActualizarRevisionAsync(IpsRevisionCreateUpdateDto dto, long usuarioId)
        {
            try
            {
                // Validar datos
                if (string.IsNullOrWhiteSpace(dto.Pregunta))
                {
                    return (false, "La pregunta es requerida");
                }

                var success = await _adapter.ActualizarRevisionAsync(dto, usuarioId);

                if (success)
                {
                    _logger.LogInformation("Revisión IPS actualizada exitosamente. ID: {Id}, Usuario: {UsuarioId}", 
                        dto.Id, usuarioId);
                    return (true, "Revisión IPS actualizada correctamente");
                }

                return (false, "No se pudo actualizar la revisión IPS");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando revisión IPS. ID: {Id}, Usuario: {UsuarioId}", 
                    dto.Id, usuarioId);
                return (false, "Error al actualizar la revisión IPS. Por favor intente nuevamente.");
            }
        }

        public async Task<(bool Success, string Message)> EliminarRevisionAsync(long revisionId, long usuarioId)
        {
            try
            {
                var success = await _adapter.EliminarRevisionAsync(revisionId, usuarioId);

                if (success)
                {
                    _logger.LogInformation("Revisión IPS eliminada exitosamente. ID: {RevisionId}, Usuario: {UsuarioId}", 
                        revisionId, usuarioId);
                    return (true, "Revisión IPS eliminada correctamente");
                }

                return (false, "No se pudo eliminar la revisión IPS");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando revisión IPS. ID: {RevisionId}, Usuario: {UsuarioId}", 
                    revisionId, usuarioId);
                return (false, "Error al eliminar la revisión IPS. Por favor intente nuevamente.");
            }
        }
    }
}
