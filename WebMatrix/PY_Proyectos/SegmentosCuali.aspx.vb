Imports CoreProject
Imports WebMatrix.Util

Public Class SegmentosCuali
    Inherits System.Web.UI.Page


#Region "Metodos"
    Sub CargarLabelTrabajo()
        Dim oTrabajo As New Trabajo
        lblTrabajo.Text = oTrabajo.obtenerXId(hfIdTrabajo.Value).JobBook & " " & oTrabajo.obtenerXId(hfIdTrabajo.Value).NombreTrabajo
    End Sub
    Sub CargarSegmentos(ByVal trabajoId As Int64)
        Dim o As New CoreProject.SegmentosCuali
        gvSegmentos.DataSource = o.ObtenerSegmentosList(trabajoId).ToList
        gvSegmentos.DataBind()
    End Sub

    Sub CargarModeradores(ByVal SegmentoId As Int64)
        Dim o As New CoreProject.CampoCualitativo
        gvModeradores.DataSource = o.ObtenerModeradoresList(SegmentoId).ToList
        gvModeradores.DataBind()
    End Sub

    Sub CargarReclutadores(ByVal SegmentoId As Int64)
        Dim o As New CoreProject.CampoCualitativo
        gvReclutadores.DataSource = o.ObtenerReclutadoresList(SegmentoId).ToList
        gvReclutadores.DataBind()
    End Sub


    Sub CargarAyudasCuali()
        Dim oSegmentos As New CoreProject.SegmentosCuali
        Me.chbAyudas.DataSource = oSegmentos.ObtenerAyudasCuali
        Me.chbAyudas.DataTextField = "Ayuda"
        Me.chbAyudas.DataValueField = "id"
        Me.chbAyudas.DataBind()
    End Sub
    Sub CargarLugares()
        Dim oSegmentos As New CoreProject.SegmentosCuali
        Me.ddlLugar.DataSource = oSegmentos.ObtenerLugaresCuali
        Me.ddlLugar.DataTextField = "Lugar"
        Me.ddlLugar.DataValueField = "id"
        Me.ddlLugar.DataBind()
    End Sub

    Public Function ValidarFecha(ByVal txtFecha As String) As Boolean
        Dim dt As DateTime

        Dim blnFlag As Boolean = DateTime.TryParse(txtFecha, dt)

        If blnFlag Then
            Return True
        Else
            Return False
        End If
    End Function
    Sub Limpiar()
        txtDescripcion.Text = String.Empty
        txtCantidad.Text = String.Empty
        txtObservacionesMetodologia.Text = String.Empty
        txtMetodoReclutamiento.Text = String.Empty
        txtNoReclutadores.Text = String.Empty
        txtEspecificacionesLugar.Text = String.Empty
        txtFechaInicial.Text = String.Empty
        txtFechaFinal.Text = String.Empty
        txtNSE.Text = String.Empty
        txtEdades.Text = String.Empty
        txtGenero.Text = String.Empty
        txtOtrasCaracteristicas.Text = String.Empty
        txtExclusionesRestricciones.Text = String.Empty
        txtNoPersonas.Text = String.Empty
        txtGastosDeViaje.Text = String.Empty
        txtIncentivos.Text = String.Empty
        txtAlimentacion.Text = String.Empty
        txtOtros.Text = String.Empty
        txtObservacionesGenerales.Text = String.Empty
        chbTranscripciones.Checked = False
        chbTraducciones.Checked = False
        chbVideo.Checked = False
        chbAudios.Checked = False
        chbFiltros.Checked = False
        chbFlashReport.Checked = False
        ddlDepartamento.SelectedIndex = 0
        ddlCiudad.DataBind()
        ddlLugar.SelectedIndex = 0
        hfIdSegmento.Value = ""
    End Sub

    Sub CargarInfo(ByVal IDSegmento As Int64)
        Dim e As New PY_SegmentosCuali
        Dim o As New CoreProject.SegmentosCuali
        e = o.ObtenerSegmentoXId(IDSegmento)
        hfIdTrabajo.Value = e.TrabajoId
        txtDescripcion.Text = e.Descripcion
        txtCantidad.Text = e.Cantidad
        txtObservacionesMetodologia.Text = e.ObservacionesMetodologia
        txtMetodoReclutamiento.Text = e.MetodoReclutamiento
        txtNoReclutadores.Text = e.NoReclutadoras
        txtFechaInicial.Text = e.FechaInicio
        txtFechaFinal.Text = e.FechaFin
        ddlDepartamento.SelectedValue = e.Departamento
        CargarCiudades()
        ddlCiudad.SelectedValue = e.Municipio
        ddlLugar.SelectedValue = e.TipoLugar
        txtEspecificacionesLugar.Text = e.EspecificacionesLugar
        txtNSE.Text = e.NSE
        txtEdades.Text = e.Edades
        txtGenero.Text = e.Genero
        txtOtrasCaracteristicas.Text = e.OtrasCaracteristicas
        txtExclusionesRestricciones.Text = e.ExclusionesYRestricciones
        txtNoPersonas.Text = e.NoPersonas
        txtGastosDeViaje.Text = e.GastosDeViaje
        txtIncentivos.Text = e.Incentivos
        txtAlimentacion.Text = e.Alimentacion
        chbTranscripciones.Checked = e.Transcripcion
        chbTraducciones.Checked = e.Traduccion
        chbVideo.Checked = e.Video
        chbAudios.Checked = e.Audios
        chbFiltros.Checked = e.Filtros
        chbFlashReport.Checked = e.FlashReport
        txtOtros.Text = e.OtrosEntregables
        txtObservacionesGenerales.Text = e.ObservacionesGenerales
        ObtenerAyudas()
    End Sub

    Sub Validar()
        If txtCantidad.Text = String.Empty Then txtCantidad.Text = 0
        If txtNoReclutadores.Text = String.Empty Then txtNoReclutadores.Text = 0
        If txtNoPersonas.Text = String.Empty Then txtNoPersonas.Text = 0
    End Sub

    Sub Guardar()
        Validar()
        Dim e As New PY_SegmentosCuali
        If IsNumeric(hfIdSegmento.Value) Then e.id = hfIdSegmento.Value
        e.TrabajoId = hfIdTrabajo.Value
        e.Descripcion = txtDescripcion.Text
        e.Cantidad = txtCantidad.Text
        e.ObservacionesMetodologia = txtObservacionesMetodologia.Text
        e.MetodoReclutamiento = txtMetodoReclutamiento.Text
        e.NoReclutadoras = txtNoReclutadores.Text
        e.OtrasAyudas = txtOtrasAyudas.Text
        e.FechaInicio = txtFechaInicial.Text
        e.FechaFin = txtFechaFinal.Text
        e.Departamento = ddlDepartamento.SelectedValue
        e.Municipio = ddlCiudad.SelectedValue
        e.TipoLugar = ddlLugar.SelectedValue
        e.EspecificacionesLugar = txtEspecificacionesLugar.Text
        e.NSE = txtNSE.Text
        e.Edades = txtEdades.Text
        e.Genero = txtGenero.Text
        e.OtrasCaracteristicas = txtOtrasCaracteristicas.Text
        e.ExclusionesYRestricciones = txtExclusionesRestricciones.Text
        e.NoPersonas = txtNoPersonas.Text
        e.GastosDeViaje = txtGastosDeViaje.Text
        e.Incentivos = txtIncentivos.Text
        e.Alimentacion = txtAlimentacion.Text
        e.Transcripcion = chbTranscripciones.Checked
        e.Traduccion = chbTraducciones.Checked
        e.Video = chbVideo.Checked
        e.Audios = chbAudios.Checked
        e.Filtros = chbFiltros.Checked
        e.FlashReport = chbFlashReport.Checked
        e.OtrosEntregables = txtOtros.Text
        e.ObservacionesGenerales = txtObservacionesGenerales.Text
        Dim oSegmentos As New CoreProject.SegmentosCuali
        hfIdSegmento.Value = oSegmentos.GuardarSegmento(e)
        GuardarAyudas()
    End Sub

    Sub GuardarAyudas()
        Dim oSegmentos As New CoreProject.SegmentosCuali
        For Each li As ListItem In chbAyudas.Items
            oSegmentos.GuardarAyudas(hfIdSegmento.Value, li.Value, li.Selected)
        Next
    End Sub

    Sub GuardarModeradores()
        Dim e As New PY_SegmentosCuali_Moderadores
        Dim o As New CoreProject.CampoCualitativo
        'If Not (hfIdModerador.Value = "") Then e = o.ObtenerModeradorxId(hfIdModerador.Value)
        e.TrabajoId = hfTrabajoId.Value
        e.SegmentoId = hfSegmentoId.Value
        e.Moderador = ddlModerador.SelectedValue
        e.FechaAsignacion = DateTime.Now
        hfIdModerador.Value = o.GuardarModerador(e)
    End Sub

    Sub GuardarReclutadores()
        Dim e As New PY_SegmentosCuali_Reclutadores
        Dim o As New CoreProject.CampoCualitativo
        'If Not (hfIdReclutador.Value = "") Then e = o.ObtenerReclutadorxId(hfIdReclutador.Value)
        e.TrabajoId = hfTrabajoId.Value
        e.SegmentoId = hfSegmentoId.Value
        e.Reclutador = ddlReclutador.SelectedValue
        e.FechaAsignacion = DateTime.Now
        hfIdReclutador.Value = o.GuardarReclutador(e)
    End Sub


    Sub ObtenerAyudas()
        Dim oSegmentos As New CoreProject.SegmentosCuali
        For i As Integer = 0 To oSegmentos.ObtenerAyudasRequeridasCualiList(hfIdSegmento.Value).ToList.Count - 1
            Dim val As Integer = oSegmentos.ObtenerAyudasRequeridasCualiList(hfIdSegmento.Value).Item(i).TipoAyuda
            For Each li As ListItem In chbAyudas.Items
                If li.Value = val Then
                    li.Selected = True
                    Exit For
                End If
            Next
        Next

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


    Public Sub CargarModeradoreslist()
        Dim oCampo As New CoreProject.CampoCualitativo
        ddlModerador.DataSource = oCampo.ObtenerModeradores
        ddlModerador.DataValueField = "Id"
        ddlModerador.DataTextField = "Nombre"
        ddlModerador.DataBind()
        Me.ddlModerador.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})

    End Sub

    Public Sub CargarReclutadoreslist()

        Dim oCampo As New CoreProject.CampoCualitativo
        ddlReclutador.DataSource = oCampo.ObtenerReclutadores
        ddlReclutador.DataValueField = "Id"
        ddlReclutador.DataTextField = "Nombre"
        ddlReclutador.DataBind()
        Me.ddlReclutador.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})

    End Sub

    Sub EliminarModerador()
        Dim o As New CoreProject.CampoCualitativo
        Dim e As New PY_SegmentosCuali_Moderadores
        e = o.ObtenerModeradorxId(hfIdModerador.Value)
        If o.EliminarModerador(e) = -1 Then
            ShowNotification("El Registro no se pudo eliminar", ShowNotifications.InfoNotification)
        Else
            ShowNotification("Registro Eliminado correctamente", ShowNotifications.InfoNotification)
        End If
        CargarModeradores(hfSegmentoId.Value)
    End Sub

    Sub EliminarReclutador()
        Dim o As New CoreProject.CampoCualitativo
        Dim e As New PY_SegmentosCuali_Reclutadores
        e = o.ObtenerReclutadorxId(hfIdReclutador.Value)
        If o.EliminarReclutador(e) = -1 Then
            ShowNotification("El Registro no se pudo eliminar", ShowNotifications.InfoNotification)
        Else
            ShowNotification("Registro Eliminado correctamente", ShowNotifications.InfoNotification)
        End If
        CargarReclutadores(hfSegmentoId.Value)
    End Sub



