# INSTRUCCIONES DE GITHUB COPILOT - MATRIXNEXT

> **Contexto**: Migración de WebMatrix (ASP.NET WebForms legacy) → MatrixNext (.NET 8 MVC)  
> **Objetivo**: Paridad funcional completa, sin agregar features nuevas  
> **Idioma**: Español para comentarios, mensajes y documentación

---

## 🎯 DIRECTIVAS PRINCIPALES PARA COPILOT

### Cuando generes código, SIEMPRE:

1. ✅ **Usa español** para comentarios, mensajes de error, logs y documentación
2. ✅ **Respeta EXACTAMENTE** los nombres de BD (tablas, SP, columnas) - NO inventes nombres
3. ✅ **Consulta CoreProject** (WebMatrix legacy) antes de implementar lógica de datos
4. ✅ **Sigue el patrón**: `Controller → Service → Adapter → BD`
5. ✅ **Usa modales** (Bootstrap) para CRUD en lugar de páginas separadas
6. ✅ **Implementa async/await** en todas las operaciones I/O
7. ✅ **Valida permisos** con `[Authorize]` en todos los controllers
8. ✅ **Maneja errores** sin exponer stack traces (retorna mensajes amigables)
9. ? **Usa MatrixNext.Data para la capa de datos y MatrixNext.Web para la capa Web (vistas, controllers, etc). Solo puedes agregar archivos o carpetas dentro de estas dos rutas**

### Cuando revises código:

- 🔍 Verifica que los nombres de objetos de BD coincidan con `CoreProject` y scripts en `MatrixNext/docs/SQL/`
- 🔍 Confirma que existe mapeo a Stored Procedures de WebMatrix
- 🔍 Valida que NO se agreguen funcionalidades que no existen en WebMatrix
- 🔍 Aplica el checklist de "Testing y Validación" (más abajo)

### ❌ PROHIBIDO (detén y alerta):

- Inventar nombres de tablas, SP, vistas o columnas
- Agregar funcionalidades que no existen en WebMatrix
- Cambiar flujos de negocio sin documentar
- Exponer información sensible o stack traces en respuestas
- Usar `.Result` o `.Wait()` (siempre async/await)
- Lógica de negocio en Controllers o Views
- Acceso directo a BD desde Controllers (usar Adapters)

---

## 📐 ARQUITECTURA Y PATRONES

### Patrón obligatorio: Adapter → Service → Controller

```
HTTP Request
    ↓
[Controller]  ← Recibe, valida headers, coordina, retorna View/JSON
    ↓
[Service]     ← Lógica de negocio, validaciones, transformaciones
    ↓
[Adapter]     ← Acceso a datos (SP o EF), mapeo a modelos
    ↓
[Database]    ← SQL Server (tablas, SP, triggers)
```

**Ejemplo de implementación correcta**:

```csharp
// ✅ Controller (delgado, solo coordina)
[Area("TH")]
[Authorize]
public class AusenciasController : Controller
{
    private readonly IAusenciasService _service;
    
    public AusenciasController(IAusenciasService service)
    {
        _service = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(SolicitudAusenciaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, message = "Datos inválidos" });
            
        var (success, message) = await _service.CrearSolicitudAsync(dto, User.GetUserId());
        
        return Json(new { success, message });
    }
}

// ✅ Service (lógica de negocio)
public class AusenciasService : IAusenciasService
{
    private readonly IAusenciasAdapter _adapter;
    private readonly ILogger<AusenciasService> _logger;
    
    public async Task<(bool, string)> CrearSolicitudAsync(SolicitudAusenciaDto dto, int userId)
    {
        try
        {
            // Validar disponibilidad de días
            var diasDisponibles = await _adapter.ObtenerDiasDisponiblesAsync(userId, dto.TipoAusencia);
            if (diasDisponibles < dto.DiasSolicitados)
                return (false, "No tiene días disponibles suficientes");
            
            // Crear solicitud
            var solicitudId = await _adapter.CrearSolicitudAsync(dto, userId);
            
            _logger.LogInformation("Solicitud {Id} creada por usuario {UserId}", solicitudId, userId);
            return (true, "Solicitud creada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando solicitud para usuario {UserId}", userId);
            return (false, "Error al crear la solicitud");
        }
    }
}

// ✅ Adapter (acceso a datos)
public class AusenciasAdapter : IAusenciasAdapter
{
    private readonly IDbConnection _connection;
    
    public async Task<int> CrearSolicitudAsync(SolicitudAusenciaDto dto, int userId)
    {
        // Ejecutar SP exactamente como en WebMatrix/CoreProject
        var parameters = new DynamicParameters();
        parameters.Add("@IdEmpleado", userId);
        parameters.Add("@FechaInicio", dto.FechaInicio);
        parameters.Add("@FechaFin", dto.FechaFin);
        parameters.Add("@IdTipoAusencia", dto.TipoAusencia);
        parameters.Add("@Observaciones", dto.Observaciones);
        parameters.Add("@RegistradoPor", userId);
        parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
        
        await _connection.ExecuteAsync(
            "TH_Ausencia.RegistrosAusencia", 
            parameters, 
            commandType: CommandType.StoredProcedure
        );
        
        return parameters.Get<int>("@Id");
    }
}
```

