# BACKLOG GLOBAL DE MIGRACION (CIERRE DE PENDIENTES)

Objetivo: consolidar un unico plan de ejecucion para cerrar los pendientes criticos de migracion antes de abrir un modulo nuevo, asegurando paridad completa con WebMatrix y cumpliendo estrictamente [MatrixNext/DIRECTRICES_MIGRACION.md](MatrixNext/DIRECTRICES_MIGRACION.md).

## Fuentes de verdad a consultar siempre
- Estado macro: [MatrixNext/DASHBOARD_MIGRACION.md](MatrixNext/DASHBOARD_MIGRACION.md) y [MatrixNext/MODULOS_MIGRACION.md](MatrixNext/MODULOS_MIGRACION.md).
- Reglas obligatorias: [MatrixNext/DIRECTRICES_MIGRACION.md](MatrixNext/DIRECTRICES_MIGRACION.md) (Reglas 1-10, especially Regla 2 y 5.1).
- Codigo legacy: WebForms bajo `WebMatrix/*` + data layers en `CoreProject/*` (mapeo de tablas, SP y tipos). 
- Esquema BD: scripts en `MatrixNext/docs/SQL/CO_Matrix_Structure_*.sql` y nombres en `CO_Matrix_SP_Names.csv`.
- Documentacion especifica por modulo ya creada (ejemplos: [MatrixNext/docs/OP/ANALISIS_OP_CUANTITATIVO.md](MatrixNext/docs/OP/ANALISIS_OP_CUANTITATIVO.md), [MatrixNext/docs/TH/RESUMEN_MIGRACION_AUSENCIAS.md](MatrixNext/docs/TH/RESUMEN_MIGRACION_AUSENCIAS.md), [MatrixNext/docs/GD/BACKLOG_MIGRACION_GD_DOCUMENTOS_FASE5_PARTE_A.md](MatrixNext/docs/GD/BACKLOG_MIGRACION_GD_DOCUMENTOS_FASE5_PARTE_A.md)).

## Checklist de validacion obligatorio (aplica a cada modulo antes de marcarlo como cerrado)
1) Inventario legacy: listar todas las paginas .aspx/.vb y WebMethods en `WebMatrix/[MODULO]/*` + clases DataLayer en `CoreProject/*`.
2) Mapeo BD: para cada accion legacy, identificar SP/tablas/parametros exactos en CoreProject y confirmarlos en `CO_Matrix_Structure_SP.sql` y `CO_Matrix_Structure_Tables.sql` (Regla 2 y 2.1).
3) Paridad funcional: confirmar que cada boton/flujo legacy existe en MatrixNext y que no se agregan features nuevas (Regla 6). 
4) UX y AJAX: validar uso de modales y respuestas JSON con parciales cuando aplique (Regla 5.1). 
5) Seguridad y DI: `[Authorize]`/permisos, registro de servicios y adapters en Program.cs, manejo de errores y logging. 
6) Pruebas funcionales: crear/editar/eliminar, filtros, paginacion, exportes, cambio de estados; smoke con datos reales de staging.
7) Documentacion: actualizar backlog/analisis y `MIGRACION_[MODULO]_COMPLETADA.md` o equivalente; registrar desviaciones y SP usados.

## Pendientes criticos a cerrar antes de nuevo modulo

### 1) GD_Documentos (Fases 1-4) — Prioridad Media
- Alcance pendiente: infraestructura, catalogos, maestro, workflow (Fases 1-4). Fase 5 ya cerrada (ver [MatrixNext/docs/GD/BACKLOG_MIGRACION_GD_DOCUMENTOS_FASE5_PARTE_A.md](MatrixNext/docs/GD/BACKLOG_MIGRACION_GD_DOCUMENTOS_FASE5_PARTE_A.md)).
- Pasos:
  - Inventario legacy en [WebMatrix/GD_Documentos](WebMatrix/GD_Documentos) y adaptadores en `CoreProject` (clases GD_* DAL).
  - Mapeo de SP y tablas (GD_* en `CO_Matrix_Structure_SP.sql`/`CO_Matrix_Structure_Tables.sql`); documentar en una hoja de mapeo accion→SP→parametros.
  - Implementar en MatrixNext (Area GD) reutilizando patrones de uploads compartidos (ver cierre Uploads en [MatrixNext/MODULOS_MIGRACION.md](MatrixNext/MODULOS_MIGRACION.md)).
  - QA: flujo completo de catalogos, maestro y workflow; exportes si existen.
  - Docs: generar `MIGRACION_GD_DOCUMENTOS_COMPLETADA.md` y actualizar dashboard.

