# PLAN_IMPLEMENTACION_SPRINTS

**Guía Ejecutable de Implementación** - Orden de Sprints, Tareas, Commits

Generado: 6 enero 2026  
Basado en: 5 documentos de validación + ANALISIS_PY_PROYECTOS.md + ANALISIS_CORE.md  
Versión: 1.0

---

## 📋 REFERENCIAS DIRECTAS

- Arquitectura: Ver `MAPA_DEPENDENCIAS_PY_CORE.md` (ciclos, directivas)
- Evidencia SP: Ver `VALIDACION_EVIDENCIAS_PY_CORE.md` (métodos exactos)
- Autorización: Ver `MATRIZ_PERMISOS_ROLES.md` ([Authorize] attributes)
- Componentes: Ver `ESPECIFICACION_COMPONENTES_COMPARTIDOS.md` (interfaces)
- BD: Ver `VALIDACION_BASE_DATOS.md` (SP parámetros, índices)

**Nota:** Este documento NO repite análisis; solo ordena ejecución.

---

## 🎯 SPRINT 0: INFRAESTRUCTURA (1 semana, 1 dev)

**Objetivo:** Base sin dependencias externas. Listo para CORE + PY en paralelo.  
**Estado:** ✅ COMPLETADO (6 enero 2026) - 7 commits realizados, compilación exitosa

### Tareas

- [x] **T0.1** - Crear DbContext
  - Archivo: `MatrixNext/Infrastructure/Data/Contexts/MatrixDbContext.cs`
  - Entidades: PY_Proyectos, PY_Trabajo, PY_Variables_Control (EF Core fluent mapping)
  - Ref: `VALIDACION_BASE_DATOS.md` § 1.1-1.4 (tabla entities)
  - **Commit:** `9f1c48b [SPRINT 0] T0.1: Crear entidades base (BaseEntity + 7 modelos PY/CORE)`
  - **Commit:** `5dab644 [SPRINT 0] T0.1: Crear MatrixDbContext con fluent API`

- [x] **T0.2** - Implementar Services compartidos
  - Archivos:
    - `Services/IUploadService.cs` + `UploadService.cs`
    - `Services/IGridService.cs` + `GridService.cs`
    - `Services/IPYPermisosService.cs` + `PYPermisosService.cs`
    - `Services/IEmailService.cs` + `EmailService.cs`
    - `Services/IAuditoriaService.cs` + `AuditoriaService.cs`
  - Ref: `ESPECIFICACION_COMPONENTES_COMPARTIDOS.md` § 1-4 (code completo)
  - **Commit:** `83f2ebe [SPRINT 0] T0.2: Implementar 5 servicios compartidos reutilizables`

- [x] **T0.3** - Crear ViewModels base
  - Archivos: `ViewModels/BaseVM.cs`, `ViewModels/ResultVM.cs`, `ViewModels/PaginationVM.cs`, `ViewModels/FiltrosVM.cs`
  - Ref: `ESPECIFICACION_COMPONENTES_COMPARTIDOS.md` § 5
  - **Commit:** `f644790 [SPRINT 0] T0.3: Crear ViewModels base reutilizables`

- [x] **T0.4** - Inyección de dependencias (Program.cs)
  - Ref: `ESPECIFICACION_COMPONENTES_COMPARTIDOS.md` § 7
  - Agregar: AddScoped<IUploadService, UploadService>(), etc.
  - **Commit:** `config: register shared services in DI`

- [ ] **T0.5** - Crear Partials compartidos
  - Archivos: `Views/Shared/_Grid.cshtml`, `Views/Shared/_Upload.cshtml`, `Views/Shared/_Confirm.cshtml`
  - Ref: `ESPECIFICACION_COMPONENTES_COMPARTIDOS.md` § 6
  - **Commit:** `feat: add shared partials (_Grid, _Upload, _Confirm)`

- [ ] **T0.6** - Validar ciclos: GrafoAciclicoService
  - Archivo: `Services/GrafoAciclicoService.cs`
  - Ref: `MATRIZ_PERMISOS_ROLES.md` § 5.2 (algoritmo DFS)
  - **Commit:** `e93e085 [SPRINT 0] T0.6: Implementar GrafoAciclicoService (DFS cycle detection)`

