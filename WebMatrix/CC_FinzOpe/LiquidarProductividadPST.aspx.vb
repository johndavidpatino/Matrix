Imports System.Data.OleDb
Imports ClosedXML.Excel
Imports CoreProject
Imports WebMatrix.Util

Public Class LiquidarProductividadPST
	Inherits System.Web.UI.Page
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Dim permisos As New Datos.ClsPermisosUsuarios
		Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
		If permisos.VerificarPermisoUsuario(132, UsuarioID) = False Then
			Response.Redirect("../Home/Home.aspx")
		End If
		If Not IsPostBack Then
			CargarStatus()
			Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
			If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
			Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)
			lblIni.Text = inicioCorteFecha.ToShortDateString()
			lblFin.Text = finCorteFecha.ToShortDateString()
		End If


	End Sub



	Sub EnviarEmailSolicitud(ByVal TrabajoId As Int64)
		Dim oEnviarCorreo As New EnviarCorreo
		Try
			oEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/SolicitudPresupuesto.aspx?TrabajoId=" & TrabajoId)
		Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
		End Try
	End Sub
	Sub CargarStatus()
		Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
		If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
		Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)
		Dim op As New OP_CuantiDapper
		gvEstatus.DataSource = op.CuantiProdProductividadStatus_Get(inicioCorteFecha, finCorteFecha, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
		gvEstatus.DataBind()

		gvResumenProduccion.DataSource = op.CuantiProduccionResumenCorte(inicioCorteFecha, finCorteFecha)
		gvResumenProduccion.DataBind()
	End Sub
	Protected Sub btnLiquidar_Click(sender As Object, e As EventArgs)
		Dim op As New OP_CuantiDapper
		Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
		If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
		Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)
		op.CuantiProduccionUpdate(inicioCorteFecha, finCorteFecha, Session("IDUsuario").ToString)
		CargarStatus()
	End Sub
	Protected Sub btnPasarProduccion_Click(sender As Object, e As EventArgs)
		Dim op As New OP_CuantiDapper
		Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
		If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
		Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)
		op.CuantiProduccionLiquidarProductividad(inicioCorteFecha, finCorteFecha, Nothing, Nothing, Nothing, Nothing, Nothing, True, True, Session("IDUsuario").ToString)
		ShowNotification("Los registros aprobados se han liquidado", ShowNotifications.InfoNotificationLong)
		CargarStatus()
	End Sub
	Protected Sub btnLoadDataExcel_Click(sender As Object, e As EventArgs)
		If FUploadExcel.HasFile Then
			Dim path As [String] = Server.MapPath("~/Files/")
			Dim fileload As New System.IO.FileInfo(FUploadExcel.FileName)
			Dim fileNameCopy As String = "DescuentosSSV2.xlsx"
			Dim filePath As String = path & fileNameCopy
			Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
			If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
			Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)

			FUploadExcel.SaveAs(filePath)

			Dim connstr As String = "Provider=Microsoft.ACE.OLEDB.12.0;" & "Data Source=" & filePath & ";" & "Extended Properties='Excel 12.0'"
			Dim sqlcmd As String = "SELECT * FROM [FormatoDescuentosSS$B5:F100]"
			Dim dt As DataTable = New DataTable
			'Abre la cadena de conexión que fue enviada como parámetro
			Using conn As OleDbConnection = New OleDbConnection(connstr)
				'Dim tables As DataTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})
				'Ejecuta un dataaapter para abrir la base y ejecutar el comando
				Using da As OleDbDataAdapter = New OleDbDataAdapter(sqlcmd, conn)
					'Llena el objeto dt que es un datatable con la información de la hoja
					da.Fill(dt)
				End Using
			End Using

			Try
				System.IO.File.Delete(filePath)
			Catch ex As Exception
			End Try
			' Verifica q
			If dt.Rows.Count = 0 Then
				ShowNotification("No se encontraron registros", ShowNotifications.ErrorNotification)
				Exit Sub
			Else
				Dim oClassDApp As New ClasesDAPFinanzas.GeneralDapper
				Dim oClassEnt As New ProcesosInternos
				For Each row In dt.Rows
					Try
						'If Not row(0).ToString() = "" Then oCot.PUT_EjecutarComando(row(0).ToString())
						'Dim cedula As Long = Long.Parse(row(0).ToString())
						'Dim valor As Double = Double.Parse(row(1).ToString())
						'Dim fecha As Date = Date.Parse(row(2).ToString())
						If row(0) Is Nothing Then
							Exit For
						End If
						Dim cedula As Long = row(0)
						Dim valorss As Double = row(1)
						Dim saldoss As Double = row(2)
						Dim ica As Double = row(3)
						Dim pagado As Double = row(4)
						Dim fecha As Date = finCorteFecha

						' DAPPER NO FUNCIONAL AÚN
						'oClassDApp.LoadDescuentosSS(cedula, valor, fecha)

						'Entity Funcional
						oClassEnt.InsertDescuentosSS(cedula, valorss, saldoss, ica, pagado, fecha)

					Catch ex As Exception
						Exit For
					End Try
				Next
			End If
			ShowNotification("Se han ejecutado las importaciones", ShowNotifications.InfoNotification)

		Else
			ShowNotification("Debe seleccionar un archivo antes de continuar", ShowNotifications.ErrorNotification)
		End If
		CargarStatus()
	End Sub

	Protected Sub btnDownloadResumen_Click(sender As Object, e As EventArgs)
		Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
		If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
		Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)
		Dim excel As New List(Of Array)
		Dim Titulos As String = "Cedula;Nombre;CargoMatrix;IdIStaff;Ciudad;Cargo;Cantidad;DiasTrabajados;VrTransporte;VrProduccion;VrBono;Subtotal;ValorSS;SaldoSS;ValorICA;TotalDescuento;TotalAPagar"
		Dim DynamicColNames() As String
		Dim lstCambios As List(Of OP_CuantiDapper.DTO.OP_CuantiResumenProduccionCorte)
		Dim workbook = New XLWorkbook()
		Dim worksheet = workbook.Worksheets.Add("1. ExportadoProduccion")

		DynamicColNames = Titulos.Split(CChar(";"))
		excel.Add(DynamicColNames)
		Dim op As New OP_CuantiDapper
		lstCambios = OP.CuantiProduccionResumenCorte(inicioCorteFecha, finCorteFecha)
		For Each x In lstCambios
			excel.Add((x.Cedula & "|" & x.Nombre & "|" & x.CargoMatrix & "|" & x.IdIStaff & "|" & x.Ciudad & "|" & x.Cargo & "|" & x.Cantidad & "|" & x.DiasTrabajados & "|" & x.VrTransporte & "|" & x.VrProduccion & "|" & x.VrBono & "|" & x.Subtotal & "|" & x.ValorSS & "|" & x.SaldoSS & "|" & x.ValorICA & "|" & x.TotalDescuento & "|" & x.TotalAPagar).Split(CChar("|")).ToArray())
		Next
		worksheet.Cell("A1").Value = excel
		Try
			worksheet.Range("A2:A" & lstCambios.Count + 1).DataType = XLCellValues.Number
			worksheet.Range("A2:A" & lstCambios.Count + 1).Style.NumberFormat.Format = "0"
			'worksheet.Range("L2:T" & lstCambios.Count + 1).Style.NumberFormat.NumberFormatId = 2

		Catch ex As Exception
		End Try
		Try
			worksheet.Range("D2:D" & lstCambios.Count + 1).DataType = XLCellValues.Number
			worksheet.Range("D2:D" & lstCambios.Count + 1).Style.NumberFormat.Format = "0"
		Catch ex As Exception
		End Try
		Try
			worksheet.Range("G2:T" & lstCambios.Count + 1).DataType = XLCellValues.Number
			worksheet.Range("G2:T" & lstCambios.Count + 1).Style.NumberFormat.Format = "0"
		Catch ex As Exception
		End Try
		worksheet.Columns("A", "T").AdjustToContents(1, 2)
		'worksheet.Columns("X", "AD").AdjustToContents(1, 2)
		'worksheet.Columns("AE", "AG").AdjustToContents(1, 2)

		excel = New List(Of Array)
		Titulos = "Cedula;Symphony;PresupuestoId;JobBook;ValorBono"
		Dim lstCambios2 As List(Of OP_CuantiDapper.DTO.OP_CuantiResumenProduccionCorteBono)
		worksheet = workbook.Worksheets.Add("2. Distribucion a Job")

		DynamicColNames = Titulos.Split(CChar(";"))
		excel.Add(DynamicColNames)

		lstCambios2 = op.CuantiProduccionResumenCorteBono(inicioCorteFecha, finCorteFecha)
		For Each x In lstCambios2
			excel.Add((x.Cedula & "|" & x.Symphony & "|" & x.PresupuestoId & "|" & x.JobBook & "|" & x.VrBono).Split(CChar("|")).ToArray())
		Next
		worksheet.Cell("A1").Value = excel
		Try
			worksheet.Range("A2:E" & lstCambios2.Count + 1).DataType = XLCellValues.Number
			worksheet.Range("A2:E" & lstCambios2.Count + 1).Style.NumberFormat.Format = "0"
		Catch ex As Exception
		End Try
		worksheet.Columns("A", "E").AdjustToContents(1, 2)

		excel = New List(Of Array)
		Titulos = "Cedula;Symphony;ValorBono"
		Dim lstCambios3 As List(Of OP_CuantiDapper.DTO.OP_CuantiResumenProduccionCorteResumenTercero)
		worksheet = workbook.Worksheets.Add("3. Resumen Tercero")

		DynamicColNames = Titulos.Split(CChar(";"))
		excel.Add(DynamicColNames)

		lstCambios3 = op.CuantiProduccionResumenCorteResumenTercero(inicioCorteFecha, finCorteFecha)
		For Each x In lstCambios3
			excel.Add((x.Cedula & "|" & x.Symphony & "|" & x.VrBono).Split(CChar("|")).ToArray())
		Next
		worksheet.Cell("A1").Value = excel
		Try
			worksheet.Range("A2:C" & lstCambios3.Count + 1).DataType = XLCellValues.Number
			worksheet.Range("A2:C" & lstCambios3.Count + 1).Style.NumberFormat.Format = "0"
		Catch ex As Exception
		End Try
		worksheet.Columns("A", "C").AdjustToContents(1, 2)


		excel = New List(Of Array)
		Titulos = "Cedula;Symphony;JBI;Presupuesto;Cuenta;VrDistribuido;Observacion"
		Dim lstCambios4 As List(Of OP_CuantiDapper.DTO.OP_CuantiProduccionDistribucionCC)
		worksheet = workbook.Worksheets.Add("4. Distribucion SS")

		DynamicColNames = Titulos.Split(CChar(";"))
		excel.Add(DynamicColNames)

		lstCambios4 = op.CuantiProduccionDistribucionSS(inicioCorteFecha, finCorteFecha)
		For Each x In lstCambios4
			excel.Add((x.Cedula & "|" & x.Symphony & "|" & x.JBI & "|" & x.Presupuesto & "|" & x.Cuenta & "|" & x.VrDistribuido & "|" & x.Observacion).Split(CChar("|")).ToArray())
		Next
		worksheet.Cell("A1").Value = excel
		Try
			worksheet.Range("A2:B" & lstCambios4.Count + 1).DataType = XLCellValues.Number
			worksheet.Range("A2:B" & lstCambios4.Count + 1).Style.NumberFormat.Format = "0"
		Catch ex As Exception
		End Try
		Try
			worksheet.Range("F2:F" & lstCambios4.Count + 1).DataType = XLCellValues.Number
			worksheet.Range("F2:F" & lstCambios4.Count + 1).Style.NumberFormat.Format = "0"
		Catch ex As Exception
		End Try
		worksheet.Columns("A", "G").AdjustToContents(1, 5)


		Crearexcel(workbook, "ReporteProduccion_" & inicioCorteFecha.ToString("yyyy-MM-dd") & "_HASTA_" & finCorteFecha.ToString("yyyy-MM-dd"))
	End Sub

	Private Sub Crearexcel(ByVal workbook As XLWorkbook, ByVal name As String)
		Response.Clear()

		Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
		Response.AddHeader("content-disposition", "attachment;filename=""" & name & ".xlsx""")

		Using memoryStream = New IO.MemoryStream()
			workbook.SaveAs(memoryStream)

			memoryStream.WriteTo(Response.OutputStream)
		End Using
		Response.End()
	End Sub
End Class