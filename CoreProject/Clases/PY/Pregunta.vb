Imports System.Data.Entity.Core.Objects
'Imports CoreProject.PY_Model

<Serializable()>
Public Class Pregunta
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
    Public Function DevolverTodos() As List(Of PY_Preguntas_Get_Result)
        Try
            Return PY_Preguntas_Get(Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxID(ByVal ID As Int64) As PY_Preguntas_Get_Result
        Try
            Dim oResult As List(Of PY_Preguntas_Get_Result)
            oResult = PY_Preguntas_Get(ID, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxIdTipo(ByVal IdTipo As Int64) As List(Of PY_Preguntas_Get_Result)
        Try
            Return PY_Preguntas_Get(Nothing, IdTipo)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function PY_Tipos_Procesos_Get() As List(Of PY_Tipos_Procesos_Get_Result)
        Try
            Dim oResult As ObjectResult(Of PY_Tipos_Procesos_Get_Result) = oMatrixContext.PY_Tipos_Procesos_Get()
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Private Function PY_Preguntas_Get(ByVal ID As Int64, ByVal IdTipo As Int64) As List(Of PY_Preguntas_Get_Result)
        Try
            Dim oResult As ObjectResult(Of PY_Preguntas_Get_Result) = oMatrixContext.PY_Preguntas_Get(ID, IdTipo)
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
    Public Function Guardar(ByVal ID As Int64?, ByVal Pregunta As String, ByVal IdTipo As Int64?, ByVal Activa As Boolean?) As Decimal
        Try
            Dim PreguntaID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.PY_Preguntas_Edit(ID, Pregunta, IdTipo, Activa)
                PreguntaID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.PY_Preguntas_Add(Pregunta, IdTipo, Activa)
                PreguntaID = Decimal.Parse(oResult.ToList().Item(0))
            End If

            Return PreguntaID
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
