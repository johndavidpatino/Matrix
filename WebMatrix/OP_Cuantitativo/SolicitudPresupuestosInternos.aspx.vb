Imports CoreProject
Imports WebMatrix.Util
Public Class SolicitudPresupuestosInternos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim op As New PresupInt

        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(100, UsuarioID) = False Then
            Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
        End If
            If Not IsPostBack Then
                If Not Session("TrabajoId") = Nothing Then
                    hfIdTrabajo.Value = Session("TrabajoId").ToString
                End If
        End If

        If op.SolicitudPresupuestoValidar(hfIdTrabajo.Value).Count > 0 Then
            ShowNotification("Ya fue realizada una solicitud de presupuesto para este trabajo", ShowNotifications.InfoNotification)
            TxtObservacion.Enabled = False
            txtsolicitar.Enabled = False
            Exit Sub
        End If
            txtsolicitar.Enabled = True
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub txtsolicitar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles txtsolicitar.Click
        If TxtObservacion.Text <> "" Then
            Me.txtsolicitar.Enabled = False
            LogPresupuesto(Session("IDUsuario"), hfIdTrabajo.Value, TxtObservacion.Text)
            EnviarEmailSolicitud(hfIdTrabajo.Value)
            ShowNotification("Ha sido realizada la solicitud", ShowNotifications.InfoNotification)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)

        Else
            ShowNotification("Escriba la observacion para La solicitud", ShowNotifications.InfoNotification)
        End If

    End Sub

    Sub EnviarEmailSolicitud(ByVal TrabajoId As Int64)
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            oEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/SolicitudPresupuesto.aspx?TrabajoId=" & TrabajoId)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub

    Sub LogPresupuesto(ByVal Usuario As Int64, ByVal TrabajoId As Int64, ByVal Observacion As String)
        Dim op As New PresupInt
        op.SolicitudPresupuestoGuardar(Usuario, TrabajoId, Now(), Observacion)
    End Sub
End Class