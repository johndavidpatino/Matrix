# QUICK REFERENCE: EQ Entity Mismatches & Corrections

**Purpose:** Quick lookup table for all column name and schema mismatches  
**Date:** January 12, 2026

---

## CRITICAL MISMATCHES TABLE

### 1. EqParamPrecio - COLUMN NAMES WRONG ⚠️ CRITICAL

| Item | Legacy SQL | Current Entity | Fixed In EF Service | Status |
|---|---|---|---|---|
| **Table/DbSet** | eq_param_precio | EqParamPrecios | ✓ EqParamPrecios | OK |
| **Column 1 (Key)** | MetodologiaCodigo | TipoMetodologia | ✓ Uses TipoMetodologia | ⚠️ NAME MISMATCH |
| **Column 2 (Key)** | PenetracionCodigo | PenetracionRango | ✓ Uses PenetracionRango | ⚠️ NAME MISMATCH |
| **Column 3 (Key)** | DuracionMin | DuracionMin | ✓ MATCH | OK |
| **Column 4** | ValorCoordinacion | ValorCoord | ✓ Uses ValorCoord | ⚠️ NAME MISMATCH |
| **Column 5** | ValorPerfil | ValorPerfil | ✓ MATCH | OK |
| **Column 6** | ValorTotal | ValorTotal | ✓ MATCH | OK |
| **UPSERT Logic** | MERGE on (MetodologiaCodigo, PenetracionCodigo, DuracionMin) | MERGE on (TipoMetodologia, PenetracionRango, DuracionMin) | ✓ Updated | FIXED |

**Impact:** Legacy SQL INSERT will fail on wrong column names

**Corrected Usage:**
```csharp
// Legacy code:
// INSERT INTO eq_param_precio (MetodologiaCodigo, PenetracionCodigo, ...)

// Corrected code:
var precio = new EqParamPrecio 
{ 
    TipoMetodologia = "F2F",           // Not MetodologiaCodigo!
    PenetracionRango = "75-82",         // Not PenetracionCodigo!
    DuracionMin = 30,
    ValorCoord = 20000,                 // Not ValorCoordinacion!
    ValorPerfil = 50000,
    ValorTotal = 150000
};
await service.UpsertPrecioAsync(precio);
```

---

### 2. EqValorHoraOps - SCHEMA STRUCTURE COMPLETELY DIFFERENT ⚠️ CRITICAL

| Item | Legacy SQL | Current Entity | Fixed In EF Service | Status |
|---|---|---|---|---|
| **Table/DbSet** | eq_valor_hora_ops | EqValorHoraOps | ✓ EqValorHoraOps | OK |
| **Column 1 (Key)** | Nivel | Nivel | ✓ MATCH | OK |
| **Column 2 (Key)** | Variante | Alternativa | ✓ Uses Alternativa | ⚠️ NAME MISMATCH |
| **Column 3 (Value)** | ValorHora (single) | BaseCostRate | ✓ Updated logic | ⚠️ STRUCTURE CHANGE |
| | | OverheadRate | ✓ Added support | ⚠️ STRUCTURE CHANGE |
| | | LoadedCostRate | ✓ Added support | ⚠️ STRUCTURE CHANGE |
| | | BillingRate | ✓ Added support | ⚠️ STRUCTURE CHANGE |
| **UPSERT Logic** | MERGE on (Nivel) - simple | MERGE on (Nivel, Alternativa) - compound | ✓ Updated | CHANGED |

**Impact:** Legacy UPSERT only updated on `Nivel` key, but new requires BOTH `Nivel` and `Alternativa`

**Corrected Usage:**
```csharp
// Legacy code:
// MERGE eq_valor_hora_ops t USING (SELECT @Nivel n) s ON t.Nivel=s.n
// INSERT (Nivel, Variante, ValorHora)

// Corrected code:
var valor = new EqValorHoraOps
{
    Nivel = "L5",
    Alternativa = "2024",  // Not Variante! AND must be compound key!
    BaseCostRate = 45000,  // Not ValorHora! NOW 4 rates instead of 1!
    OverheadRate = 15000,
    LoadedCostRate = 60000,
    BillingRate = 85000
};
await service.UpsertValorHoraOpsAsync(valor);
```

**CSV Import Format Change:**
```
Old (Legacy):
Nivel,Variante,ValorHora
L3,2023,35000
L3,2024,38000

New (Corrected):
Nivel,Alternativa,BaseCostRate,OverheadRate,LoadedCostRate,BillingRate
L3,2024,38000,12000,50000,72000
```

