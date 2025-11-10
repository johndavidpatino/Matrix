
Imports System.Data.Entity.Core.Objects
Public Class ProcesosInternos
#Region "VariableGlobal"
    Private oMatrixContext As CC_FinzOpe
    Enum EEstadosOrdenes
        Creada = 1
        Anular = 2
        Generada = 3
    End Enum
#End Region

#Region "Constructor"
    Public Sub New()
        oMatrixContext = New CC_FinzOpe
    End Sub
#End Region

#Region "Guardar"
    Sub guardarpresupuestointerno(ByVal IdActividad As Int64, ByVal UsuarioId As Int64, ByVal TrabajoId As Int64)
        oMatrixContext.CC_PresupuestosAreasInternas(IdActividad, TrabajoId, UsuarioId)
    End Sub
	Sub GuardarConteo(ByVal job As String, ByVal trabajoid As Int64, ByVal nombretrabajo As String, ByVal Unidad As Double, ByVal Producto As String, ByVal duracion As Int64, ByVal Cerradas As Int64, ByVal CerradasM As Int64, ByVal Abiertas As Int64, ByVal AbiertasM As Int64, ByVal Otros As Int64, ByVal Demograficos As Int64, ByVal Paginas As Int64, ByVal Observaciones As String, ByVal UsuarioId As Int64)
		oMatrixContext.CC_PreguntasHistoricoGuardar(duracion, Cerradas, CerradasM, Abiertas, AbiertasM, Otros, Demograficos, Paginas, Observaciones, UsuarioId, job, trabajoid, nombretrabajo, Unidad, Producto)
	End Sub
	Sub ProduccionAdd(ByVal Personaid As Int64, ByVal TrabajoId As Int64, ByVal Cantidad As Double, ByVal VrUnitario As Double, ByVal Total As Double, ByVal Fecha As Date, ByVal UsuarioId As Int64, ByVal Ciudad As Int64, ByVal DiasTrabajados As Double, ByVal PresupuestoId As Int64, ByVal TipoContratacion As Integer, ByVal cargueId As Int64)
		oMatrixContext.CC_ProduccionAdd(Personaid, TrabajoId, Cantidad, VrUnitario, Total, Fecha, UsuarioId, Ciudad, DiasTrabajados, PresupuestoId, TipoContratacion, cargueId)
	End Sub
	Public Function ordendeservicioadd(contratistaId As Nullable(Of Long), fechaRequerimiento As Nullable(Of Date), departamento As Nullable(Of Short), ciudadId As Nullable(Of Integer), fechaEntrega As Nullable(Of Date), formaPago As String, beneficiario As String, solicitadoPor As Nullable(Of Long), tipoDetalle As Nullable(Of Byte), trabajoId As Nullable(Of Long), centroCosto As Nullable(Of Integer), subtotal As Nullable(Of Double), descuento As Nullable(Of Double), iVA As Nullable(Of Double), vrtotal As Nullable(Of Double), usuarioId As Nullable(Of Long), fecha As Nullable(Of Date), estado As Nullable(Of Integer), jobBookNombre As String, jobBookCodigo As String, PorcentajeAnticipo As Nullable(Of Double), VrAnticipo As Nullable(Of Double), PagoFinalEstudio As Nullable(Of Double)) As Decimal
		Dim OrdenId As Decimal = 0
		Dim oResult As ObjectResult(Of Decimal?)
		oResult = oMatrixContext.CC_OrdenesdeServicioAdd(contratistaId, fechaRequerimiento, departamento, ciudadId, fechaEntrega, formaPago, beneficiario, solicitadoPor, tipoDetalle, trabajoId, centroCosto, subtotal, descuento, iVA, vrtotal, usuarioId, fecha, estado, jobBookNombre, jobBookCodigo, PorcentajeAnticipo, VrAnticipo, PagoFinalEstudio)
		OrdenId = Decimal.Parse(oResult(0))
		Return OrdenId
	End Function
	Sub DetalleOrdendeServicioAdd(ByVal OrdenId As Int64, ByVal Cantidad As Double, ByVal VrUnitario As Double, ByVal Vrtotal As Double, ByVal CuentaContableId As Long, ByVal servicioId As Long?)
        oMatrixContext.CC_DetalleOrdendeServicioAdd(OrdenId, Cantidad, VrUnitario, Vrtotal, CuentaContableId, servicioId)
    End Sub
    Function RecepcionCuentadeCobroadd(ByVal Consecutivo As Int64, ByVal Cantidad As Double, ByVal VrUnitario As Double, ByVal VrTotal As Double, ByVal Observacion As String, ByVal TipoDocumento As Int64, ByVal OrdenId As Int64, ByVal UsuarioId As Int64) As Nullable(Of Decimal)
        Return oMatrixContext.CC_RecepcionCuentasdeCobroAdd(Consecutivo, Cantidad, VrUnitario, VrTotal, Observacion, TipoDocumento, OrdenId, UsuarioId, 1).FirstOrDefault
    End Function
    Sub ActualizarTotalOrden(ByVal Total As Double, ByVal OrdenId As Int64)
        oMatrixContext.CC_AjustarTotalUpdate(OrdenId, Total)
    End Sub
    Sub ActualizarEstadoCuentarecibida(ByVal Estadoid As Int64, ByVal OrdenId As Int64)
        oMatrixContext.CC_ActualizarEstadoCuenta(Estadoid, OrdenId)
    End Sub

    Sub ActualizarJobBooks(ByVal jobbook As String, estado As String)
        oMatrixContext.CC_ProduccionJobsAbiertos_CRU(jobbook, estado)
    End Sub

    Sub ActualizarRadicado(ByVal Consecutivo As Int64, ByVal Cantidad As Double, ByVal VrUnitario As Double, ByVal VrTotal As Double, ByVal Observacion As String, ByVal Id As Int64)
        oMatrixContext.CC_RecepcionCuentasCobroUpdate(Id, Consecutivo, Cantidad, VrUnitario, VrTotal, Observacion)
    End Sub
    Function GrabarUsuarioApruebaOrdenServicio(ByVal idOrden As Int64, ByVal usuario As Int64) As Int64
        Return oMatrixContext.CC_LogAprobacionOrdenesServicio_Add(idOrden, usuario).FirstOrDefault
    End Function

    Sub GuardarCalificacionEvaluacionProveedor(ByRef e As CO_EvaluacionProveedoresFacturaOP)
        If e.Id = 0 Then
            oMatrixContext.CO_EvaluacionProveedoresFacturaOP.Add(e)
        End If
        oMatrixContext.SaveChanges()
    End Sub

    Function guardarLogEstadosOrdenesServicio(idOrden As Nullable(Of Long), observacion As String, fechaRegistro As Nullable(Of DateTime), usuarioId As Nullable(Of Long), estadoId As Nullable(Of Integer)) As Integer
        Return oMatrixContext.CC_LogEstadosOrdenesServicio_Add(idOrden, observacion, fechaRegistro, usuarioId, estadoId)
    End Function
    Sub ActualizarOrdeneServicio(idOrden As Long, contratistaId As Nullable(Of Long), fechaRequerimiento As Nullable(Of Date), departamento As Nullable(Of Short), ciudadId As Nullable(Of Integer), fechaEntrega As Nullable(Of Date), formaPago As String, beneficiario As String, solicitadoPor As Nullable(Of Long), tipoDetalle As Nullable(Of Byte), trabajoId As Nullable(Of Long), centroCosto As Nullable(Of Integer), subtotal As Nullable(Of Double), descuento As Nullable(Of Double), iVA As Nullable(Of Double), vrtotal As Nullable(Of Double), usuarioId As Nullable(Of Long), fecha As Nullable(Of Date), estado As Nullable(Of Integer), jobBookNombre As String, jobBookCodigo As String, PorcentajeAnticipo As Nullable(Of Double), VrAnticipo As Nullable(Of Double), PagoFinalEstudio As Nullable(Of Double))
        oMatrixContext.CC_OrdenesdeServicio_Edit(idOrden, contratistaId, fechaRequerimiento, departamento, ciudadId, fechaEntrega, formaPago, beneficiario, solicitadoPor, tipoDetalle, trabajoId, centroCosto, subtotal, descuento, iVA, vrtotal, usuarioId, fecha, estado, jobBookNombre, jobBookCodigo, PorcentajeAnticipo, VrAnticipo, PagoFinalEstudio)
    End Sub
    Sub CuentaCobroDetalle(ByVal CuentaId As Int64, ByVal Concepto As String, ByVal Valor As Double)
        oMatrixContext.CC_CuentaCobroDetalleAdd(CuentaId, Concepto, Valor)
    End Sub

    Sub CargueTemporalDiferenteEncuestasPasarAFinal()
        oMatrixContext.CC_Produccion_CargueTemporal_PasarAFinal()
    End Sub
    Function CargueProduccionAdd(ByVal Fecha As Date, ByVal Cantidad As Int64, ByVal Usuario As Int64) As Decimal?
        Return oMatrixContext.CC_CarguesProduccionAdd(Fecha, Cantidad, Usuario).FirstOrDefault
    End Function

