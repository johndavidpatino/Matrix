Imports System.IO
Imports WebMatrix.Util
Imports CoreProject

Public Class AvanceDeCampo
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("TrabajoId") IsNot Nothing Then
                Dim o As New OP_Cuanti
                hfidTrabajo.Value = Request.QueryString("TrabajoId")
                If o.OP_EstimacionesProduccionCiudad.Where(Function(x) x.TrabajoId = hfidTrabajo.Value).Count = 0 Then
                    Me.lblNoData.Visible = True
                    Exit Sub
                Else
                    Me.lblNoData.Visible = False
                End If
                AvanceGeneral(hfidTrabajo.Value)
                AvanceXCiudad(hfidTrabajo.Value)
                AvanceAreas(hfidTrabajo.Value)
                AvanceRemanentes(hfidTrabajo.Value)
                CargarEncuestasAnuladas()
                CargarMatriz(hfidTrabajo.Value)
                Me.gvEncuestadores.DataBind()
            End If
        End If
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExportar)
    End Sub
    Protected Sub ddVariables_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddVariables.SelectedIndexChanged
        Me.gvAvanceCuotas.DataBind()
        ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub ddColumna_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddColumna.SelectedIndexChanged
        If Me.ddFila.SelectedValue = 0 Or Me.ddColumna.SelectedValue = 0 Then
            ActivateAccordion(3, EffectActivateAccordion.SlideEffect)
            Exit Sub
        Else
            Me.gvAvanceCuotasCruce.DataBind()
        End If
        ActivateAccordion(3, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub ddFila_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddFila.SelectedIndexChanged
        If Me.ddFila.SelectedValue = 0 Or Me.ddColumna.SelectedValue = 0 Then
            ActivateAccordion(3, EffectActivateAccordion.SlideEffect)
            Exit Sub
        Else
            Me.gvAvanceCuotasCruce.DataBind()
        End If
        ActivateAccordion(3, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub btnExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Me.gvExportado.Visible = True
        'Actualiza los datos del gridview
        Me.gvExportado.DataBind()
        'Crea variables en memoria para almacenar el contenido del gridview
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Crea una nueva página html y un form dentro de ella
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        'Cambie el estado del gridview para que no guarde los controles de vista
        Me.gvExportado.EnableViewState = False
        'Quita la validación de la página 
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        'Agrega el form creado
        pagina.Controls.Add(form)
        'Agrega el gridview al form
        form.Controls.Add(gvExportado)
        'Hace un render para pasar los valores de la página al htmltextwriter que contendrá los datos del gridview
        pagina.RenderControl(htw)
        'Hace un response para descargar el control
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=ExportadoRMC_Trabajo_" & hfidTrabajo.Value & ".xls")
        Response.Charset = "UTF-8"

        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        gvExportado.Visible = False

    End Sub

#End Region

#Region "Metodos"
    Sub AvanceGeneral(ByVal id As Int64)
        Dim o As New CoreProject.Reportes.AvanceCampo
        Dim info = o.ObtenerAvanceGeneralxId(hfidTrabajo.Value)
        If info.fecha Is Nothing Then Exit Sub
        Me.ChartEstado.Series.Clear()
        Dim yValues() As Double = {info.fecha, info.ejecucion}
        Dim xValues() As String = {"En Fecha", "En Muestra"}
        Me.ChartEstado.Series.Add("Fecha")
        Me.ChartEstado.Series("Fecha").Points.DataBindXY(xValues, yValues)
        Me.ChartEstado.Series("Fecha").IsValueShownAsLabel = True
        Me.ChartEstado.Series("Fecha").LabelFormat = "0.00%"

        Me.ChartEstado.Series("Fecha").BackGradientStyle = DataVisualization.Charting.GradientStyle.VerticalCenter
        Me.ChartEstado.Series("Fecha").Color = Drawing.Color.MediumSlateBlue
        Select Case info.variacion
            Case Is < 0
                Me.lblVariacion.Text = "Se presenta variación de - " & FormatNumber(info.variacion, 2) & "% respecto a la estimación de producción"
            Case Is = 0
                Me.lblVariacion.Text = "No se presentan variaciones respecto a la estimación de producción"
            Case Is > 0
                Me.lblVariacion.Text = "Se presenta variación de + " & FormatNumber(info.variacion, 2) & "% respecto a la estimación de producción"
        End Select
    End Sub

    Sub AvanceXCiudad(ByVal id As Int64)
        Dim o As New CoreProject.Reportes.AvanceCampo
        Try
            Me.gvAvanceCiudad.DataSource = o.ObtenerAvanceXCiudadXId(id).ToList
            Me.gvAvanceCiudad.DataBind()
        Catch ex As Exception

        End Try

    End Sub

    Sub AvanceAreas(ByVal id As Int64)
        Dim o As New CoreProject.Reportes.AvanceCampo
        Me.gvAvanceAreas.DataSource = o.ObtenerAvancePorcentualAreasXId(id).ToList
        Me.gvAvanceAreas.DataBind()
    End Sub

    Sub AvanceRemanentes(ByVal id As Int64)
        Dim o As New CoreProject.Reportes.AvanceCampo
        Me.gvRemanentes.DataSource = o.ObtenerAvanceAreasRemanentesXid(id).ToList
        Me.gvRemanentes.DataBind()
    End Sub
#End Region

#Region "Matriz"
    Sub CargarMatriz(ByVal TrabajoId As Int64)
        Dim o As New CoreProject.Reportes.AvanceCampo
        Dim info = o.ObtenerAvanceGeneralxId(TrabajoId)
        If info.fecha Is Nothing Then Exit Sub
        Me.gvMatriz.DataSource = o.ObtenerMatrizCumplimientoXid(TrabajoId)
        Me.gvMatriz.DataBind()
        FormatoGridview()
    End Sub

    Sub FormatoGridview()
        For Each e As GridViewRow In gvMatriz.Rows
            If e.RowType = DataControlRowType.DataRow Then
                e.Cells(1).BackColor = Drawing.Color.LightGray
                e.Cells(2).BackColor = Drawing.Color.LightGray
                e.Cells(3).BackColor = Drawing.Color.LightGray
                e.Cells(4).BackColor = Drawing.Color.LightGray
                e.Cells(5).BackColor = Drawing.Color.LightGray

                e.Cells(11).BackColor = Drawing.Color.LightGray
                e.Cells(12).BackColor = Drawing.Color.LightGray
                e.Cells(13).BackColor = Drawing.Color.LightGray
                e.Cells(14).BackColor = Drawing.Color.LightGray
                e.Cells(15).BackColor = Drawing.Color.LightGray

                e.Cells(21).BackColor = Drawing.Color.LightGray
                e.Cells(22).BackColor = Drawing.Color.LightGray
                e.Cells(23).BackColor = Drawing.Color.LightGray
                e.Cells(24).BackColor = Drawing.Color.LightGray
                e.Cells(25).BackColor = Drawing.Color.LightGray
            End If
        Next
    End Sub

    Private Sub gvMatriz_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMatriz.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim gvr As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)
            Dim thc As New TableHeaderCell()
            thc.RowSpan = 2
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 5
            'thc.CssClass = "RecapturePerformance"
            thc.Text = "CAMPO"
            gvr.Cells.Add(thc)


            thc = New TableHeaderCell()
            thc.ColumnSpan = 5
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "RMC"
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 5
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "CRÍTICA"
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 5
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "VERIFICACIÓN"
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 5
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "CAPTURA"
            gvr.Cells.Add(thc)

            Me.gvMatriz.Controls(0).Controls.AddAt(0, gvr)


            ' SEGUNDA FILA DE DATOS
            gvr = New GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Normal)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "RecapturePerformance"
            thc.Text = "Planeado"
            gvr.Cells.Add(thc)


            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "Ejecutado"
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 1
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = ""
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "RecapturePerformance"
            thc.Text = "Planeado"
            gvr.Cells.Add(thc)


            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "Ejecutado"
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 1
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = ""
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "RecapturePerformance"
            thc.Text = "Planeado"
            gvr.Cells.Add(thc)


            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "Ejecutado"
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 1
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = ""
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "RecapturePerformance"
            thc.Text = "Planeado"
            gvr.Cells.Add(thc)


            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "Ejecutado"
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 1
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = ""
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "RecapturePerformance"
            thc.Text = "Planeado"
            gvr.Cells.Add(thc)


            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "Ejecutado"
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 1
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = ""
            gvr.Cells.Add(thc)

            Me.gvMatriz.Controls(0).Controls.AddAt(1, gvr)

            'formato

            'If e.Row.RowType = DataControlRowType.DataRow Then
            '    e.Row.Cells(1).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(2).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(3).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(4).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(5).BackColor = Drawing.Color.LightBlue

            '    e.Row.Cells(11).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(12).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(13).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(14).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(15).BackColor = Drawing.Color.LightBlue

            '    e.Row.Cells(21).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(22).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(23).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(24).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(25).BackColor = Drawing.Color.LightBlue
            'End If
        End If
    End Sub

    Private Sub gvMatriz_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMatriz.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 5 To 25 Step 5
                If IsNumeric(e.Row.Cells(i).Text) Then
                    If CDbl(e.Row.Cells(i).Text) > 0 Then
                        e.Row.Cells(i).BackColor = Drawing.Color.LightYellow
                        e.Row.Cells(i).BorderColor = Drawing.Color.Red
                        e.Row.Cells(i).Font.Bold = True
                        e.Row.Cells(i).ForeColor = Drawing.Color.Red
                    End If
                End If
            Next
        End If

    End Sub

