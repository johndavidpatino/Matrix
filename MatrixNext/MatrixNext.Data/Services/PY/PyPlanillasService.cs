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
    /// Servicio de planillas de moderación e informes.
    /// SP verificados en CoreProject: UU_TecnicasGet, UU_ModeradoresGet,
    /// UU_PlanillaModeracion_Add, UU_PlanillaModeracion_Update, 
    /// UU_PlanillasModeracionExport, UU_PlanillasInformesExport
    /// </summary>
    public class PyPlanillasService : IPyPlanillasService
    {
        private readonly IPyPlanillasAdapter _adapter;
        private readonly ILogger<PyPlanillasService> _logger;

        public PyPlanillasService(
            IPyPlanillasAdapter adapter,
            ILogger<PyPlanillasService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<List<TecnicaDto>> ObtenerTecnicas(string tipoTecnica)
        {
            if (string.IsNullOrWhiteSpace(tipoTecnica)) throw new ArgumentException("TipoTecnica es requerido", nameof(tipoTecnica));
            return await _adapter.ObtenerTecnicas(tipoTecnica);
        }

        /// <summary>
        /// Obtiene moderadores disponibles.
        /// SP: UU_ModeradoresGet
        /// </summary>
        public async Task<List<ModeradorDto>> ObtenerModeradoresDisponibles(DateTime fecha)
        {
            if (fecha.Date < DateTime.Today) throw new ArgumentException("Fecha no puede ser en el pasado", nameof(fecha));
            
            try
            {
                return await _adapter.ObtenerModeradores();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo moderadores. Fecha: {Fecha}", fecha);
                return new List<ModeradorDto>();
            }
        }

        public async Task<int> CrearPlanillaModeracion(PlanillaModeracionInputDto input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var id = await _adapter.CrearPlanillaModeracion(input);
            _logger.LogInformation("Planilla moderación {Id} creada. Job: {Job}", id, input.IdJob);
            return id;
        }

        /// <summary>
        /// Actualiza planilla de moderación (datos básicos).
        /// Este método es para actualizar datos de la planilla, no estados de aprobación.
        /// </summary>
        public async Task<bool> ActualizarPlanillaModeracion(PlanillaModeracionActualizacionDto input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input.IdPlanilla <= 0) throw new ArgumentException("IdPlanilla es requerido", nameof(input.IdPlanilla));
            
            try
            {
                // Para actualizaciones de estado, usar ActualizarEstadoPlanillaInputDto
                var updateInput = new ActualizarEstadoPlanillaInputDto
                {
                    IdPlanilla = input.IdPlanilla,
                    IdEstado = 1, // Pendiente por defecto
                    Observaciones = input.Observaciones ?? string.Empty
                };
                
                await _adapter.ActualizarPlanillaModeracion(updateInput);
                _logger.LogInformation("Planilla moderación {Id} actualizada", input.IdPlanilla);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando planilla moderación {Id}", input.IdPlanilla);
                return false;
            }
        }

        /// <summary>
        /// Obtiene planillas de informes para rango de fechas.
        /// SP: UU_PlanillasInformesExport
        /// </summary>
        public async Task<List<PlanillaInformesDto>> ObtenerPlanillasInformes(DateTime fechaInicio, DateTime fechaFinal)
        {
            if (fechaFinal < fechaInicio) throw new ArgumentException("FechaFinal >= FechaInicio", nameof(fechaFinal));
            
            try
            {
                return await _adapter.ObtenerPlanillasInformesParaExportar(fechaInicio, fechaFinal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo planillas informes. Rango: {Inicio} - {Fin}", fechaInicio, fechaFinal);
                return new List<PlanillaInformesDto>();
            }
        }

        /// <summary>
        /// Actualiza estado de planilla de informes.
        /// SP: UU_PlanillaInformes_Update
        /// </summary>
        public async Task<bool> ActualizarEstadoPlanillaInformes(int idPlanilla, string nuevoEstado)
        {
            if (idPlanilla <= 0) throw new ArgumentException("IdPlanilla > 0", nameof(idPlanilla));
            if (string.IsNullOrWhiteSpace(nuevoEstado)) throw new ArgumentException("NuevoEstado requerido", nameof(nuevoEstado));
            
            try
            {
                // Mapear estado a código
                short estadoCode = nuevoEstado.ToUpper() switch
                {
                    "PENDIENTE" => 1,
                    "APROBADO" or "APROBADA" => 2,
                    "RECHAZADO" or "RECHAZADA" => 3,
                    "EXPORTADO" or "EXPORTADA" => 4,
                    _ => 1
                };
                
                var input = new ActualizarEstadoPlanillaInputDto
                {
                    IdPlanilla = idPlanilla,
                    IdEstado = estadoCode
                };
                
                await _adapter.ActualizarPlanillaInformes(input);
                _logger.LogInformation("Planilla informes {Id} estado actualizado a {Estado}", idPlanilla, nuevoEstado);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando estado planilla informes {Id}", idPlanilla);
                return false;
            }
        }

        /// <summary>
        /// Obtiene planillas para exportar.
        /// SP: UU_PlanillasModeracionExport
        /// </summary>
        public async Task<List<PlanillaListDto>> ObtenerPlanillasParaExportar(DateTime fechaInicio, DateTime fechaFinal)
        {
            if (fechaFinal < fechaInicio) throw new ArgumentException("FechaFinal >= FechaInicio", nameof(fechaFinal));
            
            try
            {
                var moderacion = await _adapter.ObtenerPlanillasModeracionParaExportar(fechaInicio, fechaFinal);
                
                // Mapear a PlanillaListDto
                return moderacion.Select(m => new PlanillaListDto
                {
                    Id = m.Id,
                    IdJob = m.IdJob,
                    JobDesc = m.JobDesc,
                    FechaRegistro = m.Fecha,
                    TipoPlanilla = "Moderación",
                    IdEstado = m.IdEstado,
                    EstadoNombre = m.EstadoNombre
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo planillas para exportar. Rango: {Inicio} - {Fin}", fechaInicio, fechaFinal);
                return new List<PlanillaListDto>();
            }
        }

        /// <summary>
        /// Marca planilla como exportada.
        /// </summary>
        public async Task<bool> MarcarExportada(int idPlanilla)
        {
            if (idPlanilla <= 0) throw new ArgumentException("IdPlanilla > 0", nameof(idPlanilla));
            
            try
            {
                var input = new ActualizarEstadoPlanillaInputDto
                {
                    IdPlanilla = idPlanilla,
                    IdEstado = 4 // Estado Exportado
                };
                
                await _adapter.ActualizarPlanillaModeracion(input);
                _logger.LogInformation("Planilla {Id} marcada como exportada", idPlanilla);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marcando planilla {Id} como exportada", idPlanilla);
                return false;
            }
        }

        /// <summary>
        /// Valida datos de planilla antes de guardar.
        /// </summary>
        public async Task<List<string>> ValidarPlanillaModeracion(int idPlanilla)
        {
            if (idPlanilla <= 0) throw new ArgumentException("IdPlanilla > 0", nameof(idPlanilla));
            
            var errores = new List<string>();
            
            try
            {
                var planilla = await _adapter.ObtenerPlanillaModeracionPorId(idPlanilla);
                
                if (planilla == null)
                {
                    errores.Add("Planilla no encontrada");
                    return errores;
                }
                
                if (planilla.Moderador == null || planilla.Moderador <= 0)
                    errores.Add("Moderador es requerido");
                    
                if (planilla.Fecha == default || planilla.Fecha == null)
                    errores.Add("Fecha es requerida");
                    
                if (string.IsNullOrWhiteSpace(planilla.IdJob))
                    errores.Add("Job es requerido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando planilla {Id}", idPlanilla);
                errores.Add("Error al validar la planilla");
            }
            
            return errores;
        }

        /// <summary>
        /// Obtiene estadísticas de planillas calculadas.
        /// </summary>
        public async Task<dynamic> ObtenerEstadisticasPlanillas(DateTime fechaInicio, DateTime fechaFinal)
        {
            if (fechaFinal < fechaInicio) throw new ArgumentException("FechaFinal >= FechaInicio", nameof(fechaFinal));
            
            try
            {
                var planillas = await _adapter.ObtenerPlanillasModeracionParaExportar(fechaInicio, fechaFinal);
                
                return new 
                { 
                    TotalPlanillas = planillas.Count, 
                    PlanillasCompletadas = planillas.Count(p => p.IdEstado == 4), // Estado 4 = Exportado
                    ModeradoresActivos = planillas.Where(p => p.Moderador.HasValue).Select(p => p.Moderador).Distinct().Count(),
                    TecnicasUtilizadas = planillas.Where(p => p.Tecnica.HasValue).Select(p => p.Tecnica).Distinct().Count()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculando estadísticas. Rango: {Inicio} - {Fin}", fechaInicio, fechaFinal);
                return new { TotalPlanillas = 0, PlanillasCompletadas = 0, ModeradoresActivos = 0, TecnicasUtilizadas = 0 };
            }
        }
    }
}
