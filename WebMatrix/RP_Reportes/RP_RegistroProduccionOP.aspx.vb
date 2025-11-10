Imports ClosedXML.Excel
Imports CoreProject
Public Class RP_RegistroProduccionOP
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
#Region "Eventos"
    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        cargarGrilla()
        gvDatos.Visible = True
        btnImgExportarInforme0.Visible = True
        gvInformeConsolidadoEjecucion.Visible = False
        btnImgExportarInforme1.Visible = False
    End Sub
    Private Sub RP_RegistroProduccionOP_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsPostBack = False Then
            CargarUnidades(Nothing)
            tamanoPagina = 10
            paginaActual = 1
        End If
    End Sub
    Protected Sub btnInfConsEjec_Click(sender As Object, e As EventArgs) Handles btnInfConsEjec.Click
        cargarGrillaInformeGeneral()
        gvDatos.Visible = False
        btnImgExportarInforme0.Visible = False
        gvInformeConsolidadoEjecucion.Visible = True
        btnImgExportarInforme1.Visible = True
    End Sub

    Protected Sub btnImgExportarInforme0_Click(sender As Object, e As ImageClickEventArgs) Handles btnImgExportarInforme0.Click
        exportarExcel()
    End Sub
    Protected Sub btnImgExportarInforme1_Click(sender As Object, e As ImageClickEventArgs) Handles btnImgExportarInforme1.Click
        exportarExcelInformeConsolidado()
    End Sub
#Region "Grilla"

    Private Sub gvDatos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvDatos.PageIndexChanging

    End Sub

    Protected Sub gvDatos_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDatos.PreRender
        Dim gv As System.Web.UI.WebControls.GridView = CType(sender, System.Web.UI.WebControls.GridView)
        If Not gv Is Nothing Then
            Dim PagerRow As GridViewRow = gv.BottomPagerRow
            If Not PagerRow Is Nothing Then
                PagerRow.Visible = True
            End If
        End If
    End Sub
    Private Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
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
        cargarGrilla()
    End Sub
    Private Sub gvDatos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDatos.RowDataBound
        If e.Row.RowType = DataControlRowType.Pager Then
            CType(e.Row.FindControl("lblPaginaActual"), Label).Text = paginaActual.ToString
            CType(e.Row.FindControl("lblCantidadPaginas"), Label).Text = cantidadPaginas.ToString
        End If
    End Sub
#End Region

#Region "GrillaInformeGeneral"
    Private Sub gvInformeConsolidadoEjecucion_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvInformeConsolidadoEjecucion.RowCommand
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
        cargarGrillaInformeGeneral()
    End Sub
    Private Sub gvInformeConsolidadoEjecucion_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvInformeConsolidadoEjecucion.PageIndexChanging

    End Sub
    Protected Sub gvInformeConsolidadoEjecucion_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvInformeConsolidadoEjecucion.PreRender
        Dim gv As System.Web.UI.WebControls.GridView = CType(sender, System.Web.UI.WebControls.GridView)
        If Not gv Is Nothing Then
            Dim PagerRow As GridViewRow = gv.BottomPagerRow
            If Not PagerRow Is Nothing Then
                PagerRow.Visible = True
            End If
        End If
    End Sub
    Private Sub gvInformeConsolidadoEjecucion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvInformeConsolidadoEjecucion.RowDataBound
        If e.Row.RowType = DataControlRowType.Pager Then
            CType(e.Row.FindControl("lblPaginaActual"), Label).Text = paginaActual.ToString
            CType(e.Row.FindControl("lblCantidadPaginas"), Label).Text = cantidadPaginas.ToString
        End If
    End Sub
#End Region
#End Region

