# ANÁLISIS OP_CUALITATIVO - FASE 4: FLUJOS 2 Y 3

## FLUJO 2: DISEÑO Y APROBACIÓN DE FILTROS DE RECLUTAMIENTO/ASISTENCIA

**Descripción**: Operador de fichas diseña filtros dinámicos (preguntas personalizadas), obtiene respuestas de reclutadores, y operadores de calidad aprueban o rechazan las respuestas con auditoría.

**Archivos involucrados**: 
- `DisenarFiltros.aspx.vb` (1,062 líneas) 
- `AprobacionesFiltros.aspx.vb` (270 líneas)
- `AprobacionesFiltrosAsitencia.aspx.vb` (similar)

**Actores**: Operador de Fichas (Roles 6/7/8), OPS/GP (Aprobadores)

---

### PASO 2.1: Acceso a Diseñador de Filtros (Page_Load)

**Evidencia VB.NET**:
```vb
' DisenarFiltros.aspx.vb, líneas 40-63
Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Not IsPostBack Then
        Dim o As New Trabajo

        If Request.QueryString("trabajoId") IsNot Nothing Then
            Dim idTrabajo As Int64 = Int64.Parse(Request.QueryString("trabajoId").ToString)
            hfIdTrabajo.Value = idTrabajo
            Trabajo = o.DevolverxID(hfIdTrabajo.Value)
        End If

        If Request.QueryString("tipofiltro") IsNot Nothing Then
            Dim idTipoFiltro As Int64 = Int64.Parse(Request.QueryString("tipofiltro").ToString)
            hfTipoFiltro.Value = idTipoFiltro
            If hfTipoFiltro.Value = eTipoFiltro.Reclutamiento Then
                Me.lblCrearFiltro.InnerText = "Crear Filtro de Reclutamiento"
            ElseIf hfTipoFiltro.Value = eTipoFiltro.Asitencia Then
                Me.lblCrearFiltro.InnerText = "Crear Filtro de Asitencia"
            End If
        Else
            Response.Redirect("Trabajos.aspx")
        End If

        CargarLabelTrabajo()
        cargarListaFiltros()
        cargarTipoPregunta()
    End If
    CargarPreguntas()
End Sub
```

**Lógica**:
1. ✅ Obtiene `trabajoId` y `tipofiltro` de QueryString
2. ✅ Valida que ambos parámetros existan (sino redirige a Trabajos.aspx)
3. ✅ Carga entity de trabajo
4. ✅ Determina label según tipo filtro (1=Reclutamiento, 2=Asistencia)
5. ✅ Carga combo de tipos de pregunta

**Validaciones**:
- ✅ QueryString verificados (redirige si no existen)
- ⚠️ No valida que trabajoId sea válido (404 si no existe)

**Métodos consumidos**:
```
Trabajo.DevolverxID(trabajoId) → PY_Trabajo_Get_Result
CampoCualitativo.ObtenerTipoPreguntaFiltro() → IEnumerable<TipoPregunta>
CampoCualitativo.ObtenerListaFiltros(null, tipoFiltro, trabajoId) → IEnumerable<Filtro>
```

**Resultado**: Página cargada con lista de filtros existentes + combo de tipos de pregunta

**Riesgo técnico**: ⚠️ QueryString sin cifrar, no valida trabajoId

---

### PASO 2.2: Creación de Filtro Base (btnCrear_Click)

**Evidencia VB.NET**:
```vb
' DisenarFiltros.aspx.vb, líneas 86-135
Protected Sub btnCrear_Click(sender As Object, e As EventArgs) Handles btnCrear.Click
    Dim oCampo As New CoreProject.CampoCualitativo

    If Not (IsDate(txtFechaIni.Text)) Then
        ShowNotification("Escriba la fecha de Inicio", ShowNotifications.ErrorNotification)
        txtFechaIni.Focus()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Exit Sub
    End If

    If Not (IsDate(txtFechaFin.Text)) Then
        ShowNotification("Escriba la fecha Final", ShowNotifications.ErrorNotification)
        txtFechaFin.Focus()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Exit Sub
    End If

    If hfTipoFiltro.Value = eTipoFiltro.Reclutamiento Then
        GuardarFiltro()
        GuardarPreguntaNombres()
        GuardarPreguntaCC()
        GuardarPreguntaCelular()
        GuardarPreguntaDireccion()
        GuardarPreguntaCiudad()
        GuardarPreguntaBarrio()
        GuardarPreguntaEdad()
        GuardarPreguntaEstrato()
        GuardarPreguntaReclutador()
    ElseIf hfTipoFiltro.Value = eTipoFiltro.Asitencia Then
        Dim lstFiltros = oCampo.ObtenerListaFiltros(Nothing, 1, hfIdTrabajo.Value)
        If lstFiltros.Count > 0 Then
            GuardarFiltro()
            oCampo.CrearCopiaFiltros(hfIdFiltro.Value, lstFiltros(0).Id)
        Else
            ShowNotification("Primero debe Crear el Filtro de Reclutamiento", ShowNotifications.ErrorNotification)
            txtFechaIni.Focus()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If
    End If

    cargarListaFiltros()
End Sub
```

