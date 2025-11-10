
'Imports CoreProject.OP_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class Transcripciones
#Region "Variables Globales"
    Private oMatrixContext As OP_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New OP_Entities
    End Sub
#End Region
#Region "Obtener"
    Public Function DevolverTodos() As List(Of OP_Transcripciones_Get_Result)
        Try
            Return OP_Transcripciones_Get(Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverXTrabajoId(ByVal TrabajoId As Int64) As List(Of OP_Transcripciones_Get_Result)
        Try
            Return OP_Transcripciones_Get(Nothing, TrabajoId)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxID(ByVal ID As Int64) As OP_Transcripciones_Get_Result
        Try
            Dim oResult As List(Of OP_Transcripciones_Get_Result)
            oResult = OP_Transcripciones_Get(ID, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Private Function OP_Transcripciones_Get(ByVal ID As Int64, ByVal TrabajoId As Int64) As List(Of OP_Transcripciones_Get_Result)
        Try
            Dim oResult As ObjectResult(Of OP_Transcripciones_Get_Result) = oMatrixContext.OP_Transcripciones_Get(ID, TrabajoId)
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
    Public Function Guardar(ByVal ID As Int64?, ByVal TrabajoId As Int64?, ByVal Responsable As Int64?, ByVal Transcriptor As Int64?, ByVal Cantidad As Short?, ByVal FechaEntrega As Date?, ByVal FechaRequerida As Date?, ByVal FechaTranscripcion As Date?) As Int64
        Try
            Dim TranscripcionID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.OP_Transcripciones_Edit(ID, TrabajoId, Responsable, Transcriptor, Cantidad, FechaEntrega, FechaRequerida, FechaTranscripcion)
                TranscripcionID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.OP_Transcripciones_Add(TrabajoId, Responsable, Transcriptor, Cantidad, FechaEntrega, FechaRequerida, FechaTranscripcion)
                TranscripcionID = Decimal.Parse(oResult.ToList().Item(0))
            End If

            Return TranscripcionID
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
            Return oMatrixContext.OP_Transcripciones_Del(ID)
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
