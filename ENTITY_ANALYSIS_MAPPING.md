# EQ Entity Models Analysis & Mapping

**Analysis Date:** January 12, 2026  
**Scope:** MatrixNext EQ (EasyQuote) Module Entity Models  
**Status:** CRITICAL MISMATCHES DISCOVERED

---

## EXECUTIVE SUMMARY

Analysis of entity models in `MatrixNext.Web/Models/EQ/` reveals **SIGNIFICANT DISCREPANCIES** between:
1. **New EF Core Entity Models** (EqParam*.cs, EqCost*.cs, etc.) - SQL table schema definitions
2. **Legacy Admin Service** (EasyQuoteAdminService.cs) - Dapper/SQL expectations
3. **Master Service Row Structures** (EasyQuoteMasterService.cs) - Expected row schemas

**CRITICAL FINDING:** The legacy SQL MERGE statements use column names that **DO NOT MATCH** the new entity model property names.

---

## DETAILED ENTITY MAPPING TABLE

| Entity Class | DbSet Name | SQL Table | Primary Key | Compound Key (UPSERT) | Current Properties | Legacy Row Structure | STATUS |
|---|---|---|---|---|---|---|---|
| **EqParamPrecio** | EqParamPrecios | eq_param_precio | Id (int) | `TipoMetodologia, PenetracionRango, DuracionMin` | TipoMetodologia, PenetracionRango, DuracionMin, ValorPerfil, ValorCoord, ValorTotal, Version, VigentDesde, VigentHasta | PrecioRow: `MetodologiaCodigo, PenetracionCodigo, DuracionMin, ValorTotal` | ⚠️ **MISMATCH** |
| **EqParamScriptProc** | EqParamScriptProcs | eq_param_script_proc | Id (int) | `DuracionMin` | DuracionMin, HorasScript, HorasProc, HorasHarmoni, HorasGraficacion | HorasRow: `DuracionMin, HorasScript, HorasProcesamiento, HorasHarmoni, HorasGraficacion` | ⚠️ **PARTIAL MISMATCH** |
| **EqValorHoraOps** | EqValorHoraOps | eq_valor_hora_ops | Id (int) | `Nivel, Alternativa` | Nivel, Alternativa, BaseCostRate, OverheadRate, LoadedCostRate, BillingRate, VigentDesde, VigentHasta | ValorHoraRow: `Nivel, Variante, ValorHora` | ⚠️ **MAJOR MISMATCH** |
| **EqCostInsumos** | EqCostInsumos | eq_cost_insumos | Id (int) | `NSE` | NSE, Reclutamiento, Obsequio, Productividad, Dias, Supervisores, Logistica, TransporteEncuestador, TransporteSupervisor, ValorEnvio1erKilo, ValorKiloAdicional, SeguroPct, ValorMinDeclarar | CostInsumoRow: `NSE, Tipo, ValorUnitario` | ⚠️ **MAJOR MISMATCH** |
| **EqLocaciones** | EqLocaciones | eq_locaciones | Id (int) | `Ciudad` | Ciudad, TarifaBase, TarifaConGross, DiasBase | LocacionRow: `Ciudad, TarifaBase, TarifaConGross, DiasBase` | ✅ **MATCH** |
| **EqRateEstadistica** | EqRateEstadisticas | eq_rate_estadistica | Id (int) | `Categoria, Servicio` | Categoria, Servicio, HorasEstimadas, PrecioRef2024, FactorEscala, LeadTime, Ejemplos, FactorEconomiaEscala | RateEstadisticaRow: `Categoria, Servicio, HorasEstimadas, PrecioReferencia, FactorEscala` | ⚠️ **MINOR MISMATCH** |
| **EqQuoteHeader** | EqQuoteHeaders | eq_quote_header | Id (int) | N/A (transactional) | PropuestaNombre, GrupoObjetivo, Cliente, FechaAprobacionEstimada, FechaCampo, ProbabilidadAprobacion, SL, MetodologiaSL, RecordDetail, CategoriaProducto, ValorProveedorExterno, ValorProveedorInternacional, ValorGMU, Notas + FK relations | N/A (not in legacy admin) | ✅ **NEW** |
| **EqQuestionnaire** | EqQuestionnaires | eq_questionnaire | Id (int) | N/A (child of QuoteHeader) | QuoteHeaderId (FK), DuracionMinutos, PenetracionLabel, PenetracionValor, PreguntasAbiertas, PreguntasAbiertasMultiples, OtrosProcesos, TopLine, DataCleaning, ASCII, ScriptReclutamiento, Scripting, TipoScript, Codificacion, Procesamiento, etc. | N/A (not in legacy admin) | ✅ **NEW** |
| **EqMethodology** | EqMethodologies | eq_methodology | Id (int) | N/A (child of QuoteHeader) | QuoteHeaderId (FK), MetodologiaRecoleccion, Tecnica1Tipo, Tecnica1Flag, Tecnica2Tipo, Tecnica2Flag, Tecnica3Tipo, Tecnica3Flag, BaseDatos, IncidenciaLabel, IncidenciaValor, MetodologiasMix | N/A (not in legacy admin) | ✅ **NEW** |
| **EqSampleCity** | EqSampleCities | eq_sample_city | Id (int) | N/A (child of QuoteHeader) | QuoteHeaderId (FK), Ciudad, Activa, MuestraTotal, NSE1-6, MetodologiaTecnicaReferenciada, SobreMuestraPct, PesoProductoGramos, EnvioCiudades | N/A (not in legacy admin) | ✅ **NEW** |
| **EqMystery** | EqMysteries | eq_mystery | Id (int) | N/A (child of QuoteHeader) | QuoteHeaderId (FK), TipoVisita, Complejidad, NumOlas, Desplazamientos, Tanques, Alertas, EdicionVideo, AlquilerEquipos, CompraDispositivos, Seguimiento | N/A (not in legacy admin) | ✅ **NEW** |
| **EqStaffSL** | EqStaffSLs | eq_staff_sl | Id (int) | N/A (child of QuoteHeader) | QuoteHeaderId (FK), Nivel, HorasMinimas, HorasPresupuestadas, TarifaNivel, ValorTotal, Fuente | N/A (not in legacy admin) | ✅ **NEW** |
| **EqCostResult** | EqCostResults | eq_cost_result | Id (int) | N/A (1:1 with QuoteHeader) | QuoteHeaderId (FK), Moneda, CostoCampo, CostoCalidad, Viaticos, Incentivos, Insumos, Logistica, StaffOps, Estadistica, Scripting, DataCleaning, TopLines, Procesamiento, Harmoni, Graficacion, CompraProducto, Tablets, CostoDirectoTotal, CostoConIncentivos, DirectCostOps, GM, PB_RMF, ProfTime, OP, PctOP, AOTUnitario, AOTTotal | N/A (not in legacy admin) | ✅ **NEW** |

