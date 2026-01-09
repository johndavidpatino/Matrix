# 📋 SPRINT 6 PLAN - OP_CUALITATIVO COMPLEMENTARIOS
**Fecha inicio**: 9 de enero de 2026  
**Duración estimada**: 62 horas (4 fases)  
**Scope**: P1 Complementarios (2) + P2 Críticos (5) del backlog original  
**Directriz**: MÁXIMA REUTILIZACIÓN - Verificar qué ya existe antes de crear

---

## 🎯 OBJETIVO

Completar funcionalidades complementarias identificadas en Sprint 5:
- ✅ P1: Transcription (8h), Scheduling (14h) 
- ✅ P2: Sample (6h), Calendar/Gantt (10h), IPS Integration (16h), Email/Hangfire (12h)

**Nota**: Sprint 5 completó todos los FLUJOS CRÍTICOS (Trabajos → Filtros → Aprobación). Sprint 6 agrega complementos que enriquecen pero NO son bloqueadores para producción.

---

## 📊 MAPEO DE SERVICIOS REUTILIZABLES

### Servicios EXISTENTES (No crear nuevos)

| Servicio | Ubicación | Métodos Disponibles | Para Sprint 6 |
|----------|-----------|-------------------|--------------|
| `IOpTrabajosService` | Sprint 0 | GetAll, GetById, Filter, Navigate | Contexto para Transcription/Scheduling |
| `IOpMuestraService` | Existente | GetMuestras, GetByTrabajo, Filter | **Sample Controller** |
| `IOpEstimacionService` | Existente | GetEstimaciones, CalcularProduccion | Estimaciones para Scheduling |
| `IOpIpsService` | Sprint 3 | ObtenerRevisionesAsync, ExportarAsync | IPS ya implementado ✅ |
| `IExportService` | Compartido | ExportarExcel, ExportarCsv | Excel para Transcription/Sample |
| `IEmailService` | Compartido | EnviarAsync, EnviarPorTemplate | Email notifications |
| `IOpProgramacionService` | Sprint 3 | ObtenerProgramaciones, Exportar ICS | Datos para Calendar |
| `ILogger<T>` | Built-in | LogInformation, LogError | Auditoría estándar |
| `IWorkFlowService` | CORE | ActualizarEstado, GetEstado | Estados workflow para Scheduling |

### Servicios a EXTENDER (No crear nuevos)

**Caso 1: Transcription** → Reutilizar `IOpFichasTecnicasService` 
- Ya existe método `GuardarFicha()` y `ObtenerFicha()`
- Transcripción es variante de Ficha con tipo adicional
- **Decisión**: Agregar método `GuardarTranscripcion()` a servicio existente

**Caso 2: Scheduling** → Reutilizar `IOpProgramacionService`
- Ya tiene `ObtenerProgramacionesPorTrabajoAsync()` 
- Scheduling es análogo a Programación con detalles de horario
- **Decisión**: Extender servicio con `ActualizarHorarioAsync()`, `ObtenerDisponibilidadAsync()`

**Caso 3: IPS Emails** → Reutilizar `IEmailService` + Hangfire
- Ya existe patrón de email en MatrixNext.Web
- **Decisión**: Configurar Hangfire job para `EnviarNotificacionIpsAsync()`

---

## 🔍 VERIFICACIÓN DE EXISTENCIA PREVIO

Antes de implementar CUALQUIER controller/service en Sprint 6, ejecutar:

```bash
# 1. Buscar controlador existente
file_search "**/*TranscriptionController*"
file_search "**/*SchedulingController*"
file_search "**/*SampleController*"
file_search "**/*CalendarController*"

# 2. Verificar servicios en Program.cs
grep_search "AddScoped.*IOpTranscriptionService|AddScoped.*IOpSchedulingService"

# 3. Verificar vistas
file_search "**/*Transcription*View*"
file_search "**/*Scheduling*View*"

# 4. Verificar DTOs/ViewModels
file_search "**/*TranscriptionVm*|**/*SchedulingVm*"

# 5. Verificar rutas en Area
list_dir "MatrixNext.Web/Areas/OP/Controllers"
```

