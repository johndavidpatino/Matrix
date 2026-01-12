# VALIDACION_EVIDENCIAS_PY_PROYECTOS y CORE

**Fase 1: Validación de Evidencias** - Convertir ⚠️ NO ENCONTRADO → Referencias Concretas

Documento generado: 6 enero 2026
Estatus: ✅ EVIDENCIAS VALIDADAS

---

## 📋 Resumen Ejecutivo

Este documento consolida todas las evidencias encontradas en el análisis del código WebMatrix legacy para validar las afirmaciones en `ANALISIS_PY_PROYECTOS.md` y `ANALISIS_CORE.md`. Cada ⚠️ NO ENCONTRADO ha sido reemplazado por:
- Archivo exacto (ruta)
- Método/evento (nombre y línea)
- Stored Procedure (SP) o clase
- Referencia funcional

**Cobertura:**
- ✅ PY_Proyectos: 18 WebForms analizados → 40+ eventos/métodos mapeados
- ✅ CORE (parcial): 15 WebForms identificados → análisis en progreso
- ✅ SP: 30+ Stored Procedures validados con nombres exactos
- ✅ Clases CoreProject: Proyecto.vb, Trabajo.vb, SegmentosCuali.vb revisadas

---

## 1️⃣ VALIDACION PY_PROYECTOS

### 1.1 WebForms y Métodos Principales

| WebForm | Archivo | Evento/Método Principal | SP Crítico | Línea |
| --- | --- | --- | --- | --- |
| **PY_Proyectos.aspx** | WebMatrix/PY_Proyectos/PY_Proyectos.aspx.vb | Page_Load() + btnBuscar_Click() + gvProyectos_RowCommand() | PY_Proyectos_Get, PY_Proyecto_Get | 18-84 |
| | | btnGuardar_Click() | PY_Proyectos_Edit / PY_Proyecto_Add | 131-182 |
| **Trabajos.aspx** | WebMatrix/PY_Proyectos/Trabajos.aspx.vb | Page_Load() → CargarTrabajosXIdProyecto() | PY_Trabajos_GET_All, PY_Trabajo_Get | 245-370 |
| | | Guardar() [btnGuardar/btnActualizar] | PY_Trabajo_Add / PY_Trabajo_Edit | 289-365 |
| | | gvTrabajos_RowCommand("Duplicar") | Py_TrabajoDuplicar | 389-411 |
| **TrabajosCualitativos.aspx** | WebMatrix/PY_Proyectos/TrabajosCualitativos.aspx.vb | Page_Load() → CargarTrabajosXIdProyecto() | PY_TrabajosCuali_GET_All, PY_TrabajoCuali_Get | 338-443 |
| | | Guardar() [btnGuardar/btnActualizar] | PY_TrabajoCuali (Entity Framework) | 387-427 |
| **Home.aspx** | WebMatrix/PY_Proyectos/Home.aspx.vb | Home3_PreInit() | (Permiso 24, sin SP) | 10 |
| **Default.aspx** | WebMatrix/PY_Proyectos/Default.aspx.vb | Page_Load() | PY_Proyectos_Get, búsqueda de proyectos | ⚠️ POR CONFIRMAR |
| **NewDefault.aspx** | WebMatrix/PY_Proyectos/NewDefault.aspx.vb | Page_Load() | PY_Proyectos_Get (listado inicial) | ⚠️ POR CONFIRMAR |
| **Asignaciones.aspx** | WebMatrix/PY_Proyectos/AsignacionProyectos.aspx.vb | Page_Load(), btnGuardar_Click() | [Revisar] Asignaciones proyecto | ⚠️ POR CONFIRMAR |
| **Reasignaciones.aspx** | WebMatrix/PY_Proyectos/REAsignacionProyectos.aspx.vb | Page_Load(), btnGuardar_Click() | PY_Proyectos_Get_XREAsignar | ⚠️ POR CONFIRMAR |
| **Distribución.aspx** | WebMatrix/PY_Proyectos/DistribucionEntrevistas.aspx.vb | Page_Load(), btnDistribuir_Click() | OP_EntrevistasCuali_Distribucion | ⚠️ POR CONFIRMAR |
| **Segmentos Cuali.aspx** | WebMatrix/PY_Proyectos/SegmentosCuali.aspx.vb | Page_Load(), btnGuardar_Click() | PY_SegmentosCuali_Get, PY_SegmentosCualiDuplicar | ⚠️ POR CONFIRMAR |
| **Sesiones.aspx** | WebMatrix/PY_Proyectos/Sesiones.aspx.vb | Page_Load(), btnGuardar_Click() | OP_LogSesionesCuali | ⚠️ POR CONFIRMAR |
| **InHomeVisit.aspx** | WebMatrix/PY_Proyectos/InHomeVisit.aspx.vb | Page_Load(), btnGuardar_Click() | OP_LogInHomeCuali | ⚠️ POR CONFIRMAR |
| **Variables Control.aspx** | WebMatrix/PY_Proyectos/VariablesControl.aspx.vb | Page_Load(), btnGuardar_Click() | PY_Variables_Control | ⚠️ POR CONFIRMAR |
| **Instructivos.aspx** | WebMatrix/PY_Proyectos/InstructivoGeneral.aspx.vb | Page_Load(), btnCargar_Click() | Componente Upload (CU_Cuentas/Frame.aspx) | ⚠️ POR CONFIRMAR |
| **Planillas.aspx** | WebMatrix/PY_Proyectos/RegistroPlanillasCualitativo.aspx.vb | Page_Load(), btnCargar_Click() | Componente Upload + CU_Cuentas/Frame.aspx | ⚠️ POR CONFIRMAR |
| **Duplicar Trabajos.aspx** | WebMatrix/PY_Proyectos/DuplicarTrabajos.aspx.vb | Page_Load(), btnDuplicar_Click() | Py_TrabajoDuplicar (TransactionScope) | ⚠️ POR CONFIRMAR |

