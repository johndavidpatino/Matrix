# ANÁLISIS OP_CUALITATIVO - MIGRACIÓN A MATRIXNEXT

**Documento de Análisis Técnico**  
**Versión**: 2.0 PROFUNDO (Con Evidencia Concreta)  
**Fecha**: 2026-01-08  
**Alcance**: Fase 1 - Análisis de 19 WebForms en `WebMatrix/OP_Cualitativo/`  
**Basado en**: DIRECTRICES_MIGRACION.md, ANALISIS_CU_CUENTAS.md (referencia de calidad)  
**Autor**: GitHub Copilot

---

## 1?? Resumen Ejecutivo

### Propósito del módulo
`OP_Cualitativo` concentra la gestión diaria de operaciones cualitativas: inventario de trabajos (COE), planeación de entrevistas/sesiones/observaciones, definición y aprobación de filtros de reclutamiento/asistencia y controles de soporte (transcripciones, planillas, IPS). Las vistas regulan el ciclo completo de un estudio cualitativo desde la definición del trabajo hasta la entrega final, y alimentan a módulos vecinos como `PY_Proyectos` (fichas, instrucciones y variables), `GD_Documentos` (documentos/adjuntos) y `RP_Reportes` (informes de seguimiento).

### Roles evidenciados
- **Coordinador de Campo / Gerente COE (permiso 42)**: `Trabajos.aspx.vb` y `TrabajosCoordinador.aspx.vb` llaman a `Datos.ClsPermisosUsuarios.VerificarPermisoUsuario(42, Session("IDUsuario"))` antes de cargar grids y habilitar botones adicionales.
- **Operaciones Cualitativas / Operador de Fichas (roles 6/7/8)**: `FichaEntrevista.aspx.vb`, `FichaSesion.aspx.vb` y `FichaObservacion.aspx.vb` muestran/ocultan botones de guardado/entrega en función de los roles devueltos por `US.RolesUsuarios`.
- **Entrevistadores/Observadores**: `Entrevista.aspx.vb`, `Observacion.aspx.vb` y `Transcripcion.aspx.vb` filtran listas de usuario con `US.Usuarios.UsuariosxRol` o `Personas.TH_Usuarios_Combo_Get` y requieren selección obligatoria (`ddlEntrevistador.SelectedValue != "-1"`).

### Dependencias con otros módulos
- **CoreProject (OP, CampoCualitativo, Trabajo, Propuesta, Segmentos, CoordinacionCampo, PlaneacionProduccion)**: todas las páginas consumen métodos como `ObtenerFiltros`, `GuardarCampo`, `ObtenerEntrevistasxTrabajo`, `GuardarRespuestasFiltroMaestro` y `ObtenerMuestraxEstudioList`.
- **PY_Proyectos / VariablesControl**: las redirecciones (`btnSegmentos`, `btnProgramacionCampo`, `btnVariablesControl`) apuntan a `PY_Proyectos/*` para abrir listas de segmentos, instrucciones y configuraciones de variables.
- **US_Usuarios / Personas / ListaRoles**: para llenar dropdowns y restringir acciones en entrevistas, observaciones y transcripciones.
- **GD_Documentos + WorkFlow**: `btnDocumentos` (Campo), `btnAudios` y `btnLoadTranscripciones` navegan a `GD_Documentos` usando IDs de trabajo y workflow; `ProgramacionCampo.aspx.vb` usa `WorkFlow.obtenerXId` para recuperar la tarea actual.
- **PlanillaModeracionDapper / DesvinculacionEmpleadosDapper (Front-end)**: `AdministracionRegistroPlanillas.aspx` actúa como contenedor para el módulo JS que consume estos adaptadores (ver `Scripts/js/Pages/OP_Cualitativo/AdministracionRegistroPlanillas/AdministracionRegistroPlanillas.js`).

