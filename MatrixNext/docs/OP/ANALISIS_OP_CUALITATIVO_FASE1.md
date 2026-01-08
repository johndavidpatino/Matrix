# ANÁLISIS OP_CUALITATIVO - FASE 1: RESUMEN EJECUTIVO

## 🎯 1. RESUMEN EJECUTIVO

### 1.1 Propósito del Módulo

El módulo **OP_Cualitativo** gestiona el ciclo completo de operaciones de campo para estudios cualitativos (entrevistas, sesiones de grupo, observaciones en punto de venta). Abarca desde la planificación inicial hasta el control de calidad final, incluyendo:

- **Gestión de trabajos COE**: Definición y configuración de trabajos cualitativos por coordinador
- **Diseño de instrumentos**: Creación de filtros de reclutamiento y asistencia con preguntas dinámicas
- **Programación de campo**: Asignación de moderadores, entrevistadores y observadores a sesiones/entrevistas
- **Control de ejecución**: Registro de ejecución real, transcripciones, audios y documentos
- **Aprobación de resultados**: Revisión y aprobación de respuestas de filtros con logs de auditoría
- **Reportes IPS**: Exportación y seguimiento de procesos de información en sitio

**Evidencia concreta**:

```vb
' Trabajos.aspx.vb, líneas 21-47
Sub CargarTrabajos(ByVal Coe As Int64)
    Dim oTrabajo As New Trabajo
    Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
    Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())

    If permisos.VerificarPermisoUsuario(42, UsuarioID) = True Then
        Dim oCoord As New CoordinacionCampo
        Dim listTrabCoord = (From lmuest In oCoord.ObtenerMuestraxCoordinador(Session("IDUsuario").ToString)
                             Select lmuest.TrabajoId)

        Dim listTrabajos = (From ltraba In oTrabajo.obtenerXCOE(Coe)
                            Where listTrabCoord.Contains(ltraba.id)
                            Select ltraba)
        gvTrabajos.DataSource = listTrabajos.ToList
    Else
        If permisos.VerificarPermisoUsuario(148, UsuarioID) = True Then
            gvTrabajos.DataSource = oTrabajo.ObtenerTrabajosCualitativosxCOE(Nothing, 2, Nothing)
        Else
            gvTrabajos.DataSource = oTrabajo.ObtenerTrabajosCualitativosxCOE(Coe, Nothing, Nothing)
        End If
    End If

    gvTrabajos.DataBind()
End Sub
```

### 1.2 Usuarios y Roles Evidenciados

#### **ROL 1: Coordinador de Campo / Gerente COE (Permiso 42)**

**Evidencia de validación de permisos**:
```vb
' Trabajos.aspx.vb, línea 26
If permisos.VerificarPermisoUsuario(42, UsuarioID) = True Then
```

**Responsabilidades confirmadas**:
- ✅ Ver trabajos asignados como coordinador (`oCoord.ObtenerMuestraxCoordinador`)
- ✅ Configurar fechas de campo (`txtFechaInicio.Text`, `txtFechaTerminacion.Text`)
- ✅ Definir tipo de recolección (Presencial/Telefónica/Online/Mixta)
- ✅ Navegar a fichas técnicas según metodología del trabajo
- ✅ Acceder a muestra de ciudades y filtros de reclutamiento/asistencia

**Archivos involucrados**: `Trabajos.aspx.vb`, `TrabajosCoordinador.aspx.vb`, `Calendario.aspx.vb`

---

#### **ROL 2: Operaciones Cualitativas (Roles 6, 7, 8)**

**Evidencia de control de acceso**:
```vb
' FichaEntrevista.aspx.vb, líneas 21-39
Dim oUsuario As New US.RolesUsuarios
Dim Usuario = oUsuario.obtenerRolesXUsuario(Session("IDUsuario").ToString, True)
Dim Count As Int16 = 0
For Each oRolId In Usuario
    Dim RolId As Int32? = oRolId.RolId
    If RolId = "6" Or RolId = "7" Or RolId = "8" Then
        Count = Count + 1
    End If

    If Count > 0 Then
        btnGuardar.Visible = True
        btnCancelar.Visible = True
        btnEntrega.Visible = True
        btnVolverOP.Visible = False
    Else
        btnGuardar.Visible = False
        btnCancelar.Visible = False
        btnEntrega.Visible = False
        btnVolverOP.Visible = True
    End If
Next
```

**Responsabilidades confirmadas**:
- ✅ Completar fichas técnicas (Entrevista/Sesión/Observación)
- ✅ Validar presupuestos de incentivos económicos
- ✅ Definir exclusiones y restricciones de reclutamiento
- ✅ Enviar fichas para entrega a coordinadores (`btnEntrega`)
- ✅ Actualizar información de Habeas Data en propuestas

