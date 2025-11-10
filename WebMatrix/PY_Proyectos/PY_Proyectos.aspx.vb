Imports CoreProject
Imports WebMatrix.Util
Public Class Form_PY_Proyectos
	Inherits System.Web.UI.Page
	Private oMatrixContext As CORE_Entities
#Region "Propiedades"
	Private _IdUsuario As Int64
	Public Property IdUsuario() As Int64
		Get
			Return _IdUsuario
		End Get
		Set(ByVal value As Int64)
			_IdUsuario = value
		End Set
	End Property
#End Region

#Region "Eventos"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		IdUsuario = Session("IdUsuario")
		Session("TrabajoId") = Nothing
		If Not IsPostBack Then
			Dim permisos As New Datos.ClsPermisosUsuarios
			Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
			If permisos.VerificarPermisoUsuario(38, UsuarioID) = False Then
				Response.Redirect("../PY_Proyectos/Default.aspx")
			End If
			'cargarProyectos()
			buscar()
			lbtnVolver.PostBackUrl = "~/PY_Proyectos/Default.aspx"
		End If
	End Sub

	Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
		buscar()
	End Sub

	Private Sub gvProyectos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvProyectos.PageIndexChanging
		gvProyectos.PageIndex = e.NewPageIndex
		cargarProyectos()
	End Sub

	Private Sub gvProyectos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvProyectos.RowCommand
		If e.CommandName = "Trabajos" Then
			Session("ProyectoId") = Int64.Parse(gvProyectos.DataKeys(CInt(e.CommandArgument))("Id"))
			Session("TipoProyectoId") = Int64.Parse(gvProyectos.DataKeys(CInt(e.CommandArgument))("TipoProyectoId"))

			If Session("TipoProyectoId") = 1 Then
				Response.Redirect("Trabajos.aspx" & "?ProyectoId=" & Int64.Parse(gvProyectos.DataKeys(CInt(e.CommandArgument))("Id")))
			ElseIf Session("TipoProyectoId") = 2 Then
				Response.Redirect("TrabajosCualitativos.aspx" & "?ProyectoId=" & Int64.Parse(gvProyectos.DataKeys(CInt(e.CommandArgument))("Id")))
			End If

		ElseIf e.CommandName = "Informacion" Then
			Dim idproyecto = Int64.Parse(gvProyectos.DataKeys(CInt(e.CommandArgument))("Id"))
			Dim TipoProyectoId = Int64.Parse(gvProyectos.DataKeys(CInt(e.CommandArgument))("TipoProyectoId"))
			Dim oProyecto As New Proyecto
			Dim oEstudio As New Estudio
			Dim oPropuesta As New Propuesta
			Dim infoP = oProyecto.obtenerXId(idproyecto)
			Dim infoE = oEstudio.ObtenerXID(infoP.EstudioId)
			Dim infoPr = oPropuesta.DevolverxID(infoE.PropuestaId)
			hfIdProyecto.Value = idproyecto
			hfIdPropuesta.Value = infoPr.Id
			CargarInfoPropuesta()
			CargarFrame(infoPr.Brief)
			If TipoProyectoId = 1 Then
				pnlBriefCuentasProyectos.Visible = True
				pnlBriefCuentasProyectosCuali.Visible = False
				'CargarEspecificacionesProyecto()
				CargarEspecificacionesProyectoCuanti()
				cargarVersionesCuanti(hfIdProyecto.Value)
			Else
				pnlBriefCuentasProyectos.Visible = False
				pnlBriefCuentasProyectosCuali.Visible = True
				CargarEspecificacionesProyectoCuali()
				cargarVersionesCuali(hfIdProyecto.Value)
			End If
			pnlFrame.Visible = True
			pnlPropuesta.Visible = True
			accordion1.Visible = True
			accordion0.Visible = False
		End If
	End Sub

#End Region

#Region "Metodos"
	Sub cargarProyectos()
		Dim oProyecto As New Proyecto
		gvProyectos.DataSource = oProyecto.obtenerXGerenteProyectos(IdUsuario)
		gvProyectos.DataBind()
	End Sub
	Sub buscar()
		Dim oProyecto As New Proyecto
		gvProyectos.DataSource = oProyecto.obtener(IdUsuario, Me.txtBuscar.Text)
		gvProyectos.DataBind()
	End Sub

	Public Sub CargarInfo(ByVal idbrief As Int64)
		Try
			Dim oBrief As New Brief
			Dim info = oBrief.DevolverxID(idbrief)
			txtTitulo.Text = info.Titulo
			txtAntecedentes.Text = info.Antecedentes
			txtObjetivos.Text = info.Objetivos
			txtActionStandard.Text = info.ActionStandars
			txtMetodologia.Text = info.Metodologia
			txtTargetGroup.Text = info.TargetGroup
			txtTiempos.Text = info.Tiempos
			txtPresupuesto.Text = info.Presupuestos
			txtMateriales.Text = info.Materiales
			txtEstudiosAnteriores.Text = info.ResultadosAnteriores
			txtFormatos.Text = info.FormatoInforme
			txtAprobaciones.Text = info.Aprobaciones
			txtCompetencia.Text = info.Competencia
		Catch ex As Exception
			Throw ex
		End Try
	End Sub

