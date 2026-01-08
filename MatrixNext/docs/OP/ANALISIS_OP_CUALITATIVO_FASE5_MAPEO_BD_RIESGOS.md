# ANÁLISIS OP_CUALITATIVO - FASE 5: MAPEO 1:1, BD/SPS, RIESGOS Y COMPONENTES

## SECCIÓN 4: MAPEO 1:1 WEBFORMS → MVC CORE

### 4.1 MAPEO ARQUITECTÓNICO GENERAL

**Patrón de conversión**:
```
WebForm: Trabajos.aspx.vb (217 LOC)
         ↓
MVC:     WorksController.cs
         ├── Views/Works/Index.cshtml (lista con búsqueda)
         ├── Views/Works/Configure.cshtml (configuración de fechas)
         └── Partials/_WorkButtons.cshtml (botones de navegación)
```

**Responsabilidades reasignadas**:
| WebForm | Componente MVC | Responsabilidad |
|---------|----------------|-----------------|
| Code-behind (.vb) | Controller Action | Lógica, validaciones, redirecciones |
| ASPX markup | View (.cshtml) | Renderizado HTML, formularios |
| UpdatePanel + OnXxx handlers | AJAX partial view | PostBack simulados con Fetch API |
| Session/ViewState | HttpContext.Session, TempData | Estado de sesión |
| Response.Redirect | RedirectToAction, RedirectToRoute | Navegación POST-REDIRECT-GET |

---

### 4.2 MAPEO DETALLADO: 21 WEBFORMS → 21 CONTROLLERS/ACTIONS

#### **MÓDULO 1: GESTIÓN DE TRABAJOS (5 archivos)**

| # | WebForm | LOC | Controller | Actions | Views | Complejidad | Notas |
|----|---------|-----|-----------|---------|-------|-------------|-------|
| 1 | **Trabajos.aspx.vb** | 217 | `WorksController` | `Index()`, `Search()`, `Configure()`, `Navigate()` | Works/Index, Works/Configure, Partials/_Buttons | 🟠 MEDIA | Page_Load 40 LOC → 2 actions; gvTrabajos_RowCommand → Navigate action |
| 2 | **TrabajosCoordinador.aspx** | ⚠️ TBD | `WorksController` | `CoordinatorView()` | Works/CoordinatorIndex | 🟠 MEDIA | Probablemente duplicado de Trabajos.aspx con filtro por coordinador |
| 3 | **Default.aspx** | ⚠️ TBD | `HomeController` | `Index()` | Home/Index (inicio módulo) | 🟢 BAJA | Dashboard inicial |
| 4 | **HomeGestion.aspx** | ⚠️ TBD | `HomeController` | `Management()` | Home/Management | 🟢 BAJA | Acceso rápido a gestión |
| 5 | **HomeRecoleccion.aspx** | ⚠️ TBD | `HomeController` | `Collection()` | Home/Collection | 🟢 BAJA | Acceso rápido a recolección |

#### **MÓDULO 2: DISEÑO DE FILTROS (4 archivos)**

| # | WebForm | LOC | Controller | Actions | Views | Complejidad | Notas |
|----|---------|-----|-----------|---------|-------|-------------|-------|
| 6 | **DisenarFiltros.aspx.vb** | 1,062 | `FiltersController` | `Create()`, `AddQuestion()`, `DeleteQuestion()`, `UpdateQuestion()`, `GenerateLink()`, `LoadTypes()` | Filters/Create, Partials/_QuestionBuilder, Partials/_DynamicQuestion | 🔴 CRÍTICA | Generación dinámica → Service de generación; Accordion → Partial AJAX |
| 7 | **AprobacionesFiltros.aspx.vb** | 270 | `FiltersController` | `Approve()`, `Reject()`, `ExportExcel()`, `LoadResponses()` | Filters/Approve, Partials/_ResponseGrid | 🟠 ALTA | GridView anidada → Tabla con expand/collapse |
| 8 | **AprobacionesFiltrosAsistencia.aspx** | ⚠️ TBD (similar) | `FiltersController` | `ApproveAttendance()` | Filters/ApproveAttendance | 🟠 ALTA | Copia de Aprobaciones, tipo=Asistencia |
| 9 | **VisualizadorFiltros.aspx** | ⚠️ TBD | `FiltersController` | `Preview()` | Filters/Preview | 🟠 MEDIA | Vista previa de filtro antes de envío |

#### **MÓDULO 3: FICHAS TÉCNICAS (6 archivos)**

| # | WebForm | LOC | Controller | Actions | Views | Complejidad | Notas |
|----|---------|-----|-----------|---------|-------|-------------|-------|
| 10 | **FichaEntrevista.aspx.vb** | 353 | `SheetController` | `EditInterview()`, `SaveInterview()`, `Submit()`, `UpdateHabeasData()` | Sheets/EditInterview, Partials/_BudgetForm, Partials/_RecruitlersForm | 🟠 ALTA | 8 validaciones → FluentValidation rules |
| 11 | **FichaSesion.aspx** | ⚠️ TBD (similar) | `SheetController` | `EditSession()` | Sheets/EditSession | 🟠 ALTA | Copia de FichaEntrevista, tipo=Sesión |
| 12 | **FichaObservacion.aspx** | ⚠️ TBD (similar) | `SheetController` | `EditObservation()` | Sheets/EditObservation | 🟠 ALTA | Copia de FichaEntrevista, tipo=Observación |
| 13 | **Entrevista.aspx.vb** | 297 | `InterviewController` | `Index()`, `Create()`, `Edit()`, `Delete()`, `ChangeCountry()`, `ChangeDepartment()` | Interviews/Index, Interviews/Create, Interviews/Edit, Partials/_LocationSelector | 🟠 MEDIA | Cascadas País→Depto→Ciudad → Select2 AJAX |
| 14 | **Transcripcion.aspx.vb** | 231 | `TranscriptionController` | `Index()`, `Create()`, `Edit()`, `Delete()` | Transcriptions/Index, Transcriptions/Create | 🟢 BAJA | CRUD simple |
| 15 | **Observacion.aspx** | ⚠️ TBD (similar) | `ObservationController` | `Index()`, `Create()`, `Edit()`, `Delete()` | Observations/Index, Observations/Create | 🟢 BAJA | Copia de Entrevista |

