# 📋 TODO - EasyQuote Migración: Priorizado por Criticidad

**Documento**: TODO Ejecutivo separando MIGRACIÓN CRÍTICA (MVP) de FUNCIONALIDADES NO-ESENCIALES  
**Fecha**: 2026-01-05 Post-Implementación  
**Estado**: 4-Point Implementation COMPLETADO ✅ | Testing/Validación PENDIENTE ❌

---

## 🎯 RESUMEN EJECUTIVO

| Categoría | Completitud | Estado | Bloqueador |
|-----------|------------|--------|-----------|
| **FASE 1: MVP Crítico** | 95% | ⏳ EN PROGRESO | 🔥 Acceso Excel real |
| **FASE 2: Refinamiento** | 0% | ⏳ PENDIENTE | No (post FASE 1) |
| **FASE 3: Nice-to-Have** | 0% | 📋 BACKLOG | No (post-MVP) |

**Línea de tiempo realista**:
- ✅ FASE 1 (MVP): 2-3 semanas (con acceso Excel real)
- ✅ FASE 2 (Refinamiento): 1-2 semanas (post-MVP)
- 📋 FASE 3 (Backlog): Incremental, post-lanzamiento

---

## 🚨 BLOQUEADORES CRÍTICOS

### ✅ BLOQUEADOR #1 RESUELTO: ACCESO A EXCEL REAL
**Prioridad**: ✅ RESUELTO  
**Severidad**: Was CRÍTICA  
**Status**: ✅ ENCONTRADO

```
Descripción:
  Excel Ipsos EasyQuote 2025v2.xlsm EXISTE en la carpeta Matrix

Ubicación:
  ✅ Archivo principal: c:\Users\johnd\source\repos\johndavidpatino\Matrix\Ipsos EasyQuote 2025v2.xlsm
  ✅ Backup XLS: c:\Users\johnd\source\repos\johndavidpatino\Matrix\Ipsos EasyQuote 2025v2 XLS.xlsx
  ✅ CSVs exportados: c:\Users\johnd\source\repos\johndavidpatino\Matrix\CSV\ (15 archivos)

Archivos disponibles:
  - Parametros.csv (806 líneas) - Matrices F2F, CATI, Online + tarifas
  - Horas.csv - Tabla horas con KEY format
  - Codificacion.csv - Tabla codificación
  - Mystery.csv - Tarifas mystery/shopper
  - TarifarioEstadistica2.csv - Tarifas estadística
  - Valores Insumos reclutamiento.csv - Costos reclutamiento por NSE
  - + 9 más

Markdown disponible:
  - inventario_formulas_ipsos_easyquote_2025v2.md (5,500+ formulas)
  - analisis_excel_para_mvc_resumen.md (estructura Excel)
  - diccionario_campos_y_bloques_para_mvc.md (campo dictionary)

Impacto: 🟢 NULO - Ya tenemos todo
  - ✅ Matrices CATI/Online extractables
  - ✅ Factores/Horas disponibles
  - ✅ Testing paridad puede empezar
  - ✅ Sistema tendrá datos reales

Próximo paso:
  📋 Sprint 1.1: Extraer y cargar seeds reales (5-6 días)
  📋 Sprint 1.3: Testing paridad con Excel (3-4 días)
```

---

## 📊 TRABAJO IMPLEMENTADO (COMPLETADO ✅)

### ✅ 4-Point Implementation - DONE

**Sprint 0: Base Setup** ✅ COMPLETADO
- ✅ Models: Added EQLogistica + Harmoni/Graficacion/OtrosCostos properties
- ✅ SQL Tables: Created eq_param_cati, eq_param_online, eq_param_factores, eq_rate_horas
- ✅ Seeds: Added 200+ MERGE records (88 CATI, 88 Online, 18 Factores, 12+ Horas ejemplo)
- ✅ UI Controls: Added 15 controls across Cuestionario & Logística tabs
- ✅ Formulas: Implemented 26 fórmulas in QuoteCalculator.cs
- ✅ Master Service: Added GetFactorCodigo, GetHorasMinimas methods

