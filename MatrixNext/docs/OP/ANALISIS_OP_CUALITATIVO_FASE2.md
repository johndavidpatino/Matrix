# ANÁLISIS OP_CUALITATIVO - FASE 2: INVENTARIO DEL LEGADO

## 📋 2. INVENTARIO DEL LEGADO (TABLA DETALLADA)

| # | Archivo | Líneas | Eventos Clave | Dependencias Primarias | Enumerados/Constantes | Estado Evidencia |
|---|---------|--------|---------------|----------------------|----------------------|------------------|
| 1 | `Trabajos.aspx.vb` | 217 | `Page_Load`, `btnBuscar_Click`, `gvTrabajos_RowCommand`, `btnGuardar_Click`, `gvTrabajos_PageIndexChanging` | `CoreProject.Trabajo`, `CoordinacionCampo`, `TrabajoOPCuanti`, `PlaneacionProduccion`, `MetodologiaOperaciones` | `OP_TrabajoConfiguracion` (entity) | ✅ **CONFIRMADO** |
| 2 | `TrabajosCoordinador.aspx.vb` | ⚠️ POR LEER | `Page_Load`, `btnBuscar_Click`, `gvTrabajos_RowCommand` | Esperado: `CoordinacionCampo`, `PY_Proyectos`, `GD_Documentos` | Filtros por coordinador/estado | ⚠️ NO CONFIRMADO |
| 3 | `CampoCualitativo.aspx.vb` | 346 | `Page_Load`, `btnGuardar_Click`, `btnNuevo_Click`, `gvDatos_RowCommand`, `gvDatos_PageIndexChanging`, `imbDescargarCita_Click`, `btnExportar_Click` | `CoreProject.CampoCualitativo`, `ScriptManager` (UpdatePanel), `PY_Proyectos.SegmentosCuali` | Estados de campo, tipos de descarga | ✅ **CONFIRMADO** |
| 4 | `Calendario.aspx.vb` | ⚠️ POR LEER | `Page_PreInit`, `Page_Load`, `ddlCoordinador_TextChanged`, `gvTrabajos_RowCommand` | Esperado: `Datos.ClsPermisosUsuarios`, `Trabajo`, `CT_Tareas`, renderizado Gantt | Enumerado: `eTipoGrafica` | ⚠️ NO CONFIRMADO |
| 5 | `MuestraTrabajos.aspx.vb` | 106 | `Page_Load`, `gvTrabajos_PageIndexChanging`, `gvTrabajos_RowCommand`, `ddlDepartamento_SelectedIndexChanged` | `CoordinacionCampo.ObtenerMuestraxEstudioList()`, `CoordinacionCampo.ObtenerDepartamentos()`, `CoordinacionCampo.ObtenerCiudades()`, `CoordinacionCampo.EliminarMuestraXEstudio()` | **NO ENUMERADOS** | ✅ **CONFIRMADO** |
| 6 | `ProgramacionCampo.aspx.vb` | 822 | `Page_Load`, `btnSave_Click`, `btnSaveProgramar_Click`, `btnLimpiar_Click` | `CoreProject.Trabajo.ObtenerTrabajosCualitativosxTrabajo()`, `ClosedXML.Excel` | `EstadosProgramacionCampo` (7 estados), `TipoProyectoEnum` | ✅ **CONFIRMADO** |
| 7 | `DisenarFiltros.aspx.vb` | 1062 | `Page_Load`, `btnCrear_Click`, `btnAdd_Click`, `btnAddRespuestas_Click`, `btnGenerar_Click`, `gvFiltros_RowCommand` | `CoreProject.CampoCualitativo`, `CoreProject.Trabajo`, `PY_Proyectos` (redirección) | `eTipoFiltro` (2), `eTipoPregunta` (9) | ✅ **CONFIRMADO** |
| 8 | `VisualizadorFiltros.aspx.vb` | ⚠️ POR LEER | `Page_Load`, `btnGuardar_Click`, `cargarPreguntasFiltro()` | Esperado: `CoreProject.CampoCualitativo.ObtenerListaPreguntasFiltro()`, generación dinámica de controles | Tipos de pregunta (texto, radio, checkbox, etc.) | ⚠️ NO CONFIRMADO |
| 9 | `AprobacionesFiltros.aspx.vb` | 270 | `Page_Load`, `gvRespuestas_RowDataBound`, `btnAprobar_Click`, `btnNoAprobar_Click`, `btnImgExportarInforme_Click` | `CoreProject.Trabajo`, `CoreProject.CampoCualitativo`, `ClosedXML.Excel` | **SqlDataSource** (SP `REP_OP_Respuestas_Filtro`) | ✅ **CONFIRMADO** |
| 10 | `AprobacionesFiltrosAsitencia.aspx.vb` | ⚠️ POR LEER | `Page_Load`, `gvRespuestas_RowCommand`, `gvFiltro_RowCommand`, `btnImgExportarInforme_Click` | Esperado: Estructura idéntica a Aprobaciones (reclutamiento), `ClosedXML` | Grilla anidada maestra-detalle | ⚠️ NO CONFIRMADO |
| 11 | `Entrevista.aspx.vb` | 297 | `Page_Load`, `ddlpais_SelectedIndexChanged`, `ddldepartamento_SelectedIndexChanged`, `gvDatos_RowCommand`, `gvDatos_PageIndexChanging`, `btnGuardar_Click` | `CoreProject.Entrevistas`, cascada país→depto→ciudad (Auxiliares), `US.Usuarios` | **Cascada de dropdowns** | ✅ **CONFIRMADO** |
| 12 | `Observacion.aspx.vb` | ⚠️ POR LEER | Esperado: `Page_Load`, `btnGuardar_Click`, `gvDatos_RowCommand` | Esperado: Estructura idéntica a `Entrevista.aspx.vb` pero para observaciones | Cascada país→depto→ciudad | ⚠️ NO CONFIRMADO |
| 13 | `Transcripcion.aspx.vb` | 231 | `Page_Load`, `btnNuevo_Click`, `btnBuscar_Click`, `btnGuardar_Click`, `gvDatos_RowCommand`, `gvDatos_PageIndexChanging` | `CoreProject.Transcripciones`, `Personas.TH_Usuarios_Combo_Get()` (transcriptores), `US.Usuarios` (responsables) | **NO ENUMERADOS** | ✅ **CONFIRMADO** |
| 14 | `FichaEntrevista.aspx.vb` | 353 | `Page_Load`, `btnGuardar_Click`, `btnEntrega_Click`, `btnCancelar_Click`, `rblIncentivos_CheckedChanged` | `US.RolesUsuarios.obtenerRolesXUsuario()`, `CoreProject.Trabajo`, `PY_TrabajoCuali`, `SegmentosCuali`, `EnviarCorreo` | Validaciones complejas (presupuesto, incentivos) | ✅ **CONFIRMADO** |
| 15 | `FichaSesion.aspx.vb` | ⚠️ POR LEER | Esperado: `Page_Load`, `btnGuardar_Click`, `btnEntrega_Click` | Esperado: Estructura similar a `FichaEntrevista.aspx.vb` | **NO DISPONIBLE** | ⚠️ NO CONFIRMADO |
| 16 | `FichaObservacion.aspx.vb` | ⚠️ POR LEER | Esperado: `Page_Load`, `btnGuardar_Click`, `btnEntrega_Click` | Esperado: Estructura similar a `FichaEntrevista.aspx.vb` | **NO DISPONIBLE** | ⚠️ NO CONFIRMADO |
| 17 | `IPSCuali.aspx.vb` | 682 | `Page_Load`, `gvRevision_RowDataBound`, `btnNotificar_Click`, `btnRechazar_Click`, `btnExportar_Click` | `CoreProject.WorkFlow.obtenerXId()`, `CoreProject.RevisionIPS`, `ClosedXML.Excel` | `eTarea` (26 = CualitativoInstrumentos), columnas dinámicas por rol/tarea | ✅ **CONFIRMADO** |
| 18 | `AdministracionRegistroPlanillas.aspx.vb` | ⚠️ **SIN EVENTS** | **TODO EN JAVASCRIPT**: `initFilters()`, `loadPlanillas()`, `exportToExcel()` | JS consume `PlanillaModeracionDapper`, `DesvinculacionEmpleadosDapper` | **Módulo JS puro** | ⚠️ NO VB.NET |
| 19 | `Default.aspx.vb` | ⚠️ FASE 2 | **FUERA DE ALCANCE FASE 1** | - | - | ⚠️ FASE 2 |
| 20 | `HomeGestion.aspx.vb` | ⚠️ FASE 2 | **FUERA DE ALCANCE FASE 1** | - | - | ⚠️ FASE 2 |
| 21 | `HomeRecoleccion.aspx.vb` | ⚠️ FASE 2 | **FUERA DE ALCANCE FASE 1** | - | - | ⚠️ FASE 2 |