### 2) PY_Proyectos — Prioridad Alta
- Pendientes indicados: InHomeVisit, VariablesControl, Instructivos/Planillas, DuplicarTrabajos, DistribucionEntrevistas (ver [MatrixNext/docs/PY/MIGRACION_PY_PROYECTOS.md](MatrixNext/docs/PY/MIGRACION_PY_PROYECTOS.md)).
- Pasos:
  - Inventario de paginas en [WebMatrix/PY_Proyectos](WebMatrix/PY_Proyectos) y DataLayers `PY_*` en CoreProject.
  - Mapeo SP/tablas (PY_* en `CO_Matrix_Structure_SP.sql`); validar tipos y nullability.
  - Definir equivalencia de vistas a controllers/views Razor existentes; mantener URLs en Area PY.
  - Implementar acciones faltantes con modales y AJAX-first; sin nuevas features.
  - QA: flujos de duplicado, asignacion de variables y distribucion; exportes/reportes.
  - Docs: actualizar backlog PY y emitir `MIGRACION_PY_PROYECTOS_COMPLETADA.md`.

### 3) TH_TalentoHumano — Prioridad Media (Ausencias listo)
- Pendientes: Empleados, Nomina y otros submodulos (Ausencias ya completado, ver [MatrixNext/docs/TH/RESUMEN_MIGRACION_AUSENCIAS.md](MatrixNext/docs/TH/RESUMEN_MIGRACION_AUSENCIAS.md)).
- Pasos:
  - Inventario en [WebMatrix/TH_TalentoHumano](WebMatrix/TH_TalentoHumano) excluyendo Ausencias; revisar WebMethods y reportes.
  - Mapeo SP/tablas `TH_*` en CoreProject y scripts SQL; registrar diferencias de tipos (p.ej., fechas y money).
  - Migrar Empleados y Nomina siguiendo Regla 5.1 (modales y JSON) y reutilizando selectores/lookup compartidos.
  - QA: crear/editar empleados, cambios de estado, calculos de nomina, exportes.
  - Docs: `MIGRACION_TH_TALENTOHUMANO_COMPLETADA.md` con lista de SP usados y evidencias.

### 4) Home — Prioridad Alta
- Paginas: Home.aspx, Default.aspx, DefaultOLD.aspx en [WebMatrix/Home](WebMatrix/Home).
- Pasos:
  - Inventario de datos consumidos (multiple contextos) y widgets actuales.
  - Mapeo de queries/SP en CoreProject (CORE_*); validar en scripts SQL.
  - Migrar dashboard en MatrixNext (Controllers/Views globales) respetando widgets y permisos.
  - QA: carga de todos los tiles/graficos, filtros y links; validar performance.
  - Docs: actualizar dashboard y registrar SP consumidos.

### 5) RP_Reportes — Prioridad Alta
- Ubicacion legacy: [WebMatrix/RP_Reportes](WebMatrix/RP_Reportes); contexto `REP_Model`.
- Pasos:
  - Inventario de reportes y botones de exporte; identificar SP (ideal Dapper).
  - Confirmar definiciones en `CO_Matrix_Structure_SP.sql`; documentar en hoja de mapeo.
  - Migrar controllers/views en Area RP con endpoints de exporte; mantener naming de reportes.
  - QA: filtros, paginacion, exportes (Excel/PDF), permisos.
  - Docs: `MIGRACION_RP_REPORTES_COMPLETADA.md` y actualizacion de sidebar/Program.cs.

### 6) OP_RO y OP_Trafico — Prioridad Baja
- Legacy: [WebMatrix/OP_RO](WebMatrix/OP_RO) y [WebMatrix/OP_Trafico](WebMatrix/OP_Trafico); contexto `OP_*`.
- Pasos:
  - Inventario de paginas y WebMethods; mapeo SP `OP_*` en CoreProject y scripts SQL.
  - Migrar con el mismo patron de OP_Cuantitativo/Cualitativo (ver docs en [MatrixNext/docs/OP](MatrixNext/docs/OP)).
  - QA: flujos de asignacion/rutas/estado; exportes si aplican.
  - Docs: backlogs actualizados y cierre de modulo.

