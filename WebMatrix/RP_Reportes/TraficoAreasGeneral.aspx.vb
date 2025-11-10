Imports System.IO
Imports WebMatrix.Util
Imports CoreProject

Public Class TraficoAreasGeneral
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'If Request.QueryString("TrabajoId") IsNot Nothing Then
            '    hfIdTrabajo.Value = Request.QueryString("TrabajoId")
            'End If
            CargarGruposUnidad()
            CargarTrabajos()
        End If
    End Sub

    Private Sub btnConsultar_Click(sender As Object, e As System.EventArgs) Handles btnConsultar.Click
        If ddlAreasTrafico.SelectedValue = 0 Then Exit Sub
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        CargarTraficoAreas()
    End Sub

    Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        If e.CommandName = "Avance" Then
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            Me.hfIdTrabajo.Value = Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")
            Response.Redirect("AvanceDeCampo.aspx?TrabajoId=" & hfIdTrabajo.Value)
        ElseIf e.CommandName = "Project" Then
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Response.Redirect("../RP_Reportes/GanttUnTrabajo.aspx?TrabajoId=" & hfIdTrabajo.Value)
        End If
    End Sub
    Protected Sub ddlGrupoUnidades_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlGrupoUnidades.SelectedIndexChanged
        CargarTrabajos()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
        gvTrabajos.PageIndex = e.NewPageIndex
        CargarTrabajos()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub TraficoAreasGeneral_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
    End Sub
#End Region

#Region "Metodos"
    Sub CargarTraficoAreas()
        Dim oTrafico As New CoreProject.Reportes.AvanceCampo
        Try
            Me.gvTraficoAreas.DataSource = oTrafico.ObtenerTraficoAreasGeneral(txtFechaInicio.Text, txtFechaTerminacion.Text, ddlAreasTrafico.SelectedValue)
            Me.gvTraficoAreas.DataBind()
        Catch ex As Exception

        End Try

    End Sub

    Sub CargarTrabajos()
        If ddlGrupoUnidades.SelectedIndex = -1 Then Exit Sub
        Dim oRep As New Reportes.RP_GerOpe
        gvTrabajos.DataSource = oRep.ObtenerTrabajosPorGerencia(ddlGrupoUnidades.SelectedValue, Nothing, Nothing, Nothing)
        gvTrabajos.DataBind()
    End Sub
    Sub CargarGruposUnidad()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlGrupoUnidades.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(2)
        ddlGrupoUnidades.DataValueField = "id"
        ddlGrupoUnidades.DataTextField = "GrupoUnidad"
        ddlGrupoUnidades.DataBind()
        ddlGrupoUnidades.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
        ddlGrupoUnidades.Items.Insert(1, New ListItem With {.Text = "--Ver todos--", .Value = 0})
    End Sub
#End Region



End Class