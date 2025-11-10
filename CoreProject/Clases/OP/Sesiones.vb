Imports System.Data.Entity.Core.Objects
'Imports CoreProject.OP_Model

<Serializable()>
Public Class Sesiones
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
    Public Function DevolverTodos() As List(Of OP_Sesiones_Get_Result)
        Try
            Return OP_Sesiones_Get(Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverXSesionID(ByVal SesionID As Int64) As List(Of OP_Sesiones_Get_Result)
        Try
            Return OP_Sesiones_Get(Nothing, SesionID)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxID(ByVal ID As Int64) As OP_Sesiones_Get_Result
        Try
            Dim oResult As List(Of OP_Sesiones_Get_Result)
            oResult = OP_Sesiones_Get(ID, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Private Function OP_Sesiones_Get(ByVal ID As Int64, ByVal SesionID As Int64) As List(Of OP_Sesiones_Get_Result)
        Try
            Dim oResult As ObjectResult(Of OP_Sesiones_Get_Result) = oMatrixContext.OP_Sesiones_Get(ID, SesionID)
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
    Public Function Guardar(ByVal ID As Int64?, ByVal SesionID As Int64?, ByVal CiudadID As Int32?, ByVal Fecha As Date?, ByVal Hora As TimeSpan?, ByVal GrupoObjetivo As String, ByVal CaracteristicasEspeciales As String, ByVal Moderador As Int64?,
                            ByVal Locacion As String, ByVal AsistentesReales As Short?, ByVal FechaReal As Date?, ByVal HoraReal As TimeSpan?, ByVal Observaciones As String, ByVal Cancelada As Boolean?) As Decimal
        Try
            Dim SesionesID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.OP_Sesiones_Edit(ID, SesionID, CiudadID, Fecha, Hora, GrupoObjetivo, CaracteristicasEspeciales, Moderador, Locacion, AsistentesReales, FechaReal, HoraReal, Observaciones, Cancelada)
                SesionesID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.OP_Sesiones_Add(SesionID, CiudadID, Fecha, Hora, GrupoObjetivo, CaracteristicasEspeciales, Moderador, Locacion, AsistentesReales, FechaReal, HoraReal, Observaciones, Cancelada)
                SesionesID = Decimal.Parse(oResult.ToList().Item(0))
            End If

            Return SesionesID
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
            Return oMatrixContext.OP_Sesiones_Del(ID)
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