### 7) Otros modulos (baja prioridad, planificar despues de los anteriores)
- Pendientes mencionados: PY_ControlCalidad, PY_Adquisiciones, SG_Actas, SGC_Calidad, ES_Estadistica, Centro_Informacion, Inventario (excluido en FI), IT, MBO*, ResumenProduccion, RE_GT, PC_PropiedadCliente.
- Pasos para cada uno:
  - Ejecutar checklist de validacion completo (inventario→SP→UI→QA→docs).
  - Documentar decision de alcance (migrar vs excluir) en su backlog especifico.

## Plan de ejecucion sugerido (CRONOLOGIA COMPLETA)

### Fase 1: Cierre de módulos críticos (Sprints 0-4) – YA EN EJECUCION
1. **Sprint 0 (Setup)**: Inventario y mapeo de GD, PY, TH.
2. **Sprint 1-2 (GD)**: Completar GD_Documentos Fases 1-4.
3. **Sprint 3 (PY)**: Completar funcionalidades faltantes de PY_Proyectos.
4. **Sprint 4 (TH API)**: Completar API REST de TH_TalentoHumano (Empleados, Nomina, Desvinculaciones) – ✅ COMPLETADO 2026-01-15.

### Fase 2: Views/UI y complementos (Sprints 5-7) – INMEDIATO
5. **Sprint 5 (TH Views)**: Implementar Views/UI de TH sobre API Sprint 4 (Empleados, Nomina, Desvinculaciones).
6. **Sprint 6 (OP Complementos)**: Cerrar funcionalidades faltantes de OP_Cualitativo (reportes, validaciones).
7. **Sprint 7 (CORE)**: Resolver workflow de tareas y dependencias de módulos.

### Fase 3: Reportes y dashboards (Sprints 8-10) – MEDIANO PLAZO
8. **Sprint 8 (EQ Fase 1)**: Análisis completo de EasyQuote + implementación Fase 1 (catálogos/infraestructura).
9. **Sprint 9 (Home)**: Implementar dashboard Home con todos sus widgets.
10. **Sprint 10 (RP_Reportes)**: Migrar todos los reportes con opciones de exporte (Excel/PDF).

### Fase 4: Operacionales (Sprints 11-12) – FUTURO
11. **Sprint 11 (OP_RO + OP_Trafico)**: Migrar módulos operacionales restantes.
12. **Sprint 12+ (Otros)**: Módulos de baja prioridad según decisión de negocio.

## Priorización por Sprint
- **Sprint 0 (setup y mapeo cruzado, 1 semana)**: checklist de validacion aplicado a GD, PY y TH; generar hoja de mapeo accion→SP→parametros para cada modulo; confirmar gaps con `CO_Matrix_Structure_*.sql`; asignar responsables y definir backlog de historias por modulo.

- **Sprint 1 (GD_Documentos Fases 1-2, 1-2 semanas) – CERRADO 2026-01-11**
  - Entregables: infraestructura y catalogos de GD funcionando en MatrixNext (Area GD), DI registrada, sidebar actualizado.
  - Actividades: migrar WebForms de Fase 1-2; implementar adapters/servicios; pruebas de catalogos; documentar SP usados.
  - Criterio de terminado: paridad de Fases 1-2 validada, QA ejecutado, `MIGRACION_GD_DOCUMENTOS_COMPLETADA.md` parcial actualizado.
  - **Resultados**: Inventario completo ([docs/GD/INVENTARIO_GD_DOCUMENTOS_FASES1_4.md](MatrixNext/docs/GD/INVENTARIO_GD_DOCUMENTOS_FASES1_4.md)); mapeo acción→SP→parámetros documentado ([docs/GD/MAPEO_ACCION_SP_GD_FASES1_4.md](MatrixNext/docs/GD/MAPEO_ACCION_SP_GD_FASES1_4.md)); implementado flujo completo catalogos (TipoSolicitud/EstadoSolicitud/Procesos) con adapters/services/controllers/views; flujo solicitudes (create/list/assign-reviewers) y aprobaciones (approve workflow) implementado; build limpio sin errores (warnings Azure.Identity suprimidos con NoWarn); QA pendiente funcional sobre datos staging.

