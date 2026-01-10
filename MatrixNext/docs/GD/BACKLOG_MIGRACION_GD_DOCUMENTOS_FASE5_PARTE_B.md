# 📋 BACKLOG DE MIGRACIÓN - GD_Documentos FASE 5 PARTE B

**Fases**: FASE 5 PARTE B (Sprint 9)  
**Tema**: Escáner + Configuraciones  
**Horas Totales**: 18h  
**Duración Estimada**: 3-4 días (1 sprint)  
**Versión**: 1.0  
**Fecha**: 2026-01-09

---

## 📑 CONTENIDO

- [Resumen Ejecutivo](#resumen-ejecutivo)
- [Sprint 9: Escáner + Configuraciones](#sprint-9-escáner--configuraciones)

---

## 🎯 RESUMEN EJECUTIVO

### Objetivos de FASE 5 PARTE B

1. **Escáner** (P2-2, Sprint 9): 12h
   - Integración con servicio de escaneo existente
   - Captura de archivos PDF desde escáner
   - Auto-carga a repositorio o solicitud
   - Seguimiento de fuente escáner

2. **Configuraciones GD** (Nuevo, Sprint 9): 6h
   - Tabla de configuraciones
   - Admin panel para GD settings
   - Parámetros: límites, tipos archivo permitidos, etc.

### Dependencias Críticas

✅ **COMPLETADAS**:
- FASE 1-5 PARTE A: Todos módulos base + PNC
- BackgroundService de upload
- Email service

⚠️ **PENDIENTE**:
- Validar servicio escáner existente en MatrixNext
- API de integración con hardware escáner

### Reglas Aplicables

| Regla | Descripción | Prioridad |
|-------|-------------|-----------|
| REGLA 2 | Mapear SP exactamente | 🔴 CRÍTICA |
| REGLA 7 | Reutilizar servicios upload | 🟠 ALTA |
| REGLA 11 | Validar permisos | 🔴 CRÍTICA |
| REGLA 12 | Input validation | 🟠 ALTA |
| REGLA 14 | Async/await | 🟠 ALTA |

---

## 🚀 SPRINT 9: ESCÁNER + CONFIGURACIONES

### Objetivo

Integrar servicio escáner y crear panel de configuraciones GD.

**Horas Estimadas**: 18h  
**Duración**: 3-4 días  
**Criterio de Éxito**:
- ✅ Escáner captura archivos
- ✅ Auto-carga a repositorio
- ✅ Seguimiento escáner registrado
- ✅ Config panel funcional
- ✅ 0 errores
- ✅ Commit cambios

---

### TAREA 9.1: Investigar Servicio Escáner Existente (1.5h)

**Descripción**: Localizar e integrar API escáner

**Proceso**:

1. **Buscar implementación existente**:
   - ¿`IDocumentScannerService`, `IScannerService`, etc.?
   - ¿Ubicación en MatrixNext.Core o MatrixNext.Web?
   - ¿Métodos: `ScanDocumentAsync()`, `CapturePDFAsync()`, etc.?

2. **Documentar API**:
   ```csharp
   public interface IScannerService
   {
       Task<(bool success, string filePath, string mimeType)> ScanDocumentAsync(ScannerConfig config);
       Task<List<string>> ObtenerDispositivosEscaner();
       Task<bool> ProbarConexion(string dispositivoId);
   }
   ```

3. **Crear MAPEO_SCANNER_SERVICE.md**:
   ```markdown
   # Integración Escáner MatrixNext

   ## Servicio Escáner

   **Ubicación**: `Data/Services/Scanner/IScannerService`  
   **Métodos**:
   - `ScanDocumentAsync(ScannerConfig config)` → (bool success, string filePath, string mimeType)
   - `ObtenerDispositivosEscaner()` → List<string>
   - `ProbarConexion(string dispositivoId)` → bool

   ## Configuración

   En `appsettings.json`:
   ```json
   {
     "ScannerSettings": {
       "EnabledScanners": ["Canon", "Xerox"],
       "DefaultResolution": 300,
       "ColorMode": "RGB",
       "OutputFormat": "PDF"
     }
   }
   ```

   ## Flujo Escáner GD

   1. Usuario abre interfaz escáner en GD
   2. Selecciona dispositivo escáner
   3. Click "Escanear"
   4. Sistema llama IScannerService.ScanDocumentAsync()
   5. Archivo PDF generado en temp
   6. Auto-carga a repositorio o solicitud PNC
   ```

**Validación**:
- ✅ Escáner service localizado
- ✅ API documentada
- ✅ MAPEO_SCANNER_SERVICE.md creado

---

### TAREA 9.2: Crear ViewModel para Escáner (1h)

**Descripción**: ViewModels para interfaz escáner

**Ubicación**: `Models/ViewModels/GD/Scanner/`

**ViewModels**:

#### 1. ScannerConfigVM
```csharp
public class ScannerConfigVM
{
    public string DispositivoId { get; set; }
    public int Resolucion { get; set; } = 300; // DPI
    public string Modo { get; set; } = "RGB"; // B&W, Grayscale, RGB
    public int Paginas { get; set; } = 1; // Cantidad a escanear
    public bool BordeAutomatico { get; set; } = true;
    public string DestinoPor { get; set; } = "Repositorio"; // Repositorio o SolicitudPNC
}
```

#### 2. ScannerDispositivoVM
```csharp
public class ScannerDispositivoVM
{
    public string Id { get; set; }
    public string Nombre { get; set; }
    public string Estado { get; set; } // Online, Offline
    public bool Disponible { get; set; }
}
```

#### 3. ScannerResultVM
```csharp
public class ScannerResultVM
{
    public bool Success { get; set; }
    public string FilePath { get; set; }
    public string MimeType { get; set; }
    public int PaginasEscaneadas { get; set; }
    public string Mensaje { get; set; }
    public DateTime FechaEscaneo { get; set; }
}
```

**Validación**:
- ✅ ViewModels compilables
- ✅ Propiedades correctas

---

### TAREA 9.3: Crear Scanner Controller (2h)

**Descripción**: Controller para interfaz escáner

**Ubicación**: `Areas/GD/Controllers/ScannerController.cs`

**Métodos**:

```csharp
[Area("GD")]
[Authorize]
[Route("GD/Scanner")]
public class ScannerController : Controller
{
    private readonly IScannerService _scannerService;
    private readonly IGdPncService _pncService;
    private readonly IGdRepositorioService _repositorioService;
    private readonly ILogger<ScannerController> _logger;

    // GET: /GD/Scanner
    public async Task<IActionResult> Index()
    {
        var dispositivos = await _scannerService.ObtenerDispositivosEscaner();
        var vm = new ScannerIndexVM
        {
            Dispositivos = dispositivos.Select(d => new ScannerDispositivoVM 
            { 
                Id = d,
                Nombre = d,
                Disponible = true 
            }).ToList()
        };
        return View(vm);
    }

    // POST: /GD/Scanner/GetDispositivosAjax
    [HttpPost]
    public async Task<IActionResult> GetDispositivosAjax()
    {
        try
        {
            var dispositivos = await _scannerService.ObtenerDispositivosEscaner();
            var resultado = dispositivos.Select(d => new 
            { 
                id = d, 
                nombre = d 
            }).ToList();
            
            return Json(new { success = true, data = resultado });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error obteniendo dispositivos: {ex.Message}");
            return Json(new { success = false, message = ex.Message });
        }
    }

    // POST: /GD/Scanner/ProbarConexion
    [HttpPost]
    public async Task<IActionResult> ProbarConexion(string dispositivoId)
    {
        try
        {
            var conectado = await _scannerService.ProbarConexion(dispositivoId);
            return Json(new { success = conectado });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error probando conexión: {ex.Message}");
            return Json(new { success = false, message = ex.Message });
        }
    }

    // POST: /GD/Scanner/Escanear
    [HttpPost]
    public async Task<IActionResult> Escanear(ScannerConfigVM config)
    {
        try
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Configuración inválida" });

            // ✅ Llamar escáner
            var (success, filePath, mimeType) = await _scannerService.ScanDocumentAsync(config);

            if (!success)
                return Json(new { success = false, message = "Error escaneando documento" });

            // ✅ Auto-cargar según destino
            if (config.DestinoPor == "Repositorio")
            {
                // ⚠️ TODO: Cargar a repositorio de documento existente
                // Requiere: idDocumento, versión automática
            }
            else if (config.DestinoPor == "SolicitudPNC")
            {
                // ✅ Crear nueva solicitud PNC con documento escaneado
                var (pncSuccess, idSolicitud, pncMessage) = await CrearSolicitudPNCDesdeEscaneo(
                    filePath, 
                    mimeType);

                if (!pncSuccess)
                    return Json(new { success = false, message = pncMessage });

                return Json(new 
                { 
                    success = true, 
                    message = "Documento escaneado y solicitud PNC creada",
                    idSolicitud = idSolicitud,
                    redirectUrl = Url.Action("Detail", "Pnc", new { id = idSolicitud })
                });
            }

            return Json(new { success = true, message = "Documento escaneado exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error escaneando: {ex.Message}");
            return Json(new { success = false, message = $"Error: {ex.Message}" });
        }
    }

    // Método auxiliar
    private async Task<(bool, int, string)> CrearSolicitudPNCDesdeEscaneo(string filePath, string mimeType)
    {
        // ⚠️ TODO: Implementar lógica para crear solicitud PNC con archivo escaneado
        // 1. Obtener información escáner (usuario, dispositivo, etc.)
        // 2. Proponer valores por defecto (nombre = fecha + dispositivo)
        // 3. Llamar PncService.CrearSolicitud()
        return (true, 0, "OK");
    }
}
```

**Validación**:
- ✅ Controller compilable
- ✅ 4+ métodos
- ✅ Async/await
- ✅ Manejo de errores

---

### TAREA 9.4: Crear Vista Escáner (2h)

**Descripción**: Interfaz para escanear documentos

**Ubicación**: `Areas/GD/Views/Scanner/Index.cshtml`

**Contenido**:

```html
@model ScannerIndexVM

@{ ViewData["Title"] = "Escáner - Gestión Documental"; }

<div class="container-fluid mt-4">
    <h2>📠 Interfaz Escáner</h2>

    <div class="row">
        <!-- Panel Izquierda: Configuración -->
        <div class="col-md-6">
            <div class="card">
                <div class="card-header">
                    <h5>Configurar Escaneo</h5>
                </div>
                <div class="card-body">
                    <form id="formScanner">
                        <!-- Dispositivo -->
                        <div class="mb-3">
                            <label class="form-label">Dispositivo Escáner *</label>
                            <select id="dispositivoId" name="dispositivoId" class="form-select" required>
                                <option value="">Cargando dispositivos...</option>
                            </select>
                            <small class="form-text text-muted">
                                <button type="button" class="btn btn-link btn-sm p-0" id="btnProbar">
                                    Probar conexión
                                </button>
                            </small>
                            <div id="estadoConexion" class="mt-2"></div>
                        </div>

                        <!-- Resolución -->
                        <div class="mb-3">
                            <label class="form-label">Resolución (DPI)</label>
                            <select name="resolucion" class="form-select">
                                <option value="150">150 DPI (Baja - Rápido)</option>
                                <option value="200">200 DPI</option>
                                <option value="300" selected>300 DPI (Estándar)</option>
                                <option value="600">600 DPI (Alta - Lento)</option>
                            </select>
                        </div>

                        <!-- Modo Color -->
                        <div class="mb-3">
                            <label class="form-label">Modo Color</label>
                            <select name="modo" class="form-select">
                                <option value="B&W">Blanco y Negro (Pequeño)</option>
                                <option value="Grayscale">Escala de Grises</option>
                                <option value="RGB" selected>Color (RGB)</option>
                            </select>
                        </div>

                        <!-- Páginas -->
                        <div class="mb-3">
                            <label class="form-label">Cantidad Páginas</label>
                            <input type="number" name="paginas" class="form-control" value="1" min="1" max="999">
                        </div>

                        <!-- Borde automático -->
                        <div class="mb-3">
                            <div class="form-check">
                                <input type="checkbox" id="bordeAuto" name="bordeAutomatico" 
                                       class="form-check-input" checked>
                                <label class="form-check-label" for="bordeAuto">
                                    Detectar borde automáticamente
                                </label>
                            </div>
                        </div>

                        <!-- Destino -->
                        <div class="mb-3">
                            <label class="form-label">Destino *</label>
                            <select name="destinoPor" class="form-select" required>
                                <option value="SolicitudPNC">Crear Solicitud PNC (Nuevo documento)</option>
                                <option value="Repositorio">Agregar a Repositorio (Documento existente)</option>
                            </select>
                            <small class="form-text text-muted">
                                Selecciona dónde guardar el documento escaneado
                            </small>
                        </div>

                        <!-- Botones -->
                        <div class="d-grid gap-2">
                            <button type="button" id="btnEscanear" class="btn btn-primary btn-lg">
                                <i class="fas fa-scan"></i> Iniciar Escaneo
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>

        <!-- Panel Derecha: Dispositivos + Historial -->
        <div class="col-md-6">
            <!-- Estado Dispositivos -->
            <div class="card mb-3">
                <div class="card-header">
                    <h5>Estado Dispositivos</h5>
                </div>
                <div class="card-body">
                    <div id="listadoDispositivos">
                        <p class="text-muted">Cargando...</p>
                    </div>
                </div>
            </div>

            <!-- Últimos Escaneos -->
            <div class="card">
                <div class="card-header">
                    <h5>Últimos Escaneos</h5>
                </div>
                <div class="card-body">
                    <div id="ultimosEscaneos">
                        <p class="text-muted">Ningún escaneo reciente</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<!-- Modal Progreso Escaneo -->
<div id="modalEscaneo" class="modal fade" tabindex="-1">
    <div class="modal-dialog modal-sm">
        <div class="modal-content">
            <div class="modal-header">
                <h5>Escaneando...</h5>
            </div>
            <div class="modal-body">
                <div class="progress">
                    <div id="progressBar" class="progress-bar progress-bar-striped progress-bar-animated" 
                         role="progressbar" style="width: 0%"></div>
                </div>
                <p id="statusText" class="mt-3 text-center">Iniciando escaneo...</p>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    <script>
        let escaneoEnProgreso = false;

        $(document).ready(() => {
            CargarDispositivos();
        });

        // ========== CARGAR DISPOSITIVOS ==========
        function CargarDispositivos() {
            $.post('/GD/Scanner/GetDispositivosAjax', (result) => {
                if (result.success) {
                    const select = $('#dispositivoId');
                    select.empty();
                    result.data.forEach(d => {
                        select.append(`<option value="${d.id}">${d.nombre}</option>`);
                    });
                    ActualizarEstadoDispositivos();
                } else {
                    alert('Error cargando dispositivos: ' + result.message);
                }
            });
        }

        // ========== ACTUALIZAR ESTADO ==========
        function ActualizarEstadoDispositivos() {
            const dispositivos = $('#dispositivoId option').map((i, el) => el.value).get();
            let html = '';
            
            dispositivos.forEach(dev => {
                html += `
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        <span>${dev}</span>
                        <span id="status-${dev}" class="badge bg-secondary">Verificando...</span>
                    </div>
                `;
            });
            
            $('#listadoDispositivos').html(html);
        }

        // ========== PROBAR CONEXIÓN ==========
        $('#btnProbar').on('click', (e) => {
            e.preventDefault();
            const dispositivo = $('#dispositivoId').val();
            
            $.post('/GD/Scanner/ProbarConexion', { dispositivoId: dispositivo }, (result) => {
                const estado = result.success ? 
                    '<span class="badge bg-success">🟢 Conectado</span>' :
                    '<span class="badge bg-danger">🔴 Desconectado</span>';
                $('#estadoConexion').html(estado);
            });
        });

        // ========== INICIAR ESCANEO ==========
        $('#btnEscanear').on('click', async () => {
            if (escaneoEnProgreso) return;

            const config = {
                dispositivoId: $('#dispositivoId').val(),
                resolucion: parseInt($('[name="resolucion"]').val()),
                modo: $('[name="modo"]').val(),
                paginas: parseInt($('[name="paginas"]').val()),
                bordeAutomatico: $('#bordeAuto').is(':checked'),
                destinoPor: $('[name="destinoPor"]').val()
            };

            if (!config.dispositivoId) {
                alert('Selecciona un dispositivo');
                return;
            }

            escaneoEnProgreso = true;
            const modal = new bootstrap.Modal($('#modalEscaneo')[0]);
            modal.show();

            // Simular progreso
            let progreso = 0;
            const intervalo = setInterval(() => {
                progreso += Math.random() * 20;
                if (progreso > 90) progreso = 90;
                $('#progressBar').css('width', progreso + '%');
                $('#statusText').text(`Escaneando página ${Math.floor(progreso / 10)}...`);
            }, 500);

            try {
                const response = await fetch('/GD/Scanner/Escanear', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'X-CSRF-TOKEN': $('[name="__RequestVerificationToken"]').val()
                    },
                    body: JSON.stringify(config)
                });

                clearInterval(intervalo);
                const result = await response.json();

                $('#progressBar').css('width', '100%');
                $('#statusText').text(result.message);

                setTimeout(() => {
                    modal.hide();
                    escaneoEnProgreso = false;

                    if (result.success) {
                        alert('✅ ' + result.message);
                        if (result.redirectUrl) {
                            window.location.href = result.redirectUrl;
                        } else {
                            location.reload();
                        }
                    } else {
                        alert('❌ Error: ' + result.message);
                    }
                }, 2000);
            } catch (error) {
                clearInterval(intervalo);
                modal.hide();
                escaneoEnProgreso = false;
                alert('Error en escaneo: ' + error.message);
            }
        });
    </script>
}
```

**Validación**:
- ✅ Vista compilable
- ✅ Interfaz intuitiva
- ✅ Formulario validable
- ✅ JavaScript AJAX

---

### TAREA 9.5: Crear Configuraciones Service (2h)

**Descripción**: Service para gestionar configuraciones GD

**Ubicación**: `Data/Services/GD/GdConfigService.cs`

**Interfaz**:

```csharp
public interface IGdConfigService
{
    Task<GdConfigVM> ObtenerConfiguracion();
    Task<(bool success, string message)> ActualizarConfiguracion(GdConfigVM vm);
    
    // Métodos específicos
    Task<int> ObtenerLimiteTamañoArchivo(); // MB
    Task<List<string>> ObtenerTiposArchivoPermitidos();
    Task<int> ObtenerLimiteRevisores();
    Task<bool> EstaEscanerHabilitado();
}
```

**Modelo Configuración**:

```csharp
public class GdConfiguracion
{
    public int Id { get; set; }
    public int LimiteTamañoArchivoMB { get; set; } = 10;
    public string TiposArchivoPermitidos { get; set; } = "pdf,doc,docx,xls,xlsx,jpg,png"; // CSV
    public int LimiteRevisoresMaximo { get; set; } = 10;
    public int LimiteRevisionesPorDocumento { get; set; } = 5;
    public bool EscanerHabilitado { get; set; } = true;
    public bool EmailNotificacionesHabilitadas { get; set; } = true;
    public string ArchivosDescargarRuta { get; set; } = "~/Uploads/GD/";
    public bool PermitirActualizacionDocumentos { get; set; } = true;
    public bool PermitirAnulacionDocumentos { get; set; } = true;
    public bool AutoAprobarPNCUnRevisor { get; set; } = false;
    public DateTime FechaModificacion { get; set; }
    public int ModificadoPor { get; set; }
}
```

**ViewModel**:

```csharp
public class GdConfigVM
{
    [Range(1, 100)]
    public int LimiteTamañoArchivoMB { get; set; }

    [StringLength(500)]
    public string TiposArchivoPermitidos { get; set; }

    [Range(1, 20)]
    public int LimiteRevisoresMaximo { get; set; }

    [Range(1, 10)]
    public int LimiteRevisionesPorDocumento { get; set; }

    public bool EscanerHabilitado { get; set; }
    public bool EmailNotificacionesHabilitadas { get; set; }
    public bool PermitirActualizacionDocumentos { get; set; }
    public bool PermitirAnulacionDocumentos { get; set; }
    public bool AutoAprobarPNCUnRevisor { get; set; }

    public string ArchivosDescargarRuta { get; set; }
}
```

**Implementación** (estructura):

```csharp
public class GdConfigService : IGdConfigService
{
    private readonly IRepository<GdConfiguracion> _configRepo;
    private readonly ILogger<GdConfigService> _logger;
    private readonly IMemoryCache _cache;

    public async Task<GdConfigVM> ObtenerConfiguracion()
    {
        // Buscar en cache primero
        if (_cache.TryGetValue("GD_Config", out GdConfigVM config))
            return config;

        // Obtener de BD
        var dbConfig = await _configRepo.GetFirstAsync();
        
        var vm = new GdConfigVM
        {
            LimiteTamañoArchivoMB = dbConfig.LimiteTamañoArchivoMB,
            TiposArchivoPermitidos = dbConfig.TiposArchivoPermitidos,
            LimiteRevisoresMaximo = dbConfig.LimiteRevisoresMaximo,
            EscanerHabilitado = dbConfig.EscanerHabilitado,
            // ... resto de propiedades
        };

        // Cachear por 1 hora
        _cache.Set("GD_Config", vm, TimeSpan.FromHours(1));
        return vm;
    }

    public async Task<(bool success, string message)> ActualizarConfiguracion(GdConfigVM vm)
    {
        try
        {
            var config = await _configRepo.GetFirstAsync();
            if (config == null)
            {
                // Crear configuración por defecto
                config = new GdConfiguracion();
            }

            // Actualizar propiedades
            config.LimiteTamañoArchivoMB = vm.LimiteTamañoArchivoMB;
            config.TiposArchivoPermitidos = vm.TiposArchivoPermitidos;
            config.LimiteRevisoresMaximo = vm.LimiteRevisoresMaximo;
            config.EscanerHabilitado = vm.EscanerHabilitado;
            config.FechaModificacion = DateTime.UtcNow.AddHours(-5);

            await _configRepo.UpdateAsync(config);

            // Invalidar cache
            _cache.Remove("GD_Config");

            _logger.LogInformation("Configuración GD actualizada");
            return (true, "Configuración actualizada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error actualizando config: {ex.Message}");
            return (false, $"Error: {ex.Message}");
        }
    }

    public async Task<int> ObtenerLimiteTamañoArchivo()
    {
        var config = await ObtenerConfiguracion();
        return config.LimiteTamañoArchivoMB;
    }

    public async Task<List<string>> ObtenerTiposArchivoPermitidos()
    {
        var config = await ObtenerConfiguracion();
        return config.TiposArchivoPermitidos
            .Split(',')
            .Select(x => x.Trim())
            .ToList();
    }

    // ... resto de métodos
}
```

**Validación**:
- ✅ Service implementado
- ✅ Caché configuración
- ✅ Métodos específicos
- ✅ Async/await

---

### TAREA 9.6: Crear ConfiguracionController (1.5h)

**Descripción**: Admin panel para configuraciones

**Ubicación**: `Areas/GD/Controllers/ConfiguracionController.cs`

**Métodos**:

```csharp
[Area("GD")]
[Authorize(Roles = "Admin")]  // ⚠️ Solo admin
[Route("GD/Configuracion")]
public class ConfiguracionController : Controller
{
    private readonly IGdConfigService _service;
    private readonly ILogger<ConfiguracionController> _logger;

    // GET: /GD/Configuracion
    public async Task<IActionResult> Index()
    {
        var config = await _service.ObtenerConfiguracion();
        return View(config);
    }

    // POST: /GD/Configuracion
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(GdConfigVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var (success, message) = await _service.ActualizarConfiguracion(vm);

        if (success)
        {
            TempData["Success"] = message;
            _logger.LogInformation("Configuración actualizada");
            return RedirectToAction("Index");
        }

        TempData["Error"] = message;
        return View(vm);
    }
}
```

**Validación**:
- ✅ Controller compilable
- ✅ Autorización admin
- ✅ Validaciones

---

### TAREA 9.7: Crear Vista Configuración (1.5h)

**Descripción**: Interfaz admin para configuraciones

**Ubicación**: `Areas/GD/Views/Configuracion/Index.cshtml`

**Contenido**:

```html
@model GdConfigVM

@{ ViewData["Title"] = "Configuración - GD"; }

<div class="container-fluid mt-4">
    <h2>⚙️ Configuración Gestión Documental</h2>

    @if (TempData["Success"] != null)
    {
        <div class="alert alert-success alert-dismissible fade show" role="alert">
            @TempData["Success"]
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    }

    @if (TempData["Error"] != null)
    {
        <div class="alert alert-danger alert-dismissible fade show" role="alert">
            @TempData["Error"]
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    }

    <form method="post">
        @Html.AntiForgeryToken()

        <div class="row">
            <!-- Sección 1: Límites -->
            <div class="col-md-6">
                <div class="card mb-3">
                    <div class="card-header">
                        <h5>📏 Límites</h5>
                    </div>
                    <div class="card-body">
                        <div class="mb-3">
                            <label class="form-label">Tamaño Máximo Archivo (MB)</label>
                            <input type="number" asp-for="LimiteTamañoArchivoMB" class="form-control" 
                                   min="1" max="100">
                            <span asp-validation-for="LimiteTamañoArchivoMB" class="text-danger"></span>
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Máximo Revisores por Documento</label>
                            <input type="number" asp-for="LimiteRevisoresMaximo" class="form-control" 
                                   min="1" max="20">
                            <span asp-validation-for="LimiteRevisoresMaximo" class="text-danger"></span>
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Máximo Revisiones por Documento</label>
                            <input type="number" asp-for="LimiteRevisionesPorDocumento" class="form-control">
                        </div>
                    </div>
                </div>
            </div>

            <!-- Sección 2: Formatos -->
            <div class="col-md-6">
                <div class="card mb-3">
                    <div class="card-header">
                        <h5>📁 Formatos Permitidos</h5>
                    </div>
                    <div class="card-body">
                        <label class="form-label">Extensiones (separadas por coma)</label>
                        <textarea asp-for="TiposArchivoPermitidos" class="form-control" rows="4"
                                  placeholder="pdf,doc,docx,xls,xlsx,jpg,png"></textarea>
                        <small class="form-text text-muted">
                            Ejemplo: pdf,doc,docx,xls,xlsx,jpg,png
                        </small>
                        <span asp-validation-for="TiposArchivoPermitidos" class="text-danger"></span>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <!-- Sección 3: Features -->
            <div class="col-md-6">
                <div class="card mb-3">
                    <div class="card-header">
                        <h5>✨ Características</h5>
                    </div>
                    <div class="card-body">
                        <div class="form-check mb-2">
                            <input type="checkbox" asp-for="EscanerHabilitado" class="form-check-input" id="chkEscaner">
                            <label class="form-check-label" for="chkEscaner">
                                Habilitar Escáner
                            </label>
                        </div>

                        <div class="form-check mb-2">
                            <input type="checkbox" asp-for="EmailNotificacionesHabilitadas" 
                                   class="form-check-input" id="chkEmail">
                            <label class="form-check-label" for="chkEmail">
                                Habilitar Notificaciones Email
                            </label>
                        </div>

                        <div class="form-check mb-2">
                            <input type="checkbox" asp-for="PermitirActualizacionDocumentos" 
                                   class="form-check-input" id="chkActualizar">
                            <label class="form-check-label" for="chkActualizar">
                                Permitir Actualización de Documentos
                            </label>
                        </div>

                        <div class="form-check mb-2">
                            <input type="checkbox" asp-for="PermitirAnulacionDocumentos" 
                                   class="form-check-input" id="chkAnular">
                            <label class="form-check-label" for="chkAnular">
                                Permitir Anulación de Documentos
                            </label>
                        </div>

                        <div class="form-check mb-2">
                            <input type="checkbox" asp-for="AutoAprobarPNCUnRevisor" 
                                   class="form-check-input" id="chkAutoPNC">
                            <label class="form-check-label" for="chkAutoPNC">
                                Auto-aprobar PNC con Un Revisor
                            </label>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Sección 4: Rutas -->
            <div class="col-md-6">
                <div class="card mb-3">
                    <div class="card-header">
                        <h5>📂 Almacenamiento</h5>
                    </div>
                    <div class="card-body">
                        <div class="mb-3">
                            <label class="form-label">Ruta Descargas</label>
                            <input type="text" asp-for="ArchivosDescargarRuta" class="form-control"
                                   placeholder="~/Uploads/GD/">
                            <small class="form-text text-muted">
                                Ruta relativa o absoluta para guardar archivos
                            </small>
                        </div>

                        <div class="alert alert-info">
                            <strong>💾 Espacio Utilizado:</strong> 
                            <br>Repositorio: Calculado dinámicamente
                            <br>Archivos Temporales: Limpieza automática
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Botones -->
        <div class="mb-3">
            <button type="submit" class="btn btn-primary">💾 Guardar Cambios</button>
            <a href="@Url.Action("Index", "Dashboard")" class="btn btn-secondary">Cancelar</a>
        </div>
    </form>
</div>
```

**Validación**:
- ✅ Vista compilable
- ✅ Formulario validable
- ✅ Diseño admin intuitivo

---

### TAREA 9.8: Registrar en Program.cs (0.5h)

**Código**:

```csharp
// Configuraciones GD
builder.Services.AddScoped<IGdConfigService, GdConfigService>();

// Escáner
builder.Services.AddScoped<IScannerService, ScannerService>(); // ⚠️ Usar existente
```

**Validación**:
- ✅ Servicios registrados
- ✅ Compilación exitosa

---

### TAREA 9.9: Actualizar Menú Sidebar (0.5h)

**Descripción**: Agregar links escáner + config a menú

**Ubicación**: `Areas/GD/Views/Shared/_Sidebar.cshtml`

**Agregar**:

```html
<!-- Escáner -->
<li>
    <a href="@Url.Action("Index", "Scanner")">
        <i class="fas fa-scanner"></i> Escáner
    </a>
</li>

<!-- Configuración (solo admin) -->
@if (User.IsInRole("Admin"))
{
    <li>
        <a href="@Url.Action("Index", "Configuracion")">
            <i class="fas fa-cog"></i> Configuración
        </a>
    </li>
}
```

**Validación**:
- ✅ Menú actualizado

---

### TAREA 9.10: Testing Escáner + Config (1.5h)

**Descripción**: Validar funcionalidad

**Escenarios**:

1. **Escáner**:
   - [ ] Acceder a `/GD/Scanner`
   - [ ] Dispositivos cargan en dropdown
   - [ ] Probar conexión funciona
   - [ ] Cambiar configuración (resolución, modo, etc.)
   - [ ] Click "Iniciar Escaneo"
   - [ ] Progreso se muestra
   - [ ] Documento escaneado
   - [ ] Auto-crear PNC correctamente

2. **Configuración**:
   - [ ] Acceder a `/GD/Configuracion` (admin solo)
   - [ ] Cargar valores actuales
   - [ ] Modificar límite tamaño
   - [ ] Modificar tipos archivo
   - [ ] Modificar features
   - [ ] Guardar
   - [ ] Valores persistidos
   - [ ] Cache invalidado

**Validación**:
- ✅ Escáner funcional end-to-end
- ✅ Configuraciones guardadas
- ✅ Restricciones aplicadas
- ✅ 0 errores

---

### Registro de Completitud - Sprint 9

| Tarea | Horas | Estado |
|-------|-------|--------|
| 9.1 Investigar Escáner | 1.5h | ⏳ |
| 9.2 ViewModels Escáner | 1h | ⏳ |
| 9.3 Scanner Controller | 2h | ⏳ |
| 9.4 Vista Escáner | 2h | ⏳ |
| 9.5 Config Service | 2h | ⏳ |
| 9.6 Config Controller | 1.5h | ⏳ |
| 9.7 Config Vista | 1.5h | ⏳ |
| 9.8 Program.cs | 0.5h | ⏳ |
| 9.9 Menú Sidebar | 0.5h | ⏳ |
| 9.10 Testing | 1.5h | ⏳ |
| **TOTAL SPRINT 9** | **18h** | **⏳** |

---

## ✅ CRITERIOS DE ÉXITO - FASE 5 PARTE B

**DEBE CUMPLIRSE ANTES DE PASAR A FASE 6**:

1. ✅ Escáner captura documentos correctamente
2. ✅ Auto-carga a PNC o repositorio
3. ✅ Configuraciones guardadas
4. ✅ Restricciones aplicadas (límites, formatos)
5. ✅ Panel admin funcional (solo admin)
6. ✅ Menú actualizado
7. ✅ 0 errores compilación
8. ✅ Commit cambios

---

**Fin de FASE 5 PARTE B**

**TOTAL FASE 5**: 58h (40h PARTE A + 18h PARTE B)

→ Próxima: [FASE 6 - Testing Integral + Documentación Final]

