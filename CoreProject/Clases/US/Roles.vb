

'Imports CoreProject.US_Model
Imports System.Data.SqlClient
Imports System.Data.Entity.Core.Objects

Namespace US
    Public Class Roles
        Private _sgcontext As US_Entities

        Public Property SGContext As US_Entities
            Get
                If Me._sgcontext Is Nothing Then
                    Me._sgcontext = New US_Entities()
                End If
                Return Me._sgcontext
            End Get
            Set(value As US_Entities)
                Me._sgcontext = value
            End Set
        End Property

        Public Function GuardarRol(id As Integer, Rol As String) As Integer
            Try
                Try
                    Return SGContext.US_Roles_Add(id, Rol)
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

        Public Function EditarRol(id As Integer, Rol As String) As Integer
            Try
                Try
                    Return SGContext.US_Roles_Edit(id, Rol)
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

        Public Function EliminarRol(ByVal id As Integer) As Integer
            Try
                Try
                    Return SGContext.US_Roles_Del(id)
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

        Public Function ObtenerRol(ByVal Rol As String) As List(Of Roles_Result)
            Try
                Try
                    Dim List As ObjectResult(Of Roles_Result) = SGContext.US_Roles_Get(Rol)
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

        Public Function ObtenerRolCombo() As List(Of RolesCombo_Result)
            Try
                Try
                    Dim List As ObjectResult(Of RolesCombo_Result) = SGContext.US_Roles_GetCombo
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