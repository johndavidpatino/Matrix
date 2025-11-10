Imports CoreProject
Imports WebMatrix.Util
Imports System.IO
Imports System.Resources
Imports System.Globalization
Imports System.Threading
Imports System.Data.OleDb
Imports System.Data
Imports ClosedXML.Excel

Public Class TH_GestionAusenciaRRHH
	Inherits System.Web.UI.Page

	Sub AlertJS(ByVal mensaje As String)
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
	End Sub

	Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
		Page.Form.Attributes.Add("enctype", "multipart/form-data")
	End Sub

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			CargarTiposSolicitudesAusencia()
			llenarDdlAnos()
			ddlAnno.SelectedValue = Now.Date.Year
		End If
		Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
		smanager.RegisterPostBackControl(Me.btnReporteVacaciones)
		smanager.RegisterPostBackControl(Me.btnReporteBeneficios)
		smanager.RegisterPostBackControl(Me.btnReporteAusentismo)
		smanager.RegisterPostBackControl(Me.btnReporteIncapacidades)
		smanager.RegisterPostBackControl(Me.btnReporteNomina)
        smanager.RegisterPostBackControl(Me.btnReporteVacacionesDetallado)
        smanager.RegisterPostBackControl(Me.btnSolicitudesPendientes)
    End Sub

	Sub CargarTiposSolicitudesAusencia()
		Dim o As New TH_Ausencia.DAL
		ddlTipoSolicitud.DataSource = o.TiposSolicitudesAusencia
		ddlTipoSolicitud.DataTextField = "Tipo"
		ddlTipoSolicitud.DataValueField = "id"
		ddlTipoSolicitud.DataBind()
		ddlTipoSolicitud.Items.Insert(0, New ListItem With {.Value = "0", .Text = "-- Seleccione --"})
	End Sub


	Private Sub liMenu1_Click(sender As Object, e As EventArgs) Handles lbMenu1.Click
		liMenu2.Attributes.Remove("class")
		liMenu1.Attributes.Add("class", "active")
		liMenu3.Attributes.Remove("class")
		liMenu4.Attributes.Remove("class")

		pnlAprobaciones.Visible = True
		PnlVacaciones.Visible = False
		pnlBeneficios.Visible = False
		pnlAusentismo.Visible = False

	End Sub

	Private Sub liMenu2_Click(sender As Object, e As EventArgs) Handles lbMenu2.Click
		liMenu1.Attributes.Remove("class")
		liMenu2.Attributes.Add("class", "active")
		liMenu3.Attributes.Remove("class")
		liMenu4.Attributes.Remove("class")

		pnlAprobaciones.Visible = False
		PnlVacaciones.Visible = True
		pnlBeneficios.Visible = False
		pnlAusentismo.Visible = False
	End Sub

	Private Sub liMenu3_Click(sender As Object, e As EventArgs) Handles lbMenu3.Click
		liMenu1.Attributes.Remove("class")
		liMenu3.Attributes.Add("class", "active")
		liMenu2.Attributes.Remove("class")
		liMenu4.Attributes.Remove("class")

		pnlAprobaciones.Visible = False
		PnlVacaciones.Visible = False
		pnlBeneficios.Visible = True
		pnlAusentismo.Visible = False
	End Sub

	Private Sub liMenu4_Click(sender As Object, e As EventArgs) Handles lbMenu4.Click
		liMenu1.Attributes.Remove("class")
		liMenu4.Attributes.Add("class", "active")
		liMenu2.Attributes.Remove("class")
		liMenu3.Attributes.Remove("class")

		pnlAprobaciones.Visible = False
		PnlVacaciones.Visible = False
		pnlBeneficios.Visible = False
		pnlAusentismo.Visible = True
	End Sub



	Sub CargarAprobacionesPendientes()
		Dim o As New TH_Ausencia.DAL
		gvAprobacionesPendientes.DataSource = o.RegistrosAusencia(Nothing, Nothing, Nothing, Nothing, ddlTipoSolicitud.SelectedValue, 5, Nothing, Nothing, Nothing, Nothing, Nothing)
		gvAprobacionesPendientes.DataBind()
	End Sub

	Private Sub ddlTipoSolicitud_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoSolicitud.SelectedIndexChanged
		CargarAprobacionesPendientes()
	End Sub

	Sub EnvioCorreo(ByVal idSolicitud As Int64)
		Dim oEnviarCorreo As New EnviarCorreo
		oEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/EnvioDefinicionAusencia.aspx?&idSolicitud=" & idSolicitud.ToString)
	End Sub

	Sub EnvioCorreoAprobacionVacaciones(ByVal idSolicitud As Int64)
		Dim oEnviarCorreo As New EnviarCorreo
		oEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/EnvioAprobacionVacaciones.aspx?&idSolicitud=" & idSolicitud.ToString)
	End Sub

	Private Sub gvAprobacionesPendientes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvAprobacionesPendientes.RowCommand
		Dim ids As Int64 = Int64.Parse(Me.gvAprobacionesPendientes.DataKeys(CInt(e.CommandArgument))("ID"))
		Dim o As New TH_Ausencia.DAL
		Dim ent = o.GetSolicitudAusencia(ids)
		If e.CommandName = "Aprobar" Then
			ent.FechaAprobacion = Date.UtcNow.AddHours(-5)
			ent.Estado = 20
			ent.VoBo1 = Session("IDUsuario").ToString
			ent.FechaVoBo1 = Date.UtcNow.AddHours(-5)
			If ent.Tipo = 1 Then
				o.CausarVacaciones(ent.id)
				EnvioCorreoAprobacionVacaciones(ent.id)
			Else
				EnvioCorreo(ent.id)
			End If
		End If
		If e.CommandName = "Rechazar" Then
			ent.FechaAprobacion = Date.UtcNow.AddHours(-5)
			ent.Estado = 10
			ent.VoBo1 = Session("IDUsuario").ToString
			ent.FechaVoBo1 = Date.UtcNow.AddHours(-5)
			EnvioCorreo(ent.id)
		End If
		o.SaveChangesSolicitud(ent)
		CargarAprobacionesPendientes()
	End Sub

	Protected Sub ReporteIncapacidades()
		Dim vTitulos As String() = "Identificacion;NombreEmpleado;AreaSL;FechaIngreso;Edad;Cargo;Sede;DiaSemana;Mes;FInicio;FFin;DiasCalendario;DiasLaborales;EntidadConsulta;IPSPrestadora;NoRegistroMedico;TipoIncapacidad;ClaseAusencia;SOAT;FechaAccidenteTrabajo;Comentarios;DXAsociado;CIE;CategoriaDX;Estado".Split(";")

		Dim workbook = New XLWorkbook()
		Dim worksheet = workbook.Worksheets.Add("Reporte")

		Dim o As New TH_Ausencia.DAL

        Dim lstDatos = o.ReporteIncapacidades(ddlAnnoAusentismo.SelectedValue)
        worksheet.Cell("A2").InsertData(lstDatos)
		For x = 1 To vTitulos.Count
			worksheet.Cell(1, x).Value = vTitulos(x - 1)
		Next


		Crearexcel(workbook, "ReporteIncapacidades -" & Now.Year & "-" & Now.Month & "-" & Now.Day)
	End Sub
	Protected Sub ReporteAusentismo()
		Dim vTitulos As String() = "Identificacion;NombreEmpleado;AreaSL;FechaIngreso;TipoAusentismo;FInicio;FFin;DiasCalendario;DiasLaborales;Estado".Split(";")

		Dim workbook = New XLWorkbook()
		Dim worksheet = workbook.Worksheets.Add("Reporte")

		Dim o As New TH_Ausencia.DAL

        Dim lstDatos = o.ReporteAusentismo(ddlAnnoAusentismo.SelectedValue)
        worksheet.Cell("A2").InsertData(lstDatos)
		For x = 1 To vTitulos.Count
			worksheet.Cell(1, x).Value = vTitulos(x - 1)
		Next


		Crearexcel(workbook, "ReporteAusentismo-" & Now.Year & "-" & Now.Month & "-" & Now.Day)
	End Sub
	Protected Sub ReporteBeneficios()
		Dim vTitulos As String() = "Identificacion;NombreEmpleado;AreaSL;FechaIngreso;TipoBeneficio;FInicio;FFin;DiasCalendario;DiasLaborales;Estado".Split(";")

		Dim workbook = New XLWorkbook()
		Dim worksheet = workbook.Worksheets.Add("Reporte")

		Dim o As New TH_Ausencia.DAL

        Dim lstDatos = o.ReporteBeneficios(ddlAnnoBeneficios.SelectedValue)
        worksheet.Cell("A2").InsertData(lstDatos)
		For x = 1 To vTitulos.Count
			worksheet.Cell(1, x).Value = vTitulos(x - 1)
		Next


		Crearexcel(workbook, "ReporteBeneficios-" & Now.Year & "-" & Now.Month & "-" & Now.Day)
	End Sub

	Protected Sub ReporteVacaciones()
		Dim vTitulos As String() = "Identificacion;NombreEmpleado;AreaSL;FechaIngreso;DiasDisfrutados;DiasPendientes;ÚltimoPeriodoCausado;Observaciones;Estado".Split(";")

		Dim workbook = New XLWorkbook()
		Dim worksheet = workbook.Worksheets.Add("Reporte")

		Dim o As New TH_Ausencia.DAL

		Dim lstDatos = o.ReporteVacaciones
		worksheet.Cell("A2").InsertData(lstDatos)
		For x = 1 To vTitulos.Count
			worksheet.Cell(1, x).Value = vTitulos(x - 1)
		Next


		Crearexcel(workbook, "ReporteVacaciones-" & Now.Year & "-" & Now.Month & "-" & Now.Day)
	End Sub
	Protected Sub ReporteVacacionesDetallado()
		Dim vTitulos As String() = "Identificacion;NombreEmpleado;Area_SL;IdSolicitud;TipoSolicitud;FechaSolicitud;FechaInicioSolicitud;FechaFinSolicitud;DiasCalendario;DiasLaborales;EstadoSolicitud;Aprueba;FechaAprobacion;ApruebaRRHH;FechaAprobacionRRHH".Split(";")

		Dim workbook = New XLWorkbook()
		Dim worksheet = workbook.Worksheets.Add("Reporte")

		Dim o As New TH_Ausencia.DAL

		Dim lstDatos = o.ReporteVacacionesDetallado
		worksheet.Cell("A2").InsertData(lstDatos)
		For x = 1 To vTitulos.Count
			worksheet.Cell(1, x).Value = vTitulos(x - 1)
		Next


		Crearexcel(workbook, "ReporteVacacionesDetallado-" & Now.Year & "-" & Now.Month & "-" & Now.Day)
	End Sub
	Protected Sub ReporteVacacionesNomina()
		Dim vTitulos As String() = "Identificacion;NombreEmpleado;AreaSL;FechaIngreso;FechaInicio;FechaFin;DiasCalendarioSolicitados;DiasLaboralesSolicitados;DiasCausados;InicioPeriodo;FinPeriodo;FechaSolicitud;AprobadoPor;FechaAprobacion;VoBoRRHH;FechaVoBoRRHH".Split(";")

		Dim workbook = New XLWorkbook()
		Dim worksheet = workbook.Worksheets.Add("Reporte")

		Dim o As New TH_Ausencia.DAL

		Dim lstDatos = o.ReporteVacacionesNomina(ddlMesReporteNomina.SelectedValue, ddlAnno.SelectedValue)
		worksheet.Cell("A2").InsertData(lstDatos)
		For x = 1 To vTitulos.Count
			worksheet.Cell(1, x).Value = vTitulos(x - 1)
		Next


		Crearexcel(workbook, "ReporteVacacionesNomina-" & Now.Year & "-" & Now.Month & "-" & Now.Day)
	End Sub

	Protected Sub ReporteSolicitudesPendientesAprobacion()
		Dim vTitulos As String() = "Identificacion;NombreEmpleado;AreaSL;DiasCalendarioSolicitados;DiasLaboralesSolicitados;FechaSolicitud;FechaInicio;FechaFin;Manager;FechaAprobacionManager;Estado;Tipo".Split(";")

		Dim workbook = New XLWorkbook()
		Dim worksheet = workbook.Worksheets.Add("Reporte")

		Dim o As New TH_Ausencia.DAL

		Dim lstDatos = o.ReporteSolicitudesPendientesAprobacion()
		worksheet.Cell("A2").InsertData(lstDatos)
		For x = 1 To vTitulos.Count
			worksheet.Cell(1, x).Value = vTitulos(x - 1)
		Next


        Crearexcel(workbook, "ReporteSolicitudesPendientesAprobacion-" & Now.Year & "-" & Now.Month & "-" & Now.Day)
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

	Private Sub btnReporteAusentismo_Click(sender As Object, e As EventArgs) Handles btnReporteAusentismo.Click
		ReporteAusentismo()
	End Sub

	Private Sub btnReporteBeneficios_Click(sender As Object, e As EventArgs) Handles btnReporteBeneficios.Click
		ReporteBeneficios()
	End Sub

	Private Sub btnReporteIncapacidades_Click(sender As Object, e As EventArgs) Handles btnReporteIncapacidades.Click
		ReporteIncapacidades()
	End Sub

	Private Sub btnReporteVacaciones_Click(sender As Object, e As EventArgs) Handles btnReporteVacaciones.Click
		ReporteVacaciones()
	End Sub

	Private Sub btnReporteNomina_Click(sender As Object, e As EventArgs) Handles btnReporteNomina.Click
		ReporteVacacionesNomina()
	End Sub

	Private Sub btnSolicitudesPendientes_Click(sender As Object, e As EventArgs) Handles btnSolicitudesPendientes.Click
		ReporteSolicitudesPendientesAprobacion()
	End Sub

	Private Sub btnReporteVacacionesDetallado_Click(sender As Object, e As EventArgs) Handles btnReporteVacacionesDetallado.Click
		ReporteVacacionesDetallado()
	End Sub

	Private Sub llenarDdlAnos()
		Dim anoInicial As Integer
		Dim anoFinal As Integer

		anoFinal = Date.Now.Year + 1

		For anoInicial = 2018 To anoFinal
			ddlAnno.Items.Insert(0, New ListItem With {.Text = anoInicial, .Value = anoInicial})
			ddlAnnoAusentismo.Items.Insert(0, New ListItem With {.Text = anoInicial, .Value = anoInicial})
			ddlAnnoBeneficios.Items.Insert(0, New ListItem With {.Text = anoInicial, .Value = anoInicial})
		Next

	End Sub
End Class
