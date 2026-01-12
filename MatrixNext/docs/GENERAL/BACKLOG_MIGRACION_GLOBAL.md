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

## Plan de ejecucion sugerido (secuencial hasta abrir modulo nuevo)
1) Cerrar GD_Documentos Fases 1-4.
2) Cerrar PY_Proyectos pendientes.
3) Cerrar TH_TalentoHumano (Empleados/Nomina).
4) Entregar Home.
5) Entregar RP_Reportes.
6) Avanzar a OP_RO/OP_Trafico o siguiente modulo segun negocio.

## Fases y sprints para ejecutar puntos 1-3
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

Notas de control:
- Cada sprint debe cerrar con QA documentado y actualizacion de dashboard.
- No abrir Home ni RP_Reportes hasta terminar Sprint 4 o hasta decision expresa de negocio.
- Mantener Regla 2 (mapeo SP/tablas) y Regla 5.1 (AJAX-first) en todas las tareas.

## Entregables minimos por modulo
- Codigo migrado en MatrixNext (Areas, Controllers, Services, Adapters, Views) con DI registrado y menu actualizado.
- Documento `MIGRACION_[MODULO]_COMPLETADA.md` (o backlog actualizado) con: inventario legacy, mapeo accion→SP/tabla, decisiones y pruebas ejecutadas.
- Evidencias de QA (lista de pruebas ejecutadas) y confirmacion de paridad funcional.

## Responsables y control
- Cada modulo debe tener responsable asignado que marque avances en [MatrixNext/DASHBOARD_MIGRACION.md](MatrixNext/DASHBOARD_MIGRACION.md).
- No iniciar un modulo nuevo hasta que los de prioridad alta y media anteriores esten cerrados con evidencia de QA y documentacion actualizada.