---

## CRITICAL MISMATCHES IDENTIFIED

### 1. **EqParamPrecio** - COLUMN NAME MISMATCHES ⚠️ CRITICAL

**Legacy SQL MERGE (EasyQuoteAdminService.cs Line 51):**
```sql
MERGE eq_param_precio AS t
USING (SELECT @MetodologiaCodigo m, @PenetracionCodigo p, @DuracionMin d) s
ON t.MetodologiaCodigo=s.m AND t.PenetracionCodigo=s.p AND t.DuracionMin=s.d
WHEN MATCHED THEN UPDATE SET ValorTotal=@ValorTotal
WHEN NOT MATCHED THEN INSERT (MetodologiaCodigo,PenetracionCodigo,DuracionMin,ValorTotal,ValorPerfil,ValorCoordinacion)
VALUES (@MetodologiaCodigo,@PenetracionCodigo,@DuracionMin,@ValorTotal,0,0);
```

**Current Entity Model (EqParamPrecio.cs):**
```csharp
public string TipoMetodologia { get; set; }        // NOT MetodologiaCodigo!
public string PenetracionRango { get; set; }       // NOT PenetracionCodigo!
public int DuracionMin { get; set; }               // MATCH ✓
public decimal ValorPerfil { get; set; }           // MATCH ✓
public decimal ValorCoord { get; set; }            // NOT ValorCoordinacion!
public decimal ValorTotal { get; set; }            // MATCH ✓
```

