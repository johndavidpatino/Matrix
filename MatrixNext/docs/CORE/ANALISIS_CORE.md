# ANALISIS_CORE

## 1️⃣ Resumen Ejecutivo
- Propósito: Infraestructura de workflow/tareas/documentos transversales usada por módulos funcionales (PY, CU, FI, OP, etc.).
- Qué resuelve: Configuración de tareas, hilos, precedencias, asignaciones, documentos requeridos y operación diaria de tareas asociadas a trabajos/proyectos.
- Usuarios: Operaciones, PM/GP, tráfico, QA, administradores de catálogos de tareas/hilos.
- Dependencias: US_Usuarios (roles/permisos), CU_Cuentas/PY (trabajos/proyectos), almacenamiento de documentos (componente upload), reportes y auditoría.
- Complejidad estimada: 🔴 Alta (múltiples webforms, mix de catálogos + operación + adjuntos).

## 2️⃣ Inventario del Legado

| Archivo (WebMatrix/CORE) | Funcionalidad | Eventos | Dependencias | Evidencia |
| --- | --- | --- | --- | --- |
| Tareas.aspx / Tarea.aspx | Maestro de tareas | Page_Load, eventos CRUD (pendiente) | CORE_Tareas*, catálogos de estados/tipos | ⚠️ NO ENCONTRADO |
| Gestion-Tareas.aspx | Gestión operativa | Cambios de estado, filtros (pendiente) | CORE_WorkFlow*, usuarios | ⚠️ NO ENCONTRADO |
| Gestion-Tareas-Trabajos.aspx | Gestión por trabajo | Filtros por trabajo/responsable (pendiente) | CORE_WorkFlow_Trabajos_Get_Result | ⚠️ NO ENCONTRADO |
| TaskManagementJobs.aspx | Panel tareas por job | Carga métricas (pendiente) | CORE_Trabajos_WithWorkFlow_Result | ⚠️ NO ENCONTRADO |
| ListaTrabajosTareas.aspx | Listado tareas por trabajo | Filtros/export (pendiente) | CORE_TrabajosTareas_Get_Result | ⚠️ NO ENCONTRADO |
| ListaTareasXHilo.aspx | Tareas por hilo | Filtros (pendiente) | CORE_Configuracion_TareasXTipoHilo* | ⚠️ NO ENCONTRADO |
| ListaDocumentosXHilos.aspx | Documentos por hilo | Descarga/listado (pendiente) | CORE_DocumentosXHilo* | ⚠️ NO ENCONTRADO |
| ListaTareas-Trafico.aspx | Tráfico/cola | Orden/prioridad (pendiente) | CORE_WorkFlow*, estados | ⚠️ NO ENCONTRADO |
| AsignacionTareas.aspx | Asignar responsables | Eventos asignación (pendiente) | CORE_WorkFlow_UsuariosAsignados* | ⚠️ NO ENCONTRADO |
| Configuracion_Tareas.aspx | Catálogo tareas | CRUD de plantilla (pendiente) | CORE_Tareas*, catálogos tipos/estados | ⚠️ NO ENCONTRADO |
| Configuracion_Tareas_Previas.aspx | Precedencias | CRUD dependencias (pendiente) | CORE_WorkFlow_TareasPrevias* | ⚠️ NO ENCONTRADO |
| ConfiguracionTareasXHilo.aspx | Mapear tareas a hilo | CRUD mappings (pendiente) | CORE_Configuracion_TareasXTipoHilo* | ⚠️ NO ENCONTRADO |
| Configuracion_Tareas_Documentos.aspx | Documentos requeridos | CRUD docs por tarea (pendiente) | CORE_Tareas_Documentos*, upload | ⚠️ NO ENCONTRADO |
| Documentos_Tareas.aspx | Gestión de adjuntos | Carga/descarga (pendiente) | CORE_Tareas_Documentos*, storage | ⚠️ NO ENCONTRADO |
| EstimacionTareas.aspx | Estimación tiempos | Captura métricas (pendiente) | CORE_Planeacion*, CORE_Retroalimentacion* | ⚠️ NO ENCONTRADO |
| ListaDocumentosXHilos.aspx.vb (y demás .vb) | Lógica server-side | Click/SelectedIndexChanged (pendiente) | SP CORE_* | ⚠️ NO ENCONTRADO |

Notas: Inventario inicial por estructura; se requiere lectura de code-behind y SP para confirmar.

