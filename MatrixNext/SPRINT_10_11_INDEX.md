# SPRINT 10 & 11: ÍNDICE DE DOCUMENTACIÓN

**Fecha**: 2026-01-15  
**Propósito**: Navegar rápidamente entre documentos para Sprint 10 (RP_Reportes) y Sprint 11 (OP_RO + OP_Trafico)

---

## 📚 ESTRUCTURA DE DOCUMENTOS

### 🎯 EMPIEZA AQUÍ

1. **[SPRINT_10_11_KICKOFF_GUIDE.md](SPRINT_10_11_KICKOFF_GUIDE.md)** (5 min)
   - TL;DR de ambos sprints
   - Quick start para dev
   - Checklist pre-sprint
   - TOP 3 riesgos

2. **[SPRINT_10_11_PLAN_DETALLADO.md](SPRINT_10_11_PLAN_DETALLADO.md)** (30 min)
   - Plan día a día (10 días por sprint)
   - Inventario legacy
   - Arquitectura de solución
   - Entregables y riesgos
   - **Este es el documento MAESTRO**

---

### 🔍 ANÁLISIS TÉCNICO PROFUNDO

3. **[docs/GENERAL/SPRINT_10_11_COREPROJECT_MAPPING.md](docs/GENERAL/SPRINT_10_11_COREPROJECT_MAPPING.md)** (20 min)
   - Mapeo WebMatrix → StoredProcedures
   - Tablas de análisis por categoría
   - Plantillas para llenar
   - Validaciones en CoreProject
   - **Usar este DURANTE el sprint para completar mapeos**

---

### 📋 REGLAS Y DIRECTRICES

4. **[DIRECTRICES_MIGRACION.md](DIRECTRICES_MIGRACION.md)** (30 min - lectura obligatoria)
   - REGLA 1: Respetar nombres BD
   - REGLA 2: Mapear metadata (CRÍTICA)
   - REGLA 5.1: AJAX-first con modales
   - REGLA 10: 0 errores compilación
   - **Leer antes de empezar código**

5. **[docs/GENERAL/BACKLOG_MIGRACION_GLOBAL.md](docs/GENERAL/BACKLOG_MIGRACION_GLOBAL.md)** (20 min - contexto)
   - Visión global de sprints 5-12
   - Dependencias entre módulos
   - Checklist de validación obligatorio
   - Priorización global

---

### 📊 ESTADO Y SEGUIMIENTO

6. **[DASHBOARD_MIGRACION.md](DASHBOARD_MIGRACION.md)** (5 min - revisar diariamente)
   - Estado actual de todos los módulos
   - Semáforo de progreso
   - Timeline visual
   - **Actualizar diariamente con avances**

7. **[README_SPRINTS_5_12.md](README_SPRINTS_5_12.md)** (10 min - contexto)
   - Resumen ejecutivo de roadmap
   - Deliverables por sprint
   - Métricas de éxito
   - Referencias cruzadas

---

### 🏗️ PATRONES Y ARQUITECTURA

8. **MatrixNext/Areas/[CU|OP|TH]/Program.cs** (referencia)
   - Cómo registrar adapters/services en DI
   - Patrón de estructura de carpetas
   - **Copiar patrón para RP_Reportes y OP_RO/Trafico**

9. **MatrixNext/Data/Adapters/[Módulo]Adapter.cs** (referencia)
   - Clase base para adapters
   - Uso de Dapper + Entity Framework
   - Patrón de manejo de conexiones
   - **Usar como template**

10. **MatrixNext/Web/Areas/[Módulo]/Controllers/[Módulo]Controller.cs** (referencia)
    - Patrón REST API
    - Uso de ApiResponse<T>
    - Atributos [Authorize] y [HttpGet/Post]
    - **Copiar estructura**

---

## 📖 LECTURA RECOMENDADA POR ROL

### 👨‍💻 PARA DEV PRINCIPAL

**Pre-Sprint 10**:
1. SPRINT_10_11_KICKOFF_GUIDE.md (2 min)
2. SPRINT_10_11_PLAN_DETALLADO.md - Sprint 10 section (10 min)
3. DIRECTRICES_MIGRACION.md - REGLA 2, 5.1 (10 min)
4. SPRINT_10_11_COREPROJECT_MAPPING.md (5 min, para referencia)
5. Ejemplo: MatrixNext/Areas/OP/Data/Adapters (30 min, estudio código)

**Durante Sprint 10**:
- Diario: SPRINT_10_11_PLAN_DETALLADO.md "Plan diario" section
- Día 1-2: SPRINT_10_11_COREPROJECT_MAPPING.md "Sprint 10" section → llenar tablas
- Día 4-5: Usar adapters pattern de MatrixNext/Areas/CU
- Día 6+: DIRECTRICES_MIGRACION.md REGLA 5.1 para views AJAX

**Pre-Sprint 11**:
1. SPRINT_10_11_KICKOFF_GUIDE.md - Sprint 11 section (3 min)
2. SPRINT_10_11_PLAN_DETALLADO.md - Sprint 11 section (15 min)
3. SPRINT_10_11_COREPROJECT_MAPPING.md - Sprint 11 sections (10 min)
4. State machine pattern (buscar ejemplo si existe)

