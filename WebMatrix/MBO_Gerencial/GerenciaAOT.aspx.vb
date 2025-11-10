Imports System.Math
Imports InfosoftGlobal
Public Class GerenciaAOT
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim I As Integer
        Dim WSigla As String
        Dim WAño As Integer
        Dim WMes As Integer
        Dim WBudgetTotal As Long
        Dim WMetaTotal As Long

        Dim WBUdgetAHoy As Long
        Dim WMetaAHoy As Long
        Dim WAOTAHoy As Long

        Dim WMetaAction As Long
        Dim WMetaWatch As Long
        Dim WMetaBooster As Long

        Dim WBudgetAction As Long
        Dim WBudgetWatch As Long
        Dim WBudgetBooster As Long

        'Variables por unidad
        Dim WMetaUAction As Long
        Dim WMetaUWatch As Long
        Dim WMetaUBooster As Long
        Dim WMetaU As Long
        Dim WActualU As Long

        'WAño = Year(Now())
        'WMes = Month(Now())

        WAño = 2012
        WMes = 5

        Session("AñoActual") = WAño

        'Para traer datos por mes y unidad
        Dim AOTTotalAdapter As New GerencialTableAdapters.MBO_PGAOTBudgetEjecucionAñoMesTableAdapter
        Dim AOTTotalDataTable As New Gerencial.MBO_PGAOTBudgetEjecucionAñoMesDataTable
        AOTTotalDataTable = AOTTotalAdapter.GetData(WAño)
        GVAOTPorUnidadMes.DataSource = AOTTotalDataTable
        GVAOTPorUnidadMes.DataBind()
        GVAOTPorUnidadMes.Visible = False

        'Calcular totales al mes actual
        For I = 0 To GVAOTPorUnidadMes.Rows.Count - 1
            If Val(GVAOTPorUnidadMes.Rows(I).Cells(0).Text) = WAño And Val(GVAOTPorUnidadMes.Rows(I).Cells(1).Text) <= WMes Then
                WBUdgetAHoy = WBUdgetAHoy + Val(GVAOTPorUnidadMes.Rows(I).Cells(3).Text)
                WMetaAHoy = WMetaAHoy + Val(GVAOTPorUnidadMes.Rows(I).Cells(4).Text)
                WAOTAHoy = WAOTAHoy + (Val(GVAOTPorUnidadMes.Rows(I).Cells(5).Text) / 1000)
            End If
            WBudgetTotal = WBudgetTotal + Val(GVAOTPorUnidadMes.Rows(I).Cells(3).Text)
            WMetaTotal = WMetaTotal + Val(GVAOTPorUnidadMes.Rows(I).Cells(4).Text)
        Next

        'META Define ubicacion ACTION-WATCH-BOOSTER
        WMetaAction = 0.9 * WMetaAHoy
        WMetaWatch = WMetaAHoy
        WMetaBooster = WMetaAHoy * 1.1

        'Mostrar AOT vs META TOTAL
        Dim xmlAOTTotal As New StringBuilder()
        xmlAOTTotal.Append("<chart caption=' AOT vs META (KCOP)' formatNumberScale='1' numberScaleValue='1000' numberScaleUnit='K'  lowerLimit='10000' upperLimit='" & WMetaBooster & "' majorTMNumber='12' minorTMNumber='5' lowerLimitDisplay='Bad' upperLimitDisplay='Excelent'  showValue='1' decimals='0' >")
        xmlAOTTotal.Append("<colorRange>")
        xmlAOTTotal.Append("<color minValue='0' maxValue='" & WMetaAction & "' code='FF0000' label='ACTION' />")
        xmlAOTTotal.Append("<color minValue='" & WMetaAction & "' maxValue='" & WMetaWatch & "' code='FFFF00' label='WATCH' />")
        xmlAOTTotal.Append("<color minValue='" & WMetaWatch & "' maxValue='" & WMetaBooster & "' code='00FF00' label='BOOSTER' />")
        xmlAOTTotal.Append("</colorRange>")
        xmlAOTTotal.Append("<pointers>")
        xmlAOTTotal.Append("<pointer value='" & WAOTAHoy & "' toolText= 'Actual' />")
        xmlAOTTotal.Append("</pointers>")
        xmlAOTTotal.Append("</chart>")
        AOTMetaTotal.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTTotal.ToString(), "chartid1", "100%", "95%", False, True)

        'BUDGET-Define ubicacion ACTION-WATCH-BOOSTER
        WBudgetAction = 0.9 * WBUdgetAHoy
        WBudgetWatch = WBUdgetAHoy
        WBudgetBooster = WBUdgetAHoy * 1.1

        'Mostrar AOT vs Budget TOTAL
        Dim xmlAOTTotalB As New StringBuilder()
        xmlAOTTotalB.Append("<chart caption='  AOT vs BUDGET (KCOP)' formatNumberScale='1' numberScaleValue='1000' numberScaleUnit='K'  lowerLimit='10000' upperLimit='" & WBudgetBooster & "' majorTMNumber='12' minorTMNumber='5' lowerLimitDisplay='Bad' upperLimitDisplay='Excelent'  showValue='1' decimals='0' >")
        xmlAOTTotalB.Append("<colorRange>")
        xmlAOTTotalB.Append("<color minValue='0' maxValue='" & WBudgetAction & "' code='FF0000' label='ACTION' />")
        xmlAOTTotalB.Append("<color minValue='" & WBudgetAction & "' maxValue='" & WBudgetWatch & "' code='FFFF00' label='WATCH' />")
        xmlAOTTotalB.Append("<color minValue='" & WBudgetWatch & "' maxValue='" & WBudgetBooster & "' code='00FF00' label='BOOSTER' />")
        xmlAOTTotalB.Append("</colorRange>")
        xmlAOTTotalB.Append("<pointers>")
        xmlAOTTotalB.Append("<pointer value='" & WAOTAHoy & "' toolText= 'Actual' />")
        xmlAOTTotalB.Append("</pointers>")
        xmlAOTTotalB.Append("</chart>")
        AOTBudgetTotal.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTTotalB.ToString(), "chartid2", "100%", "95%", False, True)

        'TRAER DATOS AOT POR UNIDAD
        'Para traer datos por mes y unidad
        Dim AOTTotalUnidadAdapter As New GerencialTableAdapters.MBO_PGAOTBudgetEjecucionUnidadTableAdapter
        Dim AOTTotalUnidadDataTable As New Gerencial.MBO_PGAOTBudgetEjecucionUnidadDataTable
        AOTTotalUnidadDataTable = AOTTotalUnidadAdapter.GetData(WAño, WMes)
        GVAOTPorUnidad.DataSource = AOTTotalUnidadDataTable
        GVAOTPorUnidad.DataBind()
        GVAOTPorUnidad.Visible = False

        'Formatear
        For I = 0 To GVAOTPorUnidad.Rows.Count - 1
            GVAOTPorUnidad.Rows(I).Cells(1).Text = Val(GVAOTPorUnidad.Rows(I).Cells(1).Text) * 1000
            GVAOTPorUnidad.Rows(I).Cells(2).Text = Val(GVAOTPorUnidad.Rows(I).Cells(2).Text) * 1000
            GVAOTPorUnidad.Rows(I).Cells(3).Text = Val(GVAOTPorUnidad.Rows(I).Cells(3).Text)
        Next

        Dim xmlAOTTotalU As New StringBuilder()
        For I = 0 To GVAOTPorUnidad.Rows.Count - 1
            'AOT
            WSigla = GVAOTPorUnidad.Rows(I).Cells(0).Text
            WMetaU = Val(GVAOTPorUnidad.Rows(I).Cells(2).Text)
            WActualU = Val(GVAOTPorUnidad.Rows(I).Cells(3).Text)
            WMetaUAction = 0.9 * WMetaU
            WMetaUWatch = WMetaUAction
            WMetaUBooster = WMetaU
            If WActualU > WMetaUBooster Then
                xmlAOTTotalU.Append("<chart caption='" & GVAOTPorUnidad.Rows(I).Cells(0).Text & "' showToolTip='1' showvalues='1' numdivlines='10' xAxisName='' yAxisName='Valor' decimals='0' paletteColors='0000FF,669900,00FF00' >")
            Else
                If WActualU > WMetaUWatch Then
                    xmlAOTTotalU.Append("<chart caption='" & GVAOTPorUnidad.Rows(I).Cells(0).Text & "' showToolTip='1' showvalues='1' numdivlines='10' xAxisName='' yAxisName='Valor' decimals='0' paletteColors='0000FF,669900,FFFF10' >")
                Else
                    xmlAOTTotalU.Append("<chart caption='" & GVAOTPorUnidad.Rows(I).Cells(0).Text & "' showToolTip='1' showvalues='1' numdivlines='10' xAxisName='' yAxisName='Valor' decimals='0' paletteColors='0000FF,669900,FF0000' >")
                End If
            End If
            xmlAOTTotalU.Append("<set Label='Budget' value='" & Val(GVAOTPorUnidad.Rows(I).Cells(1).Text) & "' />")
            xmlAOTTotalU.Append("<set Label='Meta' value='" & Val(GVAOTPorUnidad.Rows(I).Cells(2).Text) & "' />")
            xmlAOTTotalU.Append("<set Label='Actual' value='" & Val(GVAOTPorUnidad.Rows(I).Cells(3).Text) & "' />")
            xmlAOTTotalU.Append("</chart>")
            Select Case WSigla
                Case "ASI" : AOTASI.Text = FusionCharts.RenderChart("../FusionCharts/Column3D.swf", "", xmlAOTTotalU.ToString(), "chartida", "300px", "300px", False, True)
                Case "LO " : AOTLO.Text = FusionCharts.RenderChart("../FusionCharts/Column3D.swf", "", xmlAOTTotalU.ToString(), "chartidb", "300px", "300px", False, True)
                Case "MD " : AOTMD.Text = FusionCharts.RenderChart("../FusionCharts/Column3D.swf", "", xmlAOTTotalU.ToString(), "chartidc", "300px", "300px", False, True)
                Case "MK " : AOTMK.Text = FusionCharts.RenderChart("../FusionCharts/Column3D.swf", "", xmlAOTTotalU.ToString(), "chartidd", "300px", "300px", False, True)
                Case "PA " : AOTPA.Text = FusionCharts.RenderChart("../FusionCharts/Column3D.swf", "", xmlAOTTotalU.ToString(), "chartide", "300px", "300px", False, True)
                Case "PG " : AOTPG.Text = FusionCharts.RenderChart("../FusionCharts/Column3D.swf", "", xmlAOTTotalU.ToString(), "chartidf", "300px", "300px", False, True)
            End Select
            xmlAOTTotalU.Clear()
        Next

        'ACQUISITION AND ACHIEVEMENT
        Dim WAcqAction As Integer
        Dim WAcqWatch As Integer
        Dim WAcqBooster As Integer
        Dim WAcqMeta As Double
        Dim WAcqActual As Double
        Dim xmlAOTACQ As New StringBuilder()
        Dim AOTAcquisitionAdapter As New GerencialTableAdapters.MBO_AOTAcquisitionTableAdapter
        Dim AOTAcquisitionDataTable As New Gerencial.MBO_AOTAcquisitionDataTable
        AOTAcquisitionDataTable = AOTAcquisitionAdapter.GetData(WAño, WMes)
        GVAOTAcquisition.DataSource = AOTAcquisitionDataTable
        GVAOTAcquisition.DataBind()
        GVAOTAcquisition.Visible = False

        For I = 0 To GVAOTAcquisition.Rows.Count - 1
            WSigla = GVAOTAcquisition.Rows(I).Cells(1).Text

            'ACQUISITION
            'Actual = ActualYTD / MetaFY 
            WAcqActual = Val(GVAOTAcquisition.Rows(I).Cells(4).Text) / Val(GVAOTAcquisition.Rows(I).Cells(3).Text) * 100
            'Meta = MetaYTD / MetaFY
            WAcqMeta = Val(GVAOTAcquisition.Rows(I).Cells(5).Text) / Val(GVAOTAcquisition.Rows(I).Cells(3).Text) * 100
            WAcqAction = 0.9 * WAcqMeta
            WAcqWatch = WAcqMeta
            WAcqBooster = WAcqMeta * 1.1

            xmlAOTACQ.Append("<chart caption='% Acquisition-" & WSigla & "' lowerLimit='0' upperLimit='" & WAcqBooster & "' majorTMNumber='5' minorTMNumber='1' lowerLimitDisplay='Bad' upperLimitDisplay='Excelent'  showValue='1' decimals='1'  tickValueDecimals='1' forceTickValueDecimals='1' numberSuffix='%' >")
            xmlAOTACQ.Append("<colorRange>")
            xmlAOTACQ.Append("<color minValue='0' maxValue='" & WAcqAction & "' code='FF0000' label='ACTION' />")
            xmlAOTACQ.Append("<color minValue='" & WAcqAction & "' maxValue='" & WAcqWatch & "' code='FFFF00' label='WATCH' />")
            xmlAOTACQ.Append("<color minValue='" & WAcqWatch & "' maxValue='" & WAcqBooster & "' code='00FF00' label='BOOSTER' />")
            xmlAOTACQ.Append("</colorRange>")
            xmlAOTACQ.Append("<pointers>")
            xmlAOTACQ.Append("<pointer value='" & WAcqActual & "' toolText= 'ActualYTD/MetaFY' />")
            xmlAOTACQ.Append("<pointer value='" & WAcqMeta & "' bgColor='CCCCCC' borderColor='333333' toolText='MetaYTD/MetaFY'/>")
            xmlAOTACQ.Append("</pointers>")
            xmlAOTACQ.Append("</chart>")
            Select Case WSigla
                Case "ASI" : AcquisitionASI.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTACQ.ToString(), "chartid10", "300", "150", False, True)
                Case "LO " : AcquisitionLO.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTACQ.ToString(), "chartid11", "300", "150", False, True)
                Case "MD " : AcquisitionMD.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTACQ.ToString(), "chartid12", "300", "150", False, True)
                Case "MK " : AcquisitionMK.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTACQ.ToString(), "chartid13", "300", "150", False, True)
                Case "PA " : AcquisitionPA.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTACQ.ToString(), "chartid14", "300", "150", False, True)
                Case "PG " : AcquisitionPG.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTACQ.ToString(), "chartid15", "300", "150", False, True)
            End Select
            xmlAOTACQ.Clear()

            'ACHIEVEMENT---Se usan las mismas variables
            'Actual = ActualYTD / MetaYTD 
            WAcqActual = Val(GVAOTAcquisition.Rows(I).Cells(4).Text) / Val(GVAOTAcquisition.Rows(I).Cells(5).Text) * 100
            'Meta = MetaYTD / MetaYTD
            WAcqMeta = Val(GVAOTAcquisition.Rows(I).Cells(5).Text) / Val(GVAOTAcquisition.Rows(I).Cells(5).Text) * 100
            WAcqAction = 0.9 * WAcqMeta
            WAcqWatch = WAcqMeta
            WAcqBooster = WAcqMeta * 1.1

            xmlAOTACQ.Append("<chart caption='% Achievement-" & WSigla & "' lowerLimit='0' upperLimit='" & WAcqBooster & "' majorTMNumber='5' minorTMNumber='1' lowerLimitDisplay='Bad' upperLimitDisplay='Excelent'  showValue='1' decimals='1'  tickValueDecimals='1' forceTickValueDecimals='1' numberSuffix='%' >")
            xmlAOTACQ.Append("<colorRange>")
            xmlAOTACQ.Append("<color minValue='0' maxValue='" & WAcqAction & "' code='FF0000' label='ACTION' />")
            xmlAOTACQ.Append("<color minValue='" & WAcqAction & "' maxValue='" & WAcqWatch & "' code='FFFF00' label='WATCH' />")
            xmlAOTACQ.Append("<color minValue='" & WAcqWatch & "' maxValue='" & WAcqBooster & "' code='00FF00' label='BOOSTER' />")
            xmlAOTACQ.Append("</colorRange>")
            xmlAOTACQ.Append("<pointers>")
            xmlAOTACQ.Append("<pointer value='" & WAcqActual & "' toolText= 'ActualYTD/MetaYTD' />")
            xmlAOTACQ.Append("<pointer value='" & WAcqMeta & "' bgColor='CCCCCC' borderColor='333333' toolText='MetaYTD/MetaYTD'/>")
            xmlAOTACQ.Append("</pointers>")
            xmlAOTACQ.Append("</chart>")
            Select Case WSigla
                Case "ASI" : AchievementASI.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTACQ.ToString(), "chartid20", "300", "150", False, True)
                Case "LO " : AchievementLO.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTACQ.ToString(), "chartid21", "300", "150", False, True)
                Case "MD " : AchievementMD.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTACQ.ToString(), "chartid22", "300", "150", False, True)
                Case "MK " : AchievementMK.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTACQ.ToString(), "chartid23", "300", "150", False, True)
                Case "PA " : AchievementPA.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTACQ.ToString(), "chartid24", "300", "150", False, True)
                Case "PG " : AchievementPG.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTACQ.ToString(), "chartid25", "300", "150", False, True)
            End Select
            xmlAOTACQ.Clear()
        Next

    End Sub

End Class