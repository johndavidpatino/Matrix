# ANÁLISIS DETALLADO SPRINT 10 & 11: MAPEO COREPROJECT

**Objetivo**: Documentar el mapeo exacto entre WebMatrix (legacy) y StoredProcedures/Tablas en CoreProject para facilitar migración en Sprint 10 y 11.

**Fecha**: 2026-01-15  
**Aplicable a**: RP_Reportes (Sprint 10), OP_RO (Sprint 11), OP_Trafico (Sprint 11)

---

## INSTRUCCIONES DE USO

Este documento debe llenarse **ANTES** de iniciar cada sprint:

1. **Día 1 de Sprint**: Abrir WebMatrix y CoreProject en paralelo
2. **Buscar patrones**: 
   - ¿Qué clase DataLayer usa cada .aspx.vb?
   - ¿Qué SP/tabla consulta?
   - ¿Qué parámetros recibe?
3. **Documentar aquí**: Completa la tabla correspondiente
4. **Validar**: Confirma SP en `CO_Matrix_Structure_SP.csv` o script SQL
5. **Actualizar DI**: Registra adapter + service en Program.cs

---

## ANÁLISIS SPRINT 10: RP_REPORTES

### 1. INVENTARIO INICIAL (VALIDAR EN WEBMATRIX)

**Ubicación**: `WebMatrix/RP_Reportes/`

**Archivos encontrados**: 72 .aspx files (según listado anterior)

**Clasificación por DataLayer esperada**:
```
REP_Model / REP_Context (probable)
├── Reportes (clase principal)
├── Indicadores
├── Planeacion
├── Recursos
├── Operacion
└── Especializados
```

**ACCIÓN**: Abrir `WebMatrix/RP_Reportes/DefaultMenu.aspx.vb` y buscar:
```vb
' Ejemplo esperado:
Imports Matrix.DataLayer.ReportesDAL
Imports Matrix.DataLayer.IndicadoresDAL

Public Class DefaultMenu
    Inherits Page
    
    Sub Page_Load(...)
        ' ¿Qué objetos crea?
        Dim rep As New ReportesDAL()
        Dim datos = rep.ObtenerReportesDisponibles()
    End Sub
End Class
```

---

### 2. MAPEO POR CATEGORÍA

#### CATEGORÍA 1: INDICADORES Y DASHBOARDS

| Archivo .aspx | DataLayer | SP/Tabla | Parámetros | Tipo Salida | Notas |
|---|---|---|---|---|---|
| IndicadoresCalidad.aspx | `IndicadoresDAL` | `[SP_NOMBRE_A_VALIDAR]` | @FechaDesde, @FechaHasta | DataTable | Buscar en .vb |
| IndicadoresCumplimientoTareas.aspx | ? | ? | ? | ? | TO DO |
| IndicadoresRegistroObservaciones.aspx | ? | ? | ? | ? | TO DO |
| Top10Encuestadores.aspx | ? | ? | ? | ? | TO DO |
| TrabajosConAtraso.aspx | ? | ? | ? | ? | TO DO |
| GanttUnTrabajo.aspx | ? | ? | ? | ? | TO DO |
| GanttRecursos.aspx | ? | ? | ? | ? | TO DO |
| ... | ... | ... | ... | ... | ... |

**INSTRUCCIONES PARA COMPLETAR**:
1. Abrir `IndicadoresCalidad.aspx.vb`
2. Buscar línea tipo: `Dim dal As New [NOMBRE]DAL()`
3. Buscar línea tipo: `connection.Execute("[SP_NAME]", ...)`
4. Completar tabla con información exacta
5. Confirmar SP en CoreProject (búsqueda de clase resultado o script SQL)

---

#### CATEGORÍA 2: REPORTES DE OPERACIÓN

| Archivo .aspx | DataLayer | SP/Tabla | Parámetros | Tipo Salida | Notas |
|---|---|---|---|---|---|
| ReporteActividades.aspx | ? | ? | ? | ? | TO DO |
| ReporteInconsistencias.aspx | ? | ? | ? | ? | TO DO |
| ReporteListadoTrabajos.aspx | ? | ? | ? | ? | TO DO |
| ... | ... | ... | ... | ... | ... |

