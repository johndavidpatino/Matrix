Imports CoreProject
Imports WebMatrix.Util

Public Class AsignacionJBI
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
    Sub CargarPresupuestos()
        If ddlGrupoUnidades.SelectedIndex = -1 Or ddlGrupoUnidades.SelectedIndex = 0 Then Exit Sub
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
        'Dim oGerenteOp As New GerentesDeOperacion
        Dim o As New Reportes.GerentesOP
        'Dim ltraba = oGerenteOp.ListadoTrabajosParaAsignarCoe(ddlGrupoUnidades.SelectedValue)
        'And ltraba.GrupoUnidadId = ddlGrupoUnidades.SelectedValue
        'gvDatos.DataSource = ltraba.ToList
        gvDatos.DataSource = o.ObtenerPresupuestosRevision(ddlGrupoUnidades.SelectedValue)
        gvDatos.DataBind()
    End Sub
    Sub CargarGruposUnidad()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlGrupoUnidades.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(2)
        ddlGrupoUnidades.DataValueField = "id"
        ddlGrupoUnidades.DataTextField = "GrupoUnidad"
        ddlGrupoUnidades.DataBind()
        ddlGrupoUnidades.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})

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


    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New LogEjecucion
            log.Guardar(37, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
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
            CargarPresupuestos()
            'CargarCombo(ddlLider, "id", "Nombres", ObtenerUsuarios())
            'CargarCOEs()
        End If
    End Sub

    Protected Sub ddlGrupoUnidades_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlGrupoUnidades.SelectedIndexChanged
        CargarPresupuestos()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        CargarPresupuestos()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        If e.CommandName = "Asignar" Then
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            Me.hfProyecto.Value = Me.gvDatos.DataKeys(CInt(e.CommandArgument))("IDProy")
            Me.hfidPropuesta.Value = Me.gvDatos.DataKeys(CInt(e.CommandArgument))("idpropuesta")
            Me.hfParAlternativa.Value = Me.gvDatos.DataKeys(CInt(e.CommandArgument))("alternativa")
            Me.hfMetCodigo.Value = Me.gvDatos.DataKeys(CInt(e.CommandArgument))("metcodigo")
            Me.hfParNacional.Value = Me.gvDatos.DataKeys(CInt(e.CommandArgument))("parnacional")

            Dim oProyecto As New CoreProject.Proyecto
            Dim infop = oProyecto.obtenerXId(hfProyecto.Value)
            Dim JobBook As String = infop.JobBook
            'If JobBook.Length = 12 Then
            Me.txtJobBook.Text = JobBook & "-00"
            Me.txtJobBook.Visible = True
            Me.txtJobBookInt.Text = ""
            Me.txtJobBookInt.Visible = False
            'Else
            '    Me.txtJobBookInt.Text = JobBook & "-00"
            '    Me.txtJobBookInt.Visible = True
            '    Me.txtJobBook.Text = ""
            '    Me.txtJobBook.Visible = False
            'End If
            upGerenteAsignar.Update()
        ElseIf e.CommandName = "Presupuestos" Then
            Me.hfidPropuesta.Value = Me.gvDatos.DataKeys(CInt(e.CommandArgument))("idpropuesta")
            Me.hfParAlternativa.Value = Me.gvDatos.DataKeys(CInt(e.CommandArgument))("alternativa")
            Response.Redirect("../CAP/Cap_Principal.aspx?IdPropuesta=" & hfidPropuesta.Value & "&Alternativa=" & hfParAlternativa.Value & "&Accion=8")
        End If
    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        'Dim GrupoUnidad As Long = ddlGrupoUnidades.SelectedValue
        Dim oProyecto As New Proyecto
        Dim oWorkFlow As New WorkFlow
        Dim o As New GerentesDeOperacion

        Dim JobBook As String = ""
        If txtJobBook.Text = "" Then
            JobBook = txtJobBookInt.Text
        Else
            JobBook = txtJobBook.Text
        End If
        If JobBook = "" Then
            ShowNotification("No podrá continuar hasta que no escriba el número del JobBook", ShowNotifications.ErrorNotification)
            Exit Sub
        End If

        If JobBook = "" Or JobBook.EndsWith("00") Then
            ShowNotification("Debe escribir un número de JobBook válido antes de continuar", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        Dim idProyecto As Int64 = hfProyecto.Value
        Dim jbproyecto As String = oProyecto.obtenerXId(idProyecto).JobBook
        If Not (JobBook.StartsWith(jbproyecto.Remove(jbproyecto.Length - 2, 2))) Then
            ShowNotification("El JobBook debe mantener la misma estructura del proyecto", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If
        hfJobBook.Value = JobBook
        o.AsignarJBI(hfidPropuesta.Value, hfParAlternativa.Value, hfMetCodigo.Value, hfParNacional.Value, JobBook)

        CargarPresupuestos()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        EnviarEmail()
        ShowNotification("JobBook interno asignado", ShowNotifications.InfoNotification)
        log(hfProyecto.Value, 3)
        Me.txtJobBook.Text = ""
        'Me.gvDatos.DataBind()
    End Sub

#End Region

    Sub EnviarEmail()
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/JBIAsignadoProyectos.aspx?idProyecto=" & hfProyecto.Value & "&CodMetod=" & hfMetCodigo.Value & "&JBIInt=" & hfJobBook.Value)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub

End Class