---

### 3. EqCostInsumos - SCHEMA FORMAT COMPLETELY DIFFERENT ⚠️ CRITICAL

| Item | Legacy SQL | Current Entity | Status |
|---|---|---|---|
| **Table/DbSet** | eq_cost_insumos | EqCostInsumos | OK |
| **Schema Type** | LONG format (rows per item) | WIDE format (columns per type) | ⚠️ INCOMPATIBLE |
| **Key Columns** | NSE + Tipo | NSE only | ⚠️ STRUCTURE CHANGE |

**Legacy LONG Format:**
```
NSE | Tipo                    | ValorUnitario
----|-------------------------|---------------
1   | Reclutamiento          | 50000
1   | Obsequio               | 10000
1   | Productividad          | 5000
1   | Supervisores           | 8000
2   | Reclutamiento          | 45000
2   | Obsequio               | 9000
```

**New WIDE Format:**
```
NSE | Reclutamiento | Obsequio | Productividad | Supervisores | Logistica | TransporteEncuestador | ...
----|---------------|----------|---------------|--------------|-----------|-----------------------|-----
1   | 50000         | 10000    | 5000          | 8000         | 3000      | 12000                 | ...
2   | 45000         | 9000     | 4500          | 7500         | 2800      | 11000                 | ...
```

**Impact:** MERGE logic is completely incompatible. Cannot use `(NSE, Tipo)` compound key with new schema.

**Corrected Usage:**
```csharp
// Legacy code (WRONG - won't work):
// MERGE eq_cost_insumos t USING (SELECT @NSE n,@Tipo t0) s 
// ON t.NSE=s.n AND t.Tipo=s.t0
// INSERT (NSE, Tipo, ValorUnitario)

// Corrected code:
var insumos = new EqCostInsumos
{
    NSE = 1,
    Reclutamiento = 50000,        // Individual columns per cost type
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

**CSV Import Format Change:**
```
Old (Legacy) - Multiple rows per NSE:
NSE,Tipo,ValorUnitario
1,Reclutamiento,50000
1,Obsequio,10000
1,Productividad,5000

New (Corrected) - One row per NSE:
NSE,Reclutamiento,Obsequio,Productividad,Dias,Supervisores,Logistica,TransporteEncuestador,TransporteSupervisor,ValorEnvio1erKilo,ValorKiloAdicional,SeguroPct,ValorMinDeclarar
1,50000,10000,5000,0,8000,3000,12000,15000,2500,500,0.15,100000
2,45000,9000,4500,0,7500,2800,11000,14000,2400,480,0.15,95000
```

---

### 4. EqParamScriptProc - PROPERTY NAME MISMATCH ⚠️ MEDIUM

| Item | Legacy SQL | Current Entity | Fixed In EF Service | Status |
|---|---|---|---|---|
| **SELECT Column** | HorasProcesamiento | HorasProc | ✓ Uses HorasProc | ⚠️ NAME MISMATCH |

**Impact:** Legacy SELECT will fail: Column 'HorasProcesamiento' not found

**Corrected Usage:**
```csharp
// Legacy query expected:
// SELECT DuracionMin, HorasScript, HorasProcesamiento, HorasHarmoni

// New entity property name:
var param = new EqParamScriptProc
{
    DuracionMin = 30,
    HorasScript = 2.5m,
    HorasProc = 1.5m,              // NOT HorasProcesamiento!
    HorasHarmoni = 0.5m,
    HorasGraficacion = 1.0m
};
await service.UpsertParamScriptProcAsync(param);
```

---

### 5. EqRateEstadistica - PROPERTY NAME MISMATCH ⚠️ MEDIUM

| Item | Legacy SQL | Current Entity | Fixed In EF Service | Status |
|---|---|---|---|---|
| **SELECT Column** | PrecioReferencia | PrecioRef2024 | ✓ Uses PrecioRef2024 | ⚠️ NAME MISMATCH |

**Impact:** Legacy SELECT will fail: Column 'PrecioReferencia' not found

**Corrected Usage:**
```csharp
// Legacy query expected:
// SELECT Categoria, Servicio, HorasEstimadas, PrecioReferencia, FactorEscala

