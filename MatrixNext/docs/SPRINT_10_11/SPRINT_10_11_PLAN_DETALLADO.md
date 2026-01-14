# SPRINT 10 & 11: PLAN DETALLADO DE EJECUCIÓN

**Fecha de Corte**: 2026-01-15  
**Responsable**: [DEV/QA/TECH LEAD a asignar]  
**Documentación Base**: 
- [PLAN_EJECUCION_SPRINTS_5_12.md](PLAN_EJECUCION_SPRINTS_5_12.md)
- [BACKLOG_MIGRACION_GLOBAL.md](docs/GENERAL/BACKLOG_MIGRACION_GLOBAL.md)
- [DIRECTRICES_MIGRACION.md](DIRECTRICES_MIGRACION.md)

---

## 📋 RESUMEN EJECUTIVO

| Aspecto | Sprint 10 | Sprint 11 |
|---|---|---|
| **Módulo** | RP_Reportes | OP_RO + OP_Trafico |
| **Duración** | 1-2 semanas | 2 semanas |
| **Esfuerzo** | 60h | 90h |
| **Prioridad** | 🔴 **ALTA** | 🟠 **MEDIA-BAJA** |
| **Inicio Estimado** | 2026-04-05 | 2026-04-19 |
| **Cierre Estimado** | 2026-04-16 | 2026-05-03 |
| **Dependencias** | Sprint 9 avanzado ~80% | Sprint 10 completado 100% |
| **Impacto Crítico** | Múltiples módulos necesitan reportes | OP_Cuantitativo/Cualitativo validación |
| **Riesgo Alto** | Scope de reportes (muchos tipos) | Integraciones complejas con OP_* |

---

## 🎯 SPRINT 10: RP_REPORTES (1-2 SEMANAS, 60 HORAS)

### Objetivo General
Migrar **100% de los reportes** del módulo RP_Reportes desde WebForms a MatrixNext con soporte completo de:
- Exportes en múltiples formatos (Excel/PDF)
- Filtros avanzados con autocomplete
- Paginación y búsqueda
- Permisos y auditoría
- Paridad funcional completa con legacy

---

### 📊 INVENTARIO LEGACY

**Ubicación**: `WebMatrix/RP_Reportes/`

#### PÁGINAS CRÍTICAS A MIGRAR (72 archivos .aspx identificados):

**Categoría 1: Indicadores y Dashboards (11 archivos)**
- IndicadoresCalidad.aspx
- IndicadoresCumplimientoTareas.aspx
- IndicadoresRegistroObservaciones.aspx
- IndicadoresCalidadAtiempoAlTiempo.aspx
- IndicadoresCronogramaTareas.aspx
- GanttUnTrabajo.aspx
- GanttRecursos.aspx
- Top10Encuestadores.aspx
- TrabajosConAtraso.aspx

**Categoría 2: Reportes de Operación (15 archivos)**
- ReporteActividades.aspx
- ReporteInconsistencias.aspx
- ReporteListadoTrabajos.aspx
- ReportePSTSinProduccion.aspx
- ReporteProyectosSinJBI.aspx
- ReporteTablets.aspx
- ReporteEvaluacionProveedores.aspx
- RP_RegistroProduccionOP.aspx
- ProduccionCampoPorFecha.aspx
- EstudiosXEntregarCCT.aspx
- ErroresDecampo.aspx
- RegistroObservacionesConsolidado.aspx
- ReporteRegistroObservacionesTipo.aspx

**Categoría 3: Reportes de Planeación (14 archivos)**
- PlaneacionCampo.aspx
- PlaneacionEstudios.aspx
- PlaneacionEstudiosPorSalir.aspx
- PlaneacionGeneralOperaciones.aspx
- PlaneacionOperaciones.aspx
- PlaneacionPorUnidad.aspx
- PlaneacionPorUnidadOPS.aspx
- PlaneacionPropuestas.aspx
- PlaneacionPropuestasYEstudios.aspx
- ListadoPropuestas.aspx
- MatrizEstimacionGeneral.aspx
- AsignacionYEjecucionCampo.aspx
- AvanceDeCampo.aspx
- InformeTiemposRevisionPresupuestos.aspx

**Categoría 4: Reportes de Recursos (10 archivos)**
- ListadoEncuestadores.aspx
- FichaEncuestador.aspx
- ListadoGeneralMatrix.aspx
- TrabajosPorGerencia.aspx
- TrabajosPorGrupoBU.aspx
- TrabajosPorCCT.aspx
- PersonalSinProduccion.aspx
- ListadoPlaneacionUnidades.aspx
- ListadoEstudiosSeguimiento.aspx

**Categoría 5: Reportes Especializados (16 archivos)**
- ReportesCumplimientoAtiempoAlTiempo.aspx (Reportes* varias variantes)
- ReportesCumplimientoTareas.aspx
- ReportesIndicadoresCronogramaTareas.aspx
- ReportesIndicadoresRegistroObservaciones.aspx
- ReportesMedicionProgramaProyectos.aspx
- ReportesVariablesControl.aspx
- TareasCumplimiento.aspx
- ListadoBrief.aspx
- ListadoPropuestasSeguimiento.aspx
- ListadoPropuestasSeguimientoCCT.aspx
- InformeAnulacion.aspx
- DetalleRequerimientosReporte.aspx

