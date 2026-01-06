# PATRONES Y ARQUITECTURA COMPARTIDA - MatrixNext FI

**Objetivo**: Definir patrones reutilizables, estructuras estándar y convenciones para todos los sprints
**Aplicable**: Sprints Pre-1, 1-6
**Versión**: 1.0

---

## 1) Estructura de Solución Estándar

```
MatrixNext.Web/
├── Program.cs                                    ← DI, middlewares
├── appsettings.json                             ← Configuración
├── appsettings.Development.json
│
├── wwwroot/
│   ├── lib/                                      ← Librerías (Bootstrap, jQuery, etc.)
│   ├── css/
│   │   ├── site.css
│   │   └── fi-custom.css
│   └── js/
│       ├── site.js
│       └── grid-component.js
│
├── Areas/
│   ├── CC/                                      ← Infraestructura (Sprint Pre-1)
│   │   ├── Controllers/                         (vacío, solo referencia)
│   │   ├── Services/
│   │   │   └── CCFinzOpeService.cs
│   │   ├── Data/
│   │   │   ├── Contexts/
│   │   │   │   └── CC_FinzOpeContext.cs
│   │   │   ├── Models/                          (generados con EF Scaffold)
│   │   │   │   ├── CC_Produccion.cs
│   │   │   │   ├── CC_PrestacionServicios.cs
│   │   │   │   ├── CC_Descuentos.cs
│   │   │   │   └── ...
│   │   │   ├── DTOs/
│   │   │   │   ├── ProduccionResultDTO.cs
│   │   │   │   ├── LiquidacionResultDTO.cs
│   │   │   │   └── ...
│   │   │   └── Adapters/
│   │   │       └── CCFinzOpeDataAdapter.cs
│   │   └── Area.cshtml
│   │
│   ├── FI/                                      ← Administrativo Financiero (Sprints 1-6)
│   │   ├── Controllers/                         (sprints 1-6 agregan controllers)
│   │   │   ├── ControlPresupuestosController.cs        (Sprint 1)
│   │   │   ├── PresupuestosInternosController.cs       (Sprint 2)
│   │   │   ├── ConteoTrabajosController.cs             (Sprint 3)
│   │   │   ├── ReportesController.cs                   (Sprint 4)
│   │   │   ├── ProduccionController.cs                 (Sprint 5)
│   │   │   └── InventarioController.cs                 (Sprint 6)
│   │   │
│   │   ├── Services/
│   │   │   ├── IControlPresupuestosService.cs
│   │   │   ├── ControlPresupuestosService.cs
│   │   │   ├── IPresupuestosInternosService.cs
│   │   │   ├── PresupuestosInternosService.cs
│   │   │   ├── IProduccionService.cs
│   │   │   ├── ProduccionService.cs
│   │   │   └── ... (más servicios por sprint)
│   │   │
│   │   ├── Data/
│   │   │   ├── Models/                          (reutiliza CC_FinzOpe + adicionales)
│   │   │   ├── DTOs/
│   │   │   │   ├── PresupuestoDTO.cs            (Sprint 1)
│   │   │   │   ├── ProduccionDTO.cs             (Sprint 5)
│   │   │   │   └── ...
│   │   │   └── Adapters/
│   │   │       ├── FIControlPresupuestosAdapter.cs
│   │   │       ├── FIProduccionAdapter.cs
│   │   │       └── ...
│   │   │
│   │   ├── Views/
│   │   │   ├── ControlPresupuestos/
│   │   │   │   ├── Index.cshtml
│   │   │   │   ├── Index.js
│   │   │   │   ├── Detalles.cshtml
│   │   │   │   └── Detalles.js
│   │   │   ├── PresupuestosInternos/
│   │   │   │   ├── Index.cshtml
│   │   │   │   └── Index.js
│   │   │   ├── Shared/                          (componentes compartidos)
│   │   │   │   ├── GridComponent.cshtml
│   │   │   │   ├── ModalCRUD.cshtml
│   │   │   │   ├── GridExport.cshtml
│   │   │   │   └── Filtros.cshtml
│   │   │   └── _ViewStart.cshtml
│   │   │
│   │   ├── Components/                          (View Components)
│   │   │   ├── FIGridComponent.cs
│   │   │   ├── FIFilterComponent.cs
│   │   │   ├── FIModalComponent.cs
│   │   │   └── ...
│   │   │
│   │   ├── Area.cshtml
│   │   └── _Layout.cshtml                       (layout FI específico)
│   │
│   └── ...
│
├── Shared/
│   ├── Controllers/
│   │   └── HomeController.cs
│   ├── Services/
│   │   └── CommonServices.cs
│   ├── Data/
│   │   └── Adapters/
│   │       └── SharedDataAdapter.cs             (si hay datos comunes)
│   └── Views/
│       └── _Layout.cshtml                       (layout principal)
│
├── Middlewares/
│   ├── ErrorHandlingMiddleware.cs
│   ├── LoggingMiddleware.cs
│   └── AuthorizationMiddleware.cs
│
├── Extensions/
│   ├── DependencyInjectionExtensions.cs        ← Registro DI centralizado
│   ├── StringExtensions.cs
│   └── DateExtensions.cs
│
└── Tests/
    ├── FI.UnitTests/
    │   ├── ControlPresupuestosServiceTests.cs
    │   ├── ProduccionServiceTests.cs
    │   └── ...
    └── FI.IntegrationTests/
        └── ControlPresupuestosIntegrationTests.cs
```

