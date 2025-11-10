Imports CoreProject
Imports WebMatrix.Util
Public Class Evaluacion_Proveedor_Facturas
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Private _factura As FI_FacturasRadicadas
    Public Property factura() As FI_FacturasRadicadas
        Get
            Return _factura
        End Get
        Set(ByVal value As FI_FacturasRadicadas)
            _factura = value
        End Set
    End Property
    Private _contratista As TH_Contratistas
    Public Property contratista() As TH_Contratistas
        Get
            Return _contratista
        End Get
        Set(ByVal value As TH_Contratistas)
            _contratista = value
        End Set
    End Property

    Private _ordenServicio As FI_OrdenServicio
    Public Property ordenservicio() As FI_OrdenServicio
        Get
            Return _ordenServicio
        End Get
        Set(ByVal value As FI_OrdenServicio)
            _ordenServicio = value
        End Set
    End Property
    Private _ordenCompra As FI_OrdenCompra
    Public Property ordenCompra() As FI_OrdenCompra
        Get
            Return _ordenCompra
        End Get
        Set(ByVal value As FI_OrdenCompra)
            _ordenCompra = value
        End Set
    End Property
    Private _OrdenRequerimiento As FI_OrdenRequerimiento
    Public Property ordenRequerimiento() As FI_OrdenRequerimiento
        Get
            Return _OrdenRequerimiento
        End Get
        Set(ByVal value As FI_OrdenRequerimiento)
            _OrdenRequerimiento = value
        End Set
    End Property
    Private _FacturaDetalle As List(Of FI_FacturasRadicadas_Detalle_Get_Result)
    Public Property FacturaDetalle() As List(Of FI_FacturasRadicadas_Detalle_Get_Result)
        Get
            Return _FacturaDetalle
        End Get
        Set(ByVal value As List(Of FI_FacturasRadicadas_Detalle_Get_Result))
            _FacturaDetalle = value
        End Set
    End Property


