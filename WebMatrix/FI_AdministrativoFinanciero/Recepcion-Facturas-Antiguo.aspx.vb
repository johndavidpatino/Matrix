Imports CoreProject
Imports WebMatrix.Util
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO

Public Class FI_Recepcion_Facturas_Antiguo
    Inherits System.Web.UI.Page

    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Page.Form.Attributes.Add("enctype", "multipart/form-data")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarCC()
            CargarDepartamentos()
        End If

        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnLoadFile)
        smanager.RegisterPostBackControl(Me.btnViewFile)
    End Sub

    Private Sub gvDetalle_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDetalle.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            If CType(e.Row.DataItem, FI_FacturasRadicadas).Estado = 0 Then
                DirectCast(e.Row.FindControl("imgBtnActualizar"), ImageButton).Visible = True
                DirectCast(e.Row.FindControl("imgBtnBorrar"), ImageButton).Visible = True
            Else
                DirectCast(e.Row.FindControl("imgBtnActualizar"), ImageButton).Visible = False
                DirectCast(e.Row.FindControl("imgBtnBorrar"), ImageButton).Visible = False
            End If

            If CType(e.Row.DataItem, FI_FacturasRadicadas).Estado = 6 Then
                DirectCast(e.Row.FindControl("imgBtnAnular"), ImageButton).Visible = False
            Else
                DirectCast(e.Row.FindControl("imgBtnAnular"), ImageButton).Visible = True
            End If
        End If

    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        hfTipoOrden.Value = 1
        Me.pnlTareasOrdenes.Visible = True
        Me.txtTitulo.Text = "Ordenes de Servicio"
        Limpiar()
        LimpiarDetalle()
        Me.pnlBuscar.Visible = False
        Me.pnlDetalleOrden.Visible = False
        Me.pnlOrden.Visible = False
        Me.pnlAprobaciones.Visible = False
        Me.pnlAnulacion.Visible = False
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs) Handles LinkButton2.Click
        hfTipoOrden.Value = 2
        Me.pnlTareasOrdenes.Visible = True
        Me.txtTitulo.Text = "Ordenes de Compra"
        Limpiar()
        LimpiarDetalle()
        Me.pnlBuscar.Visible = False
        Me.pnlDetalleOrden.Visible = False
        Me.pnlOrden.Visible = False
        Me.pnlAprobaciones.Visible = False
        Me.pnlAnulacion.Visible = False
    End Sub


    Private Sub lbMenu2_Click(sender As Object, e As EventArgs) Handles lbMenu2.Click
        Me.pnlOrden.Visible = False
        Me.pnlBuscar.Visible = True
        Me.pnlDetalleOrden.Visible = False
        Me.pnlAnulacion.Visible = False
        Limpiar()
        Me.gvOrdenes.DataSource = Nothing
        Me.gvOrdenes.DataBind()
    End Sub

    Private Sub lbMenu3_Click(sender As Object, e As EventArgs) Handles lbMenu3.Click
        If hfId.Value = "0" Then Exit Sub
        Me.pnlAprobaciones.Visible = True
        Me.pnlBuscar.Visible = False
        Me.pnlAnulacion.Visible = False
        If hfEstado.Value = 1 Then
            btnAprobar.Visible = False
            btnEnviarAprobacion.Visible = True
            Me.txtComentarios.Visible = True
        Else
            btnAprobar.Visible = True
            btnEnviarAprobacion.Visible = False
            Me.txtComentarios.Visible = True
        End If
        If hfEstado.Value > 2 Then
            btnAprobar.Visible = False
            btnEnviarAprobacion.Visible = False
            Me.txtComentarios.Visible = False
            Me.btnAdd.Visible = False
            Me.gvDetalle.Columns(4).Visible = False
        End If
        CargarLogAprobaciones()
        Dim o As New FI.Ordenes
        If hfTipoOrden.Value = 1 Then
            If hfEstado.Value = 2 Then
                If o.ObtenerUsuarioAprobacionOS(hfId.Value, Session("IDUsuario").ToString) = True Then
                    btnAprobar.Visible = True
                    btnNoAprobar.Visible = True
                    btnEnviarAprobacion.Visible = False
                    Me.txtComentarios.Visible = True
                Else
                    btnAprobar.Visible = False
                    btnNoAprobar.Visible = False
                    btnEnviarAprobacion.Visible = False
                    Me.txtComentarios.Visible = False
                End If
            End If
        ElseIf o.ObtenerUsuarioAprobacionOC(hfId.Value, Session("IDUsuario").ToString) = True Then
            btnAprobar.Visible = True
            btnNoAprobar.Visible = True
            btnEnviarAprobacion.Visible = False
            Me.txtComentarios.Visible = True
        Else
            btnAprobar.Visible = False
            btnNoAprobar.Visible = False
            btnEnviarAprobacion.Visible = False
            Me.txtComentarios.Visible = False
        End If
    End Sub
    Sub CargarLogAprobaciones()
        Dim o As New FI.Ordenes
        If hfTipoOrden.Value = 1 Then
            Me.gvAprobaciones.DataSource = o.ObtenerLogAprobacionesOrdenServicio(hfId.Value)

        Else
            Me.gvAprobaciones.DataSource = o.ObtenerLogAprobacionesOrdenCompra(hfId.Value)
        End If
        Me.gvAprobaciones.DataBind()
    End Sub

    Sub Limpiar()
        txtBeneficiario.Text = ""
        ddlCentroCostos.ClearSelection()
        ddlCiudad.SelectedValue = -1
        ddlDepartamento.SelectedValue = -1
        txtFecha.Text = ""
        txtFechaEntrega.Text = ""
        txtFormaPago.Text = ""
        hfProveedor.Value = "0"
        hfSolicitante.Value = ""
        ddlTipo.SelectedValue = -1
        hfId.Value = "0"
        txtNoOrden.Text = ""
        txtProveedor.Text = ""
        txtSolicitante.Text = ""
        LimpiarDetalle()
    End Sub

    Sub LimpiarDetalle()
        txtConsecutivo.Text = ""
        txtNoFactura.Text = ""
        txtCantidad.Text = ""
        txtValorUnitario.Text = ""
        txtSubTotal.Text = ""
        txtValorTotal.Text = ""
        txtObservaciones.Text = ""
        ddlTipoDocRadicado.ClearSelection()
        hfDetalleId.Value = "0"
        hfIdFactura.Value = "0"
        txtAnular.Text = ""
        hfSolicitanteSearch.Value = "0"
        txtUsAnula.Text = ""
        pnlAnulacion.Visible = False

    End Sub

    Private Sub btnSearchOk_Click(sender As Object, e As EventArgs) Handles btnSearchOk.Click
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
        If chbMisOrdenes.Checked = True Then elaboradopor = Session("IDUsuario").ToString
        If Not txtJobBookSearch.Text = "" Then jobbook = txtJobBookSearch.Text
        If Not ddlCentroDeCostosSearch.SelectedValue = "-1" Then cc = ddlCentroDeCostosSearch.SelectedValue
        Dim o As New FI.Ordenes
        If Me.hfTipoOrden.Value = 1 Then
            Me.gvOrdenes.DataSource = o.ObtenerOrdenesDeServicio(id, fecha, proveedor, solicitadopor, elaboradopor, jobbook, cc, 3)
        Else
            Me.gvOrdenes.DataSource = o.ObtenerOrdenesDeCompra(id, fecha, proveedor, solicitadopor, elaboradopor, jobbook, cc, 3)
        End If
        Me.gvOrdenes.DataBind()
    End Sub

    Private Sub btnSearchCancel_Click(sender As Object, e As EventArgs) Handles btnSearchCancel.Click
        txtOrdenSearch.Text = ""
        txtFechaSearch.Text = ""
        hfProveedorSearch.Value = "0"
        hfSolicitanteSearch.Value = "0"
        txtJobBookSearch.Text = ""
        ddlCentroDeCostosSearch.SelectedValue = -1
        Me.pnlBuscar.Visible = False
    End Sub

    Sub CargarCC()
        Dim o As New FI.Ordenes

        Me.ddlCentroDeCostosSearch.DataSource = o.ObtenerJBE_JBI_CC(3, Nothing)
        Me.ddlCentroDeCostosSearch.DataTextField = "Nombre"
        Me.ddlCentroDeCostosSearch.DataValueField = "id"
        Me.ddlCentroDeCostosSearch.DataBind()
        Me.ddlCentroDeCostosSearch.Items.Insert(0, New System.Web.UI.WebControls.ListItem With {.Value = "-1", .Text = "--Todos | N/A--"})
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

    Private Sub ddlDepartamento_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlDepartamento.SelectedIndexChanged
        CargarCiudades()
    End Sub

    Private Sub ddlTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipo.SelectedIndexChanged
        CargarCCJBSegunTipo()
    End Sub

    Sub CargarCCJBSegunTipo()
        Dim o As New FI.Ordenes
        Me.ddlCentroCostos.DataSource = o.ObtenerJBE_JBI_CC(Me.ddlTipo.SelectedValue, Nothing)
        Me.ddlCentroCostos.DataTextField = "Nombre"
        Me.ddlCentroCostos.DataValueField = "id"
        Me.ddlCentroCostos.DataBind()
        Me.ddlCentroCostos.Items.Insert(0, New System.Web.UI.WebControls.ListItem With {.Value = "-1", .Text = "--Seleccione--"})
        If ddlTipo.SelectedValue = -1 Then
            Me.ddlCentroCostos.Enabled = False
        Else
            Me.ddlCentroCostos.Enabled = True
        End If
    End Sub
    Sub CargarGridPersonas()
        Dim o As New CoreProject.RegistroPersonas
        Dim cedula As Int64? = Nothing
        Dim nombre As String = Nothing
        If IsNumeric(txtCedulaSolicitante.Text) Then cedula = txtCedulaSolicitante.Text
        If Not txtNombreSolicitante.Text = "" Then nombre = txtNombreSolicitante.Text
        Me.gvSolicitantes.DataSource = o.TH_PersonasGet(cedula, nombre)
        Me.gvSolicitantes.DataBind()
    End Sub

    Private Sub btnBuscarSolicitante_Click(sender As Object, e As EventArgs) Handles btnBuscarSolicitante.Click
        CargarGridPersonas()
        UPanelSolicitantes.Update()
    End Sub

    Private Sub gvSolicitantes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvSolicitantes.RowCommand
        If e.CommandName = "Seleccionar" Then
            If hfTipoBusqueda.Value = 1 Then
                hfSolicitante.Value = Me.gvSolicitantes.DataKeys(CInt(e.CommandArgument))("id")
                Me.txtSolicitante.Text = Server.HtmlDecode(Me.gvSolicitantes.Rows(CInt(e.CommandArgument)).Cells(0).Text.ToString) & " " & Server.HtmlDecode(Me.gvSolicitantes.Rows(CInt(e.CommandArgument)).Cells(1).Text.ToString)
            Else
                hfSolicitanteSearch.Value = Me.gvSolicitantes.DataKeys(CInt(e.CommandArgument))("id")
                Me.txtSolicitanteBusqueda.Text = Server.HtmlDecode(Me.gvSolicitantes.Rows(CInt(e.CommandArgument)).Cells(0).Text.ToString) & " " & Server.HtmlDecode(Me.gvSolicitantes.Rows(CInt(e.CommandArgument)).Cells(1).Text.ToString)
                Me.txtUsAnula.Text = Server.HtmlDecode(Me.gvSolicitantes.Rows(CInt(e.CommandArgument)).Cells(0).Text.ToString) & " " & Server.HtmlDecode(Me.gvSolicitantes.Rows(CInt(e.CommandArgument)).Cells(1).Text.ToString)
            End If
        End If
    End Sub

    Sub ValidarDetalle()
        Dim daFacturas As New FI.Facturas
        If Not IsNumeric(txtConsecutivo.Text) Then
            AlertJS("Escriba el consecutivo")
            txtConsecutivo.Focus()
            Exit Sub
        End If
        If Not IsNumeric(txtValorTotal.Text) Then
            AlertJS("Escriba el valor total")
            txtValorUnitario.Focus()
            Exit Sub
        End If

        If hfIdFactura.Value = 0 Then
            If daFacturas.obtenerFacturaRadicada(Nothing, If(Int64.TryParse(txtConsecutivo.Text, New Integer), txtConsecutivo.Text, Nothing), Date.Now.Year).Count > 0 Then
                AlertJS("El número de radicado ya se ingreso")
                txtConsecutivo.Focus()
                Exit Sub
            End If
        End If
        GuardarFactura()
    End Sub

    Sub GuardarFactura()
        Dim o As New FI.Facturas
        Dim e As New FI_FacturasRadicadas

        If Not hfIdFactura.Value = 0 Then
            e = o.ObtenerFactura(hfIdFactura.Value)
        End If

        If IsNumeric(txtCantidad.Text) Then e.Cantidad = txtCantidad.Text
        e.Consecutivo = txtConsecutivo.Text
        e.Fecha = Date.UtcNow.AddHours(-5)
        e.IdOrden = hfId.Value
        e.Observaciones = txtObservaciones.Text
        e.NoFactura = txtNoFactura.Text
        If IsNumeric(txtValorUnitario.Text) Then e.VrUnitario = txtValorUnitario.Text
        If IsNumeric(txtValorTotal.Text) Then e.ValorTotal = txtValorTotal.Text
        If IsNumeric(txtSubTotal.Text) Then e.Subtotal = txtSubTotal.Text
        e.Usuario = Session("IDUsuario").ToString
        e.TipoDocumento = ddlTipoDocRadicado.SelectedValue
        e.Tipo = hfTipoOrden.Value
        e.Estado = 0
        o.GuardarFactura(e)

        If hfIdFactura.Value = "0" Then
            Dim e2 As New FI_LogAprobacionFacturas
            e2.Estado = 0
            e2.Fecha = Date.UtcNow.AddHours(-5)
            e2.IdFactura = e.id
            e2.Usuario = Session("IDUsuario").ToString
            o.GuardarLogFactura(e2)
        End If

        LimpiarDetalle()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ValidarDetalle()
        CargarDetalle()
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        LimpiarDetalle()
    End Sub

    Sub CargarDetalle()
        Dim o As New FI.Facturas
        Me.gvDetalle.DataSource = o.ObtenerFacturasRadicadas(hfId.Value, hfTipoOrden.Value)
        Me.gvDetalle.DataBind()
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
        If hfTipoOrden.Value = 1 Then
            Dim infoT = o.ObtenerOrdenesDeServicio(hfId.Value, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, 3).FirstOrDefault
            Dim info = o.ObtenerOrdenServicio(hfId.Value)
            txtBeneficiario.Text = info.Beneficiario
            ddlTipo.SelectedValue = info.TipoDetalle
            CargarCCJBSegunTipo()
            If ddlTipo.SelectedValue <> 3 Then
                txtJBIJBE.Text = info.JobBookCodigo
                txtNombreJBIJBE.Text = info.JobBookNombre
                txtJBIJBE.Visible = True
                txtNombreJBIJBE.Visible = True
                lblJBIJBE.Visible = True
                lblNombreJBIJBE.Visible = True
                lblCentroCostos.Visible = False
                ddlCentroCostos.Visible = False
            Else
                txtJBIJBE.Visible = False
                txtNombreJBIJBE.Visible = False
                lblJBIJBE.Visible = False
                lblNombreJBIJBE.Visible = False
                lblCentroCostos.Visible = True
                ddlCentroCostos.Visible = True
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
            txtNoOrden.Text = info.id
            txtProveedor.Text = infoT.Proveedor
            txtSolicitante.Text = infoT.SolicitadoPor
            hfEstado.Value = info.Estado
            Me.lblSubtotal.Text = "Subtotal: " & IIf(info.Subtotal Is Nothing, FormatCurrency(0), FormatCurrency(info.Subtotal, 0))
            'Me.lblVrTotal.Text = "Total: " & IIf(info.Total Is Nothing, FormatCurrency(0), FormatCurrency(info.Total, 0))
        Else
            Dim infoT = o.ObtenerOrdenesDeCompra(hfId.Value, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, 3).FirstOrDefault
            Dim info = o.ObtenerOrdenCompra(hfId.Value)
            txtBeneficiario.Text = info.Beneficiario
            ddlTipo.SelectedValue = info.TipoDetalle
            CargarCCJBSegunTipo()
            If ddlTipo.SelectedValue <> 3 Then
                txtJBIJBE.Text = info.JobBookCodigo
                txtNombreJBIJBE.Text = info.JobBookNombre
                txtJBIJBE.Visible = True
                txtNombreJBIJBE.Visible = True
                lblJBIJBE.Visible = True
                lblNombreJBIJBE.Visible = True
                lblCentroCostos.Visible = False
                ddlCentroCostos.Visible = False
            Else
                txtJBIJBE.Visible = False
                txtNombreJBIJBE.Visible = False
                lblJBIJBE.Visible = False
                lblNombreJBIJBE.Visible = False
                lblCentroCostos.Visible = True
                ddlCentroCostos.Visible = True
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
            txtNoOrden.Text = info.id
            txtProveedor.Text = infoT.Proveedor
            txtSolicitante.Text = infoT.SolicitadoPor
            hfEstado.Value = info.Estado
            Me.lblSubtotal.Text = "Subtotal: " & IIf(info.Subtotal Is Nothing, FormatCurrency(0), FormatCurrency(info.Subtotal, 0))
            'Me.lblVrTotal.Text = "Total: " & IIf(info.Total Is Nothing, FormatCurrency(0), FormatCurrency(info.Total, 0))
        End If
        CargarDetalle()
        Me.pnlOrden.Visible = True
        Me.pnlDetalleOrden.Visible = True
        Me.pnlBuscar.Visible = False
    End Sub

    Private Sub gvDetalle_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDetalle.RowCommand
        Dim o As New FI.Facturas

        If e.CommandName = "Actualizar" Then
            Me.pnlAnulacion.Visible = False

            Dim lst = o.ObtenerFactura(gvDetalle.DataKeys(CInt(e.CommandArgument))("id"))

            txtConsecutivo.Text = lst.Consecutivo
            If lst.NoFactura IsNot Nothing Then txtNoFactura.Text = lst.NoFactura
            If lst.Cantidad IsNot Nothing Then txtCantidad.Text = lst.Cantidad
            If lst.VrUnitario IsNot Nothing Then txtValorUnitario.Text = lst.VrUnitario
            If lst.Subtotal IsNot Nothing Then txtSubTotal.Text = lst.Subtotal
            txtValorTotal.Text = lst.ValorTotal
            If lst.Observaciones IsNot Nothing Then txtObservaciones.Text = lst.Observaciones
            ddlTipoDocRadicado.SelectedValue = lst.TipoDocumento
            hfId.Value = lst.IdOrden
            hfTipoOrden.Value = lst.Tipo
            hfIdFactura.Value = lst.id
        End If
        If e.CommandName = "Borrar" Then
            Me.pnlAnulacion.Visible = False
            o.borrarFactura(gvDetalle.DataKeys(CInt(e.CommandArgument))("id"))
        End If
        If e.CommandName = "Anular" Then
            hfIdFactura.Value = gvDetalle.DataKeys(CInt(e.CommandArgument))("id")
            Me.pnlAnulacion.Visible = True
        End If
        If e.CommandName = "Load" Then
            hfIdFactura.Value = gvDetalle.DataKeys(CInt(e.CommandArgument))("id")

        End If



        CargarDetalle()
    End Sub


    Private Sub btnEnviarAprobacion_Click(sender As Object, e As EventArgs) Handles btnEnviarAprobacion.Click
        Dim o As New FI.Ordenes
        If hfTipoOrden.Value = 1 Then
            Dim ent As New FI_LogAprobacionOrdenesServicio
            ent.IdOrden = hfId.Value
            ent.Usuario = Session("IDUsuario").ToString
            ent.Observaciones = txtComentarios.Text
            ent.FechaAprobacion = Date.UtcNow.AddHours(-5)
            ent.Nivel = 0
            ent.Aprobado = True
            o.EnviarAprobacionOrdenServicio(ent)
            Dim entO = o.ObtenerOrdenServicio(hfId.Value)
            entO.Estado = 2
            o.GuardarOrdenServicio(entO)
            o.ContinuarAprobacionOrdenServicio(hfId.Value)
        Else
            Dim ent As New FI_LogAprobacionOrdenesCompra
            ent.IdOrden = hfId.Value
            ent.Usuario = Session("IDUsuario").ToString
            ent.Observaciones = txtComentarios.Text
            ent.FechaAprobacion = Date.UtcNow.AddHours(-5)
            ent.Nivel = 0
            ent.Aprobado = True
            o.EnviarAprobacionOrdenCompra(ent)
            Dim entO = o.ObtenerOrdenCompra(hfId.Value)
            entO.Estado = 2
            o.GuardarOrdenCompra(entO)
            o.ContinuarAprobacionOrdenCompra(hfId.Value)
        End If

        CargarLogAprobaciones()
        Me.btnAprobar.Visible = False
        Me.txtComentarios.Visible = False
        Me.btnEnviarAprobacion.Visible = False
    End Sub

    Private Sub btnAprobar_Click(sender As Object, e As EventArgs) Handles btnAprobar.Click
        Dim o As New FI.Ordenes
        If hfTipoOrden.Value = 1 Then
            o.AprobarOS(hfId.Value, txtComentarios.Text, Session("IDUsuario").ToString, True)
        Else
            o.AprobarOC(hfId.Value, txtComentarios.Text, Session("IDUsuario").ToString, True)
        End If
        CargarLogAprobaciones()
        Me.btnAprobar.Visible = False
        Me.btnNoAprobar.Visible = False
        Me.txtComentarios.Visible = False
        Me.btnEnviarAprobacion.Visible = False
    End Sub

    Private Sub btnNoAprobar_Click(sender As Object, e As EventArgs) Handles btnNoAprobar.Click
        Dim o As New FI.Ordenes
        If hfTipoOrden.Value = 1 Then
            o.AprobarOS(hfId.Value, txtComentarios.Text, Session("IDUsuario").ToString, False)
        Else
            o.AprobarOC(hfId.Value, txtComentarios.Text, Session("IDUsuario").ToString, False)
        End If
        CargarLogAprobaciones()
        Me.btnAprobar.Visible = False
        Me.btnNoAprobar.Visible = False
        Me.txtComentarios.Visible = False
        Me.btnEnviarAprobacion.Visible = False
    End Sub

    Protected Sub btnAnular_Click(sender As Object, e As EventArgs) Handles btnAnular.Click
        Dim e1 As New FI_FacturasRadicadas
        Dim o As New FI.Facturas

        If txtAnular.Text = "" Then
            AlertJS("Debe escribir el motivo de la Anulación de la Factura!")
            txtAnular.Focus()
            Exit Sub
        End If

        If txtUsAnula.Text = "" Then
            AlertJS("Debe seleccionar el usuario que autoriza la anulación!")
            txtUsAnula.Focus()
            Exit Sub
        End If

        If hfSolicitanteSearch.Value = 0 Then
            AlertJS("Debe seleccionar el Usuario que autoriza la anulación!")
            hfSolicitanteSearch.Focus()
            Exit Sub
        End If

        e1 = o.ObtenerFactura(hfIdFactura.Value)

        e1.Fecha = Date.UtcNow.AddHours(-5)
        e1.Usuario = Session("IDUsuario").ToString
        e1.Estado = 6
        o.GuardarFactura(e1)

        Dim e2 As New FI_LogAprobacionFacturas
        e2.Estado = 6
        e2.Fecha = Date.UtcNow.AddHours(-5)
        e2.IdFactura = e1.id
        e2.Usuario = Session("IDUsuario").ToString
        e2.Observaciones = txtAnular.Text
        e2.UsuarioAnula = hfSolicitanteSearch.Value
        o.GuardarLogFactura(e2)

        LimpiarDetalle()

        AlertJS("La Factura ha sido Anulada!")
        CargarDetalle()

    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        LimpiarDetalle()
        pnlAnulacion.Visible = False
    End Sub

    Private Sub _RecepcionFacturas_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(146, UsuarioID) = False Then
            Response.Redirect("../FI_AdministrativoFinanciero/Default-Compras.aspx")
        End If
    End Sub

