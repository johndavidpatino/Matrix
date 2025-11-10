Imports CoreProject
Imports WebMatrix.Util
Imports Utilidades.Encripcion

Public Class CambioContraseña
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub
    Sub Limpiar()
        txtconfirmacion.Text = String.Empty
        txtcontraseña.Text = String.Empty
        txtcontraseñanueva.Text = String.Empty
    End Sub
    Protected Sub btnguardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnguardar.Click
        Dim Usuario As New US.Usuarios
        Dim passwordviejo As String
        Dim password As String
        Dim passwordnuevo As String
        If txtcontraseñanueva.Text <> txtconfirmacion.Text Then
            lblresultado.Text = "Las Constraseñas No Coinciden"
            Limpiar()
            Exit Sub
        End If
        passwordviejo = Usuario.PasswordUsuario(Session("IDUsuario").ToString)
        password = Cifrado(1, 2, Me.txtcontraseña.Text, "Ipsos*23432_2013", "Ipsos*23432_2013")
        If password <> passwordviejo Then
            lblresultado.Text = "Antiguo Password Incorrecto"
            Limpiar()
            Exit Sub
        End If
        passwordnuevo = Cifrado(1, 2, Me.txtconfirmacion.Text, "Ipsos*23432_2013", "Ipsos*23432_2013")
        Usuario.actualizarpassword(passwordnuevo, Session("IDUsuario").ToString)
        lblresultado.Text = "Contraseña Cambiada Correctamente"
        log(Session("IDUsuario").ToString, 3)
        Limpiar()

    End Sub

    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New LogEjecucion
            log.Guardar(29, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class