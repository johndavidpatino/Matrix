# PLAN SPRINT PRE-1: CC_FinzOpe → MatrixNext

**Objetivo**: Migrar infraestructura de CC_FinzOpe (tablas base y SP críticos) para soportar FI_Administrativo
**Duración**: 2 semanas (80 horas estimadas)
**Dependencia**: Base de datos para que FI Sprints 1-6 funcionen correctamente
**Responsable**: Equipo de migración
**Estado**: 📋 Documentación en progreso

---

## 1) Propósito de CC_FinzOpe en FI

CC_FinzOpe es el **módulo de operaciones y finanzas** que centraliza:
- Producción (trabajo realizado)
- Conteos (registros procesados)
- Liquidación (pago de salarios, bonificación)
- Reportes financieros
- Descuentos y deducciones

**No es un módulo de usuario independiente**, sino la **infraestructura de datos** que FI consume vía SP.

### ¿Por qué migrar primero?
- 🔴 FI Sprints 1-6 dependen de 15+ SP de CC_FinzOpe
- 🔴 SP contienen lógica de negocio crítica (cálculos, validaciones)
- 🔴 Tablas base ya tienen datos históricos
- ✅ Si CC está listo en Core8, FI implementa sin retrasos

---

## 2) Análisis Actual de CC_FinzOpe en WebMatrix

### 2.1 Ubicación y Contexto

```
WebMatrix/
├── CoreProject/
│   ├── CC_FinzOpe.Context.tt    ← EF6 Template
│   ├── CC_FinzOpe.Context.vb    ← DbContext (VB.NET)
│   ├── CC_FinzOpe.Designer.vb   ← Modelos generados
│   ├── CC_FinzOpe.edmx          ← Esquema visual
│   ├── CC_FinzOpe.tt            ← Template generador
│   ├── CC_FinzOpe.vb            ← Entidades base
│   └── CC_FinzOpe1.Designer.vb  ← Variante designer
└── Otros archivos de datos y adaptadores
```

**Contexto EF6**: CC_FinzOpe (VB.NET, EF6, probablemente Database-First desde SQL Server)

### 2.2 Tablas Identificadas (Estimadas)

| Tabla | Prefijo | Descripción | Registros Estimados | Crítica |
|-------|---------|-------------|---------------------|---------|
| CC_Consecutivos | CC_ | Control de numeración | 100s | 🔴 SÍ |
| CC_Trabajos | CC_ | Jobbooks/trabajos | 1000s | 🔴 SÍ |
| CC_Produccion | CC_ | Registros de producción | 100k+ | 🔴 SÍ |
| CC_Conteos | CC_ | Conteos de trabajo | 10k+ | 🟠 Medio |
| CC_Descuentos | CC_ | Descuentos aplicados | 1k | 🟠 Medio |
| CC_ActividadesProduccion | CC_ | Catálogo actividades | 100s | 🟡 Bajo |
| CC_PrestacionServicios | CC_ | PST registrada | 10k+ | 🔴 SÍ |
| CC_DetallePresupuesto | CC_ | Detalles presupuestos | 10k+ | 🟠 Medio |
| CC_Bonificaciones | CC_ | Cálculos bonificación | 1k+ | 🔴 SÍ |
| CC_EstadosJobBooks | CC_ | Estados de trabajos | Actualizado | 🟠 Medio |
| ... | ... | (Más tablas según análisis) | ... | ... |

**Notas**:
- Estimaciones basadas en volumen típico de operaciones
- Tablas principales (Produccion, PrestacionServicios) pueden tener 100k+ registros
- Algunos prefijos pueden ser CC_*, TH_*, FI_* según contexto

### 2.3 Stored Procedures Críticos Mapeados

