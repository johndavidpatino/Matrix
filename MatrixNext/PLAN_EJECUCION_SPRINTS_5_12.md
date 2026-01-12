# PLAN DE EJECUCIÓN - SPRINTS 5-12 (Módulos Parciales → 100%)

**Fecha de corte**: 2026-01-15  
**Estado Global**: 🟢 Sprint 4 TH API completado; 🟡 Sprints 5-12 planificados

---

## RESUMEN EJECUTIVO

| Sprint | Módulo | Duración | Esfuerzo | Inicio Est. | Cierre Est. | Status |
|---|---|---|---|---|---|---|
| **5** | TH Views/UI | 2 sem | 80h | 2026-01-15 | 2026-01-29 | 🟡 PLANIFICADO |
| **6** | OP_Cualitativo Complementos | 2 sem | 75h | 2026-02-01 | 2026-02-12 | 🟡 PLANIFICADO |
| **7** | CORE Workflow | 2 sem | 85h | 2026-02-15 | 2026-02-26 | 🟡 PLANIFICADO |
| **8** | EQ_EasyQuote Fase 1 | 2-3 sem | 120h | 2026-03-01 | 2026-03-19 | 🟡 PLANIFICADO |
| **9** | Home Dashboard | 1-2 sem | 50h | 2026-03-22 | 2026-04-02 | 🔴 PLANIFICADO |
| **10** | RP_Reportes | 1-2 sem | 60h | 2026-04-05 | 2026-04-16 | 🔴 PLANIFICADO |
| **11** | OP_RO + OP_Trafico | 2 sem | 90h | 2026-04-19 | 2026-05-03 | 🔴 PLANIFICADO |
| **12+** | Módulos Baja Prioridad | Variable | TBD | 2026-05-05+ | TBD | ⚪️ PENDIENTE |

**Total Esfuerzo (Sprints 5-11)**: ~560 horas = ~14 semanas = ~3.5 meses

---

## DETALLES POR SPRINT

### SPRINT 5: TH_TalentoHumano Views/UI
**Duración**: 2 semanas  
**Esfuerzo**: 80 horas  
**Prioridad**: 🟠 Media  
**Dependencia**: Sprint 4 TH API ✅ LISTA

**Objetivo**: Implementar vistas Razor + AJAX para Empleados, Nómina, Desvinculaciones, Complementarios sobre API REST ya completada

**Entregables**:
- ✅ Views para CRUD Empleados (Index, Create/Edit con modales)
- ✅ Views para nested resources (Experiencias, Educación, Hijos, Contactos, Promociones, Salarios)
- ✅ Workflow UI de Desvinculaciones (pasos visuales)
- ✅ Nómina UI con cálculos/reportes
- ✅ Reportes/exportes (Excel/PDF) con ClosedXML
- ✅ Integración AJAX con 55 endpoints API
- ✅ Sidebar/breadcrumbs actualizados

**Criterio de terminado**: 
- 100% paridad UI con legacy TH_TalentoHumano/EmpleadosAdmin
- QA funcional completo en staging
- Documento `MIGRACION_TH_TALENTOHUMANO_VIEWS_COMPLETADA.md`
- Dashboard actualizado a 🟢 TH COMPLETO

---

### SPRINT 6: OP_Cualitativo Complementos
**Duración**: 2 semanas  
**Esfuerzo**: 75 horas  
**Prioridad**: 🔴 Alta  
**Dependencia**: Sprint 5 MVP base (puede solaparse 50%)

**Objetivo**: Cerrar funcionalidades faltantes de OP_Cualitativo (reportes, filtros avanzados, validaciones)

**Entregables**:
- ✅ Reportes de sesiones/entrevistas/moderadores (Excel/PDF)
- ✅ Filtros avanzados (autocomplete, date ranges, multi-select)
- ✅ Validaciones de concurrencia (sesiones simultáneas)
- ✅ Notificaciones de cambios de estado
- ✅ Exportes con formatos definidos
- ✅ Optimizaciones de performance (queries lentas)