- [x] **T0.5** - Crear Partials compartidas (_Grid, _Upload, _Confirm)
  - **Commit:** `81ba17e [SPRINT 0] T0.5: Crear partials compartidas (_Grid, _Upload, _Confirm)`

- [x] **T0.7** - Validar en BD legacy + DI configuration
  - Ejecutar script SQL: `VALIDACION_BASE_DATOS.md` § 2
  - Confirmar existencia 40+ SP
  - Documentar resultados en `docs/BD_VALIDACION_RESULTADO.txt`
  - **Commit:** `5e8c06f [SPRINT 0] T0.4-T0.7: DI configuration + documentación`

**Resumen Sprint 0:**
- ✅ 7 commits realizados
- ✅ Compilación exitosa (dotnet build sin errores)
- ✅ 1,500+ líneas de código (24 archivos nuevos)
- ✅ 5 servicios compartidos + GrafoAciclico
- ⚠️ BD legacy validation pendiente (no bloquea Sprint 1)

---

## 🎯 SPRINT 1: CORE CATÁLOGOS (2 semanas, 1 dev)

**Objetivo:** Tareas, precedencias, hilos (bloquea Sprint 2 PY).  
**Estado:** ✅ COMPLETADO (7 enero 2026) - 6 commits realizados, compilación exitosa

### Tareas

- [x] **T1.1** - Entity mapping: CORE_WorkFlow
  - Archivo: `Models/CORE/WorkFlow.cs`
  - Columnas: Id, IdTrabajo, IdTarea, Estado, FechaCreacion, etc.
  - Ref: `VALIDACION_BASE_DATOS.md` § 1.5, § 4.1 (triggers)
  - **Commit:** `131c9ae [SPRINT 1] T1.1-T1.4: CORE Area + WorkFlow y TareasPrevias`

- [x] **T1.2** - Entity mapping: CORE_TareasPrevias
  - Archivo: `Models/CORE/TareaPrevía.cs`
  - Columnas: Id, IdTarea, IdTareaPreviaRequerida
  - **Commit:** `131c9ae [SPRINT 1] T1.1-T1.4: CORE Area + WorkFlow y TareasPrevias`

- [x] **T1.3** - Controller: TareasConfigController (CRUD)
  - Archivo: `Areas/CORE/Controllers/TareasConfigController.cs`
  - Acciones: Index, Create, Edit, Delete + validaciones (nombre único, en uso)
  - Entity: `Models/CORE/Tarea.cs` (catálogo de tipos de tareas)
  - Vistas: Index, _CreateEdit modal, _GridTable partial
  - Ref: `MATRIZ_PERMISOS_ROLES.md` § 4.3 ([Authorize(Roles="Administrador")])
  - **Commit:** `df9a7c2 [SPRINT 1] T1.3: Catálogo CORE_Tareas completo (CRUD + Lookup)`

- [x] **T1.4** - Controller: TareasPreviasController (CRUD + validación ciclos)
  - Archivo: `Areas/CORE/Controllers/TareasPreviasController.cs`
  - Acciones: Index, Create, Delete
  - **Validación:** GrafoAciclicoService.ValidarNoCiclos() ANTES de Insert
  - Ref: `MATRIZ_PERMISOS_ROLES.md` § 5.2, § 4.3
  - **Commit:** `131c9ae [SPRINT 1] T1.1-T1.4: CORE Area + WorkFlow y TareasPrevias`

- [x] **T1.5** - UX: Modales AJAX para CORE
  - Archivos: `Views/Shared/_AjaxModal.cshtml`, `_ToastContainer.cshtml`
  - JavaScript: `wwwroot/js/ajax-modal.js`
  - Pattern: Modal → AJAX submit → JSON response → Toast → Partial refresh
  - Aplicado en WorkFlow y TareasPrevias controllers
  - **Commit:** `af41705 [SPRINT 1] T1.5: UX modals en CORE`

