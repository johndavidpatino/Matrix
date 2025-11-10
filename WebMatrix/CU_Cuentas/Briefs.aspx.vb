Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Public Class Briefs
    Inherits System.Web.UI.Page
#Region " Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            CargarUnidades()
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(2, UsuarioID) = False Then
                btnNuevo.Visible = False
                btnGuardar.Enabled = False
                bntcrearpropuesta.Enabled = False
                gvDatos.Columns.Item(4).Visible = False
                gvDatos.Columns.Item(6).Visible = False
                gvDatos.Columns.Item(7).Visible = False
                gvPropuestas.Columns.Item(7).Visible = False
            End If
            'CargarClientes()
            CargarTipoBrief()
            'Validar()
            If Request.QueryString("filtrar") IsNot Nothing Then
                Dim filtrar As Int32 = Int32.Parse(Request.QueryString("filtrar").ToString())
				If filtrar = 1 Then
					CargarBriefsSinPropuesta()
				ElseIf filtrar = 2 Then
					CrearNuevo()
				Else
					'CargarBriefs()
				End If
            ElseIf ((Request.QueryString("crear") IsNot Nothing) And (btnNuevo.Visible = True)) Then
				CrearNuevo()
			Else
				'CargarBriefs()
			End If

			'gvDatos.DataSource = CargarBriefs()
			'gvDatos.DataBind()

		End If
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            Guardar()
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
            Log(hfidbrief.Value, 2)
			'Limpiar()
			CargarBriefs()
			accordion2.Visible = True
			'ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
			'Response.Redirect("Propuestas.aspx?IdBrief=" & hfidbrief.Value)
		Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
			'ActivateAccordion(1, EffectActivateAccordion.NoEffect)
		End Try
    End Sub

    Protected Sub btnGuardarYCrear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardarYCrear.Click

        If txtNombreCliente.Text = "" Then
			ShowNotification("Debe escribir el nombre del cliente", ShowNotifications.ErrorNotification)
			Exit Sub
		End If

        If txtNombreContacto.Text = "" Then
			ShowNotification("Debe escribir el nombre del contacto", ShowNotifications.ErrorNotification)
			Exit Sub
		End If

        If ddlTipoBrief.SelectedValue = "-1" Then
			ShowNotification("Debe escribir un tipo de brief", ShowNotifications.ErrorNotification)
			Exit Sub
		End If

        Try
            Guardar()
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
            Log(hfidbrief.Value, 2)
            Dim Viable As Boolean = True
            Dim oBrief As New Brief
            oBrief.ActualizarViabilidad(hfidbrief.Value, Viable, Session("IDUsuario").ToString)
            Response.Redirect("Propuestas.aspx?IdBrief=" & hfidbrief.Value)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
			'ActivateAccordion(1, EffectActivateAccordion.NoEffect)
		End Try
    End Sub
    'Protected Sub ddlcliente_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlcliente.DataBound
    '    DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    'End Sub
    'Protected Sub ddlcliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlcliente.SelectedIndexChanged
    '    Try
    '        If (ddlcliente.SelectedValue = "-1") Then
    '            ddlcontacto.Items.Clear()
    '            ddlcontacto.DataSource = Nothing
    '            ddlcontacto.DataBind()
    '            btnnuevocontacto.Visible = False
    '        Else
    '            btnnuevocontacto.Visible = True
    '            Dim ClienteId As Int64 = Int64.Parse(ddlcliente.SelectedValue)
    '            CargarContactos(ClienteId)
    '        End If
    '        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub
    'Protected Sub ddlcontacto_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlcontacto.DataBound
    '    DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    'End Sub
    Protected Sub ddlTipoBrief_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipoBrief.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        gvDatos.DataSource = CargarBriefs()
        gvDatos.DataBind()
    End Sub

	Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
		'Limpiar()
		'ActivateAccordion(1, EffectActivateAccordion.SlideEffect)

		CrearNuevo()
	End Sub

	Sub CrearNuevo()
		accordion1.Visible = True
		accordion0.Visible = False
		txtNombreCliente.Focus()
	End Sub
	Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        gvDatos.DataSource = CargarBriefs()
        gvDatos.DataBind()
    End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Modificar"
                    Dim idBrief As Int64 = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    'Limpiar()
                    Detalles(idBrief)
					CargarInfo(idBrief)
					accordion1.Visible = True
					accordion0.Visible = False
					'ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
					Exit Sub
                Case "Eliminar"
                    Dim idBrief As Int64 = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Eliminar(idBrief)
                    CargarBriefs()
                    ShowNotification("Registro Eliminado correctamente", ShowNotifications.InfoNotification)
                    Log(hfidbrief.Value, 4)
                Case "Detalles"
                    Dim idBrief As Int64 = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Limpiar()
                    Detalles(idBrief)
                Case "SI"
                    Dim Viable As Boolean = True
                    Dim idBrief As Int64 = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Dim oBrief As New Brief
                    oBrief.ActualizarViabilidad(idBrief, Viable, Session("IDUsuario").ToString)
                    CargarBriefs()
                Case "NO"
                    Dim Viable As Boolean = False
                    Dim idBrief As Int64 = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    hfidbriefnoviab.Value = idBrief
                    Dim oBrief As New Brief
                    oBrief.ActualizarViabilidad(idBrief, Viable, Session("IDUsuario").ToString)
                    CargarBriefs()
                    'EnviarEmail(idBrief)
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
			'ActivateAccordion(0, EffectActivateAccordion.NoEffect)
		End Try
    End Sub

    'Protected Sub btnnuevocliente_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnnuevocliente.Click
    '    Response.Redirect("Clientes.aspx")
    'End Sub

    'Protected Sub btnnuevocontacto_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnnuevocontacto.Click
    '    Response.Redirect("Contactos.aspx?idcliente=" & ddlcliente.SelectedValue)
    'End Sub

    Protected Sub gvPropuestas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPropuestas.PageIndexChanging
        Try
            gvPropuestas.PageIndex = e.NewPageIndex
            CargarPropuestas()
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
			'ActivateAccordion(2, EffectActivateAccordion.NoEffect)
		End Try
    End Sub


    Protected Sub gvPropuestas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPropuestas.RowCommand
        Try
            Select Case e.CommandName
                Case "Modificar"
                    Dim idPropuesta As Int64 = Int64.Parse(Me.gvPropuestas.DataKeys(CInt(e.CommandArgument))("Id"))
                    Response.Redirect("Propuestas.aspx?IdPropuesta=" & idPropuesta.ToString())
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
			'ActivateAccordion(2, EffectActivateAccordion.NoEffect)
		End Try
    End Sub
    Protected Sub bntcrearpropuesta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bntcrearpropuesta.Click
        Try
            If String.IsNullOrEmpty(hfidbrief.Value) Then
                Throw New Exception("Debe Elegir un brief para ir a la propuesta")
            End If
            Dim oBrief As New Brief
            Dim info = oBrief.DevolverxID(hfidbrief.Value)
            If Not (info.Viabilidad = True) Or IsNothing(info.Viabilidad) Then
                Throw New Exception("La viabilidad del brief debe definirse como viable antes de continuar")
            End If
            Response.Redirect("Propuestas.aspx?IdBrief=" & hfidbrief.Value)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
			'ActivateAccordion(0, EffectActivateAccordion.NoEffect)
		End Try
    End Sub

    Protected Sub btnfiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnfiltrar.Click
        CargarBriefsSinPropuesta()
    End Sub

    Protected Sub btnDocumentos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDocumentos.Click
        If hfidbrief.Value <> "" Then
            Response.Redirect("\GD_Documentos\GD_Documentos.aspx?IdContenedor=" & hfidbrief.Value & "&IdDocumento=1")
        End If
    End Sub


    Protected Sub btnFiltroSinViabilidad_Click(sender As Object, e As EventArgs) Handles btnFiltroSinViabilidad.Click
        CargarBriefsViabilidad(Nothing)
    End Sub

    Private Sub btnGuardarRazonViabilidad_Click(sender As Object, e As System.EventArgs) Handles btnGuardarRazonViabilidad.Click
        If Me.txtRazonNoViabilidad.Text = "" Then
            ShowNotification("No podrá continuar hasta que no escriba la razón de no viabilidad", ShowNotifications.ErrorNotification)
            Exit Sub
        End If
        Dim Viable As Boolean = False
        Dim idBrief As Int64 = hfidbriefnoviab.Value
        Dim oBrief As New Brief
        oBrief.ActualizarViabilidad(idBrief, Viable, Session("IDUsuario").ToString)
        oBrief.ActualizarRazonViabilidad(idBrief, txtRazonNoViabilidad.Text)
        CargarBriefs()
        'EnviarEmail(idBrief)
    End Sub

    Private Sub btnQuitarFiltro_Click(sender As Object, e As System.EventArgs) Handles btnQuitarFiltro.Click
        Me.txtBuscar.Text = ""
		CargarBriefs()
		accordion1.Visible = False
		accordion2.Visible = False
	End Sub
