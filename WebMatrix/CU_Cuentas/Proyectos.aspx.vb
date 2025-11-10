Imports CoreProject
Imports WebMatrix.Util

Public Class Proyectos
	Inherits System.Web.UI.Page


	Enum tipoPresupuesto
		Normal = 1
		Observer = 2
	End Enum

#Region "Propiedades"

	Private _idUsuario As Int64
	Public Property idUsuario() As Int64
		Get
			Return _idUsuario
		End Get
		Set(ByVal value As Int64)
			_idUsuario = value
		End Set
	End Property

#End Region
#Region "Eventos"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		idUsuario = Session("IDUsuario")

		If Me.IsPostBack = False Then
			CargarProyectos()
			CargarEstudios()
			'CargarUnidades()
			CargarTiposProyectos()
			Dim permisos As New Datos.ClsPermisosUsuarios
			Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
			'If permisos.VerificarPermisoUsuario(2, UsuarioID) = False Then
			'    btnGrabar.Enabled = False
			'    gvProyectos.Columns.Item(6).Visible = False
			'    gvProyectos.Columns.Item(7).Visible = False
			'End If
			If Request.QueryString("IdEstudio") IsNot Nothing Then
				Limpiar()
				Dim oEstudio As New Estudio
				Dim JobBook As String = oEstudio.ObtenerXID(Int64.Parse(Request.QueryString("IdEstudio").ToString)).JobBook
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
				Me.txtNombreProyecto.Text = oEstudio.ObtenerXID(Int64.Parse(Request.QueryString("IdEstudio").ToString)).Nombre
				ddlEstudios.SelectedValue = Int64.Parse(Request.QueryString("IdEstudio").ToString())
				CargarPresupuestos(ddlEstudios.SelectedValue, Nothing, Nothing, gvPresupuestos)
				txtNombreProyecto.Focus()
				accordion0.Visible = False
				accordion1.Visible = True
				pnlEsquemaAnalisis.Visible = False
			Else
				pnlEsquemaAnalisis.Visible = True
			End If
			'Validar()
		End If
	End Sub

	Protected Sub ddlEstudios_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlEstudios.SelectedIndexChanged
		CargarPresupuestos(ddlEstudios.SelectedValue, Nothing, Nothing, gvPresupuestos)

	End Sub

	Protected Sub btnGrabar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGrabar.Click
		Dim oProyectos_Presupuestos As New Proyectos_Presupuestos
		Dim eProyecto As New PY_Proyectos_Get_Result
		Dim indPresupuestosAsignados As String = hfIndicesFilasPresupuestosAsignados.Value
		Dim indPresupuestosOriginales As String = hfIndicesFilasPresupuestosOriginales.Value
		Dim vecIndPresupuestosAsignados As String()
		Dim vecIndPresupuestosOriginales As String()
		Dim listPY_Proyectos_Presupuestos As New List(Of PY_Proyecto_Presupuesto_Get_Result)
		Dim listCU_Estudios_Presupuestos_Nuevos As New List(Of PY_Proyecto_Presupuesto_Get_Result)
		Dim listCU_Estudios_Presupuestos_Eliminados As New List(Of PY_Proyecto_Presupuesto_Get_Result)
		Dim oBLEstudio As New Estudio
		Dim oProyecto As New Proyecto
		Dim oPY As New PY_Proyectos
		Dim oPresupuesto As New Presupuesto
		Dim oDatos As New IQ.ControlCostos
		Dim presupuesto = oPresupuesto.ObtenerPresupuestosAsignadosXEstudio(ddlEstudios.SelectedValue)
		Dim DatosGenerales = oDatos.ObtenerDatosGeneralesPresupuesto(presupuesto(0).PropuestaId, presupuesto(0).Alternativa)
		Dim JobBook As String

		eProyecto.Nombre = txtNombreProyecto.Text
		eProyecto.EstudioId = ddlEstudios.SelectedValue
		eProyecto.UnidadId = ddlUnidades.SelectedValue



		If txtJobBook.Text = "" Then
			eProyecto.JobBook = txtJobBookInt.Text
		Else
			eProyecto.JobBook = txtJobBook.Text
		End If

		If eProyecto.JobBook = "" Or eProyecto.JobBook.EndsWith("00") Then
			ShowNotification("Debe escribir un número de JobBook válido antes de continuar", ShowNotifications.ErrorNotification)
			Exit Sub
		End If
		Dim oPropuesta As New Propuesta
		Dim idPropuesta As Int64 = oBLEstudio.ObtenerXID(ddlEstudios.SelectedValue).PropuestaId
		Dim jbPropuesta As String = oPropuesta.DevolverxID(idPropuesta).JobBook
		If Not (eProyecto.JobBook.StartsWith(jbPropuesta)) Then
			ShowNotification("El JobBook debe mantener la misma estructura desde la propuesta", ShowNotifications.ErrorNotification)
			Exit Sub
		End If

		eProyecto.TipoProyectoId = ddlTiposProyectos.SelectedValue
		'eProyecto.GerenteProyectos = Session("IDUsuario").ToString

		vecIndPresupuestosAsignados = indPresupuestosAsignados.Split(";").Where(Function(x) Not (String.IsNullOrEmpty(x))).ToArray
		vecIndPresupuestosOriginales = indPresupuestosOriginales.Split(";").Where(Function(x) Not (String.IsNullOrEmpty(x))).ToArray
		Dim idpres As Int64? = Me.gvPresupuestos.DataKeys(0).Value
		If String.IsNullOrEmpty(hfIdProyecto.Value) Then
			listPY_Proyectos_Presupuestos.Add(New PY_Proyecto_Presupuesto_Get_Result With {.PresupuestoId = idpres})
			hfIdProyecto.Value = oProyectos_Presupuestos.Grabar(eProyecto, listPY_Proyectos_Presupuestos)
			EnviarEmail()
			If eProyecto.TipoProyectoId = 1 Then
				EnviarEmailJBI()
			End If
		Else
			eProyecto.id = hfIdProyecto.Value
			'listCU_Estudios_Presupuestos_Nuevos = obtenerPresupuestosNuevos(vecIndPresupuestosAsignados, vecIndPresupuestosOriginales)
			'listCU_Estudios_Presupuestos_Eliminados = obtenerPresupuestosEliminados(vecIndPresupuestosAsignados, vecIndPresupuestosOriginales)
			oProyectos_Presupuestos.GrabarSoloProyecto(eProyecto)
		End If

		If DatosGenerales.TipoPresupuesto = tipoPresupuesto.Observer Or eProyecto.TipoProyectoId = 2 Then
			If txtJobBook.Text = "" Then
				JobBook = txtJobBookInt.Text
			Else
				JobBook = txtJobBook.Text
			End If
			oPresupuesto.ActualizarParNumJobBookEnIQ(JobBook, presupuesto(0).PropuestaId, presupuesto(0).Alternativa)
		End If

		oPY = oProyectos_Presupuestos.ObtenerProyecto(hfIdProyecto.Value)
		oPY.A1 = txtA1.Text
		oPY.A2 = txtA2.Text
		oPY.A3 = txtA3.Text
		oPY.A4 = txtA4.Text
		oPY.A5 = txtA5.Text
		oPY.A6 = txtA6.Text
		oPY.A7 = txtA7.Text
		oPY.A8 = txtA8.Text
		oProyectos_Presupuestos.GuardarProyecto(oPY)

		oProyecto.AsignarJBE_ProyectosCuali(hfIdProyecto.Value)
		CargarProyectos()
		log(hfIdProyecto.Value, 2)
		'Limpiar()

	End Sub

	Private Sub gvProyectos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvProyectos.RowCommand
		If e.CommandName = "PresupuestosAsignados" Then
			CargarPresupuestos(Me.gvProyectos.DataKeys(CInt(e.CommandArgument))("EstudioId"), Me.gvProyectos.DataKeys(CInt(e.CommandArgument))("Id"), True, gvPresupuestosAsignadosXProyecto)
			ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
			upPresupuestosAsignadosXProyecto.Update()
		ElseIf e.CommandName = "Actualizar" Then
			CargarProyecto(Me.gvProyectos.DataKeys(CInt(e.CommandArgument))("Id"))
			accordion0.Visible = False
			accordion1.Visible = True

		End If
	End Sub

	Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
		Limpiar()
		ddlEstudios.Focus()
		ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
	End Sub

	Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
		Buscar()

	End Sub

	Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
		Limpiar()
		accordion0.Visible = True
		accordion1.Visible = False
	End Sub
	Protected Sub gvProyectos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvProyectos.PageIndexChanging
		gvProyectos.PageIndex = e.NewPageIndex
		CargarProyectos()

	End Sub
	Private Sub gvPresupuestosAsignadosXProyecto_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPresupuestosAsignadosXProyecto.RowCommand
		Dim oePresupuesto As New CU_Presupuesto_Get_Result
		Dim oPresupuesto As New Presupuesto
		oePresupuesto = oPresupuesto.obtenerXId(gvPresupuestosAsignadosXProyecto.DataKeys(e.CommandArgument)("Id"))
		If e.CommandName = "ConsultarPresupuesto" Then
			Response.Redirect("../CAP/Cap_Principal.aspx?IdPropuesta=" & oePresupuesto.PropuestaId & "&Alternativa=" & oePresupuesto.Alternativa & "&Accion=5")
		End If
	End Sub

	Protected Sub btnDocumentos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDocumentos.Click
		If hfIdProyecto.Value <> "" Then
			Response.Redirect("\GD_Documentos\GD_Documentos.aspx?IdContenedor=" & hfIdProyecto.Value & "&IdDocumento=6")
		Else : ShowNotification("Guarde Primero el Proyecto", ShowNotifications.ErrorNotification)
		End If

	End Sub
