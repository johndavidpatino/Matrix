# SPRINT 12 KICKOFF - PY_ControlCalidad

**Fecha**: 2026-01-15  
**Sprint**: 12 (Parte A)  
**Módulo**: PY_ControlCalidad  
**Fecha Inicio**: 2026-01-16  
**Fecha Entrega**: 2026-02-13  
**Esfuerzo**: 40 horas (5 días)

---

## 📌 RESUMEN EJECUTIVO

Iniciamos la migración del módulo **PY_ControlCalidad** (Control de Calidad de Proyectos Cualitativos).

**En 5 días**, migraremos:
- ✅ 6 páginas WebForms
- ✅ 2 Controllers REST
- ✅ 2 Services (lógica de negocio)
- ✅ 2 Adapters (acceso a datos)
- ✅ 2,300-2,500 LOC nuevas
- ✅ UI con Modales + Grids
- ✅ 100% QA + Documentación

**Resultado Esperado**: Módulo 100% funcional, integrado y documentado.

---

## 📊 ALCANCE

### Funcionalidades a Migrar

| # | Funcionalidad | Tipo | Complejidad |
|---|---------------|------|------------|
| 1 | Control Calidad Campo | Evaluación | ⭐⭐⭐ |
| 2 | Evaluación Moderadora | Evaluación | ⭐⭐⭐ |
| 3 | Evaluación Entrevistadora | Evaluación | ⭐⭐⭐ |
| 4 | Control Transcripciones | Evaluación | ⭐⭐⭐ |
| 5 | Control Informe | Evaluación | ⭐⭐⭐ |
| 6 | Maestro Preguntas | CRUD | ⭐⭐ |

**Total**: 6 funcionalidades, Complejidad promedio ⭐⭐⭐ (Media)

### Dependencias

- ✅ **PY_Proyectos** (100% completado) - Necesario para FK TrabajoId
- ✅ **TH_TalentoHumano** (100% completado) - Necesario para FK PersonaId
- ✅ **Database** - SP y tablas deben existir o crearse

---

## 🎯 OBJETIVOS DEL SPRINT

### Objetivo Principal
Migrar 100% de PY_ControlCalidad manteniendo paridad funcional completa con WebMatrix.

### Objetivos Secundarios
1. ✅ Cero errores de compilación
2. ✅ Cero warnings críticos
3. ✅ 100% QA funcional
4. ✅ Auditoría completada en todas las operaciones
5. ✅ Documentación actualizada

---

## 📋 ENTREGABLES

### Backend
- [ ] `ControlCalidadAdapter.cs` (180 LOC)
- [ ] `PreguntasAdapter.cs` (120 LOC)
- [ ] `ControlCalidadService.cs` (280 LOC)
- [ ] `PreguntasService.cs` (200 LOC)
- [ ] `ControlCalidadController.cs` (250 LOC)
- [ ] `PreguntasController.cs` (150 LOC)
- [ ] 7 DTOs (~200 LOC)

**Total Backend**: ~1,380 LOC

### Frontend
- [ ] `Areas/PY/Views/ControlCalidad/Index.cshtml` (120 LOC)
- [ ] `Areas/PY/Views/ControlCalidad/_Form.cshtml` (150 LOC)
- [ ] `Areas/PY/Views/ControlCalidad/_DetallesGrid.cshtml` (200 LOC)
- [ ] `Areas/PY/Views/Preguntas/Index.cshtml` (80 LOC)
- [ ] `Areas/PY/Views/Preguntas/_Form.cshtml` (80 LOC)
- [ ] `wwwroot/js/py-controlcalidad.js` (300 LOC)
- [ ] `wwwroot/css/py-controlcalidad.css` (100 LOC)

**Total Frontend**: ~1,030 LOC

### Documentación
- [ ] `MIGRACION_PY_CONTROLCALIDAD_COMPLETADA.md`
- [ ] `VERIFICACION_SP_PY_CONTROLCALIDAD.md`
- [ ] `DASHBOARD_MIGRACION.md` (Actualizado)
- [ ] `_Sidebar.cshtml` (Menú actualizado)
- [ ] `Program.cs` (DI registrada)

---