**Problem:** The SQL MERGE expects column names `MetodologiaCodigo, PenetracionCodigo, ValorCoordinacion` but the entity model defines `TipoMetodologia, PenetracionRango, ValorCoord`.

**Impact:** UPSERT operations will FAIL - either table columns don't exist OR entity mappings are wrong.

---

### 2. **EqValorHoraOps** - COLUMN STRUCTURE MISMATCHES ⚠️ CRITICAL

**Legacy SQL MERGE (EasyQuoteAdminService.cs Line 171):**
```sql
MERGE eq_valor_hora_ops t USING (SELECT @Nivel n) s ON t.Nivel=s.n
WHEN MATCHED THEN UPDATE SET Variante=@Variante, ValorHora=@ValorHora
WHEN NOT MATCHED THEN INSERT (Nivel,Variante,ValorHora) VALUES (@Nivel,@Variante,@ValorHora);
```

**Legacy Row Structure (EasyQuoteMasterService.cs):**
```csharp
public class ValorHoraRow
{
    public string Nivel { get; set; } = string.Empty;
    public string Variante { get; set; } = string.Empty;        // Single column
    public decimal ValorHora { get; set; }                       // Single rate column
}
```

**Current Entity Model (EqValorHoraOps.cs):**
```csharp
public string Nivel { get; set; }                 // MATCH ✓
public string Alternativa { get; set; }           // NOT Variante! 
public decimal BaseCostRate { get; set; }         // DIFFERENT structure!
public decimal OverheadRate { get; set; }
public decimal LoadedCostRate { get; set; }
public decimal BillingRate { get; set; }
public DateTime VigentDesde { get; set; }
public DateTime VigentHasta { get; set; }
```

**Problem:** Legacy service expects simple `(Nivel, Variante, ValorHora)` tuple but entity has complex `(Nivel, Alternativa, BaseCostRate, OverheadRate, LoadedCostRate, BillingRate)` structure. The UPSERT logic is incompatible.

**Impact:** Admin UPSERT will fail or insert into wrong columns. Query logic expects different structure.

---

### 3. **EqCostInsumos** - COMPLETELY DIFFERENT STRUCTURE ⚠️ CRITICAL

**Legacy SQL MERGE (EasyQuoteAdminService.cs Line 166):**
```sql
MERGE eq_cost_insumos t USING (SELECT @NSE n,@Tipo t0) s ON t.NSE=s.n AND t.Tipo=s.t0
WHEN MATCHED THEN UPDATE SET ValorUnitario=@ValorUnitario
WHEN NOT MATCHED THEN INSERT (NSE,Tipo,ValorUnitario) VALUES (@NSE,@Tipo,@ValorUnitario);
```

**Legacy Row Structure (EasyQuoteMasterService.cs):**
```csharp
public class CostInsumoRow
{
    public string NSE { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;            // Cost type (e.g., "Reclutamiento")
    public decimal ValorUnitario { get; set; }                   // Single unit cost
}
```

**Current Entity Model (EqCostInsumos.cs):**
```csharp
public int NSE { get; set; }                       // Numeric, not string!
// NO Tipo column at all!
public decimal Reclutamiento { get; set; }         // Multiple cost columns instead
public decimal Obsequio { get; set; }
public decimal Productividad { get; set; }
public decimal Dias { get; set; }
public decimal Supervisores { get; set; }
public decimal Logistica { get; set; }
public decimal TransporteEncuestador { get; set; }
public decimal TransporteSupervisor { get; set; }
public decimal ValorEnvio1erKilo { get; set; }
public decimal ValorKiloAdicional { get; set; }
public decimal SeguroPct { get; set; }
public decimal ValorMinDeclarar { get; set; }
```

