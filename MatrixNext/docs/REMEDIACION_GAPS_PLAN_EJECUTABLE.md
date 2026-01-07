# PLAN DE REMEDIACIÓN - ORDEN DE EJECUCIÓN

**Fecha Inicio:** 9 Enero 2026  
**Objetivo:** Resolver 28 gaps en 4 fases  
**Esfuerzo Total:** 28-32 horas (~4 días)

---

## ✅ Ajustes y Alineación (enero 2026)

Durante la ejecución y revisión del código real se detectaron divergencias con el plan original. Se incorporan los siguientes ajustes para mantener el plan alineado con la implementación efectiva del repositorio:

- Seguridad (PYPermisosService): el sistema usa tablas/consultas legacy. Decisión pendiente: 1) mapear por EF las tablas `UsuariosPermisos/UsuariosRoles/Roles` y mantener el servicio sobre EF, o 2) mantener consultas legacy (Dapper/SQL) y documentar los mapeos. Recomendado: mantener el enfoque legacy por consistencia con otros lookups.
- Uploads: el plan proponía persistencia en BD con `ArchivoDTO`/ID; la implementación vigente es filesystem por carpeta `uploads/{moduleId}/{entityId}` con endpoints por ruta. Se formaliza el enfoque filesystem y se pospone la variante BD como mejora futura.
- Indicadores: el campo `WorkFlow.FechaCompletacion` no existe; se usa `FechaModificacion` como proxy para el promedio de días de completación en `IndicadoresCumplimientoService`.
- Metodologías (Trabajos): el plan sugería EF; la implementación usa servicio de catálogo legacy (Dapper) con SP-fallback y `ViewBag` en controlador. Se mantiene este enfoque.
- Brief auto-propuesta: requisito previo explícito: relajar validación en `PropuestaService` para permitir `EstadoId=1` sin `FechaInicioCampo` (ya aplicado).
- WorkFlowDataAdapter (SP): se valida la existencia de los SP por nombre fijo migrado desde CoreProject (sin overrides por configuración) usando `sys.objects`. Esto asegura que la migración esté correcta y evita nombres divergentes por ambiente.
- Higiene de build/solución: se crea una solución dedicada a MatrixNext (solo `MatrixNext.Data` y `MatrixNext.Web`) para eliminar referencias rotas a proyectos externos y permitir builds consistentes.
- Warnings de nulabilidad: se añade tarea P3 para reducir CS8618/CS8625 visibles en modelos/servicios PY.
- Testing mínimo: se añade tarea para smoke tests de flujos Brief→Propuesta, Trabajos con Metodologías, Uploads y endpoints de Indicadores.
- Configuración: auditar que existan las cadenas `MatrixDb` y la conexión legacy utilizada por los lookups (p.ej. `LegacyDatabase`), y documentar el uso de secrets/variables por entorno.

---

## 🔴 FASE 1: CRÍTICOS (P0) - BLOQUEAN TODO

### T1.1: Implementar PYPermisosService - SEGURIDAD

**Archivo:** `MatrixNext.Web\Services\PYPermisosService.cs`

**Problema:**
```csharp
// Método 1: Siempre aprueba
public async Task<bool> VerificarPermisoAsync(int permisoId, long usuarioId)
{
    // TODO: Implementar consulta a BD
    return true; // ← SECURITY BYPASS
}

// Método 2: Siempre aprueba
public async Task<bool> VerificarRolAsync(long usuarioId, string rolNombre)
{
    // TODO: Implementar consulta a BD
    return true; // ← SECURITY BYPASS
}

// Método 3: Siempre retorna vacío
public async Task<List<int>> ObtenerPermisosUsuarioAsync(long usuarioId)
{
    // TODO: Implementar consulta a BD
    return new List<int>(); // ← EMPTY
}
```

**Solución:**
Necesitas agregar al DbContext (si no están):
```csharp
// En MatrixDbContext
public DbSet<UsuarioPermiso> UsuariosPermisos { get; set; }
public DbSet<UsuarioRol> UsuariosRoles { get; set; }
public DbSet<Rol> Roles { get; set; }
```