---

## 2) Patrones de Arquitectura

### 2.1 Patrón de Capas

```
UI Layer (View)
    ↓
Controller Layer (ControlPresupuestosController)
    ↓
Service Layer (IPresupuestoService, PresupuestoService)
    ↓
Adapter/Repository Layer (FIControlPresupuestosAdapter, CCFinzOpeDataAdapter)
    ↓
Database Layer (SQL Server, EF Core, Dapper)
```

**Responsabilidades**:

| Capa | Responsabilidad | Ejemplo |
|------|-----------------|---------|
| **View** | Presentación, UX | Index.cshtml, modales |
| **Controller** | HTTP handling, routing | GetPresupuestos(), Guardar() |
| **Service** | Validaciones, lógica negocio | GuardarPresupuesto(), CalcularVarianza() |
| **Adapter** | Acceso a datos, mapeos | ObtenerPresupuestos(), LiquidarPlanillas() |
| **Database** | Almacenamiento | Tablas, SP, índices |

### 2.2 Patrón de Flujo de Datos

```
HTTP Request (GET /FI/ControlPresupuestos)
    ↓
ControlPresupuestosController.Index()
    ↓
IPresupuestoService.ObtenerPresupuestos()
    ↓
FIControlPresupuestosAdapter.ObtenerPresupuestos()
    ↓
CCFinzOpeDataAdapter.ObtenerDetallePresupuesto()  (via EF Core)
    ↓
CC_FinzOpeContext.CC_DetallePresupuesto.ToList()
    ↓
SQL Server
    ↓
(Retorno inversamente)
    ↓
HTTP Response (JSON o HTML)
```

### 2.3 Patrón CRUD

**Create**:
```
POST /FI/Controller/Guardar
↓
Controller valida entrada básica
↓
Service valida lógica negocio
↓
Adapter inserta vía EF o SP
↓
Response: { success: true, id: X }
```

**Read**:
```
GET /FI/Controller/GetData (AJAX)
↓
Controller llama Service.ObtenerData()
↓
Service llama Adapter.ObtenerData()
↓
Adapter obtiene de EF o Dapper
↓
Response: [{ item1 }, { item2 }, ...]
```

**Update**:
```
POST /FI/Controller/Actualizar
↓
Controller valida
↓
Service valida estado (¿puede editarse?)
↓
Adapter actualiza
↓
Response: { success: true }
```

**Delete**:
```
POST /FI/Controller/Eliminar
↓
Soft delete (marcar como inactivo) o hard delete
↓
Adapter ejecuta SP o EF
↓
Response: { success: true }
```

---

## 3) Convenciones de Código

### 3.1 Naming