---

## 💾 BASE DE DATOS - CONVENCIONES OBLIGATORIAS

### Nombres que DEBES respetar exactamente

| Elemento | Formato | Ejemplo | Regla |
|----------|---------|---------|-------|
| **Tabla** | `[MODULO]_[Entidad]` | `TH_SolicitudAusencia` | PascalCase con prefijo |
| **Columna** | Camel/PascalCase original | `IdEmpleado`, `FechaInicio` | Respetar casing exacto |
| **SP** | `[MODULO]_[ACCION]` o `[Schema].[Accion]` | `TH_AUSENCIA_GET` o `TH_Ausencia.RegistrosAusencia` | MAYÚSCULAS o schema. |
| **PK** | `Id` | `Id` | int o long |
| **FK** | `Id[Tabla]` | `IdEmpleado`, `IdSolicitud` | Referencia clara |
| **Auditoría** | Campos estándar | `RegistradoPor`, `FechaRegistro`, `ModificadoPor`, `FechaModificacion` | En todas las tablas |

### Proceso de verificación OBLIGATORIO

**Antes de usar cualquier objeto de BD**:

1. Buscar en `MatrixNext/docs/SQL/CO_Matrix_SP_Names.csv` (lista de SP disponibles)
2. Ver definición completa en:
   - SP: `MatrixNext/docs/SQL/CO_Matrix_Structure_SP.sql`
   - Tablas: `MatrixNext/docs/SQL/CO_Matrix_Structure_Tables.sql`
   - Vistas: `MatrixNext/docs/SQL/CO_Matrix_Structure_Views.sql`
3. Confirmar uso en `CoreProject` (WebMatrix DataLayer)
4. Validar en ambiente staging: `SELECT TOP 0 * FROM [Schema].[Table];`

**Si el objeto NO existe**: ❌ DETENER y documentar en `MIGRACION_[MODULO]_COMPLETADA.md`

```csharp
// ❌ INCORRECTO (inventar nombres)
var result = await _connection.QueryAsync("SELECT * FROM Empleados WHERE Id = @Id");

// ✅ CORRECTO (usar nombres exactos de BD)
var result = await _connection.QueryAsync("SELECT * FROM TH_Empleado WHERE IdEmpleado = @IdEmpleado");
```

---

## 🎮 CONTROLLERS Y SERVICES

### Responsabilidades por capa

| Capa | Responsabilidad | ❌ NO debe | ✅ SÍ debe |
|------|-----------------|-----------|-----------|
| **Controller** | Recibir request, coordinar, retornar respuesta | Lógica de negocio, acceso a BD | Validar ModelState, llamar service, retornar View/JSON |
| **Service** | Lógica de negocio, validaciones, transformaciones | Acceso directo a BD, renderizar HTML | Validar reglas, calcular, loggear, coordinar adapters |
| **Adapter** | Acceso a datos, mapeo | Lógica de negocio, validaciones | Ejecutar SP, EF CRUD, mapear resultados |

### Cuándo usar SP vs EF Core

