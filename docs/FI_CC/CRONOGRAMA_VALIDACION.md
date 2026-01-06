# CRONOGRAMA DETALLADO Y CHECKLIST DE VALIDACIÓN

**Objetivo**: Plan semanal completo + checklist de Go/No-Go para cada sprint
**Período**: 13 semanas, 784 horas
**Estado**: ✅ Sprint Pre-1 COMPLETADO (Enero 6, 2026)

---

## FASE 0: PRE-PROYECTO (Completado ✅)

```
✅ Análisis de FI module (28 páginas)
✅ Mapeo de dependencias (CU_Presupuesto, CC_FinzOpe)
✅ Decisión de incluir CC_FinzOpe como Sprint Pre-1
✅ Documentación completa
   ├─ MIGRACION_FI_ADMINISTRATIVO.md
   ├─ PLAN_SPRINT_PRE1_CC_FINZOPE.md
   ├─ PLAN_SPRINTS_1_6_FI.md
   ├─ PATRONES_ARQUITECTURA_FI.md
   └─ Este documento (CRONOGRAMA_VALIDACION.md)
✅ Alineación con stakeholders
```

---

## SPRINT PRE-1: CC_FinzOpe (Semanas 1-2, 80 horas)

### Cronograma Semanal

**SEMANA 1: Análisis e Infraestructura** (40 horas)

```
Lunes (8h)
├─ 09:00-11:00  Tarea 1.1: Análisis SQL Server
│                 • Listar tablas CC_*
│                 • Documentar columnas, tipos, FK
│                 • Revisar índices
├─ 11:00-13:00  (Continuación Tarea 1.1)
├─ 14:00-17:00  Tarea 1.2: Mapeo SP (inicio)
│                 • Listar SP CC_*
│                 • Documentar parámetros
└─ Status: 40% Tarea 1, 20% Tarea 2

Martes (8h)
├─ 09:00-12:00  Tarea 1.2: Mapeo SP (continuación)
├─ 12:00-13:00  Documentación
├─ 14:00-17:00  Tarea 3: Crear DbContext (inicio)
│                 • Instalar paquetes NuGet
│                 • Scaffold desde SQL
└─ Status: Tarea 1 completada (100%), Tarea 2 completada (100%), Tarea 3 20%

Miércoles (8h)
├─ 09:00-13:00  Tarea 3: DbContext (continuación)
│                 • Revisar modelos generados
│                 • Ajustar OnModelCreating
├─ 14:00-17:00  Tarea 4: DTOs (inicio)
│                 • Crear primera clase DTO (ProduccionResultDTO)
│                 • Crear segunda clase DTO (LiquidacionResultDTO)
└─ Status: Tarea 3 50%, Tarea 4 20%

Jueves (8h)
├─ 09:00-13:00  Tarea 3: DbContext (final)
│                 • Migration inicial
│                 • Validación
├─ 14:00-17:00  Tarea 4: DTOs (continuación)
│                 • Crear DTOs restantes (6 más)
└─ Status: Tarea 3 completada (100%), Tarea 4 80%

Viernes (8h)
├─ 09:00-12:00  Tarea 4: DTOs (final)
│                 • Validar todas las clases
│                 • Documentación
├─ 12:00-13:00  Testing preliminar
├─ 14:00-17:00  Documentación Tarea 1-2
│                 • ANALISIS_CC_FINZOPE_TABLAS.md
│                 • MAPEO_SP_CC_FINZOPE.md
└─ Status: Semana 1 completada 95%

RESUMEN SEMANA 1:
├─ Tarea 1: ✅ 100% (8h)
├─ Tarea 2: ✅ 100% (8h)
├─ Tarea 3: ✅ 100% (12h)
├─ Tarea 4: ✅ 100% (8h)
├─ Documentación: ✅ 90% (4h)
└─ TOTAL: 40 horas, 5 tareas iniciadas
```

**SEMANA 2: Adapter, Service y Testing** (40 horas)

