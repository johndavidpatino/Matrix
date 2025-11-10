Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Imports ClosedXML.Excel
Imports CoreProject.OP
Imports CoreProject.INV
Imports System.IO
Imports Newtonsoft.Json
Imports DevExpress.Security

Public Class AsignacionActivosFijos
    Inherits System.Web.UI.Page


#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarActivosFijos()
            CargarActivosFijosAS()
            CargarEstadoTablet()
            CargarEstadoTabletAS()
            CargarPerifericos()
            EstadosArticulos()
            CargarSedes()
            CargarBU()
            CargarBUAS()
            CargarCiudades()
            CargarCiudadesAS()
            CargarCombo(ddlTipoGrupoUnidad, "Id", "TipoGrupoUnidad", ObtenerTipoGrupoUnidad)
            ddlGrupoUnidad.Items.Clear()
            ddlUnidad.Items.Clear()

        End If

        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExportar)
        smanager.RegisterPostBackControl(Me.btnExportarAS)

    End Sub

    Private Sub gvArticulos_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvArticulos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If CType(e.Row.DataItem, INV_RegistroArticulos_Get_Result).IdEstado = 1 Then
                DirectCast(e.Row.FindControl("imgBtnAsignar"), ImageButton).Visible = True
            Else
                DirectCast(e.Row.FindControl("imgBtnAsignar"), ImageButton).Visible = False
            End If
        End If

    End Sub


    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Private Sub gvArticulos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvArticulos.PageIndexChanging
        gvArticulos.PageIndex = e.NewPageIndex
        CargarColumnas()
        gvArticulos.DataSource = obtenerRegistrosArticulos()
        gvArticulos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub gvAsignaciones_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAsignaciones.PageIndexChanging
        gvAsignaciones.PageIndex = e.NewPageIndex
        CargarColumnasAS()
        gvAsignaciones.DataSource = ObtenerAsignaciones()
        gvAsignaciones.DataBind()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub gvArticulos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvArticulos.RowCommand

        Dim lstRegistroArticulos As List(Of INV_RegistroArticulos_Get_Result)

        If e.CommandName = "Asignar" Then

            lstRegistroArticulos = ObtenerRegistroArticulos(gvArticulos.DataKeys(e.CommandArgument).Value)

            lblIdAsignar.Text = lstRegistroArticulos(0).Id
            lblAsignar.Visible = True
            lblIdAsignar.Visible = True
            lblArticulo.Visible = True

            If lstRegistroArticulos(0).IdArticulo = 1 Then
                lblArticulo.Text = lstRegistroArticulos(0).Articulo & " - " & lstRegistroArticulos(0).TipoComputador & "- Nombre Equipo:" & lstRegistroArticulos(0).NombreEquipo
            ElseIf lstRegistroArticulos(0).IdArticulo = 2 Then
                lblArticulo.Text = lstRegistroArticulos(0).Articulo & " - " & lstRegistroArticulos(0).TipoServidor
            ElseIf lstRegistroArticulos(0).IdArticulo = 3 Then
                lblArticulo.Text = lstRegistroArticulos(0).Articulo & " - " & lstRegistroArticulos(0).TipoPeriferico
            ElseIf lstRegistroArticulos(0).IdArticulo = 4 Then
                lblArticulo.Text = lstRegistroArticulos(0).Articulo & " - Id Tablet: " & lstRegistroArticulos(0).IdTablet
            ElseIf lstRegistroArticulos(0).IdArticulo = 5 Or lstRegistroArticulos(0).IdArticulo = 6 Or lstRegistroArticulos(0).IdArticulo = 10 Then
                lblArticulo.Text = lstRegistroArticulos(0).Articulo
            End If

            If lstRegistroArticulos(0).IdArticulo = 4 Then
                lblCentroCosto.Visible = True
                ddlCentroCosto.Visible = True
                lblEstadoTablet.Visible = True
                ddlEstadoTablet.Visible = True
            Else
                lblCentroCosto.Visible = False
                ddlCentroCosto.Visible = False
                ddlCentroCosto.ClearSelection()
                lblBU.Visible = False
                ddlBU.Visible = False
                ddlBU.ClearSelection()
                lblJBIJBE.Visible = False
                txtJBIJBE.Visible = False
                txtJBIJBE.Text = ""
                btnSearchJBEJBICC.Visible = False
                lblNombreJBIJBE.Visible = False
                txtNombreJBIJBE.Visible = False
                txtNombreJBIJBE.Text = ""
                lblEstadoTablet.Visible = False
                ddlEstadoTablet.Visible = False
            End If

            Dim lstAsignaciones As List(Of INV_Asignaciones_Get_Result)
            lstAsignaciones = ObtenerAsignaciones(lstRegistroArticulos(0).Id)

            'Default values 
            txtFechaAsignado.Text = Date.UtcNow.AddHours(-5).ToShortDateString()

            If lstAsignaciones.Count > 0 And lstRegistroArticulos(0).IdAsignado = True Then

                txtFechaAsignado.Text = lstAsignaciones(0).FechaAsignacion

                If lstAsignaciones(0).IdCentroCosto IsNot Nothing Then
                    ddlCentroCosto.SelectedValue = lstAsignaciones(0).IdCentroCosto
                End If

                If lstAsignaciones(0).IdBU IsNot Nothing Then
                    ddlBU.SelectedValue = lstAsignaciones(0).IdBU
                End If

                If lstAsignaciones(0).JobBook IsNot Nothing Then
                    hfIdTrabajo.Value = lstAsignaciones(0).JobBook
                End If

                If lstAsignaciones(0).JobBookCodigo IsNot Nothing Then
                    txtJBIJBE.Text = lstAsignaciones(0).JobBookCodigo
                End If

                If lstAsignaciones(0).JobBookNombre IsNot Nothing Then
                    txtNombreJBIJBE.Text = lstAsignaciones(0).JobBookNombre
                End If

                ddlCiudad.SelectedValue = lstAsignaciones(0).IdCiudad

                If lstAsignaciones(0).IdCiudad = 11001 Then
                    lblSede.Visible = True
                    ddlSede.Visible = True
                Else
                    lblSede.Visible = False
                    ddlSede.Visible = False
                    ddlSede.ClearSelection()
                End If

                If lstAsignaciones(0).IdSede IsNot Nothing Then
                    ddlSede.SelectedValue = lstAsignaciones(0).IdSede
                End If

                If lstAsignaciones(0).IdTipoGrupoUnidad IsNot Nothing Then
                    ddlTipoGrupoUnidad.SelectedValue = lstAsignaciones(0).IdTipoGrupoUnidad
                End If

                If lstAsignaciones(0).IdGrupoUnidad IsNot Nothing Then
                    Dim GrupoUnidades = ObtenerGrupoUnidad(ddlTipoGrupoUnidad.SelectedValue)
                    CargarCombo(ddlGrupoUnidad, "Id", "GrupoUnidad", GrupoUnidades)
                    For Each grupo In GrupoUnidades
                        If ddlGrupoUnidad.SelectedValue = grupo.id Then
                            ddlGrupoUnidad.SelectedValue = lstAsignaciones(0).IdGrupoUnidad
                        End If
                    Next
                End If

                If lstAsignaciones(0).IdUnidad IsNot Nothing Then
                    Dim Unidades = ObtenerUnidad(ddlGrupoUnidad.SelectedValue, False, hfIdUsuario.Value)
                    CargarCombo(ddlUnidad, "Id", "Unidad", Unidades)
                    For Each unidad In Unidades
                        If ddlUnidad.SelectedValue = unidad.id Then
                            ddlUnidad.SelectedValue = lstAsignaciones(0).IdUnidad
                        End If
                    Next
                End If

                If lstAsignaciones(0).IdEstadoTablet IsNot Nothing Then
                    ddlEstadoTablet.SelectedValue = lstAsignaciones(0).IdEstadoTablet
                End If

                hfIdUsuario.Value = lstAsignaciones(0).IdUsuarioAsignado
                txtUsuario.Text = lstAsignaciones(0).UsuarioAsignado
                hfIdCargo.Value = lstAsignaciones(0).IdTipoCargo
                txtCargo.Text = lstAsignaciones(0).Cargo
                txtObservacion.Text = lstAsignaciones(0).Observacion

                If lstRegistroArticulos(0).IdArticulo = 4 Then
                    lblCentroCosto.Visible = True
                    ddlCentroCosto.Visible = True

                    If ddlCentroCosto.SelectedValue = 3 Then
                        lblBU.Visible = True
                        ddlBU.Visible = True
                        lblJBIJBE.Visible = False
                        txtJBIJBE.Visible = False
                        txtJBIJBE.Text = ""
                        btnSearchJBEJBICC.Visible = False
                        lblNombreJBIJBE.Visible = False
                        txtNombreJBIJBE.Visible = False
                        txtNombreJBIJBE.Text = ""
                    Else
                        lblJBIJBE.Visible = True
                        txtJBIJBE.Visible = True
                        btnSearchJBEJBICC.Visible = True
                        lblNombreJBIJBE.Visible = True
                        txtNombreJBIJBE.Visible = True
                        lblBU.Visible = False
                        ddlBU.Visible = False
                        ddlBU.ClearSelection()
                    End If

                    lblEstadoTablet.Visible = True
                    ddlEstadoTablet.Visible = True
                Else
                    lblCentroCosto.Visible = False
                    ddlCentroCosto.Visible = False
                    lblBU.Visible = False
                    ddlBU.Visible = False
                    ddlBU.ClearSelection()
                    lblJBIJBE.Visible = False
                    txtJBIJBE.Visible = False
                    txtJBIJBE.Text = ""
                    btnSearchJBEJBICC.Visible = False
                    lblNombreJBIJBE.Visible = False
                    txtNombreJBIJBE.Visible = False
                    txtNombreJBIJBE.Text = ""
                    lblEstadoTablet.Visible = False
                    ddlEstadoTablet.Visible = False
                End If

                btnDesasignar.Visible = True

            End If

            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)

        End If

    End Sub

    Private Sub gvAsignaciones_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAsignaciones.RowCommand

        If e.CommandName = "Actualizar" Then

            Dim lstAsignaciones As List(Of INV_Asignaciones_Get_Result)
            lstAsignaciones = ObtenerAsignaciones(gvAsignaciones.DataKeys(e.CommandArgument)("IdActivoFijo"))

            lblIdAsignar.Text = lstAsignaciones(0).IdActivoFijo
            lblAsignar.Visible = True
            lblIdAsignar.Visible = True
            lblArticulo.Visible = True

            If lstAsignaciones(0).IdArticulo = 1 Then
                lblArticulo.Text = lstAsignaciones(0).Articulo & " - " & lstAsignaciones(0).TipoComputador & "- Nombre Equipo:" & lstAsignaciones(0).NombreEquipo
            ElseIf lstAsignaciones(0).IdArticulo = 2 Then
                lblArticulo.Text = lstAsignaciones(0).Articulo
            ElseIf lstAsignaciones(0).IdArticulo = 3 Then
                lblArticulo.Text = lstAsignaciones(0).Articulo & " - " & lstAsignaciones(0).TipoPeriferico
            ElseIf lstAsignaciones(0).IdArticulo = 4 Then
                lblArticulo.Text = lstAsignaciones(0).Articulo & " - Id Tablet: " & lstAsignaciones(0).IdTablet
            ElseIf lstAsignaciones(0).IdArticulo = 5 Or lstAsignaciones(0).IdArticulo = 6 Or lstAsignaciones(0).IdArticulo = 10 Then
                lblArticulo.Text = lstAsignaciones(0).Articulo
            End If

            If lstAsignaciones(0).FechaAsignacion IsNot Nothing Then
                txtFechaAsignado.Text = lstAsignaciones(0).FechaAsignacion
            End If

            If lstAsignaciones(0).IdCentroCosto IsNot Nothing Then
                ddlCentroCosto.SelectedValue = lstAsignaciones(0).IdCentroCosto
            End If

            If lstAsignaciones(0).IdBU IsNot Nothing Then
                ddlBU.SelectedValue = lstAsignaciones(0).IdBU
            End If

            If lstAsignaciones(0).JobBook IsNot Nothing Then
                hfIdTrabajo.Value = lstAsignaciones(0).JobBook
            End If

            If lstAsignaciones(0).JobBookCodigo IsNot Nothing Then
                txtJBIJBE.Text = lstAsignaciones(0).JobBookCodigo
            End If

            If lstAsignaciones(0).JobBookNombre IsNot Nothing Then
                txtNombreJBIJBE.Text = lstAsignaciones(0).JobBookNombre
            End If

            If lstAsignaciones(0).IdCiudad IsNot Nothing Then
                ddlCiudad.SelectedValue = lstAsignaciones(0).IdCiudad
            End If

            If lstAsignaciones(0).IdCiudad = 11001 Then
                lblSede.Visible = True
                ddlSede.Visible = True
            Else
                lblSede.Visible = False
                ddlSede.Visible = False
                ddlSede.ClearSelection()
            End If

            If lstAsignaciones(0).IdSede IsNot Nothing Then
                ddlSede.SelectedValue = lstAsignaciones(0).IdSede
            End If

            If lstAsignaciones(0).IdTipoGrupoUnidad IsNot Nothing Then
                ddlTipoGrupoUnidad.SelectedValue = lstAsignaciones(0).IdTipoGrupoUnidad
            End If

            If lstAsignaciones(0).IdGrupoUnidad IsNot Nothing Then
                CargarCombo(ddlGrupoUnidad, "Id", "GrupoUnidad", ObtenerGrupoUnidad(ddlTipoGrupoUnidad.SelectedValue))
                ddlGrupoUnidad.SelectedValue = lstAsignaciones(0).IdGrupoUnidad
            End If

            If lstAsignaciones(0).IdUnidad IsNot Nothing Then
                CargarCombo(ddlUnidad, "Id", "Unidad", ObtenerUnidad(ddlGrupoUnidad.SelectedValue, False, hfIdUsuario.Value))
                ddlUnidad.SelectedValue = lstAsignaciones(0).IdUnidad
            End If

            If lstAsignaciones(0).IdEstadoTablet IsNot Nothing Then
                ddlEstadoTablet.SelectedValue = lstAsignaciones(0).IdEstadoTablet
            End If

            If lstAsignaciones(0).IdUsuarioAsignado IsNot Nothing Then
                hfIdUsuario.Value = lstAsignaciones(0).IdUsuarioAsignado
            End If

            If lstAsignaciones(0).UsuarioAsignado IsNot Nothing Then
                txtUsuario.Text = lstAsignaciones(0).UsuarioAsignado
            End If

            If lstAsignaciones(0).IdTipoCargo IsNot Nothing Then
                hfIdCargo.Value = lstAsignaciones(0).IdTipoCargo
            End If

            If lstAsignaciones(0).Cargo IsNot Nothing Then
                txtCargo.Text = lstAsignaciones(0).Cargo
            End If

            txtObservacion.Text = lstAsignaciones(0).Observacion

            If lstAsignaciones(0).IdArticulo = 4 Then
                lblCentroCosto.Visible = True
                ddlCentroCosto.Visible = True

                If ddlCentroCosto.SelectedValue = 3 Then
                    lblBU.Visible = True
                    ddlBU.Visible = True
                    lblJBIJBE.Visible = False
                    txtJBIJBE.Visible = False
                    txtJBIJBE.Text = ""
                    btnSearchJBEJBICC.Visible = False
                    lblNombreJBIJBE.Visible = False
                    txtNombreJBIJBE.Visible = False
                    txtNombreJBIJBE.Text = ""
                Else
                    lblJBIJBE.Visible = True
                    txtJBIJBE.Visible = True
                    btnSearchJBEJBICC.Visible = True
                    lblNombreJBIJBE.Visible = True
                    txtNombreJBIJBE.Visible = True
                    lblBU.Visible = False
                    ddlBU.Visible = False
                    ddlBU.ClearSelection()
                End If

                lblEstadoTablet.Visible = True
                ddlEstadoTablet.Visible = True
            Else
                lblCentroCosto.Visible = False
                ddlCentroCosto.Visible = False
                lblBU.Visible = False
                ddlBU.Visible = False
                ddlBU.ClearSelection()
                lblJBIJBE.Visible = False
                txtJBIJBE.Visible = False
                txtJBIJBE.Text = ""
                btnSearchJBEJBICC.Visible = False
                lblNombreJBIJBE.Visible = False
                txtNombreJBIJBE.Visible = False
                txtNombreJBIJBE.Text = ""
                lblEstadoTablet.Visible = False
                ddlEstadoTablet.Visible = False
            End If

            btnDesasignar.Visible = True

            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        End If

        If e.CommandName = "OpenPDF" Then
            Dim lstRegistroArticulos As List(Of INV_RegistroArticulos_Get_Result)
            lstRegistroArticulos = ObtenerRegistroArticulos(gvAsignaciones.DataKeys(e.CommandArgument)("IdActivoFijo"))

            Dim currentItem = lstRegistroArticulos(0)

            Dim buildPDFParams = New ConstanciaAsignacionParams With {
                .fecha = currentItem.FechaAsignacion,
                .marcaActivo = currentItem.Marca,
                .modeloActivo = currentItem.Modelo,
                .nombrePersona = currentItem.UsuarioAsignado,
                .numeroDocumento = currentItem.IdUsuarioAsignado,
                .numeroSerie = currentItem.Serial,
                .tipoActivo = "un equipo " + currentItem.TipoPeriferico
            }

            Dim htmlTemplate = BuildEmailTemplate(buildPDFParams)
            Dim fileName = "constancia-asignacion-activo.pdf"
            Dim fullPath = SaveAsPDFFromHTML(fileName, htmlTemplate)

            OpenPDFDocument(fileName)
        End If
    End Sub

    Private Sub gvUsuarios_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvUsuarios.RowCommand
        If e.CommandName = "Seleccionar" Then
            txtUsuario.Text = Server.HtmlDecode(gvUsuarios.Rows(e.CommandArgument).Cells(0).Text)
            hfIdUsuario.Value = Me.gvUsuarios.DataKeys(CInt(e.CommandArgument))("Id")

            txtEmailToSendFile.Text = Server.HtmlDecode(gvUsuarios.Rows(e.CommandArgument).Cells(3).Text)

            If Server.HtmlDecode(gvUsuarios.Rows(e.CommandArgument).Cells(2).Text) = "Contratista" Then
                hfIdCargo.Value = 2
            Else
                hfIdCargo.Value = 1
            End If
            txtCargo.Text = Server.HtmlDecode(gvUsuarios.Rows(e.CommandArgument).Cells(2).Text)

            If lblArticulo.Text = "Tablet" Then
                lblEstadoTablet.Visible = True
                ddlEstadoTablet.Visible = True

            End If

            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End If
    End Sub

    Private Sub gvJBEJBICC_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvJBEJBICC.RowCommand
        If e.CommandName = "Seleccionar" Then
            txtJBIJBE.Text = Server.HtmlDecode(gvJBEJBICC.Rows(e.CommandArgument).Cells(1).Text)
            txtNombreJBIJBE.Text = Server.HtmlDecode(gvJBEJBICC.Rows(e.CommandArgument).Cells(0).Text)

            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End If
    End Sub

