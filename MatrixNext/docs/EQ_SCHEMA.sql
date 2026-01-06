-- EQ_SCHEMA.sql
-- Definición de tablas, tipos, procedimientos y seeds para EasyQuote en MatrixNext (área EQ).
-- Ejecutar en SQL Server antes de implementar el módulo.

SET NOCOUNT ON;

------------------------------------------------------------
-- TABLAS MAESTRAS
------------------------------------------------------------

IF OBJECT_ID('dbo.eq_param_penetracion','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_param_penetracion (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Codigo VARCHAR(20) NOT NULL UNIQUE,
        Etiqueta VARCHAR(50) NOT NULL,
        ValorMin DECIMAL(10,4) NULL,
        ValorMax DECIMAL(10,4) NULL
    );
END;

IF OBJECT_ID('dbo.eq_param_metodologia','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_param_metodologia (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Codigo VARCHAR(50) NOT NULL UNIQUE,
        Descripcion VARCHAR(100) NOT NULL
    );
END;
ELSE
BEGIN
    -- Aumentar longitud si ya existe (requiere soltar FK temporal)
    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.eq_param_metodologia') AND name = 'Codigo' AND max_length < 100)
    BEGIN
        IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_eq_param_precio_metodologia')
            ALTER TABLE dbo.eq_param_precio DROP CONSTRAINT FK_eq_param_precio_metodologia;
        ALTER TABLE dbo.eq_param_metodologia ALTER COLUMN Codigo VARCHAR(50) NOT NULL;
    END
END;

IF OBJECT_ID('dbo.eq_param_precio','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_param_precio (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        MetodologiaCodigo VARCHAR(50) NOT NULL,
        PenetracionCodigo VARCHAR(20) NOT NULL,
        DuracionMin INT NOT NULL,
        ValorPerfil DECIMAL(18,2) NOT NULL,
        ValorCoordinacion DECIMAL(18,2) NOT NULL,
        ValorTotal DECIMAL(18,2) NOT NULL,
        CONSTRAINT FK_eq_param_precio_metodologia FOREIGN KEY (MetodologiaCodigo) REFERENCES dbo.eq_param_metodologia(Codigo),
        CONSTRAINT FK_eq_param_precio_penetracion FOREIGN KEY (PenetracionCodigo) REFERENCES dbo.eq_param_penetracion(Codigo)
    );
END;
ELSE
BEGIN
    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.eq_param_precio') AND name = 'MetodologiaCodigo' AND max_length < 100)
        ALTER TABLE dbo.eq_param_precio ALTER COLUMN MetodologiaCodigo VARCHAR(50) NOT NULL;
END;

-- Re-crear FK si se eliminó
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_eq_param_precio_metodologia')
    ALTER TABLE dbo.eq_param_precio ADD CONSTRAINT FK_eq_param_precio_metodologia FOREIGN KEY (MetodologiaCodigo) REFERENCES dbo.eq_param_metodologia(Codigo);

IF OBJECT_ID('dbo.eq_param_script_proc','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_param_script_proc (
        DuracionMin INT PRIMARY KEY,
        HorasScript DECIMAL(10,2) NOT NULL,
        HorasProcesamiento DECIMAL(10,2) NOT NULL,
        HorasHarmoni DECIMAL(10,2) NOT NULL,
        HorasGraficacion DECIMAL(10,2) NOT NULL
    );
END;

IF OBJECT_ID('dbo.eq_valor_hora_ops','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_valor_hora_ops (
        Nivel VARCHAR(10) PRIMARY KEY,
        Variante VARCHAR(20) NOT NULL,
        ValorHora DECIMAL(18,2) NOT NULL
    );
END;

IF OBJECT_ID('dbo.eq_rate_estadistica','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_rate_estadistica (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Categoria VARCHAR(100) NOT NULL,
        Servicio VARCHAR(200) NOT NULL,
        HorasEstimadas DECIMAL(10,2) NOT NULL,
        PrecioReferencia DECIMAL(18,2) NOT NULL,
        FactorEscala DECIMAL(10,2) NOT NULL,
        LeadTime VARCHAR(50) NULL
    );
END;

IF OBJECT_ID('dbo.eq_cost_insumos','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_cost_insumos (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        NSE VARCHAR(10) NOT NULL,
        Tipo VARCHAR(20) NOT NULL, -- Reclutamiento / Obsequio
        ValorUnitario DECIMAL(18,2) NOT NULL
    );
END;

IF OBJECT_ID('dbo.eq_locaciones','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_locaciones (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Ciudad VARCHAR(50) NOT NULL UNIQUE,
        TarifaBase DECIMAL(18,2) NOT NULL,
        TarifaConGross DECIMAL(18,2) NOT NULL,
        DiasBase DECIMAL(10,2) NOT NULL
    );
END;

IF OBJECT_ID('dbo.eq_envio_tarifa','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_envio_tarifa (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Tipologia VARCHAR(20) NOT NULL, -- URBANO / NACIONAL / REEXPEDICION
        KiloInicial DECIMAL(18,2) NOT NULL,
        KiloAdicional DECIMAL(18,2) NOT NULL,
        SeguroPct DECIMAL(10,4) NOT NULL,
        ValorDeclaradoMin DECIMAL(18,2) NOT NULL
    );
END;

IF OBJECT_ID('dbo.eq_codificacion_param','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_codificacion_param (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Escenario VARCHAR(50) NOT NULL,
        Registros INT NOT NULL,
        PregAbiertas INT NOT NULL,
        PregAbiertasMult INT NOT NULL,
        Dias DECIMAL(10,2) NOT NULL,
        Horas DECIMAL(10,2) NOT NULL,
        ValorIpsos DECIMAL(18,2) NOT NULL
    );
END;

IF OBJECT_ID('dbo.eq_tarifa_mystery','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_tarifa_mystery (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        TipoVisita VARCHAR(50) NOT NULL,
        Complejidad VARCHAR(50) NOT NULL,
        VrUnitario DECIMAL(18,2) NOT NULL,
        OlasDefault INT NOT NULL
    );
END;

IF OBJECT_ID('dbo.eq_insumos_prueba','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_insumos_prueba (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Codigo VARCHAR(20) NOT NULL,
        Descripcion VARCHAR(100) NOT NULL,
        Precio DECIMAL(18,2) NOT NULL,
        Unidad VARCHAR(20) NOT NULL,
        Cantidad DECIMAL(18,4) NOT NULL
    );
END;

IF OBJECT_ID('dbo.eq_cost_unitario_ops','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_cost_unitario_ops (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        CodMatrix INT NOT NULL,
        Actividad VARCHAR(150) NOT NULL,
        Tarifa DECIMAL(18,2) NOT NULL,
        Unidad VARCHAR(30) NOT NULL,
        Horas DECIMAL(18,4) NULL
    );
END;

-- Productividad por ciudad (para dias de campo y viaticos)
IF OBJECT_ID('dbo.eq_productividad_ciudad','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_productividad_ciudad (
        Ciudad VARCHAR(80) PRIMARY KEY,
        Encuestadores DECIMAL(10,2) NOT NULL,
        Productividad DECIMAL(10,2) NOT NULL
    );
END;

-- Parametros varios (refrigeracion, divisor volumetrico, costo nevera, etc.)
IF OBJECT_ID('dbo.eq_param_misc','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_param_misc (
        Clave VARCHAR(50) PRIMARY KEY,
        ValorDecimal DECIMAL(18,4) NULL,
        ValorTexto VARCHAR(200) NULL
    );
END;

-- Parametros de envio (divisor volumetrico y tipologias default)
IF OBJECT_ID('dbo.eq_envio_param','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_envio_param (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        DivisorVolumetrico DECIMAL(18,4) NOT NULL,
        TipologiaUrbano VARCHAR(20) NOT NULL,
        TipologiaNacional VARCHAR(20) NOT NULL
    );
END;

-- Costos base de datos
IF OBJECT_ID('dbo.eq_cost_base_datos','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_cost_base_datos (
        Tipo VARCHAR(50) PRIMARY KEY,
        Valor DECIMAL(18,2) NOT NULL
    );
END;

-- Matriz CATI (duracion vs penetracion para CATI)
IF OBJECT_ID('dbo.eq_param_cati','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_param_cati (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        DuracionMin INT NOT NULL,
        PenetracionCodigo VARCHAR(20) NOT NULL,
        ValorPerfil DECIMAL(18,2) NOT NULL,
        ValorCoordinacion DECIMAL(18,2) NOT NULL,
        ValorTotal DECIMAL(18,2) NOT NULL,
        Version INT DEFAULT 1,
        VigenteDesde DATE NULL,
        VigenteHasta DATE NULL,
        CONSTRAINT UQ_CATI_Duracion_Penetracion UNIQUE (DuracionMin, PenetracionCodigo, Version)
    );
END;

-- Matriz Online/Auto (duracion vs penetracion para Online)
IF OBJECT_ID('dbo.eq_param_online','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_param_online (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        DuracionMin INT NOT NULL,
        PenetracionCodigo VARCHAR(20) NOT NULL,
        ValorPerfil DECIMAL(18,2) NOT NULL,
        ValorCoordinacion DECIMAL(18,2) NOT NULL,
        ValorTotal DECIMAL(18,2) NOT NULL,
        Version INT DEFAULT 1,
        VigenteDesde DATE NULL,
        VigenteHasta DATE NULL,
        CONSTRAINT UQ_ONLINE_Duracion_Penetracion UNIQUE (DuracionMin, PenetracionCodigo, Version)
    );
END;

-- Factores (script tipo, clase prueba, apoyo, etiquetado, prob aprobacion)
IF OBJECT_ID('dbo.eq_param_factores','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_param_factores (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Tipo VARCHAR(50) NOT NULL, -- 'SCRIPT_TIPO', 'CLASE_PRUEBA', 'APOYO_RECLUTAMIENTO', 'ETIQUETADO', 'PROB_APROBACION'
        Codigo VARCHAR(50) NOT NULL,
        Descripcion VARCHAR(200) NOT NULL,
        Factor DECIMAL(10,4) NOT NULL DEFAULT 1.0,
        Orden INT NOT NULL,
        Activo BIT DEFAULT 1,
        CONSTRAINT UQ_Factores_Tipo_Codigo UNIQUE (Tipo, Codigo)
    );
END;

