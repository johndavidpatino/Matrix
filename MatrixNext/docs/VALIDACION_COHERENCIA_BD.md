# VALIDACIÓN DE COHERENCIA CON BASE DE DATOS

**Fecha de ejecución**: 2026-01-17  
**Proyecto**: MatrixNext  
**Fase**: FASE 12 - Validación de Coherencia BD

---

## RESUMEN EJECUTIVO

| Categoría | Total | Validados | Problemas | Estado |
|-----------|-------|-----------|-----------|--------|
| **Stored Procedures** | 622 usados | 381 OK | 241 no documentados | ⚠️ Revisar |
| **Tablas** | 703 en BD | 159 referenciadas | 68 posibles errores | 🔍 Analizar |
| **Vistas** | 314 en BD | - | Pendiente | ⏳ |
| **DTOs** | 174 archivos | - | Pendiente validación detallada | ⏳ |

---

## 1. VALIDACIÓN DE STORED PROCEDURES

### 1.1 Estadísticas Generales

- **SP documentados en BD**: 1,497
- **SP referenciados en código**: 622
- **SP NO encontrados en documentación**: 241 (después de filtrar)
- **Cobertura**: 61% de los SP usados están documentados

### 1.2 SP No Documentados por Módulo

| Módulo | Cantidad | Riesgo | Acción Recomendada |
|--------|----------|--------|-------------------|
| **TH** (Talento Humano) | 56 | 🔴 Alto | Verificar existencia en BD |
| **OP** (Operaciones) | 50 | 🔴 Alto | Verificar existencia en BD |
| **CC** (Cuentas/Costos) | 34 | 🟠 Medio | Documentar o eliminar referencias |
| **MBO** (MBO) | 15 | 🟠 Medio | Verificar y documentar |
| **PY** (Proyectos) | 15 | 🟠 Medio | Verificar y documentar |
| **UU** (Utilidades) | 11 | 🟡 Bajo | Revisar si son helpers internos |
| **GD** (Gestión Documental) | 10 | 🟡 Bajo | Documentar |
| **CatiRMC** | 7 | 🟡 Bajo | Sistema específico CATI |
| **Sync** | 7 | 🟡 Bajo | Sincronización |
| **CORE** (Workflow) | 6 | 🟠 Medio | Verificar workflow |
| **CU** (Cuantitativo) | 5 | 🟡 Bajo | Documentar |
| **Otros** | 25 | 🟡 Bajo | Revisar individualmente |

### 1.3 SP Críticos No Documentados (Requieren Acción Inmediata)

#### Módulo TH (Talento Humano)
```
TH_AjusteVacacionesEmpleado
TH_Ausentismo_Add
TH_Ausentismo_Get
TH_Ausencia_Add
TH_Ausencia_Update
TH_Beneficios_Calcular
TH_Beneficios_Get
TH_BeneficiosPendientes
TH_Causacion_Nomina
TH_CausarIncapacidad
TH_CausarVacacionesManual
TH_DiasBeneficio_Get
TH_DiasDisponibles_Get
TH_Empleado_GetById
TH_Empleados_GetByArea
TH_Empleados_Search
TH_Festivos_Get
TH_HistorialAusencias
TH_Incapacidades_Get
TH_Liquidacion_Get
TH_Nomina_Export
TH_Nomina_Get
TH_Permisos_Add
TH_Permisos_Get
TH_REP_Ausentismo
TH_REP_Beneficios
TH_REP_Incapacidades
TH_REP_Vacaciones
TH_REP_Vacaciones_Nomina
TH_REP_VacacionesDetallado
TH_Subordinados_Get
TH_TiposAusencia_Get
TH_Vacaciones_Add
TH_Vacaciones_Calcular
TH_Vacaciones_Get
TH_VacacionesAcumuladas
```

#### Módulo OP (Operaciones)
```
OP_Actividades_Get
OP_AsignarMuestra
OP_Auditoria_Export
OP_Auditoria_Get
OP_Ciudades_Get
OP_Coordinadores_Get
OP_Dashboard_Estadisticas
OP_Dashboard_Trabajos
OP_Encuestadores_Get
OP_Estimacion_Add
OP_Estimacion_Get
OP_Estimacion_Update
OP_ExportesAuditoria_Add
OP_Festivos_Get
OP_Historial_Get
OP_IPS_Add
OP_IPS_Get
OP_IPS_Update
OP_Metodologias_Get
OP_Muestra_Distribucion
OP_Muestra_Get
OP_MuestraEstudio_Get
OP_Produccion_Get
OP_Supervisor_Get
OP_Trabajos_Activos
OP_Trabajos_GetById
OP_Trabajos_Search
OP_Trafico_Add
OP_Trafico_Get
OP_Trafico_Update
```

