Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Public Class VisualizadorFiltros
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

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("trabajoId") IsNot Nothing Then
                Dim idTrabajo As Int64 = Int64.Parse(Request.QueryString("trabajoId").ToString)
                hfIdTrabajo.Value = idTrabajo
            End If

            If Request.QueryString("idfiltro") IsNot Nothing Then
                Dim oCampo As New CoreProject.CampoCualitativo
                Dim idFiltro As Int64 = Int64.Parse(Request.QueryString("idfiltro").ToString)
                hfIdFiltro.Value = idFiltro

                Dim filtro = oCampo.ObtenerFiltroxId(hfIdFiltro.Value)
                If filtro.TipoFiltro = 1 Then
                    Me.lblFiltro.InnerText = "Filtro de Reclutamiento"
                    Me.pnlBusqueda.Visible = False
                    Me.pnlPreguntas.Visible = True
                    Me.btnGuardar.Visible = True
                ElseIf filtro.TipoFiltro = 2 Then
                    Me.lblFiltro.InnerText = "Filtro de Asitencia"
                    Me.pnlBusqueda.Visible = True
                    Me.pnlPreguntas.Visible = False
                    Me.btnGuardar.Visible = False
                End If
            Else
                Response.Redirect("Trabajos.aspx")
            End If
            CargarLabelTrabajo()
            pnlGracias.Visible = False
        End If
        CargarPreguntas()
    End Sub

    Public Sub CargarPreguntas()
        pnlPreguntas.Controls.Clear()
        cargarPreguntasFiltro()
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        GuardarRespuestas()
        Limpiar()
    End Sub

#End Region

