Imports System.Configuration
Imports System.Data.SqlClient
Imports Dapper

Namespace EmpleadosDapper

    Public Class EmpleadosDapper
#Region "Variables Globales"
        Dim sqlConnectionString = ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString
#End Region

        Public Function EmpleadosActivos() As List(Of EmpleadosActivosoResult)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                'Dim params = New With {Key .JefeId = jefeId, .FInicio = fInicio, .FFin = fFin}
                'Return db.Query(Of EmpleadosActivosoResult)(sql:="TH_AusenciasEquipo_Get", param:=params, commandType:=CommandType.StoredProcedure)

                Return db.Query(Of EmpleadosActivosoResult)("TH_EmpleadosActivos_Get", commandType:=CommandType.StoredProcedure).ToList()

            End Using
        End Function
        Public Class EmpleadosActivosoResult
            Public Property Id As Integer
            Public Property Nombres As String
            Public Property Apellidos As String
            Public ReadOnly Property NombreCompleto() As String
                Get
                    Return Me.Nombres & " " & Me.Apellidos
                End Get
            End Property
        End Class
    End Class

End Namespace
