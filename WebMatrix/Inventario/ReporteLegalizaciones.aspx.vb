Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Imports ClosedXML.Excel
Imports CoreProject.OP
Imports CoreProject.INV
Imports System.IO

Public Class ReporteLegalizaciones
    Inherits System.Web.UI.Page


#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarBU()
        End If
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExportar)
    End Sub

    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Private Sub gvLegalizaciones_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvLegalizaciones.PageIndexChanging
        gvLegalizaciones.PageIndex = e.NewPageIndex
        CargarReporteLegalizaciones()
    End Sub

#End Region


#Region "Grillas"

    Sub CargarGridPersonas()
        Dim o As New Personas
        Dim cedula As Int64? = Nothing
        Dim nombre As String = Nothing
        If IsNumeric(txtCedulaUsuario.Text) Then cedula = txtCedulaUsuario.Text
        If Not txtNombreUsuario.Text = "" Then nombre = txtNombreUsuario.Text
        Me.gvUsuarios.DataSource = o.ObtenerPersonasxCCNombre(cedula, nombre)
        Me.gvUsuarios.DataBind()
    End Sub

    Sub CargarReporteLegalizaciones()
        Dim oReporte As New Inventario
        Dim FechaIni As DateTime? = Nothing
        Dim FechaFin As DateTime? = Nothing
        Dim UsuarioAsignado As Int64? = Nothing
        Dim Articulo As Int64? = Nothing
        Dim BU As Int32? = Nothing
        Dim JobBookCodigo As String = Nothing
        Dim TodosCampos As String = Nothing

        FechaIni = If(txtfechainicio.Text = "", CType(Nothing, DateTime?), CType(txtfechainicio.Text, DateTime?))
        FechaFin = If(txtfechafin.Text = "", CType(Nothing, DateTime?), CType(txtfechafin.Text, DateTime?))
        UsuarioAsignado = If(txtIdUsuario.Text = "", CType(Nothing, Int64?), CType(txtIdUsuario.Text, Int64?))
        Articulo = If(ddlIdArticulo.SelectedValue = -1, CType(Nothing, Int64?), CType(ddlIdArticulo.SelectedValue, Int64?))
        BU = If(ddlBU.SelectedValue = -1, CType(Nothing, Int32?), CType(ddlBU.SelectedValue, Int32?))
        JobBookCodigo = If(txtJBIJBE.Text = "", CType(Nothing, String), CType(txtJBIJBE.Text, String))
        TodosCampos = If(txtTodosCampos.Text = "", CType(Nothing, String), CType(txtTodosCampos.Text, String))

        If Articulo = 8 Then
            gvLegalizaciones.Columns(12).Visible = True  'Firmas
            gvLegalizaciones.Columns(13).Visible = True  'Devoluciones
            gvLegalizaciones.Columns(14).Visible = True  'NotasCredito
            gvLegalizaciones.Columns(15).Visible = True  'DescuentoNomina
        Else
            gvLegalizaciones.Columns(12).Visible = False  'Firmas
            gvLegalizaciones.Columns(13).Visible = False  'Devoluciones
            gvLegalizaciones.Columns(14).Visible = False  'NotasCredito
            gvLegalizaciones.Columns(15).Visible = False  'DescuentoNomina
        End If

        If Articulo = 9 Then
            gvLegalizaciones.Columns(9).Visible = True  'Unidades
            gvLegalizaciones.Columns(10).Visible = True  'Valor Carrera
        Else
            gvLegalizaciones.Columns(9).Visible = False  'Unidades
            gvLegalizaciones.Columns(10).Visible = False  'Valor Carrera
        End If


        gvLegalizaciones.DataSource = oReporte.ObtenerReporteLegalizaciones(FechaIni, FechaFin, UsuarioAsignado, Articulo, BU, JobBookCodigo, TodosCampos)
        gvLegalizaciones.DataBind()
    End Sub

