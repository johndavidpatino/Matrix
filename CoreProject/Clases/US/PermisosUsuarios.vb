

'Imports CoreProject.US_Model
Imports System.Data.SqlClient
Imports System.Data.Entity.Core.Objects

Namespace US
    Public Class PermisosUsuarios
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

        Public Function GuardarPermisosUsuario(Usuario As Long, Permiso As Integer) As Integer
            Try
                Try
                    Return SGContext.US_PermisosUsuarios_Add(Permiso, Usuario)
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

        Public Function EliminarPermisosUsuario(ByVal id As Integer) As Integer
            Try
                Try
                    Return SGContext.US_PermisosUsuarios_Del(id)
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

        Public Function ObtenerPermisosUsuario(ByVal Usuario As Integer) As List(Of PermisosUsuarios_Result)
            Try
                Try
                    Dim List As ObjectResult(Of PermisosUsuarios_Result) = SGContext.US_PermisosUsuarios_Get(Usuario)
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

        Public Function obtenerPermisosXUsuario(ByVal usuarioId As Int64, ByVal asignado As Boolean) As List(Of US_PermisosXUsuario_Get_Result)
            Return SGContext.US_PermisosXUsuario_Get(usuarioId, asignado).ToList
        End Function

        Public Sub eliminar(ByVal idUsuario As Int64, ByVal idPermiso As Int16)
            Dim usuario As New US_Usuarios With {.id = idUsuario}
            Dim permiso As New US_Permisos With {.id = idPermiso}
            usuario.US_Permisos.Add(permiso)
            SGContext.US_Usuarios.Attach(usuario)

            usuario.US_Permisos.Remove(permiso)
            SGContext.SaveChanges()
        End Sub
    End Class
End Namespace