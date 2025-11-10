Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Public Class Clientes
    Inherits System.Web.UI.Page

#Region " Eventos del Control"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(11, UsuarioID) = False Then
                btnNuevo.Visible = False
                btnGuardar.Enabled = False
                gvDatos.Columns.Item(7).Visible = False
            End If
            CargarClientes()
            cargarPaises()
            cargarsectores()
            cargartipocliente()

            If Request.QueryString("idcliente") IsNot Nothing Then
                Dim idcliente As Int64 = Int64.Parse(Request.QueryString("idcliente").ToString)
                hfidcliente.Value = idcliente
                Cargarinfo(idcliente)
            Else
                ddldepartamento.Items.Clear()
                ddldepartamento.DataSource = Nothing
                ddldepartamento.DataBind()
                ddlCiudad.Items.Clear()
                ddlCiudad.DataSource = Nothing
                ddlCiudad.DataBind()
            End If
        End If
    End Sub
    Protected Sub ddlpais_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlpais.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub ddldepartamento_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddldepartamento.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub ddlCiudad_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCiudad.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub ddlTipoCliente_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipoCliente.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub ddlSector_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlSector.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        Limpiar()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        txtNit.Focus()
        'Log(hfidcliente.Value, 2)
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            CargarClientes()
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            If Not (CInt(txtAnticipo.Text) + CInt(txtSaldo.Text) = 100) Then
                ShowNotification("El anticipo más el saldo debe sumar 100", ShowNotifications.ErrorNotification)
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Exit Sub
            End If
            GuardarCliente()
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
            Log(hfidcliente.Value, 2)
            Limpiar()
            CargarClientes()
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)

        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub ddlpais_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlpais.SelectedIndexChanged
        If (ddlpais.SelectedValue <> "-1") Then
            Dim idPais As Int32 = Int32.Parse(ddlpais.SelectedValue)
            CargarDepartamentos(idPais)
        Else
            ddldepartamento.Items.Clear()
            ddldepartamento.DataSource = Nothing
            ddldepartamento.DataBind()
            ddlCiudad.Items.Clear()
            ddlCiudad.DataSource = Nothing
            ddlCiudad.DataBind()
        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub
    Protected Sub ddldepartamento_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddldepartamento.SelectedIndexChanged
        If (ddldepartamento.SelectedValue <> "-1") Then
            Dim iddpto As Int32 = Int32.Parse(ddldepartamento.SelectedValue)
            cargarciudades(iddpto)
        Else
            ddlCiudad.Items.Clear()
            ddlCiudad.DataSource = Nothing
            ddlCiudad.DataBind()
        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Modificar"
                    Dim idCliente As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Cargarinfo(idCliente)
                Case "Contactos"
                    Dim idCliente As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Response.Redirect("Contactos.aspx?idcliente=" & idCliente.ToString)
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try

    End Sub
    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        CargarClientes()
    End Sub


#End Region

