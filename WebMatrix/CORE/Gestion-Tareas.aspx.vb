Imports CoreProject
Imports WebMatrix.Util
Imports CoreProject.OP_Cuanti
Imports CoreProject.OP
Imports ClosedXML.Excel

Public Class Gestion_Tareas
    Inherits System.Web.UI.Page

#Region "Enumeradores"
    Enum eTarea
        Instrumentos = 1
        Codificacion = 8
        PDC = 12
        Procesamiento = 13
        Scripting = 20
        EstadisticaMetodologia = 23
        EstadisticaPonderacion = 22
        EstadisticaProcesoEspeciales = 37
        EstadisticaAprobacionPonderacion = 48
        EstadisticaSeleccionIDM = 51
        EstadisticaDisenoMuestral = 80
        ProyectosInforme = 16
        CreacionEsquemaAnalisis = 63
        RevisionEsquemaAnalisis = 64
        VariablesControlEvaluaCOE = 65
        VariablesControlEvaluaGP = 66
        ScriptingControlInterno = 76
        ProcesamientoControlInterno = 78
        CodificacionControlInterno = 81
        DataVisualizationPlanGraficacion = 93
        DataVisualizationInformes = 94
    End Enum
    Enum eEstadoBloqueo
        Cerrado = 10
        Anulado = 11
    End Enum

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.QueryString("idTrabajo") Is Nothing Then
            hfIdTrabajo.Value = CDbl(Request.QueryString("idTrabajo"))
        End If
        If Request.QueryString("URLRetorno") IsNot Nothing Then
            hfUrlRetorno.Value = Request.QueryString("URLRetorno")
        End If
        If Request.QueryString("IdUnidadEjecuta") IsNot Nothing Then
            hfUnidadEjecuta.Value = Request.QueryString("IdUnidadEjecuta")
        End If
        If Request.QueryString("IdRolEstima") IsNot Nothing Then
            hfRolEstima.Value = Request.QueryString("IdRolEstima")
        End If

        If Not IsPostBack Then
            If Not hfIdTrabajo.Value = 0 Then
                Dim daTrabajo As New Trabajo
                Dim daPropuesta As New Propuesta
                Dim oTrabajo = daTrabajo.ObtenerTrabajo(hfIdTrabajo.Value)
                Dim oPropuesta = daPropuesta.DevolverxID(oTrabajo.IdPropuesta)
                hfInternacional.Value = oPropuesta.internacional
                hfEstadoId.Value = oTrabajo.Estado
                CargarTareasList()
                Me.pnlTareasXTrabajo.Visible = True
                Me.pnlDetalleTarea.Visible = True
                Try
                    If Session("NombreTrabajo") Is Nothing Then
                        Session("NombreTrabajo") = oTrabajo.id.ToString & " | " & oTrabajo.JobBook & " | " & oTrabajo.NombreTrabajo
                    End If
                    Me.txtNombreTrabajo.Text = Session("NombreTrabajo").ToString
                Catch ex As Exception
                End Try
            End If
            Me.PnlIncosistencias.Visible = False

        End If
        If hfEstadoId.Value = eEstadoBloqueo.Anulado Or hfEstadoId.Value = eEstadoBloqueo.Cerrado Then
            bloquearAnulado(True)
            ShowNotification("Este Trabajo no puede ser modificado porque está Anulado o Cerrado", ShowNotifications.InfoNotificationLong)
        Else
            bloquearAnulado(False)
        End If
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.gvDocumentosCargados)

    End Sub

    Sub CargarDocumentosCargados()
        Dim o As New RepositorioDocumentos
        gvDocumentosCargados.DataSource = o.obtenerDocumentosXIdContenedorXIdDocumento(hfIdTrabajo.Value, hfIdDocDescarga.Value)
        gvDocumentosCargados.DataBind()
    End Sub

    Sub cargarDocumentosXIdTrabajo()
        Dim o As New Tareas_Documentos
        gvTareasXDocumentos.DataSource = o.obtenerDocumentosXHilo(hfIdTrabajo.Value)
        gvTareasXDocumentos.DataBind()
    End Sub

    Sub CargarCronograma()
        Dim o As New CT_Tareas
        Dim data = o.TareasList(Nothing, hfIdTrabajo.Value, Nothing, Nothing, 6, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        Me.gvCronograma.DataSource = data
        Me.gvCronograma.DataBind()

        Session.Add("Cronograma", data)

        If (Gantt_Cronograma.Visible = True) Then
            Dim dataGantt = CrearTablaGantt(data)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "cargarGantt", "cargarGantt(" + SerializarAJSON(dataGantt) + ");", True)
        End If
    End Sub

    Sub CargarTareasList()
        Dim o As New CT_Tareas
        If hfUsuarioAsignado.Value = "0" Then
            Me.gvTareasList.DataSource = o.TareasList(Nothing, hfIdTrabajo.Value, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        Else
            Me.gvTareasList.DataSource = o.TareasList(Nothing, hfIdTrabajo.Value, Nothing, Nothing, 6, hfUsuarioAsignado.Value, Nothing, Nothing, Nothing, Nothing, Nothing)
        End If
        Me.gvTareasList.DataBind()

    End Sub

    Sub DetalleTarea(ByVal idt As Int64)
        Dim o As New CT_Tareas
        Dim info = o.TareasList(idt, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault

        Dim oRevision As New RevisionIPS
        Dim olistarevision = oRevision.DevolverxTareaId(hfIdTrabajo.Value, idt)

        If olistarevision.Count > 0 Then
            Me.btnRevInconsistencias.Visible = True
        Else
            Me.btnRevInconsistencias.Visible = False
        End If

        If info.FIniR Is Nothing And info.USUARIOID = Session("IDUsuario").ToString Then
            Me.txtObservacionesEjecucion.Visible = True
            Me.btnGestionTarea.Visible = True
            Me.btnGestionTarea.Text = "Iniciar Tarea"
        Else
            If info.FFinR Is Nothing And info.USUARIOID = Session("IDUsuario").ToString Then
                Me.txtObservacionesEjecucion.Visible = True
                Me.btnGestionTarea.Visible = True
                Me.btnGestionTarea.Text = "Finalizar Tarea"
                lblObservacion.Visible = True
            Else
                Me.txtObservacionesEjecucion.Visible = False
                Me.btnGestionTarea.Visible = False
                lblObservacion.Visible = False
                Me.btnGestionTarea.Visible = False
            End If
        End If

        Me.lblTarea.Text = ""
        Me.lblEstado.Text = ""
        Me.lblInicioP.Text = ""
        Me.lblFinP.Text = ""
        Me.lblInicioR.Text = ""
        Me.lblFinR.Text = ""
        Me.txtObservacionesEjecucion.Text = ""
        Me.lblResponsable.Text = ""

        Me.lblTarea.Text = info.TAREA
        If info.RETRASO = 1 Then
            Me.lblEstado.Text = info.Estado & " - ¡TAREA RETRASADA!"
        Else
            Me.lblEstado.Text = info.Estado
        End If
        Me.hfTareaId.Value = info.TareaId
        If IsDate(info.FIniP) Then Me.lblInicioP.Text = CDate(info.FIniP)
        If IsDate(info.FFinP) Then Me.lblFinP.Text = CDate(info.FFinP)
        If IsDate(info.FIniR) Then Me.lblInicioR.Text = CDate(info.FIniR)
        If IsDate(info.FFinR) Then Me.lblFinR.Text = CDate(info.FFinR)

        Me.lblResponsable.Text = info.Responsable
        cargarTareasPrevias(idt, hfIdTrabajo.Value)
        cargarArchivosEntregablesXIdTarea(hfTareaId.Value, hfIdTrabajo.Value)

        If info.USUARIOID = Session("IDUsuario").ToString And btnGestionTarea.Visible = True Then
            Me.gvTareasAnteriores.Columns(1).Visible = True
        Else
            Me.gvTareasAnteriores.Columns(1).Visible = False
        End If
        If info.USUARIOID = Session("IDUsuario").ToString Then
            Me.gvArchivosEntregables.Columns(2).Visible = True
        Else
            Me.gvArchivosEntregables.Columns(2).Visible = False
        End If
        cargarObservacionesTarea(idt)
    End Sub

    Sub cargarTareasPrevias(ByVal idTarea As Int64, ByVal idTrabajo As Int64)
        Dim o As New CT_Tareas
        Dim oeMetodologiaOperaciones As OP_Metodologias_Get_Result
        Dim otrabajo As New Trabajo
        oeMetodologiaOperaciones = obtenerOPMetodologia(otrabajo.ObtenerTrabajo(idTrabajo).OP_MetodologiaId)
        gvTareasAnteriores.DataSource = o.TareasPrevias(idTarea, idTrabajo, oeMetodologiaOperaciones.TipoHiloId)
        gvTareasAnteriores.DataBind()
    End Sub

    Sub chkVerificar_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim checkbox As CheckBox = sender
        Dim row As GridViewRow = checkbox.NamingContainer
        Dim index As Integer = row.RowIndex
        If checkbox.Checked = False Then
            DirectCast(row.FindControl("txtFInicio"), TextBox).Enabled = True
            DirectCast(row.FindControl("txtFFin"), TextBox).Enabled = True
        Else
            DirectCast(row.FindControl("txtFInicio"), TextBox).Enabled = False
            DirectCast(row.FindControl("txtFFin"), TextBox).Enabled = False
        End If
    End Sub

    Function obtenerArchivosXIdTarea(ByVal idTarea As Int64, ByVal tipoDocumento As Int16, ByVal idHilo As Int64) As List(Of CORE_DocumentosRequeridosXTarea_Get_Result)
        Dim o As New Tareas_Documentos
        Return o.obtenerDocumentosXTarea(idTarea, tipoDocumento, idHilo)
    End Function

    Sub cargarArchivosEntregablesXIdTarea(ByVal idTarea As Int64, ByVal idHilo As Int64)
        gvArchivosEntregables.DataSource = obtenerArchivosXIdTarea(idTarea, 2, idHilo)
        gvArchivosEntregables.DataBind()
    End Sub

    Function obtenerOPMetodologia(ByVal id As Int16) As OP_Metodologias_Get_Result
        Dim oMetodologiaOperaciones As New MetodologiaOperaciones
        Return oMetodologiaOperaciones.obtenerXId(id)
    End Function

    Private Sub gvTareasList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareasList.RowCommand
        Dim o As New CT_Tareas
        Dim idt As Int64 = Int64.Parse(Me.gvTareasList.DataKeys(CInt(e.CommandArgument))("Id"))
        hfIdWFid.Value = idt
        Dim lstTareas = o.TareasList(idt, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault

        If esTareaConRegistroObservacion(lstTareas) Then
            Me.PnlIncosistencias.Visible = True
        Else
            Me.PnlIncosistencias.Visible = False
        End If

        If RegistroObservacionCuali(hfIdTrabajo.Value) Then
            Me.PnlIncosistenciasCuali.Visible = True
        Else
            Me.PnlIncosistenciasCuali.Visible = False
        End If

        Me.pnlDetalle.Visible = True
        Me.PnlDetalleNoData.Visible = False
        Me.pnlDocumentos.Visible = True
        Me.pnlDetalleDocumentos.Visible = True
        Me.pnlNoDataDocs.Visible = False
        DetalleTarea(idt)
    End Sub

    Sub bloquearAnulado(ByVal bloqueo As Boolean)
        If bloqueo Then
            txtObservacionesEjecucion.Enabled = False
            btnGestionTarea.Visible = False
            btnInconsistencias.Visible = False
            btnRevInconsistencias.Visible = False
            btnInconsistenciasCuali.Visible = False
            btnRevInconsistenciasCuali.Visible = False
            pnlDocumentos.Enabled = False
            gvTareasAnteriores.Enabled = False
            gvEstimacion.Enabled = False
            btnGuardarEstimacion.Visible = False
            gvAsignacion.Enabled = False
            gvAsignacion.Columns(4).Visible = False
            gvTareasXDocumentos.Enabled = False
            gvTareasXDocumentos.Columns(3).Visible = False
        Else
            txtObservacionesEjecucion.Enabled = True
            btnGestionTarea.Visible = True
            btnInconsistencias.Visible = True
            btnRevInconsistencias.Visible = True
            btnInconsistenciasCuali.Visible = True
            btnRevInconsistenciasCuali.Visible = True
            pnlDocumentos.Enabled = True
            gvTareasAnteriores.Enabled = True
            gvEstimacion.Enabled = True
            btnGuardarEstimacion.Visible = True
            gvAsignacion.Enabled = True
            gvAsignacion.Columns(4).Visible = True
            gvTareasXDocumentos.Enabled = True
            gvTareasXDocumentos.Columns(3).Visible = True
        End If
    End Sub

    Private Shared Function esTareaConRegistroObservacion(lstTareas As CT_TareasList_Result) As Boolean?
        Return lstTareas.TareaId = eTarea.Instrumentos OrElse
            lstTareas.TareaId = eTarea.Codificacion OrElse
            lstTareas.TareaId = eTarea.CodificacionControlInterno OrElse
            lstTareas.TareaId = eTarea.ProcesamientoControlInterno OrElse
            lstTareas.TareaId = eTarea.Procesamiento OrElse
            lstTareas.TareaId = eTarea.Scripting OrElse
            lstTareas.TareaId = eTarea.ScriptingControlInterno OrElse
            lstTareas.TareaId = eTarea.PDC OrElse
            lstTareas.TareaId = eTarea.EstadisticaMetodologia OrElse
            lstTareas.TareaId = eTarea.EstadisticaPonderacion OrElse
            lstTareas.TareaId = eTarea.EstadisticaProcesoEspeciales OrElse
            lstTareas.TareaId = eTarea.EstadisticaAprobacionPonderacion OrElse
            lstTareas.TareaId = eTarea.EstadisticaSeleccionIDM OrElse
            lstTareas.TareaId = eTarea.EstadisticaDisenoMuestral OrElse
            lstTareas.TareaId = eTarea.ProyectosInforme OrElse
            lstTareas.TareaId = eTarea.CreacionEsquemaAnalisis OrElse
            lstTareas.TareaId = eTarea.RevisionEsquemaAnalisis OrElse
            lstTareas.TareaId = eTarea.VariablesControlEvaluaCOE OrElse
            lstTareas.TareaId = eTarea.VariablesControlEvaluaGP OrElse
            lstTareas.TareaId = eTarea.DataVisualizationPlanGraficacion OrElse
            lstTareas.TareaId = eTarea.DataVisualizationInformes
    End Function

    Private Shared Function RegistroObservacionCuali(ByVal idTrabajo As Int64) As Boolean?
        Dim cuali = False
        Dim oTrabajo As New Trabajo
        Dim trabajoCuali As New PY_Trabajos_Get_Cualitativos_Result
        trabajoCuali = oTrabajo.obtenerListadoTrabajosCualitativos(idTrabajo, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, 2, Nothing).FirstOrDefault

        If Not trabajoCuali Is Nothing Then
            If trabajoCuali.TipoProyectoId = 2 Then
                cuali = True
            End If
        End If

        Return cuali
    End Function

    Private Sub gvTareasAnteriores_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareasAnteriores.RowCommand
        Dim idt As Int64 = Int64.Parse(Me.gvTareasAnteriores.DataKeys(CInt(e.CommandArgument))("Id"))
        Me.hfIdTareaRechazo.Value = idt
    End Sub

    Protected Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click
        Dim oLogWorkFlow As New LogWorkFlow
        Dim oEnviarCorreo As New EnviarCorreo
        Dim oGT As New CT_Tareas
        Try
            Dim info = oGT.ObtenerWorkFlow(hfIdTareaRechazo.Value)
            oLogWorkFlow.CORE_Log_WorkFlow_Add(hfIdTareaRechazo.Value, DateTime.UtcNow.AddHours(-5), Session("IDUsuario").ToString, LogWorkFlow.WorkFlowEstados.Devuelta, txtObservacionDevolucion.Text)
            info.Estado = LogWorkFlow.WorkFlowEstados.Devuelta
            info.FechaUltimoEstado = Date.UtcNow.AddHours(-5)
            info.FFinR = Nothing
            oGT.GuardarWorkFlow(info)
            cargarTareasPrevias(hfIdWFid.Value, hfIdTrabajo.Value)
            Dim o As New Trabajo
            oEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/Tarea.aspx?&IdTrabajo=" & hfIdTrabajo.Value & "&IdWorkFlow=" & hfIdTareaRechazo.Value & "&IdProyecto=" & o.ObtenerTrabajo(hfIdTrabajo.Value).ProyectoId & "&IdEstado=" & WorkFlow.Estados.Devuelta)
            ShowNotification("El proceso de devolución ha sido realizado satisfactoriamente!", ShowNotifications.InfoNotification)
        Catch ex As Exception
            ShowNotification("Ha ocurrido un error - " & ex.Message, ShowNotifications.ErrorNotification)
        End Try

    End Sub

    Protected Sub btnCargarArchivos_Click(sender As Object, e As EventArgs) Handles btnCargarArchivos.Click
        If IsValid Then
            GrabarSubidaArchivo()
            cargarArchivosEntregablesXIdTarea(hfTareaId.Value, hfIdTrabajo.Value)
        End If
    End Sub

    Sub GrabarSubidaArchivo()
        Dim o As New RepositorioDocumentos
        Dim guid As String = Now.Year.ToString & "-" & Now.Month.ToString & "-" & Now.Day & "-" & Now.Hour & Now.Minute & Now.Second
        GrabarArchivo(guid.ToString)
        o.Grabar(txtNombre.Text, ObtenerURLArchivo() & "\" & guid.ToString & "-" & ufArchivo.FileName, hfDocumentoId.Value, Nothing, DateTime.UtcNow.AddHours(-5), txtComentarios.Text, Session("IDUsuario").ToString, hfIdTrabajo.Value)
        Select Case hfDocumentoId.Value
            Case 13
                Try
                    Dim oEnviarCorreo As New EnviarCorreo
                    oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/CircularCargada.aspx?idTrabajo=" & hfIdTrabajo.Value)
                Catch ex As Exception
                End Try
            Case 72
                Try
                    Dim oEnviarCorreo As New EnviarCorreo
                    oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/PonderacionCargada.aspx?idTrabajo=" & hfIdTrabajo.Value)
                Catch ex As Exception
                End Try
            Case Else

        End Select
    End Sub

    Sub GrabarArchivo(ByVal guid As String)
        Dim URL As String
        Dim RutaFisica As String

        ' Obtener la URL relativa al archivo
        URL = ObtenerURLArchivo()

        ' Concatenar la URL base del directorio virtual asegurándote de utilizar las barras adecuadas para web
        Dim urlRelativa As String = "ArchivosCargados/" & URL.TrimStart("\")

        ' Usar Server.MapPath para convertir la ruta relativa al contexto del servidor si es necesario
        RutaFisica = Server.MapPath("~/" & urlRelativa)

        ' Crear el directorio si no existe ya
        IO.Directory.CreateDirectory(RutaFisica)

        ' Guardar el archivo en la ruta construida
        ufArchivo.SaveAs(IO.Path.Combine(RutaFisica, guid.ToString() & "-" & ufArchivo.FileName))
    End Sub

    Function ObtenerURLArchivo() As String
        Dim Ruta As String = ""
        Dim oeMaestroDocumentos As GD_GD_MaestroDocumentos_Get2_Result
        Dim o As New GD.GD_Procedimientos
        oeMaestroDocumentos = o.obtenerDocumentoMaestroXId(hfDocumentoId.Value)

        Dim oTrabajo As New Trabajo
        Dim oProyecto As New Proyecto
        Dim oEstudio As New Estudio
        Dim oWorkFlow As New WorkFlow
        Dim oHilo As New Hilo

        Dim oeTrabajo As PY_Trabajos_Get_Result
        Dim oeProyecto As PY_Proyectos_Get_Result
        Dim oeEstudio As CU_Estudios
        Dim oeWorkFlow As CORE_WorkFlow_Trabajos_Get_Result
        Dim oeHilo As CORE_Hilos

        oeWorkFlow = oWorkFlow.obtenerXId(hfIdWFid.Value)
        oeHilo = oHilo.obtenerXId(oeWorkFlow.HiloId)
        oeTrabajo = oTrabajo.obtenerXId(oeHilo.ContenedorId)
        oeProyecto = oProyecto.obtenerXId(oeTrabajo.ProyectoId)
        oeEstudio = oEstudio.ObtenerXID(oeProyecto.EstudioId)


        Ruta = oeEstudio.JobBook & "\" & oeProyecto.JobBook & "\" & oeTrabajo.id & "\" & oeMaestroDocumentos.URL

        Return Ruta
    End Function

    Protected Sub CustomValidator1_ServerValidate(ByVal sender As Object, ByVal e As ServerValidateEventArgs)

        'If (ufArchivo.FileBytes.Length > ConfigurationManager.AppSettings("TamanoMaximoCargaArchivos") * 1024) Then ' 1024*KB of file size
        '    e.IsValid = False
        '    AlertJS("El tamaño del archivo supera los limites permitidos, maximo " & ConfigurationManager.AppSettings("TamanoMaximoCargaArchivos") / 1024 & " Mb")
        'Else
        '    e.IsValid = True
        'End If
    End Sub

    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Private Sub gvArchivosEntregables_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvArchivosEntregables.RowCommand
        Dim idt As Int64 = Int64.Parse(Me.gvArchivosEntregables.DataKeys(CInt(e.CommandArgument))("IdDocumento"))
        Me.hfDocumentoId.Value = idt
    End Sub

    Protected Sub btnGestionTarea_Click(sender As Object, e As EventArgs) Handles btnGestionTarea.Click
        If Me.btnGestionTarea.Text = "Iniciar Tarea" Then
            Dim oLogWorkFlow As New LogWorkFlow
            Dim oGt As New CT_Tareas
            Dim oWorkFlow As New WorkFlow
            oLogWorkFlow.CORE_Log_WorkFlow_Add(hfIdWFid.Value, DateTime.UtcNow.AddHours(-5), Session("IDUsuario").ToString, LogWorkFlow.WorkFlowEstados.EnCurso, txtObservacionesEjecucion.Text)
            Dim info = oGt.ObtenerWorkFlow(hfIdWFid.Value)
            info.FIniR = Date.UtcNow.AddHours(-5)
            info.Estado = LogWorkFlow.WorkFlowEstados.EnCurso
            info.FechaUltimoEstado = Date.UtcNow.AddHours(-5)
            info.ObservacionesEjecucion = txtObservacionesEjecucion.Text
            oGt.GuardarWorkFlow(info)
            DetalleTarea(hfIdWFid.Value)
        ElseIf Me.btnGestionTarea.Text = "Finalizar Tarea" Then
            'If ValidacionFinalizacionTarea() = False Then Exit Sub
            Dim oLogWorkFlow As New LogWorkFlow
            Dim oGt As New CT_Tareas
            Dim oWorkFlow As New WorkFlow
            oLogWorkFlow.CORE_Log_WorkFlow_Add(hfIdWFid.Value, DateTime.UtcNow.AddHours(-5), Session("IDUsuario").ToString, LogWorkFlow.WorkFlowEstados.Finalizada, txtObservacionesEjecucion.Text)
            Dim info = oGt.ObtenerWorkFlow(hfIdWFid.Value)
            info.FFinR = Date.UtcNow.AddHours(-5)
            info.Estado = LogWorkFlow.WorkFlowEstados.Finalizada
            info.FechaUltimoEstado = Date.UtcNow.AddHours(-5)
            info.ObservacionesEjecucion = txtObservacionesEjecucion.Text
            oGt.GuardarWorkFlow(info)

            Dim oEnviarCorreo As New EnviarCorreo
            Dim lstUsuariosANotificar As New List(Of String)
            Dim o As New Trabajo
            lstUsuariosANotificar = obtenerUsuariosNotificacion(WorkFlow.Estados.Finalizada)
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/Tarea.aspx?IdTarea=" & hfTareaId.Value & "&IdTrabajo=" & hfIdTrabajo.Value & "&IdWorkFlow=" & hfIdWFid.Value & "&IdProyecto=" & o.ObtenerTrabajo(hfIdTrabajo.Value).ProyectoId & "&IdEstado=" & WorkFlow.Estados.Finalizada)
            oLogWorkFlow.CORE_TareasDependientes_Finalizar(hfTareaId.Value, hfIdTrabajo.Value, Session("IDUsuario").ToString)

            If lstUsuariosANotificar.Count < 1 Then
                AlertJS("Tarea finaliza con exito - Pero,  no se pudo notificar a las tareas siguientes, que estan a la espera de que esta tarea finalice, debido a que esas tareas no tienen usuario asignado aún, por favor pongase en contacto con los coordinadores que requieren saber que esta tarea finalizo con exito.")
            Else
                AlertJS("Tarea finaliza con exito")
            End If

            DetalleTarea(hfIdWFid.Value)
        End If
    End Sub

    Function ValidacionFinalizacionTarea() As Boolean
        If todasTareasPreviasEstanFinalizadas(hfTareaId.Value, hfIdTrabajo.Value) Then
            If todosArchivosEntregablesXTareaSubidos(hfTareaId.Value, hfIdTrabajo.Value) Then
                Return True
            Else
                AlertJS("Debe subir todos los archivos entregables para poder finalizar la tarea")
                Return False
            End If
        Else
            AlertJS("Todas las tareas previas a esta actividad deben estar en estado finalizado, para poder finalizar esta actividad")
            Return False
        End If
    End Function

    Function todasTareasPreviasEstanFinalizadas(ByVal idTarea As Int64, ByVal idTrabajo As Int64) As Boolean
        Dim lstTareasPrevias As New List(Of CT_TareasList_Result)
        lstTareasPrevias = obtenerTareasPrevias(idTarea, idTrabajo)
        If lstTareasPrevias.Where(Function(x) x.ESTADOID <> LogWorkFlow.WorkFlowEstados.Finalizada And x.ESTADOID <> LogWorkFlow.WorkFlowEstados.NoAplica).Count > 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Function obtenerTareasPrevias(ByVal idTarea As Int64, ByVal idTrabajo As Int64) As List(Of CT_TareasList_Result)
        Dim o As New CT_Tareas
        Dim oeMetodologiaOperaciones As OP_Metodologias_Get_Result
        Dim otrabajo As New Trabajo
        oeMetodologiaOperaciones = obtenerOPMetodologia(otrabajo.ObtenerTrabajo(idTrabajo).OP_MetodologiaId)
        Return o.TareasPrevias(hfIdWFid.Value, idTrabajo, oeMetodologiaOperaciones.TipoHiloId)
    End Function

    Function todosArchivosEntregablesXTareaSubidos(ByVal idTarea As Int64, ByVal idTrabajo As Int64) As Boolean
        Dim o As New Tareas_Documentos
        Return Not o.obtenerDocumentosXTarea(idTarea, 2, idTrabajo).Where(Function(x) x.Cantidad = 0 AndAlso x.EsOpcional = False).Count > 0
    End Function

    Function obtenerUsuariosNotificacion(ByVal idEstado As WorkFlow.Estados) As List(Of String)
        Dim oWorkFlow As New WorkFlow
        If idEstado = WorkFlow.Estados.Devuelta Then
            Return oWorkFlow.obtenerUsuariosNotificacionTareaDevuelta(hfIdTrabajo.Value, hfTareaId.Value).Select(Function(x) x.Email).ToList
        Else
            Return oWorkFlow.obtenerUsuariosNotificacionTareas(hfIdTrabajo.Value, hfTareaId.Value).Select(Function(x) x.Email).ToList
        End If

    End Function

    Sub CargarUsuariosDisponibles(ByVal rolid As Integer, ByVal WorkFlowId As Integer)
        Try
            Dim daUsuarios As New US.Usuarios
            Dim WorkFlow_UsuariosAsignados As New WorkFlow_UsuariosAsignados
            If hfInternacional.Value = True AndAlso hfRolEstima.Value = 6 Then
                Dim oUsuario = daUsuarios.obtenerUsuarioXId(Session("IDUsuario"))
                ddlUsuariosDisponibles.Items.Insert(0, New ListItem With {.Text = oUsuario.Nombres & " " & oUsuario.Apellidos, .Value = oUsuario.id})
            Else
                ddlUsuariosDisponibles.DataSource = WorkFlow_UsuariosAsignados.obtenerUsuariosXWorkFlowIdXEstadoXRolId(WorkFlowId, False, rolid)
                ddlUsuariosDisponibles.DataValueField = "Id"
                ddlUsuariosDisponibles.DataTextField = "Nombre"
                ddlUsuariosDisponibles.DataBind()
            End If
            ddlUsuariosDisponibles.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
            upUsuariosAsignados.Update()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnAdicionarUsuario_Click(sender As Object, e As EventArgs) Handles btnAdicionarUsuario.Click
        Dim oLogWorkFlow As New LogWorkFlow
        Dim oGt As New CT_Tareas
        Dim oWorkFlow As New WorkFlow
        Dim info = oGt.ObtenerWorkFlow(hfWfIdAsignacion.Value)
        info.UsuarioAsignado = ddlUsuariosDisponibles.SelectedValue
        If info.Estado = LogWorkFlow.WorkFlowEstados.Creada Then info.Estado = LogWorkFlow.WorkFlowEstados.Asignada

        oGt.GuardarWorkFlow(info)
        Dim oWorkFlow_UsuariosAsignados As New WorkFlow_UsuariosAsignados
        Dim oLogWF As New LogWorkFlow
        Dim oTarea As New CoreProject.Tarea
        Dim RolId As Integer
        Dim oEnviarCorreo As New EnviarCorreo

        oWorkFlow_UsuariosAsignados.grabar(hfWfIdAsignacion.Value, ddlUsuariosDisponibles.SelectedValue, DateTime.Now)
        oLogWF.CORE_Log_WorkFlow_Add(hfWfIdAsignacion.Value, Now, Session("IDUsuario").ToString, LogWorkFlow.WorkFlowEstados.Asignada, Nothing)
        RolId = oTarea.obtenerXId(hfWfTareaAsignacion.Value).RolEjecuta
        oEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/TareaAsignacion.aspx?IdTrabajo=" & hfIdTrabajo.Value & "&IdWorkFlow=" & hfWfIdAsignacion.Value & "&IdTipoAsignacion=1&IdUsuarioNotificar=" & ddlUsuariosDisponibles.SelectedValue)
        CargarAsignacion()

        Me.upUsuariosAsignados.Update()
    End Sub

    Private Sub gvEstimacion_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEstimacion.RowCommand
        Dim idt As Int64 = Int64.Parse(Me.gvEstimacion.DataKeys(CInt(e.CommandArgument))("Id"))
        hfWfiDEstimacion.Value = ID
        hfTareaIdEstimacion.Value = Int64.Parse(Me.gvArchivosEntregables.DataKeys(CInt(e.CommandArgument))("TareaId"))
    End Sub

    Private Sub gvAsignacion_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAsignacion.RowCommand
        Dim idt As Int64 = Int64.Parse(Me.gvAsignacion.DataKeys(CInt(e.CommandArgument))("Id"))
        Dim FIniPt = Me.gvAsignacion.DataKeys(CInt(e.CommandArgument))("FIniP")
        Dim FFinPt = Me.gvAsignacion.DataKeys(CInt(e.CommandArgument))("FFinP")
        Dim fechaActual = DateTime.Now
        'Dim fechaFinal = DateTime.Parse(FFinPt.ToShortDateString().Trim() + " 23:59:59")
        'If fechaActual > fechaFinal And FFinPt IsNot Nothing Then
        '    ShowNotification("No se puede asignar una tarea que ya terminó", ShowNotifications.ErrorNotification)
        '    ScriptManager.RegisterStartupScript(Page, Me.GetType(), "OcultarUsuariosAsignados", "<script>OcultarUsuariosAsignados()</script>", False)
        '    Exit Sub
        'Else
        hfWfIdAsignacion.Value = idt
        hfWfTareaAsignacion.Value = Int64.Parse(Me.gvAsignacion.DataKeys(CInt(e.CommandArgument))("TareaId"))
        'hfRolEjecuta.Value = Me.gvAsignacion.DataKeys(CInt(e.CommandArgument))("RolEjecuta")
        CargarUsuariosDisponibles(hfRolEjecuta.Value, hfWfIdAsignacion.Value)
        'End If
    End Sub

    Sub CargarEstimacion()
        Dim o As New CT_Tareas
        Dim rolestima As Long?
        'Verifica si el rol es gerente de proyectos y si es internacional
        If hfRolEstima.Value = 6 AndAlso hfInternacional.Value = True Then
            rolestima = Nothing
        Else
            rolestima = hfRolEstima.Value
        End If
        Me.gvEstimacion.DataSource = o.TareasList(Nothing, hfIdTrabajo.Value, rolestima, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        Me.gvEstimacion.DataBind()
    End Sub

    Sub CargarAsignacion()
        Dim o As New CT_Tareas
        Dim unidadEjecuta As Long?
        'Verifica si el rol es gerente de proyectos y si es internacional
        If hfRolEstima.Value = 6 AndAlso hfInternacional.Value = True Then
            unidadEjecuta = Nothing
        Else
            unidadEjecuta = hfUnidadEjecuta.Value
        End If
        Me.gvAsignacion.DataSource = o.TareasList(Nothing, hfIdTrabajo.Value, Nothing, unidadEjecuta, 6, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        Me.gvAsignacion.DataBind()
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Me.pnlTareasXTrabajo.Visible = True
        Me.pnlEstimacionXTrabajo.Visible = False
        Me.pnlAsignacionXTrabajo.Visible = False
        Me.pnlListadoDocumentos.Visible = False
        Me.hfUsuarioAsignado.Value = "0"
        CargarTareasList()
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs) Handles LinkButton2.Click
        Me.pnlTareasXTrabajo.Visible = True
        Me.pnlEstimacionXTrabajo.Visible = False
        Me.pnlAsignacionXTrabajo.Visible = False
        Me.pnlCronograma.Visible = False
        Me.pnlListadoDocumentos.Visible = False
        Me.hfUsuarioAsignado.Value = Session("IDUsuario").ToString
        CargarTareasList()
    End Sub

    Protected Sub LinkButton3_Click(sender As Object, e As EventArgs) Handles LinkButton3.Click
        Me.pnlTareasXTrabajo.Visible = False
        Me.pnlAsignacionXTrabajo.Visible = False
        Me.pnlEstimacionXTrabajo.Visible = True
        Me.pnlCronograma.Visible = False
        Me.pnlListadoDocumentos.Visible = False
        CargarEstimacion()
    End Sub

    Protected Sub LinkButton4_Click(sender As Object, e As EventArgs) Handles LinkButton4.Click
        Me.pnlTareasXTrabajo.Visible = False
        Me.pnlEstimacionXTrabajo.Visible = False
        Me.pnlAsignacionXTrabajo.Visible = True
        Me.pnlCronograma.Visible = False
        Me.pnlListadoDocumentos.Visible = False
        CargarAsignacion()
    End Sub

    Protected Sub LinkButton5_Click(sender As Object, e As EventArgs) Handles LinkButton5.Click
        Me.pnlTareasXTrabajo.Visible = False
        Me.pnlEstimacionXTrabajo.Visible = False
        Me.pnlAsignacionXTrabajo.Visible = False
        Me.pnlCronograma.Visible = True
        Me.pnlListadoDocumentos.Visible = False
        CargarCronograma()
    End Sub

    Protected Sub LinkButton6_Click(sender As Object, e As EventArgs) Handles LinkButton6.Click
        Me.pnlTareasXTrabajo.Visible = False
        Me.pnlEstimacionXTrabajo.Visible = False
        Me.pnlAsignacionXTrabajo.Visible = False
        Me.pnlCronograma.Visible = False
        Me.pnlListadoDocumentos.Visible = True
        Me.pnlListadoDocsTotal.Visible = True
        Me.PnlListadoDocsDescargar.Visible = False
        cargarDocumentosXIdTrabajo()
    End Sub

    Protected Sub btnGuardarEstimacion_Click(sender As Object, e As EventArgs) Handles btnGuardarEstimacion.Click
        Dim oWorkFlow As New WorkFlow
        Dim oGt As New CT_Tareas
        Dim oPlaneacion As New Planeacion
        Dim o As New WorkFlow
        Dim flag As Boolean
        For Each row As GridViewRow In Me.gvEstimacion.Rows
            Dim info = oGt.ObtenerWorkFlow(Me.gvEstimacion.DataKeys(row.RowIndex)("Id"))
            Dim fechaInicio As Date?
            Dim fechaFin As Date?
            Dim esNoAplica = DirectCast(Me.gvEstimacion.Rows(row.RowIndex).FindControl("chbAplica"), CheckBox).Checked
            Dim tareaNombre = Me.gvEstimacion.DataKeys(row.RowIndex)("Tarea")
            Dim observacion = DirectCast(Me.gvEstimacion.Rows(row.RowIndex).FindControl("txtObservacionesPlaneacion"), TextBox).Text
            TryParse(DirectCast(Me.gvEstimacion.Rows(row.RowIndex).FindControl("txtFInicio"), TextBox).Text, fechaInicio)
            TryParse(DirectCast(Me.gvEstimacion.Rows(row.RowIndex).FindControl("txtFFin"), TextBox).Text, fechaFin)

            If fechaInicio.HasValue AndAlso fechaFin.HasValue AndAlso fechaInicio > fechaFin Then
                ShowNotification("En la Tarea: " + tareaNombre + " La Fecha Inicial es mayor que la Fecha Final", ShowNotifications.InfoNotification)
                Me.gvEstimacion.Rows(row.RowIndex).FindControl("txtFInicio").Focus()
                Exit For
            End If

            If planeaciónTareaCambio(row, info) Then

                info.FIniP = fechaInicio
                info.FFinP = fechaFin
                info.ObservacionesPlaneacion = observacion

                oPlaneacion.grabar(info.id, info.FIniP, info.FFinP, Session("IDUsuario").ToString, DateTime.UtcNow.AddHours(-5), info.ObservacionesPlaneacion)
                oGt.GuardarWorkFlowContext()
                Dim oEnviarCorreo As New EnviarCorreo
                oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/TareaNotificacionFecha.aspx?idTrabajo=" & hfIdTrabajo.Value & "&idTarea=" & info.TareaId & "&IdWorkFlow=" & info.id)

            End If

            If (esNoAplica AndAlso info.Estado <> LogWorkFlow.WorkFlowEstados.NoAplica) Then
                info.Estado = LogWorkFlow.WorkFlowEstados.NoAplica
            End If

            If (Not esNoAplica AndAlso info.Estado = LogWorkFlow.WorkFlowEstados.NoAplica) Then
                info.Estado = LogWorkFlow.WorkFlowEstados.Creada
            End If

            oGt.GuardarWorkFlowContext()

        Next
        CargarEstimacion()
        ShowNotification("Cambios almacenados corretamente", ShowNotifications.InfoNotification)
        ''If flag Then
        ''	oGt.GuardarWorkFlowContext()
        ''	CargarEstimacion()
        ''	ShowNotification("Cambios almacenados corretamente", ShowNotifications.InfoNotification)
        ''End If
    End Sub

    Private Function planeaciónTareaCambio(row As GridViewRow, rowBD As CORE_WorkFlow) As Boolean
        Dim oGt As New CT_Tareas
        Dim fechaInicio As Date?
        Dim fechaFin As Date?
        Dim esNoAplica = DirectCast(Me.gvEstimacion.Rows(row.RowIndex).FindControl("chbAplica"), CheckBox).Checked
        Dim observacion = DirectCast(Me.gvEstimacion.Rows(row.RowIndex).FindControl("txtObservacionesPlaneacion"), TextBox).Text
        TryParse(DirectCast(Me.gvEstimacion.Rows(row.RowIndex).FindControl("txtFInicio"), TextBox).Text, fechaInicio)
        TryParse(DirectCast(Me.gvEstimacion.Rows(row.RowIndex).FindControl("txtFFin"), TextBox).Text, fechaFin)

        If (Not Nullable.Equals(rowBD.FIniP, fechaInicio)) Then
            Return True
        End If

        If (Not Nullable.Equals(rowBD.FFinP, fechaFin)) Then
            Return True
        End If

        If (rowBD.ObservacionesPlaneacion <> observacion) Then
            Return True
        End If

        Return False

    End Function

    Private Sub gvTareasXDocumentos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareasXDocumentos.RowCommand
        hfIdDocDescarga.Value = gvTareasXDocumentos.DataKeys(CInt(e.CommandArgument))("IdDocumento")
        Me.PnlListadoDocsDescargar.Visible = True
        Me.pnlListadoDocsTotal.Visible = False
        CargarDocumentosCargados()
    End Sub

    Private Sub btnVolverDescarga_Click(sender As Object, e As System.EventArgs) Handles btnVolverDescarga.Click
        Me.pnlTareasXTrabajo.Visible = False
        Me.pnlEstimacionXTrabajo.Visible = False
        Me.pnlAsignacionXTrabajo.Visible = False
        Me.pnlCronograma.Visible = False
        Me.pnlListadoDocumentos.Visible = True
        Me.pnlListadoDocsTotal.Visible = True
        Me.PnlListadoDocsDescargar.Visible = False
    End Sub

    Private Sub gvDocumentosCargados_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDocumentosCargados.RowCommand
        If e.CommandName = "Descargar" Then
            Dim oRepositorioDocumentos As New RepositorioDocumentos
            Dim oeRepositorioDocumentos As New GD_RepositorioDocumentos
            oeRepositorioDocumentos = oRepositorioDocumentos.obtenerDocumentosXId(gvDocumentosCargados.DataKeys(CInt(e.CommandArgument))("IdDocumentoRepositorio"))
            descargarArchivos(oeRepositorioDocumentos.Url)
        End If
    End Sub

    Sub descargarArchivos(ByVal url As String)
        ' Como estás usando un directorio virtual, simplemente usa la ruta relativa desde la raíz virtual
        Dim urlRelativa As String
        urlRelativa = "ArchivosCargados/" & url.TrimStart("\") ' Asegúrate de usar las barras adecuadas para el entorno web

        ' Procede con la ruta relativa. El servidor web debería manejarla correctamente en el contexto del directorio virtual
        Dim path As New IO.FileInfo(Server.MapPath("~/" & urlRelativa))  ' Usa Server.MapPath con el prefijo "~" si es necesario

        Try
            ' Debug: Opcionalmente imprime la ruta para asegurarte de que es correcta
            Console.WriteLine("Ruta relativa del archivo: " & urlRelativa)

            Response.Clear()
            Response.ClearHeaders()
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & path.Name & """")
            Response.AddHeader("Content-Length", path.Length.ToString())
            Response.ContentType = "application/octet-stream"
            Response.WriteFile(path.FullName)
            Response.End()
        Catch ex As Exception
            ' Manejo de errores con mensaje claro
            Console.WriteLine(ex.Message)
            ShowNotification("No se puede descargar el archivo: " & ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Private Sub LinkButton7_Click(sender As Object, e As System.EventArgs) Handles LinkButton7.Click
        Select Case hfUrlRetorno.Value
            Case UrlOriginal.OP_Cuantitativo_Trabajos
                Response.Redirect("~/OP_Cuantitativo/Trabajos.aspx")
            Case UrlOriginal.OP_Cualitativo_Trabajos
                Response.Redirect("~/OP_Cualitativo/TrabajosCoordinador.aspx")
            Case UrlOriginal.PY_Proyectos_Trabajos
                Dim o As New Trabajo
                Dim p As New Proyecto
                Dim info = o.ObtenerTrabajo(hfIdTrabajo.Value)
                Dim proyecto = p.obtenerXId(info.ProyectoId)
                If proyecto.TipoProyectoId = 1 Then
                    Response.Redirect("~/PY_Proyectos/Trabajos.aspx?proyectoId=" & info.ProyectoId & "&trabajoId=" & hfIdTrabajo.Value)
                ElseIf proyecto.TipoProyectoId = 2 Then
                    Response.Redirect("~/PY_Proyectos/TrabajosCualitativos.aspx?proyectoId=" & info.ProyectoId & "&trabajoId=" & hfIdTrabajo.Value)
                End If
            Case UrlOriginal.RE_GT_TraficoTareas_Scripting
                Response.Redirect("~/RE_GT/TraficoTareas.aspx?UnidadId=11&RolId=28&URLRetorno=5")
            Case UrlOriginal.RE_GT_TraficoTareas_Pilotos
                Response.Redirect("~/RE_GT/TraficoTareas.aspx?UnidadId=12&RolId=30&URLRetorno=6")
            Case UrlOriginal.RE_GT_TraficoTareas_Critica
                Response.Redirect("~/RE_GT/TraficoTareas.aspx?UnidadId=5&RolId=22&URLRetorno=7")
            Case UrlOriginal.RE_GT_TraficoTareas_Verificacion
                Response.Redirect("~/RE_GT/TraficoTareas.aspx?UnidadId=6&RolId=23&URLRetorno=8")
            Case UrlOriginal.RE_GT_TraficoTareas_Captura
                Response.Redirect("~/RE_GT/TraficoTareas.aspx?UnidadId=7&RolId=24&URLRetorno=9")
            Case UrlOriginal.RE_GT_TraficoTareas_Codificacion
                Response.Redirect("~/RE_GT/TraficoTareas.aspx?UnidadId=8&RolId=25&URLRetorno=10")
            Case UrlOriginal.RE_GT_TraficoTareas_Datacleaning
                Response.Redirect("~/RE_GT/TraficoTareas.aspx?UnidadId=9&RolId=27&URLRetorno=11")
            Case UrlOriginal.RE_GT_TraficoTareas_Procesamiento
                Response.Redirect("~/RE_GT/TraficoTareas.aspx?UnidadId=10&RolId=26&URLRetorno=12")
            Case UrlOriginal.RE_GT_TraficoTareas_Estadistica
                Response.Redirect("~/RE_GT/TraficoTareas.aspx?UnidadId=40&RolId=33&URLRetorno=15")
            Case UrlOriginal.CORE_ListaTrabajosTareas
                Response.Redirect("Gestion-Tareas-Trabajos.aspx")
            Case UrlOriginal.RE_GT_TrabajosPorGerencia
                Response.Redirect("~/RP_Reportes/TrabajosPorGerencia.aspx")
            Case UrlOriginal.RE_GT_TraficoEncuestasRMC
                Response.Redirect("~/OP_Cuantitativo/TraficoEncuestas.aspx?UnidadId=38")
            Case UrlOriginal.RE_GT_CallCenter
                Response.Redirect("~/OP_Cuantitativo/HomeRecoleccion.aspx")
            Case UrlOriginal.RP_Reportes_TrabajosPorGrupoBU
                Response.Redirect("~/RP_Reportes/TrabajosPorGrupoBU.aspx")
        End Select
    End Sub

    Protected Sub btnInconsistencias_Click(sender As Object, e As EventArgs) Handles btnInconsistencias.Click
        Response.Redirect("../OP_Cuantitativo/IPS.aspx?idtrabajo=" & hfIdTrabajo.Value & "&IdTarea=" & hfIdWFid.Value)
    End Sub

    Protected Sub btnRevInconsistencias_Click(sender As Object, e As EventArgs) Handles btnRevInconsistencias.Click
        Response.Redirect("../OP_Cuantitativo/IPS.aspx?idtrabajo=" & hfIdTrabajo.Value & "&IdTarea=" & hfIdWFid.Value & "&fromgerencia=1")
    End Sub

    Private Sub btnInconsistenciasCuali_Click(sender As Object, e As EventArgs) Handles btnInconsistenciasCuali.Click
        Response.Redirect("../OP_Cualitativo/IPSCuali.aspx?idtrabajo=" & hfIdTrabajo.Value & "&IdTarea=" & hfIdWFid.Value)
    End Sub

    Private Sub btnRevInconsistenciasCuali_Click(sender As Object, e As EventArgs) Handles btnRevInconsistenciasCuali.Click
        Response.Redirect("../OP_Cualitativo/IPSCuali.aspx?idtrabajo=" & hfIdTrabajo.Value & "&IdTarea=" & hfIdWFid.Value & "&fromgerencia=1")
    End Sub

    Sub cargarObservacionesTarea(core_WorkFlow_Id As Int64)
        Dim daObservaciones As New WorkFlow
        gvObservaciones.DataSource = daObservaciones.obtenerObservacionesXTarea(core_WorkFlow_Id)
        gvObservaciones.DataBind()
    End Sub

    Private Sub gvObservaciones_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvObservaciones.PageIndexChanging
        gvObservaciones.PageIndex = e.NewPageIndex
        cargarObservacionesTarea(hfIdWFid.Value)
    End Sub

    Public Function CrearTablaGantt(data As List(Of CT_TareasList_Result)) As Gantt
        Dim Cronograma = New Gantt()
        Dim FechaInicial = Nothing
        Dim FechaFinal = Nothing
        Dim ListaSerie = New List(Of serie)
        Dim dependency = Nothing
        Dim c = 0
        For Each row As CT_TareasList_Result In data
            c += 1
            If FechaInicial Is Nothing Then
                FechaInicial = row.FIniP
            Else
                Dim FechaMenor = Convert.ToDateTime(FechaInicial)
                Dim FechaActual = Convert.ToDateTime(row.FIniP)
                If (Not (row.FIniP Is Nothing)) Then
                    If FechaActual.Date < FechaMenor.Date Then
                        FechaInicial = FechaActual.ToString("dd/MM/yyyy")
                    End If
                End If
            End If

            If FechaFinal Is Nothing Then
                FechaFinal = row.FFinP
            Else
                Dim FechaMayor = Convert.ToDateTime(FechaFinal)
                Dim FechaActual = Convert.ToDateTime(row.FFinP)
                If (Not (row.FFinP Is Nothing)) Then
                    If FechaActual.Date > FechaMayor.Date Then
                        FechaFinal = FechaActual.ToString("dd/MM/yyyy")
                    End If
                End If
            End If

            Dim S As New serie()
            S.name = row.TAREA
            S.id = row.TareaId
            S.parent = "cronograma_tareas"
            S.dependency = dependency
            S.fstart = Format(row.FIniP, "dd/MM/yyyy")
            S.fend = Format(row.FFinP, "dd/MM/yyyy")
            S.owner = row.Responsable
            If (Not (S.fstart Is Nothing)) Then
                ListaSerie.Add(S)
                dependency = S.id
            End If
        Next

        Cronograma.FechaIni = FechaInicial
        Cronograma.FechaFin = FechaFinal
        Cronograma.series = ListaSerie

        Return Cronograma
    End Function

    Shared Function SerializarAJSON(Of T)(ByVal objeto As T) As String
        Dim JSON As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer
        Return JSON.Serialize(objeto)
    End Function

    Private Sub li_Gantt_Cronograma_ServerClick(sender As Object, e As EventArgs) Handles li_Gantt_Cronograma.Click
        Gantt_Cronograma.Visible = True
        Tabla_Cronograma.Visible = False
        CargarCronograma()
    End Sub

    Private Sub li_Tabla_Cronograma_ServerClick(sender As Object, e As EventArgs) Handles li_Tabla_Cronograma.Click
        Gantt_Cronograma.Visible = False
        Tabla_Cronograma.Visible = True
    End Sub

    Protected Sub btnDescarga_Click(sender As Object, e As EventArgs) Handles btnDescarga.Click
        Dim cronograma As List(Of CT_TareasList_Result) = Session("Cronograma")
        Dim listaTareas = New List(Of TareaCronograma)

        For Each row As CT_TareasList_Result In cronograma
            Dim t = New TareaCronograma()
            t.id = row.ID
            t.Tarea = row.TAREA
            t.FechaIniP = row.FIniP
            t.FechaFinP = row.FFinP
            t.FechaIniR = If(Not (row.FIniR Is Nothing), row.FIniR, "")
            t.FechaFinR = If(Not (row.FFinR Is Nothing), row.FFinR, "")
            t.ObservacionesP = row.OBSERVACIONESPLANEACION
            t.ObservacionesR = row.OBSERVACIONESEJECUCION
            t.Estado = row.Estado
            t.UsuarioAsignado = row.Responsable

            listaTareas.Add(t)
        Next

        Dim wb As New XLWorkbook

        Dim titulosProduccion As String = "Id;Tarea;FechaIniP;FechaFinP;FechaIniR;FechaFinR;ObservacionesP;ObservacionesR;Estado;UsuarioAsignado"

        Dim ws = wb.Worksheets.Add("Cronograma")
        insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

        ws.Cell(2, 1).InsertData(listaTareas)

        exportarExcel(wb, "Cronograma")
    End Sub

    Private Sub exportarExcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Dim Script = ScriptManager.GetCurrent(Me.Page)
        Script.RegisterPostBackControl(btnDescarga)
        Response.End()
    End Sub
    Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String, ByVal fila As Integer)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(fila, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub
    Private Function TryParse(text As String, ByRef outDate As Date?) As Boolean
        Dim fecha As Date
        Dim isParsed = Date.TryParse(text, fecha)
        If (isParsed) Then
            outDate = New Nullable(Of Date)(fecha)
        Else
            outDate = New Nullable(Of Date)
        End If
        Return isParsed
    End Function

    Public Class TareaCronograma
        Public Property id As String
        Public Property Tarea As String
        Public Property FechaIniP As String
        Public Property FechaFinP As String
        Public Property FechaIniR As String
        Public Property FechaFinR As String
        Public Property ObservacionesP As String
        Public Property ObservacionesR As String
        Public Property Estado As String
        Public Property UsuarioAsignado As String
    End Class

    Public Class Gantt
        Private _FechaIni As String
        Private _FechaFin As String
        Private _series As List(Of serie)
        Public Property FechaIni() As String
            Get
                Return _FechaIni
            End Get
            Set(ByVal value As String)
                _FechaIni = value
            End Set
        End Property
        Public Property FechaFin() As String
            Get
                Return _FechaFin
            End Get
            Set(ByVal value As String)
                _FechaFin = value
            End Set
        End Property
        Public Property series() As List(Of serie)
            Get
                Return _series
            End Get
            Set(ByVal value As List(Of serie))
                _series = value
            End Set
        End Property
    End Class

    Public Class serie
        Private _name As String
        Private _id As String
        Private _parent As String
        Private _dependency As String
        Private _fstart As String
        Private _fend As String
        Private _owner As String

        Public Property name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property
        Public Property id() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property
        Public Property parent() As String
            Get
                Return _parent
            End Get
            Set(ByVal value As String)
                _parent = value
            End Set
        End Property
        Public Property dependency() As String
            Get
                Return _dependency
            End Get
            Set(ByVal value As String)
                _dependency = value
            End Set
        End Property
        Public Property fstart() As String
            Get
                Return _fstart
            End Get
            Set(ByVal value As String)
                _fstart = value
            End Set
        End Property
        Public Property fend() As String
            Get
                Return _fend
            End Get
            Set(ByVal value As String)
                _fend = value
            End Set
        End Property
        Public Property owner() As String
            Get
                Return _owner
            End Get
            Set(ByVal value As String)
                _owner = value
            End Set
        End Property
    End Class
End Class