---

#### CATEGORÍA 3: REPORTES DE PLANEACIÓN

| Archivo .aspx | DataLayer | SP/Tabla | Parámetros | Tipo Salida | Notas |
|---|---|---|---|---|---|
| PlaneacionCampo.aspx | ? | ? | ? | ? | TO DO |
| PlaneacionEstudios.aspx | ? | ? | ? | ? | TO DO |
| ... | ... | ... | ... | ... | ... |

---

#### CATEGORÍA 4: REPORTES DE RECURSOS

| Archivo .aspx | DataLayer | SP/Tabla | Parámetros | Tipo Salida | Notas |
|---|---|---|---|---|---|
| ListadoEncuestadores.aspx | ? | ? | ? | ? | TO DO |
| FichaEncuestador.aspx | ? | ? | ? | ? | TO DO |
| ... | ... | ... | ... | ... | ... |

---

#### CATEGORÍA 5: REPORTES ESPECIALIZADOS

| Archivo .aspx | DataLayer | SP/Tabla | Parámetros | Tipo Salida | Notas |
|---|---|---|---|---|---|
| ReportesCumplimientoAtiempoAlTiempo.aspx | ? | ? | ? | ? | TO DO |
| ReportesVariablesControl.aspx | ? | ? | ? | ? | TO DO |
| ... | ... | ... | ... | ... | ... |

---

### 3. MATRIZ DE SP CONSOLIDADA (RP_REPORTES)

**Paso**: Después de llenar tablas anterior, consolidar aquí

| # | SP Name | Tabla(s) Consumida(s) | Parámetros (Entrada) | Salida | Reportes Asociados | Prioridad |
|---|---|---|---|---|---|---|
| 1 | [SP_REP_001] | [Tabla1], [Tabla2] | @Param1, @Param2 | DataTable | Indicadores calidad | ALTA |
| 2 | [SP_REP_002] | [Tabla] | @FechaDesde, @FechaHasta | DataTable | Reporte actividades | MEDIA |
| ... | ... | ... | ... | ... | ... | ... |

**Total SP para RP_Reportes**: [Contar tras llenar tabla]

---

### 4. VALIDACIÓN EN CO_MATRIX_STRUCTURE_SP.CSV

**Paso**: Para cada SP en matriz anterior, validar que existe:

```powershell
# En MatrixNext/docs/SQL/
# Buscar línea: "SP_REP_001" o similar

# Si NO aparece:
# ⚠️ ALERTA: SP podría estar en tabla (no en SP) o ser legacy sin migrar
# Acción: Preguntar al DBA si SP existe en BD y cómo se llama exactamente
```

**Patrón esperado en CSV**:
```csv
SP_Name,Owner,Parameters,ReturnsResultSet,Description
REP_IndicadoresCalidad_Get,[dbo],@FechaDesde;@FechaHasta;@Usuario,1,Obtiene indicadores de calidad por rango fecha
```

---

---

## ANÁLISIS SPRINT 11: OP_RO

### 1. INVENTARIO INICIAL

**Ubicación**: `WebMatrix/OP_RO/`

**Archivos**: 5 .aspx files

```
OP_RO/
├── Cuestionario.aspx          → Revisar cuestionarios
├── Cuestionario.aspx.vb       → [CONTIENE DataLayer]
├── Cuestionario.aspx.designer.vb
├── Instructivo.aspx
├── Instructivo.aspx.vb        → [CONTIENE DataLayer]
├── MaterialAyuda.aspx
├── MaterialAyuda.aspx.vb      → [CONTIENE DataLayer]
├── Metodologia.aspx
└── Metodologia.aspx.vb        → [CONTIENE DataLayer]
```

---

### 2. MAPEO OP_RO DETALLADO

#### Revisión Cuestionarios

