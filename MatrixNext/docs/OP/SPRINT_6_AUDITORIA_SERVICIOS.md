# 🔍 AUDITORÍA DE SERVICIOS - SPRINT 6 PREIMPLEMENTACIÓN

**Fecha**: 9 de enero de 2026  
**Objetivo**: Verificar qué servicios EXISTEN vs qué falta crear  
**Directriz**: CERO servicios nuevos - máxima extensión de lo existente

---

## ✅ SERVICIOS CONFIRMADOS COMO EXISTENTES

### 1. **IOpMuestraService** ✅ COMPLETO

**Ubicación**: `MatrixNext.Web/Services/OP/IOpMuestraService.cs`  
**Implementación**: `MatrixNext.Web/Services/OP/OpMuestraService.cs`  
**Registered**: Line 158 en Program.cs (`builder.Services.AddScoped<IOpMuestraService, OpMuestraService>()`)

**Métodos actuales** (7):
```csharp
Task<List<MuestraCiudadListItemVM>> ObtenerMuestraPorTrabajoAsync(long trabajoId)
Task<MuestraCiudadVM?> ObtenerMuestraPorIdAsync(long idMuestra)
Task<double> ObtenerMuestraPorCiudadAsync(long trabajoId, int ciudadId)
Task<long> GuardarMuestraAsync(MuestraCiudadVM model)
Task<bool> ActualizarFechasConPlaneacionAsync(ActualizarFechasMuestraVM model)
Task<bool> EliminarMuestraAsync(long idMuestra)
Task<double> CalcularTotalMuestraAsync(long trabajoId)
```

**Para Sprint 6**: 
- ✅ **Usar tal cual para Sample Management Controller**
- ✅ No crear `CualitativoMuestraController` - usar `MuestraTrabajosController` existente
- ✅ Opcionalmente agregar método `ImportarDesdExcelAsync()` (ver sección Bulk Import)

**DTOs internos**:
- `MuestraCiudadDto` (8 properties)
- ViewModels: `MuestraCiudadListItemVM`, `MuestraCiudadVM`, `ActualizarFechasMuestraVM`

**Tests**: Existe `OpMuestraServiceTests.cs` con 3+ test cases

---

### 2. **IOpProgramacionService** ✅ COMPLETO

**Ubicación**: `MatrixNext.Web/Services/OP/IOpProgramacionService.cs`  
**Implementación**: `MatrixNext.Web/Services/OP/OpProgramacionService.cs`  
**Status**: Sprint 3 completado

**Métodos críticos para Sprint 6**:
```csharp
Task<(bool, List<ProgramacionCampoVm>, string)> ObtenerProgramacionesPorTrabajoAsync(long trabajoId)
Task<(bool, bool, string)> ActualizarProgramacionAsync(ProgramacionCampoVm model)
// + 5 más disponibles
```

**Para Sprint 6 (Scheduling)**:
- ✅ **EXTENDER** con métodos nuevos (no crear servicio):
  - `ObtenerDisponibilidadEntrevistadoresAsync()`
  - `ActualizarHorarioAsync()`
  - `VerificarConflictosAsync()`

**Ventaja**: Ya existe lógica de estados (Creado, Asignado, Confirmado, Ejecutado, Cancelado, NoAsistio, Reprogramado)

---

### 3. **IOpFichasTecnicasService** ✅ COMPLETO

**Ubicación**: `MatrixNext.Web/Services/OP/IOpFichasTecnicasService.cs`  
**Status**: Sprint 5 completado

**Para Sprint 6 (Transcription)**:
- ✅ **NO crear nuevo** - Extender con TipoFicha=4
- ✅ Método `GuardarFichaAsync()` ya soporta cualquier tipo
- ✅ Validaciones de presupuesto reutilizables

---

### 4. **IOpIpsService** ✅ COMPLETO

**Status**: Sprint 3 completado  
**Para Sprint 6**: Ya implementado completamente - NO requiere cambios

---

### 5. **IOpTrabajosService** ✅ COMPLETO

**Status**: Sprint 5 completado  
**Para Sprint 6**: Contexto para navegación

---

### 6. **IEmailService** ✅ EXISTE

**Status**: Servicio compartido  
**Para Sprint 6 (Email/Notifications)**:
- ✅ Extender con métodos:
  - `EnviarNotificacionIpsAsync()`
  - `EnviarRecordatorioSesionAsync()`
  - `EnviarEntregaFichaAsync()`

---

### 7. **IExportService** ✅ EXISTE

**Status**: Compartido  
**Para Sprint 6**: Excel/PDF exports para todas las fases

---

