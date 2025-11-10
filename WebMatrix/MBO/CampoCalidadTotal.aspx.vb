Imports System.Math
Imports InfosoftGlobal
Imports CoreProject
Public Class MBO_Operaciones
    Inherits System.Web.UI.Page
    Dim IndicesMeses(13, 3) As String
    Dim WIndice As Double
    Public WAño As Integer
    Public WAñoAnterior As Integer
    Public WAñoActual As Integer
    Public WGrafica As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(49, UsuarioID) = False Then
            Response.Redirect("../RE_GT/RecoleccionDeDatos.aspx")
        End If


        WAñoActual = Year(Now())
        WAñoAnterior = WAñoActual - 1

        'Para mostrar indice total acumnulado
        Dim CampoCalidadTotalAdapter As New OperacionesTableAdapters.MBO_OPCampoCalidadTotalTableAdapter
        Dim CampoCalidadTotalDataTable As New Operaciones.MBO_OPCampoCalidadTotalDataTable

        CampoCalidadTotalDataTable = CampoCalidadTotalAdapter.GetData(WAñoActual)
        GVCampoCalidadTotal.DataSource = CampoCalidadTotalDataTable
        GVCampoCalidadTotal.DataBind()
        GVCampoCalidadTotal.Visible = False

        WIndice = GVCampoCalidadTotal.Rows(0).Cells(3).Text

        'Mostrar indice año anterior
        Dim xmlCampoCalidadTotal1 As New StringBuilder()
        'Generate the chart element
        xmlCampoCalidadTotal1.Append("<chart caption='Calidad Año- " & WAñoAnterior & "' lowerLimit='0' upperLimit='30' majorTMNumber='10' minorTMNumber='2' lowerLimitDisplay='Excelente' upperLimitDisplay='Mal' gaugeStartAngle='0' gaugeEndAngle='180' decimals='1' numberSuffix='%' showValue='1'>")
        xmlCampoCalidadTotal1.Append("<colorRange>")
        xmlCampoCalidadTotal1.Append("<color minValue='0' maxValue='6' code='8BBA00'/>")
        xmlCampoCalidadTotal1.Append("<color minValue='6' maxValue='10' code='F6BD0F'/>")
        xmlCampoCalidadTotal1.Append("<color minValue='10' maxValue='30' code='FF654F'/>")
        xmlCampoCalidadTotal1.Append("</colorRange>")
        xmlCampoCalidadTotal1.Append("<dials> <dial value='" & Str(WIndice) & "' valueX='150' valueY='150' /> </dials>")
        xmlCampoCalidadTotal1.Append("</chart>")

        'Create the chart - Multi-Series Line Chart with data from xmlData
        AñoAnterior.Text = FusionCharts.RenderChart("../FusionWidgets/AngularGauge.swf", "", xmlCampoCalidadTotal1.ToString(), "chartid1", "300", "200", False, True)

        'Mostrar indice año actual
        WIndice = GVCampoCalidadTotal.Rows(1).Cells(3).Text

        Dim xmlCampoCalidadTotal2 As New StringBuilder()
        'Generate the chart element
        xmlCampoCalidadTotal2.Append("<chart caption='Calidad Año- " & WAñoActual & "' lowerLimit='0' upperLimit='30' majorTMNumber='10' minorTMNumber='2' lowerLimitDisplay='Excelente' upperLimitDisplay='Mal' gaugeStartAngle='0' gaugeEndAngle='180'  decimals='1' numberSuffix='%' showValue='1'>")
        xmlCampoCalidadTotal2.Append("<colorRange>")
        xmlCampoCalidadTotal2.Append("<color minValue='0' maxValue='6' code='8BBA00'/>")
        xmlCampoCalidadTotal2.Append("<color minValue='6' maxValue='10' code='F6BD0F'/>")
        xmlCampoCalidadTotal2.Append("<color minValue='10' maxValue='30' code='FF654F'/>")

        xmlCampoCalidadTotal2.Append("</colorRange>")
        xmlCampoCalidadTotal2.Append("<dials> <dial value='" & Str(WIndice) & "' valueX='150' valueY='150' /> </dials>")
        xmlCampoCalidadTotal2.Append("</chart>")

        'Create the chart - Multi-Series Line Chart with data from xmlData
        AñoActual.Text = FusionCharts.RenderChart("../FusionWidgets/AngularGauge.swf", "", xmlCampoCalidadTotal2.ToString(), "chartid2", "300", "200", False, True)

        WGrafica = 1
        WAño = WAñoAnterior
        CalidadMesTotalAñoAnterior.Text = ObtenerDatos()

        WGrafica = 2
        WAño = WAñoActual
        CalidadMesTotalAñoActual.Text = ObtenerDatos()

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
        xmlIndiceTotalMes.Append("<chart caption='Historico mensual- " & WAño & "' showToolTip='1' showBorder='1' numdivlines='36' yAxisMaxValue='36' formatNumberScale='0' rotatelabels='1' xAxisName='Mes' yAxisName='Indice' showvalues='0' decimals='2' paletteColors='FF0000,0372AB,48FB0D'>")
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
        Dim WMesAnterior As Integer
        Dim WMesALeer As Integer

        WMesAnterior = Month(Now())
        WMesAnterior = WMesAnterior - 1
        If WGrafica = 1 Then
            WMesALeer = 12
        Else
            WMesALeer = WMesAnterior
        End If

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

        IndiceMesTotalDataTable = IndiceMesTotalAdapter.GetData(9, WAño, WMesALeer)
        IndicesMeses(0, 1) = "Total"
        For Each IndiceMesTotalRow In IndiceMesTotalDataTable.Rows
            WMes = CInt(IndiceMesTotalRow.Mes)

            Try
                If IndiceMesTotalRow.Errores > 0 Then
                    WIndice = IndiceMesTotalRow.Errores / IndiceMesTotalRow.Encuestas * 100
                Else
                    WIndice = 0
                End If
            Catch
                WIndice = 0
            End Try

            IndicesMeses(WMes, 1) = Str(WIndice)
        Next

        'Carga indice asociados del mes al arreglo
        IndiceMesTotalDataTable = IndiceMesTotalAdapter.GetData(2, WAño, WMesALeer)
        IndicesMeses(0, 2) = "Empleados"
        For Each IndiceMesTotalRow In IndiceMesTotalDataTable.Rows
            WMes = CInt(IndiceMesTotalRow.Mes)

            Try
                If IndiceMesTotalRow.Errores > 0 Then
                    WIndice = IndiceMesTotalRow.Errores / IndiceMesTotalRow.Encuestas * 100
                Else
                    WIndice = 0
                End If
            Catch
                WIndice = 0
            End Try

            IndicesMeses(WMes, 2) = Str(WIndice)
        Next

        'Carga indice contratistas del mes al arreglo
        IndiceMesTotalDataTable = IndiceMesTotalAdapter.GetData(7, WAño, WMesALeer)
        IndicesMeses(0, 3) = "Contratistas"
        For Each IndiceMesTotalRow In IndiceMesTotalDataTable.Rows
            WMes = CInt(IndiceMesTotalRow.Mes)

            Try
                If IndiceMesTotalRow.Errores > 0 Then
                    WIndice = IndiceMesTotalRow.Errores / IndiceMesTotalRow.Encuestas * 100
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