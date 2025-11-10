Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Imports System.IO
Public Class CampoCualitativo
    Inherits System.Web.UI.Page

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim scriptmanager As ScriptManager = scriptmanager.GetCurrent(Me.Page)
        scriptmanager.RegisterPostBackControl(imbDescargarCita)
        scriptmanager.RegisterPostBackControl(Me.btnExportar)
        If Not IsPostBack Then
            If Request.QueryString("idsegmento") IsNot Nothing Then
                hfIdSegmento.Value = Int64.Parse(Request.QueryString("idsegmento").ToString)
            End If
            If Request.QueryString("trabajoid") IsNot Nothing Then
                hfIdTrabajo.Value = Int64.Parse(Request.QueryString("trabajoid").ToString)
            End If
            If Request.QueryString("py") IsNot Nothing Then
                lnkProyecto.PostBackUrl = "../PY_Proyectos/SegmentosCuali.aspx?trabajoId=" & hfIdTrabajo.Value & "&py=true"
            Else
                lnkProyecto.PostBackUrl = "../PY_Proyectos/SegmentosCuali.aspx?trabajoId=" & hfIdTrabajo.Value
            End If
            CargarModeradores(hfIdSegmento.Value)
            CargarTranscriptores()
            CargarInfoSegmento(hfIdSegmento.Value)
            CargarCampo()
        End If

    End Sub

    Protected Sub ddlModerador_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlModerador.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub

    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex

    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            GuardarPlaneacion()
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
            limpiar()
            CargarCampo()
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        limpiar()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)

    End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "AbrirPlaneacion"
                    hfIdCampo.Value = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    CargarInfoCampo()
                    ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
                Case "AbrirEjecucion"
                    hfIdCampo.Value = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    CargarInfoCampo()
                    ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
                Case "Eliminar"
                    hfIdCampo.Value = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Eliminar()
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub CargarLabelTrabajo(ByVal idTrabajo As Int64)
        Dim oTrabajo As New Trabajo
        lblTrabajo.Text = oTrabajo.obtenerXId(idTrabajo).JobBook & " " & oTrabajo.obtenerXId(idTrabajo).NombreTrabajo
    End Sub
    Sub CargarInfoSegmento(ByVal idsegmento As Int64)
        Dim oSegmento As New CoreProject.SegmentosCuali
        Dim info = oSegmento.ObtenerSegmentoXId(idsegmento)
        Dim oDivipola As New Auxiliares
        txtCiudad.Text = oDivipola.DevolverNombreCiudad(info.Municipio)
        txtTipoLugar.Text = oSegmento.ObtenerNombreLugarCuali(info.TipoLugar)
        CargarLabelTrabajo(info.TrabajoId)
    End Sub
    Public Sub limpiar()
        hfIdCampo.Value = String.Empty
        ddlModerador.ClearSelection()
        ddlTranscriptor.ClearSelection()
        txtLugar.Text = String.Empty
        txtPersonaContacto.Text = String.Empty
        txtDatosContacto.Text = String.Empty
        txtDireccion.Text = String.Empty
        txtObservacionesPrevias.Text = String.Empty
        txtFecha.Text = String.Empty
        txtFechaReal.Text = String.Empty
        txtHora.Text = String.Empty
        txtHoraReal.Text = String.Empty
        txtAsistentes.Text = String.Empty
        txtAsistentesReales.Text = String.Empty
        txtObservacionesEjecucion.Text = String.Empty
        rbtCaida.Checked = False
        rbtCancelada.Checked = False
        rbtEjecutada.Checked = False
    End Sub

    'Public Sub CargarModeradores()
    '    Try
    '        Dim oCampo As New CoreProject.CampoCualitativo
    '        ddlModerador.DataSource = oCampo.ObtenerModeradores
    '        ddlModerador.DataValueField = "Id"
    '        ddlModerador.DataTextField = "Nombre"
    '        ddlModerador.DataBind()
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub


    Sub CargarModeradores(ByVal SegmentoId As Int64)
        Dim o As New CoreProject.CampoCualitativo
        ddlModerador.DataSource = o.ObtenerModeradoresList(SegmentoId).ToList
        ddlModerador.DataValueField = "Identificacion"
        ddlModerador.DataTextField = "Nombre"
        ddlModerador.DataBind()
    End Sub

    Public Sub CargarTranscriptores()

        Dim oCampo As New CoreProject.CampoCualitativo
        ddlTranscriptor.DataSource = oCampo.ObtenerTranscriptores
        ddlTranscriptor.DataValueField = "Id"
        ddlTranscriptor.DataTextField = "Nombre"
        ddlTranscriptor.DataBind()
        Me.ddlTranscriptor.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
        
    End Sub

    Sub GuardarPlaneacion()
        Dim e As New OP_CampoCuali
        Dim o As New CoreProject.CampoCualitativo
        If Not (hfIdCampo.Value = "") Then e = o.ObtenerCampoCualiXId(hfIdCampo.Value)
        e.SegmentoId = hfIdSegmento.Value
        e.Moderador = ddlModerador.SelectedValue
        e.PlaneacionPor = Session("IDUsuario").ToString
        e.Lugar = txtLugar.Text
        e.PersonaContaco = txtPersonaContacto.Text
        e.DatosContacto = txtDatosContacto.Text
        e.Direccion = txtDireccion.Text
        e.Fecha = txtFecha.Text
        e.Hora = CDate(txtHora.Text).TimeOfDay
        e.ObservacionesPrevias = txtObservacionesPrevias.Text
        e.Transcriptor = ddlTranscriptor.SelectedValue
        hfIdCampo.Value = o.GuardarCampo(e)
    End Sub
    Sub GuardarEjecucion()
        Dim e As New OP_CampoCuali
        Dim o As New CoreProject.CampoCualitativo
        e = o.ObtenerCampoCualiXId(hfIdCampo.Value)
        e.FechaReal = txtFechaReal.Text
        e.HoraReal = CDate(txtHoraReal.Text).TimeOfDay
        e.Asistentes = txtAsistentes.Text
        e.AsistentesNoCumplen = txtAsistentesReales.Text
        e.EjecucionPor = Session("IDUsuario").ToString
        e.Caida = rbtCaida.Checked
        e.Cancelada = rbtCancelada.Checked
        e.Ejecutada = rbtEjecutada.Checked
        e.ObservacionesEjecucion = txtObservacionesEjecucion.Text
        o.GuardarCampo(e)
    End Sub
    Sub Eliminar()
        Dim o As New CoreProject.CampoCualitativo
        Dim e As New OP_CampoCuali
        e = o.ObtenerCampoCualiXId(hfIdCampo.Value)
        If o.EliminarCampo(e) = -1 Then
            ShowNotification("El registro no se pudo eliminar", ShowNotifications.InfoNotification)
        Else
            ShowNotification("Registro Eliminado correctamente", ShowNotifications.InfoNotification)
        End If
        CargarCampo()
    End Sub
    Sub CargarInfoCampo()
        Dim o As New CoreProject.CampoCualitativo
        Dim info = o.ObtenerCampoCualiXId(hfIdCampo.Value)
        ddlModerador.SelectedValue = info.Moderador
        If Not info.Transcriptor Is Nothing Then ddlTranscriptor.SelectedValue = info.Transcriptor
        txtLugar.Text = info.Lugar
        txtPersonaContacto.Text = info.PersonaContaco
        txtDatosContacto.Text = info.DatosContacto
        txtDireccion.Text = info.Direccion
        txtFecha.Text = info.Fecha
        txtHora.Text = info.Hora.ToString
        txtObservacionesPrevias.Text = info.ObservacionesPrevias
        Try
            txtFechaReal.Text = info.FechaReal
            txtHoraReal.Text = info.HoraReal.ToString
            txtAsistentes.Text = info.Asistentes
            txtAsistentesReales.Text = info.AsistentesNoCumplen
            rbtCaida.Checked = info.Caida
            rbtCancelada.Checked = info.Cancelada
            rbtEjecutada.Checked = info.Ejecutada
            txtObservacionesEjecucion.Text = info.ObservacionesEjecucion
        Catch ex As Exception
        End Try

    End Sub
    Sub CargarCampo()
        Dim o As New CoreProject.CampoCualitativo
        gvDatos.DataSource = o.ObtenerCampoCualiList(hfIdSegmento.Value)
        gvDatos.DataBind()
    End Sub
    Sub CargarListadoCampo()
        Dim o As New CoreProject.CampoCualitativo
        Me.gvExportado.DataSource = o.ObtenerListadoCampoCuali(hfIdSegmento.Value)
        Me.gvDatos.DataBind()
    End Sub
    Public Function Crear_Archivo_ICS(ByVal fch_inicio As Date, ByVal fch_fin As Date, ByVal strLugar As String, ByVal strDescripcion As String, ByVal strAsunto As String) As Boolean
        Try
            Dim sb As New StringBuilder(215)
            'ENCABEZADO
            sb.AppendFormat("BEGIN:VCALENDAR{0}", Environment.NewLine)
            sb.AppendFormat("CALSCALE:GREGORIAN{0}", Environment.NewLine)
            sb.AppendFormat("VERSION:1.0{0}", Environment.NewLine)
            sb.AppendFormat("BEGIN:VEVENT{0}", Environment.NewLine)
            'BODY
            sb.AppendFormat("DTSTART:" & fch_inicio.ToUniversalTime.ToString("yyyyMMdd\THHmmss\Z") & "{0}", Environment.NewLine)    'Inicio
            sb.AppendFormat("DTEND:" & fch_fin.ToUniversalTime.ToString("yyyyMMdd\THHmmss\Z") & "{0}", Environment.NewLine)        'Fin
            sb.AppendFormat("LOCATION:" & strLugar & "{0}", Environment.NewLine)                                                    'Lugar
            sb.AppendFormat("DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" & strDescripcion & "{0}", Environment.NewLine)                 'Descripción
            sb.AppendFormat("SUMMARY:" & strAsunto & "{0}", Environment.NewLine)                                                    'Asunto
            sb.AppendFormat("CLASS:PUBLIC{0}", Environment.NewLine)
            'FIN BODY
            sb.AppendFormat("END:VEVENT{0}", Environment.NewLine)
            sb.AppendFormat("END:VCALENDAR{0}", Environment.NewLine)
            Dim enc As New UTF8Encoding
            Dim arrBytData() As Byte = enc.GetBytes(sb.ToString)
            'HACERLO DESCARGABLE
            Response.Clear()    'Clean the current output content from the buffer
            Response.ContentType = "text/plain"
            Response.AppendHeader("Content-Disposition", "attachment; filename=vCalendar.ics")
            Response.AppendHeader("Content-Length", arrBytData.Length.ToString())
            Response.ContentType = "application/octet-stream"
            Response.BinaryWrite(arrBytData)
            Response.Flush()
            Response.End()
            Crear_Archivo_ICS = True

        Catch ex As Exception
            Crear_Archivo_ICS = False
            'lblError.Text = ex.Message
        End Try
        Return Crear_Archivo_ICS
    End Function

