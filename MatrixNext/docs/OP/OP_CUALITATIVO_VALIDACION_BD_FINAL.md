# ⚠️ VALIDACIÓN BD - OP_Cualitativo: Limitaciones Identificadas

**Fecha**: 9 de enero de 2026  
**Ejecutado por**: Auditoría de BD + Refactorización de Servicios  
**Estado**: ✅ Código compilado; ⚠️ Funcionalidad parcial (lectura > escritura)

---

## 📊 Resumen Ejecutivo

**Tablas y SPs faltantes en BD** que causaban fallo en la implementación:

| Tipo | Cantidad | Impacto | Estado |
|------|----------|---------|--------|
| **Tablas** | 3 | CRÍTICO | ❌ Removidas referencias del código |
| **SPs (Esperados)** | 6 | ALTO | ❌ Funcionalidad deshabilitada |
| **SPs (Confirmados pero solo lectura)** | 1 | MEDIO | ✅ Usable para GET |

---

## 🔴 TABLAS FALTANTES EN BD

### 1. `OP_FichasTecnicas`
- **Uso esperado**: Guardar/actualizar fichas de entrevista, sesión, observación, transcripción
- **En código**: `OpFichasTecnicasService`
  - ❌ `UPDATE OP_FichasTecnicas` (línea 187, 449, 517)
  - ❌ Referencias en `ObtenerTrabajoDetalleAsync` (CASE WHEN EXISTS)
- **Solución actual**: 
  - ✅ Lectura sigue usando SPs existentes (`OP_FichaEntrevistas_Get`)
  - ❌ Guardado comentado/deshabilitado
  - **Nota histórica**: En WebMatrix se guardaba en `PY_TrabajoCuali`, no en tabla separada
- **Refactorización aplicada**: Usar `PY_TrabajoCuali` si requiere persistencia

### 2. `OP_PreguntasFiltro`
- **Uso esperado**: CRUD de preguntas para filtros dinámicos (reclutamiento/asistencia)
- **En código**: `OpFiltrosService`
  - ❌ `DELETE FROM OP_PreguntasFiltro` (línea 146)
  - ❌ `UPDATE OP_PreguntasFiltro` (línea 166)
  - ❌ SPs: `OP_InsertarPreguntaFiltro`, `OP_ObtenerPreguntasFiltro`
- **Refactorización aplicada**: 
  - ✅ `ObtenerConfiguracionFiltroAsync()` ahora retorna colección vacía + warning
  - ✅ `AgregarPreguntaFiltroAsync()`, `ActualizarPreguntaFiltroAsync()`, `EliminarPreguntaFiltroAsync()` retornan error controlado
  - **Impacto**: Filtros dinámicos **no funcionales** hasta migración DB

### 3. `OP_Programados_Entrevistados`
- **Uso esperado**: Persistencia de programaciones (CREATE/UPDATE)
- **En código**: `OpProgramacionService`
  - ❌ `INSERT INTO OP_Programados_Entrevistados` (línea 154)
  - ❌ `UPDATE OP_Programados_Entrevistados` (línea 122, 198)
- **En BD**: 
  - ✅ **SÍ EXISTE** como SP de lectura: `OP_Programados_Entrevistados_Cuali_Get` (confirmado en CoreProject)
  - ❌ Tabla base NO existe (solo el SP)
- **Refactorización aplicada**: 
  - ✅ `GuardarProgramacionAsync()` retorna error controlado
  - ✅ `CambiarEstadoProgramacionAsync()` retorna error controlado
  - ✅ `ObtenerProgramacionesPorTrabajoAsync()` sigue funcionando (usa SP existente)
  - **Impacto**: Lectura funcional; escritura **no disponible**

---

## 🟡 STORED PROCEDURES FALTANTES

