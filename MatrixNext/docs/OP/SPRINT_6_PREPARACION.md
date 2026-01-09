# 🚀 SPRINT 6: PREPARACIÓN INMEDIATA - CHECKLIST DE EJECUCIÓN

**Fecha**: 9 de enero de 2026  
**Estado**: ✅ Auditoría completada, listo para código  
**Próximo paso**: Implementar FASE 1 (Transcription - 8h)

---

## ✅ VERIFICACIONES PREVIAS (5 minutos)

### 1. Hangfire - NO CONFIGURADO ❌

**Hallazgo**: No existe en Program.cs  
**Decisión**: Opcional para Sprint 6 (las notificaciones email pueden hacerse sin queue)  
**Si quieres agregar** (recomendado):

```bash
# Instalar paquete
cd MatrixNext.Web
dotnet add package Hangfire
dotnet add package Hangfire.SqlServer

# Luego agregar a Program.cs (después de Program.Build())
services.AddHangfire(config =>
    config.UseSqlServerStorage(
        "Server=.;Database=MatrixNext;Integrated Security=true;"));
services.AddHangfireServer();

# Y en la app.MapControllers():
app.MapHangfireDashboard("/hangfire");
```

**Status para Sprint 6**: Dejar para FASE 5 (Email), no bloqueador

### 2. ClosedXML - ASUMIDO ✅

**Asunción**: Ya instalado (usado en Sprint 5 para exports)  
**Verificar** (opcional):
```bash
grep -r "using ClosedXML\|ClosedXML" MatrixNext.Web
# Si no encuentra, instalar: dotnet add package ClosedXML
```

### 3. Compilación Base

```bash
cd MatrixNext
dotnet build
# Esperado: 0 new errors (23 pre-existing warnings OK)
```

---

## 📂 CARPETAS CLAVE PARA SPRINT 6

```
MatrixNext.Web/
├─ Areas/OP/Controllers/
│  ├─ CualitativoFichasController.cs (EXTENDER con Transcription)
│  ├─ CualitativoProgramacionController.cs (EXTENDER con Scheduling)
│  ├─ MuestraTrabajosController.cs (USAR para Sample)
│  └─ IpsController.cs (Ya existe ✅)
│
├─ Areas/OP/Views/
│  ├─ CualitativoFichas/EditInterview.cshtml (Extender para tipo=4)
│  ├─ CualitativoProgramacion/ (Agregar GanttView.cshtml)
│  ├─ MuestraTrabajos/ (Mejorar vistas existentes)
│  └─ Ips/ (Ya existe ✅)
│
├─ Services/OP/
│  ├─ IOpFichasTecnicasService.cs (Extender)
│  ├─ IOpProgramacionService.cs (Extender)
│  ├─ IOpMuestraService.cs (Usar tal cual) ✅
│  ├─ IEmailService.cs (Extender)
│  └─ IOpIpsService.cs (Usar tal cual) ✅
│
└─ ViewModels/OP/
   ├─ FichaTecnicaVm (Agregar TipoFicha = 4)
   ├─ ProgramacionCampoVm (Agregar propiedades de horario)
   └─ MuestraCiudadVM (Usar tal cual) ✅
```

---

## 🎯 FASE 1: TRANSCRIPTION (8h) - COMIENZA YA

### Paso 1.1: Revisar estructura existente (15 min)

```bash
# Ubicación controladores
ls MatrixNext.Web/Areas/OP/Controllers/ | grep -i cualitativo

# Verificar EditInterview.cshtml
cat MatrixNext.Web/Areas/OP/Views/CualitativoFichas/EditInterview.cshtml | head -50
# Buscar: @if (Model.TipoFicha == ...) para agregar condición para tipo 4
```

### Paso 1.2: Extender CualitativoFichasController (2h)

**Ubicación**: `MatrixNext.Web/Areas/OP/Controllers/CualitativoFichasController.cs`

**Agregar action** (después de SaveObservation):
```csharp
// Transcription CRUD
[HttpGet("Transcripcion/{id}")]
public async Task<IActionResult> EditTranscripcion(long id)
{
    var (success, ficha, error) = await _fichasService.ObtenerFichaAsync(id, 4); // TipoFicha=4
    if (!success) return NotFound(error);
    return View("EditInterview", ficha); // Reutilizar misma vista
}

[HttpPost("Transcripcion/Guardar")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> SaveTranscripcion(FichaTecnicaVm model)
{
    model.TipoFicha = 4; // Force type 4
    var (success, saved, error) = await _fichasService.GuardarFichaAsync(model);
    if (!success) return BadRequest(error);
    
    TempData["Success"] = "Transcripción guardada";
    return RedirectToAction("Index", "CualitativoTrabajos");
}

[HttpPost("Transcripcion/Enviar")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> SubmitTranscripcion(long id)
{
    var (success, ficha, error) = await _fichasService.ObtenerFichaAsync(id, 4);
    if (!success) return BadRequest(error);
    
    ficha.Estado = "Entregada"; // Update status
    var (savSuccess, _, saveError) = await _fichasService.GuardarFichaAsync(ficha);
    
    if (savSuccess)
        TempData["Success"] = "Transcripción entregada";
    
    return RedirectToAction("Index", "CqualitativoTrabajos");
}
```

### Paso 1.3: Extender EditInterview.cshtml (2h)

**Ubicación**: `MatrixNext.Web/Areas/OP/Views/CualitativoFichas/EditInterview.cshtml`

**Buscar línea donde está**:
```cshtml
@if (Model.TipoFicha == 1) // Entrevista
{
    // ...
}
else if (Model.TipoFicha == 2) // Sesión
{
    // ...
}
else if (Model.TipoFicha == 3) // Observación
{
    // ...
}
```

