Imports System.IO
Imports WebMatrix.Util
Imports System.Web.UI.DataVisualization.Charting
Public Class PlaneacionCampo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarGerencias()
            CargarMetodologias()
            InicializarFactoresAjuste()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
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
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExport)
    End Sub

#Region "GerenciaOperaciones"
    Sub GraficoEncuestasGerencia()
        If ddlGerencias.SelectedValue = -1 Or ddlGerencias.SelectedIndex = -1 Then Exit Sub

        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartEncuestasGerencia.Series.Clear()
        Dim info = o.PlaneacionEncuestasXGerencia(HiddenField1.Value, ddlGerencias.SelectedValue)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartEncuestasGerencia.Series.Add(serie)
        Next
    End Sub
    Sub GraficoEncuestadoresGerencia()
        If ddlGerencias.SelectedValue = -1 Or ddlGerencias.SelectedIndex = -1 Then Exit Sub

        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartEncuestadoresGerencia.Series.Clear()
        Dim info = o.PlaneacionEncuestadoresXGerencia(HiddenField2.Value, ddlGerencias.SelectedValue)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartEncuestadoresGerencia.Series.Add(serie)
        Next
    End Sub
#End Region

#Region "Ciudades"
    Sub GraficoCiudadesEncuestas()
        If ddlCiudades.SelectedValue = -1 Or ddlCiudades.SelectedIndex = -1 Then Exit Sub

        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartCiudadEncuestas.Series.Clear()
        Dim info = o.PlaneacionEncuestasXCiudad(HiddenField3.Value, ddlCiudades.SelectedValue)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartCiudadEncuestas.Series.Add(serie)
        Next
    End Sub
    Sub GraficoCiudadesEncuestadores()
        If ddlCiudades.SelectedValue = -1 Or ddlCiudades.SelectedIndex = -1 Then Exit Sub

        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartCiudadEncuestadores.Series.Clear()
        Dim info = o.PlaneacionEncuestadoresXCiudad(HiddenField4.Value, ddlCiudades.SelectedValue)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartCiudadEncuestadores.Series.Add(serie)
        Next
    End Sub
#End Region

#Region "Metodologías"
    Sub GraficoMetodologiaEncuestas()
        If ddlMetodologias.SelectedValue = -1 Or Me.ddlMetodologias.SelectedIndex = -1 Then Exit Sub

        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartMetodologiaEncuestas.Series.Clear()
        Dim info = o.PlaneacionEncuestasXMetodologia(HiddenField5.Value, ddlMetodologias.SelectedValue)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartMetodologiaEncuestas.Series.Add(serie)
        Next
    End Sub
    Sub GraficoMetodologiaEncuestadores()
        If ddlMetodologias.SelectedValue = -1 Or Me.ddlMetodologias.SelectedIndex = -1 Then Exit Sub

        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartMetodologiaEncuestadores.Series.Clear()
        Dim info = o.PlaneacionEncuestadoresXMetodologia(HiddenField6.Value, ddlMetodologias.SelectedValue)
        For Each element In info.GroupBy(Function(x) x.AÑO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.AÑO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.AÑO = element.Key)(i).CANTIDAD)
            Next
            ChartMetodologiaEncuestadores.Series.Add(serie)
        Next
    End Sub
#End Region

