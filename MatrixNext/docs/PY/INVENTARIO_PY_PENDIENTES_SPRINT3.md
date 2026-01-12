# Inventario PY_Proyectos Pendientes - Sprint 3 (2026-01-11)

## Alcance y fuentes consultadas
- WebForms legacy: carpeta WebMatrix/PY_Proyectos (18 archivos .aspx/.vb)
- Implementaciones existentes MatrixNext: Areas/PY/Controllers (10 controllers)
- Documento referencia: [MIGRACION_PY_PROYECTOS.md](MIGRACION_PY_PROYECTOS.md)
- Backlog global: [BACKLOG_MIGRACION_GLOBAL.md](../GENERAL/BACKLOG_MIGRACION_GLOBAL.md)

## Inventario legacy vs MatrixNext

### Controllers ya implementados en MatrixNext ✅
| Controller MatrixNext | WebForm Legacy | Estado |
|---|---|---|
| AsignacionesProyectosController | AsignacionProyectos.aspx | ✅ Implementado |
| DashboardController | Home.aspx / Default.aspx | ✅ Implementado |
| EntrevistadorasCualiController | (parte cuali) | ✅ Implementado |
| MuestrasCualiController | (parte cuali) | ✅ Implementado |
| ProyectosController | PY_Proyectos.aspx | ✅ Implementado |
| SegmentosCualiController | SegmentosCuali.aspx | ✅ Implementado |
| SesionesCualiController | Sesiones.aspx | ✅ Implementado |
| TrabajosCualiController | TrabajosCualitativos.aspx | ✅ Implementado |
| TrabajosController | Trabajos.aspx | ✅ Implementado |
| UnidadesController | (catálogo) | ✅ Implementado |

### WebForms legacy PENDIENTES de migrar ⚠️
| WebForm Legacy | Funcionalidad | Controller objetivo | Estado Sprint 3 |
|---|---|---|---|
| **InHomeVisit.aspx** | Gestión visitas en casa | InHomeVisitController | 🔴 PENDIENTE |
| **VariablesControl.aspx** | Variables de control | VariablesControlController | 🔴 PENDIENTE |
| **InstructivoGeneral.aspx** | Instructivos cuantitativos | InstructivosController | 🔴 PENDIENTE |
| **InstructivoGeneralCuali.aspx** | Instructivos cualitativos | InstructivosController | 🔴 PENDIENTE |
| **RegistroPlanillasCualitativo.aspx** | Planillas de campo | PlanillasController | 🔴 PENDIENTE |
| **DuplicarTrabajos.aspx** | Duplicación de trabajos | (acción en TrabajosController?) | 🔴 PENDIENTE |
| **DistribucionEntrevistas.aspx** | Planeación entrevistas | DistribucionController | 🔴 PENDIENTE |
| REAsignacionProyectos.aspx | Reasignaciones | (extensión Asignaciones?) | ⚪ Revisar necesidad |

## Priorización Sprint 3 según backlog global

Según [BACKLOG_MIGRACION_GLOBAL.md](../GENERAL/BACKLOG_MIGRACION_GLOBAL.md) punto 2):
> Pendientes indicados: **InHomeVisit, VariablesControl, Instructivos/Planillas, DuplicarTrabajos, DistribucionEntrevistas**

### Alcance Sprint 3 (priorizados)
1. **InHomeVisit** (visitas en casa)
2. **VariablesControl** (variables de control)
3. **Instructivos** (General + Cuali, carga/descarga)
4. **Planillas** (RegistroPlanillasCualitativo)
5. **DuplicarTrabajos** (duplicación)
6. **DistribucionEntrevistas** (distribución)

## Plan de implementación Sprint 3

### Día 1-2: Inventario y mapeo detallado
- [ ] Listar SP usados en cada .aspx.vb pendiente
- [ ] Extraer parámetros y tipos desde CoreProject (PY_* / PY_Cuali_*)
- [ ] Validar SP en CO_Matrix_Structure_SP.sql
- [ ] Documentar en MAPEO_ACCION_SP_PY_PENDIENTES.md

### Día 2-4: Adapters y Services
- [ ] Crear InHomeVisitAdapter (Dapper) + InHomeVisitService
- [ ] Crear VariablesControlAdapter + VariablesControlService
- [ ] Crear InstructivosAdapter + InstructivosService (compartido cuanti/cuali)
- [ ] Crear PlanillasAdapter + PlanillasService
- [ ] Extender TrabajosAdapter/Service con acción Duplicar
- [ ] Crear DistribucionAdapter + DistribucionService
- [ ] Registrar DI en Program.cs

### Día 3-6: Controllers y Views
- [ ] InHomeVisitController: CRUD visitas + Index/CreateModal/EditModal
- [ ] VariablesControlController: CRUD variables + Index/CreateModal/EditModal
- [ ] InstructivosController: Upload/Download instructivos + Index/UploadModal
- [ ] PlanillasController: Upload/registro planillas + Index/UploadModal
- [ ] TrabajosController.Duplicate (POST): acción duplicar con confirmación
- [ ] DistribucionController: distribuir entrevistas + Index/DistributeModal
- [ ] Views AJAX-first con modales (Regla 5.1)

### Día 5-6: Componente de carga compartido (si no existe)
- [ ] Verificar si existe componente upload reutilizable
- [ ] Si no: crear _UploadFrame.cshtml parcial + helper JS
- [ ] Backend: endpoint común UploadController (metadata + archivo)
- [ ] Reutilizar en Instructivos y Planillas

### Día 6-7: QA funcional
- [ ] Pruebas CRUD InHomeVisit (crear/editar/eliminar visitas)
- [ ] Pruebas CRUD VariablesControl (crear/editar/eliminar variables)
- [ ] Pruebas carga/descarga Instructivos (cuanti + cuali)
- [ ] Pruebas carga/registro Planillas
- [ ] Pruebas duplicar trabajos (validar clonación de relaciones)
- [ ] Pruebas distribución entrevistas (asignación por unidad/ciudad)
- [ ] Validar permisos [Authorize] en todos los controllers
- [ ] Build limpio sin errores

### Día 7: Documentación
- [ ] Actualizar MIGRACION_PY_PROYECTOS.md con cierre
- [ ] Crear MAPEO_ACCION_SP_PY_PENDIENTES.md
- [ ] Actualizar BACKLOG_MIGRACION_GLOBAL.md (Sprint 3 cerrado)
- [ ] Evidencias QA documentadas

## Criterios de terminación Sprint 3
- ✅ 6 funcionalidades pendientes implementadas (InHomeVisit, VariablesControl, Instructivos, Planillas, DuplicarTrabajos, DistribucionEntrevistas)
- ✅ Adapters/Services/Controllers/Views completos con AJAX-first
- ✅ SP legacy mapeados y ejecutados correctamente
- ✅ Componente de carga reutilizable (si aplica)
- ✅ Build limpio sin errores
- ✅ QA funcional ejecutado y documentado
- ✅ Documentación actualizada

## Notas
- Componente carga: verificar si CU_Cuentas/Frame.aspx ya migrado o reutilizar patrón GD_RepositorioController
- DuplicarTrabajos: asegurar transacción y copia de relaciones (trabajos, especificaciones, variables)
- DistribucionEntrevistas: validar lógica de asignación por unidad/ciudad/metodología
- Todos los SP deben confirmarse en CO_Matrix_Structure_SP.sql antes de implementar adapters
