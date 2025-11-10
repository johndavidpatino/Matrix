Imports CoreProject
Imports ClosedXML.Excel
Imports Utilidades.Encripcion
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Chrome.ChromeDriverService
Imports OpenQA.Selenium.Support.UI
Imports OpenQA.Selenium.Support.UI.SelectElement
Imports System.Threading
Imports System.Data.OleDb
Imports DevExpress.Web.Internal.XmlProcessor

Public Class PresupuestosForm
	Inherits System.Web.UI.Page

#Region "Variables Globales para Grids"
	Dim _TotalViaticos As Decimal = 0
	Dim _TotalHoteles As Decimal = 0
	Dim _TotalTransporte As Decimal = 0
	Dim _Presupuestado As Decimal = 0
	Dim _TotalHoras As Integer = 0
	Dim _Presupuestado2 As Decimal = 0
	Dim _Total As Decimal = 0
	Dim TimeWindows As Integer = 5
	Dim TimeScript As Integer = 3
	Dim TimeShort As Integer = 2

#End Region
#Region "Eventos"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			If Session("InfoJobBook") IsNot Nothing Then
				LoadInfoJobBook()
			End If
		End If
	End Sub

	Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
		If ValidateSavePresupuesto() = False Then Exit Sub
		SavePresupuesto()
		CargarPresupuestos(ddlAlternativa.SelectedValue, ddlTecnica.SelectedValue)
		'UCHeader.ClearControls()
		ShowWarning(TypesWarning.Information, "Registro guardado")
	End Sub

	Protected Sub btnAddPresupuestos_Click(sender As Object, e As EventArgs)
		UCHeader.ClearControls()
		DirectCast(UCHeader.FindControl("gvAnalisisEstadisticos"), GridView).DataSource = ObtenerAnalisisEstadistico(0, 0, 0, 0)
		DirectCast(UCHeader.FindControl("gvAnalisisEstadisticos"), GridView).DataBind()
		DirectCast(UCHeader.FindControl("gvActividadesSubcontratadas"), GridView).DataSource = ObtenerActividadesSubcontratadas(0, 0, 0, 0)
		DirectCast(UCHeader.FindControl("gvActividadesSubcontratadas"), GridView).DataBind()
		DirectCast(UCHeader.FindControl("gvProfessionalTime"), GridView).DataSource = ObtenerHorasProfesionales(0, 0, 0, 0)
		DirectCast(UCHeader.FindControl("gvProfessionalTime"), GridView).DataBind()
		ddlFase.SelectedIndex = 0
		ddlTecnica.SelectedIndex = 0
		txtDuracionMinutos.Text = 0
		If ddlMetodologia.Items.Count > 0 Then ddlMetodologia.SelectedIndex = 0
		gvMuestraCATI.Visible = False
		gvMuestraF2F.Visible = False
		gvMuestraOnline.Visible = False
		lblTotalMuestra.Text = ""
		'DirectCast(UCHeader.FindControl("lblPropuesta"), Label).Text = hfPropuesta.Value
		'DirectCast(UCHeader.FindControl("lblAlternativa"), Label).Text = ddlAlternativa.SelectedValue
		lkb1_ModalPopupExtender.Show()
	End Sub

	Protected Sub btnNewAlternativa_Click(sender As Object, e As EventArgs)
		If ValidarNuevaAlternativa() = False Then Exit Sub
		If DirectCast(Session("InfoJobBook"), oJobBook).NumJobBook = "" Then
			ShowWarning(TypesWarning.Warning, "Puede continuar pero recuerde guardar el número de JobBook en el área de propuesta")
		End If
		pnlPresupuestos.Visible = False
		pnlGeneral.Visible = True
		btnNewAlternativa.Text = "Nueva"
		hfNewAlternativa.Value = True
		txtDescripcionAlternativa.Text = String.Empty
		txtObservacionesGeneral.Text = String.Empty
		txtDiasCampo.Text = 10
		txtDiasDiseno.Text = 5
		txtDiasInformes.Text = 3
		txtDiasProceso.Text = 7
		txtDiasTotal.Text = 25
		txtNoMediciones.Text = 1
		txtPeriodicidad.Text = 1
		lblNumIQuote.Text = ""
	End Sub

	Protected Sub btnCancelGeneral_Click(sender As Object, e As EventArgs)
		If ddlAlternativa.Items.Count > 0 Then
			CargarAlternativa(ddlAlternativa.SelectedValue)
			hfNewAlternativa.Value = False
		Else
			pnlGeneral.Visible = False
			btnNewAlternativa.Text = "Nueva alternativa"
			hfNewAlternativa.Value = True
			txtDescripcionAlternativa.Text = String.Empty
			txtObservacionesGeneral.Text = String.Empty
			txtDiasCampo.Text = 10
			txtDiasDiseno.Text = 5
			txtDiasInformes.Text = 3
			txtDiasProceso.Text = 7
			txtDiasTotal.Text = 25
			txtNoMediciones.Text = 1
			txtPeriodicidad.Text = 1
			lblNumIQuote.Text = ""

		End If

	End Sub

	Protected Sub ddlAlternativa_SelectedIndexChanged(sender As Object, e As EventArgs)
		CargarAlternativa(ddlAlternativa.SelectedValue)
	End Sub

	Protected Sub ddlTecnica_SelectedIndexChanged(sender As Object, e As EventArgs)
		DirectCast(UCHeader.FindControl("btnPruebaProducto"), Button).Enabled = False
		If Not ddlTecnica.SelectedValue = 0 Then CargarMetodologias(ddlTecnica.SelectedValue)
		If Not ddlTecnica.SelectedValue = 0 Then CargarFases(ddlTecnica.SelectedValue)
		If ddlTecnica.SelectedValue = "100" Then
			ddlCiudad.Enabled = True
			DirectCast(UCHeader.FindControl("ddlIncidencia"), DropDownList).Enabled = True
			DirectCast(UCHeader.FindControl("chbProbabilistico"), CheckBox).Enabled = True
			DirectCast(UCHeader.FindControl("chbF2fVirtual"), CheckBox).Enabled = True
		End If
		If ddlTecnica.SelectedValue = "200" Then
			ddlCiudad.Enabled = False
			DirectCast(UCHeader.FindControl("ddlIncidencia"), DropDownList).Enabled = True
			DirectCast(UCHeader.FindControl("chbProbabilistico"), CheckBox).Enabled = False
			DirectCast(UCHeader.FindControl("chbF2fVirtual"), CheckBox).Enabled = False
			DirectCast(UCHeader.FindControl("chbProbabilistico"), CheckBox).Checked = False
			DirectCast(UCHeader.FindControl("chbF2fVirtual"), CheckBox).Checked = False
		End If
		If ddlTecnica.SelectedValue = "300" Then
			ddlCiudad.Enabled = True
			DirectCast(UCHeader.FindControl("ddlIncidencia"), DropDownList).Enabled = False
			DirectCast(UCHeader.FindControl("chbProbabilistico"), CheckBox).Enabled = False
			DirectCast(UCHeader.FindControl("chbF2fVirtual"), CheckBox).Enabled = False
			DirectCast(UCHeader.FindControl("ddlIncidencia"), DropDownList).SelectedIndex = 0
			DirectCast(UCHeader.FindControl("chbProbabilistico"), CheckBox).Checked = False
			DirectCast(UCHeader.FindControl("chbF2fVirtual"), CheckBox).Checked = False
		End If

		lkb1_ModalPopupExtender.Show()
	End Sub

	Protected Sub ddlMetodologia_SelectedIndexChanged(sender As Object, e As EventArgs)
		If ddlMetodologia.SelectedValue = 105 Or ddlMetodologia.SelectedValue = 120 Or ddlMetodologia.SelectedValue = 125 Then
			DirectCast(UCHeader.FindControl("btnPruebaProducto"), Button).Enabled = True
		Else
			DirectCast(UCHeader.FindControl("btnPruebaProducto"), Button).Enabled = False
		End If
		If Not ddlMetodologia.SelectedValue = 0 Then CargarTipoMuestra(ddlMetodologia.SelectedValue)
		Dim alternativa As Integer? = 0
		If ddlAlternativa.Items.Count > 0 Then alternativa = ddlAlternativa.SelectedValue
		CargarCiudades(hfPropuesta.Value, alternativa, ddlMetodologia.SelectedValue, ddlFase.SelectedValue)
		lkb1_ModalPopupExtender.Show()
	End Sub

	Protected Sub gvPresupuestos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
		Dim idTecnica As Integer = Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("IdTecnica"))
		If e.CommandName = "EditP" Then
			Dim infoP = ObtenerPresupuesto(hfPropuesta.Value, ddlAlternativa.SelectedValue, Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo")), Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL")))
			CargarPresupuesto(infoP)
			lkb1_ModalPopupExtender.Show()
		End If
		If e.CommandName = "DeleteP" Then
			BorrarPresupuesto(hfPropuesta.Value, ddlAlternativa.SelectedValue, Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo")), Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL")))
			CargarPresupuestos(ddlAlternativa.SelectedValue, idTecnica)
		End If
		If e.CommandName = "DetailsP" Then
			Dim infoP = ObtenerPresupuesto(hfPropuesta.Value, ddlAlternativa.SelectedValue, Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo")), Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL")))
			txtNuevoGM.Text = CDec(infoP.ParGrossMargin).ToString("P2")
			hfMetCodigo.Value = Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo"))
			hfFase.Value = Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL"))
			CargarJBI(hfPropuesta.Value, ddlAlternativa.SelectedValue, Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo")), Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL")), 0, 0, 0)
			CargarJBE(hfPropuesta.Value, ddlAlternativa.SelectedValue, Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo")), Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL")), 0, 0, 0)
			ModalPopupExtenderGM.Show()
			'UPanelGM.Update()
		End If
		If e.CommandName = "SimulatorP" Then
			Dim oCot As New CoreProject.Cotizador.General
			'txtNuevoGM.Text = CDec(infoP.ParGrossMargin).ToString("P2")
			'hfMetCodigo.Value = Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo"))
			'hfFase.Value = Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL"))
			'CargarJBI(hfPropuesta.Value, ddlAlternativa.SelectedValue, Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo")), Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL")), 0, 0, 0)
			'CargarJBE(hfPropuesta.Value, ddlAlternativa.SelectedValue, Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo")), Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL")), 0, 0, 0)
			CargarDatosSimulador(hfPropuesta.Value, ddlAlternativa.SelectedValue, Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo")), Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL")), 1, Nothing, Nothing, Nothing, Nothing)
			'UPanelGM.Update()
		End If
		If e.CommandName = "CalcProfessionalTimeP" Then
			Dim oCot As New CoreProject.Cotizador.General
			hfMetCodigoCostos.Value = Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo"))
			hfFaseCostos.Value = Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL"))
			oCot.PUTCalculoHorasProfesionales(hfPropuesta.Value, ddlAlternativa.SelectedValue, Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo")), Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL")))
			ShowWarning(TypesWarning.Information, "El cálculo de horas ha sido realizado")
		End If
		If e.CommandName = "JBIP" Then
			If chbObserver.Checked = True Then Exit Sub
			If hfOPS.Value = 0 Then Exit Sub
			hfMetCodigoJBI.Value = Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo"))
			hfFaseJBI.Value = Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL"))
			CargarCostosJBI(hfPropuesta.Value, ddlAlternativa.SelectedValue, Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo")), Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL")))
			ModalPopupExtenderJBI.Show()
		End If
		If e.CommandName = "JBEP" Then
			hfMetCodigoJBE.Value = Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo"))
			hfFaseJBE.Value = Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL"))
			CargarCostosJBE(hfPropuesta.Value, ddlAlternativa.SelectedValue, Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo")), Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL")))
			ModalPopupExtenderJBE.Show()
		End If
		If e.CommandName = "CopyP" Then
			hfMetCodigoCopiar.Value = Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo"))
			hfFaseCopiar.Value = Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL"))
			CargarAlternativasDisponibles(hfPropuesta.Value)
			ModalPopupExtenderCopiarPresupuesto.Show()
		End If
		If e.CommandName = "ExecP" Then
			hfMetCodigoCostos.Value = Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo"))
			hfFaseCostos.Value = Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL"))
			CargarDetalleCostos(hfPropuesta.Value, ddlAlternativa.SelectedValue, Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo")), Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL")))
			ModalPopupExtenderExecution.Show()
		End If
		If e.CommandName = "ReviewP" Then
			Dim oCot As New CoreProject.Cotizador.General
			Dim IQP As New CoreProject.IQ_Parametros
			IQP = oCot.GetPresupuesto(hfPropuesta.Value, ddlAlternativa.SelectedValue, Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo")), Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL")))
			IQP.ParFechaRevision = Date.UtcNow.AddHours(-5)
			IQP.ParRevisado = True
			IQP.ParRevisadoPor = Session("IDUsuario").ToString
			oCot.PutSaveParametros(IQP, False)
			CargarPresupuestos(ddlAlternativa.SelectedValue, idTecnica)
		End If
		If e.CommandName = "UndoReviewP" Then
			Dim oCot As New CoreProject.Cotizador.General
			Dim IQP As New CoreProject.IQ_Parametros
			IQP = oCot.GetPresupuesto(hfPropuesta.Value, ddlAlternativa.SelectedValue, Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("MetCodigo")), Int64.Parse(gvPresupuestos.DataKeys(CInt(e.CommandArgument))("NACIONAL")))
			IQP.ParFechaRevision = Nothing
			IQP.ParRevisado = Nothing
			IQP.ParRevisadoPor = Nothing
			oCot.PutSaveParametros(IQP, False)
			CargarPresupuestos(ddlAlternativa.SelectedValue, idTecnica)
		End If
	End Sub

	Protected Sub btnSaveGeneral_Click(sender As Object, e As EventArgs)
		If ValidateSaveGeneral() = False Then Exit Sub
		SaveGeneralValues()
	End Sub

	Sub SaveGeneralValues()
		pnlPresupuestos.Visible = True
		btnAddPresupuestos.Visible = True
		Dim oCot As New CoreProject.Cotizador.General
		Dim IQDG As New CoreProject.IQ_DatosGeneralesPresupuesto
		IQDG.IdPropuesta = hfPropuesta.Value
		If hfNewAlternativa.Value = True Then
			IQDG.ParAlternativa = oCot.GetUltimaAlternativa(hfPropuesta.Value) + 1
		Else
			IQDG.ParAlternativa = ddlAlternativa.SelectedValue
		End If
		IQDG.Descripcion = txtDescripcionAlternativa.Text
		IQDG.Observaciones = txtObservacionesGeneral.Text
		IQDG.DiasCampo = txtDiasCampo.Text
		IQDG.DiasDiseno = txtDiasDiseno.Text
		IQDG.DiasInformes = txtDiasInformes.Text
		IQDG.DiasProcesamiento = txtDiasProceso.Text
		IQDG.NumeroMediciones = txtNoMediciones.Text
		IQDG.MesesMediciones = txtPeriodicidad.Text
		If chbObserver.Checked = True Then
			IQDG.TipoPresupuesto = 2
		Else
			IQDG.TipoPresupuesto = 1
		End If
		IQDG.Plazo = 30
		IQDG.Saldo = 30
		IQDG.Anticipo = 70
		IQDG.TasaCambio = 4000
		oCot.PutDatosGenerales(IQDG)
		hfNewAlternativa.Value = False
		NumAlternativas()
		ddlAlternativa.SelectedValue = IQDG.ParAlternativa
		CargarAlternativa(ddlAlternativa.SelectedValue)
	End Sub
	Protected Sub btnImportar_Click(sender As Object, e As EventArgs)
		'ShowWarning(TypesWarning.Information, "Test")
		If ValidarNuevaAlternativa() = False Then Exit Sub
		ModalPopupExtenderImport.Show()
	End Sub


	Protected Sub btnSearchImport_Click(sender As Object, e As EventArgs)
		Dim oData As New CU_JobBook.DAL
		Dim idPropuesta As Int64?
		If IsNumeric(txtIdPropuestaSearch.Text) Then idPropuesta = txtIdPropuestaSearch.Text
		gvDataSearchImport.DataSource = oData.InfoJobBookGet(txtTituloSearch.Text, txtJobBookSearch.Text, idPropuesta, Session("IDUsuario").ToString, ddlSearch.SelectedValue)
		gvDataSearchImport.DataBind()
		ModalPopupExtenderImport.Show()
	End Sub

	Protected Sub gvDataSearchImport_RowCommand(sender As Object, e As GridViewCommandEventArgs)
		Dim oPresupuesto As New Presupuesto
		gvPresupuestosImport.DataSource = oPresupuesto.DevolverxIdPropuesta(Int64.Parse(gvDataSearchImport.DataKeys(CInt(e.CommandArgument))("IdPropuesta")), Nothing)
		gvPresupuestosImport.DataBind()
		ModalPopupExtenderClonar.Show()
	End Sub

	Protected Sub gvPresupuestosImport_RowCommand(sender As Object, e As GridViewCommandEventArgs)
		Dim oIQ As New CoreProject.IQ.Transacciones
		oIQ.DuplicarAlternativaToPropuesta(Int64.Parse(gvPresupuestosImport.DataKeys(CInt(e.CommandArgument))("PropuestaId")), Int64.Parse(gvPresupuestosImport.DataKeys(CInt(e.CommandArgument))("Alternativa")), hfPropuesta.Value)
		ShowWarning(TypesWarning.Information, "La alternativa ha sido importada")
		NumAlternativas()
		ddlAlternativa.SelectedIndex = ddlAlternativa.Items.Count - 1
		CargarAlternativa(ddlAlternativa.SelectedValue)
	End Sub

	Protected Sub btnDuplicarAlternativa_Click(sender As Object, e As EventArgs)
		Dim oCot As New CoreProject.Cotizador.General
		oCot.PutDuplicar(hfPropuesta.Value, ddlAlternativa.SelectedValue)
		ShowWarning(TypesWarning.Information, "La alternativa ha sido duplicada")
		NumAlternativas()
		ddlAlternativa.SelectedIndex = ddlAlternativa.Items.Count - 1
		CargarAlternativa(ddlAlternativa.SelectedValue)
	End Sub

	Protected Sub btnRevision_Click(sender As Object, e As EventArgs)
		EnvioPresupuestoRevision(hfPropuesta.Value, ddlAlternativa.SelectedValue)
	End Sub

	Protected Sub btnAddMuestra_Click(sender As Object, e As EventArgs)
		If ddlCiudad.Enabled = True And (ddlCiudad.SelectedValue = "0" Or ddlCiudad.SelectedIndex = 0) Then
			ShowWarning(TypesWarning.ErrorMessage, "Seleccione primero la ciudad")
			'lkb1_ModalPopupExtender.Show()
			Exit Sub
		End If
		If ddlDificultadMuestra.SelectedValue = "0" Then
			ShowWarning(TypesWarning.ErrorMessage, "Seleccione el tipo de muestra antes de continuar")
			'lkb1_ModalPopupExtender.Show()
			Exit Sub
		End If
		If Not (IsNumeric(txtCantidadMuestra.Text)) Then
			ShowWarning(TypesWarning.ErrorMessage, "Digite la cantidad antes de continuar")
			'lkb1_ModalPopupExtender.Show()
			Exit Sub
		End If
		If ddlFase.SelectedIndex = 0 Then
			ShowWarning(TypesWarning.ErrorMessage, "Por favor seleccione la fase antes de continuar")
			Exit Sub
		End If
		If ddlMetodologia.SelectedIndex = 0 Then
			ShowWarning(TypesWarning.ErrorMessage, "Por favor seleccione la metodología antes de continuar")
			Exit Sub
		End If
		If ddlTecnica.SelectedIndex = 0 Then
			ShowWarning(TypesWarning.ErrorMessage, "Por favor seleccione la técnica antes de continuar")
			Exit Sub
		End If
		If (ddlTecnica.SelectedValue = 100 Or ddlTecnica.SelectedValue = 200) And DirectCast(UCHeader.FindControl("ddlIncidencia"), DropDownList).SelectedIndex = 0 Then
			ShowWarning(TypesWarning.ErrorMessage, "Por favor seleccione la incidencia antes de continuar")
			Exit Sub
		End If
		Dim IQM As New IQ_Muestra_1
		If ddlCiudad.Enabled = False Then
			IQM.CiuCodigo = 0
			IQM.DeptCodigo = 0
		Else
			IQM.CiuCodigo = ddlCiudad.SelectedValue
		End If
		IQM.IdPropuesta = hfPropuesta.Value
		IQM.ParAlternativa = ddlAlternativa.SelectedValue
		IQM.ParNacional = ddlFase.SelectedValue
		IQM.MetCodigo = ddlMetodologia.SelectedValue
		IQM.MuCantidad = txtCantidadMuestra.Text
		IQM.MuIdentificador = ddlDificultadMuestra.SelectedValue
		txtCantidadMuestra.Text = 0
		ddlDificultadMuestra.SelectedIndex = 0
		Try
			ddlCiudad.ClearSelection()
		Catch ex As Exception
		End Try

		SaveMuestra(IQM)
		lkb1_ModalPopupExtender.Show()
	End Sub

	Protected Sub gvMuestraF2F_RowCommand(sender As Object, e As GridViewCommandEventArgs)
		Dim oCot As New CoreProject.Cotizador.General
		Dim muestra As New IQ_Muestra_1
		muestra.IdPropuesta = hfPropuesta.Value
		muestra.ParAlternativa = ddlAlternativa.SelectedValue
		muestra.ParNacional = ddlFase.SelectedValue
		muestra.MetCodigo = ddlMetodologia.SelectedValue
		muestra.CiuCodigo = Int64.Parse(gvMuestraF2F.DataKeys(CInt(e.CommandArgument))("Codigo"))
		oCot.DELMuestra(muestra)
		CargarMuestra()
		lkb1_ModalPopupExtender.Show()
	End Sub

	Protected Sub gvMuestraCATI_RowCommand(sender As Object, e As GridViewCommandEventArgs)
		Dim oCot As New CoreProject.Cotizador.General
		Dim muestra As New IQ_Muestra_1
		muestra.IdPropuesta = hfPropuesta.Value
		muestra.ParAlternativa = ddlAlternativa.SelectedValue
		muestra.ParNacional = ddlFase.SelectedValue
		muestra.MetCodigo = ddlMetodologia.SelectedValue
		muestra.MuIdentificador = Int64.Parse(gvMuestraCATI.DataKeys(CInt(e.CommandArgument))("IDENTIFICADOR"))
		muestra.CiuCodigo = 0
		oCot.DELMuestra(muestra)
		CargarMuestra()
		lkb1_ModalPopupExtender.Show()
	End Sub

	Protected Sub gvMuestraOnline_RowCommand(sender As Object, e As GridViewCommandEventArgs)
		Dim oCot As New CoreProject.Cotizador.General
		Dim muestra As New IQ_Muestra_1
		muestra.IdPropuesta = hfPropuesta.Value
		muestra.ParAlternativa = ddlAlternativa.SelectedValue
		muestra.ParNacional = ddlFase.SelectedValue
		muestra.MetCodigo = ddlMetodologia.SelectedValue
		muestra.CiuCodigo = Int64.Parse(gvMuestraOnline.DataKeys(CInt(e.CommandArgument))("Codigo"))
		oCot.DELMuestra(muestra)
		CargarMuestra()
		lkb1_ModalPopupExtender.Show()
	End Sub


	Protected Sub GVJBE_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVJBE.RowDataBound

		If e.Row.RowType = DataControlRowType.DataRow Then

			If e.Row.Cells(0).Text.ToString.IndexOf("PORCENTAJE") > -1 Then
				e.Row.Cells(1).Text = CDec(e.Row.Cells(1).Text).ToString("P2")
			Else
				e.Row.Cells(1).Text = CDec(e.Row.Cells(1).Text).ToString("C0")
			End If

			If e.Row.Cells(0).Text.ToString.IndexOf("TOTAL") > -1 Or e.Row.Cells(0).Text.ToString.IndexOf("GROSS") > -1 Or e.Row.Cells(0).Text.ToString.IndexOf("VENTA") > -1 Then
				'e.Row.Font.Bold = True
			End If

			e.Row.Cells(1).CssClass = "RightAlign"
			e.Row.Cells(0).Text = UInitialCase(LCase(e.Row.Cells(0).Text))
		End If

	End Sub

	Protected Sub GVJBI_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GVJBI.RowDataBound
		If e.Row.RowType = DataControlRowType.DataRow Then
			If e.Row.Cells(0).Text.ToString.IndexOf("PORCENTAJE") > -1 Then
				e.Row.Cells(1).Text = CDec(e.Row.Cells(1).Text).ToString("P2")
			Else
				e.Row.Cells(1).Text = CDec(e.Row.Cells(1).Text).ToString("C0")
			End If

			If e.Row.Cells(0).Text.ToString.IndexOf("TOTAL") > -1 Or e.Row.Cells(0).Text.ToString.IndexOf("GROSS") > -1 Or e.Row.Cells(0).Text.ToString.IndexOf("VENTA") > -1 Then
				'e.Row.Font.Bold = True
			End If

			e.Row.Cells(1).CssClass = "RightAlign"
			e.Row.Cells(0).Text = UInitialCase(LCase(e.Row.Cells(0).Text))
		End If

	End Sub

	Protected Sub btnSimular_Click(sender As Object, e As EventArgs)
		ModalPopupExtenderGM.Show()
		If Not (txtValorVentaSimular.Text = String.Empty) Then
			Dim oCot As New CoreProject.Cotizador.General
			lblGMsimulado.Text = (oCot.GetSimularGM(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, CDec(txtValorVentaSimular.Text), 1) * 100).ToString("N2")
		Else
			ShowWarning(TypesWarning.ErrorMessage, "Debe digitar el valor de la venta para simular el gross")
		End If

	End Sub

	Protected Sub btnSimValorVenta_Click(sender As Object, e As EventArgs)
		ModalPopupExtenderGM.Show()
		If Not (txtNuevoGM.Text.Trim = String.Empty Or txtGMOpera.Text.Trim = String.Empty) Then
			Dim gmOpe, gmUni As Decimal
			Dim oCot As New CoreProject.Cotizador.General
			If txtGMOpera.Text = String.Empty Then
				gmOpe = -1
			Else
				gmOpe = CDec(txtGMOpera.Text) / 100
			End If

			If txtNuevoGM.Text = String.Empty Then
				gmUni = -1
			Else
				gmUni = CDec(txtNuevoGM.Text) / 100
			End If
			lblValorVentaSimulado.Text = (oCot.GetSimularVenta(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, If((txtNuevoGM.Text.Trim = String.Empty), -1, (CDec(txtNuevoGM.Text) / 100)), gmOpe, 1)).ToString("C")
			CargarJBI(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, gmUni, gmOpe, True)
			CargarJBE(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, gmUni, gmOpe, True)
		Else
			ShowWarning(TypesWarning.ErrorMessage, "Se debe digitar el valor del nuevo Gross Margin o el Gross Margin de Operaciones")
		End If
	End Sub


