

'Imports CoreProject.US_Model
Imports System.Data.SqlClient
Imports System.Data.Entity.Core.Objects

Namespace US
    Public Class GrupoPermisos
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

        Public Function GuardarGrupoPermiso(id As Integer, GrupoPermiso As String) As Integer
            Try
                Try
                    Return SGContext.US_GruposPermisos_Add(id, GrupoPermiso)
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

        Public Function EditarGrupoPermiso(id As Integer, GrupoPermiso As String) As Integer
            Try
                Try
                    Return SGContext.US_GruposPermisos_Edit(id, GrupoPermiso)
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

        Public Function EliminarGrupoPermiso(ByVal id As Integer) As Integer
            Try
                Try
                    Return SGContext.US_GruposPermisos_Del(id)
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

        Public Function ObtenerGrupoPermiso(ByVal GrupoPermiso As String) As List(Of GruposPermisos_Result)
            Try
                Try
                    Dim List As ObjectResult(Of GruposPermisos_Result) = SGContext.US_GruposPermisos_Get(GrupoPermiso)
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

        Public Function ObtenerGrupoPermisoCombo() As List(Of GruposPermisosCombo_Result)
            Try
                Try
                    Dim List As ObjectResult(Of GruposPermisosCombo_Result) = SGContext.US_GruposPermisos_GetCombo()
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