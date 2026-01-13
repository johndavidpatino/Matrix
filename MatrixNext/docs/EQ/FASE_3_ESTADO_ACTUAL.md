# 🎯 FASE 3 - Estado Actual y Plan de Completitud

**Fecha**: 2026-01-12  
**Responsable**: Dev Team  
**Status**: ✅ ANÁLISIS COMPLETADO - LISTO PARA IMPLEMENTACIÓN

---

## 📊 ESTADO ACTUAL

### ✅ LO QUE YA EXISTE

#### 1. QuoteCalculator.cs - Motor de Cálculo (351 líneas)
**Ubicación**: `MatrixNext.Web\Areas\EQ\Services\Internal\QuoteCalculator.cs`

**Fórmulas Implementadas** (26/26):
```
CAMPO (4):
  1. Parafiscales F2F: factorParafiscal = 1.16522 si metodologia == "F2F"
  2. Siembra: factorSiembra = q.Siembra ? 2m : 1m
  3. CATI lookup: _masters.GetPrecioEncuesta("CATI", penetracion, duracion)
  4. Online lookup: _masters.GetPrecioEncuesta("Online", penetracion, duracion)

MYSTERY (1):
  5. Mystery: baseTarifa + desplazamientos + tanqueos + alertas + edicion + alquiler + compra

INSUMOS (7):
  6. Prueba: factorClase * productosTestear (si ClasePrueba != "No aplica")
  7. Blind/Etiquetado: factorEtiq * productosTestear * productoPorResp
  8. Transporte Niños: 15000 * totalMuestra (si EstudioNinos)
  9. Transporte Bebidas: 28000 * totalMuestra (si TaxiParticipantes)
  10. Envío volumétrico: (L*A*A/5000) * ciudadesActivas
  11. Refrigeración: factorRef * costoLocaciones + costoNevera
  12. Reprografía: reprografiaPaginas * factor50 * totalMuestra

STAFF/OPS (9):
  13-17. Scripting, Procesamiento, DataCleaning, TopLines, Harmoni, Graficacion, ASCII, Estadistica (lookup tablas)
  18. Codificación: codValor * (pregAbiertas + pregAbiertasMult*1.5) * (muestra/100)
  19. Siembra telefónica: factorApoyo * totalMuestra
  20. Tablets: patinadoresCiudad * tarifaTablet * ciudadesActivas
  21. Staff SL: horasReal * tarifa (lookup table eq_rate_horas)
  22. Viaticos: calculados o override

MÁRGENES (5):
  23. GM (Gestión): directCost * 0.2145
  24. PB+RMF: -aot * 0.043
  25. ProfTime: -staffSl
  26. OP: gm + pb + profTime
  27. %OP: op/aot * 100

Detalle: ✅ TODAS IMPLEMENTADAS
```

#### 2. EasyCostService.cs - Orquestación EF Core
**Ubicación**: `MatrixNext.Web\Services\EQ\EasyCostService.cs`

**Status**: ⚠️ INCOMPLETO
```
- ✅ CalculateAsync(): Crea EqCostResult pero NO CALCULA (solo ceros)
- ✅ GetLastCalculationAsync(): Obtiene último cálculo (OK)
- ✅ ValidateQuoteAsync(): Valida datos de entrada (OK)
- ✅ Está registrado en DI (Program.cs)

PROBLEMA:
  Línea 59: "Los costos serán calculados por el motor en FASE 3"
  → Costos fijos en 0 (no se llama QuoteCalculator)
```

#### 3. Seeds de Maestros - Completado en FASE 2
**Ubicación**: `MatrixNext.Web\Infrastructure\Data\EqSeedData.cs`

**Status**: ✅ COMPLETO
```
- ✅ 396 precios (GetPreciosMatriz)
- ✅ 12 horas scripting/procesamiento (GetHorasScriptProceso)
- ✅ 8 tarifas recursos L1-L8 (GetTarifasRecursos)
- ✅ 6 costos insumos NSE (GetCostosInsumos)
- ✅ 21 tarifas estadística (GetRatesEstadistica)
- ✅ 16 locaciones ciudad (GetLocaciones)

Total: 600+ records en 6 maestras, listos para usar
```

---

## ❌ LO QUE FALTA

### 1. **Conectar QuoteCalculator → EasyCostService** 🔴 CRÍTICO
**Problema**:
```csharp
// Hoy: EasyCostService.CalculateAsync() NO llama al motor
var costResult = new EqCostResult
{
    CostoCampo = 0m,        // ❌ Hardcodeado en 0
    CostoCalidad = 0m,      // ❌ Hardcodeado en 0
    ...
};

// Debe ser:
var summary = _calculator.Calcular(quote.ToViewModel());
var costResult = new EqCostResult
{
    CostoCampo = summary.CostoCampo,     // ✅ Del motor
    CostoCalidad = summary.CostoCalidad, // ✅ Del motor
    ...
};
```

