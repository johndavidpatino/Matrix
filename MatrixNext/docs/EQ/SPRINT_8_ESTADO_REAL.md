# Sprint 8 - Estado Real vs Documentado

**Fecha**: 2026-01-14  
**Responsable**: Dev Team  
**Objetivo**: Resolver discrepancias entre SPRINT_8_KICKOFF.md (plan ideal) y código actual (estado real)

---

## 📊 ANÁLISIS: ¿QUÉ YA EXISTE?

### ✅ TAREAS COMPLETADAS (No duplicar)

#### 1. Modelos EF Core + Migrations (15h)
- **Estado**: ✅ COMPLETO
- **Ubicación**: `MatrixNext.Web/Models/EQ/`
- **Entidades existentes** (13 total):
  - `EqQuoteHeader.cs` - Propuesta/Cotización principal
  - `EqQuestionnaire.cs` - Datos de encuesta
  - `EqMethodology.cs` - Metodología
  - `EqSampleCity.cs` - Ciudades muestreo
  - `EqMystery.cs` - Visitas misterio
  - `EqStaffSL.cs` - Personal SL
  - `EqRateEstadistica.cs` - Tarifas estadística
  - `EqValorHoraOps.cs` - Valor hora OPS
  - `EqParamPrecio.cs` - Parámetros precio
  - `EqLocaciones.cs` - Locaciones
  - `EqCostInsumos.cs` - Costo insumos
  - `EqCostResult.cs` - Resultado de costos
  - Y +1 más
- **Relaciones**: ✅ Implementadas (1:N Header→Questionnaire, etc.)
- **Validaciones**: ✅ [Required], [Range], etc.
- **Migrations**: ✅ Ya existentes

#### 2. Datos Maestros (Seeds) (15h)
- **Estado**: ✅ COMPLETO
- **Ubicación**: `MatrixNext.Web/Infrastructure/Data/EqSeedData.cs`
- **Records registrados**:
  - 396 precios matriz
  - 12 horas scripting/procesamiento
  - 8 tarifas recursos L1-L8
  - 6 costos insumos NSE
  - 21 tarifas estadística
  - 16 locaciones ciudad
  - **Total**: 600+ records ya en BD
- **SeedController**: ✅ `EasyQuoteSeedController.cs` (Force, Clear, Seed All)

#### 3. Motor de Cálculos (26 fórmulas) (25h)
- **Estado**: ✅ COMPLETO
- **Ubicación**: `MatrixNext.Web/Areas/EQ/Services/Internal/QuoteCalculator.cs` (351 líneas)
- **Fórmulas implementadas**: 26/26 ✅
  - CAMPO: Parafiscales, Siembra, CATI, Online
  - MYSTERY: Tarifa + desplazamientos + tanqueos + alertas + edición + alquiler + compra
  - INSUMOS: Prueba, Blind/Etiquetado, Transporte, Envío, Refrigeración, Reprografía
  - STAFF/OPS: Scripting, Procesamiento, DataCleaning, TopLines, Harmoni, Graficación, ASCII, Estadística, Codificación, Siembra telefónica, Tablets, Staff SL, Viaticos
  - MÁRGENES: GM, PB+RMF, ProfTime, OP, %OP
- **Entrada**: EasyQuoteViewModel (DTO)
- **Salida**: EQSummary (objeto con todos los costos)

#### 4. Integración Motor ↔ BD (20h)
- **Estado**: ✅ COMPLETO
- **Ubicación**: 
  - Servicio: `MatrixNext.Web/Services/EQ/EasyCostService.cs` (322 líneas)
  - Adapter: `MatrixNext.Web/Services/EQ/Adapters/QuoteHeaderToViewModelAdapter.cs` (260 líneas)
- **Funcionalidad**:
  - `CalculateAsync(quoteHeaderId)`: Carga quote → Calcula costos → Persiste en EqCostResult
  - `SaveQuoteWithCostAsync(vm)`: Calcula + Guarda quote con costos (transactional)
  - `ToViewModel()`: Mapea Entity → ViewModel (necesario para motor)
  - `ToEntity()`: Mapea ViewModel → Entity (guardar en BD)