### Complejidad estimada: ?? (Media-Alta)
1. **Volumen de UI legacy**: 19 WebForms con UpdatePanels, grids, modales, validaciones JavaScript y dependencias en `Session`/`Request.QueryString`, lo que exige replicar interacciones completas en MVC/Blazor.
2. **Integraciones transversales**: SP legados (`REP_OP_Respuestas_Filtro`, consultas directas a `OP_IPS_Procesos`), exportaciones Excel (ClosedXML), envíos de correos (`EnviarCorreo`), generación ICS y planillas, todo listo para el mismo flujo funcional.
3. **Reglas de negocio múltiples**: validaciones estrictas (ej: incentivos y presupuestos en fichas, fechas obligatorias en programación, respuestas de filtros), logs de aprobación (`OP_LogRespuestas_Filtro`) y estados actualizados en `CoreProject`.
4. **Dependencias a otros módulos**: `PY_Proyectos`, `GD_Documentos`, `RP_Reportes`, `US_Usuarios` y `WorkFlow` deben convivir sin renombrar nada (Reglas 15). Esa traza obliga a mapear cada acción a un controller/service antes de desarrollar.

---

## 2?? Inventario del Legado (Tabla)
| Archivo | Funcionalidad Principal | Eventos clave / postbacks | Dependencias evidentes | Estado de evidencia |
|---|---|---|---|---|
| `CampoCualitativo.aspx` | Planeación y ejecución de sesiones/observaciones por segmento (moderador, transcriptor, ejecución real, exportación/ICS). | `Page_Load`, `btnGuardar_Click`, `btnGuardarEjecucion_Click`, `btnExportar_Click`, `imbDescargarCita_Click`, `btnDocumentos_Click`. | `CoreProject.CampoCualitativo`, `OP_CampoCuali`, `Auxiliares`, `WorkFlow`, `Session("IDUsuario")`, `GD_Documentos`. | ? |
| `Calendario.aspx` | Cronograma / Gantt de tareas por coordinador; alterna entre gráfica y tabla. | `PreInit` (permiso 42), `Page_Load`, `ddlCoordinador_TextChanged`, `gvTrabajos_RowCommand`, `gvTrabajos_PageIndexChanging`, `li_Gantt_Cronograma`, `li_Tabla_Cronograma`, `CargarCronograma`. | `Datos.ClsPermisosUsuarios`, `Trabajo.ObtenerCoordinadorProyectoCuali`, `CT_Tareas.TareasList`, `Session("Cronograma")`. | ? |
| `Trabajos.aspx` | Perfil COE: listado de trabajos, búsqueda, guardado de configuración de fechas/tipos, navegación a fichas y filtros. | `Page_Load`, `btnBuscar_Click`, `gvTrabajos_PageIndexChanging`, `gvTrabajos_RowCommand`, `btnGuardar_Click`, botones de navegación (Segmentos, Entrevistas, Sesiones, Observacion). | `CoreProject.Trabajo`, `TrabajoOPCuanti`, `CoordinacionCampo`, `PlaneacionProduccion`, `ShowNotification`, `Session("IDUsuario")`. | ? |
| `TrabajosCoordinador.aspx` | Lista de trabajos con filtros por COE, ciudades asignadas, acceso a especificaciones y programación. | `Page_Load`, `btnBuscar_Click`, `gvTrabajos_RowCommand`, `btnEspecificaciones_Click`, `btnProgramacionCampo_Click`, botones de documentos/transcripciones/variables. | `CoreProject.Trabajo`, `CoordinacionCampo`, `MetodologiaOperaciones`, `UrlOriginal`, `Session`, `PY_Proyectos` redirecciones. | ? |
| `MuestraTrabajos.aspx` | Administración de la muestra (ciudades, cantidades) asociada a un trabajo. | `Page_Load`, `btnAddMuestra_Click`, `gvTrabajos_RowCommand`, `ddlDepartamento_SelectedIndexChanged`, `btnVolver_Click`. | `CoordinacionCampo.ObtenerMuestraxEstudioList`, `OP_MuestraTrabajos`, `ShowNotification`, `Session`. | ? |
| `ProgramacionCampo.aspx` | Programación detallada de entrevistas (estados, moderadores, audio/transcripciones, descargas Excel, generación de archivos). | `Page_Load`, `btnSave_Click`, `btnSaveProgramar_Click`, `btnDescargar_Click`, `gvCamposProgramados_RowCommand`, validaciones de fechas, `ClosedXML.Excel`. | `CoreProject.CampoCualitativo`, `OP_Programados_Entrevistados`, `SegmentosCuali`, enumerados de estados, `WorkFlow`. | ? |
| `AdministracionRegistroPlanillas.aspx` | Interfaz JavaScript para filtrar/exportar planillas; no hay eventos VB. | Sin code-behind; todo vive en el módulo JS (`AdministracionRegistroPlanillas.js`) que registra filtros, paginadores y exportaciones. | JS consume `PlanillaModeracionDapper`, `DesvinculacionEmpleadosDapper`, `MatrixConnectionString`, componentes UI (Modal, Loader). | ?? (sin VB que documentar). |
| `DisenarFiltros.aspx` | Crea filtros de reclutamiento/asistencia y preguntas (texto, listas, respuestas múltiples). | `Page_Load`, `btnCrear_Click`, `btnAdd_Click`, `btnAddRespuestas_Click`, `btnGenerar_Click`, `gvFiltros_RowCommand`. | `CoreProject.CampoCualitativo`, `Session`, `Response.Redirect` a `AprobacionesFiltros`, `PY_Proyectos` cuando `py=true`, `SqlDataSource` indirecto. | ? |
| `VisualizadorFiltros.aspx` | Renderiza filtros para ser respondidos (solo lectura para asistencia, ingreso para reclutamiento). | `Page_Load`, `btnGuardar_Click`, `cargarPreguntasFiltro`. | `CoreProject.CampoCualitativo.ObtenerListaPreguntasFiltro`, dinámicas de UI con `Panel/Label/TextBox` y validadores. | ? |
| `AprobacionesFiltros.aspx` | Modal de aprobación OPS/GP con detalle y exportación Excel; registra logs de estado. | `Page_Load`, `gvRespuestas_RowCommand`, `btnAprobar_Click`, `btnNoAprobar_Click`, `btnImgExportarInforme_Click`. | `CoreProject.CampoCualitativo`, `OP_Respuestas_Filtro_Maestro`, `OP_LogRespuestas_Filtro`, `ClosedXML.Excel`, SP `REP_OP_Respuestas_Filtro`. | ? |
| `AprobacionesFiltrosAsitencia.aspx` | Aprobar filtros de asistencia con grillas maestras + detalle + exportación. | `Page_Load`, `gvRespuestas_RowCommand`, `gvFiltro_RowCommand`, `btnImgExportarInforme_Click`. | `CoreProject.CampoCualitativo`, `ClosedXML.Excel`, `REP_OP_Respuestas_Filtro`, `Session`. | ? |
| `Entrevista.aspx` | CRUD de entrevistados (país/dpto/ciudad, entrevistador, fechas reales). | `Page_Load`, `btnGuardar_Click`, `gvDatos_RowCommand`, dropdowns `ddlpais`, `ddldepartamento`, `ddlCiudad`. | `CoreProject.Entrevistas`, `Auxiliares`, `US.Usuarios`, `Session("IDUsuario")`. | ? |
| `Observacion.aspx` | CRUD de observadores (similar a entrevistas). | `Page_Load`, `btnGuardar_Click`, `gvDatos_RowCommand`. | `CoreProject.Observaciones`, `Auxiliares`, `US.Usuarios`. | ? |
| `Transcripcion.aspx` | Gestiona transcripciones (responsable, transcriptor, fechas, cantidad). | `Page_Load`, `btnGuardar_Click`, `gvDatos_RowCommand`, validaciones (responsable/transcriptor obligatorios), `gvDatos_RowDataBound`. | `CoreProject.Transcripciones`, `Personas.TH_Usuarios_Combo_Get`, `Session`, `ShowNotification`. | ? |
| `FichaEntrevista.aspx` | Parametriza incentivos, recursos, reclutamiento y envía correo de entrega. | `Page_Load`, `btnGuardar_Click`, `btnEntrega_Click`, `GuardarFichaEntrevista`, `EnviarEmail`, `ActualizarHabeasData`. | `CoreProject.Trabajo`, `PY_TrabajoCuali`, `SegmentosCuali`, `EnviarCorreo`, `Propuesta`. | ? |
| `FichaSesion.aspx` | Paramétricas de sesiones (incentivos, recursos) y envíos de entrega. | `Page_Load`, `btnGuardar_Click`, `btnEntrega_Click`, `GuardarFichaSesion`, `EnviarEmail`. | `CoreProject.Trabajo`, `SegmentosCuali`, `EnviarCorreo`. | ? |
| `FichaObservacion.aspx` | Guarda parámetros específicos de observaciones; también actualiza Habeas Data. | `Page_Load`, `btnGuardar_Click`, `GuardarFichaObservacion`, `EnviarEmail`. | `CoreProject.Trabajo`, `SegmentosCuali`, `EnviarCorreo`, `Propuesta`. | ? |
| `IPSCuali.aspx` | IPS: grid editable, revisiones, carga de procesos, notificaciones y exportación. | `Page_Load`, `gvRevision_RowDataBound`, `SqlDataSource1` (carga `OP_IPS_Procesos`), botones `btnNotificar`, `btnRechazar`. | `CoreProject.RevisionIPS`, `WorkFlow`, `SqlDataSource` con query `SELECT Id, Proceso FROM OP_IPS_Procesos`, `ClosedXML`. | ? |