#Region "Metodos"
    Sub exportarExcelInformeConsolidado()
        Dim wb As New XLWorkbook
        Dim o As New RecordProduccion
        Dim oInforme As List(Of REP_InformeConsolidadoEjecucion_Result)
        oInforme = o.REP_InformeConsolidadoEjecucion(txtFechaInicio.Text, txtFechaFin.Text, ddlAreas.SelectedValue)
        Dim titulosProduccion As String = "PersonaId;Nombres;DiasHabiles;DiasReportados;TotalHoras;PromedioHoras;HorasProductivas;%HorasProductivas;NorasNoProductivas;%HorasNoProductivas;HorasOtros;%HorasOtros;HorasReproceso;%HorasReproceso"
        oInforme = o.REP_InformeConsolidadoEjecucion(txtFechaInicio.Text, txtFechaFin.Text, ddlAreas.SelectedValue)

        Dim oExportar = (From x In oInforme
                        Select x.PersonaId, x.Nombres, x.DiasHabiles, x.DiasReportados, x.TotalHoras, x.PromedioHoras, x.HorasProductivas, x.PorcentajeHorasProductivas,
                        x.HorasNoProductivas, x.PorcentajeHorasNoProductivas, x.HorasOtros, x.PorcentajeHorasOtros,
                        x.HorasReproceso, x.PorcentajeHorasReproceso).ToList


        Dim ws = wb.Worksheets.Add("InfConsolidadoEjecucion")

        ws.Cell(1, 1).Value = "Fecha Inicio"
        ws.Cell(1, 2).Value = "Fecha Fin"
        ws.Cell(1, 3).Value = "Area"

        ws.Cell(2, 1).Value = txtFechaInicio.Text
        ws.Cell(2, 2).Value = txtFechaFin.Text
        ws.Cell(2, 3).Value = ddlAreas.SelectedItem.Text

        insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 4)


        ws.Cell(5, 1).InsertData(oExportar)
        exportarExcel(wb, "InfConsolidadoEjecucion")
    End Sub
    Sub exportarExcel()
        Dim wb As New XLWorkbook
        Dim oProduccion As List(Of OP_Produccion_Get_Result)
        Dim titulosProduccion As String = "id;FechaRegistro;Fecha;PersonaId;PersonaNombre;Cargo;Area;Actividad;SubActividad;Reproceso;TipoReproceso;Aplicativo;Tipo;Unidad;GerenciaOP;TrabajoId;JobBook;Nombre;HoraInicio;HoraFin;DiferenciaMinutos;CantidadGeneral;CantidadEfectivas;Observacion;CantVarsScript;CantVarsExport"
        oProduccion = obtenerRegistrosProduccion()

        Dim oExportar = (From x In oProduccion
                        Select x.id, x.FechaRegistro, x.Fecha, x.PersonaId, x.PersonaNombre, x.Cargo, x.AreaNombre, x.ActividadNombre, x.SubActividadNombre, x.EsReproceso, x.NombreTipoReproceso, x.NombreTipoAplicativoProceso,
                        x.TipoNombre, x.UnidadNombre, x.GerenciaOP, x.TrabajoId, x.JobBook, x.Nombre, x.HoraInicio, x.HoraFin, x.DiferenciaMinutos, x.CantidadGeneral, x.CantidadEfectivas,
                        x.Observacion, x.CantVarsScript, x.CantVarsExport).ToList


        Dim ws = wb.Worksheets.Add("Registro")
        insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)
        ws.Cell(2, 1).InsertData(oExportar)
        exportarExcel(wb, "Registro")
    End Sub
    Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String, ByVal fila As Integer)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(fila, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub
    Function obtenerRegistrosProduccion() As List(Of OP_Produccion_Get_Result)
        Dim oRecordProduccion As New RecordProduccion
        Return oRecordProduccion.obtener(If(String.IsNullOrEmpty(txtFechaInicio.Text), CType(Nothing, Date?), CType(txtFechaInicio.Text, Date?)), If(String.IsNullOrEmpty(txtFechaFin.Text), CType(Nothing, Date?), CType(txtFechaFin.Text, Date?)), Nothing, Nothing, If(ddlAreas.SelectedValue = -1, CType(Nothing, Integer?), CType(ddlAreas.SelectedValue, Integer?)))
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
    Sub cargarGrilla()
        Dim oProduccion As List(Of OP_Produccion_Get_Result)
        oProduccion = obtenerRegistrosProduccion()
        cantidadRegistros = oProduccion.Count
        cantidadPaginas = CInt(Decimal.Ceiling(CDec(cantidadRegistros) / tamanoPagina))
        If paginaActual > cantidadPaginas Then
            paginaActual = 1
        End If
        gvDatos.DataSource = oProduccion
        gvDatos.DataBind()
    End Sub
    Sub cargarGrillaInformeGeneral()
        Dim o As New RecordProduccion
        Dim oInforme As List(Of REP_InformeConsolidadoEjecucion_Result)
        oInforme = o.REP_InformeConsolidadoEjecucion(txtFechaInicio.Text, txtFechaFin.Text, ddlAreas.SelectedValue)
        cantidadRegistros = oInforme.Count
        cantidadPaginas = CInt(Decimal.Ceiling(CDec(cantidadRegistros) / tamanoPagina))
        If paginaActual > cantidadPaginas Then
            paginaActual = 1
        End If
        gvInformeConsolidadoEjecucion.DataSource = oInforme
        gvInformeConsolidadoEjecucion.PageIndex = paginaActual - 1
        gvInformeConsolidadoEjecucion.DataBind()
    End Sub
    Sub CargarUnidades(identificacion As Int64?)
        Dim o As New RecordProduccion
        Me.ddlAreas.DataSource = o.ObtenerUnidades(identificacion)
        Me.ddlAreas.DataTextField = "Unidad"
        Me.ddlAreas.DataValueField = "id"
        Me.ddlAreas.DataBind()
        Me.ddlAreas.Items.Insert(0, New ListItem With {.Text = "--Todos--", .Value = "0"})
    End Sub
#End Region
End Class