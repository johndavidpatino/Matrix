Imports System.IO
Imports WebMatrix.Util
Imports System.Web.UI.DataVisualization.Charting

Public Class REP_PersonalSinProduccion
    Inherits System.Web.UI.Page

    '#Region "Eventos"
    '    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '        If Not IsPostBack Then
    '            Dim o As New CoreProject.OP_Cuanti
    '            Me.txtFechaTerminacion.Text = Now.Date
    '            Me.txtFechaInicio.Text = DateAdd(DateInterval.Month, -1, Now.Date)

    '            GraficaGeneral(txtFechaInicio.Text, txtFechaTerminacion.Text)
    '            InformeDetalle(txtFechaInicio.Text, txtFechaTerminacion.Text)
    '        End If
    '        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
    '        smanager.RegisterPostBackControl(Me.btnExportar)
    '    End Sub

    '    Protected Sub btnExportar_Click(sender As Object, e As System.EventArgs) Handles btnExportar.Click
    '        Me.gvExportado.Visible = True
    '        'Actualiza los datos del gridview
    '        Me.gvExportado.DataBind()
    '        'Crea variables en memoria para almacenar el contenido del gridview
    '        Dim sb As StringBuilder = New StringBuilder()
    '        Dim sw As StringWriter = New StringWriter(sb)
    '        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
    '        'Crea una nueva página html y un form dentro de ella
    '        Dim pagina As Page = New Page
    '        Dim form = New HtmlForm
    '        'Cambie el estado del gridview para que no guarde los controles de vista
    '        Me.gvExportado.EnableViewState = False
    '        'Quita la validación de la página 
    '        pagina.EnableEventValidation = False
    '        pagina.DesignerInitialize()
    '        'Agrega el form creado
    '        pagina.Controls.Add(form)
    '        'Agrega el gridview al form
    '        form.Controls.Add(gvExportado)
    '        'Hace un render para pasar los valores de la página al htmltextwriter que contendrá los datos del gridview
    '        pagina.RenderControl(htw)
    '        'Hace un response para descargar el control
    '        Response.Clear()
    '        Response.Buffer = True
    '        Response.ContentType = "application/vnd.ms-excel"
    '        Response.AddHeader("Content-Disposition", "attachment;filename=ExportadoBaseAnuladas.xls")
    '        Response.Charset = "UTF-8"

    '        Response.ContentEncoding = Encoding.Default
    '        Response.Write(sb.ToString())
    '        Response.End()
    '        'Incluya esta línea si el gridview está oculto
    '        gvExportado.Visible = False

    '    End Sub

    '#End Region

    '#Region "Metodos"
    '    Sub GraficaGeneral(ByVal fInicio As Date, fFin As Date)
    '        Dim o As New CoreProject.Reportes.InformeAnuladas
    '        Dim info = o.ObtenerDetalleAnuladas(fInicio, fFin)
    '        Me.ChartEstado.Series.Clear()
    '        Dim serie As New Series
    '        serie.Points.DataBind(info, "CIUDAD", "PORCANULACION", "")
    '        serie.IsValueShownAsLabel = True
    '        serie.LabelFormat = "0.00%"
    '        serie.Points.Item(0).Color = Drawing.Color.FromArgb(255, 54, 96, 146)
    '        serie.Points.Item(1).Color = Drawing.Color.FromArgb(255, 149, 179, 215)
    '        serie.Points.Item(2).Color = Drawing.Color.FromArgb(255, 220, 230, 241)
    '        serie.Points.Item(3).Color = Drawing.Color.FromArgb(255, 252, 213, 180)
    '        serie.Points.Item(4).Color = Drawing.Color.FromArgb(255, 250, 191, 143)
    '        serie.Points.Item(5).Color = Drawing.Color.FromArgb(255, 226, 107, 10)
    '        ChartEstado.Series.Add(serie)


    '        'Dim yValues() As Double = {}
    '        'Dim xValues() As String = {}
    '        'For Each element As CoreProject.REP_InformeAnuladasxCiudad_Result In info
    '        '    Dim serie As New Series
    '        '    Dim yValues() As Double = {element.PORCANULACION}
    '        '    Dim xValues() As String = {element.CIUDAD}
    '        '    serie.Name = element.CIUDAD
    '        '    serie.ChartType = SeriesChartType.Column
    '        '    serie.Points.DataBindXY(xValues, yValues)
    '        '    ChartEstado.Series.Add(serie)
    '        'Next
    '        'If info.fecha Is Nothing Then Exit Sub
    '        'Me.ChartEstado.Series.Clear()
    '        'Dim yValues() As Double = {info.fecha, info.ejecucion}
    '        'Dim xValues() As String = {"En Fecha", "En Muestra"}
    '        'Me.ChartEstado.Series.Add("Fecha")
    '        'Me.ChartEstado.Series("Fecha").Points.DataBindXY(xValues, yValues)
    '        'Me.ChartEstado.Series("Fecha").IsValueShownAsLabel = True
    '        'Me.ChartEstado.Series("Fecha").LabelFormat = "0.00%"

    '        'Me.ChartEstado.Series("Fecha").BackGradientStyle = DataVisualization.Charting.GradientStyle.VerticalCenter
    '        'Me.ChartEstado.Series("Fecha").Color = Drawing.Color.MediumSlateBlue

    '    End Sub

    '    Sub InformeDetalle(ByVal fInicio As Date, fFin As Date)
    '        Dim o As New CoreProject.Reportes.InformeAnuladas
    '        Try
    '            Me.gvInformeAnulacion.DataSource = o.ObtenerDetalleAnuladas(fInicio, fFin)
    '            Me.gvInformeAnulacion.DataBind()
    '        Catch ex As Exception

    '        End Try

    '    End Sub

    '#End Region


    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        Dim o As New CoreProject.CoordinacionCampoPersonal
        Me.gvDatos.DataSource = o.ListadoPersonalSinProduccion(txtFechaInicio.Text, ddlCargo.SelectedValue)
        Me.gvDatos.DataBind()

        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub
End Class