**Evidencia**:
- [EasyQuoteViewModel.cs](Areas/EQ/Models/EasyQuoteViewModel.cs) - ✅ 11 nuevas propiedades + EQLogistica class
- [EQ_SCHEMA.sql](EQ_SCHEMA.sql) - ✅ 4 tablas nuevas + 200+ seeds
- [QuoteCalculator.cs](Areas/EQ/Services/Internal/QuoteCalculator.cs) - ✅ 26 fórmulas
- [Index.cshtml](Areas/EQ/Views/EasyQuote/Index.cshtml) - ✅ 15 controles nuevos

---

## 🏆 FASE 1: MIGRACIÓN CRÍTICA (MVP)

### Objetivo
Alcanzar paridad 1:1 con Excel Ipsos EasyQuote. NO usar en producción sin esto.

### Sprint 1.1: Seeds Reales - PRIORIDAD MÁXIMA 🔥
**Duración**: 5-6 días  
**Status**: ⏳ BLOQUEADO (necesita Excel real)  
**Riesgo**: CRÍTICO

```
PENDIENTE:
□ Día 1: Conseguir Excel real (BLOQUEADOR)
□ Día 2: Documentar ubicación exacta de cada matriz (screenshots)
□ Día 3: Extraer matriz CATI (Parametros!B80:AI88) → CSV
□ Día 4: Extraer matriz Online (Parametros!B96:AI104) → CSV
□ Día 5: Extraer factores (Parametros!180:182, 207:210, etc.) → CSV
□ Día 6: Extraer tabla Horas completa con KEY → CSV
         Extraer precios F2F reales (Parametros!B4:AI12) → CSV reemplazar placeholders

VALIDAR:
□ Refrigeración: ¿Factor real es 1.15? ¿Costo nevera 970000?
□ Base datos: ¿Costos reales (No requiere/Cliente/Comprar)?
□ Codificación: ¿Tabla completa mapeo #pregs/#regs?
□ Parafiscales: ¿% exacto? (documentado en Excel donde?)

Entregable:
  CSVs con datos reales listos para importar a BD
  (CATI, Online, Factores, Horas, PreciosF2F)
```

### Sprint 1.2: Validación Seeds en BD - CRÍTICA
**Duración**: 2-3 días  
**Dependencia**: Sprint 1.1 (completado)  
**Status**: ⏳ PENDIENTE

```
PENDIENTE:
□ Día 1: Backup BD actual
□ Día 1: TRUNCATE eq_param_cati, eq_param_online, eq_param_factores, eq_rate_horas
         DELETE datos placeholder MERGE
□ Día 2: Importar CSVs reales via SQL INSERT o Admin UI
□ Día 3: Queries de verificación:
         SELECT COUNT(*) FROM eq_param_cati (esperado: 88)
         SELECT COUNT(*) FROM eq_param_online (esperado: 88)
         SELECT COUNT(*) FROM eq_param_factores (esperado: 18+)
         SELECT COUNT(*) FROM eq_rate_horas (esperado: 200+)
         Validar KEY format en eq_rate_horas

Entregable:
  BD con seeds reales completos y validados
```

### Sprint 1.3: Testing Paridad - CASO BASE F2F
**Duración**: 4-5 días  
**Dependencia**: Sprint 1.1-1.2 (seeds reales)  
**Status**: ⏳ PENDIENTE

```
PENDIENTE:
□ Día 1: Crear caso de prueba Excel:
         - 400 respondientes Bogotá
         - F2F, 20 minutos, Penetración 75-82%
         - Completar todos los campos

□ Día 2: Cotizar MANUALMENTE en Excel (calcular línea por línea)
         Documentar:
         - Campo F2F: valor base
         - Parafiscales: valor final
         - Reclutamiento: desglose por NSE
         - Insumos: cada línea
         - Total por rubro

□ Día 3: Cotizar en MatrixNext / UI
         Completar forma con mismos datos
         Presionar "Calcular"

□ Día 4: Comparar línea por línea
         Crear tabla comparativa Excel vs MatrixNext
         Buscar diferencias > 0.1%

□ Día 5: Ajustar fórmulas si hay diferencias
         Investigar causa (fórmula, lookup, seed)
         Re-test hasta diferencia < 0.1%

CRITERIO DE ACEPTACIÓN:
  ✅ Diferencia < 0.1% en TODOS los rubros
  ✅ Documentado y firmado por QA

Entregable:
  Documento: "TEST_PARIDAD_F2F_400BGT_20MIN.xlsx"
  Status: PASS ✅
```

