# VERIFICACIÓN: Stored Procedures PY_ControlCalidad

**Fecha**: 2026-01-15  
**Responsable**: [Equipo Backend]  
**Status**: 📋 PENDIENTE VERIFICACIÓN  
**Sprint**: 12 (Pre-requisito)

---

## 📋 VERIFICACIÓN DE SP REQUERIDOS

Antes de iniciar implementación, TODOS los SP deben existir en BD.

### ✅ CHECKLIST DE VERIFICACIÓN

#### Grupo 1: Control Principal (5 SP)

```sql
-- [ ] 1. PY_ControlCalidad_Get - Obtener 1 control por ID
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_NAME = 'PY_ControlCalidad_Get' 
  AND ROUTINE_TYPE = 'PROCEDURE';

-- Parámetros esperados:
-- @ID [bigint]
-- 
-- Retorna: PY_ControlCalidad_Get_Result
-- ├─ Id (bigint)
-- ├─ TrabajoId (bigint)
-- ├─ Evaluador (varchar)
-- ├─ RolEvaluador (varchar)
-- ├─ Persona (bigint)
-- ├─ Fecha (date)
-- ├─ TipoProceso (bigint)
-- ├─ JobBook (varchar) -- JOIN PY_Trabajo
-- ├─ NombreTrabajo (varchar) -- JOIN PY_Trabajo
-- ├─ Nombres (varchar) -- JOIN TH_Personas
-- └─ Apellidos (varchar) -- JOIN TH_Personas
```

- [ ] Verificar existencia: `PY_ControlCalidad_Get`
- [ ] Verificar parámetros: `@ID`
- [ ] Verificar retorna JOIN con PY_Trabajo
- [ ] Verificar retorna JOIN con TH_Personas

```sql
-- [ ] 2. PY_ControlCalidad_GetByTrabajo - Obtener controles por Trabajo y TipoProceso
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_NAME = 'PY_ControlCalidad_GetByTrabajo';

-- Parámetros esperados:
-- @TrabajoId [bigint]
-- @TipoProceso [bigint]
--
-- Retorna: LIST(PY_ControlCalidad_Get_Result)
```

- [ ] Verificar existencia: `PY_ControlCalidad_GetByTrabajo`
- [ ] Verificar parámetros: `@TrabajoId`, `@TipoProceso`
- [ ] Verificar retorna lista con misma estructura que Get

```sql
-- [ ] 3. PY_ControlCalidad_Add - Crear nuevo control
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_NAME = 'PY_ControlCalidad_Add';

-- Parámetros esperados:
-- @TrabajoId [bigint]
-- @Evaluador [varchar](100)
-- @RolEvaluador [varchar](100)
-- @Persona [bigint]
-- @Fecha [date]
-- @TipoProceso [bigint]
-- @RegistradoPor [int] -- NEW
-- @Id [bigint] OUTPUT
--
-- Acción: INSERT en PY_ControlCalidad
--   - Retorna nuevo @Id en OUTPUT
--   - Rellena RegistradoPor, FechaRegistro (GETDATE())
```

- [ ] Verificar existencia: `PY_ControlCalidad_Add`
- [ ] Verificar parámetros (8 total)
- [ ] Verificar OUTPUT @Id
- [ ] Verificar auditoría (RegistradoPor, FechaRegistro)

```sql
-- [ ] 4. PY_ControlCalidad_Edit - Actualizar control
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_NAME = 'PY_ControlCalidad_Edit';

-- Parámetros esperados:
-- @Id [bigint]
-- @TrabajoId [bigint]
-- @Evaluador [varchar](100)
-- @RolEvaluador [varchar](100)
-- @Persona [bigint]
-- @Fecha [date]
-- @TipoProceso [bigint]
-- @ModificadoPor [int] -- NEW
--
-- Acción: UPDATE PY_ControlCalidad
--   - Rellena ModificadoPor, FechaModificacion (GETDATE())
```

- [ ] Verificar existencia: `PY_ControlCalidad_Edit`
- [ ] Verificar parámetros (8 total)
- [ ] Verificar auditoría (ModificadoPor, FechaModificacion)

```sql
-- [ ] 5. PY_ControlCalidad_Del - Eliminar control
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_NAME = 'PY_ControlCalidad_Del';

-- Parámetros esperados:
-- @IdControlCalidad [bigint]
--
-- Acción: 
-- 1. DELETE FROM PY_DetalleControlCalidad WHERE IdControlCalidad = @IdControlCalidad
-- 2. DELETE FROM PY_ControlCalidad WHERE Id = @IdControlCalidad
```

- [ ] Verificar existencia: `PY_ControlCalidad_Del`
- [ ] Verificar parámetro: `@IdControlCalidad`
- [ ] Verificar cascada: Elimina detalles primero

---

#### Grupo 2: Detalles (3 SP)

