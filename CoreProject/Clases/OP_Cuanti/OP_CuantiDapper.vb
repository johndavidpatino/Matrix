Imports System.Configuration
Imports System.Data.SqlClient
Imports Dapper

Public Class OP_CuantiDapper
#Region "Variables Globales"
	Dim sqlConnectionString = ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString


#End Region
	Public Function CuantiPlanillasGet(Revisado As Boolean?, PMO As Int64?, Fini As Date?, Ffin As Date?, TrabajoId As Int64?, Coordinador As Int64?) As List(Of DTO.OP_CuantiPlanillasModel)
		Dim params = New With {
			.Revisado = Revisado,
			.PMO = PMO,
			.Fini = Fini,
			.Ffin = Ffin,
			.TrabajoId = TrabajoId,
			.Coordinador = Coordinador
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of DTO.OP_CuantiPlanillasModel)(sql:="OP_CuantiPlanillas_GET", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToList
		End Using
	End Function

	Public Function CuantiPlanillasTrabajosGet(Revisado As Boolean?, PMO As Int64?, Fini As Date?, Ffin As Date?, TrabajoId As Int64?, Optional Coordinador As Int64? = Nothing) As List(Of DTO.OP_CuantiPlanillasTrabajosModel)
		Dim params = New With {
			.Revisado = Revisado,
			.PMO = PMO,
			.Fini = Fini,
			.Ffin = Ffin,
			.TrabajoId = TrabajoId,
			.Coordinador = Coordinador
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of DTO.OP_CuantiPlanillasTrabajosModel)(sql:="OP_CuantiPlanillasTrabajos_GET", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToList
		End Using
	End Function

	Public Function CuantiPlanillasPendientesGet(Revisado As Boolean?, PMO As Int64?, Fini As Date?, Ffin As Date?, TrabajoId As Int64?) As List(Of DTO.OP_CuantiPlanillasPendientesModel)
		Dim params = New With {
			.Revisado = Revisado,
			.PMO = PMO,
			.Fini = Fini,
			.Ffin = Ffin,
			.TrabajoId = TrabajoId
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of DTO.OP_CuantiPlanillasPendientesModel)(sql:="OP_CuantiPlanillasPendientes_GET", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToList
		End Using
	End Function

	Public Function CuantiPlanillasTrabajosUpdate(Revisado As Boolean?, PMO As Int64?, Fini As Date?, Ffin As Date?, TrabajoId As Int64?, UsuarioRevisa As Int64?) As String
		Dim params = New With {
			.Revisado = Revisado,
			.PMO = PMO,
			.Fini = Fini,
			.Ffin = Ffin,
			.TrabajoId = TrabajoId,
			.UsuarioRevisa = UsuarioRevisa
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of String)(sql:="OP_CuantiPlanillas_Update", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToString
		End Using
	End Function

	Public Function CuantiPlanillasTrabajosRemove(Revisado As Boolean?, PMO As Int64?, Fini As Date?, Ffin As Date?, TrabajoId As Int64?, UsuarioRevisa As Int64?) As String
		Dim params = New With {
			.Revisado = Revisado,
			.PMO = PMO,
			.Fini = Fini,
			.Ffin = Ffin,
			.TrabajoId = TrabajoId
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of String)(sql:="OP_CuantiPlanillas_Remove", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToString
		End Using
	End Function

	Public Function CuantiPlanillasLiquidar(Revisado As Boolean?, PMO As Int64?, Fini As Date?, Ffin As Date?, TrabajoId As Int64?) As String
		Dim params = New With {
			.Revisado = Revisado,
			.PMO = PMO,
			.Fini = Fini,
			.Ffin = Ffin,
			.TrabajoId = TrabajoId
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of String)(sql:="OP_CuantiPlanillas_Liquidar", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToString
		End Using
	End Function

	Public Function CuantiPlanillasSinPresupuestoGet(Revisado As Boolean?, PMO As Int64?, Fini As Date?, Ffin As Date?, TrabajoId As Int64?) As List(Of DTO.OP_CuantiPlanillasSinPresupuestoModel)
		Dim params = New With {
			.Revisado = Revisado,
			.PMO = PMO,
			.Fini = Fini,
			.Ffin = Ffin,
			.TrabajoId = TrabajoId
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of DTO.OP_CuantiPlanillasSinPresupuestoModel)(sql:="OP_CuantiPlanillasSinPresupuesto_GET", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToList
		End Using
	End Function

	Public Function CuantiProduccionUpdate(Fini As Date?, Ffin As Date?, Usuario As Int64?) As String
		Dim params = New With {
			.FIni = Fini,
			.FFin = Ffin,
			.Usuario = Usuario
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of String)(sql:="OP_CuantiProduccionPSTUpdate", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToString
		End Using
	End Function

	Public Function CuantiProdProductividad_Get(Fini As Date?, Ffin As Date?, Coordinador As Int64?, PMO As Int64?, TrabajoId As Int64?, MetodologiaAgrupada As Integer?,
												AprobadoCoordinador As Boolean?, AprobadoJefe As Boolean?, AprobadoPMO As Boolean?, EnProduccion As Boolean?) As List(Of DTO.OP_CuantiProduccionProductividad)
		Dim params = New With {
			.FIni = Fini,
			.FFin = Ffin,
			.Coordinador = Coordinador,
			.PMO = PMO,
			.TrabajoId = TrabajoId,
			.MetodologiaAgrupada = MetodologiaAgrupada,
			.AprobadoCoordinador = AprobadoCoordinador,
			.AprobadoJefe = AprobadoJefe,
			.AprobadoPMO = AprobadoPMO,
			.EnProduccion = EnProduccion
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of DTO.OP_CuantiProduccionProductividad)(sql:="OP_CuantiProduccionProductividad_GET", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToList
		End Using
	End Function

	Public Function CuantiProdProductividadTrabajos_Get(Fini As Date?, Ffin As Date?, Coordinador As Int64?, PMO As Int64?, TrabajoId As Int64?, MetodologiaAgrupada As Integer?,
														AprobadoCoordinador As Boolean?, AprobadoJefe As Boolean?, AprobadoPMO As Boolean?, EnProduccion As Boolean?) As List(Of DTO.OP_CuantiPlanillasTrabajosModel)
		Dim params = New With {
			.FIni = Fini,
			.FFin = Ffin,
			.Coordinador = Coordinador,
			.PMO = PMO,
			.TrabajoId = TrabajoId,
			.MetodologiaAgrupada = MetodologiaAgrupada,
			.AprobadoCoordinador = AprobadoCoordinador,
			.AprobadoJefe = AprobadoJefe,
			.AprobadoPMO = AprobadoPMO,
			.EnProduccion = EnProduccion
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of DTO.OP_CuantiPlanillasTrabajosModel)(sql:="OP_CuantiProduccionProductividadTrabajos_GET", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToList
		End Using
	End Function

	Public Function CuantiProduccionPrevioTrabajo(TrabajoId As Int64?) As List(Of DTO.OP_CuantiMontosTrabajo)
		Dim params = New With {
			.TrabajoId = TrabajoId
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of DTO.OP_CuantiMontosTrabajo)(sql:="OP_CuantiProduccionPrevioPorTrabajo", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToList
		End Using
	End Function

	Public Function CuantiProdProductividadStatus_Get(Fini As Date?, Ffin As Date?, Coordinador As Int64?, PMO As Int64?, TrabajoId As Int64?, MetodologiaAgrupada As Integer?,
												AprobadoCoordinador As Boolean?, AprobadoJefe As Boolean?, AprobadoPMO As Boolean?) As List(Of DTO.OP_CuantiProduccionProductividadStatus)
		Dim params = New With {
			.FIni = Fini,
			.FFin = Ffin,
			.Coordinador = Coordinador,
			.PMO = PMO,
			.TrabajoId = TrabajoId,
			.MetodologiaAgrupada = MetodologiaAgrupada,
			.AprobadoCoordinador = AprobadoCoordinador,
			.AprobadoJefe = AprobadoJefe,
			.AprobadoPMO = AprobadoPMO
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of DTO.OP_CuantiProduccionProductividadStatus)(sql:="OP_CuantiProduccionProductividadStatus_GET", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToList
		End Using
	End Function

	Public Function CuantiProduccionActualTrabajo(TrabajoId As Int64?) As List(Of DTO.OP_CuantiMontosTrabajo)
		Dim params = New With {
			.TrabajoId = TrabajoId
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of DTO.OP_CuantiMontosTrabajo)(sql:="OP_CuantiProduccionTempoPorTrabajo", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToList
		End Using
	End Function

	Public Function CuantiProduccionLiquidarProductividad(Fini As Date?, Ffin As Date?, Coordinador As Int64?, PMO As Int64?, TrabajoId As Int64?, MetodologiaAgrupada As Integer?,
												AprobadoCoordinador As Boolean?, AprobadoJefe As Boolean?, AprobadoPMO As Boolean?, UsuarioId As Int64) As String
		Dim params = New With {
			.FIni = Fini,
			.FFin = Ffin,
			.Coordinador = Coordinador,
			.PMO = PMO,
			.TrabajoId = TrabajoId,
			.MetodologiaAgrupada = MetodologiaAgrupada,
			.AprobadoCoordinador = AprobadoCoordinador,
			.AprobadoJefe = AprobadoJefe,
			.AprobadoPMO = AprobadoPMO,
			.UsuarioId = UsuarioId
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of String)(sql:="OP_CuantiProduccionProductividad_EnviarAProduccion", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToString
		End Using
	End Function

	Public Function CuantiProduccionResumenCorte(Fini As Date?, Ffin As Date?) As List(Of DTO.OP_CuantiResumenProduccionCorte)
		Dim params = New With {
			.FIni = Fini,
			.FFin = Ffin
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of DTO.OP_CuantiResumenProduccionCorte)(sql:="OP_CuantiResumenProduccionCorte_GET", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToList
		End Using
	End Function

	Public Function CuantiProduccionResumenCorteBono(Fini As Date?, Ffin As Date?) As List(Of DTO.OP_CuantiResumenProduccionCorteBono)
		Dim params = New With {
			.FIni = Fini,
			.FFin = Ffin
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of DTO.OP_CuantiResumenProduccionCorteBono)(sql:="OP_CuantiResumenProduccionCorte_Bono_Jobs_GET", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToList
		End Using
	End Function

	Public Function CuantiProduccionResumenCorteResumenTercero(Fini As Date?, Ffin As Date?) As List(Of DTO.OP_CuantiResumenProduccionCorteResumenTercero)
		Dim params = New With {
			.FIni = Fini,
			.FFin = Ffin
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of DTO.OP_CuantiResumenProduccionCorteResumenTercero)(sql:="OP_CuantiResumenProduccionCorte_Resumen_Tercero_GET", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToList
		End Using
	End Function

	Public Function CuantiProduccionDistribucionSS(Fini As Date?, Ffin As Date?) As List(Of DTO.OP_CuantiProduccionDistribucionCC)
		Dim params = New With {
			.FIni = Fini,
			.FFin = Ffin
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of DTO.OP_CuantiProduccionDistribucionCC)(sql:="OP_CuantiProduccionDistribucionSS_GET", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToList
		End Using
	End Function

	Public Function CarguesProduccionGet(id As Int64?, Fini As Date?, Ffin As Date?, Usuario As Int64?) As List(Of DTO.CC_CarguesProduccionGet2)
		Dim params = New With {
			.id = id,
			.FechaInicio = Fini,
			.FechaFin = Ffin,
			.Usuario = Usuario
			}
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of DTO.CC_CarguesProduccionGet2)(sql:="CC_CarguesProduccionGet", commandType:=CommandType.StoredProcedure, param:=params)
			Return result.ToList
		End Using
	End Function

	Public Class DTO
		Public Class OP_CuantiPlanillasModel
			Public Property Id As Integer
			Public Property TrabajoId As Integer
			Public Property NombreTrabajo As String
			Public Property PMO As String
			Public Property PersonaId As Int64
			Public Property NombrePersona As String
			Public Property Ciudad As String
			Public Property Res_Fecha As Nullable(Of DateTime)
			Public Property Cantidad As Integer
			Public Property TipoActividad As Integer
			Public Property TipoActividadDescripcion As String
			Public Property UsuarioCarga As String
			Public Property FechaCarga As Nullable(Of DateTime)
			Public Property Revisado As Boolean
			Public Property RevisadoPor As String
			Public Property FechaRevision As Nullable(Of DateTime)
		End Class

		Public Class OP_CuantiPlanillasTrabajosModel
			Public Property TrabajoId As Integer
			Public Property NombreTrabajo As String
		End Class

		Public Class OP_CuantiPlanillasPendientesModel
			Public Property TrabajoId As Integer
			Public Property NombreTrabajo As String
			Public Property PMO As String
			Public Property PeriodoInicial As Nullable(Of DateTime)
			Public Property PeriodoFinal As Nullable(Of DateTime)
			Public Property Registros As Integer
		End Class

		Public Class OP_CuantiPlanillasSinPresupuestoModel
			Public Property TrabajoId As Integer
			Public Property NombreTrabajo As String
			Public Property PMO As String
			Public Property TipoActividad As String
		End Class

		Public Class OP_CuantiProduccionProductividad
			Public Property id As Integer
			Public Property TrabajoId As Integer
			Public Property JobBook As String
			Public Property NombreTrabajo As String
			Public Property Metodologia As String
			Public Property Cedula As String
			Public Property FechaEjecucion As Date
			Public Property Cantidad As Integer
			Public Property Cargo As Integer
			Public Property TipoContratacion As Integer
			Public Property CiudadPersona As Integer
			Public Property CiudadEncuesta As Integer
			Public Property Nombre As String
			Public Property TipoContratacionNombre As String
			Public Property CargoMatrix As String
			Public Property Ciudad As String
			Public Property PMO As String
			Public Property Coordinador As String
			Public Property StatusPresupuesto As String
			Public Property VrUnitario As Decimal
			Public Property VrTotal As Decimal
			Public Property IdMetodologia As Integer
			Public Property IDPMO As Integer?
			Public Property IDCoordinador As Integer?
			Public Property CantidadCoordinador As Integer?
			Public Property FechaRevisaCoordinador As Date?
			Public Property CantidadJefe As Integer?
			Public Property FechaRevisaJefe As Date?
			Public Property CantidadPMO As Integer?
			Public Property FechaRevisaPMO As Date?
			Public Property ObservacionesCoordinador As String
			Public Property ObservacionesJefe As String
			Public Property ObservacionesPMO As String
		End Class

		Public Class OP_CuantiMontosTrabajo
			Public Property RowNum As Integer
			Public Property Ciudad As String
			Public Property Cantidad As Integer
			Public Property Monto As Double
		End Class

		Public Class OP_CuantiProduccionProductividadStatus
			Public Property TrabajoId As Integer
			Public Property JobBook As String
			Public Property NombreTrabajo As String
			Public Property StatusPresupuesto As String
			Public Property Metodologia As String
			Public Property PMO As String
			Public Property AprobadoCampo As String
			Public Property AprobadoPMO As String
			Public Property VrUnitario As Decimal?
			Public Property Cargo As Integer
		End Class

		Public Class OP_CuantiResumenProduccionCorte
			Public Property Cedula As Int64
			Public Property Nombre As String
			Public Property CargoMatrix As String
			Public Property IdIStaff As Int64
			Public Property Ciudad As String
			Public Property Cargo As String
			Public Property Cantidad As Integer
			Public Property DiasTrabajados As Integer
			Public Property VrTransporte As Double
			Public Property VrProduccion As Double
			Public Property VrBono As Double
			Public Property ValorSS As Double
			Public Property SaldoSS As Double
			Public Property ValorICA As Double
			Public Property TotalDescuento As Double
			Public Property Subtotal As Double
			Public Property TotalAPagar As Double

		End Class

		Public Class OP_CuantiResumenProduccionCorteBono
			Public Property Cedula As Int64
			Public Property PresupuestoId As Integer
			Public Property Symphony As Int64
			Public Property JobBook As Int64
			Public Property VrBono As Double
		End Class
		Public Class OP_CuantiResumenProduccionCorteResumenTercero
			Public Property Cedula As Int64
			Public Property Symphony As Int64
			Public Property VrBono As Double
		End Class
		Public Class OP_CuantiProduccionDistribucionCC
			Public Property Cedula As Int64
			Public Property Symphony As Int64
			Public Property JBI As String
			Public Property Presupuesto As String
			Public Property Cuenta As String
			Public Property VrDistribuido As Double
			Public Property Observacion As String

		End Class

		Partial Public Class CC_CarguesProduccionGet2
			Public Property Id As Long
			Public Property Fecha As Nullable(Of Date)
			Public Property Cantidad As Nullable(Of Double)
			Public Property Usuario As Nullable(Of Long)
			Public Property DesdeId As Nullable(Of Double)

			Public Property HastaId As Nullable(Of Double)


		End Class


	End Class
End Class
