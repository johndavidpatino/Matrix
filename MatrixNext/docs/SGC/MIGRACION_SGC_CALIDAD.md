# MIGRACION - MÓDULO SGC_CALIDAD

**Fecha Inicio**: 2026-01-15  
**Sprint**: Sprint 13  
**Estimación**: 42 horas  
**Estado**: 🔴 EN PROGRESO

---

## ✅ CHECKLIST DE IMPLEMENTACIÓN

### Fase 1: SETUP (4h) - 🔄 EN PROGRESO

- [x] Crear estructura de carpetas `Areas/SGC/`
- [x] Crear `MatrixNext/docs/SGC/MIGRACION_SGC_CALIDAD.md`
- [ ] Crear `AreaRegistration.cs` para área SGC
- [ ] Registrar DI en `Program.cs` (interfaces + implementaciones)
- [ ] Crear archivo seeding de catálogos (estados, tipos hallazgo)

### Fase 2: DATA ACCESS (8h) - ⏳ PENDIENTE

- [ ] Crear 9 DTOs en `Infrastructure/DTOs/SGC/`
- [ ] Crear 2 interfaces Adapter en `Infrastructure/Adapters/SGC/`
- [ ] Implementar 2 Adapters con Dapper
- [ ] Mapear 8 SP exactamente a métodos adapter
- [ ] DbContext EF Core (si aplica)
- [ ] Verificar SP en SQL Server

**Archivos por crear**:
- `ISGCAuditoriaAdapter.cs`
- `SGCAuditoriaAdapter.cs` (aprox. 300 LOC)
- `ISGCAccionMejoraAdapter.cs`
- `SGCAccionMejoraAdapter.cs` (aprox. 280 LOC)
- DTOs: AuditoriaDto, AuditadoDto, HallazgoDto, CausaDto, PlanAccionDto, etc.

### Fase 3: BUSINESS LOGIC (8h) - ⏳ PENDIENTE

- [ ] Crear 2 interfaces Service
- [ ] Implementar 2 Services (validaciones + permisos + logging)
- [ ] Implementar notificaciones si aplica
- [ ] Unit tests de lógica crítica

**Archivos por crear**:
- `ISGCAuditoriaService.cs`
- `SGCAuditoriaService.cs` (aprox. 250 LOC)
- `ISGCAccionMejoraService.cs`
- `SGCAccionMejoraService.cs` (aprox. 200 LOC)

### Fase 4: REST CONTROLLERS (6h) - ⏳ PENDIENTE

- [ ] Crear `AuditoriasController.cs` (8-10 endpoints)
- [ ] Crear `AccionesMejoraController.cs` (8-10 endpoints)
- [ ] Validar `[Authorize]` + roles
- [ ] Manejo de errores consistente

**Endpoints por crear**:
- GET `/api/sgc/auditorias` + POST + GET/{id} + PUT/{id} + DELETE/{id}
- GET `/api/sgc/auditorias/{id}/informe` + POST
- GET `/api/sgc/acciones-mejora` + POST + GET/{id} + PUT/{id} + DELETE/{id}

### Fase 5: VISTAS RAZOR (8h) - ⏳ PENDIENTE

- [ ] `Areas/SGC/Views/Auditorias/Index.cshtml` (grid + filtros)
- [ ] `Areas/SGC/Views/Auditorias/_CreateEdit.cshtml` (modal)
- [ ] `Areas/SGC/Views/Auditorias/_InformeAuditor.cshtml` (modal)
- [ ] `Areas/SGC/Views/AccionesMejora/Index.cshtml` (grid)
- [ ] `Areas/SGC/Views/AccionesMejora/_CreateEdit.cshtml` (modal)
- [ ] `Areas/SGC/Views/AccionesMejora/_Detalles.cshtml` (modal)

### Fase 6: JAVASCRIPT & CSS (4h) - ⏳ PENDIENTE

- [ ] `wwwroot/js/sgc-utilities.js` (modales, validaciones, filtros)
- [ ] `wwwroot/css/sgc.css` (estilos customizados)
- [ ] Integración con toast notifications
- [ ] Validaciones cliente (fechas, requeridos)

### Fase 7: INTEGRACION & QA (4h) - ⏳ PENDIENTE

- [ ] Testing funcional: CRUD auditorías
- [ ] Testing funcional: CRUD acciones mejora
- [ ] Testing permisos: ROL_CALIDAD, ROL_AUDITOR, ROL_AUDITADO
- [ ] Testing de filtros y paginación
- [ ] Actualizar `_Sidebar.cshtml` con links SGC
- [ ] Build sin errores
- [ ] Completar documentación

---

## 📊 PROGRESS TRACKING

### LOC Estimadas por Artefacto

| Artefacto | Estimado LOC | Real LOC | % Completitud |
|-----------|--------------|---------|---------------|
| DTOs (9 clases) | 400 | 0 | 0% |
| Adapters (2) | 580 | 0 | 0% |
| Services (2) | 450 | 0 | 0% |
| Controllers (2) | 350 | 0 | 0% |
| Vistas (6) | 800 | 0 | 0% |
| JS/CSS | 300 | 0 | 0% |
| **TOTAL** | **2,880** | **0** | **0%** |

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
