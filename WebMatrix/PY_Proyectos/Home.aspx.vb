Imports CoreProject

Public Class Home3
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

	End Sub

	Private Sub Home3_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
		Dim permisos As New Datos.ClsPermisosUsuarios
		Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
		If permisos.VerificarPermisoUsuario(24, UsuarioID) = False Then
			Response.Redirect("../home.aspx")
		End If
	End Sub
End Class