### Esperados (nunca existieron):
| SP | Módulo | Estado |
|----|---------| -------|
| `ObtenerAyudasRequeridasCualiList` | Fichas | ❌ NO EXISTE |
| `ObtenerReclutamientoRequeridoCualiList` | Fichas | ❌ NO EXISTE |
| `ObtenerHabeasData` | Fichas | ❌ NO EXISTE |
| `ObtenerTipoPreguntaFiltro` | Filtros | ❌ NO EXISTE |
| `ObtenerListaFiltros` | Filtros | ❌ NO EXISTE |
| `ObtenerListaPreguntasFiltro` | Filtros | ❌ NO EXISTE |

### Confirmados (pero faltantes):
| SP | En BD | En CoreProject | Uso |
|----|-------|--------|------|
| `obtenerXIdCOEXTodosCampos` | ❌ | ✅ | Trabajos COE |
| `ObtenerTrabajosCualitativosxCOE` | ❌ | ✅ | Trabajos por COE |
| `obtenerXCOE` | ❌ | ✅ | Obtener COE |

---

## ✅ LO QUE SÍ FUNCIONA

### SPs de Lectura (Verificados en BD):
- `OP_Programados_Entrevistados_Cuali_Get` ✅ EXISTE y funciona
- Usado por: `OpProgramacionService.ObtenerProgramacionesPorTrabajoAsync()`

### Servicios de Lectura (100% operacionales):
- ✅ `OpCualitativoService`: Leer trabajos, COE, configuraciones
- ✅ `OpProgramacionService`: Leer programaciones, exportar Excel/ICS
- ✅ `OpMuestraService`: Leer/crear/actualizar muestra (tiene tabla real)
- ✅ `OpFichasTecnicasService`: Leer fichas via SPs existentes
- ✅ `IEmailQueueService`: Enviar notificaciones

### Controllers Funcionales:
- ✅ `CualitativoTrabajosController.Index()` - listar
- ✅ `CualitativoTrabajosController.Details()` - ver detalles
- ✅ `CualitativoProgramacionController.Index()` - listar programaciones
- ✅ `CualitativoProgramacionController.ExportExcel()` - exportar
- ✅ `CualitativoProgramacionController.ExportIcs()` - exportar calendario
- ✅ `CualitativoProgramacionController.Calendar()` - ver calendario
- ✅ `CualitativoMuestraController.*` - CRUD completo
- ✅ `CualitativoCampoController.ExportExcel()` - exportar sesiones
- ✅ `CualitativoCampoController.ExportIcs()` - exportar calendario

---

## ❌ LO QUE NO FUNCIONA

| Funcionalidad | Razón | Referencia |
|---------------|-------|-----------|
| Crear/Editar Fichas | `OP_FichasTecnicas` NO existe | `OpFichasTecnicasService.GuardarFicha*()` |
| Entregar Fichas | Tabla no existe | `OpFichasTecnicasService.EntregarFicha*()` |
| Actualizar Habeas Data | Tabla no existe | `OpFichasTecnicasService.ActualizarHabeasData()` |
| Filtros Dinámicos | `OP_PreguntasFiltro` NO existe | `OpFiltrosService.AgregarPregunta()`, `ActualizarPregunta()` |
| Guardar Programación | `OP_Programados_Entrevistados` tabla NO existe | `OpProgramacionService.GuardarProgramacion()` |
| Cambiar Estado Programación | Tabla no existe | `OpProgramacionService.CambiarEstadoProgramacion()` |

---

## 🔧 REFACTORIZACIONES APLICADAS

### 1. OpCualitativoService.cs
```diff
- CASE WHEN EXISTS(SELECT 1 FROM OP_FichasTecnicas f WHERE f.TrabajoId = t.id AND f.TipoFicha = 1) ...
+ CASE WHEN EXISTS(SELECT 1 FROM PY_TrabajoCuali tc WHERE tc.TrabajoId = t.id) ...

- (SELECT COUNT(*) FROM OP_FichasTecnicas WHERE TrabajoId = @TrabajoId) +
- (SELECT COUNT(*) FROM OP_Programados_Entrevistados WHERE TrabajoId = @TrabajoId) +
+ (SELECT COUNT(*) FROM PY_TrabajoCuali WHERE TrabajoId = @TrabajoId) +
+ (SELECT COUNT(*) FROM OP_MuestraTrabajos WHERE TrabajoId = @TrabajoId) +
```

