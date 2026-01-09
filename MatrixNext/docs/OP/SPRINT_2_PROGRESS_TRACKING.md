# 📊 Sprint 2 - Progress Tracking

## Objetivo Sprint 2
Completar administración de planillas de moderación/informes cualitativos, implementar validación de participantes, testing E2E de flujos principales.

---

## 📈 Visual Progress

### ⏳ Sprint 2 - Phase 1: Planillas Backend

#### OP-PL01: Administración de Planillas
**Backend**: [████████████████████] 100% → COMPLETO (Controller 11 actions + Service 11 métodos)
- ✅ IOpPlanillasModeracionService (11 métodos)
- ✅ OpPlanillasModeracionService (~650 LOC, Dapper queries + CoreProject SPs)
- ✅ CualitativoPlanillasController (11 actions: Index, EditModeracion, SaveModeracion, EditInforme, SaveInforme, AprobarPlanilla, RechazarPlanilla, ExportExcel, BuscarJobBooks, ObtenerModeradoresDisponibles, ObtenerTecnicas)
- ✅ ViewModels (7 VMs: PlanillaListItemVm, PlanillaModeracionVm, PlanillaInformeVm, AprobacionPlanillaVm, JobBookSearchVm, ModeradorVm, TecnicaVm)
- ✅ DI registration (Program.cs line ~170)
- ✅ Build successful con 22 advertencias (nullability - pre-existentes)

**Frontend**: [████████████████████] 100% → COMPLETO
- ✅ Views/CualitativoPlanillas/Index.cshtml (grid + filtros + modales + AJAX)
- ✅ Views/CualitativoPlanillas/EditModeracion.cshtml (form con validación + autocomplete JobBooks)
- ✅ Views/CualitativoPlanillas/EditInforme.cshtml (form con validación + autocomplete JobBooks)
- ✅ Modal rechazo con observaciones requeridas
- ✅ Badges estados (EnEspera/Aprobado/NoAprobado)
- ✅ DataTable con paginación manual
- ✅ AJAX aprobar/rechazar workflow
- ✅ Build successful con 22 advertencias (nullability - pre-existentes)

---

### Fase 4: Validación Participantes (8h) - DÍA 5
- ⏳ Extender OpProgramacionService con ValidarParticipantesAsync
- ⏳ Integración con OP_MuestraTrabajos para verificar cupos

### Fase 5: Testing E2E (4h) - ✅ GUÍA PREPARADA (Ejecución pospuesta para fin de migración)
- ✅ Guía de testing creada (TESTING_GUIDE_SPRINT_2_PLANILLAS.md)
- ✅ 10 casos de prueba documentados (TC-PL-01 a TC-PL-10)
- ✅ Checklist de verificación completo (44 ítems)
- ✅ Verificación de integración backend/frontend completa
- ✅ No hay errores de compilación
- ✅ Instrucciones de ejecución documentadas
- ⏸️ **Testing manual pospuesto**: Se ejecutará al final de la migración completa
- ⏸️ Documentación de resultados pendiente para fase final

### Fase 6: Commit Sprint 2 → EN PROGRESO
- ⏳ Preparar commit con cambios Sprint 2
- ⏳ Mensaje descriptivo con resumen de implementación

---

## 🔍 Verificación de Integración (Fase 4)

### ✅ Componentes Backend
- **Service Interface**: IOpPlanillasModeracionService.cs (11 métodos) ✅
- **Service Implementation**: OpPlanillasModeracionService.cs (~650 LOC Dapper) ✅
- **ViewModels**: PlanillasModeracionVms.cs (7 modelos) ✅
- **DI Registration**: Program.cs línea 170 ✅
- **Build Status**: SUCCESS con 22 warnings (nullability - pre-existentes) ✅

