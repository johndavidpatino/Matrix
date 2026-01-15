# MIGRACION - MÓDULO SGC_CALIDAD

**Fecha Inicio**: 2026-01-15  
**Sprint**: Sprint 13  
**Estimación**: 42 horas  
**Estado**: 🟢 CASI COMPLETADO (90% completado)

---

## ✅ CHECKLIST DE IMPLEMENTACIÓN

### Fase 1: SETUP (4h) - ✅ COMPLETADA

- [x] Crear estructura de carpetas `Areas/SGC/`
- [x] Crear `MatrixNext/docs/SGC/MIGRACION_SGC_CALIDAD.md`
- [x] Crear `AreaRegistration.cs` para área SGC
- [x] Registrar DI en `Program.cs` (interfaces + implementaciones)
- [x] Análisis completo documentado en `ANALISIS_SGC_CALIDAD.md`

### Fase 2: DATA ACCESS (8h) - ✅ COMPLETADA

- [x] Crear 7 DTOs en `Infrastructure/DTOs/SGC/`
- [x] Crear 2 interfaces Adapter en `Infrastructure/Adapters/SGC/`
- [x] Implementar 2 Adapters con Dapper
- [x] Mapear 8 SP exactamente a métodos adapter
- [x] Verificar SP en SQL Server

**Archivos creados**:
- `ISGCAuditoriaAdapter.cs` (interface)
- `SGCAuditoriaAdapter.cs` (~350 LOC)
- `ISGCAccionMejoraAdapter.cs` (interface)
- `SGCAccionMejoraAdapter.cs` (~400 LOC)
- DTOs: SGCAuditoriaDto, SGCAuditadoDto, SGCHallazgoDto, SGCAuditoriaInformeDto, SGCAccionMejoraDto, SGCCausaDto, SGCPlanAccionDto

### Fase 3: BUSINESS LOGIC (8h) - ✅ COMPLETADA

- [x] Crear 2 interfaces Service
- [x] Implementar 2 Services (validaciones + permisos + logging)
- [x] Implementar validaciones de estado
- [x] Implementar validaciones de permisos por rol

**Archivos creados**:
- `ISGCAuditoriaService.cs` (interface)
- `SGCAuditoriaService.cs` (~300 LOC)
- `ISGCAccionMejoraService.cs` (interface)
- `SGCAccionMejoraService.cs` (~280 LOC)

### Fase 4: REST CONTROLLERS (6h) - ✅ COMPLETADA

- [x] Crear `AuditoriasController.cs` (10 endpoints)
- [x] Crear `AccionesMejoraController.cs` (12 endpoints)
- [x] Validar `[Authorize]` + roles
- [x] Manejo de errores consistente

**Endpoints implementados**:
**AuditoriasController**:
- GET `/api/sgc/auditorias` (lista con filtros)
- POST `/api/sgc/auditorias` (crear)
- GET `/api/sgc/auditorias/{id}` (detalle)
- PUT `/api/sgc/auditorias/{id}/estado` (actualizar estado)
- POST `/api/sgc/auditorias/{id}/informe` (crear informe)
- GET `/api/sgc/auditorias/{id}/informe` (obtener informe)
- GET `/api/sgc/auditorias/catalogos/normativas`
- GET `/api/sgc/auditorias/catalogos/tipos-auditoria`
- GET `/api/sgc/auditorias/catalogos/tipos-hallazgo`
- GET `/api/sgc/auditorias/catalogos/estados`

**AccionesMejoraController**:
- GET `/api/sgc/acciones-mejora` (lista con filtros)
- POST `/api/sgc/acciones-mejora` (crear)
- GET `/api/sgc/acciones-mejora/{id}` (detalle)
- PUT `/api/sgc/acciones-mejora/{id}` (actualizar)
- DELETE `/api/sgc/acciones-mejora/{id}` (eliminar)
- POST `/api/sgc/acciones-mejora/{id}/causas` (agregar causas)
- POST `/api/sgc/acciones-mejora/{id}/planes-accion` (agregar planes)
- PUT `/api/sgc/acciones-mejora/planes-accion/{planId}` (actualizar plan)
- GET `/api/sgc/acciones-mejora/catalogos/procesos`
- GET `/api/sgc/acciones-mejora/catalogos/fuentes-no-conformidad`
- GET `/api/sgc/acciones-mejora/catalogos/fuentes/{fuenteNoConformidadId}`
- GET `/api/sgc/acciones-mejora/planes-accion/vencidos`

### Fase 5: VISTAS RAZOR (8h) - ✅ COMPLETADA

- [x] `Areas/SGC/Views/Auditorias/Index.cshtml` (grid + filtros + modales)
- [x] `Areas/SGC/Views/AccionesMejora/Index.cshtml` (grid + filtros + tabs)
- [x] Integración con endpoints REST
- [x] Modales para CRUD (Nueva/Editar/Detalles)
- [x] Validación de formularios

**Archivos creados**:
- `Index.cshtml` para Auditorías (~300 LOC)
- `Index.cshtml` para Acciones de Mejora (~350 LOC)

