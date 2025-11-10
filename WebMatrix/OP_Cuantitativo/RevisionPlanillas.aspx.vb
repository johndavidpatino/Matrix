Imports CoreProject
Imports WebMatrix.Util

Public Class RevisionPlanillas
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
        gvDataSearch.DataSource = op.CuantiPlanillasGet(False, Session("IDUsuario").ToString, Nothing, Nothing, ddlTrabajoSeleccionado.SelectedValue, Nothing)
        gvDataSearch.DataBind()
        If gvDataSearch.Rows.Count > 0 Then
            btnAprobar.Enabled = True
            btnRechazar.Enabled = True
        Else
            btnAprobar.Enabled = False
            btnRechazar.Enabled = False
        End If
    End Sub

    Sub CargarTrabajos()
        Dim op As New OP_CuantiDapper
        ddlTrabajoSeleccionado.DataSource = OP.CuantiPlanillasTrabajosGet(False, Session("IDUsuario").ToString, Nothing, Nothing, Nothing)
        ddlTrabajoSeleccionado.DataTextField = "NombreTrabajo"
        ddlTrabajoSeleccionado.DataValueField = "TrabajoId"
        ddlTrabajoSeleccionado.DataBind()
        ddlTrabajoSeleccionado.Items.Insert(0, New ListItem With {.Text = "Seleccione un trabajo", .Value = 0})

        gvSinPresupuestos.DataSource = op.CuantiPlanillasSinPresupuestoGet(Nothing, Session("IDUsuario").ToString, Nothing, Nothing, Nothing)
        gvSinPresupuestos.DataBind()
    End Sub

    Protected Sub btnAprobar_Click(sender As Object, e As EventArgs)
        Dim op As New OP_CuantiDapper
        op.CuantiPlanillasTrabajosUpdate(True, Session("IDUsuario").ToString, Nothing, Nothing, ddlTrabajoSeleccionado.SelectedValue, Session("IDUsuario").ToString)
        CargarTrabajos()
        pnlResultados.Visible = False
    End Sub

    Protected Sub btnRechazar_Click(sender As Object, e As EventArgs)
        Dim op As New OP_CuantiDapper
        op.CuantiPlanillasTrabajosRemove(True, Session("IDUsuario").ToString, Nothing, Nothing, ddlTrabajoSeleccionado.SelectedValue, Session("IDUsuario").ToString)
        CargarTrabajos()
        pnlResultados.Visible = False
    End Sub
End Class