```
Lunes (8h)
├─ 09:00-13:00  Tarea 5: Adapter (inicio)
│                 • Implementar ObtenerProduccion()
│                 • Implementar ObtenerPST()
├─ 14:00-17:00  (Continuación Tarea 5)
└─ Status: Tarea 5 20%

Martes (8h)
├─ 09:00-13:00  Tarea 5: Adapter (continuación)
│                 • Implementar métodos Dapper para SP complejas
│                 • LiquidarPlanillas, GenerarBonificacion
├─ 14:00-17:00  (Continuación)
└─ Status: Tarea 5 50%

Miércoles (8h)
├─ 09:00-13:00  Tarea 5: Adapter (continuación)
│                 • Completar métodos restantes (15+)
├─ 14:00-17:00  Tarea 6: Service (inicio)
│                 • Crear CCFinzOpeService
│                 • Implementar métodos básicos
└─ Status: Tarea 5 80%, Tarea 6 20%

Jueves (8h)
├─ 09:00-13:00  Tarea 5: Adapter (final)
│                 • Validación
├─ 14:00-17:00  Tarea 6: Service (continuación)
│                 • Validaciones, logging
│                 • Manejo de errores
└─ Status: Tarea 5 100%, Tarea 6 50%

Viernes (8h)
├─ 09:00-12:00  Tarea 6: Service (final)
│                 • Completar todos los métodos
│                 • DI registration
├─ 12:00-13:00  Tarea 7: Testing
│                 • Testing manual de SP
├─ 14:00-17:00  Documentación final
│                 • VALIDACION_CC_FINZOPE_TESTING.md
│                 • Checklist completado
└─ Status: Semana 2 completada 100%

RESUMEN SEMANA 2:
├─ Tarea 5: ✅ 100% (20h)
├─ Tarea 6: ✅ 100% (12h)
├─ Tarea 7: ✅ 100% (8h)
├─ Documentación: ✅ 100% (4h)
└─ TOTAL: 40 horas, Sprint Pre-1 ✅ COMPLETADO (Enero 6, 2026)

ENTREGABLES SPRINT PRE-1 ✅:
✅ CcLiquidacionDto.cs (60 líneas, 8 clases DTO)
✅ CcFinzOpeAdapter.cs (100 líneas, 20+ métodos Dapper)
✅ CcFinzOpeService.cs (41 líneas, orquestación)
✅ ServiceCollectionExtensions.cs (32 líneas, DI)
✅ CcFinzOpeController.cs (102 líneas, API + Web)
✅ CcFinzOpe/Index.cshtml (350+ líneas, Bootstrap 5 UI)
✅ CcFinzOpeViewModel.cs (ViewModels)
✅ _ViewImports.cshtml (Razor imports)
✅ Program.cs - Registración de servicios
✅ Documentación actualizada

**Total**: 10 archivos, ~800 líneas de código
**Status**: ✅ LISTO PARA SPRINT 1
```

---

## SPRINT 1: Control Presupuestos (Semanas 3-4, 92 horas)

### Cronograma Semanal

**SEMANA 3: Models, Controllers, Views** (40 horas)

```
Lunes (8h)
├─ 09:00-13:00  Setup de proyecto
│                 • Crear carpeta Area/FI
│                 • Crear Controllers, Services, Data folders
│                 • Setup scaffolding
├─ 14:00-17:00  Tarea 1.1: Models + DTOs
│                 • Crear FI_Presupuesto.cs
│                 • Crear FI_DetallePresupuesto.cs
└─ Status: Tarea 1.1 20%

Martes (8h)
├─ 09:00-13:00  Tarea 1.1: Models (completar DTOs)
│                 • PresupuestoDTO
│                 • DetallePresupuestoDTO
├─ 14:00-17:00  Tarea 1.2: Controller (inicio)
│                 • ControlPresupuestosController clase
│                 • GET Index()
│                 • POST GetPresupuestos() AJAX
└─ Status: Tarea 1.1 100%, Tarea 1.2 20%

Miércoles (8h)
├─ 09:00-13:00  Tarea 1.2: Controller (continuación)
│                 • POST Guardar()
│                 • POST Eliminar()
│                 • GET Exportar()
├─ 14:00-17:00  Tarea 1.2: Views (inicio)
│                 • Index.cshtml (grid)
│                 • Index.js (DataTable, AJAX)
└─ Status: Tarea 1.2 60%

Jueves (8h)
├─ 09:00-13:00  Tarea 1.2: Views (continuación)
│                 • Modal CRUD
│                 • Modales secundarias
├─ 14:00-17:00  Testing inicial
│                 • Compilación
│                 • Index GET
│                 • Grid GET sin datos
└─ Status: Tarea 1.2 90%

Viernes (8h)
├─ 09:00-12:00  Tarea 1.2: Views (final polish)
│                 • CSS refinado
│                 • Validaciones cliente
├─ 12:00-13:00  Tarea 1.3: Service (inicio)
│                 • IPresupuestoService interface
├─ 14:00-17:00  Tarea 1.3: Service (continuación)
│                 • ObtenerPresupuestos()
│                 • GuardarPresupuesto()
└─ Status: Tarea 1.3 30%

RESUMEN SEMANA 3:
├─ Tarea 1.1: ✅ 100% (8h)
├─ Tarea 1.2: ✅ 100% (24h)
├─ Tarea 1.3: 30% (4h)
└─ TOTAL: 40 horas
```

**SEMANA 4: Services, Adapters, Testing** (52 horas)

