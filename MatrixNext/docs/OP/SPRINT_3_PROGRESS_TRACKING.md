# 📊 Sprint 3 - Progress Tracking

## Objetivo Sprint 3
Completar módulos de Programación e IPS para gestión de campo cualitativo, incluyendo vistas completas y workflows de notificación/rechazo.

---

## 📈 Visual Progress

### Sprint 3 - Programación e IPS (10-14h)

#### OP-P01: Programación de Campo
**Backend**: [████████████████████] 100% → COMPLETO
- ✅ IOpProgramacionService (7 métodos)
- ✅ OpProgramacionService (~400 LOC Dapper)
- ✅ CualitativoProgramacionController (7 actions)
- ✅ ViewModels (ProgramacionCampoVm, EntrevistadoDisponibleVm)
- ✅ DI registration (Program.cs)

**Frontend**: [████████████████████] 100% → COMPLETO (pre-existentes)
- ✅ Views/CualitativoProgramacion/Index.cshtml
- ✅ Views/CualitativoProgramacion/Edit.cshtml
- ✅ DataTable con estados (Creado/Asignado/Confirmado/Ejecutado/Cancelado/etc.)
- ✅ AJAX para cambiar estados
- ✅ Export Excel integrado

#### OP-I01: Gestión de Procesos IPS
**Backend**: [████████████████████] 100% → COMPLETADO
- ✅ IOpIpsService extendido (6 nuevos métodos)
- ✅ OpIpsService implementación (~250 LOC nuevos)
- ✅ CualitativoIpsController (5 actions) - pre-existente
- ✅ ViewModels (IpsRevisionVm, ProcesoIpsVm)
- ✅ Build successful con 22 advertencias (nullability - pre-existentes)

**Frontend**: [████████████████████] 100% → COMPLETO (pre-existente)
- ✅ Views/CualitativoIps/Index.cshtml
- ✅ Grid editable con acciones Notificar/Rechazar
- ✅ Filtros por trabajo/proceso/método
- ✅ Export Excel con filtros

---

## 🔍 Detalles de Implementación

### Servicios Actualizados

#### OpIpsService - Nuevos Métodos (Sprint 3)
```csharp
// 1. ObtenerRevisionesAsync (con filtros)
Task<(bool success, List<IpsRevisionVm> data, string error)> ObtenerRevisionesAsync(
    long? trabajoId, int? procesoId, string? metodo, string? userRol);

// 2. ObtenerProcesosAsync (dropdown)
Task<(bool success, List<ProcesoIpsVm> data, string error)> ObtenerProcesosAsync();

// 3. NotificarProcesoAsync (workflow)
Task<(bool success, string error)> NotificarProcesoAsync(long id, long usuarioId);

// 4. RechazarProcesoAsync (workflow con observaciones)
Task<(bool success, string error)> RechazarProcesoAsync(
    long id, long usuarioId, string observaciones);

// 5. ActualizarEstadoAsync (cambio estado genérico)
Task<(bool success, string error)> ActualizarEstadoAsync(
    long id, int nuevoEstado, long usuarioId, string? observaciones);

// 6. ExportarRevisionesExcelAsync (export con filtros)
Task<byte[]> ExportarRevisionesExcelAsync(
    long? trabajoId, int? procesoId, string? metodo, string? userRol);
```

**Evidencia**:
- Archivo: `MatrixNext.Web/Services/OP/OpIpsService.cs`
- LOC agregados: ~250 LOC (métodos nuevos)
- LOC totales: ~430 LOC
- Referencia legacy: `IPSCuali.aspx.vb` líneas 28-290

### Controllers Implementados

#### CualitativoIpsController (pre-existente, compatible con nuevos métodos)
- **Index**: GET con filtros (trabajoId, procesoId, metodo)
- **Notificar**: POST AJAX (cambio estado 1→2)
- **Rechazar**: POST AJAX con observaciones requeridas (cambio estado →5)
- **ExportExcel**: GET con filtros aplicados
- **ActualizarEstado**: POST AJAX genérico para cambios de estado

**Estados IPS**:
1. Generado
2. Notificado
3. En Revisión
4. Aprobado
5. Rechazado

### Tablas de Base de Datos

#### Programación
- `OP_Programaciones` (programaciones de campo)
- `OP_Entrevistados` (participantes disponibles)
- Estados: 1=Creado, 2=Asignado, 3=Confirmado, 4=Ejecutado, 5=Cancelado, 6=NoAsistio, 7=Reprogramado