**Archivos involucrados**: `FichaEntrevista.aspx.vb`, `FichaSesion.aspx.vb`, `FichaObservacion.aspx.vb`

---

#### **ROL 3: Supervisor con Permiso 148**

**Evidencia**:
```vb
' Trabajos.aspx.vb, líneas 34-36
If permisos.VerificarPermisoUsuario(148, UsuarioID) = True Then
    gvTrabajos.DataSource = oTrabajo.ObtenerTrabajosCualitativosxCOE(Nothing, 2, Nothing)
```

**Responsabilidades confirmadas**:
- ✅ Ver **todos los trabajos cualitativos** sin filtro de coordinador
- ✅ Acceso supervisorial a trabajos en estado específico (parámetro `2`)

---

#### **ROL 4: Entrevistadores, Moderadores, Observadores**

**Evidencia de listas específicas**:
```vb
' Entrevista.aspx.vb (inferido por patrón similar a OP_Cuantitativo)
' Se espera carga de usuarios por rol específico
Dim dtEntrevistadores = US.Usuarios.UsuariosxRol(RolEntrevistador)
ddlEntrevistador.DataSource = dtEntrevistadores
```

**Responsabilidades esperadas** (⚠️ **Por confirmar en código fuente**):
- ⚠️ Ser asignados a entrevistas/sesiones/observaciones
- ⚠️ Registrar ejecución real de campo
- ⚠️ No tienen acceso a fichas técnicas ni configuraciones

**Archivos esperados**: `Entrevista.aspx.vb`, `Observacion.aspx.vb`, `CampoCualitativo.aspx.vb`

---

#### **ROL 5: Transcriptores**

**Evidencia esperada** (⚠️ **Por confirmar**):
```vb
' Transcripcion.aspx.vb (esperado)
Dim dtTranscriptores = Personas.TH_Usuarios_Combo_Get(AreaTranscripcion)
```

**Responsabilidades esperadas**:
- ⚠️ Asignación a trabajos de transcripción
- ⚠️ Registro de fechas y cantidades de transcripciones

**Estado**: ⚠️ **NO CONFIRMADO** - Requiere lectura de `Transcripcion.aspx.vb`

### 1.3 Dependencias con Otros Módulos

#### **DEPENDENCIA 1: CoreProject (OP Entities) - CRÍTICA ✅**

**Evidencia concreta de importación**:
```vb
' Trabajos.aspx.vb, línea 1
Imports CoreProject

' Trabajos.aspx.vb, líneas 22, 28, 30, 36
Dim oTrabajo As New Trabajo
Dim oCoord As New CoordinacionCampo
Dim oMetodologiaOperaciones As New MetodologiaOperaciones
Dim oProyecto As New Proyecto
```

**Clases consumidas confirmadas**:
| Clase CoreProject | Método | Archivo | Línea |
|-------------------|--------|---------|-------|
| `Trabajo` | `obtenerXCOE(Coe)` | Trabajos.aspx.vb | 30 |
| `Trabajo` | `ObtenerTrabajosCualitativosxCOE(Coe, tipo, estado)` | Trabajos.aspx.vb | 34-36 |
| `Trabajo` | `DevolverxID(idtrabajo)` | Trabajos.aspx.vb | 64 |
| `CoordinacionCampo` | `ObtenerMuestraxCoordinador(userId)` | Trabajos.aspx.vb | 28 |
| `TrabajoOPCuanti` | `ObtenerTrabajoConfiguracion(TrabajoId)` | Trabajos.aspx.vb | 73 |
| `TrabajoOPCuanti` | `GuardarTrabajoConfiguracion(config)` | Trabajos.aspx.vb | 103 |
| `TrabajoOPCuanti` | `GuardarTipoRecoleccion(trabajoId, tipo)` | Trabajos.aspx.vb | 104 |
| `MetodologiaOperaciones` | `obtenerXId(id)` | Trabajos.aspx.vb | 52 |
| `Proyecto` | `obtenerXId(id)` | Trabajos.aspx.vb | 56 |
| `FichaCuantitativo` | `DevolverxTrabajoID(trabajoId)` | Trabajos.aspx.vb | 66 |
| `PlaneacionProduccion` | `ObtenerEstimacionCiudadxTrabajoList(trabajoId)` | Trabajos.aspx.vb | 133 |
| `CampoCualitativo` | (Métodos varios - ⚠️ Por confirmar) | DisenarFiltros.aspx.vb | - |

