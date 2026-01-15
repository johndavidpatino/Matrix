using MatrixNext.Data.Adapters.OP;
using MatrixNext.Data.Models.OP;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.OP
{
    /// <summary>
    /// Servicio para gestión de encuestas (activación/anulación)
    /// Contiene lógica de negocio y validaciones
    /// </summary>
    public class EncuestasService : IEncuestasService
    {
        private readonly IEncuestasAdapter _adapter;
        private readonly ILogger<EncuestasService> _logger;

        public EncuestasService(IEncuestasAdapter adapter, ILogger<EncuestasService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<IEnumerable<EncuestaAnuladaDto>> ObtenerEncuestasAnuladasAsync(long trabajoId)
        {
            try
            {
                return await _adapter.ObtenerEncuestasAnuladasAsync(trabajoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo encuestas anuladas. Trabajo: {TrabajoId}", trabajoId);
                throw;
            }
        }

        public async Task<(bool Success, string Message, long Id)> AnularEncuestaAsync(EncuestaAnuladaDto dto, long usuarioId, long unidadId)
        {
            try
            {
                // Validar que la encuesta no esté ya anulada
                var existeAnulada = await _adapter.ExisteEncuestaAnuladaAsync(dto.TrabajoId, dto.NumeroEncuesta);
                if (existeAnulada)
                {
                    return (false, "La encuesta ya está anulada", 0);
                }

                // Validar en gestión de campo
                var existeGC = await _adapter.ExisteEncuestaAnuladaGestionCampoAsync(dto.TrabajoId, dto.NumeroEncuesta);
                if (existeGC)
                {
                    return (false, "La encuesta ya está anulada en gestión de campo", 0);
                }

                // Asignar datos de auditoría
                dto.UsuarioId = usuarioId;
                dto.UnidadId = unidadId;
                dto.Fecha = DateTime.Now;

                // Anular en tabla principal
                var id = await _adapter.AnularEncuestaAsync(dto);

                // Anular en gestión de campo
                await _adapter.AnularEncuestaGestionCampoAsync(dto.TrabajoId, dto.NumeroEncuesta, dto.Observacion);

                _logger.LogInformation("Encuesta {NumeroEncuesta} anulada exitosamente. Trabajo: {TrabajoId}, Usuario: {UsuarioId}", 
                    dto.NumeroEncuesta, dto.TrabajoId, usuarioId);

                return (true, "Encuesta anulada exitosamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error anulando encuesta. Trabajo: {TrabajoId}, Encuesta: {NumeroEncuesta}, Usuario: {UsuarioId}", 
                    dto.TrabajoId, dto.NumeroEncuesta, usuarioId);
                return (false, "Error al anular la encuesta. Por favor intente nuevamente.", 0);
            }
        }

        public async Task<(bool Success, string Message)> ActivarEncuestaAsync(long trabajoId, long numeroEncuesta, string observacion, long usuarioId)
        {
            try
            {
                // Validar que la encuesta esté anulada
                var existeAnulada = await _adapter.ExisteEncuestaAnuladaAsync(trabajoId, numeroEncuesta);
                if (!existeAnulada)
                {
                    return (false, "La encuesta no está anulada");
                }

                // Activar (eliminar anulación)
                await _adapter.ActivarEncuestaAsync(numeroEncuesta, trabajoId);

                // Actualizar gestión de campo
                await _adapter.ActualizarGestionCampoActivacionAsync(trabajoId, numeroEncuesta, observacion, usuarioId);

                _logger.LogInformation("Encuesta {NumeroEncuesta} activada exitosamente. Trabajo: {TrabajoId}, Usuario: {UsuarioId}", 
                    numeroEncuesta, trabajoId, usuarioId);

                return (true, "Encuesta activada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activando encuesta. Trabajo: {TrabajoId}, Encuesta: {NumeroEncuesta}, Usuario: {UsuarioId}", 
                    trabajoId, numeroEncuesta, usuarioId);
                return (false, "Error al activar la encuesta. Por favor intente nuevamente.");
            }
        }
    }
}