| Elemento | Patrón | Ejemplo |
|----------|--------|---------|
| **Clase Controller** | `{Feature}Controller` | `ControlPresupuestosController` |
| **Interface Service** | `I{Feature}Service` | `IPresupuestoService` |
| **Clase Service** | `{Feature}Service` | `PresupuestoService` |
| **Clase Adapter** | `{Area}{Feature}Adapter` | `FIControlPresupuestosAdapter` |
| **Clase DTO** | `{Feature}DTO` | `PresupuestoDTO`, `ProduccionDTO` |
| **Método obtener** | `Obtener{Plural}()` | `ObtenerPresupuestos()` |
| **Método guardar** | `Guardar{Singular}()` | `GuardarPresupuesto()` |
| **Método eliminar** | `Eliminar{Singular}()` | `EliminarPresupuesto()` |
| **Ruta Controller** | `/Area/Controller/Action` | `/FI/ControlPresupuestos/Index` |
| **View files** | `{Action}.cshtml` | `Index.cshtml`, `Detalles.cshtml` |
| **JavaScript files** | `{Action}.js` | `Index.js`, `Detalles.js` |

### 3.2 Estructura de Métodos

**Service**:
```csharp
public ResultType MetodoPublico(InputType input)
{
    // 1. Logging de entrada
    _logger.LogInformation($"Ejecutando {nameof(MetodoPublico)} con input={input}");
    
    // 2. Validación
    if (!ValidarInput(input))
        throw new ArgumentException("Input inválido");
    
    // 3. Lógica de negocio
    var resultado = _adapter.MetodoAdapter(input);
    
    // 4. Procesamiento adicional
    var procesado = ProcesarResultado(resultado);
    
    // 5. Logging de salida
    _logger.LogInformation($"Resultado: {procesado}");
    
    // 6. Retorno
    return procesado;
}
```

**Adapter**:
```csharp
public List<DTOType> MetodoAdapter(FilterType filter)
{
    _logger.LogInformation($"Adapter: obtener datos con filter={filter}");
    
    try
    {
        // Opción A: EF Core (lectura simple)
        var data = _efContext.Tabla
            .Where(x => x.Propiedad == filter.Valor)
            .ToList();
        
        // Opción B: Dapper (SP compleja)
        using var conn = new SqlConnection(_connectionString);
        var result = conn.Query<DTOType>(
            "SP_Nombre",
            new { param1 = filter.Valor },
            commandType: CommandType.StoredProcedure
        ).ToList();
        
        return result;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error en MetodoAdapter");
        throw;
    }
}
```

### 3.3 Manejo de Errores

**Pattern 1: Result Pattern** (recomendado para servicios)
```csharp
public class Result<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
}

// Uso:
public Result<PresupuestoDTO> GuardarPresupuesto(PresupuestoDTO model)
{
    try
    {
        if (model.Monto < 0)
            return Result<PresupuestoDTO>.Failure("Monto debe ser positivo");
        
        var saved = _adapter.Guardar(model);
        return Result<PresupuestoDTO>.Success(saved, "Guardado exitosamente");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error guardando");
        return Result<PresupuestoDTO>.Failure(ex.Message);
    }
}
```

**Pattern 2: Exceptions** (para operaciones críticas)
```csharp
public void LiquidarPlanillas(int periodo)
{
    if (periodo < 202301)
        throw new ArgumentException("Período inválido");
    
    try
    {
        _adapter.LiquidarPlanillas(periodo);
    }
    catch (SqlException ex)
    {
        throw new DataAccessException("Error en DB", ex);
    }
}
```

---

## 4) Patrones Reutilizables de UI

### 4.1 Grid + Filtros + Export

**Componente Reutilizable**: `FIGridComponent.cshtml`