## 🏆 CRITERIOS DE ACEPTACIÓN

### Build
- ✅ 0 errores de compilación
- ✅ 0 warnings críticos
- ✅ IntelliSense funciona

### Funcionalidad
- ✅ CRUD completo (Crear, Leer, Actualizar, Eliminar)
- ✅ Preguntas dinámicas cargadas por tipo
- ✅ Grid paginado y filtrable
- ✅ Modales abren/cierran correctamente
- ✅ Validaciones en server y client

### Seguridad
- ✅ `[Authorize]` en todos los controllers
- ✅ Validaciones en server (no solo client)
- ✅ No se exponen stack traces

### Datos
- ✅ SP ejecutados correctamente
- ✅ FK validadas
- ✅ Auditoría completada (RegistradoPor, FechaRegistro)
- ✅ Transacciones funcionan

### Documentación
- ✅ MIGRACION.md completa
- ✅ Checklist de implementación verificado
- ✅ Menú actualizado
- ✅ Sin código comentado

---

## 👥 EQUIPO ASIGNADO

### Backend (Dev 1)
- **Responsable**: [Asignar]
- **Tareas**: Adapters, Services, Controllers
- **Estimación**: 20 horas

### Frontend (Dev 2)
- **Responsable**: [Asignar]
- **Tareas**: Vistas, JS, CSS
- **Estimación**: 15 horas

### QA & Docs (Dev 3)
- **Responsable**: [Asignar]
- **Tareas**: Testing, Documentación, Menú
- **Estimación**: 5 horas

---

## 📅 CRONOGRAMA

### Semana 1

| Día | Épica | Tareas | Horas | Checkpoint |
|-----|-------|--------|-------|-----------|
| **Lunes** | 1 | Infraestructura: DTOs, Adapters, DbContext | 8h | Adapters listos |
| **Martes** | 2 | Services: ControlCalidadService + Validaciones | 8h | Services listos |
| **Miércoles** | 2-3 | Services: PreguntasService + Controllers | 8h | Controllers listos |
| **Jueves** | 4 | Vistas: Index, Modales, Grid | 8h | UI básica funcional |
| **Viernes** | 4-5 | Vistas: JS, CSS + Testing inicial | 8h | Módulo 80% funcional |

### Semana 2

| Día | Épica | Tareas | Horas | Checkpoint |
|-----|-------|--------|-------|-----------|
| **Lunes** | 4-5 | Vistas finales + Testing | 8h | Módulo 100% funcional |
| **Martes** | 5-6 | QA final + Documentación | 8h | Documentación completa |
| **Miércoles** | - | Buffer / Ajustes / Entrega final | 4h | Sprint 12A COMPLETADO |

**Total**: 40+ horas (5 días full-time)

---

## 🔧 PREREQUISITOS (VERIFICAR ANTES DE INICIAR)

- [ ] ✅ SP en SQL Server verificadas/creadas
- [ ] ✅ FK a PY_Trabajo y TH_Personas existen
- [ ] ✅ Tabla PY_ControlCalidad con auditoría
- [ ] ✅ Tabla PY_DetalleControlCalidad existe
- [ ] ✅ DbContext (PY_Entities) mapea tablas
- [ ] ✅ Enum TipoProceso incluye tipos necesarios
- [ ] ✅ PY_Proyectos está 100% funcional
- [ ] ✅ TH_TalentoHumano está 100% funcional
- [ ] ✅ Rama `feature/py-controlcalidad` creada

**Acción**: ✅ Completar checklist antes de Lunes 16-Ene

---

## 📚 DOCUMENTACIÓN DISPONIBLE

**Análisis Completo**: [ANALISIS_PY_CONTROLCALIDAD.md](../PY/ANALISIS_PY_CONTROLCALIDAD.md)
- 6 páginas detalladas
- Funcionalidades por página
- Tablas SQL identificadas
- Stored Procedures mapeados
- Riesgos y mitigaciones

**Plan de Implementación**: [PLAN_MIGRACION_PY_CONTROLCALIDAD.md](../PY/PLAN_MIGRACION_PY_CONTROLCALIDAD.md)
- 6 Épicas detalladas
- Desglose hora x hora
- Criterios de aceptación
- Checklist pre-implementación

