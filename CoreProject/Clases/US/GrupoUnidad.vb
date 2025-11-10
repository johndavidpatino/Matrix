

'Imports CoreProject.US_Model
Imports System.Data.SqlClient
Imports System.Data.Entity.Core.Objects

Namespace US
    Public Class GrupoUnidad
        Private _sgcontext As US_Entities

        Enum ETiposGruposUnidad
            Comercial = 1
            Operativa = 2
            Administrativa = 3
            Apoyo = 4
        End Enum

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

        Public Function GuardarGrupoUnidad(GrupoUnidad As Integer, TipoGrupoUnidad As Integer, ByVal Activo As Boolean) As Integer
            Try
                Try
                    Return SGContext.US_GrupoUnidad_Add(GrupoUnidad, TipoGrupoUnidad, Activo)
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

        Public Function EditarGrupoUnidad(id As Integer, GrupoUnidad As Integer, TipoGrupoUnidad As Integer, ByVal Activo As Boolean) As Integer
            Try
                Try
                    Return SGContext.US_GrupoUnidad_Edit(id, GrupoUnidad, TipoGrupoUnidad, Activo)
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

        Public Function EliminarGrupoUnidad(ByVal id As Integer) As Integer
            Try
                Try
                    Return SGContext.US_GrupoUnidad_Del(id)
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

        Public Function ObtenerGrupoUnidad(ByVal GrupoUnidad As String, ByVal TipoGrupoUnidad As Integer) As List(Of GrupoUnidad_Result)
            Try
                Try
                    Dim List As ObjectResult(Of GrupoUnidad_Result) = SGContext.US_GrupoUnidad_Get(GrupoUnidad, TipoGrupoUnidad)
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

        Public Function ObtenerGrupoUnidadCombo(ByVal Tipo As ETiposGruposUnidad) As List(Of GrupoUnidadCombo_Result)
            Try
                Try
                    Dim List As ObjectResult(Of GrupoUnidadCombo_Result) = SGContext.US_GrupoUnidad_GetCombo(Tipo)
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