Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Public Class Contactos
    Inherits System.Web.UI.Page
#Region " Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(18, UsuarioID) = False Then
                btnNuevo.Visible = False
                gvDatos.Columns.Item(6).Visible = False
                btnGuardar.Visible = False
            End If
            If Request.QueryString("idcliente") IsNot Nothing Then
                Dim idcliente As Int64 = Int64.Parse(Request.QueryString("idcliente").ToString)
                hfidcliente.Value = idcliente
                CargarContactos()
            Else
                Response.Redirect("Clientes.aspx")
            End If
        End If
    End Sub
    Protected Sub lnkCliente_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCliente.Click
        Response.Redirect("Clientes.aspx?idcliente=" & hfidcliente.Value.ToString())
    End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Modificar"
                    Dim idContacto As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Cargarinfo(idContacto)
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        CargarContactos()
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            GuardarContacto()
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
            log(hfidcliente.Value, 2)
            limpiar()
            CargarContactos()
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)

        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End Try
    End Sub

    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        limpiar()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        txtNombre.Focus()
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            CargarContactos()
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
#End Region
#Region "Funciones y Metodos"
    Public Sub CargarContactos()
        Try
            Dim idcliente As Int64 = Int64.Parse(hfidcliente.Value)
            Dim ocliente As New Cliente
            Dim ocontacto As New Contacto
            Dim lcliente = ocliente.DevolverxID(idcliente)
            lnkCliente.Text = lcliente.RazonSocial.ToUpper
            Dim listacontactos = (From lcontacto In ocontacto.DevolverxClienteID(idcliente)
                                  Select Id = lcontacto.Id,
                                  Nombre = lcontacto.Nombre, Telefono = lcontacto.Telefono,
                                  Celular = lcontacto.Celular, Email = lcontacto.Email, Cargo = lcontacto.Cargo,
                                  Activo = lcontacto.Activo, Clienteid = lcontacto.ClienteId
                                 ).ToList().OrderBy(Function(c) c.Nombre)
            If Not String.IsNullOrEmpty(txtBuscar.Text) Then
                gvDatos.DataSource = listacontactos.Where(Function(c) (c.Nombre.ToUpper.Contains(txtBuscar.Text.ToUpper))).ToList
            Else
                gvDatos.DataSource = listacontactos.ToList()
            End If
            gvDatos.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Cargarinfo(ByVal idcontacto As Int64)
        Try
            Dim oContacto As New Contacto
            Dim info = oContacto.DevolverxID(idcontacto)
            hfidcontacto.Value = idcontacto
            txtNombre.Text = info.Nombre
            txtTelefono.Text = info.Telefono
            txtCelular.Text = info.Celular
            txtCargo.Text = info.Cargo
            txtEmail.Text = info.Email
            chkActivo.Checked = info.Activo
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub GuardarContacto()
        Try
            Dim oContacto As New Contacto
            Dim idContacto As Int64

            If Not String.IsNullOrEmpty(hfidcontacto.Value) Then
                idContacto = Int64.Parse(hfidcontacto.Value)
            End If

            oContacto.Guardar(idContacto, txtNombre.Text, txtTelefono.Text, txtCelular.Text, txtEmail.Text,
                              txtCargo.Text, chkActivo.Checked, Int64.Parse(hfidcliente.Value))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub limpiar()
        hfidcontacto.Value = String.Empty
        txtNombre.Text = String.Empty
        txtTelefono.Text = String.Empty
        txtCelular.Text = String.Empty
        txtEmail.Text = String.Empty
        txtCargo.Text = String.Empty
        chkActivo.Checked = True
    End Sub
    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Dim log As New LogEjecucion
        log.Guardar(2, iddoc, Now(), Session("IDUsuario"), idaccion)

    End Sub
#End Region
End Class