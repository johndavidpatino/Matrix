Imports CoreProject
Imports WebMatrix.Util
Imports ClosedXML
Imports ClosedXML.Excel


Public Class ReporteCambiosContratacion
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExportar)
    End Sub

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        ObtenerInformacion(If(String.IsNullOrEmpty(txtFechaInicio.Text), CType(Nothing, Date?), CDate(txtFechaInicio.Text)), If(String.IsNullOrEmpty(txtFechaFinalizacion.Text), CType(Nothing, Date?), CDate(txtFechaFinalizacion.Text)), If(String.IsNullOrEmpty(txtcedula.Text), CType(Nothing, Long?), CLng(txtcedula.Text)))
    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        Exportar(If(String.IsNullOrEmpty(txtFechaInicio.Text), CType(Nothing, Date?), CDate(txtFechaInicio.Text)), If(String.IsNullOrEmpty(txtFechaFinalizacion.Text), CType(Nothing, Date?), CDate(txtFechaFinalizacion.Text)), If(String.IsNullOrEmpty(txtcedula.Text), CType(Nothing, Long?), CLng(txtcedula.Text)))
    End Sub
#End Region

#Region "Metodos"

    Public Sub ObtenerInformacion(ByVal Fecini As Date?, ByVal Fecfin As Date?, ByVal Cedula As Int64?)
        Dim op As New Personas
        gvPersonas.DataSource = op.Th_REPCambiosContratacion(Fecini, Fecfin, Cedula)
        gvPersonas.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Protected Sub Exportar(ByVal Fecini As Date?, ByVal Fecfin As Date?, ByVal Cedula As Int64?)
        Dim vTitulos As String() = "Cedula;Nombres;Campo;TipoContratoAnterior;TipoContratoNuevo;FechaCambio;UsuarioId;UsuarioRealizaCambio".Split(";")
        Dim lstCambios As List(Of TH_REPCambiosContratacion_Result)
        Dim workbook = New XLWorkbook()
        Dim worksheet = workbook.Worksheets.Add("ReporteCambiosContratacion")


        Dim op As New Personas
        lstCambios = op.Th_REPCambiosContratacion(Fecini, Fecfin, Cedula)
        worksheet.Cell("A2").InsertData(lstCambios)
        For x = 1 To vTitulos.Count
            worksheet.Cell(1, x).Value = vTitulos(x - 1)
        Next


        Crearexcel(workbook, "ReporteCambiosContratacion -" & Now.Year & "-" & Now.Month & "-" & Now.Day)
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

#End Region


End Class