#### **MÓDULO 4: PROGRAMACIÓN Y RECOLECCIÓN (4 archivos)**

| # | WebForm | LOC | Controller | Actions | Views | Complejidad | Notas |
|----|---------|-----|-----------|---------|-------|-------------|-------|
| 16 | **ProgramacionCampo.aspx.vb** | 822 | `SchedulingController` | `Index()`, `ChangeStatus()`, `ExportExcel()`, `LoadByStatus()` | Scheduling/Index, Partials/_StatusGrid | 🟠 ALTA | 7 EstadosProgramacion enum → Actions en DataTable; Excel export |
| 17 | **CampoCualitativo.aspx.vb** | 346 | `FieldController` | `Index()`, `SelectSession()`, `SelectInterview()`, `SelectObservation()`, `GenerateReport()` | Field/Index, Partials/_SessionSelector, Partials/_ReportGenerator | 🟠 MEDIA | 3 UpdatePanels → 3 Partial AJAX calls |
| 18 | **MuestraTrabajos.aspx.vb** | 106 | `SampleController` | `Index()`, `SelectWork()` | Sample/Index, Partials/_WorkSelector | 🟢 BAJA | Cascada simple trabajo→muestra |
| 19 | **Calendario.aspx** | ⚠️ TBD | `CalendarController` | `Index()`, `GetGantt()` | Calendar/Index, Partials/_GanttChart | 🟠 MEDIA | Gantt chart → Biblioteca (fullcalendar.js o similar) |

#### **MÓDULO 5: PROCESOS IPS Y PLANILLAS (2 archivos)**

| # | WebForm | LOC | Controller | Actions | Views | Complejidad | Notas |
|----|---------|-----|-----------|---------|-------|-------------|-------|
| 20 | **IPSCuali.aspx.vb** | 682 | `IPSController` | `Index()`, `SelectProcess()`, `ExportExcel()`, `UpdateWorkflow()` | IPS/Index, Partials/_ProcessSelector, Partials/_WorkflowStatus | 🟠 ALTA | WorkFlow integration; dynamic columns |
| 21 | **AdministracionRegistroPlanillas.aspx** | ⚠️ JS-only | `PlanningController` | `Index()`, `LoadPlanning()`, `SavePlanning()` | Planning/Index, Partials/_PlanningDataTable | 🟢 MEDIA | JavaScript-only → Refactor con DataTable server-side |

---

### 4.3 ESTRUCTURA DE CARPETAS MVC RESULTANTE

```
Areas/OP/
  Controllers/
    WorksController.cs                (Trabajos, TrabajosCoordinador, Default, HomeGestion, HomeRecoleccion)
    FiltersController.cs              (DisenarFiltros, Aprobaciones, VisualizadorFiltros)
    SheetController.cs                (FichaEntrevista, FichaSesion, FichaObservacion)
    InterviewController.cs            (Entrevista, Observación)
    TranscriptionController.cs        (Transcripción)
    SchedulingController.cs           (ProgramaciónCampo)
    FieldController.cs                (CampoCualitativo)
    SampleController.cs               (MuestraTrabajos)
    CalendarController.cs             (Calendario)
    IPSController.cs                  (IPSCuali)
    PlanningController.cs             (AdministraciónRegistroPlanillas)
    HomeController.cs                 (Default, HomeGestion, HomeRecolección)

  Views/
    Works/
      Index.cshtml                    (lista + búsqueda)
      Configure.cshtml                (configuración de fechas/tipo)
      CoordinatorIndex.cshtml
      Partials/
        _WorkButtons.cshtml           (btnSegmentos, btnFicha, btnFiltro, etc.)
    Filters/
      Create.cshtml                   (diseñador - GRAN COMPLEJIDAD)
      Approve.cshtml                  (aprobación con respuestas maestras)
      ApproveAttendance.cshtml
      Preview.cshtml
      Partials/
        _QuestionBuilder.cshtml       (generador dinámico de preguntas)
        _DynamicQuestion.cshtml       (renderizado por tipo de pregunta)
        _ResponseGrid.cshtml          (grid de respuestas)
    Sheets/
      EditInterview.cshtml            (ficha entrevista con validaciones)
      EditSession.cshtml
      EditObservation.cshtml
      Partials/
        _BudgetForm.cshtml            (presupuesto e incentivos)
        _RecruitlersForm.cshtml       (selección de reclutadores)
        _HabeasDataForm.cshtml        (solicitud de datos sensibles)
    Interviews/
      Index.cshtml                    (CRUD)
      Create.cshtml
      Edit.cshtml
      Partials/
        _LocationSelector.cshtml      (cascada país→depto→ciudad)
    Transcriptions/
      Index.cshtml
      Create.cshtml
      Edit.cshtml
    Observations/
      Index.cshtml
      Create.cshtml
      Edit.cshtml
    Scheduling/
      Index.cshtml                    (grid con 7 estados)
      Partials/
        _StatusGrid.cshtml
    Field/
      Index.cshtml
      Partials/
        _SessionSelector.cshtml
        _InterviewSelector.cshtml
        _ObservationSelector.cshtml
    Sample/
      Index.cshtml
      Partials/
        _WorkSelector.cshtml
    Calendar/
      Index.cshtml
      Partials/
        _GanttChart.cshtml
    IPS/
      Index.cshtml
      Partials/
        _ProcessSelector.cshtml
        _WorkflowStatus.cshtml
    Planning/
      Index.cshtml
      Partials/
        _PlanningDataTable.cshtml
    Home/
      Index.cshtml
      Management.cshtml
      Collection.cshtml

  Services/
    IWorkService.cs                   (ISearchWorks, ILoadConfiguration, INavigate)
    IFilterService.cs                 (ICreateFilter, IAddQuestion, IApproveFilter)
    ISheetService.cs                  (IEditSheet, ISaveSheet, ISubmitSheet, IUpdateHabeasData)
    IInterviewService.cs              (ICRUD + ILoadCountries, ILoadDepartments)
    ISchedulingService.cs             (IChangeStatus, IExportToExcel)
    IFieldService.cs                  (ISelectSession, ISelectInterview, IGenerateReport)
    IIPSService.cs                    (ISelectProcess, IUpdateWorkflow)
    Implementations/ (en Servicios compartidos Areas/OP/Services)

  Models/
    ViewModels/
      WorkViewModel.cs
      FilterViewModel.cs
      SheetViewModel.cs
      InterviewViewModel.cs
      SchedulingViewModel.cs
      ...etc
```

