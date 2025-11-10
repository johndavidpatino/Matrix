

'Imports CoreProject.OP_Model
Imports System.Data.SqlClient

Namespace OP
    Public Class Verificacion
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

        Public Function EditarTraficoEncuestasVerificacion(id As Integer, cantidad As Integer, usuarioRecibe As Integer, unidadRecibe As Integer, unidadEnvia As Integer, fechaRecibo As DateTime, observacionesRecibo As String, devolucion As Boolean, motivoDevolucion As String) As Integer
            Try
                Try
                    Return OPContext.OP_TraficoEncuestas_Edit_Verificacion(id, cantidad, usuarioRecibe, unidadRecibe, unidadEnvia, fechaRecibo, observacionesRecibo, devolucion, motivoDevolucion)
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