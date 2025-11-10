Imports Dapper
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Namespace ClasesDAPFinanzas

    Public Class GeneralDapper
#Region "Variables Globales"
        Dim sqlConnectionString = ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString
#End Region



        Public Sub PUT_EjecutarComando(ByVal sqlcommand As String)
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                db.Execute(sqlcommand)
            End Using
        End Sub

        Public Sub LoadDescuentosSS(ByVal Cedula As Long, Valor As Double, Fecha As Date)
            Dim strCommand As String = ""
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim queryParameters = New DynamicParameters
                queryParameters.Add("@Cedula", Cedula)
                queryParameters.Add("@Valor", Valor)

                'db.Execute("INSERT INTO CC_PRODUCCIONDESCUENTOSSS (CEDULA,VALOR,FECHA) VALUES(@CEDULA,@VALOR,@FECHA)", New Object With { Cedula = Cedula})
            End Using
        End Sub

    End Class


End Namespace
