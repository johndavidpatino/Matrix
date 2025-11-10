Imports System.Configuration
Imports Dapper
Imports System.Data.SqlClient

Namespace CU_JobBook
	Public Class DTO
		Public Class InfoJobBook
			Public Property IdBrief As Int64
			Public Property IdPropuesta As Int64
			Public Property IdEstudio As Int64
			Public Property Cliente As String
			Public Property Titulo As String
			Public Property MarcaCategoria As String
			Public Property Estado As String
			Public Property GerenteCuentas As String
			Public Property GerenteCuentasID As Int64
			Public Property Unidad As String
			Public Property IdUnidad As Int64
			Public Property Viabilidad As Nullable(Of Boolean)
			Public Property NumJobbook As String

		End Class
	End Class


	Public Class DAL
		Dim sqlConnectionString = ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString

		Public Function InfoJobBookGet(Optional ByVal Titulo As String = Nothing, Optional ByVal Jobbook As String = Nothing, Optional ByVal IdPropuesta As Int64? = Nothing, Optional ByVal GerenteCuentas As Int64? = Nothing, Optional ByVal TipoBusqueda As Int16 = 3, Optional ByVal idBrief As Int64? = Nothing, Optional ByVal idEstudio As Int64? = Nothing) As List(Of DTO.InfoJobBook)
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Dim dp As New DynamicParameters()
				dp.Add("@Titulo", Titulo)
				dp.Add("@JobBook", Jobbook)
				dp.Add("@IdPropuesta", IdPropuesta)
				dp.Add("@IdBrief", idBrief)
				dp.Add("@IdEstudio", idEstudio)
				dp.Add("@Gerente", GerenteCuentas)
				dp.Add("@TypeSearch", TipoBusqueda)
				Return db.Query(Of DTO.InfoJobBook)("CU_InfoGeneralJobBook_GET", dp, commandType:=CommandType.StoredProcedure).ToList
			End Using
		End Function

		Public Function NumAlternativas(ByVal idPropuesta As Int64) As Integer
			Dim sqlCommandStr As String = "SELECT COUNT(1) FROM IQ_DATOSGENERALESPRESUPUESTO WHERE idpropuesta=" & idPropuesta.ToString
			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Return db.Query(Of Integer)(sqlCommandStr)(0)
			End Using
		End Function

		Public Sub CloneBrief(ByVal idBrief As Int64, IdUsuario As Int64, IdUnidad As Int32, Titulo As String)

			Using db As IDbConnection = New SqlConnection(sqlConnectionString)
				Dim dp As New DynamicParameters()
				dp.Add("@idBrief", idBrief)
				dp.Add("@idUser", IdUsuario)
				dp.Add("@idUnidad", IdUnidad)
				dp.Add("@Titulo", Titulo)
				db.Execute("CU_Brief_Clone", dp, commandType:=CommandType.StoredProcedure)
			End Using
		End Sub
	End Class
End Namespace