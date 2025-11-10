Imports System.Configuration
Imports System.Data.SqlClient
Imports Dapper
Imports Ipsos.Apis.SisConn

Namespace ExternalServices.CredentialsManager
    Public Class CredentialsManagerRepository
        Dim sqlConnectionString As String
        Public Sub New()
            Dim connectionManager = New SqlConnectionManager()
            connectionManager.StoreServerName = ConfigurationManager.AppSettings("SisConn_Server")
            connectionManager.Configure(ConfigurationManager.AppSettings("SisConn_Server"))
            Dim dbName = ConfigurationManager.AppSettings("SisConn_CredentialsDBName")
            Dim isDbProd = ConfigurationManager.AppSettings("SisConn_CredentialsDBNameProd")
            sqlConnectionString = connectionManager.GetConnectionString(dbName, isDbProd)
        End Sub
        Public Function CredentialsBy(id As Integer) As UserCredentialsEntity
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {Key .id = id}

                Return db.QueryFirstOrDefault(Of UserCredentialsEntity)("[dbo].[SP_UserCredentialsGet]", commandType:=CommandType.StoredProcedure, param:=params)

            End Using
        End Function
        Public Class UserCredentialsEntity
            Private UserCredentialsId As String
            Property UserName As String
            Property Password As String
            Property DateRegister As DateTime
        End Class
    End Class
End Namespace