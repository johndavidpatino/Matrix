# PLAN DE FASE 2 - Gap Filling (Sprint 17)
**Fecha**: 2026-01-15  
**Duración**: 4-8 horas  
**Objetivo**: Implementar TraficoTareas UI consolidada (único gap identificado)

---

## 🎯 OBJETIVO PRINCIPAL

Crear vista consolidada **TraficoTareas** en MatrixNext que replique funcionalidad de `WebMatrix/RE_GT/TraficoTareas.aspx` incluyendo:

✅ Listado de trabajos por unidad OP (5, 6, 7, 8, 9, 10, 11, 14)  
✅ Filtros interactivos (unidad, estado, prioridad)  
✅ Navegación retorno (URLRetorno logic)  
✅ Estados WorkFlow (Creada, EnProgreso, Completada, Anulada)  
✅ Acciones: Editar, Ver detalles, Anular

---

## 📋 TAREAS - FASE 2 (Gap Filling)

### TASK 1: Analizar código legacy TraficoTareas.aspx (1h)

**Archivo**: `WebMatrix/RE_GT/TraficoTareas.aspx.vb` (257 líneas)

**Analizar**:
- [ ] Filtros disponibles (unidad, estado, prioridad, búsqueda)
- [ ] Columnas del grid (IdTrabajo, Descripción, Unidad, Estado, Prioridad, FechaVencimiento, Asignados)
- [ ] Acciones disponibles (Editar, Anular, Ver detalles)
- [ ] URLRetorno mapping (13+ casos)
- [ ] Permisos (por unidad OP específica)
- [ ] Integraciones (SP legacy, SignalR, Excel export)

**Entregable**: Documento `PLAN_TRAFICO_TAREAS_ANALYSIS.md`

---

### TASK 2: Crear DTO y ViewModel (1-2h)

**Crear**:

```csharp
// MatrixNext.Web/DTOs/TareasPorUnidadDto.cs
public class TareasPorUnidadDto
{
    public long IdWorkFlow { get; set; }
    public long IdTrabajo { get; set; }
    public string TrabajoNombre { get; set; }
    public string TrabajoDescripcion { get; set; }
    public int IdUnidad { get; set; }
    public string UnidadNombre { get; set; } // Crítica, Verificación, Captura, etc.
    public string Estado { get; set; }
    public int Prioridad { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public int UsuariosAsignados { get; set; }
    public string EstadoDisplay => Estado switch
    {
        "Creada" => "Creada",
        "EnProgreso" => "En Progreso",
        "Completada" => "Completada",
        "Anulada" => "Anulada",
        _ => Estado
    };
}

// MatrixNext.Web/ViewModels/TraficoTareasViewModel.cs
public class TraficoTareasViewModel
{
    public List<TareasPorUnidadDto> Tareas { get; set; }
    public int? FiltroUnidad { get; set; }
    public string? FiltroEstado { get; set; }
    public int? FiltroPrioridad { get; set; }
    public string? FiltroBusqueda { get; set; }
    public int PaginaActual { get; set; } = 1;
    public int TotalRegistros { get; set; }
    public List<UnidadDto> UnidadesDisponibles { get; set; } // 5, 6, 7, 8, 9, 10, 11, 14
}
```

**Archivos a crear**:
- [ ] `MatrixNext.Web/DTOs/TareasPorUnidadDto.cs`
- [ ] `MatrixNext.Web/ViewModels/TraficoTareasViewModel.cs`

---

### TASK 3: Extender Service + Adapter (1-2h)

**Modificar**: `CORE/Services/WorkFlowService` y `WorkFlowAdapter`