**Criterio de terminado**: 
- 🟢 OP_Cualitativo COMPLETO (100% paridad)
- QA de reportes y edge cases
- Documento de cierre actualizado
- Dashboard marca OP_Cualitativo como 🟢 COMPLETO

---

### SPRINT 7: CORE Workflow/Tareas
**Duración**: 2 semanas  
**Esfuerzo**: 85 horas  
**Prioridad**: 🔴 Alta  
**Dependencia**: Sprints 5-6 parcialmente avanzados (para resolver dependencias cruzadas)

**Objetivo**: Resolver workflow de tareas y dependencias que bloquean otros módulos

**Entregables**:
- ✅ API REST de tareas (crear, asignar, cerrar, escalar)
- ✅ Notificaciones automáticas (email/toast)
- ✅ Integración con PY/OP/TH/GD (si lo requieren)
- ✅ Validaciones de estado y transiciones
- ✅ Vistas para gestión de tareas (dashboard personal)
- ✅ Permisos y auditoría

**Criterio de terminado**: 
- 🟢 CORE WORKFLOW COMPLETO
- Todas las dependencias cruzadas resueltas
- QA de workflows complejos
- Documento de cierre
- Dashboard actualizado

---

### SPRINT 8: EQ_EasyQuote Fase 1 (Análisis + Catálogos)
**Duración**: 2-3 semanas  
**Esfuerzo**: 120 horas (70h análisis + 50h implementación)  
**Prioridad**: 🔴🔴 CRÍTICA  
**Dependencia**: CU_Cuentas (para referencias); ninguna bloqueante

**Objetivo**: Análisis completo de EasyQuote (módulo crítico muy complejo) + implementación Fase 1

**Análisis (Semana 1)**:
- ✅ Inventario completo: páginas, WebMethods, BusinessLogic
- ✅ Mapeo exacto: SP → tablas → parámetros → tipos (especial atención a presupuestos/alternativas)
- ✅ Documento `ANALISIS_EQ_EASYQUOTE.md` similar a `ANALISIS_OP_CUANTITATIVO.md`
- ✅ Identificar patrones complejos (cálculos de presupuestos, simulador, cotización)
- ✅ Definir arquitectura API (endpoints, DTOs, flujos)
- ✅ Crear backlog desglosado Fases 1-4 con t-shirt sizing

**Implementación Fase 1 (Semana 1-2)**:
- ✅ Infraestructura: adapters/services base
- ✅ Catálogos: tipos presupuesto, modalidades, formatos, empresas
- ✅ CRUD Presupuestos (sin alternativas aún)
- ✅ Vistas básicas (Index, Create/Edit)

**Criterio de terminado**: 
- 📊 Análisis completo documentado en ANALISIS_EQ_EASYQUOTE.md
- 🟡 Fase 1 implementada (catálogos e infraestructura)
- Backlog de Fases 2-4 definido (alternativas, simulador, aprobaciones)
- Dashboard marca EQ como 🟡 EN PROGRESO (Fase 1 completa)
- Preparado para Sprint 8b si continúa o pausa para otros módulos

---

### SPRINT 9: Home Dashboard
**Duración**: 1-2 semanas  
**Esfuerzo**: 50 horas  
**Prioridad**: 🔴 Alta  
**Dependencia**: Sprints 5-8 mayormente completados (para consumir datos)

**Objetivo**: Migrar Home.aspx con todos los widgets y datos de múltiples módulos

**Entregables**:
- ✅ Dashboard principal con widgets de resumen (PY, OP, CU, TH, FI, GD, etc.)
- ✅ Gráficos/KPIs de producción, ventas, tareas pendientes, ausencias
- ✅ Links contextuales a módulos (mantienen permisos)
- ✅ Filtros dinámicos (fecha, área, usuario si aplica)
- ✅ Performance optimizado (caching, queries eficientes)
- ✅ Responsive design

**Criterio de terminado**: 
- Home carga en < 2 segundos
- 100% widgets funcionando con datos reales
- QA multi-rol (validar visibilidad por permisos)
- Documento de cierre
- Dashboard actualizado

---

