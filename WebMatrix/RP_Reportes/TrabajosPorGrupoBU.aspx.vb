Imports CoreProject
Imports WebMatrix.Util
Public Class TrabajosPorGrupoBU
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Private _proyectoId As Int64
    Public Property proyectoId() As Int64
        Get
            Return _proyectoId
        End Get
        Set(ByVal value As Int64)
            _proyectoId = value
        End Set
    End Property
    Private _idUsuario As Int64
    Public Property idUsuario() As Int64
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Int64)
            _idUsuario = value
        End Set
    End Property
#End Region

#Region "Funciones y Metodos"
    Sub CargarTrabajos()
        If ddlUnidades.SelectedIndex = -1 Then Exit Sub
        Dim oRep As New Reportes.GerpProyectos
        gvTrabajos.DataSource = oRep.ObtenerTrabajosPorGrupoBU(ddlUnidades.SelectedValue)
        gvTrabajos.DataBind()
    End Sub

    Private Sub CargarUnidades()
        Dim oUnidades As New CoreProject.US.Unidades

        'ddlUnidades.DataSource = oUnidades.ObtenerUnidadCombo
        ddlUnidades.DataSource = oUnidades.ObtenerUnidadesxUsuario(Session("IDUsuario").ToString)
        ddlUnidades.DataTextField = "Unidad"
        ddlUnidades.DataValueField = "id"
        ddlUnidades.DataBind()

        ddlUnidades.Items.Insert(0, New ListItem With {.Value = -1, .Text = "--Seleccione--"})

    End Sub
#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(35, UsuarioID) = False Then
                Response.Redirect("../Home.aspx")
            End If
            CargarUnidades()
            CargarTrabajos()
            'CargarCombo(ddlLider, "id", "Nombres", ObtenerUsuarios())
            'CargarCOEs()
        End If
    End Sub

    Protected Sub ddlGrupoUnidades_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlUnidades.SelectedIndexChanged
        CargarTrabajos()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
        gvTrabajos.PageIndex = e.NewPageIndex
        CargarTrabajos()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        If e.CommandName = "Avance" Then
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            Me.hfidTrabajo.Value = Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")
            Response.Redirect("AvanceDeCampo.aspx?TrabajoId=" & hfidTrabajo.Value)
        ElseIf e.CommandName = "Project" Then
            hfidTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Response.Redirect("../RP_Reportes/GanttUnTrabajo.aspx?TrabajoId=" & hfidTrabajo.Value)
        ElseIf e.CommandName = "Actualizar" Then
            hfidTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Response.Redirect("../CORE/Gestion-Tareas.aspx?IdTrabajo=" & hfidTrabajo.Value & "&URLRetorno=" & UrlOriginal.RP_Reportes_TrabajosPorGrupoBU & "&IdUnidadEjecuta=" & UnidadesCore.Proyectos & "&IdRolEstima=6")
        End If
    End Sub

    Private Sub TrabajosPorGerencia_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(35, UsuarioID) = False Then
            Response.Redirect("../home.aspx")
        End If
    End Sub
#End Region


End Class