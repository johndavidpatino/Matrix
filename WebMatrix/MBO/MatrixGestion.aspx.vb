Imports InfosoftGlobal
Public Class MatrixGestion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim GestionTotalAdapter As New GerencialTableAdapters.MBO_PGGestionMatrixTableAdapter
        Dim GestionTotalDataTable As New Gerencial.MBO_PGGestionMatrixDataTable
        Dim GestionTotalRow As Gerencial.MBO_PGGestionMatrixRow
        GestionTotalDataTable = GestionTotalAdapter.GetData()
        Dim xmlGestion As New StringBuilder()
        For Each GestionTotalRow In GestionTotalDataTable.Rows
            xmlGestion.Append("<chart caption='Transacciones Matrix' showToolTip='1' showvalues='1' numdivlines='10' xAxisName='Etapas' yAxisName='Cantidad' >")
            xmlGestion.Append("<set Label='Briefs' value='" & Str(GestionTotalRow.Brief) & "' color='00FF00' />")
            xmlGestion.Append("<set Label='Propuestas' value='" & Str(GestionTotalRow.Propuestas) & "' color='FFFF10' />")
            xmlGestion.Append("<set Label='Presupuestos' value='" & Str(GestionTotalRow.Presupuestos) & "' color='FF0000' />")
            xmlGestion.Append("<set Label='Estudios' value='" & Str(GestionTotalRow.Estudios) & "' color='FF0000' />")
            xmlGestion.Append("<set Label='Proyectos' value='" & Str(GestionTotalRow.Proyectos) & "' color='00FF00' />")
            xmlGestion.Append("<set Label='Trabajos' value='" & Str(GestionTotalRow.Trabajos) & "' color='FFFF10' />")
        Next
        xmlGestion.Append("</chart>")
        MatrixGestionTotal.Text = FusionCharts.RenderChart("../FusionCharts/Column3D.swf", "", xmlGestion.ToString(), "chartid1", "600px", "400px", False, True)
        xmlGestion.Clear()

    End Sub

End Class