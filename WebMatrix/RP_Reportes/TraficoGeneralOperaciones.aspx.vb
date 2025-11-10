Imports WebMatrix.Util
Imports CoreProject
Imports System.IO

Public Class REP_TraficoGeneralOperaciones
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarGerencias()
            CargarGruposUnidad()
            CargarMetodologias()
        End If
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExportar)
    End Sub


#End Region

    Sub CargarGruposUnidad()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlUnidades.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(1)
        ddlUnidades.DataValueField = "id"
        ddlUnidades.DataTextField = "GrupoUnidad"
        ddlUnidades.DataBind()
        ddlUnidades.Items.Insert(0, New ListItem With {.Text = "--Ver todas--", .Value = -1})
    End Sub

    Sub CargarGerencias()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlGerencias.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(2)
        ddlGerencias.DataValueField = "id"
        ddlGerencias.DataTextField = "GrupoUnidad"
        ddlGerencias.DataBind()
        ddlGerencias.Items.Insert(0, New ListItem With {.Text = "--Ver todas--", .Value = -1})
    End Sub

    Sub CargarMetodologias()
        Dim oMetodologias As New MetodologiaOperaciones
        ddlMetodologia.DataSource = oMetodologias.obtenerMetodologiasCuanti
        ddlMetodologia.DataValueField = "Id"
        ddlMetodologia.DataTextField = "MetNombre"
        ddlMetodologia.DataBind()
        ddlMetodologia.Items.Insert(0, New ListItem With {.Text = "--Ver todas--", .Value = -1})
    End Sub



    Protected Sub btnMostrar_Click(sender As Object, e As EventArgs) Handles btnMostrar.Click
        Me.gvDatos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub SqlDsDatos_Selecting(sender As Object, e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SqlDsDatos.Selecting
        e.Command.CommandTimeout = 180
    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        'Me.gvDatos.Visible = True
        'Actualiza los datos del gridview
        If Me.gvDatos.Rows.Count = 0 Then
            Me.gvDatos.DataBind()
        End If
        'Crea variables en memoria para almacenar el contenido del gridview
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Crea una nueva página html y un form dentro de ella
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        'Cambie el estado del gridview para que no guarde los controles de vista
        Me.gvDatos.EnableViewState = False
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
        Response.AddHeader("Content-Disposition", "attachment;filename=ExportTraficoGeneralOperaciones.xls")
        Response.Charset = "UTF-8"

        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        'gvDatos.Visible = False
    End Sub
End Class