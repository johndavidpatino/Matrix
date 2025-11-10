Imports CoreProject.Cotizador.GeneralDapper
Imports Dapper
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Namespace AusenciasEquipo

    Public Class AusenciasEquipoDapper
#Region "Variables Globales"
        Dim sqlConnectionString = ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString
#End Region

        Public Function GetAusenciasEquipo(ByVal jefeId As Int64, fInicio As DateTime, fFin As DateTime) As List(Of AusenciasEquipoResult)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {Key .JefeId = jefeId, .FInicio = fInicio, .FFin = fFin}

                Return db.Query(Of AusenciasEquipoResult)(sql:="TH_AusenciasEquipo_Get", param:=params, commandType:=CommandType.StoredProcedure)

            End Using
        End Function

        Public Function GetBeneficiosPendientes(ByVal idempleado As Int64) As List(Of BeneficiosPendientesResult)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {Key .idempleado = idempleado}

                Return db.Query(Of BeneficiosPendientesResult)(sql:="TH_BeneficiosPendientes", param:=params, commandType:=CommandType.StoredProcedure)

            End Using
        End Function
        Public Function GetAusenciasSubordinados(ByVal jefeId As Int64) As List(Of AusenciasSubordinado)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {Key .JefeId = jefeId}

                Return db.Query(Of AusenciasSubordinado)(sql:="TH_AusenciasSubordinados_Get", param:=params, commandType:=CommandType.StoredProcedure)

            End Using
        End Function

        Public Function GetAusenciasPersonas(ByVal jefeId As Int64, search As String) As List(Of AusenciasSubordinado)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {Key .JefeId = jefeId, .Search = search}

                Return db.Query(Of AusenciasSubordinado)(sql:="TH_AusenciasPersonas_Get", param:=params, commandType:=CommandType.StoredProcedure)

            End Using
        End Function

        Public Function RemoveAusenciasSubordinado(ByVal subordinacionId As Int64)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {Key .SubordinacionId = subordinacionId}

                Return db.Query(sql:="TH_AusenciasSubordinados_Remove", param:=params, commandType:=CommandType.StoredProcedure)

            End Using
        End Function

        Public Function AddAusenciasSubordinado(ByVal jefeId As Int64, empleadoId As Int64) As List(Of AusenciasSubordinado)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {Key .JefeId = jefeId, .EmpleadoId = empleadoId}

                Return db.Query(Of AusenciasSubordinado)(sql:="TH_AusenciasSubordinados_Add", param:=params, commandType:=CommandType.StoredProcedure)

            End Using
        End Function


        Public Class AusenciasSubordinado
            Public Property Id As Int64
            Public Property idEmpleado As Int64
            Public Property Nombre As String
            Public Property Avatar As String
        End Class
        Public Class BeneficiosPendientesResult
            Public Property id As Integer
            Public Property Beneficio As String
            Public Property dias As Integer
        End Class
        Public Class AusenciasEquipoResult
                Public Property SolicitudId As Integer
                Public Property idEmpleado As Int64
                Public Property Nombre As String
                Public Property FInicio As String
                Public Property FFin As String
                Public Property EstadoId As Integer
                Public Property Estado As String
                Public Property JefeInmediatoId As Int64
                Public Property JefeInmediato As String
                Public Property ObservacionesSolicitud As String
                Public Property ObservacionesAprobacion As String
                Public Property TipoId As Integer
                Public Property Tipo As String
                Public Property AreaId As Integer
                Public Property Area As String
            End Class
        End Class

End Namespace
