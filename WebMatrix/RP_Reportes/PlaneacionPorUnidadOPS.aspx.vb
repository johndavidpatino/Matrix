Public Class PlaneacionPorUnidadOPS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnMostrar_Click(sender As Object, e As EventArgs) Handles btnMostrar.Click
        Me.WebChartControl1.DataBind()
    End Sub
End Class