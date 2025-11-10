Imports System.IO
Imports ClosedXML.Excel
'Imports Microsoft.Office.Interop
Imports WebMatrix.Util
Public Class ProgramacionCampo
    Inherits System.Web.UI.Page

#Region "Enumerados"
    Enum TipoProyectoEnum
        Cuali = 2
        Cuanti = 1
    End Enum

    Enum EstadosProgramacionCampo
        Creado = 1
        Programado = 2
        Realizado = 3
        EntregaArchivos = 4
        NoCumpleFiltros = 5
        CanceladaPorEntrevistado = 6
        CanceladaPorIpsos = 7
    End Enum
#End Region

#Region "Objetos"
    Public Class dataEntrevistados
        Public Audio As String
        Public Transcripcion As String
        Public Estado As String
        Public Fecha As Date
        Public Hora As String
        Public Moderador As String
        Public Nombre As String
        Public Ciudad As String
        Public Direccion As String
        Public Perfil As String
        Public Telefono As String
        Public Celular As String
        Public Reclutador As String
        Public Observaciones As String
    End Class

#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtFechaNacimiento.Attributes.Add("onblur", "calcularFecha();")
            If Request.QueryString("idtrabajo") IsNot Nothing Then
                Dim idtrabajo As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString)
                hfTrabajoId.Value = idtrabajo
                Dim oTrabajo = New CoreProject.Trabajo
                Dim trabajo = oTrabajo.ObtenerTrabajosCualitativosxTrabajo(idtrabajo, TipoProyectoEnum.Cuali)

                CargarProgramadosEntrevistados(idtrabajo)
                'cargarProgramarFecha()

                CargarEstadoCivil()
                CargarSexo()
                CargarNivelEducativo()
                CargarModeradores()
                CargarReclutadores()
                lbtnVolver.PostBackUrl = "~/OP_Cualitativo/TrabajosCoordinador.aspx?IdTrabajo=" & idtrabajo
            Else
                Response.Redirect("FichaEntrevista.aspx")
            End If
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Guardar()
        cargarFechaNacimiento()
    End Sub

    Private Sub btnSaveProgramar_Click(sender As Object, e As EventArgs) Handles btnSaveProgramar.Click
        ProgramarCita()
    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        Limpiar()
        Session("Id") = Nothing
        Session("EntrevistadoId") = Nothing
        cargarFechaNacimiento()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Limpiar()
        Session("Id") = Nothing
        Session("EntrevistadoId") = Nothing
        cargarFechaNacimiento()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub btnDescargar_Click(sender As Object, e As EventArgs) Handles btnDescargar.Click
        Dim oProgramados As New CoreProject.CampoCualitativo
        Dim Programados_Entrevistados As List(Of CoreProject.OP_Programados_Entrevistados_Cuali_Get_Result) = Session("ProgramadosEntrevistados")
        Dim dataProgramados As New List(Of dataEntrevistados)

        For Each Entrevistado In Programados_Entrevistados
            Dim ent As New dataEntrevistados
            ent.Audio = Entrevistado.Audio
            ent.Transcripcion = Entrevistado.Transcripcion
            ent.Estado = Entrevistado.Estado
            ent.Fecha = Entrevistado.Fecha
            ent.Hora = Entrevistado.Hora
            ent.Moderador = Entrevistado.Moderador
            ent.Nombre = Entrevistado.Nombre
            ent.Ciudad = Entrevistado.Ciudad
            ent.Direccion = Entrevistado.Direccion
            ent.Perfil = Entrevistado.Perfil
            ent.Telefono = Entrevistado.Telefono
            ent.Celular = Entrevistado.Celular
            ent.Reclutador = Entrevistado.Reclutador
            ent.Observaciones = oProgramados.ObtenerObsEntrevistadosCuali(Entrevistado.TrabajoId, Entrevistado.EntrevistadoId).FirstOrDefault

            dataProgramados.Add(ent)
        Next

        If Programados_Entrevistados.Count <= 0 Then
            ShowNotification("No hay Entrevistados para descargar excel", ShowNotifications.ErrorNotification)
            Exit Sub
        End If

        Dim wb As New XLWorkbook

        Dim titulosProduccion As String = "Audio;Transcripcion;Estado;Fecha;Hora;Moderador;Nombre;Ciudad;Dirección;Perfil;Tel;Cel;Reclutador;Observaciones"

        Dim ws = wb.Worksheets.Add("Programados Entrevistados")
        ws.Cell(1, 1).Value = "CRC - Job "
        ws.Range("A1:N1").Merge()
        insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 2)

        ws.Cell(3, 1).InsertData(dataProgramados)

        exportarExcel(wb, "Programados Entrevistados")
    End Sub

    Private Sub gvCamposProgramados_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCamposProgramados.RowCommand
        Dim id = 0
        Dim EntrevistadoId = 0
        If e.CommandName <> "Page" Then
            id = gvCamposProgramados.DataKeys(CInt(e.CommandArgument))("Id")
            Session("Id") = id
            EntrevistadoId = gvCamposProgramados.DataKeys(CInt(e.CommandArgument))("EntrevistadoId")
            Session("EntrevistadoId") = EntrevistadoId
        End If

        If e.CommandName = "Programar" Then
            Dim oProgramados As New CoreProject.CampoCualitativo
            Dim ProgramadosEntrevistados = oProgramados.ObtenerProgramadosEntrevistadosCuali(Nothing, Nothing, EntrevistadoId, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).LastOrDefault

            If Not ProgramadosEntrevistados Is Nothing Then
                If ProgramadosEntrevistados.EntrevistadoId <> 0 Then
                    txtProgramarFecha.Text = If(Not ProgramadosEntrevistados.Fecha Is Nothing, String.Format("{0:dd/MM/yyyy}", ProgramadosEntrevistados.Fecha), "")
                    txtProgramarHora.Text = If(Not ProgramadosEntrevistados.Hora Is Nothing, ProgramadosEntrevistados.Hora.ToString, "")
                    ddlModerador.SelectedValue = If(Not ProgramadosEntrevistados.IdModerador Is Nothing, ProgramadosEntrevistados.IdModerador, "-1")
                    CargarActualizacion(ProgramadosEntrevistados.EstadoId, ProgramadosEntrevistados.Audio, ProgramadosEntrevistados.Transcripcion)

                    If ProgramadosEntrevistados.EstadoId = EstadosProgramacionCampo.Creado Then
                        divProgramarCita.Visible = True
                        divModerador.Visible = True
                        divAudioTranscripcion.Visible = False
                        btnCancelar.Visible = False
                        ddlEstadoCancelar.Visible = False
                    ElseIf ProgramadosEntrevistados.EstadoId = EstadosProgramacionCampo.Programado Then
                        divProgramarCita.Visible = False
                        divModerador.Visible = True
                        divAudioTranscripcion.Visible = True
                        btnCancelar.Visible = True
                        ddlEstadoCancelar.Visible = False
                    ElseIf (ProgramadosEntrevistados.EstadoId = EstadosProgramacionCampo.CanceladaPorIpsos Or ProgramadosEntrevistados.EstadoId = EstadosProgramacionCampo.CanceladaPorEntrevistado) Then
                        divProgramarCita.Visible = False
                        divModerador.Visible = False
                        divAudioTranscripcion.Visible = False
                        btnCancelar.Visible = False
                        ddlEstadoCancelar.Visible = False
                    Else
                        divProgramarCita.Visible = False
                        divModerador.Visible = False
                        divAudioTranscripcion.Visible = True
                        btnCancelar.Visible = False
                        ddlEstadoCancelar.Visible = False
                    End If
                    txtProgramarObservacion.Focus()
                Else
                    LimpiarProgramarCita()
                    ScriptManager.RegisterStartupScript(Page, Me.GetType(), "cerrarProgramar", "<script>cerrarProgramar()</script>", False)
                    ShowNotification("No se encontraron datos de este entrevistado. Intente nuevamente o Recargue la página.", ShowNotifications.InfoNotification)
                End If
            End If
        ElseIf e.CommandName = "Modificar" Then
            If EntrevistadoId = 0 Then
                ShowNotification("No se pueden ver los datos del Entrevistado indicado. Intente nuevamente o Recargue la página.", ShowNotifications.InfoNotification)
            Else
                CargarDatosEntrevistados()

                ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            End If
        End If
    End Sub

    Private Sub gvCamposProgramados_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvCamposProgramados.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim NumeroObservaciones As String = DataBinder.Eval(e.Row.DataItem, "NumeroObservaciones").ToString()
            Dim _NumObs = 11

            If NumeroObservaciones > 0 Then
                e.Row.Cells(_NumObs).CssClass = "obs-verde text-center"
            Else
                e.Row.Cells(_NumObs).CssClass = "obs-rojo text-center"
            End If
        End If
    End Sub

    Private Sub gvCamposProgramados_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvCamposProgramados.PageIndexChanging
        Dim idtrabajo As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString)
        gvCamposProgramados.PageIndex = e.NewPageIndex
        CargarProgramadosEntrevistados(idtrabajo)
    End Sub

    Private Sub btnObservaciones_Click(sender As Object, e As EventArgs) Handles btnObservaciones.Click
        Dim idtrabajo As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString)
        Dim Entrevistado = Session("EntrevistadoId").ToString
        CargarObservaciones(idtrabajo, Entrevistado)
    End Sub

    Private Sub gvObservaciones_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvObservaciones.PageIndexChanging
        Dim idtrabajo As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString)
        Dim Entrevistado = Session("EntrevistadoId").ToString
        gvObservaciones.PageIndex = e.NewPageIndex
        CargarObservaciones(idtrabajo, Entrevistado)
    End Sub

    Private Sub btnSearchNombre_Click(sender As Object, e As EventArgs) Handles btnSearchNombre.Click
        CargarProgramadosEntrevistados(hfTrabajoId.Value, txtSearchNombre.Text)
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        ddlEstadoCancelar.Visible = True
        btnCancelar.Visible = False
    End Sub

    Private Sub ddlEstadoCancelar_TextChanged(sender As Object, e As EventArgs) Handles ddlEstadoCancelar.TextChanged
        Cancelar()
        ddlEstadoCancelar.Visible = False
        btnCancelar.Visible = False
    End Sub

    Private Sub btnSubirData_Click(sender As Object, e As EventArgs) Handles btnSubirData.Click
        If FileUpData.HasFile Then
            Dim savePath As String = "~/Files/Temp/"
            Dim fileload As New FileInfo(FileUpData.FileName)
            savePath = SaveFile(FileUpData.PostedFile, FileUpData.FileName, Server.MapPath(savePath))
            Dim Lista = getDataExcel(savePath)
            If Lista > 0 Then
                ShowNotification("Se Subieron " + Lista.ToString + " entrevistados", ShowNotifications.InfoNotification)
            Else
                ShowNotification("No se encuentra en el archivo ninguna hoja 'CierreCampo'", ShowNotifications.InfoNotification)
                Exit Sub
            End If
        Else
            ShowNotification("Seleccione un archivo para subir los datos de los entrevistados", ShowNotifications.InfoNotification)
            Exit Sub
        End If
        CargarProgramadosEntrevistados(hfTrabajoId.Value)
        ScriptManager.RegisterStartupScript(Page, Me.GetType(), "cerrarObservaciones", "<script>abrirsubirDatos()</script>", False)
    End Sub