Reemplazar PYPermisosService.cs completo:
```csharp
using MatrixNext.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services
{
    public interface IPYPermisosService
    {
        Task<bool> VerificarPermisoAsync(int permisoId, long usuarioId);
        Task<bool> VerificarRolAsync(long usuarioId, string rolNombre);
        Task<List<int>> ObtenerPermisosUsuarioAsync(long usuarioId);
    }

    public class PYPermisosService : IPYPermisosService
    {
        private readonly MatrixDbContext _context;
        private readonly ILogger<PYPermisosService> _logger;

        public PYPermisosService(MatrixDbContext context, ILogger<PYPermisosService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>Verifica si un usuario tiene un permiso específico</summary>
        public async Task<bool> VerificarPermisoAsync(int permisoId, long usuarioId)
        {
            try
            {
                var tienePermiso = await _context.UsuariosPermisos
                    .Where(up => up.IdUsuario == usuarioId && up.IdPermiso == permisoId)
                    .AnyAsync();

                return tienePermiso;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando permiso {PermisoId} para usuario {UsuarioId}", 
                    permisoId, usuarioId);
                return false; // Por seguridad, retorna false si hay error
            }
        }

        /// <summary>Verifica si un usuario tiene un rol específico</summary>
        public async Task<bool> VerificarRolAsync(long usuarioId, string rolNombre)
        {
            try
            {
                var tieneRol = await _context.UsuariosRoles
                    .Where(ur => ur.IdUsuario == usuarioId)
                    .Join(_context.Roles,
                        ur => ur.IdRol,
                        r => r.Id,
                        (ur, r) => r)
                    .Where(r => r.Nombre == rolNombre)
                    .AnyAsync();

                return tieneRol;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando rol {RolNombre} para usuario {UsuarioId}",
                    rolNombre, usuarioId);
                return false; // Por seguridad, retorna false
            }
        }

        /// <summary>Obtiene lista de permisos de un usuario</summary>
        public async Task<List<int>> ObtenerPermisosUsuarioAsync(long usuarioId)
        {
            try
            {
                var permisos = await _context.UsuariosPermisos
                    .Where(up => up.IdUsuario == usuarioId)
                    .Select(up => up.IdPermiso)
                    .ToListAsync();

                return permisos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo permisos para usuario {UsuarioId}", usuarioId);
                return new List<int>(); // Retorna lista vacía como fallback
            }
        }
    }
}
```

**Entidades necesarias** (si no existen):
```csharp
// Models/UsuarioPermiso.cs
public class UsuarioPermiso : BaseEntity
{
    public long IdUsuario { get; set; }
    public int IdPermiso { get; set; }
    public DateTime FechaAsignacion { get; set; } = DateTime.Now;
}

// Models/UsuarioRol.cs
public class UsuarioRol : BaseEntity
{
    public long IdUsuario { get; set; }
    public int IdRol { get; set; }
    public DateTime FechaAsignacion { get; set; } = DateTime.Now;
}

// Models/Rol.cs
public class Rol : BaseEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
}
```

**Esfuerzo:** 3 horas  
**Impacto:** CRÍTICO - Sistema obtiene seguridad real

---

## 🟠 FASE 2: ALTOS (P1) - ANTES DE PRODUCCIÓN

### T2.1: IndicadoresCumplimientoService - PROMEDIO REAL

**Archivo:** `MatrixNext.Web\Services\CORE\IndicadoresCumplimientoService.cs`

**Problema:**
```csharp
// Línea 39 - Hardcodeado
PromedioDiasCompletacion = 5.5m // Simplificado
```

**Solución:**
```csharp
// Calcular promedio real
var tareasCompletadas = tareas
    .Where(t => t.Estado == "Completada" && 
                t.FechaCreacion != null && 
                t.FechaCompletacion != null)
    .ToList();

var promedioDias = tareasCompletadas.Any()
    ? (decimal)tareasCompletadas
        .Average(t => (t.FechaCompletacion!.Value - t.FechaCreacion!.Value).TotalDays)
    : 0m;

var resumen = new IndicadoresResumenDTO
{
    // ... otros campos ...
    PromedioDiasCompletacion = Math.Round(promedioDias, 2) // Real, no hardcodeado
};
```

**Esfuerzo:** 30 minutos

---

### T2.2: _Upload.cshtml - LISTADO DE ARCHIVOS

**Archivo:** `MatrixNext.Web\Views\Shared\_Upload.cshtml`

