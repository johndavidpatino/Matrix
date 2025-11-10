Imports CoreProject
Imports WebMatrix.Util
Imports ClosedXML.Excel

Public Class TraficoEncuestas
	Inherits System.Web.UI.Page

#Region "Propiedades"
	Dim eTrabajoOP As New OP_TrabajoConfiguracion
	Private _IDUsuario As Int64
	Public Property IDUsuario() As Int64
		Get
			Return _IDUsuario
		End Get
		Set(ByVal value As Int64)
			_IDUsuario = value
		End Set
	End Property
	Private _IDCargoEjecuta As Int64
	Public Property IDCargoEjecuta() As Int64
		Get
			Return _IDCargoEjecuta
		End Get
		Set(ByVal value As Int64)
			_IDCargoEjecuta = value
		End Set
	End Property
#End Region

#Region "Funciones y Métodos"
	Sub CargarTrabajos()
		Dim oTrabajo As New Trabajo
		If Me.txtBuscar.Text = "" Then
			gvTrabajos.DataSource = oTrabajo.obtenerAllTrabajos()
		Else
			gvTrabajos.DataSource = oTrabajo.obtenerPorBusqueda(txtBuscar.Text.Trim)
		End If
		gvTrabajos.DataBind()
	End Sub
	Function obtenerOPMetodologia(ByVal id As Int16) As OP_Metodologias_Get_Result
		Dim oMetodologiaOperaciones As New MetodologiaOperaciones
		Return oMetodologiaOperaciones.obtenerXId(id)
	End Function
	Function obtenerProyectoXId(ByVal id As Int64) As PY_Proyectos_Get_Result
		Dim oProyecto As New Proyecto
		Return oProyecto.obtenerXId(id)
	End Function
	Public Function CargarFichaCuantitativa() As Long
		Try
			Dim idtrabajo As Int64 = Int64.Parse(hfIdTrabajo.Value)
			Dim oTrabajo As New Trabajo
			Dim info = oTrabajo.DevolverxID(idtrabajo)

			Dim oFichaCuantitativa As New FichaCuantitativo
			Return oFichaCuantitativa.DevolverxTrabajoID(hfIdTrabajo.Value).Item(0).id

		Catch ex As Exception
			Throw ex
		End Try
	End Function
	Sub CargarMuestraEnviadaRMC()
		Dim O As New OP.TraficoEncuestas
		Me.gvMuestra.DataSource = O.ObtenerMuestraEnviadaCiudadRMC(hfIdTrabajo.Value)
		Me.gvMuestra.DataBind()
	End Sub
	Sub ConfigurarUnidades()
		ddlDestinoPopupEnvio.Items.Clear()
		ddlDevoluciones.Items.Clear()
		ddlEnvios.Items.Clear()
		ddlUnidadDevolucionPopup.Items.Clear()
		ddlUnidadDevolucionPopup.Items.Clear()
		Select Case hfIdUnidad.Value
			Case 38
				Me.btnPersonalAsignado.Visible = True
				Me.pnlDevoluciones.Visible = False
				Me.pnlRecepcion.Visible = False
				Me.btnDevolver.Visible = False
				Me.pnlMuestra.Visible = True
				CargarMuestraEnviadaRMC()
				Dim oTrabajo As New TrabajoOPCuanti
				Dim oUnidades As New US.Unidades
				Dim li As New ListItem
				li.Value = 28 'oTrabajo.ObtenerUnidadCritica(hfIdTrabajo.Value)
				Dim prueba As String = oUnidades.ObtenerNombreUnidad(li.Value)
				li.Text = oUnidades.ObtenerNombreUnidad(li.Value)
				'li.Selected = True
				ddlDevoluciones.Items.Add(li)
				Dim li2 As New ListItem
				li2.Value = li.Value
				li2.Text = li.Text
				'li.Selected = True
				ddlEnvios.Items.Add(li2)
				ddlDestinoPopupEnvio.Items.Add(li2)
				'--ddlEnvios.Enabled = False
				'--ddlEnvios.Enabled = False
				Dim li3 As New ListItem
				li3.Value = 38
				li3.Text = "RMC"
				ddlUnidadDevolucionPopup.Items.Add(li3)
				btnAnulacion.Visible = False
			Case 20
				Me.pnlDevoluciones.Visible = True
				Dim oTrabajo As New TrabajoOPCuanti
				Dim oUnidades As New US.Unidades
				Dim li As New ListItem
				li.Value = 28 'oTrabajo.ObtenerUnidadCritica(hfIdTrabajo.Value)
				li.Text = oUnidades.ObtenerNombreUnidad(li.Value)
				'li.Selected = True
				ddlDevoluciones.Items.Add(li)
				'ddlUnidadDevolucionPopup.Items.Add(li)
				Dim li4 As New ListItem
				li4.Value = 21
				li4.Text = "Captura"
				ddlEnvios.Items.Add(li4)
				ddlDestinoPopupEnvio.Items.Add(li4)
				ddlEnvios.Enabled = True
				btnAnulacion.Visible = True
			Case 21
				Me.pnlDevoluciones.Visible = True
				Me.btnDevolver.Visible = True
				Me.btnEnviar.Visible = False
				Dim oTrabajo As New TrabajoOPCuanti
				Dim oUnidades As New US.Unidades
				Dim li As New ListItem
				li.Value = 28 'oTrabajo.ObtenerUnidadCritica(hfIdTrabajo.Value)
				li.Text = oUnidades.ObtenerNombreUnidad(li.Value)
				'li.Selected = True
				ddlDevoluciones.Items.Add(li)
				ddlEnvios.Items.Add(li)
				ddlUnidadDevolucionPopup.Items.Add(li)
				Dim li2 As New ListItem
				li2.Value = 20
				li2.Text = "Verificación"
				ddlEnvios.Items.Add(li2)
				ddlDevoluciones.Items.Add(li2)
				ddlDestinoPopupEnvio.Items.Add(li2)
				ddlUnidadDevolucionPopup.Items.Add(li2)
				btnAnulacion.Visible = False
			Case 28
				Dim li As New ListItem
				li.Value = 38
				li.Text = "RMC"
				ddlDevoluciones.Items.Add(li)
				Dim li3 As New ListItem
				li3.Value = 20
				li3.Text = "Verificación"
				ddlEnvios.Items.Add(li3)
				ddlDestinoPopupEnvio.Items.Add(li3)
				Dim li4 As New ListItem
				li4.Value = 21
				li4.Text = "Captura"
				ddlEnvios.Items.Add(li4)
				ddlDestinoPopupEnvio.Items.Add(li4)
				ddlEnvios.Enabled = True
				btnAnulacion.Visible = True
			Case 30
				Dim li As New ListItem
				li.Value = 38
				li.Text = "RMC"
				ddlUnidadDevolucionPopup.Items.Add(li)
				ddlDevoluciones.Items.Add(li)
				Dim li3 As New ListItem
				li3.Value = 20
				li3.Text = "Verificación"
				ddlDestinoPopupEnvio.Items.Add(li3)
				ddlEnvios.Items.Add(li3)
				Dim li4 As New ListItem
				li4.Value = 21
				li4.Text = "Captura"
				ddlEnvios.Items.Add(li4)
				ddlDestinoPopupEnvio.Items.Add(li4)
				ddlEnvios.Enabled = True
				btnAnulacion.Visible = True
		End Select
		ddlDevoluciones.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
		ddlEnvios.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
	End Sub
	Sub CargarMuestraEnviada(ByVal TrabajoId As Int64)
		If Me.pnlMuestra.Visible = True Then
			Dim oTraficoEncuestas As New OP.TraficoEncuestas
			Dim oCoordinacion As New CoordinacionCampoPersonal

			Dim lEnvios = (From lmuestra In oTraficoEncuestas.ObtenerEnviosXUnidadEnviaYTrabajo(hfIdTrabajo.Value, hfIdUnidad.Value)
						   Select lmuestra).Sum(Function(x) x.Cantidad)
		End If
	End Sub
	Sub CargarRecepcionEncuestas(ByVal TrabajoId As Int64, ByVal UnidadId As Int16)
		Dim oTrafico As New OP.TraficoEncuestas
		If UnidadId = 28 Or UnidadId = 30 Then
			Me.gvRecepcion.DataSource = (From lenvio In oTrafico.ObtenerEnviosXUnidadRecibeYTrabajo(TrabajoId, UnidadId)
										 Where IsNothing(lenvio.UsuarioRecibe)
										 Select FechaEnvio = lenvio.FechaEnvio, Usuario = lenvio.UsuarioEnvia, Observaciones = lenvio.ObservacionesEnvio,
									 Ciudad = lenvio.Ciudad, Enviadas = lenvio.Cantidad, Devolucion = lenvio.Devolucion, id = lenvio.id).ToList
		Else
			Me.gvRecepcion.DataSource = (From lenvio In oTrafico.ObtenerEnviosXUnidadRecibeYTrabajo(TrabajoId, UnidadId)
										 Where IsNothing(lenvio.UsuarioRecibe)
										 Select FechaEnvio = lenvio.FechaEnvio, Usuario = lenvio.UsuarioEnvia, Observaciones = lenvio.ObservacionesEnvio,
									 Ciudad = "N/A", Enviadas = lenvio.Cantidad, Devolucion = lenvio.Devolucion, id = lenvio.id).ToList
		End If
		Me.gvRecepcion.DataBind()
	End Sub
	Sub CargarEnvios(ByVal TrabajoId As Int64, ByVal UnidadEnvia As Int32, ByVal UnidadDestino As Int32)
		Dim oTrafico As New OP.TraficoEncuestas
		Try
			Me.gvEnvios.DataSource = oTrafico.ObtenerEnviosListadoGet(TrabajoId, UnidadEnvia, UnidadDestino)
		Catch ex As Exception
			Throw New Exception
		End Try
		Dim oCantidad As New Reportes.AvanceCampo
		Try
			Me.lblEnvios.Text = "Cantidad enviada y recibida: " & oCantidad.CantidadTraficoEncuentas(TrabajoId, UnidadEnvia, UnidadDestino, True) & ". Cantidad sin recibir: " & oCantidad.CantidadTraficoEncuentas(TrabajoId, UnidadEnvia, UnidadDestino, False)
		Catch ex As Exception

		End Try
		Me.gvEnvios.DataBind()
	End Sub
	Sub CargarRecibidos(ByVal TrabajoId As Int64, ByVal UnidadEnvia As Int32, ByVal UnidadDestino As Int32)
		Dim oTrafico As New OP.TraficoEncuestas
		Try
			Me.gvRecibidas.DataSource = oTrafico.ObtenerEnviosListadoGet(TrabajoId, UnidadEnvia, UnidadDestino)
		Catch ex As Exception

		End Try
		Dim oCantidad As New Reportes.AvanceCampo
		Try
			Me.lblRecibidas.Text = "Cantidad enviada y recibida: " & oCantidad.CantidadTraficoEncuentas(TrabajoId, UnidadEnvia, UnidadDestino, True) & ". Cantidad sin recibir: " & oCantidad.CantidadTraficoEncuentas(TrabajoId, UnidadEnvia, UnidadDestino, False)
		Catch ex As Exception

		End Try
		Me.gvRecibidas.DataBind()
	End Sub
