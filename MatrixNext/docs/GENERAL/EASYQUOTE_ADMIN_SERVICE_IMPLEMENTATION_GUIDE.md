# EasyQuoteAdminServiceEF Implementation Guide

**Status:** CORRECTED & READY FOR IMPLEMENTATION  
**Created:** January 12, 2026  
**Author:** Analysis of Legacy Code vs New Entity Models  
**Version:** 1.0

---

## OVERVIEW

The corrected `EasyQuoteAdminServiceEF.cs` provides a complete replacement for the legacy `EasyQuoteAdminService.cs` that used raw SQL MERGE statements and Dapper.

### Key Improvements:

✅ **EF Core Instead of Raw SQL** - Type-safe, testable, migrable  
✅ **Async/Await** - Proper async patterns for database operations  
✅ **Property Name Alignment** - Uses correct entity property names  
✅ **Compound Key Support** - UPSERT logic for multi-column keys  
✅ **Better Error Handling** - Try-catch with operation results  
✅ **Query Helpers** - GetXAsync methods for common lookups  

### Key Fixes Applied:

| Issue | Legacy | Corrected |
|---|---|---|
| **EqParamPrecio columns** | MetodologiaCodigo, PenetracionCodigo | TipoMetodologia, PenetracionRango ✓ |
| **EqValorHoraOps rates** | Single ValorHora | Four rates (BaseCostRate, OverheadRate, etc.) ✓ |
| **EqCostInsumos schema** | Long format (NSE, Tipo) | Wide format (NSE with cost columns) ✓ |
| **EqParamScriptProc** | HorasProcesamiento | HorasProc ✓ |
| **EqRateEstadistica** | PrecioReferencia | PrecioRef2024 ✓ |
| **Database access** | SqlClient/Dapper | DbContext/EF Core ✓ |

---

## MIGRATION PATH

### Phase 1: Create Missing Entities (Required)

Before using `EasyQuoteAdminServiceEF`, you must create these missing entity models:

```
- EqParamMisc         (eq_param_misc table)
- EqEnvioParam        (eq_envio_param table)
- EqEnvioTarifa       (eq_envio_tarifa table)
- EqProductividadCiudad (eq_productividad_ciudad table)
- EqCodificacionParam (eq_codificacion_param table)
- EqCostUnitarioOps   (eq_cost_unitario_ops table)
- EqTarifaMystery     (eq_tarifa_mystery table)
- EqCostBaseDatos     (eq_cost_base_datos table)
- EqParamFactores     (eq_param_factores table)
- EqRateHoras         (eq_rate_horas table)
```

**Estimated Time:** 3-4 hours (5 mins per entity × 10)

### Phase 2: Register DbSets in MatrixDbContext

Add to `MatrixDbContext.cs`:

```csharp
// === NEW EQ MASTER TABLES ===
public DbSet<EqParamMisc> EqParamMiscs { get; set; }
public DbSet<EqEnvioParam> EqEnvioParams { get; set; }
public DbSet<EqEnvioTarifa> EqEnvioTarifas { get; set; }
public DbSet<EqProductividadCiudad> EqProductividadCiudads { get; set; }
public DbSet<EqCodificacionParam> EqCodificacionParams { get; set; }
public DbSet<EqCostUnitarioOps> EqCostUnitarioOps { get; set; }
public DbSet<EqTarifaMystery> EqTarifaMysteries { get; set; }
public DbSet<EqCostBaseDatos> EqCostBaseDatos { get; set; }
public DbSet<EqParamFactores> EqParamFactores { get; set; }
public DbSet<EqRateHoras> EqRateHoras { get; set; }
```

**Estimated Time:** 10 minutes

### Phase 3: Create Database Migration

```bash
cd MatrixNext
dotnet ef migrations add AddEQMasterTables -c MatrixDbContext -p MatrixNext.Web
dotnet ef database update
```

**Estimated Time:** 5 minutes (+ time for schema validation)

### Phase 4: Update Admin Controller to Use New Service

Old:
```csharp
var service = new EasyQuoteAdminService(masters, config);
var result = service.UpsertPrecio(row);
```

New:
```csharp
var service = new EasyQuoteAdminServiceEF(dbContext);
var result = await service.UpsertPrecioAsync(precio);
```

**Estimated Time:** 1-2 hours (controller updates)

### Phase 5: Update CSV Import Logic

The CSV import logic needs to be updated to match new entity schemas.