#Region "PDF"

    Sub ConstruirPDFOs(ByVal id As Long, ByVal document As Document)
        Dim font As New Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL)
        Dim fontB As New Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD)
        Dim path As String = Server.MapPath("~/Files/")
        Dim tabla As New PdfPTable(4)
        Dim Ancho As Single() = {Ssg(4), Ssg(9), Ssg(4), Ssg(4)}
        tabla.SetWidths(Ancho)
        tabla.DefaultCell.Border = 0
        tabla.WidthPercentage = 100

        Dim Col_Title1 As New iTextSharp.text.BaseColor(204, 232, 212)
        Dim Col_Title2 As New iTextSharp.text.BaseColor(242, 249, 220)


        Dim PdfCell As New PdfPCell

        tabla = New PdfPTable(1)
        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 0, 0, 0)
        PdfCell.HorizontalAlignment = 3

        Try


            Dim titulo As String = ""
            titulo = "ORDEN DE SERVICIO"

            Dim Parag As New Paragraph(titulo, font)
            'Parag.Alignment = 1
            'PdfCell.AddElement(Parag)
            'tabla.AddCell(PdfCell)
            'document.Add(tabla)

            tabla = New PdfPTable(1)
            Ancho = {Ssg(21)}
            tabla.SetWidths(Ancho)
            tabla.WidthPercentage = 100
            tabla.AddCell(NewPdfCellGray(titulo, 1, True, True))
            document.Add(tabla)

            document.Add(NewRenglonEnter())

            tabla = New PdfPTable(9)
            Ancho = {Ssg(2), Ssg(2), Ssg(4), Ssg(0.5), Ssg(2), Ssg(4), Ssg(0.5), Ssg(2), Ssg(4)}
            tabla.SetWidths(Ancho)
            tabla.WidthPercentage = 100

            Dim Imagen As iTextSharp.text.Image
            Imagen = iTextSharp.text.Image.GetInstance(path & "Ipsos.png")

            'Dim o As New DatosReportes.Reportes
            'Dim info = o.InfoDespacho(IDIngreso)
            Imagen.ScalePercent(25)
            PdfCell = New PdfPCell
            CellBorders(PdfCell, 0, 0, 0, 0)
            'PdfCell.AddElement(New Paragraph("", font))
            PdfCell.HorizontalAlignment = 1
            PdfCell.VerticalAlignment = 1
            PdfCell.Rowspan = 4
            PdfCell.AddElement(Imagen)
            tabla.AddCell(PdfCell)

            tabla.AddCell(NewPdfCell("Cliente", 2, True, False))
            tabla.AddCell(NewPdfCell("Cliente", 2, True, False))
            'tabla.AddCell(NewPdfCell("", 2, True, False))
            tabla.AddCell(NewPdfCell("CC", 2, True, False))
            tabla.AddCell(NewPdfCell("Cliente", 2, True, False))
            tabla.AddCell(NewPdfCell("", 2, True, False))
            tabla.AddCell(NewPdfCell("CDE", 2, True, False))
            tabla.AddCell(NewPdfCell("Cliente", 2, True, False))

            tabla.AddCell(NewPdfCell("IMEI", 2, True, False))
            tabla.AddCell(NewPdfCell("Cliente", 2, True, False))
            tabla.AddCell(NewPdfCell("", 3, True, False))
            tabla.AddCell(NewPdfCell("Marca", 2, True, False))
            tabla.AddCell(NewPdfCell("Cliente", 2, True, False))
            tabla.AddCell(NewPdfCell("", 3, True, False))
            tabla.AddCell(NewPdfCell("Modelo", 2, True, False))
            tabla.AddCell(NewPdfCell("Cliente", 2, True, False))

            tabla.AddCell(NewPdfCell("Fecha Ingreso", 2, True, False))
            tabla.AddCell(NewPdfCell("Cliente", 2, True, False))
            tabla.AddCell(NewPdfCell("", 3, True, False))
            tabla.AddCell(NewPdfCell("Asesor Ingreso", 2, True, False))
            tabla.AddCell(NewPdfCell("Cliente", 2, True, False))
            'tabla.AddCell(NewPdfCell("", 2, True, False))
            tabla.AddCell(NewPdfCell("ODS", 2, True, False))
            tabla.AddCell(NewPdfCell("Cliente", 2, True, False))

            tabla.AddCell(NewPdfCell("Fecha Entrega", 2, True, False))
            tabla.AddCell(NewPdfCell("Cliente", 2, True, False))
            tabla.AddCell(NewPdfCell("", 3, True, False))
            tabla.AddCell(NewPdfCell("Asesor Entrega", 2, True, False))
            tabla.AddCell(NewPdfCell("Cliente", 2, True, False))
            'tabla.AddCell(NewPdfCell("", 3, True, False))
            tabla.AddCell(NewPdfCell("", 3, True, False))
            tabla.AddCell(NewPdfCell("", 3, True, False))
            document.Add(tabla)

            document.Add(NewRenglonEnter())

            tabla = New PdfPTable(3)
            Ancho = {Ssg(1.5), Ssg(10), Ssg(8)}
            tabla.SetWidths(Ancho)
            tabla.WidthPercentage = 100

            'tabla.AddCell(NewPdfCell("", 2, True, False, 1, 3))


            document.Add(NewRenglonEnter())

            tabla = New PdfPTable(1)
            Ancho = {Ssg(21)}
            tabla.SetWidths(Ancho)
            tabla.WidthPercentage = 100
            tabla.AddCell(NewPdfCellGray("1. INFORMACIÓN DE RECEPCIÓN", 1, True, True))
            document.Add(tabla)

            document.Add(NewRenglonEnter())


            'tabla = New PdfPTable(3)
            'Ancho = {Ssg(1.5), Ssg(10), Ssg(8)}
            'tabla.SetWidths(Ancho)
            'tabla.WidthPercentage = 100
            'If Qi.GetDataByIDIngreso(IDIngreso).Rows(0).Item("Carrier").ToString = 1 Then
            '    tabla.AddCell(NewPdfCell("CAC", 3, True, False))
            '    tabla.AddCell(NewPdfCell(Qi.GetDataByIDIngreso(IDIngreso).Rows(0).Item("CAC").ToString, 3, False, True))
            '    tabla.AddCell(NewPdfCell("", 2, False, False))
            'End If
            'If Qi.GetDataByIDIngreso(IDIngreso).Rows(0).Item("Carrier").ToString = 2 Then
            '    tabla.AddCell(NewPdfCell("", 2, False, False))
            '    tabla.AddCell(NewPdfCell("BRIGTHSTAR", 2, True, True))
            '    tabla.AddCell(NewPdfCell("", 2, False, False))
            'End If

            'document.Add(tabla)
            'document.Add(NewRenglonEnter())


            tabla = New PdfPTable(4)
            Ancho = {Ssg(6), Ssg(6), Ssg(1), Ssg(6)}
            tabla.SetWidths(Ancho)
            tabla.WidthPercentage = 100

            'Dim oiris As New DatosSistema.CodigosIrisClass

            Dim PDFC As New PdfPCell
            PDFC = NewPdfCell("INSPECCIÓN FUNCIONAL", 1, True, True)
            PDFC.Colspan = 2
            tabla.AddCell(PDFC)
            tabla.AddCell(NewPdfCell("", 2, True, False))
            PDFC = NewPdfCell("INSPECCIÓN COSMÉTICA", 1, True, True)
            PDFC.Rowspan = 2
            tabla.AddCell(PDFC)
            tabla.AddCell(NewPdfCell("SINTOMA QUE DECLARA USUARIO", 1, True, True))
            tabla.AddCell(NewPdfCell("OTRAS AVERÍAS (T/D)", 1, True, True))
            tabla.AddCell(NewPdfCell("", 2, True, False))
            tabla.AddCell(NewPdfCell("Cliente", 2, True, False))
            tabla.AddCell(NewPdfCell("Cliente", 2, True, False))
            tabla.AddCell(NewPdfCell("", 2, True, False))
            tabla.AddCell(NewPdfCell("Cliente", 2, True, False))

            document.Add(tabla)
            document.Add(NewRenglonEnter())

            tabla = New PdfPTable(1)
            Ancho = {Ssg(21)}
            tabla.SetWidths(Ancho)
            tabla.WidthPercentage = 100
            tabla.AddCell(NewPdfCellGray("2. INFORME DE DIAGNÓSTICO", 1, True, True))
            document.Add(tabla)

            document.Add(NewRenglonEnter())

            tabla = New PdfPTable(1)
            Ancho = {Ssg(21)}
            tabla.SetWidths(Ancho)
            tabla.WidthPercentage = 100
            tabla.AddCell(NewPdfCell("DETALLE DE LA REPARACIÓN", 3, True, False))
            Dim diagnosticoreparacion As String = ""

            PDFC = New PdfPCell
            PDFC = NewPdfCell("DETALLE REPARACIÓN REALIZADA", 3, True, False)
            tabla.AddCell(PDFC)

            Dim temp2 As String = ""

            PDFC = NewPdfCell(1, 3, False, True)
            tabla.AddCell(PDFC)
            document.Add(tabla)

            tabla = New PdfPTable(5)
            Ancho = {Ssg(4.5), Ssg(4.5), Ssg(1), Ssg(4.5), Ssg(4.5)}
            tabla.SetWidths(Ancho)
            tabla.WidthPercentage = 100


            PDFC = NewPdfCell("DIAGNÓSTICO REPARACIÓN", 1, True, False)
            tabla.AddCell(PDFC)
            tabla.AddCell(NewPdfCell(diagnosticoreparacion.Replace("|", Chr(10)), 1, False, True))
            tabla.AddCell(NewPdfCell("", 1, True, False))
            tabla.AddCell(NewPdfCell("NIVEL DE REPARACIÓN", 1, True, False))
            tabla.AddCell(NewPdfCell(1, 1, False, True))
            document.Add(tabla)

            tabla = New PdfPTable(8)
            Ancho = {Ssg(3.25), Ssg(3.25), Ssg(0.25), Ssg(3.25), Ssg(3.25), Ssg(0.25), Ssg(3.25), Ssg(3.25)}
            tabla.SetWidths(Ancho)
            tabla.WidthPercentage = 100


            tabla.AddCell(NewPdfCell("GARANTÍA", 1, True, True))
            tabla.AddCell(NewPdfCell(1, 1, True, True))
            tabla.AddCell(NewPdfCell("", 2, True, False))

            tabla.AddCell(NewPdfCell("", 1, True, True))
            tabla.AddCell(NewPdfCell("", 1, True, True))
            tabla.AddCell(NewPdfCell("", 2, True, False))


            tabla.AddCell(NewPdfCell("COBRO CLIENTE", 1, True, True))
            Try
                tabla.AddCell(NewPdfCell(1, 1, True, True))
            Catch ex As Exception
                tabla.AddCell(NewPdfCell(0, 1, True, True))
            End Try


            document.Add(tabla)
            document.Add(NewRenglonEnter())

            tabla = New PdfPTable(1)
            Ancho = {Ssg(21)}
            tabla.SetWidths(Ancho)
            tabla.WidthPercentage = 100
            tabla.AddCell(NewPdfCellGray("3. RECOMENDACIONES", 1, True, True))
            document.Add(tabla)

            document.Add(NewRenglonEnter())

            tabla = New PdfPTable(1)
            Ancho = {Ssg(21)}
            tabla.SetWidths(Ancho)
            tabla.WidthPercentage = 100

            Dim recomendacion As String = ""
            recomendacion = ""
            tabla.AddCell(NewPdfCell(recomendacion.Replace("|", Chr(10)), 3, False, True))

            document.Add(tabla)
            tabla = New PdfPTable(2)
            Ancho = {Ssg(2), Ssg(19)}
            tabla.SetWidths(Ancho)
            tabla.WidthPercentage = 100

            Dim accesorios As String = ""

            document.Add(NewRenglonEnter())
            tabla.AddCell(NewPdfCell("ACCESORIOS", 3, True, False))
            tabla.AddCell(NewPdfCell(accesorios, 3, False, True))
            document.Add(tabla)

            document.Add(NewRenglonEnter)
            Parag = New Paragraph("COOPER GLOBAL S.A.S. promueve la satisfacción al cliente, garantizando la calidad de su servicio.", font)
            Parag.Alignment = 1
            document.Add(Parag)
            document.Add(NewRenglonEnter())
            document.Add(NewRenglonEnter())

            Parag = New Paragraph("Recibido a conformidad", font)
            Parag.Alignment = 3
            document.Add(Parag)
            document.Add(NewRenglonEnter())
            document.Add(NewRenglonEnter())
            document.Add(NewRenglonEnter())
            document.Add(NewRenglonEnter())
            tabla = New PdfPTable(5)
            Ancho = {Ssg(6), Ssg(1.5), Ssg(6), Ssg(1.5), Ssg(6)}
            tabla.SetWidths(Ancho)
            tabla.WidthPercentage = 100

            tabla.AddCell(NewPdfCell("", 2, True, True))
            tabla.AddCell(NewPdfCell("", 2, True, False))
            tabla.AddCell(NewPdfCell("", 2, True, True))
            tabla.AddCell(NewPdfCell("", 2, True, False))
            tabla.AddCell(NewPdfCell("", 2, True, True))

            tabla.AddCell(NewPdfCell("NOMBRE", 1, True, False))
            tabla.AddCell(NewPdfCell("", 1, True, False))
            tabla.AddCell(NewPdfCell("C.C.", 1, True, False))
            tabla.AddCell(NewPdfCell("", 1, True, False))
            tabla.AddCell(NewPdfCell("FECHA", 1, True, False))

            document.Add(tabla)

        Catch ex As Exception
            document.CloseDocument()
            document.Close()
            Exit Sub
        Finally
            document.CloseDocument()
            document.Close()
        End Try

    End Sub

    Sub DescargarPDF(ByVal namefile As String)
        Dim urlFija As String
        urlFija = "\Files"
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
    End Sub


    Public Shared Function FNumero(ByVal Valor As Double) As String
        FNumero = FormatNumber(Valor, 0, , , TriState.UseDefault)
    End Function

    Sub CellBorders(ByRef Celda As PdfPCell, ByVal _Top As Single, ByVal _Rigth As Single, ByVal _Bottom As Single, ByVal _Left As Single)
        Celda.BorderWidthBottom = _Bottom - 0.5
        Celda.BorderWidthLeft = _Left - 0.5
        Celda.BorderWidthRight = _Rigth - 0.5
        Celda.BorderWidthTop = _Top - 0.5
        Dim color As New iTextSharp.text.BaseColor(0, 0, 0) '(100, 100, 100, 100) '
        Celda.BorderColor = color
    End Sub

    Public Shared Function Ssg(ByVal Unidad As Double) As Single
        Ssg = Math.Round((652 * Unidad) / 21.59, 0)
    End Function

    Function NewPdfCell(ByVal paragr As String, ByVal alineacion As Integer, Optional ByVal bold As Boolean = False, Optional ByVal bordes As Boolean = False, Optional ByVal ColSpan As Integer = 1) As PdfPCell
        Dim Font1 As iTextSharp.text.Font = FontFactory.GetFont("Century", 7, Font.BOLD)
        Dim Font2 As iTextSharp.text.Font = FontFactory.GetFont("Century", 7, Font.NORMAL)
        Dim PdfCell = New PdfPCell
        If bordes = True Then
            CellBorders(PdfCell, 1, 1, 1, 1)
        Else
            CellBorders(PdfCell, 0, 0, 0, 0)
        End If

        PdfCell.HorizontalAlignment = alineacion
        Dim f1 = New Font(Font1)
        Dim f2 = New Font(Font2)
        Dim Parag = New Paragraph(paragr, IIf(bold = True, f1, f2))
        Parag.Leading = 8
        Parag.Alignment = alineacion
        PdfCell.AddElement(Parag)
        PdfCell.PaddingBottom = 1.5
        PdfCell.Colspan = ColSpan
        NewPdfCell = PdfCell
    End Function

    Function NewPdfCellCh(ByVal paragr As String, ByVal alineacion As Integer, Optional ByVal bold As Boolean = False, Optional ByVal bordes As Boolean = False) As PdfPCell
        Dim Font1 As iTextSharp.text.Font = FontFactory.GetFont("Century", 6, Font.BOLD)
        Dim Font2 As iTextSharp.text.Font = FontFactory.GetFont("Century", 6.5, Font.NORMAL)
        Dim PdfCell = New PdfPCell
        If bordes = True Then
            CellBorders(PdfCell, 1, 1, 1, 1)
        Else
            CellBorders(PdfCell, 0, 0, 0, 0)
        End If

        PdfCell.HorizontalAlignment = alineacion
        Dim f1 = New Font(Font1)
        Dim f2 = New Font(Font2)
        Dim Parag = New Paragraph(paragr, IIf(bold = True, f1, f2))
        Parag.Leading = 6.5
        Parag.Alignment = alineacion
        PdfCell.AddElement(Parag)
        PdfCell.PaddingBottom = 1.5
        NewPdfCellCh = PdfCell
    End Function

    Function NewPdfCellGray(ByVal paragr As String, ByVal alineacion As Integer, Optional ByVal bold As Boolean = False, Optional ByVal bordes As Boolean = False) As PdfPCell
        Dim color As New iTextSharp.text.BaseColor(System.Drawing.Color.Navy)
        Dim Font1 As iTextSharp.text.Font = FontFactory.GetFont("Century", 9, Font.BOLD, color)
        Dim Font2 As iTextSharp.text.Font = FontFactory.GetFont("Century", 9, Font.NORMAL, color)
        Dim PdfCell = New PdfPCell
        If bordes = True Then
            CellBorders(PdfCell, 1, 1, 1, 1)
        Else
            CellBorders(PdfCell, 0, 0, 0, 0)
        End If

        PdfCell.HorizontalAlignment = alineacion
        Dim colorb As New iTextSharp.text.BaseColor(System.Drawing.Color.LightGray)
        PdfCell.BackgroundColor = colorb
        Dim f1 = New Font(Font1)
        Dim f2 = New Font(Font2)
        Dim Parag = New Paragraph(paragr, IIf(bold = True, f1, f2))
        Parag.Leading = 9.5
        Parag.Alignment = alineacion
        PdfCell.AddElement(Parag)
        NewPdfCellGray = PdfCell
    End Function

    Function NewPdfTitle(ByVal titulo As String) As PdfPTable
        Dim font As New Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)
        Dim CalFont1 As iTextSharp.text.Font = FontFactory.GetFont("Century", 8, Font.BOLD)
        Dim tabla = New PdfPTable(1)
        tabla.WidthPercentage = 100
        Dim PdfCell = New PdfPCell
        CellBorders(PdfCell, 1.5, 1.5, 1.5, 1.5)
        PdfCell.BorderColor = iTextSharp.text.BaseColor.BLACK
        font = New Font(CalFont1)
        Dim Parag = New Paragraph(titulo, font)
        Parag.Leading = 9
        Parag.Alignment = 2
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)
        NewPdfTitle = tabla
    End Function

    Function NewRenglonEnter() As PdfPTable
        Dim tabla = New PdfPTable(1)
        tabla.WidthPercentage = 100
        tabla.AddCell(NewPdfCell("", 2, False, False))
        NewRenglonEnter = tabla
    End Function

    Function EnterMin() As PdfPTable
        Dim tabla = New PdfPTable(1)
        tabla.WidthPercentage = 100
        Dim PdfCell = New PdfPCell
        Dim Parag = New Paragraph("")
        Parag.Leading = 1
        Parag.Alignment = 2
        PdfCell.AddElement(Parag)
        CellBorders(PdfCell, 0, 0, 0, 0)
        tabla.AddCell(PdfCell)
        EnterMin = tabla
    End Function

    Sub AddTable(ByRef tabla As PdfPTable, ByRef document As Document)
        document.Add(tabla)
        document.Add(EnterMin)
    End Sub

    Protected Sub btnLoadFile_Click(sender As Object, e As EventArgs) Handles btnLoadFile.Click
        Dim nombreFactura As String = ""
        Dim o As New FI.Facturas
        Dim ent = o.ObtenerFactura(hfIdFactura.Value)

        If fileup.HasFile Then
            'La carpeta donde voy a almacenar el archivo
            Dim path As String = Server.MapPath("~/Facturas/")
            Dim fileload As New System.IO.FileInfo(fileup.FileName)
            nombreFactura = ent.id & ".pdf"
            'Verifica que las extensiones sean las permitidas, dependiendo de la extensión llama la función
            If fileload.Extension = ".pdf" Then
                fileup.SaveAs(path & nombreFactura)
                ShowNotification("El archivo PDF se ha cargado correctamente.", ShowNotifications.InfoNotification)

            Else
                AlertJS("Por favor verifique que el archivo que intenta cargar corresponda al formato .pdf")
                fileup.Focus()
                Exit Sub
            End If
        Else
            AlertJS("Debe cargar un archivo en formato pdf")
            fileup.Focus()
            Exit Sub
        End If
    End Sub

    Protected Sub btnViewFile_Click(sender As Object, e As EventArgs) Handles btnViewFile.Click
        Dim nombreFactura As String = ""
        Dim o As New FI.Facturas
        Dim ent = o.ObtenerFactura(hfIdFactura.Value)
        Dim path As String = Server.MapPath("~/Facturas/")
        nombreFactura = ent.id & ".pdf"

        Dim urlFija As String
        urlFija = "~/Facturas/"
        urlFija = Server.MapPath(urlFija & "\")

        If System.IO.File.Exists(urlFija & "\" & nombreFactura) Then
            ShowWindows("../Facturas/" & nombreFactura)
            'Response.Redirect("..\Facturas\" & nombreFactura, False)
        Else
            AlertJS("No se ha guardado una Factura escaneada para este item!")
            fileup.Focus()
            Exit Sub
        End If

    End Sub


#End Region


End Class
