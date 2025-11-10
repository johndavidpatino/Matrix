Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios

Public Class DisenarFiltros
    Inherits System.Web.UI.Page

    Enum eTipoFiltro
        Reclutamiento = 1
        Asitencia = 2
    End Enum
    Enum eTipoPregunta
        Titulo = 1
        TextoCorto = 2
        Parrafo = 3
        RespuestaUnica = 4
        RespuestaMultiple = 5
        ListaDesplegable = 6
        Informacion = 7
        Fecha = 8
        Hora = 9
    End Enum

#Region "Propiedades"
    Private _Trabajo As PY_Trabajo_Get_Result
    Public Property Trabajo() As PY_Trabajo_Get_Result
        Get
            Return _Trabajo
        End Get
        Set(ByVal value As PY_Trabajo_Get_Result)
            _Trabajo = value
        End Set
    End Property
#End Region

#Region "Eventos del Control"
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

            If Request.QueryString("py") IsNot Nothing Then
                hfPY.Value = "1"
            Else
                hfPY.Value = "0"
            End If

            If hfPY.Value = "1" Then
                lbtnVolver.PostBackUrl = "../PY_Proyectos/TrabajosCualitativos.aspx?ProyectoId=" & Trabajo.ProyectoId
            Else
                lbtnVolver.PostBackUrl = "~/OP_Cualitativo/Trabajos.aspx"
            End If

            CargarLabelTrabajo()
            cargarListaFiltros()
            cargarTipoPregunta()

        End If
        CargarPreguntas()
    End Sub

    Public Sub CargarPreguntas()
        pnlPreguntas.Controls.Clear()
        cargarPreguntasFiltro()
    End Sub

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

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Limpiar()
        cargarListaFiltros()
        pnlCrearFiltro.Visible = False
        pnlFiltros.Visible = True
        pnlVisualizar.Visible = True
    End Sub

    Protected Sub btnAddRespuestas_Click(sender As Object, e As EventArgs) Handles btnAddRespuestas.Click
        lstRespuestas.Items.Add(txtRespuestas.Text)
        txtRespuestas.Text = ""
        lstRespuestas.Visible = True
        btnRemoveRespuestas.Visible = True
        CargarPreguntas()
    End Sub

    Protected Sub btnRemoveRespuestas_Click(sender As Object, e As EventArgs) Handles btnRemoveRespuestas.Click
        lstRespuestas.Items.RemoveAt(lstRespuestas.SelectedIndex)
        lstRespuestas.Visible = True
        btnRemoveRespuestas.Visible = True
        CargarPreguntas()
    End Sub

    Protected Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        generarLink()
    End Sub

#End Region

