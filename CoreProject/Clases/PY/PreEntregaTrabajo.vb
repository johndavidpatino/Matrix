
'Imports CoreProject.PY_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class PreEntregaTrabajo
#Region "Variables Globales"
    Private oMatrixContext As PY_Entities
#End Region
#Region "Constructors"
    Public Sub New()
        oMatrixContext = New PY_Entities
    End Sub
#End Region

#Region "Obtener"
    Private Function GetPreEntregaTrabajo(ByVal TrabajoId As Long) As List(Of PY_Trabajo_PreEntrega_Result)
        Try
            Dim oResult As ObjectResult(Of PY_Trabajo_PreEntrega_Result) = oMatrixContext.PY_Trabajo_PreEntrega(TrabajoId)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw (ex)
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverPreEntregaxTrabajo(ByVal TrabajoId As Long)
        Try
            Dim oResult As List(Of PY_Trabajo_PreEntrega_Result)
            oResult = GetPreEntregaTrabajo(TrabajoId)
            If oResult.Count = 0 Then
                Return Nothing
            Else
                Return oResult.Item(0)
            End If
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
#End Region
End Class