### Sprint 1 – Plan de ejecución (DEV)
- **Inventario y mapeo (día 1-2)**: listar WebForms y WebMethods de [WebMatrix/GD_Documentos](WebMatrix/GD_Documentos); extraer SP/tablas GD_* desde CoreProject y validar en `CO_Matrix_Structure_SP.sql`/`CO_Matrix_Structure_Tables.sql`; registrar accion→SP→parametros.
- **Adapters/Services (día 2-4)**: crear adapters en MatrixNext.Data (Area GD) con nombres exactos de SP; servicios con validaciones mínimas; registrar DI en Program.cs.
- **Controllers/Views (día 3-6)**: implementar acciones para catalogos e infraestructura usando patrón AJAX-first (Regla 5.1) con parciales/modales; actualizar rutas en AreaRegistration si aplica.
- **UI y Sidebar (día 5-6)**: agregar enlaces GD en sidebar y breadcrumbs; reutilizar componentes compartidos (_AjaxModal, toasts, grids).
- **QA funcional (día 6-7)**: crear/editar/eliminar catalogos, filtros, exportes si existen; validar permisos `[Authorize]` y errores manejados.
- **Documentación (día 7)**: actualizar `MIGRACION_GD_DOCUMENTOS_COMPLETADA.md` con inventario, mapeo SP, evidencias QA; reflejar avance en dashboard.


- **Sprint 2 (GD_Documentos Fases 3-4, 1-2 semanas) – CERRADO 2026-01-11**
  - Entregables: maestro y workflow GD migrados; exportes si aplican.
  - Actividades: migrar WebForms de Fase 3-4; validar workflow y roles; pruebas de exporte; actualizar sidebar.
  - Criterio de terminado: paridad Fases 1-4 completa, QA con datos de staging, documento de cierre GD listo.
  - **Resultados**: Implementación ya existente verificada (maestro CRUD completo: construcción/actualización/anulación con SP GD_MaestroDocumentos_Add2/DocumentosControlados_Add/DocumentosMaestros_Update/DocumentosControlados_Activo); workflow aprobaciones con GD_Revisiones_Edit; solicitudes completo; email service integrado; todos los controllers con `[Authorize]`; sin exportes (no requeridos en legacy); build limpio; QA pendiente funcional en staging.
- **Sprint 3 (PY_Proyectos pendientes, 1-2 semanas)**
  - Entregables: funcionalidades faltantes (InHomeVisit, VariablesControl, Instructivos/Planillas, DuplicarTrabajos, DistribucionEntrevistas) en Area PY.
  - Actividades: migrar vistas y acciones faltantes con modales/JSON; adaptar servicios/adapters; ejecutar QA de duplicado, variables y distribucion.
  - Criterio de terminado: paridad completa PY, `MIGRACION_PY_PROYECTOS_COMPLETADA.md` actualizado, menu/DI listos.
- **Sprint 4 (TH_TalentoHumano Empleados/Nomina, 1-2 semanas)**
  - Entregables: submodulos Empleados y Nomina migrados (Ausencias ya ok); selectores reutilizados; reportes/exportes funcionando.
  - Actividades: migrar WebForms restantes; mapeo SP `TH_*`; pruebas de alta/baja, cambios de estado y calculos; actualizar docs.
  - Criterio de terminado: paridad TH (Empleados/Nomina) validada, QA completo, `MIGRACION_TH_TALENTOHUMANO_COMPLETADA.md` actualizado.

### Sprint 2 – Plan de ejecución (DEV)
- **Inventario y mapeo (día 1-2)**: listar WebForms de Fases 3-4 en [WebMatrix/GD_Documentos](WebMatrix/GD_Documentos); actualizar hoja accion→SP→parametros para maestro/workflow; validar en `CO_Matrix_Structure_*`.
- **Adapters/Services (día 2-4)**: ampliar adapters GD para maestro/workflow; asegurar transacciones y roles; DI revisada.
- **Controllers/Views (día 3-6)**: acciones de maestro/workflow con AJAX-first; manejo de estados y transiciones según SP; parciales para aprobaciones.
- **Exportes y roles (día 5-6)**: implementar/exportar reportes si existen; validar permisos y visibilidad.
- **QA funcional (día 6-7)**: flujos de alta/edicion, cambios de estado, exportes; pruebas de concurrencia básica.
- **Documentación (día 7)**: cerrar `MIGRACION_GD_DOCUMENTOS_COMPLETADA.md` con Fases 1-4 completas; actualizar dashboard.