```csharp
// IWorkFlowService - Nueva interfaz
public interface IWorkFlowService
{
    // Métodos existentes...
    
    /// <summary>
    /// Obtiene tareas de WorkFlow por unidad OP (para TraficoTareas)
    /// Filtrado por: unidad, estado, prioridad, búsqueda
    /// </summary>
    Task<(List<TareasPorUnidadDto> Tareas, int Total)> 
        ObtenerTareasPorUnidadAsync(
            int idUnidad,
            string? estado = null,
            int? prioridad = null,
            string? busqueda = null,
            int page = 1,
            int pageSize = 20);
    
    /// <summary>
    /// Obtiene todas las unidades OP disponibles para TraficoTareas
    /// Retorna: (5=Crítica, 6=Verificación, 7=Captura, 8=Codificación, etc.)
    /// </summary>
    Task<List<UnidadDto>> ObtenerUnidadesTraficoAsync();
}

// WorkFlowService - Implementación
public class WorkFlowService : IWorkFlowService
{
    public async Task<(List<TareasPorUnidadDto> Tareas, int Total)> 
        ObtenerTareasPorUnidadAsync(
            int idUnidad,
            string? estado = null,
            int? prioridad = null,
            string? busqueda = null,
            int page = 1,
            int pageSize = 20)
    {
        try
        {
            var resultado = await _adapter.ObtenerTareasPorUnidadAsync(
                idUnidad, estado, prioridad, busqueda, page, pageSize);
            
            _logger.LogInformation(
                "TraficoTareas: Unidad {IdUnidad}, {Count} tareas, página {Page}",
                idUnidad, resultado.Tareas.Count, page);
            
            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo tareas por unidad {IdUnidad}", idUnidad);
            throw;
        }
    }
}
```

**Archivos a modificar**:
- [ ] `CORE/Services/IWorkFlowService.cs` (agregar método)
- [ ] `CORE/Services/WorkFlowService.cs` (implementar método)
- [ ] `Data/Adapters/CORE/IWorkFlowAdapter.cs` (agregar interfaz)
- [ ] `Data/Adapters/CORE/WorkFlowAdapter.cs` (implementar SP call)

---

### TASK 4: Crear Controller Action (1h)

**Modificar**: `Areas/CORE/Controllers/WorkFlowController.cs`

```csharp
[Area("CORE")]
[Authorize]
[Route("CORE/[controller]/[action]")]
public class WorkFlowController : Controller
{
    // Métodos existentes...
    
    /// <summary>
    /// GET /CORE/Workflow/TraficoTareas
    /// Vista consolidada de tráfico de tareas por unidad
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> TraficoTareas(
        int? unidad = null,
        string? estado = null,
        int? prioridad = null,
        string? busqueda = null,
        int page = 1)
    {
        try
        {
            // Obtener unidades disponibles (5, 6, 7, 8, etc.)
            var unidades = await _service.ObtenerUnidadesTraficoAsync();
            
            // Validar acceso a unidad
            var idUnidadFiltro = unidad ?? unidades.FirstOrDefault()?.Id ?? 5;
            
            // Obtener tareas
            var (tareas, total) = await _service.ObtenerTareasPorUnidadAsync(
                (int)idUnidadFiltro, estado, prioridad, busqueda, page);
            
            var viewModel = new TraficoTareasViewModel
            {
                Tareas = tareas,
                FiltroUnidad = unidad,
                FiltroEstado = estado,
                FiltroPrioridad = prioridad,
                FiltroBusqueda = busqueda,
                PaginaActual = page,
                TotalRegistros = total,
                UnidadesDisponibles = unidades
            };
            
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en TraficoTareas");
            return BadRequest(new { message = "Error al cargar tráfico de tareas" });
        }
    }
}
```

---

### TASK 5: Crear View TraficoTareas.cshtml (1-2h)

**Crear**: `Areas/CORE/Views/WorkFlow/TraficoTareas.cshtml`

