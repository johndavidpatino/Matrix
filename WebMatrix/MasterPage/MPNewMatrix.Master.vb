Public Class MPNewMatrix
	Inherits System.Web.UI.MasterPage


	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			LoadInfoEmployee()
		End If
	End Sub

	Sub LoadInfoEmployee()
		Dim empleados As New CoreProject.Empleados
		Dim info = empleados.obtenerPorIdentificacion(Context.Session("IDUsuario"))
		lblNameEmployee.Text = info.Nombres + " " + info.Apellidos

		Dim cargos As New CoreProject.Cargos
		Dim cargo = cargos.DevolverTodos.Where(Function(x) x.id = info.Cargo).FirstOrDefault
		lblPositionEmployee.Text = cargo.Cargo

		If Not info.urlFoto Is Nothing Then
			imgEmployee.Src = info.urlFoto
		End If
	End Sub
End Class