**Directrices Obligatorias**: [DIRECTRICES_MIGRACION.md](../../DIRECTRICES_MIGRACION.md)
- 15 reglas para garantizar consistencia
- Patrones de código
- Estructura de carpetas
- Ejemplo completo de migración

---

## 🚀 PRÓXIMOS PASOS

### Antes del Kickoff (Hoy 15-Ene)

1. [ ] Revisar [ANALISIS_PY_CONTROLCALIDAD.md](../PY/ANALISIS_PY_CONTROLCALIDAD.md)
2. [ ] Revisar [PLAN_MIGRACION_PY_CONTROLCALIDAD.md](../PY/PLAN_MIGRACION_PY_CONTROLCALIDAD.md)
3. [ ] Revisar [DIRECTRICES_MIGRACION.md](../../DIRECTRICES_MIGRACION.md)
4. [ ] Asignar equipo (Dev 1, Dev 2, Dev 3)
5. [ ] Verificar prerequisitos (BD, SP, FK)
6. [ ] Crear rama `feature/py-controlcalidad` en git

### En el Kickoff (Lunes 16-Ene, 09:00)

1. [ ] Presentar alcance y objetivos
2. [ ] Distribuir tareas por épica
3. [ ] Resolver dudas técnicas
4. [ ] Iniciar Épica 1 (Infraestructura)

### Monitoreo (Diario)

- [ ] Daily standup: 09:00-09:15
- [ ] Checkpoint: Viernes 17:00 (fin de semana 1)
- [ ] Ajustes: Lunes 10:00 (semana 2)

---

## ⚠️ RIESGOS IDENTIFICADOS

| Riesgo | Probabilidad | Impacto | Mitigación | Responsable |
|--------|-------------|--------|-----------|-------------|
| SP no existen | 🟡 Media | 🔴 Alto | Verificar antes de Lunes | Dev 1 |
| Auditoría incompleta | 🟢 Baja | 🟠 Medio | Testing verifica logs | Dev 3 |
| Performance: grid 1000+ | 🟢 Baja | 🟠 Medio | Paginación servidor | Dev 2 |
| Grid dinámico complejo | 🟡 Media | 🟠 Medio | EditorTemplate + testing | Dev 2 |

---

## 📞 CONTACTO Y ESCALACIÓN

- **Project Manager**: [TBD]
- **Tech Lead**: [TBD]
- **QA Lead**: [TBD]

**Escalación**: Reportar bloqueadores antes de 16:00 cada día.

---

## ✅ CHECKLIST ANTES DE INICIAR

- [ ] Análisis leído y comprendido (30 min)
- [ ] Plan de implementación leído (30 min)
- [ ] Directrices de migración memorizadas (1 h)
- [ ] Equipo conoce las dependencias (Proyectos, TH)
- [ ] BD verificada (SP, FK, tablas)
- [ ] Rama git creada (`feature/py-controlcalidad`)
- [ ] Reunión kickoff confirmada (Lunes 09:00)
- [ ] Equipos asignados
- [ ] Visual Studio con solución actualizada

---

## 🎯 ÉXITO DEL SPRINT

**Este sprint será exitoso cuando**:

1. ✅ Todas las funcionalidades migradas (6/6)
2. ✅ Build 0 errores
3. ✅ QA 100% completado
4. ✅ Documentación actualizada
5. ✅ Menú integrado
6. ✅ Auditoría funcional
7. ✅ Módulo en staging lista para production

---

## 📊 MÉTRICAS A RASTREAR

- LOC generadas
- Errores encontrados/corregidos
- Horas reales vs estimadas
- Cobertura de testing
- Checklist completado (%)
- Bugs resueltos

---

**Documento**: SPRINT_12_KICKOFF_PY_CONTROLCALIDAD.md  
**Versión**: 1.0  
**Fecha**: 2026-01-15  
**Autor**: GitHub Copilot  

---

## 🚀 ¡LISTOS PARA COMENZAR!

Reunión Kickoff: **Lunes 16 de Enero, 09:00**

Documentación lista. Equipo asignado. ¡A producción! 🎉
