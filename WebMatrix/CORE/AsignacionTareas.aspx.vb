Imports WebMatrix
Imports CoreProject
Imports WebMatrix.Util
Public Class AsignacionTareas
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
    Public Property UnidadEjecutaid() As Int64
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
        If Request.QueryString("IdUnidadEjecuta") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdUnidadEjecuta"), UnidadEjecutaid)
        End If

        If lbtnVolver.PostBackUrl = "" Then
            lbtnVolver.PostBackUrl = Request.UrlReferrer.PathAndQuery
        End If

        If Not IsPostBack Then
            If trabajoId > 0 AndAlso UnidadEjecutaid > 0 Then
                cargarTareasXUnidadEjecuta()
            End If
        End If
    End Sub
    Private Sub gvTareas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTareas.PageIndexChanging
        gvTareas.PageIndex = e.NewPageIndex
        cargarTareasXUnidadEjecuta()
    End Sub
    Protected Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnVolver.Click
        If UnidadEjecutaid = UnidadesCore.Proyectos Then
            Dim oTrabajos As New Trabajo
            Dim pyid As Int64 = oTrabajos.obtenerXId(trabajoId).ProyectoId
            Response.Redirect("../PY_Proyectos/Trabajos.aspx?ProyectoId=" & pyid)
        End If
    End Sub
    Private Sub gvTareas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareas.RowCommand
        If e.CommandName = "Asignar" Then
            Dim oTarea As New CoreProject.Tarea
            Dim RolId As Integer
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            Me.hfidWorkFlow.Value = Me.gvTareas.DataKeys(CInt(e.CommandArgument))("Id")
            Me.hfidTarea.Value = gvTareas.DataKeys(e.CommandArgument)("TareaId")
            RolId = oTarea.obtenerXId(hfidTarea.Value).RolEjecuta
            cargarUsuariosAsignados(Me.hfidWorkFlow.Value, RolId)
            CargarUsuariosDisponibles(RolId, hfidWorkFlow.Value)
        End If
    End Sub
    Protected Sub btnAdicionarUsuario_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdicionarUsuario.Click
        Dim oWorkFlow As New WorkFlow
        Dim oWorkFlow_UsuariosAsignados As New WorkFlow_UsuariosAsignados
        Dim oLogWF As New LogWorkFlow
        Dim oTarea As New CoreProject.Tarea
        Dim RolId As Integer
        Dim oEnviarCorreo As New EnviarCorreo

        oWorkFlow_UsuariosAsignados.grabar(hfidWorkFlow.Value, ddlUsuariosDisponibles.SelectedValue, DateTime.Now)
        oLogWF.CORE_Log_WorkFlow_Add(hfidWorkFlow.Value, Now, Session("IDUsuario").ToString, LogWorkFlow.WorkFlowEstados.Asignada, Nothing)
        RolId = oTarea.obtenerXId(hfidTarea.Value).RolEjecuta
		oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/TareaAsignacion.aspx?IdTrabajo=" & trabajoId & "&IdWorkFlow=" & hfidWorkFlow.Value & "&IdTipoAsignacion=1&IdUsuarioNotificar=" & ddlUsuariosDisponibles.SelectedValue)

		cargarUsuariosAsignados(hfidWorkFlow.Value, RolId)
		CargarUsuariosDisponibles(RolId, hfidWorkFlow.Value)
		cargarTareasXUnidadEjecuta()

		ShowNotification("Responsable asignado correctamente", ShowNotifications.InfoNotification)
		ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
	End Sub
	Private Sub gvUsuariosAsignados_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvUsuariosAsignados.RowCommand
		Dim oWorkFlow_UsuariosAsignados As New WorkFlow_UsuariosAsignados
		Dim oTarea As New CoreProject.Tarea
		Dim RolId As Integer
		Dim oEnviarCorreo As New EnviarCorreo

		oWorkFlow_UsuariosAsignados.eliminar(gvUsuariosAsignados.DataKeys(e.CommandArgument)("CORE_WorkFlow_UsuariosAsignados_Id"))
		RolId = oTarea.obtenerXId(hfidTarea.Value).RolEjecuta
		oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/TareaAsignacion.aspx?IdTrabajo=" & trabajoId & "&IdWorkFlow=" & hfidWorkFlow.Value & "&IdTipoAsignacion=2&IdUsuarioNotificar=" & gvUsuariosAsignados.DataKeys(e.CommandArgument)("UsuarioId"))

		cargarUsuariosAsignados(Me.hfidWorkFlow.Value, RolId)
        CargarUsuariosDisponibles(RolId, hfidWorkFlow.Value)
        cargarTareasXUnidadEjecuta()



        ShowNotification("Usuario excluido correctamente", ShowNotifications.InfoNotification)
    End Sub
    Private Sub gvTareas_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvTareas.RowEditing
        gvTareas.EditIndex = e.NewEditIndex
        cargarTareasXUnidadEjecuta()
    End Sub
    Private Sub gvTareas_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvTareas.RowUpdating
        Dim oPlaneacion As New Planeacion
        Dim o As New WorkFlow
        oPlaneacion.grabar(e.Keys(0), CType(gvTareas.Rows(e.RowIndex).FindControl("txtFIniP"), TextBox).Text, CType(gvTareas.Rows(e.RowIndex).FindControl("txtFFinP"), TextBox).Text, idUsuario, DateTime.UtcNow.AddHours(-5), CType(gvTareas.Rows(e.RowIndex).FindControl("txtObservacion"), TextBox).Text)
        o.Editar(e.Keys(0), CType(gvTareas.Rows(e.RowIndex).FindControl("txtFIniP"), TextBox).Text, CType(gvTareas.Rows(e.RowIndex).FindControl("txtFFinP"), TextBox).Text, Nothing, Nothing, Nothing, Nothing, Nothing)

        gvTareas.EditIndex = -1
        cargarTareasXUnidadEjecuta()

    End Sub
#End Region
#Region "Metodos"
    Sub cargarTareasXUnidadEjecuta()
        Dim oWorkFlow As New WorkFlow
        gvTareas.DataSource = oWorkFlow.obtenerXUnidadEjecutaXTrabajoId(UnidadEjecutaid, trabajoId)
        gvTareas.DataBind()
    End Sub
    Sub cargarUsuariosAsignados(ByVal workFlowId As Int64, ByVal rolId As Int64)
        Dim oWorkFlow_UsuariosAsignados As New WorkFlow_UsuariosAsignados
        gvUsuariosAsignados.DataSource = oWorkFlow_UsuariosAsignados.obtenerUsuariosXWorkFlowIdXEstadoXRolId(workFlowId, True, rolId)
        gvUsuariosAsignados.DataBind()
        upUsuariosAsignados.Update()
    End Sub
    Sub CargarUsuariosDisponibles(ByVal rolid As Integer, ByVal WorkFlowId As Integer)
        Try
            Dim oUsuarios As New US.Usuarios


            Dim WorkFlow_UsuariosAsignados As New WorkFlow_UsuariosAsignados

            ddlUsuariosDisponibles.DataSource = WorkFlow_UsuariosAsignados.obtenerUsuariosXWorkFlowIdXEstadoXRolId(WorkFlowId, False, rolid)
            ddlUsuariosDisponibles.DataValueField = "Id"
            ddlUsuariosDisponibles.DataTextField = "Nombre"
            ddlUsuariosDisponibles.DataBind()

            ddlUsuariosDisponibles.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})

            upUsuariosAsignados.Update()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region


End Class