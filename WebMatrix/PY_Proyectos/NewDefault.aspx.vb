Imports CoreProject

Public Class _Default1
	Inherits System.Web.UI.Page

#Region "Metodos"
	Sub LoadProyectos(ByVal IdUsuario As Int64)
		Dim oProyecto As New Proyecto
		gvDataProyectos.DataSource = oProyecto.obtenerXGerenteProyectos(IdUsuario)
		gvDataProyectos.DataBind()
	End Sub
#End Region

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			LoadProyectos(Session("IDUsuario").ToString)
		End If
	End Sub

	Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

	End Sub

	Private Sub Home3_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
		Dim permisos As New Datos.ClsPermisosUsuarios
		Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
		If permisos.VerificarPermisoUsuario(24, UsuarioID) = False Then
			Response.Redirect("../home.aspx")
		End If
	End Sub

End Class