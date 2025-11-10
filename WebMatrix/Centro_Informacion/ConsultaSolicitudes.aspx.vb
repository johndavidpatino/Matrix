Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Imports CoreProject.CIEntities
Imports ClosedXML.Excel
Imports CoreProject.OP

Public Class ConsultaSolicitudes
    Inherits System.Web.UI.Page


#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Dim oBusqueda As New CentroInformacion
        gvSolicitudes.DataSource = oBusqueda.obtenersolicitudxtodoscampos(txtBuscar.Text)
        gvSolicitudes.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub gvSolicitudes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSolicitudes.PageIndexChanging
        gvSolicitudes.PageIndex = e.NewPageIndex
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub gvSolicitudes_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSolicitudes.RowCommand

    End Sub


#End Region

    Private Sub _ConsultaSolicitudes_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(137, UsuarioID) = False Then
            Response.Redirect("../Home.aspx")
        End If
    End Sub


End Class