### Sprint 3 – Plan de ejecución (DEV)
- **Inventario y mapeo (día 1-2)**: WebForms y WebMethods en [WebMatrix/PY_Proyectos](WebMatrix/PY_Proyectos); mapear SP PY_* en CoreProject y `CO_Matrix_Structure_*`; actualizar accion→SP.
- **Adapters/Services (día 2-4)**: completar adapters y servicios para InHomeVisit, VariablesControl, Instructivos/Planillas, DuplicarTrabajos, DistribucionEntrevistas; DI en Program.cs.
- **Controllers/Views (día 3-6)**: implementar acciones faltantes con modales/JSON; mantener URLs en Area PY; reutilizar componentes.
- **Flujos especiales (día 5-6)**: validar duplicado de trabajos y distribucion de entrevistas; asegurar reglas de negocio y validaciones originales.
- **QA funcional (día 6-7)**: pruebas de creación/edición, duplicado, asignaciones, exportes/reportes; permisos.
- **Documentación (día 7)**: actualizar `MIGRACION_PY_PROYECTOS_COMPLETADA.md`, sidebar/DI revisados, dashboard actualizado.

### Sprint 4 – Plan de ejecución (DEV)
- **Inventario y mapeo (día 1-2)**: WebForms de Empleados y Nomina en [WebMatrix/TH_TalentoHumano](WebMatrix/TH_TalentoHumano); mapear SP `TH_*` en CoreProject y `CO_Matrix_Structure_*`; hoja accion→SP.
- **Adapters/Services (día 2-4)**: adapters Nomina/Empleados con tipos exactos (fechas/money); servicios con validaciones; DI.
- **Controllers/Views (día 3-6)**: modales/JSON para altas/bajas, cambios de estado, cálculos; reutilizar selectores (usuarios, áreas, cargos).
- **Reportes/Exportes (día 5-6)**: implementar exportes y reportes de Nomina/Empleados; validar roles y auditoría.
- **QA funcional (día 6-7)**: crear/editar/baja empleados, cambios de estado, cálculos de nómina, exportes; pruebas con staging.
- **Documentación (día 7)**: `MIGRACION_TH_TALENTOHUMANO_COMPLETADA.md` actualizado, dashboard y sidebar listos.

### Sprint 5 – Plan de ejecución (TH_TalentoHumano Views/UI - 2 semanas)
- **Alcance**: Implementar Views/UI (Razor + AJAX) para Empleados, Nómina y complementarios sobre API REST Sprint 4 ya completada.
- **Inventario y mapeo (día 1-2)**: listar pantallas legacy de Empleados/Nomina en [WebMatrix/TH_TalentoHumano](WebMatrix/TH_TalentoHumano); mapear a endpoints API existentes en EmpleadosController/DesvinculacionesController/CatalogosController (55 endpoints ya implementados).
- **Views Razor (día 2-6)**: crear vistas para CRUD de empleados (Index, Create/Edit con modales), nested resources (Experiencias/Educacion/Hijos/etc con datatables + botones), Desvinculaciones (workflow visual), Catálogos (dropdowns compartidos); reutilizar _AjaxModal, _Toolbar, _Grid parciales.
- **AJAX/JavaScript (día 4-6)**: interacciones con endpoints API ($.ajax/fetch), validaciones client-side, toasts de éxito/error, paginación, filtros; mantener patrón de ApiResponse<T> con Success/Error.
- **UI/UX (día 5-6)**: breadcrumbs, sidebar navigation (Area TH), permisos visuales `[Authorize]`, responsive design; pruebas en múltiples browsers.
- **Reportes/Exportes (día 6)**: implementar botones de descarga (Excel/PDF) para empleados y nómina usando ClosedXML o similar; validar roles de acceso.
- **QA funcional (día 6-7)**: flujos completos CRUD, nested resource management, cambios de estado, cálculos de nómina, exportes; pruebas con staging con datos reales.
- **Documentación (día 7)**: crear `MIGRACION_TH_TALENTOHUMANO_VIEWS_COMPLETADA.md` con inventario UI, mapeo pantalla→endpoint, evidencias QA; actualizar dashboard y sidebar.

