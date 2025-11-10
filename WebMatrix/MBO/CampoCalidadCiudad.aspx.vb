Imports InfosoftGlobal
Public Class CampoCalidadCiudad
    Inherits System.Web.UI.Page
    Dim WFilas, WMesActual As Integer
    Dim IndicesMeses(13, 4) As String
    Dim WIndice As Double
    Dim Mes As Integer
    Dim WAño As Integer
    Dim WMes As Integer
    Dim WMesAnterior As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        WAño = Year(Now())
        WMes = Month(Now())
        If WMes = 1 Then
            WMesAnterior = 12
        Else
            WMesAnterior = WMes - 1
        End If
        'Mostrar indice mes ciudad
        CargarDatos()
        GraficarMesCiudad()

        'Para mostrar indice total por Ciudad
        Dim IndiceTotalCiudadAdapter As New OperacionesTableAdapters.MBO_OPCampoCiudadTotalTableAdapter
        Dim IndiceTotalCiudadDataTable As New Operaciones.MBO_OPCampoCiudadTotalDataTable

        '---CIUDADES TOTAL
        IndiceTotalCiudadDataTable = IndiceTotalCiudadAdapter.GetData(1, WAño)
        'Agregar columna indice al DataTable
        IndiceTotalCiudadDataTable.Columns.Add("Indice")
        IndiceTotalCiudadDataTable.AcceptChanges()
        GVCiudades.DataSource = IndiceTotalCiudadDataTable
        GVCiudades.DataBind()

        WFilas = GVCiudades.Rows.Count
        For I = 0 To WFilas - 1
            If Val(GVCiudades.Rows(I).Cells(2).Text) > 0 Then
                WIndice = Val(GVCiudades.Rows(I).Cells(2).Text) / Val(GVCiudades.Rows(I).Cells(1).Text) * 100
                GVCiudades.Rows(I).Cells(3).Text = Format(WIndice, "##0.00")
            Else
                WIndice = 0
                GVCiudades.Rows(I).Cells(3).Text = Format(WIndice, "##0.00")
            End If
        Next

        Dim xmlCiudades As New StringBuilder()
        Dim WCiudad As String
        Dim WMeta As Double
        Dim WActual As Double
        Dim WMetaAction As Double
        Dim WMetaWatch As Double
        Dim WMetaBooster As Double


        xmlCiudades.Append("<chart caption='CIUDADES TOTAL' showToolTip='1' showvalues='1' numdivlines='10' xAxisName='Ciudades' yAxisName='Indice' decimals='1' >")
        For I = 0 To GVCiudades.Rows.Count - 1
            WCiudad = GVCiudades.Rows(I).Cells(0).Text
            WMeta = 6.5
            WActual = Val(GVCiudades.Rows(I).Cells(3).Text)
            WMetaAction = 1.1 * WMeta
            WMetaWatch = WMetaAction
            WMetaBooster = WMeta
            If WActual < WMetaBooster Then
                xmlCiudades.Append("<set Label='" & GVCiudades.Rows(I).Cells(0).Text & "' value='" & Val(GVCiudades.Rows(I).Cells(3).Text) & "' color='00FF00' />")
            Else
                If WActual < WMetaWatch Then
                    xmlCiudades.Append("<set Label='" & GVCiudades.Rows(I).Cells(0).Text & "' value='" & Val(GVCiudades.Rows(I).Cells(3).Text) & "' color='FFFF10' />")
                Else
                    xmlCiudades.Append("<set Label='" & GVCiudades.Rows(I).Cells(0).Text & "' value='" & Val(GVCiudades.Rows(I).Cells(3).Text) & "' color='FF0000'  />")
                End If
            End If
        Next
        xmlCiudades.Append("</chart>")
        GraficaCiudadesTotal.Text = FusionCharts.RenderChart("../FusionCharts/Column3D.swf", "", xmlCiudades.ToString(), "chartida", "600px", "400px", False, True)
        xmlCiudades.Clear()

        '--------CIUDADES EMPLEADOS
        IndiceTotalCiudadDataTable = IndiceTotalCiudadAdapter.GetData(2, WAño)
        'Agregar columna indice al DataTable
        IndiceTotalCiudadDataTable.Columns.Add("Indice")
        IndiceTotalCiudadDataTable.AcceptChanges()

        GVCiudadesEmp.DataSource = IndiceTotalCiudadDataTable
        GVCiudadesEmp.DataBind()

        WFilas = GVCiudadesEmp.Rows.Count
        For I = 0 To WFilas - 1
            If Val(GVCiudadesEmp.Rows(I).Cells(2).Text) > 0 Then
                WIndice = Val(GVCiudadesEmp.Rows(I).Cells(2).Text) / Val(GVCiudadesEmp.Rows(I).Cells(1).Text) * 100
                GVCiudadesEmp.Rows(I).Cells(3).Text = Format(WIndice, "##0.00")
            Else
                WIndice = 0
                GVCiudadesEmp.Rows(I).Cells(3).Text = Format(WIndice, "##0.00")
            End If
        Next

        xmlCiudades.Append("<chart caption='CIUDADES EMPLEADOS' showToolTip='1' showvalues='1' numdivlines='10' xAxisName='Ciudades' yAxisName='Indice' decimals='1' >")
        For I = 0 To GVCiudadesEmp.Rows.Count - 1
            WCiudad = GVCiudadesEmp.Rows(I).Cells(0).Text
            WMeta = 6.5
            WActual = Val(GVCiudadesEmp.Rows(I).Cells(3).Text)
            WMetaAction = 1.1 * WMeta
            WMetaWatch = WMetaAction
            WMetaBooster = WMeta
            If WActual < WMetaBooster Then
                xmlCiudades.Append("<set Label='" & GVCiudadesEmp.Rows(I).Cells(0).Text & "' value='" & Val(GVCiudadesEmp.Rows(I).Cells(3).Text) & "' color='00FF00' />")
            Else
                If WActual < WMetaWatch Then
                    xmlCiudades.Append("<set Label='" & GVCiudadesEmp.Rows(I).Cells(0).Text & "' value='" & Val(GVCiudadesEmp.Rows(I).Cells(3).Text) & "' color='FFFF10' />")
                Else
                    xmlCiudades.Append("<set Label='" & GVCiudadesEmp.Rows(I).Cells(0).Text & "' value='" & Val(GVCiudadesEmp.Rows(I).Cells(3).Text) & "' color='FF0000' />")
                End If
            End If
        Next
        xmlCiudades.Append("</chart>")
        GraficaCiudadesEmp.Text = FusionCharts.RenderChart("../FusionCharts/Column3D.swf", "", xmlCiudades.ToString(), "chartidb", "600px", "400px", False, True)
        xmlCiudades.Clear()

        '-----CIUDADES CONTRATISTAS
        IndiceTotalCiudadDataTable = IndiceTotalCiudadAdapter.GetData(7, WAño)
        'Agregar columna indice al DataTable
        IndiceTotalCiudadDataTable.Columns.Add("Indice")
        IndiceTotalCiudadDataTable.AcceptChanges()

        GVCiudadesCon.DataSource = IndiceTotalCiudadDataTable
        GVCiudadesCon.DataBind()

        WFilas = GVCiudadesCon.Rows.Count
        For I = 0 To WFilas - 1
            If Val(GVCiudadesCon.Rows(I).Cells(2).Text) > 0 Then
                WIndice = Val(GVCiudadesCon.Rows(I).Cells(2).Text) / Val(GVCiudadesCon.Rows(I).Cells(1).Text) * 100
                GVCiudadesCon.Rows(I).Cells(3).Text = Format(WIndice, "##0.00")
            Else
                WIndice = 0
                GVCiudadesCon.Rows(I).Cells(3).Text = Format(WIndice, "##0.00")
            End If
        Next

        xmlCiudades.Append("<chart caption='CIUDADES CONTRATISTAS' showToolTip='1' showvalues='1' numdivlines='10' xAxisName='Ciudades' yAxisName='Indice' decimals='1' >")
        For I = 0 To GVCiudadesCon.Rows.Count - 1
            WCiudad = GVCiudadesCon.Rows(I).Cells(0).Text
            WMeta = 6.5
            WActual = Val(GVCiudadesCon.Rows(I).Cells(3).Text)
            WMetaAction = 1.1 * WMeta
            WMetaWatch = WMetaAction
            WMetaBooster = WMeta
            If WActual < WMetaBooster Then
                xmlCiudades.Append("<set Label='" & GVCiudadesCon.Rows(I).Cells(0).Text & "' value='" & Val(GVCiudadesCon.Rows(I).Cells(3).Text) & "' color='00FF00' />")
            Else
                If WActual < WMetaWatch Then
                    xmlCiudades.Append("<set Label='" & GVCiudadesCon.Rows(I).Cells(0).Text & "' value='" & Val(GVCiudadesCon.Rows(I).Cells(3).Text) & "' color='FFFF10' />")
                Else
                    xmlCiudades.Append("<set Label='" & GVCiudadesCon.Rows(I).Cells(0).Text & "' value='" & Val(GVCiudadesCon.Rows(I).Cells(3).Text) & "' color='FF0000' />")
                End If
            End If
        Next
        xmlCiudades.Append("</chart>")
        GraficaCiudadesCon.Text = FusionCharts.RenderChart("../FusionCharts/Column3D.swf", "", xmlCiudades.ToString(), "chartidc", "600px", "400px", False, True)
        xmlCiudades.Clear()
    End Sub
    Private Sub CargarDatos()
        Dim IndiceMesCiudadAdapter As New OperacionesTableAdapters.MBO_OPCampoCiudadMesTableAdapter
        Dim IndiceMesCiudadDataTable As New Operaciones.MBO_OPCampoCiudadMesDataTable
        Dim IndiceMesCiudadRow As Operaciones.MBO_OPCampoCiudadMesRow
        IndiceMesCiudadDataTable = IndiceMesCiudadAdapter.GetData(9, WAño)
        'Carga los indices al arreglo
        For Each IndiceMesCiudadRow In IndiceMesCiudadDataTable.Rows
            If IndiceMesCiudadRow.Ciudad = "BOGOTA" Or IndiceMesCiudadRow.Ciudad = "MEDELLIN" Or IndiceMesCiudadRow.Ciudad = "CALI" Or
                IndiceMesCiudadRow.Ciudad = "BARRANQUILLA" Then
                Mes = CInt(IndiceMesCiudadRow.Mes)
                Try
                    If IndiceMesCiudadRow.Errores > 0 Then
                        WIndice = IndiceMesCiudadRow.Errores / IndiceMesCiudadRow.Muestra * 100
                    Else
                        WIndice = 0
                    End If
                Catch
                    WIndice = 0
                End Try

                If IndiceMesCiudadRow.Ciudad = "BOGOTA" Then
                    IndicesMeses(0, 1) = IndiceMesCiudadRow.Ciudad
                    IndicesMeses(Mes, 1) = Str(WIndice)
                Else
                    If IndiceMesCiudadRow.Ciudad = "MEDELLIN" Then
                        IndicesMeses(0, 2) = IndiceMesCiudadRow.Ciudad
                        IndicesMeses(Mes, 2) = Str(WIndice)
                    Else
                        If IndiceMesCiudadRow.Ciudad = "CALI" Then
                            IndicesMeses(0, 3) = IndiceMesCiudadRow.Ciudad
                            IndicesMeses(Mes, 3) = Str(WIndice)
                        Else
                            IndicesMeses(0, 4) = IndiceMesCiudadRow.Ciudad
                            IndicesMeses(Mes, 4) = Str(WIndice)
                        End If
                    End If
                End If
            End If
        Next

    End Sub
    Private Sub GraficarMesCiudad()
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

        xmlIndiceTotalMes.Append("<chart caption='Historico mensual- " & WAño & "' showToolTip='1' showBorder='1' numdivlines='20' yAxisMaxValue='46' formatNumberScale='0' rotatelabels='1' xAxisName='Mes' yAxisName='Indice' showvalues='0' decimals='1' paletteColors='FF0000,0372AB,48FB0D,CC9900'>")
        xmlIndiceTotalMes.Append("<categories>")
        For WMes = 1 To WMesAnterior
            xmlIndiceTotalMes.Append("<category label='" & WNMeses(WMes) & "' />")
        Next WMes
        xmlIndiceTotalMes.Append("</categories>")

        xmlIndiceTotalMes.Append("<dataset seriesName='BOGOTA' >")
        For WMes = 1 To WMesAnterior
            xmlIndiceTotalMes.Append("<set value='" & Trim(IndicesMeses(WMes, 1)) & "' />")
        Next WMes
        xmlIndiceTotalMes.Append("</dataset>")

        xmlIndiceTotalMes.Append("<dataset seriesName='MEDELLIN' >")
        For WMes = 1 To WMesAnterior
            xmlIndiceTotalMes.Append("<set value='" & Trim(IndicesMeses(WMes, 2)) & "' />")
        Next WMes
        xmlIndiceTotalMes.Append("</dataset>")

        xmlIndiceTotalMes.Append("<dataset seriesName='CALI' >")
        For WMes = 1 To WMesAnterior
            xmlIndiceTotalMes.Append("<set value='" & Trim(IndicesMeses(WMes, 3)) & "' />")
        Next WMes
        xmlIndiceTotalMes.Append("</dataset>")

        xmlIndiceTotalMes.Append("<dataset seriesName='BARRANQUILLA' >")
        For WMes = 1 To WMesAnterior
            xmlIndiceTotalMes.Append("<set value='" & Trim(IndicesMeses(WMes, 4)) & "' />")
        Next WMes
        xmlIndiceTotalMes.Append("</dataset>")
        xmlIndiceTotalMes.Append("</chart>")

        GraficaCiudadesHistorico.Text = FusionCharts.RenderChart("../FusionCharts/MSLine.swf", "", xmlIndiceTotalMes.ToString(), "chartid10", "650", "450", False, True)
        xmlIndiceTotalMes.Clear()

    End Sub

End Class