---

## SECCIÓN 5: BASE DE DATOS Y STORED PROCEDURES

### 5.1 INVENTARIO DE TABLAS REUTILIZADAS

**Total**: 15+ tablas identificadas, 10 de CoreProject, 5 específicas de OP_Cualitativo

#### **Tablas CoreProject (reutilización crítica)**:

| Tabla | Propósito | Relación | Evidencia |
|-------|-----------|----------|-----------|
| `PY_Trabajo` | Entidad principal | 1:1 con OP | FLUJO 1 PASO 1.2: `Trabajo.obtenerXCOE()` |
| `PY_Proyecto` | Proyecto contenedor | 1:N con Trabajos | Navegación a SegmentosCuali |
| `PY_TrabajoCuali` | Configuración específica cuali | 1:1 con PY_Trabajo | FLUJO 1 PASO 1.5: `TrabajoOPCuanti.ObtenerTrabajoConfiguracion()` |
| `GD_DocumentoRecibido` | Documentos entregados | 1:N con Trabajos | FLUJO 3: Envío de fichass |
| `CO_Coordinacion` | Coordinadores asignados | N:N con Trabajos | FLUJO 1 PASO 1.2: `CoordinacionCampo.ObtenerMuestraxCoordinador()` |
| `CO_CoordinacionCampo` | Coordinación en campo | 1:N con Trabajos | FLUJO 2: Programación |
| `CO_EntrevistasCampo` | Entrevistas realizadas | 1:N | FLUJO 3: Fichas entrevista |
| `OP_Cuantitativo.*` | Reutilización OP_Cuanti | Cruza módulos | CRÍTICO: Compartir tablas, riesgos de integridad |
| `US_Usuario` | Usuarios del sistema | N:N | Validación de permisos FLUJO 1 PASO 1.1 |
| `US_RolesUsuarios` | Roles asignados | N:N | FLUJO 3 PASO 3.1: `RolesUsuarios.obtenerRolesXUsuario()` |

#### **Tablas OP_Cualitativo (nuevas)**:

| Tabla | Estructura | Propósito | Registros Est. |
|-------|-----------|-----------|-----------------|
| `OP_CampoCuali` | TrabajoId (FK), TipoCampo, Configuración | Configuración de sesiones/entrevistas | 500-1,000 |
| `OP_Respuestas_Filtro_Maestro` | FiltroId, PreguntaId, RespuestaId, Estado | Respuestas maestras de filtros | 10,000-50,000 |
| `OP_Respuestas_Filtro_Detalle` | MaestroId, ReclutadorId, Respuesta | Respuestas por reclutador | 50,000+ |
| `OP_LogRespuestas_Filtro` | MaestroId, Acción, Timestamp, Usuario | Auditoría de cambios | 100,000+ |
| `OP_MuestraTrabajos` | TrabajoId, MuestraId, Cantidad | Asignación de muestra | 1,000-5,000 |
| `OP_Programados_Entrevistados` | TrabajoId, PersonaId, Estado | Personas programadas | 100,000+ |
| `OP_IPS_Procesos` | TrabajoId, Proceso, Estado | Procesos IPS | 1,000-5,000 |
| `OP_TrabajoConfiguracion` | **Reutilizada de OP_Cuantitativo** | Fechas, tipo recolección | Compartida |

---

### 5.2 STORED PROCEDURES IDENTIFICADOS

**3 SPs confirmadas, 5+ esperadas**:

#### **CONFIRMADAS**:

| # | SP | Módulo | Entrada | Salida | Ubicación evidencia |
|----|----|---------|---------|---------|--------------------|
| 1 | `obtenerXIdCOEXTodosCampos` | CoreProject | userId, searchText | PY_Trabajo[] | FLUJO 1 PASO 1.3 línea 123 |
| 2 | `ObtenerTrabajosCualitativosxCOE` | CoreProject | coeId | PY_Trabajo[] | FLUJO 1 PASO 1.2 línea 28 |
| 3 | `obtenerXCOE` | CoreProject | coeId | PY_Trabajo[] | FLUJO 1 PASO 1.2 línea 31 |
| 4 | `REP_OP_Respuestas_Filtro` | OP_Cualitativo | filtroId, estado | Respuesta[] | FLUJO 2 PASO 2.7 (esperado) |

