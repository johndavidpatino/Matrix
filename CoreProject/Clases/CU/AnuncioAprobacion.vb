Imports System.Data.Entity.Core.Objects

'Imports CoreProject.CU_Model

<Serializable()>
Public Class AnuncioAprobacion

#Region "Variables Globales"
    Private oMatrixContext As CU_Entities
#End Region
#Region "Constructors"
    Public Sub New()
        oMatrixContext = New CU_Entities
    End Sub
#End Region

#Region "Obtener"
    Private Function GetAnuncioAprobacion(ByVal EstudioId As Long) As List(Of CU_AnuncioAprobacion_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_AnuncioAprobacion_Get_Result) = oMatrixContext.CU_AnuncioAprobacion_Get(EstudioId)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw (ex)
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverxEstudio(ByVal EstudioId As Long)
        Try
            Dim oResult As List(Of CU_AnuncioAprobacion_Get_Result)
            oResult = GetAnuncioAprobacion(EstudioId)
            Return oResult.Item(0)
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
