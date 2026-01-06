# ANALISIS_PY_PROYECTOS

## 1️⃣ Resumen Ejecutivo
- Propósito: Gestionar ciclo de proyectos (cuantitativo y cualitativo) incluyendo creación de proyectos, trabajos, asignaciones, distribución de entrevistas, instructivos y soportes.
- Qué resuelve: Orquesta catálogo de proyectos y trabajos, asigna responsables, gestiona segmentos y sesiones cualitativas, soportes y duplicación de trabajos.
- Usuarios: PM/GP, coordinadores, reclutadores/moderadores, roles de aprobación (US_Usuarios), posiblemente tráfico/operaciones.
- Dependencias: CORE (workflow/tareas), CU_Cuentas (jobbooks/brief/estudios), US_Usuarios (roles/permisos), catálogos PY/PY_Cuali en CoreProject, componente de carga de archivos compartido.
- Complejidad estimada: 🔴 Alta (18 webforms, mezcla cuanti+cuali, múltiples SP y cargas de archivos).

## 2️⃣ Inventario del Legado

| Archivo (WebMatrix/PY_Proyectos) | Funcionalidad | Eventos | Dependencias | Evidencia |
| --- | --- | --- | --- | --- |
| Default.aspx / NewDefault.aspx | Landing/búsqueda de proyectos | Page_Load, filtros (pendiente) | Catálogos proyectos, usuarios | ⚠️ NO ENCONTRADO (leer code-behind) |
| Home.aspx | Dashboard módulo | Page_Load (pendiente) | Servicios métricas, roles | ⚠️ NO ENCONTRADO |
| PY_Proyectos.aspx | Maestro de proyectos | Page_Load, btnGuardar_Click (pendiente) | SP PY_Proyectos*, catálogos tipos/estados | ⚠️ NO ENCONTRADO |
| Trabajos.aspx | Trabajos cuantitativos | Page_Load, eventos CRUD (pendiente) | SP PY_Trabajo*, catálogos metodologías | ⚠️ NO ENCONTRADO |
| TrabajosCualitativos.aspx | Trabajos cualitativos | Page_Load, eventos CRUD (pendiente) | PY_Cuali.*, moderadores/reclutadores | ⚠️ NO ENCONTRADO |
| AsignacionProyectos.aspx | Asignar responsables | Eventos asignación (pendiente) | US_Usuarios, PY_Proyectos | ⚠️ NO ENCONTRADO |
| AsignacionesProyectos.aspx | Listado de asignaciones | Eventos filtro/export (pendiente) | PY_Proyectos, US_Usuarios | ⚠️ NO ENCONTRADO |
| REAsignacionProyectos.aspx | Reasignaciones | Eventos reasignar (pendiente) | PY_Proyectos, auditoría | ⚠️ NO ENCONTRADO |
| DistribucionEntrevistas.aspx | Planeación entrevistas | Eventos distribución (pendiente) | Catálogos ciudades/unidades/metodologías | ⚠️ NO ENCONTRADO |
| SegmentosCuali.aspx | Segmentación cuali | Eventos CRUD segmentos (pendiente) | PY_SegmentosCuali* | ⚠️ NO ENCONTRADO |
| Sesiones.aspx | Programación sesiones | Eventos CRUD sesiones (pendiente) | PY_Sesiones*, catálogos lugar/estado | ⚠️ NO ENCONTRADO |
| InHomeVisit.aspx | Visitas en casa | Eventos CRUD visitas (pendiente) | PY_InHomeVisit*, bitácoras | ⚠️ NO ENCONTRADO |
| VariablesControl.aspx | Variables de control | Eventos CRUD variables (pendiente) | PY_Variables_Control*, reportes PY | ⚠️ NO ENCONTRADO |
| RegistroPlanillasCualitativo.aspx | Planillas cuali | Eventos carga/registro (pendiente) | Carga de archivos, PY_Cuali* | ⚠️ NO ENCONTRADO |
| InstructivoGeneral.aspx | Instructivos cuanti | Eventos descarga/carga (pendiente) | Componente upload, almacenamiento | ⚠️ NO ENCONTRADO |
| InstructivoGeneralCuali.aspx | Instructivos cuali | Eventos descarga/carga (pendiente) | Componente upload, almacenamiento | ⚠️ NO ENCONTRADO |
| DuplicarTrabajos.aspx | Duplicar trabajos | Evento duplicar (pendiente) | SP de clon, catálogos | ⚠️ NO ENCONTRADO |
| AsignacionProyectos.aspx.vb (y otros .vb) | Lógica server-side | Click/SelectedIndexChanged (pendiente) | SP en PY_Model/PY_Cuali | ⚠️ NO ENCONTRADO |