```csharp
// ✅ Usar EF Core para operaciones simples
public async Task<int> CrearEmpleadoAsync(Empleado empleado)
{
    _context.Empleados.Add(empleado);
    await _context.SaveChangesAsync();
    return empleado.Id;
}

// ✅ Usar SP para lógica compleja (ya existe en WebMatrix)
public async Task<bool> AprobarSolicitudAsync(int solicitudId, int aprobadorId)
{
    // Este SP ejecuta: validaciones, actualiza estado, causa vacaciones, audita
    var parameters = new DynamicParameters();
    parameters.Add("@IdSolicitud", solicitudId);
    parameters.Add("@IdAprobador", aprobadorId);
    
    await _connection.ExecuteAsync(
        "TH_Ausencia.AprobarSolicitud",
        parameters,
        commandType: CommandType.StoredProcedure
    );
    
    return true;
}
```

---

## 🎨 VISTAS Y UI - PATRONES UX

### REGLA: Modales primero, páginas después

**✅ Usar Modal para**:
- Crear/Editar registros
- Ver detalles
- Confirmar eliminación
- Cambiar estado (aprobar/rechazar)
- Agregar comentarios

**❌ NO usar Modal para**:
- Listados principales (Index)
- Dashboards
- Reportes complejos
- Navegación entre secciones

### Patrón AJAX-First (obligatorio)

```javascript
// ✅ Patrón estándar para abrir modal de edición
$('[data-ajax-modal]').on('click', function(e) {
    e.preventDefault();
    const url = $(this).data('url');
    
    $.get(url, function(html) {
        $('#modalContainer').html(html);
        $('#modalForm').modal('show');
    });
});

// ✅ Patrón estándar para submit de modal
$(document).on('submit', '[data-ajax-form]', function(e) {
    e.preventDefault();
    const form = $(this);
    
    $.ajax({
        url: form.attr('action'),
        type: form.attr('method'),
        data: form.serialize(),
        success: function(response) {
            if (response.success) {
                // Toast de éxito
                showToast(response.message, 'success');
                
                // Cerrar modal
                $('#modalForm').modal('hide');
                
                // Refrescar grid
                $('[data-grid-url]').load($('[data-grid-url]').data('url'));
            } else {
                showToast(response.message, 'error');
            }
        },
        error: function() {
            showToast('Error al procesar la solicitud', 'error');
        }
    });
});
```

### Componentes reutilizables (usa estos primero)

```
Views/Shared/_AjaxModal.cshtml          → Modal genérico CRUD
Views/Shared/_ToastContainer.cshtml     → Notificaciones
Views/Shared/_DatePicker.cshtml         → Selector de fechas
Views/Shared/_SelectUser.cshtml         → Dropdown de usuarios
Views/Shared/_Grid.cshtml               → Grid paginado
Views/Shared/_Search.cshtml             → Buscador
Views/Shared/_Confirm.cshtml            → Confirmación
Views/Shared/_Loading.cshtml            → Spinner
Views/Shared/_Badge.cshtml              → Estados
wwwroot/js/ajax-modal.js                → Lógica de modales
```

---

## 📋 ESTRUCTURA DE ÁREAS

### Organización obligatoria

```
MatrixNext.Web/
├── Areas/
│   ├── TH/                    # Talento Humano
│   │   ├── Controllers/
│   │   │   ├── AusenciasController.cs
│   │   │   ├── EmpleadosController.cs
│   │   │   └── NominaController.cs
│   │   └── Views/
│   │       ├── Ausencias/
│   │       │   ├── Index.cshtml
│   │       │   ├── _CreateEdit.cshtml    ← Modal
│   │       │   └── _Details.cshtml       ← Modal
│   │       ├── Empleados/
│   │       └── Nomina/
│   │
│   ├── PY/                    # Proyectos
│   └── [otros módulos]/
│
├── Controllers/               # Solo globales (Home, Account)
├── Views/
│   ├── Home/
│   └── Shared/               # Componentes compartidos
│
└── Program.cs
```

### Registrar áreas en Program.cs

```csharp
// ✅ Siempre registrar servicios del área
builder.Services.AddTHModule();  // Extension method para DI del módulo

// ✅ Configurar routing con áreas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
```

---

## 🧪 TESTING Y VALIDACIÓN

### Checklist Pre-Commit (verificar TODO antes de commit)

