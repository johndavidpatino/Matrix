Public Class CampoEncuestadores
    Inherits System.Web.UI.Page
    Dim WAño As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim WTipo As Integer

        If IsPostBack Then
            WTipo = Tipos.SelectedValue
            Session.Add("WTipo", WTipo)
        Else
            WAño = Year(Now())
            Session.Add("WAño", WAño)

            WTipo = 9
            Tipos.SelectedValue = 9
            Session.Add("WTipo", WTipo)

            GVEncuestadores.PageIndex = 0
        End If
    End Sub
    Private Sub GVEncuestadores_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVEncuestadores.PageIndexChanging
        GVEncuestadores.PageIndex = e.NewPageIndex
    End Sub
    Private Sub GVEncuestadores_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVEncuestadores.RowDataBound
        Dim row As GridViewRow
        Dim t1 As TextBox
        Dim WErrores As String

        row = e.Row
        If e.Row.RowType = DataControlRowType.DataRow Then
            t1 = e.Row.FindControl("Indice1")
            WErrores = row.Cells(6).Text.Replace("&nbsp;", "")
            If String.IsNullOrWhiteSpace(WErrores) = True Then
                t1.Text = CStr(0)
            Else
                t1.Text = CStr(Format((CDec(e.Row.Cells(6).Text) / CDec(e.Row.Cells(5).Text)) * 100, "##0.00"))
            End If
            e.Row.Cells(1).Width = 10
            e.Row.Cells(2).Width = 220
        End If

    End Sub

    Private Sub GVEncuestadores_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GVEncuestadores.SelectedIndexChanged
        Dim WEncuestador As Decimal

        WEncuestador = GVEncuestadores.SelectedValue
        Session.Add("WEncuestador", WEncuestador)

    End Sub

    Private Sub GVEncuestadorMeses_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVEncuestadorMeses.RowDataBound
        Dim row As GridViewRow
        Dim t1 As TextBox
        Dim WErrores As String

        row = e.Row
        If e.Row.RowType = DataControlRowType.DataRow Then
            t1 = e.Row.FindControl("Indice2")
            WErrores = row.Cells(2).Text.Replace("&nbsp;", "")
            If String.IsNullOrWhiteSpace(WErrores) = True Then
                t1.Text = CStr(0)
            Else
                t1.Text = CStr(Format((CDec(e.Row.Cells(2).Text) / CDec(e.Row.Cells(1).Text)) * 100, "##0.00"))
            End If
        End If
    End Sub
End Class