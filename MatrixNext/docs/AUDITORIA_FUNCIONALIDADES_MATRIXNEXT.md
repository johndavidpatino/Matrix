# AUDITORÍA DE FUNCIONALIDADES: MatrixNext vs WebMatrix

> **Fecha**: 18 de enero de 2026  
> **Objetivo**: Identificar funcionalidades en MatrixNext que NO existían en WebMatrix  
> **Resultado**: Clasificar para REMOVER, MANTENER o CONSOLIDAR

---

## 📊 RESUMEN EJECUTIVO

| Área | Controllers MN | Páginas WM | Nuevas sin WM | Acción |
|------|----------------|------------|---------------|--------|
| CC/FinzOpe | 22 | 33 | **6** | ⚠️ Revisar |
| OP | 42 | 53 (33+20) | **5** | ⚠️ Revisar |
| TH/Api | 3 | WebMethods | **0** | ✅ OK |

---

## 1️⃣ ÁREA CC/FinzOpe (22 Controllers vs 33 Páginas)

### ✅ MANTENER - Correspondencia correcta (16 controllers)

| Controller MatrixNext | Página WebMatrix | Estado |
|-----------------------|-----------------|--------|
| `AnulacionLiquidacionesController` | EliminarCargueProduccion.aspx | ✅ OK |
| `CargueDescuentosSsController` | CargueDescuentosSS.aspx | ✅ OK |
| `CcFinzOpeController` | CuentasdeCobro.aspx, RecepcionCuentasdeCobro.aspx | ✅ OK |
| `ConteoTrabajosController` | ConteoTrabajos.aspx, ReporteConteoTrabajos.aspx | ✅ OK |
| `ControlPresupuestosController` | ConfiguracionPresupuesto.aspx, PresupuestosPorSegmento.aspx | ✅ OK |
| `EstadoJobBooksController` | EstadoJobBooks.aspx | ✅ OK |
| `GenerarBonificacionController` | GenerarBonificacion.aspx | ✅ OK |
| `LiquidarPlanillasActividadesController` | LiquidarPlanillasActividades.aspx | ✅ OK |
| `LiquidarProductividadPstController` | LiquidarProductividadPST.aspx | ✅ OK |
| `PresupuestosInternosController` | PresupuestosInternos.aspx, PresupuestoInterno.aspx, PresupuestosInternosIndex.aspx | ✅ OK |
| `RegistroProduccionController` | Produccion.aspx, CargarInformacion.aspx | ✅ OK |
| `ReporteActividadesProduccionController` | ReporteActividadesProduccion.aspx | ✅ OK |
| `ReporteContabilizacionPstController` | ReporteContabilizacionPST.aspx | ✅ OK |
| `ReporteConteosController` | ReporteConteoTrabajos.aspx | ✅ OK |
| `ReportePagosController` | ReportePagos.aspx | ✅ OK |
| `RequerimientosEquipoController` | GenerarRequerimientos.aspx, ListadodeRequerimientos.aspx | ✅ OK |
| `ResumenProductividadController` | ResumenesdeProduccion.aspx | ✅ OK |
| `RevisarGeneracionBonificacionController` | (parte de GenerarBonificacion.aspx) | ✅ OK |

### ⚠️ REVISAR - Sin correspondencia clara en WebMatrix (6 controllers)

| Controller MatrixNext | Busqueda en WebMatrix/CoreProject | Decisión |
|-----------------------|-----------------------------------|----------|
| `CalculoJornadaLaboralController` | ❌ No encontrado | 🔴 **REMOVER** |
| `ConsolidacionProduccionController` | ❌ No encontrado | 🔴 **REMOVER** |
| `AsignacionCostosPstController` | ❌ No encontrado | 🔴 **REMOVER** |
| `ReporteVarianzasPresupuestariasController` | ❌ No encontrado | 🔴 **REMOVER** |
| | | |
| **Nota**: Estos controllers fueron creados durante la migración pero NO existían en WebMatrix. Violan regla "solo paridad funcional". |

---

## 2️⃣ ÁREA OP (42 Controllers vs 53 Páginas)

### ✅ MANTENER - Correspondencia con WebMatrix/OP_Cuantitativo (33 páginas)

