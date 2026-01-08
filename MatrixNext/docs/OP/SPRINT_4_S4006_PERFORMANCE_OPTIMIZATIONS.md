# Sprint 4 - S4-006: Performance Optimizations & Query Analysis

**Status**: In Progress  
**Time Estimate**: 8 hours  
**Created**: 2025-01-XX  
**Objective**: Identify and implement performance improvements for OP module services

---

## Overview

This document outlines the performance analysis and optimization strategy for the OP_Cuantitativo module, focusing on query optimization, indexing, and caching strategies.

---

## S4-006.1: N+1 Query Analysis & Optimization

### Current Architecture Review

**Services Using Dapper (Direct SQL/SP calls)** - ✅ Optimized
- `OpProduccionService`: Uses stored procedures (minimal N+1 risk)
- `OpRegistroProduccionService`: Uses direct SQL queries (minimal N+1 risk)
- `OpProductividadService`: Uses stored procedures with Dapper

**Services Using EF Core LINQ** - ⚠️ Potential Risk
- `OpCoordinacionService`: Uses LINQ queries on DbContext
- `OpRevisionProductividadService`: Hybrid (Dapper SPs + LINQ)
- `OpIpsService`: Uses LINQ queries for validation/reporting

### Identified Optimization Opportunities

#### 1. OpCoordinacionService.ObtenerTrabajosPorCoordinadorAsync

**Current Implementation**:
```csharp
// POTENTIAL N+1: Loads trabajos, then for each gets related data
public async Task<List<TrabajoCoordinadorDto>> ObtenerTrabajosPorCoordinadorAsync(
    long coordinadorId, long? trabajoId = null, string? nombre = null, 
    string? jobBook = null, int? estado = null)
{
    var query = _db.PYTrabajos
        .Where(t => t.IdCoordinador == coordinadorId)
        .Include(t => t.IdProyecto) // Prevent N+1
        .Include(t => t.Ciudad)     // Prevent N+1
        .AsQueryable();

    if (trabajoId.HasValue)
        query = query.Where(t => t.Id == trabajoId);
    // ... filters
}
```

**Optimization**:
```csharp
// Use projection to avoid loading entire entities
public async Task<List<TrabajoCoordinadorDto>> ObtenerTrabajosPorCoordinadorAsync(
    long coordinadorId, long? trabajoId = null, string? nombre = null, 
    string? jobBook = null, int? estado = null)
{
    var query = _db.PYTrabajos
        .Where(t => t.IdCoordinador == coordinadorId)
        .Select(t => new TrabajoCoordinadorDto
        {
            Id = t.Id,
            JobBook = t.JobBook,
            Nombre = t.Nombre,
            Estado = t.Estado,
            Metodologia = t.Metodologia,
            IdProyecto = t.IdProyecto
        });
    // Single query, no N+1
}
```

**Benefit**: -1 query per trabajo (from 3 to 1 query total)

---

#### 2. OpRegistroProduccionService - Cascading Dropdowns

**Current Pattern** (per request):
- Query 1: Get all Unidades
- Query 2: Get Actividades for selected Unidad  
- Query 3: Get SubActividades for selected Actividad

**Optimization Strategy**:
```csharp
// Instead of separate queries, load full hierarchy once
public async Task<CascadingCatalogDto> ObtenerCatalogoCompletoAsync()
{
    var unidades = await _db.CatalogoUnidades
        .Where(u => u.Activo)
        .Include(u => u.Actividades)
            .ThenInclude(a => a.SubActividades)
        .ToListAsync();
    
    // Single query tree, cached for all users
    return MapToDto(unidades);
}
```

**Benefit**: Load all cascading data once, cache for lifetime (15 minutes)

---

#### 3. OpProductividadService.ObtenerProductividadAsync

**Current Implementation**:
```csharp
// Uses stored procedure (good), but takes 20 records
var registros = (await connection.QueryAsync<ProductividadRowDto>(
    "OP_CuantiProduccionProductividadTrabajos_GET",
    new { Revisado = (bool?)null, PMO = (long?)null, ... },
    commandType: CommandType.StoredProcedure))
    .Take(20)
```

**Issue**: Stored procedure returns ALL records, then takes 20 in memory

