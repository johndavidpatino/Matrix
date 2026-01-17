# 🔄 GUÍA DE PATRONES ASYNC/AWAIT - MatrixNext

**Autor**: GitHub Copilot - MatrixNext Migration Team  
**Fecha**: 2026-01-16  
**Sprint**: 21 Semana 1 - Fase 1 (Tarea A2)  
**Objetivo**: Estandarizar el uso de async/await en MatrixNext

---

## 📋 DIRECTRIZ PRINCIPAL

**REGLA #14**: Usar async/await en TODAS las operaciones I/O  
**PROHIBIDO**: Uso de `.Result`, `.Wait()`, `.GetAwaiter().GetResult()`

### Impacto de Blocking Calls

```csharp
// ❌ INCORRECTO - Bloquea thread del pool
var data = _service.GetDataAsync().Result;

// Problemas:
// 1. Deadlock potencial en ASP.NET Core
// 2. Degradación de performance (thread starvation)
// 3. No libera thread durante operación I/O
```

```csharp
// ✅ CORRECTO - Libera thread durante I/O
var data = await _service.GetDataAsync();

// Beneficios:
// 1. No bloquea threads
// 2. Mejor escalabilidad
// 3. Evita deadlocks
```

---

## 🎯 PATRONES CORRECTOS

### 1. Controllers - Siempre async

```csharp
// ❌ INCORRECTO
public class EmpleadosController : Controller
{
    public IActionResult Index()
    {
        var empleados = _service.GetEmpleadosAsync().Result;  // ❌ Blocking
        return View(empleados);
    }
}

// ✅ CORRECTO
public class EmpleadosController : Controller
{
    public async Task<IActionResult> Index()
    {
        var empleados = await _service.GetEmpleadosAsync();
        return View(empleados);
    }
}
```

### 2. Services - Siempre async para I/O

```csharp
// ❌ INCORRECTO
public class EmpleadoService
{
    public List<EmpleadoDto> GetEmpleados()
    {
        return _adapter.GetEmpleadosAsync().Result;  // ❌ Blocking
    }
}

// ✅ CORRECTO
public class EmpleadoService
{
    public async Task<List<EmpleadoDto>> GetEmpleadosAsync()
    {
        return await _adapter.GetEmpleadosAsync();
    }
}
```

### 3. Adapters - Dapper con async

```csharp
// ❌ INCORRECTO
public class EmpleadosAdapter
{
    public List<EmpleadoDto> GetEmpleados()
    {
        return _connection.Query<EmpleadoDto>("SP_NAME", ...).ToList();  // ❌ Sync
    }
}

// ✅ CORRECTO
public class EmpleadosAdapter
{
    public async Task<List<EmpleadoDto>> GetEmpleadosAsync()
    {
        var result = await _connection.QueryAsync<EmpleadoDto>(
            "SP_NAME",
            commandType: CommandType.StoredProcedure
        );
        return result.ToList();
    }
}
```

### 4. Múltiples Operaciones Paralelas

```csharp
// ❌ INCORRECTO - Usa .Result
public DashboardViewModel GetDashboard()
{
    var tasksTask = _taskService.GetPendingTasksAsync();
    var projectsTask = _projectService.GetActiveProjectsAsync();
    
    return new DashboardViewModel
    {
        PendingTasks = tasksTask.Result,      // ❌ Blocking
        ActiveProjects = projectsTask.Result  // ❌ Blocking
    };
}

// ✅ CORRECTO - Usa Task.WhenAll
public async Task<DashboardViewModel> GetDashboardAsync()
{
    var tasksTask = _taskService.GetPendingTasksAsync();
    var projectsTask = _projectService.GetActiveProjectsAsync();
    
    // Esperar TODAS las tareas en paralelo
    await Task.WhenAll(tasksTask, projectsTask);
    
    return new DashboardViewModel
    {
        PendingTasks = await tasksTask,       // ✅ Ya completado
        ActiveProjects = await projectsTask   // ✅ Ya completado
    };
}
```

### 5. Autenticación - SignInAsync

```csharp
// ❌ INCORRECTO
[HttpPost]
public IActionResult Login(LoginViewModel model)
{
    // ... validaciones ...
    
    var claims = new[] { /* ... */ };
    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);
    
    HttpContext.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        principal
    ).Wait();  // ❌ Blocking
    
    return RedirectToAction("Index", "Home");
}

// ✅ CORRECTO
[HttpPost]
public async Task<IActionResult> Login(LoginViewModel model)
{
    // ... validaciones ...
    
    var claims = new[] { /* ... */ };
    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);
    
    await HttpContext.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        principal
    );
    
    return RedirectToAction("Index", "Home");
}
```