### ✅ Componentes Frontend
- **Controller**: CualitativoPlanillasController.cs (11 actions) ✅
  - Index (GET + filtros + paginación)
  - EditModeracion (GET)
  - SaveModeracion (POST + CSRF)
  - EditInforme (GET)
  - SaveInforme (POST + CSRF)
  - AprobarPlanilla (POST AJAX + CSRF)
  - RechazarPlanilla (POST AJAX + CSRF + validación observaciones)
  - ExportExcel (GET)
  - BuscarJobBooks (GET AJAX)
  - ObtenerModeradoresDisponibles (GET AJAX)
  - ObtenerTecnicas (GET AJAX)

- **Views**: 3 archivos Razor ✅
  - Index.cshtml (grid + filtros + modal rechazo + AJAX)
  - EditModeracion.cshtml (form + autocomplete + validación)
  - EditInforme.cshtml (form + autocomplete + validación)

### ✅ Patrones de Seguridad Implementados
- **Autenticación**: [Authorize] attribute en controller ✅
- **CSRF Protection**: [ValidateAntiForgeryToken] en POST actions ✅
- **SQL Injection Prevention**: Queries Dapper parametrizadas ✅
- **XSS Prevention**: Razor auto-escape HTML (@Model.Property) ✅
- **Claims Authentication**: ClaimTypes.NameIdentifier para UsuarioId ✅

### ✅ Funcionalidades Principales
- **CRUD Planillas Moderación**: CREATE/READ/UPDATE ✅
- **CRUD Planillas Informes**: CREATE/READ/UPDATE ✅
- **Workflow Aprobación**: Estado 1 → 2 (En Espera → Aprobado) ✅
- **Workflow Rechazo**: Estado 1 → 3 (En Espera → No Aprobado) con observaciones requeridas ✅
- **Autocomplete JobBooks**: AJAX con debounce 300ms, mínimo 2 caracteres ✅
- **Filtros**: TipoPlantilla (Moderación/Informes) + StatusRegistro (1/2/3) ✅
- **Paginación Manual**: Querystring (pageIndex, pageSize), default 25 registros/página ✅
- **Export Excel**: IExportService integration, nombres dinámicos con timestamp ✅

### ✅ ViewModels con Propiedades Alias
- **PlanillaListItemVm**: JobBook (alias JobDesc), UsuarioCreacion ✅
- **PlanillaModeracionVm**: JobBook (get/set JobDesc), UsuarioCreacion string ✅
- **PlanillaInformeVm**: JobBook (get/set JobDesc), UsuarioAprobacion, FechaAprobacion ✅
- **ModeradorVm**: NombreModerador (alias Nombre) ✅
- **JobBookSearchVm**: JobBook, NombreTrabajo (ambos alias JobDesc) ✅

### 📋 Guía de Testing Manual
**Documento**: [TESTING_GUIDE_SPRINT_2_PLANILLAS.md](TESTING_GUIDE_SPRINT_2_PLANILLAS.md)

**Casos de Prueba Documentados**: 10
1. TC-PL-01: Listado de Planillas con Filtros
2. TC-PL-02: Crear Planilla de Moderación
3. TC-PL-03: Crear Planilla de Informes
4. TC-PL-04: Editar Planilla de Moderación
5. TC-PL-05: Aprobar Planilla (Workflow)
6. TC-PL-06: Rechazar Planilla con Observaciones (Workflow)
7. TC-PL-07: Exportar Planillas a Excel
8. TC-PL-08: Búsqueda JobBooks (Autocomplete)
9. TC-PL-09: Paginación Manual
10. TC-PL-10: Validación de Permisos y Seguridad

**Instrucciones para Testing**:
```powershell
# 1. Ejecutar aplicación
cd MatrixNext/MatrixNext.Web
dotnet run

# 2. Navegar a módulo
URL: https://localhost:5001/OP/Cualitativo/Planillas

# 3. Ejecutar casos de prueba según guía
# Ver TESTING_GUIDE_SPRINT_2_PLANILLAS.md para pasos detallados
```

