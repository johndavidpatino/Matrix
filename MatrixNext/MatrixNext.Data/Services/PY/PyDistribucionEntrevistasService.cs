using MatrixNext.Data.Adapters.PY;
using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Services.PY.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY
{
    /// <summary>
    /// Servicio de distribución de entrevistas cualitativas.
    /// SP verificados en CoreProject: OP_MuestraTrabajosCuali_EntrevistasGet, 
    /// OP_EntrevistasCuali_DistribucionGet, US_UsuariosModeradoresCualitativos
    /// </summary>
    public class PyDistribucionEntrevistasService : IPyDistribucionEntrevistasService
    {
        private readonly IPyDistribucionEntrevistasAdapter _adapter;
        private readonly ILogger<PyDistribucionEntrevistasService> _logger;

        public PyDistribucionEntrevistasService(
            IPyDistribucionEntrevistasAdapter adapter,
            ILogger<PyDistribucionEntrevistasService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene entrevistas pendientes de distribución para un trabajo.
        /// SP: OP_MuestraTrabajosCuali_EntrevistasGet
        /// </summary>
        public async Task<List<EntrevistaCualiDto>> ObtenerEntrevistasPendientes(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            
            try
            {
                var entrevistas = await _adapter.ObtenerEntrevistasPorTrabajo(trabajoId);
                // Retornar todas las entrevistas (filtrado específico depende de la BD)
                return entrevistas.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo entrevistas pendientes. TrabajoId: {TrabajoId}", trabajoId);
                return new List<EntrevistaCualiDto>();
            }
        }

        /// <summary>
        /// Obtiene distribución asignada para un trabajo.
        /// SP: OP_EntrevistasCuali_DistribucionGet
        /// </summary>
        public async Task<List<DistribucionEntrevistaDto>> ObtenerDistribucionAsignada(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            
            try
            {
                // Primero obtener entrevistas del trabajo
                var entrevistas = await _adapter.ObtenerEntrevistasPorTrabajo(trabajoId);
                var distribuciones = new List<DistribucionEntrevistaDto>();
                
                foreach (var entrevista in entrevistas)
                {
                    var distEntrevista = await _adapter.ObtenerDistribucionesPorEntrevista(entrevista.Id);
                    distribuciones.AddRange(distEntrevista);
                }
                
                return distribuciones;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo distribución asignada. TrabajoId: {TrabajoId}", trabajoId);
                return new List<DistribucionEntrevistaDto>();
            }
        }

        public async Task<int> GuardarDistribucion(DistribucionEntrevistaInputDto input, string usuario)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input.TrabajoId <= 0) throw new ArgumentException("TrabajoId requerido", nameof(input.TrabajoId));
            if (string.IsNullOrWhiteSpace(usuario)) throw new ArgumentException("Usuario requerido", nameof(usuario));

            var id = await _adapter.GuardarDistribucion(input);
            if (id > 0)
            {
                await GuardarLogEntrevista((int)id, "CREACIÓN", "Distribución creada", usuario);
                _logger.LogInformation("Distribución {Id} creada. TrabajoId: {TrabajoId}, Usuario: {Usuario}", 
                    id, input.TrabajoId, usuario);
            }
            return (int)id;
        }

        public async Task<bool> ActualizarEstadoDistribucion(int distribucionId, string nuevoEstado, string observaciones)
        {
            if (distribucionId <= 0) throw new ArgumentException("DistribucionId > 0", nameof(distribucionId));
            if (string.IsNullOrWhiteSpace(nuevoEstado)) throw new ArgumentException("NuevoEstado requerido", nameof(nuevoEstado));
            
            // Mapear estado texto a código
            short estadoCode = nuevoEstado.ToUpper() switch
            {
                "PENDIENTE" => 1,
                "EN PROCESO" or "ENPROCESO" => 2,
                "COMPLETADO" or "COMPLETADA" => 3,
                "CANCELADO" or "CANCELADA" => 4,
                _ => 1
            };
            
            await _adapter.ActualizarEstadoDistribucion(distribucionId, estadoCode);
            _logger.LogInformation("Estado distribución {Id} actualizado a {Estado}", distribucionId, nuevoEstado);
            return true;
        }

        public async Task<List<LogEntrevistaCualiDto>> ObtenerLogDistribucion(int distribucionId)
        {
            if (distribucionId <= 0) throw new ArgumentException("DistribucionId > 0", nameof(distribucionId));
            return await _adapter.ObtenerLogEntrevistas(distribucionId);
        }

        public async Task<int> GuardarLogEntrevista(int distribucionId, string evento, string descripcion, string usuario)
        {
            if (distribucionId <= 0) throw new ArgumentException("DistribucionId > 0", nameof(distribucionId));
            if (string.IsNullOrWhiteSpace(evento)) throw new ArgumentException("Evento requerido", nameof(evento));
            if (string.IsNullOrWhiteSpace(usuario)) throw new ArgumentException("Usuario requerido", nameof(usuario));

            await _adapter.GuardarLogEntrevista(distribucionId, distribucionId, usuario, 1, descripcion ?? "");
            return distribucionId;
        }

        /// <summary>
        /// Obtiene moderadores disponibles.
        /// SP: US_UsuariosModeradoresCualitativos
        /// </summary>
        public async Task<List<ModeradorCualiDto>> ObtenerModeradoresDisponibles(DateTime fecha, string zona)
        {
            if (fecha.Date < DateTime.Today) throw new ArgumentException("Fecha no en pasado", nameof(fecha));
            
            try
            {
                var moderadores = await _adapter.ObtenerModeradores();
                // Retornar todos los moderadores (filtrado por zona requiere campo adicional en DTO)
                return moderadores.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo moderadores. Fecha: {Fecha}, Zona: {Zona}", fecha, zona);
                return new List<ModeradorCualiDto>();
            }
        }

        /// <summary>
        /// Obtiene avance de entrevistas calculado desde distribuciones.
        /// </summary>
        public async Task<dynamic> ObtenerAvanceEntrevistas(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            
            try
            {
                var entrevistas = await _adapter.ObtenerEntrevistasPorTrabajo(trabajoId);
                var distribuciones = await ObtenerDistribucionAsignada(trabajoId);
                
                var total = entrevistas.Count;
                var realizadas = distribuciones.Count(d => d.IdEstado == 3); // Estado 3 = Completada
                var pendientes = total - realizadas;
                var porcentaje = total > 0 ? (realizadas * 100 / total) : 0;
                
                return new 
                { 
                    TotalEntrevistas = total, 
                    EntrevistasRealizadas = realizadas, 
                    EntrevistasPendientes = pendientes, 
                    PorcentajeCompletacion = porcentaje 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculando avance. TrabajoId: {TrabajoId}", trabajoId);
                return new { TotalEntrevistas = 0, EntrevistasRealizadas = 0, EntrevistasPendientes = 0, PorcentajeCompletacion = 0 };
            }
        }

        /// <summary>
        /// Valida si la distribución está completa para el trabajo.
        /// </summary>
        public async Task<List<string>> ValidarDistribucionCompleta(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            
            var errores = new List<string>();
            
            try
            {
                var entrevistas = await _adapter.ObtenerEntrevistasPorTrabajo(trabajoId);
                
                if (!entrevistas.Any())
                {
                    errores.Add("No hay entrevistas definidas para este trabajo");
                    return errores;
                }
                
                foreach (var entrevista in entrevistas)
                {
                    var distribuciones = await _adapter.ObtenerDistribucionesPorEntrevista(entrevista.Id);
                    if (!distribuciones.Any())
                    {
                        errores.Add($"Entrevista {entrevista.Id} no tiene distribución asignada");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando distribución. TrabajoId: {TrabajoId}", trabajoId);
                errores.Add("Error al validar la distribución");
            }
            
            return errores;
        }
    }
}
