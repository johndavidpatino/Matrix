Imports CoreProject
Imports System.IO

Public Class FacturaRadicada
    Inherits System.Web.UI.Page
    Enum eTipoOrden
        servicio = 1
        compra = 2
        requerimiento = 3
    End Enum
    Enum eCosto
        JBE = 1
        JBI = 2
        centroCosto = 3
    End Enum
#Region "Propiedades"

    Private _proveedor As TH_Contratistas
    Public Property proveedor() As TH_Contratistas
        Get
            Return _proveedor
        End Get
        Set(ByVal value As TH_Contratistas)
            _proveedor = value
        End Set
    End Property
    Private _Factura As FI_FacturasRadicadas
    Public Property Factura() As FI_FacturasRadicadas
        Get
            Return _Factura
        End Get
        Set(ByVal value As FI_FacturasRadicadas)
            _Factura = value
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
    Private _ordenServicio As FI_OrdenServicio
    Public Property ordenServicio() As FI_OrdenServicio
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
    Private _ordenRequerimiento As FI_OrdenRequerimiento
    Public Property ordenRequerimiento() As FI_OrdenRequerimiento
        Get
            Return _ordenRequerimiento
        End Get
        Set(ByVal value As FI_OrdenRequerimiento)
            _ordenRequerimiento = value
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

    Private _tipoOrden As eTipoOrden
    Public Property tipoOrden() As eTipoOrden
        Get
            Return _tipoOrden
        End Get
        Set(ByVal value As eTipoOrden)
            _tipoOrden = value
        End Set
    End Property

    Private _idOrden As Long
    Public Property idOrden() As Long
        Get
            Return _idOrden
        End Get
        Set(ByVal value As Long)
            _idOrden = value
        End Set
    End Property

    Private _tipoCosto As eCosto
    Public Property costo() As eCosto
        Get
            Return _tipoCosto
        End Get
        Set(ByVal value As eCosto)
            _tipoCosto = value
        End Set
    End Property


#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim idFactura As Int64
        Dim idcontratista As Long
        If Not Request.QueryString("IdFactura") Is Nothing Then
            If Int64.TryParse(Request.QueryString("IdFactura"), idFactura) Then
                Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/FI_AdministrativoFinanciero/Aprobacion-Evaluacion-Facturas.aspx"
                Factura = obtenerFacturaXId(idFactura)
                FacturaDetalle = obtenerFacturaDetalle(idFactura)
                If Factura.Tipo = eTipoOrden.servicio Then
                    ordenServicio = obtenerOrdenServicioXId(Factura.IdOrden)
                    idcontratista = ordenServicio.ProveedorId
                    idOrden = ordenServicio.id
                    costo = ordenServicio.TipoDetalle
                ElseIf Factura.Tipo = eTipoOrden.compra Then
                    ordenCompra = obtenerOrdenCompraXId(Factura.IdOrden)
                    idcontratista = ordenCompra.ProveedorId
                    idOrden = ordenCompra.id
                    costo = ordenCompra.TipoDetalle
                ElseIf FacturaDetalle(0).TipoOrdenId = eTipoOrden.requerimiento Then
                    ordenRequerimiento = obtenerOrdenRequerimientoXId(FacturaDetalle(0).IdOrden)
                    idcontratista = ordenRequerimiento.ProveedorId
                    idOrden = ordenRequerimiento.id
                    costo = ordenRequerimiento.TipoDetalle
                Else
                    idcontratista = FacturaDetalle(0).NIT_CC
                End If
                If Factura.Tipo IsNot Nothing Then tipoOrden = Factura.Tipo
                contratista = obtenerContratistaXid(idcontratista)
            End If
        End If
        parametrizarMensaje()

        GenerarHtml()

    End Sub
#End Region
#Region "Metodos"

    Sub parametrizarMensaje()
        lblIdFactura.Text = Factura.id
        lblNoRadicado.Text = Factura.Consecutivo
        lblNIT.Text = contratista.Identificacion
        lblProveedor.Text = contratista.Nombre
        lblValor.Text = Factura.ValorTotal
    End Sub
    Function obtenerUsuariosNotificacion() As List(Of String)
        Dim daFacturas As New FI.Facturas
        Dim idUsuario As Int64
        Dim oU As New US.Usuarios
        Dim lstUsuarios As New List(Of String)

        If FacturaDetalle IsNot Nothing Then
            lstUsuarios.AddRange(daFacturas.ObtenerLogAprobacionesFactura(Factura.id).Where(Function(x) x.FechaAprobacion Is Nothing).Select(Function(x) x.Email).Distinct().ToList)
        Else
            If tipoOrden = eTipoOrden.servicio Then
                idUsuario = ordenServicio.SolicitadoPor
            Else
                idUsuario = ordenCompra.SolicitadoPor
            End If

            Dim u = oU.UsuarioGet(idUsuario)

            lstUsuarios.Add(u.Email)
        End If

        Return lstUsuarios
    End Function
    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)
        Dim contenido As String
        Dim oEnviarCorreo As New EnviarCorreo
        Dim lstUsuariosANotificar As New List(Of String)

        lstUsuariosANotificar = obtenerUsuariosNotificacion()

        Me.pnlBody.RenderControl(hw)
        contenido = sb.ToString
        contenido = contenido & "<br/>"


        oEnviarCorreo.sendMail(lstUsuariosANotificar, Me.lblAsunto.Text, contenido)
    End Sub
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

    'Function obtenerDescripcion() As String
    '    Dim descripcion As String
    '    Dim daFI As New FI.Ordenes

    '    If tipoOrden = eTipoOrden.servicio Then

    '        descripcion = If(ordenServicio.TipoDetalle = eCosto.JBE OrElse ordenServicio.TipoDetalle = eCosto.JBI, ordenServicio.JobBookCodigo & "-" & ordenServicio.JobBookNombre, obtenerCentroCosto(ordenServicio.CentroDeCosto).CentroDeCosto)
    '    Else

    '        descripcion = If(ordenCompra.TipoDetalle = eCosto.JBE OrElse ordenCompra.TipoDetalle = eCosto.JBI, ordenCompra.JobBookCodigo & "-" & ordenCompra.JobBookNombre, obtenerCentroCosto(ordenCompra.CentroDeCosto).CentroDeCosto)
    '    End If

    '    Return descripcion
    'End Function
    Function obtenerCentroCosto(ByVal id As Integer) As FI_CentroCosto_Get_Result
        Dim daFI As New FI.Ordenes
        Return daFI.obtenerCentroCostoXId(id)
    End Function
#End Region
End Class