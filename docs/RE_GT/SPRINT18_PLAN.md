# 📋 SPRINT 18 - PLAN DETALLADO

> **Módulo**: RE_GT (Recolección y Gestión/Tratamiento) - Finalización 5%  
> **Objetivo**: Completar RE_GT al 100% (últimas 3 páginas + testing)  
> **Tiempo Estimado**: 12-15 horas  
> **Fecha Inicio**: 2025-01-16

---

## 🎯 OBJETIVO GENERAL

Migrar las 3 páginas faltantes de RE_GT y alcanzar **100% completud del módulo**:
- ✅ RecoleccionDatos.aspx (landing page) → Sprint 17 ✅
- ✅ GestionTratamiento.aspx (landing page) → Sprint 17 ✅
- ⏳ RecoleccionDatos sub-pages (8+ páginas internas)
- ⏳ CambiosJBI.aspx
- ⏳ AsignacionCampo.aspx

---

## 📐 ESTRUCTURA DE SPRINT 18

### FASE 1: Análisis de Páginas Internas (2-3h)

**SUBFASE 1.1: Análisis RecoleccionDatos Sub-pages**

Tareas:
- [ ] Identificar 8+ páginas internas en WebMatrix/RE_GT/RecoleccionDatos/
- [ ] Analizar cada página (código, lógica, SPs)
- [ ] Mapear dependencias
- [ ] Crear documento TASK_1_1_ANALISIS_RECOLECCION_SUBPAGES.md

Páginas esperadas:
- AsignarOMP.aspx
- AsignarJBI.aspx
- Presupuestos.aspx
- Costos.aspx
- Trabajos.aspx
- Seguimiento.aspx
- etc.

**SUBFASE 1.2: Análisis CambiosJBI.aspx**

Tareas:
- [ ] Leer WebMatrix/RE_GT/CambiosJBI.aspx
- [ ] Analizar lógica de cambios de JobBook
- [ ] Identificar SPs relacionados
- [ ] Crear documento TASK_1_2_ANALISIS_CAMBIOSJBI.md

**SUBFASE 1.3: Análisis AsignacionCampo.aspx**

Tareas:
- [ ] Leer WebMatrix/RE_GT/AsignacionCampo.aspx
- [ ] Analizar asignación de personal
- [ ] Verificar sobreposición OP_Cuantitativo
- [ ] Crear documento TASK_1_3_ANALISIS_ASIGNACIONCAMPO.md

---

### FASE 2: Implementación (8-10h)

**SUBFASE 2.1: RecoleccionDatos Sub-pages (5-6h)**

Arquitectura:
```
Service Layer:
- IRecoleccionSubPagesService
  ├─ ObtenerAsignacionesOMP()
  ├─ CrearAsignacionOMP()
  ├─ ObtenerPresupuestos()
  ├─ ObtenerCostos()
  └─ ObtenerTrabajosAsignados()

Controller:
- RecoleccionSubPagesController
  ├─ AsignacionOMP [GET/POST]
  ├─ Presupuestos [GET]
  ├─ Costos [GET]
  └─ Trabajos [GET]

Views:
- Recoleccion/AsignacionOMP.cshtml
- Recoleccion/Presupuestos.cshtml
- Recoleccion/Costos.cshtml
- Recoleccion/Trabajos.cshtml
```

Tareas:
- [ ] Crear DTOs necesarios
- [ ] Implementar Service + Adapter
- [ ] Crear 2-3 Controllers
- [ ] Crear 4-6 Razor views
- [ ] Registrar DI
- [ ] Build verification

**SUBFASE 2.2: CambiosJBI.aspx (2-3h)**

Tareas:
- [ ] Crear DTOs (CambioJBIDto)
- [ ] Crear Service + Adapter
- [ ] Crear Controller
- [ ] Crear Vista
- [ ] Build verification

**SUBFASE 2.3: AsignacionCampo.aspx (2-3h)**

Tareas:
- [ ] Crear DTOs (AsignacionCampoDto)
- [ ] Crear Service + Adapter
- [ ] Crear Controller
- [ ] Crear Vista
- [ ] Build verification

---

### FASE 3: Integration Testing (2-3h)

**SUBFASE 3.1: Testing E2E**

Tareas:
- [ ] Test navegación RecoleccionDatos landing → sub-pages
- [ ] Test CambiosJBI funcionalidad
- [ ] Test AsignacionCampo permisos
- [ ] Test permisos generales [Authorize]
- [ ] Test error handling
- [ ] Test rendimiento

**SUBFASE 3.2: Documentación Final**

Tareas:
- [ ] Crear SPRINT18_IMPLEMENTACION_COMPLETA.md
- [ ] Actualizar MODULOS_MIGRACION.md
- [ ] Crear SPRINT18_FINAL_REPORT.md
- [ ] 8 commits Git limpios

