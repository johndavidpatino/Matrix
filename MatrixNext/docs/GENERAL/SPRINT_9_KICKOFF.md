# SPRINT 9 - Home Dashboard (Kickoff Guide)

**Fecha Inicio**: 2026-02-22  
**Fecha Fin**: 2026-03-05  
**Duración**: 1-2 semanas  
**Esfuerzo Estimado**: 50 horas  
**Prioridad**: 🟠 ALTA (Depende de Sprints 7+8)  
**Estado**: 🟡 EN CURSO

---

## 📋 Objetivo Sprint 9

Completar el **Home Dashboard** (página de inicio) con:
- ✅ Agregación de datos de múltiples módulos (TH, PY, EQ, CORE, GD, OP)
- ✅ Widgets contextuales (Tareas, Proyectos, Cotizaciones, Ausencias, Documentos)
- ✅ Métricas y KPIs (últimos 30 días)
- ✅ Performance objetivo: < 2 segundos carga inicial

**Integración crítica**:
- Tareas desde Sprint 7 (CORE Workflow)
- Cotizaciones desde Sprint 8 (EQ_EasyQuote)
- Ausencias desde Sprint 5 (TH_TalentoHumano)

---

## 🎯 Scope - Tareas Principales

### 1. Completar HomeController (8h)

**Ubicación**: `MatrixNext.Web/Controllers/HomeController.cs`

**Estado actual**: ✅ 80% completo
```csharp
public class HomeController : Controller
{
    private readonly IDashboardService _dashboardService;
    
    public async Task<IActionResult> Index()
    {
        // ✅ Obtener dashboard de servicio
        var dashboard = await _dashboardService.GetDashboardAsync(userId);
        return View(dashboard);
    }
    
    public async Task<IActionResult> RefreshDashboard()
    {
        // ✅ AJAX refresh sin recargar página
    }
    
    public async Task<IActionResult> Widget(string widgetName)
    {
        // ✅ AJAX para cargar widget individual
    }
}
```

**Tareas**:
- [x] Existe structure base
- [ ] Completar Widget() endpoint
- [ ] Validar permisos por rol
- [ ] Logging completo
- [ ] Error handling graceful

### 2. Completar DashboardService (12h)

**Ubicación**: `MatrixNext.Web/Services/Dashboard/DashboardService.cs`

**Estado actual**: ✅ 70% completo

**Métodos que deben estar implementados**:
```csharp
public interface IDashboardService
{
    // ✅ Ya existe - Retorna DashboardViewModel con todos los widgets
    Task<DashboardViewModel> GetDashboardAsync(string userId);
    
    // ✅ Ya existe - Obtiene tareas pendientes desde CORE
    Task<List<TaskSummary>> GetPendingTasksAsync(string userId);
    
    // ✅ Ya existe - Proyectos activos desde PY
    Task<List<ProjectSummary>> GetActiveProjectsAsync(string userId);
    
    // ⚠️ Verificar - Cotizaciones recientes desde EQ
    Task<List<QuoteSummary>> GetRecentQuotesAsync(string userId);
    
    // ⚠️ Verificar - Ausencias próximas desde TH
    Task<List<AbsenceSummary>> GetUpcomingAbsencesAsync(string userId);
    
    // ⚠️ Verificar - Estadísticas de documentos desde GD
    Task<DocumentStatistics> GetDocumentStatsAsync(string userId);
    
    // ⚠️ Verificar - Métricas de producción/ventas
    Task<ProductionMetrics> GetProductionMetricsAsync();
}
```

**Tareas**:
- [ ] Revisar e implementar QuoteSummary (EQ)
- [ ] Revisar e implementar AbsenceSummary (TH)
- [ ] Revisar e implementar DocumentStatistics (GD)
- [ ] Revisar e implementar ProductionMetrics
- [ ] Implementar caché con IMemoryCache (15 min)
- [ ] Validar queries con índices en BD

### 3. Crear/Actualizar Views (15h)

**Ubicación**: `MatrixNext.Web/Views/Home/`

**Archivos requeridos**:

