Imports System.Math
Imports InfosoftGlobal
Public Class MBO_Operaciones
    Inherits System.Web.UI.Page
    Dim IndicesMeses(13, 3) As String
    Dim WIndice As Double
    Public WAño As Integer
    Public WAñoAnterior As Integer
    Public WAñoActual As Integer
    Public WGrafica As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim I As Integer
        Dim WMuestra As Long
        Dim WErrores As Long
        'Para mostrar indice total acumnulado
        Dim CampoCalidadTotalAdapter As New OperacionesTableAdapters.MBO_OPCampoCalidadTotalTableAdapter
        Dim CampoCalidadTotalDataTable As New Operaciones.MBO_OPCampoCalidadTotalDataTable
        Dim CampoCalidadTotal As Operaciones.MBO_OPCampoCalidadTotalRow

        CampoCalidadTotalDataTable = CampoCalidadTotalAdapter.GetData()
        GVCampoCalidadTotal.DataSource = CampoCalidadTotalDataTable
        GVCampoCalidadTotal.DataBind()
        GVCampoCalidadTotal.Visible = False

        'Mostrar indice 2011
        Dim xmlCampoCalidadTotal1 As New StringBuilder()
        'Generate the chart element
        xmlCampoCalidadTotal1.Append("<chart caption='Calidad Año- " & WAñoAnterior & "' lowerLimit='0' upperLimit='50' lowerLimitDisplay='Excelente' upperLimitDisplay='Mal' numberSuffix='%' showValue='1'>")
        xmlCampoCalidadTotal1.Append("<colorRange>")
        xmlCampoCalidadTotal1.Append("<color minValue='0' maxValue='6' code='8BBA00'/>")
        xmlCampoCalidadTotal1.Append("<color minValue='6' maxValue='10' code='F6BD0F'/>")
        xmlCampoCalidadTotal1.Append("<color minValue='10' maxValue='50' code='FF654F'/>")
        xmlCampoCalidadTotal1.Append("</colorRange>")
        xmlCampoCalidadTotal1.Append("<dials> <dial value='" & Val(GVCampoCalidadTotal.Rows(0).Cells(3).Text) & "'/> </dials>")
        xmlCampoCalidadTotal1.Append("</chart>")

        WAñoActual = 2012
        WAñoAnterior = WAñoActual - 1

        'Create the chart - Multi-Series Line Chart with data from xmlData
        AñoAnterior.Text = FusionCharts.RenderChart("../FusionWidgets/AngularGauge.swf", "", xmlCampoCalidadTotal1.ToString(), "chartid1", "300", "200", False, True)

        'Mostrar indice 2012
        Dim xmlCampoCalidadTotal2 As New StringBuilder()
        'Generate the chart element
        xmlCampoCalidadTotal2.Append("<chart caption='Calidad Año- " & WAñoActual & "' lowerLimit='0' upperLimit='50' lowerLimitDisplay='Excelente' upperLimitDisplay='Mal' numberSuffix='%' showValue='1'>")
        xmlCampoCalidadTotal2.Append("<colorRange>")
        xmlCampoCalidadTotal2.Append("<color minValue='0' maxValue='6' code='8BBA00'/>")
        xmlCampoCalidadTotal2.Append("<color minValue='6' maxValue='10' code='F6BD0F'/>")
        xmlCampoCalidadTotal2.Append("<color minValue='10' maxValue='50' code='FF654F'/>")
        xmlCampoCalidadTotal2.Append("</colorRange>")
        xmlCampoCalidadTotal2.Append("<dials> <dial value='" & Val(GVCampoCalidadTotal.Rows(1).Cells(3).Text) & "'/> </dials>")
        xmlCampoCalidadTotal2.Append("</chart>")

        'Create the chart - Multi-Series Line Chart with data from xmlData
        AñoActual.Text = FusionCharts.RenderChart("../FusionWidgets/AngularGauge.swf", "", xmlCampoCalidadTotal2.ToString(), "chartid2", "300", "200", False, True)


        For I = 0 To GVCampoCalidadTotal.Rows.Count - 1
            WAño = Val(GVCampoCalidadTotal.Rows(I).Cells(0).Text)
            WMuestra = Val(GVCampoCalidadTotal.Rows(I).Cells(1).Text)
            WErrores = Val(GVCampoCalidadTotal.Rows(I).Cells(2).Text)
        Next

        CampoCalidadTotal = CampoCalidadTotalAdapter.GetData(0)

        WGrafica = 1
        WAño = WAñoAnterior
        CalidadMesTotalAñoAnterior.Text = ObtenerDatos()
        WGrafica = 2
        WAño = WAñoActual
        CalidadMesTotalañoActual.Text = ObtenerDatos()

    End Sub
    Public Sub GVCampoCalidadTotal_OnRowCreated(ByVal sender As Object, ByVal e As Web.UI.WebControls.GridViewRowEventArgs) Handles GVCampoCalidadTotal.RowCreated
        e.Row.Cells(1).Visible = False 'Muestra
        e.Row.Cells(2).Visible = False 'Errores
    End Sub
    Public Function ObtenerDatos() As String
        Dim xmlIndiceTotalMes As New StringBuilder()
        Dim WMes As Integer
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

        'Cargar datos del dataset
        CargarIndiceMesTotal()

        'Generate the chart element
        xmlIndiceTotalMes.Append("<chart caption='Historico mensual- " & WAño & "' showToolTip='1' showBorder='1' numdivlines='36' yAxisMaxValue='56' formatNumberScale='0' rotatelabels='1' xAxisName='Mes' yAxisName='Indice' showvalues='0' decimals='2' paletteColors='FF0000,0372AB,48FB0D'>")
        xmlIndiceTotalMes.Append("<categories>")
        For WMes = 1 To 12
            xmlIndiceTotalMes.Append("<category label='" & WNMeses(WMes) & "' />")
        Next WMes
        'Close categories element
        xmlIndiceTotalMes.Append("</categories>")

        'Iterate through each record
        xmlIndiceTotalMes.Append("<dataset seriesName='Total' >")
        For WMes = 1 To 12
            'Generate <set value='..' />
            xmlIndiceTotalMes.Append("<set value='" & Trim(IndicesMeses(WMes, 1)) & "' />")
        Next WMes
        'Close dataset element
        xmlIndiceTotalMes.Append("</dataset>")

        xmlIndiceTotalMes.Append("<dataset seriesName='Empleados' >")
        For WMes = 1 To 12
            'Generate <set value='..' />
            xmlIndiceTotalMes.Append("<set value='" & Trim(IndicesMeses(WMes, 2)) & "' />")
        Next WMes
        'Close dataset element
        xmlIndiceTotalMes.Append("</dataset>")

        xmlIndiceTotalMes.Append("<dataset seriesName='Proveedores' >")
        For WMes = 1 To 12
            xmlIndiceTotalMes.Append("<set value='" & Trim(IndicesMeses(WMes, 3)) & "' />")
        Next WMes
        xmlIndiceTotalMes.Append("</dataset>")
        xmlIndiceTotalMes.Append("</chart>")
        If WGrafica = 1 Then
            Return FusionCharts.RenderChart("../FusionCharts/MSLine.swf", "", xmlIndiceTotalMes.ToString(), "chartid10", "650", "450", False, True)
            xmlIndiceTotalMes.Clear()
        Else
            Return FusionCharts.RenderChart("../FusionCharts/MSLine.swf", "", xmlIndiceTotalMes.ToString(), "chartid20", "650", "450", False, True)
            xmlIndiceTotalMes.Clear()
        End If

    End Function
    Private Sub CargarIndiceMesTotal()
        Dim WMes As Integer
        Dim IndiceMesTotalAdapter As New OperacionesTableAdapters.MBO_OPCampoCalidadMesTotalTableAdapter
        Dim IndiceMesTotalDataTable As New Operaciones.MBO_OPCampoCalidadMesTotalDataTable
        Dim IndiceMesTotalRow As Operaciones.MBO_OPCampoCalidadMesTotalRow
        For I = 0 To 13
            IndicesMeses(I, 0) = ""
            IndicesMeses(I, 1) = ""
            IndicesMeses(I, 2) = ""
            IndicesMeses(I, 3) = ""
        Next

        'Carga indice total del mes al arreglo
        IndiceMesTotalDataTable = IndiceMesTotalAdapter.GetData(9, WAño)
        IndicesMeses(0, 1) = "Total"
        For Each IndiceMesTotalRow In IndiceMesTotalDataTable.Rows
            WMes = CInt(IndiceMesTotalRow.Mes)

            Try
                If IndiceMesTotalRow.Errores > 0 Then
                    WIndice = IndiceMesTotalRow.Errores / IndiceMesTotalRow.Muestra * 100
                Else
                    WIndice = 0
                End If
            Catch
                WIndice = 0
            End Try

            IndicesMeses(WMes, 1) = Str(WIndice)
        Next

        'Carga indice asociados del mes al arreglo
        IndiceMesTotalDataTable = IndiceMesTotalAdapter.GetData(2, WAño)
        IndicesMeses(0, 2) = "Empleados"
        For Each IndiceMesTotalRow In IndiceMesTotalDataTable.Rows
            WMes = CInt(IndiceMesTotalRow.Mes)

            Try
                If IndiceMesTotalRow.Errores > 0 Then
                    WIndice = IndiceMesTotalRow.Errores / IndiceMesTotalRow.Muestra * 100
                Else
                    WIndice = 0
                End If
            Catch
                WIndice = 0
            End Try

            IndicesMeses(WMes, 2) = Str(WIndice)
        Next

        'Carga indice contratistas del mes al arreglo
        IndiceMesTotalDataTable = IndiceMesTotalAdapter.GetData(7, WAño)
        IndicesMeses(0, 3) = "Contratistas"
        For Each IndiceMesTotalRow In IndiceMesTotalDataTable.Rows
            WMes = CInt(IndiceMesTotalRow.Mes)

            Try
                If IndiceMesTotalRow.Errores > 0 Then
                    WIndice = IndiceMesTotalRow.Errores / IndiceMesTotalRow.Muestra * 100
                Else
                    WIndice = 0
                End If
            Catch
                WIndice = 0
            End Try

            IndicesMeses(WMes, 3) = Str(WIndice)
        Next
    End Sub

End Class