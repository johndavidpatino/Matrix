# PLAN DE VALIDACIÓN Y CORRECCIÓN - COHERENCIA CON BD

**Versión**: 1.0  
**Fecha**: 2026-01-17  
**Proyecto**: MatrixNext  
**Restricción**: BD en producción - NO crear tablas, campos o SP nuevos

---

## RESUMEN EJECUTIVO

### Hallazgos de la FASE 12

| Categoría | Hallazgo | Impacto |
|-----------|----------|---------|
| SP "no documentados" | 241 identificados | ⚠️ Requiere análisis |
| **Falsos positivos** | ~60% son tablas/entidades EF, no SP | ✅ No requiere acción |
| **Typos de nombres** | ~15% tienen nombre similar correcto | 🔧 Corregir en código |
| **SP inexistentes** | ~25% probablemente no existen | ❌ Eliminar o reemplazar |

### Análisis Detallado

Tras el análisis, los 241 "SP no documentados" se clasifican en:

| Tipo | Cantidad Est. | Ejemplo | Acción |
|------|---------------|---------|--------|
| **Tablas/Entidades EF** | ~90 | `CORE_Tareas`, `OP_CuantiPlanillas` | ✅ Ignorar (son tablas) |
| **SP con nombre incorrecto** | ~40 | `TH_Areas_Get` → `TH_Area_Get` | 🔧 Corregir nombre |
| **SP que sí existen pero faltan en CSV** | ~50 | Varios de CC_, MBO_ | 📝 Verificar en BD |
| **SP que no existen** | ~60 | Nuevos creados para MatrixNext | ❌ Eliminar/Reemplazar |

---

## FASE 13: PLAN DE CORRECCIÓN (Propuesto)

### Duración Estimada: 16-20 horas

---

### 13.1: CLASIFICACIÓN AUTOMÁTICA (2h)

**Objetivo**: Separar automáticamente falsos positivos de problemas reales

**Script de clasificación**:
```powershell
# Clasificar SP no documentados
$spNoDoc = Get-Content "docs/SQL/SP_NoDocumentados.txt"
$tablas = Get-Content "docs/SQL/Tablas_BD.txt"
$spDoc = Get-Content "docs/SQL/CO_Matrix_SP_Names.csv" | % { ($_ -split ";")[1] }

# Categoría 1: Son tablas (no SP)
$sonTablas = $spNoDoc | Where-Object { $_ -in $tablas }

# Categoría 2: Tienen SP similar (typo)
$tienenSimilar = $spNoDoc | Where-Object { 
    $sp = $_
    $spDoc | Where-Object { 
        ($_ -replace '_Get|_Add|_Edit|_Del','') -eq ($sp -replace '_Get|_Add|_Edit|_Del','')
    }
}

# Categoría 3: Probablemente no existen
$noExisten = $spNoDoc | Where-Object { $_ -notin $sonTablas -and $_ -notin $tienenSimilar }
```

**Entregable**: 
- `docs/SQL/CLASIFICACION_SP.md` con 3 listas categorizadas

---

### 13.2: CORRECCIÓN DE TYPOS EN NOMBRES (4h)

**Objetivo**: Corregir nombres de SP mal escritos

**Casos identificados**:

| Incorrecto | Correcto | Archivo | Acción |
|------------|----------|---------|--------|
| `TH_Areas_Get` | `TH_Area_Get` | EmpleadoDataAdapter.cs | Renombrar |
| `CC_Conteos_Insert` | `CC_ConteosAdd` | ConteoAdapter.cs | Verificar y corregir |
| `CC_Conteos_Delete` | `CC_ConteosDel` | ConteoAdapter.cs | Verificar y corregir |
| `CC_DetallePresupuesto` | `CC_DetallePresupuestoGet` | PresupuestoAdapter.cs | Agregar sufijo |

**Proceso**:
1. Buscar archivo con referencia incorrecta
2. Verificar SP correcto en `CO_Matrix_Structure_SP.sql`
3. Validar parámetros coinciden
4. Renombrar en código
5. Ejecutar tests unitarios

**Entregable**:
- Lista de 40+ correcciones de nombres
- Commit con cambios

---

### 13.3: VERIFICACIÓN EN BD DE DESARROLLO (3h)

**Objetivo**: Confirmar existencia de SP dudosos en BD real

**SP prioritarios a verificar** (no están en CSV pero podrían existir):

```sql
-- Script para ejecutar en BD de desarrollo
SELECT name, create_date, modify_date
FROM sys.procedures
WHERE name IN (
    'CC_AprobarPresupuestoInterno',
    'CC_CalculoJornada_Get',
    'CC_ConsolidacionProduccion',
    'GD_ConfiguracionRevision_Get',
    'MBO_CampoCalidadGeneral',
    'PY_ControlCalidad_GetByTipo',
    'UU_ModeradoresGet'
    -- ... agregar más
)
ORDER BY name
```

**Proceso**:
1. Ejecutar script en BD de desarrollo
2. Para SP que SÍ existen: Actualizar `CO_Matrix_SP_Names.csv`
3. Para SP que NO existen: Marcar para eliminación/reemplazo

**Entregable**:
- `docs/SQL/VERIFICACION_BD_RESULTADO.md`
- `CO_Matrix_SP_Names.csv` actualizado

