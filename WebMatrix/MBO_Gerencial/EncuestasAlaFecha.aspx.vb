Imports System.Math
Public Class EncuestasAlaFecha
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim WEncuestas As Integer
        Dim WAño As Integer
        Dim WMes As Integer

        WAño = 2012
        WMes = 5

        Dim MBO_CampoEncuestasAlaFechaAdapter As New OperacionesTableAdapters.MBO_OPCampoEncuestasAlaFechaTableAdapter
        Dim MBO_CampoEncuestasAlaFechaDataTable As New Operaciones.MBO_OPCampoEncuestasAlaFechaDataTable
        Dim MBO_CampoEncuestasAlaFecha As Operaciones.MBO_OPCampoEncuestasAlaFechaRow
        MBO_CampoEncuestasAlaFechaDataTable = MBO_CampoEncuestasAlaFechaAdapter.GetData(WAño, WMes)
        MBO_CampoEncuestasAlaFecha = MBO_CampoEncuestasAlaFechaDataTable.Rows(0)

        WEncuestas = MBO_CampoEncuestasAlaFecha.Encuestas
        Encuestas.Text = CType(WEncuestas, String)
    End Sub

End Class