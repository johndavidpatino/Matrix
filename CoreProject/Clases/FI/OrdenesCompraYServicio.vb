

'Imports CoreProject.MatrixModel
Namespace FI
    Public Class Ordenes
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
        Function ObtenerContratistas(ByVal identificacion As Int64?, ByVal nombre As String, ByVal activo As Boolean?) As List(Of TH_ContratistasGet_Result)
            Return oMatrixContext.TH_ContratistasGet(identificacion, nombre, activo).ToList
        End Function

        Function ObtenerJBE_JBI_CC(ByVal TipoCarga As Int32, ByVal valorBusqueda As String) As List(Of FI_JBE_JBI_CC_Get_Result)
            Return oMatrixContext.FI_JBE_JBI_CC_Get(TipoCarga, valorBusqueda).ToList
        End Function

        Function ObtenerOrdenServicio(ByVal id As Int64) As FI_OrdenServicio
            Return oMatrixContext.FI_OrdenServicio.Where(Function(x) x.id = id).FirstOrDefault
        End Function

        Function ObtenerOrdenCompra(ByVal id As Int64) As FI_OrdenCompra
            Return oMatrixContext.FI_OrdenCompra.Where(Function(x) x.id = id).FirstOrDefault
        End Function

        Function ObtenerDetalleOC(ByVal id As Int64) As List(Of FI_OrdenCompradetalle)
            Return oMatrixContext.FI_OrdenCompradetalle.Where(Function(x) x.IdOrden = id).ToList
        End Function

        Function ObtenerDetalleOS(ByVal id As Int64) As List(Of FI_OrdenServiciodetalle)
            Return oMatrixContext.FI_OrdenServiciodetalle.Where(Function(x) x.IdOrden = id).ToList
        End Function

        Function ObtenerDetalleOC1(ByVal id As Int64) As FI_OrdenCompradetalle
            Return oMatrixContext.FI_OrdenCompradetalle.Where(Function(x) x.id = id).FirstOrDefault
        End Function

        Function ObtenerDetalleOS1(ByVal id As Int64) As FI_OrdenServiciodetalle
            Return oMatrixContext.FI_OrdenServiciodetalle.Where(Function(x) x.id = id).FirstOrDefault
        End Function

        Function ObtenerOrdenesDeCompra(ByVal id As Int64?, Fecha As Date?, Proveedor As Int64?, SolicitadoPor As Int64?, ElaboradoPor As Int64?, JobBook As String, CC As Int32?, Estado As Int32?) As List(Of FI_OrdenesDeCompra_Get_Result)
            Return oMatrixContext.FI_OrdenesDeCompra_Get(id, Fecha, Proveedor, SolicitadoPor, ElaboradoPor, JobBook, CC, Estado).ToList
        End Function

        Function ObtenerOrdenesDeServicio(ByVal id As Int64?, Fecha As Date?, Proveedor As Int64?, SolicitadoPor As Int64?, ElaboradoPor As Int64?, JobBook As String, CC As Int32?, Estado As Int32?) As List(Of FI_OrdenesDeServicio_Get_Result)
            Return oMatrixContext.FI_OrdenesDeServicio_Get(id, Fecha, Proveedor, SolicitadoPor, ElaboradoPor, JobBook, CC, Estado).ToList
        End Function

        Function ObtenerLogAprobacionesOrdenServicio(ByVal idOrden As Int64) As List(Of FI_LogAprobacionOrdenServicio_get_Result)
            Return oMatrixContext.FI_LogAprobacionOrdenServicio_get(idOrden).ToList
        End Function

        Function ObtenerLogAprobacionesOrdenCompra(ByVal idOrden As Int64) As List(Of FI_LogAprobacionOrdenCompra_get_Result)
            Return oMatrixContext.FI_LogAprobacionOrdenCompra_get(idOrden).ToList
        End Function

        Function ObtenerObservacionAprobacionesOrdenServicio(ByVal idOrden As Int64) As List(Of FI_ObservacionesLogAprobacionOrdenServicio_Result)
            Return oMatrixContext.FI_ObservacionesLogAprobacionOrdenServicio(idOrden).ToList
        End Function

        Function ObtenerObservacionesAprobacionesOrdenCompra(ByVal idOrden As Int64) As List(Of FI_ObservacionesLogAprobacionOrdenCompra_Result)
            Return oMatrixContext.FI_ObservacionesLogAprobacionOrdenCompra(idOrden).ToList
        End Function

        Function ObtenerUsuarioAprobacionOS(ByVal id As Int64, usuario As Int64) As Boolean
            Return oMatrixContext.FI_UsuarioAprobacionOrdenServicio(id, usuario)(0).Value
        End Function

        Function ObtenerUsuarioAprobacionOC(ByVal id As Int64, usuario As Int64) As Boolean
            Return oMatrixContext.FI_UsuarioAprobacionOrdenCompra(id, usuario)(0).Value
        End Function

        Function OrdenesServicioPendientesAprobacion(Usuario As Int64) As List(Of FI_OrdenesDeServicioPendientesAprobacion_Get_Result)
            Return oMatrixContext.FI_OrdenesDeServicioPendientesAprobacion_Get(Usuario).ToList
        End Function

        Function OrdenesCompraPendientesAprobacion(Usuario As Int64) As List(Of FI_OrdenesDeCompraPendientesAprobacion_Get_Result)
            Return oMatrixContext.FI_OrdenesDeCompraPendientesAprobacion_Get(Usuario).ToList
        End Function

        Function OrdenesRequerimientoPendientesAprobacion(Usuario As Int64) As List(Of FI_OrdenesDeRequerimientoPendientesAprobacion_Get_Result)
            Return oMatrixContext.FI_OrdenesDeRequerimientoPendientesAprobacion_Get(Usuario).ToList
        End Function

        Function obtenerDetalle(ByVal tipoOrden As Byte, ByVal idOrden As Long) As List(Of FI_OrdenDetalle_Get_Result)
            Return oMatrixContext.FI_OrdenDetalle_Get(tipoOrden, idOrden).ToList
        End Function
        Function obtenerUsuariosAprueban(ByVal tipo As Integer?, ByVal grupoUnidad As Integer?) As List(Of FI_NivelesAprobacion_Get_Result)
            Return oMatrixContext.FI_NivelesAprobacion_Get(tipo, grupoUnidad).ToList
        End Function

        Function obtenerUsuariosApruebanRequerimiento(ByVal idTrabajo As Int64) As List(Of FI_ObtenerUsuarioAprobacionRequerimiento_Result)
            Return oMatrixContext.FI_ObtenerUsuarioAprobacionRequerimiento(idTrabajo).ToList
        End Function

        Function obtenerCentroCostoXId(ByVal id As Integer) As FI_CentroCosto_Get_Result
            Return oMatrixContext.FI_CentroCosto_Get(id).FirstOrDefault
        End Function

        Function obtenerCentroCosto() As List(Of FI_CentroCosto_Get_Result)
            Return oMatrixContext.FI_CentroCosto_Get(Nothing).ToList
        End Function
        Function obtenerCentroCostos(ByVal id As Int64?, nombre As String) As List(Of FI_CentroCostos_Get_Result)
            Return oMatrixContext.FI_CentroCostos_Get(id, nombre).ToList
        End Function
        Function ObtenerOrdenRequerimiento(ByVal id As Int64) As FI_OrdenRequerimiento
            Return oMatrixContext.FI_OrdenRequerimiento.Where(Function(x) x.id = id).FirstOrDefault
        End Function
        Function ObtenerDetalleOR(ByVal id As Int64) As List(Of FI_OrdenRequerimientodetalle)
            Return oMatrixContext.FI_OrdenRequerimientodetalle.Where(Function(x) x.IdOrden = id).ToList
        End Function
        Function ObtenerDetalleOR1(ByVal id As Int64) As FI_OrdenRequerimientodetalle
            Return oMatrixContext.FI_OrdenRequerimientodetalle.Where(Function(x) x.id = id).FirstOrDefault
        End Function
        Function ObtenerOrdenesDeRequerimiento(ByVal id As Int64?, Fecha As Date?, Proveedor As Int64?, SolicitadoPor As Int64?, ElaboradoPor As Int64?, JobBook As String, CC As Int32?, Estado As Int32?) As List(Of FI_OrdenesDeRequerimiento_Get_Result)
            Return oMatrixContext.FI_OrdenesDeRequerimiento_Get(id, Fecha, Proveedor, SolicitadoPor, ElaboradoPor, JobBook, CC, Estado).ToList
        End Function
        Function ObtenerLogAprobacionesOrdenRequerimiento(ByVal idOrden As Int64) As List(Of FI_LogAprobacionOrdenRequerimiento_get_Result)
            Return oMatrixContext.FI_LogAprobacionOrdenRequerimiento_get(idOrden).ToList
        End Function
        Function ObtenerLogAprobacionesRadicacionOrdenRequerimiento(Orden As Nullable(Of Long), tipoOrden As Nullable(Of Byte)) As List(Of FI_AprobacionFacturasRadicadasXUsuarioXOrden_Result)
            Return oMatrixContext.FI_AprobacionFacturasRadicadasXUsuarioXOrden(Orden, Nothing, tipoOrden).ToList
        End Function
        Function ObtenerObservacionesAprobacionesOrdenRequerimiento(ByVal idOrden As Int64) As List(Of FI_ObservacionesLogAprobacionOrdenRequerimiento_Result)
            Return oMatrixContext.FI_ObservacionesLogAprobacionOrdenRequerimiento(idOrden).ToList
        End Function
        Function ObtenerUsuarioAprobacionOR(ByVal id As Int64, usuario As Int64) As Boolean
            Return oMatrixContext.FI_UsuarioAprobacionOrdenRequerimiento(id, usuario)(0).Value
        End Function
        Function ObtenerTiposPagoRequerimiento() As List(Of FI_TipoPagoRequerimiento)
            Return oMatrixContext.FI_TipoPagoRequerimiento.Where(Function(x) x.Habilitado = 1).ToList
        End Function