```html
@model GridComponentModel

<div class="card">
    <div class="card-header">
        <div class="row">
            <div class="col-md-6">
                <h3>@Model.Title</h3>
            </div>
            <div class="col-md-6 text-right">
                @if (Model.ShowNew)
                {
                    <button class="btn btn-primary" id="btn-nuevo">
                        <i class="fa fa-plus"></i> Nuevo
                    </button>
                }
                @if (Model.ShowExport)
                {
                    <button class="btn btn-success" id="btn-exportar">
                        <i class="fa fa-download"></i> Exportar
                    </button>
                }
                @if (Model.ShowRefresh)
                {
                    <button class="btn btn-secondary" id="btn-refresh">
                        <i class="fa fa-sync"></i> Actualizar
                    </button>
                }
            </div>
        </div>
    </div>
    
    <!-- Filtros -->
    @if (Model.Filters != null && Model.Filters.Any())
    {
        <div class="card-header bg-light">
            <div class="row">
                @foreach (var filter in Model.Filters)
                {
                    <div class="col-md-3">
                        <label>@filter.Label</label>
                        @if (filter.Type == FilterType.Text)
                        {
                            <input type="text" id="filter-@filter.Id" 
                                class="form-control filter-input" 
                                placeholder="@filter.Placeholder">
                        }
                        else if (filter.Type == FilterType.Select)
                        {
                            <select id="filter-@filter.Id" 
                                class="form-control filter-input">
                                @foreach (var option in filter.Options)
                                {
                                    <option value="@option.Key">@option.Value</option>
                                }
                            </select>
                        }
                        else if (filter.Type == FilterType.Date)
                        {
                            <input type="date" id="filter-@filter.Id" 
                                class="form-control filter-input">
                        }
                    </div>
                }
                <div class="col-md-3" style="margin-top: 24px;">
                    <button class="btn btn-primary btn-block" id="btn-buscar">
                        Buscar
                    </button>
                </div>
            </div>
        </div>
    }
    
    <!-- Grid -->
    <div class="card-body">
        <table id="@Model.GridId" class="table table-striped table-hover">
            <thead>
                <tr>
                    @foreach (var col in Model.Columns)
                    {
                        <th>@col.Header</th>
                    }
                    @if (Model.ShowActions)
                    {
                        <th style="width: 120px;">Acciones</th>
                    }
                </tr>
            </thead>
            <tbody></tbody>
        </table>
    </div>
</div>

@section Scripts {
    <script>
        // JavaScript generic grid logic
        const gridConfig = @Html.Raw(Json.Serialize(Model));
        initializeGrid(gridConfig);
    </script>
}
```

### 4.2 Modal CRUD Genérico

**Componente**: `ModalCRUD.cshtml`

```html
@model ModalCRUDModel

<div class="modal fade" id="@Model.ModalId" tabindex="-1" role="dialog">
    <div class="modal-dialog modal-@Model.Size" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="modal-title">@Model.Title</h5>
                <button type="button" class="close" data-dismiss="modal">
                    <span>&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <form id="@Model.FormId" class="form-modal" novalidate>
                    <input type="hidden" id="inp-id" name="Id" value="0">
                    
                    @foreach (var field in Model.Fields)
                    {
                        <div class="form-group">
                            <label for="@field.ControlId">@field.Label</label>
                            
                            @if (field.Type == FieldType.Text)
                            {
                                <input type="text" id="@field.ControlId" 
                                    name="@field.PropertyName" 
                                    class="form-control" 
                                    placeholder="@field.Placeholder"
                                    @if (field.Required) { <text>required</text> }>
                            }
                            @else if (field.Type == FieldType.Number)
                            {
                                <input type="number" id="@field.ControlId" 
                                    name="@field.PropertyName" 
                                    class="form-control" 
                                    step="@field.Step"
                                    @if (field.Required) { <text>required</text> }>
                            }
                            @else if (field.Type == FieldType.Select)
                            {
                                <select id="@field.ControlId" 
                                    name="@field.PropertyName" 
                                    class="form-control"
                                    @if (field.Required) { <text>required</text> }>
                                    <option value="">Seleccionar...</option>
                                    @foreach (var option in field.Options)
                                    {
                                        <option value="@option.Key">@option.Value</option>
                                    }
                                </select>
                            }
                            @else if (field.Type == FieldType.Date)
                            {
                                <input type="date" id="@field.ControlId" 
                                    name="@field.PropertyName" 
                                    class="form-control"
                                    @if (field.Required) { <text>required</text> }>
                            }
                            @else if (field.Type == FieldType.Textarea)
                            {
                                <textarea id="@field.ControlId" 
                                    name="@field.PropertyName" 
                                    class="form-control" 
                                    rows="4"
                                    @if (field.Required) { <text>required</text> }></textarea>
                            }
                            
                            @if (!string.IsNullOrEmpty(field.HelpText))
                            {
                                <small class="form-text text-muted">@field.HelpText</small>
                            }
                        </div>
                    }
                </form>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">
                    Cancelar
                </button>
                <button type="button" class="btn btn-primary" id="btn-save-@Model.ModalId">
                    Guardar
                </button>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    <script>
        // Modal form handling
        $(document).ready(function() {
            const config = @Html.Raw(Json.Serialize(Model));
            initializeModal(config);
        });
    </script>
}
```