**Categoría 6: Menús y Navegación (6 archivos)**
- DefaultMenu.aspx
- MenuOperacionesREP.aspx
- Home.aspx
- InformacionGeneral.aspx
- InformacionGeneralCuali.aspx
- AvanceDeCampoDialog.aspx
- TraficoGeneralOperaciones.aspx (Solapado con OP_Trafico)
- TraficoAreasGeneral.aspx (Solapado con OP_Trafico)

---

### 🗄️ MAPEO DE STORED PROCEDURES

**Fuente**: `CoreProject/*` + `docs/SQL/CO_Matrix_Structure_SP.csv`

**Patrón esperado**: SP `REP_*` o contexto específico (OP_*, PY_*, TH_*, etc.)

#### Ejemplos de SP Identificadas (Preliminar - validar en CoreProject):

```
CATEGORÍA INDICADORES:
├── REP_IndicadoresCalidad_Get
├── REP_IndicadoresCumplimiento_Get
├── REP_IndicadoresRegistroObservaciones_Get
├── OP_TraficoEncuestasCiudad (consumido desde Trafico)
└── ... (15+ más identificadas en análisis previo OP_Cuantitativo)

CATEGORÍA PLANEACIÓN:
├── PY_PlaneacionCampo_Get
├── PY_PlaneacionEstudios_Get
├── PY_PlaneacionOperaciones_Get
├── PY_PlaneacionPorUnidad_Get
└── ... (similar para otras subcategorías)

CATEGORÍA RECURSOS:
├── TH_ListadoEncuestadores_Get
├── TH_FichaEncuestador_Get
├── OP_PersonalSinProduccion_Get
└── ... (conecta con TH_TalentoHumano + OP_Cuantitativo)

CATEGORÍA OPERACIÓN:
├── OP_ReporteActividades_Get
├── OP_ReporteInconsistencias_Get
├── OP_ReporteListadoTrabajos_Get
├── PY_ReporteProyectosSinJBI_Get
└── ... (varias de OP_Cuantitativo + PY_Proyectos)

CATEGORÍA ESPECIALIZADA:
├── OP_CumplimientoTareas_Get
├── OP_VariablesControl_Get
├── PY_MedicionPrograma_Get
└── ... (requiere análisis de cada reporte individual)
```

**ACCIÓN REQUERIDA (Día 1)**: 
1. ✅ Abrir CoreProject y buscar clases `REP_*` y conexas (OP_*, PY_*, TH_*)
2. ✅ Mapear exactamente el SP usado en cada WebMethod de reportes
3. ✅ Generar matriz: Reporte → SP → Parámetros → Salida (DataTable/Custom)
4. ✅ Validar en `CO_Matrix_Structure_SP.csv` si no existe en CoreProject

---

### 🔧 ARQUITECTURA DE SOLUCIÓN

#### Patrón por Implementar (AJAX-first con exportes)

```
WebMatrix/RP_Reportes/*.aspx.vb (DataAdapter)
    ↓
    SP: REP_*, OP_*, PY_*, TH_* (múltiples contextos)
    ↓
MatrixNext/MatrixNext.Web/Areas/RP/
├── Data/
│   ├── Adapters/
│   │   ├── ReportesAdapter.cs (maestro de reportes)
│   │   ├── IndicadoresAdapter.cs
│   │   ├── PlaneacionAdapter.cs
│   │   ├── RecursosAdapter.cs
│   │   └── EspecializadosAdapter.cs
│   └── Services/
│       ├── ReportesService.cs (orquestador)
│       ├── ExportService.cs (Excel/PDF)
│       └── FiltrosService.cs (validaciones)
├── Controllers/
│   ├── ReportesController.cs (CRUD + export endpoints)
│   ├── IndicadoresController.cs
│   ├── PlaneacionController.cs
│   └── ... (1 por categoría principal)
└── Views/
    ├── Shared/
    │   ├── _ReportesGrid.cshtml (tabla reutilizable)
    │   ├── _FiltrosAvanzados.cshtml (modal de filtros)
    │   └── _ExportButtons.cshtml (botones Excel/PDF)
    ├── Reportes/
    │   ├── Index.cshtml (listado de reportes disponibles)
    │   ├── Indicadores.cshtml
    │   ├── Planeacion.cshtml
    │   └── ... (1 por categoría o grupo similar)
    └── Shared/_RP_Sidebar.cshtml (navegación interna)
```

#### Tecnologías

| Aspecto | Tecnología | Razón |
|---|---|---|
| **Exportes Excel** | ClosedXML | Open source, fácil formatting |
| **Exportes PDF** | iText / QuestPDF | Performance + styling |
| **Filtros Client-side** | jQuery + DataTables | Paginación, búsqueda |
| **Autocomplete Filtros** | Select2.js | Compatible con MultiSelect |
| **API Response** | `ApiResponse<T>` (patrón existente) | Consistencia global |

---

### 📅 PLAN DIARIO (SPRINT 10: 10 DÍAS DE TRABAJO = 2 SEMANAS)

#### **SEMANA 1: Inventario, Mapeo y Adaptadores** (40 horas = 5 días)