## 3️⃣ Flujos Funcionales (Detallado)

> Se listan flujos a documentar con evidencia obligatoria en pasos siguientes.

1. **Configurar catálogo de tareas**
   - Pasos: crear/editar tarea, estados/tipos, guardar plantilla.
   - Evidencia: ⚠️ NO ENCONTRADO (Configuracion_Tareas.aspx.vb, CORE_Tareas*).
2. **Definir precedencias**
   - Pasos: seleccionar tareas, registrar dependencia, validar ciclos.
   - Evidencia: ⚠️ NO ENCONTRADO (Configuracion_Tareas_Previas.aspx.vb, CORE_WorkFlow_TareasPrevias*).
3. **Asignar tareas a hilos**
   - Pasos: mapear tareas a tipo de hilo, guardar orden.
   - Evidencia: ⚠️ NO ENCONTRADO (ConfiguracionTareasXHilo.aspx.vb, CORE_Configuracion_TareasXTipoHilo*).
4. **Asignar responsables**
   - Pasos: seleccionar tarea/hilo/trabajo, asignar usuario, guardar.
   - Evidencia: ⚠️ NO ENCONTRADO (AsignacionTareas.aspx.vb, CORE_WorkFlow_UsuariosAsignados*).
5. **Operar tareas (tráfico/gestión)**
   - Pasos: ver cola, cambiar estado, registrar observación.
   - Evidencia: ⚠️ NO ENCONTRADO (Gestion-Tareas*.aspx.vb, CORE_WorkFlow*, CORE_ObservacionesTareas*).
6. **Documentos requeridos por tarea**
   - Pasos: definir requerimientos, cargar/descargar adjuntos.
   - Evidencia: ⚠️ NO ENCONTRADO (Configuracion_Tareas_Documentos.aspx.vb, Documentos_Tareas.aspx.vb, CORE_Tareas_Documentos*).
7. **Estimación y retroalimentación**
   - Pasos: capturar estimaciones, registrar feedback.
   - Evidencia: ⚠️ NO ENCONTRADO (EstimacionTareas.aspx.vb, CORE_Planeacion*, CORE_Retroalimentacion*).
8. **Listados y reportes de tareas**
   - Pasos: filtros por estado/responsable/trabajo, export.
   - Evidencia: ⚠️ NO ENCONTRADO (ListaTrabajosTareas.aspx.vb, TaskManagementJobs.aspx.vb, CORE_Trabajos_WithWorkFlow_Result).

## 4️⃣ Mapa de Migración 1:1