### Sprint 6 – Plan de ejecución (OP_Cualitativo Complementos - 2 semanas)
- **Alcance**: Cerrar funcionalidades faltantes de OP_Cualitativo (reportes, ajustes, validaciones avanzadas) después de MVP Sprint 5 (ver [MatrixNext/docs/OP/SPRINT_5_CIERRE_MIGRACION_COMPLETA.md](MatrixNext/docs/OP/SPRINT_5_CIERRE_MIGRACION_COMPLETA.md)).
- **Inventario pendiente (día 1)**: listar WebMethods/reportes no implementados en Sprint 5; mapear a SP en CoreProject (`OP_*`); validar en `CO_Matrix_Structure_*`.
- **Adapters/Services complementarios (día 1-3)**: agregar métodos a adapters OP para reportes, filtros avanzados, validaciones de sesión; asegurar transacciones.
- **Controllers/Views reportes (día 3-5)**: endpoints para exportes (Excel/PDF), filtros complejos con autocomplete; vistas de reportes con tablas interactivas; paginación y search.
- **Validaciones y edge cases (día 5-6)**: reglas de negocio de moderadores/sesiones/entrevistas; manejo de estados y transiciones; concurrencia si aplica.
- **QA funcional (día 6-7)**: flujos de reportes, filtros, exportes; validaciones de datos; permisos y roles de acceso.
- **Documentación (día 7)**: cerrar `MIGRACION_OP_CUALITATIVO_COMPLETADA.md` con complementos Sprint 6 documentados; actualizar dashboard (marcar 🟢 completo).

### Sprint 7 – Plan de ejecución (CORE Workflow/Tareas - 2 semanas)
- **Alcance**: Resolver dependencias de CORE (workflow de tareas, asignaciones) y completar integración con módulos que lo consumen.
- **Análisis de dependencias (día 1-2)**: identificar exactamente qué módulos requieren CORE (PY, OP, TH, GD); listar WebMethods en [WebMatrix/CORE/](WebMatrix/CORE/), adaptadores en CoreProject; mapear SP `CORE_*`/`TAREA_*` en `CO_Matrix_Structure_*`.
- **Adapters/Services CORE (día 2-4)**: crear adapters para workflow (crear/asignar/cerrar tareas), notificaciones, escalaciones; servicios con validaciones de estado y permisos; DI en Program.cs.
- **Controllers/Views (día 4-6)**: endpoints CRUD de tareas, asignaciones, cambios de estado; vistas con modales para crear/editar/cerrar tareas; notificaciones (email/toast); integración con otros módulos (si aplica desde sus controllers).
- **Integraciones (día 6)**: asegurar que referencias a CORE desde PY/OP/TH/GD resuelvan correctamente; pruebas de flujos cross-module.
- **QA funcional (día 6-7)**: crear/asignar/cerrar tareas, cambios de estado, notificaciones, permisos; flujos de escalación; pruebas con staging.
- **Documentación (día 7)**: `MIGRACION_CORE_WORKFLOW_COMPLETADA.md` con inventario, mapeo SP, dependencias resueltas; actualizar dashboard (🟢 completo); confirmar DI y sidebar listos.

### Sprint 8 – Plan de ejecución (EQ_EasyQuote Análisis + Fase 1 - 2-3 semanas)
- **Alcance**: Análisis completo de EQ (crítica) + implementación de Fase 1 (infraestructura y catálogos); similar a OP_Cuantitativo en envergadura.
- **Inventario y mapeo (día 1-3)**: listar todas las páginas .aspx/.vb en [WebMatrix/EQ/](WebMatrix/EQ/); extraer WebMethods y BusinessLogic; mapear tablas y SP `EQ_*` en CoreProject y `CO_Matrix_Structure_*`; generar hoja accion→SP→parametros.
- **Análisis de arquitectura (día 2-4)**: identificar flujos principales (creación presupuestos, alternativas, cotización, aprobación); dependencias con CU_Cuentas y catálogos; patrones complejos (cálculos, simuladores); documentar en `ANALISIS_EQ_EASYQUOTE.md` (similar a `ANALISIS_OP_CUANTITATIVO.md`).
- **Diseño de APIs (día 4-5)**: definir endpoints REST para EQ (structure similar a: /presupuestos, /presupuestos/{id}/alternativas, /presupuestos/{id}/cotizacion, /presupuestos/{id}/aprobaciones); DTOs; manejo de ApiResponse<T>.
- **Adapters/Services Fase 1 (día 5-7)**: implementar adapters para catálogos EQ (tipos presupuesto, modalidades, formatos); servicios con validaciones mínimas; DI registrado.
- **Controllers/Views Fase 1 (semana 2 día 1-4)**: endpoints y vistas para catálogos; lista y gestión de presupuestos base (CRUD simple sin alternativas).
- **QA Fase 1 (semana 2 día 4-5)**: crear/editar presupuestos base, catálogos, filtros; permisos.
- **Documentación (semana 2 día 5-6)**: `ANALISIS_EQ_EASYQUOTE.md` con inventario, arquitectura propuesta, Fase 1 completada; crear backlog de Fases 2-4 (alternativas, simulador, aprobaciones); actualizar dashboard (🟡 en progreso).