| Acción | Archivo | DataLayer | SP/Tabla | Parámetros | Salida | Lógica de Negocio |
|---|---|---|---|---|---|---|
| Listar Pendientes | Cuestionario.aspx | ? | `OP_RO_RevisionCuestionario_Get` | @Estado, @Usuario | List<Revisión> | Solo muestra "Pendiente" |
| Ver Detalle | Cuestionario.aspx (modal) | ? | `OP_RO_RevisionCuestionario_GetById` | @IdRevision | RevisionDetalle | Incluye preguntas del cuestionario |
| Crear Revisión | Cuestionario.aspx (post) | ? | `OP_RO_RevisionCuestionario_Save` | @IdCuestionario, @IdRevisor, @Observaciones | int (IdRevision) | Inserta registro, cambia estado cuestionario |
| Aprobar | Cuestionario.aspx (post) | ? | `OP_RO_RevisionCuestionario_Approve` | @IdRevision, @Observaciones | bit | Cambia estado a "Aprobado", notifica editor |
| Rechazar | Cuestionario.aspx (post) | ? | `OP_RO_RevisionCuestionario_Reject` | @IdRevision, @Razon | bit | Cambia estado a "Rechazado", vuelve al editor |

**ACCIÓN**: Abrir `Cuestionario.aspx.vb` y completar tabla

---

#### Revisión Instructivos

| Acción | Archivo | DataLayer | SP/Tabla | Parámetros | Salida | Lógica |
|---|---|---|---|---|---|---|
| Listar Pendientes | Instructivo.aspx | ? | `OP_RO_RevisionInstructivo_Get` | ? | ? | ? |
| Ver Detalle | ... | ? | `OP_RO_RevisionInstructivo_GetById` | ? | ? | ? |
| Aprobar | ... | ? | `OP_RO_RevisionInstructivo_Approve` | ? | ? | ? |
| Rechazar | ... | ? | `OP_RO_RevisionInstructivo_Reject` | ? | ? | ? |

**Patrón igual a Cuestionarios** (completar observando patrón)

---

#### Revisión Metodología

| Acción | Archivo | DataLayer | SP/Tabla | Parámetros | Salida | Lógica |
|---|---|---|---|---|---|---|
| Listar Pendientes | Metodologia.aspx | ? | `OP_RO_RevisionMetodologia_Get` | ? | ? | ? |
| ... | ... | ? | ... | ? | ? | ? |

---

#### Revisión Material Ayuda

| Acción | Archivo | DataLayer | SP/Tabla | Parámetros | Salida | Lógica |
|---|---|---|---|---|---|---|
| Listar Pendientes | MaterialAyuda.aspx | ? | `OP_RO_RevisionMaterialAyuda_Get` | ? | ? | ? |
| ... | ... | ? | ... | ? | ? | ? |

---

### 3. MATRIZ CONSOLIDADA OP_RO

| # | SP Name | Tabla(s) | Parámetros | Salida | Tipo Acción | Prioridad |
|---|---|---|---|---|---|---|
| 1 | OP_RO_RevisionCuestionario_Get | OP_RevisionCuestionario | @Estado, @Usuario | List | SELECT | ALTA |
| 2 | OP_RO_RevisionCuestionario_GetById | OP_RevisionCuestionario | @IdRevision | RevisionDetalle | SELECT | ALTA |
| 3 | OP_RO_RevisionCuestionario_Save | OP_RevisionCuestionario | @IdCuestionario, @IdRevisor, @Obs | int | INSERT | ALTA |
| 4 | OP_RO_RevisionCuestionario_Approve | OP_RevisionCuestionario | @IdRevision, @Obs | bit | UPDATE | ALTA |
| 5 | OP_RO_RevisionCuestionario_Reject | OP_RevisionCuestionario | @IdRevision, @Razon | bit | UPDATE | ALTA |
| 6-10 | OP_RO_RevisionInstructivo_* | OP_RevisionInstructivo | ... | ... | ... | ... |
| 11-15 | OP_RO_RevisionMetodologia_* | OP_RevisionMetodologia | ... | ... | ... | ... |
| 16-20 | OP_RO_RevisionMaterialAyuda_* | OP_RevisionMaterialAyuda | ... | ... | ... | ... |

