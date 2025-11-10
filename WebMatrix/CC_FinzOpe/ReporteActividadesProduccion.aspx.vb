Imports WebMatrix
Imports CoreProject
Imports WebMatrix.Util
Imports ClosedXML
Imports ClosedXML.Excel
Public Class ReporteActividadesProduccion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Validar()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub
    Sub Validar()
        If txttrabajo.Text = "" And txtFechaInicio.Text = "" And txtFechaFinalizacion.Text = "" Then
            ShowNotification("Ingrese valores para la busqueda", ShowNotifications.InfoNotification)
            Exit Sub
        ElseIf txtFechaInicio.Text = "" And txtFechaFinalizacion.Text = "" Then
            CargarInformacion(txttrabajo.Text, Nothing, Nothing)
            Exit Sub
        ElseIf txttrabajo.Text = "" Then
            CargarInformacion(Nothing, txtFechaInicio.Text, txtFechaFinalizacion.Text)
            Exit Sub
        Else
            CargarInformacion(txttrabajo.Text, txtFechaInicio.Text, txtFechaFinalizacion.Text)
        End If

    End Sub
    Sub CargarInformacion(ByVal TrabajoId As Int64?, ByVal Fecini As Date?, ByVal FecFin As Date?)
        Dim op As New ProcesosInternos
        GvActividadesProduccion.DataSource = op.ReporteActividadesProduccion(TrabajoId, Fecini, FecFin)
        GvActividadesProduccion.DataBind()
    End Sub

    Protected Sub ExportarErrores()
        Dim excel As New List(Of Array)
        Dim Titulos As String = "TrabajoId;NombreTrabajo;Muestra;CodActividad;Nombre;Cantidad;VrUnitario;Total"
        Dim DynamicColNames() As String
        'Dim lstCambios As List(Of CC_ReporteActividadesProduccion_Result)
        Dim workbook = New XLWorkbook()
        Dim worksheet = workbook.Worksheets.Add("Reporte")

        DynamicColNames = Titulos.Split(CChar(";"))
        excel.Add(DynamicColNames)
        Dim op As New CC_FinzOpe
        'lstCambios = op.CC_ReporteActividadesProduccion(txttrabajo.Text, txtFechaInicio.Text, txtFechaFinalizacion.Text).ToList
        For x = 0 To GvActividadesProduccion.Rows.Count - 1
            Dim row As GridViewRow = GvActividadesProduccion.Rows(x)
            excel.Add((row.Cells(0).Text & ";" & row.Cells(1).Text & ";" & row.Cells(2).Text & ";" & row.Cells(3).Text & ";" & row.Cells(4).Text & ";" & row.Cells(5).Text & ";" & row.Cells(6).Text & ";" & row.Cells(7).Text).Split(CChar(";")).ToArray())
        Next
        worksheet.Cell("A1").Value = excel
        Crearexcel(workbook, "Reporte-" & Now.Year & "-" & Now.Month & "-" & Now.Day)
    End Sub
    Private Sub Crearexcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""" & name & ".xlsx""")

        Using memoryStream = New System.IO.MemoryStream()
            workbook.SaveAs(memoryStream)

            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        ExportarErrores()
    End Sub
End Class