
'Imports CoreProject.PY_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class EntregasTrabajos
#Region "Variables Globales"
    Private oMatrixContext As PY_Entities
#End Region
#Region "Constructors"
    Public Sub New()
        oMatrixContext = New PY_Entities
    End Sub
#End Region

#Region "Obtener"
    Private Function GetEntregaTrabajoCuanti(ByVal FichaId As Long) As List(Of PY_Trabajo_Entrega_Cuantitativo_Result)
        Try
            Dim oResult As ObjectResult(Of PY_Trabajo_Entrega_Cuantitativo_Result) = oMatrixContext.PY_Trabajo_Entrega_Cuantitativo(FichaId)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw (ex)
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverPreEntregaxTrabajo(ByVal FichaId As Long)
        Try
            Dim oResult As List(Of PY_Trabajo_Entrega_Cuantitativo_Result)
            oResult = GetEntregaTrabajoCuanti(FichaId)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Private Function GetEntregaTrabajoCuali(ByVal TrabajoId As Long) As List(Of PY_Trabajo_Entrega_Cualitativo_Result)
        Try
            Dim oResult As ObjectResult(Of PY_Trabajo_Entrega_Cualitativo_Result) = oMatrixContext.PY_Trabajo_Entrega_Cualitativo(TrabajoId)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw (ex)
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverPreEntregaxTrabajoCuali(ByVal TrabajoId As Long)
        Try
            Dim oResult As List(Of PY_Trabajo_Entrega_Cualitativo_Result)
            oResult = GetEntregaTrabajoCuali(TrabajoId)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Private Function GetEntregaTrabajoSesiones(ByVal FichaId As Long) As List(Of PY_Trabajo_Entrega_Cuali_Sesiones_Result)
        Try
            Dim oResult As ObjectResult(Of PY_Trabajo_Entrega_Cuali_Sesiones_Result) = oMatrixContext.PY_Trabajo_Entrega_Cuali_Sesiones(FichaId)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw (ex)
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverPreEntregaxSesiones(ByVal FichaId As Long)
        Try
            Dim oResult As List(Of PY_Trabajo_Entrega_Cuali_Sesiones_Result)
            oResult = GetEntregaTrabajoSesiones(FichaId)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Private Function GetEntregaTrabajoEntrevistas(ByVal FichaId As Long) As List(Of PY_Trabajo_Entrega_Cuali_Entrevistas_Result)
        Try
            Dim oResult As ObjectResult(Of PY_Trabajo_Entrega_Cuali_Entrevistas_Result) = oMatrixContext.PY_Trabajo_Entrega_Cuali_Entrevistas(FichaId)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw (ex)
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverPreEntregaxEntrevistas(ByVal FichaId As Long)
        Try
            Dim oResult As List(Of PY_Trabajo_Entrega_Cuali_Entrevistas_Result)
            oResult = GetEntregaTrabajoEntrevistas(FichaId)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Private Function GetEntregaTrabajoObservaciones(ByVal FichaId As Long) As List(Of PY_Trabajo_Entrega_Cuali_Observaciones_Result)
        Try
            Dim oResult As ObjectResult(Of PY_Trabajo_Entrega_Cuali_Observaciones_Result) = oMatrixContext.PY_Trabajo_Entrega_Cuali_Observaciones(FichaId)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw (ex)
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverPreEntregaxObservaciones(ByVal FichaId As Long)
        Try
            Dim oResult As List(Of PY_Trabajo_Entrega_Cuali_Observaciones_Result)
            oResult = GetEntregaTrabajoObservaciones(FichaId)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
#End Region

End Class
