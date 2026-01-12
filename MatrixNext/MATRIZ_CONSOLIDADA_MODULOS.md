# 📋 MATRIZ CONSOLIDADA - Módulos → Sprints → 100%

**Fecha**: 2026-01-15  
**Versión**: 1.0 Final  
**Objetivo**: Vista única de todos los módulos parciales y su ruta a completación

---

## MATRIZ VISUAL

```
MÓDULOS CON PENDIENTES Y SU RUTA A 100%

Status Legend: 🟢=Completo | 🟡=Parcial | 🔴=No iniciado | ⚪️=Baja Prioridad

┌─────────────────────────────────────────────────────────────────────────────┐
│ MÓDULOS CRÍTICOS & ALTA PRIORIDAD (Sprint 5-11)                           │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│ 🟡 TH_TalentoHumano                                                        │
│    ├─ 🟢 API REST (Empleados, Nómina, Desvinculaciones) [Sprint 4 ✅]     │
│    ├─ 🔴 Views/UI (Razor + AJAX) → SPRINT 5 (80h, 2 sem)                  │
│    └─ 📋 Estado: 50% COMPLETADO | Próximo: Sprint 5                      │
│                                                                             │
│ 🟡 OP_Cualitativo                                                          │
│    ├─ 🟢 MVP Básico [Sprint 5]                                            │
│    ├─ 🔴 Reportes + Filtros Avanzados → SPRINT 6 (75h, 2 sem)            │
│    └─ 📋 Estado: 60% COMPLETADO | Próximo: Sprint 6                      │
│                                                                             │
│ 🟡 CORE (Workflow/Tareas)                                                  │
│    ├─ 🔴 Workflow de tareas + Integraciones → SPRINT 7 (85h, 2 sem)       │
│    └─ 📋 Estado: 20% COMPLETADO | Próximo: Sprint 7                      │
│                                                                             │
│ 🟡 GD_Documentos                                                           │
│    ├─ 🟢 Fase 5 (PNC) [✅]                                               │
│    ├─ 🔴 Fases 1-4 (Infraestructura/Catálogos/Maestro/Workflow)           │
│    └─ 📋 Estado: 20% COMPLETADO | Bloqueado hasta Sprint 5-6             │
│                                                                             │
│ 🟡 PY_Proyectos                                                            │
│    ├─ 🟢 Básico [Parcial]                                                 │
│    ├─ 🔴 5 Funciones Faltantes (InHomeVisit, Variables, Instructivos...)  │
│    └─ 📋 Estado: 70% COMPLETADO | Bloqueado hasta Sprint 6-7             │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────────┐
│ MÓDULOS OPERACIONALES & COMPLEMENTARIOS (Sprint 8-11)                     │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│ 🟡 EQ_EasyQuote (Crítica - módulo grande)                                 │
│    ├─ 🔴 Análisis + Fase 1 (Catálogos/Infraestructura)                   │
│    │    → SPRINT 8 (120h, 2-3 sem)                                        │
│    ├─ 🔴 Fases 2-4 (Alternativas, Simulador, Aprobaciones) → Sprints 9+ │
│    └─ 📋 Estado: 0% | Próximo: Sprint 8 (Análisis prioritario)           │
│                                                                             │
│ 🔴 Home Dashboard                                                          │
│    ├─ 🔴 Widgets + Datos de múltiples módulos → SPRINT 9 (50h, 1-2 sem)  │
│    └─ 📋 Estado: 0% | Próximo: Sprint 9                                  │
│                                                                             │
│ 🔴 RP_Reportes                                                             │
│    ├─ 🔴 Reportes Legacy + Exportes Excel/PDF → SPRINT 10 (60h, 1-2 sem) │
│    └─ 📋 Estado: 0% | Próximo: Sprint 10                                 │
│                                                                             │
│ 🔴 OP_RO (Revisión Operacional)                                           │
│    ├─ 🔴 CRUD + Flujos de Aprobación → SPRINT 11 (45h, 1 sem)            │
│    └─ 📋 Estado: 0% | Próximo: Sprint 11                                 │
│                                                                             │
│ 🔴 OP_Trafico (Gestión de Tráfico)                                        │
│    ├─ 🔴 Dashboard + Asignaciones → SPRINT 11 (45h, 1 sem)               │
│    └─ 📋 Estado: 0% | Próximo: Sprint 11                                 │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────────┐
│ MÓDULOS DE BAJA PRIORIDAD (Sprint 12+ - A Planificar)                     │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│ ⚪️ PY_ControlCalidad, PY_Adquisiciones, SG_Actas, SGC_Calidad             │
│ ⚪️ ES_Estadistica, Centro_Informacion, IT, MBO*, ResumenProduccion       │
│ ⚪️ RE_GT, PC_PropiedadCliente                                             │
│                                                                             │
│ 📋 Estado: 0% (Pendiente decisión de negocio)                             │
│ → Seguir mismo patrón inventario→mapeo→implementación→QA→docs            │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## TABLA RESUMEN POR MÓDULO

| # | Módulo | Status | % Hecho | LOC | Bloques | Sprint | Duración | Esfuerzo |
|---|---|---|---|---|---|---|---|---|
| **1** | TH_TalentoHumano | 🟡 | 50% | 2,750 (API) | Views/UI | 5 | 2w | 80h |
| **2** | OP_Cualitativo | 🟡 | 60% | 3,200 | Reportes | 6 | 2w | 75h |
| **3** | CORE Workflow | 🟡 | 20% | 800 | Integraciones | 7 | 2w | 85h |
| **4** | GD_Documentos | 🟡 | 20% | 1,500 (F5) | Fases 1-4 | 5-7 | 4-5w | 180h |
| **5** | PY_Proyectos | 🟡 | 70% | 2,100 | 5 Features | 6-7 | 2-3w | 95h |
| **6** | EQ_EasyQuote | 🟡 | 0% | 0 | Análisis | 8 | 2-3w | 120h |
| **7** | Home | 🔴 | 0% | 0 | Setup | 9 | 1-2w | 50h |
| **8** | RP_Reportes | 🔴 | 0% | 0 | Setup | 10 | 1-2w | 60h |
| **9** | OP_RO | 🔴 | 0% | 0 | Setup | 11 | 1w | 45h |
| **10** | OP_Trafico | 🔴 | 0% | 0 | Setup | 11 | 1w | 45h |

---

## DEPENDENCIAS ENTRE SPRINTS

```
Sprint 4 (TH API) ✅ COMPLETADO
    ↓
