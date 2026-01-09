# 📊 Sprint 1 - Progress Tracking

## Objetivo Sprint 1
Completar P1 Controllers (Programación y IPS) con UX-first approach, alinear servicios a SPs CoreProject existentes.

---

## 📈 Visual Progress

### ✅ Sprint 1 - Phase 1: Backend Services & Controllers

#### OP-P01: Programación de Campo
**Backend**: [████████████████████] 100% (12h)
- ✅ IOpProgramacionService.cs (5 métodos)
- ✅ OpProgramacionService.cs (~280 LOC, Dapper queries)
- ✅ CualitativoProgramacionController.cs (7 actions)
- ✅ ViewModels (ProgramacionCampoVm + 20 properties)
- ✅ DI registration en Program.cs

**Frontend**: [████████████████████] 100% (2h)
- ✅ Views/CualitativoProgramacion/Index.cshtml (filtros + grid + AJAX estado)
- ✅ Views/CualitativoProgramacion/Edit.cshtml (form + entrevistados disponibles)

**Estado**: ✅ **COMPLETADO**  
**Estimación**: 14h | **Actual**: 14h  
**Build**: ✅ Compilación exitosa (13 warnings nullability - no bloqueantes)

---

#### OP-I01: Gestión de Procesos IPS
**Backend**: [████████████████████] 100% (8h)
- ✅ IOpIpsService (pre-existente con SPs OP_IPS_Revision_Get/Edit)
- ✅ CualitativoIpsController.cs (5 actions: Index, Save, Notify, Reject, ExportExcel)
- ✅ ViewModels integration (IpsRevisionVm, IpsRevisionUpdateModel)
- ✅ Fix: Rechazar property type (string, not bool)

**Frontend**: [████████████████████] 100% (2h)
- ✅ Views/CualitativoIps/Index.cshtml (grid + modal rechazo + AJAX aprobar/rechazar)
- ✅ Bootstrap modal para rechazo con validación de observaciones requeridas

**Estado**: ✅ **COMPLETADO**  
**Estimación**: 10h | **Actual**: 10h  
**Build**: ✅ Compilación exitosa (13 warnings nullability - no bloqueantes)

---

## 📋 Detalles Técnicos

### Servicios Creados

#### OpProgramacionService
- **Archivo**: `MatrixNext.Web/Services/OP/OpProgramacionService.cs`
- **Líneas**: ~280 LOC
- **Métodos**:
  - `ObtenerProgramacionesPorTrabajoAsync`: Query con LEFT JOINs (OP_Programados_Entrevistados, PY_Trabajo, OP_EstadosProgramacion, US_Usuarios)
  - `GuardarProgramacionAsync`: INSERT/UPDATE con validación FechaProgramada/EntrevistadoId requeridos
  - `CambiarEstadoProgramacionAsync`: UPDATE con validación estadoId [1-7]
  - `ExportarProgramacionesExcelAsync`: Integration con IExportService
  - `ObtenerEntrevistadosDisponiblesAsync`: Query OP_MuestraTrabajos para dropdown
- **Tecnología**: Dapper (queries directas SQL), IDbConnection
- **Estados**: 1=Creado, 2=Asignado, 3=Confirmado, 4=Ejecutado, 5=Cancelado, 6=NoAsistio, 7=Reprogramado

### Controllers Creados

#### CualitativoProgramacionController
- **Archivo**: `MatrixNext.Web/Areas/OP/Controllers/CualitativoProgramacionController.cs`
- **Actions**:
  - `Index(trabajoId, fechaDesde, fechaHasta, estadoId)`: List con filtros
  - `Edit(id, trabajoId)`: Load form (create/edit)
  - `Save(model)`: POST con [ValidateAntiForgeryToken]
  - `ChangeStatus(programacionId, nuevoEstadoId)`: AJAX JSON
  - `ExportExcel(trabajoId)`: Descarga Excel
  - `GetEntrevistadosDisponibles(trabajoId)`: AJAX JSON
- **Claims**: `ClaimTypes.NameIdentifier` para usuarioId

#### CualitativoIpsController
- **Archivo**: `MatrixNext.Web/Areas/OP/Controllers/CualitativoIpsController.cs`
- **Actions**:
  - `Index(trabajoId, fechaDesde, fechaHasta)`: List con filtros
  - `Save(model)`: POST con [ValidateAntiForgeryToken]
  - `Notify(idProceso)`: Aprobar revisión (Rechazar = string.Empty, Estado = "Aprobado")
  - `Reject(idProceso, observaciones)`: Rechazar con observaciones requeridas (Rechazar = "S", Estado = "Rechazado")
  - `ExportExcel(trabajoId)`: Descarga Excel
- **Integration**: Usa `IOpIpsService` pre-existente sin duplicar lógica

### ViewModels

#### ProgramacionIpsVms.cs
- `ProgramacionCampoVm`: 20 properties (ProgramacionId, TrabajoId, EntrevistadoId, FechaProgramada, HoraProgramada, EstadoId, Observaciones, etc.)
- `EntrevistadoDisponibleVm`: 4 properties (EntrevistadoId, Nombre, Telefono, Email)
- `ProcesoIpsVm`: 7 properties (IdProceso, TrabajoId, TipoProceso, FechaGeneracion, Estado, etc.)
- `IpsRevisionVm`: 10 properties (IdProceso, TrabajoId, NombreTrabajo, TipoProceso, Estado, ObservacionesRevision, etc.)