#End Region

#Region "Obtener"
    Public Function ObtenerUltimoIdProduccion() As Double
        Return oMatrixContext.CC_ObtenerUltimoIdProduccion().FirstOrDefault
    End Function
    Function ObtenerPresupuestosAreasInternas(ByVal TrabajoId As Int64) As List(Of CC_PresupuestosInternosGet_Result)
        Return oMatrixContext.CC_PresupuestosInternosGet(TrabajoId).ToList
    End Function
    Function ObtenerTrabajoInfo(ByVal TrabajoId As Int64) As List(Of CC_TrabajoGetProducto_Result)
        Return oMatrixContext.CC_TrabajoGetProducto(TrabajoId).ToList
    End Function
    Function ProduccionTemp() As List(Of CC_ProduccionTempGet_Result)
        Return oMatrixContext.CC_ProduccionTempGet().ToList
    End Function
    Public Function ValidarDatosCargue(ByVal FechaIni As Date, ByVal FechaFin As Date, ByVal WTipoContratacion As Integer) As Boolean
        If oMatrixContext.CC_ProduccionValidarDatosCargue(FechaIni, FechaFin, WTipoContratacion).ToList.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Function ValidarHorasPorDia() As List(Of CC_ProduccionValidarHorasPorDia_Result)
        Return oMatrixContext.CC_ProduccionValidarHorasPorDia().ToList
    End Function
    Function ProduccionErroresCargueDatos() As List(Of CC_ObtenerProduccionErroresCargueDatos_Result)
        Return oMatrixContext.CC_ObtenerProduccionErroresCargueDatos().ToList
    End Function
    Public Function CuentasContablesGet(ByVal Nombre As String, ByVal numeroCuenta As String, ByVal id As Int64?, ByVal tipo As Int16?) As List(Of CC_CuentasContablesGet_Result)
        Return oMatrixContext.CC_CuentasContablesGet(Nombre, numeroCuenta, id, tipo).ToList
    End Function
    Public Function ConsecutivoOrdendeServicio() As List(Of CC_ConsecutivoOrdendeServicio_Result)
        Return oMatrixContext.CC_ConsecutivoOrdendeServicio().ToList
    End Function
    Public Function ProduccionResumen(ByVal PersonaId As Int64, ByVal FecIni As Date, ByVal FecFin As Date) As List(Of CC_ProduccionResumenXCedula_Result)
        Return oMatrixContext.CC_ProduccionResumenXCedula(PersonaId, FecIni, FecFin).ToList
    End Function
    Public Function ProduccionResumenPersonasProcInt(ByVal FecIni As Date, FecFin As Date) As List(Of Long?)
        Return oMatrixContext.CC_ProduccionResumenPersonas(FecIni, FecFin).ToList
    End Function
    Public Function ValidarProduccion(ByVal PersonaId As Int64?, ByVal FecIni As Date, ByVal FecFin As Date) As Boolean
        If oMatrixContext.CC_ProduccionResumenXCedula(PersonaId, FecIni, FecFin).ToList.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function OrdenesdeServicioGet(ByVal Trabajoid As Int64?, ByVal Id As Int64?, ByVal contratistaId As Int64?) As List(Of CC_OrdenesdeServicioGet_Result)
        Return oMatrixContext.CC_OrdenesdeServicioGet(Trabajoid, Id, contratistaId).ToList()
    End Function
    Public Function TipoDocumentoGet() As List(Of CC_TipoDocumentoGet_Result)
        Return oMatrixContext.CC_TipoDocumentoGet().ToList
    End Function
    Public Function CuentasRadicadasGet(ByVal ContratistaId As Int64?, ByVal OrdenId As Int64?) As List(Of CC_CuentasRadicadasGet_Result)
        Return oMatrixContext.CC_CuentasRadicadasGet(ContratistaId, OrdenId).ToList
    End Function
    Public Function ResumenPagos(ByVal TrabajoId As Int64?, ByVal Job As String) As List(Of CC_ReportePagos_Result)
        Return oMatrixContext.CC_ReportePagos(TrabajoId, Job).ToList
    End Function
    Public Function EstadosCuentasRecibidas() As List(Of CC_EstadosCuentasGet_Result)
        Return oMatrixContext.CC_EstadosCuentasGet().ToList
    End Function
    Public Function ValorEncuestaContratista(ByVal Trabajoid As Int64) As Double
        If oMatrixContext.CC_ValorUnitarioContratistaGet(Trabajoid).Count > 1 Then
            Return oMatrixContext.CC_ValorUnitarioContratistaGet(Trabajoid).FirstOrDefault
        Else
            Return 0
        End If
    End Function
    Public Function ObtenerMuestra(ByVal TrabajoId As Int64?, ByVal Job As String) As Double
        Return oMatrixContext.CC_ReporteMuestra(TrabajoId, Job).FirstOrDefault
    End Function
    Public Function ActividadesPresupuestadas(ByVal PropuestaId As Int64, ByVal ParAlternativa As Int64, ByVal ParNacional As Int64, ByVal Metcodigo As Int64) As List(Of CC_ActividadesPresupuestadas_Result)
        Return oMatrixContext.CC_ActividadesPresupuestadas(PropuestaId, ParAlternativa, ParNacional, Metcodigo).ToList
    End Function
    Public Function InformacionTrabajos(ByVal TrabajoId As Int64?, ByVal Job As String) As List(Of CC_InformacionTrabajos_Result)
        Return oMatrixContext.CC_InformacionTrabajos(TrabajoId, Job).ToList
    End Function
    Public Function InformacionCoe(ByVal Identificacion As Int64) As List(Of CC_InformacionCoe_Result)
        Return oMatrixContext.CC_InformacionCoe(Identificacion).ToList
    End Function
    Public Function ProduccionValorEncuesta(ByVal Trabajoid As Int64, ByVal Cargo As Int16, ByVal Condiciones As Int16, ByVal P1 As Int64, ByVal V1 As Int64,
                                            ByVal P2 As Int64, ByVal V2 As Int64, ByVal P3 As Int64, ByVal V3 As Int64, ByVal P4 As Int64, ByVal V4 As Int64,
                                            ByVal P5 As Int64, ByVal V5 As Int64) As Decimal

        Return oMatrixContext.CC_ProduccionValorEncuesta(Trabajoid, Cargo, Condiciones, P1, V1, P2, V2, P3, V3, P4, V4, P5, V5).FirstOrDefault
    End Function
    Public Function DiasTrabajados(ByVal PersonaId As Int64) As List(Of CC_ProduccionDiasTrabajados_Result)
        Return oMatrixContext.CC_ProduccionDiasTrabajados(PersonaId).ToList
    End Function
    Public Function ParticipantesCapacitacion(ByVal CapacitacionId As Int64, ByVal CargoCC As Int64)
        Return oMatrixContext.CC_ParticipantesPlanillaAsistencia(CapacitacionId, CargoCC)
    End Function
    Public Function EncabezadoPlanilla(ByVal CapacitacionId As Int64, ByVal CargoCC As Int64)
        Return oMatrixContext.CC_EncabezadoPlanillaAsistencia(CapacitacionId, CargoCC)
    End Function
    Public Function ActividadesPresupuestadasCampo(ByVal PropuestaId As Int64, ByVal ParAlternativa As Int64, ByVal ParNacional As Int64, ByVal Metcodigo As Int64) As List(Of CC_ActividadesPresupuestadasCampo_Result)
        Return oMatrixContext.CC_ActividadesPresupuestadasCampo(PropuestaId, ParAlternativa, ParNacional, Metcodigo).ToList
    End Function
    Public Function ReporteActividadesProduccion(ByVal TrabajoId As Int64?, ByVal FecIni As Date?, ByVal Fecfin As Date?) As List(Of CC_ReporteActividadesProduccion_Result)
        Return oMatrixContext.CC_ReporteActividadesProduccion(TrabajoId, FecIni, Fecfin).ToList
    End Function

    Public Function ObtenerProduccionPorIDs(ByVal idIni As Int64, ByVal idFin As Int64) As List(Of CC_ProduccionExportPorIDs_Result)
        Return oMatrixContext.CC_ProduccionExportPorIDs(idIni, idFin).ToList
    End Function
    Public Function ObtenerActiviadesxTrabajo(ByVal TrabajoId As Int16?, ByVal ActCodigo As Int16?) As List(Of IQ_ActiviadesXTrabajo_Result)
        Return oMatrixContext.IQ_ActiviadesXTrabajo(ActCodigo, TrabajoId).ToList
    End Function
    Public Function ActividadesIq() As List(Of IQ_ActividadesGet_Result)
        Return oMatrixContext.IQ_ActividadesGet().ToList

    End Function
    Public Function RecepcionCuentasGet(ByVal Consecutivo As Int64?, ByVal OrdenId As Int64?) As List(Of CC_RecepcionCuentasdeCobroGet_Result)
        Return oMatrixContext.CC_RecepcionCuentasdeCobroGet(Consecutivo, OrdenId).ToList
    End Function

    Public Function RecepcionCuentasGetXId(ByVal Id As Int64?) As List(Of CC_RecepcionCuentasdeCobroGetXId_Result)
        Return oMatrixContext.CC_RecepcionCuentasdeCobroGetXId(Id).ToList
    End Function

    Public Function obtenerCuentasCobro(id As Nullable(Of Long), solicitadaPor As Nullable(Of Long), estado As Nullable(Of Integer), fechaI As Nullable(Of Date), fechaF As Nullable(Of Date), tipoOrden As Nullable(Of Byte)) As List(Of CC_CuentasCobro_Get_Result)
        Return oMatrixContext.CC_CuentasCobro_Get(id, solicitadaPor, estado, fechaI, fechaF, tipoOrden).ToList
    End Function

    Public Function obtenerCalificacionesEvaluacion(ByVal idOrden As Int64) As List(Of CO_EvaluacionProveedoresFacturaOP_Get_Result)
        Return oMatrixContext.CO_EvaluacionProveedoresFacturaOP_Get(idOrden).ToList()
    End Function

    Public Function ReporteOrdenes(ByVal OrdenId As Int64?, ByVal ContratistaId As Int64?, ByVal Estado As Int16?, ByVal Fechainicio As DateTime?, ByVal fechafin As DateTime?, ByVal TodosCampos As String) As List(Of CC_ReporteOrdenesServicio_Result)
        Return oMatrixContext.CC_ReporteOrdenesServicio(ContratistaId, Estado, OrdenId, Fechainicio, fechafin, TodosCampos).ToList()
    End Function

    Public Function ReportePagosConsolidado(ByVal TrabajoId As Int64?, ByVal Job As String) As List(Of CC_ReportePagosSumaxTipo_Result)
        Return oMatrixContext.CC_ReportePagosSumaxTipo(TrabajoId, Job).ToList()
    End Function

    Public Function ReporteContabilizacion(ByVal FecIni As Date, ByVal fecfin As Date) As List(Of CC_ReporteContabilizacion_Result)
        Return oMatrixContext.CC_ReporteContabilizacion(FecIni, fecfin).ToList
    End Function

    Function validarCargueDiferenteEncuestas(ByVal fechaInicio As Date?, ByVal fechaFin As Date?) As List(Of CC_ProduccionValidarCargueDiferenteEncuestas_Result)
        Return oMatrixContext.CC_ProduccionValidarCargueDiferenteEncuestas(fechaInicio, fechaFin).ToList()
    End Function
    Function CarguesProduccionGet(ByVal Id As Int64?, ByVal FechaIni As Date?, ByVal FechaFin As Date?, ByVal Usuario As Int64?) As List(Of CC_CarguesProduccionGet_Result)
        Return oMatrixContext.CC_CarguesProduccionGet(Id, FechaIni, FechaFin, Usuario).ToList
    End Function

    Function UsuariosCarguesProduccion(ByVal Usuario As Int64?) As List(Of US_UsuariosCargueProduccionGet_Result)
        Return oMatrixContext.US_UsuariosCargueProduccionGet(Usuario).ToList
    End Function