Sprint 5 (TH Views) ← Requiere Sprint 4 ✅
    ↓
Sprint 6 (OP_Cualitativo) ← Puede iniciar en paralelo con Sprint 5
    ↓
Sprint 7 (CORE) ← Requiere Sprints 5-6 avanzados
    ↓
Sprint 8 (EQ Análisis) ← Requiere Sprint 7 ~80% avanzado
    ↓
Sprint 9 (Home) ← Requiere Sprints 5-8
    ↓
Sprint 10 (RP) ← Requiere Sprints 5-9
    ↓
Sprint 11 (OP_RO/Trafico) ← Requiere Sprints 5-10
    ↓
Sprint 12+ (Baja Prioridad) ← Requiere Sprints 5-11 completados
```

---

## CRONOGRAMA DETALLADO

### Q1 2026 (Enero-Marzo)

| Semana | Sprint | Módulo | Hitos |
|---|---|---|---|
| 15-29 Ene | **5** | TH Views/UI | Planning (15), Dev (16-27), QA (28-29) |
| 1-12 Feb | **6** | OP_Cualitativo | Planning (1), Dev (2-10), QA (11-12) |
| 15-26 Feb | **7** | CORE Workflow | Planning (15), Dev (16-24), QA (25-26) |
| 1-19 Mar | **8** | EQ Análisis+F1 | Análisis (1-7), Dev (8-16), QA (17-19) |

### Q2 2026 (Marzo-Mayo)

| Semana | Sprint | Módulo | Hitos |
|---|---|---|---|
| 22 Mar-2 Abr | **9** | Home Dashboard | Planning (22), Dev (23-31), QA (1-2 Abr) |
| 5-16 Abr | **10** | RP_Reportes | Planning (5), Dev (6-14), QA (15-16) |
| 19-3 May | **11** | OP_RO + Trafico | Planning (19), Dev (20-2 May), QA (3 May) |

### Q3 2026 (Mayo+)

| Sprint | Módulo | Estado |
|---|---|---|
| **12+** | Baja Prioridad | Planificar según negocio |

---

## ENTREGABLES CLAVE POR SPRINT

| Sprint | Entregables | Documentación |
|---|---|---|
| 5 | Views Razor, AJAX integración, Sidebar TH | MIGRACION_TH_VIEWS_COMPLETADA.md |
| 6 | Reportes, Filtros avanzados, Exportes | MIGRACION_OP_CUALITATIVO_COMPLETADA.md |
| 7 | API Workflow, Integraciones, Notificaciones | MIGRACION_CORE_WORKFLOW_COMPLETADA.md |
| 8 | Análisis EQ, Catálogos, CRUD base presupuestos | ANALISIS_EQ_EASYQUOTE.md |
| 9 | Dashboard Home, Widgets, Performance optimizado | MIGRACION_HOME_COMPLETADA.md |
| 10 | Reportes Legacy, Exportes Excel/PDF, Filtros | MIGRACION_RP_REPORTES_COMPLETADA.md |
| 11 | OP_RO CRUD+Workflow, OP_Trafico Dashboard | MIGRACION_OP_COMPLETADA.md |

---

## CONTROL DE CALIDAD

**Estándar QA por Sprint**:
- ✅ Smoke tests (flujos principales)
- ✅ Casos funcionales (CRUD completo)
- ✅ Permisos ([Authorize] funciona)
- ✅ Exportes (Excel/PDF valida)
- ✅ Performance (carga < 2s para views principales)
- ✅ Datos reales en staging (no fixtures)

---

## RECURSOS REQUERIDOS

```
Equipo Mínimo (Secuencial):
├── 1x Desarrollador Full-Stack (.NET + Razor + JS)
├── 1x QA Tester (Funcional)
├── 0.5x Tech Lead (Reviews de diseño)
└── Acceso a Staging + SQL Scripts

