Imports CoreProject
Imports WebMatrix.Util

Public Class TrabajosCoordinadorC
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

#Region "Funciones y Métodos"
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
                If chbPropios.Checked Then
                    gvTrabajos.DataSource = oTrabajo.ObtenerTrabajosCualitativosxCOE(UsuarioID, 2, Trim(txtBuscar.Text))
                Else
                    gvTrabajos.DataSource = oTrabajo.ObtenerTrabajosCualitativosxCOE(Nothing, 2, Trim(txtBuscar.Text))
                End If
                If Request.QueryString("IdTrabajo") IsNot Nothing Then
                    hfIdTrabajo.Value = Int64.Parse(Request.QueryString("IdTrabajo"))
                    CargarCiudadesAsignadas()
                    accordion1.Visible = False
                    accordion2.Visible = True
                    lbtnVolver.Visible = True
                    lbtnVolver.PostBackUrl = "~/OP_Cualitativo/TrabajosCoordinador.aspx"
                End If
            Else
                If chbPropios.Checked Then
                    gvTrabajos.DataSource = oTrabajo.ObtenerTrabajosCualitativosxCOE(Coe, 2, Trim(txtBuscar.Text))
                Else
                    gvTrabajos.DataSource = oTrabajo.ObtenerTrabajosCualitativosxCOE(Nothing, 2, Trim(txtBuscar.Text))
                End If
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
    Sub CargarCiudadesAsignadas()
        Dim oCoord As New CoordinacionCampo
        Dim listTrabCoord = (From lmuest In oCoord.ObtenerMuestraxCoordinadoryTrabajo(Session("IDUsuario").ToString, hfIdTrabajo.Value)
                             Select Ciudad = lmuest.C_Divipola.DivMuniNombre, Muestra = lmuest.Cantidad, id = lmuest.Id, CiudadId = lmuest.CiudadId)
        If listTrabCoord.Count <= 0 Then
            pnlCiudad.Visible = False
        Else
            pnlCiudad.Visible = True
            gvCiudades.DataSource = listTrabCoord.ToList
            gvCiudades.DataBind()
        End If
    End Sub

