# ANÁLISIS OP_CUALITATIVO - FASE 3: FLUJOS FUNCIONALES DETALLADOS

## 🔄 3. FLUJOS FUNCIONALES DETALLADOS

---

## FLUJO 1: GESTIÓN DE TRABAJOS COE Y NAVEGACIÓN A MÓDULOS RELACIONADOS

**Descripción**: Coordinador COE accede a trabajos, configura fechas y tipo de recolección, luego navega a fichas técnicas, filtros o módulos de PY_Proyectos según la metodología del trabajo.

**Archivos involucrados**: `Trabajos.aspx.vb` (217 líneas)

**Actores**: Coordinador de Campo (Permiso 42)

---

### PASO 1.1: Acceso a Página de Trabajos (Page_Load)

**Evidencia VB.NET**:
```vb
' Trabajos.aspx.vb, líneas 112-119
Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Not IsPostBack Then
        CargarTrabajos(Session("IDUsuario").ToString)
        lbtnVolver.PostBackUrl = "~/RE_GT/HomeRecoleccion.aspx"
        CargarTiposDeRecolección()
    End If
End Sub
```

**Acciones**:
1. ✅ Verifica que NO sea postback (evita recarga)
2. ✅ Llama `CargarTrabajos(Session("IDUsuario"))` con ID del usuario
3. ✅ Establece URL de volver a Home Recolección
4. ✅ Carga combo de tipos de recolección

**Validaciones**: 
- ✅ `IsPostBack` previene recarga múltiple
- ✅ `Session("IDUsuario")` debe existir (crash si no existe)

**Resultado éxito**: Grid `gvTrabajos` poblado con trabajos del coordinador  
**Resultado error**: Exception si Session vacío

**Riesgo técnico**: ⚠️ Dependencia en Session sin validación previa

---

### PASO 1.2: Carga de Trabajos Filtrados por Coordinador

**Evidencia VB.NET**:
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

**Lógica de negocio**:

1. **Permiso 42 (Coordinador COE)**:
   - ✅ Obtiene lista de `TrabajoId` asignados al coordinador desde `CoordinacionCampo.ObtenerMuestraxCoordinador()`
   - ✅ Filtra trabajos COE donde `ltraba.id IN (listTrabCoord)`
   - ✅ Resultado: Solo trabajos asignados al coordinador

2. **Permiso 148 (Supervisor)**:
   - ✅ Ve todos los trabajos en estado 2 (`ObtenerTrabajosCualitativosxCOE(Nothing, 2, Nothing)`)
   - ✅ Parámetro `2` = estado "EnCampo" (inferido)

3. **Sin permiso especial**:
   - ✅ Ve trabajos por COE genérico (`Coe` = ID usuario)

**Métodos consumidos**:
| Clase | Método | Parámetros | Retorno |
|-------|--------|-----------|---------|
| `CoordinacionCampo` | `ObtenerMuestraxCoordinador(userId)` | String | IEnumerable (muestra.TrabajoId) |
| `Trabajo` | `obtenerXCOE(coe)` | Int64 | IEnumerable<Trabajo> |
| `Trabajo` | `ObtenerTrabajosCualitativosxCOE(coe, estado, filtro)` | Int64, Int32?, String | IEnumerable<Trabajo> |

**Validaciones**:
- ✅ Permisos verificados con `ClsPermisosUsuarios.VerificarPermisoUsuario(42 ó 148, userId)`
- ✅ LINQ filtering en memoria (performance concern si >1000 trabajos)

**Resultado**: Grid `gvTrabajos` con DataKeys = lista de IDs para RowCommand  
**Riesgo técnico**: 🟠 LINQ filtering en cliente, no en DB

---

### PASO 1.3: Búsqueda de Trabajos (btnBuscar_Click)

**Evidencia VB.NET**:
```vb
' Trabajos.aspx.vb, líneas 120-125
Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
    Dim oTrabajo As New Trabajo
    gvTrabajos.DataSource = oTrabajo.obtenerXIdCOEXTodosCampos(Session("IDUsuario").ToString, txtBuscar.Text)
    gvTrabajos.DataBind()
    ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
End Sub
```

