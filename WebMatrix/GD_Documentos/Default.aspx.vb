Imports CoreProject
Public Class _Default3
    Inherits System.Web.UI.Page


    Private Sub _Default3_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(29, UsuarioID) = False Then
            Response.Redirect("../home.aspx")
        End If
    End Sub
End Class