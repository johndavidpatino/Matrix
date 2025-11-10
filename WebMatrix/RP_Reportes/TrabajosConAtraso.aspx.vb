Imports CoreProject
Imports WebMatrix.Util
Public Class TrabajosConAtraso
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
        If ddlGrupoUnidades.SelectedIndex = -1 Then Exit Sub
        Dim oRep As New Reportes.RP_GerOpe
        gvTrabajos.DataSource = oRep.ObtenerEstudiosAtraso(ddlGrupoUnidades.SelectedValue)
        gvTrabajos.DataBind()
    End Sub
    Sub CargarGruposUnidad()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlGrupoUnidades.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(2)
        ddlGrupoUnidades.DataValueField = "id"
        ddlGrupoUnidades.DataTextField = "GrupoUnidad"
        ddlGrupoUnidades.DataBind()
        ddlGrupoUnidades.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
        ddlGrupoUnidades.Items.Insert(1, New ListItem With {.Text = "--Ver todas las gerencias--", .Value = 0})

    End Sub


#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(19, UsuarioID) = False Then
                Response.Redirect("../Home.aspx")
            End If
            CargarGruposUnidad()
            CargarTrabajos()
            'CargarCombo(ddlLider, "id", "Nombres", ObtenerUsuarios())
            'CargarCOEs()
        End If
    End Sub

    Protected Sub ddlGrupoUnidades_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlGrupoUnidades.SelectedIndexChanged
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
            Session("TrabajoId") = Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("TrabajoId")
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            Me.pnlAvance.Visible = True
            upDialog.Update()
            'Me.ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "$('#div-dialog').dialog({modal: true,autoOpen: true,title: 'Avande ce Campo',width: '1024px',closeOnEscape: true,open: function (type, data) {$(this).parent().appendTo(‘form’);}});", True)
            'Me.hfidTrabajo.Value = Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("TrabajoId")
            'Response.Redirect("AvanceDeCampo.aspx?TrabajoId=" & hfidTrabajo.Value)
        ElseIf e.CommandName = "Project" Then
            hfidTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("TrabajoId"))
            Response.Redirect("../RP_Reportes/GanttUnTrabajo.aspx?TrabajoId=" & hfidTrabajo.Value)
        End If
    End Sub

#End Region


    Private Sub TrabajosConAtraso_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(125, UsuarioID) = False Then
            Response.Redirect("../home.aspx")
        End If
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        If ddlGrupoUnidades.SelectedIndex = -1 Then Exit Sub
        Dim oRep_Fil As New Reportes.RP_GerOpe
        If IsNumeric(txtBuscar.Text) Then
            gvTrabajos.DataSource = oRep_Fil.ObtenerEstudiosAtraso_Filtrado(ddlGrupoUnidades.SelectedValue, txtBuscar.Text, Nothing, Nothing)
            gvTrabajos.DataBind()

        Else
            gvTrabajos.DataSource = oRep_Fil.ObtenerEstudiosAtraso_Filtrado(ddlGrupoUnidades.SelectedValue, Nothing, Nothing, txtBuscar.Text)
            gvTrabajos.DataBind()

        End If
     
    End Sub
End Class