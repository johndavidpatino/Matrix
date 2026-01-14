# MAPEO DE TABLAS Y SPs - OP_CUALITATIVO vs CoreProject

**Fecha**: 9 de enero, 2026  
**Resultado Validación**: Discrepancias encontradas - Ajuste necesario

---

## 1. TABLAS NO ENCONTRADAS → MAPEO A COREPROJECT

| Tabla Esperada (FASE 5) | Tabla Real (CoreProject) | Ubicación | SP/Método |
|------------------------|--------------------------|-----------|-----------|
| `OP_FichasTecnicas` | `OP_FichaEntrevistas` | CoreProject/Clases/OP/FichaEntrevistas.vb | `OP_FichaEntrevistas_Get`, `OP_FichaEntrevistas_Add`, `OP_FichaEntrevistas_Edit` |
| `OP_FichasTecnicas` (tipo=2) | `OP_FichaSesiones` | CoreProject/Clases/OP/FichaSesiones.vb | `OP_FichaSesiones_Get`, `OP_FichaSesiones_Add`, `OP_FichaSesiones_Edit` |
| `OP_FichasTecnicas` (tipo=3) | `OP_FichaObservaciones` | CoreProject/Clases/OP/FichaObservaciones.vb | `OP_FichaObservaciones_Get`, `OP_FichaObservaciones_Add`, `OP_FichaObservaciones_Edit` |
| `OP_PreguntasFiltro` | PENDING: Buscar en CoreProject | ? | ? |
| `OP_Programados_Entrevistados` | PENDING: Buscar en CoreProject | ? | ? |

---

## 2. SPs NO ENCONTRADOS → MAPEO A COREPROJECT

| SP Esperado (FASE 5) | SP Real (CoreProject) | Clase | Método VB |
|----------------------|----------------------|-------|----------|
| `obtenerXIdCOEXTodosCampos` | ✗ NO EXISTE | Trabajo.vb | ? |
| `ObtenerTrabajosCualitativosxCOE` | `ObtenerTrabajosCualitativosxCOE` | Trabajo.vb | Línea 144 |
| `obtenerXCOE` | `obtenerXCOE` | Trabajo.vb | Línea 95 |
| `ObtenerTipoPreguntaFiltro` | PENDING | ? | ? |
| `ObtenerListaFiltros` | PENDING | ? | ? |
| `ObtenerListaPreguntasFiltro` | PENDING | ? | ? |
| `ObtenerHabeasData` | PENDING | ? | ? |
| `ObtenerAyudasRequeridasCualiList` | PENDING | ? | ? |
| `ObtenerReclutamientoRequeridoCualiList` | PENDING | ? | ? |

---

## 3. ACCIÓN REQUERIDA

### INMEDIATA (Sprint 0 - Cierre):
1. ✅ Confirmar que `OP_FichaEntrevistas`, `OP_FichaSesiones`, `OP_FichaObservaciones` existen en BD
2. ✅ Actualizar `OpFichasTecnicasService` para usar SPs correctos:
   - `ObtenerFichaEntrevistaAsync()` → `OP_FichaEntrevistas_Get(NULL, @TrabajoId)`
   - `ObtenerFichaSesionAsync()` → `OP_FichaSesiones_Get(NULL, @TrabajoId)`
   - `ObtenerFichaObservacionAsync()` → `OP_FichaObservaciones_Get(NULL, @TrabajoId)`
3. ✅ Buscar en CoreProject las SPs faltantes (Filtros, Preguntas, Habeas Data)

### PRÓXIMO (Sprint 1):
4. Actualizar `OpFiltrosService` con SPs reales
5. Actualizar `OpCualitativoService` con adaptaciones

---

## 4. ARCHIVOS AFECTADOS EN MATRIXNEXT.WEB

- `MatrixNext.Web/Services/OP/OpFichasTecnicasService.cs` → Cambiar SPs
- `MatrixNext.Web/Services/OP/OpFiltrosService.cs` → Validar y ajustar
- `MatrixNext.Web/Services/OP/OpCualitativoService.cs` → Verificar si usa SPs correctos

---

## 5. CHECKLIST PENDIENTE

- [ ] Confirmar tablas: `OP_FichaEntrevistas`, `OP_FichaSesiones`, `OP_FichaObservaciones` existen en BD
- [ ] Actualizar servicios con nombres correctos de SPs
- [ ] Validar que CoreProject tiene todas las SPs necesarias para Filtros
- [ ] Si faltan SPs en CoreProject, crear sin afectar WebMatrix (extender OP_Entities)
- [ ] Re-test controllers P0 con SPs ajustados