**Impacto**: Sistema calcula SIEMPRE valores en 0, cotizaciones no funcionales  
**Effort**: 1-2 horas

### 2. **Adapter Quote → ViewModel** 🔴 CRÍTICO
**Problema**:
```csharp
// EasyCostService recibe EqQuoteHeader (Entity)
// QuoteCalculator espera EasyQuoteViewModel

var quote = await _context.EqQuoteHeaders
    .Include(q => q.Questionnaires)
    .Include(q => q.Methodologies)
    ...
    .FirstOrDefaultAsync();

// Cómo convertir EqQuoteHeader → EasyQuoteViewModel?
// ❌ No existe adapter
```

**Impacto**: No se puede llamar Calcular() sin convertir  
**Effort**: 2-3 horas (requiere revisar mapeo de cada propiedad)

### 3. **EqSeedService → Falta integración en startup** 🟡 MEDIA
**Problema**:
```csharp
// EqSeedService creado en FASE 2 pero no se ejecuta automáticamente
// Solo existe en DI, nadie lo invoca

// Dónde y cuándo ejecutar el seed?
// ❌ No definido
```

**Opciones**:
- A) Endpoint manual POST /api/eqseed/run
- B) Migration que ejecuta automático en DB init
- C) Ambas (recomendado)

**Impacto**: Maestros no se siembran automáticamente  
**Effort**: 1-2 horas

### 4. **Testing Paridad vs Excel** 🔴 CRÍTICO (pero depende de 1-2)
**Problema**:
```
No podemos validar si fórmulas están bien sin:
  ✅ Motor conectado (falta #1)
  ✅ Viewmodel converter (falta #2)
  ✅ Datos reales (FASE 3 → necesita actualizaciones CSV/Excel)
```

**Impacto**: No sabemos si cálculos son correctos vs Excel  
**Effort**: 4-5 horas testing (depende completar 1-2)

### 5. **Documentación de Fórmulas** 🟡 MEDIA
**Problema**:
```
¿Dónde quedan documentadas las 26 fórmulas?
- ❌ Inline comments en QuoteCalculator insuficientes
- ❌ No hay mapeo línea Excel → línea código
- ❌ No hay validación de celda-a-celda

Personas futuras no van a entender de dónde vienen valores
```

**Impacto**: Mantenibilidad y debugging posterior  
**Effort**: 2-3 horas

---

## 🎯 PLAN DE COMPLETITUD (ORDENADO POR DEPENDENCIA)

### PASO 1: Adapter EqQuoteHeader ↔ EasyQuoteViewModel (2-3h)
**Criticidad**: 🔴 BLOQUEADOR  
**Archivos**:
```
- MatrixNext.Web\Services\EQ\Adapters\QuoteHeaderToViewModelAdapter.cs (NEW)
- MatrixNext.Web\Areas\EQ\Models\EasyQuoteViewModel.cs (actualizar si falta mapeo)
```

**Tareas**:
```
□ Crear adapter que convierte:
  EqQuoteHeader (Entity)
    + EqQuestionnaire (Include)
    + EqMethodology (Include)
    + EqSampleCity[] (Include)
    + EqMysteryVisit[] (Include)
    + EqStaffSL[] (Include)
    + EqLogistica? (si existe)
  ↓
  EasyQuoteViewModel (DTO para cálculos)
    + Header
    + Questionnaire
    + Methodology
    + SampleCities
    + MysteryVisits
    + StaffSL
    + Logistica

□ Validar que todas las propiedades tienen destino
□ Probar con datos de test
□ Documentar conversiones especiales (ej: enum→string)
```

**Testing**:
```csharp
var entity = await context.EqQuoteHeaders.Include(...).FirstOrDefaultAsync();
var vm = QuoteHeaderToViewModelAdapter.ToViewModel(entity);
Assert.NotNull(vm);
Assert.Equal(entity.Questionnaires.First().SiembraMin, vm.Questionnaire.SiembraMin);
```

---

### PASO 2: Conectar EasyCostService.CalculateAsync() (1-2h)
**Criticidad**: 🔴 BLOQUEADOR  
**Archivos**:
```
- MatrixNext.Web\Services\EQ\EasyCostService.cs (actualizar líneas 29-87)
```

