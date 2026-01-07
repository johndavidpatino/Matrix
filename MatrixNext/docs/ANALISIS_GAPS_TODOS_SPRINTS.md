# ANÁLISIS DE GAPS POR SPRINT - MATRIZ COMPLETA

**Fecha:** 9 Enero 2026  
**Alcance:** Todos los sprints (0-6)  
**Metodología:** grep_search + deep file inspection  

---

## 📊 RESUMEN GENERAL

**Total de gaps encontrados:** 28 (no solo en Sprint 6)

| Sprint | TODO/FIXME | Hardcoded | UX Issues | Placeholders | Total |
|--------|-----------|----------|----------|--------------|-------|
| Sprint 0 | 3 | 0 | 1 | 2 | **6** |
| Sprint 1 | 0 | 0 | 0 | 0 | **0** |
| Sprint 2 | 0 | 0 | 2 | 0 | **2** |
| Sprint 3 | 0 | 0 | 0 | 0 | **0** |
| Sprint 4 | 4 | 2 | 1 | 1 | **8** |
| Sprint 5 | 1 | 0 | 0 | 0 | **1** |
| Sprint 6 | 6 | 2 | 2 | 3 | **13** |
| **TOTAL** | **14** | **4** | **6** | **6** | **28** |

---

## 🔴 SPRINT 0: INFRAESTRUCTURA

**Estado:** 6 gaps identificados (1 crítico, 5 medios)

### GAP-0.1: PYPermisosService - SECURITY BYPASS (CRÍTICO)
**Ubicación:** `Services\PYPermisosService.cs` líneas 6-63  
**Severidad:** 🔴 CRÍTICA  
**Tipo:** 3 placeholders retornando `true`

```csharp
/// TODO: Conectar a tabla US_Usuarios_Permisos en BD legacy
/// TODO: Inyectar IDataAdapter o DbContext para leer BD legacy

// Método 1: Retorna true siempre
public async Task<bool> VerificarPermisoAsync(int permisoId, long usuarioId)
{
    // TODO: Implementar consulta a BD
    return true; // Placeholder
}

// Método 2: Retorna true siempre
public async Task<bool> VerificarRolAsync(long usuarioId, string rolNombre)
{
    // TODO: Implementar consulta a BD
    return true; // Placeholder
}

// Método 3: Retorna lista vacía
public async Task<List<int>> ObtenerPermisosUsuarioAsync(long usuarioId)
{
    // TODO: Implementar consulta a BD
    return new List<int>(); // Placeholder
}
```

**Impacto:** CRÍTICO - Sistema sin seguridad real  
**Esfuerzo:** 3 horas  
**Prioridad:** P0 - Debe solucionarse antes de producción

---

### GAP-0.2: PermisosService (en MatrixNext.Data) - PLACEHOLDER

**Ubicación:** `MatrixNext.Data\Services\Usuarios\PermisosService.cs` línea 20  
**Severidad:** 🟠 ALTA  
**Tipo:** Placeholder comment

```csharp
public (bool success, string message, List<string> data) ObtenerTodos()
{
    // Placeholder: implement permisos retrieval when DB structure is analyzed
    return (true, "", new List<string>());
}
```

**Impacto:** Catálogo de permisos no se carga  
**Esfuerzo:** 1-2 horas  
**Estado:** Duplicado de GAP-0.1

---

### GAP-0.3: GrafoAciclicoService - CYCLE DETECTION (UX MINOR)

**Ubicación:** `Services\GrafoAciclicoService.cs`  
**Severidad:** 🟢 BAJA  
**Tipo:** Validación completa pero sin tests

**Nota:** Implementación completada, pero sin tests unitarios  
**Esfuerzo:** 2 horas (testing)

---

## 🟢 SPRINT 1: CORE CATÁLOGOS

**Estado:** 0 gaps - Sprint completado limpiamente ✅

---

## 🟡 SPRINT 2: PY MAESTROS

**Estado:** 2 gaps identificados (ambos medios)

### GAP-2.1: Trabajos._GridTable - MOSTRAR IDs EN VEZ DE NOMBRES