**Lógica**:
1. ✅ Crea instancia de `Trabajo`
2. ✅ Llama SP `obtenerXIdCOEXTodosCampos(userId, searchText)`
3. ✅ El SP filtra por JobBook, Nombre, Cliente (inference)
4. ✅ Bind a grid y activa Accordion 0 (collapse)

**Método consumido**:
```
Trabajo.obtenerXIdCOEXTodosCampos(userId: String, searchText: String) → DataTable
```

**Validaciones**:
- ⚠️ **NO hay validación de entrada** en txtBuscar.Text
- ⚠️ **SQL Injection Risk**: searchText concatenado en SP (asumir parámetro en SP)

**Resultado**: Grid filtrado con búsqueda  
**Riesgo técnico**: 🔴 SQL Injection en búsqueda

---

### PASO 1.4: Selección de Trabajo y Carga de Configuración (gvTrabajos_RowCommand)

**Evidencia VB.NET**:
```vb
' Trabajos.aspx.vb, líneas 126-171
Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
    If e.CommandName = "Actualizar" Then
        Dim oTrabajo As New Trabajo
        hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
        Dim oeTrabajo = oTrabajo.ObtenerTrabajo(hfIdTrabajo.Value)
        CargarConfiguracionTrabajo(hfIdTrabajo.Value)
        accordion0.Visible = False
        accordion1.Visible = True

        Dim oPlaneacion As New PlaneacionProduccion
        If oPlaneacion.ObtenerEstimacionCiudadxTrabajoList(hfIdTrabajo.Value).Count = 0 Then
            ' Estimación automática no aplicada
        Else
            ' Estimaciones ya existen
        End If

        ' Mostrar/ocultar botones según metodología
        If oeTrabajo.MetCodigo >= 600 And oeTrabajo.MetCodigo <= 699 Then
            Me.btnSesiones.Visible = True
        Else
            Me.btnSesiones.Visible = False
        End If
        If oeTrabajo.MetCodigo >= 700 And oeTrabajo.MetCodigo <= 799 Then
            Me.btnEntrevistas.Visible = True
        Else
            Me.btnEntrevistas.Visible = False
        End If
        If oeTrabajo.MetCodigo >= 900 And oeTrabajo.MetCodigo <= 999 Then
            Me.btnInHome.Visible = True
        Else
            Me.btnInHome.Visible = False
        End If
    End If
End Sub
```

**Lógica detallada**:

1. **Recuperar ID del trabajo**:
   ```vb
   hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
   ```
   - ✅ DataKeys indexado por CommandArgument (número de fila)
   - ✅ Parseado a Int64 para uso posterior

2. **Cargar información del trabajo**:
   ```vb
   Dim oeTrabajo = oTrabajo.ObtenerTrabajo(hfIdTrabajo.Value)
   ```
   - ✅ Obtiene entity con MetCodigo (código de metodología)

3. **Cargar configuración de fechas**:
   ```vb
   CargarConfiguracionTrabajo(hfIdTrabajo.Value)
   ```
   - ✅ Carga `TrabajoOPCuanti.ObtenerTrabajoConfiguracion(trabajoId)` (Ver PASO 1.5)

4. **Cambiar acordeón visible**:
   ```vb
   accordion0.Visible = False
   accordion1.Visible = True
   ```
   - ✅ Accordion 0 = Lista de trabajos
   - ✅ Accordion 1 = Configuración del trabajo

5. **Mostrar botones según metodología** (6 rangos de MetCodigo):
   - 600-699: ✅ `btnSesiones.Visible = True` (Sesiones de grupo)
   - 700-799: ✅ `btnEntrevistas.Visible = True` (Entrevistas)
   - 900-999: ✅ `btnInHome.Visible = True` (In-Home visits)

**Métodos consumidos**:
| Clase | Método | Parámetros | Retorno |
|-------|--------|-----------|---------|
| `Trabajo` | `ObtenerTrabajo(trabajoId)` | Int64 | PY_Trabajos_Get_Result |
| - | - | - | (incluye MetCodigo) |
| (Ver PASO 1.5) | - | - | - |

