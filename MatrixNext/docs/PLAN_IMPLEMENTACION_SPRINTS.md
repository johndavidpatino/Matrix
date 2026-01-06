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

### Tareas

- [ ] **T0.1** - Crear DbContext
  - Archivo: `MatrixNext/Infrastructure/Data/Contexts/MatrixDbContext.cs`
  - Entidades: PY_Proyectos, PY_Trabajo, PY_Variables_Control (EF Core fluent mapping)
  - Ref: `VALIDACION_BASE_DATOS.md` § 1.1-1.4 (tabla entities)
  - **Commit:** `feat: add MatrixDbContext with PY+CORE+OP entities`

- [ ] **T0.2** - Implementar Services compartidos
  - Archivos:
    - `Services/IUploadService.cs` + `UploadService.cs`
    - `Services/IGridService.cs` + `GridService.cs`
    - `Services/IPermisosService.cs` + `PermisosService.cs`
    - `Services/IEmailService.cs` + `EmailService.cs`
    - `Services/IAuditoriaService.cs` + `AuditoriaService.cs`
  - Ref: `ESPECIFICACION_COMPONENTES_COMPARTIDOS.md` § 1-4 (code completo)
  - **Commit:** `feat: implement shared services (Upload, Grid, Permisos, Email)`

- [ ] **T0.3** - Crear ViewModels base
  - Archivos: `ViewModels/BaseVM.cs`, `ViewModels/ResultVM.cs`, `ViewModels/PaginationVM.cs`, `ViewModels/FiltrosVM.cs`
  - Ref: `ESPECIFICACION_COMPONENTES_COMPARTIDOS.md` § 5
  - **Commit:** `feat: add base ViewModels`

- [ ] **T0.4** - Inyección de dependencias (Program.cs)
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
  - **Commit:** `feat: implement acyclic graph validator for CORE tasks`

- [ ] **T0.7** - Validar en BD legacy
  - Ejecutar script SQL: `VALIDACION_BASE_DATOS.md` § 2
  - Confirmar existencia 40+ SP
  - Documentar resultados en `docs/BD_VALIDACION_RESULTADO.txt`
  - **Commit:** `docs: legacy database SP validation results`

---

## 🎯 SPRINT 1: CORE CATÁLOGOS (2 semanas, 1 dev)

**Objetivo:** Tareas, precedencias, hilos (bloquea Sprint 2 PY).

### Tareas

- [ ] **T1.1** - Entity mapping: CORE_WorkFlow
  - Archivo: `Models/CORE/WorkFlow.cs`
  - Columnas: Id, IdTrabajo, IdTarea, Estado, FechaCreacion, etc.
  - Ref: `VALIDACION_BASE_DATOS.md` § 1.5, § 4.1 (triggers)
  - **Commit:** `feat: add CORE.WorkFlow entity`

- [ ] **T1.2** - Entity mapping: CORE_TareasPrevias
  - Archivo: `Models/CORE/TareaPrevía.cs`
  - Columnas: Id, IdTarea, IdTareaPreviaRequerida
  - **Commit:** `feat: add CORE.TareaPrevía entity`

- [ ] **T1.3** - Controller: TareasConfigController (CRUD)
  - Archivo: `Controllers/Configuracion/TareasConfigController.cs`
  - Acciones: Index, Create, Edit, Delete
  - Ref: `MATRIZ_PERMISOS_ROLES.md` § 4.3 ([Authorize(Roles="Administrador")])
  - **Commit:** `feat: add TareasConfigController (CRUD)`

- [ ] **T1.4** - Controller: TareasPreviasController (CRUD + validación ciclos)
  - Archivo: `Controllers/Configuracion/TareasPreviasController.cs`
  - Acciones: Index, Create, Delete
  - **Validación:** GrafoAciclicoService.ValidarNoCiclos() ANTES de Insert
  - Ref: `MATRIZ_PERMISOS_ROLES.md` § 5.2, § 4.3
  - **Commit:** `feat: add TareasPreviasController with cycle validation`

