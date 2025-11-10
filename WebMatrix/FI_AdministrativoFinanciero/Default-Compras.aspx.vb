Imports CoreProject
Public Class _DefaultCO
	Inherits System.Web.UI.Page
	Private Sub _Default2_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
		Dim permisos As New Datos.ClsPermisosUsuarios
		If Session("IDUsuario") Is Nothing Then
			Response.Redirect("../Default.aspx")
		Else
			Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
			If permisos.VerificarPermisoUsuario(32, UsuarioID) = False Then
				Response.Redirect("../home.aspx")
			End If
		End If

	End Sub
End Class