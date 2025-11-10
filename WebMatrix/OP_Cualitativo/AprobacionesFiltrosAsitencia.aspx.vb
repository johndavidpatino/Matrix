Imports System.IO
Imports CoreProject
Imports CoreProject.OP
Imports WebMatrix.Util
Imports ClosedXML.Excel

Public Class AprobacionesFiltrosAsitencia
    Inherits System.Web.UI.Page

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

    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            Dim o As New Trabajo
            Dim oFiltro As New CoreProject.CampoCualitativo

            If Request.QueryString("trabajoId") Then
                hfIdTrabajo.Value = Request.QueryString("trabajoId").ToString
                Trabajo = o.DevolverxID(hfIdTrabajo.Value)
                hfIdProyecto.Value = Trabajo.ProyectoId

                Dim filtro = oFiltro.ObtenerFiltroxTrabajoYTipo(hfIdTrabajo.Value, 1)
                hfIdFiltroReclutamiento.Value = filtro.Id
            End If

            If Request.QueryString("idfiltro") IsNot Nothing Then
                hfIdFiltro.Value = Request.QueryString("idfiltro").ToString
            End If

            If Request.QueryString("py") IsNot Nothing Then
                hfPY.Value = "1"
            Else
                hfPY.Value = "0"
            End If

            If hfPY.Value = "1" Then
                lnkProyecto.PostBackUrl = "../PY_Proyectos/TrabajosCualitativos.aspx?ProyectoId=" & hfIdProyecto.Value
            Else
                lnkProyecto.PostBackUrl = "../OP_Cualitativo/Trabajos.aspx"
            End If

            CargarLabelTrabajo()
            CargarMaestroReclutamiento(hfIdFiltroReclutamiento.Value)
            CargarRespuestasFiltrosMaestro(hfIdFiltro.Value)
        End If


        ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnImgExportarInforme)

    End Sub

    Private Sub gvFiltro_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvFiltro.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If CType(e.Row.DataItem, OP_Respuestas_Filtro_Maestro_Get_Result).IdEstado = 1 Then
                DirectCast(e.Row.FindControl("imgAprobacion"), ImageButton).Visible = True
            ElseIf CType(e.Row.DataItem, OP_Respuestas_Filtro_Maestro_Get_Result).IdEstado = 2 Then
                DirectCast(e.Row.FindControl("imgAprobacion"), ImageButton).Visible = False
            ElseIf CType(e.Row.DataItem, OP_Respuestas_Filtro_Maestro_Get_Result).IdEstado = 3 Then
                DirectCast(e.Row.FindControl("imgAprobacion"), ImageButton).Visible = False
            ElseIf CType(e.Row.DataItem, OP_Respuestas_Filtro_Maestro_Get_Result).IdEstado = 4 Then
                DirectCast(e.Row.FindControl("imgAprobacion"), ImageButton).Visible = False
            End If
        End If
    End Sub

    Private Sub gvRespuestas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvRespuestas.RowCommand
        hfIdRespuestaReclutamiento.Value = Me.gvRespuestas.DataKeys(CInt(e.CommandArgument))("Id")
        CargarDetalleReclutamiento(hfIdRespuestaReclutamiento.Value)
    End Sub
    Private Sub gvRespuestas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvRespuestas.PageIndexChanging
        gvRespuestas.PageIndex = e.NewPageIndex
        CargarMaestroReclutamiento(hfIdFiltroReclutamiento.Value)
    End Sub

    Private Sub gvFiltro_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvFiltro.RowCommand
        hfIdRespuesta.Value = Me.gvFiltro.DataKeys(CInt(e.CommandArgument))("Id")
        CargarRespuestasFiltrosDetalle(hfIdRespuesta.Value)

    End Sub
    Private Sub gvFiltro_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvFiltro.PageIndexChanging
        gvFiltro.PageIndex = e.NewPageIndex
        CargarRespuestasFiltrosMaestro(hfIdFiltro.Value)
    End Sub

    Sub CargarLabelTrabajo()
        Dim oTrabajo As New Trabajo
        lblTrabajo.Text = oTrabajo.obtenerXId(hfIdTrabajo.Value).JobBook & " " & oTrabajo.obtenerXId(hfIdTrabajo.Value).NombreTrabajo
    End Sub

    Sub CargarMaestroReclutamiento(ByVal idFiltro As Int64)
        Dim o As New CoreProject.CampoCualitativo
        gvRespuestas.DataSource = o.ObtenerRespuestasFiltroMaestro(idFiltro)
        gvRespuestas.DataBind()
    End Sub

    Sub CargarRespuestasFiltrosMaestro(ByVal idFiltro As Int64)
        Dim o As New CoreProject.CampoCualitativo
        gvFiltro.DataSource = o.ObtenerRespuestasFiltroMaestro(idFiltro)
        gvFiltro.DataBind()
    End Sub

    Sub CargarDetalleReclutamiento(ByVal idRespuesta As Int64)
        Dim oCampo As New CoreProject.CampoCualitativo
        Dim datos = oCampo.ObtenerRespuestasFiltroMaestroxId(idRespuesta)
        Dim visualizar = oCampo.ObtenerRespuestasFiltroDetalle(idRespuesta)

        Dim pnlNombre As New Panel
        Dim lblNombre As New Label
        Dim lblNombreR As New Label
        Dim espacio As New LiteralControl("<br/>")
        lblNombre.ForeColor = Drawing.Color.White
        lblNombre.Font.Size = "12"
        lblNombre.Font.Bold = True
        lblNombre.Text = "Nombre:"
        lblNombreR.ForeColor = Drawing.Color.White
        lblNombreR.Font.Size = "11    "
        lblNombreR.Text = datos.Nombre
        pnlNombre.Style.Add("margin-bottom", "10px")
        pnlNombre.Style.Add("margin-top", "10px")
        pnlNombre.CssClass = "divBorder"
        pnlNombre.Controls.Add(lblNombre)
        pnlNombre.Controls.Add(espacio)
        pnlNombre.Controls.Add(lblNombreR)
        Me.pnlReclutamiento.Controls.Add(pnlNombre)

        Dim pnlCC As New Panel
        Dim lblCC As New Label
        Dim lblCCR As New Label
        Dim espacioCC As New LiteralControl("<br/>")
        lblCC.ForeColor = Drawing.Color.White
        lblCC.Font.Size = "12"
        lblCC.Font.Bold = True
        lblCC.Text = "Cédula:"
        lblCCR.ForeColor = Drawing.Color.White
        lblCCR.Font.Size = "11    "
        lblCCR.Text = datos.Cedula
        pnlCC.Style.Add("margin-bottom", "10px")
        pnlCC.Style.Add("margin-top", "10px")
        pnlCC.CssClass = "divBorder"
        pnlCC.Controls.Add(lblCC)
        pnlCC.Controls.Add(espacioCC)
        pnlCC.Controls.Add(lblCCR)
        Me.pnlReclutamiento.Controls.Add(pnlCC)

        For Each item In visualizar
            Dim pnlPreguntas As New Panel
            Dim lblPregunta As New Label
            Dim lblRespuesta As New Label
            Dim salto As New LiteralControl("<br/>")
            lblPregunta.ForeColor = Drawing.Color.White
            lblPregunta.Font.Size = "12"
            lblPregunta.Font.Bold = True
            lblPregunta.Text = item.Textopregunta
            lblRespuesta.ForeColor = Drawing.Color.White
            lblRespuesta.Font.Size = "11    "
            lblRespuesta.Text = item.Respuesta
            pnlPreguntas.Style.Add("margin-bottom", "10px")
            pnlPreguntas.Style.Add("margin-top", "10px")
            pnlPreguntas.CssClass = "divBorder"
            pnlPreguntas.Controls.Add(lblPregunta)
            pnlPreguntas.Controls.Add(salto)
            pnlPreguntas.Controls.Add(lblRespuesta)
            Me.pnlReclutamiento.Controls.Add(pnlPreguntas)
        Next

        upDetalleReclutamiento.Update()
    End Sub

    Sub CargarRespuestasFiltrosDetalle(ByVal idRespuesta As Int64)
        Dim oCampo As New CoreProject.CampoCualitativo
        Dim datos = oCampo.ObtenerRespuestasFiltroMaestroxId(idRespuesta)
        Dim visualizar = oCampo.ObtenerRespuestasFiltroDetalle(idRespuesta)

        Dim pnlNombre As New Panel
        Dim lblNombre As New Label
        Dim lblNombreR As New Label
        Dim espacio As New LiteralControl("<br/>")
        lblNombre.ForeColor = Drawing.Color.White
        lblNombre.Font.Size = "12"
        lblNombre.Font.Bold = True
        lblNombre.Text = "Nombre:"
        lblNombreR.ForeColor = Drawing.Color.White
        lblNombreR.Font.Size = "11    "
        lblNombreR.Text = datos.Nombre
        pnlNombre.Style.Add("margin-bottom", "10px")
        pnlNombre.Style.Add("margin-top", "10px")
        pnlNombre.CssClass = "divBorder"
        pnlNombre.Controls.Add(lblNombre)
        pnlNombre.Controls.Add(espacio)
        pnlNombre.Controls.Add(lblNombreR)
        Me.pnlFiltros.Controls.Add(pnlNombre)

        Dim pnlCC As New Panel
        Dim lblCC As New Label
        Dim lblCCR As New Label
        Dim espacioCC As New LiteralControl("<br/>")
        lblCC.ForeColor = Drawing.Color.White
        lblCC.Font.Size = "12"
        lblCC.Font.Bold = True
        lblCC.Text = "Cédula:"
        lblCCR.ForeColor = Drawing.Color.White
        lblCCR.Font.Size = "11    "
        lblCCR.Text = datos.Cedula
        pnlCC.Style.Add("margin-bottom", "10px")
        pnlCC.Style.Add("margin-top", "10px")
        pnlCC.CssClass = "divBorder"
        pnlCC.Controls.Add(lblCC)
        pnlCC.Controls.Add(espacioCC)
        pnlCC.Controls.Add(lblCCR)
        Me.pnlFiltros.Controls.Add(pnlCC)

        For Each item In visualizar
            Dim pnlPreguntas As New Panel
            Dim lblPregunta As New Label
            Dim lblRespuesta As New Label
            Dim salto As New LiteralControl("<br/>")
            lblPregunta.ForeColor = Drawing.Color.White
            lblPregunta.Font.Size = "12"
            lblPregunta.Font.Bold = True
            lblPregunta.Text = item.Textopregunta
            lblRespuesta.ForeColor = Drawing.Color.White
            lblRespuesta.Font.Size = "11    "
            lblRespuesta.Text = item.Respuesta
            pnlPreguntas.Style.Add("margin-bottom", "10px")
            pnlPreguntas.Style.Add("margin-top", "10px")
            pnlPreguntas.CssClass = "divBorder"
            pnlPreguntas.Controls.Add(lblPregunta)
            pnlPreguntas.Controls.Add(salto)
            pnlPreguntas.Controls.Add(lblRespuesta)
            Me.pnlFiltros.Controls.Add(pnlPreguntas)
        Next

        upAprobacionFiltros.Update()
    End Sub


    Sub limpiar()
        hfIdRespuesta.Value = "0"
        hfIdRespuestaReclutamiento.Value = 0
        hfEstado.Value = "0"
        txtComentarios.Text = ""
        upAprobacionFiltros.Visible = False
        pnlFiltros.Visible = False
        pnlAprobar.Visible = False
    End Sub

    Protected Sub btnAprobar_Click(sender As Object, e As EventArgs) Handles btnAprobar.Click
        Guardar()
    End Sub

    Protected Sub btnNoAprobar_Click(sender As Object, e As EventArgs) Handles btnNoAprobar.Click
        hfEstado.Value = 4
        If txtComentarios.Text = "" Then
            AlertJS("Escriba los Motivos por los que no Aprueba el Filtro")
            txtComentarios.Focus()
            Exit Sub
        End If
        Guardar()
    End Sub

    Protected Sub btnImgExportarInforme_Click(sender As Object, e As ImageClickEventArgs) Handles btnImgExportarInforme.Click
        Dim o As New Trabajo
        Trabajo = o.DevolverxID(hfIdTrabajo.Value)

        Me.gvReporteFiltros.DataBind()
        Me.gvReporteFiltros.Visible = True
        'Actualiza los datos del gridview
        Me.gvReporteFiltros.AllowPaging = False
        'Me.gvReporteFiltros.DataBind()

        'Crea variables en memoria para almacenar el contenido del gridview
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Crea una nueva página html y un form dentro de ella
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        'Cambie el estado del gridview para que no guarde los controles de vista
        Me.gvReporteFiltros.EnableViewState = False
        'Quita la validación de la página 
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        'Agrega el form creado
        pagina.Controls.Add(form)
        'Agrega el gridview al form
        form.Controls.Add(gvReporteFiltros)
        'Hace un render para pasar los valores de la página al htmltextwriter que contendrá los datos del gridview
        pagina.RenderControl(htw)
        'Hace un response para descargar el control
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=Reporte_FiltroAsistencia" & "-" & hfIdTrabajo.Value & "-" & Trabajo.NombreTrabajo & ".xls")
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        gvReporteFiltros.Visible = False

    End Sub

    Protected Sub Guardar()
        Dim oFiltro As New CoreProject.CampoCualitativo
        Dim eFiltro As New OP_Respuestas_Filtro_Maestro
        Dim eLog As New OP_LogRespuestas_Filtro

        Dim itemFiltro = oFiltro.ObtenerRespuestasFitroMaestroxId(hfIdRespuesta.Value)
        If hfEstado.Value = 4 Then
            itemFiltro.Estado = hfEstado.Value
        Else
            itemFiltro.Estado = 3
            hfEstado.Value = itemFiltro.Estado
        End If
        oFiltro.GuardarRespuestasFiltroMaestro(itemFiltro)

        eLog.IdRespuesta = hfIdRespuesta.Value
        eLog.Estado = hfEstado.Value
        eLog.Observacion = txtComentarios.Text
        eLog.Fecha = Date.UtcNow.AddHours(-5)
        eLog.Usuario = Session("IDUsuario").ToString
        oFiltro.GuardarLogRespuestasFiltro(eLog)

        limpiar()
        CargarMaestroReclutamiento(hfIdFiltroReclutamiento.Value)
        CargarRespuestasFiltrosMaestro(hfIdFiltro.Value)
    End Sub



End Class