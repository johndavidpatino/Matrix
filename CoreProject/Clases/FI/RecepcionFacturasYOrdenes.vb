

'Imports CoreProject.MatrixModel
Namespace FI
    Public Class Facturas
#Region "Variables Globales"
        Private oMatrixContext As FI_Entities
#End Region
#Region "Constructores"
        'Constructor public 
        Public Sub New()
            oMatrixContext = New FI_Entities
        End Sub
#End Region
#Region "Obtener"
        Function ObtenerFacturasRadicadas(ByVal idOrden As Int64, idTipo As Int32) As List(Of FI_FacturasRadicadas)
            Return oMatrixContext.FI_FacturasRadicadas.Where(Function(x) x.IdOrden = idOrden AndAlso x.Tipo = idTipo).ToList
        End Function

        Function FacturasRadicadasPorEstado(ByVal Solicitante As Int64?, ByVal Estado As Int64?, ByVal FechaI As Date?, ByVal FechaF As Date?, ByVal tipoOrden As UShort?, ByVal IdOrden As Int64?, ByVal Consecutivo As Int64?, ByVal EstadoRechazada As Int64?) As List(Of FI_FacturasRadicadasGET_Result)
            Return oMatrixContext.FI_FacturasRadicadasGET(Solicitante, Estado, FechaI, FechaF, tipoOrden, IdOrden, Consecutivo, EstadoRechazada).ToList
        End Function

        Function ObtenerFactura(ByVal id As Int64) As FI_FacturasRadicadas
            Return oMatrixContext.FI_FacturasRadicadas.Where(Function(x) x.id = id).FirstOrDefault
        End Function
        Function obtenerEstadosFacturas(ByVal id As Integer?) As List(Of FI_EstadosFactura_Get_Result)
            Return oMatrixContext.FI_EstadosFactura_Get(id).ToList
        End Function
        Function obtenerUltimaEvaluacionProveedor(ByVal idProveedor As Int64) As Date?
            Return oMatrixContext.CO_EvaluacionProveedorUltimaFecha_Get(idProveedor).FirstOrDefault
        End Function
        Function obtenerFacturaRadicada(ByVal idOrden As Int64?, ByVal consecutivo As Int64?, ByVal ano As Int16) As List(Of FI_FacturasRadicadas_Get_Result)
            Return oMatrixContext.FI_FacturasRadicadas_Get(idOrden, consecutivo, ano).ToList
        End Function

        Function obtenerFacturaLlaveCompuesta(ByVal ValoresBusqueda As String) As List(Of FI_FacturasRadicadas_LlaveCompuesta_Get_Result)
            Return oMatrixContext.FI_FacturasRadicadas_LlaveCompuesta_Get(ValoresBusqueda).ToList
        End Function

        Function ObtenerFacturasRadicadasNew(ByVal Id As Int64?, ByVal Consecutivo As Int64?, ByVal TipoDocumento As Int16?, ByVal Escaneada As Boolean?, ByVal FechaInicio As Date?, ByVal FechaFIn As Date?) As List(Of FI_FacturasRadicadasNew_Get_Result)
            Return oMatrixContext.FI_FacturasRadicadasNew_Get(Id, Consecutivo, TipoDocumento, Escaneada, FechaInicio, FechaFIn).ToList
        End Function

        Function ObtenerFacturasRadicadasDetalle(ByVal idFactura As Int64?, ByVal solicitanteId As Int64?, ByVal evaluaProveedor As Int64?, ByVal estado As Int32?) As List(Of FI_FacturasRadicadas_Detalle_Get_Result)
            Return oMatrixContext.FI_FacturasRadicadas_Detalle_Get(idFactura, solicitanteId, evaluaProveedor, estado).ToList
        End Function

        Function ObtenerOrdenesxFacturasRadicadasDetalle(ByVal IdOrden As Int64?, ByVal TipoOrden As Int32?, ByVal Generica As Boolean?) As List(Of FI_FacturasRadicadas_Detalle_Ordenes_Result)
            Return oMatrixContext.FI_FacturasRadicadas_Detalle_Ordenes(IdOrden, TipoOrden, Generica).ToList
        End Function

        Function ObtenerLogAprobacionesFactura(ByVal idFactura As Int64) As List(Of FI_LogAprobacionFacturasUsuarios_Get_Result)
            Return oMatrixContext.FI_LogAprobacionFacturasUsuarios_Get(idFactura).ToList
        End Function

        Function ObtenerLogEvaluacionesProveedorFactura(ByVal idFactura As Int64) As List(Of FI_LogEvaluacionProveedorFacturas_Get_Result)
            Return oMatrixContext.FI_LogEvaluacionProveedorFacturas_Get(idFactura).ToList
        End Function

        Function FacturasPendientesAprobacion(Usuario As Int64, ByVal Consecutivo As Int64?) As List(Of FI_FacturasPendientesAprobacion_Get_Result)
            Return oMatrixContext.FI_FacturasPendientesAprobacion_Get(Usuario, Consecutivo).ToList
        End Function

        Function FacturasPendientesEvaluacion(Usuario As Int64) As List(Of FI_FacturasPendientesEvaluacion_Get_Result)
            Return oMatrixContext.FI_FacturasPendientesEvaluacion_Get(Usuario).ToList
        End Function

