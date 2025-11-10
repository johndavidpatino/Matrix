Imports WebMatrix.Util
Imports CoreProject

Public Class PersonasF
	Inherits System.Web.UI.Page


#Region "Grillas"
	Sub CargarTipoIdentificacion()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlTipoIdentificacion.DataSource = o.TipoIdentificacionList
		Me.ddlTipoIdentificacion.DataValueField = "id"
		Me.ddlTipoIdentificacion.DataTextField = "TipoIdentificacion"
		Me.ddlTipoIdentificacion.DataBind()
		Me.ddlTipoIdentificacion.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarSexo()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlSexo.DataSource = o.SexoList
		Me.ddlSexo.DataValueField = "id"
		Me.ddlSexo.DataTextField = "Sexo"
		Me.ddlSexo.DataBind()
		Me.ddlSexo.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarEstadoCivil()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlEstadoCivil.DataSource = o.EstadoCivilList
		Me.ddlEstadoCivil.DataValueField = "id"
		Me.ddlEstadoCivil.DataTextField = "EstadoCivil"
		Me.ddlEstadoCivil.DataBind()
		Me.ddlEstadoCivil.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarNivelEducativo()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlNivelEducativo.DataSource = o.NivelEducativoList
		Me.ddlNivelEducativo.DataValueField = "id"
		Me.ddlNivelEducativo.DataTextField = "NivelEducativo"
		Me.ddlNivelEducativo.DataBind()
		Me.ddlNivelEducativo.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarGruposSanguineos()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlGrupoSanguineo.DataSource = o.GruposSanguineosList
		Me.ddlGrupoSanguineo.DataValueField = "id"
		Me.ddlGrupoSanguineo.DataTextField = "GrupoSanguineo"
		Me.ddlGrupoSanguineo.DataBind()
		Me.ddlGrupoSanguineo.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarCiudades()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlCiudadResidencia.DataSource = o.CiudadesList
		Me.ddlCiudadResidencia.DataValueField = "id"
		Me.ddlCiudadResidencia.DataTextField = "Ciudad"
		Me.ddlCiudadResidencia.DataBind()
		Me.ddlCiudadResidencia.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarEstadoActual()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlEstadoActual.DataSource = o.EstadosActualesList
		Me.ddlEstadoActual.DataValueField = "id"
		Me.ddlEstadoActual.DataTextField = "EstadoActual"
		Me.ddlEstadoActual.DataBind()
		Me.ddlEstadoActual.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarEmpresas()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlEmpresa.DataSource = o.EmpresasList
		Me.ddlEmpresa.DataValueField = "id"
		Me.ddlEmpresa.DataTextField = "Empresa"
		Me.ddlEmpresa.DataBind()
		Me.ddlEmpresa.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarTiposContrato()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlTipoContrato.DataSource = o.TipoContratoList
		Me.ddlTipoContrato.DataValueField = "id"
		Me.ddlTipoContrato.DataTextField = "Tipo"
		Me.ddlTipoContrato.DataBind()
		Me.ddlTipoContrato.Items.Insert(0, New ListItem With {.Value = "0", .Text = "--Seleccione--"})
	End Sub

	Sub CargarTiposSalario()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlTipoSalario.DataSource = o.TipoSalarioList
		Me.ddlTipoSalario.DataValueField = "id"
		Me.ddlTipoSalario.DataTextField = "TipoSalario"
		Me.ddlTipoSalario.DataBind()
		Me.ddlTipoSalario.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarFormasSalario()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlFormaSalario.DataSource = o.FormasSalarioList
		Me.ddlFormaSalario.DataValueField = "id"
		Me.ddlFormaSalario.DataTextField = "FormaSalario"
		Me.ddlFormaSalario.DataBind()
		Me.ddlFormaSalario.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarBancos()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlBanco.DataSource = o.BancosList
		Me.ddlBanco.DataValueField = "id"
		Me.ddlBanco.DataTextField = "Banco"
		Me.ddlBanco.DataBind()
		Me.ddlBanco.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarTipoCuenta()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlTipoCuenta.DataSource = o.TiposCuentaList
		Me.ddlTipoCuenta.DataValueField = "id"
		Me.ddlTipoCuenta.DataTextField = "TipoCuenta"
		Me.ddlTipoCuenta.DataBind()
		Me.ddlTipoCuenta.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarCargos()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlUltimoCargo.DataSource = o.CargosList
		Me.ddlUltimoCargo.DataValueField = "id"
		Me.ddlUltimoCargo.DataTextField = "Cargo"
		Me.ddlUltimoCargo.DataBind()
		Me.ddlUltimoCargo.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})

		Me.ddlCargo.DataSource = o.CargosList
		Me.ddlCargo.DataValueField = "id"
		Me.ddlCargo.DataTextField = "Cargo"
		Me.ddlCargo.DataBind()
		Me.ddlCargo.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarFondoPensiones()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlFondoPensiones.DataSource = o.FondosPensionesList
		Me.ddlFondoPensiones.DataValueField = "id"
		Me.ddlFondoPensiones.DataTextField = "FondoPensiones"
		Me.ddlFondoPensiones.DataBind()
		Me.ddlFondoPensiones.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarFondoCesantias()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlFondoCesantias.DataSource = o.FondosCesantiasList
		Me.ddlFondoCesantias.DataValueField = "id"
		Me.ddlFondoCesantias.DataTextField = "FondoCesantias"
		Me.ddlFondoCesantias.DataBind()
		Me.ddlFondoCesantias.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarEPS()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlEPS.DataSource = o.EPSList
		Me.ddlEPS.DataValueField = "id"
		Me.ddlEPS.DataTextField = "EPS"
		Me.ddlEPS.DataBind()
		Me.ddlEPS.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarCajaCompensacion()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlCajaCompensacion.DataSource = o.CajaCompensacionList
		Me.ddlCajaCompensacion.DataValueField = "id"
		Me.ddlCajaCompensacion.DataTextField = "CajaCompensacion"
		Me.ddlCajaCompensacion.DataBind()
		Me.ddlCajaCompensacion.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarARL()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlARL.DataSource = o.ARLList
		Me.ddlARL.DataValueField = "id"
		Me.ddlARL.DataTextField = "ARL"
		Me.ddlARL.DataBind()
		Me.ddlARL.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarBU()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlBU.DataSource = o.BUList
		Me.ddlBU.DataValueField = "id"
		Me.ddlBU.DataTextField = "BU"
		Me.ddlBU.DataBind()
		Me.ddlBU.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarArea()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlArea.DataSource = o.AreaList(ddlBU.SelectedValue)
		Me.ddlArea.DataValueField = "id"
		Me.ddlArea.DataTextField = "Area"
		Me.ddlArea.DataBind()
		Me.ddlArea.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarJefeInmediato()
		Dim o As New CoreProject.RegistroPersonas
		Dim li = (From list In o.ListadoPersonasActivas
				  Select id = list.id, Nombre = list.Nombres & " " & list.Apellidos
				  Order By Nombre)
		Me.ddlJefeInmediato.DataSource = li
		Me.ddlJefeInmediato.DataValueField = "id"
		Me.ddlJefeInmediato.DataTextField = "Nombre"
		Me.ddlJefeInmediato.DataBind()
		Me.ddlJefeInmediato.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub CargarSedes()
		Dim o As New CoreProject.RegistroPersonas
		Me.ddlSede.DataSource = o.SedesList
		Me.ddlSede.DataValueField = "id"
		Me.ddlSede.DataTextField = "Sede"
		Me.ddlSede.DataBind()
		Me.ddlSede.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
	End Sub

	Sub cargarTiposEncuestadores()
		Dim o As New CoreProject.TipoEncuestador
		ddlTipoEncuestador.DataSource = o.obtenerTodos
		ddlTipoEncuestador.DataTextField = "Tipo"
		ddlTipoEncuestador.DataValueField = "Id"
		ddlTipoEncuestador.DataBind()
		ddlTipoEncuestador.Items.Insert(0, New ListItem With {.Value = -1, .Text = "--Seleccione--"})
	End Sub

	Private Sub ddlBU_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlBU.SelectedIndexChanged
		CargarArea()
		ActivateAccordion(4, EffectActivateAccordion.NoEffect)
	End Sub


	Sub CargarGridPersonas()
		Dim o As New CoreProject.RegistroPersonas
		Dim cedula As Int64? = Nothing
		If IsNumeric(txtCedulaBuscar.Text) Then cedula = txtCedulaBuscar.Text
		Me.gvPersonas.DataSource = o.TH_PersonasGet(cedula, txtNombreBuscar.Text)
		Me.gvPersonas.DataBind()
	End Sub

