Imports CoreProject
Imports WebMatrix.Util
Public Class Configuracion_Tareas_Documentos
    Inherits System.Web.UI.Page

#Region "Enumerador"
    Enum TipoDocumentos
        Requerido = 1
        Entregable = 2
    End Enum
#End Region

#Region "Propiedades"

    Private _idTarea As Int64
    Public Property idTarea() As Int64
        Get
            Return _idTarea
        End Get
        Set(ByVal value As Int64)
            _idTarea = value
        End Set
    End Property

    Private _idTipoDocumento As TipoDocumentos
    Public Property TipoDocumento() As TipoDocumentos
        Get
            Return _idTipoDocumento
        End Get
        Set(ByVal value As TipoDocumentos)
            _idTipoDocumento = value
        End Set
    End Property

#End Region

#Region "Metodos"
    Sub CargarDocumentos(ByVal gv As GridView, ByVal Asignados As Boolean)
        Dim oTareas_Documentos As New Tareas_Documentos
        gv.DataSource = oTareas_Documentos.obtenerConfiguracionDocumentosXTarea(idTarea, TipoDocumento, Asignados)
        gv.DataBind()
    End Sub
#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("IdTarea") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdTarea"), idTarea)
            hfIdTarea.Value = idTarea
        End If
        If Request.QueryString("IdTipoDocumento") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdTipoDocumento"), _idTipoDocumento)
        End If
        lblTipoDocumento.Text = [Enum].GetName(TipoDocumento.GetType, TipoDocumento)

        Dim oTarea As New CoreProject.Tarea

        lblNombreTarea.Text = oTarea.obtenerXId(idTarea).Tarea

        lnkVolver.PostBackUrl = "Configuracion_Tareas.aspx"

        If Not IsPostBack Then
            If idTarea > 0 AndAlso TipoDocumento > 0 Then
                CargarDocumentos(gvArchivosAsignados, True)
                CargarDocumentos(gvArchivosNoAsignados, False)
            End If
        End If
    End Sub
    Private Sub gvArchivosNoAsignados_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvArchivosNoAsignados.PageIndexChanging
        gvArchivosNoAsignados.PageIndex = e.NewPageIndex
        CargarDocumentos(gvArchivosNoAsignados, False)
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub
    Protected Sub btnGrabar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGrabar.Click
        Dim oCORE_Tareas_Documentos As New Tareas_Documentos
        oCORE_Tareas_Documentos.grabar(idTarea, hfIdDocumentoNoAsignado.Value, TipoDocumento, ddlEsOpcional.SelectedValue)
        CargarDocumentos(gvArchivosAsignados, True)
        CargarDocumentos(gvArchivosNoAsignados, False)
        ddlEsOpcional.SelectedValue = -1
    End Sub
    Private Sub gvArchivosAsignados_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvArchivosAsignados.RowCommand
        If e.CommandName = "Quitar" Then
            Dim oTareas_Documentos As New Tareas_Documentos
            oTareas_Documentos.Eliminar(gvArchivosAsignados.DataKeys(e.CommandArgument)("Id"))
            CargarDocumentos(gvArchivosAsignados, True)
            CargarDocumentos(gvArchivosNoAsignados, False)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End If
    End Sub
#End Region



End Class