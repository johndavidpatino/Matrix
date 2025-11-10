Imports CoreProject
Imports WebMatrix.Util
Public Class TrabajosPorCCT
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Private _proyectoId As Int64
    Public Property proyectoId() As Int64
        Get
            Return _proyectoId
        End Get
        Set(ByVal value As Int64)
            _proyectoId = value
        End Set
    End Property
    Private _idUsuario As Int64
    Public Property idUsuario() As Int64
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Int64)
            _idUsuario = value
        End Set
    End Property
#End Region

#Region "Funciones y Metodos"
    'Sub CargarTrabajos()
    '    If ddlGrupoUnidades.SelectedIndex = -1 Then Exit Sub
    '    Dim oRep As New Reportes.RP_GerOpe
    '    gvTrabajos.DataSource = oRep.ObtenerTrabajosPorGerencia(ddlGrupoUnidades.SelectedValue, Nothing, Nothing, Nothing)
    '    gvTrabajos.DataBind()
    '    ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    'End Sub

#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            CargarUnidades()
            CargarGerentesCuentas()
        End If
    End Sub

    Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
        gvTrabajos.PageIndex = e.NewPageIndex
        CargarTrabajos()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        If e.CommandName = "Avance" Then
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            Me.hfidTrabajo.Value = Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")
            Response.Redirect("AvanceDeCampo.aspx?TrabajoId=" & hfidTrabajo.Value)
        ElseIf e.CommandName = "Project" Then
            hfidTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Response.Redirect("../RP_Reportes/GanttUnTrabajo.aspx?TrabajoId=" & hfidTrabajo.Value)
        ElseIf e.CommandName = "Tareas" Then
            hfidTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Response.Redirect("../CORE/Gestion-Tareas.aspx?IdTrabajo=" & hfidTrabajo.Value & "&URLRetorno=" & UrlOriginal.RE_GT_TrabajosPorGerencia)
        ElseIf e.CommandName = "Asignar" Then
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            Me.hfidTrabajo.Value = Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")
            Dim oProyecto As New CoreProject.Proyecto
            Dim oTrabajo As New CoreProject.Trabajo
            Dim infot = oTrabajo.obtenerXId(hfidTrabajo.Value)
            'Dim infop = oProyecto.obtenerXId(infot.ProyectoId)
            'Dim JobBook As String = infop.JobBook
            ''If JobBook.Length = 12 Then
            'Me.txtJobBook.Text = JobBook & "-00"
            'Me.txtJobBook.Visible = True
            'Me.txtJobBookInt.Text = ""
            'Me.txtJobBookInt.Visible = False
            'Me.txtJobBook.Text = infot.JobBook
        ElseIf e.CommandName = "Presupuestos" Then
            Me.hfidTrabajo.Value = Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")
            Dim oTrabajo As New CoreProject.Trabajo
            Dim oPresupuesto As New Presupuesto
            Dim info = oTrabajo.ObtenerTrabajo(hfidTrabajo.Value)
            Response.Redirect("../CAP/Cap_Principal.aspx?IdPropuesta=" & info.IdPropuesta & "&Alternativa=" & info.Alternativa & "&Accion=1")
        ElseIf e.CommandName = "Muestra" Then
            Me.hfidTrabajo.Value = Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")
            Response.Redirect("../OP_Cuantitativo/MuestraTrabajos.aspx?TrabajoId=" & hfidTrabajo.Value)
        End If
    End Sub

#End Region

    Sub CargarUnidades()
        Dim oUnidades As New US.Unidades
        ddlUnidades.DataSource = oUnidades.ObtenerUnidadesxUsuario(Session("IDUsuario").ToString)
        ddlUnidades.DataValueField = "id"
        ddlUnidades.DataTextField = "Unidad"
        ddlUnidades.DataBind()
        'ddlUnidades.Items.Insert(0, New ListItem With {.Text = "--Ver todo--", .Value = -1})
        CargarGerentesCuentas()
    End Sub

    Sub CargarGerentesCuentas()

        If ddlUnidades.SelectedValue = "-1" Or ddlUnidades.SelectedValue = "" Then
            ddlGerenteCuentas.Items.Clear()
        Else
            Dim o As New Reportes.RP_GerOpe
            ddlGerenteCuentas.DataSource = o.ObtenerUsuariosXUnidadXRol(ddlUnidades.SelectedValue, ListaRoles.GerenteProyectos)
            ddlGerenteCuentas.DataValueField = "id"
            ddlGerenteCuentas.DataTextField = "NombreCompleto"
            ddlGerenteCuentas.DataBind()
            ddlGerenteCuentas.Items.Insert(0, New ListItem With {.Text = "--Ver todo--", .Value = -1})
        End If
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        CargarTrabajos()
    End Sub

    Sub CargarTrabajos()
        Dim o As New Reportes.Directores
        Dim ID As Long? = Nothing, JobBook As String = Nothing, unidad As Integer? = Nothing, nombretrabajo As String = Nothing, gerentecuentas As Long? = Nothing
        If IsNumeric(txtIdTrabajo.Text) Then ID = txtIdTrabajo.Text
        If Not (txtJobBook.Text = "") Then JobBook = txtJobBook.Text
        If Not (ddlUnidades.SelectedValue = "") Then unidad = ddlUnidades.SelectedValue
        If Not (txtNombreTrabajo.Text = "") Then nombretrabajo = txtNombreTrabajo.Text
        If Not (ddlGerenteCuentas.SelectedValue = "-1") Then gerentecuentas = ddlGerenteCuentas.SelectedValue
        gvTrabajos.DataSource = o.ObtenerListadoTrabajosCCT(unidad, gerentecuentas, ID, JobBook, nombretrabajo)
        gvTrabajos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub TrabajosPorCCT_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(128, UsuarioID) = False Then
            Response.Redirect("../home.aspx")
        End If
    End Sub

    Private Sub ddlUnidades_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlUnidades.SelectedIndexChanged
        CargarGerentesCuentas()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub
End Class