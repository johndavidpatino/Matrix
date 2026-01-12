# SPRINT 8 KICKOFF: EQ_EasyQuote Fase 1

**Fecha**: 2026-01-12  
**Duración estimada**: 2-3 semanas (Inicio: 2026-03-01, Cierre: 2026-03-19)  
**Esfuerzo**: 120 horas (70h análisis + 50h implementación)  
**Prioridad**: 🔴🔴 CRÍTICA  
**Status**: 🔄 EN PLANIFICACIÓN

---

## 📋 OBJETIVOS

### Semana 1: ANÁLISIS COMPLETO
- ✅ Completar inventario de formularios, WebMethods, lógica de negocio de EasyQuote
- ✅ Documentar mapeo Excel → modelo de datos → API
- ✅ Identificar patrones complejos (matrices de precios, simulador, cálculos)
- ✅ Definir arquitectura REST y DTOs
- ✅ Crear backlog desglosado Fases 2-4

### Semana 1-2: IMPLEMENTACIÓN FASE 1
- ✅ Crear infraestructura (adapters, services base, DI)
- ✅ Implementar tablas maestras/catálogos
- ✅ CRUD básico de presupuestos (sin alternativas)
- ✅ Vistas iniciales (Index, Create/Edit)
- ✅ Unit tests básicos

### CRITERIO DE TERMINADO
- 📊 Documento `ANALISIS_EQ_EASYQUOTE.md` completo
- 🟡 Fase 1 compilada y pasando QA básica
- Backlog Fases 2-4 definido con t-shirt sizing
- Build verde sin errores
- Commit documentado

---

## 🗂️ DOCUMENTACIÓN BASE

| Documento | Propósito | Ubicación |
|-----------|----------|----------|
| **ANALISIS_EASYQUOTE.md** | Inventario completo, mapeos, formulas | `docs/EQ/` ✅ YA EXISTE |
| **MIGRACION_EQ_IMPLEMENTACION.md** | Plan técnico de tablas/SP/services | `docs/EQ/` ✅ YA EXISTE |
| **EQ_SCHEMA.sql** | Script SQL de schema objetivo | `docs/EQ/` ✅ YA EXISTE |
| **EQ_EXTRACCION_SEEDS_EXCEL.md** | Cómo extraer datos maestros de Excel | `docs/EQ/` ✅ YA EXISTE |
| **TODO_EQ_MIGRACION_PRIORIZADO.md** | Backlog priorizado de Sprint 8-12 | `docs/EQ/` ✅ YA EXISTE |
| **EQ_RESUMEN_EJECUTIVO_STAKEHOLDERS.md** | Stakeholder summary | `docs/EQ/` ✅ YA EXISTE |

**📌 ACCIÓN INMEDIATA**: Leer documentos en orden: ANALISIS → MIGRACION → SCHEMA → EXTRACCION → TODO

---

## 💾 TAREAS SEMANA 1 (ANÁLISIS)

### T1.1: Validar documentación existente (1h)
- [ ] Verificar que `ANALISIS_EASYQUOTE.md` cubre inventario completo
- [ ] Confirmar mapeos diccionario de datos
- [ ] Revisar patrones complejos identificados (presupuestos, alternativas, simulador)

### T1.2: Definir modelo de datos SQL (4h)
- [ ] Crear script EF Core migrations para tablas principales:
  - `eq_quote_header` (propuesta, cliente, SL, metodologias)
  - `eq_questionnaire` (duracion, penetracion, flags)
  - `eq_methodology` (tecnicas, base datos)
  - `eq_sample_city` (ciudades, NSE)
  - `eq_mystery` (visitas, olas)
- [ ] Crear tablas maestras (seeds):
  - `eq_param_precio` (F2F/CATI/ONLINE/AUTO matrices)
  - `eq_param_script_proc` (horas por duracion)
  - `eq_valor_hora_ops` (tarifas por nivel)
  - `eq_cost_insumos` (reclutamiento, obsequios, locaciones, envios)
  - `eq_rate_estadistica` (servicios adicionales)
  - `eq_locaciones` (ciudades y tarifas)
- [ ] Validar PK/FK y relaciones con `CU_Cuentas`

### T1.3: Definir DTOs y API endpoints (3h)
- [ ] Crear DTOs para cada tabla
- [ ] Especificar endpoints REST:
  - `POST /api/easyquote` (crear cotización)
  - `PUT /api/easyquote/{id}` (actualizar)
  - `GET /api/easyquote/{id}` (obtener)
  - `POST /api/easyquote/{id}/calculate` (calcular costos)
  - `GET /api/easyquote/{id}/breakdown` (resumen de rubros)
