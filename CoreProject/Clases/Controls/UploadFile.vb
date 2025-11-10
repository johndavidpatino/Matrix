'Imports CoreProject.GD_Model

Public Class UploadFile

    Public Function uploadFile(ByVal nomDoc As String, ByVal url As String,
                                    ByVal docId As Integer, ByVal comentario As String,
                                    ByVal usuarioId As Integer, ByVal idContenedor As Integer) As Integer

        Dim uploadfile_ As New GD_Entities
        Try
            Try
                Return uploadfile_.uploadFile(nomDoc, url, docId, comentario, usuarioId, idContenedor)
            Catch ex As Exception
                Throw ex
            End Try
        Catch ex As Exception
            If (IsNothing(ex.InnerException)) Then
                Throw ex
            Else
                Throw (ex.InnerException)
            End If
        End Try

    End Function
End Class