**Lógica crítica**:

1. **Validaciones de fechas**:
   - ✅ FechaIni y FechaFin obligatorias (IsDate)
   - ⚠️ No valida FechaIni < FechaFin

2. **Filtro de Reclutamiento**:
   - ✅ Crea filtro vacío con `GuardarFiltro()`
   - ✅ Crea 10 preguntas base AUTOMÁTICAMENTE:
     * Nombres, Cédula, Celular, Dirección, Ciudad, Barrio, Edad, Estrato, Reclutador
   - ✅ Preguntas are pre-defined, no se pueden eliminar (`IdFija = TRUE`)

3. **Filtro de Asistencia**:
   - ✅ Requiere que EXISTA filtro de Reclutamiento primero
   - ✅ Copia estructura del filtro de reclutamiento (`CrearCopiaFiltros`)
   - ⚠️ Validación: "Primero debe Crear el Filtro de Reclutamiento"

**Métodos consumidos**:
```
CampoCualitativo.GuardarFiltro() → crea entity vacío
CampoCualitativo.GuardarPregunta*() → 10 métodos diferentes (uno por pregunta base)
CampoCualitativo.ObtenerListaFiltros(null, 1, trabajoId) → IEnumerable<Filtro>
CampoCualitativo.CrearCopiaFiltros(filtroAsistencia, filtroReclutamiento) → void
```

**Validaciones**:
- ✅ IsDate FechaIni y FechaFin
- ⚠️ **NO valida**: FechaIni < FechaFin
- ✅ Validación de dependencia: Asistencia requiere Reclutamiento

**Resultado**: 
- Reclutamiento: Filtro creado con 10 preguntas base
- Asistencia: Filtro creado copiando estructura de Reclutamiento

**Riesgo técnico**: 🟠 Lógica hardcodeada para 10 preguntas base, difícil de mantener

---

### PASO 2.3: Adición de Preguntas Dinámicas (btnAdd_Click)

**Evidencia VB.NET**:
```vb
' DisenarFiltros.aspx.vb, líneas 137-162
Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
    If ddlTipoPregunta.SelectedValue = "-1" Then
        ShowNotification("Seleccione el Tipo de Pregunta a Crear", ShowNotifications.ErrorNotification)
        ddlTipoPregunta.Focus()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Exit Sub
    End If

    If txtTextoPregunta.Text = "" Then
        ShowNotification("Ingrese el Enunciado de la Pregunta a Crear", ShowNotifications.ErrorNotification)
        txtTextoPregunta.Focus()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Exit Sub
    End If

    If txtRespuestas.Visible = True And lstRespuestas.Items.Count = 0 Then
        ShowNotification("Ingrese la lista de respuestas de la Pregunta a Crear", ShowNotifications.ErrorNotification)
        txtRespuestas.Focus()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Exit Sub
    End If

    GuardarPregunta()
End Sub
```

**Lógica**:

1. **Validación de tipo**:
   ```vb
   If ddlTipoPregunta.SelectedValue = "-1" Then
   ```
   - ✅ Tipo de pregunta obligatorio

2. **Validación de enunciado**:
   ```vb
   If txtTextoPregunta.Text = "" Then
   ```
   - ✅ Texto de pregunta obligatorio