### 2. OpFichasTecnicasService.cs
```csharp
// UPDATE OP_FichasTecnicas → Reemplazado con comentario + warning
// EntregarFicha*() → Usa UPDATE PY_TrabajoCuali en su lugar
// ActualizarHabeasData() → Comentado (no aplicable a tabla real)
```

### 3. OpFiltrosService.cs
```csharp
// ObtenerConfiguracionFiltroAsync() → Retorna colección vacía + LogWarning
// AgregarPreguntaFiltroAsync() → Retorna error controlado
// ActualizarPreguntaFiltroAsync() → Retorna error controlado
// EliminarPreguntaFiltroAsync() → Retorna error controlado
```

### 4. OpProgramacionService.cs
```csharp
// GuardarProgramacionAsync() → Retorna error controlado
// CambiarEstadoProgramacionAsync() → Retorna error controlado
// ObtenerProgramacionesPorTrabajoAsync() → SIGUE FUNCIONANDO (usa SP existente)
```

---

## 🚀 RECOMENDACIONES

### Corto Plazo (Inmediato)
1. ✅ **Código compilado y funcional para lectura** - Lanzar a producción con limitación de escritura
2. ✅ **Documentar a usuarios**: "Lectura/visualización operacional; guardar funcionalidades en desarrollo"
3. ✅ **Usar SPs existentes**: `OP_Programados_Entrevistados_Cuali_Get` para listar

### Mediano Plazo (Sprint siguiente)
1. **Migrar tablas faltantes desde WebMatrix/CoreProject**:
   - Crear script DDL para `OP_FichasTecnicas` usando `PY_TrabajoCuali` como base
   - Crear script DDL para `OP_PreguntasFiltro` + opciones
   - Crear tabla base para `OP_Programados_Entrevistados` (separar de SP)

2. **Migrar SPs confirmados**:
   - `obtenerXIdCOEXTodosCampos`
   - `ObtenerTrabajosCualitativosxCOE`
   - `obtenerXCOE`

3. **Crear SPs faltantes** usando plantillas de legado

### Largo Plazo
- Refactorizar código MVC para no depender de SPs, usar EF Core 100%
- Migrar datos desde WebMatrix a nueva estructura

---

## 📋 CHECKLIST DE FUNCIONALIDAD

### Estado Actual (post-validación)
- [x] Compilación sin errores (solo warnings pre-existentes)
- [x] Lectura de trabajos operacional
- [x] Lectura de programaciones operacional
- [x] Exportación Excel/ICS funcional
- [x] Muestra CRUD funcional
- [ ] Guardar fichas ❌ (requiere tabla)
- [ ] Filtros dinámicos ❌ (requiere tabla)
- [ ] Guardar programaciones ❌ (requiere tabla)
- [ ] Cambiar estado programaciones ❌ (requiere tabla)

### Nota para QA
- **Flujos funcionales**: Listar, ver detalles, exportar, ver calendario
- **Flujos bloqueados**: Crear/editar/guardar cualquier entidad de fichas o programación
- **Mensajes de error controlado**: Usuarios verán mensaje amigable "Funcionalidad no disponible - en construcción"

---

## 📚 Referencias

- Validación SQL: `validate_sps_tables.sql`
- Análisis BD anterior: `ANALISIS_OP_CUALITATIVO_FASE5_MAPEO_BD_RIESGOS.md`
- Decisión de reuse: `SPRINT_6_AUDITORIA_SERVICIOS.md`