---

## 🚀 TAREAS POR ORDEN

### SPRINT 18 - TASK LIST

**TASK 1.1**: Análisis RecoleccionDatos sub-pages (1-1.5h)
- Identificar todas las páginas
- Crear análisis detallado

**TASK 1.2**: Análisis CambiosJBI.aspx (0.5-1h)
- Analizar página legacy
- Crear documento

**TASK 1.3**: Análisis AsignacionCampo.aspx (0.5-1h)
- Analizar página legacy
- Crear documento

**TASK 2.1**: Implementación RecoleccionDatos sub-pages (5-6h)
- DTOs (1h)
- Service + Adapter (2h)
- Controllers (1h)
- Views (1.5-2h)
- Build + tests (0.5h)

**TASK 2.2**: Implementación CambiosJBI (2-3h)
- DTOs (0.5h)
- Service + Adapter (1h)
- Controller (0.5h)
- View (0.5h)
- Build + tests (0.5h)

**TASK 2.3**: Implementación AsignacionCampo (2-3h)
- DTOs (0.5h)
- Service + Adapter (1h)
- Controller (0.5h)
- View (0.5h)
- Build + tests (0.5h)

**TASK 3.1**: Integration Testing (1.5-2h)
- E2E testing todas las secciones
- Validación de flujos

**TASK 3.2**: Documentación Final (1-1.5h)
- Reportes finales
- Commit final

---

## 📊 ESTIMACIÓN DE TIEMPO

| Fase | Subfase | Horas | Status |
|------|---------|-------|--------|
| 1 | 1.1 - Análisis Recolección | 1-1.5h | ⏳ Pendiente |
| 1 | 1.2 - Análisis CambiosJBI | 0.5-1h | ⏳ Pendiente |
| 1 | 1.3 - Análisis AsignacionCampo | 0.5-1h | ⏳ Pendiente |
| 2 | 2.1 - Impl Recolección | 5-6h | ⏳ Pendiente |
| 2 | 2.2 - Impl CambiosJBI | 2-3h | ⏳ Pendiente |
| 2 | 2.3 - Impl AsignacionCampo | 2-3h | ⏳ Pendiente |
| 3 | 3.1 - Testing E2E | 1.5-2h | ⏳ Pendiente |
| 3 | 3.2 - Documentación | 1-1.5h | ⏳ Pendiente |
| | **TOTAL** | **12-15h** | ⏳ Pendiente |

---

## 🏗️ ARQUITECTURA CONSISTENTE

Mantener patrones de Sprint 17:

**Service → Controller → View**

```csharp
// DTOs: MatrixNext.Core/DTOs/RE_GT/
// Services: MatrixNext.Web/Services/RE_GT/
// Controllers: MatrixNext.Web/Areas/RE_GT/Controllers/
// Views: MatrixNext.Web/Areas/RE_GT/Views/
// Adapters: MatrixNext.Data/Adapters/RE_GT/
```

**Decoradores Obligatorios**:
```csharp
[Area("RE_GT")]
[Authorize]
public class XxxController : Controller { }
```

**Logging + Error Handling**:
- ILogger<T> en todos los servicios
- Try/catch sin stack traces
- Mensajes amigables al cliente

---

## 📝 DOCUMENTACIÓN DELIVERABLE

1. ✅ `TASK_1_1_ANALISIS_RECOLECCION_SUBPAGES.md`
2. ✅ `TASK_1_2_ANALISIS_CAMBIOSJBI.md`
3. ✅ `TASK_1_3_ANALISIS_ASIGNACIONCAMPO.md`
4. ✅ `SPRINT18_IMPLEMENTACION_COMPLETA.md`
5. ✅ `SPRINT18_FINAL_REPORT.md`
6. ✅ `MODULOS_MIGRACION.md` (actualizado)
7. ✅ 8-10 commits Git limpios
8. ✅ 0 errores de compilación

---

## ✅ CHECKLIST PRE-SPRINT

- [ ] Planes de Fase 1 claros
- [ ] Documentos de análisis preparados
- [ ] DTOs template preparado
- [ ] Service template preparado
- [ ] Controller template preparado
- [ ] Vista template preparada
- [ ] DI pattern establecido
- [ ] Build configuration verificada

---

## 🎯 CRITERIO DE ÉXITO

✅ **Sprint 18 Completado** cuando:
- 3 páginas nuevas migradas (CambiosJBI + AsignacionCampo + RecoleccionSubs)
- 0 errores de compilación
- Build success verificado
- Documentación completa
- 8+ commits limpios
- RE_GT 100% completado
- MODULOS_MIGRACION.md actualizado con ✅ RE_GT COMPLETADO

---

**Responsable**: GitHub Copilot  
**Fecha Creación**: 2025-01-16  
**Estado**: 🎬 LISTO PARA INICIAR
