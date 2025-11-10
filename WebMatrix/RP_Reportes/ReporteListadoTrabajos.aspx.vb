Imports CoreProject
Imports System.IO
Imports WebMatrix.Util

Public Class ReporteListadoTrabajos
    Inherits System.Web.UI.Page

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            'If permisos.VerificarPermisoUsuario(19, UsuarioID) = False Then
            '    Response.Redirect("../Home.aspx")
            'End If
            CargarCOES()
            CargarTrabajos()
        End If

        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExportar)

    End Sub

    Protected Sub ddlCOE_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCOE.SelectedIndexChanged
        CargarTrabajos()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        CargarTrabajos()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

#End Region

#Region "Funciones y Metodos"
    Sub CargarTrabajos()
        If ddlCOE.SelectedIndex = -1 Then Exit Sub
        Dim oTrabajo As New Trabajo
        gvDatos.DataSource = oTrabajo.ObtenerTrabajosxCOE(ddlCOE.SelectedValue)
        gvDatos.DataBind()
    End Sub

    Sub CargarCOES()
        Dim oCOE As New Trabajo
        ddlCOE.DataSource = oCOE.PY_Trabajos_COES
        ddlCOE.DataValueField = "Id"
        ddlCOE.DataTextField = "Nombre"
        ddlCOE.DataBind()
        ddlCOE.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
        ddlCOE.Items.Insert(1, New ListItem With {.Text = "Sin COE", .Value = 0})
    End Sub

#End Region

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        CargarTrabajos()
        gvDatos.Visible = True
        'Actualiza los datos del gridview
        gvDatos.AllowPaging = False
        gvDatos.DataBind()

        'Crea variables en memoria para almacenar el contenido del gridview
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Crea una nueva página html y un form dentro de ella
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        'Cambie el estado del gridview para que no guarde los controles de vista
        gvDatos.EnableViewState = False
        'Quita la validación de la página 
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        'Agrega el form creado
        pagina.Controls.Add(form)
        'Agrega el gridview al form
        form.Controls.Add(gvDatos)
        'Hace un render para pasar los valores de la página al htmltextwriter que contendrá los datos del gridview
        pagina.RenderControl(htw)
        'Hace un response para descargar el control
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=Reporte_ListadoTrabajos.xls")
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        gvDatos.Visible = False
    End Sub
End Class