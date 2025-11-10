Imports CoreProject
Imports ClosedXML
Imports ClosedXML.Excel
Imports System
Imports System.Net
Public Class TrabajosSinPresupuesto
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        Dim po As New PresupInt
        Me.gvTrabajos.DataSource = po.ObtenerTrabajosSinPresupuesto(CDate(txtFechaInicio.Text), CDate(txtFechaFinalizacion.Text))
        Me.gvTrabajos.DataBind()
    End Sub
    Protected Sub btnexportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        Dim excel As New List(Of Array)
        Dim Titulos As String = "Trabajo;Ntrabajo;JobBook;COE;Fecha;Observacion"
        Dim DynamicColNames() As String

        Dim workbook = New XLWorkbook()
        Dim worksheet = workbook.Worksheets.Add("TrabajosSinPresupuesto")

        DynamicColNames = Titulos.Split(CChar(";"))
        excel.Add(DynamicColNames)
        Dim op As New PresupInt

        Dim DatosTrabajos() As String
        Dim WDatos As String

        For Each row As GridViewRow In gvTrabajos.Rows
            WDatos = row.Cells(0).Text & ";" & HttpUtility.HtmlDecode(row.Cells(1).Text) & ";" & row.Cells(2).Text & ";" & HttpUtility.HtmlDecode(row.Cells(3).Text) & ";" & row.Cells(4).Text & ";" & row.Cells(5).Text
            DatosTrabajos = WDatos.Split(CChar(";"))
            excel.Add(DatosTrabajos)
        Next

        worksheet.Cell("A1").Value = excel
        Crearexcel(workbook, "TrabajosSinPresupuesto-" & Now.Year & "-" & Now.Month & "-" & Now.Day)
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
End Class