**For EqParamPrecio:**
```csharp
// Old legacy code expected: MetodologiaCodigo, PenetracionCodigo
// New expects: TipoMetodologia, PenetracionRango

var precios = new List<EqParamPrecio>();
while ((line = reader.ReadLine()) != null)
{
    var parts = line.Split(';', ',');
    precios.Add(new EqParamPrecio
    {
        TipoMetodologia = parts[0].Trim(),      // e.g., "F2F"
        PenetracionRango = parts[1].Trim(),     // e.g., "75-82"
        DuracionMin = int.Parse(parts[2]),
        ValorTotal = decimal.Parse(parts[3]),
        ValorPerfil = 0,
        ValorCoord = 0,
        Version = version ?? DateTime.Now.ToString("s")
    });
}

var result = await service.ReplaceAllPreciosAsync(precios);
```

**For EqCostInsumos:**
```csharp
// CRITICAL: New schema is ONE row per NSE with ALL costs as columns
// Old schema was MULTIPLE rows per NSE (one per Tipo)

// New format requires:
var insumos = new EqCostInsumos
{
    NSE = 1,
    Reclutamiento = 50000,
    Obsequio = 10000,
    Productividad = 5000,
    Dias = 0,
    Supervisores = 8000,
    Logistica = 3000,
    TransporteEncuestador = 12000,
    TransporteSupervisor = 15000,
    ValorEnvio1erKilo = 2500,
    ValorKiloAdicional = 500,
    SeguroPct = 0.15m,
    ValorMinDeclarar = 100000
};

await service.UpsertCostInsumosAsync(insumos);
```

**Estimated Time:** 2-3 hours (depends on CSV format complexity)

---

## API REFERENCE

### UPSERT Operations (Async)

All UPSERT operations follow the same pattern:
- Check if record exists by compound key
- UPDATE existing or INSERT new
- Return `OperationResult { Success, Message }`

#### EqParamPrecio

```csharp
public async Task<OperationResult> UpsertPrecioAsync(EqParamPrecio precio)
```

**Compound Key:** `(TipoMetodologia, PenetracionRango, DuracionMin)`

**Example:**
```csharp
var precio = new EqParamPrecio
{
    TipoMetodologia = "F2F",
    PenetracionRango = "75-82",
    DuracionMin = 30,
    ValorTotal = 150000,
    ValorPerfil = 50000,
    ValorCoord = 20000,
    Version = "2024-Q1",
    VigentDesde = DateTime.Now
};

var result = await service.UpsertPrecioAsync(precio);
if (result.Success)
    Console.WriteLine("Precio guardado");
else
    Console.WriteLine($"Error: {result.Message}");
```

#### EqValorHoraOps

```csharp
public async Task<OperationResult> UpsertValorHoraOpsAsync(EqValorHoraOps valorHora)
```

**Compound Key:** `(Nivel, Alternativa)`

**Example:**
```csharp
var valor = new EqValorHoraOps
{
    Nivel = "L5",
    Alternativa = "2024",
    BaseCostRate = 45000,
    OverheadRate = 15000,
    LoadedCostRate = 60000,
    BillingRate = 85000,
    VigentDesde = DateTime.Now
};

var result = await service.UpsertValorHoraOpsAsync(valor);
```

#### EqCostInsumos

```csharp
public async Task<OperationResult> UpsertCostInsumosAsync(EqCostInsumos insumos)
```

**Compound Key:** `(NSE)`

**Note:** Uses WIDE schema - one row per NSE with all cost types as columns.

**Example:**
```csharp
var insumos = new EqCostInsumos { NSE = 3, Reclutamiento = 35000, ... };
var result = await service.UpsertCostInsumosAsync(insumos);
```

#### EqParamScriptProc

```csharp
public async Task<OperationResult> UpsertParamScriptProcAsync(EqParamScriptProc param)
```

**Compound Key:** `(DuracionMin)`

#### EqLocaciones

```csharp
public async Task<OperationResult> UpsertLocacionAsync(EqLocaciones locacion)
```

**Compound Key:** `(Ciudad)`

#### EqRateEstadistica

```csharp
public async Task<OperationResult> UpsertRateEstadisticaAsync(EqRateEstadistica rate)
```

**Compound Key:** `(Categoria, Servicio)`

### Bulk Operations

#### Replace All Precios
```csharp
public async Task<OperationResult> ReplaceAllPreciosAsync(List<EqParamPrecio> precios)
```

Deletes ALL existing precios and inserts new collection. Useful for batch imports.

#### Replace All Valor Hora
```csharp
public async Task<OperationResult> ReplaceAllValorHoraAsync(List<EqValorHoraOps> horasList)
```

