Imports CoreProject
Public Class _GestionyTratamientoDeDatos
    Inherits System.Web.UI.Page


    Private Sub _GestionyTratamientoDeDatos_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(27, UsuarioID) = False Then
            Response.Redirect("../home.aspx")
        End If
    End Sub
End Class