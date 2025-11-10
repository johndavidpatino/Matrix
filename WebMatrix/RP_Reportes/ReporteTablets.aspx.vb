Imports CoreProject
Imports ClosedXML.Excel

Public Class ReporteTablets
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Me.IsPostBack) Then

        End If
        ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnImgExportarAgrupado)
        ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnImgExportarDiario)
    End Sub

    Sub llenadoNewTable()
        Dim daCampo As New Campo
        daCampo.LlenadoNewTable()
    End Sub

    Sub cargarReporteAgrupado()
        gvAgrupado.DataSource = obtenerReporteTabletsAgrupado()
        gvAgrupado.DataBind()
    End Sub

    Sub cargarReporteDiario()
        gvDiario.DataSource = obtenerReporteTabletsDiario()
        gvDiario.DataBind()
    End Sub

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        'llenadoNewTable()
        cargarReporteAgrupado()
        cargarReporteDiario()
        btnImgExportarAgrupado.Visible = True
        btnImgExportarDiario.Visible = True
        upReporteTablets.Update()
    End Sub

    Private Sub gvAgrupado_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvAgrupado.PageIndexChanging
        gvAgrupado.PageIndex = e.NewPageIndex
        cargarReporteAgrupado()
        upReporteTablets.Update()
    End Sub

    Private Sub gvDiario_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvDiario.PageIndexChanging
        gvDiario.PageIndex = e.NewPageIndex
        cargarReporteDiario()
        upReporteTablets.Update()
    End Sub

    Function obtenerReporteTabletsAgrupado() As List(Of REP_InformeTablets_Agrupado_Result)
        Dim daCampo As New Campo
        Dim FechaInicio As Date? = If(String.IsNullOrEmpty(txtFechaInicio.Text), CType(Nothing, Date?), CType(txtFechaInicio.Text, Date?))
        Dim FechaFin As Date? = If(String.IsNullOrEmpty(txtFechaFin.Text), CType(Nothing, Date?), CType(txtFechaFin.Text, Date?))
        Return daCampo.obtenerReporteTabletsAgrupado(FechaInicio, FechaFin)
    End Function

    Function obtenerReporteTabletsDiario() As List(Of REP_InformeTablets_Diario_Result)
        Dim daCampo As New Campo
        Dim FechaInicio As Date? = If(String.IsNullOrEmpty(txtFechaInicio.Text), CType(Nothing, Date?), CType(txtFechaInicio.Text, Date?))
        Dim FechaFin As Date? = If(String.IsNullOrEmpty(txtFechaFin.Text), CType(Nothing, Date?), CType(txtFechaFin.Text, Date?))
        Return daCampo.obtenerReporteTabletsDiario(FechaInicio, FechaFin)
    End Function

    Sub exportarExcelAgrupado()
        Dim wb As New XLWorkbook
        Dim oLstReporteAgrupado As List(Of REP_InformeTablets_Agrupado_Result)
        Dim titulosProduccion As String = "TabletsDisponibles;TabletsConSIM;TabletsEnCampo;TabletConSIM_TabletDisponible;TabletConProduccion_TabletDisponible;TabletConProduccion_TabletConSIM;TabletConProduccion_TabletEnCampo;DiasCalendario;DiasLaborales;DiasTabletDisponiblesCalendario;DiasTabletDisponiblesLaborales;TabletsConProduccion;TotalDiasTabletConProduccion;CantidadEncuestas;PromedioProduccionTabletsDisponibles;PromedioProduccionTabletsEnCampo;PromedioProduccionTabletsConProduccion"

        Dim ws = wb.Worksheets.Add("TabletsAgrupado")
        insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

        oLstReporteAgrupado = obtenerReporteTabletsAgrupado()
        ws.Cell(2, 1).InsertData(oLstReporteAgrupado)

        exportarExcelAgrupado(wb, "TabletsAgrupado")
    End Sub

    Sub exportarExcelDiario()
        Dim wb As New XLWorkbook
        Dim oLstReporteDiario As List(Of REP_InformeTablets_Diario_Result)
        Dim titulosProduccion As String = "FECHA;TabletsDisponibles;TabletsConSIM;TabletsEnCampo;TabletConSIM_TabletDisponible;TabletConProduccion_TabletDisponible;TabletConProduccion_TabletConSIM;TabletConProduccion_TabletEnCampo;DiasCalendario;DiasLaborales;DiasTabletDisponiblesCalendario;DiasTabletDisponiblesLaborales;TabletsConProduccion;TotalDiasTabletConProduccion;CantidadEncuestas;PromedioProduccionTabletsDisponibles;PromedioProduccionTabletsEnCampo;PromedioProduccionTabletsConProduccion"

        Dim ws = wb.Worksheets.Add("TabletsDiario")
        insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

        oLstReporteDiario = obtenerReporteTabletsDiario()
        ws.Cell(2, 1).InsertData(oLstReporteDiario)

        exportarExcelDiario(wb, "TabletsDiario")
    End Sub

    Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String, ByVal fila As Integer)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(fila, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub
    Private Sub exportarExcelAgrupado(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""Reporte" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

    Private Sub exportarExcelDiario(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""Reporte" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

    Protected Sub btnImgExportarAgrupado_Click(sender As Object, e As ImageClickEventArgs) Handles btnImgExportarAgrupado.Click
        exportarExcelAgrupado()
    End Sub

    Protected Sub btnImgExportarDiario_Click(sender As Object, e As ImageClickEventArgs) Handles btnImgExportarDiario.Click
        exportarExcelDiario()
    End Sub
End Class