#Region "Funciones y Metodos"

    Private Sub gvFiltros_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvFiltros.RowCommand
        hfIdFiltro.Value = Me.gvFiltros.DataKeys(CInt(e.CommandArgument))("Id")
        If e.CommandName = "Gestionar" Then
            CargarPreguntas()
            pnlNewFiltro.Visible = True
            pnlVisualizar.Visible = True
        End If
        If e.CommandName = "Aprobar" Then
            If hfPY.Value = "1" Then
                If hfTipoFiltro.Value = eTipoFiltro.Reclutamiento Then
                    Response.Redirect("../OP_Cualitativo/AprobacionesFiltros.aspx?trabajoId=" & hfIdTrabajo.Value & "&idfiltro=" & hfIdFiltro.Value & "&py=true")
                ElseIf hfTipoFiltro.Value = eTipoFiltro.Asitencia Then
                    Response.Redirect("../OP_Cualitativo/AprobacionesFiltrosAsitencia.aspx?trabajoId=" & hfIdTrabajo.Value & "&idfiltro=" & hfIdFiltro.Value & "&py=true")
                End If
            Else
                If hfTipoFiltro.Value = eTipoFiltro.Reclutamiento Then
                    Response.Redirect("../OP_Cualitativo/AprobacionesFiltros.aspx?trabajoId=" & hfIdTrabajo.Value & "&idfiltro=" & hfIdFiltro.Value)
                ElseIf hfTipoFiltro.Value = eTipoFiltro.Asitencia Then
                    Response.Redirect("../OP_Cualitativo/AprobacionesFiltrosAsitencia.aspx?trabajoId=" & hfIdTrabajo.Value & "&idfiltro=" & hfIdFiltro.Value)
                End If

            End If
        End If
    End Sub

    Public Sub Limpiar()
        hfIdPregunta.Value = "0"
        ddlTipoPregunta.ClearSelection()
        lblOrden.Visible = False
        txtOrden.Visible = False
        txtOrden.Text = ""
        txtTextoPregunta.Text = ""
        txtRespuestas.Text = ""
        lstRespuestas.Items.Clear()
        lstRespuestas.Visible = False
        btnRemoveRespuestas.Visible = False
        chbObligatoria.Checked = False
        pnlRespuestas.Visible = False
    End Sub
    Sub CargarLabelTrabajo()
        Dim oTrabajo As New Trabajo
        lblTrabajo.Text = oTrabajo.obtenerXId(hfIdTrabajo.Value).JobBook & " " & oTrabajo.obtenerXId(hfIdTrabajo.Value).NombreTrabajo
    End Sub

    Public Sub cargarTipoPregunta()
        Dim oCampo As New CoreProject.CampoCualitativo
        ddlTipoPregunta.DataSource = oCampo.ObtenerTipoPreguntaFiltro
        ddlTipoPregunta.DataValueField = "Id"
        ddlTipoPregunta.DataTextField = "TipoPregunta"
        ddlTipoPregunta.DataBind()
        Me.ddlTipoPregunta.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Public Sub cargarListaFiltros()
        Dim oCampo As New CoreProject.CampoCualitativo
        gvFiltros.DataSource = oCampo.ObtenerListaFiltros(Nothing, hfTipoFiltro.Value, hfIdTrabajo.Value)
        gvFiltros.DataBind()

        Dim lstFiltros = oCampo.ObtenerListaFiltros(Nothing, hfTipoFiltro.Value, hfIdTrabajo.Value)
        If lstFiltros.Count > 0 Then
            pnlCrearFiltro.Visible = False
            pnlFiltros.Visible = True
        Else
            pnlCrearFiltro.Visible = True
            pnlFiltros.Visible = False
        End If

    End Sub

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
                Dim salto As New LiteralControl("<br/><br/>")
                Dim salto1 As New LiteralControl("<br/><br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                lbltitulo.ForeColor = Drawing.Color.White
                lbltitulo.Font.Bold = True
                lbltitulo.Font.Size = 18
                lbltitulo.Text = item.Textopregunta
                pnlTitulo.Style.Add("margin-bottom", "10px")
                pnlTitulo.Style.Add("margin-top", "10px")
                pnlTitulo.BorderWidth = 1
                pnlTitulo.BorderColor = Drawing.Color.White
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
                pnlTitulo.Controls.Add(salto)
                pnlTitulo.Controls.Add(lbltitulo)
                pnlTitulo.Controls.Add(salto1)
                pnlTitulo.Controls.Add(ImgUpdateTitulo)
                pnlTitulo.Controls.Add(espacio)
                pnlTitulo.Controls.Add(espacio)
                If item.IdFija = False Then
                    pnlTitulo.Controls.Add(ImgDeleteTitulo)
                End If
                Me.pnlPreguntas.Controls.Add(pnlTitulo)
            End If

            If item.IdTipoPregunta = eTipoPregunta.TextoCorto Then
                Dim pnlTextcorto As New Panel
                Dim lblOrden As New Label
                lblOrden.Text = "Orden Pregunta: " & item.OrdenPregunta
                lblOrden.Font.Size = 11
                lblOrden.ForeColor = Drawing.Color.White
                Dim lblTextoCorto As New Label
                Dim txtTextoCorto As New TextBox
                Dim ImgUpdateTextcorto As New ImageButton
                Dim ImgDeleteTextcorto As New ImageButton
                Dim salto As New LiteralControl("<br/>")
                Dim salto1 As New LiteralControl("<br/><br/>")
                Dim salto2 As New LiteralControl("<br/><br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                lblTextoCorto.ForeColor = Drawing.Color.White
                lblTextoCorto.Text = item.Textopregunta
                txtTextoCorto.Width = 400
                pnlTextcorto.Style.Add("margin-bottom", "10px")
                pnlTextcorto.Style.Add("margin-top", "10px")
                pnlTextcorto.BorderWidth = 1
                pnlTextcorto.BorderColor = Drawing.Color.White
                ImgUpdateTextcorto.ID = "ImgUpdate" & item.IdPregunta
                ImgUpdateTextcorto.ImageUrl = "~/Images/list_16_.png"
                ImgUpdateTextcorto.ToolTip = "Actualizar"
                ImgUpdateTextcorto.Attributes.Add("IdPregunta", item.IdPregunta)
                ImgDeleteTextcorto.ID = "ImgDelete" & item.IdPregunta
                ImgDeleteTextcorto.ImageUrl = "~/Images/delete_16.png"
                ImgDeleteTextcorto.ToolTip = "Eliminar"
                ImgDeleteTextcorto.Attributes.Add("IdPregunta", item.IdPregunta)
                AddHandler ImgUpdateTextcorto.Click, AddressOf actualizarPregunta
                AddHandler ImgDeleteTextcorto.Click, AddressOf eliminarPregunta
                pnlTextcorto.Controls.Add(lblOrden)
                pnlTextcorto.Controls.Add(salto2)
                pnlTextcorto.Controls.Add(lblTextoCorto)
                pnlTextcorto.Controls.Add(salto)
                pnlTextcorto.Controls.Add(txtTextoCorto)
                pnlTextcorto.Controls.Add(salto1)
                pnlTextcorto.Controls.Add(ImgUpdateTextcorto)
                pnlTextcorto.Controls.Add(espacio)
                pnlTextcorto.Controls.Add(espacio)
                If item.IdFija = False Then
                    pnlTextcorto.Controls.Add(ImgDeleteTextcorto)
                End If
                Me.pnlPreguntas.Controls.Add(pnlTextcorto)
            End If

            If item.IdTipoPregunta = eTipoPregunta.Parrafo Then
                Dim pnlParrafo As New Panel
                Dim lblOrden As New Label
                lblOrden.Text = "Orden Pregunta: " & item.OrdenPregunta
                lblOrden.Font.Size = 11
                lblOrden.ForeColor = Drawing.Color.White
                Dim lblParrafo As New Label
                Dim txtParrafo As New TextBox
                Dim ImgUpdateParrafo As New ImageButton
                Dim ImgDeleteParrafo As New ImageButton
                Dim salto As New LiteralControl("<br/><br/>")
                Dim salto1 As New LiteralControl("<br/><br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                txtParrafo.TextMode = TextBoxMode.MultiLine
                txtParrafo.Width = 600
                lblParrafo.ForeColor = Drawing.Color.White
                lblParrafo.Text = item.Textopregunta
                pnlParrafo.Style.Add("margin-bottom", "10px")
                pnlParrafo.Style.Add("margin-top", "10px")
                pnlParrafo.BorderWidth = 1
                pnlParrafo.BorderColor = Drawing.Color.White
                ImgUpdateParrafo.ID = "ImgUpdate" & item.IdPregunta
                ImgUpdateParrafo.ImageUrl = "~/Images/list_16_.png"
                ImgUpdateParrafo.ToolTip = "Actualizar"
                ImgUpdateParrafo.Attributes.Add("IdPregunta", item.IdPregunta)
                ImgDeleteParrafo.ID = "ImgDelete" & item.IdPregunta
                ImgDeleteParrafo.ImageUrl = "~/Images/delete_16.png"
                ImgDeleteParrafo.ToolTip = "Eliminar"
                ImgDeleteParrafo.Attributes.Add("IdPregunta", item.IdPregunta)
                AddHandler ImgUpdateParrafo.Click, AddressOf actualizarPregunta
                AddHandler ImgDeleteParrafo.Click, AddressOf eliminarPregunta
                pnlParrafo.Controls.Add(lblOrden)
                pnlParrafo.Controls.Add(salto1)
                pnlParrafo.Controls.Add(lblParrafo)
                pnlParrafo.Controls.Add(txtParrafo)
                pnlParrafo.Controls.Add(salto)
                pnlParrafo.Controls.Add(ImgUpdateParrafo)
                pnlParrafo.Controls.Add(espacio)
                pnlParrafo.Controls.Add(espacio)
                If item.IdFija = False Then
                    pnlParrafo.Controls.Add(ImgDeleteParrafo)
                End If

                Me.pnlPreguntas.Controls.Add(pnlParrafo)
            End If

            If item.IdTipoPregunta = eTipoPregunta.RespuestaUnica Then
                Dim pnlUnica As New Panel
                Dim lblOrden As New Label
                lblOrden.Text = "Orden Pregunta: " & item.OrdenPregunta
                lblOrden.Font.Size = 11
                lblOrden.ForeColor = Drawing.Color.White
                Dim lblUnica As New Label
                Dim rblUnica As New RadioButtonList
                Dim ImgUpdateUnica As New ImageButton
                Dim ImgDeleteUnica As New ImageButton
                Dim salto As New LiteralControl("<br/><br/>")
                Dim salto1 As New LiteralControl("<br/><br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                lblUnica.ForeColor = Drawing.Color.White
                lblUnica.Text = item.Textopregunta
                rblUnica.CssClass = "table"
                pnlUnica.Style.Add("margin-bottom", "10px")
                pnlUnica.Style.Add("margin-top", "10px")
                pnlUnica.BorderWidth = 1
                pnlUnica.BorderColor = Drawing.Color.White
                ImgUpdateUnica.ID = "ImgUpdate" & item.IdPregunta
                ImgUpdateUnica.ImageUrl = "~/Images/list_16_.png"
                ImgUpdateUnica.ToolTip = "Actualizar"
                ImgUpdateUnica.Attributes.Add("IdPregunta", item.IdPregunta)
                ImgDeleteUnica.ID = "ImgDelete" & item.IdPregunta
                ImgDeleteUnica.ImageUrl = "~/Images/delete_16.png"
                ImgDeleteUnica.ToolTip = "Eliminar"
                ImgDeleteUnica.Attributes.Add("IdPregunta", item.IdPregunta)
                AddHandler ImgUpdateUnica.Click, AddressOf actualizarPregunta
                AddHandler ImgDeleteUnica.Click, AddressOf eliminarPregunta
                Dim TestArray() As String = Split(item.Respuestas, "|")
                For i As Integer = 0 To TestArray.Length - 1
                    If TestArray(i) <> "" Then
                        rblUnica.Items.Add(TestArray(i))
                    End If
                Next
                pnlUnica.Controls.Add(lblOrden)
                pnlUnica.Controls.Add(salto1)
                pnlUnica.Controls.Add(lblUnica)
                pnlUnica.Controls.Add(rblUnica)
                pnlUnica.Controls.Add(salto)
                pnlUnica.Controls.Add(ImgUpdateUnica)
                pnlUnica.Controls.Add(espacio)
                pnlUnica.Controls.Add(espacio)
                If item.IdFija = False Then
                    pnlUnica.Controls.Add(ImgDeleteUnica)
                End If
                Me.pnlPreguntas.Controls.Add(pnlUnica)
            End If

            If item.IdTipoPregunta = eTipoPregunta.RespuestaMultiple Then
                Dim pnlMultiple As New Panel
                Dim lblOrden As New Label
                lblOrden.Text = "Orden Pregunta: " & item.OrdenPregunta
                lblOrden.Font.Size = 11
                lblOrden.ForeColor = Drawing.Color.White
                Dim lblMultiple As New Label
                Dim chblMultiple As New CheckBoxList
                Dim ImgUpdateMultiple As New ImageButton
                Dim ImgDeleteMultiple As New ImageButton
                Dim salto As New LiteralControl("<br/><br/>")
                Dim salto1 As New LiteralControl("<br/><br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                chblMultiple.CssClass = "table"
                chblMultiple.RepeatLayout = RepeatLayout.Table
                lblMultiple.ForeColor = Drawing.Color.White
                lblMultiple.Text = item.Textopregunta
                pnlMultiple.Style.Add("margin-bottom", "10px")
                pnlMultiple.Style.Add("margin-top", "10px")
                pnlMultiple.BorderWidth = 1
                pnlMultiple.BorderColor = Drawing.Color.White
                ImgUpdateMultiple.ID = "ImgUpdate" & item.IdPregunta
                ImgUpdateMultiple.ImageUrl = "~/Images/list_16_.png"
                ImgUpdateMultiple.ToolTip = "Actualizar"
                ImgUpdateMultiple.Attributes.Add("IdPregunta", item.IdPregunta)
                ImgDeleteMultiple.ID = "ImgDelete" & item.IdPregunta
                ImgDeleteMultiple.ImageUrl = "~/Images/delete_16.png"
                ImgDeleteMultiple.ToolTip = "Eliminar"
                ImgDeleteMultiple.Attributes.Add("IdPregunta", item.IdPregunta)
                AddHandler ImgUpdateMultiple.Click, AddressOf actualizarPregunta
                AddHandler ImgDeleteMultiple.Click, AddressOf eliminarPregunta
                Dim TestArray() As String = Split(item.Respuestas, "|")
                For i As Integer = 0 To TestArray.Length - 1
                    If TestArray(i) <> "" Then
                        chblMultiple.Items.Add(TestArray(i))
                    End If
                Next
                pnlMultiple.Controls.Add(lblOrden)
                pnlMultiple.Controls.Add(salto1)
                pnlMultiple.Controls.Add(lblMultiple)
                pnlMultiple.Controls.Add(chblMultiple)
                pnlMultiple.Controls.Add(salto)
                pnlMultiple.Controls.Add(ImgUpdateMultiple)
                pnlMultiple.Controls.Add(espacio)
                pnlMultiple.Controls.Add(espacio)
                If item.IdFija = False Then
                    pnlMultiple.Controls.Add(ImgDeleteMultiple)
                End If
                Me.pnlPreguntas.Controls.Add(pnlMultiple)
            End If

            If item.IdTipoPregunta = eTipoPregunta.ListaDesplegable Then
                Dim pnlLista As New Panel
                Dim lblOrden As New Label
                lblOrden.Text = "Orden Pregunta: " & item.OrdenPregunta
                lblOrden.Font.Size = 11
                lblOrden.ForeColor = Drawing.Color.White
                Dim lblLista As New Label
                Dim ddlLista As New DropDownList
                Dim ImgUpdateLista As New ImageButton
                Dim ImgDeleteLista As New ImageButton
                Dim salto As New LiteralControl("<br/>")
                Dim salto1 As New LiteralControl("<br/><br/>")
                Dim salto2 As New LiteralControl("<br/><br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                ddlLista.Width = 400
                lblLista.ForeColor = Drawing.Color.White
                lblLista.Text = item.Textopregunta
                ddlLista.CssClass = "table"
                pnlLista.Style.Add("margin-bottom", "10px")
                pnlLista.Style.Add("margin-top", "10px")
                pnlLista.BorderWidth = 1
                pnlLista.BorderColor = Drawing.Color.White
                ImgUpdateLista.ID = "ImgUpdate" & item.IdPregunta
                ImgUpdateLista.ImageUrl = "~/Images/list_16_.png"
                ImgUpdateLista.ToolTip = "Actualizar"
                ImgUpdateLista.Attributes.Add("IdPregunta", item.IdPregunta)
                ImgDeleteLista.ID = "ImgDelete" & item.IdPregunta
                ImgDeleteLista.ImageUrl = "~/Images/delete_16.png"
                ImgDeleteLista.ToolTip = "Eliminar"
                ImgDeleteLista.Attributes.Add("IdPregunta", item.IdPregunta)
                AddHandler ImgUpdateLista.Click, AddressOf actualizarPregunta
                AddHandler ImgDeleteLista.Click, AddressOf eliminarPregunta
                Dim TestArray() As String = Split(item.Respuestas, "|")
                For i As Integer = 0 To TestArray.Length - 1
                    If TestArray(i) <> "" Then
                        ddlLista.Items.Add(TestArray(i))
                    End If
                Next
                pnlLista.Controls.Add(lblOrden)
                pnlLista.Controls.Add(salto2)
                pnlLista.Controls.Add(lblLista)
                pnlLista.Controls.Add(salto)
                pnlLista.Controls.Add(ddlLista)
                pnlLista.Controls.Add(salto1)
                pnlLista.Controls.Add(ImgUpdateLista)
                pnlLista.Controls.Add(espacio)
                pnlLista.Controls.Add(espacio)
                If item.IdFija = False Then
                    pnlLista.Controls.Add(ImgDeleteLista)
                End If
                Me.pnlPreguntas.Controls.Add(pnlLista)
            End If

            If item.IdTipoPregunta = eTipoPregunta.Informacion Then
                Dim pnlInformacion As New Panel
                Dim lblOrden As New Label
                lblOrden.Text = "Orden Pregunta: " & item.OrdenPregunta
                lblOrden.Font.Size = 11
                lblOrden.ForeColor = Drawing.Color.White
                Dim lblInformacion As New Label
                Dim ImgUpdateInformacion As New ImageButton
                Dim ImgDeleteInformacion As New ImageButton
                Dim salto As New LiteralControl("<br/><br/>")
                Dim salto1 As New LiteralControl("<br/><br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                lblInformacion.ForeColor = Drawing.Color.White
                lblInformacion.Text = item.Textopregunta
                pnlInformacion.Style.Add("margin-bottom", "10px")
                pnlInformacion.Style.Add("margin-top", "10px")
                pnlInformacion.BorderWidth = 1
                pnlInformacion.BorderColor = Drawing.Color.White
                ImgUpdateInformacion.ID = "ImgUpdate" & item.IdPregunta
                ImgUpdateInformacion.ImageUrl = "~/Images/list_16_.png"
                ImgUpdateInformacion.ToolTip = "Actualizar"
                ImgUpdateInformacion.Attributes.Add("IdPregunta", item.IdPregunta)
                ImgDeleteInformacion.ID = "ImgDelete" & item.IdPregunta
                ImgDeleteInformacion.ImageUrl = "~/Images/delete_16.png"
                ImgDeleteInformacion.ToolTip = "Eliminar"
                ImgDeleteInformacion.Attributes.Add("IdPregunta", item.IdPregunta)
                AddHandler ImgUpdateInformacion.Click, AddressOf actualizarPregunta
                AddHandler ImgDeleteInformacion.Click, AddressOf eliminarPregunta
                pnlInformacion.Controls.Add(lblOrden)
                pnlInformacion.Controls.Add(salto1)
                pnlInformacion.Controls.Add(lblInformacion)
                pnlInformacion.Controls.Add(salto)
                pnlInformacion.Controls.Add(ImgUpdateInformacion)
                pnlInformacion.Controls.Add(espacio)
                pnlInformacion.Controls.Add(espacio)
                If item.IdFija = False Then
                    pnlInformacion.Controls.Add(ImgDeleteInformacion)
                End If
                Me.pnlPreguntas.Controls.Add(pnlInformacion)
            End If

            If item.IdTipoPregunta = eTipoPregunta.Fecha Then
                Dim pnlFecha As New Panel
                Dim lblOrden As New Label
                lblOrden.Text = "Orden Pregunta: " & item.OrdenPregunta
                lblOrden.Font.Size = 11
                lblOrden.ForeColor = Drawing.Color.White
                Dim lblFecha As New Label
                Dim txtFecha As New TextBox
                Dim ImgUpdateFecha As New ImageButton
                Dim ImgDeleteFecha As New ImageButton
                Dim salto As New LiteralControl("<br/>")
                Dim salto1 As New LiteralControl("<br/><br/>")
                Dim salto2 As New LiteralControl("<br/><br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                lblFecha.ForeColor = Drawing.Color.White
                lblFecha.Text = item.Textopregunta
                txtFecha.TextMode = TextBoxMode.Date
                pnlFecha.Style.Add("margin-bottom", "10px")
                pnlFecha.Style.Add("margin-top", "10px")
                pnlFecha.BorderWidth = 1
                pnlFecha.BorderColor = Drawing.Color.White
                ImgUpdateFecha.ID = "ImgUpdate" & item.IdPregunta
                ImgUpdateFecha.ImageUrl = "~/Images/list_16_.png"
                ImgUpdateFecha.ToolTip = "Actualizar"
                ImgUpdateFecha.Attributes.Add("IdPregunta", item.IdPregunta)
                ImgDeleteFecha.ID = "ImgDelete" & item.IdPregunta
                ImgDeleteFecha.ImageUrl = "~/Images/delete_16.png"
                ImgDeleteFecha.ToolTip = "Eliminar"
                ImgDeleteFecha.Attributes.Add("IdPregunta", item.IdPregunta)
                AddHandler ImgUpdateFecha.Click, AddressOf actualizarPregunta
                AddHandler ImgDeleteFecha.Click, AddressOf eliminarPregunta
                pnlFecha.Controls.Add(lblOrden)
                pnlFecha.Controls.Add(salto2)
                pnlFecha.Controls.Add(lblFecha)
                pnlFecha.Controls.Add(salto)
                pnlFecha.Controls.Add(txtFecha)
                pnlFecha.Controls.Add(salto1)
                pnlFecha.Controls.Add(ImgUpdateFecha)
                pnlFecha.Controls.Add(espacio)
                pnlFecha.Controls.Add(espacio)
                If item.IdFija = False Then
                    pnlFecha.Controls.Add(ImgDeleteFecha)
                End If
                Me.pnlPreguntas.Controls.Add(pnlFecha)
            End If

            If item.IdTipoPregunta = eTipoPregunta.Hora Then
                Dim pnlHora As New Panel
                Dim lblOrden As New Label
                lblOrden.Text = "Orden Pregunta: " & item.OrdenPregunta
                lblOrden.Font.Size = 11
                lblOrden.ForeColor = Drawing.Color.White
                Dim lblHora As New Label
                Dim txtHora As New TextBox
                Dim ImgUpdateHora As New ImageButton
                Dim ImgDeleteHora As New ImageButton
                Dim salto As New LiteralControl("<br/>")
                Dim salto1 As New LiteralControl("<br/><br/>")
                Dim salto2 As New LiteralControl("<br/><br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                lblHora.ForeColor = Drawing.Color.White
                lblHora.Text = item.Textopregunta
                txtHora.TextMode = TextBoxMode.Time
                pnlHora.Style.Add("margin-bottom", "10px")
                pnlHora.Style.Add("margin-top", "10px")
                pnlHora.BorderWidth = 1
                pnlHora.BorderColor = Drawing.Color.White
                ImgUpdateHora.ID = "ImgUpdate" & item.IdPregunta
                ImgUpdateHora.ImageUrl = "~/Images/list_16_.png"
                ImgUpdateHora.ToolTip = "Actualizar"
                ImgUpdateHora.Attributes.Add("IdPregunta", item.IdPregunta)
                ImgDeleteHora.ID = "ImgDelete" & item.IdPregunta
                ImgDeleteHora.ImageUrl = "~/Images/delete_16.png"
                ImgDeleteHora.ToolTip = "Eliminar"
                ImgDeleteHora.Attributes.Add("IdPregunta", item.IdPregunta)
                AddHandler ImgUpdateHora.Click, AddressOf actualizarPregunta
                AddHandler ImgDeleteHora.Click, AddressOf eliminarPregunta
                pnlHora.Controls.Add(lblOrden)
                pnlHora.Controls.Add(salto2)
                pnlHora.Controls.Add(lblHora)
                pnlHora.Controls.Add(salto)
                pnlHora.Controls.Add(txtHora)
                pnlHora.Controls.Add(salto1)
                pnlHora.Controls.Add(ImgUpdateHora)
                pnlHora.Controls.Add(espacio)
                pnlHora.Controls.Add(espacio)
                If item.IdFija = False Then
                    pnlHora.Controls.Add(ImgDeleteHora)
                End If
                Me.pnlPreguntas.Controls.Add(pnlHora)
            End If

        Next

    End Sub

    Sub CreacionPreguntas()
        If ddlTipoPregunta.SelectedValue = eTipoPregunta.Titulo Then
            Me.pnlRespuestas.Visible = False
        ElseIf ddlTipoPregunta.SelectedValue = eTipoPregunta.TextoCorto Then
            Me.pnlRespuestas.Visible = False
        ElseIf ddlTipoPregunta.SelectedValue = eTipoPregunta.Parrafo Then
            Me.pnlRespuestas.Visible = False
        ElseIf ddlTipoPregunta.SelectedValue = eTipoPregunta.RespuestaUnica Then
            Me.pnlRespuestas.Visible = True
        ElseIf ddlTipoPregunta.SelectedValue = eTipoPregunta.RespuestaMultiple Then
            Me.pnlRespuestas.Visible = True
        ElseIf ddlTipoPregunta.SelectedValue = eTipoPregunta.ListaDesplegable Then
            Me.pnlRespuestas.Visible = True
        ElseIf ddlTipoPregunta.SelectedValue = eTipoPregunta.Informacion Then
            Me.pnlRespuestas.Visible = False
        ElseIf ddlTipoPregunta.SelectedValue = eTipoPregunta.Fecha Then
            Me.pnlRespuestas.Visible = False
        ElseIf ddlTipoPregunta.SelectedValue = eTipoPregunta.Hora Then
            Me.pnlRespuestas.Visible = False

        End If
    End Sub

    Protected Sub GuardarFiltro()
        Dim oCampoCuali As New CoreProject.CampoCualitativo

        Try

            Dim ent As New OP_Filtros
            ent.TipoFiltro = hfTipoFiltro.Value
            ent.IdTrabajo = hfIdTrabajo.Value
            ent.FechaInicio = txtFechaIni.Text
            ent.FechaFin = txtFechaFin.Text
            ent.Activo = True
            hfIdFiltro.Value = oCampoCuali.GuardarFiltros(ent)

            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
        Catch ex As Exception
            ShowNotification("Ha ocurrido un error al intentar ingresar el registro - " & ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        End Try
    End Sub

    Protected Sub GuardarPreguntaNombres()
        Dim oCampoCuali As New CoreProject.CampoCualitativo
        Dim Preguntas = oCampoCuali.ObtenerListaPreguntasFiltro(hfIdFiltro.Value, Nothing, Nothing)
        Dim maximo = (From x In Preguntas
                      Select x.OrdenPregunta).Max()
        If maximo Is Nothing Then maximo = 0

        Dim ent As New OP_Preguntas_Filtro

        ent.IdFiltro = hfIdFiltro.Value
        ent.TipoPregunta = eTipoPregunta.TextoCorto
        ent.OrdenPregunta = maximo + 1
        ent.Textopregunta = "NOMBRES Y APELLIDOS COMPLETOS:"
        ent.Respuestas = ""
        ent.Obligatoria = True
        ent.Fija = True
        ent.Campo = "Nombre"

        oCampoCuali.GuardarPreguntasFiltro(ent)

    End Sub

    Protected Sub GuardarPreguntaCC()
        Dim oCampoCuali As New CoreProject.CampoCualitativo
        Dim Preguntas = oCampoCuali.ObtenerListaPreguntasFiltro(hfIdFiltro.Value, Nothing, Nothing)
        Dim maximo = (From x In Preguntas
                      Select x.OrdenPregunta).Max()
        If maximo Is Nothing Then maximo = 0

        Dim ent As New OP_Preguntas_Filtro

        ent.IdFiltro = hfIdFiltro.Value
        ent.TipoPregunta = eTipoPregunta.TextoCorto
        ent.OrdenPregunta = maximo + 1
        ent.Textopregunta = "C.C.:"
        ent.Respuestas = ""
        ent.Obligatoria = True
        ent.Fija = True
        ent.Campo = "Cedula"

        oCampoCuali.GuardarPreguntasFiltro(ent)

    End Sub

    Protected Sub GuardarPreguntaCelular()
        Dim oCampoCuali As New CoreProject.CampoCualitativo
        Dim Preguntas = oCampoCuali.ObtenerListaPreguntasFiltro(hfIdFiltro.Value, Nothing, Nothing)
        Dim maximo = (From x In Preguntas
                      Select x.OrdenPregunta).Max()
        If maximo Is Nothing Then maximo = 0

        Dim ent As New OP_Preguntas_Filtro

        ent.IdFiltro = hfIdFiltro.Value
        ent.TipoPregunta = eTipoPregunta.TextoCorto
        ent.OrdenPregunta = maximo + 1
        ent.Textopregunta = "Celular:"
        ent.Respuestas = ""
        ent.Obligatoria = True
        ent.Fija = True
        ent.Campo = "Celular"

        oCampoCuali.GuardarPreguntasFiltro(ent)

    End Sub

    Protected Sub GuardarPreguntaDireccion()
        Dim oCampoCuali As New CoreProject.CampoCualitativo
        Dim Preguntas = oCampoCuali.ObtenerListaPreguntasFiltro(hfIdFiltro.Value, Nothing, Nothing)
        Dim maximo = (From x In Preguntas
                      Select x.OrdenPregunta).Max()
        If maximo Is Nothing Then maximo = 0

        Dim ent As New OP_Preguntas_Filtro

        ent.IdFiltro = hfIdFiltro.Value
        ent.TipoPregunta = eTipoPregunta.TextoCorto
        ent.OrdenPregunta = maximo + 1
        ent.Textopregunta = "Dirección:"
        ent.Respuestas = ""
        ent.Obligatoria = True
        ent.Fija = True
        ent.Campo = "Direccion"

        oCampoCuali.GuardarPreguntasFiltro(ent)

    End Sub

    Protected Sub GuardarPreguntaCiudad()
        Dim oCampoCuali As New CoreProject.CampoCualitativo
        Dim Preguntas = oCampoCuali.ObtenerListaPreguntasFiltro(hfIdFiltro.Value, Nothing, Nothing)
        Dim maximo = (From x In Preguntas
                      Select x.OrdenPregunta).Max()
        If maximo Is Nothing Then maximo = 0

        Dim ent As New OP_Preguntas_Filtro

        ent.IdFiltro = hfIdFiltro.Value
        ent.TipoPregunta = eTipoPregunta.RespuestaUnica
        ent.OrdenPregunta = maximo + 1
        ent.Textopregunta = "Ciudad:"
        ent.Respuestas = "Bogotá|"
        ent.Obligatoria = True
        ent.Fija = True
        ent.Campo = "Ciudad"

        oCampoCuali.GuardarPreguntasFiltro(ent)

    End Sub

    Protected Sub GuardarPreguntaBarrio()
        Dim oCampoCuali As New CoreProject.CampoCualitativo
        Dim Preguntas = oCampoCuali.ObtenerListaPreguntasFiltro(hfIdFiltro.Value, Nothing, Nothing)
        Dim maximo = (From x In Preguntas
                      Select x.OrdenPregunta).Max()
        If maximo Is Nothing Then maximo = 0

        Dim ent As New OP_Preguntas_Filtro

        ent.IdFiltro = hfIdFiltro.Value
        ent.TipoPregunta = eTipoPregunta.TextoCorto
        ent.OrdenPregunta = maximo + 1
        ent.Textopregunta = "Barrio:"
        ent.Respuestas = ""
        ent.Obligatoria = True
        ent.Fija = True
        ent.Campo = "Barrio"

        oCampoCuali.GuardarPreguntasFiltro(ent)

    End Sub

    Protected Sub GuardarPreguntaEdad()
        Dim oCampoCuali As New CoreProject.CampoCualitativo
        Dim Preguntas = oCampoCuali.ObtenerListaPreguntasFiltro(hfIdFiltro.Value, Nothing, Nothing)
        Dim maximo = (From x In Preguntas
                      Select x.OrdenPregunta).Max()
        If maximo Is Nothing Then maximo = 0

        Dim ent As New OP_Preguntas_Filtro

        ent.IdFiltro = hfIdFiltro.Value
        ent.TipoPregunta = eTipoPregunta.TextoCorto
        ent.OrdenPregunta = maximo + 1
        ent.Textopregunta = "Edad:"
        ent.Respuestas = ""
        ent.Obligatoria = True
        ent.Fija = True
        ent.Campo = "Edad"

        oCampoCuali.GuardarPreguntasFiltro(ent)

    End Sub

    Protected Sub GuardarPreguntaEstrato()
        Dim oCampoCuali As New CoreProject.CampoCualitativo
        Dim Preguntas = oCampoCuali.ObtenerListaPreguntasFiltro(hfIdFiltro.Value, Nothing, Nothing)
        Dim maximo = (From x In Preguntas
                      Select x.OrdenPregunta).Max()
        If maximo Is Nothing Then maximo = 0

        Dim ent As New OP_Preguntas_Filtro

        ent.IdFiltro = hfIdFiltro.Value
        ent.TipoPregunta = eTipoPregunta.RespuestaUnica
        ent.OrdenPregunta = maximo + 1
        ent.Textopregunta = "Estrato de los Recibos de los Servicios Públicos del lugar donde vive:"
        ent.Respuestas = "NSE 1|NSE 2|NSE 3|NSE 4|NSE 5|NSE 6|"
        ent.Obligatoria = True
        ent.Fija = True
        ent.Campo = "Estrato"

        oCampoCuali.GuardarPreguntasFiltro(ent)

    End Sub

    Protected Sub GuardarPreguntaReclutador()
        Dim oCampoCuali As New CoreProject.CampoCualitativo
        Dim Preguntas = oCampoCuali.ObtenerListaPreguntasFiltro(hfIdFiltro.Value, Nothing, Nothing)
        Dim maximo = (From x In Preguntas
                      Select x.OrdenPregunta).Max()
        If maximo Is Nothing Then maximo = 0

        Dim ent As New OP_Preguntas_Filtro

        ent.IdFiltro = hfIdFiltro.Value
        ent.TipoPregunta = eTipoPregunta.TextoCorto
        ent.OrdenPregunta = maximo + 1
        ent.Textopregunta = "Reclutador(a) o Persona quien lo invitó:"
        ent.Respuestas = ""
        ent.Obligatoria = True
        ent.Fija = True
        ent.Campo = "Reclutador"

        oCampoCuali.GuardarPreguntasFiltro(ent)

    End Sub

    Protected Sub GuardarPregunta()
        Dim oCampoCuali As New CoreProject.CampoCualitativo
        Dim Preguntas = oCampoCuali.ObtenerListaPreguntasFiltro(hfIdFiltro.Value, Nothing, Nothing)
        Dim maximo = (From x In Preguntas
                      Select x.OrdenPregunta).Max()
        Try

            Dim ent As New OP_Preguntas_Filtro

            If hfIdPregunta.Value > 0 Then
                ent = oCampoCuali.ObtenerPreguntasFitroxId(hfIdPregunta.Value)

                If txtOrden.Text <> ent.OrdenPregunta Then
                    For Each item In Preguntas
                        If txtOrden.Text = item.OrdenPregunta Then
                            oCampoCuali.ReOrdenarPreguntasFiltro(item.IdFiltro, ent.OrdenPregunta, txtOrden.Text)
                            Exit For
                        End If
                    Next
                End If

                ent.OrdenPregunta = txtOrden.Text
                ent.Fija = ent.Fija
                ent.Campo = ent.Campo
            Else
                ent.OrdenPregunta = maximo + 1
                ent.Fija = False
                ent.Campo = Nothing
            End If

            ent.IdFiltro = hfIdFiltro.Value

            Dim strRespuestas As String = ""

            ent.TipoPregunta = ddlTipoPregunta.SelectedValue
            ent.Textopregunta = txtTextoPregunta.Text

            For Each respuesta As ListItem In lstRespuestas.Items
                strRespuestas = strRespuestas & respuesta.Text & "|"
            Next

            ent.Respuestas = strRespuestas

            If chbObligatoria.Checked = True Then
                ent.Obligatoria = True
            Else
                ent.Obligatoria = False
            End If

            oCampoCuali.GuardarPreguntasFiltro(ent)
            Limpiar()
            CargarPreguntas()

            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            ShowNotification("Pregunta guardada correctamente", ShowNotifications.InfoNotification)

            ddlTipoPregunta.Focus()

        Catch ex As Exception
            ShowNotification("Ha ocurrido un error al intentar ingresar el registro - " & ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        End Try
    End Sub

    Public Sub actualizarPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Dim oCampoCuali As New CoreProject.CampoCualitativo
        Dim pregunta = oCampoCuali.ObtenerPreguntasFitroxId(CType(sender, ImageButton).Attributes.Item("IdPregunta"))

        hfIdPregunta.Value = pregunta.Id
        ddlTipoPregunta.SelectedValue = pregunta.TipoPregunta
        CreacionPreguntas()

        lblOrden.Visible = True
        txtOrden.Visible = True
        txtOrden.Text = pregunta.OrdenPregunta

        txtTextoPregunta.Text = pregunta.Textopregunta

        If pregunta.TipoPregunta = eTipoPregunta.ListaDesplegable Or pregunta.TipoPregunta = eTipoPregunta.RespuestaMultiple Or pregunta.TipoPregunta = eTipoPregunta.RespuestaUnica Then
            pnlRespuestas.Visible = True
            lstRespuestas.Visible = True
            btnRemoveRespuestas.Visible = True

            Dim TestArray() As String = Split(pregunta.Respuestas, "|")
            For i As Integer = 0 To TestArray.Length - 1
                If TestArray(i) <> "" Then
                    lstRespuestas.Items.Add(TestArray(i))
                End If
            Next

        Else
            pnlRespuestas.Visible = False
            lstRespuestas.Visible = False
            btnRemoveRespuestas.Visible = False

        End If

        If pregunta.Obligatoria = True Then
            chbObligatoria.Checked = True
        Else
            chbObligatoria.Checked = False
        End If

        CargarPreguntas()

        txtTextoPregunta.Focus()

    End Sub

    Public Sub eliminarPregunta(sender As Object, e As EventArgs)
        Dim oCampoCuali As New CoreProject.CampoCualitativo
        Dim pregunta = oCampoCuali.ObtenerPreguntasFitroxId(CType(sender, ImageButton).Attributes.Item("IdPregunta"))
        oCampoCuali.ActualizarOrdenPreguntasFiltro(pregunta.IdFiltro, pregunta.OrdenPregunta)

        oCampoCuali.EliminarPreguntaFitro(pregunta.Id)

        CargarPreguntas()

        ShowNotification("Pregunta eliminada correctamente", ShowNotifications.InfoNotification)

        ddlTipoPregunta.Focus()

    End Sub

    Protected Sub ddlTipoPregunta_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoPregunta.SelectedIndexChanged
        CreacionPreguntas()
        CargarPreguntas()
    End Sub

    Public Sub generarLink()
        lblLink.InnerText = WebMatrix.Util.obtenerUrlRaiz() & "/OP_Cualitativo/VisualizadorFiltros.aspx?trabajoId=" & hfIdTrabajo.Value & "&idfiltro=" & hfIdFiltro.Value
    End Sub

#End Region

End Class