-- Tabla Horas SL (horas minimas por SL + RecordDetail + MetodologiaSL)
IF OBJECT_ID('dbo.eq_rate_horas','U') IS NULL
BEGIN
    CREATE TABLE dbo.eq_rate_horas (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        SL VARCHAR(50) NOT NULL,
        RecordDetail VARCHAR(100) NOT NULL,
        MetodologiaSL VARCHAR(100) NOT NULL,
        HorasL3 DECIMAL(10,2) DEFAULT 0,
        HorasL4 DECIMAL(10,2) DEFAULT 0,
        HorasL5 DECIMAL(10,2) DEFAULT 0,
        HorasL6 DECIMAL(10,2) DEFAULT 0,
        HorasL7 DECIMAL(10,2) DEFAULT 0,
        LoadedRateL3 DECIMAL(18,2) DEFAULT 0,
        LoadedRateL4 DECIMAL(18,2) DEFAULT 0,
        LoadedRateL5 DECIMAL(18,2) DEFAULT 0,
        LoadedRateL6 DECIMAL(18,2) DEFAULT 0,
        LoadedRateL7 DECIMAL(18,2) DEFAULT 0,
        BillingRateL3 DECIMAL(18,2) DEFAULT 0,
        BillingRateL4 DECIMAL(18,2) DEFAULT 0,
        BillingRateL5 DECIMAL(18,2) DEFAULT 0,
        BillingRateL6 DECIMAL(18,2) DEFAULT 0,
        BillingRateL7 DECIMAL(18,2) DEFAULT 0,
        [Key] AS (SL + '|' + RecordDetail + '|' + MetodologiaSL) PERSISTED,
        CONSTRAINT UQ_Horas_Key UNIQUE (SL, RecordDetail, MetodologiaSL)
    );
END;

------------------------------------------------------------
-- TABLAS DE OPERACION
------------------------------------------------------------

-- Drop detalle primero para evitar FK
IF OBJECT_ID(''dbo.eq_logistica'',''U'') IS NOT NULL DROP TABLE dbo.eq_logistica;
IF OBJECT_ID(''dbo.eq_staff_sl'',''U'') IS NOT NULL DROP TABLE dbo.eq_staff_sl;
IF OBJECT_ID(''dbo.eq_mystery_visit'',''U'') IS NOT NULL DROP TABLE dbo.eq_mystery_visit;
IF OBJECT_ID(''dbo.eq_sample_city'',''U'') IS NOT NULL DROP TABLE dbo.eq_sample_city;
IF OBJECT_ID(''dbo.eq_methodology'',''U'') IS NOT NULL DROP TABLE dbo.eq_methodology;
IF OBJECT_ID(''dbo.eq_questionnaire'',''U'') IS NOT NULL DROP TABLE dbo.eq_questionnaire;
IF OBJECT_ID(''dbo.eq_quote_header'',''U'') IS NOT NULL DROP TABLE dbo.eq_quote_header;

CREATE TABLE dbo.eq_quote_header (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(150) NOT NULL,
    GrupoObjetivo VARCHAR(200) NOT NULL,
    Cliente VARCHAR(200) NOT NULL,
    FechaAprobacionEstimada DATE NULL,
    FechaCampo DATE NULL,
    ProbAprobacion VARCHAR(20) NOT NULL,
    SL VARCHAR(50) NOT NULL,
    MetodologiaSL VARCHAR(100) NOT NULL,
    RecordDetail VARCHAR(100) NOT NULL,
    CategoriaProducto VARCHAR(100) NULL,
    ValorProveedorExterno DECIMAL(18,2) DEFAULT 0,
    ValorProveedorInternacional DECIMAL(18,2) DEFAULT 0,
    ValorGMU DECIMAL(18,2) DEFAULT 0,
    Estado SMALLINT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);

CREATE TABLE dbo.eq_questionnaire (
    QuoteId BIGINT PRIMARY KEY,
    DuracionMin INT NOT NULL,
    PenetracionCodigo VARCHAR(20) NOT NULL,
    PregAbiertas INT NOT NULL,
    PregAbiertasMult INT NOT NULL,
    TopLine BIT NOT NULL,
    DataCleaning VARCHAR(20) NOT NULL,
    ASCIIFlag BIT NOT NULL,
    ScriptReclutamiento BIT NOT NULL,
    Scripting BIT NOT NULL,
    ScriptingTipo VARCHAR(20) NULL,
    Codificacion BIT NOT NULL,
    Procesamiento BIT NOT NULL,
    NumProcesamientos INT NOT NULL,
    ProcesoEstadistico BIT NOT NULL,
    ClasePrueba VARCHAR(50) NULL,
    Refrigeracion BIT NOT NULL,
    CompraProducto DECIMAL(18,2) DEFAULT 0,
    EtiquetadoTipo VARCHAR(50) NULL,
    Embalaje BIT NOT NULL,
    ProductosTestear INT NOT NULL,
    ProductosPorResp INT NOT NULL,
    PatinadoresCiudad INT NOT NULL,
    Siembra BIT NOT NULL,
    Harmoni BIT DEFAULT 0,
    Graficacion BIT DEFAULT 0,
    OtrosCostos DECIMAL(18,2) DEFAULT 0,
    FOREIGN KEY (QuoteId) REFERENCES dbo.eq_quote_header(Id)
);

CREATE TABLE dbo.eq_methodology (
    QuoteId BIGINT PRIMARY KEY,
    MetodologiaRecoleccion VARCHAR(50) NOT NULL,
    Tecnica1 VARCHAR(50) NULL,
    Tecnica2 VARCHAR(50) NULL,
    Tecnica3 VARCHAR(50) NULL,
    BaseDatos VARCHAR(50) NULL,
    IncidenciaLabel VARCHAR(50) NULL,
    IncidenciaValor DECIMAL(10,4) NULL,
    SobreMuestraPct DECIMAL(10,4) DEFAULT 0,
    EnvioCiudades BIT NOT NULL,
    PesoProductoGr DECIMAL(18,2) DEFAULT 0,
    FOREIGN KEY (QuoteId) REFERENCES dbo.eq_quote_header(Id)
);

CREATE TABLE dbo.eq_sample_city (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    QuoteId BIGINT NOT NULL,
    Ciudad VARCHAR(80) NOT NULL,
    Activa BIT NOT NULL,
    MuestraTotal DECIMAL(18,2) NOT NULL,
    NSE1 DECIMAL(18,2) NOT NULL,
    NSE2 DECIMAL(18,2) NOT NULL,
    NSE3 DECIMAL(18,2) NOT NULL,
    NSE4 DECIMAL(18,2) NOT NULL,
    NSE5 DECIMAL(18,2) NOT NULL,
    NSE6 DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (QuoteId) REFERENCES dbo.eq_quote_header(Id)
);

CREATE TABLE dbo.eq_mystery_visit (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    QuoteId BIGINT NOT NULL,
    TipoVisita VARCHAR(50) NOT NULL,
    Complejidad VARCHAR(50) NOT NULL,
    NumOlas INT NOT NULL,
    Desplazamientos DECIMAL(18,2) NULL,
    Tanqueos DECIMAL(18,2) NULL,
    Alertas DECIMAL(18,2) NULL,
    Edicion DECIMAL(18,2) NULL,
    AlquilerEquipos DECIMAL(18,2) NULL,
    CompraDispositivos DECIMAL(18,2) NULL,
    FOREIGN KEY (QuoteId) REFERENCES dbo.eq_quote_header(Id)
);

CREATE TABLE dbo.eq_staff_sl (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    QuoteId BIGINT NOT NULL,
    Nivel VARCHAR(10) NOT NULL,
    HorasMinimas DECIMAL(10,2) NOT NULL,
    HorasPresup DECIMAL(10,2) NOT NULL,
    Tarifa DECIMAL(18,2) NOT NULL,
    Valor DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (QuoteId) REFERENCES dbo.eq_quote_header(Id)
);

-- Logistica operativa del proyecto
CREATE TABLE dbo.eq_logistica (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    QuoteId BIGINT NOT NULL,
    DiasSetup INT DEFAULT 0,
    DiasCampo INT DEFAULT 0,
    NumOlas INT DEFAULT 1,
    ApoyoReclutamientoTipo VARCHAR(50) NULL,
    TaxiParticipantes BIT DEFAULT 0,
    EstudioNinos BIT DEFAULT 0,
    ReprografiaPaginas INT DEFAULT 0,
    ViaticasCampoOverride DECIMAL(18,2) NULL,
    OtrosIncentivos DECIMAL(18,2) NULL DEFAULT 0,
    DimensionLargoCm DECIMAL(10,2) NULL,
    DimensionAnchoCm DECIMAL(10,2) NULL,
    DimensionAltoCm DECIMAL(10,2) NULL,
    FOREIGN KEY (QuoteId) REFERENCES dbo.eq_quote_header(Id)
);
-- TIPOS DE TABLA PARA TVP
------------------------------------------------------------

IF TYPE_ID(N'dbo.EQ_SampleCityType') IS NULL
    CREATE TYPE dbo.EQ_SampleCityType AS TABLE (
        Ciudad VARCHAR(80),
        Activa BIT,
        MuestraTotal DECIMAL(18,2),
        NSE1 DECIMAL(18,2),
        NSE2 DECIMAL(18,2),
        NSE3 DECIMAL(18,2),
        NSE4 DECIMAL(18,2),
        NSE5 DECIMAL(18,2),
        NSE6 DECIMAL(18,2)
    );

IF TYPE_ID(N'dbo.EQ_MysteryVisitType') IS NULL
    CREATE TYPE dbo.EQ_MysteryVisitType AS TABLE (
        TipoVisita VARCHAR(50),
        Complejidad VARCHAR(50),
        NumOlas INT,
        Desplazamientos DECIMAL(18,2),
        Tanqueos DECIMAL(18,2),
        Alertas DECIMAL(18,2),
        Edicion DECIMAL(18,2),
        AlquilerEquipos DECIMAL(18,2),
        CompraDispositivos DECIMAL(18,2)
    );

IF TYPE_ID(N'dbo.EQ_StaffSLType') IS NULL
    CREATE TYPE dbo.EQ_StaffSLType AS TABLE (
        Nivel VARCHAR(10),
        HorasPresup DECIMAL(10,2),
        Tarifa DECIMAL(18,2)
    );

