Imports CoreProject
Imports ClosedXML.Excel

Public Class ReporteFacturasRadicadas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Me.IsPostBack) Then

        End If
        ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnImgExportarInforme0)
    End Sub

    Sub cargarFacturasRadicadas()

        gvDatos.DataSource = obtenerFacturasRadicadas()
        gvDatos.DataBind()
        upOrdenesFacturas.Update()
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        cargarFacturasRadicadas()

        btnImgExportarInforme0.Visible = True
    End Sub

    Private Sub gvDatos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        cargarFacturasRadicadas()
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

        cargarFacturasRadicadas()
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

        Dim oLstaFacturasRadicadas As List(Of REP_FacturasRadicadas_Result)
        Dim titulosProduccion As String = "#;IdFactura;NumeroRadicado;NumeroFactura;IdOrden;CentrodeCosto;Estado;ObservacionesAnulacion;ValorFactura;IdentificacionProveedor;NombreProveedor;FechaRadicadoFactura;UsuarioRadica;FechaRecibida;FechaEnviadaAAprobacion;FechaAprobada;FechaEnviadaAContabilidad;FechaEnviadaATesoreria;FechaPagada;ValorPagado;FechaAnulada;TipoOrden;CuentaContable"

        Dim ws = wb.Worksheets.Add("FacturasRadicadas")
        insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

        oLstaFacturasRadicadas = obtenerFacturasRadicadas()
        ws.Cell(2, 1).InsertData(oLstaFacturasRadicadas)
        ws.Column(1).Delete()

        exportarExcel(wb, "FacturasRadicadas")
    End Sub
    Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String, ByVal fila As Integer)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(fila, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub
    Private Sub exportarExcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""Listado_" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

    Function obtenerFacturasRadicadas() As List(Of REP_FacturasRadicadas_Result)
        Dim daOrdenesFacturas As New OrdenesFacturas
        Dim FacturaConsecutivo As Long? = If(String.IsNullOrEmpty(txtFacturaConsecutivo.Text), CType(Nothing, Long?), CType(txtFacturaConsecutivo.Text, Long?))
        Dim Proveedor As Int64? = Nothing
        If Not (hfProveedor.Value = "0") Then Proveedor = hfProveedor.Value
        Dim FacturaFechaInicio As Date? = If(String.IsNullOrEmpty(txtFacturaFechaInicio.Text), CType(Nothing, Date?), CType(txtFacturaFechaInicio.Text, Date?))
        Dim FacturaFechaFin As Date? = If(String.IsNullOrEmpty(txtFacturaFechaFin.Text), CType(Nothing, Date?), CType(txtFacturaFechaFin.Text, Date?))
        Dim CentroCosto As Int64? = Nothing
        If Not (hfCentroCosto.Value = "0") Then CentroCosto = hfCentroCosto.Value

        Return daOrdenesFacturas.obtenerFacturasRadicadas(FacturaConsecutivo, Proveedor, FacturaFechaInicio, FacturaFechaFin, CentroCosto)
    End Function


    Protected Sub btnImgExportarInforme0_Click(sender As Object, e As ImageClickEventArgs) Handles btnImgExportarInforme0.Click
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
    Private Sub gvCentroCosto_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCentroCosto.RowCommand
        If e.CommandName = "Seleccionar" Then
            hfCentroCosto.Value = Server.HtmlDecode(Me.gvCentroCosto.Rows(CInt(e.CommandArgument)).Cells(0).Text.ToString)
            txtCentroCosto.Text = Server.HtmlDecode(Me.gvCentroCosto.Rows(CInt(e.CommandArgument)).Cells(1).Text.ToString)
        End If
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

    Private Sub btnLimpiarCC_Click(sender As Object, e As EventArgs) Handles btnLimpiarCC.Click
        hfCentroCosto.Value = "0"
        txtCentroCosto.Text = ""
        txtNumeroCuenta.Text = ""
        txtDescripcionCuenta.Text = ""
        gvCentroCosto.DataSource = Nothing
        gvCentroCosto.DataBind()
        UPanelCentroCosto.Update()
    End Sub
End Class