**DÍA 1 (8h): Inventario Completo**
- [ ] Listar los 72 archivos .aspx en WebMatrix/RP_Reportes
- [ ] Clasificar por categoría (Indicadores, Planeación, Recursos, Operación, Especializados)
- [ ] Extraer WebMethods principales de cada .aspx.vb
- [ ] Identificar parámetros de filtro (fecha, usuario, proyecto, área, etc.)
- [ ] **Deliverable**: `INVENTARIO_RP_REPORTES.md` con tabla Archivo → WebMethods → Parámetros
- [ ] **Validación**: Revisar contra `Categoría 1-6` anterior; resolver discrepancias

**DÍA 2-3 (16h): Mapeo SP y Bases de Datos**
- [ ] Para cada reporte, identificar SP exacto consumido (buscar en CoreProject)
- [ ] Documentar: Reporte → SP → Parámetros entrada → Salida (DataTable/DTO)
- [ ] Validar en `CO_Matrix_Structure_SP.csv` (scripts SQL en `docs/SQL/`)
- [ ] Generar matriz de mapeo: `MAPEO_REPORTES_SP.xlsx` (Reporte | SP | Parámetros | Tipo Salida | Notas)
- [ ] Identificar SP duplicadas o con lógica común (consolidar)
- [ ] **Validación Cruzada**: Confirmar con DBAs/Scripts SQL que todos los SP existen

**DÍA 4-5 (16h): Crear Adapters y Services Base**
- [ ] Registrar NuGet: ClosedXML, iText (si no existe)
- [ ] Crear interfaz `IReportesAdapter` en `MatrixNext.Data/Adapters/`
  ```csharp
  public interface IReportesAdapter
  {
      Task<List<IndicadorDTO>> GetIndicadoresAsync(DateTime desde, DateTime hasta, int? usuarioId);
      Task<List<ReporteDTO>> GetReporteAsync(string reporteId, ReporteFiltrosDTO filtros);
      Task<DataTable> GetReportDataAsync(string spName, Dictionary<string, object> parameters);
  }
  ```
- [ ] Implementar `ReportesAdapter` usando Dapper (consulta SP) + mapping a DTOs
- [ ] Crear `ReportesService` (orquestador) que combina múltiples adapters si aplica
- [ ] Crear `ExportService` (Excel/PDF export)
- [ ] Crear `FiltrosService` (validaciones de parámetros)
- [ ] Registrar en `Program.cs` (DI):
  ```csharp
  builder.Services.AddScoped<IReportesAdapter, ReportesAdapter>();
  builder.Services.AddScoped<IReportesService, ReportesService>();
  builder.Services.AddScoped<IExportService, ExportService>();
  ```
- [ ] **Validación**: Compilar sin errores (REGLA 10 - warnings suprimidos)

---

#### **SEMANA 2: Controllers, Views y Testing** (20 horas = 2.5 días)

