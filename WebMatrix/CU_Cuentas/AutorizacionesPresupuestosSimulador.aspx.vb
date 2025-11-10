Imports CoreProject
Public Class _AutorizacionesPresupuestosSimulador
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			ConsultarInfoInicial()
		End If
	End Sub

	Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
		Dim permisos As New Datos.ClsPermisosUsuarios
		If permisos.VerificarPermisoUsuario(155, Session("IDUsuario").ToString()) = False Then
			Response.Redirect("../Home/home.aspx")
		End If
	End Sub

	Protected Sub btnSearch_Click(sender As Object, e As EventArgs)
		Dim oCot As New Cotizador.General
		Dim idPropuesta As Int64?
		Dim Estado As Boolean?
		Dim SL As Integer?
		Dim Fini As Date?
		Dim Ffin As Date?
		If IsNumeric(txtPropuesta.Text) Then idPropuesta = txtPropuesta.Text
		If IsDate(txtFInicio.Text) Then Fini = txtFInicio.Text
		If IsDate(txtFFin.Text) Then Ffin = txtFFin.Text
		If rbSearch.SelectedValue = 2 Then Estado = True
		If rbSearch.SelectedValue = 3 Then Estado = False
		If ddlSL.SelectedIndex > 0 Then SL = ddlSL.SelectedValue
		gvDataSearch.DataSource = oCot.GetSolicitudesSimulador(Nothing, idPropuesta, Nothing, Nothing, Nothing, Estado, SL, Fini, Ffin, Nothing, Nothing, Nothing)
		gvDataSearch.DataBind()
	End Sub

	Sub ConsultarInfoInicial()
		Dim oCot As New Cotizador.General
		gvDataSearch.DataSource = oCot.GetSolicitudesSimulador(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
		gvDataSearch.DataBind()
	End Sub

	Protected Sub gvDataSearch_RowCommand(sender As Object, e As GridViewCommandEventArgs)
		Select Case e.CommandName
			Case "OpenRequest"
				Dim oCot As New CoreProject.Cotizador.General
				Dim info = oCot.GetSolicitudSimulador(Int64.Parse(gvDataSearch.DataKeys(CInt(e.CommandArgument))("ID")))
				Dim TipoSolicitud = info.TipoSolicitud
				If TipoSolicitud = 0 Then TipoSolicitud = 3
				CargarDatosSimulador(info.IdPropuesta, info.ParAlternativa, info.MetCodigo, info.ParNacional, TipoSolicitud, info.ValorVenta, info.GM, info.OP, info.GMOPS, info)
		End Select
	End Sub

	Sub CargarDatosSimulador(ByVal idPropuesta As Int64?, Alternativa As Integer?, Metodologia As Integer?, Fase As Integer?, TipoCalculo As Integer, VrVenta As Decimal?, GM As Decimal?, OP As Decimal?, GMOps As Decimal?, IQSolicitud As IQ_SolicitudesAjustesPresupuesto)
		Dim oCot As New CoreProject.Cotizador.General
		Dim info = oCot.GetSimulador(idPropuesta, Alternativa, Metodologia, Fase, TipoCalculo, VrVenta, GM, OP, GMOps)
		ModalPopupExtenderSimulator.Show()
		hfIdSolicitud.Value = IQSolicitud.id
		hfSimPropuesta.Value = idPropuesta
		hfSimAlternativa.Value = Alternativa
		hfSimMetodologia.Value = Metodologia
		hfSimFase.Value = Fase
		hfGMMin.Value = info.GMMin
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

		Dim infoSolicitud = oCot.GetSolicitudesSimulador(IQSolicitud.id, Nothing, Nothing, Nothing, Nothing, IQSolicitud.Aprobado, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
		txtDiasCampo.Text = infoSolicitud.DiasCampo
		txtDiasDiseno.Text = infoSolicitud.DiasDiseno
		txtDiasInformes.Text = infoSolicitud.DiasInformes
		txtDiasProcesamiento.Text = infoSolicitud.DiasProcesamiento
		txtObservaciones.Text = infoSolicitud.ComentariosSolicitud


	End Sub
	Sub EnviarEmailSolicitudSimulador(propuestaId As Int64, idsolicitud As Int64)
		Dim oEnviarCorreo As New EnviarCorreo
		Try
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/RespuestaSolicitudAjuste.aspx?idPropuesta=" & propuestaId.ToString & "&idSolicitud=" & idsolicitud)
		Catch ex As Exception
		End Try
	End Sub

	Protected Sub btnAprobarSolicitud_Click(sender As Object, e As EventArgs)
		Dim oCot As New CoreProject.Cotizador.General
		Dim IQS = oCot.GetSolicitudSimulador(hfIdSolicitud.Value)
		IQS.Aprobado = True
		IQS.ComentariosAprobacion = txtComentariosAprobacion.Text
		IQS.AprobadoPor = Session("IDUsuario").ToString
		oCot.GetSimularVenta(hfSimPropuesta.Value, hfSimAlternativa.Value, hfSimMetodologia.Value, hfSimFase.Value, IQS.GM, IQS.GMOPS, 0)
		If IQS.TipoSolicitud = 2 Then
			oCot.GetSimularGM(hfSimPropuesta.Value, hfSimAlternativa.Value, hfSimMetodologia.Value, hfSimFase.Value, IQS.ValorVenta, 0)
		End If
		oCot.PutSolicitud(IQS)
		EnviarEmailSolicitudSimulador(IQS.IdPropuesta, IQS.id)
		ConsultarInfoInicial()
		ModalPopupExtenderSimulator.Hide()
	End Sub

	Protected Sub btnRechazarSolicitud_Click(sender As Object, e As EventArgs)
		Dim oCot As New CoreProject.Cotizador.General
		Dim IQS = oCot.GetSolicitudSimulador(hfIdSolicitud.Value)
		IQS.Aprobado = False
		IQS.ComentariosAprobacion = txtComentariosAprobacion.Text
		IQS.AprobadoPor = Session("IDUsuario").ToString
		oCot.PutSolicitud(IQS)
		EnviarEmailSolicitudSimulador(IQS.IdPropuesta, IQS.id)
		ConsultarInfoInicial()
		ModalPopupExtenderSimulator.Hide()
	End Sub
End Class