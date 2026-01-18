# REPORTE FINAL DE VALIDACIÓN DE COHERENCIA CON BD
# MatrixNext vs CO_Matrix_Intranet
# Fecha: 2026-01-17

## RESUMEN EJECUTIVO

### Estado Inicial
- **SP referenciados en código**: 365
- **SP válidos (existen en BD)**: 200
- **SP con problemas**: 165

### Estado Después de Correcciones Automáticas
- **SP referenciados en código**: 340
- **SP válidos (existen en BD)**: 202
- **SP con problemas**: 138

### Correcciones Aplicadas (27 SP corregidos)

#### TIPO 2: Nombres corregidos (14 SP)
| SP Anterior | SP Correcto | Estado |
|-------------|-------------|--------|
| TH_Empleado_Retirar | TH_Empleados_Retirar | ✅ Corregido |
| TH_Empleado_Reintegrar | TH_Empleados_Reintegrar | ✅ Corregido |
| TH_ExperienciaLaboral_InsertUpdate | TH_ExperienciaLaboral_Add | ✅ Corregido |
| TH_ExperienciaLaboral_Delete | TH_ExperienciaLaboral_Del | ✅ Corregido |
| TH_Educacion_InsertUpdate | TH_Educacion_Add | ✅ Corregido |
| TH_Educacion_Delete | TH_Educacion_Del | ✅ Corregido |
| TH_Hijos_InsertUpdate | TH_Hijos_Add | ✅ Corregido |
| TH_Hijos_Delete | TH_Hijos_Del | ✅ Corregido |
| TH_ContactosEmergencia_InsertUpdate | TH_ContactosEmergencia_Add | ✅ Corregido |
| TH_ContactosEmergencia_Delete | TH_ContactosEmergencia_Del | ✅ Corregido |
| TH_Promociones_Insert | TH_Promociones_Add | ✅ Corregido |
| TH_Salarios_Insert | TH_Salarios_Add | ✅ Corregido |
| TH_Areas_Get | TH_Area_Get | ✅ Corregido |
| TH_NivelesIngles_Get | TH_NivelesIdiomas_Get | ✅ Corregido |

#### TIPO 1: Catálogos cambiados de SP a SELECT directo (13 SP → TABLA)
| SP Inexistente | Tabla Real | Estado |
|----------------|------------|--------|
| TH_GruposSanguineos_Get | TH_GruposSanguineos | ✅ Cambiado a SELECT |
| TH_EstadosCiviles_Get | TH_EstadosCiviles | ✅ Cambiado a SELECT |
| TH_Bancos_Get | TH_Bancos | ✅ Cambiado a SELECT |
| TH_TiposCuenta_Get | TH_TiposCuentaBancaria | ✅ Cambiado a SELECT |
| TH_EPS_Get | TH_EPS | ✅ Cambiado a SELECT |
| TH_FondosPensiones_Get | TH_FondosPensiones | ✅ Cambiado a SELECT |
| TH_FondosCesantias_Get | TH_FondosCesantias | ✅ Cambiado a SELECT |
| TH_CajasCompensacion_Get | TH_CajasCompensacion | ✅ Cambiado a SELECT |
| TH_ARL_Get | TH_ARL | ✅ Cambiado a SELECT |
| TH_Sedes_Get | TH_Sedes | ✅ Cambiado a SELECT |
| TH_TiposContrato_Get | TH_TipoContratacion | ✅ Cambiado a SELECT |
| TH_NSE_Get | ⚠️ No encontrado | ⚠️ Pendiente verificar |
| TH_TallasCamiseta_Get | TH_TallasCamiseta | ✅ Cambiado a SELECT |

---

## SP PENDIENTES (138) - REQUIEREN DECISIÓN

Estos SP fueron creados en MatrixNext pero NO existen en la BD de producción.
Según las instrucciones: "la base es una base en producción y no se contempla la creación de tablas, cambios de campos o nuevos SP"

### Por Módulo

#### MBO Campo (15 SP) - CampoAdapter.cs
```
MBO_CampoCalidadGeneral
MBO_CampoCalidadPorCiudad
MBO_CampoCalidadPorEncuestador
MBO_CampoCargarErroresExcel
MBO_CampoCiudadesGet
MBO_CampoEncuestadoresGet
MBO_CampoEncuestasRealizadas
MBO_CampoErroresDelete
MBO_CampoErroresGet
MBO_CampoErroresInsert
MBO_CampoErroresUpdate
MBO_CampoEstadisticasEncuestas
MBO_CampoTiposErrorGet
MBO_CampoValidarErrores
```
**DECISIÓN REQUERIDA**: ¿Esta funcionalidad existía en WebMatrix? Verificar CoreProject.

