Imports CoreProject
Imports ClosedXML
Imports ClosedXML.Excel
Imports WebMatrix.Util
Public Class ReportePSTSinProduccion
    Inherits System.Web.UI.Page
    Dim Op As New Personas

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExport)
    End Sub

    Protected Sub BtnConsultar_Click(sender As Object, e As EventArgs) Handles BtnConsultar.Click
        If txtFechaInicio.Text <> "" And txtFechaTerminacion.Text <> "" Then
            CargarInformacion(txtFechaInicio.Text, txtFechaTerminacion.Text)
        Else
            AlertJs("Ingrese Fechas de busqueda")
        End If
    End Sub

    Sub CargarInformacion(ByVal Fechaini As Date, ByVal FechaFin As Date)
        If Op.Th_PStSinProduccion(Fechaini, FechaFin).Count > 0 Then
            GVPersonas.DataSource = Op.Th_PStSinProduccion(Fechaini, FechaFin)
            GVPersonas.DataBind()
        End If
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If txtFechaInicio.Text <> "" And txtFechaTerminacion.Text <> "" Then
            Exportar(txtFechaInicio.Text, txtFechaTerminacion.Text)
        Else
            AlertJs("Ingrese Fechas de busqueda")
        End If

    End Sub
    Sub Exportar(ByVal Fechaini As Date, ByVal FechaFin As Date)
        Dim excel As New List(Of Array)
        Dim Titulos As String = "ID;NOMBRE;APELLIDO;CARGO"
        Dim DynamicColNames() As String
        Dim lstCambios As List(Of TH_ReportePSTSinProduccionXFecha_Result)
        Dim workbook = New XLWorkbook()
        Dim worksheet = workbook.Worksheets.Add("CedulasSinProduccion")

        DynamicColNames = Titulos.Split(CChar(";"))
        excel.Add(DynamicColNames)
        Dim o As New ProcesosInternos
        lstCambios = Op.Th_PStSinProduccion(Fechaini, FechaFin)
        For Each x In lstCambios
            excel.Add((x.id & "|" & x.Nombres & "|" & x.Apellidos & "|" & x.Cargo).Split(CChar("|")).ToArray())
        Next
        worksheet.Cell("A1").Value = excel

        worksheet.Columns("A", "S").AdjustToContents(1, 2)
        'worksheet.Columns("X", "AD").AdjustToContents(1, 2)
        'worksheet.Columns("AE", "AG").AdjustToContents(1, 2)

        Crearexcel(workbook, "CedulasSinProduccion" & Fechaini & "_HASTA_ID_" & FechaFin)
    End Sub

    Private Sub Crearexcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)

            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub
    Sub AlertJs(ByVal message As String)
        ClientScript.RegisterStartupScript(Me.GetType(), "AlertScript", "alert('" & message & "');", True)
    End Sub
End Class