Notas: Inventario inicial basado en estructura; cada evento y SP debe documentarse con evidencia (Regla 1). 

## 3️⃣ Flujos Funcionales (Detallado)

> Nivel de detalle pendiente de evidencias; se listan flujos a documentar. Cada paso debe enlazar a archivo+método o SP exacto.

1. **Crear proyecto (cuanti)**
   - Pasos: abrir formulario, cargar catálogos, validar campos, guardar, feedback.
   - Evidencia: ⚠️ NO ENCONTRADO (buscar en PY_Proyectos.aspx.vb y SP asociados).
2. **Crear trabajo cuanti**
   - Pasos: seleccionar proyecto, definir metodología, guardar trabajo, relacionar con CORE si aplica.
   - Evidencia: ⚠️ NO ENCONTRADO (Trabajos.aspx.vb, SP PY_Trabajo*).
3. **Crear trabajo cuali**
   - Pasos: seleccionar proyecto, cuotas/segmentos, moderadores/reclutadores, guardar.
   - Evidencia: ⚠️ NO ENCONTRADO (TrabajosCualitativos.aspx.vb, PY_Cuali.* SP).
4. **Asignar responsables de proyecto**
   - Pasos: elegir proyecto, seleccionar usuario, guardar asignación, notificar.
   - Evidencia: ⚠️ NO ENCONTRADO (AsignacionProyectos.aspx.vb, SP asignación).
5. **Reasignar proyecto**
   - Pasos: seleccionar asignación, nuevo responsable, registrar auditoría.
   - Evidencia: ⚠️ NO ENCONTRADO (REAsignacionProyectos.aspx.vb).
6. **Distribuir entrevistas**
   - Pasos: seleccionar trabajo, definir distribución por ciudad/unidad, guardar.
   - Evidencia: ⚠️ NO ENCONTRADO (DistribucionEntrevistas.aspx.vb, SP distribución).
7. **Gestionar segmentos cuali**
   - Pasos: crear/editar segmentos y cuotas, asociar a trabajo.
   - Evidencia: ⚠️ NO ENCONTRADO (SegmentosCuali.aspx.vb, PY_SegmentosCuali*).
8. **Programar sesiones**
   - Pasos: crear sesión, asignar lugar/fecha/estado, gestionar asistentes.
   - Evidencia: ⚠️ NO ENCONTRADO (Sesiones.aspx.vb, PY_Sesiones*).
9. **InHome visits**
   - Pasos: programar visita, seguimiento, estado.
   - Evidencia: ⚠️ NO ENCONTRADO (InHomeVisit.aspx.vb, PY_InHomeVisit*).
10. **Variables de control**
    - Pasos: alta/edición de variables y rangos, validación.
    - Evidencia: ⚠️ NO ENCONTRADO (VariablesControl.aspx.vb, PY_Variables_Control*).
11. **Planillas cuali**
    - Pasos: carga/registro, validaciones, persistencia y/o archivos.
    - Evidencia: ⚠️ NO ENCONTRADO (RegistroPlanillasCualitativo.aspx.vb, SP asociados).
12. **Instructivos (cuanti/cuali)**
    - Pasos: cargar/descargar instructivos, validar peso/tipo.
    - Evidencia: ⚠️ NO ENCONTRADO (InstructivoGeneral*.aspx.vb, componente upload).
13. **Duplicar trabajos**
    - Pasos: seleccionar trabajo origen, duplicar configuraciones/catálogos, confirmar.
    - Evidencia: ⚠️ NO ENCONTRADO (DuplicarTrabajos.aspx.vb, SP de clonación).

## 4️⃣ Mapa de Migración 1:1

