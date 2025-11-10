Imports CoreProject
Imports WebMatrix.Util
Imports ClosedXML.Excel
Imports System.Globalization

Public Class ReporteContabilizacionPST
	Inherits System.Web.UI.Page
	Dim op As New ProcesosInternos

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
		smanager.RegisterPostBackControl(Me.btngenerar)
	End Sub

	Protected Sub btnconsultar_Click(sender As Object, e As EventArgs) Handles btnconsultar.Click
		If txtfechainicio.Text <> "" And txtfechafin.Text <> "" Then
			CargarInformacion(txtfechainicio.Text, txtfechafin.Text)
		End If
	End Sub


	Sub CargarInformacion(ByVal FecIni As Date, ByVal FecFin As Date)
		GvContabilizacion.DataSource = op.ReporteContabilizacion(FecIni, FecFin)
		GvContabilizacion.DataBind()
	End Sub

	Private Sub GvContabilizacion_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvContabilizacion.PageIndexChanging
		GvContabilizacion.PageIndex = e.NewPageIndex
		CargarInformacion(txtfechainicio.Text, txtfechafin.Text)
		ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
	End Sub

	Protected Sub btngenerar_Click(sender As Object, e As EventArgs) Handles btngenerar.Click
		Exportar()
	End Sub

	Sub Exportar()
		Dim excel As New List(Of Array)
		Dim Titulos As String = "PersonaId;TrabajoId;Nombres;Cargo;CantidadEntrevistas;ValorAPagar;DiasTrabajados;JobBook;CuentaContable;CCJOB;CiudadId;Ciudad;PorcentajeTrabajoPersona;TarifaTransporte;Transporte;CCTRANSPORTE;Provision"
		Dim DynamicColNames() As String
		Dim lstInfo As List(Of CC_ReporteContabilizacion_Result)
		Dim workbook = New XLWorkbook()
		Dim worksheet = workbook.Worksheets.Add("Contabilizacion")
		Dim Transporte As Double = 0

		DynamicColNames = Titulos.Split(CChar(";"))
		excel.Add(DynamicColNames)
		Dim o As New ProcesosInternos
		lstInfo = op.ReporteContabilizacion(CDate(txtfechainicio.Text), CDate(txtfechafin.Text)).ToList
		For Each x In lstInfo
			excel.Add((x.PersonaId & ";" & x.TrabajoId & ";" & x.Nombres & ";" & x.Cargo & ";" & x.CantidadEntrevistas.Value.ToString(CultureInfo.InvariantCulture) & ";" & x.ValorAPagar & ";" & x.DiasTrabajados & ";" & x.JobBook & ";" & x.CuentaContable & ";" & x.CCJOB & ";" & x.CiudadId & ";" & x.Ciudad & ";" & x.PorcentajeTrabajoPersona.Value.ToString(CultureInfo.InvariantCulture) & ";" & x.TarifaTransporte & ";" & x.Transporte & ";" & x.CCTRANSPORTE & ";" & x.Provision).Split(CChar(";")).ToArray())
			Transporte = 0
		Next
		worksheet.Cell("A1").Value = excel
		worksheet.Range("A2:B" & lstInfo.Count + 1).DataType = XLCellValues.Text
		worksheet.Range("D2:G" & lstInfo.Count + 1).DataType = XLCellValues.Text
		worksheet.Range("N2:O" & lstInfo.Count + 1).DataType = XLCellValues.Text
		worksheet.Range("K2:K" & lstInfo.Count + 1).DataType = XLCellValues.Text
		worksheet.Range("M2:M" & lstInfo.Count + 1).DataType = XLCellValues.Text
		worksheet.Range("M2:M" & lstInfo.Count + 1).Style.NumberFormat.SetFormat("0.00%")
		worksheet.Range("J2:J" & lstInfo.Count + 1).DataType = XLCellValues.Text

		Crearexcel(workbook, "ReporteContabilizacion-" & txtfechainicio.Text & "-Hasta-" & txtfechafin.Text)
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