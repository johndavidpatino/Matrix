Imports System.Data.Entity.Core.Objects

Imports System.Data.SqlClient

Namespace INV
    Public Class Inventario

#Region "Variables Globales"
        Private oMatrixContext As INVEntities
#End Region

#Region "Constructores"
        'Constructor public 
        Public Sub New()
            oMatrixContext = New INVEntities
        End Sub
#End Region

#Region "Obtener"

        Function ObtenerStockConsumiblesxId(ByVal Id As Int64?) As List(Of INV_StockConsumibles_Get_Result)
            Return oMatrixContext.INV_StockConsumibles_Get(Id, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).ToList
        End Function

        Function ObtenerStockConsumiblesxIdusuario(ByVal IdConsumible As Int64?, ByVal IdUsuario As Int64?) As List(Of INV_StockConsumibles_Get_Result)
            Return oMatrixContext.INV_StockConsumibles_Get(Nothing, IdConsumible, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, IdUsuario, Nothing, Nothing, Nothing).ToList
        End Function

        Function ObtenerStockConsumiblesxIdConsumible(ByVal IdConsumible As Int64?) As List(Of INV_StockConsumibles_Get_Result)
            Return oMatrixContext.INV_StockConsumibles_Get(Nothing, IdConsumible, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).ToList
        End Function

        Function ObtenerStockConsumiblesxTipoMovimiento(ByVal IdConsumible As Int64?, ByVal TipoMovimiento As Int16?) As List(Of INV_StockConsumibles_Get_Result)
            Return oMatrixContext.INV_StockConsumibles_Get(Nothing, IdConsumible, Nothing, TipoMovimiento, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).ToList
        End Function

        Function ObtenerStockConsumibles(ByVal Id As Int64?, ByVal IdConsumible As Int64?, ByVal IdArticulo As Int64?, ByVal TipoMovimiento As Int16?, ByVal BU As Int32?, ByVal JobBookCodigo As String, ByVal JobBookNombre As String, ByVal Ciudad As Int64?, ByVal IdUsuario As Int64?, ByVal Usuario As String, ByVal TipoCargo As Int16?, ByVal Legalizado As Boolean?) As List(Of INV_StockConsumibles_Get_Result)
            Return oMatrixContext.INV_StockConsumibles_Get(Id, IdConsumible, IdArticulo, TipoMovimiento, BU, JobBookCodigo, JobBookNombre, Ciudad, IdUsuario, Usuario, TipoCargo, Legalizado).ToList
        End Function

        Function obtenerAsignacionesxIdActivoFijo(ByVal IdActivoFijo As Int64?) As List(Of INV_Asignaciones_Get_Result)
            Return oMatrixContext.INV_Asignaciones_Get(IdActivoFijo, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).ToList
        End Function

        Function obtenerAsignaciones(ByVal IdActivoFijo As Int64?, ByVal Articulo As Int64?, ByVal BU As Int32?, ByVal JobBookCodigo As String, ByVal JobBookNombre As String, ByVal Ciudad As Int64?, ByVal EstadoTablet As Int64?, ByVal IdUsuarioAsignado As Int64?, ByVal UsuarioAsignado As String, ByVal TipoCargo As Int16?, ByVal Asignado As Boolean?) As List(Of INV_Asignaciones_Get_Result)
            Return oMatrixContext.INV_Asignaciones_Get(IdActivoFijo, Articulo, BU, JobBookCodigo, JobBookNombre, Ciudad, EstadoTablet, IdUsuarioAsignado, UsuarioAsignado, TipoCargo, Asignado).ToList
        End Function

		Function obtenerRegistroArticulos(ByVal Id As Int64?, ByVal TipoArticulo As Int64?, ByVal Articulo As Int64?, ByVal TipoComputador As Int64?, ByVal PertenecePC As Int16?, ByVal TipoPeriferico As Int64?, ByVal TipoProducto As Int64?, Estado As Int64?, ByVal Sede As Int64?, ByVal IdUsuarioAsignado As Int64?, ByVal UsuarioAsignado As String, ByVal Asignado As Boolean?, ByVal idArticulo As Int64?, ByVal TodosCampos As String) As List(Of INV_RegistroArticulos_Get_Result)
			Return oMatrixContext.INV_RegistroArticulos_Get(Id, TipoArticulo, Articulo, TipoComputador, PertenecePC, TipoPeriferico, TipoProducto, Estado, Sede, IdUsuarioAsignado, UsuarioAsignado, Asignado, idArticulo, TodosCampos).ToList
		End Function

		Function obtenerRegistroArticulosxTodos() As List(Of INV_RegistroArticulos_Get_Result)
			Return oMatrixContext.INV_RegistroArticulos_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).ToList
		End Function

        Function obtenerRegistroArticulosTodosCampos(ByVal TodosCampos As String) As List(Of INV_RegistroArticulos_Get_Result)
			Return oMatrixContext.INV_RegistroArticulos_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, TodosCampos).ToList
		End Function

        Function obtenerRegistroArticulosxId(ByVal Id As Int64?) As List(Of INV_RegistroArticulos_Get_Result)
			Return oMatrixContext.INV_RegistroArticulos_Get(Id, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).ToList
		End Function

        Function obtenerRegistroArticulosxTipoArticulo(ByVal TipoArticulo As Int64?) As List(Of INV_RegistroArticulos_Get_Result)
			Return oMatrixContext.INV_RegistroArticulos_Get(Nothing, TipoArticulo, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).ToList
		End Function

        Function obtenerRegistroArticulosxArticulo(ByVal Articulo As Int64?) As List(Of INV_RegistroArticulos_Get_Result)
			Return oMatrixContext.INV_RegistroArticulos_Get(Nothing, Nothing, Articulo, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).ToList
		End Function

        Function obtenerArticulosTodos() As List(Of INV_Articulos_Get_Result)
            Return oMatrixContext.INV_Articulos_Get(Nothing, Nothing).ToList
        End Function

        Function obtenerArticulosxTipoArticulo(ByVal IdTipoArticulo As Int64?, ByVal GrupoUnidad As Int32?) As List(Of INV_Articulos_Get_Result)
            Return oMatrixContext.INV_Articulos_Get(IdTipoArticulo, GrupoUnidad).ToList
        End Function

        Function obtenerListaPapeleria() As List(Of INV_Papeleria_Get_Result)
            Return oMatrixContext.INV_Papeleria_Get().ToList
        End Function

		Function obtenerDispositivosPerifericos() As List(Of INV_Perifericos_Get_Result)
			Return oMatrixContext.INV_Perifericos_Get().ToList
		End Function


		Function obtenerBonosTipoValor() As List(Of INV_ValorBono_Get_Result)
            Return oMatrixContext.INV_ValorBono_Get().ToList
        End Function

        Function obtenerEstadosTablet() As List(Of INV_EstadoTablet_Get_Result)
            Return oMatrixContext.INV_EstadoTablet_Get().ToList
        End Function

        Function obtenerEstadosArticulos() As List(Of INV_EstadoArticulo_Get_Result)
            Return oMatrixContext.INV_EstadoArticulo_Get().ToList
        End Function

        Function ObtenerEstadosConsumibles() As List(Of INV_EstadoConsumible_Get_Result)
            Return oMatrixContext.INV_EstadoConsumible_Get().ToList
        End Function

        Function obtenerSedes() As List(Of INV_Sede_Get_Result)
            Return oMatrixContext.INV_Sede_Get().ToList
        End Function

        Function obtenerBU() As List(Of INV_BU_Get_Result)
            Return oMatrixContext.INV_BU_Get().ToList
        End Function

        Function obtenerTipoMovimiento(ByVal Permiso As Int32?) As List(Of INV_TipoMovimiento_Get_Result)
            Return oMatrixContext.INV_TipoMovimiento_Get(Permiso).ToList
        End Function

        Function obtenerTipoMovimientoxTodos() As List(Of INV_TipoMovimiento_Get_Result)
            Return oMatrixContext.INV_TipoMovimiento_Get(Nothing).ToList
        End Function

        Function obtenerTipoLegalizacón(ByVal Permiso As Int32?) As List(Of INV_TipoLegalizacion_Get_Result)
            Return oMatrixContext.INV_TipoLegalizacion_Get(Permiso).ToList
        End Function

        Function obtenerTipoLegalizacionxTodos() As List(Of INV_TipoLegalizacion_Get_Result)
            Return oMatrixContext.INV_TipoLegalizacion_Get(Nothing).ToList
        End Function

        Function ObtenerStockxLegalizar(ByVal BU As Int32?, ByVal JobBookCodigo As String, ByVal Articulo As Int64?, ByVal TipoProducto As Int64?, ByVal UsuarioRegistra As Int64?, ByVal UsuarioAsignado As Int64?, ByVal IdConsumible As Int64?) As List(Of INV_StockxLegalizar_Get_Result)
            Return oMatrixContext.INV_StockxLegalizar_Get(BU, JobBookCodigo, Articulo, TipoProducto, UsuarioRegistra, UsuarioAsignado, IdConsumible).ToList
        End Function

        Function ObtenerStockxLegalizarxUsuarioAsignado(ByVal BU As Int32?, ByVal JobBookCodigo As String, ByVal Articulo As Int64?, ByVal UsuarioAsignado As Int64?, ByVal IdConsumible As Int64?) As List(Of INV_StockxLegalizar_Get_Result)
            Return oMatrixContext.INV_StockxLegalizar_Get(BU, JobBookCodigo, Articulo, Nothing, Nothing, UsuarioAsignado, IdConsumible).ToList
        End Function

        Function ObtenerLegalizaciones(ByVal IdConsumible As Int64?, ByVal BU As Int32?, ByVal JobBookCodigo As String, ByVal Articulo As Int64?, ByVal UsuarioRegistra As Int64?, ByVal UsuarioAsignado As Int64?) As List(Of INV_Legalizaciones_Get_Result)
            Return oMatrixContext.INV_Legalizaciones_Get(Nothing, IdConsumible, BU, JobBookCodigo, Articulo, Nothing, UsuarioRegistra, UsuarioAsignado).ToList
        End Function

        Function ObtenerLegalizacionesxId(ByVal Id As Int64?) As List(Of INV_Legalizaciones_Get_Result)
            Return oMatrixContext.INV_Legalizaciones_Get(Id, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).ToList
        End Function

        Function ObtenerLegalizacionesxUsuario(ByVal IdConsumible As Int64?, ByVal UsuarioAsignado As Int64?) As List(Of INV_Legalizaciones_Get_Result)
            Return oMatrixContext.INV_Legalizaciones_Get(Nothing, IdConsumible, Nothing, Nothing, Nothing, Nothing, Nothing, UsuarioAsignado).ToList
        End Function

        Function ObtenerReporteLegalizaciones(ByVal Fechainicio As DateTime?, ByVal Fechafin As DateTime?, ByVal UsuarioAsignado As Int64?, ByVal Articulo As Int64?, ByVal BU As Int32?, ByVal JobBookCodigo As String, ByVal TodosCampos As String) As List(Of INV_ReporteLegalizaciones_Result)
            Return oMatrixContext.INV_ReporteLegalizaciones(Fechainicio, Fechafin, UsuarioAsignado, Articulo, BU, JobBookCodigo, TodosCampos).ToList
        End Function

        Function ObtenerMantenimientoEquipos(ByVal Id As Int64?, ByVal IdActivoFijo As Int64?, ByVal Articulo As Int64?, ByVal TipoMantenimiento As Int32?, ByVal IdUsuarioResponsable As Int64?, ByVal UsuarioResponsable As String) As List(Of INV_MantenimientoEquipos_Get_Result)
            Return oMatrixContext.INV_MantenimientoEquipos_Get(Id, IdActivoFijo, Articulo, TipoMantenimiento, IdUsuarioResponsable, UsuarioResponsable).ToList
        End Function

        Function ObtenerMantenimientoEquiposxIdActivoFijo(ByVal IdActivoFijo As Int64?) As List(Of INV_MantenimientoEquipos_Get_Result)
            Return oMatrixContext.INV_MantenimientoEquipos_Get(Nothing, IdActivoFijo, Nothing, Nothing, Nothing, Nothing).ToList
        End Function

        Function ObtenerMantenimientoEquiposxId(ByVal Id As Int64?) As List(Of INV_MantenimientoEquipos_Get_Result)
            Return oMatrixContext.INV_MantenimientoEquipos_Get(Id, Nothing, Nothing, Nothing, Nothing, Nothing).ToList
        End Function

        Function ObtenerReporteRemanente(ByVal IdConsumible As Int64?, ByVal Articulo As Int64?, ByVal TipoProducto As Int64?, ByVal JobBook As String) As List(Of INV_ReporteRemanente_Result)
            Return oMatrixContext.INV_ReporteRemanente(IdConsumible, Articulo, TipoProducto, JobBook).ToList
        End Function

