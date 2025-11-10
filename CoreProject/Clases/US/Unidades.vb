

'Imports CoreProject.US_Model
Imports System.Data.SqlClient
Imports System.Data.Entity.Core.Objects

Namespace US
    Public Class Unidades
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

        Public Function GuardarUnidad(id As Integer, Unidad As String, GrupoUnidad As Integer) As Integer
            Try
                Try
                    Return SGContext.US_Unidades_Add(id, Unidad, GrupoUnidad)
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

        Public Function EditarUnidad(id As Integer, Unidad As String, GrupoUnidad As Integer) As Integer
            Try
                Try
                    Return SGContext.US_Unidades_Edit(id, Unidad, GrupoUnidad)
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

        Public Function EliminarUnidad(ByVal id As Integer) As Integer
            Try
                Try
                    Return SGContext.US_Unidades_Del(id)
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

        Public Function ObtenerUnidades(ByVal Unidad As String, ByVal GrupoUnidad As Integer?, ByVal idUnidad As Integer?) As List(Of Unidades_Result)
            Try
                Try
                    Dim List As ObjectResult(Of Unidades_Result) = SGContext.US_Unidades_Get(Unidad, GrupoUnidad, idUnidad)
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

        Public Function ObtenerNombreUnidad(ByVal unidad As Int32) As String
            Return SGContext.US_Unidades.Where(Function(x) x.id = unidad).FirstOrDefault.Unidad
        End Function


        Public Function ObtenerGrupoUnidadxUnidad(ByVal unidad As Int32) As Integer
            Return SGContext.US_Unidades.Where(Function(x) x.id = unidad).FirstOrDefault.GrupoUnidadId
        End Function

        Public Function ObtenerUnidadesAll() As List(Of US_Unidades_Get_All_Result)
            Try
                Try
                    Dim List As ObjectResult(Of US_Unidades_Get_All_Result) = SGContext.US_Unidades_Get_All()
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

        Public Function ObtenerUnidadCombo(ByVal GrupoUnidad As Integer) As List(Of UnidadesCombo_Result)
            Try
                Try
                    Dim List As ObjectResult(Of UnidadesCombo_Result) = SGContext.US_Unidades_GetCombo(GrupoUnidad)
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

        Public Function ObtenerUnidadCombo() As List(Of UnidadesCombo_Result)
            Try
                Try
                    Dim List As ObjectResult(Of UnidadesCombo_Result) = SGContext.US_TodasUnidades_GetCombo()
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

        Public Function ObtenerUnidadesxUsuario(ByVal idUsuario As Int64) As List(Of US_Unidades)
            Dim list As List(Of US_Unidades) = SGContext.US_Usuarios.Where(Function(x) x.id = idUsuario).SelectMany(Function(y) y.US_Unidades).ToList
            Dim list2 As List(Of US_Unidades) = list.Where(Function(x) x.US_GrupoUnidad.TipoGrupoUnidad = 1).ToList
            Return list2
        End Function

        Public Function ObtenerUnidadesTrafico() As List(Of US_Unidades)
            Dim list As List(Of US_Unidades) = SGContext.US_Unidades.Where(Function(x) x.Trafico = 1).ToList
            Return list
        End Function

        Public Function ObtenerUnidadesxUsuarioYTipoGrupo(ByVal idUsuario As Int64, ByVal TipoGrupoUnidad As Int16) As List(Of US_Unidades)
            Dim list = ObtenerUnidadesxUsuario(idUsuario)
            Return list.Where(Function(x) x.US_GrupoUnidad.TipoGrupoUnidad = TipoGrupoUnidad).ToList
        End Function

        Function obtenerUnidadesXTipoGrupoUnidad(ByVal tipoGrupoUnidadId As Int64) As List(Of US_Unidades)
            Return SGContext.US_Unidades.Where(Function(x) x.US_GrupoUnidad.TipoGrupoUnidad = tipoGrupoUnidadId).ToList
        End Function

        Function ObtenerUnidadXid(ByVal IdUnidad As Int64) As US_Unidades
            Return SGContext.US_Unidades.Where(Function(x) x.id = IdUnidad).FirstOrDefault
        End Function
    End Class

End Namespace