| Controller MatrixNext | Página WebMatrix | Estado |
|-----------------------|-----------------|--------|
| `ActivacionEncuestasController` | ActivacionEncuestas.aspx | ✅ OK |
| `AnulacionEncuestasController` | AnulacionEncuestas.aspx | ✅ OK |
| `ConsultaTrabajosController` | ConsultaTrabajos.aspx | ✅ OK |
| `EstimacionProduccionController` | EstimacionProduccion.aspx | ✅ OK |
| `FichaCuantitativaController` | FichaCuantitativa.aspx | ✅ OK |
| `HomeController` | HomeGestion.aspx | ✅ OK |
| `HomeRecoleccionController` | HomeRecoleccion.aspx | ✅ OK |
| `IFieldController` | iFieldConfiguration.aspx | ✅ OK |
| `ImportacionMasivaController` | ImportarDatos.aspx + ImportarPlanillas.aspx | ✅ OK (consolidación válida) |
| `IpsController` | IPS.aspx | ✅ OK |
| `MuestraTrabajosController` | MuestraTrabajos.aspx | ✅ OK |
| `PlanillasAprobacionController` | PlanillasCargadas.aspx, PlanillasRevisadas.aspx | ✅ OK |
| `ProduccionController` | RegistroProduccionOP.aspx | ✅ OK |
| `ProductividadController` | ProductividadRevisadaCampo.aspx, ProductividadRevisadaCoordinador.aspx, etc. | ✅ OK |
| `RegistroProduccionOPController` | RegistroProduccionOP.aspx | ✅ OK |
| `ReportesController` | (varios reportes) | ✅ OK |
| `RevisionProductividadCampoController` | RevisionProductividadCampo.aspx | ✅ OK |
| `RevisionProductividadCoordinadorController` | RevisionProductividadCoordinador.aspx | ✅ OK |
| `RevisionProductividadMYSCallController` | RevisionProductividadMYSCall.aspx | ✅ OK |
| `RevisionProductividadPMOController` | RevisionProductividadPMO.aspx | ✅ OK |
| `SupervisionController` | SupervisionCampoTelefonico.aspx | ✅ OK |
| `TrabajosCallCenterController` | TrabajosCallCenter.aspx | ✅ OK |
| `TrabajosController` | Trabajos.aspx | ✅ OK |
| `TrabajosCoordinadorController` | TrabajosCoordinador.aspx | ✅ OK |
| `TraficoController` | TraficoEncuestas.aspx | ✅ OK |

### ✅ MANTENER - Correspondencia con WebMatrix/OP_Cualitativo (20 páginas)

| Controller MatrixNext | Página WebMatrix | Estado |
|-----------------------|-----------------|--------|
| `CualitativoCampoController` | CampoCualitativo.aspx | ✅ OK |
| `CualitativoFichasController` | FichaEntrevista.aspx, FichaObservacion.aspx, FichaSesion.aspx | ✅ OK |
| `CualitativoFiltrosController` | AprobacionesFiltros.aspx, DisenarFiltros.aspx, VisualizadorFiltros.aspx | ✅ OK |
| `CualitativoIpsController` | IPSCuali.aspx | ✅ OK |
| `CualitativoMuestraController` | MuestraTrabajos.aspx (cuali) | ✅ OK |
| `CualitativoPlanillasController` | AdministracionRegistroPlanillas.aspx | ✅ OK |
| `CualitativoProgramacionController` | ProgramacionCampo.aspx | ✅ OK |
| `CualitativoTrabajosController` | Trabajos.aspx (cuali), TrabajosCoordinador.aspx (cuali) | ✅ OK |

### ✅ MANTENER - Correspondencia con WebMatrix/OP_RO (CoreProject)

