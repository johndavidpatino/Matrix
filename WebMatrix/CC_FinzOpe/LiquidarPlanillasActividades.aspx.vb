Imports CoreProject
Imports WebMatrix.Util

Public Class LiquidarPlanillasActividades
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(132, UsuarioID) = False Then
            Response.Redirect("../Home/Home.aspx")
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


    Sub CargarTrabajos()
        Dim op As New OP_CuantiDapper

        gvSinPresupuestos.DataSource = op.CuantiPlanillasSinPresupuestoGet(True, Nothing, Nothing, Nothing, Nothing)
        gvSinPresupuestos.DataBind()


        gvDataPendientes.DataSource = op.CuantiPlanillasPendientesGet(False, Nothing, Nothing, Nothing, Nothing)
        gvDataPendientes.DataBind()

        gvDataToLiquidar.DataSource = op.CuantiPlanillasPendientesGet(True, Nothing, Nothing, Nothing, Nothing)
        gvDataToLiquidar.DataBind()
        If gvDataToLiquidar.Rows.Count > 0 Then
            btnLiquidar.Enabled = True
        Else
            btnLiquidar.Enabled = False
        End If
    End Sub

    Protected Sub btnLiquidar_Click(sender As Object, e As EventArgs)
        Dim op As New OP_CuantiDapper
        op.CuantiPlanillasLiquidar(True, Nothing, Nothing, Nothing, Nothing)
        CargarTrabajos()
    End Sub
End Class