#End Region

#Region "DDL"

    Sub CargarBU()
        Dim oBU As New Inventario
        Me.ddlBU.DataSource = oBU.obtenerBU
        Me.ddlBU.DataValueField = "Id"
        Me.ddlBU.DataTextField = "Nombre"
        Me.ddlBU.DataBind()
        Me.ddlBU.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub
#End Region

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargarReporteLegalizaciones()
    End Sub

    Protected Sub btnBuscarUsuario_Click(sender As Object, e As EventArgs) Handles btnBuscarUsuario.Click
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        CargarGridPersonas()
        UPanelUsuarios.Update()
    End Sub

    Protected Sub ddlCentroCosto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCentroCosto.SelectedIndexChanged
        hfJobBook.Value = "0"
        If ddlCentroCosto.SelectedValue = 3 Then
            lblBU.Visible = True
            ddlBU.Visible = True
            lblJBIJBE.Visible = False
            txtJBIJBE.Visible = False
            txtJBIJBE.Text = ""
            lblNombreJBIJBE.Visible = False
            txtNombreJBIJBE.Visible = False
            txtNombreJBIJBE.Text = ""
        Else
            lblJBIJBE.Visible = True
            txtJBIJBE.Visible = True
            txtJBIJBE.Text = ""
            lblNombreJBIJBE.Visible = True
            txtNombreJBIJBE.Visible = True
            txtNombreJBIJBE.Text = ""
            lblBU.Visible = False
            ddlBU.Visible = False
            ddlBU.ClearSelection()
        End If

    End Sub

    Private Sub txtJBIJBE_TextChanged(sender As Object, e As EventArgs) Handles txtJBIJBE.TextChanged
        Dim daProyecto As New Proyecto
        Dim daTrabajo As New Trabajo
        Dim oP As PY_Proyectos_Get_Result
        Dim oT As PY_Trabajo0
        Select Case ddlCentroCosto.SelectedValue
            Case 1
                oP = daProyecto.obtenerXJobBook(txtJBIJBE.Text)
                If Not ((oP Is Nothing)) Then
                    txtNombreJBIJBE.Text = oP.Nombre
                    hfJobBook.Value = oP.id
                Else
                    txtNombreJBIJBE.Text = ""
                    hfJobBook.Value = ""
                End If

            Case 2
                oT = daTrabajo.ObtenerTrabajoXJob(txtJBIJBE.Text)
                If Not ((oT Is Nothing)) Then
                    txtNombreJBIJBE.Text = oT.NombreTrabajo
                    hfJobBook.Value = oT.id
                Else
                    txtNombreJBIJBE.Text = ""
                    hfJobBook.Value = ""
                End If
        End Select

    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        CargarReporteLegalizaciones()
        gvLegalizaciones.Visible = True
        'Actualiza los datos del gridview
        gvLegalizaciones.AllowPaging = False
        gvLegalizaciones.DataBind()

        'Crea variables en memoria para almacenar el contenido del gridview
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Crea una nueva página html y un form dentro de ella
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        'Cambie el estado del gridview para que no guarde los controles de vista
        gvLegalizaciones.EnableViewState = False
        'Quita la validación de la página 
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        'Agrega el form creado
        pagina.Controls.Add(form)
        'Agrega el gridview al form
        form.Controls.Add(gvLegalizaciones)
        'Hace un render para pasar los valores de la página al htmltextwriter que contendrá los datos del gridview
        pagina.RenderControl(htw)
        'Hace un response para descargar el control
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=Reporte_Legalizaciones.xls")
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        gvLegalizaciones.Visible = False
    End Sub

    Private Sub _AlmacenamientoDisco_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(138, UsuarioID) = False AndAlso permisos.VerificarPermisoUsuario(140, UsuarioID) = False Then
            Response.Redirect("../Inventario/RegistroArticulos.aspx")
        End If
    End Sub

End Class