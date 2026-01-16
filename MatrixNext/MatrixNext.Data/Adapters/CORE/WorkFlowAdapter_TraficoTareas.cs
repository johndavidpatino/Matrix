// MatrixNext.Data/Adapters/CORE/WorkFlowAdapter.cs - MÉTODOS DE EXTENSIÓN

using MatrixNext.Web.DTOs.CORE;
using MatrixNext.Web.ViewModels.CORE;
using Dapper;
using System.Data;

namespace MatrixNext.Data.Adapters.CORE
{
    /// <summary>
    /// EXTENSIÓN: Métodos Adapter para TraficoTareas (Sprint 17)
    /// </summary>
    public partial class WorkFlowDataAdapter
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<WorkFlowDataAdapter> _logger;

        /// <summary>
        /// Obtiene tareas de WorkFlow por unidad OP desde SP legacy
        /// SP: WorkFlow.obtenerTrabajosWorkFlow (@IdUnidad, @TextoBusqueda)
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
                    "[WorkFlowAdapter] Llamando SP obtenerTrabajosWorkFlow: Unidad={IdUnidad}", 
                    idUnidad);

                // Llamar SP legacy con búsqueda
                var allData = await _connection.QueryAsync<TareasPorUnidadDto>(
                    "WorkFlow.obtenerTrabajosWorkFlow",
                    new 
                    { 
                        IdUnidad = idUnidad, 
                        TextoBusqueda = string.IsNullOrEmpty(busqueda) ? (object?)DBNull.Value : busqueda 
                    },
                    commandType: CommandType.StoredProcedure);

                // Aplicar filtros en memoria si es necesario
                var query = allData.AsQueryable();

                if (!string.IsNullOrEmpty(estado))
                    query = query.Where(t => t.Estado == estado);

                if (prioridad.HasValue)
                    query = query.Where(t => t.Prioridad == prioridad.Value);

                // Calcular total antes de paginar
                var total = query.Count();

                // Paginar
                var tareas = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                _logger.LogInformation(
                    "[WorkFlowAdapter] SP retornó {Count} tareas, Total={Total}, Page={Page}", 
                    tareas.Count, total, page);

                return (tareas, total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "[WorkFlowAdapter] Error en ObtenerTareasPorUnidadAsync: Unidad={IdUnidad}", 
                    idUnidad);
                throw;
            }
        }

        /// <summary>
        /// Obtiene información de un trabajo (tipo proyecto, etc)
        /// Usado para validar si mostrar/ocultar btnFichaCuanti
        /// </summary>
        public async Task<TrabajoTraficoInfoDto?> ObtenerInformacionTrabajoAsync(long idTrabajo)
        {
            try
            {
                _logger.LogInformation("[WorkFlowAdapter] Obteniendo info trabajo {IdTrabajo}", idTrabajo);

                // Ejecutar query directa (puede ser SP o query EF)
                const string sql = @"
                    SELECT 
                        t.Id AS IdTrabajo,
                        t.NombreTrabajo,
                        t.JobBook,
                        CASE WHEN tp.id = 1 THEN 0 ELSE 1 END AS EsProyectoCualitativo,
                        t.ProyectoId AS IdProyecto,
                        t.IdUnidad
                    FROM Trabajo t
                    INNER JOIN Proyecto p ON t.ProyectoId = p.id
                    INNER JOIN TipoProyecto tp ON p.TipoProyectoId = tp.id
                    WHERE t.Id = @IdTrabajo";

                var resultado = await _connection.QuerySingleOrDefaultAsync<TrabajoTraficoInfoDto>(
                    sql,
                    new { IdTrabajo = idTrabajo });

                if (resultado == null)
                {
                    _logger.LogWarning("[WorkFlowAdapter] Trabajo no encontrado: {IdTrabajo}", idTrabajo);
                }

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "[WorkFlowAdapter] Error obteniendo info trabajo {IdTrabajo}", 
                    idTrabajo);
                throw;
            }
        }
    }
}