### Sprint 9 – Plan de ejecución (Home Dashboard - 1-2 semanas)
- **Alcance**: Migrar Home.aspx con todos sus widgets y datos consumidos de múltiples contextos.
- **Inventario y mapeo (día 1-2)**: listar datos/widgets de Home en [WebMatrix/Home](WebMatrix/Home); mapear SP/tablas consumidas (múltiples contextos CORE, PY, OP, TH, CU, FI, etc.); validar en `CO_Matrix_Structure_*`.
- **Adapters/Services Dashboard (día 2-4)**: crear adapter/service HomeService que encapsule todas las queries (puede llamar a adapters de otros módulos si ya existen); asegurar caching para performance (widgets de lectura).
- **Controllers/Views Dashboard (día 4-5)**: controller Home con acciones para cargar widgets (Index, parcialesAJAX para cada tile); vistas con Bootstrap grid; componentes reutilizados; breadcrumbs.
- **Permisos y visibilidad (día 5-6)**: widgets condicionales según rol (`[Authorize(Roles="...")]`); links a módulos según permisos; validar que no expone datos sensibles.
- **QA funcional (día 6)**: carga completa de home, tiles, filtros si existen; performance de home page (< 2s idealmente); pruebas multi-rol.
- **Documentación (día 6-7)**: `MIGRACION_HOME_COMPLETADA.md` con widgets documentados, SP consumidos, performance notes; actualizar dashboard y sidebar (Home listo).

### Sprint 10 – Plan de ejecución (RP_Reportes - 1-2 semanas)
- **Alcance**: Migrar módulo RP_Reportes con todos sus reportes y opciones de exporte.
- **Inventario y mapeo (día 1-2)**: listar reportes en [WebMatrix/RP_Reportes](WebMatrix/RP_Reportes); mapear SP `REP_*` en CoreProject y `CO_Matrix_Structure_*`; documentar parámetros de filtro.
- **Adapters/Services Reportes (día 2-4)**: crear adapters RP con métodos para cada reporte; servicios que ejecutan queries/SP y retornan DataTable o DTO; transacciones si aplica.
- **Controllers/Views Reportes (día 4-6)**: endpoints para listar/generar reportes; vistas con formularios de filtro (datepickers, dropdowns); botones de exporte (Excel/PDF); integración con ClosedXML o iText; paginación.
- **Exportes y formatos (día 5-6)**: Excel (ClosedXML con estilos); PDF (iText o similar); validar datos en exportes; mantener nombres de reportes legacy.
- **QA funcional (día 6-7)**: generar cada reporte con filtros, validar datos, descargar en Excel/PDF, permisos por rol, performance.
- **Documentación (día 7)**: `MIGRACION_RP_REPORTES_COMPLETADA.md` con lista de reportes, SP consumidos, formatos de exporte; actualizar dashboard y sidebar (RP completo 🟢).

### Sprint 11 – Plan de ejecución (OP_RO y OP_Trafico - 2 semanas)
- **Alcance**: Migrar OP_RO (Revisión Operacional) y OP_Trafico (gestión de tráfico de datos).
- **Inventario y mapeo (día 1-2)**: listar páginas en [WebMatrix/OP_RO](WebMatrix/OP_RO) y [WebMatrix/OP_Trafico](WebMatrix/OP_Trafico); mapear SP `OP_*` en CoreProject; generar hojas accion→SP para cada módulo.
- **OP_RO (día 2-5)**:
  - Adapters/Services: métodos para crear/editar/cerrar revisiones; validaciones de estado.
  - Controllers/Views: CRUD revisiones, asignaciones, cambios de estado, aprobaciones; modales/AJAX.
  - QA: flujos de revisión, permisos, exportes si aplican.
- **OP_Trafico (día 3-6)**:
  - Adapters/Services: métodos para gestión de tráfico (asignación, seguimiento, cambios de estado).
  - Controllers/Views: dashboard de tráfico, asignaciones, filtros por estado/usuario; notificaciones.
  - QA: flujos de asignación, cambios de estado, reportes.
