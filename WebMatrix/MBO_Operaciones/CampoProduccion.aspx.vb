Imports System.Math
Imports InfosoftGlobal
Public Class CampoProduccion
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim I As Integer
        Dim WAño As Integer
        Dim WMes As Integer

        WAño = 2012
        WMes = 12

        'TRAER DATOS ENCUESTAS TOTAL
        Dim MBO_EncuestasAprobadasAdapter As New OperacionesTableAdapters.MBO_CUEncuestasAprobadasTableAdapter
        Dim MBO_EncuestasAprobadasDataTable As New Operaciones.MBO_CUEncuestasAprobadasDataTable
        Dim MBO_EncuestasAprobadas As Operaciones.MBO_CUEncuestasAprobadasRow
        MBO_EncuestasAprobadasDataTable = MBO_EncuestasAprobadasAdapter.GetData(WAño, WMes)
        MBO_EncuestasAprobadas = MBO_EncuestasAprobadasDataTable.Rows(0)

        Dim MBO_CampoEncuestasAlaFechaAdapter As New OperacionesTableAdapters.MBO_OPCampoEncuestasAlaFechaTableAdapter
        Dim MBO_CampoEncuestasAlaFechaDataTable As New Operaciones.MBO_OPCampoEncuestasAlaFechaDataTable
        Dim MBO_CampoEncuestasAlaFecha As Operaciones.MBO_OPCampoEncuestasAlaFechaRow
        MBO_CampoEncuestasAlaFechaDataTable = MBO_CampoEncuestasAlaFechaAdapter.GetData(WAño, WMes)
        MBO_CampoEncuestasAlaFecha = MBO_CampoEncuestasAlaFechaDataTable.Rows(0)

        'ENCUESTAS APROBADAS vs ENCUESTAS REALIZADAS TOTAL
        Dim xmlEncuestas As New StringBuilder()
        'Generate the chart element
        xmlEncuestas.Append("<chart dataStreamURL='EncuestasAlaFecha.aspx' refreshInterval='3' palette='1' caption= 'Encuestas por realizar total' ticksOnRight='0' lowerLimit='0' upperLimit='" & Val(MBO_EncuestasAprobadas.Encuestas) & "'  bgColor='FFFFFF' >")
        xmlEncuestas.Append("<value>" & Val(MBO_CampoEncuestasAlaFecha.Encuestas) & "</value>")
        xmlEncuestas.Append("</chart>")
        EncAHoyTotal.Text = FusionCharts.RenderChart("../FusionWidgets/Cylinder.swf", "", xmlEncuestas.ToString(), "chartid30", "300", "400", False, True)
        xmlEncuestas.Clear()

        'TRAER DATOS ENCUESTAS POR GRUPO
        Dim MBO_EncuestasAprobadasPorMetodologiaAdapter As New OperacionesTableAdapters.MBO_CUEncuestasAprobadasPorMetodologiaTableAdapter
        Dim MBO_EncuestasAprobadasPorMetodologiaDataTable As New Operaciones.MBO_CUEncuestasAprobadasPorMetodologiaDataTable
        Dim MBO_EncuestasAprobadasPorMetodologia As Operaciones.MBO_CUEncuestasAprobadasPorMetodologiaRow
        MBO_EncuestasAprobadasPorMetodologiaDataTable = MBO_EncuestasAprobadasPorMetodologiaAdapter.GetData(WAño, WMes)

        Dim MBO_EncuestasAlaFechaPorMetodologiaAdapter As New OperacionesTableAdapters.MBO_OPCampoEncuestasAlaFechaPorMetodologiaTableAdapter
        Dim MBO_EncuestasAlaFechaPorMetodologiaDataTable As New Operaciones.MBO_OPCampoEncuestasAlaFechaPorMetodologiaDataTable
        Dim MBO_EncuestasAlaFechaPorMetodologia As Operaciones.MBO_OPCampoEncuestasAlaFechaPorMetodologiaRow
        MBO_EncuestasAlaFechaPorMetodologiaDataTable = MBO_EncuestasAlaFechaPorMetodologiaAdapter.GetData(WAño, WMes)
        Dim WEncuestasAlaFecha As Integer
        For I = 0 To MBO_EncuestasAprobadasPorMetodologiaDataTable.Rows.Count - 1
            MBO_EncuestasAprobadasPorMetodologia = MBO_EncuestasAprobadasPorMetodologiaDataTable.Rows(I)
            WEncuestasAlaFecha = 0
            For X = 0 To MBO_EncuestasAlaFechaPorMetodologiaDataTable.Rows.Count - 1
                MBO_EncuestasAlaFechaPorMetodologia = MBO_EncuestasAlaFechaPorMetodologiaDataTable.Rows(X)
                If MBO_EncuestasAprobadasPorMetodologia.GrupoInforme = MBO_EncuestasAlaFechaPorMetodologia.GrupoInforme Then
                    WEncuestasAlaFecha = MBO_EncuestasAlaFechaPorMetodologia.Encuestas
                    Exit For
                End If
            Next
            xmlEncuestas.Append("<chart palette='1' caption= '" & MBO_EncuestasAprobadasPorMetodologia.Metodologia & "' ticksOnRight='0' lowerLimit='0' upperLimit='" & Val(MBO_EncuestasAprobadasPorMetodologia.Encuestas) & "'  bgColor='FFFFFF' >")
            xmlEncuestas.Append("<value>" & WEncuestasAlaFecha & "</value>")
            xmlEncuestas.Append("</chart>")

            Select Case MBO_EncuestasAprobadasPorMetodologia.GrupoInforme
                Case 100 : EncAHoyCARACARAHI.Text = FusionCharts.RenderChart("../FusionWidgets/Cylinder.swf", "", xmlEncuestas.ToString(), "chartid31", "200", "250", False, True)
                Case 110 : EncAHoyCARACARAEP.Text = FusionCharts.RenderChart("../FusionWidgets/Cylinder.swf", "", xmlEncuestas.ToString(), "chartid32", "200", "250", False, True)
                Case 120 : EncAHoyCARACARATK.Text = FusionCharts.RenderChart("../FusionWidgets/Cylinder.swf", "", xmlEncuestas.ToString(), "chartid33", "200", "250", False, True)
                Case 130 : EncAHoyCARACARALC.Text = FusionCharts.RenderChart("../FusionWidgets/Cylinder.swf", "", xmlEncuestas.ToString(), "chartid34", "200", "250", False, True)
                Case 199 : EncAHoyCARACARAOT.Text = FusionCharts.RenderChart("../FusionWidgets/Cylinder.swf", "", xmlEncuestas.ToString(), "chartid35", "200", "250", False, True)
                Case 200 : EncAHoyCATI.Text = FusionCharts.RenderChart("../FusionWidgets/Cylinder.swf", "", xmlEncuestas.ToString(), "chartid36", "300", "400", False, True)
            End Select
            xmlEncuestas.Clear()
        Next

    End Sub

End Class