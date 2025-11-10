Imports CoreProject
Imports WebMatrix.Util
Imports System.Net


Public Class TrabajosCOE
    Inherits System.Web.UI.Page
#Region "Enumeradores"
    Enum eEstadoBloqueo
        Cerrado = 10
        Anulado = 11
    End Enum
#End Region

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
        Dim id As Int64? = Nothing
        Dim nombre As String = Nothing
        Dim jobbook As String = Nothing
        Dim estado As Int32? = Nothing
        If IsNumeric(txtID.Text) Then id = txtID.Text
        If Not (txtNombreBuscar.Text = "") Then nombre = txtNombreBuscar.Text
        If Not (txtJobBook.Text = "") Then jobbook = txtJobBook.Text
        If Not (ddlEstado.SelectedValue = "-1") Then estado = ddlEstado.SelectedValue
        gvTrabajos.DataSource = oTrabajo.ListadoTrabajos(id, estado, nombre, jobbook, Nothing, Coe, Nothing, Nothing, Nothing, Nothing, Nothing)
        gvTrabajos.DataBind()
    End Sub
    Sub CargarTiposDeRecolección()
        Dim oTrabajoOP As New TrabajoOPCuanti
        Me.ddlTipoRecoleccion.DataSource = oTrabajoOP.ObtenerTecnicasRecoleccion
        Me.ddlTipoRecoleccion.DataValueField = "id"
        Me.ddlTipoRecoleccion.DataTextField = "Recoleccion"
        Me.ddlTipoRecoleccion.DataBind()
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
            Dim infoTrabajo = oTrabajoOP.ObtenerTrabajo(TrabajoId)
            ddlTipoRecoleccion.SelectedValue = infoTrabajo.TipoRecoleccionId
        Catch ex As Exception
            eTrabajoOP = New OP_TrabajoConfiguracion
            ddlTipoRecoleccion.SelectedIndex = 0
        End Try
    End Sub

    Sub cargarTrabajo(ByVal idTrabajo As Int64)
        Dim oTrabajo As New Trabajo
        Dim oeTrabajo = oTrabajo.ObtenerTrabajo(idTrabajo)
        Dim oMetodologias As New MetodologiaOperaciones
        txtNombre.Text = oeTrabajo.NombreTrabajo
        Session("NombreTrabajo") = oeTrabajo.id.ToString & " | " & oeTrabajo.JobBook & " | " & oeTrabajo.NombreTrabajo
        txtMuestra.Text = oeTrabajo.Muestra
        txtMedicion.Text = oeTrabajo.NoMedicion
        txtMetodologia.Text = oMetodologias.obtenerXId(oeTrabajo.OP_MetodologiaId).MetNombre
        lblEstadoTrabajo.Text = oTrabajo.ObtenerEstado(oeTrabajo.Estado)
        Me.pnlInfoTrabajo.Visible = True

        If oeTrabajo.Estado <> 1 AndAlso oeTrabajo.Estado <> 15 Then
            Me.btnCerrarTrabajo.Visible = False
            Me.btnReporteCierre.Visible = True
        Else
            Me.btnCerrarTrabajo.Visible = True
            Me.btnReporteCierre.Visible = False
        End If

		accordion0.Visible = False
		accordion1.Visible = True
	End Sub


	Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
		If ddlTipoRecoleccion.SelectedIndex = -1 Then
			ShowNotification("Debe elegir el tipo de recolección antes de continuar", ShowNotifications.ErrorNotification)
			ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
			Exit Sub
		End If
		eTrabajoOP.UnidadCritica = 28
		eTrabajoOP.TrabajoId = hfIdTrabajo.Value
		Dim oTrabajoOp As New TrabajoOPCuanti
		eTrabajoOP = oTrabajoOp.GuardarTrabajoConfiguracion(eTrabajoOP)
		oTrabajoOp.GuardarTipoRecoleccion(hfIdTrabajo.Value, Me.ddlTipoRecoleccion.SelectedValue)
		ShowNotification("Información guardada correctamente", ShowNotifications.InfoNotification)
	End Sub

	Sub ConfigurarPryCuali(ByVal TrabajoId As Int64)
        Dim ot As New Trabajo
        Dim trb = ot.obtenerXId(TrabajoId)
        Dim o As New Proyecto
        Dim pry = o.obtenerXId(trb.ProyectoId)
        If pry.TipoProyectoId = 2 Then
            btnROCuestionario.Visible = False
            btnROInstructivo.Visible = False
            btnROMaterialAyuda.Visible = False
            btnROMetodologia.Visible = False
        Else
            btnROCuestionario.Visible = False
            btnROInstructivo.Visible = False
            btnROMaterialAyuda.Visible = False
            btnROMetodologia.Visible = False
        End If
    End Sub
    Public Sub CargarGrid()
        Dim idTrabajo As Integer = hfIdTrabajo.Value
        Dim oCargar As New GD.GD_Procedimientos
        gvReporte.DataSource = oCargar.DevolverxIdTrabajoIdRolResponsable(idTrabajo, 10)
        gvReporte.DataBind()
		accordion0.Visible = False
		accordion1.Visible = True
	End Sub
    Sub EscanerArchivos()

        Dim trabajo As PY_Trabajo_Get_Result
        Dim oGD As New GD.GD_Procedimientos
        Dim oTD As New OP.CentroInformacion
        Dim daTrabajo As New Trabajo
        trabajo = daTrabajo.DevolverxID(CInt(hfIdTrabajo.Value))

        Dim documentosEscaneados = oGD.DevolverxIdTrabajo(CInt(hfIdTrabajo.Value))
        Dim oDocumentos = oTD.obtenerdocumentoscierre(trabajo.id, 10)

        Dim DE As List(Of Long) = documentosEscaneados.Select(Function(x) CType(x.IdDocumento, Long)).ToList
        Dim DC As List(Of Long) = oDocumentos.Select(Function(x) x.IdDocumento).ToList
        Dim DD = DE.Except(DC).ToList

        For Each documento In DD
            oGD.eliminarDocumentoEscaneado(Nothing, documento, hfIdTrabajo.Value)
        Next

        For Each oGD_Escaner In oDocumentos
            Dim Encontrado As Boolean = False
            If oGD_Escaner.Cantidad > 0 Then Encontrado = True

            If oGD_Escaner.Encontrado.HasValue Then
                oGD.ActualizarDocumento(Nothing, trabajo.id, oGD_Escaner.IdDocumento, Encontrado)
            Else
                oGD.Guardar(trabajo.id, oGD_Escaner.IdDocumento, Encontrado)
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
                IO.File.Copy(urlFija, ruta & "\" & nombreArchivo)
            End Using
        Next

    End Function

    Function NombreTrabajoSinCaracteresEspeciales(ByVal nombreTrabajo As String) As String
        Return Regex.Replace(nombreTrabajo, "[^A-Za-z0-9\-/]", "")
    End Function
    Function obtenerNombreArchivo(ByVal ruta As String) As String
        Dim vRutas() As String
        vRutas = ruta.Split("\")
        Return vRutas(vRutas.Count - 1)
    End Function
#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(100, UsuarioID) = False Then
            Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
        End If
        If Not IsPostBack Then
            CargarTrabajos(Session("IDUsuario").ToString)
            CargarTiposDeRecolección()
            If Not Session("TrabajoId") = Nothing Then
                hfIdTrabajo.Value = Session("TrabajoId").ToString
                cargarTrabajo(Session("TrabajoId").ToString)
            End If
        End If
    End Sub
	Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
		CargarTrabajos(Session("IDUsuario").ToString)
	End Sub
	Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
		gvTrabajos.PageIndex = e.NewPageIndex
		CargarTrabajos(Session("IDUsuario").ToString)
	End Sub
	Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        If e.CommandName = "Actualizar" Then
            'cargarTrabajo(Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")))
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Session("TrabajoId") = hfIdTrabajo.Value
            CargarConfiguracionTrabajo(hfIdTrabajo.Value)
            cargarTrabajo(hfIdTrabajo.Value)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            ConfigurarPryCuali(hfIdTrabajo.Value)
            Dim oPlaneacion As New PlaneacionProduccion
            If oPlaneacion.ObtenerEstimacionCiudadxTrabajoList(hfIdTrabajo.Value).Count = 0 Then
                btnEstimacionAuto.Visible = True
                btnEstimaciones.Visible = False
            Else
                btnEstimacionAuto.Visible = False
                btnEstimaciones.Visible = True
            End If
            pnlCierre.Visible = False
        ElseIf e.CommandName = "Avance" Then
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Response.Redirect("../RP_Reportes/AvanceDeCampo.aspx?TrabajoId=" & hfIdTrabajo.Value)
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
					'Response.Redirect("../RP_Reportes/InformacionGeneral.aspx?idTr=" & hfIdTrabajo.Value & "&URLBACK=../OP_CUANTITATIVO/TRABAJOS.ASPX")
				Case TipoFicha.Sesiones
					Response.Redirect("../OP_Cualitativo/FichaSesion.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes")
				Case TipoFicha.Observaciones
					Response.Redirect("../OP_Cualitativo/FichaObservacion.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes")
				Case TipoFicha.Entrevistas
					Response.Redirect("../OP_Cualitativo/FichaEntrevistas.aspx?idtrabajo=" & hfIdTrabajo.Value & "&op=yes")
			End Select
		Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
		End Try


	End Sub
	Protected Sub btnCapacitaciones_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCapacitaciones.Click
        Response.Redirect("../TH_TalentoHumano/Capacitacion.aspx?idtrabajo=" & hfIdTrabajo.Value)
    End Sub
    Protected Sub btnEstimacionAuto_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEstimacionAuto.Click
        Dim oCoorCampo As New CoordinacionCampo
        Dim listaMuestra = (From lmuestra In oCoorCampo.ObtenerMuestraxEstudioList(hfIdTrabajo.Value)
                            Select idMuestra = lmuestra.Id, departamento = lmuestra.C_Divipola.DivDeptoNombre, ciudad = lmuestra.C_Divipola.DivMuniNombre,
                            cantidad = lmuestra.Cantidad).OrderBy(Function(x) x.ciudad)
        If listaMuestra.Count = 0 Then
            ShowNotification("No se ha configurado aún la muestra. Por favor haga clic en Muestra para distribuir la muestra del estudio", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            Exit Sub
        End If
        Me.pnlEstimacion.Visible = True
        Me.pnlDatos.Visible = False

		accordion0.Visible = False
		accordion1.Visible = True
	End Sub
    Protected Sub btnGenerarPlaneacion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenerarPlaneacion.Click
        Dim oPlaneacion As New PlaneacionProduccion
        oPlaneacion.AgregarEstimacionAutomatica(hfIdTrabajo.Value, Session("IDUsuario").ToString, Me.chbDias.Items(0).Selected, Me.chbDias.Items(1).Selected, Me.chbDias.Items(2).Selected, Me.chbDias.Items(3).Selected, Me.chbDias.Items(4).Selected, Me.chbDias.Items(5).Selected, Me.chbDias.Items(6).Selected, chbFestivosExcluir.Checked)
        Me.btnEstimacionAuto.Visible = False
        Me.btnEstimaciones.Visible = True
        ShowNotification("Distribución automática realizada", ShowNotifications.InfoNotification)
        Me.pnlEstimacion.Visible = False
        Me.pnlDatos.Visible = True
		accordion0.Visible = False
		accordion1.Visible = True
	End Sub
    Protected Sub btnCancelGeneracionPlaneacion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelGeneracionPlaneacion.Click
        Me.pnlEstimacion.Visible = False
        Me.chbDias.Items(0).Selected = False
        Me.chbDias.Items(1).Selected = False
        Me.chbDias.Items(2).Selected = False
        Me.chbDias.Items(3).Selected = False
        Me.chbDias.Items(4).Selected = False
        Me.chbDias.Items(5).Selected = False
        Me.chbDias.Items(6).Selected = False
        chbFestivosExcluir.Checked = False
        Me.pnlEstimacion.Visible = False
        Me.pnlDatos.Visible = True
		accordion0.Visible = False
		accordion1.Visible = True
	End Sub
    Protected Sub btnMuestra_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMuestra.Click
        Response.Redirect("MuestraTrabajos.aspx?TrabajoId=" & hfIdTrabajo.Value & "&COE=fjaojoasjfois9080uioj5o252")
    End Sub
    Protected Sub btnEstimaciones_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEstimaciones.Click
        Response.Redirect("EstimacionProduccion.aspx?TrabajoId=" & hfIdTrabajo.Value)
    End Sub
    Protected Sub btnROCuestionario_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnROCuestionario.Click
        Response.Redirect("../OP_RO/Cuestionario.aspx?idtrabajo=" & hfIdTrabajo.Value)
    End Sub
    Protected Sub btnROInstructivo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnROInstructivo.Click
        Response.Redirect("../OP_RO/Instructivo.aspx?idtrabajo=" & hfIdTrabajo.Value)
    End Sub
    Protected Sub btnROMaterialAyuda_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnROMaterialAyuda.Click
        Response.Redirect("../OP_RO/MaterialAyuda.aspx?idtrabajo=" & hfIdTrabajo.Value)
    End Sub
    Protected Sub btnROMetodologia_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnROMetodologia.Click
        Response.Redirect("../OP_RO/Metodologia.aspx?idtrabajo=" & hfIdTrabajo.Value)
    End Sub

    Protected Sub btnEstadoTareas_Click(sender As Object, e As EventArgs) Handles btnEstadoTareas.Click
        Response.Redirect("../CORE/Gestion-Tareas.aspx?IdTrabajo=" & hfIdTrabajo.Value & "&URLRetorno=" & UrlOriginal.OP_Cuantitativo_Trabajos & "&IdUnidadEjecuta=" & UnidadesCore.COE & "&IdRolEstima=" & ListaRoles.COE)
    End Sub


    Protected Sub btnCerrarTrabajo_Click(sender As Object, e As EventArgs) Handles btnCerrarTrabajo.Click
        Dim oGD As New GD.GD_Procedimientos
        Dim oTrabajo As New Trabajo
        Dim idTrabajo As Integer = CInt(hfIdTrabajo.Value)
        Dim oEstados = oTrabajo.ListadoTrabajos(idTrabajo, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault

        If oEstados.Estado <> 1 AndAlso oEstados.Estado <> 15 Then 'Cerrado en operaciones
            ShowNotification("Debe estar cerrado primero en operaciones, para poderlo cerrar en proyectos, ¡comuniquese con el COE!", ShowNotifications.ErrorNotification)
            Exit Sub
        Else
            If oEstados.Estado <> 15 Then
                oTrabajo.CambioEstado(idTrabajo, 15, "Trabajo en proceso de Cierre Operaciones", Session("IDUsuario").ToString)
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
		accordion0.Visible = False
		accordion1.Visible = True
	End Sub


    Protected Sub btnConfirmarCierre_Click(sender As Object, e As EventArgs) Handles btnConfirmarCierre.Click
        Dim o As New Trabajo
        Dim oCI As New OP.CentroInformacion
        o.CambioEstado(hfIdTrabajo.Value, 2, txtObservacionesCierre.Text, Session("IDUsuario").ToString)
        oCI.GuardarLogCierres(hfIdTrabajo.Value, Session("IDUsuario").ToString, txtObservacionesCierre.Text, True)
        CargarTrabajos(Session("IDUsuario").ToString)
        Me.pnlInfoTrabajo.Visible = True
        Me.txtObservacionesCierre.Text = ""
        Me.pnlCierre.Visible = False
        EnviarEmailCambioEstado()
		accordion0.Visible = False
		accordion1.Visible = True
	End Sub

    Sub EnviarEmailCambioEstado()
        Dim oEnviarCorreo As New EnviarCorreo
		Try
			If String.IsNullOrEmpty(hfIdTrabajo.Value) Then
				Throw New Exception("Debe elegir un estudio o guardarlo antes de continuar")
			End If
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/CambioEstadoTrabajo.aspx?idTrabajo=" & hfIdTrabajo.Value)
		Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
		End Try
	End Sub

    Protected Sub btnPresupuestos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPresupuestos.Click
        Response.Redirect("../OP_Cuantitativo/SolicitudPresupuestoInterno.aspx?IdTrabajo=" & hfIdTrabajo.Value)
    End Sub

    Protected Sub btnCargar_Click(sender As Object, e As EventArgs) Handles btnCargar.Click
        Response.Redirect("../OP_Cuantitativo/ImportarDatos.aspx?IdTrabajo=" & hfIdTrabajo.Value)
    End Sub

    Protected Sub btnForzarCierre_Click(sender As Object, e As EventArgs) Handles btnForzarCierre.Click
        Dim o As New Trabajo
        Dim oCI As New OP.CentroInformacion
        Dim oGDActualizar As New GD.GD_Procedimientos
        Dim daTrabajo As New Trabajo
        Dim oTrabajo = daTrabajo.DevolverxID(hfIdTrabajo.Value)

        For fila = 0 To gvReporte.Rows.Count - 1
            If gvReporte.Rows(fila).Cells(3).Text = "NO" AndAlso DirectCast(gvReporte.Rows(fila).FindControl("txtobservacion"), TextBox).Visible Then
                oGDActualizar.ActualizarObservacion(gvReporte.DataKeys(fila)("Id"), DirectCast(gvReporte.Rows(fila).FindControl("txtobservacion"), TextBox).Text)
            End If
        Next

        o.CambioEstado(hfIdTrabajo.Value, 2, txtObservacionesCierre.Text, Session("IDUsuario").ToString)
        oCI.GuardarLogCierres(hfIdTrabajo.Value, Session("IDUsuario").ToString, txtObservacionesCierre.Text, True)
        Me.pnlInfoTrabajo.Visible = True
        Me.txtObservacionesCierre.Text = ""
        Me.pnlCierre.Visible = False
        btnCerrarTrabajo.Visible = False
        btnReporteCierre.Visible = True
        EnviarEmailCambioEstado()
        CargarTrabajos(Session("IDUsuario").ToString)
		accordion0.Visible = False
		accordion1.Visible = True
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
		accordion0.Visible = False
		accordion1.Visible = True
	End Sub

    Private Sub gvReporte_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReporte.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim data = CType(e.Row.DataItem, GD_EscanerDocumentos_Get_Result)
            If data.CodEncontrado = True OrElse data.RolResponsableCierre <> 10 OrElse data.Observacion <> "" Then
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
	End Sub

	Protected Sub btnCancelarCierre_Click(sender As Object, e As EventArgs) Handles btnCancelarCierre.Click
		Me.pnlInfoTrabajo.Visible = True
		Me.txtObservacionesCierre.Text = ""
		Me.pnlCierre.Visible = False
	End Sub

	Protected Sub ddlEstado_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEstado.SelectedIndexChanged
		CargarTrabajos(Session("IDUsuario").ToString)
	End Sub

	Private Sub btnAbrirOtroTrabajo_Click(sender As Object, e As EventArgs) Handles btnAbrirOtroTrabajo.Click
		accordion1.Visible = False
		accordion0.Visible = True
	End Sub

    Private Sub lbtnVolver_Click(sender As Object, e As EventArgs) Handles lbtnVolver.Click
        Session.Remove("TrabajoId")
        Response.Redirect("~/OP_Cuantitativo/Trabajos.aspx")
    End Sub

    Private Sub btnVariablesControl_Click(sender As Object, e As EventArgs) Handles btnVariablesControl.Click
        Response.Redirect("../PY_Proyectos/VariablesControl.aspx?idTr=" & hfIdTrabajo.Value & "&modal=GP")
    End Sub

    Private Sub gvTrabajos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvTrabajos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Estado = CType(e.Row.DataItem, PY_Trabajos_GET_All_Result).Estado
            If Estado = eEstadoBloqueo.Cerrado Or Estado = eEstadoBloqueo.Anulado Then
                e.Row.Cells(9).BackColor = System.Drawing.Color.FromArgb(224, 158, 158)
                e.Row.Cells(9).ForeColor = System.Drawing.Color.FromArgb(0, 0, 0)
            End If
        End If
    End Sub
#End Region

End Class