| SP | Descripción | Usado por (FI) | Parámetros | Complejidad |
|----|----|-------|-----------|-------------|
| CC_PRODUCCION_GET | Obtener producción por filtro | Grupo 5 (Produccion.aspx) | @IdTrabajo, @FechaInicio, @FechaFin | 🟠 Media |
| CC_Produccion.RegistrosProduccion | CRUD producción | Grupo 5 | @Accion, @IdProduccion, ... | 🔴 Alta |
| CC_LiquidarPlanillas | Calcular liquidación planillas | Grupo 5 (LiquidarPlanillas) | @IdPeriodo, @IdTrabajo, ... | 🔴 Alta |
| CC_GenerarBonificacion | Calcular bonificación | Grupo 5 (GenerarBonificacion) | @IdPeriodo, ... | 🔴 Alta |
| CC_CargueDescuentosSS | Cargar descuentos SS | Grupo 5 (CargueDescuentosSS) | @IdPeriodo, ... | 🟠 Media |
| CC_LiquidarProductividadPST | Liquidar PST productividad | Grupo 5 (LiquidarProductividad) | @IdPeriodo, ... | 🔴 Alta |
| CC_ListadoTrabajos | Obtener trabajos disponibles | Grupo 2 (ListadoTrabajos) | @FechaInicio, @FechaFin | 🟡 Baja |
| CC_ReportePagos | Reporte de pagos | Grupo 4 (ReportePagos) | @FechaInicio, @FechaFin | 🟠 Media |
| CC_ReporteActividadesProduccion | Reporte actividades | Grupo 4 (ReporteActividadesProduccion) | @FechaInicio, @FechaFin | 🟠 Media |
| CC_ReporteContabilizacionPST | Reporte contabilización | Grupo 4 (ReporteContabilizacionPST) | @Periodo | 🟠 Media |
| CC_EstadoJobBooks | Cambiar estado jobbook | Grupo 5 (EstadoJobBooks) | @IdJobBook, @EstadoNuevo | 🟡 Baja |
| CC_ConteosXIdGet | Obtener conteos por ID | Grupo 3 (ConteoTrabajos) | @IdConteo | 🟡 Baja |
| CC_ReporteConteoTrabajos | Reporte conteos | Grupo 3 (ReporteConteoTrabajos) | @FechaInicio, @FechaFin | 🟡 Baja |
| CC_ResumenesdeProduccion | Resumen productividad | Grupo 3 (ResumenProductividad) | @FechaInicio, @FechaFin | 🟠 Media |
| CC_GenerarRequerimientos | Generar requerimientos | Grupo 2 (GenerarRequerimientos) | @IdTrabajo, @Descripcion | 🟠 Media |
| ... | ... | ... | ... | ... |

**Total estimado**: 20-30 SP (algunos combinados en 1 SP con @Accion)

### 2.4 Dependencias de SP (Ejemplo: Liquidación)

```
LiquidarPlanillasActividades.aspx (Grupo 5)
    ↓
Controller: LiquidarPlanillasController.DeterminarPrecios()
    ↓
Service: LiquidacionService.CalcularPlanilla()
    ↓
SP: CC_LiquidarPlanillas
    ├─ Valida producción existe
    ├─ Consulta tarifas de CC_DetallePresupuesto
    ├─ Consulta descuentos de CC_Descuentos
    ├─ Calcula: cantidad × tarifa - descuentos
    ├─ Inserta en CC_Liquidaciones
    └─ Retorna: monto total, cantidad registros

Impacto si falla:
🔴 Empleados no cobran
🔴 Reportes de pagos incorrectos
🔴 Auditoría financiera falla
```

---

## 3) Estrategia de Migración CC_FinzOpe

### 3.1 Fases de Migración