#### a) `Index.cshtml` - Dashboard principal
```html
<!-- ✅ Probablemente ya existe -->
@model DashboardViewModel

<div class="dashboard-container">
    <!-- Widget 1: Tareas Pendientes -->
    <div class="widget widget-tasks" data-widget="pending-tasks">
        <h3>📋 Mis tareas (CORE)</h3>
        @foreach(var task in Model.PendingTasks)
        {
            <div class="task-item">
                <span>@task.Titulo</span>
                <span class="badge">@task.Prioridad</span>
            </div>
        }
        <a href="/CORE/Tareas">Ver todas →</a>
    </div>

    <!-- Widget 2: Proyectos Activos -->
    <div class="widget widget-projects" data-widget="active-projects">
        <h3>📊 Proyectos activos (PY)</h3>
        <table>
            <tr>
                <th>Nombre</th>
                <th>Estado</th>
                <th>Progreso</th>
            </tr>
            @foreach(var project in Model.ActiveProjects)
            {
                <tr>
                    <td>@project.Nombre</td>
                    <td>@project.Estado</td>
                    <td><progress value="@project.PorcentajeAvance" max="100"></progress></td>
                </tr>
            }
        </table>
    </div>

    <!-- Widget 3: Cotizaciones Recientes -->
    <div class="widget widget-quotes" data-widget="recent-quotes">
        <h3>💰 Cotizaciones (EQ)</h3>
        @foreach(var quote in Model.RecentQuotes)
        {
            <div class="quote-item">
                <span>@quote.Cliente - @quote.Monto.ToString("C")</span>
                <span class="badge">@quote.Estado</span>
            </div>
        }
    </div>

    <!-- Widget 4: Ausencias Próximas -->
    <div class="widget widget-absences" data-widget="upcoming-absences">
        <h3>📅 Ausencias próximas (TH)</h3>
        @foreach(var absence in Model.UpcomingAbsences)
        {
            <div class="absence-item">
                <span>@absence.Empleado - @absence.FechaInicio</span>
                <span>@absence.Dias días</span>
            </div>
        }
    </div>

    <!-- Widget 5: Documentos Pendientes -->
    <div class="widget widget-documents" data-widget="document-stats">
        <h3>📄 Documentos (GD)</h3>
        <div>Pendientes de aprobación: @Model.DocumentStats.PendingCount</div>
        <div>Rechazados: @Model.DocumentStats.RejectedCount</div>
    </div>

    <!-- Widget 6: Métricas de Producción -->
    <div class="widget widget-metrics" data-widget="production-metrics">
        <h3>📈 Producción (últimos 30 días)</h3>
        <canvas id="productionChart"></canvas>
    </div>
</div>
```

**Tareas**:
- [ ] Revisar si existe `Index.cshtml`
- [ ] Completar widgets faltantes
- [ ] Agregar estilos CSS (responsive, dark mode)
- [ ] Agregar charts (Chart.js para gráficos)
- [ ] Implementar skeleton loading (mientras carga)

#### b) `Shared/_DashboardWidgets.cshtml` - Componentes reutilizables
```html
@* Widget template *@
<div class="widget @Model.WidgetClass">
    <div class="widget-header">
        <h4>@Model.Title</h4>
        <button class="widget-refresh" data-widget="@Model.WidgetName">
            <i class="fas fa-sync"></i>
        </button>
    </div>
    <div class="widget-body">
        @RenderBody()
    </div>
</div>
```

#### c) `_Analytics.cshtml` - Gráficos y métricas
```html
<!-- Chart.js para gráficos interactivos -->
<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

<div class="analytics-container">
    <canvas id="productionChart"></canvas>
    <canvas id="quotesChart"></canvas>
    <canvas id="tasksChart"></canvas>
</div>

<script>
// Gráficos de producción, cotizaciones, tareas (Chart.js)
</script>
```

### 4. Crear/Actualizar ViewModels (8h)

**Ubicación**: `MatrixNext.Web/Models/DashboardViewModel.cs`

**Clases requeridas**:
```csharp
public class DashboardViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string NombreCompleto { get; set; }
    
    // Widget data
    public List<TaskSummary> PendingTasks { get; set; }
    public List<ProjectSummary> ActiveProjects { get; set; }
    public List<QuoteSummary> RecentQuotes { get; set; }
    public List<AbsenceSummary> UpcomingAbsences { get; set; }
    public DocumentStatistics DocumentStats { get; set; }
    public ProductionMetrics ProductionMetrics { get; set; }
    
    public DateTime LoadedAt { get; set; }
    public string Error { get; set; }
}

public class TaskSummary
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Estado { get; set; }
    public string Prioridad { get; set; }
    public DateTime FechaVencimiento { get; set; }
}

public class QuoteSummary
{
    public int Id { get; set; }
    public string Cliente { get; set; }
    public decimal Monto { get; set; }
    public string Estado { get; set; }
    public DateTime FechaCreacion { get; set; }
}

public class AbsenceSummary
{
    public int Id { get; set; }
    public string Empleado { get; set; }
    public string TipoAusencia { get; set; }
    public DateTime FechaInicio { get; set; }
    public int Dias { get; set; }
}

public class DocumentStatistics
{
    public int PendingCount { get; set; }
    public int RejectedCount { get; set; }
    public int ApprovedCount { get; set; }
}

public class ProductionMetrics
{
    public decimal TotalVentas { get; set; }
    public int ProyectosActivos { get; set; }
    public int TareasVencidas { get; set; }
    public List<MetricPoint> SalesLast30Days { get; set; }
}
```

**Tareas**:
- [ ] Revisar si ViewModels ya existen
- [ ] Crear/completar clases faltantes
- [ ] Agregar validaciones
- [ ] Documentar propiedades

### 5. JavaScript + AJAX (5h)