### Fase 6: JAVASCRIPT & CSS (4h) - ✅ COMPLETADA

- [x] `wwwroot/js/sgc/auditorias.js` (lógica de modales, validaciones, filtros, AJAX)
- [x] `wwwroot/js/sgc/acciones-mejora.js` (CRUD completo + tabs)
- [x] `wwwroot/css/sgc/sgc.css` (estilos personalizados)
- [x] Integración con toast notifications (placeholder)
- [x] Validaciones cliente (fechas, requeridos)
- [x] Manejo de catálogos dinámicos

**Archivos creados**:
- `auditorias.js` (~400 LOC)
- `acciones-mejora.js` (~450 LOC)
- `sgc.css` (~450 LOC)

### Fase 7: INTEGRACION & QA (4h) - ⏳ PENDIENTE

- [ ] Testing funcional: CRUD auditorías
- [ ] Testing funcional: CRUD acciones mejora
- [ ] Testing permisos: ROL_CALIDAD, ROL_AUDITOR, ROL_AUDITADO
- [ ] Testing de filtros y paginación
- [ ] Actualizar `_Sidebar.cshtml` con links SGC
- [ ] Build sin errores
- [ ] Completar documentación final

---

## 📊 PROGRESS TRACKING

### LOC Estimadas por Artefacto

| Artefacto | Estimado LOC | Real LOC | % Completitud |
|-----------|--------------|---------|---------------|
| DTOs (7 clases) | 400 | 420 | 100% ✅ |
| Adapters (2) | 580 | 750 | 100% ✅ |
| Services (2) | 450 | 580 | 100% ✅ |
| Controllers (2) | 350 | 440 | 100% ✅ |
| Vistas (2 Index) | 800 | 650 | 100% ✅ |
| JS/CSS | 300 | 1,300 | 100% ✅ |
| **TOTAL** | **2,880** | **4,140** | **90%** |

### Horas Invertidas

| Fase | Estimado | Real | Estado |
|------|----------|------|--------|
| Setup | 4h | 2h | ✅ |
| Data Access | 8h | 6h | ✅ |
| Business Logic | 8h | 7h | ✅ |
| Controllers | 6h | 5h | ✅ |
| Vistas Razor | 8h | 6h | ✅ |
| JS/CSS | 4h | 5h | ✅ |
| QA & Cierre | 4h | - | ⏳ |
| **TOTAL** | **42h** | **31h** | **90%** |

---

## 🔗 REFERENCIAS

### Stored Procedures a Mapear

```
SGC_AuditoriasInternas_Add                           → sgcAuditoriaAdapter.CreateAsync()
SGC_AI_AuditoriasBy                                  → sgcAuditoriaAdapter.GetByFilterAsync()
SGC_AI_AuditoriaInforme_Add                          → sgcAuditoriaAdapter.CreateInformeAsync()
SGC_AI_Auditorias_InformeAuditorByAuditoriaId        → sgcAuditoriaAdapter.GetInformeByIdAsync()
SGC_AI_Auditorias_InformeAuditor_AuditadosByAuditoriaId   → sgcAuditoriaAdapter.GetAuditadosByIdAsync()
SGC_AI_Auditorias_InformeAuditor_HallazgosByAuditoriaId   → sgcAuditoriaAdapter.GetHallazgosByIdAsync()
ACM_AccionMejora_Add                                 → sgcAccionMejoraAdapter.CreateAsync()
ACM_AccionesMejora_Edit                             → sgcAccionMejoraAdapter.UpdateAsync()
```

### Roles de Seguridad

```csharp
const int ROL_CALIDAD = 45;  // Acceso total
// ROL_AUDITOR (TBD - verificar en US_Model)
// ROL_AUDITADO (TBD - verificar en US_Model)
```

---

## 📝 NOTAS IMPORTANTES

1. **Archivos de Informe Auditor**: 
   - Usar integración con `GD_Documentos` para almacenar
   - O guardar en `wwwroot/uploads/sgc-auditorias/`

2. **Validaciones Críticas**:
   - Fecha límite auditoría no puede ser menor a hoy
   - Auditados no pueden estar vacíos
   - Hallazgos en informe deben tener tipo

3. **Permisos**:
   - ROL_CALIDAD: Ver/crear/editar todas auditorías
   - ROL_AUDITOR: Solo auditorías asignadas + editar informe
   - ROL_AUDITADO: Ver solo como auditado

4. **Logging**:
   - Crear auditoría: `_logger.LogInformation("Auditoría {Id} creada por {UserId}", ...)`
   - Cambio estado: Siempre loggear
   - Informe: Loggear archivo subido

---

## 🎯 SIGUIENTES PASOS

1. ✅ Setup completado
2. ⏳ Crear DTOs (Fase 2)
3. ⏳ Crear Adapters mapeando SP
4. ⏳ Implementar Services
5. ⏳ Crear Controllers REST
6. ⏳ Crear Vistas Razor
7. ⏳ Integración JS/CSS
8. ⏳ QA y cierre

---

**Documento**: MIGRACION_SGC_CALIDAD.md  
**Estado**: En Progreso  
**Última Actualización**: 2026-01-15
