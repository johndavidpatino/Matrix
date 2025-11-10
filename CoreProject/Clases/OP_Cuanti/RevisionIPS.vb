Imports System.Data.Entity.Core.Objects

'Imports CoreProject.OP_Model
Imports System.Data.SqlClient

Namespace OP
    Public Class RevisionIPS

#Region "Variables Globales"
        Private oMatrixContext As OP_Cuanti
#End Region

#Region "Constructores"
        'Constructor public 
        Public Sub New()
            oMatrixContext = New OP_Cuanti
        End Sub
#End Region

#Region "Obtener"
        Public Function DevolverTodos() As List(Of OP_IPS_Revision_Get_Result)
            Try
                Return OP_IPS_Revision_Get(Nothing, Nothing, Nothing)
            Catch ex As Exception
                If (ex.InnerException Is Nothing) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function
        Public Function DevolverxTrabajoID(ByVal TrabajoID As Int64?) As List(Of OP_IPS_Revision_Get_Result)
            Try
                Return OP_IPS_Revision_Get(Nothing, TrabajoID, Nothing)
            Catch ex As Exception
                If (ex.InnerException Is Nothing) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function DevolverxTareaId(ByVal TrabajoID As Int64?, ByVal TareaId As Int64?) As List(Of OP_IPS_Revision_Get_Result)
            Try
                Return OP_IPS_Revision_Get(Nothing, TrabajoID, TareaId)
            Catch ex As Exception
                If (ex.InnerException Is Nothing) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function DevolverxID(ByVal ID As Int64) As OP_IPS_Revision_Get_Result
            Try
                Dim oResult As List(Of OP_IPS_Revision_Get_Result)
                oResult = OP_IPS_Revision_Get(ID, Nothing, Nothing)
                Return oResult.Item(0)
            Catch ex As Exception
                If (ex.InnerException Is Nothing) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function
        Private Function OP_IPS_Revision_Get(ByVal ID As Int64?, ByVal TrabajoID As Int64?, ByVal TareaId As Int64?) As List(Of OP_IPS_Revision_Get_Result)
            Try
                Dim oResult As ObjectResult(Of OP_IPS_Revision_Get_Result) = oMatrixContext.OP_IPS_Revision_Get(ID, TrabajoID, TareaId)
                Return oResult.ToList()
            Catch ex As Exception
                If (ex.InnerException Is Nothing) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function ProcesosListXTarea(ByVal Tarea As Int16) As List(Of OP_IPS_Procesos)
            Return oMatrixContext.OP_IPS_Procesos.Where(Function(x) x.IdTarea = Tarea).ToList
        End Function

#End Region
#Region "Guardar"
        Public Function Guardar(ByVal ID As Int64?, ByVal TrabajoId As Int64?, ByVal TareaId As Int64?, ByVal Pregunta As String, ByVal Observacion As String, ByVal DescripcionObservacion As String, ByVal RespuestaProgramador As String, ByVal Rechazar As String, ByVal Estado As String, ByVal UsuarioRegistra As Int64?, ByVal UsuarioProgramador As Int64?, ByVal Instrumento As Byte?, ByVal Version As String, ByVal Aplicativo As Int32?, ByVal Proceso As Int64?) As Decimal
            Try
                Dim EjecucionID As Decimal = 0

                If ID > 0 Then
                    oMatrixContext.OP_IPS_Revision_Edit(ID, TrabajoId, TareaId, Pregunta, Observacion, DescripcionObservacion, RespuestaProgramador, Rechazar, Estado, UsuarioProgramador, Instrumento, Version, Aplicativo, Proceso)
                    EjecucionID = ID
                Else
                    Dim oResult As ObjectResult(Of Decimal?)
                    oResult = oMatrixContext.OP_IPS_Revision_Add(TrabajoId, TareaId, Pregunta, Observacion, DescripcionObservacion, RespuestaProgramador, Rechazar, Estado, UsuarioRegistra, UsuarioProgramador, Instrumento, Version, Aplicativo, Proceso)
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
                Return oMatrixContext.OP_IPS_Revision_Del(ID)
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
End Namespace