Imports CoreProject
Imports ClosedXML.Excel

Public Class CostosJBExternoObserver
    Inherits System.Web.UI.Page
    Dim _Consultas As New IQ.Consultas
    Dim _Total As Decimal = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            CargarCostos(CType(Session("PARAMETROS"), IQ_Parametros))

        End If

    End Sub

    Private Sub CargarCostos(ByVal p As IQ_Parametros)
        gvJBExterno.DataSource = _Consultas.ObtenerCostosJobBookExternoObserver(p).Tables(0)
        gvJBExterno.DataBind()
    End Sub



    Protected Sub gvJBExterno_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvJBExterno.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            _Total = _Total + CDec(e.Row.Cells(3).Text)

            If e.Row.Cells(2).Text.ToString.IndexOf("PORCENTAJE") > -1 Then
                e.Row.Cells(3).Text = CDec(e.Row.Cells(3).Text).ToString("P")
            Else
                e.Row.Cells(3).Text = CDec(e.Row.Cells(3).Text).ToString("C0")
            End If



            If e.Row.Cells(2).Text.ToString.IndexOf("TOTAL") > -1 Or e.Row.Cells(2).Text.ToString.IndexOf("GROSS") > -1 Or e.Row.Cells(2).Text.ToString.IndexOf("VENTA") > -1 Then
                e.Row.Font.Bold = True
            End If

            e.Row.Cells(3).CssClass = "RightAlign"

        End If



    End Sub


    Protected Sub gvJBExterno_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gvJBExterno.SelectedIndexChanged

    End Sub

    Protected Sub ExpToExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ExpToExcel.Click
        ExportarExcel(gvJBExterno)
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        'dont delete - needed for excel export
    End Sub


    Private Sub ExportarExcel(ByVal gv As GridView)
        'Response.Clear()
        'Response.Buffer = True

        'Response.Charset = ""
        'Response.ContentType = "application/vnd.ms-excel"
        'Response.AddHeader("Content-Disposition", "attachment;filename= JB_externo.xls")

        'Dim oStringWriter As System.IO.StringWriter = New System.IO.StringWriter()
        'Dim oHtmlTextWriter As System.Web.UI.HtmlTextWriter = New System.Web.UI.HtmlTextWriter(oStringWriter)

        'gv.RenderControl(oHtmlTextWriter)

        'Response.Write(oStringWriter.ToString())
        'Response.End()
        Dim wb = New XLWorkbook()
        wb.Worksheets.Add(_Consultas.ObtenerCostosJobBookExternoObserver(CType(Session("PARAMETROS"), IQ_Parametros)).Tables(0))
        Crearexcel(wb, "JB_externo")

    End Sub

    Private Sub Crearexcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""JB_Externo.xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)

            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub


    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButton1.Click
        Dim par As IQ_Parametros
        par = CType(Session("PARAMETROS"), IQ_Parametros)
        If Request.QueryString("ACCION") IsNot Nothing Then
            Response.Redirect("Cap_Principal.aspx?IdPropuesta=" & par.IdPropuesta.ToString() & "&Alternativa=" & par.ParAlternativa.ToString() & "&ACCION=" & Request.QueryString("ACCION"))
        Else
            Response.Redirect("Cap_Principal.aspx?IdPropuesta=" & par.IdPropuesta.ToString() & "&Alternativa=" & par.ParAlternativa.ToString())
        End If
    End Sub





End Class