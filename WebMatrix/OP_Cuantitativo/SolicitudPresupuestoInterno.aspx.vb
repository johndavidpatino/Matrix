Imports CoreProject
Imports WebMatrix.Util

Public Class SolicitudPresupuestoInterno
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
            hfNew.Value = False
            ShowNotification("Ya fue realizada una solicitud de presupuesto para este trabajo", ShowNotifications.InfoNotification)
            CargarPrevio()
            btnSolicitar.Enabled = False
        Else
            hfNew.Value = True
            btnSolicitar.Enabled = True
        End If
    End Sub

    Sub CargarPrevio()
        Dim op As New PresupInt
        Dim info = op.SolicitudPresupuestoInternoGet(hfIdTrabajo.Value)
        If info.Jornada IsNot Nothing Then chbJornadas.Checked = info.Jornada
        If info.Agendamiento IsNot Nothing Then chbAgendamiento.Checked = info.Agendamiento
        If info.Encuesta IsNot Nothing Then chbCampo.Checked = info.Encuesta
        If info.Reclutamiento IsNot Nothing Then chbReclutamiento.Checked = info.Reclutamiento
        If info.Muestra IsNot Nothing Then txtMuestra.Text = info.Muestra
        If info.VrSugeridoContratista IsNot Nothing Then txtVrSugerido.Text = info.VrSugeridoContratista
        If info.General IsNot Nothing Then txtMuestra.Text = info.General
        If info.NSE1y2 IsNot Nothing Then txtNSE1y2.Text = info.NSE1y2
        If info.NSE3y4 IsNot Nothing Then txtNSE3y4.Text = info.NSE3y4
        If info.NSE5y6 IsNot Nothing Then txtNSE5y6.Text = info.NSE5y6
        If info.Observacion IsNot Nothing Then txtObservaciones.Text = info.Observacion
    End Sub

    Protected Sub btnSolicitar_Click(sender As Object, e As EventArgs)
        If chbAgendamiento.Checked = False And chbCampo.Checked = False And chbJornadas.Checked = False And chbReclutamiento.Checked = False Then
            ShowNotification("Debe seleccionar al menos un presupuesto", ShowNotifications.ErrorNotificationLong)
            Exit Sub
        End If
        If chbJornadas.Checked = True And Not (IsNumeric(txtGeneral.Text)) And Not (IsNumeric(txtNSE1y2.Text)) And Not (IsNumeric(txtNSE3y4.Text)) And Not (IsNumeric(txtNSE5y6.Text)) Then
            ShowNotification("Debe agregar la distribución para el reclutamiento", ShowNotifications.ErrorNotificationLong)
            Exit Sub
        End If
        If Not IsNumeric(txtMuestra.Text) Then
            ShowNotification("Escriba la muestra", ShowNotifications.ErrorNotificationLong)
            Exit Sub
        End If

        Dim General As Integer = 0
        Dim NSE1y2 As Integer = 0
        Dim NSE3y4 As Integer = 0
        Dim NSE5y6 As Integer = 0
        Dim VrContratista As Integer = 0

        If IsNumeric(txtVrSugerido.Text) Then VrContratista = txtVrSugerido.Text

        If IsNumeric(txtGeneral.Text) Then General = txtGeneral.Text
        If IsNumeric(txtNSE1y2.Text) Then NSE1y2 = txtNSE1y2.Text
        If IsNumeric(txtNSE3y4.Text) Then NSE3y4 = txtNSE3y4.Text
        If IsNumeric(txtNSE5y6.Text) Then NSE5y6 = txtNSE5y6.Text
        Dim ent As New CC_SolicitudPresupuesto
        ent.UsuarioId = Session("IDUsuario").ToString
        ent.TrabajoId = hfIdTrabajo.Value
        ent.Jornada = chbJornadas.Checked
        ent.Agendamiento = chbAgendamiento.Checked
        ent.Encuesta = chbCampo.Checked
        ent.Reclutamiento = chbReclutamiento.Checked
        ent.Fecha = Date.UtcNow.AddHours(-5)
        ent.Muestra = txtMuestra.Text
        ent.VrSugeridoContratista = VrContratista
        ent.General = General
        ent.NSE1y2 = NSE1y2
        ent.NSE3y4 = NSE3y4
        ent.NSE5y6 = NSE5y6
        ent.Observacion = txtObservaciones.Text

        Dim op As New PresupInt
        op.SolicitudPresupuestoInternoAdd(hfIdTrabajo.Value)
        op.SolicitudPresupuestoInternoAddNew(ent)
        'If hfNew.Value = True Then
        '    If ent.Encuesta = True Then
        '        CrearPresupuesto(ent.TrabajoId, 1)
        '    End If
        '    If ent.Agendamiento = True Then
        '        CrearPresupuesto(ent.TrabajoId, 2)
        '    End If
        '    If ent.Jornada = True Then
        '        CrearPresupuesto(ent.TrabajoId, 6)
        '    End If
        '    If ent.Reclutamiento = True Then
        '        CrearPresupuesto(ent.TrabajoId, 7)
        '    End If
        'End If
        hfNew.Value = False

        EnviarEmailSolicitud(hfIdTrabajo.Value)
        ShowNotification("Ha sido realizada la solicitud", ShowNotifications.InfoNotification)
    End Sub

    Sub EnviarEmailSolicitud(ByVal TrabajoId As Int64)
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            oEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/SolicitudPresupuesto.aspx?TrabajoId=" & TrabajoId)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Private Sub CrearPresupuesto(ByVal trabajoid As Int64, ByVal Tipo As Int64)
        Dim op As New PresupInt
        Dim idnuevotrabajo As Decimal
        Try
            Dim tarifa = op.LstObtenerAñoValorLast(Tipo, trabajoid).Id
            idnuevotrabajo = op.GuardarPresupuestoInterno(trabajoid, Session("IDUsuario").ToString, Tipo, tarifa)
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub chbReclutamiento_CheckedChanged(sender As Object, e As EventArgs)
        If chbReclutamiento.Checked = True Then
            rowReclutamiento.Visible = True
        Else
            rowReclutamiento.Visible = False
        End If
    End Sub
End Class