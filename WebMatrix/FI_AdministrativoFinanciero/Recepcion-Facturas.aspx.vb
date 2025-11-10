Imports CoreProject
Imports WebMatrix.Util
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO

Public Class FI_Recepcion_Facturas
    Inherits System.Web.UI.Page

    Enum eTipoOrden
        OrdendeServicio = 1
        OrdendeCompra = 2
        OrdenRequerimiento = 3
    End Enum

    Enum eEstadosFactura
        Recibida = 0
        EnAprobacion = 1
        Aprobada = 2
        Contabilidad = 3
        Tesoreria = 4
        Pagada = 5
        Anulada = 6
        Rechazada = 7
    End Enum
    Enum eEstadosOrden
        Creada = 1
        EnAprobacion = 2
        Aprobada = 3
        Enviada = 4
        Facturada = 5
        Rechazada = 6
        Anulada = 7
    End Enum

    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub
    Public Sub ActivateTab(ByVal tabIndex As Integer)
        Dim mePage As Page = CType(HttpContext.Current.Handler, Page)
        ScriptManager.RegisterStartupScript(mePage, upTarea.GetType(), "tabActivate", "$('#tabs').tabs();$('#tabs').tabs('option', 'selected'," & tabIndex.ToString & ");", True)
    End Sub

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Page.Form.Attributes.Add("enctype", "multipart/form-data")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarFacturasRadicadas()
            Session("OrdenesReqNew") = Nothing
            Session("OrdenesNew") = Nothing
        End If

        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnLoadFile)
        smanager.RegisterPostBackControl(Me.btnViewFile)
    End Sub

    Private Sub gvDetalle_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDetalle.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            If CType(e.Row.DataItem, FI_FacturasRadicadasNew_Get_Result).Estado = eEstadosFactura.Recibida Then
                DirectCast(e.Row.FindControl("imgBtnActualizar"), ImageButton).Visible = True
                DirectCast(e.Row.FindControl("imgBtnBorrar"), ImageButton).Visible = True
            Else
                DirectCast(e.Row.FindControl("imgBtnActualizar"), ImageButton).Visible = False
                DirectCast(e.Row.FindControl("imgBtnBorrar"), ImageButton).Visible = False
            End If

            If CType(e.Row.DataItem, FI_FacturasRadicadasNew_Get_Result).Estado = 6 Then
                DirectCast(e.Row.FindControl("imgBtnAnular"), ImageButton).Visible = False
            Else
                DirectCast(e.Row.FindControl("imgBtnAnular"), ImageButton).Visible = True
            End If
        End If

    End Sub

    Sub Limpiar()
        txtConsecutivo.Text = ""
        txtNoFactura.Text = ""
        txtCantidad.Text = ""
        txtValorUnitario.Text = ""
        txtSubTotal.Text = ""
        txtValorTotal.Text = ""
        txtObservaciones.Text = ""
        ddlTipoDocRadicado.ClearSelection()
        hfIdFactura.Value = "0"
        hfIdOrden.Value = "0"
        hfTipoOrden.Value = "0"
        hfTipoOrdenRad.Value = "0"
        hfFacturaEscaneda.Value = "0"
        hfUsuarioAnula.Value = "0"
        txtAnular.Text = ""
        txtUsAnula.Text = ""
        pnlAnulacion.Visible = False
        btnAdd.Visible = False
        btnLimpiar.Visible = False
        Session("OrdenesNew") = Nothing
        Session("OrdenesReqNew") = Nothing
        gvOrdenes.DataBind()
        gvOrdenesReq.DataBind()
        Me.lblTextTotal.Visible = False
        Me.lblTotal.Visible = False
        Me.lblTotal.Text = ""
        LimpiarOrdenes()
    End Sub

    Sub LimpiarOrdenes()
        txtNoOrden.Text = ""
        ddlTipoOrden.ClearSelection()
        btnBuscar.Visible = True
        btnAgregar.Visible = False
        gvOrdenes.Columns(3).Visible = True
    End Sub

    Sub LimpiarBusqueda()
        txtSearchConsecutivo.Text = ""
        ddlSearchTipoDoc.ClearSelection()
        ddlSearchEscaneada.ClearSelection()
        txtFacturaFechaInicio.Text = ""
        txtFacturaFechaFin.Text = ""
        gvOrdenes.DataBind()
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
            hfUsuarioAnula.Value = Me.gvSolicitantes.DataKeys(CInt(e.CommandArgument))("id")
            Me.txtUsAnula.Text = Server.HtmlDecode(Me.gvSolicitantes.Rows(CInt(e.CommandArgument)).Cells(0).Text.ToString) & " " & Server.HtmlDecode(Me.gvSolicitantes.Rows(CInt(e.CommandArgument)).Cells(1).Text.ToString)
        End If
    End Sub

    Sub ValidarFactura()
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

        If hfIdFactura.Value = "0" Then
            If daFacturas.obtenerFacturaRadicada(Nothing, If(Int64.TryParse(txtConsecutivo.Text, New Integer), txtConsecutivo.Text, Nothing), Date.Now.Year).Count > 0 Then
                AlertJS("El número de radicado ya se ingreso")
                txtConsecutivo.Focus()
                Exit Sub
            End If
        End If

        If gvOrdenes.Rows.Count = 0 And gvOrdenesReq.Rows.Count = 0 Then
            AlertJS("Debe agregar al menos una Orden para Radicar la Factura")
            gvOrdenes.Focus()
            Exit Sub
        End If

        GuardarFactura()
    End Sub

    Sub GuardarFactura()
        Dim o As New FI.Facturas
        Dim e As New FI_FacturasRadicadas
        Dim oOrdenes As New FI.Ordenes

        If Not hfIdFactura.Value = 0 Then
            e = o.ObtenerFactura(hfIdFactura.Value)
        End If

        If IsNumeric(txtCantidad.Text) Then e.Cantidad = txtCantidad.Text
        e.IdOrden = hfIdOrden.Value
        e.Tipo = hfTipoOrden.Value
        e.Consecutivo = txtConsecutivo.Text
        e.Fecha = Date.UtcNow.AddHours(-5)
        e.IdOrden = Nothing
        e.Observaciones = txtObservaciones.Text
        e.NoFactura = txtNoFactura.Text
        If IsNumeric(txtValorUnitario.Text) Then e.VrUnitario = txtValorUnitario.Text
        If IsNumeric(txtValorTotal.Text) Then e.ValorTotal = txtValorTotal.Text
        If IsNumeric(txtSubTotal.Text) Then e.Subtotal = txtSubTotal.Text
        e.Usuario = Session("IDUsuario").ToString
        e.TipoDocumento = ddlTipoDocRadicado.SelectedValue
        e.Tipo = Nothing
        e.Estado = 0
        If hfIdFactura.Value = "0" Then e.Escaneada = False
        o.GuardarFactura(e)

        If hfIdFactura.Value = "0" Then
            Dim e2 As New FI_LogAprobacionFacturas
            e2.Estado = 0
            e2.Fecha = Date.UtcNow.AddHours(-5)
            e2.IdFactura = e.id
            e2.Usuario = Session("IDUsuario").ToString
            o.GuardarLogFactura(e2)

            hfIdFactura.Value = e.id

            If hfTipoOrdenRad.Value = 3 Then
                GuardarDetalleRequerimientoRadicado()
            End If

            AlmacenarOrdenesGridview()

            Dim entNewOrdenes As New List(Of FI_FacturasRadicadas_Detalle_Get_Result)
            entNewOrdenes = Session("OrdenesNew")
            For i As Integer = 0 To entNewOrdenes.Count - 1
                Dim daFactura As New FI.Facturas
                Dim EntOrdenes As New FI_FacturasRadicadas_Detalle
                EntOrdenes.IdFactura = hfIdFactura.Value
                EntOrdenes.IdOrden = entNewOrdenes.Item(i).IdOrden
                EntOrdenes.TipoOrden = entNewOrdenes.Item(i).TipoOrdenId
                daFactura.GuardarFacturaDetalle(EntOrdenes)

                If e.IdOrden Is Nothing Or hfTipoOrden.Value Is Nothing Then
                    e.IdOrden = entNewOrdenes.Item(i).IdOrden
                    e.Tipo = entNewOrdenes.Item(i).TipoOrdenId
                    o.GuardarFactura(e)
                End If

                Dim oFacturaDetalle = o.ObtenerFacturasRadicadasDetalle(hfIdFactura.Value, Nothing, Nothing, Nothing)

                If entNewOrdenes.Item(i).TipoOrdenId = eTipoOrden.OrdendeServicio Then
                    Dim oServicio = oOrdenes.ObtenerOrdenServicio(entNewOrdenes.Item(i).IdOrden)

                    If oFacturaDetalle.Count > 1 Then
                        If oFacturaDetalle.Where(Function(x) x.SolicitanteId = oServicio.SolicitadoPor).Count = 1 Then
                            daFactura.GrabarUsuariosApruebanFactura(hfIdFactura.Value, oServicio.SolicitadoPor)
                        End If

                        If oServicio.EvaluaProveedor IsNot Nothing Then
                            If oFacturaDetalle.Where(Function(x) x.EvaluaProveedor = oServicio.EvaluaProveedor).Count = 1 Then
                                daFactura.GrabarUsuariosEvaluanProveedorFactura(hfIdFactura.Value, oServicio.EvaluaProveedor)
                            End If
                        Else
                            If oFacturaDetalle.Where(Function(x) x.SolicitanteId = oServicio.SolicitadoPor).Count = 1 Then
                                daFactura.GrabarUsuariosEvaluanProveedorFactura(hfIdFactura.Value, oServicio.SolicitadoPor)
                            End If
                        End If

                    Else
                        daFactura.GrabarUsuariosApruebanFactura(hfIdFactura.Value, oServicio.SolicitadoPor)

                        If oServicio.EvaluaProveedor IsNot Nothing Then
                            daFactura.GrabarUsuariosEvaluanProveedorFactura(hfIdFactura.Value, oServicio.EvaluaProveedor)
                        Else
                            daFactura.GrabarUsuariosEvaluanProveedorFactura(hfIdFactura.Value, oServicio.SolicitadoPor)
                        End If
                    End If

                ElseIf entNewOrdenes.Item(i).TipoOrdenId = eTipoOrden.OrdendeCompra Then
                    Dim oCompra = oOrdenes.ObtenerOrdenCompra(entNewOrdenes.Item(i).IdOrden)
                    If oFacturaDetalle.Count > 1 Then
                        If oFacturaDetalle.Where(Function(x) x.SolicitanteId = oCompra.SolicitadoPor).Count = 1 Then
                            daFactura.GrabarUsuariosApruebanFactura(hfIdFactura.Value, oCompra.SolicitadoPor)
                        End If

                        If oCompra.EvaluaProveedor IsNot Nothing Then
                            If oFacturaDetalle.Where(Function(x) x.EvaluaProveedor = oCompra.EvaluaProveedor).Count = 1 Then
                                daFactura.GrabarUsuariosEvaluanProveedorFactura(hfIdFactura.Value, oCompra.EvaluaProveedor)
                            End If
                        Else
                            If oFacturaDetalle.Where(Function(x) x.SolicitanteId = oCompra.SolicitadoPor).Count = 1 Then
                                daFactura.GrabarUsuariosEvaluanProveedorFactura(hfIdFactura.Value, oCompra.SolicitadoPor)
                            End If
                        End If

                    Else
                        daFactura.GrabarUsuariosApruebanFactura(hfIdFactura.Value, oCompra.SolicitadoPor)

                        If oCompra.EvaluaProveedor IsNot Nothing Then
                            daFactura.GrabarUsuariosEvaluanProveedorFactura(hfIdFactura.Value, oCompra.EvaluaProveedor)
                        Else
                            daFactura.GrabarUsuariosEvaluanProveedorFactura(hfIdFactura.Value, oCompra.SolicitadoPor)
                        End If
                    End If

                ElseIf entNewOrdenes.Item(i).TipoOrdenId = eTipoOrden.OrdenRequerimiento Then
                    Dim oRequerimiento = oOrdenes.ObtenerOrdenRequerimiento(entNewOrdenes.Item(i).IdOrden)
                    If (oRequerimiento.AprobadoAdmin IsNot Nothing) Then
                        daFactura.GrabarUsuariosApruebanFactura(hfIdFactura.Value, oRequerimiento.AprobadoAdmin)
                    End If

                    If (oRequerimiento.AprobadoGerente IsNot Nothing) Then
                        daFactura.GrabarUsuariosApruebanFactura(hfIdFactura.Value, oRequerimiento.AprobadoGerente)
                    End If

                    If (oRequerimiento.AprobadoCOE IsNot Nothing) Then
                        daFactura.GrabarUsuariosApruebanFactura(hfIdFactura.Value, oRequerimiento.AprobadoCOE)
                    End If

                    If oRequerimiento.EvaluaProveedor IsNot Nothing Then
                                daFactura.GrabarUsuariosEvaluanProveedorFactura(hfIdFactura.Value, oRequerimiento.EvaluaProveedor)
                            Else
                                daFactura.GrabarUsuariosEvaluanProveedorFactura(hfIdFactura.Value, oRequerimiento.ElaboradoPor)
                            End If
                        End If
            Next

        End If

        Limpiar()
        CargarFacturasRadicadas()
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        CargarBusquedaFacturasRadicadas()
        ActivateTab(1)
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ValidarFactura()
        CargarFacturasRadicadas()
    End Sub

    Protected Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        Limpiar()
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        LimpiarBusqueda()
        CargarFacturasRadicadas()
        ActivateTab(1)
    End Sub

    Sub CargarFacturasRadicadas()
        Dim o As New FI.Facturas
        Me.gvDetalle.DataSource = o.ObtenerFacturasRadicadasNew(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        Me.gvDetalle.DataBind()
    End Sub

    Sub CargarBusquedaFacturasRadicadas()
        Dim o As New FI.Facturas
        Dim Consecutivo As Int64? = Nothing
        If Not txtSearchConsecutivo.Text = "" Then Consecutivo = txtSearchConsecutivo.Text

        Dim TipoDocRadicado As Int16? = Nothing
        If Not ddlSearchTipoDoc.SelectedValue = "-1" Then TipoDocRadicado = ddlSearchTipoDoc.SelectedValue

        Dim Escaneada As Boolean? = Nothing
        If ddlSearchEscaneada.SelectedValue = "1" Then Escaneada = True
        If ddlSearchEscaneada.SelectedValue = "0" Then Escaneada = False

        Dim FacturaFechaInicio As Date? = If(String.IsNullOrEmpty(txtFacturaFechaInicio.Text), CType(Nothing, Date?), CType(txtFacturaFechaInicio.Text, Date?))
        Dim FacturaFechaFin As Date? = If(String.IsNullOrEmpty(txtFacturaFechaFin.Text), CType(Nothing, Date?), CType(txtFacturaFechaFin.Text, Date?))

        Me.gvDetalle.DataSource = o.ObtenerFacturasRadicadasNew(Nothing, Consecutivo, TipoDocRadicado, Escaneada, FacturaFechaInicio, FacturaFechaFin)
        Me.gvDetalle.DataBind()

        ActivateTab(1)
        txtNoOrden.Text = ""
        ddlTipoOrden.ClearSelection()
        btnBuscar.Visible = True
        btnAgregar.Visible = False
        gvOrdenes.DataBind()
    End Sub

    Sub CargarGridOrdenes()
        Me.gvOrdenes.DataSource = Session("OrdenesNew")
        Me.gvOrdenes.DataBind()
    End Sub

    Sub CargarOrdenesxFactura()
        Dim o As New FI.Facturas
        Dim entNewOrdenes As New List(Of FI_FacturasRadicadas_Detalle_Get_Result)
        Me.gvOrdenes.DataSource = o.ObtenerFacturasRadicadasDetalle(hfIdFactura.Value, Nothing, Nothing, Nothing)
        Me.gvOrdenes.DataBind()
        gvOrdenes.Columns(3).Visible = False
        btnBuscar.Visible = False
        btnAgregar.Visible = False

        entNewOrdenes = o.ObtenerFacturasRadicadasDetalle(hfIdFactura.Value, Nothing, Nothing, Nothing)
        Dim valorTotal As Int64 = 0
        For i As Integer = 0 To entNewOrdenes.Count - 1
            valorTotal = valorTotal + entNewOrdenes.Item(i).ValorOrden
        Next

        Me.lblTextTotal.Visible = True
        Me.lblTotal.Visible = True
        Me.lblTotal.Text = FormatCurrency(valorTotal, 0)
    End Sub

    Sub GuardarDetalleRequerimientoRadicado()
        Dim o As New FI.Facturas
        For Each row As GridViewRow In gvOrdenesReq.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim fd = New FI_FacturasRadicadas_DetalleRequerimiento
                fd.IdFactura = hfIdFactura.Value
                fd.IdDetalleRequerimiento = gvOrdenesReq.Rows(row.RowIndex).Cells(0).Text
                fd.Cantidad = DirectCast(gvOrdenesReq.Rows(row.RowIndex).FindControl("txtCantidad"), TextBox).Text.Trim
                fd.VrUnitario = DirectCast(gvOrdenesReq.Rows(row.RowIndex).FindControl("txtVrUnitario"), TextBox).Text.Trim
                o.GrabarFacturaDetalleRequerimiento(fd)
            End If
        Next
    End Sub

    Sub AlmacenarOrdenesGridview()
        Dim entNewOrdenes As New List(Of FI_FacturasRadicadas_Detalle_Get_Result)
        If hfTipoOrdenRad.Value = 1 Or hfTipoOrdenRad.Value = 2 Then
            For Each row As GridViewRow In gvOrdenes.Rows
                entNewOrdenes.Add(New FI_FacturasRadicadas_Detalle_Get_Result With {.IdOrden = gvOrdenes.Rows(row.RowIndex).Cells(0).Text, .TipoOrdenId = gvOrdenes.DataKeys(row.RowIndex).Item("TipoOrdenId"), .TipoOrden = gvOrdenes.Rows(row.RowIndex).Cells(1).Text})
            Next
        ElseIf hfTipoOrdenRad.Value = 3 Then
            Dim IdOrden = 0
            For Each row As GridViewRow In gvOrdenesReq.Rows
                If IdOrden <> gvOrdenesReq.Rows(row.RowIndex).Cells(1).Text Then
                    IdOrden = gvOrdenesReq.Rows(row.RowIndex).Cells(1).Text
                    entNewOrdenes.Add(New FI_FacturasRadicadas_Detalle_Get_Result With {.IdOrden = IdOrden, .TipoOrdenId = "3", .TipoOrden = "Requerimiento de Servicio"})
                End If
            Next
        End If
        Session("OrdenesNew") = entNewOrdenes

    End Sub

    Sub ValidarOrdenesxProveedor()
        Dim entNewOrdenes As New List(Of FI_FacturasRadicadas_Detalle_Get_Result)
        If Not IsNothing(Session("OrdenesNew")) Then entNewOrdenes = Session("OrdenesNew")
        Dim Ordenes As New FI.Ordenes
        Dim daFacturas As New FI.Facturas
        Dim oServicioN As FI_OrdenServicio = Nothing
        Dim oCompraN As FI_OrdenCompra = Nothing
        Dim oRequerimientoN As FI_OrdenRequerimiento = Nothing

        If ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeServicio Then oServicioN = Ordenes.ObtenerOrdenServicio(txtNoOrden.Text)
        If ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeCompra Then oCompraN = Ordenes.ObtenerOrdenCompra(txtNoOrden.Text)
        If ddlTipoOrden.SelectedValue = eTipoOrden.OrdenRequerimiento Then oRequerimientoN = Ordenes.ObtenerOrdenRequerimiento(txtNoOrden.Text)

        If ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeServicio Then
            If (oServicioN.Estado = eEstadosOrden.Rechazada Or oServicioN.Estado = eEstadosOrden.Anulada) Then
                AlertJS("No se pueden agregar Órdenes de Servicio Anuladas o Rechazadas")
                btnAgregar.Visible = False
                txtNoOrden.Focus()
                LimpiarOrdenes()
                Exit Sub
            End If
        ElseIf ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeCompra Then
            If (oCompraN.Estado = eEstadosOrden.Rechazada Or oCompraN.Estado = eEstadosOrden.Anulada) Then
                AlertJS("No se pueden agregar Órdenes de Compra Anuladas o Rechazadas")
                btnAgregar.Visible = False
                txtNoOrden.Focus()
                LimpiarOrdenes()
                Exit Sub
            End If
        ElseIf ddlTipoOrden.SelectedValue = eTipoOrden.OrdenRequerimiento Then
            If (oRequerimientoN.Estado = eEstadosOrden.Rechazada Or oRequerimientoN.Estado = eEstadosOrden.Anulada) Then
                AlertJS("No se pueden agregar Órdenes de Compra Anuladas o Rechazadas")
                btnAgregar.Visible = False
                txtNoOrden.Focus()
                LimpiarOrdenes()
                Exit Sub
            End If
        End If

        If gvOrdenes.Rows.Count >= 1 Then
            Dim oServicioA As FI_OrdenServicio = Nothing
            Dim oCompraA As FI_OrdenCompra = Nothing

            If gvOrdenes.DataKeys(0).Item("TipoOrdenId") = eTipoOrden.OrdendeServicio Then oServicioA = Ordenes.ObtenerOrdenServicio(gvOrdenes.Rows(0).Cells(0).Text)
            If gvOrdenes.DataKeys(0).Item("TipoOrdenId") = eTipoOrden.OrdendeCompra Then oCompraA = Ordenes.ObtenerOrdenCompra(gvOrdenes.Rows(0).Cells(0).Text)

            If gvOrdenes.DataKeys(0).Item("TipoOrdenId") = eTipoOrden.OrdendeServicio And ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeServicio Then
                If (oServicioN.ProveedorId <> oServicioA.ProveedorId) Then
                    AlertJS("Sólo puede agregar Órdenes de un mismo Proveedor")
                    btnAgregar.Visible = False
                    txtNoOrden.Focus()
                    LimpiarOrdenes()
                    Exit Sub
                End If
            ElseIf gvOrdenes.DataKeys(0).Item("TipoOrdenId") = eTipoOrden.OrdendeServicio And ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeCompra Then
                If (oCompraN.ProveedorId <> oServicioA.ProveedorId) Then
                    AlertJS("Sólo puede agregar Órdenes de un mismo Proveedor")
                    btnAgregar.Visible = False
                    txtNoOrden.Focus()
                    LimpiarOrdenes()
                    Exit Sub
                End If
            ElseIf gvOrdenes.DataKeys(0).Item("TipoOrdenId") = eTipoOrden.OrdendeServicio And ddlTipoOrden.SelectedValue = eTipoOrden.OrdenRequerimiento Then
                AlertJS("No puede vincular una Orden de Compra o Servicio con un Requerimiento de Servicio en la misma Factura!")
                btnAgregar.Visible = False
                txtNoOrden.Focus()
                LimpiarOrdenes()
                Exit Sub
            End If

            If gvOrdenes.DataKeys(0).Item("TipoOrdenId") = eTipoOrden.OrdendeCompra And ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeServicio Then
                If (oServicioN.ProveedorId <> oCompraA.ProveedorId) Then
                    AlertJS("Sólo puede agregar Órdenes de un mismo Proveedor")
                    btnAgregar.Visible = False
                    txtNoOrden.Focus()
                    LimpiarOrdenes()
                    Exit Sub
                End If
            ElseIf gvOrdenes.DataKeys(0).Item("TipoOrdenId") = eTipoOrden.OrdendeCompra And ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeCompra Then
                If (oCompraN.ProveedorId <> oCompraA.ProveedorId) Then
                    AlertJS("Sólo puede agregar Órdenes de un mismo Proveedor")
                    btnAgregar.Visible = False
                    txtNoOrden.Focus()
                    LimpiarOrdenes()
                    Exit Sub
                End If
            ElseIf gvOrdenes.DataKeys(0).Item("TipoOrdenId") = eTipoOrden.OrdendeCompra And ddlTipoOrden.SelectedValue = eTipoOrden.OrdenRequerimiento Then
                AlertJS("No puede vincular una Orden de Compra con un Requerimiento de Servicio en la misma Factura!")
                btnAgregar.Visible = False
                txtNoOrden.Focus()
                LimpiarOrdenes()
                Exit Sub
            End If
        End If

        If gvOrdenesReq.Rows.Count >= 1 Then
            Dim oRequerimientoA As FI_OrdenRequerimiento
            oRequerimientoA = Ordenes.ObtenerOrdenRequerimiento(gvOrdenesReq.Rows(0).Cells(1).Text)
            If ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeServicio Then
                AlertJS("No puede vincular una Orden de Servicio con un Requerimiento de Servicio en la misma Factura!")
                btnAgregar.Visible = False
                txtNoOrden.Focus()
                LimpiarOrdenes()
                Exit Sub
            ElseIf ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeCompra Then
                AlertJS("No puede vincular una Orden de Compra con un Requerimiento de Servicio en la misma Factura!")
                btnAgregar.Visible = False
                txtNoOrden.Focus()
                LimpiarOrdenes()
                Exit Sub
            ElseIf ddlTipoOrden.SelectedValue = eTipoOrden.OrdenRequerimiento Then
                If (oRequerimientoN.ProveedorId <> oRequerimientoA.ProveedorId) Then
                    AlertJS("Sólo puede agregar Requerimiento de Servicio de un mismo Proveedor")
                    btnAgregar.Visible = False
                    txtNoOrden.Focus()
                    LimpiarOrdenes()
                    Exit Sub
                End If
            End If
        End If

        Dim oOrdenes = daFacturas.ObtenerOrdenesxFacturasRadicadasDetalle(txtNoOrden.Text, ddlTipoOrden.SelectedValue, False)
        If oOrdenes.Count > 0 Then
            AlertJS("Esta Orden no se puede agregar, ya se encuentra asociada a otra Factura!")
            btnAgregar.Visible = False
            txtNoOrden.Focus()
            LimpiarOrdenes()
            Exit Sub
        End If

        Dim itemOrden As New FI_FacturasRadicadas_Detalle_Get_Result
        itemOrden.IdOrden = txtNoOrden.Text
        itemOrden.TipoOrdenId = ddlTipoOrden.SelectedValue
        itemOrden.TipoOrden = ddlTipoOrden.SelectedItem.Text
        hfTipoOrden.Value = ddlTipoOrden.SelectedValue
        hfIdOrden.Value = txtNoOrden.Text
        If ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeServicio Then
            itemOrden.ValorOrden = oServicioN.Subtotal
        ElseIf ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeCompra Then
            itemOrden.ValorOrden = oCompraN.Subtotal
        ElseIf ddlTipoOrden.SelectedValue = eTipoOrden.OrdenRequerimiento Then
            itemOrden.ValorOrden = oRequerimientoN.Subtotal
        End If

        Dim valorTotal As Int64 = 0

        If ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeServicio Or ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeCompra Then
            entNewOrdenes.Add(itemOrden)
            Session("OrdenesNew") = entNewOrdenes
            CargarGridOrdenes()
            btnAdd.Visible = True
            btnLimpiar.Visible = True
            entNewOrdenes = Session("OrdenesNew")
            gvOrdenes.Visible = True
            gvOrdenesReq.Visible = False
        ElseIf ddlTipoOrden.SelectedValue = eTipoOrden.OrdenRequerimiento Then
            Dim OrdenesReqNew As List(Of FI_OrdenRequerimientodetalle) = Session("OrdenesReqNew")
            Dim oListRequerimientoN = Ordenes.ObtenerDetalleOR(txtNoOrden.Text)
            If oListRequerimientoN.Count > 0 Then
                For Each item In oListRequerimientoN
                    If OrdenesReqNew Is Nothing Then
                        OrdenesReqNew = New List(Of FI_OrdenRequerimientodetalle)
                    End If
                    OrdenesReqNew.Add(item)
                Next
                Session("OrdenesReqNew") = OrdenesReqNew
                gvOrdenesReq.DataSource = OrdenesReqNew
                gvOrdenesReq.DataBind()
                entNewOrdenes.Add(itemOrden)
                For Each item In OrdenesReqNew
                    valorTotal = valorTotal + item.VrTotal
                Next
                btnAdd.Visible = True
                btnLimpiar.Visible = True
                gvOrdenes.Visible = False
                gvOrdenesReq.Visible = True
            End If
        End If

        hfTipoOrdenRad.Value = ddlTipoOrden.SelectedValue

        If ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeServicio Or ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeCompra Then
            For i As Integer = 0 To entNewOrdenes.Count - 1
                valorTotal = valorTotal + entNewOrdenes.Item(i).ValorOrden
            Next
        End If

        Me.lblTextTotal.Visible = True
        Me.lblTotal.Visible = True
        Me.lblTotal.Text = FormatCurrency(valorTotal, 0)

        LimpiarOrdenes()
    End Sub

    Private Sub gvDetalle_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDetalle.PageIndexChanging
        gvDetalle.PageIndex = e.NewPageIndex
        CargarBusquedaFacturasRadicadas()
        ActivateTab(1)
    End Sub

    Private Sub gvDetalle_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDetalle.RowCommand
        Dim o As New FI.Facturas

        If e.CommandName = "Actualizar" Then
            Me.pnlAnulacion.Visible = False
            Me.btnAdd.Visible = True
            Me.btnLimpiar.Visible = True

            Dim lst = o.ObtenerFactura(gvDetalle.DataKeys(CInt(e.CommandArgument))("id"))

            txtConsecutivo.Text = lst.Consecutivo
            If lst.NoFactura IsNot Nothing Then txtNoFactura.Text = lst.NoFactura
            If lst.Cantidad IsNot Nothing Then txtCantidad.Text = lst.Cantidad
            If lst.VrUnitario IsNot Nothing Then txtValorUnitario.Text = lst.VrUnitario
            If lst.Subtotal IsNot Nothing Then txtSubTotal.Text = lst.Subtotal
            txtValorTotal.Text = lst.ValorTotal
            If lst.Observaciones IsNot Nothing Then txtObservaciones.Text = lst.Observaciones
            ddlTipoDocRadicado.SelectedValue = lst.TipoDocumento
            If lst.IdOrden IsNot Nothing Then hfIdOrden.Value = lst.IdOrden
            If lst.Tipo IsNot Nothing Then hfTipoOrden.Value = lst.Tipo
            hfIdFactura.Value = lst.id
            CargarOrdenesxFactura()
            ActivateTab(0)
        End If
        If e.CommandName = "Borrar" Then
            Me.pnlAnulacion.Visible = False
            o.borrarFacturaDetalle(gvDetalle.DataKeys(CInt(e.CommandArgument))("id"))
            o.borrarLogsAprobacionFactura(gvDetalle.DataKeys(CInt(e.CommandArgument))("id"))
            o.borrarLogsEvaluacionProveedorFactura(gvDetalle.DataKeys(CInt(e.CommandArgument))("id"))
            o.borrarFactura(gvDetalle.DataKeys(CInt(e.CommandArgument))("id"))
            gvOrdenes.DataBind()
            CargarFacturasRadicadas()
            ActivateTab(1)
        End If
        If e.CommandName = "Anular" Then
            hfIdFactura.Value = gvDetalle.DataKeys(CInt(e.CommandArgument))("id")
            Me.pnlAnulacion.Visible = True
            txtUsAnula.Focus()
            ActivateTab(1)
        End If
        If e.CommandName = "Load" Then
            hfFacturaEscaneda.Value = gvDetalle.DataKeys(CInt(e.CommandArgument))("id")
            ActivateTab(1)
        End If

        CargarBusquedaFacturasRadicadas()
        ActivateTab(1)
    End Sub

    Private Sub gvOrdenes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvOrdenes.RowCommand
        If e.CommandName = "Quitar" Then
            Dim oOrdenes As New FI.Ordenes
            If gvOrdenes.DataKeys(CInt(e.CommandArgument))("TipoOrdenId") = eTipoOrden.OrdendeServicio Then
                Dim oServicio = oOrdenes.ObtenerOrdenServicio(gvOrdenes.DataKeys(CInt(e.CommandArgument))("IdOrden"))
                Me.lblTotal.Text = FormatCurrency(FormatCurrency(lblTotal.Text, 0) - FormatCurrency(oServicio.Subtotal, 0), 0)
            ElseIf gvOrdenes.DataKeys(CInt(e.CommandArgument))("TipoOrdenId") = eTipoOrden.OrdendeCompra Then
                Dim oCompra = oOrdenes.ObtenerOrdenCompra(gvOrdenes.DataKeys(CInt(e.CommandArgument))("IdOrden"))
                Me.lblTotal.Text = FormatCurrency(FormatCurrency(lblTotal.Text, 0) - FormatCurrency(oCompra.Subtotal, 0), 0)
            End If
            Dim idOrden As Int32 = Int32.Parse(Me.gvOrdenes.DataKeys(CInt(e.CommandArgument))("IdOrden"))
            Dim entNewOrdenes As List(Of FI_FacturasRadicadas_Detalle_Get_Result)
            Dim itmEnc As New FI_FacturasRadicadas_Detalle_Get_Result
            entNewOrdenes = Session("OrdenesNew")
            For Each itm As FI_FacturasRadicadas_Detalle_Get_Result In entNewOrdenes
                If itm.IdOrden = idOrden Then
                    itmEnc = itm
                End If
            Next

            entNewOrdenes.Remove(itmEnc)
            Session("OrdenesNew") = entNewOrdenes
            CargarGridOrdenes()

            If gvOrdenes.Rows.Count = 0 Then
                btnAdd.Visible = False
            End If
        End If

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

        If hfUsuarioAnula.Value = 0 Then
            AlertJS("Debe seleccionar el Usuario que autoriza la anulación!")
            hfUsuarioAnula.Focus()
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
        e2.UsuarioAnula = hfUsuarioAnula.Value
        o.GuardarLogFactura(e2)

        Limpiar()

        AlertJS("La Factura ha sido Anulada!")
        CargarFacturasRadicadas()

        ActivateTab(1)

    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Limpiar()
        pnlAnulacion.Visible = False
        CargarFacturasRadicadas()
        ActivateTab(1)
    End Sub

    Private Sub _RecepcionFacturas_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(146, UsuarioID) = False Then
            Response.Redirect("../FI_AdministrativoFinanciero/Default-Compras.aspx")
        End If
    End Sub

    Protected Sub btnLoadFile_Click(sender As Object, e As EventArgs) Handles btnLoadFile.Click
        Dim nombreFactura As String = ""
        Dim o As New FI.Facturas
        Dim ent = o.ObtenerFactura(hfFacturaEscaneda.Value)

        If fileup.HasFile Then
            'La carpeta donde voy a almacenar el archivo
            Dim path As String = Server.MapPath("~/Facturas/")
            Dim fileload As New System.IO.FileInfo(fileup.FileName)
            nombreFactura = ent.id & ".pdf"
			'Verifica que las extensiones sean las permitidas, dependiendo de la extensión llama la función
			If fileload.Extension.ToLower() = ".pdf" Then
				fileup.SaveAs(path & nombreFactura)
				AlertJS("El archivo PDF se ha cargado correctamente!")
				ent.Escaneada = True
				o.GuardarFactura(ent)
				Limpiar()
				CargarFacturasRadicadas()
			Else
				AlertJS("Por favor verifique que el archivo que intenta cargar corresponda al formato .pdf")
                fileup.Focus()
                ActivateTab(1)
                Exit Sub
            End If
        Else
            AlertJS("Debe cargar un archivo en formato pdf")
            fileup.Focus()
            ActivateTab(1)
            Exit Sub
        End If

        ActivateTab(1)

    End Sub

    Protected Sub btnViewFile_Click(sender As Object, e As EventArgs) Handles btnViewFile.Click
        Dim nombreFactura As String = ""
        Dim o As New FI.Facturas
        Dim ent = o.ObtenerFactura(hfFacturaEscaneda.Value)
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
            ActivateTab(1)
            Limpiar()
            Exit Sub
        End If
        Limpiar()
        ActivateTab(1)

    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim nombreOrden As String = ""
        Dim o As New FI.Facturas
        Dim daOrdenCompra As New FI.Ordenes
        Dim daOrdenServicio As New FI.Ordenes
        Dim daOrdenRequerimiento As New FI.Ordenes
        Dim oOrdenServicio As FI_OrdenServicio
        Dim oOrdenCompra As FI_OrdenCompra
        Dim oOrdenRequerimiento As FI_OrdenRequerimiento

        If txtNoOrden.Text = "" Then
            AlertJS("Por favor Digite el Número de la Orden a Agregar")
            txtNoOrden.Focus()
            Exit Sub
        End If

        If ddlTipoOrden.SelectedValue = "-1" Then
            AlertJS("Seleccione el Tipo de orden que desea Agregar")
            ddlTipoOrden.Focus()
            Exit Sub
        End If

        If ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeServicio Then
            oOrdenServicio = daOrdenServicio.ObtenerOrdenServicio(txtNoOrden.Text)
            If Not oOrdenServicio Is Nothing Then
                If oOrdenServicio.Estado <> 3 Then
                    AlertJS("La orden de no esta aprobada!")
                    txtNoOrden.Focus()
                    Exit Sub
                End If
            Else
                AlertJS("La orden no existe!")
                txtNoOrden.Focus()
                Exit Sub
            End If
        ElseIf ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeCompra Then
            oOrdenCompra = daOrdenCompra.ObtenerOrdenCompra(txtNoOrden.Text)
            If Not oOrdenCompra Is Nothing Then
                If oOrdenCompra.Estado <> 3 Then
                    AlertJS("La orden de no esta aprobada!")
                    txtNoOrden.Focus()
                    Exit Sub
                End If
            Else
                AlertJS("La orden no existe!")
                txtNoOrden.Focus()
                Exit Sub
            End If
        ElseIf ddlTipoOrden.SelectedValue = eTipoOrden.OrdenRequerimiento Then
            oOrdenRequerimiento = daOrdenRequerimiento.ObtenerOrdenRequerimiento(txtNoOrden.Text)
            If Not oOrdenRequerimiento Is Nothing Then
                If oOrdenRequerimiento.Estado <> 3 Then
                    AlertJS("La orden de no esta aprobada!")
                    txtNoOrden.Focus()
                    Exit Sub
                End If
            Else
                AlertJS("La orden no existe!")
                txtNoOrden.Focus()
                Exit Sub
            End If
        End If

        If ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeServicio Then
            nombreOrden = "ORDENSERVICIO-" & txtNoOrden.Text & ".pdf"
        ElseIf ddlTipoOrden.SelectedValue = eTipoOrden.OrdendeCompra Then
            nombreOrden = "ORDENCOMPRA-" & txtNoOrden.Text & ".pdf"
        ElseIf ddlTipoOrden.SelectedValue = eTipoOrden.OrdenRequerimiento Then
            nombreOrden = "ORDENREQUERIMIENTO-" & txtNoOrden.Text & ".pdf"
        End If

        Dim urlFija As String
        urlFija = "~/Files/"
        urlFija = Server.MapPath(urlFija & "\")

        If Not System.IO.File.Exists(urlFija & "\" & nombreOrden) Then
            Dim generarOrden As New FI_Gestion_Ordenes_Aprobacion
            generarOrden.construirPDF(ddlTipoOrden.SelectedValue, txtNoOrden.Text)
        End If

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "visualiza", "cargarOrden(" & txtNoOrden.Text & ");", True)
        btnAgregar.Visible = True
    End Sub

    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click

        If txtNoOrden.Text = "" Then
            AlertJS("Por favor Digite el Número de la Orden a Agregar")
            txtNoOrden.Focus()
            Exit Sub
        End If

        If ddlTipoOrden.SelectedValue = "-1" Then
            AlertJS("Seleccione el Tipo de orden que desea Agregar")
            ddlTipoOrden.Focus()
            Exit Sub
        End If

        ValidarOrdenesxProveedor()

    End Sub

    Private Sub gvOrdenesReq_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvOrdenesReq.RowCommand
        If e.CommandName = "Quitar" Then
            Dim id = gvOrdenesReq.DataKeys(CInt(e.CommandArgument))("Id")
            Dim OrdenesReq = New List(Of FI_OrdenRequerimientodetalle)
            For Each row As GridViewRow In gvOrdenesReq.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim fd = New FI_OrdenRequerimientodetalle
                    fd.id = gvOrdenesReq.Rows(row.RowIndex).Cells(0).Text
                    fd.IdOrden = gvOrdenesReq.Rows(row.RowIndex).Cells(1).Text
                    fd.Descripcion = gvOrdenesReq.Rows(row.RowIndex).Cells(2).Text
                    fd.Cantidad = DirectCast(gvOrdenesReq.Rows(row.RowIndex).FindControl("txtCantidad"), TextBox).Text.Trim
                    fd.VrUnitario = DirectCast(gvOrdenesReq.Rows(row.RowIndex).FindControl("txtVrUnitario"), TextBox).Text.Trim
                    OrdenesReq.Add(fd)
                End If
            Next

            Dim OrdenesReqNew As List(Of FI_OrdenRequerimientodetalle) = OrdenesReq
            Dim itemOrdenesReqNew As FI_OrdenRequerimientodetalle = Nothing
            For Each item In OrdenesReqNew
                If item.id = id Then
                    itemOrdenesReqNew = item
                End If
            Next
            OrdenesReqNew.Remove(itemOrdenesReqNew)

            Session("OrdenesReqNew") = OrdenesReqNew
            gvOrdenesReq.DataSource = OrdenesReqNew
            gvOrdenesReq.DataBind()

        End If
    End Sub
End Class