### ⏳ Pendiente para Completar Fase 4
- [ ] Ejecutar aplicación en entorno local
- [ ] Login con usuario de prueba con permisos OP
- [ ] Ejecutar 10 casos de prueba manualmente
- [ ] Documentar defectos en sección "Issues Encontrados"
- [ ] Verificar queries SQL reales contra base de datos CoreProject
- [ ] Validar que SPs/tablas existen (PY_PlanillaModeracion, PY_PlanillaInformes)
- [ ] Testing de performance con >100 registros
- [ ] Testing responsive en móvil (Bootstrap grid)

---

## ❌ Fase 4: Validación Participantes (PENDIENTE - Separado de Sprint 2)

**Frontend**: [░░░░░░░░░░░░░░░░░░░░] 0% (4h)
- ⏳ Modal validación en programación
- ⏳ Indicators de cupos disponibles

---

## 📋 Detalles Técnicos

### Servicios a Crear

#### IOpPlanillasModeracionService
- **Métodos planificados**:
  - `ObtenerPlanillasAsync(tipoPlantilla, idEstado, pageIndex, pageSize)`: Grid paginado
  - `ObtenerPlanillaModeracionAsync(idPlanilla)`: Load form moderación
  - `ObtenerPlanillaInformeAsync(idPlanilla)`: Load form informe
  - `GuardarPlanillaModeracionAsync(model)`: INSERT/UPDATE moderación
  - `GuardarPlanillaInformeAsync(model)`: INSERT/UPDATE informe
  - `AprobarPlanillaAsync(idPlanilla, usuarioId, observaciones)`: Aprobar con workflow
  - `RechazarPlanillaAsync(idPlanilla, usuarioId, observaciones)`: Rechazar con workflow
  - `ExportarPlanillasExcelAsync(tipoPlantilla, idEstado)`: Excel via ClosedXML

#### OpPlanillasModeracionService
- **Tablas**: PY_PlanillaModeracion, PY_PlanillaInformes (CoreProject)
- **SPs esperados**: 
  - `PY_PlanillaModeracion_Get` (filtros + paginación)
  - `PY_PlanillaModeracion_Insert/Update`
  - `PY_PlanillaInformes_Insert/Update`
  - `PY_PlanillaModeracion_Aprobar`
- **Validaciones**:
  - JobBook existente en BI
  - Fechas válidas
  - Técnica/moderador asignados
  - Observaciones requeridas en rechazo

### Controllers a Crear

#### CualitativoPlanillasController
- **Archivo**: `MatrixNext.Web/Areas/OP/Controllers/CualitativoPlanillasController.cs`
- **Actions planificadas**:
  - `Index(tipoPlantilla, statusRegistro)`: Grid con filtros (tipos: Moderacion, Informes)
  - `EditModeracion(id)`: Load form moderación
  - `SaveModeracion(model)`: POST crear/editar moderación
  - `EditInforme(id)`: Load form informe
  - `SaveInforme(model)`: POST crear/editar informe
  - `AprobarPlanilla(idPlanilla, observaciones)`: AJAX aprobar
  - `RechazarPlanilla(idPlanilla, observaciones)`: AJAX rechazar con observaciones required
  - `ExportExcel(tipoPlantilla, statusRegistro)`: Descarga Excel
  - `BuscarJobBooks(termino)`: AJAX search JobBooks
  - `ObtenerModeradoresDisponibles()`: AJAX dropdown moderadores

### ViewModels