#End Region

#Region "Metodos"
    Sub CargarProgramadosEntrevistados(ByVal idtrabajo As Int64)
        Dim oProgramados As New CoreProject.CampoCualitativo
        Dim ProgramadosEntrevistados = oProgramados.ObtenerProgramadosEntrevistadosCuali(Nothing, idtrabajo, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        Session.Add("ProgramadosEntrevistados", ProgramadosEntrevistados)
        gvCamposProgramados.DataSource = ProgramadosEntrevistados
        gvCamposProgramados.DataBind()
        btnCancelar.Visible = False
        ddlEstadoCancelar.Visible = False
    End Sub

    Sub CargarProgramadosEntrevistados(ByVal idtrabajo As Int64, ByVal nombre As String)
        Dim oProgramados As New CoreProject.CampoCualitativo
        Dim ProgramadosEntrevistados = oProgramados.ObtenerProgramadosEntrevistadosCuali(Nothing, idtrabajo, Nothing, Nothing, Nothing, nombre, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        gvCamposProgramados.DataSource = ProgramadosEntrevistados
        gvCamposProgramados.DataBind()
        btnCancelar.Visible = False
        ddlEstadoCancelar.Visible = False
    End Sub

    Sub CargarEstadoCivil()
        Dim o As New CoreProject.RegistroPersonas
        ddlEstadoCivil.DataSource = o.EstadoCivilList
        ddlEstadoCivil.DataValueField = "id"
        ddlEstadoCivil.DataTextField = "EstadoCivil"
        ddlEstadoCivil.DataBind()
        ddlEstadoCivil.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarSexo()
        Dim o As New CoreProject.RegistroPersonas
        ddlSexo.DataSource = o.SexoList
        ddlSexo.DataValueField = "id"
        ddlSexo.DataTextField = "Sexo"
        ddlSexo.DataBind()
        ddlSexo.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarNivelEducativo()
        Dim o As New CoreProject.RegistroPersonas
        ddlNivelEducativo.DataSource = o.NivelEducativoList
        ddlNivelEducativo.DataValueField = "id"
        ddlNivelEducativo.DataTextField = "NivelEducativo"
        ddlNivelEducativo.DataBind()
        ddlNivelEducativo.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarModeradores()
        Dim oProgramados As New CoreProject.CampoCualitativo
        Dim moderadores = oProgramados.obtenerModeradoresCuali()

        ddlModerador.DataSource = moderadores
        ddlModerador.DataTextField = "Nombre"
        ddlModerador.DataValueField = "id"
        ddlModerador.DataBind()
        ddlModerador.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})

        ddlModeradorSubir.DataSource = moderadores
        ddlModeradorSubir.DataTextField = "Nombre"
        ddlModeradorSubir.DataValueField = "id"
        ddlModeradorSubir.DataBind()
        ddlModeradorSubir.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarReclutadores()
        Dim oProgramados As New CoreProject.CampoCualitativo
        Dim reclutadores = oProgramados.obtenerReclutadoresCuali()
        ddlReclutador.DataSource = reclutadores
        ddlReclutador.DataTextField = "Nombre"
        ddlReclutador.DataValueField = "id"
        ddlReclutador.DataBind()
        ddlReclutador.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarObservaciones(ByVal idTrabajo As Int64, ByVal Entrevistado As Int64)
        If idTrabajo = 0 Or Entrevistado = 0 Then
            ShowNotification("No se pueden cargar las observaciones", ShowNotifications.ErrorNotification)
            ScriptManager.RegisterStartupScript(Page, Me.GetType(), "cerrarObservaciones", "<script>cerrarObservaciones()</script>", False)
        Else
            Dim oProgramados As New CoreProject.CampoCualitativo
            Dim ObservacionesCuali = oProgramados.obtenerObservacionesCualixTrabajoxEntrevistado(idTrabajo, Entrevistado)
            gvObservaciones.DataSource = ObservacionesCuali
            gvObservaciones.DataBind()
        End If
    End Sub

    Sub Guardar()
        Dim idEntrevistado = 0
        Dim oEntrevistado As New CoreProject.CampoCualitativo
        Dim Entrevistados_Cuali_Get As New CoreProject.OP_Entrevistados_Cuali_Get_Result
        Dim Entrevistado As New CoreProject.OP_Entrevistados_Cuali

        If Not Session("EntrevistadoId") Is Nothing Then
            idEntrevistado = Convert.ToInt64(Session("EntrevistadoId").ToString)
            Entrevistados_Cuali_Get = oEntrevistado.obtenerEntrevistadosCualixId(idEntrevistado)
            If Not Entrevistados_Cuali_Get Is Nothing Then
                Entrevistado = oEntrevistado.emparejarEntrevistado(Entrevistados_Cuali_Get)
            End If
        End If

        If Trim(txtNombre.Text) = "" Then
            ShowNotification("El Nombre del entrevistado es requerido", ShowNotifications.InfoNotification)
            txtNombre.Focus()
            Exit Sub
        Else
            Entrevistado.Nombre = txtNombre.Text
        End If

        If txtCC.Text = "" Then
            Entrevistado.Documento = Nothing
        Else
            Entrevistado.Documento = txtCC.Text
        End If

        If Trim(txtTelefono.Text) = "" And Trim(txtCelular.Text) = "" Then
            ShowNotification("Al menos 1 Teléfono de Contacto del entrevistado es requerido", ShowNotifications.InfoNotification)
            txtCelular.Focus()
            Exit Sub
        Else
            Entrevistado.Telefono = txtTelefono.Text
            Entrevistado.Celular = txtCelular.Text
        End If

        If txtFechaNacimiento.Text = "" Then
            Entrevistado.FechaNacimiento = Nothing
        Else
            Entrevistado.FechaNacimiento = txtFechaNacimiento.Text
        End If

        If Trim(txtEdad.Text) = "" Then
            ShowNotification("La Edad del entrevistado es requerida", ShowNotifications.InfoNotification)
            txtEdad.Focus()
            Exit Sub
        Else
            Entrevistado.Edad = txtEdad.Text
        End If

        Entrevistado.EstadoCivil = ddlEstadoCivil.SelectedValue

        If ddlSexo.SelectedValue = "-1" Then
            Entrevistado.Genero = Nothing
        Else
            Entrevistado.Genero = ddlSexo.SelectedValue
        End If

        If Trim(txtDireccion.Text) = "" Then
            Entrevistado.Direccion = Nothing
        Else
            Entrevistado.Direccion = txtDireccion.Text
        End If

        If ddlEstrato.SelectedValue = "-1" Then
            Entrevistado.Estrato = Nothing
        Else
            Entrevistado.Estrato = ddlEstrato.SelectedValue
        End If

        Entrevistado.NivelEscolaridad = ddlNivelEducativo.SelectedValue

        Dim Reclutador As String = ""
        If Trim(ddlReclutador.SelectedValue) = "-1" Then
            ShowNotification("El Reclutador del entrevistado es requerido", ShowNotifications.InfoNotification)
            ddlReclutador.Focus()
            Exit Sub
        Else
            Reclutador = ddlReclutador.SelectedValue
        End If

        Entrevistado.Edad = txtEdad.Text
        Entrevistado.Ciudad = txtCiudadResidencia.Text
        Entrevistado.Barrio = txtBarrio.Text
        Entrevistado.Correo = txtCorreo.Text
        Entrevistado.Perfil = txtPerfil.Text

        If Not hfTrabajoId.Value Is Nothing Or hfTrabajoId.Value = 0 Then
            If Entrevistado.Id = 0 Then
                Entrevistado.FechaCreacion = DateAndTime.Now.ToString
                Entrevistado.UsuarioCreacion = Session("IDUsuario")
                idEntrevistado = oEntrevistado.GuardarEntrevistado(Entrevistado)

                If idEntrevistado <> 0 Then
                    Dim id = GuardarProgramacionCampo(hfTrabajoId.Value, idEntrevistado, ddlReclutador.SelectedValue, 0, 0, EstadosProgramacionCampo.Creado, Nothing, "1", Nothing)
                    Limpiar()
                    ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
                    GuardarLogProgramacionCampo(id, EstadosProgramacionCampo.Creado, "", Session("IDUsuario"), Date.UtcNow.AddHours(-5))
                    ShowNotification("El Entrevistador fue agregado correctamente", ShowNotifications.InfoNotification)
                    CargarProgramadosEntrevistados(hfTrabajoId.Value)
                End If
            Else
                idEntrevistado = oEntrevistado.GuardarEntrevistado(Entrevistado)

                Dim ProgramacionCampo = oEntrevistado.ObtenerProgramadosEntrevistadosCualiXId(idEntrevistado)
                ProgramacionCampo.Reclutador = ddlReclutador.SelectedValue

                ActualizarProgramacionCampo(ProgramacionCampo)
                Limpiar()
                ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
                ShowNotification("El Entrevistador fue actualizado correctamente", ShowNotifications.InfoNotification)
                CargarProgramadosEntrevistados(hfTrabajoId.Value)
            End If
        Else
            ShowNotification("No se encontró una tarea vinculada a este nuevo entrevistador", ShowNotifications.ErrorNotification)
        End If
    End Sub

    Sub ProgramarCita()
        Dim id = Session("Id").ToString
        Dim idEntrevistado = Convert.ToInt64(Session("EntrevistadoId").ToString)
        Dim oProgramados As New CoreProject.CampoCualitativo
        Dim ProgramacionCampo As New CoreProject.OP_Programacion_Campo_Cuali

        If id = "" Then
            LimpiarProgramarCita()
            ScriptManager.RegisterStartupScript(Page, Me.GetType(), "cerrarProgramar", "<script>cerrarProgramar()</script>", False)
            ShowNotification("No se encontraron datos de este entrevistado. Intente nuevamente o Recargue la página.", ShowNotifications.InfoNotification)
            Exit Sub
        Else
            ProgramacionCampo = oProgramados.ObtenerProgramadosEntrevistadosCualiXId(id)
        End If

        If ProgramacionCampo.Estado = EstadosProgramacionCampo.Creado Then
            If Trim(txtProgramarFecha.Text) = "" Then
                ShowNotification("La Fecha para Programar la Cita es requerida", ShowNotifications.InfoNotification)
                Exit Sub
            Else
                ProgramacionCampo.Fecha = Convert.ToDateTime(txtProgramarFecha.Text)
            End If

            If Trim(txtProgramarHora.Text) = "" Then
                ShowNotification("La Hora para Programar la Cita es requerida", ShowNotifications.InfoNotification)
                txtProgramarHora.Focus()
                Exit Sub
            Else
                ProgramacionCampo.Hora = txtProgramarHora.Text
            End If

            If Trim(ddlModerador.SelectedValue) = "-1" Then
                ShowNotification("El Moderador para Programar la Cita es requerido", ShowNotifications.InfoNotification)
                Exit Sub
            Else
                ProgramacionCampo.Moderador = ddlModerador.SelectedValue
            End If

            ProgramacionCampo.Estado = EstadosProgramacionCampo.Programado
            ProgramacionCampo.Filtro = 1
            id = oProgramados.GuardarProgramarCampo(ProgramacionCampo)
            GuardarLogProgramacionCampo(ProgramacionCampo.id, ProgramacionCampo.Estado, txtProgramarObservacion.Text, Session("IDUsuario"), Date.UtcNow.AddHours(-5))
        End If

        Dim audio = chbAudio.Checked
        Dim transcripcion = chbTranscripcion.Checked
        Dim realizado = chbRealizado.Checked
        Dim obEstado = ""

        ProgramacionCampo.Audio = If(audio = True, 1, 0)
        ProgramacionCampo.Transcripcion = If(transcripcion = True, 1, 0)
        If realizado = True And ProgramacionCampo.Estado < EstadosProgramacionCampo.Realizado Then
            ProgramacionCampo.Estado = EstadosProgramacionCampo.Realizado
            obEstado = "Se realiza la entrevista"
            ActualizarProgramacionCampo(ProgramacionCampo)
            GuardarLogProgramacionCampo(ProgramacionCampo.id, ProgramacionCampo.Estado, obEstado, Session("IDUsuario"), Date.UtcNow.AddHours(-5))
        End If

        If ProgramacionCampo.Audio <> Convert.ToInt32(audio) Or ProgramacionCampo.Transcripcion <> Convert.ToInt32(transcripcion) Then
            ActualizarProgramacionCampo(ProgramacionCampo)
        End If

        If (audio = True And transcripcion = True) And ProgramacionCampo.Estado < EstadosProgramacionCampo.EntregaArchivos Then
            ProgramacionCampo.Estado = EstadosProgramacionCampo.EntregaArchivos
            ActualizarProgramacionCampo(ProgramacionCampo)
            obEstado = "Se Entregan los Audios y las Transcripciones"
            GuardarLogProgramacionCampo(ProgramacionCampo.id, ProgramacionCampo.Estado, obEstado, Session("IDUsuario"), Date.UtcNow.AddHours(-5))
        End If

        If Trim(txtProgramarObservacion.Text) <> "" Then
            Dim idObservacion = oProgramados.GuardarObsProgramarCampo(ProgramacionCampo, txtProgramarObservacion.Text, Session("IDUsuario"))
        End If

        LimpiarActualizacion()
        LimpiarProgramarCita()
        ScriptManager.RegisterStartupScript(Page, Me.GetType(), "cerrarProgramar", "<script>cerrarProgramar()</script>", False)
        ShowNotification("Se Programó/Actualizó el campo correctamente", ShowNotifications.InfoNotification)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        CargarProgramadosEntrevistados(hfTrabajoId.Value)
    End Sub

    Sub Limpiar()
        txtNombre.Text = ""
        txtCC.Text = ""
        txtCelular.Text = ""
        txtFechaNacimiento.Text = ""
        ddlEstadoCivil.SelectedValue = "-1"
        ddlSexo.SelectedValue = "-1"
        txtCiudadResidencia.Text = ""
        txtDireccion.Text = ""
        ddlEstrato.SelectedValue = "-1"
        ddlNivelEducativo.SelectedValue = "-1"
        txtTelefono.Text = ""
        txtEdad.Text = ""
        txtBarrio.Text = ""
        txtCorreo.Text = ""
        txtPerfil.Text = ""
        ddlReclutador.SelectedValue = "-1"
    End Sub

    Sub CargarActualizacion(estado As Int32?, audio As String, transcripcion As String)
        If estado = EstadosProgramacionCampo.Realizado Or estado = EstadosProgramacionCampo.EntregaArchivos Then
            chbRealizado.Checked = True
        Else
            chbRealizado.Checked = False
        End If

        If audio = "Sí" Then
            chbAudio.Checked = True
        Else
            chbAudio.Checked = False
        End If

        If transcripcion = "Sí" Then
            chbTranscripcion.Checked = True
        Else
            chbTranscripcion.Checked = False
        End If

    End Sub

    Sub LimpiarActualizacion()
        chbAudio.Checked = False
        chbTranscripcion.Checked = True
    End Sub
    Sub GuardarLogProgramacionCampo(ByVal id As Int64, ByVal Estado As Int64, ByVal Observaciones As String, ByVal Usuario As Int64, ByVal Fecha As DateTime)
        Dim oEntrevistado As New CoreProject.CampoCualitativo
        Dim LogProgramacionCampo As New CoreProject.OP_Log_Programacion_Campo_Cuali
        LogProgramacionCampo.idProgramacionCampoCuali = id
        LogProgramacionCampo.Estado = Estado
        LogProgramacionCampo.Observaciones = Observaciones
        LogProgramacionCampo.Usuario = Usuario
        LogProgramacionCampo.Fecha = Fecha

        oEntrevistado.GuardarLogProgramacionCampo(LogProgramacionCampo)
    End Sub

    Function GuardarProgramacionCampo(ByVal trabajoId As Int64?, ByVal EntrevistadoId As Int64?, ByVal Reclutador As Int64?, ByVal Audio As Int16?, ByVal Transcripcion As Int16?, ByVal Estado As Int64?, ByVal Moderador As Int64?, ByVal Filtro As String, ByVal Observaciones As String) As Integer
        Dim oEntrevistado As New CoreProject.CampoCualitativo
        Dim ProgramacionCampo As New CoreProject.OP_Programacion_Campo_Cuali
        ProgramacionCampo.TrabajoId = trabajoId
        ProgramacionCampo.EntrevistadoId = EntrevistadoId
        ProgramacionCampo.Reclutador = Reclutador
        ProgramacionCampo.Audio = Audio
        ProgramacionCampo.Transcripcion = Transcripcion
        ProgramacionCampo.Estado = Estado
        ProgramacionCampo.Moderador = Moderador
        ProgramacionCampo.Filtro = Filtro
        ProgramacionCampo.Observaciones = Observaciones

        oEntrevistado.GuardarProgramacionCampo(ProgramacionCampo)
        Return ProgramacionCampo.id
    End Function

    Sub ActualizarProgramacionCampo(ByVal ProgramacionCampo As CoreProject.OP_Programacion_Campo_Cuali)
        Dim oEntrevistado As New CoreProject.CampoCualitativo
        oEntrevistado.GuardarProgramacionCampo(ProgramacionCampo)
    End Sub

    Sub LimpiarProgramarCita()
        txtProgramarFecha.Text = ""
        txtProgramarHora.Text = ""
        txtProgramarObservacion.Text = ""
        ddlModerador.SelectedValue = -1
    End Sub

    Sub Cancelar()
        Dim id = Session("Id").ToString
        Dim oProgramados As New CoreProject.CampoCualitativo
        Dim ProgramacionCampo As New CoreProject.OP_Programacion_Campo_Cuali

        If ddlEstadoCancelar.SelectedValue = -1 Then
            ShowNotification("Se debe indicar quién Canceló el campo correctamente", ShowNotifications.InfoNotification)
            Exit Sub
        Else
            If id = "" Then
                LimpiarProgramarCita()
                ScriptManager.RegisterStartupScript(Page, Me.GetType(), "cerrarProgramar", "<script>cerrarProgramar()</script>", False)
                ShowNotification("No se encontraron datos de este entrevistado. Intente nuevamente o Recargue la página.", ShowNotifications.InfoNotification)
                Exit Sub
            Else
                ProgramacionCampo = oProgramados.ObtenerProgramadosEntrevistadosCualiXId(id)
            End If

            If ProgramacionCampo.Estado = 2 Then
                Dim estado = 0
                If ddlEstadoCancelar.SelectedValue = 6 Then
                    estado = EstadosProgramacionCampo.CanceladaPorEntrevistado
                ElseIf ddlEstadoCancelar.SelectedValue = 7 Then
                    estado = EstadosProgramacionCampo.CanceladaPorIpsos
                Else
                    ShowNotification("Se debe indicar quién Canceló el campo correctamente", ShowNotifications.InfoNotification)
                    Exit Sub
                End If
                ProgramacionCampo.Fecha = Nothing
                ProgramacionCampo.Hora = Nothing
                ProgramacionCampo.Moderador = Nothing
                ProgramacionCampo.Estado = estado

                id = oProgramados.GuardarProgramarCampo(ProgramacionCampo)
                GuardarLogProgramacionCampo(ProgramacionCampo.id, estado, txtProgramarObservacion.Text, Session("IDUsuario"), Date.UtcNow.AddHours(-5))
            End If
            LimpiarProgramarCita()
            ScriptManager.RegisterStartupScript(Page, Me.GetType(), "cerrarProgramar", "<script>cerrarProgramar()</script>", False)
            ShowNotification("Se Canceló el campo correctamente", ShowNotifications.InfoNotification)
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            CargarProgramadosEntrevistados(hfTrabajoId.Value)
        End If
    End Sub

    Sub CargarDatosEntrevistados()
        Dim id = Convert.ToInt64(Session("Id").ToString)
        Dim EntrevistadoId = Convert.ToInt64(Session("EntrevistadoId").ToString)
        cargarFechaNacimiento()

        If id = 0 Or EntrevistadoId = 0 Then
            ShowNotification("No se encontraron datos de este entrevistado. Intente nuevamente o Recargue la página.", ShowNotifications.InfoNotification)
            Exit Sub
        Else
            Dim oProgramados As New CoreProject.CampoCualitativo
            Dim Ent = oProgramados.obtenerEntrevistadosCualixId(EntrevistadoId)
            Dim pro = oProgramados.ObtenerProgramadosEntrevistadosCualiXId(id)

            If Ent.Id = 0 Or pro.id = 0 Then
                ShowNotification("No se encontraron datos de este entrevistado. Intente nuevamente o Recargue la página.", ShowNotifications.InfoNotification)
                Exit Sub
            Else
                Dim o As New CoreProject.RegistroPersonas

                txtNombre.Text = Ent.Nombre
                txtCC.Text = If(Ent.Documento Is Nothing, "", Ent.Documento)
                txtTelefono.Text = Ent.Telefono
                txtCelular.Text = Ent.Celular
                txtFechaNacimiento.Text = Convert.ToDateTime(Ent.FechaNacimiento).ToString("dd/MM/yyyy")
                txtEdad.Text = Ent.Edad
                Dim EstadoCivil = o.EstadoCivilXid(Ent.EstadoCivil)
                If EstadoCivil Is Nothing Then
                    ddlEstadoCivil.SelectedValue = -1
                Else
                    ddlEstadoCivil.SelectedValue = EstadoCivil.Id
                End If
                Dim Genero = o.SexoXId(Ent.Genero)
                If Genero Is Nothing Then
                    ddlSexo.SelectedValue = -1
                Else
                    ddlSexo.SelectedValue = Genero.Id
                End If
                txtCiudadResidencia.Text = Ent.Ciudad
                txtDireccion.Text = Ent.Direccion
                txtBarrio.Text = Ent.Barrio
                ddlEstrato.SelectedValue = If(Ent.Estrato Is Nothing, "", Ent.Estrato)
                txtCorreo.Text = Ent.Correo
                Dim NivelEducativo = o.NivelEducativoXId(Ent.NivelEscolaridad)
                If NivelEducativo Is Nothing Then
                    ddlNivelEducativo.SelectedValue = -1
                Else
                    ddlNivelEducativo.SelectedValue = NivelEducativo.id
                End If
                txtPerfil.Text = Ent.Perfil
                CargarReclutadores()
                ddlReclutador.SelectedValue = pro.Reclutador
            End If
        End If
    End Sub

    Sub cargarFechaNacimiento()
        ScriptManager.RegisterStartupScript(Page, Me.GetType(), "cargarFechaNacimiento", "<script>cargarFechaNacimiento()</script>", False)
    End Sub

    Sub cargarProgramarFecha()
        ScriptManager.RegisterStartupScript(Page, Me.GetType(), "cargarProgramarFecha", "<script>cargarProgramarFecha()</script>", False)
    End Sub

    Function SaveFile(ByVal file As HttpPostedFile, fileName As String, savePath As String) As String
        If Not System.IO.Directory.Exists(savePath) Then System.IO.Directory.CreateDirectory(savePath)

        Dim pathToCheck As String = savePath & fileName
        Dim tempfileName As String = ""

        If System.IO.File.Exists(pathToCheck) Then
            Dim counter As Integer = 2

            While System.IO.File.Exists(pathToCheck)
                tempfileName = fileName & "_" & counter.ToString()
                pathToCheck = savePath & tempfileName
                counter += 1
            End While

            fileName = tempfileName
        End If

        savePath += fileName
        FileUpData.SaveAs(savePath)
        Return savePath
    End Function

    Public Function getDataExcel(Path As String) As Int64
        'Se elimina esta codigo debido a que se usa Excel.Interop y se deberia cambiar a ClosedXML
    End Function

    Function guardarReclutador(ByVal Reclutador As String) As Integer
        Dim idReclutador = 0
        If Trim(Reclutador <> "") Then
            Dim oCampo As New CoreProject.CampoCualitativo
            Dim oReclutador As New CoreProject.OP_Reclutadores_Cuali
            oReclutador = oCampo.obtenerReclutadoresCuali().Where(Function(x) x.Nombre = Reclutador).FirstOrDefault
            If oReclutador Is Nothing Then
                oReclutador = New CoreProject.OP_Reclutadores_Cuali
                oReclutador.Nombre = Reclutador
                idReclutador = oCampo.GuardarReclutadorCuali(oReclutador)
            Else
                idReclutador = oReclutador.id
            End If
        End If
        Return idReclutador
    End Function

    Sub actualizarCampo()

        ShowNotification("Se modificó el campo correctamente", ShowNotifications.InfoNotification)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        CargarProgramadosEntrevistados(hfTrabajoId.Value)
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
        Script.RegisterPostBackControl(btnDescargar)
        Response.End()
    End Sub
    Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String, ByVal fila As Integer)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(fila, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub

#End Region


End Class