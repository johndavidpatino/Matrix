# ANÁLISIS EXHAUSTIVO - SPRINT 8 INICIO

**Fecha**: 2026-01-12  
**Estado**: Verificación completa de código existente vs. nuevos modelos  
**Objetivo**: Evitar duplicaciones y definir ruta operativa real

---

## 📊 INVENTARIO ACTUAL - LO QUE YA EXISTE

### ✅ DOCUMENTACIÓN (COMPLETA)
| Documento | Ubicación | Estado | Notas |
|-----------|----------|--------|-------|
| ANALISIS_EASYQUOTE.md | docs/EQ/ | ✅ 311 líneas | Análisis técnico completo |
| MIGRACION_EQ_IMPLEMENTACION.md | docs/EQ/ | ✅ 1,663 líneas | **CRÍTICO**: Indica 40% calculadora, 70% seeds |
| TODO_EQ_MIGRACION_PRIORIZADO.md | docs/EQ/ | ✅ 751 líneas | Backlog priorizado, BLOQUEADORES RESUELTOS |
| EQ_EXTRACCION_SEEDS_EXCEL.md | docs/EQ/ | ✅ 510 líneas | Guía de extracción de datos maestros |
| Inventario Fórmulas Excel | docs/EQ/ | ✅ 5,500+ fórmulas documentadas | |
| SESION_RESUMEN_2026_01_05.md | docs/EQ/ | ✅ Exists | Resumen de sesión anterior |

**DECISIÓN**: NO VOLVER A ANALIZAR. Usar documentación existente como verdad única.

### ✅ MODELOS EF CORE (13 TABLAS NUEVAS CREADAS HOY)

**Ubicación**: `MatrixNext.Web/Models/EQ/`

```
✅ EqQuoteHeader.cs           - Cotización principal
✅ EqQuestionnaire.cs         - Cuestionario y procesos
✅ EqMethodology.cs           - Técnicas recolección
✅ EqSampleCity.cs            - Muestra por NSE
✅ EqMystery.cs               - Visitas mystery/shopper
✅ EqStaffSL.cs               - Staff por nivel
✅ EqParamPrecio.cs           - Matrices precios (maestras)
✅ EqParamScriptProc.cs       - Horas script/proc (maestras)
✅ EqValorHoraOps.cs          - Tarifas nivel OPS (maestras)
✅ EqCostInsumos.cs           - Costos insumos (maestras)
✅ EqRateEstadistica.cs       - Servicios estadística (maestras)
✅ EqLocaciones.cs            - Tarifas ciudades (maestras)
✅ EqCostResult.cs            - Resultados cálculos
```

**PROBLEMA**: No están mapeados en `MatrixDbContext` aún.

### ✅ VIEWMODELS (DUPLICADOS - YA EXISTEN EN AREAS/EQ)

**Ubicación**: `MatrixNext.Web/Areas/EQ/Models/`

```
✅ EasyQuoteViewModel.cs      - MAIN viewmodel (no es EF entity)
✅ EasyQuoteAdminViewModel.cs - Admin viewmodel
```

Estos tienen estructuras SIMILARES pero NO IDÉNTICAS a los nuevos Eq* models creados hoy.

**ANÁLISIS DE DUPLICACIÓN**:
| Entidad Nueva | ViewModel Existente | Status |
|---|---|---|
| EqQuoteHeader | EQHeader | Duplicado - mapeo necesario |
| EqQuestionnaire | EQQuestionnaire | Duplicado - mapeo necesario |
| EqMethodology | EQMethodology | Duplicado - mapeo necesario |
| EqSampleCity | EQSampleCity | Duplicado - mapeo necesario |
| EqStaffSL | EQStaffSL | Duplicado - mapeo necesario |
| EqParamPrecio, EqParamScriptProc, etc. | N/A | NUEVAS - OK |

### ✅ SERVICIOS (EXISTEN PERO INCOMPLETOS)

**Ubicación**: `MatrixNext.Web/Areas/EQ/Services/`

```
✅ EasyQuoteService.cs              - Básico (CargarQuote, Guardar, Calcular)
✅ EasyQuoteAdminService.cs         - Admin parametros
✅ QuoteCalculator.cs               - Motor cálculos (40% según docs)
✅ EasyQuoteAdapter.cs              - Adapter para Dapper/TVP
✅ EasyQuoteMasterService.cs        - Maestras cache
```

