Imports CoreProject
Public Class _RecoleccionDeDatos
    Inherits System.Web.UI.Page


    Private Sub _RecoleccionDeDatos_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(26, UsuarioID) = False Then
            Response.Redirect("../home.aspx")
        End If
    End Sub
End Class