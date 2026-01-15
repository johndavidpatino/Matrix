# DASHBOARD DE MIGRACION - Estado actual

Fecha de corte: 2026-01-14 (Sprints 5, 6, 7 COMPLETADOS; **Sprint 8 EN CURSO** 🔴)

## Resumen corto
- **SPRINT 5 COMPLETADO**: TH_TalentoHumano Views/UI (15-29 enero, 80h) sobre la API Sprint 4 ya establecida.
- **SPRINT 6 COMPLETADO**: OP_Cualitativo complementos + Bulk Import (13-29 enero, 75h) con notificaciones, background reminders y carga masiva Excel/CSV.
- **SPRINT 7 COMPLETADO** ✅: CORE Workflow runtime (máquina de estados, UI runtime, SignalR notificaciones, reportes alineados) - Detalles en `SPRINT_7_COMPLETADO.md`.
- **TH_TalentoHumano Sprint 4**: API REST COMPLETADA (21 archivos, 2,750+ LOC, 55 endpoints, 0 errores) con mapas en `MatrixNext/docs/TH/RESUMEN_MIGRACION_AUSENCIAS.md`.
- **SPRINT 9 (Home)**: `HomeController`, `DashboardService` y la vista `Views/Home/Index.cshtml` ya entregan widgets de tareas, proyectos, quotes, ausencias, documentos y métricas.
- **RP_Reportes + OP_RO/OP_Trafico**: Controladores y vistas generados (`Areas/RP/Controllers/ReportesController.cs`, `Areas/OP/Controllers/OP_ROController.cs`, `OP_TraficoController.cs`) listos para integrarse en Sprint 10/11.
- **EQ + CORE**: `Areas/EQ` y `Areas/CORE` contienen la base de workflows, EasyQuote y catálogos, lo que valida el backend previo a los sprints formales.
- **SPRINT 8 PRÓXIMO**: EQ_EasyQuote Fase 1 (análisis + catálogos + infraestructura)

## Estado por modulo (WebMatrix -> MatrixNext)

| Modulo | Estado | Evidencia principal |
| --- | --- | --- |
| US_Usuarios | Completo | `MatrixNext/MatrixNext.Web/Areas/US` |
| TH_TalentoHumano (Sprint 4 API) | Completo | `MatrixNext/MatrixNext.Web/Areas/TH` + 6 Adapters + 3 Services + 3 Controllers (55 endpoints) |
| TH_Ausencias (submodulo) | Completo | `MatrixNext/MatrixNext.Web/Areas/TH/Controllers/AusenciasController.cs` + vistas `MatrixNext/MatrixNext.Web/Areas/TH/Views/Ausencias` |
| CU_Cuentas | Completo | `MatrixNext/MatrixNext.Web/Areas/CU` |
| CC_FinzOpe + FI_Administrativo | Completo (Inventario no migrar) | `MatrixNext/MatrixNext.Web/Areas/CC` + `MatrixNext/docs/FI_CC/` |
| OP_Cuantitativo | Operativo | `MatrixNext/MatrixNext.Web/Areas/OP/Controllers/FichaCuantitativaController.cs` + `MatrixNext/docs/OP/ANALISIS_OP_CUANTITATIVO.md` |
| OP_Cualitativo | Completo (Sprint 6: complementos + Bulk Import) | `MatrixNext/MatrixNext.Web/Areas/OP/Controllers/CualitativoPlanillasController.cs`, `CualitativoFiltrosController.cs`, `CualitativoMuestraController.cs` |
| PY_Proyectos | Parcial | `MatrixNext/MatrixNext.Web/Areas/PY/Controllers` + `MatrixNext/docs/PY/MIGRACION_PY_PROYECTOS.md` |
| CORE (workflow) | Completo (Sprint 7) | **SPRINT 7 COMPLETADO**: Máquina de estados + UI runtime + SignalR + reportes (Ver `SPRINT_7_COMPLETADO.md`) |
| GD_Documentos | Parcial (Fase 5 completa) | `MatrixNext/MatrixNext.Web/Areas/GD/Controllers` + `MatrixNext/MatrixNext.Web/Areas/GD/Views` |
| EQ (EasyQuote) | En progreso | `MatrixNext/MatrixNext.Web/Areas/EQ/Controllers` + entrepiezas `MatrixNext/MatrixNext.Web/Areas/EQ/Views` |
| Home | En desarrollo | `MatrixNext/MatrixNext.Web/Controllers/HomeController.cs`, `MatrixNext/MatrixNext.Web/Services/Dashboard/DashboardService.cs`, `MatrixNext/MatrixNext.Web/Views/Home/Index.cshtml` |
| RP_Reportes | En desarrollo | `MatrixNext/MatrixNext.Web/Areas/RP/Controllers/ReportesController.cs`, `MatrixNext/MatrixNext.Web/Areas/RP/Views/Reportes` |
| OP_RO | En desarrollo | `MatrixNext/MatrixNext.Web/Areas/OP/Controllers/OP_ROController.cs`, `MatrixNext/MatrixNext.Web/Areas/OP/Views/OP_RO` |
| OP_Trafico | En desarrollo | `MatrixNext/MatrixNext.Web/Areas/OP/Controllers/OP_TraficoController.cs`, `MatrixNext/MatrixNext.Web/Areas/OP/Views/OP_Trafico` |
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