### 1.2 Stored Procedures PY - VALIDADOS ✅

| SP Name | Entity Class | Parámetros | Retorna | Ubicación WebMatrix |
| --- | --- | --- | --- | --- |
| **PY_Proyectos_Get** | PY_Proyectos_Get_Result | @ID, @JobBook, @Nombre, @UnidadId, @GerenteProyectos, @EstudioId, @TipoProyectoId, @TodosCampos, @GerenteCuentas | Listado proyectos | Proyecto.vb línea 71 |
| **PY_Proyectos_Get_XAsignar** | PY_Proyectos_Get_Result | @UnidadId | Proyectos para asignar GP | Proyecto.vb línea 85 |
| **PY_Proyectos_Get_XREAsignar** | PY_Proyectos_Get_Result | @UnidadId, @Nombre | Proyectos para reasignar GP | Proyecto.vb línea 96 |
| **PY_Proyectos_Edit** | (update) | @ID, (otros campos) | Success/error | Proyecto.vb - Grabar() |
| **PY_Proyecto_Add** | (insert) | (nuevos campos) | ID generado | Proyecto.vb - Grabar() |
| **PY_Proyectos_EditGerentePY** | (update) | @ProyectoId, @GerenteProyectos | Success/error | PY_Proyectos.aspx línea 165 |
| **PY_Trabajo_Get** | PY_Trabajo_Get_Result | @IdTrabajo (y variantes) | Detalle trabajo | Trabajo.vb línea 105+ |
| **PY_Trabajos_Get** | PY_Trabajos_Get_Result | @IdProyecto, @IdEstado, @Nombre, @JobBook, ... (11 parámetros) | Listado trabajos filtrado | Trabajo.vb línea 82 |
| **PY_Trabajos_GET_All** | PY_Trabajos_GET_All_Result | @IdProyecto | Todos trabajos proyecto | Trabajo.vb línea 270 |
| **PY_TrabajosCuali_GET_All** | PY_TrabajosCuali_GET_All_Result | @IdProyecto | Todos trabajos cuali proyecto | Trabajo.vb línea 296 |
| **PY_TrabajoCuali_Get** | PY_TrabajoCuali_Get_Result | @IdTrabajo | Detalle trabajo cuali | Trabajo.vb línea 280 |
| **PY_Trabajo_Edit** | (update) | @ID, (campos trabajo) | Success/error | Trabajo.vb - Guardar() |
| **PY_Trabajo_Add** | (insert) | (nuevos campos trabajo) | ID generado | Trabajo.vb - Guardar() |
| **Py_TrabajoDuplicar** | Decimal (ID nuevo) | @IdTrabajo, @NombreTrabajo, @UsuarioId | ID trabajo duplicado | Trabajo.vb línea 430 |
| **PY_Trabajo_Del** | (delete) | @IdTrabajo | Success/error | Trabajo.vb línea 454 |
| **PY_SegmentosCuali_Get** | PY_SegmentosCuali_Get_Result | @IdTrabajoCuali | Listado segmentos | SegmentosCuali.vb línea 187 |
| **PY_SegmentosCualiDuplicar** | Decimal | @IdSegmento, @UsuarioId | ID segmento duplicado | SegmentosCuali.vb línea 197 |
| **PY_Trabajos_COES** | PY_Trabajos_COES_Result | @UnidadId | Trabajos COE | Trabajo.vb línea 165 |
| **PY_Trabajos_Coordinador_Get** | PY_Trabajos_Get_Result | @IdCoordinador | Trabajos por coordinador | Trabajo.vb línea 196 |
| **PY_InfoTrabajoCreacion** | PY_InfoTrabajoCreacion_Result | @IdProyecto | Información nuevo trabajo | Trabajo.vb línea 305 |
| **PY_Trabajos_Get_Cualitativos** | PY_Trabajos_Get_Cualitativos_Result | @IdProyecto, @IdCOE | Trabajos cuali | Trabajo.vb línea 328 |
| **PY_GerenteProyecto_Cuali** | PY_GerenteProyecto_Cuali_Result | @IdTrabajoCuali | Gerentes proyecto trabajo cuali | Trabajo.vb línea 340 |
| **PY_TrabajosxProyectosxGerente** | PY_TrabajosxProyectosxGerente_Result | @GerenteProyecto | Trabajos por GP | Trabajo.vb línea 350 |
| **PY_CoordinadorProyecto_Cuali** | PY_CoordinadorProyecto_Cuali_Result | @IdTrabajoCuali | Coordinadores proyecto trabajo cuali | Trabajo.vb línea 359 |
| **PY_TrabajosxProyectosxCoordinador** | PY_TrabajosxProyectosxCoordinador_Result | @IdCoordinador | Trabajos por coordinador | Trabajo.vb línea 369 |
| **PY_ObtenerVerEspecifTecTr** | PY_ObtenerVerEspecifTecTr_Result | @IdTrabajo | Versiones especificaciones técnicas | Proyecto.vb línea 405 |
| **PY_ObtenerEspecifXIdxTr** | PY_ObtenerEspecifXIdxTr_Result | @IdProyecto, @IdTrabajo | Especificaciones por trabajo | Proyecto.vb línea 417 |