**Ubicación**: `MatrixNext.Web/wwwroot/js/dashboard.js`

```javascript
// ✅ Funcionalidades requeridas:
// 1. Auto-refresh widgets cada 5 minutos
// 2. Manual refresh al hacer clic
// 3. Skeleton loading mientras carga
// 4. Manejo de errores con toasts
// 5. Lazy loading de widgets fuera del viewport
```

**Tareas**:
- [ ] Crear `dashboard.js` si no existe
- [ ] Implementar widget refresh (AJAX)
- [ ] Implementar auto-refresh (5 min)
- [ ] Agregar animaciones smooth
- [ ] Validar en todos los navegadores

### 6. CSS + Responsive Design (2h)

**Ubicación**: `MatrixNext.Web/wwwroot/css/dashboard.css`

**Requisitos**:
- Responsive (Mobile, Tablet, Desktop)
- Dark mode compatible
- Accesibilidad (WCAG AA)
- Performance (< 100KB CSS)

**Tareas**:
- [ ] Crear `dashboard.css` con grid layout
- [ ] Media queries para mobile-first
- [ ] Dark mode variables
- [ ] Animations suave

---

## 📊 CHECKLIST PRE-EJECUCIÓN

**Dependencias verificadas**:
- [x] Sprint 7 (CORE Workflow) - Tareas, estados, SignalR
- [x] Sprint 8 (EQ_EasyQuote) - Motor de cálculos, costos
- [x] Sprint 5 (TH_TalentoHumano) - Ausencias, empleados
- [ ] Verificar que todas las tablas existan en BD
- [ ] Verificar que todos los servicios estén registrados en DI (Program.cs)
- [ ] Verificar permisos de acceso a datos por rol

**Build status**:
- [ ] Compilación exitosa (0 errores)
- [ ] 0 warnings críticos
- [ ] Tests unitarios pasen

**Performance**:
- [ ] Dashboard carga en < 2 seg
- [ ] Widgets individuales < 500ms
- [ ] Cache implementado (15 min)

---

## 🚀 PLAN DE EJECUCIÓN

### Día 1 (Lunes): Análisis + HomeController (8h)
1. Revisar código existente (HomeController, DashboardService, Views)
2. Identificar gaps
3. Completar HomeController
4. Compilación verificada

### Día 2-3 (Martes-Miércoles): DashboardService (12h)
1. Revisar/completar métodos de agregación
2. Implementar queries a cada módulo
3. Validar datos retornados
4. Implementar caching

### Día 4 (Jueves): Views + ViewModels (10h)
1. Crear/completar `Index.cshtml`
2. Crear componentes reutilizables
3. Agregar estilos básicos
4. Verificación visual

### Día 5 (Viernes): JavaScript + Testing (7h)
1. Crear `dashboard.js` con AJAX
2. Implementar auto-refresh
3. Testing funcional (manual)
4. Ajustes finales

### Fin de semana: Pulido + Commit (5h)
1. Performance tuning
2. Validar en múltiples navegadores
3. Documentation
4. Git commit final

---

## ✅ CRITERIOS DE ACEPTACIÓN

- [x] HomeController endpoint `/` funcional
- [ ] Dashboard carga en < 2 segundos
- [ ] Todos los 6 widgets visibles
- [ ] Refresh AJAX sin recargar página
- [ ] Cache implementado (15 min)
- [ ] Build exitoso (0 errores)
- [ ] Responsive en mobile/tablet/desktop
- [ ] Permisos validados por rol
- [ ] Documentación completada
- [ ] Git commit con mensaje descriptivo

---

## 📝 NOTAS IMPORTANTES

1. **Prioridad de widgets**:
   - 🔴 CRÍTICA: Tareas (CORE), Cotizaciones (EQ)
   - 🟠 ALTA: Proyectos (PY), Ausencias (TH)
   - 🟡 MEDIA: Documentos (GD), Métricas

2. **Performance**:
   - Cachear agresivamente (15 min)
   - Lazy load widgets fuera del viewport
   - Skeleton loading mientras carga

3. **Integración**:
   - Usar datos en vivo (no mock)
   - Validar permisos por rol
   - Respetar privacy data

4. **Testing**:
   - Verificar con usuarios de diferentes roles
   - Probar en navegadores antiguos
   - Validar con datos reales en staging

---

## 🎯 ENTREGABLES ESPERADOS

1. ✅ HomeController completo
2. ✅ DashboardService con todos los métodos
3. ✅ Index.cshtml responsive
4. ✅ dashboard.js con AJAX + refresh
5. ✅ dashboard.css con responsive design
6. ✅ Build exitoso (0 errores)
7. ✅ Documentación (este documento actualizado)
8. ✅ Git commits diarios con mensajes descriptivos

---

**Documento generado**: 2026-01-15  
**Última actualización**: 2026-01-15  
**Estado**: KICKOFF - LISTO PARA EJECUCIÓN