#End Region

#Region "Metodos"
	Sub ShowWarning(ByVal Tipo As TypesWarning, ByVal TextMessage As String)
		pnlMsgTextError.Visible = False
		pnlMsgTextInfo.Visible = False
		pnlMsgTextWarning.Visible = False
		Select Case Tipo
			Case TypesWarning.Warning
				lblTitleWarning.Text = "Advertencia"
				pnlMsgTextWarning.Visible = True
				lblMsgTextWarning.Text = TextMessage
			Case TypesWarning.ErrorMessage
				lblTitleWarning.Text = "Error"
				pnlMsgTextError.Visible = True
				lblMsgTextError.Text = TextMessage
			Case TypesWarning.Information
				lblTitleWarning.Text = "Información"
				pnlMsgTextInfo.Visible = True
				lblMsgTextInfo.Text = TextMessage
		End Select
		UPanelMessage.Update()
		ModalPopupExtenderWarning.Show()
	End Sub


	Function ValidarNuevaAlternativa() As Boolean
		If Session("InfoJobBook") IsNot Nothing Then
			If DirectCast(Session("InfoJobBook"), oJobBook).GuardarCambios = False Then
				ShowWarning(TypesWarning.ErrorMessage, "No puede agregar presupuestos a una propuesta diferente a su unidad")
				Return False
			End If
			If DirectCast(Session("InfoJobBook"), oJobBook).IdPropuesta = 0 Or DirectCast(Session("InfoJobBook"), oJobBook).Viabilidad Is Nothing Or DirectCast(Session("InfoJobBook"), oJobBook).Viabilidad = False Then
				ShowWarning(TypesWarning.ErrorMessage, "Se debe definir la viabilidad del frame antes de continuar")
				Return False
			End If
		Else
			ShowWarning(TypesWarning.ErrorMessage, "Se requiere una propuesta activa para poder crear presupuestos")
			Return False
		End If
		Return True
	End Function

	Function ValidateSaveGeneral()
		If Not (IsNumeric(txtDiasCampo.Text)) Or txtDiasCampo.Text = "0" Then
			ShowWarning(TypesWarning.ErrorMessage, "El número de días de campo no es válido")
			Return False
		End If
		If Not (IsNumeric(txtDiasDiseno.Text)) Then
			ShowWarning(TypesWarning.ErrorMessage, "El número de días de diseño no es válido")
			Return False
		End If
		If Not (IsNumeric(txtDiasInformes.Text)) Then
			ShowWarning(TypesWarning.ErrorMessage, "El número de días de informes no es válido")
			Return False
		End If
		If Not (IsNumeric(txtDiasProceso.Text)) Then
			ShowWarning(TypesWarning.ErrorMessage, "El número de días de proceso no es válido")
			Return False
		End If
		Return True
	End Function

	Sub LoadInfoJobBook()
		If Not (Session("InfoJobBook") Is Nothing) Then
			Dim infoJobBook As oJobBook = Session("InfoJobBook")
			lblInfo.Text = infoJobBook.NumJobBook & " | " & infoJobBook.Titulo & " | " & infoJobBook.Cliente & " | " & infoJobBook.IdPropuesta.ToString
			hfPropuesta.Value = infoJobBook.IdPropuesta
			If infoJobBook.GuardarCambios = False Then
				btnNewAlternativa.Enabled = False
				btnImportar.Enabled = False
				btnSaveGeneral.Visible = False
				btnDuplicarAlternativa.Enabled = False
				btnGuardar.Enabled = False
			End If
			NumAlternativas()
			If ddlAlternativa.Items.Count > 0 Then
				CargarAlternativa(1)
				btnNewAlternativa.Text = "Nueva"
				hfNewAlternativa.Value = False
			End If
			If infoJobBook.ReviewOPS = True Then
				hfOPS.Value = 1
				pnlSideBarCuentas.Visible = False
				pnlSidebarOPS.Visible = True
				CargarAlternativa(infoJobBook.Alternativa)
				gvPresupuestos.Columns(0).Visible = True
			Else
				hfOPS.Value = 0
				pnlSideBarCuentas.Visible = True
				pnlSidebarOPS.Visible = False
				gvPresupuestos.Columns(0).Visible = False
			End If
		End If
	End Sub

	Sub CargarCiudades(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer)
		Dim oCot As New CoreProject.Cotizador.General
		ddlCiudad.DataSource = oCot.GetListadoCiudadesGeneral(idPropuesta, Alternativa, Metodologia, Fase)
		ddlCiudad.DataValueField = "CiuCiudad"
		ddlCiudad.DataTextField = "NomCiudad"
		ddlCiudad.DataBind()
	End Sub

	Sub NumAlternativas()
		Dim o As New CU_JobBook.DAL
		Dim NumAlt As Integer = o.NumAlternativas(DirectCast(Session("InfoJobBook"), oJobBook).IdPropuesta)
		If NumAlt > 0 Then
			pnlAlternativa.Visible = True
			lblNumAlternativas.Text = NumAlt.ToString
			ddlAlternativa.Items.Clear()
			For i As Integer = 1 To NumAlt
				ddlAlternativa.Items.Add(New ListItem With {.Value = i, .Text = i.ToString})
				btnDuplicarAlternativa.Visible = True
			Next
			ddlAlternativa.DataBind()
		Else
			pnlAlternativa.Visible = False
		End If
	End Sub

	Sub CargarAlternativa(ByVal Alternativa As Integer)
		pnlGeneral.Visible = True
		Dim oCot As New CoreProject.Cotizador.General
		Dim infoG = oCot.GetGeneralByAlternativa(DirectCast(Session("InfoJobBook"), oJobBook).IdPropuesta, Alternativa)
		txtDescripcionAlternativa.Text = infoG.Descripcion
		txtObservacionesGeneral.Text = infoG.Observaciones
		txtDiasCampo.Text = infoG.DiasCampo
		txtDiasDiseno.Text = infoG.DiasDiseno
		txtDiasInformes.Text = infoG.DiasInformes
		txtDiasProceso.Text = infoG.DiasProcesamiento
		txtDiasTotal.Text = infoG.DiasCampo + infoG.DiasDiseno + infoG.DiasInformes + infoG.DiasProcesamiento
		txtNoMediciones.Text = infoG.NumeroMediciones
		txtPeriodicidad.Text = infoG.MesesMediciones
		If infoG.TipoPresupuesto = 2 Then chbObserver.Checked = True
		If infoG.NoIQuote IsNot Nothing Then
			lblNumIQuote.Text = infoG.NoIQuote
		Else
			lblNumIQuote.Text = ""
		End If
		CargarPresupuestos(Alternativa)
		Dim oPresupuesto As New Presupuesto
		If oPresupuesto.Cu_PresupestoGetByPropAndAlt(DirectCast(Session("InfoJobBook"), oJobBook).IdPropuesta, Alternativa) IsNot Nothing Then
			If oPresupuesto.Cu_PresupestoGetByPropAndAlt(DirectCast(Session("InfoJobBook"), oJobBook).IdPropuesta, Alternativa).ParaRevisar = True Then
				btnRevision.Visible = False
				If oPresupuesto.Cu_PresupestoGetByPropAndAlt(DirectCast(Session("InfoJobBook"), oJobBook).IdPropuesta, Alternativa).Aprobado = True And Not (DirectCast(Session("InfoJobBook"), oJobBook).NumJobBook = "") Then
					btnExportIQuote.Visible = True
				Else
					btnExportIQuote.Visible = False
				End If
			Else
				btnRevision.Visible = True
				btnExportIQuote.Visible = False
			End If
		End If
		ddlAlternativa.SelectedValue = Alternativa
	End Sub

	Sub CargarPresupuestos(ByVal Alternativa As Integer, Optional ByVal Tecnica As Integer = 0)
		pnlPresupuestos.Visible = True
		btnAddPresupuestos.Visible = True
		Dim oCot As New CoreProject.Cotizador.General
		gvPresupuestos.DataSource = oCot.GetPresupuestosByTecnica(DirectCast(Session("InfoJobBook"), oJobBook).IdPropuesta, Alternativa, Nothing)
		gvPresupuestos.DataBind()
		If gvPresupuestos.Rows.Count > 0 Then
			'btnCalcProfessionalTime.Visible = True
			btnImportarMuestraExcel.Visible = True
		Else
			btnCalcProfessionalTime.Visible = False
			btnImportarMuestraExcel.Visible = False
		End If
	End Sub

	Sub CargarMetodologias(ByVal Tecnica As Integer)
		Dim oCot As New CoreProject.Cotizador.General
		ddlMetodologia.DataSource = oCot.GetMetodologiasByTecnica(Tecnica)
		ddlMetodologia.DataValueField = "MetCodigo"
		ddlMetodologia.DataTextField = "MetNombre"
		ddlMetodologia.DataBind()
		ddlMetodologia.Items.Insert(0, New ListItem With {.Text = "Seleccione..", .Value = "0"})
	End Sub

	Sub CargarFases(ByVal Tecnica As Integer)
		Dim oCot As New CoreProject.Cotizador.General
		ddlFase.DataSource = oCot.GetFasesByTecnica(Tecnica)
		ddlFase.DataValueField = "IdFase"
		ddlFase.DataTextField = "DescFase"
		ddlFase.DataBind()
		ddlFase.Items.Insert(0, New ListItem With {.Text = "Seleccione..", .Value = "0"})
	End Sub

	Sub CargarTipoMuestra(ByVal Metodologia As Integer)
		Dim oCot As New CoreProject.Cotizador.General
		ddlDificultadMuestra.DataSource = oCot.GetTipoMuestraByMetodologia(Metodologia)
		ddlDificultadMuestra.DataValueField = "IdIdentificador"
		ddlDificultadMuestra.DataTextField = "DescIdentMuestra"
		ddlDificultadMuestra.DataBind()
		ddlDificultadMuestra.Items.Insert(0, New ListItem With {.Text = "Seleccione..", .Value = "0"})
	End Sub

	Private Function ObtenerPresupuesto(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As CoreProject.IQ_Parametros
		Dim oCot As New CoreProject.Cotizador.General
		Return oCot.GetPresupuesto(idPropuesta, Alternativa, Metodologia, Fase)
	End Function

	Private Sub BorrarPresupuesto(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer)
		Dim oCot As New CoreProject.Cotizador.General
		oCot.DELPresupuesto(idPropuesta, Alternativa, Metodologia, Fase)
	End Sub

	Private Function ObtenerProcesos(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of CoreProject.IQ_ProcesosPresupuesto)
		Dim oCot As New CoreProject.Cotizador.General
		Return oCot.GetProcesos(idPropuesta, Alternativa, Metodologia, Fase)
	End Function

	Private Function ObtenerAnalisisEstadistico(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of CoreProject.IQ_ObtenerAnalisisEstadisticos_Result)
		Dim oCot As New CoreProject.Cotizador.General
		Return oCot.GetAnalisisEstadisticos(idPropuesta, Alternativa, Metodologia, Fase)
	End Function

	Private Function ObtenerActividadesSubcontratadas(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of CoreProject.IQ_ObtenerActividades_Result)
		Dim oCot As New CoreProject.Cotizador.General
		Return oCot.GetActividadesSubcontratadas(idPropuesta, Alternativa, Metodologia, Fase)
	End Function

	Private Function ObtenerHorasProfesionales(ByVal idPropuesta As Int64, Alternativa As Integer, Optional Metodologia As Integer? = Nothing, Optional Fase As Integer? = Nothing) As List(Of CoreProject.IQ_ObtenerHorasProfesionales_Result)
		Dim oCot As New CoreProject.Cotizador.General
		Return oCot.GetHorasProfesionales(idPropuesta, Alternativa, Metodologia, Fase)
	End Function

	Private Function ObtenerHorasProfesionalesXAlternativa(ByVal idPropuesta As Int64, Alternativa As Integer) As List(Of CoreProject.IQ_ObtenerHorasProfesionalesXAlternativa_Result)
		Dim oCot As New CoreProject.Cotizador.General
		Return oCot.GetHorasProfesionalesByAlternativa(idPropuesta, Alternativa)
	End Function

	Sub CargarPresupuesto(ByVal infoP As IQ_Parametros)
		UCHeader.ClearControls()
		ddlTecnica.SelectedValue = infoP.TecCodigo
		If ddlTecnica.SelectedValue = "100" Then ddlCiudad.Enabled = True
		If ddlTecnica.SelectedValue = "200" Then ddlCiudad.Enabled = False
		If ddlTecnica.SelectedValue = "300" Then ddlCiudad.Enabled = True
		If ddlTecnica.SelectedValue = "100" Then CargarCiudades(infoP.IdPropuesta, infoP.ParAlternativa, infoP.MetCodigo, infoP.ParNacional)
		If ddlTecnica.SelectedValue = "300" Then CargarCiudades(infoP.IdPropuesta, infoP.ParAlternativa, infoP.MetCodigo, infoP.ParNacional)
		CargarMetodologias(infoP.TecCodigo)
		ddlMetodologia.SelectedValue = infoP.MetCodigo
		If ddlMetodologia.SelectedValue = 105 Or ddlMetodologia.SelectedValue = 120 Or ddlMetodologia.SelectedValue = 125 Then
			DirectCast(UCHeader.FindControl("btnPruebaProducto"), Button).Enabled = True
		Else
			DirectCast(UCHeader.FindControl("btnPruebaProducto"), Button).Enabled = False
		End If
		CargarTipoMuestra(infoP.MetCodigo)
		CargarFases(infoP.TecCodigo)
		If infoP.ParNacional = 2 Then
			ddlFase.SelectedValue = 17
		Else
			ddlFase.SelectedValue = infoP.ParNacional
		End If
		txtDuracionMinutos.Text = infoP.ParTiempoEncuesta

		If Not infoP.Complejidad Is Nothing Then ddlComplejidad.SelectedValue = infoP.Complejidad
		DirectCast(UCHeader.FindControl("txtGrupoObjetivo"), TextBox).Text = infoP.ParGrupoObjetivo
		DirectCast(UCHeader.FindControl("txtCerradas"), TextBox).Text = infoP.IQ_Preguntas.PregCerradas
		DirectCast(UCHeader.FindControl("txtCerradasMultiples"), TextBox).Text = infoP.IQ_Preguntas.PregCerradasMultiples
		DirectCast(UCHeader.FindControl("txtAbiertas"), TextBox).Text = infoP.IQ_Preguntas.PregAbiertas
		DirectCast(UCHeader.FindControl("txtAbiertasMultiples"), TextBox).Text = infoP.IQ_Preguntas.PregAbiertasMultiples
		DirectCast(UCHeader.FindControl("txtOtros"), TextBox).Text = infoP.IQ_Preguntas.PregOtras
		DirectCast(UCHeader.FindControl("txtDemograficos"), TextBox).Text = infoP.IQ_Preguntas.PregDemograficos

		If infoP.ParProductividad IsNot Nothing Then DirectCast(UCHeader.FindControl("txtProductividad"), TextBox).Text = infoP.ParProductividad
		If infoP.ParIncidencia IsNot Nothing Then DirectCast(UCHeader.FindControl("ddlIncidencia"), DropDownList).SelectedValue = infoP.ParIncidencia
		If infoP.ParProbabilistico IsNot Nothing Then DirectCast(UCHeader.FindControl("chbProbabilistico"), CheckBox).Checked = infoP.ParProbabilistico
		If infoP.F2FVirtual IsNot Nothing Then DirectCast(UCHeader.FindControl("chbF2fVirtual"), CheckBox).Checked = infoP.F2FVirtual


		Dim lProcesos = ObtenerProcesos(hfPropuesta.Value, ddlAlternativa.SelectedValue, infoP.MetCodigo, infoP.ParNacional)
		For Each item In lProcesos
			Select Case item.ProcCodigo
				Case 1
					DirectCast(UCHeader.FindControl("chbProcessCampo"), CheckBox).Checked = True
				Case 2
					DirectCast(UCHeader.FindControl("chbProcessVerificacion"), CheckBox).Checked = True
				Case 3
					DirectCast(UCHeader.FindControl("chbProcessCritica"), CheckBox).Checked = True
				Case 4
					DirectCast(UCHeader.FindControl("chbProcessCodificacion"), CheckBox).Checked = True
				Case 6
					DirectCast(UCHeader.FindControl("chbProcessDataClean"), CheckBox).Checked = True
				Case 7
					DirectCast(UCHeader.FindControl("chbProcessTopLines"), CheckBox).Checked = True
				Case 8
					DirectCast(UCHeader.FindControl("chbProcessProceso"), CheckBox).Checked = True
				Case 9
					DirectCast(UCHeader.FindControl("chbProcessArchivos"), CheckBox).Checked = True
				Case 10
					DirectCast(UCHeader.FindControl("chbProcessScripting"), CheckBox).Checked = True
			End Select
		Next

		If Not infoP.ComplejidadCodificacion Is Nothing Then
			DirectCast(UCHeader.FindControl("ddlComplejidadCodificación"), DropDownList).SelectedValue = infoP.ComplejidadCodificacion
		Else
			DirectCast(UCHeader.FindControl("ddlComplejidadCodificación"), DropDownList).SelectedIndex = 1
		End If

		If infoP.ParNProcesosDC IsNot Nothing Then DirectCast(UCHeader.FindControl("txtProcesosDataClean"), TextBox).Text = infoP.ParNProcesosDC
		If infoP.ParNProcesosTopLines IsNot Nothing Then DirectCast(UCHeader.FindControl("txtProcesosToplines"), TextBox).Text = infoP.ParNProcesosTopLines
		If infoP.ParNProcesosTablas IsNot Nothing Then DirectCast(UCHeader.FindControl("txtProcesosTablas"), TextBox).Text = infoP.ParNProcesosTablas
		If infoP.ParNProcesosBases IsNot Nothing Then DirectCast(UCHeader.FindControl("txtProcesosArchivos"), TextBox).Text = infoP.ParNProcesosBases
		If infoP.DPTransformacion IsNot Nothing Then DirectCast(UCHeader.FindControl("chbDPTransformacion"), CheckBox).Checked = infoP.DPTransformacion
		If infoP.DPUnificacion IsNot Nothing Then DirectCast(UCHeader.FindControl("chbDPUnificacion"), CheckBox).Checked = infoP.DPUnificacion
		If Not infoP.DPComplejidad Is Nothing Then
			DirectCast(UCHeader.FindControl("ddlComplejidadProcesamiento"), DropDownList).SelectedValue = infoP.DPComplejidad
		Else
			DirectCast(UCHeader.FindControl("ddlComplejidadProcesamiento"), DropDownList).SelectedIndex = 1
		End If
		If Not infoP.DPComplejidadCuestionario Is Nothing Then
			DirectCast(UCHeader.FindControl("ddlComplejidadCuestionario"), DropDownList).SelectedValue = infoP.DPComplejidadCuestionario
		Else
			DirectCast(UCHeader.FindControl("ddlComplejidadCuestionario"), DropDownList).SelectedIndex = 1
		End If
		If Not infoP.DPPonderacion Is Nothing Then
			DirectCast(UCHeader.FindControl("ddlPonderacion"), DropDownList).SelectedValue = infoP.DPPonderacion
		Else
			DirectCast(UCHeader.FindControl("ddlPonderacion"), DropDownList).SelectedIndex = 0
		End If

		If infoP.DPInInterna IsNot Nothing Then DirectCast(UCHeader.FindControl("chbDPInInterna"), CheckBox).Checked = infoP.DPInInterna
		If infoP.DPInCliente IsNot Nothing Then DirectCast(UCHeader.FindControl("chbDPInCliente"), CheckBox).Checked = infoP.DPInCliente
		If infoP.DPInPanel IsNot Nothing Then DirectCast(UCHeader.FindControl("chbDPInPanel"), CheckBox).Checked = infoP.DPInPanel
		If infoP.DPInExterno IsNot Nothing Then DirectCast(UCHeader.FindControl("chbDPInExterno"), CheckBox).Checked = infoP.DPInExterno
		If infoP.DPInGMU IsNot Nothing Then DirectCast(UCHeader.FindControl("chbDPInGMU"), CheckBox).Checked = infoP.DPInGMU
		If infoP.DPInOtro IsNot Nothing Then DirectCast(UCHeader.FindControl("chbDPInOtro"), CheckBox).Checked = infoP.DPInOtro
		If infoP.DPOutCliente IsNot Nothing Then DirectCast(UCHeader.FindControl("chbDPOutCliente"), CheckBox).Checked = infoP.DPOutCliente
		If infoP.DPOutWebDelivery IsNot Nothing Then DirectCast(UCHeader.FindControl("chbDPOutWebDelivery"), CheckBox).Checked = infoP.DPOutWebDelivery
		If infoP.DPOutExterno IsNot Nothing Then DirectCast(UCHeader.FindControl("chbDPOutExterno"), CheckBox).Checked = infoP.DPOutExterno
		If infoP.DPOutGMU IsNot Nothing Then DirectCast(UCHeader.FindControl("chbDPOutGMU"), CheckBox).Checked = infoP.DPOutGMU
		If infoP.DPOutOtro IsNot Nothing Then DirectCast(UCHeader.FindControl("chbDPOutOtro"), CheckBox).Checked = infoP.DPOutOtro

		If infoP.ParPorcentajeIntercep IsNot Nothing Then DirectCast(UCHeader.FindControl("txtPorcInterceptacion"), TextBox).Text = infoP.ParPorcentajeIntercep
		If infoP.ParPorcentajeRecluta IsNot Nothing Then DirectCast(UCHeader.FindControl("txtPorcReclutamiento"), TextBox).Text = infoP.ParPorcentajeRecluta
		If infoP.ParEncuestadoresPunto IsNot Nothing Then DirectCast(UCHeader.FindControl("txtEncuestadoresPunto"), TextBox).Text = infoP.ParEncuestadoresPunto
		If infoP.PTApoyosPunto IsNot Nothing Then DirectCast(UCHeader.FindControl("txtApoyosLogisticos"), TextBox).Text = infoP.PTApoyosPunto
		If infoP.PTCompra IsNot Nothing Then DirectCast(UCHeader.FindControl("chbPTRequierecompra"), CheckBox).Checked = infoP.PTCompra
		If infoP.PTNeutralizador IsNot Nothing Then DirectCast(UCHeader.FindControl("chbPTNeutralizador"), CheckBox).Checked = infoP.PTNeutralizador
		If Not infoP.PTTipoProducto Is Nothing Then
			DirectCast(UCHeader.FindControl("ddlTipoProducto"), DropDownList).SelectedValue = infoP.PTTipoProducto
		Else
			DirectCast(UCHeader.FindControl("ddlTipoProducto"), DropDownList).SelectedValue = 1
		End If
		If infoP.PTLotes IsNot Nothing Then DirectCast(UCHeader.FindControl("txtNumLotes"), TextBox).Text = infoP.PTLotes
		If infoP.ParUnidadesProducto IsNot Nothing Then DirectCast(UCHeader.FindControl("txtNumUnidadesLote"), TextBox).Text = infoP.ParUnidadesProducto
		If infoP.ParValorUnitarioProd IsNot Nothing Then DirectCast(UCHeader.FindControl("txtValorProducto"), TextBox).Text = infoP.ParValorUnitarioProd
		If infoP.PTVisitas IsNot Nothing Then DirectCast(UCHeader.FindControl("txtVisitasRequeridas"), TextBox).Text = infoP.PTVisitas
		If infoP.PTCeldas IsNot Nothing Then DirectCast(UCHeader.FindControl("txtCeldasEvaluadas"), TextBox).Text = infoP.PTCeldas
		If infoP.PTProductosEvaluar IsNot Nothing Then DirectCast(UCHeader.FindControl("txtNumeroProductosPersona"), TextBox).Text = infoP.PTProductosEvaluar
		If infoP.ParAccesoInternet IsNot Nothing Then DirectCast(UCHeader.FindControl("chbPTAccesoInternet"), CheckBox).Checked = infoP.ParAccesoInternet
		If Not infoP.ParTipoCLT Is Nothing Then
			DirectCast(UCHeader.FindControl("ddlTipoCLT"), DropDownList).SelectedValue = infoP.ParTipoCLT
		Else
			DirectCast(UCHeader.FindControl("ddlTipoCLT"), DropDownList).SelectedIndex = 0
		End If
		If infoP.ParAlquilerEquipos IsNot Nothing Then DirectCast(UCHeader.FindControl("txtValorAlquilerEquipos"), TextBox).Text = infoP.ParAlquilerEquipos
		DirectCast(UCHeader.FindControl("gvAnalisisEstadisticos"), GridView).DataSource = ObtenerAnalisisEstadistico(infoP.IdPropuesta, infoP.ParAlternativa, infoP.MetCodigo, infoP.ParNacional)
		DirectCast(UCHeader.FindControl("gvAnalisisEstadisticos"), GridView).DataBind()
		DirectCast(UCHeader.FindControl("gvActividadesSubcontratadas"), GridView).DataSource = ObtenerActividadesSubcontratadas(infoP.IdPropuesta, infoP.ParAlternativa, infoP.MetCodigo, infoP.ParNacional)
		DirectCast(UCHeader.FindControl("gvActividadesSubcontratadas"), GridView).DataBind()
		DirectCast(UCHeader.FindControl("gvProfessionalTime"), GridView).DataSource = ObtenerHorasProfesionales(infoP.IdPropuesta, infoP.ParAlternativa, infoP.MetCodigo, infoP.ParNacional)
		DirectCast(UCHeader.FindControl("gvProfessionalTime"), GridView).DataBind()
		txtObservaciones.Text = infoP.ParObservaciones
		CargarMuestra()
		If infoP.ParRevisado = True Then
			btnGuardar.Enabled = False
		Else
			btnGuardar.Enabled = True
		End If
	End Sub

	Sub SavePresupuesto()
		If ddlFase.SelectedIndex = 0 Then
			ShowWarning(TypesWarning.ErrorMessage, "Por favor seleccione la fase antes de continuar")
			Exit Sub
		End If
		If ddlMetodologia.SelectedIndex = 0 Then
			ShowWarning(TypesWarning.ErrorMessage, "Por favor seleccione la metodología antes de continuar")
			Exit Sub
		End If
		If ddlTecnica.SelectedIndex = 0 Then
			ShowWarning(TypesWarning.ErrorMessage, "Por favor seleccione la técnica antes de continuar")
			Exit Sub
		End If
		If (ddlTecnica.SelectedValue = 100 Or ddlTecnica.SelectedValue = 200) And DirectCast(UCHeader.FindControl("ddlIncidencia"), DropDownList).SelectedIndex = 0 Then
			ShowWarning(TypesWarning.ErrorMessage, "Por favor seleccione la incidencia antes de continuar")
			Exit Sub
		End If
		Dim NewPresupuesto As Boolean = False
		Dim oCot As New CoreProject.Cotizador.General
		Dim IQP As New CoreProject.IQ_Parametros
		If oCot.GetExistsPresupuesto(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue) Then
			IQP = oCot.GetPresupuesto(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue)
		Else
			NewPresupuesto = True
			Dim oUni As New CoreProject.US.Unidades
			If oUni.ObtenerUnidadXid(DirectCast(Session("InfoJobBook"), oJobBook).IdUnidad).Codigo Is Nothing Then
				IQP.ParUnidad = 20252
			Else
				IQP.ParUnidad = oUni.ObtenerUnidadXid(DirectCast(Session("InfoJobBook"), oJobBook).IdUnidad).Codigo
			End If
			IQP.ParFechaCreacion = Date.UtcNow.AddHours(-5)
		End If
		IQP.IdPropuesta = hfPropuesta.Value
		IQP.ParAlternativa = ddlAlternativa.SelectedValue
		IQP.MetCodigo = ddlMetodologia.SelectedValue
		IQP.ParNacional = ddlFase.SelectedValue
		IQP.TipoProyecto = 1
		If txtDescripcionAlternativa.Text.Length > 300 Then txtDescripcionAlternativa.Text = txtDescripcionAlternativa.Text.Substring(0, 299)
		IQP.ParNomPresupuesto = txtDescripcionAlternativa.Text
		IQP.TecCodigo = ddlTecnica.SelectedValue
		IQP.ParTotalPreguntas = Integer.Parse(DirectCast(UCHeader.FindControl("txtCerradas"), TextBox).Text) + Integer.Parse(DirectCast(UCHeader.FindControl("txtCerradasMultiples"), TextBox).Text) + Integer.Parse(DirectCast(UCHeader.FindControl("txtAbiertas"), TextBox).Text) + Integer.Parse(DirectCast(UCHeader.FindControl("txtAbiertasMultiples"), TextBox).Text) + Integer.Parse(DirectCast(UCHeader.FindControl("txtOtros"), TextBox).Text) + Integer.Parse(DirectCast(UCHeader.FindControl("txtDemograficos"), TextBox).Text) +
		IQP.ParEncuestadoresPunto = DirectCast(UCHeader.FindControl("txtEncuestadoresPunto"), TextBox).Text
		IQP.ParTiempoEncuesta = txtDuracionMinutos.Text
		IQP.Usuario = Session("IDUsuario").ToString
		IQP.ParValorDolar = 4000
		IQP.ParNProcesosDC = DirectCast(UCHeader.FindControl("txtProcesosDataClean"), TextBox).Text
		IQP.ParNProcesosTopLines = DirectCast(UCHeader.FindControl("txtProcesosToplines"), TextBox).Text
		IQP.ParNProcesosTablas = DirectCast(UCHeader.FindControl("txtProcesosTablas"), TextBox).Text
		IQP.ParNProcesosBases = DirectCast(UCHeader.FindControl("txtProcesosArchivos"), TextBox).Text
		IQP.ParGrupoObjetivo = DirectCast(UCHeader.FindControl("txtGrupoObjetivo"), TextBox).Text
		IQP.ParIncidencia = DirectCast(UCHeader.FindControl("ddlIncidencia"), DropDownList).SelectedValue

		IQP.ParProbabilistico = DirectCast(UCHeader.FindControl("chbProbabilistico"), CheckBox).Checked
		IQP.ParPorcentajeIntercep = DirectCast(UCHeader.FindControl("txtPorcInterceptacion"), TextBox).Text
		IQP.ParPorcentajeRecluta = DirectCast(UCHeader.FindControl("txtPorcReclutamiento"), TextBox).Text
		IQP.ParUnidadesProducto = DirectCast(UCHeader.FindControl("txtNumUnidadesLote"), TextBox).Text
		IQP.ParValorUnitarioProd = DirectCast(UCHeader.FindControl("txtValorProducto"), TextBox).Text
		IQP.ParTipoCLT = DirectCast(UCHeader.FindControl("ddlTipoCLT"), DropDownList).SelectedValue
		IQP.ParAlquilerEquipos = DirectCast(UCHeader.FindControl("txtValorAlquilerEquipos"), TextBox).Text
		IQP.ParAccesoInternet = DirectCast(UCHeader.FindControl("chbPTAccesoInternet"), CheckBox).Checked
		IQP.ParObservaciones = txtObservaciones.Text
		IQP.ParUsaTablet = 1
		IQP.ParUsaPapel = 0
		IQP.ParAñoSiguiente = 1
		IQP.Complejidad = ddlComplejidad.SelectedValue
		IQP.F2FVirtual = DirectCast(UCHeader.FindControl("chbF2fVirtual"), CheckBox).Checked
		IQP.ComplejidadCodificacion = DirectCast(UCHeader.FindControl("ddlComplejidadCodificación"), DropDownList).SelectedValue
		IQP.DPTransformacion = DirectCast(UCHeader.FindControl("chbDPTransformacion"), CheckBox).Checked
		IQP.DPUnificacion = DirectCast(UCHeader.FindControl("chbDPUnificacion"), CheckBox).Checked
		IQP.DPComplejidad = DirectCast(UCHeader.FindControl("ddlComplejidadProcesamiento"), DropDownList).SelectedValue
		IQP.DPPonderacion = DirectCast(UCHeader.FindControl("ddlPonderacion"), DropDownList).SelectedValue
		IQP.DPInInterna = DirectCast(UCHeader.FindControl("chbDPInInterna"), CheckBox).Checked
		IQP.DPInCliente = DirectCast(UCHeader.FindControl("chbDPInCliente"), CheckBox).Checked
		IQP.DPInPanel = DirectCast(UCHeader.FindControl("chbDPInPanel"), CheckBox).Checked
		IQP.DPInExterno = DirectCast(UCHeader.FindControl("chbDPInExterno"), CheckBox).Checked
		IQP.DPInGMU = DirectCast(UCHeader.FindControl("chbDPInGMU"), CheckBox).Checked
		IQP.DPInOtro = DirectCast(UCHeader.FindControl("chbDPInOtro"), CheckBox).Checked
		IQP.DPOutCliente = DirectCast(UCHeader.FindControl("chbDPOutCliente"), CheckBox).Checked
		IQP.DPOutWebDelivery = DirectCast(UCHeader.FindControl("chbDPOutWebDelivery"), CheckBox).Checked
		IQP.DPOutExterno = DirectCast(UCHeader.FindControl("chbDPOutExterno"), CheckBox).Checked
		IQP.DPOutGMU = DirectCast(UCHeader.FindControl("chbDPOutGMU"), CheckBox).Checked
		IQP.DPOutOtro = DirectCast(UCHeader.FindControl("chbDPOutOtro"), CheckBox).Checked
		IQP.PTApoyosPunto = DirectCast(UCHeader.FindControl("txtApoyosLogisticos"), TextBox).Text
		IQP.PTCompra = DirectCast(UCHeader.FindControl("chbPTRequierecompra"), CheckBox).Checked
		IQP.PTNeutralizador = DirectCast(UCHeader.FindControl("chbPTNeutralizador"), CheckBox).Checked
		IQP.PTTipoProducto = DirectCast(UCHeader.FindControl("ddlTipoProducto"), DropDownList).SelectedValue
		IQP.PTLotes = DirectCast(UCHeader.FindControl("txtNumLotes"), TextBox).Text
		IQP.PTVisitas = DirectCast(UCHeader.FindControl("txtVisitasRequeridas"), TextBox).Text
		IQP.PTCeldas = DirectCast(UCHeader.FindControl("txtCeldasEvaluadas"), TextBox).Text
		IQP.PTProductosEvaluar = DirectCast(UCHeader.FindControl("txtNumeroProductosPersona"), TextBox).Text
		IQP.DPComplejidadCuestionario = DirectCast(UCHeader.FindControl("ddlComplejidadCuestionario"), DropDownList).SelectedValue
		If DirectCast(UCHeader.FindControl("txtProductividad"), TextBox).Text = "" Then
			DirectCast(UCHeader.FindControl("txtProductividad"), TextBox).Text = "0"
		Else
			IQP.ParProductividad = DirectCast(UCHeader.FindControl("txtProductividad"), TextBox).Text
		End If
		oCot.PutSaveParametros(IQP, NewPresupuesto)
		If (IQP.ParRevisado) Is Nothing Or (IQP.ParRevisado = False) Then
			Dim tempoDias As Integer = txtDiasCampo.Text
			txtDiasCampo.Text = oCot.GetCalculoDiasCampo(hfPropuesta.Value, ddlAlternativa.SelectedValue)
			If Not (tempoDias = CInt(txtDiasCampo.Text)) Then
				UPanelGeneral.UpdateMode = UpdatePanelUpdateMode.Conditional
				UPanelGeneral.Update()
				UPanelGeneral.UpdateMode = UpdatePanelUpdateMode.Always
				SaveGeneralValues()
			End If
		End If
		SavePreguntas()
		SaveProcesos(IQP)
		SaveActSubcontratadas()
		SaveActEstadistica()
		SaveHorasProfesionales()
		If oCot.GetTotalMuestra(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue) > 0 Then
			EfectuarCalculos()
		End If
		Dim o As New Presupuesto
		Dim oCUP = o.Cu_PresupestoGetByPropAndAlt(IQP.IdPropuesta, IQP.ParAlternativa)
		If Not (oCUP.ParaRevisar = True) Then
			btnRevision.Visible = True
		End If
	End Sub

	Sub SavePreguntas()
		Dim IQPR As New CoreProject.IQ_Preguntas
		IQPR.IdPropuesta = hfPropuesta.Value
		IQPR.ParAlternativa = ddlAlternativa.SelectedValue
		IQPR.MetCodigo = ddlMetodologia.SelectedValue
		IQPR.ParNacional = ddlFase.SelectedValue
		IQPR.PregCerradas = Integer.Parse(DirectCast(UCHeader.FindControl("txtCerradas"), TextBox).Text)
		IQPR.PregCerradasMultiples = Integer.Parse(DirectCast(UCHeader.FindControl("txtCerradasMultiples"), TextBox).Text)
		IQPR.PregAbiertas = Integer.Parse(DirectCast(UCHeader.FindControl("txtAbiertas"), TextBox).Text)
		IQPR.PregAbiertasMultiples = Integer.Parse(DirectCast(UCHeader.FindControl("txtAbiertasMultiples"), TextBox).Text)
		IQPR.PregOtras = Integer.Parse(DirectCast(UCHeader.FindControl("txtOtros"), TextBox).Text)
		IQPR.PregDemograficos = Integer.Parse(DirectCast(UCHeader.FindControl("txtDemograficos"), TextBox).Text)
		Dim oCot As New CoreProject.Cotizador.General
		oCot.PutPreguntas(IQPR)
	End Sub

	Sub SaveProcesos(ByRef IQP As IQ_Parametros)
		Dim IQPRList As New List(Of CoreProject.IQ_ProcesosPresupuesto)

		If DirectCast(UCHeader.FindControl("chbProcessCampo"), CheckBox).Checked = True Then IQPRList.Add(NewProceso(1))
		If DirectCast(UCHeader.FindControl("chbProcessVerificacion"), CheckBox).Checked = True Then IQPRList.Add(NewProceso(2))
		If DirectCast(UCHeader.FindControl("chbProcessCritica"), CheckBox).Checked = True Then IQPRList.Add(NewProceso(3))
		If DirectCast(UCHeader.FindControl("chbProcessCodificacion"), CheckBox).Checked = True Then IQPRList.Add(NewProceso(4))
		If DirectCast(UCHeader.FindControl("chbProcessDataClean"), CheckBox).Checked = True Then IQPRList.Add(NewProceso(6))
		If DirectCast(UCHeader.FindControl("chbProcessTopLines"), CheckBox).Checked = True Then IQPRList.Add(NewProceso(7))
		If DirectCast(UCHeader.FindControl("chbProcessProceso"), CheckBox).Checked = True Then IQPRList.Add(NewProceso(8))
		If DirectCast(UCHeader.FindControl("chbProcessArchivos"), CheckBox).Checked = True Then
			IQPRList.Add(NewProceso(9))
		End If
		If DirectCast(UCHeader.FindControl("chbProcessScripting"), CheckBox).Checked = True Then IQPRList.Add(NewProceso(10))
		Dim oCot As New CoreProject.Cotizador.General
		oCot.PutProcesos(IQPRList, IQP)
	End Sub

	Sub SaveMuestra(ByVal Muestra As IQ_Muestra_1)
		If ValidateSavePresupuesto() = False Then Exit Sub
		Dim oCot As New CoreProject.Cotizador.General
		Dim flag As Boolean = False
		If oCot.GetExistsPresupuesto(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue) = False Then
			SavePresupuesto()
			flag = True
		End If
		oCot.PutMuestra(Muestra)
		CargarMuestra()
		If flag = True Then
			SavePresupuesto()
		End If
	End Sub

	Sub CargarMuestra()
		gvMuestraCATI.Visible = False
		gvMuestraF2F.Visible = False
		gvMuestraOnline.Visible = False
		If ddlTecnica.SelectedValue = 100 And Not (ddlMetodologia.SelectedValue = 140 Or ddlMetodologia.SelectedValue = 130 Or ddlMetodologia.SelectedValue = 160) Then MuestraF2F()
		If ddlTecnica.SelectedValue = 100 And (ddlMetodologia.SelectedValue = 140 Or ddlMetodologia.SelectedValue = 130 Or ddlMetodologia.SelectedValue = 160) Then MuestraOnline()
		If ddlTecnica.SelectedValue = 200 Then MuestraCATI()
		If ddlTecnica.SelectedValue = 300 Then MuestraOnline()
	End Sub

	Private Sub MuestraF2F()
		Dim oCot As New CoreProject.Cotizador.General
		gvMuestraF2F.DataSource = oCot.GetMuestraF2F(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue)
		gvMuestraF2F.DataBind()
		gvMuestraF2F.Visible = True
		lblTotalMuestra.Text = oCot.GetTotalMuestra(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue)
	End Sub

	Private Sub MuestraCATI()
		Dim oCot As New CoreProject.Cotizador.General
		gvMuestraCATI.DataSource = oCot.GetMuestraCati(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue)
		gvMuestraCATI.DataBind()
		gvMuestraCATI.Visible = True
		lblTotalMuestra.Text = oCot.GetTotalMuestra(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue)
	End Sub

	Private Sub MuestraOnline()
		Dim oCot As New CoreProject.Cotizador.General
		gvMuestraOnline.DataSource = oCot.GetMuestraOnline(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue)
		gvMuestraOnline.DataBind()
		gvMuestraOnline.Visible = True
		lblTotalMuestra.Text = oCot.GetTotalMuestra(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue)
	End Sub

	Private Function NewProceso(ByVal Proceso As Integer) As IQ_ProcesosPresupuesto
		Dim IQPR As New CoreProject.IQ_ProcesosPresupuesto
		IQPR.IdPropuesta = hfPropuesta.Value
		IQPR.ParAlternativa = ddlAlternativa.SelectedValue
		IQPR.MetCodigo = ddlMetodologia.SelectedValue
		IQPR.ParNacional = ddlFase.SelectedValue
		IQPR.ProcCodigo = Proceso
		IQPR.Porcentaje = 0
		Return IQPR
	End Function

	Function ValidateSavePresupuesto() As Boolean
		ValidarDatosNumericosPresupuesto()
		If ValidateSaveGeneralPresupuesto() = False Then Return False
		Return True
	End Function

	Function ValidateSaveGeneralPresupuesto() As Boolean

		If ddlTecnica.SelectedValue = "0" Then
			ShowWarning(TypesWarning.ErrorMessage, "Debe seleccionar la Técnica")
			ddlTecnica.Focus()
			Return False
		End If
		If ddlMetodologia.SelectedValue = "0" Then
			ShowWarning(TypesWarning.ErrorMessage, "Debe seleccionar la Metodologia")
			ddlMetodologia.Focus()
			Return False
		End If
		If ddlFase.SelectedValue = "0" Then
			ShowWarning(TypesWarning.ErrorMessage, "Debe seleccionar la Fase")
			ddlFase.Focus()
			Return False
		End If
		If Not (IsNumeric(txtDuracionMinutos.Text)) Then
			ShowWarning(TypesWarning.ErrorMessage, "Por favor escriba la duración del cuestionario")
			txtDuracionMinutos.Focus()
			Return False
		End If
		If DirectCast(UCHeader.FindControl("txtGrupoObjetivo"), TextBox).Text.Length < 3 Then
			ShowWarning(TypesWarning.ErrorMessage, "Describa el grupo objetivo")
			DirectCast(UCHeader.FindControl("txtGrupoObjetivo"), TextBox).Focus()
			Return False
		End If

		If (Integer.Parse(DirectCast(UCHeader.FindControl("txtCerradas"), TextBox).Text) + Integer.Parse(DirectCast(UCHeader.FindControl("txtCerradasMultiples"), TextBox).Text) + Integer.Parse(DirectCast(UCHeader.FindControl("txtAbiertas"), TextBox).Text) + Integer.Parse(DirectCast(UCHeader.FindControl("txtAbiertasMultiples"), TextBox).Text) + Integer.Parse(DirectCast(UCHeader.FindControl("txtOtros"), TextBox).Text)) = 0 Then
			ShowWarning(TypesWarning.ErrorMessage, "Diligencie la cantidad de preguntas del cuestionario")
			Return False
		End If
		If DirectCast(UCHeader.FindControl("ddlIncidencia"), DropDownList).Enabled = True And DirectCast(UCHeader.FindControl("ddlIncidencia"), DropDownList).SelectedIndex = 0 Then
			ShowWarning(TypesWarning.ErrorMessage, "Debe seleccionar la incidencia")
			Return False
		End If
		If Integer.Parse(DirectCast(UCHeader.FindControl("txtProcesosDataClean"), TextBox).Text) + Integer.Parse(DirectCast(UCHeader.FindControl("txtProcesosToplines"), TextBox).Text) + Integer.Parse(DirectCast(UCHeader.FindControl("txtProcesosTablas"), TextBox).Text) + Integer.Parse(DirectCast(UCHeader.FindControl("txtProcesosArchivos"), TextBox).Text) Then
			ShowWarning(TypesWarning.Warning, "No ha seleccionado ningún número de procesos para DP. Esto podría ser un error")
		End If
		Return True
	End Function

	Sub ValidarDatosNumericosPresupuesto()
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtCerradas"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtCerradas"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtCerradasMultiples"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtCerradasMultiples"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtAbiertas"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtAbiertas"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtAbiertasMultiples"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtAbiertasMultiples"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtOtros"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtOtros"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtDemograficos"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtDemograficos"), TextBox).Text = "0"

		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtProcesosDataClean"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtProcesosDataClean"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtProcesosToplines"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtProcesosToplines"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtProcesosTablas"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtProcesosTablas"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtProcesosArchivos"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtProcesosArchivos"), TextBox).Text = "0"

		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtPorcInterceptacion"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtPorcInterceptacion"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtPorcReclutamiento"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtPorcReclutamiento"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtEncuestadoresPunto"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtEncuestadoresPunto"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtApoyosLogisticos"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtApoyosLogisticos"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtNumLotes"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtNumLotes"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtNumUnidadesLote"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtNumUnidadesLote"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtValorProducto"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtValorProducto"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtVisitasRequeridas"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtVisitasRequeridas"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtCeldasEvaluadas"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtCeldasEvaluadas"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtNumeroProductosPersona"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtNumeroProductosPersona"), TextBox).Text = "0"
		If Not (IsNumeric(DirectCast(UCHeader.FindControl("txtValorAlquilerEquipos"), TextBox).Text)) Then DirectCast(UCHeader.FindControl("txtValorAlquilerEquipos"), TextBox).Text = "0"

	End Sub

	Sub EfectuarCalculos()
		Dim oCot As New CoreProject.Cotizador.General
		If DirectCast(UCHeader.FindControl("txtProductividad"), TextBox).Text = "" Or DirectCast(UCHeader.FindControl("txtProductividad"), TextBox).Text = "0" Or (oCot.GetPresupuesto(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue).ParProductividadOriginal Is Nothing) Then DirectCast(UCHeader.FindControl("txtProductividad"), TextBox).Text = oCot.GetCalculoProductividad(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue, ddlTecnica.SelectedValue)
		oCot.PutValorVenta(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue, ddlTecnica.SelectedValue, DirectCast(Session("InfoJobBook"), oJobBook).IdUnidad)
		Dim infoOP = oCot.GetSimulador(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue, 1, Nothing, Nothing, Nothing, Nothing).PercentOp
		oCot.PutOP(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue, infoOP)
	End Sub

	Sub SaveActSubcontratadas()
		Dim lActividades = UCHeader.ActividadesSubcontratadas(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue)
		Dim oCot As New CoreProject.Cotizador.General
		oCot.PutActividadesSubcontratadas(lActividades)
	End Sub

	Sub SaveActEstadistica()
		Dim lActividades = UCHeader.AnalisisEstadisticos(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue)
		Dim oCot As New CoreProject.Cotizador.General
		oCot.PutModelosEstadistica(lActividades)
	End Sub

	Sub SaveHorasProfesionales()
		Dim lActividades = UCHeader.HorasProfesionales(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue)
		Dim oCot As New CoreProject.Cotizador.General
		oCot.PutHorasProfesionales(lActividades)
	End Sub

	Sub EnviarEmailRevisionPresupuestos(propuestaId As Int64, alternativas As List(Of Integer))
		Dim oEnviarCorreo As New EnviarCorreo
		Try
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/PresupuestosParaRevisar.aspx?propuestaId=" & propuestaId.ToString & "&alternativas=" & String.Join(",", alternativas))
		Catch ex As Exception
		End Try
	End Sub

	Sub EnviarEmailSolicitudSimulador(propuestaId As Int64, idsolicitud As Int64)
		Dim oEnviarCorreo As New EnviarCorreo
		Try
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/SolicitudAjustesSimulador.aspx?idPropuesta=" & propuestaId.ToString & "&idSolicitud=" & idsolicitud)
		Catch ex As Exception
		End Try
	End Sub

	Sub EnvioPresupuestoRevision(propuestaid As Int64, alternativa As Integer)
		Dim o As New Presupuesto
		Dim log As New LogEjecucion
		Dim lstAlternativas As New List(Of Integer)
		lstAlternativas.Add(alternativa)
		Dim oCUP = o.Cu_PresupestoGetByPropAndAlt(propuestaid, alternativa)
		o.editarEnvio(oCUP.Id, True)
		log.Guardar(36, oCUP.Id, Now(), Session("IDUsuario"), 6)
		EnviarEmailRevisionPresupuestos(propuestaid, lstAlternativas)
		ShowWarning(TypesWarning.Information, "Ha sido enviado el presupuesto para revisión")
		'Dim oCot As New CoreProject.Cotizador.General
		'Dim lAlt = oCot.GetAllPresupuestosByAlternativa(propuestaid, alternativa)
		'If lAlt.Any(Function(x) x.TecCodigo = 600 Or x.TecCodigo = 700 Or x.TecCodigo = 800) Then
		'	For Each presup In lAlt
		'		Dim IQP = oCot.GetPresupuesto(hfPropuesta.Value, ddlAlternativa.SelectedValue, presup.MetCodigo, presup.ParNacional)
		'		IQP.ParFechaRevision = Date.UtcNow.AddHours(-5)
		'		IQP.ParRevisado = True
		'		IQP.ParRevisadoPor = Session("IDUsuario").ToString
		'		oCot.PutSaveParametros(IQP, False)
		'	Next
		'End If

		btnRevision.Visible = False

	End Sub

	Private Sub CargarJBI(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer, gmu As Decimal, gmo As Decimal, simular As Boolean)
		Dim oCot As New CoreProject.Cotizador.General
		GVJBI.DataSource = oCot.GetCostosJBI(idPropuesta, Alternativa, Metodologia, Fase, gmu, gmo, simular)
		GVJBI.DataBind()
	End Sub

	Private Sub CargarJBE(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer, gmu As Decimal, gmo As Decimal, simular As Boolean)
		Dim oCot As New CoreProject.Cotizador.General
		GVJBE.DataSource = oCot.GetCostosJBE(idPropuesta, Alternativa, Metodologia, Fase, gmu, gmo, simular)
		GVJBE.DataBind()
	End Sub

	Private Function UInitialCase(ByVal textToChange As String) As String
		Dim input As String = textToChange
		Dim pattern As String = "\b(\w|['-])+\b"
		' With lambda support:
		Dim result As String = Regex.Replace(input, pattern,
			Function(m) m.Value(0).ToString().ToUpper() & m.Value.Substring(1))
		Return result
	End Function


	'TipoCalculo (1: cambo de gross margin, 2: cambio de gm de operaciones y de unidad 
	Private Function AjustarGrossMargin(ByVal TipoCalculo As Integer) As Boolean
		Dim valido As Boolean = True
		Dim oCot As New CoreProject.Cotizador.General
		ModalPopupExtenderGM.Show()
		Dim GM As String = ""
		Dim valorLimiteAprobacionGerente, gmCalculado, gmOpe, gmUni As Decimal
		Dim par As IQ_Parametros
		Dim GMOPS As Double = oCot.GetParametrosGenerales(551)


		par = ObtenerPresupuesto(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value)
		valorLimiteAprobacionGerente = 2100 * oCot.GetValorTotalXalternativa(hfPropuesta.Value, ddlAlternativa.SelectedValue)
		Dim ValorVenta As Double? = par.ParValorVenta
		hfTope.Value = oCot.GetDatosPropuesta(hfPropuesta.Value).TOPE * 100
		'UPanelGM.Update()
		Dim valTope As Double = hfTope.Value
		If ValorVenta > 150000000 And ValorVenta < 350000000 Then valTope = valTope + 1
		If ValorVenta > 50000000 And ValorVenta <= 150000000 Then valTope = valTope + 2.5
		If ValorVenta <= 50000000 Then valTope = valTope + 5


		If Request.QueryString("GMU") IsNot Nothing Then
			GM = Request.QueryString("GMU")
		End If

		If ((txtValorVentaSimular.Text <> "") And TipoCalculo = 1) Or ((txtNuevoGM.Text <> "" Or txtGMOpera.Text <> "") And TipoCalculo = 2) Then
			'1. debemos valdar que e sun valor numerico
			'2. Cuand se aumenta el valor se debe permitir
			'3. cuando se disminuye el valor se debe pedir la clave para disminuirlo.
			'el tipo de calculo indica  desde que lugar se ejecuta el ambio de gross, 1. un presupuesto, 2. general
			If hfTipoCalculo.Value = "1" Then


				If TipoCalculo = 1 Then
					par.ParGrossMargin = oCot.GetSimularGM(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, CDec(txtValorVentaSimular.Text), 1) * 100
				ElseIf TipoCalculo = 2 Then
					par.ParGrossMargin = If((txtNuevoGM.Text.Trim = String.Empty), CDec(lblgmActual.Text), CDec(txtNuevoGM.Text))
				End If
				' par.ParGrossMargin = CDbl(txtNuevoGM.Text)
				Dim flag As Boolean = False
				If IsNumeric(txtGMOpera.Text) Then
					If (CDec(txtGMOpera.Text) / 100) < GMOPS Then
						flag = True
					End If
				End If
				If (par.ParGrossMargin < CDbl(valTope)) Or flag = True Then
					'ENVIAMOS UNA NOTIFICACION POR CORREO A LA PERSONA AUTORIZADA A EFECTUAR ESTA OPERACION , IGUALMENTE DESPLEGAMOS  UNA NOTIFICACION 
					'INFORMANDO AL USUARIO QUE YA SE ENVIO  EL CORREO A LA PERSONA INDICADA , SE DEBE ANEXAR EN EL CORREO EL TIPO DE CALCULO A EFECTUAR (1,2)
					'ASI COMO LOS RESPECTIVOS DATOS DEL PRESUPUESTO, PARA MODIFICAR UN PRESUPUESTO UNICO SE DEBEN ENVIAR TODOS LOS DATOS DE (IDPROPUESTA, ALTERVATIVA, NACIONALES,METODOLOGIA)
					'PARA EL CASO 2 , SE DEBE ENVIAR UNICAMENTE IDPROPUESTA Y ALTERNATIVA PUES SE MODIFICAN TODOS LOS PRESUPUESTOS INVOLUCRADOS EN LA ALTERNATIVA

					If gmTxtContrasena.Visible = False Then
						gmTxtContrasena.Visible = True
						lblContrasena.Visible = True
						pnNotificacion.Visible = True
						valido = False
						ShowWarning(TypesWarning.ErrorMessage, "DIGITE EL PASSWORD AUTORIZADO.")

					Else
						Dim cla = New US.Usuarios
						Dim password As String = ""
						password = Cifrado(1, 2, gmTxtContrasena.Text, "Ipsos*23432_2013", "Ipsos*23432_2013")

						Dim UsuAutorizado As Boolean = oCot.GetValidarUsuarioAutorizado(par.ParUnidad, CDec(Session("IDUsuario")))
						Dim resul As Long = cla.VerificarLoginXId(CDec(Session("IDUsuario")), password)

						If gmTxtContrasena.Text <> "" And Not (resul = -1) And UsuAutorizado = True Then

							If TipoCalculo = 1 Then
								gmCalculado = oCot.GetSimularGM(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, CDec(txtValorVentaSimular.Text), 0)
								CargarJBI(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, -1, gmCalculado, False)
								CargarJBE(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, -1, gmCalculado, False)

							ElseIf TipoCalculo = 2 Then
								If txtGMOpera.Text = String.Empty Then
									gmOpe = -1
								Else
									gmOpe = CDec(txtGMOpera.Text) / 100
								End If
								If txtNuevoGM.Text = String.Empty Then
									gmUni = -1
								Else
									gmUni = CDec(txtNuevoGM.Text) / 100
								End If
								GM = oCot.GetSimularVenta(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, gmUni, gmOpe, 0)
								CargarJBI(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, gmOpe, gmUni, False)
								CargarJBE(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, gmOpe, gmUni, False)
							End If

							CargarPresupuestos(ddlAlternativa.SelectedValue)
							UpdatePanel2.Update()
							' lkgm_ModalPopupExtender.Hide()
						Else
							valido = False
							ShowWarning(TypesWarning.ErrorMessage, "LA CONTRASENA NO COINCIDE, O SU USUARIO NO ESTA AUTORIZADO PARA REALIZAR ESTA TRANSACCION!!")
						End If
					End If

				Else
					gmTxtContrasena.Visible = False
					gmTxtContrasena.Visible = False
					lblContrasena.Visible = False
					pnNotificacion.Visible = False

					If TipoCalculo = 1 Then
						gmCalculado = oCot.GetSimularGM(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, CDec(txtValorVentaSimular.Text), 0)
						CargarJBI(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, -1, gmCalculado, False)
						CargarJBE(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, -1, gmCalculado, False)
					ElseIf TipoCalculo = 2 Then
						If txtGMOpera.Text = String.Empty Then
							gmOpe = -1
						Else
							gmOpe = CDec(txtGMOpera.Text) / 100
						End If

						If txtNuevoGM.Text = String.Empty Then
							gmUni = -1
						Else
							gmUni = CDec(txtNuevoGM.Text) / 100
						End If
						gmCalculado = oCot.GetSimularVenta(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, gmUni, gmOpe, 0)
						CargarJBI(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, gmOpe, gmUni, False)
						CargarJBE(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigo.Value, hfFase.Value, gmOpe, gmUni, False)
					End If
					CargarPresupuestos(ddlAlternativa.SelectedValue)
					UpdatePanel2.Update()
					'lkgm_ModalPopupExtender.Hide()
				End If

			ElseIf hfTipoCalculo.Value = "2" Then
				'1. se deben filtrar de a tabal de parametrso todos los presupuestos  de una aternativa
				'2. se debe actualizar e gross amrgin de todos estos presupuestos
				'3. refrescar  la grilla  princial
				If CDbl(txtNuevoGM.Text) < CDbl(valTope) Then

					If gmTxtContrasena.Visible = False Then
						gmTxtContrasena.Visible = True
						lblContrasena.Visible = True
						pnNotificacion.Visible = True
						valido = False
						ShowWarning(TypesWarning.ErrorMessage, "DIGITE EL PASSWORD AUTORIZADO.")
					Else
						Dim cla = New US.Usuarios
						Dim password As String = ""
						password = Cifrado(1, 2, gmTxtContrasena.Text, "Ipsos*23432_2013", "Ipsos*23432_2013")

						Dim UsuAutorizado As Boolean = oCot.GetValidarUsuarioAutorizado(CType(Session("PARAMETROS"), IQ_Parametros).ParUnidad, CDec(Session("IDUsuario")))
						Dim resul As Long = cla.VerificarLoginXId(CDec(Session("IDUsuario")), password)

						If gmTxtContrasena.Text <> "" And Not (resul = -1) And UsuAutorizado = True Then
							Dim lst As List(Of IQ_Parametros)
							lst = oCot.GetPresupuestosModificarGM(ddlAlternativa.SelectedValue, hfPropuesta.Value)
							If TipoCalculo = 1 Then
								oCot.PutCalcularGrossMarginXAlternativa(lst, CDec(txtNuevoGM.Text), TipoCalculo, CDec(txtValorVentaSimular.Text), 0, 0)
							ElseIf TipoCalculo = 2 Then
								If txtGMOpera.Text = String.Empty Then
									gmOpe = -1
								Else
									gmOpe = CDec(txtGMOpera.Text)
								End If
								oCot.PutCalcularGrossMarginXAlternativa(lst, CDec(txtNuevoGM.Text), TipoCalculo, CDec(txtValorVentaSimular.Text), CDec(txtNuevoGM.Text), gmOpe)
							End If
							CargarPresupuestos(hfNewAlternativa.Value)
						Else
							valido = False
							ShowWarning(TypesWarning.ErrorMessage, "LA CONTRASENA NO COINCIDE, O SU USUARIO NO ESTA AUTRIZADO PARA REALIZAR ESTA TRANSACCION!!!!")
						End If
					End If

				Else
					gmTxtContrasena.Visible = False
					gmTxtContrasena.Visible = False
					lblContrasena.Visible = False
					pnNotificacion.Visible = False

					Dim lst As List(Of IQ_Parametros)
					lst = oCot.GetPresupuestosModificarGM(ddlAlternativa.SelectedValue, hfPropuesta.Value)
					If TipoCalculo = 1 Then
						oCot.PutCalcularGrossMarginXAlternativa(lst, CDec(txtNuevoGM.Text), TipoCalculo, CDec(txtValorVentaSimular.Text), 0, 0)
					ElseIf TipoCalculo = 2 Then
						If txtGMOpera.Text = String.Empty Then
							gmOpe = -1
						Else
							gmOpe = CDec(txtGMOpera.Text)
						End If
						oCot.PutCalcularGrossMarginXAlternativa(lst, CDec(txtNuevoGM.Text), TipoCalculo, CDec(txtValorVentaSimular.Text), CDec(txtNuevoGM.Text), gmOpe)
					End If

					CargarPresupuestos(ddlAlternativa.SelectedValue)
					ModalPopupExtenderGM.Show()
				End If
			End If

		End If

	End Function

	Protected Sub btnModificarGM_1_Click(sender As Object, e As EventArgs)
		AjustarGrossMargin(1)
	End Sub

	Protected Sub btnModificarGM_2_Click(sender As Object, e As EventArgs)
		AjustarGrossMargin(2)
	End Sub

	Protected Sub EnviarNotificacion0_Click(sender As Object, e As EventArgs)
		Dim oUni As New CoreProject.US.Unidades
		Dim unidad As Integer
		If oUni.ObtenerUnidadXid(DirectCast(Session("InfoJobBook"), oJobBook).IdUnidad).Codigo Is Nothing Then
			unidad = 20252
		Else
			unidad = oUni.ObtenerUnidadXid(DirectCast(Session("InfoJobBook"), oJobBook).IdUnidad).Codigo
		End If
		Dim oEnviarCorreo As New EnviarCorreo
		If hfTipoCalculo.Value = "1" Then
			'ENVIAR CORREO DE NOTIFICACION GM EN ESTE PUNTO , EL LINK DEBE TENER LOS SIGUIENTES DATOS 
			Dim strRedireccionar1 As String
			strRedireccionar1 = "/CAP/Cap_Principal.aspx?IdPropuesta=" & hfPropuesta.Value & "&Alternativa=" & ddlAlternativa.SelectedValue & "&Fase" & hfFase.Value & "&Metodologia=" & hfMetCodigo.Value & "&GMU=" & txtNuevoGM.Text & "&TipoCalculo=" & hfTipoCalculo.Value


			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/SolicitudCambioGrossMargin.aspx?IdPropuesta=" & hfPropuesta.Value & "&Alternativa=" & ddlAlternativa.SelectedValue & "&Metodologia=" & hfMetCodigo.Value & "&GMU=" & txtNuevoGM.Text & "&TipoCalculo=" & hfTipoCalculo.Value & "&Fase=" & hfFase.Value & "&Unidad=" & unidad & "&GMO=" & txtGMOpera.Text)

			ModalPopupExtenderGM.Show()
			ShowWarning(TypesWarning.Warning, "Solicitud de cambio  enviada  ")
		ElseIf hfTipoCalculo.Value = "2" Then
			'ENVIAR CORREO DE NOTIFICACION GM EN ESTE PUNTO , EL LINK DEBE TENER LOS SIGUIENTES DATOS 
			Dim strRedireccionar2 As String
			strRedireccionar2 = "/CAP/Cap_Principal.aspx?IdPropuesta=" & hfPropuesta.Value & "&Alternativa=" & ddlAlternativa.SelectedValue & "&GMU=" & txtNuevoGM.Text & "&TipoCalculo=" & hfTipoCalculo.Value & "&GMO=" & txtGMOpera.Text


			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/SolicitudCambioGrossMargin.aspx?IdPropuesta=" & hfPropuesta.Value & "&Alternativa=" & ddlAlternativa.SelectedValue & "&GMU=" & txtNuevoGM.Text & "&TipoCalculo=" & hfTipoCalculo.Value & "&Unidad=" & unidad & "&GMO=" & txtGMOpera.Text)

			ModalPopupExtenderGM.Show()
			ShowWarning(TypesWarning.Warning, "Solicitud de cambio enviada ! ")
		End If
	End Sub

	Protected Sub btnCancelGM_Click(sender As Object, e As EventArgs)
		ModalPopupExtenderGM.Hide()
	End Sub

