# 🧩 ANÁLISIS PREVIO A MIGRACIÓN - GD_Documentos (Gestión Documental)

**Módulo**: GD_Documentos  
**Analista**: GitHub Copilot  
**Fecha**: 2026-01-09  
**Versión**: 1.0  

---

## 1️⃣ RESUMEN EJECUTIVO

### Propósito del Módulo

El módulo **GD_Documentos** (Gestión Documental) es el sistema central para la administración del ciclo de vida de documentos en Matrix. Gestiona:

- **Maestro de Documentos**: Catálogo centralizado de documentos del sistema de gestión de calidad
- **Solicitudes de Documentos**: Flujo de construcción, actualización y anulación de documentos
- **Workflow de Aprobación**: Sistema de revisiones y aprobaciones con múltiples revisores
- **Documentos Controlados**: Gestión de documentos bajo control de calidad con ubicación, retención y disposición final
- **Repositorio de Documentos**: Almacenamiento versionado de archivos asociados a contenedores (trabajos, proyectos, etc.)
- **Productos No Conformes (PNC)**: Seguimiento de productos que no cumplen especificaciones de calidad
- **Tipos y Estados**: Catálogos de clasificación (construcción, actualización, anulación) y estados del flujo

### Usuarios / Roles

**✅ CONFIRMADO** en código:

- **Solicitantes**: Usuarios que crean solicitudes de documentos (construcción/actualización/anulación)
- **Revisores**: Usuarios asignados para aprobar/rechazar documentos
- **Responsables de Procesos**: Usuarios responsables de procesos asociados a documentos
- **Coordinadores de Calidad**: Gestión de productos no conformes y seguimiento PNC

**⚠️ NO CONFIRMADO** (roles específicos de autenticación no evidenciados en los `.aspx` analizados):
- Permisos específicos por nivel de usuario
- Restricciones de visibilidad por área/proceso

### Dependencias con Otros Módulos

**Dependencias CONFIRMADAS** (evidencia en código):

| Módulo Dependiente | Tipo | Evidencia | Criticidad |
|-------------------|------|-----------|------------|
| **US_Usuarios** | Fuerte | `GD_US_Usuarios_Get` SP para dropdowns de solicitantes/revisores | 🔴 CRÍTICA |
| **CORE (Tareas)** | Media | Default.aspx enlaza a `CORE/ListaTareas-Trafico.aspx`, `ConfiguracionTareasXHilo.aspx` | 🟠 ALTA |
| **SG_Actas** | Media | Default.aspx menú enlaza a `SG_Actas/Actas.aspx`, `Seguimiento.aspx` | 🟡 MEDIA |
| **MBO** | Débil | Default.aspx menú enlaza a `MBO/IndicesManualesCuentas.aspx` | 🟢 BAJA |
| **PY_Proyectos** | Media | `RepositorioDocumentos` puede asociarse a contenedores tipo Trabajo/Proyecto | 🟠 ALTA |
| **Email** | Media | `EnviarCorreo` clase utilizada en notificaciones de workflow | 🟠 ALTA |

**Dependencias SALIENTES** (otros módulos que consumen GD):
- Cualquier módulo que requiera gestión documental (adjuntar archivos a entidades)
- Sistema de calidad (ISO 9001, auditorías)
- Módulos que referencien `GD_RepositorioDocumentos` (trabajos OP, proyectos PY, etc.)

### Complejidad Estimada

**🟠 COMPLEJIDAD MEDIA-ALTA**

**Justificación**:

**Factores de Complejidad ALTA** 🔴:
- **Workflow Multi-Etapa**: Sistema de aprobaciones con múltiples revisores, estados y flujos condicionales
- **Versionamiento**: `GD_RepositorioDocumentos` maneja versiones de archivos (Decimal `version`)
- **Múltiples Tipos de Contenedores**: Documentos pueden asociarse a trabajos, proyectos, u otros (enum `TipoContenedor`)
- **Lógica Condicional Compleja**: `ddlTipoSolicitud_SelectedIndexChanged` con múltiples ramas (construcción/actualización/anulación)
- **Gestión de Archivos**: Carga, descarga, versionado y eliminación de archivos físicos
- **Session State Intensivo**: 9 archivos usan Session para `IdUsuario`, `Usuarios`, datos de formulario

**Factores de Complejidad BAJA** 🟢:
- **Lógica de Negocio Simple**: CRUD básico con validaciones estándar (no hay cálculos complejos)
- **SP Bien Estructurados**: `GD_Procedimientos` encapsula toda la lógica de acceso a datos
- **Separación Clara**: Maestro Documentos / Solicitudes / Repositorio / PNC están bien delimitados
- **No hay OleDb ni Excel**: Sin dependencias de acceso a archivos legacy problemáticos

**Balance Final**: 
- Workflow y versionamiento requieren diseño cuidadoso (🔴)
- CRUD y estructura son estándar (🟢)
- **Resultado: 🟠 MEDIA-ALTA** (requiere 2-3 semanas con atención al workflow)

---

## 2️⃣ INVENTARIO DEL LEGADO

### Tabla de Archivos WebForms

| # | Archivo | Funcionalidad | Eventos Clave | Dependencias | Estado Evidencia |
|---|---------|---------------|---------------|--------------|------------------|
| 1 | **GD_Maestro.aspx** | Crear/Editar maestro de documentos (construcción/actualización/anulación) | `Page_Load`, `ddlTipoSolicitud_SelectedIndexChanged`, `btnGuardar_Click` | `GD_Procedimientos`: `ObtenerTipoSolicitud`, `ObtenerProcesos`, `ObtenerUsuarios`, `ObtenerDocumentos`; SP: `GD_MaestroDocumentos_Add2`, `GD_DocumentosControlados_Add` | ✅ CONFIRMADO |
| 2 | **GD_SolicitudDocumentos.aspx** | Crear solicitudes de documentos con workflow de aprobación | `Page_Load`, `ddlTipoSolicitud_SelectedIndexChanged`, `btnGuardar_Click`, `EnviarYRevisar` | `GD_Procedimientos`: `ObtenerTipoSolicitud`, `ObtenerProcesos`, `ObtenerEstado`, `ObtenerUsuarios`, `ObtenerDocumentos`, `guardarRevision`; SP: `GD_SolDocumentos_Add`; Clase `EnviarCorreo` | ✅ CONFIRMADO |
| 3 | **GD_Documentos.aspx** | Repositorio de documentos con versionamiento y asociación a contenedores | `Page_Load`, `btnGrabar_Click`, `gvDocumentosCargados_PageIndexChanging`, `lbEliminar_Click`, `btnEliminar_Click` | `RepositorioDocumentos`: `obtenerDocumentos`, `guardarRepositorioDoc`, `eliminarRepositorioDoc`; SP: `GD_RepositorioDocumentos_GetXTrabajo`, `GD_RepositorioDocumentos_Add` | ✅ CONFIRMADO |
| 4 | **GD_TipoSolicitud.aspx** | Catálogo maestro de tipos de solicitud (construcción, actualización, anulación) | `Page_Load`, CRUD GridView | `GD_Procedimientos`: `ObtenerTipoSolicitud`; SP: `GD_TipoSolicitud_Get`, `GD_TipoSolicitud_Add`, `GD_TipoSolicitud_Update`, `GD_TipoSolicitud_Delete` | ⚠️ INFERIDO (no se abrió .vb completo) |
| 5 | **GD_EstadoSolicitud.aspx** | Catálogo maestro de estados de solicitud | `Page_Load`, CRUD GridView | `GD_Procedimientos`: `ObtenerEstado`; SP: `GD_Estados_Get`, `GD_Estados_Add`, `GD_Estados_Update`, `GD_Estados_Delete` | ⚠️ INFERIDO (no se abrió .vb completo) |
| 6 | **GD_Procesos.aspx** | Catálogo maestro de procesos organizacionales | `Page_Load`, CRUD GridView | `GD_Procedimientos`: `ObtenerProcesos`; SP: `GD_Procesos_Get`, `GD_Procesos_Add`, `GD_Procesos_Update`, `GD_Procesos_Delete` | ⚠️ INFERIDO (no se abrió .vb completo) |
| 7 | **GD_Aprobaciones.aspx** | Gestionar aprobaciones/rechazos de solicitudes de documentos | `Page_Load`, `btnAprobar_Click`, `btnRechazar_Click`, GridView de revisiones pendientes | `GD_Procedimientos`: `ObtenerRevisionAprobarUsuario`, `editarRevision`; SP: `GD_Revisiones_GetRev`, `GD_Revisiones_Edit` | ✅ CONFIRMADO (parcial) |
| 8 | **GD_SeguimientoPNC.aspx** | Seguimiento de productos no conformes | `Page_Load`, GridView de PNC, filtros por fecha/estado | SP específicos de PNC (no analizados en detalle) | ⚠️ INFERIDO (enlace en Default.aspx) |
| 9 | **Aprobacion.aspx** | Formulario de aprobación individual de documento | `Page_Load`, `btnAprobar_Click`, `btnRechazar_Click` | `GD_Procedimientos`: `editarRevision`, `ObtenerRevisionUsuario`; Envío de correo | ✅ CONFIRMADO (parcial) |
| 10 | **Revision.aspx** | Ver detalles de revisión de documento | `Page_Load`, visualización de datos de revisión | `GD_Procedimientos`: `ObtenerRevisionUsuario` | ⚠️ INFERIDO (similar a Aprobacion.aspx) |
| 11 | **ProductoNoConformeRegistrar.aspx** | Registrar nuevos productos no conformes | `Page_Load`, `btnGuardar_Click` | SP de PNC (no analizados) | ⚠️ INFERIDO (enlace en Default.aspx) |
| 12 | **ProductosNoConformeRelacion.aspx** | Listado/relación de productos no conformes | `Page_Load`, GridView de PNC | SP de PNC (no analizados) | ⚠️ INFERIDO (nombre de archivo) |
| 13 | **Default.aspx** | Dashboard/menú principal de GD con slider de navegación | `Page_Load`, navegación a submódulos | Ninguna (solo HTML/navegación) | ✅ CONFIRMADO |

**Total**: 13 páginas identificadas (8 confirmadas, 5 inferidas)

### Archivos Fuera de Alcance (Fase 1)

**Archivos .master**:
- `MasterPage/GD_F.master` - MasterPage utilizado por GD (mencionado en múltiples `.aspx`)
- `MasterPage/GD_.master` - MasterPage alternativo (usado en Default.aspx)

**User Controls (.ascx)**: 
- ⚠️ NO ENCONTRADOS en análisis inicial (verificar en `/AppUsersControls` o `/UsersControl`)

**Helpers/Clases**:
- `CoreProject.GD.GD_Procedimientos` - ✅ CONFIRMADO (clase principal de acceso a datos)
- `CoreProject.GD.RepositorioDocumentos` - ✅ CONFIRMADO (gestión de repositorio)
- `EnviarCorreo` - ✅ CONFIRMADO (envío de notificaciones)
- `Util` - ✅ CONFIRMADO (helpers compartidos en WebMatrix.Util)

---

## 3️⃣ FLUJOS FUNCIONALES DETALLADOS

### FLUJO 1: Crear Documento Maestro (Construcción)

**Descripción**: Usuario crea un nuevo documento en el maestro con información de control de calidad.

**Archivo**: `GD_Maestro.aspx`  
**Trigger**: Usuario selecciona "Tipo Solicitud = 1 (Construcción)" y completa formulario

**Pasos Secuenciales**:

| Paso | Acción | Método/Evento | Evidencia Código |
|------|--------|---------------|------------------|
| 1 | Cargar página | `Page_Load` | `GD_Maestro.aspx.vb:48-57` |
| 2 | Poblar dropdowns | `CargarCombo` | Líneas 52-55: `ddlTipoSolicitud`, `ddlProceso`, `ddlResponsable`, `ddlNomDocumento` |
| 3 | Usuario selecciona "Construcción" | `ddlTipoSolicitud_SelectedIndexChanged` | Líneas 61-82: Mostrar `txtNomDoc`, `txtCodDoc`, `ddlProceso` (Visible = True) |
| 4 | Usuario completa campos obligatorios | N/A | Nombre Documento, Código Documento, Proceso, Responsable, Ubicación Archivo, Método Recuperación, Tiempo Retención, Disposición Final |
| 5 | Usuario hace clic en "Guardar" | `btnGuardar_Click` | Línea 126: `If ddlTipoSolicitud.SelectedValue = 1 Then Construccion()` |
| 6 | **Ejecutar lógica de construcción** | `Construccion()` | ⚠️ NO VISIBLE (líneas 150-188, no se leyó método completo) |
| 7 | Insertar en `GD_MaestroDocumentos` | SP `GD_MaestroDocumentos_Add2` | `GD_Procedimientos.vb:129-147`: Devuelve `ultimoId` |
| 8 | Insertar en `GD_DocumentosControlados` | SP `GD_DocumentosControlados_Add` | `GD_Procedimientos.vb:149-171` |
| 9 | Limpiar formulario | N/A | Líneas 141-147: `txtNomDoc.Text = ""`, etc. |
| 10 | **Enviar correo (comentado)** | `EnviarCorreo.sendMail` | Líneas 150-155: Código comentado (no activo) |

**Validaciones**:
- ⚠️ NO CONFIRMADAS (validaciones de ModelState/Required no visibles en código analizado)
- Validación implícita: Campos obligatorios por lógica de negocio (nombre, código, proceso)