#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarGrid()
        End If

    End Sub

    Private Sub RBP1_1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP1_1.SelectedIndexChanged
        If RBP1_1.SelectedValue > 0 AndAlso RBP1_1.SelectedValue < 8 Then
            pnlTxtP1_1.Visible = True
        Else
            pnlTxtP1_1.Visible = False
            txtP1_1.Text = ""
        End If
    End Sub
    Private Sub RBP1_2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP1_2.SelectedIndexChanged
        If RBP1_2.SelectedValue > 0 AndAlso RBP1_2.SelectedValue < 8 Then
            pnlTxtP1_2.Visible = True
        Else
            pnlTxtP1_2.Visible = False
            txtP1_2.Text = ""
        End If
    End Sub
    Private Sub RBP1_3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP1_3.SelectedIndexChanged
        If RBP1_3.SelectedValue > 0 AndAlso RBP1_3.SelectedValue < 8 Then
            pnlTxtP1_3.Visible = True
        Else
            pnlTxtP1_3.Visible = False
            txtP1_3.Text = ""
        End If
    End Sub
    Private Sub RBP1_4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP1_4.SelectedIndexChanged
        If RBP1_4.SelectedValue > 0 AndAlso RBP1_4.SelectedValue < 8 Then
            pnlTxtP1_4.Visible = True
        Else
            pnlTxtP1_4.Visible = False
            txtP1_4.Text = ""
        End If
    End Sub
    Private Sub RBP1_5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP1_5.SelectedIndexChanged
        If RBP1_5.SelectedValue > 0 AndAlso RBP1_5.SelectedValue < 8 Then
            pnlTxtP1_5.Visible = True
        Else
            pnlTxtP1_5.Visible = False
            txtP1_5.Text = ""
        End If
        RBP1_5_96.ClearSelection()
    End Sub
    Private Sub RBP1_5_96_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP1_5_96.SelectedIndexChanged
        RBP1_5.ClearSelection()
        pnlTxtP1_5.Visible = False
        txtP1_5.Text = ""
    End Sub
    Private Sub RBP1_6_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP1_6.SelectedIndexChanged
        If RBP1_6.SelectedValue > 0 AndAlso RBP1_6.SelectedValue < 8 Then
            pnlTxtP1_6.Visible = True
        Else
            pnlTxtP1_6.Visible = False
            txtP1_6.Text = ""
        End If
        RBP1_6_96.ClearSelection()
    End Sub
    Private Sub RBP1_6_96_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP1_6_96.SelectedIndexChanged
        RBP1_6.ClearSelection()
        pnlTxtP1_6.Visible = False
        txtP1_6.Text = ""
    End Sub
    Private Sub RBP1_7_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP1_7.SelectedIndexChanged
        If RBP1_7.SelectedValue > 0 AndAlso RBP1_7.SelectedValue < 8 Then
            pnlTxtP1_7.Visible = True
        Else
            pnlTxtP1_7.Visible = False
            txtP1_7.Text = ""
        End If
        RBP1_7_96.ClearSelection()
    End Sub
    Private Sub RBP1_7_96_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP1_7_96.SelectedIndexChanged
        RBP1_7.ClearSelection()
        pnlTxtP1_7.Visible = False
        txtP1_7.Text = ""
    End Sub

    Private Sub RBP2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP2.SelectedIndexChanged
        If RBP2.SelectedValue < 3 Then
            pnlTxtP3.Visible = False
        Else
            pnlTxtP3.Visible = True
        End If
        txtP3.Text = ""
    End Sub
    Protected Sub gvDatos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        If e.CommandName = "Seleccionar" Then
            limpiar()
            hfFactura.Value = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument)).Value)
            factura = obtenerFacturaXId(hfFactura.Value)
            If factura.Tipo = 1 Then
                ordenservicio = obtenerOrdenServicioXId(factura.IdOrden)
                contratista = obtenerContratistaXid(ordenservicio.ProveedorId)
            ElseIf factura.Tipo = 2 Then
                ordenCompra = obtenerOrdenCompraXId(factura.IdOrden)
                contratista = obtenerContratistaXid(ordenCompra.ProveedorId)
            ElseIf factura.Tipo = 3 Then
                ordenRequerimiento = obtenerOrdenRequerimientoXId(factura.IdOrden)
                contratista = obtenerContratistaXid(ordenRequerimiento.ProveedorId)
            Else
                FacturaDetalle = obtenerFacturaDetalle(hfFactura.Value)
                contratista = obtenerContratistaXid(FacturaDetalle(0).NIT_CC)
            End If
            lblProveedor1.Text = contratista.Nombre
            lblProveedor2.Text = contratista.Nombre
            lblProveedor3.Text = contratista.Nombre
            lblFacturaNo.Text = Server.HtmlDecode(Me.gvDatos.Rows(e.CommandArgument).Cells(3).Text)
            lblRadicadoNo.Text = Server.HtmlDecode(Me.gvDatos.Rows(e.CommandArgument).Cells(4).Text)
            lblProveedor.Text = Server.HtmlDecode(Me.gvDatos.Rows(e.CommandArgument).Cells(5).Text)
            lblNitCC.Text = Server.HtmlDecode(Me.gvDatos.Rows(e.CommandArgument).Cells(6).Text)
            lblValor.Text = Server.HtmlDecode(Me.gvDatos.Rows(e.CommandArgument).Cells(7).Text)
            If evaluarProveedor(contratista.Identificacion) Then
                pnlAprobacion.Visible = True
                btnAprobarSinEvaluar.Visible = False
                btnEvaluarYAprobar.Visible = False
                pnlEvaluacion.Visible = True
            Else
                pnlAprobacion.Visible = True
                btnAprobarSinEvaluar.Visible = True
                btnEvaluarYAprobar.Visible = True
            End If

        End If

        If e.CommandName = "Load" Then
            hfFactura.Value = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument)).Value)
            Dim nombreFactura As String = ""
            Dim o As New FI.Facturas
            Dim ent = o.ObtenerFactura(hfFactura.Value)
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
                gvDatos.Focus()
                Exit Sub
            End If

        End If
    End Sub

    Private Sub btnEnviar_Click(sender As Object, e As EventArgs) Handles btnEnviar.Click
        Dim usuarioEvalua As Long
        usuarioEvalua = Session("IDUsuario").ToString
        GuardarCalificacion(1, Me.RBP1_1.SelectedValue, Nothing, usuarioEvalua)
        If Me.pnlTxtP1_1.Visible = True Then
            GuardarCalificacion(2, Nothing, txtP1_1.Text, usuarioEvalua)
        End If
        GuardarCalificacion(3, Me.RBP1_2.SelectedValue, Nothing, usuarioEvalua)
        If Me.pnlTxtP1_2.Visible = True Then
            GuardarCalificacion(4, Nothing, txtP1_2.Text, usuarioEvalua)
        End If
        GuardarCalificacion(5, Me.RBP1_3.SelectedValue, Nothing, usuarioEvalua)
        If Me.pnlTxtP1_3.Visible = True Then
            GuardarCalificacion(6, Nothing, txtP1_3.Text, usuarioEvalua)
        End If
        GuardarCalificacion(7, Me.RBP1_4.SelectedValue, Nothing, usuarioEvalua)
        If Me.pnlTxtP1_4.Visible = True Then
            GuardarCalificacion(8, Nothing, txtP1_4.Text, usuarioEvalua)
        End If
        GuardarCalificacion(9, If(String.IsNullOrEmpty(Me.RBP1_5.SelectedValue), RBP1_5_96.SelectedValue, RBP1_5.SelectedValue), Nothing, usuarioEvalua)
        If Me.pnlTxtP1_5.Visible = True Then
            GuardarCalificacion(10, Nothing, txtP1_5.Text, usuarioEvalua)
        End If
        GuardarCalificacion(11, If(String.IsNullOrEmpty(Me.RBP1_6.SelectedValue), RBP1_6_96.SelectedValue, RBP1_6.SelectedValue), Nothing, usuarioEvalua)
        If Me.pnlTxtP1_6.Visible = True Then
            GuardarCalificacion(12, Nothing, txtP1_6.Text, usuarioEvalua)
        End If
        GuardarCalificacion(13, If(String.IsNullOrEmpty(Me.RBP1_7.SelectedValue), RBP1_7_96.SelectedValue, RBP1_7.SelectedValue), Nothing, usuarioEvalua)
        If Me.pnlTxtP1_7.Visible = True Then
            GuardarCalificacion(14, Nothing, txtP1_7.Text, usuarioEvalua)
        End If
        GuardarCalificacion(15, RBP2.SelectedValue, Nothing, usuarioEvalua)
        If Me.pnlTxtP3.Visible = True Then
            GuardarCalificacion(16, Nothing, txtP3.Text, usuarioEvalua)
        End If

        evaluarFactura(hfFactura.Value, "Proveedor Evaluado", True)
        If RBP2.SelectedValue = 2 Then
            Response.Redirect("QuejasReclamosProveedores.aspx?vista=nuevo")
        Else
            limpiar()
            CargarGrid()
        End If
    End Sub
    Protected Sub btnEvaluarYAprobar_Click(sender As Object, e As EventArgs) Handles btnEvaluarYAprobar.Click
        pnlEvaluacion.Visible = True
        btnEvaluarYAprobar.Visible = False
        btnAprobarSinEvaluar.Visible = False
    End Sub

    Protected Sub btnAprobarSinEvaluar_Click(sender As Object, e As EventArgs) Handles btnAprobarSinEvaluar.Click
        evaluarFactura(hfFactura.Value, "El Proveedor no necesitaba ser Evaluado", False)
        btnEvaluarYAprobar.Visible = False
        btnAprobarSinEvaluar.Visible = False
        pnlAprobacion.Visible = False
        CargarGrid()
    End Sub
