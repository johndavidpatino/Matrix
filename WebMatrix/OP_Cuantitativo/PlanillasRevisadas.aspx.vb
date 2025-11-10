Imports CoreProject
Imports WebMatrix.Util

Public Class PlanillasRevisadas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(100, UsuarioID) = False Then
            Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
        End If
        If Not IsPostBack Then
            CargarTrabajos()
        End If


    End Sub



    Sub EnviarEmailSolicitud(ByVal TrabajoId As Int64)
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            oEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/SolicitudPresupuesto.aspx?TrabajoId=" & TrabajoId)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Protected Sub ddlTrabajoSeleccionado_SelectedIndexChanged(sender As Object, e As EventArgs)
        pnlResultados.Visible = True
        Dim op As New OP_CuantiDapper
        gvDataSearch.DataSource = op.CuantiPlanillasGet(True, Session("IDUsuario").ToString, Nothing, Nothing, ddlTrabajoSeleccionado.SelectedValue, Nothing)
        gvDataSearch.DataBind()
    End Sub

    Sub CargarTrabajos()
        Dim op As New OP_CuantiDapper
        ddlTrabajoSeleccionado.DataSource = op.CuantiPlanillasTrabajosGet(True, Session("IDUsuario").ToString, Nothing, Nothing, Nothing)
        ddlTrabajoSeleccionado.DataTextField = "NombreTrabajo"
        ddlTrabajoSeleccionado.DataValueField = "TrabajoId"
        ddlTrabajoSeleccionado.DataBind()
        ddlTrabajoSeleccionado.Items.Insert(0, New ListItem With {.Text = "Seleccione un trabajo", .Value = 0})
    End Sub

    Protected Sub btnRechazar_Click(sender As Object, e As EventArgs)
        Dim op As New OP_CuantiDapper
        op.CuantiPlanillasTrabajosRemove(True, Session("IDUsuario").ToString, Nothing, Nothing, ddlTrabajoSeleccionado.SelectedValue, Session("IDUsuario").ToString)
        CargarTrabajos()
        pnlResultados.Visible = False
    End Sub
End Class