### 4.3 JavaScript Reutilizable

**archivo**: `wwwroot/js/grid-component.js`

```javascript
/**
 * Grid Component - Reutilizable para todos los CRUD
 * Requiere: DataTables, jQuery, Bootstrap
 */

function initializeGrid(config) {
    const {
        gridId,
        apiUrl,
        columns,
        filters,
        showActions,
        editUrl,
        deleteUrl,
        modalId,
        formId
    } = config;
    
    // Inicializar DataTable
    const dataTable = new DataTable(`#${gridId}`, {
        language: { url: '/lib/es.json' },
        processing: true,
        serverSide: true,
        pageLength: 10,
        ajax: {
            url: apiUrl,
            type: 'POST',
            data: function(d) {
                // Agregar filtros
                filters?.forEach(f => {
                    d[f.id] = $(`#filter-${f.id}`).val();
                });
            }
        },
        columnDefs: [
            ...columns.map((col, idx) => ({
                targets: idx,
                data: col.data,
                render: col.render || ((data) => data)
            })),
            ...(showActions ? [{
                targets: -1,
                orderable: false,
                render: function(data, type, row) {
                    return `
                        <button class="btn btn-sm btn-primary btn-edit" 
                            data-id="${row.id}">
                            <i class="fa fa-edit"></i>
                        </button>
                        <button class="btn btn-sm btn-danger btn-delete" 
                            data-id="${row.id}">
                            <i class="fa fa-trash"></i>
                        </button>
                    `;
                }
            }] : [])
        ]
    });
    
    // Búsqueda
    $('#btn-buscar').click(() => dataTable.draw());
    
    // Nuevo
    $('#btn-nuevo').click(() => {
        $(`#${formId}`)[0].reset();
        $(`#inp-id`).val('0');
        $(`#modal-title`).text('Nuevo Registro');
        $(`#${modalId}`).modal('show');
    });
    
    // Editar
    $(document).on('click', '.btn-edit', function() {
        const id = $(this).data('id');
        $.get(`${editUrl}?id=${id}`, function(data) {
            populateForm(formId, data);
            $(`#modal-title`).text('Editar Registro');
            $(`#${modalId}`).modal('show');
        });
    });
    
    // Eliminar
    $(document).on('click', '.btn-delete', function() {
        const id = $(this).data('id');
        if (confirm('¿Eliminar registro?')) {
            $.post(`${deleteUrl}?id=${id}`, function(result) {
                if (result.success) {
                    dataTable.draw();
                } else {
                    alert('Error: ' + result.message);
                }
            });
        }
    });
    
    // Guardar
    $(`#btn-save-${modalId}`).click(() => {
        const data = getFormData(formId);
        $.post(
            `/FI/${getCurrentController()}/Guardar`,
            data,
            function(result) {
                if (result.success) {
                    $(`#${modalId}`).modal('hide');
                    dataTable.draw();
                } else {
                    alert('Error: ' + result.message);
                }
            }
        );
    });
    
    // Exportar
    $('#btn-exportar').click(() => {
        const filterParams = new URLSearchParams();
        filters?.forEach(f => {
            filterParams.append(f.id, $(`#filter-${f.id}`).val());
        });
        window.location = `/FI/${getCurrentController()}/Exportar?${filterParams}`;
    });
}

// Helper functions
function populateForm(formId, data) {
    Object.keys(data).forEach(key => {
        const $input = $(`#${formId} [name="${key}"]`);
        if ($input.length) {
            if ($input.is(':checkbox')) {
                $input.prop('checked', data[key]);
            } else {
                $input.val(data[key]);
            }
        }
    });
}

function getFormData(formId) {
    return Object.fromEntries(
        new FormData(document.getElementById(formId))
    );
}

function getCurrentController() {
    // Obtener de URL o data attribute
    return document.body.dataset.controller;
}
```

---

## 5) Patrón de Inyección de Dependencias

### 5.1 Registro en Program.cs

```csharp
// Program.cs

using MatrixNext.Web.Extensions;

var builder = WebApplicationBuilder.CreateBuilder(args);

// ========== Configuración ==========
var config = builder.Configuration;

// ========== Agregar servicios ==========
builder.Services.AddControllersWithViews();

