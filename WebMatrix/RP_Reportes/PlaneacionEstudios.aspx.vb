Imports System.IO
Imports WebMatrix.Util
Imports CoreProject
Imports System.Web.UI.DataVisualization.Charting
Public Class PlaneacionEstudios
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InicializarFactoresAjuste()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            CargarCiudades()
            CargarGerencias()
            CargarGruposUnidad()
            CargarMetodologias()
        End If
        GraficoEncuestas()
        GraficoEncuestadores()
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
        smanager.RegisterPostBackControl(Me.btnExportC)
        smanager.RegisterPostBackControl(Me.btnExportR)
    End Sub

#Region "Campo"
    Sub GraficoEncuestas()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartEncuestas.Series.Clear()
        Dim Gerencia As Int64? = Nothing
        Dim Unidad As Int64? = Nothing
        Dim Metodologia As Int32? = Nothing
        Dim Ciudad As Int32? = Nothing
        VariablesConsulta(Gerencia, Unidad, Metodologia, Ciudad)
        Dim info = o.PlaneacionEstudiosEncuestas(Gerencia, Unidad, Metodologia, Ciudad)
        For Each element In info.GroupBy(Function(x) x.PERIODO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.PERIODO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.PERIODO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.PERIODO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.PERIODO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.PERIODO = element.Key)(i).CANTIDAD)
            Next
            ChartEncuestas.Series.Add(serie)
        Next
    End Sub
    Sub GraficoEncuestadores()
        Dim o As New CoreProject.PlaneacionOPCuanti
        ChartEncuestadores.Series.Clear()
        Dim Gerencia As Int64? = Nothing
        Dim Unidad As Int64? = Nothing
        Dim Metodologia As Int32? = Nothing
        Dim Ciudad As Int32? = Nothing
        VariablesConsulta(Gerencia, Unidad, Metodologia, Ciudad)
        Dim info = o.PlaneacionEstudiosEncuestadores(Gerencia, Unidad, Metodologia, Ciudad)
        For Each element In info.GroupBy(Function(x) x.PERIODO)
            Dim serie As New Series
            serie.Name = element.Key
            serie.ChartType = SeriesChartType.Line
            For i As Integer = 0 To info.ToList.Where(Function(x) x.PERIODO = element.Key).Count - 1
                serie.Points.AddXY(info.ToList.Where(Function(x) x.PERIODO = element.Key)(i).SEMANA, info.ToList.Where(Function(x) x.PERIODO = element.Key)(i).CANTIDAD)
                If IsNumeric(info.ToList.Where(Function(x) x.PERIODO = element.Key)(i).CANTIDAD) Then serie.Points(serie.Points.Count - 1).ToolTip = Int(info.ToList.Where(Function(x) x.PERIODO = element.Key)(i).CANTIDAD)
            Next
            ChartEncuestadores.Series.Add(serie)
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
        Response.AddHeader("Content-Disposition", "attachment;filename=ExportadoProyeccion.xls")
        Response.Charset = "UTF-8"

        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        gvExport.Visible = False
    End Sub

    Protected Sub btnExportC_Click(sender As Object, e As EventArgs) Handles btnExportC.Click
        Me.gvExportCantidades.Visible = True
        'Actualiza los datos del gridview
        Me.gvExportCantidades.DataBind()
        'Crea variables en memoria para almacenar el contenido del gridview
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Crea una nueva página html y un form dentro de ella
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        'Cambie el estado del gridview para que no guarde los controles de vista
        Me.gvExportCantidades.EnableViewState = False
        'Quita la validación de la página 
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        'Agrega el form creado
        pagina.Controls.Add(form)
        'Agrega el gridview al form
        form.Controls.Add(gvExportCantidades)
        'Hace un render para pasar los valores de la página al htmltextwriter que contendrá los datos del gridview
        pagina.RenderControl(htw)
        'Hace un response para descargar el control
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=ExportadoCantidadEncuestasPropAltaProb.xls")
        Response.Charset = "UTF-8"

        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        gvExportCantidades.Visible = False
    End Sub

    Protected Sub btnExportR_Click(sender As Object, e As EventArgs) Handles btnExportR.Click
        Me.gvExportRecursos.Visible = True
        'Actualiza los datos del gridview
        Me.gvExportRecursos.DataBind()
        'Crea variables en memoria para almacenar el contenido del gridview
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Crea una nueva página html y un form dentro de ella
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        'Cambie el estado del gridview para que no guarde los controles de vista
        Me.gvExportRecursos.EnableViewState = False
        'Quita la validación de la página 
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        'Agrega el form creado
        pagina.Controls.Add(form)
        'Agrega el gridview al form
        form.Controls.Add(gvExportRecursos)
        'Hace un render para pasar los valores de la página al htmltextwriter que contendrá los datos del gridview
        pagina.RenderControl(htw)
        'Hace un response para descargar el control
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=ExportadoCantidadEncuestadoresPropAltaProb.xls")
        Response.Charset = "UTF-8"

        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        gvExportRecursos.Visible = False
    End Sub

    Sub InicializarFactoresAjuste()
        Me.HiddenField1.Value = 0
        Me.HiddenField2.Value = 0

    End Sub
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If Not IsNumeric(tbFactorAjuste.Text) Then Exit Sub
        Select Case hfFactorGrafica.Value
            Case 1
                HiddenField1.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoEncuestas()
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Case 2
                HiddenField2.Value = CDbl(tbFactorAjuste.Text) / 100
                GraficoEncuestadores()
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)

        End Select

    End Sub


    Private Sub LinkButton2_Click(sender As Object, e As System.EventArgs) Handles LinkButton2.Click
        hfGraficaDatos.Value = 1
        gvExport.DataBind()
        UPanelExport.Update()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub LinkButton4_Click(sender As Object, e As System.EventArgs) Handles LinkButton4.Click
        hfGraficaDatos.Value = 2
        gvExport.DataBind()
        UPanelExport.Update()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Sub VariablesConsulta(ByRef Gerencia As Int64?, ByRef Unidad As Int64?, ByRef Metodologia As Int32?, ByRef Ciudad As Int32?)
        If Not (ddlGerencias.SelectedValue = -1) Then Gerencia = ddlGerencias.SelectedValue
        If Not (ddlUnidades.SelectedValue = -1) Then Unidad = ddlUnidades.SelectedValue
        If Not (ddlMetodologia.SelectedValue = -1) Then Metodologia = ddlMetodologia.SelectedValue
        If Not (ddlCiudad.SelectedValue = -1) Then Ciudad = ddlCiudad.SelectedValue
    End Sub

    Sub CargarGruposUnidad()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlUnidades.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(1)
        ddlUnidades.DataValueField = "id"
        ddlUnidades.DataTextField = "GrupoUnidad"
        ddlUnidades.DataBind()
        ddlUnidades.Items.Insert(0, New ListItem With {.Text = "--Ver todas--", .Value = -1})
    End Sub

    Sub CargarGerencias()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlGerencias.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(2)
        ddlGerencias.DataValueField = "id"
        ddlGerencias.DataTextField = "GrupoUnidad"
        ddlGerencias.DataBind()
        ddlGerencias.Items.Insert(0, New ListItem With {.Text = "--Ver todas--", .Value = -1})
    End Sub

    Sub CargarMetodologias()
        Dim oMetodologias As New MetodologiaOperaciones
        ddlMetodologia.DataSource = oMetodologias.obtenerMetodologiasCuanti
        ddlMetodologia.DataValueField = "Id"
        ddlMetodologia.DataTextField = "MetNombre"
        ddlMetodologia.DataBind()
        ddlMetodologia.Items.Insert(0, New ListItem With {.Text = "--Ver todas--", .Value = -1})
    End Sub

    Sub CargarCiudades()
        Dim oCiudades As New Reportes.RP_GerOpe
        ddlCiudad.DataSource = oCiudades.ListadoDiezCiudadesPrincipales
        ddlCiudad.DataValueField = "DivMunicipio"
        ddlCiudad.DataTextField = "DivMuniNombre"
        ddlCiudad.DataBind()
        ddlCiudad.Items.Insert(0, New ListItem With {.Text = "--Ver todas--", .Value = -1})
    End Sub


    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click

    End Sub
End Class