```sql
-- [ ] 6. PY_DetalleControlCalidad_Get - Obtener detalles de un control
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_NAME = 'PY_DetalleControlCalidad_Get';

-- Parámetros esperados:
-- @IdControlCalidad [bigint]
--
-- Retorna: LIST(PY_DetalleControlCalidad_Get_Result)
-- ├─ id (bigint)
-- ├─ idControlCalidad (bigint)
-- ├─ IdPregunta (bigint)
-- ├─ Si (bit)
-- └─ Comentarios (varchar)
```

- [ ] Verificar existencia: `PY_DetalleControlCalidad_Get`
- [ ] Verificar parámetro: `@IdControlCalidad`
- [ ] Verificar retorna lista de detalles

```sql
-- [ ] 7. PY_DetalleControlCalidad_Add - Crear detalle de evaluación
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_NAME = 'PY_DetalleControlCalidad_Add';

-- Parámetros esperados:
-- @IdControlCalidad [bigint]
-- @IdPregunta [bigint]
-- @SI [bit]
-- @Comentarios [varchar](max)
-- @RegistradoPor [int] -- NEW
-- @Id [bigint] OUTPUT
--
-- Acción: INSERT en PY_DetalleControlCalidad
```

- [ ] Verificar existencia: `PY_DetalleControlCalidad_Add`
- [ ] Verificar parámetros (5 total)
- [ ] Verificar OUTPUT @Id
- [ ] Verificar auditoría

```sql
-- [ ] 8. PY_DetalleControlCalidad_DelxIdControl - Eliminar detalles x control
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_NAME = 'PY_DetalleControlCalidad_DelxIdControl';

-- Parámetros esperados:
-- @IdControlCalidad [bigint]
--
-- Acción: DELETE FROM PY_DetalleControlCalidad WHERE IdControlCalidad = @IdControlCalidad
```

- [ ] Verificar existencia: `PY_DetalleControlCalidad_DelxIdControl`
- [ ] Verificar parámetro: `@IdControlCalidad`
- [ ] Verifica eliminación en cascada

---

#### Grupo 3: Preguntas (5 SP)

```sql
-- [ ] 9. PY_Preguntas_Get - Obtener todas las preguntas
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_NAME = 'PY_Preguntas_Get';

-- Parámetros: (none)
--
-- Retorna: LIST(PY_Preguntas_Result)
-- ├─ IdPregunta (bigint)
-- ├─ IdProceso (bigint)
-- ├─ Pregunta (varchar)
-- ├─ Activa (bit)
-- └─ NombreProceso (varchar) -- JOIN PY_Tipos_Procesos
```

- [ ] Verificar existencia: `PY_Preguntas_Get`
- [ ] Verificar retorna todas las preguntas
- [ ] Verificar JOIN con PY_Tipos_Procesos

```sql
-- [ ] 10. PY_Preguntas_GetByTipo - Obtener preguntas por tipo de proceso
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_NAME = 'PY_Preguntas_GetByTipo';

-- Parámetros esperados:
-- @IdTipoProceso [bigint]
--
-- Retorna: LIST(PY_Preguntas_Result)
```

- [ ] Verificar existencia: `PY_Preguntas_GetByTipo`
- [ ] Verificar parámetro: `@IdTipoProceso`
- [ ] Verificar filtra por tipo

```sql
-- [ ] 11. PY_Preguntas_Add - Crear pregunta
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_NAME = 'PY_Preguntas_Add';

-- Parámetros esperados:
-- @IdProceso [bigint]
-- @Pregunta [varchar](max)
-- @Activa [bit]
-- @RegistradoPor [int] -- NEW
-- @IdPregunta [bigint] OUTPUT
--
-- Acción: INSERT en PY_Preguntas
```

- [ ] Verificar existencia: `PY_Preguntas_Add`
- [ ] Verificar parámetros (4 total)
- [ ] Verificar OUTPUT @IdPregunta
- [ ] Verificar auditoría

```sql
-- [ ] 12. PY_Preguntas_Edit - Actualizar pregunta
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_NAME = 'PY_Preguntas_Edit';

-- Parámetros esperados:
-- @IdPregunta [bigint]
-- @IdProceso [bigint]
-- @Pregunta [varchar](max)
-- @Activa [bit]
-- @ModificadoPor [int] -- NEW
--
-- Acción: UPDATE PY_Preguntas
```

- [ ] Verificar existencia: `PY_Preguntas_Edit`
- [ ] Verificar parámetros (5 total)
- [ ] Verificar auditoría

```sql
-- [ ] 13. PY_Preguntas_Del - Eliminar pregunta
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_NAME = 'PY_Preguntas_Del';

-- Parámetros esperados:
-- @IdPregunta [bigint]
--
-- Acción: DELETE FROM PY_Preguntas WHERE IdPregunta = @IdPregunta
```

- [ ] Verificar existencia: `PY_Preguntas_Del`
- [ ] Verificar parámetro: `@IdPregunta`

---

## 📊 RESUMEN DE VERIFICACIÓN

