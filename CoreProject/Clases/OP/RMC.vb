Imports System.Data.Objects
Imports System.Data.SqlClient

Namespace OP
    Public Class RMC
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

        Public Function AgregarTraficoEncuestasRMC(trabajoId As Integer, ciudad As Integer, cantidad As Integer, usuEnvia As Integer, unidadEnvia As Integer, unidadRecibe As Integer, fechaEnvio As DateTime, observacionesEnvio As String) As Integer
            Try
                Try
                    Return OPContext.OP_TraficoEncuestas_Add_RMC(trabajoId, ciudad, cantidad, usuEnvia, unidadEnvia, unidadRecibe, fechaEnvio, observacionesEnvio)
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

        Public Function ObtenerTraficoEncuestasRMCXTrabajo(ByVal TrabajoID As Integer) As List(Of OP_TraficoEncuesta_GetRMC_Result)
            Try
                Try
                    Dim List As ObjectResult(Of OP_TraficoEncuesta_GetRMC_Result) = OPContext.OP_TraficoEncuesta_GetRMC(TrabajoID)
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