```
FASE 1: ANÁLISIS & VALIDACIÓN (2-3 días)
├─ Mapear todas las tablas en SQL Server
├─ Documentar todas las columnas y tipos
├─ Validar relaciones (FK, índices)
├─ Identificar vistas o triggers
└─ Documento: ANALISIS_CC_FINZOPE_TABLAS.md

FASE 2: EF CORE MIGRATION (3-4 días)
├─ Crear DbContext en Core8
├─ Scaffold models desde SQL Server
├─ Configurar relaciones en OnModelCreating
├─ Mapear propiedades (naming, tipos)
└─ Validar que modelos compilen

FASE 3: ADAPTER LAYER (2-3 días)
├─ Crear CCFinzOpeDataAdapter
├─ Implementar métodos para lectura de SP
├─ Usar Dapper para SP complejas
├─ Mapear resultados a DTOs
└─ Testing de queries

FASE 4: SERVICE LAYER (1-2 días)
├─ Crear CCFinzOpeService
├─ Exponer métodos públicos
├─ Agregar logging
├─ Validaciones de negocio
└─ DI registration en Program.cs

FASE 5: TESTING & VALIDATION (1-2 días)
├─ Validar SP se ejecutan correctamente
├─ Verificar índices en Core8
├─ Testing de performance
├─ Documentar resultados
└─ Checklist pre-Sprint 1

TOTAL: ~80 horas (2 semanas)
```

### 3.2 Herramientas y Patrones

**Para Lectura de SP complejas**:
- Usar `Dapper` (ya instalado en proyecto)
- Crear DTOs por SP (ej: ProduccionResultDTO, LiquidacionResultDTO)
- Mapear resultados con `ToList()` o `FirstOrDefault()`

**Para EF Core DbContext**:
- Crear `CC_FinzOpeContext : DbContext`
- Usar `ScaffoldDbContext` desde SQL Server
- Configurar connection string en appsettings.json

**Para Adapter Pattern**:
```csharp
// Patrón estándar
public class CCFinzOpeDataAdapter
{
    private readonly string _connectionString;
    
    public CCFinzOpeDataAdapter(string connStr)
    {
        _connectionString = connStr;
    }
    
    // Lectura: EF Core
    public List<CC_Produccion> ObtenerProduccion(DateTime fInicio, DateTime fFin)
    {
        using var context = new CC_FinzOpeContext(_connectionString);
        return context.CC_Produccion
            .Where(p => p.Fecha >= fInicio && p.Fecha <= fFin)
            .ToList();
    }
    
    // SP compleja: Dapper
    public (bool Success, string Message, decimal Monto) LiquidarPlanilla(
        int idPeriodo, int idTrabajo)
    {
        using var conn = new SqlConnection(_connectionString);
        try
        {
            var result = conn.QueryFirstOrDefault<dynamic>(
                "CC_LiquidarPlanillas",
                new { idPeriodo, idTrabajo },
                commandType: CommandType.StoredProcedure
            );
            return (true, "OK", result.Monto);
        }
        catch (Exception ex)
        {
            return (false, ex.Message, 0);
        }
    }
}
```

---

## 4) Estructura Propuesta en MatrixNext

### 4.1 Carpetas y Archivos

```
MatrixNext.Web/
├── Areas/
│   ├── CC/                              ← Nueva área (infraestructura)
│   │   ├── Controllers/                 (vacío, solo para referencia)
│   │   └── Data/
│   │       ├── Models/
│   │       │   ├── CC_Produccion.cs
│   │       │   ├── CC_Conteos.cs
│   │       │   ├── CC_PrestacionServicios.cs
│   │       │   ├── CC_Descuentos.cs
│   │       │   ├── CC_ActividadesProduccion.cs
│   │       │   ├── CC_DetallePresupuesto.cs
│   │       │   ├── CC_EstadosJobBooks.cs
│   │       │   ├── CC_Bonificaciones.cs
│   │       │   ├── CC_Trabajos.cs
│   │       │   └── ... (otros modelos)
│   │       ├── Contexts/
│   │       │   └── CC_FinzOpeContext.cs   ← DbContext EF Core
│   │       ├── DTOs/
│   │       │   ├── ProduccionResultDTO.cs
│   │       │   ├── LiquidacionResultDTO.cs
│   │       │   ├── ReportePagosDTO.cs
│   │       │   ├── BonificacionResultDTO.cs
│   │       │   └── ... (DTOs por SP)
│   │       └── Adapters/
│   │           └── CCFinzOpeDataAdapter.cs
│   │
│   ├── FI/                              ← Área FI (Sprint 1+)
│   │   ├── Controllers/
│   │   ├── Services/
│   │   ├── Views/
│   │   └── Data/
│   │       └── Adapters/
│   │           └── FIDataAdapter.cs    ← USA CCFinzOpeDataAdapter
│   │
│   └── ...
│
├── Program.cs                          ← Registrar CC services en DI
└── appsettings.json                    ← Connection string CC_FinzOpe
```