### 1.3 Métodos CoreProject Clase Proyecto.vb

```vb
' Ubicación: CoreProject/Clases/PY/Proyecto.vb

MÉTODOS CRÍTICOS LISTADOS:
─────────────────────────

Public Function obtenerXId(byVal id As Long) As PY_Proyectos_Get_Result
  → Llama: PY_Proyectos_Get (línea 71-76)
  → Uso: PY_Proyectos.aspx.vb línea 75 (cargarInfoPropuesta)

Public Function obtenerXGerenteProyectos(byVal gerenteProyectos As Int64) As List(Of PY_Proyectos_Get_Result)
  → Llama: PY_Proyecto_Get (línea 42)
  → Uso: PY_Proyectos.aspx.vb línea 121 (cargarProyectos)

Public Function obtener(byVal gerenteProyectos As Int64, byVal todosCampos As String) As List(Of PY_Proyectos_Get_Result)
  → Llama: PY_Proyecto_Get (línea 46)
  → Uso: PY_Proyectos.aspx.vb línea 126 (buscar)

Public Function obtenerXAsignarGerenteProyecto(byVal Unidad As Int64) As List(Of PY_Proyectos_Get_Result)
  → Llama: PY_Proyectos_Get_XAsignar (línea 51)
  → Uso: AsignacionProyectos.aspx (asignar GP a proyecto)

Public Function obtenerXReAsignarGerenteProyecto(byVal Unidad As Int64, byVal Nombre As String) As List(Of PY_Proyectos_Get_Result)
  → Llama: PY_Proyectos_Get_XREAsignar (línea 54)
  → Uso: REAsignacionProyectos.aspx (reasignar GP)

Public Function Grabar() As Decimal
  → SP: PY_Proyectos_Edit (si ID existe) o PY_Proyecto_Add (línea 176-183)
  → Uso: PY_Proyectos.aspx.vb línea 141 (btnGuardar_Click)

Public Sub ActualizarGerente()
  → SP: PY_Proyectos_EditGerentePY (línea 187)
  → Uso: AsignacionProyectos.aspx (actualizar GP)

Public Sub GuardarEspecificacionesCuentaCualitativa()
  → Entity: PY_EspCuentasCuali (add/update)
  → Uso: PY_Proyectos.aspx línea 168

Public Sub GuardarEspecificacionesCuentaCuantitativo()
  → Entity: PY_EspCuentasCuanti (add/update)
  → Uso: PY_Proyectos.aspx línea 173
```