- [ ] ✅ Compilación sin errores
- [ ] ✅ 0 warnings críticos (nullability aceptable)
- [ ] ✅ Todos los métodos implementados (sin `throw new NotImplementedException()`)
- [ ] ✅ Todos los SP verificados contra `MatrixNext/docs/SQL/` y `CoreProject`
- [ ] ✅ Modales abren, guardan y cierran correctamente
- [ ] ✅ Búsqueda/filtros funcionan
- [ ] ✅ Paginación funciona
- [ ] ✅ `[Authorize]` aplicado en todos los controllers
- [ ] ✅ Logging en operaciones críticas (create, update, delete, approve)
- [ ] ✅ Manejo de excepciones (try/catch con mensajes amigables)
- [ ] ✅ DI registrado en `Program.cs` (services, adapters)
- [ ] ✅ Menú actualizado en `Views/Shared/_Sidebar.cshtml`
- [ ] ✅ Documentación actualizada (`MIGRACION_[MODULO]_COMPLETADA.md`)
- [ ] ✅ Sin archivos sin usar (commented code, unused usings)
- [ ] ✅ Sin `TODO` o `FIXME` sin resolver

### Testing funcional mínimo

Para cada vista migrada, probar:

1. **Acceso**: ¿Puedo acceder con `[Authorize]`?
2. **Crear**: ¿Puedo crear nuevo registro via modal?
3. **Editar**: ¿Puedo editar existente via modal?
4. **Eliminar**: ¿Puedo eliminar con confirmación?
5. **Búsqueda**: ¿Funcionan filtros?
6. **Paginación**: ¿Se pagina correctamente?
7. **Modal**: ¿Se abre, guarda y cierra?
8. **Error**: ¿Qué pasa si hay error en BD? (mensaje amigable)

---

## 📖 DOCUMENTACIÓN OBLIGATORIA

### Por cada módulo migrado crear:

1. **`ANALISIS_[MODULO].md`** (conciso)
   - Descripción del módulo
   - Páginas a migrar (lista)
   - SP identificados en CoreProject
   - Flujos de negocio principales

2. **`MIGRACION_[MODULO]_COMPLETADA.md`**
   - Checklist de implementación
   - Componentes migrados
   - SP mapeados (tabla: Acción → SP → Parámetros)
   - Testing realizado
   - Problemas encontrados y soluciones

3. **Comentarios en código** (solo cuando agrega valor)
   ```csharp
   // ✅ Bueno (explica decisión no obvia)
   // Usar SP legacy porque incluye cálculo de días hábiles y validación de solapamiento
   await _connection.ExecuteAsync("TH_Ausencia.RegistrosAusencia", ...);
   
   // ❌ Malo (repite lo que dice el código)
   // Ejecutar stored procedure
   await _connection.ExecuteAsync("TH_Ausencia.RegistrosAusencia", ...);
   ```

---

## ⚠️ REGLAS ADICIONALES CRÍTICAS

### REGLA: Validar permisos SIEMPRE

```csharp
// ✅ Validar que el usuario puede ver/editar el recurso
public async Task<IActionResult> Edit(int id)
{
    var solicitud = await _service.ObtenerSolicitudAsync(id);
    
    // Validar: solo el empleado o su jefe pueden editar
    if (solicitud.IdEmpleado != User.GetUserId() && 
        !await _service.EsJefeDeAsync(User.GetUserId(), solicitud.IdEmpleado))
    {
        _logger.LogWarning("Usuario {UserId} intentó editar solicitud {SolicitudId} sin permisos", 
            User.GetUserId(), id);
        return Forbid();
    }
    
    return View(solicitud);
}
```

### REGLA: Manejar errores gracefully

```csharp
// ❌ INCORRECTO (expone stack trace)
catch (Exception ex)
{
    return Json(new { success = false, message = ex.Message });
}

// ✅ CORRECTO (log detallado, mensaje genérico al cliente)
catch (SqlException ex)
{
    _logger.LogError(ex, "Error de BD al crear solicitud. UserId: {UserId}, Dto: {@Dto}", 
        userId, dto);
    return Json(new { success = false, message = "Error al crear la solicitud. Por favor intente nuevamente." });
}
catch (Exception ex)
{
    _logger.LogError(ex, "Error inesperado al crear solicitud. UserId: {UserId}", userId);
    return Json(new { success = false, message = "Error inesperado. Contacte al administrador." });
}
```

