Imports System.Configuration
Imports CoreProject.TH_Ausencia.DTO
Imports Dapper
Imports System.Data
Imports System.Data.SqlClient

Namespace TH_Ausencia
	Public Class DTO
		Public Class AusenciaGET
			Public Property ID As Int64
			Public Property IDEMPLEADO As String
			Public Property EMPLEADO As String
			Public Property FInicio As Date?
			Public Property FFin As Date?
			Public Property DiasCalendario As Int16?
			Public Property DiasLaborales As Int16?
			Public Property Tipo As String
			Public Property Estado As String
			Public Property AprobadoPor As String
			Public Property FechaAprobacion As DateTime?
			Public Property VoBo1 As String
			Public Property VoBo2 As String
			Public Property VoBo3 As String
			Public Property RegistradoPor As String
			Public Property FechaRegistro As DateTime?
			Public Property ObservacionesSolicitud As String
			Public Property ObservacionesAprobacion As String

		End Class

		Public Class ClTiposAusencia
			Public Property id As Int16
			Public Property Tipo As String
		End Class

		Public Class DiferenciasDias
			Public Property DiasCalendario As Integer
			Public Property DiasLaborales As Integer
		End Class

		Public Class ResultValidacion
			Public Property Result As Integer
			Public Property MessageResult As String
		End Class

		Public Class DXAsociado
			Public Property CIE
			Public Property DX
			Public Property DXASOCIADO
		End Class

		Public Class BeneficiosPendientes
			Public Property id As Integer
			Public Property Beneficio As String
			Public Property dias As Integer
		End Class

		Public Class Aprobadores
			Public Property id As Int64
			Public Property NombreCompleto As String
		End Class

		Public Class PeriodosCausadosVacaciones
			Public Property id As Int64
			Public Property FIniPeriodo As Date
			Public Property FFinPeriodo As Date
			Public Property DiasDisfrutados As Integer
			Public Property DiasPendientes As Integer
		End Class

		Public Class REP_Vacaciones
			Public Property Identificacion As Int64
			Public Property NombreEmpleado As String
			Public Property AreaSL As String
			Public Property FechaIngreso As Date?
			Public Property DIASDISFRUTADOS As Integer
			Public Property DIASPENDIENTES As Integer
			Public Property ÚltimoPeriodoCausado As String
			Public Property SEISDIAS As String
			Public Property Estado As String

		End Class

		Public Class REP_Beneficios
			Public Property Identificacion As Int64
			Public Property NombreEmpleado As String
			Public Property AreaSL As String
			Public Property FechaIngreso As Date?
			Public Property TipoBeneficio As String
			Public Property FInicio As Date?
			Public Property FFin As Date?
			Public Property DiasCalendario As Int16?
			Public Property DiasLaborales As Int16?
			Public Property Estado As String

		End Class

		Public Class REP_Ausentismo
			Public Property Identificacion As Int64
			Public Property NombreEmpleado As String
			Public Property AreaSL As String
			Public Property FechaIngreso As Date?
			Public Property TipoAusentismo As String
			Public Property FInicio As Date?
			Public Property FFin As Date?
			Public Property DiasCalendario As Int16?
			Public Property DiasLaborales As Int16?
			Public Property Estado As String

		End Class

		Public Class REP_Incapacidades
			Public Property Identificacion As Int64
			Public Property NombreEmpleado As String
			Public Property AreaSL As String
			Public Property FechaIngreso As Date?
			Public Property Edad As Integer?
			Public Property Cargo As String
			Public Property Sede As String
			Public Property DiaSemana As String
			Public Property Mes As String
			Public Property FInicio As Date?
			Public Property FFin As Date?
			Public Property DiasCalendario As Int16?
			Public Property DiasLaborales As Int16?
			Public Property EntidadConsulta As String
			Public Property IPSPrestadora As String
			Public Property NoRegistroMedico As String
			Public Property TipoIncapacidad As String
			Public Property ClaseAusencia As String
			Public Property SOAT As String
			Public Property FechaAccidenteTrabajo As Date?
			Public Property Comentarios As String
			Public Property DXAsociado As String
			Public Property CIE As String
			Public Property CategoriaDX As String
			Public Property Estado As String

		End Class

		Public Class REP_VacacionesNomina
			Public Property Identificacion As Int64
			Public Property NombreEmpleado As String
			Public Property AreaSL As String
			Public Property FechaIngreso As Date?
			Public Property FechaInicio As Date?
			Public Property FechaFin As Date?
			Public Property DiasCalendarioSolicitados As Integer
			Public Property DiasLaboralesSolicitados As Integer
			Public Property DiasCausados As Integer
			Public Property InicioPeriodo As Date?
			Public Property FinPeriodo As Date?
			Public Property FechaSolicitud As Date?
			Public Property AprobadoPor As String
			Public Property FechaAprobacion As Date?
			Public Property VoBoRRHH As String
			Public Property FechaVoBoRRHH As Date?
		End Class

		Public Class REP_SolicitudesPendientes
			Public Property Identificacion As Int64
			Public Property NombreEmpleado As String
			Public Property AreaSL As String
			Public Property DiasCalendarioSolicitados As Integer
			Public Property DiasLaboralesSolicitados As Integer
			Public Property FechaSolicitud As Date?
			Public Property FechaInicio As Date?
			Public Property FechaFin As Date?

			Public Property Manager As String
			Public Property FechaAprobacionManager As Date?
			Public Property Estado As String
			Public Property Tipo As String
		End Class
		Public Class REP_VacacionesDetallado
			Public Property Identificacion As Int64
			Public Property NombreEmpleado As String
			Public Property Area_SL As String
			Public Property IdSolicitud As Int64
			Public Property TipoSolicitud As String
			Public Property FechaSolicitud As DateTime
			Public Property FechaInicioSolicitud As Date
			Public Property FechaFinSolicitud As Date
			Public Property DiasCalendario As UShort
			Public Property DiasLaborales As UShort
			Public Property EstadoSolicitud As String
			Public Property Aprueba As String
			Public Property FechaAprobacion As DateTime?

			Public Property ApruebaRRHH As String
			Public Property FechaAprobacionRRHH As DateTime?
		End Class
	End Class
	Public Class DAL
		Public Enum ETipo
			Graduacion = 5
			IncapacidadMedica = 10
		End Enum
