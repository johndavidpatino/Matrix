# DASHBOARD DE MIGRACION - Estado actual

Fecha de corte: 2026-01-15 (SPRINT 5 KICKOFF INICIADO)

## Resumen corto
- **SPRINT 5 INICIADO**: TH_TalentoHumano Views/UI (15-29 enero, 80h)
- **TH_TalentoHumano Sprint 4**: API REST COMPLETADA (21 archivos, 2,750+ LOC, 55 endpoints, 0 errores)
- **Sprints 6-12 Planificados**: Roadmap de 3.5 meses para 100% completado el 2026-05-03
- OP_Cualitativo: migracion MVP completa (Sprint 5) y documentada.

## Estado por modulo (WebMatrix -> MatrixNext)

| Modulo | Estado | Evidencia principal |
| --- | --- | --- |
| US_Usuarios | Completo | `MatrixNext/MatrixNext.Web/Areas/US` |
| TH_TalentoHumano (Sprint 4 API) | Completo | `MatrixNext/MatrixNext.Web/Areas/TH` + 6 Adapters + 3 Services + 3 Controllers (55 endpoints) |
| TH_Ausencias (submodulo) | Completo | `MatrixNext/docs/TH/RESUMEN_MIGRACION_AUSENCIAS.md` |
| CU_Cuentas | Completo | `MatrixNext/MatrixNext.Web/Areas/CU` |
| CC_FinzOpe + FI_Administrativo | Completo (Inventario no migrar) | `MatrixNext/MatrixNext.Web/Areas/CC` + `MatrixNext/docs/FI_CC/` |
| OP_Cuantitativo | Completo | `MatrixNext/docs/OP/ANALISIS_OP_CUANTITATIVO.md` |
| OP_Cualitativo | MVP completo; pendientes complementarios Sprint 6 | `MatrixNext/docs/OP/SPRINT_5_CIERRE_MIGRACION_COMPLETA.md` |
| PY_Proyectos | Parcial | `MatrixNext/MatrixNext.Web/Areas/PY` + `MatrixNext/docs/PY/MIGRACION_PY_PROYECTOS.md` |
| CORE (workflow/tareas) | Parcial | `MatrixNext/MatrixNext.Web/Areas/CORE` + `MatrixNext/docs/CORE/` |
| GD_Documentos | Parcial (Fase 5 completa) | `MatrixNext/docs/GD/BACKLOG_MIGRACION_GD_DOCUMENTOS_FASE5_PARTE_A.md` |
| EQ (EasyQuote) | En progreso | `MatrixNext/docs/EQ/` |
| Home | Pendiente | `WebMatrix/Home` |
| RP_Reportes | Pendiente | `WebMatrix/RP_Reportes` |
| OP_RO | Pendiente | `WebMatrix/OP_RO` |
| OP_Trafico | Pendiente | `WebMatrix/OP_Trafico` |
| PY_ControlCalidad | Pendiente | `WebMatrix/PY_ControlCalidad` |
| PY_Adquisiciones | Pendiente | `WebMatrix/PY_Adquisiciones` |
| PNC (legacy) | Cubierto por GD Fase 5 | `MatrixNext/docs/GD/` |
| SG_Actas | Pendiente | `WebMatrix/SG_Actas` |
| SGC_Calidad | Pendiente | `MatrixNext/docs/GENERAL/SGC_Calidad.md` |
| ES_Estadistica | Pendiente | `WebMatrix/ES_Estadistica` |
| Centro_Informacion | Pendiente | `WebMatrix/Centro_Informacion` |
| Inventario | Pendiente (excluido en FI) | `WebMatrix/Inventario` |
| IT | Pendiente | `WebMatrix/IT` |
| MBO / MBO_Gerencial / MBO_Operaciones | Pendiente | `WebMatrix/MBO*` |
| ResumenProduccion | Pendiente | `WebMatrix/ResumenProduccion` |
| RE_GT | Pendiente | `WebMatrix/RE_GT` |
| PC_PropiedadCliente | Pendiente | `WebMatrix/PC_PropiedadCliente` |

## Pendientes que deben continuar (Sprints 5-12)
1. **Sprint 5 (2 sem)**: TH_TalentoHumano Views/UI (Nómina y complementarios sobre API Sprint 4 completa)
2. **Sprint 6 (2 sem)**: OP_Cualitativo Complementos (reportes, filtros avanzados)
3. **Sprint 7 (2 sem)**: CORE Workflow (tareas, notificaciones, integraciones)
4. **Sprint 8 (2-3 sem)**: EQ_EasyQuote Fase 1 (análisis + catálogos + infraestructura)
5. **Sprint 9 (1-2 sem)**: Home Dashboard (widgets, KPIs, filtros)
6. **Sprint 10 (1-2 sem)**: RP_Reportes (72 reportes, exportes Excel/PDF) ← **🔴 PRÓXIMO FOCUS**
7. **Sprint 11 (2 sem)**: OP_RO + OP_Trafico (revisiones + tráfico de datos) ← **🔴 FINAL FOCUS**
8. **Sprint 12+ (Variable)**: Módulos baja prioridad (PY_CC, SG_Actas, SGC_Calidad, etc.)