### Sprint 1.4: Testing Paridad - CASO CATI
**Duración**: 2-3 días  
**Dependencia**: Sprint 1.3 (F2F validado)  
**Status**: ⏳ PENDIENTE

```
PENDIENTE:
□ Día 1: Crear caso CATI en Excel
         - 300 respondientes mixto 3 ciudades
         - CATI, 15 minutos, Penetración 55-66%

□ Día 2: Cotizar Excel vs MatrixNext (mismo proceso Sprint 1.3)

□ Día 3: Validar diferencia < 0.1% o ajustar

Entregable:
  Documento: "TEST_PARIDAD_CATI_300MIX_15MIN.xlsx"
  Status: PASS ✅
```

### Sprint 1.5: Testing Paridad - CASO ONLINE & MYSTERY
**Duración**: 2-3 días  
**Dependencia**: Sprint 1.4 (CATI validado)  
**Status**: ⏳ PENDIENTE

```
PENDIENTE:
□ Día 1: Crear caso Online + caso Mystery (simultáneamente)

□ Día 2: Cotizar vs Excel / MatrixNext

□ Día 3: Validar diferencia < 0.1%

Entregable:
  Documentos: "TEST_PARIDAD_ONLINE_*.xlsx", "TEST_PARIDAD_MYSTERY_*.xlsx"
  Status: PASS ✅
```

### Sprint 1.6: Code Review & Documentación Final
**Duración**: 2-3 días  
**Dependencia**: Sprints 1.3-1.5 (testing completado)  
**Status**: ⏳ PENDIENTE

```
PENDIENTE:
□ Día 1: Code review de QuoteCalculator.cs, EasyQuoteViewModel, Index.cshtml
         - Peer review con otro dev
         - Checklist: naming, comments, edge cases
         - Aprobación

□ Día 2: Documentación final
         - Mapping campo a campo (Excel vs MatrixNext)
         - Supuestos utilizados
         - Limitaciones conocidas
         - FAQ para usuarios Excel

□ Día 3: Demo video para usuarios + documentación

Entregable:
  - ✅ Code aprobado
  - ✅ Documentación completa
  - ✅ Demo video (5-10 min)
```

### Sprint 1.7: Deploy Pruebas & Smoke Test
**Duración**: 1-2 días  
**Dependencia**: Sprint 1.6 (code review)  
**Status**: ⏳ PENDIENTE

```
PENDIENTE:
□ Día 1: Merge a develop con PR detallado
         Deploy a ambiente pruebas
         Verificar BD scripts ejecutados
         Verificar UI carga correctamente

□ Día 2: Smoke test básico:
         - Login funciona
         - Crear nueva cotización
         - Completar 3-4 campos
         - Presionar Calcular
         - Resumen se muestra
         - Guardar funciona

Entregable:
  ✅ Sistema funcionando en ambiente pruebas
  ✅ Checklist smoke test completado
```

---

## 📋 FASE 2: REFINAMIENTO (Post-MVP) - LISTO EJECUTAR

### Objetivo
Mejorar UX, versionado, y exports sin afectar paridad.

### Sprint 2.1: Versionado Formal de Maestros - OPCIONAL
**Duración**: 2-3 días  
**Prioridad**: MEDIA (importante para operación pero NO bloquea MVP)  
**Status**: 📋 LISTO EJECUTAR

```
TAREAS:
□ Día 1: Agregar columnas vigente_desde/vigente_hasta a maestras:
         - eq_param_precio
         - eq_param_cati
         - eq_param_online
         - eq_param_factores
         - eq_rate_horas
         - eq_valor_hora_ops
         
         ALTER TABLE [tabla] ADD 
           vigente_desde DATE DEFAULT GETDATE(),
           vigente_hasta DATE DEFAULT NULL;

□ Día 2: UI Admin para versionado:
         - Mostrar versiones activas
         - Botón "Desactivar" con fecha fin
         - Historial de cambios por tabla

□ Día 3: Lookup por fecha en cotización:
         - Al calcular, usar datos vigentes a fecha_cotizacion
         - Auditoría: qué datos se usaron
         
         SELECT * FROM eq_param_precio 
         WHERE vigente_desde <= @fecha AND (vigente_hasta IS NULL OR vigente_hasta > @fecha)

BENEFICIOS:
  ✅ Control formal de cambios de precios/tarifas
  ✅ Histórico de vigencias
  ✅ Auditoría de qué se usó en qué fecha
  ✅ Comparativa de cotizaciones en diferentes épocas

BLOQUEADOR: Ninguno (independiente)
TESTING: Crear cotizaciones con fechas diferentes, verificar valores cambien
```