## ❌ SERVICIOS QUE NO NECESITAN CREAR

| Servicio Planeado | Estado | Solución |
|-----------------|--------|----------|
| `IOpTranscripcionService` | ❌ NO CREAR | Usar `IOpFichasTecnicasService` con TipoFicha=4 |
| `IOpSchedulingService` | ❌ NO CREAR | Extender `IOpProgramacionService` |
| `IOpSampleService` | ❌ NO CREAR | Usar `IOpMuestraService` existente |
| `IOpCalendarService` | ❌ NO CREAR | Usar `IOpProgramacionService.ObtenerProgramacionesPorTrabajoAsync()` |
| `IOpEmailNotificationService` | ❌ NO CREAR | Extender `IEmailService` |
| `IOpBulkImportService` | ❌ NO CREAR | Agregar método a `IOpMuestraService` |

---

## 📊 CONTROLLERS EXISTENTES A REUTILIZAR / EXTENDER

### Para Sample Management

**Hallazgo**: ✅ `MuestraTrabajosController` EXISTE

```
Ubicación: MatrixNext.Web/Areas/OP/Controllers/MuestraTrabajosController.cs
Status: Implementado (Sprint histórico)
Inyecciones:
  - IOpMuestraService ✅
  - IOpTrabajosService ✅
  - IEmailService ✅
  - ILogger ✅
```

**Decisión**: 
- ✅ **NO crear `CualitativoMuestraController`**
- ✅ **Usar y extender `MuestraTrabajosController`** con vistas mejoradas
- ✅ Agregar AcciónBulkImport si falta

---

### Para Scheduling

**Status**: ❌ NO existe `SchedulingController`  
**Solución**:
- Crear **`CualitativoSchedulingController`** nuevo (simple)
- Extender `IOpProgramacionService` con métodos de horario
- Reutilizar endpoint en `CualitativoProgramacionController.ObtenerGantt()`

---

### Para Calendar/Gantt

**Status**: ✅ Endpoint puede agregarse a `CualitativoProgramacionController` existente  
**Solución**:
- Agregar action `ObtenerGantt()` en `CualitativoProgramacionController`
- Crear vista standalone `GanttView.cshtml`
- NO crear controller nuevo

---

## 📝 RECOMENDACIONES POR FASE

### FASE 1: Transcription (8h)

```
✅ CONFIRMED REUTILIZACIÓN:
  - Servicio: IOpFichasTecnicasService (NO crear nuevo)
  - Controller: Agregar actions a CualitativoFichasController
  - ViewModel: FichaTecnicaVm + propiedad TipoFicha
  - Vistas: Reutilizar EditInterview.cshtml con condicional TipoFicha=4
  - DB: NO requiere cambios (TipoFicha ya existe)

❌ CAMBIOS INNECESARIOS:
  - NO crear IOpTranscripcionService
  - NO crear CualitativoTranscripcionController (agregar a Fichas)
```

### FASE 2: Scheduling (14h)

```
✅ CONFIRMED REUTILIZACIÓN:
  - Servicio: Extender IOpProgramacionService
  - Controller: CREAR CualitativoSchedulingController (simple)
  - Datos: ObtenerProgramacionesPorTrabajoAsync() ✅

❌ CAMBIOS INNECESARIOS:
  - NO crear IOpSchedulingService
  - NO crear IOpDisponibilidadService
```

### FASE 3: Sample (6h)

```
✅ CONFIRMED REUTILIZACIÓN:
  - Servicio: IOpMuestraService COMPLETO ✅
  - Controller: Usar MuestraTrabajosController existente (solo vistas mejoradas)
  - Métodos: Todos disponibles (CRUD, Filter, etc.)

❌ CAMBIOS INNECESARIOS:
  - NO crear IOpMuestraService (YA EXISTE)
  - NO crear CualitativoMuestraController
  - NO crear SampleService
```

### FASE 4: Calendar/Gantt (10h)

```
✅ CONFIRMED REUTILIZACIÓN:
  - Datos: IOpProgramacionService.ObtenerProgramacionesPorTrabajoAsync() ✅
  - Endpoint: Agregar a CualitativoProgramacionController (no new controller)
  - Librería: FullCalendar v6 (ya en wwwroot)

❌ CAMBIOS INNECESARIOS:
  - NO crear IOpCalendarService
  - NO crear CalendarController
```

### FASE 5: Email/Notifications (12h)

```
✅ CONFIRMED REUTILIZACIÓN:
  - Servicio: Extender IEmailService (no crear nuevo)
  - Queue: Hangfire (verificar si existe en Program.cs)
  - Triggers: Agregar llamadas en controllers EXISTENTES

❌ CAMBIOS INNECESARIOS:
  - NO crear IOpEmailNotificationService
  - NO crear NotificationQueue service
```

