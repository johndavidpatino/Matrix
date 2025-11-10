Imports CoreProject
Imports WebMatrix.Util

Public Class TrabajosPorGerencia
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
    Sub CargarGruposUnidad()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlGrupoUnidades.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(2)
        ddlGrupoUnidades.DataValueField = "id"
        ddlGrupoUnidades.DataTextField = "GrupoUnidad"
        ddlGrupoUnidades.DataBind()
        ddlGrupoUnidades.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
        ddlGrupoUnidades.Items.Insert(1, New ListItem With {.Text = "--Ver todos--", .Value = 0})
    End Sub
    Sub CargarTiposDeRecolección()
        Dim oTrabajoOP As New TrabajoOPCuanti
        Me.ddlTipoRecoleccion.DataSource = oTrabajoOP.ObtenerTecnicasRecoleccion
        Me.ddlTipoRecoleccion.DataValueField = "id"
        Me.ddlTipoRecoleccion.DataTextField = "Recoleccion"
        Me.ddlTipoRecoleccion.DataBind()
    End Sub

    Sub CargarCOEs()
        Try
            Dim oUsuarios As New US.Usuarios

            Dim listapersonas = (From lpersona In oUsuarios.UsuariosxGrupoUnidadXrol(ddlGrupoUnidades.SelectedValue, ListaRoles.COE)
                                 Select Id = lpersona.id,
                                 Nombre = lpersona.Apellidos & " " & lpersona.Nombres).OrderBy(Function(p) p.Nombre)
            ddlLider.DataSource = listapersonas.ToList()
            ddlLider.DataValueField = "Id"
            ddlLider.DataTextField = "Nombre"
            ddlLider.DataBind()
            upGerenteAsignar.Update()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Sub EnviarEmail()
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            If String.IsNullOrEmpty(hfidTrabajo.Value) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar un anuncio de aprobación")
            End If
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/COEAsignado.aspx?idTrabajo=" & hfidTrabajo.Value)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub

#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(19, UsuarioID) = False Then
                Response.Redirect("../Home.aspx")
            End If
            CargarGruposUnidad()
            If Not (Session("IdGerenciaOp") = Nothing) Then
                ddlGrupoUnidades.SelectedValue = Session("IdGerenciaOp").ToString
                CargarTrabajos()
                CargarCOEs()
            End If
            CargarTiposDeRecolección()
            'CargarCombo(ddlLider, "id", "Nombres", ObtenerUsuarios())
            'CargarCOEs()
        End If
    End Sub

    Protected Sub ddlGrupoUnidades_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlGrupoUnidades.SelectedIndexChanged
        CargarTrabajos()
        CargarCOEs()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
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
            Try
                ddlLider.SelectedValue = infot.COE
            Catch ex As Exception
            End Try

            'Else
            '    Me.txtJobBookInt.Text = JobBook & "-00"
            '    Me.txtJobBookInt.Visible = True
            '    Me.txtJobBook.Text = ""
            '    Me.txtJobBook.Visible = False
            'End If
            upGerenteAsignar.Update()
        ElseIf e.CommandName = "Presupuestos" Then
            Me.hfidTrabajo.Value = Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")
            Dim oTrabajo As New CoreProject.Trabajo
            Dim oPresupuesto As New Presupuesto
            Dim info = oTrabajo.ObtenerTrabajo(hfidTrabajo.Value)
            Dim infoT As New oJobBook
            Dim oData As New CU_JobBook.DAL
            Dim rData = oData.InfoJobBookGet(IdPropuesta:=info.IdPropuesta).FirstOrDefault
            infoT.Cliente = rData.Cliente
            infoT.Estado = rData.Estado
            infoT.GerenteCuentas = rData.GerenteCuentas
            infoT.GerenteCuentasID = rData.GerenteCuentasID
            infoT.IdBrief = rData.IdBrief
            infoT.IdEstudio = rData.IdEstudio
            infoT.IdPropuesta = rData.IdPropuesta
            infoT.IdUnidad = rData.IdUnidad
            infoT.MarcaCategoria = rData.MarcaCategoria
            infoT.Titulo = rData.Titulo
            infoT.Unidad = rData.Unidad
            infoT.Viabilidad = rData.Viabilidad
            infoT.NumJobBook = rData.NumJobbook
            infoT.Alternativa = info.Alternativa
            infoT.ReviewOPS = True
            infoT.GuardarCambios = False
            Session("InfoJobBook") = infoT
            Response.Redirect("../CU_Cuentas/Presupuesto.aspx")
            'Response.Redirect("../CAP/Cap_Principal.aspx?IdPropuesta=" & info.IdPropuesta & "&Alternativa=" & info.Alternativa & "&Accion=7")
        ElseIf e.CommandName = "Muestra" Then
            Me.hfidTrabajo.Value = Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")
            Response.Redirect("../OP_Cuantitativo/MuestraTrabajos.aspx?TrabajoId=" & hfidTrabajo.Value)
        ElseIf e.CommandName = "InfoGeneral" Then
            Me.hfidTrabajo.Value = Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")
            Response.Redirect("../RP_Reportes/InformacionGeneral.aspx?idTr=" & hfidTrabajo.Value & "&urlback=../RP_Reportes/TrabajosPorGerencia.aspx")
        End If
    End Sub

    Private Sub TrabajosPorGerencia_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(125, UsuarioID) = False Then
            Response.Redirect("../home.aspx")
        End If
    End Sub
