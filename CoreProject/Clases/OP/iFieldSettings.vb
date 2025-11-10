Imports Dapper
Imports System.Configuration
Imports System.Data.SqlClient

Namespace DALDAP
	Public Class iFieldSettings
#Region "Variables Globales"
		Dim sqlConnectionString = ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString
#End Region
		Public Function ProjectsGetFromMatrix(ByVal TipoBusqueda As DTOiField.TipoBusquedaProyectoiField) As List(Of DTOiField.Proyectos)
			Dim sqlComm As String
			Select Case TipoBusqueda
				Case DTOiField.TipoBusquedaProyectoiField.Activos
					sqlComm = "SELECT * FROM OP_ProyectosIField WHERE Activo=1 AND NOT(TrabajoId IS NULL)"
				Case DTOiField.TipoBusquedaProyectoiField.Pendientes
					sqlComm = "SELECT * FROM OP_ProyectosIField WHERE Activo=1 AND TrabajoId IS NULL"
				Case DTOiField.TipoBusquedaProyectoiField.Anteriores
					sqlComm = "SELECT * FROM OP_ProyectosIField WHERE Activo=0 AND NOT(TrabajoId IS NULL) AND IdProjecto>1005"
			End Select
			Using connection As IDbConnection = New SqlConnection(sqlConnectionString)
				Return connection.Query(Of DTOiField.Proyectos)(sqlComm).ToList()
			End Using
		End Function

		Public Function ProjectGet(ByVal idProjecto As Integer) As DTOiField.Proyectos
			Dim sqlComm As String
			sqlComm = "SELECT * FROM OP_ProyectosIField WHERE IdProjecto=" + idProjecto.ToString
			Using connection As IDbConnection = New SqlConnection(sqlConnectionString)
				Return connection.Query(Of DTOiField.Proyectos)(sqlComm).FirstOrDefault
			End Using
		End Function

		Public Function ProjectConfigGet(ByVal idProjecto As Integer) As List(Of DTOiField.ConfiguracionProyecto)
			Dim sqlComm As String
			sqlComm = "select C.id, C.UserIField UsuarioIfield, C.CCEncuestador, C.CCSupervisor, U.Usuario Usuario, FechaConfig from OP_ConfigIfieldData C inner join US_Usuarios U on U.id=C.IDUsuario inner join OP_ProyectosIField P on P.TrabajoId=C.IdTrabajo WHERE P.IdProjecto=" & idProjecto.ToString
			Using connection As IDbConnection = New SqlConnection(sqlConnectionString)
				Return connection.Query(Of DTOiField.ConfiguracionProyecto)(sqlComm).ToList()
			End Using
		End Function

		Public Function EncuestasPendientes(ByVal idProjecto As Integer) As List(Of DTOiField.EncuestasPendientes)
			Dim sqlComm As String
			sqlComm = "SELECT * FROM OP_DataFromIFieldPass WHERE IdIfield=" + idProjecto.ToString
			Using connection As IDbConnection = New SqlConnection(sqlConnectionString)
				Return connection.Query(Of DTOiField.EncuestasPendientes)(sqlComm).ToList()
			End Using
		End Function

		Public Sub InsertConfigItem(ByVal idProjecto As Integer, UserIField As String, Encuestador As Int64, Supervisor As Int64, Usuario As Integer)
			Dim parameters As DynamicParameters = New DynamicParameters()
			parameters.Add("IdIField", idProjecto)
			parameters.Add("UserIfield", UserIField)
			parameters.Add("Encuestador", Encuestador)
			parameters.Add("Supervisor", Supervisor)
			parameters.Add("Usuario", Usuario)
			Using connection As IDbConnection = New SqlConnection(sqlConnectionString)
				connection.Query("OP_ConfigIfieldData_ADD", parameters, commandType:=CommandType.StoredProcedure)
			End Using
		End Sub

		Public Sub RemoveConfigProject(ByVal IdItem As Long)
			Dim sqlComm As String
			sqlComm = "DELETE FROM OP_ConfigIfieldData WHERE id=" + IdItem.ToString
			Using connection As IDbConnection = New SqlConnection(sqlConnectionString)
				connection.Query(sqlComm)
			End Using
		End Sub

		Public Sub UpdateProject(ByVal idProjecto As Integer, ByVal idTrabajo As Integer)
			Dim sqlComm As String
			sqlComm = "UPDATE OP_ProyectosIField SET TrabajoId = " + idTrabajo.ToString + " WHERE IdProjecto=" + idProjecto.ToString
			Using connection As IDbConnection = New SqlConnection(sqlConnectionString)
				connection.Query(sqlComm)
			End Using
		End Sub


	End Class

	Public Class DTOiField
		Public Class Proyectos
			Public Property IdProjecto As Integer
			Public Property NombreProjecto As String
			Public Property TrabajoId As Integer?
			Public Property Activo As Boolean?
		End Class

		Public Class ConfiguracionProyecto
			Public Property id As Integer
			Public Property UsuarioIfield As String
			Public Property CCEncuestador As Int64?
			Public Property CCSupervisor As Int64?
			Public Property Usuario As String
			Public Property FechaConfig As DateTime?
		End Class

		Public Class EncuestasPendientes
			Public Property IdIfield As Integer
			Public Property NumEncuesta As Int64?
			Public Property Encuestador As String
			Public Property Ciudad As String
			Public Property FechaEncuesta As String
			Public Property FechaSync As DateTime
		End Class

		Public Enum TipoBusquedaProyectoiField
			Activos = 1
			Pendientes = 2
			Anteriores = 3
		End Enum
	End Class
End Namespace
