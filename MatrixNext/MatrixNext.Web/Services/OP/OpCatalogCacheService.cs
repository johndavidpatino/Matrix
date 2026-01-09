using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.OP.Dtos;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace MatrixNext.Web.Services.OP
{
    /// <summary>
    /// Servicio de cachés para catálogos de OP. 
    /// Implementa IMemoryCache para reducir consultas a BD de datos estáticos (Unidades, Actividades, SubActividades).
    /// S4-006.3 Performance Optimization
    /// </summary>
    public interface IOpCatalogCacheService
    {
        Task<List<CatalogoItemDto>> ObtenerUnidadesAsync(bool forceRefresh = false);
        Task<List<CatalogoItemDto>> ObtenerActividadesAsync(int unidadId, bool forceRefresh = false);
        Task<List<CatalogoItemDto>> ObtenerSubactividadesAsync(int actividadId, bool forceRefresh = false);
        void InvalidateAllCaches();
        void InvalidateActividadesCache(int unidadId);
        void InvalidateSubactividadesCache(int actividadId);
    }

    public class OpCatalogCacheService : IOpCatalogCacheService
    {
        private readonly MatrixDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpCatalogCacheService> _logger;

        private const string CACHE_KEY_UNIDADES = "OP_CATALOG_UNIDADES";
        private const string CACHE_KEY_ACTIVIDADES_TEMPLATE = "OP_CATALOG_ACTIVIDADES_{0}";
        private const string CACHE_KEY_SUBACTIVIDADES_TEMPLATE = "OP_CATALOG_SUBACTIVIDADES_{0}";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);

        public OpCatalogCacheService(
            MatrixDbContext context,
            IMemoryCache cache,
            IConfiguration configuration,
            ILogger<OpCatalogCacheService> logger)
        {
            _context = context;
            _cache = cache;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<List<CatalogoItemDto>> ObtenerUnidadesAsync(bool forceRefresh = false)
        {
            const string cacheKey = CACHE_KEY_UNIDADES;

            if (!forceRefresh && _cache.TryGetValue(cacheKey, out List<CatalogoItemDto>? cached) && cached is not null)
            {
                _logger.LogDebug("Unidades retrieved from cache");
                return cached;
            }

            try
            {
                var connectionString = _configuration.GetConnectionString("MatrixDb") ?? throw new InvalidOperationException("Connection string not configured");
                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var unidades = await connection.QueryAsync<UnidadProduccionRow>(
                        "OP_UnidadesProduccionGet",
                        new { identificacion = (long?)null },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 30);

                    var result = unidades
                        .Where(u => u.Id.HasValue && !string.IsNullOrWhiteSpace(u.Unidad))
                        .Select(u => new CatalogoItemDto
                        {
                            Id = u.Id.GetValueOrDefault(),
                            Nombre = u.Unidad ?? string.Empty,
                            Activo = true
                        })
                        .ToList();
                    _cache.Set(cacheKey, result, CacheDuration);
                    
                    _logger.LogInformation(
                        "Unidades cached: {Count} items for {CacheDurationMinutes} minutes",
                        result.Count, CacheDuration.TotalMinutes);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo unidades desde BD");
                return new List<CatalogoItemDto>();
            }
        }

        public async Task<List<CatalogoItemDto>> ObtenerActividadesAsync(int unidadId, bool forceRefresh = false)
        {
            var cacheKey = string.Format(CACHE_KEY_ACTIVIDADES_TEMPLATE, unidadId);

            if (!forceRefresh && _cache.TryGetValue(cacheKey, out List<CatalogoItemDto>? cached) && cached is not null)
            {
                _logger.LogDebug("Actividades for unidad {UnidadId} retrieved from cache", unidadId);
                return cached;
            }

            try
            {
                var connectionString = _configuration.GetConnectionString("MatrixDb") ?? throw new InvalidOperationException("Connection string not configured");
                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var actividades = await connection.QueryAsync<ActividadProduccionRow>(
                        "OP_ActividadesProduccionGet",
                        new
                        {
                            unidad = unidadId,
                            Actividad = (int?)null,
                            SubActividad = (int?)null,
                            activa = true
                        },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 30);

                    var result = actividades
                        .Where(a => a.ActividadCod.HasValue && !string.IsNullOrWhiteSpace(a.Actividad))
                        .GroupBy(a => new { a.ActividadCod, a.Actividad })
                        .Select(g => new CatalogoItemDto
                        {
                            Id = g.Key.ActividadCod!.Value,
                            Nombre = g.Key.Actividad ?? string.Empty,
                            Activo = true
                        })
                        .OrderBy(a => a.Nombre)
                        .ToList();
                    _cache.Set(cacheKey, result, CacheDuration);

                    _logger.LogInformation(
                        "Actividades for unidad {UnidadId} cached: {Count} items",
                        unidadId, result.Count);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo actividades para unidad {UnidadId}", unidadId);
                return new List<CatalogoItemDto>();
            }
        }

        public async Task<List<CatalogoItemDto>> ObtenerSubactividadesAsync(int actividadId, bool forceRefresh = false)
        {
            var cacheKey = string.Format(CACHE_KEY_SUBACTIVIDADES_TEMPLATE, actividadId);

            if (!forceRefresh && _cache.TryGetValue(cacheKey, out List<CatalogoItemDto>? cached) && cached is not null)
            {
                _logger.LogDebug("Subactividades for actividad {ActividadId} retrieved from cache", actividadId);
                return cached;
            }

            try
            {
                var connectionString = _configuration.GetConnectionString("MatrixDb") ?? throw new InvalidOperationException("Connection string not configured");
                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var subactividades = await connection.QueryAsync<ActividadProduccionRow>(
                        "OP_ActividadesProduccionGet",
                        new
                        {
                            unidad = (int?)null,
                            Actividad = actividadId,
                            SubActividad = (int?)null,
                            activa = true
                        },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 30);

                    var result = subactividades
                        .Where(s => s.SubActividadCod.HasValue && !string.IsNullOrWhiteSpace(s.SubActividad))
                        .GroupBy(s => new { s.SubActividadCod, s.SubActividad })
                        .Select(g => new CatalogoItemDto
                        {
                            Id = g.Key.SubActividadCod!.Value,
                            Nombre = g.Key.SubActividad ?? string.Empty,
                            Activo = true
                        })
                        .OrderBy(s => s.Nombre)
                        .ToList();
                    _cache.Set(cacheKey, result, CacheDuration);

                    _logger.LogInformation(
                        "Subactividades for actividad {ActividadId} cached: {Count} items",
                        actividadId, result.Count);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo subactividades para actividad {ActividadId}", actividadId);
                return new List<CatalogoItemDto>();
            }
        }

        public void InvalidateAllCaches()
        {
            try
            {
                _cache.Remove(CACHE_KEY_UNIDADES);
                _logger.LogInformation("All catalog caches invalidated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error invalidating all catalog caches");
            }
        }

        public void InvalidateActividadesCache(int unidadId)
        {
            try
            {
                var cacheKey = string.Format(CACHE_KEY_ACTIVIDADES_TEMPLATE, unidadId);
                _cache.Remove(cacheKey);
                _logger.LogInformation("Actividades cache for unidad {UnidadId} invalidated", unidadId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error invalidating actividades cache for unidad {UnidadId}", unidadId);
            }
        }

        public void InvalidateSubactividadesCache(int actividadId)
        {
            try
            {
                var cacheKey = string.Format(CACHE_KEY_SUBACTIVIDADES_TEMPLATE, actividadId);
                _cache.Remove(cacheKey);
                _logger.LogInformation("Subactividades cache for actividad {ActividadId} invalidated", actividadId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error invalidating subactividades cache for actividad {ActividadId}", actividadId);
            }
        }

        private sealed class UnidadProduccionRow
        {
            public int? Id { get; init; }
            public string? Unidad { get; init; }
        }

        private sealed class ActividadProduccionRow
        {
            public int? ActividadCod { get; init; }
            public string? Actividad { get; init; }
            public int? SubActividadCod { get; init; }
            public string? SubActividad { get; init; }
        }
    }
}