**DÍA 6 (8h): Controllers REST + Endpoints Export**
- [ ] Crear `ReportesController` (Area/RP) con endpoints:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> Index()  // Listado de reportes disponibles

  [HttpGet("{reporteId}")]
  public async Task<ApiResponse<object>> GetReporte(string reporteId, [FromQuery] ReporteFiltrosDTO filtros)  // AJAX

  [HttpGet("{reporteId}/export-excel")]
  public async Task<IActionResult> ExportExcel(string reporteId, [FromQuery] ReporteFiltrosDTO filtros)  // Download

  [HttpGet("{reporteId}/export-pdf")]
  public async Task<IActionResult> ExportPdf(string reporteId, [FromQuery] ReporteFiltrosDTO filtros)  // Download
  ```
- [ ] Agregar `[Authorize]` y permisos por rol
- [ ] Implementar validaciones de entrada (ModelState, custom validators)
- [ ] Agregar error handling y logging
- [ ] **Validación**: Swagger documenta correctamente; prueba manual de endpoints

**DÍA 7-8 (8h): Views Razor + AJAX**
- [ ] Crear `Views/Reportes/Index.cshtml`: listado de reportes con cards/tiles
  - Nombre, descripción, última generación, botón "Abrir"
  - Buscador por nombre
  - Filtro por categoría (dropdown)
  
- [ ] Crear modal reutilizable `_FiltrosAvanzados.cshtml`:
  - Campos dinámicos según reporte (fecha, usuario, proyecto, etc.)
  - Autocomplete para campos complejos (Select2)
  - Botón "Generar" y "Limpiar filtros"

- [ ] Crear parcial `_ReportesGrid.cshtml`:
  - Tabla con DataTables.js
  - Paginación, búsqueda, orden
  - Botones "Ver detalle", "Exportar Excel", "Exportar PDF"
  - Responsive design

- [ ] Crear `Views/Reportes/Indicadores.cshtml`, `Planeacion.cshtml`, etc.:
  - Layout base con sidebar categorías
  - Contiene modal filtros + grid
  - Integración AJAX con endpoints `/RP/Reportes/`

- [ ] Crear `Views/Shared/_RP_Sidebar.cshtml`:
  - Árbol de categorías (Indicadores, Planeación, Recursos, Operación, Especializados)
  - Links a cada reporte o vista categórica

- [ ] **Validación**: Views se cargan sin errores; AJAX funciona con API

**DÍA 9-10 (4h): QA Funcional Completa**
- [ ] Setup: Acceso a staging con datos reales RP_Reportes
- [ ] Ejecutar pruebas por categoría:
  1. **Indicadores**: Cargar tabla, aplicar filtros (fechas), exportar Excel/PDF → verificar datos
  2. **Planeación**: Filtro por área, proyecto → exportar
  3. **Recursos**: Listado de encuestadores, ficha detallada → validar permisos
  4. **Operación**: Reportes multiples, combinaciones de filtros
  5. **Especializados**: Reportes complejos con cálculos

- [ ] Validaciones:
  - ✅ Filtros aplican correctamente (datos coinciden con legacy)
  - ✅ Exportes Excel contienen toda la información
  - ✅ Exportes PDF con formato profesional
  - ✅ Permisos: solo usuarios autorizados ven reportes sensibles
  - ✅ Performance: carga tabla < 2s (incluye paginación)
  - ✅ Validación de entrada: intentar filtros inválidos → mensaje de error amable
  - ✅ Manejo de excepciones: SP falla → error 500 con logging

- [ ] Documentar bugs/issues encontrados y resolver en orden crítica

- [ ] **Deliverable**: 
  - QA Report: matriz de pruebas ejecutadas (✅ pasó / ❌ falló / ⚠️ parcial)
  - Screenshot de reportes funcionando
  - Performance notes (tiempos de carga por reporte)

---

### ✅ ENTREGABLES SPRINT 10

1. **Código Migrado** (MatrixNext):
   - ✅ `MatrixNext.Web/Areas/RP/` (Controllers, Views, Adapters, Services)
   - ✅ DTOs y models en `MatrixNext.Data/Models/RP/`
   - ✅ Registrado en DI y Program.cs
   - ✅ 0 errores de compilación

2. **Documentación**:
   - ✅ `INVENTARIO_RP_REPORTES.md` (72 archivos clasificados)
   - ✅ `MAPEO_REPORTES_SP.md` (Reporte → SP → Parámetros)
   - ✅ `MIGRACION_RP_REPORTES_COMPLETADA.md` (resumen cierre, QA ejecutado, SP usados)

3. **QA y Testing**:
   - ✅ 10+ pruebas funcionales ejecutadas (tabla de cobertura)
   - ✅ 0 bloqueadores críticos sin resolver
   - ✅ Performance validado (< 2s carga para reportes estándar)

4. **Actualizaciones**:
   - ✅ Sidebar agregado en _RP_Sidebar.cshtml
   - ✅ Menu global actualizado (si aplica)
   - ✅ Dashboard: RP_Reportes marcado como 🟢 COMPLETO

---

### ⚠️ RIESGOS SPRINT 10

| Riesgo | Probabilidad | Mitigación |
|---|---|---|
| **Scope creep: demasiados reportes** | 🔴 Alta | Priorizar top-10 reportes; postergar reportes de baja frecuencia |
| **SP complejas o duplicadas** | 🟡 Media | Análisis temprano (Día 2-3); consolidar lógica |
| **Exportes lento con datos grandes** | 🟡 Media | Implementar paginación en export; limitar filas |
| **Faltan parámetros/cambios legacy** | 🟡 Media | Validar con SME antes de Día 4; documentar varianzas |
| **Permisos inconsistentes** | 🟠 Bajo-Medio | Definir matriz de roles × reportes antes de implementar |

---

---

## 🎯 SPRINT 11: OP_RO + OP_TRAFICO (2 SEMANAS, 90 HORAS)

### Objetivo General
Migrar **OP_RO (Revisión Operacional)** y **OP_Trafico** desde WebForms a MatrixNext con:
- CRUD completo de revisiones (OP_RO)
- Gestión de tráfico de datos (OP_Trafico)
- Integración con OP_Cuantitativo/Cualitativo
- Workflows de cambio de estado
- Notificaciones
- Paridad funcional 100%

---

### 📊 INVENTARIO LEGACY

**Ubicación**: `WebMatrix/OP_RO/` (5 archivos) + `WebMatrix/OP_Trafico/` (6 archivos)

#### OP_RO (Revisión Operacional):

```
OP_RO/
├── Cuestionario.aspx (revisar cuestionarios de encuestas)
├── Instructivo.aspx (revisar instructivos de proyectos)
├── MaterialAyuda.aspx (revisar materiales de apoyo)
├── Metodologia.aspx (revisar metodologías)
└── [Implícito] Workflow: Editor → Revisor → Aprobación/Rechazo
```

**Contexto de Negocio**: 
- Los cuestionarios, instructivos, materiales y metodologías son preparados por equipos de diseño
- Los revisores operacionales (OP_RO) auditan que cumplan requisitos de calidad
- Aprobación = ir a producción; Rechazo = vuelve al editor

#### OP_Trafico (Tráfico de Datos):

```
OP_Trafico/
├── Captura.aspx (capture de datos iniciales)
├── Critica.aspx (validación de datos capturados)
├── InicioTraficoEncuestas.aspx (inicio flujo tráfico)
├── RMC.aspx (gestión de tráfico por ciudad)
├── TrabajosProyectos.aspx (asignación a trabajos)
└── Verificacion.aspx (verificación final)

