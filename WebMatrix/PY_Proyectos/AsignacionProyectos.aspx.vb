Imports CoreProject
Imports WebMatrix.Util

Public Class AsignacionProyectos
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            CargarProyectos()
            CargarUnidades()
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(35, UsuarioID) = False Then
                Response.Redirect("../PY_Proyectos/Default.aspx")
            End If

            'If Request.QueryString("IdEstudio") IsNot Nothing Then
            '    ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            'End If
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        End If
    End Sub


    Private Sub gvProyectos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvProyectos.RowCommand
        If e.CommandName = "PresupuestosAsignados" Then
            'CargarPresupuestos(Me.gvProyectos.DataKeys(CInt(e.CommandArgument))("EstudioId"), Me.gvProyectos.DataKeys(CInt(e.CommandArgument))("Id"), True, gvPresupuestosAsignadosXProyecto)
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            'upPresupuestosAsignadosXProyecto.Update()
        ElseIf e.CommandName = "Asignar" Then
            'CargarProyecto(Me.gvProyectos.DataKeys(CInt(e.CommandArgument))("Id"))
            Me.hfIdProyecto.Value = Me.gvProyectos.DataKeys(CInt(e.CommandArgument))("Id")
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            'CargarCombo(ddlLider, "id", "Nombres", ObtenerUsuarios())
            'CargarGerentesProyectos()
            upGerenteProyectoAsignar.Update()
        ElseIf e.CommandName = "Informacion" Then
            Dim idproyecto = Int64.Parse(gvProyectos.DataKeys(CInt(e.CommandArgument))("Id"))
            Dim oProyecto As New Proyecto
            Dim oEstudio As New Estudio
            Dim oPropuesta As New Propuesta
            Dim infoP = oProyecto.obtenerXId(idproyecto)
            Dim infoE = oEstudio.ObtenerXID(infoP.EstudioId)
            Dim infoPr = oPropuesta.DevolverxID(infoE.PropuestaId)
            hfIdProyecto.Value = idproyecto
            hfIdPropuesta.Value = infoPr.Id
            CargarInfo(infoPr.Brief)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        End If
    End Sub


    Protected Sub gvProyectos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvProyectos.PageIndexChanging
        gvProyectos.PageIndex = e.NewPageIndex
        CargarProyectos()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
#End Region

#Region "Metodos"
    Private Sub CargarProyectos()
        If Not IsNumeric(Me.ddlUnidades.SelectedValue) Then Exit Sub
        Dim oProyectos As New Proyecto
        Try
            gvProyectos.DataSource = oProyectos.obtenerXAsignarGerenteProyecto(ddlUnidades.SelectedValue)
            gvProyectos.DataBind()
        Catch ex As Exception
        End Try


    End Sub

    Sub CargarGerentesProyectos()
        Try
            Dim oUsuarios As New US.Usuarios
            Dim oUnidades As New US.Unidades

            Dim GrupoUnidad = oUnidades.ObtenerGrupoUnidadxUnidad(ddlUnidades.SelectedValue)

            Dim listapersonas = (From lpersona In oUsuarios.UsuariosxGrupoUnidadXrol(GrupoUnidad, ListaRoles.GerenteProyectos)
                                 Select Id = lpersona.id,
                                 Nombre = lpersona.Apellidos & " " & lpersona.Nombres).OrderBy(Function(p) p.Nombre)
            ddlLider.DataSource = listapersonas.ToList()
            ddlLider.DataValueField = "Id"
            ddlLider.DataTextField = "Nombre"
            ddlLider.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub CargarUnidades()
        Dim oUnidades As New CoreProject.US.Unidades

        'ddlUnidades.DataSource = oUnidades.ObtenerUnidadCombo
        ddlUnidades.DataSource = oUnidades.ObtenerUnidadesxUsuario(Session("IDUsuario").ToString)
        ddlUnidades.DataTextField = "Unidad"
        ddlUnidades.DataValueField = "id"
        ddlUnidades.DataBind()

        ddlUnidades.Items.Insert(0, New ListItem With {.Value = -1, .Text = "--Seleccione--"})

    End Sub


    Sub CargarProyecto(ByVal id As Long)
        Dim oProyecto As New Proyecto
        Dim oPY_Proyectos_Get_Result As PY_Proyectos_Get_Result
        Dim oPresupuesto As New PY.Presupuesto

        oPY_Proyectos_Get_Result = oProyecto.obtenerXId(id)
        Dim x = oPresupuesto.obtener(oPY_Proyectos_Get_Result.EstudioId, id, True)

        hfIdProyecto.Value = id

    End Sub