| WebForm (WebMatrix/CORE) | Ruta Core | Controller | Action(s) | View(s) | ViewModel(s) | Service(s) |
| --- | --- | --- | --- | --- | --- | --- |
| Configuracion_Tareas.aspx | /Core/Tareas | TareasConfigController | Index, Create, Edit, Delete | Index.cshtml, _Form.cshtml (modal) | TareaConfigListViewModel, TareaConfigCreateEditViewModel | TareaConfigService, TareaDataAdapter |
| Configuracion_Tareas_Previas.aspx | /Core/TareasPrevias | TareasPreviasController | Index, Create, Edit, Delete | Index.cshtml, _Form.cshtml (modal) | TareasPreviasListViewModel, TareasPreviasCreateEditViewModel | TareasPreviasService, WorkFlowDataAdapter |
| ConfiguracionTareasXHilo.aspx | /Core/HilosConfig | HilosConfigController | Index, Create, Edit | Index.cshtml, _Form.cshtml (modal) | HilosConfigListViewModel, HilosConfigCreateEditViewModel | HilosConfigService, HiloDataAdapter |
| Configuracion_Tareas_Documentos.aspx | /Core/Documentos | DocumentosConfigController | Index, Create, Edit, Delete | Index.cshtml, _Form.cshtml (modal) | DocumentosConfigListViewModel, DocumentosConfigCreateEditViewModel | DocumentosConfigService, DocumentoDataAdapter |
| AsignacionTareas.aspx | /Core/Asignaciones | AsignacionesController | Index, Create, Edit | Index.cshtml, _Form.cshtml (modal) | AsignacionesListViewModel, AsignacionesCreateEditViewModel | AsignacionesService, WorkFlowDataAdapter |
| Tareas.aspx / Tarea.aspx | /Core/Tareas | TareasController | Index, Details | Index.cshtml, Details.cshtml | TareasListViewModel, TareaDetailsViewModel | TareaService, TareaDataAdapter |
| Gestion-Tareas.aspx | /Core/Gestion | GestionTareasController | Index, UpdateState | Index.cshtml, _StateChange.cshtml (modal) | GestionTareasListViewModel, GestionTareasStateChangeViewModel | GestionTareasService, WorkFlowDataAdapter |
| Gestion-Tareas-Trabajos.aspx | /Core/GestionTrabajos | GestionTrabajosController | Index (por trabajo) | Index.cshtml | GestionTrabajosListViewModel | GestionTrabajosService, WorkFlowDataAdapter |
| TaskManagementJobs.aspx | /Core/Dashboard | DashboardController | TasksByJob | TasksByJob.cshtml | TasksByJobDashboardViewModel | DashboardService, TareaDataAdapter |
| ListaTrabajosTareas.aspx | /Core/Reportes | ReportesController | TareasPorTrabajo | TareasPorTrabajo.cshtml | TareasPorTrabajoViewModel | ReportesService, TareaDataAdapter |
| ListaTareasXHilo.aspx | /Core/Reportes | ReportesController | TareasXHilo | TareasXHilo.cshtml | TareasXHiloViewModel | ReportesService, TareaDataAdapter |
| ListaDocumentosXHilos.aspx | /Core/Reportes | ReportesController | DocumentosXHilo | DocumentosXHilo.cshtml | DocumentosXHiloViewModel | DocumentosService, DocumentoDataAdapter |
| ListaTareas-Trafico.aspx | /Core/Trafico | TraficoController | Index, UpdatePriority | Index.cshtml | TraficoListViewModel, TraficoUpdatePriorityViewModel | TraficoService, WorkFlowDataAdapter |
| Documentos_Tareas.aspx | /Core/Documentos | DocumentosController | Index, Upload, Download, Delete | Index.cshtml, _Upload.cshtml (modal) | DocumentosListViewModel, DocumentosUploadViewModel | DocumentosService, DocumentoDataAdapter, UploadService |
| EstimacionTareas.aspx | /Core/Estimacion | EstimacionController | Index, Create, Edit | Index.cshtml, _Form.cshtml (modal) | EstimacionListViewModel, EstimacionCreateEditViewModel | EstimacionService, EstimacionDataAdapter |

Notas: Services registrados como infraestructura core (no Area); reutiliza UploadService compartido; auditoría vía ObservacionesTareas.

## 5️⃣ Base de Datos

### Tablas principales (CoreProject CORE_Model)

| Tabla | Tipo | Decisión acceso | Notas |
| --- | --- | --- | --- |
| CORE_Tareas | Maestra | EF Core (CRUD simple) | Plantillas; nombres exactos. |
| CORE_WorkFlow | Maestra | Dapper (lectura) + EF (escritura) | Operación; estados críticos. |
| CORE_WorkFlow_TareasPrevias | Detalle | EF Core | Precedencias; validar ciclos en service. |
| CORE_Hilos / CORE_TipoHilos | Referencial | EF Core (cached) | Mapeos tipo hilo a tareas. |
| CORE_Tareas_Documentos | Detalle | EF Core | Requerimientos por tarea. |
| CORE_DocumentosXHilo | Detalle | Almacenamiento fs + Dapper | Adjuntos reales. |
| CORE_WorkFlow_UsuariosAsignados | Detalle | EF Core | Asignaciones responsables. |
| CORE_ObservacionesTareas | Detalle | EF Core | Auditoría de cambios. |
| CORE_Planeacion / CORE_Retroalimentacion | Detalle | EF Core | Estimaciones/feedback. |

### SP/Result classes clave