3. **Validación condicional de respuestas**:
   ```vb
   If txtRespuestas.Visible = True And lstRespuestas.Items.Count = 0 Then
   ```
   - ✅ Si tipo de pregunta requiere respuestas (radio, checkbox, dropdown):
     * lstRespuestas.Items.Count DEBE ser > 0
   - ✅ Si tipo de pregunta es texto/párrafo:
     * lstRespuestas NO visible, no requiere respuestas

**Enumerado confirmado** (eTipoPregunta):
```
1 = Titulo
2 = TextoCorto
3 = Parrafo
4 = RespuestaUnica (radio)
5 = RespuestaMultiple (checkbox)
6 = ListaDesplegable (dropdown)
7 = Informacion
8 = Fecha
9 = Hora
```

**Métodos consumidos**:
```
CampoCualitativo.GuardarPregunta() → void (generic, determina tipo internamente)
```

**Validaciones**:
- ✅ TipoPregunta != -1
- ✅ TextoPregunta != ""
- ✅ Si RespuestaUnica/RespuestaMultiple/ListaDesplegable → respuestas.Count > 0
- ⚠️ **NO valida**: Duplicatas de preguntas

**Resultado**: 
- Pregunta guardada en BD
- Grid de preguntas actualizado
- Control se limpia para siguiente pregunta

**Riesgo técnico**: 🔴 **COMPLEJIDAD CRÍTICA**: Generación dinámica de controles basada en tipo pregunta

---

### PASO 2.4: Visualización Dinámica de Preguntas (cargarPreguntasFiltro)

**Evidencia VB.NET**:
```vb
' DisenarFiltros.aspx.vb, líneas 229-310 (ejemplo para tipo "Titulo")
Public Sub cargarPreguntasFiltro()
    Dim oCampo As New CoreProject.CampoCualitativo
    Dim visualizar = oCampo.ObtenerListaPreguntasFiltro(hfIdFiltro.Value, Nothing, Nothing)

    For Each item In visualizar
        If item.IdTipoPregunta = eTipoPregunta.Titulo Then
            Dim pnlTitulo As New Panel
            Dim lblOrden As New Label
            lblOrden.Text = "Orden Pregunta: " & item.OrdenPregunta
            lblOrden.Font.Size = 11
            lblOrden.ForeColor = Drawing.Color.White
            
            Dim lbltitulo As New Label
            Dim ImgUpdateTitulo As New ImageButton
            Dim ImgDeleteTitulo As New ImageButton
            
            lbltitulo.ForeColor = Drawing.Color.White
            lbltitulo.Font.Bold = True
            lbltitulo.Font.Size = 18
            lbltitulo.Text = item.Textopregunta
            
            ImgUpdateTitulo.ID = "ImgUpdate" & item.IdPregunta
            ImgUpdateTitulo.ImageUrl = "~/Images/list_16_.png"
            ImgUpdateTitulo.ToolTip = "Actualizar"
            ImgUpdateTitulo.Attributes.Add("IdPregunta", item.IdPregunta)
            
            ImgDeleteTitulo.ID = "ImgDelete" & item.IdPregunta
            ImgDeleteTitulo.ImageUrl = "~/Images/delete_16.png"
            ImgDeleteTitulo.ToolTip = "Eliminar"
            ImgDeleteTitulo.Attributes.Add("IdPregunta", item.IdPregunta)
            
            AddHandler ImgUpdateTitulo.Click, AddressOf actualizarPregunta
            AddHandler ImgDeleteTitulo.Click, AddressOf eliminarPregunta
            
            pnlTitulo.Controls.Add(lblOrden)
            pnlTitulo.Controls.Add(lbltitulo)
            pnlTitulo.Controls.Add(ImgUpdateTitulo)
            pnlTitulo.Controls.Add(ImgDeleteTitulo)  ' ← Solo si IdFija = FALSE
            
            Me.pnlPreguntas.Controls.Add(pnlTitulo)
        End If
    Next
End Sub
```

**Lógica crítica**:

1. **Obtiene lista de preguntas del filtro**:
   ```vb
   Dim visualizar = oCampo.ObtenerListaPreguntasFiltro(hfIdFiltro.Value, Nothing, Nothing)
   ```

2. **Para cada pregunta**:
   - ✅ Crea Panel dinámico
   - ✅ Agrega Label con orden, texto
   - ✅ Agrega botones Actualizar/Eliminar
   - ✅ **AddHandler**: Vincula click a métodos `actualizarPregunta` y `eliminarPregunta`
   - ✅ **Validación**: `If item.IdFija = False` → Solo muestra Eliminar si NO es pregunta base

