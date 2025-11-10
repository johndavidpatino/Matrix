Imports CoreProject
Imports CoreProject.OP
Imports WebMatrix.Util

Public Class Sesiones
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
            CargarSesiones()
            End If
    End Sub

    Private Sub gvSesiones_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvSesiones.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If CType(e.Row.DataItem, OP_MuestraTrabajosCuali_SesionesGet_Result).IdEstado = 1 Then
                DirectCast(e.Row.FindControl("chbEfectiva"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("chbIncompleta"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("chbCaida"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("chbAnulada"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("imgReemplazar"), ImageButton).Enabled = False
                DirectCast(e.Row.FindControl("imgCompletar"), ImageButton).Enabled = False
            ElseIf CType(e.Row.DataItem, OP_MuestraTrabajosCuali_SesionesGet_Result).IdEstado = 2 Then
                DirectCast(e.Row.FindControl("chbEfectiva"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbIncompleta"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbCaida"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("chbAnulada"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("imgReemplazar"), ImageButton).Enabled = True
                DirectCast(e.Row.FindControl("imgCompletar"), ImageButton).Enabled = False
            ElseIf CType(e.Row.DataItem, OP_MuestraTrabajosCuali_SesionesGet_Result).IdEstado = 3 Then
                DirectCast(e.Row.FindControl("chbEfectiva"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbIncompleta"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbCaida"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbAnulada"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("imgReemplazar"), ImageButton).Enabled = False
                DirectCast(e.Row.FindControl("imgCompletar"), ImageButton).Enabled = False
            ElseIf CType(e.Row.DataItem, OP_MuestraTrabajosCuali_SesionesGet_Result).IdEstado = 4 Then
                DirectCast(e.Row.FindControl("chbEfectiva"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("chbIncompleta"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbCaida"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbAnulada"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("imgReemplazar"), ImageButton).Enabled = False
                DirectCast(e.Row.FindControl("imgCompletar"), ImageButton).Enabled = False
            ElseIf CType(e.Row.DataItem, OP_MuestraTrabajosCuali_SesionesGet_Result).IdEstado = 5 Then
                DirectCast(e.Row.FindControl("chbEfectiva"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbIncompleta"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("chbCaida"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbAnulada"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("imgReemplazar"), ImageButton).Enabled = False
                DirectCast(e.Row.FindControl("imgCompletar"), ImageButton).Enabled = True
            End If
        End If

    End Sub

    Private Sub gvSesiones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvSesiones.RowCommand
        If e.CommandName = "Reemplazar" Then
            pnlNewSesion.Visible = True
            pnlDetalle.Visible = False
            hfTipo.Value = "1"
            Dim oCampo As New CoreProject.CampoCualitativo
            Dim lstSesiones = oCampo.ObtenerSesionesxID(Int64.Parse(Me.gvSesiones.DataKeys(CInt(e.CommandArgument))("Id")))
            hfNumeroSesion.Value = lstSesiones(0).Numero
            hfIdTrabajo.Value = lstSesiones(0).TrabajoId
            ddlDepartamento.SelectedValue = lstSesiones(0).DepartamentoId
            CargarCiudades()
            ddlCiudad.SelectedValue = lstSesiones(0).CiudadId
            txtCantidad.Text = lstSesiones(0).Cantidad
            txtFecha.Text = lstSesiones(0).Fecha
            txtHora.Text = lstSesiones(0).Hora.ToString
            ddlModerador.SelectedValue = lstSesiones(0).IdModerador
            txtGrupo.Text = lstSesiones(0).GrupoObjetivo
            txtCaracteristica.Text = lstSesiones(0).Caracteristicas
            hfEstado.Value = lstSesiones(0).IdEstado
        End If

        If e.CommandName = "Completar" Then
            pnlNewSesion.Visible = True
            pnlDetalle.Visible = False
            hfTipo.Value = "2"
            Dim oCampo As New CoreProject.CampoCualitativo
            Dim lstSesiones = oCampo.ObtenerSesionesxID(Int64.Parse(Me.gvSesiones.DataKeys(CInt(e.CommandArgument))("Id")))
            hfNumeroSesion.Value = lstSesiones(0).Numero
            hfIdTrabajo.Value = lstSesiones(0).TrabajoId
            ddlDepartamento.SelectedValue = lstSesiones(0).DepartamentoId
            CargarCiudades()
            ddlCiudad.SelectedValue = lstSesiones(0).CiudadId
            txtCantidad.Text = lstSesiones(0).Cantidad
            txtFecha.Text = lstSesiones(0).Fecha
            txtHora.Text = lstSesiones(0).Hora.ToString
            ddlModerador.SelectedValue = lstSesiones(0).IdModerador
            txtGrupo.Text = lstSesiones(0).GrupoObjetivo
            txtCaracteristica.Text = lstSesiones(0).Caracteristicas
            hfEstado.Value = lstSesiones(0).IdEstado
        End If

        If e.CommandName = "Detalle" Then
            hfIdSesion.Value = Int64.Parse(Me.gvSesiones.DataKeys(CInt(e.CommandArgument))("Id"))
            pnlDetalle.Visible = True
            CargarDetalles()
        End If

    End Sub
    Private Sub gvSesiones_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSesiones.PageIndexChanging
        gvSesiones.PageIndex = e.NewPageIndex
        CargarSesiones()
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
    Sub CargarSesiones()
        Dim o As New CoreProject.CampoCualitativo
        gvSesiones.DataSource = o.ObtenerSesionesxTrabajo(hfIdTrabajo.Value)
        gvSesiones.DataBind()
    End Sub

    Sub CargarDetalles()
        Dim o As New CoreProject.CampoCualitativo
        gvDetalles.DataSource = o.ObtenerLogSesiones(hfIdSesion.Value)
        gvDetalles.DataBind()
    End Sub

    Sub limpiar()
        ddlDepartamento.ClearSelection()
        ddlCiudad.ClearSelection()
        txtCantidad.Text = ""
        txtFecha.Text = ""
        txtHora.Text = ""
        ddlModerador.ClearSelection()
        txtGrupo.Text = ""
        txtCaracteristica.Text = ""
        txtObservacion.Text = ""
        txtIncompleta.Text = ""
        hfIdSesion.Value = "0"
        hfNumeroSesion.Value = "0"
        hfEstado.Value = "0"
        hfTipo.Value = "0"
        hfCantidad.Value = "0"
        pnlNewSesion.Visible = False
        pnlDetalle.Visible = False
        lblIncompleta.Visible = False
        txtIncompleta.Visible = False
    End Sub

    Protected Sub Guardar()
        Dim oSesion As New CoreProject.CampoCualitativo
        Dim itemSesion As New OP_MuestraTrabajosCuali_Sesiones
        Dim eLog As New OP_LogSesionesCuali
        Dim Sesiones = oSesion.ObtenerSesionesxTrabajo(hfIdTrabajo.Value)
        Dim maximo = (From x In Sesiones
                      Select x.Numero).Max()

        If hfEstado.Value = "5" And hfTipo.Value <> 2 Then
            itemSesion = oSesion.ObtenerMuestraCualiEntrevistasxSesion(hfIdSesion.Value)
            itemSesion.CiudadId = ddlCiudad.SelectedValue
            itemSesion.Cantidad = txtCantidad.Text
            itemSesion.Fecha = txtFecha.Text
            itemSesion.Hora = CDate(txtHora.Text).TimeOfDay
            itemSesion.Moderador = ddlModerador.SelectedValue
            itemSesion.GrupoObjetivo = Server.HtmlDecode(txtGrupo.Text)
            itemSesion.Caracteristicas = Server.HtmlDecode(txtCaracteristica.Text)
            itemSesion.Estado = hfEstado.Value
            oSesion.GuardarMuestraXSesiones(itemSesion)

            eLog.IdSesion = hfIdSesion.Value
            eLog.Fecha = DateTime.Now()
            eLog.Usuario = Session("IDUsuario").ToString
            eLog.Estado = hfEstado.Value
            eLog.Observacion = txtIncompleta.Text
            eLog.Cantidad = txtCantidad.Text
            oSesion.GuardarLogSesiones(eLog)

            ShowNotification("La Sesión ha sido marcada como Incompleta!", ShowNotifications.InfoNotification)
        Else
            If hfTipo.Value >= 1 Then
                itemSesion.Numero = hfNumeroSesion.Value
            Else
                itemSesion.Numero = maximo + 1
            End If
            itemSesion.TrabajoId = hfIdTrabajo.Value
            itemSesion.CiudadId = ddlCiudad.SelectedValue
            itemSesion.Cantidad = txtCantidad.Text
            itemSesion.Fecha = txtFecha.Text
            itemSesion.Hora = CDate(txtHora.Text).TimeOfDay
            itemSesion.Moderador = ddlModerador.SelectedValue
            itemSesion.GrupoObjetivo = Server.HtmlDecode(txtGrupo.Text)
            itemSesion.Caracteristicas = Server.HtmlDecode(txtCaracteristica.Text)
            itemSesion.Estado = "1"
            hfIdSesion.Value = oSesion.GuardarMuestraXSesiones(itemSesion)

            eLog.IdSesion = hfIdSesion.Value
            eLog.Fecha = DateTime.Now()
            eLog.Usuario = Session("IDUsuario").ToString
            eLog.Estado = "1"
            If hfTipo.Value = 1 Then
                eLog.Observacion = "Sesion Creada de Reemplazo"
            ElseIf hfTipo.Value = 2 Then
                eLog.Observacion = "Sesion Creada para Completar otra que se realizó Incompleta"
            Else
                eLog.Observacion = "Sesion Creada"
            End If
            eLog.Cantidad = txtCantidad.Text
            oSesion.GuardarLogSesiones(eLog)
        End If

        limpiar()
        CargarSesiones()
    End Sub

    Protected Sub ddlDepartamento_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDepartamento.SelectedIndexChanged
        CargarCiudades()
    End Sub
    Sub chkCaida_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim checkbox As CheckBox = sender
        Dim row As GridViewRow = checkbox.NamingContainer
        Dim index As Integer = row.RowIndex
        Dim oCampo As New CoreProject.CampoCualitativo
        hfIdSesion.Value = Int64.Parse(Me.gvSesiones.DataKeys(row.RowIndex).Item("Id"))
        hfCantidad.Value = Int64.Parse(gvSesiones.Rows(row.RowIndex).Cells(2).Text)

        If checkbox.Checked = True Then
            DirectCast(row.FindControl("chbEfectiva"), CheckBox).Enabled = False
            DirectCast(row.FindControl("chbCaida"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbAnulada"), CheckBox).Enabled = False
            DirectCast(row.FindControl("imgReemplazar"), ImageButton).Enabled = True
            DirectCast(row.FindControl("imgCompletar"), ImageButton).Enabled = False

            hfEstado.Value = "2"

        Else
            DirectCast(row.FindControl("chbEfectiva"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbCaida"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbAnulada"), CheckBox).Enabled = True
            DirectCast(row.FindControl("imgReemplazar"), ImageButton).Enabled = False
            DirectCast(row.FindControl("imgCompletar"), ImageButton).Enabled = False

            hfEstado.Value = "1"
        End If

    End Sub

    Sub chkAnulada_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim checkbox As CheckBox = sender
        Dim row As GridViewRow = checkbox.NamingContainer
        Dim index As Integer = row.RowIndex
        Dim oCampo As New CoreProject.CampoCualitativo
        hfIdSesion.Value = Int64.Parse(Me.gvSesiones.DataKeys(row.RowIndex).Item("Id"))
        hfCantidad.Value = Int64.Parse(gvSesiones.Rows(row.RowIndex).Cells(2).Text)

        If checkbox.Checked = True Then
            DirectCast(row.FindControl("chbEfectiva"), CheckBox).Enabled = False
            DirectCast(row.FindControl("chbCaida"), CheckBox).Enabled = False
            DirectCast(row.FindControl("chbAnulada"), CheckBox).Enabled = True
            DirectCast(row.FindControl("imgReemplazar"), ImageButton).Enabled = False
            DirectCast(row.FindControl("imgCompletar"), ImageButton).Enabled = False

            hfEstado.Value = "3"
        Else
            DirectCast(row.FindControl("chbEfectiva"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbCaida"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbAnulada"), CheckBox).Enabled = True
            DirectCast(row.FindControl("imgReemplazar"), ImageButton).Enabled = False
            DirectCast(row.FindControl("imgCompletar"), ImageButton).Enabled = False

            hfEstado.Value = "1"
        End If
    End Sub

    Sub chkEfectiva_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim checkbox As CheckBox = sender
        Dim row As GridViewRow = checkbox.NamingContainer
        Dim index As Integer = row.RowIndex
        Dim oCampo As New CoreProject.CampoCualitativo
        hfIdSesion.Value = Int64.Parse(Me.gvSesiones.DataKeys(row.RowIndex).Item("Id"))
        hfCantidad.Value = Int64.Parse(gvSesiones.Rows(row.RowIndex).Cells(2).Text)

        If checkbox.Checked = True Then
            DirectCast(row.FindControl("chbEfectiva"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbCaida"), CheckBox).Enabled = False
            DirectCast(row.FindControl("chbAnulada"), CheckBox).Enabled = False
            DirectCast(row.FindControl("imgReemplazar"), ImageButton).Enabled = False
            DirectCast(row.FindControl("imgCompletar"), ImageButton).Enabled = False

            hfEstado.Value = "4"
            ShowNotification("La Sesión ha sido marcada como Efectiva", ShowNotifications.InfoNotification)
        Else
            DirectCast(row.FindControl("chbEfectiva"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbCaida"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbAnulada"), CheckBox).Enabled = True
            DirectCast(row.FindControl("imgReemplazar"), ImageButton).Enabled = False
            DirectCast(row.FindControl("imgCompletar"), ImageButton).Enabled = False

            hfEstado.Value = "1"
            ShowNotification("La Sesion estaba marcada como Efectiva, queda como Activa de nuevo", ShowNotifications.InfoNotification)
        End If

        SesionesEfectivas()
    End Sub

    Sub chkIncompleta_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim checkbox As CheckBox = sender
        Dim row As GridViewRow = checkbox.NamingContainer
        Dim index As Integer = row.RowIndex
        Dim oCampo As New CoreProject.CampoCualitativo
        Dim eLog As New OP_LogSesionesCuali
        Dim ent As New OP_MuestraTrabajosCuali_Sesiones
        hfIdSesion.Value = Int64.Parse(Me.gvSesiones.DataKeys(row.RowIndex).Item("Id"))
        hfCantidad.Value = Int64.Parse(gvSesiones.Rows(row.RowIndex).Cells(2).Text)

        If checkbox.Checked = True Then
            DirectCast(row.FindControl("chbEfectiva"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbCaida"), CheckBox).Enabled = False
            DirectCast(row.FindControl("chbAnulada"), CheckBox).Enabled = False
            DirectCast(row.FindControl("imgReemplazar"), ImageButton).Enabled = False
            DirectCast(row.FindControl("imgCompletar"), ImageButton).Enabled = True

            pnlNewSesion.Visible = True
            pnlDetalle.Visible = False
            lblIncompleta.Visible = True
            txtIncompleta.Visible = True
            Dim lstSesiones = oCampo.ObtenerSesionesxID(hfIdSesion.Value)
            hfNumeroSesion.Value = lstSesiones(0).Numero
            hfIdTrabajo.Value = lstSesiones(0).TrabajoId
            ddlDepartamento.SelectedValue = lstSesiones(0).DepartamentoId
            CargarCiudades()
            ddlCiudad.SelectedValue = lstSesiones(0).CiudadId
            txtCantidad.Text = lstSesiones(0).Cantidad
            txtFecha.Text = lstSesiones(0).Fecha
            txtHora.Text = lstSesiones(0).Hora.ToString
            ddlModerador.SelectedValue = lstSesiones(0).IdModerador
            txtGrupo.Text = lstSesiones(0).GrupoObjetivo
            txtCaracteristica.Text = lstSesiones(0).Caracteristicas
            hfEstado.Value = "5"

        Else
            DirectCast(row.FindControl("chbEfectiva"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbCaida"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbAnulada"), CheckBox).Enabled = True
            DirectCast(row.FindControl("imgReemplazar"), ImageButton).Enabled = False
            DirectCast(row.FindControl("imgCompletar"), ImageButton).Enabled = False

            hfEstado.Value = "1"

            ent = oCampo.ObtenerMuestraCualiEntrevistasxSesion(hfIdSesion.Value)
            ent.Estado = hfEstado.Value
            oCampo.GuardarMuestraXSesiones(ent)

            eLog.IdSesion = hfIdSesion.Value
            eLog.Fecha = DateTime.Now()
            eLog.Usuario = Session("IDUsuario").ToString
            eLog.Estado = hfEstado.Value
            eLog.Observacion = "La Sesion estaba marcada como Incompleta, queda como Activa de nuevo"
            eLog.Cantidad = hfCantidad.Value
            oCampo.GuardarLogSesiones(eLog)
            ShowNotification("La Sesion estaba marcada como Incompleta, queda como Activa de nuevo", ShowNotifications.InfoNotification)
            limpiar()
            CargarSesiones()

        End If

    End Sub

    Sub SesionesEfectivas()
        Dim o As New CoreProject.CampoCualitativo
        Dim eLog As New OP_LogSesionesCuali
        Dim ent As New OP_MuestraTrabajosCuali_Sesiones
        Dim Observacion As String = Nothing
        ent = o.ObtenerMuestraCualiEntrevistasxSesion(hfIdSesion.Value)
        ent.Estado = hfEstado.Value
        o.GuardarMuestraXSesiones(ent)

        eLog.IdSesion = hfIdSesion.Value
        eLog.Fecha = DateTime.Now()
        eLog.Usuario = Session("IDUsuario").ToString
        eLog.Estado = hfEstado.Value

        If hfEstado.Value = "4" Then
            Observacion = "La Sesion es Efectiva"
        ElseIf hfEstado.Value = "1" Then
            Observacion = "La Sesion estaba marcada como Efectiva, queda como Activa de nuevo"
        End If
        eLog.Observacion = Observacion
        eLog.Cantidad = hfCantidad.Value
        o.GuardarLogSesiones(eLog)

        limpiar()
        CargarSesiones()
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Me.pnlNewSesion.Visible = True
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
            ShowNotification("Indique el Número de Participantes para la Sesión", ShowNotifications.ErrorNotification)
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

        If txtIncompleta.Visible = True And txtIncompleta.Text = "" Then
            ShowNotification("Indique las razones por las que la Sesión es Incompleta", ShowNotifications.ErrorNotification)
            txtIncompleta.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        Guardar()
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim o As New CoreProject.CampoCualitativo
        Dim eLog As New OP_LogSesionesCuali
        Dim ent As New OP_MuestraTrabajosCuali_Sesiones

        ent = o.ObtenerMuestraCualiEntrevistasxSesion(hfIdSesion.Value)
        ent.Estado = hfEstado.Value
        o.GuardarMuestraXSesiones(ent)

        eLog.IdSesion = hfIdSesion.Value
        eLog.Fecha = DateTime.Now()
        eLog.Usuario = Session("IDUsuario").ToString
        eLog.Estado = hfEstado.Value
        eLog.Observacion = txtObservacion.Text
        eLog.Cantidad = hfCantidad.Value
        o.GuardarLogSesiones(eLog)

        limpiar()
        CargarSesiones()

    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        limpiar()
        CargarSesiones()
    End Sub
    Protected Sub btnvolver_Click(sender As Object, e As EventArgs) Handles btnvolver.Click
        pnlDetalle.Visible = False
        limpiar()
        CargarSesiones()
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        limpiar()
        CargarSesiones()
    End Sub
End Class