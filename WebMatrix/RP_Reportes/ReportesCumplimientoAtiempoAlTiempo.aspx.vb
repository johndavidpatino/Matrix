Imports ClosedXML.Excel
Imports CoreProject
Public Class ReportesCumplimientoAtiempoAlTiempo
	Inherits System.Web.UI.Page

	Function obtenerGraficaIndicadoresCumplimiento(indicadoresCumplimiento As List(Of REP_IndicadoresCumplimientoAlTiempoATiempo_Result)) As String
		Dim categories As List(Of category)
		Dim series As New List(Of serie)
		Dim chart As New contarinerChart With {.xAxis = New xAxis, .chart = New chart With {.type = "line"}}
		Dim script As String

		categories = (From x In indicadoresCumplimiento
					  Group x By key = x.AÑO Into Group
					  Select New category With {.name = key, .categories = Group.GroupBy(Function(x) x.MES).Select(Function(y) y.Key).ToList
												}).ToList

		series = (From x In indicadoresCumplimiento
				  Group x By key = x.GERENTEPROYECTOS Into Group
				  Select New serie With {.name = key, .type = "line", .data = (
																				From y In Group Select y.PORCENTAJECUMPLIMIENTO
																			  ).ToList
										 }).ToList


		chart.series = series
		chart.xAxis.categories = categories

		script = "Highcharts.chart('" & contenedorGrafica.ClientID & "'," & SerializarAJSON(chart) & ");"

		Return script
	End Function
	Shared Function SerializarAJSON(Of T)(ByVal objeto As T) As String
		Dim JSON As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer
		Return JSON.Serialize(objeto)
	End Function

	Private Sub btnAcualizar_Click(sender As Object, e As EventArgs) Handles btnAcualizar.Click
		Dim grafica As String
		Dim ano As Short?
		Dim mes As Short?
		Dim indicadoresCumplimiento As List(Of REP_IndicadoresCumplimientoAlTiempoATiempo_Result)
		Dim indicadoresCumplimientoDetalle As List(Of REP_IndicadoresCumplimientoAlTiempoATiempo_Detalle_Result)
		Dim da As New RegistroObservaciones

		ano = If(ddlAno.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlAno.SelectedValue))
		mes = If(ddlMes.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlMes.SelectedValue))

		indicadoresCumplimiento = da.obtenerIndicadoresATiempoAlTiempo(mes, ano)
		indicadoresCumplimientoDetalle = da.obtenerIndicadoresATiempoAlTiempoDetalle(mes, ano)
		grafica = obtenerGraficaIndicadoresCumplimiento(indicadoresCumplimiento)
		gvDatos.DataSource = indicadoresCumplimiento.Where(Function(x) x.PORCENTAJECUMPLIMIENTO IsNot Nothing)
		gvDatos.DataBind()

		gvDatosDetalle.DataSource = indicadoresCumplimientoDetalle
		gvDatosDetalle.DataBind()

				pnlData.Visible = True
		upGraficaDatos.Update()
		updDatos.Update()

		ScriptManager.RegisterStartupScript(Me, Me.GetType, "Highchart_" & contenedorGrafica.ClientID, grafica, True)
	End Sub



	Private Sub ReportesCumplimientoTareas_Load(sender As Object, e As EventArgs) Handles Me.Load
		If Not IsPostBack Then
			llenarDdlAnos()
		End If
		Dim ScriptManager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
		ScriptManager.RegisterPostBackControl(Me.btnExcelDetalle)
	End Sub



	Private Sub btnExcelDetalle_Click(sender As Object, e As EventArgs) Handles btnExcelDetalle.Click
		Dim da As New RegistroObservaciones

		Dim ano = If(ddlAno.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlAno.SelectedValue))
		Dim mes = If(ddlMes.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlMes.SelectedValue))

		Dim listaCumplimiento As List(Of REP_IndicadoresCumplimientoAlTiempoATiempo_Detalle_Result)
		listaCumplimiento = da.obtenerIndicadoresATiempoAlTiempoDetalle(mes, ano)

		Dim wb As New XLWorkbook

		Dim titulosProduccion As String = "CÓDIGO;HILO;TAREA;PROYECTO;UNIDAD;JOBBOOK;NOMBRE TRABAJO;USUARIO;CUMPLIMIENTO;AÑO;MES;INICIO PLANEACIÓN;INICIO EJECUCIÓN;FIN PLANEACIÓN;FIN EJECUCIÓN"

		Dim ws = wb.Worksheets.Add("IndicadorCumplimiento")
		insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

		ws.Cell(2, 1).InsertData(listaCumplimiento)

		exportarExcel(wb, "Indicador de Cumplimiento")
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

	Public Class contarinerChart
		Private _chart As chart
		Public Property chart() As chart
			Get
				Return _chart
			End Get
			Set(ByVal value As chart)
				_chart = value
			End Set
		End Property
		Private _series As List(Of serie)
		Public Property series() As List(Of serie)
			Get
				Return _series
			End Get
			Set(ByVal value As List(Of serie))
				_series = value
			End Set
		End Property
		Private _xAxis As xAxis
		Public Property xAxis() As xAxis
			Get
				Return _xAxis
			End Get
			Set(ByVal value As xAxis)
				_xAxis = value
			End Set
		End Property
		Private _title As String
		Public Property title() As String
			Get
				Return _title
			End Get
			Set(ByVal value As String)
				_title = value
			End Set
		End Property
	End Class
	Public Class chart
		Private _type As String
		Public Property type() As String
			Get
				Return _type
			End Get
			Set(ByVal value As String)
				_type = value
			End Set
		End Property
	End Class
	Public Class serie
		Private _name As String
		Public Property name() As String
			Get
				Return _name
			End Get
			Set(ByVal value As String)
				_name = value
			End Set
		End Property
		Private _type As String
		Public Property type() As String
			Get
				Return _type
			End Get
			Set(ByVal value As String)
				_type = value
			End Set
		End Property
		Private _data As List(Of Byte?)
		Public Property data() As List(Of Byte?)
			Get
				Return _data
			End Get
			Set(ByVal value As List(Of Byte?))
				_data = value
			End Set
		End Property

	End Class
	Public Class xAxis

		Private _categories As List(Of category)
		Public Property categories() As List(Of category)
			Get
				Return _categories
			End Get
			Set(ByVal value As List(Of category))
				_categories = value
			End Set
		End Property
	End Class
	Public Class category
		Private _name As String
		Public Property name() As String
			Get
				Return _name
			End Get
			Set(ByVal value As String)
				_name = value
			End Set
		End Property
		Private _categories As List(Of String)
		Public Property categories() As List(Of String)
			Get
				Return _categories
			End Get
			Set(ByVal value As List(Of String))
				_categories = value
			End Set
		End Property
	End Class
	Private Sub llenarDdlAnos()
		Dim anoInicial As Integer
		Dim anoFinal As Integer

		anoFinal = Date.Now.Year

		For anoInicial = 2018 To anoFinal
			ddlAno.Items.Insert(0, New ListItem With {.Text = anoInicial, .Value = anoInicial})
		Next

	End Sub
End Class