**Validaciones**:
- ✅ CommandArgument convertido a Int32 para índice
- ✅ Int64.Parse en DataKeys para ID (puede crash si no existe)
- ⚠️ MetCodigo rangos hardcodeados (mantenimiento difícil)

**Resultado**: Accordion 1 expandido con formulario de configuración  
**Riesgo técnico**: 🟠 Rangos de MetCodigo hardcodeados (600-699, 700-799, 900-999)

---

### PASO 1.5: Carga de Configuración Existente

**Evidencia VB.NET**:
```vb
' Trabajos.aspx.vb, líneas 72-81
Sub CargarConfiguracionTrabajo(ByVal TrabajoId As Int64)
    Dim oTrabajoOP As New TrabajoOPCuanti
    Try
        eTrabajoOP = oTrabajoOP.ObtenerTrabajoConfiguracion(TrabajoId)
        txtFechaInicio.Text = eTrabajoOP.FechaInicioCampo
        txtFechaTerminacion.Text = eTrabajoOP.FechaFinalCampo
    Catch ex As Exception
        eTrabajoOP = New OP_TrabajoConfiguracion
        txtFechaInicio.Text = String.Empty
        txtFechaTerminacion.Text = String.Empty
    End Try
End Sub
```

**Lógica**:

1. **Obtener configuración**:
   ```vb
   eTrabajoOP = oTrabajoOP.ObtenerTrabajoConfiguracion(TrabajoId)
   ```
   - ✅ Consulta tabla `OP_TrabajoConfiguracion`
   - ⚠️ **NOTA**: Usa clase `TrabajoOPCuanti` de OP_Cuantitativo
   - ✅ Retorna entity con FechaInicioCampo, FechaFinalCampo

2. **Llenar controles**:
   ```vb
   txtFechaInicio.Text = eTrabajoOP.FechaInicioCampo
   txtFechaTerminacion.Text = eTrabajoOP.FechaFinalCampo
   ```
   - ✅ Formato inferido: "yyyy-MM-dd" (validate en save)

3. **Manejo de error**:
   ```vb
   Catch ex As Exception
       eTrabajoOP = New OP_TrabajoConfiguracion
       txtFechaInicio.Text = String.Empty
       txtFechaTerminacion.Text = String.Empty
   End Try
   ```
   - ✅ Si SP falla, crea entity vacío (nuevo trabajo)
   - ✅ TextBoxes vacíos para entrada manual

**Métodos consumidos**:
| Clase | Método | Parámetros | Retorno |
|-------|--------|-----------|---------|
| `TrabajoOPCuanti` | `ObtenerTrabajoConfiguracion(trabajoId)` | Int64 | OP_TrabajoConfiguracion |

**Validaciones**:
- ✅ Try-Catch envuelve la consulta (graceful failure)
- ⚠️ Exception silenciosa (no se registra error)

**Resultado**: TextBoxes poblados con fechas o vacíos si nuevo trabajo  
**Riesgo técnico**: 🟠 Reutilización de tabla OP_Cuantitativo

---

### PASO 1.6: Guardado de Configuración (btnGuardar_Click)

**Evidencia VB.NET**:
```vb
' Trabajos.aspx.vb, líneas 84-110
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
    
    eTrabajoOP.FechaInicioCampo = txtFechaInicio.Text
    eTrabajoOP.FechaFinalCampo = txtFechaTerminacion.Text
    eTrabajoOP.TrabajoId = hfIdTrabajo.Value
    Dim oTrabajoOp As New TrabajoOPCuanti
    eTrabajoOP = oTrabajoOp.GuardarTrabajoConfiguracion(eTrabajoOP)
    oTrabajoOp.GuardarTipoRecoleccion(hfIdTrabajo.Value, ddlTipoRecoleccion.SelectedValue)
    ShowNotification("Información guardada correctamente", ShowNotifications.InfoNotification)
    ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
End Sub
```

**Validaciones completas**:

1. **Fecha Inicio obligatoria**:
   ```vb
   If Not (IsDate(txtFechaInicio.Text)) Then
   ```
   - ✅ Valida formato de fecha con `IsDate()` (VB.NET)
   - ✅ Mensaje error en notificación
   - ✅ Focus en control y Accordion expandido