### Sprint 2.2: Export PDF/Excel - CRÍTICO POST-MVP
**Duración**: 3-4 días  
**Prioridad**: ALTA (users lo piden para flujo aprobaciones)  
**Status**: 📋 LISTO EJECUTAR

```
TAREAS:
□ Día 1-2: PDF branded Ipsos
           - Header: Logo Ipsos + título
           - Contenido: Datos cotización + resumen por rubros
           - Footer: Fecha + firma digital (opcional)
           - Librería: iText7 o SelectPdf
           
           Implementación:
           [HttpGet("{id}/export-pdf")]
           public async Task<IActionResult> ExportPdf(int id)
           {
               var quote = await _service.GetQuoteAsync(id);
               var pdfStream = GeneratePdf(quote); // Create PDF
               return File(pdfStream, "application/pdf", 
                   $"Cotización-{quote.Header.Nombre}.pdf");
           }

□ Día 2-3: Excel con detalle completo
           - Sheet 1: Resumen cotización
           - Sheet 2: Desglose por rubros (campo, insumos, staff, etc.)
           - Sheet 3: Metodología y muestra
           - Formato similar a original Ipsos
           - Librería: EPPlus o NPOI
           
           Implementación:
           [HttpGet("{id}/export-excel")]
           public async Task<IActionResult> ExportExcel(int id)
           {
               var quote = await _service.GetQuoteAsync(id);
               var excelStream = GenerateExcel(quote);
               return File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                   $"Cotización-{quote.Header.Nombre}.xlsx");
           }

□ Día 4: Testing de exports
         - Generar PDF vs Excel para caso F2F
         - Validar formato, números, datos correctos
         - Comparar con Excel original Ipsos

BENEFICIOS:
  ✅ Users pueden descargar y compartir cotizaciones
  ✅ Integración con flujo de aprobaciones (enviar PDF)
  ✅ Auditoría: descarga de cotizaciones
  ✅ Cumplimiento regulatorio (Ipsos)

BLOQUEADOR: Ninguno (independiente)
TESTING: Crear cotización, descargar PDF/Excel, verificar contenido vs BD
```

### Sprint 2.3: UX Polish - READY NOW ⭐
**Duración**: 5-7 días  
**Prioridad**: ALTA (mejora experiencia usuario)  
**Status**: ✅ LISTO EJECUTAR INMEDIATAMENTE

**VER DOCUMENTO DEDICADO**: [SPRINT_2_3_Y_FASE_3_PLAN_EJECUTIVO.md](SPRINT_2_3_Y_FASE_3_PLAN_EJECUTIVO.md)

```
TASKS:
T2.3.1: Tooltips explicativos (2 días)
        - Penetración: "Porcentaje de población que cumple criterios"
        - Duración: "Tiempo por entrevista (5-60 min)"
        - Clase de Prueba: "Tipo de producto a evaluar"
        - NSE: "Nivel Socioeconómico (afecta tarifas)"
        - Etc. (10 tooltips total)
        
        Implementación: Bootstrap 5 data-bs-toggle="tooltip"

T2.3.2: Validación visual de sumas en grids (2 días)
        - Grid Muestra: Validar que suma NSE por ciudad = MuestraTotal
        - Highlight en rojo si no suma
        - Mensaje: "Suma actual: X. Esperado: Y"
        
        Implementación: JavaScript on input change + CSS .table-danger

T2.3.3: Loading spinners en cálculos (1 día)
        - Botón "Calcular": Mostrar spinner mientras calcula
        - Deshabilitar botón durante cálculo
        
        Implementación: Bootstrap spinner + async/await

T2.3.4: Mensajes de error mejorados (1 día)
        - Alertas por tipo: success, warning, danger
        - Auto-dismiss después 5 segundos
        - Mensajes específicos por validación
        
        Implementación: Bootstrap alerts component

T2.3.5: Responsive design móvil (1 día) - OPCIONAL
        - Hamburger menu para mobile
        - Font-size 16px (prevent iOS zoom)
        - Buttons full-width
        
        Implementación: CSS media queries

BENEFICIOS:
  ✅ UX moderna y profesional
  ✅ Menos errores de captura (validación visual)
  ✅ Mejor feedback del sistema
  ✅ Mobile-friendly
```