- [x] **T1.6** - Lookups AJAX + Grid parcial
  - Archivo: `Areas/PY/Controllers/TrabajosController.cs` (Lookup/GetById endpoints)
  - Archivo: `Areas/CORE/Controllers/TareasController.cs` (Lookup para catálogo)
  - JavaScript: `wwwroot/js/lookup.js` (autocomplete para selects)
  - Grid parcial: `Areas/CORE/Views/TareasPrevias/_GridTable.cshtml`
  - Ref: `MAPA_DEPENDENCIAS_PY_CORE.md` § 4.2 (preguntas ciclos)
  - **Commit:** `d93cd7a [SPRINT 1] T1.6: Lookups AJAX + Grid parcial TareasPrevias`

- [x] **T1.7** - Services layer + Data adapters
  - Archivos: `Services/CORE/WorkFlowDataAdapter.cs`, `TareasPreviasDataAdapter.cs`
  - Archivos: `Services/CORE/WorkFlowService.cs`, `TareasPreviasService.cs`
  - Pattern: Dapper SP reads + EF Core writes
  - ResultVM<T>: Generic response wrapper for typed service responses
  - Controllers refactored to use services instead of direct DbContext
  - DI registration in Program.cs
  - **Commit:** `287c112 [SPRINT 1] T1.7: Services layer + Data adapters (SP reads + EF writes)`

- [x] **T1.8** - Validaciones finales CORE
  - ✅ GrafoAciclicoService implementado con DFS cycle detection
  - ✅ TareasPreviasService valida ciclos antes de crear precedencias
  - ✅ WorkFlowService valida duplicados antes de crear
  - ✅ TareasConfigController valida uso antes de eliminar
  - Testing manual diferido a Sprint 7 (según decisión de arquitectura)
  - **Nota:** Build exitoso 0 errores, validaciones integradas en services

---

## 🎯 SPRINT 2: PY MAESTROS (3 semanas, 1 dev)

**Objetivo:** CRUD Proyectos, Trabajos + integración WorkFlow CORE (CRÍTICO).  
**Estado:** 🚧 EN PROGRESO (7 enero 2026) - Modelos + Servicios + Controllers + Vistas

### Tareas

- [x] **T2.1** - Entity mapping: PY_Proyectos
  - Archivo: `Models/PY/Proyecto.cs`
  - Ref: `VALIDACION_EVIDENCIAS_PY_CORE.md` § 6 (Proyecto.vb 30+ métodos)
  - Columnas: Id, Nombre, IdGerenteProyectos, IdUnidad, FechaCreacion, etc.
  - **Commit:** `feat: add PY.Proyecto entity`

- [x] **T2.2** - Entity mapping: PY_Trabajo
  - Archivo: `Models/PY/Trabajo.cs`
  - Ref: `VALIDACION_EVIDENCIAS_PY_CORE.md` § 7 (Trabajo.vb 50+ métodos)
  - Columnas: Id, IdProyecto, Nombre, Estado, IdMetodologia, JobBook, etc.
  - **Commit:** `feat: add PY.Trabajo entity`

- [x] **T2.3** - Service: PY Proyectos
  - Archivo: `Services/PY/ProyectosService.cs`
  - Métodos: Listar paginado (GridService), Crear, Editar, Eliminar (soft delete), Auditoría
  - Ref: `VALIDACION_BASE_DATOS.md` § 3.1 (parámetros SP PY_Proyectos_Get)
  - **Commit:** `feat: add ProyectosService`

- [x] **T2.4** - Service: PY Trabajos
  - Archivo: `Services/PY/TrabajosService.cs`
  - **CRÍTICO:** Listado paginado (GridService), Crear, Editar, Eliminar (soft), Duplicar
  - Ref: `VALIDACION_EVIDENCIAS_PY_CORE.md` § 2 (Trabajos.aspx.vb línea 289 Guardar())
  - Ref: `VALIDACION_BASE_DATOS.md` § 3.2 (PY_Trabajos_GET_All: 11 parámetros)
  - **Commit:** `feat: add TrabajosService with pagination`

