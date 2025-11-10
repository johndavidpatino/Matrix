

'Imports CoreProject.US_Model
Imports System.Data.SqlClient
Imports System.Data.Entity.Core.Objects

Namespace US
    Public Class RolesPermisos
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

        Public Function GuardarRolesPermisos(Permiso As Integer, Rol As Integer) As Integer
            Try
                Try
                    Return SGContext.US_RolesPermisos_Add(Permiso, Rol)
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

        Public Function ObtenerRolesPermisos(ByVal Permiso As Integer) As List(Of RolesPermisos_Result)
            Try
                Try
                    Dim List As ObjectResult(Of RolesPermisos_Result) = SGContext.US_RolesPermisos_Get(Permiso)
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