- [ ] Documentar formatos de entrada/salida

### T1.4: Planificar cálculos de costos (2h)
- [ ] Mapear formulas Excel → C# (referencias pseudocodigo sec. 5.1 ANALISIS)
- [ ] Identificar dependencias entre cálculos
- [ ] Definir precision numerica y manejo de redondeos (ROUNDUP dias campo)

### T1.5: Crear backlog Fases 2-4 (3h)
- [ ] **Fase 2**: Alternativas y simulador (estimado 60h)
- [ ] **Fase 3**: Aprobaciones y auditoria (estimado 40h)
- [ ] **Fase 4**: Reportes y exportes (estimado 50h)
- [ ] T-shirt sizing para cada tarea

---

## 💻 TAREAS SEMANA 1-2 (IMPLEMENTACIÓN FASE 1)

### T2.1: Crear proyecto EQ_Services (3h)
- [ ] Estructura: `MatrixNext.Web/Services/EQ/`
- [ ] Interfaces:
  - `IEasyQuoteService` (CRUD cotizaciones)
  - `IEasyCostService` (motor de cálculos)
  - `IEasyMasterService` (gestión catálogos)
- [ ] Implementaciones base
- [ ] DI en `Program.cs`

### T2.2: Implementar tablas maestras y seeds (8h)
- [ ] EF Core models (16 tablas) en `MatrixNext.Web/Models/EQ/`
- [ ] DbContext mappings
- [ ] Migration inicial
- [ ] Seed data desde CSV/JSON (extraer de Excel según `EQ_EXTRACCION_SEEDS_EXCEL.md`):
  - Matrices de precios (F2F/CATI/ONLINE/AUTO)
  - Horas y script/proc por duracion
  - Tarifas por nivel (L3-L7)
  - Costos por NSE (reclutamiento, obsequios)
  - Locaciones y ciudades
  - Servicios estadistica
  - Envios y transportes

### T2.3: Implementar CRUD Presupuestos (6h)
- [ ] `EasyQuoteService.CreateAsync(dto)` - create cotización header
- [ ] `EasyQuoteService.GetAsync(id)` - obtain cotización completa
- [ ] `EasyQuoteService.UpdateAsync(id, dto)` - modify
- [ ] `EasyQuoteService.ListAsync(filters)` - list con paginación
- [ ] Entity configs y validaciones
- [ ] Unit tests (xUnit + EF InMemory)

### T2.4: Implementar motor de cálculos (10h)
- [ ] `EasyCostService.CalculateAsync(quoteId)` - motor principal
- [ ] Subrutinas de cálculo:
  - Valor encuesta por metodologia/penetracion
  - Costo campo (F2F, CATI, Online, Auto, proveedores externos)
  - Reclutamiento e incentivos
  - Transporte (encuestadores, supervisores, bebidas, ninos)
  - Locaciones y refrigeracion
  - Staff OPS (scripting, procesamiento, datacleaning, estadistica)
  - Márgenes (GM, PB+RMF, OP)
- [ ] Manejo de edge cases (division por cero, valores nulos)
- [ ] Tests para casos base + variantes

### T2.5: Crear controller API (3h)
- [ ] `EasyQuoteController` en `MatrixNext.Web/Controllers/`
- [ ] Endpoints: POST/GET/PUT con [Authorize]
- [ ] Responses con ApiResponse<T>
- [ ] Validaciones y mapeos DTO

### T2.6: Crear vistas Razor iniciales (5h)
- [ ] Area `Areas/EQ/EasyQuote/`
- [ ] Views:
  - `Index.cshtml` - lista cotizaciones
  - `Create.cshtml` - formulario wizard (paso 1: datos generales)
  - `_FormGeneral.cshtml` - partial generales
- [ ] CSS/JS básico (tablas, modales)
- [ ] Sidebar entry "EasyQuote" en `_Sidebar.cshtml`

### T2.7: Menu e integraciones (2h)
- [ ] Agregar "EasyQuote" en sidebar bajo Proyectos
- [ ] Validar permisos y roles
- [ ] Links contextuales a CU_Cuentas si aplica

### T2.8: Documentación y tests (4h)
- [ ] Documentar DTOs y mappings
- [ ] Unit tests EF InMemory para CreateAsync, CalculateAsync (casos base)
- [ ] Documento `MIGRACION_EQ_FASE_1_COMPLETADA.md`

---

## 📁 ARCHIVOS A CREAR/MODIFICAR