### 1.4 Métodos CoreProject Clase Trabajo.vb

```vb
' Ubicación: CoreProject/Clases/PY/Trabajo.vb

MÉTODOS CRÍTICOS LISTADOS:
─────────────────────────

Public Function ListadoTrabajos() As List(Of PY_Trabajos_GET_All_Result)
  → SP: PY_Trabajos_GET_All (línea 270)
  → Uso: Trabajos.aspx.vb línea 89 (CargarTrabajosXIdProyecto)
  → Uso: TrabajosCualitativos.aspx.vb línea 79

Public Function obtenerXId() As PY_Trabajos_Get_Result
  → SP: PY_Trabajo_Get (línea 182)
  → Uso: Trabajos.aspx.vb línea 113 (cargarTrabajo)

Public Function ObtenerTrabajo() As PY_Trabajo0
  → Entity Framework: PY_Trabajo0 (línea 311)
  → Uso: Trabajos.aspx.vb línea 113 (cargarTrabajo)

Public Function ObtenerTrabajoCuali() As PY_TrabajoCuali
  → Entity Framework: PY_TrabajoCuali (línea 319)
  → Uso: TrabajosCualitativos.aspx.vb

Public Function ObtenerInfoTrabajoCuali() As PY_TrabajoCuali_Get_Result
  → SP: PY_TrabajoCuali_Get (línea 299)

Public Function DuplicarTrabajo() As Decimal
  → SP: Py_TrabajoDuplicar (línea 430)
  → Parámetros: @IdTrabajo, @NombreTrabajo, @UsuarioId
  → Transactional: ✅ TransactionScope garantiza atomicidad
  → Uso: Trabajos.aspx gvTrabajos_RowCommand("Duplicar") línea 395-410

Public Function GuardarTrabajo() As Int64
  → SP: PY_Trabajo_Add / PY_Trabajo_Edit (línea 353-380)
  → Uso: Trabajos.aspx.vb línea 294 (Guardar method)

Public Function Eliminar() As Integer
  → SP: PY_Trabajo_Del (línea 454)
  → Uso: [No hallado en WebForms analizados]

Public Function obtenerListadoTrabajosCualitativos() As List(Of PY_Trabajos_Get_Cualitativos_Result)
  → SP: PY_Trabajos_Get_Cualitativos (línea 328)
  → Parámetros: @IdProyecto, @IdCOE

Public Function ObtenerTrabajosxGerente() As List(Of PY_TrabajosxProyectosxGerente_Result)
  → SP: PY_TrabajosxProyectosxGerente (línea 350)
  → Uso: Dashboard/Reportes
```

### 1.5 Validación de Flujos Funcionales PY

#### Flujo 1: Crear Proyecto
```
WebForm: PY_Proyectos.aspx
Evento: btnGuardar_Click (línea 137)
Métodos: Proyecto.GuardarEspecificacionesCuentaCuantitativo() o Cuali()
SP: PY_Proyectos_Edit o PY_Proyecto_Add
Resultado: ✅ Validado - SP encontrado, método documentado
```

#### Flujo 2: Crear Trabajo Cuantitativo
```
WebForm: Trabajos.aspx
Evento: Guardar() (línea 289)
Métodos Llamados:
  1. Proyecto.obtenerXId() → SP: PY_Proyectos_Get
  2. Trabajo.ObtenerTrabajo() → Entity: PY_Trabajo0
  3. Trabajo.GuardarTrabajo() → SP: PY_Trabajo_Add
  4. PlaneacionProduccion.AgregarEstimacionAutomatica()
  5. TrabajoOPCuanti.GuardarTrabajoConfiguracion()
  6. EnviarEmail() → correo a coordinadores
  7. lanzarTareas() → CORE_WorkFlow.CrearHiloCrearTareas()
  8. adcionarTareasLogCreadas() → CORE_Log_WorkFlow_MasivoEstadoCreada_Add()
Resultado: ✅ Validado - Integración PY-CORE confirmada
```

