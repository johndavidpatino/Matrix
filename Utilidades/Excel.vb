Imports ClosedXML.Excel
Imports System.Web
Imports System.Linq

Public Class ResponseExcel
    Sub responseExcel(Of T)(ByRef responseActual As HttpResponse, ByVal nombreArchivo As String, ByVal nombreHoja As String, ByVal nombresColumnas As IEnumerable, registros As IEnumerable(Of T))
        Dim workbook As New XLWorkbook
        Dim worksheet = workbook.Worksheets.Add(nombreHoja)

        responseActual.Clear()

        insertarNombresColumnas(nombresColumnas, worksheet)
        worksheet.Cell(2, 1).InsertData(registros)

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(responseActual.OutputStream)
        End Using

        responseActual.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        responseActual.AddHeader("content-disposition", "attachment;filename=""" & nombreArchivo & ".xlsx""")
        responseActual.End()

    End Sub
    Function CreateExcelToBase64(Of T)(ByVal nombreHoja As String, ByVal nombresColumnas As IEnumerable, registros As IEnumerable(Of T)) As String
        Dim workbook As New XLWorkbook
        Dim excelBase64 As String

        Dim worksheet = workbook.Worksheets.Add(nombreHoja)
        insertarNombresColumnas(nombresColumnas, worksheet)
        worksheet.Cell(2, 1).InsertData(registros)
        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            excelBase64 = Convert.ToBase64String(memoryStream.ToArray())
        End Using
        Return excelBase64
    End Function
    Private Sub insertarNombresColumnas(ByVal nombresColumnas As IEnumerable(Of String), ByRef workSheet As IXLWorksheet)
        nombresColumnas = nombresColumnas.ToList
        For indexCol = 1 To nombresColumnas.Count
            workSheet.Cell(1, indexCol).Value = nombresColumnas(indexCol - 1)
        Next
    End Sub
End Class