#End Region

#Region "Guardar"
        Sub GuardarOrdenServicio(ByRef oS As FI_OrdenServicio)
            If oS.id = 0 Then
                oMatrixContext.FI_OrdenServicio.Add(oS)
            End If
            oMatrixContext.SaveChanges()
        End Sub

        Sub GuardarOrdenCompra(ByRef oC As FI_OrdenCompra)
            If oC.id = 0 Then
                oMatrixContext.FI_OrdenCompra.Add(oC)
            End If
            oMatrixContext.SaveChanges()
        End Sub

        Sub GuardarOrdenCompraDetalle(ByRef oC As FI_OrdenCompradetalle)
            If oC.id = 0 Then
                oMatrixContext.FI_OrdenCompradetalle.Add(oC)
            End If
            oMatrixContext.SaveChanges()
        End Sub

        Sub GuardarOrdenServicioDetalle(ByRef oS As FI_OrdenServiciodetalle)
            If oS.id = 0 Then
                oMatrixContext.FI_OrdenServiciodetalle.Add(oS)
            End If
            oMatrixContext.SaveChanges()
        End Sub

        Function OrdenCompraDetalleList(ByVal idOrden As Int64) As List(Of FI_OrdenCompradetalle)
            Return oMatrixContext.FI_OrdenCompradetalle.Where(Function(x) x.IdOrden = idOrden).ToList
        End Function

        Function OrdenServicioDetalleList(ByVal idOrden As Int64) As List(Of FI_OrdenServiciodetalle)
            Return oMatrixContext.FI_OrdenServiciodetalle.Where(Function(x) x.IdOrden = idOrden).ToList
        End Function

        Sub EnviarAprobacionOrdenCompra(ByVal e As FI_LogAprobacionOrdenesCompra)
            oMatrixContext.FI_LogAprobacionOrdenesCompra.Add(e)
            oMatrixContext.SaveChanges()
        End Sub

        Sub EnviarAprobacionOrdenServicio(ByVal e As FI_LogAprobacionOrdenesServicio)
            oMatrixContext.FI_LogAprobacionOrdenesServicio.Add(e)
            oMatrixContext.SaveChanges()
        End Sub

        Sub ContinuarAprobacionOrdenServicio(ByVal id As Int64)
            oMatrixContext.FI_ContinuarAprobacionServicio(id)
        End Sub

        Sub ContinuarAprobacionOrdenCompra(ByVal id As Int64)
            oMatrixContext.FI_ContinuarAprobacionCompra(id)
        End Sub

        Sub AprobarOC(ByVal idOrden As Int64, comentarios As String, usuario As Int64, aprobacion As Boolean)
            oMatrixContext.FI_AprobarOC(idOrden, comentarios, usuario, aprobacion)
        End Sub

        Sub AprobarOS(ByVal idOrden As Int64, comentarios As String, usuario As Int64, aprobacion As Boolean)
            oMatrixContext.FI_AprobarOS(idOrden, comentarios, usuario, aprobacion)
        End Sub

        Sub GrabarUsuarioApruebaOrdenServicio(ByVal IdOrden As Int64, ByVal Usuario As Int64)
            oMatrixContext.FI_LogAprobacionOrdenServicio_Add(IdOrden, Usuario)
        End Sub

        Sub GrabarUsuarioApruebaOrdenCompra(ByVal IdOrden As Int64, ByVal Usuario As Int64)
            oMatrixContext.FI_LogAprobacionOrdenCompra_Add(IdOrden, Usuario)
        End Sub

        Function grabarLogOrdenes(ByVal tipoOrden As Byte, ByVal idOrden As Int64, ByVal estado As Byte, ByVal Observacion As String, ByVal Fecha As DateTime, ByVal Usuario As Int64) As Int64
            Return oMatrixContext.FI_LogOrdenes_Add(tipoOrden, idOrden, estado, Observacion, Fecha, Usuario).FirstOrDefault
        End Function

        Sub GuardarDetalleOrdenCompraDuplicada(ByVal Id As Int64, ByVal IdNuevo As Int64)
            oMatrixContext.FI_DuplicarDetalleOrdenCompra(Id, IdNuevo)
        End Sub

        Sub GuardarDetalleOrdenServicioDuplicada(ByVal Id As Int64, ByVal IdNuevo As Int64)
            oMatrixContext.FI_DuplicarDetalleOrdenServicio(Id, IdNuevo)
        End Sub

        Sub GuardarOrdenRequerimiento(ByRef oReq As FI_OrdenRequerimiento)
            If oReq.id = 0 Then
                oMatrixContext.FI_OrdenRequerimiento.Add(oReq)
            End If
            oMatrixContext.SaveChanges()
        End Sub

        Sub GuardarOrdenRequerimientoDetalle(ByRef oReq As FI_OrdenRequerimientodetalle)
            If oReq.id = 0 Then
                oMatrixContext.FI_OrdenRequerimientodetalle.Add(oReq)
            End If
            oMatrixContext.SaveChanges()
        End Sub

        Function OrdenRequerimientoDetalleList(ByVal idOrden As Int64) As List(Of FI_OrdenRequerimientodetalle)
            Return oMatrixContext.FI_OrdenRequerimientodetalle.Where(Function(x) x.IdOrden = idOrden).ToList
        End Function

        Sub EnviarAprobacionOrdenRequerimiento(ByVal e As FI_LogAprobacionOrdenesRequerimiento)
            oMatrixContext.FI_LogAprobacionOrdenesRequerimiento.Add(e)
            oMatrixContext.SaveChanges()
        End Sub

        Sub ContinuarAprobacionOrdenRequerimiento(ByVal id As Int64)
            oMatrixContext.FI_ContinuarAprobacionRequerimiento(id)
        End Sub

        Sub AprobarOR(ByVal idOrden As Int64, comentarios As String, usuario As Int64, aprobacion As Boolean)
            oMatrixContext.FI_AprobarOR(idOrden, comentarios, usuario, aprobacion)
        End Sub

        Sub GrabarUsuarioApruebaOrdenRequerimiento(ByVal IdOrden As Int64, ByVal Usuario As Int64)
            oMatrixContext.FI_LogAprobacionOrdenRequerimiento_Add(IdOrden, Usuario)
        End Sub

        Sub GuardarDetalleOrdenRequerimientoDuplicada(ByVal Id As Int64, ByVal IdNuevo As Int64)
            oMatrixContext.FI_DuplicarDetalleOrdenRequerimiento(Id, IdNuevo)
        End Sub