#End Region

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			CargarTipoIdentificacion()
			CargarSexo()
			CargarEstadoCivil()
			CargarNivelEducativo()
			CargarGruposSanguineos()
			CargarCiudades()
			CargarEstadoActual()
			CargarEmpresas()
			CargarTiposContrato()
			CargarTiposSalario()
			CargarFormasSalario()
			CargarBancos()
			CargarTipoCuenta()
			CargarCargos()
			CargarEPS()
			CargarFondoCesantias()
			CargarFondoPensiones()
			CargarCajaCompensacion()
			CargarARL()
			CargarBU()
			CargarJefeInmediato()
			CargarGridPersonas()
			CargarSedes()
			cargarTiposEncuestadores()
		End If
	End Sub


	Private Sub gvPersonas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPersonas.PageIndexChanging
		gvPersonas.PageIndex = e.NewPageIndex
		CargarGridPersonas()
	End Sub

	Sub Guardar()
		Dim nuevo As Boolean = False
		If hfNuevo.Value = "1" Then
			nuevo = False
		Else
			nuevo = True
		End If
		Dim US As New US_Entities
		Dim e As New TH_Personas2
		Dim o As New CoreProject.RegistroPersonas
		If nuevo = False Then
			e = o.ObtenerPersona(hfID.Value)
		End If
		e.id = Me.txtCedula.Text
		e.TipoId = Me.ddlTipoIdentificacion.SelectedValue
		e.LugarExpedicion = Me.txtLugarExpedicion.Text
		If IsDate(txtFechaExpedicion.Text) Then e.FechaExpedicion = txtFechaExpedicion.Text
		e.Nombres = txtNombres.Text
		e.Apellidos = txtApellidos.Text
		e.Nacionalidad = txtNacionalidad.Text
		e.Sexo = Me.ddlSexo.SelectedValue
		e.GrupoSanguineo = Me.ddlGrupoSanguineo.SelectedValue
		e.EstadoCivil = Me.ddlEstadoCivil.SelectedValue
		If IsDate(txtFechaNacimiento.Text) Then e.FechaNacimiento = txtFechaNacimiento.Text
		e.CiudadId = Me.ddlCiudadResidencia.SelectedValue
		e.BarrioResidencia = txtBarrio.Text
		e.Direccion = txtDireccion.Text
		e.Telefono1 = txtTelefono1.Text
		e.Telefono2 = txtTelefono2.Text
		e.Celular = txtCelular.Text
		e.EmailPersonal = txtEmailPersonal.Text
		e.NivelEducativo = Me.ddlNivelEducativo.SelectedValue
		'e.Profesion = txtProfesion.Text
		e.BU = ddlBU.SelectedValue
		If IsNumeric(ddlArea.SelectedValue) Then e.Area = ddlArea.SelectedValue
		e.Sede = ddlSede.SelectedValue
		e.Cargo = ddlCargo.SelectedValue
		e.JefeInmediato = ddlJefeInmediato.SelectedValue
		If IsDate(txtFechaIngreso.Text) Then e.FechaIngreso = txtFechaIngreso.Text
		If ddlTipoContrato.SelectedValue <> e.TipoContratacion Then logCambio("TipoContrato", e.TipoContratacion.ToString(), ddlTipoContrato.SelectedValue.ToString(), e.id)
		e.TipoContratacion = ddlTipoContrato.SelectedValue

		e.Empresa = ddlEmpresa.SelectedValue
		e.TipoSalario = ddlTipoSalario.SelectedValue
		e.FormaSalario = ddlFormaSalario.SelectedValue
		If IsNumeric(txtSalario.Text) Then e.SalarioActual = txtSalario.Text
		If IsNumeric(txtUltimoSalario.Text) Then e.UltimoSalario = txtUltimoSalario.Text Else e.UltimoSalario = Nothing
		If IsDate(txtFechaUltimoAscenso.Text) Then e.FechaUltimoAscenso = txtFechaUltimoAscenso.Text
		e.UltimoCargo = ddlUltimoCargo.SelectedValue
		If e.EstadoActual <> ddlEstadoActual.SelectedValue Then logCambio("Estado", e.EstadoActual.ToString(), ddlEstadoActual.SelectedValue.ToString(), e.id)
		e.EstadoActual = ddlEstadoActual.SelectedValue
		If IsDate(txtFechaVencimientoContrato.Text) Then e.FechaVencimientoContrato = txtFechaVencimientoContrato.Text
		If IsDate(txtFechaRetiro.Text) Then e.FechaRetiro = txtFechaRetiro.Text
		e.Banco = ddlBanco.SelectedValue
		e.TipoCuenta = ddlTipoCuenta.SelectedValue
		e.CuentaBanco = txtCuentaBanco.Text
		e.FondoPensiones = ddlFondoPensiones.SelectedValue
		e.FondoCesantias = ddlFondoCesantias.SelectedValue
		e.EPS = ddlEPS.SelectedValue
		e.CajaCompensacion = ddlCajaCompensacion.SelectedValue
		e.ARL = ddlARL.SelectedValue
		e.MotivoRetiro = txtMotivoRetiro.Text
		If IsNumeric(ddlNivelBI.SelectedValue) Then e.NivelBI = ddlNivelBI.SelectedValue
		e.HeadCount = chbHeadCount.Checked
		If ddlEstadoActual.SelectedValue = 2 Then
			e.Activo = False
			US.US_Usuarios_Edit(txtCedula.Text, Nothing, Nothing, Nothing, Nothing, Nothing, False)
		Else
			e.Activo = True
			US.US_Usuarios_Edit(txtCedula.Text, Nothing, Nothing, Nothing, Nothing, Nothing, True)
		End If
		If nuevo Then
			o.GuardarPersona(e, nuevo)
		Else
			o.actualizar(e, hfID.Value, txtCedula.Text)
		End If
		If ddlCargo.SelectedValue = "13" Then
			o.GuardarTipoEncuestador(e.id, ddlTipoEncuestador.SelectedValue)
		Else
			o.EliminarEncuestador(e.id)
		End If
		If nuevo = True Then
			log(txtCedula.Text, 1)
		Else
			log(txtCedula.Text, 3)
		End If
	End Sub

	Sub CargarDatos()
		' Dim e As New CoreProject.TH_Personas2
		Dim e As New TH_Personas2
		Dim o As New CoreProject.RegistroPersonas

		e = o.ObtenerPersona(Me.txtCedula.Text)
		Limpiar()
		hfID.Value = e.id
		If IsNumeric(e.id) Then txtCedula.Text = e.id
		If IsNumeric(e.TipoId) Then Me.ddlTipoIdentificacion.SelectedValue = e.TipoId
		Me.txtLugarExpedicion.Text = e.LugarExpedicion
		If IsDate(e.FechaExpedicion) Then txtFechaExpedicion.Text = e.FechaExpedicion
		txtNombres.Text = e.Nombres
		txtApellidos.Text = e.Apellidos
		txtNacionalidad.Text = e.Nacionalidad
		If IsNumeric(e.Sexo) Then Me.ddlSexo.SelectedValue = e.Sexo
		If IsNumeric(e.GrupoSanguineo) Then Me.ddlGrupoSanguineo.SelectedValue = e.GrupoSanguineo
		If IsNumeric(e.EstadoCivil) Then Me.ddlEstadoCivil.SelectedValue = e.EstadoCivil
		If IsDate(e.FechaNacimiento) Then txtFechaNacimiento.Text = e.FechaNacimiento
		If IsNumeric(e.CiudadId) Then Me.ddlCiudadResidencia.SelectedValue = e.CiudadId
		txtBarrio.Text = e.BarrioResidencia
		txtDireccion.Text = e.Direccion
		txtTelefono1.Text = e.Telefono1
		txtTelefono2.Text = e.Telefono2
		txtCelular.Text = e.Celular
		txtEmailPersonal.Text = e.EmailPersonal
		If IsNumeric(e.NivelEducativo) Then Me.ddlNivelEducativo.SelectedValue = e.NivelEducativo
		'txtProfesion.Text = e.Profesion
		If IsNumeric(e.BU) Then ddlBU.SelectedValue = e.BU
		If IsNumeric(e.Area) Then ddlArea.SelectedValue = e.Area
		If IsNumeric(e.Sede) Then ddlSede.SelectedValue = e.Sede
		Try
			If IsNumeric(e.Cargo) Then
				ddlCargo.SelectedValue = e.Cargo
				If ddlCargo.SelectedValue = Cargos.TiposCargos.Encuestador Then
					ddlTipoEncuestador.Enabled = True
					ddlTipoEncuestador.SelectedValue = o.ObtenerEncuestador(e.id).OP_TipoEncuestador.id
				Else
					ddlTipoEncuestador.Enabled = False
					ddlTipoEncuestador.SelectedValue = -1
				End If
			End If
		Catch ex As Exception
		End Try
		Try
			If IsNumeric(e.JefeInmediato) Then ddlJefeInmediato.SelectedValue = e.JefeInmediato
		Catch ex As Exception
		End Try

		If IsDate(e.FechaIngreso) Then txtFechaIngreso.Text = e.FechaIngreso
		If IsNumeric(e.TipoContratacion) Then ddlTipoContrato.SelectedValue = e.TipoContratacion
		If IsNumeric(e.Empresa) Then ddlEmpresa.SelectedValue = e.Empresa
		If IsNumeric(e.TipoSalario) Then ddlTipoSalario.SelectedValue = e.TipoSalario
		If IsNumeric(e.FormaSalario) Then ddlFormaSalario.SelectedValue = e.FormaSalario
		If IsNumeric(e.SalarioActual) Then txtSalario.Text = e.SalarioActual
		If IsNumeric(e.UltimoSalario) Then txtUltimoSalario.Text = e.UltimoSalario
		If IsDate(e.FechaUltimoAscenso) Then txtFechaUltimoAscenso.Text = e.FechaUltimoAscenso
		If IsNumeric(e.UltimoCargo) Then ddlUltimoCargo.SelectedValue = e.UltimoCargo
		If IsNumeric(e.EstadoActual) Then ddlEstadoActual.SelectedValue = e.EstadoActual
		If IsDate(e.FechaVencimientoContrato) Then txtFechaVencimientoContrato.Text = e.FechaVencimientoContrato
		If IsDate(e.FechaRetiro) Then txtFechaRetiro.Text = e.FechaRetiro
		Me.txtMotivoRetiro.Text = e.MotivoRetiro
		If IsNumeric(e.Banco) Then ddlBanco.SelectedValue = e.Banco
		If IsNumeric(e.TipoCuenta) Then ddlTipoCuenta.SelectedValue = e.TipoCuenta
		If IsNumeric(e.CuentaBanco) Then txtCuentaBanco.Text = e.CuentaBanco
		If IsNumeric(e.FondoPensiones) Then ddlFondoPensiones.SelectedValue = e.FondoPensiones
		If IsNumeric(e.FondoCesantias) Then ddlFondoCesantias.SelectedValue = e.FondoCesantias
		If IsNumeric(e.EPS) Then ddlEPS.SelectedValue = e.EPS
		If IsNumeric(e.CajaCompensacion) Then ddlCajaCompensacion.SelectedValue = e.CajaCompensacion
		If IsNumeric(e.ARL) Then ddlARL.SelectedValue = e.ARL
		If IsNumeric(e.NivelBI) Then ddlNivelBI.SelectedValue = e.NivelBI
		If Not e.HeadCount Is Nothing Then chbHeadCount.Checked = e.HeadCount
		hfNuevo.Value = "1"
	End Sub
	Sub Limpiar()
		Me.hfID.Value = String.Empty
		Me.txtCedula.Text = String.Empty
		Me.ddlTipoIdentificacion.SelectedValue = -1
		Me.txtLugarExpedicion.Text = String.Empty
		txtFechaExpedicion.Text = String.Empty
		txtNombres.Text = String.Empty
		txtApellidos.Text = String.Empty
		txtNacionalidad.Text = String.Empty
		Me.ddlSexo.SelectedValue = -1
		Me.ddlGrupoSanguineo.SelectedValue = -1
		Me.ddlEstadoCivil.SelectedValue = -1
		txtFechaNacimiento.Text = String.Empty
		Me.ddlCiudadResidencia.SelectedValue = -1
		txtBarrio.Text = String.Empty
		txtDireccion.Text = String.Empty
		txtTelefono1.Text = String.Empty
		txtTelefono2.Text = String.Empty
		txtCelular.Text = String.Empty
		txtEmailPersonal.Text = String.Empty
		Me.ddlNivelEducativo.SelectedValue = -1
		txtProfesion.Text = String.Empty
		ddlBU.SelectedValue = -1
		ddlArea.SelectedValue = -1
		ddlSede.SelectedValue = -1
		ddlCargo.SelectedValue = -1
		ddlJefeInmediato.SelectedValue = -1
		txtFechaIngreso.Text = String.Empty
		ddlTipoContrato.SelectedValue = 0
		ddlEmpresa.SelectedValue = -1
		ddlTipoSalario.SelectedValue = -1
		ddlFormaSalario.SelectedValue = -1
		txtSalario.Text = String.Empty
		txtUltimoSalario.Text = String.Empty
		txtFechaUltimoAscenso.Text = String.Empty
		ddlUltimoCargo.SelectedValue = -1
		ddlEstadoActual.SelectedValue = -1
		txtFechaVencimientoContrato.Text = String.Empty
		txtFechaRetiro.Text = String.Empty
		ddlBanco.SelectedValue = -1
		ddlTipoCuenta.SelectedValue = -1
		txtCuentaBanco.Text = String.Empty
		ddlFondoPensiones.SelectedValue = -1
		ddlFondoCesantias.SelectedValue = -1
		ddlEPS.SelectedValue = -1
		ddlCajaCompensacion.SelectedValue = -1
		ddlARL.SelectedValue = -1
		chbHeadCount.Checked = False
		Me.txtMotivoRetiro.Text = ""
		Me.ddlNivelBI.SelectedValue = 0
		hfNuevo.Value = "0"
		CargarGridPersonas()
	End Sub

	Private Sub btnCancelar_Click(sender As Object, e As System.EventArgs) Handles btnCancelar.Click
		Limpiar()
		ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
	End Sub

	Private Sub gvPersonas_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPersonas.RowCommand
		If e.CommandName = "Actualizar" Then
			Me.txtCedula.Text = gvPersonas.DataKeys(CInt(e.CommandArgument))("Id")
			CargarDatos()
			ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
		End If
	End Sub

	Private Sub btnGuardar_Click(sender As Object, e As System.EventArgs) Handles btnGuardar.Click
		If ddlTipoContrato.SelectedValue = "0" Then
			ShowNotification("El tipo de contrato es obligatorio", ShowNotifications.ErrorNotificationLong)
			ActivateAccordion(3, EffectActivateAccordion.SlideEffect)
			Exit Sub
		End If
		If ddlCargo.SelectedValue = "13" Then
			If ddlTipoEncuestador.SelectedValue = "-1" Then
				ShowNotification("Debe seleccionar el tipo de encuestador", ShowNotifications.ErrorNotification)
				ActivateAccordion(4, EffectActivateAccordion.SlideEffect)
				Exit Sub
			End If
		End If
		If ddlCiudadResidencia.SelectedValue = "-1" Then
			ShowNotification("Por favor seleccione la ciudad", ShowNotifications.ErrorNotificationLong)
			ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
			Exit Sub
		End If
		If ddlEstadoActual.SelectedValue = "-1" Then
			ShowNotification("Por favor seleccione el estado actual", ShowNotifications.ErrorNotificationLong)
			ActivateAccordion(3, EffectActivateAccordion.SlideEffect)
			Exit Sub
		End If
		Try
			Guardar()
			Limpiar()
			ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
			ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
		Catch ex As Exception
			ShowNotification("Ocurrio un error al ejecutar la instrucción - " & ex.Message, ShowNotifications.InfoNotification)
			ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
		End Try

	End Sub

	Private Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
		CargarGridPersonas()
		Me.txtCedulaBuscar.Text = ""
		Me.txtNombreBuscar.Text = ""
		ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
	End Sub

	Private Sub btnNuevo_Click(sender As Object, e As System.EventArgs) Handles btnNuevo.Click
		Limpiar()
		ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
	End Sub

	Private Sub ddlCargo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCargo.SelectedIndexChanged
		If ddlCargo.SelectedValue = "13" Then
			ddlTipoEncuestador.Enabled = True
			cargarTiposEncuestadores()
		Else
			ddlTipoEncuestador.Enabled = False
		End If
		ActivateAccordion(4, EffectActivateAccordion.NoEffect)
	End Sub

	Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
		Try
			Dim log As New CoreProject.LogEjecucion
			log.Guardar(38, iddoc, Now(), Session("IDUsuario"), idaccion)
		Catch ex As Exception
			Throw ex
		End Try
	End Sub

	Public Sub logCambio(ByVal Campo As String, ByVal ValorOriginal As String, ByVal ValorNuevo As String, ByVal persona As Int64)
		Dim o As New CoreProject.Personas
		o.LogPersonas(Session("IDUsuario"), Campo, ValorOriginal, ValorNuevo, persona)
	End Sub

	Private Sub ListadoGeneralPersonasMatrix_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
		Dim permisos As New Datos.ClsPermisosUsuarios
		Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
		If permisos.VerificarPermisoUsuario(143, UsuarioID) = False Then
			Response.Redirect("../TH_TalentoHumano/Default.aspx")
		End If
	End Sub
End Class