# Migración CORE (Workflows y Tareas) – Inventario y Plan

**Propósito:** migrar el módulo CORE (gestión de hilos, tareas, documentos y workflow) de WebMatrix a MatrixNext con paridad funcional, siguiendo DIRECTRICES_MIGRACION.md y sincronizado con DASHBOARD_MIGRACION y MODULOS_MIGRACION.

## Alcance y dependencias
- Vistas WebForms en WebMatrix/CORE (14 principales) que soportan configuración y operación de tareas para proyectos/trabajos.
- Dependencias: US_Usuarios (roles/unidades), CU_Cuentas y PY_Proyectos (trabajos), reportes REP, catálogos CORE (tipos de hilo/tarea, estados), componente de carga de archivos.
- Datos: CoreProject contiene CORE_Model.edmx + wrappers; operaciones combinan CRUD simple y SP complejas.

## Inventario de vistas WebMatrix/CORE
| Página (.aspx) | Funcionalidad principal | Acciones clave | Notas de migración |
| --- | --- | --- | --- |
| Tareas / Tarea | Maestro de tareas | Alta/edición, estados, asignación responsable | Modales para CRUD; catálogos de estados/tipos. |
| Gestion-Tareas | Gestión operativa | Cambio de estado, comentarios, filtros | Requiere permisos por rol/unidad. |
| Gestion-Tareas-Trabajos | Gestión por trabajo | Ver tareas de un trabajo, actualizar avance | Integrar con PY/CU trabajos. |
| TaskManagementJobs | Panel tareas por job | Dashboard de tareas por job | Requiere métricas resumidas. |
| ListaTrabajosTareas | Listado tareas por trabajo | Filtros por estado/responsable | Reutilizar grid compartido. |
| ListaTareasXHilo / ListaDocumentosXHilos | Tareas y documentos por hilo | Listados, filtros, descargas | Reutilizar componente de archivos. |
| ListaTareas-Trafico | Trafico de tareas | Vista de cola/prioridades | Validar ordenamiento y estados. |
| AsignacionTareas | Asignar responsables | Alta/edición de asignaciones | Validar reglas de negocio de unidades/roles. |
| Configuracion_Tareas | Configuración catálogo de tareas | Crear/editar plantillas | Respeta nombres/IDs de catálogo. |
| Configuracion_Tareas_Previas | Precedencias | Definir dependencias entre tareas | Mantener lógica de validación de ciclos. |
| ConfiguracionTareasXHilo | Tareas por tipo de hilo | Mapear plantillas a hilos | Requiere catálogos de hilos. |
| Configuracion_Tareas_Documentos | Documentos requeridos por tarea | Alta/edición de requerimientos | Usar componente de carga; validar obligatoriedad. |
| Documentos_Tareas | Gestión de documentos | Cargar/descargar, validar estado | Reutilizar componente de carga de CU_Cuentas/Frame.aspx. |
| EstimacionTareas | Estimación de tiempos | Capturar estimaciones, métricas | Salvar en SP; habilitar export si aplica. |

## Mapa de migración (fases)
- **Fase 0 – Base técnica y catálogos:**
  - Crear área CORE en MatrixNext.Web con rutas y DI.
  - Scaffolding de modelos en MatrixNext.Data (namespace MatrixNext.Data.CORE) desde CoreProject.
  - Reusar componente de carga de archivos compartido para Documentos_Tareas y Configuracion_Tareas_Documentos.
- **Fase 1 – Configuración:**
  - Configuracion_Tareas, Configuracion_Tareas_Previas, ConfiguracionTareasXHilo, Configuracion_Tareas_Documentos.
  - Validar catálogos y reglas de precedencia; asegurar SP de guardado y consulta.
- **Fase 2 – Operación:**
  - Tareas, Gestion-Tareas, Gestion-Tareas-Trabajos, TaskManagementJobs, ListaTrabajosTareas, ListaTareas-Trafico.
  - Permisos por rol/unidad, filtros, cambios de estado, comentarios.
