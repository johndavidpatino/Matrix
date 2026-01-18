# CORRECCIONES DE COHERENCIA CON BD - MatrixNext
# Fecha: 2026-01-17
# Estado: EN PROCESO DE CORRECCIÓN

## RESUMEN EJECUTIVO

De la validación realizada contra la BD de producción `CO_Matrix_Intranet`:
- **SP referenciados en código**: 365
- **SP válidos (existen en BD)**: 200 ✅
- **SP que NO existen**: 165 ❌

Este documento clasifica los 165 SP problemáticos y define las acciones correctivas.

---

## CLASIFICACIÓN DE ERRORES

### TIPO 1: TABLAS usadas como SP (27 casos)
**Acción**: Cambiar de Dapper SP call a EF Core o SELECT directo

| SP Incorrecto | Objeto Real | Tipo | Archivo |
|---------------|-------------|------|---------|
| TH_ARL_Get | TH_ARL | TABLA | EmpleadoDataAdapter.cs |
| TH_Bancos_Get | TH_Bancos | TABLA | EmpleadoDataAdapter.cs |
| TH_CajasCompensacion_Get | TH_CajasCompensacion | TABLA | EmpleadoDataAdapter.cs |
| TH_EPS_Get | TH_EPS | TABLA | EmpleadoDataAdapter.cs |
| TH_EstadosCiviles_Get | TH_EstadosCiviles | TABLA | EmpleadoDataAdapter.cs |
| TH_FondosCesantias_Get | TH_FondosCesantias | TABLA | EmpleadoDataAdapter.cs |
| TH_FondosPensiones_Get | TH_FondosPensiones | TABLA | EmpleadoDataAdapter.cs |
| TH_GruposSanguineos_Get | TH_GruposSanguineos | TABLA | EmpleadoDataAdapter.cs |
| TH_Sedes_Get | TH_Sedes | TABLA | EmpleadoDataAdapter.cs |
| TH_TallasCamiseta_Get | TH_TallasCamiseta | TABLA | EmpleadoDataAdapter.cs |

### TIPO 2: SP con nombre incorrecto (25 casos)
**Acción**: Corregir nombre al SP real que existe

| SP Incorrecto | SP Correcto | Archivo |
|---------------|-------------|---------|
| TH_Areas_Get | TH_Area_Get | EmpleadoDataAdapter.cs |
| TH_ContactosEmergencia_Delete | TH_ContactosEmergencia_Del | EmpleadoDataAdapter.cs |
| TH_ContactosEmergencia_InsertUpdate | TH_ContactosEmergencia_Add | EmpleadoDataAdapter.cs |
| TH_Educacion_Delete | TH_Educacion_Del | EmpleadoDataAdapter.cs |
| TH_Educacion_Edit | TH_Educacion_Get + validar | ThEducacionAdapter.cs |
| TH_Educacion_InsertUpdate | TH_Educacion_Add | EmpleadoDataAdapter.cs |
| TH_Empleado_ActualizarDatosGenerales | TH_Empleados_DatosGenerales_Edit | EmpleadoDataAdapter.cs |
| TH_Empleado_ActualizarDatosLaborales | TH_Empleados_DatosLaborales_Edit | EmpleadoDataAdapter.cs |
| TH_Empleado_ActualizarDatosPersonales | TH_Empleados_DatosPersonales_Edit | EmpleadoDataAdapter.cs |
| TH_Empleado_ActualizarNivelIngles | TH_Empleados_NivelIngles_Edit | EmpleadoDataAdapter.cs |
| TH_Empleado_ActualizarNomina | TH_Empleados_Nomina_Edit | EmpleadoDataAdapter.cs |
| TH_Empleado_InsertUpdate | TH_Empleados_DatosGenerales_Add | EmpleadoDataAdapter.cs |
| TH_Empleado_Reintegrar | TH_Empleados_Reintegrar | EmpleadoDataAdapter.cs |
| TH_Empleado_Retirar | TH_Empleados_Retirar | EmpleadoDataAdapter.cs |
| TH_ExperienciaLaboral_Delete | TH_ExperienciaLaboral_Del | EmpleadoDataAdapter.cs |
| TH_ExperienciaLaboral_Edit | TH_ExperienciaLaboral_Get + validar | ThExperienciaLaboralAdapter.cs |
| TH_ExperienciaLaboral_InsertUpdate | TH_ExperienciaLaboral_Add | EmpleadoDataAdapter.cs |
| TH_Hijos_Delete | TH_Hijos_Del | EmpleadoDataAdapter.cs |
| TH_Hijos_InsertUpdate | TH_Hijos_Add | EmpleadoDataAdapter.cs |
| TH_Promociones_Insert | TH_Promociones_Add | EmpleadoDataAdapter.cs |
| TH_Salarios_Insert | TH_Salarios_Add | EmpleadoDataAdapter.cs |
| TH_NivelesIngles_Get | TH_NivelesIdiomas_Get | EmpleadoDataAdapter.cs |
| TH_TiposContrato_Get | Ver TH_TipoContratacion (TABLA) | EmpleadoDataAdapter.cs |
| TH_TiposCuenta_Get | TH_TiposCuentaBancaria (TABLA) | EmpleadoDataAdapter.cs |