---

## 📝 TAREAS SPRINT 6 DETALLADAS

### FASE 1: Transcription (8h) - P1 Complementario

**Descripción**: Captura y almacenamiento de transcripciones de sesiones  
**Bloquea**: Nothing (P1 complementario)  
**Depende de**: FichasController ✅, TrabajosController ✅  

#### 1.1 Controller Implementation (3h)

```csharp
// File: MatrixNext.Web/Areas/OP/Controllers/CualitativoTranscripcionController.cs
// Basado en patrón Sprint 5 + extensiones FichasController

[Area("OP")]
[Authorize]
[Route("OP/Cualitativo/Transcripcion")]
public class CualitativoTranscripcionController : Controller
{
    private readonly IOpFichasTecnicasService _fichasService;  // REUTILIZAR
    private readonly IOpTrabajosService _trabajosService;     // REUTILIZAR
    
    // Actions (7 total):
    // - Index: GET grid con transccripciones por trabajo (DataTable)
    // - Create: GET form vacío (tipo ficha = 4)
    // - Create: POST save via FichasService.GuardarFicha(TipoFicha=4)
    // - Edit: GET cargar transcripción existente
    // - Edit: POST actualizar
    // - Delete: POST soft-delete vía workflow
    // - ExportWord: GET exportar a DOCX via ClosedXML (si aplica)
}
```

**Reutilización**: 
- ✅ `IOpFichasTecnicasService` para CRUD (sin crear nuevo service)
- ✅ `FichaTecnicaVm` existente (agregar propiedad `TipoFicha = 4`)
- ✅ Validaciones de presupuesto ya existen en `ValidateBudgetAsync()`

#### 1.2 Views (2h)

```
Create/Edit/Delete/Index (4 vistas)
│
├─ Index.cshtml (Grid con DataTable, 6 columnas: Id, Trabajo, Tipo, FechaCreacion, Estado, Acciones)
├─ Create.cshtml (Reutilizar formulario EditInterview.cshtml con TipoFicha=4)
├─ Edit.cshtml (Idem Create)
└─ Delete.cshtml (Confirmación soft-delete)

Reutilización: EditInterview.cshtml ya soporta 3 tipos (Entrevista/Sesión/Observación)
→ Agregar condicional para TipoFicha=4 (Transcripción)
```

#### 1.3 Database Changes (1h)

```sql
-- NO SE REQUIERE: TipoFicha ya existe en OP_FichasTecnica
-- Solo agregar índice si falta:
CREATE INDEX IX_FichasTecnicas_TipoFicha ON OP_FichasTecnica(TipoFicha);
```

#### 1.4 Testing (2h)
- ✅ E2E: Create → Edit → Delete transcripción
- ✅ Validar presupuesto (reutilizar test de Fichas)
- ✅ Verificar soft-delete vía workflow

**Estimación Total Transcription**: 8h ✅

---

### FASE 2: Scheduling (14h) - P1 Complementario

**Descripción**: Gestión de horarios y disponibilidad de entrevistadores  
**Bloquea**: Nothing (P1 complementario)  
**Depende de**: ProgramacionController ✅, TrabajosController ✅  

#### 2.1 Service Extension (4h)

**NO CREAR NUEVO SERVICE** - Extender `IOpProgramacionService`:

```csharp
// Agregar a IOpProgramacionService (interface en MatrixNext.Web/Services/OP)

public interface IOpProgramacionService
{
    // EXISTENTES ✅
    Task<(bool, List<ProgramacionCampoVm>, string)> ObtenerProgramacionesPorTrabajoAsync(...);
    Task<(bool, bool, string)> ActualizarProgramacionAsync(...);
    
    // NUEVOS para Scheduling
    Task<(bool, List<DisponibilidadEntrevistadorVm>, string)> ObtenerDisponibilidadAsync(
        long entrevistadorId, DateTime fechaInicio, DateTime fechaFin);
    
    Task<(bool, bool, string)> ActualizarHorarioAsync(
        long programacionId, DateTime nuevaFecha, TimeSpan horaInicio, TimeSpan horaFin);
    
    Task<(bool, List<ConflictoHorarioVm>, string)> VerificarConflictosAsync(
        long entrevistadorId, List<(DateTime, TimeSpan)> sesiones);
}
```

