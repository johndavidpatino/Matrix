Imports CoreProject

Public Class PNC_Productos
    Inherits System.Web.UI.Page
    Enum EAsociadoA
        JBE = 1
        JBI = 2
        Actividad = 3
    End Enum
    Enum EEstadoProducto
        rechazado = 6
        causaRegistrada = 7
    End Enum
    Private Sub PNC_Causas_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            cargarGrillaProductos()
            cargarGrillaProductosEnviados()
        End If
    End Sub
#Region "PNC Recibidos"
#Region "Metodos"
    Function obtenerProductos(id As Decimal?, usuario As Decimal?, estado As PNCClass.EEstados?, usuarioRegisra As Decimal?) As List(Of PNC_Productos_Get_Result)
        Dim DALPNC As New PNCClass
        Return DALPNC.obtenerProductos(id, usuario, estado, usuarioRegisra)
    End Function
    Sub cargarGrillaProductos()
        Dim jsScript As String
        Dim lstProductosRecibidos = obtenerProductos(Nothing, Session("IDUsuario").ToString, PNCClass.EEstados.enviado, Nothing)
        gvDatos.DataSource = lstProductosRecibidos
        gvDatos.DataBind()
        jsScript = "actualizarCantRecibidos(" & lstProductosRecibidos.Count.ToString & ");"
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "act", jsScript.ToString(), True)

    End Sub
    Sub cargarProducto(producto As PNC_Productos_Get_Result)
        lblAsociadoA.Text = producto.AsociadoA
        lblNombreProyectoTrabajo.Text = producto.proyectoTrabajo
        lblProceso.Text = producto.Proceso
        lblProcedimiento.Text = producto.Procedimiento
        lblUnidad.Text = producto.Unidad
        lblIdentifica.Text = producto.personaIdentifica
        lblFechaReclamo.Text = producto.fechaReclamo
        lblFuente.Text = producto.Fuente
        lblCategoria.Text = producto.Categoria
        lblResponsable.Text = producto.personaResponsable
        lblInformar.Text = producto.personaInformar
        lblDescripcion.Text = producto.descripcion
        lblTarea.Text = producto.Tarea
    End Sub
    Sub limpiarControlesPNCRecibidos()
        txtCausa.Text = ""
        txtCorreccion.Text = ""
        txtFechaEstimadaCierre.Text = ""
        txtRechazo.Text = ""
    End Sub
#End Region
#Region "Eventos"
    Private Sub btnGrabarCausa_Click(sender As Object, e As EventArgs) Handles btnGrabarCausa.Click
        Dim jsScript As String
        Dim DALPNC As New PNCClass
        Dim idProducto As Decimal
        Dim usuario As Decimal
        idProducto = gvDatos.DataKeys(gvDatos.SelectedIndex)("id")
        usuario = CDec(Session("IDUsuario"))

        DALPNC.grabarCausa(idProducto, txtCausa.Text, txtCorreccion.Text, txtFechaEstimadaCierre.Text, usuario, DateTime.Now)
        DALPNC.actualizarEstadoProducto(idProducto, PNCClass.EEstados.causaRegistrada)
        DALPNC.grabarLog(idProducto, PNCClass.EEstados.causaRegistrada, DateTime.UtcNow.AddHours(-5), usuario, Nothing)

        cargarGrillaProductos()
        limpiarControlesPNCRecibidos()
        upProductosCausas.Update()
        gvDatos.SelectedIndex = -1
        jsScript = "mostrarMensajeGrabacionCausa();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "cambio", jsScript.ToString(), True)
    End Sub
    Private Sub btnGrabarRechazo_Click(sender As Object, e As EventArgs) Handles btnGrabarRechazo.Click
        Dim jsScript As String
        Dim DALPNC As New PNCClass
        Dim idProducto As Decimal
        Dim usuario As Decimal
        idProducto = gvDatos.DataKeys(gvDatos.SelectedIndex)("id")
        usuario = CDec(Session("IDUsuario"))

        DALPNC.actualizarEstadoProducto(idProducto, PNCClass.EEstados.rechazado)
        DALPNC.grabarLog(idProducto, PNCClass.EEstados.rechazado, DateTime.UtcNow.AddHours(-5), usuario, txtRechazo.Text)

        cargarGrillaProductos()
        limpiarControlesPNCRecibidos()
        upProductosCausas.Update()
        gvDatos.SelectedIndex = -1
        jsScript = "mostrarMensajeRechaza();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "cambio", jsScript.ToString(), True)
    End Sub
    Private Sub gvDatos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDatos.RowCommand
        If e.CommandName = "Seleccionar" Then
            Dim DALPNC As New PNCClass
            Dim oProducto As New PNC_Productos_Get_Result
            gvDatos.SelectRow(e.CommandArgument)
            oProducto = DALPNC.obtenerProductos(gvDatos.DataKeys(e.CommandArgument)("id"), Nothing, Nothing, Nothing).FirstOrDefault
            cargarProducto(oProducto)
            limpiarControlesPNCRecibidos()
            pnlWizard.Visible = True
            pnlProductosCausas.Visible = True
            upWizard.Update()
            upProductosCausas.Update()
        End If
    End Sub
    Private Sub gvDatos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        cargarGrillaProductos()
        limpiarControlesPNCRecibidos()
        pnlWizard.Visible = False
        pnlProductosCausas.Visible = False
        gvDatos.SelectedIndex = -1
        upWizard.Update()
        upProductosCausas.Update()
    End Sub