**Problem:** Legacy uses "long format" `(NSE, Tipo, ValorUnitario)` where each cost type is a row. New model uses "wide format" with one row per NSE containing all cost types as columns. Completely incompatible schema.

**Impact:** MERGE statement will fail - `Tipo` column doesn't exist in new table. CSV import logic incompatible with new schema.

---

### 4. **EqParamScriptProc** - PROPERTY NAME MISMATCH ⚠️ MINOR

**Legacy Row Structure (EasyQuoteMasterService.cs):**
```csharp
public class HorasRow
{
    public int DuracionMin { get; set; }
    public decimal HorasScript { get; set; }
    public decimal HorasProcesamiento { get; set; }        // Note: "Procesamiento"
    public decimal HorasHarmoni { get; set; }
    public decimal HorasGraficacion { get; set; }
}
```

**Current Entity Model (EqParamScriptProc.cs):**
```csharp
public decimal HorasScript { get; set; }
public decimal HorasProc { get; set; }              // NOT HorasProcesamiento!
public decimal HorasHarmoni { get; set; }
public decimal HorasGraficacion { get; set; }
```

**Problem:** Legacy service constructs query with `HorasProcesamiento` but entity property is `HorasProc`. Will cause query failures.

**Impact:** SELECT queries will fail to map properly.

---

### 5. **EqRateEstadistica** - COLUMN NAME MISMATCH ⚠️ MINOR

**Legacy Row Structure (EasyQuoteMasterService.cs - Line 47):**
```sql
RateEstadistica = conn.Query<RateEstadisticaRow>(
    "SELECT Categoria, Servicio, HorasEstimadas, PrecioReferencia, FactorEscala 
     FROM eq_rate_estadistica"
).ToList()
```

**Legacy Row Class:**
```csharp
public class RateEstadisticaRow
{
    public string Categoria { get; set; } = string.Empty;
    public string Servicio { get; set; } = string.Empty;
    public decimal HorasEstimadas { get; set; }
    public decimal PrecioReferencia { get; set; }           // Note: "Referencia"
    public decimal FactorEscala { get; set; }
}
```

**Current Entity Model (EqRateEstadistica.cs):**
```csharp
public decimal PrecioRef2024 { get; set; }         // NOT PrecioReferencia!
```

**Problem:** Legacy SELECT uses `PrecioReferencia` but entity defines `PrecioRef2024`. Query will fail.

**Impact:** Rate statistics queries will fail or return NULL.

---

## TABLES REFERENCED IN LEGACY BUT NOT IN NEW ENTITY MODEL

These tables/DbSets are referenced in the legacy admin service but have NO corresponding entity model:

| Table Name (Legacy SQL) | DbSet Expected | Current Status | CRITICAL? |
|---|---|---|---|
| **eq_param_misc** | ❌ MISSING | No EqParamMisc entity created | ⚠️ YES |
| **eq_envio_param** | ❌ MISSING | No EqEnvioParam entity created | ⚠️ YES |
| **eq_envio_tarifa** | ❌ MISSING | No EqEnvioTarifa entity created | ⚠️ YES |
| **eq_productividad_ciudad** | ❌ MISSING | No EqProductividadCiudad entity created | ⚠️ YES |
| **eq_codificacion_param** | ❌ MISSING | No EqCodificacionParam entity created | ⚠️ YES |
| **eq_cost_unitario_ops** | ❌ MISSING | No EqCostUnitarioOps entity created | ⚠️ YES |
| **eq_tarifa_mystery** | ❌ MISSING | No EqTarifaMystery entity created | ⚠️ YES |
| **eq_cost_base_datos** | ❌ MISSING | No EqCostBaseDatos entity created | ⚠️ YES |
| **eq_param_factores** | ❌ MISSING | No EqParamFactores entity created | ⚠️ YES |
| **eq_rate_horas** | ❌ MISSING | No EqRateHoras entity created | ⚠️ YES |

