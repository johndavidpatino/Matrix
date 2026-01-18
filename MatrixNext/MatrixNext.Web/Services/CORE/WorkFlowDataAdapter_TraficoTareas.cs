// MatrixNext.Web/Services/CORE/WorkFlowDataAdapter_TraficoTareas.cs

using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MatrixNext.Data.DTOs.CORE;

namespace MatrixNext.Web.Services.CORE
{
    /// <summary>
    /// Extensión: Métodos de adapter para TraficoTareas (Sprint 17)
    /// Acceso a datos para listado de tareas consolidadas por unidad
    /// </summary>
    public partial class WorkFlowDataAdapter
    {
        /// <summary>
        /// Obtiene tareas de WorkFlow por unidad OP desde SP legacy
        /// SP: CORE_WorkFlow_Trabajos_Get (SP real - WorkFlow.obtenerTrabajosWorkFlow no existe)
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
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                // SP real: CORE_WorkFlow_Trabajos_Get con parámetros completos
                // WorkFlow.obtenerTrabajosWorkFlow NO existe
                var allData = await connection.QueryAsync<TareasPorUnidadDto>(
                    "CORE_WorkFlow_Trabajos_Get",
                    new 
                    { 
                        Id = (long?)null,
                        HiloId = (long?)null,
                        TareaId = (long?)null,
                        FIniP = (DateTime?)null,
                        FFinP = (DateTime?)null,
                        FIniR = (DateTime?)null,
                        FFinR = (DateTime?)null,
                        UsuarioEstima = (long?)null,
                        UsuarioAsignado = (long?)null,
                        TrabajoId = (long?)null,
                        TodosCampos = string.IsNullOrEmpty(busqueda) ? (string?)null : busqueda,
                        RolEstima = (int?)null,
                        EstadoWorkFlow_Id = (short?)null,
                        unidadEjecuta = (long?)idUnidad
                    },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                // Aplicar filtros en memoria
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

                return (tareas, total);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Error en ObtenerTareasPorUnidadAsync para unidad {idUnidad}", ex);
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
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                // Ejecutar query directa
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

                var resultado = await connection.QuerySingleOrDefaultAsync<TrabajoTraficoInfoDto>(
                    sql,
                    new { IdTrabajo = idTrabajo },
                    commandTimeout: 60);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Error obteniendo información del trabajo {idTrabajo}", ex);
            }
        }
    }
}
