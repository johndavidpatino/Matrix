Public Class PropuestasSinTrabajo
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then

        Else
            Session.Add("WUnidad", "9")
            Session.Add("WUnaUnidad", 0)
            Session.Add("WUnidadMetodo", "NA")
        End If

    End Sub
    Private Sub GVPropuestasPorUnidad_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GVPropuestasPorUnidad.SelectedIndexChanged
        Dim WUnidad As String
        WUnidad = GVPropuestasPorUnidad.SelectedValue
        Session.Add("WUnidad", WUnidad)

        Session.Add("WUnaUnidad", 1)
        Session.Add("WUnidadMetodo", WUnidad)
    End Sub
    Private Sub GVPropuestasRelacion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVPropuestasRelacion.RowDataBound
        Dim row As GridViewRow
        Dim WFecha As Date

        row = e.Row
        If e.Row.RowType = DataControlRowType.DataRow Then
            WFecha = CDate(e.Row.Cells(5).Text)
            If WFecha < Now() Then
                e.Row.ForeColor = Drawing.Color.Red
            End If
            'e.Row.Cells(9).Text = String.Format("{0:N0}", CDbl(e.Row.Cells(9).Text))
        End If
    End Sub
    Private Sub GVPropuestasRelacion_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVPropuestasRelacion.PageIndexChanging
        GVPropuestasRelacion.PageIndex = e.NewPageIndex
    End Sub
    Private Sub GVPropuestasMetodologia_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVPropuestasMetodologia.RowDataBound
        Dim row As GridViewRow

        row = e.Row
        If e.Row.RowType = DataControlRowType.DataRow Then
            GVPropuestasMetodologia.Columns(0).ItemStyle.Width = 100
            GVPropuestasMetodologia.Columns(0).ItemStyle.Wrap = False
        End If
    End Sub
    Private Sub btTotales_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btTotales.Click
        Session.Add("WUnidad", "9")
        Session.Add("WUnaUnidad", 0)
        Session.Add("WUnidadMetodo", "NA")
        'GVPropuestasPorUnidad.SelectedRowStyle.BackColor = Drawing.Color.White
    End Sub
End Class