### REGLA: Async/await obligatorio en I/O

```csharp
// ❌ INCORRECTO (bloquea thread)
public IActionResult Index()
{
    var data = _service.ObtenerSolicitudesAsync().Result;  // ❌ .Result bloquea
    return View(data);
}

// ✅ CORRECTO
public async Task<IActionResult> Index()
{
    var data = await _service.ObtenerSolicitudesAsync();
    return View(data);
}
```

---

## 📊 RESUMEN DE REGLAS (Prioridades)

| # | Regla | Prioridad | Cuándo aplica |
|---|-------|-----------|---------------|
| 1 | Respetar nombres BD exactos | 🔴 CRÍTICA | Siempre |
| 2 | Consultar CoreProject antes de implementar | 🔴 CRÍTICA | Siempre |
| 2.1 | Prohibido inventar objetos de BD | 🔴 CRÍTICA | Siempre |
| 3 | Usar EF para CRUD simple, SP para lógica compleja | 🟠 ALTA | Data access |
| 4 | Ejecutar SP de WebMatrix identificados | 🔴 CRÍTICA | Siempre |
| 5 | Preferir modales para CRUD | 🟠 ALTA | UI |
| 5.1 | UX AJAX-First (modal + JSON + toast + refresh parcial) | 🟠 ALTA | UI |
| 6 | Solo migrar acciones existentes en WebMatrix | 🔴 CRÍTICA | Features |
| 7 | Aprovechar componentes compartidos existentes | 🟡 MEDIA | UI |
| 8 | Priorizar detalle sobre velocidad | 🔴 CRÍTICA | Proceso |
| 9 | Mantener estructura de áreas | 🟠 ALTA | Arquitectura |
| 10 | Actualizar menú en `_Sidebar.cshtml` | 🟠 ALTA | Navegación |
| 11 | Validar permisos con `[Authorize]` | 🔴 CRÍTICA | Security |
| 12 | Validar `ModelState` y datos de entrada | 🔴 CRÍTICA | Validación |
| 13 | Manejar errores sin stack traces | 🔴 CRÍTICA | Error handling |
| 14 | Usar async/await en I/O | 🟠 ALTA | Performance |
| 15 | Documentar en `MIGRACION_[MODULO]_COMPLETADA.md` | 🟠 ALTA | Tracking |

---

## 🎯 FLUJO DE TRABAJO RECOMENDADO

### Para cada webform a migrar:

```
1. ANÁLISIS (no saltar)
   ├─ Abrir webform en WebMatrix
   ├─ Identificar acciones (botones, eventos)
   ├─ Buscar DataAdapter en CoreProject
   ├─ Listar SP ejecutados por cada acción
   ├─ Verificar SP en MatrixNext/docs/SQL/
   ├─ Documentar en ANALISIS_[MODULO].md
   └─ ✅ Checkpoint: ¿Tengo todos los SP identificados?

2. IMPLEMENTACIÓN
   ├─ Crear Adapter (usar nombres exactos de SP)
   ├─ Crear Service (lógica de negocio)
   ├─ Crear Controller (coordinar)
   ├─ Crear Views (Index + modales)
   ├─ Actualizar _Sidebar.cshtml
   ├─ Registrar DI en Program.cs
   └─ ✅ Checkpoint: ¿Compila sin errores?

3. TESTING
   ├─ Probar crear registro
   ├─ Probar editar registro
   ├─ Probar eliminar registro
   ├─ Probar búsqueda/filtros
   ├─ Probar paginación
   ├─ Probar errores (BD offline, datos inválidos)
   └─ ✅ Checkpoint: ¿Todo funciona como en WebMatrix?

4. DOCUMENTACIÓN Y COMMIT
   ├─ Completar MIGRACION_[MODULO]_COMPLETADA.md
   ├─ Revisar checklist pre-commit
   ├─ Commit con mensaje descriptivo
   ├─ Push y crear PR
   └─ ✅ Checkpoint: ¿Listo para code review?
```

---

