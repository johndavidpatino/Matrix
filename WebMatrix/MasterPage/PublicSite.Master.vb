Public Class PublicSite
    Inherits System.Web.UI.MasterPage

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Page.Header.DataBind()
    End Sub
End Class