#End Region


#Region "Grillas"

    Public Sub CargarGrid()
        Dim oListadoArticulos As New Inventario
        gvArticulos.DataSource = oListadoArticulos.obtenerRegistroArticulosxTodos
        gvArticulos.DataBind()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Sub CargarGridPersonas()
        Dim o As New Personas
        Dim daContratistas As New Contratista
        Dim cedula As Int64? = Nothing
        Dim nombre As String = Nothing

        If IsNumeric(txtCedulaUsuario.Text) Then cedula = txtCedulaUsuario.Text
        If Not txtNombreUsuario.Text = "" Then nombre = txtNombreUsuario.Text

        Dim lstPersonas = o.ObtenerPersonasxCCNombre(cedula, nombre)
        Dim lstContratistas = daContratistas.ObtenerContratistas(cedula, nombre, True)

        Dim un = (From x In lstPersonas
                  Select Nombres = x.Nombres & " " & x.Apellidos, Id = x.id, Ciudad = x.Ciudad, Cargo = x.Cargo, Email = x.EmailUsuario
                    ).Union(
                    From y In lstContratistas
                    Select Nombres = y.Nombre, Id = y.Identificacion, Ciudad = y.Ciudad, Cargo = "Contratista", Email = y.Email
                    ).ToList


        Me.gvUsuarios.DataSource = un
        Me.gvUsuarios.DataBind()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Sub CargarGridJBEJBICC()
        Dim oProyecto As New Proyecto
        Dim oTrabajo As New Trabajo
        Dim Busqueda As String = Nothing

        If Not txtJBEJBICC.Text = "" Then Busqueda = txtJBEJBICC.Text

        Dim lstProyectos = oProyecto.obtenerTodosCampos(Busqueda)
        Dim lstTrabajos = oTrabajo.obtenerPorBusqueda(Busqueda)

        If ddlCentroCosto.SelectedValue = 1 Then
            Dim cProyecto = (From x In lstProyectos
                             Select x.Nombre, x.JobBook).ToList

            Me.gvJBEJBICC.DataSource = cProyecto

        ElseIf ddlCentroCosto.SelectedValue = 2 Then
            Dim cTrabajo = (From x In lstTrabajos
                            Select Nombre = x.NombreTrabajo, x.JobBook).ToList

            Me.gvJBEJBICC.DataSource = cTrabajo
        End If

        Me.gvJBEJBICC.DataBind()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)

    End Sub

    Sub Asignar()

        If Not (IsDate(txtFechaAsignado.Text)) Then
            ShowNotification("Escriba la fecha de la asignación", ShowNotifications.ErrorNotification)
            txtFechaAsignado.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlCentroCosto.Visible = True And ddlCentroCosto.SelectedValue = "-1" Then
            ShowNotification("Seleccione el Centro de Costo y luego el JB o la BU", ShowNotifications.ErrorNotification)
            ddlCentroCosto.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlCentroCosto.SelectedValue = 3 AndAlso ddlBU.SelectedValue = "-1" Then
            ShowNotification("Seleccione la Unidad de Negocio", ShowNotifications.ErrorNotification)
            ddlBU.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If (ddlCentroCosto.SelectedValue = 1 Or ddlCentroCosto.SelectedValue = 2) AndAlso String.IsNullOrEmpty(txtJBIJBE.Text) Then
            ShowNotification("Debe indicar un codigo de JobBook", ShowNotifications.ErrorNotification)
            txtJBIJBE.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If (ddlCentroCosto.SelectedValue = 1 Or ddlCentroCosto.SelectedValue = 2) AndAlso String.IsNullOrEmpty(txtNombreJBIJBE.Text) Then
            ShowNotification("Debe indicar un nombre de JobBook", ShowNotifications.ErrorNotification)
            txtNombreJBIJBE.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlCiudad.SelectedValue = "-1" Then
            ShowNotification("Debe seleccionar una Ciudad", ShowNotifications.ErrorNotification)
            ddlCiudad.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlSede.Visible = True And ddlSede.SelectedValue = "-1" Then
            ShowNotification("Debe seleccionar una Sede", ShowNotifications.ErrorNotification)
            ddlSede.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlTipoGrupoUnidad.SelectedValue = "-1" Then
            ShowNotification("Debe seleccionar un Tipo de Grupo de Unidad", ShowNotifications.ErrorNotification)
            ddlTipoGrupoUnidad.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlGrupoUnidad.SelectedValue = "-1" Then
            ShowNotification("Debe seleccionar un Grupo de Unidad", ShowNotifications.ErrorNotification)
            ddlGrupoUnidad.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlUnidad.SelectedValue = "-1" Then
            ShowNotification("Debe seleccionar una Unidad", ShowNotifications.ErrorNotification)
            ddlUnidad.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlEstadoTablet.Visible = True And ddlEstadoTablet.SelectedValue = "-1" Then
            ShowNotification("Debe seleccionar un Estado para Tablet", ShowNotifications.ErrorNotification)
            ddlEstadoTablet.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If hfIdUsuario.Value = 0 Then
            ShowNotification("Debe seleccionar el Usuario", ShowNotifications.ErrorNotification)
            btnSearchUsuario.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        Dim oGuardar As New Inventario
        Dim EstadoTablet As Int64? = Nothing
        If Not ddlEstadoTablet.SelectedValue = -1 Then EstadoTablet = ddlEstadoTablet.SelectedValue

        Dim lstRegistroArticulos As List(Of INV_RegistroArticulos_Get_Result)
        lstRegistroArticulos = ObtenerRegistroArticulos(lblIdAsignar.Text)

        Dim lstAsignaciones As List(Of INV_Asignaciones_Get_Result)
        lstAsignaciones = ObtenerAsignaciones(lstRegistroArticulos(0).Id)
        Dim Id As Int64? = Nothing
        If lstAsignaciones.Count > 0 Then Id = lstAsignaciones(0).Id

        Dim UsuarioRegistra As Int64?
        UsuarioRegistra = CType(Session("IDUsuario").ToString, Int64?)

        Dim CentroCosto As Int16? = Nothing
        If Not ddlCentroCosto.SelectedValue = -1 Then CentroCosto = ddlCentroCosto.SelectedValue

        Dim BU As Int32? = Nothing
        If Not ddlBU.SelectedValue = -1 Then BU = ddlBU.SelectedValue

        Dim IdTrabajo As Int64? = Nothing
        If ddlCentroCosto.SelectedValue = 1 Or ddlCentroCosto.SelectedValue = 2 Then IdTrabajo = hfIdTrabajo.Value

        Dim JobBookCodigo As String = Nothing
        If Not txtJBIJBE.Text = "" Then JobBookCodigo = txtJBIJBE.Text

        Dim JobBookNombre As String = Nothing
        If Not txtNombreJBIJBE.Text = "" Then JobBookNombre = txtNombreJBIJBE.Text

        Dim Sede As Int32? = Nothing
        If Not ddlSede.SelectedValue = -1 Then Sede = ddlSede.SelectedValue

        Dim TipoGrupoUnidad As Int16? = Nothing
        If Not ddlTipoGrupoUnidad.SelectedValue = -1 Then TipoGrupoUnidad = ddlTipoGrupoUnidad.SelectedValue

        Dim GrupoUnidad As Int32? = Nothing
        If Not ddlGrupoUnidad.SelectedValue = -1 Then GrupoUnidad = ddlGrupoUnidad.SelectedValue

        Dim Unidad As Int32? = Nothing
        If Not ddlUnidad.SelectedValue = -1 Then Unidad = ddlUnidad.SelectedValue

        If lstAsignaciones.Count > 0 Then
            oGuardar.ActualizarAsignaciones(Id, lblIdAsignar.Text, UsuarioRegistra, txtFechaAsignado.Text, CentroCosto, BU, IdTrabajo, JobBookCodigo, JobBookNombre, ddlCiudad.SelectedValue, EstadoTablet, hfIdUsuario.Value, hfIdCargo.Value, txtCargo.Text, txtObservacion.Text, Sede, TipoGrupoUnidad, GrupoUnidad, Unidad)
        Else
            oGuardar.GuardarAsignaciones(lblIdAsignar.Text, UsuarioRegistra, txtFechaAsignado.Text, CentroCosto, BU, IdTrabajo, JobBookCodigo, JobBookNombre, ddlCiudad.SelectedValue, EstadoTablet, hfIdUsuario.Value, hfIdCargo.Value, txtCargo.Text, txtObservacion.Text, Sede, TipoGrupoUnidad, GrupoUnidad, Unidad)
        End If

        oGuardar.ActualizarRegistroArticulos_Asignado(lblIdAsignar.Text, True)
        oGuardar.GuardarLogAsignaciones(lblIdAsignar.Text, lstRegistroArticulos(0).IdArticulo, hfIdUsuario.Value, CentroCosto, BU, IdTrabajo, JobBookCodigo, JobBookNombre, ddlCiudad.SelectedValue, EstadoTablet, True)

        ShowNotification("La Asignación se ha hecho correctamente", ShowNotifications.InfoNotification)
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)



        Dim firstObject = lstRegistroArticulos(0)

        Dim paramsCorreo = New ConstanciaAsignacionParams With {
            .fecha = firstObject.FechaAsignacion,
            .marcaActivo = firstObject.Marca,
            .modeloActivo = firstObject.Modelo,
            .nombrePersona = firstObject.UsuarioAsignado,
            .numeroDocumento = firstObject.IdUsuarioAsignado,
            .numeroSerie = firstObject.Serial,
            .tipoActivo = "un equipo " + firstObject.TipoPeriferico
        }

        EnviarCorreoAsignacion(txtEmailToSendFile.Text, paramsCorreo)


        limpiar()

    End Sub

    Sub Desasignar()

        Dim oAccion As New Inventario
        Dim lstRegistroArticulos As List(Of INV_RegistroArticulos_Get_Result)
        lstRegistroArticulos = ObtenerRegistroArticulos(lblIdAsignar.Text)

        Dim UsuarioRegistra As Int64?
        UsuarioRegistra = CType(Session("IDUsuario").ToString, Int64?)

        'Dim FechaDesasigna As Date?
        'FechaDesasigna = CType(Date.Now.ToString, Date?)

        'Dim EstadoTablet As Int64? = Nothing
        'If lstRegistroArticulos(0).IdArticulo = 4 Then EstadoTablet = 1

        'If lstRegistroArticulos(0).IdAsignado = True Then
        '    Dim lstAsignaciones As List(Of INV_Asignaciones_Get_Result)
        '    lstAsignaciones = ObtenerAsignaciones(lstRegistroArticulos(0).Id)
        '    Dim Id As Int64? = lstAsignaciones(0).Id
        '    oAccion.ActualizarAsignaciones(Id, lblIdAsignar.Text, UsuarioRegistra, FechaDesasigna, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, EstadoTablet, Nothing, Nothing, Nothing, "Articulo Desasignado", Nothing, Nothing, Nothing, Nothing)
        'End If

        oAccion.ActualizarRegistroArticulos_Asignado(lblIdAsignar.Text, False)
        oAccion.GuardarLogAsignaciones(lblIdAsignar.Text, lstRegistroArticulos(0).IdArticulo, UsuarioRegistra, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, False)
        oAccion.EliminarAsignacion(lstRegistroArticulos(0).Id)

        ShowNotification("La Desasignación se ha hecho correctamente", ShowNotifications.InfoNotification)
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)

        limpiar()

    End Sub

    Public Sub CargarColumnas()

        'Computador
        If ddlIdArticulo.SelectedValue = 1 Then
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = True  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = True  'TipoComputador
            gvArticulos.Columns(21).Visible = True  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = True  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = True  'Serial
            gvArticulos.Columns(30).Visible = True  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
        End If

        'Servidor
        If ddlIdArticulo.SelectedValue = 2 Then
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = True  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = True  'Serial
            gvArticulos.Columns(30).Visible = True  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = True  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
        End If

        'Dispositivos Periféricos
        If ddlIdArticulo.SelectedValue = 3 Then
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = True  'TipoPeriferico
            gvArticulos.Columns(23).Visible = True  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = True  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
        End If

        'Tablet
        If ddlIdArticulo.SelectedValue = 4 Then
            gvArticulos.Columns(5).Visible = False  'Fecha Registro
            gvArticulos.Columns(6).Visible = False  'Usuario Registra
            gvArticulos.Columns(14).Visible = False  'Valor
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(16).Visible = False  'Descripcion
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = False  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = False  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = False  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = True  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = True  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
        End If

        'Celular
        If ddlIdArticulo.SelectedValue = 5 Then
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = True  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = False  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = True  'Operador
            gvArticulos.Columns(42).Visible = True  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
        End If

        'SimCard
        If ddlIdArticulo.SelectedValue = 6 Then
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = False  'Marca
            gvArticulos.Columns(24).Visible = False  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = False  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = True  'Operador
            gvArticulos.Columns(42).Visible = True  'NumeroCelular
            gvArticulos.Columns(43).Visible = True  'CantidadMinutos
        End If

        'Diademas FI
        If ddlIdArticulo.SelectedValue = 10 Then
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = True  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = True  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
        End If
        Try
            gvArticulos.Columns(31).HeaderText = "Serial Windows"
        Catch ex As Exception
        End Try
    End Sub
    Public Sub CargarColumnasAS()

        'Computador
        If ddlArticuloAS.SelectedValue = 1 Then
            gvAsignaciones.Columns(3).Visible = True  'TipoComputador
            gvAsignaciones.Columns(4).Visible = False  'TipoPeriferico
            gvAsignaciones.Columns(5).Visible = True  'Marca
            gvAsignaciones.Columns(6).Visible = True  'Modelo
            gvAsignaciones.Columns(7).Visible = True  'Serial
            gvAsignaciones.Columns(8).Visible = True  'NombreEquipo
            gvAsignaciones.Columns(9).Visible = False  'IdTablet
            gvAsignaciones.Columns(10).Visible = False  'Chip
            gvAsignaciones.Columns(11).Visible = False  'IMEI
            gvAsignaciones.Columns(12).Visible = False  'NumeroCelular
            gvAsignaciones.Columns(13).Visible = False  'CantidadMinutos
            gvAsignaciones.Columns(16).Visible = False  'CentroCosto
            gvAsignaciones.Columns(17).Visible = False  'BU
            gvAsignaciones.Columns(18).Visible = False  'JobBookCodigo
            gvAsignaciones.Columns(19).Visible = False  'JobBookNombre
            gvAsignaciones.Columns(24).Visible = False  'EstadoTablet
            gvAsignaciones.Columns(25).Visible = True  'IdFisico

        End If

        'Servidor
        If ddlArticuloAS.SelectedValue = 2 Then
            gvAsignaciones.Columns(3).Visible = False  'TipoComputador
            gvAsignaciones.Columns(4).Visible = False  'TipoPeriferico
            gvAsignaciones.Columns(5).Visible = True  'Marca
            gvAsignaciones.Columns(6).Visible = True  'Modelo
            gvAsignaciones.Columns(7).Visible = True  'Serial
            gvAsignaciones.Columns(8).Visible = False  'NombreEquipo
            gvAsignaciones.Columns(9).Visible = False  'IdTablet
            gvAsignaciones.Columns(10).Visible = False  'Chip
            gvAsignaciones.Columns(11).Visible = False  'IMEI
            gvAsignaciones.Columns(12).Visible = False  'NumeroCelular
            gvAsignaciones.Columns(13).Visible = False  'CantidadMinutos
            gvAsignaciones.Columns(16).Visible = False  'CentroCosto
            gvAsignaciones.Columns(17).Visible = False  'BU
            gvAsignaciones.Columns(18).Visible = False  'JobBookCodigo
            gvAsignaciones.Columns(19).Visible = False  'JobBookNombre
            gvAsignaciones.Columns(24).Visible = False  'EstadoTablet
            gvAsignaciones.Columns(25).Visible = False  'IdFisico

        End If

        'Dispositivos Periféricos
        If ddlArticuloAS.SelectedValue = 3 Then
            gvAsignaciones.Columns(3).Visible = False  'TipoComputador
            gvAsignaciones.Columns(4).Visible = True  'TipoPeriferico
            gvAsignaciones.Columns(5).Visible = True  'Marca
            gvAsignaciones.Columns(6).Visible = True  'Modelo
            gvAsignaciones.Columns(7).Visible = True  'Serial
            gvAsignaciones.Columns(8).Visible = False  'NombreEquipo
            gvAsignaciones.Columns(9).Visible = False  'IdTablet
            gvAsignaciones.Columns(10).Visible = False  'Chip
            gvAsignaciones.Columns(11).Visible = False  'IMEI
            gvAsignaciones.Columns(12).Visible = False  'NumeroCelular
            gvAsignaciones.Columns(13).Visible = False  'CantidadMinutos
            gvAsignaciones.Columns(16).Visible = False  'CentroCosto
            gvAsignaciones.Columns(17).Visible = False  'BU
            gvAsignaciones.Columns(18).Visible = False  'JobBookCodigo
            gvAsignaciones.Columns(19).Visible = False  'JobBookNombre
            gvAsignaciones.Columns(24).Visible = False  'EstadoTablet
            gvAsignaciones.Columns(25).Visible = False  'IdFisico

        End If

        'Tablet
        If ddlArticuloAS.SelectedValue = 4 Then
            gvAsignaciones.Columns(3).Visible = False  'TipoComputador
            gvAsignaciones.Columns(4).Visible = False  'TipoPeriferico
            gvAsignaciones.Columns(5).Visible = False  'Marca
            gvAsignaciones.Columns(6).Visible = True  'Modelo
            gvAsignaciones.Columns(7).Visible = False  'Serial
            gvAsignaciones.Columns(8).Visible = False  'NombreEquipo
            gvAsignaciones.Columns(9).Visible = True  'IdTablet
            gvAsignaciones.Columns(10).Visible = True  'Chip
            gvAsignaciones.Columns(11).Visible = True  'IMEI
            gvAsignaciones.Columns(12).Visible = False  'NumeroCelular
            gvAsignaciones.Columns(13).Visible = False  'CantidadMinutos
            gvAsignaciones.Columns(16).Visible = True  'CentroCosto
            gvAsignaciones.Columns(17).Visible = True  'BU
            gvAsignaciones.Columns(18).Visible = True  'JobBookCodigo
            gvAsignaciones.Columns(19).Visible = True  'JobBookNombre
            gvAsignaciones.Columns(24).Visible = True  'EstadoTablet
            gvAsignaciones.Columns(25).Visible = False  'IdFisico

        End If

        'Celular
        If ddlArticuloAS.SelectedValue = 5 Then
            gvAsignaciones.Columns(3).Visible = False  'TipoComputador
            gvAsignaciones.Columns(4).Visible = False  'TipoPeriferico
            gvAsignaciones.Columns(5).Visible = True  'Marca
            gvAsignaciones.Columns(6).Visible = True  'Modelo
            gvAsignaciones.Columns(7).Visible = False  'Serial
            gvAsignaciones.Columns(8).Visible = False  'NombreEquipo
            gvAsignaciones.Columns(9).Visible = False  'IdTablet
            gvAsignaciones.Columns(10).Visible = False  'Chip
            gvAsignaciones.Columns(11).Visible = False  'IMEI
            gvAsignaciones.Columns(12).Visible = True  'NumeroCelular
            gvAsignaciones.Columns(13).Visible = False  'CantidadMinutos
            gvAsignaciones.Columns(16).Visible = False  'CentroCosto
            gvAsignaciones.Columns(17).Visible = False  'BU
            gvAsignaciones.Columns(18).Visible = False  'JobBookCodigo
            gvAsignaciones.Columns(19).Visible = False  'JobBookNombre
            gvAsignaciones.Columns(24).Visible = False  'EstadoTablet
            gvAsignaciones.Columns(25).Visible = False  'IdFisico

        End If

        'SimCard
        If ddlArticuloAS.SelectedValue = 6 Then
            gvAsignaciones.Columns(3).Visible = False  'TipoComputador
            gvAsignaciones.Columns(4).Visible = False  'TipoPeriferico
            gvAsignaciones.Columns(5).Visible = False  'Marca
            gvAsignaciones.Columns(6).Visible = False  'Modelo
            gvAsignaciones.Columns(7).Visible = False  'Serial
            gvAsignaciones.Columns(8).Visible = False  'NombreEquipo
            gvAsignaciones.Columns(9).Visible = False  'IdTablet
            gvAsignaciones.Columns(10).Visible = False  'Chip
            gvAsignaciones.Columns(11).Visible = False  'IMEI
            gvAsignaciones.Columns(12).Visible = True  'NumeroCelular
            gvAsignaciones.Columns(13).Visible = True  'CantidadMinutos
            gvAsignaciones.Columns(16).Visible = False  'CentroCosto
            gvAsignaciones.Columns(17).Visible = False  'BU
            gvAsignaciones.Columns(18).Visible = False  'JobBookCodigo
            gvAsignaciones.Columns(19).Visible = False  'JobBookNombre
            gvAsignaciones.Columns(24).Visible = False  'EstadoTablet
            gvAsignaciones.Columns(25).Visible = False  'IdFisico

        End If

        'Diademas FI
        If ddlArticuloAS.SelectedValue = 10 Then
            gvAsignaciones.Columns(3).Visible = False  'TipoComputador
            gvAsignaciones.Columns(4).Visible = False  'TipoPeriferico
            gvAsignaciones.Columns(5).Visible = True  'Marca
            gvAsignaciones.Columns(6).Visible = True  'Modelo
            gvAsignaciones.Columns(7).Visible = True  'Serial
            gvAsignaciones.Columns(8).Visible = False  'NombreEquipo
            gvAsignaciones.Columns(9).Visible = False  'IdTablet
            gvAsignaciones.Columns(10).Visible = False  'Chip
            gvAsignaciones.Columns(11).Visible = False  'IMEI
            gvAsignaciones.Columns(12).Visible = False  'NumeroCelular
            gvAsignaciones.Columns(13).Visible = False  'CantidadMinutos
            gvAsignaciones.Columns(16).Visible = False  'CentroCosto
            gvAsignaciones.Columns(17).Visible = False  'BU
            gvAsignaciones.Columns(18).Visible = False  'JobBookCodigo
            gvAsignaciones.Columns(19).Visible = False  'JobBookNombre
            gvAsignaciones.Columns(24).Visible = False  'EstadoTablet
            gvAsignaciones.Columns(25).Visible = False  'IdFisico

        End If

    End Sub

    Function ObtenerRegistroArticulos(ByVal Id As Int64?) As List(Of INV_RegistroArticulos_Get_Result)
        Dim lstRegistroArticulos As New List(Of INV_RegistroArticulos_Get_Result)
        Dim RecordArticulos As New Inventario
        lstRegistroArticulos = RecordArticulos.obtenerRegistroArticulosxId(Id)
        Return lstRegistroArticulos
    End Function

    Function ObtenerAsignaciones(ByVal IdActivoFijo As Int64?) As List(Of INV_Asignaciones_Get_Result)
        Dim lstAsignaciones As New List(Of INV_Asignaciones_Get_Result)
        Dim RecordAsignaciones As New Inventario
        lstAsignaciones = RecordAsignaciones.obtenerAsignacionesxIdActivoFijo(IdActivoFijo)
        Return lstAsignaciones
    End Function

    Function obtenerRegistrosArticulos() As List(Of INV_RegistroArticulos_Get_Result)
        Dim oBusqueda As New Inventario
        Dim Articulo As Int64? = Nothing
        Dim TipoComputador As Int64? = Nothing
        Dim PertenecePC As Int16? = Nothing
        Dim TipoPeriferico As Int64? = Nothing
        Dim Estado As Int64? = Nothing
        Dim Sede As Int64? = Nothing
        Dim IdUsuarioAsignado As Int64? = Nothing
        Dim UsuarioAsignado As String = Nothing
        Dim Asignado As Boolean? = Nothing
        Dim idArticulo As Int64? = Nothing
        Dim TodosCampos As String = Nothing

        If Not ddlIdArticulo.SelectedValue = -1 Then Articulo = ddlIdArticulo.SelectedValue
        If Not ddlIdTipoComputador.SelectedValue = -1 Then TipoComputador = ddlIdTipoComputador.SelectedValue
        If Not ddlIdPertenecePC.SelectedValue = -1 Then PertenecePC = ddlIdPertenecePC.SelectedValue
        If Not ddlIdPeriferico.SelectedValue = -1 Then TipoPeriferico = ddlIdPeriferico.SelectedValue
        If Not ddlIdEstado.SelectedValue = -1 Then Estado = ddlIdEstado.SelectedValue
        If Not ddlIdSede.SelectedValue = -1 Then Sede = ddlIdSede.SelectedValue
        If Not ddlIdAsignado.SelectedValue = -1 Then
            If ddlIdAsignado.SelectedValue = 1 Then Asignado = True
            If ddlIdAsignado.SelectedValue = 0 Then Asignado = False
        End If
        If IsNumeric(txtIdUsuario.Text) Then IdUsuarioAsignado = txtIdUsuario.Text
        If Not txtNomUsuario.Text = "" Then UsuarioAsignado = txtNomUsuario.Text
        If Not txtIdArticulo.Text = "" Then idArticulo = Convert.ToInt64(txtIdArticulo.Text)
        If Not txtTodosCampos.Text = "" Then TodosCampos = txtTodosCampos.Text

        Return oBusqueda.obtenerRegistroArticulos(Nothing, 1, Articulo, TipoComputador, PertenecePC, TipoPeriferico, Nothing, Estado, Sede, IdUsuarioAsignado, UsuarioAsignado, Asignado, idArticulo, TodosCampos)
    End Function
    Function ObtenerAsignaciones() As List(Of INV_Asignaciones_Get_Result)
        Dim oBusqueda As New Inventario
        Dim Articulo As Int64? = Nothing
        Dim BU As Int32? = Nothing
        Dim JobBookCodigo As String = Nothing
        Dim JobBookNombre As String = Nothing
        Dim Ciudad As Int64? = Nothing
        Dim EstadoTablet As Int64? = Nothing
        Dim IdUsuarioAsignado As Int64? = Nothing
        Dim UsuarioAsignado As String = Nothing
        Dim TipoCargo As Int16? = Nothing
        Dim Asignado As Boolean? = Nothing

        If Not ddlArticuloAS.SelectedValue = -1 Then Articulo = ddlArticuloAS.SelectedValue
        If Not ddlBUAS.SelectedValue = -1 Then BU = ddlBUAS.SelectedValue
        If Not txtJBIJBEAS.Text = "" Then JobBookCodigo = txtJBIJBEAS.Text
        If Not txtNombreJBIJBEAS.Text = "" Then JobBookNombre = txtNombreJBIJBEAS.Text
        If Not ddlCiudadAS.SelectedValue = -1 Then Ciudad = ddlCiudadAS.SelectedValue
        If Not ddlEstadoTabletAS.SelectedValue = -1 Then EstadoTablet = ddlEstadoTabletAS.SelectedValue
        If IsNumeric(txtIdUsuarioAs.Text) Then IdUsuarioAsignado = txtIdUsuarioAs.Text
        If Not txtUsuarioAs.Text = "" Then UsuarioAsignado = txtUsuarioAs.Text
        If Not ddlTipoCargoAS.SelectedValue = -1 Then TipoCargo = ddlTipoCargoAS.SelectedValue

        If Not ddlAsignadoAS.SelectedValue = -1 Then
            If ddlAsignadoAS.SelectedValue = 1 Then Asignado = True
            If ddlAsignadoAS.SelectedValue = 0 Then Asignado = False
        End If

        Return oBusqueda.obtenerAsignaciones(Nothing, Articulo, BU, JobBookCodigo, JobBookNombre, Ciudad, EstadoTablet, IdUsuarioAsignado, UsuarioAsignado, TipoCargo, Asignado)
    End Function