**Cambio**:
```csharp
public async Task<ApiResponse<EasyCostResultDto>> CalculateAsync(int quoteHeaderId)
{
    var quote = await _context.EqQuoteHeaders
        .Include(q => q.Questionnaires)
        .Include(q => q.Methodologies)
        .Include(q => q.SampleCities)
        .Include(q => q.Mysteries)
        .Include(q => q.StaffSL)
        .FirstOrDefaultAsync(q => q.Id == quoteHeaderId);

    if (quote == null)
        return new ApiResponse<EasyCostResultDto> { Success = false, ... };

    // ✅ PASO 2: Aquí va el adapter y cálculo
    var vm = QuoteHeaderToViewModelAdapter.ToViewModel(quote);
    var summary = _calculator.Calcular(vm);  // ✅ Llama motor

    var costResult = new EqCostResult
    {
        QuoteHeaderId = quoteHeaderId,
        Moneda = "COP",
        FechaCalculo = DateTime.UtcNow,
        CostoCampo = summary.CostoCampo,         // ✅ Del motor
        CostoCalidad = summary.CostoCalidad,     // ✅ Del motor
        Viaticos = summary.Viaticos,             // ✅ Del motor
        Incentivos = summary.Incentivos,         // ✅ Del motor
        Insumos = summary.Insumos,               // ✅ Del motor
        StaffOps = summary.StaffOps,             // ✅ Del motor
        StaffSL = summary.StaffSL,               // ✅ Del motor
        CompraProducto = summary.CompraProducto, // ✅ Del motor
        Tablets = summary.Tablets,               // ✅ Del motor
        DirectCostOps = summary.DirectCostOps,   // ✅ Del motor
        GM = summary.GM,                         // ✅ Del motor
        PB_RMF = summary.PB_RMF,                 // ✅ Del motor
        ProfTime = summary.ProfTime,             // ✅ Del motor
        OP = summary.OP,                         // ✅ Del motor
        AOTTotal = summary.AOT,                  // ✅ Del motor
        PorcOP = summary.PorcOP                  // ✅ Del motor
    };

    _context.EqCostResults.Add(costResult);
    await _context.SaveChangesAsync();

    return new ApiResponse<EasyCostResultDto>
    {
        Success = true,
        Message = "Cálculo completado exitosamente",
        Data = MapToDto(costResult)
    };
}
```

**Testing**:
```csharp
var response = await service.CalculateAsync(testQuoteId);
Assert.True(response.Success);
Assert.NotEqual(0m, response.Data.CostoCampo);  // ✅ Ya no es 0
```

---

### PASO 3: Integrar EqSeedService en startup (1-2h)
**Criticidad**: 🟡 MEDIA (no bloquea calculos, pero bloquea datos)  
**Archivos**:
```
- MatrixNext.Web\Program.cs (actualizar DI)
- MatrixNext.Web\Areas\EQ\Services\Internal\EqSeedService.cs (usar)
```

**Opciones**:

**Opción A: Migration automática**
```csharp
// En Program.cs, después de BuildAsync:
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MatrixDbContext>();
    var seedService = scope.ServiceProvider.GetRequiredService<EqSeedService>();
    
    // Ejecutar seed automático
    await seedService.SeedAllMasterTablesAsync(force: false);
}
```

**Opción B: Endpoint manual** (ya existe)
```
POST /api/eqseed/run?force=false
```

**Recomendación**: Opción A + B (automático en dev, manual en prod para control)

---

### PASO 4: Unit Tests para EasyCostService (2-3h)
**Criticidad**: 🟡 MEDIA  
**Archivos**:
```
- MatrixNext.Tests.Unit\Services\EQ\EasyCostServiceTests.cs (NEW)
```

**Tests a crear**:
```csharp
[Fact]
public async Task CalculateAsync_WithValidQuote_ReturnsNonZeroCosts()
{
    // Arrange: Crear quote con datos de prueba
    var quoteId = CreateTestQuoteWithData();

    // Act
    var response = await service.CalculateAsync(quoteId);

    // Assert
    Assert.True(response.Success);
    Assert.NotNull(response.Data);
    Assert.NotEqual(0m, response.Data.CostoCampo);
    Assert.NotEqual(0m, response.Data.Incentivos);
    Assert.NotEqual(0m, response.Data.DirectCostOps);
}

[Fact]
public async Task CalculateAsync_WithQuoteNotFound_ReturnsFalse()
{
    var response = await service.CalculateAsync(999999);
    Assert.False(response.Success);
}

[Fact]
public async Task CalculateAsync_StoresToDatabase()
{
    var quoteId = CreateTestQuoteWithData();
    await service.CalculateAsync(quoteId);
    
    var stored = context.EqCostResults
        .FirstOrDefault(r => r.QuoteHeaderId == quoteId);
    
    Assert.NotNull(stored);
    Assert.NotEqual(0m, stored.DirectCostOps);
}

[Fact]
public async Task ValidateCostsAgainstSummary_ContainsFourDot()
{
    // Paridad F2F: 400 Bogotá, 20min, etc.
    var summary = new EQSummary { CostoCampo = 1234567m, ... };
    // Verificar que CostoCampo = específicamente 1234567
    // (This requires real Excel data to populate)
}
```