IF TYPE_ID(N'dbo.EQ_QuestionnaireType') IS NULL
    CREATE TYPE dbo.EQ_QuestionnaireType AS TABLE (
        DuracionMin INT,
        PenetracionCodigo VARCHAR(20),
        PregAbiertas INT,
    PregAbiertasMult INT,
    TopLine BIT,
    DataCleaning VARCHAR(20),
    ASCIIFlag BIT,
    ScriptReclutamiento BIT,
    Scripting BIT,
    ScriptingTipo VARCHAR(20),
    Codificacion BIT,
    Procesamiento BIT,
    NumProcesamientos INT,
    ProcesoEstadistico BIT,
    ClasePrueba VARCHAR(50),
    Refrigeracion BIT,
    CompraProducto DECIMAL(18,2),
    EtiquetadoTipo VARCHAR(50),
    Embalaje BIT,
    ProductosTestear INT,
    ProductosPorResp INT,
    PatinadoresCiudad INT,
    Siembra BIT,
    Harmoni BIT,
    Graficacion BIT,
    OtrosCostos DECIMAL(18,2)
);

IF TYPE_ID(N'dbo.EQ_MethodologyType') IS NULL
    CREATE TYPE dbo.EQ_MethodologyType AS TABLE (
        MetodologiaRecoleccion VARCHAR(50),
        Tecnica1 VARCHAR(50),
        Tecnica2 VARCHAR(50),
        Tecnica3 VARCHAR(50),
        BaseDatos VARCHAR(50),
        IncidenciaLabel VARCHAR(50),
        IncidenciaValor DECIMAL(10,4),
        SobreMuestraPct DECIMAL(10,4),
        EnvioCiudades BIT,
        PesoProductoGr DECIMAL(18,2)
    );

IF TYPE_ID(N'dbo.EQ_LogisticaType') IS NOT NULL
    DROP TYPE dbo.EQ_LogisticaType;
GO
CREATE TYPE dbo.EQ_LogisticaType AS TABLE (
    DiasSetup INT,
    DiasCampo INT,
    NumOlas INT,
    ApoyoReclutamientoTipo VARCHAR(50),
    TaxiParticipantes BIT,
    EstudioNinos BIT,
    ReprografiaPaginas INT,
    ViaticasCampoOverride DECIMAL(18,2),
    OtrosIncentivos DECIMAL(18,2),
    DimensionLargoCm DECIMAL(10,2),
    DimensionAnchoCm DECIMAL(10,2),
    DimensionAltoCm DECIMAL(10,2)
);

------------------------------------------------------------
-- PROCEDIMIENTOS
------------------------------------------------------------

IF OBJECT_ID('dbo.EQ_Quote_Save','P') IS NOT NULL
    DROP PROCEDURE dbo.EQ_Quote_Save;
GO
CREATE PROCEDURE dbo.EQ_Quote_Save
(
    @Id BIGINT OUTPUT,
    @Nombre VARCHAR(150),
    @GrupoObjetivo VARCHAR(200),
    @Cliente VARCHAR(200),
    @FechaAprobacionEstimada DATE = NULL,
    @FechaCampo DATE = NULL,
    @ProbAprobacion VARCHAR(20),
    @SL VARCHAR(50),
    @MetodologiaSL VARCHAR(100),
    @RecordDetail VARCHAR(100),
    @CategoriaProducto VARCHAR(100) = NULL,
    @ValorProveedorExterno DECIMAL(18,2) = 0,
    @ValorProveedorInternacional DECIMAL(18,2) = 0,
    @ValorGMU DECIMAL(18,2) = 0,
    @Questionnaire dbo.EQ_QuestionnaireType READONLY, -- se espera una fila
    @Methodology dbo.EQ_MethodologyType READONLY,     -- se espera una fila
    @SampleCities dbo.EQ_SampleCityType READONLY,
    @Mystery dbo.EQ_MysteryVisitType READONLY,
    @StaffSL dbo.EQ_StaffSLType READONLY,
    @Logistica dbo.EQ_LogisticaType READONLY          -- se espera una fila
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF (@Id IS NULL OR @Id = 0)
        BEGIN
            INSERT dbo.eq_quote_header
            (Nombre, GrupoObjetivo, Cliente, FechaAprobacionEstimada, FechaCampo,
             ProbAprobacion, SL, MetodologiaSL, RecordDetail, CategoriaProducto,
             ValorProveedorExterno, ValorProveedorInternacional, ValorGMU)
            VALUES
            (@Nombre, @GrupoObjetivo, @Cliente, @FechaAprobacionEstimada, @FechaCampo,
             @ProbAprobacion, @SL, @MetodologiaSL, @RecordDetail, @CategoriaProducto,
             @ValorProveedorExterno, @ValorProveedorInternacional, @ValorGMU);

            SET @Id = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            UPDATE dbo.eq_quote_header
            SET Nombre = @Nombre,
                GrupoObjetivo = @GrupoObjetivo,
                Cliente = @Cliente,
                FechaAprobacionEstimada = @FechaAprobacionEstimada,
                FechaCampo = @FechaCampo,
                ProbAprobacion = @ProbAprobacion,
                SL = @SL,
                MetodologiaSL = @MetodologiaSL,
                RecordDetail = @RecordDetail,
                CategoriaProducto = @CategoriaProducto,
                ValorProveedorExterno = @ValorProveedorExterno,
                ValorProveedorInternacional = @ValorProveedorInternacional,
                ValorGMU = @ValorGMU
            WHERE Id = @Id;
        END

        -- questionnaire (uno a uno)
        DELETE FROM dbo.eq_questionnaire WHERE QuoteId = @Id;
        INSERT dbo.eq_questionnaire
        (QuoteId, DuracionMin, PenetracionCodigo, PregAbiertas, PregAbiertasMult, TopLine, DataCleaning,
         ASCIIFlag, ScriptReclutamiento, Scripting, ScriptingTipo, Codificacion, Procesamiento,
         NumProcesamientos, ProcesoEstadistico, ClasePrueba, Refrigeracion, CompraProducto,
         EtiquetadoTipo, Embalaje, ProductosTestear, ProductosPorResp, PatinadoresCiudad, Siembra,
         Harmoni, Graficacion, OtrosCostos)
        SELECT @Id, DuracionMin, PenetracionCodigo, PregAbiertas, PregAbiertasMult, TopLine, DataCleaning,
               ASCIIFlag, ScriptReclutamiento, Scripting, ScriptingTipo, Codificacion, Procesamiento,
               NumProcesamientos, ProcesoEstadistico, ClasePrueba, Refrigeracion, CompraProducto,
               EtiquetadoTipo, Embalaje, ProductosTestear, ProductosPorResp, PatinadoresCiudad, Siembra,
               Harmoni, Graficacion, OtrosCostos
        FROM @Questionnaire;

        -- metodologia (uno a uno)
        DELETE FROM dbo.eq_methodology WHERE QuoteId = @Id;
        INSERT dbo.eq_methodology
        (QuoteId, MetodologiaRecoleccion, Tecnica1, Tecnica2, Tecnica3, BaseDatos,
         IncidenciaLabel, IncidenciaValor, SobreMuestraPct, EnvioCiudades, PesoProductoGr)
        SELECT @Id, MetodologiaRecoleccion, Tecnica1, Tecnica2, Tecnica3, BaseDatos,
               IncidenciaLabel, IncidenciaValor, SobreMuestraPct, EnvioCiudades, PesoProductoGr
        FROM @Methodology;

        -- sample cities
        DELETE FROM dbo.eq_sample_city WHERE QuoteId = @Id;
        INSERT dbo.eq_sample_city
        (QuoteId, Ciudad, Activa, MuestraTotal, NSE1, NSE2, NSE3, NSE4, NSE5, NSE6)
        SELECT @Id, Ciudad, Activa, MuestraTotal, NSE1, NSE2, NSE3, NSE4, NSE5, NSE6
        FROM @SampleCities;

        -- mystery
        DELETE FROM dbo.eq_mystery_visit WHERE QuoteId = @Id;
        INSERT dbo.eq_mystery_visit
        (QuoteId, TipoVisita, Complejidad, NumOlas, Desplazamientos, Tanqueos, Alertas, Edicion, AlquilerEquipos, CompraDispositivos)
        SELECT @Id, TipoVisita, Complejidad, NumOlas, Desplazamientos, Tanqueos, Alertas, Edicion, AlquilerEquipos, CompraDispositivos
        FROM @Mystery;

        -- staff SL
        DELETE FROM dbo.eq_staff_sl WHERE QuoteId = @Id;
        INSERT dbo.eq_staff_sl (QuoteId, Nivel, HorasMinimas, HorasPresup, Tarifa, Valor)
        SELECT @Id, Nivel,
               0, -- horas minimas se calculan en capa de negocio al cargar desde eq_valor_hora_ops/tabla Horas
               HorasPresup,
               Tarifa,
               HorasPresup * Tarifa
        FROM @StaffSL;

        -- logistica (uno a uno)
        DELETE FROM dbo.eq_logistica WHERE QuoteId = @Id;
        INSERT dbo.eq_logistica
        (QuoteId, DiasSetup, DiasCampo, NumOlas, ApoyoReclutamientoTipo, TaxiParticipantes, EstudioNinos,
         ReprografiaPaginas, ViaticasCampoOverride, OtrosIncentivos, DimensionLargoCm, DimensionAnchoCm, DimensionAltoCm)
        SELECT @Id, DiasSetup, DiasCampo, NumOlas, ApoyoReclutamientoTipo, TaxiParticipantes, EstudioNinos,
               ReprografiaPaginas, ViaticasCampoOverride, OtrosIncentivos, DimensionLargoCm, DimensionAnchoCm, DimensionAltoCm
        FROM @Logistica;
    END TRY
    BEGIN CATCH
        DECLARE @Err NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err, 16, 1);
    END CATCH
END;
GO

IF OBJECT_ID('dbo.EQ_Quote_Get','P') IS NOT NULL
    DROP PROCEDURE dbo.EQ_Quote_Get;
GO
CREATE PROCEDURE dbo.EQ_Quote_Get (@Id BIGINT)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.eq_quote_header WHERE Id = @Id;
    SELECT * FROM dbo.eq_questionnaire WHERE QuoteId = @Id;
    SELECT * FROM dbo.eq_methodology WHERE QuoteId = @Id;
    SELECT * FROM dbo.eq_sample_city WHERE QuoteId = @Id;
    SELECT * FROM dbo.eq_mystery_visit WHERE QuoteId = @Id;
    SELECT * FROM dbo.eq_staff_sl WHERE QuoteId = @Id;
END;
GO

------------------------------------------------------------
-- SEEDS
------------------------------------------------------------

