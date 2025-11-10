Imports WebMatrix.Util
Imports System.Data.Objects
Imports CoreProject
Public Class ProductoNoConformeRegistrar
    Inherits System.Web.UI.Page
    Dim o As New PNCClass
    Dim c As New CoreProject.WorkFlow
    Dim IdEstudio As Integer
    Dim WIdTrabajo As Integer
    Dim WReporta As Long
    Dim WUnidad As Integer
    Dim WCliente As Long
    Dim WCerrado As Boolean
    Dim WFechaCierre As Date
    Dim WError As Boolean
    Dim WFechaReclamo As Date
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("idPNC") IsNot Nothing Then
                'Dim o As New CoreProject.PNCClass
                Dim info = o.ObtenerPNCEntidad(Request.QueryString("idPNC").ToString)
                If info.IdTrabajo = 0 Then
                    rdEstudio.Checked = True
                    rdTrabajo.Checked = False
                Else
                    rdEstudio.Checked = False
                    rdTrabajo.Checked = True
                End If
                lblJobBook.Text = info.JobBook
                NEstudio.DataSource = o.ObtenerlstNombreEstudio(lblJobBook.Text)
                NEstudio.DataTextField = "Nombre"
                NEstudio.DataValueField = "id"
                NEstudio.DataBind()
                NEstudio.SelectedValue = info.IdEstudio
                CargarInformacion()
            End If
        End If
    End Sub
    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        LimpiarPantalla()
        Session("IdPNC") = 0
        If rdEstudio.Checked = True Or rdTrabajo.Checked = True Then
            If Len(lblJobBook.Text) > 8 Then
                NEstudio.DataSource = o.ObtenerlstNombreEstudio(lblJobBook.Text)
                NEstudio.DataTextField = "Nombre"
                NEstudio.DataValueField = "id"
                NEstudio.DataBind()
                NEstudio.SelectedValue = "-1"
                ShowNotification("SELECCIONE UN ESTUDIO", WebMatrix.ShowNotifications.InfoNotification)
            Else
                ShowNotification("DIGITE UN NUMERO DE JOB BOOK (Debe ser minimo 9 carecteres con guiones", WebMatrix.ShowNotifications.ErrorNotification)
            End If
        Else
            ShowNotification("DEBE SELECCIONAR ESTUDIO / TRABAJO", WebMatrix.ShowNotifications.ErrorNotification)
        End If
    End Sub
    Private Sub GVTrabajos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GVTrabajos.SelectedIndexChanged
        LimpiarPNC()
        WIdTrabajo = CInt(GVTrabajos.SelectedRow().Cells(1).Text)
        IdTrabajo.Text = WIdTrabajo

        Me.Tarea.DataSource = c.obtenerXIdTrabajo(WIdTrabajo)
        Tarea.DataValueField = "TareaId"
        Tarea.DataTextField = "Tarea"
        Tarea.DataBind()

        Tarea.SelectedValue = "-1"

        JobBook.Text = Server.HtmlDecode(GVTrabajos.SelectedRow().Cells(2).Text)
        NomTrabajo.Text = Server.HtmlDecode(GVTrabajos.SelectedRow().Cells(3).Text)

        Session("IdPNC") = 0

        btnGrabarPNC.Visible = True
        btnGrabarPNCDetalle.Visible = False
        btnActualizarFecha.Visible = False
    End Sub
    Sub btnGrabarPNC_Click(sender As Object, e As EventArgs) Handles btnGrabarPNC.Click
        Dim IdPNC As Integer
        Dim WTarea As Integer
        Date.TryParse(FechaReclamo.Text, WFechaReclamo)

        WError = False
        ValidarDatos()
        If WError = False Then
            WReporta = Session("IdUsuario")
            WUnidad = Session("Unidad")
            WCliente = Session("Cliente")
            WCerrado = False
            WFechaCierre = Nothing

            If rdTrabajo.Checked = True Then
                WIdTrabajo = CInt(IdTrabajo.Text)
            Else
                WIdTrabajo = Nothing
                Tarea.SelectedValue = "-1"
            End If

            If Tarea.SelectedValue = "-1" Or rdEstudio.Checked = True Then
                WTarea = Nothing
            Else
                WTarea = Tarea.SelectedValue
            End If

            IdPNC = o.GrabarRegistroPNC(NEstudio.SelectedValue, WIdTrabajo, JobBook.Text, FechaReclamo.Text, WReporta, WUnidad, WCliente, Fuente.SelectedValue, Categoria.SelectedValue, WTarea, txtDescripcion.Text, WCerrado, WFechaCierre, WReporta, Now(), Now())
            Session("IdPNC") = IdPNC

            'GRABA DETALLE
            btnGrabarPNCDetalle_Click(btnGrabarPNCDetalle, New EventArgs())

            If hfId.Value = "0" Then
                Dim oEnviarCorreo As New EnviarCorreo
                Try
                    oEnviarCorreo.enviarCorreo(Request.Url.Scheme & Uri.SchemeDelimiter & Request.Url.Authority & "/Emails/NotificacionPNC.aspx?idPNC=" & IdPNC)
                Catch ex As Exception
                End Try
            End If
            hfId.Value = IdPNC

            LimpiarAccion()

            Me.GVPNC.DataSource = o.LstPNC(Session("JobBookExterno"))
            GVPNC.DataBind()

            Me.GVPNCDetalle.DataSource = o.LstPNCDetalle(Session("JobBookExterno"))
            GVPNCDetalle.DataBind()

            btnGrabarPNC.Visible = False
            btnGrabarPNCDetalle.Visible = True
            btnActualizarFecha.Visible = False

            ShowNotification("PRODUCTO NO CONFORME GRABADO", WebMatrix.ShowNotifications.InfoNotification)

        End If
    End Sub
    Protected Sub btnGrabarPNCDetalle_Click(sender As Object, e As EventArgs) Handles btnGrabarPNCDetalle.Click
        Dim IdPNCDetalle As Integer

        WError = False
        ValidarDatos()
        If WError = False Then
            'GRABA DETALLE
            IdPNCDetalle = o.GrabarRegistroPNCDetalle(CInt(Session("IdPNC")), txtCausa.Text, TipoAccion.SelectedValue, txtAccion.Text, FechaPlanAccion.Text, FechaEjecAccion.Text, ResponsableAccion.SelectedValue, ResponsableSeguir.SelectedValue, txtEvidencia.Text)
            Dim oEnviarCorreo As New EnviarCorreo
            Try
                oEnviarCorreo.enviarCorreo(Request.Url.Scheme & Uri.SchemeDelimiter & Request.Url.Authority & "/Emails/NotificacionPNCAcciones.aspx?idPNCDet=" & IdPNCDetalle & "&JOBBOOK=" & JobBook.Text)
            Catch ex As Exception
            End Try

            LimpiarAccion()

            Me.GVPNCDetalle.DataSource = o.LstPNCDetalle(Session("JobBookExterno"))
            GVPNCDetalle.DataBind()

            ShowNotification("CAUSA Y ACCION GRABADA", WebMatrix.ShowNotifications.InfoNotification)
        End If

    End Sub
    Protected Sub btnActualizarFecha_Click(sender As Object, e As EventArgs) Handles btnActualizarFecha.Click
        If FechaEjecAccion.Text = Nothing Or Len(txtEvidencia.Text) <= 15 Then
            ShowNotification("REGISTRE FECHA DE EJECUCION Y EVIDENCIA", WebMatrix.ShowNotifications.ErrorNotification)
        Else
            o.ActualizarPNCdetalle(Session("Id"), FechaEjecAccion.Text, txtEvidencia.Text)

            Me.GVPNCDetalle.DataSource = o.LstPNCDetalle(Session("JobBookExterno"))
            GVPNCDetalle.DataBind()
            ShowNotification("ACCION ACTUALIZADA", WebMatrix.ShowNotifications.InfoNotification)
            LimpiarAccion()
            DesbloquearCamposAccion()
            btnGrabarPNCDetalle.Visible = True
            btnActualizarFecha.Visible = False
            btnGrabarPNC.Visible = False
        End If
    End Sub
    Private Sub ValidarDatos()
        If rdTrabajo.Checked = True And IdTrabajo.Text = Nothing Then
            ShowNotification("NO HA SELECCIONADO UN TRABAJO", WebMatrix.ShowNotifications.ErrorNotification)
            WError = True
        End If

        If Fuente.SelectedValue = "-1" Then
            ShowNotification("SELECCIONE FUENTE DEL RECLAMO", WebMatrix.ShowNotifications.ErrorNotification)
            WError = True
        End If
        If Categoria.SelectedValue = "-1" Then
            ShowNotification("SELECCIONE CATEGORIA DEL RECLAMO", WebMatrix.ShowNotifications.ErrorNotification)
            WError = True
        End If

        If Tarea.SelectedValue = "-1" Then
            ShowNotification("SELECCIONE TAREA", WebMatrix.ShowNotifications.ErrorNotification)
            WError = True
        End If

        If Len(txtDescripcion.Text) < 15 Then
            ShowNotification("DIGITE DESCRIPCION (MAYOR A 15 CARACTERES)", WebMatrix.ShowNotifications.ErrorNotification)
            WError = True
        End If

        If (Len(txtCausa.Text) < 15) Or (CInt(TipoAccion.SelectedValue) = 1 And Len(txtAccion.Text) < 15) Then
            ShowNotification("DIGITE CAUSA / ACCION (MAYOR A 15 CARACTERES)", WebMatrix.ShowNotifications.ErrorNotification)
            WError = True
        End If

        If TipoAccion.SelectedValue = "-1" Then
            ShowNotification("SELECCIONE TIPO DE ACCION", WebMatrix.ShowNotifications.ErrorNotification)
            WError = True
        End If

        If o.ExisteAccion(CInt(Session("IdPNC")), TipoAccion.SelectedValue) = True Then
            ShowNotification("TIPO de ACCION YA EXISTE", WebMatrix.ShowNotifications.ErrorNotification)
            WError = True
        End If

        If (TipoAccion.SelectedValue = 2 Or TipoAccion.SelectedValue = 3) And o.ExisteAccionInmediata(CInt(Session("IdPNC"))) = False Then
            ShowNotification("DEBE REGISTRAR PRIMERO UNA ACCION INMEDIATA", WebMatrix.ShowNotifications.ErrorNotification)
            WError = True
        End If

        If ResponsableAccion.SelectedValue = "-1" Then
            ShowNotification("SELECCIONE RESPONSABLE DE ACCION", WebMatrix.ShowNotifications.ErrorNotification)
            WError = True
        End If

        If ResponsableSeguir.SelectedValue = "-1" Then
            ShowNotification("SELECCIONE RESPONSABLE DE SEGUIMIENTO", WebMatrix.ShowNotifications.ErrorNotification)
            WError = True
        End If

        If WFechaReclamo > Now() Then
            ShowNotification("FECHA DE RECLAMO DEBE SER MENOR O IGUAL A HOY", WebMatrix.ShowNotifications.ErrorNotification)
            WError = True
        End If
    End Sub
    Private Sub GVPNC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GVPNC.SelectedIndexChanged
        Session("IdPNC") = GVPNC.SelectedRow().Cells(1).Text
        Me.GVPNCDetalle.DataSource = o.LstUnPNCDetalle(CInt(GVPNC.SelectedRow().Cells(1).Text))
        GVPNCDetalle.DataBind()
        JobBook.Text = Server.HtmlDecode(GVPNC.SelectedRow().Cells(2).Text)
        IdTrabajo.Text = GVPNC.SelectedRow().Cells(3).Text
        NomTrabajo.Text = Server.HtmlDecode(GVPNC.SelectedRow().Cells(4).Text)
        Unidad.Text = Server.HtmlDecode(GVPNC.SelectedRow().Cells(7).Text)
        txtDescripcion.Text = Server.HtmlDecode(CType(GVPNC.SelectedRow().Cells(10).FindControl("Descripcion"), TextBox).Text)

        FechaReclamo.Text = o.ObtenerFechaReclamo(CInt(Session("IdPNC")))
        Fuente.SelectedValue = o.ObtenerFuente(CInt(Session("IdPNC")))
        Categoria.SelectedValue = o.ObtenerCategoria(CInt(Session("IdPNC")))

        Me.Tarea.DataSource = c.obtenerXIdTrabajo(CInt(IdTrabajo.Text))
        Tarea.DataValueField = "TareaId"
        Tarea.DataTextField = "Tarea"
        Tarea.DataBind()

        Tarea.SelectedValue = o.ObtenerTarea(CInt(Session("IdPNC")))

        DesbloquearCamposAccion()

        btnGrabarPNC.Visible = False
        btnGrabarPNCDetalle.Visible = True
        btnActualizarFecha.Visible = False

        LimpiarAccion()
    End Sub
    Private Sub GVPNCDetalle_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GVPNCDetalle.SelectedIndexChanged
        If CInt(Session("IdPNC")) >= 0 Then
            Session("Id") = CInt(GVPNCDetalle.SelectedRow().Cells(2).Text)
            txtCausa.Text = CType(GVPNCDetalle.SelectedRow().Cells(3).FindControl("CausaRaiz"), TextBox).Text
            TipoAccion.SelectedValue = o.ObtenerTipoAccion(CInt(GVPNCDetalle.SelectedRow().Cells(2).Text))
            txtAccion.Text = CType(GVPNCDetalle.SelectedRow().Cells(5).FindControl("DescripcionAccion"), TextBox).Text
            ResponsableAccion.SelectedValue = o.ObtenerResponsableAccion(CInt(GVPNCDetalle.SelectedRow().Cells(2).Text))
            ResponsableSeguir.SelectedValue = o.ObtenerResponsableSeguir(CInt(GVPNCDetalle.SelectedRow().Cells(2).Text))
            FechaPlanAccion.Text = GVPNCDetalle.SelectedRow().Cells(6).Text
            FechaEjecAccion.Text = GVPNCDetalle.SelectedRow().Cells(7).Text
            txtEvidencia.Text = CType(GVPNCDetalle.SelectedRow().Cells(10).FindControl("EvidenciaCierre"), TextBox).Text

            BloquearCamposAccion()

            btnGrabarPNCDetalle.Visible = False
            btnActualizarFecha.Visible = True
            btnGrabarPNC.Visible = False
        Else
            ShowNotification("DEBE SELECCIONAR UN PNC ANTES DE SELECCIONAR UNA ACCION", WebMatrix.ShowNotifications.ErrorNotification)
        End If
    End Sub
    Private Sub NEstudio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles NEstudio.SelectedIndexChanged
        CargarInformacion()
    End Sub

    Sub CargarInformacion()
        If NEstudio.Items.Count > 1 And NEstudio.SelectedValue <> "-1" Then
            IdEstudio = NEstudio.SelectedValue

            Session("JobBookExterno") = o.ObtenerJobBook(IdEstudio)
            JobBook.Text = Session("JobBookExterno")

            Reporta.Text = o.ObtenerUsuario(Session("IdUsuario"))

            FechaReclamo.Text = Now()

            Cliente.Text = o.ObtenerNombreCliente(IdEstudio)
            Session("Cliente") = o.ObtenerIdCliente(IdEstudio)

            Unidad.Text = o.ObtenerNombreUnidad(IdEstudio)
            Session("Unidad") = o.ObtenerCodigoUnidad(IdEstudio)

            CargarUsuarios()
            CargarFuenteCategoriaTipoAccion()
            InicializarComboBox()

            'Si es para un trabajo muestra trabajos
            If rdTrabajo.Checked = True Then
                Me.GVTrabajos.DataSource = o.LstTrabajos(Session("JobBookExterno"))
                GVTrabajos.DataBind()
                If GVTrabajos.Rows.Count > 0 Then
                    ShowNotification("SELECCIONE UN TRABAJO", WebMatrix.ShowNotifications.InfoNotification)
                Else
                    ShowNotification("NO HAY TRABAJOS PARA ESTE ESTUDIO - Se registra AL ESTUDIO", WebMatrix.ShowNotifications.InfoNotification)
                    rdTrabajo.Checked = False
                    rdEstudio.Checked = True
                    JobBook.Text = Session("JobBookExterno")
                    IdTrabajo.Text = Nothing
                    NomTrabajo.Text = NEstudio.Text
                    Tarea.SelectedValue = "-1"
                End If
            Else
                JobBook.Text = Session("JobBookExterno")
                IdTrabajo.Text = Nothing
                NomTrabajo.Text = NEstudio.Text
                Tarea.SelectedValue = "-1"
            End If

            hfId.Value = 0

            Me.GVPNC.DataSource = o.LstPNC(Session("JobBookExterno"))
            GVPNC.DataBind()

            Me.GVPNCDetalle.DataSource = o.LstPNCDetalle(Session("JobBookExterno"))
            GVPNCDetalle.DataBind()

            DesbloquearCamposAccion()

            btnGrabarPNCDetalle.Visible = False
            btnActualizarFecha.Visible = False
            btnGrabarPNC.Visible = True
        Else
            ShowNotification("SELECCIONE ESTUDIO - SI NO HAY ESTUDIOS CON ESTE JOB BOOK - VERIFIQUE EL NUMERO", WebMatrix.ShowNotifications.ErrorNotification)
        End If
    End Sub
    Private Sub rdEstudio_CheckedChanged(sender As Object, e As EventArgs) Handles rdEstudio.CheckedChanged
        If rdEstudio.Checked = True Then
            rdTrabajo.Checked = False
            LimpiarPantalla()
            lblJobBook.Text = Nothing
            NEstudio.Text = Nothing
            ShowNotification("DIGITE JOB BOOK y PULSE BUSCAR JB", WebMatrix.ShowNotifications.InfoNotification)
        Else
            rdTrabajo.Checked = True
        End If
        NEstudio.SelectedValue = "-1"
    End Sub
    Private Sub rdTrabajo_CheckedChanged(sender As Object, e As EventArgs) Handles rdTrabajo.CheckedChanged
        If rdTrabajo.Checked = True Then
            rdEstudio.Checked = False
            lblJobBook.Text = Nothing
            NEstudio.Text = Nothing
            ShowNotification("DIGITE JOB BOOK y PULSE BUSCAR JB", WebMatrix.ShowNotifications.InfoNotification)
        Else
            rdEstudio.Checked = True
        End If
        NEstudio.SelectedValue = "-1"
    End Sub
    Private Sub BloquearCamposAccion()
        'Bloquear campos de accion
        txtCausa.ReadOnly = True
        txtAccion.ReadOnly = True
        FechaPlanAccion.ReadOnly = True
        TipoAccion.Enabled = False
        ResponsableAccion.Enabled = False
        ResponsableSeguir.Enabled = False
    End Sub
    Private Sub DesbloquearCamposAccion()
        'Bloquear campos de accion
        txtCausa.ReadOnly = False
        txtAccion.ReadOnly = False
        FechaPlanAccion.ReadOnly = False
        TipoAccion.Enabled = True
        ResponsableAccion.Enabled = True
        ResponsableSeguir.Enabled = True
    End Sub
    Private Sub LimpiarPantalla()
        GVTrabajos.DataSource = Nothing
        GVTrabajos.DataBind()
        GVPNC.DataSource = Nothing
        GVPNC.DataBind()
        GVPNCDetalle.DataSource = Nothing
        GVPNCDetalle.DataBind()

        JobBook.Text = Nothing
        IdTrabajo.Text = Nothing
        NomTrabajo.Text = Nothing
        Unidad.Text = Nothing
        Reporta.Text = Nothing
        Cliente.Text = Nothing
        FechaReclamo.Text = Nothing
        txtDescripcion.Text = Nothing
        txtCausa.Text = Nothing
        txtAccion.Text = Nothing
        FechaPlanAccion.Text = Nothing
        FechaEjecAccion.Text = Nothing
        txtEvidencia.Text = Nothing
    End Sub
    Private Sub LimpiarPNC()
        GVPNC.SelectedRowStyle.ForeColor = Drawing.Color.Black
        GVPNC.SelectedRowStyle.Font.Bold = False

        JobBook.Text = Nothing
        IdTrabajo.Text = Nothing
        NomTrabajo.Text = Nothing

        txtDescripcion.Text = Nothing
        txtCausa.Text = Nothing
        txtAccion.Text = Nothing
        FechaPlanAccion.Text = Nothing
        FechaEjecAccion.Text = Nothing
        txtEvidencia.Text = Nothing
        InicializarComboBox()
    End Sub
    Private Sub InicializarComboBox()
        Fuente.SelectedValue = "-1"
        Categoria.SelectedValue = "-1"
        TipoAccion.SelectedValue = "-1"
        ResponsableAccion.SelectedValue = "-1"
        ResponsableSeguir.SelectedValue = "-1"
    End Sub
    Private Sub LimpiarAccion()
        txtCausa.Text = Nothing
        TipoAccion.SelectedValue = "-1"
        txtAccion.Text = Nothing
        ResponsableAccion.SelectedValue = "-1"
        ResponsableSeguir.SelectedValue = "-1"
        FechaPlanAccion.Text = Nothing
        FechaEjecAccion.Text = Nothing
        txtEvidencia.Text = Nothing
    End Sub
    Sub CargarFuenteCategoriaTipoAccion()
        Me.Fuente.DataSource = o.LstFuente
        Fuente.DataValueField = "Id"
        Fuente.DataTextField = "Descripcion"
        Fuente.DataBind()

        Me.Categoria.DataSource = o.LstCategoria
        Categoria.DataValueField = "Id"
        Categoria.DataTextField = "Descripcion"
        Categoria.DataBind()

        Me.TipoAccion.DataSource = o.LstTipoAccion
        TipoAccion.DataValueField = "Id"
        TipoAccion.DataTextField = "Accion"
        TipoAccion.DataBind()
    End Sub
    Sub CargarUsuarios()
        Me.ResponsableAccion.DataSource = o.LstUsuarios
        ResponsableAccion.DataValueField = "id"
        ResponsableAccion.DataTextField = "Nombre"
        ResponsableAccion.DataBind()

        Me.ResponsableSeguir.DataSource = o.LstUsuarios
        ResponsableSeguir.DataValueField = "id"
        ResponsableSeguir.DataTextField = "Nombre"
        ResponsableSeguir.DataBind()
    End Sub
    Protected Sub NEstudio_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles NEstudio.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub Fuente_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles Fuente.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub Categoria_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles Categoria.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub Tarea_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles Tarea.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub TipoAccion_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles TipoAccion.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub ResponsableAccion_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ResponsableAccion.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub ResponsableSeguir_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ResponsableSeguir.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
End Class