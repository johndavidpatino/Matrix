Imports ClosedXML.Excel
Imports CoreProject
Public Class ReportesIndicadorCuestionarios
	Inherits System.Web.UI.Page
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			llenarDdlAnos()
		End If
	End Sub
	Function obtenerGraficaErroresTarea(erroresTarea As List(Of REP_ErroresRegistroObservaciones_Result)) As String
        Dim da As New RegistroObservaciones
        Dim categories As List(Of category)
        Dim series As New List(Of serie)
        Dim chart As New contarinerChart With {.xAxis = New xAxis, .chart = New chart With {.type = "line"}}
        Dim script As String

        categories = (From x In erroresTarea
                      Group x By key = x.Ano Into Group
                      Select New category With {.name = key, .categories = Group.GroupBy(Function(x) x.Mes).Select(Function(y) y.Key).ToList
                                                }).ToList

        series = (From x In erroresTarea
                  Group x By key = x.Grupo Into Group
                  Select New serie With {.name = key, .type = "line", .data = (
                                                                                From y In Group Select If(y.Porcentaje.HasValue, y.Porcentaje.Value, CShort(0))
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

        erroresRegistroObservaciones = da.obtenerPorcentajesErrorRegistroObservaciones(idTarea, mes, ano, verPor, idInstrumento)
        erroresRegistroObservacionesDetalle = da.obtenerPorcentajesErrorRegistroObservacionesDetalle(idTarea, mes, ano, verPor, idInstrumento, Nothing)
        grafica = obtenerGraficaErroresTarea(erroresRegistroObservaciones)
        gvDatos.DataSource = erroresRegistroObservaciones
        gvDatos.DataBind()

        gvDatosDetalle.DataSource = erroresRegistroObservacionesDetalle
        gvDatosDetalle.DataBind()

        Dim ListaUsuario As List(Of String) = ((From s In erroresRegistroObservacionesDetalle Select s.Usuario Order By Usuario).Distinct()).ToList()
        Session.Add("erroresRegistroObservacionesDetalle", erroresRegistroObservacionesDetalle)

        ddlUsuario.Items.Clear()
        ddlUsuario.DataSource = ListaUsuario
        ddlUsuario.DataBind()
        ddlUsuario.Items.Insert(0, New ListItem("--Todos--", ""))

        updGrafica.Update()
        updDatos.Update()
        updDatosDetalle.Update()

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "Highchart_" & contenedorGrafica.ClientID, grafica, True)
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
        gvDatos.DataSource = Nothing
        gvDatos.DataBind()
    End Sub

    Private Sub btnExcelDetalle_Click(sender As Object, e As EventArgs) Handles btnExcelDetalle.Click
        Dim da As New RegistroObservaciones

        Dim verPor = [Enum].Parse(RegistroObservaciones.ETiposAgrupacion.Total.GetType, ddlVerPor.SelectedValue)
        Dim ano = If(ddlAno.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlAno.SelectedValue))
        Dim mes = If(ddlMes.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlMes.SelectedValue))
        Dim idInstrumento = If(ddlInstrumentos.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlInstrumentos.SelectedValue))
        Dim idTarea = If(ddlTareas.SelectedValue = "", DirectCast(Nothing, Short?), Short.Parse(ddlTareas.SelectedValue))
        Dim usuario = ddlUsuario.SelectedValue.ToString
        Dim listaErrores As List(Of REP_ErroresRegistroObservacionesDetalle_Result)
        If usuario <> "" Then
            listaErrores = da.obtenerPorcentajesErrorRegistroObservacionesDetalle(idTarea, mes, ano, verPor, idInstrumento, usuario)
        Else
            listaErrores = da.obtenerPorcentajesErrorRegistroObservacionesDetalle(idTarea, mes, ano, verPor, idInstrumento, Nothing)
        End If

        Dim wb As New XLWorkbook

        Dim titulosProduccion As String = "JOBBOOK;TRABAJO ID;TAREA;DOCUMENTO;AÑO;MES;USUARIO;GRUPOUNIDAD;TIPO DE OBSERVACIÓN"

        Dim ws = wb.Worksheets.Add("DetErroresRegistroObservaciones")
        insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

        ws.Cell(2, 1).InsertData(listaErrores)

        exportarExcel(wb, "Detalles de Errores Registro Observaciones")
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

    Private Sub ddlUsuario_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUsuario.SelectedIndexChanged
        Dim usuario = ddlUsuario.SelectedValue.ToString
        Dim Detalle As List(Of REP_ErroresRegistroObservacionesDetalle_Result)
        Detalle = Session("erroresRegistroObservacionesDetalle")
        If usuario = "" Then
            gvDatosDetalle.DataSource = Detalle
        Else
            gvDatosDetalle.DataSource = Detalle.Where(Function(x) x.Usuario = usuario).Distinct()
        End If

        gvDatosDetalle.DataBind()
        updDatosDetalle.Update()
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
        Private _data As List(Of Short)
        Public Property data() As List(Of Short)
            Get
                Return _data
            End Get
            Set(ByVal value As List(Of Short))
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