Contexto: 
- Gestión del movimiento de encuestas desde digitación hasta control final
- Asignación a RMC (Revisores Metodología/Control) por ciudad
- Cambios de estado (capturado → criticado → verificado)
```

---

### 🗄️ MAPEO DE STORED PROCEDURES

**Fuentes Identificadas**:
- `CoreProject/OP_RO_*_Get_Result.vb` (9 clases de resultado)
- `CoreProject/OP_TraficoEncuestas_Result.vb`

#### OP_RO SP Identificadas:

```
Revisión Cuestionarios:
├── OP_RO_RevisionCuestionario_Get (listado revisiones)
├── OP_RO_RevisionCuestionario_Save (crear/actualizar revisión)
├── OP_RO_RevisionCuestionario_Delete (anular revisión)
├── OP_RO_RevisionCuestionario_Approve (aprobar)
└── OP_RO_RevisionCuestionario_Reject (rechazar)

Revisión Instructivos:
├── OP_RO_RevisionInstructivo_Get
├── OP_RO_RevisionInstructivo_Save
├── OP_RO_RevisionInstructivo_Delete
├── OP_RO_RevisionInstructivo_Approve
└── OP_RO_RevisionInstructivo_Reject

Revisión Metodología:
├── OP_RO_RevisionMetodologia_Get
├── OP_RO_RevisionMetodologia_Save
├── ... (similar pattern)

Revisión Material Ayuda:
├── OP_RO_RevisionMaterialAyuda_Get
├── ... (similar pattern)

Ejecución (auditoría de verificación):
├── OP_RO_EjecucionCuestionario_Get
├── OP_RO_EjecucionInstructivo_Get
├── OP_RO_EjecucionMaterialAyuda_Get
├── OP_RO_EjecucionMetodologia_Get
└── ... (registros de auditoría)
```

#### OP_Trafico SP Identificadas:

```
Gestión Tráfico:
├── OP_TraficoEncuestas_Get (listado encuestas)
├── OP_TraficoEncuestas_Save (crear/actualizar)
├── OP_TraficoEncuestas_CambiarEstado (capturado → criticado → verificado)
├── OP_TraficoEncuestasCiudad (tráfico por RMC/ciudad)
├── OP_TraficoEncuestasAsignacion (asignar a RMC)
└── OP_TraficoEncuestasVerificacion_Get (estado final)