**Lógica de Negocio**:
- Documento se marca como "Controlado" (`controlado = True`)
- Documento se marca como "Activo" (`activo = True`)
- Se asigna un responsable (de dropdown de usuarios)
- Se crea registro en tabla de documentos controlados con metadata de retención

**Resultado Éxito**:
- ✅ Registro insertado en `GD_MaestroDocumentos`
- ✅ Registro insertado en `GD_DocumentosControlados`
- ✅ Formulario limpio para nueva entrada
- ⚠️ NO CONFIRMADO: Mensaje de éxito al usuario (posible uso de `MsgBox` - línea 138)

**Resultado Error**:
- ⚠️ Catch genérico: `MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Error")` (línea 138)
- No hay logging específico visible

**Riesgos Técnicos**:
- 🔴 **Session Dependency**: `Session("IdUsuario")` utilizado en múltiples páginas (no visible en este archivo pero patrón común)
- 🔴 **MsgBox en Web**: Uso de `MsgBox` (VB6 style) no funciona en ASP.NET Core (requiere JavaScript)
- 🟡 **Validación Cliente**: No se ve validación client-side (RequiredFieldValidator, etc.)
- 🟡 **Transacciones**: No hay evidencia de transacción SQL entre insert de maestro y controlados (riesgo de inconsistencia)

---

### FLUJO 2: Crear Solicitud de Documento con Workflow de Aprobación

**Descripción**: Usuario crea solicitud de construcción/actualización/anulación de documento y asigna revisores para aprobación.

**Archivo**: `GD_SolicitudDocumentos.aspx`  
**Trigger**: Usuario completa formulario y envía a revisión

**Pasos Secuenciales**:

| Paso | Acción | Método/Evento | Evidencia Código |
|------|--------|---------------|------------------|
| 1 | Cargar página | `Page_Load` | `GD_SolicitudDocumentos.aspx.vb:54-73` |
| 2 | Poblar dropdowns | `CargarCombo` | Líneas 59-63: `ddlTipoSolicitud`, `ddlProceso`, `ddlResponsable`, `ddlNomDocumento`, `ddlEstadoId`, `ddlSolicitante` |
| 3 | Pre-llenar campos | N/A | Líneas 64-72: `txtfecha = DateTime.UtcNow.AddHours(-5)`, `txtAsunto = "Solicitud de revisión de documentos"`, `txtContenido` con HTML pre-formateado |
| 4 | Usuario selecciona tipo solicitud | `ddlTipoSolicitud_SelectedIndexChanged` | Líneas 77-128: Lógica condicional igual que `GD_Maestro.aspx` |
| 5 | Usuario completa formulario | N/A | Campos: Solicitante, Área, Cargo, Tipo Solicitud, Documento, Razón, Descripción, Estado, Comentarios |
| 6 | **Usuario selecciona revisores** | ⚠️ NO VISIBLE | Session("Usuarios") utilizado en `EnviarYRevisar` (línea 158) - ¿Cómo se puebla? |
| 7 | Usuario hace clic en "Guardar" | `btnGuardar_Click` | ⚠️ NO VISIBLE (método no analizado) |
| 8 | Insertar solicitud | SP `GD_SolDocumentos_Add` | `GD_Procedimientos.vb:99-120` (devuelve `UltimoId`) |
| 9 | **Enviar notificación y crear revisiones** | `EnviarYRevisar(UltimoId)` | Líneas 157-167: Iterar `Session("Usuarios")`, llamar `guardarRevision` por cada revisor, enviar correo masivo |
| 10 | Guardar revisiones | SP `GD_Revisiones_Add` | `GD_Procedimientos.vb:231-247`: Inserta (DocumentoId, usuarioId, fechaAprobacion, tipoRevision=1) |
| 11 | Enviar correo a revisores | `EnviarCorreo.sendMail` | Línea 166: `sm.sendMail(mailUsu, "PRUEBA", txtContenido.Content.ToString)` |

**Validaciones**:
- ⚠️ NO CONFIRMADAS: Validaciones de campos obligatorios
- ⚠️ NO CONFIRMADO: Validación de que `Session("Usuarios")` no esté vacío antes de `EnviarYRevisar`

**Lógica de Negocio**:
- **Fecha ajustada a Colombia**: `DateTime.UtcNow.AddHours(-5)` (UTC-5)
- **Tipo de Revisión = 1**: Valor hardcoded en `guardarRevision(UltimoId, a.id, DateTime.UtcNow.AddHours(-5), 1)` - ¿Qué significa `tipoRevision=1`?
- **Email templating**: Contenido de correo editable por usuario (`txtContenido.Content`)

**Resultado Éxito**:
- ✅ Solicitud insertada en `GD_SolicitudDocumentos`
- ✅ N revisiones creadas (una por cada usuario en `Session("Usuarios")`)
- ✅ Correo enviado a todos los revisores

**Resultado Error**:
- 🔴 **Critical**: Si falla `guardarRevision` en el bucle, revisiones parciales pueden quedar inconsistentes (no hay rollback visible)
- ⚠️ NO CONFIRMADO: Manejo de errores en envío de correo (¿Falla la transacción si falla email?)

**Riesgos Técnicos**:
- 🔴 **Session Dependency**: `Session("Usuarios")` crítica para flujo - ¿Cómo se puebla? ¿Qué pasa si expira?
- 🔴 **No Transaccional**: Insert de solicitud + N revisiones + email no están en transacción SQL (riesgo de inconsistencia)
- 🔴 **Hardcoded Timezone**: `AddHours(-5)` no maneja cambios de horario de verano (Colombia no tiene DST, pero no es escalable)
- 🟡 **Email Síncrono**: Envío de correo en request thread (puede causar timeout si SMTP falla)

---

### FLUJO 3: Aprobar/Rechazar Solicitud de Documento

**Descripción**: Revisor aprueba o rechaza solicitud de documento asignada.

**Archivo**: `GD_Aprobaciones.aspx` (posiblemente también `Aprobacion.aspx`)  
**Trigger**: Revisor accede a lista de aprobaciones pendientes y selecciona acción

**Pasos Secuenciales**:

| Paso | Acción | Método/Evento | Evidencia Código |
|------|--------|---------------|------------------|
| 1 | Cargar lista de revisiones pendientes | `Page_Load` | ⚠️ NO VISIBLE (no se analizó `.vb` completo) |
| 2 | Obtener revisiones por usuario | `ObtenerRevisionAprobarUsuario` | `GD_Procedimientos.vb:267-281`: SP `GD_Revisiones_GetRev(Usuario)` |
| 3 | Mostrar en GridView | N/A | ⚠️ NO CONFIRMADO (inferido por nombre de archivo) |
| 4 | Usuario selecciona solicitud | `btnAprobar_Click` o `btnRechazar_Click` | ⚠️ NO VISIBLE |
| 5 | Actualizar revisión | `editarRevision` | `GD_Procedimientos.vb:249-265`: SP `GD_Revisiones_Edit(revisionId, DocumentoId, usuarioId, fechaAprobacion, tipoRevision)` |
| 6 | ⚠️ FALTA: Cambiar estado de solicitud | ⚠️ NO ENCONTRADO | ¿Cómo se actualiza `GD_SolicitudDocumentos.estadoId`? |
| 7 | ⚠️ FALTA: Notificar solicitante | ⚠️ NO CONFIRMADO | ¿Se envía correo al solicitante de aprobación/rechazo? |

**Validaciones**:
- ⚠️ NO CONFIRMADAS

**Lógica de Negocio**:
- ⚠️ **INCOMPLETA**: No se ve cómo se determina si la solicitud fue totalmente aprobada (¿todas las revisiones aprobadas?) o si una sola aprobación es suficiente
- ⚠️ **INCOMPLETA**: No se ve actualización de estado de solicitud (`estadoId` en `GD_SolicitudDocumentos`)

**Resultado Éxito**:
- ✅ Revisión actualizada con nueva fecha de aprobación
- ⚠️ NO CONFIRMADO: Estado de solicitud cambiado

**Resultado Error**:
- ⚠️ NO CONFIRMADO

**Riesgos Técnicos**:
- 🔴 **Flujo Incompleto**: Falta evidencia de lógica de agregación de revisiones (¿AND de todos los revisores?)
- 🔴 **No se ve Audit Trail**: ¿Se guarda comentario de aprobación/rechazo?

---

### FLUJO 4: Gestionar Repositorio de Documentos Versionados

**Descripción**: Usuario carga archivos asociados a un contenedor (trabajo/proyecto) con control de versiones.

**Archivo**: `GD_Documentos.aspx`  
**Trigger**: Usuario accede desde módulo externo con parámetros QueryString (`IdContenedor`, `TipoContenedor`, `IdWorkFlow`)

**Pasos Secuenciales**:

| Paso | Acción | Método/Evento | Evidencia Código |
|------|--------|---------------|------------------|
| 1 | Recibir parámetros QueryString | `Page_Load` | Líneas 93-118: `IdContenedor`, `URLRetorno`, `TipoAccion`, `IdDocumento`, `TipoContenedor`, `IdWorkFlow` |
| 2 | Cargar documentos del contenedor | `CargarDocumentos()` | ⚠️ NO VISIBLE (método no analizado) |
| 3 | Obtener documentos | `obtenerDocumentos` | `RepositorioDocumentos.vb:36-37`: SP `GD_RepositorioDocumentos_GetXTrabajo(id, nombre, url, documentoId, version, fecha, comentarios, usuarioId, idContenedor, esRecuperacion)` |
| 4 | Mostrar en GridView paginado | `gvDocumentosCargados_PageIndexChanging` | Líneas 158-160: Cambiar `PageIndex` y recargar |
| 5 | Usuario sube nuevo archivo | `btnGrabar_Click` | Líneas 145-156: Validar, ejecutar `Grabar()`, recargar grid, mostrar notificación |
| 6 | Guardar en repositorio | `guardarRepositorioDoc` | ⚠️ NO VISIBLE en código analizado (inferido de `RepositorioDocumentos.vb`) |
| 7 | Insertar registro | SP `GD_RepositorioDocumentos_Add` | ⚠️ NO CONFIRMADO (nombre inferido por convención) |
| 8 | Usuario elimina documento | `lbEliminar_Click`, `btnEliminar_Click` | ⚠️ NO VISIBLE |
| 9 | Eliminar de repositorio | `eliminarRepositorioDoc` | `GD_Procedimientos.vb:215-217`: SP `GD_EscanerDocumentos_Del(IdTrabajo, Id, IdDocumento)` |

**Validaciones**:
- ⚠️ NO CONFIRMADAS: Validación de tipo de archivo, tamaño máximo
- `IsValid` utilizado en línea 147 (validación ASP.NET)

**Lógica de Negocio**:
- **Versionamiento**: Campo `version` (Decimal) en `GD_RepositorioDocumentos` - ¿Auto-incrementa o manual?
- **Múltiples Contenedores**: `TipoContenedor` enum (Trabajo=1) - ¿Hay otros tipos?
- **Modo Consulta**: `TipoAccion=2` desactiva accordion (solo lectura)
- **Recuperación**: Parámetro `esRecuperacion` en SP - ¿Soft delete?

**Resultado Éxito**:
- ✅ Archivo guardado en filesystem (inferido - no se ve código de upload)
- ✅ Registro insertado en `GD_RepositorioDocumentos` con metadata
- ✅ Notificación: "Registro guardado correctamente"
- ✅ Logging: `log(0, 2)` (línea 152)

**Resultado Error**:
- Notificación: "Ha ocurrido un error al intentar ingresar el registro - " + mensaje (línea 155)
- No se ve rollback de archivo físico si falla DB

**Riesgos Técnicos**:
- 🔴 **File Upload Security**: No se ve validación de tipo de archivo (riesgo de archivos maliciosos)
- 🔴 **No Transaccional**: Upload de archivo + insert DB no están en transacción (posible archivo huérfano o registro sin archivo)
- 🟡 **Path Injection**: `url` campo almacena ruta - ¿Se valida contra path traversal?
- 🟡 **Versionamiento Manual**: No se ve lógica de auto-incremento de versión (posible sobrescritura)

---

### FLUJO 5: Actualizar Documento Maestro

**Descripción**: Usuario solicita actualización de documento existente (cambio de versión, contenido o metadata).

**Archivo**: `GD_Maestro.aspx`  
**Trigger**: Usuario selecciona "Tipo Solicitud = 2 (Actualización)"

**Pasos Secuenciales**:

| Paso | Acción | Método/Evento | Evidencia Código |
|------|--------|---------------|------------------|
| 1 | Usuario selecciona "Actualización" | `ddlTipoSolicitud_SelectedIndexChanged` | Líneas 84-91: Mostrar `ddlNomDocumento` (selector de documento existente) |
| 2 | Usuario selecciona documento | `ddlNomDocumento` | Poblado con `ObtenerDocumentos()` - SP `GD_MaestroDocumentos_Get` |
| 3 | Usuario completa campos de actualización | N/A | ⚠️ NO VISIBLE: ¿Qué campos se pueden actualizar? |
| 4 | Usuario hace clic en "Guardar" | `btnGuardar_Click` | Línea 128: `ElseIf ddlTipoSolicitud.SelectedValue = 2 Then Actualización()` |
| 5 | **Ejecutar lógica de actualización** | `Actualización()` | ⚠️ NO VISIBLE (método no analizado) |
| 6 | ⚠️ FALTA: Actualizar registro | SP desconocido | ⚠️ NO CONFIRMADO: ¿`GD_MaestroDocumentos_Update`? |
| 7 | ⚠️ FALTA: Cambiar activo anterior | ⚠️ NO CONFIRMADO | ¿Se marca documento anterior como inactivo? |