#### Módulo CC (Cuentas/Costos)
```
CC_AprobarPresupuestoInterno
CC_CalculoJornada_Get
CC_CalculoJornada_Insert
CC_ConsolidacionProduccion
CC_Conteos_Delete
CC_Conteos_Insert
CC_DetallePresupuesto
CC_EliminarDetallePresupuestoInterno
CC_EliminarPresupuesto
CC_GenerarBonificacion
CC_GenerarRequerimientos
CC_GuardarAsignacionPresupuesto
CC_GuardarDetallePresupuesto
CC_GuardarDetallePresupuestoInterno
CC_GuardarDistribucionCostos
CC_GuardarPresupuesto
CC_GuardarPresupuestoInterno
CC_HistoricoPresupuestosInterno
CC_LiquidarPlanillas
CC_MuestraGenerarRequerimiento
CC_ObtenerPresupuestos
CC_Produccion_PendienteConsolidar
CC_ProductividadAgregada
CC_ReporteContabilizacionPST
CC_ReporteConteoTrabajos
CC_ReporteVarianzasPresupuestarias
CC_Requerimientos_Delete
CC_Requerimientos_Get
CC_ResumenConsolidacion
CC_ResumenesdeProduccion
CC_ResumenJornadas
CC_ResumenPresupuestosInternos
CC_TotalesConteoTrabajos
CC_VerificacionPresupuestosRealizados
```

### 1.4 Posibles Causas de SP No Documentados

1. **SP nuevos creados para MatrixNext** - No existían en WebMatrix original
2. **SP renombrados** - Nombres diferentes entre código y BD
3. **SP en schema diferente** - No en `dbo` sino en schema específico (ej: `TH_Ausencia.`)
4. **SP deprecados** - Ya no existen en BD pero siguen referenciados
5. **Errores de tipeo** - Nombres mal escritos en código

### 1.5 Recomendaciones SP

| Prioridad | Acción | Responsable | Plazo |
|-----------|--------|-------------|-------|
| 🔴 **Alta** | Verificar SP de TH y OP en BD de producción | DBA | 2 días |
| 🔴 **Alta** | Documentar SP nuevos si existen | Dev | 3 días |
| 🟠 **Media** | Corregir nombres si hay typos | Dev | 1 semana |
| 🟡 **Baja** | Eliminar referencias a SP obsoletos | Dev | 2 semanas |

---

## 2. VALIDACIÓN DE TABLAS

### 2.1 Estadísticas Generales

- **Tablas en BD**: 703
- **Tablas referenciadas en código**: 159
- **Referencias que NO son tablas**: ~68 (falsos positivos de regex)

### 2.2 Tablas Usadas (Muestra)

Las siguientes tablas fueron identificadas en queries directos (no via SP):

```
_Ciudades
_Constantes
_Festivos
_Generalidades
_Paises
ACM_AccionesMejora
ACM_Causas
aspnet_Membership
aspnet_Users
CC_Presupuestos
CC_PresupuestosDetalle
GD_Documentos
GD_Solicitudes
OP_Trabajos
OP_Muestra
PY_Proyectos
TH_Empleados
TH_Ausencias
US_Usuarios
US_Permisos
```

### 2.3 Convención de Nombres

| Patrón | Cantidad | Cumplimiento |
|--------|----------|--------------|
| `[MODULO]_[Entidad]` | 650 | ✅ 92% |
| `_[Entidad]` (catálogos) | 12 | ✅ Correcto |
| `aspnet_*` | 15 | ✅ Framework |
| Otros | 26 | ⚠️ Revisar |

---

## 3. VALIDACIÓN DE VISTAS

### 3.1 Estadísticas Generales

- **Vistas en BD**: 314
- **Prefijos más comunes**:
  - `MBO_*`: Reportes MBO
  - `CC_*`: Costos y Cuentas
  - `DD_*`: Data Dictionary/Validaciones
  - `DH_*`: Dashboard
  - `OP_*`: Operaciones

### 3.2 Vistas Más Usadas

```
MBO_OPCampoMuestra
MBO_PropuestasCreadasEnviadas
CC_CPYProduccionCedulasConProduccion
CC_CPYProduccionCedulasActivasPST
DH_CostosOperacionesMatrixAprobados1
```

---

## 4. VALIDACIÓN DE DTOs

### 4.1 Estadísticas Generales

- **Archivos DTO/Model**: 174
- **Módulos cubiertos**: CC, CORE, GD, INV, IT, MBO, OP, PC, PY, RE_GT, RP, SGC, TH, US

