Imports System.Net
Imports Newtonsoft.Json
Namespace ExternalServices.BIServices
    Public Class RequestService
        Private ReadOnly _URLBase As String
        Sub New(URLBase As String)
            _URLBase = URLBase
        End Sub
        Public Function RequestPost(urlPart As String, body As Object, Optional token As String = "") As String
            Dim serialisedData = JsonConvert.SerializeObject(body)
            Dim client = New System.Net.WebClient()

            Dim response As String
            ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)
            client.Headers(HttpRequestHeader.ContentType) = "application/json"
            'client.Headers.Add("user-agent", "Mozilla / 5.0(Windows NT 6.1; WOW32; Trident / 7.0; rv: 11.0) like Gecko")
            'client.Headers.Add("Accept-Encoding", "gzip, deflate, br")
            If Not String.IsNullOrEmpty(token) Then
                client.Headers(HttpRequestHeader.Authorization) = "Bearer " + token
            End If

            response = client.UploadString(_URLBase + urlPart, "POST", serialisedData)
            Return response

        End Function
        Public Function RequestPut(urlPart As String, body As Object, token As String) As String
            Dim serialisedData = JsonConvert.SerializeObject(body)
            Dim client = New System.Net.WebClient()
            Dim response As String
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
            client.Headers(HttpRequestHeader.ContentType) = "application/json"
            client.Headers(HttpRequestHeader.Authorization) = "Bearer " + token
            'client.Headers.Add("user-agent", "Mozilla / 5.0(Windows NT 6.1; WOW32; Trident / 7.0; rv: 11.0) like Gecko")
            'client.Headers.Add("Accept-Encoding", "gzip, deflate, br")

            response = client.UploadString(_URLBase + urlPart, "PUT", serialisedData)

            Return response

        End Function
        Public Function RequestGet(urlPart As String, token As String) As String
            Dim client = New System.Net.WebClient()
            Dim response As String
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
            client.Headers(HttpRequestHeader.ContentType) = "application/json"

            If Not String.IsNullOrEmpty(token) Then
                client.Headers(HttpRequestHeader.Authorization) = "Bearer " + token
            End If

            'client.Headers.Add("user-agent", "Mozilla / 5.0(Windows NT 6.1; WOW32; Trident / 7.0; rv: 11.0) like Gecko")
            'client.Headers.Add("Accept-Encoding", "gzip, deflate, br")

            response = client.DownloadString(_URLBase + urlPart)
            Return response

        End Function
        Public Function RequestDelete(urlPart As String, token As String) As String
            Dim client = New System.Net.WebClient()
            Dim response As String
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
            client.Headers(HttpRequestHeader.ContentType) = "application/json"
            client.Headers(HttpRequestHeader.Authorization) = "Bearer " + token
            'client.Headers.Add("user-agent", "Mozilla / 5.0(Windows NT 6.1; WOW32; Trident / 7.0; rv: 11.0) like Gecko")
            'client.Headers.Add("Accept-Encoding", "gzip, deflate, br")

            response = client.UploadString(_URLBase + urlPart, "DELETE", "")
            Return response

        End Function
    End Class
End Namespace