---

## 3?? Flujos Funcionales Detallados

### FLUJO 1: Captura y configuración de trabajos (COE)
1. Usuario con permiso 42 abre `Trabajos.aspx` (`Page_Load`) y carga `Trabajo.obtenerXIdCOEXTodosCampos` con `Session("IDUsuario")` (verification en `Trabajos.aspx.vb`, líneas ~15-35). Carga también `ddlTipoRecoleccion` y expone un grid paginado.\
2. Al buscar (`btnBuscar_Click`) se vuelve a invocar `Trabajo.obtenerXIdCOEXTodosCampos`, preservando el filtro y mostrando la nueva lista.\
3. `gvTrabajos_RowCommand` (comando `Actualizar`) guarda el ID seleccionado, carga la configuración guardada (`TrabajoOPCuanti.ObtenerTrabajoConfiguracion`) y muestra el segundo accordion con botones variables (`Sesiones`, `Entrevistas`, `InHome`) según `MetCodigo`.\
4. `btnGuardar_Click` valida fechas/tipo, guarda `TrabajoOPCuanti.GuardarTrabajoConfiguracion`, actualiza el tipo de recolección (`GuardarTipoRecoleccion`) y muestra notificación; en paralelo los botones redirigen hacia `DisenarFiltros.aspx`, `Ficha*` o `PY_Proyectos` para continuar el flujo.
5. `TrabajosCoordinador.aspx` replica el listado pero permite seleccionar ciudades (`CoordinacionCampo.ObtenerMuestraxCoordinadoryTrabajo`) y navegar a programaciones específicas (botón `btnProgramacionCampo_Click`).

