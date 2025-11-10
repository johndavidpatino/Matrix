Imports System.Data.Entity.Core.Objects
'Imports CoreProject.TH_Model
'Imports CoreProject

<Serializable()>
Public Class CapacitacionesParticipantes
#Region "Variables Globales"
    Private oMatrixContext As TH_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New TH_Entities
    End Sub
#End Region
#Region "Obtener"
    Public Function DevolverTodos() As List(Of TH_CapacitacionesParticipantes_Get_Result)
        Try
            Return TH_CapacitacionesParticipantes_Get(Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverXCapacitacionId(ByVal CapacitacionId As Int64) As List(Of TH_CapacitacionesParticipantes_Get_Result)
        Try
            Return TH_CapacitacionesParticipantes_Get(Nothing, CapacitacionId)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxID(ByVal ID As Int64) As TH_CapacitacionesParticipantes_Get_Result
        Try
            Dim oResult As List(Of TH_CapacitacionesParticipantes_Get_Result)
            oResult = TH_CapacitacionesParticipantes_Get(ID, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Private Function TH_CapacitacionesParticipantes_Get(ByVal ID As Int64, ByVal CapacitacionId As Int64) As List(Of TH_CapacitacionesParticipantes_Get_Result)
        Try
            Dim oResult As ObjectResult(Of TH_CapacitacionesParticipantes_Get_Result) = oMatrixContext.TH_CapacitacionesParticipantes_Get(ID, CapacitacionId)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function TH_CapacitacionesParticipantesRefuerzo_Get(ByVal Cargo As Int64?, ByVal CapacitacionId As Int64) As List(Of TH_Personas_Get_Result)
        Try
            Dim oResult As ObjectResult(Of TH_Personas_Get_Result) = oMatrixContext.TH_ParticipantesCapacitacionesRefuerzo(CapacitacionId, Cargo)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function TH_CargosRefuerzo_Get(ByVal CapacitacionId As Int64) As List(Of TH_Cargos_Get_Result)
        Try
            Dim oResult As ObjectResult(Of TH_Cargos_Get_Result) = oMatrixContext.TH_CargosCapacitacionesRefuerzo(CapacitacionId)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function TH_CapacitacionesParticipantesNoAprobaron_Get(ByVal Cargo As Int64?, ByVal CapacitacionId As Int64) As List(Of TH_ParticipantesCapacitacionesNoAprobaron_Result)
        Try
            Dim oResult As ObjectResult(Of TH_ParticipantesCapacitacionesNoAprobaron_Result) = oMatrixContext.TH_ParticipantesCapacitacionesNoAprobaron(CapacitacionId, Cargo)
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
    Public Function Guardar(ByVal ID As Int64?, ByVal CapacitacionId As Int64?, ByVal Participante As Int64?, ByVal Eficacia As Decimal?, ByVal OportunidadMejora As String, ByVal Aprobo As Boolean?)
        Try
            Dim CapacitacionParticipanteId As Int64
            If ID > 0 Then
                oMatrixContext.TH_CapacitacionesParticipantes_Edit(ID, CapacitacionId, Participante, Eficacia, OportunidadMejora, Aprobo)
                CapacitacionParticipanteId = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.TH_CapacitacionesParticipantes_Add(CapacitacionId, Participante, Eficacia, OportunidadMejora, Aprobo)
                CapacitacionParticipanteId = Decimal.Parse(oResult.ToList().Item(0))
            End If
            Return CapacitacionParticipanteId
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
#Region "Eliminar"
    Public Function Eliminar(ByVal ID As Int64) As Integer
        Try
            Return oMatrixContext.TH_CapacitacionesParticipantes_Del(ID)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function EliminarXCapacitacionID(ByVal CapacitacionID As Int64) As Integer
        Try
            Return oMatrixContext.TH_CapacitacionesParticipantes_DelXCapacitacionID(CapacitacionID)
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
