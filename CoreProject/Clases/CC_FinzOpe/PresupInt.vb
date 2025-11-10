
Imports System.Data.Entity.Core.Objects
Imports CoreProject.CC_FinzOpe
Public Class PresupInt
#Region "VariableGlobal"
    Private oMatrixContext As CC_FinzOpe
#End Region

#Region "Constructor"
    Public Sub New()
        oMatrixContext = New CC_FinzOpe
    End Sub
#End Region
    Enum ETipoEncuestador
        campo = 1
        tracking = 2
        especializado = 3
        bilingue = 4
        telefonico = 5
        mystery = 6
    End Enum
#Region "Obtener"
    Public Function ObtenerTrabajosSinPresupuesto(ByVal FechaInicio As Date, ByVal FechaFin As Date) As List(Of CC_ProduccionTrabajosSinPresupuesto_Result)
        Return oMatrixContext.CC_ProduccionTrabajosSinPresupuesto(FechaInicio, FechaFin).ToList
    End Function
    Public Function ObtenerAñoDelPresupuesto(ByVal IdPresupuesto As Int64) As Int64
        Dim VAño = (From c In oMatrixContext.CC_PresupuestoInterno Where c.Id = IdPresupuesto Select c.AñoPresupuesto).FirstOrDefault()
        Return VAño
    End Function
    Public Function ObtenerTipoDePresupuesto(ByVal IdPresupuesto As Int64) As Int64
        Dim VTipo = (From c In oMatrixContext.CC_PresupuestoInterno Where c.Id = IdPresupuesto Select c.TipoPresupuesto).FirstOrDefault()
        Return VTipo
    End Function
    Public Function ObtenerIDCondicion(ByVal IdPresupuesto As Int64) As Int64
        Dim VIdCondicion = (From c In oMatrixContext.CC_PresupuestoInternoCondiciones Where c.PresupuestoId = IdPresupuesto Select c.Id).FirstOrDefault()
        Return VIdCondicion
    End Function
    Public Function LstTipoPresupuesto() As List(Of CC_TiposDePresupuesto)
        Return oMatrixContext.CC_TiposDePresupuesto.ToList
    End Function
    Public Function LstObtenerAñoValor(ByVal WTipo As String) As List(Of CC_ObtenerAñoValor_Result)
        Return oMatrixContext.CC_ObtenerAñoValor(WTipo).ToList
    End Function
    Public Function LstObtenerAñoValorLast(ByVal Tipo As String, TrabajoId As Int64) As CC_ObtenerAñoValorLast_Result
        Return oMatrixContext.CC_ObtenerAñoValorLast(Tipo, TrabajoId).ToList().FirstOrDefault
    End Function
    Public Function ObtenerPresupuestoXID(ByVal IdMuestra As Int64) As List(Of CCPresupuestosInternosGet_Result)
        Return oMatrixContext.CCPresupuestosInternosGet(IdMuestra).ToList
    End Function
    Public Function GuardarPresupuestoInterno(ByVal trabajoId As Int64, ByVal UsuarioId As Int64, ByVal Tipo As Int64, IdAñoValor As Int64) As Int64
        Dim PresupuestoId As Decimal = 0
        Dim oResult As ObjectResult(Of Int64?)
        oResult = oMatrixContext.CC_CalcularPresupuestoInterno(trabajoId, UsuarioId, Tipo, IdAñoValor)
        PresupuestoId = Decimal.Parse(oResult(0))
        Return PresupuestoId
    End Function
    Public Function ObtenerPresupuesto(ByVal IdPresupuesto As Int64) As List(Of CC_PresupuestosInternosGetXId_Result)
        Return oMatrixContext.CC_PresupuestosInternosGetXId(IdPresupuesto).ToList
    End Function
    Public Function GuadarDetalle(ByVal Cantidad As Int64, ByVal IdCargo As Int64, ByVal IdPresupuesto As Double)
        Return oMatrixContext.CCDetallePresupuestoInternoAdd(Cantidad, IdCargo, IdPresupuesto)
    End Function
    Public Sub ActualizarTotal(ByVal Id As Int64, ByVal Total As Double)
        oMatrixContext.CCPresupuestoInternoEditarTotal(Id, Total)
    End Sub
    Public Sub EliminarDetallePresupuesto(ByVal IdCargo As Int64?, ByVal IdPresupuestoInterno As Int64?)
        oMatrixContext.CCDetallePresupuestoInternoDelete(IdCargo, IdPresupuestoInterno)
    End Sub
    Public Function ObtenerDetallePresupuesto(ByVal IDPresupuesto As Int64) As List(Of CC_DetallePresupuestosSelect_Result)
        Return oMatrixContext.CC_DetallePresupuestosSelect(IDPresupuesto).ToList
    End Function
    Public Function ObtenerDetallePresupuestoReclutamiento(ByVal IDPresupuesto As Int64) As List(Of CC_DetallePresupuestosReclutamientoSelect_Result)
        Return oMatrixContext.CC_DetallePresupuestosReclutamientoSelect(IDPresupuesto).ToList
    End Function
    Public Sub CalcularDetallePresupuesto(ByVal IdPresupuesto As Int64, ByVal WIdAñoValor As Int64, ByVal WTipoPresup As Int64, ByVal WActualiza As Int64)
        If WTipoPresup = 3 Then
            oMatrixContext.CC_CalcularPresupuestoInternoDetalleCATI(IdPresupuesto, WIdAñoValor, WTipoPresup, WActualiza)
        ElseIf WTipoPresup = 6 Then
            oMatrixContext.CC_CalcularPresupuestoInternoDetalleJornadas(IdPresupuesto, WIdAñoValor, WTipoPresup, WActualiza)
        Else

            oMatrixContext.CC_CalcularPresupuestoInternoDetalle(IdPresupuesto, WIdAñoValor, WTipoPresup, WActualiza)
        End If
    End Sub
    Public Sub ActualizarDetallePresupuesto(ByVal IdDetalle As Int64, ByVal Cantidad As Int64)
        oMatrixContext.CC_DetallePresupuestosUpdate(IdDetalle, Cantidad)
    End Sub
    Public Sub ActualizarProductividad(ByVal IdPresupuesto As Int64, ByVal Productividad As Double, ByVal Muestra As Integer)
        oMatrixContext.CC_ActualizarProductividad(IdPresupuesto, Productividad, Muestra)
    End Sub
    Public Function ObtenerPresupuestosInternos(ByVal TrabajoId As Int64) As List(Of CC_PresupuestoInternoGet_Result)
        Return oMatrixContext.CC_PresupuestoInternoGet(TrabajoId).ToList
    End Function
    Public Function ObtenerPresupuestosCondiciones(ByVal PresupuestoId As Int64) As List(Of CC_ObtenerPresupuestosCondicionesDescripciones_Result)
        Return oMatrixContext.CC_ObtenerPresupuestosCondicionesDescripciones(PresupuestoId).ToList
    End Function
    Public Function ObtenerMuestraTrabajo(ByVal Idtrabjo As Int64) As List(Of CC_MuestraTrabajosGet_Result)
        Return oMatrixContext.CC_MuestraTrabajosGet(Idtrabjo).ToList
    End Function
    Public Sub SolicitudPresupuestoGuardar(ByVal Usario As Int64, ByVal TrabajoId As Int64, ByVal fecha As Date, ByVal Observacion As String)
        oMatrixContext.CC_SolicitudPresupuestoInternoAdd(Usario, fecha, TrabajoId, Observacion)
    End Sub

    Public Sub SolicitudPresupuestoInternoAdd(ByVal TrabajoId As Int64)
        oMatrixContext.CC_SolicitudPresupuestoInternoAddMod(TrabajoId)
    End Sub

    Public Sub SolicitudPresupuestoInternoAddNew(ByRef ent As CC_SolicitudPresupuesto)
        oMatrixContext.CC_SolicitudPresupuesto.Add(ent)
        oMatrixContext.SaveChanges()
    End Sub

    Public Function SolicitudPresupuestoInternoGet(ByVal TrabajoId As Int64) As CC_SolicitudPresupuesto
        Return oMatrixContext.CC_SolicitudPresupuesto.Where(Function(x) x.TrabajoId = TrabajoId).FirstOrDefault
    End Function


    Public Function ValorEncuesta(ByVal Trabajoid As Int64, ByVal Tipo As Int64) As List(Of CC_ValorEncuestaGet_Result)
        Return oMatrixContext.CC_ValorEncuestaGet(Trabajoid, Tipo).ToList
    End Function

    Public Function ObtenerNombreCoordinador(ByVal trabajoid As Int64, ByVal ciudadid As Int64) As List(Of CC_NombreCoordinadorGet_Result)
        Return oMatrixContext.CC_NombreCoordinadorGet(trabajoid, ciudadid).ToList
    End Function

    Public Function MuestraGerenarRequerimiento(ByVal Trabajo As Int64) As List(Of CC_MuestraGenerarRequerimiento_Result)
        Return oMatrixContext.CC_MuestraGenerarRequerimiento(Trabajo).ToList
    End Function

    Public Function PersonalAsginado(ByVal Trabajo As Int64?, ByVal Ciudad As Int64?, ByVal Tipocontratacion As Int64?) As List(Of CC_PersonalAsignadoGet_Result)
        Return oMatrixContext.CC_PersonalAsignadoGet(Trabajo, Ciudad, Tipocontratacion).ToList
    End Function

    Public Function DetallePresupuestoGet(ByVal idpresupuesto As Int64) As List(Of CC_DetallePresupuestoGet_Result)
        Return oMatrixContext.CC_DetallePresupuestoGet(idpresupuesto).ToList
    End Function

    Public Function PresupuestosXId(ByVal Presupuestoid As Int64, ByVal Trabajoid As Int64) As List(Of CC_DETALLEPRESUPUESTOXID_Result)
        Return oMatrixContext.CC_DETALLEPRESUPUESTOXID(Presupuestoid, Trabajoid).ToList
    End Function

    Public Function DetalleXId(ByVal Id As Int64) As List(Of CC_DetallePresupuestoGetXId_Result)
        Return oMatrixContext.CC_DetallePresupuestoGetXId(Id).ToList
    End Function
    Public Function ProduccionXFechas(ByVal FecIni As Date, ByVal FecFin As Date) As List(Of CC_LiquidacionXFechas_Result)
        Return oMatrixContext.CC_LiquidacionXFechas(FecIni, FecFin).ToList
    End Function
    Public Function ProducccionGet(ByVal Fechaini As Date, ByVal FechaFin As Date) As List(Of CC_ProduccionXFechas_Result)
        Return oMatrixContext.CC_ProduccionXFechas(Fechaini, FechaFin).ToList
    End Function
    Public Function ObservacionPresupuestoGet(ByVal TrabajoId As Int64) As List(Of CC_ObservacionPresupuestoGet_Result)
        Return oMatrixContext.CC_ObservacionPresupuestoGet(TrabajoId).ToList
    End Function
    Public Function CedulasProduccion(ByVal Fechaini As Date, ByVal FechaFin As Date, ByVal CiudadId As Int64?, ByVal WTipoContratacion As Int64?, ByVal WCedula As Int64?, ByVal tipoEncuestador As ETipoEncuestador?) As List(Of CC_CedulasProduccion_Result)
        Return oMatrixContext.CC_CedulasProduccion(Fechaini, FechaFin, CiudadId, WTipoContratacion, WCedula, tipoEncuestador).ToList
    End Function
    Public Function PreguntasGestionCampo(ByVal TrabajoId As Int64) As List(Of CC_PreguntasGestionCampoXTrabajo_Result)
        Return oMatrixContext.CC_PreguntasGestionCampoXTrabajo(TrabajoId).ToList
    End Function
    Public Function CodigosPregunta(ByVal Pr_Id As Int64) As List(Of CC_CodigosPreguntasGestionCampo_Result)
        Return oMatrixContext.CC_CodigosPreguntasGestionCampo(Pr_Id).ToList
    End Function
    Public Function Listadotrabajosdescargar()
        Return oMatrixContext.CC_ListadoTrabajosActivosDescargar().ToList
    End Function
    Sub Requisiciones(ByVal PersonaId As Int64, ByVal TrabajoId As Int64, ByVal Valor As Double, ByVal CiudadId As Int64, ByVal UsuarioId As Int64)
        oMatrixContext.CC_RequisiscionesAdd(PersonaId, TrabajoId, Valor, CiudadId, UsuarioId)
    End Sub
    Public Function ProduccionGetxFecha(ByVal fecini As Date, ByVal fecfin As Date, ByVal Cedula As Int64?) As List(Of CC_ProduccionGetxFecha_Result)
        Return oMatrixContext.CC_ProduccionGetxFecha(fecini, fecfin, Cedula).ToList
    End Function
    Public Function guardarcondiciones(ByVal Info As CC_PresupuestoInternoCondiciones) As Int64
        oMatrixContext.CC_PresupuestoInternoCondiciones.Add(Info)
        oMatrixContext.SaveChanges()
        Return Info.Id
    End Function
    Sub guardardetallecondicion(ByVal Info As CC_PresupuestoInternoCondicionesDetalle)
        oMatrixContext.CC_PresupuestoInternoCondicionesDetalle.Add(Info)
        oMatrixContext.SaveChanges()
    End Sub
    Public Function InfoTrbajo(ByVal TrabajoId As Int64) As List(Of CC_InfoTrabajo_Result)
        Return oMatrixContext.CC_InfoTrabajo(TrabajoId).ToList
    End Function
    Public Function ProduccionExport(ByVal Fecini As Date, ByVal Fecfin As Date) As List(Of CC_ProduccionExport_Result)
        Return oMatrixContext.CC_ProduccionExport(Fecini, Fecfin).ToList
    End Function

    Public Function ArchivoNomina(ByVal Fecini As Date, ByVal Fecfin As Date) As IList(Of CC_ArchivoNomina_Result)
        Return oMatrixContext.CC_ArchivoNomina(Fecini, Fecfin).ToList
    End Function

    Public Function TarifasAutomaticas(ByVal Id As Int64?) As List(Of CC_TarifasAutomaticasGet_Result)
        Return oMatrixContext.CC_TarifasAutomaticasGet(Id).ToList
    End Function

    Sub CuentasdeCobro(ByVal Consecutivo As Int64, ByVal PersonaId As Int64, ByVal Total As Double, ByVal UsuarioId As Int64)
        oMatrixContext.CC_CuentasdeCobroAdd(Consecutivo, PersonaId, Total, UsuarioId)
    End Sub
    Public Function Descuentos(ByVal Cedula As Int64, ByVal FecIni As Date, ByVal FecFin As Date) As List(Of CC_DescuentosGet_Result)
        Return oMatrixContext.CC_DescuentosGet(Cedula, FecIni, FecFin).ToList
    End Function

    Public Function JobsCerrados(ByVal TrabajoId As Int64) As List(Of CC_JobBookCerradoGet_Result)
        Return oMatrixContext.CC_JobBookCerradoGet(TrabajoId).ToList
    End Function

    Public Function Conteos(ByVal TrabajoId As Int64) As List(Of CC_ConteosXIdGet_Result)
        Return oMatrixContext.CC_ConteosXIdGet(TrabajoId).ToList
    End Function

    Public Function ConsecutivoCC() As List(Of CC_ConsecutivoCC_Result)
        Return oMatrixContext.CC_ConsecutivoCC.ToList
    End Function


    Public Function FecchaUltimaCC(ByVal PersonaId As Int64) As List(Of CC_FechaUltimaCC_Result)
        Return oMatrixContext.CC_FechaUltimaCC(PersonaId).ToList
    End Function

    Public Function ValorUnitarioContratista(ByVal TrabajoId As Int64) As Double
        Dim Valor As Double = 0
        Dim oResult As ObjectResult(Of Double?)
        oResult = oMatrixContext.CC_ValorUnitarioContratistaGet(TrabajoId)
        Valor = Decimal.Parse(oResult(0))
        Return Valor
    End Function

    Public Function SolicitudPresupuestoValidar(ByVal Trabajoid As Int64) As List(Of CC_SolicitudPresupuestoGet_Result)
        Return oMatrixContext.CC_SolicitudPresupuestoGet(Trabajoid).ToList
    End Function

    Public Function ProduccionCiudades(ByVal fecini As Date, ByVal fecfin As Date) As List(Of CC_ProduccionCiudades_Result)
        Return oMatrixContext.CC_ProduccionCiudades(fecini, fecfin).ToList
    End Function

    Public Function tiposEncuestadorConProduccion(fechaInicio As Date, fechaFin As Date) As List(Of CC_ProduccionTiposEncuestador_Result)
        Return oMatrixContext.CC_ProduccionTiposEncuestador(fechaInicio, fechaFin).ToList
    End Function

    Public Function ProduccionXCiudad(ByVal fecini As Date, ByVal fecfin As Date, ByVal Cedula As Int64?, ByVal Ciudad As Int64?, ByVal tipoEncuestador As ETipoEncuestador?) As List(Of CC_ProduccionXCiudad_Result)
        Return oMatrixContext.CC_ProduccionXCiudad(fecini, fecfin, Cedula, Ciudad, tipoEncuestador).ToList
    End Function
