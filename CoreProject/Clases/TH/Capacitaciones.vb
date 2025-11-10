Imports System.Data.Entity.Core.Objects
'Imports CoreProject.TH_Model

<Serializable()>
Public Class Capacitaciones
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
    Public Function DevolverTodos() As List(Of TH_Capacitaciones_Get_Result)
        Try
            Return TH_Capacitaciones_Get(Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxTrabajoID(ByVal TrabajoID As Int64?) As List(Of TH_Capacitaciones_Get_Result)
        Try
            Return TH_Capacitaciones_Get(Nothing, TrabajoID)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxID(ByVal ID As Int64) As TH_Capacitaciones_Get_Result
        Try
            Dim oResult As List(Of TH_Capacitaciones_Get_Result)
            oResult = TH_Capacitaciones_Get(ID, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Private Function TH_Capacitaciones_Get(ByVal ID As Int64, ByVal TrabajoID As Int64) As List(Of TH_Capacitaciones_Get_Result)
        Try
            Dim oResult As ObjectResult(Of TH_Capacitaciones_Get_Result) = oMatrixContext.TH_Capacitaciones_Get(ID, TrabajoID)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function TH_CapacitacionPersonas_Get(parameters As TH_CapacitacionPersonasParameters) As List(Of TH_CapacitacionPersonas_Get_Result)
        Try

            Dim oResult As ObjectResult(Of TH_CapacitacionPersonas_Get_Result) = oMatrixContext.TH_CapacitacionPersonas_Get(
                identificacion:=parameters.Identificacion,
                nombre:=parameters.Nombre,
                contratistaId:=parameters.ContratistaId,
                nombreContratista:=parameters.NombreContratista,
                capacitacionId:=parameters.CapacitacionId,
                sonParticipantes:=parameters.SonParticipantes,
                page:=parameters.Page,
                pageSize:=parameters.PageSize
            )
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function TH_CapacitacionParticipantes_Get(CapacitacionId As Long) As List(Of TH_CapacitacionParticipantes_Get_Result)
        Try

            Dim oResult As ObjectResult(Of TH_CapacitacionParticipantes_Get_Result) = oMatrixContext.TH_CapacitacionParticipantes_Get(
                CapacitacionId
            )
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Structure TH_CapacitacionPersonasParameters
        Public Identificacion As Long?
        Public Nombre As String
        Public ContratistaId As Long?
        Public NombreContratista As String
        Public CapacitacionId As Long?
        Public SonParticipantes As Int32?
        Public Page As Int32?
        Public PageSize As Int32?
    End Structure
#End Region
#Region "Guardar"
    Public Function Guardar(ByVal ID As Int64?, ByVal ubicacion As String, ByVal Fecha As Date?, ByVal Duracion As Byte?, ByVal Actividad As String, ByVal Responsable As Int64?, ByVal Capacitador As String, ByVal ObjetivoActividad As String, ByVal ModoEvaluacion As String, ByVal TrabajoId As Int64?) As Int64
        Try
            Dim CapacitacionId As Int64
            If ID > 0 Then
                oMatrixContext.TH_Capacitaciones_Edit(ID, ubicacion, Fecha, Duracion, Actividad, Responsable, Capacitador, ObjetivoActividad, ModoEvaluacion, TrabajoId)
                CapacitacionId = ID

            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.TH_Capacitaciones_Add(ubicacion, Fecha, Duracion, Actividad, Responsable, Capacitador, ObjetivoActividad, ModoEvaluacion, TrabajoId)
                CapacitacionId = Decimal.Parse(oResult.ToList().Item(0))
            End If
            Return CapacitacionId
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function AdicionarRefuerzo(ByVal ID As Int64?) As Int64
        Try
            Dim oResult As ObjectResult(Of Decimal?)
            Dim CapacitacionId As Int64
            oResult = oMatrixContext.TH_Capacitaciones_AddRefuerzo(ID)
                CapacitacionId = Decimal.Parse(oResult.ToList().Item(0))
            Return CapacitacionId
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
#Region "Agregar"
    Public Function TH_CapacitacionesParticipantes_Add(parameters As TH_CapacitacionesParticipantes_Add_Parameters) As Boolean
        Try

            oMatrixContext.TH_CapacitacionesParticipantes_Add(
                participante:=parameters.Participante,
                capacitacionId:=parameters.CapacitacionId,
                eficacia:=parameters.Eficacia,
                oportunidadMejora:=parameters.OportunidadMejora,
                aprobo:=parameters.Aprobo
                )
            Return True
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function TH_CapacitacionesParticipantes_Del(participantId As Long) As Boolean
        Try
            oMatrixContext.TH_CapacitacionesParticipantes_Del(participantId)
            Return True
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function TH_CapacitacionesParticipantes_Update(parameters As TH_CapacitacionesParticipantes_Update_Parameters) As Boolean
        Try
            Dim updateResponse As Integer = oMatrixContext.TH_CapacitacionesParticipantes_Edit(
                iD:=parameters.CapacitacionParticipanteId,
                capacitacionId:=parameters.CapacitacionId,
                participante:=parameters.Participante,
                eficacia:=parameters.Eficacia,
                oportunidadMejora:=parameters.OportunidadMejora,
                aprobo:=parameters.Aprobo
            )
            Return True
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function


    Public Structure TH_CapacitacionesParticipantes_Add_Parameters
        Public CapacitacionId As Long
        Public Participante As Long
        Public Eficacia As Long?
        Public OportunidadMejora As String
        Public Aprobo As Boolean
    End Structure
    Public Structure TH_CapacitacionesParticipantes_Update_Parameters
        Public CapacitacionParticipanteId As Long
        Public CapacitacionId As Long
        Public Participante As Long
        Public Eficacia As Double?
        Public OportunidadMejora As String
        Public Aprobo As Boolean
    End Structure
#End Region
#Region "Eliminar"
    Public Function Eliminar(ByVal ID As Int64) As Integer
        Try
            Return oMatrixContext.TH_Capacitaciones_Del(ID)
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