## 🚀 EJEMPLOS COMPLETOS

### Ejemplo: Migrar módulo de Ausencias

#### 1. Identificar en CoreProject

```csharp
// CoreProject/DataLayer/AusenciasDataAdapter.cs (WebMatrix legacy)
public DataTable ObtenerSolicitudes(int idEmpleado)
{
    // SP identificado: TH_AUSENCIA_GET
    return ExecuteSP("TH_AUSENCIA_GET", new { IdEmpleado = idEmpleado });
}

public int CrearSolicitud(Solicitud solicitud)
{
    // SP identificado: TH_Ausencia.RegistrosAusencia
    return ExecuteSP("TH_Ausencia.RegistrosAusencia", solicitud);
}
```

#### 2. Verificar en SQL

```powershell
# Buscar SP en archivos
Select-String -Path .\MatrixNext\docs\SQL\CO_Matrix_SP_Names.csv -Pattern "TH_AUSENCIA"

# Resultado: TH_AUSENCIA_GET, TH_Ausencia.RegistrosAusencia existen ✅
```

#### 3. Implementar en MatrixNext

```csharp
// MatrixNext.Infrastructure/Adapters/AusenciasAdapter.cs
public class AusenciasAdapter : IAusenciasAdapter
{
    private readonly IDbConnection _connection;
    
    public async Task<IEnumerable<SolicitudDto>> ObtenerSolicitudesAsync(int idEmpleado)
    {
        return await _connection.QueryAsync<SolicitudDto>(
            "TH_AUSENCIA_GET",  // ✅ Nombre exacto de CoreProject
            new { IdEmpleado = idEmpleado },
            commandType: CommandType.StoredProcedure
        );
    }
    
    public async Task<int> CrearSolicitudAsync(SolicitudDto dto, int userId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@IdEmpleado", dto.IdEmpleado);
        parameters.Add("@FechaInicio", dto.FechaInicio);
        parameters.Add("@FechaFin", dto.FechaFin);
        parameters.Add("@IdTipoAusencia", dto.IdTipoAusencia);
        parameters.Add("@Observaciones", dto.Observaciones);
        parameters.Add("@RegistradoPor", userId);
        parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
        
        await _connection.ExecuteAsync(
            "TH_Ausencia.RegistrosAusencia",  // ✅ Nombre exacto
            parameters,
            commandType: CommandType.StoredProcedure
        );
        
        return parameters.Get<int>("@Id");
    }
}

// MatrixNext.Core/Services/AusenciasService.cs
public class AusenciasService : IAusenciasService
{
    private readonly IAusenciasAdapter _adapter;
    private readonly ILogger<AusenciasService> _logger;
    
    public async Task<(bool, string, int)> CrearSolicitudAsync(SolicitudDto dto, int userId)
    {
        try
        {
            // Validar días disponibles
            var disponibles = await _adapter.ObtenerDiasDisponiblesAsync(userId, dto.IdTipoAusencia);
            if (disponibles < dto.DiasSolicitados)
            {
                return (false, "No tiene días disponibles suficientes", 0);
            }
            
            // Crear
            var id = await _adapter.CrearSolicitudAsync(dto, userId);
            
            _logger.LogInformation("Solicitud {Id} creada por usuario {UserId}", id, userId);
            return (true, "Solicitud creada exitosamente", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando solicitud. UserId: {UserId}, Dto: {@Dto}", userId, dto);
            return (false, "Error al crear la solicitud", 0);
        }
    }
}

// MatrixNext.Web/Areas/TH/Controllers/AusenciasController.cs
[Area("TH")]
[Authorize]
public class AusenciasController : Controller
{
    private readonly IAusenciasService _service;
    
    public AusenciasController(IAusenciasService service)
    {
        _service = service;
    }
    
    public async Task<IActionResult> Index()
    {
        var solicitudes = await _service.ObtenerMisSolicitudesAsync(User.GetUserId());
        return View(solicitudes);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        if (Request.IsAjaxRequest())
            return PartialView("_CreateEdit", new SolicitudDto());
        
        return View(new SolicitudDto());
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(SolicitudDto dto)
    {
        if (!ModelState.IsValid)
        {
            if (Request.IsAjaxRequest())
                return PartialView("_CreateEdit", dto);
            
            return View(dto);
        }
        
        var (success, message, id) = await _service.CrearSolicitudAsync(dto, User.GetUserId());
        
        if (Request.IsAjaxRequest())
            return Json(new { success, message });
        
        if (success)
        {
            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }
        
        ModelState.AddModelError("", message);
        return View(dto);
    }
}
```

