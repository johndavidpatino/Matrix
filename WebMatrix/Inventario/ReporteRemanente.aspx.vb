Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Imports ClosedXML.Excel
Imports CoreProject.OP
Imports CoreProject.INV
Imports System.IO

Public Class ReporteRemanente
    Inherits System.Web.UI.Page

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
        End If

        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExportar)

    End Sub

    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Private Sub gvStock_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvStock.PageIndexChanging
        gvStock.PageIndex = e.NewPageIndex
        CargarGridStock()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

#End Region


#Region "Grillas"

    Public Sub CargarGridStock()
        gvStock.DataSource = obtenerReporteRemanente()
        gvStock.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Function obtenerReporteRemanente() As List(Of INV_ReporteRemanente_Result)
        Dim oBusqueda As New Inventario
        Dim Articulo As Int64? = Nothing
        Dim TipoProducto As Int64? = Nothing
        Dim JobBook As String = Nothing

        If Not ddlIdArticulo.SelectedValue = "-1" Then Articulo = ddlIdArticulo.SelectedValue
        If Not ddlIdTipoProducto.SelectedValue = "-1" Then TipoProducto = ddlIdTipoProducto.SelectedValue
        If Not txtBusqueda.Text = "" Then JobBook = txtBusqueda.Text

        Return oBusqueda.ObtenerReporteRemanente(Nothing, Articulo, TipoProducto, JobBook)
    End Function

    Public Sub CargarColumnas()

        If ddlIdArticulo.SelectedValue = 7 AndAlso ddlIdTipoProducto.SelectedValue = 1 Then
            gvStock.Columns(5).Visible = True  'TipoObsequio
        Else
            gvStock.Columns(5).Visible = False  'TipoObsequio

        End If

        If ddlIdArticulo.SelectedValue = 7 Then
            gvStock.Columns(3).Visible = True  'TipoProducto
            gvStock.Columns(4).Visible = True  'Producto
            gvStock.Columns(6).Visible = True  'EstadoProducto
        Else
            gvStock.Columns(3).Visible = False  'TipoProducto
            gvStock.Columns(4).Visible = False  'Producto
            gvStock.Columns(6).Visible = False  'EstadoProducto
        End If

        If ddlIdArticulo.SelectedValue = 8 Then
            gvStock.Columns(7).Visible = True  'TipoBono
        Else
            gvStock.Columns(7).Visible = False  'TipoBono
        End If

    End Sub

#End Region


#Region "Formulario"

    Sub limpiar()

        ddlIdArticulo.ClearSelection()
        ddlIdTipoProducto.ClearSelection()
        txtBusqueda.Text = ""
        gvStock.DataSource = Nothing
        gvStock.DataBind()

    End Sub

#End Region

#Region "Eventos"

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargarColumnas()
        gvStock.DataSource = obtenerReporteRemanente()
        gvStock.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlIdArticulo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlIdArticulo.SelectedIndexChanged

        If ddlIdArticulo.SelectedValue = 7 Then
            lblIdTipoProducto.Visible = True
            ddlIdTipoProducto.Visible = True
        Else
            lblIdTipoProducto.Visible = False
            ddlIdTipoProducto.Visible = False
            ddlIdTipoProducto.ClearSelection()
        End If
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)

    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        exportarExcel()
    End Sub

#Region "Exportar a Excel Listado Remanente"
    Sub exportarExcel()
        Dim wb As New XLWorkbook
        Dim oStock As List(Of INV_ReporteRemanente_Result)
        Dim titulosStock As String = "IdConsumible;Articulo;TipoProducto;Producto;TipoObsequio;EstadoProducto;TipoBono;Fecha;JobBookCodigo;JobBookNombre;Total;Disponible"
        oStock = obtenerReporteRemanente()
        Dim oExportar = (From x In oStock
                        Select x.IdConsumible, x.Articulo, x.TipoProducto, x.Producto, x.TipoObsequio, x.EstadoProducto, x.TipoBono, x.Fecha, x.JobBookCodigo, x.JobBookNombre, x.Total, x.Disponible).ToList
        Dim ws = wb.Worksheets.Add("Remanente")
        insertarNombreColumnasExcelSC(ws, titulosStock.Split(";"))
        ws.Cell(2, 1).InsertData(oExportar)
        exportarExcel(wb, "Remanente")
    End Sub

    Sub insertarNombreColumnasExcelSC(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(1, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub

    Private Sub exportarExcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""Reporte_" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

#End Region

    Private Sub _AlmacenamientoDisco_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(138, UsuarioID) = False AndAlso permisos.VerificarPermisoUsuario(140, UsuarioID) = False Then
            Response.Redirect("../Inventario/RegistroArticulos.aspx")
        End If
    End Sub

#End Region

End Class