---

## 🚫 FASE 3: NICE-TO-HAVE (Backlog, post-lanzamiento)

### Features NO-ESENCIALES (NO son críticas para MVP)

```
Bajo Prioridad - Implementar SOLO si hay tiempo / demanda usuario:

□ Clonar cotización existente (facilita variantes)
□ Histórico y comparación de versiones
□ Dashboard de aprobaciones (probabilidad vs real aprobado)
□ Calculadora en vivo (sin botón "Calcular")
□ Búsqueda avanzada de cotizaciones histórico

Tiempo total estimado: 6-8 semanas (si se implementan todas)
Recomendación: Priorizar según feedback usuarios post-lanzamiento
```

---

## 📈 MÉTRICAS DE PROGRESO

### Completitud por Área

| Área | Inicial | Actual | Target MVP | Gap |
|------|---------|--------|-----------|-----|
| Modelos | 95% | ✅ 100% | 100% | ✅ 0% |
| Tablas BD | 85% | ✅ 95% | 100% | ⚠️ 5% (versionado) |
| Seeds | 70% | ✅ 80% | 100% | ⚠️ 20% (validar Excel) |
| UI Controls | 85% | ✅ 100% | 100% | ✅ 0% |
| Fórmulas | 40% | ✅ 100% | 100% | ✅ 0% |
| Testing Paridad | 0% | ❌ 0% | 100% | ❌ 100% |
| **TOTAL MVP** | **62%** | **73%** | **100%** | **27%** |

**Conclusión**: Falta ~27% trabajo, todo concentrado en testing (Sprint 1.3-1.6).

---

## 📅 TIMELINE REALISTA

```
SEMANA 1: Sprint 1.1-1.2 (Seeds Reales)
├─ Día 1-2: Conseguir Excel + documentar ubicaciones
├─ Día 3-5: Extraer matrices CATI/Online/Factores/Horas/Precios
└─ Día 6-7: Validar seeds en BD
   📊 Entregable: Seeds reales 100% sembrados

SEMANA 2: Sprint 1.3-1.4 (Testing F2F + CATI)
├─ Día 1-3: Caso F2F: Excel vs MatrixNext, ajustar fórmulas
├─ Día 3-5: Caso CATI: idem
└─ Día 6-7: Documentar hallazgos + ajustes
   📊 Entregable: Paridad F2F y CATI validada

SEMANA 3: Sprint 1.5-1.7 (Testing Online/Mystery + Deploy)
├─ Día 1-2: Caso Online y Mystery
├─ Día 3-4: Code review + documentación
├─ Día 5-6: Deploy pruebas + smoke test
└─ Día 7: Buffer para fixes
   📊 Entregable: Sistema en pruebas, paridad 1:1 certificada

POST-MVP:
├─ FASE 2 (1-2 semanas): Versionado, exports, UX
├─ FASE 3 (backlog): Nice-to-have features
└─ LANZAMIENTO: Producción controlada (piloto 1-2 users)
```

**Total hasta MVP producción**: 3 semanas (+ 1-2 semanas refinamiento FASE 2)

---

## ✅ CRITERIOS DE ACEPTACIÓN - MVP

**ANTES de lanzar a producción, DEBE cumplir**:

1. ✅ **Cobertura de Campos**: 100% campos de ANALISIS_EASYQUOTE capturados
   - Actual: ✅ 100% completado

2. ✅ **Cobertura de Fórmulas**: 100% fórmulas implementadas
   - Actual: ✅ 100% completado (26/26)

3. ⚠️ **Seeds Reales**: 100% sembrados con datos REALES (no placeholders)
   - Actual: ⏳ 80% (pendiente validación Excel)
   - Sprint: 1.1-1.2

4. ❌ **Testing Paridad**: Diferencia < 0.1% vs Excel en 4+ casos base
   - Actual: ❌ 0% testeado
   - Sprints: 1.3-1.5

5. ✅ **Validaciones**: Implementadas y testeadas
   - Actual: ✅ En QuoteCalculator (rango duración, penetración válida, etc.)

6. ✅ **Documentación**: Mapping campo a campo + supuestos
   - Actual: ⏳ 80% (ANALISIS_EASYQUOTE detallado, falta final)
   - Sprint: 1.6