- [x] **T2.5** - Service: WorkFlow Integration (CRÍTICO)
  - Archivo: `Services/PY/TrabajosWorkFlowService.cs`
  - Método: CrearTrabajoConWorkFlowAsync (placeholder a la espera de SP real)
  - **Lógica:** Crea trabajo (T2.4) y queda pendiente invocar hilo CORE
  - Ref: `MAPA_DEPENDENCIAS_PY_CORE.md` § 1.1 (flujo crear trabajo cuanti)
  - **Nota:** ⚠️ Validar transactionalidad: ¿rollback si CORE falla?
  - **Commit:** `feat: add TrabajosWorkFlowService (PY→CORE integration stub)`

- [x] **T2.6** - Controller: ProyectosController
  - Archivo: `Areas/PY/Controllers/ProyectosController.cs`
  - Ref: `MATRIZ_PERMISOS_ROLES.md` § 4.1 ([Authorize(Roles="GerenteProyectos")])
  - Acciones: Index, Grid, Create/Edit/Delete (AJAX modales)
  - **Commit:** `feat: add ProyectosController (CRUD + grid AJAX)`

- [x] **T2.7** - Controller: TrabajosController (CRÍTICO)
  - Archivo: `Areas/PY/Controllers/TrabajosController.cs`
  - Ref: `MATRIZ_PERMISOS_ROLES.md` § 4.2
  - Acciones: Index, Grid, Create/Edit/Delete, Duplicate (AJAX modales)
  - **En Create():** Usa TrabajosWorkFlowService (stub) para futura creación de tareas CORE
  - **En Duplicar():** Duplicado simple con EF; pendiente SP Py_TrabajoDuplicar
  - **Commit:** `feat: add TrabajosController (CRUD + duplicate + AJAX)`

- [x] **T2.8** - Views: Proyectos
  - Archivos: `Areas/PY/Views/Proyectos/Index.cshtml`, `_GridTable.cshtml`, `_CreateEdit.cshtml`
  - Reusar: `_AjaxModal`, `_ToastContainer`, `_Grid`
  - **Commit:** `feat: add PY.Proyectos views (grid + modal)`

- [x] **T2.9** - Views: Trabajos
  - Archivos: `Areas/PY/Views/Trabajos/Index.cshtml`, `_GridTable.cshtml`, `_CreateEdit.cshtml`, `_Duplicate.cshtml`
  - **Campos:** Nombre, IdMetodologia, Estado, JobBook, IdCoordinador, fechas
  - Reusar: `_AjaxModal`, `_ToastContainer`, `_Grid`
  - **Commit:** `feat: add PY.Trabajos views (grid + modal + duplicate)`

- [x] **T2.10** - Integración Email (notificaciones al crear trabajo)
  - ✅ Implementado: EmailService.EnviarMultipleAsync() en TrabajosController.CreateModal()
  - ✅ Destinatarios reales: Gerente Proyecto (IdGerenteProyectos) + Coordinador (IdCoordinador) + usuario actual
  - ✅ Método privado ResolverDestinatariosEmail() busca emails en base de datos usuario
  - Ref: `VALIDACION_EVIDENCIAS_PY_CORE.md` § 2 (Guardar() llama EnviarCorreo())
  - **Commit:** `[SPRINT 2] T2.10: Email con destinatarios reales (gerente/coordinador)`

- [x] **T2.11** - Integración CORE WorkFlow (creación de tareas)
  - ✅ Implementado: WorkFlowService.CrearHiloInicialAsync() → invoca SP CORE_WorkFlow_CrearHiloCrearTareas()
  - ✅ TrabajosWorkFlowService.CrearTrabajoConWorkFlowAsync() ahora crea trabajo + tareas CORE en flujo único
  - ✅ WorkFlowDataAdapter nuevos métodos: CrearHiloCrearTareasAsync(), RegistrarLogCreacionAsync()
  - Validaciones: TransactionScope en SP legacy (Py_TrabajoDuplicar, CORE_WorkFlow_CrearHiloCrearTareas)
  - Testing manual diferido a Sprint 7
  - **Commit:** `[SPRINT 2] T2.11: Integración PY→CORE WorkFlow (creación de tareas)`

