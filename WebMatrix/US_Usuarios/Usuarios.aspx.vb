Imports CoreProject
Imports Utilidades.Encripcion
Imports WebMatrix.Util


Public Class Usuarios
    Inherits System.Web.UI.Page

    Private _Identificacion As Int64
    Public Property Indentificacion() As Int64
        Get
            Return _Identificacion
        End Get
        Set(ByVal value As Int64)
            _Identificacion = value
        End Set
    End Property


    Public Function ConsultarUsuariosXNombre(ByVal Nombre As String) As List(Of Usuarios_Result)
        Dim Data As New Datos.ClsPermisosUsuarios
        Dim Info As List(Of Usuarios_Result)
        Try
            Info = Data.ConsultarUsuariosXNombre(Nombre)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ConsultarUsuariosXUnidades(ByVal UnidadId As Integer) As List(Of Usuarios_Result)
        Dim Data As New Datos.ClsPermisosUsuarios
        Dim Info As List(Of Usuarios_Result)
        Try
            Info = Data.ConsultarUsuariosXUnidades(UnidadId)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ConsultarUsuariosXGrupoUnidad(ByVal GrupoUnidadId As Integer) As List(Of Usuarios_Result)
        Dim Data As New Datos.ClsPermisosUsuarios
        Dim Info As List(Of Usuarios_Result)
        Try
            Info = Data.ConsultarUsuariosXGrupoUnidad(GrupoUnidadId)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ConsultarUsuariosXRolesUsuarios(ByVal RolId As Integer) As List(Of Usuarios_Result)
        Dim Data As New Datos.ClsPermisosUsuarios
        Dim Info As List(Of Usuarios_Result)
        Try
            Info = Data.ConsultarUsuariosXRolesUsuarios(RolId)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarGrid(ByVal Opcion As Integer)
        Select Case Opcion
            Case 1
                Session("ListaCargada") = ConsultarUsuariosXNombre(txtBuscar.Text)
                gvDatos.DataSource = Session("ListaCargada")
                gvDatos.DataBind()
        End Select

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(88, UsuarioID) = False Then
            Response.Redirect("../TH_TalentoHumano/Default.aspx")
        End If
        If Not Request.QueryString("Identificacion") Is Nothing Then
            Int64.TryParse(Request.QueryString("Identificacion"), Indentificacion)
            If Indentificacion > 0 Then
                cargarDatosPreviosUsuario(Indentificacion)
                ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            End If
        End If

        If Not Page.IsPostBack Then
            CargarGrid(1)
        End If
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        CargarGrid(1)
    End Sub

    Sub Limpiar()
        txtId.Text = String.Empty
        txtNombres.Text = String.Empty
        txtApellidos.Text = String.Empty
        txtEmail.Text = String.Empty
        txtUsuario.Text = String.Empty
        chkActivo.Checked = False
    End Sub

    Function Validar() As Boolean
        Dim val As Boolean = True
        If txtId.Text = String.Empty Then
            lblResult.Text = "Especifique la identificación"
            val = False
        End If
        If txtNombres.Text = String.Empty Then
            lblResult.Text = "Especifique los nombres"
            val = False
        End If
        If txtApellidos.Text = String.Empty Then
            lblResult.Text = "Especifique los apellidos"
            val = False
        End If
        If txtEmail.Text = String.Empty Then
            lblResult.Text = "Especifique el correo del usuario"
            val = False
        End If
        If txtUsuario.Text = String.Empty Then
            lblResult.Text = "Especifique el alias de usuario"
            val = False
        End If
        If txtContraseña.Text = String.Empty And btnEditar.Visible = False Then
            lblResult.Text = "Especifique la contraseña del usuario"
            val = False
        End If
        Return val
    End Function



    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        datos.Visible = True
        btnEditar.Visible = False
        btnGuardar.Visible = True
        lista.Visible = False
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        If Validar() = True Then
            Dim password As String = ""
            password = Cifrado(1, 2, Me.txtContraseña.Text, "Ipsos*23432_2013", "Ipsos*23432_2013")
            Dim US As New US_Entities
            US.US_Usuarios_Add(txtId.Text, txtUsuario.Text, password, txtNombres.Text, txtApellidos.Text, txtEmail.Text, chkActivo.Checked)
            Dim oEnviarCorreo As New EnviarCorreo
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/NuevoUsuarioCreado.aspx?idUsuario=" & txtId.Text)
            ShowNotification("Usuario creado correctamente", ShowNotifications.InfoNotification)
            log(txtId.Text, 2)
            Limpiar()
            CargarGrid(1)
            lista.Visible = True
            datos.Visible = False
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        End If
    End Sub

    Protected Sub btnEditar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditar.Click
        If Validar() = True Then
            Dim US As New US_Entities
            Dim password As String = ""
            If Trim(txtContraseña.Text) <> "" Then
                password = Cifrado(1, 2, Me.txtContraseña.Text, "Ipsos*23432_2013", "Ipsos*23432_2013")
            End If

            US.US_Usuarios_Edit(txtId.Text, txtUsuario.Text, If(String.IsNullOrEmpty(password), Nothing, password), txtNombres.Text, txtApellidos.Text, txtEmail.Text, chkActivo.Checked)
            ShowNotification("Usuario editado correctamente", ShowNotifications.InfoNotification)
            log(txtId.Text, 3)
            Limpiar()
            CargarGrid(1)
            lista.Visible = True
            datos.Visible = False
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        End If
    End Sub

    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        gvDatos.DataSource = Session("ListaCargada")
        gvDatos.DataBind()
    End Sub

    Sub cargarDatosPreviosUsuario(ByVal id As Int64)
        Dim oPersonas As New Personas
        Dim oeTH_PErsonas As TH_Personas_Get_Result
        oeTH_PErsonas = oPersonas.DevolverxID(id)

        txtId.Text = id
        txtNombres.Text = oeTH_PErsonas.Nombres
        txtApellidos.Text = oeTH_PErsonas.Apellidos

        datos.Visible = True
        btnEditar.Visible = False
        btnGuardar.Visible = True
        lista.Visible = False

    End Sub
    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New LogEjecucion
            log.Guardar(31, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


#Region "Eventos"
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Dim usuario As Long
            If Long.TryParse(e.CommandArgument, usuario) Then
                hfIdUsuario.Value = gvDatos.DataKeys(CInt(e.CommandArgument))("Id")
            End If
            Select Case e.CommandName
                Case "Editar"
                    Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Session("UserId") = Id
                    txtId.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("Id")
                    txtNombres.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("Nombres")
                    txtApellidos.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("Apellidos")
                    txtEmail.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("Email")
                    txtUsuario.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("Usuario")
                    chkActivo.Checked = gvDatos.DataKeys(CInt(e.CommandArgument))("Activo")
                    datos.Visible = True
                    btnEditar.Visible = True
                    btnGuardar.Visible = False
                    lista.Visible = False
                Case "Eliminar"
                    Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Dim US As New US_Entities
                    US.US_Usuarios_Del(Id)
                    CargarGrid(1)
                Case "UsuariosUnidades"
                    CargarCombo(ddlTipoGrupoUnidad, "Id", "TipoGrupoUnidad", ObtenerTipoGrupoUnidad)
                    ddlGrupoUnidad.Items.Clear()

                    ddlUnidad.Items.Clear()
                    CargarGridUnidades(gvDatos.DataKeys(e.CommandArgument)("Id"))
                    upUnidadesAsignadas.Update()
                Case "Permisos"
                    cargarGridPermisosAsignados()
                    CargarPermisosDisponibles()
                    upPermisos.Update()
                Case "Roles"
                    cargarGridRolesAsignados()
                    CargarRolesDisponibles()
                    upRolesAsignados.Update()
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub ddlTipoGrupoUnidad_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipoGrupoUnidad.SelectedIndexChanged
        CargarCombo(ddlGrupoUnidad, "Id", "GrupoUnidad", ObtenerGrupoUnidad(ddlTipoGrupoUnidad.SelectedValue))
        ddlUnidad.Items.Clear()
        upUnidadesAsignadas.Update()
    End Sub
    Protected Sub ddlGrupoUnidad_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlGrupoUnidad.SelectedIndexChanged
        CargarCombo(ddlUnidad, "Id", "Unidad", ObtenerUnidad(ddlGrupoUnidad.SelectedValue, False, hfIdUsuario.Value))
        upUnidadesAsignadas.Update()
    End Sub

    Private Sub gvRolesAsignados_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvRolesAsignados.PageIndexChanging
        gvRolesAsignados.PageIndex = e.NewPageIndex
        cargarGridRolesAsignados()
        upRolesAsignados.Update()
    End Sub
    Private Sub gvRolesAsignados_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvRolesAsignados.RowCommand
        If e.CommandName = "Eliminar" Then
            Dim oRolesUsuarios As New US.RolesUsuarios
            oRolesUsuarios.eliminar(gvRolesAsignados.DataKeys(e.CommandArgument)("UsuarioId"), gvRolesAsignados.DataKeys(e.CommandArgument)("RolId"))
            cargarGridRolesAsignados()
            CargarRolesDisponibles()
            ShowNotification("Se elimino correctamente el rol", ShowNotifications.InfoNotification)
            upRolesAsignados.Update()
        End If
    End Sub
    Private Sub gvPermisosAsignados_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPermisosAsignados.RowCommand
        If e.CommandName = "Eliminar" Then
            Dim oPermisosUsuarios As New US.PermisosUsuarios
            oPermisosUsuarios.eliminar(gvPermisosAsignados.DataKeys(e.CommandArgument)("UsuarioId"), gvPermisosAsignados.DataKeys(e.CommandArgument)("PermisoId"))
            cargarGridPermisosAsignados()
            CargarPermisosDisponibles()
            ShowNotification("Se elimino correctamente el permiso", ShowNotifications.InfoNotification)
            upPermisos.Update()
        End If
    End Sub
    Private Sub gvUnidadesAsignadas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvUnidadesAsignadas.PageIndexChanging
        gvUnidadesAsignadas.PageIndex = e.NewPageIndex
        CargarGridUnidades(hfIdUsuario.Value)
        upUnidadesAsignadas.Update()
    End Sub
    Private Sub gvUnidadesAsignadas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvUnidadesAsignadas.RowCommand
        If e.CommandName = "Eliminar" Then
            Dim oUsuariosUnidades As New US.UsuariosUnidades
            oUsuariosUnidades.eliminar(gvUnidadesAsignadas.DataKeys(e.CommandArgument)("UsuarioId"), gvUnidadesAsignadas.DataKeys(e.CommandArgument)("UnidadId"))
            CargarGridUnidades(hfIdUsuario.Value)
            ddlGrupoUnidad.Items.Clear()
            ddlUnidad.Items.Clear()
            ShowNotification("Se elimino correctamente la unidad", ShowNotifications.InfoNotification)
            upUnidadesAsignadas.Update()
        End If
    End Sub
    Protected Sub btnGrabar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGrabar.Click
        Dim oUsuariosUnidades As New US.UsuariosUnidades
        oUsuariosUnidades.GuardarUsuariosUnidades(hfIdUsuario.Value, ddlUnidad.SelectedValue)
        CargarGridUnidades(hfIdUsuario.Value)
        CargarCombo(ddlUnidad, "Id", "Unidad", ObtenerUnidad(ddlGrupoUnidad.SelectedValue, False, hfIdUsuario.Value))
        ShowNotification("Se agrego correctamente la unidad al usuario", ShowNotifications.InfoNotification)
        upUnidadesAsignadas.Update()
    End Sub

    Private Sub gvPermisosAsignados_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPermisosAsignados.PageIndexChanging
        gvPermisosAsignados.PageIndex = e.NewPageIndex
        cargarGridPermisosAsignados()
        upPermisos.Update()
    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click
        Dim oPermisosUsuarios As New US.PermisosUsuarios
        oPermisosUsuarios.GuardarPermisosUsuario(hfIdUsuario.Value, ddlPermisos.SelectedValue)
        cargarGridPermisosAsignados()
        CargarPermisosDisponibles()
        upPermisos.Update()
    End Sub

    Private Sub btnAgregarRol_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregarRol.Click
        Dim oRolesUsuarios As New US.RolesUsuarios
        oRolesUsuarios.GuardarRolesUsuarios(hfIdUsuario.Value, ddlRolesDisponibles.SelectedValue)
        cargarGridRolesAsignados()
        CargarRolesDisponibles()
        upRolesAsignados.Update()
    End Sub
#End Region
#Region "Metodos"
    Sub CargarCombo(ByVal cmb As DropDownList, ByVal dvf As String, ByVal dtf As String, ByVal ds As Object)
        cmb.ClearSelection()
        cmb.DataValueField = dvf
        cmb.DataTextField = dtf
        cmb.DataSource = ds
        cmb.DataBind()

        cmb.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})

    End Sub
    Public Function ObtenerTipoGrupoUnidad() As List(Of TipoGrupoUnidadCombo_Result)
        Dim Data As New US.TipoGrupoUnidad
        Try
            Return Data.ObtenerTipoGrupoUnidadCombo()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerGrupoUnidad(ByVal TipoGrupo As Integer) As List(Of GrupoUnidadCombo_Result)
        Dim Data As New US.GrupoUnidad
        Try
            Return Data.ObtenerGrupoUnidadCombo(TipoGrupo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerUnidad(ByVal Grupo As Integer, ByVal asignado As Boolean, ByVal usuario As Int64) As List(Of US_UnidadesXUsuario_Get_Result)
        Dim Data As New US.UsuariosUnidades
        Return Data.obtenerUnidadesXUsuario(usuario, asignado, Grupo)
    End Function
    Sub CargarGridUnidades(ByVal id As Int64)
        Dim Data As New US.UsuariosUnidades
        gvUnidadesAsignadas.DataSource = Data.ObtenerUsuariosUnidades(id)
        gvUnidadesAsignadas.DataBind()
    End Sub
    Sub cargarGridPermisosAsignados()
        Dim oPermisosUsuarios As New US.PermisosUsuarios
        gvPermisosAsignados.DataSource = oPermisosUsuarios.obtenerPermisosXUsuario(hfIdUsuario.Value, True)
        gvPermisosAsignados.DataBind()
    End Sub
    Sub CargarPermisosDisponibles()
        Dim oPermisosUsuarios As New US.PermisosUsuarios
        ddlPermisos.DataSource = oPermisosUsuarios.obtenerPermisosXUsuario(hfIdUsuario.Value, False)
        ddlPermisos.DataValueField = "Id"
        ddlPermisos.DataTextField = "Permiso"
        ddlPermisos.DataBind()
        ddlPermisos.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub
    Sub cargarGridRolesAsignados()
        Dim oPermisosUsuarios As New US.RolesUsuarios
        gvRolesAsignados.DataSource = oPermisosUsuarios.obtenerRolesXUsuario(hfIdUsuario.Value, True)
        gvRolesAsignados.DataBind()
    End Sub
    Sub CargarRolesDisponibles()
        Dim oRolesUsuarios As New US.RolesUsuarios
        ddlRolesDisponibles.DataSource = oRolesUsuarios.obtenerRolesXUsuario(hfIdUsuario.Value, False)
        ddlRolesDisponibles.DataValueField = "Id"
        ddlRolesDisponibles.DataTextField = "Rol"
        ddlRolesDisponibles.DataBind()
        ddlRolesDisponibles.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub
#End Region


End Class