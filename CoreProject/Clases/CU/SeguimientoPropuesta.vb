
'Imports CoreProject.CU_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class SeguimientoPropuesta
#Region "Variables Globales"
    Private oMatrixContext As CU_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New CU_Entities
    End Sub
#End Region
#Region "Obtener"
    Public Function DevolverSeguimientoPropuesta(ByVal IdPropuesta As Int64) As List(Of CU_SeguimientoPropuestas_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_SeguimientoPropuestas_Get_Result) = oMatrixContext.CU_SeguimientoPropuestas_Get(IdPropuesta)
            Return oResult.ToList
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
#End Region
#Region "Guardar"
    Public Sub Guardar(ByVal Observacion As String, ByVal UsuarioId As Int64, ByVal PropuestaId As Int64)
        Try
            oMatrixContext.CU_SeguimientoPropuestas_Add(Observacion, UsuarioId, PropuestaId)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Sub
#End Region
End Class