#Region "Variables Globales"
		Private oContext As TH_Entities
#End Region
#Region "Constructores"
		'Constructor public 
		Public Sub New()
			oContext = New TH_Entities
		End Sub
#End Region

		Dim sqlConnectionString = ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString
		Public Function TiposSolicitudesAusencia() As List(Of ClTiposAusencia)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Return db.Query(Of ClTiposAusencia)("SELECT * FROM TH_Ausencia_Tipo").ToList()
			End Using
		End Function
		Public Function RegistrosAusencia(ByVal id As Int64?, ByVal idEmpleado As Int64?, Finicio As Date?, FFin As Date?, Tipo As Integer?, Estado As Integer?, FaprobacionI As Date?, FaprobacionF As Date?, FregistroI As Date?, FregistroF As Date?, Aprobador As Int64?) As List(Of AusenciaGET)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Dim dp As New DynamicParameters()
				dp.Add("@id", id)
				dp.Add("@idEmpleado", idEmpleado)
				dp.Add("@FInicio", Finicio)
				dp.Add("@FFin", FFin)
				dp.Add("@Tipo", Tipo)
				dp.Add("@Estado", Estado)
				dp.Add("@FAprobacionI", FaprobacionI)
				dp.Add("@FAprobacionF", FaprobacionF)
				dp.Add("@FRegistroI", FregistroI)
				dp.Add("@FRegistroF", FregistroF)
				dp.Add("@Aprobador", Aprobador)
				Return db.Query(Of AusenciaGET)("TH_AUSENCIA_GET", dp, commandType:=CommandType.StoredProcedure).ToList
			End Using
		End Function

		Public Function CalculoDias(Finicio As Date?, FFin As Date?, incluirSabadoComoDiaLaboral As Boolean, idEmpleado As Int64) As List(Of DiferenciasDias)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Dim dp As New DynamicParameters()
				dp.Add("@FInicio", Finicio)
				dp.Add("@FFin", FFin)
				dp.Add("@isSaturdayWorkDay", incluirSabadoComoDiaLaboral)
				dp.Add("@idEmpleado", idEmpleado)
				Return db.Query(Of DiferenciasDias)("TH_Ausencia_DiferenciaDias", dp, commandType:=CommandType.StoredProcedure).ToList
			End Using
		End Function


		Public Sub AddSolicitudAusencia(ByRef ent As TH_SolicitudAusencia)
			oContext.TH_SolicitudAusencia.Add(ent)
			oContext.SaveChanges()
		End Sub

		Public Function GetSolicitudAusencia(ByVal id As Int64) As TH_SolicitudAusencia
			Return oContext.TH_SolicitudAusencia.Where(Function(x) x.id = id).FirstOrDefault
		End Function

		Public Sub AddIncapacidad(ByRef ent As TH_Ausencia_Incapacidades)
			oContext.TH_Ausencia_Incapacidades.Add(ent)
			oContext.SaveChanges()
		End Sub

		Public Function GetIncapacidad(ByVal id As Int64) As TH_Ausencia_Incapacidades
			Return oContext.TH_Ausencia_Incapacidades.Where(Function(x) x.idSolicitudAusencia = id).FirstOrDefault
		End Function

		Public Function ValidarSolicitudAusencia(ByVal idEmpleado As Int64?, Finicio As Date?, FFin As Date?, Tipo As Integer?) As List(Of ResultValidacion)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Dim dp As New DynamicParameters()
				dp.Add("@idEmpleado", idEmpleado)
				dp.Add("@FInicio", Finicio)
				dp.Add("@FFin", FFin)
				dp.Add("@Tipo", Tipo)
				Return db.Query(Of ResultValidacion)("TH_ValidarSolicitudAusencia", dp, commandType:=CommandType.StoredProcedure).ToList
			End Using
		End Function

		Public Sub CausarVacaciones(ByVal idSolicitud As Int64)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Dim dp As New DynamicParameters()
				dp.Add("@IDSOLICITUD", idSolicitud)
				db.Execute("TH_CausarPeriodoVacaciones", dp, commandType:=CommandType.StoredProcedure)
			End Using
		End Sub
		Public Sub AnularSolicitud(ByVal idSolicitud As Int64)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Dim dp As New DynamicParameters()
				dp.Add("@IDSOLICITUD", idSolicitud)
				db.Execute("TH_SolicitudAusenciaAnular", dp, commandType:=CommandType.StoredProcedure)
			End Using
		End Sub

		Public Function ListadoDXAsociado() As List(Of String)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Return db.Query(Of String)("SELECT DISTINCT DXASOCIADO FROM TH_DX ORDER BY DXASOCIADO").ToList()
			End Using
		End Function

		Public Function ListadoDX(ByVal DXAsociado As String) As List(Of DXAsociado)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Return db.Query(Of DXAsociado)("SELECT CIE, DX, DXASOCIADO FROM TH_DX WHERE DXASOCIADO='" & DXAsociado & "' ORDER BY DX").ToList()
			End Using
		End Function
		Public Function ListadoDXCIE(ByVal CIE As String) As List(Of DXAsociado)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Return db.Query(Of DXAsociado)("SELECT CIE, DX, DXASOCIADO FROM TH_DX WHERE CIE='" & CIE & "' ORDER BY DX").ToList()
			End Using
		End Function

		Public Function ListadoBeneficiosPendientes(ByVal idEmpleado As Int64) As List(Of BeneficiosPendientes)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Dim dp As New DynamicParameters()
				dp.Add("@idEmpleado", idEmpleado)
				Return db.Query(Of BeneficiosPendientes)("TH_BeneficiosPendientes", dp, commandType:=CommandType.StoredProcedure).ToList
			End Using
		End Function

		Public Function ObtenerAprobadores() As List(Of Aprobadores)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Return db.Query(Of Aprobadores)("SELECT id, Nombres + ' ' + Apellidos AS NombreCompleto FROM US_USUARIOS WHERE ACTIVO=1 ORDER BY NOMBRES + ' ' + APELLIDOS").ToList()
			End Using
		End Function

		Public Function ObtenerJefeInmediato(ByVal idEmpleado As Int64) As Int64
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Return db.ExecuteScalar(Of Int64)("SELECT JEFEINMEDIATO FROM TH_PERSONAS WHERE ID=" & idEmpleado)
			End Using
		End Function

		Public Function ObtenerDiaRegreso(ByVal Fecha As Date) As Date
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Dim dp As New DynamicParameters()
				dp.Add("@dia", Fecha)
				Return db.ExecuteScalar(Of Date)("TH_Ausencia_DiaRegreso", dp, commandType:=CommandType.StoredProcedure)
			End Using
		End Function

		Public Function GetCausacionPeriodosVAcaciones(idSolicitud As Int64) As List(Of PeriodosCausadosVacaciones)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Dim dp As New DynamicParameters()
				dp.Add("@IdSolicitud", idSolicitud)
				Return db.Query(Of PeriodosCausadosVacaciones)("TH_GetCausacionVacaciones", dp, commandType:=CommandType.StoredProcedure).ToList
			End Using
		End Function

		Public Sub SaveChangesSolicitud(ByRef ent As TH_SolicitudAusencia)
			Dim entO = GetSolicitudAusencia(ent.id)
			entO.Estado = ent.Estado
			entO.FechaAprobacion = ent.FechaAprobacion
			oContext.SaveChanges()
		End Sub

