

'Imports CoreProject.US_Model
Imports System.Data.Entity.Core.Objects
Imports System.Data.SqlClient

<Serializable()>
Public Class Validacion
#Region "Variables Globales"
    Private oMatrixContext As US_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New US_Entities
    End Sub
#End Region
#Region "Obtener"
    Public Function ValidarPermiso(ByVal Permiso As String, ByVal UsuarioID As Integer) As Boolean
        Try
            Try

                Dim List As ObjectResult(Of US_PermisosxIdUsuario_Get_Result) = oMatrixContext.US_PermisosxIdUsuario_Get(Permiso, UsuarioID)
                If List.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As SqlException
                Throw ex
            End Try
        Catch ex As Exception
            If IsNothing(ex.InnerException) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
#End Region
End Class
