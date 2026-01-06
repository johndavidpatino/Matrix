-- EQ_MIGRATION_VERSIONING.sql
-- Sprint 2.1: Agregar columnas de versionado (vigente_desde, vigente_hasta) a maestras
-- Ejecutar después de EQ_SCHEMA.sql

SET NOCOUNT ON;

------------------------------------------------------------
-- AGREGAR COLUMNAS DE VERSIONADO A TABLAS MAESTRAS
------------------------------------------------------------

-- eq_param_penetracion
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.eq_param_penetracion') AND name = 'vigente_desde')
BEGIN
    ALTER TABLE dbo.eq_param_penetracion ADD 
        vigente_desde DATE DEFAULT CAST(GETDATE() AS DATE),
        vigente_hasta DATE NULL;
    PRINT 'Versionado agregado a eq_param_penetracion';
END;

-- eq_param_metodologia
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.eq_param_metodologia') AND name = 'vigente_desde')
BEGIN
    ALTER TABLE dbo.eq_param_metodologia ADD 
        vigente_desde DATE DEFAULT CAST(GETDATE() AS DATE),
        vigente_hasta DATE NULL;
    PRINT 'Versionado agregado a eq_param_metodologia';
END;

-- eq_param_precio
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.eq_param_precio') AND name = 'vigente_desde')
BEGIN
    ALTER TABLE dbo.eq_param_precio ADD 
        vigente_desde DATE DEFAULT CAST(GETDATE() AS DATE),
        vigente_hasta DATE NULL;
    PRINT 'Versionado agregado a eq_param_precio';
END;

-- eq_param_script_proc
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.eq_param_script_proc') AND name = 'vigente_desde')
BEGIN
    ALTER TABLE dbo.eq_param_script_proc ADD 
        vigente_desde DATE DEFAULT CAST(GETDATE() AS DATE),
        vigente_hasta DATE NULL;
    PRINT 'Versionado agregado a eq_param_script_proc';
END;

-- eq_valor_hora_ops
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.eq_valor_hora_ops') AND name = 'vigente_desde')
BEGIN
    ALTER TABLE dbo.eq_valor_hora_ops ADD 
        vigente_desde DATE DEFAULT CAST(GETDATE() AS DATE),
        vigente_hasta DATE NULL;
    PRINT 'Versionado agregado a eq_valor_hora_ops';
END;

-- eq_rate_estadistica
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.eq_rate_estadistica') AND name = 'vigente_desde')
BEGIN
    ALTER TABLE dbo.eq_rate_estadistica ADD 
        vigente_desde DATE DEFAULT CAST(GETDATE() AS DATE),
        vigente_hasta DATE NULL;
    PRINT 'Versionado agregado a eq_rate_estadistica';
END;

-- eq_param_cati
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.eq_param_cati') AND name = 'vigente_desde')
BEGIN
    ALTER TABLE dbo.eq_param_cati ADD 
        vigente_desde DATE DEFAULT CAST(GETDATE() AS DATE),
        vigente_hasta DATE NULL;
    PRINT 'Versionado agregado a eq_param_cati';
END;

-- eq_param_online
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.eq_param_online') AND name = 'vigente_desde')
BEGIN
    ALTER TABLE dbo.eq_param_online ADD 
        vigente_desde DATE DEFAULT CAST(GETDATE() AS DATE),
        vigente_hasta DATE NULL;
    PRINT 'Versionado agregado a eq_param_online';
END;

-- eq_param_factores
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.eq_param_factores') AND name = 'vigente_desde')
BEGIN
    ALTER TABLE dbo.eq_param_factores ADD 
        vigente_desde DATE DEFAULT CAST(GETDATE() AS DATE),
        vigente_hasta DATE NULL;
    PRINT 'Versionado agregado a eq_param_factores';
END;

-- eq_rate_horas
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.eq_rate_horas') AND name = 'vigente_desde')
BEGIN
    ALTER TABLE dbo.eq_rate_horas ADD 
        vigente_desde DATE DEFAULT CAST(GETDATE() AS DATE),
        vigente_hasta DATE NULL;
    PRINT 'Versionado agregado a eq_rate_horas';
END;

------------------------------------------------------------
-- CREAR TABLA PARA HISTORIAL DE CAMBIOS
------------------------------------------------------------

IF OBJECT_ID('dbo.eq_audit_maestras','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_audit_maestras (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        tabla_nombre VARCHAR(100) NOT NULL,
        registro_id INT NOT NULL,
        fecha_cambio DATETIME DEFAULT GETDATE(),
        tipo_cambio VARCHAR(20) NOT NULL, -- INSERT, UPDATE, DELETE
        datos_antes NVARCHAR(MAX) NULL,
        datos_despues NVARCHAR(MAX) NULL,
        usuario_cambio NVARCHAR(128) DEFAULT SYSTEM_USER
    );
    PRINT 'Tabla eq_audit_maestras creada';
END;

------------------------------------------------------------
-- CREAR ÍNDICES PARA LOOKUPS POR FECHA
------------------------------------------------------------

-- Índice para eq_param_precio lookup por fecha
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_eq_param_precio_vigencia' AND object_id = OBJECT_ID('dbo.eq_param_precio'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_eq_param_precio_vigencia 
    ON dbo.eq_param_precio (vigente_desde, vigente_hasta)
    INCLUDE (MetodologiaCodigo, PenetracionCodigo, DuracionMin, ValorTotal);
    PRINT 'Índice IX_eq_param_precio_vigencia creado';
END;

