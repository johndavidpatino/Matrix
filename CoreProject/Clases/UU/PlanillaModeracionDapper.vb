Imports System.Configuration
Imports System.Data.SqlClient
Imports Dapper
Imports Ipsos.Apis.SisConn

Namespace PlanillaModeracionDapper

    Public Class PlanillaModeracionDapper
#Region "Variables Globales"
        Dim sqlConnectionString = ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString

#End Region

        Public Function GetTecnicas(TipoTecnica As String) As List(Of TecnicaModel)
            Dim params = New With {
                    .TipoTecnica = TipoTecnica
                    }
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim result = db.Query(Of TecnicaModel)(sql:="UU_Tecnicas_Get", commandType:=CommandType.StoredProcedure, param:=params)
                Return result.ToList
            End Using
        End Function

        Public Function GetModeradores() As List(Of ModeradorModel)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim result = db.Query(Of ModeradorModel)(sql:="UU_Moderador_Get", commandType:=CommandType.StoredProcedure)
                Return result.ToList
            End Using
        End Function

        Public Function SavePlanillaModeracion(planillaModerador As PlanillaModeracionModel) As String
            Dim params = New With {
                    .IdJob = planillaModerador.IdJob,
                    .JobDesc = planillaModerador.jobDesc,
                    .Fecha = planillaModerador.fecha,
                    .Hora = planillaModerador.hora,
                    .IdTecnica = planillaModerador.tecnica,
                    .Tiempo = planillaModerador.tiempo,
                    .IdModerador = planillaModerador.moderador,
                    .Rol = planillaModerador.rol,
                    .IdUsuarioRegistro = planillaModerador.idUsuarioRegistro.ToString,
                    .Observaciones = planillaModerador.Observaciones,
                    .IdCuentasUU = planillaModerador.IdCuentasUU,
                    .ServiceLineName = planillaModerador.BI_WBSL
                    }

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim result = db.Query(Of String)(sql:="UU_PlanillaModeracion_Save", commandType:=CommandType.StoredProcedure, param:=params)
                Return result.ToString
            End Using
        End Function

        Public Function UpdatePlanillaModeracion(idPlanilla As Integer, idEstado As Short, observaciones As String, dineroBi As String, statusBi As String, idUsuarioAprueba As Long, fechaAprobacion As DateTime, JobEncontradoEnBI As Boolean) As String
            Dim params = New With {
                    .IdPlanilla = idPlanilla,
                    .IdEstado = idEstado,
                    .Observaciones = observaciones,
                    .dineroBI = dineroBi,
                    .statusBI = statusBi,
                    .IdUsuarioAprueba = idUsuarioAprueba,
                    .FechaAprobacion = fechaAprobacion,
                    .JobEncontradoEnBI = JobEncontradoEnBI
                    }

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim result = db.Query(Of String)(sql:="UU_PlanillaModeracion_UpdateStatus", commandType:=CommandType.StoredProcedure, param:=params)
                Return result.ToString
            End Using
        End Function

        Public Function SavePlanillaInformes(planillaInformes As PlanillaInformesModel) As String
            Dim params = New With {
                    .IdJob = planillaInformes.IdJob,
                    .JobDesc = planillaInformes.jobDesc,
                    .Fecha = planillaInformes.fecha,
                    .Muestra = planillaInformes.muestra,
                    .IdTecnica = planillaInformes.tecnica,
                    .IdAnalista = planillaInformes.analista,
                    .IdUsuarioRegistro = planillaInformes.idUsuarioRegistro,
                    .Observaciones = planillaInformes.Observaciones,
                    .IdCuentasUU = planillaInformes.IdCuentasUU,
                    .ServiceLineName = planillaInformes.ServiceLineName
                    }

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim result = db.Query(Of String)(sql:="UU_PlanillaInformes_Save", commandType:=CommandType.StoredProcedure, param:=params)
                Return result.ToString
            End Using
        End Function

        Public Function UpdatePlanillaInformes(idPlanilla As Integer, IdEstado As Short, observaciones As String, dineroBi As String, statusBi As String, idUsuarioAprueba As Long, fechaAprobacion As DateTime, JobEncontradoEnBI As Boolean) As String
            Dim params = New With {
                    .IdPlanilla = idPlanilla,
                    .IdEstado = IdEstado,
                    .Observaciones = observaciones,
                    .dineroBI = dineroBi,
                    .statusBI = statusBi,
                    .IdUsuarioAprueba = idUsuarioAprueba,
                    .FechaAprobacion = fechaAprobacion,
                    .JobEncontradoEnBI = JobEncontradoEnBI
                    }

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim result = db.Query(Of String)(sql:="UU_PlanillaInformes_UpdateStatus", commandType:=CommandType.StoredProcedure, param:=params)
                Return result.ToString
            End Using
        End Function

        Public Function GetPlanillasModeracionBy(idPlanilla As Integer) As PlanillaModeracionListModel
            Dim params = New With {
                    .idPlanilla = idPlanilla
                    }

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Return db.QueryFirstOrDefault(Of PlanillaModeracionListModel)(sql:="UU_PlanillaModeracion_Get_ById", commandType:=CommandType.StoredProcedure, param:=params)
            End Using
        End Function

        Public Function GetPlanillasInformesBy(idPlanilla As Integer) As PlanillaInformesListModel
            Dim params = New With {
                    .idPlanilla = idPlanilla
                    }

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Return db.QueryFirstOrDefault(Of PlanillaInformesListModel)(sql:="UU_PlanillaInformes_Get_ById", commandType:=CommandType.StoredProcedure, param:=params)
            End Using
        End Function

        Public Function GetPlanillas(pageSize As Integer, pageIndex As Integer, filtroPlanilla As String, idEstado As Short?) As List(Of PlanillaRegistrosListModel)
            Dim params = New With {
                    .pageSize = pageSize,
                    .pageIndex = pageIndex,
                    .filtroTipo = filtroPlanilla,
                    .idEstado = idEstado
                    }

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim result = db.Query(Of PlanillaRegistrosListModel)(sql:="UU_PlanillasRegistros_Get", commandType:=CommandType.StoredProcedure, param:=params)
                Return result.ToList
            End Using
        End Function

        Public Function GetPlanillasModeracionToExport(fechaInicio As Date, fechaFinal As Date) As List(Of PlanillasModeracionExportModel)
            Dim params = New With {
                    .fechaInicio = fechaInicio,
                    .fechaFinal = fechaFinal
                    }

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim result = db.Query(Of PlanillasModeracionExportModel)(sql:="UU_PlanillasModeracionToExport", commandType:=CommandType.StoredProcedure, param:=params)
                Return result.ToList
            End Using
        End Function

        Public Function GetPlanillasInformesToExport(fechaInicio As Date, fechaFinal As Date) As List(Of PlanillasInformesExportModel)
            Dim params = New With {
                    .fechaInicio = fechaInicio,
                    .fechaFinal = fechaFinal
                    }

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim result = db.Query(Of PlanillasInformesExportModel)(sql:="UU_PlanillasInformesToExport", commandType:=CommandType.StoredProcedure, param:=params)
                Return result.ToList
            End Using
        End Function
    End Class

    Public Class BIDataModel
        Property jobId As String
        Property jobName As String
        Property jobSL As String
        Property jobStatus As String
        Property jobDinero As String
    End Class

    Public Class TecnicaModel
        Property Id As Integer
        Property TecnicaNombre As String
        Property Puntos As Decimal

    End Class

    Public Class ModeradorModel
        Property Id As Integer
        Property NombreModerador As String

    End Class

    Public Class PlanillaModeracionModel
        Property IdJob As String
        Property jobDesc As String
        Property fecha As Date
        Property hora As String
        Property tecnica As Integer
        Property tiempo As String
        Property moderador As Integer
        Property rol As String
        Property idUsuarioRegistro As Integer
        Property Observaciones As String
        Property IdCuentasUU As Integer
        Property BI_WBSL As String = ""
        Property BI_3320_Moderacion_DineroDisponible As String = ""
        Property BI_Status As String = ""

    End Class

    Public Class PlanillaModeracionListModel
        Property Id As Integer
        Property IdJob As String
        Property jobDesc As String
        Property fecha As String
        Property hora As String
        Property tecnica As String
        Property puntos As String
        Property tiempo As String
        Property moderador As String
        Property rol As String
        Property IdEstado As Short
        Property EstadoPlanilla As String
        Property Observaciones As String
        Property ObservacionesAprobacion As String
        Property BI_WBSL As String = ""
        Property BI_3320_Moderacion_DineroDisponible As String = ""
        Property BI_Status As String
        Property cuentasUU As String = ""
        Property BI_DefaultMessage As String = ""

    End Class

    Public Class PlanillaInformesModel
        Property IdJob As String
        Property jobDesc As String
        Property fecha As Date
        Property tecnica As Integer
        Property muestra As String
        Property analista As Integer
        Property idUsuarioRegistro As Integer
        Property Observaciones As String
        Property IdCuentasUU As Integer
        Property ServiceLineName As String

    End Class

    Public Class PlanillaInformesListModel
        Property Id As Integer
        Property IdJob As String
        Property jobDesc As String
        Property fecha As String
        Property tecnica As String
        Property muestra As String
        Property analista As String
        Property idUsuarioRegistro As Integer
        Property UsuarioRegistro As String
        Property IdEstado As Short
        Property EstadoPlanilla As String
        Property Observaciones As String
        Property IdCuentasUU As Integer
        Property cuentasUU As String
        Property ObservacionesAprobacion As String
        Property BI_WBSL As String = ""
        Property BI_3320_Moderacion_DineroDisponible As String = ""
        Property BI_Status As String
        Property BI_DefaultMessage As String = ""

    End Class

    Public Class PlanillaRegistrosListModel
        Property IdRegistro As Integer
        Property TipoRegistro As String
        Property FechaRegistro As String
        Property UsuarioRegistro As String
        Property IdJob As String
        Property JobDesc As String
        Property FechaActividad As String
        Property Tecnica As String
        Property EstadoPlanilla As String
        Property TotalRows As Integer

    End Class

    Public Class PlanillasInformesExportModel
        Property FechaRegistro As String
        Property UsuarioRegistro As String
        Property IdJob As String
        Property jobDesc As String
        Property BI_WBSL As String
        Property BI_Status As String
        Property BI_3320_Moderacion_DineroDisponible As String
        Property CuentasUU As String
        Property tecnica As String
        Property Puntos As String
        Property muestra As String
        Property analista As String
        Property fecha As String
        Property Observaciones As String
        Property ObservacionesAprobacion As String
        Property EstadoPlanilla As String
    End Class

    Public Class PlanillasModeracionExportModel
        Property fechaRegistro As String
        Property UsuarioRegistro As String
        Property IdJob As String
        Property jobDesc As String
        Property BI_WBSL As String
        Property BI_Status As String
        Property BI_3320_Moderacion_DineroDisponible As String
        Property cuentasUU As String
        Property tecnica As String
        Property puntos As String
        Property Tiempo As String
        Property moderador As String
        Property rol As String
        Property fecha As String
        Property hora As String
        Property Observaciones As String
        Property ObservacionesAprobacion As String
        Property EstadoPlanilla As String
    End Class


End Namespace