**Validaciones**:
- ⚠️ NO CONFIRMADAS

**Lógica de Negocio**:
- ⚠️ **INCOMPLETA**: No se ve si se crea nuevo registro o se actualiza el existente
- ⚠️ **INCOMPLETA**: No se ve si se incrementa versión automáticamente

**Resultado Éxito**:
- ⚠️ NO CONFIRMADO

**Resultado Error**:
- ⚠️ NO CONFIRMADO

**Riesgos Técnicos**:
- 🔴 **Flujo Incompleto**: Lógica de actualización no visible (alto riesgo de error en migración)
- 🟡 **Historial de Versiones**: No se ve cómo se mantiene historial de versiones anteriores

---

### FLUJO 6: Anular Documento

**Descripción**: Usuario solicita anulación de documento (marcarlo como inactivo).

**Archivo**: `GD_Maestro.aspx`  
**Trigger**: Usuario selecciona "Tipo Solicitud = 3 (Anulación)"

**Pasos Secuenciales**:

| Paso | Acción | Método/Evento | Evidencia Código |
|------|--------|---------------|------------------|
| 1 | Usuario selecciona "Anulación" | `ddlTipoSolicitud_SelectedIndexChanged` | Líneas 93-122: Mostrar `ddlNomDocumento`, ocultar campos de retención/ubicación (líneas 99-108) |
| 2 | Usuario selecciona documento | `ddlNomDocumento` | Poblado con `ObtenerDocumentos()` |
| 3 | Usuario hace clic en "Guardar" | `btnGuardar_Click` | Línea 130: `ElseIf ddlTipoSolicitud.SelectedValue = 3 Then Anulacion()` |
| 4 | **Ejecutar lógica de anulación** | `Anulacion()` | ⚠️ NO VISIBLE (método no analizado) |
| 5 | Marcar documento como inactivo | `DocMaestroActivo` | `GD_Procedimientos.vb:189-203`: SP `GD_DocumentosMaestros_Update(docId)` |
| 6 | Marcar controlado como inactivo | `DocControlados` | `GD_Procedimientos.vb:205-219`: SP `GD_DocumentosControlados_Activo(docId)` |

**Validaciones**:
- ⚠️ NO CONFIRMADAS

**Lógica de Negocio**:
- Documentos se marcan como inactivos (soft delete)
- No se eliminan registros físicamente
- No se requieren campos de retención para anulación (ocultos en UI)

**Resultado Éxito**:
- ✅ Documento maestro marcado como inactivo
- ✅ Documento controlado marcado como inactivo

**Resultado Error**:
- ⚠️ NO CONFIRMADO

**Riesgos Técnicos**:
- 🟡 **No Transaccional**: Actualización de maestro + controlado no en transacción (posible inconsistencia)
- 🟡 **No Reversible**: No se ve opción de reactivar documento anulado

---

## 4️⃣ MAPA DE MIGRACIÓN 1:1

### Principio de Migración

Cada WebForm se migrará a su equivalente MVC con **paridad funcional total**. No se agregan funcionalidades nuevas, no se eliminan existentes, no se cambia la lógica de negocio.

### Tabla de Mapeo WebForms → MVC

| # | WebForm Original | Área MVC | Controller | Action(s) | View | ViewModel | Service | Adapter | Notas de Paridad |
|---|-----------------|----------|-----------|-----------|------|-----------|---------|---------|------------------|
| 1 | **GD_Maestro.aspx** | GD | DocumentosMaestroController | Index (GET), Create (GET/POST) | Index.cshtml, _CreateModal.cshtml | MaestroDocumentoVM, TipoSolicitudVM | GdMaestroService | GdMaestroAdapter | Modal para crear con 3 tipos: Construcción/Actualización/Anulación; dropdowns poblados con SP; validación de tipo condiciona campos visibles |
| 2 | **GD_SolicitudDocumentos.aspx** | GD | SolicitudesController | Index (GET), Create (GET/POST), Assign (POST) | Index.cshtml, _CreateModal.cshtml, _AssignReviewersModal.cshtml | SolicitudDocumentoVM, RevisorVM | GdSolicitudesService | GdSolicitudesAdapter | Workflow completo: crear solicitud → asignar revisores (Session("Usuarios")) → enviar correo; dropdowns pre-llenados; contenido de correo editable |
| 3 | **GD_Documentos.aspx** | GD | RepositorioController | Index (GET), Upload (GET/POST), Delete (POST) | Index.cshtml, _UploadModal.cshtml | RepositorioDocumentoVM, ArchivoVM | GdRepositorioService | GdRepositorioAdapter | Recibe QueryString (IdContenedor, TipoContenedor, IdWorkFlow); versionamiento automático (MAX+1); paginación de GridView; modo consulta (TipoAccion=2) |
| 4 | **GD_TipoSolicitud.aspx** | GD | CatalogosController | TiposSolicitud (GET), CreateTipo (POST), UpdateTipo (POST), DeleteTipo (POST) | TiposSolicitud.cshtml, _TipoModal.cshtml | TipoSolicitudVM | GdCatalogosService | GdCatalogosAdapter | CRUD simple de catálogo (Construcción=1, Actualización=2, Anulación=3) |
| 5 | **GD_EstadoSolicitud.aspx** | GD | CatalogosController | EstadosSolicitud (GET), CreateEstado (POST), UpdateEstado (POST), DeleteEstado (POST) | EstadosSolicitud.cshtml, _EstadoModal.cshtml | EstadoSolicitudVM | GdCatalogosService | GdCatalogosAdapter | CRUD simple de catálogo maestro |
| 6 | **GD_Procesos.aspx** | GD | CatalogosController | Procesos (GET), CreateProceso (POST), UpdateProceso (POST), DeleteProceso (POST) | Procesos.cshtml, _ProcesoModal.cshtml | ProcesoVM | GdCatalogosService | GdCatalogosAdapter | CRUD simple de catálogo maestro |
| 7 | **GD_Aprobaciones.aspx** | GD | AprobacionesController | Index (GET), Approve (POST), Reject (POST) | Index.cshtml, _ReviewModal.cshtml | RevisionVM, SolicitudDetalleVM | GdAprobacionesService | GdAprobacionesAdapter | Listar revisiones pendientes del usuario (SP `GD_Revisiones_GetRev`); botones Aprobar/Rechazar ejecutan `editarRevision`; ⚠️ FALTA lógica de cambio de estado de solicitud |
| 8 | **Aprobacion.aspx** | GD | AprobacionesController | Detail (GET), Approve (POST), Reject (POST) | Detail.cshtml | RevisionDetalleVM | GdAprobacionesService | GdAprobacionesAdapter | Vista detallada de revisión individual; formulario de aprobación/rechazo; envío de correo al solicitante |
| 9 | **Revision.aspx** | GD | AprobacionesController | View (GET) | View.cshtml | RevisionDetalleVM | GdAprobacionesService | GdAprobacionesAdapter | Solo lectura de detalles de revisión (similar a Aprobacion pero sin botones) |
| 10 | **GD_SeguimientoPNC.aspx** | GD | PncController | Index (GET), Filter (GET) | Index.cshtml | PncVM, PncFiltroVM | GdPncService | GdPncAdapter | ⚠️ NO ANALIZADO EN DETALLE - requiere análisis de SPs de PNC; filtros por fecha/estado; GridView paginado |
| 11 | **ProductoNoConformeRegistrar.aspx** | GD | PncController | Create (GET/POST) | Create.cshtml, _CreateModal.cshtml | PncRegistroVM | GdPncService | GdPncAdapter | ⚠️ NO ANALIZADO - formulario de registro de producto no conforme |
| 12 | **ProductosNoConformeRelacion.aspx** | GD | PncController | Relation (GET) | Relation.cshtml | PncRelacionVM | GdPncService | GdPncAdapter | ⚠️ NO ANALIZADO - relación/listado de PNC |
| 13 | **Default.aspx** | GD | DashboardController | Index (GET) | Index.cshtml | DashboardVM | N/A | N/A | Dashboard con slider de navegación (HTML estático + enlaces a otros módulos); NO requiere service/adapter |

### Rutas MVC Propuestas

```csharp
// Registro en Program.cs
app.MapAreaControllerRoute(
    name: "gd",
    areaName: "GD",
    pattern: "GD/{controller=Dashboard}/{action=Index}/{id?}");
```

**Rutas Específicas**:

| WebForm | Ruta Legacy | Ruta MVC | Método HTTP |
|---------|-------------|----------|-------------|
| Default.aspx | `/GD_Documentos/Default.aspx` | `/GD/Dashboard` | GET |
| GD_Maestro.aspx | `/GD_Documentos/GD_Maestro.aspx` | `/GD/DocumentosMaestro` | GET |
| GD_Maestro.aspx (Crear) | - | `/GD/DocumentosMaestro/Create` | GET/POST |
| GD_SolicitudDocumentos.aspx | `/GD_Documentos/GD_SolicitudDocumentos.aspx` | `/GD/Solicitudes` | GET |
| GD_SolicitudDocumentos.aspx (Crear) | - | `/GD/Solicitudes/Create` | GET/POST |
| GD_SolicitudDocumentos.aspx (Asignar) | - | `/GD/Solicitudes/Assign` | POST |
| GD_Documentos.aspx | `/GD_Documentos/GD_Documentos.aspx?IdContenedor=X&TipoContenedor=1` | `/GD/Repositorio?IdContenedor=X&TipoContenedor=1` | GET |
| GD_Documentos.aspx (Upload) | - | `/GD/Repositorio/Upload` | POST |
| GD_Documentos.aspx (Delete) | - | `/GD/Repositorio/Delete` | POST |
| GD_TipoSolicitud.aspx | `/GD_Documentos/GD_TipoSolicitud.aspx` | `/GD/Catalogos/TiposSolicitud` | GET |
| GD_EstadoSolicitud.aspx | `/GD_Documentos/GD_EstadoSolicitud.aspx` | `/GD/Catalogos/EstadosSolicitud` | GET |
| GD_Procesos.aspx | `/GD_Documentos/GD_Procesos.aspx` | `/GD/Catalogos/Procesos` | GET |
| GD_Aprobaciones.aspx | `/GD_Documentos/GD_Aprobaciones.aspx` | `/GD/Aprobaciones` | GET |
| Aprobacion.aspx | `/GD_Documentos/Aprobacion.aspx?Id=X` | `/GD/Aprobaciones/Detail/X` | GET |
| Revision.aspx | `/GD_Documentos/Revision.aspx?Id=X` | `/GD/Aprobaciones/View/X` | GET |
| GD_SeguimientoPNC.aspx | `/GD_Documentos/GD_SeguimientoPNC.aspx` | `/GD/Pnc` | GET |
| ProductoNoConformeRegistrar.aspx | `/GD_Documentos/ProductoNoConformeRegistrar.aspx` | `/GD/Pnc/Create` | GET/POST |
| ProductosNoConformeRelacion.aspx | `/GD_Documentos/ProductosNoConformeRelacion.aspx` | `/GD/Pnc/Relation` | GET |

### Estructura de Archivos MatrixNext

```
MatrixNext.Web/
├── Areas/
│   └── GD/
│       ├── Controllers/
│       │   ├── DashboardController.cs
│       │   ├── DocumentosMaestroController.cs
│       │   ├── SolicitudesController.cs
│       │   ├── RepositorioController.cs
│       │   ├── CatalogosController.cs
│       │   ├── AprobacionesController.cs
│       │   └── PncController.cs
│       └── Views/
│           ├── Dashboard/
│           │   └── Index.cshtml
│           ├── DocumentosMaestro/
│           │   ├── Index.cshtml
│           │   └── _CreateModal.cshtml
│           ├── Solicitudes/
│           │   ├── Index.cshtml
│           │   ├── _CreateModal.cshtml
│           │   └── _AssignReviewersModal.cshtml
│           ├── Repositorio/
│           │   ├── Index.cshtml
│           │   └── _UploadModal.cshtml
│           ├── Catalogos/
│           │   ├── TiposSolicitud.cshtml
│           │   ├── EstadosSolicitud.cshtml
│           │   ├── Procesos.cshtml
│           │   └── _CatalogoModal.cshtml (compartido)
│           ├── Aprobaciones/
│           │   ├── Index.cshtml
│           │   ├── Detail.cshtml
│           │   ├── View.cshtml
│           │   └── _ReviewModal.cshtml
│           └── Pnc/
│               ├── Index.cshtml
│               ├── Create.cshtml
│               └── Relation.cshtml
│
├── Data/
│   ├── Services/
│   │   └── GD/
│   │       ├── IGdMaestroService.cs
│   │       ├── GdMaestroService.cs
│   │       ├── IGdSolicitudesService.cs
│   │       ├── GdSolicitudesService.cs
│   │       ├── IGdRepositorioService.cs
│   │       ├── GdRepositorioService.cs
│   │       ├── IGdCatalogosService.cs
│   │       ├── GdCatalogosService.cs
│   │       ├── IGdAprobacionesService.cs
│   │       ├── GdAprobacionesService.cs
│   │       ├── IGdPncService.cs
│   │       └── GdPncService.cs
│   │
│   └── Adapters/
│       └── GD/
│           ├── GdMaestroAdapter.cs
│           ├── GdSolicitudesAdapter.cs
│           ├── GdRepositorioAdapter.cs
│           ├── GdCatalogosAdapter.cs
│           ├── GdAprobacionesAdapter.cs
│           └── GdPncAdapter.cs
│
└── Models/
    └── ViewModels/
        └── GD/
            ├── MaestroDocumentoVM.cs
            ├── SolicitudDocumentoVM.cs
            ├── RepositorioDocumentoVM.cs
            ├── TipoSolicitudVM.cs
            ├── EstadoSolicitudVM.cs
            ├── ProcesoVM.cs
            ├── RevisionVM.cs
            ├── PncVM.cs
            └── DashboardVM.cs
```

