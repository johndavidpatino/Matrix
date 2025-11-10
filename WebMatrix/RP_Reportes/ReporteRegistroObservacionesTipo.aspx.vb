Imports ClosedXML.Excel
Imports CoreProject
Imports WebMatrix.Util

Public Class ReporteRegistroObservacionesTipo
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			llenarDdlAnos()
		End If
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

	Private Sub btnAcualizar_Click(sender As Object, e As EventArgs) Handles btnAcualizar.Click
		Dim da As New RegistroObservaciones

		Dim verPor = [Enum].Parse(RegistroObservaciones.ETiposAgrupacionTipo.Tarea.GetType, ddlVerPor.SelectedValue)
		Dim ano = If(ddlAno.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlAno.SelectedValue))
		Dim mes = If(ddlMes.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlMes.SelectedValue))
		Dim idInstrumento = If(ddlInstrumentos.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlInstrumentos.SelectedValue))
		Dim idTarea = If(ddlTareas.SelectedValue = "", DirectCast(Nothing, Short?), Short.Parse(ddlTareas.SelectedValue))
		Dim idObservacion = If(ddlTipos.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlTipos.SelectedValue))

		Dim oRegistroObservaciones = Nothing
		If verPor = RegistroObservaciones.ETiposAgrupacionTipo.Tarea Then
			If ddlTareas.SelectedItem.Text = "Instrumentos" Then
				oRegistroObservaciones = da.obtenerRegistroObservacionesInstrumento(Nothing, idTarea, idInstrumento, idObservacion, ano, mes, Nothing)
			Else
				oRegistroObservaciones = da.obtenerRegistroObservacionesTarea(Nothing, idTarea, idInstrumento, idObservacion, ano, mes, Nothing)
			End If
		ElseIf verPor = RegistroObservaciones.ETiposAgrupacionTipo.Tipo Then
			oRegistroObservaciones = da.obtenerRegistroObservacionesTipo(Nothing, idTarea, idInstrumento, idObservacion, ano, mes, Nothing)
		ElseIf verPor = RegistroObservaciones.ETiposAgrupacionTipo.Persona Then
			oRegistroObservaciones = da.obtenerRegistroObservacionesUsuario(Nothing, idTarea, idInstrumento, idObservacion, ano, mes, Nothing)
		Else
			ShowNotification("No se puede agrupar por la opción indicada", ShowNotifications.ErrorNotification)
			Exit Sub
		End If

		Dim oRegistroObservacionesDetalle As List(Of REP_RegistroObservaciones_Result) = da.obtenerRegistroObservacionesDetalle(Nothing, idTarea, idInstrumento, idObservacion, ano, mes, Nothing)
		If oRegistroObservacionesDetalle.Count <= 0 Then
			ShowNotification("No se encontraron datos del filtro indicado", ShowNotifications.ErrorNotification)
			Exit Sub
		End If

		gvDatos.DataSource = oRegistroObservaciones
		gvDatos.DataBind()
		gvDatosDetalle.DataSource = oRegistroObservacionesDetalle
		gvDatosDetalle.DataBind()

		Dim ListaUsuario As List(Of String) = ((From s In oRegistroObservacionesDetalle Select s.Usuario Order By Usuario).Distinct()).ToList()
		Session.Add("RegistroObservacionesDetalle", oRegistroObservacionesDetalle)

		ListaUsuario.RemoveAll(Function(x) x Is Nothing)
		ddlUsuario.Items.Clear()
		ddlUsuario.DataSource = ListaUsuario
		ddlUsuario.DataBind()
		ddlUsuario.Items.Insert(0, New ListItem("--Todos--", ""))

		updDatos.Update()
		updDatosDetalle.Update()
	End Sub

	Private Sub ddlUsuario_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUsuario.SelectedIndexChanged
		Dim usuario = ddlUsuario.SelectedValue.ToString
		Dim Detalle As List(Of REP_RegistroObservaciones_Result)
		Detalle = Session("RegistroObservacionesDetalle")
		If usuario = "" Then
			gvDatosDetalle.DataSource = Detalle
		Else
			gvDatosDetalle.DataSource = Detalle.Where(Function(x) x.Usuario = usuario).Distinct()
		End If

		gvDatosDetalle.DataBind()
		updDatosDetalle.Update()
	End Sub

	Private Sub btnExcelDetalle_Click(sender As Object, e As EventArgs) Handles btnExcelDetalle.Click
		Dim da As New RegistroObservaciones

		Dim verPor = [Enum].Parse(RegistroObservaciones.ETiposAgrupacion.Total.GetType, ddlVerPor.SelectedValue)
		Dim ano = If(ddlAno.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlAno.SelectedValue))
		Dim mes = If(ddlMes.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlMes.SelectedValue))
		Dim idInstrumento = If(ddlInstrumentos.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlInstrumentos.SelectedValue))
		Dim idTarea = If(ddlTareas.SelectedValue = "", DirectCast(Nothing, Short?), Short.Parse(ddlTareas.SelectedValue))
		Dim idObservacion = If(ddlTipos.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlTipos.SelectedValue))
		Dim usuario = ddlUsuario.SelectedValue.ToString
		Dim lista As List(Of REP_RegistroObservaciones_Result)
		If usuario <> "" Then
			lista = da.obtenerRegistroObservacionesDetalle(Nothing, idTarea, idInstrumento, idObservacion, ano, mes, usuario)
		Else
			lista = da.obtenerRegistroObservacionesDetalle(Nothing, idTarea, idInstrumento, idObservacion, ano, mes, Nothing)
		End If

		Dim wb As New XLWorkbook

		Dim titulosProduccion As String = "TrabajoId;Tarea;Instrumento;Ano;Mes;Usuario;Grupo_Unidad;Observacion;DescripcionObservacion;FechaHoraObservacion"

		Dim ws = wb.Worksheets.Add("RegistroObservacionesDetalle")
		insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

		ws.Cell(2, 1).InsertData(lista)

		exportarExcel(wb, "Registro Observaciones Por Tipo")
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