**Paso 1:** Crear DTO
```csharp
// ViewModels/ArchivoDTO.cs
public class ArchivoDTO
{
    public long Id { get; set; }
    public string NombreOriginal { get; set; }
    public string RutaAlmacenamiento { get; set; }
    public long Tamaño { get; set; }
    public DateTime FechaSubida { get; set; }
    public long IdEntidad { get; set; }
    public string TipoEntidad { get; set; }
}
```

**Paso 2:** Extender UploadService
```csharp
// En IUploadService
Task<List<ArchivoDTO>> ListarArchivosAsync(long entityId, string entityType);

// En UploadService
public async Task<List<ArchivoDTO>> ListarArchivosAsync(long entityId, string entityType)
{
    var archivos = await _context.Archivos
        .Where(a => a.IdEntidad == entityId && a.TipoEntidad == entityType)
        .Select(a => new ArchivoDTO
        {
            Id = a.Id,
            NombreOriginal = a.NombreOriginal,
            RutaAlmacenamiento = a.RutaAlmacenamiento,
            Tamaño = a.Tamaño,
            FechaSubida = a.FechaSubida,
            IdEntidad = a.IdEntidad,
            TipoEntidad = a.TipoEntidad
        })
        .OrderByDescending(a => a.FechaSubida)
        .ToListAsync();

    return archivos;
}
```

**Paso 3:** Agregar endpoints en UploadController
```csharp
[HttpGet("list")]
public async Task<IActionResult> ListarArchivos(long entityId, string entityType)
{
    try
    {
        var archivos = await _uploadService.ListarArchivosAsync(entityId, entityType);
        return Json(ResultVM<List<ArchivoDTO>>.Ok(archivos, "Archivos listados"));
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error listando archivos");
        return Json(ResultVM<List<ArchivoDTO>>.Fail("Error listando archivos"));
    }
}

[HttpGet("download/{id}")]
public async Task<IActionResult> DescargarArchivo(long id)
{
    try
    {
        var archivo = await _context.Archivos.FindAsync(id);
        if (archivo == null)
            return NotFound();

        var bytes = await System.IO.File.ReadAllBytesAsync(archivo.RutaAlmacenamiento);
        return File(bytes, "application/octet-stream", archivo.NombreOriginal);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error descargando archivo {Id}", id);
        return StatusCode(500);
    }
}

[HttpDelete("delete/{id}")]
public async Task<IActionResult> EliminarArchivo(long id)
{
    try
    {
        var archivo = await _context.Archivos.FindAsync(id);
        if (archivo == null)
            return Json(ResultVM<bool>.Fail("Archivo no encontrado"));

        if (System.IO.File.Exists(archivo.RutaAlmacenamiento))
            System.IO.File.Delete(archivo.RutaAlmacenamiento);

        _context.Archivos.Remove(archivo);
        await _context.SaveChangesAsync();

        return Json(ResultVM<bool>.Ok(true, "Archivo eliminado"));
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error eliminando archivo {Id}", id);
        return Json(ResultVM<bool>.Fail("Error eliminando archivo"));
    }
}
```

