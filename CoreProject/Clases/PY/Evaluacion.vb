
'Imports CoreProject.PY_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class Evaluacion
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
    Public Function DevolverTodos() As List(Of PY_Evaluacion_Get_Result)
        Try
            Return PY_Evaluacion_Get(Nothing, Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxID(ByVal ID As Int64) As PY_Evaluacion_Get_Result
        Try
            Dim oResult As List(Of PY_Evaluacion_Get_Result)
            oResult = PY_Evaluacion_Get(ID, Nothing, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxIdTipoProcesoyTrabajo(ByVal IdTrabajo As Int64?, ByVal IdTipoProceso As Int64?) As List(Of PY_Evaluacion_Get_Result)
        Try
            Return PY_Evaluacion_Get(Nothing, IdTrabajo, IdTipoProceso)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Private Function PY_Evaluacion_Get(ByVal ID As Int64?, ByVal IdTrabajo As Int64?, ByVal IdTipoProceso As Int64?) As List(Of PY_Evaluacion_Get_Result)
        Try
            Dim oResult As ObjectResult(Of PY_Evaluacion_Get_Result) = oMatrixContext.PY_Evaluacion_Get(ID, IdTrabajo, IdTipoProceso)
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
    Public Function Guardar(ByVal ID As Int64?, ByVal IdTrabajo As Int64?, ByVal Persona As Int64?, ByVal Evaluador As String, ByVal RoleEvaluador As String, ByVal Fecha As Date, ByVal Comentarios As String, ByVal TipoProceso As Int64?)
        Try
            Dim EvaluacionID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.PY_Evaluacion_Edit(ID, IdTrabajo, Persona, Evaluador, RoleEvaluador, Fecha, Comentarios, TipoProceso)
                EvaluacionID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.PY_Evaluacion_Add(IdTrabajo, Persona, Evaluador, RoleEvaluador, Fecha, Comentarios, TipoProceso)
                EvaluacionID = Decimal.Parse(oResult.ToList().Item(0))
            End If

            Return EvaluacionID
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
            Return oMatrixContext.PY_Evaluacion_Del(ID)
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