**ESTADO**:
- `QuoteCalculator.cs`: 26 fórmulas implementadas de ~52 identificadas (50% approx)
- `EasyQuoteAdapter.cs`: Usa Dapper/TVP (NOT EF Core) → **CONFLICTO con nuevos Eq* models**
- `EasyQuoteMasterService.cs`: Cache de maestras en memoria, busca en SQL

### ✅ CONTROLLERS (EXISTEN)

**Ubicación**: `MatrixNext.Web/Areas/EQ/Controllers/`

```
✅ EasyQuoteController.cs       - Main (Index, Guardar, Calcular)
✅ EasyQuoteAdminController.cs  - Admin (CRUD parametros)
✅ MaestrasAdminController.cs   - Maestras (read-only)
```

### ✅ VISTAS (EXISTEN)

**Ubicación**: `MatrixNext.Web/Areas/EQ/Views/`

```
✅ EasyQuote/Index.cshtml              - Main form con tabs/grids
✅ EasyQuoteAdmin/Parametros.cshtml    - Admin UI para maestras
✅ MaestrasAdmin/Index.cshtml          - Maestras list
✅ MaestrasAdmin/Tabla.cshtml          - Maestras tabla editable
```

### ✅ DTOs (CREADOS HOY - NUEVOS)

**Ubicación**: `MatrixNext.Web/DTOs/EasyQuoteDtos.cs`

```
✅ EasyQuoteCreateDto           - Para API
✅ EasyQuoteUpdateDto           - Para API
✅ EasyQuoteDetailDto           - Para API
✅ EasyCostResultDto            - Para API
✅ + 15 más DTOs maestras/detalle
```

### ✅ INTERFACES (CREADAS HOY - NUEVAS)

**Ubicación**: `MatrixNext.Web/Services/EQ/IEasServices.cs`

```
✅ IEasyQuoteService            - CRUD cotizaciones
✅ IEasyCostService             - Cálculos
✅ IEasyMasterService           - Maestras
```

### ❌ FALTA: DbContext DbSets para Eq* models

Los nuevos Eq* models NO están en `MatrixDbContext`.

---

## 🔄 CONFLICTO ARQUITECTÓNICO IDENTIFICADO

### ESCENARIO 1: Usar AREAS/EQ con Dapper (ACTUAL)
```
Controllers/Services en Areas/EQ
├── Usan Dapper + TVP (legacy)
├── ViewModels: EQHeader, EQQuestionnaire, etc.
├── NOT using EF Core
└── Funciona (40% calculadora, 70% seeds OK)
```

### ESCENARIO 2: Nuevo enfoque con EF Core (HOY CREADO)
```
Models/EQ con EF Core entities
├── Eq* models creados hoy
├── DTOs para API (EasyQuoteDtos.cs)
├── Interfaces servicios (IEasServices.cs)
├── Requires: DbContext mappings + migrations
└── NOT YET INTEGRATED con Areas/EQ
```

### ⚠️ PROBLEMA
- **Dos implementaciones paralelas** sin coordinación
- **Areas/EQ sigue usando Dapper** (legacy)
- **Nuevos Eq* models en Models/EQ** (EF Core, sin DbContext)
- **DTOs y interfaces nuevas** (sin servicios implementados)

---

## 📋 DECISIÓN ARQUITECTÓNICA REQUERIDA

### OPCIÓN A: Completar Implementación EF Core (RECOMENDADO)
```
1. Mapear Eq* models en MatrixDbContext
2. Crear migration Add_EQ_Tables
3. Implementar servicios EF Core (IEasyQuoteService, etc.)
4. Migrar Areas/EQ/Controllers a usar nuevos servicios
5. DEPRECATE EasyQuoteAdapter (Dapper)
6. Resultado: EF Core uniforme + EasyQuote funcional 100%
Esfuerzo: ~40-50h (incluye testing paridad vs Excel)
```