#### OP RO - Revisión Operativa (20 SP) - OP_ROAdapter.cs
```
OP_RO_Cuestionario_GetById
OP_RO_Cuestionario_Save
OP_RO_Cuestionarios_Get
OP_RO_Fases_Get
OP_RO_Instructivo_GetById
OP_RO_Instructivo_Save
OP_RO_Instructivos_Get
OP_RO_Material_GetById
OP_RO_Material_Save
OP_RO_Materiales_Get
OP_RO_Metodologia_GetById
OP_RO_Metodologia_Save
OP_RO_Metodologias_Get
OP_RO_Pasos_Get
OP_RO_Preguntas_Get
OP_RO_Revision_Aprobar
OP_RO_Revision_GetById
OP_RO_Revision_Historial_Get
OP_RO_Revision_Rechazar
OP_RO_Revisiones_Get
```
**NOTA**: Existen SP similares en BD: `OP_RO_RevisionCuestionario_Get`, `OP_RO_EjecucionCuestionario_Get`, etc.

#### OP Tráfico (18 SP) - OP_TraficoAdapter.cs
```
OP_Trafico_Advertencias_Get
OP_Trafico_Anulado_GetById
OP_Trafico_Anulado_Save
OP_Trafico_Capturado_GetById
OP_Trafico_Capturado_Save
OP_Trafico_Criticado_GetById
OP_Trafico_Criticado_Save
OP_Trafico_Dashboard_Get
OP_Trafico_DatosCapturados_Get
OP_Trafico_Errores_Get
OP_Trafico_EstadisticasEstado_Get
OP_Trafico_Evento_GetById
OP_Trafico_Evento_Historial_Get
OP_Trafico_Eventos_Get
OP_Trafico_Inconsistencias_Get
OP_Trafico_Verificado_GetById
OP_Trafico_Verificado_Save
OP_TraficoEncuestas_Enviar
```
**NOTA**: Existen SP relacionados: `OP_TraficoEncuestas_Get`, `OP_TraficoEncuesta_GetCritica`.

#### GD - Gestión Documental (9 SP)
```
GD_ConfiguracionRevision_Get
GD_Email_EnviarNotificacion
GD_RevisoresPorDefecto_Get
GD_SolDocumentos_Get
GD_SolicitudDocumentos_Add
GD_SolicitudDocumentos_CambiarEstado
GD_SolicitudDocumentos_Get
GD_SolicitudDocumentos_GetById
GD_SolicitudDocumentos_Update
```

#### TH - Talento Humano (20 SP)
```
TH_Ausencia_Causacion
TH_Desvinculacion_Evaluacion_Save
TH_Desvinculacion_Evaluaciones_Get
TH_Desvinculacion_Finalizar
TH_Desvinculacion_GenerarPDF
TH_Desvinculacion_Get
TH_Desvinculacion_Iniciar
TH_Educacion_Edit
TH_Empleado_ActualizarDatosGenerales
TH_Empleado_ActualizarDatosLaborales
TH_Empleado_ActualizarDatosPersonales
TH_Empleado_ActualizarNivelIngles
TH_Empleado_ActualizarNomina
TH_Empleado_GetPorIdentificacion
TH_Empleado_InsertUpdate
TH_Empleados_DatosLaborales_ActualizarSalario
TH_ExperienciaLaboral_Edit
TH_FichaEncuestador_Get
TH_ListadoEncuestadores_Get
TH_REP_Vacaciones
TH_REP_Vacaciones_Nomina
TH_ReporteDiligenciamientoEmpleados_Get
```

#### PY - Proyectos (12 SP)
```
PY_ControlCalidad_GetByTipo
PY_ControlCalidad_GetByTrabajo
PY_InHomeVisit_Save
PY_PlaneacionCampo_Get
PY_PlaneacionEstudios_Get
PY_Preguntas_GetByTipo
PY_Trabajo.AsignarCampo
PY_Trabajo.GuardarLogAsignacion
PY_Trabajos_UpdateEstado
PY_TrabajosConfiguracion_Add
PY_TrabajosConfiguracionGet
PY_TrabajosDuplicar
PY_VariablesControl_Add
```

