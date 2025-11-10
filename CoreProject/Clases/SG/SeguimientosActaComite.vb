Imports System.Data.Entity.Core.Objects
Imports System.Data.SqlClient

'Imports CoreProject.SG_Model

Namespace SG
    Public Class SeguimientosActaComite

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

        Public Function GuardarSeguimiento(actaId As Integer, accion As String, responsable As Integer, ByVal fechaInicio As Date, ByVal fechaCompromiso As Date, ByVal estado As Integer, ByVal fechaCierre As Date) As Integer
            Try
                Try
                    Return SGContext.SG_SeguimientosActaComite_Add(actaId, accion, responsable, fechaInicio, fechaCompromiso, estado, fechaCierre)
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

        Public Function EditarSeguimiento(id As Integer, actaId As Integer, accion As String, responsable As Integer, ByVal fechaInicio As Date, ByVal fechaCompromiso As Date, ByVal estado As Integer, ByVal fechaCierre As Date) As Integer
            Try
                Try
                    Return SGContext.SG_SeguimientosActaComite_Edit(id, actaId, accion, responsable, fechaInicio, fechaCompromiso, estado, fechaCierre)
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

        Public Function EliminarSeguimiento(ByVal id As Integer) As Integer
            Try
                Try
                    Return SGContext.SG_SeguimientosActaComite_Del(id)
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

        Public Function ObtenerSeguimientos(ByVal actaId As Integer) As List(Of SG_SeguimientosActaComite_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_SeguimientosActaComite_Get_Result) = SGContext.SG_SeguimientosActaComite_Get(actaId)
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

        Public Function ObtenerEstadoSeguimiento() As List(Of SG_EstadoSeguimiento_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_EstadoSeguimiento_Get_Result) = SGContext.SG_EstadoSeguimiento_Get
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