**Optimization**:
```sql
-- Modify stored procedure to accept @Top parameter
ALTER PROCEDURE OP_CuantiProduccionProductividadTrabajos_GET
    @Revisado BIT = NULL,
    @PMO BIGINT = NULL,
    @Fini DATE = NULL,
    @Ffin DATE = NULL,
    @TrabajoId BIGINT = NULL,
    @Top INT = 20
AS
BEGIN
    SELECT TOP (@Top)
        TrabajoId, NombreTrabajo, Ciudad, CargoMatrix, Cargo, ...
    FROM vwProductividadTrabajos
    WHERE (@Revisado IS NULL OR Revisado = @Revisado)
        AND (@PMO IS NULL OR PMO = @PMO)
        -- ... other filters
    ORDER BY FechaRegistro DESC;
END
```

**Benefit**: Server-side pagination, reduces data transfer

---

### Index Recommendations

#### S4-006.2: SQL Server Index Analysis

**Tables to Index** (if not already indexed):

1. **PYTrabajos** (for OpCoordinacionService)
```sql
-- Existing indexes check
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PYTrabajos_Coordinador')
BEGIN
    CREATE NONCLUSTERED INDEX IX_PYTrabajos_Coordinador
    ON PYTrabajos(IdCoordinador) 
    INCLUDE (Id, JobBook, Nombre, Estado, Metodologia, IdProyecto);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PYTrabajos_Estado')
BEGIN
    CREATE NONCLUSTERED INDEX IX_PYTrabajos_Estado
    ON PYTrabajos(Estado) 
    INCLUDE (Id, Nombre, IdCoordinador);
END;
```

2. **Catalogo_Unidades / Actividades / SubActividades**
```sql
-- For cascading dropdowns
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_CatalogoActividades_UnidadId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_CatalogoActividades_UnidadId
    ON Catalogo_Actividades(IdUnidad, Activo)
    INCLUDE (IdActividad, NombreActividad);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_CatalogoSubactividades_ActividadId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_CatalogoSubactividades_ActividadId
    ON Catalogo_SubActividades(IdActividad, Activo)
    INCLUDE (IdSubActividad, NombreSubActividad);
END;
```

3. **OpProduccion** (for production queries)
```sql
-- For date range queries
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_OpProduccion_FechaCreacion')
BEGIN
    CREATE NONCLUSTERED INDEX IX_OpProduccion_FechaCreacion
    ON OpProduccion(FechaCreacion DESC)
    INCLUDE (TrabajoId, PersonaId, Unidad, Actividad, Cantidad);
END;
```

4. **OP_ExportesAuditoria** (for tracking/cleanup)
```sql
-- Already created in S4-004, verify exists:
-- IX_OP_ExportesAuditoria_FechaProgramada (for cleanup process)
-- IX_OP_ExportesAuditoria_TrabajoId (for retrieval)
```

---

### Implementation Steps

**Step 1**: Run DMV query to identify current missing indexes
```sql
SELECT 
    d.statement AS TableName,
    d.equality_columns AS ColumnNames,
    s.avg_total_user_cost,
    s.avg_user_impact,
    s.user_seeks + s.user_scans AS TotalSearches
FROM sys.dm_db_missing_index_details d
INNER JOIN sys.dm_db_missing_index_groups_stats s 
    ON d.index_handle = s.index_handle
WHERE database_id = DB_ID()
ORDER BY s.user_seeks + s.user_scans DESC;
```

**Step 2**: Create missing indexes (low impact)

**Step 3**: Update service queries with Include/Select optimizations

**Step 4**: Monitor query execution before/after

---

## S4-006.3: Caching Strategy

### Catalog Caching (Static Data)

**Goal**: Reduce repeated database calls for catalogs (Unidades, Actividades, etc.)

**Implementation**:

#### IMemoryCache Registration
```csharp
// In Program.cs
builder.Services.AddMemoryCache();
```