-- Penetraciones
IF NOT EXISTS (SELECT 1 FROM dbo.eq_param_penetracion)
BEGIN
    INSERT dbo.eq_param_penetracion (Codigo, Etiqueta, ValorMin, ValorMax) VALUES
    ('MAS82','Mas 82%',0.82,1),
    ('75_82','75%-82%',0.75,0.82),
    ('67_74','67%-74%',0.67,0.74),
    ('55_66','55%-66%',0.55,0.66),
    ('46_54','46%-54%',0.46,0.54),
    ('37_45','37%-45%',0.37,0.45);
END;

-- Metodologias (upsert por código)
MERGE dbo.eq_param_metodologia AS target
USING (VALUES
    ('F2F','Entrevista presencial'),
    ('CATI','Entrevista telefonica'),
    ('ONLINE','Online/Panel'),
    ('AUTO_INNO','Autoaplicado INNO'),
    ('MYSTERY','Mystery Shopper'),
    ('SHOPPER_JORNADA','Shopper por jornada'),
    ('SHOPPER_PRODUCTIVIDAD','Shopper por productividad')
) AS src(Codigo, Descripcion)
ON target.Codigo = src.Codigo
WHEN MATCHED THEN UPDATE SET target.Descripcion = src.Descripcion
WHEN NOT MATCHED THEN INSERT (Codigo, Descripcion) VALUES (src.Codigo, src.Descripcion);

-- Corregir posibles códigos truncados existentes
UPDATE dbo.eq_param_metodologia
SET Codigo = 'SHOPPER_PRODUCTIVIDAD'
WHERE Codigo LIKE 'SHOPPER_PRODUCTIVIDA%';

-- Matriz precios base (F2F, CATI, ONLINE, AUTO_INNO) desde XLSM (upsert)
MERGE dbo.eq_param_precio AS target
USING (VALUES
    ('F2F','MAS82',5,7123,13207,20330),
    ('F2F','75_82',5,7623,13207,20830),
    ('F2F','67_74',5,8123,13207,21330),
    ('F2F','55_66',5,8523,13307,21830),
    ('F2F','MAS82',10,8123,13207,21330),
    ('F2F','75_82',10,8623,13207,21830),
    ('F2F','67_74',10,9123,13207,22330),
    ('F2F','55_66',10,9523,13307,22830),
    ('F2F','MAS82',15,9623,13207,22830),
    ('F2F','75_82',15,10123,13207,23330),
    ('F2F','67_74',15,10623,13207,23830),
    ('F2F','55_66',15,11023,13307,24330),
    ('F2F','MAS82',20,11623,13207,24830),
    ('F2F','75_82',20,12123,13207,25330),
    ('F2F','67_74',20,12623,13207,25830),
    ('F2F','55_66',20,13023,13307,26330),
    ('F2F','MAS82',30,14123,13207,27330),
    ('F2F','75_82',30,14623,13207,27830),
    ('F2F','67_74',30,15123,13207,28330),
    ('F2F','55_66',30,15523,13307,28830),
    ('F2F','MAS82',40,17123,13207,30330),
    ('F2F','75_82',40,17623,13207,30830),
    ('F2F','67_74',40,18123,13207,31330),
    ('F2F','55_66',40,18523,13307,31830),
    ('F2F','MAS82',50,20123,13207,33330),
    ('F2F','75_82',50,20623,13207,33830),
    ('F2F','67_74',50,21123,13207,34330),
    ('F2F','55_66',50,21523,13307,34830),
    ('F2F','MAS82',60,25123,13207,38330),
    ('F2F','75_82',60,25623,13207,38830),
    ('F2F','67_74',60,26123,13207,39330),
    ('F2F','55_66',60,26523,13307,39830),
    ('CATI','MAS82',5,15028.872,13893.764,21387.16),
    ('CATI','75_82',5,15028.872,13893.764,21913.16),
    ('CATI','67_74',5,15028.872,13893.764,22439.16),
    ('CATI','55_66',5,18785.564,13998.964,22965.16),
    ('CATI','MAS82',10,15028.872,13893.764,22439.16),
    ('CATI','75_82',10,15028.872,13893.764,22965.16),
    ('CATI','67_74',10,15028.872,13893.764,23491.16),
    ('CATI','55_66',10,18785.564,13998.964,24017.16),
    ('CATI','MAS82',15,19689.232,13893.764,24017.16),
    ('CATI','75_82',15,19689.232,13893.764,24543.16),
    ('CATI','67_74',15,19689.232,13893.764,25069.16),
    ('CATI','55_66',15,24750.404,13998.964,25595.16),
    ('CATI','MAS82',20,19689.232,13893.764,26121.16),
    ('CATI','75_82',20,19689.232,13893.764,26647.16),
    ('CATI','67_74',20,19689.232,13893.764,27173.16),
    ('CATI','55_66',20,24750.404,13998.964,27699.16),
    ('CATI','MAS82',30,24045.564,13893.764,28751.16),
    ('CATI','75_82',30,24045.564,13893.764,29277.16),
    ('CATI','67_74',30,24045.564,13893.764,29803.16),
    ('CATI','55_66',30,30056.692,13998.964,30329.16),
    ('CATI','MAS82',40,30056.692,13893.764,31907.16),
    ('CATI','75_82',40,30056.692,13893.764,32433.16),
    ('CATI','67_74',40,30056.692,13893.764,32959.16),
    ('CATI','55_66',40,37571.128,13998.964,33485.16),
    ('CATI','MAS82',50,40586.16,13893.764,35063.16),
    ('CATI','75_82',50,40586.16,13893.764,35589.16),
    ('CATI','67_74',50,40586.16,13893.764,36115.16),
    ('CATI','55_66',50,63120.0,13998.964,36641.16),
    ('CATI','MAS82',60,51106.16,13893.764,40323.16),
    ('CATI','75_82',60,51106.16,13893.764,40849.16),
    ('CATI','67_74',60,51106.16,13893.764,41375.16),
    ('CATI','55_66',60,73640.0,13998.964,41901.16),
    ('ONLINE','MAS82',5,12020.152,13893.764,21387.16),
    ('AUTO_INNO','MAS82',5,12020.152,13893.764,21387.16),
    ('ONLINE','75_82',5,12966.952,13893.764,21913.16),
    ('AUTO_INNO','75_82',5,12966.952,13893.764,21913.16),
    ('ONLINE','67_74',5,13913.752,13893.764,22439.16),
    ('AUTO_INNO','67_74',5,13913.752,13893.764,22439.16),
    ('ONLINE','55_66',5,14860.552,13998.964,22965.16),
    ('AUTO_INNO','55_66',5,14860.552,13998.964,22965.16),
    ('ONLINE','MAS82',10,13913.752,13893.764,22439.16),
    ('AUTO_INNO','MAS82',10,13913.752,13893.764,22439.16),
    ('ONLINE','75_82',10,14860.552,13893.764,22965.16),
    ('AUTO_INNO','75_82',10,14860.552,13893.764,22965.16),
    ('ONLINE','67_74',10,15807.352,13893.764,23491.16),
    ('AUTO_INNO','67_74',10,15807.352,13893.764,23491.16),
    ('ONLINE','55_66',10,16754.152000000002,13998.964,24017.16),
    ('AUTO_INNO','55_66',10,16754.152000000002,13998.964,24017.16),
    ('ONLINE','MAS82',15,15807.352,13893.764,24017.16),
    ('AUTO_INNO','MAS82',15,15807.352,13893.764,24017.16),
    ('ONLINE','75_82',15,16754.152000000002,13893.764,24543.16),
    ('AUTO_INNO','75_82',15,16754.152000000002,13893.764,24543.16),
    ('ONLINE','67_74',15,17700.952,13893.764,25069.16),
    ('AUTO_INNO','67_74',15,17700.952,13893.764,25069.16),
    ('ONLINE','55_66',15,18647.752,13998.964,25595.16),
    ('AUTO_INNO','55_66',15,18647.752,13998.964,25595.16),
    ('ONLINE','MAS82',20,17700.952,13893.764,26121.16),
    ('AUTO_INNO','MAS82',20,17700.952,13893.764,26121.16),
    ('ONLINE','75_82',20,18647.752,13893.764,26647.16),
    ('AUTO_INNO','75_82',20,18647.752,13893.764,26647.16),
    ('ONLINE','67_74',20,19594.552,13893.764,27173.16),
    ('AUTO_INNO','67_74',20,19594.552,13893.764,27173.16),
    ('ONLINE','55_66',20,20541.352,13998.964,27699.16),
    ('AUTO_INNO','55_66',20,20541.352,13998.964,27699.16),
    ('ONLINE','MAS82',30,19594.552,13893.764,28751.16),
    ('AUTO_INNO','MAS82',30,19594.552,13893.764,28751.16),
    ('ONLINE','75_82',30,20541.352,13893.764,29277.16),
    ('AUTO_INNO','75_82',30,20541.352,13893.764,29277.16),
    ('ONLINE','67_74',30,21488.152000000002,13893.764,29803.16),
    ('AUTO_INNO','67_74',30,21488.152000000002,13893.764,29803.16),
    ('ONLINE','55_66',30,22434.952,13998.964,30329.16),
    ('AUTO_INNO','55_66',30,22434.952,13998.964,30329.16),
    ('ONLINE','MAS82',40,21488.152000000002,13893.764,31907.16),
    ('AUTO_INNO','MAS82',40,21488.152000000002,13893.764,31907.16),
    ('ONLINE','75_82',40,22434.952,13893.764,32433.16),
    ('AUTO_INNO','75_82',40,22434.952,13893.764,32433.16),
    ('ONLINE','67_74',40,23381.752,13893.764,32959.16),
    ('AUTO_INNO','67_74',40,23381.752,13893.764,32959.16),
    ('ONLINE','55_66',40,24328.552,13998.964,33485.16),
    ('AUTO_INNO','55_66',40,24328.552,13998.964,33485.16),
    ('ONLINE','MAS82',50,23381.752,13893.764,35063.16),
    ('AUTO_INNO','MAS82',50,23381.752,13893.764,35063.16),
    ('ONLINE','75_82',50,24328.552,13893.764,35589.16),
    ('AUTO_INNO','75_82',50,24328.552,13893.764,35589.16),
    ('ONLINE','67_74',50,25275.352,13893.764,36115.16),
    ('AUTO_INNO','67_74',50,25275.352,13893.764,36115.16),
    ('ONLINE','55_66',50,26222.152000000002,13998.964,36641.16),
    ('AUTO_INNO','55_66',50,26222.152000000002,13998.964,36641.16),
    ('ONLINE','MAS82',60,25275.352,13893.764,40323.16),
    ('AUTO_INNO','MAS82',60,25275.352,13893.764,40323.16),
    ('ONLINE','75_82',60,26222.152000000002,13893.764,40849.16),
    ('AUTO_INNO','75_82',60,26222.152000000002,13893.764,40849.16),
    ('ONLINE','67_74',60,27168.952,13893.764,41375.16),
    ('AUTO_INNO','67_74',60,27168.952,13893.764,41375.16),
    ('ONLINE','55_66',60,28115.752,13998.964,41901.16),
    ('AUTO_INNO','55_66',60,28115.752,13998.964,41901.16)
) AS src(MetodologiaCodigo, PenetracionCodigo, DuracionMin, ValorPerfil, ValorCoordinacion, ValorTotal)
ON target.MetodologiaCodigo = src.MetodologiaCodigo
AND target.PenetracionCodigo = src.PenetracionCodigo
AND target.DuracionMin = src.DuracionMin
WHEN MATCHED THEN UPDATE SET ValorPerfil = src.ValorPerfil, ValorCoordinacion = src.ValorCoordinacion, ValorTotal = src.ValorTotal
WHEN NOT MATCHED THEN INSERT (MetodologiaCodigo, PenetracionCodigo, DuracionMin, ValorPerfil, ValorCoordinacion, ValorTotal)
VALUES (src.MetodologiaCodigo, src.PenetracionCodigo, src.DuracionMin, src.ValorPerfil, src.ValorCoordinacion, src.ValorTotal);

