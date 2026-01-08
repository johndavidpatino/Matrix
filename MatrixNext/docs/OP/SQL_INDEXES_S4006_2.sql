-- S4-006.2: SQL Server Index Optimization for OP Module
-- Purpose: Create missing indexes on frequently queried columns to improve query performance
-- Estimated Impact: 20-40% improvement in query execution time
-- Created: 2025-01-XX
-- Author: Sprint 4 Performance Optimization

-- ============================================================================
-- STEP 1: Analyze current missing indexes (DMV query)
-- ============================================================================
-- Run this first to identify which indexes are most critical

SELECT 
    d.statement AS TableName,
    d.equality_columns AS KeyColumns,
    d.included_columns AS IncludedColumns,
    CONVERT(DECIMAL(18,2), (s.avg_total_user_cost * s.avg_user_impact * (s.user_seeks + s.user_scans + s.user_lookups)) / 1000000.0) AS ImprovementMeasure,
    s.user_seeks AS SeekCount,
    s.user_scans AS ScanCount,
    s.user_lookups AS LookupCount,
    s.avg_total_user_cost AS AvgCost,
    s.avg_user_impact AS AvgImpact
FROM sys.dm_db_missing_index_details d
INNER JOIN sys.dm_db_missing_index_groups_stats s 
    ON d.index_handle = s.index_handle
WHERE database_id = DB_ID('MatrixDb')
ORDER BY ImprovementMeasure DESC;

-- ============================================================================
-- STEP 2: Create recommended indexes for OP operations
-- ============================================================================

-- 2.1: PYTrabajos - Optimize coordinator and status filtering
-- Impact: Speeds up OpCoordinacionService.ObtenerTrabajosPorCoordinadorAsync queries
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PYTrabajos_CoordinadorId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_PYTrabajos_CoordinadorId
    ON PY_Trabajo(IdCoordinador, Estado)
    INCLUDE (JobBook, Nombre, IdMetodologia, IdProyecto)
    WITH (FILLFACTOR = 90);
    PRINT 'Created index IX_PYTrabajos_CoordinadorId';
END
ELSE
    PRINT 'Index IX_PYTrabajos_CoordinadorId already exists';

-- 2.2: Catalogo_Actividades - Optimize cascading dropdowns
-- Impact: Speeds up OpRegistroProduccionService.ObtenerActividadesAsync
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Catalogo_Actividades_UnidadId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Catalogo_Actividades_UnidadId
    ON Catalogo_Actividades(IdUnidad, Activo)
    INCLUDE (IdActividad, NombreActividad)
    WITH (FILLFACTOR = 90);
    PRINT 'Created index IX_Catalogo_Actividades_UnidadId';
END
ELSE
    PRINT 'Index IX_Catalogo_Actividades_UnidadId already exists';

-- 2.3: Catalogo_Subactividades - Optimize cascading dropdowns
-- Impact: Speeds up OpRegistroProduccionService.ObtenerSubactividadesAsync
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Catalogo_Subactividades_ActividadId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Catalogo_Subactividades_ActividadId
    ON Catalogo_Subactividades(IdActividad, Activo)
    INCLUDE (IdSubactividad, NombreSubactividad)
    WITH (FILLFACTOR = 90);
    PRINT 'Created index IX_Catalogo_Subactividades_ActividadId';
END
ELSE
    PRINT 'Index IX_Catalogo_Subactividades_ActividadId already exists';

-- 2.4: OpProduccion - Optimize date range queries and status filtering
-- Impact: Speeds up production queries with date filters
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_OpProduccion_FechaCreacion')
BEGIN
    CREATE NONCLUSTERED INDEX IX_OpProduccion_FechaCreacion
    ON OpProduccion(FechaCreacion DESC, IdTrabajo)
    INCLUDE (IdPersona, Unidad, Actividad, Cantidad, Estado)
    WITH (FILLFACTOR = 90);
    PRINT 'Created index IX_OpProduccion_FechaCreacion';
END
ELSE
    PRINT 'Index IX_OpProduccion_FechaCreacion already exists';

-- 2.5: OpAsignaciones - Optimize work and coordinator filtering
-- Impact: Speeds up assignment lookups
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_OpAsignaciones_TrabajoCoordinador')
BEGIN
    CREATE NONCLUSTERED INDEX IX_OpAsignaciones_TrabajoCoordinador
    ON OpAsignaciones(IdTrabajo, IdCoordinador)
    INCLUDE (Estado, FechaAsignacion)
    WITH (FILLFACTOR = 90);
    PRINT 'Created index IX_OpAsignaciones_TrabajoCoordinador';
