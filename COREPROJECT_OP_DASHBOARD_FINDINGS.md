# CoreProject - OP Dashboard & Metrics Implementation Analysis

## Executive Summary

Found **NO direct Dashboard metrics implementations** (OP_Dashboard_Metricas or OP_Trabajos_Activos SPs in legacy code).

However, identified **3 primary OP_Cuantitativo collection task classes** that implement metrics-related functionality through Stored Procedures and data aggregation.

---

## 📊 Found Classes - OP_Cuantitativo Collection Tasks

### 1. **TraficoEncuestas** (Operational Traffic Management)

| Property | Value |
|----------|-------|
| **Class Name** | `TraficoEncuestas` |
| **File Path** | `CoreProject/Clases/OP_Cuanti/TraficoEncuestas.vb` |
| **Namespace** | `OP` |
| **Context** | Survey traffic tracking by unit and city |
| **Key SPs Used** | `OP_TraficoEncuestasCiudad`, `OP_TraficoEncuestas_Get`, `OP_TraficoEncuestas_ListadoGet` |

#### Methods (Relevant to Collection Dashboard):

| Method Name | SP Name | Parameters | Return Type | Purpose |
|---|---|---|---|---|
| `ObtenerTraficoEncuestasXTrabajo()` | `OP_TraficoEncuestasCiudad` | `TrabajoID: Int32` | `List<OP_TraficoEncuestasCiudad_Result>` | Get traffic by work/job |
| `ObtenerEnviosXUnidadEnviaYTrabajo()` | `OP_TraficoEncuestas_Get` | `TrabajoId: Int64, Unidad: Int32` | `List<OP_TraficoEncuestas_Get_Result>` | Shipments sent by unit |
| `ObtenerEnviosXUnidadRecibeYTrabajo()` | `OP_TraficoEncuestas_Get` | `TrabajoId: Int64, Unidad: Int32` | `List<OP_TraficoEncuestas_Get_Result>` | Shipments received by unit |
| `ObtenerEnviosXUnidadYTrabajo()` | `OP_TraficoEncuestas_Get` | `TrabajoId, UnidadEnvia, UnidadRecibe` | `List<OP_TraficoEncuestas_Get_Result>` | Bidirectional unit traffic |
| `ObtenerEnviosListadoGet()` | `OP_TraficoEncuestas_ListadoGet` | `TrabajoId, UnidadEnvia, UnidadRecibe` | `List<OP_TraficoEncuestas_ListadoGet_Result>` | Detailed traffic listing |
| `ObtenerMuestraEnviadaCiudadRMC()` | `OP_TraficoEncuestasMuestraCiudadesRMC` | `TrabajoId: Int64` | `List<OP_TraficoEncuestasMuestraCiudadesRMC_Result>` | Sample sent by city (RMC) |

#### Related Edit Operations:

| Method Name | SP Name | Parameters | Purpose |
|---|---|---|---|
| `GuardarTraficoEnvio()` | N/A (Direct EF) | Entity: `OP_TraficoEncuestas` | Save traffic shipment |
| `BorrarEnvio()` | `OP_TraficoEncuestasBorrarEnvio` | `idEnvio: Int64` | Delete shipment record |

---

### 2. **FichaCuantitativo** (Quantitative Specification Form)

| Property | Value |
|----------|-------|
| **Class Name** | `FichaCuantitativo` |
| **File Path** | `CoreProject/Clases/OP_Cuanti/FichaCuantitativo.vb` |
| **Namespace** | Root (Global) |
| **Context** | Quantitative research spec metadata (objectives, methodology, budgets) |
| **Key SPs Used** | `OP_FichaCuantitativo_Get`, `OP_FichaCuantitativo_Add`, `OP_FichaCuantitativo_Edit`, `OP_FichaCuantitativo_Del` |

#### Methods:

| Method Name | SP Name | Parameters | Return Type | Purpose |
|---|---|---|---|---|
| `DevolverTodos()` | `OP_FichaCuantitativo_Get` | None (defaults NULL) | `List<OP_FichaCuantitativo_Get_Result>` | Get all quantitative specs |
| `DevolverxTrabajoID()` | `OP_FichaCuantitativo_Get` | `TrabajoID: Int64?` | `List<OP_FichaCuantitativo_Get_Result>` | Get specs by work ID |
| `DevolverxID()` | `OP_FichaCuantitativo_Get` | `ID: Int64` | `OP_FichaCuantitativo_Get_Result` | Get single spec by ID |
| `Guardar()` | `OP_FichaCuantitativo_Add` / `OP_FichaCuantitativo_Edit` | ID, TrabajoId, GrupoObjetivo, CubrimientoGeografico, MarcoMuestral, DistribucionMuestra, Cuotas, NivelDesagregacionResultados, Ponderacion, RequerimientosEspeciales, OtrasObservaciones, IncentivoEconomico, PresupuestoIncentivo, RegalosCliente, CompraIpsos, Presupuesto | `Decimal` (FichaCuantitativaID) | Create or update spec |
| `Eliminar()` | `OP_FichaCuantitativo_Del` | `ID: Int64` | `Integer` | Delete spec record |

#### Data Fields Tracked (Metrics-relevant):
- Grupo Objetivo (Target Group)
- Cobertura Geográfica (Geographic Coverage)
- Marco Muestral (Sample Frame)
- Distribución de Muestra (Sample Distribution)
- Cuotas (Quotas)
- Incentivo Económico (Economic Incentive)
- Presupuesto (Budget)

---

### 3. **GestionTrabajosOP** (OP Work Management)

| Property | Value |
|----------|-------|
| **Class Name** | `GestionTrabajosOP` |
| **File Path** | `CoreProject/Clases/OP/GestionTrabajosOP.vb` |
| **Namespace** | Root (Global) |
| **Context** | Core operational work listing and filtering |
| **Key SPs Used** | `OP_Trabajos_Get`, `Op_MuestraTrabajosGet`, `OP_Trabajos_xCoordinador_Get`, `OP_Trabajos_CallCenter_Get` |

#### Methods:

| Method Name | SP Name | Parameters | Return Type | Purpose |
|---|---|---|---|---|
| `ListaTrabajos()` | `OP_Trabajos_Get` | Id, Nombre, JobBook, Proyecto, COE, GerenteCuentas, Unidad, Gerencia, Propuesta, Estado (Int32?) | `List<OP_Trabajos_Get_Result>` | List active OP works with filtering |
| `MuestraXTrabajo()` | `Op_MuestraTrabajosGet` | `IdMuestra, IdTrabajo` | `List<Op_MuestraTrabajos_Get_Result>` | Get sample/work mapping |
| `ListaTrabajosXCoordinador()` | `OP_Trabajos_xCoordinador_Get` | Same as ListaTrabajos + Coordinador | `List<OP_Trabajos_xCoordinador_Get_Result>` | Works by coordinator filter |
| `ListaTrabajosCallCenter()` | `OP_Trabajos_CallCenter_Get` | Same as ListaTrabajos + Coordinador | `List<OP_Trabajos_CallCenter_Get_Result>` | Call Center specific works |

#### Key Filters (Dashboard-relevant):
- **Estado** (State/Status) - Critical for dashboard status breakdown
- **Unidad** (Unit) - For unit-level metrics
- **Proyecto** (Project) - For project aggregation
- **Gerencia** (Management) - For hierarchical rollups
- **COE** (Center of Excellence) - For organizational metrics

---

## 📊 Production Recording Classes

### 4. **RecordProduccion** (Production Recording & Estimation)

| Property | Value |
|----------|-------|
| **Class Name** | `RecordProduccion` |
| **File Path** | `CoreProject/Clases/OP_Cuanti/RegistroProduccion.vb` |
| **Namespace** | Root (Global) |
| **Context** | Daily production tracking and estimation management |
| **Key SPs Used** | `OP_UnidadesProduccionGet`, `OP_ActividadesProduccionGet`, `OP_Produccion_Get`, `OP_Produccion_Add`, `OP_Produccion_Edit`, `OP_JBE_JBI_CC_Get`, `REP_InformeConsolidadoEjecucion` |