-- Horas script/procesamiento (Parametros!171-179)
IF NOT EXISTS (SELECT 1 FROM dbo.eq_param_script_proc)
BEGIN
    INSERT dbo.eq_param_script_proc (DuracionMin, HorasScript, HorasProcesamiento, HorasHarmoni, HorasGraficacion) VALUES
    (5,20,18,9,27),
    (10,20,18,9,27),
    (15,20,18,9,27),
    (20,28,18,9,27),
    (30,28,18,9,36),
    (40,41,18,9,36),
    (50,52,18,9,45),
    (60,52,18,9,45);
END;

-- Valor hora OPS (tomando 2023 Alternativa 2)
MERGE dbo.eq_valor_hora_ops AS target
USING (VALUES
    ('L1','2023_ALT2',448570),
    ('L2','2023_ALT2',448570),
    ('L3','2023_ALT2',448570),
    ('L4','2023_ALT2',186189),
    ('L5','2023_ALT2',82663),
    ('L6','2023_ALT2',43293),
    ('L7','2023_ALT2',38855),
    ('L8','2023_ALT2',19428)
) AS src(Nivel, Variante, ValorHora)
ON target.Nivel = src.Nivel AND target.Variante = src.Variante
WHEN MATCHED THEN UPDATE SET ValorHora = src.ValorHora
WHEN NOT MATCHED THEN INSERT (Nivel, Variante, ValorHora) VALUES (src.Nivel, src.Variante, src.ValorHora);

-- Tarifario estadistica completo (Tarifario Estadistica2) upsert por Categoria+Servicio
MERGE dbo.eq_rate_estadistica AS target
USING (VALUES
    ('Procesos Especiales','Modelo de satisfaccion (CX-variable respuesta en escala)',2,224910,0.6,'Dos dias'),
    ('Procesos Especiales','Orden de atributos o Analisis de Drivers (variable respuesta en escala)',3,286856,0.3,'Dos dias'),
    ('Procesos Especiales','Orden de atributos o Analisis de Drivers (variable respuesta dicotomica)',4,449820,0.3,'Dos dias'),
    ('Procesos Especiales','Random Forest',3.5,317829,0.3,'Dos dias'),
    ('Procesos Especiales','Modelos GLM (variable con respuesta dicotomica)',9,759550,0.3,'Dos dias'),
    ('Procesos Especiales','Modelos de machine learning',30,3878740,0.3,'Cuatro dias'),
    ('Procesos Especiales','Modelos para tasa de no respuesta muy altas',10,1023532,0.5,'Tres dias'),
    ('Procesos Especiales','Modelo de anonimizacion',8,899640,0.5,'Tres dias'),
    ('Procesos Especiales','Graphical modelling (INNO-sin magnitud de la relacion)',2.5,356901,0.7,'Dos dias'),
    ('Procesos Especiales','Graphical modelling (con magnitud de la relacion)',3,387874,0.7,'Dos dias'),
    ('Procesos Especiales','Graphical modelling + Orden de atributos  (variable respuesta en escala)',5,511766,0.7,'Dos dias'),
    ('Procesos Especiales','Graphical modelling + Orden de atributos  (variable respuesta dicotomica)',5.5,542739,0.7,'Dos dias'),
    ('Procesos Especiales','Correlaciones/asociaciones',1,162964,0.7,'Dos dias'),
    ('Procesos Especiales','Inferencia Estadistica (diferencias, pruebas de hipotesis, metas, analisis migratorio)',4,449820,0.7,'Dos dias'),
    ('Procesos Especiales','Text analytics (CREATIVE-De palabras)',5,511766,0.75,'Dos dias'),
    ('Procesos Especiales','Text Analytics por LDA ( frases construidas)',13,1209370,0,'Una semana'),
    ('Procesos Especiales','Text analytics con segmentacion y depurado de palabras',8,899640,0.75,'Tres dias'),
    ('Procesos Especiales','Text analytics depurado de marcas',11,1085478,0.75,'Tres dias'),
    ('Procesos Especiales','Text analytics con segmentacion y asociando imagenes',11,1085478,0,'Tres dias'),
    ('Procesos Especiales','Penalty Analysis',2,224910,0.5,'Un dia'),
    ('Procesos Especiales','Reglas de asociacion',3,387874,0.5,'Dos dias'),
    ('Procesos Especiales','Indicadores (ACP)',4,449820,0.7,'Dos dias'),
    ('Procesos Especiales','Analisis factorial (reducion o agrupamiento de variables)',5,612784,0.7,'Tres dias'),
    ('Procesos Especiales','Residuales ajustados',3,286856,0.7,'Dos dias'),
    ('Procesos Especiales','Mapa de correspondencia',4.5,480793,0.7,'Dos dias'),
    ('Procesos Especiales','Mapa de correspondencia+residuales',5.5,542739,0.7,'Tres dias'),
    ('Procesos Especiales','Equity actitudinal con funnel',3,286856,0.8,'Dos dias'),
    ('Procesos Especiales','Segmentacion o perfilamiento (Analisis de Cluster)',14,1271316,0.9,'Tres dias'),
    ('Procesos Especiales','Segmentacion cruzada',28,2542632,0,'Siete dias'),
    ('Procesos Especiales','Segmentacion (Analisis de cluster) + Algoritmo (typing tool)',30,2666524,0.9,'Dos semanas'),
    ('Procesos Especiales','Turf',2.5,255883,0.4,'Dos dias'),
    ('Procesos Especiales','Odds ration',2,224910,0.5,'Dos dias'),
    ('Procesos Especiales','Indicadores simples',2.5,255883,0.9,'Dos dias'),
    ('Procesos Especiales','Penalty & Rewards',5,511766,0.6,'Dos dias'),
    ('Procesos Especiales','Aporte entre marcas (similar a Brand Halo)',4,449820,0.3,'Dos dias'),
    ('Procesos Especiales','Aplicacion de algoritmo de segmentacion',3,286856,0.75,'Un dia'),
    ('Procesos Especiales','Analisis chaid (Arboles de clasificacion)',3.5,418847,0.75,'Dos dias'),
    ('Procesos Especiales','Analisis chaid (CX-Arboles de clasificacion)',2.5,255883,0.75,'Dos dias'),
    ('Procesos Especiales','Analisis de causalidad con redes bayesianas (similar a IBN)',13,1209370,0.5,'Una semana'),
    ('Procesos Especiales','PSM',3.5,317829,0.6,'Dos dias'),
    ('Procesos Especiales','Arbol de decision del Shopper',5,511766,0.9,'Tres dias'),
    ('Procesos Especiales','Max-diff (Analisis Conjoint con diseno ortogonal)',7,837694,0.95,'Dos dias'),
    ('Procesos Especiales','Ecuaciones Estructurales',8,899640,0.9,'Tres dias'),
    ('Procesos Especiales','Elasticidad',5,511766,0.8,'Dos dias'),
    ('Procesos Especiales','Elasticidad (similar a Gabor Granger)',8,697604,0,'Tres dias'),
    ('Procesos Especiales','Modelo de Atributos de Precio por Marca (similar a BIM)',16,1395208,0,'Una semana'),
    ('Procesos Especiales','SOVA',5,511766,0.9,'Dos dias'),
    ('Procesos Especiales','Analisis de consistencia (Alpha cronbach)',5.5,542739,0.9,'Dos dias'),
    ('Procesos Especiales','Ecosystem Flow',18,1519100,0.9,'Cuatro dias'),
    ('Procesos Especiales','Channel Vision',18,1519100,0.75,'Tres dias'),
    ('Procesos Especiales','Potential modelling',4,449820,0.9,'Dos dias'),
    ('Procesos Especiales','t-plot',4,449820,0.9,'Dos dias'),
    ('Procesos Especiales','Analisis Conjoint (CBC)',25,2154758,0.95,'Tres dias'),
    ('Procesos Especiales','Analisis Conjoint (CBC) con elasticidades',42,3813948,0.95,'Una semana'),
    ('Procesos Especiales','Espacios de demanda (similar Demand Space)',26,3226884,1,'Siete dias'),
    ('Procesos Especiales','Means - End Chain (Text Analytics de Atributos-Beneficios-Valores)',39,3224038,0.5,'Dos semanas'),
    ('Procesos Especiales','Dendograma',5,511766,1,'Dos dias'),
    ('Ponderacion','Ponderacion por una variable o dos variables cruzadas',3,255883,1,'Un dia'),
    ('Ponderacion','Ponderacion dos variables (no cruzadas)',4.5,480793,1,'Un dia'),
    ('Ponderacion','Ponderacion tres o mas variables no cruzadas',9,759550,1,'Un dia'),
    ('Ponderacion','Ponderacion por variable diferente a universo (ejemplo ventas)',14,1271316,1,'Tres dias'),
    ('Ponderacion','Factor de Expansion',19,1581046,1,'Tres dias'),
    ('Ponderacion','Factor de Expansion en muestreos no-probabilisticos (estimaciones doblemente robustas)',56,5489336,1,'Una semana'),
    ('Seleccion IDM','Seleccion IDM por listado',5,511766,1,'Dos dias'),
    ('Seleccion IDM','Seleccion IDM de municipios menos de 5 municpios)',4.5,278757,1,'Dos dias'),
    ('Seleccion IDM','Seleccion IDM entre 6 y 20 municipios',7.5,464595,1,'Dos dias'),
    ('Seleccion IDM','Seleccion IDM mas de 20 municipios',11,883442,1,'Tres dias'),
    ('Seleccion IDM','Reemplazos de IDM',2,184502.8,1,'El mismo dia (si se solicita antes del medio dia)'),
    ('Seleccion IDM','Cartografia para un municipio nuevo en la seleccion de IDM',3,185838,0.7,'Un dia'),
    ('Metodologias','Metodologia (sin diseno muestral)',2,224910,0.5,'Un dia'),
    ('Metodologias','Metodologia (con diseno muestral)',5,511766,0.5,'Un dia'),
    ('Metodologias','Metodologia (PA-con diseno muestral probabilistico)',10,1023532,0.5,'Dos dias'),
    ('Diseno Muestral','Propuesta Diseno muestral basico',7,635658,0.5,'Dos dias'),
    ('Diseno Muestral','Propuesta diseno muestral complejo',70,8376940,0,'A convenir'),
    ('Diseno Muestral','Actualizacion de diseno muestral complejo',120,11474240,0,'A convenir'),
    ('Procesos Especiales','Tabulados con error muestral o estimaciones asistidas por modelos',50,5117660,1,'A convenir')
) AS src(Categoria, Servicio, HorasEstimadas, PrecioReferencia, FactorEscala, LeadTime)
ON target.Categoria = src.Categoria AND target.Servicio = src.Servicio
WHEN MATCHED THEN UPDATE SET HorasEstimadas = src.HorasEstimadas, PrecioReferencia = src.PrecioReferencia, FactorEscala = src.FactorEscala, LeadTime = src.LeadTime
WHEN NOT MATCHED THEN INSERT (Categoria, Servicio, HorasEstimadas, PrecioReferencia, FactorEscala, LeadTime)
VALUES (src.Categoria, src.Servicio, src.HorasEstimadas, src.PrecioReferencia, src.FactorEscala, src.LeadTime);

