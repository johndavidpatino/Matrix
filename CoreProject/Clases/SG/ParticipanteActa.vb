Imports System.Data.Entity.Core.Objects
Imports System.Data.SqlClient

'Imports CoreProject.SG_Model

Namespace SG
    Public Class ParticipanteActa

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

        Public Function GuardarParticipanteActa(ActaId As Integer, UsuarioId As Integer) As Integer
            Try
                Try
                    Return SGContext.SG_ParticipanteActa_Add(ActaId, UsuarioId)
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

        Public Function EditarParticipanteActa(id As Integer, ActaId As Integer, UsuarioId As Integer) As Integer
            Try
                Try
                    Return SGContext.SG_ParticipanteActa_Edit(id, ActaId, UsuarioId)
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

        Public Function EliminarParticipanteActa(ByVal id As Integer) As Integer
            Try
                Try
                    Return SGContext.SG_ParticipanteActa_Del(id)
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

        Public Function ObtenerParticipanteActa() As List(Of SG_ParticipanteActa_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_ParticipanteActa_Result) = SGContext.SG_ParticipanteActa_Get
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

        Public Function ConsultarParticipanteActaXNoActa(ByVal ActaId As Integer) As List(Of SG_ParticipanteActa_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_ParticipanteActa_Result) = SGContext.SG_ParticipanteActa_Get_ActaId(ActaId)
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