#### PlanillasModeracionVms.cs (nuevo archivo)
```csharp
// PlanillaModeracionVm
public class PlanillaModeracionVm
{
    public long IdPlanilla { get; set; }
    public long? IdJob { get; set; }
    public string JobDesc { get; set; } = string.Empty;
    public DateTime? FechaPlanilla { get; set; }
    public int? IdTecnica { get; set; }
    public string NombreTecnica { get; set; } = string.Empty;
    public int? Muestra { get; set; }
    public long? IdModerador { get; set; }
    public string NombreModerador { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
    public short IdEstadoAprobacion { get; set; } // 1=EnEspera, 2=Aprobado, 3=NoAprobado
    public string EstadoAprobacion { get; set; } = string.Empty;
    public DateTime? FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
}

// PlanillaInformeVm
public class PlanillaInformeVm
{
    public long IdPlanilla { get; set; }
    public long? IdJob { get; set; }
    public string JobDesc { get; set; } = string.Empty;
    public DateTime? Fecha { get; set; }
    public string Tecnica { get; set; } = string.Empty;
    public int? Muestra { get; set; }
    public long? IdCuentasUU { get; set; }
    public string Analista { get; set; } = string.Empty;
    public string ServiceLineName { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
    public short IdEstadoAprobacion { get; set; }
    public string EstadoAprobacion { get; set; } = string.Empty;
}

// AprobacionPlanillaVm
public class AprobacionPlanillaVm
{
    public long IdPlanilla { get; set; }
    public string TipoPlantilla { get; set; } = string.Empty; // "Moderacion" o "Informes"
    public string Accion { get; set; } = string.Empty; // "Aprobar" o "Rechazar"
    public string Observaciones { get; set; } = string.Empty;
}
```

---

## 🎯 Referencia WebForms Legacy

### AdministracionRegistroPlanillas.aspx
- **Ubicación**: `WebMatrix/OP_Cualitativo/AdministracionRegistroPlanillas.aspx`
- **JavaScript**: `AdministracionRegistroPlanillas.js` (~500 LOC)
- **Service**: `RegistroPlanillasCualitativoService.js`
- **Funcionalidad**:
  - Grid con filtros (tipo plantilla, estado aprobación)
  - Modal para crear/editar planilla moderación
  - Modal para crear/editar planilla informes
  - Aprobar/Rechazar con observaciones
  - Export a Excel por tipo y estado
  - Búsqueda JobBooks autocompletable
  - Dropdown moderadores/analistas

### RegistroPlanillasCualitativo.aspx (PY_Proyectos)
- **Ubicación**: `WebMatrix/PY_Proyectos/RegistroPlanillasCualitativo.aspx`
- **WebMethods**:
  - `SavePlanillaModeracion` (INSERT/UPDATE)
  - `SavePlanillaInformes` (INSERT/UPDATE)
  - `PlanillasGet` (filtros + paginación)
  - `SaveStatusAprobacionModeracion` (aprobar/rechazar)
  - `GetJobsBy` (búsqueda JobBooks)
  - `GetModeradores` (dropdown)
  - `GetTecnicas` (dropdown por tipo)

---

## 📦 Archivos a Crear

### Backend (6 archivos)
1. `MatrixNext.Web/Services/OP/IOpPlanillasModeracionService.cs` (interface, 8 métodos)
2. `MatrixNext.Web/Services/OP/OpPlanillasModeracionService.cs` (~350 LOC)
3. `MatrixNext.Web/Services/OP/Models/PlanillasModeracionVms.cs` (3 ViewModels)
4. `MatrixNext.Web/Areas/OP/Controllers/CualitativoPlanillasController.cs` (10 actions)

### Frontend (3 archivos)
1. `MatrixNext.Web/Areas/OP/Views/CualitativoPlanillas/Index.cshtml` (grid + filtros + modales)
2. `MatrixNext.Web/Areas/OP/Views/CualitativoPlanillas/_ModalModeracion.cshtml` (partial modal)
3. `MatrixNext.Web/Areas/OP/Views/CualitativoPlanillas/_ModalInforme.cshtml` (partial modal)

### Modificados
1. `MatrixNext.Web/Program.cs` (DI registration: AddScoped<IOpPlanillasModeracionService>)

---

## 🗺️ Roadmap Sprint 2

### Fase 1: Backend Services (16h) - DÍA 1-2 ✅ COMPLETO
- ✅ Crear IOpPlanillasModeracionService.cs
- ✅ Implementar OpPlanillasModeracionService.cs con Dapper (~650 LOC)
- ✅ Crear PlanillasModeracionVms.cs (7 ViewModels)
- ✅ Registrar DI en Program.cs
- ✅ Build verification (22 warnings nullability - non-blocking)

