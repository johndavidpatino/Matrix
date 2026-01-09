-- =============================================================================
-- VALIDACIÓN DE STORED PROCEDURES Y TABLAS - OP_CUALITATIVO
-- =============================================================================
-- Ref: ANALISIS_OP_CUALITATIVO_FASE5_MAPEO_BD_RIESGOS.md
-- Base de Datos: CO_Matrix_Intranet
-- Fecha: 9 de enero, 2026
-- =============================================================================

USE CO_Matrix_Intranet;
GO

-- SECCIÓN 1: TABLAS ESPERADAS - OP_CUALITATIVO
-- =============================================================================
PRINT '=== 1. TABLAS OP_CUALITATIVO (Esperadas) ==='
PRINT ''

SELECT 
    TABLE_NAME,
    'EXISTS' AS Status,
    'Tabla encontrada' AS Nota
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo' 
AND TABLE_NAME IN (
    'OP_CampoCuali',
    'OP_Respuestas_Filtro_Maestro',
    'OP_Respuestas_Filtro_Detalle',
    'OP_LogRespuestas_Filtro',
    'OP_MuestraTrabajos',
    'OP_Programados_Entrevistados',
    'OP_IPS_Procesos',
    'OP_TrabajoConfiguracion',
    'OP_PreguntasFiltro',
    'OP_FichasTecnicas'
)
ORDER BY TABLE_NAME;

-- TABLAS NO ENCONTRADAS
SELECT 
    'OP_CampoCuali' AS TABLE_NAME
UNION ALL SELECT 'OP_Respuestas_Filtro_Maestro'
UNION ALL SELECT 'OP_Respuestas_Filtro_Detalle'
UNION ALL SELECT 'OP_LogRespuestas_Filtro'
UNION ALL SELECT 'OP_MuestraTrabajos'
UNION ALL SELECT 'OP_Programados_Entrevistados'
UNION ALL SELECT 'OP_IPS_Procesos'
UNION ALL SELECT 'OP_TrabajoConfiguracion'
UNION ALL SELECT 'OP_PreguntasFiltro'
UNION ALL SELECT 'OP_FichasTecnicas'
WHERE TABLE_NAME NOT IN (
    SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'dbo'
)
ORDER BY TABLE_NAME;

PRINT ''
PRINT '=== 2. STORED PROCEDURES (Esperados) ==='
PRINT ''

-- CONFIRMADAS
SELECT 
    ROUTINE_NAME,
    'EXISTS' AS Status,
    'SP encontrado' AS Nota
FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_SCHEMA = 'dbo' 
AND ROUTINE_NAME IN (
    'obtenerXIdCOEXTodosCampos',
    'ObtenerTrabajosCualitativosxCOE',
    'obtenerXCOE',
    'REP_OP_Respuestas_Filtro'
)
AND ROUTINE_TYPE = 'PROCEDURE'
ORDER BY ROUTINE_NAME;

PRINT ''
PRINT '=== 3. STORED PROCEDURES NO ENCONTRADOS ==='
PRINT ''

SELECT 
    'obtenerXIdCOEXTodosCampos' AS ROUTINE_NAME, 'CONFIRMADA' AS Tipo
UNION ALL SELECT 'ObtenerTrabajosCualitativosxCOE', 'CONFIRMADA'
UNION ALL SELECT 'obtenerXCOE', 'CONFIRMADA'
UNION ALL SELECT 'REP_OP_Respuestas_Filtro', 'CONFIRMADA'
UNION ALL SELECT 'ObtenerTipoPreguntaFiltro', 'ESPERADA'
UNION ALL SELECT 'ObtenerListaFiltros', 'ESPERADA'
UNION ALL SELECT 'ObtenerListaPreguntasFiltro', 'ESPERADA'
UNION ALL SELECT 'ObtenerHabeasData', 'ESPERADA'
UNION ALL SELECT 'ObtenerAyudasRequeridasCualiList', 'ESPERADA'
UNION ALL SELECT 'ObtenerReclutamientoRequeridoCualiList', 'ESPERADA'
WHERE ROUTINE_NAME NOT IN (
    SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES 
    WHERE ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'PROCEDURE'
)
ORDER BY ROUTINE_NAME;

PRINT ''
PRINT '=== 4. TABLAS COREPROJECT RELACIONADAS ==='
PRINT ''

SELECT 
    TABLE_NAME,
    'EXISTS' AS Status
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo' 
AND TABLE_NAME IN (
    'PY_Trabajo',
    'PY_TrabajoCuali',
    'CO_Coordinacion',
    'CO_CoordinacionCampo',
    'CO_EntrevistasCampo',
    'GD_DocumentoRecibido',
    'US_Usuario',
    'US_RolesUsuarios'
)
ORDER BY TABLE_NAME;

PRINT ''
PRINT '=== 5. RESUMEN POR SP EN OpCualitativoService ==='
PRINT ''

-- Listar todas las SPs que contiene "Trabajo" o "Cualitativo"
SELECT 
    ROUTINE_NAME,
    ROUTINE_TYPE,
    ROUTINE_DEFINITION AS Cuerpo
FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_SCHEMA = 'dbo' 
AND (ROUTINE_NAME LIKE '%Trabajo%' OR ROUTINE_NAME LIKE '%Cuali%')
ORDER BY ROUTINE_NAME;