**Implementación**: OpProgramacionService.cs (Dapper) - 400+ LOC

#### 2.2 ViewModels (2h)

```csharp
// Agregar a Services/OP/Models/ProgramacionIpsVms.cs

public class DisponibilidadEntrevistadorVm
{
    public long EntrevistadorId { get; set; }
    public string Nombre { get; set; }
    public List<FranjasDisponiblesVm> Franjas { get; set; } = new();
}

public class ConflictoHorarioVm
{
    public DateTime Fecha { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public string ConflictoDesc { get; set; }
}
```

#### 2.3 Controller (4h)

```csharp
// File: MatrixNext.Web/Areas/OP/Controllers/CualitativoSchedulingController.cs

[Area("OP")]
[Authorize]
[Route("OP/Cualitativo/Scheduling")]
public class CualitativoSchedulingController : Controller
{
    private readonly IOpProgramacionService _programacionService;  // REUTILIZAR
    private readonly IOpTrabajosService _trabajosService;
    
    // Actions (8 total):
    // - Index: GET vista calendario con disponibilidades
    // - ObtenerDisponibilidad: AJAX POST retorna franjas disponibles
    // - VerificarConflictos: AJAX POST verifica solapamientos
    // - GuardarHorario: POST actualizar programación con horario
    // - ExportarCalendario: GET exportar a iCal o Excel
}
```

#### 2.4 Views (3h)

```
Index.cshtml (Calendario interactivo)
├─ Full-calendar JS (FullCalendar v6) con eventos programación
├─ Modal para editar horario + verificación en tiempo real
├─ Grid de conflictos detectados (si aplica)
└─ Botón exportar iCal

Reutilización: 
- Eventos de `IOpProgramacionService.ObtenerProgramacionesPorTrabajoAsync()` ✅
- Modales Bootstrap existentes ✅
```

#### 2.5 Testing (1h)
- ✅ E2E: Cargar calendario → Seleccionar franja → Guardar sin conflictos
- ✅ Detectar conflicto → Mostrar error → Seleccionar otra franja
- ✅ Export iCal válido

**Estimación Total Scheduling**: 14h ✅

---

### FASE 3: Sample Management (6h) - P2 Crítico

**Descripción**: Gestión de muestras/participantes en trabajos cualitativos  
**Bloquea**: Nothing (P2 pero de soporte)  
**Depende de**: TrabajosController ✅, Muestra service ✅  

#### 3.1 Auditar servicio existente (1h)

```bash
# Verificar qué EXISTE de IOpMuestraService
grep_search "IOpMuestraService\|GetMuestrasByTrabajo\|FilterMuestras"
file_search "**/*MuestraService*"
```

**Hallazgo esperado**: `IOpMuestraService` ya existe con métodos CRUD  
**Decisión**: Reutilizar 100% - solo crear Controller + Views

#### 3.2 Controller (2h)

```csharp
// File: MatrixNext.Web/Areas/OP/Controllers/CualitativoMuestraController.cs

[Area("OP")]
[Authorize]
[Route("OP/Cualitativo/Muestra")]
public class CualitativoMuestraController : Controller
{
    private readonly IOpMuestraService _muestraService;  // REUTILIZAR
    
    // Actions (6 total):
    // - Index: GET grid muestras por trabajo
    // - Create: GET form para agregar muestra
    // - Create: POST via _muestraService.CrearAsync()
    // - Edit: GET/POST
    // - Delete: POST soft-delete
    // - BuscarPorCriterio: AJAX para búsqueda en vivo
}
```

#### 3.3 Views (2h)