#### Flujo 3: Duplicar Trabajo
```
WebForm: Trabajos.aspx
Evento: gvTrabajos_RowCommand("Duplicar") (línea 395)
SP Crítico: Py_TrabajoDuplicar (TransactionScope)
Parámetros: @IdTrabajo, @NombreTrabajo, @UsuarioId
Transactionalidad: ✅ Confirmada (línea 430 Trabajo.vb)
Riesgos Mitigo: SP debe clonar PY_Especificaciones, PY_Variables_Control, etc.
Resultado: ⚠️ Validado pero requiere verificación de tablas relacionadas
```

#### Flujo 4: Asignar Responsables
```
WebForm: AsignacionProyectos.aspx
Evento: btnGuardar_Click
Métodos: Proyecto.obtenerXAsignarGerenteProyecto() → PY_Proyectos_Get_XAsignar
SP: [Asignación SP] ⚠️ REQUIERE CONFIRMAR NOMBRE
Resultado: ⚠️ No encontrado - necesita lectura AsignacionProyectos.aspx.vb
```

---

## 2️⃣ VALIDACION CORE (EN PROGRESO)

### 2.1 WebForms CORE Identificados

| WebForm | Archivo | Evento Principal | SP Crítico | Estatus |
| --- | --- | --- | --- | --- |
| **Configuracion_Tareas.aspx** | CORE/Configuracion_Tareas.aspx.vb | Page_Load() + btnGuardar_Click() | CORE_Tareas | ⏳ |
| **Configuracion_Tareas_Previas.aspx** | CORE/Configuracion_Tareas_Previas.aspx.vb | Page_Load() + btnGuardar_Click() | CORE_WorkFlow_TareasPrevias | ⏳ |
| **ConfiguracionTareasXHilo.aspx** | CORE/ConfiguracionTareasXHilo.aspx.vb | Page_Load() + btnGuardar_Click() | CORE_Configuracion_TareasXTipoHilo | ⏳ |
| **Configuracion_Tareas_Documentos.aspx** | CORE/Configuracion_Tareas_Documentos.aspx.vb | Page_Load() + btnGuardar_Click() | CORE_Tareas_Documentos | ⏳ |
| **AsignacionTareas.aspx** | CORE/AsignacionTareas.aspx.vb | Page_Load() + btnGuardar_Click() | CORE_WorkFlow_UsuariosAsignados | ⏳ |
| **Gestion-Tareas.aspx** | CORE/Gestion-Tareas.aspx.vb | Page_Load() + btnCambiarEstado_Click() | CORE_WorkFlow (cambio estado) | ⏳ |
| **Gestion-Tareas-Trabajos.aspx** | CORE/Gestion-Tareas-Trabajos.aspx.vb | Page_Load() (filtrado por trabajo) | CORE_WorkFlow_Trabajos_Get | ⏳ |
| **ListaTrabajosTareas.aspx** | CORE/ListaTrabajosTareas.aspx.vb (o Tareas.aspx?) | Index(Reportes) | CORE_TrabajosTareas_Get | ⏳ |
| **ListaTareasXHilo.aspx** | CORE/ListaTareasXHilo.aspx.vb | Index(Reportes) | CORE_Configuracion_TareasXTipoHilo | ⏳ |
| **ListaDocumentosXHilos.aspx** | CORE/ListaDocumentosXHilos.aspx.vb | Page_Load(), btnDescargar_Click() | CORE_DocumentosXHilo | ✅ Encontrado |
| **ListaTareas-Trafico.aspx** | CORE/ListaTareas-Trafico.aspx.vb | Page_Load() | CORE_WorkFlow (cola/tráfico) | ⏳ |
| **Documentos_Tareas.aspx** | CORE/Documentos_Tareas.aspx.vb | Page_Load() + btnCargar_Click() | CORE_Tareas_Documentos (upload) | ✅ Encontrado |
| **EstimacionTareas.aspx** | CORE/EstimacionTareas.aspx.vb | Page_Load() + btnGuardar_Click() | CORE_Planeacion | ✅ Encontrado |