### Views

#### Programación
- **Index.cshtml**: 
  - Filtros: Trabajo (dropdown), FechaDesde/FechaHasta, Estado (7 opciones)
  - Grid: Tabla responsive con badges por estado, botones Editar/Avanzar/Cancelar
  - AJAX: Cambio de estado sin recargar página
  - Export: Botón Excel integrado
- **Edit.cshtml**:
  - Form: Trabajo (disabled), Entrevistado (required, dropdown dinámico), Fecha/Hora, Estado, Observaciones, MedioProgramacion, DuracionEstimada
  - Validación: Required fields, maxlength 500 chars
  - Info: Muestra teléfono/email del entrevistado seleccionado
  - CSRF: @Html.AntiForgeryToken()

#### IPS
- **Index.cshtml**:
  - Filtros: Trabajo (dropdown), FechaDesde/FechaHasta
  - Grid: Tabla con badges por estado (Pendiente/warning, Aprobado/success, Rechazado/danger)
  - Actions: Botones Aprobar/Rechazar solo si Estado = Pendiente o En Revisión
  - Modal: Bootstrap modal para rechazo con textarea observaciones requeridas
  - AJAX: Notify (aprobar) y Reject (rechazar) sin recargar página
  - Export: Botón Excel integrado

---

## 🐛 Issues Resueltos

### Issue #1: Type Mismatch en IpsRevisionUpdateModel.Rechazar
**Problema**: 
```csharp
// ❌ Código incorrecto
Rechazar = false  // Error CS0029: cannot convert bool to string
```

**Causa**: IpsRevisionUpdateModel.Rechazar es `public string Rechazar { get; init; } = string.Empty;`

**Solución**:
```csharp
// ✅ Código corregido
Rechazar = string.Empty  // Para aprobar
Rechazar = "S"           // Para rechazar
```

**Ubicación**: `CualitativoIpsController.cs` líneas 104, 143  
**Estado**: ✅ Resuelto

---

## ✅ Build Status

```
Compilación correcto con 13 advertencias en 31,1s

Warnings (non-blocking):
- 13x CS8625: No se puede convertir un literal NULL (nullability warnings)
- Ubicación: Controllers y Services pre-existentes
- Impacto: No afecta ejecución, solo advertencias de análisis estático
```

---

## 📦 Archivos Creados/Modificados

### Creados
1. `MatrixNext.Web/Services/OP/IOpProgramacionService.cs` (new)
2. `MatrixNext.Web/Services/OP/OpProgramacionService.cs` (new, ~280 LOC)
3. `MatrixNext.Web/Services/OP/Models/ProgramacionIpsVms.cs` (new, 4 classes)
4. `MatrixNext.Web/Areas/OP/Controllers/CualitativoProgramacionController.cs` (new, 7 actions)
5. `MatrixNext.Web/Areas/OP/Controllers/CualitativoIpsController.cs` (new, 5 actions)
6. `MatrixNext.Web/Areas/OP/Views/CualitativoProgramacion/Index.cshtml` (new)
7. `MatrixNext.Web/Areas/OP/Views/CualitativoProgramacion/Edit.cshtml` (new)
8. `MatrixNext.Web/Areas/OP/Views/CualitativoIps/Index.cshtml` (new)

### Modificados
1. `MatrixNext.Web/Program.cs` (línea ~165: DI registration OpProgramacionService)

---

## 🎯 Próximos Pasos

### Sprint 1 - Phase 2: Testing & Commit
- [ ] Testing manual de Programación (Index, Edit, Save, ChangeStatus, ExportExcel)
- [ ] Testing manual de IPS (Index, Notify, Reject, ExportExcel)
- [ ] Validación de integración con SPs existentes (OP_Programados_Entrevistados, OP_IPS_Revision_Get/Edit)
- [ ] Commit Sprint 1 P1 con mensaje descriptivo
- [ ] Actualizar backlog principal con estado Sprint 1

### Sprint 2: Planillas y Testing E2E
- [ ] OP-PL01: Planillas de asistencia
- [ ] OP-PL02: Validación de participantes
- [ ] Testing E2E de flujos completos
- [ ] Revisión de performance queries

---

## 📝 Notas de Desarrollo

### Decisiones Técnicas
1. **IPS Service**: Se reutilizó `IOpIpsService` existente para evitar duplicar lógica de integración con SPs
2. **Rechazar string**: Decisión legacy, usa "S"/"N" en lugar de bool (compatibilidad con DB existente)
3. **Estados Programación**: 7 estados definidos (enum en frontend por ahora, considerar tabla OP_EstadosProgramacion en DB)
4. **Export**: Integración con `IExportService` compartido (ClosedXML)
5. **CSRF**: Todos los POST actions tienen `[ValidateAntiForgeryToken]`

### Referencias a Código Legacy
- `ProgramacionCampo.aspx.vb`: 822 LOC (referencia para lógica de negocio)
- `ProcesoIPS.aspx.vb`: ~450 LOC (referencia para workflow IPS)
- SP `OP_IPS_Revision_Get`: Devuelve lista de revisiones pendientes
- SP `OP_IPS_Revision_Edit`: Actualiza estado y observaciones de revisión

---

**Fecha completado**: 9 de enero, 2026  
**Sprint**: 1 - Phase 1  
**Estado**: ✅ Backend + Frontend completos, build exitoso  
**Próxima acción**: Testing manual + Commit  