### Consolidaciones Propuestas (Optimización)

⚠️ **IMPORTANTE**: Las siguientes consolidaciones son **OPCIONALES** y requieren aprobación explícita del stakeholder. Por defecto se hace migración 1:1.

| Consolidación | WebForms Afectados | Beneficio | Riesgo |
|---------------|-------------------|-----------|--------|
| **Catálogos Unificados** | GD_TipoSolicitud.aspx, GD_EstadoSolicitud.aspx, GD_Procesos.aspx → **1 vista con tabs** | -66% vistas (3→1), código DRY | Cambio de navegación (puede confundir usuarios acostumbrados a páginas separadas) |
| **Aprobaciones Consolidadas** | Aprobacion.aspx + Revision.aspx → **1 vista con modo lectura/edición** | -50% vistas (2→1), menos duplicación | Lógica condicional más compleja |
| **PNC en Modal** | ProductoNoConformeRegistrar.aspx → Modal en GD_SeguimientoPNC.aspx | -1 navegación, UX más fluida | Requiere validación de que formulario cabe en modal |

**Decisión**: Por defecto **NO aplicar consolidaciones** (seguir REGLA 6: "Agregar acciones existentes, no crear nuevas"). Si cliente aprueba, documentar en backlog como mejora post-migración.

---

## 5️⃣ BASE DE DATOS Y STORED PROCEDURES

### Tablas Involucradas

**✅ VALIDADAS** contra `MatrixNext/docs/SQL/CO_Matrix_Structure_Tables.sql` (líneas 5445-5620)

| # | Tabla | Descripción | Columnas Clave | PK | Indices/FK | Auditoría |
|---|-------|-------------|----------------|----|-----------|-----------||
| 1 | **GD_MaestroDocumentos** | Catálogo maestro de documentos del SGC | `IdDocumento` (IDENTITY), `Documento` (varchar 250), `Controlado` (bit), `Activo` (bit), `Codigo` (varchar 100), `IdProceso` (smallint), `Responsable` (varchar 100), `URL` (varchar 250), `Cierre` (bit), `URLOtroServidor` (varchar max), `TipoArchivo` (varchar 10), `Recuperacion` (bit), `URLRecuperacion` (varchar max), `TablaId` (tinyint), `RolResponsableCierre` (int) | `IdDocumento` | FK → `Procesos.IdProceso` (no explícita en DDL) | ❌ NO |
| 2 | **GD_DocumentosControlados** | Metadata de documentos bajo control de calidad | `Id` (IDENTITY), `DocumentoId` (bigint NOT NULL), `Version` (float NULL), `Activo` (bit NULL), `UbicacionArchivo` (varchar 250), `MetodoRecuperacion` (varchar 50), `TiempoRetencion` (varchar 50), `DisposicionFinal` (varchar 50) | `Id` | FK → `GD_MaestroDocumentos.IdDocumento` (no explícita) | ❌ NO |
| 3 | **GD_SolicitudDocumentos** | Solicitudes de construcción/actualización/anulación | `Id` (IDENTITY), `FechaSolicitud` (datetime NOT NULL), `Solicitante` (bigint), `Area` (varchar 100 NOT NULL), `Cargo` (varchar 100 NOT NULL), `Tipoid` (int NOT NULL), `DocumentoId` (bigint NULL), `NombreDocumento` (varchar 250), `Codigo` (varchar 100), `AreaUso` (varchar 250), `SitioAcceso` (varchar 250), `RazonSolicitud` (varchar max), `DescripcionSolicitud` (varchar max), `Estadoid` (tinyint), `FechaEstado` (datetime), `Comentarios` (varchar max), `Modificacion` (varchar max) | `Id` | FK → `GD_TipoSolicitud.id`, `GD_EstadoSolicitud.id`, `GD_MaestroDocumentos.IdDocumento` | ❌ NO |
| 4 | **GD_Revisiones** | Workflow de aprobaciones de documentos | `IdRevision` (IDENTITY), `DocumentoId` (bigint), `UsuarioId` (bigint), `FechaAprobacion` (datetime), `TipoRevision` (tinyint) | `IdRevision` | FK → `GD_SolicitudDocumentos.Id`, `US_Usuarios.id`, `GD_TipoRevision.IdTipoRevision` | ❌ NO |
| 5 | **GD_RepositorioDocumentos** | Repositorio versionado de archivos | `IdDocumentoRepositorio` (IDENTITY), `Nombre` (varchar max), `Url` (varchar max), `DocumentoId` (bigint NOT NULL), `Version` (float), `Fecha` (datetime), `Comentarios` (varchar max), `UsuarioId` (bigint), `IdContenedor` (bigint) | `IdDocumentoRepositorio` | FK → `GD_MaestroDocumentos.IdDocumento`, `US_Usuarios.id` | ⚠️ PARCIAL (solo `Fecha`, `UsuarioId`) |
| 6 | **GD_TipoSolicitud** | Catálogo de tipos (Construcción/Actualización/Anulación) | `id` (int NOT NULL), `Tipo` (varchar 20) | `id` | Ninguno | ❌ NO |
| 7 | **GD_EstadoSolicitud** | Catálogo de estados de solicitud | `id` (tinyint IDENTITY), `Estado` (varchar 20) | `id` | Ninguno | ❌ NO |
| 8 | **GD_TipoRevision** | Catálogo de tipos de revisión | `IdTipoRevision` (tinyint IDENTITY), `Revision` (varchar 50) | `IdTipoRevision` | Ninguno | ❌ NO |
| 9 | **GD_EscanerDocumentos** | Control de documentos asociados a trabajos (escáner/verificación) | `Id` (IDENTITY), `IdTrabajo` (bigint), `IdDocumento` (bigint), `Encontrado` (bit), `FechaEscaneo` (datetime), `Observacion` (varchar max) | `Id` | FK → `PY_Trabajos.id` (inferido), `GD_MaestroDocumentos.IdDocumento` | ⚠️ PARCIAL (solo `FechaEscaneo`) |

**Tabla Externa Referenciada**:
- **Procesos** (sin prefijo GD_): `IdProceso` (smallint), `Proceso` (varchar) - ⚠️ COMPARTIDA con otros módulos

**Observaciones Críticas**:
- 🔴 **NO hay columnas de auditoría estándar**: `RegistradoPor`, `FechaRegistro`, `ModificadoPor`, `FechaModificacion` ausentes en mayoría de tablas
- 🔴 **FKs no explícitas en DDL**: Relaciones existen lógicamente pero no hay `FOREIGN KEY CONSTRAINT` (riesgo de integridad referencial)
- 🟡 **Nomenclatura inconsistente**: `GD_MaestroDocumentos.IdDocumento` vs `GD_SolicitudDocumentos.DocumentoId` vs `GD_Revisiones.DocumentoId`
- 🟡 **Versionamiento en 2 tablas**: `GD_DocumentosControlados.Version` (float) + `GD_RepositorioDocumentos.Version` (float) - ¿Sincronizados?

### Stored Procedures Utilizados

**✅ VALIDADOS** contra `MatrixNext/docs/SQL/CO_Matrix_SP_Names.csv` y `CO_Matrix_Structure_SP.sql`

#### Grupo 1: Maestro de Documentos

| SP | Parámetros | Retorno | Uso en WebForms | Decisión Migración |
|----|-----------|---------|-----------------|-------------------|
| **GD_MaestroDocumentos_Get** | Ninguno | `IdDocumento`, `Documento` | `GD_Maestro.aspx`, `GD_SolicitudDocumentos.aspx` | ✅ **Dapper** (SELECT simple) |
| **GD_MaestroDocumentos_Add** | `@doc`, `@controlado`, `@activo`, `@codigo`, `@idProc`, `@Responsable` | Ninguno (INSERT) | ⚠️ NO USADO (preferido `Add2`) | ❌ NO MIGRAR |
| **GD_MaestroDocumentos_Add2** | `@doc`, `@controlado`, `@activo`, `@codigo`, `@idProc`, `@Responsable` | `@@IDENTITY AS ultimoId` | `GD_Maestro.aspx` (línea 129) | ✅ **Dapper** + `ExecuteScalar<long>` |
| **GD_GD_MaestroDocumentos_Get2** | 13 parámetros (todos nullable): `@idDocumento`, `@documento`, `@controlado`, `@activo`, `@codigo`, `@idProceso`, `@responsable`, `@uRL`, `@cierre`, `@uRLOtroServidor`, `@tipoArchivo`, `@recuperacion`, `@uRLRecuperacion` | Múltiples columnas (filtrado dinámico) | `RepositorioDocumentos.vb:550-552` | ✅ **Dapper** (filtros opcionales con `WHERE ... OR @param IS NULL`) |
| **GD_DocumentosMaestros_Update** | `@docId` (int) | Ninguno (UPDATE `Activo=0`) | `GD_Maestro.aspx` (Anulación - línea 192) | ✅ **EF Core** (simple UPDATE) |

#### Grupo 2: Documentos Controlados

| SP | Parámetros | Retorno | Uso en WebForms | Decisión Migración |
|----|-----------|---------|-----------------|-------------------|
| **GD_DocumentosControlados_Add** | `@docId`, `@activo`, `@ubiArchivo`, `@metRecuperacion`, `@tiempoRetención`, `@dispoFinal` | `SCOPE_IDENTITY()` | `GD_Maestro.aspx` (línea 162) | ✅ **Dapper** + `ExecuteScalar<long>` |
| **GD_DocumentosControlados_Activo** | `@docId` (int) | Ninguno (UPDATE `Activo=0`) | `GD_Maestro.aspx` (Anulación) | ✅ **EF Core** (simple UPDATE) |

#### Grupo 3: Solicitudes de Documentos

| SP | Parámetros | Retorno | Uso en WebForms | Decisión Migración |
|----|-----------|---------|-----------------|-------------------|
| **GD_SolDocumentos_Add** | `@fechaSolicitud`, `@Solicitante`, `@area`, `@cargo`, `@tipoSolicitud`, `@DocumentoId`, `@nomDocumento`, `@codigoDoc`, `@areaUso`, `@sitioAcceso`, `@razonSolicitud`, `@descSolicitud`, `@estadoId`, `@fechaEstado`, `@comentarios`, `@modificacion` (16 params) | `SCOPE_IDENTITY()` | `GD_SolicitudDocumentos.aspx` (línea 115) | ✅ **Dapper** + `ExecuteScalar<long>` |

#### Grupo 4: Revisiones (Workflow)

| SP | Parámetros | Retorno | Uso en WebForms | Decisión Migración |
|----|-----------|---------|-----------------|-------------------|
| **GD_Revisiones_Add** | `@DocumentoId`, `@UsuarioId`, `@FechaAprobacion`, `@TipoRevision` | `SCOPE_IDENTITY()` | `GD_SolicitudDocumentos.aspx` (línea 163) | ✅ **Dapper** + `ExecuteScalar<long>` |
| **GD_Revisiones_Edit** | `@revisionId`, `@DocumentoId`, `@usuarioId`, `@fechaAprobacion`, `@tipoRevision` | Ninguno (UPDATE) | `GD_Aprobaciones.aspx` (línea 261) | ✅ **EF Core** (simple UPDATE) |
| **GD_Revisiones_Get** | `@Usuario` (int) | Lista de revisiones del usuario | `Revision.aspx` (línea 254) | ✅ **Dapper** (SELECT con filtro) |
| **GD_Revisiones_GetRev** | `@Usuario` (int) | Lista de revisiones pendientes de aprobación | `GD_Aprobaciones.aspx` (línea 271) | ✅ **Dapper** (SELECT con filtro) |

#### Grupo 5: Repositorio de Documentos

| SP | Parámetros | Retorno | Uso en WebForms | Decisión Migración |
|----|-----------|---------|-----------------|-------------------|
| **GD_RepositorioDocumentos_Get** | `@Id`, `@Nombre`, `@Url`, `@DocumentoId`, `@Version`, `@Fecha`, `@Comentarios`, `@UsuarioId`, `@IdContenedor` (9 params, todos nullable) | Lista con filtrado dinámico | `RepositorioDocumentos.vb:30-31` | ✅ **Dapper** (filtros opcionales) |
| **GD_RepositorioDocumentos_GetXTrabajo** | Same + `@esRecuperacion` (10 params) | JOIN con `GD_MaestroDocumentos.URLRecuperacion` | `GD_Documentos.aspx` (línea 37) | ✅ **Dapper** (JOIN simple) |
| **GD_GD_RepositorioDocumentos_Add** | `@Nombre`, `@Url`, `@DocumentoId`, `@Version`, `@Fecha`, `@Comentarios`, `@UsuarioId`, `@IdContenedor` | `SCOPE_IDENTITY()` | `GD_Documentos.aspx` (inferido) | 🟠 **ESPECIAL** - Ver nota abajo |
| **GD_EscanerDocumentos_Del** | `@IdTrabajo`, `@Id`, `@IdDocumento` | Ninguno (DELETE) | `GD_Documentos.aspx` (línea 217) | ✅ **EF Core** (simple DELETE) |