#Region "Funciones y Metodos"


    Sub CargarLabelTrabajo()
        Dim oTrabajo As New Trabajo
    End Sub


    Public Sub cargarPreguntasFiltro()
        Dim oCampo As New CoreProject.CampoCualitativo
        Dim visualizar = oCampo.ObtenerListaPreguntasFiltro(hfIdFiltro.Value, Nothing, Nothing)

        For Each item In visualizar
            If item.IdTipoPregunta = eTipoPregunta.Titulo Then
                Dim pnlTitulo As New Panel
                Dim lbltitulo As New Label
                Dim salto As New LiteralControl("<br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                lbltitulo.ForeColor = Drawing.Color.White
                lbltitulo.Font.Bold = True
                lbltitulo.Font.Size = 18
                lbltitulo.Text = item.Textopregunta
                pnlTitulo.Style.Add("margin-bottom", "10px")
                pnlTitulo.Style.Add("margin-top", "10px")
                pnlTitulo.CssClass = "divBorder"
                pnlTitulo.Controls.Add(lbltitulo)
                pnlTitulo.Controls.Add(salto)
                Me.pnlPreguntas.Controls.Add(pnlTitulo)
            End If

            If item.IdTipoPregunta = eTipoPregunta.TextoCorto Then
                Dim pnlTextcorto As New Panel
                Dim lblTextoCorto As New Label
                Dim txtTextoCorto As New TextBox
                Dim salto As New LiteralControl("<br/>")
                Dim salto1 As New LiteralControl("<br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")

                lblTextoCorto.ForeColor = Drawing.Color.White
                lblTextoCorto.Text = item.Textopregunta
                txtTextoCorto.ID = "Ctr_" & item.IdPregunta
                txtTextoCorto.Width = 400
                pnlTextcorto.Style.Add("margin-bottom", "10px")
                pnlTextcorto.Style.Add("margin-top", "10px")
                pnlTextcorto.CssClass = "divBorder"
                pnlTextcorto.Controls.Add(lblTextoCorto)
                pnlTextcorto.Controls.Add(salto)
                pnlTextcorto.Controls.Add(txtTextoCorto)
                pnlTextcorto.Controls.Add(salto1)

                If item.IdObligatoria = True Then
                    Dim rfvTextoCorto As New RequiredFieldValidator
                    rfvTextoCorto.ID = "rfv_" & item.IdPregunta
                    rfvTextoCorto.ControlToValidate = "Ctr_" & item.IdPregunta
                    rfvTextoCorto.ErrorMessage = "Esta pregunta es obligatoria"
                    rfvTextoCorto.ForeColor = Drawing.Color.Red
                    pnlTextcorto.Controls.Add(rfvTextoCorto)
                End If

                Me.pnlPreguntas.Controls.Add(pnlTextcorto)
            End If

            If item.IdTipoPregunta = eTipoPregunta.Parrafo Then
                Dim pnlParrafo As New Panel
                Dim lblParrafo As New Label
                Dim txtParrafo As New TextBox
                Dim salto As New LiteralControl("<br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                txtParrafo.ID = "Ctr_" & item.IdPregunta
                txtParrafo.TextMode = TextBoxMode.MultiLine
                txtParrafo.Width = 600
                lblParrafo.ForeColor = Drawing.Color.White
                lblParrafo.Text = item.Textopregunta
                pnlParrafo.Style.Add("margin-bottom", "10px")
                pnlParrafo.Style.Add("margin-top", "10px")
                pnlParrafo.CssClass = "divBorder"
                pnlParrafo.Controls.Add(lblParrafo)
                pnlParrafo.Controls.Add(txtParrafo)
                pnlParrafo.Controls.Add(salto)

                If item.IdObligatoria = True Then
                    Dim rfvParrafo As New RequiredFieldValidator
                    rfvParrafo.ID = "rfv_" & item.IdPregunta
                    rfvParrafo.ControlToValidate = "Ctr_" & item.IdPregunta
                    rfvParrafo.ErrorMessage = "Esta pregunta es obligatoria"
                    rfvParrafo.ForeColor = Drawing.Color.Red
                    pnlParrafo.Controls.Add(rfvParrafo)
                End If

                Me.pnlPreguntas.Controls.Add(pnlParrafo)
            End If

            If item.IdTipoPregunta = eTipoPregunta.RespuestaUnica Then
                Dim pnlUnica As New Panel
                Dim lblUnica As New Label
                Dim rblUnica As New RadioButtonList
                Dim salto As New LiteralControl("<br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                lblUnica.ForeColor = Drawing.Color.White
                lblUnica.Text = item.Textopregunta
                rblUnica.ID = "Ctr_" & item.IdPregunta
                rblUnica.CssClass = "table"
                pnlUnica.Style.Add("margin-bottom", "10px")
                pnlUnica.Style.Add("margin-top", "10px")
                pnlUnica.CssClass = "divBorder"
                Dim TestArray() As String = Split(item.Respuestas, "|")
                For i As Integer = 0 To TestArray.Length - 1
                    If TestArray(i) <> "" Then
                        rblUnica.Items.Add(TestArray(i))
                    End If
                Next
                pnlUnica.Controls.Add(lblUnica)
                pnlUnica.Controls.Add(rblUnica)
                pnlUnica.Controls.Add(salto)

                If item.IdObligatoria = True Then
                    Dim rfvUnica As New RequiredFieldValidator
                    rfvUnica.ID = "rfv_" & item.IdPregunta
                    rfvUnica.ControlToValidate = "Ctr_" & item.IdPregunta
                    rfvUnica.ErrorMessage = "Esta pregunta es obligatoria"
                    rfvUnica.ForeColor = Drawing.Color.Red
                    pnlUnica.Controls.Add(rfvUnica)
                End If

                Me.pnlPreguntas.Controls.Add(pnlUnica)
            End If

            If item.IdTipoPregunta = eTipoPregunta.RespuestaMultiple Then
                Dim pnlMultiple As New Panel
                Dim lblMultiple As New Label
                Dim chblMultiple As New CheckBoxList
                Dim salto As New LiteralControl("<br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                chblMultiple.ID = "Ctr_" & item.IdPregunta
                chblMultiple.CssClass = "table"
                chblMultiple.RepeatLayout = RepeatLayout.Table
                lblMultiple.ForeColor = Drawing.Color.White
                lblMultiple.Text = item.Textopregunta
                pnlMultiple.Style.Add("margin-bottom", "10px")
                pnlMultiple.Style.Add("margin-top", "10px")
                pnlMultiple.CssClass = "divBorder"

                Dim TestArray() As String = Split(item.Respuestas, "|")
                For i As Integer = 0 To TestArray.Length - 1
                    If TestArray(i) <> "" Then
                        chblMultiple.Items.Add(TestArray(i))
                    End If
                Next
                pnlMultiple.Controls.Add(lblMultiple)
                pnlMultiple.Controls.Add(chblMultiple)
                pnlMultiple.Controls.Add(salto)

                If item.IdObligatoria = True Then
                    Dim cvMultiple As New CustomValidator
                    cvMultiple.ID = "cv_" & item.IdPregunta
                    cvMultiple.ClientValidationFunction = "verifyCheckboxList"
                    cvMultiple.ErrorMessage = "Esta pregunta es obligatoria"
                    cvMultiple.ForeColor = Drawing.Color.Red
                    pnlMultiple.Controls.Add(cvMultiple)
                End If

                Me.pnlPreguntas.Controls.Add(pnlMultiple)
            End If

            If item.IdTipoPregunta = eTipoPregunta.ListaDesplegable Then
                Dim pnlLista As New Panel
                Dim lblLista As New Label
                Dim ddlLista As New DropDownList
                Dim salto As New LiteralControl("<br/>")
                Dim salto1 As New LiteralControl("<br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                ddlLista.ID = "Ctr_" & item.IdPregunta
                ddlLista.CssClass = "table"
                ddlLista.Width = 400
                lblLista.ForeColor = Drawing.Color.White
                lblLista.Text = item.Textopregunta
                pnlLista.Style.Add("margin-bottom", "10px")
                pnlLista.Style.Add("margin-top", "10px")
                pnlLista.CssClass = "divBorder"
                Dim TestArray() As String = Split(item.Respuestas, "|")
                For i As Integer = 0 To TestArray.Length - 1
                    If TestArray(i) <> "" Then
                        ddlLista.Items.Add(TestArray(i))
                    End If
                Next
                pnlLista.Controls.Add(lblLista)
                pnlLista.Controls.Add(salto)
                pnlLista.Controls.Add(ddlLista)
                pnlLista.Controls.Add(salto1)

                If item.IdObligatoria = True Then
                    Dim rfvLista As New RequiredFieldValidator
                    rfvLista.ID = "rfv_" & item.IdPregunta
                    rfvLista.ControlToValidate = "Ctr_" & item.IdPregunta
                    rfvLista.ErrorMessage = "Esta pregunta es obligatoria"
                    rfvLista.ForeColor = Drawing.Color.Red
                    pnlLista.Controls.Add(rfvLista)
                End If

                Me.pnlPreguntas.Controls.Add(pnlLista)
            End If

            If item.IdTipoPregunta = eTipoPregunta.Informacion Then
                Dim pnlInformacion As New Panel
                Dim lblInformacion As New Label
                Dim salto As New LiteralControl("<br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                lblInformacion.ForeColor = Drawing.Color.White
                lblInformacion.Text = item.Textopregunta
                pnlInformacion.Style.Add("margin-bottom", "10px")
                pnlInformacion.Style.Add("margin-top", "10px")
                pnlInformacion.CssClass = "divBorder"
                pnlInformacion.Controls.Add(lblInformacion)
                pnlInformacion.Controls.Add(salto)
                Me.pnlPreguntas.Controls.Add(pnlInformacion)
            End If

            If item.IdTipoPregunta = eTipoPregunta.Fecha Then
                Dim pnlFecha As New Panel
                Dim lblFecha As New Label
                Dim txtFecha As New TextBox
                Dim salto As New LiteralControl("<br/>")
                Dim salto1 As New LiteralControl("<br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                lblFecha.ForeColor = Drawing.Color.White
                lblFecha.Text = item.Textopregunta
                txtFecha.ID = "Ctr_" & item.IdPregunta
                txtFecha.TextMode = TextBoxMode.Date
                pnlFecha.Style.Add("margin-bottom", "10px")
                pnlFecha.Style.Add("margin-top", "10px")
                pnlFecha.CssClass = "divBorder"
                pnlFecha.Controls.Add(lblFecha)
                pnlFecha.Controls.Add(salto)
                pnlFecha.Controls.Add(txtFecha)
                pnlFecha.Controls.Add(salto1)

                If item.IdObligatoria = True Then
                    Dim rfvFecha As New RequiredFieldValidator
                    rfvFecha.ID = "rfv_" & item.IdPregunta
                    rfvFecha.ControlToValidate = "Ctr_" & item.IdPregunta
                    rfvFecha.ErrorMessage = "Esta pregunta es obligatoria"
                    rfvFecha.ForeColor = Drawing.Color.Red
                    pnlFecha.Controls.Add(rfvFecha)
                End If

                Me.pnlPreguntas.Controls.Add(pnlFecha)
            End If

            If item.IdTipoPregunta = eTipoPregunta.Hora Then
                Dim pnlHora As New Panel
                Dim lblHora As New Label
                Dim txtHora As New TextBox
                Dim salto As New LiteralControl("<br/>")
                Dim salto1 As New LiteralControl("<br/>")
                Dim espacio As New LiteralControl("&nbsp;&nbsp;&nbsp;")
                lblHora.ForeColor = Drawing.Color.White
                lblHora.Text = item.Textopregunta
                txtHora.ID = "Ctr_" & item.IdPregunta
                txtHora.TextMode = TextBoxMode.Time
                pnlHora.Style.Add("margin-bottom", "10px")
                pnlHora.Style.Add("margin-top", "10px")
                pnlHora.CssClass = "divBorder"
                pnlHora.Controls.Add(lblHora)
                pnlHora.Controls.Add(salto)
                pnlHora.Controls.Add(txtHora)
                pnlHora.Controls.Add(salto1)

                If item.IdObligatoria = True Then
                    Dim rfvHora As New RequiredFieldValidator
                    rfvHora.ID = "rfv_" & item.IdPregunta
                    rfvHora.ControlToValidate = "Ctr_" & item.IdPregunta
                    rfvHora.ErrorMessage = "Esta pregunta es obligatoria"
                    rfvHora.ForeColor = Drawing.Color.Red
                    pnlHora.Controls.Add(rfvHora)
                End If

                Me.pnlPreguntas.Controls.Add(pnlHora)
            End If

        Next

    End Sub


    Protected Sub GuardarRespuestas()
        Dim oCampo As New CoreProject.CampoCualitativo
        Dim visualizar = oCampo.ObtenerListaPreguntasFiltro(hfIdFiltro.Value, Nothing, Nothing)
        Dim RespMaestra As New OP_Respuestas_Filtro_Maestro
        Dim lstRespDetalle As New List(Of OP_Respuestas_Filtro_Detalle)
        Dim eLog As New OP_LogRespuestas_Filtro

        Try
            For Each item In visualizar

                If item.IdFija = True Then
                    RespMaestra = ObtenerRespuestaMaestro(item)
                ElseIf item.IdFija = False And (item.IdTipoPregunta <> eTipoPregunta.Informacion And item.IdTipoPregunta <> eTipoPregunta.Titulo) Then
                    lstRespDetalle.Add(New OP_Respuestas_Filtro_Detalle With {.IdPregunta = item.IdPregunta, .Respuesta = obtenerValoresControl(item, pnlPreguntas)})
                End If

            Next

            RespMaestra.IdFiltro = hfIdFiltro.Value
            RespMaestra.Fecha = DateTime.Now
            RespMaestra.Estado = 1
            hfIdRespuesta.Value = oCampo.GuardarRespuestasFiltroMaestro(RespMaestra)

            For i As Integer = 0 To lstRespDetalle.Count - 1
                Dim RespDetalle As New OP_Respuestas_Filtro_Detalle
                RespDetalle.IdRespuesta = hfIdRespuesta.Value
                RespDetalle.IdPregunta = lstRespDetalle.Item(i).IdPregunta
                RespDetalle.Respuesta = lstRespDetalle.Item(i).Respuesta
                oCampo.GuardarRespuestasFiltroDetalle(RespDetalle)
            Next

            Dim itemFiltro = oCampo.ObtenerRespuestasFitroMaestroxId(hfIdRespuesta.Value)
            eLog.IdRespuesta = hfIdRespuesta.Value
            eLog.Estado = 1
            eLog.Observacion = ""
            eLog.Fecha = Date.UtcNow.AddHours(-5)
            eLog.Fecha = Date.UtcNow.AddHours(-5)
            eLog.Usuario = itemFiltro.Cedula
            oCampo.GuardarLogRespuestasFiltro(eLog)

            Limpiar()
            CargarPreguntas()

            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            ShowNotification("Respuestas guardadas correctamente", ShowNotifications.InfoNotification)


        Catch ex As Exception
            ShowNotification("Ha ocurrido un error al intentar ingresar las respuestas - " & ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        End Try
    End Sub

    Public Sub Limpiar()
        Dim oCampo As New CoreProject.CampoCualitativo
        Dim visualizar = oCampo.ObtenerListaPreguntasFiltro(hfIdFiltro.Value, Nothing, Nothing)

        For Each item In visualizar
            limpiarControles(item, pnlPreguntas)
        Next

        Session("SesionRespMaestra") = Nothing
        hfIdRespuesta.Value = "0"
        txtCedula.Text = ""
        pnlPreguntas.Visible = False
        pnlGuardar.Visible = False
        pnlGracias.Visible = True

    End Sub

    Function obtenerValoresControl(ByVal pregunta As OP_Preguntas_Filtro_Get_Result, ByVal contenedor As Panel) As String
        Dim valor As String = ""
        Select Case pregunta.IdTipoPregunta
            Case eTipoPregunta.TextoCorto, eTipoPregunta.Parrafo, eTipoPregunta.Fecha, eTipoPregunta.Hora
                Dim ctr As TextBox
                ctr = CType(contenedor.FindControl("Ctr_" & pregunta.IdPregunta), TextBox)
                If ctr.Text Is Nothing Then
                    valor = ""
                Else
                    valor = ctr.Text
                End If
            Case eTipoPregunta.RespuestaUnica
                Dim ctr As RadioButtonList
                ctr = CType(contenedor.FindControl("Ctr_" & pregunta.IdPregunta), RadioButtonList)
                If ctr.SelectedItem Is Nothing Then
                    valor = ""
                Else
                    valor = ctr.SelectedItem.Text
                End If
            Case eTipoPregunta.RespuestaMultiple
                Dim ctr As CheckBoxList
                ctr = CType(contenedor.FindControl("Ctr_" & pregunta.IdPregunta), CheckBoxList)
                Dim allChecked As Integer = ctr.Items.Cast(Of ListItem)().Where(Function(i) i.Selected).Count
                If allChecked > 0 Then
                    Dim strRespuestas As String = ""
                    For Each respuesta As ListItem In ctr.Items
                        If respuesta.Selected = True Then
                            strRespuestas = strRespuestas & respuesta.Text & "|"
                        End If
                    Next
                    valor = strRespuestas
                Else
                    valor = ""
                End If
            Case eTipoPregunta.ListaDesplegable
                Dim ctr As DropDownList
                ctr = CType(contenedor.FindControl("Ctr_" & pregunta.IdPregunta), DropDownList)
                If ctr.SelectedItem Is Nothing Then
                    valor = ""
                Else
                    valor = ctr.SelectedItem.Text
                End If
        End Select
        Return valor
    End Function

    Function ObtenerRespuestaMaestro(ByVal pregunta As OP_Preguntas_Filtro_Get_Result) As OP_Respuestas_Filtro_Maestro
        Dim RespMaestra As New OP_Respuestas_Filtro_Maestro
        If Not IsNothing(Session("SesionRespMaestra")) Then RespMaestra = Session("SesionRespMaestra")

        Select Case pregunta.Campo
            Case "Nombre"
                RespMaestra.Nombre = obtenerValoresControl(pregunta, pnlPreguntas)
                Session("SesionRespMaestra") = RespMaestra
            Case "Cedula"
                RespMaestra.Cedula = obtenerValoresControl(pregunta, pnlPreguntas)
                Session("SesionRespMaestra") = RespMaestra
            Case "Celular"
                RespMaestra.Celular = obtenerValoresControl(pregunta, pnlPreguntas)
                Session("SesionRespMaestra") = RespMaestra
            Case "Direccion"
                RespMaestra.Direccion = obtenerValoresControl(pregunta, pnlPreguntas)
                Session("SesionRespMaestra") = RespMaestra
            Case "Ciudad"
                RespMaestra.Ciudad = obtenerValoresControl(pregunta, pnlPreguntas)
                Session("SesionRespMaestra") = RespMaestra
            Case "Barrio"
                RespMaestra.Barrio = obtenerValoresControl(pregunta, pnlPreguntas)
                Session("SesionRespMaestra") = RespMaestra
            Case "Edad"
                RespMaestra.Edad = obtenerValoresControl(pregunta, pnlPreguntas)
                Session("SesionRespMaestra") = RespMaestra
            Case "Estrato"
                RespMaestra.Estrato = obtenerValoresControl(pregunta, pnlPreguntas)
                Session("SesionRespMaestra") = RespMaestra
            Case "Reclutador"
                RespMaestra.Reclutador = obtenerValoresControl(pregunta, pnlPreguntas)
                Session("SesionRespMaestra") = RespMaestra
        End Select

        Return Session("SesionRespMaestra")

    End Function

    Public Sub ObtenerRespuestasPreguntasMaestro()
        Dim oCampo As New CoreProject.CampoCualitativo
        Dim Respuesta = oCampo.ObtenerRespuestasMaestroFiltroReclutamiento(hfIdTrabajo.Value, txtCedula.Text)
        Dim visualizar = oCampo.ObtenerListaPreguntasFiltro(hfIdFiltro.Value, Nothing, Nothing)
        For Each item In visualizar
            If item.Campo = "Nombre" Then
                Dim ctr As TextBox
                ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), TextBox)
                If Not Respuesta.Nombre Is Nothing Then ctr.Text = Respuesta.Nombre
            End If
            If item.Campo = "Cedula" Then
                Dim ctr As TextBox
                ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), TextBox)
                If Not Respuesta.Cedula Is Nothing Then ctr.Text = Respuesta.Cedula
            End If
            If item.Campo = "Celular" Then
                Dim ctr As TextBox
                ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), TextBox)
                If Not Respuesta.Celular Is Nothing Then ctr.Text = Respuesta.Celular
            End If
            If item.Campo = "Direccion" Then
                Dim ctr As TextBox
                ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), TextBox)
                If Not Respuesta.Direccion Is Nothing Then ctr.Text = Respuesta.Direccion
            End If
            If item.Campo = "Ciudad" Then
                If item.IdTipoPregunta = eTipoPregunta.TextoCorto Or item.IdTipoPregunta = eTipoPregunta.Parrafo Then
                    Dim ctr As TextBox
                    ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), TextBox)
                    If Not Respuesta.Ciudad Is Nothing Then ctr.Text = Respuesta.Ciudad
                ElseIf item.IdTipoPregunta = eTipoPregunta.RespuestaUnica Then
                    Dim ctr As RadioButtonList
                    ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), RadioButtonList)
                    If Not Respuesta.Ciudad Is Nothing Then ctr.SelectedValue = Respuesta.Ciudad
                ElseIf item.IdTipoPregunta = eTipoPregunta.ListaDesplegable Then
                    Dim ctr As DropDownList
                    ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), DropDownList)
                    If Not Respuesta.Ciudad Is Nothing Then ctr.SelectedValue = Respuesta.Ciudad
                End If
            End If
            If item.Campo = "Barrio" Then
                Dim ctr As TextBox
                ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), TextBox)
                If Not Respuesta.Barrio Is Nothing Then ctr.Text = Respuesta.Barrio
            End If
            If item.Campo = "Edad" Then
                If item.IdTipoPregunta = eTipoPregunta.TextoCorto Or item.IdTipoPregunta = eTipoPregunta.Parrafo Then
                    Dim ctr As TextBox
                    ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), TextBox)
                    If Not Respuesta.Edad Is Nothing Then ctr.Text = Respuesta.Edad
                ElseIf item.IdTipoPregunta = eTipoPregunta.RespuestaUnica Then
                    Dim ctr As RadioButtonList
                    ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), RadioButtonList)
                    If Not Respuesta.Edad Is Nothing Then ctr.SelectedValue = Respuesta.Edad
                ElseIf item.IdTipoPregunta = eTipoPregunta.ListaDesplegable Then
                    Dim ctr As DropDownList
                    ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), DropDownList)
                    If Not Respuesta.Edad Is Nothing Then ctr.SelectedValue = Respuesta.Edad
                End If
            End If
            If item.Campo = "Estrato" Then
                If item.IdTipoPregunta = eTipoPregunta.TextoCorto Or item.IdTipoPregunta = eTipoPregunta.Parrafo Then
                    Dim ctr As TextBox
                    ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), TextBox)
                    If Not Respuesta.Estrato Is Nothing Then ctr.Text = Respuesta.Estrato
                ElseIf item.IdTipoPregunta = eTipoPregunta.RespuestaUnica Then
                    Dim ctr As RadioButtonList
                    ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), RadioButtonList)
                    If Not Respuesta.Estrato Is Nothing Then ctr.SelectedValue = Respuesta.Estrato
                ElseIf item.IdTipoPregunta = eTipoPregunta.ListaDesplegable Then
                    Dim ctr As DropDownList
                    ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), DropDownList)
                    If Not Respuesta.Estrato Is Nothing Then ctr.SelectedValue = Respuesta.Estrato
                End If
            End If
            If item.Campo = "Reclutador" Then
                If item.IdTipoPregunta = eTipoPregunta.TextoCorto Or item.IdTipoPregunta = eTipoPregunta.Parrafo Then
                    Dim ctr As TextBox
                    ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), TextBox)
                    If Not Respuesta.Reclutador Is Nothing Then ctr.Text = Respuesta.Reclutador
                ElseIf item.IdTipoPregunta = eTipoPregunta.RespuestaUnica Then
                    Dim ctr As RadioButtonList
                    ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), RadioButtonList)
                    If Not Respuesta.Reclutador Is Nothing Then ctr.SelectedValue = Respuesta.Reclutador
                ElseIf item.IdTipoPregunta = eTipoPregunta.ListaDesplegable Then
                    Dim ctr As DropDownList
                    ctr = CType(pnlPreguntas.FindControl("Ctr_" & item.IdPregunta), DropDownList)
                    If Not Respuesta.Reclutador Is Nothing Then ctr.SelectedValue = Respuesta.Reclutador
                End If
            End If
        Next

    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim oCampo As New CoreProject.CampoCualitativo
        Dim Respuesta = oCampo.ObtenerRespuestasMaestroFiltroReclutamiento(hfIdTrabajo.Value, txtCedula.Text)
        If Not Respuesta Is Nothing Then
            Me.lblErrorMessage.Visible = False
            Me.pnlPreguntas.Visible = True
            Me.btnGuardar.Visible = True
            ObtenerRespuestasPreguntasMaestro()
        Else
            Me.lblErrorMessage.Visible = True
            Me.pnlPreguntas.Visible = False
            Me.btnGuardar.Visible = False
        End If
    End Sub

    Sub limpiarControles(ByVal pregunta As OP_Preguntas_Filtro_Get_Result, ByVal contenedor As Panel)
        Select Case pregunta.IdTipoPregunta
            Case eTipoPregunta.TextoCorto, eTipoPregunta.Parrafo, eTipoPregunta.Fecha, eTipoPregunta.Hora
                Dim ctr As TextBox
                ctr = CType(contenedor.FindControl("Ctr_" & pregunta.IdPregunta), TextBox)
                ctr.Text = ""
            Case eTipoPregunta.RespuestaUnica
                Dim ctr As RadioButtonList
                ctr = CType(contenedor.FindControl("Ctr_" & pregunta.IdPregunta), RadioButtonList)
                ctr.ClearSelection()
            Case eTipoPregunta.RespuestaMultiple
                Dim ctr As CheckBoxList
                ctr = CType(contenedor.FindControl("Ctr_" & pregunta.IdPregunta), CheckBoxList)
                ctr.ClearSelection()
            Case eTipoPregunta.ListaDesplegable
                Dim ctr As DropDownList
                ctr = CType(contenedor.FindControl("Ctr_" & pregunta.IdPregunta), DropDownList)
                ctr.ClearSelection()
        End Select
    End Sub

#End Region

End Class