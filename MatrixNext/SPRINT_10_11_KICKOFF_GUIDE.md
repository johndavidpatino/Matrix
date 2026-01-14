# SPRINT 10 & 11: GUÍA RÁPIDA DE KICKOFF

**Fecha**: 2026-01-15  
**Para**: Dev, QA, Tech Lead  
**Duración total**: 4 semanas (2 semanas por sprint)

---

## ⚡ TL;DR (2 minutos de lectura)

| Aspecto | Sprint 10 | Sprint 11 |
|---|---|---|
| **Qué migramos** | 72 reportes | Revisión Operacional (4 tipos) + Tráfico de datos |
| **Complejidad** | 🟡 Media (muchos reportes) | 🟡 Media (state machine) |
| **SP a mapear** | ~25-30 | ~29-30 |
| **Días de trabajo** | 10 días | 10 días |
| **Riesgo TOP** | Scope creep | Integraciones complejas |
| **Inicio** | 2026-04-05 | 2026-04-19 |
| **Fin** | 2026-04-16 | 2026-05-03 |
| **Hito Global** | 60% del backlog | 🎯 **100% completado!** |

---

## 📋 DOCUMENTOS CLAVE (LEER EN ORDEN)

1. **[SPRINT_10_11_PLAN_DETALLADO.md](SPRINT_10_11_PLAN_DETALLADO.md)** ← **LEER PRIMERO**
   - Plan día a día (10 días por sprint)
   - Entregables, riesgos, checklist

2. **[SPRINT_10_11_COREPROJECT_MAPPING.md](docs/GENERAL/SPRINT_10_11_COREPROJECT_MAPPING.md)**
   - Mapeo exacto WebMatrix → SP/Tablas
   - Plantillas para llenar

3. **[DIRECTRICES_MIGRACION.md](DIRECTRICES_MIGRACION.md)**
   - Reglas obligatorias (REGLA 1-10)
   - Especialmente REGLA 2 (mapeo BD) y REGLA 5.1 (AJAX-first)

4. **[BACKLOG_MIGRACION_GLOBAL.md](docs/GENERAL/BACKLOG_MIGRACION_GLOBAL.md)**
   - Context global de sprints
   - Checklist de validación

---

## 🚀 QUICK START PARA CADA SPRINT

### SPRINT 10 (RP_Reportes): Quick Start

**Hora 0 (Kick-off)**:
```
✅ Leer: SPRINT_10_11_PLAN_DETALLADO.md (Sprint 10 section)
✅ Setup: WebMatrix RP_Reportes + CoreProject lado a lado
✅ Acceso: Staging RP_Reportes confirmado
✅ NuGet: ClosedXML + iText instalados
```

**Día 1 (Inventario - 8h)**:
```
1. Listar 72 archivos .aspx en WebMatrix/RP_Reportes
2. Clasificar en 5 categorías (ver SPRINT_10_11_PLAN_DETALLADO.md)
3. Crear INVENTARIO_RP_REPORTES.md
4. Validación: ¿Total 72 archivos? ✅
```

**Día 2-3 (Mapeo SP - 16h)**:
```
1. Abrir cada .aspx.vb y encontrar DataLayer
2. Buscar SP usado (connection.Execute("SP_NAME", ...))
3. Llenar MAPEO_REPORTES_SP.md con Reporte→SP→Parámetros
4. Validación: ¿Todos los SP encontrados en CoreProject? ✅
```

**Día 4-5 (Adapters - 16h)**:
```
1. Crear IReportesAdapter interfaz
2. Implementar ReportesAdapter (Dapper + mapping)
3. Crear ExportService, FiltrosService
4. Registrar en Program.cs
5. Compilación: 0 errores ✅
```

**Semana 2 (Controllers/Views/QA - 20h)**:
```
Día 6: ReportesController endpoints REST
Día 7-8: Views Razor + AJAX
Día 9-10: QA funcional completo
```

**Deliverable Fin Sprint 10**:
- ✅ `MatrixNext.Web/Areas/RP/` completo
- ✅ `INVENTARIO_RP_REPORTES.md`
- ✅ `MAPEO_REPORTES_SP.md`
- ✅ `MIGRACION_RP_REPORTES_COMPLETADA.md`
- ✅ QA Report (10+ pruebas ejecutadas)
- ✅ Dashboard: RP_Reportes = 🟢 COMPLETO

---

### SPRINT 11 (OP_RO + OP_Trafico): Quick Start