#### Methods (Dashboard-relevant):

| Method Name | SP Name | Parameters | Return Type | Purpose |
|---|---|---|---|---|
| `ObtenerUnidades()` | `OP_UnidadesProduccionGet` | `identificacion: Int64?` | `List<OP_UnidadesProduccionGet_Result>` | Get production units |
| `MatrizActividades()` | `OP_ActividadesProduccionGet` | `Unidadid, Actividad, SubActividad, activa` | `List<OP_ActividadesProduccionGet_Result>` | Activity matrix by unit |
| `JBE_JBI()` | `OP_JBE_JBI_CC_Get` | `tipo: Int32` | `List<OP_JBE_JBI_CC_Get_Result>` | Job Book Element / Job Book Item |
| `JBE_JBI_Busqueda()` | `OP_JBE_JBI_CC_Get` | `tipo: Int32, busqueda: String` | `List<OP_JBE_JBI_CC_Get_Result>` | Search JBE/JBI codes |
| `REP_InformeConsolidadoEjecucion()` | `REP_InformeConsolidadoEjecucion` | `fechaInicio, fechaFin, areaId` | `List<REP_InformeConsolidadoEjecucion_Result>` | **Consolidated execution report** |
| `obtener()` | `OP_Produccion_Get` | `fechaInicio, fechaFin, personaId, id, unidad` | `List<OP_Produccion_Get_Result>` | Retrieve production records |
| `grabar()` | `OP_Produccion_Add` | actividad, subActividad, unidad, trabajoId, estudioId, fecha, horaInicio, horaFin, cantidad, observacion, estado, etc. | `Void` | Record new production |
| `actualizar()` | `OP_Produccion_Edit` | Same params as grabar + id | `Void` | Update production record |

#### Metrics Tracked:
- **Cantidad** (Quantity/Count)
- **Fecha** (Date)
- **Horas** (Hours - start/end)
- **Estado** (Status)
- **Persona** (Person/User)
- **Cantidad Efectivas** (Effective Quantity)
- **Tipo Reproceso** (Reprocess Type - for quality metrics)

---

### 5. **PlaneacionProduccion** (Production Planning & Estimation)

| Property | Value |
|----------|-------|
| **Class Name** | `PlaneacionProduccion` |
| **File Path** | `CoreProject/Clases/OP_Cuanti/PlaneacionProduccion.vb` |
| **Namespace** | Root (Global) |
| **Context** | Production estimation at work and city level |
| **Key SPs Used** | `OP_PlaneaccionProduccionAutomatica`, `OP_PlaneaccionProduccionManual`, `OP_MuestraTrabajosUpdateFechas` |

#### Methods (Dashboard-relevant):

| Method Name | SP Name | Parameters | Return Type | Purpose |
|---|---|---|---|---|
| `ObtenerEstimacionxIdList()` | N/A (Direct EF) | `EstimacionId: Int64` | `List<OP_EstimacionProduccion>` | Get estimation records |
| `ObtenerEstimacionxTrabajo()` | N/A (Direct EF) | `EstimacionId: Int64` | `OP_EstimacionProduccion` | Get single estimation |
| `GuardarEstimacion()` | N/A (Direct EF) | `id, cantidad` | `Void` | Save/update quantity estimate |
| `ObtenerEstimacionCiudadxTrabajoList()` | N/A (Direct EF) | `TrabajoId: Int64` | `List<OP_EstimacionesProduccionCiudad>` | Get city-level estimations |
| `AgregarEstimacionAutomatica()` | `OP_PlaneaccionProduccionAutomatica` | TrabajoId, UsuarioId, lunes...domingo, festivos (Boolean flags) | `Void` | **Auto-generate estimation by day** |
| `AgregarEstimacionManual()` | `OP_PlaneaccionProduccionManual` | TrabajoId, UsuarioId, day flags, estimacionid | `Void` | **Manual estimation creation** |
| `AgregarEstimacionCiudad()` | N/A (Direct EF) | TrabajoId, UsuarioId, day flags, Observaciones, Ciudad | `OP_EstimacionesProduccionCiudad` | Add city-level estimation |