---

### PASO 5: Testing Manual Paridad vs Excel (4-5h)
**Criticidad**: 🔴 CRÍTICO (pero DEPENDE de Pasos 1-3)  
**Documentación**:
```
- MatrixNext\docs\EQ\TEST_PARIDAD_F2F_BASELINE.xlsx
- MatrixNext\docs\EQ\TEST_PARIDAD_CATI_BASELINE.xlsx
```

**Proceso**:
```
□ Caso 1: F2F 400 respondientes Bogotá, 20 min, 75-82%
  1. Calcular MANUALMENTE en Excel línea por línea
  2. Registrar cada valor (campo, insumos, staff, márgenes)
  3. Crear cotización en MatrixNext con mismos datos
  4. Presionar "Calcular"
  5. Comparar cada rubro: Excel vs MatrixNext
  6. Si diferencia > 0.1%, investigar y documentar
  7. Validar o ajustar fórmula

□ Caso 2: CATI 300 respondientes (3 ciudades), 15 min
  ... mismo proceso

□ Caso 3: Online 250 respondientes, 10 min
  ... mismo proceso

□ Caso 4: Mystery shopper 50 visitas, 3 olas
  ... mismo proceso
```

**Criterio Aceptación**:
```
✅ Diferencia < 0.1% en todos los rubros
✅ Documentado en hoja "Validación" con firma QA
✅ Todos los 4 casos PASSED
```

---

### PASO 6: Documentación de Fórmulas (2-3h)
**Criticidad**: 🟡 MEDIA  
**Archivos**:
```
- MatrixNext.Web\Areas\EQ\Services\Internal\QuoteCalculator.cs (inline comments)
- MatrixNext\docs\EQ\FORMULAS_DETALLE_CON_MAPEO_EXCEL.md (NEW)
```

**Contenido a documentar**:
```markdown
# Mapeo Fórmulas: Excel → QuoteCalculator.cs

## FÓRMULA 1: Parafiscales F2F
Excel: Parametros!D45 = IF(metodologia="F2F", 1.16522, 1)
Código: QuoteCalculator.cs:67 = factorParafiscal = ... ? 1.16522m : 1m
Celda Excel: B142 (costoCampo base) * D45 (factor)
Cálculo: valorEncuesta * totalMuestra * factorSiembra * factorParafiscal
Validación: ✅ PASS (factor 16.522% documentado)

## FÓRMULA 2: Siembra
Excel: Parametros!D50 = IF(siembra, 2, 1)
Código: QuoteCalculator.cs:69 = q.Siembra ? 2m : 1m
...

[similar para todas las 26]
```

---

## 📋 RESUMEN EJECUTIVO

### Trabajo Completado (FASE 2)
- ✅ 600+ seeds en 6 maestras (EqSeedData + EqSeedService)
- ✅ 26 fórmulas codificadas en QuoteCalculator.cs
- ✅ 8 tests validando seeds

### Trabajo Crítico Faltante (FASE 3)
1. **Adapter EqQuoteHeader ↔ ViewModel** (2-3h) 🔴 BLOQUEADOR
2. **Conectar EasyCostService** (1-2h) 🔴 BLOQUEADOR
3. **Integrar EqSeedService en startup** (1-2h) 🟡 MEDIO
4. **Tests EasyCostService** (2-3h) 🟡 MEDIO
5. **Testing paridad Excel** (4-5h) 🔴 CRÍTICO (depende 1-2)
6. **Documentación fórmulas** (2-3h) 🟡 MEDIO

**Total Effort**: 14-19 horas

### Timeline Realista
```
Hoy (Ene 12):    Pasos 1-4 (implementation)     ✅ 6-10h
Mañana (Ene 13): Paso 5 (testing manual)        ⏳ 4-5h  
Siguiente (Ene 14): Paso 6 + fixes              ✅ 2-3h
TOTAL: 1.5 días de desarrollo
```

### Next Action
🎯 **Empezar PASO 1**: Crear adapter QuoteHeaderToViewModelAdapter.cs
- Mapear cada propiedad EqQuoteHeader → EasyQuoteViewModel
- Validar nulls y conversiones
- Test básico de conversión

---

**Documento**: FASE_3_ESTADO_ACTUAL.md  
**Versión**: 1.0  
**Status**: 🟢 LISTO PARA IMPLEMENTACIÓN INMEDIATA