#End Region

#Region "Actualizar"
    Sub DuplicarDetalle(ByVal Cantidad As Int64, ByVal ValorUnitario As Single, ByVal Cesantias As Single, ByVal IntCesantias As Single, ByVal Vacaciones As Single, ByVal PrimaLegal As Single, ByVal SaludDto As Single, ByVal SaludCosto As Single, ByVal PensionDto As Single, ByVal PensionCosto As Single, ByVal ARLDto As Single, ByVal ARLCosto As Single, ByVal CCFDto As Single, ByVal CCFCosto As Single, ByVal Otro1Dto As Single, ByVal Otro1Costo As Single, ByVal Otro2Dto As Single, ByVal Otro2Costo As Single, ByVal Ajustado As Single, ByVal TotalIngreso As Single, ByVal TotalDescuento As Single, ByVal TotalCosto As Single, ByVal PresupuestoId As Int64, ByVal CodigoId As Int64)
        oMatrixContext.CC_UpdateDetallePresupuesto(Cantidad, ValorUnitario, Cesantias, IntCesantias, Vacaciones, PrimaLegal, SaludDto, SaludCosto, PensionDto, PensionCosto, ARLDto, ARLCosto, CCFDto, CCFCosto, Otro1Dto, Otro1Costo, Otro2Dto, Otro2Costo, Ajustado, TotalIngreso, TotalDescuento, TotalCosto, PresupuestoId, CodigoId)
    End Sub

    Sub InsertCondicion(ByVal Condicion As String, ByVal presupuesto As Int64, ByVal estado As Int64, ByVal tipo As Int64)
        oMatrixContext.CC_InsertCondicionPresupuesto(Condicion, presupuesto, estado, tipo)
    End Sub


    Sub ActualizarValorContratista(ByVal presupuestoid As Int64, ByVal Valor As Double)
        oMatrixContext.CC_ActualizarValorContratista(presupuestoid, Valor)
    End Sub
    Sub ActualizarValoresSupervision(ByVal presupuestoid As Int64, ByVal Valor As Double, ByVal cargo As Integer)
        oMatrixContext.CC_ActualizarValorIndividualContratista(presupuestoid, Valor, cargo)
    End Sub
    Sub ActualizarVrunitario(ByVal IdPresupuesto As Int64, ByVal Vrunitario As Double)
        oMatrixContext.CC_ActualizarVrUnitario(IdPresupuesto, Vrunitario)
    End Sub

    Sub ActualizarPresupuestoReclutamiento(ByVal idrecord As Int64, ByVal muestra As Integer, ByVal tarifapst As Double, ByVal tarifacontratista As Double)
        oMatrixContext.CC_CalcularPresupuestoInternoReclutamiento(idrecord, muestra, tarifapst, tarifacontratista)
    End Sub

    Function GetPresupuestoInterno(ByVal idPresup As Int64) As CC_PresupuestoInterno
        Return oMatrixContext.CC_PresupuestoInterno.Where(Function(x) x.Id = idPresup).FirstOrDefault
    End Function

    Sub SavePresupuestoInterno(ByRef ent As CC_PresupuestoInterno)
        oMatrixContext.SaveChanges()
    End Sub
#End Region


End Class