#End Region

#Region "Eventos del Control"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Dim permisos As New Datos.ClsPermisosUsuarios
		Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())

		If Not IsPostBack Then
			If Request.QueryString("UnidadId") IsNot Nothing Then
				hfIdUnidad.Value = Int64.Parse(Request.QueryString("UnidadId").ToString)
				Select Case hfIdUnidad.Value
					Case 20
						If permisos.VerificarPermisoUsuario(117, UsuarioID) = False Then
							Response.Redirect("../OP_Cuantitativo/HomeGestion.aspx")
						End If
						Me.lblComentForm.Text = "Verificación"
					Case 21
						If permisos.VerificarPermisoUsuario(118, UsuarioID) = False Then
							Response.Redirect("../OP_Cuantitativo/HomeGestion.aspx")
						End If
						Me.lblComentForm.Text = "Captura"
					Case 28
						If permisos.VerificarPermisoUsuario(119, UsuarioID) = False Then
							Response.Redirect("../OP_Cuantitativo/HomeGestion.aspx")
						End If
						Me.lblComentForm.Text = "Crítica"
					Case 38
						If permisos.VerificarPermisoUsuario(120, UsuarioID) = False Then
							Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
						End If
						Me.lblComentForm.Text = "RMC"
				End Select
				'ConfigurarUnidades()
				CargarTrabajos()
				If Not Session("TrabajoId") = Nothing Then
					hfIdTrabajo.Value = Session("TrabajoId").ToString
					CargaTrabajo()
				End If
			Else
				Response.Redirect("../Home.aspx")
			End If
		End If
		Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
		smanager.RegisterPostBackControl(Me.btnPersonalAsignado)
	End Sub
	Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
		CargarTrabajos()
		ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
	End Sub
	Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
		gvTrabajos.PageIndex = e.NewPageIndex
		CargarTrabajos()
		ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
	End Sub
	Sub CargaTrabajo()
		ConfigurarUnidades()
		CargarRecepcionEncuestas(hfIdTrabajo.Value, hfIdUnidad.Value)
		Select Case hfIdUnidad.Value
			Case 20
				hfIdCargoEjecuta.Value = Cargos.TiposCargos.Verificador
			Case 21
				hfIdCargoEjecuta.Value = Cargos.TiposCargos.Digitador
			Case 22
				hfIdCargoEjecuta.Value = Cargos.TiposCargos.Codificador
			Case 38
				'CargarEnvios(hfIdTrabajo.Value, hfIdUnidad.Value, ddlEnvios.SelectedValue)
				hfIdCargoEjecuta.Value = Cargos.TiposCargos.DigitadorRMC
				Me.pnlDevoluciones.Visible = False
			Case 28
				' CargarEnvios(hfIdTrabajo.Value, hfIdUnidad.Value, ddlEnvios.SelectedValue)
				'CargarRecibidos(hfIdTrabajo.Value, ddlDevoluciones.SelectedValue, hfIdUnidad.Value)
				hfIdCargoEjecuta.Value = Cargos.TiposCargos.Critico
				Me.pnlDevoluciones.Visible = True
			Case 30
				' CargarEnvios(hfIdTrabajo.Value, hfIdUnidad.Value, ddlEnvios.SelectedValue)
				'  CargarRecibidos(hfIdTrabajo.Value, ddlDevoluciones.SelectedValue, hfIdUnidad.Value)
				Me.pnlDevoluciones.Visible = True
				hfIdCargoEjecuta.Value = Cargos.TiposCargos.Critico
		End Select
		CargarPersonalAsignado(hfIdTrabajo.Value, hfIdCargoEjecuta.Value)
		CargarPersonalDisponible(hfIdTrabajo.Value, hfIdCargoEjecuta.Value)
		ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
	End Sub
	Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
		If e.CommandName = "Gestionar" Then
			hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
			Session("TrabajoId") = hfIdTrabajo.Value
			Dim oTrabajo As New Trabajo
			Dim oeTrabajo = oTrabajo.ObtenerTrabajo(hfIdTrabajo.Value)
			Session("NombreTrabajo") = oeTrabajo.id.ToString & " | " & oeTrabajo.JobBook & " | " & oeTrabajo.NombreTrabajo
			Try
				Me.txtNombreTrabajo.Text = Session("NombreTrabajo").ToString
			Catch ex As Exception
			End Try
			CargaTrabajo()
		ElseIf e.CommandName = "Detalles" Then
			hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
			ConfigurarUnidades()
			Select Case hfIdUnidad.Value
				Case 38
					'  CargarEnvios(hfIdTrabajo.Value, hfIdUnidad.Value, ddlEnvios.SelectedValue)
					Me.pnlDevoluciones.Visible = False
				Case 28
					'  CargarEnvios(hfIdTrabajo.Value, hfIdUnidad.Value, ddlEnvios.SelectedValue)
					'  CargarRecibidos(hfIdTrabajo.Value, ddlDevoluciones.SelectedValue, hfIdUnidad.Value)
					Me.pnlDevoluciones.Visible = True
				Case 30
					'  CargarEnvios(hfIdTrabajo.Value, hfIdUnidad.Value, ddlEnvios.SelectedValue)
					'  CargarRecibidos(hfIdTrabajo.Value, ddlDevoluciones.SelectedValue, hfIdUnidad.Value)
					Me.pnlDevoluciones.Visible = True
			End Select
			ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
		ElseIf e.CommandName = "Avance" Then
			hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
			Response.Redirect("../RP_Reportes/AvanceDeCampo.aspx?TrabajoId=" & hfIdTrabajo.Value)
		End If
	End Sub
	Protected Sub btnEnviar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEnviar.Click
		If hfIdUnidad.Value = 38 Then
			pnlCiudadEnvioPopup.Visible = True
			Dim oCiudades As New OP.TraficoEncuestas
			ddlCiudadEnvio.DataSource = oCiudades.ObtenerEncuestasDisponibles(hfIdTrabajo.Value, hfIdUnidad.Value)
			ddlCiudadEnvio.DataValueField = "C_MPIO"
			ddlCiudadEnvio.DataTextField = "MPIO"
			ddlCiudadEnvio.DataBind()
			ddlCiudadEnvio.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
			upEnvioEncuestas.Update()
		Else
			pnlCiudadEnvioPopup.Visible = False
			Dim oCiudades As New OP.TraficoEncuestas
			lblEncDisponiblesEnvio.Text = oCiudades.ObtenerEncuestasDisponibles(hfIdTrabajo.Value, hfIdUnidad.Value).FirstOrDefault.DISPONIBLES
			upEnvioEncuestas.Update()
		End If
		ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
	End Sub
	Protected Sub ddlCiudadEnvio_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCiudadEnvio.SelectedIndexChanged
		Dim oCiudades As New OP.TraficoEncuestas
		lblEncDisponiblesEnvio.Text = oCiudades.ObtenerEncuestasDisponibles(hfIdTrabajo.Value, hfIdUnidad.Value).Where(Function(x) x.C_MPIO = ddlCiudadEnvio.SelectedValue).FirstOrDefault.DISPONIBLES
		upEnvioEncuestas.Update()
	End Sub
	Protected Sub btnUpdateEnvio_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdateEnvio.Click
		If Not (IsNumeric(tbCantidadEnvio.Text)) Then
			ShowNotification("Debe agregar la cantidad antes de continuar", ShowNotifications.ErrorNotification)
			tbCantidadEnvio.Focus()
			Exit Sub
		End If
		If CInt(tbCantidadEnvio.Text) > CInt(lblEncDisponiblesEnvio.Text) Then
			ShowNotification("La cantidad a enviar no puede ser mayor a la disponible", ShowNotifications.ErrorNotification)
			tbCantidadEnvio.Focus()
			Exit Sub
		End If
		Dim Entidad As New OP_TraficoEncuestas
		Entidad.UnidadEnvia = hfIdUnidad.Value
		Entidad.UnidadRecibe = ddlDestinoPopupEnvio.SelectedValue
		Entidad.TrabajoId = hfIdTrabajo.Value
		Entidad.Cantidad = tbCantidadEnvio.Text
		Entidad.UsuarioEnvia = Session("IDUsuario").ToString
		If Me.pnlCiudadEnvioPopup.Visible = True Then
			If ddlCiudadEnvio.SelectedValue = "-1" Then
				ShowNotification("Debe seleccionar la ciudad antes", ShowNotifications.ErrorNotification)
				Exit Sub
			End If
			Entidad.Ciudad = ddlCiudadEnvio.SelectedValue
		End If
		Entidad.FechaEnvio = Date.UtcNow.AddHours(-5)
		Entidad.ObservacionesEnvio = tbObservacionesEnvio.Text
		Dim oTrafico As New OP.TraficoEncuestas
		oTrafico.GuardarTraficoEnvio(Entidad)
		tbCantidadEnvio.Text = ""
		tbObservacionesEnvio.Text = ""
		ShowNotification("Encuestas enviadas satisfactoriamente", ShowNotifications.InfoNotification)
		Me.lblEncDisponiblesEnvio.Text = ""
		Me.lblEncuestasDisponiblesDevolver.Text = ""
		Me.tbCantidadDevolver.Text = ""
		Me.tbCantidadEnvio.Text = ""
		Me.tbObservacionesDevolucion.Text = ""
		Me.tbObservacionesEnvio.Text = ""
		ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
	End Sub
	Private Sub gvRecepcion_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvRecepcion.RowCommand
		If e.CommandName = "Recibir" Then
			Dim id As Int64 = Int64.Parse(Me.gvRecepcion.DataKeys(CInt(e.CommandArgument))("Id"))
			Dim rowindex As Int32 = e.CommandArgument
			If CInt(gvRecepcion.Rows(rowindex).Cells(5).Text) < CInt(DirectCast(Me.gvRecepcion.Rows(rowindex).FindControl("txtRecibidas"), TextBox).Text) Then
				ShowNotification("La cantidad de encuestas recibidas no puede ser mayor que las enviadas", ShowNotifications.ErrorNotification)
				ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
				Exit Sub
			End If
			If (CInt(gvRecepcion.Rows(rowindex).Cells(5).Text) > CInt(DirectCast(Me.gvRecepcion.Rows(rowindex).FindControl("txtRecibidas"), TextBox).Text)) And DirectCast(Me.gvRecepcion.Rows(rowindex).FindControl("txtObservaciones"), TextBox).Text = String.Empty Then
				ShowNotification("Cuando la cantidad no coincida debe escribir en las observaciones", ShowNotifications.ErrorNotification)
				ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
				Exit Sub
			End If
			Dim oTrafico As New OP.TraficoEncuestas
			Dim Entidad As New OP_TraficoEncuestas
			Entidad = oTrafico.ObtenerTraficoEncuestaXId(id)
			Entidad.Cantidad = CInt(DirectCast(Me.gvRecepcion.Rows(rowindex).FindControl("txtRecibidas"), TextBox).Text)
			Entidad.UsuarioRecibe = Session("IDUsuario").ToString
			Entidad.FechaRecibo = Date.UtcNow.AddHours(-5)
			Entidad.ObservacionesEnvio = DirectCast(Me.gvRecepcion.Rows(rowindex).FindControl("txtObservaciones"), TextBox).Text
			oTrafico.GuardarTraficoEnvio(Entidad)
			CargarRecepcionEncuestas(hfIdTrabajo.Value, hfIdUnidad.Value)
			ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
		End If
	End Sub
	Protected Sub btnDevolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDevolver.Click
		If hfIdUnidad.Value = 28 Or hfIdUnidad.Value = 30 Then
			pnlCiudadDevolucionPopup.Visible = True
			Dim oCiudades As New OP.TraficoEncuestas
			ddlCiudadDevolucionPopup.DataSource = oCiudades.ObtenerEncuestasDisponiblesDevol(hfIdTrabajo.Value, hfIdUnidad.Value)
			ddlCiudadDevolucionPopup.DataValueField = "C_MPIO"
			ddlCiudadDevolucionPopup.DataTextField = "MPIO"
			ddlCiudadDevolucionPopup.DataBind()
			ddlCiudadDevolucionPopup.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
			upDevolucionEncuestas.Update()
		Else
			pnlCiudadDevolucionPopup.Visible = False
			Dim oCiudades As New OP.TraficoEncuestas
			lblEncuestasDisponiblesDevolver.Text = oCiudades.ObtenerEncuestasDisponiblesDevol(hfIdTrabajo.Value, hfIdUnidad.Value).FirstOrDefault.DISPONIBLES
			upDevolucionEncuestas.Update()
		End If
		ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
	End Sub
	Protected Sub btnDevolverPopup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDevolverPopup.Click
		If Not (IsNumeric(tbCantidadDevolver.Text)) Then
			ShowNotification("Debe agregar la cantidad antes de continuar", ShowNotifications.ErrorNotification)
			tbCantidadDevolver.Focus()
			Exit Sub
		End If
		If CInt(tbCantidadDevolver.Text) > CInt(lblEncuestasDisponiblesDevolver.Text) Then
			ShowNotification("La cantidad a enviar no puede ser mayor a la disponible", ShowNotifications.ErrorNotification)
			tbCantidadDevolver.Focus()
			Exit Sub
		End If
		Dim Entidad As New OP_TraficoEncuestas
		Entidad.UnidadEnvia = hfIdUnidad.Value
		Entidad.UnidadRecibe = ddlUnidadDevolucionPopup.SelectedValue
		Entidad.TrabajoId = hfIdTrabajo.Value
		Entidad.Cantidad = tbCantidadDevolver.Text
		Entidad.UsuarioEnvia = Session("IDUsuario").ToString
		If Me.pnlCiudadEnvioPopup.Visible = True Then
			Entidad.Ciudad = ddlCiudadDevolucionPopup.SelectedValue
		End If
		Entidad.Devolucion = True
		Entidad.FechaEnvio = Date.UtcNow.AddHours(-5)
		Entidad.ObservacionesEnvio = tbObservacionesDevolucion.Text
		Dim oTrafico As New OP.TraficoEncuestas
		oTrafico.GuardarTraficoEnvio(Entidad)
		tbCantidadEnvio.Text = ""
		tbObservacionesEnvio.Text = ""
		ShowNotification("Encuestas enviadas satisfactoriamente", ShowNotifications.InfoNotification)
		ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
	End Sub
	Protected Sub btnEstimacion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEstimacion.Click
		Dim oPlaneacion As New PlaneacionProduccion
		If hfIdUnidad.Value = 38 Then
			Me.tbEstimacion.Text = oPlaneacion.ObtenerEstimacionOPTrafico(hfIdTrabajo.Value).RMC
		ElseIf hfIdUnidad.Value = 28 Or hfIdUnidad.Value = 30 Then
			Me.tbEstimacion.Text = oPlaneacion.ObtenerEstimacionOPTrafico(hfIdTrabajo.Value).Critica
		ElseIf hfIdUnidad.Value = 20 Then
			Me.tbEstimacion.Text = oPlaneacion.ObtenerEstimacionOPTrafico(hfIdTrabajo.Value).Verificacion
		ElseIf hfIdUnidad.Value = 21 Then
			Me.tbEstimacion.Text = oPlaneacion.ObtenerEstimacionOPTrafico(hfIdTrabajo.Value).Captura
		End If
		Me.pnlEstimacion.Visible = True
		ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
	End Sub
	Protected Sub btnEstimacionOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEstimacionOk.Click
		If Not (IsNumeric(Me.tbEstimacion.Text)) Then
			ShowNotification("Debe agregar la estimación antes de continuar", ShowNotifications.ErrorNotification)
			ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
			Exit Sub
		End If
		If Not (CInt(Me.tbEstimacion.Text) > 0) Then
			ShowNotification("La estimación debe ser mayor a cero", ShowNotifications.ErrorNotification)
			ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
			Exit Sub
		End If
		Dim oPlaneacion As New PlaneacionProduccion
		Dim Ent As New OP_ProduccionEstimadaOPTrafico
		Ent.id = hfIdUnidad.Value
		If hfIdUnidad.Value = 38 Then
			Ent.RMC = tbEstimacion.Text
		ElseIf hfIdUnidad.Value = 28 Or hfIdUnidad.Value = 30 Then
			Ent.Critica = tbEstimacion.Text
		ElseIf hfIdUnidad.Value = 20 Then
			Ent.Verificacion = tbEstimacion.Text
		ElseIf hfIdUnidad.Value = 21 Then
			Ent.Captura = tbEstimacion.Text
		End If
		oPlaneacion.GuardarEstimacionOpTrafico(Ent)
		ShowNotification("Estimación guardada satisfactoriamente", ShowNotifications.ErrorNotification)
		Me.pnlEstimacion.Visible = False
		ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
	End Sub
	Protected Sub btnEstimacionCAncel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEstimacionCAncel.Click
		Me.pnlEstimacion.Visible = False
		ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
	End Sub
	Protected Sub ddlDevoluciones_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlDevoluciones.SelectedIndexChanged
		CargarRecibidos(hfIdTrabajo.Value, ddlDevoluciones.SelectedValue, hfIdUnidad.Value)
		ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
	End Sub
	Protected Sub ddlEnvios_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlEnvios.SelectedIndexChanged
		CargarEnvios(hfIdTrabajo.Value, hfIdUnidad.Value, ddlEnvios.SelectedValue)
		ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
	End Sub

	Protected Sub btnCapacitaciones_Click(sender As Object, e As EventArgs) Handles btnCapacitaciones.Click
		Response.Redirect("../TH_TalentoHumano/Capacitacion.aspx?idtrabajo=" & hfIdTrabajo.Value)
	End Sub
	Private Sub gvPersonalPorAsignar_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPersonalPorAsignar.RowCommand
		If e.CommandName = "Asignar" Then
			Dim oCoord As New CoordinacionCampoPersonal
			Dim Ent As New OP_PersonasAsignadasTrabajo
			Ent.Persona = Int64.Parse(Me.gvPersonalPorAsignar.DataKeys(CInt(e.CommandArgument))("Id"))
			Ent.TrabajoId = hfIdTrabajo.Value
			oCoord.GuardarPersonalAsignado(Ent)
			CargarPersonalAsignado(hfIdTrabajo.Value, hfIdCargoEjecuta.Value)
			CargarPersonalDisponible(hfIdTrabajo.Value, hfIdCargoEjecuta.Value)
			ActivateAccordion(3, EffectActivateAccordion.SlideEffect)
		End If
	End Sub
	Private Sub gvPersonalAsignado_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPersonalAsignado.RowCommand
		If e.CommandName = "Eliminar" Then
			Dim oCoordinacion As New CoordinacionCampoPersonal
			oCoordinacion.EliminarPersonalAsignado(Me.gvPersonalAsignado.DataKeys(CInt(e.CommandArgument))("Id"), hfIdTrabajo.Value)
			CargarPersonalAsignado(hfIdTrabajo.Value, hfIdCargoEjecuta.Value)
			CargarPersonalDisponible(hfIdTrabajo.Value, hfIdCargoEjecuta.Value)
		End If
		ActivateAccordion(3, EffectActivateAccordion.NoEffect)
	End Sub