#End Region
#End Region
#Region "Nuevo PNC"
#Region "Eventos"
    Private Sub gvJBEJBICC_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvJBEJBICC.RowCommand
        If e.CommandName = "Seleccionar" Then
            lblId_PT.Text = Server.HtmlDecode(gvJBEJBICC.Rows(e.CommandArgument).Cells(0).Text)
            lblNombre_PT.Text = "Nombre:" & Server.HtmlDecode(gvJBEJBICC.Rows(e.CommandArgument).Cells(1).Text)
        End If
    End Sub
    Private Sub btnBuscarJBEJBICC_Click(sender As Object, e As EventArgs) Handles btnBuscarJBEJBICC.Click
        CargarGridJBEJBICC()
    End Sub
    Private Sub ddlAsociadoA_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAsociadoA.SelectedIndexChanged
        Select Case ddlAsociadoA.SelectedValue
            Case 1
                pnlBuscarJBIJBE.Visible = True
                btnBuscarProyectoEstudio.Text = "Buscar JBE"
            Case 2
                pnlBuscarJBIJBE.Visible = True
                btnBuscarProyectoEstudio.Text = "Buscar JBI"
            Case 3
                pnlBuscarJBIJBE.Visible = False
        End Select
        txtJBEJBICC.Text = ""
        lblId_PT.Text = ""
        lblNombre_PT.Text = ""
        Me.gvJBEJBICC.DataSource = Nothing
        Me.gvJBEJBICC.DataBind()
        upJBEJBICC.Update()

    End Sub
    Private Sub PNC_Diligenciar_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            cargarFuente()
            cargarCategorias()
            cargarDropDownListUsuarios()
            cargarProcesos()
            cargarProcedimientos()
            cargarUnidades()
            cargarTareas()
        End If
    End Sub
    Private Sub btnEnviar_Click(sender As Object, e As EventArgs) Handles btnEnviar.Click
        Dim DALPNC As New PNCClass
        Dim asociadoA As Int16
        Dim estudioId As Decimal? = Nothing
        Dim trabajoId As Decimal? = Nothing
        Dim proceso As Int16
        Dim procedimiento As Int16
        Dim unidad As Int16
        Dim personaIdentifica As Decimal
        Dim fechaReclamo As Date
        Dim fuente As Int16
        Dim categoria As Int16
        Dim tarea As Int16? = Nothing
        Dim responsable As Decimal
        Dim informarA As Decimal
        Dim descripcion As String
        Dim fechaCreacion As DateTime
        Dim usuario As Decimal
        Dim jsScript As String
        Dim idProducto As Decimal
        Dim blEnviarCorreo As New EnviarCorreo


        asociadoA = ddlAsociadoA.SelectedValue
        Select Case asociadoA
            Case EAsociadoA.JBE
                estudioId = CLng(lblId_PT.Text)
            Case EAsociadoA.JBI
                trabajoId = CLng(lblId_PT.Text)
        End Select
        proceso = ddlProcesos.SelectedValue
        procedimiento = ddlProcedimientos.SelectedValue
        unidad = ddlUnidad.SelectedValue
        personaIdentifica = ddlIdentifica.SelectedValue
        fechaReclamo = CDate(txtFechaReclamo.Text)
        fuente = ddlFuente.SelectedValue
        categoria = ddlCategoria.SelectedValue
        If ddlTarea.SelectedValue <> "-1" Then
            tarea = ddlTarea.SelectedValue
        End If
        responsable = ddlResponsable.SelectedValue
        informarA = ddlInformar.SelectedValue
        descripcion = txtDescripcion.Text
        fechaCreacion = DateTime.Now
        usuario = CDec(Session("IDUsuario"))

        idProducto = DALPNC.grabarProducto(asociadoA, estudioId, trabajoId, proceso, procedimiento, unidad, personaIdentifica, fechaReclamo, fuente, categoria, tarea, responsable, informarA, descripcion, PNCClass.EEstados.enviado, fechaCreacion, usuario)
        DALPNC.grabarLog(idProducto, PNCClass.EEstados.enviado, DateTime.UtcNow.AddHours(-5), usuario, Nothing)
        blEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/NotificacionPNC.aspx?idPNC=" & idProducto.ToString)

        cargarGrillaProductos()
        cargarGrillaProductosEnviados()
        limpiarControlesNuevoPNC()


        jsScript = "mostrarMensajeNuevoProducto();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "cambio", jsScript.ToString(), True)

    End Sub

