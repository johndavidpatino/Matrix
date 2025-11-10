

'Imports CoreProject.SG_Model
Imports System.Data.SqlClient
Imports System.Data.Entity.Core.Objects

Namespace SG
    Public Class Seguimientos

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

        Public Function GuardarSeguimiento(id As Integer, TareaId As Integer, Seguimiento As String, Usuario As Integer, CierraTarea As Boolean, fechaSeguimiento As DateTime) As Integer
            Try
                Try
                    Return SGContext.SG_Seguimiento_Add(id, TareaId, Seguimiento, Usuario, CierraTarea, fechaSeguimiento)
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

        Public Function EditarSeguimiento(id As Integer, TareaId As Integer, Seguimiento As String, Usuario As Integer, CierraTarea As Boolean, ByVal fechaSeguimiento As DateTime) As Integer
            Try
                Try
                    Return SGContext.SG_Seguimiento_Edit(id, TareaId, Seguimiento, Usuario, CierraTarea, fechaSeguimiento)
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

        Public Function EliminarSeguimiento(ByVal id As Integer) As Integer
            Try
                Try
                    Return SGContext.SG_Seguimiento_Del(id)
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

        Public Function ObtenerSeguimientos() As List(Of SG_Seguimiento_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_Seguimiento_Result) = SGContext.SG_Seguimiento_Get
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

        Public Function ConsultarSeguimientosXTarea(ByVal TareaId As Integer) As List(Of SG_Seguimiento_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_Seguimiento_Result) = SGContext.SG_Seguimiento_Get_TareaId(TareaId)
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

        Public Function UltimoId() As Integer
            Try
                Try
                    Dim Res As ObjectResult(Of SeguimientoUltimo_Id_Result)
                    Res = SGContext.SG_Seguimiento_Ultimo_Id()
                    'devuelvo el siguiente id
                    Return Res(0).MaxId + 1
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