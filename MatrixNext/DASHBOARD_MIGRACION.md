# DASHBOARD DE MIGRACION - Estado actual

Fecha de corte: 2026-01-11

## Resumen corto
- OP_Cualitativo: migracion MVP completa (Sprint 5) y documentada.
- GD_Documentos: Fase 5 (PNC + configuraciones) completa; Fases 1-4 pendientes.
- Documentacion reorganizada por modulo en `MatrixNext/docs/`.

## Estado por modulo (WebMatrix -> MatrixNext)

| Modulo | Estado | Evidencia principal |
| --- | --- | --- |
| US_Usuarios | Completo | `MatrixNext/MatrixNext.Web/Areas/US` |
| TH_TalentoHumano | Parcial | `MatrixNext/MatrixNext.Web/Areas/TH` + `MatrixNext/docs/TH/ANALISIS_TH_EMPLEADOS.md` |
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

## Pendientes que deben continuar
1. GD_Documentos Fases 1-4 (infraestructura, catalogos, maestro, workflow).
2. PY_Proyectos (faltan InHomeVisit, VariablesControl, Instructivos/Planillas, DuplicarTrabajos, DistribucionEntrevistas).
3. TH_TalentoHumano (Empleados, Nomina, otros submodulos).
4. Home y RP_Reportes (segun prioridad de negocio).
5. OP_RO y OP_Trafico.
6. Resto de modulos legacy de baja prioridad.

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
| TH_TalentoHumano | Media | En ejecución (ausencias completado) | 🟡 |
| TH_Ausencias | Media | Líder en producción | 🟢 |
| CU_Cuentas | Alta | Completo | 🟢 |
| CC_FinzOpe / FI | Alta | Infraestructura lista | 🟢 |
| OP_Cuantitativo | Alta | Documentado y concluido | 🟢 |
| OP_Cualitativo | Alta | MVP terminado; complementos en Sprint 6 | 🟡 |
| PY_Proyectos | Alta | Catálogos/maestros en marcha | 🟡 |
| CORE (workflow) | Alta | Parcial, requiere más dependencias | 🟡 |
| GD_Documentos | Media | Fase 5 cerrada; Fases 1-4 pendientes | 🟡 |
| EQ (EasyQuote) | Crítica | En progreso | 🟡 |
| SGC_Calidad | Baja | Planeado (sin ejecución) | ⚪️ |
| Home / RP_Reportes / OP_RO / OP_Trafico / PY_S etc. | Baja | Pendientes | 🔴 |

Usa este semáforo como referencia rápida para los standups; los detalles de cada módulo todavía están en la tabla anterior y en `MatrixNext/MODULOS_MIGRACION.md`.