```
Lunes (8h)
├─ 09:00-13:00  Tarea 1.3: Service (completar)
│                 • Guardar, Eliminar, Export
│                 • Validaciones de negocio
├─ 14:00-17:00  Tarea 1.4: Adapter (inicio)
│                 • FIControlPresupuestosAdapter
│                 • ObtenerPresupuestos()
└─ Status: Tarea 1.3 100%, Tarea 1.4 20%

Martes (8h)
├─ 09:00-13:00  Tarea 1.4: Adapter (continuación)
│                 • GuardarPresupuesto()
│                 • EliminarPresupuesto()
├─ 14:00-17:00  Testing de Adapter
│                 • Tests unitarios
│                 • Mocks de CCFinzOpeAdapter
└─ Status: Tarea 1.4 60%

Miércoles (8h)
├─ 09:00-13:00  Tarea 1.4: Adapter (final)
│                 • Validar integración con CC
│                 • Logging, error handling
├─ 14:00-17:00  Testing de Service
│                 • Tests unitarios de PresupuestoService
│                 • Cases de validación
└─ Status: Tarea 1.4 100%

Jueves (12h) [día extendido para UAT]
├─ 09:00-13:00  Tarea 1.2: Views secundarias
│                 • Detalles.cshtml
│                 • Detalles.js
│                 • Validaciones adicionales
├─ 14:00-17:00  Testing funcional
│                 • Crear presupuesto (modal)
│                 • Editar presupuesto
│                 • Eliminar presupuesto
│                 • Exportar Excel
├─ 17:00-19:00  Testing de performance
│                 • Grid con 1000 registros
│                 • Export de 10k registros
└─ Status: Pre-UAT completado

Viernes (8h)
├─ 09:00-13:00  UAT preliminar
│                 • Funcionalidad CRUD completa
│                 • Validaciones funcionando
│                 • Export generando Excel
├─ 14:00-17:00  Documentación
│                 • Actualizar MIGRACION_FI_ADMINISTRATIVO.md
│                 • TESTING_RESULTS_SPRINT1.md
└─ Status: Sprint 1 ✅ COMPLETADO

RESUMEN SEMANA 4:
├─ Tarea 1.3: ✅ 100% (8h)
├─ Tarea 1.4: ✅ 100% (20h)
├─ Tarea 1.2 (vistas sec): ✅ 100% (12h)
├─ Testing + UAT: ✅ 100% (12h)
├─ Documentación: ✅ 100% (4h)
└─ TOTAL: 52 horas, Sprint 1 ✅ COMPLETADO (92h total)

ENTREGABLES SPRINT 1:
✅ ControlPresupuestosController completo
✅ Index.cshtml + modales
✅ Index.js + AJAX
✅ PresupuestoService (CRU + Export)
✅ FIControlPresupuestosAdapter
✅ 90%+ test coverage
✅ UAT sign-off
```

---

## SPRINT 2: Presupuestos Internos (Semana 5, 68 horas)

**Duración**: 1.5 semanas (patrón similar a Sprint 1 pero menos complejidad)

```
SEMANA 5 (40h) + mitad SEMANA 6 (28h)

Lunes-Martes (16h):
├─ Models + DTOs
├─ Controller (completo)
└─ Views (Index + modales)

Miércoles-Jueves (20h):
├─ Service (completo)
├─ Adapter (completo)
└─ Testing (unitario + integración)

Viernes (12h):
├─ UAT
└─ Documentación

STATUS: Menor complejidad que Sprint 1, reutiliza patrones
```

---

## SPRINT 3: Procesos Internos (Semanas 6-7, 132 horas)

**Duración**: 2.5 semanas (sprint MÁS LARGO después de Pre-1)

```
SEMANA 6 (40h):
├─ Lunes-Martes: Validación de SP externas (TH_Ausencia)
├─ Miércoles-Jueves: Models + Controllers (2 de 6 páginas)
└─ Viernes: DTOs + inicio Services

SEMANA 7 (40h):
├─ Lunes-Miércoles: Controllers + Views (4 de 6 páginas)
├─ Jueves-Viernes: Services (lógica compleja)
└─ Documentación de riesgos (CalculoJornadaLaboral)

SEMANA 8 (inicio) (52h para alcanzar 132h total):
├─ Lunes-Martes: Adapters (lógica de conteos, requerimientos)
├─ Miércoles-Jueves: Testing intensivo (validación de cálculos)
└─ Viernes: UAT con stakeholders

RIESGOS MITIGATION:
├─ TH_Ausencia SP: validar antes de Lunes
├─ Cálculos de jornada: testing con datos reales
└─ Performance: optimizar índices pre-Sprint 3
```

---

## SPRINT 4: Reportes (Semana 8, 72 horas)

**Duración**: 1.5 semanas

```
SEMANA 8 (mitad) + SEMANA 9 (inicio)

Patrón:
├─ Controllers (4 páginas, read-only)
├─ Views (grillas + exportes)
├─ Services (validaciones permisos)
├─ Adapters (queries complejas con Dapper)
└─ Testing (datasets grandes)

CRÍTICO:
├─ Permisos por trabajo
├─ Auditoría de acceso
└─ Performance de reportes grandes
```

