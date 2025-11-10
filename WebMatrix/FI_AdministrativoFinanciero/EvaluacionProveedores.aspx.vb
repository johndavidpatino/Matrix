Public Class EvaluacionProveedores
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			If Not Request.QueryString("idEvaluacion") Is Nothing Then
				Dim o As New CoreProject.FI.Formatos
				Dim info = o.ObtenerEvaluacionProveedor(Request.QueryString("idEvaluacion").ToString())
				txtNombreProveedor.Text = info.Proveedor
				txtSymphonyCenter.Text = info.SymphonyCenter
			Else
				Response.Redirect("../Home/Home.aspx")
			End If

		End If
	End Sub

	Protected Sub Submit_Click(sender As Object, e As EventArgs)
		Dim o As New CoreProject.FI.Formatos
		Dim info = o.ObtenerEvaluacionProveedor(Request.QueryString("idEvaluacion").ToString())
		info.Evaluador = Session("IDUsuario").ToString
		info.Q1 = Q1.SelectedValue
		info.Q2 = Q2.SelectedValue
		info.Q3 = Q3.SelectedValue
		info.Q4 = Q4.SelectedValue
		info.Q5 = Q5.SelectedValue
		info.Q6 = Q6.SelectedValue
		info.Q7 = rbQ7.SelectedValue
		info.FechaEvaluacion = Date.UtcNow.AddHours(-5)
		o.GuardarEvaluacionProveedor(info)
		Response.Redirect("../Home/Home.aspx")
	End Sub

	Protected Sub Volver_Click(sender As Object, e As EventArgs)
		Response.Redirect("../Home/Home.aspx")
	End Sub
End Class