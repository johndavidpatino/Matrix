Imports CoreProject

Public Class EstudioForm
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			LoadInfoJobBook()
			LoadEstudios()
			CargarDocumentosSoporte()
			CargarUnidades()
		End If
	End Sub

	Sub LoadInfoJobBook()
		If Not (Session("InfoJobBook") Is Nothing) Then
			Dim infoJobBook As oJobBook = Session("InfoJobBook")
			lblInfo.Text = infoJobBook.NumJobBook & " | " & infoJobBook.Titulo & " | " & infoJobBook.Cliente & " | " & infoJobBook.IdPropuesta.ToString
			hfPropuesta.Value = infoJobBook.IdPropuesta
			If infoJobBook.GuardarCambios = True Then
				'btnSave.Visible = False
				btnNew.Visible = True
			End If
		End If
	End Sub

	Sub LoadEstudios()
		Dim oEstudio As New CoreProject.Estudio
		gvEstudios.DataSource = oEstudio.ObtenerXIdPropuesta(hfPropuesta.Value)
		gvEstudios.DataBind()
	End Sub

	Protected Sub LoadFiles_Click(sender As Object, e As EventArgs)
		If btnLoadFiles.Text = "Ocultar Carga de archivos" Then
			pnlLoadFiles.Visible = False
			btnLoadFiles.Text = "Ver / Cargar Archivos"
		Else
			Dim oContenedor As New oContenedorDocumento
			oContenedor.ContenedorId = hfEstudio.Value
			oContenedor.DocumentoId = 50
			Session("oContenedorDocumento") = oContenedor
			pnlLoadFiles.Visible = True
			UCFiles.ContenedorId = hfEstudio.Value
			UCFiles.DocumentoId = 2
			UCFiles.CargarDocumentos()

			btnLoadFiles.Text = "Ocultar Carga de archivos"
		End If
	End Sub

	Private Sub gvEstudios_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvEstudios.RowCommand
		If e.CommandName = "EditP" Then
			LoadEstudio(Int64.Parse(gvEstudios.DataKeys(CInt(e.CommandArgument))("Id")))
		End If
	End Sub

	Sub LoadEstudio(idEstudio As Int64)
		Dim oEstudio As New CoreProject.Estudio
		Dim infoE = oEstudio.ObtenerXID(idEstudio)
		pnlNew.Visible = True
		txtJobBook.Text = infoE.JobBook
		txtAnticipo.Text = infoE.Anticipo
		txtFechaFin.Text = infoE.FechaTerminacion
		txtFechaInicio.Text = infoE.FechaInicio
		txtFechaInicioCampo.Text = infoE.FechaInicioCampo
		txtObservaciones.Text = infoE.Observaciones
		txtPlazoPago.Text = infoE.Plazo
		If infoE.TiempoRetencionAnnos IsNot Nothing Then txtRetencion.Text = infoE.TiempoRetencionAnnos
		txtSaldo.Text = infoE.Saldo
		txtValor.Text = infoE.Valor
		If infoE.DocumentoSoporte IsNot Nothing Then ddlDocumentoSoporte.SelectedValue = infoE.DocumentoSoporte
		Dim oPresupuesto As New CoreProject.Presupuesto
		gvPresupuestosAsignadosXEstudio.DataSource = oPresupuesto.ObtenerPresupuestosAsignadosXEstudio(infoE.id)
		gvPresupuestosAsignadosXEstudio.DataBind()
		pnlPresupuestosAsociados.Visible = True
		hfEstudio.Value = idEstudio
		CargarProyectos()
		pnlListadoProyectos.Visible = True
		pnlListadoEstudios.Visible = False
		btnChangeAlternativa.Visible = True
	End Sub

	Sub ClearForm()
		pnlNew.Visible = True
		txtJobBook.Text = String.Empty
		txtAnticipo.Text = String.Empty
		txtFechaFin.Text = String.Empty
		txtFechaInicio.Text = String.Empty
		txtFechaInicioCampo.Text = String.Empty
		txtObservaciones.Text = String.Empty
		txtPlazoPago.Text = String.Empty
		txtRetencion.Text = String.Empty
		txtSaldo.Text = String.Empty
		txtValor.Text = String.Empty
		ddlDocumentoSoporte.SelectedIndex = 0
		pnlPresupuestosAsociados.Visible = False
		pnlPresupuestosPropuesta.Visible = False
		pnlLoadFiles.Visible = False
		btnLoadFiles.Visible = False
		pnlListadoEstudios.Visible = True
		pnlNewProyects.Visible = False
		pnlEsquemaAnalisis.Visible = False
		btnViewEsquemaAnalisis.Visible = False
	End Sub
	Protected Sub btnNew_Click(sender As Object, e As EventArgs)
		hfEstudio.Value = 0
		Dim oPresupuesto As New CoreProject.Presupuesto
		gvPresupuestos.DataSource = oPresupuesto.DevolverxIdPropuestaAprobados(hfPropuesta.Value, Nothing)
		gvPresupuestos.DataBind()
		If gvPresupuestos.Rows.Count = 0 Then
			ShowWarning(TypesWarning.Warning, "No se encuentran presupuestos aprobados. Asegúrese de tener al menos un presupuesto aprobado antes de continuar")
			Exit Sub
		End If
		ClearForm()
		pnlListadoEstudios.Visible = False
		pnlPresupuestosPropuesta.Visible = True
		pnlNew.Visible = True
		btnSave.Visible = True
		pnlNewProyects.Visible = True
		pnlEsquemaAnalisis.Visible = True
		btnChangeAlternativa.Visible = False
		If gvPresupuestos.Rows.Count = 0 Then
			btnSave.Visible = False
		Else
			btnSave.Visible = True
		End If
		Dim oPropuesta As New CoreProject.Propuesta
		Dim infoP = oPropuesta.DevolverxID(hfPropuesta.Value)
		txtJobBook.Text = infoP.JobBook & "-01"
		txtFechaInicio.Text = Date.UtcNow.AddHours(-5).Date
		txtFechaInicioCampo.Text = infoP.FechaInicioCampo
		txtSaldo.Text = 30
		txtAnticipo.Text = 70
		txtPlazoPago.Text = 30
		txtRetencion.Text = 1
	End Sub

	Function ValidateSave(Optional ByVal CambioAlternativa As Boolean = False) As Boolean
		Dim flag As Boolean = False
		If (hfEstudio.Value = 0) Then
			For Each row As GridViewRow In gvPresupuestos.Rows
				If DirectCast(row.FindControl("chkAsignar"), RadioButton).Checked = True Then
					flag = True
				End If
			Next
			If flag = False Then
				ShowWarning(TypesWarning.ErrorMessage, "Debe seleccionar un presupuesto antes de continuar")
				Return False
			End If
		End If
		If Not String.IsNullOrEmpty(txtFechaInicioCampo.Text) Then
			If ValidarFecha(txtFechaInicioCampo.Text) Then
				If Not (Date.Parse(txtFechaInicioCampo.Text) > Now()) Then
					ShowWarning(TypesWarning.ErrorMessage, "La fecha de inicio de campo debe ser mayor a la fecha actual")
					Return False
				End If
			Else
				ShowWarning(TypesWarning.ErrorMessage, "Fecha de Inicio de Campo No Válida")
				Return False
			End If
		Else
			ShowWarning(TypesWarning.ErrorMessage, "Digite la fecha de Inicio de Campo")
			Return False
		End If
		If ddlDocumentoSoporte.SelectedValue = "-1" Then
			ShowWarning(TypesWarning.ErrorMessage, "Seleccione el documento soporte")
			Return False
		End If
		If Not (IsNumeric(txtRetencion.Text)) Then
			ShowWarning(TypesWarning.ErrorMessage, "El tiempo de retención debe ser numérico")
			Return False
		End If

		If txtJobBook.Text = "" Or txtJobBook.Text.EndsWith("00") Then
			ShowWarning(TypesWarning.ErrorMessage, "Debe escribir un número de JobBook válido antes de continuar")
			Return False
		End If
		If Not String.IsNullOrEmpty(txtFechaInicio.Text) Then
			'Validar formato de la fecha
			If Not (ValidarFecha(txtFechaInicio.Text)) Then
				ShowWarning(TypesWarning.ErrorMessage, "Fecha de inicio No Válida")
				Return False
			End If
		Else
			ShowWarning(TypesWarning.ErrorMessage, "Fecha de inicio requerida")
			Return False
		End If
		If Not String.IsNullOrEmpty(txtFechaFin.Text) Then
			'Validar formato de la fecha
			If ValidarFecha(txtFechaFin.Text) Then
				If Not (Date.Parse(txtFechaFin.Text) > Date.Parse(txtFechaInicio.Text)) Then
					ShowWarning(TypesWarning.ErrorMessage, "La fecha de terminación debe ser mayor a la fecha de inicio")
					Return False
				End If
			Else
				ShowWarning(TypesWarning.ErrorMessage, "Fecha de terminación No Válida")
				Return False
			End If
		Else
			ShowWarning(TypesWarning.ErrorMessage, "Fecha de terminación requerida")
			Return False
		End If
		If pnlEsquemaAnalisis.Visible = True Then

		End If
		Return True
	End Function

	Protected Sub btnSave_Click(sender As Object, e As EventArgs)
		If ValidateSave() = False Then Exit Sub
		Dim bolNew As Boolean = False
		If hfEstudio.Value = 0 Then bolNew = True
		Dim oEstudio As New CoreProject.Estudio
		Dim Estudio As New CoreProject.CU_Estudios
		If bolNew = False Then
			Estudio = oEstudio.ObtenerXID(hfEstudio.Value)
		End If
		Estudio.id = hfEstudio.Value
		Estudio.Nombre = DirectCast(Session("InfoJobBook"), oJobBook).Titulo
		Estudio.Anticipo = txtAnticipo.Text
		Estudio.FechaTerminacion = txtFechaFin.Text
		Estudio.FechaInicio = txtFechaInicio.Text
		Estudio.FechaInicioCampo = txtFechaInicioCampo.Text
		Estudio.Observaciones = txtObservaciones.Text
		Estudio.Plazo = txtPlazoPago.Text
		Estudio.TiempoRetencionAnnos = txtRetencion.Text
		Estudio.Saldo = txtSaldo.Text
		Estudio.Valor = txtValor.Text
		Estudio.DocumentoSoporte = ddlDocumentoSoporte.SelectedValue
		Estudio.Estado = 1
		Estudio.GerenteCuentas = Session("IDUsuario").ToString
		Estudio.JobBook = txtJobBook.Text
		Estudio.PropuestaId = hfPropuesta.Value
		oEstudio.GuardarEstudio(Estudio)
		hfEstudio.Value = Estudio.id
		Dim idpresup As Int64
		Dim flag As Boolean = False
		For Each row As GridViewRow In gvPresupuestos.Rows
			If DirectCast(row.FindControl("chkAsignar"), RadioButton).Checked = True Then
				flag = True
				idpresup = gvPresupuestos.DataKeys(row.RowIndex).Value
			End If
		Next
		Dim oEstudios_Presupuestos As New CoreProject.Estudios_Presupuestos
		If bolNew = True Then
			oEstudios_Presupuestos.GrabarEstudiosPresupuestos(hfEstudio.Value, idpresup)
			EnviarEmailAnuncio()
			Dim oProyectos_Presupuestos As New CoreProject.Proyectos_Presupuestos
			Dim oPresupuesto As New CoreProject.Presupuesto
			Dim presupuesto = oPresupuesto.ObtenerPresupuestosAsignadosXEstudio(hfEstudio.Value)
			Dim oProyecto As New CoreProject.Proyecto
			Dim Proyecto As New CoreProject.PY_Proyectos
			If chbProjectCuanti.Checked = True Then
				Proyecto.A1 = txtA1.Text
				Proyecto.A2 = txtA2.Text
				Proyecto.A3 = txtA3.Text
				Proyecto.A4 = txtA4.Text
				Proyecto.A5 = txtA5.Text
				Proyecto.A6 = txtA6.Text
				Proyecto.A7 = txtA7.Text
				Proyecto.A8 = txtA8.Text
				Proyecto.EstudioId = hfEstudio.Value
				Proyecto.Estado = 1
				Proyecto.JobBook = txtJobBook.Text
				Proyecto.Nombre = DirectCast(Session("InfoJobBook"), oJobBook).Titulo
				Proyecto.TipoProyectoId = 1
				Proyecto.UnidadId = DirectCast(Session("InfoJobBook"), oJobBook).IdUnidad
				oProyecto.GuardarProyecto(Proyecto)
				oProyectos_Presupuestos.Grabar(idpresup, Proyecto.id)
				Dim oDatos As New CoreProject.IQ.ControlCostos
				Dim DatosGenerales = oDatos.ObtenerDatosGeneralesPresupuesto(presupuesto(0).PropuestaId, presupuesto(0).Alternativa)
				If DatosGenerales.TipoPresupuesto = 2 Then
					oPresupuesto.ActualizarParNumJobBookEnIQ(txtJobBook.Text, presupuesto(0).PropuestaId, presupuesto(0).Alternativa)
				End If
				EnviarEmail(Proyecto.id)
				EnviarEmailJBI(Proyecto.id)
			End If
			If chbProjectCuali.Checked = True Then
				Proyecto = New CoreProject.PY_Proyectos
				Proyecto.A1 = txtA1.Text
				Proyecto.A2 = txtA2.Text
				Proyecto.A3 = txtA3.Text
				Proyecto.A4 = txtA4.Text
				Proyecto.A5 = txtA5.Text
				Proyecto.A6 = txtA6.Text
				Proyecto.A7 = txtA7.Text
				Proyecto.A8 = txtA8.Text
				Proyecto.EstudioId = hfEstudio.Value
				Proyecto.Estado = 1
				Proyecto.JobBook = txtJobBook.Text
				Proyecto.Nombre = DirectCast(Session("InfoJobBook"), oJobBook).Titulo.ToString & " - Cualitativo"
				Proyecto.TipoProyectoId = 2
				Proyecto.UnidadId = 17
				oProyecto.GuardarProyecto(Proyecto)
				oProyectos_Presupuestos.Grabar(idpresup, Proyecto.id)
				oPresupuesto.ActualizarParNumJobBookEnIQ(txtJobBook.Text, presupuesto(0).PropuestaId, presupuesto(0).Alternativa)
				EnviarEmail(Proyecto.id)
			End If
			logNew(hfEstudio.Value, 2)
		End If
		ClearForm()
		LoadEstudio(hfEstudio.Value)
	End Sub

	Public Sub logNew(ByVal iddoc As Int64?, ByVal idaccion As Int16)
		Dim log As New LogEjecucion
		log.Guardar(14, iddoc, Now(), Session("IDUsuario"), idaccion)
	End Sub
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

	Public Function ValidarFecha(ByVal txtFecha As String, Optional prueba As Int64? = Nothing) As Boolean
		Dim dt As DateTime

		Dim blnFlag As Boolean = DateTime.TryParse(txtFecha, dt)

		If blnFlag Then
			Return True
		Else
			Return False
		End If
	End Function

	Protected Sub btnCancel_Click(sender As Object, e As EventArgs)
		ClearForm()
		pnlNew.Visible = False
	End Sub

	Protected Sub btnActualizarCambio_Click(sender As Object, e As EventArgs)
		pnlListadoProyectos.Visible = False
		pnlDatosProyectos.Visible = False
		pnlEsquemaAnalisis.Visible = False
		Dim o As New CoreProject.Reportes.CambiosAlternativas
		If ValidateSave(True) = False Then
			Exit Sub
		End If
		o.CambioAlternativaEstudio(hfEstudio.Value, ddlAlternativas.SelectedValue)
		EnviarEmailAnuncioCorreccion()
		ShowWarning(TypesWarning.Information, "Se han confirmado y comunicado los cambios")
		pnlCambiosPresupuestos.Visible = False
		LoadEstudio(hfEstudio.Value)
	End Sub

	Protected Sub btnCancelarCambio_Click(sender As Object, e As EventArgs)
		pnlCambiosPresupuestos.Visible = False
	End Sub

	Protected Sub btnChangeAlternativa_Click(sender As Object, e As EventArgs)
		Me.pnlCambiosPresupuestos.Visible = True
		Dim o As New CoreProject.Reportes.CambiosAlternativas
		ddlAlternativas.DataSource = o.ObtenerAlternativasNoAsociadas(hfEstudio.Value)
		ddlAlternativas.DataTextField = "NomAlternativa"
		ddlAlternativas.DataValueField = "Alternativa"
		ddlAlternativas.DataBind()
	End Sub

	Sub EnviarEmailAnuncio()
		Dim oEnviarCorreo As New EnviarCorreo
		Try
			If String.IsNullOrEmpty(hfEstudio.Value) Then
				Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar un anuncio de aprobación")
			End If
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/AnuncioAprobacionConValor.aspx?idEstudio=" & hfEstudio.Value)
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/AnuncioAprobacionSinValor.aspx?idEstudio=" & hfEstudio.Value)
			'Dim script As String = "window.open('../Emails/AnuncioAprobacion.aspx?idEstudio=" & hfIdEstudio.Value & "','cal','width=400,height=250,left=270,top=180')"
			'Dim page As Page = DirectCast(Context.Handler, Page)
			'ScriptManager.RegisterStartupScript(page, GetType(Page), "Anuncio", script, True)
		Catch ex As Exception
		End Try
	End Sub
	Sub EnviarEmailAnuncioCorreccion()
		Dim oEnviarCorreo As New EnviarCorreo
		Try
			If String.IsNullOrEmpty(hfEstudio.Value) Then
				ShowWarning(TypesWarning.ErrorMessage, "Debe elegir un estudio o guardarlo antes de enviar un anuncio de aprobación")
			End If
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/AnuncioAprobacionConValorCorreccion.aspx?idEstudio=" & hfEstudio.Value)
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/AnuncioAprobacionSinValorCorreccion.aspx?idEstudio=" & hfEstudio.Value)
			'Dim script As String = "window.open('../Emails/AnuncioAprobacion.aspx?idEstudio=" & hfIdEstudio.Value & "','cal','width=400,height=250,left=270,top=180')"
			'Dim page As Page = DirectCast(Context.Handler, Page)
			'ScriptManager.RegisterStartupScript(page, GetType(Page), "Anuncio", script, True)
		Catch ex As Exception
			ShowWarning(TypesWarning.ErrorMessage, ex.Message)
		End Try
	End Sub

	Sub EnviarEmail(IdProyecto As Int64)
		Dim oEnviarCorreo As New EnviarCorreo
		Try
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/NuevoProyectoCoordinacion.aspx?idProyecto=" & IdProyecto)
		Catch ex As Exception
		End Try
	End Sub

	Sub EnviarEmailJBI(IdProyecto As Int64)
		Dim oEnviarCorreo As New EnviarCorreo
		Try
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/NuevoProyectoJBI.aspx?idProyecto=" & IdProyecto)
		Catch ex As Exception
		End Try
	End Sub

	Protected Sub btnViewEsquemaAnalisis_Click(sender As Object, e As EventArgs)
		If btnViewEsquemaAnalisis.Text = "Ocultar Esquema de Análisis" Then
			pnlEsquemaAnalisis.Visible = False
			btnViewEsquemaAnalisis.Text = "Ver Esquema de Análisis"
		Else
			pnlEsquemaAnalisis.Visible = True
			btnViewEsquemaAnalisis.Text = "Ocultar Esquema de Análisis"
		End If
	End Sub

	Sub CargarDocumentosSoporte()
		Dim oEstudios As New CoreProject.Estudio
		ddlDocumentoSoporte.DataSource = oEstudios.ObtenerDocumentosSoporte
		ddlDocumentoSoporte.DataValueField = "id"
		ddlDocumentoSoporte.DataTextField = "DocumentoSoporte"
		ddlDocumentoSoporte.DataBind()
		ddlDocumentoSoporte.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarProyectos()
		Dim o As New CoreProject.Proyecto
		gvProyectos.DataSource = o.ObtenerListadoXEstudio(hfEstudio.Value)
		gvProyectos.DataBind()
	End Sub
	Protected Sub btnNewProject_Click(sender As Object, e As EventArgs)
		ClearProyecto()
		pnlListadoProyectos.Visible = False
		pnlDatosProyectos.Visible = True
		btnSave.Visible = False
		btnCancel.Visible = False
		btnChangeAlternativa.Visible = False
		pnlEsquemaAnalisis.Visible = True
		hfProyecto.Value = 0
		btnSaveProyecto.Visible = True
		btnCancelProject.Visible = True
		txtJobBookProyecto.Text = txtJobBook.Text
	End Sub

	Sub deselect_RB_in_gridview()
		Dim gvr As GridViewRow
		Dim i As Int32
		'deselect all radiobutton in gridview
		For Each gvr In gvPresupuestos.Rows
			Dim rb As RadioButton
			rb = CType(gvPresupuestos.Rows(i).FindControl("chkAsignar"), RadioButton)
			rb.Checked = False
			i += 1
		Next
	End Sub

	Protected Sub chkAsignar_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
		deselect_RB_in_gridview()
		'deselect radiobutton1
		'RadioButton1.Checked = False
		'check the radiobutton which is checked
		Dim SenderRB As RadioButton = sender
		SenderRB.Checked = True
		'--------------------------------------
		'Reflect the event
		'---------------------------------------
		'fire_visible_window()
		chbProjectCuali.Checked = False
		chbProjectCuanti.Checked = False
		Dim oCot As New CoreProject.Cotizador.General
		For Each row As GridViewRow In gvPresupuestos.Rows
			If DirectCast(row.FindControl("chkAsignar"), RadioButton).Checked = True Then
				Dim InfoP = oCot.GetTiposPresupuestoXCuPresupuesto(gvPresupuestos.DataKeys(row.RowIndex)("Id"))
				If InfoP.Cuali > 0 Then chbProjectCuali.Checked = True
				If InfoP.Cuanti > 0 Then chbProjectCuanti.Checked = True
			End If
		Next
	End Sub

	Protected Sub gvProyectos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
		If e.CommandName = "EditP" Then
			LoadProyecto(Int64.Parse(gvProyectos.DataKeys(CInt(e.CommandArgument))("Id")))
		End If
	End Sub

	Sub LoadProyecto(ByVal idProyecto As Int64)
		hfProyecto.Value = idProyecto
		Dim oProyecto As New CoreProject.Proyecto
		Dim Proyecto As New CoreProject.PY_Proyectos
		pnlDatosProyectos.Visible = True
		pnlEsquemaAnalisis.Visible = True
		pnlListadoProyectos.Visible = True
		btnChangeAlternativa.Visible = False
		Proyecto = oProyecto.ObtenerProyecto(idProyecto)
		txtA1.Text = Proyecto.A1
		txtA2.Text = Proyecto.A2
		txtA3.Text = Proyecto.A3
		txtA4.Text = Proyecto.A4
		txtA5.Text = Proyecto.A5
		txtA6.Text = Proyecto.A6
		txtA7.Text = Proyecto.A7
		txtA8.Text = Proyecto.A8
		hfProyecto.Value = Proyecto.id
		txtJobBookProyecto.Text = Proyecto.JobBook
		txtNombreProyecto.Text = Proyecto.Nombre
		btnSaveProyecto.Visible = True
		btnCancelProject.Visible = True
		btnSave.Visible = False
		btnCancel.Visible = False
		btnChangeAlternativa.Visible = False
		CargarUnidades()
		If Proyecto.TipoProyectoId = 2 Then
			ddlUnidad.Items.Add(New ListItem With {.Text = "IUU - Ipsos UU", .Value = 17})
		End If
		ddlUnidad.SelectedValue = Proyecto.UnidadId
	End Sub

	Protected Sub btnSaveProyecto_Click(sender As Object, e As EventArgs)
		Dim oProyecto As New CoreProject.Proyecto
		Dim Proyecto As New CoreProject.PY_Proyectos
		If Not (hfProyecto.Value = 0) Then Proyecto = oProyecto.ObtenerProyecto(hfProyecto.Value)
		Proyecto.A1 = txtA1.Text
		Proyecto.A2 = txtA2.Text
		Proyecto.A3 = txtA3.Text
		Proyecto.A4 = txtA4.Text
		Proyecto.A5 = txtA5.Text
		Proyecto.A6 = txtA6.Text
		Proyecto.A7 = txtA7.Text
		Proyecto.A8 = txtA8.Text
		Proyecto.TipoProyectoId = ddlTipoProyecto.SelectedValue
		Proyecto.EstudioId = hfEstudio.Value
		Proyecto.JobBook = txtJobBook.Text
		Proyecto.Nombre = txtNombreProyecto.Text
		Proyecto.UnidadId = ddlUnidad.SelectedValue
		Proyecto.Estado = 1
		oProyecto.GuardarProyecto(Proyecto)
		hfProyecto.Value = Proyecto.id
		CargarProyectos()
	End Sub

	Protected Sub btnCancelProject_Click(sender As Object, e As EventArgs)
		pnlListadoProyectos.Visible = True
		pnlDatosProyectos.Visible = False
		btnSave.Visible = True
		btnCancel.Visible = True
		btnChangeAlternativa.Visible = True
		pnlDatosProyectos.Visible = False
		pnlEsquemaAnalisis.Visible = False
		'btnViewEsquemaAnalisis.Visible = True
	End Sub

	Private Sub CargarUnidades()
		Dim oUnidades As New CoreProject.US.Unidades
		ddlUnidad.DataSource = oUnidades.ObtenerUnidadesxUsuario(Session("IDUsuario").ToString)
		ddlUnidad.DataTextField = "Unidad"
		ddlUnidad.DataValueField = "id"
		ddlUnidad.DataBind()
	End Sub

	Protected Sub ddlTipoProyecto_SelectedIndexChanged(sender As Object, e As EventArgs)
		CargarUnidades()
		If ddlTipoProyecto.SelectedValue = 2 Then
			ddlUnidad.Items.Add(New ListItem With {.Text = "IUU - Ipsos UU", .Value = 17})
		End If
	End Sub

	Sub ClearProyecto()
		txtA1.Text = String.Empty
		txtA2.Text = String.Empty
		txtA3.Text = String.Empty
		txtA4.Text = String.Empty
		txtA5.Text = String.Empty
		txtA6.Text = String.Empty
		txtA7.Text = String.Empty
		txtA8.Text = String.Empty
		hfProyecto.Value = 0
		txtJobBookProyecto.Text = txtJobBook.Text
		txtNombreProyecto.Text = String.Empty
	End Sub
End Class