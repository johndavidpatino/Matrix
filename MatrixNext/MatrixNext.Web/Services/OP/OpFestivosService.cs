using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.OP
{
    /// <summary>
    /// Implementación del servicio de gestión de festivos.
    /// Consulta la tabla _Festivos y proporciona caché en memoria.
    /// </summary>
    public class OpFestivosService : IOpFestivosService
    {
        private readonly string? _connectionString;
        private readonly ILogger<OpFestivosService> _logger;
        
        // Caché simple por año: Dictionary<año, HashSet<fechas>>
        private readonly Dictionary<int, HashSet<DateOnly>> _cacheFestivos = new();
        private readonly SemaphoreSlim _cacheLock = new(1, 1);

        public OpFestivosService(
            MatrixDbContext context,
            ILogger<OpFestivosService> logger)
        {
            _connectionString = context.Database.GetConnectionString();
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<List<DateOnly>> ObtenerFestivosEnRangoAsync(
            DateOnly fechaInicio,
            DateOnly fechaFin,
            CancellationToken cancellationToken = default)
        {
            if (fechaInicio > fechaFin)
            {
                _logger.LogWarning("Rango de fechas inválido: {FechaInicio} > {FechaFin}", fechaInicio, fechaFin);
                return new List<DateOnly>();
            }

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                _logger.LogWarning("No se encontró cadena de conexión MatrixDb para cargar festivos.");
                return new List<DateOnly>();
            }

            var festivos = new List<DateOnly>();

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                
                const string sql = @"
                    SELECT festivo 
                    FROM _Festivos 
                    WHERE festivo >= @FechaInicio 
                      AND festivo <= @FechaFin
                    ORDER BY festivo";

                var result = await connection.QueryAsync<DateTime>(
                    sql,
                    new { FechaInicio = fechaInicio.ToDateTime(TimeOnly.MinValue), FechaFin = fechaFin.ToDateTime(TimeOnly.MinValue) },
                    commandTimeout: 30);

                festivos = result.Select(f => DateOnly.FromDateTime(f)).ToList();

                _logger.LogInformation(
                    "Cargados {Count} festivos entre {FechaInicio:yyyy-MM-dd} y {FechaFin:yyyy-MM-dd}",
                    festivos.Count, fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar festivos desde _Festivos para rango {FechaInicio} - {FechaFin}",
                    fechaInicio, fechaFin);
            }

            return festivos;
        }

        /// <inheritdoc />
        public async Task<bool> EsDiaFestivoAsync(
            DateOnly fecha,
            CancellationToken cancellationToken = default)
        {
            var año = fecha.Year;
            
            // Intentar obtener del caché
            var festivosAño = await ObtenerFestivosPorAñoAsync(año, cancellationToken);
            
            return festivosAño.Contains(fecha);
        }

        /// <inheritdoc />
        public async Task<HashSet<DateOnly>> ObtenerFestivosPorAñoAsync(
            int año,
            CancellationToken cancellationToken = default)
        {
            // Verificar caché primero (sin lock para lectura rápida)
            if (_cacheFestivos.TryGetValue(año, out var cached))
            {
                return cached;
            }

            // Si no está en caché, cargar con lock
            await _cacheLock.WaitAsync(cancellationToken);
            try
            {
                // Double-check después de obtener el lock
                if (_cacheFestivos.TryGetValue(año, out cached))
                {
                    return cached;
                }

                // Cargar festivos del año desde BD
                var festivosSet = await CargarFestivosDeAñoAsync(año, cancellationToken);
                
                // Guardar en caché
                _cacheFestivos[año] = festivosSet;
                
                // Limpiar años antiguos del caché (mantener solo últimos 3 años)
                LimpiarCacheAntiguos(año);

                return festivosSet;
            }
            finally
            {
                _cacheLock.Release();
            }
        }

        /// <inheritdoc />
        public void LimpiarCache()
        {
            _cacheLock.Wait();
            try
            {
                _cacheFestivos.Clear();
                _logger.LogInformation("Caché de festivos limpiado manualmente");
            }
            finally
            {
                _cacheLock.Release();
            }
        }

        /// <summary>
        /// Carga festivos de un año específico desde la base de datos.
        /// </summary>
        private async Task<HashSet<DateOnly>> CargarFestivosDeAñoAsync(
            int año,
            CancellationToken cancellationToken)
        {
            var festivosSet = new HashSet<DateOnly>();

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                _logger.LogWarning("No se encontró cadena de conexión MatrixDb para cargar festivos del año {Año}", año);
                return festivosSet;
            }

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                
                const string sql = @"
                    SELECT festivo 
                    FROM _Festivos 
                    WHERE YEAR(festivo) = @Año
                    ORDER BY festivo";

                var result = await connection.QueryAsync<DateTime>(
                    sql,
                    new { Año = año },
                    commandTimeout: 30);

                foreach (var fecha in result)
                {
                    festivosSet.Add(DateOnly.FromDateTime(fecha));
                }

                _logger.LogInformation(
                    "Cargados {Count} festivos para el año {Año} en caché",
                    festivosSet.Count, año);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar festivos desde _Festivos para año {Año}", año);
            }

            return festivosSet;
        }

        /// <summary>
        /// Limpia del caché los años antiguos, manteniendo solo los últimos 3 años.
        /// </summary>
        private void LimpiarCacheAntiguos(int añoActual)
        {
            var añosAEliminar = _cacheFestivos.Keys
                .Where(a => a < añoActual - 2) // Mantener año actual y 2 anteriores
                .ToList();

            foreach (var año in añosAEliminar)
            {
                _cacheFestivos.Remove(año);
            }

            if (añosAEliminar.Any())
            {
                _logger.LogDebug("Eliminados {Count} años antiguos del caché de festivos", añosAEliminar.Count);
            }
        }
    }
}