### Fase 2: Backend Controller (8h) - DÍA 3 ✅ COMPLETO
- ✅ Crear CualitativoPlanillasController.cs (11 actions)
- ✅ Validaciones con [ValidateAntiForgeryToken]
- ✅ AJAX endpoints para aprobar/rechazar
- ✅ Export Excel integration
- ✅ Build verification (SUCCESS con 22 warnings pre-existentes)
- ✅ 11 actions: Index, EditModeracion, SaveModeracion, EditInforme, SaveInforme, AprobarPlanilla, RechazarPlanilla, ExportExcel, BuscarJobBooks, ObtenerModeradoresDisponibles, ObtenerTecnicas

### Fase 3: Frontend Views (8h) - DÍA 4 ✅ COMPLETO
- ✅ Crear Index.cshtml (grid Bootstrap + DataTables)
- ✅ Crear EditModeracion.cshtml (form con validación)
- ✅ Crear EditInforme.cshtml (form con validación)
- ✅ AJAX para JobBooks search (autocomplete)
- ✅ AJAX para aprobar/rechazar
- ✅ Badges estados (EnEspera/Aprobado/Rechazado)
- ✅ Modal rechazo con observaciones obligatorias
- ✅ Build verification (SUCCESS con 22 warnings pre-existentes)

### Fase 4: Validación Participantes (8h) - DÍA 5
- [ ] Extender OpProgramacionService con ValidarParticipantesAsync
- [ ] Query OP_MuestraTrabajos para verificar cupos
- [ ] Modal validación en programación
- [ ] Indicators en UI

### Fase 5: Testing E2E (4h) - DÍA 5
- [ ] Testing manual flujo completo planillas
- [ ] Testing aprobación/rechazo workflow
- [ ] Testing export Excel
- [ ] Testing validación participantes
- [ ] Commit Sprint 2

---

## 📝 Notas de Implementación

### Decisiones Técnicas
1. **Tipo Plantilla**: Enum o string? → String ("Moderacion", "Informes") para compatibilidad con BD existente
2. **Estados**: 1=EnEspera, 2=Aprobado, 3=NoAprobado (tabla PY_EstadosAprobacion)
3. **JobBook search**: LIKE query contra CoreProject (tabla JobBook o similar)
4. **Moderadores**: Query US_Usuarios con rol específico
5. **Export**: Reutilizar IExportService existente (ClosedXML)

### SPs CoreProject Esperados
```sql
-- Listar planillas con filtros
PY_PlanillaModeracion_Get @TipoPlantilla, @IdEstado, @PageIndex, @PageSize

-- Insertar/actualizar moderación
PY_PlanillaModeracion_Insert @IdJob, @FechaPlanilla, @IdTecnica, @Muestra, @IdModerador, @Observaciones, @UsuarioId
PY_PlanillaModeracion_Update @IdPlanilla, @IdJob, @FechaPlanilla, ...

-- Insertar/actualizar informes
PY_PlanillaInformes_Insert @IdJob, @Fecha, @Tecnica, @Muestra, @IdCuentasUU, @Analista, @ServiceLineName, @Observaciones, @UsuarioId
PY_PlanillaInformes_Update @IdPlanilla, ...

-- Aprobar/rechazar
PY_PlanillaModeracion_Aprobar @IdPlanilla, @UsuarioId, @Observaciones
PY_PlanillaModeracion_Rechazar @IdPlanilla, @UsuarioId, @Observaciones
```

**Si SPs no existen**: Usar queries directas Dapper con INSERT/UPDATE/SELECT

---

**Fecha inicio**: 9 de enero, 2026  
**Sprint**: 2 - Planillas y Testing  
**Estado**: ⏳ Backend iniciando  
**Estimación**: 44h total (16h backend + 8h controller + 8h views + 8h validación + 4h testing)  
**Próxima acción**: Crear IOpPlanillasModeracionService  
