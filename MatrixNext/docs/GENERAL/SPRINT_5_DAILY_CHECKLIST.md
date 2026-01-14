# 📋 SPRINT 5 - DAILY CHECKLIST

**Sprint**: 5 - TH_TalentoHumano Views/UI  
**Periodo**: 15-29 enero 2026  
**Dev Lead**: [Asignar]  
**Status**: 🟡 IN PROGRESS

---

## 📅 SEMANA 1 (15-19 enero)

### ✅ Día 1 (martes 15 enero) - Planning & Inventario

**Inicio**: 09:00 | **Estimado**: 8h | **Status**: 🟡

**Tareas**:
- [ ] (1h) Kick-off meeting - Review SPRINT_5_KICKOFF.md
- [ ] (1h) Leer PLAN_EJECUCION_SPRINTS_5_12.md (Sprint 5 section)
- [ ] (2h) Inventory del legacy en WebMatrix/TH_TalentoHumano
  - [ ] Listar todas las vistas .aspx (EmpleadosAdmin, Nomina, Desvinculaciones, etc.)
  - [ ] Crear archivo Inventario_TH_Views.xlsx
- [ ] (2h) Mapear legacy → API endpoints
  - [ ] Crear tabla Excel: Vista Legacy | Función | Endpoint API | Método HTTP
  - [ ] Completar para todos los 55 endpoints
- [ ] (1h) Diagrama de flujos AJAX (draw.io o similar)
- [ ] (1h) Confirm 55 endpoints en EmpleadosController + Services

**Entregables**:
- [ ] Inventario_TH_Views.xlsx
- [ ] Diagrama AJAX_Flows.png
- [ ] Checklist de vistas a crear

**Verificación**:
- [ ] 55 endpoints confirmados en API
- [ ] Mapeo 1:1 legacy ↔ endpoint
- [ ] Diagramas claros

**Notas**:
```

```

---

### 📋 Día 2 (miércoles 16 enero) - Views Razor Base

**Inicio**: 09:00 | **Estimado**: 8h | **Status**: 🟡

**Tareas**:
- [ ] (3h) EmpleadosIndex.cshtml
  - [ ] DataTable con columnas: ID, Nombres, Apellidos, Identificación, Cargo, Área, Estado, Acciones
  - [ ] Filtros HTML (no funcionales aún)
  - [ ] Paginación HTML
  - [ ] Botones: Crear, Editar, Ver Detalles, Retiro
- [ ] (2h) _AjaxModalEmpleado.cshtml (partial)
  - [ ] Modal Bootstrap con Tabs (General/Laboral/Personal/Nómina/Salario)
  - [ ] Form inputs para cada tab
  - [ ] Validaciones HTML5
- [ ] (2h) EmpleadosDetails.cshtml
  - [ ] Tabs de información
  - [ ] Links a nested resources
  - [ ] Actions buttons
- [ ] (1h) Build & verificación

**Entregables**:
- [ ] EmpleadosIndex.cshtml
- [ ] _AjaxModalEmpleado.cshtml
- [ ] EmpleadosDetails.cshtml

**Verificación**:
- [ ] Build sin errores
- [ ] Views cargan en navegador
- [ ] HTML estructura correcta

**Bloqueadores**:
- [ ] Nada por ahora

**Notas**:
```

```

---

### 📋 Día 3 (jueves 17 enero) - Nested Resources Views

**Inicio**: 09:00 | **Estimado**: 8h | **Status**: 🟡

**Tareas**:
- [ ] (1h) ExperienciasLaboral views
  - [ ] Index.cshtml (datatable)
  - [ ] _CreateEdit.cshtml (modal)
- [ ] (1h) Educacion views
  - [ ] Index.cshtml
  - [ ] _CreateEdit.cshtml
- [ ] (1h) Hijos views
  - [ ] Index.cshtml
  - [ ] _CreateEdit.cshtml
- [ ] (1h) ContactosEmergencia views
  - [ ] Index.cshtml
  - [ ] _CreateEdit.cshtml
- [ ] (1h) Promociones views
  - [ ] Index.cshtml
  - [ ] _CreateEdit.cshtml
- [ ] (1h) Salarios views
  - [ ] Index.cshtml
  - [ ] _CreateEdit.cshtml
- [ ] (1h) Build & verificación

**Entregables**:
- [ ] 14 archivos (7 Index + 7 modales)

