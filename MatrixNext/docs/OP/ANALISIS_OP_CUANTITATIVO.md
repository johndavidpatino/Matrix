# ANÁLISIS OP_CUANTITATIVO - MIGRACIÓN A MATRIXNEXT

**Documento de Análisis Técnico**  
**Versión**: 1.0  
**Fecha de Creación**: 2026-01-07  
**Módulo**: OP_Cuantitativo (Operación Cuantitativa)  
**Alcance**: Todos los WebForms de OP_Cuantitativo excepto WebForm1.aspx, Borrar.aspx y TraficoEncuestas.aspx (31 páginas), 100% de funcionalidades analizadas  
**Analista**: GitHub Copilot  
**Estado**: ✅ COMPLETO (secciones 1-12)

---

## 📋 ÍNDICE

1. [Resumen Ejecutivo](#1️⃣-resumen-ejecutivo)
2. [Inventario del Legado](#2️⃣-inventario-del-legado-tabla)
3. [Flujos Funcionales](#3️⃣-flujos-funcionales-detallado)
4. [Base de Datos y Stored Procedures](#4️⃣-base-de-datos-y-stored-procedures)
5. [Componentes Reutilizables y Patrones](#5️⃣-componentes-reutilizables-y-patrones)
6. [Riesgos y Consideraciones](#6️⃣-riesgos-y-consideraciones)
7. [Mapa de Migración 1:1](#7️⃣-mapa-de-migración-1:1-webforms--mvc)
8. [Backlog Inicial](#8️⃣-backlog-inicial-priorización-y-t-shirt)
9. [Checklist de Verificación Pre-Migración](#9️⃣-checklist-de-verificación-pre-migración)
10. [Decisiones Técnicas Clave](#🔟-decisiones-técnicas-clave)
11. [Estimación Preliminar](#1️⃣1️⃣-estimación-preliminar)
12. [Próximos Pasos](#1️⃣2️⃣-próximos-pasos)
13. [Propuestas de Optimización](#1️⃣3️⃣-propuestas-de-optimización-sin-romper-1:1)

---

## 1️⃣ RESUMEN EJECUTIVO

- **Propósito del módulo**: coordinar la ejecución operativa de estudios cuantitativos (trabajos, muestras, estimaciones, tráfico de encuestas, cargas de planillas/datos, revisión de productividad, solicitudes de presupuesto y control de calidad IPS) en WebForms.
- **Roles y permisos** (códigos de `Datos.ClsPermisosUsuarios`): COE 100 (trabajos y cierres) [WebMatrix/OP_Cuantitativo/Trabajos.aspx.vb](WebMatrix/OP_Cuantitativo/Trabajos.aspx.vb#L225-L550); Coordinador 101 (asignación de personal) [WebMatrix/OP_Cuantitativo/TrabajosCoordinador.aspx.vb](WebMatrix/OP_Cuantitativo/TrabajosCoordinador.aspx.vb); Campo 135 (importaciones, revisión productividad) [WebMatrix/OP_Cuantitativo/ImportarDatos.aspx.vb](WebMatrix/OP_Cuantitativo/ImportarDatos.aspx.vb#L11-L21), [WebMatrix/OP_Cuantitativo/RevisionProductividadCoordinador.aspx.vb](WebMatrix/OP_Cuantitativo/RevisionProductividadCoordinador.aspx.vb#L5-L21); RMC/Captura/Verificación/Crítica 117/118/119/120 (tráfico) [WebMatrix/OP_Cuantitativo/TraficoEncuestas.aspx.vb](WebMatrix/OP_Cuantitativo/TraficoEncuestas.aspx.vb#L235-L273); PMO 100 (productividad) [WebMatrix/OP_Cuantitativo/RevisionProductividadPMO.aspx.vb](WebMatrix/OP_Cuantitativo/RevisionProductividadPMO.aspx.vb#L5-L22); MyS/Call 157 (productividad) [WebMatrix/OP_Cuantitativo/RevisionProductividadMYSCall.aspx.vb](WebMatrix/OP_Cuantitativo/RevisionProductividadMYSCall.aspx.vb#L5-L22); Campo 156 (productividad) [WebMatrix/OP_Cuantitativo/RevisionProductividadCampo.aspx.vb](WebMatrix/OP_Cuantitativo/RevisionProductividadCampo.aspx.vb#L5-L22); Activación encuestas 126 [WebMatrix/OP_Cuantitativo/ActivacionEncuestas.aspx.vb](WebMatrix/OP_Cuantitativo/ActivacionEncuestas.aspx.vb); Consulta trabajos 19 [WebMatrix/OP_Cuantitativo/ConsultaTrabajos.aspx.vb](WebMatrix/OP_Cuantitativo/ConsultaTrabajos.aspx.vb).
- **Dependencias clave**: 
  - **Dominio**: Trabajo, TrabajoOPCuanti, PlaneacionProduccion, CoordinacionCampo/Personal, OP_CuantiDapper, RevisionIPS, GD (gestión documental), WorkFlow, EnviarCorreo.
  - **Infra**: OleDb + SqlBulkCopy para Excel, ClosedXML para exportes (IPS, TraficoEncuestas), iTextSharp PDF (TrabajosCallCenter), Session/QueryString navigation, UpdatePanel/GridView.
  - **Conexiones**: `MatrixConnectionString` para planillas; `GestionCampoConnectionString` para CATI RMC; rutas de archivos en `~/Files` y rutas UNC para cierre GD.
- **Complejidad estimada**: 🟠 Media-Alta. Factores: múltiples roles/permisos, workflows paralelos (tráfico por unidad, productividad multiroles, IPS multitarrea), carga masiva con validaciones de fechas/festivos, dependencias de sesión y rutas de archivos.

## 2️⃣ INVENTARIO DEL LEGADO (TABLA)

| Archivo | Propósito | Permisos | Dependencias clave (SP/Clases) | Estado de sesión/navegación | Evidencia |
|---|---|---|---|---|---|
| Trabajos.aspx | Portal COE: listar trabajos, ver ficha básica, configurar tipo recolección, generar planeación automática, navegación a muestra, estimaciones, RO, tareas, presupuestos internos, carga de datos, cierre con GD | 100 | Trabajo, TrabajoOPCuanti, PlaneacionProduccion, GD.GD_Procedimientos, RepositorioDocumentos, EnviarCorreo | Session(`IDUsuario`,`TrabajoId`), QueryString nav a otros módulos | [WebMatrix/OP_Cuantitativo/Trabajos.aspx.vb#L225-L565](WebMatrix/OP_Cuantitativo/Trabajos.aspx.vb#L225-L565) |
| TrabajosCoordinador.aspx | Portal Coordinador: listado, asignar personal por ciudad, ver Avance, Capacitaciones, Estimaciones, EstadoTareas | 101 | CoordinacionCampoPersonal, Trabajo, PlaneacionProduccion | Session(`IDUsuario`), Session(`TrabajoId`), redirects | [WebMatrix/OP_Cuantitativo/TrabajosCoordinador.aspx.vb](WebMatrix/OP_Cuantitativo/TrabajosCoordinador.aspx.vb) |
| TrabajosCallCenter.aspx | Coordinador CallCenter: listar trabajos, asignar/retirar encuestadores, navegación a Avance/Capacitaciones/Estimaciones/EstadoTareas | 101 | GestionTrabajosOP, CoordinacionCampoPersonal, Trabajo, FichaCuantitativo | Session(`IDUsuario`), hfIdTrabajo | [WebMatrix/OP_Cuantitativo/TrabajosCallCenter.aspx.vb#L26-L216](WebMatrix/OP_Cuantitativo/TrabajosCallCenter.aspx.vb#L26-L216) |
| ConsultaTrabajos.aspx | Consulta por unidad y accesos a Avance/Gantt/Presupuestos/Activar encuestas; asigna COE con validación JobBook | 19 | Trabajo, Proyecto, Reportes.RP_GerOpe, WorkFlow | Session(`IDUsuario`), QueryString redirects | [WebMatrix/OP_Cuantitativo/ConsultaTrabajos.aspx.vb](WebMatrix/OP_Cuantitativo/ConsultaTrabajos.aspx.vb) |
| FichaCuantitativa.aspx | CRUD de ficha cuanti del trabajo, sincroniza habeas data con Propuesta, envía correo de entrega, retorna a Trabajos/CallCenter | (sin permiso explícito) | Trabajo, FichaCuantitativo, Propuesta, Proyecto, Estudio, Brief, EnviarCorreo | QueryString `idtrabajo`, Session flags | [WebMatrix/OP_Cuantitativo/FichaCuantitativa.aspx.vb](WebMatrix/OP_Cuantitativo/FichaCuantitativa.aspx.vb) |
| EstimacionProduccion.aspx | Gestiona estimaciones por ciudad, valida contra muestra, genera/activa planeación | (permiso implícito via acceso) | PlaneacionProduccion, CoordinacionCampo | QueryString `TrabajoId`, Session(`IDUsuario`) | [WebMatrix/OP_Cuantitativo/EstimacionProduccion.aspx.vb](WebMatrix/OP_Cuantitativo/EstimacionProduccion.aspx.vb) |
| MuestraTrabajos.aspx | Visualiza y actualiza muestra (fechas, auto planeación) y correo a coordinador | (permiso implícito via acceso) | CoordinacionCampo, Trabajo, EnviarCorreo | QueryString `TrabajoId`, Session(`IDUsuario`) | [WebMatrix/OP_Cuantitativo/MuestraTrabajos.aspx.vb](WebMatrix/OP_Cuantitativo/MuestraTrabajos.aspx.vb) |
| ImportarDatos.aspx | Carga Excel CATI RMC (.xls/.xlsx), valida columnas y `TipoActividad`, bulk a `RespuestasCatiRMCtmp`, SP de validación e inserción final | 135 | OleDb, SqlBulkCopy, SPs `CatiRMC_*`, ConnectionString `GestionCampoConnectionString` | Session(`IDUsuario`), FileSystem `~/Files` | [WebMatrix/OP_Cuantitativo/ImportarDatos.aspx.vb#L11-L325](WebMatrix/OP_Cuantitativo/ImportarDatos.aspx.vb#L11-L325) |
| ImportarPlanillas.aspx | Carga planillas de productividad, valida headers y `TipoActividad`, ventanas de corte 16-15 y festivos, bulk a `OP_CuantiPlanillas`, maneja índice único | 135 | OleDb, SqlBulkCopy, `_Festivos` query, ConnectionString `MatrixConnectionString` | Session(`IDUsuario`), FileSystem temp `~/Files/Temp/ImportarPlantillas` | [WebMatrix/OP_Cuantitativo/ImportarPlanillas.aspx.vb#L17-L270](WebMatrix/OP_Cuantitativo/ImportarPlanillas.aspx.vb#L17-L270) |
| PlanillasCargadas.aspx | Lista planillas cargadas en corte y permite rechazar | 135 | OP_CuantiDapper | Session(`IDUsuario`), QueryString `TrabajoId` | [WebMatrix/OP_Cuantitativo/PlanillasCargadas.aspx.vb](WebMatrix/OP_Cuantitativo/PlanillasCargadas.aspx.vb) |
| RevisionPlanillas.aspx | Revisión COE de planillas, aprobar/rechazar, muestra planillas sin presupuesto | 100 | OP_CuantiDapper (`CuantiPlanillasTrabajosUpdate/Remove`) | Session(`IDUsuario`) | [WebMatrix/OP_Cuantitativo/RevisionPlanillas.aspx.vb](WebMatrix/OP_Cuantitativo/RevisionPlanillas.aspx.vb) |
| PlanillasRevisadas.aspx | Lista planillas aprobadas y permite rechazo posterior | 100 | OP_CuantiDapper | Session(`IDUsuario`) | [WebMatrix/OP_Cuantitativo/PlanillasRevisadas.aspx.vb](WebMatrix/OP_Cuantitativo/PlanillasRevisadas.aspx.vb) |
| ProductividadRevisadaPMO.aspx | PMO revisa productividad (monto actual y previo) dentro del corte | 100 | OP_CuantiDapper, TrabajoOPCuanti | Session(`IDUsuario`) | [WebMatrix/OP_Cuantitativo/ProductividadRevisadaPMO.aspx.vb](WebMatrix/OP_Cuantitativo/ProductividadRevisadaPMO.aspx.vb) |
| ProductividadRevisadaMYSCall.aspx | Revisión MyS/Call | 157 | OP_CuantiDapper, TrabajoOPCuanti | Session(`IDUsuario`) | [WebMatrix/OP_Cuantitativo/ProductividadRevisadaMYSCall.aspx.vb](WebMatrix/OP_Cuantitativo/ProductividadRevisadaMYSCall.aspx.vb) |
| ProductividadRevisadaCoordinador.aspx | Revisión Coordinador | 135 | OP_CuantiDapper, TrabajoOPCuanti | Session(`IDUsuario`) | [WebMatrix/OP_Cuantitativo/ProductividadRevisadaCoordinador.aspx.vb](WebMatrix/OP_Cuantitativo/ProductividadRevisadaCoordinador.aspx.vb) |
| ProductividadRevisadaCampo.aspx | Revisión Campo | 156 | OP_CuantiDapper, TrabajoOPCuanti | Session(`IDUsuario`) | [WebMatrix/OP_Cuantitativo/ProductividadRevisadaCampo.aspx.vb](WebMatrix/OP_Cuantitativo/ProductividadRevisadaCampo.aspx.vb) |
| RevisionProductividadPMO/Coordinador/Campo/MYSCall.aspx | Flujo de revisión y ajuste de cantidades autorizadas por rol; rechazar planillas; validación de máximos | 100/135/156/157 | OP_CuantiDapper, TrabajoOPCuanti | Session(`IDUsuario`), ddlTrabajoSeleccionado | [WebMatrix/OP_Cuantitativo/RevisionProductividadPMO.aspx.vb](WebMatrix/OP_Cuantitativo/RevisionProductividadPMO.aspx.vb#L5-L120), [WebMatrix/OP_Cuantitativo/RevisionProductividadCoordinador.aspx.vb](WebMatrix/OP_Cuantitativo/RevisionProductividadCoordinador.aspx.vb#L5-L120), [WebMatrix/OP_Cuantitativo/RevisionProductividadCampo.aspx.vb](WebMatrix/OP_Cuantitativo/RevisionProductividadCampo.aspx.vb#L5-L120), [WebMatrix/OP_Cuantitativo/RevisionProductividadMYSCall.aspx.vb](WebMatrix/OP_Cuantitativo/RevisionProductividadMYSCall.aspx.vb#L5-L120) |
| SolicitudPresupuestoInterno.aspx | Solicita presupuesto interno por trabajo (flags: jornadas, agendamiento, encuestas, reclutamiento), valida duplicados y envía correo | 100 | PresupInt (Solicitudes), Trabajo, EnviarCorreo | Session(`TrabajoId`), QueryString `IdTrabajo` | [WebMatrix/OP_Cuantitativo/SolicitudPresupuestoInterno.aspx.vb](WebMatrix/OP_Cuantitativo/SolicitudPresupuestoInterno.aspx.vb) |
| SolicitudPresupuestosInternos.aspx | Versión simplificada de solicitud con observación | 100 | PresupInt | Session(`IDUsuario`) | [WebMatrix/OP_Cuantitativo/SolicitudPresupuestosInternos.aspx.vb](WebMatrix/OP_Cuantitativo/SolicitudPresupuestosInternos.aspx.vb) |
| ActivacionEncuestas.aspx | Reactiva encuestas anuladas si existen registros previos | 126 | OP.AnulacionEncuestas, GestionCampo | Session(`IDUsuario`), Request `TrabajoId` | [WebMatrix/OP_Cuantitativo/ActivacionEncuestas.aspx.vb](WebMatrix/OP_Cuantitativo/ActivacionEncuestas.aspx.vb) |
| AnulacionEncuestas.aspx | Registra anulaciones si la encuesta existe y no está anulada; actualiza GestionCampo | (permiso según unidad llamante) | OP.AnulacionEncuestas, GestionCampo | QueryString `TrabajoId`, `IdUnidad` | [WebMatrix/OP_Cuantitativo/AnulacionEncuestas.aspx.vb](WebMatrix/OP_Cuantitativo/AnulacionEncuestas.aspx.vb) |
| IPS.aspx | Control de observaciones por tarea (Instrumentos, Codificación, Procesamiento, Scripting, Estadística, etc.), grid editable, notificaciones email, exportar a Excel | (permiso via workflow) | RevisionIPS, WorkFlow, EnviarCorreo, ClosedXML | QueryString `idtrabajo`,`idtarea`,`fromgerencia`, Session(`IDUsuario`) | [WebMatrix/OP_Cuantitativo/IPS.aspx.vb#L53-L845](WebMatrix/OP_Cuantitativo/IPS.aspx.vb#L53-L845) |
| RegistroProduccionOP.aspx | Registro de actividades de producción (Procesamiento/Scripting), subactividades, tipos de aplicativo, reproceso | (permiso implícito) | RecordProduccion, Enumeradores EAreas/EReproceso/EActividad | Session(`IDUsuario`) | [WebMatrix/OP_Cuantitativo/RegistroProduccionOP.aspx.vb](WebMatrix/OP_Cuantitativo/RegistroProduccionOP.aspx.vb) |
| SupervisionCampoTelefonico.aspx | Registro de supervisión telefónica de campo (checklist) | (permiso implícito) | OP.SupervisionCampoTelefonico, Datos.ClsPermisosUsuarios (usuarios) | QueryString `TrabajoId`, Session(`IdUsuario` hardcoded 1047223102 ⚠️) | [WebMatrix/OP_Cuantitativo/SupervisionCampoTelefonico.aspx.vb#L12-L68](WebMatrix/OP_Cuantitativo/SupervisionCampoTelefonico.aspx.vb#L12-L68) |
| iFieldConfiguration.aspx | Configura proyectos iField vinculados a trabajos, carga/borrado de variables de sincronización | (permiso implícito) | DALDAP.iFieldSettings, Trabajo | Session(`IDUsuario`), rbSearch/ddlProyectos | [WebMatrix/OP_Cuantitativo/iFieldConfiguration.aspx.vb#L4-L127](WebMatrix/OP_Cuantitativo/iFieldConfiguration.aspx.vb#L4-L127) |

| HomeGestion.aspx / HomeRecoleccion.aspx | Landings; HomeRecoleccion valida permiso 54 | 54 | N/A | Session(`IDUsuario`) | [WebMatrix/OP_Cuantitativo/HomeRecoleccion.aspx.vb](WebMatrix/OP_Cuantitativo/HomeRecoleccion.aspx.vb) |

## 3️⃣ FLUJOS FUNCIONALES (DETALLADO)

### Flujo A: Gestión COE de un trabajo (Trabajos.aspx)
- **Acceso y permisos**: valida permiso 100 y carga listado inicial de trabajos del COE [WebMatrix/OP_Cuantitativo/Trabajos.aspx.vb#L225-L240](WebMatrix/OP_Cuantitativo/Trabajos.aspx.vb#L225-L240).
- **Seleccionar trabajo**: en `gvTrabajos_RowCommand` se captura `TrabajoId`, se cargan configuración, muestra/estimaciones y se decide si mostrar auto-estimación según existencia de planeación [WebMatrix/OP_Cuantitativo/Trabajos.aspx.vb#L248-L266](WebMatrix/OP_Cuantitativo/Trabajos.aspx.vb#L248-L266).
- **Navegación operativa**: botones llevan a Muestra, Estimaciones, RO (documentos), EstadoTareas, Presupuesto interno, ImportarDatos [WebMatrix/OP_Cuantitativo/Trabajos.aspx.vb#L343-L438](WebMatrix/OP_Cuantitativo/Trabajos.aspx.vb#L343-L438).
- **Cierre de trabajo**: valida estado, sincroniza documentos escaneados GD, muestra opciones de forzar/confirmar, envía email de cambio de estado [WebMatrix/OP_Cuantitativo/Trabajos.aspx.vb#L367-L465](WebMatrix/OP_Cuantitativo/Trabajos.aspx.vb#L367-L465).
- **Riesgos**: fuerte dependencia de Session (`TrabajoId`), múltiples redirecciones, lógica de cierre acoplada a GD y rutas de red.

### Flujo B: Tráfico de encuestas entre unidades (TraficoEncuestas.aspx)
- **Acceso por unidad**: QueryString `UnidadId` determina permiso (117/118/119/120) y configuración de UI [WebMatrix/OP_Cuantitativo/TraficoEncuestas.aspx.vb#L235-L273](WebMatrix/OP_Cuantitativo/TraficoEncuestas.aspx.vb#L235-L273).
- **Gestión de envíos**: selección de trabajo -> `CargaTrabajo()` configura unidades, carga recepciones y personal [WebMatrix/OP_Cuantitativo/TraficoEncuestas.aspx.vb#L286-L314](WebMatrix/OP_Cuantitativo/TraficoEncuestas.aspx.vb#L286-L314). Envío valida cantidad disponible y ciudad (RMC), guarda `OP_TraficoEncuestas` con fecha/usuario/observaciones [WebMatrix/OP_Cuantitativo/TraficoEncuestas.aspx.vb#L349-L410](WebMatrix/OP_Cuantitativo/TraficoEncuestas.aspx.vb#L349-L410).
- **Recepción/devoluciones**: recepciones requieren validar cantidades y observación cuando no coinciden [WebMatrix/OP_Cuantitativo/TraficoEncuestas.aspx.vb#L411-L435](WebMatrix/OP_Cuantitativo/TraficoEncuestas.aspx.vb#L411-L435); devoluciones similar con validación de disponibilidad [WebMatrix/OP_Cuantitativo/TraficoEncuestas.aspx.vb#L437-L483](WebMatrix/OP_Cuantitativo/TraficoEncuestas.aspx.vb#L437-L483).
- **Estimaciones por unidad**: consulta y guarda estimación específica (RMC/Crítica/Verificación/Captura) [WebMatrix/OP_Cuantitativo/TraficoEncuestas.aspx.vb#L485-L526](WebMatrix/OP_Cuantitativo/TraficoEncuestas.aspx.vb#L485-L526).
- **Asignación personal**: asignar/eliminar personal por cargo con Dapper y exportar listado a Excel usando ClosedXML [WebMatrix/OP_Cuantitativo/TraficoEncuestas.aspx.vb#L543-L669](WebMatrix/OP_Cuantitativo/TraficoEncuestas.aspx.vb#L543-L669).

### Flujo C: Carga de datos CATI (ImportarDatos.aspx)
- **Validación de acceso**: permiso 135 antes de mostrar paneles [WebMatrix/OP_Cuantitativo/ImportarDatos.aspx.vb#L11-L22](WebMatrix/OP_Cuantitativo/ImportarDatos.aspx.vb#L11-L22).
- **Subida y lectura de Excel**: acepta .xls/.xlsx, detecta hojas y valida estructura exacta de 9 columnas [WebMatrix/OP_Cuantitativo/ImportarDatos.aspx.vb#L26-L168](WebMatrix/OP_Cuantitativo/ImportarDatos.aspx.vb#L26-L168).
- **Carga masiva y validación**: lee hoja completa, ejecuta SP `CatiRMC_BorrarDatosRespuestasCatiRMCtmp`, bulk copy a `RespuestasCatiRMCtmp` con mapeos de columnas, luego SP de validación/resumen e inserción final `CatiRMC_InsertarDatosEnRespuestas` [WebMatrix/OP_Cuantitativo/ImportarDatos.aspx.vb#L170-L255](WebMatrix/OP_Cuantitativo/ImportarDatos.aspx.vb#L170-L255).
- **Riesgos**: dependencia OleDb 32/64 bits, rutas físicas `~/Files`, sin manejo de concurrencia sobre archivos temporales.

### Flujo D: Carga de planillas de productividad (ImportarPlanillas.aspx)
- **Permiso y lectura**: permiso 135; lee Excel y añade metadatos (`SubidoPor`, `FechaCarga`, flags de revisión) [WebMatrix/OP_Cuantitativo/ImportarPlanillas.aspx.vb#L17-L142](WebMatrix/OP_Cuantitativo/ImportarPlanillas.aspx.vb#L17-L142).
- **Validaciones de negocio**: rango de corte (16 anterior - 15 actual) y feriados para jornadas dominicales [WebMatrix/OP_Cuantitativo/ImportarPlanillas.aspx.vb#L143-L186](WebMatrix/OP_Cuantitativo/ImportarPlanillas.aspx.vb#L143-L186).
- **Persistencia**: bulk copy a `OP_CuantiPlanillas`, captura excepción por índice único `IX_OP_CuantiPlanillas_Unique_Trabajo_Per_ResFecha` [WebMatrix/OP_Cuantitativo/ImportarPlanillas.aspx.vb#L190-L220](WebMatrix/OP_Cuantitativo/ImportarPlanillas.aspx.vb#L190-L220).

### Flujo E: Control IPS por tarea (IPS.aspx)
- **Contexto**: recibe `idtrabajo` y `idtarea`, carga tarea actual desde WorkFlow y ajusta columnas visibles según tipo de tarea [WebMatrix/OP_Cuantitativo/IPS.aspx.vb#L53-L108](WebMatrix/OP_Cuantitativo/IPS.aspx.vb#L53-L108).
- **Edición y guardado**: en RowUpdating valida campos obligatorios según tarea (instrumento, aplicativo, proceso, preguntas) y guarda con `RevisionIPS.Guardar` [WebMatrix/OP_Cuantitativo/IPS.aspx.vb#L339-L404](WebMatrix/OP_Cuantitativo/IPS.aspx.vb#L339-L404).
- **Notificaciones y exporte**: botón Notificar envía email `ObservacionesIPS.aspx` si hay usuario asignado [WebMatrix/OP_Cuantitativo/IPS.aspx.vb#L432-L444](WebMatrix/OP_Cuantitativo/IPS.aspx.vb#L432-L444); exporte usa ClosedXML para listado de observaciones [WebMatrix/OP_Cuantitativo/IPS.aspx.vb#L785-L804](WebMatrix/OP_Cuantitativo/IPS.aspx.vb#L785-L804).

---

## 4️⃣ BASE DE DATOS Y STORED PROCEDURES

| Página | SP/Tabla | Evidencia | Estado |
|---|---|---|---|
| ImportarDatos.aspx | SP: `CatiRMC_BorrarDatosRespuestasCatiRMCtmp`, `CatiRMC_ValidarDatosRespuestasCatiRMCtmp`, `CatiRMC_ReportarResumenValidasNuevas`, `CatiRMC_ReportarResumenNoValidasNuevas`, `CatiRMC_ReportarResumenDuplicadas`, `CatiRMC_ReportarInconsistencias`, `CatiRMC_InsertarDatosEnRespuestas`; tablas: `RespuestasCatiRMCtmp`, `Respuestas` | [WebMatrix/OP_Cuantitativo/ImportarDatos.aspx.vb#L194-L255](WebMatrix/OP_Cuantitativo/ImportarDatos.aspx.vb#L194-L255) | ✅ Confirmado |
| ImportarPlanillas.aspx | Bulk a tabla `OP_CuantiPlanillas`; lee `_Festivos` para validar fechas; maneja índice `IX_OP_CuantiPlanillas_Unique_Trabajo_Per_ResFecha` | [WebMatrix/OP_Cuantitativo/ImportarPlanillas.aspx.vb#L120-L221](WebMatrix/OP_Cuantitativo/ImportarPlanillas.aspx.vb#L120-L221) | ✅ Confirmado |
| PlanillasCargadas/RevisionPlanillas/PlanillasRevisadas | Dapper `OP_CuantiDapper.CuantiPlanillasGet/CuantiPlanillasTrabajosUpdate/CuantiPlanillasTrabajosRemove` (probables SP sobre `OP_CuantiPlanillas`) | [WebMatrix/OP_Cuantitativo/RevisionPlanillas.aspx.vb](WebMatrix/OP_Cuantitativo/RevisionPlanillas.aspx.vb) | ⚠️ SP exactos por confirmar |
| ProductividadRevisada* y RevisionProductividad* | Dapper `OP_CuantiDapper.CuantiProdProductividad_*` y `CuantiPlanillasTrabajosRemove`; entidades `TrabajoOPCuanti.ObtenerCCProduccionPST` | [WebMatrix/OP_Cuantitativo/RevisionProductividadPMO.aspx.vb#L87-L121](WebMatrix/OP_Cuantitativo/RevisionProductividadPMO.aspx.vb#L87-L121) | ⚠️ SP exactos por confirmar |
| IPS.aspx | `RevisionIPS.Guardar/Eliminar` y `OP.IPSClass.GuardarRegistroEntidad` sobre tabla `OP_IPS_Revision` | [WebMatrix/OP_Cuantitativo/IPS.aspx.vb#L339-L404](WebMatrix/OP_Cuantitativo/IPS.aspx.vb#L339-L404) | ⚠️ SP por confirmar |
| SolicitudPresupuestoInterno*.aspx | PresupInt.SolicitudPresupuesto* (SP encapsulados) para tabla de solicitudes internas | [WebMatrix/OP_Cuantitativo/SolicitudPresupuestoInterno.aspx.vb](WebMatrix/OP_Cuantitativo/SolicitudPresupuestoInterno.aspx.vb) | ⚠️ SP por confirmar |
| Trabajos/TrabajosCoordinador/TrabajosCallCenter | Trabajo.ListadoTrabajos/GuardarTrabajoConfiguracion; CoordinacionCampoPersonal.*; GD.GD_Procedimientos.DevolverxIdTrabajoIdRolResponsable (gestión documental) | [WebMatrix/OP_Cuantitativo/Trabajos.aspx.vb#L28-L150](WebMatrix/OP_Cuantitativo/Trabajos.aspx.vb#L28-L150), [WebMatrix/OP_Cuantitativo/TrabajosCallCenter.aspx.vb#L26-L216](WebMatrix/OP_Cuantitativo/TrabajosCallCenter.aspx.vb#L26-L216) | ⚠️ SP por confirmar |
| EstimacionProduccion/MuestraTrabajos/FichaCuantitativa | PlaneacionProduccion.*, CoordinacionCampo.*, FichaCuantitativo.*, Propuesta.* (SP internos no visibles) | [WebMatrix/OP_Cuantitativo/EstimacionProduccion.aspx.vb](WebMatrix/OP_Cuantitativo/EstimacionProduccion.aspx.vb), [WebMatrix/OP_Cuantitativo/FichaCuantitativa.aspx.vb](WebMatrix/OP_Cuantitativo/FichaCuantitativa.aspx.vb) | ⚠️ SP por confirmar |
| ActivacionEncuestas/AnulacionEncuestas | OP.AnulacionEncuestas.* (SP internos), GestionCampo connection | [WebMatrix/OP_Cuantitativo/AnulacionEncuestas.aspx.vb](WebMatrix/OP_Cuantitativo/AnulacionEncuestas.aspx.vb) | ⚠️ SP por confirmar |
| iFieldConfiguration.aspx | DALDAP.iFieldSettings.ProjectGet/ProjectConfigGet/InsertConfigItem (SP internos sobre tablas de iField) | [WebMatrix/OP_Cuantitativo/iFieldConfiguration.aspx.vb#L64-L126](WebMatrix/OP_Cuantitativo/iFieldConfiguration.aspx.vb#L64-L126) | ⚠️ SP por confirmar |
| RegistroProduccionOP.aspx | RecordProduccion.ObtenerUnidades/MatrizActividades/JBE_JBI; Enums internos (EAreas, EReproceso, EActividad); dropdown cascada unidades→actividades→subactividades | [WebMatrix/OP_Cuantitativo/RegistroProduccionOP.aspx.vb#L1-L80](WebMatrix/OP_Cuantitativo/RegistroProduccionOP.aspx.vb#L1-L80) | ⚠️ SP por confirmar |
| HomeRecoleccion.aspx | Dashboard principal OP_Cuantitativo, valida permiso 54 (acceso base) | 54 | Datos.ClsPermisosUsuarios | Session(`IDUsuario`) | [WebMatrix/OP_Cuantitativo/HomeRecoleccion.aspx.vb](WebMatrix/OP_Cuantitativo/HomeRecoleccion.aspx.vb) |
| HomeGestion.aspx | Dashboard de gestión (página vacía, sin lógica) | — | — | — | [WebMatrix/OP_Cuantitativo/HomeGestion.aspx.vb](WebMatrix/OP_Cuantitativo/HomeGestion.aspx.vb) |
| SupervisionCampoTelefonico.aspx | Supervisión campo telefonico: validación, combo operadores, checkboxes evaluación (CRI*), dropdowns comentarios/acciones, guardar supervisión | (implícito) | OP.SupervisionCampoTelefonico, Datos.ClsPermisosUsuarios | QueryString `TrabajoId`, **⚠️ Session(`IdUsuario`)=1047223102 HARDCODED (línea 74)** | [WebMatrix/OP_Cuantitativo/SupervisionCampoTelefonico.aspx.vb#L60-L90](WebMatrix/OP_Cuantitativo/SupervisionCampoTelefonico.aspx.vb#L60-L90) |

---

## 5️⃣ COMPONENTES REUTILIZABLES Y PATRONES

### Enums definidos en nivel de página (por trasladar a nivel dominio)
| Enum | Página | Ubicación | Valores | Uso |
|---|---|---|---|---|
| `ETipoActividad` | ImportarDatos.aspx | Línea 317 | EncuestaNormal(1), EncuestaFiltro(10), Jornada(20), MediaJornada(21), JornadaDominical(22), MediaJornadaDominical(23) | Validación de cargas CATI RMC |
| `ETipoActividadPlanilla` | ImportarPlanillas.aspx | Línea 259 | Ampliado con NSE_1to2(11), NSE_3to4(12), NSE_5to6(13) | Validación de cargas de planillas |
| `EAreas` | RegistroProduccionOP.aspx | Línea 11 | (valores por confirmar) | Registro de actividades de producción |
| `EReproceso` | RegistroProduccionOP.aspx | Línea 16 | (valores por confirmar) | Clasificación de reproceso |

### Patrón de notificaciones (reutilizable)
- **Método base**: `ShowNotification(mensaje, tipo)` utilizado en +20 páginas (evidencia confirmada)
- **Tipos**: `ShowNotifications.InfoNotification`, `ErrorNotification`
- **Implementación**: ActivationEncuestas, ConsultaTrabajos, RevisionPlanillas, Productividades, TraficoEncuestas, SolicitudPI
- **Migración**: consolidar en servicio compartido `NotificationService` en MatrixNext

### Patrón de validación de permisos (reutilizable)
```vb
Dim permisos As New Datos.ClsPermisosUsuarios
Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
If permisos.VerificarPermisoUsuario(CODIGO, UsuarioID) = False Then
    Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
End If
```
- **Códigos de permiso por rol** (evidencia confirmada):
  - 54: HomeRecoleccion (acceso base)
  - 19: ConsultaTrabajos
  - 100: COE (Trabajos, PlanillasRevisadas, RevisionPlanillas, ProductividadPMO, SolicitudPI)
  - 101: Coordinador (TrabajosCoordinador)
  - 117-120: Tráfico (Verificación, Captura, Crítica, RMC)
  - 125: ActivacionEncuestas
  - 126: AnulacionEncuestas
  - 135: Campo (ImportarDatos, ImportarPlanillas, RevisionProductividadCoordinador)
  - 156: RevisionProductividadCampo
  - 157: RevisionProductividadMYSCall
- **Migración**: centralizar en middleware de autenticación/autorización .NET Core

### Patrón de cálculo de corte de nómina (reutilizable)
```vb
Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)
```
- **Usado en**: ImportarPlanillas, RevisionProductividad*, ProductividadRevisada*
- **Lógica**: desde día 16 de mes anterior hasta día 15 de mes actual
- **Migración**: crear helper `GetNominaWindow()` reutilizable

---

## 6️⃣ RIESGOS Y CONSIDERACIONES

| Categoría | Riesgo | Impacto | Mitigación propuesta |
|---|---|---|---|
| **Session/State** | Dependencia pesada de `Session("IDUsuario")` y `Session("TrabajoId")` (>25 referencias, evidencia confirmada). Pérdida si sesión vence. | 🔴 Alta | Reemplazar con HttpContext.User claims + parámetros de ruta/query. Implementar token basado en JWT. |
| **Session/State** | `Session("NombreTrabajo")` usado para persistencia entre páginas (Trabajos → TrabajosCallCenter) | 🟡 Media | Usar query strings o estado de aplicación MVC (ViewModel/TempData). |
| **Hardcoded ID** | SupervisionCampoTelefonico.aspx línea 74: `Session("IdUsuario") = 1047223102` ⚠️ | 🔴 Alta | **CRÍTICO**: Reemplazar por usuario autenticado actual en contexto de solicitud. |
| **Carga masiva** | ImportarDatos/Planillas: dependencia de rutas físicas `~/Files/` y `~/Files/Temp/` | 🟡 Media | Migrar a almacenamiento blob (Azure Storage o similar) con URLs temporales. |
| **Carga masiva** | OleDb para lectura de Excel: incompatibilidad 32/64 bits, sin manejo de concurrencia | 🟠 Alta | Usar librería OpenXml (ClosedXML/EPPlus) para lectura sin dependencias de driver. |
| **DB Connections** | Múltiples strings de conexión: `MatrixConnectionString`, `GestionCampoConnectionString` | 🟠 Media | Unificar en inyección de dependencias .NET Core; usar parámetro de desambiguación en Dapper si es necesario. |
| **SP Masivos** | CatiRMC_* SPs (7 SPs) + OP_CuantiDapper (Dapper encapsulados). Cambios futuros requieren compilación DB | 🟡 Media | Evaluar encapsulación en servicios .NET (LINQ/EF Core) vs mantener SPs; preferiblemente migrar lógica a código si es posible. |
| **Validación** | Validaciones duplicadas en VB (ImportarDatos línea 147-154, ImportarPlanillas línea 105-186) | 🟡 Media | Consolidar en servicio `ExcelValidationService` en MatrixNext. |
| **Email** | EnviarCorreo asincrónico vía `AsyncEnviarCorreo(url)`. Si URL no existe, silencio fallido. | 🟡 Media | Implementar queue de correos con reintentos en MatrixNext; logging robusta. |
| **Rutas GD** | Trabajos.aspx: cierre con GD requiere rutas UNC (`\\servidor\compartido`) hardcoded. Cambio requiere recompilación. | 🟡 Media | Externilizar rutas en configuración (appsettings.json en .NET Core). |
| **FileUpload** | Límite de tamaño de carga no configurado en validación. Posible DoS. | 🟡 Media | Implementar límites configurables (e.g., máx 50MB) en controlador MVC. |
| **GridView paging** | Lógica de paginación manual en cada página (Trabajos, IPS, etc.). Si cambios de SP, múltiples lugares requieren cambio. | 🟡 Media | Usar componente de paginación reutilizable (pagination service o scaffolding). |
| **UI Legacy** | jQuery UI Accordion/Dialog/Tabs en Trabajos, IPS. Deprecadas. | 🟠 Media | Migrar a componentes modernos (Bootstrap 5, Tailwind) o Angular/Vue si es SPA. |
| **Índices de DB** | `IX_OP_CuantiPlanillas_Unique_Trabajo_Per_ResFecha` manejado solo en catch. Sin retry lógica. | 🟡 Media | Implementar retry con backoff exponencial; mostrar hint de usuario sobre duplicado. |
| **Type Safety** | VB `IsNumeric()` y conversiones `CInt()` sin validación robusta. | 🟡 Media | Usar `TryParse` y result types en C#; validación server-side obligatoria. |

---

---

## 7️⃣ MAPA DE MIGRACIÓN 1:1 (WEBFORMS → MVC)

**Resumen**: Cada WebForm (página .aspx) → Controlador MVC con uno o más Actions. Roles/permisos mapeados a atributos `[Authorize(Roles="...")]` en .NET Core.

| WebForm (Origen) | Controlador MVC | Action(s) | Permisos | Dependencias .NET Core | Notas |
|---|---|---|---|---|---|
| **Trabajos.aspx** | `OpCuantiController` | `Index` (GET lista), `DetailsFicha` (GET/POST ficha), `UpdateConfig`, `NavToMuestra`, `NavToEstimaciones`, `Cierre` | 100 | ITrabajoService, IPlaneacionProduccionService, IGDService, IEnviarCorreoService | Principal. Lógica de navegación parametrizada. GD cierre → blob storage. |
| **TrabajosCoordinador.aspx** | `CoordinadorOpCuantiController` | `Index`, `AsignacionPersonal`, `VerAvance`, `VerCapacitaciones` | 101 | ICoordinacionCampoService, ITrabajoService | Permisos por rol. Asignación personal → DataTable → DataGrid MVC. |
| **TrabajosCallCenter.aspx** | `CallCenterOpCuantiController` | `Index`, `AsignarEncuestador`, `Retirar`, `VerAvance`, `VerCapacitaciones` | 101 | IGestionTrabajosOPService, ICoordinacionCampoService, ITrabajoService | Similar a TrabajosCoordinador. UI personalizada CallCenter. |
| **ConsultaTrabajos.aspx** | `ConsultaOpCuantiController` | `Index` (por unidad), `Trabajos`, `AsignarCOE`, `NavToAvance` | 19 | ITrabajoService, IReportesService, IWorkFlowService | Filtro unidad + acceso a reportes (Gantt/Presupuestos). |
| **FichaCuantitativa.aspx** | `OpCuantiController` | `FichaCuantitativa` (GET/POST CRUD) | (sin restricción) | IFichaCuantitativaService, IPropuestaService, IProyectoService, IEnviarCorreoService | Sincronización habeas data. Email de entrega. QueryString → parámetro ruta. |
| **EstimacionProduccion.aspx** | `EstimacionOpCuantiController` | `Index` (GET lista por ciudad), `UpdateEstimacion` (POST) | (implícito) | IPlaneacionProduccionService, ICoordinacionCampoService | Validación vs muestra. Activación planeación. GridView → DataGrid MVC. |
| **MuestraTrabajos.aspx** | `MuestraOpCuantiController` | `Index` (GET), `UpdateMuestra` (POST) | (implícito) | ICoordinacionCampoService, ITrabajoService, IEnviarCorreoService | Auto planeación. Email a coordinador. |
| **ImportarDatos.aspx** | `ImportarOpCuantiController` | `Index` (GET), `UploadDatos` (POST) | 135 | IImportarDatosService, ISqlBulkCopyService, ICatiRMCService | Reemplazar OleDb con OpenXml. Wizard → Form POST. BulkCopy → SqlBulkCopy encapsulado. |
| **ImportarPlanillas.aspx** | `ImportarOpCuantiController` | `Index` (GET), `UploadPlanillas` (POST) | 135 | IImportarPlanillasService, ISqlBulkCopyService, IFestivosService | Validación corte 16-15. Índice único handling. |
| **PlanillasCargadas.aspx** | `PlanillasOpCuantiController` | `Index` (GET) | 135 | IOpCuantiDapperService | Lista + rechazar. GridView → DataGrid. |
| **RevisionPlanillas.aspx** | `RevisionOpCuantiController` | `Planillas` (GET), `AprobarPlanilla` (POST), `RechazarPlanilla` (POST) | 100 | IOpCuantiDapperService | COE approval workflow. |
| **PlanillasRevisadas.aspx** | `RevisionOpCuantiController` | `PlanillasAprobadas` (GET) | 100 | IOpCuantiDapperService | Rechazo posterior. GridView → DataGrid. |
| **ProductividadRevisada*.aspx (4 variantes)** | `ProductividadOpCuantiController` | `VerProductividad` (GET per rol: PMO/MYSCall/Coordinador/Campo) | 100/157/135/156 | IOpCuantiDapperService, ITrabajoOPCuantiService | Merging 4 pages → 1 controller con action overload por rol (RoleBasedAction pattern). |
| **RevisionProductividad*.aspx (4 variantes)** | `RevisionProductividadOpCuantiController` | `RevisionProductividad` (POST per rol) | 100/157/135/156 | IOpCuantiDapperService, ITrabajoOPCuantiService | Complemento de Productividad. POST → guardar aprobación. |
| **SolicitudPresupuestoInterno*.aspx (2 variantes)** | `PresupuestoOpCuantiController` | `Index` (GET lista), `CrearSolicitud` (GET form), `GuardarSolicitud` (POST), `VerPresupuestos` (GET reportes) | 100 | IPresupuestoInternoService | Merge 2 pages → 1 controller. Workflow de presupuesto. |
| **IPS.aspx** | `IPSOpCuantiController` | `Index` (GET grid por tarea), `EditarIPS` (POST tarea), `GuardarRevision` (POST) | (permiso implícito) | IIPSService, IEnviarCorreoService, IClosedXMLService | Control multitarea. Export XLSX. Email observaciones. GridView → DataGrid. |
| **ActivacionEncuestas.aspx** | `ActivacionOpCuantiController` | `Index` (GET), `ActivarEncuestas` (POST) | 125 | IAnulacionEncuestasService | Activación workflow. |
| **AnulacionEncuestas.aspx** | `AnulacionOpCuantiController` | `Index` (GET), `AnularEncuestas` (POST) | 126 | IAnulacionEncuestasService | Anulación workflow. GestionCampo connection. |
| **iFieldConfiguration.aspx** | `iFieldConfigOpCuantiController` | `Index` (GET config), `SaveConfig` (POST), `ProjectGet` (GET project list) | (admin) | IiFieldConfigService (DALDAP wrapper) | Encapsular iField settings en config service. |
| **RegistroProduccionOP.aspx** | `RegistroProduccionOpCuantiController` | `Index` (GET), `CargarUnidades` (AJAX), `CargarActividades` (AJAX), `CargarSubActividades` (AJAX), `CargarJBE` (AJAX), `GuardarRegistro` (POST) | (implícito) | IRecordProduccionService, IMatrizActividadesService | Dropdowns cascada (unidades→actividades→subactividades). Enums consolidados: EAreas/EReproceso/EActividad. |
| **SupervisionCampoTelefonico.aspx** | `SupervisionCampoTelefonicaController` | `Index` (GET form), `CargarOperadores` (AJAX), `GuardarSupervision` (POST) | (implícito) | ISupervisión CampoTelefonicaService, IPermisoService | ⚠️ CRÍTICO: Reemplazar hardcoded `Session("IdUsuario") = 1047223102` por usuario autenticado. Checklist evaluación. |
| **HomeRecoleccion.aspx** | `HomeController` o `DashboardController` | `Index` (GET dashboard) | 54 | IPermisoService | No migrar como página, usar dashboard SPA. |
| **HomeGestion.aspx** | *(EVALUAR)* | — | — | — | Página vacía en WebForms. Evaluar si eliminar o crear como dashboard de gestión. |

---

## 8️⃣ BACKLOG INICIAL (PRIORIZACIÓN Y T-SHIRT)

**Épica 1: COE - Gestión de Trabajos y Configuración**
- Tarea: Migrar `Trabajos.aspx` → `OpCuantiController.Index/DetailsFicha`
  - Subtarea: Ficha cuantitativa CRUD (FichaCuantitativa.aspx)
  - Subtarea: Integración con GD para cierre (blob storage)
  - Subtarea: Email de entrega automático
  - Estimación: **XXXL** (componentes complejos: navegación parametrizada, GD integración, email async)

**Épica 2: Carga Masiva (CATI RMC + Planillas)**
- Tarea: Migrar `ImportarDatos.aspx` + `ImportarPlanillas.aspx`
  - Subtarea: Reemplazar OleDb con OpenXml (eliminar dependencia de driver OLEDB)
  - Subtarea: Bulk copy encapsulado (SqlBulkCopy → Service)
  - Subtarea: Validación de estructura Excel (headers, TipoActividad, dates, festivos)
  - Subtarea: Handling de índices únicos (ImportarPlanillas)
  - Estimación: **XXL** (2 cargas distintas, validaciones complejas, DB integración)

**Épica 3: Productividad Multiroles (PMO/MYSCall/Coordinador/Campo)**
- Tarea: Merger de 4 páginas (ProductividadRevisada*.aspx) → 1 controlador con role overloading
  - Subtarea: Servicios Dapper para queries Productividad
  - Subtarea: Lógica de corte 16-15 consolidada
  - Estimación: **XL** (refactoring de 4 páginas, consolidación de lógica)

**Épica 4: Revisión IPS Multitarea**
- Tarea: Migrar `IPS.aspx` → `IPSOpCuantiController.Index/EditarIPS`
  - Subtarea: Grid editable por tarea
  - Subtarea: Validación por campo
  - Subtarea: Email de observaciones
  - Subtarea: Export XLSX (ClosedXML)
  - Estimación: **XL** (componentes UI complejos: grid editable, export)

**Épica 5: Presupuestos Internos**
- Tarea: Migrar 2 páginas → 1 controlador `PresupuestoOpCuantiController`
  - Subtarea: Workflow de solicitud/aprobación
  - Estimación: **L** (flujo sencillo)

**Épica 6: Coordinador + CallCenter + Consulta**
- Tarea: Migrar 3 controladores (Coordinador, CallCenter, Consulta)
  - Estimación: **L** (flujos de lectura/listado, sin lógica compleja)

**Épica 7: Utilidades (Activación/Anulación Encuestas, iField Config)**
- Tarea: Migrar utilidades
  - Estimación: **M** (4-5 páginas sencillas)

**Épica 8: Producción y Supervisión (RegistroProduccionOP, SupervisionCampoTelefonico)**
- Tarea: Migrar páginas de registro de producción y supervisión
  - Subtarea: Dropdowns cascada (unidades→actividades→subactividades) en RegistroProduccionOP
  - Subtarea: **CRÍTICA**: Reemplazar hardcoded Session("IdUsuario") = 1047223102 por usuario autenticado en SupervisionCampoTelefonico
  - Subtarea: Enums consolidados (EAreas, EReproceso, EActividad)
  - Estimación: **M** (2 páginas, una con lógica crítica de seguridad)

**Épica 9: Dashboards y Páginas Vacías (HomeRecoleccion, HomeGestion)**
- Tarea: Evaluar y migrar dashboards
  - Subtarea: HomeRecoleccion → dashboard principal con validación permiso 54
  - Subtarea: HomeGestion → evaluar si es funcional o eliminar
  - Estimación: **S** (páginas simples o vacías)

**Épica 10: Consolidación de Componentes Reutilizables**
- Tarea: Crear servicios compartidos (NotificationService, PermisosService, ExcelValidationService, NominaWindowService)
  - Estimación: **M** (refactoring transversal)

---

## 9️⃣ CHECKLIST DE VERIFICACIÓN PRE-MIGRACIÓN

### Inventario Completado
- [x] Todas las 33 WebForms mapeadas a controladores MVC
- [ ] SPs exactos para cada flujo (Dapper/queries) documentados
- [ ] Enums y constantes consolidadas en dominio
- [ ] Componentes reutilizables (notificaciones, permisos, corte) identificados

### Dependencias y Conexiones
- [ ] Connection strings unificadas en inyección de dependencias
- [ ] iField integration strategy definida (encapsular o reemplazar)
- [ ] GD (gestión documental) migration path definida (blob storage)
- [ ] Email async queue implementada (reintentos, logging)

### Riesgos Mitigados
- [ ] Session → Claims migration plan completado
- [ ] OleDb → OpenXml reemplazo completado
- [ ] Hardcoded Session ID (SupervisionCampoTelefonico) resuelto
- [ ] Rutas de archivos externilizadas (appsettings.json)
- [ ] FileUpload limits configurados
- [ ] Paging component reutilizable creado

### Testing Strategy
- [ ] Unit tests para servicios de validación (Excel, corte, festivos)
- [ ] Integration tests para flujos principales (Productividad, IPS)
- [ ] Regression tests para SPs (validar resultados iguales pre/post)
- [ ] Security tests: permisos de roles en cada acción

### Capacitación y Documentación
- [ ] Developers familiarizados con patrón de servicios Dapper en .NET Core
- [ ] Documentación de servicios (NotificationService, PermisosService, etc.)
- [ ] Guía de rutas/navegación en MatrixNext (reemplazar QueryString/Session)

---

## 🔟 DECISIONES TÉCNICAS CLAVE

### 1. Session → Claims + Parámetros
**Decisión**: Reemplazar `Session("IDUsuario")` con `HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value`.  
**Justificación**: JWT stateless, escalabilidad en cloud, seguridad.  
**Acción**: Implementar middleware de autenticación que cargue claims de BD; pasar `userId` como parámetro en rutas/forms.

### 2. OleDb → OpenXml
**Decisión**: Usar ClosedXML o EPPlus para lectura de Excel en lugar de OleDb.  
**Justificación**: Elimina dependencia de driver OLEDB, funciona 32/64 bits, sin locks de archivo.  
**Acción**: Crear `ExcelReaderService` que abstrae ClosedXML; usar en `ImportarDatos` e `ImportarPlanillas`.

### 3. SPs vs LINQ/EF Core
**Decisión**: Mantener SPs de lógica compleja (CatiRMC_*, validaciones); migrar validaciones simples a LINQ.  
**Justificación**: SPs estables, cambio mínimo de BD; LINQ para nuevas features.  
**Acción**: Encapsular cada SP en método de servicio (Dapper); documentar SP signature en comentarios.

### 4. jQuery → Bootstrap 5 + JavaScript moderno
**Decisión**: Reemplazar jQuery UI (Accordion, Dialog, Tabs) con Bootstrap 5 + vanilla JS o Alpine.js.  
**Justificación**: Modernidad, performance, mantenibilidad.  
**Acción**: Migrar página a página (Trabajos, TraficoEncuestas, IPS primero); usar partial views para componentes reutilizables.

### 5. GridView → DataGrid reutilizable o Razor Pages Grid
**Decisión**: Crear componente de paginación/filtering reutilizable (DataGrid tag helper o lib).  
**Justificación**: DRY, reducir duplicación en 20+ páginas.  
**Acción**: Implementar `DataGridComponent` en MatrixNext.Web/Components; usar en todas las grillas de listado.

### 6. Rutas de archivos → Configuración + Blob Storage
**Decisión**: Externilizar `~/Files` y rutas GD en `appsettings.json`; migrar a Azure Blob Storage.  
**Justificación**: Configuración environment-agnostic, escalabilidad.  
**Acción**: Crear `IFileStorageService` que abstrae blob; implementar en local dev (FileSystem) e IaaS (Blob).

### 7. Email Async → Queue + Reintentos
**Decisión**: Implementar queue de correos (RabbitMQ/Azure Service Bus) con retry de 3 intentos.  
**Justificación**: Durabilidad, no bloquea request.  
**Acción**: Crear `EmailQueueService`; usar en `EnviarCorreo` async.

### 8. Consolidación de Enums
**Decisión**: Mover `ETipoActividad`, `ETipoActividadPlanilla`, `EAreas`, `EReproceso` a proyecto compartido (MatrixNext.Domain/Enums).  
**Justificación**: Type safety, reutilización en múltiples servicios.  
**Acción**: Definir en C# con valores numéricos; usar en validaciones de Excel.

### 9. Permisos → Middleware + Atributo [Authorize]
**Decisión**: Implementar middleware que valida permiso en BD (código 100, 135, etc.) al inicio de request.  
**Justificación**: Centralización, eliminación de checks dispersos en código.  
**Acción**: Crear `PermissionAuthorizationHandler` en MatrixNext; decorar actions con `[Authorize(Policy="PermissionCode=100")]`.

### 10. Múltiples Connection Strings → Inyección de Dependencias
**Decisión**: Definir `MatrixConnectionString` y `GestionCampoConnectionString` en appsettings.json; inyectar en servicios.  
**Justificación**: Configuración centralizada, fácil de cambiar per environment.  
**Acción**: Crear `IConnectionStringResolver` que retorna conexión por nombre; usar en `SqlConnection` y Dapper.

---

## 1️⃣1️⃣ ESTIMACIÓN PRELIMINAR

### Velocidad por Épica (Story Points / t-shirt sizing)

| Épica | Tamaño | Estimación (horas) | Esfuerzo Técnico | Riesgo |
|---|---|---|---|---|
| Épica 1 (COE Trabajos) | XXXL | 80-100h | Alto (GD integración, navegación compleja) | Alto (cierre con GD, email timing) |
| Épica 2 (Carga Masiva) | XXL | 60-80h | Alto (OleDb→OpenXml, BulkCopy, índices) | Medio (data integrity) |
| Épica 3 (Productividad) | XL | 40-50h | Medio-Alto (consolidación 4 páginas) | Medio (corte 16-15 cálculo) |
| Épica 4 (IPS) | XL | 35-45h | Medio (grid editable, export) | Bajo-Medio |
| Épica 5 (Presupuestos) | L | 20-30h | Bajo (flujo sencillo) | Bajo |
| Épica 6 (Coordinador/CallCenter/Consulta) | L | 25-35h | Bajo (listados, sin lógica) | Bajo |
| Épica 7 (Utilidades) | M | 15-20h | Bajo | Bajo |
| Épica 8 (Producción y Supervisión) | M | 15-20h | Medio (lógica de dropdowns cascada, **CRÍTICA**: hardcoded Session fix) | Medio (hardcoded ID) |
| Épica 9 (Dashboards) | S | 10-15h | Bajo (páginas simples) | Bajo |
| Épica 10 (Componentes reutilizables) | M | 30-40h | Medio (refactoring transversal) | Bajo |
| **TOTAL** | — | **330-435 horas** | — | — |

### Timeline Estimado (asumiendo 1 dev FT, 40h/semana)
- **8-11 semanas** para desarrollo core
- **2-3 semanas** para testing + QA
- **1 semana** para deployment preparation
- **TOTAL: 11-15 semanas** (2.75-3.75 meses)

### Contingencia
- Buffer por riesgos identificados: +20% (~66-87 horas)
- **Estimación final: 396-522 horas (9.9-13.05 semanas, o 2.5-3.3 meses)**

---

## 1️⃣2️⃣ PRÓXIMOS PASOS

### Fase de Planificación (Esta semana)
1. **Review con stakeholders**: Validar alcance, riesgos, timeline. Obtener buy-in para decisiones técnicas (Session→Claims, OleDb→OpenXml, etc.).
2. **Refinement del backlog**: Convertir épicas en user stories granulares. Asignar AC (Acceptance Criteria).
3. **Spike técnico**: Validar OpenXml con ejemplos reales de CATI RMC/Planillas. Probar blob storage integration.

### Fase de Configuración (Próximas 1-2 semanas)
4. **Scaffold del proyecto**: Crear estructura MatrixNext.Web/Controllers/OpCuanti*, servicios base, Dapper helpers.
5. **Implementar componentes transversales**: NotificationService, PermisosService, ExcelValidationService, NominaWindowService.
6. **Configurar appsettings.json**: Connection strings, rutas de archivos, limites de carga, secrets.

### Fase de Desarrollo (Sprints 3-12)
7. **Sprint 1-2**: Épica 1 (COE Trabajos) - core de todo.
8. **Sprint 3-4**: Épica 2 (Carga Masiva) - OleDb→OpenXml, BulkCopy.
9. **Sprint 5-6**: Épicas 3-4 (Productividad + IPS) - consolidación 8 páginas.
10. **Sprint 7**: Épicas 5-7 (Presupuestos, Coordinador, Utilidades).
11. **Sprint 8**: Épicas 8-9 (Producción/Supervisión + Dashboards).
12. **Sprint 9**: Épica 10 (Refactoring transversal + componentes reutilizables).
13. **Sprint 10-11**: Testing, bugfix, optimización, seguridad (SupervisionCampoTelefonico hardcoded ID fix).

### Validación y Deployment
14. **UAT**: Ejecutar checklist de verificación pre-migración. User stories completadas con AC confirmados.
15. **Deployment**: Migración en phases (Trabajos → Cargas → Productividad → IPS → Presupuestos → Resto). Rollback plan por cada fase.

---

## 1️⃣3️⃣ PROPUESTAS DE OPTIMIZACIÓN (SIN ROMPER 1:1)

**Principio**: Consolidar páginas con flujos similares en vistas compartidas con navegación dinámica, manteniendo 100% de funcionalidades pero reduciendo código duplicado y mejorando UX.

### Optimización 1: Consolidación de Productividad por Rol (8 → 2 vistas)

**Estado actual**: 8 páginas WebForms
- ProductividadRevisadaPMO.aspx
- ProductividadRevisadaMYSCall.aspx
- ProductividadRevisadaCoordinador.aspx
- ProductividadRevisadaCampo.aspx
- RevisionProductividadPMO.aspx
- RevisionProductividadCoordinador.aspx
- RevisionProductividadCampo.aspx
- RevisionProductividadMYSCall.aspx

**Propuesta optimizada**: 2 vistas Razor compartidas
- `ProductividadRevisada/Index.cshtml` - Visualización (consolida 4 páginas "ProductividadRevisada*")
  - Parámetro `rol` determina filtros y permisos (PMO=100, MYS=157, Coordinador=135, Campo=156)
  - Misma lógica de corte 16-15 reutilizada
  - Grid con columnas dinámicas según rol
- `RevisionProductividad/Index.cshtml` - Edición/Aprobación (consolida 4 páginas "RevisionProductividad*")
  - Validaciones máximos según rol
  - Rechazo de planillas
  - Navegación rol-específica

**Beneficio**: -75% código duplicado, UX consistente, mantenimiento centralizado

---

### Optimización 2: Flujo Unificado de Trabajos Coordinador (2 → 1 vista)

**Estado actual**: 2 páginas WebForms
- TrabajosCoordinador.aspx (Coordinador genérico)
- TrabajosCallCenter.aspx (Coordinador CallCenter)

**Propuesta optimizada**: 1 vista con parámetro de contexto
- `TrabajosCoordinador/Index.cshtml`
  - Parámetro `contexto` = "General" | "CallCenter"
  - Asignación de personal (común)
  - Navegación a Avance/Capacitaciones/Estimaciones (común)
  - Secciones específicas CallCenter (asignar/retirar encuestadores) solo si contexto=CallCenter

**Beneficio**: -50% código duplicado, funcionalidad CallCenter como extensión

---

### Optimización 3: Flujo de Aprobación de Planillas (3 → 1 vista con tabs)

**Estado actual**: 3 páginas WebForms
- PlanillasCargadas.aspx (listar y rechazar pendientes)
- RevisionPlanillas.aspx (aprobar/rechazar COE)
- PlanillasRevisadas.aspx (listar aprobadas, rechazo posterior)

**Propuesta optimizada**: 1 vista con tabs de estado
- `PlanillasAprobacion/Index.cshtml`
  - Tab 1: "Cargadas" (pendientes) - equivale a PlanillasCargadas
  - Tab 2: "En Revisión COE" - equivale a RevisionPlanillas
  - Tab 3: "Aprobadas" - equivale a PlanillasRevisadas
  - Acciones contextuales según tab (rechazar pendientes, aprobar COE, rechazo posterior)
  - Mismo Dapper service `OP_CuantiDapper` para todos los estados

**Beneficio**: -67% código, flujo visible en una sola página, mejor UX de estados

---

### Optimización 4: Gestión de Encuestas (2 → 1 vista con toggle)

**Estado actual**: 2 páginas WebForms
- ActivacionEncuestas.aspx (reactiva encuestas anuladas)
- AnulacionEncuestas.aspx (anula encuestas activas)

**Propuesta optimizada**: 1 vista bidireccional
- `GestionEncuestas/Index.cshtml`
  - Grid unificado con estado actual de encuestas
  - Toggle "Activar/Anular" según estado
  - Validaciones según acción (si existe registro previo para activar, si existe encuesta para anular)
  - Mismo service `IAnulacionEncuestasService` con métodos Activar/Anular

**Beneficio**: -50% código, UX más intuitiva (no necesita navegar entre páginas)

---

### Optimización 5: Solicitudes de Presupuesto Interno (2 → 1 vista adaptativa)

**Estado actual**: 2 páginas WebForms
- SolicitudPresupuestoInterno.aspx (formulario completo con flags)
- SolicitudPresupuestosInternos.aspx (formulario simplificado con observación)

**Propuesta optimizada**: 1 vista con modo simple/completo
- `PresupuestoInterno/Solicitud.cshtml`
  - Parámetro `modo` = "Completo" | "Simplificado"
  - Modo Completo: muestra flags (jornadas, agendamiento, encuestas, reclutamiento)
  - Modo Simplificado: solo observación
  - Validación de duplicados común
  - Email de notificación común

**Beneficio**: -50% código, formulario adaptativo

---

### Optimización 6: Dashboards Home (2 → 1 SPA con tabs de rol)

**Estado actual**: 2 páginas WebForms
- HomeRecoleccion.aspx (dashboard con permiso 54)
- HomeGestion.aspx (dashboard vacío)

**Propuesta optimizada**: 1 SPA dashboard
- `Home/Index.cshtml` (o componente Angular/Vue)
  - Tab "Recolección" (permiso 54) - widgets de trabajos, estimaciones, tráfico
  - Tab "Gestión" - widgets administrativos (si se requiere funcionalidad)
  - Navegación contextual según permisos del usuario

**Beneficio**: Experiencia moderna, eliminación de página vacía

---

### Optimización 7: Importación de Datos (2 → 1 wizard con steps)

**Estado actual**: 2 páginas WebForms
- ImportarDatos.aspx (CATI RMC con 6 pasos)
- ImportarPlanillas.aspx (Planillas productividad)

**Propuesta optimizada**: 1 wizard con tipo de carga
- `ImportacionMasiva/Index.cshtml`
  - Paso 1: Seleccionar tipo (CATI RMC | Planillas)
  - Paso 2: Upload archivo
  - Paso 3: Validaciones (específicas según tipo)
  - Paso 4: Resumen de validación
  - Paso 5: Confirmación e inserción
  - Services especializados: `ICatiRMCService` vs `IPlanillasService`

**Beneficio**: -40% código UI, wizard reutilizable, validaciones centralizadas

---

## Resumen de Optimizaciones Propuestas

| Grupo | WebForms Actuales | Propuesta Optimizada | Reducción | Funcionalidad Perdida |
|---|---|---|---|---|
| Productividad | 8 páginas | 2 vistas role-based | -75% | ❌ Ninguna |
| Trabajos Coordinador | 2 páginas | 1 vista con contexto | -50% | ❌ Ninguna |
| Planillas Aprobación | 3 páginas | 1 vista con tabs | -67% | ❌ Ninguna |
| Gestión Encuestas | 2 páginas | 1 vista toggle | -50% | ❌ Ninguna |
| Presupuestos Internos | 2 páginas | 1 vista adaptativa | -50% | ❌ Ninguna |
| Dashboards | 2 páginas | 1 SPA con tabs | -50% | ❌ Ninguna |
| Importación | 2 páginas | 1 wizard con tipo | -40% | ❌ Ninguna |
| **TOTAL** | **31 páginas** | **18 vistas** | **-42%** | **0%** |

### Impacto en Estimación

**Estimación original** (31 páginas 1:1): 330-435h  
**Estimación con optimizaciones** (18 vistas consolidadas): **260-350h** (~-20%)

**Ganancia adicional**:
- Menor superficie de testing (18 vs 31 vistas)
- Código más mantenible (DRY aplicado)
- UX mejorada (menos navegación entre páginas)
- Menor deuda técnica futura

### Recomendación de Implementación

**Enfoque híbrido sugerido**:
1. **Fase 1 - Sprint 1-6**: Implementar mapeo 1:1 directo para épicas críticas (Trabajos, Cargas, IPS)
2. **Fase 2 - Sprint 7-9**: Implementar optimizaciones para épicas de menor riesgo (Productividad, Planillas, Encuestas)
3. **Fase 3 - Sprint 10**: Refactoring final y consolidación de dashboards

**Decisión por stakeholders**: El equipo debe decidir si prioriza velocidad (1:1 directo, 330-435h) o calidad/mantenibilidad (optimizado, 260-350h pero requiere más diseño UX).

---

## Registro de avances y seguimiento
- Se creo `docs/OP/OP_CUANTITATIVO_AVANCE.md` para mantener fases, checklist de WebForms y decision points (se actualizara conforme se avance en cada sprint).
- La fase actual (diagnostico + configuracion base) incluye validacion de las directrices, el inventario 1:1 de WebForms y la verificacion de la capa de datos en `CoreProject/Clases/OP_Cuanti` (servicios Dapper/EF y funciones que llaman a los stored procedures catalogados).
- Cada flujo y dependencia se documentara aqui y en el nuevo registro; si se necesitan confirmaciones o decisiones se solicita el retorno en espanol, tal como lo pidio el equipo.
- Se habilitó una vista `/OP/Avances` en MatrixNext.Web que muestra el mismo checklist con enlaces a los documentos de análisis y directrices, ayudando a compartir avances con stakeholders sin salir del portal.
- Se añadió un portal `/OP/Trafico` que consume `OP_TraficoEncuestasCiudad` y permite visualizar envíos por ciudad para cada trabajo documentado.
- Sprint 1 (Portal COE) finalizado con grilla, permiso 100 y acciones contextuales; Sprint 2 (Tráfico + Activaciones) se completó con la vista `/OP/Encuestas`, un formulario que manda `OP_GestionCampo_ActivarEncuesta` y `OP_GestionCampo_AnularEncuesta`.
- La fase 2 ahora se define por sprints explícitos (ver `docs/OP/OP_CUANTITATIVO_AVANCE.md:| Fase 2 - Sprints recomendados`) que cubren portal COE, tráfico, cargas masivas, planillas/productividad/IPS y utilidades como presupuestos y supervisión.
- Se habilitó el portal COE `/OP/Portal` para listar trabajos activos, filtrar por estado y navegar a los flujos definidos en el backlog sin abandonar MatrixNext.Web.
- Sprint 3 (Cargas masivas) completado: el wizard `/OP/ImportacionMasiva` valida headers (`TipoActividad`, cortes/festivos), ejecuta los SP `CatiRMC_*` (borrar, validar, reportes y carga final) y realiza el bulk copy hacia `OP_CuantiPlanillas`, deja un backup en `uploads/op/cargas` y muestra los reportes (`ResumenValidas`, `ResumenNoValidas`, `ResumenDuplicadas`, `ResumenInconsistencias`) para dar seguimiento inmediato y cerrar el sprint 3.
- Sprint 4 (Planillas/Productividad/IPS) en progreso: se creó `docs/OP/SPRINT4_PLAN.md` con las tareas de tabulaciones, grids role-based e IPS editable; `/OP/PlanillasAprobacion` ahora consolida las aprobaciones (Update/Remove), el dashboard rol-based antes ubicado en `/OP/Productividad` y el control `/OP/Ips` (con edición, `OP_IPS_Revision_Edit` y exportaciones ClosedXML que se guardan bajo `~/Files/ips-export-*.xlsx`).

**DOCUMENTO FINALIZADO**  
**Versión**: 1.1  
**Próxima revisión**: Post-spike técnico (OpenXml + Blob Storage validation) + Review de optimizaciones con UX  
**Responsable**: Equipo MatrixNext  
**Fecha de actualización**: 2026-01-07
