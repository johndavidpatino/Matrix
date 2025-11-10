Imports ClosedXML.Excel
Imports CoreProject
Public Class ReportesVariablesControl
	Inherits System.Web.UI.Page
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			llenarDdlAnos()
			llenarEmpleadosConEvaluacion()
		End If
	End Sub
	Shared Function SerializarAJSON(Of T)(ByVal objeto As T) As String
		Dim JSON As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer
		Return JSON.Serialize(objeto)
	End Function

	Private Sub btnAcualizar_Click(sender As Object, e As EventArgs) Handles btnAcualizar.Click

		Dim da As New CoreProject.VariablesControl

		Dim ano = If(ddlAno.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlAno.SelectedValue))
		Dim mes = If(ddlMes.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlMes.SelectedValue))
		Dim idEvaluado = If(ddlEmpleadosConEvaluacion.SelectedValue = "-1", DirectCast(Nothing, Long?), Long.Parse(ddlEmpleadosConEvaluacion.SelectedValue))

		gvVariablesControl.DataSource = da.obtenerReporteVariablesControl(ano, mes, idEvaluado)
		gvVariablesControl.DataBind()

		gvVariablesControlPorMes.DataSource = da.obtenerReporteVariablesControlPorMes(ano, mes, idEvaluado)
		gvVariablesControlPorMes.DataBind()

		upVariablesControl.Update()
		upVariablesControlPorMes.Update()
	End Sub

	Private Sub btnExportarExcelVariablesControl_Click(sender As Object, e As EventArgs) Handles btnExportarExcelVariablesControl.Click
		Dim da As New CoreProject.VariablesControl

		Dim ano = If(ddlAno.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlAno.SelectedValue))
		Dim mes = If(ddlMes.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlMes.SelectedValue))
		Dim idEvaluado = If(ddlEmpleadosConEvaluacion.SelectedValue = "-1", DirectCast(Nothing, Long?), Long.Parse(ddlEmpleadosConEvaluacion.SelectedValue))


		Dim wb As New XLWorkbook

		Dim nombresColumnas As String = "GerenteCOE_Evaluado;FechaEvaluacion;ServiceLineTrabajo;Nombre Trabajo;JobBook;Si;No;Seguridad;Obs Seguridad;Obtención;Obs Obtención;Objetivo;Obs Objetivo;Aplicación;Obs Aplicación;Distribución;Obs Distribución;Cumplimiento;Obs Cumplimiento"

		Dim ws = wb.Worksheets.Add("VariablesControls")

		Dim variablesControl = da.obtenerReporteVariablesControl(ano, mes, idEvaluado)

		insertarNombreColumnasExcel(ws, nombresColumnas.Split(";"), 1)

		ws.Cell(2, 1).InsertData(variablesControl)

		exportarExcel(wb, "VariablesControl")
	End Sub

	Private Sub btnExportarExcelVariablesControlPorMes_Click(sender As Object, e As EventArgs) Handles btnExportarExcelVariablesControlPorMes.Click
		Dim da As New CoreProject.VariablesControl

		Dim ano = If(ddlAno.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlAno.SelectedValue))
		Dim mes = If(ddlMes.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlMes.SelectedValue))
		Dim idEvaluado = If(ddlEmpleadosConEvaluacion.SelectedValue = "-1", DirectCast(Nothing, Long?), Long.Parse(ddlEmpleadosConEvaluacion.SelectedValue))

		Dim wb As New XLWorkbook

		Dim nombresColumnas As String = "GerenteCOE;Año;Mes;Si(%);No(%)"

		Dim ws = wb.Worksheets.Add("VariablesControlPorMes")

		Dim variablesControl = da.obtenerReporteVariablesControlPorMes(ano, mes, idEvaluado)

		insertarNombreColumnasExcel(ws, nombresColumnas.Split(";"), 1)

		ws.Cell(2, 1).InsertData(variablesControl)

		exportarExcel(wb, "VariablesControlPorMes")
	End Sub

	Private Sub exportarExcel(ByVal workbook As XLWorkbook, ByVal name As String)
		Response.Clear()

		Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
		Response.AddHeader("content-disposition", "attachment;filename=""" & name & ".xlsx""")

		Using memoryStream = New IO.MemoryStream()
			workbook.SaveAs(memoryStream)
			memoryStream.WriteTo(Response.OutputStream)
		End Using
		Response.End()
	End Sub

	Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String, ByVal fila As Integer)
		For columna = 0 To nombresColumnas.Count - 1
			hoja.Cell(fila, columna + 1).Value = nombresColumnas(columna)
		Next
	End Sub

	Private Sub llenarDdlAnos()
		Dim anoInicial As Integer
		Dim anoFinal As Integer

		anoFinal = Date.Now.Year

		For anoInicial = 2018 To anoFinal
			ddlAno.Items.Insert(0, New ListItem With {.Text = anoInicial, .Value = anoInicial})
		Next

	End Sub
	Private Sub llenarEmpleadosConEvaluacion()
		Dim da As New CoreProject.VariablesControl

		ddlEmpleadosConEvaluacion.DataSource = da.obtenerEmpleadosConEvaluacion()

		ddlEmpleadosConEvaluacion.DataValueField = "Id"
		ddlEmpleadosConEvaluacion.DataTextField = "NombreCompletoEvaluado"

		ddlEmpleadosConEvaluacion.DataBind()

		ddlEmpleadosConEvaluacion.Items.Insert(0, New ListItem("--Seleccione--", "-1"))

	End Sub

End Class