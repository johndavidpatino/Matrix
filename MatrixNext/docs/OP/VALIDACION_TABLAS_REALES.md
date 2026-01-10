# VALIDACIÓN DE TABLAS Y SPs REALES - OP_Cualitativo

**Fecha**: 9 Enero 2026  
**Fuente de Verdad**: `MatrixNext/docs/OP/SQL/CO_Matrix_Structure_Tables.sql`

---

## TABLAS REALES CONFIRMADAS ✅

| Tabla | Confirmar | PK | FK | Notas |
|-------|-----------|----|----|-------|
| `OP_FichaEntrevistas` | ✅ EXISTE | `id` | `TrabajoId → PY_Trabajo` | Para fichas de entrevistas cualitativas |
| `OP_FichaSesiones` | ✅ EXISTE | `id` | `TrabajoId → PY_Trabajo` | Para fichas de sesiones de grupos |
| `OP_FichaObservaciones` | ✅ EXISTE | `id` | `TrabajoId → PY_Trabajo` | Para fichas de observación |
| `OP_FichaCuantitativo` | ✅ EXISTE | `id` | `TrabajoId → PY_Trabajo` | Para fichas cuantitativas |
| `OP_Preguntas_Filtro` | ✅ EXISTE | `Id` | `IdFiltro → OP_Filtros` | Para preguntas de filtros (NOT `OP_PreguntasFiltro`) |
| `OP_MuestraTrabajos` | ✅ EXISTE | - | - | Para registro de muestra |
| `OP_IPS_Revisiones` | ✅ EXISTE | - | - | Para revisiones IPS |

---

## TABLAS QUE NO EXISTEN ❌

| Tabla | Motivo | Impacto | Acción |
|-------|--------|--------|--------|
| `OP_FichasTecnicas` | **NUNCA existió en BD** (alucinación de IA) | CRÍTICO | **Reemplazar por tabla específica** |
| `OP_PreguntasFiltro` | Nombre incorrecto (real: `OP_Preguntas_Filtro`) | CRÍTICO | **Usar `OP_Preguntas_Filtro`** |
| `OP_Programados_Entrevistados` | Solo existe SP lectura; no hay tabla para WRITE | CRÍTICO | **Usar solo SP `OP_Programados_Entrevistados_Cuali_Get` para leer** |

---

## SPs REALES CONFIRMADAS ✅

| SP | Tipo | Parámetros | Notas |
|----|------|-----------|-------|
| `OP_FichaEntrevistas_Get` | READ | `@ID`, `@TrabajoID` | Obtener fichas de entrevistas |
| `OP_FichaEntrevistas_Add` | WRITE | 33 parámetros | Crear ficha entrevista |
| `OP_FichaEntrevistas_Edit` | WRITE | 33 parámetros | Editar ficha entrevista |
| `OP_FichaEntrevistas_Del` | WRITE | `@ID` | Eliminar ficha entrevista |
| `OP_FichaSesiones_Get` | READ | `@ID`, `@TrabajoID` | Obtener fichas de sesiones |
| `OP_FichaSesiones_Add` | WRITE | 30+ parámetros | Crear ficha sesión |
| `OP_FichaSesiones_Edit` | WRITE | 30+ parámetros | Editar ficha sesión |
| `OP_FichaSesiones_Del` | WRITE | `@ID` | Eliminar ficha sesión |
| `OP_FichaObservaciones_Get` | READ | `@ID`, `@TrabajoID` | Obtener fichas de observación |
| `OP_FichaObservaciones_Add` | WRITE | 20+ parámetros | Crear ficha observación |
| `OP_FichaObservaciones_Edit` | WRITE | 20+ parámetros | Editar ficha observación |
| `OP_FichaObservaciones_Del` | WRITE | `@ID` | Eliminar ficha observación |
| `OP_FichaCuantitativo_Get` | READ | `@ID`, `@TrabajoID` | Obtener fichas cuantitativas |
| `OP_FichaCuantitativo_Add` | WRITE | 15 parámetros | Crear ficha cuantitativa |
| `OP_FichaCuantitativo_Edit` | WRITE | 15 parámetros | Editar ficha cuantitativa |
| `OP_FichaCuantitativo_Del` | WRITE | `@ID` | Eliminar ficha cuantitativa |
| `OP_Preguntas_Filtro_Get` | READ | - | Obtener preguntas filtro |
| `OP_Programados_Entrevistados_Cuali_Get` | READ | - | Obtener programaciones (lectura solo) |

---

## CORRECCIONES NECESARIAS EN CÓDIGO