#### UU - Planillas (11 SP)
```
UU_ModeradoresGet
UU_PlanillaInformes_Add
UU_PlanillaInformes_Update
UU_PlanillaInformesGetBy
UU_PlanillaModeracion_Add
UU_PlanillaModeracion_Update
UU_PlanillaModeracionGetBy
UU_PlanillasGet
UU_PlanillasInformesExport
UU_PlanillasModeracionExport
UU_TecnicasGet
```

#### Otros (33 SP)
```
AUTH_ValidarOwnership
CC_GenerarBonificacion
CC_LiquidarPlanillas
CU_Presupuestos.DevolverxIdPropuestaAprobados
CU_Presupuestos.ObtenerPresupuestosAsignadosXEstudio
GrabarAuditoria
IQ_JBI.CambiarJBI
IQ_JBI.GuardarLogCambios
obtenerRespuestaIdRegistroXIdTrabajoNumeroEncuesta
obtenerTrabajosWorkFlow
OP_FichaTranscripciones_Add
OP_FichaTranscripciones_Edit
OP_FichaTranscripciones_Get
OP_ObtenerRespuestasFiltro
OP_PersonalSinProduccion_Get
OP_ReporteActividades_Get
OP_ReporteInconsistencias_Get
OP_ReporteListadoTrabajos_Get
OP_SupervisionCampoTelefonico_Save
OP_Trabajos_Activos
REP_IndicadoresCalidad_Get
REP_IndicadoresCumplimiento_Get
REP_ReportesDisponibles_Get
Sync_EncuestaPiloto
Sync_EncuestasEntrenamiento
Sync_ErrorTrabajoEspecializado
Sync_HabilitarEncuestasPiloto
Sync_HabilitarSincronizacionEstudio
Sync_Preguntas_Get
Sync_Preguntas_UpdateInfo
US_Usuario_TieneRol
```

---

## OPCIONES DE RESOLUCIÓN

### Opción A: Crear SP faltantes en BD
- **Pros**: MatrixNext funcionaría sin cambios
- **Contras**: Viola la regla de no crear SP nuevos en producción
- **Esfuerzo**: Alto (crear 138 SP)

### Opción B: Eliminar funcionalidad no existente en legacy
- **Pros**: Mantiene paridad con WebMatrix
- **Contras**: Puede eliminar funcionalidad útil
- **Esfuerzo**: Medio (eliminar código)

### Opción C: Mapear a SP existentes similares
- **Pros**: Reutiliza infraestructura existente
- **Contras**: Requiere análisis detallado de cada SP
- **Esfuerzo**: Alto (análisis + refactoring)

### Opción D: Implementar con SQL directo (sin SP)
- **Pros**: No requiere cambios en BD
- **Contras**: Menos mantenible, posibles problemas de seguridad
- **Esfuerzo**: Medio (cambiar llamadas SP a SQL)

---

## RECOMENDACIÓN

1. **Priorizar verificación en CoreProject** para determinar qué funcionalidad realmente existía en WebMatrix
2. **Para funcionalidad nueva (no existía)**: Evaluar si es necesaria o se elimina
3. **Para funcionalidad existente con SP diferente**: Mapear al SP correcto
4. **Para catálogos**: Usar SELECT directo de tablas (ya aplicado en TH)

---

## MÉTRICAS

| Métrica | Valor |
|---------|-------|
| SP en BD | 1,497 |
| Tablas en BD | 723 |
| Vistas en BD | 314 |
| SP referenciados (inicial) | 365 |
| SP referenciados (actual) | 340 |
| SP válidos | 202 (59.4%) |
| SP inválidos | 138 (40.6%) |
| Correcciones aplicadas | 27 |
| Archivos modificados | 1 |

---

## ARCHIVOS MODIFICADOS

1. `MatrixNext.Data/Modules/TH/Empleados/Adapters/EmpleadoDataAdapter.cs`
   - 14 correcciones de nombres de SP
   - 13 cambios de SP a SELECT de tablas

---

## PRÓXIMOS PASOS

1. [ ] Verificar en CoreProject qué SP/funcionalidad realmente existía
2. [ ] Tomar decisión por módulo (crear SP, eliminar, o mapear)
3. [ ] Aplicar correcciones en archivos restantes
4. [ ] Re-ejecutar validación hasta 0 errores
5. [ ] Documentar decisiones tomadas

