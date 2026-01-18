using MatrixNext.Data.Adapters.PY;
using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Services.PY.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY
{
    /// <summary>
    /// Servicio de trabajos PY.
    /// SP verificados en CoreProject: PY_Trabajo_Get, PY_TrabajosConfiguracionGet,
    /// PY_TrabajosConfiguracion_Add, PY_TrabajosDuplicar
    /// </summary>
    public class PyTrabajosService : IPyTrabajosService
    {
        private readonly IPyTrabajosAdapter _adapter;
        private readonly ILogger<PyTrabajosService> _logger;

        public PyTrabajosService(
            IPyTrabajosAdapter adapter,
            ILogger<PyTrabajosService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene trabajo básico por ID.
        /// </summary>
        public async Task<TrabajoBasicoDto?> ObtenerAsync(long trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));

            try
            {
                // Intentar obtener desde configuración para validar existencia
                var config = await _adapter.ObtenerConfiguracionTrabajo(trabajoId);
                
                return new TrabajoBasicoDto
                {
                    Id = trabajoId,
                    NombreTrabajoPresupuesto = $"Trabajo {trabajoId}",
                    TipoTrabajo = string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo trabajo {TrabajoId}", trabajoId);
                return null;
            }
        }

        public async Task<DuplicarTrabajoResultDto> DuplicarTrabajoCompleto(DuplicarTrabajoInputDto input, string usuario)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input.TrabajoIdOrigen <= 0) throw new ArgumentException("TrabajoIdOrigen requerido", nameof(input.TrabajoIdOrigen));
            if (string.IsNullOrWhiteSpace(input.NombreNuevo)) throw new ArgumentException("NombreNuevo requerido", nameof(input.NombreNuevo));
            if (input.ProyectoIdNuevo <= 0) throw new ArgumentException("ProyectoIdNuevo requerido", nameof(input.ProyectoIdNuevo));

            var result = await _adapter.DuplicarTrabajoCompleto(input);
            _logger.LogInformation("Trabajo {Origen} duplicado a {Nuevo} por {Usuario}", 
                input.TrabajoIdOrigen, result.NuevoTrabajoId, usuario);
            return result;
        }

        /// <summary>
        /// Obtiene configuración de trabajo.
        /// SP: PY_TrabajosConfiguracionGet
        /// </summary>
        public async Task<TrabajoConfiguracionDto> ObtenerConfiguracionTrabajo(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            
            try
            {
                var config = await _adapter.ObtenerConfiguracionTrabajo(trabajoId);
                return config ?? new TrabajoConfiguracionDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo configuración. TrabajoId: {TrabajoId}", trabajoId);
                return new TrabajoConfiguracionDto();
            }
        }

        /// <summary>
        /// Guarda configuración de trabajo.
        /// SP: PY_TrabajosConfiguracion_Add
        /// </summary>
        public async Task<bool> GuardarConfiguracionTrabajo(TrabajoConfiguracionInputDto input, string usuario)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input.TrabajoId <= 0) throw new ArgumentException("TrabajoId requerido", nameof(input.TrabajoId));
            if (string.IsNullOrWhiteSpace(usuario)) throw new ArgumentException("Usuario requerido", nameof(usuario));
            
            try
            {
                // Serializar configuración a JSON para guardar
                var configuracionJson = System.Text.Json.JsonSerializer.Serialize(input);
                await _adapter.GuardarConfiguracionTrabajo(input.TrabajoId, configuracionJson, input.UsuarioId);
                
                _logger.LogInformation("Configuración guardada. TrabajoId: {TrabajoId}, Usuario: {Usuario}", 
                    input.TrabajoId, usuario);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando configuración. TrabajoId: {TrabajoId}", input.TrabajoId);
                return false;
            }
        }

        public async Task<bool> ValidarTrabajoListo(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            var config = await ObtenerConfiguracionTrabajo(trabajoId);
            return config != null && config.TrabajoId > 0;
        }

        /// <summary>
        /// Obtiene estado del trabajo basado en configuración.
        /// </summary>
        public async Task<dynamic> ObtenerEstadoTrabajo(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            
            try
            {
                var config = await _adapter.ObtenerConfiguracionTrabajo(trabajoId);
                
                // Determinar estado basado en existencia de configuración
                var estadoGeneral = config != null && config.TrabajoId > 0 
                    ? "En Progreso" 
                    : "Sin Iniciar";
                
                return new 
                { 
                    EstadoGeneral = estadoGeneral, 
                    EspecificacionesCompletadas = config != null && !string.IsNullOrEmpty(config.Configuracion), 
                    MuestrasValidadas = false, 
                    AvanceEjecucion = 0 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo estado. TrabajoId: {TrabajoId}", trabajoId);
                return new { EstadoGeneral = "Sin Iniciar", EspecificacionesCompletadas = false, MuestrasValidadas = false, AvanceEjecucion = 0 };
            }
        }

        /// <summary>
        /// Cierra trabajo con motivo.
        /// </summary>
        public async Task<bool> CerrarTrabajo(int trabajoId, string motivo, string usuario)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            if (string.IsNullOrWhiteSpace(motivo)) throw new ArgumentException("Motivo requerido", nameof(motivo));
            if (string.IsNullOrWhiteSpace(usuario)) throw new ArgumentException("Usuario requerido", nameof(usuario));
            
            try
            {
                // Guardar cierre en configuración
                var configInput = new TrabajoConfiguracionInputDto
                {
                    TrabajoId = trabajoId,
                    Configuracion = $"{{\"cerrado\": true, \"motivo\": \"{motivo}\", \"usuario\": \"{usuario}\", \"fecha\": \"{DateTime.Now:yyyy-MM-dd HH:mm}\"}}"
                };
                
                return await GuardarConfiguracionTrabajo(configInput, usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cerrando trabajo {TrabajoId}. Usuario: {Usuario}", trabajoId, usuario);
                return false;
            }
        }
    }
}