---

## SPRINT 5: Producción (Semanas 9-12, 232 horas - ⚠️ CRÍTICO)

**Duración**: 4 semanas

```
SEMANA 9 (40h):
├─ Validación exhaustiva de SP (CC_LiquidarPlanillas, CC_GenerarBonificacion)
├─ Backup del DB original
├─ RegistroProduccion page (Grid CRUD)
└─ DTOs + Models

SEMANA 10 (40h):
├─ LiquidarPlanillasActividades (wizard 3 pasos)
├─ GenerarBonificacion
├─ Service layer (lógica crítica)
└─ Testing manual con nómina real

SEMANA 11 (40h):
├─ CargueDescuentosSS
├─ LiquidarProductividadPST
├─ EstadoJobBooks
├─ Adapters (Dapper para todas)
└─ Integration testing

SEMANA 12 (52h):
├─ AnulacionLiquidaciones (workflow complejo)
├─ UAT con equipo de Nómina (2 días)
├─ Reconciliación histórica
├─ Documentación de riesgos
└─ Go/No-Go decision

⚠️ CRÍTICO:
├─ Errores aquí = empleados cobran mal
├─ Requiere sign-off de Nómina
├─ Testing con datos históricos reales
├─ Backup + rollback strategy
└─ Auditoría completa
```

---

## SPRINT 6: Inventario (Semana 13, 16 horas)

**Duración**: 1 semana

```
SEMANA 13 (16h):

Lunes-Martes (8h):
├─ InventarioProductos page (Grid CRUD simple)
├─ Models + DTOs

Miércoles (8h):
├─ Controller + Service
├─ Adapter
├─ Testing + UAT

Patrón: Reutiliza 100% estructura de Sprint 1-2
Complejidad: Mínima
```

---

## TIMELINE GLOBAL

```
┌─────────────────────────────────────────────────────────┐
│ CRONOGRAMA ACUMULATIVO - 13 SEMANAS                     │
├─────────────────────────────────────────────────────────┤
│ Semana 1-2: Sprint Pre-1 (80h)        ✅ Infraestructura
│ Semana 3-4: Sprint 1 (92h)            ✅ Control Presupuestos
│ Semana 5: Sprint 2 (68h)              🔄 Presupuestos Internos
│ Semana 6-7: Sprint 3 (132h)           📋 Procesos Internos
│ Semana 8: Sprint 4 (72h)              📋 Reportes
│ Semana 9-12: Sprint 5 (232h)          ⚠️ Producción (CRÍTICO)
│ Semana 13: Sprint 6 (16h)             📋 Inventario
├─────────────────────────────────────────────────────────┤
│ TOTAL: 13 semanas, 784 horas @ 60h/sem (2 devs part-time)
│        10 semanas @ 80h/sem (1 dev full-time + overtime)
└─────────────────────────────────────────────────────────┘
```

---

## CHECKLIST GO/NO-GO POR SPRINT

### Sprint Pre-1: CC_FinzOpe

**Pre-Sprint Checklist** (Antes de Semana 1):
- [ ] SQL Server accesible
- [ ] Permisos de lectura en BD CC_FinzOpe
- [ ] Proyecto MatrixNext.Web creado en VS
- [ ] Git repo inicializado
- [ ] NuGet packages confirmados (EF Core, Dapper, ClosedXML)

**Go/No-Go Checklist** (Fin Semana 2):
- [ ] DbContext compila sin errores
- [ ] 20+ modelos generados correctamente
- [ ] 8-10 DTOs creadas y compiladas
- [ ] Adapter implementa 20+ métodos
- [ ] Service registrado en DI
- [ ] appsettings.json tiene connection string
- [ ] SP CC_LiquidarPlanillas ejecuta correctamente
- [ ] SP CC_GenerarBonificacion ejecuta correctamente
- [ ] Logging funciona (Information level)
- [ ] 0 warnings críticos
- **BLOCKER**: Si alguno es ❌, no avanzar a Sprint 1

**Sign-Off**: Tech Lead + Arquitecto

---

### Sprint 1: Control Presupuestos

**Pre-Sprint Checklist** (Antes de Semana 3):
- [ ] Sprint Pre-1 completado y ✅
- [ ] CCFinzOpeService funcionando
- [ ] Área FI/ creada en proyecto
- [ ] Patrón grid_component.js copiado a wwwroot

