# MIGRACION EASYQUOTE A MATRIXNEXT (AREA EQ)

**Objetivo**: Implementar el modulo EasyQuote con UX de grillas editables, calculo 1:1 con el Excel y administracion de parametros. Este documento es el plan operativo y backlog hasta completar 100%.

---

## 📊 RESUMEN EJECUTIVO - AUDITORÍA 2026-01-05

### Estado General
> ⚠️ **ADVERTENCIA**: Módulo **NO LISTO para producción**. Estructura sólida (~75-95%) pero calculadora incompleta (~40%) y seeds placeholders (~40%).

### Métricas de Completitud
| Componente | Completitud | Estado | Bloqueador |
|------------|-------------|--------|------------|
| **Modelos/ViewModels** | 95% | ✅ Muy bueno | Faltan ~8 propiedades |
| **Tablas BD Schema** | 85% | ⚠️ Bueno | Faltan 5 tablas maestras |
| **Seeds Maestros** | 70% sembrado<br>40% placeholders | ⚠️ Preocupante | 🔥 **Sin Excel real** |
| **UI Tabs/Grids** | 85% | ⚠️ Bueno | Faltan ~8 controles |
| **Calculadora** | 40% | ❌ **CRÍTICO** | Faltan ~26 fórmulas |
| **Testing Paridad** | 0% | ❌ **CRÍTICO** | Sin validación vs Excel |

### Bloqueadores Críticos
1. 🔥 **Sin Excel Real**: No tenemos archivo Ipsos EasyQuote 2025v2.xlsm para extraer seeds reales.
2. ❌ **Calculadora 40%**: Faltan 26 fórmulas críticas (parafiscales, CATI/Online lookup, mystery completo, insumos, transportes, márgenes).
3. ❌ **Testing 0%**: Sin validación de paridad vs Excel.

### Riesgo de Uso Actual
> **PÉRDIDA DE DINERO**: Si se usa en producción actualmente, las cotizaciones tendrán **costos incorrectos** que podrían causar pérdidas.

### Próximos Pasos Urgentes
1. 🔥 **Conseguir archivo Excel** Ipsos EasyQuote 2025v2.xlsm.
2. ⚠️ **Completar FASE 1** (~3 semanas): Seeds reales + 26 fórmulas faltantes + testing paridad.
3. ✅ **Solo entonces lanzar a producción**.

### Timeline hasta Producción
- **Pre-requisito**: 1-2 días (conseguir Excel).
- **FASE 1** (paridad 1:1): 3 semanas (15-18 días hábiles).
- **FASE 2** (refinamiento): 2 semanas.
- **Total**: ~5 semanas desde hoy.

---

## 1. Estructura de area (EQ)
- `Areas/EQ/Controllers/EasyQuoteController.cs` (captura/calculo)
- `Areas/EQ/Controllers/EasyQuoteAdminController.cs` (parametros/seeds UI)
- `Areas/EQ/Views/EasyQuote/` (`Index` con tabs/grids)
- `Areas/EQ/Views/EasyQuoteAdmin/` (`Parametros.cshtml` para tablas maestras)
- `Areas/EQ/Models/` (ViewModels/DTOs segun analisis)
- `Areas/EQ/Services/` (`EasyQuoteService`, `EasyQuoteAdminService`)
- `Areas/EQ/Services/Internal/` (`EasyQuoteAdapter`, `QuoteCalculator`)
- `Areas/EQ/Services/Masters/` (`EasyQuoteMasterService` cache maestros)

## 2. UX clave (grids editables)
- Tabs: Datos, Cuestionario, Muestra, Mystery, Staff, Resumen.
- Grid de muestra por ciudad/NSE con validacion de sumas.
- Grid de mystery/shoppers y staff SL con add-row.
- Resumen con GM/PB/OP y costos por rubro.
- Admin: tablas maestras con formulario de upsert basico (precios, valor hora OPS, insumos, envios) y lectura del resto de seeds.

## 3. Datos y SP
- Script BD: `MatrixNext/EQ_SCHEMA.sql` (maestras, operacion, TVP, seeds completos).
- SP: `EQ_Quote_Save`, `EQ_Quote_Get`.

## 4. Backlog / TODO (estado actualizado 2026-01-05)

### ✅ COMPLETADOS
- [x] Area EQ + rutas/menu.
- [x] Modelos y adapter Dapper/TVP (95% completo).
- [x] Estructura BD básica (85% tablas creadas).
- [x] Seeds iniciales (70% sembrado, aunque 40% placeholders).
- [x] UI EasyQuote con tabs/grids JS (datos, cuestionario, muestra, mystery, staff, resumen) con calcular/guardar y add-row inline (85% completo).
- [x] Admin: vista Parametros + upsert para precios, valor hora OPS, insumos, envios, locaciones, codificacion, mystery, costos unitarios, misc/envio param (divisor/tipologias), productividad por ciudad y base de datos.
- [x] Admin: import masiva CSV para precios base y valor hora OPS con versionado (eq_param_misc PRECIOS_VERSION/VALORHORA_VERSION) desde la vista de parametros.
- [x] Calculadora BÁSICA (campo F2F lookup, reclutamiento/incentivos por NSE, scripting/procesamiento/DC básicos, estadística lookup, staff SL básico, locaciones/envíos/base datos básicos, viaticos básicos, codificación escenario 1, mystery básico, GM/AOT básicos) - **~40% completitud**.

### 🔥 BLOQUEADORES CRÍTICOS (FASE 1 - URGENTE)
- [x] 🔥 **Conseguir archivo Excel** Ipsos EasyQuote 2025v2.xlsm para extracción seeds reales. ✅ **RESUELTO** (2026-01-05) - Archivos .xlsm y .xlsx disponibles en raíz + CSVs en carpeta CSV/.
- [ ] ❌ **Sprint 1.1-1.2**: Seeds reales + tablas faltantes (7-9 días).
  - [ ] Extraer matriz CATI (Parametros!80:104) → CSV.
  - [ ] Extraer matriz Online (Parametros!94:104) → CSV.
  - [ ] Extraer factores (Parametros!180:182, 207:210, 214:217, 226:229) → CSV.
  - [ ] Extraer tabla Horas completa con KEY → CSV.
  - [ ] Extraer precios F2F REALES (Parametros!B4:AI12) → CSV.
  - [ ] Confirmar refrigeración, base datos, codificación valores reales.
  - [ ] Crear tablas eq_param_cati, eq_param_online, eq_param_factores, eq_rate_horas.
  - [ ] Importar todos CSVs via SQL/Admin.