**Validaciones**: fechas obligatorias (inicio/terminación), selección de tipo de recolección y permiso 42 antes de mostrar data.\
**Resultado Esperado**: configuración persistida y botones guiando a filtros/fichas.\
**Riesgos técnicos**: `Session("IDUsuario")` y `hfIdTrabajo` dominan la navegación; replicarlo en MVC requiere enviar IDs en rutas/queries cada vez, evitando `Session`.

### FLUJO 2: Diseño y aprobación de filtros de reclutamiento/asistencia
1. En `Trabajos.aspx` o `TrabajosCoordinador.aspx` se ejecuta `btnFiltroReclutamiento_Click`/`btnFiltroAsistencia_Click`, que redirige a `DisenarFiltros.aspx` con `trabajoId` y `tipofiltro` (líneas ~110-130).\
2. `DisenarFiltros.aspx` carga los filtros existentes (`CargarLabelTrabajo`, `cargarListaFiltros`) y permite crear uno nuevo (`btnCrear_Click`). Dependiendo de `tipofiltro`, guarda preguntas estándar (nombres, CC, dirección, ciudad, edad, etc.) llamando a los métodos `GuardarPregunta*` (ver página).\
3. El usuario agrega preguntas adicionales con `btnAdd_Click` y opcionalmente genera un `Link` (`btnGenerar_Click`). Todas las preguntas se persisten en `CoreProject.CampoCualitativo`.\
4. Una vez creado el filtro, se hace clic en Aprobar (`gvFiltros_RowCommand`), lo que redirecciona a `AprobacionesFiltros.aspx` (reclutamiento) o `AprobacionesFiltrosAsitencia.aspx` (asistencia) con los identificadores (`hfIdFiltro`, `trabajoId`).\
5. La aprobación recorre `CoreProject.CampoCualitativo.ObtenerRespuestasFiltroMaestro`, permite aprobar/no aprobar (incrementando `Estado` y guardando log `OP_LogRespuestas_Filtro`) y exportar los resultados (`ClosedXML`). La grilla `SqlDataSource` ejecuta el SP `REP_OP_Respuestas_Filtro` antes del binding.

