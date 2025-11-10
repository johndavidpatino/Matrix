
'Imports CoreProject.OP_RO_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class EjecucionCuestionario
#Region "Variables Globales"
    Private oMatrixContext As OP_RO_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New OP_RO_Entities
    End Sub
#End Region
#Region "Obtener"
    Public Function DevolverTodos() As List(Of OP_RO_EjecucionCuestionario_Get_Result)
        Try
            Return OP_RO_EjecucionCuestionario_Get(Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxTrabajoID(ByVal TrabajoID As Int64?) As List(Of OP_RO_EjecucionCuestionario_Get_Result)
        Try
            Return OP_RO_EjecucionCuestionario_Get(Nothing, TrabajoID)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxID(ByVal ID As Int64) As OP_RO_EjecucionCuestionario_Get_Result
        Try
            Dim oResult As List(Of OP_RO_EjecucionCuestionario_Get_Result)
            oResult = OP_RO_EjecucionCuestionario_Get(ID, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Private Function OP_RO_EjecucionCuestionario_Get(ByVal ID As Int64?, ByVal TrabajoID As Int64?) As List(Of OP_RO_EjecucionCuestionario_Get_Result)
        Try
            Dim oResult As ObjectResult(Of OP_RO_EjecucionCuestionario_Get_Result) = oMatrixContext.OP_RO_EjecucionCuestionario_Get(ID, TrabajoID)
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
    Public Function Guardar(ByVal ID As Int64?, ByVal TrabajoId As Int64?, ByVal Pregunta As String, ByVal Observacion As String, ByVal DescripcionObservacion As String, ByVal RespuestaGerenteProyecto As String) As Decimal
        Try
            Dim EjecucionID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.OP_RO_EjecucionCuestionario_Edit(ID, TrabajoId, Pregunta, Observacion, DescripcionObservacion, RespuestaGerenteProyecto)
                EjecucionID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.OP_RO_EjecucionCuestionario_Add(TrabajoId, Pregunta, Observacion, DescripcionObservacion, RespuestaGerenteProyecto)
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
            Return oMatrixContext.OP_RO_EjecucionCuestionario_Del(ID)
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