---

### 🧪 PARA QA LEAD

**Pre-Sprint 10**:
1. SPRINT_10_11_KICKOFF_GUIDE.md (2 min)
2. SPRINT_10_11_PLAN_DETALLADO.md - Sprint 10 "QA" section (5 min)
3. DIRECTRICES_MIGRACION.md "Testing y Validación" (5 min)
4. BACKLOG_MIGRACION_GLOBAL.md "Checklist de validación obligatorio" (10 min)

**Durante Sprint 10**:
- Día 5-7: Preparar matriz de pruebas (reportes × filtros × exportes)
- Día 9-10: Ejecutar pruebas funcionales (ver SPRINT_10_11_PLAN_DETALLADO.md "DÍA 9-10")
- Generar QA Report con cobertura

**Pre-Sprint 11**:
- Similar a Sprint 10, pero con focus en state machine testing
- Validaciones de integraciones con OP_Cuantitativo/Cualitativo

---

### 👔 PARA TECH LEAD

**Pre-Sprint 10**:
1. SPRINT_10_11_KICKOFF_GUIDE.md (2 min)
2. SPRINT_10_11_PLAN_DETALLADO.md "Arquitectura de Solución" (10 min)
3. README_SPRINTS_5_12.md (5 min - contexto global)
4. DIRECTRICES_MIGRACION.md (revisión completa, 30 min)

**Día 1 Sprint 10**:
- Validar que inventario es correcto
- Revisar mapeo SP (REGLA 2)
- Aprobar arquitectura propuesta

**Día 5 Sprint 10**:
- Code review: Adapters, Services, Controllers
- Validar DI registration
- Compilación sin errores

**Día 10 Sprint 10**:
- Validar QA completado
- Documentación OK
- Cierre de sprint

---

### 📊 PARA PRODUCT OWNER

**Pre-Sprint 10**:
1. README_SPRINTS_5_12.md "Métricas de éxito" (5 min)
2. SPRINT_10_11_KICKOFF_GUIDE.md "Métricas de éxito" (2 min)
3. SPRINT_10_11_PLAN_DETALLADO.md "Entregables" (5 min)
4. DASHBOARD_MIGRACION.md (1 min)

**Durante Sprints**:
- Revisar DASHBOARD_MIGRACION.md cada viernes
- Estar disponible para clarificaciones de scope
- Validar que no hay scope creep

---

## 🎯 RESPONDER PREGUNTAS COMUNES

### "¿Dónde encuentro info de...?"

| Pregunta | Respuesta |
|---|---|
| Plan día a día de Sprint 10/11 | [SPRINT_10_11_PLAN_DETALLADO.md](SPRINT_10_11_PLAN_DETALLADO.md) "Plan diario" |
| Qué reportes migrar en Sprint 10 | [SPRINT_10_11_PLAN_DETALLADO.md](SPRINT_10_11_PLAN_DETALLADO.md) "Inventario legacy" |
| Qué SP usa cada reporte | [docs/GENERAL/SPRINT_10_11_COREPROJECT_MAPPING.md](docs/GENERAL/SPRINT_10_11_COREPROJECT_MAPPING.md) "Matriz consolidada" |
| Cómo hacer adapter/service | MatrixNext/Areas/CU/Data (ejemplo) + [DIRECTRICES_MIGRACION.md](DIRECTRICES_MIGRACION.md) REGLA 3-4 |
| Cómo hacer views AJAX | [DIRECTRICES_MIGRACION.md](DIRECTRICES_MIGRACION.md) REGLA 5.1 + MatrixNext/Areas/CU/Views |
| Cuáles son los riesgos | [SPRINT_10_11_PLAN_DETALLADO.md](SPRINT_10_11_PLAN_DETALLADO.md) "Riesgos" |
| Qué exportar: Excel/PDF | [SPRINT_10_11_PLAN_DETALLADO.md](SPRINT_10_11_PLAN_DETALLADO.md) "Tecnologías" (ClosedXML, iText) |
| State machine OP_Trafico | [SPRINT_10_11_PLAN_DETALLADO.md](SPRINT_10_11_PLAN_DETALLADO.md) "State Machine Example" |
| Cómo registrar en Program.cs | MatrixNext.Web/Program.cs (buscar builder.Services.AddScoped) |
| Checklist pre-sprint | [SPRINT_10_11_KICKOFF_GUIDE.md](SPRINT_10_11_KICKOFF_GUIDE.md) "Checklist pre-sprint" |
| Estado actual | [DASHBOARD_MIGRACION.md](DASHBOARD_MIGRACION.md) (revisar diariamente) |
| Reglas de migración | [DIRECTRICES_MIGRACION.md](DIRECTRICES_MIGRACION.md) REGLA 1-10 (lectura obligatoria) |

---

## 🔄 FLUJO DE TRABAJO DURANTE SPRINTS

