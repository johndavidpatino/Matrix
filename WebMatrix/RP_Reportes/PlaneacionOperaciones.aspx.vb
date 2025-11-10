Imports System.IO
Imports WebMatrix.Util
Imports System.Web.UI.DataVisualization.Charting
Public Class PlaneacionOperaciones
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InicializarFactoresAjuste()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            smanager.RegisterPostBackControl(Me.btnExport)
        End If
        'GraficoEncuestas()
        'GraficoEncuestadores()
        'GraficoScriptingC()
        'GraficoScriptingP()
        'GraficoCriticaC()
        'GraficoCriticaP()
        'GraficoVerificacionC()
        'GraficoVerificacionP()
        'GraficoCapturaC()
        'GraficoCapturaP()
        'GraficoCodificacionC()
        'GraficoCodificacionP()
        'GraficoProcesamientoC()
        'GraficoProcesamientoP()
       
    End Sub

#Region "Campo"
    Sub GraficoEncuestas()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartEncuestas.Series.Clear()
        Dim info = o.PlaneacionEncuestas(HiddenField1.Value)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartEncuestas.Series.Add(serie)
        Next
    End Sub
    Sub GraficoEncuestadores()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartEncuestadores.Series.Clear()
        Dim info = o.PlaneacionEncuestadores(HiddenField2.Value)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartEncuestadores.Series.Add(serie)
        Next
    End Sub
#End Region

#Region "Scripting"
    Sub GraficoScriptingC()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartScriptingTrabajos.Series.Clear()
        Dim info = o.PlaneacionScriptTrabajos(HiddenField3.Value)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartScriptingTrabajos.Series.Add(serie)
        Next
    End Sub
    Sub GraficoScriptingP()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartScriptingPersonas.Series.Clear()
        Dim info = o.PlaneacionScriptPersonas(HiddenField4.Value)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartScriptingPersonas.Series.Add(serie)
        Next
    End Sub
#End Region

#Region "Critica"
    Sub GraficoCriticaC()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartCriticaEncuestas.Series.Clear()
        Dim info = o.PlaneacionCriticaEncuestas(HiddenField5.Value)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartCriticaEncuestas.Series.Add(serie)
        Next
    End Sub
    Sub GraficoCriticaP()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartCriticaPersonas.Series.Clear()
        Dim info = o.PlaneacionScriptPersonas(HiddenField6.Value)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartCriticaPersonas.Series.Add(serie)
        Next
    End Sub
#End Region

#Region "Verificacion"
    Sub GraficoVerificacionC()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartVerificacionHoras.Series.Clear()
        Dim info = o.PlaneacionVerificacionHoras(HiddenField7.Value)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartVerificacionHoras.Series.Add(serie)
        Next
    End Sub
    Sub GraficoVerificacionP()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartVerificacionPersonas.Series.Clear()
        Dim info = o.PlaneacionVerificacionPersonas(HiddenField8.Value)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartVerificacionPersonas.Series.Add(serie)
        Next
    End Sub
#End Region

#Region "Captura"
    Sub GraficoCapturaC()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartCapturaPreguntas.Series.Clear()
        Dim info = o.PlaneacionCapturaPreguntas(HiddenField9.Value)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartCapturaPreguntas.Series.Add(serie)
        Next
    End Sub
    Sub GraficoCapturaP()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartCapturaPersonas.Series.Clear()
        Dim info = o.PlaneacionCapturaPersonas(HiddenField10.Value)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartCapturaPersonas.Series.Add(serie)
        Next
    End Sub
#End Region

#Region "Codificacion"
    Sub GraficoCodificacionC()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartCodificacionPreguntas.Series.Clear()
        Dim info = o.PlaneacionCodificacionPreguntas(HiddenField11.Value)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartCodificacionPreguntas.Series.Add(serie)
        Next
    End Sub
    Sub GraficoCodificacionP()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartCodificacionPersonas.Series.Clear()
        Dim info = o.PlaneacionCodificacionPersonas(HiddenField12.Value)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartCodificacionPersonas.Series.Add(serie)
        Next
    End Sub
#End Region

#Region "Procesamiento"
    Sub GraficoProcesamientoC()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartProcesamientoTrabajos.Series.Clear()
        Dim info = o.PlaneacionProcesamientoTrabajos(HiddenField13.Value)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartProcesamientoTrabajos.Series.Add(serie)
        Next
    End Sub
    Sub GraficoProcesamientoP()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartProcesamientoPersonas.Series.Clear()
        Dim info = o.PlaneacionProcesamientoPersonas(HiddenField14.Value)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartProcesamientoPersonas.Series.Add(serie)
        Next
    End Sub
