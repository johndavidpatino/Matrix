Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Public Class Estudios
	Inherits System.Web.UI.Page


	Private _idUsuario As Int64
	Public Property idUsuario() As Int64
		Get
			Return _idUsuario
		End Get
		Set(ByVal value As Int64)
			_idUsuario = value
		End Set
	End Property


#Region "Eventos"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		idUsuario = Session("IDUsuario")

		If Me.IsPostBack = False Then
			Dim permisos As New Datos.ClsPermisosUsuarios
			Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
			If permisos.VerificarPermisoUsuario(6, UsuarioID) = False Then
				btnGrabar.Enabled = False
				gvEstudios.Columns.Item(10).Visible = False
				btnCrearEstudio.Enabled = False
			End If
            CargarEstudios()
            CargarDocumentosSoporte()

            If Request.QueryString("IdPropuesta") IsNot Nothing Then
				Dim IdPropuesta As Int64
				Long.TryParse(Request.QueryString("IdPropuesta"), IdPropuesta)
				If IdPropuesta > 0 Then
					Dim oPropuesta As New Propuesta
					If Not (oPropuesta.DevolverxID(IdPropuesta).JobBook = "") Then
						Dim JobBook As String = oPropuesta.DevolverxID(IdPropuesta).JobBook
						If JobBook.Length = 9 Then
							Me.txtJobBook.Text = JobBook & "-00"
							Me.txtJobBook.Visible = True
							Me.txtJobBookInt.Text = ""
							Me.txtJobBookInt.Visible = False
						Else
							Me.txtJobBookInt.Text = JobBook & "-00"
							Me.txtJobBookInt.Visible = True
							Me.txtJobBook.Text = ""
							Me.txtJobBook.Visible = False
						End If
						Me.txtNombreEstudio.Text = oPropuesta.DevolverxID(IdPropuesta).Titulo
						Me.btnCorreccionEstudio.Enabled = False
						Me.btnDocumentos.Enabled = False
					End If
					Me.gvPresupuestos.Enabled = True
					txtPropuesta.Text = IdPropuesta
					CargarPresupuestos(IdPropuesta)
					accordion0.Visible = False
					accordion1.Visible = True
				End If
			End If
			Validar()
		End If
	End Sub
	Protected Sub txtPropuesta_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtPropuesta.TextChanged
		If IsNumeric(txtPropuesta.Text) Then
			CargarPresupuestos(CLng(txtPropuesta.Text))
		Else
			CargarPresupuestos(0)
		End If
		ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
	End Sub
	Private Sub gvEstudios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEstudios.RowCommand
		If e.CommandName = "PresupuestosAsignados" Then
			CargarPresupuestosXEstudio(Int64.Parse(Me.gvEstudios.DataKeys(CInt(e.CommandArgument))("Id")))
		ElseIf e.CommandName = "Actualizar" Then
			hfIdEstudio.Value = Me.gvEstudios.DataKeys(CInt(e.CommandArgument))("Id")
			CargarEstudio(Me.gvEstudios.DataKeys(CInt(e.CommandArgument))("Id"))
			Me.pnlCambiosPresupuestos.Visible = False
			Me.pnlEstudio.Visible = True
			accordion0.Visible = False
			accordion1.Visible = True
		ElseIf e.CommandName = "Trabajos" Then
			hfIdEstudio.Value = Me.gvEstudios.DataKeys(CInt(e.CommandArgument))("Id")
			Response.Redirect("TrabajosCuentas.aspx?EstudioId=" & hfIdEstudio.Value)
		End If
	End Sub
	Protected Sub btnGrabar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGrabar.Click
		Dim idpresup As Int64
		Dim flag As Boolean = False
		For Each row As GridViewRow In gvPresupuestos.Rows
			If DirectCast(row.FindControl("chkAsignar"), RadioButton).Checked = True Then
				flag = True
				idpresup = gvPresupuestos.DataKeys(row.RowIndex).Value
			End If
		Next
		If flag = False Then
			ShowNotification("Debe seleccionar un presupuesto antes de continuar", ShowNotifications.ErrorNotification)
			ActivateAccordion(1, EffectActivateAccordion.NoEffect)
			Exit Sub
		End If
        If Not String.IsNullOrEmpty(txtFechaInicioCampo.Text) Then
            If ValidarFecha(txtFechaInicioCampo.Text) Then
                If Not (Date.Parse(txtFechaInicioCampo.Text) > Now()) Then
                    ShowNotification("La fecha de inicio de campo debe ser mayor a la fecha actual", ShowNotifications.ErrorNotification)
                    ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                    Exit Sub
                    Throw New Exception("La fecha de inicio de campo debe ser mayor a la fecha actual")
                End If
            Else
                ShowNotification("Fecha de Inicio de Campo No Válida", ShowNotifications.ErrorNotification)
                Exit Sub
                Throw New Exception("Fecha de Inicio de Campo No Válida")
            End If
        Else
            ShowNotification("Digite la fecha de Inicio de Campo", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
            Throw New Exception("Digite la fecha de Inicio de Campo")
        End If
        If ddlDocumentoSoporte.SelectedValue = "-1" Then
            ddlDocumentoSoporte.Focus()
            ShowNotification("Seleccione el documento soporte", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
            Throw New Exception("Seleccione el documento soporte")
        End If
        If Not (IsNumeric(txtTiempoRetencion.Text)) Then
            txtTiempoRetencion.Focus()
            ShowNotification("El tiempo de retención debe ser numérico", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
            Throw New Exception("El tiempo de retención debe ser numérico")
        End If

        Dim oEstudios_Presupuestos As New Estudios_Presupuestos
		Dim oEstudio As New CU_Estudios
		Dim indPresupuestosAsignados As String = hfIndicesFilasPresupuestosAsignados.Value
		Dim indPresupuestosOriginales As String = hfIndicesFilasPresupuestosOriginales.Value
		Dim vecIndPresupuestosAsignados As String()
		Dim vecIndPresupuestosOriginales As String()
		Dim listCU_Estudios_Presupuestos As New List(Of CU_Estudios_Presupuestos)
		Dim listCU_Estudios_Presupuestos_Nuevos As New List(Of CU_Estudios_Presupuestos)
		Dim listCU_Estudios_Presupuestos_Eliminados As New List(Of CU_Estudios_Presupuestos)
		Dim oBLEstudio As New Estudio


		Dim idEstudio As String = hfIdEstudio.Value
		Try

			vecIndPresupuestosAsignados = indPresupuestosAsignados.Split(";").Where(Function(x) Not (String.IsNullOrEmpty(x))).ToArray
			vecIndPresupuestosOriginales = indPresupuestosOriginales.Split(";").Where(Function(x) Not (String.IsNullOrEmpty(x))).ToArray
			If txtJobBook.Text = "" Then
				oEstudio.JobBook = txtJobBookInt.Text
			Else
				oEstudio.JobBook = txtJobBook.Text
			End If
			If oEstudio.JobBook = "" Or oEstudio.JobBook.EndsWith("00") Then
				ShowNotification("Debe escribir un número de JobBook válido antes de continuar", ShowNotifications.ErrorNotification)
				ActivateAccordion(1, EffectActivateAccordion.NoEffect)
				Exit Sub
			End If
			Dim oPropuesta As New Propuesta
			Dim jbPropuesta As String = oPropuesta.DevolverxID(txtPropuesta.Text).JobBook
			If Not (oEstudio.JobBook.StartsWith(jbPropuesta)) Then
				ShowNotification("El JobBook debe mantener la misma estructura desde la propuesta", ShowNotifications.ErrorNotification)
				ActivateAccordion(1, EffectActivateAccordion.NoEffect)
				Exit Sub
			End If
			oEstudio.Nombre = txtNombreEstudio.Text
			oEstudio.PropuestaId = txtPropuesta.Text
			oEstudio.Valor = txtValor.Text
			oEstudio.Observaciones = txtObservaciones.Text
			If Not String.IsNullOrEmpty(txtFechaInicio.Text) Then
				'Validar formato de la fecha
				If ValidarFecha(txtFechaInicio.Text) Then
					oEstudio.FechaInicio = Date.Parse(txtFechaInicio.Text)
				Else
					ShowNotification("Fecha de inicio no válida", ShowNotifications.ErrorNotification)
					ActivateAccordion(1, EffectActivateAccordion.NoEffect)
					Exit Sub
					Throw New Exception("Fecha de inicio No Válida")
				End If
			Else
				ShowNotification("Fecha de inicio requerida", ShowNotifications.ErrorNotification)
				ActivateAccordion(1, EffectActivateAccordion.NoEffect)
				Exit Sub
				Throw New Exception("Fecha de inicio requerida")
			End If
			If Not String.IsNullOrEmpty(txtFechaTerminacion.Text) Then
				'Validar formato de la fecha
				If ValidarFecha(txtFechaTerminacion.Text) Then
					If Date.Parse(txtFechaTerminacion.Text) > Date.Parse(txtFechaInicio.Text) Then
						oEstudio.FechaTerminacion = Date.Parse(txtFechaTerminacion.Text)
					Else
						ShowNotification("La fecha de terminación debe ser mayor a la fecha de inicio", ShowNotifications.ErrorNotification)
						ActivateAccordion(1, EffectActivateAccordion.NoEffect)
						Exit Sub
						Throw New Exception("La fecha de terminación debe ser mayor a la fecha de inicio")
					End If
				Else
					ShowNotification("Fecha de terminación No Válida", ShowNotifications.ErrorNotification)
					ActivateAccordion(1, EffectActivateAccordion.NoEffect)
					Exit Sub
					Throw New Exception("Fecha de terminación No Válida")
				End If
			Else
				ShowNotification("Fecha de terminación requerida", ShowNotifications.ErrorNotification)
				ActivateAccordion(1, EffectActivateAccordion.NoEffect)
				Exit Sub
				Throw New Exception("Fecha de terminación requerida")
			End If
			oEstudio.Anticipo = txtAnticipo.Text
			oEstudio.Saldo = txtSaldo.Text
			oEstudio.Plazo = txtPlazo.Text
			oEstudio.FechaInicioCampo = txtFechaInicioCampo.Text
			oEstudio.Estado = 1
            oEstudio.GerenteCuentas = idUsuario
            oEstudio.DocumentoSoporte = ddlDocumentoSoporte.SelectedValue
            oEstudio.TiempoRetencionAnnos = txtTiempoRetencion.Text.Trim
            If String.IsNullOrEmpty(idEstudio) Then
				listCU_Estudios_Presupuestos.Add(New CU_Estudios_Presupuestos With {.PresupuestoId = idpresup})
				hfIdEstudio.Value = oEstudios_Presupuestos.Grabar(oEstudio, listCU_Estudios_Presupuestos)
				EnviarEmailAnuncio()
				Response.Redirect("Proyectos.aspx?IdEstudio=" & hfIdEstudio.Value)
			Else
				oEstudio = oBLEstudio.ObtenerXID(idEstudio)
				If txtJobBook.Text = "" Then
					oEstudio.JobBook = txtJobBookInt.Text
				Else
					oEstudio.JobBook = txtJobBook.Text
				End If
				oEstudio.Nombre = txtNombreEstudio.Text
				oEstudio.PropuestaId = txtPropuesta.Text
				oEstudio.Valor = txtValor.Text
				oEstudio.Observaciones = txtObservaciones.Text
				oEstudio.FechaInicio = txtFechaInicio.Text
				oEstudio.FechaTerminacion = txtFechaTerminacion.Text
				oEstudio.Anticipo = txtAnticipo.Text
				oEstudio.Saldo = txtSaldo.Text
				oEstudio.Plazo = txtPlazo.Text
				oEstudio.GerenteCuentas = idUsuario
                oEstudio.FechaInicioCampo = txtFechaInicioCampo.Text
                oEstudio.DocumentoSoporte = ddlDocumentoSoporte.SelectedValue
                oEstudio.TiempoRetencionAnnos = txtTiempoRetencion.Text.Trim
                oEstudio.Estado = 1
				listCU_Estudios_Presupuestos_Nuevos = obtenerPresupuestosNuevos(vecIndPresupuestosAsignados, vecIndPresupuestosOriginales)

				listCU_Estudios_Presupuestos_Eliminados = obtenerPresupuestosEliminados(vecIndPresupuestosAsignados, vecIndPresupuestosOriginales)
				GuardarNoNuevo()
				'oEstudios_Presupuestos.Grabar(oEstudio, listCU_Estudios_Presupuestos_Eliminados, listCU_Estudios_Presupuestos_Nuevos)

				actualizarEstadoPropuesta(txtPropuesta.Text)

			End If
			CargarEstudios()
			log(hfIdEstudio.Value, 2)
			'Limpiar()
			ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
			If gvPresupuestos.Enabled = True Then
				Response.Redirect("Estudios.aspx")
			End If
			ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
		Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
			ActivateAccordion(1, EffectActivateAccordion.NoEffect)
		End Try

	End Sub
	Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
		Limpiar()
		accordion0.Visible = True
		accordion1.Visible = False
	End Sub
	Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
		Buscar()
	End Sub
	Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
		ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
		txtNombreEstudio.Focus()
		Limpiar()
	End Sub
	Protected Sub gvEstudios_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvEstudios.PageIndexChanging
		gvEstudios.PageIndex = e.NewPageIndex
		CargarEstudios()
		ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
	End Sub
	Protected Sub btnCrearEstudio_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCrearEstudio.Click
		Try
			Dim daDocumentos As New RepositorioDocumentos

			If daDocumentos.obtenerDocumentosXIdContenedorXIdDocumento(hfIdEstudio.Value, 50).Count = 0 Then
				ShowNotification("Debe subir el correo de aprobación", ShowNotifications.ErrorNotification)
				Exit Sub
			End If

			If String.IsNullOrEmpty(hfIdEstudio.Value) Then
				ShowNotification("Debe Elegir un estudio para crear el proyecto", ShowNotifications.ErrorNotification)
				ActivateAccordion(0, EffectActivateAccordion.NoEffect)
				Exit Sub
			End If
			Response.Redirect("Proyectos.aspx?IdEstudio=" & hfIdEstudio.Value)

		Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)

		End Try
	End Sub
	Protected Sub btnDocumentos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDocumentos.Click
		If hfIdEstudio.Value <> "" Then
			Response.Redirect("\GD_Documentos\GD_Documentos.aspx?IdContenedor=" & hfIdEstudio.Value & "&IdDocumento=50")
		Else : ShowNotification("Guarde Primero el Estudio", ShowNotifications.ErrorNotification)
		End If
		ActivateAccordion(0, EffectActivateAccordion.NoEffect)
	End Sub
#End Region
#Region "Metodos"
	Function GuardarNoNuevo() As Boolean
		Dim oEstudio As New CU_Estudios
		Dim oBLEstudio As New Estudio


		Dim idEstudio As String = hfIdEstudio.Value
		Try
			If txtJobBook.Text = "" Then
				oEstudio.JobBook = txtJobBookInt.Text
			Else
				oEstudio.JobBook = txtJobBook.Text
			End If
			If oEstudio.JobBook = "" Or oEstudio.JobBook.EndsWith("00") Then
				ShowNotification("Debe escribir un número de JobBook válido antes de continuar", ShowNotifications.ErrorNotification)
				ActivateAccordion(1, EffectActivateAccordion.NoEffect)
				Return False
			End If
			Dim oPropuesta As New Propuesta
			Dim jbPropuesta As String = oPropuesta.DevolverxID(txtPropuesta.Text).JobBook
            If Not (oEstudio.JobBook.StartsWith(jbPropuesta)) Then
                ShowNotification("El JobBook debe mantener la misma estructura desde la propuesta", ShowNotifications.ErrorNotification)
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Return False
            End If
            If ddlDocumentoSoporte.SelectedValue = "-1" Then
                ddlDocumentoSoporte.Focus()
                ShowNotification("Seleccione el documento soporte", ShowNotifications.ErrorNotification)
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Return False
                Throw New Exception("Seleccione el documento soporte")
            End If
            If Not (IsNumeric(txtTiempoRetencion.Text)) Then
                txtTiempoRetencion.Focus()
                ShowNotification("El tiempo de retención debe ser numérico", ShowNotifications.ErrorNotification)
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Return False
                Throw New Exception("El tiempo de retención debe ser numérico")
            End If
            oEstudio.id = hfIdEstudio.Value
			oEstudio.Nombre = txtNombreEstudio.Text
			oEstudio.PropuestaId = txtPropuesta.Text
			oEstudio.Valor = txtValor.Text
			oEstudio.Observaciones = txtObservaciones.Text

			oEstudio.Anticipo = txtAnticipo.Text
			oEstudio.Saldo = txtSaldo.Text
			oEstudio.Plazo = txtPlazo.Text
			oEstudio.FechaInicioCampo = txtFechaInicioCampo.Text

			oEstudio.GerenteCuentas = idUsuario
			oEstudio.FechaInicio = txtFechaInicio.Text
            oEstudio.FechaTerminacion = txtFechaTerminacion.Text
            oEstudio.DocumentoSoporte = ddlDocumentoSoporte.SelectedValue
            oEstudio.TiempoRetencionAnnos = txtTiempoRetencion.Text.Trim
            oBLEstudio.GuardarEstudio(oEstudio)
			Return True
		Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
			ActivateAccordion(1, EffectActivateAccordion.NoEffect)
			Return False
		End Try
	End Function
	Sub CargarPresupuestos(ByVal propuestaId As Int64)
		Dim oPresupuesto As New Presupuesto

		If hfPropuestaOriginal.Value <> "" AndAlso propuestaId = hfPropuestaOriginal.Value Then
			hfIndicesFilasPresupuestosAsignados.Value = hfIndicesFilasPresupuestosOriginales.Value
		Else
			'TODO Deuda tecnica, aqui se debe corregir cuando se cambia el estudio, pero tiene algunos presupuestos asignados del otro
			hfIndicesFilasPresupuestosAsignados.Value = ""
		End If

		gvPresupuestos.DataSource = oPresupuesto.DevolverxIdPropuestaAprobados(propuestaId, If(hfIdEstudio.Value <> "", hfIdEstudio.Value, Nothing))
		gvPresupuestos.DataBind()

		Dim oPropuesta As New Propuesta
		If Not (oPropuesta.DevolverxID(propuestaId).JobBook = "") Then
			For Each row As GridViewRow In Me.gvPresupuestos.Rows
				If row.RowType = DataControlRowType.DataRow Then
					DirectCast(row.FindControl("txtJobBook"), TextBox).Text = "" = oPropuesta.DevolverxID(propuestaId).JobBook
				End If
			Next
		End If

		upPresupuestos.Update()
		upDetalleEstudios.Update()
	End Sub
	Sub CargarEstudios()
		Dim oEstudio As New Estudio
		gvEstudios.DataSource = oEstudio.ObtenerXIdGerenteCuentas(idUsuario)
		gvEstudios.DataBind()
		upEstudios.Update()
	End Sub
	Sub CargarPresupuestosXEstudio(ByVal estudioID As Int64)
		Dim oPresupuesto As New Presupuesto
		Dim x = oPresupuesto.ObtenerPresupuestosAsignadosXEstudio(estudioID)

		gvPresupuestosAsignadosXEstudio.DataSource = x
		gvPresupuestosAsignadosXEstudio.DataBind()
		upPresupuestosAsignadosXEstudio.Update()
	End Sub
	Sub CargarEstudio(ByVal estudioID As Int64)
		Dim oEstudio As New Estudio
		Dim CU_Estudio As CU_Estudios
		Dim oPresupuesto As New Presupuesto
		Dim x = oPresupuesto.ObtenerPresupuestosAsignadosXEstudio(estudioID)

		CU_Estudio = oEstudio.ObtenerXID(estudioID)

		Dim JobBook As String = CU_Estudio.JobBook
		If JobBook.Length = 12 Then
			Me.txtJobBook.Text = JobBook
			Me.txtJobBook.Visible = True
			Me.txtJobBookInt.Text = ""
			Me.txtJobBookInt.Visible = False
		Else
			Me.txtJobBookInt.Text = JobBook
			Me.txtJobBookInt.Visible = True
			Me.txtJobBook.Text = ""
			Me.txtJobBook.Visible = False
		End If
		txtNombreEstudio.Text = CU_Estudio.Nombre
		txtValor.Text = CU_Estudio.Valor
		txtPropuesta.Text = CU_Estudio.PropuestaId
		txtObservaciones.Text = CU_Estudio.Observaciones
		txtFechaInicio.Text = CU_Estudio.FechaInicio
		txtFechaTerminacion.Text = CU_Estudio.FechaTerminacion
		txtAnticipo.Text = CU_Estudio.Anticipo
		txtSaldo.Text = CU_Estudio.Saldo
		txtPlazo.Text = CU_Estudio.Plazo
        txtFechaInicioCampo.Text = CU_Estudio.FechaInicioCampo
        If CU_Estudio.TiempoRetencionAnnos IsNot Nothing Then
            If IsNumeric(CU_Estudio.TiempoRetencionAnnos) Then
                txtTiempoRetencion.Text = CU_Estudio.TiempoRetencionAnnos
            End If
        End If
        If CU_Estudio.DocumentoSoporte IsNot Nothing Then
            Try
                ddlDocumentoSoporte.SelectedValue = CU_Estudio.DocumentoSoporte
            Catch ex As Exception

            End Try
        End If

        hfPropuestaOriginal.Value = CU_Estudio.PropuestaId

		hfIndicesFilasPresupuestosOriginales.Value = String.Join(";;", x.Select(Function(y) y.Id).ToArray)
		hfIndicesFilasPresupuestosAsignados.Value = hfIndicesFilasPresupuestosOriginales.Value

		hfIndicesFilasPresupuestosOriginales.Value = ";" & hfIndicesFilasPresupuestosOriginales.Value & ";"
		hfIndicesFilasPresupuestosAsignados.Value = hfIndicesFilasPresupuestosOriginales.Value

		upDetalleEstudios.Update()

		CargarPresupuestos(CU_Estudio.PropuestaId)

	End Sub
	Function obtenerPresupuestosNuevos(ByVal presupuestosAsignados() As String, ByVal presupuestosOriginales() As String) As List(Of CU_Estudios_Presupuestos)
		Dim listCU_Estudios_Presupuestos_Nuevos As New List(Of CU_Estudios_Presupuestos)
		Dim PresupuestosNuevos = (From x In presupuestosAsignados Group Join y In presupuestosOriginales On x Equals y Into Coincidencia = Group Where Coincidencia.Count = 0).ToArray

		For Each i In PresupuestosNuevos
			listCU_Estudios_Presupuestos_Nuevos.Add(New CU_Estudios_Presupuestos With {.PresupuestoId = i.x})
		Next

		Return listCU_Estudios_Presupuestos_Nuevos

	End Function
	Function obtenerPresupuestosEliminados(ByVal presupuestosAsignados() As String, ByVal presupuestosOriginales() As String) As List(Of CU_Estudios_Presupuestos)
		Dim listCU_Estudios_Presupuestos_Eliminados As New List(Of CU_Estudios_Presupuestos)
		Dim PresupuestosEliminados = (From x In presupuestosOriginales Group Join y In presupuestosAsignados On x Equals y Into Coincidencia = Group Where Coincidencia.Count = 0).ToArray

		For Each i In PresupuestosEliminados
			listCU_Estudios_Presupuestos_Eliminados.Add(New CU_Estudios_Presupuestos With {.PresupuestoId = i.x})
		Next

		Return listCU_Estudios_Presupuestos_Eliminados

	End Function
	Sub Limpiar()
		hfIdEstudio.Value = ""
		hfIndicesFilasPresupuestosAsignados.Value = ""
		hfIndicesFilasPresupuestosOriginales.Value = ""
		hfPropuestaOriginal.Value = ""
		txtJobBook.Text = ""
		txtJobBookInt.Text = ""
		txtNombreEstudio.Text = ""
		txtValor.Text = ""
		txtObservaciones.Text = ""
		txtFechaInicio.Text = ""
		txtFechaTerminacion.Text = ""
        txtAnticipo.Text = "70"
        txtSaldo.Text = "30"
        txtPlazo.Text = "30"
        txtPropuesta.Text = ""
        txtTiempoRetencion.Text = "1"
        gvPresupuestos.DataSource = Nothing
		gvPresupuestos.DataBind()
		upDetalleEstudios.Update()
		upPresupuestos.Update()
	End Sub
	Sub Buscar()
		Dim oEstudio As New Estudio
		gvEstudios.DataSource = oEstudio.obtenerTodosCampos(idUsuario, Me.txtBuscar.Text)
		gvEstudios.DataBind()
		upEstudios.Update()
	End Sub
	Sub actualizarEstadoPropuesta(ByVal idPropuesta As Int64)
		Dim oPropuesta As New Propuesta
		Dim oePropuesta As CU_Propuestas_Get_Result
		oePropuesta = oPropuesta.DevolverxID(idPropuesta)

		oPropuesta.Guardar(idPropuesta, oePropuesta.Titulo, oePropuesta.TipoId, oePropuesta.ProbabilidadId, oePropuesta.FechaEnvio, 3, oePropuesta.OrigenId, oePropuesta.FechaAprob, oePropuesta.RazonNoAprobId, oePropuesta.FormaEnvio, oePropuesta.Brief, oePropuesta.Tracking, oePropuesta.JobBook, oePropuesta.internacional, txtAnticipo.Text, txtSaldo.Text, txtPlazo.Text, oePropuesta.FechaInicioCampo, oePropuesta.RequestHabeasData)

	End Sub
	Public Sub Validar()
		Try

			If Session("IDUsuario") IsNot Nothing Then
				Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
				Dim oValidacion As New Validacion
				If Not oValidacion.ValidarPermiso("Propuesta", UsuarioID) Then
					btnGrabar.Enabled = False
					gvEstudios.Columns.Item(8).Visible = False
					btnCrearEstudio.Enabled = False
				Else
					btnGrabar.Enabled = True
					gvEstudios.Columns.Item(8).Visible = True
					btnCrearEstudio.Enabled = True
				End If
			End If

		Catch ex As Exception
			Throw ex
		End Try
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

	Sub EnviarEmailAnuncio()
		Dim oEnviarCorreo As New EnviarCorreo
		Try
			If String.IsNullOrEmpty(hfIdEstudio.Value) Then
				Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar un anuncio de aprobación")
			End If
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/AnuncioAprobacionConValor.aspx?idEstudio=" & hfIdEstudio.Value)
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/AnuncioAprobacionSinValor.aspx?idEstudio=" & hfIdEstudio.Value)
			'Dim script As String = "window.open('../Emails/AnuncioAprobacion.aspx?idEstudio=" & hfIdEstudio.Value & "','cal','width=400,height=250,left=270,top=180')"
			'Dim page As Page = DirectCast(Context.Handler, Page)
			'ScriptManager.RegisterStartupScript(page, GetType(Page), "Anuncio", script, True)
			ActivateAccordion(1, EffectActivateAccordion.NoEffect)
		Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
			ActivateAccordion(0, EffectActivateAccordion.NoEffect)
		End Try
	End Sub

	Sub EnviarEmailAnuncioCorreccion()
		Dim oEnviarCorreo As New EnviarCorreo
		Try
			If String.IsNullOrEmpty(hfIdEstudio.Value) Then
				Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar un anuncio de aprobación")
			End If
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/AnuncioAprobacionConValorCorreccion.aspx?idEstudio=" & hfIdEstudio.Value)
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/AnuncioAprobacionSinValorCorreccion.aspx?idEstudio=" & hfIdEstudio.Value)
			'Dim script As String = "window.open('../Emails/AnuncioAprobacion.aspx?idEstudio=" & hfIdEstudio.Value & "','cal','width=400,height=250,left=270,top=180')"
			'Dim page As Page = DirectCast(Context.Handler, Page)
			'ScriptManager.RegisterStartupScript(page, GetType(Page), "Anuncio", script, True)
			ActivateAccordion(1, EffectActivateAccordion.NoEffect)
		Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
			ActivateAccordion(0, EffectActivateAccordion.NoEffect)
		End Try
	End Sub

	Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
		Dim log As New LogEjecucion
		log.Guardar(14, iddoc, Now(), Session("IDUsuario"), idaccion)
	End Sub
#End Region

	Private Sub gvPresupuestosAsignadosXEstudio_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPresupuestosAsignadosXEstudio.RowCommand
		Dim oePresupuesto As New CU_Presupuesto_Get_Result
		Dim oPresupuesto As New Presupuesto
		oePresupuesto = oPresupuesto.obtenerXId(gvPresupuestosAsignadosXEstudio.DataKeys(e.CommandArgument)("Id"))
		If e.CommandName = "ConsultarPresupuesto" Then
			Response.Redirect("../CAP/Cap_Principal.aspx?IdPropuesta=" & oePresupuesto.PropuestaId & "&Alternativa=" & oePresupuesto.Alternativa & "&Accion=3")
		End If
	End Sub



	Protected Sub btnCorreccionEstudio_Click(sender As Object, e As EventArgs) Handles btnCorreccionEstudio.Click
		Me.pnlEstudio.Visible = False
		Me.pnlCambiosPresupuestos.Visible = True
		Dim o As New Reportes.CambiosAlternativas
		ddlAlternativas.DataSource = o.ObtenerAlternativasNoAsociadas(hfIdEstudio.Value)
		ddlAlternativas.DataTextField = "NomAlternativa"
		ddlAlternativas.DataValueField = "Alternativa"
		ddlAlternativas.DataBind()
		ActivateAccordion(1, EffectActivateAccordion.NoEffect)
	End Sub

	Protected Sub btnCancelarCambio_Click(sender As Object, e As EventArgs) Handles btnCancelarCambio.Click
		Me.pnlEstudio.Visible = True
		Me.pnlCambiosPresupuestos.Visible = False
		ActivateAccordion(1, EffectActivateAccordion.NoEffect)
	End Sub

	Protected Sub btnActualizarCambio_Click(sender As Object, e As EventArgs) Handles btnActualizarCambio.Click
		Dim o As New Reportes.CambiosAlternativas
		If GuardarNoNuevo() = False Then
			ShowNotification("Surgió un error al actualizar", ShowNotifications.InfoNotification)
			Exit Sub
		End If
		o.CambioAlternativaEstudio(hfIdEstudio.Value, ddlAlternativas.SelectedValue)
		EnviarEmailAnuncioCorreccion()
		ShowNotification("Se han confirmado y comunicado los cambios", ShowNotifications.InfoNotification)
		ActivateAccordion(0, EffectActivateAccordion.NoEffect)
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
		ActivateAccordion(1, EffectActivateAccordion.NoEffect)
	End Sub

    Sub CargarDocumentosSoporte()
        Dim oEstudios As New Estudio
        ddlDocumentoSoporte.DataSource = oEstudios.ObtenerDocumentosSoporte
        ddlDocumentoSoporte.DataValueField = "id"
        ddlDocumentoSoporte.DataTextField = "DocumentoSoporte"
        ddlDocumentoSoporte.DataBind()
        ddlDocumentoSoporte.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub
End Class