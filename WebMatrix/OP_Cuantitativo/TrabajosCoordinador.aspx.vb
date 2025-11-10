Imports CoreProject
Imports WebMatrix.Util
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports ClosedXML.Excel

Public Class TrabajosCoordinador
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
    Sub CargarTrabajos()
        Dim oTrabajo As New GestionTrabajosOP
        Dim oCoord As New CoordinacionCampo

        Dim Id As Int64? = Nothing
        Dim Nombre As String = Nothing
        Dim JobBook As String = Nothing
        If IsNumeric(txtID.Text) Then Id = txtID.Text
        If Not (txtNombre.Text = "") Then Nombre = txtNombre.Text.Trim
        If Not (txtJobBook.Text = "") Then JobBook = txtJobBook.Text.Trim
        gvTrabajos.DataSource = oTrabajo.ListaTrabajosXCoordinador(Id, Nombre, JobBook, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, ddlEstado.SelectedValue, Session("IDUsuario").ToString).OrderByDescending(Function(x) x.id).ToList
		gvTrabajos.DataBind()
		accordion1.Visible = True
		accordion2.Visible = False
		accordion3.Visible = False
		accordion4.Visible = False
		accordion5.Visible = False
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
        gvCiudades.DataSource = listTrabCoord.ToList
        gvCiudades.DataBind()
    End Sub
    Sub CargarPersonalAsignado(ByVal TrabajoId As Int64, ByVal CiudadId As Int32)
        Dim oCoord As New CoordinacionCampoPersonal
        'Dim listPersonalASignado = (From lpersonal In oCoord.ObtenerPersonalAsignadoXCiudad(TrabajoId, CiudadId)
        '                          Select id = lpersonal.id, Nombre = lpersonal.TH_Personas.Nombres & " " & lpersonal.TH_Personas.Apellidos, Cargo = lpersonal.TH_Personas.TH_Cargos.Cargo, Ciudad = lpersonal.TH_Personas.C_Divipola.DivMuniNombre)
        'gvPersonalAsignado.DataSource = listPersonalASignado.ToList
        gvPersonalAsignado.DataSource = oCoord.ObtenerPersonalAsignado(TrabajoId, Nothing, CiudadId)
        gvPersonalAsignado.DataBind()
    End Sub
    Sub CargarEncuestadoresDisponibles(ByVal TrabajoId As Int64, ByVal CiudadId As Int64)
        Dim oCoord As New CoordinacionCampoPersonal
        'Dim listEncu = (From lpersonas In oCoord.ObtenerPersonalCapacitadoNoAsignadoList(TrabajoId, CiudadId, 13)
        '               From lEncuestadores In oCoord.ObtenerListadoEncuestadores
        '               Where lpersonas.id = lEncuestadores.id
        '               Select id = lpersonas.id, Nombre = lpersonas.Nombres & " " & lpersonas.Apellidos, Cargo = lpersonas.TH_Cargos.Cargo,
        '               Tipo = lpersonas.OP_Encuestadores.OP_TipoEncuestador.Tipo, Contratacion = lpersonas.TH_TipoContratacion.Tipo, Ciudad = lpersonas.C_Divipola.DivMuniNombre)
        'Me.gvEncuestadoresPorAsignar.DataSource = listEncu.ToList
        Me.gvEncuestadoresPorAsignar.DataSource = oCoord.ObtenerPersonalSinAsignar(TrabajoId, Nothing, CiudadId)
        Me.gvEncuestadoresPorAsignar.DataBind()
    End Sub
    Sub CargarEnvios(ByVal TrabajoID As Int64, ByVal CiudadId As Int32)
        Dim oCoordinacion As New CoordinacionCampo
        Dim lisEnvios = (From lEnvios In oCoordinacion.ObtenerEnviosList(TrabajoID, CiudadId)
                       Select guia = lEnvios.Guia, courier = lEnvios.Courier, usuario = lEnvios.US_Usuarios.Nombres & " " & lEnvios.US_Usuarios.Apellidos,
                       fecha = lEnvios.FechaEnvio, cantidad = lEnvios.Cantidad, id = lEnvios.id).ToList
        gvEnvios.DataSource = lisEnvios
        gvEnvios.DataBind()
    End Sub

    Sub CargarEjecucionxEncuestador(ByVal IdTrabajo As Int64?, ByVal Ciudad As Int64?)
        Dim oEjec As New Reportes.AvanceCampo
        gvEjecucion.DataSource = oEjec.ObtenerEjecucionxEncuestador(IdTrabajo, Ciudad)
        gvEjecucion.DataBind()
    End Sub

    Sub CargarEjecucionxSupervisor(ByVal IdTrabajo As Int64?, ByVal Ciudad As Int64?)
        Dim oEjec As New Reportes.AvanceCampo
        gvEjecucion.DataSource = oEjec.ObtenerEjecucionxSupervisor(IdTrabajo, Ciudad)
        gvEjecucion.DataBind()
    End Sub
