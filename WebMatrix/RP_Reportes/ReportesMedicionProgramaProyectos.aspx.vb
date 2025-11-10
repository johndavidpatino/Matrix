Imports ClosedXML.Excel
Imports CoreProject
Public Class ReportesMedicionProgramaProyectos
	Inherits System.Web.UI.Page
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			llenarDdlAnos()
		End If
	End Sub

	Private Sub btnAcualizar_Click(sender As Object, e As EventArgs) Handles btnAcualizar.Click
		Dim verPor As RegistroObservaciones.ETiposAgrupacion
		Dim ano As Short?
		Dim mes As Short?
		Dim idInstrumento As Short?
		Dim idTarea As Short?
		Dim erroresRegistroObservaciones As List(Of REP_ErroresRegistroObservaciones_Result)
		Dim erroresRegistroObservacionesDetalle As List(Of REP_ErroresRegistroObservacionesDetalle_Result)

		Dim da As New RegistroObservaciones

		verPor = [Enum].Parse(RegistroObservaciones.ETiposAgrupacion.Total.GetType, ddlVerPor.SelectedValue)
		ano = If(ddlAno.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlAno.SelectedValue))
		mes = If(ddlMes.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlMes.SelectedValue))
		idInstrumento = If(ddlInstrumentos.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlInstrumentos.SelectedValue))
		idTarea = If(ddlTareas.SelectedValue = "", DirectCast(Nothing, Short?), Short.Parse(ddlTareas.SelectedValue))

		gvDatosGerente.DataSource = da.obtenerCalidadErroresRO_GerenteProyectos(idTarea, mes, ano, verPor, idInstrumento)
		gvDatosGerente.DataBind()

		gvDatosDetalleGerente.DataSource = da.obtenerCalidadErroresRO_GerenteProyectosDetalle(idTarea, mes, ano, verPor, idInstrumento, Nothing)
		gvDatosDetalleGerente.DataBind()

		gvDatosCOE.DataSource = da.obtenerCalidadErroresRO_COE(idTarea, mes, ano, verPor, idInstrumento)
		gvDatosCOE.DataBind()

		gvDatosDetalleCOE.DataSource = da.obtenerCalidadErroresRO_COEDetalle(idTarea, mes, ano, verPor, idInstrumento, Nothing)
		gvDatosDetalleCOE.DataBind()

		gvDatosResponsable.DataSource = da.obtenerCalidadErroresRO_ResponsableTarea(idTarea, mes, ano, verPor, idInstrumento)
		gvDatosResponsable.DataBind()

		gvDatosDetalleResponsable.DataSource = da.obtenerCalidadErroresRO_ResponsableTareaDetalle(idTarea, mes, ano, verPor, idInstrumento, Nothing)
		gvDatosDetalleResponsable.DataBind()

		updDatos.Update()
		updDatosDetalle.Update()

	End Sub

	Private Sub ddlTareas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTareas.SelectedIndexChanged
		If ddlTareas.SelectedItem.Text = "Instrumentos" Then
			ddlInstrumentos.Visible = True
			lblInstrumento.Visible = True
		Else
			ddlInstrumentos.Visible = False
			lblInstrumento.Visible = False
			ddlInstrumentos.ClearSelection()
		End If
		gvDatosGerente.DataSource = Nothing
		gvDatosGerente.DataBind()
		gvDatosCOE.DataSource = Nothing
		gvDatosCOE.DataBind()
		gvDatosResponsable.DataSource = Nothing
		gvDatosResponsable.DataBind()
	End Sub

	Private Sub btnExcelDetalleGerente_Click(sender As Object, e As EventArgs) Handles btnExcelDetalleGerente.Click
		Dim da As New RegistroObservaciones

		Dim verPor = [Enum].Parse(RegistroObservaciones.ETiposAgrupacion.Total.GetType, ddlVerPor.SelectedValue)
		Dim ano = If(ddlAno.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlAno.SelectedValue))
		Dim mes = If(ddlMes.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlMes.SelectedValue))
		Dim idInstrumento = If(ddlInstrumentos.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlInstrumentos.SelectedValue))
		Dim idTarea = If(ddlTareas.SelectedValue = "", DirectCast(Nothing, Short?), Short.Parse(ddlTareas.SelectedValue))
		Dim listaErrores = da.obtenerCalidadErroresRO_GerenteProyectosDetalle(idTarea, mes, ano, verPor, idInstrumento, Nothing)

		Dim wb As New XLWorkbook

		Dim titulosProduccion As String = "JOBBOOK;TRABAJO ID;TAREA;DOCUMENTO;AÑO;MES;USUARIO;GRUPOUNIDAD;TIPO DE OBSERVACIÓN"

		Dim ws = wb.Worksheets.Add("DetErroresRegistroObservaciones")
		insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

		ws.Cell(2, 1).InsertData(listaErrores)

		exportarExcel(wb, "Detalles de Errores RO Gerente")
	End Sub

	Private Sub btnExcelDetalleCOE_Click(sender As Object, e As EventArgs) Handles btnExcelDetalleCOE.Click
		Dim da As New RegistroObservaciones

		Dim verPor = [Enum].Parse(RegistroObservaciones.ETiposAgrupacion.Total.GetType, ddlVerPor.SelectedValue)
		Dim ano = If(ddlAno.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlAno.SelectedValue))
		Dim mes = If(ddlMes.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlMes.SelectedValue))
		Dim idInstrumento = If(ddlInstrumentos.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlInstrumentos.SelectedValue))
		Dim idTarea = If(ddlTareas.SelectedValue = "", DirectCast(Nothing, Short?), Short.Parse(ddlTareas.SelectedValue))
		Dim listaErrores = da.obtenerCalidadErroresRO_COEDetalle(idTarea, mes, ano, verPor, idInstrumento, Nothing)

		Dim wb As New XLWorkbook

		Dim titulosProduccion As String = "JOBBOOK;TRABAJO ID;TAREA;DOCUMENTO;AÑO;MES;USUARIO;GRUPOUNIDAD;TIPO DE OBSERVACIÓN"

		Dim ws = wb.Worksheets.Add("DetErroresRegistroObservaciones")
		insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

		ws.Cell(2, 1).InsertData(listaErrores)

		exportarExcel(wb, "Detalles de Errores RO Gerente")
	End Sub

	Private Sub btnExcelDetalleResponsable_Click(sender As Object, e As EventArgs) Handles btnExcelDetalleResponsable.Click
		Dim da As New RegistroObservaciones

		Dim verPor = [Enum].Parse(RegistroObservaciones.ETiposAgrupacion.Total.GetType, ddlVerPor.SelectedValue)
		Dim ano = If(ddlAno.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlAno.SelectedValue))
		Dim mes = If(ddlMes.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlMes.SelectedValue))
		Dim idInstrumento = If(ddlInstrumentos.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlInstrumentos.SelectedValue))
		Dim idTarea = If(ddlTareas.SelectedValue = "", DirectCast(Nothing, Short?), Short.Parse(ddlTareas.SelectedValue))
		Dim listaErrores = da.obtenerCalidadErroresRO_ResponsableTareaDetalle(idTarea, mes, ano, verPor, idInstrumento, Nothing)

		Dim wb As New XLWorkbook

		Dim titulosProduccion As String = "JOBBOOK;TRABAJO ID;TAREA;DOCUMENTO;AÑO;MES;USUARIO;GRUPOUNIDAD;TIPO DE OBSERVACIÓN"

		Dim ws = wb.Worksheets.Add("DetErroresRegistroObservaciones")
		insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

		ws.Cell(2, 1).InsertData(listaErrores)

		exportarExcel(wb, "Detalles de Errores RO Gerente")
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
End Class