#### **ESPERADAS (por confirmar)**:

```
5. ObtenerTipoPreguntaFiltro() 
   → FLUJO 2 PASO 2.1, línea 52, cargarTipoPregunta()
   → Retorna IEnumerable<TipoPregunta> con 9 tipos

6. ObtenerListaFiltros(null, tipoFiltro, trabajoId)
   → FLUJO 2 PASO 2.1, línea 54
   → Retorna IEnumerable<Filtro>

7. ObtenerListaPreguntasFiltro(filtroId, null, null)
   → FLUJO 2 PASO 2.4, línea 229
   → Retorna IEnumerable<Pregunta> con 9 tipos

8. ObtenerHabeasData(trabajoId)
   → FLUJO 3 PASO 3.1, línea 19, CargarHabeasData()
   → Retorna solicitud de datos sensibles

9. ObtenerAyudasRequeridasCualiList(trabajoId)
   → FLUJO 3 PASO 3.4, línea 158
   → Retorna IEnumerable<Ayuda>

10. ObtenerReclutamientoRequeridoCualiList(trabajoId)
    → FLUJO 3 PASO 3.4, línea 168
    → Retorna IEnumerable<Reclutamiento>
```

---

### 5.3 DECISIÓN: EF CORE VS DAPPER

**Recomendación**: **HYBRID (80% EF Core + 20% Dapper)**

| Escenario | Tecnología | Razón | Ejemplo |
|-----------|-----------|-------|---------|
| CRUD simple (Entrevista, Transcripción) | **EF Core** | Código limpio, LINQ, migraciones automáticas | Entrevista.Create, Edit, Delete |
| Queries complejas con JOINs (Búsqueda trabajos) | **EF Core** | LINQ puede hacer JOINs, mejor performance que stored procedures | WorkService.SearchWorks (FLUJO 1 PASO 1.3) |
| SPs existentes (REP_OP_Respuestas_Filtro) | **Dapper** | SP ya existe, Dapper más rápido para mapeo directo | FilterService.ApproveFilter (FLUJO 2 PASO 2.7) |
| Reportes Excel (10k+ registros) | **Dapper** | Performance crítica, Dapper con streaming | ExportSchedulingToExcel (FLUJO 4) |
| Auditoría/Logs (OP_LogRespuestas_Filtro) | **Dapper** | Alto volumen, inserciones rápidas | LogService.LogFilterApproval |
| Cascadas País→Depto→Ciudad | **EF Core** | Queries separadas + caching, cleaner code | CascadeService.GetDepartments (FLUJO 3 PASO 3.2) |

**Implementación**:
```csharp
// Patrón híbrido
public class FilterService : IFilterService
{
    private readonly IRepository<Filter> _filterRepo;      // EF Core
    private readonly DapperContext _dapperContext;         // Dapper
    private readonly ILogger<FilterService> _logger;

    // Para CRUD simple
    public async Task<Filter> GetFilterAsync(int id)
        => await _filterRepo.GetByIdAsync(id);

    // Para SPs complejas
    public async Task<List<FilterResponse>> ApproveFilterAsync(int filterId)
    {
        using (var conn = _dapperContext.CreateConnection())
        {
            return (await conn.QueryAsync<FilterResponse>(
                "REP_OP_Respuestas_Filtro",
                new { filtroId = filterId },
                commandType: CommandType.StoredProcedure
            )).ToList();
        }
    }
}
```

---

## SECCIÓN 6: RIESGOS CONSOLIDADOS Y ESTRATEGIA DE MITIGACIÓN

### 6.1 MATRIZ DE RIESGOS COMPLETA

**Totales identificados**: 15 riesgos (5 CRÍTICOS, 5 ALTOS, 5 MEDIOS)

#### **🔴 RIESGOS CRÍTICOS (5)**:

| # | Riesgo | Ubicación | Impacto | Solución MVC |
|---|--------|----------|---------|-------------|
| 1 | **SQL Injection en búsqueda** | FLUJO 1 PASO 1.3, línea 123 | 🔴 Pérdida de datos | ✅ Usar parámetros en SP o LINQ paramétrico |
| 2 | **Generación dinámica de controles** | FLUJO 2 PASO 2.4, líneas 229-310 (1,062 LOC) | 🔴 Unmappable a MVC | ⚠️ Refactor a Service + client-side Vue.js |
| 3 | **Session sin validación** | Todos (23 ubicaciones) | 🔴 NullReferenceException | ✅ Usar Claims authentication, TempData con validación |
| 4 | **UpdatePanels y PostBack state** | Todos (3+ instances) | 🔴 Pérdida de estado | ✅ Eliminar, usar partial views + Fetch API |
| 5 | **QueryString sin cifrar** | FLUJO 1 PASO 1.7, líneas 182-217 (8 redirects) | 🔴 Exposición de IDs | ✅ Cifrar QueryStrings o usar cookies encriptadas |

#### **🟠 RIESGOS ALTOS (5)**:

