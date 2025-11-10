Imports CoreProject
Imports WebMatrix.Util

Public Class AsignacionCampo
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
    Sub CargarTrabajos()
        Dim oTrabajo As New Trabajo
        'Dim oUsuarios As New US.Usuarios
        'Dim oProyecto As New Proyecto
        'Dim oMetodologias As New MetodologiaOperaciones
        'Dim liTrabajos = (From ltraba In oTrabajo.obtenerAllTrabajos
        '                  From lmetod In oMetodologias.obtenerTodos()
        '                  From lproyect In oProyecto.obtenerTodos
        '                  From lusuarios In oUsuarios.UsuariosxRol(ListaRoles.GerenteProyectos)
        '                Where IsNothing(ltraba.COE) And ltraba.OP_MetodologiaId = lmetod.Id And lmetod.MetGrupoUnidad = ddlGrupoUnidades.SelectedValue And ltraba.ProyectoId = lproyect.id And lusuarios.id = lproyect.GerenteProyectos
        '                Select id = ltraba.id, NombreTrabajo = ltraba.NombreTrabajo, Muestra = ltraba.Muestra, FechaTentativaInicioCampo = ltraba.FechaTentativaInicioCampo, FechaTentativaFinalizacion = ltraba.FechaTentativaFinalizacion, GerenteProyectos = lusuarios.Nombres & " " & lusuarios.Apellidos)
        Dim oGerenteOp As New GerentesDeOperacion
        Dim ltraba = oGerenteOp.ListadoTrabajosParaAsignarCoe(ddlGrupoUnidades.SelectedValue)
        'And ltraba.GrupoUnidadId = ddlGrupoUnidades.SelectedValue
        gvTrabajos.DataSource = ltraba.ToList
        gvTrabajos.DataBind()
    End Sub
    Sub CargarGruposUnidad()
        ddlGrupoUnidades.Items.Insert(0, New ListItem With {.Text = "IUU - Cualitativo", .Value = 20})
    End Sub
    Sub CargarCombo(ByVal cmb As DropDownList, ByVal dvf As String, ByVal dtf As String, ByVal ds As Object)
        cmb.DataValueField = dvf
        cmb.DataTextField = dtf
        cmb.DataSource = ds
        cmb.DataBind()
    End Sub
    Public Function ObtenerUsuarios() As List(Of ObtenerUsuarios_Result)
        Dim Data As New Datos.ClsPermisosUsuarios
        Try
            Return Data.ObtenerUsuarios
        Catch ex As Exception
            Throw ex
        End Try
    End Function
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
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/CampoAsignado.aspx?idTrabajo=" & hfidTrabajo.Value)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub

    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New LogEjecucion
            log.Guardar(27, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            If Session("IDUsuario") Is Nothing Then
                Response.Redirect("../Default.aspx?ReturnUrl=%2fRE_GT%AsignacionCampo.aspx")
            End If
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(19, UsuarioID) = False Then
                Response.Redirect("../Home.aspx")
            End If
            CargarGruposUnidad()
            CargarTrabajos()
            CargarCOEs()
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        End If
    End Sub
    Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
        gvTrabajos.PageIndex = e.NewPageIndex
        CargarTrabajos()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        If e.CommandName = "Asignar" Then
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            Me.hfidTrabajo.Value = Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")
            Me.hfidTipoProyecto.Value = Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("TipoProyectoId")
            upGerenteAsignar.Update()
        End If
    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        'Dim GrupoUnidad As Long = ddlGrupoUnidades.SelectedValue
        Dim oTrabajo As New Trabajo
        Dim oeTrabajo As PY_Trabajos_Get_Result
        Dim oProyecto As New Proyecto
        Dim oWorkFlow As New WorkFlow
        Dim o As New GerentesDeOperacion
        Dim TipoRecoleccion As Int16? = Nothing

        oeTrabajo = oTrabajo.obtenerXId(hfidTrabajo.Value)

        oTrabajo.Guardar(hfidTrabajo.Value, oeTrabajo.ProyectoId, oeTrabajo.OP_MetodologiaId, oeTrabajo.PresupuestoId, oeTrabajo.NombreTrabajo, oeTrabajo.Muestra, oeTrabajo.FechaTentativaInicioCampo, oeTrabajo.FechaTentativaFinalizacion, ddlLider.SelectedValue, oeTrabajo.Unidad, oeTrabajo.JobBook, TipoRecoleccion, Nothing)
        CargarTrabajos()
        log(hfidTrabajo.Value, 3)
        EnviarEmail()
        ShowNotification("Trabajo actualizado y asignado", ShowNotifications.InfoNotification)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

#End Region


End Class