### OPCIÓN B: Mantener Dapper existente (RÁPIDO PERO TÉCNICA DEUDA)
```
1. IGNORAR nuevos Eq* models
2. Completar QuoteCalculator (26 fórmulas restantes)
3. Extraer seeds reales de Excel
4. Implementar testing paridad Excel
5. DEJAR Areas/EQ como está (Dapper + TVP)
6. LIMPIAR Models/EQ (no mapear, son orphans)
Esfuerzo: ~20-30h (más rápido pero menor calidad)
```

### ✅ RECOMENDACIÓN TÉCNICA
**OPCIÓN A** es mejor para:
- Código mantenible a largo plazo
- Uniforme con resto de MatrixNext (todo EF Core)
- Testing más fácil (EF InMemory)
- Performance queries transparente

---

## 🎯 PLAN OPERATIVO REAL PARA SPRINT 8

### FASE 1A: DECISIÓN + LIMPIEZA (2h)
- [ ] Confirmar OPCIÓN A (completar EF Core)
- [ ] Verificar que NO hay dependencias críticas en Dapper
- [ ] Hacer backup de Areas/EQ actual (git tag)

### FASE 1B: MAPEOS EF CORE (4h)
- [ ] Agregar DbSets para 13 Eq* models en MatrixDbContext
- [ ] Configurar relaciones PK/FK
- [ ] Crear migration: `Add_EQ_Entities`
- [ ] Validar migration

### FASE 1C: SERVICIOS EF CORE (12h)
- [ ] Implementar `EasyQuoteService` (CRUD completo + EF)
- [ ] Implementar `EasyCostService` (motor cálculos mejorado)
- [ ] Implementar `EasyMasterService` (queries sobre maestras)
- [ ] DI registration en Program.cs
- [ ] Unit tests (EF InMemory)

### FASE 2: SEEDS REALES (8h)
- [ ] Extraer CSV desde Excel (Parametros, CATI, Online, Horas, etc.)
- [ ] Crear seed configuration (OnModelCreating)
- [ ] Validar consistencia con Excel
- [ ] IMPORTANTE: Usar datos reales, no placeholders

### FASE 3: CALCULADORA COMPLETA (24h)
- [ ] Implementar 26 fórmulas faltantes (parafiscales, CATI, Online, mystery, insumos, transporte, etc.)
- [ ] Testing paridad vs Excel (celda a celda)
- [ ] Casos edge (division por cero, valores nulos, redondeos)
- [ ] QuoteCalculator.cs actualizado

### FASE 4: MIGRATION DE CONTROLLERS (6h)
- [ ] Actualizar Areas/EQ/Controllers para usar nuevos servicios
- [ ] Mapeo EasyQuoteViewModel → Eq* models
- [ ] ApiResponse wrapper
- [ ] Testing integración

### FASE 5: VISTAS + FINALES (8h)
- [ ] Actualizar Index.cshtml para nuevo flujo
- [ ] Admin UI para maestras (si no está lista)
- [ ] QA end-to-end
- [ ] Documentación

**TOTAL**: ~64h (dentro de 120h estimadas)

---

## 🔍 VALIDACIÓN PRE-SPRINT

- [ ] Excel Ipsos EasyQuote 2025v2.xlsm disponible ✅
- [ ] CSVs en carpeta CSV/ disponibles ✅
- [ ] ANALISIS_EASYQUOTE.md accesible ✅
- [ ] Build actual: `dotnet build` exitoso ✅
- [ ] No cambios pendientes en git ✅

---

## 📌 RECOMENDACIONES FINALES

1. **NO duplicar trabajo**: Areas/EQ ya tiene UI y lógica. Mantener donde sea posible.
2. **Usar EF Core uniformemente**: MatrixNext es 100% EF Core. Mantener consistencia.
3. **Validación es CRÍTICA**: Testing paridad vs Excel es bloqueador. No saltear.
4. **Datos reales AHORA**: Usar Excel real desde el inicio, no placeholders.
5. **Documentar decisiones**: Actualizar TODO_EQ_MIGRACION_PRIORIZADO.md con lo decidido.

---

**Próximo paso**: Confirmar OPCIÓN A con usuario, luego iniciar FASE 1A (decisión + limpieza).

**Tiempo estimado para confirmación**: 5 minutos  
**Bloqueador actual**: Decisión arquitectónica (Dapper vs. EF Core)