**Verificación**:
- [ ] Build sin errores
- [ ] Estructura HTML correcta
- [ ] Modals abren en navegador

**Bloqueadores**:
- [ ] Nada por ahora

**Notas**:
```

```

---

### 📋 Día 4 (viernes 18 enero) - AJAX Integration Parte 1

**Inicio**: 09:00 | **Estimado**: 8h | **Status**: 🟡

**Tareas**:
- [ ] (5h) empleados.js
  - [ ] loadEmpleados() - GET /api/th/empleados?pageSize=20
  - [ ] createEmpleado() - POST /api/th/empleados
  - [ ] updateEmpleado(id) - PUT /api/th/empleados/{id}
  - [ ] deleteEmpleado(id) - DELETE /api/th/empleados/{id}
  - [ ] Manejo ApiResponse<T>
  - [ ] Toasts (success/error/warning)
  - [ ] Validaciones client-side
  - [ ] Paginación
  - [ ] Filtros
- [ ] (2h) Integrar con EmpleadosIndex.cshtml
  - [ ] Botón "Crear" → abre modal
  - [ ] Botón "Editar" → carga datos + modal
  - [ ] Botón "Eliminar" → confirm + DELETE
- [ ] (1h) Testing en navegador + console

**Entregables**:
- [ ] empleados.js (200+ LOC)
- [ ] EmpleadosIndex.cshtml actualizado

**Verificación**:
- [ ] F12 Network: llamadas AJAX funcionando
- [ ] Console: sin errores
- [ ] CRUD básico funciona

**Bloqueadores**:
- [ ] ¿API retorna datos correctamente?

**Notas**:
```

```

---

### 📋 Día 5 (lunes 21 enero) - AJAX Integration Parte 2 & Validaciones

**Inicio**: 09:00 | **Estimado**: 8h | **Status**: 🟡

**Tareas**:
- [ ] (4h) nestedResources.js
  - [ ] Funciones genéricas loadItems(endpoint)
  - [ ] createItem(endpoint, data)
  - [ ] updateItem(endpoint, id, data)
  - [ ] deleteItem(endpoint, id)
  - [ ] Manejo ApiResponse<T> reutilizable
- [ ] (2h) Integrar nested resources en EmpleadosDetails
  - [ ] Experiencias: load/create/edit/delete
  - [ ] Educación: load/create/edit/delete
  - [ ] Hijos: load/create/edit/delete
  - [ ] (Contactos, Promociones, Salarios si hay tiempo)
- [ ] (1h) Validaciones avanzadas
  - [ ] Fechas: inicio ≤ fin
  - [ ] Montos: > 0
  - [ ] Required fields
- [ ] (1h) Testing

**Entregables**:
- [ ] nestedResources.js (150+ LOC)
- [ ] Todas las vistas con AJAX funcionando

**Verificación**:
- [ ] F12: llamadas AJAX OK
- [ ] Validaciones client-side OK
- [ ] Toasts mostrándose

**Bloqueadores**:
- [ ] ¿Todos los 55 endpoints disponibles?

**Notas**:
```

```

---

## 📅 SEMANA 2 (22-29 enero)

### 📋 Día 6 (martes 22 enero) - Desvinculaciones & Catálogos

**Inicio**: 09:00 | **Estimado**: 8h | **Status**: 🟡

**Tareas**:
- [ ] (3h) DesvinculacionesIndex.cshtml
  - [ ] Listado con filtros
  - [ ] Paginación
  - [ ] Botones (Crear, Ver Evaluaciones, Descargar PDF)
  - [ ] Workflow visual
- [ ] (2h) _AjaxModalDesvinculacion.cshtml
  - [ ] Formulario para iniciar proceso
  - [ ] Campos: empleado, fecha retiro, observaciones
  - [ ] Validaciones
- [ ] (2h) CatalogosIndex.cshtml + catalogos.js
  - [ ] 13 dropdowns (Áreas, Cargos, Bandas, etc.)
  - [ ] loadCatalogos() function
  - [ ] Cache en localStorage
- [ ] (1h) Build & testing

**Entregables**:
- [ ] DesvinculacionesIndex.cshtml
- [ ] _AjaxModalDesvinculacion.cshtml
- [ ] CatalogosIndex.cshtml
- [ ] catalogos.js (80+ LOC)