#End Region

#Region "DDL"

    Sub CargarActivosFijos()
        Dim oArticulos As New Inventario
        Dim oUsuario As New US.UsuariosUnidades
        Dim Usuario = oUsuario.obtenerUnidadesXUsuario(Session("IDUsuario").ToString, True, Nothing)

        For Each oGrupoUnidad In Usuario
            Dim GrupoUnidad As Int32? = oGrupoUnidad.GrupoUnidadId
            Dim Busqueda = oArticulos.obtenerArticulosxTipoArticulo(1, GrupoUnidad)
            If Busqueda.Count > 0 Then
                Me.ddlIdArticulo.DataSource = oArticulos.obtenerArticulosxTipoArticulo(1, GrupoUnidad)
                Exit For
            Else
                Me.ddlIdArticulo.Items.Clear()
            End If
        Next

        Me.ddlIdArticulo.DataValueField = "Id"
        Me.ddlIdArticulo.DataTextField = "Articulo"
        Me.ddlIdArticulo.DataBind()
        Me.ddlIdArticulo.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarActivosFijosAS()
        Dim oArticulos As New Inventario
        Dim oUsuario As New US.UsuariosUnidades
        Dim Usuario = oUsuario.obtenerUnidadesXUsuario(Session("IDUsuario").ToString, True, Nothing)

        For Each oGrupoUnidad In Usuario
            Dim GrupoUnidad As Int32? = oGrupoUnidad.GrupoUnidadId
            Dim Busqueda = oArticulos.obtenerArticulosxTipoArticulo(1, GrupoUnidad)
            If Busqueda.Count > 0 Then
                Me.ddlArticuloAS.DataSource = oArticulos.obtenerArticulosxTipoArticulo(1, GrupoUnidad)
                Exit For
            Else
                Me.ddlArticuloAS.Items.Clear()
            End If
        Next

        Me.ddlArticuloAS.DataValueField = "Id"
        Me.ddlArticuloAS.DataTextField = "Articulo"
        Me.ddlArticuloAS.DataBind()
        Me.ddlArticuloAS.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarPerifericos()
        Dim oPerifericos As New Inventario
        Me.ddlIdPeriferico.DataSource = oPerifericos.obtenerDispositivosPerifericos
        Me.ddlIdPeriferico.DataValueField = "Id"
        Me.ddlIdPeriferico.DataTextField = "Periferico"
        Me.ddlIdPeriferico.DataBind()
        Me.ddlIdPeriferico.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarEstadoTablet()
        Dim oArticulos As New Inventario
        Me.ddlEstadoTablet.DataSource = oArticulos.obtenerEstadosTablet
        Me.ddlEstadoTablet.DataValueField = "Id"
        Me.ddlEstadoTablet.DataTextField = "Estado"
        Me.ddlEstadoTablet.DataBind()
        Me.ddlEstadoTablet.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarEstadoTabletAS()
        Dim oArticulos As New Inventario
        Me.ddlEstadoTabletAS.DataSource = oArticulos.obtenerEstadosTablet
        Me.ddlEstadoTabletAS.DataValueField = "Id"
        Me.ddlEstadoTabletAS.DataTextField = "Estado"
        Me.ddlEstadoTabletAS.DataBind()
        Me.ddlEstadoTabletAS.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarBU()
        Dim oBU As New Inventario
        Me.ddlBU.DataSource = oBU.obtenerBU
        Me.ddlBU.DataValueField = "Id"
        Me.ddlBU.DataTextField = "Nombre"
        Me.ddlBU.DataBind()
        Me.ddlBU.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarBUAS()
        Dim oBU As New Inventario
        Me.ddlBUAS.DataSource = oBU.obtenerBU
        Me.ddlBUAS.DataValueField = "Id"
        Me.ddlBUAS.DataTextField = "Nombre"
        Me.ddlBUAS.DataBind()
        Me.ddlBUAS.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarCiudades()
        Dim oCiudad As New CoreProject.RegistroPersonas
        Me.ddlCiudad.DataSource = oCiudad.CiudadesList
        Me.ddlCiudad.DataValueField = "id"
        Me.ddlCiudad.DataTextField = "Ciudad"
        Me.ddlCiudad.DataBind()
        Me.ddlCiudad.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarCiudadesAS()
        Dim oCiudad As New CoreProject.RegistroPersonas
        Me.ddlCiudadAS.DataSource = oCiudad.CiudadesList
        Me.ddlCiudadAS.DataValueField = "id"
        Me.ddlCiudadAS.DataTextField = "Ciudad"
        Me.ddlCiudadAS.DataBind()
        Me.ddlCiudadAS.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub EstadosArticulos()
        Dim oEstados As New Inventario
        Me.ddlIdEstado.DataSource = oEstados.obtenerEstadosArticulos
        Me.ddlIdEstado.DataValueField = "Id"
        Me.ddlIdEstado.DataTextField = "Estado"
        Me.ddlIdEstado.DataBind()
        Me.ddlIdEstado.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarSedes()
        Dim oSedes As New Inventario
        Me.ddlIdSede.DataSource = oSedes.obtenerSedes
        Me.ddlIdSede.DataValueField = "Id"
        Me.ddlIdSede.DataTextField = "Sede"
        Me.ddlIdSede.DataBind()
        Me.ddlIdSede.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarCombo(ByVal cmb As DropDownList, ByVal dvf As String, ByVal dtf As String, ByVal ds As Object)
        cmb.ClearSelection()
        cmb.DataValueField = dvf
        cmb.DataTextField = dtf
        cmb.DataSource = ds
        cmb.DataBind()

        cmb.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})

    End Sub