- **Métodos públicos**:
  ```csharp
  public EQSummary CalculateCost(EasyQuoteViewModel vm)
  public async Task<SaveQuoteResult> SaveQuoteWithCostAsync(EasyQuoteViewModel vm)
  public async Task<ApiResponse<EasyCostResultDto>> CalculateAsync(int quoteHeaderId)
  ```

#### 5. Controllers API + UI (20h)
- **Estado**: ✅ PARCIAL
- **Ubicación**: `MatrixNext.Web/Areas/EQ/Controllers/`
- **Controllers existentes**:
  - `EasyQuoteController.cs` - Index (GET), Guardar (POST JSON)
  - `EasyQuoteAdminController.cs` - Admin panel
  - `EasyQuoteSeedController.cs` - Seed/Clear data (desarrollo)
  - `MaestrasAdminController.cs` - Gestión maestros (CRUD)
- **Endpoints**:
  - GET `/EQ/EasyQuote/Index` - UI principal
  - POST `/EQ/EasyQuote/Guardar` - Guardar quote con cálculo
  - GET `/EQ/EasyQuoteSeed/Index` - Dashboard seeds
  - GET `/EQ/Maestras/Tabla/{tabla}` - Ver maestros

#### 6. Views (UI)
- **Estado**: ✅ PARCIAL
- **Ubicación**: `MatrixNext.Web/Areas/EQ/Views/`
- **Views existentes**:
  - `EasyQuote/Index.cshtml` - Formulario principal
  - `EasyQuoteAdmin/` - Vistas admin
  - `EasyQuoteSeed/Index.cshtml` - Dashboard seeds
  - `MaestrasAdmin/` - CRUD maestros
- **Scripts**: ✅ AJAX scripts para modales, cálculos en vivo

#### 7. Services de Lógica (15h)
- **Estado**: ✅ PARCIAL
- **Ubicación**: `MatrixNext.Web/Services/EQ/`
- **Servicios existentes**:
  - `EasyQuoteService.cs` - CRUD async con EF Core (CreateAsync, GetAsync, UpdateAsync, DeleteAsync, ListAsync)
  - `EasyCostService.cs` - Cálculos + persistencia (CalculateAsync, SaveQuoteWithCostAsync)
  - `EasyQuoteRetrievalService.cs` - Recuperación avanzada
  - `EqSeedService.cs` - Seed datos
  - `IEasServices.cs` - Interfaces

#### 8. DI + Program.cs
- **Estado**: ✅ COMPLETO
- **Registro en Program.cs**:
  ```csharp
  services.AddScoped<IEasyCostService, EasyCostService>();
  services.AddScoped<EasyQuoteService>();
  services.AddScoped<QuoteCalculator>();
  services.AddScoped<EqSeedService>();
  // ... más servicios
  ```

---

## ❌ LO QUE FALTA (Ejecutar en Sprint 8)

### 1. **Documentación + Análisis** (5h) 🟡 PARCIAL
- [ ] ✅ Existe: `ANALISIS_EASYQUOTE.md`, `FASE_3_ESTADO_ACTUAL.md`
- [ ] ❌ Falta: `ANALISIS_EASIQUOTE_MAPEO_SPS.md` - Mapeo SPs WebMatrix → MatrixNext
  - Necesario para conectar adapters legacy si es requerido
- [ ] ❌ Falta: Documentar todos los endpoints en postman/swagger

### 2. **Adapters Legacy** (10h) 🔴 CRÍTICO SI SE USA BD LEGACY
- [ ] ❌ Si BD legacy requiere SPs:
  - `EQ_SolicitudAdapter.cs` - Acceso SPs solicitudes
  - `EQ_CotizacionAdapter.cs` - Acceso SPs cotizaciones
- [ ] ❌ Si BD legacy tiene datos:
  - Crear migración de datos históricos
  - Crear mapper de modelos legacy → EF Core entities
- [ ] ✅ Estado actual: Usando EF Core directamente (NO legacy SPs aún)

### 3. **Endpoints REST Faltantes** (10h) 🟡 PARCIAL
- [ ] ❌ API de Solicitudes (CRUD)
  - GET `/api/eq/solicitudes` - Listar
  - GET `/api/eq/solicitudes/{id}` - Detalle
  - PUT `/api/eq/solicitudes/{id}` - Actualizar
  - DELETE `/api/eq/solicitudes/{id}` - Eliminar