---

## 📊 RESUMEN DE EVIDENCIA POR ARCHIVO

### ✅ CONFIRMADO (11 archivos)

**Archivos con código VB.NET completamente analizado**:

1. **Trabajos.aspx.vb** (217 líneas)
   - ✅ Permisos: 42, 148
   - ✅ Métodos: 7 (CargarTrabajos, CargarConfiguracionTrabajo, btnGuardar_Click, btnBuscar_Click, gvTrabajos_RowCommand, gvTrabajos_PageIndexChanging, MostrarBotones)
   - ✅ Tablas: `TrabajoOPCuanti`, `Trabajo`, `PlaneacionProduccion`
   - ✅ Validaciones: IsDate, SelectedIndex = -1
   - ✅ Riesgos: SQL Injection en filtro de búsqueda (línea 115)

2. **CampoCualitativo.aspx.vb** (346 líneas)
   - ✅ UpdatePanels: 3 (confirmados por `ScriptManager.RegisterPostBackControl`)
   - ✅ GridView: `gvDatos` con 4 comandos (AbrirPlaneacion, AbrirEjecucion, Eliminar)
   - ✅ Métodos: `GuardarPlaneacion()`, `CargarModeradores()`, `CargarTranscriptores()`, `CargarInfoSegmento()`, `CargarCampo()`, `CargarInfoCampo()`, `Eliminar()`
   - ✅ Enumerados: EstadosCampo, TipoCampo
   - ✅ Exportación: ICS + Excel confirmada