#### 4. Vista con modal

```cshtml
@* MatrixNext.Web/Areas/TH/Views/Ausencias/Index.cshtml *@
@model IEnumerable<SolicitudDto>

<div class="container-fluid">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2>Mis Solicitudes de Ausencia</h2>
        <button class="btn btn-primary" 
                data-ajax-modal 
                data-url="@Url.Action("Create")">
            <i class="fas fa-plus"></i> Nueva Solicitud
        </button>
    </div>
    
    <div data-grid-url="@Url.Action("Index")">
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>Tipo</th>
                    <th>Fecha Inicio</th>
                    <th>Fecha Fin</th>
                    <th>Días</th>
                    <th>Estado</th>
                    <th>Acciones</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var item in Model)
                {
                    <tr>
                        <td>@item.TipoAusencia</td>
                        <td>@item.FechaInicio.ToString("dd/MM/yyyy")</td>
                        <td>@item.FechaFin.ToString("dd/MM/yyyy")</td>
                        <td>@item.DiasSolicitados</td>
                        <td><span class="badge badge-@item.EstadoClass">@item.EstadoTexto</span></td>
                        <td>
                            <button class="btn btn-sm btn-info" 
                                    data-ajax-modal 
                                    data-url="@Url.Action("Details", new { id = item.Id })">
                                <i class="fas fa-eye"></i>
                            </button>
                            @if (item.PuedeEditar)
                            {
                                <button class="btn btn-sm btn-warning" 
                                        data-ajax-modal 
                                        data-url="@Url.Action("Edit", new { id = item.Id })">
                                    <i class="fas fa-edit"></i>
                                </button>
                            }
                        </td>
                    </tr>
                }
            </tbody>
        </table>
    </div>
</div>

<div id="modalContainer"></div>

@* MatrixNext.Web/Areas/TH/Views/Ausencias/_CreateEdit.cshtml *@
@model SolicitudDto

<div class="modal-header">
    <h5 class="modal-title">@(Model.Id == 0 ? "Nueva" : "Editar") Solicitud</h5>
    <button type="button" class="close" data-dismiss="modal">&times;</button>
</div>

<form asp-action="@(Model.Id == 0 ? "Create" : "Edit")" 
      method="post" 
      data-ajax-form>
    <div class="modal-body">
        <div asp-validation-summary="ModelOnly" class="text-danger"></div>
        
        <div class="form-group">
            <label asp-for="IdTipoAusencia">Tipo de Ausencia</label>
            <select asp-for="IdTipoAusencia" class="form-control" asp-items="ViewBag.TiposAusencia">
                <option value="">Seleccione...</option>
            </select>
            <span asp-validation-for="IdTipoAusencia" class="text-danger"></span>
        </div>
        
        <div class="form-group">
            <label asp-for="FechaInicio">Fecha Inicio</label>
            <input asp-for="FechaInicio" type="date" class="form-control" />
            <span asp-validation-for="FechaInicio" class="text-danger"></span>
        </div>
        
        <div class="form-group">
            <label asp-for="FechaFin">Fecha Fin</label>
            <input asp-for="FechaFin" type="date" class="form-control" />
            <span asp-validation-for="FechaFin" class="text-danger"></span>
        </div>
        
        <div class="form-group">
            <label asp-for="Observaciones">Observaciones</label>
            <textarea asp-for="Observaciones" class="form-control" rows="3"></textarea>
            <span asp-validation-for="Observaciones" class="text-danger"></span>
        </div>
    </div>
    
    <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
        <button type="submit" class="btn btn-primary">Guardar</button>
    </div>
</form>
```

---

**Documento optimizado para GitHub Copilot**  
**Versión**: 2.0 (Copilot-enhanced)  
**Fecha**: 2026-01-14  
**Mantenimiento**: Actualizar cuando se descubran nuevos patrones o inconsistencias