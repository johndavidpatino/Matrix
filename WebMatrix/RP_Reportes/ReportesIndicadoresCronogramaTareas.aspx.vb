Imports ClosedXML.Excel
Imports CoreProject
Public Class ReportesIndicadoresCronogramaTareas
	Inherits System.Web.UI.Page
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			llenarTrabajosConTareaEntregaProyectos()
		End If
	End Sub
	Private Sub llenarTrabajosConTareaEntregaProyectos()
		Dim dalTrabajos As New Trabajo

		ddlTrabajosConTareaEntregaProyecto.DataSource = dalTrabajos.ObtenerTrabajosConTareaEntregaDeProyectos()
		ddlTrabajosConTareaEntregaProyecto.DataValueField = "id"
		ddlTrabajosConTareaEntregaProyecto.DataTextField = "NombreTrabajo"
		ddlTrabajosConTareaEntregaProyecto.DataBind()

	End Sub

	Private Sub btnCargarIndicadoresTrabajo_Click(sender As Object, e As EventArgs) Handles btnCargarIndicadoresTrabajo.Click
		Dim dal As New ReportesTareas
		gvIndicadoresPorTrabajo.DataSource = dal.ObtenerIndicadoresCronogramaPorTrabajoId(ddlTrabajosConTareaEntregaProyecto.SelectedValue)
		gvIndicadoresPorTrabajo.DataBind()
		upIndicadoresPorTrabajo.Update()
	End Sub
End Class