#### Key Estimation Fields:
- **Cantidad** (Estimated Quantity)
- **FechaEstimacion** (Estimation Date)
- **CiudadId** (City)
- **Bloqueada** (Locked/Finalized)
- **Activa** (Active)

---

### 6. **OP_CuantiDapper** (Dapper-based Queries for Planillas/Worksheets)

| Property | Value |
|----------|-------|
| **Class Name** | `OP_CuantiDapper` |
| **File Path** | `CoreProject/Clases/OP_Cuanti/OP_CuantiDapper.vb` |
| **Namespace** | Root (Global) |
| **Context** | High-performance planilla (worksheet) queries using Dapper ORM |
| **Key SPs Used** | `OP_CuantiPlanillas_GET`, `OP_CuantiPlanillasTrabajos_GET`, `OP_CuantiPlanillasPendientes_GET`, `OP_CuantiPlanillas_Trabajos_Update` |

#### Methods (Dashboard-relevant):

| Method Name | SP Name | Parameters | Return Type | Purpose |
|---|---|---|---|---|
| `CuantiPlanillasGet()` | `OP_CuantiPlanillas_GET` | `Revisado: Bool?, PMO, Fini: Date?, Ffin, TrabajoId, Coordinador` | `List<OP_CuantiPlanillasModel>` | **Get all planillas with filters** |
| `CuantiPlanillasTrabajosGet()` | `OP_CuantiPlanillasTrabajos_GET` | `Revisado, PMO, Fini, Ffin, TrabajoId, Coordinador` | `List<OP_CuantiPlanillasTrabajosModel>` | **Planillas by work** |
| `CuantiPlanillasPendientesGet()` | `OP_CuantiPlanillasPendientes_GET` | `Revisado, PMO, Fini, Ffin, TrabajoId` | `List<OP_CuantiPlanillasPendientesModel>` | **Pending planillas** |
| `CuantiPlanillasTrabajosUpdate()` | `OP_CuantiPlanillas_Trabajos_Update` | `Revisado, PMO, Fini, Ffin, TrabajoId, UsuarioRevisa` | `String` | Batch update planillas status |

#### Dashboard Metrics Available:
- **Revisado** (Reviewed status)
- **PMO** (Project Manager/Owner filter)
- **Fecha Inicio / Fecha Fin** (Date range for trending)
- **Coordinador** (Coordinator filter for unit breakdown)
- **TrabajoId** (Work-level drill-down)

---

## 📋 Stored Procedures Summary (Referenced in Code)

### Traffic/Tráfico Related SPs:
1. `OP_TraficoEncuestasCiudad` - Traffic by city
2. `OP_TraficoEncuestas_Get` - Traffic detailed records
3. `OP_TraficoEncuestas_ListadoGet` - Traffic listing with aggregates
4. `OP_TraficoEncuestasMuestraCiudadesRMC` - Sample delivery by city
5. `OP_TraficoEncuestasBorrarEnvio` - Delete traffic record
6. `OP_TraficoEncuestas_Edit_Critica` - Update at criticism stage
7. `OP_TraficoEncuestas_Edit_Verificacion` - Update at verification stage

### Production Related SPs:
1. `OP_Produccion_Get` - Production records query
2. `OP_Produccion_Add` - Insert production record
3. `OP_Produccion_Edit` - Update production record
4. `OP_UnidadesProduccionGet` - Unit list for production
5. `OP_ActividadesProduccionGet` - Activity matrix
6. `OP_JBE_JBI_CC_Get` - Job Book reference codes

### Planilla Related SPs:
1. `OP_CuantiPlanillas_GET` - All planillas
2. `OP_CuantiPlanillasTrabajos_GET` - Planillas by work
3. `OP_CuantiPlanillasPendientes_GET` - Pending planillas
4. `OP_CuantiPlanillas_Trabajos_Update` - Update planilla status

