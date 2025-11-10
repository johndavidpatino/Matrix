
'Imports CoreProject.PY_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class ControlCalidad
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
    Public Function DevolverTodos() As List(Of PY_ControlCalidad_Get_Result)
        Try
            Return PY_ControlCalidad_Get(Nothing, Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxID(ByVal ID As Int64) As PY_ControlCalidad_Get_Result
        Try
            Dim oResult As List(Of PY_ControlCalidad_Get_Result)
            oResult = PY_ControlCalidad_Get(ID, Nothing, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxIdTipoProcesoyTrabajo(ByVal IdTrabajo As Int64?, ByVal IdTipoProceso As Int64?) As List(Of PY_ControlCalidad_Get_Result)
        Try
            Return PY_ControlCalidad_Get(Nothing, IdTrabajo, IdTipoProceso)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Private Function PY_ControlCalidad_Get(ByVal ID As Int64?, ByVal IdTrabajo As Int64?, ByVal IdTipoProceso As Int64?) As List(Of PY_ControlCalidad_Get_Result)
        Try
            Dim oResult As ObjectResult(Of PY_ControlCalidad_Get_Result) = oMatrixContext.PY_ControlCalidad_Get(ID, IdTrabajo, IdTipoProceso)
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
    Public Function Guardar(ByVal ID As Int64?, ByVal IdTrabajo As Int64?, ByVal Evaluador As String, ByVal RoleEvaluador As String, ByVal Persona As Int64?, ByVal Fecha As Date, ByVal TipoProceso As Int64?)
        Try
            Dim ControlID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.PY_ControlCalidad_Edit(ID, IdTrabajo, Evaluador, RoleEvaluador, Persona, Fecha, TipoProceso)
                ControlID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.PY_ControlCalidad_Add(IdTrabajo, Evaluador, RoleEvaluador, Persona, Fecha, TipoProceso)
                ControlID = Decimal.Parse(oResult.ToList().Item(0))
            End If

            Return ControlID
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
            Return oMatrixContext.PY_ControlCalidad_Del(ID)
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