### 1. OpCualitativoService.cs

#### Línea 270 (GetDetalleTrabajoAsync - Verificación de Fichas)
**ANTES**:
```csharp
CASE WHEN EXISTS(SELECT 1 FROM PY_TrabajoCuali tc WHERE tc.TrabajoId = t.id) THEN 1 ELSE 0 END AS TieneFichaEntrevista,
CASE WHEN EXISTS(SELECT 1 FROM PY_TrabajoCuali tc WHERE tc.TrabajoId = t.id AND tc.IncentivoEconomico = 1) THEN 1 ELSE 0 END AS TieneFichaSesion,
CASE WHEN EXISTS(SELECT 1 FROM PY_TrabajoCuali tc WHERE tc.TrabajoId = t.id) THEN 1 ELSE 0 END AS TieneFichaObservacion,
```

**DESPUÉS**:
```csharp
CASE WHEN EXISTS(SELECT 1 FROM OP_FichaEntrevistas WHERE TrabajoId = t.id) THEN 1 ELSE 0 END AS TieneFichaEntrevista,
CASE WHEN EXISTS(SELECT 1 FROM OP_FichaSesiones WHERE TrabajoId = t.id) THEN 1 ELSE 0 END AS TieneFichaSesion,
CASE WHEN EXISTS(SELECT 1 FROM OP_FichaObservaciones WHERE TrabajoId = t.id) THEN 1 ELSE 0 END AS TieneFichaObservacion,
```

#### Línea 420 (EliminarTrabajoAsync - Validación de Dependencias)
**ANTES**:
```csharp
(SELECT COUNT(*) FROM OP_FichaEntrevistas WHERE TrabajoId = @TrabajoId) +
(SELECT COUNT(*) FROM OP_FichaSesiones WHERE TrabajoId = @TrabajoId) +
(SELECT COUNT(*) FROM OP_FichaObservaciones WHERE TrabajoId = @TrabajoId) +
(SELECT COUNT(*) FROM OP_MuestraTrabajos WHERE TrabajoId = @TrabajoId) +
(SELECT COUNT(*) FROM PY_PlanillaModeracion WHERE TrabajoId = @TrabajoId)
```

**RESPUESTA**: Esto está CORRECTO ✅

#### Línea 458-462 (GetProximasAccionesTrabajoAsync - Verificación de Fichas y Filtros)
**ANTES**:
```csharp
CASE WHEN EXISTS(SELECT 1 FROM OP_FichaEntrevistas WHERE TrabajoId = @TrabajoId) THEN 1 ELSE 0 END AS TieneFichaEntrevista,
CASE WHEN EXISTS(SELECT 1 FROM OP_FichaSesiones WHERE TrabajoId = @TrabajoId) THEN 1 ELSE 0 END AS TieneFichaSesion,
CASE WHEN EXISTS(SELECT 1 FROM OP_FichaObservaciones WHERE TrabajoId = @TrabajoId) THEN 1 ELSE 0 END AS TieneFichaObservacion,
CASE WHEN EXISTS(SELECT 1 FROM OP_MuestraTrabajos WHERE TrabajoId = @TrabajoId) THEN 1 ELSE 0 END AS TieneMuestra,
0 AS TieneFiltroReclutamiento,
0 AS TieneFiltroAsistencia,
0 AS TieneProgramacion,
CASE WHEN EXISTS(SELECT 1 FROM OP_IPS_Revisiones WHERE TrabajoId = @TrabajoId) THEN 1 ELSE 0 END AS TieneIps
```

**RESPUESTA**: Esto está CORRECTO ✅ (pero mejorar filtro check)

**MEJORADO**:
```csharp
CASE WHEN EXISTS(SELECT 1 FROM OP_FichaEntrevistas WHERE TrabajoId = @TrabajoId) THEN 1 ELSE 0 END AS TieneFichaEntrevista,
CASE WHEN EXISTS(SELECT 1 FROM OP_FichaSesiones WHERE TrabajoId = @TrabajoId) THEN 1 ELSE 0 END AS TieneFichaSesion,
CASE WHEN EXISTS(SELECT 1 FROM OP_FichaObservaciones WHERE TrabajoId = @TrabajoId) THEN 1 ELSE 0 END AS TieneFichaObservacion,
CASE WHEN EXISTS(SELECT 1 FROM OP_MuestraTrabajos WHERE TrabajoId = @TrabajoId) THEN 1 ELSE 0 END AS TieneMuestra,
CASE WHEN EXISTS(
    SELECT 1 FROM OP_Preguntas_Filtro pf 
    INNER JOIN OP_Filtros f ON pf.IdFiltro = f.Id 
    WHERE f.TrabajoId = @TrabajoId
) THEN 1 ELSE 0 END AS TieneFiltroReclutamiento,
0 AS TieneFiltroAsistencia,
0 AS TieneProgramacion,
CASE WHEN EXISTS(SELECT 1 FROM OP_IPS_Revisiones WHERE TrabajoId = @TrabajoId) THEN 1 ELSE 0 END AS TieneIps
```

