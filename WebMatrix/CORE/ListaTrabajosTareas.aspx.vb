Imports CoreProject

Public Class ListaTrabajosTareas
    Inherits System.Web.UI.Page


    Private _idUsuario As Int64
    Public Property idUsuario() As Int64
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Int64)
            _idUsuario = value
        End Set
    End Property
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        idUsuario = Session("IDUsuario")
        If Not IsPostBack Then
            cargarTareasXTrabajo()
            lbtnVolver.PostBackUrl = "~\Home.aspx"
        End If
    End Sub
    Private Sub gvTrabajosTareas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajosTareas.PageIndexChanging
        gvTrabajosTareas.PageIndex = e.NewPageIndex
        cargarTareasXTrabajo()
    End Sub
    Private Sub gvTrabajosTareas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajosTareas.RowCommand
        If e.CommandName = "Ver" Then
            Dim oTrabajo As New Trabajo
            Dim oePY_Trabajo_Get_Result As PY_Trabajo_Get_Result
            oePY_Trabajo_Get_Result = oTrabajo.DevolverxID(gvTrabajosTareas.DataKeys(e.CommandArgument)("Id"))
            Response.Redirect("Tareas.aspx?IdTrabajo=" & oePY_Trabajo_Get_Result.id & "&IdProyecto=" & oePY_Trabajo_Get_Result.ProyectoId & "&URLRetorno=" & UrlOriginal.CORE_ListaTrabajosTareas)
        End If
    End Sub
#End Region
#Region "Metodos"
    Sub cargarTareasXTrabajo()
        Dim oLogWorkFlow As New LogWorkFlow
        gvTrabajosTareas.DataSource = oLogWorkFlow.obtenerCantidadTareasXTrabajo(idUsuario)
        gvTrabajosTareas.DataBind()
    End Sub
#End Region
End Class