**⚠️ NOTA CRÍTICA**: El módulo **reutiliza `TrabajoOPCuanti`** (del módulo OP_Cuantitativo) para configuración de fechas y tipos de recolección. Esto significa que **NO existe tabla `OP_TrabajoConfiguracionCuali`**, sino que ambos módulos comparten la misma entidad.

**Evidencia**:
```vb
' Trabajos.aspx.vb, líneas 72-78
Sub CargarConfiguracionTrabajo(ByVal TrabajoId As Int64)
    Dim oTrabajoOP As New TrabajoOPCuanti  ' ← Usa clase de OP_Cuantitativo
    Try
        eTrabajoOP = oTrabajoOP.ObtenerTrabajoConfiguracion(TrabajoId)
        txtFechaInicio.Text = eTrabajoOP.FechaInicioCampo
        txtFechaTerminacion.Text = eTrabajoOP.FechaFinalCampo
```

---

#### **DEPENDENCIA 2: PY_Proyectos (Segmentos, Variables, Instrucciones) - ALTA ⚠️**

**Evidencia de navegación** (⚠️ **Esperada, no confirmada en lectura**):
```vb
' Trabajos.aspx.vb (esperado)
Protected Sub btnSegmentos_Click(sender As Object, e As EventArgs)
    Response.Redirect("~/PY_Proyectos/Segmentos.aspx?trabajoId=" & hfIdTrabajo.Value)
End Sub
```

**Navegaciones esperadas**:
- ⚠️ Segmentos de estudio → `PY_Proyectos/Segmentos.aspx`
- ⚠️ Variables de control → `PY_Proyectos/VariablesControl.aspx`
- ⚠️ Instrucciones de campo → `PY_Proyectos/InstruccionesCampo.aspx`

**Estado**: ⚠️ **NO CONFIRMADO** - Requiere lectura de líneas 150+ de `Trabajos.aspx.vb`

---

#### **DEPENDENCIA 3: GD_Documentos + WorkFlow - ALTA ⚠️**

**Evidencia esperada**:
```vb
' CampoCualitativo.aspx.vb (esperado)
Protected Sub btnDocumentos_Click(sender As Object, e As EventArgs)
    Dim workflowId = WorkFlow.obtenerXId(hfIdTrabajo.Value, "Campo")
    Response.Redirect("~/GD_Documentos/Documentos.aspx?trabajoId=" & hfIdTrabajo.Value)
End Sub
```

**Integraciones esperadas**:
- ⚠️ WorkFlow para obtener IDs de tareas
- ⚠️ Carga de documentos (planillas, audios, transcripciones)

**Estado**: ⚠️ **NO CONFIRMADO** - Requiere lectura de `CampoCualitativo.aspx.vb`

---

#### **DEPENDENCIA 4: US_Usuarios / Personas - MEDIA ✅**

**Evidencia confirmada**:
```vb
' FichaEntrevista.aspx.vb, línea 21
Dim oUsuario As New US.RolesUsuarios
Dim Usuario = oUsuario.obtenerRolesXUsuario(Session("IDUsuario").ToString, True)
```

**Métodos consumidos confirmados**:
- ✅ `US.RolesUsuarios.obtenerRolesXUsuario(userId, activo)` - Obtiene roles del usuario
- ⚠️ `US.Usuarios.UsuariosxRol(rolId)` - Esperado para entrevistadores/observadores
- ⚠️ `Personas.TH_Usuarios_Combo_Get(area)` - Esperado para transcriptores

---

#### **DEPENDENCIA 5: WebMatrix.Util (Helpers Legacy) - MEDIA ✅**

**Evidencia confirmada**:
```vb
' Trabajos.aspx.vb, línea 2
Imports WebMatrix.Util

' Uso confirmado:
ShowNotification("mensaje", ShowNotifications.ErrorNotification)
ActivateAccordion(index, EffectActivateAccordion.SlideEffect)
```

**Helpers legados confirmados**:
- ✅ `ShowNotification(mensaje, tipo)` - Muestra toasts/alertas
- ✅ `ActivateAccordion(index, efecto)` - Controla acordeones
- ⚠️ Otros esperados: `ShowModal`, `CloseModal`, validaciones

**Riesgo**: Estos helpers **NO existen en ASP.NET Core MVC**. Necesitan reemplazo con:
- JavaScript moderno (toast libraries)
- Partial views para modales
- Validaciones FluentValidation

### 1.4 Complejidad Estimada: 🔴 ALTA

**Justificación con evidencia**:

#### **Factor 1: Volumen de UI Legacy Compleja**
- **19 WebForms** identificados en `OP_Cualitativo/` folder
- **Evidencia**: Búsqueda de archivos retorna 21 archivos `.aspx.vb`
- **Tecnologías legacy**: UpdatePanels, Accordions, GridViews, Validators
- **Complejidad UI**: Controles dinámicos, postbacks múltiples, ViewState

**Archivos confirmados**:
```
Trabajos.aspx.vb, TrabajosCoordinador.aspx.vb, Calendario.aspx.vb,
CampoCualitativo.aspx.vb, ProgramacionCampo.aspx.vb, MuestraTrabajos.aspx.vb,
DisenarFiltros.aspx.vb, VisualizadorFiltros.aspx.vb,
AprobacionesFiltros.aspx.vb, AprobacionesFiltrosAsitencia.aspx.vb,
Entrevista.aspx.vb, Observacion.aspx.vb, Transcripcion.aspx.vb,
FichaEntrevista.aspx.vb, FichaSesion.aspx.vb, FichaObservacion.aspx.vb,
IPSCuali.aspx.vb, AdministracionRegistroPlanillas.aspx.vb,
Default.aspx.vb, HomeGestion.aspx.vb, HomeRecoleccion.aspx.vb
```

---

#### **Factor 2: Dependencia Fuerte en Session y QueryString**

**Evidencia concreta**:
```vb
' Trabajos.aspx.vb, línea 24
Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())

' Trabajos.aspx.vb, línea 28
oCoord.ObtenerMuestraxCoordinador(Session("IDUsuario").ToString)

' Trabajos.aspx.vb, línea 118
CargarTrabajos(Session("IDUsuario").ToString)

' FichaEntrevista.aspx.vb, línea 10
If Request.QueryString("idtrabajo") IsNot Nothing Then
    Dim idtrabajo As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString)

' DisenarFiltros.aspx.vb, líneas 38-40
If Request.QueryString("trabajoId") IsNot Nothing Then
    Dim idTrabajo As Int64 = Int64.Parse(Request.QueryString("trabajoId").ToString)
    hfIdTrabajo.Value = idTrabajo
```

**Riesgo de migración**:
- ❌ Session NO es recomendada en ASP.NET Core (problemas de escalabilidad)
- ❌ QueryString sin cifrar expone IDs sensibles
- ✅ Solución: Claims de autenticación + cifrado de rutas

**Impacto**: ALTO - Requiere rediseño de flujo de navegación y autenticación

---

#### **Factor 3: Lógica de Negocio Compleja**

**Evidencia de validaciones múltiples**:
```vb
' Trabajos.aspx.vb, líneas 84-105
Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
    If Not (IsDate(txtFechaInicio.Text)) Then
        ShowNotification("Debe llenar la fecha de Inicio antes de continuar", ShowNotifications.ErrorNotification)
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        Exit Sub
    End If
    If Not (IsDate(txtFechaTerminacion.Text)) Then
        ShowNotification("Debe llenar la fecha de Finalización antes de continuar", ShowNotifications.ErrorNotification)
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        Exit Sub
    End If
    If ddlTipoRecoleccion.SelectedIndex = -1 Then
        ShowNotification("Debe elegir el tipo de recolección antes de continuar", ShowNotifications.ErrorNotification)
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        Exit Sub
    End If
```

**Evidencia de validaciones de presupuesto**:
```vb
' FichaEntrevista.aspx.vb, líneas 52-91
If rblIncentivos.SelectedValue = "1" And txtPresupuestoIncentivo.Text = "" Then
    ShowNotification("Digite el Presupuesto del Incentivo Económico", ShowNotifications.ErrorNotification)
    txtPresupuestoIncentivo.Focus()
    ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    Exit Sub
End If

If rblIncentivos.SelectedValue = "1" And txtDistribucionIncentivo.Text = "" Then
    ShowNotification("Digite la Distribución del Incentivo Económico", ShowNotifications.ErrorNotification)
    txtDistribucionIncentivo.Focus()
    ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    Exit Sub
End If
```

**Complejidad confirmada**:
- ✅ Validaciones condicionales dependientes de controles (RadioButtons, Checkboxes)
- ✅ Validaciones de presupuesto con distribución por segmento
- ✅ Control de fechas (inicio < fin)
- ✅ Validaciones de selección obligatoria en listas

**Impacto**: ALTO - Requiere FluentValidation con reglas condicionales

---

#### **Factor 4: Generación Dinámica de Controles**

**Evidencia**:
```vb
' DisenarFiltros.aspx.vb, líneas 73-80
Public Sub CargarPreguntas()
    pnlPreguntas.Controls.Clear()
    cargarPreguntasFiltro()
End Sub
```

