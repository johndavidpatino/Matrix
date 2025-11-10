Imports WebMatrix
Imports CoreProject
Imports WebMatrix.Util


Public Class Contratacion
    Inherits System.Web.UI.Page


#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(92, UsuarioID) = False Then
            Response.Redirect("../TH_TalentoHumano/Default.aspx")
        End If
        If Request.QueryString("coordinador") IsNot Nothing Then
            hfCoordinador.Value = "1"
            btnCrearUsuario.Visible = False
        Else
            hfCoordinador.Value = "0"
        End If
        hfCoordinador.Value = "1"
        If Not IsPostBack Then

            cargarCargos()
            cargarCiudades()
            'cargarTiposContratacion()
            cargarTiposEncuestadores()
            cargarPersonas()
            cargarContratistas()
        End If
    End Sub
    Protected Sub ddlCargos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCargos.SelectedIndexChanged
        If ddlCargos.SelectedValue = Cargos.TiposCargos.Encuestador Then
            ddlTipoEncuestador.Enabled = True
        Else
            ddlTipoEncuestador.Enabled = False
        End If
        ddlTipoEncuestador.SelectedValue = -1
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub
    Protected Sub btnGrabar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGrabar.Click

        If ddlCargos.SelectedValue = Cargos.TiposCargos.Encuestador Then
            If ddlContratista.SelectedValue = "-1" Then
                ShowNotification("Elija el contratista antes de continuar", ShowNotifications.ErrorNotification)
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Exit Sub
            End If
        End If
        Dim oPersonas As New Personas
        Dim password As String = ""
        Dim flag As Boolean = True
        Dim contratista As Int64? = Nothing
        Dim estadoanterior As Int32 = 0
        If Not (ddlContratista.SelectedValue = "-1") Then
            contratista = ddlContratista.SelectedValue
        End If
        'Dim ent As New CoreProject.TH_Personas2  Revisar Ajuste Cambio
        Dim ent As New TH_Personas2
        Dim o As New CoreProject.RegistroPersonas
        Dim Ctr As New Contratista
        ent = o.ObtenerPersona(txtIdentificacion.Text)
        If ent Is Nothing Then
            flag = False
        Else
            If ent.EstadoActual Is Nothing Then
                estadoanterior = 1
            Else
                estadoanterior = ent.EstadoActual
            End If
        End If
        If Not (ddlCargos.SelectedValue = Cargos.TiposCargos.Encuestador) Then o.EliminarEncuestador(txtIdentificacion.Text)
        oPersonas.Grabar(txtIdentificacion.Text, txtNombres.Text, txtApellidos.Text, ddlCargos.SelectedValue, ddlCiudad.SelectedValue, ddlTipoContratacion.SelectedValue, txtFechaIngreso.Text, txtFechaNacimiento.Text, chkActivo.Checked, If(ddlCargos.SelectedValue = Cargos.TiposCargos.Encuestador, True, False), ddlTipoEncuestador.SelectedValue, contratista)
        If chkActivo.Checked = False Then
            ent = o.ObtenerPersona(txtIdentificacion.Text)

            If flag = True Then
                If Not (estadoanterior = 2) Then
                    ent.FechaRetiro = Date.UtcNow.AddHours(-5).Date
                End If
            End If
            ent.EstadoActual = 2
            o.GuardarPersona(ent, False)
            Ctr.ActualizarEstadoPersonasContratista(txtIdentificacion.Text)
            Ctr.LogPersonasAddContratistas(txtIdentificacion.Text, Session("IDUsuario"))
        End If

        cargarPersonas()
        log(txtIdentificacion.Text, 2)
        limpiar()
        ShowNotification("Persona creada correctamente", ShowNotifications.InfoNotification)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        cargarPersonas()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        limpiar()
    End Sub

    Private Sub gvContratacion_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvContratacion.PageIndexChanging
        gvContratacion.PageIndex = e.NewPageIndex
        cargarPersonas()
    End Sub
    Private Sub gvContratacion_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvContratacion.RowCommand
        If e.CommandName = "Actualizar" Then
            hfId.Value = gvContratacion.DataKeys(e.CommandArgument)("Id")
            cargarPersona(gvContratacion.DataKeys(e.CommandArgument)("Id"))
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            log(hfId.Value, 3)
        End If
    End Sub