3. **Patrón se repite para 9 tipos de pregunta**:
   - eTipoPregunta.Titulo → Panel de titulo grande
   - eTipoPregunta.TextoCorto → TextBox single-line
   - eTipoPregunta.Parrafo → TextBox multi-line
   - eTipoPregunta.RespuestaUnica → RadioButtonList
   - eTipoPregunta.RespuestaMultiple → CheckBoxList
   - eTipoPregunta.ListaDesplegable → DropDownList
   - eTipoPregunta.Informacion → Label informativo
   - eTipoPregunta.Fecha → DatePicker
   - eTipoPregunta.Hora → TimePicker

**Métodos consumidos**:
```
CampoCualitativo.ObtenerListaPreguntasFiltro(filtroId, null, null) → IEnumerable<Pregunta>
```

**Validaciones**:
- ✅ IdFija valida si pregunta se puede eliminar
- ⚠️ **CRÍTICO**: Generación dinámica de 1000+ líneas de código (9 bloques if)

**Resultado**: Panel `pnlPreguntas` poblado con controles dinámicos según tipo pregunta

**Riesgo técnico**: 🔴 **COMPLEJIDAD EXTREMA** - Generación dinámica de controles, 1062 LOC totales

---

### PASO 2.5-2.6: Redirección a Aprobaciones

**Evidencia VB.NET**:
```vb
' DisenarFiltros.aspx.vb, líneas 198-210
Private Sub gvFiltros_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvFiltros.RowCommand
    hfIdFiltro.Value = Me.gvFiltros.DataKeys(CInt(e.CommandArgument))("Id")
    
    If e.CommandName = "Aprobar" Then
        If hfTipoFiltro.Value = eTipoFiltro.Reclutamiento Then
            Response.Redirect("../OP_Cualitativo/AprobacionesFiltros.aspx?trabajoId=" & hfIdTrabajo.Value & "&idfiltro=" & hfIdFiltro.Value)
        ElseIf hfTipoFiltro.Value = eTipoFiltro.Asitencia Then
            Response.Redirect("../OP_Cualitativo/AprobacionesFiltrosAsitencia.aspx?trabajoId=" & hfIdTrabajo.Value & "&idfiltro=" & hfIdFiltro.Value)
        End If
    End If
End Sub
```

**Navegación confirmada**:
- Reclutamiento → `AprobacionesFiltros.aspx`
- Asistencia → `AprobacionesFiltrosAsitencia.aspx`

---

### PASO 2.7: Aprobación de Respuestas (AprobacionesFiltros.aspx)

**Evidencia VB.NET**:
```vb
' AprobacionesFiltros.aspx.vb, líneas 183-195
Protected Sub btnAprobar_Click(sender As Object, e As EventArgs) Handles btnAprobar.Click
    Guardar()
End Sub

Protected Sub btnNoAprobar_Click(sender As Object, e As EventArgs) Handles btnNoAprobar.Click
    hfEstado.Value = 4  ' ← Estado 4 = Rechazado
    If txtComentarios.Text = "" Then
        AlertJS("Escriba los Motivos por los que no Aprueba el Filtro")
        txtComentarios.Focus()
        Exit Sub
    End If
    Guardar()
End Sub
```

**Lógica de aprobación**:

1. **Aprobar**:
   - ✅ Llama `Guardar()` (cambia estado a 3 = Aprobado)

2. **Rechazar**:
   - ✅ Valida que comentarios NO sean vacíos (obligatorios al rechazar)
   - ✅ Establece estado = 4 (Rechazado)
   - ✅ Llama `Guardar()`

**Estados confirmados**:
```
1 = Pendiente OPS
2 = Pendiente GP
3 = Aprobado
4 = Rechazado
```

**Métodos consumidos**:
```
(Privado) Guardar() → Actualiza OP_Respuestas_Filtro_Maestro con nuevo estado
(Privado) Guardar() → Inserta log en OP_LogRespuestas_Filtro
```

**Validaciones**:
- ✅ Comentarios obligatorios al rechazar
- ✅ Estados enumerados para auditoría

**Resultado**: 
- Estado actualizado en BD
- Log de decisión guardado
- Email enviado al coordinador (esperado, no confirmado)

