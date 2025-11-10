Public Class Site1
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Header.DataBind()
        If Session("IDUsuario") = Nothing Then
            Response.Redirect("../Default.aspx")
        Else
            Session("IDUsuario") = Session("IDUsuario").ToString
        End If
    End Sub

	Private Sub LoginStatus1_LoggingOut(sender As Object, e As LoginCancelEventArgs) Handles LoginStatus1.LoggingOut
		Session("IDUsuario") = Nothing
		Session("TrabajoId") = Nothing
	End Sub
End Class