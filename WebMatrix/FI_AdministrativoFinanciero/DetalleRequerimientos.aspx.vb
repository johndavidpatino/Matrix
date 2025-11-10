Imports CoreProject
Imports ClosedXML.Excel
Public Class DetalleRadicacion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Me.IsPostBack) Then

        End If
        ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnExportar)
    End Sub

    'Sub cargarOrdenesServiciofacturas()

    '    gvDatos.DataSource = obtenerOrdenesServicioFacturas()
    '    gvDatos.DataBind()
    '    upOrdenesFacturas.Update()
    'End Sub
    'Sub cargarOrdenesComprafacturas()

    '    gvDatos.DataSource = obtenerOrdenesCompraFacturas()
    '    gvDatos.DataBind()
    '    upOrdenesFacturas.Update()
    'End Sub

    Sub cargarOrdenesUnificadas()

        gvDatos.DataSource = obtenerOrdenesUnificadasRequerimientosServicio()
        gvDatos.DataBind()
        upOrdenesFacturas.Update()
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        'If ddlTipoOrden.SelectedValue = 1 Then
        '    cargarOrdenesServiciofacturas()
        'Else
        '    cargarOrdenesComprafacturas()
        'End If

        cargarOrdenesUnificadas()

        If hfNumRequerimiento.Value > 0 Then
            btnExportar.Visible = True
        Else
            btnExportar.Visible = False
        End If

    End Sub

    Private Sub gvDatos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        cargarOrdenesUnificadas()
    End Sub
    Private Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Select Case e.CommandArgument
            Case "First"
                gvDatos.PageIndex = 0
            Case "Prev"
                If gvDatos.PageIndex > 0 Then
                    gvDatos.PageIndex -= 1
                End If
            Case "Next"
                If gvDatos.PageIndex < gvDatos.PageCount - 1 Then
                    gvDatos.PageIndex += 1
                End If
            Case "Last"
                gvDatos.PageIndex = gvDatos.PageCount - 1
        End Select
        'If ddlTipoOrden.SelectedValue = 1 Then
        '    cargarOrdenesServiciofacturas()
        'Else
        '    cargarOrdenesComprafacturas()
        'End If
        cargarOrdenesUnificadas()
    End Sub
    Private Sub gvDatos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDatos.RowDataBound
        If e.Row.RowType = DataControlRowType.Pager Then
            CType(e.Row.FindControl("lblPaginaActual"), Label).Text = gvDatos.PageIndex + 1
            CType(e.Row.FindControl("lblCantidadPaginas"), Label).Text = gvDatos.PageCount
        End If
    End Sub

    Private Sub gvProveedores_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvProveedores.RowCommand
        If e.CommandName = "Seleccionar" Then
            hfProveedor.Value = Me.gvProveedores.DataKeys(CInt(e.CommandArgument))("Identificacion")
            Me.txtProveedor.Text = Server.HtmlDecode(Me.gvProveedores.Rows(CInt(e.CommandArgument)).Cells(1).Text.ToString)
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
    Sub exportarExcel()
        Dim wb As New XLWorkbook
        'Dim oLstOrdenesCompraFacturas As List(Of REP_OrdenesCompra_Facturas_Result)
        'Dim oLstOrdenesServicioFacturas As List(Of REP_OrdenesServicio_Facturas_Result)
        Dim oLstOrdenesUnificadaFacturas As List(Of REP_OrdenesUnificado_RequerimientosServicio_Result)
        Dim titulosProduccion As String = "Id Orden;Fecha Solicitud;Creado Por;Fecha Creacion Orden;Solicitado Por;Usuario Evalua Factura;Tipo Pago;Tipo Orden;Tipo Texto;Estado;Observacion Anulacion;Identificacion Proveedor;Nombre Proveedor;Tipo;ID Trabajo;JobBook;JobBook Nombre;Valor Orden;Descripción;Cantidad;Vr Unitario;Valor Total;Numero Factura;Numero Radicado;Fecha Radicado Factura;Cantidad Radicado;Vr Unitario Radicado;Valor Total Radicado;Cuenta Contable;Usuario Radica;Fecha Recibida;Fecha Enviada A Aprobacion;Estado Final Radicado;COE APRUEBA;ESTADO COE;FECHA COE;OBSERVACION COE;GERENTE APRUEBA;ESTADO GERENTE;FECHA GERENTE;OBSERVACION GERENTE;ADMON APRUEBA;ESTADO ADMON;FECHA ADMON;OBSERVACION ADMON;"

        Dim ws = wb.Worksheets.Add("OrdenesRequerimientoServicio")
        insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

        'If ddlTipoOrden.SelectedValue = 1 Then
        '    oLstOrdenesServicioFacturas = obtenerOrdenesServicioFacturas()
        '    ws.Cell(2, 1).InsertData(oLstOrdenesServicioFacturas)
        'Else
        '    oLstOrdenesCompraFacturas = obtenerOrdenesCompraFacturas()
        '    ws.Cell(2, 1).InsertData(oLstOrdenesCompraFacturas)
        'End If

        oLstOrdenesUnificadaFacturas = obtenerOrdenesUnificadasRequerimientosServicio()
        ws.Cell(2, 1).InsertData(oLstOrdenesUnificadaFacturas)

        exportarExcel(wb, "OrdenesFacturas")
    End Sub
    Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String, ByVal fila As Integer)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(fila, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub
    Private Sub exportarExcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""Control_" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub
    Function obtenerOrdenesServicioFacturas() As List(Of REP_OrdenesServicio_Facturas_Result)
        Dim daOrdenesFacturas As New OrdenesFacturas
        Dim idOrden As Long? = If(String.IsNullOrEmpty(txtIdOrden.Text), CType(Nothing, Long?), CType(txtIdOrden.Text, Long?))
        Dim OrdenFechaInicio As Date? = If(String.IsNullOrEmpty(txtOrdenFechaInicio.Text), CType(Nothing, Date?), CType(txtOrdenFechaInicio.Text, Date?))
        Dim OrdenFechaFin As Date? = If(String.IsNullOrEmpty(txtOrdenFechaFin.Text), CType(Nothing, Date?), CType(txtOrdenFechaFin.Text, Date?))
        Dim FacturaFechaInicio As Date? = If(String.IsNullOrEmpty(txtFacturaFechaInicio.Text), CType(Nothing, Date?), CType(txtFacturaFechaInicio.Text, Date?))
        Dim FacturaFechaFin As Date? = If(String.IsNullOrEmpty(txtFacturaFechaFin.Text), CType(Nothing, Date?), CType(txtFacturaFechaFin.Text, Date?))
        Dim FacturaConsecutivo As Long? = If(String.IsNullOrEmpty(txtFacturaConsecutivo.Text), CType(Nothing, Long?), CType(txtFacturaConsecutivo.Text, Long?))
        Return daOrdenesFacturas.obtenerOrdenesServicioFacturas(idOrden, OrdenFechaInicio, OrdenFechaFin, FacturaFechaInicio, FacturaFechaFin, FacturaConsecutivo)
    End Function
    Function obtenerOrdenesCompraFacturas() As List(Of REP_OrdenesCompra_Facturas_Result)
        Dim daOrdenesFacturas As New OrdenesFacturas
        Dim idOrden As Long? = If(String.IsNullOrEmpty(txtIdOrden.Text), CType(Nothing, Long?), CType(txtIdOrden.Text, Long?))
        Dim OrdenFechaInicio As Date? = If(String.IsNullOrEmpty(txtOrdenFechaInicio.Text), CType(Nothing, Date?), CType(txtOrdenFechaInicio.Text, Date?))
        Dim OrdenFechaFin As Date? = If(String.IsNullOrEmpty(txtOrdenFechaFin.Text), CType(Nothing, Date?), CType(txtOrdenFechaFin.Text, Date?))
        Dim FacturaFechaInicio As Date? = If(String.IsNullOrEmpty(txtFacturaFechaInicio.Text), CType(Nothing, Date?), CType(txtFacturaFechaInicio.Text, Date?))
        Dim FacturaFechaFin As Date? = If(String.IsNullOrEmpty(txtFacturaFechaFin.Text), CType(Nothing, Date?), CType(txtFacturaFechaFin.Text, Date?))
        Dim FacturaConsecutivo As Long? = If(String.IsNullOrEmpty(txtFacturaConsecutivo.Text), CType(Nothing, Long?), CType(txtFacturaConsecutivo.Text, Long?))
        Return daOrdenesFacturas.obtenerOrdenesCompraFacturas(idOrden, OrdenFechaInicio, OrdenFechaFin, FacturaFechaInicio, FacturaFechaFin, FacturaConsecutivo)
    End Function

    Function obtenerOrdenesUnificadasRequerimientosServicio() As List(Of REP_OrdenesUnificado_RequerimientosServicio_Result)
        Dim daOrdenesFacturas As New OrdenesFacturas
        Dim idOrden As Long? = If(String.IsNullOrEmpty(txtIdOrden.Text), CType(Nothing, Long?), CType(txtIdOrden.Text, Long?))
        Dim OrdenFechaInicio As Date? = If(String.IsNullOrEmpty(txtOrdenFechaInicio.Text), CType(Nothing, Date?), CType(txtOrdenFechaInicio.Text, Date?))
        Dim OrdenFechaFin As Date? = If(String.IsNullOrEmpty(txtOrdenFechaFin.Text), CType(Nothing, Date?), CType(txtOrdenFechaFin.Text, Date?))
        Dim FacturaFechaInicio As Date? = If(String.IsNullOrEmpty(txtFacturaFechaInicio.Text), CType(Nothing, Date?), CType(txtFacturaFechaInicio.Text, Date?))
        Dim FacturaFechaFin As Date? = If(String.IsNullOrEmpty(txtFacturaFechaFin.Text), CType(Nothing, Date?), CType(txtFacturaFechaFin.Text, Date?))
        Dim FacturaConsecutivo As Long? = If(String.IsNullOrEmpty(txtFacturaConsecutivo.Text), CType(Nothing, Long?), CType(txtFacturaConsecutivo.Text, Long?))
        Dim TipoOrden As Int64? = Nothing
        If Not (ddlTipoOrden.SelectedValue = "-1") Then TipoOrden = ddlTipoOrden.SelectedValue
        Dim Proveedor As Int64? = Nothing
        If Not (hfProveedor.Value = "0") Then Proveedor = hfProveedor.Value
        Dim IdTrabajo As Int64? = Nothing
        If Not (txtIdTrabajo.Text = "") Then IdTrabajo = Convert.ToInt32(txtIdTrabajo.Text)
        Dim datasource As List(Of REP_OrdenesUnificado_RequerimientosServicio_Result) = daOrdenesFacturas.obtenerOrdenesUnificadasRequerimientoServicio(idOrden, OrdenFechaInicio, OrdenFechaFin, FacturaFechaInicio, FacturaFechaFin, FacturaConsecutivo, TipoOrden, Proveedor, IdTrabajo)
        hfNumRequerimiento.Value = datasource.Count
        Return datasource
    End Function

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        exportarExcel()
    End Sub

    Protected Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        hfProveedor.Value = "0"
        txtProveedor.Text = ""
        txtNitProveedor.Text = ""
        txtNombreProveedor.Text = ""
        gvProveedores.DataSource = Nothing
        gvProveedores.DataBind()
        UPanelProveedores.Update()
    End Sub

    Private Sub btnBuscarCentroCosto_Click(sender As Object, e As EventArgs) Handles btnBuscarCentroCosto.Click
        gvCentroCosto.DataSource = obtenerCentroCosto(txtNumeroCuenta.Text, txtDescripcionCuenta.Text)
        gvCentroCosto.DataBind()
    End Sub

    Function obtenerCentroCosto(ByVal numeroCuenta As String, ByVal descripcion As String) As List(Of FI_CentroCostos_Get_Result)
        Dim o As New FI.Ordenes
        Dim numCentro
        If String.IsNullOrEmpty(numeroCuenta) Then
            numCentro = Nothing
        Else
            numCentro = Convert.ToInt64(numeroCuenta)
        End If
        If String.IsNullOrEmpty(descripcion) Then
            descripcion = Nothing
        End If

        Return o.obtenerCentroCostos(numCentro, descripcion)
    End Function
End Class