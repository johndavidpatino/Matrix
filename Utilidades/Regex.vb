Public Class Regex
    Public Shared Function DireccionIP(ByVal s As String) As Boolean
        Return System.Text.RegularExpressions.Regex.IsMatch(s, "^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\z")
    End Function
    Public Shared Function OID(ByVal s As String) As Boolean
        Return System.Text.RegularExpressions.Regex.IsMatch(s, "^(\d+)((\.)\d+)*\z")
    End Function
    Public Shared Function Entero(ByVal s As String) As Boolean
        Return System.Text.RegularExpressions.Regex.IsMatch(s, "^\d+\z")
    End Function
    Public Shared Function Real(ByVal s As String, ByVal separadorDecimal As String) As Boolean
        Return System.Text.RegularExpressions.Regex.IsMatch(s, "^(\d+)" + separadorDecimal + "(\d+)\z")
    End Function
End Class