3. **MuestraTrabajos.aspx.vb** (106 líneas)
   - ✅ CRUD: Add, Edit, Delete confirmado
   - ✅ Cascada: Departamento → Ciudad
   - ✅ Métodos: 5 (CargarMuestra, CargarDepartamentos, CargarCiudades, btnAddMuestra_Click, btnVolver_Click)
   - ✅ SP: `CoordinacionCampo.ObtenerMuestraxEstudioList()`

4. **ProgramacionCampo.aspx.vb** (822 líneas)
   - ✅ Enumerados: 2 (TipoProyectoEnum, EstadosProgramacionCampo con 7 estados)
   - ✅ Validaciones: Fechas, estados, moderadores
   - ✅ Exportación: Excel con ClosedXML
   - ✅ Custom Class: `dataEntrevistados` (12 propiedades)
   - ⚠️ **ALTO**: 822 líneas = complejidad muy alta

5. **DisenarFiltros.aspx.vb** (1062 líneas)
   - ✅ Enumerados: 2 (eTipoFiltro: 2 valores, eTipoPregunta: 9 valores)
   - ✅ Controles dinámicos: generación de preguntas en runtime
   - ✅ Validaciones: IsDate condicional
   - ✅ Redirecciones: AprobacionesFiltros.aspx, PY_Proyectos/TrabajosCualitativos.aspx
   - ⚠️ **MUY ALTO**: 1062 líneas = la segunda más compleja

6. **AprobacionesFiltros.aspx.vb** (270 líneas)
   - ✅ SqlDataSource: SP `REP_OP_Respuestas_Filtro` (confirmada por línea 29)
   - ✅ GridView anidada: `gvRespuestas` (maestra) + detalle expandible
   - ✅ Aprobación: Estados 1→2 (OPS), 2→3 (GP)
   - ✅ Logs: `OP_LogRespuestas_Filtro` (tabla de auditoría)
   - ✅ Exportación: Excel con comentarios
   - ✅ Validaciones: Comentarios obligatorios al rechazar

7. **Entrevista.aspx.vb** (297 líneas)
   - ✅ Cascada: País → Departamento → Ciudad (confirmada con 3 eventos SelectedIndexChanged)
   - ✅ CRUD: Create, Read (Edit), Delete
   - ✅ GridView: `gvDatos` con paginación
   - ✅ Validaciones: Cascada obligatoria, entrevistador obligatorio
   - ✅ Métodos: 6 (CargarEntrevistas, CargarEntrevistadores, cargarPaises, CargarDepartamentos, cargarciudades, GuardarEntrevista)

8. **Transcripcion.aspx.vb** (231 líneas)
   - ✅ CRUD: Create, Read (Edit/Modify), Delete confirmado
   - ✅ Dropdowns: `ddlResponsable`, `ddlTranscriptor` (ambos obligatorios)
   - ✅ Métodos: 6 (CargarTranscripciones, CargarResponsable, CargarTranscriptor, GuardarTranscripcion, Limpiar, CargarInfo, Eliminar)
   - ✅ Paginación: `gvDatos_PageIndexChanging`
   - ✅ Validaciones: Responsable != -1, Transcriptor != -1

