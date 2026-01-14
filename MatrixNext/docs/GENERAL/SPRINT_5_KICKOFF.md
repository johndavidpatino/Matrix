# 🚀 SPRINT 5 KICKOFF - TH_TalentoHumano Views/UI

**Duración**: 2 semanas (15-29 enero 2026)  
**Esfuerzo**: 80 horas  
**Prioridad**: 🟠 Media  
**Status**: ✅ QA PASSED

---

## 📋 OBJETIVO

Implementar Views Razor + AJAX para Empleados, Nómina, Desvinculaciones y complementarios sobre API REST Sprint 4 (55 endpoints ya completados).

**Resultado esperado**: 100% paridad UI con legacy TH_TalentoHumano.

---

## ✅ DELIVERABLES PRINCIPALES

- [ ] **EmpleadosIndex.cshtml** - Listado con filtros, paginación
- [ ] **EmpleadosCreate.cshtml** - Modal para crear empleado
- [ ] **EmpleadosEdit.cshtml** - Modal para editar datos (General/Laboral/Personal/Nómina/Salario)
- [ ] **Nested Resources Views** - Experiencias, Educación, Hijos, Contactos, Promociones, Salarios (datatables + botones CRUD)
- [ ] **DesvinculacionesIndex.cshtml** - Listado y workflow visual
- [ ] **CatalogosIndex.cshtml** - Dropdown selector compartido para catálogos
- [ ] **AJAX/JavaScript Integration** - fetch calls a 55 endpoints, validaciones, toasts
- [ ] **Reportes/Exportes** - Excel (ClosedXML) para empleados y nómina
- [ ] **Sidebar Navigation** - Links a TH completo en layout
- [ ] **QA Documentación** - MIGRACION_TH_TALENTOHUMANO_VIEWS_COMPLETADA.md

---

## 📅 TIMELINE POR DÍA

### **SEMANA 1 (15-19 enero)**

#### **Día 1 (martes 15 ene) - Planning & Inventario**
- [ ] **Duración**: 8h
- [ ] Tareas:
  - Leer PLAN_EJECUCION_SPRINTS_5_12.md completo
  - Listar todas las vistas legacy en [WebMatrix/TH_TalentoHumano](WebMatrix/TH_TalentoHumano)
    - [ ] EmpleadosAdmin.aspx
    - [ ] Crear inventario completo (20+ vistas aprox)
  - Mapear pantalla legacy → endpoint API (tabla Excel)
    - [ ] Empleados List → GET /api/th/empleados
    - [ ] Empleados Create → POST /api/th/empleados
    - [ ] Empleados Edit → PUT /api/th/empleados/{id}
    - ... (completar para 55 endpoints)
  - Confirmar 55 endpoints disponibles en EmpleadosController/DesvinculacionesController/CatalogosController
  - Crear diagrama de flujos AJAX (EmpleadosIndex → Create/Edit/Delete/Filter)
- [ ] **Entregables**:
  - [ ] Inventario_TH_Views.xlsx (legacy → endpoint mapping)
  - [ ] Diagrama de flujos AJAX
  - [ ] Checklist de vistas a migrar

#### **Día 2 (miércoles 16 ene) - Views Razor Base**
- [ ] **Duración**: 8h
- [ ] Tareas:
  - Crear **EmpleadosIndex.cshtml**
    - [ ] DataTable con 55+ columns (id, nombres, apellidos, identificación, cargo, área, estado, acciones)
    - [ ] Filtros: nombres, apellidos, identificación, estado, cargo
    - [ ] Paginación (pageSize: 20)
    - [ ] Botones: Crear, Editar, Ver Detalles, Retiro/Reintegro
    - [ ] Vincular a GET /api/th/empleados?nombres=...&apellidos=...
  - Crear **_AjaxModalEmpleado.cshtml** (partial)
    - [ ] Modal reutilizable para Create/Edit
    - [ ] Tabs: Datos Generales | Laborales | Personales | Nómina | Salario
    - [ ] Validaciones client-side (required, min/max)
  - Crear **EmpleadosDetails.cshtml** (view detail)
    - [ ] Tabs de información
    - [ ] Links a nested resources (Experiencias, Educación, etc.)