**Paso 4:** Actualizar _Upload.cshtml
```javascript
function cargarArchivos() {
    const entityId = $('#entityId').val();
    const entityType = $('#entityType').val();
    
    if (!entityId || !entityType) return;
    
    $.ajax({
        url: '/api/upload/list',
        method: 'GET',
        data: { entityId: entityId, entityType: entityType },
        success: function(result) {
            if (result.success) {
                mostrarListaArchivos(result.data);
            }
        },
        error: function() {
            console.error('Error cargando archivos');
        }
    });
}

function mostrarListaArchivos(archivos) {
    const $lista = $('#archivosList');
    $lista.empty();
    
    if (!archivos || archivos.length === 0) {
        $lista.html('<p class="text-muted">No hay archivos subidos</p>');
        return;
    }
    
    archivos.forEach(archivo => {
        const fechaFormato = new Date(archivo.fechaSubida).toLocaleDateString();
        const tamaño = (archivo.tamaño / 1024).toFixed(2); // KB
        
        const item = `
            <div class="archivo-item mb-2 p-2 border rounded">
                <div class="d-flex justify-content-between align-items-center">
                    <div>
                        <strong>${archivo.nombreOriginal}</strong>
                        <small class="text-muted d-block">${fechaFormato} - ${tamaño} KB</small>
                    </div>
                    <div>
                        <a href="/api/upload/download/${archivo.id}" class="btn btn-sm btn-primary me-1">
                            <i class="fas fa-download"></i> Descargar
                        </a>
                        <button onclick="eliminarArchivo(${archivo.id})" class="btn btn-sm btn-danger">
                            <i class="fas fa-trash"></i>
                        </button>
                    </div>
                </div>
            </div>
        `;
        
        $lista.append(item);
    });
}

function eliminarArchivo(archivoId) {
    if (!confirm('¿Está seguro de que desea eliminar este archivo?')) return;
    
    $.ajax({
        url: `/api/upload/delete/${archivoId}`,
        method: 'DELETE',
        success: function(result) {
            if (result.success) {
                // Recargar lista
                cargarArchivos();
                mostrarMensaje('Archivo eliminado correctamente');
            }
        },
        error: function() {
            mostrarMensaje('Error eliminando archivo', 'error');
        }
    });
}

// Llamar al cargar la página
$(document).ready(function() {
    cargarArchivos();
});

// Llamar después de subir archivo
document.getElementById('fileInput').addEventListener('change', function() {
    // Esperar a que se complete el upload
    setTimeout(function() {
        cargarArchivos();
    }, 1000);
});
```

**Esfuerzo:** 2 horas

---

### T2.3: IQuoteCalculatorService - PRODUCTIVIDAD ONLINE

**Archivo:** `MatrixNext.Data\Services\CU\IQuoteCalculatorService.cs`

**Problema:**
```csharp
// Línea 68 - Hardcodeado
if (tecCodigo == 300)
{
    return 1000; // Placeholder - depende de panel
}
```

**Solución:**
```csharp
// Consultar maestro de productividad por técnica
if (tecCodigo == 300)
{
    // Online: obtener productividad del maestro
    var productividadOnline = _masters.GetProductividadOnline(
        muestra, 
        duracionMinutos, 
        tipoEncuesta
    ) ?? 800m; // Default 800 si no hay valor en maestro
    
    return Math.Round(productividadOnline, 2);
}
```

**Nota:** Requiere crear tabla maestro `eq_productividad_online` o consultar la existente.

**Esfuerzo:** 2 horas

---

### T2.4: EstudioService - CARGAR PRESUPUESTOS

**Archivo:** `MatrixNext.Data\Services\CU\EstudioService.cs`

**Problema:**
```csharp
// TODO-P0-02: Cargar presupuestos asignados si es edición
// TODO-P0-02: Obtener presupuestos aprobados de la propuesta
// TODO-P0-02: Asignar presupuestos aprobados al estudio
```

**Solución:**
Los comentarios indican que el código YA INTENTA hacer esto, solo necesita validación:

```csharp
// Línea 51-60: Ya carga presupuestos asignados
var presupuestosAsignados = _presupuestoAdapter.ObtenerPresupuestosAsignadosXEstudio(idEstudio.Value);
vm.Estudio.PresupuestosSeleccionados = presupuestosAsignados.Select(p => p.Id).ToList();

// Línea 72-75: Ya obtiene presupuestos aprobados
vm.PresupuestosAprobados = _presupuestoAdapter.ObtenerPresupuestosAprobados(idPropuesta.Value);

// Línea 118-124: Ya asigna presupuestos
_presupuestoAdapter.AsignarPresupuestosAEstudio(id, model.PresupuestosSeleccionados);
```

**Acción:** Solo remover comentarios TODO, validar que adapters existen

**Esfuerzo:** 1 hora (testing)

---

## 🟡 FASE 3: MEDIOS (P2) - PRÓXIMO SPRINT

### T3.1: TareasConfigController - AUDIT TRAIL

**Archivo:** `MatrixNext.Web\Areas\CORE\Controllers\TareasConfigController.cs`

**Problema:**
```csharp
// Línea 115 y 200
tarea.UsuarioCreacion = 1; // TODO: UsuarioCreacion debe ser el ID del usuario actual (long)
tarea.UsuarioModificacion = 1; // TODO: UsuarioModificacion debe ser el ID del usuario actual (long)
```