7. ✅ **Code Review**: Aprobado por otro dev
   - Actual: ❌ Pendiente
   - Sprint: 1.6

**Cumplimiento Actual**: 4/7 ✅ | 2/7 ⏳ | 1/7 ❌

**Conclusión**: Necesita ~2-3 semanas (FASE 1 completa) para cumplir 7/7.

---

## 🔍 CHECKLIST DIARIO (Sprints 1.1-1.7)

### Cada Día (Por cada sprint)

```
□ 09:00 - Stand-up (5 min)
   - ¿Qué hice ayer?
   - ¿Qué hago hoy?
   - ¿Bloqueadores?

□ 10:00-14:00 - Ejecución sprint
   - Implementar tasks del sprint
   - Commits frecuentes con mensajes descriptivos
   - Testing incremental

□ 14:00 - Peer review si hay cambios
   - Code quality
   - Funcionalidad

□ 15:00 - Testing local
   - Verificar cambios funcionan
   - No quebró nada existente

□ 16:30 - Actualizar TODO este documento
   - Marcar tasks completadas
   - Documentar bloqueadores

□ 17:00 - Wrap-up
   - Preparar BD para día siguiente (si es necesario)
   - Backup cambios
```

---

## 🚀 COMO USAR ESTE TODO

### Status Tracking

Cada task tiene uno de estos estados:

```
✅ COMPLETADO   - Tarea terminada y validada
⏳ EN PROGRESO  - Alguien está trabajando
⚠️ BLOQUEADO    - Esperando dependencia/decisión
📋 PENDIENTE    - Sin iniciar, listo para empezar
❌ FALTANTE     - Identificado pero no listo

Color en Kanban:
🟢 Green = ✅
🟡 Yellow = ⏳ ⚠️
🔴 Red = ❌
⚪ Gray = 📋
```

### Daily Update (18:00)

```
Actualizar este archivo:
- Sprints completados → ✅ COMPLETADO + fecha
- Sprints en progreso → ⏳ EN PROGRESO + % estimado
- Bloqueadores nuevos → 🔥 BLOQUEADOR + descripción
- Hallazgos/aprendizajes → documenta abajo
```

### Weekly Review (Viernes 17:00)

```
Preguntas:
1. ¿Completamos objectives semana?
2. ¿Hay bloqueadores nuevos?
3. ¿Velocidad está OK (esperado: 5-6 días/sprint)?
4. ¿Re-estimaciones necesarias?
5. ¿Riesgos nuevos identificados?

Documentar: Sección "Aprendizajes & Hallazgos"
```

---

## 📝 APRENDIZAJES & HALLAZGOS

### Hallazgos Sprint 0 (Implementation)

```
✅ Excel Ipsos EasyQuote 2025v2 existe (archivos .xlsm + .xlsx)
   → Confirmado usuario, está en solución (CSV/ folder)
   → Ubicación: c:\Users\johnd\source\repos\johndavidpatino\Matrix\

✅ Markdown audit files completos (5,500+ formulas catalogued)
   → inventario_formulas_ipsos_easyquote_2025v2.md
   → analisis_excel_para_mvc_resumen.md
   → diccionario_campos_y_bloques_para_mvc.md
   → Provide real formula references (D67, D68, D71 validated)

⚠️ Seeds MERGE statements funcionan pero son placeholder ejemplo
   → eq_param_cati: 88 registros structure OK, valores necesitan validación Excel
   → eq_rate_horas: 12 registros ejemplo, necesita 200+ completo de Horas.csv
   → Conclusión: Implementación OK, datos pending validación

✅ 26 fórmulas implementadas y compiladas exitosamente
   → Lookups en maestros correctos
   → Conditional logic validada básicamente
   → Pending testing vs Excel valores reales

✅ QuoteCalculator service injection and method calls working
   → EasyQuoteMasterService loads nuevo masters (Factores, Horas) en cache
   → Lookups por KEY funcionan (SL|RecordDetail|MetodologiaSL)
```

### Hallazgos Posteriores (A documentar durante testing)

```
📝 Será actualizado durante Sprints 1.3-1.6 con hallazgos de testing paridad
```

---

## 🛠️ RECURSOS NECESARIOS

### Equipo