```
Index.cshtml (Grid DataTable 8 columnas)
├─ Documento, Nombre, Edad, Género, Estado, Acciones
├─ Create modal (formulario inline)
└─ Actions: Edit, Delete, Ver-Sesiones-Participadas

Reutilización: 
- Patrón de Index grid Sprint 5 ✅
- Modal Bootstrap existente ✅
```

#### 3.4 Testing (1h)
- ✅ CRUD básico (Create, Read, Update, Delete)
- ✅ Búsqueda por documento/nombre
- ✅ Validaciones requeridas (documento único, email válido)

**Estimación Total Sample**: 6h ✅

---

### FASE 4: Calendar/Gantt View (10h) - P2 Crítico

**Descripción**: Vista de Gantt para visualizar timeline de trabajos y sesiones  
**Bloquea**: Nothing (P2 visualización)  
**Depende de**: ProgramacionController ✅, TrabajosController ✅  

#### 4.1 Library Selection & Setup (2h)

**Opción recomendada**: FullCalendar v6 (ya usado en Scheduling)  
**Alternativa**: Frappe Gantt (más simple, 8KB, open-source)

```html
<!-- Agregar a _Layout.cshtml para Areas/OP -->
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/frappe-gantt@0.6.0/dist/frappe-gantt.css">
<script src="https://cdn.jsdelivr.net/npm/frappe-gantt@0.6.0/dist/frappe-gantt.js"></script>
```

#### 4.2 API Endpoint (3h)

**NO crear nuevo controller** - Agregar a `CualitativoProgramacionController`:

```csharp
[HttpGet("ObtenerGantt")]
public async Task<IActionResult> ObtenerGantt(long? trabajoId)
{
    // Retorna JSON en formato Frappe Gantt:
    // { id, name, start, end, progress, dependencies, custom_class }
    
    var programaciones = await _programacionService.ObtenerProgramacionesPorTrabajoAsync(trabajoId);
    
    var tasks = programaciones.Select(p => new GanttTaskVm
    {
        Id = p.ProgramacionId.ToString(),
        Name = $"{p.EntrevistadoNombre} - {p.TipoSesion}",
        Start = p.FechaProgramada?.ToString("yyyy-MM-dd"),
        End = p.FechaProgramada?.AddHours(2).ToString("yyyy-MM-dd"),
        Progress = p.EstadoId == "Ejecutado" ? 100 : 0,
        CustomClass = ObtenerColorPorEstado(p.EstadoId)
    }).ToList();
    
    return Json(tasks);
}
```

#### 4.3 Views (4h)

```
ObtenerGantt() en CualitativoProgramacionController
└─ Retorna JSON para Frappe Gantt

NUEVA vista: CualitativoCalendario/Index.cshtml
├─ Header: Filtro por trabajo, rango de fechas
├─ Canvas Gantt interactivo
├─ Legend: colores por estado (Creado, Ejecutado, Cancelado)
└─ Botón Exportar PNG (si aplica)
```

#### 4.4 Testing (1h)
- ✅ Cargar Gantt para trabajo con 5+ programaciones
- ✅ Verificar colores por estado
- ✅ Interacción: click en barra → detalle programación

**Estimación Total Calendar**: 10h ✅

---

### FASE 5: Email & Notifications (12h) - P2 Crítico

**Descripción**: Sistema de notificaciones vía email con cola Hangfire  
**Bloquea**: Nothing (P2 soporte)  
**Depende de**: `IEmailService` ✅, Hangfire setup  

#### 5.1 Hangfire Setup (3h)

**Verificar si ya existe**:
```bash
grep_search "AddHangfire|RecurringJob|BackgroundJob" Program.cs
file_search "**/*HangfireConfig*"
```

Si NO existe, agregar a Program.cs:

```csharp
// Program.cs

services.AddHangfire(config =>
    config.UseSqlServerStorage(
        "Server=.; Database=MatrixNext; Integrated Security=true;"));

services.AddHangfireServer();

// Dashboard en /hangfire
app.MapHangfireDashboard();
```