### 4.2 Registración en Program.cs

```csharp
// Program.cs

var builder = WebApplicationBuilder.CreateBuilder(args);

// ========== CC_FinzOpe (Infraestructura) ==========
builder.Services.AddScoped<CCFinzOpeDataAdapter>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connString = config.GetConnectionString("CCFinzOpe");
    return new CCFinzOpeDataAdapter(connString);
});

// DbContext para EF Core (si se usa)
builder.Services.AddDbContext<CC_FinzOpeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CCFinzOpe"))
);

// ========== FI Services (dependen de CC) ==========
builder.Services.AddScoped<FIControlPresupuestosService>();
builder.Services.AddScoped<FIProduccionService>();
builder.Services.AddScoped<FILiquidacionService>();
// ... otros servicios FI

// ========== Controllers ==========
builder.Services.AddControllersWithViews();

// ========== Areas routing ==========
var app = builder.Build();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
```

### 4.3 Configuración appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Matrix;Integrated Security=true;",
    "CCFinzOpe": "Server=localhost;Database=Matrix;Integrated Security=true;",
    "LegacyWebMatrix": "Server=localhost;Database=WebMatrix;Integrated Security=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

---

## 5) Tareas Detalladas de Sprint Pre-1

### Tarea 1: Análisis de Tablas (8 horas)

**Objetivo**: Inventariar todas las tablas de CC_FinzOpe en SQL Server

**Actividades**:
```sql
-- 1. Listar todas las tablas
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME LIKE 'CC_%'
ORDER BY TABLE_NAME;

-- 2. Para cada tabla: Columnas, tipos, nullabilidad
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'CC_Produccion'
ORDER BY ORDINAL_POSITION;

-- 3. Relaciones (FK)
SELECT CONSTRAINT_NAME, TABLE_NAME, COLUMN_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME LIKE 'CC_%' AND REFERENCED_TABLE_NAME IS NOT NULL;

-- 4. Índices
SELECT OBJECT_NAME(i.object_id) AS TableName, i.name AS IndexName
FROM sys.indexes i
WHERE OBJECT_NAME(i.object_id) LIKE 'CC_%';
```

**Deliverable**: Documento `ANALISIS_CC_FINZOPE_TABLAS.md` (3-5 páginas)

### Tarea 2: Mapeo de SP (8 horas)

**Objetivo**: Documentar todos los SP usados por FI

**Checklist**:
- [ ] Listar todos los SP que comienzan con `CC_` o están en schema `CC`
- [ ] Para cada SP: nombre, descripción, parámetros, tipos retorno
- [ ] Documentar cuál FI Grupo/página usa cada SP
- [ ] Validar SP existen en SQL Server (no deprecated)
- [ ] Revisar si hay SP con lógica de negocio crítica

**Query SQL**:
```sql
SELECT ROUTINE_SCHEMA, ROUTINE_NAME, ROUTINE_TYPE
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_SCHEMA = 'dbo' AND ROUTINE_NAME LIKE 'CC_%'
ORDER BY ROUTINE_NAME;

-- Para cada SP: parámetros
EXEC sp_help 'CC_LiquidarPlanillas';
```

**Deliverable**: Documento `MAPEO_SP_CC_FINZOPE.md` (tablas de SP)

### Tarea 3: Crear DbContext EF Core (16 horas)

**Objetivo**: Migrar EF6 context a EF Core