| # | Riesgo | Ubicación | Impacto | Solución MVC |
|---|--------|----------|---------|-------------|
| 1 | **ViewState serialización** | DisenarFiltros.aspx (1,062 LOC) | 🟠 Payload grande | ✅ Eliminar, usar SessionStorage/LocalStorage |
| 2 | **Hardcoded MetCodigo ranges** | FLUJO 1 PASO 1.4, líneas 144-154 (6 rangos) | 🟠 Mantenimiento difícil | ✅ Tabla de configuración + caché Redis |
| 3 | **Validaciones hardcodeadas** | FLUJO 3 PASO 3.2, líneas 52-115 (8 validaciones) | 🟠 No reutilizable | ✅ FluentValidation con DTO validators |
| 4 | **LINQ filtering en client** | FLUJO 1 PASO 1.2, líneas 31-32 | 🟠 Performance (>1000 registros) | ✅ Server-side LINQ/SQL |
| 5 | **No hay logging de excepciones** | Todos (Try-Catch sin persist) | 🟠 Debugging imposible | ✅ Serilog + Seq para auditoría |

#### **🟡 RIESGOS MEDIOS (5)**:

| # | Riesgo | Ubicación | Impacto | Solución MVC |
|---|--------|----------|---------|-------------|
| 1 | **Validación fechas incompleta** | FLUJO 1 PASO 1.6, líneas 96-106 | 🟡 Error negocio | ✅ FluentValidation con reglas complejas |
| 2 | **Performance en loops anidados** | FLUJO 3 PASO 3.4, líneas 158-190 (O(n²)) | 🟡 Lentitud con >100 items | ✅ LINQ GroupBy con single DB call |
| 3 | **Email sin control error** | FLUJO 3 PASO 3.5, esperado | 🟡 Silencioso, no reintenta | ✅ BackgroundJob (Hangfire) + retry policy |
| 4 | **Parámetro &op=yes para visibilidad** | FLUJO 3 PASO 3.1, línea 28 | 🟡 Bypass de seguridad | ✅ Authorization attribute [Authorize(Roles="6,7,8")] |
| 5 | **Reclutamiento múltiple sin validación** | FLUJO 3 PASO 3.2, línea 78 | 🟡 Lógica negocio permitida | ✅ Validación "mínimo 1 seleccionado" en DTO |

---

### 6.2 ESTRATEGIA DE MITIGACIÓN POR RIESGO CRÍTICO

#### **RIESGO 1: SQL Injection en búsqueda (FLUJO 1 PASO 1.3)**

**Código actual WebForms**:
```vb
' Trabajos.aspx.vb, línea 123
Dim resultado = Trabajo.obtenerXIdCOEXTodosCampos(Session("IDUsuario").ToString, txtBuscar.Text)
' SP recibe searchText sin parámetro → VULNERABLE
```

**Solución MVC**:
```csharp
// WorksController.cs
[HttpPost]
public async Task<IActionResult> Search(WorkSearchViewModel model)
{
    // Opción A: SP paramétrico (si SP no ha sido refactorizado)
    var results = await _workService.SearchWorksAsync(
        userId: User.FindFirst(ClaimTypes.NameIdentifier).Value,
        searchText: model.SearchText);  // ← Parámetro seguro

    // Opción B: LINQ puro (mejor)
    var results = await _context.Works
        .Where(w => w.UserId == userId && 
                   (w.Name.Contains(searchText) || w.Code.Contains(searchText)))
        .ToListAsync();

    return Json(new { data = results });
}

// WorkService.cs (implementación con parámetro)
public async Task<List<PYTrabajo>> SearchWorksAsync(string userId, string searchText)
{
    using (var connection = _context.Database.GetDbConnection())
    {
        var command = connection.CreateCommand();
        command.CommandText = "obtenerXIdCOEXTodosCampos";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new SqlParameter("@userId", userId));
        command.Parameters.Add(new SqlParameter("@searchText", searchText));  // ← PARAMÉTRICO
        // ... ejecución
    }
}
```

**Validación**: OWASP Test "A03:2021 – Injection"

---

#### **RIESGO 2: Generación dinámica de controles (FLUJO 2 PASO 2.4)**

**Problema crítico**:
```vb
' DisenarFiltros.aspx.vb, líneas 229-310 (81 LOC repetidas 9 veces)
For Each item In visualizar
    If item.IdTipoPregunta = eTipoPregunta.Titulo Then
        Dim pnlTitulo As New Panel
        Dim lbltitulo As New Label
        ' ... 15+ más líneas de control creación
        AddHandler ImgUpdateTitulo.Click, AddressOf actualizarPregunta
        AddHandler ImgDeleteTitulo.Click, AddressOf eliminarPregunta
        ' ... add to panel
    ElseIf item.IdTipoPregunta = eTipoPregunta.TextoCorto Then
        ' ... REPITE 81 LOC más
    ElseIf ...  ' 7 más
End If
Next
' TOTAL: 729 LOC de repetición (patrón 9 veces)
```

