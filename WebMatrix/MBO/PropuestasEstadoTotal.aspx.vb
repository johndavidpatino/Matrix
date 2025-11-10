Imports InfosoftGlobal
Public Class PropuestasEstadoTotal
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim WPorcentaje As Decimal
        Dim WMaxpropuestas As Integer
        Dim CreadasEnviadasAdapter As New GerencialTableAdapters.MBO_PropuestasCreadasEnviadasSinAnuncioActualizarTableAdapter
        Dim CreadasEnviadasDataTable As New Gerencial.MBO_PropuestasCreadasEnviadasSinAnuncioActualizarDataTable
        Dim CreadasEnviadasRow As Gerencial.MBO_PropuestasCreadasEnviadasSinAnuncioActualizarRow
        CreadasEnviadasDataTable = CreadasEnviadasAdapter.GetData("9")  'Lee todas las unidades

        'CALCULAR MAXIMO DE PROPUESTAS
        For Each CreadasEnviadasRow In CreadasEnviadasDataTable.Rows
            If WMaxpropuestas < CreadasEnviadasRow.PropuestasEnGestion Then
                WMaxpropuestas = CreadasEnviadasRow.PropuestasEnGestion
            End If
        Next

        Dim xmlPropuestas As New StringBuilder()

        xmlPropuestas.Append("<chart caption='Propuestas creadas / enviadas' showvalues='1' yAxisMaxValue='" & Str(WMaxpropuestas) & "' numdivlines='5' xAxisName='Unidades' yAxisName='Cantidad' >")
        xmlPropuestas.Append("<categories>")
        For Each CreadasEnviadasRow In CreadasEnviadasDataTable.Rows
            xmlPropuestas.Append("<category label='" & CreadasEnviadasRow.GrupoUnidad & "'/>")
        Next
        xmlPropuestas.Append("</categories>")

        xmlPropuestas.Append("<dataset seriesName='Creadas y enviadas' color='00FF00'>")
        For Each CreadasEnviadasRow In CreadasEnviadasDataTable.Rows
            xmlPropuestas.Append("<set value='" & Str(CreadasEnviadasRow.PropuestasEnGestion) & "' color='00FF00' />")
        Next
        xmlPropuestas.Append("</dataset>")

        xmlPropuestas.Append("<dataset seriesName='Por actualizar' color='FF0000'>")
        For Each CreadasEnviadasRow In CreadasEnviadasDataTable.Rows
            WPorcentaje = Format(CreadasEnviadasRow.PropuestasPorActualizar / CreadasEnviadasRow.PropuestasEnGestion * 100, "##0.00")
            xmlPropuestas.Append("<set value='" & Str(CreadasEnviadasRow.PropuestasPorActualizar) & "' tooltext= '" & Str(CreadasEnviadasRow.PropuestasPorActualizar) & "{br}" & Str(WPorcentaje) & "%" & "' color='FF0000' />")
        Next
        xmlPropuestas.Append("</dataset>")

        xmlPropuestas.Append("</chart>")

        CreadasEnviadas.Text = FusionCharts.RenderChart("../FusionCharts/MSColumn3D.swf", "", xmlPropuestas.ToString(), "chartid1", "500px", "400px", False, True)
        xmlPropuestas.Clear()

        'ALTA PROBABILIDAD SIN ACTUALIZAR
        Dim AltaProbabilidadAdapter As New GerencialTableAdapters.MBO_PropuestasAltaProbabilidadPorActualizarTableAdapter
        Dim AltaProbabilidadDataTable As New Gerencial.MBO_PropuestasAltaProbabilidadPorActualizarDataTable
        Dim AltaProbabilidadRow As Gerencial.MBO_PropuestasAltaProbabilidadPorActualizarRow
        AltaProbabilidadDataTable = AltaProbabilidadAdapter.GetData("9")

        xmlPropuestas.Append("<chart caption='Propuestas ALTA PROBABILIDAD' showvalues='1' yAxisMaxValue='" & Str(WMaxpropuestas) & "' numdivlines='2' xAxisName='Unidades' yAxisName='Cantidad' >")
        xmlPropuestas.Append("<categories>")
        For Each AltaProbabilidadRow In AltaProbabilidadDataTable.Rows
            xmlPropuestas.Append("<category label='" & AltaProbabilidadRow.GrupoUnidad & "'/>")
        Next
        xmlPropuestas.Append("</categories>")

        xmlPropuestas.Append("<dataset seriesName='Alta probabilidad' color='00FF00'>")
        For Each AltaProbabilidadRow In AltaProbabilidadDataTable.Rows
            xmlPropuestas.Append("<set value='" & Str(AltaProbabilidadRow.TPropuestas) & "' color='00FF00' />")
        Next
        xmlPropuestas.Append("</dataset>")

        xmlPropuestas.Append("<dataset seriesName='Por actualizar' color='FF0000'>")
        For Each AltaProbabilidadRow In AltaProbabilidadDataTable.Rows
            WPorcentaje = Format(AltaProbabilidadRow.NSinActualizar / AltaProbabilidadRow.TPropuestas * 100, "##0.00")
            xmlPropuestas.Append("<set value='" & Str(AltaProbabilidadRow.NSinActualizar) & "' tooltext= '" & Str(AltaProbabilidadRow.NSinActualizar) & "{br}" & Str(WPorcentaje) & "%" & "' color='FF0000' />")
        Next
        xmlPropuestas.Append("</dataset>")

        xmlPropuestas.Append("</chart>")

        AltaProbabilidad.Text = FusionCharts.RenderChart("../FusionCharts/MSColumn3D.swf", "", xmlPropuestas.ToString(), "chartid2", "500px", "400px", False, True)
        xmlPropuestas.Clear()

        'CREADAS ENVIADAS POR GC
        Dim WGerente As String
        Dim CreadasEnviadasGCAdapter As New GerencialTableAdapters.MBO_PropuestasCreadasEnviadasSinAnuncioGCTableAdapter
        Dim CreadasEnviadasGCDataTable As New Gerencial.MBO_PropuestasCreadasEnviadasSinAnuncioGCDataTable
        Dim CreadasEnviadasGCRow As Gerencial.MBO_PropuestasCreadasEnviadasSinAnuncioGCRow
        CreadasEnviadasGCDataTable = CreadasEnviadasGCAdapter.GetData("9")

        'CALCULAR MAXIMO DE PROPUESTAS
        WMaxpropuestas = 0
        For Each CreadasEnviadasGCRow In CreadasEnviadasGCDataTable.Rows
            If WMaxpropuestas < CreadasEnviadasGCRow.PropuestasEnGestion Then
                WMaxpropuestas = CreadasEnviadasGCRow.PropuestasEnGestion
            End If
        Next

        xmlPropuestas.Append("<chart caption='Creadas / enviadas por Gerente de cuentas' showvalues='1' yAxisMaxValue='" & Str(WMaxpropuestas) & "' numdivlines='15' xAxisName='Cantidad' yAxisName='Gerentes' >")
        xmlPropuestas.Append("<categories>")
        For Each CreadasEnviadasGCRow In CreadasEnviadasGCDataTable.Rows
            WGerente = CreadasEnviadasGCRow.Nombres & " " & CreadasEnviadasGCRow.Apellidos
            xmlPropuestas.Append("<category label='" & WGerente & "'/>")
        Next
        xmlPropuestas.Append("</categories>")

        xmlPropuestas.Append("<dataset seriesName='Creadas y enviadas' color='00FF00'>")
        For Each CreadasEnviadasGCRow In CreadasEnviadasGCDataTable.Rows
            xmlPropuestas.Append("<set value='" & Str(CreadasEnviadasGCRow.PropuestasEnGestion) & "' color='00FF00' />")
        Next
        xmlPropuestas.Append("</dataset>")

        xmlPropuestas.Append("<dataset seriesName='Por actualizar' color='FF0000'>")
        For Each CreadasEnviadasGCRow In CreadasEnviadasGCDataTable.Rows
            WPorcentaje = Format(CreadasEnviadasGCRow.WPorcentaje, "##0.00")
            xmlPropuestas.Append("<set value='" & Str(CreadasEnviadasGCRow.PropuestasPorActualizar) & "' tooltext= '" & Str(CreadasEnviadasGCRow.PropuestasPorActualizar) & "{br}" & Str(WPorcentaje) & "%" & "' color='FF0000' />")
        Next
        xmlPropuestas.Append("</dataset>")

        xmlPropuestas.Append("</chart>")

        CreadasEnviadasGC.Text = FusionCharts.RenderChart("../FusionCharts/MSBar3D.swf", "", xmlPropuestas.ToString(), "chartid3", "1100px", "1100px", False, True)
        xmlPropuestas.Clear()

    End Sub

End Class