### TIPO 3: SP INVENTADOS - NO EXISTEN EN LEGACY (85+ casos)
**Acción**: ELIMINAR funcionalidad o buscar alternativa en legacy

#### 3.1 Módulo MBO Campo (15 SP inventados)
Estos SP fueron creados en MatrixNext pero NO existen en BD:
- MBO_CampoCalidadGeneral
- MBO_CampoCalidadPorCiudad
- MBO_CampoCalidadPorEncuestador
- MBO_CampoCargarErroresExcel
- MBO_CampoCiudadesGet
- MBO_CampoEncuestadoresGet
- MBO_CampoEncuestasRealizadas
- MBO_CampoErroresDelete
- MBO_CampoErroresGet
- MBO_CampoErroresInsert
- MBO_CampoErroresUpdate
- MBO_CampoEstadisticasEncuestas
- MBO_CampoTiposErrorGet
- MBO_CampoValidarErrores

**DECISIÓN REQUERIDA**: ¿Crear SP o eliminar funcionalidad?

#### 3.2 Módulo OP RO - Revisión Operativa (20 SP inventados)
- OP_RO_Cuestionario_GetById
- OP_RO_Cuestionario_Save
- OP_RO_Cuestionarios_Get
- OP_RO_Fases_Get
- OP_RO_Instructivo_GetById
- OP_RO_Instructivo_Save
- OP_RO_Instructivos_Get
- OP_RO_Material_GetById
- OP_RO_Material_Save
- OP_RO_Materiales_Get
- OP_RO_Metodologia_GetById
- OP_RO_Metodologia_Save
- OP_RO_Metodologias_Get
- OP_RO_Pasos_Get
- OP_RO_Preguntas_Get
- OP_RO_Revision_Aprobar
- OP_RO_Revision_GetById
- OP_RO_Revision_Historial_Get
- OP_RO_Revision_Rechazar
- OP_RO_Revisiones_Get

**NOTA**: Existen SP similares como `OP_RO_RevisionCuestionario_Get`, `OP_RO_EjecucionCuestionario_Get`, etc. VERIFICAR si la funcionalidad se puede mapear a estos.

#### 3.3 Módulo OP Tráfico (18 SP inventados)
- OP_Trafico_Advertencias_Get
- OP_Trafico_Anulado_GetById
- OP_Trafico_Anulado_Save
- OP_Trafico_Capturado_GetById
- OP_Trafico_Capturado_Save
- OP_Trafico_Criticado_GetById
- OP_Trafico_Criticado_Save
- OP_Trafico_Dashboard_Get
- OP_Trafico_DatosCapturados_Get
- OP_Trafico_Errores_Get
- OP_Trafico_EstadisticasEstado_Get
- OP_Trafico_Evento_GetById
- OP_Trafico_Evento_Historial_Get
- OP_Trafico_Eventos_Get
- OP_Trafico_Inconsistencias_Get
- OP_Trafico_Verificado_GetById
- OP_Trafico_Verificado_Save
- OP_TraficoEncuestas_Enviar

**NOTA**: Existen SP relacionados como `OP_TraficoEncuestas_Get`, `OP_TraficoEncuesta_GetCritica`, etc.

#### 3.4 Módulo GD - Gestión Documental (8 SP inventados)
- GD_ConfiguracionRevision_Get
- GD_Email_EnviarNotificacion
- GD_RevisoresPorDefecto_Get
- GD_SolDocumentos_Get
- GD_SolicitudDocumentos_Add
- GD_SolicitudDocumentos_CambiarEstado
- GD_SolicitudDocumentos_Get
- GD_SolicitudDocumentos_GetById
- GD_SolicitudDocumentos_Update

#### 3.5 Módulo PY - Proyectos (12 SP inventados)
- PY_ControlCalidad_GetByTipo
- PY_ControlCalidad_GetByTrabajo
- PY_InHomeVisit_Save
- PY_PlaneacionCampo_Get
- PY_PlaneacionEstudios_Get
- PY_Preguntas_GetByTipo
- PY_Trabajo.AsignarCampo
- PY_Trabajo.GuardarLogAsignacion
- PY_Trabajos_UpdateEstado
- PY_TrabajosConfiguracion_Add
- PY_TrabajosConfiguracionGet
- PY_TrabajosDuplicar
- PY_VariablesControl_Add