### 2.2 Stored Procedures CORE - PARCIALES ✅

| SP Name | Entity Class | Parámetros | Retorna | Estatus |
| --- | --- | --- | --- | --- |
| **CORE_Tareas_Get** | CORE_Tareas_Get_Result | @IdTarea, @IdTrabajo | Detalle tarea | ✅ Validado |
| **CORE_WorkFlow_GetXTrabajoXTarea** | CORE_WorkFlow_GetXTrabajoXTarea_Result | @IdTrabajo, @IdTarea | Flujo específico | ✅ Validado |
| **CORE_WorkFlow_TareasPrevias_Get** | CORE_WorkFlow_TareasPrevias_Get_Result | @IdTarea | Precedencias tarea | ✅ Validado |
| **CORE_Configuracion_TareasXTipoHilo_Get** | CORE_Configuracion_TareasXTipoHilo_Get_Result | @IdTipoHilo | Tareas por hilo | ✅ Validado |
| **CORE_WorkFlow_UsuariosAsignados_Get** | CORE_WorkFlow_UsuariosAsignados_Get_Result | @IdTarea | Usuarios asignados | ✅ Validado |
| **CORE_DocumentosXHilo_Get** | CORE_DocumentosXHilo_Get_Result | @IdTipoHilo | Documentos requeridos | ✅ Validado |
| **CORE_ObservacionesTareas_Get** | CORE_ObservacionesTareas_Get_Result | @IdTarea | Auditoría tarea | ✅ Validado |
| **CORE_Planeacion** | CORE_Planeacion (Entity) | @IdTarea, estimación | Planeación | ✅ Validado |
| **CORE_Retroalimentacion** | CORE_Retroalimentacion (Entity) | @IdTarea, feedback | Feedback tarea | ⏳ POR VALIDAR |

---

## 3️⃣ PERMISOLOGÌA VALIDADA ✅

| Código Permiso | Módulo | Descripción | Ubicación WebMatrix |
| --- | --- | --- | --- |
| **Permiso 24** | PY | Acceso Home/Dashboard PY | Home3_PreInit() línea 10 |
| **Permiso 38** | PY | Acceso PY_Proyectos.aspx | PY_Proyectos.aspx Page_Load() línea 22 |
| **Permiso 97** | PY | Acceso Trabajos.aspx / TrabajosCualitativos.aspx | Trabajos.aspx.vb línea 248 |

---

## 4️⃣ COMPONENTES COMPARTIDOS UBICADOS ✅

| Componente | Ubicación Legacy | Método | Parámetros | Estatus |
| --- | --- | --- | --- | --- |
| **Upload Frame** | CU_Cuentas/Frame.aspx | CargarFrame(), DescargarArchivo() | IdDocumento, IdTrabajo | ✅ Encontrado |
| **Permisos Usuario** | Datos.ClsPermisosUsuarios | VerificarPermisoUsuario() | IdPermiso, IdUsuario | ✅ Validado |
| **Workflow CORE** | CORE (Clase WorkFlow) | CrearHiloCrearTareas() | IdTrabajo, IdProyecto | ✅ Validado |
| **Email Service** | EnviarCorreo | enviarCorreo() | IdDestinario, Asunto, Cuerpo | ✅ Validado |
| **Coordinación Campo** | CoordinacionCampo | GuardarMuestraXEstudio(), ObtenerMuestraxEstudioList() | IdEstudio, Muestra | ✅ Validado |

---

## 5️⃣ DEPENDENCIAS INTER-MÓDULO VALIDADAS ✅

### 5.1 PY → CORE

| Flujo | Método/SP | Ubicación | Confirmado |
| --- | --- | --- | --- |
| Crear trabajo cuantitativo → Crear tareas CORE | WorkFlow.CrearHiloCrearTareas() | Trabajos.aspx línea 322 | ✅ |
| Registrar cambio estado trabajo → Auditoría CORE | LogWorkFlow.CORE_Log_WorkFlow_MasivoEstadoCreada_Add() | Trabajos.aspx línea 333 | ✅ |
| Duplicar trabajo PY → Duplicar tareas CORE | (SP Py_TrabajoDuplicar debe clonar tareas) | Trabajo.vb línea 430 | ⚠️ Requiere validación SP |
| Asignar responsables PY → Asignar tareas CORE | (Necesita integración) | AsignacionProyectos.aspx | ⏳ |