// New entity property name:
var rate = new EqRateEstadistica
{
    Categoria = "Crosstabs",
    Servicio = "Tablas Cruzadas Bàsicas",
    HorasEstimadas = 4.0m,
    PrecioRef2024 = 250000,         // NOT PrecioReferencia!
    FactorEscala = 1.2m
};
await service.UpsertRateEstadisticaAsync(rate);
```

---

## MISSING ENTITIES (NOT IN NEW MODEL)

These exist in legacy SQL but have no EF Core entity:

| Table | Entity Model | DbSet | Status |
|---|---|---|---|
| eq_param_misc | ❌ MISSING | - | ⚠️ MUST CREATE |
| eq_envio_param | ❌ MISSING | - | ⚠️ MUST CREATE |
| eq_envio_tarifa | ❌ MISSING | - | ⚠️ MUST CREATE |
| eq_productividad_ciudad | ❌ MISSING | - | ⚠️ MUST CREATE |
| eq_codificacion_param | ❌ MISSING | - | ⚠️ MUST CREATE |
| eq_cost_unitario_ops | ❌ MISSING | - | ⚠️ MUST CREATE |
| eq_tarifa_mystery | ❌ MISSING | - | ⚠️ MUST CREATE |
| eq_cost_base_datos | ❌ MISSING | - | ⚠️ MUST CREATE |
| eq_param_factores | ❌ MISSING | - | ⚠️ MUST CREATE |
| eq_rate_horas | ❌ MISSING | - | ⚠️ MUST CREATE |

---

## MIGRATION CHECKLIST

### Column Renames Needed

- [ ] eq_param_precio: `MetodologiaCodigo` → `TipoMetodologia`
- [ ] eq_param_precio: `PenetracionCodigo` → `PenetracionRango`
- [ ] eq_param_precio: `ValorCoordinacion` → `ValorCoord`
- [ ] eq_valor_hora_ops: `Variante` → `Alternativa`
- [ ] eq_param_script_proc: Check SQL column name matches `HorasProc` (not `HorasProcesamiento`)
- [ ] eq_rate_estadistica: Check SQL column name matches `PrecioRef2024` (not `PrecioReferencia`)

### Schema Restructuring Needed

- [ ] eq_cost_insumos: Convert from LONG to WIDE format (add 12 new cost columns, remove `Tipo` column)
- [ ] eq_valor_hora_ops: Add 4 rate columns (BaseCostRate, OverheadRate, LoadedCostRate, BillingRate)

### Entity Models to Create

- [ ] EqParamMisc
- [ ] EqEnvioParam
- [ ] EqEnvioTarifa
- [ ] EqProductividadCiudad
- [ ] EqCodificacionParam
- [ ] EqCostUnitarioOps
- [ ] EqTarifaMystery
- [ ] EqCostBaseDatos
- [ ] EqParamFactores
- [ ] EqRateHoras

### DbContext Updates

- [ ] Register 10 new DbSet properties
- [ ] Configure OnModelCreating for new entities
- [ ] Add table/column mappings if names differ

### CSV Import Updates

- [ ] Update ImportPreciosCsv to use `TipoMetodologia, PenetracionRango`
- [ ] Update ImportValorHoraCsv to use `Alternativa` and handle 4 rates
- [ ] Update ImportCostInsumosAsync to handle WIDE format (one row per NSE)
- [ ] Test CSV imports with sample data

### Code Updates

- [ ] Replace `EasyQuoteAdminService` with `EasyQuoteAdminServiceEF`
- [ ] Update all controller methods to use async/await
- [ ] Update all raw SQL queries to use EF Core
- [ ] Add DI registration for new service
- [ ] Update error handling

---

## VALIDATION SQL QUERIES

Run these to validate current database state vs entity expectations:

```sql
-- Check EqParamPrecio columns
SELECT COLUMN_NAME, DATA_TYPE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'eq_param_precio' 
ORDER BY ORDINAL_POSITION;

-- Check EqValorHoraOps columns
SELECT COLUMN_NAME, DATA_TYPE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'eq_valor_hora_ops' 
ORDER BY ORDINAL_POSITION;

-- Check EqCostInsumos columns
SELECT COLUMN_NAME, DATA_TYPE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'eq_cost_insumos' 
ORDER BY ORDINAL_POSITION;

-- Verify missing tables
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME LIKE 'eq_%' 
ORDER BY TABLE_NAME;
```

Expected output should match the "Current Entity" columns in tables above.

---

## ROLLBACK COMMANDS

If you need to rollback migrations:

```bash
# See all migrations
dotnet ef migrations list

# Revert to previous migration
dotnet ef database update PreviousMigrationName

# Remove last migration (careful!)
dotnet ef migrations remove
```

---

**Document Version:** 1.0  
**Created:** January 12, 2026  
**Last Updated:** January 12, 2026