**Verificación**:
- [ ] Catálogos cargan correctamente
- [ ] Dropdowns poblados
- [ ] Modales funcionan

**Bloqueadores**:
- [ ] ¿Endpoints de desvinculación y catálogos OK?

**Notas**:
```

```

---

### 📋 Día 7 (miércoles 23 enero) - UI/UX Polish

**Inicio**: 09:00 | **Estimado**: 8h | **Status**: 🟡

**Tareas**:
- [ ] (2h) Breadcrumbs en todas las vistas
  - [ ] Home → TH → Empleados → [action]
  - [ ] Home → TH → Desvinculaciones → [action]
- [ ] (2h) Sidebar Navigation
  - [ ] Links a Empleados, Desvinculaciones, Nómina (si existe)
  - [ ] Links a Catálogos (admin)
  - [ ] Active states
- [ ] (2h) Responsive design
  - [ ] DataTables responsive en mobile
  - [ ] Modales adaptativos
  - [ ] Pruebas en Chrome DevTools (tablet + mobile)
- [ ] (1h) Browser testing
  - [ ] Chrome, Firefox, Edge
  - [ ] Console: sin errores
- [ ] (1h) [Authorize] validation

**Entregables**:
- [ ] Layout actualizado
- [ ] Sidebar navigation funcional
- [ ] Responsive OK

**Verificación**:
- [ ] Build sin warnings
- [ ] No 404s en navigation
- [ ] Responsive: 1366x768, 768x1024, 375x667

**Bloqueadores**:
- [ ] Nada

**Notas**:
```

```

---

### 📋 Día 8 (jueves 24 enero) - Reportes & Exportes

**Inicio**: 09:00 | **Estimado**: 8h | **Status**: 🟡

**Tareas**:
- [ ] (3h) EmpleadosReportes.cshtml
  - [ ] Botones: Descargar Empleados (Excel)
  - [ ] Botones: Descargar Nómina (Excel)
  - [ ] Filtros (área, cargo, estado, rango fecha)
- [ ] (3h) reportes.js
  - [ ] exportarEmpleadosExcel(filters)
  - [ ] exportarNominaExcel(filters)
  - [ ] Manejo de descarga (blob → file)
- [ ] (2h) Controllers - Endpoints de exporte
  - [ ] GET /api/th/empleados/export/excel
  - [ ] GET /api/th/nomina/export/excel
  - [ ] Usar ClosedXML para generar
  - [ ] Validar permisos

**Entregables**:
- [ ] EmpleadosReportes.cshtml
- [ ] reportes.js (50+ LOC)
- [ ] 2-3 métodos en Controllers

**Verificación**:
- [ ] Descargas funcionan (file en disco)
- [ ] Excel format correcto
- [ ] Filtros reflejados en exports

**Bloqueadores**:
- [ ] ¿ClosedXML disponible?

**Notas**:
```

```

---

### 📋 Día 9 (viernes 25 enero) - QA Funcional Parte 1

**Inicio**: 09:00 | **Estimado**: 8h | **Status**: 🟡

**Tareas**:
- [ ] (6h) Smoke Tests
  - [ ] EmpleadosIndex carga, lista OK
  - [ ] Create empleado: modal abre, POST OK
  - [ ] Edit empleado: datos cargan, PUT OK
  - [ ] Delete empleado: DELETE OK
  - [ ] Nested resources CRUD
  - [ ] Desvinculaciones flujo
  - [ ] Catálogos cargan
  - [ ] Reportes descargan
  - [ ] Documentar en QA_SPRINT5_SMOKE_TESTS.xlsx
- [ ] (2h) Data Validation Tests
  - [ ] Nombres > 3 caracteres (validar error)
  - [ ] Identificación > 0
  - [ ] Salarios > 0
  - [ ] Fechas: inicio ≤ fin

**Entregables**:
- [ ] QA_SPRINT5_SMOKE_TESTS.xlsx (30+ test cases)

**Verificación**:
- [ ] Todos los tests documentados
- [ ] Status: PASS/FAIL registrados
- [ ] Console limpio

**Bloqueadores**:
- [ ] ¿Todos los endpoints OK?

**Notas**:
```

```

---

### 📋 Día 10 (lunes 28 enero) - QA Funcional Parte 2 & Bug Fixes

**Inicio**: 09:00 | **Estimado**: 8h | **Status**: 🟡

