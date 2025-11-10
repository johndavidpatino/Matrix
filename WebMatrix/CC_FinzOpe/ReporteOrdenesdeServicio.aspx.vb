Imports CoreProject
Imports System.IO

Public Class ReporteOrdenesdeServicio
    Inherits System.Web.UI.Page
    Dim Op As New ProcesosInternos

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(132, UsuarioID) = False Then
            Response.Redirect("../FI_AdministrativoFinanciero/Default.aspx")
        End If
        If Not IsPostBack Then
            ObtenerContratista()
        End If

        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExportar)

    End Sub

    Private Sub GvOrdenes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvOrdenes.PageIndexChanging
        GvOrdenes.PageIndex = e.NewPageIndex
        CargarOrdenes()
    End Sub

    Sub ObtenerContratista()
        Dim oContratista As New Contratista
        ddlContratista.DataSource = oContratista.ObtenerContratistas(Nothing, Nothing, Nothing)
        ddlContratista.DataValueField = "Identificacion"
        ddlContratista.DataTextField = "Nombre"
        ddlContratista.DataBind()
        ddlContratista.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargarOrdenes()
    End Sub

    Sub CargarOrdenes()
        Dim Orden As Int64?
        Dim Contratista As Int64?
        Dim estado As Int16?
        Dim fechaini As DateTime?
        Dim fechafin As DateTime?
        Dim TodosCampos As String

        If ddlContratista.SelectedValue = -1 Then
            Contratista = Nothing
        Else
            Contratista = ddlContratista.SelectedValue
        End If
        If ddlestado.SelectedValue = -1 Then
            estado = Nothing
        Else
            estado = ddlestado.SelectedValue
        End If
        If txtorden.Text = "" Then
            Orden = Nothing
        Else
            Orden = txtorden.Text
        End If
        If txtfechainicio.Text = "" Then
            fechaini = Nothing
        Else
            fechaini = txtfechainicio.Text
        End If
        If txtfechafin.Text = "" Then
            fechafin = Nothing
        Else
            fechafin = txtfechafin.Text
        End If

        If txtTodosCampos.Text = "" Then
            TodosCampos = Nothing
        Else
            TodosCampos = txtTodosCampos.Text
        End If

        GvOrdenes.DataSource = Op.ReporteOrdenes(Orden, Contratista, estado, fechaini, fechafin, TodosCampos)
        GvOrdenes.DataBind()

    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        CargarOrdenes()
        GvOrdenes.Visible = True
        'Actualiza los datos del gridview
        GvOrdenes.AllowPaging = False
        GvOrdenes.DataBind()

        'Crea variables en memoria para almacenar el contenido del gridview
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Crea una nueva página html y un form dentro de ella
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        'Cambie el estado del gridview para que no guarde los controles de vista
        GvOrdenes.EnableViewState = False
        'Quita la validación de la página 
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        'Agrega el form creado
        pagina.Controls.Add(form)
        'Agrega el gridview al form
        form.Controls.Add(GvOrdenes)
        'Hace un render para pasar los valores de la página al htmltextwriter que contendrá los datos del gridview
        pagina.RenderControl(htw)
        'Hace un response para descargar el control
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=Reporte_OrdenesServicio.xls")
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        GvOrdenes.Visible = False
    End Sub


End Class