Imports System.Data.Objects
Imports System.Data.SqlClient

Namespace OP
    Public Class TraficoEncuestas
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

        Public Function ObtenerTraficoEncuestasXTrabajo(ByVal TrabajoID As Integer) As List(Of OP_TraficoEncuestasCiudad_Result)
            Try
                Try
                    Dim List As ObjectResult(Of OP_TraficoEncuestasCiudad_Result) = OPContext.OP_TraficoEncuestasCiudad(TrabajoID)
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