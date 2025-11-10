Imports CoreProject
Public Class ListaDocumentosXHilos
    Inherits System.Web.UI.Page
#Region "Propiedades"
    Private _idTrabajo As Int64
    Public Property idTrabajo() As Int64
        Get
            Return _idTrabajo
        End Get
        Set(ByVal value As Int64)
            _idTrabajo = value
        End Set
    End Property

    Private _URLRetorno As UrlOriginal
    Public Property URLRetorno() As UrlOriginal
        Get
            Return _URLRetorno
        End Get
        Set(ByVal value As UrlOriginal)
            _URLRetorno = value
        End Set
    End Property

    Private _IdProyecto As Int64
    Public Property IdProyecto() As Int64
        Get
            Return _IdProyecto
        End Get
        Set(ByVal value As Int64)
            _IdProyecto = value
        End Set
    End Property

#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.QueryString("IdTrabajo") Is Nothing Then
            Int64.TryParse(Request.QueryString("IdTrabajo"), idTrabajo)
        End If
        If Request.QueryString("URLRetorno") IsNot Nothing Then
            Long.TryParse(Request.QueryString("URLRetorno"), URLRetorno)
        End If
        If Request.QueryString("IdProyecto") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdProyecto"), IdProyecto)
        End If
        asignarURLDevolucion()
        If Not IsPostBack Then
            If idTrabajo > 0 Then
                cargarDocumentosXIdTrabajo()
            End If
        End If
    End Sub
    Private Sub gvTareasXDocumentos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareasXDocumentos.RowCommand
        If e.CommandName = "Archivos" Then
            Response.Redirect("..\GD_Documentos\GD_Documentos.aspx" & "?IdContenedor=" & idTrabajo & "&IdDocumento=" & gvTareasXDocumentos.DataKeys(CInt(e.CommandArgument))("IdDocumento") & "&TipoContenedor=2&TipoAccion=2")
        End If
    End Sub
    Private Sub gvTareasXDocumentos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTareasXDocumentos.PageIndexChanging
        gvTareasXDocumentos.PageIndex = e.NewPageIndex
        cargarDocumentosXIdTrabajo()
    End Sub
#End Region
#Region "Metodos"
    Sub cargarDocumentosXIdTrabajo()
        Dim o As New Tareas_Documentos
        gvTareasXDocumentos.DataSource = o.obtenerDocumentosXHilo(idTrabajo)
        gvTareasXDocumentos.DataBind()
    End Sub
    Sub asignarURLDevolucion()
        Select Case URLRetorno
            Case UrlOriginal.OP_Cuantitativo_Trabajos
                lbtnVolver.PostBackUrl = "~/OP_Cuantitativo/Trabajos.aspx"
            Case UrlOriginal.PY_Proyectos_Trabajos
                lbtnVolver.PostBackUrl = "~/PY_Proyectos/Trabajos.aspx?proyectoId=" & IdProyecto & "&trabajoId=" & IdTrabajo
            Case UrlOriginal.OP_Cualitativo_Trabajos
                lbtnVolver.PostBackUrl = "~/OP_Cualitativo/Trabajos.aspx"
            Case UrlOriginal.OP_Cualitativo_TrabajosCoordinador
                lbtnVolver.PostBackUrl = "~/OP_Cualitativo/TrabajosCoordinador.aspx?IdTrabajo=" & idTrabajo
            Case UrlOriginal.RE_GT_TraficoTareas_Scripting
                lbtnVolver.PostBackUrl = "~/RE_GT/TraficoTareas.aspx?UnidadId=11&RolId=28&URLRetorno=5"
            Case UrlOriginal.RE_GT_TraficoTareas_Pilotos
                lbtnVolver.PostBackUrl = "~/RE_GT/TraficoTareas.aspx?UnidadId=12&RolId=30&URLRetorno=6"
            Case UrlOriginal.RE_GT_TraficoTareas_Critica
                lbtnVolver.PostBackUrl = "~/RE_GT/TraficoTareas.aspx?UnidadId=5&RolId=22&URLRetorno=7"
            Case UrlOriginal.RE_GT_TraficoTareas_Verificacion
                lbtnVolver.PostBackUrl = "~/RE_GT/TraficoTareas.aspx?UnidadId=6&RolId=23&URLRetorno=8"
            Case UrlOriginal.RE_GT_TraficoTareas_Captura
                lbtnVolver.PostBackUrl = "~/RE_GT/TraficoTareas.aspx?UnidadId=7&RolId=24&URLRetorno=9"
            Case UrlOriginal.RE_GT_TraficoTareas_Codificacion
                lbtnVolver.PostBackUrl = "~/RE_GT/TraficoTareas.aspx?UnidadId=8&RolId=25&URLRetorno=10"
            Case UrlOriginal.RE_GT_TraficoTareas_Datacleaning
                lbtnVolver.PostBackUrl = "~/RE_GT/TraficoTareas.aspx?UnidadId=9&RolId=27&URLRetorno=11"
            Case UrlOriginal.RE_GT_TraficoTareas_Procesamiento
                lbtnVolver.PostBackUrl = "~/RE_GT/TraficoTareas.aspx?UnidadId=10&RolId=26&URLRetorno=12"
            Case UrlOriginal.RE_GT_TraficoTareas_Estadistica
                lbtnVolver.PostBackUrl = "~/RE_GT/TraficoTareas.aspx?UnidadId=10&RolId=33&URLRetorno=15"
        End Select
    End Sub
#End Region
End Class