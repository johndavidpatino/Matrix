Imports WebMatrix.Util
Imports CoreProject
Public Class Tareas
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


    Private _IdTrabajo As Int64
    Public Property IdTrabajo() As Int64
        Get
            Return _IdTrabajo
        End Get
        Set(ByVal value As Int64)
            _IdTrabajo = value
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


    Private _URLRetorno As UrlOriginal
    Public Property URLRetorno() As UrlOriginal
        Get
            Return _URLRetorno
        End Get
        Set(ByVal value As UrlOriginal)
            _URLRetorno = value
        End Set
    End Property


#End Region

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        IdUsuario = Session("IdUsuario")

        If Request.QueryString("IdTrabajo") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdTrabajo"), IdTrabajo)
        End If

        If Request.QueryString("IdProyecto") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdProyecto"), IdProyecto)
        End If

        If Request.QueryString("URLRetorno") IsNot Nothing Then
            Long.TryParse(Request.QueryString("URLRetorno"), URLRetorno)
        End If

        If IdTrabajo > 0 Then
            lblNombreTrabajo.Text = obtenerTrabajoXId(IdTrabajo).NombreTrabajo
        End If

        asignarURLDevolucion()

        If Not IsPostBack Then
            If IdTrabajo > 0 Then
                cargarXUsuarioXIdTrabajoXEstado(IdUsuario, IdTrabajo, WorkFlow.Estados.Asignada, gvTareasXRealizar)
                cargarXUsuarioXIdTrabajoXEstado(IdUsuario, IdTrabajo, WorkFlow.Estados.Devuelta, gvTareasDevueltas)
                cargarXUsuarioXIdTrabajoXEstado(IdUsuario, IdTrabajo, WorkFlow.Estados.Finalizada, gvTareasRealizadas)
                cargarXUsuarioXIdTrabajoXEstado(IdUsuario, IdTrabajo, WorkFlow.Estados.EnCurso, gvTareasEnCurso)
            Else
                cargarTareasXUsuarioXEstado(IdUsuario, WorkFlow.Estados.Asignada, gvTareasXRealizar)
                cargarTareasXUsuarioXEstado(IdUsuario, WorkFlow.Estados.Devuelta, gvTareasDevueltas)
                cargarTareasXUsuarioXEstado(IdUsuario, WorkFlow.Estados.Finalizada, gvTareasRealizadas)
                cargarTareasXUsuarioXEstado(IdUsuario, WorkFlow.Estados.EnCurso, gvTareasEnCurso)
            End If

        End If

    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        If IdTrabajo > 0 Then
            buscarXUsuarioXIdTrabajo(IdUsuario, IdTrabajo, txtBuscar.Text)
        Else
            buscarTareasXUsuario(IdUsuario, txtBuscar.Text)
        End If
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub gvTareasDevueltas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareasDevueltas.RowCommand
        If e.CommandName = "Ver" Then
            Response.Redirect("Tarea.aspx?IdTarea=" & gvTareasDevueltas.DataKeys(e.CommandArgument)("TareaId") & "&IdTrabajo=" & IdTrabajo & "&IdWorkFlow=" & gvTareasDevueltas.DataKeys(e.CommandArgument)("Id") & "&IdProyecto=" & IdProyecto & "&URLRetorno=" & URLRetorno)
        End If
    End Sub

    Private Sub gvTareasRealizadas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareasRealizadas.RowCommand
        If e.CommandName = "Ver" Then
            Response.Redirect("Tarea.aspx?IdTarea=" & gvTareasRealizadas.DataKeys(e.CommandArgument)("TareaId") & "&IdTrabajo=" & IdTrabajo & "&IdWorkFlow=" & gvTareasRealizadas.DataKeys(e.CommandArgument)("Id") & "&IdProyecto=" & IdProyecto & "&URLRetorno=" & URLRetorno)
        End If
    End Sub

    Private Sub gvTareasEnCurso_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareasEnCurso.RowCommand
        If e.CommandName = "Ver" Then
            Response.Redirect("Tarea.aspx?IdTarea=" & gvTareasEnCurso.DataKeys(e.CommandArgument)("TareaId") & "&IdTrabajo=" & IdTrabajo & "&IdWorkFlow=" & gvTareasEnCurso.DataKeys(e.CommandArgument)("Id") & "&IdProyecto=" & IdProyecto & "&URLRetorno=" & URLRetorno)
        End If
    End Sub
    Private Sub gvTareasXRealizar_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareasXRealizar.RowCommand
        If e.CommandName = "Ver" Then
            Response.Redirect("Tarea.aspx?IdTarea=" & gvTareasXRealizar.DataKeys(e.CommandArgument)("TareaId") & "&IdTrabajo=" & IdTrabajo & "&IdWorkFlow=" & gvTareasXRealizar.DataKeys(e.CommandArgument)("Id") & "&IdProyecto=" & IdProyecto & "&URLRetorno=" & URLRetorno)
        End If
    End Sub
    Private Sub gvTareasXRealizar_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTareasXRealizar.PageIndexChanging
        gvTareasXRealizar.PageIndex = e.NewPageIndex
        If IdTrabajo > 0 Then
            cargarXUsuarioXIdTrabajoXEstado(IdUsuario, IdTrabajo, WorkFlow.Estados.Asignada, gvTareasXRealizar)
        Else
            cargarTareasXUsuarioXEstado(IdUsuario, WorkFlow.Estados.Asignada, gvTareasXRealizar)
        End If
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub
    Private Sub gvTareasDevueltas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTareasDevueltas.PageIndexChanging
        gvTareasDevueltas.PageIndex = e.NewPageIndex
        If IdTrabajo > 0 Then
            cargarXUsuarioXIdTrabajoXEstado(IdUsuario, IdTrabajo, WorkFlow.Estados.Devuelta, gvTareasDevueltas)
        Else
            cargarTareasXUsuarioXEstado(IdUsuario, WorkFlow.Estados.Devuelta, gvTareasDevueltas)
        End If
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub
    Private Sub gvTareasEnCurso_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTareasEnCurso.PageIndexChanging
        gvTareasEnCurso.PageIndex = e.NewPageIndex
        If IdTrabajo > 0 Then
            cargarXUsuarioXIdTrabajoXEstado(IdUsuario, IdTrabajo, WorkFlow.Estados.EnCurso, gvTareasEnCurso)
        Else
            cargarTareasXUsuarioXEstado(IdUsuario, WorkFlow.Estados.EnCurso, gvTareasEnCurso)
        End If
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub
    Private Sub gvTareasRealizadas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTareasRealizadas.PageIndexChanging
        gvTareasRealizadas.PageIndex = e.NewPageIndex
        If IdTrabajo > 0 Then
            cargarXUsuarioXIdTrabajoXEstado(IdUsuario, IdTrabajo, WorkFlow.Estados.Finalizada, gvTareasRealizadas)
        Else
            cargarTareasXUsuarioXEstado(IdUsuario, WorkFlow.Estados.Finalizada, gvTareasRealizadas)
        End If

        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub
#End Region

#Region "Metodos"
    Sub cargarTareasXUsuarioXEstado(ByVal usuario As Int64, ByVal estado As Int16, ByVal gridView As GridView)
        Dim oWorkFlow As New WorkFlow
        gridView.DataSource = oWorkFlow.obtenerXUsuarioAsignadoXEstado(usuario, estado)
        gridView.DataBind()
    End Sub
    Sub cargarXUsuarioXIdTrabajoXEstado(ByVal usuario As Int64, ByVal idtrabajo As Int64, ByVal Estado As WorkFlow.Estados, ByVal grdView As GridView)
        Dim oWorkFlow As New WorkFlow
        grdView.DataSource = oWorkFlow.obtenerXUsuarioAsignadoXIdTrabajoXEstado(usuario, idtrabajo, Estado)
        grdView.DataBind()
    End Sub
    Sub buscarTareasXUsuario(ByVal usuario As Int64, ByVal todosCampos As String)
        Dim oWorkFlow As New WorkFlow
        gvTareasXRealizar.DataSource = oWorkFlow.obtenerXUsuarioAsignadoXTodosCampos(usuario, todosCampos)
        gvTareasXRealizar.DataBind()
    End Sub
    Sub buscarXUsuarioXIdTrabajo(ByVal usuario As Int64, ByVal idtrabajo As Int64, ByVal todosCampos As String)
        Dim oWorkFlow As New WorkFlow
        gvTareasXRealizar.DataSource = oWorkFlow.obtenerXUsuarioAsignadoXIdTrabajoXTodosCampos(usuario, idtrabajo, todosCampos)
        gvTareasXRealizar.DataBind()
    End Sub
    Function obtenerTrabajoXId(ByVal id As Int64) As PY_Trabajos_Get_Result
        Dim oTrabajo As New Trabajo
        Return oTrabajo.obtenerXId(id)
    End Function
    Sub asignarURLDevolucion()
        Select Case URLRetorno
            Case UrlOriginal.CORE_ListaTrabajosTareas
                lbtnVolver.PostBackUrl = "~/CORE/ListaTrabajosTareas.aspx"
            Case UrlOriginal.OP_Cuantitativo_Trabajos
                lbtnVolver.PostBackUrl = "~/OP_Cuantitativo/Trabajos.aspx"
            Case UrlOriginal.PY_Proyectos_Trabajos
                lbtnVolver.PostBackUrl = "~/PY_Proyectos/Trabajos.aspx?proyectoId=" & IdProyecto & "&trabajoId=" & IdTrabajo
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