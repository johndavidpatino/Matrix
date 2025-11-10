Imports CoreProject
Imports System.Data.OleDb

Public Class CargueDescuentosSSForm
	Inherits System.Web.UI.Page





	Protected Sub btnLoadDataExcel_Click(sender As Object, e As EventArgs)
		If FUploadExcel.HasFile Then
			Dim path As [String] = Server.MapPath("~/Files/")
			Dim fileload As New System.IO.FileInfo(FUploadExcel.FileName)
			Dim fileNameCopy As String = "DescuentosSS.xlsx"
			Dim filePath As String = path & fileNameCopy

			FUploadExcel.SaveAs(filePath)

			Dim connstr As String = "Provider=Microsoft.ACE.OLEDB.12.0;" & "Data Source=" & filePath & ";" & "Extended Properties='Excel 12.0'"
			Dim sqlcmd As String = "SELECT * FROM [Hoja1$A1:C100]"
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
				lblMessages.Text = "No se encontraron registros"
				lblMessages.Visible = True
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

						Dim cedula As Long = row(0)
						Dim valor As Double = row(1)
						Dim fecha As Date = row(2)

						' DAPPER NO FUNCIONAL AÚN
						'oClassDApp.LoadDescuentosSS(cedula, valor, fecha)

						'Entity Funcional
						oClassEnt.InsertDescuentosSS(cedula, valor, 0, 0, 0, fecha)

					Catch ex As Exception
					End Try
				Next
			End If
			lblMessageSuccess.Text = "Se han ejecutado las importaciones"
			lblMessageSuccess.Visible = True

		Else
			lblMessages.Visible = True
			lblMessages.Text = "Debe seleccionar un archivo antes de continuar"
		End If

	End Sub
End Class