#### 5.2 Email Templates & Jobs (5h)

**NO crear nuevo servicio** - Extender `IEmailService`:

```csharp
// Agregar métodos a IEmailService (interface)

public interface IEmailService
{
    // EXISTENTE
    Task<(bool, string)> EnviarAsync(string destinatario, string asunto, string cuerpo);
    
    // NUEVOS para Notificaciones
    Task<(bool, string)> EnviarNotificacionIpsAsync(long idRevisionIps);
    
    Task<(bool, string)> EnviarRecordatorioSesionAsync(long idProgramacion);
    
    Task<(bool, string)> EnviarEntregaFichaAsync(long idFicha, string[] destinatarios);
}
```

**Implementación en EmailService.cs**:

```csharp
public async Task<(bool, string)> EnviarNotificacionIpsAsync(long idRevisionIps)
{
    // 1. Obtener detalles de revisión vía IOpIpsService
    var revision = await _ipsService.ObtenerRevisionAsync(idRevisionIps);
    
    // 2. Renderizar template (Razor o Liquid)
    var html = await _viewRenderer.RenderAsync("Emails/NotificacionIps", revision);
    
    // 3. Enviar vía background job Hangfire
    BackgroundJob.Enqueue(() => SendEmailAsync(revision.Email, "Notificación IPS", html));
    
    return (true, "Email encolado");
}
```

**Templates (Emails folder)**:
```
Emails/
├─ NotificacionIps.cshtml (Estado cambió a "Notificado")
├─ RecordatorioSesion.cshtml (24h antes de sesión)
└─ EntregaFicha.cshtml (Ficha completada y lista)
```

#### 5.3 Configuration en appsettings.json (2h)

```json
{
  "Email": {
    "SmtpServer": "smtp.office365.com",
    "SmtpPort": 587,
    "FromEmail": "noreply@empresa.com",
    "Username": "config via secrets",
    "Password": "config via secrets",
    "EnableTls": true
  },
  "Hangfire": {
    "ConnectionString": "Server=.; Database=MatrixNext; ...",
    "DashboardPath": "/hangfire",
    "WorkerCount": 5
  }
}
```

#### 5.4 Triggers (Integration) (2h)

Agregar llamadas en controladores **EXISTENTES** (no crear nuevos):

```csharp
// En CualitativoFichasController.SubmitInterview()
[HttpPost]
public async Task<IActionResult> SubmitInterview(FichaTecnicaVm model)
{
    var (success, ficha, error) = await _fichasService.GuardarFichaAsync(model);
    
    if (success)
    {
        // NUEVO: Notificar via email
        await _emailService.EnviarEntregaFichaAsync(ficha.Id, new[] { User.GetEmail() });
        
        return RedirectToAction("Index");
    }
    
    return View(model);
}

// En IpsController cuando se aprueba
[HttpPost]
public async Task<IActionResult> AprobarRevision(long id)
{
    // ... lógica de aprobación
    
    // NUEVO: Notificar cambio de estado
    await _emailService.EnviarNotificacionIpsAsync(id);
    
    return Json(new { success = true });
}
```

#### 5.5 Testing (1h)
- ✅ Email encolado en Hangfire
- ✅ Verificar en dashboard `/hangfire`
- ✅ Template renderiza correctamente
- ✅ Entrega exitosa (verificar en logs)

**Estimación Total Email**: 12h ✅

---

### FASE 6: Bulk Import Excel (8h) - P2 Complementario

**Descripción**: Herramienta para importar muestras/participantes en lote desde Excel  
**Bloquea**: Nothing (P2 complementario)  
**Depende de**: MuestraService ✅, `ClosedXML` ✅  

#### 6.1 Service Extension (3h)

**NO crear nuevo service** - Agregar método a `IOpMuestraService`:

