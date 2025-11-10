

'Imports CoreProject.CU_Model
Imports System.Data.SqlClient
Imports System.Data.Entity.Core.Objects

Namespace CU
    Public Class ProductoInterno

        Private _sgcontext As CU_Entities

        Public Property CUContext As CU_Entities
            Get
                If Me._sgcontext Is Nothing Then
                    Me._sgcontext = New CU_Entities()
                End If
                Return Me._sgcontext
            End Get
            Set(value As CU_Entities)
                Me._sgcontext = value
            End Set
        End Property

        Public Function GuardarProductoInterno(ByVal ProyectoId As Integer, ByVal FechaEnvio As DateTime, ByVal UnidadEnvia As Integer, ByVal UnidadRecibe As Integer, ByVal Tipo As Integer, ByVal Producto As String, ByVal Descripcion As String, ByVal Cantidad As Double, ByVal Envia As Integer, ByVal Recibe As Integer, ByVal FechaRecepcion As DateTime, ByVal Observaciones As String) As Integer
            Try
                Try
                    Return CUContext.CU_ProductoInterno_Add(ProyectoId, FechaEnvio, UnidadEnvia, UnidadRecibe, Tipo, Producto, Descripcion, Cantidad, Envia, Recibe, FechaRecepcion, Observaciones)
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

        Public Function EditarProductoInterno(ByVal Id As Integer, ByVal ProyectoId As Integer, ByVal FechaEnvio As DateTime, ByVal UnidadEnvia As Integer, ByVal UnidadRecibe As Integer, ByVal Tipo As Integer, ByVal Producto As String, ByVal Descripcion As String, ByVal Cantidad As Double, ByVal Envia As Integer, ByVal Recibe As Integer, ByVal FechaRecepcion As DateTime, ByVal Observaciones As String) As Integer
            Try
                Try
                    Return CUContext.CU_ProductoInterno_Edit(Id, ProyectoId, FechaEnvio, UnidadEnvia, UnidadRecibe, Tipo, Producto, Descripcion, Cantidad, Envia, Recibe, FechaRecepcion, Observaciones)
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

        Public Function EditarCantProductoInterno(ByVal Id As Integer, ByVal Cantidad As Double) As Integer
            Try
                Try
                    Return CUContext.CU_ProductoInterno_EditCant(Id, Cantidad)
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

        Public Function EliminarProductoInterno(ByVal id As Integer) As Integer
            Try
                Try
                    Return CUContext.CU_ProductoInterno_Del(id)
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

        Public Function ObtenerProductoInterno() As List(Of CU_ProductoInterno_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of CU_ProductoInterno_Get_Result) = CUContext.CU_ProductoInterno_Get
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

        Public Function ObtenerProductoInternoRecibe(ByVal IdUsuario As Integer, ByVal IdProyecto As Integer) As List(Of CU_ProductoInterno_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of CU_ProductoInterno_Get_Result) = CUContext.CU_ProductoInterno_GetRecibe(IdUsuario, IdProyecto)
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

        Public Function ObtenerProductoInternoEnvia(ByVal IdUsuario As Integer, ByVal IdProyecto As Integer) As List(Of CU_ProductoInterno_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of CU_ProductoInterno_Get_Result) = CUContext.CU_ProductoInterno_GetEnvia(IdUsuario, IdProyecto)
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

        Public Function ObtenerTipoMovimiento() As List(Of CU_TipoMovimientoCombo_Result)
            Try
                Try
                    Dim List As ObjectResult(Of CU_TipoMovimientoCombo_Result) = CUContext.CU_TipoMovimientoProductoInterno_GetCombo
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