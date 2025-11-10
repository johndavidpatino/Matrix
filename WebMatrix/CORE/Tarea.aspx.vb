Imports WebMatrix
Imports WebMatrix.Util
Imports CoreProject
Imports System.Net
Imports System.IO

Public Class Tarea
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

    Private _idtrabajo As Int64
    Public Property idTrabajo() As Int64
        Get
            Return _idtrabajo
        End Get
        Set(ByVal value As Int64)
            _idtrabajo = value
        End Set
    End Property


    Private _idUsuario As Int64
    Public Property idusuario() As Int64
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Int64)
            _idUsuario = value
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


    Private _idProyecto As Int64
    Public Property idProyecto() As Int64
        Get
            Return _idProyecto
        End Get
        Set(ByVal value As Int64)
            _idProyecto = value
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
        If Request.QueryString("IdTarea") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdTarea"), idTarea)
        End If
        If Request.QueryString("IdTrabajo") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdTrabajo"), idTrabajo)
        End If
        If Request.QueryString("IdWorkFlow") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdWorkFlow"), idWorkFlow)
        End If
        If Request.QueryString("IdProyecto") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdProyecto"), idProyecto)
        End If

        If Request.QueryString("URLRetorno") IsNot Nothing Then
            Long.TryParse(Request.QueryString("URLRetorno"), URLRetorno)
        End If

        If idProyecto > 0 AndAlso idTrabajo > 0 Then
            lblNombreTrabajo.Text = obtenerTrabajoXId(idTrabajo).NombreTrabajo.ToUpper
            lbtnVolverTareasTrabajo.PostBackUrl = "Tareas.aspx" & "?IdProyecto=" & idProyecto & "&IdTrabajo=" & idTrabajo & "&URLRetorno=" & URLRetorno
            cargarTarea(_idWorkFlow)
        End If

        idusuario = Session("IdUsuario")
        If Not IsPostBack Then
            If idTarea > 0 AndAlso idTrabajo > 0 AndAlso idWorkFlow > 0 Then
                cargarTareasPrevias(idTarea, idTrabajo)

                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            End If
        End If
    End Sub
    Protected Sub btnArchivos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnArchivos.Click
        Response.Redirect("Documentos_Tareas.aspx?IdTarea=" & idTarea & "&IdContenedor=" & idTrabajo & "&IdTrabajo=" & idTrabajo & "&IdProyecto=" & idProyecto & "&IdWorkFlow=" & idWorkFlow & "&URLRetorno=" & URLRetorno)
    End Sub
    Protected Sub btnIniciarTarea_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnIniciarTarea.Click
        Dim oLogWorkFlow As New LogWorkFlow
        Dim oWorkFlow As New WorkFlow
        oLogWorkFlow.CORE_Log_WorkFlow_Add(idWorkFlow, DateTime.UtcNow.AddHours(-5), idusuario, LogWorkFlow.WorkFlowEstados.EnCurso, txtObservaciones.Text)
        oWorkFlow.Editar(idWorkFlow, Nothing, Nothing, DateTime.UtcNow.AddHours(-5), Nothing, Nothing, Nothing, Nothing)
        btnIniciarTarea.Enabled = False
        btnFinalizarTarea.Enabled = True
        cargarTarea(idWorkFlow)
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        'HACK Falta validar las opciones del inicio de la tarea, que todas las actividades previas hayan finalizado
    End Sub
    Protected Sub btnFinalizarTarea_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFinalizarTarea.Click
        Dim oLogWorkFlow As New LogWorkFlow
        Dim oWorkFlow As New WorkFlow
        Dim oEnviarCorreo As New EnviarCorreo
        Dim lstUsuariosANotificar As New List(Of String)
        Dim pagina As Page = New Tarea1

        If todasTareasPreviasEstanFinalizadas(idTarea, idTrabajo) Then
            If todosArchivosEntregablesXTareaSubidos(idTarea, idTrabajo) Then
                oLogWorkFlow.CORE_Log_WorkFlow_Add(idWorkFlow, DateTime.UtcNow.AddHours(-5), idusuario, LogWorkFlow.WorkFlowEstados.Finalizada, txtObservaciones.Text)
                oWorkFlow.Editar(idWorkFlow, Nothing, Nothing, Nothing, DateTime.UtcNow.AddHours(-5), Nothing, Nothing, Nothing)
                cargarTarea(idWorkFlow)
                lstUsuariosANotificar = obtenerUsuariosNotificacion(WorkFlow.Estados.Finalizada)
                oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/Tarea.aspx?IdTarea=" & idTarea & "&IdTrabajo=" & idTrabajo & "&IdWorkFlow=" & idWorkFlow & "&IdProyecto=" & idProyecto & "&IdEstado=" & WorkFlow.Estados.Finalizada)
                oLogWorkFlow.CORE_TareasDependientes_Finalizar(idTarea, idTrabajo, idusuario)

                If lstUsuariosANotificar.Count < 1 Then
                    ShowNotification("Tarea finaliza con exito - Pero,  no se pudo notificar a las tareas siguientes, que estan a la espera de que esta tarea finalice, debido a que esas tareas no tienen usuario asignado aún, por favor pongase en contacto con los coordinadores que requieren saber que esta tarea finalizo con exito.", ShowNotifications.InfoNotification)
                Else
                    ShowNotification("Tarea finaliza con exito", ShowNotifications.InfoNotification)
                End If
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Else
                ShowNotification("Debe subir todos los archivos entregables para poder finalizar la tarea", ShowNotifications.InfoNotification)
            End If
        Else
            ShowNotification("Todas las tareas previas a esta actividad deben estar en estado finalizado, para poder finalizar esta actividad", ShowNotifications.InfoNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End If
        btnFinalizarTarea.Enabled = False
    End Sub
    Private Sub gvTareasAnteriores_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareasAnteriores.RowCommand
        If e.CommandName = "Devolver" Then
            Dim oLogWorkFlow As New LogWorkFlow
            oLogWorkFlow.CORE_Log_WorkFlow_Add(gvTareasAnteriores.DataKeys(e.CommandArgument)("Id"), DateTime.UtcNow.AddHours(-5), idusuario, LogWorkFlow.WorkFlowEstados.Devuelta, txtObservaciones.Text)
            cargarTareasPrevias(idTarea, idTrabajo)
        End If
    End Sub
    Protected Sub btnGrabar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGrabar.Click
        Dim oLogWorkFlow As New LogWorkFlow
        Dim oEnviarCorreo As New EnviarCorreo
        oLogWorkFlow.CORE_Log_WorkFlow_Add(hfWorkFlowIdTareaDevolver.Value, DateTime.UtcNow.AddHours(-5), idusuario, LogWorkFlow.WorkFlowEstados.Devuelta, txtObservacionDevolucion.Text)
        cargarTareasPrevias(idTarea, idTrabajo)

        oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/Tarea.aspx?&IdTrabajo=" & idTrabajo & "&IdWorkFlow=" & hfWorkFlowIdTareaDevolver.Value & "&IdProyecto=" & idProyecto & "&IdEstado=" & WorkFlow.Estados.Devuelta)


    End Sub
    Private Sub btnVerTodasObservaciones_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVerTodasObservaciones.Click
        cargarObservaciones(idWorkFlow)
    End Sub
    Private Sub gvObservaciones_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvObservaciones.PageIndexChanging
        gvObservaciones.PageIndex = e.NewPageIndex
        cargarObservaciones(idWorkFlow)
    End Sub
#End Region
#Region "Metodos"
    Function obtenerTrabajoXId(ByVal idTrabajo As Int64) As PY_Trabajos_Get_Result
        Dim oTrabajo As New Trabajo
        Return oTrabajo.obtenerXId(idTrabajo)
    End Function
    Function obtenerOPMetodologia(ByVal id As Int16) As OP_Metodologias_Get_Result
        Dim oMetodologiaOperaciones As New MetodologiaOperaciones
        Return oMetodologiaOperaciones.obtenerXId(id)
    End Function
    Sub cargarTarea(ByVal WorkFlowid As Int64)
        Dim oWorkFlow As New WorkFlow
        Dim oeWorkFlow As CORE_WorkFlow_Trabajos_Get_Result
        oeWorkFlow = oWorkFlow.obtenerXId(WorkFlowid)
        lblTarea.Text = oeWorkFlow.Tarea.ToString
        lblFIniP.Text = If(oeWorkFlow.FIniP.HasValue, String.Format(oeWorkFlow.FIniP, "dd/mm/yyyy"), "")
        lblFFinP.Text = If(oeWorkFlow.FFinP.HasValue, String.Format(oeWorkFlow.FFinP, "dd/mm/yyyy"), "")
        lblFIniR.Text = If(oeWorkFlow.FIniR.HasValue, String.Format(oeWorkFlow.FIniR, "dd/mm/yyyy"), "")
        lblFFinR.Text = If(oeWorkFlow.FFinR.HasValue, String.Format(oeWorkFlow.FFinR, "dd/mm/yyyy"), "")
        If oeWorkFlow.EstadoWorkFlow_Id = LogWorkFlow.WorkFlowEstados.EnCurso OrElse oeWorkFlow.EstadoWorkFlow_Id = LogWorkFlow.WorkFlowEstados.Finalizada Then
            btnIniciarTarea.Enabled = False
        End If
        If oeWorkFlow.EstadoWorkFlow_Id = LogWorkFlow.WorkFlowEstados.Asignada OrElse oeWorkFlow.EstadoWorkFlow_Id = LogWorkFlow.WorkFlowEstados.Creada OrElse oeWorkFlow.EstadoWorkFlow_Id = LogWorkFlow.WorkFlowEstados.Finalizada Then
            btnFinalizarTarea.Enabled = False
        End If

        lblTarea1.Text = oeWorkFlow.Tarea.ToString
    End Sub
    Sub cargarTareasPrevias(ByVal idTarea As Int64, ByVal idTrabajo As Int64)

        gvTareasAnteriores.DataSource = obtenerTareasPrevias(idTarea, idTrabajo)
        gvTareasAnteriores.DataBind()
    End Sub
    Function obtenerTareasPrevias(ByVal idTarea As Int64, ByVal idTrabajo As Int64) As List(Of CORE_WorkFlow_TareasPrevias_Get_Result)
        Dim oTareasPrevias As New TareasPrevias
        Dim oeMetodologiaOperaciones As OP_Metodologias_Get_Result
        Dim oeTrabajo As PY_Trabajos_Get_Result
        oeTrabajo = obtenerTrabajoXId(idTrabajo)
        oeMetodologiaOperaciones = obtenerOPMetodologia(oeTrabajo.OP_MetodologiaId)
        Return oTareasPrevias.CORE_TareasPrevias_Get(oeMetodologiaOperaciones.TipoHiloId, idTarea, idTrabajo)
    End Function
    Function todasTareasPreviasEstanFinalizadas(ByVal idTarea As Int64, ByVal idTrabajo As Int64) As Boolean
        Dim lstTareasPrevias As New List(Of CORE_WorkFlow_TareasPrevias_Get_Result)
        lstTareasPrevias = obtenerTareasPrevias(idTarea, idTrabajo)
        If lstTareasPrevias.Where(Function(x) x.EstadoWorkFlow_Id <> LogWorkFlow.WorkFlowEstados.Finalizada).Count > 0 Then
            Return False
        Else
            Return True
        End If
    End Function
    Function todosArchivosEntregablesXTareaSubidos(ByVal idTarea As Int64, ByVal idTrabajo As Int64) As Boolean
        Dim o As New Tareas_Documentos
        Return Not o.obtenerDocumentosXTarea(idTarea, 2, idTrabajo).Where(Function(x) x.Cantidad = 0 AndAlso x.EsOpcional = False).Count > 0
    End Function
    Function obtenerUsuariosNotificacion(ByVal idEstado As WorkFlow.Estados) As List(Of String)
        Dim oWorkFlow As New WorkFlow
        If idEstado = WorkFlow.Estados.Devuelta Then
            Return oWorkFlow.obtenerUsuariosNotificacionTareaDevuelta(idTrabajo, idTarea).Select(Function(x) x.Email).ToList
        Else
            Return oWorkFlow.obtenerUsuariosNotificacionTareas(idTrabajo, idTarea).Select(Function(x) x.Email).ToList
        End If

    End Function
    Sub cargarObservaciones(ByVal workFlow_Id As Int64)
        Dim oWorkFlow As New WorkFlow
        gvObservaciones.DataSource = oWorkFlow.obtenerObservacionesXTarea(workFlow_Id)
        gvObservaciones.DataBind()
        upObservaciones.Update()
    End Sub
#End Region
End Class