```csharp
public interface IOpMuestraService
{
    // EXISTENTE
    Task<(bool, List<MuestraVm>, string)> ObtenerMuestrasByTrabajoAsync(...);
    
    // NUEVO
    Task<(bool, List<ImportResultadoVm>, string)> ImportarDesdExcelAsync(
        long trabajoId, Stream archivo, CancellationToken ct = default);
}
```

**Implementación**: OpMuestraService.cs

#### 6.2 Controller Endpoint (2h)

**Agregar a `CualitativoMuestraController`**:

```csharp
[HttpPost("ImportarExcel")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ImportarExcel(long trabajoId, IFormFile archivo)
{
    if (archivo == null || archivo.Length == 0)
        return BadRequest("Archivo requerido");
    
    using var stream = archivo.OpenReadStream();
    var (success, resultados, error) = await _muestraService.ImportarDesdExcelAsync(trabajoId, stream);
    
    if (!success)
        return BadRequest(error);
    
    // Retornar resumen: 50 importadas, 3 duplicadas, 2 errores
    return Json(new { success = true, resultados });
}
```

#### 6.3 Views (2h)

```
NUEVA vista: CualitativoMuestra/ImportarExcel.cshtml
├─ Formulario drag-and-drop para archivo Excel
├─ Template download (botón descargar plantilla)
├─ Validaciones cliente (file extension, size < 5MB)
└─ Tabla con resultados importación (exitosas/duplicadas/errores)
```

**Template Excel descargable**:
```
Columns: Documento | Nombre | Edad | Género | Telefono | Email
Rows: 100 vacías (para llenar)
Validaciones integradas en Sheet (dropdown de Género, etc.)
```

#### 6.4 Testing (1h)
- ✅ Importar archivo válido (50 muestras)
- ✅ Detectar duplicados (documento ya existe)
- ✅ Validaciones: email formato, edad rango, género enum
- ✅ Rollback si hay error crítico

**Estimación Total Bulk Import**: 8h ✅

---

## 📈 RESUMEN ESTIMACIONES SPRINT 6

| Fase | Componente | Servicios Nuevos | Controllers Nuevos | Vistas Nuevas | Horas | Estado |
|------|-----------|-----------------|-------------------|---------------|-------|--------|
| 1 | Transcription | 0 (REUTILIZAR) | 1 | 4 | 8h | Planificado |
| 2 | Scheduling | 0 (EXTENDER) | 1 (NEW) | 1 (NEW) | 14h | Planificado |
| 3 | Sample | 0 (REUTILIZAR) | 1 | 3 | 6h | Planificado |
| 4 | Calendar | 0 (ADD ENDPOINT) | - | 1 (NEW) | 10h | Planificado |
| 5 | Email/Hangfire | 0 (EXTENDER) | - | 3 templates | 12h | Planificado |
| 6 | Bulk Import | 0 (EXTENDER) | - | 1 (NEW) | 8h | Planificado |
| | **TOTALES** | **0 NEW** | **3 NEW** | **13 NEW** | **62h** | ✅ LISTO |

---

## 🛠️ CHECKLIST PRE-IMPLEMENTACIÓN

Ejecutar ANTES de cada fase:

### Transcription (Antes de iniciar)
- [ ] `grep_search "TranscriptionController"` - verificar no existe
- [ ] `grep_search "TipoFicha.*4"` en BD - confirmar tipo = 4 disponible
- [ ] Revisar `EditInterview.cshtml` para agregar condicional TipoFicha=4
- [ ] Auditar `IOpFichasTecnicasService.GuardarFichaAsync()` - soporta tipo 4?

### Scheduling (Antes de iniciar)
- [ ] `file_search "CualitativoProgramacionController"` - verificar ubicación
- [ ] `grep_search "ObtenerProgramacionesPorTrabajoAsync"` - confirmar método existe
- [ ] Auditar DB: tabla OP_Programacion - campos horario (HoraInicio, HoraFin)
- [ ] Revisar `IWorkFlowService` para estados disponibles