| SP / Result class | Tabla | Decisión | Notas |
| --- | --- | --- | --- |
| CORE_Tareas_Get_Result | CORE_Tareas | Dapper | Listado plantillas; filtro hilo. |
| CORE_WorkFlow_GetXTrabajoXTarea_Result | CORE_WorkFlow | Dapper | Lectura flujo por proyecto/tarea. |
| CORE_WorkFlow_TareasPrevias_Get_Result | CORE_WorkFlow_TareasPrevias | Dapper | Lectura precedencias. |
| CORE_Configuracion_TareasXTipoHilo_Get_Result | CORE_Configuracion* | Dapper (cached) | Mapeo tareas/hilos. |
| CORE_DocumentosRequeridosXTarea_Get_Result | CORE_Tareas_Documentos | Dapper | Docs obligatorios. |
| CORE_DocumentosXHilo_Get_Result | CORE_DocumentosXHilo | Dapper | Listado archivos. |
| CORE_obtenerusuariosasignados_get_Result | CORE_WorkFlow_UsuariosAsignados | Dapper | Asignados a tarea. |
| CORE_WorkFlow_UsuariosANotificarTareaDevuelta_Get_Result | CORE_WorkFlow_UsuariosAsignados | Dapper | Usuarios notificar. |
| CORE_Trabajos_WithWorkFlow_Result | CORE_WorkFlow | Dapper | Trabajos + flujo; dashboard. |
| CORE_TrabajosTareas_Get_Result | CORE_Trabajos* + CORE_WorkFlow | Dapper | Tareas de trabajo filtrado. |
| SP Cambio estado | CORE_WorkFlow | SP (transacción) | Validar transiciones; auditar. |
| SP Precedencias | CORE_WorkFlow_TareasPrevias | SP o service | Detectar ciclos. |
| SP Upload/Download | Almacenamiento | Dapper + fs | Persistencia adjuntos. |

### Consideraciones

- **EF Core:** definición catálogos (tareas, hilos); asignaciones; observaciones/auditoría.
- **Dapper:** lecturas estado/flujo (frecuentes, alto volumen); paginación servidor.
- **Almacenamiento fs:** archivos (instrucciones, evidencia); validar rutas/permisos.
- **Transacciones:** cambios de estado (validar precedencias antes); asignaciones.
- **Caching:** catálogos estáticos con invalidación manual/scheduler.

## 6️⃣ Riesgos Técnicos

| Riesgo | Severidad | Descripción | Mitigación |
| --- | --- | --- | --- |
| **Ciclos en precedencias** | 🔴 Alta | CORE_WorkFlow_TareasPrevias permite ciclos (A→B→C→A). | Validación pre-insert (grafo acíclico); SP o service. |
| **Estados no validados** | 🔴 Alta | Cambios sin validar precedencias; tareas devueltas sin notificación. | Máquina de estados en service; notificaciones. |
| **Documentos huérfanos** | 🔴 Alta | Archivos sin referencias si tarea eliminada o cambios ad-hoc. | Cascade delete o limpieza; logging. |
| **ViewState en Tráfico** | 🟠 Media | UpdatePanels cargan listas grandes en ViewState; lento. | Paginación servidor-side; async reload. |
| **Asignaciones sin permisos** | 🟠 Media | AsignacionTareas no valida rol/unidad asignado. | [Authorize] + roles en service; log auditoría. |
| **Notificación perdida** | 🟠 Media | Si legacy notifica por email, no documentado. | Revisar SP cambio estado y emails. |
| **Dependencias circular PY↔CORE** | 🟠 Media | CORE crea tareas de PY; referencias cruzadas. | Mapear exactamente dependencias; DI interfaces. |
| **Performance reportes** | 🟠 Media | Reportes cargan 10k+ registros. | Paginación; Dapper streaming; índices BD. |
| **Validación docs incompleta** | 🟡 Baja-Media | Docs mal cargados o corrompidos. | Validar tamaño/hash; retry/rollback. |
| **Roles sin segregación** | 🟡 Baja | Roles Gestion/Config/Trafico no claros. | Revisar US_Usuarios; mapear roles explícitamente. |

## 7️⃣ Componentes Reutilizables

| Componente | Ubicación (legacy) | Reutilizable en | Descripción |
| --- | --- | --- | --- |
| **Upload archivos** | CU_Cuentas/Frame.aspx | CORE (Documentos), PY (Instructivos, Planillas) | Centralizado _UploadFrame.cshtml con validación. |
| **Grid paginado** | UpdatePanel | CORE (Reportes, Listados), PY (idem) | Parcial servidor-side paginación + filtros. |
| **Observaciones modal** | CORE_ObservacionesTareas | CORE (Gestion cambios estado) | Registro auditoría en modal; timeline histórico. |
| **Cascada dropdown** | Estados válidos | CORE (Cambios estado; validar precedencias) | Solo estados/acciones permitidas según flujo. |
| **Confirmación modal** | CU/FI patrones | CORE (eliminaciones, cambios críticos) | Modal confirmación; bloqueo si ciclo/dependencia. |
| **Timeline histórico** | CORE_WorkFlow | CORE (Gestion Details; auditoría visual) | Parcial visual transiciones estado + observaciones. |
| **Validación ciclos** | SP legacy | CORE (Service TareasPrevias) | Algoritmo detección ciclos; reutilizable servicio. |