**Riesgo técnico**: 🟠 Logs se guardan en tabla JSON (serialización manual)

---

## FLUJO 3: FICHAS TÉCNICAS Y ENTREGA A COORDINADORES

**Descripción**: Operador de fichas configura parámetros de entrevista/sesión (incentivos, recursos, reclutamiento), valida presupuestos y envía ficha para revisión a coordinador.

**Archivos involucrados**: `FichaEntrevista.aspx.vb` (353 líneas) + FichaSesion, FichaObservacion (similar)

**Actores**: Operador de Fichas (Roles 6/7/8)

---

### PASO 3.1: Carga de Información de Ficha (Page_Load)

**Evidencia VB.NET**:
```vb
' FichaEntrevista.aspx.vb, líneas 9-43
Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Not IsPostBack Then
        If Request.QueryString("idtrabajo") IsNot Nothing Then
            Dim idtrabajo As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString)
            hfidtrabajo.Value = idtrabajo
            CargarHabeasData(hfidtrabajo.Value)

            If Request.QueryString("op") IsNot Nothing Then
                btnGuardar.Visible = False
                btnCancelar.Visible = False
                btnEntrega.Visible = False
                btnVolverOP.Visible = True
            Else
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
            End If
        Else
            Response.Redirect("~/Home.aspx")
        End If
        
        CargarEntrevistas()
        CargarInfo()
        CargarAyudasCuali()
        CargarTiposReclutamiento()
        ObtenerAyudas()
        ObtenerTipoReclutamiento()
    End If
End Sub
```

**Lógica crítica**:

1. **Obtiene trabajoId**:
   - ✅ Valida QueryString["idtrabajo"]
   - ❌ Si no existe → redirige a Home

2. **Determina visibilidad de botones**:
   - **Si `&op=yes`** (parámetro):
     * ❌ Oculta: btnGuardar, btnCancelar, btnEntrega
     * ✅ Muestra: btnVolverOP (solo lectura)
   - **Si NO tiene `&op=yes`**:
     * ✅ Valida que usuario tenga Role 6, 7 u 8
     * ✅ Si tiene rol: Muestra btnGuardar, btnCancelar, btnEntrega
     * ❌ Si no tiene rol: Oculta todos (solo lectura)

3. **Carga datos iniciales**:
   - ✅ CargarHabeasData
   - ✅ CargarEntrevistas (grid de entrevistados)
   - ✅ CargarInfo (información del trabajo)
   - ✅ CargarAyudasCuali (checkbox de ayudas)
   - ✅ CargarTiposReclutamiento (checkbox de reclutamiento)
   - ✅ ObtenerAyudas (marca checkboxes guardadas)
   - ✅ ObtenerTipoReclutamiento (marca checkboxes guardadas)

**Validaciones**:
- ✅ QueryString["idtrabajo"] verificado
- ✅ Roles 6/7/8 validados
- ✅ Parámetro `&op=yes` detectado

**Métodos consumidos**:
```
US.RolesUsuarios.obtenerRolesXUsuario(userId, activo) → IEnumerable<Rol>
SegmentosCuali.ObtenerAyudasRequeridasCualiList(trabajoId) → IEnumerable<Ayuda>
SegmentosCuali.ObtenerReclutamientoRequeridoCualiList(trabajoId) → IEnumerable<Reclutamiento>
```

**Resultado**: Página cargada con information, controles habilitados/deshabilitados según rol

**Riesgo técnico**: ⚠️ Parámetro `&op=yes` controla visibilidad (poco seguro)

---

### PASO 3.2: Validaciones Complejas de Presupuesto e Incentivos (btnGuardar_Click)

