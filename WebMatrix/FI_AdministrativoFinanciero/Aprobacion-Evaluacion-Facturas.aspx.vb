Imports CoreProject
Imports WebMatrix.Util
Public Class Aprobacion_Evaluacion_Facturas
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

#End Region
#Region "Eventos"
    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarGrid()
        End If

    End Sub

    Protected Sub gvDatos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        If e.CommandName = "Seleccionar" Then
            hfFactura.Value = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument)).Value)
            cargarAprobaciones()
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "visualiza", "cargarFactura(" & hfFactura.Value & ");", True)

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

    Sub cargarAprobaciones()
        Dim daOrdenes As New FI.Facturas
        gvAprobaciones.DataSource = daOrdenes.ObtenerLogAprobacionesFactura(hfFactura.Value)
        gvAprobaciones.DataBind()
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

    Private Function obtenerFacturaXId(idFactura As Long) As FI_FacturasRadicadas
        Dim daFacturas As New FI.Facturas
        Return daFacturas.ObtenerFactura(idFactura)
    End Function

    Sub CargarGrid()
        Dim o As New FI.Facturas
        Dim Consecutivo As Int64? = Nothing
        If Not txtNumRadicado.Text = "" Then Consecutivo = txtNumRadicado.Text
        Me.gvDatos.DataSource = o.FacturasPendientesAprobacion(Session("IDUsuario").ToString, Consecutivo)
        Me.gvDatos.DataBind()
    End Sub
    Private Sub btnAprobar_Click(sender As Object, e As EventArgs) Handles btnAprobar.Click
        Dim o As New FI.Facturas
        o.AprobarFactura(hfFactura.Value, txtComentarios.Text, Session("IDUsuario").ToString, True)

        CargarGrid()
        txtComentarios.Text = ""
    End Sub

    Private Sub btnNoAprobar_Click(sender As Object, e As EventArgs) Handles btnNoAprobar.Click
        Dim o As New FI.Facturas
        o.AprobarFactura(hfFactura.Value, txtComentarios.Text, Session("IDUsuario").ToString, False)
        txtComentarios.Text = ""
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargarGrid()
    End Sub
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        txtNumRadicado.Text = ""
        CargarGrid()
    End Sub
#End Region


    Private Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        CargarGrid()
        gvDatos.PageIndex = e.NewPageIndex
        CargarGrid()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub


End Class