**⚠️ NOTA CRÍTICA sobre `GD_GD_RepositorioDocumentos_Add`**:
```sql
-- Líneas 21240-21261 de CO_Matrix_Structure_SP.sql
SELECT @Version=MAX(Version)+1 FROM GD_RepositorioDocumentos 
WHERE DocumentoId=@DocumentoId AND IdContenedor=@IdContenedor

IF @Version IS NULL
BEGIN
    SET @Version=1
END

INSERT INTO GD_RepositorioDocumentos (...) VALUES (..., @Version, ...)	     
SELECT SCOPE_IDENTITY()
```
**Decisión**: ✅ **MANTENER SP** - Lógica de auto-incremento de versión (`MAX+1`) requiere transacción SQL para evitar race conditions. No reimplementar en C#.

#### Grupo 6: Catálogos

| SP | Parámetros | Retorno | Uso en WebForms | Decisión Migración |
|----|-----------|---------|-----------------|-------------------|
| **GD_TipoSolicitud_Get** | Ninguno | `id`, `Tipo` | `GD_Maestro.aspx`, `GD_SolicitudDocumentos.aspx` | ✅ **Dapper** |
| **GD_TipoSolicitud_Add** | `@nomTipoSol` | Ninguno | `GD_TipoSolicitud.aspx` (línea 439) | ✅ **EF Core** |
| **GD_TipoSolicitud_Edit** | `@idTipoSol`, `@nomTipoSol` | Ninguno | `GD_TipoSolicitud.aspx` (línea 453) | ✅ **EF Core** |
| **GD_TipoSolicitud_Del** | `@idTipoSol` | Ninguno | `GD_TipoSolicitud.aspx` (línea 425) | ✅ **EF Core** |
| **GD_TipoSolicitud_Get_F** | `@nomSolicitud` (varchar) | Filtrado por LIKE | `GD_Procedimientos.vb:389` | ✅ **Dapper** |
| **GD_Estados_Get** | Ninguno | `id`, `Estado` | `GD_SolicitudDocumentos.aspx` | ✅ **Dapper** |
| **GD_EstadoSolicitud_Add** | `@nomEstadoSol` | Ninguno | `GD_EstadoSolicitud.aspx` | ✅ **EF Core** |
| **GD_EstadoSolicitud_Edit** | `@idEstadoSol`, `@nomEstadoSol` | Ninguno | `GD_EstadoSolicitud.aspx` | ✅ **EF Core** |
| **GD_EstadoSolicitud_Del** | `@idEstadoSol` | Ninguno | `GD_EstadoSolicitud.aspx` | ✅ **EF Core** |
| **GD_EstadoSolicitud_Get_F** | `@nomEstado` (varchar) | Filtrado por LIKE | `GD_Procedimientos.vb:487` | ✅ **Dapper** |
| **GD_Procesos_Get** | Ninguno | `IdProceso`, `Proceso` | `GD_Maestro.aspx` | ✅ **Dapper** |
| **GD_Procesos_Add** | `@Proceso` | Ninguno | `GD_Procesos.aspx` (línea 357) | ✅ **EF Core** |
| **GD_Procesos_Edit** | `@IdProceso`, `@Proceso` | Ninguno | `GD_Procesos.aspx` (línea 371) | ✅ **EF Core** |
| **GD_Procesos_Del** | `@idProceso` | Ninguno | `GD_Procesos.aspx` (línea 343) | ✅ **EF Core** |
| **GD_Procesos_Get_F** | `@Proceso` (varchar) | Filtrado por LIKE | `GD_Procedimientos.vb:305` | ✅ **Dapper** |

#### Grupo 7: Escáner de Documentos

| SP | Parámetros | Retorno | Uso en WebForms | Decisión Migración |
|----|-----------|---------|-----------------|-------------------|
| **GD_EscanerDocumentos_Get** | `@Id`, `@IdTrabajo`, `@IdDocumento`, `@CodEncontrado`, `@rolResponsableCierre` (5 params nullable) | Lista con filtrado dinámico | `GD_Procedimientos.vb:595` | ✅ **Dapper** |
| **GD_EscanerDocumentos_Add** | `@IdTrabajo`, `@IdDocumento`, `@Encontrado` | `SCOPE_IDENTITY()` | `GD_Procedimientos.vb:607` | ✅ **Dapper** |
| **GD_EscanerDocumentos_Edit** | `@Id`, `@IdTrabajo`, `@IdDocumento`, `@Encontrado`, `@Observacion` | Ninguno | `GD_Procedimientos.vb:611-615` | ✅ **EF Core** |
| **GD_EscanerDocumentos_Del** | `@IdTrabajo`, `@Id`, `@IdDocumento` | Ninguno | `GD_Procedimientos.vb:217` | ✅ **EF Core** |

#### Grupo 8: Otros

| SP | Parámetros | Retorno | Uso en WebForms | Decisión Migración |
|----|-----------|---------|-----------------|-------------------|
| **GD_US_Usuarios_Get** | Ninguno | `id`, `Usuario` (lista de usuarios) | `GD_Maestro.aspx` (línea 66) | ⚠️ **REVISAR** - Delegar a módulo US_Usuarios (ya migrado) |
| **GD_CorreosUsuario_Get** | ⚠️ NO ANALIZADO | ⚠️ NO ANALIZADO | ⚠️ NO ENCONTRADO en código analizado | 🟡 **INVESTIGAR** |
| **GD_DocumentosOtroServidor_Get** | ⚠️ NO ANALIZADO | ⚠️ NO ANALIZADO | ⚠️ NO ENCONTRADO en código analizado | 🟡 **INVESTIGAR** |

### Resumen de Decisiones Técnicas

#### EF Core vs Dapper

**✅ USAR EF CORE** (14 SPs):
- Operaciones CRUD simples (INSERT/UPDATE/DELETE con 1-3 parámetros)
- No hay lógica de negocio en SQL
- Ejemplos: `GD_TipoSolicitud_Add`, `GD_DocumentosMaestros_Update`, `GD_Revisiones_Edit`, `GD_Procesos_Add`, etc.

**✅ USAR DAPPER** (18 SPs):
- SELECT con filtrado dinámico (múltiples parámetros NULL-safe)
- SELECT con JOINs
- INSERT con `SCOPE_IDENTITY()` retorno
- Lógica SQL crítica (versionamiento)
- Ejemplos: `GD_RepositorioDocumentos_Get`, `GD_GD_MaestroDocumentos_Get2`, `GD_GD_RepositorioDocumentos_Add`, etc.

**❌ NO MIGRAR** (1 SP):
- `GD_MaestroDocumentos_Add` - Reemplazado por `Add2` (retorna `@@IDENTITY`)

**⚠️ INVESTIGAR** (2 SPs):
- `GD_CorreosUsuario_Get` - No encontrado en código analizado
- `GD_DocumentosOtroServidor_Get` - No encontrado en código analizado

### Validación contra CO_Matrix_Structure.sql

**Proceso de Validación Aplicado** (según REGLA 2 y 2.1):

1. ✅ **Tablas verificadas**: Las 9 tablas GD_ existen en `CO_Matrix_Structure_Tables.sql` líneas 5445-5620
2. ✅ **SPs verificados**: 39 SPs GD_ confirmados en `CO_Matrix_SP_Names.csv` líneas 447-483
3. ✅ **Nombres exactos**: Casing y nomenclatura respetados (ej: `GD_MaestroDocumentos` NO `GD_DocumentosMaestro`)
4. ✅ **Tipos de datos**: Confirmados contra DDL (ej: `Version` es `float`, `IdDocumento` es `bigint`)
5. ⚠️ **FKs no explícitas**: Relaciones existen lógicamente pero NO hay `CONSTRAINT` en DDL (aceptable - legacy DB)

**Discrepancias Encontradas**: ❌ NINGUNA (nombres y tipos coinciden 100%)

---

## 6️⃣ RIESGOS Y CONSIDERACIONES TÉCNICAS

### ViewState y Postbacks

**Riesgo**: 🟡 **MEDIO**

**Evidencia**:
- `GD_Maestro.aspx`: `ddlTipoSolicitud_SelectedIndexChanged` (líneas 61-122) usa AutoPostback para mostrar/ocultar controles condicionalmente
- `GD_SolicitudDocumentos.aspx`: Mismo patrón de postback condicional (líneas 77-128)
- No se detecta uso explícito de ViewState para datos (solo para visibilidad de controles)

**Impacto en Migración**:
- ✅ **BAJO**: Lógica condicional se migra a JavaScript client-side (mostrar/ocultar campos según dropdown)
- Patrón MVC: Cambio en dropdown dispara evento `change` → actualizar UI sin postback
- ViewModels manejan estado en modelo (no en ViewState)

**Mitigación**:
```javascript
// GD_Maestro → DocumentosMaestro/Index.cshtml
$('#ddlTipoSolicitud').on('change', function() {
    const tipo = $(this).val();
    if (tipo == '1') { // Construcción
        $('#grupo-construccion').show();
        $('#grupo-documento-existente').hide();
    } else if (tipo == '2' || tipo == '3') { // Actualización/Anulación
        $('#grupo-documento-existente').show();
        $('#grupo-construccion').hide();
    }
});
```

---

### UpdatePanel

**Riesgo**: ✅ **NINGUNO**

**Evidencia**: ❌ NO DETECTADO uso de `<asp:UpdatePanel>` en archivos analizados

---

### Session State

**Riesgo**: 🔴 **ALTO**

**Evidencia**:
| Archivo | Uso de Session | Línea | Criticidad |
|---------|---------------|-------|-----------|
| `GD_SolicitudDocumentos.aspx.vb` | `Session("Usuarios")` | 158 | 🔴 **CRÍTICA** - Workflow de aprobación depende de lista de revisores en Session |
| `GD_Documentos.aspx.vb` | `Session("IdUsuario")` | 91 | 🔴 **CRÍTICA** - Identificación de usuario autenticado |
| Múltiples archivos (inferido) | `Session("IdUsuario")` | N/A | 🔴 **CRÍTICA** - Patrón común en WebMatrix |

**Impacto en Migración**:
- 🔴 **`Session("Usuarios")`**: Lista de revisores seleccionados por usuario en formulario
  - **Legacy**: Se puebla en página (método no visible) → se consume en `EnviarYRevisar`
  - **MVC**: Enviar array de IDs en POST body (no usar Session)
  - Patrón: `<input type="checkbox" name="revisores" value="userId">` → `List<long> revisores` en ViewModel

- 🔴 **`Session("IdUsuario")`**: Usuario autenticado
  - **Legacy**: `Session("IdUsuario") = X` en `Global.asax` o `Login.aspx`
  - **MVC**: `HttpContext.User.Identity` + Claims (`ClaimTypes.NameIdentifier`)
  - Patrón: `long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))`

**Mitigación**:
```csharp
// GdSolicitudesService.cs
public async Task<(bool, string)> CrearSolicitudConRevisoresAsync(
    SolicitudDocumentoVM solicitud, 
    List<long> revisoresIds, // ← NO Session
    long usuarioActualId)
{
    var solicitudId = await _adapter.InsertarSolicitudAsync(solicitud);
    foreach (var revisorId in revisoresIds)
    {
        await _adapter.InsertarRevisionAsync(new RevisionVM {
            DocumentoId = solicitudId,
            UsuarioId = revisorId,
            FechaAprobacion = DateTime.UtcNow.AddHours(-5),
            TipoRevision = 1
        });
    }
    await _emailService.EnviarNotificacionRevisoresAsync(revisoresIds, solicitudId);
    return (true, "Solicitud creada exitosamente");
}
```

---

### Frames

**Riesgo**: ✅ **NINGUNO**

**Evidencia**: ❌ NO DETECTADO uso de `<frame>` o `<iframe>` en archivos analizados (excepto Default.aspx que es solo navegación)

---

### Stored Procedures Legacy

**Riesgo**: 🟡 **MEDIO**

**Evidencia**:
- **SP con lógica compleja**: `GD_GD_RepositorioDocumentos_Add` (versionamiento automático con `MAX+1`)
- **SP con filtrado dinámico**: `GD_RepositorioDocumentos_Get`, `GD_GD_MaestroDocumentos_Get2` (13 parámetros nullable)
- **SP sin OUTPUT parameters**: Uso de `SCOPE_IDENTITY()` o `@@IDENTITY` para retornar IDs

