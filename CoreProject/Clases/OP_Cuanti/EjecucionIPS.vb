Imports System.Data.Entity.Core.Objects

'Imports CoreProject.OP_Model
Imports System.Data.SqlClient

Namespace OP
    Public Class EjecucionIPS

#Region "Variables Globales"
        Private oMatrixContext As OP_Cuanti
#End Region

#Region "Constructores"
        'Constructor public 
        Public Sub New()
            oMatrixContext = New OP_Cuanti
        End Sub
#End Region

#Region "Obtener"
        Public Function DevolverTodos() As List(Of OP_IPS_Ejecucion_Get_Result)
            Try
                Return OP_IPS_Ejecucion_Get(Nothing, Nothing)
            Catch ex As Exception
                If (ex.InnerException Is Nothing) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function
        Public Function DevolverxTrabajoID(ByVal TrabajoID As Int64?) As List(Of OP_IPS_Ejecucion_Get_Result)
            Try
                Return OP_IPS_Ejecucion_Get(Nothing, TrabajoID)
            Catch ex As Exception
                If (ex.InnerException Is Nothing) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function
        Public Function DevolverxID(ByVal ID As Int64) As OP_IPS_Ejecucion_Get_Result
            Try
                Dim oResult As List(Of OP_IPS_Ejecucion_Get_Result)
                oResult = OP_IPS_Ejecucion_Get(ID, Nothing)
                Return oResult.Item(0)
            Catch ex As Exception
                If (ex.InnerException Is Nothing) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function
        Private Function OP_IPS_Ejecucion_Get(ByVal ID As Int64?, ByVal TrabajoID As Int64?) As List(Of OP_IPS_Ejecucion_Get_Result)
            Try
                Dim oResult As ObjectResult(Of OP_IPS_Ejecucion_Get_Result) = oMatrixContext.OP_IPS_Ejecucion_Get(ID, TrabajoID)
                Return oResult.ToList()
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
        Public Function Guardar(ByVal ID As Int64?, ByVal TrabajoId As Int64?, ByVal Pregunta As String, ByVal Observacion As String, ByVal DescripcionObservacion As String, ByVal RespuestaProgramador As String) As Decimal
            Try
                Dim EjecucionID As Decimal = 0

                If ID > 0 Then
                    oMatrixContext.OP_IPS_Ejecucion_Edit(ID, TrabajoId, Pregunta, Observacion, DescripcionObservacion, RespuestaProgramador)
                    EjecucionID = ID
                Else
                    Dim oResult As ObjectResult(Of Decimal?)
                    oResult = oMatrixContext.OP_IPS_Ejecucion_Add(TrabajoId, Pregunta, Observacion, DescripcionObservacion, RespuestaProgramador)
                    EjecucionID = Decimal.Parse(oResult.ToList().Item(0))
                End If

                Return EjecucionID
            Catch ex As Exception
                If (ex.InnerException Is Nothing) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function
#End Region
#Region "Eliminar"
        Public Function Eliminar(ByVal ID As Int64) As Integer
            Try
                Return oMatrixContext.OP_IPS_Ejecucion_Del(ID)
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
End Namespace