-- Insumos reclutamiento/obsequios (NSE base)
IF NOT EXISTS (SELECT 1 FROM dbo.eq_cost_insumos)
BEGIN
    INSERT dbo.eq_cost_insumos (NSE, Tipo, ValorUnitario) VALUES
    ('NSE5_6','Reclutamiento',30000),
    ('NSE4','Reclutamiento',25000),
    ('NSE3','Reclutamiento',20000),
    ('NSE1_2','Reclutamiento',20000),
    ('NSE5_6','Obsequio',30000),
    ('NSE4','Obsequio',30000),
    ('NSE3','Obsequio',25000),
    ('NSE1_2','Obsequio',25000);
END;

-- Insumos para pruebas (hoja Valores Insumos reclutamiento)
MERGE dbo.eq_insumos_prueba AS target
USING (VALUES
    ('INS_AGUA','Agua Prueba',725.88,'Unid',806),
    ('INS_PLATOS','Platos',4523.60,'Unid',161.2),
    ('INS_GALLETAS','Galletas soda',6522.40,'Unid',40.3),
    ('INS_VASOS_AGUA','Vasos agua',3682.00,'Unid',32.24),
    ('INS_VASOS_MURANO','Vasos Murano',5470.40,'Unid',67.1667),
    ('INS_SERVILLETAS','Servilletas',5260.00,'Unid',4.03),
    ('INS_BOLSAS','Bolsas basura',5260.00,'Unid',1),
    ('INS_GUANTES','Guantes nitrilo',38924.00,'Unid',3),
    ('INS_COFIAS','Cofias',28404.00,'Unid',2),
    ('INS_DELANTALES','Delantales desechables',1683.20,'Unid',100.75)
) AS src(Codigo, Descripcion, Precio, Unidad, Cantidad)
ON target.Codigo = src.Codigo
WHEN MATCHED THEN UPDATE SET Descripcion = src.Descripcion, Precio = src.Precio, Unidad = src.Unidad, Cantidad = src.Cantidad
WHEN NOT MATCHED THEN INSERT (Codigo, Descripcion, Precio, Unidad, Cantidad) VALUES (src.Codigo, src.Descripcion, src.Precio, src.Unidad, src.Cantidad);

-- Costos unitarios operativos (hoja Valores Insumos reclutamiento sección costo unitario)
MERGE dbo.eq_cost_unitario_ops AS target
USING (VALUES
    (1300,'Transportes PST Encuestadores',3260,'Dias',13),
    (1301,'Transportes PST Supervisores',6522,'Dias',3),
    (4428,'Duplicar cuestionarios e instructivos',4737.3125,'Paginas',16),
    (5400,'Realización encuestas personal',10465.3,'Dias',13),
    (5401,'Supervisión de campo personal',102036.3333,'Dias',3),
    (5402,'Campo Coordinación',134681,'Dias',1),
    (5403,'Verificacion Campo',28683.625,'Horas',8),
    (5409,'Entrenamiento a equipos de campo',26526.6667,'Horas',3),
    (5430,'Reclutamiento campo presencial',15000,'Reclutados',100),
    (5431,'Productos para pruebas',4476,'Productos',100),
    (5433,'Apoyo logistico localizacion central',81000,'Dias',2),
    (5472,'Verificacion-Campo Costo telefónico',93.6061,'Minutos',165),
    (5504,'Crítica Encuestas',1360.6,'Horas',5),
    (5506,'Codificación',25740,'Horas',31),
    (5601,'Scripting',54235,'Horas',41),
    (5701,'Datacleaning',54235,'Horas',18),
    (5705,'TopLines',54235,'Horas',18),
    (5710,'Procesamiento',54235,'Horas',18),
    (5720,'Conversión/generación archivos ASCII y otros',39251,'Horas',9)
) AS src(CodMatrix, Actividad, Tarifa, Unidad, Horas)
ON target.CodMatrix = src.CodMatrix
WHEN MATCHED THEN UPDATE SET Actividad = src.Actividad, Tarifa = src.Tarifa, Unidad = src.Unidad, Horas = src.Horas
WHEN NOT MATCHED THEN INSERT (CodMatrix, Actividad, Tarifa, Unidad, Horas) VALUES (src.CodMatrix, src.Actividad, src.Tarifa, src.Unidad, src.Horas);

-- Locaciones (Valores Insumos reclutamiento)
IF NOT EXISTS (SELECT 1 FROM dbo.eq_locaciones)
BEGIN
    INSERT dbo.eq_locaciones (Ciudad, TarifaBase, TarifaConGross, DiasBase) VALUES
    ('Bogota',650000,683800,14.39),
    ('Medellin',756000,795312,14.39),
    ('Barranquilla',860000,904720,14.39),
    ('Cali',600000,683800,14.39),
    ('Bucaramanga',0,0,14.39),
    ('Cartagena',0,0,14.39),
    ('Otra1',0,0,14.39),
    ('Otra2',0,0,14.39),
    ('Otra3',0,0,14.39);
END;

-- Envios (Valor por peso)
IF NOT EXISTS (SELECT 1 FROM dbo.eq_envio_tarifa)
BEGIN
    INSERT dbo.eq_envio_tarifa (Tipologia, KiloInicial, KiloAdicional, SeguroPct, ValorDeclaradoMin) VALUES
    ('URBANO',8700,2000,0.01,25000),
    ('NACIONAL',14000,3000,0.01,25000),
    ('REEXPEDICION',17000,4500,0.01,25000);
END;

-- Codificacion (hoja Codificación)
IF NOT EXISTS (SELECT 1 FROM dbo.eq_codificacion_param)
BEGIN
    INSERT dbo.eq_codificacion_param (Escenario, Registros, PregAbiertas, PregAbiertasMult, Dias, Horas, ValorIpsos) VALUES
    ('Codif Normal',976,6,6,18.50,166.50,2764899),
    ('Codif Pruebas Prod',480,2,2,7.67,69.00,16606);
END;

-- Mystery
IF NOT EXISTS (SELECT 1 FROM dbo.eq_tarifa_mystery)
BEGIN
    INSERT dbo.eq_tarifa_mystery (TipoVisita, Complejidad, VrUnitario, OlasDefault) VALUES
    ('TIPO VISITA 1','Basica - Video',55000,1),
    ('TIPO VISITA 2','Ninguna',0,1),
    ('TIPO VISITA 3','Ninguna',0,1);
END;

-- Mystery costos adicionales (hoja MYSTERY)
MERGE dbo.eq_cost_unitario_ops AS target
USING (VALUES
    (5418,'Mystery Shopper TIPO VISITA 1',55000,'Visita',NULL),
    (5418,'Mystery Shopper TIPO VISITA 2',0,'Visita',NULL),
    (5418,'Mystery Shopper TIPO VISITA 3',0,'Visita',NULL),
    (5420,'Coord Campo - Capacitaciones / entrega (PMO)',69235.0083,'Horas',4),
    (5420,'Asist. Campo - Capacitaciones / entrega (Coord campo)',69235.0083,'Horas',4),
    (5420,'Coord Campo (PMO)',69235.0083,'Horas',36),
    (5420,'Asist. Campo (Coord. Campo)',69235.0083,'Horas',36),
    (200,'Apoyo otra asistencia - Unificación videos',69235.0083,'Horas',10),
    (200,'Apoyo otra asistencia - RMC',69235.0083,'Horas',10),
    (5504,'Crítica Encuestas Mystery',111921.0399,'Horas',1.1111),
    (9997,'Desplazamientos (viaticos) Mystery',1,'Unidad',NULL),
    (6305,'Costo Telefonico Obtención citas',30000,'Unidad',1),
    (9997,'Fotocopias Mystery',1157.2,'Unidad',1),
    (8005,'Edición videos - Apoyo Logístico',23144,'Unidad',NULL),
    (454,'Alquiler equipos/recargas alertas',18936,'Unidad',1),
    (9997,'Otros costos Mystery',1,'Unidad',NULL),
    (654,'Plan vimeo',0,'Unidad',1),
    (500,'Bonos Mystery visita 1',35000,'Unidad',NULL),
    (500,'Bonos Mystery visita 2',0,'Unidad',NULL),
    (500,'Bonos Mystery visita 3',0,'Unidad',NULL),
    (500,'Bonos Mystery comisión',0.1,'Porcentaje',NULL),
    (500,'Otras compras Mystery',1,'Unidad',NULL),
    (500,'Compra de dispositivos de grabación',300000,'Unidad',2)
) AS src(CodMatrix, Actividad, Tarifa, Unidad, Horas)
ON target.CodMatrix = src.CodMatrix AND target.Actividad = src.Actividad
WHEN MATCHED THEN UPDATE SET Tarifa = src.Tarifa, Unidad = src.Unidad, Horas = src.Horas
WHEN NOT MATCHED THEN INSERT (CodMatrix, Actividad, Tarifa, Unidad, Horas) VALUES (src.CodMatrix, src.Actividad, src.Tarifa, src.Unidad, src.Horas);