#End Region
    Sub CargarTraficoAreas()
        Dim oTrafico As New CoreProject.Reportes.AvanceCampo
        Me.gvTraficoAreas.DataSource = oTrafico.ObtenerTraficoAreas(hfidTrabajo.Value, ddlAreasTrafico.SelectedValue)
        Me.gvTraficoAreas.DataBind()
    End Sub
    Sub CargarEncuestasAnuladas()
        Dim oAnuladas As New CoreProject.Reportes.AvanceCampo
        Me.gvAnuladas.DataSource = oAnuladas.ObtenerEncuestasAnuladas(hfidTrabajo.Value)
        Me.gvAnuladas.DataBind()
    End Sub
    Private Sub ddlAreasTrafico_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAreasTrafico.SelectedIndexChanged
        If ddlAreasTrafico.SelectedValue = 0 Then Exit Sub
        ActivateAccordion(8, EffectActivateAccordion.NoEffect)
        CargarTraficoAreas()
    End Sub

    Private Sub gvEncuestadores_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEncuestadores.RowCommand
        If e.CommandName = "Actualizar" Then
            Dim id As Int64 = gvEncuestadores.DataKeys(e.CommandArgument)("Per_NumIdentificacionEncu")
            Response.Redirect("../TH_TalentoHumano/FichaEncuestador.aspx?Identificacion=" & id)
        End If
    End Sub
End Class