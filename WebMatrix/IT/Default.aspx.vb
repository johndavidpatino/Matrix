Imports CoreProject

Public Class _Default7
	Inherits System.Web.UI.Page

	Private Sub _Default7_Init(sender As Object, e As EventArgs) Handles Me.Init
		Dim permisos As New Datos.ClsPermisosUsuarios
		Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
		If permisos.VerificarPermisoUsuario(133, UsuarioID) = False Then
			Response.Redirect("../home.aspx")
		End If
	End Sub
End Class