### 6. Cache con SemaphoreSlim

```csharp
// ❌ INCORRECTO - Usa .Wait()
private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);

public List<FestivoDto> GetFestivos()
{
    _cacheLock.Wait();  // ❌ Blocking
    
    try
    {
        if (_cache.TryGetValue("festivos", out List<FestivoDto> festivos))
            return festivos;
        
        festivos = _adapter.GetFestivosAsync().Result;  // ❌ Blocking
        _cache.Set("festivos", festivos);
        return festivos;
    }
    finally
    {
        _cacheLock.Release();
    }
}

// ✅ CORRECTO - Usa WaitAsync
private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);

public async Task<List<FestivoDto>> GetFestivosAsync()
{
    await _cacheLock.WaitAsync();
    
    try
    {
        if (_cache.TryGetValue("festivos", out List<FestivoDto> festivos))
            return festivos;
        
        festivos = await _adapter.GetFestivosAsync();
        _cache.Set("festivos", festivos);
        return festivos;
    }
    finally
    {
        _cacheLock.Release();
    }
}
```

---

## 🚨 CASOS ESPECÍFICOS - CORRECCIONES REQUERIDAS

### CASO 1: DashboardService.cs (9 ocurrencias)

**Archivo**: `MatrixNext.Web/Services/Dashboard/DashboardService.cs`  
**Líneas**: 99-107  
**Problema**: Múltiples `.Result` en operaciones paralelas

```csharp
// ❌ ACTUAL (líneas 99-107)
public DashboardViewModel GetDashboard()
{
    var tasksTask = _taskService.GetPendingTasksAsync();
    var projectsTask = _projectService.GetActiveProjectsAsync();
    var quotesTask = _quoteService.GetRecentQuotesAsync();
    var absencesTask = _absenceService.GetPendingAbsencesAsync();
    var docsTask = _documentService.GetPendingDocsAsync();
    var metricsTask = _metricsService.GetCurrentMetricsAsync();
    
    return new DashboardViewModel
    {
        PendingTasks = tasksTask.Result,        // ❌ BLOQUEA
        ActiveProjects = projectsTask.Result,   // ❌ BLOQUEA
        RecentQuotes = quotesTask.Result,       // ❌ BLOQUEA
        PendingAbsences = absencesTask.Result,  // ❌ BLOQUEA
        PendingDocs = docsTask.Result,          // ❌ BLOQUEA
        Metrics = metricsTask.Result            // ❌ BLOQUEA
    };
}

// ✅ CORRECCIÓN
public async Task<DashboardViewModel> GetDashboardAsync()
{
    var tasksTask = _taskService.GetPendingTasksAsync();
    var projectsTask = _projectService.GetActiveProjectsAsync();
    var quotesTask = _quoteService.GetRecentQuotesAsync();
    var absencesTask = _absenceService.GetPendingAbsencesAsync();
    var docsTask = _documentService.GetPendingDocsAsync();
    var metricsTask = _metricsService.GetCurrentMetricsAsync();
    
    // Esperar TODAS en paralelo
    await Task.WhenAll(
        tasksTask, 
        projectsTask, 
        quotesTask, 
        absencesTask, 
        docsTask, 
        metricsTask
    );
    
    return new DashboardViewModel
    {
        PendingTasks = await tasksTask,
        ActiveProjects = await projectsTask,
        RecentQuotes = await quotesTask,
        PendingAbsences = await absencesTask,
        PendingDocs = await docsTask,
        Metrics = await metricsTask
    };
}
```

**Controller debe cambiar también**:
```csharp
// ❌ ANTES
public IActionResult Index()
{
    var dashboard = _dashboardService.GetDashboard();
    return View(dashboard);
}

// ✅ DESPUÉS
public async Task<IActionResult> Index()
{
    var dashboard = await _dashboardService.GetDashboardAsync();
    return View(dashboard);
}
```

---

### CASO 2: OpFestivosService.cs (1 ocurrencia)

**Archivo**: `MatrixNext.Web/Services/OP/OpFestivosService.cs`  
**Línea**: 140  
**Problema**: `_cacheLock.Wait()` bloquea thread