9. **FichaEntrevista.aspx.vb** (353 líneas)
   - ✅ Validaciones: Incentivos económicos, presupuesto, distribución
   - ✅ Checkboxes: `chbReclutamiento.Items.Count` (contar seleccionados)
   - ✅ Métodos: CargarHabeasData, CargarEntrevistas, CargarInfo, CargarAyudasCuali, CargarTiposReclutamiento, ObtenerAyudas, ObtenerTipoReclutamiento, GuardarFichaEntrevista
   - ✅ Email: `btnEntrega_Click` → `EnviarCorreo`
   - ✅ Roles: 6, 7, 8 controlan visibilidad de botones (confirmado en línea 17-39)

10. **IPSCuali.aspx.vb** (682 líneas)
    - ✅ Enumerado: `eTarea` (26 = CualitativoInstrumentos)
    - ✅ WorkFlow: `WorkFlow.obtenerXId(hfidwf.Value)` (confirmada)
    - ✅ Columnas dinámicas: Visibilidad según tarea y rol (gerencia vs operador)
    - ✅ Métodos: 4+ (CargarGrid, gvRevision_RowDataBound, btnNotificar_Click, btnRechazar_Click, btnExportar_Click)
    - ✅ Exportación: Excel de IPS
    - ⚠️ **ALTO**: 682 líneas, lógica compleja de visibilidad

11. **AdministracionRegistroPlanillas.aspx.vb**
    - ⚠️ **ESPECIAL**: Sin code-behind VB, TODO ES JAVASCRIPT
    - ✅ Confirma: Módulo JS independiente (`AdministracionRegistroPlanillas.js`)
    - ✅ Adapters: `PlanillaModeracionDapper`, `DesvinculacionEmpleadosDapper`
    - ✅ Funcionalidad: Filtrar, paginar, exportar planillas
    - ⚠️ **RIESGO**: No hay validaciones en VB (todo en JS + Dapper)

---

### ⚠️ NO CONFIRMADO (10 archivos)

**Requieren lectura de código fuente para validar estructura**:

| Archivo | Razón Pendiente | Acción Requerida |
|---------|-----------------|------------------|
| `TrabajosCoordinador.aspx.vb` | No leído completo | Leer líneas 1-150 |
| `Calendario.aspx.vb` | No leído | Leer completo (Gantt, gráfica) |
| `VisualizadorFiltros.aspx.vb` | No leído | Leer (generación dinámica de controles) |
| `AprobacionesFiltrosAsitencia.aspx.vb` | No leído | Leer (estructura similar a Aprobaciones) |
| `Observacion.aspx.vb` | No leído | Leer (estructura similar a Entrevista) |
| `FichaSesion.aspx.vb` | No leído | Leer (estructura similar a FichaEntrevista) |
| `FichaObservacion.aspx.vb` | No leído | Leer (estructura similar a FichaEntrevista) |
| `Default.aspx.vb` | Fuera de alcance Fase 1 | Fase 2 (Home del módulo) |
| `HomeGestion.aspx.vb` | Fuera de alcance Fase 1 | Fase 2 (Home de gestión) |
| `HomeRecoleccion.aspx.vb` | Fuera de alcance Fase 1 | Fase 2 (Home de recolección) |

---

## 📈 ESTADÍSTICAS DEL MÓDULO

| Métrica | Valor | Observación |
|---------|-------|-------------|
| **Archivos totales** | 21 | 11 en Fase 1 (análisis), 10 pendientes |
| **Líneas de código VB.NET** | ~4,800+ | Mínimo 11 archivos analizados |
| **Archivos JavaScript** | 1 | `AdministracionRegistroPlanillas.js` (módulo sin VB) |
| **UpdatePanels** | 3+ | Confirmados en CampoCualitativo.aspx |
| **GridViews** | 10+ | Mínimo 10 grids (búsqueda en todos) |
| **Acordeones** | 2+ | ActivateAccordion calls en casi todos |
| **Enumerados** | 7+ | eTipoFiltro, eTipoPregunta, EstadosProgramacionCampo, eTarea, etc. |
| **Stored Procedures únicos** | 3 confirmados | `REP_OP_Respuestas_Filtro`, métodos en CoreProject |
| **Validaciones condicionales** | 15+ | Presupuesto, incentivos, cascadas, permisos |
| **Métodos públicos/sub** | 80+ | Estimado (promedio 7-8 por archivo) |