#### Replace All Cost Insumos
```csharp
public async Task<OperationResult> ReplaceAllCostInsumosAsync(List<EqCostInsumos> insumosList)
```

### Query Helpers

#### Get Precio by Compound Key
```csharp
public async Task<EqParamPrecio?> GetPrecioAsync(
    string tipoMetodologia, 
    string penetracionRango, 
    int duracionMin)
```

#### Get All Precios for Metodologia
```csharp
public async Task<List<EqParamPrecio>> GetPreciosByMetodologiaAsync(string tipoMetodologia)
```

Returns precios sorted by PenetracionRango, then DuracionMin.

#### Get Valor Hora
```csharp
public async Task<EqValorHoraOps?> GetValorHoraAsync(string nivel, string? alternativa = null)
```

#### Get All Valor Hora
```csharp
public async Task<List<EqValorHoraOps>> GetAllValorHoraAsync()
```

#### Get Cost Insumos
```csharp
public async Task<EqCostInsumos?> GetCostInsumosAsync(int nse)
```

#### Get Locacion
```csharp
public async Task<EqLocaciones?> GetLocacionAsync(string ciudad)
```

#### Get Rate Estadistica
```csharp
public async Task<EqRateEstadistica?> GetRateEstadisticaAsync(string categoria, string servicio)
```

---

## DEPENDENCY INJECTION SETUP

Add to `Program.cs` or `Startup.cs`:

```csharp
// In ConfigureServices or WebApplicationBuilder
services.AddScoped<EasyQuoteAdminServiceEF>();
```

Usage in controller:

```csharp
[ApiController]
[Route("api/[controller]")]
public class EQAdminController : ControllerBase
{
    private readonly EasyQuoteAdminServiceEF _adminService;

    public EQAdminController(EasyQuoteAdminServiceEF adminService)
    {
        _adminService = adminService;
    }

    [HttpPost("precio")]
    public async Task<IActionResult> UpsertPrecio([FromBody] EqParamPrecio precio)
    {
        var result = await _adminService.UpsertPrecioAsync(precio);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
```

---

## TRANSACTION HANDLING

The service uses automatic transaction management:

```csharp
await _context.SaveChangesAsync();  // Wraps in transaction automatically
```

For explicit transaction control:

```csharp
using var transaction = await _context.Database.BeginTransactionAsync();
try
{
    await _adminService.ReplaceAllPreciosAsync(precios);
    await transaction.CommitAsync();
}
catch (Exception ex)
{
    await transaction.RollbackAsync();
    throw;
}
```

---

## ERROR HANDLING

All operations return `OperationResult`:

```csharp
public class OperationResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }
}
```

Example usage:

```csharp
var result = await service.UpsertPrecioAsync(precio);

if (!result.Success)
{
    _logger.LogError($"Admin operation failed: {result.Message}");
    return StatusCode(500, result);
}

return Ok(result);
```

---

## TESTING EXAMPLES

### Unit Test Example

```csharp
[TestFixture]
public class EasyQuoteAdminServiceEFTests
{
    private DbContextOptions<MatrixDbContext> _options;
    private MatrixDbContext _context;
    private EasyQuoteAdminServiceEF _service;

    [SetUp]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<MatrixDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new MatrixDbContext(_options);
        _service = new EasyQuoteAdminServiceEF(_context);
    }

    [Test]
    public async Task UpsertPrecio_NewRecord_InsertSuccess()
    {
        // Arrange
        var precio = new EqParamPrecio
        {
            TipoMetodologia = "F2F",
            PenetracionRango = "75-82",
            DuracionMin = 30,
            ValorTotal = 150000
        };

        // Act
        var result = await _service.UpsertPrecioAsync(precio);

        // Assert
        Assert.IsTrue(result.Success);
        var stored = await _service.GetPrecioAsync("F2F", "75-82", 30);
        Assert.IsNotNull(stored);
        Assert.AreEqual(150000, stored.ValorTotal);
    }

    [Test]
    public async Task UpsertPrecio_ExistingRecord_UpdateSuccess()
    {
        // Arrange
        var precio1 = new EqParamPrecio
        {
            TipoMetodologia = "CATI",
            PenetracionRango = "46-54",
            DuracionMin = 20,
            ValorTotal = 100000
        };
        await _service.UpsertPrecioAsync(precio1);

        // Act - Update
        precio1.ValorTotal = 110000;
        var result = await _service.UpsertPrecioAsync(precio1);

        // Assert
        Assert.IsTrue(result.Success);
        var stored = await _service.GetPrecioAsync("CATI", "46-54", 20);
        Assert.AreEqual(110000, stored.ValorTotal);
    }

    [TearDown]
    public void Teardown()
    {
        _context.Dispose();
    }
}
```

