Imports CoreProject
Imports WebMatrix.Util
Imports ClosedXML.Excel
Imports CoreProject.CIEntities
Imports CoreProject.OP

Public Class RecuperacionCierre
    Inherits System.Web.UI.Page

#Region "Funciones y Métodos"
    Sub CargarTrabajos()
        Dim oTrabajo As New Trabajo
        gvTrabajos.DataSource = oTrabajo.ListadoTrabajos(Nothing, 12, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        gvTrabajos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarTrabajos()
        End If
    End Sub

    Sub CargarDocumentosCargados()
        Dim o As New RepositorioDocumentos
        gvDocumentosCargados.DataSource = o.obtenerDocumentosXIdContenedorXIdDocumento(hfIdTrabajo.Value, hfIdDocDescarga.Value)
        gvDocumentosCargados.DataBind()
    End Sub

    Sub cargarDocumentosXIdTrabajo(ByVal IdTrabajo As Int64)
        Dim oCI As New CentroInformacion
        gvTareasXDocumentos.DataSource = oCI.obtenerdocumentosrecuperacion(IdTrabajo)
        gvTareasXDocumentos.DataBind()
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Dim oTrabajo As New Trabajo
        gvTrabajos.DataSource = oTrabajo.ListadoTrabajos(Nothing, 12, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, txtBuscar.Text)
        gvTrabajos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
        gvTrabajos.PageIndex = e.NewPageIndex
        CargarTrabajos()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        If e.CommandName = "Gestionar" Then
            Dim daCentro As New Trabajo
            hfIdTrabajo.Value = gvTrabajos.DataKeys(e.CommandArgument).Value
            Me.pnlListadoDocumentos.Visible = True
            Me.pnlListadoDocsTotal.Visible = True
            Me.PnlListadoDocsDescargar.Visible = False
            cargarDocumentosXIdTrabajo(hfIdTrabajo.Value)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        End If
    End Sub

    Private Sub gvTareasXDocumentos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareasXDocumentos.RowCommand
        hfIdDocDescarga.Value = gvTareasXDocumentos.DataKeys(CInt(e.CommandArgument))("IdDocumento")
        Me.PnlListadoDocsDescargar.Visible = True
        Me.pnlListadoDocsTotal.Visible = False
        CargarDocumentosCargados()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub btnVolverDescarga_Click(sender As Object, e As System.EventArgs) Handles btnVolverDescarga.Click
        Me.pnlListadoDocumentos.Visible = True
        Me.pnlListadoDocsTotal.Visible = True
        Me.PnlListadoDocsDescargar.Visible = False
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
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
#End Region

End Class