- [ ] **T1.5** - Service: CORE Tareas
  - Archivo: `Services/CORE/ITareasService.cs` + `TareasService.cs`
  - Métodos: ObtenerTareas(), CrearTarea(), ActualizarTarea(), EliminarTarea()
  - **Commit:** `feat: add TareasService`

- [ ] **T1.6** - Service: CORE Precedencias
  - Archivo: `Services/CORE/ITareasPreviasService.cs` + `TareasPreviasService.cs`
  - Métodos: ObtenerPrecedencias(), CrearPrecedencia(), ValidarNoCiclos()
  - Ref: `MAPA_DEPENDENCIAS_PY_CORE.md` § 4.2 (preguntas ciclos)
  - **Commit:** `feat: add TareasPreviasService with validation`

- [ ] **T1.7** - Views: Tareas & Precedencias (grillas)
  - Archivos: `Views/Configuracion/TareasConfig/Index.cshtml`, `Create.cshtml`, Edit.cshtml
  - Archivos: `Views/Configuracion/TareasPrevias/Index.cshtml`, `Create.cshtml`
  - Reusar: `_Grid.cshtml` partial
  - **Commit:** `feat: add CORE configuration views`

- [ ] **T1.8** - Validaciones finales CORE
  - Asegura que no hay ciclos en datos existentes
  - Test: Intentar crear ciclo → debe fallar con error amable
  - **Commit:** `test: verify cycle detection in CORE workflow`

---

## 🎯 SPRINT 2: PY MAESTROS (3 semanas, 1 dev)

**Objetivo:** CRUD Proyectos, Trabajos + integración WorkFlow CORE (CRÍTICO).

### Tareas

- [ ] **T2.1** - Entity mapping: PY_Proyectos
  - Archivo: `Models/PY/Proyecto.cs`
  - Ref: `VALIDACION_EVIDENCIAS_PY_CORE.md` § 6 (Proyecto.vb 30+ métodos)
  - Columnas: Id, Nombre, IdGerenteProyectos, IdUnidad, FechaCreacion, etc.
  - **Commit:** `feat: add PY.Proyecto entity`

- [ ] **T2.2** - Entity mapping: PY_Trabajo
  - Archivo: `Models/PY/Trabajo.cs`
  - Ref: `VALIDACION_EVIDENCIAS_PY_CORE.md` § 7 (Trabajo.vb 50+ métodos)
  - Columnas: Id, IdProyecto, Nombre, Estado, IdMetodologia, JobBook, etc.
  - **Commit:** `feat: add PY.Trabajo entity`

- [ ] **T2.3** - Service: PY Proyectos
  - Archivo: `Services/PY/IProyectosService.cs` + `ProyectosService.cs`
  - Métodos: ObtenerXGerenteProyectos(), ObtenerTodos(), Crear(), Editar(), Eliminar()
  - Ref: `VALIDACION_BASE_DATOS.md` § 3.1 (parámetros SP PY_Proyectos_Get)
  - **Commit:** `feat: add ProyectosService`

- [ ] **T2.4** - Service: PY Trabajos
  - Archivo: `Services/PY/ITrabajosService.cs` + `TrabajosService.cs`
  - **CRÍTICO:** Método ListadoTrabajos(filtros) → GridService.PaginarAsync()
  - Ref: `VALIDACION_EVIDENCIAS_PY_CORE.md` § 2 (Trabajos.aspx.vb línea 289 Guardar())
  - Ref: `VALIDACION_BASE_DATOS.md` § 3.2 (PY_Trabajos_GET_All: 11 parámetros)
  - Métodos: ObtenerXProyecto(), ObtenerXId(), ListadoTrabajos(), Crear(), Editar(), Eliminar(), Duplicar()
  - **Commit:** `feat: add TrabajosService with pagination`