**Pasos**:
1. Instalar paquetes NuGet (si faltan):
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   ```

2. Scaffold desde SQL Server:
   ```bash
   dotnet ef dbcontext scaffold "Server=localhost;Database=Matrix;Integrated Security=true;" `
     Microsoft.EntityFrameworkCore.SqlServer `
     --context CC_FinzOpeContext `
     --context-dir Data/Contexts `
     --models-dir Data/Models `
     --namespace MatrixNext.Web.Areas.CC.Data.Models `
     --force
   ```

3. Revisar modelos generados:
   - Verificar que todas las tablas CC_* estén incluidas
   - Validar relaciones (FK)
   - Revisar nullable properties

4. Configurar OnModelCreating si es necesario:
   ```csharp
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
       // Configuraciones específicas
       modelBuilder.Entity<CC_Produccion>()
           .HasKey(e => e.Id);
       
       modelBuilder.Entity<CC_Produccion>()
           .HasOne(e => e.CC_Trabajo)
           .WithMany(t => t.CC_Producciones)
           .HasForeignKey(e => e.IdTrabajo);
   }
   ```

5. Crear migration inicial:
   ```bash
   dotnet ef migrations add InitialCC_FinzOpe --context CC_FinzOpeContext
   ```

**Deliverable**: `Areas/CC/Data/Contexts/CC_FinzOpeContext.cs` + modelos

### Tarea 4: Crear DTOs por SP (12 horas)

**Objetivo**: Definir clases para resultados de SP

**Ejemplo: ProduccionResultDTO**:
```csharp
// Areas/CC/Data/DTOs/ProduccionResultDTO.cs

public class ProduccionResultDTO
{
    public long IdProduccion { get; set; }
    public long IdTrabajo { get; set; }
    public string CodigoTrabajo { get; set; }
    public long IdEmpleado { get; set; }
    public string NombreEmpleado { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Cantidad { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal Total { get; set; }
    public byte Estado { get; set; }
    public DateTime FechaRegistro { get; set; }
}

public class LiquidacionResultDTO
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public int RegistrosLiquidados { get; set; }
    public decimal MontoTotal { get; set; }
    public DateTime FechaLiquidacion { get; set; }
}

// ... más DTOs por cada SP
```

**Lista de DTOs necesarios**:
- `ProduccionResultDTO` (CC_PRODUCCION_GET)
- `LiquidacionResultDTO` (CC_LiquidarPlanillas)
- `BonificacionResultDTO` (CC_GenerarBonificacion)
- `DescuentosResultDTO` (CC_CargueDescuentosSS)
- `ReportePagosDTO` (CC_ReportePagos)
- `ReporteActividadesDTO` (CC_ReporteActividadesProduccion)
- `ReporteContabilizacionDTO` (CC_ReporteContabilizacionPST)
- `EstadoJobBookResultDTO` (CC_EstadoJobBooks)
- ... más según SP

**Deliverable**: 8-10 DTO classes en `Areas/CC/Data/DTOs/`

### Tarea 5: Crear Adapter (20 horas)

**Objetivo**: Implementar `CCFinzOpeDataAdapter` con métodos para todas las operaciones

**Estructura**:
```csharp
// Areas/CC/Data/Adapters/CCFinzOpeDataAdapter.cs

public class CCFinzOpeDataAdapter
{
    private readonly string _connectionString;
    private readonly CC_FinzOpeContext _efContext;
    private readonly ILogger<CCFinzOpeDataAdapter> _logger;
    
    public CCFinzOpeDataAdapter(string connStr, CC_FinzOpeContext efContext, 
        ILogger<CCFinzOpeDataAdapter> logger)
    {
        _connectionString = connStr;
        _efContext = efContext;
        _logger = logger;
    }
    
    // ===== LECTURA CON EF CORE =====
    
    public List<CC_Produccion> ObtenerProduccion(DateTime fInicio, DateTime fFin)
    {
        _logger.LogInformation($"Obtener producción {fInicio:yyyy-MM-dd} a {fFin:yyyy-MM-dd}");
        return _efContext.CC_Produccion
            .Where(p => p.Fecha >= fInicio && p.Fecha <= fFin)
            .ToList();
    }
    
