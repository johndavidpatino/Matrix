Public Class CampoErroresUnEstudio
    Inherits System.Web.UI.Page
    Dim WEstudio1 As Decimal
    Dim ErroresTotalAdapter As New OperacionesTableAdapters.MBO_OPCampoErroresUnEstudioTableAdapter
    Dim ErroresTotalDataTable As New Operaciones.MBO_OPCampoErroresUnEstudioDataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim I As Integer

        Dim TrabajosAdapter As New OperacionesTableAdapters.MBO_TrabajosConErroresTableAdapter
        Dim TrabajosDataTable As New Operaciones.MBO_TrabajosConErroresDataTable
        Dim TrabajosRow As Operaciones.MBO_TrabajosConErroresRow
        TrabajosDataTable = TrabajosAdapter.GetData()
        
        If IsPostBack Then
            WEstudio1 = Trabajos.SelectedValue

            Session.Add("WEstudio", WEstudio1)
        Else
            'Carga el combo
            'Trabajos.items.Clear()
            Trabajos.Items.Add(New ListItem("Seleccione trabajo", 0))
            I = 0
            For Each TrabajosRow In TrabajosDataTable.Rows
                I = I + 1
                Trabajos.Items.Add(New ListItem(TrabajosRow.Estudio & "-" & TrabajosRow.NombreTrabajo, TrabajosRow.Estudio))
                Trabajos.Items(I).Value = TrabajosRow.Estudio
            Next
            WEstudio1 = Trabajos.SelectedValue

            Session.Add("WEstudio", WEstudio1)
            DGErrores.PageIndex = 1

        End If
    End Sub
    Private Sub DGErrores_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DGErrores.PageIndexChanging
        DGErrores.PageIndex = e.NewPageIndex
    End Sub
    Private Sub DGErrores_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles DGErrores.RowDataBound
        'DGErrores.Columns(0).Visible = False
    End Sub
End Class