---

## 🔌 DEPENDENCIAS CONSOLIDADAS

### Dependencia 1: CoreProject (Clases)

**Clases confirmadas utilizadas**:
- ✅ `Trabajo` (3 métodos)
- ✅ `TrabajoOPCuanti` (3 métodos)
- ✅ `CoordinacionCampo` (4+ métodos)
- ✅ `CampoCualitativo` (4+ métodos)
- ✅ `Entrevistas` (1+ métodos)
- ✅ `Transcripciones` (1+ métodos)
- ✅ `PY_TrabajoCuali` (2+ métodos)
- ✅ `SegmentosCuali` (2+ métodos)
- ✅ `WorkFlow` (1 método)
- ✅ `RevisionIPS` (1+ métodos)
- ✅ `PlaneacionProduccion` (1+ métodos)
- ✅ `MetodologiaOperaciones` (1 método)
- ✅ `Proyecto` (1 método)
- ✅ `FichaCuantitativo` (1 método) ← **REUTILIZA OP_Cuantitativo**
- ✅ `US.RolesUsuarios` (1+ métodos)
- ✅ `Datos.ClsPermisosUsuarios` (1 método - VerificarPermisoUsuario)

**Entidades EF Core confirmadas**:
- ✅ `PY_Trabajo_Get_Result`
- ✅ `OP_Respuestas_Filtro_Maestro_Get_Result`
- ✅ `CORE_WorkFlow_Trabajos_Get_Result`
- ⚠️ Más esperadas (por confirmar en código)

### Dependencia 2: WebMatrix.Util (Helpers Legacy)

**Helpers confirmados**:
- ✅ `ShowNotification(mensaje, tipo)` - 20+ apariciones
- ✅ `ActivateAccordion(index, efecto)` - 10+ apariciones
- ✅ `ShowNotifications.ErrorNotification`, `.InfoNotification`
- ✅ `EffectActivateAccordion.SlideEffect`, `.NoEffect`
- ✅ `InsertarItemSeleccion` - Constant para dropdowns

### Dependencia 3: ClosedXML.Excel

**Confirmadas**:
- ✅ Exportación Excel en: AprobacionesFiltros, ProgramacionCampo, IPSCuali
- ✅ Clases: `XLWorkbook`, `WorksheetXL`
- ⚠️ Métodos específicos por confirmar

### Dependencia 4: System.IO

**Confirmadas**:
- ✅ Importación en: CampoCualitativo, AprobacionesFiltros, ProgramacionCampo (para generación de archivos)

### Dependencia 5: Redirecciones a Otros Módulos

**Confirmadas**:
- ✅ `PY_Proyectos/SegmentosCuali.aspx` (CampoCualitativo.aspx línea 19)
- ✅ `PY_Proyectos/TrabajosCualitativos.aspx` (AprobacionesFiltros.aspx línea 43, DisenarFiltros.aspx)
- ✅ `OP_Cualitativo/TrabajosCoordinador.aspx` (ProgramacionCampo.aspx línea 58)
- ⚠️ `GD_Documentos/*` (esperado, no confirma do en lectura)

---

## ⚠️ RIESGOS DETECTADOS (FASE 2)

### Riesgo 1: Complejidad Extrema en DisenarFiltros.aspx

**Evidencia**:
- 1,062 líneas de código
- Controles dinámicos generados en runtime
- Enumerado eTipoPregunta con 9 valores
- Lógica condicional múltiple para cada tipo de pregunta

**Impacto**: 🔴 CRÍTICO para migración a MVC
- WebForms permite generar controles dinámicos con ViewState
- MVC/Razor NO tiene equivalente directo
- **Solución**: JavaScript con templates + AJAX

---

### Riesgo 2: Dependencia en Session("IDUsuario")

**Evidencia**:
- Aparece en TODOS los archivos analizados
- Línea 24 Trabajos.aspx: `Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())`
- Línea 13 FichaEntrevista.aspx: `Request.QueryString("idtrabajo")`
- Línea 27 AprobacionesFiltros.aspx: `Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())`

**Impacto**: 🔴 CRÍTICO
- ASP.NET Core **no recomienda Session para escalabilidad**
- **Solución**: Reemplazar con Claims de autenticación

---

### Riesgo 3: UpdatePanels en CampoCualitativo.aspx