---

### 13.4: ELIMINAR REFERENCIAS A SP INEXISTENTES (6h)

**Objetivo**: Reemplazar llamadas a SP que no existen con alternativas válidas

**Estrategias por caso**:

| Caso | Estrategia | Ejemplo |
|------|------------|---------|
| SP nuevo creado para MatrixNext | Usar SP existente equivalente | `CC_AprobarPresupuestoInterno` → usar SP de aprobación existente |
| SP deprecado | Eliminar funcionalidad o usar EF | `CatiRMC_*` → Evaluar si módulo está activo |
| SP de módulo no migrado | Comentar con TODO | `Sync_*` → //TODO: Migrar módulo Sync |

**Archivos prioritarios** (más referencias):

1. `MatrixNext.Data/Modules/CC/Adapters/CcPresupuestosInternosAdapter.cs`
2. `MatrixNext.Data/Modules/TH/Empleados/Adapters/EmpleadoDataAdapter.cs`
3. `MatrixNext.Web/Services/CORE/TareasService.cs`
4. `MatrixNext.Web/Services/OP/OpPlanillasService.cs`

**Proceso por archivo**:
1. Identificar SP inexistente
2. Buscar SP equivalente en `CO_Matrix_Structure_SP.sql`
3. Verificar parámetros
4. Reemplazar o eliminar
5. Actualizar tests

**Entregable**:
- Código corregido
- Lista de SP eliminados/reemplazados

---

### 13.5: VALIDACIÓN DE PARÁMETROS (3h)

**Objetivo**: Verificar que parámetros en código coinciden con definición de SP

**Script de validación**:
```powershell
# Extraer parámetros de SP usados
Get-ChildItem -Path MatrixNext.Data -Recurse -Filter "*.cs" |
    Select-String -Pattern '@\w+' |
    # Comparar contra definición en CO_Matrix_Structure_SP.sql
```

**Casos comunes de error**:
- Parámetro con nombre incorrecto: `@IdEmpleado` vs `@EmpleadoId`
- Parámetro faltante: SP espera 5 params, código envía 4
- Tipo incorrecto: `int` vs `long`

**Entregable**:
- Lista de discrepancias de parámetros
- Correcciones aplicadas

---

### 13.6: DOCUMENTACIÓN Y CIERRE (2h)

**Objetivo**: Documentar cambios y actualizar tracking

**Tareas**:
1. Actualizar `SEMAFORO_AVANCE_AUDITORIA.md` con FASE 13
2. Crear `CHANGELOG_CORRECCION_BD.md` con todos los cambios
3. Actualizar `CO_Matrix_SP_Names.csv` con SP faltantes
4. Commit final con resumen

**Entregable**:
- Documentación completa
- Semáforo actualizado

---

## CRITERIOS DE ACEPTACIÓN

### Para considerar FASE 13 completa:

- [ ] 0 referencias a SP que no existen en BD
- [ ] Todos los nombres de SP corregidos (typos)
- [ ] `CO_Matrix_SP_Names.csv` actualizado con SP verificados
- [ ] Parámetros de SP validados contra definición
- [ ] Build exitoso sin errores
- [ ] Tests de integración pasan (si existen)

---

## RIESGOS Y MITIGACIÓN

| Riesgo | Probabilidad | Impacto | Mitigación |
|--------|--------------|---------|------------|
| SP requerido no existe en BD | Media | Alto | Usar SP equivalente o EF |
| Cambio de nombre rompe funcionalidad | Baja | Alto | Tests + rollback plan |
| Parámetros incompatibles | Media | Medio | Verificar definición SP antes de cambiar |
| Tiempo excede estimación | Media | Bajo | Priorizar SP críticos (TH, OP, CC) |

---

## PRIORIZACIÓN DE MÓDULOS

| Prioridad | Módulo | SP a corregir | Justificación |
|-----------|--------|---------------|---------------|
| 🔴 **1** | TH (Talento Humano) | 56 | Core business, más usado |
| 🔴 **2** | OP (Operaciones) | 50 | Flujo de trabajo crítico |
| 🟠 **3** | CC (Cuentas/Costos) | 34 | Facturación y costos |
| 🟠 **4** | CORE (Workflow) | 6 | Sistema transversal |
| 🟡 **5** | MBO/PY/GD | 40 | Módulos secundarios |
| ⚪ **6** | Otros | 55 | Baja prioridad |

---

## DECISIONES PENDIENTES (Requieren Input)

1. **Módulos deprecados**: ¿Eliminar referencias a `CatiRMC_*`, `Sync_*`?
2. **SP nuevos**: Si un SP fue creado para MatrixNext y no existe en BD, ¿se crea en BD o se elimina del código?
3. **Ambiente de pruebas**: ¿Hay acceso a BD de desarrollo para verificar SP?

---

## SIGUIENTE PASO

**Aprobar este plan** para proceder con la implementación de FASE 13.

Una vez aprobado:
1. Ejecutar clasificación automática (13.1)
2. Corregir typos más evidentes (13.2)
3. Solicitar acceso a BD para verificación (13.3)

---

**Documento creado**: 2026-01-17  
**Estado**: 📋 PENDIENTE APROBACIÓN