Coordinación con OP_Cuantitativo:
├── OP_Trabajos_Get (trabajos existentes)
├── OP_Recepciones_Get (recepciones capturadas)
└── ... (sincronización con módulo OP)
```

**ACCIÓN REQUERIDA (Día 1)**: 
1. ✅ Revisar CoreProject para clases `OP_RO_*` y `OP_Trafico*`
2. ✅ Extraer exactamente los SP usados por cada .aspx.vb
3. ✅ Generar matriz: Página → SP → Parámetros → Salida
4. ✅ Validar que SP existan en `CO_Matrix_Structure_SP.csv`

---

### 🔧 ARQUITECTURA DE SOLUCIÓN

#### PATRÓN: Workflow + Estado Machine

```
WebMatrix/OP_RO/*.aspx.vb + WebMatrix/OP_Trafico/*.aspx.vb (DataAdapter)
    ↓
    SP: OP_RO_*, OP_Trafico* (múltiples acciones)
    ↓
MatrixNext/MatrixNext.Web/Areas/OP/
└── Subfolders para RO y Trafico:
    
OP_RO (Revisión Operacional):
├── Data/
│   ├── Adapters/
│   │   ├── OP_ROCuestionarioAdapter.cs
│   │   ├── OP_ROInstructivoAdapter.cs
│   │   ├── OP_ROMetodologiaAdapter.cs
│   │   └── OP_ROMaterialAyudaAdapter.cs
│   └── Services/
│       ├── OP_ROService.cs (orquestador)
│       └── OP_ROWorkflowService.cs (máquina de estados: Pendiente → Aprobado/Rechazado)
├── Controllers/
│   ├── OP_RO_CuestionarioController.cs
│   ├── OP_RO_InstructivoController.cs
│   └── ... (1 por tipo de revisión)
└── Views/
    ├── ROCuestionario/
    │   ├── Index.cshtml (listado pendientes)
    │   ├── _Detalle.cshtml (modal para ver detalles + revisar)
    │   └── _ApruebaPaquete.cshtml (modal aprobar/rechazar)
    └── ... (similar para otros tipos)

OP_Trafico (Tráfico de Datos):
├── Data/
│   ├── Adapters/
│   │   ├── OP_TraficoEncuestasAdapter.cs
│   │   ├── OP_TraficoRMCAdapter.cs
│   │   └── OP_TraficoVerificacionAdapter.cs
│   └── Services/
│       ├── OP_TraficoService.cs
│       └── OP_TraficoWorkflowService.cs (máquina: Capturado → Criticado → Verificado)
├── Controllers/
│   ├── OP_TraficoController.cs (Dashboard)
│   ├── OP_TraficoEncuestasController.cs (CRUD)
│   └── OP_TraficoRMCController.cs (gestión RMC)
└── Views/
    ├── Trafico/
    │   ├── Index.cshtml (dashboard general)
    │   ├── Encuestas.cshtml (listado por estado)
    │   ├── RMC.cshtml (tráfico por ciudad)
    │   └── _CambiarEstado.cshtml (modal workflow)
    └── Shared/
        └── _TraficoIndicadores.cshtml (KPIs por ciudad)
```

#### State Machine Example (OP_Trafico):

```csharp
public enum TraficoEstado
{
    Capturado = 1,      // Inicial
    Criticado = 2,      // Después de validación
    Verificado = 3,     // Final / Listo para producción
    Anulado = 99        // Cancelado
}

// Transiciones permitidas:
// Capturado → Criticado (cuando usuario clic "Criticar")
// Criticado → Verificado (cuando usuario clic "Verificar")
// Cualquier → Anulado (solo admin)
```

---

### 📅 PLAN DIARIO (SPRINT 11: 10 DÍAS DE TRABAJO)

#### **SEMANA 1: Inventario, Mapeo, Adapters** (50 horas = 5 días)

**DÍA 1 (10h): Inventario Completo OP_RO + OP_Trafico**

- [ ] Listar 5 archivos OP_RO + 6 archivos OP_Trafico
- [ ] Extraer WebMethods para cada módulo
- [ ] Identificar tipos de revisión (Cuestionario, Instructivo, Metodología, Material)
- [ ] Identificar flujos de tráfico (Captura, Crítica, Verificación, RMC)
- [ ] Mapear parámetros principales (IdEmpleado, IdProyecto, IdTrabajo, Estado, Observaciones, etc.)
- [ ] **Deliverable**: `INVENTARIO_OP_RO_TRAFICO.md` con tabla detallada

**DÍA 2-3 (20h): Mapeo SP + Database**

- **OP_RO**:
  - [ ] Mapear 4 tipos de revisión → SP (cada tipo tiene Save, Get, Approve, Reject)
  - [ ] Total aprox. 20 SP de OP_RO (4 tipos × 5 acciones)
  - [ ] Validar existencia en CoreProject + CO_Matrix_Structure_SP.csv
  
- **OP_Trafico**:
  - [ ] Mapear flujo: Captura → Crítica → Verificación
  - [ ] Identificar SP de movimiento de estado
  - [ ] Mapear integración con OP_Cuantitativo (trabajos, recepciones)
  - [ ] Total aprox. 12 SP de OP_Trafico

- [ ] **Deliverable**: `MAPEO_OP_RO_TRAFICO_SP.md` (Página | Acción | SP | Parámetros | Salida)
- [ ] **Validación Cruzada**: Confirmar con DBAs

**DÍA 4-5 (20h): Crear Adapters, Services y State Machines**

- **Adapters (OP_RO)**:
  ```csharp
  IOP_ROAdapter
  ├── GetRevisionesAsync(tipo, estado) → List<RevisionDTO>
  ├── GetDetalleAsync(idRevision) → RevisionDetalleDTO
  ├── SaveRevisionAsync(revision) → (bool, string)
  ├── AprobarRevisionAsync(idRevision, observaciones) → (bool, string)
  └── RechazarRevisionAsync(idRevision, observaciones) → (bool, string)
  ```

- **Adapters (OP_Trafico)**:
  ```csharp
  IOP_TraficoAdapter
  ├── GetEncuestasAsync(estado, ciudad) → List<TraficoDTO>
  ├── CambiarEstadoAsync(idTrafico, nuevoEstado) → (bool, string)
  ├── GetEncuestasCiudadAsync(ciudad) → List<TraficoRMCDTO>
  └── AsignarRMCAsync(idTrafico, idRMC) → (bool, string)
  ```

- **State Machines**:
  ```csharp
  public class OP_ROWorkflowService
  {
      public bool PuedeAprobar(Revision r) => r.Estado == EstadoRevision.Pendiente;
      public bool PuedeRechazar(Revision r) => r.Estado == EstadoRevision.Pendiente;
      // ... validaciones de transición
  }
  
  public class OP_TraficoWorkflowService
  {
      public bool PuedeCriticar(Trafico t) => t.Estado == TraficoEstado.Capturado;
      public bool PuedeVerificar(Trafico t) => t.Estado == TraficoEstado.Criticado;
      // ... etc
  }
  ```

- [ ] Registrar en Program.cs (DI):
  ```csharp
  builder.Services.AddScoped<IOP_ROAdapter, OP_ROAdapter>();
  builder.Services.AddScoped<IOP_TraficoAdapter, OP_TraficoAdapter>();
  builder.Services.AddScoped<OP_ROWorkflowService>();
  builder.Services.AddScoped<OP_TraficoWorkflowService>();
  ```

- [ ] **Validación**: Compilación exitosa, DTOs correctos

---

#### **SEMANA 2: Controllers, Views, Integraciones, QA** (40 horas = 5 días)

**DÍA 6-7 (16h): Controllers REST + Integración**

- **OP_RO Controllers**:
  ```csharp
  [Area("OP")]
  [Route("api/op/ro/[controller]")]
  public class OP_RO_CuestionarioController : ControllerBase
  {
      [HttpGet]
      public async Task<ApiResponse<List<RevisionDTO>>> GetPendientes([FromQuery] OP_RO_Filtros filtros)
      
      [HttpGet("{id}")]
      public async Task<ApiResponse<RevisionDetalleDTO>> GetDetalle(int id)
      
      [HttpPost("{id}/aprobar")]
      public async Task<ApiResponse<object>> Aprobar(int id, [FromBody] AprobacionDTO datos)
      
      [HttpPost("{id}/rechazar")]
      public async Task<ApiResponse<object>> Rechazar(int id, [FromBody] RechazoDTO datos)
  }
  ```

- **OP_Trafico Controllers**:
  ```csharp
  [Area("OP")]
  [Route("api/op/trafico/[controller]")]
  public class OP_TraficoController : ControllerBase
  {
      [HttpGet("dashboard")]
      public async Task<ApiResponse<TraficoDashboardDTO>> Dashboard([FromQuery] FiltrosFecha filtros)
      
      [HttpGet("encuestas")]
      public async Task<ApiResponse<List<TraficoDTO>>> GetEncuestas([FromQuery] OP_Trafico_Filtros filtros)
      
      [HttpPost("{id}/cambiar-estado")]
      public async Task<ApiResponse<object>> CambiarEstado(int id, [FromBody] CambioEstadoDTO datos)
  }
  ```

- **Integraciones Críticas**:
  - [ ] OP_RO debe poder acceder a datos de OP_Cuantitativo (cuestionarios, instructivos)
  - [ ] OP_Trafico debe integrar con OP_Cuantitativo (trabajos, recepciones)
  - [ ] Notificaciones cuando revisión es aprobada/rechazada (email a editor)
  - [ ] Auditoría de cambios (quién aprobó, cuándo, observaciones)

- [ ] **Validación**: Swagger, prueba manual de endpoints

**DÍA 8 (8h): Views Razor + AJAX**

- **OP_RO Views**:
  - [ ] `ROCuestionario/Index.cshtml`: listado de revisiones pendientes
    - Tabla con estado, fecha, editor, reviewer
    - Filtro por estado, fecha, editor
    - Botón "Revisar" (abre modal)
  
  - [ ] `ROCuestionario/_Detalle.cshtml` (modal):
    - Información del cuestionario (metadatos)
    - Botones "Aprobar" y "Rechazar"
    - Campo de observaciones (si aplica)
    - Historial de cambios (si existe)

  - [ ] Similar para Instructivos, Metodología, Material

- **OP_Trafico Views**:
  - [ ] `Trafico/Index.cshtml` (dashboard):
    - KPIs por estado (Capturado, Criticado, Verificado)
    - KPIs por ciudad/RMC
    - Gráfica de tráfico en el tiempo
    - Links a "Ver Encuestas"

  - [ ] `Trafico/Encuestas.cshtml`:
    - Tabla filtrable por estado, ciudad, trabajo, fecha
    - Cambiar estado (dropdown + botón "Actualizar")
    - Exportar listado

  - [ ] `Trafico/RMC.cshtml`:
    - Tráfico agrupado por ciudad (RMC)
    - Asignaciones pendientes
    - Botón "Asignar a RMC"

- [ ] **Validación**: Vistas se cargan, AJAX funciona

**DÍA 9 (8h): Workflow Testing + Integraciones**

- [ ] Testear máquina de estados completa:
  - OP_RO: Revisar cuestionario → Aprobar → Verificar que sale de "Pendientes"
  - OP_Trafico: Capturado → Criticar → Verificar → Verificado
  
- [ ] Validar integraciones:
  - [ ] OP_RO accede a cuestionarios de OP_Cuantitativo (sin errores)
  - [ ] OP_Trafico sincroniza trabajos desde OP_Cuantitativo
  - [ ] Notificaciones se envían al cambiar estado

- [ ] Pruebas de concurrencia básica:
  - Revisor A y Revisor B ven la misma revisión → uno aprueba → otro ve "ya procesado"

- [ ] **Documentar issues encontrados**

**DÍA 10 (8h): QA Final Completa**

- [ ] Setup: Acceso a staging con datos reales OP_RO + OP_Trafico

- [ ] **Pruebas OP_RO**:
  - [ ] Listar revisiones pendientes → filtrar por tipo/estado → cargar < 1s
  - [ ] Abrir detalle de revisión → datos correctos
  - [ ] Aprobar revisión → se mueve a "Procesado" → editor recibe notificación
  - [ ] Rechazar revisión → se mueve a "Rechazado" → editor recibe razón
  - [ ] Permisos: solo revisores OP_RO pueden aprobar/rechazar
  - [ ] Auditoría: historial registra quién/cuándo/qué

- [ ] **Pruebas OP_Trafico**:
  - [ ] Dashboard carga KPIs correctamente
  - [ ] Filtrar encuestas por estado/ciudad → datos coinciden con BD
  - [ ] Cambiar estado → transición válida (no permite Verificado → Capturado)
  - [ ] Exportar listado → Excel con toda la información
  - [ ] Performance: carga dashboard < 2s, tabla encuestas < 1s
  - [ ] Permisos: usuarios solo ven encuestas de su ciudad/área

- [ ] **Integraciones**:
  - [ ] OP_RO consume cuestionarios OP_Cuantitativo → datos actualizados
  - [ ] OP_Trafico sincroniza trabajos OP → nuevo trabajo aparece en Trafico
  - [ ] Notificaciones funcionan (email cuando se aprueba/rechaza)

- [ ] **Documentar** en QA Report matriz de cobertura

---

### ✅ ENTREGABLES SPRINT 11

1. **Código Migrado** (MatrixNext):
   - ✅ `MatrixNext.Web/Areas/OP/OP_RO/` (Controllers, Views, Adapters, Services, State Machines)
   - ✅ `MatrixNext.Web/Areas/OP/OP_Trafico/` (similar)
   - ✅ DTOs en `MatrixNext.Data/Models/OP/`
   - ✅ Registrado en DI + Program.cs
   - ✅ 0 errores de compilación

2. **Documentación**:
   - ✅ `INVENTARIO_OP_RO_TRAFICO.md` (11 archivos clasificados)
   - ✅ `MAPEO_OP_RO_TRAFICO_SP.md` (Página → SP → Parámetros → Salida)
   - ✅ `MIGRACION_OP_RO_COMPLETADA.md` (resumen, QA, SP usados, integraciones)
   - ✅ `MIGRACION_OP_TRAFICO_COMPLETADA.md` (similar)
   - ✅ `OP_RO_TRAFICO_STATE_MACHINE.md` (diagramas de máquina de estados)

3. **QA y Testing**:
   - ✅ 15+ pruebas funcionales ejecutadas
   - ✅ Integraciones validadas con OP_Cuantitativo/Cualitativo
   - ✅ State machine validada (no permite transiciones inválidas)
   - ✅ Performance < 2s para dashboards/listados
   - ✅ 0 bloqueadores críticos

4. **Actualizaciones**:
   - ✅ Sidebar OP actualizado con OP_RO y OP_Trafico
   - ✅ Menu global (si aplica)
   - ✅ Dashboard: OP_RO y OP_Trafico marcados como 🟢 COMPLETO

---

### ⚠️ RIESGOS SPRINT 11

| Riesgo | Probabilidad | Mitigación |
|---|---|---|
| **Integraciones rotas con OP_Cuantitativo** | 🔴 Alta | Validar APIs OP_Cuantitativo antes de Sprint 11 start; coordinar con OP team |
| **State machine de Trafico compleja** | 🟡 Media | Diagramar transiciones antes de codificar; usar librería StateMachine si es complejo |
| **Datos obsoletos en staging** | 🟡 Media | Pedir refresh de datos OP_Trafico antes de QA |
| **Permisos inconsistentes** | 🟠 Bajo-Medio | Definir matriz roles × acciones; documentar antes de Día 4 |
| **Notifications no se envían** | 🟡 Media | Testear email service tempranamente (Día 6) |

---

---

## 🔄 DEPENDENCIAS Y SECUENCIA

```
Sprint 9 (Home) 80% completo
    ↓
Sprint 10 (RP_Reportes) puede iniciar en paralelo
    ├─ COMPLETO (100%)
    └─→ Genera datos para Sprint 11

Sprint 10 (RP_Reportes) 100% COMPLETADO
    ↓
Sprint 11 (OP_RO + OP_Trafico) puede iniciar
    └─ Requiere OP_Cuantitativo/Cualitativo listos (✅ ya están)
```

**Timeline Recomendado**:
- **Sprint 10 start**: 2026-04-05 (cuando Sprint 9 esté 80%)
- **Sprint 10 end**: 2026-04-16 (2 semanas)
- **Sprint 11 start**: 2026-04-19 (solapamiento de 3 días para ramp-up)
- **Sprint 11 end**: 2026-05-03 (HITO GLOBAL: 100% módulos alta/media completados)

---

## 📋 CHECKLIST FINAL

### Pre-Sprint 10
- [ ] Inventario legacy (72 archivos RP_Reportes) confirmado
- [ ] Mapeo SP completo y validado
- [ ] NuGet packages (ClosedXML, iText) agregados
- [ ] Acceso a staging RP_Reportes confirmado
- [ ] Recursos asignados (1 dev, 1 QA parcial)

### Pre-Sprint 11
- [ ] Inventario legacy (11 archivos OP_RO + OP_Trafico) confirmado
- [ ] Mapeo SP completo y validado
- [ ] State machine diseñada y documentada
- [ ] APIs OP_Cuantitativo validadas (datos accesibles)
- [ ] Recursos asignados (1 dev, 1 QA)

### Post-Sprint 10
- [ ] 🟢 RP_Reportes COMPLETO en MatrixNext
- [ ] QA ejecutado y documentado
- [ ] Dashboard actualizado
- [ ] Cierre de sprint: document + demo

### Post-Sprint 11
- [ ] 🟢 OP_RO + OP_Trafico COMPLETO en MatrixNext
- [ ] Integraciones validadas
- [ ] QA ejecutado (incluye integraciones)
- [ ] Dashboard actualizado: SPRINT 11 = FIN (🎯 2026-05-03 HITO CRÍTICO ALCANZADO)
- [ ] Cierre de sprint: demo + retrospectiva

---

## 📞 CONTACTOS Y ESCALACIÓN

- **Sprint Lead (Dev Principal)**: [Asignar]
- **QA Lead**: [Asignar]
- **Tech Lead Review**: Validar arquitectura (Día 1 cada sprint)
- **Bloqueadores**: Escalar inmediatamente al Team Lead

---

**Documento creado**: 2026-01-15  
**Próxima revisión**: Cuando Sprint 9 esté 80% completo (pre-Sprint 10 kickoff)  
**Actualización frecuencia**: Diaria durante ejecución de sprints

