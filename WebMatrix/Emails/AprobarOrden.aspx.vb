Imports CoreProject
Imports System.IO

Public Class AprobarOrden
    Inherits System.Web.UI.Page
#Region "Enumerados"
    Enum ETipo
        JBE = 1
        JBI = 2
        CentroCosto = 3
    End Enum
    Enum ETipoOrden
        Servicio = 1
        Compra = 2
        Requerimiento = 3
    End Enum
#End Region
#Region "Propiedades"

    Private _Trabajo As PY_Trabajo_Get_Result
    Public Property Trabajo() As PY_Trabajo_Get_Result
        Get
            Return _Trabajo
        End Get
        Set(ByVal value As PY_Trabajo_Get_Result)
            _Trabajo = value
        End Set
    End Property
    Private _centroCosto As FI_CentroCosto_Get_Result
    Public Property centroCosto As FI_CentroCosto_Get_Result
        Get
            Return _centroCosto
        End Get
        Set(ByVal value As FI_CentroCosto_Get_Result)
            _centroCosto = value
        End Set
    End Property
    Private _Contratista As TH_Contratistas
    Public Property Contratista() As TH_Contratistas
        Get
            Return _Contratista
        End Get
        Set(ByVal value As TH_Contratistas)
            _Contratista = value
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
    Private _proyecto As PY_Proyectos_Get_Result
    Public Property proyecto() As PY_Proyectos_Get_Result
        Get
            Return _proyecto
        End Get
        Set(ByVal value As PY_Proyectos_Get_Result)
            _proyecto = value
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
    Private _tipoOrden As ETipoOrden
    Public Property tipoOrden() As ETipoOrden
        Get
            Return _tipoOrden
        End Get
        Set(ByVal value As ETipoOrden)
            _tipoOrden = value
        End Set
    End Property


#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim idOrden As Int64
        If Request.QueryString("IdOrden") Is Nothing Then
            Exit Sub
        End If
        If Int64.TryParse(Request.QueryString("IdOrden"), idOrden) = False Then
            Exit Sub
        End If
        If Request.QueryString("TipoOrden") Is Nothing Then
            Exit Sub
        End If
        If Int64.TryParse(Request.QueryString("TipoOrden"), tipoOrden) = False Then
            Exit Sub
        End If
		lnkRuta.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/FI_AdministrativoFinanciero/Gestion-Ordenes-Aprobacion.aspx"
		Select Case tipoOrden
            Case ETipoOrden.Servicio
                ordenServicio = obtenerOrdenServicioXId(idOrden)
            Case ETipoOrden.Compra
                ordenCompra = obtenerOrdenCompraXId(idOrden)
            Case ETipoOrden.Requerimiento
                ordenRequerimiento = obtenerOrdenRequerimientoXId(idOrden)
        End Select


        parametrizarMensaje()
        Dim page As Page = DirectCast(Context.Handler, Page)
        GenerarHtml()

    End Sub
#End Region
#Region "Metodos"

    Sub parametrizarMensaje()
        Dim idJBIJBE As String = ""
        Dim idCentroCosto As Int64?
        Dim idProveedor As Int64?
        Dim nombreJBIJBE As String = ""
        Dim tipoDetalle As Short

        Select Case tipoOrden
            Case ETipoOrden.Servicio
                idJBIJBE = ordenServicio.JobBookCodigo
                idCentroCosto = ordenServicio.CentroDeCosto
                idProveedor = ordenServicio.ProveedorId
                lblValor.Text = ordenServicio.Subtotal
                nombreJBIJBE = ordenServicio.JobBookNombre
                tipoDetalle = ordenServicio.TipoDetalle
            Case ETipoOrden.Compra
                idJBIJBE = ordenCompra.JobBook
                idCentroCosto = ordenCompra.CentroDeCosto
                idProveedor = ordenCompra.ProveedorId
                lblValor.Text = ordenCompra.Subtotal
                nombreJBIJBE = ordenCompra.JobBookNombre
                tipoDetalle = ordenCompra.TipoDetalle
            Case ETipoOrden.Requerimiento
                idJBIJBE = ordenRequerimiento.JobBook
                idCentroCosto = ordenRequerimiento.CentroDeCosto
                idProveedor = ordenRequerimiento.ProveedorId
                lblValor.Text = ordenRequerimiento.Subtotal
                nombreJBIJBE = ordenRequerimiento.JobBookNombre
                tipoDetalle = ordenRequerimiento.TipoDetalle
        End Select

        Select Case tipoDetalle
            Case ETipo.JBE, ETipo.JBI
                lblJBIJBECCNombre.Text = nombreJBIJBE
                lblIdJBIJBECC.Text = idJBIJBE
            Case ETipo.CentroCosto
                centroCosto = obtenerCentroCosto(idCentroCosto)
                lblIdJBIJBECC.Text = centroCosto.id
                lblJBIJBECCNombre.Text = centroCosto.CentroDeCosto
        End Select
        Contratista = obtenerContratistaXid(idProveedor)
        lblNombreProveedor.Text = Contratista.Nombre
        lblAsunto.Text &= [Enum].GetName(ETipoOrden.Servicio.GetType, tipoOrden)
    End Sub
    Function obtenerUsuariosNotificacion() As List(Of String)
        Dim daOrdenes As New FI.Ordenes
        Dim lstUsuarios As New List(Of String)

        Select Case tipoOrden
            Case ETipoOrden.Servicio
                lstUsuarios.AddRange(daOrdenes.ObtenerLogAprobacionesOrdenServicio(ordenServicio.id).Where(Function(x) x.FechaAprobacion Is Nothing).Select(Function(x) x.Email).Distinct().ToList)
            Case ETipoOrden.Compra
                lstUsuarios.AddRange(daOrdenes.ObtenerLogAprobacionesOrdenCompra(ordenCompra.id).Where(Function(x) x.FechaAprobacion Is Nothing).Select(Function(x) x.Email).Distinct().ToList)
            Case ETipoOrden.Requerimiento
                lstUsuarios.AddRange(daOrdenes.ObtenerLogAprobacionesOrdenRequerimiento(ordenRequerimiento.id).Where(Function(x) x.FechaAprobacion Is Nothing).Select(Function(x) x.Email).Distinct().ToList)
        End Select
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
    Function obtenerTrabajoXId(ByVal id As Int64) As PY_Trabajo_Get_Result
        Dim oPY_Trabajos_Get_Result As New Trabajo
        Return oPY_Trabajos_Get_Result.DevolverxID(id)
    End Function
    Private Function obtenerFacturaXId(idFactura As Long) As CC_RecepcionCuentasdeCobroGetXId_Result
        Dim oPI As New ProcesosInternos
        Return oPI.RecepcionCuentasGetXId(idFactura).FirstOrDefault()
    End Function
    Private Function obtenerProyectoXId(idProyecto As Long) As PY_Proyectos_Get_Result
        Dim da As New Proyecto
        Return da.obtenerXId(idProyecto)
    End Function
    Private Function obtenerCentroCosto(ByVal id As Int64) As FI_CentroCosto_Get_Result
        Dim daOrden As New FI.Ordenes
        Return daOrden.obtenerCentroCostoXId(id)
    End Function
#End Region
End Class