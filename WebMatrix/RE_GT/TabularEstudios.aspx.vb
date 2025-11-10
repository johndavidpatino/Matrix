Imports DevExpress.Web.ASPxPivotGrid
Imports DevExpress.Data.PivotGrid
Imports DevExpress.Utils
Imports CoreProject.GestionCampo
Imports CoreProject
Imports DevExpress.XtraPivotGrid

Public Class TabularEstudios
    Inherits System.Web.UI.Page
    Dim gc As New CoreProject.GestionCampo.GC_Tabulacion

    Private Sub TabularEstudios_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Request.QueryString("ESTUDIO") IsNot Nothing Then
            CrearPivotGrid()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub CrearPivotGrid()

        Dim DSD As DataSet
        DSD = CType(Session("TABULAR_DATA"), DataSet)
        ' DSD = gc.ObtenerRespuestasTabular(CDec(Request.QueryString("ESTUDIO")))
        'Session("TABULAR_DATA") = DSD

        Dim pv As New ASPxPivotGrid()
        Panel1.Controls.Add(pv)
        pv.ID = "PivotG1"
        'pv.OptionsView.ShowHorizontalScrollBar = True

        pv.Width = Unit.Percentage(100)

        pv.DataSource = DSD.Tables(0)

        'pv.OptionsView.ShowHorizontalScrollBar = True
        If Not IsPostBack Then
            'If pv.Fields.Count = 0 Then
            pv.RetrieveFields(PivotArea.FilterArea, True)
            pv.Fields("NumEncuesta").Area = PivotArea.DataArea
            pv.Fields("NumEncuesta").AllowedAreas = PivotGridAllowedAreas.DataArea
            pv.Fields("NumEncuesta").SummaryType = PivotSummaryType.Count
            'End If

        End If

        'dim o as datarow
        'For Each o In DS.Tables(0).Rows
        '    Dim pvf As New PivotGridField()
        '    pvf.FieldName = o(1).ToString()
        '    pvf.ID = "field" & o(1).ToString()
        '    pvf.AllowedAreas = DevExpress.XtraPivotGrid.PivotGridAllowedAreas.All

        '    ' pv.Fields(o(1).ToString()).Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
        '    ASPxPivotGrid1.Fields.Add(pvf)
        '    'pv.Fields.Add(pvf)
        'Next
        'ASPxPivotGrid1.Fields("P0").Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
        'ASPxPivotGrid1.Fields("P5").Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea
        'ASPxPivotGrid1.DataSource = DSD.Tables(0)
        'ASPxPivotGrid1.DataBind()

    End Sub


    Protected Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Response.Redirect("SeleccionarPreguntasTabular.aspx?ESTUDIO=" & Request.QueryString("ESTUDIO"))
    End Sub
End Class