**Go/No-Go Checklist** (Fin Semana 4):
- [ ] Controller.Index() GET responde 200 OK
- [ ] Grid carga sin datos (0 registros)
- [ ] Modal nuevo abre sin errores
- [ ] Service.ObtenerPresupuestos() retorna lista
- [ ] Service.GuardarPresupuesto() inserta en BD
- [ ] Service.GuardarPresupuesto() valida montos negativos
- [ ] Guardar → modal cierra → grid recarga
- [ ] Eliminar → confirmación → grid actualiza
- [ ] Exportar → descarga archivo .xlsx
- [ ] 80%+ test coverage (unitario)
- [ ] Compilación sin warnings críticos
- [ ] Documentación actualizada
- **BLOCKER**: Si alguno es ❌, extender Sprint 1

**Sign-Off**: Product Owner + QA

---

### Sprint 2: Presupuestos Internos

**Go/No-Go Checklist** (Fin Semana 5):
- [ ] Controller CRUD completo funcionando
- [ ] Service con validaciones
- [ ] Adapter integrado con CC_FinzOpe
- [ ] Grid + modales + export
- [ ] 80%+ test coverage
- [ ] Compilación OK

**Sign-Off**: QA + Product Owner

---

### Sprint 3: Procesos Internos

**Pre-Sprint Checklist**:
- [ ] TH_Ausencia.CalculoDias SP validada
- [ ] Índices en CC_Produccion optimizados
- [ ] Documentación de SP externas completada

**Go/No-Go Checklist** (Fin Semana 8):
- [ ] 6 páginas implementadas
- [ ] ConteoTrabajos CRUD funciona
- [ ] CalculoJornadaLaboral calcula correctamente con TH_Ausencia
- [ ] RequerimientosEquipo genera requerimientos
- [ ] Todos los adapters ejecutan SP sin timeout (< 2s)
- [ ] 90%+ test coverage
- [ ] UAT con stakeholders completada
- **BLOCKER**: Si CalculoJornadaLaboral falla, halt

**Sign-Off**: Tech Lead + Nómina (stakeholder)

---

### Sprint 4: Reportes

**Go/No-Go Checklist** (Fin Semana 9):
- [ ] 4 reportes generan datos
- [ ] Permisos por trabajo validados
- [ ] Export a Excel funciona
- [ ] Performance < 3s para 10k registros
- [ ] Auditoría de acceso logged
- [ ] 80%+ test coverage

**Sign-Off**: QA + Security

---

### Sprint 5: Producción (⚠️ CRÍTICO)

**Pre-Sprint Checklist** (Antes de Semana 9):
- [ ] Backup DB original realizado
- [ ] CC_LiquidarPlanillas SP testeda con datos reales
- [ ] CC_GenerarBonificacion SP testeda
- [ ] Nómina team alineada en cambios
- [ ] Rollback script preparado
- [ ] Documentación de validaciones de negocio completada

**Interim Checklist** (Fin Semana 10):
- [ ] RegistroProduccion funciona
- [ ] LiquidarPlanillasActividades ejecuta SP sin error
- [ ] GenerarBonificacion calcula correctamente
- [ ] Resultados reconcilian con datos históricos ✅

**Go/No-Go Checklist** (Fin Semana 12):
- [ ] 9 páginas implementadas
- [ ] Liquidación calcula correctamente (reconciliación ✅)
- [ ] Bonificación genera montos correctos
- [ ] Descuentos SS se aplican correctamente
- [ ] PST liquidación funciona
- [ ] Anulaciones pueden revertirse sin data loss
- [ ] 95%+ test coverage (crítico)
- [ ] UAT con Nómina: PASSED ✅
- [ ] Performance bajo carga (5k registros): OK
- [ ] Auditoría trail completo
- [ ] Documentación de cases especiales
- **BLOCKER**: UAT falla = NO avanzar

**Sign-Off Requerido**:
- [ ] Tech Lead
- [ ] QA Lead
- [ ] Nómina Manager
- [ ] CFO (financiero)
- [ ] Auditor

**Rollback Decision**: Si Go/No-Go falla, ejecutar:
1. Revertir DB a backup
2. Mantener código en rama feature (no merge a main)
3. Schedule retrospectiva
4. Analizar root causes

---

### Sprint 6: Inventario

**Go/No-Go Checklist** (Fin Semana 13):
- [ ] CRUD funciona
- [ ] 80%+ test coverage
- [ ] Compilación OK
- [ ] Puede hacer merge a main

**Sign-Off**: QA

---

## MÉTRICAS DE SEGUIMIENTO

### Por Sprint

| Métrica | Objetivo | Semana 1 | Semana 2 | Semana 3 | ... |
|---------|----------|----------|----------|----------|-----|
| **Completitud de Tareas** | 100% | 95% | 100% | 100% | |
| **Test Coverage** | 80%+ | 75% | 85% | 90% | |
| **Bugs Encontrados** | < 5 | 2 | 1 | 0 | |
| **Performance (avg)** | < 2s | 1.8s | 1.5s | 1.2s | |
| **Code Review Pass** | 100% | 90% | 95% | 100% | |