#End Region

#Region "Formulario"

    Sub limpiar()
        Me.txtFechaAsignado.Text = ""
        Me.ddlCentroCosto.SelectedValue = "-1"
        Me.ddlBU.SelectedValue = "-1"
        hfIdTrabajo.Value = "0"
        Me.txtJBIJBE.Text = ""
        Me.txtNombreJBIJBE.Text = ""
        ddlCiudad.ClearSelection()
        ddlSede.ClearSelection()
        ddlTipoGrupoUnidad.ClearSelection()
        ddlGrupoUnidad.ClearSelection()
        ddlUnidad.ClearSelection()
        ddlEstadoTablet.ClearSelection()
        Me.txtUsuario.Text = ""
        Me.txtCedulaUsuario.Text = ""
        Me.txtNombreUsuario.Text = ""
        hfIdUsuario.Value = "0"
        Me.txtCargo.Text = ""
        hfIdCargo.Value = "0"
        Me.txtObservacion.Text = ""

        lblIdAsignar.Text = ""
        lblArticulo.Text = ""
        lblIdAsignar.Visible = False
        lblAsignar.Visible = False
        lblArticulo.Visible = False
        lblSede.Visible = False
        ddlSede.Visible = False

        ddlIdArticulo.ClearSelection()
        ddlIdTipoComputador.ClearSelection()
        ddlIdPertenecePC.ClearSelection()
        ddlIdPeriferico.ClearSelection()
        ddlIdEstado.ClearSelection()
        ddlIdSede.ClearSelection()
        ddlIdAsignado.ClearSelection()
        txtIdUsuario.Text = ""
        txtNomUsuario.Text = ""
        gvArticulos.DataSource = Nothing
        gvArticulos.DataBind()

        ddlArticuloAS.ClearSelection()
        ddlBUAS.ClearSelection()
        txtJBIJBEAS.Text = ""
        txtNombreJBIJBEAS.Text = ""
        ddlCiudadAS.ClearSelection()
        ddlEstadoTabletAS.ClearSelection()
        txtIdUsuarioAs.Text = ""
        txtUsuarioAs.Text = ""
        ddlTipoCargoAS.ClearSelection()
        ddlAsignadoAS.ClearSelection()
        gvAsignaciones.DataSource = Nothing
        gvAsignaciones.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

