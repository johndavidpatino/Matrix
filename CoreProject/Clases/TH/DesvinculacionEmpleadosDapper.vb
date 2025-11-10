Imports System.Configuration
Imports System.Data.SqlClient
Imports Dapper

Namespace DesvinculacionEmpleadosDapper

    Public Class DesvinculacionEmpleadosDapper
#Region "Variables Globales"
        Dim sqlConnectionString = ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString

        Public Sub New()

        End Sub
#End Region

        Public Function PendientesPorEvaluarPorArea(AreaId As Integer) As List(Of DesvinculacionEmpleadoPendientePorEvaluarArea)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {.AreaId = AreaId}

                Dim result = db.Query(Of DesvinculacionEmpleadoPendientePorEvaluarArea)(sql:="TH_DesvinculacionesEmpleadosPendientesEvaluarPorArea", commandType:=CommandType.StoredProcedure, param:=params)

                Return result.ToList
            End Using
        End Function

        Public Function ItemsVerificarPor(AreaId As Integer) As List(Of DesvinculacionEmpleadosAreaItemVerificar)
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {.AreaId = AreaId}

                Dim result = db.Query(Of DesvinculacionEmpleadosAreaItemVerificar)(sql:="TH_DesvinculacionesEmpleadosItemsVerificarPorArea", commandType:=CommandType.StoredProcedure, param:=params)

                Return result.ToList
            End Using
        End Function

        Public Function InformacionEmpleadoPor(DesvinculacionEmpleadoId As Integer) As DesvinculacionEmpleadoEmpleadoInfo
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {.DesvinculacionEmpleadoId = DesvinculacionEmpleadoId}

                Dim result = db.Query(Of DesvinculacionEmpleadoEmpleadoInfo)(sql:="TH_DesvinculacionesEmpleadosEmpleadoInfo", commandType:=CommandType.StoredProcedure, param:=params)

                Return result.FirstOrDefault()
            End Using
        End Function

        Public Function GuardarEvaluacion(DesvinculacionEmpleadoEvaluacionArea As DesvinculacionEmpleadoEvaluacionArea) As Integer
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim result = db.ExecuteScalar(Of Integer)(sql:="TH_DesvinculacionEmpleadoAreaEvaluacion_Add", commandType:=CommandType.StoredProcedure, param:=DesvinculacionEmpleadoEvaluacionArea)
                Return result
            End Using
        End Function
        Public Function PendientesPorEvaluarPorEvaluador(EvaluadorId As Long) As List(Of DesvinculacionEmpleadoPendienteEvaluarPorEvaluador)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {.EvaluadorId = EvaluadorId}

                Dim result = db.Query(Of DesvinculacionEmpleadoPendienteEvaluarPorEvaluador)(sql:="TH_DesvinculacionesEmpleadosPendientesEvaluarPorEvaluador", commandType:=CommandType.StoredProcedure, param:=params)

                Return result.ToList
            End Using
        End Function
        Public Function EvaluacionesRealizadasPorEvaluador(EvaluadorId As Long) As List(Of DesvinculacionEmpleadoEvaluacionRealizadaPorEvaluador)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {.EvaluadorId = EvaluadorId}

                Dim result = db.Query(Of DesvinculacionEmpleadoEvaluacionRealizadaPorEvaluador)(sql:="TH_DesvinculacionEmpleadosEvaluacionesRealizadasPorEvaluador", commandType:=CommandType.StoredProcedure, param:=params)

                Return result.ToList
            End Using
        End Function
        Public Function DesvinculacionesResumenGeneral(pageIndex As Integer, pageSize As Integer, textoBuscado As String) As List(Of TH_DesvinculacionEmpleadosEstatus)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {
                    .PageIndex = pageIndex,
                    .PageSize = pageSize,
                    .TextoBuscado = textoBuscado
                    }

                Dim result = db.Query(Of TH_DesvinculacionEmpleadosEstatus)(sql:="TH_DesvinculacionEmpleadosEstatus", commandType:=CommandType.StoredProcedure, param:=params)

                Return result.ToList
            End Using
        End Function

        Public Function DesvinculacionesEstatusEvaluacionesPor(desvinculacionEmpleadoId As Integer) As List(Of TH_DesvinculacionEmpleadosEstatusEvaluacionPorDesvinculacion)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {
                    .DesvinculacionEmpleadoId = desvinculacionEmpleadoId
                    }

                Dim result = db.Query(Of TH_DesvinculacionEmpleadosEstatusEvaluacionPorDesvinculacion)(sql:="TH_DesvinculacionEmpleadosEstatusEvaluacionesPorDesvinculacion", commandType:=CommandType.StoredProcedure, param:=params)

                Return result.ToList
            End Using
        End Function
        Public Function DesvinculacionAdd(empleadoId As Integer, fechaRetiro As DateTime, motivosDesvinculacion As String, fechaRegistro As DateTime, usuarioRegistroId As Long) As Long
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim parameters As New DynamicParameters()
                parameters.Add("@EmpleadoId", empleadoId)
                parameters.Add("@FechaRegistro", fechaRegistro)
                parameters.Add("@FechaRetiro", fechaRetiro)
                parameters.Add("@MotivosDesvinculacion", motivosDesvinculacion)
                parameters.Add("@UsuarioRegistroId", usuarioRegistroId)

                Return db.ExecuteScalar(Of Long)("TH_DesvinculacionEmpleadosAdd", parameters, commandType:=CommandType.StoredProcedure)
            End Using
        End Function
        Public Function FinalizarProceso(desvinculacionEmpleadoId As Long) As String
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim parameters As New DynamicParameters()
                parameters.Add("@desvinculacionEmpleadoId", desvinculacionEmpleadoId)

                Return db.ExecuteScalar(Of String)("TH_DesvinculacionEmpleadoFinalizarProceso", parameters, commandType:=CommandType.StoredProcedure)
            End Using
        End Function

    End Class

    Public Class DesvinculacionEmpleadoPendientePorEvaluarArea
        Property DesvinculacionEmpleadoId As Integer
        Property EmpleadoId As Long
        Property Nombres As String
        Property Apellidos As String
        Property FechaRegistro As DateTime
        Public ReadOnly Property NombreEmpleadoCompleto() As String
            Get
                Return Me.Nombres & " " & Me.Apellidos
            End Get
        End Property

    End Class

    Public Class DesvinculacionEmpleadosAreaItemVerificar
        Property Id As Integer
        Property AreaId As Integer
        Property Descripcion As String
        Property Activo As Boolean
    End Class
    Public Class DesvinculacionEmpleadoEmpleadoInfo
        Property EmpleadoId As Long
        Property Nombres As String
        Property Apellidos As String
        Property CargoId As Integer
        Property Cargo As String
        Property FechaRetiro As Date
        Public ReadOnly Property NombreEmpleadoCompleto() As String
            Get
                Return Me.Nombres & " " & Me.Apellidos
            End Get
        End Property
    End Class
    Public Class DesvinculacionEmpleadoEvaluacionArea
        Property DesvinculacionEmpleadoId As Integer
        Property AreaId As Integer
        Property FechaDiligenciamiento As DateTime
        Property Comentarios As String
        Property UsuarioRegistra As Long
    End Class
    Public Class DesvinculacionEmpleadoPendienteEvaluarPorEvaluador
        Property DesvinculacionEmpleadoId As Integer
        Property EmpleadoId As Integer
        Property FechaRegistro As DateTime
        Property AreaId As Integer
        Property NombreArea As String
        Property Nombres As String
        Property Apellidos As String
        Public ReadOnly Property NombreEmpleadoCompleto() As String
            Get
                Return Me.Nombres & " " & Me.Apellidos
            End Get
        End Property
    End Class
    Public Class DesvinculacionEmpleadoEvaluacionRealizadaPorEvaluador
        Property DesvinculacionEmpleadoId As Long
        Property EmpleadoId As Long
        Property FechaDiligenciamiento As DateTime
        Property Nombres As String
        Property Apellidos As String
        Property NombreArea As String
        Property Comentarios As String
        Public ReadOnly Property NombreEmpleadoCompleto() As String
            Get
                Return Me.Nombres & " " & Me.Apellidos
            End Get
        End Property
    End Class
    Public Class TH_DesvinculacionEmpleadosEstatus
        Property DesvinculacionEmpleadoId As Long
        Property EmpleadoId As Long
        Property Nombres As String
        Property Apellidos As String
        Property urlFoto As String
        Property PorcentajeAvanceDesvinculacion As Integer
        Property CantidadTotalFilas As Integer
        Public ReadOnly Property NombreEmpleadoCompleto() As String
            Get
                Return Me.Nombres & " " & Me.Apellidos
            End Get
        End Property
    End Class
    Public Class TH_DesvinculacionEmpleadosEstatusEvaluacionPorDesvinculacion
        Property DesvinculacionEmpleadoId As Long
        Property NombreEvaluador As String
        Property ApellidosEvaluador As String
        Property AreaId As Integer
        Property NombreArea As String
        Property NombresEvaluadores As String
        Property FechaDiligenciamiento As DateTime?
        Property Comentarios As String
        Property Estado As String
        Property EmailsEvaluadores As String
        Public ReadOnly Property NombreEvaluadorCompleto() As String
            Get
                Return Me.NombreEvaluador & " " & Me.ApellidosEvaluador
            End Get
        End Property
    End Class
End Namespace
