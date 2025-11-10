Imports InfosoftGlobal
Public Class IndicesManuales
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim WIndicador As String = ""
        Dim WMeta As String = ""
        Dim WAñoActual As Integer
        Dim WAñoAnterior As Integer
        Dim WMes As Integer
        Dim IndicesMeses(12) As String
        Dim WNMeses(12) As String
        WNMeses(1) = "Ene"
        WNMeses(2) = "Feb"
        WNMeses(3) = "Mar"
        WNMeses(4) = "Abr"
        WNMeses(5) = "May"
        WNMeses(6) = "Jun"
        WNMeses(7) = "Jul"
        WNMeses(8) = "Ago"
        WNMeses(9) = "Sep"
        WNMeses(10) = "Oct"
        WNMeses(11) = "Nov"
        WNMeses(12) = "Dic"

        WAñoActual = Year(Now())
        WAñoAnterior = Year(Now()) - 1

        ReDim IndicesMeses(12)

        Dim IndicesManualesAdapter As New GerencialTableAdapters.MBO_PGIndicesManualesTableAdapter
        Dim IndicesManualesDataTable As New Gerencial.MBO_PGIndicesManualesDataTable
        Dim IndicesManualesRow As Gerencial.MBO_PGIndicesManualesRow

        '--CT1
        IndicesManualesDataTable = IndicesManualesAdapter.GetData("CT")

        'Carga los indices al arreglo
        For Each IndicesManualesRow In IndicesManualesDataTable.Rows
            If IndicesManualesRow.ID = "CT1" Then
                If IndicesManualesRow.Resultado = 0 Then
                Else
                    WMes = CInt(IndicesManualesRow.Mes)
                    IndicesMeses(WMes) = Str(IndicesManualesRow.Resultado)
                End If
                WIndicador = "-" & IndicesManualesRow.Indicador
                WMeta = "- Meta -" & Str(IndicesManualesRow.Meta)
            End If
        Next

        Dim xmlIndiceTotalMes As New StringBuilder()

        xmlIndiceTotalMes.Append("<chart caption='" & WAñoAnterior & WIndicador & WMeta & "' showToolTip='1' showBorder='1' numdivlines='10' yAxisMaxValue='10' formatNumberScale='0' rotatelabels='1' xAxisName='Mes' yAxisName='Indice' showvalues='0' decimals='1' paletteColors='FF0000'>")
        xmlIndiceTotalMes.Append("<categories>")
        For WMes = 1 To 12
            xmlIndiceTotalMes.Append("<category label='" & WNMeses(WMes) & "' />")
        Next WMes
        xmlIndiceTotalMes.Append("</categories>")

        xmlIndiceTotalMes.Append("<dataset seriesName='CT1' >")
        For WMes = 1 To 12
            If IndicesMeses(WMes) > 0 Then
                xmlIndiceTotalMes.Append("<set value='" & IndicesMeses(WMes) & "' />")
            Else
                xmlIndiceTotalMes.Append("<set value=' ' />")
            End If
        Next WMes
        xmlIndiceTotalMes.Append("</dataset>")
        xmlIndiceTotalMes.Append("</chart>")

        GRCuentasCT1.Text = FusionCharts.RenderChart("../FusionCharts/MSLine.swf", "", xmlIndiceTotalMes.ToString(), "chartid10", "350", "300", False, True)
        xmlIndiceTotalMes.Clear()

        '----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        '--CT2
        ReDim IndicesMeses(12)
        'Carga los indices al arreglo
        For Each IndicesManualesRow In IndicesManualesDataTable.Rows
            If IndicesManualesRow.ID = "CT2" Then
                If IndicesManualesRow.Resultado = 0 Then
                Else
                    WMes = CInt(IndicesManualesRow.Mes)
                    IndicesMeses(WMes) = Str(IndicesManualesRow.Resultado)
                End If
                WIndicador = "-" & IndicesManualesRow.Indicador
                WMeta = "- Meta -" & Str(IndicesManualesRow.Meta)
            End If
        Next

        xmlIndiceTotalMes.Append("<chart caption='" & WAñoActual & WIndicador & WMeta & "' showToolTip='1' showBorder='1' numdivlines='10' yAxisMaxValue='100' formatNumberScale='0' rotatelabels='1' xAxisName='Mes' yAxisName='Indice' showvalues='0' decimals='1' paletteColors='FF0000'>")
        xmlIndiceTotalMes.Append("<categories>")
        For WMes = 1 To 12
            xmlIndiceTotalMes.Append("<category label='" & WNMeses(WMes) & "' />")
        Next WMes
        xmlIndiceTotalMes.Append("</categories>")

        xmlIndiceTotalMes.Append("<dataset seriesName='CT2' >")
        For WMes = 1 To 12
            If IndicesMeses(WMes) > 0 Then
                xmlIndiceTotalMes.Append("<set value='" & IndicesMeses(WMes) & "' />")
            Else
                xmlIndiceTotalMes.Append("<set value=' ' />")
            End If
        Next WMes
        xmlIndiceTotalMes.Append("</dataset>")
        xmlIndiceTotalMes.Append("</chart>")

        GRCuentasCT2.Text = FusionCharts.RenderChart("../FusionCharts/MSLine.swf", "", xmlIndiceTotalMes.ToString(), "chartid20", "350", "300", False, True)
        xmlIndiceTotalMes.Clear()

        '------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        '--CT3
        ReDim IndicesMeses(12)
        'Carga los indices al arreglo
        For Each IndicesManualesRow In IndicesManualesDataTable.Rows
            If IndicesManualesRow.ID = "CT3" Then
                If IndicesManualesRow.Resultado = 0 Then
                Else
                    WMes = CInt(IndicesManualesRow.Mes)
                    IndicesMeses(WMes) = Str(IndicesManualesRow.Resultado)
                End If
                WIndicador = "-" & IndicesManualesRow.Indicador
                WMeta = "- Meta -" & Str(IndicesManualesRow.Meta)
            End If
        Next

        xmlIndiceTotalMes.Append("<chart caption='" & WAñoActual & WIndicador & WMeta & "' showToolTip='1' showBorder='1' numdivlines='10' yAxisMaxValue='5' formatNumberScale='0' rotatelabels='1' xAxisName='Mes' yAxisName='Indice' showvalues='0' decimals='1' paletteColors='FF0000'>")
        xmlIndiceTotalMes.Append("<categories>")
        For WMes = 1 To 12
            xmlIndiceTotalMes.Append("<category label='" & WNMeses(WMes) & "' />")
        Next WMes
        xmlIndiceTotalMes.Append("</categories>")

        xmlIndiceTotalMes.Append("<dataset seriesName='CT3' >")
        For WMes = 1 To 12
            If IndicesMeses(WMes) > 0 Then
                xmlIndiceTotalMes.Append("<set value='" & IndicesMeses(WMes) & "' />")
            Else
                xmlIndiceTotalMes.Append("<set value=' ' />")
            End If
        Next WMes
        xmlIndiceTotalMes.Append("</dataset>")
        xmlIndiceTotalMes.Append("</chart>")

        GRCuentasCT3.Text = FusionCharts.RenderChart("../FusionCharts/MSLine.swf", "", xmlIndiceTotalMes.ToString(), "chartid30", "350", "300", False, True)
        xmlIndiceTotalMes.Clear()

    End Sub
End Class