#End Region

#Region "Metodos"
    Sub CargarGridJBEJBICC()

        Dim o As New RecordProduccion
        Dim Busqueda As String = Nothing

        If Not txtJBEJBICC.Text = "" Then Busqueda = txtJBEJBICC.Text

        If ddlAsociadoA.SelectedItem.Value <> -1 Then
            Me.gvJBEJBICC.DataSource = o.JBE_JBI_Busqueda(ddlAsociadoA.SelectedItem.Value, Busqueda)
            Me.gvJBEJBICC.DataBind()
        End If

        upJBEJBICC.Update()

    End Sub
    Sub cargarFuente()
        Dim DALPNC As New PNCClass
        ddlFuente.DataSource = DALPNC.LstFuente
        ddlFuente.DataValueField = "Id"
        ddlFuente.DataTextField = "Descripcion"
        ddlFuente.DataBind()
        ddlFuente.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub
    Sub cargarCategorias()
        Dim DALPNC As New PNCClass
        ddlCategoria.DataSource = DALPNC.LstCategoria
        ddlCategoria.DataValueField = "Id"
        ddlCategoria.DataTextField = "Descripcion"
        ddlCategoria.DataBind()
        ddlCategoria.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub
    Sub cargarProcesos()
        Dim DALPNC As New PNCClass
        ddlProcesos.DataSource = DALPNC.obtenerProcesos
        ddlProcesos.DataValueField = "Id"
        ddlProcesos.DataTextField = "Descripcion"
        ddlProcesos.DataBind()
        ddlProcesos.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub
    Sub cargarProcedimientos()
        Dim DALPNC As New PNCClass
        ddlProcedimientos.DataSource = DALPNC.obtenerProcedimientos
        ddlProcedimientos.DataValueField = "Id"
        ddlProcedimientos.DataTextField = "Descripcion"
        ddlProcedimientos.DataBind()
        ddlProcedimientos.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub
    Sub cargarDropDownListUsuarios()
        Dim lstUsuarios As New List(Of PNC_VObtenerUsuarios)

        lstUsuarios = obtenerUsuarios()
        ddlResponsable.DataSource = lstUsuarios
        ddlResponsable.DataValueField = "id"
        ddlResponsable.DataTextField = "Nombre"
        ddlResponsable.DataBind()
        ddlResponsable.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})

        ddlInformar.DataSource = lstUsuarios
        ddlInformar.DataValueField = "id"
        ddlInformar.DataTextField = "Nombre"
        ddlInformar.DataBind()
        ddlInformar.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})

        ddlIdentifica.DataSource = lstUsuarios
        ddlIdentifica.DataValueField = "id"
        ddlIdentifica.DataTextField = "Nombre"
        ddlIdentifica.DataBind()
        ddlIdentifica.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})

    End Sub
    Function obtenerUsuarios() As List(Of PNC_VObtenerUsuarios)
        Dim lstUsuarios As New List(Of PNC_VObtenerUsuarios)
        Dim DALPNC As New PNCClass
        lstUsuarios = DALPNC.LstUsuarios
        Return lstUsuarios
    End Function
    Sub cargarUnidades()
        Dim DALUnidades As New US.Unidades
        ddlUnidad.DataSource = DALUnidades.ObtenerUnidadesAll()
        ddlUnidad.DataValueField = "id"
        ddlUnidad.DataTextField = "Unidad"
        ddlUnidad.DataBind()
        ddlUnidad.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub
    Sub cargarTareas()
        Dim DALTareas As New CoreProject.Tarea
        ddlTarea.DataSource = DALTareas.obtenerTodas()
        ddlTarea.DataValueField = "id"
        ddlTarea.DataTextField = "Tarea"
        ddlTarea.DataBind()
        ddlTarea.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub
    Sub limpiarControlesNuevoPNC()
        ddlAsociadoA.ClearSelection()
        lblId_PT.Text = ""
        lblNombreProyectoTrabajo.Text = ""
        pnlBuscarJBIJBE.Visible = False
        ddlProcesos.ClearSelection()
        ddlProcedimientos.ClearSelection()
        ddlUnidad.ClearSelection()
        ddlIdentifica.ClearSelection()
        txtFechaReclamo.Text = ""
        ddlFuente.ClearSelection()
        ddlCategoria.ClearSelection()
        ddlTarea.ClearSelection()
        ddlResponsable.ClearSelection()
        ddlInformar.ClearSelection()
        txtDescripcion.Text = ""
    End Sub