#### 3.6 Módulo UU - Planillas (11 SP inventados)
- UU_ModeradoresGet
- UU_PlanillaInformes_Add
- UU_PlanillaInformes_Update
- UU_PlanillaInformesGetBy
- UU_PlanillaModeracion_Add
- UU_PlanillaModeracion_Update
- UU_PlanillaModeracionGetBy
- UU_PlanillasGet
- UU_PlanillasInformesExport
- UU_PlanillasModeracionExport
- UU_TecnicasGet

#### 3.7 Otros módulos
- AUTH_ValidarOwnership
- CC_GenerarBonificacion
- CC_LiquidarPlanillas
- CU_Presupuestos.DevolverxIdPropuestaAprobados
- CU_Presupuestos.ObtenerPresupuestosAsignadosXEstudio
- GrabarAuditoria
- IQ_JBI.CambiarJBI
- IQ_JBI.GuardarLogCambios
- OP_FichaTranscripciones_Add
- OP_FichaTranscripciones_Edit
- OP_FichaTranscripciones_Get
- OP_ObtenerRespuestasFiltro
- OP_PersonalSinProduccion_Get
- OP_ReporteActividades_Get
- OP_ReporteInconsistencias_Get
- OP_ReporteListadoTrabajos_Get
- OP_SupervisionCampoTelefonico_Save
- OP_Trabajos_Activos
- REP_IndicadoresCalidad_Get
- REP_IndicadoresCumplimiento_Get
- REP_ReportesDisponibles_Get
- Sync_EncuestaPiloto
- Sync_EncuestasEntrenamiento
- Sync_ErrorTrabajoEspecializado
- Sync_HabilitarEncuestasPiloto
- Sync_HabilitarSincronizacionEstudio
- Sync_Preguntas_Get
- Sync_Preguntas_UpdateInfo
- TH_Ausencia_Causacion
- TH_Desvinculacion_Evaluacion_Save
- TH_Desvinculacion_Evaluaciones_Get
- TH_Desvinculacion_Finalizar
- TH_Desvinculacion_GenerarPDF
- TH_Desvinculacion_Get
- TH_Desvinculacion_Iniciar
- TH_Empleado_GetPorIdentificacion
- TH_Empleados_DatosLaborales_ActualizarSalario
- TH_FichaEncuestador_Get
- TH_ListadoEncuestadores_Get
- TH_REP_Vacaciones
- TH_REP_Vacaciones_Nomina
- TH_ReporteDiligenciamientoEmpleados_Get
- US_Usuario_TieneRol
- obtenerRespuestaIdRegistroXIdTrabajoNumeroEncuesta
- obtenerTrabajosWorkFlow

---

## PLAN DE ACCIÓN

### Fase 1: Correcciones Inmediatas (TIPO 1 y 2)
**Tiempo estimado**: 4-6 horas
**Prioridad**: ALTA

1. Corregir nombres de SP incorrectos (TIPO 2)
2. Cambiar llamadas a tablas de Dapper a EF Core (TIPO 1)

### Fase 2: Decisión sobre SP Inventados (TIPO 3)
**Requiere**: Reunión con equipo de proyecto
**Opciones**:
- A) Crear los SP faltantes en BD (viola regla de no crear SP nuevos)
- B) Eliminar funcionalidad no existente en legacy
- C) Mapear a funcionalidad existente alternativa

### Fase 3: Verificación Legacy
Para cada SP inventado, verificar en CoreProject/WebMatrix:
1. ¿Existía esta funcionalidad?
2. ¿Cómo se implementaba (EF, SQL directo, otro SP)?
3. ¿Es funcionalidad nueva que no debería existir?

---

## ARCHIVOS QUE REQUIEREN CORRECCIÓN

| Archivo | Errores | Tipo |
|---------|---------|------|
| EmpleadoDataAdapter.cs | 25+ | TIPO 1 y 2 |
| CampoAdapter.cs | 15 | TIPO 3 |
| OP_TraficoAdapter.cs | 18 | TIPO 3 |
| OP_ROAdapter.cs | 20 | TIPO 3 |
| ThDesvinculacionAdapter.cs | 6 | TIPO 3 |
| SolicitudesAdapter.cs | 8 | TIPO 3 |
| PyPlanillasAdapter.cs | 11 | TIPO 3 |
| ITSyncAdapter.cs | 8 | TIPO 3 |
| ReportesAdapter.cs | 7 | TIPO 3 |

---

## SIGUIENTE PASO

1. ✅ Validación completada
2. ⏳ Aplicar correcciones TIPO 1 y 2 (automáticas)
3. ⏳ Revisar legacy para SP TIPO 3
4. ⏳ Generar reporte final

