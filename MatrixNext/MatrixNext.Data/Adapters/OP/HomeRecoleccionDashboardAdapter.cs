using Dapper;
using MatrixNext.Data.Models.OP;
using MatrixNext.Data.Context;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MatrixNext.Data.Adapters.OP;

/// <summary>
/// Implementación del adapter para Dashboard HomeRecoleccion
/// Acceso a datos mediante Dapper + Stored Procedures
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.4
/// </summary>
public class HomeRecoleccionDashboardAdapter : IHomeRecoleccionDashboardAdapter
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeRecoleccionDashboardAdapter> _logger;

    public HomeRecoleccionDashboardAdapter(
        ApplicationDbContext context,
        ILogger<HomeRecoleccionDashboardAdapter> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene trabajos activos filtrando por unidad (si aplica)
    /// SP: OP_Trabajos_Activos (basada en OP_Trabajos_Get de CoreProject)
    /// </summary>
    public async Task<IEnumerable<TrabajoActivoDashboardDto>> ObtenerTrabajosActivosAsync(long? idUnidad = null, int? limite = 10)
    {
        try
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@IdUnidad", idUnidad);
            parameters.Add("@Limite", limite ?? 10);
            parameters.Add("@Estado", "Activo"); // Solo trabajos activos

            var result = await connection.QueryAsync<TrabajoActivoDashboardDto>(
                "OP_Trabajos_Activos",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("Trabajos activos obtenidos. Unidad: {IdUnidad}, Total: {Total}", 
                idUnidad, result.Count());

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo trabajos activos. Unidad: {IdUnidad}", idUnidad);
            return Enumerable.Empty<TrabajoActivoDashboardDto>();
        }
    }

    /// <summary>
    /// Calcula y retorna métricas consolidadas del dashboard
    /// Agrupa por estado: Activos, Pausados, Completados, En Riesgo
    /// </summary>
    public async Task<IEnumerable<DashboardMetricaDto>> ObtenerMetricasAsync(long? idUnidad = null)
    {
        try
        {
            using var connection = _context.CreateConnection();

            // Obtener trabajos para calcular estadísticas
            var parameters = new DynamicParameters();
            parameters.Add("@IdUnidad", idUnidad);

            var trabajos = await connection.QueryAsync<dynamic>(
                @"SELECT 
                    Estado, 
                    COUNT(*) AS Total
                FROM PY_Trabajos
                WHERE (Estado IN ('Activo', 'Pausado', 'Completado', 'En Riesgo'))
                  AND (@IdUnidad IS NULL OR IdUnidad = @IdUnidad)
                GROUP BY Estado",
                parameters
            );

            var metricas = new List<DashboardMetricaDto>
            {
                // Trabajos activos
                new DashboardMetricaDto
                {
                    Etiqueta = "Trabajos Activos",
                    Valor = trabajos.FirstOrDefault(x => x.Estado == "Activo")?.Total ?? 0,
                    Icono = "fas fa-hourglass-half",
                    Color = "primary",
                    Descripcion = "Trabajos en ejecución activa"
                },

                // Trabajos pausados
                new DashboardMetricaDto
                {
                    Etiqueta = "Trabajos en Pausa",
                    Valor = trabajos.FirstOrDefault(x => x.Estado == "Pausado")?.Total ?? 0,
                    Icono = "fas fa-pause-circle",
                    Color = "warning",
                    Descripcion = "Trabajos temporalmente pausados"
                },

                // Trabajos completados
                new DashboardMetricaDto
                {
                    Etiqueta = "Trabajos Completados",
                    Valor = trabajos.FirstOrDefault(x => x.Estado == "Completado")?.Total ?? 0,
                    Icono = "fas fa-check-circle",
                    Color = "success",
                    Descripcion = "Trabajos finalizados"
                },

                // Trabajos en riesgo
                new DashboardMetricaDto
                {
                    Etiqueta = "En Riesgo",
                    Valor = trabajos.FirstOrDefault(x => x.Estado == "En Riesgo")?.Total ?? 0,
                    Icono = "fas fa-exclamation-triangle",
                    Color = "danger",
                    Descripcion = "Trabajos con avance bajo o próximos a vencer"
                }
            };

            _logger.LogInformation("Métricas calculadas para dashboard. Unidad: {IdUnidad}", idUnidad);

            return metricas;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculando métricas dashboard. Unidad: {IdUnidad}", idUnidad);
            return Enumerable.Empty<DashboardMetricaDto>();
        }
    }

    /// <summary>
    /// Obtiene producción diaria de los últimos N días
    /// Compara planeado vs ejecutado para mostrar tendencia
    /// </summary>
    public async Task<IEnumerable<ProduccionDiariaDto>> ObtenerProduccionDiariaAsync(int diasAtras = 7)
    {
        try
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@DiasAtras", diasAtras);

            var result = await connection.QueryAsync<ProduccionDiariaDto>(
                @"SELECT TOP (@DiasAtras)
                    CAST(FechaEjecucion AS DATE) AS Fecha,
                    SUM(CASE WHEN Tipo = 'Planeada' THEN Cantidad ELSE 0 END) AS EncuestasPlaneadas,
                    SUM(CASE WHEN Tipo = 'Ejecutada' THEN Cantidad ELSE 0 END) AS EncuestasEjecutadas,
                    SUM(CASE WHEN Tipo = 'Ejecutada' THEN Cantidad ELSE 0 END) -
                    SUM(CASE WHEN Tipo = 'Planeada' THEN Cantidad ELSE 0 END) AS Diferencia
                FROM OP_ProduccionDiaria
                WHERE FechaEjecucion >= DATEADD(DAY, -@DiasAtras, CAST(GETDATE() AS DATE))
                GROUP BY CAST(FechaEjecucion AS DATE)
                ORDER BY Fecha DESC",
                parameters
            );

            // Calcular porcentaje
            foreach (var item in result)
            {
                item.ProcentajeAvance = item.EncuestasPlaneadas > 0
                    ? (decimal)item.EncuestasEjecutadas / item.EncuestasPlaneadas * 100
                    : 0;
            }

            _logger.LogInformation("Producción diaria obtenida. Días: {DiasAtras}, Registros: {Total}", 
                diasAtras, result.Count());

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo producción diaria. DiasAtras: {DiasAtras}", diasAtras);
            return Enumerable.Empty<ProduccionDiariaDto>();
        }
    }

    /// <summary>
    /// Obtiene trabajos que están en riesgo (bajo avance o próximos a vencer)
    /// Criterio: Avance < 50% y ≤ 7 días para finalizar
    /// </summary>
    public async Task<IEnumerable<TrabajoActivoDashboardDto>> ObtenerTrabajosEnRiesgoAsync(long? idUnidad = null)
    {
        try
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@IdUnidad", idUnidad);
            parameters.Add("@DiasRestantes", 7);

            var result = await connection.QueryAsync<TrabajoActivoDashboardDto>(
                @"SELECT 
                    t.IdTrabajo,
                    t.NumeroTrabajo,
                    p.CodigoProyecto,
                    p.NombreProyecto,
                    t.Estado,
                    fc.Metodologia,
                    fc.MetaEncuestas,
                    ISNULL(pr.EncuestasActuales, 0) AS EncuestasActuales,
                    CAST((ISNULL(pr.EncuestasActuales, 0) * 100.0 / NULLIF(fc.MetaEncuestas, 0)) AS DECIMAL(5,2)) AS AvancePercentual,
                    t.FechaInicio,
                    t.FechaFin AS FechaFinaProgramada,
                    u.NombreCompleto AS CoordinadorNombre,
                    t.IdUnidad,
                    un.NombreUnidad
                FROM PY_Trabajos t
                INNER JOIN PY_Proyectos p ON t.IdProyecto = p.IdProyecto
                LEFT JOIN OP_FichaCuantitativo fc ON t.IdTrabajo = fc.IdTrabajo
                LEFT JOIN (SELECT IdTrabajo, COUNT(*) AS EncuestasActuales FROM OP_RegistrosProduccion GROUP BY IdTrabajo) pr ON t.IdTrabajo = pr.IdTrabajo
                LEFT JOIN TH_Usuario u ON t.IdCoordinador = u.IdUsuario
                LEFT JOIN CS_Unidad un ON t.IdUnidad = un.IdUnidad
                WHERE t.Estado = 'Activo'
                  AND (@IdUnidad IS NULL OR t.IdUnidad = @IdUnidad)
                  AND (
                    (ISNULL(pr.EncuestasActuales, 0) * 100.0 / NULLIF(fc.MetaEncuestas, 0)) < 50
                    OR DATEDIFF(DAY, GETDATE(), t.FechaFin) <= @DiasRestantes
                  )
                ORDER BY AvancePercentual ASC",
                parameters
            );

            _logger.LogInformation("Trabajos en riesgo obtenidos. Unidad: {IdUnidad}, Total: {Total}", 
                idUnidad, result.Count());

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo trabajos en riesgo. Unidad: {IdUnidad}", idUnidad);
            return Enumerable.Empty<TrabajoActivoDashboardDto>();
        }
    }

    /// <summary>
    /// Obtiene resumen de carga por coordinador
    /// Útil para identificar cuellos de botella
    /// </summary>
    public async Task<IEnumerable<(string NombreCoordinador, int TrabajosAsignados, int EncuestasPlaneadas)>> ObtenerCargaCoordinadoresAsync()
    {
        try
        {
            using var connection = _context.CreateConnection();

            var result = await connection.QueryAsync<dynamic>(
                @"SELECT 
                    u.NombreCompleto AS NombreCoordinador,
                    COUNT(DISTINCT t.IdTrabajo) AS TrabajosAsignados,
                    SUM(ISNULL(fc.MetaEncuestas, 0)) AS EncuestasPlaneadas
                FROM PY_Trabajos t
                LEFT JOIN TH_Usuario u ON t.IdCoordinador = u.IdUsuario
                LEFT JOIN OP_FichaCuantitativo fc ON t.IdTrabajo = fc.IdTrabajo
                WHERE t.Estado = 'Activo'
                GROUP BY u.NombreCompleto, t.IdCoordinador
                ORDER BY COUNT(DISTINCT t.IdTrabajo) DESC"
            );

            var carga = result.Select(r => 
            (
                NombreCoordinador: (string)(r.NombreCoordinador ?? "Sin Asignar"),
                TrabajosAsignados: (int)(r.TrabajosAsignados ?? 0),
                EncuestasPlaneadas: (int)(r.EncuestasPlaneadas ?? 0)
            )).ToList();

            _logger.LogInformation("Carga de coordinadores obtenida. Total coordinadores: {Total}", carga.Count);

            return carga;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo carga de coordinadores");
            return Enumerable.Empty<(string, int, int)>();
        }
    }
}
