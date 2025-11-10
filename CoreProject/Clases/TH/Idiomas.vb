Imports System.Data.Entity.Core.Objects

'Imports CoreProject.TH_Model
'Imports CoreProject
Imports System.Data.SqlClient

Namespace TH
    Public Class Idiomas

        Private _THcontext As TH_Entities

        Public Property THContext As TH_Entities
            Get
                If Me._THcontext Is Nothing Then
                    Me._THcontext = New TH_Entities()
                End If
                Return Me._THcontext
            End Get
            Set(value As TH_Entities)
                Me._THcontext = value
            End Set
        End Property

        Public Function ObtenerIdiomasHVID(ByVal HojaVidaId As Integer) As List(Of TH_Idiomas_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of TH_Idiomas_Get_Result) = THContext.TH_Idiomas_Get(HojaVidaId)
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

        Public Function EliminarIdioma(ByVal id As Integer) As Integer
            Try
                Try
                    Return THContext.TH_Idiomas_Del(id)
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

        Public Function EditarIdiomas(Id As Integer, hojaVidaId As Integer, dominioId As Integer, lugar As String, idiomaId As Integer) As Integer
            Try
                Try
                    Return THContext.TH_Idiomas_Edit(Id, hojaVidaId, dominioId, lugar, idiomaId)
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

        Public Function AgregarIdiomas(hojaVidaId As Integer, dominioId As Integer, lugar As String, idiomaId As Integer) As Integer
            Try
                Try
                    Return THContext.TH_Idiomas_Add(hojaVidaId, dominioId, lugar, idiomaId)
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