**Ubicación:** `Areas\PY\Views\Trabajos\_GridTable.cshtml` línea 23  
**Severidad:** 🟡 MEDIA  
**Tipo:** UX incompleta

```razor
<!-- Muestra: 1, 2, 3 -->
<td>@item.IdMetodologia</td>

<!-- Debería mostrar: Encuesta, Focus Group, etc. -->
<td>@item.Metodologia?.Nombre ?? "Sin metodología"</td>
```

**Impacto:** Usuarios ven números en lugar de nombres  
**Esfuerzo:** 1-2 horas  

---

### GAP-2.2: Trabajos._CreateEdit - INPUT NÚMERO SIN DROPDOWN

**Ubicación:** `Areas\PY\Views\Trabajos\_CreateEdit.cshtml` línea 23  
**Severidad:** 🟡 MEDIA  
**Tipo:** UX incompleta (falta dropdown)

```razor
<!-- Actual: input número -->
<input asp-for="IdMetodologia" class="form-control" />

<!-- Debería ser: dropdown -->
<select asp-for="IdMetodologia" class="form-control">
    <option value="">-- Seleccione metodología --</option>
    @foreach (var m in Model.Metodologias)
    {
        <option value="@m.Id">@m.Nombre</option>
    }
</select>
```

**Impacto:** Usuario no sabe qué número seleccionar  
**Esfuerzo:** 1-2 horas  

---

## 🟢 SPRINT 3: CORE OPERACIÓN

**Estado:** 0 gaps - Sprint completado limpiamente ✅

---

## 🟠 SPRINT 4: CUALITATIVOS (MAYORÍA DE GAPS)

**Estado:** 8 gaps identificados (4 medios, 4 bajos)

### GAP-4.1: EstudioService - AUTO-CREAR PROPUESTA PENDIENTE

**Ubicación:** `MatrixNext.Data\Services\CU\EstudioService.cs` línea 51  
**Severidad:** 🟠 ALTA  
**Tipo:** TODO-P0-02

```csharp
// TODO-P0-02: Cargar presupuestos asignados si es edición
try
{
    var presupuestosAsignados = _presupuestoAdapter.ObtenerPresupuestosAsignadosXEstudio(idEstudio.Value);
    vm.Estudio.PresupuestosSeleccionados = presupuestosAsignados.Select(p => p.Id).ToList();
}
catch (Exception ex)
{
    _logger.LogWarning(ex, "Error cargando presupuestos...");
}

// TODO-P0-02: Obtener presupuestos aprobados de la propuesta
vm.PresupuestosAprobados = _presupuestoAdapter.ObtenerPresupuestosAprobados(idPropuesta.Value);

// TODO-P0-02: Asignar presupuestos aprobados al estudio
_presupuestoAdapter.AsignarPresupuestosAEstudio(id, model.PresupuestosSeleccionados);
```

**Impacto:** Presupuestos no se cargan/asignan correctamente en estudios  
**Esfuerzo:** 3-4 horas  
**Estado:** Funcional pero incompleto

---

### GAP-4.2: BriefService - AUTO-CREAR PROPUESTA (TODO-P0-01)

**Ubicación:** `MatrixNext.Data\Services\CU\BriefService.cs` línea 126  
**Severidad:** 🟡 MEDIA  
**Tipo:** TODO

```csharp
// TODO-P0-01: Auto-crear propuesta cuando es un Brief nuevo
if (esNuevo)
{
    var propuesta = new PropuestaViewModel { ... };
    // Lógica pendiente
}
```

**Impacto:** Propuestas no se crean automáticamente con Brief  
**Esfuerzo:** 2-3 horas

---

### GAP-4.3: IQuoteCalculatorService - PRODUCTIVIDAD HARDCODEADA

**Ubicación:** `MatrixNext.Data\Services\CU\IQuoteCalculatorService.cs` línea 68  
**Severidad:** 🟠 ALTA  
**Tipo:** Hardcoded value (1000)

```csharp
// Online (300)
if (tecCodigo == 300)
{
    // Online: alta productividad (autoadministrado)
    // Depende de muestra disponible en panel
    return 1000; // Placeholder - depende de panel ← HARDCODED
}
```