#End Region

	Sub CargarPersonalAsignado(ByVal TrabajoId As Int64, ByVal CargoId As Int64)
		Dim oCoord As New CoordinacionCampoPersonal
		Dim lstPersonal As List(Of OP_PersonasAsignacion_Result)

		If CargoId = Cargos.TiposCargos.DigitadorRMC Then
			lstPersonal = oCoord.ObtenerPersonalSinAsignar(TrabajoId, Cargos.TiposCargos.Supervisor, Nothing)
			lstPersonal.AddRange(oCoord.ObtenerPersonalAsignado(TrabajoId, Cargos.TiposCargos.Encuestador, Nothing))
		Else
			lstPersonal = oCoord.ObtenerPersonalAsignado(TrabajoId, CargoId, Nothing)
		End If
		Me.gvPersonalAsignado.DataSource = lstPersonal

		gvPersonalAsignado.DataBind()
	End Sub
	Sub CargarPersonalDisponible(ByVal TrabajoId As Int64, ByVal CargoId As Int64)
		Dim oCoord As New CoordinacionCampoPersonal
		Dim lstPersonal As List(Of OP_PersonasAsignacion_Result)

		If CargoId = Cargos.TiposCargos.DigitadorRMC Then
			lstPersonal = oCoord.ObtenerPersonalSinAsignar(TrabajoId, Cargos.TiposCargos.Supervisor, Nothing)
			lstPersonal.AddRange(oCoord.ObtenerPersonalSinAsignar(TrabajoId, Cargos.TiposCargos.Encuestador, Nothing))
		Else
			lstPersonal = oCoord.ObtenerPersonalSinAsignar(TrabajoId, CargoId, Nothing)
		End If

		Me.gvPersonalPorAsignar.DataSource = lstPersonal
		Me.gvPersonalPorAsignar.DataBind()
	End Sub

	Protected Sub btnAnulacion_Click(sender As Object, e As EventArgs) Handles btnAnulacion.Click
		Response.Redirect("AnulacionEncuestas.aspx?TrabajoId=" & hfIdTrabajo.Value & "&IdUnidad=" & hfIdUnidad.Value)
	End Sub

	Private Sub gvEnvios_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvEnvios.PageIndexChanging
		gvEnvios.PageIndex = e.NewPageIndex
		CargarEnvios(hfIdTrabajo.Value, hfIdUnidad.Value, ddlEnvios.SelectedValue)
		ActivateAccordion(2, EffectActivateAccordion.NoEffect)
	End Sub

	Private Sub gvRecibidas_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvRecibidas.PageIndexChanging
		gvRecibidas.PageIndex = e.NewPageIndex
		CargarRecibidos(hfIdTrabajo.Value, ddlDevoluciones.SelectedValue, hfIdUnidad.Value)
		ActivateAccordion(2, EffectActivateAccordion.NoEffect)
	End Sub

	Private Sub gvEnvios_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEnvios.RowCommand
		If e.CommandName = "Eliminar" Then
			Dim idEnvio As Int64 = Me.gvEnvios.DataKeys(CInt(e.CommandArgument))("Id")
			Dim o As New OP.TraficoEncuestas
			o.BorrarEnvio(idEnvio)
			ShowNotification("Envio borrado", ShowNotifications.InfoNotification)
			CargarEnvios(hfIdTrabajo.Value, hfIdUnidad.Value, ddlEnvios.SelectedValue)
			ActivateAccordion(2, EffectActivateAccordion.NoEffect)
		End If
	End Sub

	Private Sub gvEnvios_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvEnvios.RowDataBound
		If e.Row.RowType = DataControlRowType.DataRow Then
			If IsDate(e.Row.Cells(5).Text) Then
				e.Row.Cells(9).Visible = False
			End If
		End If
	End Sub

	Private Sub btnTareas_Click(sender As Object, e As System.EventArgs) Handles btnTareas.Click
		Response.Redirect("../CORE/Gestion-Tareas.aspx?IdTrabajo=" & hfIdTrabajo.Value & "&URLRetorno=" & UrlOriginal.RE_GT_TraficoEncuestasRMC)
	End Sub

	Private Sub btnPersonalAsignado_Click(sender As Object, e As System.EventArgs) Handles btnPersonalAsignado.Click
		Dim excel As New List(Of Array)
		Dim Titulos As String = "IdAsignacion;TrabajoId;Nombres;Apellidos;Identificacion;Cargo;CodDane;Ciudad;FechaAsignacion;TipoContrato;IdContratista;NombreContratista"
		Dim DynamicColNames() As String
		Dim lstCambios As List(Of OP_ListadoPersonasAsignadasTrabajo_Result)
		Dim workbook = New XLWorkbook()
		Dim worksheet = workbook.Worksheets.Add("Listado")

		DynamicColNames = Titulos.Split(CChar(";"))
		excel.Add(DynamicColNames)
		Dim o As New CoordinacionCampoPersonal
		lstCambios = o.ListadoPersonasAsignadas(hfIdTrabajo.Value)

		For Each x In lstCambios
			excel.Add((x.IdAsignacion.ToString() & ";" & x.TrabajoId & ";" & x.Nombres & ";" & x.Apellidos & ";" & x.Identificacion & ";" & x.Cargo & ";" & x.CodDane.ToString & ";" & x.Ciudad & ";" & x.FechaAsignacion & ";" & x.Tipo & ";" & x.IDCONTRATISTA & ";" & x.CONTRATISTA).Split(CChar(";")).ToArray())
		Next
		worksheet.Cell("A1").Value = excel
		'worksheet.Range("C2:L100").DataType = XLCellValues.Number
		'worksheet.Range("C2:L100").Style.NumberFormat.NumberFormatId = 4
		Crearexcel(workbook, "Listado")
		ActivateAccordion(1, EffectActivateAccordion.NoEffect)
	End Sub

	Private Sub Crearexcel(ByVal workbook As XLWorkbook, ByVal name As String)
		Response.Clear()

		Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
		Response.AddHeader("content-disposition", "attachment;filename=""PersonalAsignadoTrabajo " & hfIdTrabajo.Value & ".xlsx""")

		Using memoryStream = New IO.MemoryStream()
			workbook.SaveAs(memoryStream)

			memoryStream.WriteTo(Response.OutputStream)
		End Using
		Response.End()
	End Sub
End Class