#End Region


    Protected Sub btnCancelEjecucion_Click(sender As Object, e As EventArgs) Handles btnCancelEjecucion.Click
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        limpiar()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub btnGuardarEjecucion_Click(sender As Object, e As EventArgs) Handles btnGuardarEjecucion.Click
        GuardarEjecucion()
        ShowNotification("Ejecución guardada correctamente", ShowNotifications.InfoNotification)
        limpiar()
        CargarCampo()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub imbDescargarCita_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imbDescargarCita.Click
        Dim fecha As New Date
        Dim fecha2 As New Date
        fecha = CDate(txtFecha.Text & " " & txtHora.Text)
        fecha2 = fecha.AddHours(1)
        Crear_Archivo_ICS(fecha, fecha2, txtLugar.Text, "Dirección: " & txtDireccion.Text & "| Datos contacto: " & txtDatosContacto.Text & "| Observaciones: " & txtObservacionesPrevias.Text, "Campo Planeado " & lblTrabajo.Text & " - " & txtDatosContacto.Text)
    End Sub

    Private Sub btnDuplicar_Click(sender As Object, e As System.EventArgs) Handles btnDuplicar.Click
        Dim o As New CoreProject.CampoCualitativo
        If hfIdCampo.Value = "" Then Exit Sub
        hfIdCampo.Value = o.Duplicar(hfIdCampo.Value)
        ShowNotification("Campo duplicado", ShowNotifications.InfoNotification)
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As System.EventArgs) Handles btnExportar.Click
        CargarListadoCampo()
        Me.gvExportado.Visible = True
        'Actualiza los datos del gridview
        Me.gvExportado.DataBind()
        'Crea variables en memoria para almacenar el contenido del gridview
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Crea una nueva página html y un form dentro de ella
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        'Cambie el estado del gridview para que no guarde los controles de vista
        Me.gvExportado.EnableViewState = False
        'Quita la validación de la página 
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        'Agrega el form creado
        pagina.Controls.Add(form)
        'Agrega el gridview al form
        form.Controls.Add(gvExportado)
        'Hace un render para pasar los valores de la página al htmltextwriter que contendrá los datos del gridview
        pagina.RenderControl(htw)
        'Hace un response para descargar el control
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=ExportadoCampoCualitativoXSegmento_" & hfIdSegmento.Value & ".xls")
        Response.Charset = "UTF-8"

        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        gvExportado.Visible = False

    End Sub

    Private Sub btnDocumentos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDocumentos.Click
        If hfIdSegmento.Value = "" Then Exit Sub
        Dim o As New WorkFlow
        Dim oS As New CoreProject.SegmentosCuali
        Dim idTrabajo As Int64 = oS.ObtenerSegmentoXId(hfIdSegmento.Value).TrabajoId
        Dim idWF As Int64 = o.ObtenerWorkflowXTrabajoXTarea(idTrabajo, 3).FirstOrDefault.id
        If hfIdCampo.Value <> "" Then
            Response.Redirect("\GD_Documentos\GD_Documentos.aspx?IdContenedor=" & idTrabajo & "&IdDocumento=77&TipoContenedor=1&IdWorkFlow=" & idWF)
        End If
    End Sub
End Class