-- Índice para eq_param_cati
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_eq_param_cati_vigencia' AND object_id = OBJECT_ID('dbo.eq_param_cati'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_eq_param_cati_vigencia 
    ON dbo.eq_param_cati (vigente_desde, vigente_hasta)
    INCLUDE (codigo, nivel, minutos, valor);
    PRINT 'Índice IX_eq_param_cati_vigencia creado';
END;

-- Índice para eq_param_online
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_eq_param_online_vigencia' AND object_id = OBJECT_ID('dbo.eq_param_online'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_eq_param_online_vigencia 
    ON dbo.eq_param_online (vigente_desde, vigente_hasta)
    INCLUDE (codigo, nivel, minutos, valor);
    PRINT 'Índice IX_eq_param_online_vigencia creado';
END;

------------------------------------------------------------
-- CREAR VISTAS PARA OBTENER DATOS VIGENTES A UNA FECHA
------------------------------------------------------------

-- Vista: Precios vigentes a una fecha determinada
IF OBJECT_ID('dbo.vw_eq_param_precio_vigente','V') IS NOT NULL
    DROP VIEW dbo.vw_eq_param_precio_vigente;

CREATE VIEW dbo.vw_eq_param_precio_vigente AS
SELECT 
    Id,
    MetodologiaCodigo,
    PenetracionCodigo,
    DuracionMin,
    ValorPerfil,
    ValorCoordinacion,
    ValorTotal,
    vigente_desde,
    vigente_hasta
FROM dbo.eq_param_precio
WHERE vigente_desde <= CAST(GETDATE() AS DATE) 
  AND (vigente_hasta IS NULL OR vigente_hasta > CAST(GETDATE() AS DATE));

PRINT 'Vista vw_eq_param_precio_vigente creada';
GO

-- Vista: CATI vigente
IF OBJECT_ID('dbo.vw_eq_param_cati_vigente','V') IS NOT NULL
    DROP VIEW dbo.vw_eq_param_cati_vigente;

CREATE VIEW dbo.vw_eq_param_cati_vigente AS
SELECT 
    Id,
    codigo,
    nivel,
    minutos,
    valor,
    vigente_desde,
    vigente_hasta
FROM dbo.eq_param_cati
WHERE vigente_desde <= CAST(GETDATE() AS DATE) 
  AND (vigente_hasta IS NULL OR vigente_hasta > CAST(GETDATE() AS DATE));

PRINT 'Vista vw_eq_param_cati_vigente creada';
GO

-- Vista: Online vigente
IF OBJECT_ID('dbo.vw_eq_param_online_vigente','V') IS NOT NULL
    DROP VIEW dbo.vw_eq_param_online_vigente;

CREATE VIEW dbo.vw_eq_param_online_vigente AS
SELECT 
    Id,
    codigo,
    nivel,
    minutos,
    valor,
    vigente_desde,
    vigente_hasta
FROM dbo.eq_param_online
WHERE vigente_desde <= CAST(GETDATE() AS DATE) 
  AND (vigente_hasta IS NULL OR vigente_hasta > CAST(GETDATE() AS DATE));

PRINT 'Vista vw_eq_param_online_vigente creada';
GO

------------------------------------------------------------
-- CREAR PROCEDIMIENTOS PARA GESTIONAR VIGENCIAS
------------------------------------------------------------

-- SP: Desactivar un registro maestro (soft delete)
IF OBJECT_ID('dbo.sp_eq_desactivar_maestro','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_eq_desactivar_maestro;
GO

CREATE PROCEDURE dbo.sp_eq_desactivar_maestro
    @tabla_nombre VARCHAR(100),
    @id INT,
    @fecha_fin DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Si no se especifica fecha, usar hoy
    IF @fecha_fin IS NULL
        SET @fecha_fin = CAST(GETDATE() AS DATE);
    
    -- Verificar que la fecha fin sea posterior a hoy
    IF @fecha_fin <= CAST(GETDATE() AS DATE)
    BEGIN
        RAISERROR('La fecha de vigencia debe ser posterior a hoy', 16, 1);
        RETURN;
    END;
    
    -- Actualizar según tabla
    DECLARE @sqlUpdate NVARCHAR(MAX);
    SET @sqlUpdate = 'UPDATE dbo.' + @tabla_nombre + ' SET vigente_hasta = @fecha_fin WHERE Id = @id;';
    
    EXEC sp_executesql @sqlUpdate, N'@fecha_fin DATE, @id INT', @fecha_fin, @id;
    
    -- Registrar en auditoría
    INSERT INTO dbo.eq_audit_maestras (tabla_nombre, registro_id, tipo_cambio)
    VALUES (@tabla_nombre, @id, 'DESACTIVATION');
    
    PRINT 'Registro ' + CAST(@id AS VARCHAR) + ' en ' + @tabla_nombre + ' desactivado para ' + CAST(@fecha_fin AS VARCHAR);
END;

PRINT 'Procedimiento sp_eq_desactivar_maestro creado';
GO

-- SP: Obtener versiones de un maestro
IF OBJECT_ID('dbo.sp_eq_obtener_versiones','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_eq_obtener_versiones;
GO

CREATE PROCEDURE dbo.sp_eq_obtener_versiones
    @tabla_nombre VARCHAR(100),
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @sqlSelect NVARCHAR(MAX);
    SET @sqlSelect = 'SELECT * FROM dbo.' + @tabla_nombre + ' WHERE Id = @id ORDER BY vigente_desde DESC;';
    
    EXEC sp_executesql @sqlSelect, N'@id INT', @id;
END;

PRINT 'Procedimiento sp_eq_obtener_versiones creado';

PRINT 'Sprint 2.1 (Versionado) completado exitosamente';
