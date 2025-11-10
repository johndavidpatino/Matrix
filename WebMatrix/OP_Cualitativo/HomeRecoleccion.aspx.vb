Imports CoreProject
Public Class _HomeRecoleccionC
    Inherits System.Web.UI.Page
    Private Sub _HomeRecoleccionC_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(53, UsuarioID) = False Then
            Response.Redirect("../RE_GT/RecoleccionDeDatos.aspx")
        End If
    End Sub
End Class