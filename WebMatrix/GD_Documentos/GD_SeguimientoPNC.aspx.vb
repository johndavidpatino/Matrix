Imports ClosedXML.Excel
Imports CoreProject

Public Class GD_SeguimientoPNC
    Inherits System.Web.UI.Page
#Region "Enumerados"
#End Region
#Region "Propiedades"
    Private _gerenteProyectosId As Int64
    Public Property gerenteProyectosId() As Int64
        Get
            Return _gerenteProyectosId
        End Get
        Set(ByVal value As Int64)
            _gerenteProyectosId = value
        End Set
    End Property
    Private _usuarioAsignadoId As Int64
    Public Property usuarioAsignadoId() As Int64
        Get
            Return _usuarioAsignadoId
        End Get
        Set(ByVal value As Int64)
            _usuarioAsignadoId = value
        End Set
    End Property
    Private _unidadId As Int64
    Public Property unidadId() As Int64
        Get
            Return _unidadId
        End Get
        Set(ByVal value As Int64)
            _unidadId = value
        End Set
    End Property
    Private _gerenteOperacionesId As Int64
    Public Property gerenteOperacionesId() As Int64
        Get
            Return _gerenteOperacionesId
        End Get
        Set(ByVal value As Int64)
            _gerenteOperacionesId = value
        End Set
    End Property
    Private _COEId As Int64
    Public Property COEId() As Int64
        Get
            Return _COEId
        End Get
        Set(ByVal value As Int64)
            _COEId = value
        End Set
    End Property
    Private _procesoId As Int64
    Public Property procesoId() As Int64
        Get
            Return _procesoId
        End Get
        Set(ByVal value As Int64)
            _procesoId = value
        End Set
    End Property
    Private _permiso As Int64
    Public Property permiso() As Int64
        Get
            Return _permiso
        End Get
        Set(ByVal value As Int64)
            _permiso = value
        End Set
    End Property

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


#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim IdUsuario As Long = Session("IDUsuario").ToString
        Dim permisos As New Datos.ClsPermisosUsuarios
        'If Not Request.QueryString("Permiso") Is Nothing Then
        ' Integer.TryParse(Request.QueryString("Permiso"), permiso)
        ' End If

        'If permisos.VerificarPermisoUsuario(permiso, IdUsuario) = False Then
        'Response.Redirect("../PY_Proyectos/Default.aspx")
        'End If

        If IsPostBack = False Then
            tamanoPagina = 10
            paginaActual = 1
        End If
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExportar)
    End Sub
    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        cargarListaPNC()
    End Sub
    Private Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        exportarExcel(If(ddlEstado.SelectedValue > -1, CType(ddlEstado.SelectedValue, Byte?), CType(Nothing, Byte?)))
    End Sub
#Region "Grilla"

    Private Sub gvLista_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvLista.PageIndexChanging

    End Sub

    Protected Sub gvLista_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvLista.PreRender
        Dim gv As System.Web.UI.WebControls.GridView = CType(sender, System.Web.UI.WebControls.GridView)
        If Not gv Is Nothing Then
            Dim PagerRow As GridViewRow = gv.BottomPagerRow
            If Not PagerRow Is Nothing Then
                PagerRow.Visible = True
            End If
        End If
    End Sub
    Private Sub gvLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvLista.RowCommand
        Select Case e.CommandArgument
            Case "First"
                paginaActual = 1
            Case "Prev"
                If paginaActual > 1 Then
                    paginaActual -= 1
                End If
            Case "Next"
                If paginaActual < cantidadPaginas Then
                    paginaActual += 1
                End If
            Case "Last"
                If paginaActual < cantidadPaginas Then
                    paginaActual = cantidadPaginas
                End If
        End Select
        If paginaActual > cantidadPaginas Then
            paginaActual = 1
        End If
        cargarListaPNC()
    End Sub
    Private Sub gvLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLista.RowDataBound
        If e.Row.RowType = DataControlRowType.Pager Then
            CType(e.Row.FindControl("lblPaginaActual"), Label).Text = paginaActual.ToString
            CType(e.Row.FindControl("lblCantidadPaginas"), Label).Text = cantidadPaginas.ToString
        End If
    End Sub
#End Region
#End Region
#Region "Metodos"
    Sub cargarListaPNC()
        Dim oPNC As List(Of PNC_Seguimiento_Get_Result)
        oPNC = obtenerListaPNC()
        cantidadRegistros = oPNC.Count
        cantidadPaginas = CInt(Decimal.Ceiling(CDec(cantidadRegistros) / tamanoPagina))
        If paginaActual > cantidadPaginas Then
            paginaActual = 1
        End If
        gvLista.DataSource = oPNC
        gvLista.DataBind()
        upGVLista.Update()
    End Sub
    Sub exportarExcel(ByVal estado As Byte?)
        Dim wb As New XLWorkbook
        Dim oPNC As List(Of PNC_Seguimiento_Get_Result)
        Dim oPNCC As List(Of PNC_ProductoNoConformeCausas_Get_Result)
        Dim titulosPNC As String = "Id;IdEstudio;NombreEstudio;IdTrabajo;NombreTrabajo;JobBook;FechaReclamo;IdReporta;Reporta;IdUnidad;Unidad;IdClienteExterno;Cliente;IdFuenteReclamo;FuenteReclamo;IdCategoria;Categoria;IdTarea;Tarea;Descripcion;CantidadCausas;CantidadAcciones;Estado"
        oPNCC = obtenerListaPNCCausas()
        oPNC = obtenerListaPNC()
        Dim ws = wb.Worksheets.Add("PNC")
        insertarNombreColumnasExcel(ws, titulosPNC.Split(";"))
        Dim ws2 = wb.Worksheets.Add("PNCCausas")
        insertarNombreColumnasExcel(ws2, "Id;IdPNC;CausaRaiz;Estado".Split(";"))
        ws.Cell(2, 1).InsertData(oPNC)
        ws2.Cell(2, 1).InsertData(oPNCC)
        exportarExcel(wb, "PNC")
    End Sub
    Function obtenerListaPNC() As List(Of PNC_Seguimiento_Get_Result)
        Dim oPNCClass As New PNCClass
        Return oPNCClass.obtenerPNCXEstado(If(ddlEstado.SelectedValue > -1, CType(ddlEstado.SelectedValue, Byte?), CType(Nothing, Byte?)))
    End Function
    Function obtenerListaPNCCausas() As List(Of PNC_ProductoNoConformeCausas_Get_Result)
        Dim oPNCClass As New PNCClass
        Return oPNCClass.obtenerLstPNCCausasXEstado(If(ddlEstado.SelectedValue > -1, CType(ddlEstado.SelectedValue, Byte?), CType(Nothing, Byte?)))
    End Function
    Private Sub exportarExcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""Control_" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub
    Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(1, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub
#End Region


End Class