#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lbtnVolver.Visible = False '.PostBackUrl = "~/OP_Cualitativo/HomeRecoleccion.aspx"
            CargarTrabajos(Session("IDUsuario").ToString)
        End If
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Dim oTrabajo As New Trabajo
        Dim CoeActual = chbPropios.Checked
        If CoeActual Then
            gvTrabajos.DataSource = oTrabajo.ObtenerTrabajosCualitativosxCOE(Session("IDUsuario").ToString, 2, txtBuscar.Text)
        Else
            '17 IUU - Ipsos UU
            gvTrabajos.DataSource = oTrabajo.ObtenerTrabajosCualitativosxCOE(Nothing, 2, txtBuscar.Text)
        End If
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
            'cargarTrabajo(Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")))
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            CargarCiudadesAsignadas()
            Dim oTrabajo As New Trabajo
            Dim trabajo = oTrabajo.ObtenerTrabajosCualitativosxTrabajo(hfIdTrabajo.Value, Nothing).FirstOrDefault
            Session("NombreTrabajo") = trabajo.id.ToString & " | " & trabajo.JobBook & " | " & trabajo.NombreTrabajo
            If trabajo.TipoProyectoId = 2 Then
                btnCancelar.Visible = False
                btnSegmentos.Visible = False
                btnEspecificaciones.Visible = False
                btnAudios.Visible = False
                btnFiltroReclutamiento.Visible = False
                btnFiltroAsistencia.Visible = False
                btnLoadTranscripciones.Visible = False
            Else
                btnCancelar.Visible = True
                btnSegmentos.Visible = True
                btnEspecificaciones.Visible = True
                btnAudios.Visible = True
                btnFiltroReclutamiento.Visible = True
                btnFiltroAsistencia.Visible = True
                btnLoadTranscripciones.Visible = True
            End If

            If trabajo.MetCodigo >= 700 And trabajo.MetCodigo <= 799 Then
                btnProgramacionCampo.Visible = True
            Else
                btnProgramacionCampo.Visible = False
            End If

            accordion1.Visible = False
            accordion2.Visible = True
            lbtnVolver.Visible = True
            lbtnVolver.PostBackUrl = "~/OP_Cualitativo/TrabajosCoordinador.aspx"
        ElseIf e.CommandName = "Tareas" Then
            'Response.Redirect("\CORE\Tareas.aspx" & "?IdTrabajo=" & gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id") & "&Coe=" & Session("IDUsuario").ToString)
        End If
    End Sub
    Private Sub gvCiudades_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCiudades.RowCommand
        If e.CommandName = "Enviar" Then
            hfidCiudadId.Value = Int64.Parse(Me.gvCiudades.DataKeys(CInt(e.CommandArgument))("CiudadId"))

            accordion1.Visible = False
            accordion2.Visible = True
        ElseIf e.CommandName = "Asignar" Then
            hfidCiudadId.Value = Int64.Parse(Me.gvCiudades.DataKeys(CInt(e.CommandArgument))("CiudadId"))
            accordion1.Visible = False
            accordion2.Visible = True
        End If
    End Sub
    Protected Sub btnEspecificaciones_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEspecificaciones.Click
        Try
            If String.IsNullOrEmpty(hfIdTrabajo.Value) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de ver las especificaciones")
            End If
            Dim oTrabajo As New Trabajo
            Dim oeTrabajo As PY_Trabajos_Get_Result
            oeTrabajo = oTrabajo.obtenerXId(hfIdTrabajo.Value)

            Response.Redirect("../PY_Proyectos/InstructivoGeneralCuali.aspx")
            'Dim oTipoFicha As New MetodologiaOperaciones
            'Dim tipo As Int32 = oTipoFicha.ObtenerFichaMetodologiaxId(oeTrabajo.OP_MetodologiaId).Ficha
            'Select Case tipo
            '    Case TipoFicha.Cuanti
            '        Response.Redirect("../OP_Cuantitativo/FichaCuantitativa.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes&coordinador=1")
            '    Case TipoFicha.Sesiones
            '        Response.Redirect("../OP_Cualitativo/FichaSesion.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes&coordinador=1")
            '    Case TipoFicha.Observaciones
            '        Response.Redirect("../OP_Cualitativo/FichaObservacion.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes&coordinador=1")
            '    Case TipoFicha.Entrevistas
            '        Response.Redirect("../OP_Cualitativo/FichaEntrevistas.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes&coordinador=1")
            'End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try


    End Sub
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        accordion1.Visible = True
        accordion2.Visible = False
    End Sub
    Protected Sub btnVerInfoGeneral_Click(sender As Object, e As EventArgs) Handles btnVerInfoGeneral.Click
        Response.Redirect("../RP_Reportes/InformacionGeneralCuali.aspx?idTr=" & hfIdTrabajo.Value & "&URLBACK=../OP_Cualitativo/TrabajosCoordinador.aspx")
    End Sub

