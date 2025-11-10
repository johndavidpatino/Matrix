Imports CoreProject
Imports WebMatrix.Util
Imports System.IO
Imports System.Web.Hosting
Imports System.Net
Imports CoreProject.CIEntities
Imports CoreProject.OP
Imports System.Threading.Tasks

Public Class Trabajos1
    Inherits System.Web.UI.Page

#Region "Enumeradores"
    Enum eTipo
        Grabar = 1
        Actualizar = 2
    End Enum
    Enum eTipoGRupoUnidad
        Comercial = 1
        Operativa = 2
    End Enum
    Enum eGrupoUnidad
        GerenciaMysteryShopper = 21
        GerenciaTelefonico = 10
    End Enum
    Enum ERolResponsable
        GerenteProyectos = 6
    End Enum

    Enum eEstadoBloqueo
        Cerrado = 10
        Anulado = 11
    End Enum
#End Region
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

    Private _trabajoId As Int64
    Public Property trabajoId() As Int64
        Get
            Return _trabajoId
        End Get
        Set(ByVal value As Int64)
            _trabajoId = value
        End Set
    End Property

    Private _informacion As PY_Trabajo_Get_Result
    Public Property informacion() As PY_Trabajo_Get_Result
        Get
            Return _informacion
        End Get
        Set(ByVal value As PY_Trabajo_Get_Result)
            _informacion = value
        End Set
    End Property

#End Region