---

## LEGACY SQL CONVERSION REFERENCE

### Original MERGE Statements → New EF Core Equivalent

#### UpsertPrecio

**Legacy SQL:**
```sql
MERGE eq_param_precio AS t
USING (SELECT @MetodologiaCodigo m, @PenetracionCodigo p, @DuracionMin d) s
ON t.MetodologiaCodigo=s.m AND t.PenetracionCodigo=s.p AND t.DuracionMin=s.d
WHEN MATCHED THEN UPDATE SET ValorTotal=@ValorTotal
WHEN NOT MATCHED THEN INSERT (MetodologiaCodigo,PenetracionCodigo,DuracionMin,ValorTotal,ValorPerfil,ValorCoordinacion)
VALUES (@MetodologiaCodigo,@PenetracionCodigo,@DuracionMin,@ValorTotal,0,0);
```

**New EF Core:**
```csharp
public async Task<OperationResult> UpsertPrecioAsync(EqParamPrecio precio)
{
    var existing = await _context.EqParamPrecios
        .FirstOrDefaultAsync(p =>
            p.TipoMetodologia == precio.TipoMetodologia &&
            p.PenetracionRango == precio.PenetracionRango &&
            p.DuracionMin == precio.DuracionMin);

    if (existing != null)
    {
        existing.ValorTotal = precio.ValorTotal;
        existing.ValorPerfil = precio.ValorPerfil;
        existing.ValorCoord = precio.ValorCoord;
        _context.Update(existing);
    }
    else
    {
        _context.Add(precio);
    }
    
    await _context.SaveChangesAsync();
    return new OperationResult { Success = true };
}
```

**Key Differences:**
- Column names: `MetodologiaCodigo` → `TipoMetodologia`, etc.
- No raw SQL needed
- Type-safe C# properties
- Async/await pattern
- Automatic DateTime management

---

## KNOWN LIMITATIONS & FUTURE WORK

1. **Version tracking in precios**: Currently stored in `Version` field but no historical queries
   - TODO: Add `GetPreciosHistorico(fecha)` method for date-based lookups

2. **EqCostInsumos schema change**: Wide format is different from legacy long format
   - **Impact:** CSV imports must be reformatted (1 row per NSE instead of multiple rows per NSE)

3. **EqValorHoraOps rate structure**: Now supports 4 rates instead of 1
   - **Impact:** Legacy queries expecting single `ValorHora` need to be updated to use specific rate type

4. **Missing entity models**: 10 models still need to be created
   - TODO: Create EqParamMisc, EqEnvioParam, EqEnvioTarifa, etc.

5. **Concurrency**: No optimistic concurrency control (RowVersion)
   - TODO: Add `[Timestamp]` property to entities for concurrency handling

---

## ROLLBACK PLAN

If issues arise during migration:

1. Keep legacy `EasyQuoteAdminService.cs` as fallback
2. Run in parallel with new service for validation
3. Gradual cutover: Start with read-only queries, then UPSERT operations
4. Monitor for 2-3 weeks before full decommission

---

## DEPLOYMENT CHECKLIST

- [ ] Create 10 missing entity models
- [ ] Register DbSets in MatrixDbContext
- [ ] Create and run migration (`add AddEQMasterTables`)
- [ ] Verify database schema matches entity definitions
- [ ] Update CSV import logic
- [ ] Update admin controllers to use new service
- [ ] Add DI registration to Program.cs
- [ ] Create unit tests for UPSERT operations
- [ ] Load test: Import 10k records
- [ ] Run integration tests against staging database
- [ ] Update admin UI endpoints
- [ ] Documentation update
- [ ] Deploy to staging
- [ ] UAT sign-off
- [ ] Deploy to production

**Estimated Total Time:** 3-5 days (depending on CSV import complexity)

---

## CONTACT & SUPPORT

For issues with:
- **Entity schema mapping:** See ENTITY_ANALYSIS_MAPPING.md
- **Database migrations:** Run `dotnet ef migrations list`
- **Performance:** Check DbContext query logs with `LogTo(Console.WriteLine)`
- **Compound key UPSERT:** Review GetXAsync helpers in service

---

**Generated:** January 12, 2026  
**Version:** 1.0  
**Status:** Ready for Implementation