#End Region

#Region "Borrar"
        Sub BorrarDetalleOC(ByVal e As FI_OrdenCompradetalle)
            oMatrixContext.FI_OrdenCompradetalle.Remove(e)
            oMatrixContext.SaveChanges()
        End Sub
        Sub BorrarDetalleOS(ByVal e As FI_OrdenServiciodetalle)
            oMatrixContext.FI_OrdenServiciodetalle.Remove(e)
            oMatrixContext.SaveChanges()
        End Sub
        Sub borrarPersonaApruebaOrdenCompra(ByVal id As Int64)
            oMatrixContext.FI_LogAprobacionOrdenCompra_Del(id)
        End Sub
        Sub borrarPersonaApruebaOrdenServicio(ByVal id As Int64)
            oMatrixContext.FI_LogAprobacionOrdenServicio_Del(id)
        End Sub
        Sub borrarLogsAprobacionOrdenCompra(ByVal idOrden As Int64)
            oMatrixContext.FI_LogAprobacion_OrdenCompra_Del(idOrden)
        End Sub
        Sub borrarLogsAprobacionOrdenServicio(ByVal idOrden As Int64)
            oMatrixContext.FI_LogAprobacion_OrdenServicio_Del(idOrden)
        End Sub
        Sub BorrarDetalleOR(ByVal e As FI_OrdenRequerimientodetalle)
            oMatrixContext.FI_OrdenRequerimientodetalle.Remove(e)
            oMatrixContext.SaveChanges()
        End Sub

        Sub borrarPersonaApruebaOrdenRequerimiento(ByVal id As Int64)
            oMatrixContext.FI_LogAprobacionOrdenRequerimiento_Del(id)
        End Sub

        Sub borrarLogsAprobacionOrdenRequerimiento(ByVal idOrden As Int64)
            oMatrixContext.FI_LogAprobacion_OrdenRequerimiento_Del(idOrden)
        End Sub
#End Region
    End Class
End Namespace