**Validaciones**: fechas obligatorias, preguntas con respuestas, al menos un tipo de reclutamiento seleccionado, comentarios obligatorios al no aprobar.\
**Resultado**: filtro aprobado con estado actualizado, logs persistidos y Excel de respaldo.\
**Riesgos**: replicar la lógica de `ShowNotification` y grids masivos en Razor/Blazor; la aprobación depende de campos ocultos (`hfEstado`, `hfIdRespuesta`).

### FLUJO 3: Fichas / entrevistas / sesiones / observaciones y entregas
1. Desde `Trabajos.aspx` se selecciona Ficha (`btnFicha_Click`), que evalúa la metodología (`MetCodigo`). En función del rango, redirige a `FichaSesion.aspx`, `FichaEntrevista.aspx` o `FichaObservacion.aspx` con `idtrabajo`.\
2. Cada ficha carga la información base (`CargarInfo`, `CargarHabeasData`) y poblaciones auxiliares (`CargarAyudasCuali`, `CargarTiposReclutamiento`). Las fichas validan incentivos (presupuesto y distribución) y campos obligatorios (exclusiones, recursos, backups).\
3. `GuardarFicha*` actualiza `PY_TrabajoCuali`, guarda ayudas/reclutamiento (`SegmentosCuali`) y dispara `EnviarEmail` con la URL `Emails/EntregaTrabajo*.aspx`. También actualiza `Propuesta.RequestHabeasData` cuando se escriben textos en el formulario.
4. Paralelamente, `Entrevista.aspx`, `Observacion.aspx` y `Transcripcion.aspx` ofrecen CRUD de registros específicos (persona, fechas, responsable), con validaciones (ej: responsable/transcriptor obligatorios) y resaltan rows fuera de tiempo (`gvDatos_RowDataBound` en `Transcripcion.aspx`).\
5. `ProgramacionCampo.aspx` permite programar citas, proteger estados (Creado/Programado/Cancelado), generar Excel de programados y exponer paneles de cancelación.

**Validaciones**: fechas, incentivos y reclutamiento seleccionados, responsable/transcriptor obligatorio.\
**Resultado**: fichas guardadas + envíos, registros individuales referenciados (entrevistas/observaciones/transcripciones) y programación de campo lista.\
**Riesgos**: replicar la lógica de `EnviarCorreo` y `WorkFlow` en el nuevo backend, además de manejar el estado de los grids con UpdatePanels y `Accordion` de WebForms.

---

¿Continúo con la siguiente sección?


