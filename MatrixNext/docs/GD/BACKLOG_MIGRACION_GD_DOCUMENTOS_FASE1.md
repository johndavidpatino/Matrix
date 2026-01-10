# 📋 BACKLOG DE MIGRACIÓN - GD_Documentos (Gestión Documental)

**Módulo**: GD_Documentos  
**Versión**: 1.0  
**Fecha**: 2026-01-09  
**Responsable**: Equipo de Migración  
**Estado**: 🟡 EN PLANIFICACIÓN

---

## 📑 ÍNDICE

- [Visión General](#visión-general)
- [Fases de Migración](#fases-de-migración)
- [Sprint 1: Infraestructura Base](#sprint-1-infraestructura-base)
- [Próximas Fases](#próximas-fases)

---

## 🎯 VISIÓN GENERAL

### Alcance Total de la Migración

**Módulo**: GD_Documentos (Gestión Documental)

**Objetivo**: Migrar 100% de funcionalidad de WebMatrix → MatrixNext Core 8.0 MVC

**Artefactos a Migrar**:
- 13 páginas WebForms (8 confirmadas, 5 inferidas)
- 39 Stored Procedures
- 9 tablas de base de datos
- 2 clases de lógica de negocio
- 1 sistema de notificaciones

**Principios de Migración** (según DIRECTRICES_MIGRACION.md):
1. 🔴 CRÍTICO: Respetar exactamente nombres de BD, SP, tablas, columnas
2. 🔴 CRÍTICO: Analizar y mapear todo en CoreProject antes de codificar
3. 🔴 CRÍTICO: Ejecutar procedimientos exactamente como en WebMatrix
4. 🔴 CRÍTICO: Validar entrada y permisos
5. 🟠 ALTO: Preferir modales para CRUD
6. 🟠 ALTO: No agregar nuevas features, solo paridad 1:1
7. 🟠 ALTO: Reutilizar componentes existentes

### Timeline Total Estimado

| Componente | Horas | Semanas | Sprints |
|-----------|-------|---------|---------|
| Infraestructura (P0-1) | 4h | 0.1 | 1 |
| Catálogos (P0-2) | 12h | 0.3 | 1 |
| Maestro Documentos (P0-3) | 16h | 0.4 | 2 |
| Repositorio (P0-4) | 20h | 0.5 | 2 |
| Investigación Workflow (P0-5) | 8h | 0.2 | 1 |
| Solicitudes (P1-1) | 24h | 0.6 | 2 |
| Aprobaciones (P1-2) | 20h | 0.5 | 2 |
| Email Asíncrono (P1-3) | 12h | 0.3 | 1 |
| Actualización (P1-4) | 16h | 0.4 | 2 |
| Anulación (P1-5) | 8h | 0.2 | 1 |
| Dashboard (P1-6) | 6h | 0.15 | 1 |
| PNC (P2-1) | 40h | 1 | 2 |
| Escáner (P2-2) | 16h | 0.4 | 1 |
| UX/Config (P2-3/P2-4) | 18h | 0.45 | 1 |
| Testing e Integración | 20h | 0.5 | 1 |
| Documentación Final | 10h | 0.25 | - |
| **TOTAL BASE** | **220h** | **5.5** | **11** |
| **Buffer 20%** | **44h** | **1.1** | - |
| **TOTAL CON BUFFER** | **264h** | **6.6** | **11** |

**Timeline Real**: 7-8 semanas con 1 developer full-time

---

## 🔄 FASES DE MIGRACIÓN

### Estructura de Fases

La migración se divide en **6 fases** para evitar problemas de token y bloqueos:

| Fase | Nombre | Sprints | Horas | Duración | Contenido |
|------|--------|---------|-------|----------|-----------|
| **1** | Infraestructura Base | 1 | 16h | 1 semana | Estructura MVC, DI, Catálogos base |
| **2** | Maestro + Repositorio | 2-3 | 36h | 1 semana | Documentos maestro, repositorio versionado |
| **3** | Workflow Core | 4-5 | 44h | 1 semana | Solicitudes, Aprobaciones, Investigación |
| **4** | Features Restantes | 6-7 | 34h | 1 semana | Email, Actualización, Anulación, Dashboard |
| **5** | PNC + Optimizaciones | 8-9 | 56h | 1.5 semanas | PNC completo, Escáner, UX, Config |
| **6** | Testing + Documentación | 10-11 | 30h | 1 semana | Testing integral, documentación final |

**Total**: 11 sprints, 216 horas base + 44h buffer = 260h (~7 semanas)

---

## 🚀 SPRINT 1: INFRAESTRUCTURA BASE

### Objetivo

Establecer la estructura MVC base del módulo GD_Documentos, registrar servicios en DI, crear ViewModels básicos y completar catálogos maestros (3 tablas CRUD).

**Horas Estimadas**: 16h  
**Duración**: 1 semana (100% P0-1 + P0-2)  
**Criterio de Éxito**:
- ✅ Proyecto compila sin errores
- ✅ Área GD creada y registrada
- ✅ 6 servicios principales registrados en DI
- ✅ CRUD de 3 catálogos funcional
- ✅ Menú sidebar actualizado
- ✅ 0 warnings críticos

---

### TAREA 1.1: Crear Estructura de Área GD (1.5h)

**Descripción**: Crear carpetas y archivos base para el área GD

**Reglas Aplicables**:
- REGLA 9: Mantener estructura de áreas
- REGLA 10: Crear menú de acceso

**Subtareas**:

| ID | Tarea | Detalle | Responsable |
|----|-------|--------|-------------|
| 1.1.1 | Crear carpetas | Crear `Areas/GD/{Controllers,Views}/{Dashboard,DocumentosMaestro,Solicitudes,Repositorio,Catalogos,Aprobaciones,Pnc}` | Dev |
| 1.1.2 | Crear archivos de vista base | Crear `_ViewStart.cshtml`, `_ViewImports.cshtml` en `Areas/GD/Views` | Dev |
| 1.1.3 | Crear controllers stub | Crear archivos .cs vacíos para 7 controllers (ver lista abajo) | Dev |
| 1.1.4 | Crear Layout específico (si aplica) | ⚠️ REVISAR si GD necesita layout diferente al global | Dev |

**Controllers a Crear**:
1. `DashboardController.cs` (dashboard principal)
2. `DocumentosMaestroController.cs` (crear/editar/listar maestro)
3. `SolicitudesController.cs` (crear solicitudes con workflow)
4. `RepositorioController.cs` (upload/versioning)
5. `CatalogosController.cs` (CRUD de tipos, estados, procesos)
6. `AprobacionesController.cs` (aprobar/rechazar)
7. `PncController.cs` (Productos No Conformes)

**Evidencia de Completitud**:
- ✅ Carpetas existen y están vacías
- ✅ Archivos .cs compilables (sin implementación)
- ✅ _ViewStart.cshtml apunta a Layout correcto
- ✅ `dotnet build` exitoso

---

### TAREA 1.2: Registrar Área en Program.cs (0.5h)

**Descripción**: Registrar ruta de área GD en Program.cs

**Reglas Aplicables**:
- REGLA 9: Mantener estructura de áreas

**Código a Agregar**:

```csharp
// En Program.cs, después de app.UseRouting()
app.MapAreaControllerRoute(
    name: "gd_route",
    areaName: "GD",
    pattern: "GD/{controller=Dashboard}/{action=Index}/{id?}");
```

**Validación**:
- ✅ Ruta registrada antes de MapControllerRoute default
- ✅ Compila sin errores

---

### TAREA 1.3: Crear Interfaces y Servicios (4h)

**Descripción**: Definir interfaces de servicios y crear clases de servicios base

**Reglas Aplicables**:
- REGLA 2: Mapear metadata BD en CoreProject
- REGLA 3: Usar EF para CRUD simple

**Servicios a Crear** (en `Data/Services/GD/`):

| # | Interfaz | Implementación | Métodos Base | Notas |
|---|----------|----------------|--------------|-------|
| 1 | `IGdCatalogosService` | `GdCatalogosService` | `ObtenerTipoSolicitudes()`, `ObtenerEstadosSolicitud()`, `ObtenerProcesos()`, `CrearTipo/Estado/Proceso()`, `ActualizarTipo/Estado/Proceso()`, `EliminarTipo/Estado/Proceso()` | CRUD de 3 catálogos |
| 2 | `IGdMaestroService` | `GdMaestroService` | `ObtenerMaestros()`, `ObtenerMaestroById()`, `CrearMaestro()`, `ActualizarMaestro()`, `AnularMaestro()` | Documetos maestros |
| 3 | `IGdSolicitudesService` | `GdSolicitudesService` | `ObtenerSolicitudes()`, `CrearSolicitud()`, `AsignarRevisores()` | Solicitudes con workflow |
| 4 | `IGdRepositorioService` | `GdRepositorioService` | `ObtenerDocumentos()`, `UploadDocumento()`, `EliminarDocumento()` | Repositorio versionado |
| 5 | `IGdAprobacionesService` | `GdAprobacionesService` | `ObtenerRevisionesPendientes()`, `AprobarRevision()`, `RechazarRevision()` | Workflow aprobación |
| 6 | `IGdPncService` | `GdPncService` | `ObtenerPnc()`, `CrearPnc()`, `ActualizarPnc()` | Productos No Conformes |
| 7 | `IGdEmailService` | `GdEmailService` | `EnviarNotificacionAprobacion()`, `EnviarNotificacionRechazo()`, `EnviarNotificacionSolicitud()` | Email async |

**Pasos**:

1. **Crear interfaces** (7 archivos en `Data/Services/GD/Interfaces/`):
   - Definir métodos públicos sin implementación
   - Documentar parámetros y retorno
   - Ejemplo:
     ```csharp
     public interface IGdCatalogosService
     {
         Task<(bool success, List<TipoSolicitudViewModel> data)> ObtenerTipoSolicitudes();
         Task<(bool success, int idCreado)> CrearTipoSolicitud(TipoSolicitudViewModel vm);
         Task<(bool success, string message)> ActualizarTipoSolicitud(int id, TipoSolicitudViewModel vm);
         Task<(bool success, string message)> EliminarTipoSolicitud(int id);
     }
     ```

2. **Crear clases de implementación** (7 archivos en `Data/Services/GD/`):
   - Inyectar DbContext y adapters
   - Implementar métodos (puede ser stub con return default por ahora)
   - Agregar logging básico
   - Ejemplo:
     ```csharp
     public class GdCatalogosService : IGdCatalogosService
     {
         private readonly MatrixNextContext _context;
         private readonly IGdCatalogosAdapter _adapter;
         private readonly ILogger<GdCatalogosService> _logger;

         public GdCatalogosService(MatrixNextContext context, IGdCatalogosAdapter adapter, ILogger<GdCatalogosService> logger)
         {
             _context = context;
             _adapter = adapter;
             _logger = logger;
         }

         public async Task<(bool success, List<TipoSolicitudViewModel> data)> ObtenerTipoSolicitudes()
         {
             try
             {
                 var data = await _adapter.ObtenerTipoSolicitudes();
                 return (true, data);
             }
             catch (Exception ex)
             {
                 _logger.LogError($"Error obtaining tipos solicitud: {ex.Message}");
                 return (false, new List<TipoSolicitudViewModel>());
             }
         }
         // ... otros métodos stub
     }
     ```

3. **Registrar en Program.cs**:
   ```csharp
   builder.Services.AddScoped<IGdCatalogosService, GdCatalogosService>();
   builder.Services.AddScoped<IGdMaestroService, GdMaestroService>();
   builder.Services.AddScoped<IGdSolicitudesService, GdSolicitudesService>();
   builder.Services.AddScoped<IGdRepositorioService, GdRepositorioService>();
   builder.Services.AddScoped<IGdAprobacionesService, GdAprobacionesService>();
   builder.Services.AddScoped<IGdPncService, GdPncService>();
   builder.Services.AddScoped<IGdEmailService, GdEmailService>();
   ```

**Validación**:
- ✅ Todas las interfaces definidas
- ✅ Todas las clases compilables
- ✅ DI registrado sin errores
- ✅ `dotnet build` exitoso

---

### TAREA 1.4: Crear Adapters Base (4h)

**Descripción**: Crear adapters Dapper para acceso a datos (mapeo SP directo)

**Reglas Aplicables**:
- REGLA 2: Mapear exactamente SP de CoreProject
- REGLA 4: Ejecutar procedimientos almacenados de WebMatrix

**Archivos a Crear** (en `Data/Adapters/GD/`):

| # | Adapter | SP a Mapear | Métodos |
|---|---------|-------------|---------|
| 1 | `IGdCatalogosAdapter` + `GdCatalogosAdapter` | `GD_TipoSolicitud_Get`, `GD_TipoSolicitud_Add`, `GD_TipoSolicitud_Update`, `GD_TipoSolicitud_Delete`, `GD_Estados_Get`, `GD_Estados_Add`, `GD_Estados_Update`, `GD_Estados_Delete`, `GD_Procesos_Get`, `GD_Procesos_Add`, `GD_Procesos_Update`, `GD_Procesos_Delete` | 12 métodos (3 CRUD × 4 tipos) |
| 2 | `IGdMaestroAdapter` + `GdMaestroAdapter` | `GD_MaestroDocumentos_Get`, `GD_MaestroDocumentos_Add2`, `GD_DocumentosControlados_Add`, `GD_DocumentosMaestros_Update`, `GD_DocumentosControlados_Activo` | 5 métodos |
| 3 | `IGdRepositorioAdapter` + `GdRepositorioAdapter` | `GD_RepositorioDocumentos_GetXTrabajo`, `GD_RepositorioDocumentos_Add`, `GD_EscanerDocumentos_Del` | 3 métodos |
| 4 | `IGdSolicitudesAdapter` + `GdSolicitudesAdapter` | `GD_SolDocumentos_Add`, `GD_Revisiones_Add`, `GD_US_Usuarios_Get` | 3 métodos |
| 5 | `IGdAprobacionesAdapter` + `GdAprobacionesAdapter` | `GD_Revisiones_GetRev`, `GD_Revisiones_Edit`, `GD_SolicitudDocumentos_Update` | 3 métodos |
| 6 | `IGdPncAdapter` + `GdPncAdapter` | ⚠️ TBD (pending análisis P2-1.1) | TBD |

**Pasos**:

1. **Analizar CoreProject** (REGLA 2):
   - Buscar clase `GD_Procedimientos.vb`
   - Documentar cada SP: nombre, parámetros, tipo de retorno
   - Crear tabla de mapeo (ver anexo MAPEO_SP_GD.csv)

2. **Crear interfaces** (5 archivos):
   - Definir métodos Dapper
   - Parámetros tipados
   - Ejemplo:
     ```csharp
     public interface IGdCatalogosAdapter
     {
         Task<List<TipoSolicitudViewModel>> ObtenerTipoSolicitudes();
         Task<int> CrearTipoSolicitud(string nombre, string descripcion);
         Task<bool> ActualizarTipoSolicitud(int id, string nombre, string descripcion);
         Task<bool> EliminarTipoSolicitud(int id);
     }
     ```

3. **Crear implementaciones** (5 archivos):
   - Usar `IDbConnection` (Dapper)
   - Mapear parámetros → SP
   - Manejo de excepciones
   - Ejemplo:
     ```csharp
     public class GdCatalogosAdapter : IGdCatalogosAdapter
     {
         private readonly string _connectionString;

         public GdCatalogosAdapter(IConfiguration config)
         {
             _connectionString = config.GetConnectionString("DefaultConnection");
         }

         public async Task<List<TipoSolicitudViewModel>> ObtenerTipoSolicitudes()
         {
             using (var connection = new SqlConnection(_connectionString))
             {
                 var result = await connection.QueryAsync<TipoSolicitudViewModel>(
                     "GD_TipoSolicitud_Get",
                     commandType: CommandType.StoredProcedure);
                 return result.ToList();
             }
         }
     }
     ```

4. **Registrar en Program.cs**:
   ```csharp
   builder.Services.AddScoped<IGdCatalogosAdapter, GdCatalogosAdapter>();
   // ... otros adapters
   ```

5. **Validar contra CO_Matrix_Structure_SP.sql**:
   - Confirmar nombre exacto de SP
   - Confirmar parámetros y tipos
   - Documentar cualquier diferencia

**Validación**:
- ✅ 5 adapters implementados
- ✅ Todos los métodos compilables
- ✅ SP names verificados contra CO_Matrix_Structure_SP.sql
- ✅ DI registrado
- ✅ `dotnet build` exitoso

---

### TAREA 1.5: Crear ViewModels para Catálogos (3h)

**Descripción**: Crear ViewModels para operaciones CRUD de catálogos

**Archivos a Crear** (en `Models/ViewModels/GD/`):

| ViewModel | Propiedades | Validaciones | Notas |
|-----------|------------|--------------|-------|
| `TipoSolicitudViewModel` | `Id` (int), `Nombre` (string), `Descripcion` (string) | `[Required]` Nombre, `[MaxLength(100)]` | Construcción/Actualización/Anulación |
| `EstadoSolicitudViewModel` | `Id` (int), `Nombre` (string), `Descripcion` (string) | `[Required]` Nombre, `[MaxLength(100)]` | Estados de solicitud |
| `ProcesoViewModel` | `Id` (int), `Nombre` (string), `Descripcion` (string) | `[Required]` Nombre, `[MaxLength(100)]` | Procesos organizacionales |
| `CatalogosResponseVM` | `Tipos` (List), `Estados` (List), `Procesos` (List) | N/A | Response combinado para GET |

**Pasos**:

1. Crear 3 archivos de ViewModel simples
2. Agregar DataAnnotations para validación
3. Agregar propiedades de audit (si aplica)

**Validación**:
- ✅ ViewModels compilables
- ✅ Propiedades mapeables a SP parámetros
- ✅ Validaciones aplicables

---

### TAREA 1.6: Implementar CatalogosController CRUD (2h)

**Descripción**: Implementar controlador CRUD para catálogos

**Reglas Aplicables**:
- REGLA 5: Preferir modales para edición
- REGLA 5.1: UX AJAX-First

**Métodos a Implementar**:

| Método | HTTP | Parámetros | Retorna | Notas |
|--------|------|-----------|---------|-------|
| `TiposSolicitud` | GET | - | View lista tipos | GET /GD/Catalogos/TiposSolicitud |
| `CreateTipo` | GET | - | PartialView modal (si AJAX) | GET /GD/Catalogos/CreateTipo |
| `CreateTipo` | POST | `TipoSolicitudViewModel` | JSON {success, id} o PartialView error | POST /GD/Catalogos/CreateTipo |
| `UpdateTipo` | GET | `int id` | PartialView modal (si AJAX) | GET /GD/Catalogos/UpdateTipo/1 |
| `UpdateTipo` | POST | `int id`, `TipoSolicitudViewModel` | JSON {success} o PartialView error | POST /GD/Catalogos/UpdateTipo/1 |
| `DeleteTipo` | POST | `int id` | JSON {success} | DELETE via POST |
| Similar para Estados y Procesos | - | - | - | Repetir patrón 3×3 = 9 métodos |

**Código Ejemplo**:

```csharp
[Area("GD")]
[Authorize] // REGLA 11: Validar permisos
public class CatalogosController : Controller
{
    private readonly IGdCatalogosService _service;
    private readonly ILogger<CatalogosController> _logger;

    public CatalogosController(IGdCatalogosService service, ILogger<CatalogosController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: /GD/Catalogos/TiposSolicitud
    public async Task<IActionResult> TiposSolicitud()
    {
        _logger.LogInformation("Accediendo a TiposSolicitud");
        var (success, data) = await _service.ObtenerTipoSolicitudes();
        return success ? View(data) : View(new List<TipoSolicitudViewModel>());
    }

    // GET: /GD/Catalogos/CreateTipo (modal)
    public async Task<IActionResult> CreateTipo()
    {
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("_CreateTipoModal");
        return View("CreateTipo");
    }

    // POST: /GD/Catalogos/CreateTipo
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTipo(TipoSolicitudViewModel vm)
    {
        // REGLA 12: Validar entrada
        if (!ModelState.IsValid)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_CreateTipoModal", vm);
            return View(vm);
        }

        var (success, idCreado) = await _service.CrearTipoSolicitud(vm);
        
        if (success)
        {
            _logger.LogInformation($"Tipo solicitud creado: {idCreado}");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = true, id = idCreado, message = "Tipo creado exitosamente" });
            return RedirectToAction(nameof(TiposSolicitud));
        }

        // REGLA 13: Manejar errores gracefully
        ModelState.AddModelError("", "Error al crear tipo solicitud");
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("_CreateTipoModal", vm);
        return View(vm);
    }

    // Similar para Update, Delete, EstadosSolicitud, Procesos...
}
```

**Validación**:
- ✅ Todos los métodos implementados
- ✅ [Authorize] en todos
- ✅ ModelState validado
- ✅ Logging presente
- ✅ JSON responses para AJAX
- ✅ PartialView para modales

---

### TAREA 1.7: Crear Vistas CRUD para Catálogos (1.5h)

**Descripción**: Crear vistas HTML para listados y modales de catálogos

**Archivos a Crear** (en `Areas/GD/Views/Catalogos/`):

| Archivo | Tipo | Contenido | Framework |
|---------|------|----------|-----------|
| `TiposSolicitud.cshtml` | View | Grid con CRUD botones | Bootstrap 5 + DataTables |
| `EstadosSolicitud.cshtml` | View | Grid con CRUD botones | Bootstrap 5 + DataTables |
| `Procesos.cshtml` | View | Grid con CRUD botones | Bootstrap 5 + DataTables |
| `_CreateTipoModal.cshtml` | PartialView | Form modal crear tipo | Bootstrap 5 Modal |
| `_UpdateTipoModal.cshtml` | PartialView | Form modal editar tipo | Bootstrap 5 Modal |
| `_CreateEstadoModal.cshtml` | PartialView | Form modal crear estado | Bootstrap 5 Modal |
| `_UpdateEstadoModal.cshtml` | PartialView | Form modal editar estado | Bootstrap 5 Modal |
| `_CreateProcesoModal.cshtml` | PartialView | Form modal crear proceso | Bootstrap 5 Modal |
| `_UpdateProcesoModal.cshtml` | PartialView | Form modal editar proceso | Bootstrap 5 Modal |

**Estructura de Vista de Listado** (ejemplo TiposSolicitud.cshtml):

```html
@model List<TipoSolicitudViewModel>

@{
    ViewData["Title"] = "Tipos de Solicitud";
}

<div class="container mt-4">
    <h2>Tipos de Solicitud</h2>
    
    <button class="btn btn-primary mb-3" id="btnCreateTipo">
        <i class="fas fa-plus"></i> Nuevo Tipo
    </button>

    <table class="table table-striped" id="tblTipos">
        <thead>
            <tr>
                <th>ID</th>
                <th>Nombre</th>
                <th>Descripción</th>
                <th>Acciones</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var item in Model ?? new List<TipoSolicitudViewModel>())
            {
                <tr>
                    <td>@item.Id</td>
                    <td>@item.Nombre</td>
                    <td>@item.Descripcion</td>
                    <td>
                        <button class="btn btn-sm btn-warning btnEdit" data-id="@item.Id">
                            <i class="fas fa-edit"></i>
                        </button>
                        <button class="btn btn-sm btn-danger btnDelete" data-id="@item.Id">
                            <i class="fas fa-trash"></i>
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
            // Abrir modal crear
            $('#btnCreateTipo').click(function() {
                $.get('@Url.Action("CreateTipo")', function(html) {
                    $('#ajaxModal').html(html).modal('show');
                });
            });

            // Abrir modal editar
            $(document).on('click', '.btnEdit', function() {
                var id = $(this).data('id');
                $.get('@Url.Action("UpdateTipo")/' + id, function(html) {
                    $('#ajaxModal').html(html).modal('show');
                });
            });

            // Eliminar con confirmación
            $(document).on('click', '.btnDelete', function() {
                if (confirm('¿Está seguro?')) {
                    var id = $(this).data('id');
                    $.post('@Url.Action("DeleteTipo")', { id: id }, function(result) {
                        if (result.success) {
                            location.reload();
                        }
                    });
                }
            });
        });
    </script>
}
```

**Estructura de Modal** (ejemplo _CreateTipoModal.cshtml):

```html
@model TipoSolicitudViewModel

<div class="modal-header">
    <h5 class="modal-title">Nuevo Tipo de Solicitud</h5>
    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
</div>

<form id="frmCreateTipo" method="post">
    <div class="modal-body">
        <div class="mb-3">
            <label for="Nombre" class="form-label">Nombre *</label>
            <input type="text" class="form-control" id="Nombre" name="Nombre" 
                   value="@Model?.Nombre" required maxlength="100" />
            <span class="text-danger" asp-validation-for="Nombre"></span>
        </div>

        <div class="mb-3">
            <label for="Descripcion" class="form-label">Descripción</label>
            <textarea class="form-control" id="Descripcion" name="Descripcion" 
                      rows="3">@Model?.Descripcion</textarea>
            <span class="text-danger" asp-validation-for="Descripcion"></span>
        </div>
    </div>

    <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
        <button type="submit" class="btn btn-primary">Guardar</button>
    </div>
</form>

@section Scripts {
    <script>
        $(function() {
            $('#frmCreateTipo').on('submit', function(e) {
                e.preventDefault();
                $.ajax({
                    type: 'POST',
                    url: '@Url.Action("CreateTipo")',
                    data: $(this).serialize(),
                    headers: { 'X-Requested-With': 'XMLHttpRequest' },
                    success: function(result) {
                        if (result.success) {
                            toastr.success(result.message);
                            location.reload();
                        }
                    },
                    error: function(xhr) {
                        toastr.error('Error al guardar');
                    }
                });
            });
        });
    </script>
}
```

**Validación**:
- ✅ Vistas compilables
- ✅ Formularios tienen campo CSRF
- ✅ Validación client-side
- ✅ Modales reutilizan `_AjaxModal.cshtml`
- ✅ Botones CRUD presentes

---

### TAREA 1.8: Actualizar Menú Sidebar (0.5h)

**Descripción**: Agregar entrada de GD en menú de navegación principal

**Reglas Aplicables**:
- REGLA 10: Crear menú de acceso

**Archivo a Modificar**: `Views/Shared/_Sidebar.cshtml`

**Código a Agregar**:

```html
<li class="nav-item">
    <a class="nav-link" href="#gdMenu" data-bs-toggle="collapse">
        <i class="fas fa-file-alt"></i>
        <span>Gestión Documental</span>
        <i class="fas fa-chevron-down ms-auto"></i>
    </a>
    <div class="collapse" id="gdMenu">
        <ul class="nav flex-column ms-3">
            <li class="nav-item">
                <a class="nav-link" href="@Url.Action("Index", "Dashboard", new { area = "GD" })">
                    <i class="fas fa-home"></i>
                    <span>Dashboard</span>
                </a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="@Url.Action("Index", "DocumentosMaestro", new { area = "GD" })">
                    <i class="fas fa-book"></i>
                    <span>Maestro Documentos</span>
                </a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="@Url.Action("Index", "Solicitudes", new { area = "GD" })">
                    <i class="fas fa-file-invoice"></i>
                    <span>Solicitudes</span>
                </a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="@Url.Action("Index", "Aprobaciones", new { area = "GD" })">
                    <i class="fas fa-check-circle"></i>
                    <span>Aprobaciones</span>
                </a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="@Url.Action("Index", "Repositorio", new { area = "GD" })">
                    <i class="fas fa-folder"></i>
                    <span>Repositorio</span>
                </a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="@Url.Action("Index", "Pnc", new { area = "GD" })">
                    <i class="fas fa-exclamation-triangle"></i>
                    <span>Productos No Conformes</span>
                </a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="@Url.Action("TiposSolicitud", "Catalogos", new { area = "GD" })">
                    <i class="fas fa-cogs"></i>
                    <span>Catálogos</span>
                </a>
            </li>
        </ul>
    </div>
</li>
```

**Validación**:
- ✅ Menú aparece en sidebar
- ✅ Enlaces navegan correctamente
- ✅ Iconos Font Awesome presentes
- ✅ Collapse funciona

---

### TAREA 1.9: Testing de Infraestructura (0.5h)

**Descripción**: Validar que compilación y estructura base funciona

**Checklist**:

- [x] `dotnet clean` exitoso
- [x] `dotnet build -c Debug` exitoso (0 errores, warnings aceptables)
- [x] Área GD registrada correctamente
- [x] DI servicios registrados
- [x] Rutas MVC funcionan `/GD/Dashboard`
- [x] Catálogos CRUD compilable
- [x] Menú sidebar muestra entradas GD
- [ ] `dotnet test` (si hay tests) — fallan pruebas existentes de otras áreas (Email/OP); no bloquea GD

**Validación Final**:
- ✅ Proyecto compila limpio
- ✅ No hay errores de DI
- ✅ Menú funciona
- ✅ Controllers accesibles

---

### Registro de Completitud - Sprint 1

**Estado**: 🟢 COMPLETADO (GD Fase 1)

| Tarea | Horas | Estado | Evidencia |
|-------|-------|--------|-----------|
| 1.1 Crear estructura área | 1.5h | ✅ | Carpetas creadas |
| 1.2 Registrar en Program.cs | 0.5h | ✅ | MapAreaControllerRoute |
| 1.3 Servicios e interfaces | 4h | ✅ | 7 interfaces + 7 clases |
| 1.4 Adapters Dapper | 4h | ✅ | 5 adapters + 12 métodos |
| 1.5 ViewModels catálogos | 3h | ✅ | 4 ViewModels |
| 1.6 CatalogosController | 2h | ✅ | 9 métodos (3 CRUD) |
| 1.7 Vistas CRUD | 1.5h | ✅ | 9 vistas + modales |
| 1.8 Menú sidebar | 0.5h | ✅ | 7 enlaces en sidebar |
| 1.9 Testing infraestructura | 0.5h | ✅ | Build + validación (tests globales con fallas ajenas a GD) |
| **TOTAL SPRINT 1** | **16h** | **✅** | - |

---

## 📌 PRÓXIMAS FASES

Esta es **FASE 1 de 6**. Las siguientes fases se documentarán en archivos separados:

- **FASE 2** (BACKLOG_MIGRACION_GD_DOCUMENTOS_FASE2.md): Sprints 2-3 (Maestro Documentos + Repositorio)
- **FASE 3** (BACKLOG_MIGRACION_GD_DOCUMENTOS_FASE3.md): Sprints 4-5 (Solicitudes + Aprobaciones + Workflow)
- **FASE 4** (BACKLOG_MIGRACION_GD_DOCUMENTOS_FASE4.md): Sprints 6-7 (Email + Actualización + Anulación + Dashboard)
- **FASE 5** (BACKLOG_MIGRACION_GD_DOCUMENTOS_FASE5.md): Sprints 8-9 (PNC + Escáner + UX + Config)
- **FASE 6** (BACKLOG_MIGRACION_GD_DOCUMENTOS_FASE6.md): Sprints 10-11 (Testing + Documentación)

### Criterios de Éxito para Completar Sprint 1

✅ **DEBE CUMPLIRSE ANTES DE PASAR A FASE 2**:

1. Proyecto compila sin errores críticos
2. Todos los 7 servicios registrados en DI
3. CRUD de catálogos (tipos, estados, procesos) completamente funcional
4. Menú sidebar actualizado con 7 enlaces
5. 0 warnings relacionados con GD
6. Documentación de adaptación de adapters vs CoreProject completada
7. Commit de cambios con descripción detallada ✅ (realizado)
8. Revisión aprobada por arquitecto ⚠️ pendiente (fuera de alcance de desarrollo)

---

**Fin de FASE 1**

Próxima: [Crear FASE 2 - Maestro + Repositorio]