**Tareas**:
- [ ] (3h) Ejecutar QA plan completo
  - [ ] Correr todos los test cases
  - [ ] Documentar resultados en QA_SPRINT5_RESULTS.xlsx
  - [ ] Crear BUGS_ENCONTRADOS_SPRINT5.md
- [ ] (3h) Bug Fixes
  - [ ] Fijar bugs Critical/High
  - [ ] Documentar Medium/Low para Sprint 6
- [ ] (1h) Performance Testing
  - [ ] EmpleadosIndex < 2s (1000 registros)
  - [ ] Modales < 500ms
  - [ ] Búsquedas < 1s
- [ ] (1h) Data Integrity Tests
  - [ ] Create → Edit → Delete flujo

**Entregables**:
- [ ] QA_SPRINT5_RESULTS.xlsx
- [ ] BUGS_ENCONTRADOS_SPRINT5.md
- [ ] PERFORMANCE_RESULTS.txt

**Verificación**:
- [ ] >= 95% test cases PASS
- [ ] Bugs Críticos resueltos
- [ ] Performance OK

**Bloqueadores**:
- [ ] ¿Encontrar bugs críticos?

**Notas**:
```

```

---

### 📋 Día 11 (martes 29 enero) - Documentación & Cierre

**Inicio**: 09:00 | **Estimado**: 8h | **Status**: 🟡

**Tareas**:
- [ ] (4h) MIGRACION_TH_TALENTOHUMANO_VIEWS_COMPLETADA.md
  - [ ] Objetivo Sprint 5
  - [ ] Entregables (lista de views)
  - [ ] Mapeo pantalla → endpoint (tabla)
  - [ ] Arquitectura AJAX
  - [ ] QA Results
  - [ ] Bugs & Resolvidos
  - [ ] Performance Metrics
  - [ ] Próximos pasos
- [ ] (2h) Actualizar DASHBOARD_MIGRACION.md
  - [ ] TH → 🟢 COMPLETO
  - [ ] Sprint 6 → 🟡 PRÓXIMO
  - [ ] Fecha cierre
- [ ] (1h) Confirmar Sidebar/Navigation
  - [ ] Links OK
  - [ ] Permisos OK
- [ ] (1h) Commit de cierre Sprint 5
  - [ ] Mensaje: "feat(TH): Sprint 5 - Views/UI (14 files, 2,000+ LOC)"

**Entregables**:
- [ ] MIGRACION_TH_TALENTOHUMANO_VIEWS_COMPLETADA.md
- [ ] DASHBOARD_MIGRACION.md actualizado
- [ ] Commit de cierre

**Verificación**:
- [ ] Build final sin errores
- [ ] Documentación completa
- [ ] Commit refleja todos los cambios

**Bloqueadores**:
- [ ] Nada

**Notas**:
```

```

---

## 📊 RESUMEN SEMANAL

### Semana 1 (15-19 ene)
- [ ] Inventario completado
- [ ] 14 views Razor creadas (base)
- [ ] AJAX Parte 1 & 2 integrado
- [ ] **LOC creadas**: ~1,200
- [ ] **Status**: 🟡 On Track

### Semana 2 (22-29 ene)
- [ ] Desvinculaciones & Catálogos integrados
- [ ] UI/UX polish completo
- [ ] Reportes/Exportes funcionales
- [ ] QA Funcional >= 95% PASS
- [ ] Documentación de cierre
- [ ] **LOC creadas**: ~800
- [ ] **Status**: 🟡 On Track

**Total Sprint 5**: 25 archivos, 2,000+ LOC, 0 errores, 100% QA

---

## 🎯 MÉTRICAS EN VIVO

| Métrica | Objetivo | Actual | % |
|---|---|---|---|
| Views Razor | 14 | — | 0% |
| JavaScript | 4 archivos | — | 0% |
| QA Tests | 30+ | — | 0% |
| Test Pass Rate | 95%+ | — | 0% |
| Build Errors | 0 | — | 0% |
| Documentation | 100% | — | 0% |

---

## 📞 ESCALACIONES

**Bloqueador crítico**: Contact Product Owner  
**Issue con API**: Contact Tech Lead  
**Bug severidad**: Document en BUGS_ENCONTRADOS_SPRINT5.md

---

**Últime actualización**: 2026-01-15  
**Próxima revisión**: Diaria (EOD standup)  
**Sprint end**: 2026-01-29