#End Region

#Region "JBE / JBI"
	Private Sub CargarCostosJBE(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer)
		Dim oCot As New CoreProject.Cotizador.General
		If chbObserver.Checked = True Then
			gvJBExterno.DataSource = oCot.GetCostosJobBookExternoObserver(idPropuesta, Alternativa, Metodologia, Fase).Tables(0)
		Else
			gvJBExterno.DataSource = oCot.GetCostosJobBookExterno(idPropuesta, Alternativa, Metodologia, Fase).Tables(0)
		End If
		gvJBExterno.DataSource = oCot.GetCostosJobBookExterno(idPropuesta, Alternativa, Metodologia, Fase).Tables(0)
		gvJBExterno.DataBind()
	End Sub

	Private Sub CargarCostosJBI(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer)
		Dim oCot As New CoreProject.Cotizador.General
		gvJBInterno.DataSource = oCot.GetCostosJobBookInterno(idPropuesta, Alternativa, Metodologia, Fase).Tables(0)
		gvJBInterno.DataBind()
	End Sub

	Sub CargarDetalleCostos(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer)
		Dim oCot As New CoreProject.Cotizador.General
		gvControlCostos.DataSource = oCot.GetCostos(idPropuesta, Alternativa, Metodologia, Fase, 1)
		gvControlCostos.DataBind()

		gvDetallesOperaciones.DataSource = oCot.GetCostos(idPropuesta, Alternativa, Metodologia, Fase, 2)
		gvDetallesOperaciones.DataBind()

		gvViaticos.DataSource = oCot.GetViaticos(idPropuesta, Alternativa, Metodologia, Fase)
		gvViaticos.DataBind()

		gvPYGPresupuesto.DataSource = oCot.GetPyG(idPropuesta, Alternativa, Metodologia, Fase)
		gvPYGPresupuesto.DataBind()

		gvPYGAlternativa.DataSource = oCot.GetPyG(idPropuesta, Alternativa, Nothing, Nothing)
		gvPYGAlternativa.DataBind()

		If hfOPS.Value = 1 Then
			TabPanel2.Visible = True
		Else
			TabPanel2.Visible = False
		End If
	End Sub
	Protected Sub gvJBExterno_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvJBExterno.RowDataBound
		If e.Row.RowType = DataControlRowType.DataRow Then
			_Total = _Total + CDec(e.Row.Cells(3).Text)

			If e.Row.Cells(2).Text.ToString.IndexOf("PORCENTAJE") > -1 Then
				e.Row.Cells(3).Text = CDec(e.Row.Cells(3).Text).ToString("P")
			Else
				e.Row.Cells(3).Text = CDec(e.Row.Cells(3).Text).ToString("C0")
			End If

			If e.Row.Cells(2).Text.ToString.IndexOf("TOTAL") > -1 Or e.Row.Cells(2).Text.ToString.IndexOf("GROSS") > -1 Or e.Row.Cells(2).Text.ToString.IndexOf("VENTA") > -1 Then
				e.Row.Font.Bold = True
			End If
			e.Row.Cells(3).CssClass = "RightAlign"
		End If

	End Sub

	Protected Sub gvJBInterno_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvJBInterno.RowDataBound
		If e.Row.RowType = DataControlRowType.DataRow Then
			_Total = _Total + CDec(e.Row.Cells(3).Text)
			If e.Row.Cells(2).Text.ToString.IndexOf("PORCENTAJE") > -1 Then
				e.Row.Cells(3).Text = CDec(e.Row.Cells(3).Text).ToString("P")
			Else
				e.Row.Cells(3).Text = CDec(e.Row.Cells(3).Text).ToString("C0")
			End If
			If e.Row.Cells(2).Text.ToString.IndexOf("TOTAL") > -1 Or e.Row.Cells(2).Text.ToString.IndexOf("GROSS") > -1 Or e.Row.Cells(2).Text.ToString.IndexOf("VENTA") > -1 Then
				e.Row.Font.Bold = True
			End If
			e.Row.Cells(3).CssClass = "RightAlign"
		ElseIf e.Row.RowType = DataControlRowType.Footer Then

		End If

	End Sub

	Protected Sub gvDetallesOperaciones_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDetallesOperaciones.RowDataBound

		If e.Row.RowType = DataControlRowType.DataRow Then
			_Presupuestado2 = _Presupuestado2 + CDec(e.Row.Cells(2).Text.Replace(".", ""))
			e.Row.Cells(2).CssClass = "RightAlign"
			e.Row.Cells(11).CssClass = "RightAlign"
			_TotalHoras = _TotalHoras + CInt(e.Row.Cells(13).Text)

		ElseIf e.Row.RowType = DataControlRowType.Footer Then
			e.Row.Cells(1).Text = "TOTALES"
			e.Row.Cells(2).Text = _Presupuestado2.ToString("C0")
			e.Row.Cells(2).CssClass = "RightAlign"
			e.Row.Cells(13).Text = _TotalHoras.ToString("N0")

		End If

		If hfOPS.Value = 0 Then
			e.Row.Cells(11).Visible = False
			e.Row.Cells(12).Visible = False
			e.Row.Cells(13).Visible = False
		End If
	End Sub

	Protected Sub gvViaticos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvViaticos.RowDataBound
		If e.Row.RowType = DataControlRowType.DataRow Then
			_TotalViaticos = _TotalViaticos + CDec(e.Row.Cells(4).Text)
			_TotalHoteles = _TotalHoteles + CDec(e.Row.Cells(2).Text)
			_TotalTransporte = _TotalTransporte + CDec(e.Row.Cells(3).Text)
			e.Row.Cells(4).CssClass = "RightAlign"
			e.Row.Cells(2).CssClass = "RightAlign"
			e.Row.Cells(3).CssClass = "RightAlign"

		ElseIf e.Row.RowType = DataControlRowType.Footer Then
			e.Row.Cells(1).Text = "TOTALES"
			e.Row.Cells(4).Text = _TotalViaticos.ToString("C0")
			e.Row.Cells(2).Text = _TotalHoteles.ToString("C0")
			e.Row.Cells(3).Text = _TotalTransporte.ToString("C0")

			e.Row.Cells(4).CssClass = "RightAlign"
			e.Row.Cells(2).CssClass = "RightAlign"
			e.Row.Cells(3).CssClass = "RightAlign"
		End If
	End Sub

	Protected Sub gvControlCostos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvControlCostos.RowDataBound
		If e.Row.RowType = DataControlRowType.DataRow Then
			_Presupuestado = _Presupuestado + CDec(e.Row.Cells(2).Text)
			e.Row.Cells(2).CssClass = "RightAlign"
			e.Row.Cells(11).CssClass = "RightAlign"
		ElseIf e.Row.RowType = DataControlRowType.Footer Then
			e.Row.Cells(1).Text = "TOTALES"
			e.Row.Cells(2).Text = _Presupuestado.ToString("C0")
			e.Row.Cells(2).CssClass = "RightAlign"
		End If
	End Sub

	Protected Sub ExpToExcelJBE_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ExpToExcelJBE.Click
		ExportarExcelJBE(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigoJBE.Value, hfFaseJBE.Value)
	End Sub

	Protected Sub ExpToExcelJBI_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ExpToExcelJBI.Click
		ExportarExcelJBI(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigoJBI.Value, hfFaseJBI.Value)
	End Sub

	Private Sub ExportarExcelJBE(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer)
		Dim oCot As New CoreProject.Cotizador.General
		Dim wb = New XLWorkbook()
		wb.Worksheets.Add(oCot.GetCostosJobBookExterno(idPropuesta, Alternativa, Metodologia, Fase).Tables(0))
		Crearexcel(wb, "JB_externo")
	End Sub

	Private Sub ExportarExcelJBI(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer)
		Dim oCot As New CoreProject.Cotizador.General
		Dim wb = New XLWorkbook()
		wb.Worksheets.Add(oCot.GetCostosJobBookInterno(idPropuesta, Alternativa, Metodologia, Fase).Tables(0))
		Crearexcel(wb, "JB_interno")
	End Sub

	Protected Sub ExpToExcelCostos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExpToExcelCostos.Click
		Dim oCot As New CoreProject.Cotizador.General
		Dim excel As New List(Of Array)
		Dim Titulos As String
		If TabContainer1.ActiveTab.ID = "TabPanel3" Then
			Titulos = "CODIGO;CIUDAD;HOTELES;TRANSPORTE;TOTAL"
		Else
			If hfOPS.Value = 0 Then
				Titulos = "ID;ACTIVIDAD;PRESUPUESTADO"
			Else
				Titulos = "ID;ACTIVIDAD;PRESUPUESTADO;UNIDADES;DESCRIPCION;HORAS"
			End If
		End If

		Dim DynamicColNames() As String
		Dim lstCambios As New List(Of IQ_ObtenerActControlCostos_Result)
		Dim lstViaticos As New List(Of IQ_ObtenerViaticosPresupuesto_Result)
		Dim workbook = New XLWorkbook()
		Dim worksheet = workbook.Worksheets.Add("Costos")

		DynamicColNames = Titulos.Split(CChar(";"))
		excel.Add(DynamicColNames)

		If TabContainer1.ActiveTab.ID = "TabPanel1" Then
			lstCambios = oCot.GetCostos(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigoCostos.Value, hfFaseCostos.Value, 1)
		ElseIf TabContainer1.ActiveTab.ID = "TabPanel3" Then
			lstViaticos = oCot.GetViaticos(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigoCostos.Value, hfFaseCostos.Value)
		Else
			lstCambios = oCot.GetCostos(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigoCostos.Value, hfFaseCostos.Value, 2)
		End If

		If TabContainer1.ActiveTab.ID = "TabPanel3" Then
			For Each s In lstViaticos
				excel.Add((s.CODIGO.ToString() & ";" & s.CIUDAD.ToString() & ";" & CDec(s.HOTELES).ToString("N2") & ";" & CDec(s.TRANSPORTE).ToString("N2") & ";" & CDec(s.TOTAL).ToString("N2")).Split(CChar(";")).ToArray())
			Next
			worksheet.Cell("A1").Value = excel
			worksheet.Range("C2:E100").DataType = XLCellValues.Number
			worksheet.Range("C2:E100").Style.NumberFormat.NumberFormatId = 4
		Else
			If hfOPS.Value = 0 Then
				For Each x In lstCambios
					excel.Add((x.ID.ToString() & ";" & x.ActNombre & ";" & CDec(x.PRESUPUESTADO).ToString("N2")).Split(CChar(";")).ToArray())
				Next
				worksheet.Cell("A1").Value = excel
				worksheet.Range("C2:C100").DataType = XLCellValues.Number
				worksheet.Range("C2:C100").Style.NumberFormat.NumberFormatId = 4
			Else
				For Each x In lstCambios
					excel.Add((x.ID.ToString() & ";" & x.ActNombre & ";" & CDec(x.PRESUPUESTADO).ToString("N2") & ";" & CDec(x.UNIDADES).ToString("N0") & ";" & x.DESC_UNIDADES).Split(CChar(";") & ";" & CDec(x.HORAS).ToString("N0")).ToArray())
				Next
				worksheet.Cell("A1").Value = excel
				worksheet.Range("C2:C100").DataType = XLCellValues.Number
				worksheet.Range("C2:C100").Style.NumberFormat.NumberFormatId = 4
				worksheet.Range("F2:F100").DataType = XLCellValues.Number
				worksheet.Range("F2:F100").Style.NumberFormat.NumberFormatId = 4

			End If
		End If
		If TabContainer1.ActiveTab.ID = "TabPanel1" Then
			Crearexcel(workbook, "Detalles Costo unidad")
		ElseIf TabContainer1.ActiveTab.ID = "TabPanel3" Then
			Crearexcel(workbook, "Viaticos")
		Else
			Crearexcel(workbook, "Detalles costo operaciones")
		End If

	End Sub

	Private Sub Crearexcel(ByVal workbook As XLWorkbook, ByVal name As String)
		Response.Clear()

		Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
		Response.AddHeader("content-disposition", "attachment;filename=""" & name.ToString & ".xlsx""")

		Using memoryStream = New IO.MemoryStream()
			workbook.SaveAs(memoryStream)

			memoryStream.WriteTo(Response.OutputStream)
		End Using
		Response.End()
	End Sub

	Protected Sub gvPresupuestos_RowDataBound(sender As Object, e As GridViewRowEventArgs)
		If e.Row.RowType = DataControlRowType.DataRow Then
			If Convert.ToBoolean(gvPresupuestos.DataKeys(CInt(e.Row.RowIndex))("Revisado")) = True And gvPresupuestos.Columns(0).Visible Then
				Dim lkButton As LinkButton = e.Row.FindControl("lbReview")
				'e.Row.Cells(0).Enabled = False
				lkButton.Enabled = False
				lkButton.Visible = False
			End If
		End If
	End Sub

