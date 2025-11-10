Imports System.Math
Imports InfosoftGlobal
Imports CoreProject
Public Class AOTDireccion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        Dim WUnidad As Integer
        Dim WNUnidad As String
        Dim WNUsuario As String
        Dim WParametroSigla As String = ""

        Dim UnidadAdapter As New GerencialTableAdapters.MBO_ObtenerUnidadesUsuarioTableAdapter
        Dim UnidadDataTable As New Gerencial.MBO_ObtenerUnidadesUsuarioDataTable
        Dim UnidadRow As Gerencial.MBO_ObtenerUnidadesUsuarioRow
        UnidadDataTable = UnidadAdapter.GetData(UsuarioID)

        For Each UnidadRow In UnidadDataTable.Rows
            WUnidad = CInt(UnidadRow.IdUnidad)
            WNUnidad = UnidadRow.Unidad
            WNUsuario = UnidadRow.Usuario
            WParametroSigla = Trim(UnidadRow.Sigla)
        Next

        Dim I As Integer
        Dim WSigla As String
        Dim WAño As Integer
        Dim WMesActual As Integer
        Dim WMesAnterior As Integer
        Dim WBudgetTotal As Long
        Dim WMetaTotal As Long

        Dim WBudgetAHoy As Long
        Dim WMetaAHoy As Long
        Dim WAOTAHoy As Long
        Dim WLowerLimiteTotal As Long
        Dim WUpperLimiteTotal As Long

        Dim WBudgetMes As Long
        Dim WMetaMes As Long
        Dim WAOTMes As Long

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

        Dim WNMeses(12) As String
        WNMeses(1) = "ENERO"
        WNMeses(2) = "FEBRERO"
        WNMeses(3) = "MARZO"
        WNMeses(4) = "ABRIL"
        WNMeses(5) = "MAYO"
        WNMeses(6) = "JUNIO"
        WNMeses(7) = "JULIO"
        WNMeses(8) = "AGOSTO"
        WNMeses(9) = "SEPTIEMBRE"
        WNMeses(10) = "OCTUBRE"
        WNMeses(11) = "NOVIEMBRE"
        WNMeses(12) = "DICIEMBRE"
        'Control para escoger mes
        Meses.Items.Add(New ListItem("Seleccione mes", 0))
        Meses.Items.Add(New ListItem("ENERO", 1))
        Meses.Items.Add(New ListItem("FEBRERO", 2))
        Meses.Items.Add(New ListItem("MARZO", 3))
        Meses.Items.Add(New ListItem("ABRIL", 4))
        Meses.Items.Add(New ListItem("MAYO", 5))
        Meses.Items.Add(New ListItem("JUNIO", 6))
        Meses.Items.Add(New ListItem("JULIO", 7))
        Meses.Items.Add(New ListItem("AGOSTO", 8))
        Meses.Items.Add(New ListItem("SEPTIEMBRE", 9))
        Meses.Items.Add(New ListItem("OCTUBRE", 10))
        Meses.Items.Add(New ListItem("NOVIEMBRE", 11))
        Meses.Items.Add(New ListItem("DICIEMBRE", 12))
        Meses.Items(0).Value = 0
        Meses.Items(1).Value = 1
        Meses.Items(2).Value = 2
        Meses.Items(3).Value = 3
        Meses.Items(4).Value = 4
        Meses.Items(5).Value = 5
        Meses.Items(6).Value = 6
        Meses.Items(7).Value = 7
        Meses.Items(8).Value = 8
        Meses.Items(9).Value = 9
        Meses.Items(10).Value = 10
        Meses.Items(11).Value = 11
        Meses.Items(12).Value = 12

        If Not IsPostBack Then
            If Day(Now()) < 7 Then
                WMesActual = Month(Now()) - 1
                Meses.SelectedIndex = WMesActual
            Else
                WMesActual = Month(Now())
                Meses.SelectedIndex = WMesActual
            End If
        Else
            WMesActual = Meses.SelectedValue
        End If

        WMesAnterior = WMesActual - 1
        WAño = Year(Now())

        Session("Año") = WAño
        Session("MesActual") = WMesActual
        Session("MesAnterior") = WMesAnterior
        Session("Unidad") = WParametroSigla

        'TRAER DATOS POR MES Y UNIDAD
        Dim AOTTotalAdapter As New GerencialTableAdapters.MBO_PGAOTBudgetEjecucionAñoMesTableAdapter
        Dim AOTTotalDataTable As New Gerencial.MBO_PGAOTBudgetEjecucionAñoMesDataTable
        AOTTotalDataTable = AOTTotalAdapter.GetData(WAño)
        GVAOTPorUnidadMes.DataSource = AOTTotalDataTable
        GVAOTPorUnidadMes.DataBind()
        GVAOTPorUnidadMes.Visible = False

        'CALCULAR TOTALES AL MES ANTERIOR
        For I = 0 To GVAOTPorUnidadMes.Rows.Count - 1
            If Val(GVAOTPorUnidadMes.Rows(I).Cells(0).Text) = WAño And Val(GVAOTPorUnidadMes.Rows(I).Cells(1).Text) <= WMesAnterior And Trim(GVAOTPorUnidadMes.Rows(I).Cells(2).Text) = WParametroSigla Then
                WBudgetAHoy = WBudgetAHoy + Val(GVAOTPorUnidadMes.Rows(I).Cells(3).Text)
                WMetaAHoy = WMetaAHoy + Val(GVAOTPorUnidadMes.Rows(I).Cells(4).Text)
                WAOTAHoy = WAOTAHoy + (Val(GVAOTPorUnidadMes.Rows(I).Cells(5).Text))
            End If
            WBudgetTotal = WBudgetTotal + Val(GVAOTPorUnidadMes.Rows(I).Cells(3).Text)
            WMetaTotal = WMetaTotal + Val(GVAOTPorUnidadMes.Rows(I).Cells(4).Text)
        Next
        WAOTAHoy = WAOTAHoy / 1000

        'META Define ubicacion ACTION-WATCH-BOOSTER
        WLowerLimiteTotal = Round(0.3 * WMetaAHoy / 100000, 0) * 100000
        WMetaAction = 0.9 * WMetaAHoy
        WMetaWatch = WMetaAHoy
        WMetaBooster = WMetaAHoy * 1.1
        WUpperLimiteTotal = Round(WMetaAHoy * 1.1 / 100000, 0) * 100000

        'AOT vs META TOTAL
        Dim xmlAOTTotalM As New StringBuilder()
        xmlAOTTotalM.Append("<chart caption=' " & WParametroSigla & "-AOT vs META (KCOP) al mes de : " & WNMeses(WMesAnterior) & "' formatNumberScale='1' numberScaleValue='1000' numberScaleUnit='K'  lowerLimit='" & WLowerLimiteTotal & "' upperLimit='" & WUpperLimiteTotal & "' adjustTM='0' majorTMNumber='10' minorTMNumber='3' lowerLimitDisplay='Bad' upperLimitDisplay='Excelent'  showValue='1' decimals='0' >")
        xmlAOTTotalM.Append("<colorRange>")
        xmlAOTTotalM.Append("<color minValue='0' maxValue='" & WMetaAction & "' code='FF0000' label='ACTION' />")
        xmlAOTTotalM.Append("<color minValue='" & WMetaAction & "' maxValue='" & WMetaWatch & "' code='FFFF00' label='WATCH' />")
        xmlAOTTotalM.Append("<color minValue='" & WMetaWatch & "' maxValue='" & WMetaBooster & "' code='00FF00' label='BOOSTER' />")
        xmlAOTTotalM.Append("</colorRange>")
        xmlAOTTotalM.Append("<pointers>")
        xmlAOTTotalM.Append("<pointer value='" & WAOTAHoy & "' toolText= 'Actual' />")
        xmlAOTTotalM.Append("</pointers>")
        xmlAOTTotalM.Append("</chart>")
        AOTMetaTotal.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTTotalM.ToString(), "chartid1", "100%", "120px", False, True)

        'BUDGET-Define ubicacion ACTION-WATCH-BOOSTER
        WLowerLimiteTotal = Round(0.3 * WBudgetAHoy / 100000, 0) * 100000
        WBudgetAction = 0.9 * WBudgetAHoy
        WBudgetWatch = WBudgetAHoy
        WBudgetBooster = WBudgetAHoy * 1.1
        WUpperLimiteTotal = Round(WBudgetAHoy * 1.1 / 100000, 0) * 100000

        'AOT vs BUDGET TOTAL
        Dim xmlAOTTotalB As New StringBuilder()
        xmlAOTTotalB.Append("<chart caption='" & WParametroSigla & "-AOT vs BUDGET (KCOP) al mes de : " & WNMeses(WMesAnterior) & "' formatNumberScale='1' numberScaleValue='1000' numberScaleUnit='K'  lowerLimit='" & WLowerLimiteTotal & "' upperLimit='" & WUpperLimiteTotal & "' adjustTM='0' majorTMNumber='10' minorTMNumber='2' lowerLimitDisplay='Bad' upperLimitDisplay='Excelent'  showValue='1' decimals='0' >")
        xmlAOTTotalB.Append("<colorRange>")
        xmlAOTTotalB.Append("<color minValue='0' maxValue='" & WBudgetAction & "' code='FF0000' label='ACTION' />")
        xmlAOTTotalB.Append("<color minValue='" & WBudgetAction & "' maxValue='" & WBudgetWatch & "' code='FFFF00' label='WATCH' />")
        xmlAOTTotalB.Append("<color minValue='" & WBudgetWatch & "' maxValue='" & WBudgetBooster & "' code='00FF00' label='BOOSTER' />")
        xmlAOTTotalB.Append("</colorRange>")
        xmlAOTTotalB.Append("<pointers>")
        xmlAOTTotalB.Append("<pointer value='" & WAOTAHoy & "' toolText= 'Actual' />")
        xmlAOTTotalB.Append("</pointers>")
        xmlAOTTotalB.Append("</chart>")
        AOTBudgetTotal.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTTotalB.ToString(), "chartid2", "100%", "120px", False, True)

        '-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        'CALCULAR MES
        For I = 0 To GVAOTPorUnidadMes.Rows.Count - 1
            If Val(GVAOTPorUnidadMes.Rows(I).Cells(0).Text) = WAño And Val(GVAOTPorUnidadMes.Rows(I).Cells(1).Text) = (WMesActual) And Trim(GVAOTPorUnidadMes.Rows(I).Cells(2).Text) = WParametroSigla Then
                WBudgetMes = WBudgetMes + Val(GVAOTPorUnidadMes.Rows(I).Cells(3).Text)
                WMetaMes = WMetaMes + Val(GVAOTPorUnidadMes.Rows(I).Cells(4).Text)
                WAOTMes = WAOTMes + (Val(GVAOTPorUnidadMes.Rows(I).Cells(5).Text))
            End If
        Next
        WAOTMes = WAOTMes / 1000

        'META Define ubicacion ACTION-WATCH-BOOSTER
        WMetaAction = 0.9 * WMetaMes
        WMetaWatch = WMetaMes
        WMetaBooster = WMetaMes * 1.1

        'AOT vs META MES
        Dim xmlAOTMesM As New StringBuilder()
        xmlAOTMesM.Append("<chart caption='" & WParametroSigla & "-AOT vs META (KCOP) - Mes : " & WNMeses(WMesActual) & "' formatNumberScale='1' numberScaleValue='300'  lowerLimit='0' upperLimit='" & WMetaBooster & "' adjustTM='0' majorTMNumber='8' minorTMNumber='2' lowerLimitDisplay='Bad' upperLimitDisplay='Excelent'  showValue='1' decimals='0' >")
        xmlAOTMesM.Append("<colorRange>")
        xmlAOTMesM.Append("<color minValue='0' maxValue='" & WMetaAction & "' code='FF0000' label='ACTION' />")
        xmlAOTMesM.Append("<color minValue='" & WMetaAction & "' maxValue='" & WMetaWatch & "' code='FFFF00' label='WATCH' />")
        xmlAOTMesM.Append("<color minValue='" & WMetaWatch & "' maxValue='" & WMetaBooster & "' code='00FF00' label='BOOSTER' />")
        xmlAOTMesM.Append("</colorRange>")
        xmlAOTMesM.Append("<pointers>")
        xmlAOTMesM.Append("<pointer value='" & WAOTMes & "' toolText= 'Actual' />")
        xmlAOTMesM.Append("</pointers>")
        xmlAOTMesM.Append("</chart>")
        AOTMetaMes.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTMesM.ToString(), "chartid3", "100%", "120px", False, True)

        'BUDGET-Define ubicacion ACTION-WATCH-BOOSTER
        WBudgetAction = 0.9 * WBudgetMes
        WBudgetWatch = WBudgetMes
        WBudgetBooster = WBudgetMes * 1.1

        'AOT vs Budget MES
        Dim xmlAOTMesB As New StringBuilder()
        xmlAOTMesB.Append("<chart caption='" & WParametroSigla & "-AOT vs BUDGET (KCOP) - Mes: " & WNMeses(WMesActual) & "' formatNumberScale='1' numberScaleValue='300'  lowerLimit='0' upperLimit='" & WBudgetBooster & "' adjustTM='0' majorTMNumber='8' minorTMNumber='2' lowerLimitDisplay='Bad' upperLimitDisplay='Excelent'  showValue='1' decimals='0' >")
        xmlAOTMesB.Append("<colorRange>")
        xmlAOTMesB.Append("<color minValue='0' maxValue='" & WBudgetAction & "' code='FF0000' label='ACTION' />")
        xmlAOTMesB.Append("<color minValue='" & WBudgetAction & "' maxValue='" & WBudgetWatch & "' code='FFFF00' label='WATCH' />")
        xmlAOTMesB.Append("<color minValue='" & WBudgetWatch & "' maxValue='" & WBudgetBooster & "' code='00FF00' label='BOOSTER' />")
        xmlAOTMesB.Append("</colorRange>")
        xmlAOTMesB.Append("<pointers>")
        xmlAOTMesB.Append("<pointer value='" & WAOTMes & "' toolText= 'Actual' />")
        xmlAOTMesB.Append("</pointers>")
        xmlAOTMesB.Append("</chart>")
        AOTBudgetMes.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTMesB.ToString(), "chartid4", "100%", "120px", False, True)

        '------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        'DATOS AOT POR UNIDAD  MES ANTERIOR
        Dim AOTTotalUnidadAdapter As New GerencialTableAdapters.MBO_PGAOTBudgetEjecucionUnidadTableAdapter
        Dim AOTTotalUnidadDataTable As New Gerencial.MBO_PGAOTBudgetEjecucionUnidadDataTable
        AOTTotalUnidadDataTable = AOTTotalUnidadAdapter.GetData(WAño, 1, WMesAnterior)
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

        'AOT
        For I = 0 To GVAOTPorUnidad.Rows.Count - 1
            If Trim(GVAOTPorUnidad.Rows(I).Cells(0).Text) = WParametroSigla Then
                WSigla = GVAOTPorUnidad.Rows(I).Cells(0).Text
                WMetaU = Val(GVAOTPorUnidad.Rows(I).Cells(2).Text)
                WActualU = Val(GVAOTPorUnidad.Rows(I).Cells(3).Text)

                WMetaUAction = 0.9 * WMetaU
                WMetaUWatch = WMetaUAction
                WMetaUBooster = WMetaU
                If WActualU > WMetaUBooster Then
                    xmlAOTTotalU.Append("<chart caption= '" & WSigla & "-ACUMULADO a: " & WNMeses(WMesAnterior) & " Gerentes CLICK en ACTUAL' showToolTip='1' showvalues='1' numdivlines='10' xAxisName='' yAxisName='Valor' decimals='0' paletteColors='0000FF,669900,00FF00' >")
                Else
                    If WActualU > WMetaUWatch Then
                        xmlAOTTotalU.Append("<chart caption='" & WSigla & "-ACUMULADO a: " & WNMeses(WMesAnterior) & " Gerentes CLICK en ACTUAL' showToolTip='1' showvalues='1' numdivlines='10' xAxisName='' yAxisName='Valor' decimals='0' paletteColors='0000FF,669900,FFFF10' >")
                    Else
                        xmlAOTTotalU.Append("<chart caption='" & WSigla & "-ACUMULADO a: " & WNMeses(WMesAnterior) & " Gerentes CLICK en ACTUAL' showToolTip='1' showvalues='1' numdivlines='10' xAxisName='' yAxisName='Valor' decimals='0' paletteColors='0000FF,669900,FF0000' >")
                    End If
                End If
                xmlAOTTotalU.Append("<set Label='Budget' value='" & Val(GVAOTPorUnidad.Rows(I).Cells(1).Text) & "' />")
                xmlAOTTotalU.Append("<set Label='Meta' value='" & Val(GVAOTPorUnidad.Rows(I).Cells(2).Text) & "' />")
                xmlAOTTotalU.Append("<set Label='Actual' value='" & Val(GVAOTPorUnidad.Rows(I).Cells(3).Text) & "' link='AOTPorGerentes.aspx' />")
                xmlAOTTotalU.Append("</chart>")
                AOTUnidad.Text = FusionCharts.RenderChart("../FusionCharts/Column3D.swf", "", xmlAOTTotalU.ToString(), "chartida", "300px", "300px", False, True)
                xmlAOTTotalU.Clear()
            End If
        Next
        '-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        'DATOS MES ACTUAL
        AOTTotalUnidadDataTable = AOTTotalUnidadAdapter.GetData(WAño, WMesActual, WMesActual)
        GVAOTPorUnidad.DataSource = AOTTotalUnidadDataTable
        GVAOTPorUnidad.DataBind()
        GVAOTPorUnidad.Visible = False

        'Formatear
        For I = 0 To GVAOTPorUnidad.Rows.Count - 1
            GVAOTPorUnidad.Rows(I).Cells(1).Text = Val(GVAOTPorUnidad.Rows(I).Cells(1).Text) * 1000
            GVAOTPorUnidad.Rows(I).Cells(2).Text = Val(GVAOTPorUnidad.Rows(I).Cells(2).Text) * 1000
            GVAOTPorUnidad.Rows(I).Cells(3).Text = Val(GVAOTPorUnidad.Rows(I).Cells(3).Text)
        Next

        Dim xmlAOTTotalUMes As New StringBuilder()

        'AOT
        For I = 0 To GVAOTPorUnidad.Rows.Count - 1
            If Trim(GVAOTPorUnidad.Rows(I).Cells(0).Text) = WParametroSigla Then
                WSigla = GVAOTPorUnidad.Rows(I).Cells(0).Text
                WMetaU = Val(GVAOTPorUnidad.Rows(I).Cells(2).Text)
                WActualU = Val(GVAOTPorUnidad.Rows(I).Cells(3).Text)
                WMetaUAction = 0.9 * WMetaU
                WMetaUWatch = WMetaUAction
                WMetaUBooster = WMetaU
                If WActualU > WMetaUBooster Then
                    xmlAOTTotalUMes.Append("<chart caption='" & WSigla & "-MES de: " & WNMeses(WMesActual) & " Gerentes CLICK en ACTUAL' showToolTip='1' showvalues='1' numdivlines='10' xAxisName='' yAxisName='Valor' decimals='0' paletteColors='0000FF,669900,00FF00' >")
                Else
                    If WActualU > WMetaUWatch Then
                        xmlAOTTotalUMes.Append("<chart caption='" & WSigla & "-MES de: " & WNMeses(WMesActual) & " Gerentes CLICK en ACTUAL' showToolTip='1' showvalues='1' numdivlines='10' xAxisName='' yAxisName='Valor' decimals='0' paletteColors='0000FF,669900,FFFF10' >")
                    Else
                        xmlAOTTotalUMes.Append("<chart caption='" & WSigla & "-MES de: " & WNMeses(WMesActual) & " Gerentes CLICK en ACTUAL' showToolTip='1' showvalues='1' numdivlines='10' xAxisName='' yAxisName='Valor' decimals='0' paletteColors='0000FF,669900,FF0000' >")
                    End If
                End If
                xmlAOTTotalUMes.Append("<set Label='Budget' value='" & Val(GVAOTPorUnidad.Rows(I).Cells(1).Text) & "' />")
                xmlAOTTotalUMes.Append("<set Label='Meta' value='" & Val(GVAOTPorUnidad.Rows(I).Cells(2).Text) & "' />")
                xmlAOTTotalUMes.Append("<set Label='Actual' value='" & Val(GVAOTPorUnidad.Rows(I).Cells(3).Text) & "' />")
                xmlAOTTotalUMes.Append("</chart>")
                AOTUnidadMes.Text = FusionCharts.RenderChart("../FusionCharts/Column3D.swf", "", xmlAOTTotalUMes.ToString(), "chartidg", "300px", "300px", False, True)
                xmlAOTTotalUMes.Clear()
            End If
        Next
        '-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        'ACQUISITION AND ACHIEVEMENT
        Dim WAcqAction As Integer
        Dim WAcqWatch As Integer
        Dim WAcqBooster As Integer
        Dim WAcqMeta As Double
        Dim WAcqActual As Double
        Dim xmlAOTACQ As New StringBuilder()
        Dim AOTAcquisitionAdapter As New GerencialTableAdapters.MBO_AOTAcquisitionTableAdapter
        Dim AOTAcquisitionDataTable As New Gerencial.MBO_AOTAcquisitionDataTable
        AOTAcquisitionDataTable = AOTAcquisitionAdapter.GetData(WAño, WMesAnterior)
        GVAOTAcquisition.DataSource = AOTAcquisitionDataTable
        GVAOTAcquisition.DataBind()
        GVAOTAcquisition.Visible = False

        For I = 0 To GVAOTAcquisition.Rows.Count - 1
            If Trim(GVAOTAcquisition.Rows(I).Cells(1).Text) = WParametroSigla Then
                WSigla = GVAOTAcquisition.Rows(I).Cells(1).Text

                'ACQUISITION
                'Actual = ActualYTD / MetaFY 
                WAcqActual = Val(GVAOTAcquisition.Rows(I).Cells(4).Text) / Val(GVAOTAcquisition.Rows(I).Cells(3).Text) * 100
                'Meta = MetaYTD / MetaFY
                WAcqMeta = Val(GVAOTAcquisition.Rows(I).Cells(5).Text) / Val(GVAOTAcquisition.Rows(I).Cells(3).Text) * 100
                WAcqAction = 0.9 * WAcqMeta
                WAcqWatch = WAcqMeta
                WAcqBooster = WAcqMeta * 1.1

                xmlAOTACQ.Append("<chart caption='% Acquisition-" & WSigla & "' lowerLimit='0' upperLimit='" & WAcqBooster & "' adjustTM='0' majorTMNumber='6' minorTMNumber='1' lowerLimitDisplay='Bad' upperLimitDisplay='Excelent'  showValue='1' decimals='1'  tickValueDecimals='1' forceTickValueDecimals='1' numberSuffix='%' >")
                xmlAOTACQ.Append("<colorRange>")
                xmlAOTACQ.Append("<color minValue='0' maxValue='" & WAcqAction & "' code='FF0000' label='ACTION' />")
                xmlAOTACQ.Append("<color minValue='" & WAcqAction & "' maxValue='" & WAcqWatch & "' code='FFFF00' label='WATCH' />")
                xmlAOTACQ.Append("<color minValue='" & WAcqWatch & "' maxValue='" & WAcqBooster & "' code='00FF00' label='BOOSTER' />")
                xmlAOTACQ.Append("</colorRange>")
                xmlAOTACQ.Append("<pointers>")
                xmlAOTACQ.Append("<pointer value='" & Str(WAcqActual) & "' toolText= 'ActualYTD/MetaFY' />")
                xmlAOTACQ.Append("</pointers>")
                xmlAOTACQ.Append("</chart>")
                AcquisitionUnidad.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTACQ.ToString(), "chartid10", "300", "150", False, True)
                xmlAOTACQ.Clear()

                'ACHIEVEMENT---Se usan las mismas variables
                'Actual = ActualYTD / MetaYTD 
                WAcqActual = Val(GVAOTAcquisition.Rows(I).Cells(4).Text) / Val(GVAOTAcquisition.Rows(I).Cells(5).Text) * 100
                'Meta = MetaYTD / MetaYTD
                WAcqMeta = Val(GVAOTAcquisition.Rows(I).Cells(5).Text) / Val(GVAOTAcquisition.Rows(I).Cells(5).Text) * 100
                WAcqAction = 0.9 * WAcqMeta
                WAcqWatch = WAcqMeta
                WAcqBooster = WAcqMeta * 1.1

                xmlAOTACQ.Append("<chart caption='% Achievement-" & WSigla & "' lowerLimit='0' upperLimit='" & WAcqBooster & "' adjustTM='0' majorTMNumber='6' minorTMNumber='1' lowerLimitDisplay='Bad' upperLimitDisplay='Excelent'  showValue='1' decimals='1'  tickValueDecimals='1' forceTickValueDecimals='1' numberSuffix='%' >")
                xmlAOTACQ.Append("<colorRange>")
                xmlAOTACQ.Append("<color minValue='0' maxValue='" & WAcqAction & "' code='FF0000' label='ACTION' />")
                xmlAOTACQ.Append("<color minValue='" & WAcqAction & "' maxValue='" & WAcqWatch & "' code='FFFF00' label='WATCH' />")
                xmlAOTACQ.Append("<color minValue='" & WAcqWatch & "' maxValue='" & WAcqBooster & "' code='00FF00' label='BOOSTER' />")
                xmlAOTACQ.Append("</colorRange>")
                xmlAOTACQ.Append("<pointers>")
                xmlAOTACQ.Append("<pointer value='" & Str(WAcqActual) & "' toolText= 'ActualYTD/MetaYTD' />")
                xmlAOTACQ.Append("</pointers>")
                xmlAOTACQ.Append("</chart>")
                AchievementUnidad.Text = FusionCharts.RenderChart("../FusionWidgets/HLinearGauge.swf", "", xmlAOTACQ.ToString(), "chartid20", "300", "150", False, True)
                xmlAOTACQ.Clear()
            End If
        Next
    End Sub

End Class