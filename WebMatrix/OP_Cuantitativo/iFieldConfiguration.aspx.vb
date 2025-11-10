Imports DevExpress.Web.ASPxHtmlEditor
Imports CoreProject
Public Class iFieldConfiguration
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			LoadProjects(DALDAP.DTOiField.TipoBusquedaProyectoiField.Activos)
		End If
	End Sub


	Protected Sub btnNew_Click(sender As Object, e As EventArgs)

	End Sub

	Sub Clear()
		txtNumTrabajo.Text = ""
		txtNumTrabajo.Enabled = False
		lblNombreTrabajo.Text = ""
		gvDataConfiguracion.DataSource = Nothing
		gvDataConfiguracion.DataBind()
		txtNewConfig.Text = ""
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

	Protected Sub txtJobBookSearch_TextChanged(sender As Object, e As EventArgs)
		Dim oTrabajo As New Trabajo
		Dim oeTrabajo = oTrabajo.ObtenerTrabajo(txtNumTrabajo.Text)
		If oeTrabajo Is Nothing Then
			ShowWarning(TypesWarning.ErrorMessage, "El trabajo digitado no existe")
			Exit Sub
		End If
		lblNombreTrabajo.Text = oeTrabajo.NombreTrabajo
	End Sub

	Sub LoadProjects(ByVal TipoBusqueda As CoreProject.DALDAP.DTOiField.TipoBusquedaProyectoiField)
		Dim oDAL As New DALDAP.iFieldSettings
		ddlProyectosIField.DataSource = oDAL.ProjectsGetFromMatrix(rbSearch.SelectedValue)
		ddlProyectosIField.DataTextField = "NombreProjecto"
		ddlProyectosIField.DataValueField = "IdProjecto"
		ddlProyectosIField.DataBind()
		ddlProyectosIField.Items.Insert(0, New ListItem With {.Text = "-- Seleccione --", .Value = "0"})
	End Sub

	Private Sub rbSearch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbSearch.SelectedIndexChanged
		LoadProjects(rbSearch.SelectedValue)
		Clear()
	End Sub

	Protected Sub ddlProyectosIField_SelectedIndexChanged(sender As Object, e As EventArgs)
		Clear()
		If ddlProyectosIField.SelectedValue = 0 Then
			btnConfirm.Visible = False
			Exit Sub
		End If
		btnConfirm.Visible = True
		txtNumTrabajo.Enabled = True
		Dim oDAL As New DALDAP.iFieldSettings
		Dim info = oDAL.ProjectGet(ddlProyectosIField.SelectedValue)
		If info Is Nothing Then
			Exit Sub
		End If
		If info.TrabajoId Is Nothing Then
			Exit Sub
		End If
		txtNumTrabajo.Text = info.TrabajoId
		If rbSearch.SelectedValue < 3 Then
			txtNumTrabajo.Enabled = True
		End If
		Dim oTrabajo As New Trabajo
		Dim oeTrabajo = oTrabajo.ObtenerTrabajo(txtNumTrabajo.Text)
		lblNombreTrabajo.Text = oeTrabajo.NombreTrabajo
		LoadConfig(ddlProyectosIField.SelectedValue)
		LoadMIssingSync(ddlProyectosIField.SelectedValue)
	End Sub

	Protected Sub btnConfirm_Click(sender As Object, e As EventArgs)
		If Not (IsNumeric(txtNumTrabajo.Text)) Then
			lblNombreTrabajo.Text = ""
			ShowWarning(TypesWarning.Warning, "Debe digitar un número de trabajo válido")
			Exit Sub
		End If
		Dim oTrabajo As New Trabajo
		Dim oeTrabajo = oTrabajo.ObtenerTrabajo(txtNumTrabajo.Text)
		If oeTrabajo Is Nothing Then
			lblNombreTrabajo.Text = ""
			ShowWarning(TypesWarning.ErrorMessage, "El trabajo digitado no existe")
			Exit Sub
		End If
		Dim oDAL As New DALDAP.iFieldSettings
		oDAL.UpdateProject(ddlProyectosIField.SelectedValue, txtNumTrabajo.Text)
	End Sub

	Sub LoadConfig(ByVal IdProyecto As Integer)
		Dim oDAL As New DALDAP.iFieldSettings
		gvDataConfiguracion.DataSource = oDAL.ProjectConfigGet(IdProyecto)
		gvDataConfiguracion.DataBind()
	End Sub

	Sub LoadMIssingSync(ByVal IdProyecto As Integer)
		Dim oDAL As New DALDAP.iFieldSettings
		gvPendientes.DataSource = oDAL.EncuestasPendientes(IdProyecto)
		gvPendientes.DataBind()
	End Sub

	Protected Sub gvDataConfiguracion_RowCommand(sender As Object, e As GridViewCommandEventArgs)
		Dim oDAL As New DALDAP.iFieldSettings
		oDAL.RemoveConfigProject(Int64.Parse(gvDataConfiguracion.DataKeys(CInt(e.CommandArgument))("id")))
		LoadConfig(ddlProyectosIField.SelectedValue)
	End Sub

	Protected Sub btnAddConfiguration_Click(sender As Object, e As EventArgs)
		Dim oDAL As New DALDAP.iFieldSettings
		Dim info = oDAL.ProjectGet(ddlProyectosIField.SelectedValue)
		If info.TrabajoId Is Nothing Then
			ShowWarning(TypesWarning.ErrorMessage, "Primero debe configurar el número de trabajo del proyecto")
			Exit Sub
		End If
		Dim datos() As String
		datos = txtNewConfig.Text.Split(Chr(10).ToString.ToCharArray)
		Dim flagOk As Boolean = True
		For i = 0 To datos.Length - 1
			Dim datos2() As String
			datos2 = datos(i).ToString.Split(Chr(9).ToString.ToCharArray)
			If (datos2.Length = 3) Then
				Try
					If IsNumeric(datos2(1)) And IsNumeric(datos2(2)) Then
						oDAL.InsertConfigItem(ddlProyectosIField.SelectedValue, datos2(0).ToString, datos2(1), datos2(2).Replace(Chr(13), ""), Session("IDUsuario").ToString)
					Else
						flagOk = False
						'ShowWarning(TypesWarning.Warning, "Verifique la información suministrada ya que no es válida")
						'Exit Sub
					End If
				Catch ex As Exception
					flagOk = False
					'ShowWarning(TypesWarning.Warning, "Verifique la información suministrada ya que no es válida")
					'Exit Sub
				End Try
			End If
		Next
		LoadConfig(ddlProyectosIField.SelectedValue)
		If flagOk = False Then
			ShowWarning(TypesWarning.Warning, "Algunos datos presentaron problemas al subir, por favor verifique la información que ha sido cargada")
		Else
			ShowWarning(TypesWarning.Information, "Se ha cargado la nueva información")
		End If
		txtNewConfig.Text = ""
	End Sub
End Class