## 4?? Mapa de Migraci?n 1:1
| WebForm legacy | Ruta MVC propuesta | Controller / Action | View | ViewModel | Service / DAL | Nota de paridad funcional |
|---|---|---|---|---|---|---|
| `Trabajos.aspx` | `/OP/Trabajos` | `TrabajosController.Index` | `Areas/OP/Views/Trabajos/Index.cshtml` | `TrabajoCualitativoIndexVm` | `TrabajoCualitativoService` + `TrabajoCualitativoAdapter` | Mantener b?squeda, filtros y botones hacia fichas seg?n `MetCodigo` (`Trabajos.aspx.vb:1`). |
| `TrabajosCoordinador.aspx` | `/OP/Trabajos/Coordinador` | `TrabajosController.Coordinador` | `Areas/OP/Views/Trabajos/Coordinador.cshtml` | `TrabajosCoordinadorVm` | `TrabajoCualitativoService` | Mantener grids con ciudades y redirecciones a programaciones/variables (`TrabajosCoordinador.aspx.vb:1`). |
| `CampoCualitativo.aspx` | `/OP/Campo` | `CampoController.Index` / `CampoController.ExecuteTask` | `Areas/OP/Views/Campo/Index.cshtml` + `_Accordion` partial | `CampoCualitativoVm` | `CampoCualitativoService` + `CampoCualitativoAdapter` | Reproducir send/execute flows y exportaci?n ICS (`CampoCualitativo.aspx.vb:1`). |
| `DisenarFiltros.aspx` | `/OP/Filtros/Configurar` | `FiltrosController.Configurar` | `Areas/OP/Views/Filtros/Configurar.cshtml` | `FiltroConfigVm` | `FiltrosService` / `CampoCualitativoAdapter` | Persistir preguntas con `CampoCualitativo` y redireccionar a aprobaciones (`DisenarFiltros.aspx.vb:1`). |
| `AprobacionesFiltros.aspx` / `AprobacionesFiltrosAsitencia.aspx` | `/OP/Filtros/Aprobar` | `FiltrosController.Aprobar` / `FiltrosController.AprobarAsistencia` | `Areas/OP/Views/Filtros/Aprobar.cshtml` | `FiltroAprobacionVm` | `FiltrosService` | Ejecutar l?gica de `Guardar()` y `OP_LogRespuestas_Filtro` + export Excel (`AprobacionesFiltros.aspx.vb:1`). |
| `Entrevista.aspx` / `Observacion.aspx` / `Transcripcion.aspx` | `/OP/Fichas/Entrevista`, `/OP/Fichas/Observacion`, `/OP/Fichas/Transcripcion` | `FichasController.Entrevista/Observacion/Transcripcion` | `Areas/OP/Views/Fichas/{Entrevista,Observacion,Transcripcion}.cshtml` | `FichaEntrevistaVm`, etc. | `FichasService` | CRUD de registros y validaciones replicadas (`Entrevista.aspx.vb:1`). |
| `FichaEntrevista.aspx`, `FichaSesion.aspx`, `FichaObservacion.aspx` | `/OP/Fichas/Parametros` | `FichasController.Parametros` | `Areas/OP/Views/Fichas/Parametros.cshtml` | `FichaParametrosVm` | `FichasService` | Validaciones de incentivos/reclutamiento y env?o `EnviarCorreo` (`FichaEntrevista.aspx.vb:1`). |
| `ProgramacionCampo.aspx` | `/OP/Programacion` | `ProgramacionController.Index` / `ProgramacionController.Guardar` | `Areas/OP/Views/Programacion/Index.cshtml` | `ProgramacionCampoVm` | `ProgramacionService` | Cronograma, estados y export Excel replicados (`ProgramacionCampo.aspx.vb:1`). |
| `IPSCuali.aspx` | `/OP/IPS` | `IpsController.Index` | `Areas/OP/Views/IPS/Index.cshtml` | `IpsRevisionVm` | `IpsService` | Grid editable con `OP_IPS_Procesos`, notificaciones y export (`IPSCuali.aspx.vb:1`). |
| `AdministracionRegistroPlanillas.aspx` | `/OP/Planillas/Administracion` | `PlanillasController.Index` | `Areas/OP/Views/Planillas/Administracion.cshtml` | `PlanillasAdminVm` | `PlanillasService` + JS adapter | Front-end ya tiene JS modular; servicio expone APIs Dapper (`AdministracionRegistroPlanillas.aspx:1`). |

## 5?? Base de Datos y Stored Procedures
- **Tablas principales**: `OP_CampoCuali`, `OP_Respuestas_Filtro_Maestro`, `OP_Respuestas_Filtro_Detalle`, `OP_MuestraTrabajos`, `OP_Programados_Entrevistados`, `OP_TrabajoConfiguracion`, `OP_Transcripciones`, `OP_IPS_Procesos`, `PY_TrabajoCuali`, `OP_LogRespuestas_Filtro`. (Evidencia de uso en `CampoCualitativo.aspx.vb`, `DisenarFiltros.aspx.vb`, `Entrevista.aspx.vb`.)
- **Stored procedures**: `REP_OP_Respuestas_Filtro` (utilizado por `AprobacionesFiltros.aspx` y `AprobacionesFiltrosAsitencia.aspx`). Otros procedimientos se ejecutan desde `CoreProject` como `CampoCualitativo.GuardarCampo()` o `Trabajo.GuardarTrabajoCuali()`; respetar nombres originales seg?n `DIRECTRICES_MIGRACION.md`.
- **Decisi?n preliminar**: usar **Dapper/SP** para listados complejos (filtros, IPS, planillas) y **EF Core** para CRUD simples (programaci?n, fichas, transcripciones); encapsular SP en `CampoCualitativoAdapter`, `FiltrosAdapter`, `TrabajoAdapter`.