### SPRINT 10: RP_Reportes
**Duración**: 1-2 semanas  
**Esfuerzo**: 60 horas  
**Prioridad**: 🔴 Alta  
**Dependencia**: Sprints 5-9 completados (para datos a reportar)

**Objetivo**: Migrar todos los reportes con opciones de exporte (Excel/PDF)

**Entregables**:
- ✅ Inventario de reportes legacy (lista completa)
- ✅ Endpoints REST para cada reporte
- ✅ Vistas con formularios de filtro (date pickers, dropdowns, autocomplete)
- ✅ Exportes a Excel (ClosedXML con estilos)
- ✅ Exportes a PDF (iText o similar)
- ✅ Paginación en reportes grandes
- ✅ Validaciones de permisos por rol

**Criterio de terminado**: 
- 100% reportes legacy migrados
- Exportes funcionando en Excel y PDF
- QA de filtros, paginación, exportes
- Documento de cierre
- Dashboard actualizado a 🟢 RP COMPLETO

---

### SPRINT 11: OP_RO + OP_Trafico
**Duración**: 2 semanas  
**Esfuerzo**: 90 horas (45h cada módulo)  
**Prioridad**: 🟠 Baja  
**Dependencia**: Sprints 5-10 completados (para validar integraciones)

**Objetivo**: Migrar módulos operacionales (OP_RO y OP_Trafico)

**OP_RO (Revisión Operacional)**:
- ✅ CRUD revisiones operacionales
- ✅ Asignaciones y cambios de estado
- ✅ Aprobaciones y flujo de validación
- ✅ Reportes/exportes

**OP_Trafico (Gestión de Tráfico)**:
- ✅ Dashboard de tráfico (asignaciones, estado)
- ✅ CRUD de rutas/asignaciones
- ✅ Cambios de estado y notificaciones
- ✅ Reportes de tráfico

**Criterio de terminado**: 
- 🟢 OP_RO COMPLETO
- 🟢 OP_Trafico COMPLETO
- Integraciones validadas con OP_Cuantitativo/Cualitativo
- QA completo
- Dashboard actualizado (ambos 🟢)

---

### SPRINT 12+: Módulos de Baja Prioridad
**Duración**: Variable  
**Prioridad**: 🟢 Baja  
**Dependencia**: Todos los anteriores completados

**Módulos pendientes** (por prioridad de negocio):
- PY_ControlCalidad (control de calidad de proyectos)
- PY_Adquisiciones (compras/adquisiciones)
- SG_Actas (actas de reunión)
- SGC_Calidad (sistema de gestión de calidad)
- ES_Estadistica (estadísticas)
- Centro_Informacion (centro de información)
- IT (informática/soporte)
- MBO* (objetivos - MBO_Gerencial, MBO_Operaciones)
- ResumenProduccion (resumen de producción)
- RE_GT (relaciones)
- PC_PropiedadCliente (propiedad del cliente)
- **Inventario** (excluido de FI - evaluar relevancia)

**Pasos para cada módulo**:
1. Ejecutar inventario + mapeo SP/tablas
2. Implementar adapters/services/controllers siguiendo patrón
3. QA funcional
4. Documentar cierre

---

## ROADMAP VISUAL

```
2026-01-15 -------- 2026-01-29          Sprint 5: TH Views/UI (80h)
                                  \
2026-02-01 -------- 2026-02-12          Sprint 6: OP_Cualitativo Complementos (75h)
                             |
2026-02-15 -------- 2026-02-26          Sprint 7: CORE Workflow (85h)
                                  \
2026-03-01 -------- 2026-03-19          Sprint 8: EQ_EasyQuote Fase 1 (120h)
                                  |
2026-03-22 -------- 2026-04-02          Sprint 9: Home Dashboard (50h)
                             |
2026-04-05 -------- 2026-04-16          Sprint 10: RP_Reportes (60h)
                                  \
2026-04-19 -------- 2026-05-03          Sprint 11: OP_RO + OP_Trafico (90h)
                                  |
2026-05-05 -------- TBD                  Sprint 12+: Módulos Baja Prioridad (Variable)

📊 HITO: 2026-05-03 = FIN SPRINTS 5-11 (100% módulos alta/media prioridad completados)
```