#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(101, UsuarioID) = False Then
            Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
        End If
        If Not IsPostBack Then
            CargarTrabajos()
        End If

        ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnImgExportar)
        ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnImgExportar1)
    End Sub
	Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
		Dim oTrabajo As New Trabajo
		CargarTrabajos()
	End Sub
	Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
		gvTrabajos.PageIndex = e.NewPageIndex
		CargarTrabajos()
	End Sub

	Private Sub gvEjecucion_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvEjecucion.PageIndexChanging
        gvEjecucion.PageIndex = e.NewPageIndex
		If hfTipoEjec.Value = 1 Then
			CargarEjecucionxEncuestador(hfIdTrabajo.Value, hfidCiudadId.Value)
		ElseIf hfTipoEjec.Value = 2 Then
			CargarEjecucionxSupervisor(hfIdTrabajo.Value, hfidCiudadId.Value)
		End If
		accordion1.Visible = False
		accordion2.Visible = False
		accordion3.Visible = False
		accordion4.Visible = False
		accordion5.Visible = True
	End Sub

    Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        If e.CommandName = "Actualizar" Then
            'cargarTrabajo(Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")))
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Dim oTrabajo As New Trabajo
            Dim oeTrabajo = oTrabajo.ObtenerTrabajo(hfIdTrabajo.Value)
            Session("NombreTrabajo") = oeTrabajo.id.ToString & " | " & oeTrabajo.JobBook & " | " & oeTrabajo.NombreTrabajo
            CargarCiudadesAsignadas()
			accordion1.Visible = False
			accordion2.Visible = True
			accordion3.Visible = False
			accordion4.Visible = False
			accordion5.Visible = False
		ElseIf e.CommandName = "Tareas" Then
            'Response.Redirect("\CORE\Tareas.aspx" & "?IdTrabajo=" & gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id") & "&Coe=" & Session("IDUsuario").ToString)
        ElseIf e.CommandName = "Avance" Then
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Response.Redirect("../RP_Reportes/AvanceDeCampo.aspx?TrabajoId=" & hfIdTrabajo.Value)
        End If
    End Sub
    Private Sub gvCiudades_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCiudades.RowCommand

		If e.CommandName = "Enviar" Then
			hfidCiudadId.Value = Int64.Parse(Me.gvCiudades.DataKeys(CInt(e.CommandArgument))("CiudadId"))
			CargarEnvios(hfIdTrabajo.Value, hfidCiudadId.Value)
			accordion3.Visible = True
		ElseIf e.CommandName = "Asignar" Then
			hfidCiudadId.Value = Int64.Parse(Me.gvCiudades.DataKeys(CInt(e.CommandArgument))("CiudadId"))
            hfidCiudad.Value = Me.gvCiudades.DataKeys(CInt(e.CommandArgument))("Ciudad")
            hfidIdMuestra.Value = Me.gvCiudades.DataKeys(CInt(e.CommandArgument))("Id")
            CargarPersonalAsignado(hfIdTrabajo.Value, hfidCiudadId.Value)
            CargarEncuestadoresDisponibles(hfIdTrabajo.Value, hfidCiudadId.Value)
			accordion4.Visible = True
		ElseIf e.CommandName = "EjecucionEnc" Then
            hfTipoEjec.Value = 1
            hfidCiudadId.Value = Int64.Parse(Me.gvCiudades.DataKeys(CInt(e.CommandArgument))("CiudadId"))
            CargarEjecucionxEncuestador(hfIdTrabajo.Value, hfidCiudadId.Value)
            gvEjecucion.Columns(5).Visible = True
			accordion5.Visible = True

		ElseIf e.CommandName = "EjecucionSup" Then
            hfTipoEjec.Value = 2
            hfidCiudadId.Value = Int64.Parse(Me.gvCiudades.DataKeys(CInt(e.CommandArgument))("CiudadId"))
            CargarEjecucionxSupervisor(hfIdTrabajo.Value, hfidCiudadId.Value)
            gvEjecucion.Columns(5).Visible = False
			accordion5.Visible = True
		End If
    End Sub
    Private Sub gvEnvios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEnvios.RowCommand
        If e.CommandName = "Eliminar" Then
            Dim idval As Int64 = Int64.Parse(Me.gvEnvios.DataKeys(CInt(e.CommandArgument))("Id"))
            Dim oCoordinacion As New CoordinacionCampo
            oCoordinacion.EliminarEnvioPaquete(idval)
            CargarEnvios(hfIdTrabajo.Value, hfidCiudadId.Value)
            ShowNotification("Registro eliminado", ShowNotifications.InfoNotification)
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
            Dim oTipoFicha As New MetodologiaOperaciones
            Dim tipo As Int32 = oTipoFicha.ObtenerFichaMetodologiaxId(oeTrabajo.OP_MetodologiaId).Ficha
            Select Case tipo
				Case TipoFicha.Cuanti
					Response.Redirect("../RP_Reportes/InformacionGeneral.aspx?idTr=" & hfIdTrabajo.Value & "&URLBACK=.." & Request.Url.PathAndQuery.ToString.Replace("&", "|"))
					'Response.Redirect("../OP_Cuantitativo/FichaCuantitativa.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes&coordinador=1")
				Case TipoFicha.Sesiones
                    Response.Redirect("../OP_Cualitativo/FichaSesion.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes&coordinador=1")
                Case TipoFicha.Observaciones
                    Response.Redirect("../OP_Cualitativo/FichaObservacion.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes&coordinador=1")
                Case TipoFicha.Entrevistas
                    Response.Redirect("../OP_Cualitativo/FichaEntrevistas.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes&coordinador=1")
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try


    End Sub
    Protected Sub btnCapacitaciones_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCapacitaciones.Click
        Response.Redirect("../TH_TalentoHumano/Capacitacion.aspx?idtrabajo=" & hfIdTrabajo.Value & "&coordinador=1")
    End Sub
    Protected Sub btnEstimaciones_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEstimaciones.Click
        Response.Redirect("EstimacionProduccion.aspx?TrabajoId=" & hfIdTrabajo.Value & "&coordinador=1")
    End Sub
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
		accordion1.Visible = True
		accordion2.Visible = False
		accordion3.Visible = False
		accordion4.Visible = False
		accordion5.Visible = False
	End Sub
    Protected Sub btnEnvio_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEnvio.Click
        Dim Entidad As New OP_EnvioPaquetesEncuestas
        Entidad.Guia = txtGuia.Text
        Entidad.Courier = txtCourier.Text
        Entidad.FechaEnvio = txtFechaEnvio.Text
        Entidad.Cantidad = txtCantidad.Text
        Entidad.FechaRegistro = Date.UtcNow.AddHours(-5)
        Entidad.Usuario = Session("IDUsuario").ToString
        If hfIdTrabajo.Value = "" Then
            ShowNotification("Debe primero seleccionar el trabajo del que desea enviar paquetes", ShowNotifications.ErrorNotification)
            Exit Sub
        End If
        Entidad.TrabajoId = hfIdTrabajo.Value
        If hfidCiudadId.Value = "" Then
            ShowNotification("Debe primero seleccionar la ciudad de la que desea enviar paquetes", ShowNotifications.ErrorNotification)
            Exit Sub
        End If
        Entidad.CiudadId = hfidCiudadId.Value

        Dim oCoordinacion As New CoordinacionCampo
        oCoordinacion.GuardarEnvioPaquetes(Entidad)
        txtGuia.Text = String.Empty
        txtCourier.Text = String.Empty
        txtFechaEnvio.Text = String.Empty
        txtCantidad.Text = String.Empty
        ShowNotification("Envio realizado satisfactoriamente", ShowNotifications.InfoNotification)
        CargarEnvios(hfIdTrabajo.Value, hfidCiudadId.Value)
        accordion3.Visible = true 
    End Sub
    Private Sub gvPersonalAsignado_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPersonalAsignado.RowCommand
        If e.CommandName = "Eliminar" Then
            Dim oCoordinacion As New CoordinacionCampoPersonal
            oCoordinacion.EliminarPersonalAsignado(Me.gvPersonalAsignado.DataKeys(CInt(e.CommandArgument))("Id"), hfIdTrabajo.Value)
            CargarPersonalAsignado(hfIdTrabajo.Value, hfidCiudadId.Value)
            CargarEncuestadoresDisponibles(hfIdTrabajo.Value, hfidCiudadId.Value)
        End If
		accordion4.Visible = False
	End Sub
    Private Sub gvEncuestadoresPorAsignar_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEncuestadoresPorAsignar.RowCommand
        If e.CommandName = "Asignar" Then
            Dim oCoord As New CoordinacionCampoPersonal
            Dim Ent As New OP_PersonasAsignadasTrabajo
            Ent.Persona = Int64.Parse(Me.gvEncuestadoresPorAsignar.DataKeys(CInt(e.CommandArgument))("Id"))
            Ent.TrabajoId = hfIdTrabajo.Value
            Ent.Ciudad = hfidCiudadId.Value
            oCoord.GuardarPersonalAsignado(Ent)
            CargarPersonalAsignado(hfIdTrabajo.Value, hfidCiudadId.Value)
            CargarEncuestadoresDisponibles(hfIdTrabajo.Value, hfidCiudadId.Value)
			accordion4.Visible = False
		End If
    End Sub
    Protected Sub btnCancelAsignacion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelAsignacion.Click
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub btnListadoDocumentos_Click(sender As Object, e As EventArgs) Handles btnListadoDocumentos.Click
        Response.Redirect("../CORE/ListaDocumentosXHilos.aspx?IdTrabajo=" & hfIdTrabajo.Value)
    End Sub

    Protected Sub btnEstadoTareas_Click(sender As Object, e As EventArgs) Handles btnEstadoTareas.Click
        Response.Redirect("../CORE/ListaTareasXHilo.aspx" & "?IdTrabajo=" & hfIdTrabajo.Value)
    End Sub