**Solución:**
```csharp
private long ObtenerUsuarioActualId()
{
    var userIdClaim = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
    if (userIdClaim != null && long.TryParse(userIdClaim.Value, out var userId))
    {
        return userId;
    }
    return 0; // Usuario no autenticado
}

// En Create
tarea.UsuarioCreacion = ObtenerUsuarioActualId();

// En Edit
tarea.UsuarioModificacion = ObtenerUsuarioActualId();
```

**Aplicar también en:**
- ProyectosController
- TrabajosController
- Otros controllers que crean/editan

**Esfuerzo:** 1 hora

---

### T3.2: Trabajos Views - DROPDOWN METODOLOGÍAS

**Archivos:** 
- `MatrixNext.Web\Areas\PY\Views\Trabajos\_GridTable.cshtml` (línea 23)
- `MatrixNext.Web\Areas\PY\Views\Trabajos\_CreateEdit.cshtml` (línea 23)

**Paso 1:** Actualizar ViewModel
```csharp
public class TrabajoCreateEditVM
{
    public Trabajo Trabajo { get; set; }
    public List<MetodologiaDTO> Metodologias { get; set; } // ← Agregar
}
```

**Paso 2:** Controller - cargar catálogo
```csharp
public IActionResult Create(int? idProyecto)
{
    var vm = new TrabajoCreateEditVM
    {
        Trabajo = new Trabajo { IdProyecto = idProyecto ?? 0 },
        Metodologias = _context.Metodologias
            .Where(m => m.Activo)
            .Select(m => new MetodologiaDTO { Id = m.Id, Nombre = m.Nombre })
            .ToList()
    };
    return View(vm);
}
```

**Paso 3:** _GridTable.cshtml
```razor
<!-- Antes -->
<td>@item.IdMetodologia</td>

<!-- Después -->
<td>@item.Metodologia?.Nombre ?? "Sin metodología"</td>
```

**Paso 4:** _CreateEdit.cshtml
```razor
<!-- Antes -->
<input asp-for="IdMetodologia" class="form-control" />

<!-- Después -->
<select asp-for="Trabajo.IdMetodologia" class="form-control">
    <option value="">-- Seleccione metodología --</option>
    @foreach (var m in Model.Metodologias)
    {
        <option value="@m.Id" selected="@(m.Id == Model.Trabajo.IdMetodologia)">
            @m.Nombre
        </option>
    }
</select>
```

**Esfuerzo:** 2 horas

---

### T3.3: BriefService - AUTO-CREAR PROPUESTA

**Archivo:** `MatrixNext.Data\Services\CU\BriefService.cs`

**Problema:**
```csharp
// TODO-P0-01: Auto-crear propuesta cuando es un Brief nuevo
if (esNuevo)
{
    // Lógica pendiente
}
```

**Solución:**
```csharp
// Después de guardar el Brief nuevo
if (esNuevo)
{
    try
    {
        var propuesta = new PropuestaViewModel
        {
            BriefId = id,
            Titulo = $"Propuesta para {entidad.Titulo}",
            Descripcion = entidad.Complicacion,
            Estado = "Borrador",
            FechaCreacion = DateTime.Now,
            UsuarioCreacion = usuarioId
        };
        
        var (success, message, propuestaId) = _propuestaService.Guardar(propuesta);
        
        if (success)
        {
            _logger.LogInformation("Propuesta {PropuestaId} auto-creada para Brief {BriefId}", 
                propuestaId, id);
        }
        else
        {
            _logger.LogWarning("Error auto-creando propuesta para Brief {BriefId}: {Message}", 
                id, message);
        }
    }
    catch (Exception exProp)
    {
        _logger.LogError(exProp, "Error al auto-crear propuesta para Brief {BriefId}", id);
        // No falla el Brief, solo registra el error
    }
}
```

**Esfuerzo:** 3 horas

---

## 🟢 FASE 4: BAJOS (P3) - MANTENIMIENTO

### T4.1: WorkFlowDataAdapter - VALIDAR SP NAME

**Archivo:** `MatrixNext.Web\Services\CORE\WorkFlowDataAdapter.cs` (línea 56)