#Region "Funciones y Metodos"
    Public Sub cargarPaises()
        Try
            Dim oAuxiliar As New Auxiliares
            Dim listaPaises = (From lpaises In oAuxiliar.DevolverPaises()
                               Select id = lpaises.PaiPais, pais = lpaises.PaiNombre).ToList().OrderBy(Function(p) p.pais)
            ddlpais.DataSource = listaPaises
            ddlpais.DataTextField = "pais"
            ddlpais.DataValueField = "id"
            ddlpais.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarDepartamentos(ByVal idPais As Int32)
        Dim oCoordCampo As New CoordinacionCampo
        Dim list = (From ldep In oCoordCampo.ObtenerDepartamentos()
                  Select iddep = ldep.DivDeptoMunicipio, nomdep = ldep.DivDeptoNombre, pais = ldep.Pais).Where(Function(x) x.pais = idPais).Distinct.ToList
        ddldepartamento.DataSource = list
        ddldepartamento.DataValueField = "iddep"
        ddldepartamento.DataTextField = "nomdep"
        ddldepartamento.DataBind()
        'Try
        '    Dim oAuxiliar As New Auxiliares
        '    Dim listaDepartamentos = (From ldpto In oAuxiliar.DevolverDepartamentos(idPais)
        '                              Select id = ldpto.DivDepto, dpto = ldpto.DivDeptoNombre).ToList().OrderBy(Function(d) d.dpto)
        '    ddldepartamento.DataSource = listaDepartamentos
        '    ddldepartamento.DataValueField = "id"
        '    ddldepartamento.DataTextField = "dpto"
        '    ddldepartamento.DataBind()

        'Catch ex As Exception
        '    Throw ex
        'End Try
    End Sub
    Public Sub cargarciudades(ByVal iddpto As Int32)
        Dim oCoordCampo As New CoordinacionCampo
        Dim listciudades = (From ldep In oCoordCampo.ObtenerCiudades(ddldepartamento.SelectedValue)
                  Select idmuni = ldep.DivMunicipio, nommuni = ldep.DivMuniNombre)
        ddlCiudad.DataSource = listciudades
        ddlCiudad.DataValueField = "idmuni"
        ddlCiudad.DataTextField = "nommuni"
        ddlCiudad.DataBind()
        'Try
        '    Dim oAuxiliar As New Auxiliares
        '    Dim listaCiudades = (From lciudad In oAuxiliar.DevolverCiudades(iddpto)
        '                         Select id = lciudad.DivDeptoMunicipio, idciudad = lciudad.DivMunicipio, ciudad = lciudad.DivMuniNombre).ToList().OrderBy(Function(c) c.ciudad)
        '    ddlCiudad.DataSource = listaCiudades
        '    ddlCiudad.DataValueField = "id"
        '    ddlCiudad.DataTextField = "ciudad"
        '    ddlCiudad.DataBind()

        'Catch ex As Exception
        '    Throw ex
        'End Try
    End Sub
    Public Sub cargartipocliente()
        Try
            Dim oAuxiliar As New Auxiliares
            Dim listatipo = (From ltipo In oAuxiliar.DevolverTipoCliente()
                             Select id = ltipo.Id, tipo = ltipo.tipocliente).ToList().OrderBy(Function(t) t.tipo)
            ddlTipoCliente.DataSource = listatipo
            ddlTipoCliente.DataValueField = "id"
            ddlTipoCliente.DataTextField = "tipo"
            ddlTipoCliente.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub cargarsectores()
        Try
            Dim oAuxiliar As New Auxiliares
            Dim listasectores = (From lsector In oAuxiliar.DevolverSectores()
                                  Select id = lsector.id, sector = lsector.sector).ToList().OrderBy(Function(s) s.sector)
            ddlSector.DataSource = listasectores
            ddlSector.DataValueField = "id"
            ddlSector.DataTextField = "sector"
            ddlSector.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarClientes()
        Try

            Dim oCliente As New Cliente
            Dim listaCliente = (From lCliente In oCliente.DevolverTodos()
                                                       Select Id = lCliente.Id, Nit = lCliente.Nit,
                                                       GrupoEconomico = lCliente.GrupoEconomico, RazonSocial = lCliente.RazonSocial,
                                                       idCiudad = lCliente.Ciudad, AliasCliente = lCliente.Alias,
                                                       idTipoCliente = lCliente.TipoCliente, FechaGrabacion = lCliente.FechaGrabacion,
                                                       obsFacturacion = lCliente.ObsFacturacion, Direccion = lCliente.Direccion,
                                                       Telefono = lCliente.Telefono, SectorId = lCliente.SectorId,
                                                       dpto = lCliente.DivDeptoNombre, ciudad = lCliente.DivMuniNombre, idpais = lCliente.Pais).ToList().OrderBy(Function(c) c.RazonSocial)

            If (Not String.IsNullOrEmpty(txtBuscar.Text)) Then
                gvDatos.DataSource = listaCliente.Where(Function(c) (c.RazonSocial.ToUpper.Contains(txtBuscar.Text.ToUpper))).ToList
            Else
                gvDatos.DataSource = listaCliente.ToList
            End If
            gvDatos.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Cargarinfo(ByVal idCliente As Int32)
        Try
            Dim oCliente As New Cliente
            Dim info = oCliente.DevolverxID(idCliente)
            hfidcliente.Value = idCliente
            txtNit.Text = info.Nit
            TxtRazonSocial.Text = info.RazonSocial
            txtGrupoEconomico.Text = info.GrupoEconomico
            txtDireccion.Text = info.Direccion
            txtTelefono.Text = info.Telefono
            txtAlias.Text = info.Alias
            ddlpais.SelectedValue = info.Pais
            CargarDepartamentos(info.Pais)
            ddldepartamento.SelectedValue = info.DivDepto
            cargarciudades(info.DivDepto)
            ddlCiudad.SelectedValue = info.Ciudad
            ddlTipoCliente.SelectedValue = info.id_tipo_cliente
            ddlSector.SelectedValue = info.SectorId
            txtAnticipo.Text = info.Anticipo
            txtSaldo.Text = String.Empty
            txtPlazo.Text = String.Empty
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub GuardarCliente()
        Try
            Dim oCliente As New Cliente
            Dim idCliente As Int64

            If Not String.IsNullOrEmpty(hfidcliente.Value) Then
                idCliente = Int64.Parse(hfidcliente.Value)
            End If

            oCliente.Guardar(idCliente, Decimal.Parse(txtNit.Text), txtGrupoEconomico.Text, TxtRazonSocial.Text, Int32.Parse(ddlCiudad.SelectedValue),
                                txtAlias.Text, Int32.Parse(ddlTipoCliente.SelectedValue), txtDireccion.Text, txtTelefono.Text, ddlSector.SelectedValue, txtAnticipo.Text, txtSaldo.Text, txtPlazo.Text)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Limpiar()
        txtAlias.Text = String.Empty
        txtBuscar.Text = String.Empty
        txtDireccion.Text = String.Empty
        txtGrupoEconomico.Text = String.Empty
        txtNit.Text = String.Empty
        TxtRazonSocial.Text = String.Empty
        txtTelefono.Text = String.Empty
        'txtAnticipo.Text = String.Empty
        'txtSaldo.Text = String.Empty
        'txtPlazo.Text = String.Empty
        hfidcliente.Value = String.Empty
        cargarPaises()
        ddldepartamento.Items.Clear()
        ddldepartamento.DataSource = Nothing
        ddldepartamento.DataBind()
        ddlCiudad.Items.Clear()
        ddlCiudad.DataSource = Nothing
        ddlCiudad.DataBind()
    End Sub

    Public Sub Log(ByVal iddoc As Int64?, ByVal idaccion As Int64)
        Dim log As New LogEjecucion
        log.Guardar(1, iddoc, Now(), Session("IDUsuario"), idaccion)
    End Sub
#End Region


    
End Class