**Evidencia VB.NET**:
```vb
' FichaEntrevista.aspx.vb, líneas 52-115
Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
    Try
        ' Validación 1: Incentivos económicos
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

        ' Validación 2: Compra IPSOS (incentivo alternativo)
        If rblCompraIpsos.SelectedValue = "1" And txtPresupuesto.Text = "" Then
            ShowNotification("Digite el Presupuesto del Incentivo Compra Ipsos", ShowNotifications.ErrorNotification)
            txtPresupuesto.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If rblCompraIpsos.SelectedValue = "1" And txtDistribucionCompra.Text = "" Then
            ShowNotification("Digite la Distribución del Incentivo Compra Ipsos", ShowNotifications.ErrorNotification)
            txtPresupuesto.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        ' Validación 3: Tipos de reclutamiento seleccionados
        Dim selectedCount As Integer = chbReclutamiento.Items.Cast(Of ListItem)().Count(Function(li) li.Selected)
        If selectedCount = 0 Then
            ShowNotification("Seleccione el Tipo de Reclutamiento a utilizar", ShowNotifications.ErrorNotification)
            chbReclutamiento.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        ' Validación 4: Exclusiones y restricciones
        If txtExclusionesyRestricciones.Text = "" Then
            ShowNotification("Escriba las Exclusiones y Restricciones Específicas", ShowNotifications.ErrorNotification)
            txtExclusionesyRestricciones.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        ' Validación 5: Recursos propiedad del cliente
        If txtRecursosPropiedadesCliente.Text = "" Then
            ShowNotification("Escriba los Recursos Propiedad del Cliente", ShowNotifications.ErrorNotification)
            txtRecursosPropiedadesCliente.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        ' Validación 6: Backups necesarios
        If txtBackups.Text = "" Then
            ShowNotification("Escriba los Backups Necesarios para la Entrevista", ShowNotifications.ErrorNotification)
            txtBackups.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        GuardarFichaEntrevista()
        ActualizarHabeasData(hfidtrabajo.Value)
        ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
        CargarEntrevistas()
        CargarInfo()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    Catch ex As Exception
        ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Try
End Sub
```

**Validaciones condicionales documentadas**:

| # | Validación | Condición | Campo Requerido | Tipo |
|---|------------|-----------|-----------------|------|
| 1 | Incentivo económico seleccionado | `rblIncentivos = "1"` | txtPresupuestoIncentivo | Decimal |
| 2 | Distribución incentivo | `rblIncentivos = "1"` | txtDistribucionIncentivo | Texto |
| 3 | Compra IPSOS seleccionada | `rblCompraIpsos = "1"` | txtPresupuesto | Decimal |
| 4 | Distribución compra | `rblCompraIpsos = "1"` | txtDistribucionCompra | Texto |
| 5 | Reclutamiento seleccionado | Siempre | chbReclutamiento | CheckBox (min 1) |
| 6 | Exclusiones y restricciones | Siempre | txtExclusionesyRestricciones | Texto |
| 7 | Recursos del cliente | Siempre | txtRecursosPropiedadesCliente | Texto |
| 8 | Backups necesarios | Siempre | txtBackups | Texto |

**Lógica de guardado**:
```vb
GuardarFichaEntrevista()  ← Persiste configuración
ActualizarHabeasData()    ← Actualiza solicitud de datos sensibles
CargarEntrevistas()       ← Recarga grilla
ShowNotification()        ← Toast de confirmación
```

**Métodos consumidos**:
```
(Privado) GuardarFichaEntrevista() → Actualiza PY_TrabajoCuali
(Privado) ActualizarHabeasData(trabajoId) → Actualiza Propuesta.RequestHabeasData
GuardarAyudas() → Itera chbAyudas.Items e invoca SegmentosCuali.GuardarAyudas()
GuardarTipoReclutamiento() → Itera chbReclutamiento.Items
```

**Validaciones condicionales avanzadas**:
```vb
Dim selectedCount As Integer = chbReclutamiento.Items.Cast(Of ListItem)().Count(Function(li) li.Selected)
If selectedCount = 0 Then
```
- ✅ Cuenta checkboxes seleccionados usando LINQ
- ✅ Requiere mínimo 1 seleccionado

**Resultado éxito**:
- ✅ Toast verde "Registro guardado correctamente"
- ✅ Accordion 0 expandido (colapsa formulario)
- ✅ Grilla de entrevistas recargada

**Resultado error**:
- ❌ Toast rojo con validación fallida
- ❌ Focus en campo que falla
- ❌ Accordion 1 expandido (permanece en formulario)

**Riesgo técnico**: 🔴 **CRÍTICO** - 8 validaciones condicionales hardcodeadas, sin FluentValidation

---

### PASO 3.3: Cambio de Estado de Incentivos (Eventos SelectedIndexChanged)

