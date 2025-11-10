Imports CoreProject
Imports WebMatrix.Util
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Resources
Imports System.Globalization
Imports System.Threading
Imports System.Data.OleDb
Imports System.Data

Public Class FI_Gestion_Ordenes1
    Inherits System.Web.UI.Page

#Region "Enumerados"
    Enum eTipoOrden
        OrdenServicio = 1
        OrdenCompra = 2
        OrdenRequerimiento = 3
    End Enum
    Enum eTipo
        JBE = 1
        JBI = 2
        CentroCosto = 3
    End Enum

    Enum eEstados
        Creada = 1
        EnAprobacion = 2
        Aprobada = 3
        Enviada = 4
        Facturada = 5
        Rechazada = 6
        Anulada = 7
    End Enum
#End Region

    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Page.Form.Attributes.Add("enctype", "multipart/form-data")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            cargarCentroCostoDropDown(ddlCentroDeCostosSearch, 3)
            CargarDepartamentos()
            cargarCoexGerente()
        End If
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnGenerarPDF)
        smanager.RegisterPostBackControl(Me.btnDocEquivalente)
        smanager.RegisterPostBackControl(Me.btnNotaCredito)
        smanager.RegisterPostBackControl(Me.btnLoadFile)

    End Sub
    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        hfTipoOrden.Value = eTipoOrden.OrdenServicio
        pnlTitulo.Visible = True
        txtTitulo.Text = "Ordenes de Servicio"
        Limpiar()
        LimpiarBuscar("1")
        liMenu1.Attributes.Remove("class")
        liMenu2.Attributes.Remove("class")
        liMenu3.Attributes.Remove("class")
        liMenu4.Attributes.Remove("class")
        LimpiarDetalle()
        pnlBuscar.Visible = False
        pnlDetalleOrden.Visible = False
        pnlOrden.Visible = False
        pnlAprobaciones.Visible = False
        divFechaSolicitud.Visible = False
        divSolicitado.Visible = False
        divTipoPago.Visible = False
        btnDocEquivalente.Visible = False
        btnNotaCredito.Visible = False
        pnlObservacionesAprobacion.Visible = True
        upObservacionesAprobacion.Update()
        txtFechaSolicitud.Visible = False
        txtSolicitado.Visible = False
        ddlTipoPago.Visible = False
        cargarTipos(1)
    End Sub
    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs) Handles LinkButton2.Click
        hfTipoOrden.Value = eTipoOrden.OrdenCompra
        pnlTitulo.Visible = True
        txtTitulo.Text = "Ordenes de Compra"
        Limpiar()
        LimpiarBuscar("1")
        liMenu1.Attributes.Remove("class")
        liMenu2.Attributes.Remove("class")
        liMenu3.Attributes.Remove("class")
        liMenu4.Attributes.Remove("class")
        LimpiarDetalle()
        pnlBuscar.Visible = False
        pnlDetalleOrden.Visible = False
        pnlOrden.Visible = False
        pnlAprobaciones.Visible = False
        divFechaSolicitud.Visible = False
        divSolicitado.Visible = False
        divTipoPago.Visible = False
        btnDocEquivalente.Visible = False
        btnNotaCredito.Visible = False
        pnlObservacionesAprobacion.Visible = True
        upObservacionesAprobacion.Update()
        txtFechaSolicitud.Visible = False
        txtSolicitado.Visible = False
        ddlTipoPago.Visible = False
        cargarTipos(2)
    End Sub
    Protected Sub LinkButton3_Click(sender As Object, e As EventArgs) Handles LinkButton3.Click
        hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento
        pnlTitulo.Visible = True
        txtTitulo.Text = "Requerimientos de Servicio"
        Limpiar()
        LimpiarBuscar("1")
        liMenu1.Attributes.Remove("class")
        liMenu2.Attributes.Remove("class")
        liMenu3.Attributes.Remove("class")
        liMenu4.Attributes.Remove("class")
        LimpiarDetalle()
        pnlBuscar.Visible = False
        pnlDetalleOrden.Visible = False
        pnlOrden.Visible = False
        pnlAprobaciones.Visible = False
        ddlTipo.Items.Clear()
        ddlTipo.Items.Insert(0, New WebControls.ListItem("Trabajo", "10"))
        ddlTipo.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))
        CargarTipoPagos()

        divAprobarFactura.Visible = False
        divAprobarFacturaCOExGerente.Visible = True
        divAprobarFacturaAdmin.Visible = True
        divAprobarOrden.Visible = False
        divFechaSolicitud.Visible = True
        divSolicitado.Visible = True
        divTipoPago.Visible = True
        btnDocEquivalente.Visible = True
        btnNotaCredito.Visible = True
        pnlObservacionesAprobacion.Visible = False
        upObservacionesAprobacion.Update()
        txtFechaSolicitud.Visible = True
        txtSolicitado.Visible = True
        ddlTipoPago.Visible = True
    End Sub
    Function obtenerIdGrupoUnidad(ByVal JobBook As String, ByVal tipo As eTipo) As Short?

        Dim idGrupoUnidad As Short?
        Dim daU As New US.Unidades

        Select Case tipo
            Case eTipo.JBI
                Dim oT As PY_Trabajo0
                Dim daTrabajo As New Trabajo
                oT = daTrabajo.ObtenerTrabajoXJob(JobBook)
                If Not (oT Is Nothing) Then
                    idGrupoUnidad = daU.ObtenerUnidades(Nothing, Nothing, oT.Unidad).FirstOrDefault().GrupoUnidadId
                End If
            Case eTipo.JBE
                Dim oP As PY_Proyectos_Get_Result
                Dim daProyecto As New Proyecto
                oP = daProyecto.obtenerXJobBook(JobBook)
                If Not (oP Is Nothing) Then
                    idGrupoUnidad = daU.ObtenerUnidades(Nothing, Nothing, oP.UnidadId).FirstOrDefault().GrupoUnidadId
                End If
        End Select
        Return idGrupoUnidad
    End Function
    Sub cargarTipos(ByVal tipoOrden As Short)
        ddlTipo.Items.Clear()
        ddlTipo.Items.Insert(0, New WebControls.ListItem("JBE - JobBookExterno", "1"))
        ddlTipo.Items.Insert(0, New WebControls.ListItem("JBI - JbBookInterno", "2"))
        ddlTipo.Items.Insert(2, New WebControls.ListItem("Centro de Costo", "3"))
        ddlTipo.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))
    End Sub

    Sub cargarCoexGerente()
        Dim u As New CoreProject.US.Usuarios
        Dim listaGerenteOperaciones = u.UsuariosxRol(ListaRoles.AprobacionRSGerente)
        ddlApruebaFacturaGerente.DataSource = listaGerenteOperaciones
        ddlApruebaFacturaGerente.DataTextField = "Usuario"
        ddlApruebaFacturaGerente.DataValueField = "id"
        ddlApruebaFacturaGerente.DataBind()
        ddlApruebaFacturaGerente.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))

        Dim listaCOE = u.UsuariosxRol(ListaRoles.AprobacionRSCoe)
        ddlApruebaFacturaCoe.DataSource = listaCOE
        ddlApruebaFacturaCoe.DataTextField = "Usuario"
        ddlApruebaFacturaCoe.DataValueField = "id"
        ddlApruebaFacturaCoe.DataBind()
        ddlApruebaFacturaCoe.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))

        Dim listaAdmin = u.UsuariosxRol(ListaRoles.AprobacionRSAdmin)
        ddlApruebaFacturaAdmin.DataSource = listaAdmin
        ddlApruebaFacturaAdmin.DataTextField = "Usuario"
        ddlApruebaFacturaAdmin.DataValueField = "id"
        ddlApruebaFacturaAdmin.DataBind()
        ddlApruebaFacturaAdmin.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))
    End Sub