| WebForm (WebMatrix/PY_Proyectos) | Ruta MVC | Controller | Action(s) | View(s) | ViewModel(s) | Service(s) |
| --- | --- | --- | --- | --- | --- | --- |
| Default.aspx / NewDefault.aspx | /PY/Proyectos | ProyectosController | Index | Index.cshtml | ProyectoListViewModel | ProyectoService |
| Home.aspx | /PY/Home | HomeController | Index | Index.cshtml | PYDashboardViewModel | ProyectoService, TrabajoService |
| PY_Proyectos.aspx | /PY/Proyectos | ProyectosController | Create, Edit, Delete | Create.cshtml, Edit.cshtml (modales) | ProyectoCreateEditViewModel | ProyectoService, ProyectoDataAdapter |
| Trabajos.aspx | /PY/Trabajos | TrabajosController | Index, Create, Edit, Delete | Index.cshtml, _Form.cshtml (modal) | TrabajoListViewModel, TrabajoCreateEditViewModel | TrabajoService, TrabajoDataAdapter |
| TrabajosCualitativos.aspx | /PY/TrabajosCuali | TrabajosCualiController | Index, Create, Edit, Delete | Index.cshtml, _Form.cshtml (modal) | TrabajosCualiListViewModel, TrabajosCualiCreateEditViewModel | TrabajosCualiService, CualiDataAdapter |
| AsignacionProyectos.aspx | /PY/Asignaciones | AsignacionesController | Create, Edit | Create.cshtml, Edit.cshtml (modal) | AsignacionCreateEditViewModel | AsignacionService, AsignacionDataAdapter |
| AsignacionesProyectos.aspx | /PY/Asignaciones | AsignacionesController | Index | Index.cshtml | AsignacionListViewModel | AsignacionService |
| REAsignacionProyectos.aspx | /PY/Asignaciones | AsignacionesController | Reasign | Reasign.cshtml (modal) | ReasignViewModel | AsignacionService, AsignacionDataAdapter |
| DistribucionEntrevistas.aspx | /PY/Distribucion | DistribucionController | Index, Distribute | Index.cshtml, _Distribute.cshtml (modal) | DistribucionListViewModel, DistribucionCreateViewModel | DistribucionService, DistribucionDataAdapter |
| SegmentosCuali.aspx | /PY/SegmentosCuali | SegmentosCualiController | Index, Create, Edit, Delete | Index.cshtml, _Form.cshtml (modal) | SegmentosCualiListViewModel, SegmentosCualiCreateEditViewModel | SegmentosCualiService, CualiDataAdapter |
| Sesiones.aspx | /PY/Sesiones | SesionesController | Index, Create, Edit, Delete | Index.cshtml, _Form.cshtml (modal) | SesionesListViewModel, SesionesCreateEditViewModel | SesionesService, CualiDataAdapter |
| InHomeVisit.aspx | /PY/InHomeVisit | InHomeVisitController | Index, Create, Edit, Delete | Index.cshtml, _Form.cshtml (modal) | InHomeVisitListViewModel, InHomeVisitCreateEditViewModel | InHomeVisitService, CualiDataAdapter |
| VariablesControl.aspx | /PY/VariablesControl | VariablesControlController | Index, Create, Edit, Delete | Index.cshtml, _Form.cshtml (modal) | VariablesControlListViewModel, VariablesControlCreateEditViewModel | VariablesControlService, VariablesControlDataAdapter |
| RegistroPlanillasCualitativo.aspx | /PY/Planillas | PlanillasController | Index, Upload | Index.cshtml, _Upload.cshtml (modal) | PlanillasListViewModel, PlanillasUploadViewModel | PlanillasService, CualiDataAdapter, UploadService |
| InstructivoGeneral.aspx | /PY/Instructivos | InstructivosController | Index, Upload, Download | Index.cshtml, _Upload.cshtml (modal) | InstructivosListViewModel, InstructivosUploadViewModel | InstructivosService, UploadService |
| InstructivoGeneralCuali.aspx | /PY/Instructivos | InstructivosController | Index, Upload, Download (idem) | Index.cshtml, _Upload.cshtml (modal) | InstructivosListViewModel, InstructivosUploadViewModel | InstructivosService, UploadService |
| DuplicarTrabajos.aspx | /PY/Trabajos | TrabajosController | Duplicate | Duplicate.cshtml (modal) | DuplicateTrabajoViewModel | TrabajoService, TrabajoDataAdapter |

