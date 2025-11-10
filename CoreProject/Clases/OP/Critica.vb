Imports System.Data.Objects
Imports System.Data.SqlClient

Namespace OP
    Public Class Critica
        Private _OPcontext As OP_Entities

        Public Property OPContext As OP_Entities
            Get
                If Me._OPcontext Is Nothing Then
                    Me._OPcontext = New OP_Entities()
                End If
                Return Me._OPcontext
            End Get
            Set(value As OP_Entities)
                Me._OPcontext = value
            End Set
        End Property

        Public Function EditarTraficoEncuestasCritica(id As Integer, cantidad As Integer, usuarioRecibe As Integer, unidadRecibe As Integer, unidadEnvia As Integer, fechaRecibo As DateTime, observacionesRecibo As String) As Integer
            Try
                Try
                    Return OPContext.OP_TraficoEncuestas_Edit_Critica(id, cantidad, usuarioRecibe, unidadRecibe, unidadEnvia, fechaRecibo, observacionesRecibo)
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

        Public Function ObtenerTraficoEncuestasCriticaXTrabajo(ByVal TrabajoID As Integer, ByVal UnidadID As Integer) As List(Of OP_TraficoEncuesta_GetCritica_Result)
            Try
                Try
                    Dim List As ObjectResult(Of OP_TraficoEncuesta_GetCritica_Result) = OPContext.OP_TraficoEncuesta_GetCritica(TrabajoID, UnidadID)
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