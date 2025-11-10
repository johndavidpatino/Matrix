Imports CoreProject
Imports System.IO

Public Class EvaluacionProveedorOP
    Inherits System.Web.UI.Page
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

    Private _Contratista As TH_Contratistas
    Public Property Contratista() As TH_Contratistas
        Get
            Return _Contratista
        End Get
        Set(ByVal value As TH_Contratistas)
            _Contratista = value
        End Set
    End Property
    Private _Factura As CC_RecepcionCuentasdeCobroGetXId_Result
    Public Property Factura() As CC_RecepcionCuentasdeCobroGetXId_Result
        Get
            Return _Factura
        End Get
        Set(ByVal value As CC_RecepcionCuentasdeCobroGetXId_Result)
            _Factura = value
        End Set
    End Property
    Private _ordenServicio As CC_OrdenesdeServicioGet_Result
    Public Property ordenServicio() As CC_OrdenesdeServicioGet_Result
        Get
            Return _ordenServicio
        End Get
        Set(ByVal value As CC_OrdenesdeServicioGet_Result)
            _ordenServicio = value
        End Set
    End Property


#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim idOrden As Int64
        If Not Request.QueryString("IdOrden") Is Nothing Then
            If Int64.TryParse(Request.QueryString("IdOrden"), idOrden) Then
                ordenServicio = obtenerOrdenServicioXId(idOrden)
                If ordenServicio.SolicitadoPor.HasValue = False Then
                    Exit Sub
                End If
                Trabajo = obtenerTrabajoXId(ordenServicio.TrabajoId)
                Contratista = obtenerContratistaXid(ordenServicio.ContratistaId)
				lnkRuta.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/CC_FinzOpe/Evaluacion-Facturas-Operaciones.aspx?Tipo=1&IdOrden=" & idOrden
			End If
        End If
        parametrizarMensaje()

        Dim page As Page = DirectCast(Context.Handler, Page)
        GenerarHtml()

    End Sub
#End Region
#Region "Metodos"
    
    Sub parametrizarMensaje()
        lblProveedor.Text = Contratista.Nombre
        lblTrabajoId.Text = Trabajo.id
        lblTrabajoNombre.Text = Trabajo.NombreTrabajo
    End Sub
    Function obtenerUsuariosNotificacion() As List(Of String)
        Dim oU As New US.Usuarios
        Dim u = oU.UsuarioGet(ordenServicio.SolicitadoPor)
        Dim lstUsuarios As New List(Of String)
        lstUsuarios.Add(u.Email)
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
    Function obtenerOrdenServicioXId(ByVal id As Int64) As CC_OrdenesdeServicioGet_Result
        Dim oPI As New ProcesosInternos
        Return oPI.OrdenesdeServicioGet(Nothing, id, Nothing).FirstOrDefault
    End Function
    Function obtenerTrabajoXId(ByVal id As Int64) As PY_Trabajo_Get_Result
        Dim oPY_Trabajos_Get_Result As New Trabajo
        Return oPY_Trabajos_Get_Result.DevolverxID(id)
    End Function
    Private Function obtenerFacturaXId(idFactura As Long) As CC_RecepcionCuentasdeCobroGetXId_Result
        Dim oPI As New ProcesosInternos
        Return oPI.RecepcionCuentasGetXId(idFactura).FirstOrDefault()
    End Function
#End Region
End Class