#End Region

    Protected Sub btnSearchJBEJBICC_Click(sender As Object, e As EventArgs) Handles btnSearchJBEJBICC.Click
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnSearchUsuario_Click(sender As Object, e As EventArgs) Handles btnSearchUsuario.Click
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnAsignar_Click(sender As Object, e As EventArgs) Handles btnAsignar.Click
        Asignar()
        'EnviarCorreoAsignacion()
    End Sub

    Protected Sub btnDesasignar_Click(sender As Object, e As EventArgs) Handles btnDesasignar.Click
        Desasignar()
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        limpiar()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargarColumnas()
        gvArticulos.DataSource = obtenerRegistrosArticulos()
        gvArticulos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnBuscarAS_Click(sender As Object, e As EventArgs) Handles btnBuscarAS.Click
        CargarColumnasAS()
        gvAsignaciones.DataSource = ObtenerAsignaciones()
        gvAsignaciones.DataBind()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnBuscarUsuario_Click(sender As Object, e As EventArgs) Handles btnBuscarUsuario.Click
        CargarGridPersonas()
        UPanelUsuarios.Update()
    End Sub

    Protected Sub btnBuscarJBEJBICC_Click(sender As Object, e As EventArgs) Handles btnBuscarJBEJBICC.Click
        CargarGridJBEJBICC()
        upJBEJBICC.Update()
    End Sub

    Protected Sub ddlIdArticulo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlIdArticulo.SelectedIndexChanged
        If ddlIdArticulo.SelectedValue = 1 Then
            lblIdTipoComputador.Visible = True
            ddlIdTipoComputador.Visible = True
            lblIdPertenecePC.Visible = True
            ddlIdPertenecePC.Visible = True
        Else
            lblIdTipoComputador.Visible = False
            ddlIdTipoComputador.Visible = False
            ddlIdTipoComputador.ClearSelection()
            lblIdPertenecePC.Visible = False
            ddlIdPertenecePC.Visible = False
            ddlIdPertenecePC.ClearSelection()
        End If

        If ddlIdArticulo.SelectedValue = 3 Then
            lblIdPeriferico.Visible = True
            ddlIdPeriferico.Visible = True
        Else
            lblIdPeriferico.Visible = False
            ddlIdPeriferico.Visible = False
            ddlIdPeriferico.ClearSelection()
        End If

        ActivateAccordion(0, EffectActivateAccordion.NoEffect)

    End Sub

    Protected Sub ddlArticuloAS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlArticuloAS.SelectedIndexChanged

        If ddlArticuloAS.SelectedValue = 4 Then
            lblBUAS.Visible = True
            ddlBUAS.Visible = True
            lblJBIJBEAS.Visible = True
            txtJBIJBEAS.Visible = True
            lblNombreJBIJBEAS.Visible = True
            txtNombreJBIJBEAS.Visible = True
            lblEstadoTabletAS.Visible = True
            ddlEstadoTabletAS.Visible = True
        Else
            lblBUAS.Visible = False
            ddlBUAS.Visible = False
            ddlBUAS.ClearSelection()
            lblJBIJBEAS.Visible = False
            txtJBIJBEAS.Visible = False
            txtJBIJBEAS.Text = ""
            lblNombreJBIJBEAS.Visible = False
            txtNombreJBIJBEAS.Visible = False
            txtNombreJBIJBEAS.Text = ""
            lblEstadoTabletAS.Visible = False
            ddlEstadoTabletAS.Visible = False
            ddlEstadoTabletAS.ClearSelection()
        End If
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlCentroCosto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCentroCosto.SelectedIndexChanged
        hfIdTrabajo.Value = "0"
        If ddlCentroCosto.SelectedValue = 3 Then

            lblBU.Visible = True
            ddlBU.Visible = True
            lblJBIJBE.Visible = False
            txtJBIJBE.Visible = False
            txtJBIJBE.Text = ""
            btnSearchJBEJBICC.Visible = False
            lblNombreJBIJBE.Visible = False
            txtNombreJBIJBE.Visible = False
            txtNombreJBIJBE.Text = ""

        Else

            lblJBIJBE.Visible = True
            txtJBIJBE.Visible = True
            txtJBIJBE.Text = ""
            btnSearchJBEJBICC.Visible = True
            lblNombreJBIJBE.Visible = True
            txtNombreJBIJBE.Visible = True
            txtNombreJBIJBE.Text = ""
            lblBU.Visible = False
            ddlBU.Visible = False
            ddlBU.ClearSelection()
        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub txtJBIJBE_TextChanged(sender As Object, e As EventArgs) Handles txtJBIJBE.TextChanged
        Dim daProyecto As New Proyecto
        Dim daTrabajo As New Trabajo
        Dim oP As PY_Proyectos_Get_Result
        Dim oT As PY_Trabajo0
        Select Case ddlCentroCosto.SelectedValue
            Case 1
                oP = daProyecto.obtenerXJobBook(txtJBIJBE.Text)
                If Not ((oP Is Nothing)) Then
                    txtNombreJBIJBE.Text = oP.Nombre
                    hfIdTrabajo.Value = oP.id
                Else
                    txtNombreJBIJBE.Text = ""
                    hfIdTrabajo.Value = ""
                End If

            Case 2
                oT = daTrabajo.ObtenerTrabajoXJob(txtJBIJBE.Text)
                If Not ((oT Is Nothing)) Then
                    txtNombreJBIJBE.Text = oT.NombreTrabajo
                    hfIdTrabajo.Value = oT.id
                Else
                    txtNombreJBIJBE.Text = ""
                    hfIdTrabajo.Value = ""
                End If
        End Select
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlCiudad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCiudad.SelectedIndexChanged
        If ddlCiudad.SelectedValue = 11001 Then
            lblSede.Visible = True
            ddlSede.Visible = True
        Else
            lblSede.Visible = False
            ddlSede.Visible = False
            ddlSede.ClearSelection()
        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlTipoGrupoUnidad_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipoGrupoUnidad.SelectedIndexChanged
        CargarCombo(ddlGrupoUnidad, "Id", "GrupoUnidad", ObtenerGrupoUnidad(ddlTipoGrupoUnidad.SelectedValue))
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlGrupoUnidad_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlGrupoUnidad.SelectedIndexChanged
        CargarCombo(ddlUnidad, "Id", "Unidad", ObtenerUnidad(ddlGrupoUnidad.SelectedValue, False, hfIdUsuario.Value))
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
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
        Return Data.obtenerUnidadesXUsuario(usuario, Nothing, Grupo)
    End Function

    Protected Sub btnExportar_Click(sender As Object, e As System.EventArgs) Handles btnExportar.Click
        CargarColumnas()
        gvArticulos.DataSource = obtenerRegistrosArticulos()
        gvArticulos.DataBind()
        gvArticulos.Visible = True
        'Actualiza los datos del gridview
        gvArticulos.AllowPaging = False
        gvArticulos.DataBind()

        'Crea variables en memoria para almacenar el contenido del gridview
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Crea una nueva página html y un form dentro de ella
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        'Cambie el estado del gridview para que no guarde los controles de vista
        gvArticulos.EnableViewState = False
        'Quita la validación de la página 
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        'Agrega el form creado
        pagina.Controls.Add(form)
        'Agrega el gridview al form
        form.Controls.Add(gvArticulos)
        'Hace un render para pasar los valores de la página al htmltextwriter que contendrá los datos del gridview
        pagina.RenderControl(htw)
        'Hace un response para descargar el control
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=ActivosFijos_Asignaciones.xls")
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        gvArticulos.Visible = False

    End Sub


    Protected Sub btnExportarAS_Click(sender As Object, e As EventArgs) Handles btnExportarAS.Click
        CargarColumnasAS()
        gvAsignaciones.DataSource = ObtenerAsignaciones()
        gvAsignaciones.DataBind()
        gvAsignaciones.Visible = True
        'Actualiza los datos del gridview
        gvAsignaciones.AllowPaging = False
        gvAsignaciones.DataBind()

        'Crea variables en memoria para almacenar el contenido del gridview
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Crea una nueva página html y un form dentro de ella
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        'Cambie el estado del gridview para que no guarde los controles de vista
        gvAsignaciones.EnableViewState = False
        'Quita la validación de la página 
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        'Agrega el form creado
        pagina.Controls.Add(form)
        'Agrega el gridview al form
        form.Controls.Add(gvAsignaciones)
        'Hace un render para pasar los valores de la página al htmltextwriter que contendrá los datos del gridview
        pagina.RenderControl(htw)
        'Hace un response para descargar el control
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=Listado_Asignaciones.xls")
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        gvAsignaciones.Visible = False
    End Sub


    Public Function BuildEmailTemplate(params As ConstanciaAsignacionParams) As String
        Dim htmlTemplate = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Emails/Templates/template-correo-asignacion-activos.html"))


        htmlTemplate = htmlTemplate.Replace("{{nombre}}", params.nombrePersona)
        htmlTemplate = htmlTemplate.Replace("{{cedula}}", params.numeroDocumento)
        htmlTemplate = htmlTemplate.Replace("{{descripcion_activo}}", params.tipoActivo)
        htmlTemplate = htmlTemplate.Replace("{{marca}}", params.marcaActivo)
        htmlTemplate = htmlTemplate.Replace("{{modelo}}", params.modeloActivo)
        htmlTemplate = htmlTemplate.Replace("{{numero_serie}}", params.numeroSerie)
        htmlTemplate = htmlTemplate.Replace("{{fecha}}", params.fecha)

        Return htmlTemplate
    End Function

    Public Function EnviarCorreoAsignacion(correo As String, params As ConstanciaAsignacionParams)
        Dim oEnviarCorreo As New EnviarCorreo
        Dim destinatarios As New List(Of String) From {
            correo
        }

        Dim htmlTemplate = BuildEmailTemplate(params)
        Dim fileName = "constancia-asignacion-activo.pdf"
        Dim fullPath = SaveAsPDFFromHTML(fileName, htmlTemplate)

        oEnviarCorreo.sendMail(destinatarios, "Constancia Asignacion de Activo", htmlTemplate)

    End Function

    Private Function SaveAsPDFFromHTML(fileName As String, htmlTemplate As String) As String

        Dim htmlTemplateBytes = Encoding.UTF8.GetBytes(htmlTemplate)
        Dim htmlTemplateBase64 = System.Convert.ToBase64String(htmlTemplateBytes)

        Dim serialisedData = JsonConvert.SerializeObject(New With {.HTMLBase64String = htmlTemplateBase64})

        Dim httpeClient = New Net.WebClient()

        httpeClient.Headers(Net.HttpRequestHeader.ContentType) = "application/json"

        Dim responseApi = httpeClient.UploadString("https://ipsoslatam.com/BIPServices/api/ConvertFiles/HTMLToPDF", "POST", serialisedData)


        Dim responseDictionary = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(responseApi)

        Dim pdfBase64 = responseDictionary.Item("pdfBase64String")

        Dim pdfBytes = Convert.FromBase64String(pdfBase64)

        Dim folderPath = Server.MapPath("~/Files/Temp")

        Dim fullPath = folderPath & "/" & fileName

        Dim pdfFile = File.Create(fullPath)

        pdfFile.Write(pdfBytes, 0, pdfBytes.Length)

        pdfFile.Flush()

        pdfFile.Close()

        Return fullPath
    End Function

    Private Sub OpenPDFDocument(fileName As String)
        Dim folderPath = "/Files/Temp"

        Dim fullPath = folderPath & "/" & fileName

        Response.Redirect(fullPath)


    End Sub

    Private Sub _AlmacenamientoDisco_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(141, UsuarioID) = False AndAlso permisos.VerificarPermisoUsuario(142, UsuarioID) = False Then
            Response.Redirect("../Inventario/RegistroArticulos.aspx")
        End If
    End Sub

    Public Structure ConstanciaAsignacionParams
        Public nombrePersona As String
        Public numeroDocumento As String
        Public tipoActivo As String
        Public marcaActivo As String
        Public modeloActivo As String
        Public numeroSerie As String
        Public fecha As String
    End Structure

End Class