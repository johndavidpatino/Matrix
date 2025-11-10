Public Class PropuestaForm
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			If Not (Session("InfoJobBook") Is Nothing) Then
				hfPropuesta.Value = DirectCast(Session("InfoJobBook"), oJobBook).IdPropuesta
				LoadInfoJobBook()
				If Not hfPropuesta.Value = 0 Then LoadDataPropuesta(hfPropuesta.Value)
			End If
		End If
	End Sub

	Sub LoadDataPropuesta(ByVal IdPropuesta As Int64)
		Dim oPropuesta As New CoreProject.Propuesta
		Dim info = oPropuesta.DevolverxID(IdPropuesta)
		If info.JobBook IsNot Nothing Then
			txtJobBook.Text = info.JobBook
		End If
		If info.ProbabilidadId IsNot Nothing Then
			ddlprobabilidadaprob.SelectedValue = info.ProbabilidadId
		End If

		If info.FechaEnvio IsNot Nothing Then
			txtFechaEnvio.Text = info.FechaEnvio
		End If

		If info.EstadoId IsNot Nothing Then
			ddlestadopropuesta.SelectedValue = info.EstadoId
		End If

		If info.FechaAprob IsNot Nothing Then
			txtFechaAprobacion.Text = info.FechaAprob
		End If

		If info.FechaInicioCampo IsNot Nothing Then
			Me.txtFechaInicioCampo.Text = info.FechaInicioCampo
		End If
		If info.RequestHabeasData IsNot Nothing Then
			txtHabeasData.Text = info.RequestHabeasData
		End If
		txtAnticipo.Text = info.Anticipo
		txtSaldo.Text = info.Saldo
		txtPlazoPago.Text = info.Plazo
		If DirectCast(Session("InfoJobBook"), oJobBook).GuardarCambios = True Then
			btnSave.Visible = True
		End If
		btnLoadFiles.Visible = True
	End Sub
	Sub LoadInfoJobBook()
		If Not (Session("InfoJobBook") Is Nothing) Then
			Dim infoJobBook As oJobBook = Session("InfoJobBook")
			lblInfo.Text = infoJobBook.NumJobBook & " | " & infoJobBook.Titulo & " | " & infoJobBook.Cliente
		End If
	End Sub

	Function ValidateSave() As Boolean
		If Not (txtFechaEnvio.Text = "") And Not (IsDate(txtFechaEnvio.Text)) Then
			ShowWarning(TypesWarning.ErrorMessage, "La fecha de envío no es válida")
			Return False
		End If
		If Not (txtFechaAprobacion.Text = "") And Not (IsDate(txtFechaAprobacion.Text)) Then
			ShowWarning(TypesWarning.ErrorMessage, "La fecha de envío no es válida")
			Return False
		End If
		If Not (IsDate(txtFechaInicioCampo.Text)) Then
			ShowWarning(TypesWarning.ErrorMessage, "La fecha estimada de inicio de campo es requerida")
			Return False
		End If
		If txtHabeasData.Text = "" Then
			txtHabeasData.Text = "De acuerdo a la ley colombiana"
		End If
		If Not (IsNumeric(txtAnticipo.Text)) Then
			ShowWarning(TypesWarning.ErrorMessage, "El porcentaje de anticipo no es válido")
			Return False
		End If
		If Not (IsNumeric(txtSaldo.Text)) Then
			ShowWarning(TypesWarning.ErrorMessage, "El porcentaje del saldo no es válido")
			Return False
		End If
		If Not (IsNumeric(txtPlazoPago.Text)) Then
			ShowWarning(TypesWarning.ErrorMessage, "El plazo de pago no es válido")
			Return False
		End If
		Return True
	End Function
	Protected Sub btnSave_Click(sender As Object, e As EventArgs)
		If ValidateSave() = False Then
			Exit Sub
		End If
		Dim oPropuesta As New CoreProject.Propuesta
		Dim ent As New CoreProject.CU_Propuestas
		ent = oPropuesta.ObtenerPropuestaXID(hfPropuesta.Value)
		If Not (txtJobBook.Text = "") Then
			ent.JobBook = txtJobBook.Text
		Else
			ShowWarning(TypesWarning.Warning, "La información ha sido almacenada, pero asegúrese de actualizar el número del jobbook (creado en symphony) antes de crear presupuestos")
		End If
		ent.ProbabilidadId = ddlprobabilidadaprob.SelectedValue
		ent.EstadoId = ddlestadopropuesta.SelectedValue
		ent.RequestHabeasData = txtHabeasData.Text
		ent.Anticipo = txtAnticipo.Text
		ent.Saldo = txtSaldo.Text
		ent.Plazo = txtPlazoPago.Text

		If (IsDate(txtFechaEnvio.Text)) Then
			ent.FechaEnvio = txtFechaEnvio.Text
		End If
		If (IsDate(txtFechaAprobacion.Text)) Then
			ent.FechaAprob = txtFechaAprobacion.Text
		End If
		If (IsDate(txtFechaInicioCampo.Text)) Then
			ent.FechaInicioCampo = txtFechaInicioCampo.Text
		End If
		oPropuesta.GuardarPropuesta(ent)
		If Not (txtJobBook.Text = "") Then
			ShowWarning(TypesWarning.Information, "Los datos han sido actualizados")
			If Not (txtJobBook.Text = "") Then
				Dim infoJobBook As oJobBook = Session("InfoJobBook")
				infoJobBook.NumJobBook = txtJobBook.Text
				Session("InfoJobBook") = infoJobBook
			End If
		End If
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

	Protected Sub LoadFiles_Click(sender As Object, e As EventArgs)
		If btnLoadFiles.Text = "Ocultar Carga de archivos" Then
			pnlLoadFiles.Visible = False
			btnLoadFiles.Text = "Ver / Cargar Archivos"
		Else
			Dim oContenedor As New oContenedorDocumento
			oContenedor.ContenedorId = hfPropuesta.Value
			oContenedor.DocumentoId = 2
			Session("oContenedorDocumento") = oContenedor
			pnlLoadFiles.Visible = True
			UCFiles.ContenedorId = hfPropuesta.Value
			UCFiles.DocumentoId = 2
			UCFiles.CargarDocumentos()

			btnLoadFiles.Text = "Ocultar Carga de archivos"
		End If
	End Sub
End Class