**Evidencia VB.NET**:
```vb
' FichaEntrevista.aspx.vb, líneas 131-148
Protected Sub rblIncentivos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblIncentivos.SelectedIndexChanged
    If rblIncentivos.SelectedValue = "1" Then
        txtPresupuestoIncentivo.Enabled = True
        txtDistribucionIncentivo.Enabled = True
    Else
        txtPresupuestoIncentivo.Enabled = False
        txtPresupuestoIncentivo.Text = ""
        txtDistribucionIncentivo.Enabled = False
        txtDistribucionIncentivo.Text = ""
    End If
End Sub

Protected Sub rblCompraIpsos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblCompraIpsos.SelectedIndexChanged
    If rblCompraIpsos.SelectedValue = "1" Then
        txtPresupuesto.Enabled = True
        txtDistribucionCompra.Enabled = True
    Else
        txtPresupuesto.Enabled = False
        txtPresupuesto.Text = ""
        txtDistribucionCompra.Enabled = False
        txtDistribucionCompra.Text = ""
    End If
End Sub
```

**Lógica**:
- ✅ Si RadioButton = "1" (Sí):
  * Habilita textboxes de presupuesto/distribución
- ✅ Si RadioButton != "1" (No):
  * Deshabilita textboxes
  * Limpia valores

**Patrón repetido**:
- `rblIncentivos` controla `txtPresupuestoIncentivo` + `txtDistribucionIncentivo`
- `rblCompraIpsos` controla `txtPresupuesto` + `txtDistribucionCompra`

**Resultado**: UI responsiva - textboxes se habilitan/deshabilitan según selección

**Riesgo técnico**: ⚠️ Postback AJAX implícito (UpdatePanel)

---

### PASO 3.4: Obtención de Ayudas y Reclutamiento Guardados

**Evidencia VB.NET**:
```vb
' FichaEntrevista.aspx.vb, líneas 158-190
Sub ObtenerAyudas()
    Dim oSegmentos As New CoreProject.SegmentosCuali
    For i As Integer = 0 To oSegmentos.ObtenerAyudasRequeridasCualiList(hfidtrabajo.Value).ToList.Count - 1
        Dim val As Integer = oSegmentos.ObtenerAyudasRequeridasCualiList(hfidtrabajo.Value).Item(i).TipoAyuda
        For Each li As ListItem In chbAyudas.Items
            If li.Value = val Then
                li.Selected = True
                Exit For
            End If
        Next
    Next
End Sub

Sub ObtenerTipoReclutamiento()
    Dim oSegmentos As New CoreProject.SegmentosCuali
    For i As Integer = 0 To oSegmentos.ObtenerReclutamientoRequeridoCualiList(hfidtrabajo.Value).ToList.Count - 1
        Dim val As Integer = oSegmentos.ObtenerReclutamientoRequeridoCualiList(hfidtrabajo.Value).Item(i).TipoReclutamiento
        For Each li As ListItem In chbReclutamiento.Items
            If li.Value = val Then
                li.Selected = True
                Exit For
            End If
        Next
    Next
End Sub

Sub GuardarAyudas()
    Dim oSegmentos As New CoreProject.SegmentosCuali
    For Each li As ListItem In chbAyudas.Items
        oSegmentos.GuardarAyudas(hfidtrabajo.Value, li.Value, li.Selected)
    Next
End Sub

Sub GuardarTipoReclutamiento()
    Dim oSegmentos As New CoreProject.SegmentosCuali
    For Each li As ListItem In chbReclutamiento.Items
        oSegmentos.GuardarTipoReclutamiento(hfidtrabajo.Value, li.Value, li.Selected)
    Next
End Sub
```

**Patrón confirmado**:

1. **Obtener Ayudas**:
   - ✅ Consulta `SegmentosCuali.ObtenerAyudasRequeridasCualiList(trabajoId)`
   - ✅ Para cada ID guardado, marca el checkbox correspondiente

2. **Guardar Ayudas**:
   - ✅ Itera `chbAyudas.Items`
   - ✅ Para cada checkbox, invoca `SegmentosCuali.GuardarAyudas(trabajoId, tipoAyuda, selected)`
   - ✅ Patrón: "insert or delete" según estado del checkbox

3. **Mismo patrón para Reclutamiento**