#End Region

    Function ObtenerEjecucionxEncuestador() As List(Of REP_EjecucionXEncuestador_Result)
        Dim oEjec As New Reportes.AvanceCampo
        Return oEjec.ObtenerEjecucionxEncuestador(hfIdTrabajo.Value, hfidCiudadId.Value)
    End Function

    Function ObtenerEjecucionxSupervisor() As List(Of REP_EjecucionXSupervisor_Result)
        Dim oEjec As New Reportes.AvanceCampo
        Return oEjec.ObtenerEjecucionxSupervisor(hfIdTrabajo.Value, hfidCiudadId.Value)
    End Function


    Function ObtenerPersonalAsignado() As List(Of OP_PersonasAsignacion_Result)
        Dim oCoord As New CoordinacionCampoPersonal
        Return oCoord.ObtenerPersonalAsignado(hfIdTrabajo.Value, Nothing, hfidCiudadId.Value)
    End Function

    Sub exportarExcel()
        Dim wb As New XLWorkbook
        Dim oLstEjecucionxENC As List(Of REP_EjecucionXEncuestador_Result)
        Dim oLstEjecucionxSUP As List(Of REP_EjecucionXSupervisor_Result)
        Dim titulosProduccion As String = "Ciudad;Cedula;Nombre;Cargo;Contratacion;NombreSupervisor;NombreContratista;TrabajoId;NombreTrabajo;Realizadas;Supervisadas;Porcentaje_Supervision;VerificadasAnuladas;VerificadasObservadas;VerificadasOK;VerificadasOKAudio;VerificadasOKTelefonica;Verificadas;Porcentaje_Verificacion;EncuestasNoContactadas;NoColabora_NodaInformacion;NoContacto;Porcentaje_NoContactado;Anuladas;Porcentaje_Anuladas"

        Dim ws = wb.Worksheets.Add("ReporteEjecucionCampo")
        insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

        If hfTipoEjec.Value = 1 Then
            oLstEjecucionxENC = ObtenerEjecucionxEncuestador()
            ws.Cell(2, 1).InsertData(oLstEjecucionxENC)
        ElseIf hfTipoEjec.Value = 2 Then
            oLstEjecucionxSUP = ObtenerEjecucionxSupervisor()
            ws.Cell(2, 1).InsertData(oLstEjecucionxSUP)
        End If

        exportarExcel(wb, "ReporteEjecucionCampo")
    End Sub
    Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String, ByVal fila As Integer)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(fila, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub

    Private Sub exportarExcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""Control_" & name & ".xlsx""")

        Using memoryStream = New System.IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

    Protected Sub btnImgExportar_Click(sender As Object, e As ImageClickEventArgs) Handles btnImgExportar.Click
        exportarExcel()
    End Sub

    Sub exportarExcel1()
        Dim wb As New XLWorkbook
        Dim oLstPersonal As List(Of OP_PersonasAsignacion_Result)
        Dim titulosProduccion As String = "Cedula;Nombre;Ciudad;Cargo;TipoContratacion;TipoEncuestador"

        Dim ws = wb.Worksheets.Add("PersonalAsignado")
        insertarNombreColumnasExcel1(ws, titulosProduccion.Split(";"), 1)

        oLstPersonal = ObtenerPersonalAsignado()
        ws.Cell(2, 1).InsertData(oLstPersonal)

        exportarExcel1(wb, "PersonalAsignado")
    End Sub
    Sub insertarNombreColumnasExcel1(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String, ByVal fila As Integer)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(fila, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub
    Private Sub exportarExcel1(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""Reporte_" & name & ".xlsx""")

        Using memoryStream = New System.IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

    Protected Sub btnImgExportar1_Click(sender As Object, e As ImageClickEventArgs) Handles btnImgExportar1.Click
        exportarExcel1()
    End Sub
End Class