#End Region

    Protected Sub btnProgramacionCampo_Click(sender As Object, e As EventArgs) Handles btnProgramacionCampo.Click
        Try
            If String.IsNullOrEmpty(hfIdTrabajo.Value) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de ver las especificaciones")
            End If
            Dim oTrabajo As New Trabajo
            Dim oeTrabajo As PY_Trabajos_Get_Result
            oeTrabajo = oTrabajo.obtenerXId(hfIdTrabajo.Value)
            Dim oTipoFicha As New MetodologiaOperaciones
            Dim tipo As Int32 = oTipoFicha.ObtenerFichaMetodologiaxId(oeTrabajo.OP_MetodologiaId).Ficha
            Dim metodologia = oTipoFicha.obtenerXId(oeTrabajo.OP_MetodologiaId)

            If metodologia.TipoProyecto = 2 Then
                Response.Redirect("../OP_Cualitativo/ProgramacionCampo.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes")
            Else
                Select Case tipo
                    Case TipoFicha.Cuanti
                        Response.Redirect("../OP_Cuantitativo/FichaCuantitativa.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes")
                    'Case TipoFicha.Sesiones
                    '    Response.Redirect("../OP_Cualitativo/Sesion.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes")
                    Case TipoFicha.Observaciones
                        Response.Redirect("../OP_Cualitativo/Observacion.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes")
                    Case TipoFicha.Entrevistas
                        Response.Redirect("../OP_Cualitativo/Entrevista.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes")
                End Select
            End If

        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub

    Protected Sub btnAudios_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAudios.Click
        If hfIdTrabajo.Value <> "" Then
            Response.Redirect("\GD_Documentos\GD_Documentos.aspx?IdContenedor=" & hfIdTrabajo.Value & "&IdDocumento=1")
        Else : ShowNotification("Seleccione Primero Una Tarea a Gestionar", ShowNotifications.ErrorNotification)
        End If
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub


    Protected Sub btnListadoDocumentos_Click(sender As Object, e As EventArgs) Handles btnListadoDocumentos.Click
        Response.Redirect("../CORE/ListaDocumentosXHilos.aspx?IdTrabajo=" & hfIdTrabajo.Value & "&URLRetorno=" & UrlOriginal.OP_Cualitativo_TrabajosCoordinador)
    End Sub

    'Protected Sub btnTranscripciones_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTranscripciones.Click
    '    Response.Redirect("../OP_Cualitativo/Transcripcion.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes")

    'End Sub

    Protected Sub btnLoadTranscripciones_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLoadTranscripciones.Click
        If hfIdTrabajo.Value <> "" Then
            Response.Redirect("\GD_Documentos\GD_Documentos.aspx?IdContenedor=" & hfIdTrabajo.Value & "&IdDocumento=1")
        Else : ShowNotification("Seleccione Primero Una Tarea a Gestionar", ShowNotifications.ErrorNotification)
        End If
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub btnVariablesControl_Click(sender As Object, e As EventArgs) Handles btnVariablesControl.Click
        Dim proyectoId As New Int64
        Dim oTrabajo As New Trabajo
        Dim oeTrabajo As New PY_Trabajo0
        oeTrabajo = oTrabajo.ObtenerTrabajo(hfIdTrabajo.Value)

        Long.TryParse(oeTrabajo.ProyectoId, proyectoId)
        hfIdProyecto.Value = proyectoId
        Session("ProyectoId") = hfIdProyecto.Value

        Dim oProyecto As New Proyecto
        Dim oeProyecto As New PY_Proyectos_Get_Result
        oeProyecto = oProyecto.obtenerXId(hfIdProyecto.Value)
        Dim tipoProyecto = oeProyecto.TipoProyectoId

        If oeProyecto.GerenteProyectos Is Nothing Then
            ShowNotification("No está asignado el Gerente de Proyecto", ShowNotifications.ErrorNotification)
        Else
            Response.Redirect("../PY_Proyectos/VariablesControl.aspx?idTr=" & hfIdTrabajo.Value & "&modal=GP")
        End If
    End Sub

    Private Sub lbtnVolver_Click(sender As Object, e As EventArgs) Handles lbtnVolver.Click
        If hfIdTrabajo.Value <> "" Then
            CargarTrabajos(Session("IDUsuario").ToString)
            accordion1.Visible = True
            accordion2.Visible = False
            lbtnVolver.Visible = False
            lbtnVolver.PostBackUrl = ""
        End If
    End Sub

    Protected Sub btnEstadoTareas_Click(sender As Object, e As EventArgs) Handles btnEstadoTareas.Click
        Response.Redirect("../CORE/Gestion-Tareas.aspx?IdTrabajo=" & hfIdTrabajo.Value & "&URLRetorno=" & UrlOriginal.OP_Cualitativo_Trabajos & "&IdUnidadEjecuta=" & UnidadesCore.COE & "&IdRolEstima=" & ListaRoles.COE)
    End Sub
End Class