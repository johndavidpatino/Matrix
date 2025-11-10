Imports CoreProject
Public Class Configuracion_Tareas_Previas
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Private _idTarea As Int64
    Public Property idTarea() As Int64
        Get
            Return _idTarea
        End Get
        Set(ByVal value As Int64)
            _idTarea = value
        End Set
    End Property

    Private _idTipoHiloId As Int64
    Public Property IdTipoHilo() As Int64
        Get
            Return _idTipoHiloId
        End Get
        Set(ByVal value As Int64)
            _idTipoHiloId = value
        End Set
    End Property

#End Region

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("IdTarea") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdTarea"), idTarea)
        End If
        If Request.QueryString("IdTipoHilo") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdTipoHilo"), IdTipoHilo)
        End If
        lnkVolver.PostBackUrl = "ConfiguracionTareasXHilo.aspx?TipoHiloId=" & IdTipoHilo
        If Not IsPostBack Then
            Dim oTiposHilos As New TiposHilos
            Dim oTarea As New CoreProject.Tarea
            cargarTareasPrevias(gvTareasAsignadas, idTarea, IdTipoHilo, True)
            cargarTareasPrevias(gvTareasNoAsignadas, idTarea, IdTipoHilo, False)
            lblTipoHilo.Text = oTiposHilos.obtenerXId(IdTipoHilo).Hilo
            lblNombreTarea.Text = oTarea.obtenerXId(idTarea).Tarea
        End If
    End Sub
    Private Sub gvTareasAsignadas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTareasAsignadas.PageIndexChanging
        gvTareasAsignadas.PageIndex = e.NewPageIndex
        cargarTareasPrevias(gvTareasAsignadas, idTarea, IdTipoHilo, True)
    End Sub
    Private Sub gvTareasNoAsignadas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTareasNoAsignadas.PageIndexChanging
        gvTareasNoAsignadas.PageIndex = e.NewPageIndex
        cargarTareasPrevias(gvTareasNoAsignadas, idTarea, IdTipoHilo, False)
    End Sub
    Private Sub gvTareasNoAsignadas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareasNoAsignadas.RowCommand
        If e.CommandName = "Adicionar" Then
            Dim oTareasPrevias As New TareasPrevias
            oTareasPrevias.grabar(idTarea, IdTipoHilo, gvTareasNoAsignadas.DataKeys(e.CommandArgument)("Id"))
            cargarTareasPrevias(gvTareasAsignadas, idTarea, IdTipoHilo, True)
            cargarTareasPrevias(gvTareasNoAsignadas, idTarea, IdTipoHilo, False)
        End If
    End Sub

    Private Sub gvTareasAsignadas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareasAsignadas.RowCommand
        If e.CommandName = "Quitar" Then
            Dim oTareasPrevias As New TareasPrevias
            oTareasPrevias.Eliminar(gvTareasAsignadas.DataKeys(e.CommandArgument)("CORE_TareasPrevias_Id"))
            cargarTareasPrevias(gvTareasAsignadas, idTarea, IdTipoHilo, True)
            cargarTareasPrevias(gvTareasNoAsignadas, idTarea, IdTipoHilo, False)
        End If
    End Sub
#End Region

#Region "Metodos"
    Sub cargarTareasPrevias(ByRef gv As GridView, ByVal idTarea As Int64, ByVal idTipoHiloId As Int64, ByVal Asignada As Boolean)
        Dim oTareasPrevias As New TareasPrevias
        gv.DataSource = oTareasPrevias.CORE_Configuracion_Tareas_Previas(idTarea, idTipoHiloId, Asignada)
        gv.DataBind()
    End Sub

#End Region


End Class