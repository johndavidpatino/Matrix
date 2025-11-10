Imports Newtonsoft.Json

Public Class HTMLToPDFGenerator
    Public Function Convert(HTMLTextToConvert As String) As String
        Dim htmlTemplateBytes = Encoding.UTF8.GetBytes(HTMLTextToConvert)
        Dim htmlTemplateBase64 = System.Convert.ToBase64String(htmlTemplateBytes)
        Dim URL = ConfigurationManager.AppSettings("URLHTMLToPDFGenerator").ToString

        Dim serialisedData = JsonConvert.SerializeObject(New With {.HTMLBase64String = htmlTemplateBase64})

        Dim httpeClient = New Net.WebClient()

        httpeClient.Headers(Net.HttpRequestHeader.ContentType) = "application/json"

        Dim responseApi = httpeClient.UploadString(URL, "POST", serialisedData)

        Dim responseDictionary = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(responseApi)

        Dim pdfBase64 = responseDictionary.Item("pdfBase64String")
        Return pdfBase64
    End Function

End Class