---

## LEGACY SQL MERGE STATEMENTS vs NEW ENTITY SCHEMA

### UpsertPrecio (Legacy)
```csharp
public object UpsertPrecio(EasyQuoteMasterService.PrecioRow row)
{
    const string sql = @"MERGE eq_param_precio AS t
USING (SELECT @MetodologiaCodigo m, @PenetracionCodigo p, @DuracionMin d) s
ON t.MetodologiaCodigo=s.m AND t.PenetracionCodigo=s.p AND t.DuracionMin=s.d
WHEN MATCHED THEN UPDATE SET ValorTotal=@ValorTotal
WHEN NOT MATCHED THEN INSERT (MetodologiaCodigo,PenetracionCodigo,DuracionMin,ValorTotal,ValorPerfil,ValorCoordinacion)
VALUES (@MetodologiaCodigo,@PenetracionCodigo,@DuracionMin,@ValorTotal,0,0);";
    Exec(sql, row);
    _masters.Reset();
    return new { success = true };
}
```

**Issue:** Uses `MetodologiaCodigo, PenetracionCodigo, ValorCoordinacion` which don't exist in new entity.

**Fix Required:** Either:
- Option A: Update SQL to use `TipoMetodologia, PenetracionRango, ValorCoord`
- Option B: Rename entity properties to match legacy SQL
- Option C: Use EF Core UPSERT patterns instead of raw SQL MERGE

---

## RECOMMENDATIONS

### IMMEDIATE (P0 - BLOCKING)

1. **Create Missing Entity Models** (10 models needed):
   - `EqParamMisc` 
   - `EqEnvioParam`
   - `EqEnvioTarifa`
   - `EqProductividadCiudad`
   - `EqCodificacionParam`
   - `EqCostUnitarioOps`
   - `EqTarifaMystery`
   - `EqCostBaseDatos`
   - `EqParamFactores`
   - `EqRateHoras`

2. **Register DbSets in MatrixDbContext** for all 10 new entities

3. **Create Database Migrations** to ensure all 10 tables exist

### HIGH (P1 - MUST BEFORE ADMIN SERVICE)

4. **Fix EqParamPrecio Columns:**
   - Rename `TipoMetodologia` → `MetodologiaCodigo` (OR update SQL MERGE)
   - Rename `PenetracionRango` → `PenetracionCodigo` (OR update SQL MERGE)
   - Rename `ValorCoord` → `ValorCoordinacion` (OR update SQL MERGE)

5. **Fix EqValorHoraOps Structure:**
   - Either: Flatten back to simple `(Nivel, Variante, ValorHora)` 
   - Or: Rewrite UPSERT logic to handle all 4 rate columns

6. **Fix EqCostInsumos Schema:**
   - MAJOR DECISION: Keep wide format OR convert back to long format?
   - Current wide format cannot use legacy MERGE logic
   - Rewrite CSV import and UPSERT logic to match new schema

7. **Fix EqParamScriptProc:**
   - Rename `HorasProc` → `HorasProcesamiento` (OR update legacy query)

8. **Fix EqRateEstadistica:**
   - Rename `PrecioRef2024` → `PrecioReferencia` (OR update legacy query)

### MEDIUM (P2 - REFACTOR)

9. **Replace Raw SQL MERGE with EF Core**
   - Migrate from `Dapper.Exec()` to `DbContext.SaveChanges()` with UPSERT patterns
   - Use `ExecuteUpdateAsync()` and `AddAsync()` for INSERT/UPDATE logic
   - Improves testability and reduces SQL injection surface

10. **Create EQ-Specific DbContext or Service**
    - Separate EQ concerns from main MatrixDbContext
    - Better schema isolation for EQ module

---

## CSV IMPORT COMPATIBILITY

**Current ImportPreciosCsv expects column order:**
```csv
Metodologia,Penetracion,Duracion,Valor
F2F,75-82,30,150000
```