---

### 2. OpFichasTecnicasService.cs

#### Línea 187 (EntregarFichaEntrevistaAsync)
**ESTADO**: ⚠️ Requiere validación
- Debe usar SPs de CoreProject: `OP_FichaEntrevistas_Get`, `OP_FichaEntrevistas_Edit`
- **NO actualizar `PY_TrabajoCuali`** directamente
- Usar el SP correspondiente para el tipo de ficha

**ACCIÓN**: Refactorizar para usar SPs reales

#### Línea 447 (SaveSessionFichaAsync)  
**ESTADO**: ⚠️ Requiere validación
- Debe usar SPs: `OP_FichaSesiones_Get`, `OP_FichaSesiones_Edit`
- **NO actualizar `PY_TrabajoCuali`** directamente

**ACCIÓN**: Refactorizar para usar SPs reales

#### Línea 514 (ActualizarHabeasDataAsync)
**ESTADO**: ❌ Comentado correctamente (tabla no existe)
- Mantener comentado hasta que se defina cómo se maneja en nuevo sistema

---

### 3. OpFiltrosService.cs

#### General - Tabla Incorrecta
**PROBLEMA**: `OP_PreguntasFiltro` (nombre incorrecto)
**CORRECCIÓN**: Usar `OP_Preguntas_Filtro` (con underscore)

#### Línea 84-100 (ObtenerConfiguracionFiltroAsync)
**ESTADO**: ⚠️ Parcialmente correcto
- Usar tabla `OP_Preguntas_Filtro` (no `OP_PreguntasFiltro`)
- Usar SP `OP_Preguntas_Filtro_Get` si existe

#### Línea 73-117 (Agregar/Actualizar/Eliminar Preguntas)
**ESTADO**: ❌ Métodos deshabilitados (correcto por ahora)
- Esperar migración DB para implementar
- Funcionalidad debe esperar tabla real en ambiente

---

### 4. OpProgramacionService.cs

#### Línea 68 (ObtenerProgramacionesPorTrabajoAsync)
**ESTADO**: ⚠️ Requiere validación
- Usa SP `OP_Programados_Entrevistados_Cuali_Get` (CORRECTO ✅)
- **NO debe intentar escribir en `OP_Programados_Entrevistados`** (tabla no existe)

#### Línea 104-190 (GuardarProgramacionAsync, CambiarEstadoProgramacionAsync)
**ESTADO**: ✅ Deshabilitado correctamente
- Estos métodos están retornando error (correcto)
- Esperar decisión arquitectónica sobre dónde guardar programaciones

---

## RESUMEN DE CAMBIOS REQUERIDOS

### CRÍTICOS (Hacer ahora):
1. ✅ Cambiar `OP_PreguntasFiltro` → `OP_Preguntas_Filtro` en OpFiltrosService
2. ✅ Reemplazar verificaciones de `PY_TrabajoCuali` por tablas reales (OP_FichaEntrevistas, etc.)
3. ✅ Refactorizar OpFichasTecnicasService para usar SPs de CoreProject (NO update directo a PY_TrabajoCuali)

### ESTADO ACTUAL (ACEPTABLE):
1. ✅ OpProgramacionService - Lectura vía SP confirmada; escritura deshabilitada (correcto)
2. ✅ Métodos deshabilitados con mensajes amigables (error handling correcto)
3. ✅ Build pasa sin errores

---

## BUILD VALIDATION

**Última ejecución**: 8 Enero 2026, 18:45  
**Resultado**: ✅ Success (0 new errors, 25 pre-existing nullability warnings)  
**Comando**: `dotnet build "MatrixNext.Web.csproj" -c Debug`

**Post-Cambios**: Ejecutar nuevamente para validar NO hay breaking changes

---

## PRÓXIMOS PASOS

1. [ ] Actualizar OpFiltrosService (nombre tabla)
2. [ ] Validar joins en OpCualitativoService (OP_Preguntas_Filtro)
3. [ ] Refactorizar OpFichasTecnicasService (usar SPs)
4. [ ] Build + test
5. [ ] Commit con referencia a esta validación