**Hora 0 (Kick-off)**:
```
✅ Sprint 10 COMPLETADO 100%
✅ Leer: SPRINT_10_11_PLAN_DETALLADO.md (Sprint 11 section)
✅ Setup: WebMatrix OP_RO + OP_Trafico + CoreProject
✅ Validación: OP_Cuantitativo/Cualitativo accesibles ✅
```

**Día 1 (Inventario - 10h)**:
```
1. Inventario: 5 archivos OP_RO + 6 archivos OP_Trafico
2. Mapear tipos de revisión (Cuestionario, Instructivo, Metodología, Material)
3. Mapear flujo Trafico (Capturado→Criticado→Verificado)
4. Crear INVENTARIO_OP_RO_TRAFICO.md
5. Validación: ¿Todos los archivos documentados? ✅
```

**Día 2-3 (Mapeo SP - 20h)**:
```
OP_RO:
  └─ Mapear 4 tipos × 5 acciones = ~20 SP
     (Get, GetById, Save, Approve, Reject por tipo)

OP_Trafico:
  └─ Mapear flujo: ~10 SP
     (Dashboard, Get, Save, Critica, Asignacion, Verificacion)

Validación: ¿SP confirmadas en CoreProject? ✅
```

**Día 4-5 (Adapters + State Machines - 20h)**:
```
1. OP_RO Adapter (4 adapters o 1 parametrizado)
2. OP_Trafico Adapter
3. OP_ROWorkflowService (máquina de estados)
4. OP_TraficoWorkflowService (máquina de estados)
5. Registrar en Program.cs
6. Compilación: 0 errores ✅
```

**Semana 2 (Controllers/Views/Integraciones/QA - 40h)**:
```
Día 6-7: OP_RO + OP_Trafico Controllers (16h)
Día 8: Views + AJAX (8h)
Día 9: Workflow testing + Integraciones (8h)
Día 10: QA completa + documentación (8h)
```

**Deliverable Fin Sprint 11 (= FIN PROYECTO 🎉)**:
- ✅ `MatrixNext.Web/Areas/OP/OP_RO/` completo
- ✅ `MatrixNext.Web/Areas/OP/OP_Trafico/` completo
- ✅ State machines documentadas
- ✅ `MIGRACION_OP_RO_COMPLETADA.md`
- ✅ `MIGRACION_OP_TRAFICO_COMPLETADA.md`
- ✅ QA Report (15+ pruebas, integraciones validadas)
- ✅ Dashboard: OP_RO = 🟢, OP_Trafico = 🟢
- ✅ **2026-05-03: HITO CRÍTICO = 100% COMPLETADO**

---

## 🎯 CHECKLIST PRE-SPRINT

### ANTES DE SPRINT 10 (Semana del 2026-04-05)

- [ ] **Setup Técnico**:
  - [ ] NuGet packages: ClosedXML, iText7 (o QuestPDF)
  - [ ] Acceso staging RP_Reportes confirmado
  - [ ] Máquina de desarrollo: WebMatrix + CoreProject + MatrixNext lado a lado

- [ ] **Documentación**:
  - [ ] SPRINT_10_11_PLAN_DETALLADO.md leído por dev + QA + tech lead
  - [ ] DIRECTRICES_MIGRACION.md revisadas (REGLA 2, 5.1)
  - [ ] SPRINT_10_11_COREPROJECT_MAPPING.md impreso

- [ ] **Recursos**:
  - [ ] Dev asignado: [Nombre]
  - [ ] QA asignado: [Nombre]
  - [ ] Tech Lead review: [Nombre]

- [ ] **Blockers Mitigados**:
  - [ ] ¿Falta algún NuGet? → Instalar
  - [ ] ¿Acceso staging bloqueado? → Resolver antes
  - [ ] ¿SP falta en CoreProject? → Documentar, no bloquea

---

### ANTES DE SPRINT 11 (Semana del 2026-04-19)

- [ ] **Sprint 10 Status**:
  - [ ] ✅ RP_Reportes 100% completado
  - [ ] ✅ QA ejecutado
  - [ ] ✅ Dashboard actualizado

- [ ] **Setup Técnico**:
  - [ ] OP_Cuantitativo/Cualitativo APIs validadas (acceso + datos)
  - [ ] Staging OP_RO + OP_Trafico confirmado
  - [ ] Email service funcional (para notificaciones)

