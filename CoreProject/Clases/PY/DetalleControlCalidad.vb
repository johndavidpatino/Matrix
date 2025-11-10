Imports System.Data.Entity.Core.Objects
Imports System.Data.SqlClient
'Imports CoreProject.PY_Model

<Serializable()>
Public Class DetalleControlCalidad
#Region "Variables Globales"
    Private oMatrixContext As PY_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New PY_Entities
    End Sub
#End Region
#Region "Obtener"
    Public Function DevolverTodos() As List(Of PY_DetalleControlCalidad_Get_Result)
        Try
            Return PY_DetalleControlCalidad_Get(Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxID(ByVal ID As Int64) As PY_DetalleControlCalidad_Get_Result
        Try
            Dim oResult As List(Of PY_DetalleControlCalidad_Get_Result)
            oResult = PY_DetalleControlCalidad_Get(ID, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxIDControl(ByVal IDControl As Int64) As List(Of PY_DetalleControlCalidad_Get_Result)
        Try
            Return PY_DetalleControlCalidad_Get(Nothing, IDControl)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Private Function PY_DetalleControlCalidad_Get(ByVal ID As Int64, ByVal IDControl As Int64) As List(Of PY_DetalleControlCalidad_Get_Result)
        Try
            Dim oResult As ObjectResult(Of PY_DetalleControlCalidad_Get_Result) = oMatrixContext.PY_DetalleControlCalidad_Get(ID, IDControl)
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
    Public Function Guardar(ByVal ID As Int64?, ByVal IdControlCalidad As Int64?, ByVal IdPregunta As Int64?, ByVal SI As Boolean?, ByVal Comentarios As String)
        Try
            Dim DetalleId As Int64
            If ID > 0 Then
                'oMatrixContext.(ID, CapacitacionId, Participante, Eficacia, OportunidadMejora, Aprobo)
                DetalleId = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.PY_DetalleControlCalidad_Add(IdControlCalidad, IdPregunta, SI, Comentarios)
                DetalleId = Decimal.Parse(oResult.ToList().Item(0))
            End If
            Return DetalleId
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
#Region "Eliminar"
    Public Function EliminarXControlCalidadID(ByVal ControlCalidadID As Int64) As Integer
        Try
            Return oMatrixContext.PY_DetalleControlCalidad_DelxIdControl(ControlCalidadID)
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