#End Region

#Region "Funciones y Metodos"
    Public Sub Limpiar()
        'btnnuevocontacto.Visible = False
        hfidbrief.Value = String.Empty
        'ddlcontacto.SelectedValue = "-1"
        txtNombreContacto.Text = ""
        txtNombreCliente.Text = ""
        ddlTipoBrief.SelectedValue = "-1"
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
        lbldetalleregistro.Text = String.Empty
        lblTextoViabilidad.Visible = False
        lblFechaViabilidad.Visible = False
        lblFechaViabilidad.InnerText = String.Empty
        pnlbotones.Visible = True
        gvPropuestas.Visible = True
    End Sub

    Function CargarBriefs() As List(Of CU_Brief_Get_Result)
        Dim oBusqueda As New Brief

        Dim Id As Int64? = Nothing
        Dim ContactoId As Int64? = Nothing
        Dim TodosCampos As String = Nothing

        If Not txtBuscar.Text = "" Then TodosCampos = txtBuscar.Text

        Return oBusqueda.ObtenerBriefs(Id, ContactoId, Session("IDUsuario").ToString, TodosCampos)
    End Function


    Public Sub CargarBriefsSinPropuesta()
        Try
            Dim oBrief As New Brief
            Dim listabriefs = (From lbrief In CargarBriefs()
                                  Select Id = lbrief.Id,
                                  Titulo = lbrief.Titulo,
                                  Nombre = lbrief.Nombre,
                                  NombreBrief = lbrief.NombreBrief,
                                  Objetivos = lbrief.Objetivos,
                                  Antecedentes = lbrief.Antecedentes,
                                  ActionStandars = lbrief.ActionStandars,
                                  TargetGroup = lbrief.TargetGroup,
                                  Metodologia = lbrief.Metodologia,
                                  Tiempos = lbrief.Tiempos,
                                  Presupuestos = lbrief.Presupuestos,
                                  ResultadosAnteriores = lbrief.ResultadosAnteriores,
                                  Materiales = lbrief.Materiales,
                                  FormatoInforme = lbrief.FormatoInforme,
                                  Aprobaciones = lbrief.Aprobaciones,
                                  Competencia = lbrief.Competencia,
                                  Viabilidad = lbrief.Viabilidad, RazonSocial = lbrief.RazonSocial, Propuesta = lbrief.Propuesta, GerenteCuentas = lbrief.GerenteCuentas Where Propuesta = False AndAlso Viabilidad = True AndAlso GerenteCuentas = Session("IDUsuario").ToString).ToList().OrderByDescending(Function(c) c.Id)

            If (Not String.IsNullOrEmpty(txtBuscar.Text)) Then
                gvDatos.DataSource = listabriefs.Where(Function(c) (c.Titulo.Contains(txtBuscar.Text.ToUpper)) Or (c.Nombre.ToUpper.Contains(txtBuscar.Text.ToUpper)) Or (c.NombreBrief.ToUpper.Contains(txtBuscar.Text.ToUpper))).ToList
            Else
                gvDatos.DataSource = listabriefs.ToList
            End If
            gvDatos.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub CargarBriefsViabilidad(ByVal viable As Boolean)
        If viable = True Then
            Try
                Dim oBrief As New Brief
                Dim listabriefs = (From lbrief In CargarBriefs()
                                      Select Id = lbrief.Id,
                                      Titulo = lbrief.Titulo,
                                      Nombre = lbrief.Nombre,
                                      NombreBrief = lbrief.NombreBrief,
                                      Objetivos = lbrief.Objetivos,
                                      Antecedentes = lbrief.Antecedentes,
                                      ActionStandars = lbrief.ActionStandars,
                                      TargetGroup = lbrief.TargetGroup,
                                      Metodologia = lbrief.Metodologia,
                                      Tiempos = lbrief.Tiempos,
                                      Presupuestos = lbrief.Presupuestos,
                                      ResultadosAnteriores = lbrief.ResultadosAnteriores,
                                      Materiales = lbrief.Materiales,
                                      FormatoInforme = lbrief.FormatoInforme,
                                      Aprobaciones = lbrief.Aprobaciones,
                                      Competencia = lbrief.Competencia,
                                      Viabilidad = lbrief.Viabilidad, RazonSocial = lbrief.RazonSocial, Propuesta = lbrief.Propuesta, GerenteCuentas = lbrief.GerenteCuentas Where Propuesta = False AndAlso GerenteCuentas = Session("IDUsuario").ToString And Viabilidad = True).ToList().OrderByDescending(Function(c) c.Id)
                gvDatos.DataSource = listabriefs.ToList
                gvDatos.DataBind()
            Catch ex As Exception
                Throw ex
            End Try
        End If
        If viable = False Then
            Try
                Dim oBrief As New Brief
                Dim listabriefs = (From lbrief In CargarBriefs()
                                      Select Id = lbrief.Id,
                                      Titulo = lbrief.Titulo,
                                      Nombre = lbrief.Nombre,
                                      NombreBrief = lbrief.NombreBrief,
                                      Objetivos = lbrief.Objetivos,
                                      Antecedentes = lbrief.Antecedentes,
                                      ActionStandars = lbrief.ActionStandars,
                                      TargetGroup = lbrief.TargetGroup,
                                      Metodologia = lbrief.Metodologia,
                                      Tiempos = lbrief.Tiempos,
                                      Presupuestos = lbrief.Presupuestos,
                                      ResultadosAnteriores = lbrief.ResultadosAnteriores,
                                      Materiales = lbrief.Materiales,
                                      FormatoInforme = lbrief.FormatoInforme,
                                      Aprobaciones = lbrief.Aprobaciones,
                                      Competencia = lbrief.Competencia,
                                      Viabilidad = lbrief.Viabilidad, RazonSocial = lbrief.RazonSocial, Propuesta = lbrief.Propuesta, GerenteCuentas = lbrief.GerenteCuentas Where Propuesta = False AndAlso GerenteCuentas = Session("IDUsuario").ToString And Viabilidad = False).ToList().OrderByDescending(Function(c) c.Id)
                gvDatos.DataSource = listabriefs.ToList
                gvDatos.DataBind()
            Catch ex As Exception
                Throw ex
            End Try
        End If
        If viable = Nothing Then
            Try
                Dim oBrief As New Brief
                Dim listabriefs = (From lbrief In CargarBriefs()
                                      Select Id = lbrief.Id,
                                      Titulo = lbrief.Titulo,
                                      Nombre = lbrief.Nombre,
                                      NombreBrief = lbrief.NombreBrief,
                                      Objetivos = lbrief.Objetivos,
                                      Antecedentes = lbrief.Antecedentes,
                                      ActionStandars = lbrief.ActionStandars,
                                      TargetGroup = lbrief.TargetGroup,
                                      Metodologia = lbrief.Metodologia,
                                      Tiempos = lbrief.Tiempos,
                                      Presupuestos = lbrief.Presupuestos,
                                      ResultadosAnteriores = lbrief.ResultadosAnteriores,
                                      Materiales = lbrief.Materiales,
                                      FormatoInforme = lbrief.FormatoInforme,
                                      Aprobaciones = lbrief.Aprobaciones,
                                      Competencia = lbrief.Competencia,
                                      Viabilidad = lbrief.Viabilidad, RazonSocial = lbrief.RazonSocial, Propuesta = lbrief.Propuesta, GerenteCuentas = lbrief.GerenteCuentas Where Propuesta = False AndAlso GerenteCuentas = Session("IDUsuario").ToString And Viabilidad Is Nothing).ToList().OrderByDescending(Function(c) c.Id)
                gvDatos.DataSource = listabriefs.ToList
                gvDatos.DataBind()
            Catch ex As Exception
                Throw ex
            End Try
        End If
    End Sub
    'Public Sub CargarClientes()
    '    Try

    '        Dim oCliente As New Cliente
    '        Dim listaCliente = (From lCliente In oCliente.DevolverTodos()
    '                                                   Select Id = lCliente.Id,
    '                                                    RazonSocial = lCliente.RazonSocial).ToList().OrderBy(Function(c) c.RazonSocial).Distinct()
    '        ddlcliente.DataSource = listaCliente
    '        ddlcliente.DataTextField = "RazonSocial"
    '        ddlcliente.DataValueField = "Id"
    '        ddlcliente.DataBind()

    '        ddlcontacto.DataSource = Nothing
    '        ddlcontacto.DataBind()

    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub
    'Public Sub CargarContactos(ByVal ClienteID As Int64)
    '    Try
    '        Dim oContacto As New Contacto
    '        Dim listacontactos = (From lcontacto In oContacto.DevolverxClienteID(ClienteID)
    '                              Select Id = lcontacto.Id,
    '                              Nombre = lcontacto.Nombre).ToList().OrderBy(Function(c) c.Nombre)

    '        ddlcontacto.DataSource = listacontactos.ToList()
    '        ddlcontacto.DataTextField = "Nombre"
    '        ddlcontacto.DataValueField = "Id"
    '        ddlcontacto.DataBind()
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub
    Public Sub CargarTipoBrief()
        Try
            Dim oBrief As New Brief
            Dim listatipobrief = (From ltipo In oBrief.DevolverTiposBrief
                                  Select Id = ltipo.ID,
                                  tipobrief = ltipo.tipobrief).ToList().OrderBy(Function(c) c.tipobrief)

            ddlTipoBrief.DataSource = listatipobrief.ToList()
            ddlTipoBrief.DataTextField = "tipobrief"
            ddlTipoBrief.DataValueField = "Id"
            ddlTipoBrief.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarInfo(ByVal idbrief As Int64)
        Try
            Dim oBrief As New Brief
            Dim info = oBrief.ObtenerBriefXID(idbrief)
			hfidbrief.Value = idbrief
			txtNombreCliente.Text = info.Cliente
			'ddlcliente.SelectedValue = info.ClienteId
			'CargarContactos(info.ClienteId)
			'btnnuevocontacto.Visible = True
			'ddlcontacto.SelectedValue = info.ContactoId
			txtNombreContacto.Text = info.Contacto
			ddlTipoBrief.SelectedValue = info.TipoBrief
			'ddlcliente.SelectedValue = info.ClienteId
			'CargarContactos(info.ClienteId)
			'btnnuevocontacto.Visible = True
			'ddlcontacto.SelectedValue = info.ContactoId
			'txtNombreCliente.Text = info.RazonSocial
			'ddlcliente.SelectedValue = info.ClienteId
			'CargarContactos(info.ClienteId)
			'btnnuevocontacto.Visible = True
			'ddlcontacto.SelectedValue = info.ContactoId
			'txtNombreContacto.Text = info.Nombre
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
            ddlUnidades.SelectedValue = info.Unidad

            If info.FechaViabilidad IsNot Nothing Then
                lblTextoViabilidad.Visible = True
                lblFechaViabilidad.Visible = True
                lblFechaViabilidad.InnerText = info.FechaViabilidad

            Else
                lblTextoViabilidad.Visible = False
                lblFechaViabilidad.Visible = False
                lblFechaViabilidad.InnerText = ""
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

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Detalles(ByVal idbrief As Int64)
        Try
            hfidbrief.Value = idbrief
            Dim oBrief As New Brief
            Dim info = oBrief.DevolverxID(idbrief)
            Dim strDetalle As New StringBuilder
            'txtNombreClienteDetalle.Text = info.RazonSocial
            'txtNombreContactoDetalle.Text = info.Nombre
            'strDetalle.AppendLine("<br/><b>Antecedentes y Problema de Marketing: </b> ")
            'strDetalle.AppendLine("<br/>" & info.Antecedentes)
            'strDetalle.AppendLine("<br/>")
            'strDetalle.AppendLine("<br/><b>Objetivos de la Investigación: </b> ")
            'strDetalle.AppendLine("<br/>" & info.Objetivos)
            'strDetalle.AppendLine("<br/>")
            'lbldetalleregistro.Text = strDetalle.ToString()
            CargarPropuestas()
            'ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
        Catch ex As Exception
            Limpiar()
            Throw ex
        End Try
    End Sub
    Public Sub CargarPropuestas()
        Try
            Dim idBrief As Int64 = Int64.Parse(hfidbrief.Value)
            Dim oPropuesta As New Propuesta
            Dim listapropuestas = (From lprop In oPropuesta.DevolverxBriefID(idBrief)
                                  Select Id = lprop.Id,
                                  Titulo = lprop.Titulo, TipoPropuesta = lprop.TipoPropuesta,
                                  Probabilidad = lprop.Probabilidad, FechaEnvio = lprop.FechaEnvio,
                                  Estado = lprop.Estado, OrigenProp = lprop.OrigenProp,
                                  FechaAprob = lprop.FechaAprob, Razon = lprop.Razon, FormaEnvio = lprop.FormaEnvio,
                                    NombreBrief = lprop.NombreBrief, Tracking = lprop.Tracking, Internacional = lprop.internacional, RazonSocial = lprop.RazonSocial
                                  ).OrderBy(Function(p) p.Titulo).ToList()

            'If listapropuestas.Count > 0 Then
            '    pnlbotones.Visible = False
            '    gvPropuestas.Visible = True
            gvPropuestas.DataSource = listapropuestas.ToList()
            gvPropuestas.DataBind()
			'Else
			'    gvPropuestas.Visible = False
			'    pnlbotones.Visible = True
			'End If
			'ActivateAccordion(2, EffectActivateAccordion.NoEffect)

		Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Guardar()
        Try
            Dim idBrief As Int64
            Dim oBrief As New Brief
            Dim Antecedentes = "", Objetivos = "", ActionStandar = "", Metodologia = "", TargetGroup = "", Tiempos = "", Presupuestos = "", Materiales As String = ""
            Dim EstudiosAnteriores = "", Formatos = "", Aprobaciones = "", Competencia As String = ""

            If txtNombreCliente.Text = "" Then
                Throw New Exception("Debe escribir el nombre del cliente")
            End If

            If txtNombreContacto.Text = "" Then
                Throw New Exception("Debe escribir el nombre del contacto")
            End If

            If ddlTipoBrief.SelectedValue = "-1" Then
                Throw New Exception("Debe elegir un tipo de brief")
            End If


            If Not String.IsNullOrEmpty(hfidbrief.Value) Then
                idBrief = Int64.Parse(hfidbrief.Value)
            End If

            If Not String.IsNullOrEmpty(txtAntecedentes.Text) Then
                Antecedentes = txtAntecedentes.Text
            End If

            If Not String.IsNullOrEmpty(txtObjetivos.Text) Then
                Objetivos = txtObjetivos.Text
            End If

            If Not String.IsNullOrEmpty(txtActionStandard.Text) Then
                ActionStandar = txtActionStandard.Text
            End If

            If Not String.IsNullOrEmpty(txtMetodologia.Text) Then
                Metodologia = txtMetodologia.Text
            End If

            If Not String.IsNullOrEmpty(txtTargetGroup.Text) Then
                TargetGroup = txtTargetGroup.Text
            End If

            If Not String.IsNullOrEmpty(txtTiempos.Text) Then
                Tiempos = txtTiempos.Text
            End If

            If Not String.IsNullOrEmpty(txtPresupuesto.Text) Then
                Presupuestos = txtPresupuesto.Text
            End If

            If Not String.IsNullOrEmpty(txtMateriales.Text) Then
                Materiales = txtMateriales.Text
            End If

            If Not String.IsNullOrEmpty(txtEstudiosAnteriores.Text) Then
                EstudiosAnteriores = txtEstudiosAnteriores.Text
            End If

            If Not String.IsNullOrEmpty(txtFormatos.Text) Then
                Formatos = txtFormatos.Text
            End If

            If Not String.IsNullOrEmpty(txtAprobaciones.Text) Then
                Aprobaciones = txtAprobaciones.Text
            End If

            If Not String.IsNullOrEmpty(txtCompetencia.Text) Then
                Competencia = txtCompetencia.Text
            End If

            'Dim ent As New CU_Entities
            Dim ent As New CU_Brief
			If (idBrief > 0) Then
				ent = oBrief.ObtenerBriefXID(idBrief)
			End If
			ent.Id = idBrief
            ent.Titulo = txtTitulo.Text.Trim()
            ent.Contacto = txtNombreContacto.Text.Trim
            ent.Cliente = txtNombreCliente.Text.Trim
            ent.TipoBrief = ddlTipoBrief.SelectedValue
            ent.Antecedentes = Antecedentes
            ent.Objetivos = Objetivos
            ent.ActionStandars = ActionStandar
            ent.Metodologia = Metodologia
            ent.TargetGroup = TargetGroup
            ent.Tiempos = Tiempos
            ent.Presupuestos = Presupuestos
            ent.Materiales = Materiales
            ent.ResultadosAnteriores = EstudiosAnteriores
            ent.FormatoInforme = Formatos
            ent.Aprobaciones = Aprobaciones
            ent.Competencia = Competencia
            ent.GerenteCuentas = Session("IDUsuario").ToString()
            ent.Unidad = ddlUnidades.SelectedValue

			ent.O1 = txtO1.Text
            ent.O2 = txtO2.Text
            ent.O3 = txtO3.Text
            ent.O4 = txtO4.Text
            ent.O5 = txtO5.Text
            ent.O6 = txtO6.Text
            ent.O7 = txtO7.Text
            ent.D1 = txtD1.Text
            ent.D2 = txtD2.Text
            ent.D3 = txtD3.Text
            ent.C1 = txtC1.Text
            ent.C2 = txtC2.Text
            ent.C3 = txtC3.Text
            ent.C4 = txtC4.Text
            ent.C5 = txtC5.Text
            ent.M1 = txtM1.Text
            ent.M2 = txtM2.Text
            ent.M3 = txtM3.Text
            ent.DI1 = txtDI1.Text
            ent.DI2 = txtDI2.Text
            ent.DI3 = txtDI3.Text
            ent.DI4 = txtDI4.Text
            ent.DI5 = txtDI5.Text
            ent.DI6 = txtDI6.Text
            ent.DI7 = txtDI7.Text
            ent.DI8 = txtDI8.Text
            ent.DI9 = txtDI9.Text
            ent.DI10 = txtDI10.Text
            ent.DI11 = txtDI11.Text
            ent.DI12 = txtDI12.Text
            ent.DI13 = txtDI13.Text
            ent.DI14 = txtDI14.Text
            ent.DI15 = txtDI15.Text
            ent.DI16 = txtDI16.Text
            ent.DI17 = txtDI17.Text
            ent.DI18 = txtDI18.Text
			hfidbrief.Value = oBrief.GuardarBrief(ent)
			'hfidbrief.Value = oBrief.Guardar(idBrief, txtTitulo.Text.Trim, ddlcontacto.SelectedValue, ddlTipoBrief.SelectedValue, Antecedentes, Objetivos, ActionStandar, Metodologia, TargetGroup, Tiempos, Presupuestos, Materiales, EstudiosAnteriores, Formatos, Aprobaciones, Competencia, Session("IDUsuario").ToString, ddlUnidades.SelectedValue)

		Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Eliminar(ByVal idbrief As Int64)
        Try
            Dim oBrief As New Brief
            oBrief.Eliminar(idbrief)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub EnviarEmail(ByVal idBrief As Int32)
        Try
            ''Info Brief
            Dim oBrief As New Brief
            Dim infobrief = oBrief.DevolverxID(idBrief)

            ''Info Propuesta
            Dim oPropuesta As New Propuesta
            ''Dim infopropuesta = oPropuesta.DevolverxBriefID(infobrief.Id)
            Dim otexto As New StringBuilder
            Dim destinatarios As New List(Of String)
            'otexto.AppendLine("<br/> <p> <img alt='' src='../Images/logo-titulo.png' width='217px' height='52px' /></p>")
            otexto.AppendLine("<br/>Bogotá, " & Day(Now.Date).ToString() & " de " & MonthName(Now.Month) & " de " & Now.Year)
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>Doctor")
            otexto.AppendLine("<br/><b>" & infobrief.Nombre & "</b>")
            otexto.AppendLine("<br/>La Ciudad")
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>Estimado(a) Señor(a):")
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>Agradecemos su amable invitación para presentar una propuesta sobre ")
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>Desafortunadamente nuestros actuales compromisos con clientes permanentes comprometen los recursos disponibles en diseño, ejecución y análisis, y nos impiden, en estos momentos, atender a su amable solicitud.")
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>Confiando en un futuro construir una conjunta y benéfica relación,")
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>Reciban un atento saludo,")
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>Nombre Gerente de Cuentas")
            otexto.AppendLine("<br/>CCCCCCC")
            otexto.AppendLine("<br/>Ipsos-Napoleón Franco")
            otexto.AppendLine("<br/>Calle 74 No. 11 – 81 Piso 5")
            otexto.AppendLine("<br/>Tels: 3769400 – Fax 525")
            otexto.AppendLine("<br/>E-mail: nfranco@ipsos.com.co - www.ipsos.com.co")

            Dim oEnviarCorreo As New EnviarCorreo
            oEnviarCorreo.sendMail(destinatarios, "prueba", otexto.ToString)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Validar()
        Try

            If Session("IDUsuario") IsNot Nothing Then
                Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
                Dim oValidacion As New Validacion
                If Not oValidacion.ValidarPermiso("Brief", UsuarioID) Then
                    btnNuevo.Visible = False
                    btnGuardar.Enabled = False
                    bntcrearpropuesta.Enabled = False
                    gvDatos.Columns.Item(4).Visible = False
                    gvDatos.Columns.Item(6).Visible = False
                    gvDatos.Columns.Item(7).Visible = False
                    gvPropuestas.Columns.Item(7).Visible = False
                Else
                    btnNuevo.Visible = True
                    btnGuardar.Enabled = True
                    bntcrearpropuesta.Enabled = True
                    gvDatos.Columns.Item(4).Visible = True
                    gvDatos.Columns.Item(6).Visible = True
                    gvDatos.Columns.Item(7).Visible = True
                    gvPropuestas.Columns.Item(7).Visible = True
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub Log(ByVal iddoc As Int64?, ByVal idaccion As Int64)
        Dim log As New LogEjecucion
        log.Guardar(3, iddoc, Now(), Session("IDUsuario"), idaccion)
    End Sub

    Private Sub CargarUnidades()
        Dim oUnidades As New CoreProject.US.Unidades

        'ddlUnidades.DataSource = oUnidades.ObtenerUnidadCombo
        ddlUnidades.DataSource = oUnidades.ObtenerUnidadesxUsuario(Session("IDUsuario").ToString)
        ddlUnidades.DataTextField = "Unidad"
        ddlUnidades.DataValueField = "id"
        ddlUnidades.DataBind()


    End Sub

#End Region


End Class