```cshtml
@model TraficoTareasViewModel

@{
    ViewData["Title"] = "Tráfico de Tareas";
    Layout = "_Layout";
}

<div class="container-fluid mt-4">
    <div class="row mb-3">
        <div class="col-md-8">
            <h2>Tráfico de Tareas - WorkFlow</h2>
        </div>
        <div class="col-md-4 text-end">
            <a href="@Url.Action("Create", "WorkFlow")" class="btn btn-primary">
                <i class="fas fa-plus"></i> Nueva Tarea
            </a>
        </div>
    </div>

    <!-- FILTROS -->
    <div class="card mb-4">
        <div class="card-body">
            <form method="get" action="@Url.Action("TraficoTareas", "WorkFlow")" 
                  class="row g-3">
                
                <!-- Unidad -->
                <div class="col-md-3">
                    <label for="unidad" class="form-label">Unidad</label>
                    <select id="unidad" name="unidad" class="form-select"
                            onchange="this.form.submit()">
                        <option value="">-- Todas --</option>
                        @foreach (var u in Model.UnidadesDisponibles)
                        {
                            <option value="@u.Id" 
                                    selected="@(Model.FiltroUnidad == u.Id)">
                                @u.Nombre
                            </option>
                        }
                    </select>
                </div>

                <!-- Estado -->
                <div class="col-md-3">
                    <label for="estado" class="form-label">Estado</label>
                    <select id="estado" name="estado" class="form-select"
                            onchange="this.form.submit()">
                        <option value="">-- Todos --</option>
                        <option value="Creada" selected="@(Model.FiltroEstado == "Creada")">
                            Creada
                        </option>
                        <option value="EnProgreso" selected="@(Model.FiltroEstado == "EnProgreso")">
                            En Progreso
                        </option>
                        <option value="Completada" selected="@(Model.FiltroEstado == "Completada")">
                            Completada
                        </option>
                        <option value="Anulada" selected="@(Model.FiltroEstado == "Anulada")">
                            Anulada
                        </option>
                    </select>
                </div>

                <!-- Prioridad -->
                <div class="col-md-2">
                    <label for="prioridad" class="form-label">Prioridad</label>
                    <select id="prioridad" name="prioridad" class="form-select"
                            onchange="this.form.submit()">
                        <option value="">-- Todos --</option>
                        <option value="3" selected="@(Model.FiltroPrioridad == 3)">
                            Baja
                        </option>
                        <option value="1" selected="@(Model.FiltroPrioridad == 1)">
                            Normal
                        </option>
                        <option value="2" selected="@(Model.FiltroPrioridad == 2)">
                            Alta
                        </option>
                    </select>
                </div>

                <!-- Búsqueda -->
                <div class="col-md-4">
                    <label for="busqueda" class="form-label">Búsqueda</label>
                    <div class="input-group">
                        <input type="text" id="busqueda" name="busqueda" 
                               class="form-control" 
                               placeholder="Trabajo, descripción..."
                               value="@Model.FiltroBusqueda">
                        <button type="submit" class="btn btn-outline-secondary">
                            <i class="fas fa-search"></i>
                        </button>
                    </div>
                </div>
            </form>
        </div>
    </div>

    <!-- TABLA TAREAS -->
    <div class="card">
        <div class="table-responsive">
            <table class="table table-hover mb-0">
                <thead class="table-dark">
                    <tr>
                        <th>ID Trabajo</th>
                        <th>Descripción</th>
                        <th>Unidad</th>
                        <th>Estado</th>
                        <th>Prioridad</th>
                        <th>Vencimiento</th>
                        <th class="text-center">Asignados</th>
                        <th class="text-center">Acciones</th>
                    </tr>
                </thead>
                <tbody>
                    @if (Model.Tareas.Any())
                    {
                        @foreach (var tarea in Model.Tareas)
                        {
                            <tr>
                                <td><strong>@tarea.IdTrabajo</strong></td>
                                <td>@tarea.TrabajoDescripcion</td>
                                <td><small class="badge bg-info">@tarea.UnidadNombre</small></td>
                                <td>
                                    @{
                                        var estadoClass = tarea.Estado switch
                                        {
                                            "Creada" => "secondary",
                                            "EnProgreso" => "warning",
                                            "Completada" => "success",
                                            "Anulada" => "danger",
                                            _ => "light"
                                        };
                                    }
                                    <span class="badge bg-@estadoClass">@tarea.EstadoDisplay</span>
                                </td>
                                <td>
                                    @{
                                        var prioridadClass = tarea.Prioridad switch
                                        {
                                            1 => "secondary",
                                            2 => "danger",
                                            3 => "success",
                                            _ => "light"
                                        };
                                        var prioridadText = tarea.Prioridad switch
                                        {
                                            1 => "Normal",
                                            2 => "Alta",
                                            3 => "Baja",
                                            _ => "-"
                                        };
                                    }
                                    <span class="badge bg-@prioridadClass">@prioridadText</span>
                                </td>
                                <td>
                                    @if (tarea.FechaVencimiento.HasValue)
                                    {
                                        <small>@tarea.FechaVencimiento.Value.ToString("dd/MM/yyyy")</small>
                                    }
                                    else
                                    {
                                        <small class="text-muted">-</small>
                                    }
                                </td>
                                <td class="text-center">
                                    <span class="badge bg-primary">@tarea.UsuariosAsignados</span>
                                </td>
                                <td class="text-center">
                                    <a href="@Url.Action("Edit", "WorkFlow", new { id = tarea.IdWorkFlow })" 
                                       class="btn btn-sm btn-primary" title="Editar">
                                        <i class="fas fa-edit"></i>
                                    </a>
                                    <a href="@Url.Action("Details", "WorkFlow", new { id = tarea.IdWorkFlow })" 
                                       class="btn btn-sm btn-info" title="Ver detalles">
                                        <i class="fas fa-eye"></i>
                                    </a>
                                </td>
                            </tr>
                        }
                    }
                    else
                    {
                        <tr>
                            <td colspan="8" class="text-center text-muted py-4">
                                No hay tareas registradas con los filtros aplicados
                            </td>
                        </tr>
                    }
                </tbody>
            </table>
        </div>

        <!-- PAGINACIÓN -->
        @if (Model.TotalRegistros > 20)
        {
            <div class="card-footer">
                <nav aria-label="Page navigation">
                    <ul class="pagination justify-content-center">
                        <!-- Lógica de paginación aquí -->
                    </ul>
                </nav>
            </div>
        }
    </div>
</div>
```