#End Region
#Region "Metodos"
    Sub cargarCargos()
        Dim oCargos As New Cargos
        If hfCoordinador.Value = 0 Then
            ddlCargos.DataSource = oCargos.DevolverTodos
            ddlCargos.DataTextField = "Cargo"
            ddlCargos.DataValueField = "Id"
            ddlCargos.DataBind()
        Else
            ddlCargos.Items.Clear()
            Dim li As New ListItem
            li.Text = "Supervisor"
            li.Value = "12"
            Dim li2 As New ListItem
            li2.Text = "Encuestador"
            li2.Value = "13"
            ddlCargos.Items.Add(li)
            ddlCargos.Items.Add(li2)
        End If
        ddlCargos.Items.Insert(0, New ListItem With {.Value = -1, .Text = "--Seleccione--"})
    End Sub
    Sub cargarCiudades()
        Dim oDivipola As New Divipola
        'HACK Deuda tecnica, deberiamos colocar los paises en un enumerado para evitar números magicos
        Dim o As New CoreProject.RegistroPersonas
        Me.ddlCiudad.DataSource = o.CiudadesList
        Me.ddlCiudad.DataValueField = "id"
        Me.ddlCiudad.DataTextField = "Ciudad"
        Me.ddlCiudad.DataBind()
        Me.ddlCiudad.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub
    Sub cargarTiposContratacion()
        Dim oTipoContratacion As New TH.TipoContratacion
        ddlTipoContratacion.DataSource = oTipoContratacion.obtenerTodos()
        ddlTipoContratacion.DataTextField = "Tipo"
        ddlTipoContratacion.DataValueField = "Id"
        ddlTipoContratacion.DataBind()
        ddlTipoContratacion.Items.Insert(0, New ListItem With {.Value = -1, .Text = "--Seleccione--"})
    End Sub
    Sub cargarTiposEncuestadores()
        Dim o As New TipoEncuestador
        ddlTipoEncuestador.DataSource = o.obtenerTodos
        ddlTipoEncuestador.DataTextField = "Tipo"
        ddlTipoEncuestador.DataValueField = "Id"
        ddlTipoEncuestador.DataBind()
        ddlTipoEncuestador.Items.Insert(0, New ListItem With {.Value = -1, .Text = "--Seleccione--"})
    End Sub
    Sub cargarContratistas()
        Dim o As New RegistroPersonas
        ddlContratista.DataSource = o.ContratistasList
        ddlContratista.DataTextField = "Nombre"
        ddlContratista.DataValueField = "Identificacion"
        ddlContratista.DataBind()
        ddlContratista.Items.Insert(0, New ListItem With {.Value = -1, .Text = "--Seleccione--"})
    End Sub
    Sub cargarPersonas()
        Dim o As New CoreProject.RegistroPersonas
        Dim cedula As Int64? = Nothing
        Dim nombre As String = Nothing
        If IsNumeric(txtCedulaBuscar.Text) Then cedula = txtCedulaBuscar.Text
        If Not (txtNombreBuscar.Text = "") Then nombre = txtNombreBuscar.Text
        Me.gvContratacion.DataSource = o.TH_PersonasContratistas(cedula, nombre)
        Me.gvContratacion.DataBind()
    End Sub
    Sub limpiar()
        hfId.Value = ""
        txtIdentificacion.Text = ""
        txtNombres.Text = ""
        txtApellidos.Text = ""
        ddlCargos.SelectedValue = -1
        ddlTipoEncuestador.SelectedValue = -1
        ddlCiudad.SelectedValue = -1
        'ddlTipoContratacion.SelectedValue = -1
        txtFechaIngreso.Text = ""
        txtFechaNacimiento.Text = ""
        chkActivo.Checked = True
    End Sub
    Sub cargarPersona(ByVal id As Int64)
        Dim oPersona As New Personas
        Dim oePersona As New TH_Personas_Get_Result
        Dim oEncuestadores As New Encuestadores
        oePersona = oPersona.DevolverxID(id)
        txtIdentificacion.Text = oePersona.id
        txtNombres.Text = oePersona.Nombres
        txtApellidos.Text = oePersona.Apellidos
        ddlCargos.SelectedValue = oePersona.Cargo
        Try
            ddlTipoEncuestador.SelectedValue = If(oePersona.Cargo = Cargos.TiposCargos.Encuestador, oEncuestadores.obtenerXId(id).TipoId, -1)
        Catch ex As Exception
            ddlTipoEncuestador.SelectedValue = -1
        End Try
        ddlTipoEncuestador.Enabled = If(oePersona.Cargo = Cargos.TiposCargos.Encuestador, True, False)
        ddlCiudad.SelectedValue = oePersona.CiudadId
        ddlTipoContratacion.SelectedValue = oePersona.TipoContratacion
        If oePersona.FechaIngreso Is Nothing Then
            txtFechaIngreso.Text = ""
        Else
            txtFechaIngreso.Text = oePersona.FechaIngreso
        End If

        If IsDate(oePersona.FechaNacimiento) Then txtFechaNacimiento.Text = oePersona.FechaNacimiento
        chkActivo.Checked = oePersona.Activo
        Try

        Catch ex As Exception

        End Try
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub
    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New LogEjecucion
            log.Guardar(33, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

    Protected Sub btnCrearUsuario_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCrearUsuario.Click
        If Not String.IsNullOrEmpty(hfId.Value) Then
            Response.Redirect("\US_Usuarios\Usuarios.aspx?Identificacion=" & hfId.Value)
        Else
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End If
    End Sub

    Private Sub ddlTipoContratacion_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTipoContratacion.SelectedIndexChanged
        If ddlTipoContratacion.SelectedValue = "4" Then
            Me.ddlContratista.Enabled = True
        Else
            Me.ddlContratista.Enabled = False
            Me.ddlContratista.SelectedValue = -1
        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

End Class