### Sample (Antes de iniciar)
- [ ] `grep_search "IOpMuestraService"` - verificar métodos disponibles
- [ ] `file_search "**/*MuestraService*"` - ubicar implementación
- [ ] Auditar BD: tabla de muestras (estructura campos)
- [ ] Revisar permisos: ¿Quién puede crear/editar muestras?

### Calendar (Antes de iniciar)
- [ ] Decidir: FullCalendar vs Frappe Gantt vs Chart.js
- [ ] Verificar si `_Layout.cshtml` (OP) ya carga librerías necesarias
- [ ] Auditar datos: ¿Qué campos usar para start/end de actividades?

### Email/Hangfire (Antes de iniciar)
- [ ] `grep_search "Hangfire"` en Program.cs - ¿Ya configurado?
- [ ] `grep_search "IEmailService"` - ubicar interfaz
- [ ] Auditar templates existentes (Emails folder)
- [ ] Verificar credenciales SMTP en appsettings (dev, staging)

### Bulk Import (Antes de iniciar)
- [ ] `grep_search "ClosedXML\|ExcelPackage"` - confirmar paquete disponible
- [ ] Auditar validaciones en `MuestraVm` - ¿Cuáles aplicar en importación?
- [ ] Revisar `IOpMuestraService` - ¿Soporta batch inserts?

---

## 📋 DIRECTRICES CLAVE PARA SPRINT 6

### 1. **MÁXIMA REUTILIZACIÓN** 
✅ No crear nuevo Service si existe método compatible  
✅ Extender interfaz en lugar de crear nueva  
✅ Reutilizar ViewModels (agregar propiedades si necesario)  

### 2. **VERIFICACIÓN PREVIA**
✅ Antes de cualquier implementación, ejecutar búsquedas para confirmar NO existe  
✅ Documentar "Ya existe" en plan (si descubres durante implementación)  

### 3. **PATRÓN CONSISTENTE**
✅ Todos los controllers siguen patrón Sprint 5:  
   - `[Area("OP")]`, `[Authorize]`, `[Route("OP/Cualitativo/[action]")]`
   - IActionResult con model binding
   - Validaciones vía attributes + service
   - JSON para AJAX, View para páginas

✅ Todas las vistas siguen patrón Sprint 5:
   - Bootstrap 5 + DataTables (si es grid)
   - Modal para confirmar acciones destructivas
   - CSRF token en todos los forms
   - Error/Success alerts en TempData

### 4. **BUILD & TESTING**
✅ Ejecutar `dotnet build` después de CADA FASE  
✅ Verificar 0 new errors (pre-existing warnings OK)  
✅ E2E testing: al menos 1 flujo completo por feature  

### 5. **DOCUMENTATION**
✅ Agregar XML comments en métodos públicos  
✅ Actualizar este plan si descubres cambios  
✅ Commit después de CADA FASE completada  

---

## 🎯 PRÓXIMOS PASOS

1. **HOY**: Ejecutar auditoría de servicios (verificar qué existe)
2. **Mañana**: Iniciar Fase 1 (Transcription) - 8h
3. **Día 3**: Fase 2 (Scheduling) - 14h
4. **Día 4-5**: Fases 3-4 (Sample + Calendar) - 16h
5. **Día 6**: Fase 5 (Email) - 12h
6. **Día 7**: Fase 6 (Bulk Import) + testing - 8h
7. **Cierre**: Commit + SPRINT_6_CIERRE_DOCUMENTACION.md

**Timeline total**: ~7 días calendario (62h desarrollo)  
**Resultado esperado**: MVP + complementos 100% producción-ready ✅

---

## 📌 NOTAS FINALES

- **Sprint 5 completó TODOS los flujos críticos** (Trabajos → Filtros → Aprobación → Fichas)
- **Sprint 6 agrega funcionalidades que enriquecen pero NO son bloqueadores**
- **Meta: CERO servicios nuevos, máxima extensión de lo existente**
- **Ganancia de eficiencia**: Arquitectura Sprint 0 + reutilización = 79% ahorro estimado

¡Listo para empezar Sprint 6! 🚀