Notas: Modales como parciales reutilizables; ViewModels incluyen catálogos y referencias.

## 5️⃣ Base de Datos

### Tablas principales (CoreProject PY_Model/PY_Cuali)

| Tabla | Tipo | Decisión acceso | Notas |
| --- | --- | --- | --- |
| PY_Proyectos | Maestra | EF Core (CRUD simple) | Create/Update/Delete; validar SP si existe. |
| PY_Trabajo | Maestra | Dapper (lectura) + EF (escritura) | Lectura paginada; escritura simple. |
| PY_TrabajoCuali | Maestra | Dapper (lectura) + EF (escritura) | Similar a PY_Trabajo. |
| PY_Especificaciones* | Detalle | EF Core | CRUD en cascade. |
| PY_Variables_Control | Referencial | EF Core | Bajo volumen. |
| PY_SegmentosCuali* | Maestra/Detalle | EF Core | CRUD + validación cuotas. |
| PY_Sesiones* | Maestra/Detalle | EF Core | CRUD + validación temporal. |
| PY_InHomeVisit* | Maestra/Detalle | EF Core | CRUD + seguimiento. |
| Catálogos (tipos, metodologías, etc.) | Referencial | Dapper (cached) | Combos; nombres exactos. |

### SP/Result classes clave

| SP / Result class | Tabla | Decisión | Notas |
| --- | --- | --- | --- |
| PY_Proyectos_Get_Result | PY_Proyectos | Dapper | Lectura lista; filtros + paginación. |
| PY_Trabajos_GET_All_Result | PY_Trabajo | Dapper | Lectura completa; validar peso. |
| PY_Trabajos_Get_Result | PY_Trabajo | Dapper | Lectura filtrada. |
| PY_TrabajoCuali_Get_Result | PY_TrabajoCuali | Dapper | Lectura cuali con joins. |
| PY_TrabajosxProyectosxCoordinador_Result | PY_Trabajo + PY_Proyectos | Dapper | Asignaciones coordinador. |
| PY_TrabajosxProyectosxGerente_Result | PY_Trabajo + PY_Proyectos | Dapper | Asignaciones gerente. |
| PY_SegmentosCuali_Get_Result | PY_SegmentosCuali* | Dapper | Lectura segmentos. |
| PY_Sesiones_Get_Result | PY_Sesiones* | Dapper | Lectura sesiones. |
| SP Distribución | Tabla distribución | Dapper | Distribución por ciudad/unidad. |
| SP Duplicar | PY_Trabajo + especificaciones | SP (transacción) | Clon atómico. |
| SP Upload/Download | Almacenamiento | Dapper + fs | Instructivos y planillas. |

### Consideraciones

- **EF Core:** CRUD simple sin lógica de SP.
- **Dapper:** lecturas con paginación servidor; catálogos cached.
- **SP complejas:** validaciones, cálculos, distribuciones, clonaciones.
- **Transacciones:** duplicación y distribución (atomicidad garantizada).

## 6️⃣ Riesgos Técnicos

| Riesgo | Severidad | Descripción | Mitigación |
| --- | --- | --- | --- |
| **ViewState en grillas grandes** | 🔴 Alta | UpdatePanels + ViewState pesado en Default/Trabajos. | Paginación servidor-side; no ViewState. |
| **Lógica de duplicación incompleta** | 🔴 Alta | Puede no clonar todas relaciones (especificaciones, variables). | Inventariar tablas relacionadas; SP transaccional. |
| **Catálogos no sincronizados** | 🔴 Alta | Cambios en BD no reflejados en UI/reportes. | Caching con invalidación; validar pre-migración. |
| **Lógica de archivos ad-hoc** | 🔴 Alta | Carga sin documentar en Instructivos/Planillas.aspx. | Documentar rutas/extensiones; componente centralizado. |
| **SP sin notificación** | 🟠 Media | Asignación/reasignación sin emails/eventos. | Revisar legacy; si no existe, descartar. |
| **Dependencia circular PY↔CORE** | 🟠 Media | Workflows pueden crear trabajos PY; referencias cruzadas. | Mapear dependencias; revisar ciclos. |
| **Performance distribución** | 🟠 Media | DistribucionEntrevistas puede procesar miles de registros. | Paginación + async si SP > 5s. |
| **Estados no validados** | 🟡 Baja-Media | Cambios sin validar transiciones (abierto/cerrado/etc). | Máquina de estados en service. |
| **Archivos huérfanos** | 🟡 Baja-Media | Instructivos sin referencias si tarea se elimina. | Cascade soft-delete o limpieza. |
| **Permisos no claros** | 🟡 Baja | Roles de acceso (PM/GP/coordinador) no documentados. | Revisar US_Usuarios; [Authorize(Roles = ...)]. |