| Controller MatrixNext | Correspondencia | Estado |
|-----------------------|-----------------|--------|
| `OP_ROController` | CoreProject/Clases/OP_RO/* + WebMatrix/OP_RO/ | ✅ OK |
| `OP_ROViewController` | WebMatrix/OP_RO/Cuestionario.aspx, etc. | ✅ OK |

### ✅ MANTENER - Correspondencia con WebMatrix/OP_Trafico

| Controller MatrixNext | Correspondencia | Estado |
|-----------------------|-----------------|--------|
| `OP_TraficoController` | CoreProject/TraficoEncuestas + WebMatrix/OP_Trafico/ | ✅ OK |
| `OP_TraficoViewController` | WebMatrix/OP_Trafico/Captura.aspx, Critica.aspx, etc. | ✅ OK |

### ⚠️ REVISAR - Funcionalidades nuevas (5 controllers)

| Controller MatrixNext | Busqueda en WebMatrix/CoreProject | Decisión |
|-----------------------|-----------------------------------|----------|
| `AvancesController` | ❌ No encontrado - Dashboard de progreso migración | 🟡 **INTERNO** |
| `PortalController` | ❌ No encontrado - Dashboard OP | 🟡 **EVALUAR** |
| `FiltersController` | ❌ API para autocomplete - mejora técnica | 🟢 **MANTENER** |
| `EncuestasController` | Consolida funciones de ActivacionEncuestas + AnulacionEncuestas | 🟡 **EVALUAR** |
| `PresupuestosController` | Duplica SolicitudPresupuestosInternos.aspx? | 🟡 **EVALUAR** |

---

## 3️⃣ ÁREA TH/Api (3 Controllers REST)

### ✅ MANTENER - Equivalencia con WebMethods de WebMatrix

| API Controller MatrixNext | WebMethods en WebMatrix | Estado |
|---------------------------|------------------------|--------|
| `Api/CatalogosController` | EmpleadosAdmin.aspx: getAreasServiceLines, getCargos, getBandas, etc. | ✅ OK |
| `Api/EmpleadosController` | EmpleadoUpdate.aspx, EmpleadosAdmin.aspx WebMethods | ✅ OK |
| `Api/DesvinculacionesController` | DesvinculacionesEmpleadosGestionRRHH.aspx: `<WebMethod>` decorators | ✅ OK |

### Verificación de WebMethods encontrados:
```
WebMatrix/TH_TalentoHumano/DesvinculacionesEmpleadosGestionRRHH.aspx.vb:
  - IniciarProcesoDesvinculacion() ← Migrado a POST /api/th/desvinculaciones
  - DesvinculacionEmpleadosEstatusEvaluacionesPor() ← Migrado a GET /api/th/desvinculaciones/{id}/evaluaciones
  - PDFFormato() ← Migrado a GET /api/th/desvinculaciones/{id}/pdf

WebMatrix/TH_TalentoHumano/EmpleadosAdmin.aspx.vb:
  - getAreasServiceLines() ← Migrado a /TH/Catalogos/AreasServiceLines
  - getCargos() ← Migrado a /TH/Catalogos/Cargos
  - getBandas() ← Migrado a /TH/Catalogos/Bandas
```

---

## 🔴 LISTA PARA REMOVER (6 controllers)

### Área CC/FinzOpe:
1. `CalculoJornadaLaboralController` - Funcionalidad nueva, no existe en WebMatrix
2. `ConsolidacionProduccionController` - Funcionalidad nueva, no existe en WebMatrix
3. `AsignacionCostosPstController` - Funcionalidad nueva, no existe en WebMatrix
4. `ReporteVarianzasPresupuestariasController` - Funcionalidad nueva, no existe en WebMatrix

### Acciones requeridas:
```powershell
# Archivos a eliminar (después de confirmar que no se usan):
Remove-Item MatrixNext.Web/Areas/CC/Controllers/CalculoJornadaLaboralController.cs
Remove-Item MatrixNext.Web/Areas/CC/Controllers/ConsolidacionProduccionController.cs
Remove-Item MatrixNext.Web/Areas/CC/Controllers/AsignacionCostosPstController.cs
Remove-Item MatrixNext.Web/Areas/CC/Controllers/ReporteVarianzasPresupuestariasController.cs
# También eliminar vistas y servicios asociados
```

---

## 🟡 LISTA PARA EVALUAR (5 controllers)

### Área OP:
1. `AvancesController` - Dashboard interno de migración → **Marcar como [Obsolete] o mover a área interna**
2. `PortalController` - Dashboard OP unificado → **Evaluar si es mejora válida o reemplazo de Home**
3. `EncuestasController` - Consolida Activación + Anulación → **Puede quedarse si no duplica**
4. `PresupuestosController` - Revisar si duplica otro controller → **Consolidar si es duplicado**

---

## 🟢 LISTA PARA MANTENER (Mejoras técnicas válidas)

1. `FiltersController` - API de autocomplete que mejora UX sin agregar funcionalidad nueva

---

## 📋 CONSOLIDACIONES DETECTADAS

### OP/ImportacionMasivaController → ✅ VÁLIDO
- Combina: ImportarDatos.aspx + ImportarPlanillas.aspx
- Razón: Mejora UX con wizard unificado
- Mantiene: Misma funcionalidad de carga de datos/planillas

---

## ⚠️ PRÓXIMOS PASOS

1. **Confirmar con negocio** antes de eliminar controllers CC nuevos
2. **Verificar referencias** en vistas, rutas y menús antes de eliminar
3. **Ejecutar tests** después de cada eliminación
4. **Documentar** razón de eliminación en commits

---

## 📝 NOTAS ADICIONALES

### Regla violada:
> "Solo migrar acciones existentes en WebMatrix - PROHIBIDO agregar funcionalidades nuevas"

### Controllers con potencial duplicación:
- `PresupuestosController` (OP) vs `PresupuestosInternosController` (CC)
- `TrabajosController` (OP) vs `ConsultaTrabajosController` (OP)

---

**Auditoría realizada**: 18/01/2026  
**Archivos revisados**: 67 controllers, 53+ páginas aspx  
**Método**: Comparación nombre-a-nombre + búsqueda en CoreProject