#End Region

#Region "Guardar"
        Sub GuardarFactura(ByRef e As FI_FacturasRadicadas)
            If e.id = 0 Then
                oMatrixContext.FI_FacturasRadicadas.Add(e)
            Else
                oMatrixContext.FI_FacturasRadicadas.Attach(e)
                oMatrixContext.Entry(e).State = Entity.EntityState.Modified
            End If
            oMatrixContext.SaveChanges()
        End Sub

        Sub GuardarLogFactura(ByRef e As FI_LogAprobacionFacturas)
            If e.id = 0 Then
                oMatrixContext.FI_LogAprobacionFacturas.Add(e)
            End If
            oMatrixContext.SaveChanges()
        End Sub

        Sub GuardarCalificacionEvaluacionProveedor(ByRef e As CO_EvaluacionProveedoresFactura)
            If e.id = 0 Then
                oMatrixContext.CO_EvaluacionProveedoresFactura.Add(e)
            End If
            oMatrixContext.SaveChanges()
        End Sub

        Sub GuardarFacturaDetalle(ByRef e As FI_FacturasRadicadas_Detalle)
            If e.Id = 0 Then
                oMatrixContext.FI_FacturasRadicadas_Detalle.Add(e)
            End If
            oMatrixContext.SaveChanges()
        End Sub

        Sub GrabarFacturaDetalleRequerimiento(ByRef e As FI_FacturasRadicadas_DetalleRequerimiento)
            If e.Id = 0 Then
                oMatrixContext.FI_FacturasRadicadas_DetalleRequerimiento.Add(e)
            End If
            oMatrixContext.SaveChanges()
        End Sub

        Sub GrabarUsuariosApruebanFactura(ByVal IdFactura As Int64, ByVal Usuario As Int64)
            oMatrixContext.FI_LogAprobacionFacturasUsuarios_Add(IdFactura, Usuario)
        End Sub

        Sub GrabarUsuariosEvaluanProveedorFactura(ByVal IdFactura As Int64, ByVal Usuario As Int64)
            oMatrixContext.FI_LogEvaluacionProveedorFacturas_Add(IdFactura, Usuario)
        End Sub

        Sub AprobarFactura(ByVal idFactura As Int64, comentarios As String, usuario As Int64, aprobacion As Boolean)
            oMatrixContext.FI_AprobarFactura(idFactura, comentarios, usuario, aprobacion)
        End Sub

        Sub EvaluarFactura(ByVal idFactura As Int64, comentarios As String, usuario As Int64, evaluacion As Boolean)
            oMatrixContext.FI_EvaluarFactura(idFactura, comentarios, usuario, evaluacion)
        End Sub

#End Region

#Region "Borrar"
        Sub borrarFactura(ByVal id As Int64)
            oMatrixContext.FI_FacturasRadicadas_Del(id)
        End Sub

        Sub borrarFacturaDetalle(ByVal idFactura As Int64)
            oMatrixContext.FI_FacturasRadicadas_Detalle_Del(idFactura)
        End Sub
        Sub borrarPersonaApruebaFactura(ByVal id As Int64)
            oMatrixContext.FI_LogAprobacionFacturasUsuarios_Del(id)
        End Sub

        Sub borrarLogsAprobacionFactura(ByVal idFactura As Int64)
            oMatrixContext.FI_LogAprobacion_FacturasUsuarios_Del(idFactura)
        End Sub

        Sub borrarPersonaEvaluaProveedorFactura(ByVal id As Int64)
            oMatrixContext.FI_LogEvaluacionProveedorFacturas_Del(id)
        End Sub

        Sub borrarLogsEvaluacionProveedorFactura(ByVal idFactura As Int64)
            oMatrixContext.FI_LogEvaluacionProveedor_Facturas_Del(idFactura)
        End Sub

#End Region
    End Class
End Namespace