#Region "Menu vertical"
    Private Sub lbMenu1_Click(sender As Object, e As EventArgs) Handles lbMenu1.Click
        Me.btnAdd.Visible = False
        Me.btnCancelDetalle.Visible = False
        Me.pnlOrden.Visible = True
        Me.pnlBuscar.Visible = False
        Me.pnlDetalleOrden.Visible = False
        Me.pnlAprobaciones.Visible = False
        Me.btnAnular.Visible = False
        Limpiar()
        LimpiarBuscar("1")
        AsignacionCiudadesxDefecto()
        If hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
            divAprobarFactura.Visible = False
            divAprobarFacturaCOExGerente.Visible = True
            divAprobarFacturaAdmin.Visible = True
            divAprobarOrden.Visible = False
            checkboxGenerica.Visible = False
        Else
            divAprobarFactura.Visible = True
            divAprobarFacturaCOExGerente.Visible = False
            divAprobarFacturaAdmin.Visible = False
            divAprobarOrden.Visible = True
            checkboxGenerica.Visible = True
        End If
        liMenu1.Attributes.Add("class", "active")
        liMenu2.Attributes.Remove("class")
        liMenu3.Attributes.Remove("class")
        liMenu4.Attributes.Remove("class")
        Dim fecha = DateTime.Today.ToString("dd/MM/yyyy")
        txtFecha.Text = fecha
    End Sub

    Private Sub lbMenu2_Click(sender As Object, e As EventArgs) Handles lbMenu2.Click
        Me.pnlOrden.Visible = False
        Me.pnlBuscar.Visible = True
        Me.pnlDetalleOrden.Visible = False
        Me.pnlAprobaciones.Visible = False
        Me.gvOrdenes.DataSource = Nothing
        Me.gvOrdenes.DataBind()
        Limpiar()
        LimpiarBuscar("1")
        liMenu1.Attributes.Remove("class")
        liMenu2.Attributes.Add("class", "active")
        liMenu3.Attributes.Remove("class")
        liMenu4.Attributes.Remove("class")
    End Sub

    Private Sub lbMenu3_Click(sender As Object, e As EventArgs) Handles lbMenu3.Click
        If hfId.Value = "0" Then Exit Sub
        Me.pnlAprobaciones.Visible = True
        Me.pnlBuscar.Visible = False
        Me.pnlDetalleOrden.Visible = False
        If hfEstado.Value = 1 Then
            btnEnviarAprobacion.Visible = True
            Me.txtComentarios.Visible = True
            Me.lblComentarios.Visible = True
        Else
            btnEnviarAprobacion.Visible = False
            If hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
                Me.txtComentarios.Visible = False
                Me.lblComentarios.Visible = False
            Else
                Me.txtComentarios.Visible = True
                Me.lblComentarios.Visible = True
            End If
        End If
        If hfEstado.Value > 2 Then
            btnEnviarAprobacion.Visible = False
            Me.txtComentarios.Visible = False
            Me.lblComentarios.Visible = False
            Me.btnAdd.Visible = False
            Me.btnCancelDetalle.Visible = False
            'Me.gvDetalle.Columns(4).Visible = False
        Else
            Me.btnAdd.Visible = True
            Me.btnCancelDetalle.Visible = True
        End If
        CargarLogAprobaciones()

        liMenu1.Attributes.Remove("class")
        liMenu2.Attributes.Remove("class")
        liMenu3.Attributes.Add("class", "active")
        liMenu4.Attributes.Remove("class")

        Dim o As New FI.Ordenes
        If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
            If hfEstado.Value = 2 Then
                If o.ObtenerUsuarioAprobacionOS(hfId.Value, Session("IDUsuario").ToString) = True Then
                    btnEnviarAprobacion.Visible = False
                    Me.txtComentarios.Visible = True
                    Me.lblComentarios.Visible = True
                Else
                    btnEnviarAprobacion.Visible = False
                    Me.txtComentarios.Visible = False
                    Me.lblComentarios.Visible = False
                End If
            End If
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
            If hfEstado.Value = 2 Then
                If o.ObtenerUsuarioAprobacionOC(hfId.Value, Session("IDUsuario").ToString) = True Then
                    btnEnviarAprobacion.Visible = False
                    Me.txtComentarios.Visible = True
                    Me.lblComentarios.Visible = True
                Else
                    btnEnviarAprobacion.Visible = False
                    Me.txtComentarios.Visible = False
                    Me.lblComentarios.Visible = False
                End If
            End If
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
            If hfEstado.Value = 2 Then
                If o.ObtenerUsuarioAprobacionOR(hfId.Value, Session("IDUsuario").ToString) = True Then
                    btnEnviarAprobacion.Visible = False
                    If hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
                        Me.txtComentarios.Visible = False
                        Me.lblComentarios.Visible = False
                    Else
                        Me.txtComentarios.Visible = True
                        Me.lblComentarios.Visible = True
                    End If
                Else
                    btnEnviarAprobacion.Visible = False
                    Me.txtComentarios.Visible = False
                    Me.lblComentarios.Visible = False
                End If
            End If
        End If
    End Sub

    Private Sub lbMenu4_Click(sender As Object, e As EventArgs) Handles lbMenu4.Click
        If hfId.Value = "0" Then
            ScriptManager.RegisterStartupScript(Page, Me.GetType(), "CerrarObservacionesAprobacion", "<script>CerrarObservacionesAprobacion()</script>", False)
            Exit Sub
        End If
        CargarObservacionesAprobacion()
    End Sub

    Protected Sub btnGenerarPDF_Click(sender As Object, e As EventArgs) Handles btnGenerarPDF.Click
        If hfId.Value = "0" Then Exit Sub
        Dim oOrdenes As New FI.Ordenes
        Dim generarOrden As New FI_Gestion_Ordenes_Aprobacion

        If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
            Dim o = oOrdenes.ObtenerLogAprobacionesOrdenServicio(hfId.Value)

            If o.Count = 0 OrElse o.Where(Function(x) IsNothing(x.Aprobado)).ToList.Count > 0 OrElse o.Where(Function(x) (x.Aprobado) = False).ToList.Count > 0 Then
                ShowNotification("El PDF solo se puede generar, hasta que tenga todas las aprobaciones", ShowNotifications.ErrorNotification)
                Exit Sub
            End If

            Dim path As String = Server.MapPath("~/Files/")
            Dim nombrepdf As String = "ORDENSERVICIO-" & hfId.Value & ".pdf"
            If Not System.IO.File.Exists(path & "\" & nombrepdf) Then
                generarOrden.construirPDF(FI_Gestion_Ordenes_Aprobacion.eTipoOrden.OrdenServicio, hfId.Value)
            End If
            DescargarPDF(nombrepdf)
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
            Dim o = oOrdenes.ObtenerLogAprobacionesOrdenCompra(hfId.Value)
            If o.Count = 0 OrElse o.Where(Function(x) IsNothing(x.Aprobado)).ToList.Count > 0 OrElse o.Where(Function(x) (x.Aprobado) = False).ToList.Count > 0 Then
                ShowNotification("El PDF solo se puede generar, hasta que tenga todas las aprobaciones", ShowNotifications.ErrorNotification)
                Exit Sub
            End If

            Dim path As String = Server.MapPath("~/Files/")
            Dim nombrepdf As String = "ORDENCOMPRA-" & hfId.Value & ".pdf"
            If Not System.IO.File.Exists(path & "\" & nombrepdf) Then
                generarOrden.construirPDF(FI_Gestion_Ordenes_Aprobacion.eTipoOrden.OrdenCompra, hfId.Value)
            End If
            DescargarPDF(nombrepdf)
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
            Dim o = oOrdenes.ObtenerLogAprobacionesOrdenRequerimiento(hfId.Value)
            If o.Count = 0 OrElse o.Where(Function(x) IsNothing(x.Aprobado)).ToList.Count > 0 OrElse o.Where(Function(x) (x.Aprobado) = False).ToList.Count > 0 Then
                ShowNotification("El PDF solo se puede generar, hasta que tenga todas las aprobaciones", ShowNotifications.ErrorNotification)
                Exit Sub
            End If

            Dim path As String = Server.MapPath("~/Files/")
            Dim nombrepdf As String = "ORDENREQUERIMIENTO-" & hfId.Value & ".pdf"
            If Not System.IO.File.Exists(path & "\" & nombrepdf) Then
                generarOrden.construirPDF(FI_Gestion_Ordenes_Aprobacion.eTipoOrden.OrdenRequerimiento, hfId.Value)
            End If
            DescargarPDF(nombrepdf)
        End If
    End Sub

    Protected Sub btnDocEquivalente_Click(sender As Object, e As EventArgs) Handles btnDocEquivalente.Click
        If hfId.Value = "0" Then Exit Sub
        Dim oOrdenes As New FI.Ordenes
        Dim generarOrden As New FI_Gestion_Ordenes_Aprobacion

        If hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
            Dim o = oOrdenes.ObtenerLogAprobacionesOrdenRequerimiento(hfId.Value)
            If o.Count = 0 OrElse o.Where(Function(x) IsNothing(x.Aprobado)).ToList.Count > 0 OrElse o.Where(Function(x) (x.Aprobado) = False).ToList.Count > 0 Then
                ShowNotification("El PDF solo se puede generar, hasta que tenga todas las aprobaciones", ShowNotifications.ErrorNotification)
                Exit Sub
            End If

            Dim path As String = Server.MapPath("~/Files/")
            Dim nombrepdf As String = "DOCUMENTOEQUIVALENTE-" & hfId.Value & ".pdf"
            generarOrden.construirDocEqui(hfId.Value)
            DescargarPDF(nombrepdf)
        Else
            ShowNotification("No se puede generar un documento equivalente de esta Orden", ShowNotifications.ErrorNotification)
        End If
    End Sub

    Protected Sub btnNotaCredito_Click(sender As Object, e As EventArgs) Handles btnNotaCredito.Click
        If hfId.Value = "0" Then Exit Sub
        Dim oOrdenes As New FI.Ordenes
        Dim generarOrden As New FI_Gestion_Ordenes_Aprobacion

        If hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
            Dim o = oOrdenes.ObtenerLogAprobacionesOrdenRequerimiento(hfId.Value)
            If o.Count = 0 OrElse o.Where(Function(x) IsNothing(x.Aprobado)).ToList.Count > 0 OrElse o.Where(Function(x) (x.Aprobado) = False).ToList.Count > 0 Then
                ShowNotification("El PDF solo se puede generar, hasta que tenga todas las aprobaciones", ShowNotifications.ErrorNotification)
                Exit Sub
            End If

            Dim path As String = Server.MapPath("~/Files/")
            Dim nombrepdf As String = "NOTACREDITO-" & hfId.Value & ".pdf"
            generarOrden.construirNotaCredito(hfId.Value)
            DescargarPDF(nombrepdf)
        Else
            ShowNotification("No se puede generar la Nota Crédito de esta Orden", ShowNotifications.ErrorNotification)
        End If
    End Sub

    Sub CargarObservacionesAprobacion()
        If hfId.Value = "0" Then Exit Sub
        Dim oOrdenes As New FI.Ordenes

        If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
            Me.gvObservacionesAprobacion.DataSource = oOrdenes.ObtenerObservacionAprobacionesOrdenServicio(hfId.Value)

        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
            Me.gvObservacionesAprobacion.DataSource = oOrdenes.ObtenerObservacionesAprobacionesOrdenCompra(hfId.Value)

        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
            Me.gvObservacionesAprobacion.DataSource = oOrdenes.ObtenerObservacionesAprobacionesOrdenRequerimiento(hfId.Value)

        End If

        Me.gvObservacionesAprobacion.DataBind()
        upObservacionesAprobacion.Update()
    End Sub

    Sub CargarLogAprobaciones()
        Dim o As New FI.Ordenes
        If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
            Me.gvAprobaciones.DataSource = o.ObtenerLogAprobacionesOrdenServicio(hfId.Value)
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
            Me.gvAprobaciones.DataSource = o.ObtenerLogAprobacionesOrdenCompra(hfId.Value)
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
            Me.gvAprobaciones.DataSource = o.ObtenerLogAprobacionesOrdenRequerimiento(hfId.Value)
        End If
        Me.gvAprobaciones.DataBind()
    End Sub
    Private Sub btnEnviarAprobacion_Click(sender As Object, e As EventArgs) Handles btnEnviarAprobacion.Click
        Dim o As New FI.Ordenes
        Dim oEnvioCorreo As New EnviarCorreo
        If o.obtenerDetalle(hfTipoOrden.Value, hfId.Value).Count = 0 Then
            AlertJS("No se puede enviar a aprobación una orden sin detalle")
            Exit Sub
        End If


        If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
            Dim entO = o.ObtenerOrdenServicio(hfId.Value)
            entO.Estado = 2
            o.GuardarOrdenServicio(entO)
            o.grabarLogOrdenes(eTipoOrden.OrdenServicio, hfId.Value, eEstados.EnAprobacion, "", DateTime.UtcNow.AddHours(-5), Session("IDUsuario"))
            If ddlTipo.SelectedValue = 3 Then
                o.ContinuarAprobacionOrdenServicio(hfId.Value)
            End If
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
            Dim entO = o.ObtenerOrdenCompra(hfId.Value)
            entO.Estado = 2
            o.GuardarOrdenCompra(entO)
            o.grabarLogOrdenes(eTipoOrden.OrdenCompra, hfId.Value, eEstados.EnAprobacion, "", DateTime.UtcNow.AddHours(-5), Session("IDUsuario"))
            If ddlTipo.SelectedValue = 3 Then
                o.ContinuarAprobacionOrdenCompra(hfId.Value)
            End If
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
            Dim entO = o.ObtenerOrdenRequerimiento(hfId.Value)
            entO.Estado = 2
            o.GuardarOrdenRequerimiento(entO)
            o.grabarLogOrdenes(eTipoOrden.OrdenRequerimiento, hfId.Value, eEstados.EnAprobacion, txtComentarios.Text, DateTime.UtcNow.AddHours(-5), Session("IDUsuario"))
            If ddlTipo.SelectedValue = 3 Then
                o.ContinuarAprobacionOrdenRequerimiento(hfId.Value)
            End If
        End If
        oEnvioCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/AprobarOrden.aspx?IdOrden=" & hfId.Value & "&TipoOrden=" & hfTipoOrden.Value)

        CargarLogAprobaciones()
        Me.txtComentarios.Visible = False
        Me.btnEnviarAprobacion.Visible = False
    End Sub
#End Region
#Region "Busqueda"

    Private Sub btnSearchOk_Click(sender As Object, e As EventArgs) Handles btnSearchOk.Click
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        Dim id As Int64? = Nothing
        Dim fecha As Date? = Nothing
        Dim proveedor As Int64? = Nothing
        Dim solicitadopor As Int64? = Nothing
        Dim elaboradopor As Int64? = Nothing
        Dim jobbook As String = Nothing
        Dim cc As Int64? = Nothing
        If IsNumeric(txtOrdenSearch.Text) Then id = txtOrdenSearch.Text
        If IsDate(txtFechaSearch.Text) Then fecha = txtFechaSearch.Text
        If Not (hfProveedorSearch.Value = "0") Then proveedor = hfProveedorSearch.Value
        If Not hfSolicitanteSearch.Value = "0" Then solicitadopor = hfSolicitanteSearch.Value

        If permisos.VerificarPermisoUsuario(147, UsuarioID) = True Then
            If chbMisOrdenes.Checked = True Then elaboradopor = Session("IDUsuario").ToString
        Else
            elaboradopor = Session("IDUsuario").ToString
        End If


        If Not txtJobBookSearch.Text = "" Then jobbook = txtJobBookSearch.Text
        If Not ddlCentroDeCostosSearch.SelectedValue = "-1" Then cc = ddlCentroDeCostosSearch.SelectedValue
        Dim o As New FI.Ordenes
        Dim daPI As New ProcesosInternos
        Select Case Me.hfTipoOrden.Value
            Case eTipoOrden.OrdenServicio
                Me.gvOrdenes.DataSource = o.ObtenerOrdenesDeServicio(id, fecha, proveedor, solicitadopor, elaboradopor, jobbook, cc, Nothing)
            Case eTipoOrden.OrdenCompra
                Me.gvOrdenes.DataSource = o.ObtenerOrdenesDeCompra(id, fecha, proveedor, solicitadopor, elaboradopor, jobbook, cc, Nothing)
            Case eTipoOrden.OrdenRequerimiento
                Me.gvOrdenes.DataSource = o.ObtenerOrdenesDeRequerimiento(id, fecha, proveedor, solicitadopor, elaboradopor, jobbook, cc, Nothing)
        End Select
        Me.gvOrdenes.DataBind()
    End Sub

    Private Sub btnSearchCancel_Click(sender As Object, e As EventArgs) Handles btnSearchCancel.Click
        LimpiarBuscar("1")

        Me.pnlBuscar.Visible = False
        liMenu1.Attributes.Remove("class")
        liMenu2.Attributes.Remove("class")
        liMenu3.Attributes.Remove("class")
        liMenu4.Attributes.Remove("class")
    End Sub

    Private Sub btnSearchProveedor_Click(sender As Object, e As EventArgs) Handles btnSearchProveedor.Click
        hfTipoBusqueda.Value = 1
    End Sub

    Private Sub btnSolicitanteBusqueda_Click(sender As Object, e As EventArgs) Handles btnSolicitanteBusqueda.Click
        hfTipoBusqueda.Value = 0
    End Sub

    Private Sub btnProveedorBusqueda_Click(sender As Object, e As EventArgs) Handles btnProveedorBusqueda.Click
        hfTipoBusqueda.Value = 0
    End Sub

    Private Sub gvOrdenes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvOrdenes.RowCommand
        hfId.Value = Me.gvOrdenes.DataKeys(CInt(e.CommandArgument))("Id")
        Dim o As New FI.Ordenes
        If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
            Dim infoT = o.ObtenerOrdenesDeServicio(hfId.Value, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
            Dim info = o.ObtenerOrdenServicio(hfId.Value)
            Dim infoLogApp = o.ObtenerLogAprobacionesOrdenServicio(hfId.Value).FirstOrDefault
            Dim infoRadic = o.ObtenerLogAprobacionesRadicacionOrdenRequerimiento(hfId.Value, eTipoOrden.OrdenServicio)
            If infoRadic.Count <= 0 Then
                Session("FacturaAprobada") = False
            Else
                Session("FacturaAprobada") = True
            End If
            Dim idGrupounidad As Short?
            txtBeneficiario.Text = info.Beneficiario
            If Not info.EvaluaProveedor Is Nothing Then hfEvaluador.Value = info.EvaluaProveedor
            ddlTipo.SelectedValue = info.TipoDetalle
            If info.Generica Then chbGenerica.Checked = True Else chbGenerica.Checked = False

            If ddlTipo.SelectedValue <> eTipo.CentroCosto Then
                txtJBIJBE.Text = info.JobBookCodigo
                txtNombreJBIJBE.Text = info.JobBookNombre
                txtJBIJBE.Visible = True
                txtNombreJBIJBE.Visible = True
                lblJBIJBE.Visible = True
                ddlApruebaOrden.Visible = True
                lblApruebaOrden.Visible = True
                lblNombreJBIJBE.Visible = True
                lblCentroCostos.Visible = False
                ddlCentroCostos.Visible = False
                If ddlTipo.SelectedValue = eTipo.JBE Then
                    idGrupounidad = obtenerIdGrupoUnidad(txtJBIJBE.Text, ddlTipo.Text)
                End If
                cargarPersonasAprueban(ddlTipo.SelectedValue, If(idGrupounidad.HasValue, idGrupounidad, Nothing), False)
                If Not infoLogApp Is Nothing Then
                    ddlApruebaOrden.SelectedValue = infoLogApp.UsuarioId
                End If
            Else
                txtJBIJBE.Visible = False
                txtNombreJBIJBE.Visible = False
                lblJBIJBE.Visible = False
                lblNombreJBIJBE.Visible = False
                ddlApruebaOrden.Visible = False
                lblApruebaOrden.Visible = False
                lblCentroCostos.Visible = True
                ddlCentroCostos.Visible = True
                cargarCentroCostoDropDown(ddlCentroCostos, info.TipoDetalle)

            End If

            txtFechaSolicitud.Visible = False
            txtSolicitado.Visible = False
            ddlTipoPago.Visible = False

            If info.CentroDeCosto Is Nothing Then
                If info.JobBook <> "" Then
                    hfCentroCosto.Value = info.JobBook
                Else
                    hfCentroCosto.Value = ""
                End If
            Else
                hfCentroCosto.Value = info.CentroDeCosto
                ddlCentroCostos.SelectedValue = info.CentroDeCosto
            End If

            ddlDepartamento.SelectedValue = info.Departamento
            CargarCiudades()
            ddlCiudad.SelectedValue = info.Ciudad
            txtFecha.Text = info.Fecha
            txtFechaEntrega.Text = info.FechaEntrega
            txtFormaPago.Text = info.FormaPago
            hfProveedor.Value = info.ProveedorId
            hfSolicitante.Value = info.SolicitadoPor
            txtApruebaFactura.Text = infoT.SolicitadoPor
            txtNoOrden.Text = info.id
            txtProveedor.Text = infoT.Proveedor
            hfEstado.Value = info.Estado
            Me.lblSubtotal.InnerText = "Subtotal: " & IIf(info.Subtotal Is Nothing, FormatCurrency(0), FormatCurrency(info.Subtotal, 0))
            checkboxGenerica.Visible = True
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
            Dim infoT = o.ObtenerOrdenesDeCompra(hfId.Value, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
            Dim info = o.ObtenerOrdenCompra(hfId.Value)
            Dim infoLogApp = o.ObtenerLogAprobacionesOrdenCompra(hfId.Value).FirstOrDefault
            Dim infoRadic = o.ObtenerLogAprobacionesRadicacionOrdenRequerimiento(hfId.Value, eTipoOrden.OrdenCompra)
            If infoRadic.Count <= 0 Then
                Session("FacturaAprobada") = False
            Else
                Session("FacturaAprobada") = True
            End If
            Dim idGrupounidad As Short?
            txtBeneficiario.Text = info.Beneficiario
            If Not info.EvaluaProveedor Is Nothing Then hfEvaluador.Value = info.EvaluaProveedor
            ddlTipo.SelectedValue = info.TipoDetalle
            If info.Generica Then chbGenerica.Checked = True Else chbGenerica.Checked = False

            If ddlTipo.SelectedValue <> eTipo.CentroCosto Then
                txtJBIJBE.Text = info.JobBookCodigo
                txtNombreJBIJBE.Text = info.JobBookNombre
                txtJBIJBE.Visible = True
                txtNombreJBIJBE.Visible = True
                lblJBIJBE.Visible = True
                ddlApruebaOrden.Visible = True
                lblApruebaOrden.Visible = True
                lblNombreJBIJBE.Visible = True
                lblCentroCostos.Visible = False
                ddlCentroCostos.Visible = False
                If ddlTipo.SelectedValue = eTipo.JBE Then
                    idGrupounidad = obtenerIdGrupoUnidad(txtJBIJBE.Text, ddlTipo.Text)
                End If
                cargarPersonasAprueban(ddlTipo.SelectedValue, If(idGrupounidad.HasValue, idGrupounidad, Nothing), False)
                If Not infoLogApp Is Nothing Then
                    ddlApruebaOrden.SelectedValue = infoLogApp.UsuarioId
                End If
            Else
                txtJBIJBE.Visible = False
                txtNombreJBIJBE.Visible = False
                lblJBIJBE.Visible = False
                lblNombreJBIJBE.Visible = False
                ddlApruebaOrden.Visible = False
                lblApruebaOrden.Visible = False
                lblCentroCostos.Visible = True
                ddlCentroCostos.Visible = True
                cargarCentroCostoDropDown(ddlCentroCostos, info.TipoDetalle)
            End If

            txtFechaSolicitud.Visible = False
            txtSolicitado.Visible = False
            ddlTipoPago.Visible = False

            If info.CentroDeCosto Is Nothing Then
                If info.JobBook <> "" Then
                    hfCentroCosto.Value = info.JobBook
                Else
                    hfCentroCosto.Value = ""
                End If
            Else
                hfCentroCosto.Value = info.CentroDeCosto
                ddlCentroCostos.SelectedValue = info.CentroDeCosto
            End If
            ddlDepartamento.SelectedValue = info.Departamento
            CargarCiudades()
            ddlCiudad.SelectedValue = info.Ciudad
            txtFecha.Text = info.Fecha
            txtFechaEntrega.Text = info.FechaEntrega
            txtFormaPago.Text = info.FormaPago
            hfProveedor.Value = info.ProveedorId
            hfSolicitante.Value = info.SolicitadoPor
            txtApruebaFactura.Text = infoT.SolicitadoPor
            txtNoOrden.Text = info.id
            txtProveedor.Text = infoT.Proveedor
            hfEstado.Value = info.Estado
            Me.lblSubtotal.InnerText = "Subtotal: " & IIf(info.Subtotal Is Nothing, FormatCurrency(0), FormatCurrency(info.Subtotal, 0))
            checkboxGenerica.Visible = True
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
            Dim infoT = o.ObtenerOrdenesDeRequerimiento(hfId.Value, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
            Dim info = o.ObtenerOrdenRequerimiento(hfId.Value)
            Dim infoLogApp = o.ObtenerLogAprobacionesOrdenRequerimiento(hfId.Value).FirstOrDefault
            Dim infoRadic = o.ObtenerLogAprobacionesRadicacionOrdenRequerimiento(hfId.Value, eTipoOrden.OrdenRequerimiento)
            If infoRadic.Count <= 0 Then
                Session("FacturaAprobada") = False
            Else
                Session("FacturaAprobada") = True
            End If
            Dim idGrupounidad As Short?
            txtBeneficiario.Text = info.Beneficiario
            If Not info.EvaluaProveedor Is Nothing Then hfEvaluador.Value = info.EvaluaProveedor
            If info.TipoDetalle = 2 Then
                ddlTipo.SelectedValue = 10
            Else
                ddlTipo.SelectedValue = info.TipoDetalle
            End If
            If info.Generica Then chbGenerica.Checked = True Else chbGenerica.Checked = False

            If ddlTipo.SelectedValue <> eTipo.CentroCosto Then
                txtJBIJBE.Text = info.JobBookCodigo
                txtNombreJBIJBE.Text = info.JobBookNombre
                txtJBIJBE.Visible = True
                txtNombreJBIJBE.Visible = True
                lblJBIJBE.Visible = True
                ddlApruebaOrden.Visible = True
                lblApruebaOrden.Visible = True
                lblNombreJBIJBE.Visible = True
                lblCentroCostos.Visible = False
                ddlCentroCostos.Visible = False
                If ddlTipo.SelectedValue = eTipo.JBE Then
                    idGrupounidad = obtenerIdGrupoUnidad(txtJBIJBE.Text, ddlTipo.Text)
                End If
                cargarPersonasAprueban(ddlTipo.SelectedValue, If(idGrupounidad.HasValue, idGrupounidad, Nothing), False)
                divAprobarOrden.Visible = False
                divAprobarFactura.Visible = False
                divAprobarFacturaCOExGerente.Visible = True
                divAprobarFacturaAdmin.Visible = True

                ddlApruebaFacturaCoe.Visible = True
                If info.AprobadoCOE Is Nothing Then
                    ddlApruebaFacturaCoe.SelectedValue = -1
                Else
                    Try
                        ddlApruebaFacturaCoe.SelectedValue = info.AprobadoCOE
                    Catch ex As Exception
                        ddlApruebaFacturaCoe.SelectedValue = -1
                    End Try
                End If
                ddlApruebaFacturaCoe.Enabled = False

                ddlApruebaFacturaGerente.Visible = True
                If info.AprobadoGerente Is Nothing Then
                    ddlApruebaFacturaGerente.SelectedValue = -1
                Else
                    Try
                        ddlApruebaFacturaGerente.SelectedValue = info.AprobadoGerente
                    Catch ex As Exception
                        ddlApruebaFacturaGerente.SelectedValue = -1
                    End Try
                End If
                ddlApruebaFacturaGerente.Enabled = False

                ddlApruebaFacturaAdmin.Visible = True
                If info.AprobadoAdmin Is Nothing Then
                    ddlApruebaFacturaAdmin.SelectedValue = -1
                Else
                    ddlApruebaFacturaAdmin.SelectedValue = info.AprobadoAdmin
                End If
                ddlApruebaFacturaAdmin.Enabled = False

                If Not infoLogApp Is Nothing Then
                    If Not hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
                        ddlApruebaOrden.SelectedValue = infoLogApp.UsuarioId
                    End If
                End If

                If Not info.FechaSolicitud Is Nothing Then
                    txtFechaSolicitud.Text = info.FechaSolicitud
                Else
                    txtFechaSolicitud.Text = ""
                End If

                If Not info.NombreSolicitud Is Nothing Then
                    txtSolicitado.Text = info.NombreSolicitud
                Else
                    txtSolicitado.Text = ""
                End If

                If Not info.TipoPago Is Nothing Then
                    ddlTipoPago.SelectedValue = info.TipoPago
                Else
                    ddlTipoPago.SelectedValue = "-1"
                End If

                txtFechaSolicitud.Visible = True
                txtSolicitado.Visible = True
                ddlTipoPago.Visible = True
            Else
                txtJBIJBE.Visible = False
                txtNombreJBIJBE.Visible = False
                lblJBIJBE.Visible = False
                lblNombreJBIJBE.Visible = False
                ddlApruebaOrden.Visible = False
                lblApruebaOrden.Visible = False
                lblCentroCostos.Visible = True
                ddlCentroCostos.Visible = True
                cargarCentroCostoDropDown(ddlCentroCostos, info.TipoDetalle)
            End If

            If info.CentroDeCosto Is Nothing Then
                If info.JobBook <> "" Then
                    hfCentroCosto.Value = info.JobBook
                Else
                    hfCentroCosto.Value = ""
                End If
            Else
                hfCentroCosto.Value = info.CentroDeCosto
                ddlCentroCostos.SelectedValue = info.CentroDeCosto
            End If
            ddlDepartamento.SelectedValue = info.Departamento
            CargarCiudades()
            ddlCiudad.SelectedValue = info.Ciudad
            txtFecha.Text = info.Fecha
            txtFechaEntrega.Text = info.FechaEntrega
            txtFormaPago.Text = info.FormaPago
            hfProveedor.Value = info.ProveedorId
            hfSolicitante.Value = info.SolicitadoPor
            txtApruebaFactura.Text = infoT.SolicitadoPor
            txtNoOrden.Text = info.id
            txtProveedor.Text = infoT.Proveedor
            hfEstado.Value = info.Estado
            Me.lblSubtotal.InnerText = "Subtotal: " & IIf(info.Subtotal Is Nothing, FormatCurrency(0), FormatCurrency(info.Subtotal, 0))
            checkboxGenerica.Visible = False
        End If
        cargarCuentasContablesDropDown()
        CargarDetalle()
        Me.pnlOrden.Visible = True
        Me.pnlDetalleOrden.Visible = True
        Me.pnlBuscar.Visible = False
        If hfEstado.Value = 1 Then
            btnEnviarAprobacion.Visible = True
            Me.txtComentarios.Visible = True
        Else
            btnEnviarAprobacion.Visible = False
            Me.txtComentarios.Visible = True
        End If
        If hfEstado.Value > 2 Then
            btnEnviarAprobacion.Visible = False
            Me.txtComentarios.Visible = False
            Me.btnAdd.Visible = False
            Me.btnCancelDetalle.Visible = False
            'Me.gvDetalle.Columns(5).Visible = False
        End If
        If hfEstado.Value = eEstados.Anulada Then
            lblAnulada.Visible = True
            btnSave.Visible = False
            btnCancel.Visible = False
            btnAnular.Visible = False
            Me.gvDetalle.Columns(5).Visible = False
        ElseIf hfEstado.Value = eEstados.Aprobada Or hfEstado.Value = eEstados.EnAprobacion Then
            lblAnulada.Visible = False
            btnSave.Visible = False
            btnCancel.Visible = False
            btnAnular.Visible = True
            Me.gvDetalle.Columns(5).Visible = False
        Else
            lblAnulada.Visible = False
            btnSave.Visible = True
            btnCancel.Visible = True
            btnAnular.Visible = True
            Me.gvDetalle.Columns(5).Visible = True
        End If
        bloquearRespuestas()
        Dim FacturaAprobada = Session("FacturaAprobada")
        If FacturaAprobada = False Then
            btnAnular.Visible = True
        Else
            btnAnular.Visible = False
        End If

        If hfEstado.Value = 2 Or hfEstado.Value = 6 Then
            If hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
                Me.btnCancelEnvio.Visible = False
            Else
                Me.btnCancelEnvio.Visible = True
            End If
        Else
            Me.btnCancelEnvio.Visible = False
        End If
    End Sub

#End Region
#Region "Maestro Orden"
#Region "Eventos"
    Private Sub ddlTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipo.SelectedIndexChanged
        hfCentroCosto.Value = "0"
        If ddlTipo.SelectedValue <> -1 Then
            Dim tipoddl = ddlTipo.SelectedValue
            If (tipoddl = 10) Then
                tipoddl = 2
            End If
            cargarCentroCostoDropDown(ddlCentroCostos, tipoddl)
            If ddlTipo.SelectedValue = 3 Then

                btnSearchCentroCostos.Visible = True
                lblCentroCostos.Visible = True
                ddlCentroCostos.Visible = True
                lblJBIJBE.Visible = False
                txtJBIJBE.Visible = False
                txtJBIJBE.Text = ""
                lblNombreJBIJBE.Visible = False
                txtNombreJBIJBE.Visible = False
                txtNombreJBIJBE.Text = ""
                ddlApruebaOrden.ClearSelection()
                ddlApruebaOrden.DataSource = ""
                ddlApruebaOrden.DataBind()
                ddlApruebaOrden.Visible = False
                lblApruebaOrden.Visible = False
            Else

                lblJBIJBE.Visible = True
                txtJBIJBE.Visible = True
                txtJBIJBE.Text = ""
                btnSearchCentroCostos.Visible = False
                lblNombreJBIJBE.Visible = True
                txtNombreJBIJBE.Visible = True
                txtNombreJBIJBE.Text = ""
                lblCentroCostos.Visible = False
                ddlCentroCostos.Visible = False
                ddlCentroCostos.ClearSelection()
                ddlApruebaOrden.ClearSelection()
                ddlApruebaOrden.DataSource = ""
                ddlApruebaOrden.DataBind()
                ddlApruebaOrden.Visible = True
                lblApruebaOrden.Visible = True
                If hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
                    Me.txtNombreJBIJBE.ReadOnly = True
                    Me.txtApruebaFactura.Visible = False
                    Me.btnApruebaFactura.Visible = False
                    Me.ddlApruebaFactura.Visible = True
                Else
                    Me.txtNombreJBIJBE.ReadOnly = False
                End If
            End If
        Else
            lblJBIJBE.Visible = False
            txtJBIJBE.Visible = False
            txtJBIJBE.Text = ""
            btnSearchCentroCostos.Visible = False
            lblNombreJBIJBE.Visible = False
            txtNombreJBIJBE.Visible = False
            txtNombreJBIJBE.Text = ""
            lblCentroCostos.Visible = False
            ddlCentroCostos.Visible = False
            ddlCentroCostos.ClearSelection()
            ddlApruebaOrden.ClearSelection()
            ddlApruebaOrden.DataSource = ""
            ddlApruebaOrden.DataBind()
            ddlApruebaOrden.Visible = False
            lblApruebaOrden.Visible = False
        End If
    End Sub
    Private Sub txtJBIJBE_TextChanged(sender As Object, e As EventArgs) Handles txtJBIJBE.TextChanged
        Dim daProyecto As New Proyecto
        Dim daTrabajo As New Trabajo
        Dim oP As PY_Proyectos_Get_Result
        Dim oT As PY_Trabajos_Get_Result
        Dim oT1 As PY_Trabajo0
        Dim daU As New US.Unidades
        Dim idUnidad As Integer = 0
        Dim idGrupoUnidad As Integer = 0
        Dim TipoOrden As Integer = hfTipoOrden.Value
        Dim daO As New FI.Ordenes
        Select Case ddlTipo.SelectedValue
            Case 1
                oP = daProyecto.obtenerXJobBook(txtJBIJBE.Text)
                If Not ((oP Is Nothing)) Then
                    txtNombreJBIJBE.Text = oP.Nombre
                    hfCentroCosto.Value = oP.id
                    idUnidad = oP.UnidadId
                Else
                    txtNombreJBIJBE.Text = ""
                    hfCentroCosto.Value = ""
                End If
                If idUnidad > 0 Then
                    idGrupoUnidad = daU.ObtenerUnidades(Nothing, Nothing, idUnidad).FirstOrDefault().GrupoUnidadId
                    cargarPersonasAprueban(ddlTipo.SelectedValue, idGrupoUnidad, True)
                Else
                    cargarPersonasAprueban(ddlTipo.SelectedValue, Nothing, True)
                End If
            Case 2
                oT1 = daTrabajo.ObtenerTrabajoXJob(txtJBIJBE.Text)
                If Not ((oT1 Is Nothing)) Then
                    txtNombreJBIJBE.Text = oT1.NombreTrabajo
                    hfCentroCosto.Value = oT1.id
                Else
                    txtNombreJBIJBE.Text = ""
                    hfCentroCosto.Value = ""
                End If
                cargarPersonasAprueban(ddlTipo.SelectedValue, Nothing, True)
            Case 10
                If Trim(txtJBIJBE.Text) <> "" Then
                    Dim trabajo As String = Trim(txtJBIJBE.Text)
                    oT = daTrabajo.obtenerXId(trabajo)
                    If Not ((oT Is Nothing)) Then
                        txtNombreJBIJBE.Text = oT.NombreTrabajo
                        txtNombreJBIJBE.ReadOnly = True
                        hfCentroCosto.Value = oT.id
                    Else
                        txtNombreJBIJBE.Text = ""
                        hfCentroCosto.Value = ""
                    End If
                    If TipoOrden = 3 Then
                        cargarPersonasApruebanRequerimiento(Trim(txtJBIJBE.Text))
                    Else
                        cargarPersonasAprueban(ddlTipo.SelectedValue, Nothing, True)
                    End If
                Else
                    cargarPersonasAprueban(ddlTipo.SelectedValue, Nothing, True)
                End If
        End Select

    End Sub

    Private Sub btnBuscarJBEJBICC_Click(sender As Object, e As EventArgs) Handles btnBuscarJBEJBICC.Click
        cargarGrillaBusquedaCentroCostoXTipoYValor(ddlTipo.SelectedValue, txtJBEJBICC.Text)
    End Sub

    Private Sub gvJBEJBICC_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvJBEJBICC.RowCommand
        If e.CommandName = "Seleccionar" Then
            hfCentroCosto.Value = Me.gvJBEJBICC.DataKeys(CInt(e.CommandArgument))("id")
            ddlCentroCostos.SelectedValue = Me.gvJBEJBICC.DataKeys(CInt(e.CommandArgument))("id")
        End If
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Validar()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.pnlDetalleOrden.Visible = False
        Me.pnlOrden.Visible = False
        Limpiar()
        liMenu1.Attributes.Remove("class")
        liMenu2.Attributes.Remove("class")
        liMenu3.Attributes.Remove("class")
        liMenu4.Attributes.Remove("class")
    End Sub

    Private Sub gvProveedores_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvProveedores.RowCommand
        If e.CommandName = "Seleccionar" Then
            If hfTipoBusqueda.Value = 1 Then
                hfProveedor.Value = Me.gvProveedores.DataKeys(CInt(e.CommandArgument))("Identificacion")
                Me.txtProveedor.Text = Server.HtmlDecode(Me.gvProveedores.Rows(CInt(e.CommandArgument)).Cells(1).Text.ToString)
            Else
                hfProveedorSearch.Value = Me.gvProveedores.DataKeys(CInt(e.CommandArgument))("Identificacion")
                Me.txtProveedorBusqueda.Text = Server.HtmlDecode(Me.gvProveedores.Rows(CInt(e.CommandArgument)).Cells(1).Text.ToString)
            End If
        End If
    End Sub

    Private Sub btnBuscarProveedor_Click(sender As Object, e As EventArgs) Handles btnBuscarProveedor.Click
        Dim o As New FI.Ordenes
        Dim identificacion As Int64? = Nothing
        Dim proveedor As String = Nothing
        If IsNumeric(Me.txtNitProveedor.Text) Then identificacion = Me.txtNitProveedor.Text
        If Not Me.txtNombreProveedor.Text = "" Then proveedor = Me.txtNombreProveedor.Text
        Me.gvProveedores.DataSource = o.ObtenerContratistas(identificacion, proveedor, True)
        Me.gvProveedores.DataBind()
        Me.UPanelProveedores.Update()
    End Sub

    Private Sub btnBuscarSolicitante_Click(sender As Object, e As EventArgs) Handles btnBuscarSolicitante.Click
        CargarGridPersonas()
        UPanelSolicitantes.Update()
    End Sub

    Private Sub gvSolicitantes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvSolicitantes.RowCommand
        If e.CommandName = "Seleccionar" Then
            If hfTipoAprobacion.Value = "ApruebaFactura" Then
                txtApruebaFactura.Text = Server.HtmlDecode(gvSolicitantes.Rows(e.CommandArgument).Cells(0).Text & " " & gvSolicitantes.Rows(e.CommandArgument).Cells(1).Text)
                hfSolicitante.Value = Me.gvSolicitantes.DataKeys(CInt(e.CommandArgument))("id")
            ElseIf hfTipoAprobacion.Value = "EvaluaProveedor" Then
                txtBeneficiario.Text = Server.HtmlDecode(gvSolicitantes.Rows(e.CommandArgument).Cells(0).Text & " " & gvSolicitantes.Rows(e.CommandArgument).Cells(1).Text)
                hfEvaluador.Value = Me.gvSolicitantes.DataKeys(CInt(e.CommandArgument))("id")
            ElseIf hfTipoAprobacion.Value = "BuscarSolicitante" Then
                txtSolicitanteBusqueda.Text = Server.HtmlDecode(gvSolicitantes.Rows(e.CommandArgument).Cells(0).Text & " " & gvSolicitantes.Rows(e.CommandArgument).Cells(1).Text)
                hfSolicitanteSearch.Value = Me.gvSolicitantes.DataKeys(CInt(e.CommandArgument))("id")
            Else
                Dim oOrdenes As New FI.Ordenes()
                If hfId.Value <> 0 Then
                    If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
                        oOrdenes.GrabarUsuarioApruebaOrdenServicio(hfId.Value, Me.gvSolicitantes.DataKeys(CInt(e.CommandArgument))("id"))
                    ElseIf hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
                        oOrdenes.GrabarUsuarioApruebaOrdenCompra(hfId.Value, Me.gvSolicitantes.DataKeys(CInt(e.CommandArgument))("id"))
                    ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
                        oOrdenes.GrabarUsuarioApruebaOrdenRequerimiento(hfId.Value, Me.gvSolicitantes.DataKeys(CInt(e.CommandArgument))("id"))
                    End If
                Else
                    AlertJS("Primero debe grabar la orden, luego si asignar las personas")
                End If

            End If
        End If
    End Sub
    Private Sub ddlDepartamento_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlDepartamento.SelectedIndexChanged
        CargarCiudades()
    End Sub



#End Region
#Region "Metodos"
    Function obtenerCentroCosto(ByVal tipo As Byte, ByVal valorBusqueda As String) As List(Of FI_JBE_JBI_CC_Get_Result)
        Dim o As New FI.Ordenes
        Return o.ObtenerJBE_JBI_CC(tipo, valorBusqueda)
    End Function
    Sub cargarGrillaBusquedaCentroCostoXTipoYValor(ByVal tipo As Byte, ByVal valorBusqueda As String)
        Me.gvJBEJBICC.DataSource = obtenerCentroCosto(tipo, valorBusqueda)
        Me.gvJBEJBICC.DataBind()
    End Sub
    Sub cargarCentroCostoDropDown(ByVal ddl As DropDownList, ByVal tipo As Byte)
        ddl.DataSource = obtenerCentroCosto(tipo, Nothing)
        ddl.DataValueField = "id"
        ddl.DataTextField = "nombre"
        ddl.DataBind()
        ddl.Items.Insert(0, New System.Web.UI.WebControls.ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub
    Sub CargarDepartamentos()
        Dim oCoordCampo As New CoordinacionCampo
        Dim list = (From ldep In oCoordCampo.ObtenerDepartamentos()
                    Select iddep = ldep.DivDeptoMunicipio, nomdep = ldep.DivDeptoNombre).Distinct.ToList
        ddlDepartamento.DataSource = list
        ddlDepartamento.DataValueField = "iddep"
        ddlDepartamento.DataTextField = "nomdep"
        ddlDepartamento.DataBind()
        Me.ddlDepartamento.Items.Insert(0, New System.Web.UI.WebControls.ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub
    Sub CargarCiudades()
        Dim oCoordCampo As New CoordinacionCampo
        Dim listciudades = (From ldep In oCoordCampo.ObtenerCiudades(ddlDepartamento.SelectedValue)
                            Select idmuni = ldep.DivMunicipio, nommuni = ldep.DivMuniNombre)
        ddlCiudad.DataSource = listciudades
        ddlCiudad.DataValueField = "idmuni"
        ddlCiudad.DataTextField = "nommuni"
        ddlCiudad.DataBind()
        Me.ddlCiudad.Items.Insert(0, New System.Web.UI.WebControls.ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub
    Sub CargarGridPersonas()
        Dim o As New CoreProject.US.Usuarios
        Dim cedula As Int64? = Nothing
        Dim nombre As String = Nothing
        If IsNumeric(txtCedulaSolicitante.Text) Then cedula = txtCedulaSolicitante.Text
        If Not txtNombreSolicitante.Text = "" Then nombre = txtNombreSolicitante.Text
        Me.gvSolicitantes.DataSource = o.obtener(cedula, nombre, Nothing, Nothing, Nothing, True)
        Me.gvSolicitantes.DataBind()
    End Sub
    Sub Validar()
        If hfProveedor.Value = 0 Then
            AlertJS("Debe seleccionar el proveedor")
            btnSearchProveedor.Focus()
            Exit Sub
        End If

        If Not (IsDate(txtFecha.Text)) Then
            AlertJS("Escriba la fecha de la orden")
            txtFecha.Focus()
            Exit Sub
        End If

        If Not (IsDate(txtFechaEntrega.Text)) Then
            AlertJS("Escriba la fecha de entrega")
            txtFechaEntrega.Focus()
            Exit Sub
        End If

        If ddlDepartamento.SelectedValue = -1 Then
            AlertJS("Seleccione el departamento")
            ddlDepartamento.Focus()
            Exit Sub
        End If

        If ddlCiudad.SelectedValue = "-1" Or Not IsNumeric(ddlCiudad.SelectedValue) Then
            AlertJS("Seleccione la ciudad")
            ddlCiudad.Focus()
            Exit Sub
        End If

        If txtFormaPago.Text = "" Then
            AlertJS("Escriba la forma de pago")
            txtFormaPago.Focus()
            Exit Sub
        End If

        If txtBeneficiario.Text = "" Then
            AlertJS("Seleccione el encargado de Evaluar al Proveedor")
            txtFormaPago.Focus()
            Exit Sub
        End If

        If ddlTipo.SelectedValue = "-1" Then
            AlertJS("Seleccione el tipo de orden y luego el JB o el CC")
            ddlTipo.Focus()
            Exit Sub
        End If

        If hfCentroCosto.Value = "0" Then
            AlertJS("Seleccione el JB o el CC")
            btnBuscarJBEJBICC.Focus()
            Exit Sub
        End If

        If ddlTipo.SelectedValue <> 3 AndAlso String.IsNullOrEmpty(txtJBIJBE.Text) Then
            AlertJS("Debe indicar un codigo de JobBook")
            txtJBIJBE.Focus()
            Exit Sub
        End If

        If ddlTipo.SelectedValue <> 3 AndAlso String.IsNullOrEmpty(txtNombreJBIJBE.Text) Then
            AlertJS("Debe indicar un nombre de JobBook")
            txtNombreJBIJBE.Focus()
            Exit Sub
        End If

        If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
            GuardarOrdenServicio()
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
            GuardarOrdenCompra()
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
            If ddlTipoPago.SelectedValue = "-1" Then
                AlertJS("Seleccione un Tipo de Pago")
                ddlTipoPago.Focus()
                Exit Sub
            ElseIf ddlTipoPago.SelectedValue = "1" Then
                If ddlApruebaFacturaCoe.SelectedValue = "-1" And ddlApruebaFacturaGerente.SelectedValue = "-1" Then
                    AlertJS("Seleccione COE o GERENTE que podrá aprobar la factura")
                    ddlApruebaFacturaGerente.Focus()
                    Exit Sub
                End If
            End If

            'If ddlApruebaFacturaAdmin.SelectedValue = "-1" Then
            '    AlertJS("Seleccione el Administrativo que podrá aprobar la factura")
            '    ddlApruebaFacturaAdmin.Focus()
            '    Exit Sub
            'End If

            GuardarOrdenRequerimiento()
        End If


        If hfEstado.Value = "0" Then
            hfEstado.Value = "1"
        End If
        Me.pnlDetalleOrden.Visible = True
        'txtFechaDetalle.Text = txtFechaEntrega.Text
        cargarCuentasContablesDropDown()
        Me.btnAdd.Visible = True
        Me.btnAnular.Visible = True
        Me.btnCancelDetalle.Visible = True
    End Sub

    Sub GuardarOrdenCompra()
        Dim e As New FI_OrdenCompra
        Dim o As New FI.Ordenes
        If Not hfId.Value = 0 Then
            e = o.ObtenerOrdenCompra(hfId.Value)
        End If
        e.Beneficiario = txtBeneficiario.Text
        If ddlTipo.SelectedValue = 3 Then e.CentroDeCosto = ddlCentroCostos.SelectedValue
        e.Ciudad = ddlCiudad.SelectedValue
        e.Departamento = ddlDepartamento.SelectedValue
        e.ElaboradoPor = Session("IDUsuario").ToString
        e.Fecha = txtFecha.Text
        e.FechaElaboracion = Date.UtcNow.AddHours(-5)
        e.FechaEntrega = txtFechaEntrega.Text
        e.FormaPago = txtFormaPago.Text
        If ddlTipo.SelectedValue <> 3 Then e.JobBook = hfCentroCosto.Value
        If ddlTipo.SelectedValue <> 3 Then e.JobBookCodigo = txtJBIJBE.Text
        If ddlTipo.SelectedValue <> 3 Then e.JobBookNombre = txtNombreJBIJBE.Text
        e.ProveedorId = hfProveedor.Value
        e.SolicitadoPor = hfSolicitante.Value
        e.TipoDetalle = ddlTipo.SelectedValue
        e.EvaluaProveedor = hfEvaluador.Value
        If chbGenerica.Checked = True Then
            e.Generica = True
        Else
            e.Generica = False
        End If
        o.GuardarOrdenCompra(e)

        If ddlTipo.SelectedValue <> 3 Then
            Dim countLogOC = o.ObtenerLogAprobacionesOrdenCompra(e.id)
            If countLogOC.Count > 0 Then
                o.borrarLogsAprobacionOrdenCompra(e.id)
                o.GrabarUsuarioApruebaOrdenCompra(e.id, ddlApruebaOrden.SelectedValue)
            Else
                o.GrabarUsuarioApruebaOrdenCompra(e.id, ddlApruebaOrden.SelectedValue)
            End If
        End If

        If hfId.Value = "0" Then
            e.Estado = 1
            hfEstado.Value = 1
            o.grabarLogOrdenes(eTipoOrden.OrdenCompra, e.id, eEstados.Creada, "", DateTime.UtcNow.AddHours(-5), Session("IDUsuario"))
        End If

        hfId.Value = e.id
        Me.txtNoOrden.Text = hfId.Value
        If hfDuplicar.Value = 1 Then
            DuplicarDetalleoc(hfIdAnterior.Value, hfId.Value)
            'o.GuardarDetalleOrdenCompraDuplicada(hfIdAnterior.Value, hfId.Value)
            'AlertJS("Registro Duplicado")
            'hfDuplicar.Value = 0
        Else
            AlertJS("Registro guardado")
            hfDuplicar.Value = 0
        End If

    End Sub
    Sub GuardarOrdenServicio()
        Dim e As New FI_OrdenServicio
        Dim o As New FI.Ordenes
        If Not hfId.Value = 0 Then
            e = o.ObtenerOrdenServicio(hfId.Value)
        End If
        e.Beneficiario = txtBeneficiario.Text
        If ddlTipo.SelectedValue = 3 Then e.CentroDeCosto = ddlCentroCostos.SelectedValue
        e.Ciudad = ddlCiudad.SelectedValue
        e.Departamento = ddlDepartamento.SelectedValue
        e.ElaboradoPor = Session("IDUsuario").ToString
        e.Fecha = txtFecha.Text
        e.FechaElaboracion = Date.UtcNow.AddHours(-5)
        e.FechaEntrega = txtFechaEntrega.Text
        e.FormaPago = txtFormaPago.Text
        If ddlTipo.SelectedValue <> 3 Then e.JobBook = hfCentroCosto.Value
        If ddlTipo.SelectedValue <> 3 Then e.JobBookCodigo = txtJBIJBE.Text
        If ddlTipo.SelectedValue <> 3 Then e.JobBookNombre = txtNombreJBIJBE.Text
        e.ProveedorId = hfProveedor.Value
        e.SolicitadoPor = hfSolicitante.Value
        e.TipoDetalle = ddlTipo.SelectedValue
        e.EvaluaProveedor = hfEvaluador.Value
        If chbGenerica.Checked = True Then
            e.Generica = True
        Else
            e.Generica = False
        End If
        o.GuardarOrdenServicio(e)

        If ddlTipo.SelectedValue <> 3 Then
            Dim countLogOS = o.ObtenerLogAprobacionesOrdenServicio(e.id)
            If countLogOS.Count > 0 Then
                o.borrarLogsAprobacionOrdenServicio(e.id)
                o.GrabarUsuarioApruebaOrdenServicio(e.id, ddlApruebaOrden.SelectedValue)
            Else
                o.GrabarUsuarioApruebaOrdenServicio(e.id, ddlApruebaOrden.SelectedValue)
            End If
        End If

        If hfId.Value = "0" Then
            e.Estado = 1
            hfEstado.Value = 1
            o.grabarLogOrdenes(eTipoOrden.OrdenServicio, e.id, eEstados.Creada, "", DateTime.UtcNow.AddHours(-5), Session("IDUsuario"))
        End If

        hfId.Value = e.id
        Me.txtNoOrden.Text = hfId.Value
        If hfDuplicar.Value = 1 Then
            DuplicarDetalleos(hfIdAnterior.Value, hfId.Value)
        Else
            AlertJS("Registro guardado")
            hfDuplicar.Value = 0
        End If

    End Sub
    Sub GuardarOrdenRequerimiento()
        Dim e As New FI_OrdenRequerimiento
        Dim o As New FI.Ordenes
        If Not hfId.Value = 0 Then
            e = o.ObtenerOrdenRequerimiento(hfId.Value)
        End If

        If (ddlApruebaFacturaCoe.SelectedValue <> -1) Then
            e.AprobadoCOE = ddlApruebaFacturaCoe.SelectedValue
        End If

        If (ddlApruebaFacturaGerente.SelectedValue <> -1) Then
            e.AprobadoGerente = ddlApruebaFacturaGerente.SelectedValue
        End If

        If (ddlApruebaFacturaAdmin.SelectedValue <> -1) Then
            e.AprobadoAdmin = ddlApruebaFacturaAdmin.SelectedValue
        End If

        hfSolicitante.Value = Session("IDUsuario").ToString
        e.Beneficiario = txtBeneficiario.Text
        If ddlTipo.SelectedValue = 3 Then e.CentroDeCosto = ddlCentroCostos.SelectedValue
        e.Ciudad = ddlCiudad.SelectedValue
        e.Departamento = ddlDepartamento.SelectedValue
        e.ElaboradoPor = Session("IDUsuario").ToString
        e.Fecha = txtFecha.Text
        e.FechaElaboracion = Date.UtcNow.AddHours(-5)
        e.FechaEntrega = txtFechaEntrega.Text
        e.FormaPago = txtFormaPago.Text
        If ddlTipo.SelectedValue <> 3 Then e.JobBook = hfCentroCosto.Value
        If ddlTipo.SelectedValue <> 3 Then e.JobBookCodigo = txtJBIJBE.Text
        If ddlTipo.SelectedValue <> 3 Then e.JobBookNombre = txtNombreJBIJBE.Text
        e.ProveedorId = hfProveedor.Value
        e.SolicitadoPor = hfSolicitante.Value
        If ddlTipo.SelectedValue = 10 Then
            e.TipoDetalle = 2
        Else
            e.TipoDetalle = ddlTipo.SelectedValue
        End If
        e.EvaluaProveedor = hfEvaluador.Value

        e.FechaSolicitud = txtFechaSolicitud.Text
        e.NombreSolicitud = txtSolicitado.Text
        e.TipoPago = ddlTipoPago.SelectedValue

        If chbGenerica.Checked = True Then
            e.Generica = True
        Else
            e.Generica = False
        End If
        e.Estado = 2
        o.GuardarOrdenRequerimiento(e)
        o.grabarLogOrdenes(eTipoOrden.OrdenRequerimiento, hfId.Value, eEstados.EnAprobacion, "Requerimiento de Servicios Aprobados automáticamente", DateTime.UtcNow.AddHours(-5), Session("IDUsuario"))
        If ddlTipo.SelectedValue = 3 Then
            o.ContinuarAprobacionOrdenRequerimiento(hfId.Value)
        End If

        If ddlTipo.SelectedValue <> 3 Then
            Dim countLogOR = o.ObtenerLogAprobacionesOrdenRequerimiento(e.id)
            If countLogOR.Count > 0 Then
                o.borrarLogsAprobacionOrdenRequerimiento(e.id)
                o.GrabarUsuarioApruebaOrdenRequerimiento(e.id, Session("IDUsuario").ToString)
            Else
                o.GrabarUsuarioApruebaOrdenRequerimiento(e.id, Session("IDUsuario").ToString)
            End If
        End If

        If hfId.Value = "0" Then
            e.Estado = 1
            hfEstado.Value = 1
            o.grabarLogOrdenes(eTipoOrden.OrdenRequerimiento, e.id, eEstados.Creada, "", DateTime.UtcNow.AddHours(-5), Session("IDUsuario"))
        End If

        hfId.Value = e.id
        Me.txtNoOrden.Text = hfId.Value
        If hfDuplicar.Value = 1 Then
            DuplicarDetalleor(hfIdAnterior.Value, hfId.Value)
        Else
            AlertJS("Registro guardado")
            hfDuplicar.Value = 0
        End If
    End Sub
    Sub DuplicarDetalleos(ByVal IdOrden As Int16, ByVal NuevoId As Int16)
        Dim o As New FI.Ordenes
        Dim InfoDetalle As New List(Of FI_OrdenDetalle_Get_Result)
        If o.obtenerDetalle(eTipoOrden.OrdenServicio, IdOrden).Count > 0 Then
            InfoDetalle = o.obtenerDetalle(eTipoOrden.OrdenServicio, IdOrden)
            For i = 0 To InfoDetalle.Count - 1
                Dim Y As New FI_OrdenServiciodetalle
                Y.Cantidad = InfoDetalle.Item(i).Cantidad
                Y.CtaContable = InfoDetalle.Item(i).CtaContable
                Y.Descripcion = InfoDetalle.Item(i).Descripcion
                Y.Fecha = InfoDetalle.Item(i).Fecha
                Y.IdOrden = NuevoId
                Y.VrUnitario = InfoDetalle.Item(i).VrUnitario
                Y.VrTotal = InfoDetalle.Item(i).VrTotal
                o.GuardarOrdenServicioDetalle(Y)
                Me.gvDetalle.Columns(5).Visible = True
            Next
        End If
        AlertJS("Orden de servicio duplicada")
        hfDuplicar.Value = 0
    End Sub
    Sub DuplicarDetalleoc(ByVal IdOrden As Int16, ByVal NuevoId As Int16)
        Dim o As New FI.Ordenes
        Dim InfoDetalle As New List(Of FI_OrdenDetalle_Get_Result)
        If o.obtenerDetalle(eTipoOrden.OrdenCompra, IdOrden).Count > 0 Then
            InfoDetalle = o.obtenerDetalle(eTipoOrden.OrdenCompra, IdOrden)
            For i = 0 To InfoDetalle.Count - 1
                Dim Y As New FI_OrdenCompradetalle
                Y.Cantidad = InfoDetalle.Item(i).Cantidad
                Y.CtaContable = InfoDetalle.Item(i).CtaContable
                Y.Descripcion = InfoDetalle.Item(i).Descripcion
                Y.Fecha = InfoDetalle.Item(i).Fecha
                Y.IdOrden = NuevoId
                Y.VrUnitario = InfoDetalle.Item(i).VrUnitario
                Y.VrTotal = InfoDetalle.Item(i).VrTotal
                o.GuardarOrdenCompraDetalle(Y)
                Me.gvDetalle.Columns(5).Visible = True
            Next
        End If
        AlertJS("Orden de compra duplicada")
        hfDuplicar.Value = 0
    End Sub
    Sub DuplicarDetalleor(ByVal IdOrden As Int16, ByVal NuevoId As Int16)
        Dim o As New FI.Ordenes
        Dim InfoDetalle As New List(Of FI_OrdenDetalle_Get_Result)
        If o.obtenerDetalle(eTipoOrden.OrdenRequerimiento, IdOrden).Count > 0 Then
            InfoDetalle = o.obtenerDetalle(eTipoOrden.OrdenRequerimiento, IdOrden)
            For i = 0 To InfoDetalle.Count - 1
                Dim Y As New FI_OrdenRequerimientodetalle
                Y.Cantidad = InfoDetalle.Item(i).Cantidad
                Y.CtaContable = InfoDetalle.Item(i).CtaContable
                Y.Descripcion = InfoDetalle.Item(i).Descripcion
                Y.Fecha = InfoDetalle.Item(i).Fecha
                Y.IdOrden = NuevoId
                Y.VrUnitario = InfoDetalle.Item(i).VrUnitario
                Y.VrTotal = InfoDetalle.Item(i).VrTotal
                o.GuardarOrdenRequerimientoDetalle(Y)
                Me.gvDetalle.Columns(5).Visible = True
            Next
        End If
        AlertJS("Orden de Requerimiento duplicada")
        hfDuplicar.Value = 0
    End Sub

    Sub LimpiarBuscar(todo As String)
        If todo = "1" Then
            txtOrdenSearch.Text = ""
            txtFechaSearch.Text = ""
            hfProveedorSearch.Value = "0"
            hfSolicitanteSearch.Value = "0"
            txtJobBookSearch.Text = ""
            txtProveedorBusqueda.Text = ""
            ddlCentroDeCostosSearch.SelectedValue = -1
            txtSolicitanteBusqueda.Text = ""
        End If
        lblAnulada.Visible = False
        btnSave.Visible = True
        btnduplicar.Visible = True
        btnCancel.Visible = True
        btnCancelEnvio.Visible = False
        btnApruebaFactura.Enabled = True
        btnBeneficiario.Enabled = True
        btnSearchProveedor.Enabled = True
        hfSolicitante.Value = ""
        txtNitProveedor.Text = ""
        txtNombreProveedor.Text = ""
        gvProveedores.DataSource = Nothing
        gvProveedores.DataBind()
        UPanelProveedores.Update()
    End Sub

    Sub Limpiar()
        txtBeneficiario.Text = ""
        hfEvaluador.Value = "0"
        ddlCentroCostos.ClearSelection()
        ddlCiudad.ClearSelection()
        ddlCiudad.DataSource = Nothing
        ddlCiudad.Enabled = True
        ddlDepartamento.ClearSelection()
        ddlDepartamento.Enabled = True
        ddlTipo.Enabled = True
        txtFecha.Text = ""
        txtFecha.Enabled = True
        txtFechaEntrega.Text = ""
        txtFechaEntrega.Enabled = True
        txtFormaPago.Text = ""
        txtFormaPago.Enabled = True
        hfCentroCosto.Value = "0"
        hfProveedor.Value = "0"
        hfSolicitante.Value = ""
        ddlTipo.SelectedIndex = -1
        hfId.Value = "0"
        txtNoOrden.Text = ""
        txtProveedor.Text = ""
        txtApruebaFactura.Text = ""
        txtJBIJBE.Text = ""
        txtNombreJBIJBE.Text = ""
        ddlApruebaOrden.DataSource = ""
        ddlApruebaOrden.DataBind()

        lblJBIJBE.Visible = False
        txtJBIJBE.Visible = False
        txtJBIJBE.Text = ""
        txtJBIJBE.Enabled = True
        btnSearchCentroCostos.Visible = False
        lblNombreJBIJBE.Visible = False
        txtNombreJBIJBE.Visible = False
        txtNombreJBIJBE.Text = ""
        txtNombreJBIJBE.ReadOnly = False
        txtNombreJBIJBE.Enabled = True
        txtFechaSolicitud.Text = ""
        txtFechaSolicitud.Enabled = True
        txtSolicitado.Text = ""
        txtSolicitado.Enabled = True
        ddlTipoPago.ClearSelection()
        ddlTipoPago.Enabled = True
        lblCentroCostos.Visible = False
        ddlCentroCostos.Visible = False
        ddlCentroCostos.Enabled = True
        ddlCentroCostos.ClearSelection()
        ddlApruebaOrden.ClearSelection()
        ddlApruebaOrden.DataSource = ""
        ddlApruebaOrden.DataBind()
        ddlApruebaOrden.Visible = False
        lblApruebaOrden.Visible = False
        txtApruebaFactura.Visible = True
        btnApruebaFactura.Visible = True
        ddlApruebaFactura.Visible = False
        divAprobarFactura.Visible = True
        divAprobarFacturaCOExGerente.Visible = False
        divAprobarFacturaAdmin.Visible = False
        divAprobarOrden.Visible = True
        ddlApruebaFactura.Enabled = True
        ddlApruebaFacturaCoe.Enabled = True
        ddlApruebaFacturaCoe.SelectedValue = -1
        ddlApruebaFacturaGerente.Enabled = True
        ddlApruebaFacturaGerente.SelectedValue = -1
        ddlApruebaFacturaAdmin.Enabled = True
        ddlApruebaFacturaAdmin.SelectedValue = -1

        LimpiarDetalle()
    End Sub

    Sub AsignacionCiudadesxDefecto()
        ddlDepartamento.SelectedValue = 11
        CargarCiudades()
        ddlCiudad.SelectedValue = 11001
    End Sub


    Sub cargarPersonasAprueban(ByVal idTipo As Integer?, ByVal idGrupounidad As Integer?, ByVal soloActivas As Boolean)
        Dim daO As New FI.Ordenes
        Dim datasource = (From X In daO.obtenerUsuariosAprueban(idTipo, idGrupounidad)
                          Where X.Activo = soloActivas Or If(soloActivas = False, True, False)
                          Group By X.Usuario, X.Nombres Into Group).ToList
        ddlApruebaOrden.DataSource = datasource
        ddlApruebaOrden.DataValueField = "Usuario"
        ddlApruebaOrden.DataTextField = "Nombres"
        ddlApruebaOrden.DataBind()

        ddlApruebaFactura.DataSource = datasource
        ddlApruebaFactura.DataValueField = "Usuario"
        ddlApruebaFactura.DataTextField = "Nombres"
        ddlApruebaFactura.DataBind()
    End Sub

    Sub cargarPersonasApruebanRequerimiento(ByVal idTrabajo As Integer?)
        Dim daO As New FI.Ordenes
        Dim datasource = (From X In daO.obtenerUsuariosApruebanRequerimiento(idTrabajo)).ToList

        ddlApruebaOrden.DataSource = datasource
        ddlApruebaOrden.DataValueField = "IDUsuario"
        ddlApruebaOrden.DataTextField = "NombreUsuario"
        ddlApruebaOrden.DataBind()
        ddlApruebaOrden.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))

        ddlApruebaFactura.DataSource = datasource
        ddlApruebaFactura.DataValueField = "IDUsuario"
        ddlApruebaFactura.DataTextField = "NombreUsuario"
        ddlApruebaFactura.DataBind()
        ddlApruebaFactura.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))
    End Sub

    Sub bloquearRespuestas()
        If hfEstado.Value = eEstados.Anulada Or hfEstado.Value = eEstados.Aprobada Or hfEstado.Value = eEstados.EnAprobacion Then
            txtProveedor.Enabled = False
            btnSearchProveedor.Enabled = False
            txtFecha.Enabled = False
            txtFechaEntrega.Enabled = False
            txtFechaSolicitud.Enabled = False
            txtSolicitado.Enabled = False
            ddlTipoPago.Enabled = False
            ddlDepartamento.Enabled = False
            ddlCiudad.Enabled = False
            txtFormaPago.Enabled = False
            txtBeneficiario.Enabled = False
            ddlTipo.Enabled = False
            ddlCentroCostos.Enabled = False
            txtJBIJBE.Enabled = False
            txtNombreJBIJBE.Enabled = False
            btnSearchCentroCostos.Enabled = False
            ddlApruebaOrden.Enabled = False
            txtApruebaFactura.Enabled = False
            btnApruebaFactura.Enabled = False

        Else
            txtProveedor.Enabled = True
            btnSearchProveedor.Enabled = True
            txtFecha.Enabled = True
            txtFechaEntrega.Enabled = True
            txtFechaSolicitud.Enabled = True
            txtSolicitado.Enabled = True
            ddlTipoPago.Enabled = True
            ddlDepartamento.Enabled = True
            ddlCiudad.Enabled = True
            txtFormaPago.Enabled = True
            txtBeneficiario.Enabled = True
            ddlTipo.Enabled = True
            ddlCentroCostos.Enabled = True
            txtJBIJBE.Enabled = True
            txtNombreJBIJBE.Enabled = True
            btnSearchCentroCostos.Enabled = True
            ddlApruebaOrden.Enabled = True
            txtApruebaFactura.Enabled = True
            btnApruebaFactura.Enabled = True
        End If
    End Sub

#End Region
#End Region
#Region "Detalle Orden"
#Region "Eventos"
    Private Sub gvCuentasContables_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCuentasContables.RowCommand
        If e.CommandName = "Seleccionar" Then
            ddlCuentasContables.SelectedValue = Me.gvCuentasContables.DataKeys(CInt(e.CommandArgument))("id")
        End If
    End Sub
    Private Sub btnBuscarCuentaContable_Click(sender As Object, e As EventArgs) Handles btnBuscarCuentaContable.Click
        gvCuentasContables.DataSource = obtenerCuentasContables(txtDescripcion.Text, txtNumeroCuenta.Text)
        gvCuentasContables.DataBind()
    End Sub

    Private Sub gvDetalle_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDetalle.RowCommand
        Dim o As New FI.Ordenes
        If e.CommandName = "Borrar" Then
            If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
                Dim ent = o.ObtenerDetalleOS1(gvDetalle.DataKeys(CInt(e.CommandArgument))("id"))
                o.BorrarDetalleOS(ent)
            ElseIf hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
                Dim ent = o.ObtenerDetalleOC1(gvDetalle.DataKeys(CInt(e.CommandArgument))("id"))
                o.BorrarDetalleOC(ent)
            ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
                Dim ent = o.ObtenerDetalleOR1(gvDetalle.DataKeys(CInt(e.CommandArgument))("id"))
                o.BorrarDetalleOR(ent)
            Else
            End If
        End If
        CargarDetalle()
    End Sub
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ValidarDetalle()
        'txtFechaDetalle.Text = txtFechaEntrega.Text
        CargarDetalle()
    End Sub
#End Region
#Region "Metodos"
    Sub cargarCuentasContablesDropDown()
        ddlCuentasContables.DataSource = obtenerCuentasContables(Nothing, Nothing)
        ddlCuentasContables.DataValueField = "id"
        ddlCuentasContables.DataTextField = "descripcion"
        ddlCuentasContables.DataBind()
        ddlCuentasContables.Items.Insert(0, New System.Web.UI.WebControls.ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub
    Function obtenerCuentasContables(ByVal numeroCuenta As String, ByVal descripcion As String) As List(Of CC_CuentasContablesGet_Result)
        Dim o As New ProcesosInternos
        Dim tipo As Int16
        If String.IsNullOrEmpty(numeroCuenta) Then
            numeroCuenta = Nothing
        End If
        If String.IsNullOrEmpty(descripcion) Then
            descripcion = Nothing
        End If

        If ddlTipo.SelectedValue = 3 Then
            tipo = 1
        ElseIf (ddlTipo.SelectedValue = 1 Or ddlTipo.SelectedValue = 2 Or ddlTipo.SelectedValue = 10) Then
            tipo = 2
        End If

        Return o.CuentasContablesGet(numeroCuenta, descripcion, Nothing, tipo)
    End Function
    Sub CargarDetalle()
        Dim o As New FI.Ordenes

        If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
            Dim info As CoreProject.FI_OrdenServicio
            info = o.ObtenerOrdenServicio(hfId.Value)
            Me.lblSubtotal.InnerText = "Subtotal: " & IIf(info.Subtotal Is Nothing, FormatCurrency(0), FormatCurrency(info.Subtotal, 0))
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
            Dim info As CoreProject.FI_OrdenCompra
            info = o.ObtenerOrdenCompra(hfId.Value)
            Me.lblSubtotal.InnerText = "Subtotal: " & IIf(info.Subtotal Is Nothing, FormatCurrency(0), FormatCurrency(info.Subtotal, 0))
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
            Dim info As CoreProject.FI_OrdenRequerimiento
            info = o.ObtenerOrdenRequerimiento(hfId.Value)
            Me.lblSubtotal.InnerText = "Subtotal: " & IIf(info.Subtotal Is Nothing, FormatCurrency(0), FormatCurrency(info.Subtotal, 0))
        End If
        Me.gvDetalle.DataSource = o.obtenerDetalle(hfTipoOrden.Value, hfId.Value)
        Me.gvDetalle.DataBind()
    End Sub
    Sub ValidarDetalle()
        If txtFechaDetalle.Text = "" Then
            AlertJS("Seleccione la fecha")
            txtFechaDetalle.Focus()
            Exit Sub
        End If
        If txtDescripcionDetalle.Text = "" Then
            AlertJS("Escriba la descripción")
            txtDescripcionDetalle.Focus()
            Exit Sub
        End If
        If Not IsNumeric(txtValorUnitario.Text) Then
            AlertJS("Escriba el valor")
            txtValorUnitario.Focus()
            Exit Sub
        End If
        If Not IsNumeric(txtCantidad.Text) Then
            AlertJS("Escriba el valor")
            txtCantidad.Focus()
            Exit Sub
        End If
        If ddlCuentasContables.SelectedValue = -1 Then
            AlertJS("Debe seleccionar una cuenta contable")
            ddlCuentasContables.Focus()
            Exit Sub
        End If
        If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
            GuardarDetalleOS()
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
            GuardarDetalleOC()
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
            GuardarDetalleOR()
        End If
    End Sub

    Sub GuardarDetalleOC()
        Dim o As New FI.Ordenes
        Dim e As New FI_OrdenCompradetalle
        e.Cantidad = txtCantidad.Text
        e.CtaContable = ddlCuentasContables.SelectedValue
        e.Descripcion = txtDescripcionDetalle.Text
        e.Fecha = txtFecha.Text
        e.IdOrden = hfId.Value
        e.VrUnitario = txtValorUnitario.Text
        o.GuardarOrdenCompraDetalle(e)
        LimpiarDetalle()
    End Sub

    Sub GuardarDetalleOS()
        Dim o As New FI.Ordenes
        Dim e As New FI_OrdenServiciodetalle
        e.Cantidad = txtCantidad.Text
        e.CtaContable = ddlCuentasContables.SelectedValue
        e.Descripcion = txtDescripcionDetalle.Text
        e.Fecha = txtFecha.Text
        e.IdOrden = hfId.Value
        e.VrUnitario = txtValorUnitario.Text
        o.GuardarOrdenServicioDetalle(e)
        LimpiarDetalle()
    End Sub
    Sub GuardarDetalleOR()
        Dim o As New FI.Ordenes
        Dim e As New FI_OrdenRequerimientodetalle
        e.Cantidad = txtCantidad.Text
        e.CtaContable = ddlCuentasContables.SelectedValue
        e.Descripcion = txtDescripcionDetalle.Text
        e.Fecha = txtFecha.Text
        e.IdOrden = hfId.Value
        e.VrUnitario = txtValorUnitario.Text
        o.GuardarOrdenRequerimientoDetalle(e)
        LimpiarDetalle()
    End Sub

    Sub LimpiarDetalle()
        txtFechaDetalle.Text = ""
        txtDescripcionDetalle.Text = ""
        txtValorUnitario.Text = ""
        txtCantidad.Text = ""
        ddlCuentasContables.ClearSelection()
        hfDetalleId.Value = "0"
        gvDetalle.DataSource = Nothing
        gvDetalle.DataBind()
        lblSubtotal.InnerText = ""
    End Sub

    Sub CargarTipoPagos()
        ddlTipoPago.Items.Clear()
        Dim o As New FI.Ordenes
        Dim TipoPago = o.ObtenerTiposPagoRequerimiento()
        ddlTipoPago.DataSource = TipoPago
        ddlTipoPago.DataValueField = "id"
        ddlTipoPago.DataTextField = "TipoPago"
        ddlTipoPago.DataBind()
        ddlTipoPago.Items.Insert(0, New System.Web.UI.WebControls.ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub
#End Region

#End Region
#Region "PDF"
    Sub DescargarPDF(ByVal namefile As String)
        Try
            Dim urlFija As String
            urlFija = "~/Files/"
            'HACK Deuda tecnica Aquí se deberia colocar la ruta raíz en un parametro en la base de datos

            urlFija = Server.MapPath(urlFija & "\" & namefile)

            Dim path As New FileInfo(urlFija)

            Response.Clear()
            'Response.ClearContent()
            'Response.ClearHeaders()
            Response.AddHeader("Content-Disposition", "attachment; filename=" & namefile)
            Response.ContentType = "application/octet-stream"
            Response.WriteFile(urlFija)
            Response.End()
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub

#End Region
    Protected Sub btnAnularOrden_Click(sender As Object, e As EventArgs) Handles btnAnularOrden.Click
        Dim daOrden As New FI.Ordenes
        If String.IsNullOrEmpty(txtObservacionAnulacion.Text) OrElse String.IsNullOrWhiteSpace(txtObservacionAnulacion.Text) Then
            AlertJS("Debe ingresar una observación")
        Else
            daOrden.grabarLogOrdenes(hfTipoOrden.Value, hfId.Value, eEstados.Anulada, If(String.IsNullOrEmpty(txtObservacionAnulacion.Text), Nothing, txtObservacionAnulacion.Text), DateTime.UtcNow.AddHours(-5), Session("IDUsuario"))
            ShowNotification("La orden ha sido anulada satisfactoriamente", ShowNotifications.InfoNotification)
        End If
    End Sub

    Protected Sub btnCancelEnvio_Click(sender As Object, e As EventArgs) Handles btnCancelEnvio.Click
        Dim o As New FI.Ordenes
        If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
            o.borrarLogsAprobacionOrdenServicio(hfId.Value)
            Dim entO = o.ObtenerOrdenServicio(hfId.Value)
            entO.Estado = 1
            o.GuardarOrdenServicio(entO)
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
            o.borrarLogsAprobacionOrdenCompra(hfId.Value)
            Dim entO = o.ObtenerOrdenCompra(hfId.Value)
            entO.Estado = 1
            o.GuardarOrdenCompra(entO)
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
            o.borrarLogsAprobacionOrdenRequerimiento(hfId.Value)
            Dim entO = o.ObtenerOrdenRequerimiento(hfId.Value)
            entO.Estado = 1
            o.GuardarOrdenRequerimiento(entO)
        End If

        AlertJS("El Estado de la Orden ha cambiado a Creada")

        If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
            Dim infoT = o.ObtenerOrdenesDeServicio(hfId.Value, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
            Dim info = o.ObtenerOrdenServicio(hfId.Value)
            Dim infoLogApp = o.ObtenerLogAprobacionesOrdenServicio(hfId.Value).FirstOrDefault
            Dim idGrupounidad As Short?
            txtBeneficiario.Text = info.Beneficiario
            If Not info.EvaluaProveedor Is Nothing Then hfEvaluador.Value = info.EvaluaProveedor
            ddlTipo.SelectedValue = info.TipoDetalle

            If ddlTipo.SelectedValue <> eTipo.CentroCosto Then
                txtJBIJBE.Text = info.JobBookCodigo
                txtNombreJBIJBE.Text = info.JobBookNombre
                txtJBIJBE.Visible = True
                txtNombreJBIJBE.Visible = True
                lblJBIJBE.Visible = True
                ddlApruebaOrden.Visible = True
                lblApruebaOrden.Visible = True
                lblNombreJBIJBE.Visible = True
                lblCentroCostos.Visible = False
                ddlCentroCostos.Visible = False
                If ddlTipo.SelectedValue = eTipo.JBE Then
                    idGrupounidad = obtenerIdGrupoUnidad(txtJBIJBE.Text, ddlTipo.Text)
                End If
                cargarPersonasAprueban(ddlTipo.SelectedValue, If(idGrupounidad.HasValue, idGrupounidad, Nothing), True)
                If Not infoLogApp Is Nothing Then
                    ddlApruebaOrden.SelectedValue = infoLogApp.UsuarioId
                End If
            Else
                txtJBIJBE.Visible = False
                txtNombreJBIJBE.Visible = False
                lblJBIJBE.Visible = False
                lblNombreJBIJBE.Visible = False
                ddlApruebaOrden.Visible = False
                lblApruebaOrden.Visible = False
                lblCentroCostos.Visible = True
                ddlCentroCostos.Visible = True
                cargarCentroCostoDropDown(ddlCentroCostos, info.TipoDetalle)

            End If

            If info.CentroDeCosto Is Nothing Then
                If info.JobBook <> "" Then
                    hfCentroCosto.Value = info.JobBook
                Else
                    hfCentroCosto.Value = ""
                End If
            Else
                hfCentroCosto.Value = info.CentroDeCosto
                ddlCentroCostos.SelectedValue = info.CentroDeCosto
            End If

            ddlDepartamento.SelectedValue = info.Departamento
            CargarCiudades()
            ddlCiudad.SelectedValue = info.Ciudad
            txtFecha.Text = info.Fecha
            txtFechaEntrega.Text = info.FechaEntrega
            txtFormaPago.Text = info.FormaPago
            hfProveedor.Value = info.ProveedorId
            hfSolicitante.Value = info.SolicitadoPor
            txtApruebaFactura.Text = infoT.SolicitadoPor
            txtNoOrden.Text = info.id
            txtProveedor.Text = infoT.Proveedor
            hfEstado.Value = info.Estado
            Me.lblSubtotal.InnerText = "Subtotal: " & IIf(info.Subtotal Is Nothing, FormatCurrency(0), FormatCurrency(info.Subtotal, 0))
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
            Dim infoT = o.ObtenerOrdenesDeCompra(hfId.Value, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
            Dim info = o.ObtenerOrdenCompra(hfId.Value)
            Dim infoLogApp = o.ObtenerLogAprobacionesOrdenCompra(hfId.Value).FirstOrDefault
            Dim idGrupounidad As Short?
            txtBeneficiario.Text = info.Beneficiario
            If Not info.EvaluaProveedor Is Nothing Then hfEvaluador.Value = info.EvaluaProveedor
            ddlTipo.SelectedValue = info.TipoDetalle

            If ddlTipo.SelectedValue <> eTipo.CentroCosto Then
                txtJBIJBE.Text = info.JobBookCodigo
                txtNombreJBIJBE.Text = info.JobBookNombre
                txtJBIJBE.Visible = True
                txtNombreJBIJBE.Visible = True
                lblJBIJBE.Visible = True
                ddlApruebaOrden.Visible = True
                lblApruebaOrden.Visible = True
                lblNombreJBIJBE.Visible = True
                lblCentroCostos.Visible = False
                ddlCentroCostos.Visible = False
                If ddlTipo.SelectedValue = eTipo.JBE Then
                    idGrupounidad = obtenerIdGrupoUnidad(txtJBIJBE.Text, ddlTipo.Text)
                End If
                cargarPersonasAprueban(ddlTipo.SelectedValue, If(idGrupounidad.HasValue, idGrupounidad, Nothing), True)
                If Not infoLogApp Is Nothing Then
                    ddlApruebaOrden.SelectedValue = infoLogApp.UsuarioId
                End If
            Else
                txtJBIJBE.Visible = False
                txtNombreJBIJBE.Visible = False
                lblJBIJBE.Visible = False
                lblNombreJBIJBE.Visible = False
                ddlApruebaOrden.Visible = False
                lblApruebaOrden.Visible = False
                lblCentroCostos.Visible = True
                ddlCentroCostos.Visible = True
                cargarCentroCostoDropDown(ddlCentroCostos, info.TipoDetalle)
            End If

            If info.CentroDeCosto Is Nothing Then
                If info.JobBook <> "" Then
                    hfCentroCosto.Value = info.JobBook
                Else
                    hfCentroCosto.Value = ""
                End If
            Else
                hfCentroCosto.Value = info.CentroDeCosto
                ddlCentroCostos.SelectedValue = info.CentroDeCosto
            End If
            ddlDepartamento.SelectedValue = info.Departamento
            CargarCiudades()
            ddlCiudad.SelectedValue = info.Ciudad
            txtFecha.Text = info.Fecha
            txtFechaEntrega.Text = info.FechaEntrega
            txtFormaPago.Text = info.FormaPago
            hfProveedor.Value = info.ProveedorId
            hfSolicitante.Value = info.SolicitadoPor
            txtApruebaFactura.Text = infoT.SolicitadoPor
            txtNoOrden.Text = info.id
            txtProveedor.Text = infoT.Proveedor
            hfEstado.Value = info.Estado
            Me.lblSubtotal.InnerText = "Subtotal: " & IIf(info.Subtotal Is Nothing, FormatCurrency(0), FormatCurrency(info.Subtotal, 0))
        ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
            Dim infoT = o.ObtenerOrdenesDeRequerimiento(hfId.Value, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
            Dim info = o.ObtenerOrdenRequerimiento(hfId.Value)
            Dim infoLogApp = o.ObtenerLogAprobacionesOrdenRequerimiento(hfId.Value).FirstOrDefault
            Dim idGrupounidad As Short?
            txtBeneficiario.Text = info.Beneficiario
            If Not info.EvaluaProveedor Is Nothing Then hfEvaluador.Value = info.EvaluaProveedor
            ddlTipo.SelectedValue = info.TipoDetalle
            If ddlTipo.SelectedValue <> eTipo.CentroCosto Then
                txtJBIJBE.Text = info.JobBookCodigo
                txtNombreJBIJBE.Text = info.JobBookNombre
                txtJBIJBE.Visible = True
                txtNombreJBIJBE.Visible = True
                lblJBIJBE.Visible = True
                ddlApruebaOrden.Visible = True
                lblApruebaOrden.Visible = True
                lblNombreJBIJBE.Visible = True
                lblCentroCostos.Visible = False
                ddlCentroCostos.Visible = False
                If ddlTipo.SelectedValue = eTipo.JBE Then
                    idGrupounidad = obtenerIdGrupoUnidad(txtJBIJBE.Text, ddlTipo.Text)
                End If
                cargarPersonasAprueban(ddlTipo.SelectedValue, If(idGrupounidad.HasValue, idGrupounidad, Nothing), True)
                If Not infoLogApp Is Nothing Then
                    ddlApruebaOrden.SelectedValue = infoLogApp.UsuarioId
                End If
            Else
                txtJBIJBE.Visible = False
                txtNombreJBIJBE.Visible = False
                lblJBIJBE.Visible = False
                lblNombreJBIJBE.Visible = False
                ddlApruebaOrden.Visible = False
                lblApruebaOrden.Visible = False
                lblCentroCostos.Visible = True
                ddlCentroCostos.Visible = True
                cargarCentroCostoDropDown(ddlCentroCostos, info.TipoDetalle)

            End If

            If info.CentroDeCosto Is Nothing Then
                If info.JobBook <> "" Then
                    hfCentroCosto.Value = info.JobBook
                Else
                    hfCentroCosto.Value = ""
                End If
            Else
                hfCentroCosto.Value = info.CentroDeCosto
                ddlCentroCostos.SelectedValue = info.CentroDeCosto
            End If

            ddlDepartamento.SelectedValue = info.Departamento
            CargarCiudades()
            ddlCiudad.SelectedValue = info.Ciudad
            txtFecha.Text = info.Fecha
            txtFechaEntrega.Text = info.FechaEntrega
            txtFormaPago.Text = info.FormaPago
            hfProveedor.Value = info.ProveedorId
            hfSolicitante.Value = info.SolicitadoPor
            txtApruebaFactura.Text = infoT.SolicitadoPor
            txtNoOrden.Text = info.id
            txtProveedor.Text = infoT.Proveedor
            hfEstado.Value = info.Estado
            Me.lblSubtotal.InnerText = "Subtotal: " & IIf(info.Subtotal Is Nothing, FormatCurrency(0), FormatCurrency(info.Subtotal, 0))
        End If
        cargarCuentasContablesDropDown()
        CargarDetalle()
        Me.pnlOrden.Visible = True
        Me.pnlDetalleOrden.Visible = True
        Me.pnlBuscar.Visible = False
        If hfEstado.Value = 1 Then
            btnEnviarAprobacion.Visible = True
            Me.txtComentarios.Visible = True
        Else
            btnEnviarAprobacion.Visible = False
            Me.txtComentarios.Visible = True
        End If
        If hfEstado.Value > 2 Then
            btnEnviarAprobacion.Visible = False
            Me.txtComentarios.Visible = False
            Me.btnAdd.Visible = False
            Me.btnCancelDetalle.Visible = False
            'Me.gvDetalle.Columns(5).Visible = False
        End If
        If hfEstado.Value = eEstados.Anulada Then
            lblAnulada.Visible = True
            btnSave.Visible = False
            btnCancel.Visible = False
            btnAnular.Visible = False
            Me.gvDetalle.Columns(5).Visible = False
        ElseIf hfEstado.Value = eEstados.Aprobada Or hfEstado.Value = eEstados.EnAprobacion Then
            lblAnulada.Visible = False
            btnSave.Visible = False
            btnCancel.Visible = False
            btnAnular.Visible = True
            Me.gvDetalle.Columns(5).Visible = False
        Else
            lblAnulada.Visible = False
            btnSave.Visible = True
            btnCancel.Visible = True
            btnAnular.Visible = True
            Me.gvDetalle.Columns(5).Visible = True
        End If
        bloquearRespuestas()
        If hfEstado.Value = 2 Or hfEstado.Value = 6 Then
            Me.btnCancelEnvio.Visible = True
        Else
            Me.btnCancelEnvio.Visible = False
        End If
    End Sub

    Private Sub gvDetalle_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvDetalle.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.FindControl("imgArchivos").Visible = If(hfEstado.Value = 7, False, True)
        End If
    End Sub

    Protected Sub btnLoadFile_Click(sender As Object, e As EventArgs) Handles btnLoadFile.Click
        lblFileIncorrect.Visible = False
        'Verifica que hayan seleccionado una ruta correcta
        If FileUpData.HasFile Then
            'La carpeta donde voy a almacer el archivo
            Dim path As String = Server.MapPath("~/Files/")
            Dim fileload As New System.IO.FileInfo(FileUpData.FileName)
            'Verifica que las extensiones sean las permitidas, dependiendo de la extensión llama la función
            If fileload.Extension = ".xls" Then
                FileUpData.SaveAs(path & "CargueOrden.xls")
                hfTypeFile.Value = 0
                ReadExcel(0, path & "CargueOrden.xls")
            ElseIf fileload.Extension = ".xlsx" Then
                FileUpData.SaveAs(path & "CargueOrden.xlsx")
                hfTypeFile.Value = 1
                ReadExcel(1, path & "CargueOrden.xlsx")
            Else
                lblFileIncorrect.Visible = True
                Exit Sub
            End If

            CargarDetalle()
        End If
    End Sub

    'Procedimiento que lee el archivo de Excel
    Sub ReadExcel(ByVal typefile As Int16, ByVal path As String)
        Dim connstr As String = ""
        'Dependiendo del tipo de archivo configura la cadena de conexión
        If typefile = 0 Then
            connstr = "Provider=Microsoft.Jet.OLEDB.4.0;" & "Data Source=" & path & ";" & "Extended Properties='Excel 8.0'"
        ElseIf typefile = 1 Then
            connstr = "Provider=Microsoft.ACE.OLEDB.12.0;" & "Data Source=" & path & ";" & "Extended Properties='Excel 12.0'"
        End If

        'El objeto System.IO.FileInfo guarda todas las propiedades del archivo, nombre, extensión, tamaño, etc.
        LoadRecords("CargueOrden".Replace("$", "").Replace("'", ""), connstr, New System.IO.FileInfo(path & "CargueOrden.xls"))

        'Gestion la cadena de conexión
        Using cnn As New OleDbConnection(connstr)
            'Abre la conexión
            cnn.Open()

            'Crea un datatable que lee y lista las hojas del archivo
            Dim tables As DataTable = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})

            cnn.Close()
        End Using


    End Sub

    Sub LoadRecords(ByVal NameSheet As String, ByVal connstr As String, fileloadinfo As System.IO.FileInfo)

        'La instrucción SQL para leer los datos de la hoja. 
        Dim sqlcmd As String = "SELECT * FROM [" & NameSheet & "$A1:F51]"
        Dim dt As DataTable = New DataTable
        'Abre la cadena de conexión que fue enviada como parámetro
        Using conn As OleDbConnection = New OleDbConnection(connstr)
            'Ejecuta un dataaapter para abrir la base y ejecutar el comando
            Using da As OleDbDataAdapter = New OleDbDataAdapter(sqlcmd, conn)
                'Llena el objeto dt que es un datatable con la información de la hoja
                da.Fill(dt)
            End Using
        End Using
        'Verifica que efectivamente traiga datos
        If dt.Rows.Count = 0 Then
            AlertJS("Error: No sé encontro ningún registro")
            FileUpData.Focus()
            Exit Sub
        End If

        'Columnas que debe contener la plantilla
        Dim MiVectorColumnas(6) As String
        MiVectorColumnas(0) = "CuentaContable"
        MiVectorColumnas(1) = "CtaContable"
        MiVectorColumnas(2) = "Fecha"
        MiVectorColumnas(3) = "Descripcion"
        MiVectorColumnas(4) = "Cantidad"
        MiVectorColumnas(5) = "VrUnitario"

        'Verificar las columnas en el excel

        If dt.Columns.Count < 5 Then
            AlertJS("Error: Las columnas del archivo no coinciden con la plantilla, por favor revise!")
            FileUpData.Focus()
            Exit Sub
        End If

        Dim MiError As Integer
        MiError = 0

        For i = 0 To 5
            If dt.Columns.Count > i And dt.Columns(i).ColumnName <> MiVectorColumnas(i) Then MiError = MiError + 1
        Next

        If MiError > 0 Then
            AlertJS("Error: Las columnas del archivo no coinciden con la plantilla, por favor revise!")
            FileUpData.Focus()
            Exit Sub
        Else
            For i As Integer = 0 To dt.Rows.Count - 1
                If dt.Rows(i).Item(0).ToString() <> "" Then
                    ddlCuentasContables.SelectedValue = dt.Rows(i).Item(1).ToString()
                    txtFecha.Text = dt.Rows(i).Item(2).ToString()
                    txtDescripcionDetalle.Text = dt.Rows(i).Item(3).ToString()
                    txtCantidad.Text = dt.Rows(i).Item(4).ToString()
                    txtValorUnitario.Text = dt.Rows(i).Item(5).ToString()
                    If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
                        GuardarDetalleOS()
                    ElseIf hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
                        GuardarDetalleOC()
                    ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
                        GuardarDetalleOR()
                    End If
                Else
                    Exit For
                End If
            Next
            AlertJS("La información fue cargada correctamente")
        End If

    End Sub

    Protected Sub btnduplicar_Click(sender As Object, e As EventArgs) Handles btnduplicar.Click
        hfDuplicar.Value = 1
        hfIdAnterior.Value = hfId.Value
        txtNoOrden.Text = ""
        'txtProveedor.Text = ""
        'hfProveedor.Value = 0
        hfId.Value = 0
        btnSave.Visible = True
        btnSearchProveedor.Enabled = True
        hfEstado.Value = 1
        LimpiarDetalle()
        pnlDetalleOrden.Visible = False
        bloquearRespuestas()
        Validar()
        CargarDetalle()
    End Sub


End Class
