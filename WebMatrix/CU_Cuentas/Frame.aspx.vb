Imports DevExpress.Web.ASPxHtmlEditor
Public Class Frame
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			SetupEditors()
			PreFillData()
			CargarUnidades()
			If Not (Session("InfoJobBook") Is Nothing) Then
				hfBrief.Value = DirectCast(Session("InfoJobBook"), oJobBook).IdBrief
				LoadInfoJobBook()
				If Not hfBrief.Value = 0 Then LoadDataBrief(hfBrief.Value)
			End If
		End If
	End Sub

	Sub PreFillData()
		Dim textSituacion As String = "<p><strong>&iquest;Cu&aacute;l es tu marca o categor&iacute;a?</strong> Escribe marca o categor&iacute;a y, si es posible, per&iacute;odos de tiempo de esta situaci&oacute;n (a&ntilde;os, meses).</p>
		<p>&nbsp;&nbsp;</p>
		<p><strong>&iquest;Qui&eacute;n es tu consumidor o comprador?</strong> Grupo objetivo de tu marca o categor&iacute;a (Geograf&iacute;as, g&eacute;nero, edad, estilo de vida, buyer persona, m&eacute;tricas o KPI de compra o consumo, etc.).</p>
		<p>&nbsp;&nbsp;</p>
		<p><strong>&iquest;Canales de compra de tu marca o categor&iacute;a?</strong>&nbsp;</p>
		<ul>
		<li>Canal moderno (grandes superficies)</li>
		<li>Canal tradicional/ independientes</li>
		<li>Tiendas propias (f&iacute;sico)</li>
		<li>Online (e-commerce web, app o m&oacute;vil)</li>
		<li>Venta directa/ Direct to consumer</li>
		<li>Otras &iquest;Cu&aacute;les?</li>
		</ul>
		<p>&nbsp;</p>
		<p><strong>&iquest;Cu&aacute;l es tu objetivo de negocio?</strong> El estado final aspiracional de lo que quieres lograr. Punto de partida, fortalezas, &eacute;xitos.</p>
		<p>&nbsp;</p>"

		Dim textComplicacion As String = "<p><strong>&iquest;Qu&eacute; ha cambiado?</strong> Cambios o desaf&iacute;os que pueden requerir que tomes decisiones o acciones.</p>
		<p>&nbsp;&nbsp;</p>
		<p><strong>&iquest;Cu&aacute;l es la complicaci&oacute;n de las cosas?</strong> Problemas que le impiden alcanzar su objetivo (Mercado, consumidor, competencia, etc.)</p>
		<p>&nbsp;&nbsp;</p>
		<p><strong>&iquest;Cu&aacute;les son las consecuencias de hacer o no hacer algo?</strong> Si no haces nada al respecto, que puede pasar.</p>
		<p></p>"

		Dim textQuestion As String = "<p><strong>Escribe la Pregunta o Reto de negocios.</strong></p>
		<p>Te ayuda a conocer la acci&oacute;n principal para cubrir / cumplir con los objetivos y las metas de la marca o categor&iacute;a. La pregunta debe ser precisa y completa; expresada en forma de ACCI&Oacute;N DE NEGOCIOS (palancas comerciales o de mercadeo)</p>
		<p></p>
		<p><strong>&iquest;Cu&aacute;les son las decisiones o acciones que vamos a tomar? </strong>Es muy importante describirla como un problema de acci&oacute;n, no de investigaci&oacute;n. Escribe que decisiones o acciones vamos a realizar.</p>
		<p>&nbsp;</p>
		<p><strong>&iquest;Cu&aacute;les son tus Hip&oacute;tesis o Est&aacute;ndares de Acci&oacute;n?</strong> Que crees que debe ser cierto para resolver tu reto de negocios. M&eacute;tricas o KPI de compra o consumo que quieres mover o alcanzar a partir de acciones basadas en insights.</p>
		<p></p>"

		Dim textEvidencia As String = "<p><strong>&iquest;Cu&aacute;ntos y cu&aacute;les productos o marcas quieres evaluar? </strong>Cantidad aproximada de &ldquo;est&iacute;mulos&rdquo; (Tambi&eacute;n es importante <u>describir</u> si son productos terminados, prototipos, im&aacute;genes digitales, conceptos, empaques, etc.&nbsp;</p>
		<p>&nbsp;</p>
		<p><strong>&iquest;Metodolog&iacute;as de investigaci&oacute;n sugeridas?</strong></p>
		<ul>
		<li>Cualitativa</li>
		<li>Cuantitativa</li>
		<li>Online/ M&oacute;vil</li>
		<li>Redes sociales</li>
		<li>Otras &iquest;Cu&aacute;les? __________________</li>
		</ul>
		<p>&nbsp;</p>
		<p><strong>&iquest;Fecha estimada en que quiere iniciar la investigaci&oacute;n, una vez aprobada? (DD/MM/AA)</strong></p>
		<p>&nbsp;</p>
		<p><strong>&iquest;Fecha estimada en que quiere tener los resultados finales? </strong><strong>(DD/MM/AA) </strong></p>
		<p>&nbsp;</p>
		<p><strong>&iquest;Tienes investigaciones o informaci&oacute;n documental?</strong> Otros estudios internos o externos que ayudan al proyecto o con las cuales se necesite comparar (investigaciones hist&oacute;ricas)</p>
		<p></p>"
		DevEdSituacion.Html = textSituacion
		DevEdComplicacion.Html = textComplicacion
		DevEdEvidencia.Html = textEvidencia
		DevEdPregunta.Html = textQuestion

	End Sub

	Sub SetupEditors()
		'HtmlEditorUtils.SetHtml(Me, DemoHtmlEditor, "Features/CustomToolbar.html")
		'DevEdSituacion.Toolbars.Add(CreateDemoCustomToolbar("CustomToolbar"))
		'DevEdSituacion.ToolbarMode = HtmlEditorToolbarMode.None
		'DevEdComplicacion.ToolbarMode = HtmlEditorToolbarMode.None
		'DevEdPregunta.ToolbarMode = HtmlEditorToolbarMode.None
		'DevEdEvidencia.ToolbarMode = HtmlEditorToolbarMode.None

		DevEdSituacion.Settings.AllowHtmlView = False
		DevEdSituacion.Settings.AllowPreview = False
		DevEdComplicacion.Settings.AllowHtmlView = False
		DevEdComplicacion.Settings.AllowPreview = False
		DevEdPregunta.Settings.AllowHtmlView = False
		DevEdPregunta.Settings.AllowPreview = False
		DevEdEvidencia.Settings.AllowHtmlView = False
		DevEdEvidencia.Settings.AllowPreview = False
	End Sub

	Protected Function CreateDemoCustomToolbar(ByVal name As String) As HtmlEditorToolbar
		Return New HtmlEditorToolbar(name, New ToolbarFontNameEdit(), New ToolbarFontSizeEdit(), New ToolbarJustifyLeftButton(True), New ToolbarJustifyCenterButton(), New ToolbarJustifyRightButton(), New ToolbarJustifyFullButton(), New ToolbarInsertLinkDialogButton(True)).CreateDefaultItems()
	End Function

	Private Sub CargarUnidades()
		Dim oUnidades As New CoreProject.US.Unidades
		ddlUnidades.DataSource = oUnidades.ObtenerUnidadesxUsuario(Session("IDUsuario").ToString)
		ddlUnidades.DataTextField = "Unidad"
		ddlUnidades.DataValueField = "id"
		ddlUnidades.DataBind()
	End Sub
	Sub LoadDataBrief(ByVal idBrief As Int64)
		Dim oBrief As New CoreProject.Brief
		Dim info = oBrief.ObtenerBriefXID(idBrief)
		txtEmpresa.Text = info.Cliente
		txtSolicitante.Text = info.Contacto

		txtNombreProyecto.Text = info.Titulo
		If Not info.Antecedentes = "" Then DevEdSituacion.Html = info.Antecedentes
		If Not info.Objetivos = "" Then DevEdComplicacion.Html = info.Objetivos
		If Not info.ActionStandars = "" Then DevEdPregunta.Html = info.ActionStandars
		If Not info.Metodologia = "" Then DevEdEvidencia.Html = info.Metodologia

		Try
			ddlUnidades.SelectedValue = info.Unidad
		Catch ex As Exception
			ddlUnidades.DataSource = Nothing
			ddlUnidades.DataBind()
			ddlUnidades.Items.Insert(0, New ListItem With {.Selected = True, .Text = "N/A", .Value = "-1"})
		End Try
		If info.Fecha IsNot Nothing Then txtFechaFrame.Text = info.Fecha
		txtProductoMarca.Text = info.MarcaCategoria
		If info.FechaViabilidad IsNot Nothing Then
			lblFechaViabilidad.Visible = True
			lblFechaViabilidad.Text = info.FechaViabilidad
			If info.Viabilidad = False Then
				btnNotViabilidad.Text = "BRIEF SIN VIABILIDAD"
				btnNotViabilidad.Enabled = False
				btnNotViabilidad.Visible = True
				btnViabilidadOk.Visible = False
			End If
		Else
			lblFechaViabilidad.Text = "Por definir"
			If DirectCast(Session("InfoJobBook"), oJobBook).GuardarCambios = True Then
				btnNotViabilidad.Visible = True
				btnViabilidadOk.Visible = True
			End If
		End If

		If DirectCast(Session("InfoJobBook"), oJobBook).GuardarCambios = False Then
			btnSave.Visible = False
		End If
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
		If Not info.NewClient Is Nothing Then chbNewClient.Checked = info.NewClient
		btnLoadFiles.Visible = True
	End Sub

	Sub LoadInfoJobBook()
		If Not (Session("InfoJobBook") Is Nothing) Then
			Dim infoJobBook As oJobBook = Session("InfoJobBook")
			lblInfo.Text = infoJobBook.NumJobBook & " | " & infoJobBook.Titulo & " | " & infoJobBook.Cliente & " | " & infoJobBook.IdPropuesta.ToString
			If infoJobBook.GuardarCambios = False Then
				btnSave.Visible = False
				btnNew.Visible = True
			End If
		End If
	End Sub

	Sub ClearForm()
		CargarUnidades()
		SetupEditors()
		PreFillData()
		hfBrief.Value = 0
		txtEmpresa.Text = String.Empty
		txtSolicitante.Text = String.Empty
		txtNombreProyecto.Text = String.Empty
		lblFechaViabilidad.Text = String.Empty
		btnNotViabilidad.Visible = False
		btnViabilidadOk.Visible = False
		btnLoadFiles.Visible = False
		txtO1.Text = String.Empty
		txtO2.Text = String.Empty
		txtO3.Text = String.Empty
		txtO4.Text = String.Empty
		txtO5.Text = String.Empty
		txtO6.Text = String.Empty
		txtO7.Text = String.Empty
		txtD1.Text = String.Empty
		txtD2.Text = String.Empty
		txtD3.Text = String.Empty
		txtC1.Text = String.Empty
		txtC2.Text = String.Empty
		txtC3.Text = String.Empty
		txtC4.Text = String.Empty
		txtC5.Text = String.Empty
		txtM1.Text = String.Empty
		txtM2.Text = String.Empty
		txtM3.Text = String.Empty
		txtDI1.Text = String.Empty
		txtDI2.Text = String.Empty
		txtDI3.Text = String.Empty
		txtDI4.Text = String.Empty
		txtDI5.Text = String.Empty
		txtDI6.Text = String.Empty
		txtDI7.Text = String.Empty
		txtDI8.Text = String.Empty
		txtDI9.Text = String.Empty
		txtDI10.Text = String.Empty
		txtDI11.Text = String.Empty
		txtDI12.Text = String.Empty
		txtDI13.Text = String.Empty
		txtDI14.Text = String.Empty
		txtDI15.Text = String.Empty
		txtDI16.Text = String.Empty
		txtDI17.Text = String.Empty
		txtDI18.Text = String.Empty
		chbNewClient.Checked = False
	End Sub
	Protected Sub btnNew_Click(sender As Object, e As EventArgs)
		ClearForm()
		Session("InfoJobBook") = Nothing
		lblInfo.Text = "Nuevo Frame"
		btnNew.Visible = False
		btnSave.Visible = True
	End Sub

	Public Function ValidateSave() As Boolean
		If Not IsDate(txtFechaFrame.Text) Then
			ShowWarning(TypesWarning.ErrorMessage, "Asegúrese de incluir la fecha de solicitud del brief")
			Return False
		End If
		If txtEmpresa.Text = "" Then
			ShowWarning(TypesWarning.ErrorMessage, "Por favor incluya el nombre de la empresa")
			Return False
		End If
		If txtSolicitante.Text = "" Then
			ShowWarning(TypesWarning.ErrorMessage, "Por favor incluya quién solicitó el brief (contacto por parte del cliente)")
			Return False
		End If
		If txtProductoMarca.Text = "" Then
			ShowWarning(TypesWarning.ErrorMessage, "Por favor escriba el nombre del producto o marca a evaluar")
			Return False
		End If
		If txtNombreProyecto.Text = "" Then
			ShowWarning(TypesWarning.ErrorMessage, "Por favor escriba el nombre del proyecto, el cual le permitirá identificarlo de ahora en adelante")
			Return False
		End If
		Return True
	End Function

	Protected Sub btnSave_Click(sender As Object, e As EventArgs)
		If ValidateSave() = False Then
			Exit Sub
		End If
		Dim oBrief As New CoreProject.Brief
		Dim ent As New CoreProject.CU_Brief
		Dim NewBrief As Boolean = True
		If hfBrief.Value > 0 Then
			ent = oBrief.ObtenerBriefXID(hfBrief.Value)
			NewBrief = False
		End If
		ent.Fecha = txtFechaFrame.Text
		ent.MarcaCategoria = txtProductoMarca.Text
		ent.Cliente = txtEmpresa.Text
		ent.Contacto = txtSolicitante.Text
		ent.Titulo = txtNombreProyecto.Text
		ent.Antecedentes = DevEdSituacion.Html
		ent.Objetivos = DevEdComplicacion.Html
		ent.ActionStandars = DevEdPregunta.Html
		ent.Metodologia = DevEdEvidencia.Html

		ent.TipoBrief = 1
		ent.NewClient = chbNewClient.Checked

		If NewBrief = True Then
			ent.Unidad = ddlUnidades.SelectedValue
			ent.GerenteCuentas = Session("IDUsuario").ToString
			ent.FechaViabilidad = Date.UtcNow.AddHours(-5)
			ent.Viabilidad = True
			ent.MarcaViabilidad = Session("IDUsuario").ToString
			lblFechaViabilidad.Text = ent.FechaViabilidad
		End If

		hfBrief.Value = oBrief.GuardarBrief(ent)
		If NewBrief = True Then
			Dim infoJ As New oJobBook
			Dim oData As New CoreProject.CU_JobBook.DAL
			Dim rData = oData.InfoJobBookGet(idBrief:=hfBrief.Value).FirstOrDefault
			infoJ.Cliente = rData.Cliente
			infoJ.Estado = rData.Estado
			infoJ.GerenteCuentas = rData.GerenteCuentas
			infoJ.GerenteCuentasID = rData.GerenteCuentasID
			infoJ.IdBrief = rData.IdBrief
			infoJ.IdEstudio = rData.IdEstudio
			infoJ.IdPropuesta = rData.IdPropuesta
			infoJ.IdUnidad = rData.IdUnidad
			infoJ.MarcaCategoria = rData.MarcaCategoria
			infoJ.Titulo = rData.Titulo
			infoJ.Unidad = rData.Unidad
			infoJ.Viabilidad = rData.Viabilidad
			infoJ.NumJobBook = rData.NumJobbook
			infoJ.GuardarCambios = True
			lblInfo.Text = infoJ.Titulo & " | " & infoJ.Cliente
			btnNotViabilidad.Visible = False
			btnViabilidadOk.Visible = False
			infoJ.IdPropuesta = SavePropuesta()
			Session("InfoJobBook") = infoJ
			ShowWarning(TypesWarning.Information, "Se ha almacenado y el brief y se ha creado la información de la propuesta. Vaya a la sección Propuesta y asegúrese de actualizar la información, INCLUYENDO LA FECHA DE INICIO DE CAMPO")
		Else
			ShowWarning(TypesWarning.Information, "Se han actualizado los datos")
		End If
		btnLoadFiles.Visible = True
	End Sub

	Function SavePropuesta() As Int64
		Dim oPropuesta As New CoreProject.Propuesta
		Return oPropuesta.Guardar(0, txtNombreProyecto.Text, 1, 50, Nothing, 1, Nothing, Nothing, Nothing, "", hfBrief.Value, False, "", False, 70, 30, 30, Now.Date.AddDays(8).Date, "De acuerdo a la ley colombiana")
	End Function

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

	Protected Sub btnViabilidadOk_Click(sender As Object, e As EventArgs)
		Dim oBrief As New CoreProject.Brief
		Dim ent As New CoreProject.CU_Brief
		ent = oBrief.ObtenerBriefXID(hfBrief.Value)
		ent.FechaViabilidad = Date.UtcNow.AddHours(-5)
		ent.Viabilidad = True
		ent.MarcaViabilidad = Session("IDUsuario").ToString
		lblFechaViabilidad.Text = ent.FechaViabilidad
		oBrief.GuardarBrief(ent)
		btnNotViabilidad.Visible = False
		btnViabilidadOk.Visible = False
		Dim infoJobBook As oJobBook = Session("InfoJobBook")
		If infoJobBook.IdPropuesta = 0 Then infoJobBook.IdPropuesta = SavePropuesta()
		infoJobBook.Viabilidad = True
		Session("InfoJobBook") = infoJobBook
		ShowWarning(TypesWarning.Information, "Se ha marcado la viablidad del brief. Se ha creado la información de la propuesta. Vaya a la sección Propuesta y asegúrese de completar la información")
	End Sub

	Protected Sub btnNotViabilidad_Click(sender As Object, e As EventArgs)
		Dim oBrief As New CoreProject.Brief
		Dim ent As New CoreProject.CU_Brief
		ent = oBrief.ObtenerBriefXID(hfBrief.Value)
		ent.FechaViabilidad = Date.UtcNow.AddHours(-5)
		ent.Viabilidad = False
		ent.MarcaViabilidad = Session("IDUsuario").ToString
		lblFechaViabilidad.Text = ent.FechaViabilidad
		oBrief.GuardarBrief(ent)
		btnNotViabilidad.Text = "BRIEF SIN VIABILIDAD"
		btnNotViabilidad.Enabled = False
		btnViabilidadOk.Visible = False
		Dim infoJobBook As oJobBook = Session("InfoJobBook")
		infoJobBook.Viabilidad = False
		Session("InfoJobBook") = infoJobBook
		ShowWarning(TypesWarning.Warning, "Se ha marcado la NO viablidad del brief. No podrá ejecutar más procesos en este job")
	End Sub

	Protected Sub LoadFiles_Click(sender As Object, e As EventArgs)
		If btnLoadFiles.Text = "Ocultar Carga de archivos" Then
			pnlLoadFiles.Visible = False
			btnLoadFiles.Text = "Ver / Cargar Archivos"
		Else
			Dim oContenedor As New oContenedorDocumento
			oContenedor.ContenedorId = hfBrief.Value
			oContenedor.DocumentoId = 1
			Session("oContenedorDocumento") = oContenedor
			pnlLoadFiles.Visible = True
			UCFiles.ContenedorId = hfBrief.Value
			UCFiles.DocumentoId = 1
			UCFiles.CargarDocumentos()

			btnLoadFiles.Text = "Ocultar Carga de archivos"
		End If
	End Sub
End Class