- [ ] ❌ API de Cotizaciones Avanzado
  - POST `/api/eq/cotizaciones/calcular` - Pre-calcular
  - POST `/api/eq/cotizaciones/{id}/exportar-pdf` - Exportar PDF
  - GET `/api/eq/cotizaciones/{id}/historial` - Historial cambios

- [ ] ❌ API de Catálogos (lectura)
  - GET `/api/eq/maestros/componentes` - Componentes activos
  - GET `/api/eq/maestros/materiales` - Materiales activos

### 4. **UI Enhancements** (15h) 🟡 PARCIAL
- [ ] ❌ Historial de cambios en vista detalle
- [ ] ❌ Modal de comparación (quote vs versión anterior)
- [ ] ❌ Dashboard de indicadores (quotes creadas, actualizadas, pendientes)
- [ ] ❌ Exportación a PDF/Excel
- [ ] ❌ Timeline de estados (Borrador → Enviada → Aceptada → Facturada)

### 5. **Estados + Workflow** (10h) 🟡 PARCIAL
- [ ] ❌ Integración con WorkFlowStateTransitionService (Sprint 7)
  - Quote debe pasar por estados: Borrador → Revisada → Aprobada → Aceptada
  - Solo usuarios con rol "GestorEQ" pueden aprobar
- [ ] ❌ Crear EstadoSolicitud y EstadoCotizacion enums
- [ ] ❌ Agregar validaciones de transición de estado

### 6. **Reportes + Analítica** (15h) 🔴 CRÍTICO PARA SPRINT 10
- [ ] ❌ Crear `EQ_ReportesService.cs`
  - Quotes pendientes por cliente
  - Quotes vencidas
  - Margen promedio por metodología
  - Tiempo promedio de creación-aceptación
- [ ] ❌ Crear `Areas/EQ/Controllers/ReportesEqController.cs`
  - GET `/api/eq/reportes/dashboard` - KPIs principales
  - GET `/api/eq/reportes/quotes-por-cliente` - Desglose
  - GET `/api/eq/reportes/margenes-metodologia` - Análisis de rentabilidad

### 7. **Testing + Validation** (10h)
- [ ] ❌ Unit tests para QuoteCalculator (26 fórmulas)
- [ ] ❌ Integration tests para CalculateAsync
- [ ] ❌ UI tests para Index.cshtml (form validation)
- [ ] ❌ Validation de datos entrada (NaN, negativos, etc.)

---

## 🎯 RECOMENDACIÓN DE EJECUCIÓN

### Enfoque: **BUILD ON EXISTING** (No duplicar)

**Fase A: Quick Wins (10h)**
1. ✅ Crear API REST endpoints faltantes (Solicitudes CRUD, Cotizaciones avanzado)
2. ✅ Conectar WorkFlowStateTransitionService a EQ (estados + transiciones)
3. ✅ Documentar SPs si BD legacy es requerida (análisis/mapeo)

**Fase B: UI + Reporting (20h)**
4. ✅ Crear vistas para historial + comparación
5. ✅ Implementar EQ_ReportesService.cs + endpoints
6. ✅ Dashboard de indicadores EQ

**Fase C: Validación + Cierre (5h)**
7. ✅ Testing funcional de flujo completo
8. ✅ Git commit + documentación final

---

## 🚀 PRÓXIMAS ACCIONES

1. **HOY**: Ejecutar Fase A (10h) - Endpoints REST + States
2. **MAÑANA**: Ejecutar Fase B (20h) - UI + Reporting
3. **DÍA 3**: Fase C (5h) - Testing + Commit

**Total Sprint 8 Real**: 35h (no 120h como decía kickoff)
**Razón**: 85h ya completados en FASE 2+3

---

## 📋 CHECKLIST DE NO-DUPLICACIÓN

- ✅ Revisar EasyCostService antes de crear nuevos adapters
- ✅ Revisar QuoteCalculator antes de modificar fórmulas
- ✅ Revisar EasyQuoteService antes de crear CRUD
- ✅ Revisar Views/ antes de crear nuevas UI
- ✅ Revisar Services/EQ/ antes de crear servicios
- ✅ Revisar Areas/EQ/Controllers/ antes de crear controllers

**Estado**: LISTO PARA EJECUTAR SIN DUPLICAR