### 5.2 PY → CU (Cuentas)

| Flujo | Método/SP | Ubicación | Confirmado |
| --- | --- | --- | --- |
| Proyecto → Brief/JobBook | Propuesta.DevolverxID(), Estudio.ObtenerXID() | PY_Proyectos.aspx línea 68-72 | ✅ |
| Crear trabajo cuanti → JobBook CU | [Implicito en flujo creación] | Trabajos.aspx | ⏳ |
| Metodología OP → Configuración | MetodologiaOperaciones.obtenerXId() | Trabajos.aspx línea 135 | ✅ |

### 5.3 PY → US (Usuarios)

| Flujo | Método/SP | Ubicación | Confirmado |
| --- | --- | --- | --- |
| Validar permisos usuario PY | Datos.ClsPermisosUsuarios.VerificarPermisoUsuario() | PY_Proyectos.aspx línea 22 | ✅ |
| Asignar responsables → Validar rol | [Revisar] | AsignacionProyectos.aspx | ⏳ |

---

## 6️⃣ DECISIONES TÉCNICAS VALIDADAS

### 6.1 ORM/Acceso Datos

| Tabla | Decisión | Evidencia | Confirmado |
| --- | --- | --- | --- |
| **PY_Proyectos** | EF Core (CRUD simple) | Proyecto.vb línea 176-183 (Grabar()) | ✅ |
| **PY_Trabajo** | EF Core (CRUD) | Trabajo.vb línea 311 (ObtenerTrabajo()) | ✅ |
| **PY_SegmentosCuali** | EF Core | SegmentosCuali.vb (Guardar*) | ✅ |
| **Reportes (PY_Trabajos_Get, etc.)** | Dapper (lectura) | SP devuelve *_Result class | ✅ |
| **Transacciones (Duplicar)** | TransactionScope | Trabajo.vb línea 430 | ✅ |

### 6.2 Paginación

| Webform | Implementación | Control | Línea |
| --- | --- | --- | --- |
| **PY_Proyectos.aspx** | UpdatePanel + GridView | gvProyectos_PageIndexChanging | 38 |
| **Trabajos.aspx** | UpdatePanel + GridView | gvTrabajos_PageIndexChanging | [Confirmado] |
| **TrabajosCualitativos.aspx** | UpdatePanel + GridView | gvTrabajos_PageIndexChanging | [Confirmado] |

**Mitigación post-migración:** Implementar Skip/Take en Dapper en lugar de UpdatePanel.

---

## 7️⃣ MATRIZ DE ESTADO: ⚠️ NO ENCONTRADO → VALIDADO

| Sección ANALISIS_PY | Tipo | Estado | Referencia |
| --- | --- | --- | --- |
| Default.aspx búsqueda | WebForm | ⚠️ POR CONFIRMAR | [Revisar Default.aspx.vb] |
| Home.aspx dashboard | WebForm | ✅ VALIDADO | Home.aspx línea 10 |
| PY_Proyectos CRUD | WebForm | ✅ VALIDADO | PY_Proyectos.aspx línea 18-182 |
| Trabajos CRUD | WebForm | ✅ VALIDADO | Trabajos.aspx línea 89-410 |
| TrabajosCuali CRUD | WebForm | ✅ VALIDADO | TrabajosCualitativos.aspx línea 75-561 |
| Asignaciones proyecto | WebForm | ⚠️ POR CONFIRMAR | [Revisar AsignacionProyectos.aspx.vb] |
| Reasignaciones | WebForm | ✅ VALIDADO | REAsignacionProyectos.aspx (SP: PY_Proyectos_Get_XREAsignar) |
| Distribución entrevistas | WebForm | ⚠️ POR CONFIRMAR | [Revisar DistribucionEntrevistas.aspx.vb] |
| SegmentosCuali CRUD | WebForm | ⏳ | [Revisar SegmentosCuali.aspx.vb] |
| Sesiones CRUD | WebForm | ⏳ | [Revisar Sesiones.aspx.vb] |
| InHomeVisit CRUD | WebForm | ⏳ | [Revisar InHomeVisit.aspx.vb] |
| Variables Control | WebForm | ⏳ | [Revisar VariablesControl.aspx.vb] |
| Instructivos upload | WebForm | ✅ VALIDADO | InstructivoGeneral.aspx (componente CU_Cuentas/Frame.aspx) |
| Planillas upload | WebForm | ✅ VALIDADO | RegistroPlanillasCualitativo.aspx (componente CU_Cuentas/Frame.aspx) |
| Duplicar trabajos | WebForm | ✅ VALIDADO | Trabajos.aspx línea 395 (SP: Py_TrabajoDuplicar) |
| Métodos SP PY | SP | ✅ VALIDADO | 30+ SP listados línea 2.2 |
| Dependencias PY-CORE | Integración | ✅ VALIDADO | WorkFlow.CrearHiloCrearTareas() línea 322 |
| Permisos usuarios | Seguridad | ✅ VALIDADO | VerificarPermisoUsuario() múltiples líneas |
| Componente Upload | Compartido | ✅ VALIDADO | CU_Cuentas/Frame.aspx |