// ========== Areas ==========
builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        // Búsqueda de vistas en Areas
        options.ViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
    });

// ========== DI Extensión ==========
builder.Services.AddFIServices(config);        // Registra servicios FI
builder.Services.AddCCServices(config);        // Registra servicios CC

// ========== Logging ==========
builder.Services.AddLogging(configure =>
{
    configure.AddConsole();
    configure.AddDebug();
});

// ========== Build y Run ==========
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ========== Routing Areas ==========
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
```

### 5.2 Extensiones de DI

**Archivo**: `Extensions/DependencyInjectionExtensions.cs`

```csharp
using MatrixNext.Web.Areas.CC.Data.Adapters;
using MatrixNext.Web.Areas.CC.Data.Contexts;
using MatrixNext.Web.Areas.CC.Data.Services;
using MatrixNext.Web.Areas.FI.Data.Adapters;
using MatrixNext.Web.Areas.FI.Services;

namespace MatrixNext.Web.Extensions;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registra servicios de CC_FinzOpe (infraestructura)
    /// </summary>
    public static IServiceCollection AddCCServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        // DbContext
        services.AddDbContext<CC_FinzOpeContext>(options =>
            options.UseSqlServer(config.GetConnectionString("CCFinzOpe"))
        );
        
        // Adapter
        services.AddScoped(sp =>
        {
            var connStr = config.GetConnectionString("CCFinzOpe");
            var context = sp.GetRequiredService<CC_FinzOpeContext>();
            var logger = sp.GetRequiredService<ILogger<CCFinzOpeDataAdapter>>();
            return new CCFinzOpeDataAdapter(connStr, context, logger);
        });
        
        // Service
        services.AddScoped<CCFinzOpeService>();
        
        return services;
    }
    
    /// <summary>
    /// Registra servicios de FI (todos los sprints)
    /// </summary>
    public static IServiceCollection AddFIServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        // Sprint 1: Control Presupuestos
        services.AddScoped<IPresupuestoService, PresupuestoService>();
        services.AddScoped<FIControlPresupuestosAdapter>();
        
        // Sprint 2: Presupuestos Internos
        services.AddScoped<IPresupuestosInternosService, PresupuestosInternosService>();
        services.AddScoped<FIPresupuestosInternosAdapter>();
        
        // Sprint 3: Procesos Internos
        services.AddScoped<IConteoTrabajosService, ConteoTrabajosService>();
        services.AddScoped<FIConteoTrabajosAdapter>();
        
        // Sprint 4: Reportes
        services.AddScoped<IReportesService, ReportesService>();
        services.AddScoped<FIReportesAdapter>();
        
        // Sprint 5: Producción
        services.AddScoped<IProduccionService, ProduccionService>();
        services.AddScoped<FIProduccionAdapter>();
        
        // Sprint 6: Inventario
        services.AddScoped<IInventarioService, InventarioService>();
        services.AddScoped<FIInventarioAdapter>();
        
        return services;
    }
}
```

---

## 6) Patrón de Testing

### 6.1 Unit Tests

**Estructura**: `Tests/FI.UnitTests/Services/`

```csharp
[TestClass]
public class PresupuestoServiceTests
{
    // Arrange
    private Mock<FIControlPresupuestosAdapter> _mockAdapter;
    private Mock<ILogger<PresupuestoService>> _mockLogger;
    private IPresupuestoService _service;
    
    [TestInitialize]
    public void Setup()
    {
        _mockAdapter = new Mock<FIControlPresupuestosAdapter>();
        _mockLogger = new Mock<ILogger<PresupuestoService>>();
        _service = new PresupuestoService(_mockAdapter.Object, _mockLogger.Object);
    }
    
    // Test: Obtener presupuestos
    [TestMethod]
    public void ObtenerPresupuestos_DebeRetornarLista()
    {
        // Arrange
        var expected = new List<PresupuestoDTO>
        {
            new() { Id = 1, Monto = 1000 },
            new() { Id = 2, Monto = 2000 }
        };
        _mockAdapter.Setup(x => x.ObtenerPresupuestos(It.IsAny<string>(), 
            It.IsAny<string>(), It.IsAny<string>()))
            .Returns(expected);
        
        // Act
        var result = _service.ObtenerPresupuestos("202401", "", "", 0, 10);
        
        // Assert
        Assert.AreEqual(2, result.Items.Count);
        Assert.AreEqual(1000, result.Items[0].Monto);
    }
    