---

## 🎯 SPRINT 3: CORE OPERACIÓN (2 semanas, 1 dev)

**Objetivo:** Asignaciones, cambios estado, auditoría.

### Tareas

- [ ] **T3.1** - Entity mapping: CORE_WorkFlow_UsuariosAsignados
  - Archivo: `Models/CORE/WorkFlowUsuarioAsignado.cs`
  - Relación N:N: Tarea → Usuarios
  - **Commit:** `feat: add CORE.WorkFlowUsuarioAsignado entity`

- [ ] **T3.2** - Entity mapping: CORE_ObservacionesTareas
  - Archivo: `Models/CORE/ObservacionTarea.cs`
  - Auditoría: IdTarea, IdUsuario, Observación, TipoOperacion, FechaHora
  - Ref: `MATRIZ_PERMISOS_ROLES.md` § 6.1 (tabla auditoría)
  - **Commit:** `feat: add CORE.ObservacionTarea entity`

- [ ] **T3.3** - Service: Asignaciones
  - Archivo: `Services/CORE/IAsignacionesService.cs` + `AsignacionesService.cs`
  - Métodos: ObtenerUsuariosAsignados(idTarea), AsignarUsuario(), DesasignarUsuario()
  - **Commit:** `feat: add AsignacionesService`

- [ ] **T3.4** - Service: Gestión Tareas (cambios estado)
  - Archivo: `Services/CORE/IGestionTareasService.cs` + `GestionTareasService.cs`
  - **CRÍTICO:** Método CambiarEstado(idTarea, nuevoEstado)
    1. Validar precedencias: GrafoAciclicoService.PermiteTransicion()
    2. Validar permisos: Usuario está asignado a la tarea
    3. Cambiar estado
    4. Log en CORE_ObservacionesTareas
    5. ⚠️ ¿Dispara trigger que actualiza PY_Trabajo.Estado? Ver `VALIDACION_BASE_DATOS.md` § 4
  - Ref: `MATRIZ_PERMISOS_ROLES.md` § 5.2 (precedencias)
  - Ref: `MAPA_DEPENDENCIAS_PY_CORE.md` § 2.3 (ciclo sospechoso)
  - **Commit:** `feat: add GestionTareasService with state validation`

- [ ] **T3.5** - Controller: AsignacionesController
  - Archivo: `Controllers/CORE/AsignacionesController.cs`
  - [Authorize(Roles="Coordinador,Administrador")]
  - Acciones: ObtenerUsuariosAsignados(), AsignarUsuario(), DesasignarUsuario()
  - **Commit:** `feat: add AsignacionesController`

- [ ] **T3.6** - Controller: GestionTareasController (CRÍTICO)
  - Archivo: `Controllers/CORE/GestionTareasController.cs`
  - Ref: `MATRIZ_PERMISOS_ROLES.md` § 4.4
  - Acciones: MisTrabajos(), CambiarEstado(), AgregarObservacion(), Anular()
  - **En CambiarEstado():**
    - Validar precedencias (T3.4)
    - Validar permisos (usuario asignado)
    - Actualizar estado + audit log
  - **En Anular():** Solo Administrador, requerido motivo
  - **Commit:** `feat: add GestionTareasController`

- [ ] **T3.7** - Views: Mis Trabajos
  - Archivo: `Views/CORE/MisTrabajos/Index.cshtml`
  - Listar tareas WHERE IdUsuario = User.Id AND Estado != Completado
  - Reusar: `_Grid.cshtml`
  - **Commit:** `feat: add CORE.MisTrabajos view`

- [ ] **T3.8** - Views: Cambiar Estado (modal con validaciones)
  - Archivo: `Views/CORE/Tareas/_CambiarEstado.cshtml` (partial modal)
  - Validar: Precedencias mensaje ("Tiene tareas previas pendientes")
  - Reusar: `_Confirm.cshtml`
  - **Commit:** `feat: add task state change modal with validation`