#Region "Reportes"
		Public Function ReporteVacaciones() As List(Of REP_Vacaciones)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Return db.Query(Of REP_Vacaciones)("TH_REP_StatusVacaciones", commandType:=CommandType.StoredProcedure).ToList
			End Using
		End Function

        Public Function ReporteBeneficios(ByVal Anno As Integer) As List(Of REP_Beneficios)
            Dim dp As New DynamicParameters()
            dp.Add("@anno", Anno)
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Return db.Query(Of REP_Beneficios)("TH_REP_Beneficios", dp, commandType:=CommandType.StoredProcedure).ToList
            End Using
        End Function

        Public Function ReporteAusentismo(ByVal Anno As Integer) As List(Of REP_Ausentismo)
            Dim dp As New DynamicParameters()
            dp.Add("@anno", Anno)
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Return db.Query(Of REP_Ausentismo)("TH_REP_Ausentismo", dp, commandType:=CommandType.StoredProcedure).ToList
            End Using
        End Function

        Public Function ReporteIncapacidades(ByVal Anno As Integer) As List(Of REP_Incapacidades)
            Dim dp As New DynamicParameters()
            dp.Add("@anno", Anno)
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Return db.Query(Of REP_Incapacidades)("TH_REP_Incapacidades", dp, commandType:=CommandType.StoredProcedure).ToList
            End Using
        End Function
        Public Function ReporteVacacionesNomina(ByVal Mes As Integer, ByVal Anno As Integer) As List(Of REP_VacacionesNomina)
			Dim dp As New DynamicParameters()
			dp.Add("@Mes", Mes)
			dp.Add("@Anno", Anno)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Return db.Query(Of REP_VacacionesNomina)("TH_REP_VacacionesNomina", dp, commandType:=CommandType.StoredProcedure).ToList
			End Using
		End Function
		Public Function ReporteSolicitudesPendientesAprobacion() As List(Of REP_SolicitudesPendientes)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Return db.Query(Of REP_SolicitudesPendientes)("TH_REP_SolicitudesPendientesAprobacion", commandType:=CommandType.StoredProcedure).ToList
			End Using
		End Function
		Public Function ReporteVacacionesDetallado() As List(Of REP_VacacionesDetallado)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Return db.Query(Of REP_VacacionesDetallado)("TH_REP_VacacionesDetallado", commandType:=CommandType.StoredProcedure).ToList
			End Using
		End Function
#End Region
	End Class

End Namespace
