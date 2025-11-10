Imports System.IO
Imports WebMatrix.Util
Imports CoreProject

Public Class ReporteInconsistencias
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExport)
    End Sub

    Private Sub gvProduccion_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvProduccion.PageIndexChanging
        gvProduccion.PageIndex = e.NewPageIndex
        CargarDatos(txtFechaInicio.Text, txtFechaTerminacion.Text, ddlTareas.SelectedValue, Nothing)
    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        Dim tarea As Int32? = ddlTareas.SelectedValue
        If tarea = -1 Then tarea = Nothing
        Dim o As New Reportes.AvanceCampo
        'Me.gvExport.DataSource = o.ObtenerProduccionCampoXfecha(txtFechaInicio.Text, txtFechaTerminacion.Text, ciudad)
        Me.gvExport.DataBind()
        Me.gvExport.Visible = True
        'Actualiza los datos del gridview
        Me.gvExport.AllowPaging = False
        'Me.gvExport.DataBind()

        'Crea variables en memoria para almacenar el contenido del gridview
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Crea una nueva página html y un form dentro de ella
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        'Cambie el estado del gridview para que no guarde los controles de vista
        Me.gvExport.EnableViewState = False
        'Quita la validación de la página 
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        'Agrega el form creado
        pagina.Controls.Add(form)
        'Agrega el gridview al form
        form.Controls.Add(gvExport)
        'Hace un render para pasar los valores de la página al htmltextwriter que contendrá los datos del gridview
        pagina.RenderControl(htw)
        'Hace un response para descargar el control
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=Reporte_Observaciones.xls")
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        gvExport.Visible = False

    End Sub

#End Region

#Region "Metodos"
    Sub CargarDatos(ByVal FechaI As Date, FechaF As Date, Ciudad As Int32?, VerifResultado As Int32?)
        If Ciudad = -1 Then Ciudad = Nothing
        If VerifResultado = -1 Then VerifResultado = Nothing
        Dim o As New Reportes.AvanceCampo
        Me.gvProduccion.DataSource = o.ObtenerProduccionCampoXfecha(FechaI, FechaF, Ciudad, VerifResultado)
        Me.gvProduccion.DataBind()
    End Sub
#End Region

    Private Sub btnConsultar_Click(sender As Object, e As System.EventArgs) Handles btnConsultar.Click
        'CargarDatos(txtFechaInicio.Text, txtFechaTerminacion.Text, ddlTareas.SelectedValue)
        Me.gvProduccion.DataBind()
    End Sub

    Private Sub SQLDsDatos_Selecting(sender As Object, e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SQLDsDatos.Selecting
        e.Command.CommandTimeout = 180
    End Sub

    Private Sub ReporteInconsistencias_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(136, UsuarioID) = False Then
            Response.Redirect("../../Home.aspx")
        End If
    End Sub
End Class