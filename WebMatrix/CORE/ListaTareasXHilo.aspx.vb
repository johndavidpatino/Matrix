Imports CoreProject
Public Class ListaTareasXHilo
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

#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.QueryString("idTrabajo") Is Nothing Then
            Int64.TryParse(Request.QueryString("idTrabajo"), idTrabajo)
        End If
        lbtnVolver.PostBackUrl = Request.UrlReferrer.PathAndQuery
        If Not IsPostBack Then
            If idTrabajo > 0 Then
                CargarTareasXTrabajo(idTrabajo)
            End If
        End If
    End Sub
    Private Sub gvTareasEnCurso_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTareasEnCurso.PageIndexChanging
        gvTareasEnCurso.PageIndex = e.NewPageIndex
        CargarTareasXTrabajo(idTrabajo)
    End Sub
    Private Sub gvTareasEnCurso_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareasEnCurso.RowCommand
        If e.CommandName = "Observaciones" Then
            hfWorkFlowId.Value = gvTareasEnCurso.DataKeys(e.CommandArgument)("Id")
            cargarObservaciones(hfWorkFlowId.Value)
        End If
    End Sub

    Private Sub gvObservaciones_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvObservaciones.PageIndexChanging
        gvObservaciones.PageIndex = e.NewPageIndex
        cargarObservaciones(hfWorkFlowId.Value)
    End Sub
#End Region
#Region "Metodos"
    Sub CargarTareasXTrabajo(ByVal idTrabajo As Int64)
        Dim oWorkFlow As New WorkFlow
        gvTareasEnCurso.DataSource = oWorkFlow.obtenerXIdTrabajo(idTrabajo)
        gvTareasEnCurso.DataBind()
    End Sub
    Sub cargarObservaciones(ByVal workFlow_Id As Int64)
        Dim oWorkFlow As New WorkFlow
        gvObservaciones.DataSource = oWorkFlow.obtenerObservacionesXTarea(workFlow_Id)
        gvObservaciones.DataBind()
        upObservaciones.Update()
    End Sub
#End Region


End Class