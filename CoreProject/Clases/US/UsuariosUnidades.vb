

'Imports CoreProject.US_Model
Imports System.Data.SqlClient
Imports System.Data.Entity.Core.Objects

Namespace US
    Public Class UsuariosUnidades
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

        Public Function GuardarUsuariosUnidades(Usuario As Long, Unidad As Integer) As Integer
            Try
                Try
                    Return SGContext.US_UsuariosUnidades_Add(Usuario, Unidad)
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

        Public Function EliminarUsuariosUnidades(ByVal id As Integer) As Integer
            Try
                Try
                    Return SGContext.US_UsuariosUnidades_Del(id)
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

        Public Function ObtenerUsuariosUnidades(ByVal Usuario As Int64) As List(Of UsuariosUnidades_Result)
            Try
                Try
                    Dim List As ObjectResult(Of UsuariosUnidades_Result) = SGContext.US_UsuariosUnidades_Get(Usuario)
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

        Public Sub eliminar(ByVal idUsuario As Int64, ByVal idUnidad As Int16)
            Dim usuario As New US_Usuarios With {.id = idUsuario}
            Dim unidad As New US_Unidades With {.id = idUnidad}
            usuario.US_Unidades.Add(unidad)
            SGContext.US_Usuarios.Attach(usuario)

            usuario.US_Unidades.Remove(unidad)
            SGContext.SaveChanges()
        End Sub

        Public Function obtenerUnidadesXUsuario(ByVal usuarioId As Int64, ByVal asignado As Boolean?, ByVal grupoUnidadId As Int16?) As List(Of US_UnidadesXUsuario_Get_Result)
            Return SGContext.US_UnidadesXUsuario_Get(usuarioId, asignado, grupoUnidadId).ToList
        End Function

        Public Function obtenerUsuariosXUnidadesXRol(ByVal unidades As String, ByVal rolId As Int64?, ByVal usuarioId As Int64?) As List(Of US_UsuariosXUnidadesXRol_Get_Result)
            Return SGContext.US_UsuariosXUnidadesXRol_Get(unidades, rolId, usuarioId).ToList
        End Function

        Public Function obtenerUsuariosGrupoUnidad(ByVal usuarioId As Int64?) As List(Of US_UsuariosGrupoUnidad_Get_Result)
            Return SGContext.US_UsuariosGrupoUnidad_Get(usuarioId).ToList
        End Function
    End Class
End Namespace