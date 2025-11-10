Imports CoreProject

Public Class HomeRecoleccion
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

	End Sub

	Private Sub HomeRecoleccion_Init(sender As Object, e As EventArgs) Handles Me.Init
		Dim permisos As New Datos.ClsPermisosUsuarios
		Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
		If permisos.VerificarPermisoUsuario(54, UsuarioID) = False Then
			Response.Redirect("../home.aspx")
		End If
	End Sub
End Class