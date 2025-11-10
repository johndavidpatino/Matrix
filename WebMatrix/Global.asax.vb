Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
        If Me.Request.Url.Authority = "LatCoBoApp8" Then
            Response.Redirect("http://AmCoBogApp1")
        End If
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

    Private Sub Global_asax_BeginRequest(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.BeginRequest
        If (Request.AppRelativeCurrentExecutionFilePath.ToLower = "~/".ToLower) Then
            HttpContext.Current.RewritePath("/Default.aspx")
        End If
    End Sub
End Class