### Estimation Related SPs:
1. `OP_PlaneaccionProduccionAutomatica` - Auto-estimate by day
2. `OP_PlaneaccionProduccionManual` - Manual estimation
3. `OP_MuestraTrabajosUpdateFechas` - Update sample dates

### Spec/Ficha Related SPs:
1. `OP_FichaCuantitativo_Get` - Quantitative spec retrieval
2. `OP_FichaCuantitativo_Add` - Create quantitative spec
3. `OP_FichaCuantitativo_Edit` - Update quantitative spec
4. `OP_FichaCuantitativo_Del` - Delete quantitative spec

### Work Management SPs:
1. `OP_Trabajos_Get` - Active works list (**Key for dashboard**)
2. `Op_MuestraTrabajosGet` - Sample/work mapping
3. `OP_Trabajos_xCoordinador_Get` - Works by coordinator
4. `OP_Trabajos_CallCenter_Get` - Call Center works

### Reporting SPs:
1. `REP_InformeConsolidadoEjecucion` - **Consolidated execution report** (city/date/area)

---

## 🎯 Dashboard Implementation Candidates

### For Collection Task Dashboard (HomeRecoleccion):

**Recommended Primary SP for dashboard:**
- `OP_Trabajos_Get` (from GestionTrabajosOP.ListaTrabajos) - **Active works listing by status**

**Secondary SPs for metrics:**
1. `OP_CuantiPlanillas_GET` - Planilla submission status
2. `OP_TraficoEncuestasCiudad` - Traffic by city heatmap
3. `OP_CuantiPlanillasPendientes_GET` - Pending work count
4. `REP_InformeConsolidadoEjecucion` - Execution trend by date/area

**Supporting Data Classes:**
- `GestionTrabajosOP` - Work filtering and state aggregation
- `OP_CuantiDapper` - High-performance planilla metrics
- `RecordProduccion` - Daily production trending

---

## 🔍 NO Direct Dashboard SPs Found

The legacy WebMatrix does **NOT have**:
- ❌ `OP_Dashboard_Metricas` SP
- ❌ `OP_Trabajos_Activos` SP (this concept is implemented via `OP_Trabajos_Get` with Estado filter)
- ❌ `HomeRecoleccion` dedicated dashboard SP
- ❌ `HomeOP` unified dashboard SP

**Instead:** Dashboard is built by **composing multiple SPs** from the above classes.

---

## 📌 Key Findings Summary

| Finding | Details |
|---------|---------|
| **Total Classes Found** | 6 primary OP_Cuanti related classes |
| **Total SP References** | 20+ Stored Procedures |
| **Missing Dashboard SP** | None - metrics are built from operational SPs |
| **Primary Metrics Source** | `OP_Trabajos_Get` (GestionTrabajosOP) |
| **Secondary Metrics** | `OP_CuantiPlanillas_GET`, `REP_InformeConsolidadoEjecucion` |
| **Query Approach** | Mostly EF Core + Limited Dapper (OP_CuantiDapper) |
| **Dashboard Pattern** | Multi-SP composition (no unified dashboard SP) |

---

## 🚀 Migration Recommendation

For MatrixNext Home Dashboard (OP module):

1. **Create OP_Dashboard_Metricas SP** aggregating:
   - Work count by status (from OP_Trabajos_Get)
   - Pending planillas (from OP_CuantiPlanillasPendientes_GET)
   - Traffic summary by unit (from OP_TraficoEncuestasCiudad)
   - Production summary by day (from OP_Produccion_Get)

2. **Map to existing services:**
   - Use `GestionTrabajosOP` as base
   - Extend with `OP_CuantiDapper` for planilla metrics
   - Add `RecordProduccion` for production metrics

3. **Create Dashboard Service** (MatrixNext.Web):
   - `OpDashboardService` - Orchestrate calls to above classes
   - Cache aggressively (10-15 minute TTL)
   - Return unified `OpDashboardDTO`

---

**Document Generated:** 2026-01-15  
**Analysis Scope:** CoreProject VB.NET Legacy Codebase  
**Status:** Complete - No matching HomeRecoleccion/HomeOP dashboard classes found in legacy
