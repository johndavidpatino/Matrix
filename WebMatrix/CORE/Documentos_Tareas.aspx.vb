Imports WebMatrix.Util
Imports CoreProject

Public Class Documentos_Tareas
    Inherits System.Web.UI.Page

#Region "Propiedades"

    Private _IdUsuario As Int64
    Public Property IdUsuario() As Int64
        Get
            Return _IdUsuario
        End Get
        Set(ByVal value As Int64)
            _IdUsuario = value
        End Set
    End Property
    Private _IdTarea As Int64
    Public Property IdTarea() As Int64
        Get
            Return _IdTarea
        End Get
        Set(ByVal value As Int64)
            _IdTarea = value
        End Set
    End Property


    Private _IdTrabajo As Int64
    Public Property IdTrabajo() As Int64
        Get
            Return _IdTrabajo
        End Get
        Set(ByVal value As Int64)
            _IdTrabajo = value
        End Set
    End Property

    Private _IdContenedor As Int64
    Public Property IdContenedor() As Int64
        Get
            Return _IdContenedor
        End Get
        Set(ByVal value As Int64)
            _IdContenedor = value
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

    Private _idWorkFlow As Int64
    Public Property idWorkFlow() As Int64
        Get
            Return _idWorkFlow
        End Get
        Set(ByVal value As Int64)
            _idWorkFlow = value
        End Set
    End Property

    Private _URLRetorno As Int16
    Public Property URLRetorno() As Int16
        Get
            Return _URLRetorno
        End Get
        Set(ByVal value As Int16)
            _URLRetorno = value
        End Set
    End Property
#End Region

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        IdUsuario = Session("IdUsuario")

        If Request.QueryString("IdTarea") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdTarea"), IdTarea)
        End If

        If Request.QueryString("IdContenedor") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdContenedor"), IdContenedor)
        End If

        If Request.QueryString("IdTrabajo") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdTrabajo"), IdTrabajo)
        End If

        If Request.QueryString("IdProyecto") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdProyecto"), IdProyecto)
        End If

        If Request.QueryString("IdWorkFlow") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdWorkFlow"), idWorkFlow)
        End If

        If Request.QueryString("URLRetorno") IsNot Nothing Then
            Long.TryParse(Request.QueryString("URLRetorno"), URLRetorno)
        End If

        lblNombreTarea.Text = obtenerTareaXId(IdTarea).Tarea
        lblNombreTrabajo.Text = obtenerTrabajoXId(IdTrabajo).NombreTrabajo
        lnkTarea.PostBackUrl = "Tarea.aspx?IdTrabajo=" & IdTrabajo & "&IdProyecto=" & IdProyecto & "&IdTarea=" & IdTarea & "&IdWorkFlow=" & idWorkFlow & "&URLRetorno=" & URLRetorno

        ActivateAccordion(1, EffectActivateAccordion.NoEffect)

        If Not IsPostBack Then
            If IdTarea > 0 AndAlso IdContenedor > 0 Then
                cargarArchivosRequeridosXIdTarea(IdTarea, IdContenedor)
                cargarArchivosEntregablesXIdTarea(IdTarea, IdContenedor)
            End If
        End If
    End Sub
    Private Sub gvArchivosEntregables_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvArchivosEntregables.RowCommand
        If e.CommandName = "Archivos" Then
            Response.Redirect("..\GD_Documentos\GD_Documentos.aspx" & "?IdContenedor=" & IdContenedor & "&IdDocumento=" & gvArchivosEntregables.DataKeys(CInt(e.CommandArgument))("IdDocumento") & "&TipoContenedor=1" & "&URLRetorno=" & URLRetorno & "&TipoAccion=1" & "&IdWorkFlow=" & idWorkFlow & "&a=1")
        End If
    End Sub
    Private Sub gvArchivosRequeridos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvArchivosRequeridos.RowCommand
        If e.CommandName = "Archivos" Then
            Response.Redirect("..\GD_Documentos\GD_Documentos.aspx" & "?IdContenedor=" & IdContenedor & "&IdDocumento=" & gvArchivosRequeridos.DataKeys(CInt(e.CommandArgument))("IdDocumento") & "&TipoContenedor=2" & "&URLRetorno=" & URLRetorno & "&TipoAccion=2" & "&IdWorkFlow=" & idWorkFlow & "")
        End If
    End Sub
#End Region

#Region "Metodos"
    Function obtenerArchivosXIdTarea(ByVal idTarea As Int64, ByVal tipoDocumento As Int16, ByVal idHilo As Int64) As List(Of CORE_DocumentosRequeridosXTarea_Get_Result)
        Dim o As New Tareas_Documentos
        Return o.obtenerDocumentosXTarea(idTarea, tipoDocumento, idHilo)
    End Function

    Sub cargarArchivosRequeridosXIdTarea(ByVal idTarea As Int64, ByVal idHilo As Int64)
        gvArchivosRequeridos.DataSource = obtenerArchivosXIdTarea(idTarea, 1, idHilo)
        gvArchivosRequeridos.DataBind()
    End Sub

    Sub cargarArchivosEntregablesXIdTarea(ByVal idTarea As Int64, ByVal idHilo As Int64)
        gvArchivosEntregables.DataSource = obtenerArchivosXIdTarea(idTarea, 2, idHilo)
        gvArchivosEntregables.DataBind()
    End Sub
    Function obtenerTareaXId(ByVal id As Int64) As CORE_Tareas_Get_Result
        Dim oTarea As New CoreProject.Tarea
        Return oTarea.obtenerXId(id)
    End Function

    Function obtenerTrabajoXId(ByVal id As Int64) As PY_Trabajos_Get_Result
        Dim oTrabajo As New CoreProject.Trabajo
        Return oTrabajo.obtenerXId(id)
    End Function

#End Region


End Class