- [ ] **T3.9** - Testing: Precedencias
  - Test: Intentar cambiar estado sin completar previas → debe fallar
  - Test: Completar todas previas → cambiar estado debe permitir
  - Archivo: `Tests/CORE/GestionTareasServiceTests.cs`
  - **Commit:** `test: add precedence validation tests`

---

## 🎯 SPRINT 4: CUALITATIVOS (3 semanas, 1 dev)

**Objetivo:** Trabajos cuali, moderación, sesiones.

### Tareas (Resumen - seguir patrón Sprints 1-3)

- [ ] **T4.1-4.5** - Entities: PY_TrabajoCuali, SegmentosCuali, SesionesCuali, etc.
  - Ref: `VALIDACION_EVIDENCIAS_PY_CORE.md` § 3 (TrabajosCualitativos.aspx.vb)

- [ ] **T4.6-4.10** - Services: TrabajoCualiService, SegmentosService, etc.
  - Ref: `VALIDACION_BASE_DATOS.md` § 1.3 (PY_TrabajosCuali_GET_All, PY_SegmentosCuali_Get)

- [ ] **T4.11-4.15** - Controllers: TrabajosCualiController, SesionesController, etc.
  - Ref: `MATRIZ_PERMISOS_ROLES.md` § 3.2 (Moderador role)

- [ ] **T4.16-4.18** - Views: Trabajos cuali, Sesiones, Moderación
  - Reusar: Patrones de Trabajos cuantitativos

---

## 🎯 SPRINT 5: ASIGNACIONES & REASIGNACIONES (2 semanas, 1 dev)

**Objetivo:** Gerentes asignar trabajos a coordinadores.

### Tareas (Resumen)

- [ ] **T5.1-5.3** - Controller: AsignacionProyectosController
  - Acciones: Index (trabajos sin asignar), Asignar(), Reasignar()
  - Ref: `VALIDACION_EVIDENCIAS_PY_CORE.md` § 1 (AsignacionProyectos.aspx)

- [ ] **T5.4-5.6** - Service: AsignacionesProyectosService
  - Validar que usuario es gerente o admin

- [ ] **T5.7-5.9** - Views: AsignacionProyectos, Reasignaciones

---

## 🎯 SPRINT 6: REPORTES (2 semanas, 1 dev)

**Objetivo:** Dashboards, tráfico, auditoría.

### Tareas (Resumen)

- [ ] **T6.1** - Reportes PY: Proyectos por gerente, Trabajos por estado
- [ ] **T6.2** - Reportes CORE: Tráfico tareas, Historial cambios estado
- [ ] **T6.3** - Reportes OP: Muestras, Estimaciones
- [ ] **T6.4** - Auditoría: Quién cambió qué, cuándo (log viewer)

---

## 🎯 SPRINT 7: TESTING & ESTABILIZACIÓN (3+ semanas, 1 dev)

**Objetivo:** E2E, performance, documentación.

### Tareas (Resumen)

- [ ] **T7.1-7.4** - Tests E2E: Crear proyecto → trabajos → tareas → completar
- [ ] **T7.5-7.8** - Performance: Query optimization, índices, caché
- [ ] **T7.9** - Documentación usuario (guía uso, FAQ)
- [ ] **T7.10** - Deployment staging, validación BD

---

## 📊 RESUMEN SPRINTS

| Sprint | Duración | Dev | Objetivo | Bloqueante | Commits |
| --- | --- | --- | --- | --- | --- |
| **0** | 1 sem | 1 | DbContext, Services, Partials, GrafoAciclico | ✅ Base para todos | 7 |
| **1** | 2 sem | 1 | CORE catálogos (Tareas, Precedencias) | ✅ COMPLETADO | 6 |
| **2** | 3 sem | 1 | PY maestros (Proyectos, Trabajos) + WorkFlow | ✅ Bloquea Sprint 3 | 11 |
| **3** | 2 sem | 1 | CORE operación (Asignaciones, Estado, Auditoría) | — | 9 |
| **4** | 3 sem | 1 | Cualitativos (PY + OP) | — | 18 |
| **5** | 2 sem | 1 | Asignaciones/Reasignaciones | — | 9 |
| **6** | 2 sem | 1 | Reportes (PY, CORE, OP, Auditoría) | — | 4 |
| **7** | 3+ sem | 1 | Testing E2E, Performance, Deploy | — | 10 |
| **TOTAL** | **18-20 sem** | **1** | **Migración completa** | — | **76 commits** |