-- Productividad por ciudad (semilla editable)
MERGE dbo.eq_productividad_ciudad AS target
USING (VALUES
    ('Bogota',7,4),
    ('B/quilla',6,5),
    ('M/llin',7,5),
    ('Cali',7,5),
    ('B/manga',7,5),
    ('C/gena',7,5),
    ('Otras Ciudades',6,5)
) AS src(Ciudad, Encuestadores, Productividad)
ON target.Ciudad = src.Ciudad
WHEN MATCHED THEN UPDATE SET Encuestadores = src.Encuestadores, Productividad = src.Productividad
WHEN NOT MATCHED THEN INSERT (Ciudad, Encuestadores, Productividad) VALUES (src.Ciudad, src.Encuestadores, src.Productividad);

-- Parametros misc (ajustables)
MERGE dbo.eq_param_misc AS target
USING (VALUES
    ('FACTOR_REFRIGERACION',1.15,NULL),
    ('COSTO_NEVERA',970000,NULL),
    ('DIVISOR_VOLUMETRICO',5000,NULL)
) AS src(Clave, ValorDecimal, ValorTexto)
ON target.Clave = src.Clave
WHEN MATCHED THEN UPDATE SET ValorDecimal = src.ValorDecimal, ValorTexto = src.ValorTexto
WHEN NOT MATCHED THEN INSERT (Clave, ValorDecimal, ValorTexto) VALUES (src.Clave, src.ValorDecimal, src.ValorTexto);

-- Parametros de envio (default)
IF NOT EXISTS (SELECT 1 FROM dbo.eq_envio_param)
BEGIN
    INSERT dbo.eq_envio_param (DivisorVolumetrico, TipologiaUrbano, TipologiaNacional)
    VALUES (5000,'URBANO','NACIONAL');
END;

-- Costos base de datos (placeholder ajustable)
MERGE dbo.eq_cost_base_datos AS target
USING (VALUES
    ('No requiere',0),
    ('Cliente',0),
    ('Comprar',500000)
) AS src(Tipo, Valor)
ON target.Tipo = src.Tipo
WHEN MATCHED THEN UPDATE SET Valor = src.Valor
WHEN NOT MATCHED THEN INSERT (Tipo, Valor) VALUES (src.Tipo, src.Valor);

-- Factores (script tipo, clase prueba, apoyo reclutamiento, etiquetado, probabilidad aprobacion)
MERGE dbo.eq_param_factores AS target
USING (VALUES
    ('SCRIPT_TIPO','Nuevo','Script nuevo desarrollo',1.0,1,1),
    ('SCRIPT_TIPO','Duplicado','Script duplicado/copia',4.0,2,1),
    ('SCRIPT_TIPO','Reutilizacion','Script reutilización',2.0,3,1),
    ('CLASE_PRUEBA','No aplica','Sin prueba',0.0,1,1),
    ('CLASE_PRUEBA','Monodica','Prueba monódica',1.0,2,1),
    ('CLASE_PRUEBA','Secuencial','Prueba secuencial',1.2,3,1),
    ('CLASE_PRUEBA','Comparativa','Prueba comparativa',1.5,4,1),
    ('APOYO_RECLUTAMIENTO','Sin apoyo','Sin apoyo',0.0,1,1),
    ('APOYO_RECLUTAMIENTO','Telefonica','Apoyo telefónico',5000.0,2,1),
    ('APOYO_RECLUTAMIENTO','Presencial','Apoyo presencial',8000.0,3,1),
    ('APOYO_RECLUTAMIENTO','Mixta','Apoyo mixto',6500.0,4,1),
    ('ETIQUETADO','No','Sin etiquetado',0.0,1,1),
    ('ETIQUETADO','Simple','Etiquetado simple',50.0,2,1),
    ('ETIQUETADO','Blind','Blind/Ciego',100.0,3,1),
    ('ETIQUETADO','Rotulado','Rotulado completo',150.0,4,1),
    ('PROB_APROBACION','Alta','Probabilidad alta',1.0,1,1),
    ('PROB_APROBACION','Media','Probabilidad media',0.7,2,1),
    ('PROB_APROBACION','Baja','Probabilidad baja',0.5,3,1)
) AS src(Tipo, Codigo, Descripcion, Factor, Orden, Activo)
ON target.Tipo = src.Tipo AND target.Codigo = src.Codigo
WHEN MATCHED THEN UPDATE SET Descripcion = src.Descripcion, Factor = src.Factor, Orden = src.Orden, Activo = src.Activo
WHEN NOT MATCHED THEN INSERT (Tipo, Codigo, Descripcion, Factor, Orden, Activo)
VALUES (src.Tipo, src.Codigo, src.Descripcion, src.Factor, src.Orden, src.Activo);

-- Rate horas SL (ejemplos con KEY format: SL|RecordDetail|MetodologiaSL)
-- Basado en hoja Horas del Excel con formato A67: =B67&" | "&C67&" | "&D67
MERGE dbo.eq_rate_horas AS target
USING (VALUES
    ('Cuanti | Descriptivo | F2F','Cuanti','Descriptivo','F2F',8,10,12,14,16,186189,186189,186189,43293,38855,186189,186189,186189,43293,38855),
    ('Cuanti | Descriptivo | CATI','Cuanti','Descriptivo','CATI',6,8,10,12,14,186189,186189,186189,43293,38855,186189,186189,186189,43293,38855),
    ('Cuanti | Descriptivo | Online','Cuanti','Descriptivo','Online',4,6,8,10,12,186189,186189,186189,43293,38855,186189,186189,186189,43293,38855),
    ('Cuanti | Ad-hoc | F2F','Cuanti','Ad-hoc','F2F',10,12,14,16,18,186189,186189,186189,43293,38855,186189,186189,186189,43293,38855),
    ('Cuanti | Ad-hoc | CATI','Cuanti','Ad-hoc','CATI',8,10,12,14,16,186189,186189,186189,43293,38855,186189,186189,186189,43293,38855),
    ('Cuanti | Ad-hoc | Online','Cuanti','Ad-hoc','Online',6,8,10,12,14,186189,186189,186189,43293,38855,186189,186189,186189,43293,38855),
    ('Cuanti | Tracker | F2F','Cuanti','Tracker','F2F',6,8,10,12,14,186189,186189,186189,43293,38855,186189,186189,186189,43293,38855),
    ('Cuanti | Tracker | CATI','Cuanti','Tracker','CATI',4,6,8,10,12,186189,186189,186189,43293,38855,186189,186189,186189,43293,38855),
    ('Cuanti | Tracker | Online','Cuanti','Tracker','Online',3,5,7,9,11,186189,186189,186189,43293,38855,186189,186189,186189,43293,38855),
    ('Cuali | Focus Group | F2F','Cuali','Focus Group','F2F',12,14,16,18,20,186189,186189,186189,43293,38855,186189,186189,186189,43293,38855),
    ('Cuali | Entrevista profundidad | F2F','Cuali','Entrevista profundidad','F2F',14,16,18,20,22,186189,186189,186189,43293,38855,186189,186189,186189,43293,38855),
    ('Mystery | Shopper | Presencial','Mystery','Shopper','Presencial',8,10,12,14,16,186189,186189,186189,43293,38855,186189,186189,186189,43293,38855)
) AS src([Key], SL, RecordDetail, MetodologiaSL, HorasL3, HorasL4, HorasL5, HorasL6, HorasL7, 
         LoadedRateL3, LoadedRateL4, LoadedRateL5, LoadedRateL6, LoadedRateL7,
         BillingRateL3, BillingRateL4, BillingRateL5, BillingRateL6, BillingRateL7)
ON target.SL = src.SL AND target.RecordDetail = src.RecordDetail AND target.MetodologiaSL = src.MetodologiaSL
WHEN MATCHED THEN UPDATE SET 
    HorasL3 = src.HorasL3, HorasL4 = src.HorasL4, HorasL5 = src.HorasL5, HorasL6 = src.HorasL6, HorasL7 = src.HorasL7,
    LoadedRateL3 = src.LoadedRateL3, LoadedRateL4 = src.LoadedRateL4, LoadedRateL5 = src.LoadedRateL5, 
    LoadedRateL6 = src.LoadedRateL6, LoadedRateL7 = src.LoadedRateL7,
    BillingRateL3 = src.BillingRateL3, BillingRateL4 = src.BillingRateL4, BillingRateL5 = src.BillingRateL5,
    BillingRateL6 = src.BillingRateL6, BillingRateL7 = src.BillingRateL7
WHEN NOT MATCHED THEN INSERT (SL, RecordDetail, MetodologiaSL, HorasL3, HorasL4, HorasL5, HorasL6, HorasL7,
    LoadedRateL3, LoadedRateL4, LoadedRateL5, LoadedRateL6, LoadedRateL7,
    BillingRateL3, BillingRateL4, BillingRateL5, BillingRateL6, BillingRateL7)
VALUES (src.SL, src.RecordDetail, src.MetodologiaSL, src.HorasL3, src.HorasL4, src.HorasL5, src.HorasL6, src.HorasL7,
    src.LoadedRateL3, src.LoadedRateL4, src.LoadedRateL5, src.LoadedRateL6, src.LoadedRateL7,
    src.BillingRateL3, src.BillingRateL4, src.BillingRateL5, src.BillingRateL6, src.BillingRateL7);