#End Region

	Private Sub btnDescargarPropuesta_Click(sender As Object, e As System.EventArgs) Handles btnDescargarPropuesta.Click
		Response.Redirect("../GD_Documentos/GD_Documentos.aspx?IdContenedor=" & hfIdPropuesta.Value & "&IdDocumento=2&TipoAccion=2")
	End Sub

	Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
		pnlBriefCuentasProyectos.Visible = False
		pnlFrame.Visible = False
		pnlPropuesta.Visible = False
		accordion0.Visible = True
		accordion1.Visible = False
		limpiar()
	End Sub

	Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
		Dim o As New Proyecto
		Dim ent As New PY_EspCuentasCuanti
		ent = o.obtenerEspCuentasCuanti(hfIdProyecto.Value)
		Dim NoVersion = 0
		If IsNothing(ent.NoVersion) Then
			NoVersion = 1
		Else
			NoVersion = Convert.ToInt64(ent.NoVersion) + 1
		End If

		Dim TipoMedicion = ""
		If chbBCPMedicion.SelectedValue <> "" Then
			TipoMedicion = chbBCPMedicion.SelectedValue
		Else
			TipoMedicion = Nothing
		End If
		ent.ProyectoId = hfIdProyecto.Value
		ent.BCPObservaciones = txtBCPObservaciones.Text
		ent.BCPMedicion = TipoMedicion
		ent.BCPOlas = txtBCPOlas.Text
		ent.BCPPilotos = chbBCPPilotos.Checked
		ent.BCPPilotosEspecificaciones = txtBCPPilotosEspecificaciones.Text
		ent.BCPIncentivosEspecificaciones = txtBCPIncentivosEspecificaciones.Text
		ent.BCPBDDEspecificaciones = txtBCPBDDEspecificaciones.Text
		ent.BCPProductoEspecificaciones = txtBCPProductoEspecificaciones.Text
		If IsDate(txtBCPFechaBDD.Text) Then ent.BCPFechaBDD = txtBCPFechaBDD.Text
		If IsDate(txtBCPFechaConceptos.Text) Then ent.BCPFechaConceptos = txtBCPFechaConceptos.Text
		If IsDate(txtBCPFechaCuestionario.Text) Then ent.BCPFechaCuestionario = txtBCPFechaCuestionario.Text
		If IsDate(txtBCPFechaInicioCampo.Text) Then ent.BCPFechaInicioCampo = txtBCPFechaInicioCampo.Text
		If IsDate(txtBCPFechaInformeCuentas.Text) Then ent.BCPFechaInformeCuentas = txtBCPFechaInformeCuentas.Text
		If IsDate(txtBCPFechaInformeCliente.Text) Then ent.BCPFechaInformeCliente = txtBCPFechaInformeCliente.Text
		ent.NoVersion = NoVersion
		ent.Fecha = DateTime.Now.ToString
		ent.Usuario = Session("IDUsuario").ToString

		o.GuardarEspecificacionesCuentasCuantitativo(ent)
		ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
		lblVersionEspCuanti.InnerText = "Versión " + ent.NoVersion.ToString

		pnlBriefCuentasProyectos.Visible = True
		pnlBriefCuentasProyectosCuali.Visible = False
		CargarEspecificacionesProyectoCuanti()
		cargarVersionesCuanti(hfIdProyecto.Value)

	End Sub

	Sub CargarEspecificacionesProyecto()
		Dim o As New Proyecto
		Dim ent As New PY_Proyectos
		ent = o.ObtenerProyecto(hfIdProyecto.Value)
		Try
			If Not ent.BCPObservaciones Is Nothing Then txtBCPObservaciones.Text = ent.BCPObservaciones
			If Not ent.BCPMedicion Is Nothing Then chbBCPMedicion.SelectedValue = ent.BCPMedicion
			If Not ent.BCPOlas Is Nothing Then txtBCPOlas.Text = ent.BCPOlas
			If Not ent.BCPPilotos Is Nothing Then chbBCPPilotos.Checked = ent.BCPPilotos
			If Not ent.BCPPilotosEspecificaciones Is Nothing Then txtBCPPilotosEspecificaciones.Text = ent.BCPPilotosEspecificaciones
			If Not ent.BCPIncentivosEspecificaciones Is Nothing Then txtBCPIncentivosEspecificaciones.Text = ent.BCPIncentivosEspecificaciones
			If Not ent.BCPBDDEspecificaciones Is Nothing Then txtBCPBDDEspecificaciones.Text = ent.BCPBDDEspecificaciones
			If Not ent.BCPProductoEspecificaciones Is Nothing Then txtBCPProductoEspecificaciones.Text = ent.BCPProductoEspecificaciones
			If ent.BCPFechaBDD.HasValue Then txtBCPFechaBDD.Text = ent.BCPFechaBDD.ToString() Else txtBCPFechaBDD.Text = ""
			If ent.BCPFechaConceptos.HasValue Then txtBCPFechaConceptos.Text = ent.BCPFechaConceptos.ToString() Else txtBCPFechaConceptos.Text = ""
			If ent.BCPFechaCuestionario.HasValue Then txtBCPFechaCuestionario.Text = ent.BCPFechaCuestionario.ToString() Else txtBCPFechaCuestionario.Text = ""
			If ent.BCPFechaInicioCampo.HasValue Then txtBCPFechaInicioCampo.Text = ent.BCPFechaInicioCampo.ToString() Else txtBCPFechaInicioCampo.Text = ""
			If ent.BCPFechaInformeCuentas.HasValue Then txtBCPFechaInformeCuentas.Text = ent.BCPFechaInformeCuentas.ToString() Else txtBCPFechaInformeCuentas.Text = ""
			If ent.BCPFechaInformeCliente.HasValue Then txtBCPFechaInformeCliente.Text = ent.BCPFechaInformeCliente.ToString() Else txtBCPFechaInformeCliente.Text = ""
		Catch ex As Exception

		End Try
	End Sub

	Sub cargarVersionesCuanti(ByVal CodProyecto As Integer)
		Dim o As New Proyecto
		Dim ent As New List(Of PY_EspCuentasCuanti)
		ent = o.obtenerEspCuentasCuantiList(CodProyecto, 1)
		gvVerEspCuanti.DataSource = ent
		gvVerEspCuanti.DataBind()
		UPanelVersionesEspCuanti.Update()
	End Sub

	Sub cargarVersionesCuali(ByVal CodProyecto As Integer)
		Dim o As New Proyecto
		Dim ent As New List(Of PY_EspCuentasCuali)
		ent = o.obtenerEspCuentasCualiList(CodProyecto, 1)
		gvVerEspCuali.DataSource = ent
		gvVerEspCuali.DataBind()
		UPanelVersionesEspCuali.Update()
	End Sub

	Sub CargarEspecificacionesProyectoCuanti()
		Try
			Dim o As New Proyecto
			Dim ent As New PY_EspCuentasCuanti
			ent = o.obtenerEspCuentasCuanti(hfIdProyecto.Value)

			If Not (IsNothing(ent.NoVersion)) Then
				If Not ent.BCPObservaciones Is Nothing Then txtBCPObservaciones.Text = ent.BCPObservaciones
				If Not ent.BCPMedicion Is Nothing Then chbBCPMedicion.SelectedValue = ent.BCPMedicion
				If Not ent.BCPOlas Is Nothing Then txtBCPOlas.Text = ent.BCPOlas
				If Not ent.BCPPilotos Is Nothing Then chbBCPPilotos.Checked = ent.BCPPilotos
				If Not ent.BCPPilotosEspecificaciones Is Nothing Then txtBCPPilotosEspecificaciones.Text = ent.BCPPilotosEspecificaciones
				If Not ent.BCPIncentivosEspecificaciones Is Nothing Then txtBCPIncentivosEspecificaciones.Text = ent.BCPIncentivosEspecificaciones
				If Not ent.BCPBDDEspecificaciones Is Nothing Then txtBCPBDDEspecificaciones.Text = ent.BCPBDDEspecificaciones
				If Not ent.BCPProductoEspecificaciones Is Nothing Then txtBCPProductoEspecificaciones.Text = ent.BCPProductoEspecificaciones
				If ent.BCPFechaBDD.HasValue Then txtBCPFechaBDD.Text = ent.BCPFechaBDD.ToString() Else txtBCPFechaBDD.Text = ""
				If ent.BCPFechaConceptos.HasValue Then txtBCPFechaConceptos.Text = ent.BCPFechaConceptos.ToString() Else txtBCPFechaConceptos.Text = ""
				If ent.BCPFechaCuestionario.HasValue Then txtBCPFechaCuestionario.Text = ent.BCPFechaCuestionario.ToString() Else txtBCPFechaCuestionario.Text = ""
				If ent.BCPFechaInicioCampo.HasValue Then txtBCPFechaInicioCampo.Text = ent.BCPFechaInicioCampo.ToString() Else txtBCPFechaInicioCampo.Text = ""
				If ent.BCPFechaInformeCuentas.HasValue Then txtBCPFechaInformeCuentas.Text = ent.BCPFechaInformeCuentas.ToString() Else txtBCPFechaInformeCuentas.Text = ""
				If ent.BCPFechaInformeCliente.HasValue Then txtBCPFechaInformeCliente.Text = ent.BCPFechaInformeCliente.ToString() Else txtBCPFechaInformeCliente.Text = ""
				If Not ent.NoVersion Is Nothing Then lblVersionEspCuanti.InnerText = "Versión " + ent.NoVersion.ToString
			End If
		Catch ex As Exception

		End Try
	End Sub

	Sub CargarEspecificacionesProyectoCuali()
		Try
			Dim o As New Proyecto
			Dim esp As New PY_EspCuentasCuali
			esp = o.obtenerEspCuentasCuali(hfIdProyecto.Value)

			If Not IsNothing(esp.NoVersion) Then
				txtObservacionesCuali.Text = esp.BCPObservaciones
				chbBCPTecnicaCuali.SelectedValue = esp.BCPTecnica
				otraTecnica.Text = esp.BCPotraTecnica
				txtBCPIncentivosEspCuali.Text = esp.BCPIncentivosEsp
				txtBCPBDDEspCuali.Text = esp.BCPBDDEsp
				txtBCPProductoEspCuali.Text = esp.BCPProductoEsp
				chbBCPReclutamientoCuali.SelectedValue = esp.BCPReclutamiento
				txtBCPEspReclutamientoCuali.Text = esp.BCPEspReclutamiento
				chbBCPEspProductoCuali.Checked = esp.BCPEspProducto
				chbBCPMaterialEvalCuali.Checked = esp.BCPMaterialEval
				txtBCPObsProductoCuali.Text = esp.BCPObsProducto
				lblVersionEspCuali.InnerText = "Versión " + esp.NoVersion.ToString
			Else
				lblVersionEspCuali.InnerText = ""
			End If
		Catch ex As Exception

		End Try
	End Sub
	Sub CargarInfoPropuesta()
		Dim o As New Reportes.ReportesGenerales
		Me.gvPropuestaInfoGeneral.DataSource = o.PropuestaInfoGeneral(hfIdProyecto.Value, Nothing)
		Me.gvPropuestaPreguntas.DataSource = o.PropuestaInfoPreguntas(hfIdProyecto.Value, Nothing)
		Me.gvPropuestaMuestra.DataSource = o.PropuestaInfoMuestra(hfIdProyecto.Value, Nothing)
		gvPropuestaInfoGeneral.DataBind()
		gvPropuestaPreguntas.DataBind()
		gvPropuestaMuestra.DataBind()
	End Sub

	Sub limpiar()
		txtBCPObservaciones.Text = String.Empty
		chbBCPMedicion.ClearSelection()
		txtBCPOlas.Text = String.Empty
		chbBCPPilotos.Checked = False
		txtBCPPilotosEspecificaciones.Text = String.Empty
		txtBCPIncentivosEspecificaciones.Text = String.Empty
		txtBCPBDDEspecificaciones.Text = String.Empty
		txtBCPProductoEspecificaciones.Text = String.Empty
		txtBCPFechaBDD.Text = String.Empty
		txtBCPFechaConceptos.Text = String.Empty
		txtBCPFechaCuestionario.Text = String.Empty
		txtBCPFechaInicioCampo.Text = String.Empty
		txtBCPFechaInformeCuentas.Text = String.Empty
		txtBCPFechaInformeCliente.Text = String.Empty
		txtTitulo.Text = String.Empty
		txtAntecedentes.Text = String.Empty
		txtObjetivos.Text = String.Empty
		txtActionStandard.Text = String.Empty
		txtMetodologia.Text = String.Empty
		txtTargetGroup.Text = String.Empty
		txtTiempos.Text = String.Empty
		txtPresupuesto.Text = String.Empty
		txtMateriales.Text = String.Empty
		txtEstudiosAnteriores.Text = String.Empty
		txtFormatos.Text = String.Empty
		txtAprobaciones.Text = String.Empty
		txtCompetencia.Text = String.Empty
		txtObservacionesCuali.Text = String.Empty
		chbBCPTecnicaCuali.SelectedIndex = -1
		otraTecnica.Text = String.Empty
		txtBCPIncentivosEspCuali.Text = String.Empty
		txtBCPBDDEspCuali.Text = String.Empty
		txtBCPProductoEspCuali.Text = String.Empty
		chbBCPReclutamientoCuali.SelectedIndex = -1
		txtBCPEspReclutamientoCuali.Text = String.Empty
		chbBCPEspProductoCuali.Checked = False
		chbBCPMaterialEvalCuali.Checked = False
		txtBCPObsProductoCuali.Text = String.Empty
		lblVersionEspCuanti.InnerText = String.Empty
		lblVersionEspCuali.InnerText = String.Empty
		gvPropuestaInfoGeneral.DataSource = Nothing
		gvPropuestaInfoGeneral.DataBind()
		gvPropuestaMuestra.DataSource = Nothing
		gvPropuestaMuestra.DataBind()
		gvPropuestaPreguntas.DataSource = Nothing
		gvPropuestaPreguntas.DataBind()
	End Sub

	Public Sub CargarFrame(ByVal idbrief As Int64)
		Try
			Dim oBrief As New Brief
			Dim info = oBrief.ObtenerBriefXID(idbrief)
			txtO1.Text = info.O1
			txtO2.Text = info.O2
			txtO3.Text = info.O3
			txtO4.Text = info.O4
			txtO5.Text = info.O5
			txtO6.Text = info.O6
			txtO7.Text = info.O7
			txtD1.Text = info.D1
			txtD2.Text = info.D2
			txtD3.Text = info.D3
			txtC1.Text = info.C1
			txtC2.Text = info.C2
			txtC3.Text = info.C3
			txtC4.Text = info.C4
			txtC5.Text = info.C5
			txtM1.Text = info.M1
			txtM2.Text = info.M2
			txtM3.Text = info.M3
			txtDI1.Text = info.DI1
			txtDI2.Text = info.DI2
			txtDI3.Text = info.DI3
			txtDI4.Text = info.DI4
			txtDI5.Text = info.DI5
			txtDI6.Text = info.DI6
			txtDI7.Text = info.DI7
			txtDI8.Text = info.DI8
			txtDI9.Text = info.DI9
			txtDI10.Text = info.DI10
			txtDI11.Text = info.DI11
			txtDI12.Text = info.DI12
			txtDI13.Text = info.DI13
			txtDI14.Text = info.DI14
			txtDI15.Text = info.DI15
			txtDI16.Text = info.DI16
			txtDI17.Text = info.DI17
			txtDI18.Text = info.DI18
		Catch ex As Exception
			Throw ex
		End Try
	End Sub

	Private Sub btnGuardar2_Click(sender As Object, e As EventArgs) Handles btnGuardar2.Click
		Dim o As New Proyecto
		Dim esp As New PY_EspCuentasCuali
		esp = o.obtenerEspCuentasCuali(hfIdProyecto.Value)
		Dim NoVersion = 0
		If IsNothing(esp.NoVersion) Then
			NoVersion = 1
		Else
			NoVersion = Convert.ToInt64(esp.NoVersion) + 1
		End If

		Dim tecnica = If(chbBCPTecnicaCuali.SelectedValue = String.Empty, 0, Convert.ToInt32(chbBCPTecnicaCuali.SelectedValue))
		Dim reclutamiento = If(chbBCPReclutamientoCuali.SelectedValue = String.Empty, 0, Convert.ToInt32(chbBCPReclutamientoCuali.SelectedValue))

		If tecnica = 5 And otraTecnica.Text = "" Then
			AlertJS("Debe indicar qué técnica se va a usar en este proyecto")
			Exit Sub
		ElseIf tecnica <> 5 Then
			otraTecnica.Text = ""
		End If

		esp.ProyectoId = hfIdProyecto.Value
		esp.BCPObservaciones = txtObservacionesCuali.Text
		esp.BCPTecnica = tecnica
		esp.BCPotraTecnica = otraTecnica.Text
		esp.BCPIncentivosEsp = txtBCPIncentivosEspCuali.Text
		esp.BCPBDDEsp = txtBCPBDDEspCuali.Text
		esp.BCPProductoEsp = txtBCPProductoEspCuali.Text
		esp.BCPReclutamiento = reclutamiento
		esp.BCPEspReclutamiento = txtBCPEspReclutamientoCuali.Text
		esp.BCPEspProducto = chbBCPEspProductoCuali.Checked
		esp.BCPMaterialEval = chbBCPMaterialEvalCuali.Checked
		esp.BCPObsProducto = txtBCPObsProductoCuali.Text
		esp.NoVersion = NoVersion
		esp.Fecha = DateTime.Now.ToString
		esp.Usuario = Session("IDUsuario").ToString

		o.GuardarEspecificacionesCuentasCualitativa(esp)
		ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
		lblVersionEspCuali.InnerText = "Versión " + esp.NoVersion.ToString

		pnlBriefCuentasProyectos.Visible = False
		pnlBriefCuentasProyectosCuali.Visible = True
		CargarEspecificacionesProyectoCuali()
		cargarVersionesCuali(hfIdProyecto.Value)
	End Sub

	Sub AlertJS(ByVal mensaje As String)
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
	End Sub

	Private Sub btnCancela2_Click(sender As Object, e As EventArgs) Handles btnCancela2.Click
		pnlBriefCuentasProyectos.Visible = False
		pnlFrame.Visible = False
		pnlPropuesta.Visible = False
		accordion0.Visible = True
		accordion1.Visible = False
		limpiar()
	End Sub

	Private Sub gvVerEspCuanti_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvVerEspCuanti.RowDataBound
		If e.Row.RowType = DataControlRowType.DataRow Then
			If Me.gvVerEspCuanti.DataKeys(e.Row.RowIndex)("NoVersion") = "1" Then
				e.Row.Cells(5).Visible = False
				e.Row.Cells(4).ColumnSpan = 2
			End If
		End If
	End Sub

	Private Sub gvVerEspCuanti_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvVerEspCuanti.RowCommand
		If e.CommandName = "Ver" Then
			pnlVerEspCuanti.Visible = False
			pnlDetalleVerEspCuanti.Visible = True
			pnlCompararVerEspCuanti.Visible = False
			Dim id = gvVerEspCuanti.DataKeys(CInt(e.CommandArgument))("id")
			Dim o As New Proyecto
			Dim ent = o.obtenerEspCuentasCuantixID(hfIdProyecto.Value, id)

			llenarDetalleVerEspCuanti(ent)
		ElseIf e.CommandName = "Comparar" Then
			pnlVerEspCuanti.Visible = False
			pnlDetalleVerEspCuanti.Visible = False
			pnlCompararVerEspCuanti.Visible = True
			Dim id = gvVerEspCuanti.DataKeys(CInt(e.CommandArgument))("id")
			Dim o As New Proyecto
			Dim ent As New PY_EspCuentasCuanti
			Dim ent2 As New PY_EspCuentasCuanti
			ent = o.obtenerEspCuentasCuantixID(hfIdProyecto.Value, id)

			Dim versionActual = ent.NoVersion - 1
			If versionActual > 0 Then
				lblErrorVerEspCuanti.Text = ""
				ent2 = o.obtenerEspCuentasCuantixVersion(hfIdProyecto.Value, versionActual)
				CompararVerEspCuanti(ent2, ent)
			Else
				lblErrorVerEspCuanti.Text = "No hay versiones anteriores"
			End If
		End If
	End Sub

	Private Sub gvVerEspCuali_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvVerEspCuali.RowCommand
		If e.CommandName = "Ver" Then
			pnlVerEspCuali.Visible = False
			pnlDetalleVerEspCuali.Visible = True
			pnlCompararVerEspCuali.Visible = False
			Dim id = gvVerEspCuali.DataKeys(CInt(e.CommandArgument))("id")
			Dim o As New Proyecto
			Dim ent = o.obtenerEspCuentasCualixID(hfIdProyecto.Value, id)

			llenarDetalleVerEspCuali(ent)
		ElseIf e.CommandName = "Comparar" Then
			pnlVerEspCuali.Visible = False
			pnlDetalleVerEspCuali.Visible = False
			pnlCompararVerEspCuali.Visible = True
			Dim id = gvVerEspCuali.DataKeys(CInt(e.CommandArgument))("id")
			Dim o As New Proyecto
			Dim ent As New PY_EspCuentasCuali
			Dim ent2 As New PY_EspCuentasCuali
			ent = o.obtenerEspCuentasCualixID(hfIdProyecto.Value, id)

			Dim versionActual = ent.NoVersion - 1
			If versionActual > 0 Then
				lblErrorVerEspCuali.Text = ""
				ent2 = o.obtenerEspCuentasCualixVersion(hfIdProyecto.Value, versionActual)
				CompararVerEspCuali(ent2, ent)
			Else
				lblErrorVerEspCuali.Text = "No hay versiones anteriores"
			End If
		End If
	End Sub

	Sub CompararVerEspCuali(ByVal ent As PY_EspCuentasCuali, ByVal ent2 As PY_EspCuentasCuali)
		lblVerEspCualiA.Text = "Versión " + ent.NoVersion.ToString
		lblVerEspCualiB.Text = "Versión " + ent2.NoVersion.ToString

		If ent.BCPObservaciones <> ent2.BCPObservaciones Then
			txtObservacionesCualiV1.CssClass = "cambioVersion"
			txtObservacionesCualiV2.CssClass = "cambioVersion1"
		Else
			txtObservacionesCualiV1.CssClass = "versionIgual"
			txtObservacionesCualiV2.CssClass = "versionIgual"
		End If
		txtObservacionesCualiV1.Text = ent.BCPObservaciones
		txtObservacionesCualiV2.Text = ent2.BCPObservaciones

		If ent.BCPTecnica <> ent2.BCPTecnica Then
			chbBCPTecnicaCualiV1.CssClass = "cambioVersion"
			chbBCPTecnicaCualiV2.CssClass = "cambioVersion1"
		Else
			chbBCPTecnicaCualiV1.CssClass = "versionIgual"
			chbBCPTecnicaCualiV2.CssClass = "versionIgual"
		End If

		Select Case ent.BCPTecnica
			Case 1
				chbBCPTecnicaCualiV1.Text = "Entrevista"
			Case 2
				chbBCPTecnicaCualiV1.Text = "Sesiones de grupo/Talleres"
			Case 3
				chbBCPTecnicaCualiV1.Text = "Inmersiones"
			Case 4
				chbBCPTecnicaCualiV1.Text = "Estudios online"
			Case 5
				chbBCPTecnicaCualiV1.Text = "Otro"
			Case Else
				chbBCPTecnicaCualiV1.Text = ""
		End Select

		Select Case ent2.BCPTecnica
			Case 1
				chbBCPTecnicaCualiV2.Text = "Entrevista"
			Case 2
				chbBCPTecnicaCualiV2.Text = "Sesiones de grupo/Talleres"
			Case 3
				chbBCPTecnicaCualiV2.Text = "Inmersiones"
			Case 4
				chbBCPTecnicaCualiV2.Text = "Estudios online"
			Case 5
				chbBCPTecnicaCualiV2.Text = "Otro"
			Case Else
				chbBCPTecnicaCualiV2.Text = ""
		End Select

		If ent.BCPotraTecnica <> ent2.BCPotraTecnica Then
			otraTecnicaV1.CssClass = "cambioVersion"
			otraTecnicaV2.CssClass = "cambioVersion1"
		Else
			otraTecnicaV1.CssClass = "versionIgual"
			otraTecnicaV2.CssClass = "versionIgual"
		End If
		otraTecnicaV1.Text = ent.BCPotraTecnica
		otraTecnicaV2.Text = ent2.BCPotraTecnica

		If ent.BCPIncentivosEsp <> ent2.BCPIncentivosEsp Then
			txtBCPIncentivosEspCualiV1.CssClass = "cambioVersion"
			txtBCPIncentivosEspCualiV2.CssClass = "cambioVersion1"
		Else
			txtBCPIncentivosEspCualiV1.CssClass = "versionIgual"
			txtBCPIncentivosEspCualiV2.CssClass = "versionIgual"
		End If
		txtBCPIncentivosEspCualiV1.Text = ent.BCPIncentivosEsp
		txtBCPIncentivosEspCualiV2.Text = ent2.BCPIncentivosEsp

		If ent.BCPBDDEsp <> ent2.BCPBDDEsp Then
			txtBCPBDDEspCualiV1.CssClass = "cambioVersion"
			txtBCPBDDEspCualiV2.CssClass = "cambioVersion1"
		Else
			txtBCPBDDEspCualiV1.CssClass = "versionIgual"
			txtBCPBDDEspCualiV2.CssClass = "versionIgual"
		End If
		txtBCPBDDEspCualiV1.Text = ent.BCPBDDEsp
		txtBCPBDDEspCualiV2.Text = ent2.BCPBDDEsp

		If ent.BCPProductoEsp <> ent2.BCPProductoEsp Then
			txtBCPProductoEspCualiV1.CssClass = "cambioVersion"
			txtBCPProductoEspCualiV2.CssClass = "cambioVersion1"
		Else
			txtBCPProductoEspCualiV1.CssClass = "versionIgual"
			txtBCPProductoEspCualiV2.CssClass = "versionIgual"
		End If
		txtBCPProductoEspCualiV1.Text = ent.BCPProductoEsp
		txtBCPProductoEspCualiV2.Text = ent2.BCPProductoEsp

		If ent.BCPReclutamiento <> ent2.BCPReclutamiento Then
			chbBCPReclutamientoCualiV1.CssClass = "cambioVersion"
			chbBCPReclutamientoCualiV2.CssClass = "cambioVersion1"
		Else
			chbBCPReclutamientoCualiV1.CssClass = "versionIgual"
			chbBCPReclutamientoCualiV2.CssClass = "versionIgual"
		End If

		Select Case ent.BCPReclutamiento
			Case 1
				chbBCPReclutamientoCualiV1.Text = "Base de datos"
			Case 2
				chbBCPReclutamientoCualiV1.Text = "Convencional"
			Case 3
				chbBCPReclutamientoCualiV1.Text = "Referidos"
			Case 4
				chbBCPReclutamientoCualiV1.Text = "En frío"
			Case Else
				chbBCPReclutamientoCualiV1.Text = ""
		End Select

		Select Case ent2.BCPReclutamiento
			Case 1
				chbBCPReclutamientoCualiV2.Text = "Base de datos"
			Case 2
				chbBCPReclutamientoCualiV2.Text = "Convencional"
			Case 3
				chbBCPReclutamientoCualiV2.Text = "Referidos"
			Case 4
				chbBCPReclutamientoCualiV2.Text = "En frío"
			Case Else
				chbBCPReclutamientoCualiV2.Text = ""
		End Select

		If ent.BCPEspReclutamiento <> ent2.BCPEspReclutamiento Then
			txtBCPEspReclutamientoCualiV1.CssClass = "cambioVersion"
			txtBCPEspReclutamientoCualiV2.CssClass = "cambioVersion1"
		Else
			txtBCPEspReclutamientoCualiV1.CssClass = "versionIgual"
			txtBCPEspReclutamientoCualiV2.CssClass = "versionIgual"
		End If
		txtBCPEspReclutamientoCualiV1.Text = ent.BCPEspReclutamiento
		txtBCPEspReclutamientoCualiV2.Text = ent2.BCPEspReclutamiento

		If ent.BCPEspProducto <> ent2.BCPEspProducto Then
			chbBCPEspProductoCualiV1.CssClass = "cambioVersion"
			chbBCPEspProductoCualiV2.CssClass = "cambioVersion1"
		Else
			chbBCPEspProductoCualiV1.CssClass = "versionIgual"
			chbBCPEspProductoCualiV2.CssClass = "versionIgual"
		End If

		If ent.BCPEspProducto Then
			chbBCPEspProductoCualiV1.Text = "Sí"
		Else
			chbBCPEspProductoCualiV1.Text = "No"
		End If
		If ent2.BCPEspProducto Then
			chbBCPEspProductoCualiV2.Text = "Sí"
		Else
			chbBCPEspProductoCualiV2.Text = "No"
		End If

		If ent.BCPMaterialEval <> ent2.BCPMaterialEval Then
			chbBCPMaterialEvalCualiV1.CssClass = "cambioVersion"
			chbBCPMaterialEvalCualiV2.CssClass = "cambioVersion1"
		Else
			chbBCPMaterialEvalCualiV1.CssClass = "versionIgual"
			chbBCPMaterialEvalCualiV2.CssClass = "versionIgual"
		End If

		If ent.BCPMaterialEval Then
			chbBCPMaterialEvalCualiV1.Text = "Sí"
		Else
			chbBCPMaterialEvalCualiV1.Text = "No"
		End If
		If ent2.BCPMaterialEval Then
			chbBCPMaterialEvalCualiV2.Text = "Sí"
		Else
			chbBCPMaterialEvalCualiV2.Text = "No"
		End If


		Dim txtBCPMaterialEval = "No"
		If ent.BCPMaterialEval Then
			txtBCPMaterialEval = "Sí"
		End If
		chbBCPMaterialEvalCualiVer.Text = txtBCPMaterialEval

		If ent.BCPObsProducto <> ent2.BCPObsProducto Then
			txtBCPObsProductoCualiV1.CssClass = "cambioVersion"
			txtBCPObsProductoCualiV2.CssClass = "cambioVersion1"
		Else
			txtBCPObsProductoCualiV1.CssClass = "versionIgual"
			txtBCPObsProductoCualiV2.CssClass = "versionIgual"
		End If
		txtBCPObsProductoCualiV1.Text = ent.BCPObsProducto
		txtBCPObsProductoCualiV2.Text = ent2.BCPObsProducto
	End Sub

	Sub CompararVerEspCuanti(ByVal ent As PY_EspCuentasCuanti, ByVal ent2 As PY_EspCuentasCuanti)
		lblVerEspCuantiA.Text = "Versión " + ent.NoVersion.ToString
		lblVerEspCuantiB.Text = "Versión " + ent2.NoVersion.ToString

		If ent.BCPObservaciones <> ent2.BCPObservaciones Then
			txtBCPObservacionesV1.CssClass = "cambioVersion"
			txtBCPObservacionesV2.CssClass = "cambioVersion1"
		Else
			txtBCPObservacionesV1.CssClass = "versionIgual"
			txtBCPObservacionesV2.CssClass = "versionIgual"
		End If
		txtBCPObservacionesV1.Text = ent.BCPObservaciones
		txtBCPObservacionesV2.Text = ent2.BCPObservaciones

		If ent.BCPMedicion <> ent2.BCPMedicion Then
			chbBCPMedicionV1.CssClass = "cambioVersion"
			chbBCPMedicionV2.CssClass = "cambioVersion1"
		Else
			chbBCPMedicionV1.CssClass = "versionIgual"
			chbBCPMedicionV2.CssClass = "versionIgual"
		End If

		Select Case ent.BCPMedicion
			Case 1
				chbBCPMedicionV1.Text = "Medición Puntual"
			Case 2
				chbBCPMedicionV1.Text = "Medición Multifases"
			Case 3
				chbBCPMedicionV1.Text = "Tracking Puntuales"
			Case 4
				chbBCPMedicionV1.Text = "Tracking Continuo"
			Case Else
				chbBCPMedicionV1.Text = ""
		End Select

		Select Case ent2.BCPMedicion
			Case 1
				chbBCPMedicionV2.Text = "Medición Puntual"
			Case 2
				chbBCPMedicionV2.Text = "Medición Multifases"
			Case 3
				chbBCPMedicionV2.Text = "Tracking Puntuales"
			Case 4
				chbBCPMedicionV2.Text = "Tracking Continuo"
			Case Else
				chbBCPMedicionV2.Text = ""
		End Select

		If ent.BCPOlas <> ent2.BCPOlas Then
			txtBCPOlasV1.CssClass = "cambioVersion"
			txtBCPOlasV2.CssClass = "cambioVersion1"
		Else
			txtBCPOlasV1.CssClass = "versionIgual"
			txtBCPOlasV2.CssClass = "versionIgual"
		End If
		txtBCPOlasV1.Text = ent.BCPOlas
		txtBCPOlasV2.Text = ent2.BCPOlas

		If ent.BCPPilotos <> ent2.BCPPilotos Then
			chbBCPPilotosV1.CssClass = "cambioVersion"
			chbBCPPilotosV2.CssClass = "cambioVersion1"
		Else
			chbBCPPilotosV1.CssClass = "versionIgual"
			chbBCPPilotosV2.CssClass = "versionIgual"
		End If

		If ent.BCPPilotos Then
			chbBCPPilotosV1.Text = "Sí"
		Else
			chbBCPPilotosV1.Text = "No"
		End If

		If ent2.BCPPilotos Then
			chbBCPPilotosV2.Text = "Sí"
		Else
			chbBCPPilotosV2.Text = "No"
		End If

		If ent.BCPPilotosEspecificaciones <> ent2.BCPPilotosEspecificaciones Then
			txtBCPPilotosEspecificacionesV1.CssClass = "cambioVersion"
			txtBCPPilotosEspecificacionesV2.CssClass = "cambioVersion1"
		Else
			txtBCPPilotosEspecificacionesV1.CssClass = "versionIgual"
			txtBCPPilotosEspecificacionesV2.CssClass = "versionIgual"
		End If
		txtBCPPilotosEspecificacionesV1.Text = ent.BCPPilotosEspecificaciones
		txtBCPPilotosEspecificacionesV2.Text = ent2.BCPPilotosEspecificaciones

		If ent.BCPIncentivosEspecificaciones <> ent2.BCPIncentivosEspecificaciones Then
			txtBCPIncentivosEspecificacionesV1.CssClass = "cambioVersion"
			txtBCPIncentivosEspecificacionesV2.CssClass = "cambioVersion1"
		Else
			txtBCPIncentivosEspecificacionesV1.CssClass = "versionIgual"
			txtBCPIncentivosEspecificacionesV2.CssClass = "versionIgual"
		End If
		txtBCPIncentivosEspecificacionesV1.Text = ent.BCPIncentivosEspecificaciones
		txtBCPIncentivosEspecificacionesV2.Text = ent2.BCPIncentivosEspecificaciones

		If ent.BCPBDDEspecificaciones <> ent2.BCPBDDEspecificaciones Then
			txtBCPBDDEspecificacionesV1.CssClass = "cambioVersion"
			txtBCPBDDEspecificacionesV2.CssClass = "cambioVersion1"
		Else
			txtBCPBDDEspecificacionesV1.CssClass = "versionIgual"
			txtBCPBDDEspecificacionesV2.CssClass = "versionIgual"
		End If
		txtBCPBDDEspecificacionesV1.Text = ent.BCPBDDEspecificaciones
		txtBCPBDDEspecificacionesV2.Text = ent2.BCPBDDEspecificaciones

		If ent.BCPProductoEspecificaciones <> ent2.BCPProductoEspecificaciones Then
			txtBCPProductoEspecificacionesV1.CssClass = "cambioVersion"
			txtBCPProductoEspecificacionesV2.CssClass = "cambioVersion1"
		Else
			txtBCPProductoEspecificacionesV1.CssClass = "versionIgual"
			txtBCPProductoEspecificacionesV2.CssClass = "versionIgual"
		End If
		txtBCPProductoEspecificacionesV1.Text = ent.BCPProductoEspecificaciones
		txtBCPProductoEspecificacionesV2.Text = ent2.BCPProductoEspecificaciones

		If ent.BCPFechaBDD <> ent2.BCPFechaBDD Then
			txtBCPFechaBDDV1.CssClass = "cambioVersion"
			txtBCPFechaBDDV2.CssClass = "cambioVersion1"
		Else
			txtBCPFechaBDDV1.CssClass = "versionIgual"
			txtBCPFechaBDDV2.CssClass = "versionIgual"
		End If
		txtBCPFechaBDDV1.Text = ent.BCPFechaBDD
		txtBCPFechaBDDV2.Text = ent2.BCPFechaBDD

		If ent.BCPFechaConceptos <> ent2.BCPFechaConceptos Then
			txtBCPFechaConceptosV1.CssClass = "cambioVersion"
			txtBCPFechaConceptosV2.CssClass = "cambioVersion1"
		Else
			txtBCPFechaConceptosV1.CssClass = "versionIgual"
			txtBCPFechaConceptosV2.CssClass = "versionIgual"
		End If
		txtBCPFechaConceptosV1.Text = ent.BCPFechaConceptos
		txtBCPFechaConceptosV2.Text = ent2.BCPFechaConceptos

		If ent.BCPFechaCuestionario <> ent2.BCPFechaCuestionario Then
			txtBCPFechaCuestionarioV1.CssClass = "cambioVersion"
			txtBCPFechaCuestionarioV2.CssClass = "cambioVersion1"
		Else
			txtBCPFechaCuestionarioV1.CssClass = "versionIgual"
			txtBCPFechaCuestionarioV2.CssClass = "versionIgual"
		End If
		txtBCPFechaCuestionarioV1.Text = ent.BCPFechaCuestionario
		txtBCPFechaCuestionarioV2.Text = ent2.BCPFechaCuestionario

		If ent.BCPFechaInicioCampo <> ent2.BCPFechaInicioCampo Then
			txtBCPFechaInicioCampoV1.CssClass = "cambioVersion"
			txtBCPFechaInicioCampoV2.CssClass = "cambioVersion1"
		Else
			txtBCPFechaInicioCampoV1.CssClass = "versionIgual"
			txtBCPFechaInicioCampoV2.CssClass = "versionIgual"
		End If
		txtBCPFechaInicioCampoV1.Text = ent.BCPFechaInicioCampo
		txtBCPFechaInicioCampoV2.Text = ent2.BCPFechaInicioCampo

		If ent.BCPFechaInformeCuentas <> ent2.BCPFechaInformeCuentas Then
			txtBCPFechaInformeCuentasV1.CssClass = "cambioVersion"
			txtBCPFechaInformeCuentasV2.CssClass = "cambioVersion1"
		Else
			txtBCPFechaInformeCuentasV1.CssClass = "versionIgual"
			txtBCPFechaInformeCuentasV2.CssClass = "versionIgual"
		End If
		txtBCPFechaInformeCuentasV1.Text = ent.BCPFechaInformeCuentas
		txtBCPFechaInformeCuentasV2.Text = ent2.BCPFechaInformeCuentas

		If ent.BCPFechaInformeCliente <> ent2.BCPFechaInformeCliente Then
			txtBCPFechaInformeClienteV1.CssClass = "cambioVersion"
			txtBCPFechaInformeClienteV2.CssClass = "cambioVersion1"
		Else
			txtBCPFechaInformeClienteV1.CssClass = "versionIgual"
			txtBCPFechaInformeClienteV2.CssClass = "versionIgual"
		End If
		txtBCPFechaInformeClienteV1.Text = ent.BCPFechaInformeCliente
		txtBCPFechaInformeClienteV2.Text = ent2.BCPFechaInformeCliente
	End Sub

	Sub llenarDetalleVerEspCuali(ByVal esp As PY_EspCuentasCuali)
		txtObservacionesCualiVer.Text = esp.BCPObservaciones
		Dim BCPTecnica = esp.BCPTecnica
		Dim txtBCPTecnica = ""
		Select Case BCPTecnica
			Case 1
				txtBCPTecnica = "Entrevista"
			Case 2
				txtBCPTecnica = "Sesiones de grupo/Talleres"
			Case 3
				txtBCPTecnica = "Inmersiones"
			Case 4
				txtBCPTecnica = "Estudios online"
			Case 5
				txtBCPTecnica = "Otro"
			Case Else
				txtBCPTecnica = ""
		End Select
		chbBCPTecnicaCualiVer.Text = txtBCPTecnica
		If Not esp.BCPotraTecnica Is Nothing Then otraTecnicaVer.Text = esp.BCPotraTecnica
		If Not esp.BCPIncentivosEsp Is Nothing Then txtBCPIncentivosEspCualiVer.Text = esp.BCPIncentivosEsp
		If Not esp.BCPBDDEsp Is Nothing Then txtBCPBDDEspCualiVer.Text = esp.BCPBDDEsp
		If Not esp.BCPProductoEsp Is Nothing Then txtBCPProductoEspCualiVer.Text = esp.BCPProductoEsp
		Dim BCPReclutamiento = esp.BCPReclutamiento
		Dim txtBCPReclutamiento = ""
		Select Case BCPReclutamiento
			Case 1
				txtBCPReclutamiento = "Base de datos"
			Case 2
				txtBCPReclutamiento = "Convencional"
			Case 3
				txtBCPReclutamiento = "Referidos"
			Case 4
				txtBCPReclutamiento = "En frío"
			Case Else
				txtBCPReclutamiento = ""
		End Select
		If Not txtBCPReclutamiento Is Nothing Then chbBCPReclutamientoCualiVer.Text = txtBCPReclutamiento
		If Not esp.BCPEspReclutamiento Is Nothing Then txtBCPEspReclutamientoCualiVer.Text = esp.BCPEspReclutamiento

		Dim BCPEspProductoCuali = "No"
		If esp.BCPEspProducto Then
			BCPEspProductoCuali = "Sí"
		End If
		If Not BCPEspProductoCuali Is Nothing Then chbBCPEspProductoCualiVer.Text = BCPEspProductoCuali
		Dim txtBCPMaterialEval = "No"
		If esp.BCPMaterialEval Then
			txtBCPMaterialEval = "Sí"
		End If
		If Not txtBCPMaterialEval Is Nothing Then chbBCPMaterialEvalCualiVer.Text = txtBCPMaterialEval
		If Not esp.BCPObsProducto Is Nothing Then txtBCPObsProductoCualiVer.Text = esp.BCPObsProducto
	End Sub

	Sub llenarDetalleVerEspCuanti(ByVal ent As PY_EspCuentasCuanti)
		txtBCPObservacionesVer.Text = ent.BCPObservaciones
		Dim BCPMedicion = ent.BCPMedicion
		Dim txtBCPMedicion = ""
		Select Case BCPMedicion
			Case 1
				txtBCPMedicion = "Medición Puntual"
			Case 2
				txtBCPMedicion = "Medición Multifases"
			Case 3
				txtBCPMedicion = "Tracking Puntuales"
			Case 4
				txtBCPMedicion = "Tracking Continuo"
			Case Else
				txtBCPMedicion = ""
		End Select
		chbBCPMedicionVer.Text = txtBCPMedicion
		txtBCPOlasVer.Text = ent.BCPOlas
		Dim BCPPilotos = ent.BCPPilotos
		Dim txtBCPPilotos = "No"
		If BCPPilotos Then
			txtBCPPilotos = "Sí"
		End If
		chbBCPPilotosVer.Text = txtBCPPilotos
		If Not ent.BCPPilotosEspecificaciones Is Nothing Then txtBCPPilotosEspecificacionesVer.Text = ent.BCPPilotosEspecificaciones
		If Not ent.BCPPilotosEspecificaciones Is Nothing Then txtBCPPilotosEspecificacionesVer.Text = ent.BCPPilotosEspecificaciones
		If Not ent.BCPIncentivosEspecificaciones Is Nothing Then txtBCPIncentivosEspecificacionesVer.Text = ent.BCPIncentivosEspecificaciones
		If Not ent.BCPBDDEspecificaciones Is Nothing Then txtBCPBDDEspecificacionesVer.Text = ent.BCPBDDEspecificaciones
		If Not ent.BCPProductoEspecificaciones Is Nothing Then txtBCPProductoEspecificacionesVer.Text = ent.BCPProductoEspecificaciones
		If Not ent.BCPFechaBDD Is Nothing Then txtBCPFechaBDDVer.Text = ent.BCPFechaBDD
		If Not ent.BCPFechaConceptos Is Nothing Then txtBCPFechaConceptosVer.Text = ent.BCPFechaConceptos
		If Not ent.BCPFechaCuestionario Is Nothing Then txtBCPFechaCuestionarioVer.Text = ent.BCPFechaCuestionario
		If Not ent.BCPFechaInicioCampo Is Nothing Then txtBCPFechaInicioCampoVer.Text = ent.BCPFechaInicioCampo
		If Not ent.BCPFechaInformeCuentas Is Nothing Then txtBCPFechaInformeCuentasVer.Text = ent.BCPFechaInformeCuentas
		If Not ent.BCPFechaInformeCliente Is Nothing Then txtBCPFechaInformeClienteVer.Text = ent.BCPFechaInformeCliente
	End Sub

	Sub LimpiarDetalleVerEspCuanti()
		txtBCPObservacionesVer.Text = ""
		chbBCPMedicionVer.Text = ""
		txtBCPOlasVer.Text = ""
		chbBCPPilotosVer.Text = ""
		txtBCPPilotosEspecificacionesVer.Text = ""
		txtBCPIncentivosEspecificacionesVer.Text = ""
		txtBCPBDDEspecificacionesVer.Text = ""
		txtBCPProductoEspecificacionesVer.Text = ""
		txtBCPFechaBDDVer.Text = ""
		txtBCPFechaConceptosVer.Text = ""
		txtBCPFechaCuestionarioVer.Text = ""
		txtBCPFechaInicioCampoVer.Text = ""
		txtBCPFechaInformeCuentasVer.Text = ""
		txtBCPFechaInformeClienteVer.Text = ""
	End Sub

	Sub LimpiarDetalleVerEspCuali()
		txtObservacionesCualiVer.Text = ""
		chbBCPTecnicaCualiVer.Text = ""
		otraTecnicaVer.Text = ""
		txtBCPIncentivosEspCualiVer.Text = ""
		txtBCPBDDEspCualiVer.Text = ""
		txtBCPProductoEspCualiVer.Text = ""
		chbBCPReclutamientoCualiVer.Text = ""
		txtBCPEspReclutamientoCualiVer.Text = ""
		chbBCPEspProductoCualiVer.Text = ""
		chbBCPMaterialEvalCualiVer.Text = ""
		txtBCPObsProductoCualiVer.Text = ""
	End Sub

	Private Sub volverListadoVerEspCuanti_ServerClick(sender As Object, e As EventArgs) Handles volverListadoVerEspCuanti.ServerClick
		lblErrorVerEspCuanti.Text = ""
		pnlVerEspCuanti.Visible = True
		pnlDetalleVerEspCuanti.Visible = False
		pnlCompararVerEspCuanti.Visible = False
		LimpiarDetalleVerEspCuanti()
	End Sub

	Private Sub volverListadoVerEspCuanti2_ServerClick(sender As Object, e As EventArgs) Handles volverListadoVerEspCuanti2.ServerClick
		lblErrorVerEspCuanti.Text = ""
		pnlVerEspCuanti.Visible = True
		pnlDetalleVerEspCuanti.Visible = False
		pnlCompararVerEspCuanti.Visible = False
		LimpiarDetalleVerEspCuanti()
	End Sub

	Private Sub volverListadoVerEspCuali_ServerClick(sender As Object, e As EventArgs) Handles volverListadoVerEspCuali.ServerClick
		lblErrorVerEspCuali.Text = ""
		pnlVerEspCuali.Visible = True
		pnlDetalleVerEspCuali.Visible = False
		pnlCompararVerEspCuali.Visible = False
		LimpiarDetalleVerEspCuali()
	End Sub

	Private Sub volverListadoVerEspCuali2_ServerClick(sender As Object, e As EventArgs) Handles volverListadoVerEspCuali2.ServerClick
		lblErrorVerEspCuali.Text = ""
		pnlVerEspCuali.Visible = True
		pnlDetalleVerEspCuali.Visible = False
		pnlCompararVerEspCuali.Visible = False
		LimpiarDetalleVerEspCuali()
	End Sub

	Private Sub gvVerEspCuali_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvVerEspCuali.RowDataBound
		If e.Row.RowType = DataControlRowType.DataRow Then
			If Me.gvVerEspCuali.DataKeys(e.Row.RowIndex)("NoVersion") = "1" Then
				e.Row.Cells(5).Visible = False
				e.Row.Cells(4).ColumnSpan = 2
			End If
		End If
	End Sub

	Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
		accordion1.Visible = False
		accordion0.Visible = True
	End Sub
End Class