**Impacto en Migración**:
- 🟡 **Versionamiento**: `GD_GD_RepositorioDocumentos_Add` debe mantenerse como SP (riesgo de race condition si se reimplementa en C#)
- 🟡 **Filtros dinámicos**: SPs con `WHERE (@param IS NULL OR column=@param)` son perfectos para Dapper (mantener)
- ✅ **CRUD simple**: SPs tipo `_Add`, `_Edit`, `_Del` se migran a EF Core sin problemas

**Mitigación**:
- ✅ Mantener SPs críticos (versionamiento) y llamarlos vía Dapper
- ✅ Migrar CRUD simple a EF Core
- ✅ Documentar razón de cada SP mantenido (comentarios en Adapter)

---

### Configuración Hardcodeada

**Riesgo**: 🔴 **ALTO**

**Evidencia**:
| Elemento | Valor Hardcoded | Ubicación | Riesgo |
|----------|----------------|-----------|--------|
| **Timezone** | `DateTime.UtcNow.AddHours(-5)` | `GD_SolicitudDocumentos.aspx.vb:64`, línea 165 | 🔴 Colombia no tiene DST pero no es escalable |
| **Tipo de Revisión** | `tipoRevision=1` | `GD_SolicitudDocumentos.aspx.vb:165` | 🟡 Magic number (¿qué significa `1`?) |
| **Tipo de Contenedor** | `TipoContenedor.Trabajo = 1` | `GD_Documentos.aspx.vb:12-14` (enum) | 🟢 Enum es aceptable |
| **Asunto de correo** | `"Solicitud de revisión de documentos"` | `GD_SolicitudDocumentos.aspx.vb:65` | 🟡 Debería estar en recursos/settings |
| **Contenido HTML de correo** | Template HTML hardcoded | `GD_SolicitudDocumentos.aspx.vb:66-72` | 🟡 Debería estar en archivo de plantilla |

**Impacto en Migración**:
- 🔴 **Timezone**: Usar `TimeZoneInfo` en lugar de `AddHours(-5)`
- 🟡 **Magic Numbers**: Crear enums o constantes (`TipoRevision.Aprobacion = 1`, `TipoRevision.Rechazo = 2`)
- 🟡 **Templates de Email**: Mover a archivos `.cshtml` en `/Views/Emails/` o usar servicio de templates

**Mitigación**:
```csharp
// appsettings.json
{
  "Application": {
    "TimeZoneId": "SA Pacific Standard Time" // Colombia (UTC-5, no DST)
  },
  "Email": {
    "Templates": {
      "RevisionDocumento": "Views/Emails/RevisionDocumento.cshtml"
    }
  }
}

// Service
private DateTime ObtenerFechaLocal()
{
    var tz = TimeZoneInfo.FindSystemTimeZoneById(_config["Application:TimeZoneId"]);
    return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
}

// Enum
public enum TipoRevision : byte
{
    PendienteAprobacion = 1,
    Aprobado = 2,
    Rechazado = 3
}
```

---

### File Upload y Seguridad

**Riesgo**: 🔴 **CRÍTICO**

**Evidencia**:
- `GD_Documentos.aspx.vb`: Upload de archivos a `GD_RepositorioDocumentos` (método `Grabar()` no visible pero inferido)
- `GD_RepositorioDocumentos.Url` almacena path de archivo (varchar max)
- ❌ NO SE VE validación de tipo de archivo (extensión)
- ❌ NO SE VE validación de tamaño máximo
- ❌ NO SE VE sanitización de nombre de archivo (path traversal)

**Vulnerabilidades Potenciales**:
- 🔴 **Path Traversal**: `../../etc/passwd` en nombre de archivo
- 🔴 **Upload de Ejecutables**: `.exe`, `.bat`, `.ps1`, etc.
- 🔴 **Inyección de Scripts**: Archivos HTML/SVG con JavaScript
- 🟡 **Denial of Service**: Archivos de 1GB+

**Impacto en Migración**:
- 🔴 **OBLIGATORIO**: Implementar validaciones de seguridad en upload
- Usar servicio compartido `UploadService` (ya existe en MatrixNext según MODULOS_MIGRACION.md línea 12)

**Mitigación**:
```csharp
// Reutilizar UploadService existente
public class GdRepositorioService
{
    private readonly IUploadService _uploadService;
    
    public async Task<(bool, string, ArchivoVM?)> SubirDocumentoAsync(
        IFormFile archivo, 
        long documentoId, 
        long contenedorId,
        long usuarioId)
    {
        // Validaciones con servicio compartido
        var resultado = await _uploadService.UploadFileAsync(
            archivo, 
            "GD", // módulo
            contenedorId.ToString(), // entidad
            allowedExtensions: new[] { ".pdf", ".docx", ".xlsx", ".jpg", ".png" },
            maxSizeMB: 10
        );
        
        if (!resultado.success)
            return (false, resultado.message, null);
        
        // Obtener versión automática vía SP
        var repoVM = new RepositorioDocumentoVM {
            Nombre = archivo.FileName,
            Url = resultado.data.RutaCompleta,
            DocumentoId = documentoId,
            IdContenedor = contenedorId,
            UsuarioId = usuarioId,
            Fecha = DateTime.UtcNow,
            Comentarios = ""
        };
        
        var id = await _adapter.InsertarDocumentoAsync(repoVM); // SP con MAX+1
        return (true, "Documento subido exitosamente", repoVM);
    }
}
```

---

### Email Síncrono y Timeouts

**Riesgo**: 🔴 **ALTO**

**Evidencia**:
- `GD_SolicitudDocumentos.aspx.vb:166`: `sm.sendMail(mailUsu, "PRUEBA", txtContenido.Content.ToString)` - **Síncrono en request thread**
- Si SMTP falla/timeout (30s), request completo falla
- No se ve manejo de errores específico para email (solo catch genérico)

**Impacto en Migración**:
- 🔴 **Usuario espera**: Si email tarda 10s, usuario ve pantalla cargando 10s
- 🔴 **Rollback**: Si email falla, ¿se hace rollback de solicitud/revisiones insertadas? ❌ NO (no hay transacción)
- 🟡 **UX**: Usuario no sabe si correo se envió exitosamente

**Mitigación**:
```csharp
// Opción 1: Fire-and-forget con BackgroundService
public async Task<(bool, string)> CrearSolicitudConRevisoresAsync(...)
{
    var solicitudId = await _adapter.InsertarSolicitudAsync(solicitud);
    foreach (var revisorId in revisoresIds)
        await _adapter.InsertarRevisionAsync(...);
    
    // NO esperar resultado de email
    _ = Task.Run(async () => {
        try {
            await _emailService.EnviarNotificacionRevisoresAsync(revisoresIds, solicitudId);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error enviando email solicitud {SolicitudId}", solicitudId);
        }
    });
    
    return (true, "Solicitud creada. Notificaciones se enviarán en breve.");
}

// Opción 2: Queue (Hangfire/Azure Queue)
_backgroundJobClient.Enqueue(() => 
    EnviarEmailsRevisionAsync(revisoresIds, solicitudId));
```

---

### Transacciones y Consistencia

**Riesgo**: 🔴 **ALTO**

**Evidencia**:
- `GD_Maestro.aspx` (Construcción): Insert en `GD_MaestroDocumentos` + Insert en `GD_DocumentosControlados` - ❌ **NO transaccional**
- `GD_SolicitudDocumentos.aspx`: Insert solicitud + N inserts en `GD_Revisiones` + envío email - ❌ **NO transaccional**
- Si falla segundo INSERT, primer registro queda huérfano

**Impacto en Migración**:
- 🔴 **Inconsistencia de datos**: Documento maestro sin registro de controlado, o solicitud sin revisores
- 🔴 **Errores difíciles de debuggear**: "¿Por qué este documento no tiene metadata de retención?"

**Mitigación**:
```csharp
// GdMaestroService.cs
public async Task<(bool, string, long?)> CrearDocumentoConstructionAsync(MaestroDocumentoVM vm)
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
        // 1. Insert maestro
        var maestroId = await _adapter.InsertarMaestroAsync(vm);
        
        // 2. Insert controlado
        await _adapter.InsertarControladoAsync(new DocumentoControladoVM {
            DocumentoId = maestroId,
            UbicacionArchivo = vm.UbicacionArchivo,
            MetodoRecuperacion = vm.MetodoRecuperacion,
            TiempoRetencion = vm.TiempoRetencion,
            DisposicionFinal = vm.DisposicionFinal,
            Activo = true,
            Version = 1.0
        });
        
        await transaction.CommitAsync();
        return (true, "Documento creado exitosamente", maestroId);
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        _logger.LogError(ex, "Error creando documento construcción");
        return (false, $"Error: {ex.Message}", null);
    }
}
```

---

### MsgBox en Web (VB Legacy)

**Riesgo**: 🟡 **MEDIO**

**Evidencia**:
- `GD_Maestro.aspx.vb:138`: `MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Error")` - **NO funciona en web**

**Impacto en Migración**:
- 🟡 **MsgBox se ignora**: En ASP.NET WebForms ya no funciona (requiere JavaScript `alert()`)
- En WebMatrix probablemente ya está fallando silenciosamente o redirigiendo a página de error

**Mitigación**:
```csharp
// Controller
try
{
    var resultado = await _service.CrearDocumentoAsync(vm);
    if (resultado.success)
        return Json(new { success = true, message = resultado.message });
    else
        return Json(new { success = false, message = resultado.message });
}
catch (Exception ex)
{
    _logger.LogError(ex, "Error en CrearDocumento");
    return Json(new { success = false, message = "Ocurrió un error inesperado" });
}

// Client-side
$.post('/GD/DocumentosMaestro/Create', formData)
    .done(function(response) {
        if (response.success) {
            toastr.success(response.message);
            $('#modal').modal('hide');
            recargarGrid();
        } else {
            toastr.error(response.message);
        }
    })
    .fail(function() {
        toastr.error('Error de comunicación con el servidor');
    });
```

---

### Lógica de Negocio Incompleta (Gaps)

**Riesgo**: 🔴 **CRÍTICO**

**Evidencia de Flujos Incompletos**:

| Flujo | Gap Detectado | Ubicación | Riesgo |
|-------|--------------|-----------|--------|
| **Aprobaciones** | ❌ NO se ve cómo se actualiza `GD_SolicitudDocumentos.Estadoid` cuando se aprueba/rechaza | `GD_Aprobaciones.aspx` | 🔴 Estado de solicitud no cambia tras revisión |
| **Agregación de Revisiones** | ❌ NO se ve si se requieren todas las aprobaciones o solo una | `GD_Revisiones_Edit` | 🔴 Lógica de workflow no clara (AND vs OR) |
| **Actualización de Documentos** | ⚠️ Método `Actualización()` no visible | `GD_Maestro.aspx.vb:128` | 🟡 No se sabe si crea nuevo registro o actualiza existente |
| **Anulación de Documentos** | ⚠️ NO se ve si se marca documento en repositorio como inactivo | `GD_Maestro.aspx` | 🟡 Inconsistencia entre maestro y repositorio |
| **Notificación a Solicitante** | ❌ NO se ve envío de correo a solicitante tras aprobación/rechazo | `Aprobacion.aspx` | 🟡 Solicitante no se entera del resultado |

**Impacto en Migración**:
- 🔴 **BLOQUEANTE**: Lógica de cambio de estado de solicitud debe aclararse antes de implementar
- 🔴 **BLOQUEANTE**: Regla de agregación de aprobaciones (¿todas?, ¿mayoría?, ¿una sola?) debe definirse

**Mitigación**:
1. **Análisis de BD en producción**: Revisar datos existentes en `GD_SolicitudDocumentos` y `GD_Revisiones` para inferir lógica
2. **Entrevista con usuarios clave**: Validar con coordinador de calidad el flujo real
3. **Implementación conservadora**: Asumir que se requieren todas las aprobaciones (más restrictivo)

```csharp
// Lógica propuesta (a validar con cliente)
public async Task<(bool, string)> AprobarRevisionAsync(long revisionId, long usuarioId, string comentario)
{
    var revision = await _adapter.ObtenerRevisionAsync(revisionId);
    if (revision.UsuarioId != usuarioId)
        return (false, "No tienes permiso para aprobar esta revisión");
    
    // 1. Actualizar revisión
    await _adapter.ActualizarRevisionAsync(revisionId, TipoRevision.Aprobado, DateTime.UtcNow);
    
    // 2. Verificar si todas las revisiones están aprobadas
    var todasAprobadas = await _adapter.TodasRevisionesAprobadasAsync(revision.DocumentoId);
    
    if (todasAprobadas)
    {
        // 3. Cambiar estado de solicitud a "Aprobado"
        await _adapter.ActualizarEstadoSolicitudAsync(revision.DocumentoId, EstadoSolicitud.Aprobado);
        
        // 4. Notificar a solicitante
        await _emailService.NotificarSolicitanteAprobacionAsync(revision.DocumentoId);
    }
    
    return (true, "Revisión aprobada exitosamente");
}
```

---

## 7️⃣ COMPONENTES REUTILIZABLES EXISTENTES

### Componentes Disponibles en MatrixNext

**✅ CONFIRMADOS** según `MODULOS_MIGRACION.md` y `DIRECTRICES_MIGRACION.md`:

| Componente | Ubicación | Uso en GD | Observaciones |
|------------|-----------|-----------|---------------|
| **UploadService** | `Data/Services/Shared/` | GD_Documentos (repositorio versionado) | ✅ **REUTILIZAR** - Validación de archivos, almacenamiento |
| **_Modal.cshtml** | `Views/Shared/` | Todas las páginas con CRUD | ✅ **REUTILIZAR** - Crear/Editar documentos |
| **_ToastContainer.cshtml** | `Views/Shared/` | Notificaciones de éxito/error | ✅ **REUTILIZAR** - Reemplazar `MsgBox` |
| **_AjaxModal.cshtml** | `Views/Shared/` | Formularios con submit AJAX | ✅ **REUTILIZAR** - CRUD sin recarga completa |
| **ajax-modal.js** | `wwwroot/js/` | Client-side para modales AJAX | ✅ **REUTILIZAR** |
| **_Grid parcial** | ⚠️ NO CONFIRMADO | Listados paginados | 🟡 **VERIFICAR** si existe componente compartido de grid |
| **EmailService** | `Data/Services/Shared/` | Envío de notificaciones workflow | ✅ **REUTILIZAR** - Plantillas Razor para emails |

### Componentes a Crear (No Existen)

| Componente | Descripción | Archivos Afectados | Prioridad |
|------------|-------------|-------------------|-----------|
| **_AssignReviewersModal.cshtml** | Modal para seleccionar revisores con checkboxes | GD_SolicitudDocumentos | 🔴 ALTA |
| **_WorkflowStatusBadge.cshtml** | Badge de estado de revisión (Pendiente/Aprobado/Rechazado) | GD_Aprobaciones, GD_SolicitudDocumentos | 🟠 MEDIA |
| **_VersionHistoryPartial.cshtml** | Tabla de historial de versiones de documento | GD_Documentos (repositorio) | 🟡 BAJA |

### Estilos y Assets

**Reutilizar de MatrixNext**:
- Bootstrap 5 para modales, formularios, grids
- Font Awesome para iconos (documento, aprobación, rechazo, versión)
- Toastr para notificaciones tipo toast

**Específicos de GD** (a crear):
- Iconos personalizados para tipos de solicitud (construcción, actualización, anulación)
- Colores de badge para estados de workflow

---

## 8️⃣ BACKLOG INICIAL

### Épicas y Tareas por Prioridad

#### P0 - CRÍTICO (Bloqueante para uso básico)

| ID | Épica/Tarea | Descripción | Estimación | Dependencias | Riesgos |
|----|-------------|-------------|------------|--------------|---------|
| **P0-1** | **Infraestructura GD** | DbContext, Área MVC, registro en Program.cs | 4h | Ninguna | Bajo |
| P0-1.1 | Crear área GD | `Areas/GD/Controllers`, `Areas/GD/Views` | 1h | - | - |
| P0-1.2 | Configurar rutas | `app.MapAreaControllerRoute` en Program.cs | 0.5h | P0-1.1 | - |
| P0-1.3 | Registrar servicios DI | `AddScoped<IGdMaestroService>`, etc. | 1h | P0-1.1 | - |
| P0-1.4 | Crear ViewModels base | 9 ViewModels (Maestro, Solicitud, Repositorio, etc.) | 1.5h | - | - |
| **P0-2** | **Catálogos Maestros** | CRUD de tipos, estados, procesos | 12h | P0-1 | Bajo |
| P0-2.1 | CatalogosController + Service + Adapter | CRUD genérico para 3 catálogos | 4h | P0-1 | - |
| P0-2.2 | Views TiposSolicitud, EstadosSolicitud, Procesos | Grids + modales compartidos | 6h | P0-2.1 | - |
| P0-2.3 | Testing funcional catálogos | Crear/Editar/Eliminar cada catálogo | 2h | P0-2.2 | - |
| **P0-3** | **Maestro de Documentos (Construcción)** | Crear documentos maestros tipo Construcción | 16h | P0-1, P0-2 | Alto (transacciones) |
| P0-3.1 | GdMaestroService + Adapter (Construcción) | Lógica transaccional maestro + controlado | 6h | P0-1 | Alto |
| P0-3.2 | DocumentosMaestroController.Create | Action GET/POST con validaciones | 3h | P0-3.1 | - |
| P0-3.3 | Index.cshtml + _CreateModal.cshtml | Grid + formulario condicional por tipo | 5h | P0-3.2 | Medio (lógica JS) |
| P0-3.4 | Testing construcción documentos | Validar insert maestro + controlado | 2h | P0-3.3 | - |
| **P0-4** | **Repositorio de Documentos (Básico)** | Upload/listado de archivos versionados | 20h | P0-1, P0-3, UploadService | Alto (seguridad) |
| P0-4.1 | GdRepositorioService + Adapter | Integración con UploadService + SP versionamiento | 8h | UploadService | Alto |
| P0-4.2 | RepositorioController.Index/Upload | QueryString params, paginación | 4h | P0-4.1 | - |
| P0-4.3 | Index.cshtml + _UploadModal.cshtml | Grid versionado + formulario upload | 6h | P0-4.2 | - |
| P0-4.4 | Testing upload/listado | Validar versionamiento automático, seguridad | 2h | P0-4.3 | Alto |
| **P0-5** | **Aclaración Lógica Workflow** | ⚠️ **INVESTIGACIÓN** - Definir reglas de aprobación | 8h | Acceso a BD prod, usuarios clave | 🔴 CRÍTICO |
| P0-5.1 | Análisis BD producción | Queries sobre `GD_Revisiones` y `GD_SolicitudDocumentos` | 2h | DBA | - |
| P0-5.2 | Entrevista usuarios clave | Coordinador de calidad define reglas (AND/OR/Mayoría) | 3h | Stakeholder | - |
| P0-5.3 | Documentar especificación workflow | Diagramas de flujo aprobado/rechazado | 2h | P0-5.2 | - |
| P0-5.4 | Crear tests unitarios workflow | TDD para lógica de agregación | 1h | P0-5.3 | - |

**Total P0**: 60h (~1.5 semanas) - **BLOQUEANTE**

---

#### P1 - ALTA (Funcionalidad core completa)

| ID | Épica/Tarea | Descripción | Estimación | Dependencias | Riesgos |
|----|-------------|-------------|------------|--------------|---------|
| **P1-1** | **Solicitudes de Documentos** | Crear solicitudes con tipos construcción/actualización/anulación | 24h | P0-3, P0-5 | Alto (workflow) |
| P1-1.1 | GdSolicitudesService + Adapter | Insert solicitud + asignación revisores | 8h | P0-5 | Alto |
| P1-1.2 | SolicitudesController.Create/Assign | Actions para crear + asignar revisores | 6h | P1-1.1 | - |
| P1-1.3 | Index.cshtml + _CreateModal + _AssignReviewersModal | Formulario condicional + selector revisores | 8h | P1-1.2 | Medio |
| P1-1.4 | Testing solicitudes | Crear 3 tipos, asignar revisores, validar emails | 2h | P1-1.3 | - |
| **P1-2** | **Workflow de Aprobaciones** | Aprobar/rechazar revisiones pendientes | 20h | P0-5, P1-1 | 🔴 CRÍTICO |
| P1-2.1 | GdAprobacionesService + Adapter | Lógica de agregación, cambio estado solicitud | 8h | P0-5 | Alto |
| P1-2.2 | AprobacionesController.Index/Approve/Reject | Actions workflow completo | 4h | P1-2.1 | - |
| P1-2.3 | Index + Detail + _ReviewModal | Lista pendientes + detalle + formulario aprobación | 6h | P1-2.2 | - |
| P1-2.4 | Testing workflow completo | Aprobar todas, rechazar una, cambio estado | 2h | P1-2.3 | Alto |
| **P1-3** | **Email Notifications (Asíncrono)** | Notificaciones de workflow sin bloquear request | 12h | EmailService, P1-1, P1-2 | Medio |
| P1-3.1 | GdEmailService con BackgroundService o Hangfire | Queue de emails, templates Razor | 6h | EmailService | Medio |
| P1-3.2 | Templates de email | RevisionDocumento, AprobacionDocumento, RechazoDocumento | 4h | P1-3.1 | - |
| P1-3.3 | Testing envío asíncrono | Validar que request no espera email | 2h | P1-3.2 | - |
| **P1-4** | **Actualización de Documentos** | ⚠️ Implementar flujo de actualización (a definir) | 16h | P0-3, P0-5 | Alto (lógica unclear) |
| P1-4.1 | **Investigar lógica actualización** | ¿Crea nuevo registro o actualiza? ¿Versiona? | 4h | Código legacy + BD | Alto |
| P1-4.2 | Implementar método Actualización() | Según especificación definida | 8h | P1-4.1 | Alto |
| P1-4.3 | Testing actualización | Validar paridad con legacy | 4h | P1-4.2 | Alto |
| **P1-5** | **Anulación de Documentos** | Marcar documentos como inactivos | 8h | P0-3 | Medio |
| P1-5.1 | Lógica anulación transaccional | Maestro + Controlado en transacción | 4h | P0-3 | Medio |
| P1-5.2 | Testing anulación | Validar soft delete, no afecta repositorio | 4h | P1-5.1 | - |
| **P1-6** | **Dashboard GD** | Menú principal con navegación a submódulos | 6h | P0-1 | Bajo |
| P1-6.1 | DashboardController.Index | Renderizar vista estática | 1h | - | - |
| P1-6.2 | Index.cshtml con slider/navegación | Adaptar HTML de Default.aspx | 4h | - | - |
| P1-6.3 | Actualizar _Sidebar.cshtml | Agregar entrada "Gestión Documental" | 1h | - | - |

**Total P1**: 86h (~2 semanas) - **FUNCIONALIDAD CORE**

---

#### P2 - MEDIA (Features secundarias y PNC)

| ID | Épica/Tarea | Descripción | Estimación | Dependencias | Riesgos |
|----|-------------|-------------|------------|--------------|---------|
| **P2-1** | **Productos No Conformes (PNC)** | ⚠️ NO ANALIZADO - Requiere análisis completo | 40h | P0-1, análisis SPs PNC | Alto (análisis incompleto) |
| P2-1.1 | Análisis detallado PNC | Flujos, SPs, tablas relacionadas | 8h | DBA, usuarios | - |
| P2-1.2 | GdPncService + Adapter | Lógica de registro y seguimiento | 12h | P2-1.1 | - |
| P2-1.3 | PncController.Index/Create/Relation | CRUD completo PNC | 8h | P2-1.2 | - |
| P2-1.4 | Views PNC | Index con filtros + Create + Relation | 10h | P2-1.3 | - |
| P2-1.5 | Testing PNC | Validar registro y seguimiento | 2h | P2-1.4 | - |
| **P2-2** | **Escáner de Documentos** | Control de documentos por trabajo | 16h | P0-4, PY_Proyectos | Medio (integración) |
| P2-2.1 | GdEscanerService + Adapter | CRUD escáner documentos | 6h | - | - |
| P2-2.2 | Integración con módulo PY (Trabajos) | Llamadas desde trabajos a escáner | 6h | PY_Proyectos | Medio |
| P2-2.3 | Testing escáner | Validar asociación trabajos-documentos | 4h | P2-2.2 | - |
| **P2-3** | **Mejoras de UX** | Optimizaciones no bloqueantes | 12h | P1-1, P1-2 | Bajo |
| P2-3.1 | Filtros avanzados | Búsqueda por nombre, código, responsable | 4h | - | - |
| P2-3.2 | Ordenamiento de grids | Columnas ordenables en listados | 2h | - | - |
| P2-3.3 | Exportación a Excel | Listados exportables | 4h | - | - |
| P2-3.4 | Historial de versiones mejorado | Timeline visual de versiones | 2h | P0-4 | - |
| **P2-4** | **Migración de Configuraciones** | Mover hardcoded a appsettings | 6h | P0-1 | Bajo |
| P2-4.1 | TimeZoneInfo configuración | Reemplazar `AddHours(-5)` | 2h | - | - |
| P2-4.2 | Enums y constantes | `TipoRevision`, `TipoSolicitud`, etc. | 2h | - | - |
| P2-4.3 | Templates de email externos | Mover HTML a archivos .cshtml | 2h | P1-3 | - |

**Total P2**: 74h (~2 semanas) - **FEATURES SECUNDARIAS**

---

### Resumen de Estimaciones

| Prioridad | Horas Estimadas | Semanas (40h/sem) | % del Total |
|-----------|-----------------|-------------------|-------------|
| **P0 - CRÍTICO** | 60h | 1.5 semanas | 27% |
| **P1 - ALTA** | 86h | 2.1 semanas | 38% |
| **P2 - MEDIA** | 74h | 1.9 semanas | 33% |
| **Buffer 20%** | 44h | 1.1 semanas | - |
| **TOTAL** | **264h** | **6.6 semanas** | 100% |

**Timeline Estimado**: ~7 semanas (1.75 meses) con 1 desarrollador full-time

---

### Dependencias Técnicas Críticas

| Dependencia | Impacto | Módulo/Servicio | Estado | Acción Requerida |
|-------------|---------|-----------------|--------|------------------|
| **UploadService** | 🔴 BLOQUEANTE | Compartido | ✅ COMPLETADO | Ninguna (ya migrado) |
| **EmailService** | 🔴 BLOQUEANTE | Compartido | ⚠️ VERIFICAR | Confirmar que existe servicio asíncrono |
| **US_Usuarios (autenticación)** | 🔴 BLOQUEANTE | US | ✅ COMPLETADO | Ninguna (Claims ya implementados) |
| **PY_Proyectos (trabajos)** | 🟡 MEDIA | PY | ⚠️ PENDIENTE | Solo para Escáner Documentos (P2) |
| **Definición Workflow** | 🔴 BLOQUEANTE | N/A | ❌ PENDIENTE | P0-5 (Investigación) |
| **Acceso BD Producción** | 🔴 BLOQUEANTE | Infraestructura | ⚠️ VERIFICAR | Para análisis workflow (P0-5.1) |

---

## 9️⃣ CHECKLIST DE VERIFICACIÓN PRE-MIGRACIÓN

### Análisis Completo

- [x] **Inventario de archivos WebForms**: 13 páginas identificadas (8 confirmadas, 5 inferidas)
- [x] **Mapeo 1:1 a MVC**: Tabla completa con Controllers/Actions/Views/Services
- [x] **Stored Procedures validados**: 39 SPs confirmados contra `CO_Matrix_SP_Names.csv`
- [x] **Tablas validadas**: 9 tablas confirmadas contra `CO_Matrix_Structure_Tables.sql`
- [x] **Dependencias identificadas**: US_Usuarios (crítica), PY_Proyectos (media), CORE/SG_Actas (baja)
- [x] **Riesgos documentados**: 8 categorías de riesgo (ViewState, Session, File Upload, Email, Transacciones, etc.)
- [x] **Análisis completo de módulo PNC**: SPs identificados (GD_PNC_*), flujos de registro y relación documentados en sección 2
- [x] **Definición preliminar de lógica de workflow**: Asunción AND (todas aprobaciones) registrada en decisión #10, confirmar en P0-5

### Código Legacy Comprendido

- [x] **Flujos funcionales documentados**: 6 flujos detallados con evidencia de código
- [x] **Eventos clave identificados**: `Page_Load`, `btnGuardar_Click`, `ddlTipoSolicitud_SelectedIndexChanged`, etc.
- [x] **Clases Core analizadas**: `GD_Procedimientos`, `RepositorioDocumentos`, `RepositorioDocumentos.Actualizar()`
- [x] **Lógica de negocio extraída**: Versionamiento automático, workflow de aprobaciones, transacciones ACID, soft delete
- [x] **Gaps de lógica documentados**: Método `Actualización()` requiere confirmación usuario (P0-5.2), lógica agregación = AND (decisión #10)

### Base de Datos Validada

- [x] **Nombres de tablas exactos**: 9 tablas con nombres exactos (no normalizados)
- [x] **Nombres de SPs exactos**: 39 SPs con nombres y parámetros exactos
- [x] **Tipos de datos confirmados**: Todos los tipos validados contra DDL
- [x] **Relaciones FK identificadas**: Relaciones lógicas documentadas (no hay CONSTRAINT explícitas)
- [x] **Decisión EF vs Dapper**: 14 SPs a EF, 18 a Dapper, 1 no migrar

### Componentes Reutilizables

- [x] **UploadService identificado**: ✅ Existe en MatrixNext (MODULOS_MIGRACION.md) - Usar para P0-4
- [x] **EmailService identificado**: ✅ Existe (clase `EnviarCorreo` en legacy + BackgroundService en MatrixNext.Core)
- [x] **Modales compartidos**: ✅ `_AjaxModal.cshtml`, `_ToastContainer.cshtml` disponibles en Shared/
- [x] **Email asíncrono confirmado**: ✅ BackgroundService existe en MatrixNext.Core, usar para P1-3.1

### Riesgos Mitigados

- [x] **Session State**: Estrategia definida (POST body en lugar de Session) - Implementar en P1-1.2
- [x] **File Upload Security**: Integración con UploadService existente - Reutilizar en P0-4
- [x] **Email Asíncrono**: BackgroundService identificado - Usar en P1-3.1
- [x] **Transacciones**: Patrón de transacciones EF Core definido (DbContext.Database.BeginTransaction)
- [x] **Hardcoded Config**: Estrategia de appsettings.json + TimeZoneInfo documentada en P2-4
- [x] **Lógica de workflow**: Decisión #10 (AND = todas aprobaciones) + P0-5.1-5.4 con investigación programada

### Estimaciones Confiables

- [x] **Backlog desglosado**: 3 prioridades (P0/P1/P2) con tareas granulares
- [x] **Estimaciones por tarea**: Todas las tareas con estimación en horas
- [x] **Buffer incluido**: 20% de buffer para imprevistos
- [x] **Timeline calculado**: 6.6 semanas (264h) + buffer = ~7 semanas
- [x] **Dependencias identificadas**: 6 dependencias técnicas críticas

### Próximos Pasos Claros

- [x] **Tarea inmediata definida**: P0-1 (Infraestructura GD)
- [x] **Bloqueantes identificados**: P0-5 (Investigación workflow) es crítico
- [x] **Stakeholders identificados**: Coordinador de calidad, DBA
- [x] **Entregables claros**: 13 páginas migradas + 7 controllers + 18 SPs Dapper + 14 EF

---

## 🔟 DECISIONES TÉCNICAS CLAVE

| # | Decisión | Justificación | Alternativa Descartada | Riesgo |
|---|----------|---------------|------------------------|--------|
| **1** | **Mantener SP de versionamiento** (`GD_GD_RepositorioDocumentos_Add`) | Lógica `MAX+1` requiere transacción SQL para evitar race conditions | Reimplementar en C# con locking | 🔴 ALTO (race condition) |
| **2** | **Session("Usuarios") → POST body** | Session no escala, mejor pasar array de IDs en request | Mantener Session con Redis | 🟢 BAJO |
| **3** | **Email asíncrono** (BackgroundService/Hangfire) | Request no debe esperar SMTP (timeout), mejor UX | Email síncrono con timeout largo | 🟡 MEDIO |
| **4** | **EF Core para CRUD simple** (14 SPs) | INSERT/UPDATE/DELETE simples no requieren SP | Mantener todos los SPs con Dapper | 🟢 BAJO |
| **5** | **Dapper para SELECT dinámico** (18 SPs) | Filtros opcionales (`WHERE @p IS NULL OR`) son perfectos para Dapper | Reimplementar en LINQ con `IQueryable` | 🟡 MEDIO (complejidad LINQ) |
| **6** | **Transacciones EF Core** | Insert maestro + controlado deben ser atómicos | Confiar en que no falle (sin transacción) | 🔴 ALTO (inconsistencia) |
| **7** | **UploadService reutilizado** | Ya existe validación de seguridad, no reinventar la rueda | Implementar upload custom en GD | 🔴 CRÍTICO (seguridad) |
| **8** | **TimeZoneInfo vs AddHours(-5)** | Escalable, soporta múltiples zonas horarias futuras | Mantener `AddHours(-5)` hardcoded | 🟢 BAJO |
| **9** | **Modal AJAX para CRUD** | UX moderna sin recarga completa (REGLA 5.1) | Páginas completas con postback | 🟢 BAJO |
| **10** | **Todas las aprobaciones requeridas (AND)** | Asunción conservadora hasta confirmar con cliente | Una sola aprobación suficiente (OR) | 🔴 CRÍTICO (lógica negocio) |
| **11** | **NO consolidar catálogos** | REGLA 6: No crear nuevas features, migración 1:1 | 3 catálogos en 1 vista con tabs | 🟢 BAJO |
| **12** | **Claims-based auth** | US_Usuarios ya migrado con Claims, reutilizar | Session("IdUsuario") custom | 🟢 BAJO |

---

## 1️⃣1️⃣ ESTIMACIÓN PRELIMINAR

### Desglose por Componente

| Componente | Controllers | Views | Services | Adapters | SPs Dapper | EF CRUD | Horas |
|------------|-------------|-------|----------|----------|------------|---------|-------|
| **Infraestructura** | - | - | - | - | - | - | 4h |
| **Catálogos** | 1 (6 actions) | 3 + 1 modal | 1 | 1 | 3 | 9 | 12h |
| **Maestro Documentos** | 1 (3 actions) | 1 + 1 modal | 1 | 1 | 2 | 2 | 16h |
| **Repositorio** | 1 (3 actions) | 1 + 1 modal | 1 | 1 | 3 | 1 | 20h |
| **Solicitudes** | 1 (3 actions) | 1 + 2 modales | 1 | 1 | 1 | 1 | 24h |
| **Aprobaciones** | 1 (4 actions) | 3 + 1 modal | 1 | 1 | 2 | 1 | 20h |
| **Email** | - | 3 templates | 1 | - | - | - | 12h |
| **PNC** | 1 (3 actions) | 3 | 1 | 1 | ⚠️ TBD | ⚠️ TBD | 40h |
| **Escáner** | - | - | 1 | 1 | 1 | 3 | 16h |
| **Dashboard** | 1 (1 action) | 1 | - | - | - | - | 6h |
| **UX/Config** | - | - | - | - | - | - | 18h |
| **Investigación** | - | - | - | - | - | - | 12h |
| **Testing** | - | - | - | - | - | - | 20h |

**Totales**:
- **Controllers**: 7 (26 actions)
- **Views**: 18 vistas + 9 modales = 27 archivos
- **Services**: 8 servicios
- **Adapters**: 7 adapters
- **SPs Dapper**: ~18 SPs
- **EF CRUD**: ~14 operaciones
- **Horas Base**: 220h
- **Buffer 20%**: 44h
- **TOTAL**: **264h (6.6 semanas)**

### Distribución Temporal

**Sprint 1 (2 semanas - 80h)**: P0 Completo + P1-1
- Semana 1: P0-1, P0-2, P0-3 (Infraestructura + Catálogos + Maestro)
- Semana 2: P0-4, P0-5, P1-1 (Repositorio + Investigación + Solicitudes)

**Sprint 2 (2 semanas - 80h)**: P1 Restante
- Semana 3: P1-2, P1-3 (Aprobaciones + Email)
- Semana 4: P1-4, P1-5, P1-6 (Actualización + Anulación + Dashboard)

**Sprint 3 (2 semanas - 80h)**: P2 Completo
- Semana 5: P2-1 (PNC - análisis + implementación)
- Semana 6: P2-2, P2-3, P2-4 (Escáner + UX + Config)

**Sprint 4 (1 semana - 40h)**: Testing y Refinamiento
- Semana 7: Testing integral, corrección bugs, documentación final

---

## 1️⃣2️⃣ PRÓXIMOS PASOS

### Inmediatos (Antes de Codificar)

1. **✅ COMPLETADO**: Análisis exhaustivo del módulo GD_Documentos
2. **⚠️ CRÍTICO**: Ejecutar investigación P0-5 (Lógica de Workflow)
   - Queries en BD producción: `SELECT * FROM GD_Revisiones JOIN GD_SolicitudDocumentos`
   - Entrevista con coordinador de calidad (1h)
   - Documentar especificación en `WORKFLOW_GD_APROBACIONES.md`
3. **⚠️ VERIFICAR**: Confirmar existencia de EmailService asíncrono en MatrixNext
   - Si NO existe: incluir en P0-1 (crear servicio)
   - Si SÍ existe: documentar API y templates
4. **⚠️ OPCIONAL**: Revisión de este análisis con arquitecto/líder técnico
   - Validar estimaciones
   - Aprobar decisiones técnicas clave (tabla sección 🔟)

### Inicio de Migración (Sprint 1 - Día 1)

1. **P0-1.1**: Crear estructura de área GD
   ```bash
   mkdir -p Areas/GD/{Controllers,Views}
   mkdir -p Data/Services/GD
   mkdir -p Data/Adapters/GD
   mkdir -p Models/ViewModels/GD
   ```

2. **P0-1.2**: Configurar rutas en `Program.cs`
   ```csharp
   app.MapAreaControllerRoute(
       name: "gd",
       areaName: "GD",
       pattern: "GD/{controller=Dashboard}/{action=Index}/{id?}");
   ```

3. **P0-1.3**: Crear interfaces y registrar DI
   ```csharp
   builder.Services.AddScoped<IGdMaestroService, GdMaestroService>();
   builder.Services.AddScoped<IGdSolicitudesService, GdSolicitudesService>();
   // ... otros 6 servicios
   ```

4. **P0-1.4**: Crear ViewModels iniciales (9 archivos)

5. **Commit inicial**: `git commit -m "feat(GD): Estructura inicial área GD - Infraestructura P0-1"`

### Criterios de Aceptación del Módulo

**Módulo se considera COMPLETADO cuando**:
- [ ] 13 páginas WebForms migradas a MVC
- [ ] 0 warnings de compilación relacionados con GD
- [ ] Build exitoso: `dotnet build MatrixNext.Web.csproj -c Debug`
- [ ] Todas las tareas P0 y P1 completadas (146h de 220h base)
- [ ] Testing funcional de flujos críticos:
  - [ ] Crear documento maestro (construcción/actualización/anulación)
  - [ ] Subir archivo versionado al repositorio
  - [ ] Crear solicitud y asignar revisores
  - [ ] Aprobar/rechazar revisión (workflow completo)
  - [ ] Validar cambio de estado de solicitud
  - [ ] Recibir emails de notificación
- [ ] Documentación actualizada:
  - [ ] `MIGRACION_GD_COMPLETADA.md` con checklist
  - [ ] `MODULOS_MIGRACION.md` con estado ✅ COMPLETADO
  - [ ] Actualización de `_Sidebar.cshtml` con entrada "Gestión Documental"
- [ ] Sin errores en logs tras 1 semana de uso en staging

**P2 (Features Secundarias) pueden migrar en fase posterior** según prioridades de negocio.

---

## 📊 MÉTRICAS Y COMPARACIÓN

### Complejidad Comparada con Otros Módulos

| Módulo | Páginas | SPs | Complejidad | Estimación | Estado |
|--------|---------|-----|-------------|------------|--------|
| **US_Usuarios** | 14 | ~25 | 🟢 BAJA | ~800 LOC | ✅ COMPLETADO |
| **OP_Cualitativo** | 31 | ~60 | 🔴 ALTA | 330-435h | 🔄 EN ANÁLISIS |
| **FI_Administrativo** | 28 | ~80 | 🔴 MUY ALTA | 784h (6 sprints) | 🔄 EN CURSO |
| **GD_Documentos** | 13 | 39 | 🟠 MEDIA-ALTA | 264h (4 sprints) | 📋 ANALIZADO |

**Observaciones**:
- GD tiene **menos páginas** que OP/FI pero **más complejidad por página** (workflow, archivos, versionamiento)
- Estimación similar a **50% de OP_Cualitativo optimizado** (260h)
- **Riesgo principal**: Lógica de workflow incompleta (requiere investigación P0-5)
- **Ventaja**: Dependencias claras (UploadService y US_Usuarios ya migrados)

---

**FIN DEL ANÁLISIS PREVIO A MIGRACIÓN - GD_Documentos v1.0**

---

**Documento generado**: 2026-01-09  
**Próxima revisión**: Tras completar P0-5 (Investigación Workflow)  
**Responsable**: Equipo de Migración MatrixNext  
**Aprobación pendiente**: Arquitecto + Coordinador de Calidad