-- Matriz CATI extendida (Parametros B80:AI88 del Excel)
-- Basada en fórmula D68: VLOOKUP(D18,Parametros!$B$80:$AI$88,HLOOKUP(D62,Parametros!B1:AI2,2,0),0)*F14
MERGE dbo.eq_param_cati AS target
USING (VALUES
    (5,'MAS82',7493,13894,21387),
    (5,'75_82',8019,13894,21913),
    (5,'67_74',8545,13894,22439),
    (5,'55_66',8966,13999,22965),
    (5,'46_54',9849,14168,24017),
    (5,'37_45',10757,14838,25595),
    (5,'30_36',13287,15990,29277),
    (5,'22_29',16443,15990,32433),
    (5,'15_21',19073,15990,35063),
    (5,'10_14',21703,15990,37693),
    (5,'0_9',24333,15990,40323),
    (10,'MAS82',8545,13894,22439),
    (10,'75_82',9071,13894,22965),
    (10,'67_74',9597,13894,23491),
    (10,'55_66',10018,13999,24017),
    (10,'46_54',10901,14168,25069),
    (10,'37_45',11809,14838,26647),
    (10,'30_36',14339,15990,30329),
    (10,'22_29',18547,15990,34537),
    (10,'15_21',21177,15990,37167),
    (10,'10_14',23807,15990,39797),
    (10,'0_9',26437,15990,42427),
    (15,'MAS82',10123,13894,24017),
    (15,'75_82',10649,13894,24543),
    (15,'67_74',11175,13894,25069),
    (15,'55_66',11596,13999,25595),
    (15,'46_54',12479,14168,26647),
    (15,'37_45',13387,14838,28225),
    (15,'30_36',15917,15990,31907),
    (15,'22_29',20651,15990,36641),
    (15,'15_21',23281,15990,39271),
    (15,'10_14',25911,15990,41901),
    (15,'0_9',28541,15990,44531),
    (20,'MAS82',12227,13894,26121),
    (20,'75_82',12753,13894,26647),
    (20,'67_74',13279,13894,27173),
    (20,'55_66',13700,13999,27699),
    (20,'46_54',14583,14168,28751),
    (20,'37_45',15491,14838,30329),
    (20,'30_36',18021,15990,34011),
    (20,'22_29',22755,15990,38745),
    (20,'15_21',25385,15990,41375),
    (20,'10_14',28015,15990,44005),
    (20,'0_9',30645,15990,46635),
    (30,'MAS82',14857,13894,28751),
    (30,'75_82',15383,13894,29277),
    (30,'67_74',15909,13894,29803),
    (30,'55_66',16330,13999,30329),
    (30,'46_54',17213,14168,31381),
    (30,'37_45',18121,14838,32959),
    (30,'30_36',20651,15990,36641),
    (30,'22_29',24859,15990,40849),
    (30,'15_21',27489,15990,43479),
    (30,'10_14',30119,15990,46109),
    (30,'0_9',32749,15990,48739),
    (40,'MAS82',18013,13894,31907),
    (40,'75_82',18539,13894,32433),
    (40,'67_74',19065,13894,32959),
    (40,'55_66',19486,13999,33485),
    (40,'46_54',20369,14168,34537),
    (40,'37_45',21277,14838,36115),
    (40,'30_36',23807,15990,39797),
    (40,'22_29',26963,15990,42953),
    (40,'15_21',29593,15990,45583),
    (40,'10_14',32223,15990,48213),
    (40,'0_9',34853,15990,50843),
    (50,'MAS82',21169,13894,35063),
    (50,'75_82',21695,13894,35589),
    (50,'67_74',22221,13894,36115),
    (50,'55_66',22642,13999,36641),
    (50,'46_54',23525,14168,37693),
    (50,'37_45',24433,14838,39271),
    (50,'30_36',26963,15990,42953),
    (50,'22_29',31171,15990,47161),
    (50,'15_21',33801,15990,49791),
    (50,'10_14',36431,15990,52421),
    (50,'0_9',39061,15990,55051),
    (60,'MAS82',26429,13894,40323),
    (60,'75_82',26955,13894,40849),
    (60,'67_74',27481,13894,41375),
    (60,'55_66',27902,13999,41901),
    (60,'46_54',28785,14168,42953),
    (60,'37_45',29693,14838,44531),
    (60,'30_36',32223,15990,48213),
    (60,'22_29',35379,15990,51369),
    (60,'15_21',38009,15990,53999),
    (60,'10_14',40639,15990,56629),
    (60,'0_9',43269,15990,59259)
) AS src(DuracionMin, PenetracionCodigo, ValorPerfil, ValorCoordinacion, ValorTotal)
ON target.DuracionMin = src.DuracionMin AND target.PenetracionCodigo = src.PenetracionCodigo
WHEN MATCHED THEN UPDATE SET ValorPerfil = src.ValorPerfil, ValorCoordinacion = src.ValorCoordinacion, ValorTotal = src.ValorTotal
WHEN NOT MATCHED THEN INSERT (DuracionMin, PenetracionCodigo, ValorPerfil, ValorCoordinacion, ValorTotal)
VALUES (src.DuracionMin, src.PenetracionCodigo, src.ValorPerfil, src.ValorCoordinacion, src.ValorTotal);

-- Matriz Online/AUTO extendida (Parametros B96:AI104 del Excel)
-- Basada en fórmula D71: VLOOKUP(D18,Parametros!$B$96:$AI$104,HLOOKUP(D62,Parametros!B94:AI95,2,0),0)*F14
MERGE dbo.eq_param_online AS target
USING (VALUES
    (5,'MAS82',7493,13894,21387),
    (5,'75_82',8019,13894,21913),
    (5,'67_74',8545,13894,22439),
    (5,'55_66',8966,13999,22965),
    (5,'46_54',9849,14168,24017),
    (5,'37_45',10757,14838,25595),
    (5,'30_36',13287,15990,29277),
    (5,'22_29',16443,15990,32433),
    (5,'15_21',19073,15990,35063),
    (5,'10_14',21703,15990,37693),
    (5,'0_9',24333,15990,40323),
    (10,'MAS82',8545,13894,22439),
    (10,'75_82',9071,13894,22965),
    (10,'67_74',9597,13894,23491),
    (10,'55_66',10018,13999,24017),
    (10,'46_54',10901,14168,25069),
    (10,'37_45',11809,14838,26647),
    (10,'30_36',14339,15990,30329),
    (10,'22_29',18547,15990,34537),
    (10,'15_21',21177,15990,37167),
    (10,'10_14',23807,15990,39797),
    (10,'0_9',26437,15990,42427),
    (15,'MAS82',10123,13894,24017),
    (15,'75_82',10649,13894,24543),
    (15,'67_74',11175,13894,25069),
    (15,'55_66',11596,13999,25595),
    (15,'46_54',12479,14168,26647),
    (15,'37_45',13387,14838,28225),
    (15,'30_36',15917,15990,31907),
    (15,'22_29',20651,15990,36641),
    (15,'15_21',23281,15990,39271),
    (15,'10_14',25911,15990,41901),
    (15,'0_9',28541,15990,44531),
    (20,'MAS82',12227,13894,26121),
    (20,'75_82',12753,13894,26647),
    (20,'67_74',13279,13894,27173),
    (20,'55_66',13700,13999,27699),
    (20,'46_54',14583,14168,28751),
    (20,'37_45',15491,14838,30329),
    (20,'30_36',18021,15990,34011),
    (20,'22_29',22755,15990,38745),
    (20,'15_21',25385,15990,41375),
    (20,'10_14',28015,15990,44005),
    (20,'0_9',30645,15990,46635),
    (30,'MAS82',14857,13894,28751),
    (30,'75_82',15383,13894,29277),
    (30,'67_74',15909,13894,29803),
    (30,'55_66',16330,13999,30329),
    (30,'46_54',17213,14168,31381),
    (30,'37_45',18121,14838,32959),
    (30,'30_36',20651,15990,36641),
    (30,'22_29',24859,15990,40849),
    (30,'15_21',27489,15990,43479),
    (30,'10_14',30119,15990,46109),
    (30,'0_9',32749,15990,48739),
    (40,'MAS82',18013,13894,31907),
    (40,'75_82',18539,13894,32433),
    (40,'67_74',19065,13894,32959),
    (40,'55_66',19486,13999,33485),
    (40,'46_54',20369,14168,34537),
    (40,'37_45',21277,14838,36115),
    (40,'30_36',23807,15990,39797),
    (40,'22_29',26963,15990,42953),
    (40,'15_21',29593,15990,45583),
    (40,'10_14',32223,15990,48213),
    (40,'0_9',34853,15990,50843),
    (50,'MAS82',21169,13894,35063),
    (50,'75_82',21695,13894,35589),
    (50,'67_74',22221,13894,36115),
    (50,'55_66',22642,13999,36641),
    (50,'46_54',23525,14168,37693),
    (50,'37_45',24433,14838,39271),
    (50,'30_36',26963,15990,42953),
    (50,'22_29',31171,15990,47161),
    (50,'15_21',33801,15990,49791),
    (50,'10_14',36431,15990,52421),
    (50,'0_9',39061,15990,55051),
    (60,'MAS82',26429,13894,40323),
    (60,'75_82',26955,13894,40849),
    (60,'67_74',27481,13894,41375),
    (60,'55_66',27902,13999,41901),
    (60,'46_54',28785,14168,42953),
    (60,'37_45',29693,14838,44531),
    (60,'30_36',32223,15990,48213),
    (60,'22_29',35379,15990,51369),
    (60,'15_21',38009,15990,53999),
    (60,'10_14',40639,15990,56629),
    (60,'0_9',43269,15990,59259)
) AS src(DuracionMin, PenetracionCodigo, ValorPerfil, ValorCoordinacion, ValorTotal)
ON target.DuracionMin = src.DuracionMin AND target.PenetracionCodigo = src.PenetracionCodigo
WHEN MATCHED THEN UPDATE SET ValorPerfil = src.ValorPerfil, ValorCoordinacion = src.ValorCoordinacion, ValorTotal = src.ValorTotal
WHEN NOT MATCHED THEN INSERT (DuracionMin, PenetracionCodigo, ValorPerfil, ValorCoordinacion, ValorTotal)
VALUES (src.DuracionMin, src.PenetracionCodigo, src.ValorPerfil, src.ValorCoordinacion, src.ValorTotal);

PRINT 'EQ schema y seeds creados/actualizados correctamente (incluye CATI, Online, Factores, Rate Horas).';