**Total SP para OP_RO**: ~20 SP (4 tipos × 5 acciones)

---

### 4. VALIDACIÓN EN COREPROJECT

**Clase Esperada**: Buscar en CoreProject:
```vb
' OP_RO_RevisionCuestionario_Get_Result.vb
Partial Public Class OP_RO_RevisionCuestionario_Get_Result
    Public Property IdRevision As Integer
    Public Property IdCuestionario As Integer
    Public Property Estado As String
    Public Property Observaciones As String
    ' ... más propiedades
End Class

' Si existe → SP ya está definida en BD
' Si NO existe → Posible problema: SP falta o tabla tiene otro nombre
```

---

---

## ANÁLISIS SPRINT 11: OP_TRAFICO

### 1. INVENTARIO INICIAL

**Ubicación**: `WebMatrix/OP_Trafico/`

**Archivos**: 6 .aspx files

```
OP_Trafico/
├── Captura.aspx                    → Capturar encuestas
├── Critica.aspx                    → Criticar encuestas capturadas
├── InicioTraficoEncuestas.aspx     → Inicio flujo / dashboard
├── RMC.aspx                        → Gestión tráfico por RMC/ciudad
├── TrabajosProyectos.aspx          → Asignación a trabajos
└── Verificacion.aspx               → Verificación final
```

---

### 2. MAPEO OP_TRAFICO DETALLADO

#### Flujo Principal: Capturado → Criticado → Verificado

| Paso | Archivo | Acción | DataLayer | SP/Tabla | Parámetros | Salida | Lógica |
|---|---|---|---|---|---|---|---|
| 1 | InicioTraficoEncuestas.aspx | Dashboard inicial | ? | `OP_Trafico_GetDashboard` | @FechaDesde, @FechaHasta | DashboardDTO | Muestra KPIs por estado |
| 2 | Captura.aspx | Seleccionar trabajo | ? | `OP_Trabajos_GetActivos` | @Proyecto | List<Trabajo> | Carga trabajos para capturar |
| 3 | Captura.aspx | Registrar encuestas | ? | `OP_TraficoEncuestas_Save` | @IdTrabajo, @Cantidad, @Usuario, @Fecha | int | INSERT de encuestas, estado=Capturado |
| 4 | Critica.aspx | Listar para criticar | ? | `OP_TraficoEncuestas_Get` | @Estado=Capturado | List<Trafico> | Muestra encuestas sin criticar |
| 5 | Critica.aspx | Validar encuestas | ? | `OP_TraficoEncuestas_Critica` | @IdTrafico, @Validaciones, @Usuario | bit | UPDATE estado → Criticado |
| 6 | RMC.aspx | Agrupar por ciudad | ? | `OP_TraficoEncuestasCiudad` | @Ciudad, @Estado | List<TraficoRMC> | Agrupa por RMC (Revisor Metodología/Control) |
| 7 | RMC.aspx | Asignar a RMC | ? | `OP_TraficoEncuestasAsignacion` | @IdTrafico, @IdRMC, @Observaciones | bit | INSERT de asignación |
| 8 | Verificacion.aspx | Listar para verificar | ? | `OP_TraficoEncuestas_Get` | @Estado=Criticado | List<Trafico> | Muestra encuestas listas para verificación |
| 9 | Verificacion.aspx | Verificar final | ? | `OP_TraficoEncuestas_Verificacion` | @IdTrafico, @Usuario, @Observaciones | bit | UPDATE estado → Verificado (FINAL) |
| 10 | TrabajosProyectos.aspx | Cruzar con trabajos | ? | `OP_Trabajos_GetTrafico` | @IdTrabajo | List<TraficoTrabajoDTO> | Info de tráfico por trabajo |

**ACCIÓN**: Abrir `InicioTraficoEncuestas.aspx.vb` y completar tabla

---

### 3. MATRIZ CONSOLIDADA OP_TRAFICO