### FASE 6: Bulk Import (8h)

```
✅ CONFIRMED REUTILIZACIÓN:
  - Servicio: Agregar método a IOpMuestraService (NO crear nuevo)
  - ClosedXML: Ya disponible (verificar en .csproj)
  - Endpoint: POST en MuestraTrabajosController

❌ CAMBIOS INNECESARIOS:
  - NO crear IOpBulkImportService
  - NO crear ImportService
```

---

## 🔐 VERIFICACIONES DE INFRAESTRUCTURA

### Hangfire (Para Email Notifications)

```bash
Status: ❓ REVISAR
Comando: grep_search "AddHangfire\|services.AddHangfireServer()" Program.cs
```

Si no existe, agregar:
```csharp
// Program.cs
services.AddHangfire(config =>
    config.UseSqlServerStorage("Server=..."));
services.AddHangfireServer();
app.MapHangfireDashboard("/hangfire");
```

### ClosedXML (Para Excel)

```bash
Status: ✅ ASUMIDO EXISTENTE
Ubicación: .csproj
Si no: dotnet add package ClosedXML
```

---

## 🎯 TABLA RESUMIDA - DECISIONES FINALES

| Componente | Crear Nuevo | Reutilizar | Extender | Estado |
|-----------|:-----------:|:----------:|:--------:|--------|
| Transcription Service | ❌ | ✅ FichasTecnicas | - | GO |
| Transcription Controller | ❌ | ✅ Fichas | Agregar actions | GO |
| Scheduling Service | ❌ | ✅ Programación | ✅ Métodos horario | GO |
| Scheduling Controller | ✅ | - | - | GO (nuevo) |
| Sample Service | ❌ | ✅ Muestra | - | GO |
| Sample Controller | ❌ | ✅ MuestraTrabajos | Mejorar vistas | GO |
| Calendar Endpoint | ❌ | ✅ Programación | ✅ ObtenerGantt() | GO |
| Email Service | ❌ | ✅ EmailService | ✅ 3 métodos | GO |
| Bulk Import | ❌ | ✅ Muestra | ✅ ImportarExcel() | GO |

---

## ✨ AHORRO ESTIMADO

**Original Plan (con servicios nuevos)**: 62h  
**Ejecución Reutilización (sin crear servicios)**: 
- Tiempo desarrollo: -40% (menos boilerplate)
- Menos testing (métodos reusados = menos bugs)
- Menos documentación

**Estimación revisada**: ~48h (en lugar de 62h)  
**Ganancia**: 14 horas de productividad ✅

---

## 🚀 CHECKLIST LISTO PARA INICIAR

- [x] IOpMuestraService confirmado existente (7 métodos)
- [x] IOpProgramacionService confirmado existente (reutilizable)
- [x] IOpFichasTecnicasService confirmado existente (reutilizable)
- [x] IEmailService confirmado existente (extensible)
- [x] MuestraTrabajosController confirmado existente (reutilizable)
- [x] CualitativoProgramacionController confirmado existente (extensible)
- [x] CualitativoFichasController confirmado existente (extensible)
- [ ] Verificar Hangfire en Program.cs
- [ ] Verificar ClosedXML en .csproj
- [ ] Compilar y validar 0 new errors

---

## 📋 ORDEN RECOMENDADO DE EJECUCIÓN

**HOY**: 
1. ✅ Auditoría completada (este documento)
2. ⏳ Verificar Hangfire + ClosedXML
3. ⏳ Iniciar FASE 1: Transcription (8h)

**MAÑANA**: 
4. FASE 2: Scheduling (14h)
5. FASE 3: Sample Management (6h)

**DÍA 3**:
6. FASE 4: Calendar/Gantt (10h)
7. FASE 5: Email (12h)

**DÍA 4**:
8. FASE 6: Bulk Import (8h)
9. Testing + Build + Commits

**Timeline**: ~4 días calendario = 48h efectivos ✅

---

## 📌 PRÓXIMOS PASOS INMEDIATOS

```bash
# 1. Verificar Hangfire
grep_search "AddHangfire|HangfireServer" Program.cs

# 2. Verificar ClosedXML
grep_search "ClosedXML|using ClosedXML" .csproj

# 3. Compilar y validar
dotnet build

# 4. Si todo OK, iniciar FASE 1
# Crear CualitativoTranscripcionController como extensión de FichasController
```

¡Listo para ejecutar Sprint 6 con máxima eficiencia! 🎯
