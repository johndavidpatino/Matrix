# 📋 BACKLOG DE MIGRACIÓN - GD_Documentos FASE 2

**Fases**: FASE 2 (Sprints 2-3)  
**Tema**: Maestro de Documentos + Repositorio Versionado  
**Horas Totales**: 36h  
**Duración Estimada**: 1 semana (2 sprints)  
**Versión**: 1.0  
**Fecha**: 2026-01-09

---

## 📑 CONTENIDO

- [Resumen Ejecutivo](#resumen-ejecutivo)
- [Sprint 2: Maestro de Documentos](#sprint-2-maestro-de-documentos)
- [Sprint 3: Repositorio Versionado](#sprint-3-repositorio-versionado)

---

## 🎯 RESUMEN EJECUTIVO

### Objetivos de FASE 2

Implementar **100% funcional** los dos pilares arquitectónicos de GD:

1. **Maestro de Documentos** (P0-3): CRUD de documentos con control de calidad
   - 3 tipos: Construcción, Actualización, Anulación
   - Transacciones ACID (maestro + controlado)
   - Validaciones por tipo

2. **Repositorio de Documentos** (P0-4): Almacenamiento versionado
   - Upload/descarga de archivos
   - Versionamiento automático (MAX+1)
   - Asociación a contenedores (trabajos, proyectos)
   - Integración con UploadService existente

### Dependencias Críticas

✅ **COMPLETADAS en FASE 1**:
- Estructura MVC GD
- Servicios base (IGdMaestroService, IGdRepositorioService)
- Adapters base (IGdMaestroAdapter, IGdRepositorioAdapter)

⚠️ **PENDIENTE - VALIDAR**:
- UploadService en MatrixNext (ubicación, API)
- Estructura de carpetas de upload (física)
- Permisos de lectura/escritura filesystem

### Reglas Aplicables

| Regla | Descripción | Prioridad |
|-------|-------------|-----------|
| REGLA 2 | Mapear exactamente SP en CoreProject | 🔴 CRÍTICA |
| REGLA 3 | Usar EF para transacciones simples | 🟠 ALTA |
| REGLA 4 | Ejecutar SP de WebMatrix | 🔴 CRÍTICA |
| REGLA 5 | Preferir modales para edición | 🟠 ALTA |
| REGLA 6 | Solo paridad 1:1, no nuevas features | 🔴 CRÍTICA |
| REGLA 11 | Validar permisos de usuario | 🔴 CRÍTICA |
| REGLA 12 | Validar entrada | 🔴 CRÍTICA |

---

## 🚀 SPRINT 2: MAESTRO DE DOCUMENTOS

### Objetivo

Implementar CRUD completo de Maestro de Documentos con 3 tipos de solicitud (Construcción/Actualización/Anulación) y transacciones ACID.

**Horas Estimadas**: 16h  
**Duración**: 3-4 días  
**Criterio de Éxito**:
- ✅ CRUD funcional para 3 tipos
- ✅ Transacciones con maestro + controlado
- ✅ Validaciones por tipo
- ✅ 0 inconsistencias de datos
- ✅ Testing transaccional completo

---

### TAREA 2.1: Analizar y Mapear SP de Maestro (1h)

**Descripción**: Documentar exactamente qué SP ejecuta GD_Maestro.aspx en WebMatrix

**Reglas Aplicables**:
- REGLA 2: Mapear metadata en CoreProject
- REGLA 4: Ejecutar SP de WebMatrix

**Proceso**:

1. **Abrir CoreProject/GD_Procedimientos.vb**
   - Ubicación: `C:\Users\johnd\source\repos\johndavidpatino\Matrix\CoreProject\GD_Procedimientos.vb`
   - Buscar métodos relacionados a maestro (líneas ~129-220)

2. **Documentar SP y Parámetros**:

| Método VB | SP Name | Parámetros | Retorna | Usado en |
|-----------|---------|-----------|---------|----------|
| `MaestroDocumentos_Add2()` | `GD_MaestroDocumentos_Add2` | docNombre, docCodigo, docProceso, docResponsable, ubicacion, metodoRecuperacion, tiempoRetencion, disposicionFinal, controlado, activo | ultimoId (int) | Construcción |
| `DocumentosControlados_Add()` | `GD_DocumentosControlados_Add` | idMaestro, docUbicacion, metodoRecuperacion, tiempoRetencion, disposicionFinal, activo | - | Construcción (transacción) |
| `DocumentosMaestros_Update()` | `GD_DocumentosMaestros_Update` | idDoc, [...campos a actualizar...] | - | Actualización |
| `DocumentosControlados_Activo()` | `GD_DocumentosControlados_Activo` | idDoc | - | Anulación (soft delete) |
| `ObtenerDocumentos()` | `GD_MaestroDocumentos_Get` | - | DataTable | Listado |
| `ObtenerProcesos()` | `GD_Procesos_Get` | - | DataTable | Dropdown procesos |
| `ObtenerTipoSolicitud()` | `GD_TipoSolicitud_Get` | - | DataTable | Dropdown tipos |
| `ObtenerUsuarios()` | `GD_US_Usuarios_Get` | - | DataTable | Dropdown responsables |

3. **Validar contra CO_Matrix_Structure_SP.sql**:
   - Abrir archivo de estructura
   - Confirmar cada SP existe
   - Validar nombres de parámetros exactos
   - Documentar discrepancias

4. **Crear MAPEO_SP_MAESTRO.csv** en `docs/GD/`:
   ```csv
   Acción,SP Name,Parámetros,Retorna,Archivo SP,Línea
   Construcción - Maestro,GD_MaestroDocumentos_Add2,"docNombre, docCodigo, docProceso, docResponsable, ubicacion, metodoRecuperacion, tiempoRetencion, disposicionFinal, controlado, activo",ultimoId,CO_Matrix_Structure_SP.sql,12345
   Construcción - Controlado,GD_DocumentosControlados_Add,"idMaestro, docUbicacion, metodoRecuperacion, tiempoRetencion, disposicionFinal, activo",-,CO_Matrix_Structure_SP.sql,12400
   ...
   ```

**Validación**:
- ✅ MAPEO_SP_MAESTRO.csv creado
- ✅ Todos los SP validados contra CO_Matrix_Structure_SP.sql
- ✅ Discrepancias documentadas
- ✅ Parámetros confirmados

---

### TAREA 2.2: Expandir ViewModels para Maestro (1.5h)

**Descripción**: Crear ViewModels para operaciones de maestro documentos

**Archivos a Crear** (en `Models/ViewModels/GD/`):

| ViewModel | Propiedades | Validaciones | Notas |
|-----------|------------|--------------|-------|
| `MaestroDocumentoVM` | `Id` (int), `Nombre` (string), `Codigo` (string), `IdProceso` (int), `IdResponsable` (int), `TipoSolicitud` (int), `Activo` (bool), `ControlledDoc` (DocumentoControlledVM) | `[Required]` Nombre, Código; `[MaxLength]` 100 | Base para CRUD |
| `DocumentoControlledVM` | `Id` (int), `IdMaestro` (int), `Ubicacion` (string), `MetodoRecuperacion` (string), `TiempoRetencion` (int), `DisposicionFinal` (string), `Activo` (bool), `FechaRegistro` (DateTime) | Todos requeridos | Control calidad |
| `MaestroListVM` | `Id` (int), `Nombre` (string), `Codigo` (string), `Proceso` (string), `Responsable` (string), `Estado` (string), `Acciones` (string) | N/A | Listado con datos denormalizados |
| `MaestroCreateVM` | Hereda `MaestroDocumentoVM` + `Dropdowns` (TipoSolicitudes, Procesos, Usuarios) | Igual que MaestroDocumentoVM | Para formulario de creación |
| `MaestroUpdateVM` | Hereda `MaestroDocumentoVM` + `FechaRegistro`, `RegistradoPor`, `FechaModificacion` | Igual que MaestroDocumentoVM | Para formulario de edición (read-only audit) |

**Código Ejemplo** (MaestroDocumentoVM.cs):

```csharp
using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Models.ViewModels.GD
{
    public class MaestroDocumentoVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del documento es requerido")]
        [MaxLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El código del documento es requerido")]
        [MaxLength(50, ErrorMessage = "El código no puede exceder 50 caracteres")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El proceso es requerido")]
        public int IdProceso { get; set; }

        [Required(ErrorMessage = "El responsable es requerido")]
        public int IdResponsable { get; set; }

        [Required(ErrorMessage = "El tipo de solicitud es requerido")]
        public int TipoSolicitud { get; set; } // 1=Construcción, 2=Actualización, 3=Anulación

        public bool Activo { get; set; } = true;

        // Para visualización en dropdowns
        public string ProcesoNombre { get; set; }
        public string ResponsableNombre { get; set; }
        public string TipoNombre { get; set; }

        // Documento controlado anidado
        public DocumentoControlledVM ControlledDoc { get; set; } = new();
    }

    public class DocumentoControlledVM
    {
        [Required(ErrorMessage = "La ubicación es requerida")]
        [MaxLength(500)]
        public string Ubicacion { get; set; }

        [Required(ErrorMessage = "El método de recuperación es requerido")]
        [MaxLength(100)]
        public string MetodoRecuperacion { get; set; }

        [Required(ErrorMessage = "El tiempo de retención es requerido")]
        [Range(1, 100, ErrorMessage = "El tiempo debe estar entre 1 y 100 años")]
        public int TiempoRetencion { get; set; }

        [Required(ErrorMessage = "La disposición final es requerida")]
        [MaxLength(200)]
        public string DisposicionFinal { get; set; }

        public bool Activo { get; set; } = true;
    }

    public class MaestroCreateVM : MaestroDocumentoVM
    {
        // Dropdowns para formulario
        public List<TipoSolicitudViewModel> TiposSolicitud { get; set; } = new();
        public List<ProcesoViewModel> Procesos { get; set; } = new();
        public List<UsuarioViewModel> Usuarios { get; set; } = new();
    }

    public class MaestroUpdateVM : MaestroDocumentoVM
    {
        public List<TipoSolicitudViewModel> TiposSolicitud { get; set; } = new();
        public List<ProcesoViewModel> Procesos { get; set; } = new();
        public List<UsuarioViewModel> Usuarios { get; set; } = new();

        // Audit trail
        public int RegistradoPor { get; set; }
        public string RegistradoPorNombre { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int? ModificadoPor { get; set; }
        public string ModificadoPorNombre { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }

    public class MaestroListVM
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Proceso { get; set; }
        public string Responsable { get; set; }
        public string Estado { get; set; } // Activo/Inactivo
        public DateTime FechaRegistro { get; set; }
    }
}
```

**Validación**:
- ✅ ViewModels compilables
- ✅ DataAnnotations presentes
- ✅ Herencia correcta
- ✅ Propiedades para dropdowns

---

### TAREA 2.3: Expandir Adapter para Maestro (2h)

**Descripción**: Implementar métodos Dapper en GdMaestroAdapter

**Archivo**: `Data/Adapters/GD/GdMaestroAdapter.cs`

**Métodos a Implementar**:

```csharp
public interface IGdMaestroAdapter
{
    // Lectura
    Task<List<MaestroDocumentoVM>> ObtenerMaestros();
    Task<MaestroDocumentoVM> ObtenerMaestroById(int idMaestro);
    Task<DocumentoControlledVM> ObtenerControlledDocById(int idMaestro);

    // Creación
    Task<int> CrearMaestroConControlled(MaestroDocumentoVM vm);

    // Actualización (por tipo)
    Task<bool> ActualizarMaestroConstitucion(int idMaestro, MaestroDocumentoVM vm);
    Task<bool> ActualizarMaestroActualizacion(int idMaestro, MaestroDocumentoVM vm);

    // Anulación
    Task<bool> AnularMaestro(int idMaestro);
    Task<bool> AnularControlado(int idMaestro);

    // Dropdowns
    Task<List<TipoSolicitudViewModel>> ObtenerTiposSolicitud();
    Task<List<ProcesoViewModel>> ObtenerProcesos();
    Task<List<UsuarioViewModel>> ObtenerUsuarios();
}
```

**Implementación Parcial** (ejemplo método Crear):

```csharp
public class GdMaestroAdapter : IGdMaestroAdapter
{
    private readonly string _connectionString;

    public GdMaestroAdapter(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    public async Task<int> CrearMaestroConControlled(MaestroDocumentoVM vm)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // REGLA 4: Ejecutar SP exactamente como en WebMatrix
                    // SP: GD_MaestroDocumentos_Add2
                    var parameters = new DynamicParameters();
                    parameters.Add("@docNombre", vm.Nombre);
                    parameters.Add("@docCodigo", vm.Codigo);
                    parameters.Add("@docProceso", vm.IdProceso);
                    parameters.Add("@docResponsable", vm.IdResponsable);
                    parameters.Add("@ubicacion", vm.ControlledDoc.Ubicacion);
                    parameters.Add("@metodoRecuperacion", vm.ControlledDoc.MetodoRecuperacion);
                    parameters.Add("@tiempoRetencion", vm.ControlledDoc.TiempoRetencion);
                    parameters.Add("@disposicionFinal", vm.ControlledDoc.DisposicionFinal);
                    parameters.Add("@controlado", true); // Siempre controlado para maestro
                    parameters.Add("@activo", true);

                    var maestroId = await connection.QueryFirstOrDefaultAsync<int>(
                        "GD_MaestroDocumentos_Add2",
                        parameters,
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction);

                    // SP: GD_DocumentosControlados_Add (dentro de transacción)
                    var controlledParams = new DynamicParameters();
                    controlledParams.Add("@idMaestro", maestroId);
                    controlledParams.Add("@docUbicacion", vm.ControlledDoc.Ubicacion);
                    controlledParams.Add("@metodoRecuperacion", vm.ControlledDoc.MetodoRecuperacion);
                    controlledParams.Add("@tiempoRetencion", vm.ControlledDoc.TiempoRetencion);
                    controlledParams.Add("@disposicionFinal", vm.ControlledDoc.DisposicionFinal);
                    controlledParams.Add("@activo", true);

                    await connection.ExecuteAsync(
                        "GD_DocumentosControlados_Add",
                        controlledParams,
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction);

                    transaction.Commit();
                    return maestroId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Error creando maestro documento: {ex.Message}", ex);
                }
            }
        }
    }

    // Otros métodos...
}
```

**Validación**:
- ✅ Todos los métodos implementados
- ✅ Transacciones explícitas para operaciones múltiples
- ✅ SP names exactos (validado contra MAPEO_SP_MAESTRO.csv)
- ✅ Parámetros mapeados correctamente
- ✅ Manejo de excepciones con rollback

---

### TAREA 2.4: Expandir Service para Maestro (2h)

**Descripción**: Implementar lógica de negocio en GdMaestroService

**Archivo**: `Data/Services/GD/GdMaestroService.cs`

**Métodos a Implementar**:

```csharp
public interface IGdMaestroService
{
    // Lectura
    Task<(bool success, List<MaestroListVM> data)> ObtenerMaestros();
    Task<(bool success, MaestroUpdateVM data)> ObtenerMaestroById(int id);

    // Creación
    Task<(bool success, int id, string message)> CrearMaestro(MaestroCreateVM vm);

    // Actualización (por tipo de solicitud)
    Task<(bool success, string message)> ActualizarMaestro(int id, MaestroUpdateVM vm);

    // Anulación
    Task<(bool success, string message)> AnularMaestro(int id);

    // Dropdowns
    Task<(bool success, MaestroCreateVM formData)> ObtenerFormData();
}
```

**Implementación** (métodos clave):

```csharp
public class GdMaestroService : IGdMaestroService
{
    private readonly IGdMaestroAdapter _adapter;
    private readonly ILogger<GdMaestroService> _logger;

    public GdMaestroService(IGdMaestroAdapter adapter, ILogger<GdMaestroService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    public async Task<(bool success, int id, string message)> CrearMaestro(MaestroCreateVM vm)
    {
        try
        {
            // REGLA 12: Validar entrada
            if (string.IsNullOrWhiteSpace(vm.Nombre))
                return (false, 0, "El nombre del documento es requerido");

            if (vm.IdProceso <= 0)
                return (false, 0, "El proceso es requerido");

            // Validar por tipo (REGLA 6: paridad 1:1)
            var validacion = ValidarPorTipo(vm);
            if (!validacion.valid)
                return (false, 0, validacion.message);

            // Crear en BD (incluye transacción en adapter)
            var maestroId = await _adapter.CrearMaestroConControlled(vm);

            _logger.LogInformation($"Maestro documento creado: {maestroId} ({vm.Nombre})");
            return (true, maestroId, "Documento creado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creando maestro: {ex.Message}");
            return (false, 0, $"Error: {ex.Message}");
        }
    }

    public async Task<(bool success, string message)> AnularMaestro(int id)
    {
        try
        {
            // Anular maestro
            var resultMaestro = await _adapter.AnularMaestro(id);
            if (!resultMaestro)
                return (false, "Error anulando maestro");

            // Anular controlado
            var resultControlado = await _adapter.AnularControlado(id);
            if (!resultControlado)
                return (false, "Error anulando documento controlado");

            _logger.LogInformation($"Maestro anulado: {id}");
            return (true, "Documento anulado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error anulando maestro: {ex.Message}");
            return (false, $"Error: {ex.Message}");
        }
    }

    private (bool valid, string message) ValidarPorTipo(MaestroCreateVM vm)
    {
        // TipoSolicitud: 1=Construcción, 2=Actualización, 3=Anulación
        if (vm.TipoSolicitud == 1) // Construcción
        {
            if (string.IsNullOrWhiteSpace(vm.ControlledDoc?.Ubicacion))
                return (false, "La ubicación del documento controlado es requerida");
            if (string.IsNullOrWhiteSpace(vm.ControlledDoc?.MetodoRecuperacion))
                return (false, "El método de recuperación es requerido");
            if (vm.ControlledDoc?.TiempoRetencion <= 0)
                return (false, "El tiempo de retención debe ser mayor a 0");
            if (string.IsNullOrWhiteSpace(vm.ControlledDoc?.DisposicionFinal))
                return (false, "La disposición final es requerida");
        }
        else if (vm.TipoSolicitud == 2) // Actualización
        {
            // Validaciones de actualización (si aplican)
        }
        else if (vm.TipoSolicitud == 3) // Anulación
        {
            // Anulación requiere solo seleccionar documento existente
        }

        return (true, "");
    }
}
```

**Validación**:
- ✅ Métodos implementados
- ✅ Validación por tipo
- ✅ Logging presente
- ✅ Error handling completo
- ✅ Reglas 6, 11, 12 aplicadas

---

### TAREA 2.5: Crear DocumentosMaestroController (3h)

**Descripción**: Implementar controller CRUD para maestro documentos

**Archivo**: `Areas/GD/Controllers/DocumentosMaestroController.cs`

**Métodos a Implementar**:

| Método | HTTP | URL | Parámetros | Retorna | Notas |
|--------|------|-----|-----------|---------|-------|
| `Index` | GET | `/GD/DocumentosMaestro` | - | View (listado) | Grid paginado |
| `Create` | GET | `/GD/DocumentosMaestro/Create` | - | PartialView modal o View | Formulario crear |
| `Create` | POST | `/GD/DocumentosMaestro/Create` | MaestroCreateVM | JSON o Redirect | AJAX-friendly |
| `Edit` | GET | `/GD/DocumentosMaestro/Edit/{id}` | int id | PartialView modal o View | Formulario editar |
| `Edit` | POST | `/GD/DocumentosMaestro/Edit/{id}` | int id, MaestroUpdateVM | JSON o Redirect | AJAX-friendly |
| `Delete` | POST | `/GD/DocumentosMaestro/Delete/{id}` | int id | JSON | Confirmación |
| `GetFormData` | GET | `/GD/DocumentosMaestro/GetFormData` | - | JSON | Dropdowns para modal |

**Código Skeleton**:

```csharp
[Area("GD")]
[Authorize] // REGLA 11
public class DocumentosMaestroController : Controller
{
    private readonly IGdMaestroService _service;
    private readonly ILogger<DocumentosMaestroController> _logger;

    public DocumentosMaestroController(IGdMaestroService service, ILogger<DocumentosMaestroController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: /GD/DocumentosMaestro
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Accediendo a listado de maestro documentos");
        var (success, data) = await _service.ObtenerMaestros();
        return View(success ? data : new List<MaestroListVM>());
    }

    // GET: /GD/DocumentosMaestro/Create
    public async Task<IActionResult> Create()
    {
        var (success, formData) = await _service.ObtenerFormData();
        
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("_CreateMaestroModal", formData);
        
        return View(formData);
    }

    // POST: /GD/DocumentosMaestro/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MaestroCreateVM vm)
    {
        // REGLA 12: Validar entrada
        if (!ModelState.IsValid)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var (_, formData) = await _service.ObtenerFormData();
                return PartialView("_CreateMaestroModal", formData);
            }
            return View(vm);
        }

        var (success, id, message) = await _service.CrearMaestro(vm);

        if (success)
        {
            _logger.LogInformation($"Maestro creado: {id}");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = true, id, message });
            return RedirectToAction(nameof(Index));
        }

        // REGLA 13: Manejo de errores gracefully
        ModelState.AddModelError("", message);
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            var (_, formData) = await _service.ObtenerFormData();
            return PartialView("_CreateMaestroModal", formData);
        }
        return View(vm);
    }

    // GET: /GD/DocumentosMaestro/Edit/{id}
    public async Task<IActionResult> Edit(int id)
    {
        var (success, data) = await _service.ObtenerMaestroById(id);
        if (!success)
            return NotFound();

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("_EditMaestroModal", data);
        
        return View(data);
    }

    // POST: /GD/DocumentosMaestro/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MaestroUpdateVM vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, message = "Datos inválidos" });

        var (success, message) = await _service.ActualizarMaestro(id, vm);

        if (success)
        {
            _logger.LogInformation($"Maestro actualizado: {id}");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = true, message });
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", message);
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("_EditMaestroModal", vm);
        return View(vm);
    }

    // POST: /GD/DocumentosMaestro/Delete/{id}
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, message) = await _service.AnularMaestro(id);

        if (success)
        {
            _logger.LogInformation($"Maestro anulado: {id}");
            return Json(new { success = true, message });
        }

        return Json(new { success = false, message });
    }

    // GET: /GD/DocumentosMaestro/GetFormData (AJAX)
    [HttpGet]
    public async Task<IActionResult> GetFormData()
    {
        var (success, formData) = await _service.ObtenerFormData();
        return Json(new { success, data = formData });
    }
}
```

**Validación**:
- ✅ [Authorize] en todos los métodos
- ✅ ModelState validado
- ✅ Logging presente
- ✅ AJAX support
- ✅ JSON responses
- ✅ Error handling

---

### TAREA 2.6: Crear Vistas para Maestro (3h)

**Descripción**: Crear vistas de listado y modales para maestro

**Archivos a Crear** (en `Areas/GD/Views/DocumentosMaestro/`):

| Archivo | Tipo | Contenido |
|---------|------|----------|
| `Index.cshtml` | View | Grid con botones crear/editar/eliminar |
| `_CreateMaestroModal.cshtml` | PartialView | Form modal crear (3 tipos) |
| `_EditMaestroModal.cshtml` | PartialView | Form modal editar |
| `_DocumentoControlledPartial.cshtml` | PartialView | Section de documento controlado (condicionado por tipo) |

**Vista Index.cshtml** (estructura):

```html
@model List<MaestroListVM>

@{
    ViewData["Title"] = "Maestro de Documentos";
}

<div class="container-fluid mt-4">
    <h2>Maestro de Documentos</h2>

    <button class="btn btn-primary mb-3" id="btnCreateMaestro">
        <i class="fas fa-plus"></i> Nuevo Documento
    </button>

    <table class="table table-striped table-hover" id="tblMaestro">
        <thead>
            <tr>
                <th>Nombre</th>
                <th>Código</th>
                <th>Proceso</th>
                <th>Responsable</th>
                <th>Estado</th>
                <th>Creado</th>
                <th>Acciones</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var item in Model ?? new List<MaestroListVM>())
            {
                <tr>
                    <td>@item.Nombre</td>
                    <td><span class="badge bg-info">@item.Codigo</span></td>
                    <td>@item.Proceso</td>
                    <td>@item.Responsable</td>
                    <td>
                        @if (item.Estado == "Activo")
                        {
                            <span class="badge bg-success">Activo</span>
                        }
                        else
                        {
                            <span class="badge bg-danger">Inactivo</span>
                        }
                    </td>
                    <td>@item.FechaRegistro.ToString("dd/MM/yyyy")</td>
                    <td>
                        <button class="btn btn-sm btn-warning btnEdit" data-id="@item.Id">
                            <i class="fas fa-edit"></i>
                        </button>
                        <button class="btn btn-sm btn-danger btnDelete" data-id="@item.Id" 
                                @if (item.Estado != "Activo") { <text> disabled </text> }>
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
            $('#btnCreateMaestro').click(function() {
                $.get('@Url.Action("Create")', function(html) {
                    $('#ajaxModal').html(html).modal('show');
                });
            });

            $(document).on('click', '.btnEdit', function() {
                var id = $(this).data('id');
                $.get('@Url.Action("Edit")/' + id, function(html) {
                    $('#ajaxModal').html(html).modal('show');
                });
            });

            $(document).on('click', '.btnDelete', function() {
                if (confirm('¿Está seguro de anular este documento?')) {
                    var id = $(this).data('id');
                    $.post('@Url.Action("Delete")/' + id, function(result) {
                        if (result.success) {
                            toastr.success(result.message);
                            setTimeout(() => location.reload(), 1000);
                        } else {
                            toastr.error(result.message);
                        }
                    });
                }
            });
        });
    </script>
}
```

**Modal Crear** (_CreateMaestroModal.cshtml):
- Form con campos: Nombre, Código, TipoSolicitud (dropdown), Proceso, Responsable
- Section condicional `_DocumentoControlledPartial` mostrado si TipoSolicitud = 1 (Construcción)
- JavaScript para mostrar/ocultar campos según tipo seleccionado

**Validación**:
- ✅ Vistas compilables
- ✅ CSRF tokens presentes
- ✅ Modales Bootstrap
- ✅ Validación client-side
- ✅ Condicionalidad por tipo

---

### TAREA 2.7: Testing de Maestro (1h)

**Descripción**: Validar funcionalidad CRUD de maestro

**Checklist**:

- [ ] Index carga sin errores
- [ ] Create modal se abre
- [ ] Crear documento Construcción exitosamente
- [ ] Tabla maestro + controlado se insertan (verificar BD)
- [ ] Listado muestra nuevo documento
- [ ] Edit modal se abre con datos correos
- [ ] Actualizar documento exitosamente
- [ ] Botón eliminar anula documento (soft delete)
- [ ] Validaciones por tipo funcionan
- [ ] Errores se muestran gracefully

**Validación**:
- ✅ 100% funcional
- ✅ Sin errores de runtime
- ✅ Transacciones funcionan

---

### Registro de Completitud - Sprint 2

| Tarea | Horas | Estado |
|-------|-------|--------|
| 2.1 Mapear SP maestro | 1h | ⏳ |
| 2.2 ViewModels maestro | 1.5h | ⏳ |
| 2.3 Adapter maestro | 2h | ⏳ |
| 2.4 Service maestro | 2h | ⏳ |
| 2.5 Controller maestro | 3h | ⏳ |
| 2.6 Vistas maestro | 3h | ⏳ |
| 2.7 Testing maestro | 1h | ⏳ |
| **TOTAL SPRINT 2** | **16h** | **⏳** |

---

## 🚀 SPRINT 3: REPOSITORIO VERSIONADO

### Objetivo

Implementar repositorio de documentos con versionamiento automático, upload/descarga de archivos y asociación a contenedores (trabajos, proyectos).

**Horas Estimadas**: 20h  
**Duración**: 4-5 días  
**Criterio de Éxito**:
- ✅ Upload funcional con UploadService
- ✅ Versionamiento automático (MAX+1)
- ✅ Listado con paginación
- ✅ Descarga de archivos
- ✅ Eliminación con referencia de trabajo
- ✅ 0 archivos huérfanos

---

### TAREA 3.1: Validar UploadService Existente (1h)

**Descripción**: Localizar y documentar API de UploadService en MatrixNext

**Regla Aplicable**: REGLA 7 - Aprovechar elementos visuales disponibles

**Proceso**:

1. **Buscar UploadService**:
   - ¿En `Data/Services/`?
   - ¿En `Data/Adapters/`?
   - Documentar ubicación exacta

2. **Mapear Interfaz**:
   ```csharp
   public interface IUploadService
   {
       Task<(bool success, string fileName, string fullPath)> UploadFileAsync(IFormFile file, string folder);
       Task<bool> DeleteFileAsync(string filePath);
       Task<FileStream> DownloadFileAsync(string filePath);
       bool ValidateFile(IFormFile file, string[] allowedExtensions, long maxSize);
   }
   ```

3. **Documentar en MAPEO_UPLOAD_SERVICE.md**:
   - Ubicación exacta
   - Métodos disponibles
   - Parámetros y retornos
   - Carpeta destino (si configurable)
   - Validaciones soportadas

**Validación**:
- ✅ UploadService localizado
- ✅ API documentada
- ✅ Ejemplos de uso encontrados

---

### TAREA 3.2: Mapear SP de Repositorio (1h)

**Descripción**: Documentar SP para repositorio documentos

**Proceso** (similar a 2.1):

1. Buscar en `CoreProject/GD_Procedimientos.vb` métodos de repositorio
2. Documentar cada SP en MAPEO_SP_REPOSITORIO.csv
3. Validar contra CO_Matrix_Structure_SP.sql

**SP Expected**:

| Método VB | SP Name | Parámetros | Retorna | Notas |
|-----------|---------|-----------|---------|-------|
| `obtenerDocumentos()` | `GD_RepositorioDocumentos_GetXTrabajo` | idTrabajo, nombreDoc, urlArchivo, documentoId, version, ... | DataTable | Listado versionado |
| `guardarRepositorioDoc()` | `GD_RepositorioDocumentos_Add` | idContenedor, tipoContenedor, idDocumento, urlArchivo, version, comentarios, usuarioId, fecha | - | Insert archivo |
| `eliminarRepositorioDoc()` | `GD_EscanerDocumentos_Del` | idTrabajo, idDocumento | - | Soft delete |

**Validación**:
- ✅ MAPEO_SP_REPOSITORIO.csv creado
- ✅ SP validados en CO_Matrix_Structure_SP.sql

---

### TAREA 3.3: Crear ViewModels Repositorio (1.5h)

**Descripción**: ViewModels para operaciones de repositorio

**ViewModels**:

| ViewModel | Propiedades | Validaciones |
|-----------|------------|--------------|
| `RepositorioDocumentoVM` | `Id` (int), `IdContenedor` (int), `TipoContenedor` (int), `IdDocumento` (int), `UrlArchivo` (string), `Version` (decimal), `Comentarios` (string), `UsuarioId` (int), `FechaRegistro` (DateTime) | `[Required]` archivo, contenedor |
| `UploadDocumentoVM` | `IdContenedor` (int), `TipoContenedor` (int), `IdDocumento` (int), `Archivo` (IFormFile), `Comentarios` (string) | `[Required]` archivo; `[MaxFileSize]` validación |
| `RepositorioListVM` | `Id` (int), `NombreArchivo` (string), `Version` (decimal), `FechaRegistro` (DateTime), `RegistradoPor` (string), `Comentarios` (string) | N/A |

**Validación**:
- ✅ ViewModels compilables
- ✅ Atributos de validación presentes

---

### TAREA 3.4: Expandir Adapter Repositorio (2h)

**Descripción**: Implementar métodos Dapper en GdRepositorioAdapter

**Métodos**:

```csharp
public interface IGdRepositorioAdapter
{
    // Lectura
    Task<List<RepositorioListVM>> ObtenerDocumentos(int idContenedor, int tipoContenedor);
    Task<RepositorioDocumentoVM> ObtenerDocumentoById(int id);
    Task<decimal> ObtenerProximaVersion(int idContenedor, int idDocumento);

    // Creación
    Task<int> GuardarDocumento(RepositorioDocumentoVM vm);

    // Eliminación
    Task<bool> EliminarDocumento(int id);
    Task<List<RepositorioDocumentoVM>> ObtenerDocumentosContenedor(int idContenedor);
}
```

**Implementación** (métodos clave):

```csharp
public async Task<decimal> ObtenerProximaVersion(int idContenedor, int idDocumento)
{
    using (var connection = new SqlConnection(_connectionString))
    {
        var sql = @"
            SELECT ISNULL(MAX(version), 0) + 1 as proximaVersion
            FROM GD_RepositorioDocumentos
            WHERE idContenedor = @idContenedor AND idDocumento = @idDocumento
        ";
        var result = await connection.QueryFirstOrDefaultAsync<decimal>(sql, 
            new { idContenedor, idDocumento });
        return result;
    }
}

public async Task<int> GuardarDocumento(RepositorioDocumentoVM vm)
{
    using (var connection = new SqlConnection(_connectionString))
    {
        // Obtener próxima versión
        var proximaVersion = await ObtenerProximaVersion(vm.IdContenedor, vm.IdDocumento);

        var parameters = new DynamicParameters();
        parameters.Add("@idContenedor", vm.IdContenedor);
        parameters.Add("@tipoContenedor", vm.TipoContenedor);
        parameters.Add("@idDocumento", vm.IdDocumento);
        parameters.Add("@urlArchivo", vm.UrlArchivo);
        parameters.Add("@version", proximaVersion);
        parameters.Add("@comentarios", vm.Comentarios ?? "");
        parameters.Add("@usuarioId", vm.UsuarioId);
        parameters.Add("@fecha", DateTime.UtcNow.AddHours(-5));

        var idInsertion = await connection.QueryFirstOrDefaultAsync<int>(
            "GD_RepositorioDocumentos_Add",
            parameters,
            commandType: CommandType.StoredProcedure);

        return idInsertion;
    }
}
```

**Validación**:
- ✅ Métodos implementados
- ✅ Versionamiento automático (MAX+1)
- ✅ SP names validados

---

### TAREA 3.5: Expandir Service Repositorio (2h)

**Descripción**: Lógica de negocio en GdRepositorioService

**Métodos**:

```csharp
public interface IGdRepositorioService
{
    Task<(bool success, List<RepositorioListVM> data)> ObtenerDocumentos(int idContenedor, int tipoContenedor);
    Task<(bool success, int id, string message)> UploadDocumento(UploadDocumentoVM vm, IFormFile archivo);
    Task<(bool success, string message)> EliminarDocumento(int id);
    Task<(bool success, byte[] contenido)> DescargarDocumento(int id);
}
```

**Implementación** (método Upload):

```csharp
public async Task<(bool success, int id, string message)> UploadDocumento(UploadDocumentoVM vm, IFormFile archivo)
{
    try
    {
        // REGLA 12: Validar entrada
        if (archivo == null || archivo.Length == 0)
            return (false, 0, "Debe seleccionar un archivo");

        if (vm.IdContenedor <= 0 || vm.IdDocumento <= 0)
            return (false, 0, "Contenedor o documento inválido");

        // REGLA 7: Usar UploadService existente
        var (uploadSuccess, fileName, fullPath) = await _uploadService.UploadFileAsync(
            archivo, 
            $"GD/Repositorio/{vm.IdContenedor}");

        if (!uploadSuccess)
            return (false, 0, "Error al guardar archivo");

        // Crear registro en BD con ruta del archivo
        var repoVM = new RepositorioDocumentoVM
        {
            IdContenedor = vm.IdContenedor,
            TipoContenedor = vm.TipoContenedor,
            IdDocumento = vm.IdDocumento,
            UrlArchivo = fullPath,
            Comentarios = vm.Comentarios,
            UsuarioId = _currentUser.Id
        };

        var repoId = await _adapter.GuardarDocumento(repoVM);

        _logger.LogInformation($"Documento subido: {repoId} ({fileName})");
        return (true, repoId, $"Archivo {fileName} cargado exitosamente");
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error upload documento: {ex.Message}");
        return (false, 0, $"Error: {ex.Message}");
    }
}
```

**Validación**:
- ✅ Upload funcional
- ✅ Integración con UploadService
- ✅ Validación entrada
- ✅ Logging

---

### TAREA 3.6: Crear RepositorioController (3h)

**Descripción**: Implementar controller para repositorio

**Métodos**:

| Método | HTTP | URL | Parámetros | Retorna |
|--------|------|-----|-----------|---------|
| `Index` | GET | `/GD/Repositorio?IdContenedor=X&TipoContenedor=Y` | idContenedor, tipoContenedor | View listado |
| `Upload` | GET | `/GD/Repositorio/Upload` | idContenedor, tipoContenedor | PartialView modal |
| `Upload` | POST | `/GD/Repositorio/Upload` | UploadDocumentoVM, IFormFile | JSON |
| `Download` | GET | `/GD/Repositorio/Download/{id}` | int id | FileResult |
| `Delete` | POST | `/GD/Repositorio/Delete/{id}` | int id | JSON |

**Código Skeleton**:

```csharp
[Area("GD")]
[Authorize]
public class RepositorioController : Controller
{
    private readonly IGdRepositorioService _service;
    private readonly ILogger<RepositorioController> _logger;

    public RepositorioController(IGdRepositorioService service, ILogger<RepositorioController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: /GD/Repositorio?IdContenedor=1&TipoContenedor=1
    public async Task<IActionResult> Index(int idContenedor, int tipoContenedor)
    {
        _logger.LogInformation($"Accediendo repositorio: {idContenedor}, {tipoContenedor}");
        
        var (success, data) = await _service.ObtenerDocumentos(idContenedor, tipoContenedor);
        
        var vm = new RepositorioIndexVM
        {
            IdContenedor = idContenedor,
            TipoContenedor = tipoContenedor,
            Documentos = success ? data : new List<RepositorioListVM>()
        };

        return View(vm);
    }

    // GET: /GD/Repositorio/Upload
    public IActionResult Upload(int idContenedor, int tipoContenedor)
    {
        var vm = new UploadDocumentoVM
        {
            IdContenedor = idContenedor,
            TipoContenedor = tipoContenedor
        };

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("_UploadModal", vm);
        
        return View(vm);
    }

    // POST: /GD/Repositorio/Upload
    [HttpPost]
    public async Task<IActionResult> Upload(UploadDocumentoVM vm)
    {
        // REGLA 12: Validar entrada
        if (!ModelState.IsValid)
            return Json(new { success = false, message = "Datos inválidos" });

        var archivo = Request.Form.Files.FirstOrDefault("Archivo");
        if (archivo == null || archivo.Length == 0)
            return Json(new { success = false, message = "Archivo requerido" });

        var (success, id, message) = await _service.UploadDocumento(vm, archivo);

        if (success)
        {
            _logger.LogInformation($"Upload exitoso: {id}");
            return Json(new { success = true, id, message });
        }

        return Json(new { success = false, message });
    }

    // GET: /GD/Repositorio/Download/{id}
    public async Task<IActionResult> Download(int id)
    {
        var (success, contenido) = await _service.DescargarDocumento(id);
        
        if (!success)
            return NotFound();

        return File(contenido, "application/octet-stream", $"documento_{id}.pdf");
    }

    // POST: /GD/Repositorio/Delete/{id}
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, message) = await _service.EliminarDocumento(id);

        return Json(new { success, message });
    }
}
```

**Validación**:
- ✅ [Authorize] presente
- ✅ QueryString parámetros manejados
- ✅ File upload con validación
- ✅ File download
- ✅ AJAX support

---

### TAREA 3.7: Crear Vistas Repositorio (3h)

**Descripción**: Vistas para repositorio

**Archivos**:

| Archivo | Contenido |
|---------|----------|
| `Index.cshtml` | Grid listado versionado + botón upload + descarga/eliminar |
| `_UploadModal.cshtml` | Form upload archivo con comentarios |

**Index.cshtml** (estructura):

```html
@model RepositorioIndexVM

@{
    ViewData["Title"] = "Repositorio de Documentos";
}

<div class="container-fluid mt-4">
    <h2>Repositorio de Documentos</h2>
    
    <button class="btn btn-primary mb-3" id="btnUpload">
        <i class="fas fa-upload"></i> Subir Documento
    </button>

    <table class="table table-striped" id="tblRepositorio">
        <thead>
            <tr>
                <th>Nombre Archivo</th>
                <th>Versión</th>
                <th>Cargado Por</th>
                <th>Fecha</th>
                <th>Comentarios</th>
                <th>Acciones</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var doc in Model?.Documentos ?? new List<RepositorioListVM>())
            {
                <tr>
                    <td>@doc.NombreArchivo</td>
                    <td><span class="badge bg-info">v@doc.Version</span></td>
                    <td>@doc.RegistradoPor</td>
                    <td>@doc.FechaRegistro.ToString("dd/MM/yyyy HH:mm")</td>
                    <td>@doc.Comentarios</td>
                    <td>
                        <a href="@Url.Action("Download", new { id = doc.Id })" class="btn btn-sm btn-info" title="Descargar">
                            <i class="fas fa-download"></i>
                        </a>
                        <button class="btn btn-sm btn-danger btnDelete" data-id="@doc.Id" title="Eliminar">
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
            $('#btnUpload').click(function() {
                $.get('@Url.Action("Upload")', { 
                    idContenedor: @Model.IdContenedor, 
                    tipoContenedor: @Model.TipoContenedor 
                }, function(html) {
                    $('#ajaxModal').html(html).modal('show');
                });
            });

            $(document).on('click', '.btnDelete', function() {
                if (confirm('¿Está seguro?')) {
                    var id = $(this).data('id');
                    $.post('@Url.Action("Delete")/' + id, function(result) {
                        if (result.success) {
                            toastr.success(result.message);
                            setTimeout(() => location.reload(), 1000);
                        }
                    });
                }
            });
        });
    </script>
}
```

**Validación**:
- ✅ Vistas compilables
- ✅ Tabla con versionamiento visible
- ✅ Botones funcionales
- ✅ Modal AJAX

---

### TAREA 3.8: Testing Repositorio (1h)

**Descripción**: Validar repositorio funcional

**Checklist**:

- [ ] Index carga sin errores
- [ ] Upload modal se abre
- [ ] Subir archivo exitosamente
- [ ] Archivo se guarda en filesystem
- [ ] Registro en BD creado con versión correcta
- [ ] Listado muestra archivo v1.0
- [ ] Subir otro archivo del mismo documento
- [ ] Versión incrementa a 2.0 automáticamente
- [ ] Descargar archivo funciona
- [ ] Eliminar archivo funciona

**Validación**:
- ✅ 100% funcional
- ✅ Versionamiento automático
- ✅ Sin archivos huérfanos

---

### Registro de Completitud - Sprint 3

| Tarea | Horas | Estado |
|-------|-------|--------|
| 3.1 Validar UploadService | 1h | ⏳ |
| 3.2 Mapear SP repositorio | 1h | ⏳ |
| 3.3 ViewModels repositorio | 1.5h | ⏳ |
| 3.4 Adapter repositorio | 2h | ⏳ |
| 3.5 Service repositorio | 2h | ⏳ |
| 3.6 Controller repositorio | 3h | ⏳ |
| 3.7 Vistas repositorio | 3h | ⏳ |
| 3.8 Testing repositorio | 1h | ⏳ |
| **TOTAL SPRINT 3** | **20h** | **⏳** |

---

## ✅ CRITERIOS DE ÉXITO - FASE 2

**DEBE CUMPLIRSE ANTES DE PASAR A FASE 3**:

1. ✅ Maestro CRUD 100% funcional (Construcción/Actualización/Anulación)
2. ✅ Transacciones ACID funcionando (maestro + controlado insertan juntos)
3. ✅ Repositorio upload/descarga/delete 100% funcional
4. ✅ Versionamiento automático (MAX+1) validado
5. ✅ Menú actualizado con enlaces a DocumentosMaestro + Repositorio
6. ✅ 0 errores de compilación
7. ✅ SP exactamente como en WebMatrix (validado vs CO_Matrix_Structure_SP.sql)
8. ✅ Commit de cambios completo

---

**Fin de FASE 2**

Próxima: [Crear FASE 3 - Solicitudes + Aprobaciones + Workflow]

