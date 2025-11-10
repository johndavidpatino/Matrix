

'Imports CoreProject.US_Model
Imports System.Data.SqlClient
Imports System.Data.Entity.Core.Objects

Namespace US
    Public Class Permisos
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

        Public Function GuardarPermiso(id As Integer, Permiso As String, GrupoPermiso As Integer) As Integer
            Try
                Try
                    Return SGContext.US_Permisos_Add(Permiso, GrupoPermiso)
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

        Public Function EditarPermiso(id As Integer, Permiso As String, GrupoPermiso As Integer) As Integer
            Try
                Try
                    Return SGContext.US_Permisos_Edit(id, Permiso, GrupoPermiso)
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

        Public Function EliminarPermiso(ByVal id As Integer) As Integer
            Try
                Try
                    Return SGContext.US_Permisos_Del(id)
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

        Public Function ObtenerPermisos(ByVal Permiso As String, ByVal GrupoPermiso As Integer) As List(Of Permisos_Result)
            Try
                Try
                    Dim List As ObjectResult(Of Permisos_Result) = SGContext.US_Permisos_Get(Permiso, GrupoPermiso)
                    Return List.ToList
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

        Public Function ObtenerPermisoCombo(ByVal GrupoPermiso As Integer) As List(Of PermisosCombo_Result)
            Try
                Try
                    Dim List As ObjectResult(Of PermisosCombo_Result) = SGContext.US_Permisos_GetCombo(GrupoPermiso)
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
        Public Function ValidarPermiso(ByVal Permiso As String, ByVal GrupoPermiso As Integer) As Boolean
            Try
                Try

                    Dim List As ObjectResult(Of Permisos_Result) = SGContext.US_Permisos_Get(Permiso, GrupoPermiso)
                    If List.Count > 0 Then
                        Return True
                    Else
                        Return False
                    End If
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