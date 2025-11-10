Imports CoreProject
Imports WebMatrix.Util

Public Class TrabajosCuali
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Dim eTrabajoOP As New OP_TrabajoConfiguracion
    Private _IDUsuario As Int64
    Public Property IDUsuario() As Int64
        Get
            Return _IDUsuario
        End Get
        Set(ByVal value As Int64)
            _IDUsuario = value
        End Set
    End Property
#End Region

#Region "Metodos y Funciones"
    Sub CargarTrabajos(ByVal Coe As Int64)
        Dim oTrabajo As New Trabajo
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())

        If permisos.VerificarPermisoUsuario(42, UsuarioID) = True Then
            Dim oCoord As New CoordinacionCampo
            Dim listTrabCoord = (From lmuest In oCoord.ObtenerMuestraxCoordinador(Session("IDUsuario").ToString)
                                 Select lmuest.TrabajoId)

            Dim listTrabajos = (From ltraba In oTrabajo.obtenerXCOE(Coe)
                                Where listTrabCoord.Contains(ltraba.id)
                                Select ltraba)
            gvTrabajos.DataSource = listTrabajos.ToList
        Else
            If permisos.VerificarPermisoUsuario(148, UsuarioID) = True Then
                gvTrabajos.DataSource = oTrabajo.ObtenerTrabajosCualitativosxCOE(Nothing, 2, Nothing)
            Else
                gvTrabajos.DataSource = oTrabajo.ObtenerTrabajosCualitativosxCOE(Coe, Nothing, Nothing)
            End If
        End If

        gvTrabajos.DataBind()
    End Sub
    Function obtenerOPMetodologia(ByVal id As Int16) As OP_Metodologias_Get_Result
        Dim oMetodologiaOperaciones As New MetodologiaOperaciones
        Return oMetodologiaOperaciones.obtenerXId(id)
    End Function
    Function obtenerProyectoXId(ByVal id As Int64) As PY_Proyectos_Get_Result
        Dim oProyecto As New Proyecto
        Return oProyecto.obtenerXId(id)
    End Function
    Public Function CargarFichaCuantitativa() As Long
        Try
            Dim idtrabajo As Int64 = Int64.Parse(hfIdTrabajo.Value)
            Dim oTrabajo As New Trabajo
            Dim info = oTrabajo.DevolverxID(idtrabajo)

            Dim oFichaCuantitativa As New FichaCuantitativo
            Return oFichaCuantitativa.DevolverxTrabajoID(hfIdTrabajo.Value).Item(0).id

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Sub CargarConfiguracionTrabajo(ByVal TrabajoId As Int64)
        Dim oTrabajoOP As New TrabajoOPCuanti
        Try
            eTrabajoOP = oTrabajoOP.ObtenerTrabajoConfiguracion(TrabajoId)
            txtFechaInicio.Text = eTrabajoOP.FechaInicioCampo
            txtFechaTerminacion.Text = eTrabajoOP.FechaFinalCampo
        Catch ex As Exception
            eTrabajoOP = New OP_TrabajoConfiguracion
            txtFechaInicio.Text = String.Empty
            txtFechaTerminacion.Text = String.Empty
        End Try
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        If Not (IsDate(txtFechaInicio.Text)) Then
            ShowNotification("Debe llenar la fecha de Inicio antes de continuar", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            Exit Sub
        End If
        If Not (IsDate(txtFechaTerminacion.Text)) Then
            ShowNotification("Debe llenar la fecha de Finalización antes de continuar", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            Exit Sub
        End If
        If ddlTipoRecoleccion.SelectedIndex = -1 Then
            ShowNotification("Debe elegir el tipo de recolección antes de continuar", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            Exit Sub
        End If
        eTrabajoOP.FechaInicioCampo = txtFechaInicio.Text
        eTrabajoOP.FechaFinalCampo = txtFechaTerminacion.Text
        eTrabajoOP.TrabajoId = hfIdTrabajo.Value
        Dim oTrabajoOp As New TrabajoOPCuanti
        eTrabajoOP = oTrabajoOp.GuardarTrabajoConfiguracion(eTrabajoOP)
        oTrabajoOp.GuardarTipoRecoleccion(hfIdTrabajo.Value, ddlTipoRecoleccion.SelectedValue)
        ShowNotification("Información guardada correctamente", ShowNotifications.InfoNotification)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarTrabajos(Session("IDUsuario").ToString)
			lbtnVolver.PostBackUrl = "~/RE_GT/HomeRecoleccion.aspx"
			CargarTiposDeRecolección()
        End If
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Dim oTrabajo As New Trabajo
        gvTrabajos.DataSource = oTrabajo.obtenerXIdCOEXTodosCampos(Session("IDUsuario").ToString, txtBuscar.Text)
        gvTrabajos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
        gvTrabajos.PageIndex = e.NewPageIndex
        CargarTrabajos(Session("IDUsuario").ToString)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        If e.CommandName = "Actualizar" Then
            Dim oTrabajo As New Trabajo
            'cargarTrabajo(Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")))
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Dim oeTrabajo = oTrabajo.ObtenerTrabajo(hfIdTrabajo.Value)
            CargarConfiguracionTrabajo(hfIdTrabajo.Value)
			accordion0.Visible = False
			accordion1.Visible = True

			Dim oPlaneacion As New PlaneacionProduccion
            If oPlaneacion.ObtenerEstimacionCiudadxTrabajoList(hfIdTrabajo.Value).Count = 0 Then
                'btnEstimacionAuto.Visible = True
                'btnEstimaciones.Visible = False
            Else
                'btnEstimacionAuto.Visible = False
                'btnEstimaciones.Visible = True
            End If

            If oeTrabajo.MetCodigo >= 600 And oeTrabajo.MetCodigo <= 699 Then
                Me.btnSesiones.Visible = True
            Else
                Me.btnSesiones.Visible = False
            End If
            If oeTrabajo.MetCodigo >= 700 And oeTrabajo.MetCodigo <= 799 Then
                Me.btnEntrevistas.Visible = True
            Else
                Me.btnEntrevistas.Visible = False
            End If
            If oeTrabajo.MetCodigo >= 900 And oeTrabajo.MetCodigo <= 999 Then
                Me.btnInHome.Visible = True
            Else
                Me.btnInHome.Visible = False
            End If

        ElseIf e.CommandName = "Tareas" Then
            'Response.Redirect("\CORE\Tareas.aspx" & "?IdTrabajo=" & gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id") & "&Coe=" & Session("IDUsuario").ToString)
        End If
    End Sub

    Sub CargarTiposDeRecolección()
        Dim oTrabajoOP As New TrabajoOPCuanti
        Me.ddlTipoRecoleccion.DataSource = oTrabajoOP.ObtenerTecnicasRecoleccion
        Me.ddlTipoRecoleccion.DataValueField = "id"
        Me.ddlTipoRecoleccion.DataTextField = "Recoleccion"
        Me.ddlTipoRecoleccion.DataBind()
    End Sub

#End Region


    Protected Sub btnEstadoTareas_Click(sender As Object, e As EventArgs) Handles btnEstadoTareas.Click
        Response.Redirect("../CORE/Gestion-Tareas.aspx?IdTrabajo=" & hfIdTrabajo.Value & "&URLRetorno=" & UrlOriginal.OP_Cualitativo_Trabajos & "&IdUnidadEjecuta=" & UnidadesCore.COE & "&IdRolEstima=" & ListaRoles.COE)
    End Sub

    Protected Sub btnSegmentos_Click(sender As Object, e As EventArgs) Handles btnSegmentos.Click
        Response.Redirect("../PY_Proyectos/SegmentosCuali.aspx?trabajoId=" & hfIdTrabajo.Value)
    End Sub
    Protected Sub btnEntrevistas_Click(sender As Object, e As EventArgs) Handles btnEntrevistas.Click
        Response.Redirect("../PY_Proyectos/DistribucionEntrevistas.aspx?trabajoId=" & hfIdTrabajo.Value)
    End Sub
    Protected Sub btnSesiones_Click(sender As Object, e As EventArgs) Handles btnSesiones.Click
        Response.Redirect("../PY_Proyectos/Sesiones.aspx?trabajoId=" & hfIdTrabajo.Value)
    End Sub
    Protected Sub btnInHome_Click(sender As Object, e As EventArgs) Handles btnInHome.Click
        Response.Redirect("../PY_Proyectos/InHomeVisit.aspx?trabajoId=" & hfIdTrabajo.Value)
    End Sub
    Protected Sub btnFiltroReclutamiento_Click(sender As Object, e As EventArgs) Handles btnFiltroReclutamiento.Click
        Response.Redirect("../OP_Cualitativo/DisenarFiltros.aspx?trabajoId=" & hfIdTrabajo.Value & "&tipofiltro=1")
    End Sub
    Protected Sub btnFiltroAsistencia_Click(sender As Object, e As EventArgs) Handles btnFiltroAsistencia.Click
        Response.Redirect("../OP_Cualitativo/DisenarFiltros.aspx?trabajoId=" & hfIdTrabajo.Value & "&tipofiltro=2")
    End Sub

    Protected Sub btnFicha_Click(sender As Object, e As EventArgs) Handles btnFicha.Click
        Dim oTrabajo As New Trabajo
        Dim oeTrabajo As PY_Trabajos_Get_Result
        oeTrabajo = oTrabajo.obtenerXId(hfIdTrabajo.Value)

        If oeTrabajo.MetCodigo >= 600 And oeTrabajo.MetCodigo <= 699 Then
            Response.Redirect("../OP_Cualitativo/FichaSesion.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes")
        ElseIf oeTrabajo.MetCodigo >= 700 And oeTrabajo.MetCodigo <= 799 Then
            Response.Redirect("../OP_Cualitativo/FichaEntrevista.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes")
        ElseIf oeTrabajo.MetCodigo >= 900 And oeTrabajo.MetCodigo <= 999 Then
            Response.Redirect("../OP_Cualitativo/FichaObservacion.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes")
        End If
    End Sub

    Private Sub btnVariablesControl_Click(sender As Object, e As EventArgs) Handles btnVariablesControl.Click
        Response.Redirect("../PY_Proyectos/VariablesControl.aspx?idTr=" & hfIdTrabajo.Value & "&modal=GP")
    End Sub


End Class