```
SEMANA 1 (SPRINT 10):
├── DÍA 1 (Inventario)
│   ├── 📄 Leer: SPRINT_10_11_PLAN_DETALLADO.md "Día 1"
│   ├── 🛠️ Usar: SPRINT_10_11_COREPROJECT_MAPPING.md "Matriz inicial"
│   └── 📊 Actualizar: DASHBOARD_MIGRACION.md
│
├── DÍA 2-3 (Mapeo SP)
│   ├── 📄 Leer: SPRINT_10_11_PLAN_DETALLADO.md "Día 2-3"
│   ├── 🛠️ Usar: SPRINT_10_11_COREPROJECT_MAPPING.md "Tablas de mapeo"
│   ├── 📋 Referencia: DIRECTRICES_MIGRACION.md REGLA 2
│   └── 📊 Actualizar: DASHBOARD_MIGRACION.md
│
├── DÍA 4-5 (Adapters/Services)
│   ├── 📄 Leer: SPRINT_10_11_PLAN_DETALLADO.md "Día 4-5"
│   ├── 📋 Referencia: DIRECTRICES_MIGRACION.md REGLA 3-4, 10
│   ├── 🛠️ Usar: MatrixNext/Areas/CU (ejemplo)
│   ├── 📝 Program.cs DI registration
│   └── 📊 Actualizar: DASHBOARD_MIGRACION.md
│
SEMANA 2 (SPRINT 10):
├── DÍA 6 (Controllers)
│   ├── 📄 Leer: SPRINT_10_11_PLAN_DETALLADO.md "Día 6"
│   ├── 📋 Referencia: DIRECTRICES_MIGRACION.md REGLA 5.1 (API endpoints)
│   └── 🛠️ Usar: MatrixNext/Areas/OP/Controllers (ejemplo)
│
├── DÍA 7-8 (Views)
│   ├── 📄 Leer: SPRINT_10_11_PLAN_DETALLADO.md "Día 7-8"
│   ├── 📋 Referencia: DIRECTRICES_MIGRACION.md REGLA 5.1 (AJAX + modales)
│   └── 🛠️ Usar: MatrixNext/Areas/CU/Views (ejemplo)
│
├── DÍA 9-10 (QA)
│   ├── 📄 Leer: SPRINT_10_11_PLAN_DETALLADO.md "Día 9-10"
│   ├── 📊 Ejecutar: Pruebas funcionales (ver matriz)
│   └── 📝 Generar: QA Report
│
└── CIERRE
    ├── 📄 Crear: MIGRACION_RP_REPORTES_COMPLETADA.md
    ├── 📊 Actualizar: DASHBOARD_MIGRACION.md (RP = 🟢)
    └── ✅ Validar: Checklist post-sprint

SPRINT 11: (similar, con focus en state machine + integraciones)
```

---

## 📞 CÓMO USAR ESTE ÍNDICE

1. **Eres DEV**: Lee "Para DEV Principal" → haz follow para documentos listados
2. **Eres QA**: Lee "Para QA Lead" → haz follow para documentos listados
3. **Eres TECH LEAD**: Lee "Para Tech Lead" → haz follow
4. **Eres PO**: Lee "Para Product Owner" → monitorea DASHBOARD
5. **Tienes pregunta**: Busca en tabla "¿Dónde encuentro info de...?"

---

## 🔗 REFERENCIAS CRUZADAS

### Documentos relacionados de sprints anteriores:

- **Sprint 4 (TH API)**: 
  - [docs/TH/RESUMEN_MIGRACION_AUSENCIAS.md](docs/TH/RESUMEN_MIGRACION_AUSENCIAS.md)
  - `MatrixNext/MatrixNext.Web/Areas/TH` (código)

- **Sprint 5-7 (TH Views, OP Complementos, CORE)**:
  - [docs/OP/SPRINT_5_CIERRE_MIGRACION_COMPLETA.md](docs/OP/SPRINT_5_CIERRE_MIGRACION_COMPLETA.md)

- **Sprint 8 (EQ_EasyQuote)**:
  - [docs/EQ/ANALISIS_EQ_EASYQUOTE.md](docs/EQ/ANALISIS_EQ_EASYQUOTE.md)
  - [docs/EQ/FORMULAS_MAPPING.md](docs/EQ/FORMULAS_MAPPING.md)

---

## ✅ ANTES DE LLAMAR AL KICKOFF DE SPRINT 10

- [ ] He leído SPRINT_10_11_KICKOFF_GUIDE.md (2 min)
- [ ] He leído SPRINT_10_11_PLAN_DETALLADO.md Sprint 10 section (10 min)
- [ ] Tengo claro mi rol (dev/QA/tech lead/PO)
- [ ] Tengo acceso a WebMatrix + CoreProject + MatrixNext
- [ ] Tengo acceso a staging (BD real)
- [ ] Comprendo las REGLAS (DIRECTRICES_MIGRACION.md)
- [ ] Sé dónde encontrar recursos (este índice)

---

**Documento creado**: 2026-01-15  
**Próxima revisión**: Pre-Sprint 10 (2026-04-05)  
**Owner**: Tech Lead / Documentation

