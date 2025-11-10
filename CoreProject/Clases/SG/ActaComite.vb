'Imports CoreProject.SG_Model
Imports System.Data.Entity.Core.Objects
Imports System.Data.SqlClient

Namespace SG
    Public Class ActasComite

        Private _sgcontext As SG_Entities

        Public Property SGContext As SG_Entities
            Get
                If Me._sgcontext Is Nothing Then
                    Me._sgcontext = New SG_Entities()
                End If
                Return Me._sgcontext
            End Get
            Set(value As SG_Entities)
                Me._sgcontext = value
            End Set
        End Property

        Public Function GuardarActaComite(unidadId As Integer, noActa As Integer, tipoReunionId As Integer, Contenedor As Integer, ContenedorId As Integer, ordenDia As String, conclusiones As String, descripcion As String, usuarioCrea As Integer, usuarioLidera As Integer) As Integer
            Try
                Try
                    Dim Res As ObjectResult(Of SG_ActaComite_Add_Result)
                    Res = SGContext.SG_ActaComite_Add(Date.Now, unidadId, noActa, tipoReunionId, Contenedor, ContenedorId, ordenDia, conclusiones, descripcion, usuarioCrea, usuarioLidera)
                    'devuelvo el ultimo id ingresado en la tabla de Actas
                    Return Res(0).Id
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

        Public Function EditarActaComite(id As Integer, unidadId As Integer, noActa As Integer, tipoReunionId As Integer, Contenedor As Integer, ContenedorId As Integer, ordenDia As String, conclusiones As String, descripcion As String, usuarioCrea As Integer, usuarioLidera As Integer) As Integer
            Try
                Try
                    Return SGContext.SG_ActaComite_Edit(id, unidadId, noActa, tipoReunionId, Contenedor, ContenedorId, ordenDia, conclusiones, descripcion, usuarioCrea, usuarioLidera)
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

        Public Function EliminarActaComite(ByVal id As Integer) As Integer
            Try
                Try
                    Return SGContext.SG_Acta_Del(id)
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

        Public Function ObtenerActasComite() As List(Of SG_ActaComite_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_ActaComite_Get_Result) = SGContext.SG_ActaComite_Get
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

        Public Function ObtenerActasComite(ByVal ActaId As Integer) As List(Of SG_ActaComite_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_ActaComite_Get_Result) = SGContext.SG_ActaComite_Get_Id(ActaId)
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

        Public Function ObtenerTipoReunion() As List(Of SG_TipoReunion_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_TipoReunion_Get_Result) = SGContext.SG_TipoReunion_Get()
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