- [ ] **Entregables**:
  - [ ] EmpleadosIndex.cshtml (sin funcionalidad AJAX aún)
  - [ ] _AjaxModalEmpleado.cshtml
  - [ ] EmpleadosDetails.cshtml
- [ ] **Verificación**:
  - [ ] Build sin errores
  - [ ] Views cargan sin data (estructura HTML ok)

#### **Día 3 (jueves 17 ene) - Nested Resources Views**
- [ ] **Duración**: 8h
- [ ] Tareas:
  - Crear views para nested resources (reutilizar patrón):
    - [ ] ExperienciasLaboral/Index.cshtml (datatable)
    - [ ] ExperienciasLaboral/_CreateEdit.cshtml (modal)
    - [ ] Educacion/Index.cshtml + modal
    - [ ] Hijos/Index.cshtml + modal
    - [ ] ContactosEmergencia/Index.cshtml + modal
    - [ ] Promociones/Index.cshtml + modal
    - [ ] Salarios/Index.cshtml + modal
  - Cada view debe tener:
    - [ ] Botones: Agregar, Editar, Eliminar
    - [ ] Validaciones básicas (fechas, montos)
    - [ ] Links a endpoints (GET, POST, PUT, DELETE)
- [ ] **Entregables**:
  - [ ] 7 views principales + 7 modales = 14 archivos
- [ ] **Verificación**:
  - [ ] Build sin errores
  - [ ] Estructura HTML correcta

#### **Día 4 (viernes 18 ene) - AJAX Integration (Parte 1)**
- [ ] **Duración**: 8h
- [ ] Tareas:
  - Crear **empleados.js** (main AJAX file)
    - [ ] Función: loadEmpleados() → GET /api/th/empleados?pageSize=20&pageIndex=0
    - [ ] Función: createEmpleado() → POST /api/th/empleados
    - [ ] Función: updateEmpleado(id) → PUT /api/th/empleados/{id}
    - [ ] Función: deleteEmpleado(id) → DELETE /api/th/empleados/{id}
    - [ ] Validaciones client-side (nombres, identificación, montos)
    - [ ] Manejo de ApiResponse<T> (Success/Error)
    - [ ] Toasts (success/error/warning)
    - [ ] Paginación (siguiente/anterior)
    - [ ] Filtros (nombres, apellidos)
  - Integrar con EmpleadosIndex.cshtml
    - [ ] Botón "Crear" → abre modal
    - [ ] Botón "Editar" → carga datos + abre modal
    - [ ] Botón "Eliminar" → confirm + DELETE
    - [ ] Click en fila → cargar detalles
- [ ] **Entregables**:
  - [ ] empleados.js (200+ líneas)
  - [ ] EmpleadosIndex.cshtml funcionando (CRUD básico)
- [ ] **Verificación**:
  - [ ] Console.log sin errores
  - [ ] Llamadas AJAX ejecutándose (F12 Network)
  - [ ] Respuestas ApiResponse parseadas correctamente

#### **Día 5 (lunes 21 ene) - AJAX Integration (Parte 2) & Validaciones**
- [ ] **Duración**: 8h
- [ ] Tareas:
  - Crear **nestedResources.js** (compartido)
    - [ ] Funciones reutilizables para CRUD (Create/Edit/Delete)
    - [ ] Manejo genérico de ApiResponse<T>
    - [ ] Toasts standardizados
  - Integrar nested resources en EmpleadosDetails
    - [ ] Experiencias: load/create/edit/delete
    - [ ] Educación: load/create/edit/delete
    - [ ] Hijos: load/create/edit/delete
    - [ ] Contactos: load/create/edit/delete
    - [ ] Promociones: load/create/edit/delete
    - [ ] Salarios: load/create/edit/delete
  - Validaciones avanzadas:
    - [ ] Fechas: inicio ≤ fin
    - [ ] Montos: > 0
    - [ ] Required fields
    - [ ] Longitud mínima (nombres/apellidos)
  - Agregar toasts para todas las acciones
- [ ] **Entregables**:
  - [ ] nestedResources.js (150+ líneas)
  - [ ] Todas las vistas con AJAX funcionando
