# 📋 BACKLOG DE MIGRACIÓN - GD_Documentos FASE 3

**Fases**: FASE 3 (Sprints 4-5)  
**Tema**: Solicitudes de Documentos + Aprobaciones + Workflow  
**Horas Totales**: 52h (44h implementación + 8h investigación)  
**Duración Estimada**: 1.5 semanas (2 sprints)  
**Versión**: 1.0  
**Fecha**: 2026-01-09

---

## 📑 CONTENIDO

- [Resumen Ejecutivo](#resumen-ejecutivo)
- [Sprint 4: Solicitudes de Documentos](#sprint-4-solicitudes-de-documentos)
- [Sprint 5: Aprobaciones + Investigación Workflow](#sprint-5-aprobaciones--investigación-workflow)

---

## 🎯 RESUMEN EJECUTIVO

### Objetivos de FASE 3

Implementar el sistema de workflow (lo más complejo de GD_Documentos):

1. **Solicitudes de Documentos** (P1-1, Sprint 4): 24h
   - Crear solicitud de construcción/actualización/anulación
   - Asignar múltiples revisores
   - Notificaciones por email (asíncrono)

2. **Aprobaciones** (P1-2, Sprint 5): 20h
   - Listar revisiones pendientes por usuario
   - Aprobar/rechazar con comentarios
   - Cambiar estado solicitud
   - Investigación de lógica agregación (P0-5): 8h

### ⚠️ RIESGOS CRÍTICOS - GAPS EN ANÁLISIS

**PROBLEMA IDENTIFICADO**: La lógica de agregación de aprobaciones **NO está clara** en el código legacy.

| Gap | Impacto | Decisión Temporal | Investigación |
|-----|---------|------------------|---------------|
| ¿Una aprobación basta (OR) o todas (AND)? | 🔴 CRÍTICA | Asumir AND (todas) hasta confirmar | P0-5.2: Entrevista stakeholder |
| ¿Cambio automático de estado o manual? | 🔴 CRÍTICA | Asumir automático cuando todas aprobadas | P0-5.2: Verificar BD producción |
| ¿Cómo se guarda comentario rechazo? | 🟠 ALTA | Campo `comentarios` en `GD_Revisiones` | P0-5.1: Verificar DDL |
| ¿Quién puede aprobar/rechazar? | 🟠 ALTA | Solo revisores asignados a solicitud | P0-5: Validar con lógica legacy |

**ACCIÓN INMEDIATA (P0-5 - Sprint 5, Tarea 5.1)**:
- Ejecutar queries en BD de staging para confirmar lógica
- Entrevista con Coordinador de Calidad
- Documentar en WORKFLOW_GD_APROBACIONES.md
- Actualizar esta sección con hallazgos

### Dependencias Críticas

✅ **COMPLETADAS en FASES 1-2**:
- Estructura MVC, Servicios, Adapters, Catálogos, Maestro, Repositorio

⚠️ **PENDIENTE - CRÍTICA**:
- **Investigación Workflow (P0-5)**: Debe completarse en Sprint 5, Tarea 5.1 ANTES de implementar aprobaciones

### Reglas Aplicables

| Regla | Descripción | Prioridad |
|-------|-------------|-----------|
| REGLA 2 | Mapear SP exactamente | 🔴 CRÍTICA |
| REGLA 4 | Ejecutar SP de WebMatrix | 🔴 CRÍTICA |
| REGLA 5.1 | UX AJAX-First (modales + JSON) | 🟠 ALTA |
| REGLA 6 | Paridad 1:1 (no nuevas features) | 🔴 CRÍTICA |
| REGLA 11 | Validar permisos usuario | 🔴 CRÍTICA |
| REGLA 14 | Usar async/await | 🟠 ALTA |

---

## 🚀 SPRINT 4: SOLICITUDES DE DOCUMENTOS

### Objetivo

Implementar creación de solicitudes con asignación de revisores y notificación por email.

**Horas Estimadas**: 24h  
**Duración**: 4-5 días  
**Criterio de Éxito**:
- ✅ CRUD completo de solicitudes
- ✅ Asignación de múltiples revisores (NO Session, POST body)
- ✅ Notificaciones email asíncrono (BackgroundService)
- ✅ 0 inconsistencias de datos
- ✅ Testing funcional

---

### TAREA 4.1: Mapear SP de Solicitudes (1h)

**Descripción**: Documentar SP para solicitudes y asignación de revisores

**Proceso**:

1. Buscar en `CoreProject/GD_Procedimientos.vb`:
   - Métodos relacionados a solicitudes (líneas ~100-120)
   - Métodos de revisiones (líneas ~231-265)

2. Documentar en MAPEO_SP_SOLICITUDES.csv:

| Método VB | SP Name | Parámetros | Retorna | Usado en |
|-----------|---------|-----------|---------|----------|
| `guardarSolicitud()` | `GD_SolDocumentos_Add` | tipoSolicitud, idDocumento, solicitante, area, cargo, razon, descripcion, estado, comentarios, [...] | ultimoId (int) | Crear solicitud |
| `guardarRevision()` | `GD_Revisiones_Add` | idDocumento, usuarioId, fechaAprobacion, tipoRevision | - | Asignar revisor |
| `ObtenerRevisionAprobarUsuario()` | `GD_Revisiones_GetRev` | idUsuario | DataTable | Listar pendientes |
| `ObtenerUsuarios()` | `GD_US_Usuarios_Get` | - | DataTable | Dropdown revisores |
| `ObtenerDocumentos()` | `GD_MaestroDocumentos_Get` | - | DataTable | Dropdown documentos |

3. Validar contra CO_Matrix_Structure_SP.sql

**Validación**:
- ✅ MAPEO_SP_SOLICITUDES.csv creado
- ✅ SP validados

---

### TAREA 4.2: Crear ViewModels Solicitudes (1.5h)

**Descripción**: ViewModels para solicitudes

**ViewModels**:

```csharp
public class SolicitudDocumentoVM
{
    public int Id { get; set; }
    public int TipoSolicitud { get; set; } // 1=Construcción, 2=Actualización, 3=Anulación
    public int IdDocumento { get; set; }
    [Required] public int IdSolicitante { get; set; }
    [Required] public string Area { get; set; }
    [Required] public string Cargo { get; set; }
    [Required] public string Razon { get; set; }
    [Required] public string Descripcion { get; set; }
    public int IdEstado { get; set; }
    public string Comentarios { get; set; }
    public DateTime FechaRegistro { get; set; }

    // Para visualización
    public string NombreDocumento { get; set; }
    public string NombreSolicitante { get; set; }
    public string NombreEstado { get; set; }
}

public class SolicitudCreateVM : SolicitudDocumentoVM
{
    public List<TipoSolicitudViewModel> TiposSolicitud { get; set; } = new();
    public List<MaestroListVM> Documentos { get; set; } = new();
    public List<UsuarioViewModel> Usuarios { get; set; } = new();
    public List<EstadoSolicitudViewModel> Estados { get; set; } = new();
}

public class AsignReviewersVM
{
    [Required(ErrorMessage = "Debe seleccionar al menos un revisor")]
    public List<int> IdRevisores { get; set; } = new();
    public List<UsuarioViewModel> RevisoresDisponibles { get; set; } = new();
    public string NombreSolicitud { get; set; }
}

public class SolicitudListVM
{
    public int Id { get; set; }
    public string NombreDocumento { get; set; }
    public string TipoSolicitud { get; set; }
    public string Solicitante { get; set; }
    public string Estado { get; set; }
    public int RevisoresPendientes { get; set; }
    public DateTime FechaRegistro { get; set; }
}

public class RevisorVM
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
}
```

**Validación**:
- ✅ ViewModels compilables
- ✅ Validaciones presentes

---

### TAREA 4.3: Expandir Adapter Solicitudes (2h)

**Descripción**: Métodos Dapper para solicitudes

**Interfaz**:

```csharp
public interface IGdSolicitudesAdapter
{
    Task<List<SolicitudListVM>> ObtenerSolicitudes();
    Task<SolicitudDocumentoVM> ObtenerSolicitudById(int id);
    Task<int> CrearSolicitud(SolicitudDocumentoVM vm);
    
    // Revisores
    Task<bool> AsignarRevisor(int idSolicitud, int idRevisor);
    Task<bool> CrearRevision(int idSolicitud, int idRevisor);
    Task<List<RevisorVM>> ObtenerRevisoresPendientes(int idSolicitud);
    
    // Dropdowns
    Task<List<MaestroListVM>> ObtenerDocumentos();
    Task<List<UsuarioViewModel>> ObtenerUsuarios();
    Task<List<EstadoSolicitudViewModel>> ObtenerEstados();
}
```

**Implementación clave** (AsignarRevisores):

```csharp
public async Task<bool> CrearRevision(int idSolicitud, int idRevisor)
{
    using (var connection = new SqlConnection(_connectionString))
    {
        var parameters = new DynamicParameters();
        parameters.Add("@idSolicitud", idSolicitud); // ⚠️ Mapear correctamente a parámetro SP
        parameters.Add("@idRevisor", idRevisor);
        parameters.Add("@fechaAprobacion", DateTime.UtcNow.AddHours(-5));
        parameters.Add("@tipoRevision", 1); // Construcción=1 (verificar valores)

        var result = await connection.ExecuteAsync(
            "GD_Revisiones_Add",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result > 0;
    }
}
```

**Validación**:
- ✅ Métodos implementados
- ✅ SP names validados

---

### TAREA 4.4: Expandir Service Solicitudes (2.5h)

**Descripción**: Lógica de negocio solicitudes

**Interfaz**:

```csharp
public interface IGdSolicitudesService
{
    Task<(bool success, List<SolicitudListVM> data)> ObtenerSolicitudes();
    Task<(bool success, SolicitudDocumentoVM data)> ObtenerSolicitudById(int id);
    Task<(bool success, int id, string message)> CrearSolicitud(SolicitudCreateVM vm);
    Task<(bool success, string message)> AsignarRevisores(int idSolicitud, List<int> idRevisores);
    Task<(bool success, SolicitudCreateVM formData)> ObtenerFormData();
}
```

**Implementación clave** (AsignarRevisores):

```csharp
public async Task<(bool success, string message)> AsignarRevisores(int idSolicitud, List<int> idRevisores)
{
    try
    {
        // REGLA 12: Validar entrada
        if (idRevisores == null || idRevisores.Count == 0)
            return (false, "Debe seleccionar al menos un revisor");

        // Crear transacción para múltiples revisores
        foreach (var idRevisor in idRevisores)
        {
            var result = await _adapter.CrearRevision(idSolicitud, idRevisor);
            if (!result)
                return (false, $"Error asignando revisor {idRevisor}");
        }

        // ⚠️ TODO: Enviar notificaciones email a revisores (P1-3)
        _logger.LogInformation($"Asignados {idRevisores.Count} revisores a solicitud {idSolicitud}");
        return (true, $"Asignados {idRevisores.Count} revisores exitosamente");
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error asignando revisores: {ex.Message}");
        return (false, $"Error: {ex.Message}");
    }
}
```

**⚠️ REGLA 6 - NO Session("Usuarios")**:

En WebMatrix, la lista de revisores está en `Session("Usuarios")` (anti-patrón).  
En MatrixNext, pasar array de IDs en POST body (REGLA 2 de DIRECTRICES).

**Validación**:
- ✅ Métodos implementados
- ✅ Validación entrada
- ✅ Sin Session

---

### TAREA 4.5: Crear SolicitudesController (3.5h)

**Descripción**: Controller para solicitudes

**Métodos**:

| Método | HTTP | URL | Parámetros | Retorna |
|--------|------|-----|-----------|---------|
| `Index` | GET | `/GD/Solicitudes` | - | View listado |
| `Create` | GET | `/GD/Solicitudes/Create` | - | PartialView modal |
| `Create` | POST | `/GD/Solicitudes/Create` | SolicitudCreateVM | JSON |
| `AssignReviewers` | GET | `/GD/Solicitudes/AssignReviewers/{id}` | int id | PartialView modal |
| `AssignReviewers` | POST | `/GD/Solicitudes/AssignReviewers/{id}` | int id, List<int> idRevisores | JSON |
| `GetFormData` | GET | `/GD/Solicitudes/GetFormData` | - | JSON (dropdowns) |

**Código Skeleton**:

```csharp
[Area("GD")]
[Authorize]
public class SolicitudesController : Controller
{
    private readonly IGdSolicitudesService _service;
    private readonly ILogger<SolicitudesController> _logger;

    public SolicitudesController(IGdSolicitudesService service, ILogger<SolicitudesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: /GD/Solicitudes
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Listado solicitudes");
        var (success, data) = await _service.ObtenerSolicitudes();
        return View(success ? data : new List<SolicitudListVM>());
    }

    // GET: /GD/Solicitudes/Create
    public async Task<IActionResult> Create()
    {
        var (success, formData) = await _service.ObtenerFormData();
        
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("_CreateSolicitudModal", formData);
        
        return View(formData);
    }

    // POST: /GD/Solicitudes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SolicitudCreateVM vm)
    {
        // REGLA 12: Validar entrada
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Datos incompletos");
            return Json(new { success = false, message = "Validación fallida" });
        }

        var (success, id, message) = await _service.CrearSolicitud(vm);

        if (success)
        {
            _logger.LogInformation($"Solicitud creada: {id}");
            return Json(new { success = true, id, message, redirectUrl = Url.Action("AssignReviewers", new { id }) });
        }

        return Json(new { success = false, message });
    }

    // GET: /GD/Solicitudes/AssignReviewers/{id}
    public async Task<IActionResult> AssignReviewers(int id)
    {
        var (success, solicitud) = await _service.ObtenerSolicitudById(id);
        if (!success)
            return NotFound();

        var (_, formData) = await _service.ObtenerFormData();
        var vm = new AsignReviewersVM
        {
            NombreSolicitud = solicitud.NombreDocumento,
            RevisoresDisponibles = formData.Usuarios
        };

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("_AssignReviewersModal", vm);
        
        return View(vm);
    }

    // POST: /GD/Solicitudes/AssignReviewers/{id}
    // ⚠️ CRÍTICO: Recibir array de IDs, NO Session
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignReviewers(int id, [FromBody] AsignReviewersVM vm)
    {
        // REGLA 12: Validar permisos
        var (success, message) = await _service.AsignarRevisores(id, vm.IdRevisores);

        if (success)
        {
            _logger.LogInformation($"Revisores asignados a {id}");
            return Json(new { success = true, message });
        }

        return Json(new { success = false, message });
    }

    // GET: /GD/Solicitudes/GetFormData
    [HttpGet]
    public async Task<IActionResult> GetFormData()
    {
        var (success, formData) = await _service.ObtenerFormData();
        return Json(new { success, data = formData });
    }
}
```

**Validación**:
- ✅ [Authorize] presente
- ✅ ModelState validado
- ✅ AJAX support
- ✅ ⚠️ NO Session, usar POST body para revisores

---

### TAREA 4.6: Crear Vistas Solicitudes (4h)

**Descripción**: Vistas para solicitudes

**Archivos**:

| Archivo | Contenido |
|---------|----------|
| `Index.cshtml` | Grid solicitudes (con estado, revisores pendientes) |
| `_CreateSolicitudModal.cshtml` | Form crear solicitud (3 tipos, condicional) |
| `_AssignReviewersModal.cshtml` | Selector múltiple de revisores (checklist) |

**Index.cshtml** (estructura):

```html
@model List<SolicitudListVM>

<div class="container-fluid mt-4">
    <h2>Solicitudes de Documentos</h2>
    
    <button class="btn btn-primary mb-3" id="btnCreateSolicitud">
        <i class="fas fa-plus"></i> Nueva Solicitud
    </button>

    <table class="table table-striped" id="tblSolicitudes">
        <thead>
            <tr>
                <th>Documento</th>
                <th>Tipo</th>
                <th>Solicitante</th>
                <th>Estado</th>
                <th>Revisores Pendientes</th>
                <th>Fecha</th>
                <th>Acciones</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var item in Model ?? new List<SolicitudListVM>())
            {
                <tr>
                    <td>@item.NombreDocumento</td>
                    <td><span class="badge bg-info">@item.TipoSolicitud</span></td>
                    <td>@item.Solicitante</td>
                    <td><span class="badge" 
                        style="background-color: @(item.Estado == "Aprobado" ? "green" : "orange")">
                        @item.Estado</span></td>
                    <td>@item.RevisoresPendientes</td>
                    <td>@item.FechaRegistro.ToString("dd/MM/yyyy")</td>
                    <td>
                        <button class="btn btn-sm btn-primary btnAssignReviewers" data-id="@item.Id">
                            <i class="fas fa-user-check"></i> Asignar
                        </button>
                        <button class="btn btn-sm btn-info btnDetails" data-id="@item.Id">
                            <i class="fas fa-eye"></i> Ver
                        </button>
                    </td>
                </tr>
            }
        </tbody>
    </table>
</div>

@section Scripts {
    <script>
        $(function() {
            $('#btnCreateSolicitud').click(function() {
                $.get('@Url.Action("Create")', function(html) {
                    $('#ajaxModal').html(html).modal('show');
                    
                    // Después de crear, mostrar modal asignar revisores
                    $('#frmCreateSolicitud').on('submit', function(e) {
                        e.preventDefault();
                        $.ajax({
                            type: 'POST',
                            url: '@Url.Action("Create")',
                            data: $(this).serialize(),
                            headers: { 'X-Requested-With': 'XMLHttpRequest' },
                            success: function(result) {
                                if (result.success) {
                                    $('#ajaxModal').modal('hide');
                                    // Mostrar modal asignar revisores
                                    $.get('@Url.Action("AssignReviewers")/' + result.id, function(html) {
                                        $('#ajaxModal').html(html).modal('show');
                                    });
                                }
                            }
                        });
                    });
                });
            });

            $(document).on('click', '.btnAssignReviewers', function() {
                var id = $(this).data('id');
                $.get('@Url.Action("AssignReviewers")/' + id, function(html) {
                    $('#ajaxModal').html(html).modal('show');
                });
            });
        });
    </script>
}
```

**_CreateSolicitudModal.cshtml**:
- Form con campos: TipoSolicitud, Documento, Solicitante, Area, Cargo, Razon, Descripcion
- JavaScript para mostrar/ocultar campos según tipo
- Submit creador solicitud

**_AssignReviewersModal.cshtml**:
- Selector múltiple (checkboxes) de revisores
- Validación: al menos 1 revisor
- Submit asigna revisores (POST JSON)

**Validación**:
- ✅ Vistas compilables
- ✅ Modales funcionales
- ✅ Flujo: Crear → Asignar Revisores
- ✅ Selector múltiple sin Session

---

### TAREA 4.7: Testing Solicitudes (1.5h)

**Descripción**: Validar solicitudes funcional

**Checklist**:

- [ ] Index carga sin errores
- [ ] Create modal abre
- [ ] Crear solicitud tipo Construcción
- [ ] Automáticamente se abre modal asignar revisores
- [ ] Seleccionar 3 revisores
- [ ] Revisor 1 + 2 + 3 insertados en BD
- [ ] Listado muestra solicitud con 3 revisores pendientes
- [ ] Validaciones por tipo funcionan
- [ ] Error si no selecciona revisores

**Validación**:
- ✅ 100% funcional
- ✅ Sin Session
- ✅ Flujo smooth

---

### Registro de Completitud - Sprint 4

| Tarea | Horas | Estado |
|-------|-------|--------|
| 4.1 Mapear SP solicitudes | 1h | ⏳ |
| 4.2 ViewModels solicitudes | 1.5h | ⏳ |
| 4.3 Adapter solicitudes | 2h | ⏳ |
| 4.4 Service solicitudes | 2.5h | ⏳ |
| 4.5 Controller solicitudes | 3.5h | ⏳ |
| 4.6 Vistas solicitudes | 4h | ⏳ |
| 4.7 Testing solicitudes | 1.5h | ⏳ |
| **TOTAL SPRINT 4** | **24h** | **⏳** |

---

## 🚀 SPRINT 5: APROBACIONES + INVESTIGACIÓN WORKFLOW

### Objetivo

Implementar aprobaciones/rechazos y **COMPLETAR investigación de lógica de workflow**.

**Horas Estimadas**: 28h (20h implementación + 8h investigación)  
**Duración**: 4-5 días  
**Criterio de Éxito**:
- ✅ Aprobaciones 100% funcional
- ✅ Investigación workflow completada
- ✅ Lógica agregación confirmada (AND vs OR)
- ✅ Testing completo
- ✅ Documento WORKFLOW_GD_APROBACIONES.md creado

---

### TAREA 5.1: INVESTIGACIÓN CRÍTICA - Lógica de Workflow (P0-5) (8h)

**Descripción**: BLOQUEANTE - Investigar y documentar exactamente cómo funciona el workflow de aprobaciones

**⚠️ PRIORIDAD MÁXIMA**: Esta tarea DEBE completarse antes de implementar AprobacionesController

**Subtareas**:

#### 5.1.1: Análisis BD Producción (2h)

**Objetivo**: Ejecutar queries en BD de staging para confirmar lógica

**Queries a Ejecutar**:

```sql
-- Query 1: Ver estructura de revisiones
SELECT * FROM GD_Revisiones
WHERE idSolicitud = <ejemplo> -- Usar solicitud real de testing
ORDER BY fechaAprobacion DESC;

-- Campos esperados: idSolicitud, idRevisor, estado, comentarios, tipoRevision, fechaAprobacion

-- Query 2: Ver cambio de estado de solicitud
SELECT id, estadoId, FechaRegistro, FechaModificacion
FROM GD_SolicitudDocumentos
WHERE id = <ejemplo>
ORDER BY FechaModificacion DESC;

-- Query 3: Ver patrón de aprobaciones (¿todas se requieren?)
SELECT 
    s.id as solicitudId,
    s.estadoId,
    COUNT(r.id) as totalRevisores,
    SUM(CASE WHEN r.estado = 'Aprobado' THEN 1 ELSE 0 END) as aprobados,
    SUM(CASE WHEN r.estado = 'Rechazado' THEN 1 ELSE 0 END) as rechazados
FROM GD_SolicitudDocumentos s
LEFT JOIN GD_Revisiones r ON s.id = r.idSolicitud
GROUP BY s.id, s.estadoId
HAVING SUM(CASE WHEN r.estado = 'Rechazado' THEN 1 ELSE 0 END) > 0
LIMIT 10;

-- Si hay rechazos: ¿cuántos aprobados vs rechazados? (pregunta AND vs OR)
```

**Documentar en CSV**:

```csv
Solicitud ID,Total Revisores,Aprobados,Rechazados,Estado Final,Observación
S001,3,3,0,Aprobado,"Todas aprobadas → estado Aprobado"
S002,3,2,1,Rechazado,"1 rechazo → estado Rechazado (confirma AND)"
S003,4,0,1,Rechazado,"0 aprobadas → estado Rechazado"
```

**Resultado esperado**: Patrón que responda:
- ¿Se requieren todas las aprobaciones (AND) o solo una (OR)?
- ¿Qué estado final corresponde a cada caso?
- ¿Se guarda comentario de rechazo?

#### 5.1.2: Revisión de Código Legacy (1.5h)

**Objetivo**: Examinar WebMatrix GD_Aprobaciones.aspx y métodos en CoreProject

**Archivos a Revisar**:

1. `WebMatrix/GD_Documentos/GD_Aprobaciones.aspx.vb`:
   - Método `btnAprobar_Click` - ¿qué hace?
   - Método `btnRechazar_Click` - ¿qué hace?
   - ¿Hay lógica de cambio de estado?

2. `CoreProject/GD_Procedimientos.vb`:
   - Método `editarRevision()` - ¿actualiza solo revisión o también solicitud?
   - Buscar cualquier método que actualice `GD_SolicitudDocumentos.estadoId`

**Documentar en ANALISIS_WORKFLOW_LEGACY.txt**:
```
Método: editarRevision()
Parámetros: [listar exactamente]
Acciones:
  1. Actualiza revisión (estado, comentarios, fecha)
  2. ¿Actualiza solicitud? SI/NO
  3. ¿Envía email? SI/NO (especificar cuándo)

Método: btnAprobar_Click
Flujo:
  1. Usuario selecciona revisión pendiente
  2. Llamar editarRevision(...)
  3. ¿Valida si todas aprobadas?
  4. ¿Cambia estado solicitud automáticamente?
  5. ¿Guarda comentarios de aprobación?
```

#### 5.1.3: Entrevista Stakeholder (2h)

**Objetivo**: Confirmar con Coordinador de Calidad

**Participante**: Coordinador de Calidad (debe estar en Sprint Planning)

**Preguntas a Hacer**:

1. **Lógica de Agregación**: "¿Se requiere que TODOS los revisores aprueben (AND) o puede estar aprobada con solo una aprobación (OR)?"
   - Respuesta esperada: AND (todas deben aprobar)
   - Alternativa: OR (con una basta)
   - Mixta: X de N revisores

2. **Cambio de Estado**: "¿Cuándo cambia el estado de la solicitud de 'En Revisión' a 'Aprobado'?"
   - Automático cuando última revisión se aprueba?
   - Manual por usuario especial?
   - Según cantidad de aprobaciones?

3. **Rechazo**: "Si un revisor rechaza, ¿qué sucede?"
   - ¿Se cancela todo el flujo inmediatamente?
   - ¿Pueden otros revisores seguir aprobando después?
   - ¿Quién puede enviar documento a revisión nuevamente?

4. **Comentarios**: "¿Se guardan comentarios de rechazo/aprobación?"
   - Dónde se guardan?
   - Campo específico en `GD_Revisiones`?

**Documentar en ENTREVISTA_WORKFLOW_<FECHA>.txt**:
```
Coordinador: [Nombre]
Fecha: [Fecha]

P1. Lógica agregación: RESPUESTA
P2. Cambio estado: RESPUESTA
P3. Rechazo: RESPUESTA
P4. Comentarios: RESPUESTA

Decisiones confirmadas:
- [1]
- [2]
- [3]
```

#### 5.1.4: Crear Documento WORKFLOW_GD_APROBACIONES.md (2.5h)

**Objetivo**: Especificación técnica completa de workflow

**Contenido Mínimo**:

```markdown
# Especificación Técnica - Workflow de Aprobaciones GD_Documentos

## Lógica Confirmada (Investigación P0-5)

### Tipo: AND - Todas las aprobaciones requeridas

Confirmado en: [Entrevista fecha], [Query BD resultado]

### Estados del Flujo

| Estado | Código | Descripción | Acciones Disponibles |
|--------|--------|-------------|----------------------|
| En Revisión | 1 | Solicitud creada, revisores asignados | Aprobar/Rechazar (revisores) |
| Aprobado | 2 | Todas las revisiones aprobadas | Descargar documento aprobado |
| Rechazado | 3 | Al menos 1 revisión rechazada | Enviar a revisión nuevamente |

### Cambio de Estado Automático

| Evento | Condición | Nuevo Estado | Quién |
|--------|-----------|--------------|-------|
| Última revisión aprobada | Todas las revisiones = Aprobado | Aprobado | Sistema (SP trigger?) |
| Primera revisión rechazada | Cualquier revisión = Rechazado | Rechazado | Sistema (SP trigger?) |

## Implementación en MatrixNext

### FlowChart

```
1. Solicitud Creada (Estado = 1 En Revisión)
   ↓
2. Revisores Asignados
   ├─ Revisor 1
   ├─ Revisor 2
   └─ Revisor 3
   ↓
3. Revisor 1 Aprueba → Revisión 1 = Aprobado
   ↓
4. Revisor 2 Rechaza → Solicitud = 3 Rechazado ❌ (FIN)
   
   O Si Revisor 2 Aprueba:
   ↓
5. Revisor 3 Aprueba → TODAS Aprobadas → Solicitud = 2 Aprobado ✅ (FIN)
```

### SP Trigger o SP Agregador?

¿GD_Revisiones_Edit automáticamente actualiza GD_SolicitudDocumentos?
O ¿Debo llamar a otro SP para cambiar estado?

Evidencia: [Query BD + análisis código legacy]

## Reglas de Validación

- [ ] Solo revisores asignados pueden aprobar/rechazar
- [ ] Revisor no puede aprobar su propia solicitud
- [ ] Comentario obligatorio en rechazo
- [ ] Comentario opcional en aprobación
- [ ] No puede cambiar aprobación después de enviar
```

**Validación**:
- ✅ Documento creado con hallazgos
- ✅ Todas las preguntas respondidas
- ✅ Lógica clara para implementación
- ✅ Enviado a Coordinador para confirmación final

---

### TAREA 5.2: Mapear SP de Aprobaciones (1h)

**Descripción**: Documentar SP para aprobaciones

**Proceso**:

1. Buscar en CoreProject (basado en resultado de 5.1.2)
2. Documentar MAPEO_SP_APROBACIONES.csv
3. Validar contra CO_Matrix_Structure_SP.sql

**SP Expected**:

| Método | SP Name | Parámetros | Retorna | Crítico |
|--------|---------|-----------|---------|---------|
| `ObtenerRevisionAprobarUsuario()` | `GD_Revisiones_GetRev` | idUsuario | DataTable | Sí |
| `editarRevision()` | `GD_Revisiones_Edit` | idRevision, estado, comentarios, [...] | - | 🔴 CRÍTICO |
| `ObtenerRevisionUsuario()` | `GD_Revisiones_Get` | idRevision | Row | Sí |
| `ActualizarSolicitud()` | `GD_SolicitudDocumentos_Update` (¿o trigger?) | idSolicitud, nuevoEstado | - | 🔴 CRÍTICO |

**Validación**:
- ✅ MAPEO_SP_APROBACIONES.csv creado
- ✅ ⚠️ Confirmar si SP trigger actualiza solicitud automáticamente

---

### TAREA 5.3: Crear ViewModels Aprobaciones (1.5h)

**Descripción**: ViewModels para aprobaciones

**ViewModels**:

```csharp
public class RevisionVM
{
    public int Id { get; set; }
    public int IdSolicitud { get; set; }
    public int IdRevisor { get; set; }
    public string NombreRevisor { get; set; }
    public string NombreDocumento { get; set; }
    public string NombreSolicitante { get; set; }
    public string EstadoRevision { get; set; } // Pendiente, Aprobado, Rechazado
    public string Comentarios { get; set; }
    public DateTime? FechaAprobacion { get; set; }
}

public class RevisionDetalleVM : RevisionVM
{
    public SolicitudDocumentoVM SolicitudDetalle { get; set; }
    public string DescripcionSolicitud { get; set; }
    [Required(ErrorMessage = "Comentarios requerido en rechazo")]
    public string ComentariosUsuario { get; set; }
}

public class AprobacionListVM
{
    public int Id { get; set; }
    public string NombreDocumento { get; set; }
    public string Solicitante { get; set; }
    public string Motivo { get; set; }
    public DateTime FechaRegistro { get; set; }
    public int RevisoresPendientes { get; set; }
    public int RevisoresAprobados { get; set; }
}

public class AprobacionRequestVM
{
    [Required] public int IdRevision { get; set; }
    public string Accion { get; set; } // "Aprobar" o "Rechazar"
    [MaxLength(500)] public string Comentarios { get; set; }
}
```

**Validación**:
- ✅ ViewModels compilables

---

### TAREA 5.4: Expandir Adapter Aprobaciones (2h)

**Descripción**: Métodos Dapper para aprobaciones

**Interfaz**:

```csharp
public interface IGdAprobacionesAdapter
{
    Task<List<AprobacionListVM>> ObtenerRevisionesPendientes(int idUsuario);
    Task<RevisionDetalleVM> ObtenerRevisionDetalle(int idRevision);
    Task<bool> ActualizarRevision(int idRevision, string estado, string comentarios);
    Task<bool> ActualizarEstadoSolicitud(int idSolicitud, string nuevoEstado);
    Task<int> ObtenerTotalRevisoresAprobados(int idSolicitud);
    Task<int> ObtenerTotalRevisores(int idSolicitud);
}
```

**Implementación clave**:

```csharp
public async Task<bool> ActualizarRevision(int idRevision, string estado, string comentarios)
{
    using (var connection = new SqlConnection(_connectionString))
    {
        var parameters = new DynamicParameters();
        parameters.Add("@idRevision", idRevision);
        parameters.Add("@estado", estado); // "Aprobado" o "Rechazado"
        parameters.Add("@comentarios", comentarios ?? "");
        parameters.Add("@fecha", DateTime.UtcNow.AddHours(-5));

        var result = await connection.ExecuteAsync(
            "GD_Revisiones_Edit",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result > 0;
    }
}

public async Task<int> ObtenerTotalRevisoresAprobados(int idSolicitud)
{
    using (var connection = new SqlConnection(_connectionString))
    {
        var sql = @"
            SELECT COUNT(*) as aprobados
            FROM GD_Revisiones
            WHERE idSolicitud = @idSolicitud AND estado = 'Aprobado'
        ";
        return await connection.QueryFirstOrDefaultAsync<int>(sql, new { idSolicitud });
    }
}
```

**Validación**:
- ✅ Métodos implementados
- ✅ Lógica para verificar si todas están aprobadas

---

### TAREA 5.5: Expandir Service Aprobaciones (2.5h)

**Descripción**: Lógica de negocio aprobaciones

**Interfaz**:

```csharp
public interface IGdAprobacionesService
{
    Task<(bool success, List<AprobacionListVM> data)> ObtenerRevisionesPendientes(int idUsuario);
    Task<(bool success, RevisionDetalleVM data)> ObtenerRevisionDetalle(int idRevision);
    Task<(bool success, string message)> AprobarRevision(int idRevision, string comentarios = "");
    Task<(bool success, string message)> RechazarRevision(int idRevision, string comentarios);
}
```

**Implementación clave** (lógica de agregación):

```csharp
public async Task<(bool success, string message)> AprobarRevision(int idRevision, string comentarios = "")
{
    try
    {
        var revision = await _adapter.ObtenerRevisionDetalle(idRevision);
        if (revision == null)
            return (false, "Revisión no encontrada");

        // REGLA 12: Validar que usuario es el revisor asignado
        if (revision.IdRevisor != _currentUser.Id)
            return (false, "No tienes permiso para aprobar esta revisión");

        // Actualizar revisión
        var updateResult = await _adapter.ActualizarRevision(
            idRevision, 
            "Aprobado", 
            comentarios);

        if (!updateResult)
            return (false, "Error actualizando revisión");

        // 🔴 LÓGICA CRÍTICA SEGÚN INVESTIGACIÓN:
        // Si TODAS las revisiones están aprobadas, cambiar estado solicitud
        var idSolicitud = revision.IdSolicitud;
        var totalRevisores = await _adapter.ObtenerTotalRevisores(idSolicitud);
        var aprobados = await _adapter.ObtenerTotalRevisoresAprobados(idSolicitud);

        if (aprobados == totalRevisores) // AND - todas aprobadas
        {
            var changeStateResult = await _adapter.ActualizarEstadoSolicitud(
                idSolicitud, 
                "Aprobado"); // Estado = 2

            if (!changeStateResult)
                _logger.LogWarning($"Advertencia: No se pudo cambiar estado de solicitud {idSolicitud}");
        }

        _logger.LogInformation($"Revisión aprobada: {idRevision}");
        return (true, "Documento aprobado exitosamente");
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error aprobando revisión: {ex.Message}");
        return (false, $"Error: {ex.Message}");
    }
}

public async Task<(bool success, string message)> RechazarRevision(int idRevision, string comentarios)
{
    try
    {
        // REGLA 12: Validar comentario obligatorio en rechazo
        if (string.IsNullOrWhiteSpace(comentarios))
            return (false, "Debe proporcionar motivo del rechazo");

        var revision = await _adapter.ObtenerRevisionDetalle(idRevision);
        if (revision == null)
            return (false, "Revisión no encontrada");

        if (revision.IdRevisor != _currentUser.Id)
            return (false, "No tienes permiso para rechazar esta revisión");

        // Actualizar revisión
        var updateResult = await _adapter.ActualizarRevision(
            idRevision, 
            "Rechazado", 
            comentarios);

        if (!updateResult)
            return (false, "Error actualizando revisión");

        // Cambiar estado solicitud a Rechazado inmediatamente
        var changeStateResult = await _adapter.ActualizarEstadoSolicitud(
            revision.IdSolicitud, 
            "Rechazado"); // Estado = 3

        _logger.LogInformation($"Revisión rechazada: {idRevision}");
        return (true, "Documento rechazado. El solicitante será notificado.");
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error rechazando revisión: {ex.Message}");
        return (false, $"Error: {ex.Message}");
    }
}
```

**Validación**:
- ✅ Lógica agregación AND implementada
- ✅ Cambio automático estado cuando todas aprobadas
- ✅ Cambio inmediato a Rechazado al primer rechazo
- ✅ Comentarios obligatorios en rechazo

---

### TAREA 5.6: Crear AprobacionesController (3h)

**Descripción**: Controller para aprobaciones

**Métodos**:

| Método | HTTP | URL | Parámetros | Retorna |
|--------|------|-----|-----------|---------|
| `Index` | GET | `/GD/Aprobaciones` | - | View listado |
| `Detail` | GET | `/GD/Aprobaciones/Detail/{id}` | int id | PartialView modal |
| `Approve` | POST | `/GD/Aprobaciones/Approve/{id}` | int id, string comentarios | JSON |
| `Reject` | POST | `/GD/Aprobaciones/Reject/{id}` | int id, string comentarios | JSON |

**Código Skeleton**:

```csharp
[Area("GD")]
[Authorize]
public class AprobacionesController : Controller
{
    private readonly IGdAprobacionesService _service;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<AprobacionesController> _logger;

    // GET: /GD/Aprobaciones
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation($"Listado aprobaciones para usuario {_currentUser.Id}");
        var (success, data) = await _service.ObtenerRevisionesPendientes(_currentUser.Id);
        return View(success ? data : new List<AprobacionListVM>());
    }

    // GET: /GD/Aprobaciones/Detail/{id}
    public async Task<IActionResult> Detail(int id)
    {
        var (success, data) = await _service.ObtenerRevisionDetalle(id);
        if (!success)
            return NotFound();

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("_ReviewDetailModal", data);
        
        return View(data);
    }

    // POST: /GD/Aprobaciones/Approve/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id, string comentarios = "")
    {
        var (success, message) = await _service.AprobarRevision(id, comentarios);

        if (success)
        {
            _logger.LogInformation($"Revisión aprobada por {_currentUser.Id}: {id}");
            return Json(new { success = true, message, redirectUrl = Url.Action("Index") });
        }

        return Json(new { success = false, message });
    }

    // POST: /GD/Aprobaciones/Reject/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id, [FromBody] AprobacionRequestVM vm)
    {
        // REGLA 12: Validar comentario obligatorio
        if (string.IsNullOrWhiteSpace(vm.Comentarios))
            return Json(new { success = false, message = "Debe proporcionar motivo del rechazo" });

        var (success, message) = await _service.RechazarRevision(id, vm.Comentarios);

        if (success)
        {
            _logger.LogInformation($"Revisión rechazada por {_currentUser.Id}: {id}");
            return Json(new { success = true, message, redirectUrl = Url.Action("Index") });
        }

        return Json(new { success = false, message });
    }
}
```

**Validación**:
- ✅ [Authorize] presente
- ✅ Validación permisos (solo revisor puede aprobar)
- ✅ Validación comentario en rechazo
- ✅ AJAX support
- ✅ Logging completo

---

### TAREA 5.7: Crear Vistas Aprobaciones (3.5h)

**Descripción**: Vistas para aprobaciones

**Archivos**:

| Archivo | Contenido |
|---------|----------|
| `Index.cshtml` | Grid con aprobaciones pendientes del usuario |
| `_ReviewDetailModal.cshtml` | Detalle solicitud + botones Aprobar/Rechazar |

**Index.cshtml** (estructura):

```html
@model List<AprobacionListVM>

<div class="container-fluid mt-4">
    <h2>Mis Revisiones Pendientes</h2>

    @if (!Model?.Any() ?? true)
    {
        <div class="alert alert-info">No hay revisiones pendientes</div>
    }
    else
    {
        <table class="table table-striped" id="tblAprobaciones">
            <thead>
                <tr>
                    <th>Documento</th>
                    <th>Solicitante</th>
                    <th>Motivo</th>
                    <th>Revisores Pendientes</th>
                    <th>Revisores Aprobados</th>
                    <th>Fecha Solicitud</th>
                    <th>Acciones</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var item in Model)
                {
                    <tr>
                        <td><strong>@item.NombreDocumento</strong></td>
                        <td>@item.Solicitante</td>
                        <td>@item.Motivo</td>
                        <td><span class="badge bg-warning">@item.RevisoresPendientes</span></td>
                        <td><span class="badge bg-success">@item.RevisoresAprobados</span></td>
                        <td>@item.FechaRegistro.ToString("dd/MM/yyyy")</td>
                        <td>
                            <button class="btn btn-sm btn-primary btnReview" data-id="@item.Id">
                                <i class="fas fa-eye"></i> Revisar
                            </button>
                        </td>
                    </tr>
                }
            </tbody>
        </table>
    }
</div>

@section Scripts {
    <script>
        $(function() {
            $(document).on('click', '.btnReview', function() {
                var id = $(this).data('id');
                $.get('@Url.Action("Detail")/' + id, function(html) {
                    $('#ajaxModal').html(html).modal('show');
                });
            });
        });
    </script>
}
```

**_ReviewDetailModal.cshtml**:
- Mostrar detalles de la solicitud (documento, solicitante, motivo, descripción)
- Historial de otras revisiones (quiénes aprobaron, quiénes pendientes)
- Textarea para comentarios
- Botones: Aprobar, Rechazar, Cerrar
- Validación: comentarios obligatorio en rechazo

**Validación**:
- ✅ Vistas compilables
- ✅ Detalle solicitud visible
- ✅ Historial revisores mostrado
- ✅ Botones Aprobar/Rechazar funcionales

---

### TAREA 5.8: Testing Aprobaciones (1h)

**Descripción**: Validar aprobaciones funcional

**Escenario 1: Aprobación exitosa (AND)**:

- [ ] Usuario 1 (Revisor) ve 1 aprobación pendiente
- [ ] Abre detalle, ve solicitud + otros revisores pendientes
- [ ] Aprueba sin comentarios
- [ ] Estado se actualiza a Aprobado localmente
- [ ] Usuario 2 (Revisor 2) ve 1 aprobación pendiente
- [ ] Aprueba sin comentarios
- [ ] Estado aún Pendiente (1 revisor falta)
- [ ] Usuario 3 (Revisor 3) ve 1 aprobación pendiente
- [ ] Aprueba sin comentarios
- [ ] Estado de solicitud cambia a APROBADO automáticamente

**Escenario 2: Rechazo**:

- [ ] Usuario 1 Rechaza con comentarios
- [ ] Estado de solicitud cambia a RECHAZADO inmediatamente
- [ ] Usuarios 2-3 ya NO ven aprobación pendiente

**Validación**:
- ✅ Lógica AND funcional
- ✅ Cambio estado automático
- ✅ Comentarios guardados
- ✅ Permisos validados

---

### Registro de Completitud - Sprint 5

| Tarea | Horas | Estado |
|-------|-------|--------|
| 5.1 Investigación Workflow (P0-5) | 8h | 🔴 BLOQUEANTE |
| 5.2 Mapear SP aprobaciones | 1h | ⏳ |
| 5.3 ViewModels aprobaciones | 1.5h | ⏳ |
| 5.4 Adapter aprobaciones | 2h | ⏳ |
| 5.5 Service aprobaciones | 2.5h | ⏳ |
| 5.6 Controller aprobaciones | 3h | ⏳ |
| 5.7 Vistas aprobaciones | 3.5h | ⏳ |
| 5.8 Testing aprobaciones | 1h | ⏳ |
| **TOTAL SPRINT 5** | **28h** | 🔴 **BLOQUEANTE EN 5.1** |

---

## ✅ CRITERIOS DE ÉXITO - FASE 3

**DEBE CUMPLIRSE ANTES DE PASAR A FASE 4**:

1. ✅ Investigación Workflow completada (5.1) - **CRÍTICA**
2. ✅ Solicitudes CRUD 100% funcional
3. ✅ Asignación múltiples revisores (sin Session)
4. ✅ Aprobaciones funcional con lógica AND
5. ✅ Cambio estado automático cuando todas aprobadas
6. ✅ Documento WORKFLOW_GD_APROBACIONES.md creado y aprobado
7. ✅ 0 errores de compilación
8. ✅ Commit de cambios

---

**Fin de FASE 3**

Próxima: [Crear FASE 4 - Email + Features Restantes]