### 4.2 Patrones de Nomenclatura

| Patrón | Ejemplo | Estado |
|--------|---------|--------|
| `*Dto.cs` | `PresupuestoDto.cs` | ✅ Correcto |
| `*ViewModel.cs` | `AusenciaViewModel.cs` | ✅ Correcto |
| `*Model.cs` | `EmpleadoModel.cs` | ✅ Correcto |
| Propiedades PascalCase | `IdEmpleado`, `FechaInicio` | ✅ Correcto |
| Nullable con `?` | `string?`, `DateTime?` | ✅ Correcto |

### 4.3 Tipos de Datos Mapping

| C# Type | SQL Type | Validación |
|---------|----------|------------|
| `int` | `INT` | ✅ |
| `long` | `BIGINT` | ✅ |
| `decimal` | `DECIMAL/MONEY` | ✅ |
| `string` | `VARCHAR/NVARCHAR` | ✅ |
| `DateTime` | `DATETIME/DATE` | ✅ |
| `bool` | `BIT` | ✅ |

---

## 5. ARCHIVOS GENERADOS

Durante la validación se generaron los siguientes archivos de referencia:

| Archivo | Descripción | Ubicación |
|---------|-------------|-----------|
| `SP_Usados_Codigo.txt` | Lista de 622 SP referenciados | `docs/SQL/` |
| `SP_NoDocumentados.txt` | SP sin documentación en BD | `docs/SQL/` |
| `Tablas_BD.txt` | 703 tablas de BD | `docs/SQL/` |
| `Tablas_Usadas_Codigo.txt` | Tablas referenciadas en código | `docs/SQL/` |
| `Vistas_BD.txt` | 314 vistas de BD | `docs/SQL/` |

---

## 6. PLAN DE ACCIÓN

### Fase Inmediata (1-2 días)
- [ ] Ejecutar script de validación en BD de desarrollo
- [ ] Confirmar existencia de SP críticos (TH, OP, CC)
- [ ] Documentar SP nuevos que sí existen

### Fase Corta (1 semana)
- [ ] Corregir nombres de SP con typos
- [ ] Actualizar `CO_Matrix_SP_Names.csv` con SP faltantes
- [ ] Crear scripts de migración si hay SP obsoletos

### Fase Media (2 semanas)
- [ ] Validación completa de DTOs vs estructura de tablas
- [ ] Revisión de vistas usadas en reportes
- [ ] Actualizar documentación de BD

---

## 7. SCRIPTS DE VALIDACIÓN

### Script 1: Verificar SP en BD
```sql
-- Ejecutar en SQL Server Management Studio
DECLARE @SPList TABLE (SPName VARCHAR(255))
INSERT INTO @SPList VALUES 
('TH_AjusteVacacionesEmpleado'),
('TH_Ausentismo_Add'),
-- ... agregar más SP a verificar
('OP_Dashboard_Estadisticas')

SELECT 
    s.SPName,
    CASE WHEN p.name IS NOT NULL THEN 'EXISTE' ELSE 'NO EXISTE' END AS Estado
FROM @SPList s
LEFT JOIN sys.procedures p ON p.name = s.SPName
ORDER BY Estado DESC, s.SPName
```

### Script 2: Validar Parámetros de SP
```sql
SELECT 
    p.name AS StoredProcedure,
    par.name AS Parametro,
    t.name AS TipoDato,
    par.max_length,
    par.is_nullable
FROM sys.procedures p
INNER JOIN sys.parameters par ON p.object_id = par.object_id
INNER JOIN sys.types t ON par.system_type_id = t.system_type_id
WHERE p.name IN ('TH_AUSENCIA_GET', 'OP_Trabajos_Search')
ORDER BY p.name, par.parameter_id
```

---

## 8. CONCLUSIONES

### Estado General: ⚠️ Requiere Atención

1. **SP**: 39% de los SP usados no están documentados - necesita verificación urgente
2. **Tablas**: Convención de nombres bien aplicada (92%)
3. **DTOs**: Estructura correcta, pendiente validación detallada de columnas
4. **Vistas**: Bien organizadas por módulo

### Riesgo Principal

Los **241 SP no documentados** pueden ser:
- SP que **sí existen** pero no fueron incluidos en el export de documentación
- SP que **no existen** y causarán errores en runtime
- SP con **nombres incorrectos** (typos)

**Recomendación**: Ejecutar validación en BD real antes de despliegue.

---

**Documento generado**: 2026-01-17  
**Próxima revisión**: Después de validación en BD de desarrollo