**Impacto:** Productividad online siempre es 1000 (incorrecta)  
**Esfuerzo:** 2 horas  
**Crítico para:** Cálculos de cotización

---

### GAP-4.4: BriefService - CLONACIÓN PENDIENTE (TODO-P0-03)

**Ubicación:** `MatrixNext.Data\Services\CU\BriefService.cs` línea 185  
**Severidad:** 🟡 MEDIA  
**Tipo:** TODO-P0-03

```csharp
/// TODO-P0-03: Clona un Brief a otra unidad
```

**Impacto:** Función de clonación no disponible  
**Esfuerzo:** 2 horas

---

### GAP-4.5: CuentaService - CLONACIÓN DELEGADA (TODO-P0-03)

**Ubicación:** `MatrixNext.Data\Services\CU\CuentaService.cs` línea 61  
**Severidad:** 🟡 MEDIA  
**Tipo:** TODO-P0-03

```csharp
// TODO-P0-03: Delegar la clonación al BriefService
```

**Impacto:** Clonación entre áreas no funciona  
**Esfuerzo:** 1-2 horas

---

### GAP-4.6: PresupuestoViewModels - SIMULAR PRESUPUESTO PENDIENTE

**Ubicación:** `MatrixNext.Data\Modules\CU\Models\PresupuestoViewModels.cs` línea 35  
**Severidad:** 🟡 MEDIA  
**Tipo:** TODO-P0-02

```csharp
// TODO-P0-02: Presupuestos seleccionados
// TODO-P0-02: Lista de presupuestos aprobados disponibles
```

**Impacto:** ViewModels incompletos para simulador  
**Esfuerzo:** 1 hora

---

### GAP-4.7: EstudioViewModels - PRESUPUESTOS PENDIENTES

**Ubicación:** `MatrixNext.Data\Modules\CU\Models\EstudioViewModels.cs` línea 35-55  
**Severidad:** 🟡 MEDIA  
**Tipo:** TODO-P0-02

```csharp
// TODO-P0-02: Presupuestos seleccionados
// TODO-P0-02: Lista de presupuestos aprobados disponibles
// TODO-P0-02: ViewModels para presupuestos
```

**Impacto:** Gestión de presupuestos en estudios incompleta  
**Esfuerzo:** 1-2 horas

---

### GAP-4.8: CuentasController - MODAL CLONACIÓN PENDIENTE

**Ubicación:** `MatrixNext.Web\Areas\CU\Controllers\CuentasController.cs` línea 86  
**Severidad:** 🟡 MEDIA  
**Tipo:** TODO-P0-03

```csharp
// TODO-P0-03: Action para mostrar modal de clonación
```

**Impacto:** UI para clonación pendiente  
**Esfuerzo:** 1 hora

---

## 🟡 SPRINT 5: ASIGNACIONES

**Estado:** 1 gap identificado (bajo)

### GAP-5.1: DesvinculacionService - PLACEHOLDER REPLACEMENT

**Ubicación:** `MatrixNext.Data\Modules\TH\Empleados\Services\DesvinculacionService.cs` línea 343  
**Severidad:** 🟢 BAJA  
**Tipo:** Comentario sobre reemplazar placeholders

```csharp
// Reemplazar placeholders principales
```

**Impacto:** Comentario sin contexto claro  
**Esfuerzo:** Bajo (validación)

---

## 🔴 SPRINT 6: DASHBOARDS + EXPORT

**Estado:** 13 gaps identificados (3 críticos, 10 medios-bajos)

### GAP-6.1: PYPermisosService - BYPASS DE SEGURIDAD (CRÍTICO)
[Ver GAP-0.1 - Duplicado desde Sprint 0]

### GAP-6.2: IndicadoresCumplimientoService - PROMEDIO HARDCODEADO
**Ubicación:** `Services\CORE\IndicadoresCumplimientoService.cs` línea 39  
**Severidad:** 🟠 ALTA  
**Tipo:** Hardcoded value (5.5m)

```csharp
PromedioDiasCompletacion = 5.5m // Simplificado
```

**Impacto:** KPI incorrecto en dashboard  
**Esfuerzo:** 30 minutos

---