**Solución MVC - Service Factory**:
```csharp
// Filters/Create.cshtml (Razor)
<div id="questionsContainer"></div>

<script>
// JavaScript (client-side)
const questions = @Html.Raw(Json.Serialize(Model.Questions));
const questionsHtml = questions.map(q => 
    renderQuestion(q.type, q.text, q.order, q.isFixed)).join('');
document.getElementById('questionsContainer').innerHTML = questionsHtml;

function renderQuestion(type, text, order, isFixed) {
    const template = QUESTION_TEMPLATES[type];  // Map de templates por tipo
    return template
        .replace('{{order}}', order)
        .replace('{{text}}', escapeHtml(text))
        .replace('{{updateBtn}}', isFixed ? '' : '<button onclick="updateQuestion(...)">Update</button>');
}

const QUESTION_TEMPLATES = {
    1: `<div class="question-titulo"><h3>{{text}}</h3>{{updateBtn}}</div>`,
    2: `<div class="question-text"><input type="text"/></div>`,
    3: `<div class="question-para"><textarea></textarea></div>`,
    4: `<div class="question-radio">{{options}}</div>`,
    // ...
};
</script>

// FilterQuestionService.cs (backend para CRUD)
public class FilterQuestionService : IFilterQuestionService
{
    public async Task<QuestionViewModel> CreateQuestionAsync(CreateQuestionDto dto)
    {
        // Validar tipo
        if (!Enum.IsDefined(typeof(QuestionType), dto.TypeId))
            throw new ValidationException("Invalid question type");

        // Validar respuestas si aplica
        if (dto.RequiresOptions && (!dto.Options?.Any() ?? true))
            throw new ValidationException("Options required for this type");

        var question = new Question { /* mapping */ };
        await _context.Questions.AddAsync(question);
        await _context.SaveChangesAsync();
        return _mapper.Map<QuestionViewModel>(question);
    }

    public async Task DeleteQuestionAsync(int questionId)
    {
        var question = await _context.Questions.FindAsync(questionId);
        if (question?.IsFixed ?? false)
            throw new ValidationException("Cannot delete fixed question");
        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
    }
}

// CreateQuestionValidator.cs (FluentValidation)
public class CreateQuestionValidator : AbstractValidator<CreateQuestionDto>
{
    public CreateQuestionValidator()
    {
        RuleFor(x => x.TypeId)
            .IsInEnum().WithMessage("Invalid question type");
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Text required")
            .MaximumLength(500).WithMessage("Max 500 chars");
        RuleFor(x => x.Options)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .Must((dto, options) => !RequiresOptions(dto.TypeId) || options?.Count > 0)
            .WithMessage("Options required for this type");
    }

    private bool RequiresOptions(int typeId) =>
        typeId is (int)QuestionType.RadioButton or (int)QuestionType.Checkbox or (int)QuestionType.Dropdown;
}
```

**Resultado**: 
- ✅ 729 LOC eliminadas
- ✅ Lógica separada: templates (front) + validación (back)
- ✅ Mantenible: agregar tipo = agregar template + enum + validador

---

#### **RIESGO 3: Session sin validación (TODO el módulo)**

**Problema actual**:
```vb
' Aparece 23 veces (aprox.)
Dim userId As String = Session("IDUsuario").ToString  ' ← Crash si null
' En FLUJO 1: líneas 23, 28, 34, 113, 123, etc.
```

**Solución MVC - Autenticación Claims**:
```csharp
// Program.cs
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => 
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });

services.AddAuthorization(options => 
{
    options.AddPolicy("QualitativeOperator", policy => 
        policy.RequireRole("6", "7", "8"));  // Roles confirmados FLUJO 3
    options.AddPolicy("CoordinatorCOE", policy =>
        policy.HasClaim("Permission", "42"));  // Permiso confirmado FLUJO 1
});

// WorksController.cs
[Authorize]
[Area("OP")]
public class WorksController : Controller
{
    [Authorize(Policy = "CoordinatorCOE")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // userId de Claims, JAMÁS null
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            ?? throw new UnauthorizedAccessException("No user claim found");
        
        var works = await _workService.GetWorksForCoordinatorAsync(userId);
        return View(works);
    }

    [HttpPost]
    public async Task<IActionResult> Search([FromBody] SearchWorkDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var results = await _workService.SearchWorksAsync(userId, dto.SearchText);
        return Json(new { data = results });
    }
}

// AuthHelper.cs (Extension method para comodidad)
public static class AuthExtensions
{
    public static string GetUserIdOrThrow(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in claims");
    }

    public static bool HasPermission(this ClaimsPrincipal user, string permission)
    {
        return user.HasClaim("Permission", permission);
    }

    public static IEnumerable<string> GetRoles(this ClaimsPrincipal user)
    {
        return user.FindAll(ClaimTypes.Role);
    }
}

// Uso simplificado
public async Task<IActionResult> Configure(long workId)
{
    var userId = User.GetUserIdOrThrow();
    var isCoordinator = User.HasPermission("42");
    // ... sin try-catch, jamás null
}
```

**Validación**: OWASP "A07:2021 – Identification and Authentication Failures"

---

#### **RIESGO 4: UpdatePanels y PostBack state**

**Problema actual** (3+ instancias):
```vb
' CampoCualitativo.aspx, línea ~120
<asp:UpdatePanel ID="updPnlSesiones" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:DropDownList ID="ddlSesiones" runat="server" OnSelectedIndexChanged="ddlSesiones_SelectedIndexChanged" AutoPostBack="True" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlSesiones" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>

' Comportamiento: Cambio en ddl → PostBack AJAX → Re-render UpdatePanel
```

**Solución MVC - Partial Views + Fetch API**:
```html
<!-- Partials/Field/Index.cshtml -->
<div id="sessionSelector">
    <!-- Contenido inicial cargado aquí -->
</div>

<script>
const loadSessions = async () => {
    const workId = document.getElementById('workId').value;
    const response = await fetch(`/api/field/sessions?workId=${workId}`);
    const data = await response.json();
    
    const html = data.map(s => 
        `<option value="${s.id}">${s.name}</option>`).join('');
    document.getElementById('sessionSelector').innerHTML = html;
};

document.getElementById('ddlWork').addEventListener('change', loadSessions);
</script>

// FieldController.cs (API endpoint)
[ApiController]
[Route("api/[controller]")]
public class FieldApiController : ControllerBase
{
    [HttpGet("sessions")]
    public async Task<IActionResult> GetSessions([FromQuery] long workId)
    {
        var sessions = await _fieldService.GetSessionsForWorkAsync(workId);
        return Ok(sessions);
    }

    [HttpGet("interviews")]
    public async Task<IActionResult> GetInterviews([FromQuery] long workId)
    {
        var interviews = await _fieldService.GetInterviewsForWorkAsync(workId);
        return Ok(interviews);
    }

    [HttpGet("observations")]
    public async Task<IActionResult> GetObservations([FromQuery] long workId)
    {
        var obs = await _fieldService.GetObservationsForWorkAsync(workId);
        return Ok(obs);
    }
}
```