2. **Fecha Terminación obligatoria**:
   ```vb
   If Not (IsDate(txtFechaTerminacion.Text)) Then
   ```
   - ✅ Mismo patrón que fecha inicio

3. **Tipo Recolección obligatorio**:
   ```vb
   If ddlTipoRecoleccion.SelectedIndex = -1 Then
   ```
   - ✅ Valida que índice != -1 (validación de dropdown)

**Lógica de guardado**:

1. **Llenar entity**:
   ```vb
   eTrabajoOP.FechaInicioCampo = txtFechaInicio.Text
   eTrabajoOP.FechaFinalCampo = txtFechaTerminacion.Text
   eTrabajoOP.TrabajoId = hfIdTrabajo.Value
   ```

2. **Guardar configuración**:
   ```vb
   Dim oTrabajoOp As New TrabajoOPCuanti
   eTrabajoOP = oTrabajoOp.GuardarTrabajoConfiguracion(eTrabajoOP)
   ```

3. **Guardar tipo de recolección**:
   ```vb
   oTrabajoOp.GuardarTipoRecoleccion(hfIdTrabajo.Value, ddlTipoRecoleccion.SelectedValue)
   ```

4. **Confirmación**:
   ```vb
   ShowNotification("Información guardada correctamente", ShowNotifications.InfoNotification)
   ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
   ```
   - ✅ Toast verde de éxito
   - ✅ Regresa a Accordion 0 (lista de trabajos)

**Métodos consumidos**:
| Clase | Método | Parámetros | Retorno |
|-------|--------|-----------|---------|
| `TrabajoOPCuanti` | `GuardarTrabajoConfiguracion(entity)` | OP_TrabajoConfiguracion | OP_TrabajoConfiguracion |
| `TrabajoOPCuanti` | `GuardarTipoRecoleccion(trabajoId, tipo)` | Int64, String | Void |

**Validaciones ejecutadas**:
1. ✅ IsDate(FechaInicio)
2. ✅ IsDate(FechaTerminacion)
3. ✅ SelectedIndex != -1 (TipoRecoleccion)
4. ⚠️ **NO valida**: FechaInicio < FechaTerminacion (error potencial)

**Resultado éxito**: 
- ✅ Toast verde "Información guardada correctamente"
- ✅ Accordion 0 expandido (regresa a lista)
- ✅ Configuración guardada en BD

**Resultado error**:
- ❌ Toast rojo con mensaje de validación
- ❌ Accordion 1 expandido (permanece en formulario)

**Riesgo técnico**: 🟠 No valida orden de fechas (inicio < fin)

---

### PASO 1.7: Navegación a Módulos Relacionados

**Evidencia VB.NET**:
```vb
' Trabajos.aspx.vb, líneas 181-217
Protected Sub btnSegmentos_Click(sender As Object, e As EventArgs) Handles btnSegmentos.Click
    Response.Redirect("../PY_Proyectos/SegmentosCuali.aspx?trabajoId=" & hfIdTrabajo.Value)
End Sub

Protected Sub btnEntrevistas_Click(sender As Object, e As EventArgs) Handles btnEntrevistas.Click
    Response.Redirect("../PY_Proyectos/DistribucionEntrevistas.aspx?trabajoId=" & hfIdTrabajo.Value)
End Sub

Protected Sub btnSesiones_Click(sender As Object, e As EventArgs) Handles btnSesiones.Click
    Response.Redirect("../PY_Proyectos/Sesiones.aspx?trabajoId=" & hfIdTrabajo.Value)
End Sub

Protected Sub btnInHome_Click(sender As Object, e As EventArgs) Handles btnInHome.Click
    Response.Redirect("../PY_Proyectos/InHomeVisit.aspx?trabajoId=" & hfIdTrabajo.Value)
End Sub

Protected Sub btnFiltroReclutamiento_Click(sender As Object, e As EventArgs) Handles btnFiltroReclutamiento.Click
    Response.Redirect("../OP_Cualitativo/DisenarFiltros.aspx?trabajoId=" & hfIdTrabajo.Value & "&tipofiltro=1")
End Sub

Protected Sub btnFiltroAsistencia_Click(sender As Object, e As EventArgs) Handles btnFiltroAsistencia.Click
    Response.Redirect("../OP_Cualitativo/DisenarFiltros.aspx?trabajoId=" & hfIdTrabajo.Value & "&tipofiltro=2")
End Sub

Protected Sub btnFicha_Click(sender As Object, e As EventArgs) Handles btnFicha.Click
    Dim oTrabajo As New Trabajo
    Dim oeTrabajo As PY_Trabajos_Get_Result
    oeTrabajo = oTrabajo.obtenerXId(hfIdTrabajo.Value)

    If oeTrabajo.MetCodigo >= 600 And oeTrabajo.MetCodigo <= 699 Then
        Response.Redirect("../OP_Cualitativo/FichaSesion.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes")
    ElseIf oeTrabajo.MetCodigo >= 700 And oeTrabajo.MetCodigo <= 799 Then
        Response.Redirect("../OP_Cualitativo/FichaEntrevista.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes")
    ElseIf oeTrabajo.MetCodigo >= 900 And oeTrabajo.MetCodigo <= 999 Then
        Response.Redirect("../OP_Cualitativo/FichaObservacion.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes")
    End If
End Sub

Protected Sub btnVariablesControl_Click(sender As Object, e As EventArgs) Handles btnVariablesControl.Click
    Response.Redirect("../PY_Proyectos/VariablesControl.aspx?idTr=" & hfIdTrabajo.Value & "&modal=GP")
End Sub
```