### Global

```
TRACKER SEMANAL:

Semana  Sprint          Horas   Completitud   Blocker?   Sign-Off
────────────────────────────────────────────────────────────────────
1       Pre-1           40h     100% ✅        No         Tech
2       Pre-1           40h     100% ✅        No         Tech
3       Sprint 1        40h     100% ✅        No         QA
4       Sprint 1        52h     100% ✅        No         PO
5       Sprint 2        68h     100% ✅        No         QA
6-7     Sprint 3        72h     100% ✅        No         Tech
8-9     Sprint 4        72h     100% ✅        No         Security
9-12    Sprint 5       232h     100% ✅        SÍ?        CFO*
13      Sprint 6        16h     100% ✅        No         QA

*Requiere sign-off de Finance si sprint 5 tiene issues
```

---

## MATRIZ DE RIESGOS Y MITIGACIONES

### Riesgos Críticos (Sprint 5)

| Riesgo | Probab. | Impacto | Mitigación | Contingencia |
|--------|---------|---------|-----------|--------------|
| **Liquidación incorrecta** | Media | 🔴 CRÍTICO | Testing exhaustivo vs nómina | Rollback + reprocess |
| **Pérdida datos históricos** | Baja | 🔴 CRÍTICO | Backup pre-Sprint 5 | Restore from backup |
| **Performance < SLA** | Media | 🟠 Alto | Optimizar índices | Paginación + async |
| **SP corruption** | Baja | 🔴 CRÍTICO | Validar SP en Pre-1 | Script de reparación |
| **UAT falla** | Media | 🟠 Alto | Ambiente de staging | Extender 1 semana |

### Riesgos Medios (Sprint 3)

| Riesgo | Mitigación |
|--------|-----------|
| Cálculo de jornada incorrecto | Testing con TH_Ausencia real |
| Requerimientos generad mal | Validar lógica con Producción team |
| Conteos no cuadran | Reconciliación pre-Sprint |

### Riesgos Bajos (Sprints 1-2, 4, 6)

| Riesgo | Mitigación |
|--------|-----------|
| UI no responsive | Usar Bootstrap grid |
| Export falla | Testing con datasets 10k |
| Permisos insuficientes | Validar pre-Sprint |

---

## CHECKLIST DE DOCUMENTACIÓN

**Pre-Implementación**:
- [x] PLAN_SPRINT_PRE1_CC_FINZOPE.md ✅
- [x] PLAN_SPRINTS_1_6_FI.md ✅
- [x] PATRONES_ARQUITECTURA_FI.md ✅
- [x] Este documento (CRONOGRAMA_VALIDACION.md) ✅

**Por Sprint** (durante implementación):
- [ ] TESTING_RESULTS_{SPRINT}.md
- [ ] CODE_REVIEW_CHECKLIST_{SPRINT}.md
- [ ] UAT_SIGNOFF_{SPRINT}.md
- [ ] RETROSPECTIVE_{SPRINT}.md

**Post-Proyecto**:
- [ ] MIGRACION_FI_FINAL_REPORT.md
- [ ] LESSONS_LEARNED.md
- [ ] ARCHITECTURE_GUIDE_FI.md (para futuros devs)

---

## ENTREGABLES POR FASE

### Phase 0: Pre-Proyecto (Completado)
```
✅ Documentación completa (5 arquivos .md)
✅ Análisis de dependencias
✅ Arquitectura definida
✅ Patrones establecidos
✅ Timeline acordado
```

### Phase 1: Sprint Pre-1 (Semanas 1-2)
```
✅ DbContext EF Core
✅ DTOs (8-10)
✅ Adapter + Service
✅ DI registration
✅ Testing manual
✅ 3 documentos de análisis
```

### Phase 2: Sprints 1-6 (Semanas 3-13)
```
✅ 28 páginas migradas
✅ Controllers + Views + JS
✅ Services + Adapters
✅ Unit + Integration tests
✅ UAT sign-off por sprint
✅ Documentación actualizada
```

### Phase 3: Post-Proyecto
```
✅ Code review completo
✅ Performance audit
✅ Security audit
✅ Lessons learned
✅ Architecture guide
```

---

## RESPONSABILIDADES

### Por Rol

| Rol | Responsabilidad | Sprints |
|-----|-----------------|---------|
| **Tech Lead** | Sign-off código, arquitectura | Pre-1, 3, 5 |
| **Dev** | Implementación, testing | Todos |
| **QA** | Testing funcional, UAT | Todos |
| **Product Owner** | Validación requisitos | 1, 2, 4, 6 |
| **Nómina Team** | Validar liquidaciones | 3, 5 |
| **CFO** | Aprobación financiera | 5 |
| **Auditor** | Compliance, auditoría | 5 |

---

## DECISIONES GO/NO-GO FINALES