#End Region

#Region "Metodos"
	Private Sub CargarProyectos()
		Dim oProyectos As New Proyecto
		gvProyectos.DataSource = oProyectos.obtenerXGerenteCuentas(idUsuario)
		gvProyectos.DataBind()

	End Sub

	Private Sub CargarEstudios()
		Dim oEstudios As New Estudio

		ddlEstudios.DataSource = oEstudios.Todos()
		ddlEstudios.DataTextField = "Nombre"
		ddlEstudios.DataValueField = "Id"
		ddlEstudios.DataBind()

		ddlEstudios.Items.Insert(0, New ListItem With {.Value = -1, .Text = "--Seleccione--"})

	End Sub

	Private Sub CargarUnidades()
		ddlUnidades.Items.Clear()
		If ddlTiposProyectos.SelectedValue <> 2 Then
			Dim oUnidades As New CoreProject.US.Unidades
			ddlUnidades.DataSource = oUnidades.obtenerUnidadesXTipoGrupoUnidad(TipoGrupoUnidad.Comercial)
			ddlUnidades.DataTextField = "Unidad"
			ddlUnidades.DataValueField = "id"
			ddlUnidades.DataBind()
			ddlUnidades.Items.Insert(0, New ListItem With {.Value = -1, .Text = "--Seleccione--"})
			ddlUnidades.Items.Remove(New ListItem With {.Value = "17", .Text = "Cualitativa"})
		Else
			ddlUnidades.Items.Insert(0, New ListItem With {.Value = 17, .Text = "Cualitativa"})
		End If

	End Sub

	Private Sub CargarTiposProyectos()
		Dim oTipoProyecto As New TipoProyecto
		ddlTiposProyectos.DataSource = oTipoProyecto.obtenerTodos
		ddlTiposProyectos.DataTextField = "TipoProyecto"
		ddlTiposProyectos.DataValueField = "id"
		ddlTiposProyectos.DataBind()

		ddlTiposProyectos.Items.Insert(0, New ListItem With {.Value = -1, .Text = "--Seleccione--"})

	End Sub

	Private Sub CargarPresupuestos(ByVal idEstudio As Long?, ByVal idProyecto As Long?, ByVal Asignado As Boolean?, ByRef grilla As GridView)



		Dim oPY_Presupuesto As New PY.Presupuesto

		If hfEstudioOriginal.Value <> "" AndAlso idEstudio.HasValue AndAlso idEstudio.Value = hfEstudioOriginal.Value Then
			hfIndicesFilasPresupuestosAsignados.Value = hfIndicesFilasPresupuestosOriginales.Value
		Else
			'TODO Deuda tecnica, aqui se debe corregir cuando se cambia el estudio, pero tiene algunos presupuestos asignados del otro
			hfIndicesFilasPresupuestosAsignados.Value = ""
		End If

		grilla.DataSource = oPY_Presupuesto.obtener(idEstudio, If(hfIdProyecto.Value <> "" And idProyecto.HasValue = False, CType(hfIdProyecto.Value, Long?), idProyecto), Asignado)
		grilla.DataBind()

	End Sub

	Sub CargarProyecto(ByVal id As Long)
		Dim oProyecto As New Proyecto
		Dim oPY_Proyectos_Get_Result As PY_Proyectos_Get_Result
		Dim oPresupuesto As New PY.Presupuesto

		oPY_Proyectos_Get_Result = oProyecto.obtenerXId(id)
		Dim x = oPresupuesto.obtener(oPY_Proyectos_Get_Result.EstudioId, id, True)

		hfEstudioOriginal.Value = oPY_Proyectos_Get_Result.EstudioId
		hfIdProyecto.Value = id

		hfIndicesFilasPresupuestosOriginales.Value = String.Join(";;", x.Select(Function(y) y.Id).ToArray)
		hfIndicesFilasPresupuestosAsignados.Value = hfIndicesFilasPresupuestosOriginales.Value
		hfIndicesFilasPresupuestosOriginales.Value = ";" & hfIndicesFilasPresupuestosOriginales.Value & ";"
		hfIndicesFilasPresupuestosAsignados.Value = hfIndicesFilasPresupuestosOriginales.Value

		txtNombreProyecto.Text = oPY_Proyectos_Get_Result.Nombre
		ddlEstudios.SelectedValue = oPY_Proyectos_Get_Result.EstudioId
		txtJobBook.Text = oPY_Proyectos_Get_Result.JobBook
		ddlTiposProyectos.SelectedValue = oPY_Proyectos_Get_Result.TipoProyectoId
		Dim oProyectos_Presupuestos As New Proyectos_Presupuestos
		Dim opI = oProyectos_Presupuestos.ObtenerProyecto(hfIdProyecto.Value)

		txtA1.Text = opI.A1
		txtA2.Text = opI.A2
		txtA3.Text = opI.A3
		txtA4.Text = opI.A4
		txtA5.Text = opI.A5
		txtA6.Text = opI.A6
		txtA7.Text = opI.A7
		txtA8.Text = opI.A8
		CargarUnidades()

		ddlUnidades.SelectedValue = oPY_Proyectos_Get_Result.UnidadId

		CargarPresupuestos(oPY_Proyectos_Get_Result.EstudioId, oPY_Proyectos_Get_Result.id, Nothing, gvPresupuestos)

	End Sub
	Private Function obtenerPresupuestosNuevos(ByVal presupuestosAsignados() As String, ByVal presupuestosOriginales() As String) As List(Of PY_Proyecto_Presupuesto_Get_Result)
		Dim listPY_Proyectos_Presupuestos_Nuevos As New List(Of PY_Proyecto_Presupuesto_Get_Result)
		Dim PresupuestosNuevos = (From x In presupuestosAsignados Group Join y In presupuestosOriginales On x Equals y Into Coincidencia = Group Where Coincidencia.Count = 0).ToArray

		For Each i In PresupuestosNuevos
			listPY_Proyectos_Presupuestos_Nuevos.Add(New PY_Proyecto_Presupuesto_Get_Result With {.PresupuestoId = i.x})
		Next

		Return listPY_Proyectos_Presupuestos_Nuevos

	End Function
	Private Function obtenerPresupuestosEliminados(ByVal presupuestosAsignados() As String, ByVal presupuestosOriginales() As String) As List(Of PY_Proyecto_Presupuesto_Get_Result)
		Dim listPY_Proyectos_Presupuestos_Eliminados As New List(Of PY_Proyecto_Presupuesto_Get_Result)
		Dim PresupuestosEliminados = (From x In presupuestosOriginales Group Join y In presupuestosAsignados On x Equals y Into Coincidencia = Group Where Coincidencia.Count = 0).ToArray

		For Each i In PresupuestosEliminados
			listPY_Proyectos_Presupuestos_Eliminados.Add(New PY_Proyecto_Presupuesto_Get_Result With {.PresupuestoId = i.x})
		Next

		Return listPY_Proyectos_Presupuestos_Eliminados

	End Function
	Sub Limpiar()
		txtNombreProyecto.Text = ""
		ddlEstudios.SelectedValue = -1
		ddlUnidades.Items.Clear()
		txtJobBook.Text = ""
		CargarTiposProyectos()
		ddlTiposProyectos.SelectedValue = -1
		gvPresupuestos.DataSource = Nothing
		gvPresupuestos.DataBind()

		hfIdProyecto.Value = ""
		hfEstudioOriginal.Value = ""
		hfIndicesFilasPresupuestosAsignados.Value = ""
		hfIndicesFilasPresupuestosOriginales.Value = ""


	End Sub

	Sub Buscar()
		Dim oProyecto As New Proyecto
		gvProyectos.DataSource = oProyecto.obtenerTodosCampos(Me.txtBuscar.Text)
		gvProyectos.DataBind()
	End Sub

	Public Sub Validar()
		Try
			If Session("IDUsuario") IsNot Nothing Then
				Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
				Dim oValidacion As New Validacion
				If Not oValidacion.ValidarPermiso("Propuesta", UsuarioID) Then
					btnGrabar.Enabled = False
					gvProyectos.Columns.Item(6).Visible = False
					gvProyectos.Columns.Item(7).Visible = False
				Else
					btnGrabar.Enabled = True
					gvProyectos.Columns.Item(5).Visible = True
					gvProyectos.Columns.Item(6).Visible = True
				End If
			End If

		Catch ex As Exception
			Throw ex
		End Try
	End Sub

	Sub EnviarEmail()
		Dim oEnviarCorreo As New EnviarCorreo
		Try
			If String.IsNullOrEmpty(hfIdProyecto.Value) Then
				Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar una solicitud de Gerente de Proyectos")
			End If
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/NuevoProyectoCoordinacion.aspx?idProyecto=" & hfIdProyecto.Value)
			'Dim page As Page = DirectCast(Context.Handler, Page)
			'ScriptManager.RegisterStartupScript(page, GetType(Page), "Anuncio", script, True)
			ActivateAccordion(0, EffectActivateAccordion.NoEffect)
		Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
			ActivateAccordion(0, EffectActivateAccordion.NoEffect)
		End Try
	End Sub

	Sub EnviarEmailJBI()
		Dim oEnviarCorreo As New EnviarCorreo
		Try
			If String.IsNullOrEmpty(hfIdProyecto.Value) Then
				Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar una solicitud de Gerente de Proyectos")
			End If
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/NuevoProyectoJBI.aspx?idProyecto=" & hfIdProyecto.Value)
			'Dim page As Page = DirectCast(Context.Handler, Page)
			'ScriptManager.RegisterStartupScript(page, GetType(Page), "Anuncio", script, True)

		Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)

		End Try
	End Sub

	Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
		Dim log As New LogEjecucion
		log.Guardar(16, iddoc, Now(), Session("IDUsuario"), idaccion)

	End Sub
#End Region

	Protected Sub ddlTiposProyectos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTiposProyectos.SelectedIndexChanged
		CargarUnidades()
	End Sub
End Class