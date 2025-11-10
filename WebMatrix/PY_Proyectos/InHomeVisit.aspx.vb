Imports CoreProject
Imports CoreProject.OP
Imports WebMatrix.Util

Public Class InHomeVisit
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            Dim o As New Trabajo

            'If permisos.VerificarPermisoUsuario(97, UsuarioID) = False Then
            '    Response.Redirect("../PY_Proyectos/PY_Proyectos.aspx")
            'End If
            If Request.QueryString("proyectoId") IsNot Nothing Then
                hfidProyecto.Value = Request.QueryString("ProyectoId").ToString
            End If
            If Request.QueryString("trabajoId") IsNot Nothing Then
                hfIdTrabajo.Value = Int64.Parse(Request.QueryString("trabajoId").ToString)
            Else
                Response.Redirect("../OP_Cualitativo/Trabajos.aspx")
            End If
            If Request.QueryString("py") IsNot Nothing Then
                lnkProyecto.PostBackUrl = "TrabajosCualitativos.aspx?ProyectoId=" & hfidProyecto.Value
                btnNuevo.Visible = True
            Else
                lnkProyecto.PostBackUrl = "../OP_Cualitativo/Trabajos.aspx"
                btnNuevo.Visible = False
            End If

            CargarLabelTrabajo()
            CargarDepartamentos()
            CargarModeradores()
            CargarInHome()
        End If
    End Sub

    Private Sub gvInHome_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvInHome.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If CType(e.Row.DataItem, OP_MuestraTrabajosCuali_InHomeGet_Result).IdEstado = 1 Then
                DirectCast(e.Row.FindControl("chbEfectiva"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("chbCaida"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("chbAnulada"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("imgReemplazar"), ImageButton).Enabled = False
            ElseIf CType(e.Row.DataItem, OP_MuestraTrabajosCuali_InHomeGet_Result).IdEstado = 2 Then
                DirectCast(e.Row.FindControl("chbEfectiva"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbCaida"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("chbAnulada"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("imgReemplazar"), ImageButton).Enabled = True
            ElseIf CType(e.Row.DataItem, OP_MuestraTrabajosCuali_InHomeGet_Result).IdEstado = 3 Then
                DirectCast(e.Row.FindControl("chbEfectiva"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbCaida"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbAnulada"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("imgReemplazar"), ImageButton).Enabled = False
            ElseIf CType(e.Row.DataItem, OP_MuestraTrabajosCuali_InHomeGet_Result).IdEstado = 4 Then
                DirectCast(e.Row.FindControl("chbEfectiva"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("chbCaida"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbAnulada"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("imgReemplazar"), ImageButton).Enabled = False
            End If
        End If

    End Sub

    Private Sub gvSesiones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvInHome.RowCommand
        If e.CommandName = "Reemplazar" Then
            pnlNewInHome.Visible = True
            pnlDetalle.Visible = False
            hfTipo.Value = "1"
            Dim oCampo As New CoreProject.CampoCualitativo
            Dim lstInHome = oCampo.ObtenerInHomexID(Int64.Parse(Me.gvInHome.DataKeys(CInt(e.CommandArgument))("Id")))
            hfNumeroInHome.Value = lstInHome(0).Numero
            hfIdTrabajo.Value = lstInHome(0).TrabajoId
            ddlDepartamento.SelectedValue = lstInHome(0).DepartamentoId
            CargarCiudades()
            ddlCiudad.SelectedValue = lstInHome(0).CiudadId
            txtFecha.Text = lstInHome(0).Fecha
            txtHora.Text = lstInHome(0).Hora.ToString
            ddlModerador.SelectedValue = lstInHome(0).IdModerador
            txtGrupo.Text = lstInHome(0).GrupoObjetivo
            txtCaracteristica.Text = lstInHome(0).Caracteristicas
            hfEstado.Value = lstInHome(0).IdEstado
        End If

        If e.CommandName = "Detalle" Then
            hfIdInHome.Value = Int64.Parse(Me.gvInHome.DataKeys(CInt(e.CommandArgument))("Id"))
            pnlDetalle.Visible = True
            CargarDetalles()
        End If

    End Sub
    Private Sub gvInHome_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvInHome.PageIndexChanging
        gvInHome.PageIndex = e.NewPageIndex
        CargarInHome()
    End Sub

    Sub CargarLabelTrabajo()
        Dim oTrabajo As New Trabajo
        lblTrabajo.Text = oTrabajo.obtenerXId(hfIdTrabajo.Value).JobBook & " " & oTrabajo.obtenerXId(hfIdTrabajo.Value).NombreTrabajo
    End Sub
    Sub CargarDepartamentos()
        Dim oCoordCampo As New CoordinacionCampo
        Dim list = (From ldep In oCoordCampo.ObtenerDepartamentos()
                    Select iddep = ldep.DivDeptoMunicipio, nomdep = ldep.DivDeptoNombre).Distinct.ToList
        ddlDepartamento.DataSource = list
        ddlDepartamento.DataValueField = "iddep"
        ddlDepartamento.DataTextField = "nomdep"
        ddlDepartamento.DataBind()
    End Sub

    Sub CargarCiudades()
        Dim oCoordCampo As New CoordinacionCampo
        Dim listciudades = (From ldep In oCoordCampo.ObtenerCiudades(ddlDepartamento.SelectedValue)
                            Select idmuni = ldep.DivMunicipio, nommuni = ldep.DivMuniNombre)
        ddlCiudad.DataSource = listciudades
        ddlCiudad.DataValueField = "idmuni"
        ddlCiudad.DataTextField = "nommuni"
        ddlCiudad.DataBind()
    End Sub

    Public Sub CargarModeradores()
        Dim oCampo As New CoreProject.CampoCualitativo
        ddlModerador.DataSource = oCampo.ObtenerModeradores
        ddlModerador.DataValueField = "Id"
        ddlModerador.DataTextField = "Nombre"
        ddlModerador.DataBind()
        Me.ddlModerador.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})

    End Sub
    Sub CargarInHome()
        Dim o As New CoreProject.CampoCualitativo
        gvInHome.DataSource = o.ObtenerInHomexTrabajo(hfIdTrabajo.Value)
        gvInHome.DataBind()
    End Sub

    Sub CargarDetalles()
        Dim o As New CoreProject.CampoCualitativo
        gvDetalles.DataSource = o.ObtenerLogInHome(hfIdInHome.Value)
        gvDetalles.DataBind()
    End Sub

    Sub limpiar()
        ddlDepartamento.ClearSelection()
        ddlCiudad.ClearSelection()
        txtFecha.Text = ""
        txtHora.Text = ""
        ddlModerador.ClearSelection()
        txtGrupo.Text = ""
        txtCaracteristica.Text = ""
        txtObservacion.Text = ""
        hfIdInHome.Value = "0"
        hfNumeroInHome.Value = "0"
        hfEstado.Value = "0"
        hfTipo.Value = "0"
        pnlNewInHome.Visible = False
        pnlDetalle.Visible = False
    End Sub
    Protected Sub Guardar()
        Dim oInHome As New CoreProject.CampoCualitativo
        Dim itemInHome As New OP_MuestraTrabajosCuali_InHome
        Dim eLog As New OP_LogInHomeCuali
        Dim InHome = oInHome.ObtenerInHomexTrabajo(hfIdTrabajo.Value)
        Dim maximo = (From x In InHome
                      Select x.Numero).Max()

        If hfTipo.Value = 1 Then
            itemInHome.Numero = hfNumeroInHome.Value
        Else
            itemInHome.Numero = maximo + 1
        End If
        itemInHome.TrabajoId = hfIdTrabajo.Value
        itemInHome.CiudadId = ddlCiudad.SelectedValue
        itemInHome.Cantidad = txtCantidad.Text
        itemInHome.Fecha = txtFecha.Text
        itemInHome.Hora = CDate(txtHora.Text).TimeOfDay
        itemInHome.Moderador = ddlModerador.SelectedValue
        itemInHome.GrupoObjetivo = Server.HtmlDecode(txtGrupo.Text)
        itemInHome.Caracteristicas = Server.HtmlDecode(txtCaracteristica.Text)
        itemInHome.Estado = "1"
        hfIdInHome.Value = oInHome.GuardarMuestraXInHome(itemInHome)

        eLog.IdSesion = hfIdInHome.Value
        eLog.Fecha = DateTime.Now()
        eLog.Usuario = Session("IDUsuario").ToString
        eLog.Estado = "1"
        If hfTipo.Value = 1 Then
            eLog.Observacion = "In Home Visit Creada de Reemplazo"
        Else
            eLog.Observacion = "In Home Visit Creada"
        End If
        oInHome.GuardarLogInHome(eLog)

        limpiar()
        CargarInHome()
    End Sub

    Protected Sub ddlDepartamento_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDepartamento.SelectedIndexChanged
        CargarCiudades()
    End Sub
    Sub chkCaida_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim checkbox As CheckBox = sender
        Dim row As GridViewRow = checkbox.NamingContainer
        Dim index As Integer = row.RowIndex
        hfIdInHome.Value = Int64.Parse(Me.gvInHome.DataKeys(row.RowIndex).Item("Id"))
        If checkbox.Checked = True Then
            DirectCast(row.FindControl("chbEfectiva"), CheckBox).Enabled = False
            DirectCast(row.FindControl("chbCaida"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbAnulada"), CheckBox).Enabled = False
            DirectCast(row.FindControl("imgReemplazar"), ImageButton).Enabled = True

            hfEstado.Value = "2"

        Else
            DirectCast(row.FindControl("chbEfectiva"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbCaida"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbAnulada"), CheckBox).Enabled = True
            DirectCast(row.FindControl("imgReemplazar"), ImageButton).Enabled = False

            hfEstado.Value = "1"
        End If

    End Sub

    Sub chkAnulada_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim checkbox As CheckBox = sender
        Dim row As GridViewRow = checkbox.NamingContainer
        Dim index As Integer = row.RowIndex
        hfIdInHome.Value = Int64.Parse(Me.gvInHome.DataKeys(row.RowIndex).Item("Id"))
        If checkbox.Checked = True Then
            DirectCast(row.FindControl("chbEfectiva"), CheckBox).Enabled = False
            DirectCast(row.FindControl("chbCaida"), CheckBox).Enabled = False
            DirectCast(row.FindControl("chbAnulada"), CheckBox).Enabled = True
            DirectCast(row.FindControl("imgReemplazar"), ImageButton).Enabled = False

            hfEstado.Value = "3"
        Else
            DirectCast(row.FindControl("chbEfectiva"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbCaida"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbAnulada"), CheckBox).Enabled = True
            DirectCast(row.FindControl("imgReemplazar"), ImageButton).Enabled = False

            hfEstado.Value = "1"
        End If
    End Sub

    Sub chkEfectiva_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim checkbox As CheckBox = sender
        Dim row As GridViewRow = checkbox.NamingContainer
        Dim index As Integer = row.RowIndex
        hfIdInHome.Value = Int64.Parse(Me.gvInHome.DataKeys(row.RowIndex).Item("Id"))
        If checkbox.Checked = True Then
            DirectCast(row.FindControl("chbEfectiva"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbCaida"), CheckBox).Enabled = False
            DirectCast(row.FindControl("chbAnulada"), CheckBox).Enabled = False
            DirectCast(row.FindControl("imgReemplazar"), ImageButton).Enabled = False

            hfEstado.Value = "4"
            ShowNotification("La In Home Visit ha sido marcada como Efectiva", ShowNotifications.InfoNotification)
        Else
            DirectCast(row.FindControl("chbEfectiva"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbCaida"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbAnulada"), CheckBox).Enabled = True
            DirectCast(row.FindControl("imgReemplazar"), ImageButton).Enabled = False

            hfEstado.Value = "1"
            ShowNotification("La In Home Visit estaba marcada como Efectiva, queda como Activa de nuevo", ShowNotifications.InfoNotification)
        End If

        InHomeEfectivas()
    End Sub

    Sub InHomeEfectivas()
        Dim o As New CoreProject.CampoCualitativo
        Dim eLog As New OP_LogInHomeCuali
        Dim ent As New OP_MuestraTrabajosCuali_InHome
        Dim Observacion As String = Nothing
        ent = o.ObtenerMuestraCualiEntrevistasxInHome(hfIdInHome.Value)
        ent.Estado = hfEstado.Value
        o.GuardarMuestraXInHome(ent)

        eLog.IdSesion = hfIdInHome.Value
        eLog.Fecha = DateTime.Now()
        eLog.Usuario = Session("IDUsuario").ToString
        eLog.Estado = hfEstado.Value

        If hfEstado.Value = "4" Then
            Observacion = "La In Home Visit es Efectiva"
        ElseIf hfEstado.Value = "1" Then
            Observacion = "La In Home Visit estaba marcada como Efectiva, queda como Activa de nuevo"
        End If
        eLog.Observacion = Observacion
        o.GuardarLogInHome(eLog)

        limpiar()
        CargarInHome()
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Me.pnlNewInHome.Visible = True
        Me.pnlDetalle.Visible = False
    End Sub
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        If ddlDepartamento.SelectedValue = "-1" Then
            ShowNotification("Seleccione el Departamento", ShowNotifications.ErrorNotification)
            ddlDepartamento.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlCiudad.SelectedValue = "-1" Then
            ShowNotification("Seleccione la Ciudad", ShowNotifications.ErrorNotification)
            ddlCiudad.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If txtCantidad.Text = "" Then
            ShowNotification("Indique el Número de Participantes para el In Home", ShowNotifications.ErrorNotification)
            txtGrupo.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If Not (IsDate(txtFecha.Text)) Then
            ShowNotification("Escriba la fecha de Inicio", ShowNotifications.ErrorNotification)
            txtFecha.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If txtHora.Text = "" Then
            ShowNotification("Escriba la Hora de la Sesión", ShowNotifications.ErrorNotification)
            txtHora.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlModerador.SelectedValue = "-1" Then
            ShowNotification("Seleccione el Moderador", ShowNotifications.ErrorNotification)
            ddlModerador.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If txtGrupo.Text = "" Then
            ShowNotification("Indique el Grupo Objetivo de la Sesión", ShowNotifications.ErrorNotification)
            txtGrupo.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If txtCaracteristica.Text = "" Then
            ShowNotification("Indique las caracteristicas especiales de la Sesión", ShowNotifications.ErrorNotification)
            txtCaracteristica.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        Guardar()
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        limpiar()
        CargarInHome()
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim o As New CoreProject.CampoCualitativo
        Dim eLog As New OP_LogInHomeCuali
        Dim ent As New OP_MuestraTrabajosCuali_InHome

        ent = o.ObtenerMuestraCualiEntrevistasxInHome(hfIdInHome.Value)
        ent.Estado = hfEstado.Value
        o.GuardarMuestraXInHome(ent)

        eLog.IdSesion = hfIdInHome.Value
        eLog.Fecha = DateTime.Now()
        eLog.Usuario = Session("IDUsuario").ToString
        eLog.Estado = hfEstado.Value
        eLog.Observacion = txtObservacion.Text
        o.GuardarLogInHome(eLog)

        limpiar()
        CargarInHome()
    End Sub

    Protected Sub btnvolver_Click(sender As Object, e As EventArgs) Handles btnvolver.Click
        pnlDetalle.Visible = False
        limpiar()
        CargarInHome()
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        limpiar()
        CargarInHome()
    End Sub
End Class