#End Region

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Me.gvExport.Visible = True
        'Actualiza los datos del gridview
        Me.gvExport.DataBind()
        'Crea variables en memoria para almacenar el contenido del gridview
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Crea una nueva página html y un form dentro de ella
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        'Cambie el estado del gridview para que no guarde los controles de vista
        Me.gvExport.EnableViewState = False
        'Quita la validación de la página 
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        'Agrega el form creado
        pagina.Controls.Add(form)
        'Agrega el gridview al form
        form.Controls.Add(gvExport)
        'Hace un render para pasar los valores de la página al htmltextwriter que contendrá los datos del gridview
        pagina.RenderControl(htw)
        'Hace un response para descargar el control
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=ExportadoPlaneacion_" & hfidTrabajo.Value & ".xls")
        Response.Charset = "UTF-8"

        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        gvExport.Visible = False
    End Sub

    Sub InicializarFactoresAjuste()
        Me.HiddenField1.Value = 0
        Me.HiddenField2.Value = 0
        Me.HiddenField3.Value = 0
        Me.HiddenField4.Value = 0
        Me.HiddenField5.Value = 0
        Me.HiddenField6.Value = 0
        Me.HiddenField7.Value = 0
        Me.HiddenField8.Value = 0
        Me.HiddenField9.Value = 0
        Me.HiddenField10.Value = 0
        Me.HiddenField11.Value = 0
        Me.HiddenField12.Value = 0
        Me.HiddenField13.Value = 0
        Me.HiddenField14.Value = 0
    End Sub
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If Not IsNumeric(tbFactorAjuste.Text) Then Exit Sub
        Select Case hfFactorGrafica.Value
            Case 1
                HiddenField1.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoEncuestas()
                GraficoEncuestadores()
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Case 2
                HiddenField2.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoEncuestadores()
                GraficoEncuestas()
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Case 3
                HiddenField3.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoScriptingC()
                GraficoScriptingP()
                ActivateAccordion(2, EffectActivateAccordion.NoEffect)
            Case 4
                HiddenField4.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoScriptingC()
                GraficoScriptingP()
                ActivateAccordion(2, EffectActivateAccordion.NoEffect)
            Case 5
                HiddenField5.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoCriticaC()
                GraficoCriticaP()
                ActivateAccordion(3, EffectActivateAccordion.NoEffect)
            Case 6
                HiddenField6.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoCriticaC()
                GraficoCriticaP()
                ActivateAccordion(3, EffectActivateAccordion.NoEffect)
            Case 7
                HiddenField7.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoVerificacionC()
                GraficoVerificacionP()
                ActivateAccordion(4, EffectActivateAccordion.NoEffect)
            Case 8
                HiddenField8.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoVerificacionC()
                GraficoVerificacionP()
                ActivateAccordion(4, EffectActivateAccordion.NoEffect)
            Case 9
                HiddenField9.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoCapturaC()
                GraficoCapturaP()
                ActivateAccordion(5, EffectActivateAccordion.NoEffect)
            Case 10
                HiddenField10.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoCapturaC()
                GraficoCapturaP()
                ActivateAccordion(5, EffectActivateAccordion.NoEffect)
            Case 11
                HiddenField11.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoCodificacionC()
                GraficoCodificacionP()
                ActivateAccordion(6, EffectActivateAccordion.NoEffect)
            Case 12
                HiddenField12.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoCodificacionC()
                GraficoCodificacionP()
                ActivateAccordion(6, EffectActivateAccordion.NoEffect)
            Case 13
                HiddenField13.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoProcesamientoC()
                GraficoProcesamientoP()
                ActivateAccordion(7, EffectActivateAccordion.NoEffect)
            Case 14
                HiddenField14.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoProcesamientoC()
                GraficoProcesamientoP()
                ActivateAccordion(7, EffectActivateAccordion.NoEffect)
        End Select

    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        hfFactorGrafica.Value = 1
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionEncuestasFactor)
        upanelFactor.Update()
        GraficoEncuestadores()
        GraficoEncuestas()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton2_Click(sender As Object, e As System.EventArgs) Handles LinkButton2.Click
        hfGraficaDatos.Value = 1
        hfFactorDatos.Value = HiddenField1.Value
        gvExport.DataBind()
        UPanelExport.Update()
        GraficoEncuestadores()
        GraficoEncuestas()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton3_Click(sender As Object, e As EventArgs) Handles LinkButton3.Click
        hfFactorGrafica.Value = 2
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionEncuestadoresFactor)
        upanelFactor.Update()
        GraficoEncuestadores()
        GraficoEncuestas()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton4_Click(sender As Object, e As System.EventArgs) Handles LinkButton4.Click
        hfGraficaDatos.Value = 2
        hfFactorDatos.Value = HiddenField2.Value
        gvExport.DataBind()
        UPanelExport.Update()
        GraficoEncuestadores()
        GraficoEncuestas()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton5_Click(sender As Object, e As EventArgs) Handles LinkButton5.Click
        hfFactorGrafica.Value = 3
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionScriptTrabajosFactor)
        upanelFactor.Update()
        GraficoScriptingC()
        GraficoScriptingP()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton6_Click(sender As Object, e As System.EventArgs) Handles LinkButton6.Click
        hfGraficaDatos.Value = 3
        hfFactorDatos.Value = HiddenField3.Value
        gvExport.DataBind()
        UPanelExport.Update()
        GraficoScriptingC()
        GraficoScriptingP()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton7_Click(sender As Object, e As EventArgs) Handles LinkButton7.Click
        hfFactorGrafica.Value = 4
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionScriptPersonasFactor)
        upanelFactor.Update()
        GraficoScriptingC()
        GraficoScriptingP()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton8_Click(sender As Object, e As System.EventArgs) Handles LinkButton8.Click
        hfGraficaDatos.Value = 4
        hfFactorDatos.Value = HiddenField4.Value
        gvExport.DataBind()
        UPanelExport.Update()
        GraficoScriptingC()
        GraficoScriptingP()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub
    Protected Sub LinkButton9_Click(sender As Object, e As EventArgs) Handles LinkButton9.Click
        hfFactorGrafica.Value = 5
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionCriticaEncuestasFactor)
        upanelFactor.Update()
        GraficoCriticaC()
        GraficoCriticaP()
        ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton10_Click(sender As Object, e As System.EventArgs) Handles LinkButton10.Click
        hfGraficaDatos.Value = 5
        hfFactorDatos.Value = HiddenField5.Value
        gvExport.DataBind()
        UPanelExport.Update()
        GraficoCriticaC()
        GraficoCriticaP()
        ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton11_Click(sender As Object, e As EventArgs) Handles LinkButton11.Click
        hfFactorGrafica.Value = 6
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionCriticaPersonasFactor)
        upanelFactor.Update()
        GraficoCriticaC()
        GraficoCriticaP()
        ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton12_Click(sender As Object, e As System.EventArgs) Handles LinkButton12.Click
        hfGraficaDatos.Value = 6
        hfFactorDatos.Value = HiddenField6.Value
        gvExport.DataBind()
        UPanelExport.Update()
        GraficoCriticaC()
        GraficoCriticaP()
        ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton13_Click(sender As Object, e As EventArgs) Handles LinkButton13.Click
        hfFactorGrafica.Value = 7
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionVerificacionHorasFactor)
        upanelFactor.Update()
        GraficoVerificacionC()
        GraficoVerificacionP()
        ActivateAccordion(4, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton14_Click(sender As Object, e As System.EventArgs) Handles LinkButton14.Click
        hfGraficaDatos.Value = 7
        hfFactorDatos.Value = HiddenField7.Value
        gvExport.DataBind()
        UPanelExport.Update()
        GraficoVerificacionC()
        GraficoVerificacionP()
        ActivateAccordion(4, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton15_Click(sender As Object, e As EventArgs) Handles LinkButton15.Click
        hfFactorGrafica.Value = 8
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionVerificacionPersonasFactor)
        upanelFactor.Update()
        GraficoVerificacionC()
        GraficoVerificacionP()
        ActivateAccordion(4, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton16_Click(sender As Object, e As System.EventArgs) Handles LinkButton16.Click
        hfGraficaDatos.Value = 8
        hfFactorDatos.Value = HiddenField8.Value
        gvExport.DataBind()
        UPanelExport.Update()
        GraficoVerificacionC()
        GraficoVerificacionP()
        ActivateAccordion(4, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton17_Click(sender As Object, e As EventArgs) Handles LinkButton17.Click
        hfFactorGrafica.Value = 9
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionCapturaPreguntasFactor)
        upanelFactor.Update()
        GraficoCapturaC()
        GraficoCapturaP()
        ActivateAccordion(5, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton18_Click(sender As Object, e As System.EventArgs) Handles LinkButton18.Click
        hfGraficaDatos.Value = 9
        hfFactorDatos.Value = HiddenField9.Value
        gvExport.DataBind()
        UPanelExport.Update()
        GraficoCapturaC()
        GraficoCapturaP()
        ActivateAccordion(5, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton19_Click(sender As Object, e As EventArgs) Handles LinkButton19.Click
        hfFactorGrafica.Value = 10
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionCapturaPersonasFactor)
        upanelFactor.Update()
        GraficoCapturaC()
        GraficoCapturaP()
        ActivateAccordion(5, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton20_Click(sender As Object, e As System.EventArgs) Handles LinkButton20.Click
        hfGraficaDatos.Value = 10
        hfFactorDatos.Value = HiddenField10.Value
        gvExport.DataBind()
        UPanelExport.Update()
        GraficoCapturaC()
        GraficoCapturaP()
        ActivateAccordion(5, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton21_Click(sender As Object, e As EventArgs) Handles LinkButton21.Click
        hfFactorGrafica.Value = 11
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionCodificacionPreguntasFactor)
        upanelFactor.Update()
        GraficoCodificacionC()
        GraficoCodificacionP()
        ActivateAccordion(6, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton22_Click(sender As Object, e As System.EventArgs) Handles LinkButton22.Click
        hfGraficaDatos.Value = 11
        hfFactorDatos.Value = HiddenField11.Value
        gvExport.DataBind()
        UPanelExport.Update()
        GraficoCodificacionC()
        GraficoCodificacionP()
        ActivateAccordion(6, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton23_Click(sender As Object, e As EventArgs) Handles LinkButton23.Click
        hfFactorGrafica.Value = 12
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionCodificacionPersonasFactor)
        upanelFactor.Update()
        GraficoCodificacionC()
        GraficoCodificacionP()
        ActivateAccordion(6, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton24_Click(sender As Object, e As System.EventArgs) Handles LinkButton24.Click
        hfGraficaDatos.Value = 12
        hfFactorDatos.Value = HiddenField12.Value
        gvExport.DataBind()
        UPanelExport.Update()
        GraficoCodificacionC()
        GraficoCodificacionP()
        ActivateAccordion(6, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton25_Click(sender As Object, e As EventArgs) Handles LinkButton25.Click
        hfFactorGrafica.Value = 13
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionProcesamientoTrabajosFactor)
        upanelFactor.Update()
        GraficoProcesamientoC()
        GraficoProcesamientoP()
        ActivateAccordion(7, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton26_Click(sender As Object, e As System.EventArgs) Handles LinkButton26.Click
        hfGraficaDatos.Value = 13
        hfFactorDatos.Value = HiddenField13.Value
        gvExport.DataBind()
        UPanelExport.Update()
        GraficoProcesamientoC()
        GraficoProcesamientoP()
        ActivateAccordion(7, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton27_Click(sender As Object, e As EventArgs) Handles LinkButton27.Click
        hfFactorGrafica.Value = 14
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionProcesamientoPersonasFactor)
        upanelFactor.Update()
        GraficoProcesamientoC()
        GraficoProcesamientoP()
        ActivateAccordion(7, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton28_Click(sender As Object, e As System.EventArgs) Handles LinkButton28.Click
        hfGraficaDatos.Value = 14
        hfFactorDatos.Value = HiddenField14.Value
        gvExport.DataBind()
        UPanelExport.Update()
        GraficoProcesamientoC()
        GraficoProcesamientoP()
        ActivateAccordion(7, EffectActivateAccordion.NoEffect)
    End Sub



    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        GraficoEncuestas()
        GraficoEncuestadores()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        GraficoScriptingC()
        GraficoScriptingP()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        GraficoCriticaC()
        GraficoCriticaP()
        ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        GraficoVerificacionC()
        GraficoVerificacionP()
        ActivateAccordion(4, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        GraficoCapturaC()
        GraficoCapturaP()
        ActivateAccordion(5, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        GraficoCodificacionC()
        GraficoCodificacionP()
        ActivateAccordion(6, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        GraficoProcesamientoC()
        GraficoProcesamientoP()
        ActivateAccordion(7, EffectActivateAccordion.NoEffect)
    End Sub
End Class