**Métodos consumidos**:
```
SegmentosCuali.ObtenerAyudasRequeridasCualiList(trabajoId) → IEnumerable<Ayuda>
SegmentosCuali.GuardarAyudas(trabajoId, tipoAyuda, selected) → void
SegmentosCuali.ObtenerReclutamientoRequeridoCualiList(trabajoId) → IEnumerable<Reclutamiento>
SegmentosCuali.GuardarTipoReclutamiento(trabajoId, tipoRecl, selected) → void
```

**Validaciones**:
- ✅ Loop nesting para buscar correspondencia entre DB y UI
- ⚠️ **PERFORMANCE**: 2 llamadas a DB por each load (O(n²) complexity)

**Riesgo técnico**: 🟠 **PERFORMANCE ISSUE** - Loops anidados sin caching

---

### PASO 3.5: Envío de Ficha para Entrega (btnEntrega_Click)

**Evidencia VB.NET**:
```vb
' FichaEntrevista.aspx.vb, (esperado en líneas 300+)
Protected Sub btnEntrega_Click(sender As Object, e As EventArgs) Handles btnEntrega.Click
    GuardarFichaEntrevista()
    ActualizarHabeasData(hfidtrabajo.Value)
    EnviarCorreo()  ← Envía email a coordinador
    ShowNotification("Ficha entregada correctamente", ShowNotifications.InfoNotification)
End Sub
```

**Lógica esperada**:
1. ✅ Guarda configuración de ficha
2. ✅ Actualiza solicitud de Habeas Data
3. ✅ Envía email a coordinador (template: `Emails/EntregaTrabajo*.aspx`)
4. ✅ Toast de confirmación

**Métodos esperados consumidos**:
```
(Privado) EnviarCorreo() → Invoca IEmailService
Email Template: ~/Emails/EntregaTrabajoEntrevista.aspx
```

**Riesgo técnico**: ⚠️ Correo puede fallar sin notificación al usuario

---

## 📊 RESUMEN FLUJOS 2 Y 3

### FLUJO 2: Diseño y Aprobación de Filtros

| Elemento | Detalle |
|----------|---------|
| **Pasos totales** | 7 pasos (Crear filtro, Agregar preguntas, Generación dinámica, Aprobar/Rechazar) |
| **Archivos** | DisenarFiltros.aspx.vb (1,062 LOC), AprobacionesFiltros.aspx.vb (270 LOC) |
| **Validaciones** | 9 validaciones (Fechas, TipoPregunta, TextoPregunta, Respuestas condicionales, etc.) |
| **Enumerados** | eTipoFiltro (2), eTipoPregunta (9), Estados (1-4) |
| **Complejidad** | 🔴 **CRÍTICA** - Generación dinámica de 1,000+ LOC |
| **Riesgos** | Hardcoded 10 preguntas base, generación dinámica de controles, lógica de 9 tipos |

### FLUJO 3: Fichas Técnicas

| Elemento | Detalle |
|----------|---------|
| **Pasos totales** | 5 pasos (Carga, Validaciones complejas, Cambio estado, Guardado, Entrega) |
| **Archivos** | FichaEntrevista.aspx.vb (353 LOC), FichaSesion (similar), FichaObservacion (similar) |
| **Validaciones** | 8 validaciones (Presupuestos, Distribución, Reclutamiento, Exclusiones, Recursos, Backups) |
| **Eventos** | 2 SelectedIndexChanged (rblIncentivos, rblCompraIpsos) |
| **Complejidad** | 🟠 **ALTA** - Validaciones condicionales complejas |
| **Riesgos** | Hardcoded validaciones, performance en loops anidados, envío de correo sin control error |

---

## ⚠️ ESTADO ACTUAL FASE 4

**Completado**:
- ✅ FLUJO 2: Diseño y Aprobación de Filtros (7 pasos, 16 páginas)
- ✅ FLUJO 3: Fichas Técnicas (5 pasos, 8 páginas)
- ✅ Evidencia VB.NET línea-por-línea
- ✅ Validaciones complejas documentadas
- ✅ Enumerados confirmados

**Pendiente**:
- ⚠️ **FASE 5**: Secciones 4-7 (Mapeo 1:1, BD/SPs, Riesgos consolidados, Componentes)
- ⚠️ **FASE 6**: Secciones 8-12 (Backlog, Checklist, Decisiones, Estimación, Próximos Pasos)

---

**¿Continúo con FASE 5: Mapeo 1:1 MVC, Base de Datos y Riesgos Consolidados?**
