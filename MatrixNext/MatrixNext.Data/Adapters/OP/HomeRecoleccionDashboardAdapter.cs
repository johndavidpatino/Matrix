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
    /// SP: OP_Trabajos_Get (SP real - OP_Trabajos_Activos no existe)
    /// </summary>
    public async Task<IEnumerable<TrabajoActivoDashboardDto>> ObtenerTrabajosActivosAsync(long? idUnidad = null, int? limite = 10)
    {
        try
        {
            using var connection = _context.CreateConnection();

            // SP OP_Trabajos_Activos no existe - usar OP_Trabajos_Get con filtro de estado
            var parameters = new DynamicParameters();
            parameters.Add("@Id", (long?)null);
            parameters.Add("@IdUnidad", idUnidad);
            parameters.Add("@Estado", (short?)1); // 1 = Activo

            var result = await connection.QueryAsync<TrabajoActivoDashboardDto>(
                "OP_Trabajos_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            // Aplicar límite en memoria
            var limitados = result.Take(limite ?? 10);

            _logger.LogInformation("Trabajos activos obtenidos. Unidad: {IdUnidad}, Total: {Total}", 
                idUnidad, limitados.Count());

            return limitados;
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

            // Obtener trabajos para calcular estadísticas (CORREGIDO: PY_Trabajo, Unidad)
            var parameters = new DynamicParameters();
            parameters.Add("@IdUnidad", idUnidad);

            var trabajos = await connection.QueryAsync<dynamic>(
                @"SELECT 
                    Estado, 
                    COUNT(*) AS Total
                FROM PY_Trabajo
                WHERE (Estado IN ('Activo', 'Pausado', 'Completado', 'En Riesgo'))
                  AND (@IdUnidad IS NULL OR Unidad = @IdUnidad)
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
    /// CORREGIDO: OP_ProduccionDiaria no existe - usar OP_Produccion con Fecha
    /// </summary>
    public async Task<IEnumerable<ProduccionDiariaDto>> ObtenerProduccionDiariaAsync(int diasAtras = 7)
    {
        try
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@DiasAtras", diasAtras);

            // CORREGIDO: Usar OP_Produccion con columna Fecha (tabla real)
            var result = await connection.QueryAsync<ProduccionDiariaDto>(
                @"SELECT TOP (@DiasAtras)
                    CAST(Fecha AS DATE) AS Fecha,
                    0 AS EncuestasPlaneadas, -- No existe columna planeada en OP_Produccion
                    SUM(Cantidad) AS EncuestasEjecutadas,
                    SUM(Cantidad) AS Diferencia
                FROM OP_Produccion
                WHERE Fecha >= DATEADD(DAY, -@DiasAtras, CAST(GETDATE() AS DATE))
                GROUP BY CAST(Fecha AS DATE)
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

            // CORREGIDO: PY_Trabajos → PY_Trabajo, OP_RegistrosProduccion → OP_Produccion (TrabajoId)
            var result = await connection.QueryAsync<TrabajoActivoDashboardDto>(
                @"SELECT 
                    t.id AS IdTrabajo,
                    CAST(t.id AS VARCHAR) AS NumeroTrabajo,
                    pr.JobBook AS CodigoProyecto,
                    pr.Nombre AS NombreProyecto,
                    t.Estado,
                    fc.Metodologia,
                    fc.MetaEncuestas,
                    ISNULL(prod.EncuestasActuales, 0) AS EncuestasActuales,
                    CAST((ISNULL(prod.EncuestasActuales, 0) * 100.0 / NULLIF(fc.MetaEncuestas, 0)) AS DECIMAL(5,2)) AS AvancePercentual,
                    t.FechaTentativaInicioCampo AS FechaInicio,
                    t.FechaTentativaFinalizacion AS FechaFinaProgramada,
                    u.NombreUsuario AS CoordinadorNombre,
                    t.Unidad AS IdUnidad,
                    un.Nombre AS NombreUnidad
                FROM PY_Trabajo t
                LEFT JOIN PY_Proyectos pr ON t.ProyectoId = pr.id
                LEFT JOIN OP_FichaCuantitativo fc ON t.id = fc.IdTrabajo
                LEFT JOIN (SELECT TrabajoId, SUM(Cantidad) AS EncuestasActuales FROM OP_Produccion GROUP BY TrabajoId) prod ON t.id = prod.TrabajoId
                LEFT JOIN US_Usuarios u ON t.COE = u.Id
                LEFT JOIN US_Unidades un ON t.Unidad = un.id
                WHERE t.Estado = 1
                  AND (@IdUnidad IS NULL OR t.Unidad = @IdUnidad)
                  AND (
                    (ISNULL(prod.EncuestasActuales, 0) * 100.0 / NULLIF(fc.MetaEncuestas, 0)) < 50
                    OR DATEDIFF(DAY, GETDATE(), t.FechaTentativaFinalizacion) <= @DiasRestantes
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

            // CORREGIDO: PY_Trabajos → PY_Trabajo, TH_Usuario → US_Usuarios
            var result = await connection.QueryAsync<dynamic>(
                @"SELECT 
                    u.NombreUsuario AS NombreCoordinador,
                    COUNT(DISTINCT t.id) AS TrabajosAsignados,
                    SUM(ISNULL(fc.MetaEncuestas, 0)) AS EncuestasPlaneadas
                FROM PY_Trabajo t
                LEFT JOIN US_Usuarios u ON t.COE = u.Id
                LEFT JOIN OP_FichaCuantitativo fc ON t.id = fc.IdTrabajo
                WHERE t.Estado = 1
                GROUP BY u.NombreUsuario, t.COE
                ORDER BY COUNT(DISTINCT t.id) DESC"
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
