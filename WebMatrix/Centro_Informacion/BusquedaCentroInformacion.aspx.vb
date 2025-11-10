Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Imports CoreProject.CIEntities
Imports ClosedXML.Excel
Imports CoreProject.OP

Public Class BusquedaCentroInformacion
    Inherits System.Web.UI.Page
#Region "Propiedades GridView"

    Public Property tamanoPagina() As Integer
        Get
            Return Session("tamanoPagina")
        End Get
        Set(ByVal value As Integer)
            Session("tamanoPagina") = value
        End Set
    End Property

    Public Property paginaActual() As Integer
        Get
            Return Session("paginaActual")
        End Get
        Set(ByVal value As Integer)
            Session("paginaActual") = value
        End Set
    End Property

    Public Property cantidadPaginas() As Integer
        Get
            Return Session("cantidadPaginas")
        End Get
        Set(ByVal value As Integer)
            Session("cantidadPaginas") = value
        End Set
    End Property

    Public Property cantidadRegistros() As Int32?
        Get
            Return Session("cantidadRegistros")
        End Get
        Set(ByVal value As Int32?)
            Session("cantidadRegistros") = value
        End Set
    End Property

#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            tamanoPagina = 10
            paginaActual = 1
        End If
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        cargarGrillaTrabajos()
        limpiar()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging

    End Sub

    Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand

        Dim cargarGrilla As Boolean = False

        Select Case e.CommandArgument
            Case "First"
                paginaActual = 1
                cargarGrilla = True
            Case "Prev"
                If paginaActual > 1 Then
                    paginaActual -= 1
                    cargarGrilla = True
                End If
            Case "Next"
                If paginaActual < cantidadPaginas Then
                    paginaActual += 1
                    cargarGrilla = True
                End If
            Case "Last"
                If paginaActual < cantidadPaginas Then
                    paginaActual = cantidadPaginas
                    cargarGrilla = True
                End If
        End Select
        If paginaActual > cantidadPaginas Then
            paginaActual = 1
            cargarGrilla = True
        End If

        If cargarGrilla Then
            cargarGrillaTrabajos()
        End If

        Select Case e.CommandName
            Case "Ver"
                Dim idTrabajo = Int64.Parse(gvTrabajos.DataKeys(CInt(e.CommandArgument))("IdTrabajo"))
                Dim oTrabajo As New Trabajo
                Dim oProyecto As New Proyecto
                Dim oEstudio As New Estudio
                Dim oPropuesta As New Propuesta
                Dim infoT = oTrabajo.obtenerXId(idTrabajo)
                Dim infoP = oProyecto.obtenerXId(infoT.ProyectoId)
                Dim infoE = oEstudio.ObtenerXID(infoP.EstudioId)
                Dim infoPr = oPropuesta.DevolverxID(infoE.PropuestaId)
                CargarInfoBrief(infoPr.Brief)
                cargarDocumentosXIdTrabajo(idTrabajo)
                cargarMediosXTrabajo(idTrabajo)
                pnlListadoDocsTotal.Visible = True
                pnlBrief.Visible = True
                lblMensaje.Visible = True
                lblIdTrabajo.Text = infoT.id
                lblNombreTrabajo.Text = infoT.NombreTrabajo
                hfIdTrabajo.Value = Int64.Parse(gvTrabajos.DataKeys(CInt(e.CommandArgument))("IdTrabajo"))
                upInfoTrabajo.Update()
        End Select



    End Sub

    Private Sub gvTareasXDocumentos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareasXDocumentos.RowCommand
        hfIdDocDescarga.Value = gvTareasXDocumentos.DataKeys(CInt(e.CommandArgument))("IdDocumento")
        Me.PnlListadoDocsDescargar.Visible = True
        CargarDocumentosCargados()
        ActivateTab(1)
        upInfoTrabajo.Update()
    End Sub

    Private Sub gvMedios_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMedios.RowCommand
        Select Case e.CommandName
            Case "Solicitar"
                Dim oGuardar As New CentroInformacion

                Dim otxtUsuario As New TextBox With {.Text = Session("IDUsuario").ToString}

                Dim IdSolicitud = oGuardar.GuardarSolicitudMedios(gvMedios.DataKeys(e.CommandArgument).Value, otxtUsuario.Text)

                EnviarEmail(IdSolicitud)

                ShowNotification("Solicitud enviada correctamente", ShowNotifications.InfoNotification)
        End Select
    End Sub