Equipo Óptimo (Híbrido):
├── 2x Desarrolladores Full-Stack (paralelo Sprints 5+6)
├── 1x QA Tester
├── 0.5x Tech Lead
└── Acceso a Staging + SQL Scripts

Duración:
├── Secuencial: 14 semanas (Sprint 5-11)
├── Híbrido:    9-10 semanas (con paralelo cuidado)
└── Hito:       2026-05-03 = 100% Sprints 5-11 completados
```

---

## RIESGOS PRIORITARIOS

| # | Riesgo | Probabilidad | Impacto | Mitigación |
|---|---|---|---|---|
| 1 | Sprint 8 (EQ) scope creep | 🔴 Alta | 🔴 Alto | Bloquear features nuevas; análisis temprano |
| 2 | Dependencias tardías en Sprint 11 | 🟡 Media | 🟡 Medio | Validación cruzada en previa |
| 3 | Performance en Home/RP con datos reales | 🟡 Media | 🟠 Bajo | Pruebas con staging desde Sprint 9 |
| 4 | Recursos limitados bloquean paralelo | 🟡 Media | 🔴 Alto | Mantener secuencial como plan base |

---

## INDICADORES DE ÉXITO

### Por Sprint
- ✅ 0 errores de compilación
- ✅ 100% de flujos funcionales migrados
- ✅ 100% QA ejecutado y documentado
- ✅ Documento de cierre generado
- ✅ Dashboard actualizado

### Global (2026-05-03)
- ✅ 11 módulos en 100% (Sprints 5-11)
- ✅ +5,000 LOC adicionales
- ✅ 7 documentos de cierre (`MIGRACION_*_COMPLETADA.md`)
- ✅ 0 errores de compilación en build final
- ✅ 100% paridad con WebMatrix legacy

---

## DOCUMENTOS DE REFERENCIA

| Documento | Propósito | Detalle |
|---|---|---|
| **BACKLOG_MIGRACION_GLOBAL.md** | Plan técnico global | Todos los sprints, reglas, checklist |
| **PLAN_EJECUCION_SPRINTS_5_12.md** | Detalles de ejecución | 9 secciones, 1 por sprint, con timeline |
| **RESUMEN_EJECUTIVO_SPRINTS_5_12.md** | Para stakeholders | Situación, plan, beneficios, métricas |
| **DASHBOARD_MIGRACION.md** | Estado en tiempo real | Tabla de progreso, semáforo, timeline |
| **MATRIZ_CONSOLIDADA.md** (este) | Vista única consolidada | Todas las vistas, matrices, roadmap |

---

## PRÓXIMOS PASOS (HOY)

1. ✅ Revisar matriz consolidada
2. ✅ Aprobación ejecutiva del plan
3. ✅ Asignar responsables por Sprint (Product Owner)
4. ✅ Preparar inventario para Sprint 5 (Dev)
5. ✅ Schedulear kick-off Sprint 5 (Mañana 2026-01-16)

---

**Creado**: 2026-01-15  
**Versión**: 1.0  
**Próxima revisión**: 2026-01-29 (fin Sprint 5)  
**Responsable**: Sistema de Migracion