## Estructura de documentacion (ordenada)
- `MatrixNext/docs/CORE/`
- `MatrixNext/docs/CU/`
- `MatrixNext/docs/EQ/`
- `MatrixNext/docs/FI_CC/`
- `MatrixNext/docs/GD/`
- `MatrixNext/docs/GENERAL/`
- `MatrixNext/docs/OP/`
- `MatrixNext/docs/PY/`
- `MatrixNext/docs/SQL/`
- `MatrixNext/docs/TH/`

Ultima revision: 2026-01-11

## Semáforo de progreso por modulo

| Módulo | Prioridad | Status resumido | Indicador |
| --- | --- | --- | --- |
| US_Usuarios | Crítica | Migrado y validado | 🟢 |
| TH_TalentoHumano (API Sprint 4) | Media | API REST completa (55 endpoints, 0 errores); Views pendientes Sprint 5 | 🟢 |
| TH_Ausencias + Empleados API | Media | API REST completa (55 endpoints, 0 errores, Sprint 4) | 🟢 |
| CU_Cuentas | Alta | Completo | 🟢 |
| CC_FinzOpe / FI | Alta | Infraestructura lista | 🟢 |
| OP_Cuantitativo | Alta | Documentado y concluido | 🟢 |
| OP_Cualitativo | Alta | MVP terminado; complementos planificados Sprint 6 | 🟡 |
| PY_Proyectos | Alta | Catálogos/maestros en marcha | 🟡 |
| CORE (workflow) | Alta | Parcial; completar Sprint 7 | 🟡 |
| GD_Documentos | Media | Fase 5 cerrada; Fases 1-4 pendientes | 🟡 |
| EQ (EasyQuote) | Crítica | En progreso Sprint 8 | 🟡 |
| Home | Alta | Planificado Sprint 9 | 🔴 |
| **RP_Reportes** | **🔴 Alta** | **72 reportes + exportes → SPRINT 10 (04-05 a 04-16)** | **🔴** |
| **OP_RO + OP_Trafico** | **🟠 Media-Baja** | **Revisiones + tráfico → SPRINT 11 (04-19 a 05-03, HITO FINAL)** | **🔴** |
| PY_ControlCalidad | Baja | Sprint 12+ | ⚪️ |
| SG_Actas | Baja | Sprint 12+ | ⚪️ |
| SGC_Calidad | Baja | Sprint 12+ | ⚪️ |
| Resto (ES_Estadistica, Centro_Informacion, IT, MBO*, etc.) | Baja | Sprint 12+ | ⚪️ |

**🎯 HITO CRÍTICO**: 2026-05-03 (Fin Sprint 11) = 100% módulos alta/media completados

---

## Timeline de Ejecución (Sprints 5-12 Planificados)

📊 **Ver documento detallado**: [PLAN_EJECUCION_SPRINTS_5_12.md](PLAN_EJECUCION_SPRINTS_5_12.md)

📋 **Ver planes Sprint 10 & 11**: [SPRINT_10_11_PLAN_DETALLADO.md](SPRINT_10_11_PLAN_DETALLADO.md) + [SPRINT_10_11_KICKOFF_GUIDE.md](SPRINT_10_11_KICKOFF_GUIDE.md)

| Sprint | Módulo | Duración | Fechas Estimadas | Esfuerzo | Estado |
|---|---|---|---|---|---|
| 5 | TH Views/UI | 2 sem | 2026-01-15 → 2026-01-29 | 80h | 🟡 **IN PROGRESS** ⏱️ |
| 6 | OP_Cualitativo Complementos | 2 sem | 2026-02-01 → 2026-02-12 | 75h | ⚪️ PRÓXIMO |
| 7 | CORE Workflow | 2 sem | 2026-02-15 → 2026-02-26 | 85h | 🟡 PLANIFICADO |
| 8 | EQ_EasyQuote Fase 1 | 2-3 sem | 2026-03-01 → 2026-03-19 | 120h | 🟡 PLANIFICADO |
| 9 | Home Dashboard | 1-2 sem | 2026-03-22 → 2026-04-02 | 50h | 🔴 PLANIFICADO |
| **10** | **RP_Reportes** | **1-2 sem** | **2026-04-05 → 2026-04-16** | **60h** | **🔴 PRÓXIMO FOCUS** |
| **11** | **OP_RO + OP_Trafico** | **2 sem** | **2026-04-19 → 2026-05-03** | **90h** | **🔴 FINAL FOCUS** |
| 12+ | Módulos Baja Prioridad | Variable | 2026-05-05+ | TBD | ⚪️ PENDIENTE |

**Total**: ~560 horas (~3.5 meses de ejecución secuencial)

**🎯 HITO CRÍTICO**: 2026-05-03 = Fin Sprints 5-11 (100% módulos alta/media completados)