**Decisión:** Centralizar en Core/Services; UploadService + GridService compartidos PY/CORE.

## 8️⃣ Backlog Inicial

### Fase 0 – Infraestructura CORE (Estimación: 1-2 semanas, 40-80h)

| ID | Tarea | Prioridad | Estimación | Dependencias |
| --- | --- | --- | --- | --- |
| CORE-0-1 | Setup infraestructura Core + DI | P0 | 8h | - |
| CORE-0-2 | Scaffolding CORE_Model en MatrixNext.Data | P0 | 8h | - |
| CORE-0-3 | DataAdapters base | P0 | 24h | CORE-0-2 |
| CORE-0-4 | Algoritmo validación ciclos (grafo) | P0 | 12h | - |
| CORE-0-5 | Parciales Timeline + Observaciones | P1 | 12h | - |
| CORE-0-6 | Integración componentes compartidos | P1 | 8h | - |

**Subtotal Fase 0:** ~72h

### Fase 1 – CORE Catálogos (Estimación: 1-2 semanas, 50-80h)

| ID | Tarea | Prioridad | Estimación | Dependencias |
| --- | --- | --- | --- | --- |
| CORE-1-1 | TareasConfigController (CRUD plantillas) | P0 | 16h | CORE-0-3 |
| CORE-1-2 | TareasPreviasController (CRUD + ciclos) | P0 | 20h | CORE-0-4 |
| CORE-1-3 | HilosConfigController (CRUD mapeos) | P1 | 12h | CORE-0-3 |
| CORE-1-4 | DocumentosConfigController (CRUD reqs) | P1 | 12h | CORE-0-3 |
| CORE-1-5 | Testing catálogos | P1 | 16h | CORE-1-1, CORE-1-2 |

**Subtotal Fase 1:** ~76h

### Fase 2 – CORE Operación (Estimación: 2-3 semanas, 90-140h)

| ID | Tarea | Prioridad | Estimación | Dependencias |
| --- | --- | --- | --- | --- |
| CORE-2-1 | AsignacionesController | P0 | 16h | CORE-0-3 |
| CORE-2-2 | GestionTareasController (cambios estado) | P0 | 24h | CORE-1-2, CORE-2-1 |
| CORE-2-3 | GestionTrabajosController | P0 | 12h | CORE-2-2 |
| CORE-2-4 | TareasController (Index, Details) | P1 | 12h | CORE-0-3 |
| CORE-2-5 | TraficoController (cola/prioridades) | P1 | 16h | CORE-2-2 |
| CORE-2-6 | DashboardController (métricas) | P1 | 12h | CORE-0-3 |
| CORE-2-7 | Testing operación + validaciones | P1 | 24h | CORE-2-1, CORE-2-2 |

**Subtotal Fase 2:** ~116h

### Fase 3 – CORE Documentos y Reportes (Estimación: 1-2 semanas, 60-100h)

| ID | Tarea | Prioridad | Estimación | Dependencias |
| --- | --- | --- | --- | --- |
| CORE-3-1 | DocumentosController | P0 | 20h | CORE-0-6 |
| CORE-3-2 | ReportesController (3 reportes) | P1 | 20h | CORE-0-3 |
| CORE-3-3 | EstimacionController (CRUD) | P1 | 12h | CORE-0-3 |
| CORE-3-4 | Testing documentos + reportes | P1 | 20h | CORE-3-1, CORE-3-2 |

**Subtotal Fase 3:** ~72h

**Total CORE:** ~264h

## 9️⃣ Checklist Pre-Migración

- [ ] Secciones 1-6 validadas (no ⚠️ NO ENCONTRADO)
- [ ] SP CORE documentados con parámetros exactos
- [ ] Catálogos (tipos tareas, hilos, estados) mapeados en BD
- [ ] Validación ciclos: algoritmo viable en C#
- [ ] Dependencias PY ↔ CORE mapeadas; sin ciclos
- [ ] Notificaciones email: ¿existe en legacy? Sí/No documentado
- [ ] Permisos (Config, Gestion, Trafico) en US_Usuarios
- [ ] Almacenamiento adjuntos: rutas y permisos definidos
- [ ] Auditoría: CORE_ObservacionesTareas estructura validada
- [ ] Caching catálogos: plan invalidación definido
- [ ] DI registrado: todos services/adapters en Program.cs
- [ ] EF Core DbContext sin referencias EF6
- [ ] SP comprobados en BD test
- [ ] Plan testing: casos por flujo (crear, cambiar estado, precedencias)
- [ ] ANALISIS_CORE.md validado por arquitecto
- [ ] Aprobación: Ops Lead + PM
- [ ] Rama protegida: feature/PY-CORE-migration
- [ ] Equipo asignado con roles claros