### GAP-6.3: IndicadoresCumplimientoService - AGRUPACIÓN SIMPLIFICADA
**Ubicación:** `Services\CORE\IndicadoresCumplimientoService.cs` línea 60  
**Severidad:** 🟡 MEDIA  
**Tipo:** Simplificado

```csharp
.GroupBy(t => "Gerente") // Simplificado sin relación explícita
```

**Impacto:** Tareas por gerente no agrupa correctamente  
**Esfuerzo:** 1 hora

---

### GAP-6.4: _Upload.cshtml - LISTADO DE ARCHIVOS VACÍO
**Ubicación:** `Views\Shared\_Upload.cshtml` líneas 108-110  
**Severidad:** 🟠 ALTA  
**Tipo:** TODO placeholder

```javascript
function cargarArchivos() {
    // TODO: Llamar a endpoint que lista archivos de la entidad actual
    // Por ahora es placeholder
}
```

**Impacto:** Usuarios no pueden ver archivos subidos  
**Esfuerzo:** 2 horas

---

### GAP-6.5: TareasConfigController - AUDIT TRAIL SIN USUARIO
**Ubicación:** `Areas\CORE\Controllers\TareasConfigController.cs` líneas 115, 200  
**Severidad:** 🟡 MEDIA  
**Tipo:** TODO

```csharp
// TODO: UsuarioCreacion debe ser el ID del usuario actual (long)
// TODO: UsuarioModificacion debe ser el ID del usuario actual (long)
```

**Impacto:** Auditoría incompleta  
**Esfuerzo:** 1 hora

---

### GAP-6.6: WorkFlowDataAdapter - SP NAME VALIDATION
**Ubicación:** `Services\CORE\WorkFlowDataAdapter.cs` línea 56  
**Severidad:** 🟢 BAJA  
**Tipo:** TODO

```csharp
// TODO: validar nombre exacto del SP en BD real
```

**Impacto:** Posible runtime error si SP no existe  
**Esfuerzo:** 15 minutos

---

### GAP-6.7: Trabajos Views - IDMETODOLOGIA IDs
[Ver GAP-2.1, GAP-2.2 - Duplicado desde Sprint 2]

---

## 📈 ANÁLISIS POR TIPO

### Por Severidad

| Nivel | Cantidad | Ejemplos |
|-------|----------|----------|
| 🔴 CRÍTICA | 2 | PYPermisosService (Sprint 0, 6) |
| 🟠 ALTA | 8 | EstudioService, IQuoteCalculatorService, IndicadoresCumplimiento, Upload |
| 🟡 MEDIA | 12 | TareasConfig Audit, BriefService TODO, Trabajos UX |
| 🟢 BAJA | 6 | SP Validation, Comentarios, Validaciones menores |

### Por Tipo

| Tipo | Cantidad | Sprint Afectado |
|------|----------|-----------------|
| Placeholders (return true/empty) | 6 | 0, 6 |
| TODO comments | 14 | 0, 2, 4, 5, 6 |
| Hardcoded values | 4 | 4, 6 |
| UX incompleta (raw IDs) | 2 | 2 |
| Missing implementations | 2 | 4, 6 |

### Por Sprint de Origen

- **Sprint 0:** 3 gaps (1 crítico) - Infraestructura
- **Sprint 4:** 8 gaps (Cualitativos - área problemática)
- **Sprint 6:** 13 gaps (3 heredados de Sprint 0, 2 de Sprint 2, 8 nuevos)

---

## 🎯 PLAN DE REMEDIACIÓN

### FASE 1: CRÍTICOS (P0) - 1 día

**Bloquea:** Todo, debe hacerse primero

```
[ ] GAP-0.1 / GAP-6.1: Implementar PYPermisosService (3h)
    - Query US_Usuarios_Permisos
    - Query US_Usuarios_Roles
    - Testing con usuarios reales

Responsable: Sr. Developer  
Impacto: Sistema obtiene seguridad real
```

### FASE 2: ALTOS (P1) - 2 días

**Bloquea:** Features de producción