#End Region

#Region "Selenium - iQuote"

	Function ExportiQuoteGeneral(ByVal UseriQuote As String, ByVal PassiQuote As String) As String
		If chbVPNIQuote.Checked = True Then
			TimeWindows = 7
			TimeScript = 4
			TimeShort = 3
		Else
			TimeWindows = 5
			TimeScript = 3
			TimeShort = 2
		End If
		Dim driver As New ChromeDriver


		Try
			driver.Navigate.GoToUrl("http://nwb.ipsos.com/")

			Dim userMatrix = driver.FindElement(By.Name("UserName"))
			Dim passMatrix = driver.FindElement(By.Name("Password"))
			Dim btnEnter = driver.FindElement(By.Id("wsfLogonButton"))
			'userMatrix.SendKeys("Paola.Gutierrez")
			'passMatrix.SendKeys("Ipsos*235")
			userMatrix.SendKeys(UseriQuote)
			passMatrix.SendKeys(PassiQuote)
			'passMatrix.Submit()
			btnEnter.Click()

			Dim Propuesta As Integer = hfPropuesta.Value ' 10292
			Dim Alternativa As Integer = ddlAlternativa.SelectedValue ' 8

			Dim oCot As New Cotizador.General
			Dim PRGeneral = oCot.GetGeneralByAlternativa(Propuesta, Alternativa)
			Dim PRlst = oCot.GetAllPresupuestosByAlternativa(Propuesta, Alternativa)

			' ---> Búsqueda de propuesta
			'driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#proposal~19016667")
			driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#proposal~" & DirectCast(Session("InfoJobBook"), oJobBook).NumJobBook.Replace("-", ""))

			' ---> Nueva estimación
			Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))

			'driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#" & NumIQuote)
			PRGeneral.Descripcion = PRGeneral.Descripcion.Replace(vbCrLf, " -- ").Replace(vbTab, " || ").Replace("&", "")
			driver.FindElement(By.Id("o503")).SendKeys(PRGeneral.Descripcion.Replace("vbCrLf", " -- ").Replace("vbTab", " || ").Replace("&", "")) ' Descripción de presupuesto
			driver.FindElement(By.Id("o511")).SendKeys("Alternativa " & Alternativa.ToString) ' Descripción de versión
			driver.ExecuteScript("jsSaveLinkClick(0);")
			Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
			Dim NumIQuote As String
			NumIQuote = driver.Url.Split("#")(1)
			Dim pCampo As Boolean = False
			Dim pCodificacion As Boolean = False
			Dim pDP As Boolean = False
			Dim pScripting As Boolean = False
			For Each presup In PRlst
				If presup.TecCodigo = "100" Or presup.TecCodigo = "200" Or presup.TecCodigo = "300" Then
					Dim lProcesos = ObtenerProcesos(Propuesta, Alternativa, presup.MetCodigo, presup.ParNacional)
					For Each item In lProcesos
						Select Case item.ProcCodigo
							Case 1
								pCampo = True
						'DirectCast(UCHeader.FindControl("chbProcessCampo"), CheckBox).Checked = True
							Case 2
						'DirectCast(UCHeader.FindControl("chbProcessVerificacion"), CheckBox).Checked = True
							Case 3
						'DirectCast(UCHeader.FindControl("chbProcessCritica"), CheckBox).Checked = True
							Case 4
								pCodificacion = True
						'DirectCast(UCHeader.FindControl("chbProcessCodificacion"), CheckBox).Checked = True
							Case 6 To 9
								pDP = True
						'DirectCast(UCHeader.FindControl("chbProcessDataClean"), CheckBox).Checked = True
						'DirectCast(UCHeader.FindControl("chbProcessTopLines"), CheckBox).Checked = True
						'DirectCast(UCHeader.FindControl("chbProcessProceso"), CheckBox).Checked = True
						'DirectCast(UCHeader.FindControl("chbProcessArchivos"), CheckBox).Checked = True
							Case 10
								pScripting = True
								'DirectCast(UCHeader.FindControl("chbProcessScripting"), CheckBox).Checked = True
						End Select
					Next
					If pCampo = True Then
						ExportiQuoteTelefonico(driver, presup, PRGeneral)
						ExportiQuoteF2F(driver, presup, PRGeneral)
					End If
					If pCodificacion = True Then
						ExportiQuoteCodificacion(driver, presup, PRGeneral)
					End If
					If pDP = True Then
						ExportiQuoteDP(driver, presup, PRGeneral)
					End If
					If pScripting = True Then
						ExportiQuoteScripting(driver, presup, PRGeneral)
					End If
				End If
				driver.ExecuteScript("jsSectionSave_click(0);")
				Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
			Next
			driver.Navigate.GoToUrl(driver.Url)
			Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
			ExportiQuoteProfessionalTime(driver, Propuesta, Alternativa)
			driver.ExecuteScript("jsSectionSave_click(0);")
			Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
			PRGeneral.NoIQuote = NumIQuote.ToString
			oCot.PutDatosGenerales(PRGeneral)
			ModalPopupExtenderExpiQuote.Hide()
			lblNumIQuote.Text = NumIQuote
			'ShowWarning(TypesWarning.Information, "Se ha exportado la alternativa al iQuote #" & NumIQuote)
			Return "Se ha exportado la alternativa al iQuote #" & NumIQuote
		Catch ex As Exception
			'ShowWarning(TypesWarning.Warning, "Ha surgido el siguiente error: " & ex.Message.ToString)
			Return "Ha surgido el siguiente error: " & ex.Message.ToString & " | " & ex.Source
		Finally
			driver.Quit()
			driver.Dispose()
		End Try
	End Function

	Sub ExportiQuoteScripting(ByRef driver As ChromeDriver, presup As IQ_Parametros, pGeneral As IQ_DatosGeneralesPresupuesto)
		' ---> SCRIPTING <---
		driver.ExecuteScript("jsDialogDirectScriptingWaveNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o36230a")).Clear()
		driver.FindElement(By.Id("o36230a")).SendKeys(NombreFase(presup.ParNacional)) ' Nombre de la fase
		driver.ExecuteScript("jsSaveLinkClick(36);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))

		'		Setup en minutos
		driver.FindElement(By.Id("o36280")).Clear()
		driver.FindElement(By.Id("o36280")).SendKeys(presup.ParTiempoEncuesta.ToString)
		driver.FindElement(By.Id("o36380_comment")).SendKeys("") ' Comentarios
		'		Tipo de scripting
		Dim Selectionddl As New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o36380")))
		Selectionddl.SelectByIndex(1) 'Data Collection Script
		'		Número de paises
		driver.FindElement(By.Id("o36270")).Clear()
		driver.FindElement(By.Id("o36270")).SendKeys("1")
		driver.FindElement(By.Id("o36270_comment")).SendKeys("") ' Comentarios
		'		Setup Survey en minutos
		driver.FindElement(By.Id("o36280")).Clear()
		driver.FindElement(By.Id("o36280")).SendKeys(presup.ParTiempoEncuesta.ToString)
		driver.FindElement(By.Id("o36280_comment")).SendKeys("") ' Comentarios
		'		Complejidad del scripting
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o36295")))
		If presup.DPComplejidadCuestionario IsNot Nothing Then
			Selectionddl.SelectByIndex(presup.DPComplejidadCuestionario)
		Else
			Selectionddl.SelectByIndex(1)
		End If
		driver.FindElement(By.Id("o36295_comment")).SendKeys("") ' Comentarios 
		'		Es Online? No
		If presup.TecCodigo = "300" Then
			driver.FindElement(By.Id("o36300_1")).Click()
		Else
			driver.FindElement(By.Id("o36300_0")).Click()
		End If
		'		Telefónico? Sí

		If presup.TecCodigo = "200" Then
			driver.FindElement(By.Id("o36305_1")).Click()
			driver.ExecuteScript("jsScriptingTelephoneRequired();")
			Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
			Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o36456")))
			Selectionddl.SelectByIndex(1) 'No quotas
		Else
			driver.FindElement(By.Id("o36305_0")).Click()
			driver.ExecuteScript("jsScriptingTelephoneRequired();")
		End If
		'		Es F2F? No
		If presup.TecCodigo = "100" Then
			driver.FindElement(By.Id("o36310_1")).Click()
		Else
			driver.FindElement(By.Id("o36310_0")).Click()
		End If

		'	GUARDAR SCRIPTING
		driver.FindElement(By.Id("o36375_comment")).SendKeys("") ' Otros Comentarios generales
		driver.ExecuteScript("jsSectionSave_click(36);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogDirectScriptingWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		'driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#proposal#" & NumIQuote)

	End Sub

	Sub ExportiQuoteTelefonico(ByRef driver As ChromeDriver, presup As IQ_Parametros, pGeneral As IQ_DatosGeneralesPresupuesto)
		If Not presup.TecCodigo = "200" Then Exit Sub
		' ---> TELEFÓNICO <---
		driver.ExecuteScript("javascript:jsDialogDirectTelephoneWaveNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o20230a")).Clear()
		driver.FindElement(By.Id("o20230a")).SendKeys(NombreFase(presup.ParNacional)) ' Nombre de la fase
		driver.ExecuteScript("javascript:jsSaveLinkClick(20);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Es Tracking? Sí
		If pGeneral.NumeroMediciones > 1 Then
			driver.FindElement(By.Id("o20240_1")).Click()
			driver.ExecuteScript("jsTelephoneIsTracking();")
			driver.FindElement(By.Id("o20242")).Clear()
			driver.FindElement(By.Id("o20242")).SendKeys(pGeneral.NumeroMediciones.ToString) 'Número de olas
		Else
			driver.FindElement(By.Id("o20240_0")).Click()
			driver.ExecuteScript("jsTelephoneIsTracking();")
		End If

		'		Tema
		Dim Selectionddl As New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20244")))
		Selectionddl.SelectByIndex(2) 'Consumidor mediano interés
		driver.FindElement(By.Id("o20244_comment")).SendKeys("") 'Comentarios tema o grupo objetivo
		'		Días en campo
		driver.FindElement(By.Id("o20250")).Clear()
		driver.FindElement(By.Id("o20250")).SendKeys(pGeneral.DiasCampo.ToString) 'Días en campo
		driver.FindElement(By.Id("o20250_comment")).SendKeys("") 'Comentarios días en campo
		'		Quotas
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20291")))
		driver.FindElement(By.Id("o20291_comment")).SendKeys("") 'Comentarios cuotas
		Selectionddl.SelectByIndex(1) 'Tipo de control de cuota -> No Cuota
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20293")))
		Selectionddl.SelectByIndex(1) 'Número de celdas de cuotas -> No Cuota
		'	GUARDAR TELEFÓNICO
		driver.ExecuteScript("jsSectionSave_click(20);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'DETALLE 
		driver.ExecuteScript("javascript:jsDialogDirectTelephoneSampleNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		'		Número de encuestas
		Dim oCot1 As New Cotizador.General
		driver.FindElement(By.Id("o20930")).SendKeys(oCot1.GetTotalMuestra(presup.IdPropuesta, presup.ParAlternativa, presup.MetCodigo, presup.ParNacional).ToString)
		'		Tipo de respondiente
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20940")))
		Selectionddl.SelectByIndex(1) 'Consumidor individual
		driver.FindElement(By.Id("o20940_comment")).SendKeys("n/a") 'Comentarios 
		'		Selección respondientes
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20945")))
		Selectionddl.SelectByIndex(1) 'Quota selection
		driver.FindElement(By.Id("o20945_comment")).SendKeys("n/a") 'Comentarios 
		'		Origen de muestra
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20965")))
		Selectionddl.SelectByIndex(2) 'Client supplied
		driver.FindElement(By.Id("o20965_comment")).SendKeys("n/a") 'Comentarios 
		'		Disponibilidad de muestra
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20970")))
		Selectionddl.SelectByIndex(3) '7:1 to 10:1
		driver.FindElement(By.Id("o20970_comment")).SendKeys("n/a") 'Comentarios 
		'		Incidencia
		driver.FindElement(By.Id("o20925")).SendKeys(Math.Round(CInt(presup.ParIncidencia), 0).ToString) 'Comentarios 
		driver.FindElement(By.Id("o20925_comment")).SendKeys("n/a") 'Comentarios 
		'		Screener Lenght
		driver.FindElement(By.Id("o20980")).Clear()
		driver.FindElement(By.Id("o20980")).SendKeys(presup.ParTiempoEncuesta)
		'		Duración encuesta
		driver.FindElement(By.Id("o20935")).SendKeys(presup.ParTiempoEncuesta)
		'		Identificación cliente
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20985")))
		Selectionddl.SelectByIndex(1) 'None / Unsure
		driver.FindElement(By.Id("o20985_comment")).SendKeys("n/a") 'Comentarios 
		driver.ExecuteScript("jsSectionSave_click(200);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogDirectTelephoneSampleDialogClose();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		'	GUARDAR TELEFÓNICO
		driver.ExecuteScript("jsSectionSave_click(20);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))

		driver.ExecuteScript("jsDialogDirectTelephoneWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))

	End Sub

	Sub ExportiQuoteF2F(ByRef driver As ChromeDriver, presup As IQ_Parametros, pGeneral As IQ_DatosGeneralesPresupuesto)
		If Not presup.TecCodigo = "100" Then Exit Sub
		driver.ExecuteScript("javascript:jsDialogDirectF2FWaveNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o39230a")).Clear()
		driver.FindElement(By.Id("o39230a")).SendKeys(NombreFase(presup.ParNacional).ToString) ' Nombre de la fase
		driver.ExecuteScript("javascript:jsSaveLinkClick(39);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Es Tracking? Sí
		If pGeneral.NumeroMediciones > 1 Then
			driver.FindElement(By.Id("o39240_1")).Click()
			driver.ExecuteScript("jsF2FIsTracking();")
			driver.FindElement(By.Id("o39242")).Clear()
			driver.FindElement(By.Id("o39242")).SendKeys(pGeneral.NumeroMediciones.ToString) 'Número de olas
		Else
			driver.FindElement(By.Id("o39240_0")).Click()
			driver.ExecuteScript("jsF2FIsTracking();")
		End If
		'		Tema
		Dim Selectionddl As New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39244")))
		Selectionddl.SelectByIndex(2) 'Consumidor mediano interés
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		Try
			driver.FindElement(By.Id("o39244_comment")).SendKeys("n/a") 'Comentarios tema o grupo objetivo
		Catch ex As Exception

		End Try

		'		Es Product Test? Sí
		If presup.MetCodigo = 105 Or presup.MetCodigo = 120 Or presup.MetCodigo = 125 Then
			driver.FindElement(By.Id("o39302_1")).Click()
			driver.ExecuteScript("jsF2FIsThisProductChange();")
			'		Requiere compra de producto? Sí
			If Not presup.PTCompra Is Nothing Then
				If presup.PTCompra = True Then
					driver.FindElement(By.Id("o39250_1")).Click()
					driver.FindElement(By.Id("o39250_comment")).SendKeys("n/a") 'Comentarios compra de producto
				Else
					driver.FindElement(By.Id("o39250_0")).Click()
				End If
			Else
				driver.FindElement(By.Id("o39250_0")).Click()
			End If
			'		Tipo de producto
			Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39251")))
			Selectionddl.SelectByIndex(1) 'Small
			'		Productos por respondiente
			If Not presup.PTProductosEvaluar Is Nothing Then
				driver.FindElement(By.Id("o39252")).SendKeys(presup.PTProductosEvaluar.ToString) 'Comentarios tema o grupo objetivo
			Else
				driver.FindElement(By.Id("o39252")).SendKeys("1") 'Comentarios tema o grupo objetivo
			End If

		Else
			driver.FindElement(By.Id("o39302_0")).Click()
			driver.ExecuteScript("jsF2FIsThisProductChange();")
		End If
		'	GUARDAR F2F
		driver.ExecuteScript("jsSectionSave_click(39);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Especificaciones F2F - Stage
		driver.ExecuteScript("jsDialogDirectF2FStageNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o39910")).SendKeys("Main Interview") 'Nombre stage
		'		Stage
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39915")))
		Selectionddl.SelectByIndex(1) 'Main interview
		'		Data collection mode
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39948")))
		Selectionddl.SelectByIndex(1) 'Data collection mode - iField
		'		Duración días
		driver.FindElement(By.Id("o39955")).SendKeys(pGeneral.DiasCampo) 'Número de días en campo
		driver.FindElement(By.Id("o39960")).SendKeys(pGeneral.DiasCampo) 'Número de días iField
		'		Sample Strategy
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39878")))
		Selectionddl.SelectByIndex(2) 'Free find
		'		Geographical coverage
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39874")))
		Selectionddl.SelectByIndex(1) 'National coverage
		'		Target group name
		driver.FindElement(By.Id("iqtMain_sample001_1")).SendKeys("General") '
		'		Respondent type
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("iqtMain_sample002_1")))
		Selectionddl.SelectByIndex(2) 'Consumers individuals
		'		Respondent selection
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("iqtMain_sample003_1")))
		Selectionddl.SelectByIndex(1) 'Quota selection
		'		Cantidad e incidencia
		Dim oCot1 As New Cotizador.General
		driver.FindElement(By.Id("iqtMain_sample004_1")).SendKeys(oCot1.GetTotalMuestra(presup.IdPropuesta, presup.ParAlternativa, presup.MetCodigo, presup.ParNacional).ToString) 'Número de encustas
		driver.FindElement(By.Id("iqtMain_sample005_1")).SendKeys(Math.Round(CInt(presup.ParIncidencia), 0).ToString) 'Gen Pop Incidence
		driver.FindElement(By.Id("iqtMain_sample006_1")).SendKeys(Math.Round(CInt(presup.ParIncidencia), 0).ToString) 'Target incidence
		'		Quota control
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("iqtMain_sample007_1")))
		Selectionddl.SelectByIndex(1) 'Parallel flexible quota
		'		Quota categories
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("iqtMain_sample008_1")))
		Selectionddl.SelectByIndex(1) 'Standar quotas
		'		Duración
		driver.FindElement(By.Id("iqtMain_sample009_1")).SendKeys(presup.ParTiempoEncuesta.ToString) 'Screener lenght
		driver.FindElement(By.Id("iqtMain_sample010_1")).SendKeys(presup.ParTiempoEncuesta.ToString) 'Interview lenght

		driver.ExecuteScript("jsDialogDirectF2FStageSet(false,'none',0);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))

		driver.ExecuteScript("jsDialogDirectF2FStageDialogClose();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))


		driver.ExecuteScript("jsSectionSave_click(39);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))

		driver.ExecuteScript("jsDialogDirectF2FWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		'driver.Manage.Window.Minimize()

	End Sub

	Sub ExportiQuoteCodificacion(ByRef driver As ChromeDriver, presup As IQ_Parametros, pGeneral As IQ_DatosGeneralesPresupuesto)
		' ---> CODIFICACIÓN <---
		driver.ExecuteScript("javascript:jsDialogDirectCodingWaveNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o22233")).Clear()
		driver.FindElement(By.Id("o22233")).SendKeys(NombreFase(presup.ParNacional)) ' Nombre de la fase
		driver.ExecuteScript("javascript:jsSaveLinkClick(22);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Es Tracking? Sí
		If pGeneral.NumeroMediciones > 1 Then
			driver.FindElement(By.Id("o22240_1")).Click()
			driver.ExecuteScript("jsCodingIsTracking();")
			driver.FindElement(By.Id("o22242")).Clear()
			driver.FindElement(By.Id("o22242")).SendKeys(pGeneral.NumeroMediciones) 'Número de olas
		Else
			'driver.FindElement(By.Id("o22240_0")).Click()
		End If
		Dim oCot1 As New Cotizador.General

		'		Número de encuestas
		driver.FindElement(By.Id("o22245")).SendKeys(oCot1.GetTotalMuestra(presup.IdPropuesta, presup.ParAlternativa, presup.MetCodigo, presup.ParNacional).ToString)
		'		Complejidad de la codificación
		Dim Selectionddl As New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o22246")))
		If presup.ComplejidadCodificacion IsNot Nothing Then
			Selectionddl.SelectByIndex(presup.ComplejidadCodificacion) 'Standard
		Else
			Selectionddl.SelectByIndex(2) 'Standard
		End If
		driver.FindElement(By.Id("o22246_comment")).SendKeys("n/a") 'Comentarios 
		'		Preguntas abiertas múltiples
		driver.FindElement(By.Id("o22250")).SendKeys(presup.IQ_Preguntas.PregAbiertasMultiples.ToString)
		'		Complejidad de la codificación (cleaner preguntas)
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o22252")))
		Selectionddl.SelectByIndex(2) 'Standard
		'		Preguntas abiertas
		driver.FindElement(By.Id("o22254")).SendKeys(presup.IQ_Preguntas.PregAbiertas)
		'		Preguntas otros
		driver.FindElement(By.Id("o22258")).SendKeys(presup.IQ_Preguntas.PregOtras.ToString)
		driver.ExecuteScript("jsSectionSave_click(22);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogDirectCodingWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))

	End Sub

	Sub ExportiQuoteDP(ByRef driver As ChromeDriver, presup As IQ_Parametros, pGeneral As IQ_DatosGeneralesPresupuesto)
		driver.ExecuteScript("javascript:jsDialogDirectDataProcessingWaveNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o21230a")).Clear()
		driver.FindElement(By.Id("o21230a")).SendKeys(NombreFase(presup.ParNacional)) ' Nombre de la fase
		driver.ExecuteScript("javascript:jsSaveLinkClick(21);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Es Tracking? Sí
		If pGeneral.MesesMediciones > 1 Then
			driver.FindElement(By.Id("o21250_1")).Click()
			driver.ExecuteScript("jsDataProcessingIsTracking();")

			driver.FindElement(By.Id("o21260")).Clear()
			driver.FindElement(By.Id("o21260")).SendKeys(pGeneral.NumeroMediciones) 'Número de olas
			driver.FindElement(By.Id("oDataProcessingsw_none")).Clear()
			driver.FindElement(By.Id("oDataProcessingsw_none")).SendKeys((pGeneral.NumeroMediciones - 1).ToString) 'Cambios
			'		Tipo de Ola
			Dim Selectionddl1 As New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o21255")))
			Selectionddl1.SelectByIndex(1) 'Setup wave / New study
			driver.FindElement(By.Id("o21255_comment")).SendKeys("n/a") 'Comentarios 

		Else
			driver.FindElement(By.Id("o21250_0")).Click()
			driver.ExecuteScript("jsDataProcessingIsTracking();")
		End If

		'		Origen de la data
		Dim Selectionddl As New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o21290")))
		Selectionddl.SelectByIndex(1) 'Ipsos OPS Collected Data
		driver.FindElement(By.Id("o21290_comment")).SendKeys("n/a") 'Comentarios 
		'		Survey Setup length
		driver.FindElement(By.Id("o21280")).SendKeys(presup.ParTiempoEncuesta.ToString)
		'		Requiere datacleaning? Sí
		If presup.ParNProcesosDC > 0 Then
			driver.FindElement(By.Id("o21285_1")).Click()
			driver.ExecuteScript("jsDataProcessingIsTracking();")
		Else
			driver.FindElement(By.Id("o21285_0")).Click()
			driver.ExecuteScript("jsDataProcessingIsTracking();")
		End If
		'		Complejidad de tablas
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o21295")))
		If presup.DPComplejidad IsNot Nothing Then
			Selectionddl.SelectByIndex(presup.DPComplejidad) 'Standard
		Else
			Selectionddl.SelectByIndex(2) 'Standard
		End If
		driver.FindElement(By.Id("o21295_comment")).SendKeys("n/a") 'Comentarios 
		'		Es CATI? SI
		If presup.TecCodigo = "200" Then
			driver.FindElement(By.Id("o21315_1")).Click()
		Else
			driver.FindElement(By.Id("o21315_0")).Click()
		End If
		'		Es ONLINE? SI
		If presup.TecCodigo = "300" Then
			driver.FindElement(By.Id("o21320_1")).Click()
		Else
			driver.FindElement(By.Id("o21320_0")).Click()
		End If
		'		Es HandHeld? SI
		driver.FindElement(By.Id("o21335_1")).Click()
		driver.ExecuteScript("jsSectionSave_click(21);")
		Thread.Sleep(TimeSpan.FromSeconds(5))
		driver.ExecuteScript("jsDialogDirectDataProcessingWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(2))
		driver.ExecuteScript("jsSectionSave_click(0);")
	End Sub

	Sub ExportiQuoteProfessionalTime(ByRef driver As ChromeDriver, IdPropuesta As Int64, Alternativa As Int32)

		Dim oHorasProfesionales = ObtenerHorasProfesionalesXAlternativa(IdPropuesta, Alternativa).Where(Function(x) x.TotalHoras > 0).ToList
		If oHorasProfesionales.Count = 0 Then
			Exit Sub
		End If
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("toggleImg_5_")).Click()
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		For Each level In oHorasProfesionales
			If level.TotalHoras > 0 Then
				Try
					driver.ExecuteScript("jsDetailGlobalProfTimeAdd();")
				Catch ex As Exception
					driver.SwitchTo().Alert().Dismiss()
					Try
						'driver.ExecuteScript("jsDetailGlobalProfTimeAdd();")
						driver.SwitchTo().Alert().Dismiss()
					Catch ex2 As Exception
						driver.SwitchTo().Alert().Dismiss()
					End Try
				End Try
			End If
		Next
		Try
			driver.SwitchTo().Alert().Dismiss()
			driver.SwitchTo().Alert().Dismiss()
		Catch ex As Exception

		End Try
		Dim tabHoras As IWebElement
		Try
			tabHoras = driver.FindElement(By.Id("o5701"))
		Catch ex As Exception
			Try
				driver.SwitchTo().Alert().Dismiss()
				driver.SwitchTo().Alert().Dismiss()
			Catch ex2 As Exception

			End Try
		End Try
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		tabHoras = driver.FindElement(By.Id("o5701"))
		Dim lstSelect As IList(Of IWebElement) = tabHoras.FindElements(By.TagName("Select"))
		While lstSelect.Count < oHorasProfesionales.Count
			Try
				driver.ExecuteScript("jsDetailGlobalProfTimeAdd();")
				Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
			Catch ex As Exception
				Try
					driver.SwitchTo().Alert().Dismiss()
				Catch ex2 As Exception
					Try
						driver.SwitchTo().Alert().Dismiss()
					Catch ex3 As Exception
						driver.SwitchTo().Alert().Dismiss()
					End Try
				End Try
			End Try
			Try
				lstSelect = tabHoras.FindElements(By.TagName("Select"))
			Catch ex As Exception
				driver.SwitchTo().Alert().Dismiss()
			End Try
			'lstSelect = tabHoras.FindElements(By.TagName("Select"))
			Try
				lstSelect = tabHoras.FindElements(By.TagName("Select"))
			Catch ex As Exception
				driver.SwitchTo().Alert().Dismiss()
				Exit While
			End Try

		End While
		Try
			driver.SwitchTo().Alert().Dismiss()
			driver.SwitchTo().Alert().Dismiss()
		Catch ex As Exception

		End Try
		Try
			lstSelect = tabHoras.FindElements(By.TagName("Select"))
		Catch ex As Exception
			Try
				driver.SwitchTo().Alert().Dismiss()
			Catch ex2 As Exception
			End Try
			lstSelect = tabHoras.FindElements(By.TagName("Select"))
		End Try
		Dim indSelect As Integer = 0
		For Each elem In lstSelect
			If indSelect = oHorasProfesionales.Count Then Exit For
			Dim SelectElement As New OpenQA.Selenium.Support.UI.SelectElement(elem)
			SelectElement.SelectByIndex(oHorasProfesionales(indSelect).PGrCodigo - 3001)
			driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("level", "hours2") & "').value = '" & oHorasProfesionales(indSelect).PreField.ToString & "';")
			driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("level", "hours3") & "').value = '" & oHorasProfesionales(indSelect).FieldWork.ToString & "';")
			driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("level", "hours4") & "').value = '" & oHorasProfesionales(indSelect).DPandCoding.ToString & "';")
			driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("level", "hours5") & "').value = '" & oHorasProfesionales(indSelect).Analysis.ToString & "';")
			driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("level", "hours6") & "').value = '" & oHorasProfesionales(indSelect).PM.ToString & "';")
			driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("level", "hours8") & "').value = '" & oHorasProfesionales(indSelect).Meetings.ToString & "';")
			driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("level", "hours10") & "').value = '" & oHorasProfesionales(indSelect).Presentation.ToString & "';")
			driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("level", "hours9") & "').value = '" & oHorasProfesionales(indSelect).ClientTravel.ToString & "';")
			indSelect += 1
		Next
		driver.ExecuteScript("jsSectionSave_click(5);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
	End Sub


	Sub Other(ByRef driver As ChromeDriver)

	End Sub
	Sub iQuoteTest()

		Dim driver As New ChromeDriver


		driver.Navigate.GoToUrl("http://nwb.ipsos.com/")

		Dim userMatrix = driver.FindElement(By.Name("UserName"))
		Dim passMatrix = driver.FindElement(By.Name("Password"))
		Dim btnEnter = driver.FindElement(By.Id("wsfLogonButton"))
		userMatrix.SendKeys("Paola.Gutierrez")
		passMatrix.SendKeys("Ipsos*235")
		'passMatrix.Submit()
		btnEnter.Click()

		' ---> Búsqueda de propuesta
		driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#proposal~19016667")


		' ---> Nueva estimación
		driver.ExecuteScript("jsSaveLinkClick(0);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))

		Dim NumIQuote As String
		NumIQuote = driver.Url.Split("#")(1)
		'driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#" & NumIQuote)

		driver.FindElement(By.Id("o503")).SendKeys("Descripción del presupuesto") ' Descripción de presupuesto
		driver.FindElement(By.Id("o511")).SendKeys("Número de alternativa") ' Descripción de versión

		' ---> SCRIPTING <---
		driver.ExecuteScript("jsDialogDirectScriptingWaveNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o36230a")).Clear()
		driver.FindElement(By.Id("o36230a")).SendKeys("Fase 1") ' Nombre de la fase
		driver.ExecuteScript("jsSaveLinkClick(36);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))

		'		Setup en minutos
		driver.FindElement(By.Id("o36280")).Clear()
		driver.FindElement(By.Id("o36280")).SendKeys("30")
		driver.FindElement(By.Id("o36380_comment")).SendKeys("") ' Comentarios
		'		Tipo de scripting
		Dim Selectionddl As New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o36380")))
		Selectionddl.SelectByIndex(1) 'Data Collection Script
		'		Número de paises
		driver.FindElement(By.Id("o36270")).Clear()
		driver.FindElement(By.Id("o36270")).SendKeys("1")
		driver.FindElement(By.Id("o36270_comment")).SendKeys("") ' Comentarios
		'		Setup Survey en minutos
		driver.FindElement(By.Id("o36280")).Clear()
		driver.FindElement(By.Id("o36280")).SendKeys("20")
		driver.FindElement(By.Id("o36280_comment")).SendKeys("") ' Comentarios
		'		Complejidad del scripting
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o36295")))
		Selectionddl.SelectByIndex(1)
		driver.FindElement(By.Id("o36295_comment")).SendKeys("") ' Comentarios 
		'		Es Online? No
		driver.FindElement(By.Id("o36300_0")).Click()
		'		Telefónico? Sí
		driver.FindElement(By.Id("o36305_1")).Click()
		driver.ExecuteScript("jsScriptingTelephoneRequired();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o36456")))
		Selectionddl.SelectByIndex(1) 'No quotas
		'		Es F2F? No
		driver.FindElement(By.Id("o36310_0")).Click()
		'	GUARDAR SCRIPTING
		driver.FindElement(By.Id("o36375_comment")).SendKeys("") ' Otros Comentarios generales
		driver.ExecuteScript("jsSectionSave_click(36);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogDirectScriptingWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		'driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#proposal#" & NumIQuote)


		' ---> TELEFÓNICO <---
		driver.ExecuteScript("javascript:jsDialogDirectTelephoneWaveNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o20230a")).Clear()
		driver.FindElement(By.Id("o20230a")).SendKeys("Fase 1") ' Nombre de la fase
		driver.ExecuteScript("javascript:jsSaveLinkClick(20);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Es Tracking? Sí
		driver.FindElement(By.Id("o20240_1")).Click()
		driver.ExecuteScript("jsTelephoneIsTracking();")
		driver.FindElement(By.Id("o20242")).Clear()
		driver.FindElement(By.Id("o20242")).SendKeys("2") 'Número de olas
		'		Tema
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20244")))
		Selectionddl.SelectByIndex(2) 'Consumidor mediano interés
		driver.FindElement(By.Id("o20244_comment")).SendKeys("") 'Comentarios tema o grupo objetivo
		'		Días en campo
		driver.FindElement(By.Id("o20250")).Clear()
		driver.FindElement(By.Id("o20250")).SendKeys("5") 'Días en campo
		driver.FindElement(By.Id("o20250_comment")).SendKeys("") 'Comentarios días en campo
		'		Quotas
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20291")))
		driver.FindElement(By.Id("o20291_comment")).SendKeys("") 'Comentarios cuotas
		Selectionddl.SelectByIndex(1) 'Tipo de control de cuota -> No Cuota
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20293")))
		Selectionddl.SelectByIndex(1) 'Número de celdas de cuotas -> No Cuota
		'	GUARDAR TELEFÓNICO
		driver.ExecuteScript("jsSectionSave_click(20);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'DETALLE 
		driver.ExecuteScript("javascript:jsDialogDirectTelephoneSampleNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		'		Número de encuestas
		driver.FindElement(By.Id("o20930")).SendKeys("100")
		'		Tipo de respondiente
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20940")))
		Selectionddl.SelectByIndex(1) 'Consumidor individual
		driver.FindElement(By.Id("o20940_comment")).SendKeys("n/a") 'Comentarios 
		'		Selección respondientes
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20945")))
		Selectionddl.SelectByIndex(1) 'Quota selection
		driver.FindElement(By.Id("o20945_comment")).SendKeys("n/a") 'Comentarios 
		'		Origen de muestra
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20965")))
		Selectionddl.SelectByIndex(2) 'Client supplied
		driver.FindElement(By.Id("o20965_comment")).SendKeys("n/a") 'Comentarios 
		'		Disponibilidad de muestra
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20970")))
		Selectionddl.SelectByIndex(3) '7:1 to 10:1
		driver.FindElement(By.Id("o20970_comment")).SendKeys("n/a") 'Comentarios 
		'		Incidencia
		driver.FindElement(By.Id("o20925")).SendKeys("50") 'Comentarios 
		driver.FindElement(By.Id("o20925_comment")).SendKeys("n/a") 'Comentarios 
		'		Screener Lenght
		driver.FindElement(By.Id("o20980")).Clear()
		driver.FindElement(By.Id("o20980")).SendKeys("2")
		'		Duración encuesta
		driver.FindElement(By.Id("o20935")).SendKeys("15")
		'		Identificación cliente
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20985")))
		Selectionddl.SelectByIndex(1) 'None / Unsure
		driver.FindElement(By.Id("o20985_comment")).SendKeys("n/a") 'Comentarios 
		driver.ExecuteScript("jsSectionSave_click(200);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogDirectTelephoneSampleDialogClose();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		'	GUARDAR TELEFÓNICO
		driver.ExecuteScript("jsSectionSave_click(20);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))

		driver.ExecuteScript("jsDialogDirectTelephoneWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))


		' ---> F2F <---
		driver.ExecuteScript("javascript:jsDialogDirectF2FWaveNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o39230a")).Clear()
		driver.FindElement(By.Id("o39230a")).SendKeys("Fase 1") ' Nombre de la fase
		driver.ExecuteScript("javascript:jsSaveLinkClick(39);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Es Tracking? Sí
		driver.FindElement(By.Id("o39240_1")).Click()
		driver.ExecuteScript("jsF2FIsTracking();")
		driver.FindElement(By.Id("o39242")).Clear()
		driver.FindElement(By.Id("o39242")).SendKeys("2") 'Número de olas
		'		Tema
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39244")))
		Selectionddl.SelectByIndex(2) 'Consumidor mediano interés
		driver.FindElement(By.Id("o39244_comment")).SendKeys("n/a") 'Comentarios tema o grupo objetivo
		'		Es Product Test? Sí
		driver.FindElement(By.Id("o39302_1")).Click()
		driver.ExecuteScript("jsF2FIsThisProductChange();")
		'		Requiere compra de producto? Sí
		driver.FindElement(By.Id("o39250_1")).Click()
		driver.FindElement(By.Id("o39250_comment")).SendKeys("n/a") 'Comentarios compra de producto
		'		Tipo de producto
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39251")))
		Selectionddl.SelectByIndex(1) 'Small
		'		Productos por respondiente
		driver.FindElement(By.Id("o39252")).SendKeys("1") 'Comentarios tema o grupo objetivo
		'	GUARDAR F2F
		driver.ExecuteScript("jsSectionSave_click(39);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Especificaciones F2F - Stage
		driver.ExecuteScript("jsDialogDirectF2FStageNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o39910")).SendKeys("Main Interview") 'Nombre stage
		'		Stage
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39915")))
		Selectionddl.SelectByIndex(1) 'Main interview
		'		Data collection mode
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39948")))
		Selectionddl.SelectByIndex(1) 'Data collection mode - iField
		'		Duración días
		driver.FindElement(By.Id("o39955")).SendKeys("10") 'Número de días en campo
		driver.FindElement(By.Id("o39960")).SendKeys("10") 'Número de días iField
		'		Sample Strategy
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39878")))
		Selectionddl.SelectByIndex(2) 'Free find
		'		Geographical coverage
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39874")))
		Selectionddl.SelectByIndex(1) 'National coverage
		'		Target group name
		driver.FindElement(By.Id("iqtMain_sample001_1")).SendKeys("General") '
		'		Respondent type
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("iqtMain_sample002_1")))
		Selectionddl.SelectByIndex(2) 'Consumers individuals
		'		Respondent selection
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("iqtMain_sample003_1")))
		Selectionddl.SelectByIndex(1) 'Quota selection
		'		Cantidad e incidencia
		driver.FindElement(By.Id("iqtMain_sample004_1")).SendKeys("100") 'Número de encustas
		driver.FindElement(By.Id("iqtMain_sample005_1")).SendKeys("40") 'Gen Pop Incidence
		driver.FindElement(By.Id("iqtMain_sample006_1")).SendKeys("50") 'Target incidence
		'		Quota control
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("iqtMain_sample007_1")))
		Selectionddl.SelectByIndex(1) 'Parallel flexible quota
		'		Quota categories
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("iqtMain_sample008_1")))
		Selectionddl.SelectByIndex(1) 'Standar quotas
		'		Duración
		driver.FindElement(By.Id("iqtMain_sample009_1")).SendKeys("40") 'Screener lenght
		driver.FindElement(By.Id("iqtMain_sample010_1")).SendKeys("50") 'Interview lenght

		driver.ExecuteScript("jsDialogDirectF2FStageSet(false,'none',0);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))

		driver.ExecuteScript("jsDialogDirectF2FStageDialogClose();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))


		driver.ExecuteScript("jsSectionSave_click(39);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))

		driver.ExecuteScript("jsDialogDirectF2FWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		'driver.Manage.Window.Minimize()


		' ---> CODIFICACIÓN <---
		driver.ExecuteScript("javascript:jsDialogDirectCodingWaveNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o22233")).Clear()
		driver.FindElement(By.Id("o22233")).SendKeys("Fase 1") ' Nombre de la fase
		driver.ExecuteScript("javascript:jsSaveLinkClick(22);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Es Tracking? Sí
		driver.FindElement(By.Id("o22240_1")).Click()
		driver.ExecuteScript("jsF2FIsTracking();")
		driver.FindElement(By.Id("o22242")).Clear()
		driver.FindElement(By.Id("o22242")).SendKeys("2") 'Número de olas
		'		Número de encuestas
		driver.FindElement(By.Id("o22245")).SendKeys("100")
		'		Complejidad de la codificación
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o22246")))
		Selectionddl.SelectByIndex(2) 'Standard
		driver.FindElement(By.Id("o22246_comment")).SendKeys("n/a") 'Comentarios 
		'		Preguntas abiertas múltiples
		driver.FindElement(By.Id("o22250")).SendKeys("20")
		'		Complejidad de la codificación (cleaner preguntas)
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o22252")))
		Selectionddl.SelectByIndex(2) 'Standard
		'		Preguntas abiertas
		driver.FindElement(By.Id("o22254")).SendKeys("50")
		'		Preguntas otros
		driver.FindElement(By.Id("o22258")).SendKeys("20")
		driver.ExecuteScript("jsSectionSave_click(22);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogDirectCodingWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))


		' ---> DP <---
		driver.ExecuteScript("javascript:jsDialogDirectDataProcessingWaveNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o21230a")).Clear()
		driver.FindElement(By.Id("o21230a")).SendKeys("Fase 1") ' Nombre de la fase
		driver.ExecuteScript("javascript:jsSaveLinkClick(21);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Es Tracking? Sí
		driver.FindElement(By.Id("o21250_1")).Click()
		driver.ExecuteScript("jsDataProcessingIsTracking();")
		driver.FindElement(By.Id("oDataProcessingsw_none")).SendKeys("1") 'Cambios
		'		Tipo de Ola
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o21255")))
		Selectionddl.SelectByIndex(1) 'Setup wave / New study
		driver.FindElement(By.Id("o21255_comment")).SendKeys("n/a") 'Comentarios 

		driver.FindElement(By.Id("o21260")).Clear()
		driver.FindElement(By.Id("o21260")).SendKeys("2") 'Número de olas
		'		Origen de la data
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o21290")))
		Selectionddl.SelectByIndex(1) 'Ipsos OPS Collected Data
		driver.FindElement(By.Id("o21290_comment")).SendKeys("n/a") 'Comentarios 
		'		Survey Setup length
		driver.FindElement(By.Id("o21280")).SendKeys("20")
		'		Requiere datacleaning? Sí
		driver.FindElement(By.Id("o21285_1")).Click()
		driver.ExecuteScript("jsDataProcessingIsTracking();")
		'		Complejidad de tablas
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o21295")))
		Selectionddl.SelectByIndex(2) 'Standard
		driver.FindElement(By.Id("o21295_comment")).SendKeys("n/a") 'Comentarios 
		'		Es CATI? SI
		driver.FindElement(By.Id("o21315_1")).Click()
		'		Es ONLINE? SI
		driver.FindElement(By.Id("o21320_1")).Click()
		'		Es HandHeld? SI
		driver.FindElement(By.Id("o21335_1")).Click()
		driver.ExecuteScript("jsSectionSave_click(21);")
		Thread.Sleep(TimeSpan.FromSeconds(5))
		driver.ExecuteScript("jsDialogDirectDataProcessingWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(2))
		driver.ExecuteScript("jsSectionSave_click(0);")
	End Sub

	Function NombreFase(ByVal idFase As Int32) As String
		If idFase = 1 Then
			Return "General"
		Else
			Return "Fase " & (idFase - 2).ToString
		End If

	End Function

	Protected Sub btnExportIQuote_Click(sender As Object, e As EventArgs)
		ModalPopupExtenderExpiQuote.Show()
	End Sub

	Protected Sub btnExportToiQuote_Click(sender As Object, e As EventArgs)
		Dim oCot As New Cotizador.General
		If oCot.ExistsAlternativaMarkedToExport(hfPropuesta.Value, ddlAlternativa.SelectedValue) = True Then
			ShowWarning(TypesWarning.ErrorMessage, "Esta alternativa ya ha sido marcada para enviar. Use la aplicación de escritorio para enviarla a iQuote")
		Else
			If oCot.ExistsAlternativaMarkedToExportPrevious(hfPropuesta.Value, ddlAlternativa.SelectedValue) = True Then
				ShowWarning(TypesWarning.Information, "Ya hay una versión previa subida a iQuote. Sin embargo se marcará esta alternativa para enviar una nueva versión")
			Else
				ShowWarning(TypesWarning.Information, "La alternativa ha sido marcada para ser enviada a iQuote")
			End If
			oCot.MarkAlternativaToExportToIQuote(hfPropuesta.Value, ddlAlternativa.SelectedValue)
		End If
		'If txtUsuarioiQuote.Text = "" Or txtPasswordiQuote.Text = "" Then
		'	ShowWarning(TypesWarning.ErrorMessage, "Escriba las credenciales para poder continuar")
		'	Exit Sub
		'End If
		'Dim txtResultado As String = ExportiQuoteGeneral(txtUsuarioiQuote.Text, txtPasswordiQuote.Text)
		'If txtResultado.StartsWith("Ha surgido") Then
		'	ShowWarning(TypesWarning.ErrorMessage, txtResultado.ToString)
		'Else
		'	ShowWarning(TypesWarning.Information, txtResultado.ToString)
		'End If
	End Sub

	Sub iQuoteTestHoras()

		Dim driver As New ChromeDriver


		driver.Navigate.GoToUrl("http://nwb.ipsos.com/")

		Dim userMatrix = driver.FindElement(By.Name("UserName"))
		Dim passMatrix = driver.FindElement(By.Name("Password"))
		Dim btnEnter = driver.FindElement(By.Id("wsfLogonButton"))
		userMatrix.SendKeys("Paola.Gutierrez")
		passMatrix.SendKeys("Ipsos*235")
		'passMatrix.Submit()
		btnEnter.Click()

		' ---> Búsqueda de propuesta
		driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#1175745-B1")

		' ---> Nueva estimación
		driver.ExecuteScript("javascript:jsDetailGlobalProfTimeAdd();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))




		Dim NumIQuote As String
		NumIQuote = driver.Url.Split("#")(1)
		'driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#" & NumIQuote)

		driver.FindElement(By.Id("o503")).SendKeys("Descripción del presupuesto") ' Descripción de presupuesto
		driver.FindElement(By.Id("o511")).SendKeys("Número de alternativa") ' Descripción de versión

		' ---> SCRIPTING <---
		driver.ExecuteScript("jsDialogDirectScriptingWaveNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o36230a")).Clear()
		driver.FindElement(By.Id("o36230a")).SendKeys("Fase 1") ' Nombre de la fase
		driver.ExecuteScript("jsSaveLinkClick(36);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))

		'		Setup en minutos
		driver.FindElement(By.Id("o36280")).Clear()
		driver.FindElement(By.Id("o36280")).SendKeys("30")
		driver.FindElement(By.Id("o36380_comment")).SendKeys("") ' Comentarios
		'		Tipo de scripting
		Dim Selectionddl As New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o36380")))
		Selectionddl.SelectByIndex(1) 'Data Collection Script
		'		Número de paises
		driver.FindElement(By.Id("o36270")).Clear()
		driver.FindElement(By.Id("o36270")).SendKeys("1")
		driver.FindElement(By.Id("o36270_comment")).SendKeys("") ' Comentarios
		'		Setup Survey en minutos
		driver.FindElement(By.Id("o36280")).Clear()
		driver.FindElement(By.Id("o36280")).SendKeys("20")
		driver.FindElement(By.Id("o36280_comment")).SendKeys("") ' Comentarios
		'		Complejidad del scripting
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o36295")))
		Selectionddl.SelectByIndex(1)
		driver.FindElement(By.Id("o36295_comment")).SendKeys("") ' Comentarios 
		'		Es Online? No
		driver.FindElement(By.Id("o36300_0")).Click()
		'		Telefónico? Sí
		driver.FindElement(By.Id("o36305_1")).Click()
		driver.ExecuteScript("jsScriptingTelephoneRequired();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o36456")))
		Selectionddl.SelectByIndex(1) 'No quotas
		'		Es F2F? No
		driver.FindElement(By.Id("o36310_0")).Click()
		'	GUARDAR SCRIPTING
		driver.FindElement(By.Id("o36375_comment")).SendKeys("") ' Otros Comentarios generales
		driver.ExecuteScript("jsSectionSave_click(36);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogDirectScriptingWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		'driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#proposal#" & NumIQuote)


		' ---> TELEFÓNICO <---
		driver.ExecuteScript("javascript:jsDialogDirectTelephoneWaveNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o20230a")).Clear()
		driver.FindElement(By.Id("o20230a")).SendKeys("Fase 1") ' Nombre de la fase
		driver.ExecuteScript("javascript:jsSaveLinkClick(20);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Es Tracking? Sí
		driver.FindElement(By.Id("o20240_1")).Click()
		driver.ExecuteScript("jsTelephoneIsTracking();")
		driver.FindElement(By.Id("o20242")).Clear()
		driver.FindElement(By.Id("o20242")).SendKeys("2") 'Número de olas
		'		Tema
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20244")))
		Selectionddl.SelectByIndex(2) 'Consumidor mediano interés
		driver.FindElement(By.Id("o20244_comment")).SendKeys("") 'Comentarios tema o grupo objetivo
		'		Días en campo
		driver.FindElement(By.Id("o20250")).Clear()
		driver.FindElement(By.Id("o20250")).SendKeys("5") 'Días en campo
		driver.FindElement(By.Id("o20250_comment")).SendKeys("") 'Comentarios días en campo
		'		Quotas
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20291")))
		driver.FindElement(By.Id("o20291_comment")).SendKeys("") 'Comentarios cuotas
		Selectionddl.SelectByIndex(1) 'Tipo de control de cuota -> No Cuota
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20293")))
		Selectionddl.SelectByIndex(1) 'Número de celdas de cuotas -> No Cuota
		'	GUARDAR TELEFÓNICO
		driver.ExecuteScript("jsSectionSave_click(20);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'DETALLE 
		driver.ExecuteScript("javascript:jsDialogDirectTelephoneSampleNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		'		Número de encuestas
		driver.FindElement(By.Id("o20930")).SendKeys("100")
		'		Tipo de respondiente
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20940")))
		Selectionddl.SelectByIndex(1) 'Consumidor individual
		driver.FindElement(By.Id("o20940_comment")).SendKeys("n/a") 'Comentarios 
		'		Selección respondientes
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20945")))
		Selectionddl.SelectByIndex(1) 'Quota selection
		driver.FindElement(By.Id("o20945_comment")).SendKeys("n/a") 'Comentarios 
		'		Origen de muestra
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20965")))
		Selectionddl.SelectByIndex(2) 'Client supplied
		driver.FindElement(By.Id("o20965_comment")).SendKeys("n/a") 'Comentarios 
		'		Disponibilidad de muestra
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20970")))
		Selectionddl.SelectByIndex(3) '7:1 to 10:1
		driver.FindElement(By.Id("o20970_comment")).SendKeys("n/a") 'Comentarios 
		'		Incidencia
		driver.FindElement(By.Id("o20925")).SendKeys("50") 'Comentarios 
		driver.FindElement(By.Id("o20925_comment")).SendKeys("n/a") 'Comentarios 
		'		Screener Lenght
		driver.FindElement(By.Id("o20980")).Clear()
		driver.FindElement(By.Id("o20980")).SendKeys("2")
		'		Duración encuesta
		driver.FindElement(By.Id("o20935")).SendKeys("15")
		'		Identificación cliente
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20985")))
		Selectionddl.SelectByIndex(1) 'None / Unsure
		driver.FindElement(By.Id("o20985_comment")).SendKeys("n/a") 'Comentarios 
		driver.ExecuteScript("jsSectionSave_click(200);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogDirectTelephoneSampleDialogClose();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		'	GUARDAR TELEFÓNICO
		driver.ExecuteScript("jsSectionSave_click(20);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))

		driver.ExecuteScript("jsDialogDirectTelephoneWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))


		' ---> F2F <---
		driver.ExecuteScript("javascript:jsDialogDirectF2FWaveNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o39230a")).Clear()
		driver.FindElement(By.Id("o39230a")).SendKeys("Fase 1") ' Nombre de la fase
		driver.ExecuteScript("javascript:jsSaveLinkClick(39);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Es Tracking? Sí
		driver.FindElement(By.Id("o39240_1")).Click()
		driver.ExecuteScript("jsF2FIsTracking();")
		driver.FindElement(By.Id("o39242")).Clear()
		driver.FindElement(By.Id("o39242")).SendKeys("2") 'Número de olas
		'		Tema
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39244")))
		Selectionddl.SelectByIndex(2) 'Consumidor mediano interés
		driver.FindElement(By.Id("o39244_comment")).SendKeys("n/a") 'Comentarios tema o grupo objetivo
		'		Es Product Test? Sí
		driver.FindElement(By.Id("o39302_1")).Click()
		driver.ExecuteScript("jsF2FIsThisProductChange();")
		'		Requiere compra de producto? Sí
		driver.FindElement(By.Id("o39250_1")).Click()
		driver.FindElement(By.Id("o39250_comment")).SendKeys("n/a") 'Comentarios compra de producto
		'		Tipo de producto
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39251")))
		Selectionddl.SelectByIndex(1) 'Small
		'		Productos por respondiente
		driver.FindElement(By.Id("o39252")).SendKeys("1") 'Comentarios tema o grupo objetivo
		'	GUARDAR F2F
		driver.ExecuteScript("jsSectionSave_click(39);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Especificaciones F2F - Stage
		driver.ExecuteScript("jsDialogDirectF2FStageNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o39910")).SendKeys("Main Interview") 'Nombre stage
		'		Stage
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39915")))
		Selectionddl.SelectByIndex(1) 'Main interview
		'		Data collection mode
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39948")))
		Selectionddl.SelectByIndex(1) 'Data collection mode - iField
		'		Duración días
		driver.FindElement(By.Id("o39955")).SendKeys("10") 'Número de días en campo
		driver.FindElement(By.Id("o39960")).SendKeys("10") 'Número de días iField
		'		Sample Strategy
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39878")))
		Selectionddl.SelectByIndex(2) 'Free find
		'		Geographical coverage
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39874")))
		Selectionddl.SelectByIndex(1) 'National coverage
		'		Target group name
		driver.FindElement(By.Id("iqtMain_sample001_1")).SendKeys("General") '
		'		Respondent type
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("iqtMain_sample002_1")))
		Selectionddl.SelectByIndex(2) 'Consumers individuals
		'		Respondent selection
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("iqtMain_sample003_1")))
		Selectionddl.SelectByIndex(1) 'Quota selection
		'		Cantidad e incidencia
		driver.FindElement(By.Id("iqtMain_sample004_1")).SendKeys("100") 'Número de encustas
		driver.FindElement(By.Id("iqtMain_sample005_1")).SendKeys("40") 'Gen Pop Incidence
		driver.FindElement(By.Id("iqtMain_sample006_1")).SendKeys("50") 'Target incidence
		'		Quota control
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("iqtMain_sample007_1")))
		Selectionddl.SelectByIndex(1) 'Parallel flexible quota
		'		Quota categories
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("iqtMain_sample008_1")))
		Selectionddl.SelectByIndex(1) 'Standar quotas
		'		Duración
		driver.FindElement(By.Id("iqtMain_sample009_1")).SendKeys("40") 'Screener lenght
		driver.FindElement(By.Id("iqtMain_sample010_1")).SendKeys("50") 'Interview lenght

		driver.ExecuteScript("jsDialogDirectF2FStageSet(false,'none',0);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))

		driver.ExecuteScript("jsDialogDirectF2FStageDialogClose();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))


		driver.ExecuteScript("jsSectionSave_click(39);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))

		driver.ExecuteScript("jsDialogDirectF2FWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		'driver.Manage.Window.Minimize()


		' ---> CODIFICACIÓN <---
		driver.ExecuteScript("javascript:jsDialogDirectCodingWaveNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o22233")).Clear()
		driver.FindElement(By.Id("o22233")).SendKeys("Fase 1") ' Nombre de la fase
		driver.ExecuteScript("javascript:jsSaveLinkClick(22);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Es Tracking? Sí
		driver.FindElement(By.Id("o22240_1")).Click()
		driver.ExecuteScript("jsF2FIsTracking();")
		driver.FindElement(By.Id("o22242")).Clear()
		driver.FindElement(By.Id("o22242")).SendKeys("2") 'Número de olas
		'		Número de encuestas
		driver.FindElement(By.Id("o22245")).SendKeys("100")
		'		Complejidad de la codificación
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o22246")))
		Selectionddl.SelectByIndex(2) 'Standard
		driver.FindElement(By.Id("o22246_comment")).SendKeys("n/a") 'Comentarios 
		'		Preguntas abiertas múltiples
		driver.FindElement(By.Id("o22250")).SendKeys("20")
		'		Complejidad de la codificación (cleaner preguntas)
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o22252")))
		Selectionddl.SelectByIndex(2) 'Standard
		'		Preguntas abiertas
		driver.FindElement(By.Id("o22254")).SendKeys("50")
		'		Preguntas otros
		driver.FindElement(By.Id("o22258")).SendKeys("20")
		driver.ExecuteScript("jsSectionSave_click(22);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogDirectCodingWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))


		' ---> DP <---
		driver.ExecuteScript("javascript:jsDialogDirectDataProcessingWaveNew();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.FindElement(By.Id("o21230a")).Clear()
		driver.FindElement(By.Id("o21230a")).SendKeys("Fase 1") ' Nombre de la fase
		driver.ExecuteScript("javascript:jsSaveLinkClick(21);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Es Tracking? Sí
		driver.FindElement(By.Id("o21250_1")).Click()
		driver.ExecuteScript("jsDataProcessingIsTracking();")
		driver.FindElement(By.Id("oDataProcessingsw_none")).SendKeys("1") 'Cambios
		'		Tipo de Ola
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o21255")))
		Selectionddl.SelectByIndex(1) 'Setup wave / New study
		driver.FindElement(By.Id("o21255_comment")).SendKeys("n/a") 'Comentarios 

		driver.FindElement(By.Id("o21260")).Clear()
		driver.FindElement(By.Id("o21260")).SendKeys("2") 'Número de olas
		'		Origen de la data
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o21290")))
		Selectionddl.SelectByIndex(1) 'Ipsos OPS Collected Data
		driver.FindElement(By.Id("o21290_comment")).SendKeys("n/a") 'Comentarios 
		'		Survey Setup length
		driver.FindElement(By.Id("o21280")).SendKeys("20")
		'		Requiere datacleaning? Sí
		driver.FindElement(By.Id("o21285_1")).Click()
		driver.ExecuteScript("jsDataProcessingIsTracking();")
		'		Complejidad de tablas
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o21295")))
		Selectionddl.SelectByIndex(2) 'Standard
		driver.FindElement(By.Id("o21295_comment")).SendKeys("n/a") 'Comentarios 
		'		Es CATI? SI
		driver.FindElement(By.Id("o21315_1")).Click()
		'		Es ONLINE? SI
		driver.FindElement(By.Id("o21320_1")).Click()
		'		Es HandHeld? SI
		driver.FindElement(By.Id("o21335_1")).Click()
		driver.ExecuteScript("jsSectionSave_click(21);")
		Thread.Sleep(TimeSpan.FromSeconds(5))
		driver.ExecuteScript("jsDialogDirectDataProcessingWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(2))
		driver.ExecuteScript("jsSectionSave_click(0);")
	End Sub


#End Region

	'Protected Sub btnProfessionalTime_Click(sender As Object, e As EventArgs)
	'	gvProfessionalTime.DataSource = ObtenerHorasProfesionales(hfPropuesta.Value, ddlAlternativa.SelectedValue)
	'	gvProfessionalTime.DataBind()
	'	ModalPopupExtenderPT.Show()
	'End Sub

	Protected Sub btnCalcProfessionalTime_Click(sender As Object, e As EventArgs)
		Dim oCot As New CoreProject.Cotizador.General
		oCot.PUTCalculoHorasProfesionales(hfPropuesta.Value, ddlAlternativa.SelectedValue, Nothing, Nothing)
		ShowWarning(TypesWarning.Information, "La información de horas profesionales ha sido calculada para todos los presupuestos")
	End Sub

	Private Sub gvPYGAlternativa_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPYGAlternativa.RowDataBound
		If e.Row.RowType = DataControlRowType.DataRow Then
			If IsNumeric(e.Row.Cells(1).Text) Then
				If gvPYGAlternativa.DataKeys(e.Row.RowIndex).Value = 6 Or gvPYGAlternativa.DataKeys(e.Row.RowIndex).Value = 11 Then
					e.Row.Cells(1).Text = FormatPercent(e.Row.Cells(1).Text, 2)
				Else
					e.Row.Cells(1).Text = FormatCurrency(e.Row.Cells(1).Text, 0)
				End If
			End If
		End If
	End Sub

	Private Sub gvPYGPresupuesto_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPYGPresupuesto.RowDataBound
		If e.Row.RowType = DataControlRowType.DataRow Then
			If IsNumeric(e.Row.Cells(1).Text) Then
				If gvPYGPresupuesto.DataKeys(e.Row.RowIndex).Value = 6 Or gvPYGPresupuesto.DataKeys(e.Row.RowIndex).Value = 11 Then
					e.Row.Cells(1).Text = FormatPercent(e.Row.Cells(1).Text, 2)
				Else
					e.Row.Cells(1).Text = FormatCurrency(e.Row.Cells(1).Text, 0)
				End If
			End If
		End If
	End Sub


	'Protected Sub btnCalcularHorasProfesionalesPresupuesto_Click(sender As Object, e As EventArgs)
	'	Dim oCot As New CoreProject.Cotizador.General
	'	oCot.PUTCalculoHorasProfesionales(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue)
	'	DirectCast(UCHeader.FindControl("gvProfessionalTime"), GridView).DataSource = ObtenerHorasProfesionales(hfPropuesta.Value, ddlAlternativa.SelectedValue, ddlMetodologia.SelectedValue, ddlFase.SelectedValue)
	'	DirectCast(UCHeader.FindControl("gvProfessionalTime"), GridView).DataBind()
	'End Sub

	Sub CargarAlternativasDisponibles(ByVal IdPropuesta As Int64)
		Dim o As New IQ.Transacciones
		Me.ddlAlternativaToCopy.DataSource = o.AlternativasDisponibles(IdPropuesta)
		'Me.ddlAlternativa.DataTextField = "Alternativa"
		'Me.ddlAlternativa.DataValueField = "Alternativa"
		Me.ddlAlternativaToCopy.DataBind()
		CargarFasesDisponibles(IdPropuesta, ddlAlternativaToCopy.SelectedValue, hfMetCodigoCopiar.Value)
	End Sub

	Sub CargarFasesDisponibles(ByVal IdPropuesta As Int64, ByVal Alternativa As Int64, ByVal Metodologia As Int32)
		Dim o As New IQ.Transacciones
		Me.ddlFaseToCopy.DataSource = o.FasesDisponibles(IdPropuesta, Alternativa, Metodologia)
		Me.ddlFaseToCopy.DataTextField = "Fase"
		Me.ddlFaseToCopy.DataValueField = "id"
		Me.ddlFaseToCopy.DataBind()
	End Sub

	Sub CargarDatosSimulador(ByVal idPropuesta As Int64?, Alternativa As Integer?, Metodologia As Integer?, Fase As Integer?, TipoCalculo As Integer, VrVenta As Decimal?, GM As Decimal?, OP As Decimal?, GMOps As Decimal?)
		Dim oCot As New CoreProject.Cotizador.General
		Dim info = oCot.GetSimulador(idPropuesta, Alternativa, Metodologia, Fase, TipoCalculo, VrVenta, GM, OP, GMOps)
		ModalPopupExtenderSimulator.Show()
		hfSimPropuesta.Value = idPropuesta
		hfSimAlternativa.Value = Alternativa
		hfSimMetodologia.Value = Metodologia
		hfSimFase.Value = Fase
		hfGMMin.Value = info.GMMin
		hfVrVenta.Value = info.venta
		hfMargenBruto.Value = info.margenbruto
		lblSIMCargosGrupo.Text = FormatCurrency(info.cargosgrupo, 0, TriState.True)
		lblSIMCostoOPS.Text = FormatCurrency(info.costoops, 0, TriState.True)
		lblSIMCostosDirectos.Text = FormatCurrency(info.costosdirectos, 0, TriState.True)
		lblSIMGM.Text = FormatPercent(info.grossmargin, 2)
		lblSIMGMOPS.Text = FormatPercent(info.GMOps, 2)
		lblSIMMargenBruto.Text = FormatCurrency(info.margenbruto, 0, TriState.True)
		lblSIMOP.Text = FormatCurrency(info.OP, 0, TriState.True)
		lblSIMOPPercent.Text = FormatPercent(info.PercentOp, True)
		lblSIMOtrosCostos.Text = FormatCurrency(info.otroscostos, 0, TriState.True)
		lblSIMProfessionalTime.Text = FormatCurrency(info.proftimes, 0, TriState.True)
		lblSIMTargetProfessionalTime.Text = FormatCurrency(info.targetpt, 0, TriState.True)
		lblSIMVrVenta.Text = FormatCurrency(Math.Round(CDbl(info.venta), 0), 0, TriState.True)
		'If TipoCalculo = 1 Then
		txtVentaSimular.Text = Math.Round(CDbl(info.venta), 0)
		txtGMSimular.Text = FormatNumber(info.grossmargin * 100, 2)
		txtOPSimular.Text = FormatNumber(info.PercentOp * 100, 2)
		txtGMOPSSimular.Text = FormatNumber(info.GMOps * 100, 2)
		'End If
		If info.grossmargin >= hfGMMin.Value And info.PercentOp > 0.15 And info.GMOps >= 0.2145 Then
			btnAdjustment.Visible = True
			btnRequestAdjustment.Visible = False
			txtComentariosSolicitud.Visible = False
		Else
			btnAdjustment.Visible = False
			btnRequestAdjustment.Visible = True
			txtComentariosSolicitud.Visible = True
		End If
		gvSolicitudes.DataSource = oCot.GetSolicitudesSimulador(Nothing, idPropuesta, Alternativa, Metodologia, Fase, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
		gvSolicitudes.DataBind()
	End Sub

	Protected Sub ddlAlternativaToCopy_SelectedIndexChanged(sender As Object, e As EventArgs)
		CargarFasesDisponibles(hfPropuesta.Value, ddlAlternativaToCopy.SelectedValue, hfMetCodigoCopiar.Value)
	End Sub

	Protected Sub btnOkCopiarAlternativa_Click(sender As Object, e As EventArgs)
		Dim o As New IQ.Transacciones
		o.CopiarPresupuesto(hfPropuesta.Value, ddlAlternativa.SelectedValue, hfMetCodigoCopiar.Value, hfFaseCopiar.Value, ddlAlternativaToCopy.SelectedValue, ddlFaseToCopy.SelectedValue)
		ShowWarning(TypesWarning.Information, "Se ha copiado el presupuesto")
		CargarAlternativa(ddlAlternativa.SelectedValue)
		CargarAlternativasDisponibles(hfPropuesta.Value)
		'ModalPopupExtenderCopiarPresupuesto.Hide()
	End Sub

	Protected Sub btnCancelCopiar_Click(sender As Object, e As EventArgs)
		ModalPopupExtenderCopiarPresupuesto.Hide()
	End Sub

	Protected Sub btnImportarMuestraExcel_Click(sender As Object, e As EventArgs)
		ModalPopupExtenderExcelMuestra.Show()
	End Sub

	Protected Sub btnLoadDataExcel_Click(sender As Object, e As EventArgs)
		If FUploadExcelMuestra.HasFile Then
			Dim path As [String] = Server.MapPath("~/Files/")
			Dim fileload As New System.IO.FileInfo(FUploadExcelMuestra.FileName)
			FUploadExcelMuestra.SaveAs(path & "Muestra_" & hfPropuesta.Value.ToString & ".xlsx")
			Dim connstr As String = "Provider=Microsoft.ACE.OLEDB.12.0;" & "Data Source=" & path & "Muestra_" & hfPropuesta.Value.ToString & ".xlsx;" & "Extended Properties='Excel 12.0'"
			Dim sqlcmd As String = "SELECT * FROM [" & ddlHojaArchivo.SelectedValue & "$I14:I264]"
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
			' Verifica q
			If dt.Rows.Count = 0 Then
				ShowWarning(TypesWarning.ErrorMessage, "No se encontraron registros")
				Exit Sub
			Else
				Dim oCot As New Cotizador.GeneralDapper
				For Each row In dt.Rows
					Try
						If Not row(0).ToString() = "" Then oCot.PUT_EjecutarComando(row(0).ToString())
					Catch ex As Exception
					End Try
				Next
			End If
			ShowWarning(TypesWarning.Information, "Se han ejecutado las importaciones")
			Try
				System.IO.File.Delete(path & "Muestra_" & hfPropuesta.Value.ToString & ".xlsx")
			Catch ex As Exception
			End Try
		Else
			ShowWarning(TypesWarning.ErrorMessage, "Debe seleccionar un archivo antes de continuar")
		End If
		CargarAlternativa(ddlAlternativa.SelectedValue)

	End Sub

	Protected Sub rbSearch_SelectedIndexChanged(sender As Object, e As EventArgs)
		txtVentaSimular.Enabled = False
		txtGMSimular.Enabled = False
		txtOPSimular.Enabled = False
		Select Case rbSearch.SelectedValue
			Case 1
				txtVentaSimular.Enabled = True
			Case 2
				txtGMSimular.Enabled = True
			Case 3
				txtOPSimular.Enabled = True
		End Select
	End Sub

	Protected Sub btnSimularExec_Click(sender As Object, e As EventArgs)
		Dim VrVenta As Decimal?
		Dim GM As Decimal?
		Dim OP As Decimal?
		Dim GMOps As Decimal?
		Dim TipoCalculo As Integer = 1
		Select Case rbSearch.SelectedValue
			Case 1 ' Venta
				If IsNumeric(txtVentaSimular.Text) Then VrVenta = txtVentaSimular.Text
				TipoCalculo = 2
			Case 2 ' GM
				If IsNumeric(txtGMSimular.Text) Then GM = Decimal.Parse(txtGMSimular.Text) / 100
				TipoCalculo = 4
			Case 3 ' OP
				If IsNumeric(txtOPSimular.Text) Then OP = Decimal.Parse(txtOPSimular.Text) / 100
				TipoCalculo = 3
		End Select
		If IsNumeric(txtGMOPSSimular.Text) Then GMOps = Decimal.Parse(txtGMOPSSimular.Text) / 100
		CargarDatosSimulador(hfSimPropuesta.Value, hfSimAlternativa.Value, hfSimMetodologia.Value, hfSimFase.Value, TipoCalculo, VrVenta, GM, OP, GMOps)
	End Sub

	Protected Sub btnAdjustment_Click(sender As Object, e As EventArgs)
		If Not (IsNumeric(txtVentaSimular.Text)) Or Not (IsNumeric(txtGMSimular.Text)) Or Not (IsNumeric(txtOPSimular.Text)) Or Not (IsNumeric(txtGMOPSSimular.Text)) Then
			ShowWarning(TypesWarning.ErrorMessage, "Faltan valores por confirmar. Diligencie los datos, haga clic en simular e intente nuevamente")
			Exit Sub
		End If
		If ((Decimal.Parse(txtOPSimular.Text) / 100) < 0.15) Or ((Decimal.Parse(txtGMSimular.Text) / 100) < hfGMMin.Value) Or ((Decimal.Parse(txtGMOPSSimular.Text) / 100) < 0.2145) Then
			ShowWarning(TypesWarning.ErrorMessage, "Algo no está correcto. Luego de hacer el ajuste haga click en simular antes de intentar el ajuste")
			Exit Sub
		End If

		Dim oCot As New CoreProject.Cotizador.General
		Dim ValorVenta As Double = txtVentaSimular.Text
		oCot.GetSimularVenta(hfSimPropuesta.Value, hfSimAlternativa.Value, hfSimMetodologia.Value, hfSimFase.Value, Decimal.Parse(txtGMSimular.Text) / 100, Decimal.Parse(txtGMOPSSimular.Text) / 100, 0)
		If Not rbSearch.SelectedValue = "" Then
			If rbSearch.SelectedValue = 1 Then
				oCot.GetSimularGM(hfSimPropuesta.Value, hfSimAlternativa.Value, hfSimMetodologia.Value, hfSimFase.Value, ValorVenta, 0)
			End If
		End If
		CargarDatosSimulador(hfSimPropuesta.Value, hfSimAlternativa.Value, hfSimMetodologia.Value, hfSimFase.Value, 1, Nothing, Nothing, Nothing, Nothing)
		ShowWarning(TypesWarning.Information, "Han sido realizados los ajustes")

	End Sub

	Protected Sub btnRequestAdjustment_Click(sender As Object, e As EventArgs)
		Dim oCot As New CoreProject.Cotizador.General
		Dim IQS As New IQ_SolicitudesAjustesPresupuesto
		Select Case rbSearch.SelectedValue
			Case 1 ' Venta
				IQS.TipoSolicitud = 2
			Case 2 ' GM
				IQS.TipoSolicitud = 4
			Case 3 ' OP
				IQS.TipoSolicitud = 3
		End Select
		IQS.IdPropuesta = hfSimPropuesta.Value
		IQS.ParAlternativa = hfSimAlternativa.Value
		IQS.MetCodigo = hfSimMetodologia.Value
		IQS.ParNacional = hfSimFase.Value
		IQS.ValorVenta = txtVentaSimular.Text
		IQS.GM = Decimal.Parse(txtGMSimular.Text) / 100
		IQS.GMOPS = Decimal.Parse(txtGMOPSSimular.Text) / 100
		IQS.OP = Decimal.Parse(txtOPSimular.Text) / 100
		IQS.SolicitadoPor = Session("IDUsuario").ToString
		IQS.ComentariosSolicitud = txtComentariosSolicitud.Text
		IQS.FechaSolicitud = Date.UtcNow.AddHours(-5)
		oCot.PutSolicitud(IQS)
		EnviarEmailSolicitudSimulador(IQS.IdPropuesta, IQS.id)
		ShowWarning(TypesWarning.Information, "Ha sido enviada la solicitud de modificación del presupuesto")
		gvSolicitudes.DataSource = oCot.GetSolicitudesSimulador(Nothing, hfSimPropuesta.Value, hfSimAlternativa.Value, hfSimMetodologia.Value, hfSimFase.Value, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
		gvSolicitudes.DataBind()
	End Sub

	Protected Sub btnRecalcularHoras_Click(sender As Object, e As EventArgs)
		Dim oCot As New CoreProject.Cotizador.General
		oCot.PUTCalculoHorasProfesionalesVenta(hfSimPropuesta.Value, hfSimAlternativa.Value, hfSimMetodologia.Value, hfSimFase.Value, hfMargenBruto.Value)

		Dim VrVenta As Decimal?
		Dim GM As Decimal?
		Dim OP As Decimal?
		Dim GMOps As Decimal?
		Dim TipoCalculo As Integer = 1
		Select Case rbSearch.SelectedValue
			Case 1 ' Venta
				If IsNumeric(txtVentaSimular.Text) Then VrVenta = txtVentaSimular.Text
				TipoCalculo = 2
			Case 2 ' GM
				If IsNumeric(txtGMSimular.Text) Then GM = Decimal.Parse(txtGMSimular.Text) / 100
				TipoCalculo = 4
			Case 3 ' OP
				If IsNumeric(txtOPSimular.Text) Then OP = Decimal.Parse(txtOPSimular.Text) / 100
				TipoCalculo = 3
		End Select
		If IsNumeric(txtGMOPSSimular.Text) Then GMOps = Decimal.Parse(txtGMOPSSimular.Text) / 100
		CargarDatosSimulador(hfSimPropuesta.Value, hfSimAlternativa.Value, hfSimMetodologia.Value, hfSimFase.Value, TipoCalculo, VrVenta, GM, OP, GMOps)
	End Sub
End Class