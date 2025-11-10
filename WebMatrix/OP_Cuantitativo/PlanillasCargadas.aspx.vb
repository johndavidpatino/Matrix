Imports CoreProject
Imports WebMatrix.Util

Public Class PlanillasCargadas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(135, UsuarioID) = False Then
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
        Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
        If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
        Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)
        pnlResultados.Visible = True
        Dim op As New OP_CuantiDapper
        gvDataSearch.DataSource = op.CuantiPlanillasGet(Nothing, Nothing, inicioCorteFecha, finCorteFecha, ddlTrabajoSeleccionado.SelectedValue, Session("IDUsuario").ToString)
        gvDataSearch.DataBind()
    End Sub

    Sub CargarTrabajos()
        Dim op As New OP_CuantiDapper
        Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
        If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
        Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)
        ddlTrabajoSeleccionado.DataSource = op.CuantiPlanillasTrabajosGet(Nothing, Nothing, inicioCorteFecha, finCorteFecha, Nothing, Session("IDUsuario").ToString)
        ddlTrabajoSeleccionado.DataTextField = "NombreTrabajo"
        ddlTrabajoSeleccionado.DataValueField = "TrabajoId"
        ddlTrabajoSeleccionado.DataBind()
        ddlTrabajoSeleccionado.Items.Insert(0, New ListItem With {.Text = "Seleccione un trabajo", .Value = 0})
    End Sub

    Protected Sub btnRechazar_Click(sender As Object, e As EventArgs)
        Dim op As New OP_CuantiDapper
        op.CuantiPlanillasTrabajosRemove(True, Nothing, Nothing, Nothing, ddlTrabajoSeleccionado.SelectedValue, Session("IDUsuario").ToString)
        CargarTrabajos()
        pnlResultados.Visible = False
    End Sub
End Class