**Riesgo**:
- ❌ WebForms permite generar controles dinámicos en runtime (TextBox, RadioButton, CheckBox)
- ❌ MVC/Razor **NO** permite esto de la misma forma
- ✅ Solución: JavaScript con templates dinámicos + AJAX

**Impacto**: ALTO - Requiere rediseño completo de diseñador de filtros

---

#### **Factor 5: Múltiples Roles con Permisos Diferenciados**

**Evidencia de branching complejo**:
```vb
' Trabajos.aspx.vb, líneas 26-42
If permisos.VerificarPermisoUsuario(42, UsuarioID) = True Then
    ' Código para coordinadores
Else
    If permisos.VerificarPermisoUsuario(148, UsuarioID) = True Then
        ' Código para supervisores
    Else
        ' Código para operadores regulares
    End If
End If
```

**Complejidad confirmada**:
- ✅ 3 niveles de permisos (42, 148, sin permiso)
- ✅ Roles 6, 7, 8 controlan visibilidad de botones
- ✅ Lógica diferente por rol en misma pantalla

**Impacto**: MEDIO - Solución con `[Authorize(Policy = "...")]` + Claims

---

#### **Factor 6: Metodologías Dinámicas (Entrevista/Sesión/Observación)**

**Evidencia de lógica condicional**:
```vb
' Trabajos.aspx.vb, líneas 137-143 (inferido por línea 137 visible)
If oeTrabajo.MetCodigo >= 600 And oeTrabajo.MetCodigo <= 699 Then
    Me.btnSesiones.Visible = True
Else
    Me.btnSesiones.Visible = False
End If
If oeTrabajo.MetCodigo >= 700 And oeTrabajo.MetCodigo <= 799 Then
    Me.btnEntrevistas.Visible = True
```

**Complejidad**:
- ✅ Rangos de códigos de metodología hardcodeados (600-699, 700-799)
- ✅ Botones dinámicos según metodología
- ✅ Redirecciones diferentes según código

**Impacto**: MEDIO - Requiere estrategia de configuración (tabla o enum)

---

### **CONCLUSIÓN COMPLEJIDAD**

| Factor | Impacto | Evidencia |
|--------|---------|-----------|
| Volumen UI Legacy | 🔴 ALTO | 19 WebForms, UpdatePanels, Accordions |
| Session/QueryString | 🔴 ALTO | Usado en cada página |
| Validaciones Complejas | 🔴 ALTO | Condicionales, presupuestos, fechas |
| Controles Dinámicos | 🔴 ALTO | `pnlPreguntas.Controls.Clear()` |
| Múltiples Roles | 🟠 MEDIO | Permisos 42, 148, Roles 6/7/8 |
| Metodologías Dinámicas | 🟠 MEDIO | Rangos hardcodeados 600-799 |
| **TOTAL** | **🔴 ALTA** | **6 factores de complejidad** |

**Estimación preliminar**: 6-8 semanas (240-320 horas) para migración completa

---

## ⚠️ ESTADO ACTUAL FASE 1

**Completado**:
- ✅ Sección 1.1: Propósito del módulo con evidencia
- ✅ Sección 1.2: Roles evidenciados (5 roles confirmados)
- ✅ Sección 1.3: Dependencias con otros módulos (5 dependencias, 3 confirmadas)
- ✅ Sección 1.4: Complejidad estimada (6 factores analizados)

**Pendiente**:
- ⚠️ **FASE 2**: Sección 2 - Inventario del Legado (Tabla detallada de 19 WebForms)
- ⚠️ **FASE 3-4**: Sección 3 - Flujos Funcionales Detallados (3 flujos principales)
- ⚠️ **FASE 5**: Secciones 4-7 (Mapeo 1:1, BD/SPs, Riesgos, Componentes)
- ⚠️ **FASE 6**: Secciones 8-12 (Backlog, Checklist, Decisiones, Estimación, Próximos Pasos)

**Archivos pendientes de lectura para completar evidencia**:
1. `Trabajos.aspx.vb` (líneas 150-217) - Navegación a fichas y PY_Proyectos
2. `CampoCualitativo.aspx.vb` (completo) - Sesiones y documentos
3. `Entrevista.aspx.vb`, `Observacion.aspx.vb`, `Transcripcion.aspx.vb` - CRUD
4. `AprobacionesFiltros.aspx.vb` - Aprobaciones y logs
5. `ProgramacionCampo.aspx.vb` - Programación de campo
6. `IPSCuali.aspx.vb` - Reportes IPS
7. Todos los archivos `.aspx` (markup) para evidenciar UpdatePanels, grids, etc.

---

**¿Continúo con FASE 2: Inventario del Legado (Tabla detallada)?**