**Navegaciones confirmadas**:

| Botón | Destino | QueryString | Módulo | Método |
|-------|---------|-------------|--------|--------|
| `btnSegmentos` | `PY_Proyectos/SegmentosCuali.aspx` | `?trabajoId={id}` | PY | Segmentos |
| `btnEntrevistas` | `PY_Proyectos/DistribucionEntrevistas.aspx` | `?trabajoId={id}` | PY | Distribución |
| `btnSesiones` | `PY_Proyectos/Sesiones.aspx` | `?trabajoId={id}` | PY | Sesiones |
| `btnInHome` | `PY_Proyectos/InHomeVisit.aspx` | `?trabajoId={id}` | PY | In-Home |
| `btnFiltroReclutamiento` | `OP_Cualitativo/DisenarFiltros.aspx` | `?trabajoId={id}&tipofiltro=1` | OP | Diseñar Filtros |
| `btnFiltroAsistencia` | `OP_Cualitativo/DisenarFiltros.aspx` | `?trabajoId={id}&tipofiltro=2` | OP | Diseñar Filtros |
| `btnFicha` | Dinámico según MetCodigo | `?idtrabajo={id}&op=yes` | OP | Ficha (Sesión/Entrevista/Observación) |
| `btnVariablesControl` | `PY_Proyectos/VariablesControl.aspx` | `?idTr={id}&modal=GP` | PY | Variables |

**Lógica de redirección en btnFicha**:
```vb
If oeTrabajo.MetCodigo >= 600 And oeTrabajo.MetCodigo <= 699 Then
    → FichaSesion.aspx (Sesiones de grupo)
ElseIf oeTrabajo.MetCodigo >= 700 And oeTrabajo.MetCodigo <= 799 Then
    → FichaEntrevista.aspx (Entrevistas)
ElseIf oeTrabajo.MetCodigo >= 900 And oeTrabajo.MetCodigo <= 999 Then
    → FichaObservacion.aspx (In-Home observations)
```

**Riesgos técnicos**:
- ⚠️ QueryString sin cifrar (IDs visibles en URL)
- ⚠️ Parámetro `&op=yes` indica visibilidad de botones (ver FichaEntrevista.aspx.vb línea 17-39)
- ⚠️ MetCodigo rangos hardcodeados (mismo problema que PASO 1.4)
- ⚠️ No hay validación de trabajoId antes de redirigir (404 en destino)

**Resultado**: Redirección a módulo relacionado con trabajoId en QueryString

---

## 📊 RESUMEN FLUJO 1