- [ ] ❌ **Sprint 1.3-1.6**: Fórmulas faltantes (17-21 días).
  - [ ] Parafiscales F2F + siembra/recolección factor.
  - [ ] Campo CATI lookup eq_param_cati.
  - [ ] Campo Online lookup eq_param_online.
  - [ ] Mystery completo (D73-D75: coord campo, asistencias, critica, desplazamientos, bonos).
  - [ ] Insumos prueba (precio * muestra * num_productos).
  - [ ] Blind/rotulación (precio * muestra * productos_por_resp * factor_etiquetado).
  - [ ] Transporte niños (15.000 especial si estudio_ninos).
  - [ ] Transporte bebidas (28.000 especial si categoria bebidas).
  - [ ] Envíos volumétrico ((largo*ancho*alto)/divisor vs peso_real).
  - [ ] Locaciones refrigeración (tarifa * factor_refrigeracion + costo_nevera).
  - [ ] Reprografía (paginas * costo_por_pagina).
  - [ ] Verificación con GM (costo_base * 1.2145).
  - [ ] Siembra telefónica (si aplica).
  - [ ] Tablets (si metodología requiere).
  - [ ] Toplines (tarifa * horas si flag).
  - [ ] Harmoni (tarifa * horas_lookup si flag).
  - [ ] Graficación (tarifa * horas_lookup si flag).
  - [ ] Codificación completa (selección escenario por #pregs/#regs).
  - [ ] Staff SL lookup KEY compuesta eq_rate_horas.
  - [ ] Viaticos diferenciados por ciudad (productividad específica).
  - [ ] PB+RMF (-AOT * 4.3%).
  - [ ] ProfTime (-staff_sl_total).
  - [ ] OP (GM + PB+RMF + ProfTime).
  - [ ] %OP ((OP/AOT)*100).
  - [ ] Resumen sin/con gross por rubro.
- [ ] ❌ **Sprint 1.7**: Campos UI faltantes (2-3 días).
  - [ ] Agregar Harmoni/Graficacion a EQQuestionnaire.
  - [ ] Crear clase EQLogistica (dias setup/campo, olas, apoyo, taxi, ninos, reprografía, viaticos, otros costos/incentivos, dimensiones).
  - [ ] Controles UI tabs Cuestionario y Logística.
- [ ] ❌ **Sprint 1.8**: Validaciones (2-3 días).
  - [ ] Validaciones Service (minutos 5-60, penetración válida, sumas NSE, etc.).
  - [ ] Validación client-side UI.
- [ ] ❌ **Sprint 1.9**: Testing paridad (3-4 días).
  - [ ] Caso 400 Bogotá F2F 20 min vs Excel (diferencia < 0.1%).
  - [ ] Caso CATI vs Excel.
  - [ ] Caso Online vs Excel.
  - [ ] Caso Mystery vs Excel.

### ⚠️ MEDIOS (FASE 2 - POST PARIDAD)
- [ ] Admin: versionado formal (vigente_desde/vigente_hasta) y UI para activar/desactivar versiones (2-3 días).
- [ ] Admin: carga CSV resto tablas maestras si aplica (1-2 días).
- [ ] UX polish: tooltips, validación visual sumas, mensajes claros, loading spinners (3-4 días).
- [ ] Export PDF/Excel cotización (plantilla Ipsos branded) (3-4 días).

### 📋 BAJOS (FASE 3 - POST-MVP)
- [ ] Clonar cotización.
- [ ] Histórico y comparación versiones.
- [ ] Dashboard aprobaciones (prob vs real).
- [ ] Reportes analytics (rentabilidad por metodología, forecast).
- [ ] Integración BI/PowerBI.
- [ ] Sugerencias ML.
- [ ] Notificaciones email.
- [ ] Workflow aprobación formal.

### 📝 DOCUMENTACIÓN
- [ ] Documentar mapping campo a campo final (post Sprint 1.9).
- [ ] Actualizar este doc con estado COMPLETADO (post FASE 1).
- [ ] Grabar demo con usuario Excel (post FASE 1).
- [ ] Training usuarios finales (pre-producción).

## 5. Notas de calculo recientes
- Locaciones: usa tarifa gross * dias_base (seed). Refrigeracion aplica `eq_param_misc.FACTOR_REFRIGERACION` y costo nevera (placeholder 1.15 y 970000).
- Envios: si envio_ciudades y peso >0, usa tipologia URBANO (1 ciudad) o NACIONAL (2+) desde `eq_envio_param`, divisor volumetrico seed (5000). Falta integrar dimensiones y tabla de tipologias especificas.
- Base de datos: costo lee `eq_cost_base_datos` (No requiere/Cliente/Comprar) seeds placeholder.
- Productividad: dias de campo calculados con `eq_productividad_ciudad` (encuestadores/productividad por ciudad).
- Viaticos: transportes PST encuestadores/supervisores * dias de campo calculados.
- Codificacion: si flag y preguntas abiertas >0, aplica `eq_codificacion_param` primer escenario; pendiente calibrar por #regs/#preguntas regs.

---

## 6. ESTADO ACTUAL VERIFICADO (2026-01-05)

### 6.1 Modelos - Estado Actual
**Archivo**: `Areas/EQ/Models/EasyQuoteViewModel.cs`

**✅ COMPLETADOS** (ya en código):
```
EQHeader:
- ✅ Nombre, GrupoObjetivo, Cliente
- ✅ ProbAprobacion (default "Alta")
- ✅ SL, MetodologiaSL, RecordDetail
- ✅ CategoriaProducto
- ✅ ValorProveedorExterno, ValorProveedorInternacional
- ✅ ValorGMU
- ✅ FechaAprobacionEstimada, FechaCampo

EQQuestionnaire:
- ✅ DuracionMin, PenetracionCodigo
- ✅ PregAbiertas, PregAbiertasMult
- ✅ TopLine, DataCleaning, ASCIIFlag
- ✅ ScriptReclutamiento, Scripting, ScriptingTipo
- ✅ Codificacion, Procesamiento, NumProcesamientos
- ✅ ProcesoEstadistico
- ✅ ClasePrueba, Refrigeracion, CompraProducto
- ✅ EtiquetadoTipo, Embalaje
- ✅ ProductosTestear, ProductosPorResp, PatinadoresCiudad
- ✅ Siembra

EQMethodology:
- ✅ MetodologiaRecoleccion
- ✅ Tecnica1, Tecnica2, Tecnica3
- ✅ BaseDatos, IncidenciaLabel, IncidenciaValor
- ✅ SobreMuestraPct, EnvioCiudades, PesoProductoGr

EQSampleCity:
- ✅ Ciudad, Activa, MuestraTotal
- ✅ NSE1-NSE6

EQMysteryVisit:
- ✅ TipoVisita, Complejidad, NumOlas
- ✅ Desplazamientos, Tanqueos, Alertas, Edicion
- ✅ AlquilerEquipos, CompraDispositivos

EQStaffSL:
- ✅ Nivel, HorasMinimas, HorasPresup, Tarifa

EQSummary:
- ✅ CostoCampo, CostoCalidad, Viaticos, Incentivos
- ✅ Insumos, StaffOps, StaffSL
- ✅ CompraProducto, Tablets
- ✅ DirectCostOps, GM, PB_RMF, ProfTime, OP, AOT, PorcOP
```

**❌ FALTANTES** (necesarios agregar):
```
EQLogistica (clase nueva):
- ❌ DiasSetup, DiasCampo (calculados)
- ❌ NumOlas
- ❌ ApoyoReclutamientoTipo
- ❌ TaxiParticipantes, EstudioNinos
- ❌ ReprografiaPaginas
- ❌ ViaticasCampo (override manual)
- ❌ OtrosIncentivos
- ❌ OtrosCostos
- ❌ DimensionLargoCm, DimensionAnchoCm, DimensionAltoCm (envíos volumétricos)

EQQuestionnaire (agregar):
- ❌ Harmoni (bool)
- ❌ Graficacion (bool)
```

**Conclusión Modelos**: ~95% completado. Faltan solo ~8 propiedades (harmoni, graficacion, dimensiones, viaticos, otros).

### 6.2 Tablas BD - Estado Actual
**Archivo**: `EQ_SCHEMA.sql`

**✅ CREADAS** (ya en schema):
```
- ✅ eq_param_penetracion
- ✅ eq_param_metodologia
- ✅ eq_param_precio (F2F base)
- ✅ eq_param_script_proc
- ✅ eq_valor_hora_ops
- ✅ eq_rate_estadistica
- ✅ eq_cost_insumos
- ✅ eq_locaciones
- ✅ eq_envio_tarifa
- ✅ eq_codificacion_param
- ✅ eq_tarifa_mystery
- ✅ eq_insumos_prueba
- ✅ eq_cost_unitario_ops
- ✅ eq_productividad_ciudad
- ✅ eq_param_misc
- ✅ eq_envio_param
- ✅ eq_cost_base_datos
```

**❌ FALTANTES** (necesarias crear):
```
- ❌ eq_param_cati (matriz CATI duracion vs penetracion)
- ❌ eq_param_online (matriz Online/Auto duracion vs penetracion)
- ❌ eq_param_factores (script tipo, clase prueba, apoyo, etiquetado, prob aprobacion)
- ❌ eq_rate_horas (tabla Horas completa con KEY compuesta SL|RecordDetail|MetodologiaSL)
- ❌ eq_param_versionado (vigente_desde/vigente_hasta formal)
```

**Conclusión BD**: ~85% completado. Faltan 5 tablas maestras para lookups específicos.

### 6.3 Seeds - Estado Actual
**Verificado en**: `EQ_SCHEMA.sql` líneas 200+

**✅ SEMBRADOS** (con datos placeholder o iniciales):
```
- ✅ eq_param_penetracion (6 rangos)
- ✅ eq_param_metodologia (F2F, CATI, ONLINE, etc.)
- ✅ eq_param_precio (matriz F2F 5-60 min x 6 penetraciones = ~56 registros placeholder)
- ✅ eq_param_script_proc (horas por duracion)
- ✅ eq_valor_hora_ops (niveles L3-L7)
- ✅ eq_cost_insumos (reclutamiento/obsequio por NSE)
- ✅ eq_locaciones (7 ciudades)
- ✅ eq_envio_tarifa (URBANO/NACIONAL)
- ✅ eq_codificacion_param (1 escenario)
- ✅ eq_tarifa_mystery (3 tipos)
- ✅ eq_productividad_ciudad (7 ciudades)
- ✅ eq_param_misc (FACTOR_REFRIGERACION, COSTO_NEVERA, DIVISOR_VOLUMETRICO, PRECIOS_VERSION, VALORHORA_VERSION)
- ✅ eq_envio_param (tipologias default)
- ✅ eq_cost_base_datos (No requiere/Cliente/Comprar)
```

**⚠️ PLACEHOLDER** (necesitan valores reales de Excel):
```
- ⚠️ eq_param_precio: valores actualmente placeholder, NECESITAN extracción real de Excel Parametros!B4:AI12
- ⚠️ eq_valor_hora_ops: validar contra Excel tabla "Valor Hora - Alternativas"
- ⚠️ eq_cost_base_datos: confirmar costos reales (actualmente 100/200/300 placeholder)
- ⚠️ eq_param_misc: FACTOR_REFRIGERACION=1.15 y COSTO_NEVERA=970000 confirmar vs Excel
- ⚠️ eq_codificacion_param: solo 1 escenario, necesita tabla completa de Excel "Codificacion"
- ⚠️ eq_rate_estadistica: verificar completitud vs Excel "Tarifario Estadistica*"
```

**❌ NO SEMBRADOS** (necesitan creación):
```
- ❌ eq_param_cati (extraer de Excel Parametros!80:104)
- ❌ eq_param_online (extraer de Excel Parametros!94:104)
- ❌ eq_param_factores (extraer factores de Excel Parametros!180:182, 207:210, 214:217, 226:229)
- ❌ eq_rate_horas (extraer de Excel hoja "Horas" con KEY compuesta)
```

**Conclusión Seeds**: ~70% sembrado pero ~40% son placeholders. Necesita extracción masiva de Excel.

### 6.4 UI - Estado Actual
**Archivo**: `Areas/EQ/Views/EasyQuote/Index.cshtml`

**✅ IMPLEMENTADOS**:
```
- ✅ Tab Datos (nombre, cliente, grupo, fechas, SL, metodologia, probabilidad, GMU)
- ✅ Tab Cuestionario (duración, penetración, preguntas, flags scripting/procesamiento/codificación)
- ✅ Tab Muestra (grid ciudades con NSE1-6, add-row, validación sumas)
- ✅ Tab Mystery (grid tipos visita con costos, add-row)
- ✅ Tab Staff (grid SL niveles con horas, add-row)
- ✅ Tab Resumen (costos por rubro, GM, PB+RMF, OP, %OP)
- ✅ Botón Calcular
- ✅ Botón Guardar
- ✅ Ajax para calcular/guardar
```

**❌ FALTANTES**:
```
- ❌ Controles para campos nuevos (harmoni, graficacion, dimensiones producto, viaticos override, otros costos, otros incentivos, reprografía, apoyo reclutamiento tipo)
- ❌ Tooltips explicativos en campos complejos
- ❌ Validación client-side visual de sumas
- ❌ Preview resumen mientras editas (opcional)
- ❌ Export PDF/Excel
```

**Conclusión UI**: ~85% completado. Faltan controles para ~8 campos nuevos y features UX.

### 6.5 Calculadora - Estado Actual
**Archivo**: `Areas/EQ/Services/Internal/QuoteCalculator.cs`

**✅ IMPLEMENTADOS** (fórmulas básicas):
```
- ✅ Campo F2F lookup matriz precio
- ✅ Reclutamiento por NSE
- ✅ Incentivos por NSE
- ✅ Scripting horas * tarifa
- ✅ Procesamiento horas * tarifa * num_procesamientos
- ✅ DataCleaning
- ✅ Estadística (lookup servicio)
- ✅ Staff SL (horas * tarifa por nivel)
- ✅ Locaciones por ciudad
- ✅ Envíos (peso, tipología URBANO/NACIONAL)
- ✅ Base de datos (lookup tipo)
- ✅ Viaticos (transporte encuestadores/supervisores)
- ✅ Codificación (escenario básico)
- ✅ Mystery (cálculo básico)
- ✅ Direct Cost OPS (con GM OPS 21.45%)
- ✅ GM básico
- ✅ AOT
```

**❌ FALTANTES** (fórmulas críticas):
```
- ❌ Parafiscales F2F (% sobre valor base)
- ❌ Siembra/recolección factor 1 o 2
- ❌ Campo CATI (lookup matriz dedicada eq_param_cati)
- ❌ Campo Online (lookup matriz dedicada eq_param_online)
- ❌ Mystery completo (coord campo, asistencias, critica, desplazamientos, bonos D73-D75)
- ❌ Insumos prueba (precio * muestra * num_productos)
- ❌ Blind/rotulación (precio * muestra * productos_por_resp * factor_etiquetado)
- ❌ Transporte niños (si estudio_ninos, 15.000 especial)
- ❌ Transporte bebidas (si categoria bebidas, 28.000 especial)
- ❌ Envíos volumétrico (con dimensiones largo/ancho/alto)
- ❌ Locaciones con refrigeración exacta (factor * costo_nevera)
- ❌ Verificación con GM 21.45%
- ❌ Siembra telefónica (si aplica)
- ❌ Tablets (si metodología requiere)
- ❌ Toplines (si flag top_line)
- ❌ Harmoni (si flag harmoni, horas lookup)
- ❌ Graficación (si flag graficacion, horas lookup)
- ❌ Reprografía (páginas * costo_por_pagina)
- ❌ Viaticos diferenciados por ciudad (productividad específica)
- ❌ Codificación completa (selección escenario por #pregs/#regs)
- ❌ Staff SL lookup con KEY compuesta (SL|RecordDetail|MetodologiaSL)
- ❌ PB+RMF = -AOT * 4.3%
- ❌ ProfTime = -staff_sl_total
- ❌ OP = GM + PB+RMF + ProfTime
- ❌ %OP = OP / AOT * 100
- ❌ Resumen sin/con gross por rubro
```

**Conclusión Calculadora**: ~40% completado. Faltan ~26 fórmulas críticas para paridad 1:1.

---

## 7. AUDITORIA DE PARIDAD (2026-01-05)

### 7.1 Resumen Ejecutivo
**Estado General**: Implementación base sólida (~75% estructura, ~40% cálculos), **CRÍTICO** afinar fórmulas y seeds reales para paridad 1:1 con Excel.

**Métricas de Completitud**:
- Modelos/ViewModels: **95%** ✅
- Tablas BD Schema: **85%** ⚠️
- Seeds Maestros: **70%** sembrado, **40%** placeholders ⚠️
- UI Tabs/Grids: **85%** ⚠️
- Calculadora Fórmulas: **40%** ❌ (26 fórmulas faltantes)

**Conclusión**: La estructura está muy bien (~75-95%) pero la **CALCULADORA** está incompleta (~40%). Faltan ~26 fórmulas críticas y seeds reales para alcanzar paridad 1:1.

**Riesgo**: Si se usa actualmente, **los costos estarán incorrectos** y podría haber pérdida de dinero.

### 7.2 Análisis de Gaps por Categoría

#### 🔴 GAPS CRÍTICOS (bloquean paridad 1:1)

##### 7.2.1 Campos de Captura - REVISADO
**Análisis**: ANALISIS_EASYQUOTE §3 documenta 80+ campos de captura.

**Estado Actual**: **95% completado** ✅

**Faltantes (solo 8 propiedades)**:
```
EQQuestionnaire:
□ Harmoni (bool) - flag para activar cálculo Harmoni D127
□ Graficacion (bool) - flag para activar cálculo Graficación D128

EQLogistica (clase nueva a crear):
□ DiasSetup (int, default 2)
□ DiasCampo (int, calculado o manual override)
□ NumOlas (int, default 1)
□ ApoyoReclutamientoTipo (string enum)
□ TaxiParticipantes (bool)
□ EstudioNinos (bool)
□ ReprografiaPaginas (int)
□ ViaticasCampo (decimal, opcional override)
□ OtrosIncentivos (decimal)
□ OtrosCostos (decimal)
□ DimensionLargoCm (decimal?)
□ DimensionAnchoCm (decimal?)
□ DimensionAltoCm (decimal?)
```

**Impacto**: BAJO ahora (solo 8 campos vs 25 inicialmente estimados). **Prioridad: MEDIA** (no bloquea MVP, pero necesario para completitud).

**Acción**:
1. Agregar propiedades Harmoni/Graficacion a EQQuestionnaire.
2. Crear clase EQLogistica con propiedades listadas.
3. Agregar controles UI en tabs Cuestionario y Logística.
4. Conectar en Adapter para guardar/leer.

##### 7.2.2 Motor de Cálculo - CRÍTICO
**Análisis**: ANALISIS_EASYQUOTE §5 documenta 40+ fórmulas.

**Estado Actual**: **40% completado** ❌

**Faltantes (26 fórmulas críticas)**:
```
CAMPO:
□ Parafiscales F2F: valor_base * (1 + pct_parafiscales)
□ Siembra/recolección: factor 1 o 2 multiplicando campo F2F
□ Campo CATI: lookup matriz eq_param_cati (NO existe tabla aún)
□ Campo Online: lookup matriz eq_param_online (NO existe tabla aún)

MYSTERY:
□ Mystery completo: coord campo + asistencias + critica + desplazamientos + bonos (D73-D75)

INSUMOS/LOGÍSTICA:
□ Insumos prueba: precio_insumo * muestra_total * num_productos
□ Blind/rotulación: precio_rotulacion * muestra * productos_por_resp * factor_etiquetado
□ Transporte niños: tarifa especial 15.000 si estudio_ninos
□ Transporte bebidas: tarifa especial 28.000 si categoria bebidas alcohólicas
□ Envíos volumétrico: (largo*ancho*alto)/divisor vs peso_real, usar mayor
□ Locaciones refrigeración: tarifa * factor_refrigeracion + costo_nevera
□ Reprografía: paginas * costo_por_pagina

STAFF/OPS:
□ Verificación con GM: costo_base * (1 + 0.2145)
□ Siembra telefónica: cálculo si aplica (pendiente mapear)
□ Tablets: costo si metodología requiere (pendiente mapear lógica)
□ Toplines: tarifa_hora * horas_estimadas si flag top_line
□ Harmoni: tarifa_hora * horas_lookup_por_duracion si flag harmoni
□ Graficación: tarifa_hora * horas_lookup_por_duracion si flag graficacion
□ Codificación completa: selección escenario por #pregs/#regs (actualmente solo escenario 1)
□ Staff SL lookup: KEY compuesta "SL|RecordDetail|MetodologiaSL" (tabla NO existe)
□ Viaticos diferenciados: por ciudad con productividad específica

MÁRGENES:
□ PB+RMF: -AOT * 4.3%
□ ProfTime: -staff_sl_total
□ OP: GM + PB+RMF + ProfTime
□ %OP: (OP / AOT) * 100
□ Resumen sin/con gross: separar por rubro
```

**Impacto**: CRÍTICO ❌ - Sin estas fórmulas, los costos NO son confiables. **Prioridad: CRÍTICA**.

**Acción**:
1. **Sprint 1**: Implementar fórmulas de campo (parafiscales, siembra, CATI lookup, Online lookup) - 4 días.
2. **Sprint 2**: Implementar insumos/logística (prueba, blind, transportes, envíos volumétrico, locaciones, reprografía) - 5 días.
3. **Sprint 3**: Implementar staff/OPS (verificación GM, toplines, harmoni, graficación, codificación, staff SL lookup, viaticos) - 5 días.
4. **Sprint 4**: Implementar márgenes finales (PB+RMF, ProfTime, OP, %OP, resumen sin/con gross) - 2 días.
5. **Sprint 5**: Mystery completo - 2 días.

**Bloqueadores**:
- Campo CATI/Online requieren crear tablas eq_param_cati y eq_param_online primero.
- Staff SL lookup requiere crear tabla eq_rate_horas con KEY compuesta.
- Todos los cálculos requieren seeds reales (no placeholders).

##### 7.2.3 Tablas Maestras - CRÍTICO
**Análisis**: ANALISIS_EASYQUOTE §4 documenta 15+ tablas de soporte.

**Estado Actual**: **85% creadas**, **70% sembradas** pero **40% placeholders** ⚠️

**Tablas Faltantes (crear)**:
```
□ eq_param_cati (matriz CATI duracion vs penetracion)
□ eq_param_online (matriz Online/Auto duracion vs penetracion)
□ eq_param_factores (factores: script tipo, clase prueba, apoyo reclutamiento, etiquetado, probabilidad aprobación)
□ eq_rate_horas (tabla Horas completa con KEY compuesta SL|RecordDetail|MetodologiaSL)
```

**Seeds Placeholder a Reemplazar**:
```
⚠️ eq_param_precio: valores actuales son placeholder, NECESITAN extracción real de Excel Parametros!B4:AI12
⚠️ eq_valor_hora_ops: validar contra Excel "Valor Hora - Alternativas"
⚠️ eq_cost_base_datos: confirmar costos reales (actualmente 100/200/300 placeholder)
⚠️ eq_param_misc: FACTOR_REFRIGERACION=1.15 y COSTO_NEVERA=970000 confirmar vs Excel
⚠️ eq_codificacion_param: solo 1 escenario, necesita tabla completa de Excel "Codificacion"
⚠️ eq_rate_estadistica: verificar completitud vs Excel "Tarifario Estadistica*"
```

**Impacto**: CRÍTICO ❌ - Seeds incorrectos = cálculos incorrectos. **Prioridad: CRÍTICA**.

**Acción**:
1. **Día 1-2**: Abrir Excel, extraer matrices CATI (Parametros!80:104) y Online (Parametros!94:104) → CSV.
2. **Día 2-3**: Extraer factores (Parametros!180:182, 207:210, 214:217, 226:229) → CSV.
3. **Día 3-4**: Extraer tabla Horas completa con KEY → CSV.
4. **Día 4-5**: Extraer valores REALES de precios F2F (Parametros!B4:AI12) → CSV.
5. **Día 5**: Confirmar valores reales de refrigeración, base datos, codificación.
6. **Día 6**: Crear tablas SQL faltantes (eq_param_cati, eq_param_online, eq_param_factores, eq_rate_horas).
7. **Día 7**: Cargar todos los CSV via Admin import o script SQL INSERT.
8. **Día 8**: Validar datos sembrados con queries de verificación.

**Bloqueador**: Sin acceso al Excel real, imposible extraer seeds. Necesita archivo Ipsos EasyQuote 2025v2.xlsm.

#### 🟡 GAPS MEDIOS (impactan precisión)

##### 6.2.4 Validaciones y Reglas de Negocio
**Análisis**: ANALISIS_EASYQUOTE §7 documenta validaciones implícitas.

**Faltantes**:
```
□ Validar minutos dentro 5-60
□ Validar penetración dentro de catálogo
□ Validar suma NSE por ciudad = muestra_total ciudad
□ Validar suma ciudades = muestra_total global
□ Validar fechas: fecha_campo >= fecha_aprobacion_estimada
□ Validar metodología vs tecnica1/2/3 coherencia
□ Validar staff SL horas >= horas_minimas de tabla Horas
□ Validar peso producto si envio_ciudades=true
□ Validar # productos testear si clase_prueba != No aplica
□ Validar mystery tipos visita coherencia con num_olas
```

**Impacto**: Usuarios pueden ingresar datos inválidos. **Prioridad: MEDIA**.

**Acción**:
1. Implementar validaciones en Service antes de Save.
2. Retornar mensajes claros de error.
3. UI: validación client-side también.

##### 6.2.5 UX y Usabilidad
**Análisis**: UI actual tiene tabs básicos, falta polish.

**Faltantes**:
```
□ Tooltips explicativos en campos complejos (ej: penetración, clase de prueba)
□ Calculadora en vivo (sin necesidad de "Calcular") - opcional pero ideal
□ Exportar cotización a PDF/Excel
□ Comparar versiones de cotización (histórico)
□ Clonar cotización existente
□ Preview de resumen mientras editas tabs
□ Validación visual de sumas en grid muestra (highlight si no suma)
□ Sugerencias de valores según histórico (ML opcional)
```

**Impacto**: UX mejorable pero no bloquea funcionalidad. **Prioridad: BAJA**.

**Acción**:
1. Post-MVP, iterar UX con feedback usuarios.
2. Implementar export PDF/Excel primero.

#### 🟢 GAPS BAJOS (nice-to-have)

##### 6.2.6 Reportes y Analytics
**Análisis**: ANALISIS_EASYQUOTE menciona reportes en §5 pero no detalla.

**Faltantes**:
```
□ Reporte histórico de cotizaciones por cliente
□ Dashboard de probabilidad aprobación vs real
□ Análisis de rentabilidad por tipo metodología
□ Forecast de costos por mes/trimestre
□ Comparativa de cotizaciones similares
```

**Impacto**: Mejora gestión pero no bloquea cotización. **Prioridad: MUY BAJA**.

**Acción**:
1. Post-MVP v2 o v3.
2. Requerir BI/PowerBI integración.

### 6.3 Problemas de Paridad Específicos

#### Problema 1: Días de Campo - Fórmula Incompleta
**Excel**: `C57 = ROUNDUP(C58)` donde `C58 = total_encuestas / (productividad_por_dia * encuestadores_por_ciudad)`
**Actual**: Usa solo `eq_productividad_ciudad` sin considerar #encuestadores real por ciudad.
**Fix**: Agregar #encuestadores a seed o calcular dinámicamente.

#### Problema 2: Envíos Volumétrico - Dimensiones Faltantes
**Excel**: Usa peso volumétrico = `(largo * ancho * alto) / divisor_volumetrico` y compara con peso real.
**Actual**: Solo considera peso en gramos, no dimensiones.
**Fix**: Agregar campos dimensiones producto (largo/ancho/alto cm) y aplicar fórmula volumétrica.

#### Problema 3: Base de Datos - Costos Placeholder
**Excel**: Costos reales por tipo (No requiere=0, Cliente=valor_X, Comprar=valor_Y).
**Actual**: Seeds placeholder (100, 200, 300).
**Fix**: Extraer valores reales de Excel y actualizar seed.

#### Problema 4: Viaticos - Productividad Diferenciada
**Excel**: Usa productividad específica por ciudad para calcular días y viaticos.
**Actual**: Usa productividad general.
**Fix**: Ya tenemos `eq_productividad_ciudad`, aplicar correctamente en días y viaticos.

#### Problema 5: Refrigeración - Factor Exacto
**Excel**: Factor refrigeración placeholder (1.15) y costo nevera (970000).
**Actual**: Usa seed placeholder.
**Fix**: Confirmar valores reales de Excel D42 y actualizar.

#### Problema 6: Codificación - Fórmula por Preguntas/Registros
**Excel**: Codificación usa tabla compleja con #registros, #preguntas abiertas, #abiertas múltiples, días y horas.
**Actual**: Aplica solo primer escenario de `eq_codificacion_param`.
**Fix**: Implementar lógica completa que seleccione escenario según #pregs y #regs.

#### Problema 7: Staff SL - Lookup por KEY Compuesta
**Excel**: Horas mínimas lookup con `KEY = "<SL> | <RecordDetail> | <Metodologia SL>"`.
**Actual**: No implementado lookup, solo placeholders.
**Fix**: Implementar lookup exacto en Service con KEY compuesta.

#### Problema 8: Mystery/Shopper - Cálculos D73-D75
**Excel**: Usa totales de `MYSTERY!M39` o multiplicadores por días/productividad.
**Actual**: Cálculo básico, no considera todos los componentes (coord campo, asistencias, critica, desplazamientos, bonos).
**Fix**: Mapear exactamente fórmulas D73-D75 de Excel.

#### Problema 9: Parafiscales - Porcentaje No Documentado
**Excel**: Campo F2F incluye parafiscales pero porcentaje no está en ANALISIS.
**Actual**: No aplicado.
**Fix**: Buscar en Excel el % parafiscales (probablemente en `Parametros` o `Valores Insumos`).

#### Problema 10: Resumen sin/con Gross - Separación Faltante
**Excel**: D150-D154 separa costos sin gross y con gross por rubro.
**Actual**: Solo un total.
**Fix**: Agregar campos en resultado para diferenciar sin_gross y con_gross por cada rubro.

### 6.4 Mapeo de Implementación Faltante

#### Modelo de Datos - Campos a Agregar
**Tabla**: `eq_quote_header`
```sql
ALTER TABLE eq_quote_header ADD
    prob_aprobacion VARCHAR(20) DEFAULT 'Alta',
    categoria_producto VARCHAR(50),
    valor_gmu DECIMAL(18,2) DEFAULT 0,
    metodologia_sl VARCHAR(50),
    record_detail VARCHAR(50);
```

**Tabla**: `eq_questionnaire`
```sql
ALTER TABLE eq_questionnaire ADD
    tipo_script VARCHAR(30),
    num_procesamientos INT DEFAULT 1,
    clase_prueba VARCHAR(50),
    refrigeracion_anticipada BIT DEFAULT 0,
    compra_producto DECIMAL(18,2) DEFAULT 0,
    etiquetado_tipo VARCHAR(30),
    embalaje BIT DEFAULT 0,
    productos_testear INT DEFAULT 0,
    productos_por_resp INT DEFAULT 0,
    siembra_recoleccion BIT DEFAULT 0,
    harmoni BIT DEFAULT 0,
    graficacion BIT DEFAULT 0,
    otros_costos DECIMAL(18,2) DEFAULT 0;
```

**Tabla**: `eq_logistica`
```sql
ALTER TABLE eq_logistica ADD
    sobre_muestra_pct DECIMAL(5,2) DEFAULT 0,
    patinadores_por_ciudad INT DEFAULT 0,
    apoyo_reclutamiento_tipo VARCHAR(50),
    taxi_participantes BIT DEFAULT 0,
    estudio_ninos BIT DEFAULT 0,
    reprografia_paginas INT DEFAULT 0,
    viaticos_campo DECIMAL(18,2) DEFAULT 0,
    otros_incentivos DECIMAL(18,2) DEFAULT 0,
    dimension_largo_cm DECIMAL(10,2),
    dimension_ancho_cm DECIMAL(10,2),
    dimension_alto_cm DECIMAL(10,2);
```

**Tabla**: Nueva `eq_param_cati` (matriz CATI)
```sql
CREATE TABLE eq_param_cati (
    id INT IDENTITY PRIMARY KEY,
    duracion_min INT NOT NULL,
    penetracion_rango VARCHAR(20) NOT NULL,
    valor_perfil DECIMAL(18,2),
    valor_coord DECIMAL(18,2),
    valor_total DECIMAL(18,2),
    version INT DEFAULT 1,
    vigente_desde DATE,
    vigente_hasta DATE
);
```

**Tabla**: Nueva `eq_param_online` (matriz Online/Auto)
```sql
CREATE TABLE eq_param_online (
    id INT IDENTITY PRIMARY KEY,
    duracion_min INT NOT NULL,
    penetracion_rango VARCHAR(20) NOT NULL,
    valor_perfil DECIMAL(18,2),
    valor_coord DECIMAL(18,2),
    valor_total DECIMAL(18,2),
    version INT DEFAULT 1,
    vigente_desde DATE,
    vigente_hasta DATE
);
```

**Tabla**: Nueva `eq_param_factores` (script, clase prueba, apoyo, etiquetado, prob)
```sql
CREATE TABLE eq_param_factores (
    id INT IDENTITY PRIMARY KEY,
    tipo VARCHAR(50) NOT NULL, -- 'SCRIPT_TIPO', 'CLASE_PRUEBA', 'APOYO_RECLUTAMIENTO', 'ETIQUETADO', 'PROB_APROBACION'
    codigo VARCHAR(50) NOT NULL,
    descripcion VARCHAR(200),
    factor DECIMAL(10,4) DEFAULT 1.0,
    orden INT,
    activo BIT DEFAULT 1
);
```

**Tabla**: Extender `eq_rate_horas` (tabla Horas completa)
```sql
CREATE TABLE eq_rate_horas (
    id INT IDENTITY PRIMARY KEY,
    sl VARCHAR(50) NOT NULL,
    record_detail VARCHAR(50) NOT NULL,
    metodologia_sl VARCHAR(50) NOT NULL,
    horas_l3 DECIMAL(10,2),
    horas_l4 DECIMAL(10,2),
    horas_l5 DECIMAL(10,2),
    horas_l6 DECIMAL(10,2),
    horas_l7 DECIMAL(10,2),
    loaded_rate_l3 DECIMAL(18,2),
    loaded_rate_l4 DECIMAL(18,2),
    loaded_rate_l5 DECIMAL(18,2),
    loaded_rate_l6 DECIMAL(18,2),
    loaded_rate_l7 DECIMAL(18,2),
    billing_rate_l3 DECIMAL(18,2),
    billing_rate_l4 DECIMAL(18,2),
    billing_rate_l5 DECIMAL(18,2),
    billing_rate_l6 DECIMAL(18,2),
    billing_rate_l7 DECIMAL(18,2),
    KEY AS (sl + '|' + record_detail + '|' + metodologia_sl) PERSISTED,
    UNIQUE (sl, record_detail, metodologia_sl)
);
```

#### ViewModels - Propiedades a Agregar
**EasyQuoteViewModel**:
```csharp
// Agregar a clase existente
public string ProbAprobacion { get; set; } = "Alta";
public string CategoriaProducto { get; set; }
public decimal ValorGMU { get; set; }
public string MetodologiaSL { get; set; }
public string RecordDetail { get; set; }
public string TipoScript { get; set; }
public int NumProcesamientos { get; set; } = 1;
public string ClasePrueba { get; set; }
public bool RefrigeracionAnticipada { get; set; }
public decimal CompraProducto { get; set; }
public string EtiquetadoTipo { get; set; }
public bool Embalaje { get; set; }
public int ProductosTestear { get; set; }
public int ProductosPorResp { get; set; }
public bool SiembraRecoleccion { get; set; }
public bool Harmoni { get; set; }
public bool Graficacion { get; set; }
public decimal OtrosCostos { get; set; }
public decimal SobreMuestraPct { get; set; }
public int PatinadoresPorCiudad { get; set; }
public string ApoyoReclutamientoTipo { get; set; }
public bool TaxiParticipantes { get; set; }
public bool EstudioNinos { get; set; }
public int ReprografiaPaginas { get; set; }
public decimal ViaticasCampo { get; set; }
public decimal OtrosIncentivos { get; set; }
public decimal? DimensionLargoCm { get; set; }
public decimal? DimensionAnchoCm { get; set; }
public decimal? DimensionAltoCm { get; set; }
```

#### UI - Controles a Agregar

**Tab Datos** (`Areas/EQ/Views/EasyQuote/Index.cshtml`):
```html
<!-- Agregar después de campos existentes -->
<div class="mb-3">
    <label class="form-label">Probabilidad Aprobación</label>
    <select id="probAprobacion" class="form-select">
        <option value="Alta">Alta</option>
        <option value="Media">Media</option>
        <option value="Baja">Baja</option>
    </select>
</div>
<div class="mb-3">
    <label class="form-label">Categoría/Producto</label>
    <input type="text" id="categoriaProducto" class="form-control">
</div>
<div class="mb-3">
    <label class="form-label">Valor GMU (internacional)</label>
    <input type="number" id="valorGMU" class="form-control" step="0.01">
</div>
<div class="mb-3">
    <label class="form-label">Metodología SL</label>
    <input type="text" id="metodologiaSL" class="form-control">
</div>
<div class="mb-3">
    <label class="form-label">Record Detail</label>
    <input type="text" id="recordDetail" class="form-control">
</div>
```

**Tab Cuestionario**:
```html
<!-- Agregar controles -->
<div class="mb-3">
    <label class="form-label">Tipo Script</label>
    <select id="tipoScript" class="form-select">
        <option value="Nuevo">Nuevo</option>
        <option value="Duplicado">Duplicado</option>
        <option value="Reutilizacion">Reutilización</option>
    </select>
</div>
<div class="mb-3">
    <label class="form-label"># Procesamientos</label>
    <input type="number" id="numProcesamientos" class="form-control" min="1" value="1">
</div>
<div class="mb-3">
    <label class="form-label">Clase de Prueba</label>
    <select id="clasePrueba" class="form-select">
        <!-- Cargar desde eq_param_factores tipo='CLASE_PRUEBA' -->
    </select>
</div>
<div class="form-check mb-3">
    <input class="form-check-input" type="checkbox" id="refrigeracionAnticipada">
    <label class="form-check-label">Refrigeración Anticipada</label>
</div>
<div class="mb-3">
    <label class="form-label">Compra de Producto</label>
    <input type="number" id="compraProducto" class="form-control" step="0.01">
</div>
<div class="mb-3">
    <label class="form-label">Etiquetado/Blind</label>
    <select id="etiquetadoTipo" class="form-select">
        <!-- Cargar desde eq_param_factores tipo='ETIQUETADO' -->
    </select>
</div>
<div class="form-check mb-3">
    <input class="form-check-input" type="checkbox" id="embalaje">
    <label class="form-check-label">Embalaje</label>
</div>
<div class="mb-3">
    <label class="form-label"># Productos a Testear</label>
    <input type="number" id="productosTestear" class="form-control" min="0">
</div>
<div class="mb-3">
    <label class="form-label">Productos por Respondiente</label>
    <input type="number" id="productosPorResp" class="form-control" min="0">
</div>
<div class="form-check mb-3">
    <input class="form-check-input" type="checkbox" id="siembraRecoleccion">
    <label class="form-check-label">Siembra y Recolección</label>
</div>
<div class="form-check mb-3">
    <input class="form-check-input" type="checkbox" id="harmoni">
    <label class="form-check-label">Harmoni</label>
</div>
<div class="form-check mb-3">
    <input class="form-check-input" type="checkbox" id="graficacion">
    <label class="form-check-label">Graficación</label>
</div>
<div class="mb-3">
    <label class="form-label">Otros Costos</label>
    <input type="number" id="otrosCostos" class="form-control" step="0.01">
</div>
```

**Tab Logística**:
```html
<!-- Agregar controles -->
<div class="mb-3">
    <label class="form-label">Sobre-muestra (%)</label>
    <input type="number" id="sobreMuestraPct" class="form-control" step="0.01" min="0" max="100">
</div>
<div class="mb-3">
    <label class="form-label">Patinadores por Ciudad</label>
    <input type="number" id="patinadoresPorCiudad" class="form-control" min="0">
</div>
<div class="mb-3">
    <label class="form-label">Apoyo Reclutamiento en Sitio</label>
    <select id="apoyoReclutamientoTipo" class="form-select">
        <!-- Cargar desde eq_param_factores tipo='APOYO_RECLUTAMIENTO' -->
    </select>
</div>
<div class="form-check mb-3">
    <input class="form-check-input" type="checkbox" id="taxiParticipantes">
    <label class="form-check-label">Taxi Participantes</label>
</div>
<div class="form-check mb-3">
    <input class="form-check-input" type="checkbox" id="estudioNinos">
    <label class="form-check-label">Estudio con Niños</label>
</div>
<div class="mb-3">
    <label class="form-label">Reprografía (páginas)</label>
    <input type="number" id="reprografiaPaginas" class="form-control" min="0">
</div>
<div class="mb-3">
    <label class="form-label">Viaticos Campo</label>
    <input type="number" id="viaticasCampo" class="form-control" step="0.01">
</div>
<div class="mb-3">
    <label class="form-label">Otros Incentivos</label>
    <input type="number" id="otrosIncentivos" class="form-control" step="0.01">
</div>
<h5 class="mt-4">Dimensiones Producto (para envíos volumétricos)</h5>
<div class="row">
    <div class="col-md-4 mb-3">
        <label class="form-label">Largo (cm)</label>
        <input type="number" id="dimensionLargoCm" class="form-control" step="0.1">
    </div>
    <div class="col-md-4 mb-3">
        <label class="form-label">Ancho (cm)</label>
        <input type="number" id="dimensionAnchoCm" class="form-control" step="0.1">
    </div>
    <div class="col-md-4 mb-3">
        <label class="form-label">Alto (cm)</label>
        <input type="number" id="dimensionAltoCm" class="form-control" step="0.1">
    </div>
</div>
```

#### Calculadora - Fórmulas a Implementar

**QuoteCalculator.cs** - Agregar métodos:

```csharp
// Método: CalcularParafiscalesF2F
private decimal CalcularParafiscalesF2F(decimal valorBase, decimal pctParafiscales)
{
    return valorBase * (1 + pctParafiscales);
}

// Método: CalcularCampoF2FConSiembra
private decimal CalcularCampoF2FConSiembra(decimal valorEncuesta, int muestra, bool siembraRecoleccion)
{
    int factor = siembraRecoleccion ? 2 : 1;
    return valorEncuesta * muestra * factor;
}

// Método: LookupCATI (matriz dedicada)
private decimal LookupCATI(int duracionMin, string penetracionRango, List<ParamCATI> matrizCATI)
{
    var registro = matrizCATI.FirstOrDefault(m => 
        m.DuracionMin == duracionMin && 
        m.PenetracionRango == penetracionRango);
    
    return registro?.ValorTotal ?? 0;
}

// Método: LookupOnline (matriz dedicada)
private decimal LookupOnline(int duracionMin, string penetracionRango, List<ParamOnline> matrizOnline)
{
    var registro = matrizOnline.FirstOrDefault(m => 
        m.DuracionMin == duracionMin && 
        m.PenetracionRango == penetracionRango);
    
    return registro?.ValorTotal ?? 0;
}

// Método: CalcularInsumoPrueba
private decimal CalcularInsumoPrueba(decimal precioInsumo, int muestraTotal, int numProductos)
{
    return precioInsumo * muestraTotal * numProductos;
}

// Método: CalcularBlindRotulacion
private decimal CalcularBlindRotulacion(decimal precioRotulacion, int muestra, int productosPorResp, decimal factorEtiquetado)
{
    return precioRotulacion * muestra * productosPorResp * factorEtiquetado;
}

// Método: CalcularTransporteNinos
private decimal CalcularTransporteNinos(bool estudioNinos, int diasCampo, int ciudades, decimal tarifaNinos = 15000)
{
    return estudioNinos ? (tarifaNinos * diasCampo * ciudades) : 0;
}

// Método: CalcularTransporteBebidas
private decimal CalcularTransporteBebidas(bool esBebidas, int diasCampo, int ciudades, decimal tarifaBebidas = 28000)
{
    return esBebidas ? (tarifaBebidas * diasCampo * ciudades) : 0;
}

// Método: CalcularEnviosVolumetrico
private decimal CalcularEnviosVolumetrico(decimal pesoGr, decimal? largo, decimal? ancho, decimal? alto, 
    int numCiudades, decimal divisorVolumetrico, List<EnvioParam> tarifas)
{
    // Peso real en kg
    decimal pesoRealKg = pesoGr / 1000m;
    
    // Peso volumétrico en kg (si dimensiones provistas)
    decimal pesoVolumetricoKg = 0;
    if (largo.HasValue && ancho.HasValue && alto.HasValue)
    {
        pesoVolumetricoKg = (largo.Value * ancho.Value * alto.Value) / divisorVolumetrico;
    }
    
    // Usar el mayor
    decimal pesoFacturableKg = Math.Max(pesoRealKg, pesoVolumetricoKg);
    
    // Tipología según ciudades
    string tipologia = numCiudades == 1 ? "URBANO" : "NACIONAL";
    var tarifa = tarifas.FirstOrDefault(t => t.Tipologia == tipologia);
    
    if (tarifa == null) return 0;
    
    // Primer kilo + adicionales
    decimal costo = tarifa.PrimerKilo;
    if (pesoFacturableKg > 1)
    {
        costo += (pesoFacturableKg - 1) * tarifa.KiloAdicional;
    }
    
    return costo * numCiudades;
}

// Método: CalcularLocacionesPorCiudad
private decimal CalcularLocacionesPorCiudad(List<CiudadMuestra> ciudades, int diasSetup, int diasCampo, 
    bool refrigeracion, decimal factorRefrigeracion, decimal costoNevera, List<Locacion> tarifasLocacion)
{
    decimal total = 0;
    int diasTotales = diasSetup + diasCampo;
    
    foreach (var ciudad in ciudades.Where(c => c.Activa))
    {
        var tarifa = tarifasLocacion.FirstOrDefault(l => l.Ciudad == ciudad.Nombre);
        if (tarifa != null)
        {
            decimal costoBase = tarifa.TarifaGross * diasTotales;
            
            if (refrigeracion)
            {
                costoBase *= factorRefrigeracion;
                costoBase += costoNevera;
            }
            
            total += costoBase;
        }
    }
    
    return total;
}

// Método: CalcularVerificacionConGM
private decimal CalcularVerificacionConGM(decimal costoBase, decimal gmOps = 0.2145m)
{
    return costoBase * (1 + gmOps);
}

// Método: CalcularBaseDatos
private decimal CalcularBaseDatos(string tipoBD, List<CostBaseDatos> costos)
{
    var costo = costos.FirstOrDefault(c => c.Tipo == tipoBD);
    return costo?.Valor ?? 0;
}

// Método: CalcularToplines
private decimal CalcularToplines(bool flagTopline, decimal tarifaHora, decimal horasEstimadas)
{
    return flagTopline ? (tarifaHora * horasEstimadas) : 0;
}

// Método: CalcularHarmoni
private decimal CalcularHarmoni(bool flagHarmoni, int duracionMin, decimal tarifaHora, List<ParamScriptProc> tablaHoras)
{
    if (!flagHarmoni) return 0;
    
    var horas = tablaHoras.FirstOrDefault(h => h.DuracionMin == duracionMin)?.HorasHarmoni ?? 0;
    return tarifaHora * horas;
}

// Método: CalcularGraficacion
private decimal CalcularGraficacion(bool flagGraficacion, int duracionMin, decimal tarifaHora, List<ParamScriptProc> tablaHoras)
{
    if (!flagGraficacion) return 0;
    
    var horas = tablaHoras.FirstOrDefault(h => h.DuracionMin == duracionMin)?.HorasGraficacion ?? 0;
    return tarifaHora * horas;
}

// Método: CalcularReprografia
private decimal CalcularReprografia(int paginas, decimal costoPorPagina = 100)
{
    return paginas * costoPorPagina;
}

// Método: CalcularPBRMF
private decimal CalcularPBRMF(decimal aot, decimal factor = 0.043m)
{
    return -aot * factor;
}

// Método: CalcularProfTime
private decimal CalcularProfTime(decimal staffSLTotal)
{
    return -staffSLTotal;
}

// Método: CalcularOP
private decimal CalcularOP(decimal gm, decimal pbRmf, decimal profTime)
{
    return gm + pbRmf + profTime;
}

// Método: CalcularPctOP
private decimal CalcularPctOP(decimal op, decimal aot)
{
    return aot != 0 ? (op / aot) * 100 : 0;
}

// Método: LookupHorasMinimasSL
private Dictionary<string, decimal> LookupHorasMinimasSL(string sl, string recordDetail, string metodologiaSL, 
    List<RateHoras> tablaHoras)
{
    string key = $"{sl}|{recordDetail}|{metodologiaSL}";
    var registro = tablaHoras.FirstOrDefault(h => h.Key == key);
    
    if (registro == null)
        return new Dictionary<string, decimal>
        {
            ["L3"] = 0, ["L4"] = 0, ["L5"] = 0, ["L6"] = 0, ["L7"] = 0
        };
    
    return new Dictionary<string, decimal>
    {
        ["L3"] = registro.HorasL3,
        ["L4"] = registro.HorasL4,
        ["L5"] = registro.HorasL5,
        ["L6"] = registro.HorasL6,
        ["L7"] = registro.HorasL7
    };
}

// Método: CalcularCodificacionCompleta
private decimal CalcularCodificacionCompleta(bool flagCodificacion, int numPregsAbiertas, int numPregsAbiertasMult,
    int numRegistros, List<CodificacionParam> tablaParam)
{
    if (!flagCodificacion || numPregsAbiertas == 0) return 0;
    
    // Lógica: seleccionar escenario según #pregs y #regs
    // Placeholder: usar primer escenario
    var escenario = tablaParam.FirstOrDefault();
    
    if (escenario == null) return 0;
    
    // Calcular según fórmula Excel (pendiente mapear exactamente)
    // Por ahora, placeholder
    decimal horas = escenario.Horas * (numPregsAbiertas + numPregsAbiertasMult);
    decimal costo = horas * escenario.ValorIpsos;
    
    return costo;
}

// Método: SepararCostosSinConGross
private (ResumenRubros sinGross, ResumenRubros conGross) SepararCostosSinConGross(
    decimal campoSinGross, decimal campoConGross,
    decimal tddSinGross, decimal tddConGross,
    decimal incentivosSinGross, decimal incentivosConGross,
    decimal estadisticaSinGross, decimal estadisticaConGross)
{
    return (
        new ResumenRubros
        {
            Campo = campoSinGross,
            TDD = tddSinGross,
            Incentivos = incentivosSinGross,
            Estadistica = estadisticaSinGross
        },
        new ResumenRubros
        {
            Campo = campoConGross,
            TDD = tddConGross,
            Incentivos = incentivosConGross,
            Estadistica = estadisticaConGross
        }
    );
}
```

### 7.5 Plan de Acción REVISADO - Priorizado por Estado Actual

#### FASE 1: CRÍTICOS - Paridad 1:1 (2-3 semanas) ⚠️

**Objetivo**: Alcanzar paridad completa con Excel para caso base.

**Pre-requisito CRÍTICO**: Acceso al archivo Excel Ipsos EasyQuote 2025v2.xlsm para extracción de seeds reales.

**Sprint 1.1: Seeds Reales - PRIORIDAD MÁXIMA** (5-6 días) 🔥
```
□ Día 1: Abrir Excel, documentar ubicación exacta de cada seed (screenshots).
□ Día 2: Extraer matriz CATI (Parametros!80:104) → CSV (duracion x penetracion).
□ Día 3: Extraer matriz Online (Parametros!94:104) → CSV.
□ Día 4: Extraer factores (Parametros!180:182, 207:210, 214:217, 226:229) → CSV.
□ Día 5: Extraer tabla Horas completa con KEY "SL|RecordDetail|MetodologiaSL" → CSV.
□ Día 6: Extraer valores REALES de precios F2F (Parametros!B4:AI12) → CSV (reemplazar placeholders).
□ Validar: Confirmar refrigeración (D42), base datos (costos reales), codificación (tabla completa).

Entregable: CSVs con datos reales listos para importar.
```

**Sprint 1.2: Tablas BD Faltantes** (2-3 días)
```
□ Día 1: Crear eq_param_cati (estructura igual a eq_param_precio pero para CATI).
□ Día 1: Crear eq_param_online (estructura igual a eq_param_precio pero para Online).
□ Día 2: Crear eq_param_factores (tipo, codigo, descripcion, factor, orden).
□ Día 2: Crear eq_rate_horas (sl, record_detail, metodologia_sl, horas_l3-l7, rates, KEY compuesta).
□ Día 3: Importar todos los CSVs via SQL INSERT o Admin UI.
□ Día 3: Queries de verificación (COUNT, sample rows).

Entregable: BD con seeds reales completos.
```

**Sprint 1.3: Fórmulas Campo y Mystery** (4-5 días)
```
□ Día 1: Parafiscales F2F (buscar % en Excel) + siembra/recolección factor 1|2.
□ Día 2: Campo CATI lookup eq_param_cati + Campo Online lookup eq_param_online.
□ Día 3-4: Mystery completo (mapear D73-D75 con coord campo, asistencias, critica, desplazamientos, bonos).
□ Día 5: Testing campo F2F/CATI/Online y Mystery vs Excel.

Entregable: Cálculos de campo correctos.
```

**Sprint 1.4: Fórmulas Insumos/Logística** (5-6 días)
```
□ Día 1: Insumos prueba (precio * muestra * num_productos).
□ Día 1: Blind/rotulación (precio * muestra * productos_por_resp * factor_etiquetado).
□ Día 2: Transporte niños (15.000 si estudio_ninos) + transporte bebidas (28.000 si categoria bebidas).
□ Día 3: Envíos volumétrico ((largo*ancho*alto)/divisor vs peso_real, usar mayor).
□ Día 4: Locaciones refrigeración (tarifa * factor_refrigeracion + costo_nevera).
□ Día 5: Reprografía (paginas * costo_por_pagina).
□ Día 6: Testing insumos/logística vs Excel.

Entregable: Cálculos de insumos/logística correctos.
```

**Sprint 1.5: Fórmulas Staff/OPS** (5-6 días)
```
□ Día 1: Verificación con GM (costo_base * 1.2145).
□ Día 2: Toplines (tarifa * horas si flag) + Harmoni (lookup horas por duracion).
□ Día 3: Graficación (lookup horas por duracion) + codificación completa (selección escenario).
□ Día 4: Staff SL lookup KEY compuesta eq_rate_horas.
□ Día 5: Viaticos diferenciados por ciudad (productividad específica).
□ Día 6: Testing staff/OPS vs Excel.

Entregable: Cálculos de staff/OPS correctos.
```

**Sprint 1.6: Fórmulas Márgenes Finales** (3-4 días)
```
□ Día 1: PB+RMF (-AOT * 4.3%) + ProfTime (-staff_sl_total).
□ Día 2: OP (GM + PB+RMF + ProfTime) + %OP ((OP/AOT)*100).
□ Día 3: Resumen sin/con gross por rubro (separar campo, TDD, incentivos, estadística).
□ Día 4: Testing márgenes vs Excel.

Entregable: Cálculos de márgenes correctos.
```

**Sprint 1.7: Campos Faltantes UI** (2-3 días)
```
□ Día 1: Agregar Harmoni/Graficacion a EQQuestionnaire + controles UI tab Cuestionario.
□ Día 2: Crear clase EQLogistica + agregar a ViewModel + controles UI tab Logística.
□ Día 3: Conectar en Adapter Save/Load + testing captura.

Entregable: UI completa con todos los campos.
```

**Sprint 1.8: Validaciones** (2-3 días)
```
□ Día 1: Implementar validaciones Service (minutos 5-60, penetración válida, sumas NSE, etc.).
□ Día 2: Validación client-side UI (visual, mensajes claros).
□ Día 3: Testing validaciones.

Entregable: Validaciones robustas.
```

**Sprint 1.9: Testing Paridad FINAL** (3-4 días) ✅
```
□ Día 1: Caso 400 Bogotá F2F 20 min → comparar Excel vs MatrixNext línea por línea.
□ Día 2: Caso CATI → comparar.
□ Día 3: Caso Online → comparar.
□ Día 4: Caso Mystery → comparar.
□ Validación: Diferencia < 0.1% en TODOS los rubros.
□ Si falla, ajustar y re-testear.

Entregable: Paridad 1:1 certificada.
```

**Duración FASE 1**: ~3 semanas (15-18 días hábiles).

**Entregable FASE 1**: Cotizador funcional con paridad 1:1 Excel para F2F/CATI/Online/Mystery, seeds reales, validaciones completas.

---

#### FASE 2: MEDIOS - Refinamiento (1-2 semanas) ⚠️

**Objetivo**: Mejorar UX y robustez.

**Sprint 2.1: UX Polish** (3-4 días)
```
□ Día 1-2: Tooltips explicativos en campos complejos (penetración, clase prueba, etc.).
□ Día 2-3: Validación visual de sumas en grid muestra (highlight rojo si no suma).
□ Día 3: Mensajes de error claros y consistentes.
□ Día 4: Loading spinners en cálculos largos.

Entregable: UX mejorada.
```

**Sprint 2.2: Versionado Formal** (2-3 días)
```
□ Día 1: Implementar vigente_desde/vigente_hasta en maestras (ALTER TABLE).
□ Día 2: UI Admin para activar/desactivar versiones.
□ Día 3: Lookup por fecha de cotización (usar vigencias).

Entregable: Versionado formal funcional.
```

**Sprint 2.3: Export PDF/Excel** (3-4 días)
```
□ Día 1-2: Generar PDF con resumen cotización (plantilla Ipsos branded).
□ Día 2-3: Generar Excel con detalle completo (formato similar a original).
□ Día 4: Testing exports.

Entregable: Exports PDF/Excel funcionales.
```

**Duración FASE 2**: ~2 semanas (8-11 días hábiles).

**Entregable FASE 2**: Cotizador refinado con UX mejorada, versionado formal y exports.

---

#### FASE 3: BAJOS - Valor Agregado (post-MVP, backlog incremental) 📋

**Objetivo**: Features adicionales que no bloquean operación.

**Backlog Post-MVP** (no priorizado):
```
□ Clonar cotización existente (facilita variantes).
□ Histórico y comparación de versiones.
□ Dashboard de aprobaciones (probabilidad vs real).
□ Reportes y analytics (rentabilidad por metodología, forecast).
□ Integración BI/PowerBI.
□ Sugerencias ML (basadas en histórico).
□ Notificaciones email (cotización creada/aprobada).
□ Workflow aprobación formal (requiere módulo de aprobaciones).
```

**Duración FASE 3**: Incremental según demanda.

### 7.6 Criterios de Aceptación - Paridad 1:1 (ACTUALIZADO)

**Para considerar COMPLETADO, debe cumplir**:

1. **Cobertura de Campos**: 100% campos de ANALISIS_EASYQUOTE §3 capturados en UI.
   - **Actual**: 95% ✅ (faltan solo ~8 propiedades logística).
   - **Acción**: Sprint 1.7 (2-3 días).

2. **Cobertura de Fórmulas**: 100% fórmulas de ANALISIS_EASYQUOTE §5 implementadas.
   - **Actual**: 40% ❌ (faltan ~26 fórmulas críticas).
   - **Acción**: Sprints 1.3, 1.4, 1.5, 1.6 (17-21 días).

3. **Seeds Completos**: 100% tablas maestras de §4 sembradas con datos REALES (no placeholders).
   - **Actual**: 70% sembrado pero 40% placeholders ⚠️.
   - **Acción**: Sprints 1.1, 1.2 (7-9 días).

4. **Validaciones**: Todas las validaciones de §7 implementadas.
   - **Actual**: 60% ⚠️ (validaciones básicas, faltan complejas).
   - **Acción**: Sprint 1.8 (2-3 días).

5. **Testing Paridad**:
   - ✅ Caso 400 Bogotá F2F 20 min: diferencia < 0.1% vs Excel.
   - ✅ Caso CATI: diferencia < 0.1% vs Excel.
   - ✅ Caso Online: diferencia < 0.1% vs Excel.
   - ✅ Caso Mystery: diferencia < 0.1% vs Excel.
   - **Actual**: 0% ❌ (no testeado aún).
   - **Acción**: Sprint 1.9 (3-4 días).

6. **Documentación**: Mapping campo a campo y supuestos documentados.
   - **Actual**: 80% ✅ (ANALISIS_EASYQUOTE detallado, falta mapping final).
   - **Acción**: Durante sprints y final Sprint 1.9.

7. **Code Review**: Código revisado y aprobado.
   - **Actual**: Pendiente ⏳.
   - **Acción**: Post FASE 1.

**Resumen Criterios**:
- ✅ Completos: 1 de 7 (Documentación parcial).
- ⚠️ Parciales: 3 de 7 (Campos 95%, Seeds 70%, Validaciones 60%).
- ❌ Faltantes: 3 de 7 (Fórmulas 40%, Testing 0%, Code Review 0%).

**Conclusión**: Necesita ~3 semanas enfocadas (FASE 1 completa) para alcanzar 7/7 criterios.

---

### 7.7 Riesgos Identificados (ACTUALIZADO)

| Riesgo | Probabilidad | Impacto | Mitigación | Estado |
|--------|--------------|---------|------------|---------|
| Fórmulas Excel no documentadas (parafiscales %) | Media | Alto | Reverse engineering con casos conocidos | ⏳ Activo |
| Seeds con valores placeholder incorrectos | **ALTA** | **CRÍTICO** | **Extraer valores reales ANTES de continuar** | 🔥 **BLOQUEADOR** |
| Lógica de negocio compleja no capturada | Media | Medio | Entrevistas con usuarios Excel actuales | ⏳ Activo |
| Performance cálculo con muchas ciudades/NSE | Baja | Medio | Optimizar queries, cache de maestros | ✅ OK (ya cache maestros) |
| Cambios en Excel mientras migramos | Media | Alto | Versionado formal y fecha snapshot Excel | ⏳ Activo |
| Testing insuficiente (solo caso base) | **ALTA** | **ALTO** | **Suite de 10+ casos variados obligatoria** | ❌ **Pendiente** |
| **Sin acceso al Excel real** | **ALTA** | **CRÍTICO** | **Conseguir archivo Ipsos EasyQuote 2025v2.xlsm** | 🔥 **BLOQUEADOR CRÍTICO** |

**Nuevos Riesgos Detectados**:
- 🔥 **Bloqueador**: Sin Excel real, imposible extraer seeds reales → cálculos incorrectos.
- ⚠️ **Alto**: Falta ~60% de fórmulas → riesgo de uso prematuro con costos incorrectos.
- ⚠️ **Alto**: Testing 0% → bugs no detectados hasta producción.

**Acciones Inmediatas**:
1. 🔥 **CRÍTICO**: Conseguir acceso al archivo Excel Ipsos EasyQuote 2025v2.xlsm.
2. ⚠️ **URGENTE**: NO lanzar a producción hasta completar FASE 1 (paridad 1:1).
3. ⚠️ **URGENTE**: Advertir a usuarios que sistema actual tiene cálculos incompletos (~40%).

---

### 7.8 Supuestos Críticos a Validar (ACTUALIZADO)

**ANTES de continuar implementación, confirmar CON EXCEL REAL**:

1. ✅ **Parafiscales F2F**: ¿Cuál es el % exacto? → Buscar en Excel `Parametros` o `Valores Insumos reclutamiento`.
2. ⚠️ **Refrigeración**: ¿Factor real es 1.15 y costo nevera 970000? → Validar en Excel D42 (actualmente placeholder).
3. ⚠️ **Base de datos**: ¿Costos reales por tipo? → Extraer de Excel, no usar placeholder 100/200/300.
4. ✅ **Codificación**: ¿Lógica exacta de selección de escenario por #pregs/#regs? → Mapear tabla `Codificacion` completa.
5. ✅ **Mystery D73-D75**: ¿Fórmulas exactas? → Documentar en pseudocódigo antes de implementar.
6. ✅ **Staff SL KEY**: ¿Formato exacto del KEY en tabla Horas? → Validar separador "|" y orden.
7. ⚠️ **Versionado**: ¿Qué fecha usar para lookup? → Fecha de cotización o fecha de aprobación (definir).
8. ✅ **Dimensiones volumétrico**: ¿En qué unidades se capturan en Excel? → Confirmar cm (actualmente asumido).
9. 🔥 **Matriz CATI**: ¿Ubicación exacta en Excel? → Parametros!80:104 (según ANALISIS).
10. 🔥 **Matriz Online**: ¿Ubicación exacta en Excel? → Parametros!94:104 (según ANALISIS).
11. 🔥 **Factores**: ¿Ubicación exacta en Excel? → Parametros!180:182, 207:210, 214:217, 226:229 (según ANALISIS).
12. 🔥 **Tabla Horas**: ¿KEY exacto y estructura? → Validar formato "SL|RecordDetail|MetodologiaSL" y columnas.

**Supuestos Validados**:
- ✅ Penetración: rangos MAS82, 75-82, 67-74, 55-66, 46-54, 37-45 (confirmado en ANALISIS §4).
- ✅ Duración: 5-60 minutos (confirmado en ANALISIS §3).
- ✅ Metodologías: F2F=1, CATI=2, ONLINE=10, AUTO=4, MYSTERY=5, SHOPPER=8/9 (confirmado en ANALISIS §7).

**Supuestos Pendientes Validar** (🔥 requieren Excel):
- 🔥 Parafiscales %
- 🔥 Refrigeración factor + costo nevera
- 🔥 Base datos costos reales
- 🔥 Todas las matrices CATI/Online/Factores/Horas

---

### 7.9 Bloqueadores Actuales (NUEVO)

**🔥 BLOQUEADOR CRÍTICO 1: Sin Excel Real**
- **Descripción**: No tenemos acceso al archivo Ipsos EasyQuote 2025v2.xlsm para extraer seeds reales.
- **Impacto**: Imposible completar Sprint 1.1 (seeds reales) → imposible paridad 1:1.
- **Solución**: Conseguir archivo Excel de usuario/cliente.
- **Tiempo perdido si no se resuelve**: Bloqueado indefinidamente.

**⚠️ BLOQUEADOR ALTO 2: Fórmulas Incompletas**
- **Descripción**: 60% de fórmulas críticas faltantes (26 de ~40).
- **Impacto**: Si se usa actualmente, costos incorrectos → pérdida de dinero.
- **Solución**: Completar Sprints 1.3-1.6 (~17-21 días).
- **Advertencia**: **NO usar en producción hasta completar FASE 1**.

**⚠️ BLOQUEADOR MEDIO 3: Testing 0%**
- **Descripción**: No se ha testeado paridad vs Excel en ningún caso.
- **Impacto**: Bugs no detectados → problemas en producción.
- **Solución**: Completar Sprint 1.9 (~3-4 días) con suite de casos.
- **Advertencia**: Validación obligatoria antes de lanzamiento.

---

## 8. Conclusión y Próximos Pasos (ACTUALIZADO)

### 8.1 Resumen Ejecutivo Final

**Estado Actual Verificado**:
- ✅ **Estructura**: Muy sólida (~75-95% completitud en modelos, BD, UI).
- ⚠️ **Seeds**: 70% sembrado pero **40% placeholders** → datos incorrectos.
- ❌ **Calculadora**: Solo 40% implementada → **cálculos incorrectos**.
- ❌ **Testing**: 0% paridad verificada → **sin garantía de funcionamiento**.

**Conclusión**:
> El módulo EasyQuote tiene una base arquitectónica **excelente** (~75-95% estructura), pero la **CALCULADORA está incompleta** (~40%) y los **SEEDS son placeholders** (~40%). 
> 
> **RIESGO CRÍTICO**: Si se usa en producción actualmente, las cotizaciones tendrán **costos incorrectos** que podrían causar pérdida de dinero.

**Recomendación**:
> 🔥 **NO LANZAR A PRODUCCIÓN** hasta completar FASE 1 (paridad 1:1 verificada).
> 
> ⚠️ **BLOQUEADOR**: Necesita acceso al Excel real Ipsos EasyQuote 2025v2.xlsm para extraer seeds.

### 8.2 Próximos Pasos Inmediatos

**URGENTE** (esta semana):
1. 🔥 **Conseguir archivo Excel** Ipsos EasyQuote 2025v2.xlsm del usuario/cliente.
2. ⚠️ **Advertir a stakeholders**: Sistema actual tiene cálculos incompletos (~40%), NO usar en producción.
3. ⚠️ **Planificar FASE 1**: Asignar ~3 semanas (15-18 días hábiles) enfocadas.

**FASE 1** (semanas 1-3):
1. **Semana 1**: Sprints 1.1-1.2 (seeds reales + tablas faltantes) - 7-9 días.
2. **Semana 2**: Sprints 1.3-1.5 (fórmulas campo, insumos, staff) - 14-17 días acumulados.
3. **Semana 3**: Sprints 1.6-1.9 (fórmulas márgenes, campos UI, validaciones, testing) - 10-14 días.
4. **Validación**: Paridad 1:1 certificada con diferencia < 0.1% vs Excel en 4+ casos.

**Post FASE 1**:
1. Code review formal.
2. Demo con usuario Excel actual.
3. Deploy a ambiente de pruebas.
4. Training usuarios finales.
5. Lanzamiento producción controlado.

### 8.3 Timeline Realista Actualizado

| Fase | Duración | Inicio | Fin | Entregable |
|------|----------|--------|-----|------------|
| **Pre-requisito** | 1-2 días | 2026-01-06 | 2026-01-07 | Conseguir Excel real |
| **FASE 1** | 3 semanas | 2026-01-08 | 2026-01-29 | Paridad 1:1 certificada |
| **FASE 2** | 2 semanas | 2026-02-01 | 2026-02-15 | UX refinada + exports |
| **FASE 3** | Incremental | 2026-02-16+ | - | Features adicionales |

**Total hasta MVP producción**: ~5 semanas (incluyendo FASE 1 + FASE 2).

### 8.4 Recursos Necesarios

**Equipo**:
- 1 desarrollador full-time (backend + frontend).
- 1 QA/tester (FASE 1 final, Sprint 1.9).
- 1 usuario Excel experto (validación paridad).

**Herramientas**:
- ✅ VS Code / Visual Studio.
- ✅ SQL Server Management Studio.
- 🔥 **Archivo Excel** Ipsos EasyQuote 2025v2.xlsm (PENDIENTE).
- ✅ Git para versionado.

**Accesos**:
- ✅ Repositorio Git Matrix.
- ✅ Base de datos SQL Server.
- 🔥 **Archivo Excel** (BLOQUEADOR).

### 8.5 Criterio de Lanzamiento Producción

**NO lanzar hasta cumplir TODO**:
1. ✅ FASE 1 completada (9 sprints).
2. ✅ Paridad 1:1 certificada (diferencia < 0.1% en 4+ casos).
3. ✅ Code review aprobado.
4. ✅ Testing QA pasado.
5. ✅ Demo con usuario Excel exitosa.
6. ✅ Documentación completa (mapping campo a campo).
7. ✅ Training usuarios finales realizado.

**Actualmente cumplidos**: 0 de 7 ❌

---

## 9. Checklist Final de Refinación (ACTUALIZADO)

### Pre-Refinación
```
□ ✅ Leer esta auditoría completa.
□ 🔥 Conseguir archivo Excel Ipsos EasyQuote 2025v2.xlsm (BLOQUEADOR).
□ ⚠️ Validar supuestos §7.8 con Excel real.
□ ⚠️ Priorizar FASE 1 (críticos) primero - NO shortcuts.
□ ⚠️ Advertir stakeholders: sistema actual incompleto (~40% cálculos).
□ ✅ Crear branch Git para refinación (ej: feature/eq-paridad-1-1).
□ ✅ Planificar 3 semanas enfocadas sin interrupciones.
```

### Durante Refinación
```
□ Implementar sprints en orden estricto (1.1 → 1.2 → ... → 1.9).
□ NO saltar sprints (especialmente 1.1 seeds reales).
□ Testing incremental después de cada sprint.
□ Documentar cada fórmula con comentarios detallados.
□ Commit frecuente con mensajes descriptivos (ej: "feat(EQ): implement CATI lookup eq_param_cati").
□ Code review mini después de cada sprint (pair programming ideal).
□ Actualizar este doc con progreso (marcar sprints completados).
```

### Post-Refinación
```
□ Testing de paridad completo (§7.6 criterio 5).
□ Documentar mapping campo a campo final.
□ Actualizar MIGRACION_EQ_IMPLEMENTACION con estado COMPLETADO.
□ Demo con usuario Excel experto (grab screen recording).
□ Code review formal con otro dev.
□ Merge a develop/main con PR detallado.
□ Deploy a ambiente de pruebas.
□ Smoke testing en pruebas.
□ Training usuarios finales (session grabada).
□ Lanzamiento producción controlado (piloto con 1-2 usuarios).
□ Monitor primeras cotizaciones vs Excel (validación post-lanzamiento).
```

---

## 10. Advertencia Final

> ⚠️ **ADVERTENCIA CRÍTICA**:
> 
> El módulo EasyQuote MatrixNext **NO está listo para producción** en su estado actual (2026-01-05).
> 
> **Razón**: 
> - ❌ Calculadora solo 40% implementada (~26 fórmulas faltantes).
> - ❌ Seeds maestros 40% placeholders (datos incorrectos).
> - ❌ Testing paridad 0% (sin validación vs Excel).
> 
> **Riesgo**: 
> - Cotizaciones con **costos incorrectos**.
> - Potencial **pérdida de dinero** en propuestas.
> - **Daño reputacional** con clientes.
> 
> **Acción requerida**:
> 1. 🔥 Conseguir Excel real.
> 2. Completar FASE 1 (~3 semanas).
> 3. Validar paridad 1:1 (diferencia < 0.1%).
> 4. SOLO entonces lanzar a producción.
> 
> **NO hay shortcuts en paridad 1:1**. Es matemática exacta o nada.

---

**Documento actualizado**: 2026-01-05  
**Estado**: AUDITORÍA COMPLETA - LISTO PARA REFINACIÓN FASE 1  
**Próxima revisión**: Post-Sprint 1.1 (seeds reales cargados)

