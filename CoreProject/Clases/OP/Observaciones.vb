Imports System.Data.Entity.Core.Objects
'Imports CoreProject.OP_Model

<Serializable()>
Public Class Observaciones
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
    Public Function DevolverTodos() As List(Of OP_Observaciones_Get_Result)
        Try
            Return OP_Observaciones_Get(Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverXObservacionId(ByVal ObservacionId As Int64) As List(Of OP_Observaciones_Get_Result)
        Try
            Return OP_Observaciones_Get(Nothing, ObservacionId)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxID(ByVal ID As Int64) As OP_Observaciones_Get_Result
        Try
            Dim oResult As List(Of OP_Observaciones_Get_Result)
            oResult = OP_Observaciones_Get(ID, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Private Function OP_Observaciones_Get(ByVal ID As Int64, ByVal ObservacionId As Int64) As List(Of OP_Observaciones_Get_Result)
        Try
            Dim oResult As ObjectResult(Of OP_Observaciones_Get_Result) = oMatrixContext.OP_Observaciones_Get(ID, ObservacionId)
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
    Public Function Guardar(ByVal ID As Int64?, ByVal ObservacionId As Int64?, ByVal CiudadID As Int32?, ByVal PersonaAEntrevistar As String,
                            ByVal Direccion As String, ByVal Telefono As String, ByVal Fecha As Date?, ByVal Hora As TimeSpan?, ByVal GrupoObjetivo As String, ByVal CaracteristicasEspeciales As String, ByVal Entrevistador As Int64?,
                            ByVal FechaReal As Date?, ByVal HoraReal As TimeSpan?, ByVal Observaciones As String, ByVal Cancelada As Boolean?) As Decimal
        Try
            Dim ObservacionesID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.OP_Observaciones_Edit(ID, ObservacionId, CiudadID, PersonaAEntrevistar, Direccion, Telefono, Fecha, Hora, GrupoObjetivo, CaracteristicasEspeciales, Entrevistador, FechaReal, HoraReal, Observaciones, Cancelada)
                ObservacionesID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.OP_Observaciones_Add(ObservacionId, CiudadID, PersonaAEntrevistar, Direccion, Telefono, Fecha, Hora, GrupoObjetivo, CaracteristicasEspeciales, Entrevistador, FechaReal, HoraReal, Observaciones, Cancelada)
                ObservacionesID = Decimal.Parse(oResult.ToList().Item(0))
            End If

            Return ObservacionesID
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
            Return oMatrixContext.OP_Observaciones_Del(ID)
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
