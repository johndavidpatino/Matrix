// MatrixNext.Web/Services/CORE/WorkFlowService.cs - MÉTODOS DE EXTENSIÓN

using MatrixNext.Web.DTOs.CORE;
using MatrixNext.Web.ViewModels.CORE;
using MatrixNext.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.CORE
{
    /// <summary>
    /// EXTENSIÓN: Métodos para TraficoTareas (Sprint 17 - RE_GT)
    /// Agregados a la clase WorkFlowService existente
    /// </summary>
    public partial class WorkFlowService : IWorkFlowService
    {
        private readonly ILogger<WorkFlowService> _logger;

        /// <summary>
        /// Obtiene tareas de WorkFlow por unidad OP (para TraficoTareas consolidada)
        /// Filtrado por: unidad, estado, prioridad, búsqueda
        /// </summary>
        public async Task<(List<TareasPorUnidadDto> Tareas, int Total)> 
            ObtenerTareasPorUnidadAsync(
                int idUnidad,
                string? estado = null,
                int? prioridad = null,
                string? busqueda = null,
                int page = 1,
                int pageSize = 20)
        {
            try
            {
                _logger.LogInformation(
                    "[TraficoTareas] Obteniendo tareas: Unidad={IdUnidad}, Estado={Estado}, Page={Page}", 
                    idUnidad, estado, page);

                var resultado = await _adapter.ObtenerTareasPorUnidadAsync(
                    idUnidad, estado, prioridad, busqueda, page, pageSize);

                _logger.LogInformation(
                    "[TraficoTareas] Tareas encontradas: {Count} de {Total}", 
                    resultado.Tareas.Count, resultado.Total);

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "[TraficoTareas] Error obteniendo tareas por unidad {IdUnidad}", 
                    idUnidad);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todas las unidades OP disponibles para TraficoTareas
        /// </summary>
        public async Task<List<UnidadTraficoDto>> ObtenerUnidadesTraficoAsync()
        {
            try
            {
                _logger.LogInformation("[TraficoTareas] Obteniendo unidades disponibles");

                // Retornar lista estática de 10 unidades (pueden venir de BD si es necesario)
                return await Task.FromResult(UnidadTraficoDto.ObtenerUnidadesTrafico());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[TraficoTareas] Error obteniendo unidades");
                throw;
            }
        }

        /// <summary>
        /// Obtiene información de un trabajo específico incluyendo tipo de proyecto
        /// </summary>
        public async Task<TrabajoTraficoInfoDto?> ObtenerInformacionTrabajoAsync(long idTrabajo)
        {
            try
            {
                _logger.LogInformation("[TraficoTareas] Obteniendo info trabajo {IdTrabajo}", idTrabajo);

                var resultado = await _adapter.ObtenerInformacionTrabajoAsync(idTrabajo);

                if (resultado != null)
                {
                    _logger.LogInformation(
                        "[TraficoTareas] Trabajo {IdTrabajo} es {Tipo}", 
                        idTrabajo, 
                        resultado.EsProyectoCualitativo ? "Cualitativo" : "Cuantitativo");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[TraficoTareas] Error obteniendo info trabajo {IdTrabajo}", idTrabajo);
                throw;
            }
        }
    }

    /// <summary>
    /// DTO auxiliar para información de trabajo
    /// </summary>
    public class TrabajoTraficoInfoDto
    {
        public long IdTrabajo { get; set; }
        public string NombreTrabajo { get; set; } = string.Empty;
        public string JobBook { get; set; } = string.Empty;
        public bool EsProyectoCualitativo { get; set; }
        public long IdProyecto { get; set; }
        public int IdUnidad { get; set; }
    }
}