Maps to `PrecioRow(MetodologiaCodigo, PenetracionCodigo, DuracionMin, ValorTotal)`

**New entity expects:**
- `TipoMetodologia` instead of `MetodologiaCodigo`
- `PenetracionRango` instead of `PenetracionCodigo`

**Action Required:** Update CSV parser OR standardize entity naming.

---

## SUMMARY TABLE: What Needs to Be Created/Fixed

| Item | Type | Priority | Effort | Status |
|---|---|---|---|---|
| EqParamMisc entity & DbSet | CREATE | P0 | 30min | ❌ |
| EqEnvioParam entity & DbSet | CREATE | P0 | 30min | ❌ |
| EqEnvioTarifa entity & DbSet | CREATE | P0 | 30min | ❌ |
| EqProductividadCiudad entity & DbSet | CREATE | P0 | 30min | ❌ |
| EqCodificacionParam entity & DbSet | CREATE | P0 | 30min | ❌ |
| EqCostUnitarioOps entity & DbSet | CREATE | P0 | 30min | ❌ |
| EqTarifaMystery entity & DbSet | CREATE | P0 | 30min | ❌ |
| EqCostBaseDatos entity & DbSet | CREATE | P0 | 30min | ❌ |
| EqParamFactores entity & DbSet | CREATE | P0 | 30min | ❌ |
| EqRateHoras entity & DbSet | CREATE | P0 | 30min | ❌ |
| Fix EqParamPrecio column names | FIX | P1 | 15min | ❌ |
| Fix EqValorHoraOps schema | FIX | P1 | 1hr | ❌ |
| Fix EqCostInsumos schema | FIX | P1 | 2hr | ❌ |
| Fix EqParamScriptProc property names | FIX | P1 | 10min | ❌ |
| Fix EqRateEstadistica property names | FIX | P1 | 10min | ❌ |
| Update legacy admin SQL MERGE statements | REFACTOR | P1 | 2hr | ❌ |
| Database migrations | MIGRATE | P1 | 30min | ❌ |
| Corrected EasyQuoteAdminServiceEF.cs | CREATE | P1 | 3hr | ❌ |

---

## LEGACY SQL ANALYSIS - ALL UPSERT OPERATIONS

### Summary of All Legacy MERGE Statements:

1. **UpsertPrecio** - Uses `eq_param_precio` with `(MetodologiaCodigo, PenetracionCodigo, DuracionMin)` compound key
2. **UpsertMisc** - Uses `eq_param_misc` with `(Clave)` key - **TABLE MISSING**
3. **UpsertEnvioParam** - Uses `eq_envio_param` - **TABLE/ENTITY MISSING**
4. **UpsertProductividad** - Uses `eq_productividad_ciudad` with `(Ciudad)` key - **TABLE MISSING**
5. **UpsertBaseDatos** - Uses `eq_cost_base_datos` with `(Tipo)` key - **TABLE MISSING**
6. **UpsertValorHora** - Uses `eq_valor_hora_ops` with `(Nivel)` key
7. **UpsertInsumo** - Uses `eq_cost_insumos` with `(NSE, Tipo)` keys - **WRONG SCHEMA**
8. **UpsertEnvio** - Uses `eq_envio_tarifa` with `(Tipologia)` key - **TABLE MISSING**
9. **UpsertLocacion** - Uses `eq_locaciones` with `(Ciudad)` key
10. **UpsertMystery** - Uses `eq_tarifa_mystery` with `(TipoVisita, Complejidad)` keys - **TABLE MISSING**
11. **UpsertCodificacion** - Uses `eq_codificacion_param` with `(Escenario)` key - **TABLE MISSING**
12. **UpsertCostUnitario** - Uses `eq_cost_unitario_ops` with `(CodMatrix)` key - **TABLE MISSING**

---

**Generated:** January 12, 2026  
**Analysis Tool:** Semantic code analysis + direct entity inspection  
**Confidence Level:** HIGH (all sources directly examined)