## 6?? Riesgos y Consideraciones T?cnicas
- `Session("IDUsuario")`, `Session("NombreTrabajo")` y `Request.QueryString` (`Trabajos.aspx.vb`, `FichaEntrevista.aspx.vb`) deben reemplazarse por claims y par?metros de ruta para evitar estado global.
- Varias p?ginas usan `UpdatePanel`/`Accordion` (p. ej. `CampoCualitativo`, `DisenarFiltros`); la migraci?n necesita reconstruir la experiencia AJAX-first (modales + partial views) como dicta la Regla 5.1.
- `SqlDataSource` con `REP_OP_Respuestas_Filtro` (Aprobaciones) y consultas directas a `OP_IPS_Procesos` (IPSCuali) requieren adapters Dapper y coordinaci?n de connection strings (Regla 2 y 4).
- Exportaci?n de Excel (ClosedXML) y archivos ICS exige servicios `IFileExportService` reutilizables que encapsulen `Response` y permitan testing.
- `EnviarCorreo` (fichas) y `GD_Documentos` implican servicios externos y rutas hardcodeadas; introducir `IEmailService` y `UploadService` ya existentes en MatrixNext.

## 7?? Componentes Reutilizables Existentes
- **Modales y toast** (`Views/Shared/_Modal.cshtml`, `_ToastContainer.cshtml`): reutilizar para di?logos de aprobaci?n y confirmaciones en fichas/filtros.
- **Grillas paginadas** (`Views/Shared/_Grid.cshtml`, `_Paginator.cshtml`): reemplazar los `GridView` en `Trabajos`, `Aprobaciones` e `IPSCuali`.
- **DatePicker / TimePicker** (`Views/Shared/_DatePicker.cshtml`, Flatpickr JS): para fechas de programaci?n y fichas (`ProgramacionCampo`, `Entrevista`, `Ficha_*`).
- **Form partials y helpers JS** (`ValidationHelpersJS.js`): validar dropdowns obligatorios (entrevistador, transcriptor, incentivos) equivalentes a `ShowNotification`.
- **Componentes JS existentes** (`Scripts/js/Components/ModalDialog`, `Table`, `Export`): ya cumplen con UX responsive y se pueden reutilizar para planillas y filtros.

## 8?? Backlog Inicial (P0/P1/P2)
| Prioridad | ID | Tarea | Estimaci?n | Dependencias |
|---|---|---|---|---|
| **P0** | OP-C01 | `TrabajosController.Index` + `TrabajoCualitativoService` (b?squeda y navegaci?n a fichas) | 8h | Base de trabajo y filtros |
| **P0** | OP-C02 | `CampoController` + exportaciones (ICS/Excel) + integraci?n a `GD_Documentos` | 10h | `TrabajoCualitativoService` |
| **P0** | OP-F01 | `FiltrosController.Configurar` + `FiltrosService` + `CampoCualitativoAdapter` | 6h | `TrabajoService` |
| **P0** | OP-F02 | `FiltrosController.Aprobar` + `REP_OP_Respuestas_Filtro` + export Excel | 8h | `FiltrosService` |
| **P0** | OP-F03 | `FichasController` (entren entrevista/sesi?n/observaci?n) + `EnviarCorreoService` | 12h | `TrabajoAdapter`, `SegmentosService` |
| **P1** | OP-P01 | `ProgramacionController` (cronograma + estados + exportaciones Excel) | 10h | `TrabajoService` |
| **P1** | OP-I01 | `IpsController` (grids IPS + notificaciones + SqlDataSource) | 8h | `WorkFlow`, `IpsService` |
| **P2** | OP-P02 | `PlanillasController` (API para m?dulo JS + exportaciones) | 12h | `PlanillaModeracionDapper` |