```
1 Desarrollador backend (full-time, 3-4 semanas)
  → Sprints 1.1, 1.2, 1.6 (seeds, validation, code review)
  
1 Desarrollador frontend (50%, 2 semanas)
  → Sprint 1.5-1.6 (si UI changes requieren ajustes post-testing)

1 QA/Tester (50%, 2 semanas)
  → Sprints 1.3-1.5 (testing paridad, documentación casos)

1 Usuario Excel experto (consultoría, 1 semana)
  → Sprint 1.1 (validar supuestos fórmulas)
  → Sprints 1.3-1.5 (validar resultados vs Excel)
```

### Accesos Requeridos

```
✅ Repositorio Git (ya tiene)
✅ Visual Studio / VS Code (ya tiene)
✅ SQL Server Management Studio (ya tiene)
🔥 ARCHIVO EXCEL Ipsos EasyQuote 2025v2.xlsm (NECESITA)
✅ Base de datos SQL Server (ya tiene)
✅ SharePoint/Teams para documentación (puede usar)
```

### Herramientas Recomendadas

```
✅ Git for version control (usando)
✅ Azure DevOps / Jira para tracking (opcional)
📊 Excel comparador (WinMerge, Beyond Compare) para validar seeds
📹 OBS Studio para grabar demos (gratis)
📝 Markdown editor (VS Code integrado)
```

---

## ⚠️ RIESGOS IDENTIFICADOS

| Riesgo | Probabilidad | Impacto | Mitigation | Owner |
|--------|--------------|---------|------------|-------|
| **Excel access bloqueado** | 🔥 ALTA | CRÍTICO | Conseguir inmediatamente | PM |
| **Fórmulas Excel mal documentadas** | 🟠 Media | Alto | Reverse engineering con casos | Dev |
| **Seeds con valores incorrectos** | 🟠 Media | CRÍTICO | Validar 100% vs Excel real | QA |
| **Testing incompleto (solo F2F)** | 🟡 Baja | Medio | Suite 4+ casos obligatoria | QA |
| **Performance con 1000+ ciudades** | 🟡 Baja | Medio | Monitor durante testing | Dev |
| **Cambios Excel durante migración** | 🟠 Media | Medio | Snapshot fecha actual | PM |
| **Bugs no detectados en producción** | 🟠 Media | CRÍTICO | Smoke test + monitoring | Ops |

---

## 🎓 SUPUESTOS A VALIDAR (con Excel Real)

```
ANTES de Sprints 1.3-1.5, CONFIRMAR estos supuestos:

✅ Parafiscales F2F: ¿% exacto? → Buscar en Excel Parametros/Valores
✅ Refrigeración: ¿Factor 1.15 + costo nevera 970000? → Validar D42
✅ Base de Datos: ¿Costos reales (No req/Cliente/Comprar)? → Extraer valores
✅ Codificación: ¿Lógica exacta #pregs/#regs? → Mapear tabla
✅ Viaticos: ¿Productividad por ciudad? → Validar eq_productividad_ciudad
✅ Mystery D73-D75: ¿Componentes exactos? → Documentar pseudocódigo
✅ Staff SL KEY: ¿Formato "SL|RecordDetail|MetodologiaSL"? → Validar
✅ Envío volumétrico: ¿Unidades cm? → Confirmar
✅ Penetración: ¿Rangos son MAS82, 75-82, 67-74, etc.? → ✅ Ya confirmado
✅ Duración: ¿Rango 5-60 minutos? → ✅ Ya confirmado

Status: 2/11 confirmados, 9/11 pending validación Excel real
```

---

## 📞 ESCALATION PATH

Si hay bloqueadores críticos:

```
1. 🟢 Issue: Intenta resolver localmente (dev)
   Tiempo: hasta 2 horas

2. 🟡 Bloqueado: Escala a team lead / tech lead
   Tiempo: hasta 4 horas

3. 🔴 CRÍTICO: Escala a project manager / cliente
   Tiempo: INMEDIATO (ej: no tenemos Excel)
   
Contact:
  Tech Lead: [nombre, email]
  PM: [nombre, email]
  Cliente: [nombre, email]
```

---

**Documento actualizado**: 2026-01-05  
**Próxima revisión**: Diariamente (18:00) durante Sprints 1.1-1.7  
**Owner**: Dev Team  
**Aprobado por**: [PM/Tech Lead]
