Imports CoreProject
Imports ClosedXML.Excel

Public Class ReporteEvaluacionProveedores
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Me.IsPostBack) Then

        End If
        ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnImgExportar)
    End Sub

    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Function obtenerEvaluacionProveedores74() As List(Of REP_EvaluacionProveedores_Result)
        Dim daGestionCalidad As New GestionCalidad
        Dim FechaInicio As Date? = If(String.IsNullOrEmpty(txtFechaInicio.Text), CType(Nothing, Date?), CType(txtFechaInicio.Text, Date?))
        Dim FechaFin As Date? = If(String.IsNullOrEmpty(txtFechaFin.Text), CType(Nothing, Date?), CType(txtFechaFin.Text, Date?))
        Return daGestionCalidad.obtenerEvaluacionProveedoresCalle74(FechaInicio, FechaFin)
    End Function

    Function obtenerEvaluacionProveedores78() As List(Of REP_EvaluacionProveedores_Ops_Result)
        Dim daGestionCalidad As New GestionCalidad
        Dim FechaInicio As Date? = If(String.IsNullOrEmpty(txtFechaInicio.Text), CType(Nothing, Date?), CType(txtFechaInicio.Text, Date?))
        Dim FechaFin As Date? = If(String.IsNullOrEmpty(txtFechaFin.Text), CType(Nothing, Date?), CType(txtFechaFin.Text, Date?))
        Return daGestionCalidad.obtenerEvaluacionProveedoresCalle78(FechaInicio, FechaFin)
    End Function

    Sub exportarExcelEvaluacionCalle74()
        Dim wb As New XLWorkbook
        Dim oLstReporteCalle74 As List(Of REP_EvaluacionProveedores_Result)
        Dim titulosProduccion As String = "IdFactura;ProveedorId;NombreProveedor;FechaEvaluacion;Como evaluaría el servicio prestado en el último MES;¿Podría decirme por qué ha dado usted esa calificación a calidad del producto servicio en general?;La eficiencia, entendida como las actividades realizadas para obtener el resultado:;¿Podría decirme por qué ha dado usted esa calificación a la eficiencia, entendida como las actividades realizadas para obtener el resultado? ;Cumplimiento de tiempos pactados (cronograma).;¿Podría decirme por qué ha dado usted esa calificación a cumplimiento de tiempos pactados (cronograma)?;Cumplimiento de los objetivos.:;¿Podría decirme por qué ha dado usted esa calificación a cumplimiento de los objetivos.?;Cumplimiento de privacidad de datos y confidencialidad.;¿Podría decirme por qué ha dado usted esa calificación a cumplimiento de privacidad de datos y confidencialidad.?;Cumplimiento de estándares ISO 9001 – 20252 requeridos.;¿Podría decirme por qué ha dado usted esa calificación a cumplimiento de estándares ISO 9001 – 20252 requeridos.?;Resultados Sesiones Operaciones Cualitativas (Aplica para proveedores de Operaciones Cualitativas).;¿Podría decirme por qué ha dado usted esa calificación a resultados Sesiones Operaciones Cualitativas (Aplica para proveedores de Operaciones Cualitativas).?;Teniendo en cuenta las calificaciones asignadas, Usted considera que la acción a seguir es …"

        Dim ws = wb.Worksheets.Add("EvaluacionProveedoresCalle74")
        insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

        oLstReporteCalle74 = obtenerEvaluacionProveedores74()
        ws.Cell(2, 1).InsertData(oLstReporteCalle74)

        exportarExcelEvaluacionCalle74(wb, "EvaluacionProveedoresCalle74")
    End Sub

    Sub exportarExcelEvaluacionCalle78()
        Dim wb As New XLWorkbook
        Dim oLstReporteCalle78 As List(Of REP_EvaluacionProveedores_Ops_Result)
        Dim titulosProduccion As String = "IdFactura;ProveedorId;NombreProveedor;FechaEvaluacion;1. Cumplimiento de tiempos:;¿Podría decirme por qué ha dado usted esa calificación a Cumplimiento de tiempos?;2. Calidad del servicio:;¿Podría decirme por qué ha dado usted esa calificación a Calidad del servicio?;3. Cumplimiento de las especificaciones e instrucciones del proyecto:;¿Podría decirme por qué ha dado usted esa calificación a Cumplimiento de las especificaciones e instrucciones del proyecto?;4. Proactividad y resolución de problemas:;¿Podría decirme por qué ha dado usted esa calificación a Proactividad y resolución de problemas?;5. Cumplimiento de estándares ISO 9001 – 20252 requeridos:;¿Podría decirme por qué ha dado usted esa calificación a Cumplimiento de estándares ISO 9001 – 20252 requeridos?;6. Cumplimiento de privacidad de datos y confidencialidad:;¿Podría decirme por qué ha dado usted esa calificación a Cumplimiento de privacidad de datos y confidencialidad?;7. Si tiene otros comentarios o recomendaciones que puedan aportar a que el proveedor mejore su servicio, por favor regístrelos a continuación:"

        Dim ws = wb.Worksheets.Add("EvaluacionProveedoresCalle78")
        insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

        oLstReporteCalle78 = obtenerEvaluacionProveedores78()
        ws.Cell(2, 1).InsertData(oLstReporteCalle78)

        exportarExcelEvaluacionCalle78(wb, "EvaluacionProveedoresCalle78")
    End Sub

    Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String, ByVal fila As Integer)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(fila, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub
    Private Sub exportarExcelEvaluacionCalle74(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""Reporte" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

    Private Sub exportarExcelEvaluacionCalle78(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""Reporte" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

    Protected Sub btnImgExportarAgrupado_Click(sender As Object, e As ImageClickEventArgs) Handles btnImgExportar.Click
        If ddlTipoReporte.SelectedValue = 74 Then
            exportarExcelEvaluacionCalle74()
        ElseIf ddlTipoReporte.SelectedValue = 78 Then
            exportarExcelEvaluacionCalle78()
        Else
            AlertJS("Debe seleccionar el Tipo de Reporte que quiere exportar")
            ddlTipoReporte.Focus()
            Exit Sub
        End If

    End Sub

End Class