- [ ] **Verificación**:
  - [ ] QA manual: CRUD completo en Chrome DevTools
  - [ ] Validaciones funcionando
  - [ ] Toasts mostrándose

### **SEMANA 2 (22-29 enero)**

#### **Día 6 (martes 22 ene) - Desvinculaciones & Catalogs**
- [ ] **Duración**: 8h
- [ ] Tareas:
  - Crear **DesvinculacionesIndex.cshtml**
    - [ ] Listado con filtros (empleado, estado, fecha)
    - [ ] Paginación
    - [ ] Botones: Crear solicitud, Ver evaluaciones, Descargar PDF
    - [ ] Workflow visual (pasos: Iniciada → Pendiente → Finalizada)
  - Crear **_AjaxModalDesvinculacion.cshtml**
    - [ ] Formulario para iniciar proceso
    - [ ] Campos: empleado, fecha retiro, observaciones
    - [ ] Validaciones (fecha > hoy)
  - Crear **CatalogosIndex.cshtml** (compartido)
    - [ ] Dropdowns para: Áreas, Cargos, Bandas, Estados Civiles, Grupos Sanguíneos, Sedes, Tipos Contrato, Tiempos Contrato, Empresas, Job Functions, Parentescos, Motivos Cambio Salario, Tipos Salario
    - [ ] Cargar via GET /api/th/catalogos/* endpoints
    - [ ] Cache en JavaScript (reutilizar en modales)
- [ ] **Entregables**:
  - [ ] DesvinculacionesIndex.cshtml
  - [ ] _AjaxModalDesvinculacion.cshtml
  - [ ] CatalogosIndex.cshtml
  - [ ] catalogos.js con funciones loadAreaas(), loadCargos(), etc.
- [ ] **Verificación**:
  - [ ] Catálogos cargando correctamente
  - [ ] Dropdowns poblados en modales

#### **Día 7 (miércoles 23 ene) - UI/UX Polish**
- [ ] **Duración**: 8h
- [ ] Tareas:
  - Breadcrumbs en todas las vistas
    - [ ] Home → TH → Empleados → [action]
    - [ ] Home → TH → Desvinculaciones → [action]
  - Sidebar Navigation (actualizar layout)
    - [ ] Link a Empleados (EmpleadosIndex)
    - [ ] Link a Desvinculaciones
    - [ ] Link a Nómina (si existe)
    - [ ] Link a Catálogos (admin only)
  - Responsive design
    - [ ] DataTables responsive en mobile
    - [ ] Modales adaptativos
    - [ ] Botones accesibles
  - Permisos [Authorize]
    - [ ] Validar que controller tiene [Authorize]
    - [ ] Roles específicos si aplica (solo RRHH puede acceder ciertos datos)
    - [ ] Ocultar botones si no hay permiso (client-side checking)
  - Testing en múltiples browsers
    - [ ] Chrome, Firefox, Edge (si es posible)
    - [ ] DevTools: sin errores de console
- [ ] **Entregables**:
  - [ ] Layout actualizado con breadcrumbs
  - [ ] Sidebar navigation actualizado
  - [ ] Todas las views responsive
  - [ ] Testing checklist completado
- [ ] **Verificación**:
  - [ ] Build sin warnings
  - [ ] No errores 404 en navigation
  - [ ] Responsive en mobile (1366x768, 768x1024, 375x667)

#### **Día 8 (jueves 24 ene) - Reportes & Exportes**
- [ ] **Duración**: 8h
- [ ] Tareas:
  - Crear **EmpleadosReportes.cshtml**
    - [ ] Botón: Descargar Empleados (Excel)
    - [ ] Botón: Descargar Nómina (Excel)
    - [ ] Filtros: por área, cargo, estado, fecha rango
  - Implementar **reportes.js**
    - [ ] Función exportarEmpleadosExcel(filters) → GET /api/th/empleados/export/excel
    - [ ] Función exportarNominaExcel(filters) → GET /api/th/nomina/export/excel
    - [ ] Manejo de descarga (blob → file)
  - En Controllers, crear endpoints:
    - [ ] GET /api/th/empleados/export/excel → retorna ClosedXML file
    - [ ] GET /api/th/nomina/export/excel → retorna ClosedXML file
  - Validar permisos (solo RRHH puede exportar)
- [ ] **Entregables**:
  - [ ] EmpleadosReportes.cshtml
  - [ ] reportes.js
  - [ ] Endpoints de exporte en Controllers (2-3 métodos)
  - [ ] ClosedXML dependency (ya debe estar en proyecto)
- [ ] **Verificación**:
  - [ ] Descargas funcionan (file appear en disco)
  - [ ] Excel format correcto (headers, datos, estilos)
  - [ ] Filtros reflejados en exports

#### **Día 9 (viernes 25 ene) - QA Funcional (Parte 1)**
- [ ] **Duración**: 8h
- [ ] Tareas:
  - QA Smoke Tests
    - [ ] Load EmpleadosIndex → lista carga, no errores
    - [ ] Create empleado → modal abre, validaciones funcionan, POST exitoso
    - [ ] Edit empleado → datos cargan, PUT exitoso
    - [ ] Delete empleado → DELETE exitoso
    - [ ] Nested resources CRUD → todas las acciones OK
    - [ ] Desvinculaciones flujo completo → crear, evaluar, finalizar
    - [ ] Catálogos cargan → dropdowns poblados
    - [ ] Reportes descargan → file valido en disco
  - Data Validation Tests
    - [ ] Nombres > 3 caracteres (error si < 3)
    - [ ] Identificación > 0 (error si <= 0)
    - [ ] Salarios > 0 (error si <= 0)
    - [ ] Fechas: inicio ≤ fin (error si no cumple)
  - Permissions Tests
    - [ ] [Authorize] funciona (redirect a login si no autenticado)
    - [ ] Botones ocultos si no hay rol (admin only features)
- [ ] **Entregables**:
  - [ ] QA_SPRINT5_SMOKE_TESTS.xlsx (documento)
    - [ ] Columnas: Test ID | Test Case | Expected | Actual | Status | Notes
    - [ ] 30+ test cases mínimo
- [ ] **Verificación**:
  - [ ] Todos los smoke tests PASS
  - [ ] Console limpio (no errores)
  - [ ] Staging environment con datos reales

#### **Día 10 (lunes 28 ene) - QA Funcional (Parte 2) & Bug Fixes**
- [ ] **Duración**: 8h
- [ ] Tareas:
  - Ejecutar QA plan completamente
    - [ ] Correr todos los test cases de QA_SPRINT5_SMOKE_TESTS.xlsx
    - [ ] Documentar resultados (PASS/FAIL)
    - [ ] Crear lista de bugs encontrados
  - Bug Fixes
    - [ ] Priorizar bugs por severidad (Critical/High/Medium/Low)
    - [ ] Fijar bugs Críticos/Altos
    - [ ] Documentar bugs Medios/Bajos para Sprint 6
  - Performance Testing (basic)
    - [ ] EmpleadosIndex carga en < 2s (con 1000 registros)
    - [ ] Modales abren en < 500ms
    - [ ] Búsquedas responden en < 1s
  - Data Integrity Tests
    - [ ] Crear → Edit → Delete → verificar estado final
    - [ ] Nested resources: crear experiencia → editar → eliminar → confirmar
    - [ ] Desvinculaciones: workflow completo sin rollbacks
- [ ] **Entregables**:
  - [ ] QA_SPRINT5_RESULTS.xlsx (todos los tests ejecutados + resultados)
  - [ ] BUGS_ENCONTRADOS_SPRINT5.md (lista con severity)
  - [ ] PERFORMANCE_RESULTS.txt
- [ ] **Verificación**:
  - [ ] >= 95% test cases PASS
  - [ ] Bugs Críticos/Altos resueltos
  - [ ] Performance OK (< 2s)

#### **Día 11 (martes 29 ene) - Documentation & Cierre**
- [ ] **Duración**: 8h
- [ ] Tareas:
  - Crear **MIGRACION_TH_TALENTOHUMANO_VIEWS_COMPLETADA.md**
    - [ ] Sección 1: Objetivo Sprint 5
    - [ ] Sección 2: Entregables (lista de views creadas)
    - [ ] Sección 3: Mapeo pantalla legacy → endpoint API (tabla)
    - [ ] Sección 4: Arquitectura AJAX (flujos)
    - [ ] Sección 5: QA Results (summary + evidencias)
    - [ ] Sección 6: Bugs Encontrados + Resueltos
    - [ ] Sección 7: Performance Metrics
    - [ ] Sección 8: Próximos Pasos (Sprint 6)
  - Actualizar **DASHBOARD_MIGRACION.md**
    - [ ] Cambiar TH_TalentoHumano a 🟢 COMPLETO (Views + API)
    - [ ] Marcar Sprint 6 como 🟡 PRÓXIMO
    - [ ] Agregar fecha de cierre Sprint 5
  - Actualizar **sidebar/navigation**
    - [ ] Confirmar links a TH funcionales
    - [ ] Confirmar permisos en [Authorize]
  - Crear **commit de cierre Sprint 5**
    - [ ] Mensaje: "feat(TH): Sprint 5 Fase 1 - Views/UI para Empleados (14 files, 2,000+ LOC)"
- [ ] **Entregables**:
  - [ ] MIGRACION_TH_TALENTOHUMANO_VIEWS_COMPLETADA.md
  - [ ] DASHBOARD_MIGRACION.md actualizado
  - [ ] Commit con todos los archivos de Sprint 5
- [ ] **Verificación**:
  - [ ] Build final sin errores
  - [ ] Documentación completa
  - [ ] Commit refleja todos los cambios

---

## 📊 ESTIMACIÓN DE ARCHIVOS A CREAR

```
Views Razor (14 archivos):
├── EmpleadosIndex.cshtml
├── EmpleadosDetails.cshtml
├── _AjaxModalEmpleado.cshtml
├── ExperienciasLaboral/Index.cshtml
├── ExperienciasLaboral/_CreateEdit.cshtml
├── Educacion/Index.cshtml
├── Educacion/_CreateEdit.cshtml
├── Hijos/Index.cshtml
├── Hijos/_CreateEdit.cshtml
├── ContactosEmergencia/Index.cshtml
├── ContactosEmergencia/_CreateEdit.cshtml
├── Promociones/Index.cshtml
├── Promociones/_CreateEdit.cshtml
├── Salarios/Index.cshtml
├── Salarios/_CreateEdit.cshtml
├── DesvinculacionesIndex.cshtml
├── _AjaxModalDesvinculacion.cshtml
└── CatalogosIndex.cshtml

JavaScript (4 archivos):
├── empleados.js (200+ líneas)
├── nestedResources.js (150+ líneas)
├── desvinculaciones.js (100+ líneas)
└── catalogos.js (80+ líneas)

Documentación (3 archivos):
├── MIGRACION_TH_TALENTOHUMANO_VIEWS_COMPLETADA.md
├── QA_SPRINT5_SMOKE_TESTS.xlsx
└── QA_SPRINT5_RESULTS.xlsx

Total estimado: 25+ archivos, 2,000+ LOC
```

---

## 🎯 MÉTRICAS DE ÉXITO (Sprint 5)

| Métrica | Objetivo | Status |
|---|---|---|
| Views creadas | 14+ | 🟡 |
| JavaScript creado | 500+ LOC | 🟡 |
| QA Smoke Tests | 30+ cases | 🟡 |
| QA Pass Rate | >= 95% | 🟡 |
| Build Errors | 0 | 🟡 |
| Performance | < 2s | 🟡 |
| Documentación | Completa | 🟡 |

---

## 📞 CONTACTOS

- **Dev Lead**: [Asignar]
- **QA**: [Asignar]
- **Bloqueador crítico**: Escalate a Product Owner

---

## 📅 PRÓXIMO HITO

- **Fin Sprint 5**: 29 enero 2026
- **Inicio Sprint 6**: 1 febrero 2026 (OP_Cualitativo Complementos)

---

**Documento creado**: 2026-01-15  
**Status**: 🟡 KICKOFF INICIADO  
**Próxima revisión**: 29-01-2026 (cierre Sprint 5)