#End Region
#End Region

#Region "Productos Enviados"
#Region "Eventos"
    Private Sub gvProductosEnviados_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvProductosEnviados.RowCommand
        If e.CommandName = "Seleccionar" Then
            Dim DALPNC As New PNCClass
            Dim oProducto As PNC_Productos_Get_Result
            Dim idPNC As Long
            idPNC = gvProductosEnviados.DataKeys(e.CommandArgument)("id")
            gvProductosEnviados.SelectRow(e.CommandArgument)
            oProducto = DALPNC.obtenerProductos(idPNC, Nothing, Nothing, Nothing).FirstOrDefault
            cargarProductoEnviado(oProducto)
            cargarRespuesta(oProducto)
            pnlProductoEnviado.Visible = True
        End If
    End Sub
    Private Sub gvProductosEnviados_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvProductosEnviados.PageIndexChanging
        gvProductosEnviados.PageIndex = e.NewPageIndex
        cargarGrillaProductosEnviados()
        pnlProductoEnviado.Visible = False
        gvProductosEnviados.SelectedIndex = -1
        limpiarControlesProductoEnviado()
    End Sub
#End Region
#Region "Metodos"
    Sub cargarGrillaProductosEnviados()
        Dim jsScript As String
        Dim lstProductoEnviados = obtenerProductos(Nothing, Nothing, Nothing, Session("IdUsuario").ToString)
        gvProductosEnviados.DataSource = lstProductoEnviados
        gvProductosEnviados.DataBind()
        jsScript = "actualizarCantEnviados(" & lstProductoEnviados.Count.ToString & ");"
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "act2", jsScript.ToString(), True)
    End Sub
    Sub limpiarControlesProductoEnviado()
        lbl_PE_AsociadoA.Text = ""
        lbl_PE_Nombre.Text = ""
        lbl_PE_Proceso.Text = ""
        lbl_PE_Procedimiento.Text = ""
        lbl_PE_FechaReclamo.Text = ""
        lbl_PE_Fuente.Text = ""
        lbl_PE_Categoria.Text = ""
        lbl_PE_Tarea.Text = ""
        lbl_PE_Unidad.Text = ""
        lbl_PE_Identifica.Text = ""
        lbl_PE_Responsable.Text = ""
        lbl_PE_Informar.Text = ""
        lbl_PE_Descripcion.Text = ""
    End Sub
    Sub cargarProductoEnviado(producto As PNC_Productos_Get_Result)
        lbl_PE_AsociadoA.Text = producto.AsociadoA
        lbl_PE_Nombre.Text = producto.proyectoTrabajo
        lbl_PE_Proceso.Text = producto.Proceso
        lbl_PE_Procedimiento.Text = producto.Procedimiento
        lbl_PE_FechaReclamo.Text = producto.fechaReclamo
        lbl_PE_Fuente.Text = producto.Fuente
        lbl_PE_Categoria.Text = producto.Categoria
        lbl_PE_Tarea.Text = producto.Tarea
        lbl_PE_Unidad.Text = producto.Unidad
        lbl_PE_Identifica.Text = producto.personaIdentifica
        lbl_PE_Responsable.Text = producto.personaResponsable
        lbl_PE_Informar.Text = producto.personaInformar
        lbl_PE_Descripcion.Text = producto.descripcion
    End Sub
    Sub cargarRespuesta(producto As PNC_Productos_Get_Result)
        Dim DALPNC As New PNCClass
        Dim oCausa As PNC_Causa_Get_Result
        Dim oLog As PNC_Productos_Log_Get_Result
        Select Case producto.estado
            Case EEstadoProducto.rechazado
                oLog = DALPNC.obtenerLogProducto(producto.id, 6)
                lblRechazo.Text = oLog.observacion
                divRespuesta.Visible = False
                divRechazo.Visible = True
                btnMostrarRespuestaProductoEnviado.Visible = True
                lblRegistroCausa.Visible = False
            Case EEstadoProducto.causaRegistrada
                oCausa = DALPNC.obtenerCausaPorIdPNC(producto.id)
                lblCausa.Text = oCausa.causa
                lblCorreccion.Text = oCausa.correccion
                lblFechaEstimadaCierre.Text = oCausa.fechaEstimadaCierre
                btnMostrarRespuestaProductoEnviado.Visible = True
                lblRegistroCausa.Visible = False
                divRespuesta.Visible = True
                divRechazo.Visible = False
            Case Else
                divRespuesta.Visible = False
                divRechazo.Visible = False
                btnMostrarRespuestaProductoEnviado.Visible = False
                lblRegistroCausa.Visible = True
        End Select
    End Sub
#End Region
#End Region
End Class