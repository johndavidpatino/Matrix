Imports InfosoftGlobal
Public Class AOTPorGerentes
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim WAño As Integer
        Dim WMesI As Integer
        Dim WMesF As Integer
        Dim WUnidad As String

        WMesI = 1
        WAño = Session("Año")
        WMesF = Session("MesActual")
        WUnidad = Session("Unidad")

        Dim AOTUnidadGerenteAdapter As New GerencialTableAdapters.MBO_PGAOTPorUnidadGerenteTableAdapter
        Dim AOTUnidadGerenteDataTable As New Gerencial.MBO_PGAOTPorUnidadGerenteDataTable
        Dim AOTUnidadGerenteRow As Gerencial.MBO_PGAOTPorUnidadGerenteRow
        AOTUnidadGerenteDataTable = AOTUnidadGerenteAdapter.GetData(WAño, WMesI, WMesF, WUnidad)

        Dim xmlGerentes As New StringBuilder()
        xmlGerentes.Append("<chart caption='ACUMULADO' showToolTip='1' showvalues='1' numdivlines='10' xAxisName='Gerentes' yAxisName='AOT' decimals='0' >")
        For Each AOTUnidadGerenteRow In AOTUnidadGerenteDataTable.Rows
            xmlGerentes.Append("<set Label='" & AOTUnidadGerenteRow.Gerente & "' value='" & Trim(Str(AOTUnidadGerenteRow.Valor)) & "' />")
        Next
        xmlGerentes.Append("</chart>")
        AOTGerentesAcumulado.Text = FusionCharts.RenderChart("../FusionCharts/Bar2D.swf", "", xmlGerentes.ToString(), "chartidx", "100%", "500px", False, True)
        xmlGerentes.Clear()


        AOTUnidadGerenteDataTable = AOTUnidadGerenteAdapter.GetData(WAño, WMesF, WMesF, WUnidad)

        xmlGerentes.Append("<chart caption='MES ACTUAL' showToolTip='1' showvalues='1' numdivlines='10' xAxisName='Gerentes' yAxisName='AOT' decimals='0' >")
        For Each AOTUnidadGerenteRow In AOTUnidadGerenteDataTable.Rows
            xmlGerentes.Append("<set Label='" & AOTUnidadGerenteRow.Gerente & "' value='" & Trim(Str(AOTUnidadGerenteRow.Valor)) & "' />")
        Next
        xmlGerentes.Append("</chart>")
        AOTGerentesMesActual.Text = FusionCharts.RenderChart("../FusionCharts/Bar2D.swf", "", xmlGerentes.ToString(), "chartidy", "100%", "500px", False, True)
        xmlGerentes.Clear()

    End Sub

End Class