

'Imports CoreProject.US_Model
Imports System.Data.Entity.Core.Objects
Imports System.Data.SqlClient

Namespace US
    Public Class RolesUsuarios
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

        Public Function GuardarRolesUsuarios(Usuario As Long, Rol As Integer) As Integer
            Try
                Try
                    Return SGContext.US_RolesUsuarios_Add(Usuario, Rol)
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

        Public Function ObtenerRolesUsuarios(ByVal Rol As Integer?, ByVal idUsuario As Int64?) As List(Of RolesUsuarios_Result)
            Try
                Try
                    Dim List As ObjectResult(Of RolesUsuarios_Result) = SGContext.US_RolesUsuarios_Get(Rol, idUsuario)
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

        Public Function obtenerRolesXUsuario(ByVal usuarioId As Int64, ByVal asignado As Boolean) As List(Of US_RolesXUsuario_Get_Result)
            Return SGContext.US_RolesXUsuario_Get(usuarioId, asignado).ToList
        End Function

        Public Sub eliminar(ByVal idUsuario As Int64, ByVal idRol As Int16)
            Dim usuario As New US_Usuarios With {.id = idUsuario}
            Dim rol As New US_Roles With {.id = idRol}
            usuario.US_Roles.Add(rol)
            SGContext.US_Usuarios.Attach(usuario)

            usuario.US_Roles.Remove(rol)
            SGContext.SaveChanges()
        End Sub
    End Class
End Namespace