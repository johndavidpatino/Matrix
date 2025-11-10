

'Imports CoreProject.SG_Model
Imports System.Data.Entity.Core.Objects
Imports System.Data.SqlClient

Namespace SG
    Public Class AsistenteActa

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

        Public Function GuardarAsistenteActa(UsuarioId As Integer, ActaId As Integer) As Integer
            Try
                Try
                    Return SGContext.SG_AsistenteActaComite_Add(UsuarioId, ActaId)
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

        Public Function EditarAsistenteActa(id As Integer, ActaId As Integer, UsuarioId As Integer) As Integer
            Try
                Try
                    Return SGContext.SG_AsistenteActaComite_Edit(id, ActaId, UsuarioId)
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

        Public Function EliminarAsistenteActa(ByVal id As Integer) As Integer
            Try
                Try
                    Return SGContext.SG_AsistenteActaComite_Del(id)
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

        Public Function ConsultarAsistenteActaXActaId(ByVal ActaId As Integer) As List(Of SG_AsistenteActaComite_Get_ActaId_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_AsistenteActaComite_Get_ActaId_Result) = SGContext.SG_AsistenteActaComite_Get_ActaId(ActaId)
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