```
DECISION TREE:

Sprint Pre-1 Go?
├─ SÍ → Proceder a Sprint 1
└─ NO → Fix issues + retry

Sprint 1 Go?
├─ SÍ → Proceder a Sprint 2
└─ NO → Extend 1 sem + retry

Sprint 2 Go?
├─ SÍ → Proceder a Sprint 3
└─ NO → Extend 1 sem + retry

Sprint 3 Go?
├─ SÍ → Proceder a Sprint 4
└─ NO → Extend 2 sem + retry (CalculoJornadaLaboral crítico)

Sprint 4 Go?
├─ SÍ → Proceder a Sprint 5
└─ NO → Extend 1 sem + retry

Sprint 5 Go?  ⚠️ CRÍTICO - Requiere unanimidad
├─ SÍ (todos firman) → Proceder a Sprint 6 + producción
├─ NO (cualquier blocker) → ROLLBACK + análisis root cause
└─ CONDICIONAL → Fix issues + retry (máx 2 attempts)

Sprint 6 Go?
└─ SÍ → Project complete ✅
```

---

## MATRIZ DE COMUNICACIÓN

### Reuniones Semanales

| Día | Reunión | Duración | Asistentes | Agenda |
|-----|---------|----------|-----------|--------|
| **Lunes 09:00** | Sprint Kickoff | 30min | Dev, Tech Lead, PO | Plan de semana |
| **Miércoles 14:00** | Progress Check | 15min | Dev, Tech Lead | Status, blockers |
| **Viernes 17:00** | Sprint Review | 45min | Dev, QA, PO, Tech Lead | Demostración, sign-off |

### Reuniones por Hito (Go/No-Go)

| Hito | Reunión | Duración | Asistentes |
|------|---------|----------|-----------|
| **Fin Pre-1** | Go/No-Go | 1h | Tech Lead, Arquitecto |
| **Fin Sprint 1-4, 6** | Go/No-Go | 1h | Tech Lead, QA, PO |
| **Fin Sprint 5** | Go/No-Go + UAT | 2h | Tech + QA + Finance + Nómina + Auditor |

---

## ✅ CHECKLIST PRE-INICIO

**Completar ANTES de comenzar Sprint Pre-1 (Semana 1 Lunes)**

### 📚 Lectura Obligatoria (~80 min)

- [ ] README_MIGRACION.md (10 min)
- [ ] CRONOGRAMA_VALIDACION.md - SEMANA 1-2 (20 min)
- [ ] PLAN_SPRINT_PRE1_CC_FINZOPE.md - § 1-3 (20 min)
- [ ] PATRONES_ARQUITECTURA_FI.md - § 1-2 (20 min)

### 💻 Setup Técnico

**Acceso y Permisos**
- [ ] SQL Server acceso confirmado
- [ ] Base de datos WebMatrix accesible
- [ ] Base de datos Matrix (destino) creada
- [ ] Permisos de lectura en WebMatrix
- [ ] Permisos de lectura/escritura en Matrix

**Software Requerido**
- [ ] Visual Studio 2022 Community+ instalado
- [ ] .NET 8.0 SDK instalado
- [ ] SQL Server Management Studio instalado
- [ ] Git instalado y configurado
- [ ] Postman/Insomnia instalado

**Proyecto y Repositorio**
- [ ] MatrixNext.sln creado
- [ ] Estructura carpetas básica: Areas/, wwwroot/, etc.
- [ ] Git repository inicializado
- [ ] Rama `feature/sprint-pre1` creada
- [ ] .gitignore configurado

**Paquetes NuGet Requeridos**
- [ ] Microsoft.EntityFrameworkCore (8.0+)
- [ ] Microsoft.EntityFrameworkCore.SqlServer (8.0+)
- [ ] Microsoft.EntityFrameworkCore.Tools (8.0+)
- [ ] Dapper (latest)
- [ ] ClosedXML (latest)
- [ ] Microsoft.Extensions.Logging (8.0+)
- [ ] AutoMapper (latest, opcional)

### 👥 Alineación del Equipo

**Participantes Asignados**
- [ ] Desarrollador(es) identificado(s)
- [ ] Tech Lead asignado
- [ ] QA/Tester asignado
- [ ] Project Manager asignado
- [ ] Stakeholder (Nómina, Finance) para Sprint 5

**Reuniones Completadas**
- [ ] **Kickoff meeting** (1h): Timeline, arquitectura, roles explicados
- [ ] **Tech alignment** (1h): Patrones, stack, DI, testing aprobado
- [ ] **Go/No-Go pre-proyecto** (30min): Documentación OK, setup OK, PROCEED decidido

### 🎯 Conocimiento Crítico