    public List<CC_PrestacionServicios> ObtenerPST(long idContratista)
    {
        return _efContext.CC_PrestacionServicios
            .Where(p => p.IdContratista == idContratista)
            .ToList();
    }
    
    // ===== SP COMPLEJAS CON DAPPER =====
    
    public (bool Success, int RegistrosLiquidados, decimal Monto) LiquidarPlanillas(
        int idPeriodo, int idTrabajo, long registradoPor)
    {
        _logger.LogInformation($"Liquidar planillas período {idPeriodo}");
        
        using var conn = new SqlConnection(_connectionString);
        try
        {
            conn.Open();
            var result = conn.QueryFirstOrDefault<dynamic>(
                "CC_LiquidarPlanillas",
                new { idPeriodo, idTrabajo, registradoPor },
                commandType: CommandType.StoredProcedure
            );
            
            if (result == null)
                return (false, 0, 0);
            
            return (true, result.RegistrosLiquidados, result.Monto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error en LiquidarPlanillas: {ex.Message}");
            return (false, 0, 0);
        }
    }
    
    public (bool Success, int RegistrosGenerados, decimal MontoTotal) GenerarBonificacion(
        int idPeriodo)
    {
        _logger.LogInformation($"Generar bonificación período {idPeriodo}");
        
        using var conn = new SqlConnection(_connectionString);
        try
        {
            var result = conn.QueryFirstOrDefault<dynamic>(
                "CC_GenerarBonificacion",
                new { idPeriodo },
                commandType: CommandType.StoredProcedure
            );
            
            return (true, result.RegistrosGenerados, result.MontoTotal);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error en GenerarBonificacion: {ex.Message}");
            return (false, 0, 0);
        }
    }
    
    // ... más métodos para cada SP crítico
}
```

**Métodos mínimos requeridos** (15-20):
- ObtenerProduccion
- ObtenerPST
- ObtenerConteos
- LiquidarPlanillas
- GenerarBonificacion
- CargueDescuentosSS
- LiquidarProductividadPST
- ReportePagos
- ReporteActividadesProduccion
- ReporteContabilizacionPST
- EstadoJobBooks
- ... otros

**Deliverable**: `Areas/CC/Data/Adapters/CCFinzOpeDataAdapter.cs` (500+ líneas)

### Tarea 6: Crear Service Layer (12 horas)

**Objetivo**: Exponer Adapter vía Service con validaciones y logging

```csharp
// Areas/CC/Data/Services/CCFinzOpeService.cs

public class CCFinzOpeService
{
    private readonly CCFinzOpeDataAdapter _adapter;
    private readonly ILogger<CCFinzOpeService> _logger;
    
    public CCFinzOpeService(CCFinzOpeDataAdapter adapter, 
        ILogger<CCFinzOpeService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }
    
    public List<ProduccionResultDTO> ObtenerProduccion(DateTime fInicio, DateTime fFin)
    {
        if (fInicio > fFin)
            throw new ArgumentException("Fecha inicio no puede ser mayor a fecha fin");
        
        var produccion = _adapter.ObtenerProduccion(fInicio, fFin);
        _logger.LogInformation($"Obtenidas {produccion.Count} registros de producción");
        return produccion;
    }
    
    public (bool Success, string Message, int Registros, decimal Monto) LiquidarPlanillas(
        int idPeriodo, int idTrabajo, long registradoPor)
    {
        // Validaciones
        if (idPeriodo <= 0)
            return (false, "Período inválido", 0, 0);
        
        // Ejecutar
        var (success, registros, monto) = _adapter.LiquidarPlanillas(
            idPeriodo, idTrabajo, registradoPor
        );
        
        if (success)
            _logger.LogInformation(
                $"Liquidación exitosa: {registros} registros, monto ${monto}"
            );
        else
            _logger.LogError("Error en liquidación de planillas");
        
        return (success, success ? "OK" : "Error", registros, monto);
    }
    