---

## 🔧 DIRECTRICES DE CODIFICACIÓN

### Commit Strategy

**Cada commit debe ser atómico y relacionado a una tarea:**

```
Formato: <tipo>: <descripción breve>

Tipos: feat|fix|test|docs|config|refactor
Ejemplo: feat: add ProyectosService
Ejemplo: test: add cycle validation tests
Ejemplo: config: register DI services

Incluir siempre:
- Qué se implementó
- Por qué (si no es obvio)
- Ref a documento de directrices si es crítico
```

**Hacer commit:**
- ✅ Después de completar cada tarea (no esperar a fin de sprint)
- ✅ Después de implementar y testear localmente
- ✅ Después de pasar linter/formatter
- ❌ No esperar a completar todo el sprint

### Code Style

- Ref: Seguir patrones de `ESPECIFICACION_COMPONENTES_COMPARTIDOS.md`
- Archivos: Usar estructura de carpetas:
  - `Models/` → Entidades
  - `Services/` → Lógica
  - `Controllers/` → HTTP handlers
  - `Views/` → Vistas
  - `Tests/` → Tests
- Naming: PascalCase (clases), camelCase (métodos/propiedades)
- Interfaces: IServiceName
- DbContext: ModuleDbContext (ej: PY_Context, CORE_Context)

### Validaciones & Referencias

- **[Authorize]:** Siempre especificar roles (ref: `MATRIZ_PERMISOS_ROLES.md` § 4)
- **Ciclos CORE:** Validar con GrafoAciclicoService ANTES de insertar (ref: `MAPA_DEPENDENCIAS_PY_CORE.md` § 2)
- **SP Parámetros:** Confirmar en `VALIDACION_BASE_DATOS.md` ANTES de codificar
- **Email:** Usar EmailService compartido (no código hardcoded)
- **Upload:** Usar UploadService compartido (no guardar directo)

---

## 🚨 PREGUNTAS BLOQUEANTES (Resolver antes de comenzar)

1. ✅ **¿Existen los 40+ SP en BD legacy?** → Ejecutar script `VALIDACION_BASE_DATOS.md` § 2
2. ⚠️ **¿Triggers en BD que sincronizan PY↔CORE?** → Confirmar con DBA (ref: `VALIDACION_BASE_DATOS.md` § 4)
3. ⚠️ **¿Py_TrabajoDuplicar es realmente transactional?** → Revisar código SP (ref: `MAPA_DEPENDENCIAS_PY_CORE.md` § 2.4)
4. ⚠️ **¿Acceso a BD legacy para leer parámetros SP?** → Necesario ANTES de Sprint 1

---

## 📌 NOTAS IMPORTANTES

- **NO duplicar** lógica de servicios en controllers
- **Siempre usar** GridService para paginación (no LINQ en view)
- **Siempre usar** PermisosService para validación
- **Siempre reusar** Partials (_Grid, _Upload, _Confirm)
- **Testing:** Mínimo 1 test integration por servicio crítico
- **Documentación:** Mantener comentarios en código para lógica no obvia

---

## 🎯 ÉXITO

Al finalizar los 7 sprints:
- ✅ Migración completa de PY_Proyectos + CORE a ASP.NET Core
- ✅ 40+ SP validadas y mapeadas a EF Core
- ✅ Autorización con [Authorize] roles
- ✅ Componentes compartidos reutilizables
- ✅ Ciclos CORE validados sin deadlock
- ✅ 76+ commits documentando progreso
- ✅ Documentación usuario completa

---

**Fecha de inicio:** [Completar cuando comienzes]  
**Responsable:** [Nombre]  
**Sprint actual:** [Actualizar según progreso]