## Módulos catalogados como faltantes
- La lista completa de módulos que todavía no se han movido del legacy (y que deben seguir la especificación en `MatrixNext/docs/GENERAL/MIGRACION_ESPECIFICACIONES.md`) incluye OP_RO, OP_Trafico, PY_ControlCalidad, PY_Adquisiciones, PNC, SG_Actas, SGC_Calidad, ES_Estadistica, Centro_Informacion, Inventario, IT, MBO (y sus variantes Gerencial/Operaciones), ResumenProduccion, RE_GT, PC_PropiedadCliente y los módulos transversales (Account, Controls, etc.).
- Cada uno debe recibir una entrada nueva en el backlog, con un plan de mapeo SP + área + menú antes de marcarlo como “en progreso”.

## Pendientes que deben continuar (Sprints 8-12)
1. **Sprint 8 (2-3 sem)**: EQ_EasyQuote Fase 1 (análisis + catálogos + infraestructura) ← **🔴 PRÓXIMO**
2. **Sprint 9 (1-2 sem)**: Home Dashboard (widgets / servicios ya corriendo; afinando datos de tareas, proyectos, ausencias y documentos)
3. **Sprint 10 (1-2 sem)**: RP_Reportes (controladores + vistas ya listos; foco en datos y exportes)
4. **Sprint 11 (2 sem)**: OP_RO + OP_Trafico (controladores + vistas existentes; cerrando integración de revisiones y tráfico)
5. **Sprint 12+ (Variable)**: Módulos baja prioridad (PY_CC, SG_Actas, SGC_Calidad, etc.)

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

Ultima revision: 2026-01-14

## Semáforo de progreso por modulo

| Módulo | Prioridad | Status resumido | Indicador |
| --- | --- | --- | --- |
| US_Usuarios | Crítica | Migrado y validado | 🟢 |
| TH_TalentoHumano (API Sprint 4) | Media | API REST completa (55 endpoints, 0 errores); Views pendientes Sprint 5 | 🟢 |
| TH_Ausencias + Empleados API | Media | API REST completa (55 endpoints, 0 errores, Sprint 4) | 🟢 |
| CU_Cuentas | Alta | Completo | 🟢 |
| CC_FinzOpe / FI | Alta | Infraestructura lista | 🟢 |
| OP_Cuantitativo | Alta | Documentado y concluido | 🟢 |
| OP_Cualitativo | Alta | Sprint 6 COMPLETADO (complementos + Bulk Import) | 🟢 |
| PY_Proyectos | Alta | Catálogos/maestros en marcha | 🟡 |
| CORE (workflow) | Alta | **SPRINT 7 COMPLETADO**: Máquina de estados + UI runtime + SignalR + reportes | 🟢 |
| GD_Documentos | Media | Controladores y vistas live en `MatrixNext.Web/Areas/GD` (Fase 5) | 🟡 |
| EQ (EasyQuote) | Crítica | Controladores/vistas en `MatrixNext.Web/Areas/EQ`; Sprint 8 cerrando catálogos | 🟡 |
| Home | Alta | HomeController + DashboardService + `Views/Home/Index` en pruebas (Sprint 9) | 🔴 |
| **RP_Reportes** | **🔴 Alta** | **ReportesController + vistas listos; foco en datos y exportes S10** | **🔴** |
| **OP_RO + OP_Trafico** | **🟠 Media-Baja** | **OP_ROController / OP_TraficoController + vistas existen; cerrando integraciones S11** | **🔴** |
| PY_ControlCalidad | Baja | Sprint 12+ | ⚪️ |
| SG_Actas | Baja | Sprint 12+ | ⚪️ |
| SGC_Calidad | Baja | Sprint 12+ | ⚪️ |
| Resto (ES_Estadistica, Centro_Informacion, IT, MBO*, etc.) | Baja | Sprint 12+ | ⚪️ |

---

## Timeline de Ejecución (Sprints 5-12 Planificados)

📊 **Ver documento detallado**: [PLAN_EJECUCION_SPRINTS_5_12.md](PLAN_EJECUCION_SPRINTS_5_12.md)

📋 **Ver planes Sprint 10 & 11**: [SPRINT_10_11_PLAN_DETALLADO.md](SPRINT_10_11_PLAN_DETALLADO.md) + [SPRINT_10_11_KICKOFF_GUIDE.md](SPRINT_10_11_KICKOFF_GUIDE.md)

| Sprint | Módulo | Duración | Fechas Estimadas | Esfuerzo | Estado |
|---|---|---|---|---|---|
| 5 | TH Views/UI | 2 sem | 2026-01-15 → 2026-01-29 | 80h | ✅ COMPLETADO |
| 6 | OP_Cualitativo Complementos + Bulk Import | 2 sem | 2026-01-13 → 2026-01-29 | 75h | ✅ COMPLETADO |
| 7 | CORE Workflow | 2 sem | 2026-02-01 → 2026-02-12 | 85h | ✅ COMPLETADO |
| **8** | **EQ_EasyQuote Fase 1** | **2-3 sem** | **2026-02-15 → 2026-03-05** | **120h** | **🔴 EN CURSO** |
| 9 | Home Dashboard | 1-2 sem | 2026-03-08 → 2026-03-19 | 50h | 🔴 PLANIFICADO |
| **10** | **RP_Reportes** | **1-2 sem** | **2026-03-22 → 2026-04-02** | **60h** | **🔴 PRÓXIMO FOCUS** |
| **11** | **OP_RO + OP_Trafico** | **2 sem** | **2026-04-05 → 2026-04-16** | **90h** | **🔴 FINAL FOCUS** |
| 12+ | Módulos Baja Prioridad | Variable | 2026-04-19+ | TBD | ⚪️ PENDIENTE |

**Total**: ~560 horas (~2.5 meses de ejecución secuencial - acelerado por Sprint 6 exitoso)

**🎯 HITO CRÍTICO**: 2026-04-16 = Fin Sprints 5-11 (100% módulos alta/media completados) ← **ADELANTADO 17 DÍAS**