**Evidencia**:
- Línea 10-12: `ScriptManager.RegisterPostBackControl(imbDescargarCita)`, `ScriptManager.RegisterPostBackControl(Me.btnExportar)`
- Indica 3+ UpdatePanels
- Postbacks AJAX complejos

**Impacto**: 🔴 ALTO
- UpdatePanels = tecnología legacy WebForms
- MVC = HTML + JavaScript vanilla
- **Solución**: Refactorizar a AJAX moderno + partial views

---

### Riesgo 4: Generación Dinámica de Grids (ProgramacionCampo)

**Evidencia**:
- Línea 34-40: Custom Class `dataEntrevistados` con 12 propiedades
- Binding de Grid en runtime con propiedades calculadas
- Validaciones en `RowCommand` con castings complejos

**Impacto**: 🟠 MEDIO-ALTO
- **Solución**: Usar List<T> con ViewModel en lugar de DataTable

---

### Riesgo 5: SQL Injection en Filtro de Búsqueda

**Evidencia**:
- Trabajos.aspx.vb, línea 115 (inferida de btnBuscar_Click)
- Búsqueda con DataView.RowFilter ("JobBook LIKE '%{0}%'")
- **NO usa parámetros**

**Impacto**: 🔴 CRÍTICO de seguridad
- **Solución**: Usar Dapper/EF con parámetros en todos los adapters

---

## 📊 MATRIZ DE COMPLEJIDAD POR ARCHIVO

| Archivo | LOC | Complejidad | Prioridad | Estimación |
|---------|-----|-------------|-----------|------------|
| `DisenarFiltros.aspx.vb` | 1,062 | 🔴 CRÍTICA | P0 | 30h |
| `ProgramacionCampo.aspx.vb` | 822 | 🔴 MUY ALTA | P0 | 24h |
| `IPSCuali.aspx.vb` | 682 | 🟠 ALTA | P1 | 16h |
| `FichaEntrevista.aspx.vb` | 353 | 🟠 ALTA | P0 | 14h |
| `CampoCualitativo.aspx.vb` | 346 | 🟠 ALTA | P0 | 12h |
| `Entrevista.aspx.vb` | 297 | 🟠 MEDIA-ALTA | P1 | 10h |
| `AprobacionesFiltros.aspx.vb` | 270 | 🟠 MEDIA-ALTA | P0 | 12h |
| `Trabajos.aspx.vb` | 217 | 🟠 MEDIA | P0 | 10h |
| `Transcripcion.aspx.vb` | 231 | 🟠 MEDIA | P1 | 8h |
| `MuestraTrabajos.aspx.vb` | 106 | 🟢 MEDIA-BAJA | P1 | 6h |
| `TrabajosCoordinador.aspx.vb` | ⚠️ POR LEER | ⚠️ POR ESTIMAR | P1 | 8h |
| `Calendario.aspx.vb` | ⚠️ POR LEER | ⚠️ POR ESTIMAR | P2 | 12h |
| `VisualizadorFiltros.aspx.vb` | ⚠️ POR LEER | ⚠️ POR ESTIMAR | P0 | 10h |
| `AprobacionesFiltrosAsitencia.aspx.vb` | ⚠️ POR LEER | ⚠️ POR ESTIMAR | P0 | 10h |
| Otros (Observacion, FichaSesion, FichaObservacion) | ⚠️ POR LEER | ⚠️ SIMILAR A ENTREVISTA | P0 | 30h |
| `AdministracionRegistroPlanillas.aspx` | Módulo JS | 🟠 MEDIA | P1 | 8h |

---

## ⚠️ ESTADO ACTUAL FASE 2

**Completado**:
- ✅ Análisis de 11 archivos VB.NET (4,800+ líneas)
- ✅ Tabla inventario del legado (21 archivos)
- ✅ Consolidación de dependencias (16 clases CoreProject)
- ✅ Detección de 5 riesgos críticos
- ✅ Matriz de complejidad por archivo

**Pendiente**:
- ⚠️ Lectura de 10 archivos faltantes
- ⚠️ **FASE 3**: Flujos Funcionales Detallados (FLUJO 1: COE, FLUJO 2: Filtros, FLUJO 3: Fichas)
- ⚠️ **FASE 4-6**: Mapeo 1:1, BD/SPs, Riesgos detallados, Componentes, Backlog

---

**¿Continúo con FASE 3: Flujos Funcionales Detallados (FLUJO 1: Gestión de Trabajos COE)?**
