Imports WebMatrix
Imports CoreProject
Imports WebMatrix.Util
Public Class EstimacionTareas
    Inherits System.Web.UI.Page

#Region "Propiedades"

    Private _idUsuario As Int64
    Public Property idUsuario() As Int64
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Int64)
            _idUsuario = value
        End Set
    End Property

    Private _trabajoId As Int64
    Public Property trabajoId() As Int64
        Get
            Return _trabajoId
        End Get
        Set(ByVal value As Int64)
            _trabajoId = value
        End Set
    End Property

    Private _rolEstimaId As Int16
    Public Property rolEstimaId() As Int64
        Get
            Return _rolEstimaId
        End Get
        Set(ByVal value As Int64)
            _rolEstimaId = value
        End Set
    End Property

#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        idUsuario = Session("IDUsuario")
        If Request.QueryString("IdTrabajo") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdTrabajo"), trabajoId)
        End If
        If Request.QueryString("IdRolEstima") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdRolEstima"), rolEstimaId)
        End If

        If lbtnVolver.PostBackUrl = "" Then
            lbtnVolver.PostBackUrl = Request.UrlReferrer.PathAndQuery
        End If

        If Not IsPostBack Then
            If trabajoId > 0 AndAlso rolEstimaId > 0 Then
                cargarTareasXRoleEstima()
            End If
        End If
    End Sub
    Private Sub gvEstimacionTareas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvEstimacionTareas.PageIndexChanging
        gvEstimacionTareas.PageIndex = e.NewPageIndex
        cargarTareasXRoleEstima()
    End Sub
Private Sub gvEstimacionTareas_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvEstimacionTareas.RowCancelingEdit
        gvEstimacionTareas.EditIndex = -1
        cargarTareasXRoleEstima()
    End Sub
    Private Sub gvEstimacionTareas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEstimacionTareas.RowCommand
        If e.CommandName = "Historico" Then
            cargarHistoricoFechasXIdWorkFlow(gvEstimacionTareas.DataKeys(e.CommandArgument)("Id"))
        End If
    End Sub
    Private Sub gvEstimacionTareas_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvEstimacionTareas.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(0).Controls.Count > 1 Then
                CType(e.Row.Cells(0).Controls(2), LinkButton).Attributes.Add("Title", "Actualizar")
            Else
                CType(e.Row.Cells(0).Controls(0), LinkButton).Attributes.Add("Title", "Editar")
            End If
        End If
    End Sub
    Private Sub gvEstimacionTareas_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvEstimacionTareas.RowEditing
        gvEstimacionTareas.EditIndex = e.NewEditIndex
        cargarTareasXRoleEstima()
    End Sub
    Private Sub gvEstimacionTareas_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvEstimacionTareas.RowUpdating
        Dim oPlaneacion As New Planeacion
        Dim o As New WorkFlow
        oPlaneacion.grabar(e.Keys(0), CType(gvEstimacionTareas.Rows(e.RowIndex).FindControl("txtFIniP"), TextBox).Text, CType(gvEstimacionTareas.Rows(e.RowIndex).FindControl("txtFFinP"), TextBox).Text, idUsuario, DateTime.UtcNow.AddHours(-5), CType(gvEstimacionTareas.Rows(e.RowIndex).FindControl("txtObservacion"), TextBox).Text)
        o.Editar(e.Keys(0), CType(gvEstimacionTareas.Rows(e.RowIndex).FindControl("txtFIniP"), TextBox).Text, CType(gvEstimacionTareas.Rows(e.RowIndex).FindControl("txtFFinP"), TextBox).Text, Nothing, Nothing, Nothing, Nothing, Nothing)
        Try
            Dim oEnviarCorreo As New EnviarCorreo
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/TareaNotificacionFecha.aspx?idTrabajo=" & trabajoId & "&idTarea=" & e.Keys(2) & "&IdWorkFlow=" & e.Keys(0))
        Catch ex As Exception

        End Try
        gvEstimacionTareas.EditIndex = -1
        cargarTareasXRoleEstima()

        'HACK Implementar la funcionalidad de actualizar registro
    End Sub
    Sub chkAplica_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim checkbox As CheckBox = sender
        Dim row As GridViewRow = checkbox.NamingContainer
        Dim index As Integer = row.RowIndex
        Dim oLogWorkFlow As New LogWorkFlow
        If checkbox.Checked = False Then
            oLogWorkFlow.CORE_Log_WorkFlow_Add(gvEstimacionTareas.DataKeys(index)("Id"), DateTime.UtcNow.AddHours(-5), idUsuario, LogWorkFlow.WorkFlowEstados.NoAplica, Nothing)
        Else
            oLogWorkFlow.CORE_Log_WorkFlow_Add(gvEstimacionTareas.DataKeys(index)("Id"), DateTime.UtcNow.AddHours(-5), idUsuario, LogWorkFlow.WorkFlowEstados.Creada, Nothing)
        End If
    End Sub
    Protected Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnVolver.Click
        If rolEstimaId = 6 Then
            Dim oTrabajos As New Trabajo
            Dim pyid As Int64 = oTrabajos.obtenerXId(trabajoId).ProyectoId
            Response.Redirect("../PY_Proyectos/Trabajos.aspx?ProyectoId=" & pyid)
        End If
    End Sub
#End Region
#Region "Metodos"
    Sub cargarTareasXRoleEstima()
        Dim oWorkFlow As New WorkFlow
        gvEstimacionTareas.DataSource = oWorkFlow.obtenerXRoleEstimaXTrabajoId(rolEstimaId, trabajoId)
        gvEstimacionTareas.DataBind()
    End Sub
    Sub cargarHistoricoFechasXIdWorkFlow(ByVal Id As Int64)
        Dim oPlaneacion As New Planeacion
        gvHistoricoFechas.DataSource = oPlaneacion.obtenerXWorkFlowId(Id)
        gvHistoricoFechas.DataBind()
        upHistoricoFechas.Update()
    End Sub
#End Region
End Class