    // ... más métodos
}
```

**Deliverable**: `Areas/CC/Data/Services/CCFinzOpeService.cs`

### Tarea 7: Validación y Testing (8 horas)

**Objetivo**: Validar que CC_FinzOpe funciona correctamente antes de FI Sprint 1

**Checklist**:
- [ ] DbContext compila sin errores
- [ ] Modelos tienen todas las propiedades
- [ ] Adapter se puede instanciar (DI)
- [ ] Service se puede instanciar (DI)
- [ ] SP CC_LiquidarPlanillas se ejecuta correctamente
- [ ] SP CC_GenerarBonificacion se ejecuta correctamente
- [ ] Índices en SQL Server optimizados
- [ ] Performance acceptable (SP < 2s)
- [ ] Connection string válida en appsettings
- [ ] Logging funciona

**Testing Manual**:
```csharp
// En Program.cs o en test project
var adapter = new CCFinzOpeDataAdapter(connStr, efContext, logger);
var service = new CCFinzOpeService(adapter, logger);

// Test 1: Leer producción
var produccion = service.ObtenerProduccion(
    DateTime.Now.AddMonths(-1), 
    DateTime.Now
);
Assert.True(produccion.Count > 0);

// Test 2: Liquidar (si datos existen)
var (success, msg, registros, monto) = service.LiquidarPlanillas(202401, 1, 1);
Assert.True(success, msg);
```

**Deliverable**: Documento `VALIDACION_CC_FINZOPE_TESTING.md`

---

## 6) Dependencias Internas CC_FinzOpe

### 6.1 Relaciones entre Tablas

```
CC_Trabajos (1)
    ├─→ (N) CC_Produccion
    ├─→ (N) CC_Conteos
    ├─→ (N) CC_DetallePresupuesto
    └─→ (N) CC_Liquidaciones

CC_Produccion (1)
    └─→ (1) CC_Trabajos

CC_PrestacionServicios (1)
    ├─→ (1) CC_Trabajos
    └─→ (N) CC_LiquidacionesPST

CC_Descuentos (1)
    └─→ (1) CC_PrestacionServicios

CC_Bonificaciones (1)
    ├─→ (1) CC_Trabajos
    └─→ (1) CC_PrestacionServicios
```

### 6.2 Dependencias de SP

```
CC_LiquidarPlanillas
├─ Consulta: CC_Produccion
├─ Consulta: CC_DetallePresupuesto
├─ Consulta: CC_Descuentos
├─ Inserta: CC_Liquidaciones
└─ Requiere: Índices en CC_Produccion por IdTrabajo, Fecha

CC_GenerarBonificacion
├─ Consulta: CC_PrestacionServicios
├─ Consulta: CC_Produccion (si aplica)
├─ Inserta: CC_Bonificaciones
└─ Requiere: Índices en CC_PrestacionServicios por IdContratista

CC_ReportePagos
├─ Consulta: CC_Liquidaciones (LEFT JOIN múltiples tablas)
└─ Performance: Crítica (puede retornar 10k+ registros)
```

---

## 7) Validación Pre-Sprint 1

### 7.1 Checklist Final

- [ ] **Tablas**:
  - [ ] Todas las tablas CC_* están en SQL Server
  - [ ] Índices existen y son óptimos
  - [ ] FK están definidas correctamente
  - [ ] Datos históricos presentes (Produccion, PST, etc.)

- [ ] **SP**:
  - [ ] CC_LiquidarPlanillas existe y se ejecuta
  - [ ] CC_GenerarBonificacion existe y se ejecuta
  - [ ] CC_ReportePagos se ejecuta correctamente
  - [ ] Otros 15+ SP validados
  - [ ] Todos los SP aceptan parámetros esperados

- [ ] **Código**:
  - [ ] DbContext compila sin errores
  - [ ] 10+ modelos generados correctamente
  - [ ] Adapter implementa 15+ métodos
  - [ ] Service layer registrado en DI
  - [ ] Logging funciona

- [ ] **Configuración**:
  - [ ] appsettings.json tiene connection string CCFinzOpe
  - [ ] Program.cs registra servicios
  - [ ] Áreas/CC/ está creada

- [ ] **Testing**:
  - [ ] Service.ObtenerProduccion() retorna datos ✅
  - [ ] Service.LiquidarPlanillas() ejecuta sin error ✅
  - [ ] Performance: SP < 2 segundos ✅
  - [ ] Logging muestra operaciones ✅

### 7.2 Criterios de Aprobación

**Sprint Pre-1 se aprueba cuando**:
1. ✅ Compilación sin errores
2. ✅ 0 warnings críticos (nullability aceptable)
3. ✅ Todas las tablas y SP mapeadas
4. ✅ Testing manual exitoso
5. ✅ Documentación completa (tareas 1-2)
6. ✅ Ready para FI Sprint 1

---

## 8) Timeline Detallado

```
SEMANA 1 (40 horas)
├─ Día 1-2 (8h): Tarea 1 - Análisis tablas
├─ Día 2-3 (8h): Tarea 2 - Mapeo SP
├─ Día 3-5 (16h): Tarea 3 - DbContext EF Core
└─ Día 5 (8h): Tarea 4 - DTOs (inicio)

SEMANA 2 (40 horas)
├─ Día 1 (8h): Tarea 4 - DTOs (final)
├─ Día 1-3 (20h): Tarea 5 - Adapter
├─ Día 3-4 (12h): Tarea 6 - Service
└─ Día 5 (8h): Tarea 7 - Testing & Validación

TOTAL: 80 horas (2 semanas @ 40h/sem)
```

---

## 9) Riesgos y Mitigaciones

| Riesgo | Probabilidad | Impacto | Mitigación |
|--------|--------------|---------|-----------|
| SP CC_LiquidarPlanillas tiene bugs | Baja | Muy Alto | Validar con datos reales antes de Sprint 1 |
| Índices en CC_Produccion insuficientes | Media | Alto | Revisar índices; agregar si necesario |
| Connection string incorrecta | Baja | Alto | Validar en appsettings.json |
| Modelos EF Core incompletos | Baja | Medio | Revisar scaffold output; completar manual |
| Performance SP > 2s | Media | Medio | Optimizar índices; usar paginación si es necesario |

---

## 10) Documentos de Entrega

**Al finalizar Sprint Pre-1, entregar**:

1. **Documentación**:
   - ✅ `ANALISIS_CC_FINZOPE_TABLAS.md` (tablas, columnas, FK, índices)
   - ✅ `MAPEO_SP_CC_FINZOPE.md` (todos los SP mapeados a Grupo FI)
   - ✅ `VALIDACION_CC_FINZOPE_TESTING.md` (resultados testing)

2. **Código**:
   - ✅ `Areas/CC/Data/Contexts/CC_FinzOpeContext.cs` + modelos
   - ✅ `Areas/CC/Data/DTOs/` (8-10 DTO classes)
   - ✅ `Areas/CC/Data/Adapters/CCFinzOpeDataAdapter.cs`
   - ✅ `Areas/CC/Data/Services/CCFinzOpeService.cs`
   - ✅ `Program.cs` actualizado con DI

3. **Validación**:
   - ✅ Compilación sin errores
   - ✅ Todos los tests pasando
   - ✅ Checklist final completado
   - ✅ Listo para FI Sprint 1

---

## 11) Próximo Paso: FI Sprint 1

Una vez Sprint Pre-1 completado:
- CC_FinzOpe está migrado ✅
- Adapter y Service listos ✅
- Dependencias satisfechas ✅
- Iniciar FI Sprint 1 (Grupo 1: Control Presupuestos) en semana 3

---

**Documento**: PLAN_SPRINT_PRE1_CC_FINZOPE.md  
**Versión**: 1.0  
**Estado**: 📋 Ready for review  
**Próxima actualización**: Post-análisis de tablas (Tarea 1)