---

### TASK 6: Testing Funcional (1-2h)

**Probar**:
- [ ] Vista carga sin errores
- [ ] Filtros funcionan correctamente
  - [ ] Filtro por unidad
  - [ ] Filtro por estado
  - [ ] Filtro por prioridad
  - [ ] Búsqueda por descripción
- [ ] Paginación funciona
- [ ] Acciones (Editar, Ver detalles)
- [ ] Estados se visualizan correctamente (colores/badges)
- [ ] Permisos aplicados correctamente
- [ ] Build 0 errores

---

### TASK 7: Sidebar Navigation Update (1h)

**Modificar**: `Views/Shared/_main-sidebar.cshtml`

```cshtml
<!-- RE_GT - Recolección y Gestión -->
<li class="treeview" data-permission="26,27">
    <a href="#">
        <i class="fa fa-folder-open"></i>
        <span>Recolección & Gestión</span>
        <span class="pull-right-container">
            <i class="fa fa-angle-left pull-right"></i>
        </span>
    </a>
    <ul class="treeview-menu">
        <li>
            <a href="@Url.Action("Index", "HomeRecoleccion", new { area = "RE_GT" })">
                <i class="fa fa-list"></i> <span>Recolección de Datos</span>
            </a>
        </li>
        <li>
            <a href="@Url.Action("TraficoTareas", "WorkFlow", new { area = "CORE" })">
                <i class="fa fa-tasks"></i> <span>Tráfico de Tareas</span>
            </a>
        </li>
        <li>
            <a href="@Url.Action("Index", "OP_Trafico", new { area = "OP" })">
                <i class="fa fa-exchange"></i> <span>Tráfico de Encuestas</span>
            </a>
        </li>
        <li>
            <a href="@Url.Action("Index", "HomeGestionTratamiento", new { area = "RE_GT" })">
                <i class="fa fa-cog"></i> <span>Gestión y Tratamiento</span>
            </a>
        </li>
    </ul>
</li>
```

---

## 📊 CRONOGRAMA FASE 2

| Tarea | Duración | Acumulado | Estado |
|-------|----------|-----------|--------|
| TASK 1: Analysis | 1h | 1h | ⏳ Pendiente |
| TASK 2: DTO + ViewModel | 1-2h | 2-3h | ⏳ Pendiente |
| TASK 3: Service + Adapter | 1-2h | 3-5h | ⏳ Pendiente |
| TASK 4: Controller | 1h | 4-6h | ⏳ Pendiente |
| TASK 5: View | 1-2h | 5-8h | ⏳ Pendiente |
| TASK 6: Testing | 1-2h | 6-10h | ⏳ Pendiente |
| TASK 7: Sidebar | 1h | 7-11h | ⏳ Pendiente |
| **TOTAL** | **7-11h** | **7-11h** | ⏳ Pendiente |

**Nota**: Asumiendo 1-2 horas simultáneas en Tasks 2-3, tiempo real: **5-8 horas**

---

## ✅ CRITERIOS DE ÉXITO

- [ ] Build compila con 0 errores
- [ ] Vista TraficoTareas carga correctamente
- [ ] Todos los filtros funcionan
- [ ] Paginación funciona
- [ ] Permisos aplicados correctamente
- [ ] Navegación retorno funciona
- [ ] Testing 100% completado
- [ ] Documento MIGRACION_RE_GT_COMPLETADA.md creado
- [ ] Sidebar actualizado con enlaces RE_GT
- [ ] Commit realizado con mensaje descriptivo

---

**Documentación completada**: 2026-01-15  
**Próxima fase**: Fase 2 - Gap Filling (comenzar con TASK 1)