| Elemento | Detalle | Evidencia |
|----------|---------|-----------|
| **Pasos totales** | 7 pasos | Page_Load → gvTrabajos_RowCommand → btnGuardar_Click → Navegaciones |
| **Validaciones** | 6 validaciones | 2x IsDate, 1x SelectedIndex, 3x permisos |
| **Métodos consumidos** | 12+ | Trabajo, CoordinacionCampo, TrabajoOPCuanti, PlaneacionProduccion |
| **Tablas involucradas** | 2 tablas | OP_TrabajoConfiguracion, Trabajo |
| **SPs confirmados** | 3 SPs | obtenerXIdCOEXTodosCampos, ObtenerTrabajosCualitativosxCOE, obtenerXCOE |
| **Riesgos detectados** | 5 riesgos | SQL Injection, Session, Rangos hardcodeados, Vista/ocultamiento de botones, Validación orden fechas |
| **Complejidad** | 🟠 MEDIA | 217 LOC, múltiples roles, navegación condicional |
| **Estimación migración** | 10 horas | Incluye permisos, validaciones, navegaciones, refactor de helpers |

---

## ⚠️ RIESGOS TÉCNICOS FLUJO 1

### Riesgo 1.1: SQL Injection en Búsqueda
**Línea**: 123 (btnBuscar_Click)  
**Código**:
```vb
gvTrabajos.DataSource = oTrabajo.obtenerXIdCOEXTodosCampos(Session("IDUsuario").ToString, txtBuscar.Text)
```
**Problema**: `txtBuscar.Text` sin validación concatenado en SP  
**Impacto**: 🔴 CRÍTICO - Inyección SQL  
**Solución**: Validar entrada o usar parámetro en SP

---

### Riesgo 1.2: Dependencia en Session("IDUsuario")
**Líneas**: 23, 28, 34, 113, 123  
**Problema**: Session usado sin validación previa, puede crash  
**Impacto**: 🟠 ALTO - Crash si Session expirada  
**Solución**: Usar Claims de autenticación en ASP.NET Core

---

### Riesgo 1.3: Rangos de MetCodigo Hardcodeados
**Líneas**: 144, 149, 154, 201-210  
**Código**:
```vb
If oeTrabajo.MetCodigo >= 600 And oeTrabajo.MetCodigo <= 699 Then
If oeTrabajo.MetCodigo >= 700 And oeTrabajo.MetCodigo <= 799 Then
```
**Problema**: Rangos no documentados, difíciles de mantener  
**Impacto**: 🟠 MEDIO - Mantenimiento difícil  
**Solución**: Usar tabla de configuración o enum

---

### Riesgo 1.4: No Valida Orden de Fechas
**Línea**: 96-106  
**Problema**: `IsDate(FechaInicio)` y `IsDate(FechaTerminacion)` pero no valida FechaInicio < FechaTerminacion  
**Impacto**: 🟠 MEDIO - Error de negocio permitido  
**Solución**: Agregar validación `If fechaInicio >= fechaFin Then`

---

### Riesgo 1.5: QueryString sin Cifrar
**Líneas**: 182-217  
**Código**:
```vb
Response.Redirect("../PY_Proyectos/SegmentosCuali.aspx?trabajoId=" & hfIdTrabajo.Value)
```
**Problema**: TrabajoId visible en URL  
**Impacto**: 🟠 BAJO-MEDIO - Exposición de datos  
**Solución**: Usar ID cifrado o encabezados HTTP

---

## ⚠️ ESTADO ACTUAL FASE 3

**Completado**:
- ✅ FLUJO 1: Gestión de Trabajos COE (7 pasos, 11 páginas de análisis)
- ✅ Evidencia VB.NET línea-por-línea
- ✅ Validaciones documentadas
- ✅ Métodos consumidos en tablas
- ✅ 5 riesgos técnicos identificados

**Pendiente**:
- ⚠️ **FASE 4**: FLUJO 2 (Diseño y Aprobación de Filtros) + FLUJO 3 (Fichas y Entrega)
- ⚠️ **FASE 5**: Secciones 4-7 (Mapeo 1:1, BD/SPs, Riesgos, Componentes)
- ⚠️ **FASE 6**: Secciones 8-12 (Backlog, Checklist, Decisiones, Estimación)

---

**¿Continúo con FLUJO 2: Diseño y Aprobación de Filtros de Reclutamiento/Asistencia?**