- [ ] **Documentación**:
  - [ ] SPRINT_10_11_PLAN_DETALLADO.md (Sprint 11 section) leído
  - [ ] State machine diseñada y diagramada
  - [ ] SPRINT_10_11_COREPROJECT_MAPPING.md (OP_RO/Trafico section) actualizado

- [ ] **Recursos**:
  - [ ] Dev asignado: [Nombre] (podría ser mismo que Sprint 10)
  - [ ] QA asignado: [Nombre]
  - [ ] Tech Lead para validar integraciones

---

## ⚠️ TOP 3 RIESGOS

### SPRINT 10:

| Riesgo | Cómo Evitarlo |
|---|---|
| **Scope creep**: "Necesitamos agregar este reporte también..." | Mantener lista de 72 reportes fija; postergar nuevos a Sprint 12+ |
| **SP complejas o faltantes** | Validar completamente Día 2-3; si falta SP, crear issue temprano |
| **Exportes lentos con datos grandes** | Implementar paginación en exports; limitar a 10K filas por export |

### SPRINT 11:

| Riesgo | Cómo Evitarlo |
|---|---|
| **Integraciones rotas con OP_Cuantitativo** | Coordinar con OP team antes de Día 1; validar APIs el primer día |
| **State machine de Trafico mal diseñada** | Diagramar ANTES de código (Día 2-3); revisar transiciones con SME |
| **Permisos/notificaciones no funcionan** | Testear email service Día 6 (no esperar a Day 10) |

---

## 📞 CONTACTOS CLAVE

| Rol | Contacto | Disponibilidad |
|---|---|---|
| **Dev Principal** | [Asignar] | Daily |
| **QA** | [Asignar] | Daily |
| **Tech Lead** | [Asignar] | Revisiones Día 1, 5, 10 |
| **DBA** | [Contacto] | Para validar SP (Día 2) |
| **Product Owner** | [Contacto] | Kick-off + cierre sprint |

---

## 📊 MÉTRICAS DE ÉXITO

### SPRINT 10 (RP_Reportes):
- ✅ 0 errores de compilación
- ✅ 100% de los 72 reportes inventariados
- ✅ 25-30 SP mapeadas y validadas
- ✅ 10+ pruebas funcionales ejecutadas (cobertura ~80%+)
- ✅ Performance: reportes cargan < 2s
- ✅ Exportes funcionan: Excel + PDF

### SPRINT 11 (OP_RO + OP_Trafico):
- ✅ 0 errores de compilación
- ✅ State machines documentadas y testeadas
- ✅ ~29-30 SP mapeadas y validadas
- ✅ 15+ pruebas funcionales (incluye integraciones)
- ✅ OP_Cuantitativo/Cualitativo integración validada ✅
- ✅ Email notificaciones funcionan
- ✅ **HITO: 2026-05-03 = 100% COMPLETADO** 🎉

---

## 📚 REFERENCIAS RÁPIDAS

```
# Si tienes duda de...                    → Lee...
Arquitectura general                       → PLAN_EJECUCION_SPRINTS_5_12.md
Reglas de migración                        → DIRECTRICES_MIGRACION.md
Mapeo BD exacto                            → SPRINT_10_11_COREPROJECT_MAPPING.md
Plan día a día                             → SPRINT_10_11_PLAN_DETALLADO.md
Estado global                              → DASHBOARD_MIGRACION.md
Patrón de adapters/services                → MatrixNext.Web/Areas/[CU/OP/TH/etc]
Cómo registrar en DI                       → MatrixNext.Web/Program.cs (línea ~50+)
Test patterns                              → MatrixNext.Tests/Areas/[MODULE]/[SERVICE]Tests.cs
```

---

## ✅ COMPLETAR ANTES DE LLAMADA DE KICKOFF

- [ ] He leído SPRINT_10_11_PLAN_DETALLADO.md completo
- [ ] He revisado las DIRECTRICES_MIGRACION.md
- [ ] Tengo acceso a WebMatrix + CoreProject + MatrixNext
- [ ] Tengo acceso a staging (BD real)
- [ ] Sé cuál es mi rol (dev/QA/tech lead)
- [ ] Tengo claro el timeline: Sprint 10 (2 sem) → Sprint 11 (2 sem) → 2026-05-03 FIN

---

**Próxima revisión**: Día 1 de Sprint 10 (2026-04-05)  
**Owner**: Tech Lead / Product Owner  
**Versión**: 1.0