#Region "Unidades"
    Sub GraficoUnidadesEncuestas()
        If Me.ddlUnidades.SelectedValue = -1 Or Me.ddlUnidades.SelectedIndex = -1 Then Exit Sub

        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartVerificacionHoras.Series.Clear()
        Dim info = o.PlaneacionEncuestasXUnidad(HiddenField7.Value, ddlUnidades.SelectedValue)
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
    Sub GraficoUnidadesEncuestadores()
        If Me.ddlUnidades.SelectedValue = -1 Or Me.ddlUnidades.SelectedIndex = -1 Then Exit Sub

        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartVerificacionPersonas.Series.Clear()
        Dim info = o.PlaneacionEncuestadoresXUnidad(HiddenField8.Value, ddlUnidades.SelectedValue)
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
    End Sub
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If Not IsNumeric(tbFactorAjuste.Text) Then Exit Sub
        Select Case hfFactorGrafica.Value
            Case 1
                HiddenField1.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoEncuestasGerencia()
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Case 2
                HiddenField2.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoEncuestadoresGerencia()
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Case 3
                HiddenField3.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoCiudadesEncuestas()
                ActivateAccordion(2, EffectActivateAccordion.NoEffect)
            Case 4
                HiddenField4.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoCiudadesEncuestadores()
                ActivateAccordion(2, EffectActivateAccordion.NoEffect)
            Case 5
                HiddenField5.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoMetodologiaEncuestas()
                ActivateAccordion(3, EffectActivateAccordion.NoEffect)
            Case 6
                HiddenField6.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoMetodologiaEncuestadores()
                ActivateAccordion(3, EffectActivateAccordion.NoEffect)
            Case 7
                HiddenField7.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoUnidadesEncuestas()
                ActivateAccordion(4, EffectActivateAccordion.NoEffect)
            Case 8
                HiddenField8.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoUnidadesEncuestadores()
                ActivateAccordion(4, EffectActivateAccordion.NoEffect)

        End Select

    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        hfFactorGrafica.Value = 1
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionEncuestasXGerenciaFactor(ddlGerencias.SelectedValue))
        upanelFactor.Update()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton2_Click(sender As Object, e As System.EventArgs) Handles LinkButton2.Click
        hfGraficaDatos.Value = 1
        hfFactorDatos.Value = HiddenField1.Value
        hfFactorDatos.Value = ddlGerencias.SelectedValue
        gvExport.DataBind()
        UPanelExport.Update()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton3_Click(sender As Object, e As EventArgs) Handles LinkButton3.Click
        hfFactorGrafica.Value = 2
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionEncuestadoresXGerenciaFactor(ddlGerencias.SelectedValue))
        upanelFactor.Update()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton4_Click(sender As Object, e As System.EventArgs) Handles LinkButton4.Click
        hfGraficaDatos.Value = 2
        hfFactorDatos.Value = HiddenField2.Value
        hfFiltroDatos.Value = ddlGerencias.SelectedValue
        gvExport.DataBind()
        UPanelExport.Update()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton5_Click(sender As Object, e As EventArgs) Handles LinkButton5.Click
        hfFactorGrafica.Value = 3
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionEncuestasXCiudadFactor(ddlCiudades.SelectedValue))
        upanelFactor.Update()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton6_Click(sender As Object, e As System.EventArgs) Handles LinkButton6.Click
        hfGraficaDatos.Value = 3
        hfFactorDatos.Value = HiddenField3.Value
        hfFiltroDatos.Value = ddlCiudades.SelectedValue
        gvExport.DataBind()
        UPanelExport.Update()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton7_Click(sender As Object, e As EventArgs) Handles LinkButton7.Click
        hfFactorGrafica.Value = 4
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionEncuestadoresXCiudadFactor(ddlCiudades.SelectedValue))
        upanelFactor.Update()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton8_Click(sender As Object, e As System.EventArgs) Handles LinkButton8.Click
        hfGraficaDatos.Value = 4
        hfFactorDatos.Value = HiddenField4.Value
        hfFiltroDatos.Value = ddlCiudades.SelectedValue
        gvExport.DataBind()
        UPanelExport.Update()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub
    Protected Sub LinkButton9_Click(sender As Object, e As EventArgs) Handles LinkButton9.Click
        hfFactorGrafica.Value = 5
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionEncuestasXMetodologiaFactor(ddlMetodologias.SelectedValue))
        upanelFactor.Update()
        ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton10_Click(sender As Object, e As System.EventArgs) Handles LinkButton10.Click
        hfGraficaDatos.Value = 5
        hfFactorDatos.Value = HiddenField5.Value
        hfFiltroDatos.Value = ddlMetodologias.SelectedValue
        gvExport.DataBind()
        UPanelExport.Update()
        ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton11_Click(sender As Object, e As EventArgs) Handles LinkButton11.Click
        hfFactorGrafica.Value = 6
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionEncuestadoresXMetodologiaFactor(ddlMetodologias.SelectedValue))
        upanelFactor.Update()
        ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton12_Click(sender As Object, e As System.EventArgs) Handles LinkButton12.Click
        hfGraficaDatos.Value = 6
        hfFactorDatos.Value = HiddenField6.Value
        hfFiltroDatos.Value = ddlMetodologias.SelectedValue
        gvExport.DataBind()
        UPanelExport.Update()
        ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton13_Click(sender As Object, e As EventArgs) Handles LinkButton13.Click
        hfFactorGrafica.Value = 7
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionEncuestasXUnidadFactor(ddlUnidades.SelectedValue))
        upanelFactor.Update()
        ActivateAccordion(4, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton14_Click(sender As Object, e As System.EventArgs) Handles LinkButton14.Click
        hfGraficaDatos.Value = 7
        hfFactorDatos.Value = HiddenField7.Value
        hfFiltroDatos.Value = ddlUnidades.SelectedValue
        gvExport.DataBind()
        UPanelExport.Update()
        ActivateAccordion(4, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub LinkButton15_Click(sender As Object, e As EventArgs) Handles LinkButton15.Click
        hfFactorGrafica.Value = 8
        Dim o As New CoreProject.PlaneacionOPCuanti
        lblFactorVariacion.Text = FormatPercent(o.PlaneacionEncuestadoresXUnidadFactor(ddlUnidades.SelectedValue))
        upanelFactor.Update()
        ActivateAccordion(4, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton16_Click(sender As Object, e As System.EventArgs) Handles LinkButton16.Click
        hfGraficaDatos.Value = 8
        hfFactorDatos.Value = HiddenField8.Value
        hfFiltroDatos.Value = ddlUnidades.SelectedValue
        gvExport.DataBind()
        UPanelExport.Update()
        ActivateAccordion(4, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        GraficoEncuestasGerencia()
        GraficoEncuestadoresGerencia()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        GraficoCiudadesEncuestas()
        GraficoCiudadesEncuestadores()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        GraficoMetodologiaEncuestas()
        GraficoMetodologiaEncuestadores()
        ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        GraficoUnidadesEncuestas()
        GraficoUnidadesEncuestadores()
        ActivateAccordion(4, EffectActivateAccordion.NoEffect)
    End Sub

    Sub CargarGerencias()
        Dim oGruposUnidad As New CoreProject.US.GrupoUnidad
        ddlGerencias.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(2)
        ddlGerencias.DataValueField = "id"
        ddlGerencias.DataTextField = "GrupoUnidad"
        ddlGerencias.DataBind()
        ddlGerencias.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub

    Sub CargarMetodologias()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ddlMetodologias.DataSource = o.ObtenerMetodologias
        ddlMetodologias.DataValueField = "id"
        ddlMetodologias.DataTextField = "MetNombre"
        ddlMetodologias.DataBind()
        ddlMetodologias.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub


    Protected Sub ddlGerencias_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGerencias.SelectedIndexChanged
        GraficoEncuestadoresGerencia()
        GraficoEncuestasGerencia()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlCiudades_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCiudades.SelectedIndexChanged
        GraficoCiudadesEncuestadores()
        GraficoCiudadesEncuestas()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlMetodologias_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMetodologias.SelectedIndexChanged
        GraficoMetodologiaEncuestadores()
        GraficoMetodologiaEncuestas()
        ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlUnidades_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUnidades.SelectedIndexChanged
        GraficoUnidadesEncuestadores()
        GraficoUnidadesEncuestas()
        ActivateAccordion(4, EffectActivateAccordion.NoEffect)
    End Sub
End Class