#End Region

#Region "Guardar"

        Public Function GuardarRegistroArticulos(ByVal TipoArticulo As Int64?, ByVal Articulo As Int64?, ByVal FechaCompra As DateTime?, ByVal UsuarioRegistra As Int64?, ByVal CentroCosto As Int16?, ByVal BU As Int32?, ByVal IdTrabajo As Int64?, ByVal JobBookCodigo As String, ByVal JobBookNombre As String, ByVal CuentaContable As Int64?, ByVal ValorUnitario As Int64?, ByVal Estado As Int64?, ByVal descripcion As String, ByVal Symphony As String, ByVal IdFisico As Int64?, ByVal Sede As Int64?, ByVal TipoComputador As Int64?, ByVal PertenecePC As Int16?, ByVal TipoPeriferico As Int64?, ByVal Marca As String, ByVal Modelo As String, ByVal Procesador As String, ByVal Memoria As String, ByVal Almacenamiento As String, ByVal SistemaOperativo As String, ByVal Serial As String, ByVal NombreEquipo As String, ByVal Office As String, ByVal Programas As String, ByVal TipoServidor As String, ByVal Raid As String, ByVal IdTablet As Int64?, ByVal IdSTG As Int64?, ByVal TamanoPantalla As String, ByVal Chip As Int64?, ByVal IMEI As Int64?, ByVal Pertenece As Int64?, ByVal Operador As Int64?, ByVal NumeroCelular As Int64?, ByVal CantidadMinutos As Integer?, ByVal TipoProducto As Int64?, ByVal Producto As String, ByVal TipoObsequio As Int16?, ByVal TipoBono As Int64?, ByVal Asignado As Boolean?, ByVal FechaFinRenta As DateTime?, ByVal NumeroPV As Int64?, ByVal ProveedorId As Int64?, ByVal Cantidad As Int64?, ByVal ProductoPapeleria As Int64?) As Decimal
            Return oMatrixContext.INV_RegistroArticulos_Add(TipoArticulo, Articulo, FechaCompra, UsuarioRegistra, CentroCosto, BU, IdTrabajo, JobBookCodigo, JobBookNombre, CuentaContable, ValorUnitario, Estado, descripcion, Symphony, IdFisico, Sede, TipoComputador, PertenecePC, TipoPeriferico, Marca, Modelo, Procesador, Memoria, Almacenamiento, SistemaOperativo, Serial, NombreEquipo, Office, Programas, TipoServidor, Raid, IdTablet, IdSTG, TamanoPantalla, Chip, IMEI, Pertenece, Operador, NumeroCelular, CantidadMinutos, TipoProducto, Producto, TipoObsequio, TipoBono, Asignado, FechaFinRenta, NumeroPV, ProveedorId, Cantidad, ProductoPapeleria).FirstOrDefault
        End Function

        Public Sub ActualizarRegistroArticulos(ByVal Id As Int64?, ByVal TipoArticulo As Int64?, ByVal Articulo As Int64?, ByVal FechaCompra As Date?, ByVal UsuarioRegistra As Int64?, ByVal CentroCosto As Int16?, ByVal BU As Int32?, ByVal IdTrabajo As Int64?, ByVal JobBookCodigo As String, ByVal JobBookNombre As String, ByVal CuentaContable As Int64?, ByVal ValorUnitario As Int64?, ByVal Estado As Int64?, ByVal descripcion As String, ByVal Symphony As String, ByVal IdFisico As Int64?, ByVal Sede As Int64?, ByVal TipoComputador As Int64?, ByVal PertenecePC As Int16?, ByVal TipoPeriferico As Int64?, ByVal Marca As String, ByVal Modelo As String, ByVal Procesador As String, ByVal Memoria As String, ByVal Almacenamiento As String, ByVal SistemaOperativo As String, ByVal Serial As String, ByVal NombreEquipo As String, ByVal Office As String, ByVal Programas As String, ByVal TipoServidor As String, ByVal Raid As String, ByVal IdTablet As Int64?, ByVal IdSTG As Int64?, ByVal TamanoPantalla As String, ByVal Chip As Int64?, ByVal IMEI As Int64?, ByVal Pertenece As Int64?, ByVal Operador As Int64?, ByVal NumeroCelular As Int64?, ByVal CantidadMinutos As Integer?, ByVal TipoProducto As Int64?, ByVal Producto As String, ByVal TipoObsequio As Int16?, ByVal TipoBono As Int64?, ByVal Asignado As Boolean?, ByVal FechaFinRenta As DateTime?, ByVal NumeroPV As Int64?, ByVal ProveedorId As Int64?, ByVal Cantidad As Int64?, ByVal ProductoPapeleria As Int64?)
            oMatrixContext.INV_RegistroArticulos_Edit(Id, TipoArticulo, Articulo, FechaCompra, UsuarioRegistra, CentroCosto, BU, IdTrabajo, JobBookCodigo, JobBookNombre, CuentaContable, ValorUnitario, Estado, descripcion, Symphony, IdFisico, Sede, TipoComputador, PertenecePC, TipoPeriferico, Marca, Modelo, Procesador, Memoria, Almacenamiento, SistemaOperativo, Serial, NombreEquipo, Office, Programas, TipoServidor, Raid, IdTablet, IdSTG, TamanoPantalla, Chip, IMEI, Pertenece, Operador, NumeroCelular, CantidadMinutos, TipoProducto, Producto, TipoObsequio, TipoBono, Asignado, FechaFinRenta, NumeroPV, ProveedorId, Cantidad, ProductoPapeleria)
        End Sub

        Public Sub ActualizarRegistroArticulos_Asignado(ByVal Id As Int64?, ByVal Asignado As Boolean?)
            oMatrixContext.INV_RegistroArticulos_Asignado_Edit(Id, Asignado)
        End Sub

        Public Function GuardarAsignaciones(ByVal IdActivoFijo As Int64?, ByVal UsuarioRegistra As Int64?, ByVal FechaAsignacion As DateTime?, ByVal CentroCosto As Int16?, ByVal BU As Int32?, ByVal IdTrabajo As Int64?, ByVal JobBookCodigo As String, ByVal JobBookNombre As String, ByVal Ciudad As Int64?, ByVal EstadoTablet As Int64?, ByVal UsuarioAsignado As Int64?, ByVal TipoCargo As Int16?, ByVal Cargo As String, ByVal Observacion As String, ByVal Sede As Int32?, ByVal TipoGrupoUnidad As Int16?, ByVal GrupoUnidad As Int32?, ByVal Unidad As Int32?) As Decimal
            Return oMatrixContext.INV_Asignaciones_Add(IdActivoFijo, UsuarioRegistra, FechaAsignacion, CentroCosto, BU, IdTrabajo, JobBookCodigo, JobBookNombre, Ciudad, EstadoTablet, UsuarioAsignado, TipoCargo, Cargo, Observacion, Sede, TipoGrupoUnidad, GrupoUnidad, Unidad).FirstOrDefault
        End Function

        Public Sub ActualizarAsignaciones(ByVal Id As Int64?, ByVal IdActivoFijo As Int64?, ByVal UsuarioRegistra As Int64?, ByVal FechaAsignacion As DateTime?, ByVal CentroCosto As Int16?, ByVal BU As Int32?, ByVal IdTrabajo As Int64?, ByVal JobBookCodigo As String, ByVal JobBookNombre As String, ByVal Ciudad As Int64?, ByVal EstadoTablet As Int64?, ByVal UsuarioAsignado As Int64?, ByVal TipoCargo As Int16?, ByVal Cargo As String, ByVal Observacion As String, ByVal Sede As Int32?, ByVal TipoGrupoUnidad As Int16?, ByVal GrupoUnidad As Int32?, ByVal Unidad As Int32?)
            oMatrixContext.INV_Asignaciones_Edit(Id, IdActivoFijo, UsuarioRegistra, FechaAsignacion, CentroCosto, BU, IdTrabajo, JobBookCodigo, JobBookNombre, Ciudad, EstadoTablet, UsuarioAsignado, TipoCargo, Cargo, Observacion, Sede, TipoGrupoUnidad, GrupoUnidad, Unidad)
        End Sub

        Public Function GuardarLogAsignaciones(ByVal IdActivoFijo As Int64?, ByVal IdArticulo As Int64?, ByVal IdUsuario As Int64?, ByVal CentroCosto As Int16?, ByVal BU As Int32?, ByVal IdTrabajo As Int64?, ByVal JobBookCodigo As String, ByVal JobBookNombre As String, ByVal Ciudad As Int64?, ByVal EstadoTablet As Int64?, ByVal Asignado As Boolean?) As Decimal
            Return oMatrixContext.INV_LogAsignaciones_Add(IdActivoFijo, IdArticulo, IdUsuario, CentroCosto, BU, IdTrabajo, JobBookCodigo, JobBookNombre, Ciudad, EstadoTablet, Asignado).FirstOrDefault
        End Function

        Public Function GuardarStockConsumibles(ByVal IdConsumible As Int64?, ByVal NumeroVale As Int64?, ByVal Fecha As DateTime?, ByVal UsuarioRegistra As Int64?, ByVal TipoMovimiento As Int16?, ByVal Estado As Int16?, ByVal CentroCosto As Int16?, ByVal BU As Int32?, ByVal IdTrabajo As Int64?, ByVal JobBookCodigo As String, ByVal JobBookNombre As String, ByVal CuentaContable As Int64?, ByVal Ciudad As Int64?, ByVal Valor As Int64?, ByVal Total As Int64?, ByVal Disponible As Int64?, ByVal UsuarioAsignado As Int64?, ByVal TipoCargo As Int32?, ByVal Observaciones As String) As Decimal
            Return oMatrixContext.INV_StockConsumibles_Add(IdConsumible, NumeroVale, Fecha, UsuarioRegistra, TipoMovimiento, Estado, CentroCosto, BU, IdTrabajo, JobBookCodigo, JobBookNombre, CuentaContable, Ciudad, Valor, Total, Disponible, UsuarioAsignado, TipoCargo, Observaciones).FirstOrDefault
        End Function

        Public Function GuardarLegalizaciones(ByVal IdConsumible As Int64?, ByVal UsuarioRegistra As Int64?, ByVal TipoLegalizacion As Int16?, ByVal Radicado As String, ByVal Fecha As DateTime?, ByVal UsuarioResponsable As Int64?, ByVal Unidades As Int32?, ByVal Firmas As Int64?, ByVal Devoluciones As Int64?, ByVal NotasCredito As Int64?, ByVal DescuentoNomina As Int64?, ByVal ValorLegalizado As Int64?, ByVal Pendiente As Int64?, ByVal Observaciones As String, ByVal Legalizado As Boolean?, ByVal CentroCosto As Int16?, ByVal BU As Int32?, ByVal JobBook As Int64?, ByVal JobBookCodigo As String, ByVal JobBookNombre As String, ByVal ValorCarrera As Int64?, ByVal Verificado As Boolean?, ByVal FechaVerificacion As DateTime?, ByVal UsuarioVerifica As Int64?) As Decimal
            Return oMatrixContext.INV_Legalizaciones_Add(IdConsumible, UsuarioRegistra, TipoLegalizacion, Radicado, Fecha, UsuarioResponsable, Unidades, Firmas, Devoluciones, NotasCredito, DescuentoNomina, ValorLegalizado, Pendiente, Observaciones, Legalizado, CentroCosto, BU, JobBook, JobBookCodigo, JobBookNombre, ValorCarrera, Verificado, FechaVerificacion, UsuarioVerifica).FirstOrDefault
        End Function

        Public Sub ActualizarLegalizaciones(ByVal Id As Int64?, ByVal IdConsumible As Int64?, ByVal UsuarioRegistra As Int64?, ByVal TipoLegalizacion As Int16?, ByVal Radicado As String, ByVal Fecha As DateTime?, ByVal UsuarioResponsable As Int64?, ByVal Unidades As Int32?, ByVal Firmas As Int64?, ByVal Devoluciones As Int64?, ByVal NotasCredito As Int64?, ByVal DescuentoNomina As Int64?, ByVal ValorLegalizado As Int64?, ByVal Pendiente As Int64?, ByVal Observaciones As String, ByVal Legalizado As Boolean?, ByVal CentroCosto As Int16?, ByVal BU As Int32?, ByVal JobBook As Int64?, ByVal JobBookCodigo As String, ByVal JobBookNombre As String, ByVal ValorCarrera As Int64?, ByVal Verificado As Boolean?, ByVal FechaVerificacion As DateTime?, ByVal UsuarioVerifica As Int64?)
            oMatrixContext.INV_Legalizaciones_Edit(Id, IdConsumible, UsuarioRegistra, TipoLegalizacion, Radicado, Fecha, UsuarioResponsable, Unidades, Firmas, Devoluciones, NotasCredito, DescuentoNomina, ValorLegalizado, Pendiente, Observaciones, Legalizado, CentroCosto, BU, JobBook, JobBookCodigo, JobBookNombre, ValorCarrera, Verificado, FechaVerificacion, UsuarioVerifica)
        End Sub

        Public Function GuardarMantenimientoEquipos(ByVal IdActivoFijo As Int64?, ByVal UsuarioRegistra As Int64?, ByVal Fecha As Date?, ByVal TipoMantenimiento As Int32?, ByVal UsuarioResponsable As Int64?, ByVal Observaciones As String) As Decimal
            Return oMatrixContext.INV_MantenimientoEquipos_Add(IdActivoFijo, UsuarioRegistra, Fecha, TipoMantenimiento, UsuarioResponsable, Observaciones).FirstOrDefault
        End Function

        Public Sub ActualizarMantenimientoEquipos(ByVal Id As Int64?, ByVal IdActivoFijo As Int64?, ByVal UsuarioRegistra As Int64?, ByVal Fecha As Date?, ByVal TipoMantenimiento As Int32?, ByVal UsuarioResponsable As Int64?, ByVal Observaciones As String)
            oMatrixContext.INV_MantenimientoEquipos_Edit(Id, IdActivoFijo, UsuarioRegistra, Fecha, TipoMantenimiento, UsuarioResponsable, Observaciones)
        End Sub

#End Region

#Region "Eliminar"
        Public Function EliminarAsignacion(ByVal IdActivoFijo As Int64) As Integer
            Return oMatrixContext.INV_Asignaciones_Del(IdActivoFijo)
        End Function
        Public Function EliminarLegalizacion(ByVal Id As Int64) As Integer
            Return oMatrixContext.INV_Legalizaciones_Del(Id)
        End Function
#End Region

    End Class
End Namespace