#End Region

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            'If permisos.VerificarPermisoUsuario(97, UsuarioID) = False Then
            '    Response.Redirect("../PY_Proyectos/PY_Proyectos.aspx")
            'End If

            If Request.QueryString("trabajoId") IsNot Nothing Then
                hfIdTrabajo.Value = Int64.Parse(Request.QueryString("trabajoId").ToString)
            Else
                Response.Redirect("../OP_Cualitativo/Trabajos.aspx")
            End If
            If Request.QueryString("py") IsNot Nothing Then
                lnkProyecto.PostBackUrl = "TrabajosCualitativos.aspx"
                btnNuevo.Enabled = True
            Else
                lnkProyecto.PostBackUrl = "../OP_Cualitativo/Trabajos.aspx"
                btnNuevo.Enabled = False
            End If

            CargarLabelTrabajo()
            CargarSegmentos(hfIdTrabajo.Value)
            CargarDepartamentos()
            CargarAyudasCuali()
            CargarLugares()
        End If
    End Sub

    Protected Sub ddlDepartamento_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlDepartamento.SelectedIndexChanged
        If ddlDepartamento.SelectedValue = "" Then Exit Sub
        CargarCiudades()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click

        Try
            btnDuplicar.Enabled = False
            Limpiar()
        Catch ex As Exception
        End Try
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub btnGrabar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGrabar.Click
        Dim oTrabajo As New Trabajo
        Dim oProyecto As New Proyecto
        Dim oeProyecto As New PY_Proyectos_Get_Result
        Dim oPlaneacion As New PlaneacionProduccion
        Dim oWorkFlow As New WorkFlow

        Try
            If Not String.IsNullOrEmpty(txtFechaInicial.Text) Then
                'Validar formato de la fecha
                If Not ValidarFecha(txtFechaInicial.Text) Then Throw New Exception("Fecha de inicio No Válida")
            End If

            If Not String.IsNullOrEmpty(txtFechaFinal.Text) Then
                'Validar formato de la fecha
                If ValidarFecha(txtFechaFinal.Text) Then
                    If Date.Parse(txtFechaFinal.Text) < Date.Parse(txtFechaInicial.Text) Then
                        Throw New Exception("La fecha de finalizacion debe ser mayor a la de inicio")
                    End If
                Else
                    Throw New Exception("Fecha de finalización No Válida")
                End If
            End If

            Guardar()
            CargarSegmentos(hfIdTrabajo.Value)
            Limpiar()
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            EnviarEmail()
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
        Catch ex As Exception
            ShowNotification("Ha ocurrido un error al intentar ingresar el registro - " & ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        End Try
    End Sub

    Protected Sub btnAsignarModerador_Click(sender As Object, e As EventArgs) Handles btnAsignarModerador.Click
        GuardarModeradores()
        CargarModeradores(hfSegmentoId.Value)
        ddlModerador.ClearSelection()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        ShowNotification("Moderador Asignado correctamente!", ShowNotifications.InfoNotification)
    End Sub

    Protected Sub btnAsignarReclutador_Click(sender As Object, e As EventArgs) Handles btnAsignarReclutador.Click
        GuardarReclutadores()
        CargarReclutadores(hfSegmentoId.Value)
        ddlReclutador.ClearSelection()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        ShowNotification("Reclutador Asignado correctamente!", ShowNotifications.InfoNotification)
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        Limpiar()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub gvSegmentos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSegmentos.PageIndexChanging
        gvSegmentos.PageIndex = e.NewPageIndex
        CargarSegmentos(hfIdTrabajo.Value)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvSegmentos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSegmentos.RowCommand
        If e.CommandName = "Abrir" Then
            pnlModeradores.Visible = False
            pnlReclutadores.Visible = False
            'cargarTrabajo(Int64.Parse(Me.gvSegmentos.DataKeys(CInt(e.CommandArgument))("Id")))
            hfIdSegmento.Value = Int64.Parse(Me.gvSegmentos.DataKeys(CInt(e.CommandArgument))("Id"))
            Me.btnDuplicar.Enabled = True
            CargarInfo(hfIdSegmento.Value)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        ElseIf e.CommandName = "Campo" Then
            pnlModeradores.Visible = False
            pnlReclutadores.Visible = False
            hfIdSegmento.Value = Int64.Parse(Me.gvSegmentos.DataKeys(CInt(e.CommandArgument))("Id"))
            If Request.QueryString("py") IsNot Nothing Then
                Response.Redirect("../OP_Cualitativo/CampoCualitativo.aspx?trabajoId=" & hfIdTrabajo.Value & "&idsegmento=" & hfIdSegmento.Value & "&py=true")
                btnNuevo.Enabled = True
            Else
                Response.Redirect("../OP_Cualitativo/CampoCualitativo.aspx?trabajoId=" & hfIdTrabajo.Value & "&idsegmento=" & hfIdSegmento.Value)
                btnNuevo.Enabled = False
            End If
        ElseIf e.CommandName = "Moderador" Then
            hfTrabajoId.Value = Int64.Parse(Request.QueryString("trabajoId").ToString)
            hfSegmentoId.Value = Int64.Parse(Me.gvSegmentos.DataKeys(CInt(e.CommandArgument))("Id"))
            CargarModeradoreslist()
            CargarModeradores(hfSegmentoId.Value)
            pnlModeradores.Visible = True
            pnlReclutadores.Visible = False
            If Request.QueryString("py") IsNot Nothing Then
                lblModerador.Visible = True
                ddlModerador.Visible = True
                btnAsignarModerador.Visible = True
                gvModeradores.Columns(3).Visible = True  'Eliminar
            Else
                lblModerador.Visible = False
                ddlModerador.Visible = False
                ddlModerador.ClearSelection()
                btnAsignarModerador.Visible = False
                gvModeradores.Columns(3).Visible = False  'Eliminar
            End If
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        ElseIf e.CommandName = "Reclutador" Then
            hfTrabajoId.Value = Int64.Parse(Request.QueryString("trabajoId").ToString)
            hfSegmentoId.Value = Int64.Parse(Me.gvSegmentos.DataKeys(CInt(e.CommandArgument))("Id"))
            CargarReclutadoreslist()
            CargarReclutadores(hfSegmentoId.Value)
            pnlModeradores.Visible = False
            pnlReclutadores.Visible = True
            If Request.QueryString("py") IsNot Nothing Then
                lblReclutador.Visible = False
                ddlReclutador.Visible = False
                ddlReclutador.ClearSelection()
                btnAsignarReclutador.Visible = False
                gvReclutadores.Columns(3).Visible = False  'Eliminar
            Else
                lblReclutador.Visible = True
                ddlReclutador.Visible = True
                btnAsignarReclutador.Visible = True
                gvReclutadores.Columns(3).Visible = True  'Eliminar
            End If
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End If
    End Sub

    Private Sub gvModeradores_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvModeradores.RowCommand
        If e.CommandName = "BorrarModerador" Then
            hfIdModerador.Value = Int64.Parse(Me.gvModeradores.DataKeys(CInt(e.CommandArgument))("Id"))
            CargarModeradoreslist()
            CargarModeradores(hfSegmentoId.Value)
            EliminarModerador()
            pnlModeradores.Visible = True
            pnlReclutadores.Visible = False
            If Request.QueryString("py") IsNot Nothing Then
                lblModerador.Visible = True
                ddlModerador.Visible = True
                btnAsignarModerador.Visible = True
                gvModeradores.Columns(3).Visible = True  'Eliminar
            Else
                lblModerador.Visible = False
                ddlModerador.Visible = False
                ddlModerador.ClearSelection()
                btnAsignarModerador.Visible = False
                gvModeradores.Columns(3).Visible = False  'Eliminar
            End If
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End If
    End Sub

    Private Sub gvReclutadores_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvReclutadores.RowCommand
        If e.CommandName = "BorrarReclutador" Then
            hfIdReclutador.Value = Int64.Parse(Me.gvReclutadores.DataKeys(CInt(e.CommandArgument))("Id"))
            CargarReclutadoreslist()
            CargarReclutadores(hfSegmentoId.Value)
            EliminarReclutador()
            pnlModeradores.Visible = False
            pnlReclutadores.Visible = True
            If Request.QueryString("py") IsNot Nothing Then
                lblReclutador.Visible = False
                ddlReclutador.Visible = False
                ddlReclutador.ClearSelection()
                btnAsignarReclutador.Visible = False
                gvReclutadores.Columns(3).Visible = False  'Eliminar
            Else
                lblReclutador.Visible = True
                ddlReclutador.Visible = True
                btnAsignarReclutador.Visible = True
                gvReclutadores.Columns(3).Visible = True  'Eliminar
            End If
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End If
    End Sub



#End Region

    Protected Sub btnDuplicar_Click(sender As Object, e As EventArgs) Handles btnDuplicar.Click
        Dim o As New CoreProject.SegmentosCuali
        hfIdSegmento.Value = o.Duplicar(hfIdSegmento.Value)
        txtDescripcion.Focus()
        ShowNotification("Segmento duplicado", ShowNotifications.InfoNotification)
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Sub EnviarEmail()
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            If String.IsNullOrEmpty(hfIdTrabajo.Value) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de continuar")
            End If
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/NuevoSegmento.aspx?idTrabajo=" & hfIdTrabajo.Value)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End Try
        'Response.Redirect("../OP_Cuantitativo/Trabajos.aspx")
    End Sub

   
End Class