**Desarrollador debe entender**:
- [ ] Qué es CC_FinzOpe y por qué está en Sprint Pre-1
- [ ] Las 7 tareas de Sprint Pre-1
- [ ] Pattern: Adapter → Service → Controller
- [ ] DI configuration en Program.cs
- [ ] Cuándo usar EF Core vs Dapper
- [ ] Dónde buscar ayuda (PATRONES_ARQUITECTURA_FI.md, PLAN_SPRINTS_1_6_FI.md)

**Tech Lead debe entender**:
- [ ] Arquitectura propuesta (Areas + Service + Adapter)
- [ ] Convenciones de naming
- [ ] Testing strategy
- [ ] Go/No-Go criteria por sprint
- [ ] Qué buscar en code reviews

**QA debe entender**:
- [ ] Go/No-Go checklists y criterios
- [ ] Testing levels: unit, integration, UAT
- [ ] Qué testear en Sprint Pre-1
- [ ] Acceso a SQL Server para validación

**Project Manager debe entender**:
- [ ] Timeline completo (13 semanas)
- [ ] 7 sprints y sus hitos
- [ ] Go/No-Go por sprint
- [ ] Riesgos críticos (especialmente Sprint 5)

### ✨ Ambiente Listo

- [ ] VS solution crea sin errores
- [ ] NuGet packages instalados correctamente
- [ ] Conexión a SQL Server WebMatrix OK
- [ ] Conexión a SQL Server Matrix (destino) OK
- [ ] Git está funcionando
- [ ] Documentación accesible

### 🚫 Riesgos Pre-Mitigados

- [ ] SQL Server acceso validado (NO esperar conectarse por primera vez Lunes)
- [ ] NuGet packages instalados (NO esperar instalar durante Sprint)
- [ ] Proyecto setup completado (NO esperar crear estructura)
- [ ] Equipo alineado (NO esperar sorpresas de arquitectura)
- [ ] Documentación leída (NO esperar "¿cómo hacemos X?")
- [ ] Go/No-Go criterios entendidos

### 📞 Comunicación Establecida

- [ ] Slack/Teams channel creado: #migracion-fi
- [ ] **Standup diario**: Lunes-Viernes 09:00 (15 min)
- [ ] **Miércoles 14:00**: Progress check (15 min)
- [ ] **Viernes 17:00**: Sprint review (45 min)
- [ ] Escalation path claro para blockers

### 🏁 Señal de Listo

**Cuando TODOS estos boxes están checked, estamos listos:**

```
DOCUMENTACIÓN:          ☑️ Todos leyeron
SETUP TÉCNICO:          ☑️ Todo configurado  
ALINEACIÓN EQUIPO:      ☑️ Reuniones OK
CONOCIMIENTO:           ☑️ Team entiende plan
HERRAMIENTAS:           ☑️ Todo instalado
COMUNICACIÓN:           ☑️ Canales establecidos
RIESGOS:                ☑️ Mitigados

═══════════════════════════════════════════════════════════════
              🟢 LISTO PARA SPRINT PRE-1
═══════════════════════════════════════════════════════════════
```

### 📋 Firmas

```
Completado por:

DESARROLLADOR:          ________________  Fecha: ________

TECH LEAD:              ________________  Fecha: ________

QA LEAD:                ________________  Fecha: ________

PROJECT MANAGER:        ________________  Fecha: ________
```

---

## RESUMEN EJECUTIVO

```
PROYECTO: Migración FI_Administrativo a MatrixNext
DURACIÓN: 13 semanas
ESFUERZO: 784 horas
RECURSOS: 1-2 devs
COSTO: ~$50k-80k (estimado @ $65/h)

FASES:
├─ Pre-proyecto: ✅ Completada (documentación)
├─ Sprint Pre-1: 80h (CC_FinzOpe infraestructura)
├─ Sprints 1-6: 704h (28 páginas FI)
└─ Validación: Incluida en sprints (no extra)

RIESGOS PRINCIPALES:
🔴 Sprint 5 (Producción): Requiere validación con Nómina
🟠 Sprint 3: Cálculos de jornada dependente de TH_Ausencia
🟡 Performance: Índices SQL deben estar optimizados

MAPA CRÍTICO:
├─ Pre-1 debe estar 100% OK antes de Sprint 1
├─ Sprint 5 requiere sign-off de Finance/Auditor
└─ Cualquier blocker puede extender 1-2 semanas

NEXT STEPS:
1. ✅ Revisión de documentación (esta semana)
2. ⏳ Setup de entorno (SQL, VS, Git)
3. ⏳ Inicio Sprint Pre-1 (Semana 1)
4. ⏳ Daily standup a partir de Lunes
5. ⏳ Go/No-Go meeting fin de semana 2
```

---

**Documento**: CRONOGRAMA_VALIDACION.md  
**Versión**: 1.0  
**Estado**: 📋 Ready for kickoff  
**Creado**: Enero 2026  
**Próxima revisión**: Fin Sprint Pre-1