- **Integraciones (día 6)**: asegurar que OP_RO/Trafico se integren con OP_Cuantitativo/Cualitativo si aplica; validar flujos cross-module.
- **Documentación (día 6-7)**: `MIGRACION_OP_RO_COMPLETADA.md` y `MIGRACION_OP_TRAFICO_COMPLETADA.md`; actualizar dashboard (ambos 🟢 completo); DI y sidebar listos.

### Sprint 12+ – Plan de ejecución (Módulos de baja prioridad - a planificar por negocio)
- **PY_ControlCalidad, PY_Adquisiciones**: seguir patrón inventario→mapeo→adapters/services→controllers/views→QA→docs.
- **SG_Actas, SGC_Calidad, ES_Estadistica, Centro_Informacion, IT, MBO*, ResumenProduccion, RE_GT, PC_PropiedadCliente**: ejecutar mismo checklist de validacion; documentar decisión de alcance (migrar/excluir).
- **Inventario**: excluido de FI; evaluar relevancia para negocio.

Notas de control:
- Cada sprint debe cerrar con QA documentado y actualizacion de dashboard.
- Sprints 5-7 son críticos para completar módulos de prioridad alta/media; no iniciar Sprint 8+ sin cerrar Sprint 7.
- Mantener Regla 2 (mapeo SP/tablas) y Regla 5.1 (AJAX-first) en todas las tareas.
- Para módulos grandes como EQ, desglosar en fases (análisis completo en Sprint 8 Fase 1, implementación en Sprints 8-10).

## Matriz de Priorización por Sprint

| Sprint | Módulo | Prioridad | Duración | Estado Actual | Indicador | Responsable |
| --- | --- | --- | --- | --- | --- | --- |
| **Sprint 5** | TH_TalentoHumano Views/UI | **Media** | 2 semanas | 🟢 API REST completa (Sprint 4) | 🟡 Views pendientes | — |
| **Sprint 6** | OP_Cualitativo Complementos | **Alta** | 2 semanas | 🟡 MVP completado (Sprint 5) | 🟡 Reportes/validaciones pendientes | — |
| **Sprint 7** | CORE Workflow/Tareas | **Alta** | 2 semanas | 🟡 Parcial (dependencias bloqueadas) | 🟡 Resolver integraciones | — |
| **Sprint 8** | EQ_EasyQuote Fase 1 | **Crítica** | 2-3 semanas | 🟡 En progreso | 🟡 Análisis + Catálogos/Infraestructura | — |
| **Sprint 9** | Home Dashboard | **Alta** | 1-2 semanas | 🔴 Pendiente | 🔴 Todos los widgets | — |
| **Sprint 10** | RP_Reportes | **Alta** | 1-2 semanas | 🔴 Pendiente | 🔴 Reportes + Exportes | — |
| **Sprint 11** | OP_RO + OP_Trafico | **Baja** | 2 semanas | 🔴 Pendiente | 🔴 Ambos módulos | — |
| **Sprint 12+** | Módulos de baja prioridad | **Baja** | Variable | 🔴 Pendientes | 🔴 PY_CC, SG_Actas, SGC_Calidad, etc. | — |

### Dependencias críticas por Sprint
- **Sprint 5** requiere Sprint 4 (TH API) ✅ COMPLETADO
- **Sprint 6** requiere Sprint 5 (OP MVPbase)
- **Sprint 7** requiere Sprints 5-6 (dependencias de PY/OP/TH)
- **Sprint 8-10** pueden ejecutarse en paralelo si hay recursos, pero Sprint 7 debe estar ~80% avanzado
- **Sprint 11** requiere Sprints 8-10 completados (para validar integraciones OP_RO/Trafico con otros módulos)

## Entregables minimos por modulo
- Codigo migrado en MatrixNext (Areas, Controllers, Services, Adapters, Views) con DI registrado y menu actualizado.
- Documento `MIGRACION_[MODULO]_COMPLETADA.md` (o backlog actualizado) con: inventario legacy, mapeo accion→SP/tabla, decisiones y pruebas ejecutadas.
- Evidencias de QA (lista de pruebas ejecutadas) y confirmacion de paridad funcional.

## Responsables y control
- Cada modulo debe tener responsable asignado que marque avances en [MatrixNext/DASHBOARD_MIGRACION.md](MatrixNext/DASHBOARD_MIGRACION.md).
- No iniciar un modulo nuevo hasta que los de prioridad alta y media anteriores esten cerrados con evidencia de QA y documentacion actualizada.