| # | SP | Existe | Parámetros OK | Retorna OK | Auditoría OK | Status |
|---|----|----|----|----|----|----|
| 1 | `PY_ControlCalidad_Get` | [ ] | [ ] | [ ] | [ ] | ⏳ |
| 2 | `PY_ControlCalidad_GetByTrabajo` | [ ] | [ ] | [ ] | N/A | ⏳ |
| 3 | `PY_ControlCalidad_Add` | [ ] | [ ] | [ ] | [ ] | ⏳ |
| 4 | `PY_ControlCalidad_Edit` | [ ] | [ ] | N/A | [ ] | ⏳ |
| 5 | `PY_ControlCalidad_Del` | [ ] | [ ] | N/A | N/A | ⏳ |
| 6 | `PY_DetalleControlCalidad_Get` | [ ] | [ ] | [ ] | N/A | ⏳ |
| 7 | `PY_DetalleControlCalidad_Add` | [ ] | [ ] | [ ] | [ ] | ⏳ |
| 8 | `PY_DetalleControlCalidad_DelxIdControl` | [ ] | [ ] | N/A | N/A | ⏳ |
| 9 | `PY_Preguntas_Get` | [ ] | N/A | [ ] | N/A | ⏳ |
| 10 | `PY_Preguntas_GetByTipo` | [ ] | [ ] | [ ] | N/A | ⏳ |
| 11 | `PY_Preguntas_Add` | [ ] | [ ] | [ ] | [ ] | ⏳ |
| 12 | `PY_Preguntas_Edit` | [ ] | [ ] | N/A | [ ] | ⏳ |
| 13 | `PY_Preguntas_Del` | [ ] | [ ] | N/A | N/A | ⏳ |

**Total SP a Verificar**: 13  
**Status**: ⏳ Pendiente verificación (completa todos los checkboxes)

---

## 🔧 SI FALTA ALGÚN SP

### Opción A: Crear SP desde Template

```sql
-- Template: PY_ControlCalidad_Get
CREATE PROCEDURE [dbo].[PY_ControlCalidad_Get]
    @ID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        cc.Id,
        cc.TrabajoId,
        cc.Evaluador,
        cc.RolEvaluador,
        cc.Persona,
        cc.Fecha,
        cc.TipoProceso,
        pt.JobBook,
        pt.NombreTrabajo,
        p.Nombres,
        p.Apellidos
    FROM PY_ControlCalidad cc
    LEFT JOIN PY_Trabajo pt ON cc.TrabajoId = pt.Id
    LEFT JOIN TH_Personas p ON cc.Persona = p.id
    WHERE cc.Id = @ID;
END;
```

### Opción B: Obtener de CoreProject

Si existe en WebMatrix, buscar en:
- `CoreProject/Datos/ControlCalidad.vb` - Métodos de SP
- `CoreProject/CC_FinzOpe.Designer.vb` - Definiciones de SP (sí, aunque sea CC, puede estar)

---

## 📝 TABLA DE VERIFICACIÓN MANUAL

Ejecuta en SQL Server Management Studio:

```sql
-- 1. Verificar SP existen
SELECT ROUTINE_NAME, ROUTINE_TYPE
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_SCHEMA = 'dbo' 
  AND (ROUTINE_NAME LIKE 'PY_ControlCalidad%' 
    OR ROUTINE_NAME LIKE 'PY_DetalleControlCalidad%'
    OR ROUTINE_NAME LIKE 'PY_Preguntas%')
ORDER BY ROUTINE_NAME;

-- 2. Verificar tablas existen
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo'
  AND TABLE_NAME IN ('PY_ControlCalidad', 'PY_DetalleControlCalidad', 'PY_Preguntas')
ORDER BY TABLE_NAME;

-- 3. Verificar FK existen
SELECT CONSTRAINT_NAME, TABLE_NAME, REFERENCED_TABLE_NAME
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS
WHERE TABLE_SCHEMA = 'dbo'
  AND (TABLE_NAME = 'PY_ControlCalidad' OR TABLE_NAME = 'PY_DetalleControlCalidad')
ORDER BY TABLE_NAME;

-- 4. Verificar columnas de auditoría
SELECT COLUMN_NAME, DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'PY_ControlCalidad'
ORDER BY ORDINAL_POSITION;
```

---

## ✅ CHECKLIST FINAL

Cuando TODOS los SP estén verificados/creados:

- [ ] 13/13 SP existen
- [ ] Parámetros son correctos
- [ ] FK existen y válidas
- [ ] Auditoría está presente
- [ ] Tables de detalle existen
- [ ] Enum TipoProceso actualizado
- [ ] DbContext mapea tablas
- [ ] ✅ LISTO PARA COMENZAR IMPLEMENTACIÓN

---

## 📞 ESCALACIÓN

Si faltan SP o hay errores:

1. Revisar código WebMatrix en `CoreProject`
2. Si no existe, crear basado en templates
3. Reportar al Tech Lead si hay discrepancias
4. ❌ NO INICIAR implementación hasta resolver

---

**Documento**: VERIFICACION_SP_PY_CONTROLCALIDAD.md  
**Versión**: 1.0  
**Fecha**: 2026-01-15  
**Status**: 📋 PENDIENTE (Completa ANTES del Lunes 16)  
**Responsable**: [Backend Team]