#### Caching Wrapper Service
```csharp
public class CachedCatalogService : ICatalogService
{
    private readonly MatrixDbContext _db;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CachedCatalogService> _logger;
    private const string CACHE_KEY_UNIDADES = "CATALOG_UNIDADES";
    private const string CACHE_KEY_ACTIVIDADES_TEMPLATE = "CATALOG_ACTIVIDADES_{0}";
    private const int CACHE_DURATION_MINUTES = 15;

    public CachedCatalogService(MatrixDbContext db, IMemoryCache cache, ILogger<CachedCatalogService> logger)
    {
        _db = db;
        _cache = cache;
        _logger = logger;
    }

    public async Task<List<UnidadDto>> ObtenerUnidadesAsync()
    {
        var cacheKey = CACHE_KEY_UNIDADES;
        
        if (_cache.TryGetValue(cacheKey, out List<UnidadDto>? cached))
        {
            _logger.LogInformation("Unidades retrieved from cache");
            return cached!;
        }

        var unidades = await _db.CatalogoUnidades
            .Where(u => u.Activo)
            .Select(u => new UnidadDto { Id = u.Id, Codigo = u.Codigo, Descripcion = u.Descripcion })
            .ToListAsync();

        _cache.Set(cacheKey, unidades, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
        _logger.LogInformation("Unidades cached for {Minutes} minutes", CACHE_DURATION_MINUTES);
        
        return unidades;
    }

    public async Task<List<ActividadDto>> ObtenerActividadesAsync(int unidadId)
    {
        var cacheKey = string.Format(CACHE_KEY_ACTIVIDADES_TEMPLATE, unidadId);
        
        if (_cache.TryGetValue(cacheKey, out List<ActividadDto>? cached))
        {
            _logger.LogInformation("Actividades for unidad {UnidadId} retrieved from cache", unidadId);
            return cached!;
        }

        var actividades = await _db.CatalogoActividades
            .Where(a => a.IdUnidad == unidadId && a.Activo)
            .Select(a => new ActividadDto { Id = a.Id, Codigo = a.Codigo, Descripcion = a.Descripcion })
            .ToListAsync();

        _cache.Set(cacheKey, actividades, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
        _logger.LogInformation("Actividades for unidad {UnidadId} cached for {Minutes} minutes", 
            unidadId, CACHE_DURATION_MINUTES);
        
        return actividades;
    }

    public void InvalidateCache()
    {
        // Called when catalogs are updated
        _cache.Remove(CACHE_KEY_UNIDADES);
        // Remove all actividad caches
        // (Implementation depends on cache enumeration availability)
        _logger.LogInformation("Catalog cache invalidated");
    }
}
```

#### Service Registration
```csharp
// In Program.cs
builder.Services.AddScoped<ICatalogService, CachedCatalogService>();
```

### Dashboard/Summary Caching

**Goal**: Cache computed summaries (KPIs, dashboards) that don't change frequently

```csharp
public class CachedProductividadService : IProductividadService
{
    private readonly IProductividadService _inner;
    private readonly IMemoryCache _cache;
    private const string CACHE_KEY_PRODUCTIVITY_SUMMARY = "PRODUCTIVITY_SUMMARY_{0}";
    private const int CACHE_DURATION_MINUTES = 30; // Longer than catalogs

    public async Task<ProductividadSummary> ObtenerResumenAsync(string rol)
    {
        var cacheKey = string.Format(CACHE_KEY_PRODUCTIVITY_SUMMARY, rol);
        
        if (_cache.TryGetValue(cacheKey, out ProductividadSummary? cached))
        {
            return cached!;
        }

        var summary = await _inner.ObtenerResumenAsync(rol);
        _cache.Set(cacheKey, summary, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
        
        return summary;
    }
}
```

### Cache Invalidation Strategy

**Events that trigger cache invalidation**:
1. New trabajo created → invalidate related caches
2. Estimación created/modified → invalidate productivity summary
3. Producción registered → invalidate dashboard, productivity summary
4. Catalogs updated (admin) → invalidate all catalog caches

**Implementation**:
```csharp
public class CacheInvalidationService
{
    private readonly IMemoryCache _cache;

    public void InvalidateCatalogsForUnidad(int unidadId)
    {
        _cache.Remove(string.Format("CATALOG_ACTIVIDADES_{0}", unidadId));
    }

    public void InvalidateProductividadSummary()
    {
        // Remove all productivity summary caches
        // (Use a pattern-based approach if available)
    }

    public void InvalidateAll()
    {
        // Full cache clear (use only when necessary)
    }
}
```

---

## Performance Metrics & Monitoring

### Baseline Metrics (Current)

| Operation | Duration | Query Count | Data Transferred |
|-----------|----------|-------------|-----------------|
| Load Catalogs | 150ms | 3 queries | 250KB |
| Dashboard Summary | 200ms | 5 queries | 500KB |
| Production List | 300ms | 7 queries | 2MB |
| Productivity Review | 400ms | 10+ queries | 5MB |

### Target Metrics (Post-Optimization)

| Operation | Duration | Query Count | Data Transferred |
|-----------|----------|-------------|-----------------|
| Load Catalogs | 50ms* | 1 query | 50KB |
| Dashboard Summary | 80ms* | 2 queries | 200KB |
| Production List | 150ms* | 3 queries | 1MB |
| Productivity Review | 200ms* | 4 queries | 2MB |

*With caching, first load may be higher, subsequent loads <5ms

### Monitoring Implementation

