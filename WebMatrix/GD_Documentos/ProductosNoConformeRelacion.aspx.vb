Imports WebMatrix.Util
Imports CoreProject
Public Class ProductoNoConformeRelacion
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
                Dim info = o.ObtenerPNCEntidad(Request.QueryString("idPNC").ToString)
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
    Private Sub btnTodos_Click(sender As Object, e As EventArgs) Handles btnTodos.Click
        Me.GVPNC.DataSource = o.LstPNCTodos()
        GVPNC.DataBind()
    End Sub
    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        LimpiarPantalla()
        Session("IdPNC") = 0

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
    End Sub
    'Protected Sub btnActualizarFecha_Click(sender As Object, e As EventArgs) Handles btnActualizarFecha.Click
    '    If FechaEjecAccion.Text = Nothing Or Len(txtEvidencia.Text) <= 15 Then
    '        ShowNotification("REGISTRE FECHA DE EJECUCION Y EVIDENCIA", WebMatrix.ShowNotifications.ErrorNotification)
    '    Else
    '        o.ActualizarPNCdetalle(Session("Id"), FechaEjecAccion.Text, txtEvidencia.Text)
    '
    '       Me.GVPNCDetalle.DataSource = o.LstPNCDetalle(Session("JobBookExterno"))
    '        GVPNCDetalle.DataBind()
    '        ShowNotification("ACCION ACTUALIZADA", WebMatrix.ShowNotifications.InfoNotification)
    '        LimpiarAccion()
    '    End If
    'End Sub
    Private Sub GVPNC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GVPNC.SelectedIndexChanged
        Session("IdPNC") = GVPNC.SelectedRow().Cells(1).Text

        'Me.GVPNCDetalle.DataSource = o.LstPNCAcciones(CInt(Session("IdPNC"))
        'GVPNCDetalle.DataBind()

        JobBook.Text = GVPNC.SelectedRow().Cells(2).Text
        IdTrabajo.Text = GVPNC.SelectedRow().Cells(3).Text
        NomTrabajo.Text = GVPNC.SelectedRow().Cells(4).Text
        Unidad.Text = GVPNC.SelectedRow().Cells(7).Text
        txtDescripcion.Text = CType(GVPNC.SelectedRow().Cells(10).FindControl("Descripcion"), TextBox).Text

        FechaReclamo.Text = o.ObtenerFechaReclamo(CInt(Session("IdPNC")))
        Fuente.SelectedValue = o.ObtenerFuente(CInt(Session("IdPNC")))
        Categoria.SelectedValue = o.ObtenerCategoria(CInt(Session("IdPNC")))

        Me.Tarea.DataSource = c.obtenerXIdTrabajo(CInt(IdTrabajo.Text))
        Tarea.DataValueField = "TareaId"
        Tarea.DataTextField = "Tarea"
        Tarea.DataBind()

        Tarea.SelectedValue = o.ObtenerTarea(CInt(Session("IdPNC")))

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

            FechaReclamo.Text = Date.UtcNow.AddHours(-5)

            Cliente.Text = o.ObtenerNombreCliente(IdEstudio)
            Session("Cliente") = o.ObtenerIdCliente(IdEstudio)

            Unidad.Text = o.ObtenerNombreUnidad(IdEstudio)
            Session("Unidad") = o.ObtenerCodigoUnidad(IdEstudio)

            CargarUsuarios()
            CargarFuenteCategoriaTipoAccion()
            InicializarComboBox()

            'Si es para un trabajo muestra trabajos
            'Chequear si hay tarea en PNC
            'If Tarea Then
            'JobBook.Text = Session("JobBookExterno")
            'IdTrabajo.Text = Nothing
            'NomTrabajo.Text = NEstudio.Text
            'Tarea.SelectedValue = "-1"

            'Else
            '    JobBook.Text = Session("JobBookExterno")
            '    IdTrabajo.Text = Nothing
            '    NomTrabajo.Text = NEstudio.Text
            '    Tarea.SelectedValue = "-1"
            'End If

            hfId.Value = 0

            Me.GVPNC.DataSource = o.LstPNC(Session("JobBookExterno"))
            GVPNC.DataBind()

            'Me.GVPNCDetalle.DataSource = o.LstPNCDetalle(Session("JobBookExterno"))
            'GVPNCDetalle.DataBind()

        Else
            ShowNotification("SELECCIONE ESTUDIO - SI NO HAY ESTUDIOS CON ESTE JOB BOOK - VERIFIQUE EL NUMERO", WebMatrix.ShowNotifications.ErrorNotification)
        End If
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
    Private Sub LimpiarPantalla()
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