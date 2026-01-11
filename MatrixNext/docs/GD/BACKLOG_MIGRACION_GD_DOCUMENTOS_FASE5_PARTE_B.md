# 📋 BACKLOG DE MIGRACIÓN - GD_Documentos FASE 5 PARTE B

**Fases**: FASE 5 PARTE B (Sprint 9)  
**Tema**: Configuraciones + Testing (Escáner omitido)  
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
> ACTUALIZACIÓN 2026-01-10: Se omite la migración del módulo de Escáner.
> Continuamos con Configuraciones de GD y Testing E2E del módulo PNC.

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

## 🚀 SPRINT 9: CONFIGURACIONES + TESTING (SIN ESCÁNER)

### Objetivo

Completar configuraciones de GD y ejecutar testing E2E del módulo **PNC** y flujos asociados.

**Horas Estimadas**: 18h  
**Duración**: 3-4 días  
**Criterio de Éxito**:
- ✅ Navegación a PNC desde el menú
- ✅ Controlador MVC UI para PNC funcional
- ✅ Rutas y DI configuradas (IPncService)
- ✅ appsettings con cadenas de conexión correctas
- ✅ Testing E2E de creación, edición, cierre PNC
- ✅ 0 errores críticos
- ✅ Commits y documentación

---

### TAREA 9.1: Configurar UI MVC PNC (2h)

**Descripción**: Crear controlador MVC (UI) para servir las vistas Razor del módulo PNC.

**Ubicación**: `MatrixNext.Web/Controllers/PncUiController.cs`

**Acciones**:
- `Index()` → Renderiza listado (Views/Pnc/Index.cshtml)
- `Crear()` → Renderiza formulario (Views/Pnc/Crear.cshtml)
- `Seguimiento()` → Renderiza dashboard (Views/Pnc/Seguimiento.cshtml)
- `Detalle(int id)` → Carga modelo vía `IPncService.ObtenerPncById(id)` y renderiza detalle

**Validación**:
- ✅ Compila y navega a `/Pnc`, `/Pnc/Crear`, `/Pnc/Seguimiento`, `/Pnc/Detalle/{id}`
- ✅ Inyección de `IPncService` funcionando

---

### TAREA 9.2: Navegación en Layout (1h)

**Descripción**: Agregar entrada del menú lateral hacia PNC.

**Ubicación**: `MatrixNext.Web/Views/Shared/layouts/_main-sidebar.cshtml`

**Cambio**:
- Reemplazar entrada de área GD por enlace directo: `/Pnc`

**Validación**:
- ✅ Menú muestra "Productos No Conformes" y navega a `/Pnc`

---

### TAREA 9.3: appsettings y DI (2h)

**Descripción**: Validar `appsettings.json` y registro de DI para PNC.

**Acciones**:
- Verificar cadena de conexión utilizada por `PncAdapter` (via `IConfiguration`)
- Confirmar registro de `IPncService` y `IPncAdapter` en `Program.cs` / `Startup.cs`
- Revisar CORS/JWT si aplica para llamadas AJAX desde vistas

**Métodos**:
**Validación**:
- ✅ `PncAdapter` obtiene connection string correcta
- ✅ Servicios registrados en DI sin errores
- ✅ Peticiones AJAX autenticadas

---

### TAREA 9.4: Testing E2E PNC (8h)

**Descripción**: Pruebas de extremo a extremo para el módulo PNC.

**Escenarios**:
- Crear PNC con causas iniciales
- Agregar causa en detalle
- Agregar acción inmediata (validación ISO 9001)
- Agregar acción correctiva/preventiva
- Ejecutar acción con evidencia
- Validar cierre PNC (pre-check)
- Cerrar PNC y verificar notificaciones

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
                    **Herramientas**:
                    - Navegación UI (Razor)
                    - Llamadas API (PncController `api/pnc/*`)
                    - Logs de `ILogger` en Service y Controller

                    **Validación**:
                    - ✅ Todos los escenarios pasan sin errores
                    - ✅ Mensajes claros al usuario en errores

                    ### TAREA 9.5: Fixes y Deploy (4h)

                    **Descripción**: Correcciones detectadas en pruebas y preparación para deploy.

                    **Acciones**:
                    - Ajustes menores UI/UX en vistas
                    - Revisión de permisos `[Authorize]`
                    - Documentación breve de uso PNC
                    - Preparar variables de entorno y connection strings para staging/producción

                    **Validación**:
                    - ✅ Sin warnings críticos
                    - ✅ Documentación actualizada
                    - ✅ Preparado para deployment
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