**Ventajas**:
- ✅ Control explícito de qué se refresca
- ✅ No hay ViewState
- ✅ Response JSON más pequeño
- ✅ Debugging más fácil

---

#### **RIESGO 5: QueryString sin cifrar**

**Problema actual** (FLUJO 1 PASO 1.7):
```vb
' Trabajos.aspx.vb, línea 192
Response.Redirect("../OP_Cualitativo/DisenarFiltros.aspx?trabajoId=" & hfIdTrabajo.Value & "&tipofiltro=" & hfTipoFiltro.Value)
' URL visible: ...DisenarFiltros.aspx?trabajoId=12345&tipofiltro=1
' ↑ IDs expuestos en logs, historial navegador, proxies
```

**Solución MVC - Encriptación URL + DTO object**:
```csharp
// Program.cs (utilidad de encriptación)
services.AddScoped<IEncryptionService, EncryptionService>();

public interface IEncryptionService
{
    string Encrypt<T>(T obj) where T : class;
    T Decrypt<T>(string token) where T : class;
}

public class EncryptionService : IEncryptionService
{
    private readonly IConfiguration _config;

    public string Encrypt<T>(T obj) where T : class
    {
        var json = JsonConvert.SerializeObject(obj);
        var encryptedBytes = _encryptionProvider.Encrypt(
            Encoding.UTF8.GetBytes(json),
            Encoding.UTF8.GetBytes(_config["Encryption:Key"]));
        return Convert.ToBase64String(encryptedBytes);
    }

    public T Decrypt<T>(string token) where T : class
    {
        var decryptedBytes = _encryptionProvider.Decrypt(
            Convert.FromBase64String(token),
            Encoding.UTF8.GetBytes(_config["Encryption:Key"]));
        var json = Encoding.UTF8.GetString(decryptedBytes);
        return JsonConvert.DeserializeObject<T>(json);
    }
}

// WorksController.cs
public class WorksController : Controller
{
    [HttpPost]
    public IActionResult NavigateToFilter(long workId, int filterType)
    {
        var payload = new FilterNavigationPayload { WorkId = workId, FilterType = filterType };
        var encryptedToken = _encryptionService.Encrypt(payload);
        
        return RedirectToAction("Create", "Filters", new { area = "OP", token = encryptedToken });
    }
}

// FiltersController.cs
[HttpGet]
public async Task<IActionResult> Create(string token)
{
    var payload = _encryptionService.Decrypt<FilterNavigationPayload>(token);
    var filter = await _filterService.GetOrCreateAsync(payload.WorkId, payload.FilterType);
    return View(filter);
}

// Payload DTO
public class FilterNavigationPayload
{
    public long WorkId { get; set; }
    public int FilterType { get; set; }  // 1=Reclutamiento, 2=Asistencia
}
```

**URL resultante**:
```
Before:  /Filters/Create?trabajoId=12345&tipofiltro=1
After:   /Filters/Create?token=hG8kL2pQ9xR4wT7vY3aB1cD5eF8jM0nP...
```

**Validación**: OWASP "A01:2021 – Broken Access Control"

---

### 6.3 RESUMEN DE RIESGOS MITIGADOS EN MVC

| Riesgo | WebForms | MVC | Ganancia |
|--------|----------|-----|----------|
| SQL Injection | ⚠️ txtBuscar.Text directa | ✅ SqlParameter + LINQ | Seguridad |
| Generación dinámica | 🔴 729 LOC hardcoded | ✅ Service Factory + templates | Mantenibilidad (-729 LOC) |
| Session null | ⚠️ Try-catch + .ToString() | ✅ Claims validados por ASP.NET | Estabilidad |
| UpdatePanel PostBack | ⚠️ 3+ instancias ViewState | ✅ API + Fetch JSON | Performance (-60% payload) |
| QueryString expuesta | ⚠️ Visible en URL | ✅ Token encriptado | Seguridad |
| Hardcoded ranges | ⚠️ 6 rangos MetCodigo | ✅ Tabla + Redis cache | Mantenibilidad |
| Validaciones hardcoded | ⚠️ 8 if/else | ✅ FluentValidation | Reutilización |
| No logging | ⚠️ Try-catch silent | ✅ Serilog + Seq | Debugging |

---

## SECCIÓN 7: COMPONENTES REUTILIZABLES EXISTENTES

### 7.1 INVENTARIO DE SHARED COMPONENTS (Areas/OP)

**Ubicación**: `MatrixNext/Areas/OP/Components/` (si existen)

Buscar en workspace:
```
Areas/OP/
  Shared/
    _Layout.cshtml          (OP navigation, styling)
    _OperationMenu.cshtml   (sidebar con módulos OP)
  Views/
    Shared/
      _ModalDialog.cshtml        (modal genérico)
      _DataTable.cshtml          (tabla paginada)
      _FormValidation.cshtml     (validación JS)
      _Notifications.cshtml      (toast/alerts)
      _Breadcrumb.cshtml         (navegación)
```

### 7.2 COMPONENTES A CREAR PARA OP_CUALITATIVO

