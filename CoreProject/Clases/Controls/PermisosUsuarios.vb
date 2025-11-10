
Imports System.Data.Entity.Core.Objects
Imports System.Data.SqlClient

Namespace Datos
    Public Class ClsPermisosUsuarios
        Public Function VerificarPermisoUsuario(ByVal PermisoId As Long, ByVal UsuarioId As Long) As Boolean
            Dim PermisoUsuario = New US_Entities
            If PermisoUsuario.VerificarPermisoUsuario(PermisoId, UsuarioId).First.Cantidad > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function VerificarRolUsuario(ByVal RolId As Long, ByVal UsuarioId As Long) As Boolean
            Dim PermisoUsuario = New US_Entities
            Dim result As Integer? = 0
            result = PermisoUsuario.VerificarRolUsuario(RolId, UsuarioId)(0).Value
            If result > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function ConsultarUsuariosXNombre(ByVal Nombre As String) As List(Of Usuarios_Result)
            Dim PermisoUsuario = New US_Entities
            Try
                Try
                    Dim List As ObjectResult(Of Usuarios_Result) = PermisoUsuario.ConsultarUsuariosXNombre(Nombre)
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

        Public Function ConsultarUsuariosXUnidades(ByVal UnidadId As Integer) As List(Of Usuarios_Result)
            Dim PermisoUsuario = New US_Entities
            Try
                Try
                    Dim List As ObjectResult(Of Usuarios_Result) = PermisoUsuario.ConsultarUsuariosXUnidades(UnidadId)
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

        Public Function ConsultarUsuariosXGrupoUnidad(ByVal GrupoUnidadId As Integer) As List(Of Usuarios_Result)
            Dim PermisoUsuario = New US_Entities
            Try
                Try
                    Dim List As ObjectResult(Of Usuarios_Result) = PermisoUsuario.ConsultarUsuariosXGrupoUnidad(GrupoUnidadId)
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

        Public Function ConsultarUsuariosXRolesUsuarios(ByVal RolId As Integer) As List(Of Usuarios_Result)
            Dim PermisoUsuario = New US_Entities
            Try
                Try
                    Dim List As ObjectResult(Of Usuarios_Result) = PermisoUsuario.ConsultarUsuariosXRolesUsuarios(RolId)
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

        Public Function ObtenerUsuarios() As List(Of ObtenerUsuarios_Result)
            Dim PermisoUsuario = New US_Entities
            Try
                Try
                    Dim List As ObjectResult(Of ObtenerUsuarios_Result) = PermisoUsuario.ObtenerUsuarios
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