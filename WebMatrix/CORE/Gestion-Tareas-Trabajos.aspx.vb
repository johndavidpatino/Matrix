Imports CoreProject
Imports WebMatrix.Util
Public Class Gestion_Tareas_Trabajos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.QueryString("idTrabajo") Is Nothing Then
            hfIdTrabajo.Value = CDbl(Request.QueryString("idTrabajo"))
        End If
        If Request.QueryString("URLRetorno") IsNot Nothing Then
            hfUrlRetorno.Value = Request.QueryString("URLRetorno")
        End If
        If Request.QueryString("IdUnidadEjecuta") IsNot Nothing Then
            hfUnidadEjecuta.Value = Request.QueryString("IdUnidadEjecuta")
        End If
        If Request.QueryString("IdRolEstima") IsNot Nothing Then
            hfRolEstima.Value = Request.QueryString("IdRolEstima")
        End If
        'hfIdTrabajo.Value = 374
        'hfRolEjecuta.Value = 7
        'hfRolEstima.Value = 6
        'hfUnidadEjecuta.Value = UnidadesCore.Proyectos
        If Not IsPostBack Then
            CargarUnidadesEjecucion()
        End If

    End Sub
    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
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

    Protected Sub btnAdicionarUsuario_Click(sender As Object, e As EventArgs) Handles btnAdicionarUsuario.Click
        Dim oLogWorkFlow As New LogWorkFlow
        Dim oGt As New CT_Tareas
        Dim oWorkFlow As New WorkFlow
        Dim oHilo As New CoreProject.Hilo
        Dim info = oGt.ObtenerWorkFlow(hfWfIdAsignacion.Value)
        info.UsuarioAsignado = ddlUsuariosDisponibles.SelectedValue
        info.Estado = LogWorkFlow.WorkFlowEstados.Asignada
        oGt.GuardarWorkFlow(info)
        Dim oWorkFlow_UsuariosAsignados As New WorkFlow_UsuariosAsignados
        Dim oLogWF As New LogWorkFlow
        Dim oTarea As New CoreProject.Tarea
        Dim RolId As Integer
        Dim oEnviarCorreo As New EnviarCorreo

        oWorkFlow_UsuariosAsignados.grabar(hfWfIdAsignacion.Value, ddlUsuariosDisponibles.SelectedValue, DateTime.Now)
        oLogWF.CORE_Log_WorkFlow_Add(hfWfIdAsignacion.Value, Now, Session("IDUsuario").ToString, LogWorkFlow.WorkFlowEstados.Asignada, Nothing)
        RolId = oTarea.obtenerXId(hfWfTareaAsignacion.Value).RolEjecuta
        oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/TareaAsignacion.aspx?IdTrabajo=" & oHilo.obtenerXId(info.HiloId).ContenedorId & "&IdWorkFlow=" & hfWfIdAsignacion.Value & "&IdTipoAsignacion=1&IdUsuarioNotificar=" & ddlUsuariosDisponibles.SelectedValue)
        CargarTareasAsignacion()
        Me.upUsuariosAsignados.Update()
    End Sub



    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Me.pnlTrabajos.Visible = True
        Me.pnlAsignaciones.Visible = False
        Me.pnlCronograma.Visible = False
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs) Handles LinkButton2.Click
        Me.pnlTrabajos.Visible = False
        Me.pnlAsignaciones.Visible = True
        Me.pnlCronograma.Visible = False
    End Sub

    Protected Sub ddEstado_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddEstado.SelectedIndexChanged
        CargarTareasXUsuario()
    End Sub

    Sub CargarTareasXUsuario()
        Dim o As New CT_Tareas
        Dim ListaTareas = o.TareasList(Nothing, Nothing, Nothing, Nothing, Nothing, Session("IDUsuario").ToString, ddEstado.SelectedValue, Nothing, Nothing, Nothing, Nothing)
        ListaTareas = ListaTareas.OrderByDescending(Function(tarea) tarea.FIniP).ToList()
        Me.gvTrabajos.DataSource = ListaTareas
        Me.gvTrabajos.DataBind()
    End Sub

    Sub CargarTareasAsignacion()
        Dim o As New CT_Tareas
        Me.gvAsignaciones.DataSource = o.TareasPendientesAsignacion(Nothing, Nothing, Nothing, ddUnidad.SelectedValue, Nothing, Nothing, Nothing)
        Me.gvAsignaciones.DataBind()
    End Sub

    Sub CargarTareasCronograma()
        Dim o As New CT_Tareas
        Dim Fini As Date? = Nothing
        Dim Ffin As Date? = Nothing
        If IsDate(txtFInicioP.Text) Then Fini = txtFInicioP.Text
        If IsDate(txtFFinP.Text) Then Ffin = txtFFinP.Text
        Dim li = (From list In o.TareasList(Nothing, Nothing, Nothing, ddUnidadCronogramaGeneral.SelectedValue, LogWorkFlow.WorkFlowEstados.NoAplica, Nothing, Nothing, Fini, Ffin, Nothing, Nothing)
                Order By list.FFinP Descending)
        Me.gvCronograma.DataSource = li
        Me.gvCronograma.DataBind()
    End Sub

    Sub CargarUnidadesEjecucion()
        Dim o As New CT_Tareas
        Me.ddUnidad.DataSource = o.UnidadesAsignacion(Session("IDUsuario").ToString)
        Me.ddUnidad.DataValueField = "id"
        Me.ddUnidad.DataTextField = "Unidad"
        Me.ddUnidad.DataBind()
        Me.ddUnidad.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})

        Me.ddUnidadCronogramaGeneral.DataSource = o.UnidadesAsignacion(Session("IDUsuario").ToString)
        Me.ddUnidadCronogramaGeneral.DataValueField = "id"
        Me.ddUnidadCronogramaGeneral.DataTextField = "Unidad"
        Me.ddUnidadCronogramaGeneral.DataBind()
        Me.ddUnidadCronogramaGeneral.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub

    Private Sub gvAsignaciones_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAsignaciones.RowCommand
        If e.CommandName = "Info" Then
            Dim idt As Int64 = Int64.Parse(Me.gvAsignaciones.DataKeys(CInt(e.CommandArgument))("Id"))
            hfWfIdAsignacion.Value = idt
            hfWfTareaAsignacion.Value = Int64.Parse(Me.gvAsignaciones.DataKeys(CInt(e.CommandArgument))("TareaId"))
            hfRolEjecuta.Value = Int64.Parse(Me.gvAsignaciones.DataKeys(CInt(e.CommandArgument))("RolEjecuta"))
            CargarUsuariosDisponibles(hfRolEjecuta.Value, hfWfIdAsignacion.Value)
        End If
    End Sub

    Private Sub gvTrabajos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        Dim idt As Int64 = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("IdTrabajo"))
        Dim oTrabajo As New Trabajo
        Dim oeTrabajo = oTrabajo.ObtenerTrabajo(idt)
        Session("NombreTrabajo") = oeTrabajo.id.ToString & " | " & oeTrabajo.JobBook & " | " & oeTrabajo.NombreTrabajo
        Response.Redirect("../CORE/Gestion-Tareas.aspx?IdTrabajo=" & idt & "&URLRetorno=" & UrlOriginal.CORE_ListaTrabajosTareas)
    End Sub

    Protected Sub ddUnidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddUnidad.SelectedIndexChanged
        CargarTareasAsignacion()
    End Sub

    Private Sub gvAsignaciones_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAsignaciones.PageIndexChanging
        gvAsignaciones.PageIndex = e.NewPageIndex
        If Me.pnlAsignaciones.Visible = True Then
            CargarTareasAsignacion()
        Else
            CargarTareasXUsuario()
        End If
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub btnFiltrarCronograma_Click(sender As Object, e As System.EventArgs) Handles btnFiltrarCronograma.Click
        CargarTareasCronograma()
    End Sub

    Private Sub LinkButton3_Click(sender As Object, e As System.EventArgs) Handles LinkButton3.Click
        Me.pnlTrabajos.Visible = False
        Me.pnlAsignaciones.Visible = False
        Me.pnlCronograma.Visible = True
    End Sub
End Class