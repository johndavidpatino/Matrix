Imports CoreProject
Imports CoreProject.OP
Imports WebMatrix.Util

Public Class DistribucionEntrevistas
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
            CargarEntrevistas()
        End If
    End Sub

    Private Sub gvEntrevistas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvEntrevistas.RowCommand
        If e.CommandName = "Seleccionar" Then
            pnlDistribucion.Visible = True
            pnlNewEntrevista.Visible = False
            pnlDetalle.Visible = False
            hfIdEntrevista.Value = Int64.Parse(Me.gvEntrevistas.DataKeys(CInt(e.CommandArgument))("Id"))
            CargarEntrevistasDistribucion()
        End If
    End Sub

    Private Sub gvEntrevistas_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvEntrevistas.PageIndexChanging
        gvEntrevistas.PageIndex = e.NewPageIndex
        CargarEntrevistas()
    End Sub


    Private Sub gvDistribucion_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvDistribucion.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If CType(e.Row.DataItem, OP_EntrevistasCuali_DistribucionGet_Result).IdEstado = 1 Then
                DirectCast(e.Row.FindControl("chbEfectiva"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("chbCaida"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("chbAnulada"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("imgReemplazar"), ImageButton).Enabled = False
            ElseIf CType(e.Row.DataItem, OP_EntrevistasCuali_DistribucionGet_Result).IdEstado = 2 Then
                DirectCast(e.Row.FindControl("chbEfectiva"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbCaida"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("chbAnulada"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("imgReemplazar"), ImageButton).Enabled = True
            ElseIf CType(e.Row.DataItem, OP_EntrevistasCuali_DistribucionGet_Result).IdEstado = 3 Then
                DirectCast(e.Row.FindControl("chbEfectiva"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbCaida"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbAnulada"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("imgReemplazar"), ImageButton).Enabled = False
            ElseIf CType(e.Row.DataItem, OP_EntrevistasCuali_DistribucionGet_Result).IdEstado = 4 Then
                DirectCast(e.Row.FindControl("chbEfectiva"), CheckBox).Enabled = True
                DirectCast(e.Row.FindControl("chbCaida"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("chbAnulada"), CheckBox).Enabled = False
                DirectCast(e.Row.FindControl("imgReemplazar"), ImageButton).Enabled = False
            End If
        End If

    End Sub

    Private Sub gvDistribucion_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDistribucion.RowCommand
        If e.CommandName = "Reemplazar" Then
            pnlNewEntrevista.Visible = True
            pnlDetalle.Visible = False
            hfTipo.Value = "1"
            Dim oCampo As New CoreProject.CampoCualitativo
            Dim lstDistribucion = oCampo.ObtenerEntrevistasDistribucionxIdDistribucion(Int64.Parse(Me.gvDistribucion.DataKeys(CInt(e.CommandArgument))("Id")))
            hfIdEntrevista.Value = lstDistribucion(0).IdEntrevista
            hfNumero.Value = lstDistribucion(0).Numero
            hfIdTrabajo.Value = lstDistribucion(0).TrabajoId
            txtGrupoObjetivo.Text = lstDistribucion(0).GrupoObjetivo
            ddlDepartamento.SelectedValue = lstDistribucion(0).DepartamentoId
            CargarCiudades()
            ddlCiudad.SelectedValue = lstDistribucion(0).CiudadId
            txtFechaInicio.Text = lstDistribucion(0).FechaInicio
            txtFechaFin.Text = lstDistribucion(0).FechaFin
            ddlModerador.SelectedValue = lstDistribucion(0).IdModerador
            hfEstado.Value = lstDistribucion(0).IdEstado
        End If

        If e.CommandName = "Detalle" Then
            hfIdDistribucion.Value = Int64.Parse(Me.gvDistribucion.DataKeys(CInt(e.CommandArgument))("Id"))
            pnlDetalle.Visible = True
            CargarDetalles()
        End If

    End Sub
    Private Sub gvDistribucion_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDistribucion.PageIndexChanging
        gvDistribucion.PageIndex = e.NewPageIndex
        CargarEntrevistasDistribucion()
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

    Sub CargarEntrevistas()
        Dim o As New CoreProject.CampoCualitativo
        gvEntrevistas.DataSource = o.ObtenerEntrevistasxTrabajo(hfIdTrabajo.Value)
        gvEntrevistas.DataBind()
    End Sub

    Sub CargarEntrevistasDistribucion()
        Dim o As New CoreProject.CampoCualitativo
        gvDistribucion.DataSource = o.ObtenerEntrevistasDistribucionxIdEntrevista(hfIdEntrevista.Value)
        gvDistribucion.DataBind()
    End Sub

    Sub CargarDetalles()
        Dim o As New CoreProject.CampoCualitativo
        gvDetalles.DataSource = o.ObtenerLogEntrevistas(hfIdDistribucion.Value)
        gvDetalles.DataBind()
    End Sub

    Sub limpiar()
        txtGrupoObjetivo.Text = ""
        ddlDepartamento.ClearSelection()
        ddlCiudad.ClearSelection()
        txtFechaInicio.Text = ""
        txtFechaFin.Text = ""
        ddlModerador.ClearSelection()
        hfIdDistribucion.Value = "0"
        hfNumero.Value = "0"
        hfEstado.Value = "0"
        hfTipo.Value = "0"
        pnlNewEntrevista.Visible = False
        pnlDetalle.Visible = False
    End Sub
    Protected Sub Guardar()
        Dim oDistribucion As New CoreProject.CampoCualitativo
        Dim itemDistribucion As New OP_EntrevistasCuali_Distribucion
        Dim eLog As New OP_LogEntrevistasCuali
        Dim Distribucion = oDistribucion.ObtenerEntrevistasDistribucionxIdEntrevista(hfIdEntrevista.Value)
        itemDistribucion.IdEntrevista = hfIdEntrevista.Value

        Dim maximo = (From x In Distribucion
                      Select x.Numero).Max()

        If hfTipo.Value = 1 Then
            itemDistribucion.Numero = hfNumero.Value
        Else
            itemDistribucion.Numero = maximo + 1
        End If

        itemDistribucion.TrabajoId = hfIdTrabajo.Value
        itemDistribucion.GrupoObjetivo = txtGrupoObjetivo.Text
        itemDistribucion.CiudadId = ddlCiudad.SelectedValue
        itemDistribucion.Cantidad = "1"
        itemDistribucion.FechaInicio = txtFechaInicio.Text
        itemDistribucion.FechaFin = txtFechaFin.Text
        itemDistribucion.Moderador = ddlModerador.SelectedValue
        itemDistribucion.Estado = "1"
        hfIdDistribucion.Value = oDistribucion.GuardarDistribucionEntrevistas(itemDistribucion)

        eLog.IdDistribucion = hfIdDistribucion.Value
        eLog.IdEntrevista = hfIdEntrevista.Value
        eLog.Fecha = DateTime.Now()
        eLog.Usuario = Session("IDUsuario").ToString
        eLog.Estado = "1"
        If hfTipo.Value = 1 Then
            eLog.Observacion = "Entrevista Creada de Reemplazo"
        Else
            eLog.Observacion = "Entrevista Creada"
        End If
        oDistribucion.GuardarLogEntrevistas(eLog)

        limpiar()
        CargarEntrevistasDistribucion()
    End Sub

    Protected Sub ddlDepartamento_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDepartamento.SelectedIndexChanged
        CargarCiudades()
    End Sub
    Sub chkCaida_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim checkbox As CheckBox = sender
        Dim row As GridViewRow = checkbox.NamingContainer
        Dim index As Integer = row.RowIndex
        hfIdDistribucion.Value = Int64.Parse(Me.gvDistribucion.DataKeys(row.RowIndex).Item("Id"))
        hfIdEntrevista.Value = Int64.Parse(Me.gvDistribucion.DataKeys(row.RowIndex).Item("IdEntrevista"))
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
        hfIdDistribucion.Value = Int64.Parse(Me.gvDistribucion.DataKeys(row.RowIndex).Item("Id"))
        hfIdEntrevista.Value = Int64.Parse(Me.gvDistribucion.DataKeys(row.RowIndex).Item("IdEntrevista"))
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
        hfIdDistribucion.Value = Int64.Parse(Me.gvDistribucion.DataKeys(row.RowIndex).Item("Id"))
        hfIdEntrevista.Value = Int64.Parse(Me.gvDistribucion.DataKeys(row.RowIndex).Item("IdEntrevista"))
        If checkbox.Checked = True Then
            DirectCast(row.FindControl("chbEfectiva"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbCaida"), CheckBox).Enabled = False
            DirectCast(row.FindControl("chbAnulada"), CheckBox).Enabled = False
            DirectCast(row.FindControl("imgReemplazar"), ImageButton).Enabled = False

            hfEstado.Value = "4"
            ShowNotification("La Entrevista ha sido marcada como Efectiva", ShowNotifications.InfoNotification)
        Else
            DirectCast(row.FindControl("chbEfectiva"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbCaida"), CheckBox).Enabled = True
            DirectCast(row.FindControl("chbAnulada"), CheckBox).Enabled = True
            DirectCast(row.FindControl("imgReemplazar"), ImageButton).Enabled = False

            hfEstado.Value = "1"
            ShowNotification("La Entrevista estaba marcada como Efectiva, queda como Activa de nuevo", ShowNotifications.InfoNotification)
        End If

        EntrevistasEfectivas()
    End Sub

    Sub EntrevistasEfectivas()
        Dim o As New CoreProject.CampoCualitativo
        Dim eLog As New OP_LogEntrevistasCuali
        Dim ent As New OP_EntrevistasCuali_Distribucion
        Dim Observacion As String = Nothing
        ent = o.ObtenerEntrevistasDistribucionxId(hfIdDistribucion.Value)
        ent.Estado = hfEstado.Value
        o.GuardarDistribucionEntrevistas(ent)

        eLog.IdDistribucion = hfIdDistribucion.Value
        eLog.IdEntrevista = hfIdEntrevista.Value
        eLog.Fecha = DateTime.Now()
        eLog.Usuario = Session("IDUsuario").ToString
        eLog.Estado = hfEstado.Value

        If hfEstado.Value = "4" Then
            Observacion = "La Entrevista es Efectiva"
        ElseIf hfEstado.Value = "1" Then
            Observacion = "La Entrevista estaba marcada como Efectiva, queda como Activa de nuevo"
        End If
        eLog.Observacion = Observacion
        o.GuardarLogEntrevistas(eLog)

        limpiar()
        CargarEntrevistasDistribucion()
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Me.pnlNewEntrevista.Visible = True
        Me.pnlDetalle.Visible = False
    End Sub
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        If txtGrupoObjetivo.Text = "" Then
            ShowNotification("Indique el Grupo Objetivo de la Entrevista", ShowNotifications.ErrorNotification)
            txtGrupoObjetivo.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

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

        If Not (IsDate(txtFechaInicio.Text)) Then
            ShowNotification("Escriba la fecha de Inicio", ShowNotifications.ErrorNotification)
            txtFechaInicio.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If Not (IsDate(txtFechaFin.Text)) Then
            ShowNotification("Escriba la fecha de Finalización", ShowNotifications.ErrorNotification)
            txtFechaFin.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlModerador.SelectedValue = "-1" Then
            ShowNotification("Seleccione el Moderador", ShowNotifications.ErrorNotification)
            ddlModerador.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        Guardar()
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        limpiar()
        CargarEntrevistasDistribucion()
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim o As New CoreProject.CampoCualitativo
        Dim eLog As New OP_LogEntrevistasCuali
        Dim ent As New OP_EntrevistasCuali_Distribucion

        ent = o.ObtenerEntrevistasDistribucionxId(hfIdDistribucion.Value)
        ent.Estado = hfEstado.Value
        o.GuardarDistribucionEntrevistas(ent)

        eLog.IdDistribucion = hfIdDistribucion.Value
        eLog.IdEntrevista = hfIdEntrevista.Value
        eLog.Fecha = DateTime.Now()
        eLog.Usuario = Session("IDUsuario").ToString
        eLog.Estado = hfEstado.Value
        eLog.Observacion = txtObservacion.Text
        o.GuardarLogEntrevistas(eLog)

        limpiar()
        CargarEntrevistasDistribucion()
    End Sub

    Protected Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        pnlDistribucion.Visible = False
        hfIdEntrevista.Value = "0"
        limpiar()
        CargarEntrevistas()
    End Sub

    Protected Sub btnvolver_Click(sender As Object, e As EventArgs) Handles btnvolver.Click
        pnlDetalle.Visible = False
        limpiar()
        CargarEntrevistasDistribucion()
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        limpiar()
        CargarEntrevistasDistribucion()
    End Sub


End Class