---

## MATRIZ DE DECISIÓN

**¿Ejecutar Sprints 5-11 en secuencial o paralelo?**

| Escenario | Pros | Contras | Recomendación |
|---|---|---|---|
| **Secuencial** (1 sprint a la vez) | Calidad alta, menos overhead, enfoque total | Toma ~14 sem, retrasa entrega | ✅ RECOMENDADO (evita context switch) |
| **Paralelo 2 sprints** (ej. 5+6+7) | Acelera entrega (~8 sem) | Requiere 2 devs, riesgo de dependencias | ⚠️ SI HAY RECURSOS |
| **Híbrido** (secuencial prioridad alta, paralelo baja) | Balance de velocidad y calidad | Complejidad de coordinación | ✅ MEJOR OPCIÓN |

**Propuesta HÍBRIDA**:
- **Eje crítico secuencial**: Sprint 5 → 6 → 7 (Sprint 8 puede iniciar cuando Sprint 7 esté 50%)
- **Paralelo permitido**: Sprint 8 (EQ análisis) + Sprint 9 (Home) + Sprint 10 (Reportes) una vez Sprint 7 avanzado
- **Sprint 11 después de 10** (validar integraciones)

---

## RIESGOS Y MITIGACIONES

| Riesgo | Probabilidad | Impacto | Mitigación |
|---|---|---|---|
| Cambios de scope en Sprint 8 (EQ análisis complejo) | 🔴 Alta | 🔴 Alto | Cerrar análisis temprano; crear backlog de Fases 2-4; decir "NO" a nuevas features |
| Dependencias entre sprints encontradas tarde | 🟡 Media | 🟡 Medio | Mapeo de dependencias en previa de cada sprint |
| OP_RO/Trafico descubren gaps en OP_Cuantitativo/Cualitativo | 🟡 Media | 🟡 Medio | Validación cruzada en Sprint 11 previa; QA coordenado |
| Home/Reportes requieren cambios en APIs previas | 🟡 Media | 🟠 Bajo-Medio | Versionamiento de APIs; compatibilidad backward |
| Recursos insuficientes para ejecutar sprints | 🟡 Media | 🔴 Alto | Priorizar secuencial (Sprint 5-7) antes de paralelo |

---

## CHECKLIST DE CONTROL

### Pre-Sprint (Antes de iniciar cada sprint)
- [ ] Inventario completo del módulo (páginas, WebMethods, SP)
- [ ] Mapeo acción → SP → parámetros documentado
- [ ] Backlog de historias desglosado y priorizado
- [ ] Recursos asignados y disponibles
- [ ] Dependencias externas validadas

### During Sprint (Diario)
- [ ] Standup diario de 15 min
- [ ] Actualización de progreso en DASHBOARD_MIGRACION.md
- [ ] Bloqueadores escalados inmediatamente

### End of Sprint (Cierre)
- [ ] 100% funcionalidad implementada y testeada
- [ ] QA funcional completo (staging)
- [ ] Documento de cierre generado (`MIGRACION_[MODULO]_COMPLETADA.md`)
- [ ] Dashboard actualizado con nuevo estado
- [ ] Commit de cierre con evidencias
- [ ] Retrospectiva de Sprint (qué fue bien, qué mejorar)

---

## CONTACTS & ESCALACIÓN

- **Sprint Lead**: [Asignar responsable por sprint]
- **QA Lead**: [Asignar responsable QA]
- **Architecture Review**: Revisar diseño antes de Sprint 5 start
- **Escalación**: Cualquier bloqueador crítico → Product Owner + Tech Lead

---

**Documento fuente**: [MatrixNext/docs/GENERAL/BACKLOG_MIGRACION_GLOBAL.md](MatrixNext/docs/GENERAL/BACKLOG_MIGRACION_GLOBAL.md)  
**Última actualización**: 2026-01-15  
**Próxima revisión**: Después de Sprint 5 cierre (2026-01-29)