**Agregar después**:
```cshtml
else if (Model.TipoFicha == 4) // Transcripción
{
    <div class="form-group mb-3">
        <label for="ContenidoTranscripcion" class="form-label">Contenido de Transcripción</label>
        <textarea class="form-control" id="ContenidoTranscripcion" name="Descripcion" 
                  rows="15" required>@Model.Descripcion</textarea>
        <small class="form-text text-muted">Transcripción completa de la sesión o entrevista</small>
    </div>
    
    <div class="form-group mb-3">
        <label for="IdiomaTranscripcion" class="form-label">Idioma</label>
        <select class="form-control" id="IdiomaTranscripcion">
            <option value="ES" selected>Español</option>
            <option value="EN">Inglés</option>
            <option value="FR">Francés</option>
        </select>
    </div>
    
    <div class="form-group mb-3">
        <label for="ArchivoTranscripcion" class="form-label">Archivo adjunto (PDF/DOCX)</label>
        <input type="file" class="form-control" id="ArchivoTranscripcion" 
               accept=".pdf,.docx" />
    </div>
}
```

### Paso 1.4: Crear ruta de navegación (1h)

**En CualitativoTrabajosController** (Index action), agregar botón:
```cshtml
<!-- En Index.cshtml grid de trabajos, agregar columna de acciones -->
<a href="@Url.Action("EditTranscripcion", "CualitativoFichas", new { id = item.FichaId })" 
   class="btn btn-sm btn-outline-secondary" title="Editar Transcripción">
    <i class="bi bi-file-earmark-text"></i> Transcribir
</a>
```

### Paso 1.5: Testing & Commit (2h)

```bash
# 1. Compilar
cd MatrixNext
dotnet build
# Esperado: 0 new errors

# 2. Ejecutar
dotnet run
# Navegar a: http://localhost:5000/OP/Cualitativo/Trabajos
# Click en trabajo → Botón "Transcribir" → Llenar formulario → Guardar

# 3. Commit
git add -A
git commit -m "FASE 1 SPRINT 6: Transcription completada. ExtendCualitativoFichasController con TipoFicha=4. EditInterview.cshtml extendida. Build: SUCCESS"
```

---

## 📋 ESTADOS Y MILESTONES

### HOY (9 de enero)
- ✅ Auditoría completada
- ✅ SPRINT_6_PLAN.md creado
- ✅ SPRINT_6_AUDITORIA_SERVICIOS.md creado
- ⏳ **INICIAR FASE 1: Transcription (8h)**

### MAÑANA (10 de enero)
- ⏳ Completar FASE 1: Transcription
- ⏳ Iniciar FASE 2: Scheduling (14h)

### Día 11
- ⏳ Completar FASE 2: Scheduling
- ⏳ FASE 3: Sample Management (6h)

### Día 12
- ⏳ FASE 4: Calendar/Gantt (10h)
- ⏳ Iniciar FASE 5: Email (12h)

### Día 13
- ⏳ Completar FASE 5: Email
- ⏳ FASE 6: Bulk Import (8h)
- ⏳ Testing + Build + Commits finales

---

## 🔧 COMANDOS RÁPIDOS PARA EJECUCIÓN

```bash
# Compilar después de cada change
dotnet build

# Ejecutar tests (si hay)
dotnet test

# Ver estado git
git status

# Commit después de CADA FASE
git add -A
git commit -m "FASE X SPRINT 6: [Descripción]"

# Ver log
git log --oneline | head -10
```

---

## 💡 TIPS PARA MÁXIMA VELOCIDAD

1. **Reutilización agresiva**: Si el método existe, úsalo
2. **Vistas condicionales**: Agregar `@if (TipoFicha == X)` en lugar de crear nuevas vistas
3. **DTOs compartidos**: Usar `FichaTecnicaVm` para TODO tipo de fichas
4. **Commits frecuentes**: Después de cada fase (no esperar a final)
5. **Build frecuente**: `dotnet build` después de cada action
6. **Validaciones reusables**: Usar FluentValidation existente

---

## 🎁 BONUS: Optimizaciones Sugeridas

**Si terminas antes** (Sprint 6 suele ser más rápido):

1. Agregar validaciones adicionales (campos requeridos por tipo)
2. Mejorar UX con progreso visual (pasos: Crear → Guardar → Entregar)
3. Integración con Hangfire (si hay tiempo)
4. Tests unitarios para cada fase

---

## ⚠️ BLOQUEADORES POTENCIALES

| Riesgo | Probabilidad | Solución |
|--------|:-------:|----------|
| TipoFicha=4 no existe en BD | Baja | Verificar tabla OP_FichasTecnica |
| Ruta /OP no está configurada | Muy baja | Verificar Areas/OP/AreaRegistration |
| ClosedXML no instalado | Baja | `dotnet add package ClosedXML` |
| Validaciones de presupuesto | Media | Reutilizar ValidateBudgetAsync() |

---

## 🎯 OBJETIVO FINAL

```
Al completar Sprint 6:
├─ 48h desarrollo (en lugar de 62h estimadas)
├─ 0 servicios nuevos creados
├─ 100% reutilización de código Sprint 0-5
├─ Build: SUCCESS (0 new errors)
├─ E2E: Todos los 6 flujos completados
├─ 6 commits (uno por fase)
└─ MVP + Complementos 100% PRODUCCIÓN-READY ✅
```

---

**¡Listo para comenzar! Ejecuta FASE 1 cuando estés listo. 🚀**