    // Test: Guardar con validación
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GuardarPresupuesto_MontoNegativo_ThrowsException()
    {
        var model = new PresupuestoDTO { Monto = -100 };
        _service.GuardarPresupuesto(model);
    }
}
```

### 6.2 Integration Tests

```csharp
[TestClass]
public class ControlPresupuestosControllerTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    
    [TestInitialize]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }
    
    [TestMethod]
    public async Task Index_ReturnsSuccessAndContent()
    {
        var response = await _client.GetAsync("/FI/ControlPresupuestos");
        Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        Assert.IsTrue(content.Contains("Control de Presupuestos"));
    }
    
    [TestCleanup]
    public void Cleanup()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
}
```

---

## 7) Patrones de Error Handling

### 7.1 Middleware de Errores

```csharp
// Middlewares/ErrorHandlingMiddleware.cs

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    
    public ErrorHandlingMiddleware(RequestDelegate next, 
        ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no manejado");
            HandleException(context, ex);
        }
    }
    
    private void HandleException(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = new
        {
            success = false,
            message = exception.Message,
            details = exception.StackTrace
        };
        
        context.Response.StatusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
        
        context.Response.WriteAsJsonAsync(response);
    }
}
```

### 7.2 Uso en Program.cs

```csharp
app.UseMiddleware<ErrorHandlingMiddleware>();
```

---

## 8) Patrones de Validación

### 8.1 Data Annotations

```csharp
public class PresupuestoDTO
{
    [Required(ErrorMessage = "Período es requerido")]
    public int Periodo { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Monto debe ser positivo")]
    public decimal Monto { get; set; }
    
    [StringLength(100, ErrorMessage = "Descripción máx 100 caracteres")]
    public string Descripcion { get; set; }
}
```

### 8.2 FluentValidation (alternativa)

```csharp
public class PresupuestoDTOValidator : AbstractValidator<PresupuestoDTO>
{
    public PresupuestoDTOValidator()
    {
        RuleFor(x => x.Periodo)
            .NotEmpty().WithMessage("Período requerido")
            .GreaterThan(202301).WithMessage("Período inválido");
        
        RuleFor(x => x.Monto)
            .NotEmpty().WithMessage("Monto requerido")
            .GreaterThan(0).WithMessage("Monto debe ser positivo");
    }
}
```

---

## 9) Patrones de Logging

### 9.1 Uso Estándar

```csharp
public class PresupuestoService
{
    private readonly ILogger<PresupuestoService> _logger;
    
    public void GuardarPresupuesto(PresupuestoDTO model)
    {
        _logger.LogInformation("Guardando presupuesto {0}", model.Id);
        
        try
        {
            // Lógica
            _logger.LogInformation("Presupuesto {0} guardado exitosamente", model.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error guardando presupuesto {0}", model.Id);
            throw;
        }
    }
}
```

### 9.2 Log Levels

| Nivel | Cuándo | Ejemplo |
|-------|--------|---------|
| **Debug** | Información interna | Valores internos, loops |
| **Information** | Eventos normales | Inicio de operación, resultado |
| **Warning** | Condiciones inesperadas | Validación fallida, reintento |
| **Error** | Errores que no rompen app | Excepción manejada |
| **Critical** | Errores críticos | DB desconectada, corrupción data |

---

## 10) Checklist de Implementación por Sprint

- [ ] Controllers creados para cada página
- [ ] Views creadas (Index.cshtml, modales)
- [ ] JavaScript agregado (AJAX, grid)
- [ ] Services implementados con validaciones
- [ ] Adapters conectados a CC_FinzOpe
- [ ] DTOs definidos
- [ ] DI registrado en Program.cs
- [ ] appsettings.json actualizado
- [ ] Unit tests escritos (80%+ cobertura)
- [ ] Integration tests (happy path)
- [ ] Errores manejados (middleware)
- [ ] Logging en lugar (información, error)
- [ ] Documentación actualizada
- [ ] Compilación sin errores
- [ ] Code review completado

---

**Documento**: PATRONES_ARQUITECTURA_FI.md  
**Versión**: 1.0  
**Estado**: 📋 Ready for implementation