## 7️⃣ Componentes Reutilizables

| Componente | Ubicación (legacy) | Reutilizable en | Descripción |
| --- | --- | --- | --- |
| **Upload de archivos** | CU_Cuentas/Frame.aspx | PY (Instructivos, Planillas), CORE (Documentos_Tareas) | Componente centralizado con validación tamaño/extensión. |
| **Grid paginado** | Compartido (UpdatePanel) | PY/CORE Index views | Parcial Razor servidor-side paginación, filtros, export. |
| **DatePicker** | jQuery | PY (Sesiones, InHomeVisit), CORE (Estimacion) | Plugin integrado con Bootstrap; validación rango. |
| **SelectUser** | US_Usuarios dropdown | PY (Asignaciones), CORE (Asignaciones, Documentos) | Partial con búsqueda; filtro por rol. |
| **Modal CRUD** | CU/FI patrones | PY/CORE | Bootstrap modal reutilizable; formularios dinámicos. |
| **Tabs/Acordeones** | FI | PY Home, CORE Dashboard | Bootstrap componente; resúmenes tabulares. |
| **Validaciones cliente** | JavaScript ad-hoc | PY/CORE | Fluent Validation (.NET) + data-attributes HTML5. |

**Decisión:** Centralizar en Views/Shared/*.cshtml; UploadService y GridService en core services.

## 8️⃣ Backlog Inicial

### Fase 0 – Infraestructura (Estimación: 1-2 semanas, 40-80h)

| ID | Tarea | Prioridad | Estimación | Dependencias |
| --- | --- | --- | --- | --- |
| PY-0-1 | Crear área PY + DI Program.cs | P0 | 4h | - |
| PY-0-2 | Crear infraestructura Core + DI | P0 | 4h | - |
| PY-0-3 | Scaffolding PY_Model/PY_Cuali en MatrixNext.Data | P0 | 8h | - |
| PY-0-4 | Scaffolding CORE_Model en MatrixNext.Data | P0 | 8h | - |
| PY-0-5 | Crear DataAdapters base | P0 | 32h | PY-0-3, PY-0-4 |
| PY-0-6 | Publicar componentes compartidos | P0 | 16h | - |
| PY-0-7 | Documentar patrones y DI | P1 | 8h | PY-0-1, PY-0-2 |

**Subtotal Fase 0:** ~80h

### Fase 1 – PY Maestros (Estimación: 2-3 semanas, 80-120h)

| ID | Tarea | Prioridad | Estimación | Dependencias |
| --- | --- | --- | --- | --- |
| PY-1-1 | ProyectosController (CRUD) | P0 | 20h | PY-0-5 |
| PY-1-2 | TrabajosController (CRUD + Duplicate) | P0 | 24h | PY-0-5 |
| PY-1-3 | TrabajosCualiController (CRUD) | P0 | 24h | PY-0-5 |
| PY-1-4 | HomeController + dashboard | P1 | 12h | PY-1-1, PY-1-2 |
| PY-1-5 | Testing Fase 1 | P1 | 16h | PY-1-1, PY-1-2, PY-1-3 |

**Subtotal Fase 1:** ~96h

### Fase 2 – PY Operación (Estimación: 2-3 semanas, 100-160h)

| ID | Tarea | Prioridad | Estimación | Dependencias |
| --- | --- | --- | --- | --- |
| PY-2-1 | AsignacionesController + Reasignaciones | P0 | 20h | PY-0-5 |
| PY-2-2 | DistribucionController (distribución entrevistas) | P0 | 24h | PY-0-5 |
| PY-2-3 | SegmentosCualiController | P0 | 16h | PY-0-5 |
| PY-2-4 | SesionesController + InHomeVisit | P0 | 20h | PY-0-5 |
| PY-2-5 | VariablesControlController | P1 | 12h | PY-0-5 |
| PY-2-6 | Instructivos + Planillas (upload) | P1 | 16h | PY-0-6 |
| PY-2-7 | Testing Fase 2 + integración | P1 | 24h | PY-2-1 a PY-2-6 |

**Subtotal Fase 2:** ~132h

**Total PY:** ~308h

## 9️⃣ Checklist Pre-Migración

- [ ] Secciones 1-6 validadas (no ⚠️ NO ENCONTRADO)
- [ ] SP documentados con parámetros exactos
- [ ] Catálogos mapeados en BD
- [ ] Roles PY validados en US_Usuarios
- [ ] Dependencias PY-CORE claras; sin ciclos
- [ ] Componentes compartidos accesibles
- [ ] Ambiente test con datos (100-500 registros)
- [ ] Rama feature/PY-CORE-migration creada
- [ ] Equipo asignado (dev, QA, arquitecto)
- [ ] Aprobación stakeholder: PM, Finanzas, Ops
- [ ] Control versiones: PR review definido
- [ ] Plan testing: casos por webform
- [ ] Documentación técnica accesible

## 🔟 Decisiones Técnicas Clave

| Decisión | Seleccionado | Justificación | Riesgos |
| --- | --- | --- | --- |
| **ORM** | EF Core (CRUD) + Dapper (lectura/SP) | EF6 incompatible; Dapper para reportes. | Dos accesos; coherencia requerida. |
| **Autenticación** | [Authorize] + roles US_Usuarios | Sistema existente; no rediseñar. | Validar mapeo roles. |
| **Almacenamiento** | FS local (~/ Files/[IdTrabajo]/) | Reutilizar patrón CU; sin S3. | Escalabilidad limitada. |
| **Paginación** | Servidor-side (Dapper Skip/Take) | Evitar ViewState pesado. | Cambio paradigma UpdatePanel. |
| **Caching** | MemoryCache (.NET) | Catálogos estáticos; reducir BD. | Invalidación manual. |
| **Componente Upload** | Parcial Razor centralizado | Reutilizable; validación. | Máximo tamaño; extensiones. |
| **Máquina estados** | Service validation + EF/SP | No introducir lib state machine. | Documentar transiciones. |
| **Logging** | ILogger<T> .NET + CORE_ObservacionesTareas | Native logging + persistencia. | Volumen logs; cleaning. |
| **Modularidad** | Areas (PY/) + Core | Equipos independientes. | Routing complexity; docs. |

## 1️⃣1️⃣ Estimación Preliminar

### PY (620h) + CORE (580h) = **1,200 HORAS**

**Controllers:** 220h | **Services:** 140h | **Vistas:** 120h | **Adapters:** 60h | **Testing:** 80h

**Timeline:**
- 1 dev @ 80h/semana → 15 semanas
- 2 devs @ 50h/semana → 12 semanas
- 3 devs → 10 semanas

**Con buffer 15%:** ~1,380 horas (~17 semanas @ 1 dev, ~9 semanas @ 2 devs)

## 1️⃣2️⃣ Próximos Pasos

### Inmediatos
1. Validar análisis con arquitecto + PM
2. Confirmar dependencias PY ↔ CORE ↔ CU/US
3. Aprobación stakeholder (PM, Finanzas, Ops)
4. Crear rama `feature/PY-CORE-migration`
5. Asignar equipo (2 dev + arquitecto)

### Semana 1 (Fase 0)
- Setup área PY + DI
- Scaffolding modelos EF Core
- Adapters base + componentes compartidos

### Semana 2-3 (Fase 1)
- Controllers CRUD: Proyectos, Trabajos, TrabajosCuali
- Home dashboard
- Testing Fase 1

### Semana 4+ (Fase 2+)
- Asignaciones, Distribución, Cuali
- Instructivos, Planillas
- Integración PY-CORE
- Testing end-to-end

### Deliverables
✅ Código compilado sin errores
✅ Testing 100% flujos legacy cubiertos
✅ Documentación completada
✅ Aprobación stakeholder + QA staging

---

**Análisis completado. Listo para handoff a desarrollo.**