#End Region
#Region "Metodos"
    Public Sub CargarInfoBrief(ByVal idbrief As Int64)
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
    End Sub
    Sub EnviarEmail(ByVal IdSolicitud As Decimal)
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            Dim script As String = ""
			oEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/SolicitudCentroInformacion.aspx?IdSolicitud=" & IdSolicitud)
			ShowNotification("Notificación enviada correctamente", ShowNotifications.InfoNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub

    Sub cargarGrillaTrabajos()
        Dim oBusqueda As New CentroInformacion
        Dim lTrabajos = oBusqueda.busquedaXVariosCampos(txtBuscar.Text)
        cantidadRegistros = lTrabajos.Count
        cantidadPaginas = CInt(Decimal.Ceiling(CDec(cantidadRegistros) / tamanoPagina))
        If paginaActual > cantidadPaginas Then
            paginaActual = 1
        End If
        gvTrabajos.DataSource = lTrabajos
        gvTrabajos.PageIndex = paginaActual - 1
        gvTrabajos.DataBind()
    End Sub

    Sub CargarDocumentosCargados()
        Dim o As New RepositorioDocumentos
        gvDocumentosCargados.DataSource = o.obtenerDocumentosXIdContenedorXIdDocumento(hfIdTrabajo.Value, hfIdDocDescarga.Value)
        gvDocumentosCargados.DataBind()
    End Sub

    Sub cargarDocumentosXIdTrabajo(ByVal IdTrabajo As Int64)
        Dim oCI As New CentroInformacion
        pnlListadoDocsTotal.Visible = True
        gvTareasXDocumentos.DataSource = oCI.obtenerdocumentosrecuperacion(IdTrabajo)
        gvTareasXDocumentos.DataBind()
    End Sub
    Sub descargarArchivos(ByVal url As String)
        Dim urlFija As String
        urlFija = "\ArchivosCargados"
        'HACK Deuda tecnica Aquí se deberia colocar la ruta raíz en un parametro en la base de datos

        urlFija = Server.MapPath(urlFija & "\" & url)

        Dim path As New IO.FileInfo(urlFija)

        Response.Clear()
        Response.AddHeader("Content-Disposition", "attachment; filename=" & path.Name)
        Response.AddHeader("Content-Length", path.Length.ToString())
        Response.ContentType = "application/octet-stream"
        Response.WriteFile(urlFija)
        Response.End()

    End Sub
    Public Sub ActivateTab(ByVal tabIndex As Integer)
        Dim mePage As Page = CType(HttpContext.Current.Handler, Page)
        ScriptManager.RegisterStartupScript(mePage, upInfoTrabajo.GetType(), "tabActivate", "$('#tabs').tabs();$('#tabs').tabs('option', 'selected'," & tabIndex.ToString & ");", True)
    End Sub
    Sub cargarMediosXTrabajo(ByVal idTrabajo As Int64)
        Dim daCI As New CentroInformacion
        Dim lstAlmacenamiento As List(Of CI_DetalleAlmacenamiento_Get_Result)
        lstAlmacenamiento = daCI.obtenerdetallesxidtrabajo(idTrabajo)
        gvMedios.DataSource = lstAlmacenamiento
        gvMedios.DataBind()
    End Sub
    Sub limpiar()

        gvMedios.DataSource = Nothing
        gvMedios.DataBind()
        gvTareasXDocumentos.DataSource = Nothing
        gvTareasXDocumentos.DataBind()
        gvDocumentosCargados.DataSource = Nothing
        gvDocumentosCargados.DataBind()

        txtAntecedentes.Text = ""
        txtObjetivos.Text = ""
        txtActionStandard.Text = ""
        txtMetodologia.Text = ""
        txtTargetGroup.Text = ""
        txtTiempos.Text = ""
        txtPresupuesto.Text = ""
        txtMateriales.Text = ""
        txtEstudiosAnteriores.Text = ""
        txtFormatos.Text = ""
        txtAprobaciones.Text = ""
        txtCompetencia.Text = ""

        lblIdTrabajo.Text = ""
        lblNombreTrabajo.Text = ""

        lblMensaje.Visible = False

        pnlBrief.Visible = False
        hfIdTrabajo.Value = ""
    End Sub
#End Region
End Class