Imports Dapper
Imports System.Data
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Public Class DAL
	Shared SqlConnectionString As String = "Data Source=AmCoBogApp1;Initial Catalog=Matrix;User ID=MatrixUsr;Password=matrix123" 'Configuration.ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString

	Public Function TiposSolicitudesAusencia() As List(Of String)
		Using db As IDbConnection = New SqlConnection(SqlConnectionString)
			Return db.Query(Of String)("SELECT * FROM TH_Ausencia_Tipo").ToList()
		End Using
	End Function

	Public Class CotizadorGeneral

		Public Function GetAllPresupuestosByAlternativa(ByVal idPropuesta As Int64, idAlternativa As Integer) As List(Of DTO.IQ_Parametros)
			Using db As IDbConnection = New SqlConnection(SqlConnectionString)
				Return db.Query(Of DTO.IQ_Parametros)("SELECT * FROM IQ_PARAMETROS WHERE IDPROPUESTA=" & idPropuesta.ToString & " AND PARALTERNATIVA=" & idAlternativa.ToString).ToList
			End Using
		End Function
		Public Function GetGeneralByAlternativa(ByVal idPropuesta As Int64, ByVal idAlternativa As Integer) As DTO.IQ_DatosGeneralesPresupuesto
			Using db As IDbConnection = New SqlConnection(SqlConnectionString)
				Return db.Query(Of DTO.IQ_DatosGeneralesPresupuesto)("SELECT TOP 1 * FROM IQ_DatosGeneralesPresupuesto WHERE IDPROPUESTA=" & idPropuesta.ToString & " AND PARALTERNATIVA=" & idAlternativa.ToString).FirstOrDefault
			End Using
		End Function

		Public Function GetProcesos(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of DTO.IQ_ProcesosPresupuesto)
			Using db As IDbConnection = New SqlConnection(SqlConnectionString)
				Return db.Query(Of DTO.IQ_ProcesosPresupuesto)("SELECT * FROM IQ_ProcesosPresupuesto WHERE IDPROPUESTA=" & idPropuesta.ToString & " AND PARALTERNATIVA=" & Alternativa.ToString & " AND METCODIGO=" & Metodologia.ToString & " AND PARNACIONAL=" & Fase.ToString).ToList
			End Using
		End Function

		Public Sub UpdateNumIQuote(ByVal idPropuesta As Int64, ByVal idAlternativa As Integer, NumiQuote As String)
			Using db As IDbConnection = New SqlConnection(SqlConnectionString)
				db.Query("UPDATE IQ_DatosGeneralesPresupuesto SET NoIQuote='" & NumiQuote & "' WHERE IDPROPUESTA=" & idPropuesta.ToString & " AND PARALTERNATIVA=" & idAlternativa.ToString)
			End Using
		End Sub

		Public Function GetTotalMuestra(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As Integer
			Using db As IDbConnection = New SqlConnection(SqlConnectionString)
				Return db.ExecuteScalar(Of Integer)("SELECT SUM(ISNULL(MUCANTIDAD,0)) FROM IQ_Muestra WHERE IDPROPUESTA=" & idPropuesta.ToString & " AND PARALTERNATIVA=" & Alternativa.ToString & " AND METCODIGO=" & Metodologia.ToString & " AND PARNACIONAL=" & Fase.ToString)
			End Using
		End Function

		Public Function GetPreguntas(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As DTO.IQ_Preguntas
			Using db As IDbConnection = New SqlConnection(SqlConnectionString)
				Return db.Query(Of DTO.IQ_Preguntas)("SELECT * FROM IQ_PREGUNTAS WHERE IDPROPUESTA=" & idPropuesta.ToString & " AND PARALTERNATIVA=" & Alternativa.ToString & " AND METCODIGO=" & Metodologia.ToString & " AND PARNACIONAL=" & Fase.ToString).FirstOrDefault
			End Using
		End Function

		Public Function GetHorasProfesionalesByAlternativa(ByVal idPropuesta As Int64, Alternativa As Integer) As List(Of DTO.IQ_ObtenerHorasProfesionalesXAlternativa_Result)
			Using db As IDbConnection = New SqlConnection(SqlConnectionString)
				Dim dp As New DynamicParameters()
				dp.Add("@idPropuesta", idPropuesta)
				dp.Add("@ParAlternativa", Alternativa)
				Return db.Query(Of DTO.IQ_ObtenerHorasProfesionalesXAlternativa_Result)("IQ_ObtenerHorasProfesionalesXAlternativa", dp, commandType:=CommandType.StoredProcedure)
			End Using
		End Function

		Public Function GetCostoActividadesIQuote(ByVal idPropuesta As Int64, Alternativa As Integer, TipoCosto As String) As List(Of DTO.IQ_COSTOACTIVIDADES_GET_TO_IQUOTE_Result)
			Using db As IDbConnection = New SqlConnection(SqlConnectionString)
				Dim dp As New DynamicParameters()
				dp.Add("@idPropuesta", idPropuesta)
				dp.Add("@Alternativa", Alternativa)
				dp.Add("@TypeIQuote", TipoCosto)
				Return db.Query(Of DTO.IQ_COSTOACTIVIDADES_GET_TO_IQUOTE_Result)("IQ_COSTOACTIVIDADES_GET_TO_IQUOTE", dp, commandType:=CommandType.StoredProcedure)
			End Using
		End Function

		Public Sub UpdateMarkedAlternativa(ByVal idPropuesta As Int64, ByVal idAlternativa As Integer, NumiQuote As String)
			Using db As IDbConnection = New SqlConnection(SqlConnectionString)
				db.Query("UPDATE IQ_AlternativasToExportIQuote SET NoIQuote='" & NumiQuote & "', FECHAEXPORTACION=GETDATE() WHERE IDPROPUESTA=" & idPropuesta.ToString & " AND ALTERNATIVA=" & idAlternativa.ToString & " AND FECHAEXPORTACION IS NULL")
			End Using
		End Sub

		Public Function GetAlternativasToExport(ByVal usuario As String, ByVal password As String) As List(Of DTO.IQ_AlternativasMarkedToIquote_Result)
			Using db As IDbConnection = New SqlConnection(SqlConnectionString)
				Dim sqlString As String = "SELECT IQ.ID, IQ.idPropuesta Propuesta, IQ.Alternativa, P.JobBook, P.Titulo, B.Cliente, IQ.Fecha FROM [Matrix].[dbo].[IQ_AlternativasToExportIQuote] IQ inner join CU_Propuestas P on P.Id=IQ.idPropuesta inner join CU_Brief B ON B.Id=P.Brief inner join US_Usuarios U on u.id=B.GerenteCuentas WHERE IQ.FechaExportacion IS NULL AND U.Usuario='" & usuario & "' AND (U.PASSWORD='" & Cifrado(1, 2, password.ToString, "Ipsos*23432_2013", "Ipsos*23432_2013") & "' OR '" & password.ToString & "'='matrix#$%&'" & ")"
				Return db.Query(Of DTO.IQ_AlternativasMarkedToIquote_Result)(sqlString)
			End Using
		End Function

		Public Function GetTotalVenta(ByVal idPropuesta As Int64, Alternativa As Integer) As Int64
			Using db As IDbConnection = New SqlConnection(SqlConnectionString)
				Return db.ExecuteScalar(Of Int64)("SELECT SUM(ISNULL(ParValorVenta,0)) FROM IQ_PARAMETROS WHERE IDPROPUESTA=" & idPropuesta.ToString & " AND PARALTERNATIVA=" & Alternativa.ToString)
			End Using
		End Function

		Public Function GetCalculosOPSToIQUOTE(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of DTO.IQ_CostosOPS_ToIQuote_Result)
			Using db As IDbConnection = New SqlConnection(SqlConnectionString)
				Dim dp As New DynamicParameters()
				dp.Add("@idPropuesta", idPropuesta)
				dp.Add("@ParAlternativa", Alternativa)
				dp.Add("@MetCodigo", Metodologia)
				dp.Add("@ParNacional", Fase)
				Return db.Query(Of DTO.IQ_CostosOPS_ToIQuote_Result)("IQ_CalculosOPS_ToIQuote", dp, commandType:=CommandType.StoredProcedure)
			End Using
		End Function

		Public Function GetFase(ByVal Fase As Integer) As String
			Using db As IDbConnection = New SqlConnection(SqlConnectionString)
				Return db.QuerySingle(Of String)("SELECT DescFase FROM IQ_Fases WHERE IdFase=" & Fase)
			End Using
		End Function

		Public Function GetMetodologia(ByVal Metodologia As Integer) As String
			Using db As IDbConnection = New SqlConnection(SqlConnectionString)
				Return db.QuerySingle(Of String)("select MetNombre from OP_Metodologias WHERE MetCodigo=" & Metodologia)
			End Using
		End Function
	End Class


	Public Shared Function Cifrado(ByVal modo As Byte, ByVal Algoritmo As Byte, ByVal cadena As String, ByVal key As String, ByVal VecI As String) As String
		Dim plaintext() As Byte = Nothing
		If modo = 1 Then
			plaintext = Encoding.ASCII.GetBytes(cadena)
		ElseIf modo = 2 Then
			plaintext = Convert.FromBase64String(cadena)
		End If

		Dim keys() As Byte = Encoding.ASCII.GetBytes(key)
		Dim memdata As New MemoryStream
		Dim transforma As ICryptoTransform = Nothing


		Select Case Algoritmo
			Case 1
				Dim des As New DESCryptoServiceProvider ' DES
				des.Mode = CipherMode.CBC
				If modo = 1 Then
					transforma = des.CreateEncryptor(keys, Encoding.ASCII.GetBytes(VecI))
				ElseIf modo = 2 Then
					transforma = des.CreateDecryptor(keys, Encoding.ASCII.GetBytes(VecI))
				End If

			Case 2
				Dim des3 As New TripleDESCryptoServiceProvider 'TripleDES
				des3.Mode = CipherMode.CBC
				If modo = 1 Then
					transforma = des3.CreateEncryptor(keys, Encoding.ASCII.GetBytes(VecI))
				ElseIf modo = 2 Then
					transforma = des3.CreateDecryptor(keys, Encoding.ASCII.GetBytes(VecI))
				End If

			Case 3
				Dim rc2 As New RC2CryptoServiceProvider 'RC2
				rc2.Mode = CipherMode.CBC
				If modo = 1 Then
					transforma = rc2.CreateEncryptor(keys, Encoding.ASCII.GetBytes(VecI))
				ElseIf modo = 2 Then
					transforma = rc2.CreateDecryptor(keys, Encoding.ASCII.GetBytes(VecI))
				End If

			Case 4
				Dim rj As New RijndaelManaged 'Rijndael
				rj.Mode = CipherMode.CBC
				If modo = 1 Then
					transforma = rj.CreateEncryptor(keys, Encoding.ASCII.GetBytes(VecI))
				ElseIf modo = 2 Then
					transforma = rj.CreateDecryptor(keys, Encoding.ASCII.GetBytes(VecI))
				End If
		End Select

		Dim encstream As New CryptoStream(memdata, transforma, CryptoStreamMode.Write)

		encstream.Write(plaintext, 0, plaintext.Length)
		encstream.FlushFinalBlock()
		encstream.Close()

		If modo = 1 Then
			cadena = Convert.ToBase64String(memdata.ToArray)
		ElseIf modo = 2 Then
			cadena = Encoding.ASCII.GetString(memdata.ToArray)
		End If
		Return cadena 'Aquí Devuelve los Datos Cifrados

	End Function

End Class