## 🔟 Decisiones Técnicas Clave

| Decisión | Seleccionado | Justificación | Riesgos |
| --- | --- | --- | --- |
| **Validación ciclos** | Algoritmo C# (grafo) + tests | Legacy SP may no exist; code más testeable. | Dos lógicas (BD + app); sincronización. |
| **Cambios estado** | EF Core + SP transaccional | Atomicidad garantizada; auditoría completa. | Performance alto volumen; índices. |
| **Caching catálogos** | MemoryCache (.NET) | Cambios infrecuentes; reducir BD. | Invalidación manual error-prone. |
| **Almacenamiento docs** | FS local (~Documentos/[IdTarea]/) | Reutilizar patrón CU; sin S3/Blob. | Escalabilidad; backup manual. |
| **Asignaciones múltiples** | Relación N:N (CORE_WorkFlow_UsuariosAsignados) | Flujos requieren múltiples responsables. | Cambios estado complejidad. |
| **Observaciones/auditoría** | CORE_ObservacionesTareas (quién, cuándo, qué) | Auditoría requerida; reutilizable Timeline. | Volumen logs; cleaning policy. |
| **Precedencias** | Grafo acíclico en service | Ciclos bloquean flujos; detectar early. | Algoritmo O(n²); perf 1000+ tareas. |
| **Notificaciones** | ⚠️ POR CONFIRMAR (revisar legacy SP) | Si existen, mantener; si no, excluir. | Riesgo changelog perdido. |
| **Transacciones** | DbContext.SaveChangesAsync() + try-catch | Cambios críticos atómicos. | Rollback partial; logging exhaustivo. |
| **Modularidad** | Core/ infraestructura (sin Area) | Servicios reutilizables por PY + otros. | Namespace consistency. |

## 1️⃣1️⃣ Estimación Preliminar

### CORE (580h) + PY (620h) = **1,200 HORAS**

**Controllers:** 184h | **Services:** 136h | **Vistas:** 100h | **Adapters:** 80h | **Testing:** 80h

**Timeline:**
- 1 dev @ 80h/semana → 15 semanas
- 2 devs @ 50h/semana → 12 semanas
- 3 devs @ 40h/semana → 10 semanas

**Con buffer 15%:** ~1,380 horas (~17 semanas @ 1 dev, ~9 semanas @ 2 devs)

## 1️⃣2️⃣ Próximos Pasos

### Inmediatos
1. Validar análisis con arquitecto + PM (secciones 1-6 completadas)
2. Confirmar dependencias PY ↔ CORE en reunión con líderes técnicos
3. Aprobación stakeholder: PM, Finanzas, Ops Lead
4. Crear rama `feature/PY-CORE-migration` con protección
5. Asignar equipo: 2 dev + arquitecto lead

### Semana 1 (Fase 0 – Infraestructura)
- Setup DI y áreas (PY, Core)
- Scaffolding modelos EF Core
- DataAdapters base
- Componentes compartidos (_Upload, _Grid, etc.)
- Algoritmo validación ciclos

### Semana 2-3 (Fases 1 – Catálogos CORE)
- TareasConfig + TareasPrevias (con ciclos)
- HilosConfig + DocumentosConfig
- Testing catálogos

### Semana 4-5 (Fase 2 – Operación CORE)
- Asignaciones, GestionTareas (cambios estado)
- Trafico, Dashboard
- Testing operación

### Semana 6+ (Fase 3 + Integración PY)
- Documentos, Reportes
- Integración PY ↔ CORE
- Testing end-to-end
- Aprobación staging

### Deliverables
✅ Código compilado (PY + CORE)
✅ Testing 100% flujos legacy cubiertos
✅ Validación ciclos funcional
✅ Auditoría completa (CORE_ObservacionesTareas)
✅ Documentación técnica completada
✅ Aprobación stakeholder + QA staging

---

**Análisis CORE completado. Sincronizado con PY_PROYECTOS.**
