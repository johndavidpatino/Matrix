Public NotInheritable Class Notificacion

    Public Shared Sub ShowNotification(ByVal page As System.Web.UI.Page, ByVal message As String, ByVal ctr As Control, Optional ByVal sc As ScriptManager = Nothing)

        If (sc Is Nothing) Then
            page.ClientScript.RegisterStartupScript((GetType(Page)), "notificationScript", "<script> $(function () {$.notifyBar({html: '" & message & "',delay: 10000,animationSpeed: 'normal',close: true});});</script>")
        Else
            ScriptManager.RegisterStartupScript(ctr, GetType(Page), "notificationScript", "<script> $(function () {$.notifyBar({html: '" & message & "',delay: 10000,animationSpeed: 'normal',close: true});});</script>", False)
        End If


    End Sub
End Class
