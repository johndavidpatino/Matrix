Imports CoreProject

Public Class Home1
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

	End Sub

	Private Sub Home1_Init(sender As Object, e As EventArgs) Handles Me.Init
		Dim permisos As New Datos.ClsPermisosUsuarios
		Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
		If permisos.VerificarPermisoUsuario(22, UsuarioID) = False Then
			Response.Redirect("../home.aspx")
		End If
	End Sub
End Class