**Acción (actualizado):**
1. Validación de existencia del SP en código (`sys.objects`) con los nombres estándar migrados desde CoreProject: `CORE_WorkFlow_GetXTrabajoXTarea`, `CORE_WorkFlow_Get`, `CORE_WorkFlow_CrearHiloCrearTareas`, `CORE_Log_WorkFlow_MasivoEstadoCreada_Add`.
2. No se usarán overrides por configuración; si la validación falla, revisar la migración o los nombres de SP en BD.

**Esfuerzo:** 15 minutos

---

## 📋 MATRIZ DE EJECUCIÓN

| Tarea | P | Archivo | Líneas | Tipo | H | Bloqueador |
|-------|---|---------|--------|------|---|-----------|
| T1.1 | 🔴 | PYPermisosService | 23-63 | Implement | 3 | SÍ |
| T2.1 | 🟠 | IndicadoresCumplimiento | 39 | Fix value | 0.5 | NO |
| T2.2 | 🟠 | _Upload + Controller | 108-110 | Implement | 2 | NO |
| T2.3 | 🟠 | IQuoteCalculatorService | 68 | Fix value | 2 | NO |
| T2.4 | 🟠 | EstudioService | 51-124 | Validate | 1 | NO |
| T3.1 | 🟡 | TareasConfigController | 115,200 | Fix | 1 | NO |
| T3.2 | 🟡 | Trabajos Views | 23 x2 | UX | 2 | NO |
| T3.3 | 🟡 | BriefService | 126 | Implement | 3 | NO |
| T4.1 | 🟢 | WorkFlowDataAdapter | 56 | Validate | 0.25 | NO |

**Total:** 14.75 horas (≈2 días de desarrollo focus)

---

## ➕ Nuevas tareas incorporadas (alineación)

| Tarea | P | Archivo/Área | Tipo | H | Notas |
|------|---|---------------|------|---|-------|
| NX1 | 🟠 | Build/Solución | Crear solución `MatrixNext.sln` con `MatrixNext.Data` y `MatrixNext.Web` | 0.5 | Elimina proyectos faltantes del build global |
| NX2 | 🟠 | Subida de archivos | Consolidar enfoque filesystem (listar/descargar/eliminar por ruta) y actualizar `_Upload` | 1 | BD como mejora futura |
| NX3 | 🟡 | Indicadores | Usar `FechaModificacion` como proxy de completación | 0.25 | Campo `FechaCompletacion` no existe |
| NX4 | 🟡 | Metodologías | Mantener lookup legacy via Dapper + `ViewBag` | 0.5 | Confirmar conexión legacy en config |
| NX5 | 🟡 | Brief/Propuesta | Validación relajada para `EstadoId=1` sin fecha | 0.25 | Ya aplicado |
| NX6 | 🟢 | Configuración | Asegurar `ConnectionStrings:LegacyDatabase` está presente | 0.25 | Requerido por catálogos legacy |
| NX7 | 🟢 | Nulabilidad | Reducir CS8618/CS8625 en modelos/servicios PY | 2 | Mantenimiento |
| NX8 | 🟠 | Smoke tests | Flujos Brief→Propuesta, Trabajos, Uploads, Indicadores | 1 | Validación funcional |

---

## 🧩 Solución dedicada MatrixNext

Para aislar `MatrixNext` de referencias faltantes en la solución global, se crea una solución enfocada solo en los proyectos activos:

1. `MatrixNext.Data`
2. `MatrixNext.Web`

Uso recomendado (PowerShell):

```powershell
cd .\MatrixNext
dotnet new sln -n MatrixNext
dotnet sln .\MatrixNext.sln add .\MatrixNext.Data\MatrixNext.Data.csproj
dotnet sln .\MatrixNext.sln add .\MatrixNext.Web\MatrixNext.Web.csproj
dotnet build .\MatrixNext.sln -c Debug
```

Esta solución permite compilar y ejecutar la web sin depender de proyectos legacy ausentes.

## 🚀 PRÓXIMOS PASOS

1. **Hoy:** Comenzar T1.1 (PYPermisosService) - CRÍTICO
2. **Mañana:** T2.1-T2.4 (ALTOS)
3. **Semana siguiente:** T3.1-T3.3 (MEDIOS)
4. **Cuando haya tiempo:** T4.1 (BAJO)

---

**Generado:** 9 Enero 2026  
**Prioridad:** P0 P1 P2 P3  
**Versión:** 1.0 Plan Ejecutable