#End Region

    Protected Sub ddlUnidades_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUnidades.SelectedIndexChanged
        CargarProyectos()
        CargarGerentesProyectos()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
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

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim oProyecto As New Proyecto
        oProyecto.ActualizarGerente(ddlLider.SelectedValue, hfIdProyecto.Value)
        CargarProyectos()
        EnviarEmail()
        EnviarEmailAnuncio()
        log(hfIdProyecto.Value, 3)
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Sub EnviarEmail()
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            If String.IsNullOrEmpty(hfIdProyecto.Value) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar un anuncio de aprobación")
            End If
            'Dim script As String = "window.open('../Emails/GerenteProyectosAsignado.aspx?idProyecto=" & hfIdProyecto.Value & "','cal','width=400,height=250,left=270,top=180')"
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/GerenteProyectosAsignado.aspx?idProyecto=" & hfIdProyecto.Value)
            'Dim page As Page = DirectCast(Context.Handler, Page)
            'ScriptManager.RegisterStartupScript(page, GetType(Page), "Anuncio", script, True)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub

    Sub EnviarEmailAnuncio()
        Dim oEnviarCorreo As New EnviarCorreo
        Dim oProyecto As New Proyecto
        Dim oPY_Proyecto As New PY_Proyectos_Get_Result
        oPY_Proyecto = oProyecto.obtenerXId(hfIdProyecto.Value)
        Dim idEstudio As Long = oPY_Proyecto.EstudioId
        Try
            If String.IsNullOrEmpty(hfIdProyecto.Value) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar un anuncio de aprobación")
            End If
            'Dim script As String = "window.open('../Emails/AnuncioAprobacion.aspx?idEstudio=" & idEstudio & "&idGerenteProyectos=" & Me.ddlLider.SelectedValue & "','cal','width=400,height=250,left=270,top=180')"
            'Dim page As Page = DirectCast(Context.Handler, Page)
            'ScriptManager.RegisterStartupScript(page, GetType(Page), "Anuncio", script, True)
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/AnuncioAprobacionSinValor.aspx?idEstudio=" & idEstudio & "&idGerenteProyectos=" & Me.ddlLider.SelectedValue)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub

    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New LogEjecucion
            log.Guardar(21, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Public Sub CargarInfo(ByVal idbrief As Int64)
        Try
            Dim oBrief As New Brief
            Dim info = oBrief.DevolverxID(idbrief)
            txtTitulo.Text = info.Titulo
            txtAntecedentes.Text = info.Antecedentes
            txtObjetivos.Text = info.Objetivos
            txtActionStandard.Text = info.ActionStandars
            txtMetodologia.Text = info.Metodologia
            txtTargetGroup.Text = info.TargetGroup
            txtTiempos.Text = info.Tiempos
            txtPresupuesto.Text = info.Presupuestos
            txtMateriales.Text = info.Materiales
            txtEstudiosAnteriores.Text = info.ResultadosAnteriores
            txtFormatos.Text = info.FormatoInforme
            txtAprobaciones.Text = info.Aprobaciones
            txtCompetencia.Text = info.Competencia
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnDescargarPropuesta_Click(sender As Object, e As System.EventArgs) Handles btnDescargarPropuesta.Click
        Response.Redirect("../GD_Documentos/GD_Documentos.aspx?IdContenedor=" & hfIdPropuesta.Value & "&IdDocumento=2&TipoAccion=2")
    End Sub
End Class