#End Region


    Private Sub btnUpdate_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click
        'Dim GrupoUnidad As Long = ddlGrupoUnidades.SelectedValue
        Dim oTrabajo As New Trabajo
        Dim oeTrabajo As PY_Trabajos_Get_Result
        Dim oProyecto As New Proyecto
        Dim oWorkFlow As New WorkFlow
        Dim o As New GerentesDeOperacion

        oeTrabajo = oTrabajo.obtenerXId(hfidTrabajo.Value)

        oTrabajo.Guardar(hfidTrabajo.Value, oeTrabajo.ProyectoId, oeTrabajo.OP_MetodologiaId, oeTrabajo.PresupuestoId, oeTrabajo.NombreTrabajo, oeTrabajo.Muestra, oeTrabajo.FechaTentativaInicioCampo, oeTrabajo.FechaTentativaFinalizacion, ddlLider.SelectedValue, oeTrabajo.Unidad, oeTrabajo.JobBook, ddlTipoRecoleccion.SelectedValue, Nothing)
        CargarTrabajos()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        EnviarEmail()
        ShowNotification("Trabajo actualizado y asignado", ShowNotifications.InfoNotification)


        'Me.gvTrabajos.DataBind()
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        CargarTrabajos()
    End Sub

    Sub CargarTrabajos()
        Dim oRep_fil As New Reportes.RP_GerOpe
        If ddlGrupoUnidades.SelectedIndex = -1 Then
            If IsNumeric(txtBuscar.Text) Then
                gvTrabajos.DataSource = oRep_fil.ObtenerTrabajosPorGerencia_filtrados(Nothing, txtBuscar.Text, Nothing, Nothing)
                gvTrabajos.DataBind()
            End If
        End If
        If IsNumeric(txtBuscar.Text) Then
            gvTrabajos.DataSource = oRep_fil.ObtenerTrabajosPorGerencia_filtrados(ddlGrupoUnidades.SelectedValue, txtBuscar.Text, Nothing, Nothing)
            gvTrabajos.DataBind()
        Else
            gvTrabajos.DataSource = oRep_fil.ObtenerTrabajosPorGerencia_filtrados(ddlGrupoUnidades.SelectedValue, Nothing, Nothing, txtBuscar.Text)
            gvTrabajos.DataBind()

        End If
        If Not (ddlGrupoUnidades.SelectedIndex = -1) Then
            Session("IdGerenciaOp") = ddlGrupoUnidades.SelectedValue
        End If
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
End Class