### Nuevos Archivos
```
MatrixNext.Web/
├── Models/EQ/
│   ├── EqQuoteHeader.cs
│   ├── EqQuestionnaire.cs
│   ├── EqMethodology.cs
│   ├── EqSampleCity.cs
│   ├── EqMystery.cs
│   ├── EqStaffSL.cs
│   ├── EqParamPrecio.cs
│   ├── EqParamScriptProc.cs
│   ├── EqValorHoraOps.cs
│   ├── EqCostInsumos.cs
│   ├── EqRateEstadistica.cs
│   ├── EqLocaciones.cs
│   └── ... (16 tablas total)
├── Services/EQ/
│   ├── IEasyQuoteService.cs
│   ├── IEasyCostService.cs
│   ├── IEasyMasterService.cs
│   ├── EasyQuoteService.cs
│   ├── EasyCostService.cs
│   └── EasyMasterService.cs
├── DTOs/
│   ├── EasyQuoteDtos.cs
│   ├── EasyCostDtos.cs
│   └── EasyMasterDtos.cs
├── Controllers/
│   └── EasyQuoteController.cs
├── Areas/EQ/EasyQuote/
│   ├── Views/
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   └── _FormGeneral.cshtml
│   └── Controllers/
│       └── EasyQuoteController.cs (UI)
├── Migrations/
│   └── [DateTimeStamp]_AddEasyQuoteTables.cs

MatrixNext.Tests.Unit/
├── EQ/
│   ├── EasyQuoteServiceTests.cs
│   ├── EasyCostServiceTests.cs
│   └── EasyCostCalculationTests.cs

docs/EQ/
├── MIGRACION_EQ_FASE_1_COMPLETADA.md (nuevo al cierre)
└── BACKLOG_FASES_2_4.md (nuevo)
```

### Modificados
```
MatrixNext.Web/
├── Program.cs (DI: AddScoped<IEasyQuoteService>, etc.)
├── MatrixDbContext.cs (DbSets para 16 tablas)
├── Views/Shared/layouts/_main-sidebar.cshtml (menu entry)

docs/
├── README_SPRINTS_5_12.md (actualizar Sprint 8 status)
├── MODULOS_MIGRACION.md (agregar EQ)
├── DASHBOARD_MIGRACION.md (columna Sprint 8)
```

---

## 🎯 CHECKLIST DE VALIDACIÓN (EOD)

- [ ] Build exitoso: `dotnet build`
- [ ] Migrations createadas y aplicables
- [ ] Unit tests compilando y algunos pasando (casos base)
- [ ] DTOs y controllers con respuestas ApiResponse<T>
- [ ] Sidebar menu "EasyQuote" visible (no rotos otros menus)
- [ ] Documentación técnica actualizada
- [ ] TODO_EQ_MIGRACION_PRIORIZADO.md reflejando Fases 2-4

---

## 🚨 RIESGOS IDENTIFICADOS

1. **Complejidad de cálculos**: Múltiples matrices y formulas interdependientes → Validar contra Excel celda a celda
2. **Tablas maestras desactualizadas**: Tarifas, GM, OP pueden cambiar → Versionar en seed data
3. **Precision numerica**: Redondeos (ROUNDUP, divisiones) → Usar `decimal` (precisión 28-29 dígitos)
4. **Dependencias en Excel**: Si Excel se activa durante desarrollo, riesgo de inconsistencia → Mantener ANALISIS actualizado
5. **UX de captura**: Formulario wizard puede ser complejo → Diseñar paso a paso, validar en cada paso

---

## 📞 CONTACTOS Y REFERENCIAS

- **Product Owner**: [TBD]
- **QA Lead**: [TBD]
- **Documentación base**: `docs/EQ/ANALISIS_EASYQUOTE.md`
- **Plan maestro**: `PLAN_EJECUCION_SPRINTS_5_12.md` (Sección Sprint 8)
- **Dashboard**: `DASHBOARD_MIGRACION.md`

---

## 📅 PRÓXIMOS HITOS

| Fecha | Milestone | Responsable |
|-------|-----------|-------------|
| 2026-01-15 | Análisis completado (ANALISIS_EQ_EASYQUOTE.md validado) | Dev |
| 2026-02-15 | Infraestructura + seeds (tablas maestras cargadas) | Dev |
| 2026-02-22 | CRUD + motor cálculos (calculate endpoint funcional) | Dev |
| 2026-02-28 | Vistas iniciales + tests | Dev |
| 2026-03-01 | Fase 1 completada + commit | Dev |
| 2026-03-19 | Sprint 8 cerrado (backlog Fases 2-4 definido) | All |

---

**Estado**: 🔄 READY TO START  
**Autorizado por**: [TBD]  
**Fecha de creación**: 2026-01-12  
**Última actualización**: 2026-01-12
