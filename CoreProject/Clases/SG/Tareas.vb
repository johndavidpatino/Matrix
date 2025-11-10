

'Imports CoreProject.SG_Model
Imports System.Data.Entity.Core.Objects
Imports System.Data.SqlClient

Namespace SG
    Public Class Tareas

        Private _sgcontext As SG_Entities

        Public Property SGContext As SG_Entities
            Get
                If Me._sgcontext Is Nothing Then
                    Me._sgcontext = New SG_Entities()
                End If
                Return Me._sgcontext
            End Get
            Set(value As SG_Entities)
                Me._sgcontext = value
            End Set
        End Property

        Public Function GuardarTarea(ActaId As Integer, Tarea As String, Responsable As Integer, Cerrada As Boolean, ByVal fecha As DateTime, fechaInicioEjecucion As DateTime, fechaLimite As DateTime, ByVal fechaCierre As DateTime, ByVal estadoId As Integer) As Integer

            Try
                Try
                    Dim Res As ObjectResult(Of SG_Tarea_Add_Result)
                    Res = SGContext.SG_Tarea_Add(ActaId, Tarea, Responsable, Cerrada, fecha, fechaInicioEjecucion, fechaLimite, fechaCierre, estadoId)
                    Return Res(0).id
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function EditarTarea(id As Integer, ActaId As Integer, Tarea As String, Responsable As Integer, Cerrada As Boolean, ByVal fecha As DateTime, fechaInicioEjecucion As DateTime, fechaLimite As DateTime, ByVal fechaCierre As DateTime, ByVal estadoId As Integer)
            Try
                Try
                    Return SGContext.SG_Tarea_Edit(id, ActaId, Tarea, Responsable, Cerrada, fecha, fechaInicioEjecucion, fechaLimite, fechaCierre, estadoId)
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function EditarEstado(id As Integer, ByVal estadoId As Integer)
            Try
                Try
                    Return SGContext.SG_Tarea_EditEstadoId(id, estadoId)
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function EliminarTarea(ByVal id As Integer) As Integer
            Try
                Try
                    Return SGContext.SG_Tarea_Del(id)
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function ObtenerTareas() As List(Of SG_Tarea_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_Tarea_Result) = SGContext.SG_Tarea_Get
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function ConsultarTareasXActa(ByVal Acta As Integer) As List(Of SG_Tarea_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_Tarea_Result) = SGContext.SG_Tarea_Get_ActaId(Acta)
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

    End Class
End Namespace