#### IPS
- `OP_IPS_Revisiones` (revisiones de procesos IPS)
- `OP_IPS_Procesos` (catálogo de procesos)
- `PY_Trabajo` (join para JobBook/Trabajo)
- `US_Usuarios` (join para revisores)

---

## ✅ Funcionalidades Completadas

### Programación de Campo
- [x] Listar programaciones por trabajo
- [x] Crear/editar programaciones
- [x] Asignar entrevistadores
- [x] Confirmar programaciones (estado 1/2→3)
- [x] Cancelar programaciones con motivo
- [x] Exportar a Excel (IExportService)
- [x] Estados con badges de colores

### Gestión IPS
- [x] Listar revisiones con filtros (trabajo, proceso, método)
- [x] Notificar procesos (estado 1→2)
- [x] Rechazar procesos con observaciones requeridas (→5)
- [x] Actualizar estados de workflow
- [x] Exportar revisiones a Excel con ClosedXML
- [x] Grid editable con acciones por estado

---

## 🔒 Patrones de Seguridad Implementados

### Autenticación y Autorización
- [Authorize] attribute en todos los controllers
- Claims authentication (ClaimTypes.NameIdentifier, ClaimTypes.Role)
- Validación de usuario autenticado antes de operaciones

### CSRF Protection
- [ValidateAntiForgeryToken] en todos los POST actions
- @Html.AntiForgeryToken() en vistas con formularios

### SQL Injection Prevention
- Dapper queries parametrizadas (todos los parámetros con @)
- No concatenación de strings en queries
- Ejemplo: `WHERE r.TrabajoId = @TrabajoId`

### XSS Prevention
- Razor auto-escape HTML (@Model.Property)
- Validación de observaciones requeridas en rechazo

---

## 📊 Estadísticas Sprint 3

| Métrica | Valor |
|---------|-------|
| **Servicios actualizados** | 1 (OpIpsService) |
| **Nuevos métodos** | 6 (IPS) |
| **Controllers** | 2 (Programación + IPS, pre-existentes) |
| **Views** | 3 (Index Programación, Edit Programación, Index IPS) |
| **LOC agregados** | ~250 LOC (OpIpsService) |
| **Build status** | SUCCESS con 22 warnings (nullability - pre-existentes) |
| **Tareas completadas** | 2 (OP-P01, OP-I01) |
| **Tiempo invertido** | ~6h (vs 22h estimadas - eficiencia por código pre-existente) |

---

## 🎯 Referencia WebForms Legacy

### ProgramacionCampo.aspx (822 LOC)
- **Ubicación**: `WebMatrix/OP_Cualitativo/ProgramacionCampo.aspx.vb`
- **Funcionalidad migrada**:
  - Grid con estados (líneas 45-89)
  - Crear/editar programaciones (líneas 125-214)
  - Export Excel (líneas 250-310)
  - Estados enumerados (líneas 320-380)

### IPSCuali.aspx (682 LOC)
- **Ubicación**: `WebMatrix/OP_Cualitativo/IPSCuali.aspx.vb`
- **Funcionalidad migrada**:
  - SqlDataSource OP_IPS_Procesos (líneas 28-35)
  - gvRevision_RowDataBound (líneas 38-125)
  - btnNotificar_Click (líneas 145-178)
  - btnRechazar_Click (líneas 180-215)
  - btnExport_Click (líneas 220-255)
  - gvRevision_RowUpdating (líneas 260-290)

---

## 🚀 Próximos Pasos

### Inmediatos
- [x] Build verification Sprint 3
- [x] Update tracking document
- [ ] Commit Sprint 3 completo
- [ ] Actualizar backlog principal

### Sprint 4 (siguiente)
Según backlog `BACKLOG_MODULO_OP_CUALITATIVO.md`:
- Validación de Participantes (OP-V01)
- Optimización de performance (caching, índices)
- Testing E2E final
- Integración con queues/email si aplica

---

**Fecha completado**: 9 de enero, 2026  
**Sprint**: 3 - Programación e IPS  
**Estado**: ✅ COMPLETADO  
**Build**: SUCCESS con 22 warnings (nullability - pre-existentes)  
**Próxima acción**: Commit Sprint 3