- [ ] **T2.5** - Service: WorkFlow Integration (CRÍTICO)
  - Archivo: `Services/PY/TrabajosWorkFlowService.cs`
  - Método: CrearHiloCrearTareas(idTrabajo, idProyecto)
  - **Lógica:** 
    1. Guardar PY_Trabajo (T2.4)
    2. Llamar WorkFlow.CrearHiloCrearTareas() (CORE)
    3. Log en CORE_ObservacionesTareas
  - Ref: `VALIDACION_EVIDENCIAS_PY_CORE.md` § 2 (Trabajos.aspx línea 322)
  - Ref: `MAPA_DEPENDENCIAS_PY_CORE.md` § 1.1 (flujo crear trabajo cuanti)
  - **Nota:** ⚠️ Validar transactionalidad: ¿rollback si CORE falla?
  - **Commit:** `feat: add TrabajosWorkFlowService (PY→CORE integration)`

- [ ] **T2.6** - Controller: ProyectosController
  - Archivo: `Controllers/PY/ProyectosController.cs`
  - Ref: `MATRIZ_PERMISOS_ROLES.md` § 4.1 ([Authorize(Roles="GerenteProyectos")])
  - Acciones: Index, Create, Edit, Reasignar
  - **Validación:** VerificarPermisoUsuario(38) + EsOwner() en Edit/Reasignar
  - **Commit:** `feat: add ProyectosController`

- [ ] **T2.7** - Controller: TrabajosController (CRÍTICO)
  - Archivo: `Controllers/PY/TrabajosController.cs`
  - Ref: `MATRIZ_PERMISOS_ROLES.md` § 4.2
  - Acciones: Index, Create, Edit, Duplicar, CambiarEstado
  - **En Create():**
    - Llamar: TrabajosWorkFlowService.CrearHiloCrearTareas()
    - Validar: VerificarPermisoUsuario(97)
  - **En Duplicar():**
    - Llamar: SP Py_TrabajoDuplicar (⚠️ validar transactionalidad)
    - Ref: `VALIDACION_BASE_DATOS.md` § 3.2 (SP transactional)
  - **Commit:** `feat: add TrabajosController with WorkFlow integration`

- [ ] **T2.8** - Views: Proyectos
  - Archivos: `Views/PY/Proyectos/Index.cshtml`, `Create.cshtml`, `Edit.cshtml`
  - Reusar: `_Grid.cshtml`, `_Upload.cshtml` (para Brief/Especificaciones)
  - **Commit:** `feat: add PY.Proyectos views`

- [ ] **T2.9** - Views: Trabajos
  - Archivos: `Views/PY/Trabajos/Index.cshtml`, `Create.cshtml`, `Edit.cshtml`
  - **Campos:** Nombre, IdMetodologia, Estado, JobBook, + 10 filtros (ref: T2.4)
  - Reusar: `_Grid.cshtml`, `_Upload.cshtml` (para documentos)
  - **Commit:** `feat: add PY.Trabajos views with filters`

- [ ] **T2.10** - Integración Email (notificaciones al crear trabajo)
  - En TrabajosController.Create(): Llamar EmailService.EnviarAsync()
  - Recipientes: Gerente proyecto, Coordinadores asignados
  - Ref: `VALIDACION_EVIDENCIAS_PY_CORE.md` § 2 (Guardar() llama EnviarCorreo())
  - **Commit:** `feat: add email notifications on trabajo creation`

- [ ] **T2.11** - Testing: Integration test (PY→CORE)
  - Crear proyecto → Crear trabajo → Validar tareas CORE creadas
  - Archivo: `Tests/PY/TrabajosWorkFlowServiceTests.cs`
  - **Commit:** `test: add integration tests for PY→CORE workflow`

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
| **1** | 2 sem | 1 | CORE catálogos (Tareas, Precedencias) | ✅ Bloquea Sprint 2 | 8 |
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