| # | SP Name | Tabla(s) Consumida(s) | Parámetros | Salida | Tipo | Prioridad |
|---|---|---|---|---|---|---|
| 1 | OP_Trafico_GetDashboard | OP_TraficoEncuestas | @FechaDesde, @FechaHasta | DashboardDTO | SELECT aggregate | ALTA |
| 2 | OP_Trabajos_GetActivos | OP_Trabajos | @IdProyecto | List<Trabajo> | SELECT | ALTA |
| 3 | OP_TraficoEncuestas_Save | OP_TraficoEncuestas | @IdTrabajo, @Cantidad, @Usuario, @Fecha | int | INSERT | ALTA |
| 4 | OP_TraficoEncuestas_Get | OP_TraficoEncuestas | @Estado, @FechaDesde, @FechaHasta | List<Trafico> | SELECT | ALTA |
| 5 | OP_TraficoEncuestas_Critica | OP_TraficoEncuestas | @IdTrafico, @Validaciones, @Usuario | bit | UPDATE | ALTA |
| 6 | OP_TraficoEncuestasCiudad | OP_TraficoEncuestas (+join) | @Ciudad, @Estado | List<TraficoRMC> | SELECT aggregate | MEDIA |
| 7 | OP_TraficoEncuestasAsignacion | OP_TraficoAsignacion | @IdTrafico, @IdRMC | bit | INSERT | MEDIA |
| 8 | OP_TraficoEncuestas_Verificacion | OP_TraficoEncuestas | @IdTrafico, @Usuario, @Obs | bit | UPDATE | ALTA |
| 9 | OP_Trabajos_GetTrafico | OP_Trabajos, OP_TraficoEncuestas | @IdTrabajo | List<TraficoTrabajoDTO> | SELECT join | MEDIA |

**Total SP para OP_Trafico**: ~9-10 SP

---

### 4. VALIDACIÓN EN COREPROJECT

**Clases Esperadas**:
```vb
' CoreProject/OP_TraficoEncuestas_Result.vb
' CoreProject/OP_RO_EjecucionCuestionario_Get_Result.vb
' etc.

' Si existe → SP definida
' Si NO existe → Investigar nombre exacto en BD
```

---

---

## RESUMEN: TOTALES Y CONSOLIDACIÓN

| Sprint | Módulo | Total SP Estimadas | Total Tablas | Complejidad |
|---|---|---|---|---|
| **10** | RP_Reportes | 25-30 | 8-10 | 🟡 MEDIA (muchos reportes simples) |
| **11A** | OP_RO | 20 | 4 | 🟡 MEDIA (workflow, 4 tipos) |
| **11B** | OP_Trafico | 9-10 | 3-4 | 🟠 MEDIA-BAJA (estado machine clara) |

**Total Esfuerzo Combinado**:
- Sprint 10: 60h = 10 días
- Sprint 11: 90h = 15 días

---

## PLANTILLA: COMPLETAR DURANTE SPRINT

**Instrucciones**:
1. Imprimir o copiar esta sección antes de iniciar sprint
2. Llenar una tabla completa POR CADA ARCHIVO .aspx.vb
3. Validar en CoreProject/SQL
4. Crear issue en GitHub si SP falta
5. Usar como fuente de verdad para implementar adapters

### TEMPLATE PARA LLENAR

```markdown
## [NOMBRE ARCHIVO].aspx

**Ubicación**: WebMatrix/[MODULO]/[ARCHIVO].aspx.vb

**DataLayer principal**: [Nombre clase]

**WebMethods/Acciones**:

| Acción | SP/Tabla | Parámetros | Salida | Validar SP |
|---|---|---|---|---|
| [Acción1] | | | | [ ] |
| [Acción2] | | | | [ ] |

**Notas**:
- [Cualquier observación]

**Validación**:
- [ ] SP confirmada en CoreProject
- [ ] SP confirmada en CO_Matrix_Structure_SP.csv
- [ ] Parámetros exactos documentados
```

---

**Documento**: 2026-01-15  
**Próxima revisión**: Día 1 de Sprint 10  
**Owner**: [DEV/TECH LEAD]

