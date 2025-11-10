Public Class DialogMaster
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Header.DataBind()
        If Session("IDUsuario") = Nothing Then
            Response.Redirect("../Default.aspx")
        Else
            Session("IDUsuario") = Session("IDUsuario").ToString
        End If
    End Sub

End Class