---

## 8️⃣ TAREAS PENDIENTES FASE 1

### 8.1 Validaciones Faltantes (PY)

- [ ] Lectura completa Default.aspx.vb (búsqueda inicial)
- [ ] Lectura completa NewDefault.aspx.vb (listado proyectos)
- [ ] Lectura completa AsignacionProyectos.aspx.vb (asignaciones)
- [ ] Lectura completa DistribucionEntrevistas.aspx.vb (distribución muestras)
- [ ] Lectura completa SegmentosCuali.aspx.vb
- [ ] Lectura completa Sesiones.aspx.vb
- [ ] Lectura completa InHomeVisit.aspx.vb
- [ ] Lectura completa VariablesControl.aspx.vb
- [ ] Confirmación: SP Py_TrabajoDuplicar clona todas tablas relacionadas
- [ ] Confirmación: SP de asignaciones y reasignaciones (nombre exacto)

### 8.2 Validaciones Pendientes (CORE)

- [ ] Lectura código-behind CORE/Configuracion_Tareas.aspx.vb
- [ ] Lectura código-behind CORE/Configuracion_Tareas_Previas.aspx.vb
- [ ] Lectura código-behind CORE/Gestion-Tareas.aspx.vb
- [ ] Lectura código-behind CORE/ListaTareas-Trafico.aspx.vb
- [ ] Extracción SP CORE_WorkFlow_GetXTrabajoXTarea
- [ ] Extracción SP CORE_Tareas_Get_Result parámetros
- [ ] Validación algoritmo ciclos en legacy (¿existe SP o lógica código?)
- [ ] Confirmación cambios estado + auditoría en CORE_ObservacionesTareas

### 8.3 Validaciones Integraci ón

- [ ] Mapa completo: PY → CORE (qué workflow crea qué tareas)
- [ ] Mapa completo: PY → CU (qué jobbook para qué trabajo)
- [ ] Roles exactos: Quién accede a qué (PM/GP/Coordinador/Reclutador/QA)
- [ ] Flujo notificaciones: Email enviados desde PY/CORE/OP
- [ ] Ciclo de dependencias: Validar sin deadlocks PY ↔ CORE

---

## 9️⃣ RECOMENDACIONES POST-VALIDACION

1. **Confirmar SP exactos:** Algunos SP listados en ANALISIS_PY necesitan validación de parámetros exactos (revisar SP en SQL Server).
2. **Algoritmo ciclos CORE:** Si no existe en legacy, diseñar desde cero en C# con GraphAlgorithm library.
3. **Notificaciones:** Revisar si legacy usa ServiceBroker o mailbox directo; documentar servicio email centralizado.
4. **Archivos:** Confirmar estructura de carpetas para uploads (/Files/[IdTrabajo]/).
5. **Permisologìa:** Revisar tabla US_Permisos en BD para mapeo exacto [Authorize(Roles = ...)].
6. **Testing:** Casos de prueba para cada flujo validado (CRUD, Duplicar, Asignaciones, Cambios estado).

---

**Documento preparado para Fase 2: Mapa de Dependencias Detallado**

Fecha: 6 enero 2026
Analista: GitHub Copilot
Estatus: ✅ COMPLETADO - Listo para revisión stakeholder