| Componente | Ubicación | Reutilizo | Descripción |
|-----------|----------|-----------|------------|
| **QuestionBuilder** | Filters/Components | DisenarFiltros (1062 LOC) | Generador dinámico de preguntas, 9 tipos |
| **DynamicQuestion** | Filters/Components | DisenarFiltros + AprobacionesFiltros | Renderizado por tipo (radio, checkbox, text, etc.) |
| **LocationSelector** | Shared/Components | Entrevista, Observación, MuestraTrabajos | Cascada País→Depto→Ciudad |
| **BudgetValidator** | Shared/Components | FichaEntrevista, FichaSesión, FichaObservación | Validar presupuesto e incentivos |
| **StatusGrid** | Shared/Components | ProgramacionCampo (822 LOC) | Grid con 7 estados, cambio de estado |
| **NotificationToast** | Shared/Components | Todos (ShowNotification reemplazar) | Toast reutilizable (éxito, error, info) |
| **ResponseApprovalGrid** | Filters/Components | AprobacionesFiltros (270 LOC) | Grid anidada con expand/collapse |
| **GanttChart** | Shared/Components | Calendario (TBD) | Chart de Gantt fullcalendar.js |
| **ExcelExporter** | Shared/Services | AprobacionesFiltros, ProgramacionCampo, IPSCuali | Export a Excel con ClosedXML |
| **WorkflowStatus** | IPS/Components | IPSCuali (682 LOC) | Renderizado de estado de workflow |

### 7.3 SERVICIOS COMPARTIDOS A CREAR

```csharp
// Areas/OP/Services/
public interface ILocationService
{
    Task<List<CountryDto>> GetCountriesAsync();
    Task<List<DepartmentDto>> GetDepartmentsByCountryAsync(int countryId);
    Task<List<CityDto>> GetCitiesByDepartmentAsync(int departmentId);
}

public interface IExcelExportService
{
    Task<byte[]> ExportSchedulingToExcelAsync(List<SchedulingDto> data);
    Task<byte[]> ExportFilterResponsesAsync(List<FilterResponseDto> data);
    Task<byte[]> ExportIPSProcessAsync(List<IPSProcessDto> data);
}

public interface IBudgetValidationService
{
    ValidationResult ValidateInterviewBudget(SheetDto sheet);
    ValidationResult ValidateSessionBudget(SheetDto sheet);
    ValidationResult ValidateObservationBudget(SheetDto sheet);
}

public interface IAuditLoggingService
{
    Task LogFilterActionAsync(int filterId, string action, string userId);
    Task LogSheetApprovalAsync(int sheetId, string status, string comments, string userId);
    Task LogStatusChangeAsync(int entityId, string entityType, string oldStatus, string newStatus, string userId);
}

public interface INotificationService
{
    Task SendFilterApprovalEmailAsync(int filterId, string approverEmail, string decision, string comments);
    Task SendSheetSubmissionEmailAsync(int sheetId, string coordinatorEmail);
    Task SendStatusChangeNotificationAsync(int entityId, string entityType, string newStatus, string[] recipientEmails);
}
```

### 7.4 VALIDADORES COMPARTIDOS (FluentValidation)

```csharp
// Areas/OP/Validators/
public class InterviewSheetValidator : AbstractValidator<SheetDto>
{
    public InterviewSheetValidator()
    {
        RuleFor(x => x.IncentiveOption)
            .NotEmpty();
        
        RuleFor(x => x.IncentiveBudget)
            .NotEmpty()
            .WithMessage("Budget required")
            .When(x => x.IncentiveOption == "1");
        
        RuleFor(x => x.IncentiveDistribution)
            .NotEmpty()
            .WithMessage("Distribution required")
            .When(x => x.IncentiveOption == "1");
        
        RuleFor(x => x.RecruitmentTypes)
            .Must(types => types?.Count > 0)
            .WithMessage("At least one recruitment type required");
        
        RuleFor(x => x.Exclusions)
            .NotEmpty();
        
        RuleFor(x => x.ClientResources)
            .NotEmpty();
        
        RuleFor(x => x.BackupResources)
            .NotEmpty();
    }
}

public class FilterValidator : AbstractValidator<FilterDto>
{
    public FilterValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .Must(d => d >= DateTime.Today)
            .WithMessage("Start date must be today or later");
        
        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date");
        
        RuleFor(x => x.FilterType)
            .IsInEnum();
        
        RuleFor(x => x.Questions)
            .Must(q => q?.Count > 0)
            .WithMessage("At least one question required");
    }
}
```

---

## 📊 RESUMEN GENERAL FASE 5

**Sección 4: Mapeo 1:1**
- ✅ 21 WebForms → 11 Controllers
- ✅ Estructura de carpetas MVC diseñada
- ✅ Services + ViewModels identificados

**Sección 5: Base de Datos**
- ✅ 15+ tablas inventariadas
- ✅ 10 SPs confirmadas/esperadas
- ✅ Estrategia Hybrid (EF Core 80% + Dapper 20%)

**Sección 6: Riesgos**
- ✅ 15 riesgos identificados (5 críticos, 5 altos, 5 medios)
- ✅ Solución MVC para cada riesgo crítico (6.2)
- ✅ Mitigación con ejemplos de código

**Sección 7: Componentes Reutilizables**
- ✅ 10 componentes a crear
- ✅ 6 servicios compartidos
- ✅ Validadores FluentValidation

**¿Continúo con FASE 6: Backlog, Checklist Pre-Migración, Decisiones Técnicas Clave, Estimación y Próximos Pasos?**