#Region "Metodos"
    Sub CargarTrabajosXIdProyecto(ByVal proyectoId As Int64)
        Dim oTrabajo As New Trabajo
        Dim id As Int64? = Nothing
        Dim nombre As String = Nothing
        Dim jobbook As String = Nothing
        Dim estado As Int32? = Nothing
        If IsNumeric(txtID.Text) Then id = txtID.Text
        If Not (txtNombreBuscar.Text = "") Then nombre = txtNombreBuscar.Text
        If Not (txtJobBook.Text = "") Then jobbook = txtJobBook.Text
        If Not (ddlEstado.SelectedValue = "-1") Then estado = ddlEstado.SelectedValue
        gvTrabajos.DataSource = oTrabajo.ListadoTrabajos(id, estado, nombre, jobbook, proyectoId, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        gvTrabajos.DataBind()
    End Sub

    Sub limpiar()
        txtNombreTrabajo.Text = ""
        txtMuestra.Text = ""
        txtFechaTentativaInicioCampo.Text = ""
        txtFechaTentativaFinalizacion.Text = ""
        hfIdTrabajo.Value = 0
        Me.pnlNewMuestra.Visible = False
        Me.pnlInfoTrabajo.Visible = False
        Me.pnlNewFecha.Visible = False
        Me.pnlNewTrabajo.Visible = False
        hfIdPropuesta.Value = ""
        hfIdAlternativa.Value = ""
        hfCodigoMetodologia.Value = ""
        hfIdFase.Value = ""
        hfIdMetodologia.Value = ""
        hfNoMediciones.Value = ""
        hfJobBook.Value = ""
        hfActualizar.Value = "0"
        Me.gvMuestraNew.DataBind()
        Me.gvOpcionesTrabajo.DataBind()
    End Sub
    Sub cargarTrabajo(ByVal idTrabajo As Int64)
        Dim oTrabajo As New Trabajo
        Dim oeTrabajo = oTrabajo.ObtenerTrabajo(idTrabajo)
        Dim oMetodologias As New MetodologiaOperaciones
        Dim daPropuesta As New Propuesta
        Dim oPropuesta = daPropuesta.DevolverxID(oeTrabajo.IdPropuesta)
        txtNombreTrabajo.Text = oeTrabajo.NombreTrabajo
        txtNombre.Text = oeTrabajo.NombreTrabajo
        Session("NombreTrabajo") = oeTrabajo.id.ToString & " | " & oeTrabajo.JobBook & " | " & oeTrabajo.NombreTrabajo
        txtMuestra.Text = oeTrabajo.Muestra
        txtMuestraNew.Text = oeTrabajo.Muestra
        txtMuestraNew.Text = oeTrabajo.Muestra
        txtMedicion.Text = oeTrabajo.NoMedicion
        txtNoMedicion.Text = oeTrabajo.NoMedicion
        txtFechaTentativaInicioCampo.Text = oeTrabajo.FechaTentativaInicioCampo
        txtFechaTentativaFinalizacion.Text = oeTrabajo.FechaTentativaFinalizacion
        txtMetodologia.Text = oMetodologias.obtenerXId(oeTrabajo.OP_MetodologiaId).MetNombre
        hfIdPropuesta.Value = oeTrabajo.IdPropuesta
        hfIdAlternativa.Value = oeTrabajo.Alternativa
        hfIdMetodologia.Value = oeTrabajo.OP_MetodologiaId
        hfCodigoMetodologia.Value = oeTrabajo.MetCodigo
        hfIdFase.Value = oeTrabajo.Fase
        hfJobBook.Value = oeTrabajo.JobBook
        hfNoMediciones.Value = oeTrabajo.NoMedicion
        hfInternacional.Value = oPropuesta.internacional
        lblEstadoTrabajo.Text = oTrabajo.ObtenerEstado(oeTrabajo.Estado)
        Me.pnlInfoTrabajo.Visible = True
        If oeTrabajo.Estado = 13 Then
            Me.btnAnularTrabajo.Visible = False
        End If
        If oeTrabajo.Estado = 12 Then
            Me.btnAnularTrabajo.Visible = False
            Me.btnCerrarTrabajo.Visible = False
            Me.btnReporteCierre.Visible = True
        End If
        If oeTrabajo.Estado = eEstadoBloqueo.Anulado Or oeTrabajo.Estado = eEstadoBloqueo.Cerrado Then
            Me.btnAnularTrabajo.Visible = False
            Me.btnCerrarTrabajo.Visible = False
        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub
    Sub lanzarTareas(ByVal idTrabajo As Int64, ByVal tipoHilo As Int64)
        Dim oWorkFlow As New WorkFlow
        oWorkFlow.CrearHiloCrearTareas(tipoHilo, idTrabajo)
    End Sub
    Function obtenerOPMetodologia(ByVal id As Int16) As OP_Metodologias_Get_Result
        Dim oMetodologiaOperaciones As New MetodologiaOperaciones
        Return oMetodologiaOperaciones.obtenerXId(id)
    End Function
    Function obtenerProyectoXId(ByVal id As Int64) As PY_Proyectos_Get_Result
        Dim oProyecto As New Proyecto
        Return oProyecto.obtenerXId(id)
    End Function
    Sub cargarProyectoXId(ByVal proyectoId As Int64)
        Dim oProyectos As New Proyecto
        Dim lstProyectos As New List(Of PY_Proyectos_Get_Result)
        lstProyectos.Add(oProyectos.obtenerXId(proyectoId))
    End Sub
    Sub adcionarTareasLogCreadas(ByVal trabajoId As Int64, ByVal fechaRegistro As DateTime, ByVal usuario As Int64, ByVal tipoHilo As Int64)
        Dim oLogWorkFlow As New LogWorkFlow
        oLogWorkFlow.CORE_Log_WorkFlow_MasivoEstadoCreada_Add(trabajoId, fechaRegistro, usuario, tipoHilo)
    End Sub
    Public Function ValidarFecha(ByVal txtFecha As String) As Boolean
        Dim dt As DateTime

        Dim blnFlag As Boolean = DateTime.TryParse(txtFecha, dt)

        If blnFlag Then
            Return True
        Else
            Return False
        End If
    End Function

    Sub EnviarEmail()
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            If String.IsNullOrEmpty(hfIdTrabajo.Value) Or hfIdTrabajo.Value = "0" Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar un anuncio de aprobación")
            End If
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/NuevoTrabajoGerenciaOp.aspx?idTrabajo=" & hfIdTrabajo.Value)
            'Dim script As String = "window.open('../Emails/NuevoTrabajoGerenciaOp.aspx?idTrabajo=" & hfIdTrabajo.Value & "','cal','width=400,height=250,left=270,top=180')"
            'Dim page As Page = DirectCast(Context.Handler, Page)
            'ScriptManager.RegisterStartupScript(page, GetType(Page), "Anuncio", script, True)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
#End Region

    Sub ConfigurarPryCuali(ByVal ProyectoId As Int64)
        Dim o As New Proyecto
        Dim pry = o.obtenerXId(ProyectoId)
        If pry.TipoProyectoId = 2 Then
            Me.btnSegmentos.Visible = True
            Me.btnROCuestionario.Visible = False
            Me.btnROInstructivo.Visible = False
            Me.btnROMaterialAyuda.Visible = False
            Me.btnROMetodologia.Visible = False
            Me.btnFichaCuanti.Visible = False
            Me.gvTrabajos.Columns(Me.gvTrabajos.Columns.Count - 2).Visible = False
            Me.pnlCOE.Visible = False
        Else
            btnROCuestionario.Visible = False
            btnROInstructivo.Visible = False
            btnROMaterialAyuda.Visible = False
            btnROMetodologia.Visible = False
            Me.btnSegmentos.Visible = False
            Me.pnlCOE.Visible = False
        End If
    End Sub
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim daCentro As New Trabajo

        If hfIdTrabajo.Value IsNot Nothing Then
            informacion = daCentro.DevolverxID(hfIdTrabajo.Value)
        End If

        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(97, UsuarioID) = False Then
            Response.Redirect("../PY_Proyectos/PY_Proyectos.aspx")
        End If

        idUsuario = Session("IdUsuario")

        If Request.QueryString("proyectoId") IsNot Nothing Then
            Long.TryParse(Request.QueryString("proyectoId"), proyectoId)
            hfIdProyecto.Value = proyectoId
            Session("ProyectoId") = hfIdProyecto.Value
        End If

        If Request.QueryString("trabajoId") IsNot Nothing Then
            Long.TryParse(Request.QueryString("trabajoId"), trabajoId)
        End If
        If Not Session("ProyectoId") = Nothing Then
            hfIdProyecto.Value = hfIdProyecto.Value
            proyectoId = Session("ProyectoId").ToString
        End If
        If Not IsPostBack Then
            If proyectoId > 0 Then
                If trabajoId > 0 Then
                    txtID.Text = trabajoId
                    CargarTrabajosXIdProyecto(proyectoId)
                Else
                    CargarTrabajosXIdProyecto(proyectoId)
                End If
                lblProyecto.Text = obtenerProyectoXId(proyectoId).Nombre.ToUpper
                txtNombreTrabajo.Text = obtenerProyectoXId(proyectoId).Nombre.ToUpper
                ConfigurarPryCuali(proyectoId)
                If Not Session("TrabajoId") = Nothing Then
                    hfIdTrabajo.Value = Session("TrabajoId").ToString
                    cargarTrabajo(Session("TrabajoId").ToString)
                End If
            Else
            End If
        End If
    End Sub
    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        NuevoTrabajo()
        cargarProyectoXId(proyectoId)
		accordion0.Visible = False
		accordion1.Visible = True
	End Sub

    Protected Sub Guardar()
        Dim oTrabajo As New Trabajo
        Dim oProyecto As New Proyecto
        Dim oeProyecto As New PY_Proyectos_Get_Result
        Dim oPlaneacion As New PlaneacionProduccion
        Dim oWorkFlow As New WorkFlow
        Dim oCoordCampo As New CoordinacionCampo

        Try

            Dim IQ As New IQ_MODEL
            oeProyecto = oProyecto.obtenerXId(hfIdProyecto.Value)
            Dim ent As New PY_Trabajo0
            If Not (hfIdTrabajo.Value = "0") Then
                ent = oTrabajo.ObtenerTrabajo(hfIdTrabajo.Value)
            End If
            ent.ProyectoId = hfIdProyecto.Value
            ent.OP_MetodologiaId = hfIdMetodologia.Value
            ent.JobBook = hfJobBook.Value
            ent.PresupuestoId = hfIdPresupuesto.Value
            ent.NombreTrabajo = txtNombreTrabajo.Text
            ent.Muestra = txtMuestra.Text
            Dim flagcambiofecha As Boolean = False
            If Not (hfIdTrabajo.Value = "0") Then
                If Not (ent.FechaTentativaInicioCampo = txtFechaTentativaInicioCampo.Text) Then
                    flagcambiofecha = True
                End If
            End If
            ent.FechaTentativaInicioCampo = txtFechaTentativaInicioCampo.Text
            ent.FechaTentativaFinalizacion = txtFechaTentativaFinalizacion.Text
            ent.Unidad = oeProyecto.UnidadId
            ent.IdPropuesta = hfIdPropuesta.Value
            ent.Alternativa = hfIdAlternativa.Value
            ent.MetCodigo = hfCodigoMetodologia.Value
            ent.Fase = hfIdFase.Value
            ent.NoMedicion = Convert.ToInt64(txtMedicion.Text)
            Me.hfDiasCampo.Value = IQ.IQ_DatosGeneralesPresupuesto.Where(Function(x) x.IdPropuesta = hfIdPropuesta.Value AndAlso x.ParAlternativa = hfIdAlternativa.Value).FirstOrDefault.DiasCampo
            ent.Estado = 1
            If Me.pnlCOE.Visible = True Then
                ent.COE = ddlCOE.SelectedValue
            End If
            If Not (hfIdTrabajo.Value = "0") Then
                hfIdTrabajo.Value = oTrabajo.GuardarTrabajo(ent)
                If flagcambiofecha = True Then
                    oPlaneacion.AgregarEstimacionAutomatica(hfIdTrabajo.Value, Session("IDUsuario").ToString, True, True, True, True, True, False, False, False)
                End If
            Else
                hfIdTrabajo.Value = oTrabajo.GuardarTrabajo(ent)
                If hfIdTrabajo.Value = 0 Then
                    Exit Sub
                End If
                Dim eTrabajoOP As New OP_TrabajoConfiguracion
                eTrabajoOP.FechaInicioCampo = txtFechaTentativaInicioCampo.Text
                eTrabajoOP.FechaFinalCampo = CDate(txtFechaTentativaInicioCampo.Text).Date.AddDays(hfDiasCampo.Value)
                eTrabajoOP.UnidadCritica = 28
                eTrabajoOP.TrabajoId = hfIdTrabajo.Value
                Dim oTrabajoOp As New TrabajoOPCuanti
                eTrabajoOP = oTrabajoOp.GuardarTrabajoConfiguracion(eTrabajoOP)
                If Me.pnlCOE.Visible = False Then
                    EnviarEmail()
                End If
                EnviarPreentrega()

                lanzarTareas(hfIdTrabajo.Value, obtenerOPMetodologia(hfIdMetodologia.Value).TipoHiloId)
                adcionarTareasLogCreadas(hfIdTrabajo.Value, DateTime.UtcNow.AddHours(-5), idUsuario, obtenerOPMetodologia(hfIdMetodologia.Value).TipoHiloId)


                Dim entNewMuestra As New List(Of IQ_MuestraInfoTrabajo_Result)
                entNewMuestra = Session("MuestraNew")
                For i As Integer = 0 To entNewMuestra.Count - 1
                    Dim EntMuestra As New OP_MuestraTrabajos
                    EntMuestra.CiudadId = entNewMuestra.Item(i).CiudadId
                    EntMuestra.TrabajoId = hfIdTrabajo.Value
                    EntMuestra.Cantidad = entNewMuestra.Item(i).Muestra
                    EntMuestra.FechaInicio = txtFechaTentativaInicioCampo.Text
                    EntMuestra.FechaFin = CDate(txtFechaTentativaInicioCampo.Text).Date.AddDays(hfDiasCampo.Value)
                    oCoordCampo.GuardarMuestraXEstudio(EntMuestra)
                Next
                oPlaneacion.GuardarEstimacionInicialOPTrafico(hfIdTrabajo.Value)
                oPlaneacion.AgregarEstimacionAutomatica(hfIdTrabajo.Value, Session("IDUsuario").ToString, True, True, True, True, True, False, False, False)

                If oeProyecto.TipoProyectoId = 1 Then
                    EnviarEmailCoordinadores()
                End If
                If Me.pnlCOE.Visible = True Then
                    EnviarEmailCoeAsignado()
                End If
            End If
            CargarTrabajosXIdProyecto(hfIdProyecto.Value)
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
        Catch ex As Exception
            ShowNotification("Ha ocurrido un error al intentar ingresar el registro - " & ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            Exit Sub
        End Try
    End Sub


    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Dim oTrabajo As New Trabajo
        Dim id As Int64? = Nothing
        Dim nombre As String = Nothing
        Dim jobbook As String = Nothing
        Dim estado As Int32? = Nothing
        If IsNumeric(txtID.Text) Then id = txtID.Text
        If Not (txtNombreBuscar.Text = "") Then nombre = txtNombreBuscar.Text
        If Not (txtJobBook.Text = "") Then jobbook = txtJobBook.Text
        If Not (ddlEstado.SelectedValue = "-1") Then estado = ddlEstado.SelectedValue
        gvTrabajos.DataSource = oTrabajo.ListadoTrabajos(id, estado, nombre, jobbook, proyectoId, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        gvTrabajos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
        gvTrabajos.PageIndex = e.NewPageIndex
        CargarTrabajosXIdProyecto(proyectoId)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        If e.CommandName = "Actualizar" Then
            Me.pnlInfoTrabajo.Visible = False
            Me.pnlNewFecha.Visible = False
            Me.pnlNewMuestra.Visible = False
            Me.pnlAnulacion.Visible = False
            Me.pnlCierre.Visible = False
            Me.PnlMuestra.Visible = False
            Me.pnlNewTrabajo.Visible = False
            cargarTrabajo(Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")))
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Session("TrabajoId") = hfIdTrabajo.Value
            accordion0.Visible = False
            accordion1.Visible = True
        ElseIf e.CommandName = "Tareas" Then
            Response.Redirect("\CORE\Tareas.aspx" & "?IdTrabajo=" & gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id") & "&IdProyecto=" & proyectoId)
        ElseIf e.CommandName = "Avance" Then
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Response.Redirect("../RP_Reportes/AvanceDeCampo.aspx?TrabajoId=" & hfIdTrabajo.Value)
        ElseIf e.CommandName = "Project" Then
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Response.Redirect("../RP_Reportes/GanttUnTrabajo.aspx?TrabajoId=" & hfIdTrabajo.Value)
        ElseIf e.CommandName = "Duplicar" Then

        End If
    End Sub

    Private Sub gvReporte_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReporte.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim data = CType(e.Row.DataItem, GD_EscanerDocumentos_Get_Result)
            If data.CodEncontrado = True OrElse (data.RolResponsableCierre <> 6 AndAlso hfInternacional.Value = False) OrElse data.Observacion <> "" Then
                DirectCast(e.Row.FindControl("txtobservacion"), TextBox).Visible = False
            Else
                DirectCast(e.Row.FindControl("lblobservacion"), Label).Visible = False
            End If
        End If
    End Sub

    Protected Sub gvReporte_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvReporte.RowUpdating
        Dim oGDActualizar As New GD.GD_Procedimientos
        Dim fila As GridViewRow = gvReporte.Rows(e.RowIndex)
        Dim oid As Label = CType(fila.FindControl("lblid"), Label)
        Dim otxtobservacion As TextBox = CType(fila.FindControl("txtobservacion"), TextBox)
        oGDActualizar.ActualizarObservacion(oid.Text, otxtobservacion.Text)
        gvReporte.EditIndex = -1
        CargarGrid()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub



    Protected Sub btnFichaCuanti_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFichaCuanti.Click
        Try
            If String.IsNullOrEmpty(hfIdTrabajo.Value) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar una preentrega")
            End If
            Dim oTrabajo As New Trabajo
            Dim oeTrabajo As PY_Trabajos_Get_Result
            oeTrabajo = oTrabajo.obtenerXId(hfIdTrabajo.Value)
            Response.Redirect("../OP_Cuantitativo/FichaCuantitativa.aspx?idtrabajo=" & hfIdTrabajo.Value)
            'Dim oTipoFicha As New MetodologiaOperaciones
            'Dim tipo As Int32 = oTipoFicha.ObtenerFichaMetodologiaxId(oeTrabajo.OP_MetodologiaId).Ficha
            'Select Case tipo
            '    Case TipoFicha.Cuanti
            '        Response.Redirect("../OP_Cuantitativo/FichaCuantitativa.aspx?idtrabajo=" & hfIdTrabajo.Value)
            '    Case TipoFicha.Sesiones
            '        Response.Redirect("../OP_Cualitativo/FichaSesion.aspx?idtrabajo=" & hfIdTrabajo.Value)
            '    Case TipoFicha.Observaciones
            '        Response.Redirect("../OP_Cualitativo/FichaObservacion.aspx?idtrabajo=" & hfIdTrabajo.Value)
            '    Case TipoFicha.Entrevistas
            '        Response.Redirect("../OP_Cualitativo/FichaEntrevista.aspx?idtrabajo=" & hfIdTrabajo.Value)
            'End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try


    End Sub
    Protected Sub btnPreEntrega_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPreEntrega.Click
        EnviarPreentrega()
    End Sub

    Sub EnviarPreentrega()
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            If String.IsNullOrEmpty(hfIdTrabajo.Value) Or (hfIdTrabajo.Value = "0") Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar una preentrega")
            End If
            'Dim script As String = "window.open('../Emails/PreEntregaTrabajo.aspx?idTrabajo=" & hfIdTrabajo.Value & "','cal','width=400,height=250,left=270,top=180')"
            'Dim page As Page = DirectCast(Context.Handler, Page)
            'ScriptManager.RegisterStartupScript(page, GetType(Page), "Preentrega", script, True)
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/PreEntregaTrabajo.aspx?idTrabajo=" & hfIdTrabajo.Value)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub btnROCuestionario_Click(sender As Object, e As EventArgs) Handles btnROCuestionario.Click
        Response.Redirect("../OP_RO/Cuestionario.aspx?idtrabajo=" & hfIdTrabajo.Value & "&fromgerencia=1")
    End Sub

    Protected Sub btnROInstructivo_Click(sender As Object, e As EventArgs) Handles btnROInstructivo.Click
        Response.Redirect("../OP_RO/Instructivo.aspx?idtrabajo=" & hfIdTrabajo.Value & "&fromgerencia=1")
    End Sub

    Protected Sub btnROMaterialAyuda_Click(sender As Object, e As EventArgs) Handles btnROMaterialAyuda.Click
        Response.Redirect("../OP_RO/MaterialAyuda.aspx?idtrabajo=" & hfIdTrabajo.Value & "&fromgerencia=1")
    End Sub

    Protected Sub btnROMetodologia_Click(sender As Object, e As EventArgs) Handles btnROMetodologia.Click
        Response.Redirect("../OP_RO/Metodologia.aspx?idtrabajo=" & hfIdTrabajo.Value & "&fromgerencia=1")
    End Sub


    Protected Sub btnDocumentos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDocumentos.Click
        If Not (hfIdTrabajo.Value = "0") Then
            ''Response.Redirect("\GD_Documentos\GD_Documentos.aspx?IdContenedor=" & hfIdTrabajo.Value & "&IdDocumento=6")
            Response.Redirect("\GD_Documentos\GD_Documentos.aspx?IdContenedor=" & proyectoId & "&IdDocumento=6")
        Else : ShowNotification("Guarde Primero el Proyecto", ShowNotifications.ErrorNotification)
        End If
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub


    Protected Sub btnEstadoTareas_Click(sender As Object, e As EventArgs) Handles btnEstadoTareas.Click
        Response.Redirect("../CORE/Gestion-Tareas.aspx?IdTrabajo=" & hfIdTrabajo.Value & "&URLRetorno=" & UrlOriginal.PY_Proyectos_Trabajos & "&IdUnidadEjecuta=" & UnidadesCore.Proyectos & "&IdRolEstima=6")
    End Sub
#End Region

    Protected Sub btnSegmentos_Click(sender As Object, e As EventArgs) Handles btnSegmentos.Click
        Response.Redirect("SegmentosCuali.aspx?trabajoId=" & hfIdTrabajo.Value & "&py=true")
    End Sub

    Protected Sub btnCircular_Click(sender As Object, e As EventArgs) Handles btnCircular.Click
        If hfIdTrabajo.Value = "" Then Exit Sub
        Dim o As New WorkFlow
        Dim oS As New CoreProject.SegmentosCuali
        Dim idTrabajo As Int64 = hfIdTrabajo.Value
        Dim idWF As Int64 = o.ObtenerWorkflowXTrabajoXTarea(idTrabajo, 3).FirstOrDefault.id
        Response.Redirect("\GD_Documentos\GD_Documentos.aspx?IdContenedor=" & idTrabajo & "&IdDocumento=13&TipoContenedor=1&IdWorkFlow=" & idWF)
    End Sub

    Sub NuevoTrabajo()
        CargarInfoTrabajo()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Sub CargarInfoTrabajo()
        Me.pnlInfoTrabajo.Visible = False
        Me.pnlNewFecha.Visible = False
        Me.pnlNewMuestra.Visible = False
        Me.pnlAnulacion.Visible = False
        Me.pnlCierre.Visible = False
        Me.PnlMuestra.Visible = False
        Me.pnlNewTrabajo.Visible = False
        Me.hfIdTrabajo.Value = "0"
        Me.pnlNewTrabajo.Visible = True
        Dim o As New Trabajo
        Me.gvOpcionesTrabajo.DataSource = o.ObtenerInfoNuevoTrabajo(hfIdProyecto.Value)
        Me.gvOpcionesTrabajo.DataBind()
    End Sub

    Sub CargarMuestraNew()
        Dim o As New IQ.IquoteGeneral
        Dim entNewMuestra As List(Of IQ_MuestraInfoTrabajo_Result)
        entNewMuestra = o.MuestraInfoTrabajo(hfIdPropuesta.Value, hfIdAlternativa.Value, hfCodigoMetodologia.Value, hfIdFase.Value)
        Session("MuestraNew") = entNewMuestra
    End Sub

    Sub CargarGridMuestraNew()
        Me.gvMuestraNew.DataSource = Session("MuestraNew")
        Me.gvMuestraNew.DataBind()
    End Sub

    Private Sub gvOpcionesTrabajo_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvOpcionesTrabajo.RowCommand
        If e.CommandName = "Usar" Then
            hfIdPropuesta.Value = Int64.Parse(Me.gvOpcionesTrabajo.DataKeys(CInt(e.CommandArgument))("IdPropuesta"))
            hfIdAlternativa.Value = Int32.Parse(Me.gvOpcionesTrabajo.DataKeys(CInt(e.CommandArgument))("ParAlternativa"))
            hfCodigoMetodologia.Value = Int32.Parse(Me.gvOpcionesTrabajo.DataKeys(CInt(e.CommandArgument))("MetCodigo"))
            hfIdFase.Value = Int32.Parse(Me.gvOpcionesTrabajo.DataKeys(CInt(e.CommandArgument))("ParNacional"))
            hfIdMetodologia.Value = Int32.Parse(Me.gvOpcionesTrabajo.DataKeys(CInt(e.CommandArgument))("IdMetodologia"))
            hfNoMediciones.Value = Int32.Parse(Me.gvOpcionesTrabajo.DataKeys(CInt(e.CommandArgument))("NumeroMediciones"))
            hfJobBook.Value = Me.gvOpcionesTrabajo.DataKeys(CInt(e.CommandArgument))("JoBBook")
            hfDiasCampo.Value = Me.gvOpcionesTrabajo.DataKeys(CInt(e.CommandArgument))("DiasCampo")
            Me.pnlNewMuestra.Visible = True
            Me.pnlInfoTrabajo.Visible = False
            Me.pnlNewFecha.Visible = False
            Me.pnlAnulacion.Visible = False
            Me.pnlCierre.Visible = False
            Me.PnlMuestra.Visible = False
            Me.pnlNewTrabajo.Visible = False
            CargarMuestraNew()
            CargarGridMuestraNew()
            CargarDepartamentos()
            CargarCiudades()
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        End If
    End Sub

    Private Sub gvMuestraNew_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMuestraNew.RowCommand
        If e.CommandName = "Quitar" Then
            Dim idciudad As Int32 = Int32.Parse(Me.gvMuestraNew.DataKeys(CInt(e.CommandArgument))("CiudadId"))
            Dim entNewMuestra As List(Of IQ_MuestraInfoTrabajo_Result)
            Dim itmEnc As New IQ_MuestraInfoTrabajo_Result
            entNewMuestra = Session("MuestraNew")
            For Each itm As IQ_MuestraInfoTrabajo_Result In entNewMuestra
                If itm.CiudadId = idciudad Then
                    itmEnc = itm
                End If
            Next
            entNewMuestra.Remove(itmEnc)
            Session("MuestraNew") = entNewMuestra
            CargarGridMuestraNew()
        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub btnAddMuestraNew_Click(sender As Object, e As System.EventArgs) Handles btnAddMuestraNew.Click

        If (ddlCiudadNew.SelectedValue = "-1") Then
            ShowNotification("Debe seleccionar una ciudad", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(txtMuestraNew.Text) OrElse Integer.Parse(txtMuestraNew.Text) <= 0 Then
            ShowNotification("El valor ingresado en la cantidad de la muestra no es valido!", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        Dim entNewMuestra As List(Of IQ_MuestraInfoTrabajo_Result)
        entNewMuestra = Session("MuestraNew")
        Dim flag As Boolean = False
        For Each itm As IQ_MuestraInfoTrabajo_Result In entNewMuestra
            If itm.CiudadId = ddlCiudadNew.SelectedValue Then
                flag = True
            End If
        Next
        If flag = True Then
            ShowNotification("Esta ciudad ya está adicionada en la muestra", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If
        Dim itemmuestra As New IQ_MuestraInfoTrabajo_Result
        itemmuestra.CiudadId = ddlCiudadNew.SelectedValue
        itemmuestra.Ciudad = ddlCiudadNew.SelectedItem.Text
        itemmuestra.Muestra = txtMuestraNew.Text
        entNewMuestra.Add(itemmuestra)
        Session("MuestraNew") = entNewMuestra
        CargarGridMuestraNew()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        ddlCiudadNew.SelectedIndex = -1
        ddlDepartamentoNew.SelectedIndex = -1
        txtMuestraNew.Text = ""
    End Sub

    Sub CargarDepartamentos()
        Dim oCoordCampo As New CoordinacionCampo
        Dim list = (From ldep In oCoordCampo.ObtenerDepartamentos()
                  Select iddep = ldep.DivDeptoMunicipio, nomdep = ldep.DivDeptoNombre).Distinct.ToList
        ddlDepartamentoNew.DataSource = list
        ddlDepartamentoNew.DataValueField = "iddep"
        ddlDepartamentoNew.DataTextField = "nomdep"
        ddlDepartamentoNew.DataBind()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub
    Sub CargarCiudades()
        ddlCiudadNew.Items.Clear()

        Dim oCoordCampo As New CoordinacionCampo
        Dim listciudades = (From ldep In oCoordCampo.ObtenerCiudades(ddlDepartamentoNew.SelectedValue)
                            Select idmuni = ldep.DivMunicipio, nommuni = ldep.DivMuniNombre)
        ddlCiudadNew.DataSource = listciudades
        ddlCiudadNew.DataValueField = "idmuni"
        ddlCiudadNew.DataTextField = "nommuni"
        ddlCiudadNew.DataBind()
        ddlCiudadNew.Items.Insert(0, New ListItem With {.Value = -1, .Text = "--Seleccione--"})
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub ddlDepartamentoNew_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlDepartamentoNew.SelectedIndexChanged
        CargarCiudades()
    End Sub

    Private Sub btnContinuarStep2_Click(sender As Object, e As System.EventArgs) Handles btnContinuarStep2.Click
        Dim muestra As Integer = 0
        For Each row As GridViewRow In gvMuestraNew.Rows
            If Not (IsNumeric(DirectCast(row.FindControl("txtMuestra"), TextBox).Text)) Then
                ShowNotification("Los datos de muestra deben ser numéricos y no puede haber vacíos", ShowNotifications.ErrorNotification)
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Exit Sub
            Else
                muestra = muestra + CInt(DirectCast(row.FindControl("txtMuestra"), TextBox).Text)
            End If
        Next
        Dim entNewMuestra As New List(Of IQ_MuestraInfoTrabajo_Result)
        For Each row As GridViewRow In gvMuestraNew.Rows
            entNewMuestra.Add(New IQ_MuestraInfoTrabajo_Result With {.CiudadId = gvMuestraNew.DataKeys(row.RowIndex).Value, .Ciudad = gvMuestraNew.Rows(row.RowIndex).Cells(0).Text, .Muestra = CInt(DirectCast(row.FindControl("txtMuestra"), TextBox).Text)})
        Next
        Session("MuestraNew") = entNewMuestra
        txtMuestra.Text = muestra
        txtNoMedicion.Text = hfNoMediciones.Value
        Me.pnlNewMuestra.Visible = False
        Me.pnlNewFecha.Visible = True
        If Me.pnlCOE.Visible = True Then
            CargarCOEs()
        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub btnCancelStep1_Click(sender As Object, e As System.EventArgs) Handles btnCancelStep1.Click
        limpiar()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub btnCancelStep2_Click(sender As Object, e As System.EventArgs) Handles btnCancelStep2.Click
        limpiar()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub btnCancelStep3_Click(sender As Object, e As System.EventArgs) Handles btnCancelStep3.Click
        limpiar()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnContinuarStep3_Click(sender As Object, e As EventArgs) Handles btnContinuarStep3.Click
        Dim daTrabajo As New Trabajo
        Dim Trabajo As PY_Trabajos_Get_Result


        If String.IsNullOrEmpty(txtFechaTentativaInicioCampo.Text) Or String.IsNullOrEmpty(txtFechaTentativaFinalizacion.Text) Or String.IsNullOrEmpty(txtNoMedicion.Text) Then
            ShowNotification("Las fechas y número de medición no pueden venir vacías", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If
        If ddlCOE.SelectedValue = "-1" Then
            ShowNotification("Seleccione el COE antes de continuar", ShowNotifications.ErrorNotification)
            Exit Sub
        End If
        If Not String.IsNullOrEmpty(txtFechaTentativaInicioCampo.Text) Then
            'Validar formato de la fecha
            If Not ValidarFecha(txtFechaTentativaInicioCampo.Text) Then
                ShowNotification("Fecha de inicio no válida", ShowNotifications.ErrorNotification)
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Exit Sub
            End If
        End If

        If Not String.IsNullOrEmpty(txtFechaTentativaFinalizacion.Text) Then
            'Validar formato de la fecha
            If ValidarFecha(txtFechaTentativaFinalizacion.Text) Then
                If Date.Parse(txtFechaTentativaFinalizacion.Text) < Date.Parse(txtFechaTentativaInicioCampo.Text) Then
                    ShowNotification("La Fecha de inicio no debe ser mayor a la de finalización", ShowNotifications.ErrorNotification)
                    ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                    Exit Sub
                End If
            Else
                ShowNotification("Fecha de finalización no válida", ShowNotifications.ErrorNotification)
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Exit Sub
            End If
        End If
        If hfActualizar.Value = "0" And daTrabajo.existeNombreTrabajo(txtNombreTrabajo.Text) Then
            ShowNotification("Este nombre de trabajo ya se uso en otro trabajo", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If
        Try
            Dim NoMedicion = Convert.ToInt32(txtNoMedicion.Text)
            txtNoMedicion.Text = NoMedicion
            txtMedicion.Text = NoMedicion
        Catch ex As Exception
            ShowNotification("La medición debe ser un número válido", ShowNotifications.ErrorNotification)
            txtNoMedicion.Focus()
            Exit Sub
        End Try
        Guardar()
        If hfIdTrabajo.Value <> 0 Then
            Trabajo = daTrabajo.obtenerXId(hfIdTrabajo.Value)
            daTrabajo.CambioEstado(hfIdTrabajo.Value, 1, "Creado", Session("IDUsuario").ToString)
            'Task(Of Boolean).Factory.StartNew(Function() crearCarpetas(Trabajo.id, Trabajo.JobBook, Trabajo.Unidad, Trabajo.GrupoUnidad, Trabajo.NombreTrabajo, Trabajo.OP_MetodologiaId))

            limpiar()
            Me.pnlInfoTrabajo.Visible = True
            Me.pnlNewFecha.Visible = False
            Me.btnContinuarStep3.Text = "Crear Trabajo"
            Me.btnCancelStep3.Visible = True
            Me.btnCancelCambioInfo.Visible = False
        End If
    End Sub

    Sub EnviarEmailCoordinadores()
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            If String.IsNullOrEmpty(hfIdTrabajo.Value) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de continuar")
            End If
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/NuevoTrabajoCoordCampo.aspx?idTrabajo=" & hfIdTrabajo.Value)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End Try
        'Response.Redirect("../OP_Cuantitativo/Trabajos.aspx")
    End Sub

    Sub EnviarEmailCambioEstado()
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            If String.IsNullOrEmpty(hfIdTrabajo.Value) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de continuar")
            End If
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/CambioEstadoTrabajo.aspx?idTrabajo=" & hfIdTrabajo.Value)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub

    Sub CargarMuestra(ByVal TrabajoId As Int64)
        Dim oCoorCampo As New CoordinacionCampo
        Dim listaMuestra = (From lmuestra In oCoorCampo.ObtenerMuestraxEstudioList(TrabajoId)
                            Select idMuestra = lmuestra.Id, departamento = lmuestra.C_Divipola.DivDeptoNombre, ciudad = lmuestra.C_Divipola.DivMuniNombre,
                            cantidad = lmuestra.Cantidad).OrderBy(Function(x) x.ciudad)
        gvMuestra.DataSource = listaMuestra.ToList
        gvMuestra.DataBind()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Sub CargarDepartamentosMuestra()
        Dim oCoordCampo As New CoordinacionCampo
        Dim list = (From ldep In oCoordCampo.ObtenerDepartamentos()
                  Select iddep = ldep.DivDeptoMunicipio, nomdep = ldep.DivDeptoNombre).Distinct.ToList
        ddlDepartamento.DataSource = list
        ddlDepartamento.DataValueField = "iddep"
        ddlDepartamento.DataTextField = "nomdep"
        ddlDepartamento.DataBind()
    End Sub
    Sub CargarCiudadesMuestra()
        Dim oCoordCampo As New CoordinacionCampo
        Dim listciudades = (From ldep In oCoordCampo.ObtenerCiudades(ddlDepartamento.SelectedValue)
                  Select idmuni = ldep.DivMunicipio, nommuni = ldep.DivMuniNombre)
        ddlCiudad.DataSource = listciudades
        ddlCiudad.DataValueField = "idmuni"
        ddlCiudad.DataTextField = "nommuni"
        ddlCiudad.DataBind()
    End Sub

    Sub CalcularMuestra()
        Dim muestra As Long
        If gvMuestra.Rows.Count = 0 Then
            muestra = 0
        Else
            For Each row As GridViewRow In Me.gvMuestra.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    If IsNumeric(DirectCast(row.FindControl("txtMuestra"), TextBox).Text) Then
                        muestra = muestra + CInt(DirectCast(row.FindControl("txtMuestra"), TextBox).Text)
                    End If
                End If
            Next
        End If
        Dim ent As New PY_Trabajo0
        Dim oTrabajo As New Trabajo
        ent = oTrabajo.ObtenerTrabajo(hfIdTrabajo.Value)
        ent.Muestra = muestra
        oTrabajo.GuardarTrabajo(ent)
        txtMuestra.Text = muestra
        txtMuestraNew.Text = muestra
    End Sub
    Private Sub gvMuestra_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvMuestra.PageIndexChanging
        gvMuestra.PageIndex = e.NewPageIndex
        CargarMuestra(hfIdTrabajo.Value)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvMuestra_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMuestra.RowCommand
        If e.CommandName = "Eliminar" Then
            Dim idMuestra As Int64 = Int64.Parse(Me.gvMuestra.DataKeys(CInt(e.CommandArgument))("idMuestra"))
            Dim oCoordCampo As New CoordinacionCampo
            oCoordCampo.EliminarMuestraXEstudio(idMuestra)
            CargarMuestra(hfIdTrabajo.Value)
            CalcularMuestra()
            ShowNotification("Registro Eliminado correctamente", ShowNotifications.InfoNotification)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        ElseIf e.CommandName = "Actualizar" Then
            Dim idMuestra As Int64 = Int64.Parse(Me.gvMuestra.DataKeys(CInt(e.CommandArgument))("idMuestra"))
            Dim oCoordCampo As New CoordinacionCampo
            If Not (IsNumeric(DirectCast(Me.gvMuestra.Rows(CInt(e.CommandArgument)).FindControl("txtMuestra"), TextBox).Text)) Or DirectCast(Me.gvMuestra.Rows(CInt(e.CommandArgument)).FindControl("txtMuestra"), TextBox).Text = "0" Then
                ShowNotification("La muestra debe ser numérica y mayor que cero", ShowNotifications.ErrorNotification)
                ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
                Exit Sub
            End If
            Dim valormuestra As Integer = DirectCast(Me.gvMuestra.Rows(CInt(e.CommandArgument)).FindControl("txtMuestra"), TextBox).Text
            Dim infomuestra = oCoordCampo.ObtenerMuestraxId(idMuestra)
            If infomuestra.Cantidad <> valormuestra Then
                infomuestra.Cantidad = valormuestra
                oCoordCampo.ActualizarMuestra(infomuestra)
            End If
            CalcularMuestra()
            CargarMuestra(hfIdTrabajo.Value)
            ShowNotification("Registro Actualizado correctamente", ShowNotifications.InfoNotification)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        End If
    End Sub
    Protected Sub ddlDepartamento_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlDepartamento.SelectedIndexChanged
        If ddlDepartamento.SelectedValue = "" Then Exit Sub
        CargarCiudadesMuestra()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub
    Protected Sub btnAddMuestra_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddMuestra.Click
        Dim oCoordCampo As New CoordinacionCampo
        Dim Ent As New OP_MuestraTrabajos
        If Not (IsNumeric(tbCantidad.Text)) Then
            ShowNotification("La cantidad debe ser numérica", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If
        Ent.CiudadId = ddlCiudad.SelectedValue
        Ent.TrabajoId = hfIdTrabajo.Value
        Ent.Cantidad = tbCantidad.Text
        oCoordCampo.GuardarMuestraXEstudio(Ent)
        CargarMuestra(hfIdTrabajo.Value)
        ddlDepartamento.ClearSelection()
        ddlCiudad.ClearSelection()
        Me.tbCantidad.Text = String.Empty
        CalcularMuestra()
        EnviarEmailCoordinadores()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnMuestra_Click(sender As Object, e As EventArgs) Handles btnMuestra.Click
        If Me.PnlMuestra.Visible = True Then
            Me.PnlMuestra.Visible = False
        Else
            Me.PnlMuestra.Visible = True
            CargarMuestra(hfIdTrabajo.Value)
            CargarDepartamentosMuestra()
        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnCambiarInfo_Click(sender As Object, e As EventArgs) Handles btnCambiarInfo.Click
        hfActualizar.Value = "1"
        Me.pnlInfoTrabajo.Visible = False
        Me.pnlNewFecha.Visible = True
        Me.btnContinuarStep3.Text = "Actualizar"
        Me.btnCancelStep3.Visible = False
        Me.btnCancelCambioInfo.Visible = True
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnCancelCambioInfo_Click(sender As Object, e As EventArgs) Handles btnCancelCambioInfo.Click
        Me.pnlInfoTrabajo.Visible = True
        Me.pnlNewFecha.Visible = False
        Me.btnCancelStep3.Visible = True
        Me.btnCancelCambioInfo.Visible = False
        Me.btnCancelStep3.Text = "Crear trabajo"
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnAnularTrabajo_Click(sender As Object, e As EventArgs) Handles btnAnularTrabajo.Click
        Me.pnlAnulacion.Visible = True
        Me.pnlInfoTrabajo.Visible = False
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnCerrarTrabajo_Click(sender As Object, e As EventArgs) Handles btnCerrarTrabajo.Click

        Dim oGD As New GD.GD_Procedimientos
        Dim oTrabajo As New Trabajo
        Dim idTrabajo As Integer = informacion.id
        Dim oEstados = oTrabajo.ListadoTrabajos(idTrabajo, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault

        If (oEstados.Estado <> 2 AndAlso oEstados.Estado <> 13) AndAlso hfInternacional.Value = False Then 'Cerrado en operaciones
            ShowNotification("Debe estar cerrado primero en operaciones, para poderlo cerrar en proyectos, ¡comuniquese con el COE!", ShowNotifications.ErrorNotification)
            Exit Sub
        Else
            If oEstados.Estado <> 13 Then
                oTrabajo.CambioEstado(idTrabajo, 13, "Trabajo en proceso de Cierre", Session("IDUsuario").ToString)
            End If
        End If
        EscanerArchivos()


        Dim oEscaner = oGD.DevolverxEncontrado(idTrabajo)
        If oEscaner.Count > 0 Then
            Me.lblforzar.Visible = True
            Me.btnForzarCierre.Visible = True
            Me.btnActualizarCierre.Visible = True
        Else
            Me.btnConfirmarCierre.Visible = True
        End If
        Me.lblInfoCierre.Visible = True
        Me.lblobservaciones.Visible = True
        Me.txtObservacionesCierre.Visible = True
        Me.btnCancelarCierre.Visible = True
        Me.pnlCierre.Visible = True
        Me.pnlInfoTrabajo.Visible = False
        CargarGrid()
        Me.lblreporte.Visible = True
        Me.gvReporte.Visible = True
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnReporteCierre_Click(sender As Object, e As EventArgs) Handles btnReporteCierre.Click
        Me.pnlCierre.Visible = True
        Me.pnlInfoTrabajo.Visible = False
        Me.lblforzar.Visible = False
        Me.lblobservaciones.Visible = False
        Me.txtObservacionesCierre.Visible = False
        Me.btnConfirmarCierre.Visible = False
        Me.btnForzarCierre.Visible = False
        Me.btnCancelarCierre.Visible = False
        Me.btnActualizarCierre.Visible = False
        CargarGrid()
        Me.lblreporte.Visible = True
        Me.gvReporte.Visible = True
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnActualizarCierre_Click(sender As Object, e As EventArgs) Handles btnActualizarCierre.Click
        Dim oGD As New GD.GD_Procedimientos

        Me.pnlCierre.Visible = True
        Me.pnlInfoTrabajo.Visible = False
        EscanerArchivos()

        Dim oEscaner = oGD.DevolverxEncontrado(hfIdTrabajo.Value)
        If oEscaner.Count > 0 Then
            Me.lblforzar.Visible = True
            Me.btnForzarCierre.Visible = True
            Me.btnActualizarCierre.Visible = True
            Me.btnConfirmarCierre.Visible = False
        Else
            Me.btnConfirmarCierre.Visible = True
            Me.lblforzar.Visible = False
            Me.btnForzarCierre.Visible = False
            Me.btnActualizarCierre.Visible = False
        End If

        CargarGrid()
        Me.lblreporte.Visible = True
        Me.gvReporte.Visible = True
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnCancelarAnulacion_Click(sender As Object, e As EventArgs) Handles btnCancelarAnulacion.Click
        Me.pnlInfoTrabajo.Visible = True
        Me.txtObservacionesAnulacion.Text = ""
        Me.pnlAnulacion.Visible = False
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnConfirmarAnulacion_Click(sender As Object, e As EventArgs) Handles btnConfirmarAnulacion.Click
        Dim o As New Trabajo
        o.CambioEstado(hfIdTrabajo.Value, 11, txtObservacionesAnulacion.Text, Session("IDUsuario").ToString)
        CargarTrabajosXIdProyecto(hfIdProyecto.Value)
        Me.txtObservacionesAnulacion.Text = ""
        Me.pnlAnulacion.Visible = False
        EnviarEmailCambioEstado()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        accordion0.Visible = True
        accordion1.Visible = False
        hfIdTrabajo.Value = 0
    End Sub

    Protected Sub btnConfirmarCierre_Click(sender As Object, e As EventArgs) Handles btnConfirmarCierre.Click
        Dim o As New Trabajo
        Dim oCI As New CentroInformacion
        Dim daTrabajo As New Trabajo
        Dim oTrabajo = daTrabajo.DevolverxID(hfIdTrabajo.Value)
        o.CambioEstado(hfIdTrabajo.Value, 12, txtObservacionesCierre.Text, Session("IDUsuario").ToString)
        CargarTrabajosXIdProyecto(hfIdProyecto.Value)
        'copiarArchivosRecuperacion(oTrabajo.GrupoUnidad, oTrabajo.id, oTrabajo.NombreTrabajo)
        oCI.GuardarLogCierres(hfIdTrabajo.Value, Session("IDUsuario").ToString, txtObservacionesCierre.Text, False)
        Me.pnlInfoTrabajo.Visible = True
        Me.txtObservacionesCierre.Text = ""
        Me.pnlCierre.Visible = False
        EnviarEmailCambioEstado()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        accordion0.Visible = True
        accordion1.Visible = False
        hfIdTrabajo.Value = 0
    End Sub

    Protected Sub btnForzarCierre_Click(sender As Object, e As EventArgs) Handles btnForzarCierre.Click
        Dim o As New Trabajo
        Dim oCI As New CentroInformacion
        Dim oGDActualizar As New GD.GD_Procedimientos
        Dim daTrabajo As New Trabajo
        Dim oTrabajo = daTrabajo.DevolverxID(hfIdTrabajo.Value)

        For fila = 0 To gvReporte.Rows.Count - 1
            If gvReporte.Rows(fila).Cells(3).Text = "NO" AndAlso DirectCast(gvReporte.Rows(fila).FindControl("txtobservacion"), TextBox).Visible Then
                oGDActualizar.ActualizarObservacion(gvReporte.DataKeys(fila)("Id"), DirectCast(gvReporte.Rows(fila).FindControl("txtobservacion"), TextBox).Text)
            End If
        Next

        'copiarArchivosRecuperacion(oTrabajo.GrupoUnidad, oTrabajo.id, oTrabajo.NombreTrabajo)
        o.CambioEstado(hfIdTrabajo.Value, 12, txtObservacionesCierre.Text, Session("IDUsuario").ToString)
        oCI.GuardarLogCierres(hfIdTrabajo.Value, Session("IDUsuario").ToString, txtObservacionesCierre.Text, True)
        Me.pnlInfoTrabajo.Visible = True
        Me.txtObservacionesCierre.Text = ""
        Me.pnlCierre.Visible = False
        btnCerrarTrabajo.Visible = False
        btnReporteCierre.Visible = True
        EnviarEmailCambioEstado()
        CargarTrabajosXIdProyecto(hfIdProyecto.Value)
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnCancelarCierre_Click(sender As Object, e As EventArgs) Handles btnCancelarCierre.Click
        Me.pnlInfoTrabajo.Visible = True
        Me.txtObservacionesCierre.Text = ""
        Me.pnlCierre.Visible = False
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Sub CargarCOEs()
        Dim oUsuarios As New US.Usuarios

        Dim listapersonas = (From lpersona In oUsuarios.UsuariosxGrupoUnidadXrol(20, ListaRoles.COE)
                             Select Id = lpersona.id,
                             Nombre = lpersona.Apellidos & " " & lpersona.Nombres).OrderBy(Function(p) p.Nombre)
        ddlCOE.DataSource = listapersonas.ToList()
        ddlCOE.DataValueField = "Id"
        ddlCOE.DataTextField = "Nombre"
        ddlCOE.DataBind()
        ddlCOE.Items.Insert(0, New ListItem With {.Value = -1, .Text = "--Seleccione--"})
    End Sub

    Sub EnviarEmailCoeAsignado()
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            If String.IsNullOrEmpty(hfIdTrabajo.Value) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar un anuncio de aprobación")
            End If
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/COEAsignado.aspx?idTrabajo=" & hfIdTrabajo.Value)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub

    Protected Sub BtnDuplicar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnDuplicar.Click
        Response.Redirect("../Py_Proyectos/DuplicarTrabajos.aspx?TrabajoId=" & hfIdTrabajo.Value & "&ProyectoId=" & proyectoId)
    End Sub


    Sub EscanerArchivos()

        Dim oGD As New GD.GD_Procedimientos
        Dim oTD As New CentroInformacion
        Dim idTrabajo As Integer = informacion.id
        Dim documentosEscaneados = oGD.DevolverxIdTrabajo(CInt(hfIdTrabajo.Value))
        Dim oDocumentos = oTD.obtenerdocumentoscierre(idTrabajo, Nothing)

        Dim DE As List(Of Long) = documentosEscaneados.Select(Function(x) CType(x.IdDocumento, Long)).ToList
        Dim DC As List(Of Long) = oDocumentos.Select(Function(x) x.IdDocumento).ToList
        Dim DD = DE.Except(DC).ToList

        For Each documento In DD
            oGD.eliminarDocumentoEscaneado(Nothing, documento, hfIdTrabajo.Value)
        Next

        For Each oGD_Escaner In oDocumentos
            Dim Encontrado As Boolean = False
            If oGD_Escaner.Cantidad > 0 Then Encontrado = True

            If oGD_Escaner.RolResponsableCierre = ERolResponsable.GerenteProyectos OrElse hfInternacional.Value = True Then
                If oGD_Escaner.Encontrado.HasValue Then
                    oGD.ActualizarDocumento(Nothing, idTrabajo, oGD_Escaner.IdDocumento, Encontrado)
                Else
                    oGD.Guardar(idTrabajo, oGD_Escaner.IdDocumento, Encontrado)
                End If
            End If
        Next
    End Sub

    Function obtenerRutasinComodines(ByVal ruta As String, ByVal servidor As String, ByVal unidadt As String, ByVal jbi As String, ByVal nombretrabajo As String, ByVal proceso As String, ByVal IdTrabajo As Int64) As String
        Dim rutaSinComodines As String = ruta
        rutaSinComodines = rutaSinComodines.Replace("{Servidor}", servidor)
        rutaSinComodines = rutaSinComodines.Replace("{Unidad}", unidadt)
        rutaSinComodines = rutaSinComodines.Replace("{JBI}", jbi)
        rutaSinComodines = rutaSinComodines.Replace("{NombreTrabajo}", nombretrabajo)
        rutaSinComodines = rutaSinComodines.Replace("{Proceso}", proceso)
        rutaSinComodines = rutaSinComodines.Replace("{IdTrabajo}", IdTrabajo)
        Return rutaSinComodines
    End Function

    Public Sub CargarGrid()
        Dim idTrabajo As Integer = informacion.id
        Dim oCargar As New GD.GD_Procedimientos
        gvReporte.DataSource = oCargar.DevolverxIdTrabajo(idTrabajo)
        gvReporte.DataBind()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Function crearCarpetas(ByVal idTrabajo As Int64, ByVal JBI As String, ByVal unidadId As Int16, ByVal grupoUnidadNombre As String, ByVal NombreTrabajo As String, ByVal metodogologiaId As Integer) As Boolean
        Dim daCentroInformacion As New CentroInformacion
        Dim lstCarpetasUnidad As New List(Of CI_CarpetasRed_Get_Result)
        Dim lstCarpetasOperaciones As New List(Of CI_CarpetasRed_Get_Result)
        Dim rutaSinComodines As String
        Dim daMetodologia As New MetodologiaOperaciones
        Dim Metodologia As OP_Metodologias_Get_Result
        Dim daConstante As New Constantes
        Dim usuario As String
        Dim contrasena As String

        usuario = daConstante.obtenerXId(Constantes.EConstantes.UsuarioArchivos).Valor
        contrasena = daConstante.obtenerXId(Constantes.EConstantes.ContrasenaArchivos).Valor

        NombreTrabajo = StrConv(NombreTrabajo, VbStrConv.ProperCase)
        NombreTrabajo = NombreTrabajoSinCaracteresEspeciales(NombreTrabajo)
        lstCarpetasUnidad = daCentroInformacion.obtenerCarpetas(unidadId, Nothing)
        lstCarpetasOperaciones = daCentroInformacion.obtenerCarpetas(Nothing, CentroInformacion.eTipoGrupounidad.Operativa)
        Metodologia = daMetodologia.obtenerXId(metodogologiaId)

        For Each carpeta In lstCarpetasUnidad
            Try
                If Metodologia.MetGrupoUnidad <> eGrupoUnidad.GerenciaMysteryShopper Then
                    rutaSinComodines = obtenerRutasinComodines(carpeta.RutaRaiz & carpeta.Ubicacion, "", grupoUnidadNombre, JBI, NombreTrabajo, "", idTrabajo)
                    Dim rutaPartida = rutaSinComodines.Split("\")
                    crearCarpeta(rutaSinComodines, rutaPartida(2) & "\" & rutaPartida(3), usuario, contrasena)
                    daCentroInformacion.guardarLogCreacionCarpetasRed(idTrabajo, carpeta.IdCarpeta, True, Nothing, idUsuario, DateTime.Now)
                End If
            Catch ex As Exception
                daCentroInformacion.guardarLogCreacionCarpetasRed(idTrabajo, carpeta.IdCarpeta, False, ex.Message, idUsuario, DateTime.Now)
            End Try
        Next

        For Each carpeta In lstCarpetasOperaciones
            Try
                rutaSinComodines = obtenerRutasinComodines(carpeta.RutaRaiz & carpeta.Ubicacion, "", grupoUnidadNombre, JBI, NombreTrabajo, "", idTrabajo)
                Dim rutaPartida = rutaSinComodines.Split("\")
                If carpeta.GrupoUnidadId = eGrupoUnidad.GerenciaMysteryShopper OrElse carpeta.GrupoUnidadId = eGrupoUnidad.GerenciaTelefonico Then
                    If (Metodologia.MetGrupoUnidad = eGrupoUnidad.GerenciaMysteryShopper AndAlso carpeta.GrupoUnidadId = eGrupoUnidad.GerenciaMysteryShopper) OrElse (Metodologia.MetGrupoUnidad = eGrupoUnidad.GerenciaTelefonico AndAlso carpeta.GrupoUnidadId = eGrupoUnidad.GerenciaTelefonico) Then
                        crearCarpeta(rutaSinComodines, rutaPartida(2) & "\" & rutaPartida(3), usuario, contrasena)
                        daCentroInformacion.guardarLogCreacionCarpetasRed(idTrabajo, carpeta.IdCarpeta, True, Nothing, idUsuario, DateTime.Now)
                    End If
                Else
                    crearCarpeta(rutaSinComodines, rutaPartida(2) & "\" & rutaPartida(3), usuario, contrasena)
                    daCentroInformacion.guardarLogCreacionCarpetasRed(idTrabajo, carpeta.IdCarpeta, True, Nothing, idUsuario, DateTime.Now)
                End If
            Catch ex As Exception
                daCentroInformacion.guardarLogCreacionCarpetasRed(idTrabajo, carpeta.IdCarpeta, False, ex.Message, idUsuario, DateTime.Now)
            End Try
        Next

    End Function
    Function NombreTrabajoSinCaracteresEspeciales(ByVal nombreTrabajo As String) As String
        Return Regex.Replace(nombreTrabajo, "[^A-Za-z0-9\-/]", "")
    End Function
    Sub crearCarpeta(ByVal ubicacion As String, ByVal servidor As String, ByVal usuario As String, ByVal contrasena As String)

        Using directory As New NetworkConnection("\\" & servidor, New NetworkCredential(usuario, contrasena))
            Try
                IO.Directory.CreateDirectory(ubicacion)
            Catch ex As Exception
                Dim ubicacionError = System.AppDomain.CurrentDomain.BaseDirectory & "PY_Proyectos\ErroresCrearTrabajo.txt"
                Using sw2 As New IO.StreamWriter(ubicacionError, True, System.Text.Encoding.Default)
                    sw2.WriteLine("Fecha:" & DateTime.Now.ToString)
                    sw2.WriteLine("Ubicación y servidor:" & ubicacion & "-" & servidor)
                    sw2.WriteLine("Error:" & ex.Message)
                End Using
                Throw ex
            End Try
        End Using
    End Sub

    Function copiarArchivosRecuperacion(ByVal grupounidadnombre As String, ByVal IdTrbajo As Int64, ByVal nombreTrabajo As String) As Boolean
        Dim daRD As New RepositorioDocumentos
        Dim daConstante As New Constantes
        Dim usuario As String
        Dim contrasena As String

        usuario = daConstante.obtenerXId(Constantes.EConstantes.UsuarioArchivos).Valor
        contrasena = daConstante.obtenerXId(Constantes.EConstantes.ContrasenaArchivos).Valor

        nombreTrabajo = StrConv(nombreTrabajo, VbStrConv.ProperCase)
        nombreTrabajo = NombreTrabajoSinCaracteresEspeciales(nombreTrabajo)

        Dim lstDocumentoRecuperacion = daRD.obtenerDocumentos(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, IdTrbajo, True)

        For Each documento In lstDocumentoRecuperacion
            Dim ruta = obtenerRutasinComodines(documento.URLRecuperacion, "", grupounidadnombre, "", nombreTrabajo, "", IdTrbajo)
            Dim nombreArchivo = obtenerNombreArchivo(documento.Url)
            Using directory As New NetworkConnection("\\" & documento.URLRecuperacion.Split("\")(2), New NetworkCredential(usuario, contrasena))
                Dim urlFija As String
                urlFija = "\ArchivosCargados"
                urlFija = Server.MapPath(urlFija & "\" & documento.Url)
                IO.Directory.CreateDirectory(ruta)
                IO.File.Copy(urlFija, ruta & "\" & nombreArchivo, True)
            End Using
        Next

    End Function

    Function obtenerNombreArchivo(ByVal ruta As String) As String
        Dim vRutas() As String
        vRutas = ruta.Split("\")
        Return vRutas(vRutas.Count - 1)
    End Function


    Sub eliminarDocumentosEscaneadosNoSonCierre()
        Dim oGD As New GD.GD_Procedimientos
        Dim oTD As New CentroInformacion
        Dim documentosEscaneados = oGD.DevolverxIdTrabajo(CLng(hfIdTrabajo.Value))
        Dim oDocumentos = oTD.obtenerdocumentoscierre(CLng(hfIdTrabajo.Value), Nothing)
        Dim DE = documentosEscaneados.Select(Function(x) CType(x.IdDocumento, Long)).ToList
        Dim DC = oDocumentos.Select(Function(x) CType(x.IdDocumento, Long)).ToList
        Dim DD = DE.Except(DC).ToList

        For Each documento In DD
            oGD.eliminarDocumentoEscaneado(Nothing, documento, hfIdTrabajo.Value)
        Next
    End Sub

    Private Sub btnInstructivoGeneral_Click(sender As Object, e As EventArgs) Handles btnInstructivoGeneral.Click
        If String.IsNullOrEmpty(hfIdTrabajo.Value) Then
            Throw New Exception("Debe elegir un trabajo o guardarlo antes de diligenciar el Instructivo")
        End If
        Session("TrabajoId") = hfIdTrabajo.Value
        Dim o = New Proyecto
        Dim ent As New PY_EspecifTecTrabajo
        ent = o.ObtenerEspecifaciones(hfIdTrabajo.Value)

        If ent.id <> 0 Then
            Session("duplicar") = 1
        End If

        Response.Redirect("InstructivoGeneral.aspx")
    End Sub

    Private Sub btnVerInfoGeneral_Click(sender As Object, e As EventArgs) Handles btnVerInfoGeneral.Click
		Response.Redirect("../RP_Reportes/InformacionGeneral.aspx?idTr=" & hfIdTrabajo.Value & "&URLBACK=../PY_PROYECTOS/TRABAJOS.ASPX")
	End Sub

    Private Sub btnVariablesControl_Click(sender As Object, e As EventArgs) Handles btnVariablesControl.Click
        Response.Redirect("../PY_Proyectos/VariablesControl.aspx?idTr=" & hfIdTrabajo.Value & "&modal=COE")
    End Sub

    Private Sub lnkProyecto_Click(sender As Object, e As EventArgs) Handles lnkProyecto.Click
        Dim trabajoId = hfIdTrabajo.Value
        Dim ProyectoId = hfIdProyecto.Value
        If trabajoId Is Nothing Or trabajoId = 0 Then
            Response.Redirect("Py_Proyectos.aspx")
        Else
            accordion0.Visible = True
            accordion1.Visible = False
            hfIdTrabajo.Value = 0
        End If

    End Sub

    Private Sub gvTrabajos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvTrabajos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Estado = CType(e.Row.DataItem, PY_Trabajos_GET_All_Result).Estado
            If Estado = eEstadoBloqueo.Cerrado Or Estado = eEstadoBloqueo.Anulado Then
                e.Row.Cells(8).BackColor = System.Drawing.Color.FromArgb(224, 158, 158)
                e.Row.Cells(8).ForeColor = System.Drawing.Color.FromArgb(0, 0, 0)
            End If
        End If
    End Sub
End Class