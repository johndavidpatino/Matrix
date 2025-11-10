

'Imports CoreProject.US_Model
Imports System.Data.SqlClient
Imports System.Data.Entity.Core.Objects

Namespace US
    Public Class TipoGrupoUnidad

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

        Public Function GuardarTipoGrupoUnidad(id As Integer, TipoGrupoUnidad As String) As Integer
            Try
                Try
                    Return SGContext.US_TipoGrupoUnidad_Add(id, TipoGrupoUnidad)
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

        Public Function EditarTipoGrupoUnidad(id As Integer, TipoGrupoUnidad As String) As Integer
            Try
                Try
                    Return SGContext.US_TipoGrupoUnidad_Edit(id, TipoGrupoUnidad)
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

        Public Function EliminarTipoGrupoUnidad(ByVal id As Integer) As Integer
            Try
                Try
                    Return SGContext.US_TipoGrupoUnidad_Del(id)
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

        Public Function ObtenerTipoGrupoUnidad() As List(Of US_TipoGrupoUnidad_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of US_TipoGrupoUnidad_Get_Result) = SGContext.US_TipoGrupoUnidad_Get
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

        Public Function ObtenerTipoGrupoUnidadNombre(ByVal Nombre As String) As List(Of US_TipoGrupoUnidad_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of US_TipoGrupoUnidad_Get_Result) = SGContext.US_TipoGrupoUnidad_Get_Nombre(Nombre)
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

        Public Function ObtenerTipoGrupoUnidadCombo() As List(Of TipoGrupoUnidadCombo_Result)
            Try
                Try
                    Dim List As ObjectResult(Of TipoGrupoUnidadCombo_Result) = SGContext.US_TipoGrupoUnidad_GetCombo()
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