- **Fase 3 – Documentos y estimaciones:**
  - Documentos_Tareas, ListaDocumentosXHilos, EstimacionTareas.
  - Cargas/descargas, validaciones de obligatoriedad, métricas de estimación.
- **Cierre:** checklist de paridad y actualización de [DASHBOARD_MIGRACION.md](../DASHBOARD_MIGRACION.md) y [MODULOS_MIGRACION.md](../MODULOS_MIGRACION.md).

## Capa de datos (CoreProject) a migrar
- **Contexto:** CORE_Model.edmx y clases afines (CORE_Hilos, CORE_Tareas, CORE_WorkFlow, CORE_WorkFlow_TareasPrevias, CORE_TipoHilos, CORE_Tareas_Documentos, CORE_ObservacionesTareas, CORE_TareasXUnidadEjecuta, CORE_UsuariosAsignadosXProceso, CORE_WorkFlow_UsuariosAsignados, CORE_Planeacion, CORE_Retroalimentacion, CORE_DocumentosXHilo).
- **SP/result wrappers:** CORE_Tareas_Get_Result, CORE_WorkFlow_GetXTrabajoXTarea_Result, CORE_WorkFlow_TareasPrevias_Get_Result, CORE_obtenerusuariosasignados_get_Result, CORE_DocumentosRequeridosXTarea_Get_Result, CORE_DocumentosXHilo_Get_Result, CORE_TrabajosTareas_Get_Result, CORE_Trabajos_WithWorkFlow_Result, CORE_Configuracion_TareasXTipoHilo_Get_Result.
- **Estrategia acceso datos:** EF Core para CRUD de catálogos/plantillas; Dapper para SP de workflow, asignaciones, documentos y listados pesados.
- **Adapters sugeridos:** TareaDataAdapter, WorkFlowDataAdapter, HiloDataAdapter, DocumentoDataAdapter, EstimacionDataAdapter, AsignacionDataAdapter.
- **Servicios:** TareaService, WorkFlowService, DocumentoService, EstimacionService con validaciones y logging.

## Integración con otros módulos
- PY/CU: tareas asociadas a trabajos/proyectos; respetar IdTrabajo/IdProyecto.
- US_Usuarios: permisos y roles para asignación/aprobación.
- Reutilizar componente de carga de archivos para adjuntos.

## Checklist de paridad
- [ ] CRUD en modales con AJAX/JSON + toasts + refresh parcial (sin navegación completa).
- [ ] Cada vista WebForms tiene Razor equivalente con acciones y validaciones migradas.
- [ ] Catálogos (tipos de hilo/tarea/estado) cargan con nombres intactos.
- [ ] Precedencias y asignaciones se guardan y consultan vía SP originales.
- [ ] Cambios de estado registran auditoría/observaciones.
- [ ] Documentos de tarea se cargan/descargan con misma validación y llaves.
- [ ] Filtros y paginación replican comportamiento legacy.
- [ ] Métricas/estimaciones calculan igual y se exportan si aplica.
- [ ] Roles/permisos aplicados y verificados.
- [ ] Logging y manejo de errores homogéneo.
- [ ] Documentación y dashboard actualizados tras cada fase.

### Lineamientos UX (CORE)
- Modales AJAX para todas las altas/ediciones y confirmaciones.
- POST responde JSON en éxito; errores devuelven parcial con validaciones.
- Notificaciones con `toast` no bloqueantes; sin recargar la página.
- Refresco parcial de grillas/contadores usando `data-grid-url`.

## Pruebas mínimas
- Acceso autorizado a cada vista.
- Crear/editar/eliminar catálogos y tareas.
- Asignar tareas y cambiar estados respetando precedencias.
- Adjuntar y descargar documentos obligatorios y opcionales.
- Validar listas y filtros (por estado, responsable, trabajo, hilo).
- Validar estimaciones y cualquier exportación asociada.
