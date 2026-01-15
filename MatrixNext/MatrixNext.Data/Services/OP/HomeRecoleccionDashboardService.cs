using MatrixNext.Data.Adapters.OP;
using MatrixNext.Data.Models.OP;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.OP;

/// <summary>
/// Implementación del servicio de Dashboard HomeRecoleccion
/// Orquesta llamadas a adapter y aplica lógica de negocio
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.4
/// </summary>
public class HomeRecoleccionDashboardService : IHomeRecoleccionDashboardService
{
    private readonly IHomeRecoleccionDashboardAdapter _adapter;
    private readonly ILogger<HomeRecoleccionDashboardService> _logger;

    public HomeRecoleccionDashboardService(
        IHomeRecoleccionDashboardAdapter adapter,
        ILogger<HomeRecoleccionDashboardService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene dashboard completo con todos los componentes
    /// </summary>
    public async Task<HomeRecoleccionDashboardDto> ObtenerDashboardCompletoAsync(long? idUnidad = null)
    {
        try
        {
            var dashboard = new HomeRecoleccionDashboardDto
            {
                FechaConsulta = DateTime.UtcNow,
                PeriodoReporte = GenerarEtiquetaPeriodo()
            };

            // Obtener métricas (sin esperar)
            var metricasTask = _adapter.ObtenerMetricasAsync(idUnidad);

            // Obtener trabajos activos
            var trabajosTask = _adapter.ObtenerTrabajosActivosAsync(idUnidad, 10);

            // Ejecutar en paralelo
            await Task.WhenAll(metricasTask, trabajosTask);

            dashboard.Metricas = (await metricasTask).ToList();
            dashboard.TrabajosActivos = (await trabajosTask).ToList();

            _logger.LogInformation(
                "Dashboard completo obtenido. Unidad: {IdUnidad}, Métricas: {MetricasCount}, Trabajos: {TrabajosCount}",
                idUnidad, dashboard.Metricas.Count, dashboard.TrabajosActivos.Count);

            return dashboard;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo dashboard completo. Unidad: {IdUnidad}", idUnidad);

            // Retornar dashboard vacío con mensaje de error
            return new HomeRecoleccionDashboardDto
            {
                FechaConsulta = DateTime.UtcNow,
                PeriodoReporte = GenerarEtiquetaPeriodo(),
                Metricas = new List<DashboardMetricaDto>(),
                TrabajosActivos = new List<TrabajoActivoDashboardDto>()
            };
        }
    }

    /// <summary>
    /// Obtiene trabajos activos de forma individual
    /// </summary>
    public async Task<IEnumerable<TrabajoActivoDashboardDto>> ObtenerTrabajosActivosAsync(long? idUnidad = null, int? limite = 10)
    {
        try
        {
            var trabajos = await _adapter.ObtenerTrabajosActivosAsync(idUnidad, limite);

            _logger.LogInformation(
                "Trabajos activos obtenidos por servicio. Unidad: {IdUnidad}, Total: {Total}",
                idUnidad, trabajos.Count());

            return trabajos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo trabajos activos. Unidad: {IdUnidad}", idUnidad);
            return Enumerable.Empty<TrabajoActivoDashboardDto>();
        }
    }

    /// <summary>
    /// Obtiene métricas de forma individual
    /// </summary>
    public async Task<IEnumerable<DashboardMetricaDto>> ObtenerMetricasAsync(long? idUnidad = null)
    {
        try
        {
            var metricas = await _adapter.ObtenerMetricasAsync(idUnidad);

            _logger.LogInformation(
                "Métricas obtenidas por servicio. Unidad: {IdUnidad}, Total: {Total}",
                idUnidad, metricas.Count());

            return metricas;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo métricas. Unidad: {IdUnidad}", idUnidad);
            return Enumerable.Empty<DashboardMetricaDto>();
        }
    }

    /// <summary>
    /// Obtiene producción diaria para gráfico
    /// </summary>
    public async Task<IEnumerable<ProduccionDiariaDto>> ObtenerProduccionDiariaAsync(int diasAtras = 7)
    {
        try
        {
            if (diasAtras <= 0 || diasAtras > 365)
            {
                _logger.LogWarning("Validación: diasAtras fuera de rango. Valor: {DiasAtras}, Asignando: 7", diasAtras);
                diasAtras = 7;
            }

            var produccion = await _adapter.ObtenerProduccionDiariaAsync(diasAtras);

            _logger.LogInformation(
                "Producción diaria obtenida. DiasAtras: {DiasAtras}, Registros: {Total}",
                diasAtras, produccion.Count());

            return produccion.OrderBy(x => x.Fecha); // Ordenar cronológico ascendente
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo producción diaria. DiasAtras: {DiasAtras}", diasAtras);
            return Enumerable.Empty<ProduccionDiariaDto>();
        }
    }

    /// <summary>
    /// Obtiene trabajos en riesgo para alertas
    /// </summary>
    public async Task<IEnumerable<TrabajoActivoDashboardDto>> ObtenerTrabajosEnRiesgoAsync(long? idUnidad = null)
    {
        try
        {
            var trabajosEnRiesgo = await _adapter.ObtenerTrabajosEnRiesgoAsync(idUnidad);

            _logger.LogInformation(
                "Trabajos en riesgo obtenidos. Unidad: {IdUnidad}, Total: {Total}",
                idUnidad, trabajosEnRiesgo.Count());

            return trabajosEnRiesgo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo trabajos en riesgo. Unidad: {IdUnidad}", idUnidad);
            return Enumerable.Empty<TrabajoActivoDashboardDto>();
        }
    }

    /// <summary>
    /// Genera etiqueta de período (ej: "Semana 1: 02-08 Ene 2026")
    /// </summary>
    public string GenerarEtiquetaPeriodo()
    {
        try
        {
            var hoy = DateTime.Today;
            var semana = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                hoy,
                CalendarWeekRule.FirstDay,
                DayOfWeek.Monday);

            var inicioSemana = hoy.AddDays(-(int)hoy.DayOfWeek + (int)DayOfWeek.Monday);
            var finSemana = inicioSemana.AddDays(6);

            return $"Semana {semana}: {inicioSemana:dd-MM} - {finSemana:dd MMM yyyy}";
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error generando etiqueta de período, usando fecha actual");
            return $"Reporte al {DateTime.Today:dd/MM/yyyy}";
        }
    }
}
