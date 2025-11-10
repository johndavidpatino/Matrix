Public Class SyncIssues
    Inherits System.Web.UI.Page

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim o As New CoreProject.GestionCampo.Sync
        Me.ddlPreguntas.DataSource = o.PreguntasGet(txtTrabajoId.Text, Nothing)
        Me.ddlPreguntas.DataValueField = "DCP_Descripcion"
        Me.ddlPreguntas.DataTextField = "Pr_Nombre"
        Me.ddlPreguntas.DataBind()
        Util.ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub btnActualizarValor_Click(sender As Object, e As EventArgs) Handles btnActualizarValor.Click
        Dim o As New CoreProject.GestionCampo.Sync
        Dim da As New CoreProject.GestionCampo.Sync
        Dim idRegistro As Decimal
        Dim vValor() As String
        Dim valor As String = ""
        If Me.ddlPreguntas.Items.Count = 0 Then
            Util.ShowNotification("Primero debe elegir la pregunta", ShowNotifications.ErrorNotification)
            Util.ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If
        If Not (IsNumeric(txtSbjNum.Text)) Then
            Util.ShowNotification("Digite el SbjNum correctamente", ShowNotifications.ErrorNotification)
            Util.ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If
        If txtNewValor.Text = "" Then
            Util.ShowNotification("Debe escribir el nuevo valor", ShowNotifications.ErrorNotification)
            Exit Sub
        End If
        valor = txtNewValor.Text
        If Me.ddlPreguntas.SelectedValue = "Res_Fecha" Then
            vValor = valor.Split("/")
            If vValor.Count = 3 Then
                valor = vValor(1) & "/" & vValor(0) & "/" & vValor(2)
            Else
                Util.ShowNotification("El formato de fecha no es correcto", ShowNotifications.ErrorNotification)
                Exit Sub
            End If
        End If
        o.ActualizarPregunta(txtSbjNum.Text, valor, Me.ddlPreguntas.SelectedValue, txtTrabajoId.Text)
        idRegistro = o.obtenerIdRegistroRespuestas(txtTrabajoId.Text, txtSbjNum.Text)
        da.grabarAuditoria(Session("IdUsuario").ToString, CoreProject.GestionCampo.Sync.ETipoAccion.actualizado, CoreProject.GestionCampo.Sync.EModulo.Matrix_SoftSyn_ActualizaciónDatos, "El nuevo valor es " & txtNewValor.Text, DateTime.UtcNow.AddHours(-5), idRegistro, CoreProject.GestionCampo.Sync.ETabla.Respuestas)
        Util.ShowNotification("Cambio realizado", ShowNotifications.InfoNotification)
        Util.ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub btnQuitarEntrenamiento_Click(sender As Object, e As EventArgs) Handles btnQuitarEntrenamiento.Click
        Dim o As New CoreProject.GestionCampo.Sync
        o.QuitarPreguntasEntrenamiento(txtNumeroTrabajo.Text)
        Util.ShowNotification("Cambio realizado", ShowNotifications.InfoNotification)
        Util.ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub btnSincronizacion_Click(sender As Object, e As EventArgs) Handles btnSincronizacion.Click
        Dim o As New CoreProject.GestionCampo.Sync
        o.HabilitarSincronizacion(txtNumeroTrabajo.Text)
        Util.ShowNotification("Cambio realizado", ShowNotifications.InfoNotification)
        Util.ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub btnSupervision_Click(sender As Object, e As EventArgs) Handles btnSupervision.Click
        Dim o As New CoreProject.GestionCampo.Sync
        o.ErrorTrabajoEspecializado(txtNumeroTrabajo.Text)
        Util.ShowNotification("Cambio realizado", ShowNotifications.InfoNotification)
        Util.ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub _Default7_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(134, UsuarioID) = False Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Private Sub btnHabilitarPiloto_Click(sender As Object, e As EventArgs) Handles btnHabilitarPiloto.Click
        Dim o As New CoreProject.GestionCampo.Sync
        o.HabilitarEncuestaPiloto(txtSbjNumPiloto.Text)
        Util.ShowNotification("Cambio realizado", ShowNotifications.InfoNotification)
        Util.ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub _SyncIssues_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(133, UsuarioID) = False Then
            Response.Redirect("../Home.aspx")
        End If
    End Sub

    Protected Sub btnEncuestaPiloto_Click(sender As Object, e As EventArgs) Handles btnEncuestaPiloto.Click
        Dim o As New CoreProject.GestionCampo.Sync
        o.EncuestaPiloto(txtSbjNumPiloto2.Text)
        Util.ShowNotification("Cambio realizado", ShowNotifications.InfoNotification)
        Util.ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub
End Class