```csharp
// ❌ ACTUAL (línea 140)
private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);

public List<FestivoDto> GetFestivos()
{
    _cacheLock.Wait();  // ❌ BLOQUEA
    // ...
}

// ✅ CORRECCIÓN
private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);

public async Task<List<FestivoDto>> GetFestivosAsync()
{
    await _cacheLock.WaitAsync();  // ✅ Async
    
    try
    {
        if (_cache.TryGetValue("festivos", out List<FestivoDto> festivos))
            return festivos;
        
        festivos = await _adapter.GetFestivosAsync();
        _cache.Set("festivos", festivos, TimeSpan.FromHours(24));
        return festivos;
    }
    finally
    {
        _cacheLock.Release();
    }
}
```

---

### CASO 3: LoginController.cs (1 ocurrencia)

**Archivo**: `MatrixNext.Web/Controllers/LoginController.cs`  
**Línea**: 98  
**Problema**: `SignInAsync().Wait()` bloquea en autenticación

```csharp
// ❌ ACTUAL (línea 98)
[HttpPost]
public IActionResult Login(LoginViewModel model)
{
    // ... validaciones ...
    
    HttpContext.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        principal
    ).Wait();  // ❌ BLOQUEA
    
    return RedirectToAction("Index", "Home");
}

// ✅ CORRECCIÓN
[HttpPost]
public async Task<IActionResult> Login(LoginViewModel model)
{
    // ... validaciones ...
    
    await HttpContext.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        principal
    );
    
    return RedirectToAction("Index", "Home");
}
```

---

## ✅ CHECKLIST DE VALIDACIÓN

Antes de commit, verificar:

- [ ] ✅ Controllers: Todos los métodos con I/O son `async Task<IActionResult>`
- [ ] ✅ Services: Todos los métodos con I/O son `async Task<T>`
- [ ] ✅ Adapters: Dapper usa `QueryAsync`, `ExecuteAsync`
- [ ] ✅ Sin `.Result` en todo el código
- [ ] ✅ Sin `.Wait()` en todo el código
- [ ] ✅ Sin `.GetAwaiter().GetResult()` en todo el código
- [ ] ✅ Operaciones paralelas usan `Task.WhenAll()`
- [ ] ✅ SemaphoreSlim usa `WaitAsync()` en lugar de `Wait()`
- [ ] ✅ HttpContext.SignInAsync() usa `await`
- [ ] ✅ Compilación sin warnings de async

---

## 🔍 SCRIPT DE DETECCIÓN

Para encontrar violaciones:

```powershell
# Buscar .Result
Get-ChildItem -Path .\MatrixNext.Web\,.\MatrixNext.Data\ -Recurse -Filter *.cs | 
  Select-String -Pattern "\.Result" | 
  Select-Object Path, LineNumber, Line

# Buscar .Wait()
Get-ChildItem -Path .\MatrixNext.Web\,.\MatrixNext.Data\ -Recurse -Filter *.cs | 
  Select-String -Pattern "\.Wait\(\)" | 
  Select-Object Path, LineNumber, Line

# Buscar .GetAwaiter().GetResult()
Get-ChildItem -Path .\MatrixNext.Web\,.\MatrixNext.Data\ -Recurse -Filter *.cs | 
  Select-String -Pattern "\.GetAwaiter\(\)\.GetResult\(\)" | 
  Select-Object Path, LineNumber, Line
```

---

## 📊 MÉTRICAS DE ÉXITO

| Métrica | Objetivo | Validación |
|---------|----------|------------|
| **Blocking Calls (.Result/.Wait())** | 0 | Script detección |
| **Controllers async** | 100% | Code review |
| **Services async** | 100% I/O | Code review |
| **Adapters async** | 100% Dapper | Code review |

---

## 🚀 BENEFICIOS ESPERADOS

1. **Performance**: Mejor throughput (+ 30-50% solicitudes/seg)
2. **Escalabilidad**: Threads liberados durante I/O
3. **Estabilidad**: 0 deadlocks relacionados a sync-over-async
4. **Calidad**: Cumplimiento 100% best practices .NET

---

**Generado por**: GitHub Copilot  
**Referencia**: [Async/await Best Practices - Microsoft Docs](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming)  
**Directriz**: Regla #14 - Usar async/await en TODAS las operaciones I/O