#End Region

#Region "Eliminar"
    Sub EliminarProduccionTemp()
        oMatrixContext.CC_ProduccionTempBorrar()
    End Sub
    'Sub EliminarRadicadoCuenta(ByVal Consecutivo As Int64)
    '    oMatrixContext.CC_RadicadoCuentasEliminar(Consecutivo)
    'End Sub

    Sub EliminarRadicadoCuenta(ByVal Id As Int64)
        oMatrixContext.CC_RecepcionCuentasdeCobroDelete(Id)
    End Sub

    Sub EliminarPresupuesto(ByVal PresupuestoId As Int64)
        oMatrixContext.CC_PresupuestoInternoDelete(PresupuestoId)
    End Sub

    Sub eliminarProduccionTemporalDiferenteEncuestas()
        oMatrixContext.CC_Produccion_CargueTemporal_Del()
    End Sub

    Sub EliminarCargueProduccion(ByVal Id As Int64)
        oMatrixContext.CC_ProduccionCargueDelete(Id)
    End Sub

#End Region


#Region "Actualizar"
    Public Sub actualizarDiasTrabajados(fechaInicio As Date, fechaFin As Date)
        oMatrixContext.CC_ActualizarDiasTrabajadosXFechasProduccion(fechaInicio, fechaFin)
    End Sub
    Sub ActualizarProduccion(ByVal FechaInicio As Date, ByVal Fechafin As Date, ByVal Cedula As Int64, ByVal Ciudad As Int64?, ByVal ConsecutivoCuenta As Int64)
        oMatrixContext.CC_ProduccionUpdateCC(FechaInicio, Fechafin, Cedula, Ciudad, ConsecutivoCuenta)
    End Sub
    Sub ActualizarIdCargue(ByVal Id As Int64)
        oMatrixContext.CC_ProduccionCargueTemportalUpdate(Id)
    End Sub

    Public Sub InsertDescuentosSS(cedula As Long, valorSS As Double, saldoSS As Double, valorICA As Double, valorPagado As Double, fecha As Date)
        oMatrixContext.CC_ProduccionDescuentosSS.Add(New CC_ProduccionDescuentosSS With {.Cedula = cedula, .SaldoSS = saldoSS, .ValorICA = valorICA, .ValorSS = valorSS, .Descuento = valorSS + saldoSS + valorICA, .ValorPagado = valorPagado, .Fecha = fecha})
        'Se agrega código para guardar los cambios
        oMatrixContext.SaveChanges()
    End Sub
#End Region



End Class