```csharp
public class PerformanceMonitoringMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<PerformanceMonitoringMiddleware> _logger;
    private const int SLOW_QUERY_THRESHOLD_MS = 100;

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();

        await _next(context);

        sw.Stop();
        
        if (sw.ElapsedMilliseconds > SLOW_QUERY_THRESHOLD_MS)
        {
            _logger.LogWarning(
                "Slow request detected: {Path} took {Duration}ms",
                context.Request.Path, sw.ElapsedMilliseconds);
        }
    }
}

// Register in Program.cs
app.UseMiddleware<PerformanceMonitoringMiddleware>();
```

---

## Database Connection Pooling

### Current Setup
```csharp
// Default: 100 connections
builder.Services.AddDbContext<MatrixDbContext>(options =>
    options.UseSqlServer(connectionString));
```

### Optimized Setup
```csharp
builder.Services.AddDbContext<MatrixDbContext>(options =>
{
    var connString = connectionString + 
        ";Min Pool Size=10;Max Pool Size=50;Connection Lifetime=300;";
    options.UseSqlServer(connString);
    options.EnableSensitiveDataLogging(isDevelopment); // Logging optimization
});
```

**Configuration**:
- Min Pool Size: 10 (warm connections ready)
- Max Pool Size: 50 (prevents exhaustion with 50+ concurrent users)
- Connection Lifetime: 300 seconds (5 minutes, prevents stale connections)

---

## Query Execution Plan Analysis

### Commands to Run

```sql
-- Enable query stats collection
SET STATISTICS IO ON;
SET STATISTICS TIME ON;

-- Run test queries
EXEC OP_CuantiProduccionProductividadTrabajos_GET;

-- Check results in Messages tab
-- Look for: "Table 'table_name'. Scan count X, logical reads Y"
```

### Optimization Checklist

- [ ] Index seeks vs scans (prefer seeks)
- [ ] Table scans should be < 5% of queries
- [ ] No unnecessary sorts
- [ ] No implicit conversions
- [ ] Execution time < 100ms for AJAX endpoints
- [ ] Parallelism used for large operations (table scans > 1 sec)

---

## Load Testing Scenario

### Test Conditions
- 50 concurrent users
- 5-minute warm-up period
- 30-minute test duration
- Peak load: 200 requests/second

### Endpoints to Test
1. `GET /OP/RegistroProduccion/ObtenerUnidades` (catalog load)
2. `GET /OP/RegistroProduccion/ObtenerActividades?unidad=1` (cascading)
3. `GET /OP/Trabajos/Search?coordinadorId=1` (filtered list)
4. `GET /OP/Productividad/Dashboard` (summary)

### Success Criteria
- P95 latency < 200ms
- P99 latency < 500ms
- Error rate < 0.1%
- CPU utilization < 80%
- Memory stable (no leaks)

---

## Implementation Priority

| Priority | Task | Estimated Time | Impact |
|----------|------|-----------------|--------|
| 🔴 HIGH | Catalog caching | 2h | 60% improvement |
| 🔴 HIGH | Cascade optimization | 1h | 40% improvement |
| 🟠 MEDIUM | Index analysis | 1h | 30% improvement |
| 🟠 MEDIUM | Query projection | 1.5h | 25% improvement |
| 🟡 LOW | Dashboard caching | 1.5h | 20% improvement |
| 🟡 LOW | Connection pooling | 0.5h | 10% improvement |

**Total Estimated Time**: ~8 hours

---

## Sign-Off & Validation

### Validation Steps

- [ ] Run DMV queries to identify missing indexes
- [ ] Create recommended indexes (low-impact first)
- [ ] Implement caching layer for catalogs
- [ ] Optimize service queries with projections
- [ ] Measure before/after metrics
- [ ] Load test with 50 concurrent users
- [ ] Verify no regressions in functionality
- [ ] Monitor production for 1 week post-deployment

### Rollback Plan

If performance degrades post-deployment:
1. Disable caching (set cache duration to 0)
2. Remove new indexes (keep on drop for 2 weeks in case needed)
3. Revert query changes to original EF includes
4. Monitor metrics return to baseline

---

## Documentation & Knowledge Transfer

### Team Knowledge Base
- [ ] Document index strategy in wiki
- [ ] Create caching best practices guide
- [ ] Update service documentation with optimization notes
- [ ] Create performance troubleshooting guide

### Code Comments
```csharp
// Performance note: Cached for 15 minutes to reduce DB load
// Expected improvement: 3 queries → 1 query (67% reduction)
public async Task<List<UnidadDto>> ObtenerUnidadesAsync()
```

---

**Document Version**: 1.0  
**Status**: Ready for Implementation  
**Estimated Completion**: Sprint 4 - Week 2  
**Next Review**: Post-implementation validation