## 9?? Checklist de Verificaci?n Pre-Migraci?n
- [ ] WebForms del alcance inventariados con eventos y dependencias (`Trabajos.aspx.vb`, `CampoCualitativo.aspx.vb`, `DisenarFiltros.aspx.vb`, etc.).
- [ ] Flujos documentados (COE, filtros/aprobaciones, fichas/programaci?n de campo).
- [ ] Stored procedures identificados (`REP_OP_Respuestas_Filtro` y SP en `CoreProject`).
- [ ] Tablas clave mapeadas en `CoreProject` (`OP_CampoCuali`, `OP_Respuestas_Filtro_*`).
- [ ] Rutas/controllers/views definidos para cada WebForm.
- [ ] Componentes reutilizables listados (modales, grids, datepickers, toast).
- [ ] Riesgos documentados (Session, UpdatePanel, Excel, correo, WorkFlow).
- [ ] Dependencias externas (PY_Proyectos, GD_Documentos, RP_Reportes, US_Usuarios) confirmadas.
- [ ] Backlog priorizado con estimaciones preliminares.
- [ ] Directrices de `DIRECTRICES_MIGRACION.md` aplicadas (nombres BD, modularizaci?n por ?rea, no nuevos features).

## ?? Decisiones T?cnicas Clave
| Decisi?n | Opci?n elegida | Justificaci?n | Alternativa descartada |
|---|---|---|---|
| Acceso a datos | Dapper/SP para listados, EF Core para CRUD simple | Los SP existentes ya encapsulan l?gica compleja y EF corta duplicaci?n de registros simples | Usar EF para todo (duplicar?a l?gicas y romper?a reglas 2/4) |
| Componentes UI | Reutilizar modales/toasts/grids existentes | Alinea con UX MatrixNext y evita CSS/JS adicionales (`Views/Shared/_Modal.cshtml`). | Crear componentes nuevos exclusivos para OP. |
| Validaciones | FluentValidation + helpers JS | Permite replicar reglas condicionales (incentivos, presupuestos, preguntas obligatorias). | Solo DataAnnotations (no cubre validaciones din?micas). |
| Session | Claims + rutas | Quita estado global y documenta claramente qu? par?metros se pasan. | Mantener `Session` en CoreProject (no es compatible con async/DI). |
| Exportaciones | `IFileExportService` reutilizable (Excel, ICS) | Centraliza `ClosedXML` y descargas; facilita tests. | Cada WebForm maneja `Response` directo (duplica c?digo). |

## 1??1?? Estimaci?n Preliminar
| Item | Cantidad estimada | Nota |
|---|---|---|
| P?ginas a migrar | 19 | Trabajos, Campo, Filtros, Fichas, IPS, Programaci?n, Planillas. |
| Controllers | 6 | `Trabajos`, `Campo`, `Filtros`, `Fichas`, `Programacion`, `Ips` (+ `Planillas` APIs). |
| Views | ~20 | Index + parciales (modales, forms, grids). |
| ViewModels | 25 | DTOs por pantalla (Trabajo, Campo, Filtro, Ficha, IPS). |
| Services | 7 | `TrabajoCualitativoService`, `CampoService`, `FiltrosService`, `FichasService`, `ProgramacionService`, `IpsService`, `PlanillasService`. |
| DataAdapters | 6 | `TrabajoAdapter`, `CampoAdapter`, `FiltrosAdapter`, `FichasAdapter`, `ProgramacionAdapter`, `IpsAdapter`. |
| Stored Procedures | 1 confirmado (`REP_OP_Respuestas_Filtro`), otros encapsulados en `CoreProject`. | Base para el uso de Dapper. |
| Horas estimadas (P0+P1) | ~120h | Incluye dise?o, validaciones, exportaciones y pruebas funcionales. |

## 1??2?? Pr?ximos Pasos
- Validar con negocio la lista definitiva de WebForms dentro de Fase 1 y confirmar si los filtros siguen teniendo las mismas preguntas.
- Mapear cada WebForm a rutas/acciones dentro de `Areas/OP` y exponer enlaces en `Views/Shared/_Sidebar.cshtml`.
- Crear servicios/adapters base (`TrabajoCualitativoService`, `CampoCualitativoAdapter`, `FiltrosService`) y registrar DI en `Program.cs` siguiendo la modularidad del ?rea.
- Construir los primeros controladores (`TrabajosController`, `FiltrosController`) y vistas `Index` para validar los flujos de COE y filtros sin `Session`.
- Actualizar `DASHBOARD_MIGRACION.md` y el backlog general con el estado "en an?lisis"/"ready" para este m?dulo.