END
ELSE
    PRINT 'Index IX_OpAsignaciones_TrabajoCoordinador already exists';

-- 2.6: OP_ExportesAuditoria - Optimize cleanup process
-- Impact: Speeds up scheduled cleanup jobs that run hourly
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_OP_ExportesAuditoria_FechaProgramada')
BEGIN
    CREATE NONCLUSTERED INDEX IX_OP_ExportesAuditoria_FechaProgramada
    ON OP_ExportesAuditoria(FechaProgramadaLimpieza, Estado)
    INCLUDE (IdTrabajo, UsuarioExportacion, FechaExportacion)
    WITH (FILLFACTOR = 90);
    PRINT 'Created index IX_OP_ExportesAuditoria_FechaProgramada';
END
ELSE
    PRINT 'Index IX_OP_ExportesAuditoria_FechaProgramada already exists';

-- ============================================================================
-- STEP 3: Verify indexes were created
-- ============================================================================

SELECT 
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType,
    CONVERT(VARCHAR(MAX), (SELECT ','+ col_name(ic.object_id, ic.column_id) 
        FROM sys.index_columns ic 
        WHERE ic.object_id = i.object_id 
        AND ic.index_id = i.index_id 
        AND ic.is_included_column = 0 
        FOR XML PATH('')), 2) AS KeyColumns,
    CONVERT(VARCHAR(MAX), (SELECT ','+ col_name(ic.object_id, ic.column_id) 
        FROM sys.index_columns ic 
        WHERE ic.object_id = i.object_id 
        AND ic.index_id = i.index_id 
        AND ic.is_included_column = 1 
        FOR XML PATH('')), 2) AS IncludedColumns,
    s.avg_fragmentation_in_percent AS Fragmentation
FROM sys.indexes i
LEFT JOIN sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'LIMITED') s 
    ON i.object_id = s.object_id 
    AND i.index_id = s.index_id
WHERE OBJECT_NAME(i.object_id) IN (
    'PY_Trabajo',
    'Catalogo_Actividades',
    'Catalogo_Subactividades',
    'OpProduccion',
    'OpAsignaciones',
    'OP_ExportesAuditoria'
)
AND i.name IN (
    'IX_PYTrabajos_CoordinadorId',
    'IX_Catalogo_Actividades_UnidadId',
    'IX_Catalogo_Subactividades_ActividadId',
    'IX_OpProduccion_FechaCreacion',
    'IX_OpAsignaciones_TrabajoCoordinador',
    'IX_OP_ExportesAuditoria_FechaProgramada'
)
ORDER BY OBJECT_NAME(i.object_id), i.name;

-- ============================================================================
-- STEP 4: Update index statistics (run if needed)
-- ============================================================================
-- Note: Run this during maintenance windows, not during production hours

/*
-- Optional: Update statistics for all OP-related tables
UPDATE STATISTICS PY_Trabajo;
UPDATE STATISTICS Catalogo_Actividades;
UPDATE STATISTICS Catalogo_Subactividades;
UPDATE STATISTICS OpProduccion;
UPDATE STATISTICS OpAsignaciones;
UPDATE STATISTICS OP_ExportesAuditoria;

-- Optional: Rebuild fragmented indexes (>10% fragmentation)
ALTER INDEX IX_PYTrabajos_CoordinadorId ON PY_Trabajo REBUILD;
*/

-- ============================================================================
-- STEP 5: Performance baseline capture
-- ============================================================================
-- Before/After metrics for S4-006 optimization

PRINT '';
PRINT '==================================================';
PRINT 'Index Creation Complete for S4-006.2';
PRINT '==================================================';
PRINT 'Created 6 non-clustered indexes targeting:';
PRINT '  1. Coordinator work filtering (PYTrabajos)';
PRINT '  2. Cascading dropdown performance (Catalogs)';
PRINT '  3. Production date range queries (OpProduccion)';
PRINT '  4. Assignment lookups (OpAsignaciones)';
PRINT '  5. Export cleanup process (OP_ExportesAuditoria)';
PRINT '';
PRINT 'Expected improvements:';
PRINT '  - Catalog queries: 50-70% faster (seek instead of scan)';
PRINT '  - Coordinator queries: 40-60% faster';
PRINT '  - Cleanup queries: 70-80% faster';
PRINT '';
PRINT 'Next: Run load tests to validate improvements';
PRINT '==================================================';