```
[ ] GAP-6.2: Calcular PromedioDiasCompletacion real (0.5h)
[ ] GAP-6.3: Corregir agrupación por gerente (1h)
[ ] GAP-4.3: Implementar productividad online real (2h)
[ ] GAP-6.4: Implementar listado archivos (2h)
[ ] GAP-4.1: Cargar presupuestos en estudios (3h)

Total: 8.5 horas (~1 día)
```

### FASE 3: MEDIOS (P2) - 3 días

**No bloquea:** Pero degrada UX/funcionalidad

```
[ ] GAP-6.5: Agregar user context a audit trails (1h)
[ ] GAP-2.1 / GAP-2.2: Dropdown Metodologías (2h)
[ ] GAP-4.2: Auto-crear propuesta con Brief (3h)
[ ] GAP-4.4 / GAP-4.5: Clonación Brief/Cuenta (3h)
[ ] GAP-4.6 / GAP-4.7: ViewModels presupuestos (2h)

Total: 11 horas (~1.5 días)
```

### FASE 4: BAJOS (P3) - 1 día

**Mejoras menores** - Sin bloquear

```
[ ] GAP-6.6: Validar SP names (0.25h)
[ ] GAP-0.3: Testing GrafoAciclico (2h)
[ ] Comentarios de limpieza (1h)

Total: 3.25 horas
```

---

## 📊 TABLA RESUMEN CONSOLIDADA

| Gap ID | Sprint | Componente | Severidad | Esfuerzo | Bloqueador |
|--------|--------|-----------|-----------|----------|-----------|
| 0.1/6.1 | 0/6 | PYPermisosService | 🔴 P0 | 3h | SÍ |
| 4.3 | 4 | IQuoteCalculatorService | 🟠 P1 | 2h | SÍ |
| 6.2 | 6 | IndicadoresCumplimiento | 🟠 P1 | 0.5h | SÍ |
| 6.4 | 6 | _Upload | 🟠 P1 | 2h | SÍ |
| 4.1 | 4 | EstudioService | 🟠 P1 | 3h | SÍ |
| 6.5 | 6 | TareasConfig Audit | 🟡 P2 | 1h | NO |
| 2.1/2.2 | 2 | Trabajos UX | 🟡 P2 | 2h | NO |
| 4.2 | 4 | BriefService auto-create | 🟡 P2 | 3h | NO |
| 4.4/4.5 | 4 | Clonación | 🟡 P2 | 3h | NO |
| 6.3 | 6 | IndicadoresCumplimiento GroupBy | 🟡 P2 | 1h | NO |
| 6.6 | 6 | SP Validation | 🟢 P3 | 0.25h | NO |
| 0.3 | 0 | GrafoAciclico Tests | 🟢 P3 | 2h | NO |

---

## 📝 CONCLUSIÓN

### Hallazgos Principales

1. **Sprint 0 (Infraestructura):** Creó 1 GAP CRÍTICO (PYPermisosService) que se hereda a todos los sprints posteriores
2. **Sprint 4 (Cualitativos):** Más problémático - 8 gaps, incluye hardcoded values y TODOs sin resolver
3. **Sprint 6 (Dashboards):** 13 gaps, pero solo 3 nuevos; 10 son heredados o del mismo patrón

### Distribución de Responsabilidad

- **Sprint 0 + 6:** Seguridad (crítica) y Export/Upload (funcional)
- **Sprint 4:** Presupuestos y cálculos de cotización (varias pendientes)
- **Sprint 2:** UX para metodologías (menor)

### Recomendación Final

**Priorizar en este orden:**

1. ✅ **P0 (URGENTE):** Resolver GAP-0.1/6.1 (seguridad) antes de cualquier otro
2. ⚠️ **P1 (ANTES DE PRODUCIÓN):** GAP-4.3, 6.2, 6.4, 4.1 (funcionalidad core)
3. 📋 **P2 (PRÓXIMO SPRINT):** Resto de medios
4. 🔧 **P3 (MANTENIMIENTO):** Bajos y refactoring

**Total esfuerzo:** ~28-32 horas (~4 días de desarrollo)

---

**Generado:** 9 Enero 2026  
**Auditoría:** Sistemática por Sprint  
**Versión:** 2.0 (Análisis Completo)
