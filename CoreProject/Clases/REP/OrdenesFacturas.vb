Public Class OrdenesFacturas
#Region "Variables Globales"
    Private oMatrixContext As RP_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New RP_Entities
    End Sub
#End Region

#Region "Obtener"
    Function obtenerOrdenesServicioFacturas(idOrden As Nullable(Of Long), ordenFechaInicio As Nullable(Of Date), ordenFechaFin As Nullable(Of Date), facturaFechaInicio As Nullable(Of Date), facturaFechaFin As Nullable(Of Date), facturaConsecutivo As Nullable(Of Long)) As List(Of REP_OrdenesServicio_Facturas_Result)
        Return oMatrixContext.REP_OrdenesServicio_Facturas(idOrden, ordenFechaInicio, ordenFechaFin, facturaFechaInicio, facturaFechaFin, facturaConsecutivo).ToList
    End Function
    Function obtenerOrdenesCompraFacturas(idOrden As Nullable(Of Long), ordenFechaInicio As Nullable(Of Date), ordenFechaFin As Nullable(Of Date), facturaFechaInicio As Nullable(Of Date), facturaFechaFin As Nullable(Of Date), facturaConsecutivo As Nullable(Of Long)) As List(Of REP_OrdenesCompra_Facturas_Result)
        Return oMatrixContext.REP_OrdenesCompra_Facturas(idOrden, ordenFechaInicio, ordenFechaFin, facturaFechaInicio, facturaFechaFin, facturaConsecutivo).ToList
    End Function

    Function obtenerOrdenesUnificadasFacturas(idOrden As Nullable(Of Long), ordenFechaInicio As Nullable(Of Date), ordenFechaFin As Nullable(Of Date), facturaFechaInicio As Nullable(Of Date), facturaFechaFin As Nullable(Of Date), facturaConsecutivo As Nullable(Of Long), TipoOrden As Nullable(Of Int64), Proveedor As Nullable(Of Int64), CuentaContable As Nullable(Of Int64)) As List(Of REP_OrdenesUnificado_Facturas_Result)
        Return oMatrixContext.REP_OrdenesUnificado_Facturas(idOrden, ordenFechaInicio, ordenFechaFin, facturaFechaInicio, facturaFechaFin, facturaConsecutivo, TipoOrden, Proveedor, CuentaContable).ToList
    End Function

    Function obtenerFacturasRadicadas(ByVal Consecutivo As Int64?, ByVal Proveedor As Int64?, ByVal facturaFechaInicio As Date?, ByVal facturaFechaFin As Date?, CuentaContable As Nullable(Of Int64)) As List(Of REP_FacturasRadicadas_Result)
        Return oMatrixContext.REP_FacturasRadicadas(Consecutivo, Proveedor, facturaFechaInicio, facturaFechaFin, CuentaContable).ToList
    End Function

    Function obtenerOrdenesUnificadasRequerimientoServicio(idOrden As Nullable(Of Long), ordenFechaInicio As Nullable(Of Date), ordenFechaFin As Nullable(Of Date), facturaFechaInicio As Nullable(Of Date), facturaFechaFin As Nullable(Of Date), facturaConsecutivo As Nullable(Of Long), TipoOrden As Nullable(Of Int64), Proveedor As Nullable(Of Int64), idTrabajo As Nullable(Of Long)) As List(Of REP_OrdenesUnificado_RequerimientosServicio_Result)
        Return oMatrixContext.REP_OrdenesUnificado_RequerimientosServicio(idOrden, ordenFechaInicio, ordenFechaFin, facturaFechaInicio, facturaFechaFin, facturaConsecutivo, TipoOrden, Proveedor, idTrabajo).ToList
    End Function
#End Region
End Class