#End Region
#Region "Metodos"
    Function obtenerContratistaXid(ByVal id As Long) As TH_Contratistas
        Dim o As New Contratista
        Return o.ObtenerContratista(id)
    End Function
    Function obtenerOrdenServicioXId(ByVal id As Int64) As FI_OrdenServicio
        Dim daOrden As New FI.Ordenes
        Return daOrden.ObtenerOrdenServicio(id)
    End Function
    Function obtenerOrdenCompraXId(ByVal id As Int64) As FI_OrdenCompra
        Dim daOrden As New FI.Ordenes
        Return daOrden.ObtenerOrdenCompra(id)
    End Function
    Function obtenerOrdenRequerimientoXId(ByVal id As Int64) As FI_OrdenRequerimiento
        Dim daOrden As New FI.Ordenes
        Return daOrden.ObtenerOrdenRequerimiento(id)
    End Function

    Private Function obtenerFacturaXId(idFactura As Long) As FI_FacturasRadicadas
        Dim daFacturas As New FI.Facturas
        Return daFacturas.ObtenerFactura(idFactura)
    End Function

    Private Function obtenerFacturaDetalle(idFactura As Long) As List(Of FI_FacturasRadicadas_Detalle_Get_Result)
        Dim daFacturas As New FI.Facturas
        Return daFacturas.ObtenerFacturasRadicadasDetalle(idFactura, Nothing, Nothing, Nothing)
    End Function

    Sub GuardarCalificacion(ByVal item As Integer, ByVal calificacion As Integer?, abierta As String, usuarioEvalua As Long)
        Dim o As New FI.Facturas
        Dim e As New CO_EvaluacionProveedoresFactura
        e.IdFactura = hfFactura.Value
        e.Usuario = usuarioEvalua
        e.Fecha = Date.UtcNow.AddHours(-5)
        e.Item = item
        e.Calificacion = calificacion
        e.Abierta = abierta
        o.GuardarCalificacionEvaluacionProveedor(e)
    End Sub
    Sub limpiar()
        Me.RBP1_1.ClearSelection()
        Me.RBP1_2.ClearSelection()
        Me.RBP1_3.ClearSelection()
        Me.RBP1_4.ClearSelection()
        Me.RBP1_5.ClearSelection()
        Me.RBP1_5_96.ClearSelection()
        Me.RBP1_6.ClearSelection()
        Me.RBP1_6_96.ClearSelection()
        Me.RBP1_7.ClearSelection()
        Me.RBP1_7_96.ClearSelection()
        Me.RBP2.ClearSelection()
        Me.txtP3.Text = ""
        Me.pnlTxtP3.Visible = False
        Me.pnlEvaluacion.Visible = False
        pnlAprobacion.Visible = False

    End Sub
    Sub CargarGrid()
        Dim o As New FI.Facturas
        Me.gvDatos.DataSource = o.FacturasPendientesEvaluacion(Session("IDUsuario").ToString)
        Me.gvDatos.DataBind()
    End Sub
    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub
    Function evaluarProveedor(ByVal idProveedor As Int64) As Boolean
        Dim daFI As New FI.Facturas
        Dim fechaUltimaEvaluacion As Date?
        fechaUltimaEvaluacion = daFI.obtenerUltimaEvaluacionProveedor(idProveedor)
		If fechaUltimaEvaluacion.HasValue = False OrElse Date.Now.Year <> fechaUltimaEvaluacion.Value.Year Then
			Return True
		Else
			Return False
        End If
    End Function
    Sub evaluarFactura(ByVal idFactura As Int64, ByVal comentarios As String, ByVal evaluado As Boolean)
        Dim o As New FI.Facturas
        o.EvaluarFactura(idFactura, comentarios, Session("IDUsuario").ToString, evaluado)
    End Sub
#End Region

    Private Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        Dim o As New FI.Facturas
        gvDatos.PageIndex = e.NewPageIndex
        Me.gvDatos.DataSource = o.FacturasPendientesEvaluacion(Session("IDUsuario").ToString)
        Me.gvDatos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub


End Class