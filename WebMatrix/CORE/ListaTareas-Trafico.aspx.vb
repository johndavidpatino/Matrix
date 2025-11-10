Imports ClosedXML.Excel
Imports CoreProject

Public Class ListaTareas_Trafico
    Inherits System.Web.UI.Page
#Region "Enumerados"
    Enum FiltrarTareasPor
        Unidad = 1
        GerenteProyectos = 2
        GerenteOPeraciones = 3
        COE = 4
        CordinadorOperaciones = 4
        Usuario = 5
    End Enum
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
        If Not Request.QueryString("Permiso") Is Nothing Then
            Integer.TryParse(Request.QueryString("Permiso"), permiso)
        End If

        If permisos.VerificarPermisoUsuario(permiso, IdUsuario) = False Then
            Response.Redirect("../PY_Proyectos/Default.aspx")
        End If

        Dim TipoFiltro As FiltrarTareasPor

        If Not Request.QueryString("FiltrarTareasPor") Is Nothing Then
            Integer.TryParse(Request.QueryString("FiltrarTareasPor"), TipoFiltro)
        End If
        If Not Request.QueryString("ProcesoId") Is Nothing Then
            Integer.TryParse(Request.QueryString("ProcesoId"), procesoId)
        End If
        Select Case TipoFiltro
            Case FiltrarTareasPor.GerenteProyectos
                gerenteProyectosId = IdUsuario
            Case FiltrarTareasPor.GerenteOPeraciones
                gerenteOperacionesId = IdUsuario
            Case FiltrarTareasPor.COE
                COEId = IdUsuario
            Case FiltrarTareasPor.CordinadorOperaciones
                procesoId = procesoId
            Case FiltrarTareasPor.Usuario
                usuarioAsignadoId = IdUsuario
        End Select

        If IsPostBack = False Then
            tamanoPagina = 30
            paginaActual = 1
            cargarProcesos()
            cargarGerente()
            cargarCoe()
            cargarUnidades()
            cargarEstadosProcesados()
            cargarListaTareas()
        End If

    End Sub
    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFiltrar.Click
        cargarListaTareas()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        txtNombreTrabajo.Text = ""
        ddlProceso.SelectedValue = -1
        ddlGerente.SelectedValue = -1
        ddlCoe.SelectedValue = -1
        ddlAno.SelectedValue = Date.Now.Year
        ddlMes.SelectedValue = -1
        ddlUnidad.SelectedValue = -1
        ddlEstado.SelectedValue = -1
        paginaActual = 1
        cargarListaTareas()
    End Sub

    Private Sub ddlAno_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAno.SelectedIndexChanged
        paginaActual = 1
        cargarListaTareas()
    End Sub

    Private Sub ddlCoe_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCoe.SelectedIndexChanged
        paginaActual = 1
        cargarListaTareas()
    End Sub

    Private Sub ddlEstado_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEstado.SelectedIndexChanged
        paginaActual = 1
        cargarListaTareas()
    End Sub

    Private Sub ddlGerente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGerente.SelectedIndexChanged
        paginaActual = 1
        cargarListaTareas()
    End Sub

    Private Sub ddlMes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMes.SelectedIndexChanged
        paginaActual = 1
        cargarListaTareas()
    End Sub

    Private Sub ddlProceso_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProceso.SelectedIndexChanged
        paginaActual = 1
        cargarListaTareas()
    End Sub

    Private Sub ddlUnidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUnidad.SelectedIndexChanged
        paginaActual = 1
        cargarListaTareas()
    End Sub

    Private Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        exportarExcel()
    End Sub
#Region "Grilla"

    Private Sub gvLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvLista.PageIndexChanging

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
        cargarListaTareas()
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
    Sub cargarListaTareas()
        gvLista.DataSource = getlistaTareas(False)
        gvLista.DataBind()
    End Sub

    Function getlistaTareas(ByVal Todas As Boolean) As List(Of CORE_TrabajosTareas_Get_Result)
        Dim oWorkFlow As New WorkFlow
        Dim coeid = If(obtenerIdCoeSeleccionados() = 0, Nothing, obtenerIdCoeSeleccionados())
        Dim gerenteid = If(obtenerIdGerenteSeleccionados() = 0, Nothing, obtenerIdGerenteSeleccionados())
        Dim procesoid = If(obtenerIdProcesoSeleccionado() = 0, Nothing, obtenerIdProcesoSeleccionado())
        Dim unidadesid = obtenerIdsUnidadesSeleccionadas()
        Dim estadoactual = If(ddlEstado.SelectedValue > 0, CType(ddlEstado.SelectedValue, Int64?), CType(Nothing, Int64?))
        Dim ano = obtenerAnoSeleccionado()
        Dim mes = If(obtenerMesSeleccionado() = 0, Nothing, obtenerMesSeleccionado())

        cantidadRegistros = oWorkFlow.obtenerCantidadListaTrabajosTareas(coeid, gerenteid, procesoid, unidadesid, txtNombreTrabajo.Text, estadoactual, ano, mes)

        Dim listaTareas As New List(Of CORE_TrabajosTareas_Get_Result)
        If Todas Then
            listaTareas = oWorkFlow.obtenerListaTrabajosTareas(coeid, gerenteid, procesoid, unidadesid, txtNombreTrabajo.Text, estadoactual, Nothing, Nothing, ano, mes)
        Else
            cantidadPaginas = CInt(Decimal.Ceiling(CDec(cantidadRegistros) / tamanoPagina))
            If paginaActual > cantidadPaginas Then
                paginaActual = 1
            End If
            listaTareas = oWorkFlow.obtenerListaTrabajosTareas(coeid, gerenteid, procesoid, unidadesid, txtNombreTrabajo.Text, estadoactual, paginaActual, tamanoPagina, ano, mes)
        End If

        Return listaTareas
    End Function

    Sub cargarProcesos()
        Dim oUnidades As New CoreProject.Unidades
        Dim unidades = oUnidades.obtenerTodas.Where(Function(x) x.id <> 7 And x.id <> 12 And x.id <> 15).ToList

        ddlProceso.DataSource = unidades
        ddlProceso.DataTextField = "Unidad"
        ddlProceso.DataValueField = "Id"
        ddlProceso.DataBind()
        ddlProceso.Items.Insert(0, New ListItem With {.Text = "--Todas--", .Value = -1})
    End Sub
    Sub cargarUnidades()
        Dim oUnidades As New US.Unidades
        Dim lstUnidades As New List(Of US_Unidades)
        lstUnidades = oUnidades.ObtenerUnidadesTrafico()

        ddlUnidad.DataSource = lstUnidades
        ddlUnidad.DataTextField = "Unidad"
        ddlUnidad.DataValueField = "Id"
        ddlUnidad.DataBind()
        ddlUnidad.Items.Insert(0, New ListItem With {.Text = "--Todas--", .Value = -1})
    End Sub
    Sub cargarGerente()
        Dim oUsuarios As New US.Usuarios
        Dim gerentes = oUsuarios.UsuariosxRol(6).OrderBy(Function(x) x.Usuario)

        ddlGerente.DataSource = gerentes
        ddlGerente.DataTextField = "Usuario"
        ddlGerente.DataValueField = "Id"
        ddlGerente.DataBind()
        ddlGerente.Items.Insert(0, New ListItem With {.Text = "--Todos--", .Value = -1})
    End Sub

    Sub cargarCoe()
        Dim oUsuarios As New US.Usuarios
        Dim COES = oUsuarios.UsuariosxRol(10).OrderBy(Function(x) x.Usuario)

        ddlCoe.DataSource = COES
        ddlCoe.DataTextField = "Usuario"
        ddlCoe.DataValueField = "Id"
        ddlCoe.DataBind()
        ddlCoe.Items.Insert(0, New ListItem With {.Text = "--Todos--", .Value = -1})
    End Sub

    Sub cargarEstadosProcesados()
        Dim oWorkFlow As New WorkFlow
        Dim lst As List(Of String)
        lst = oWorkFlow.obtenerEstadosProcesados()
        ddlEstado.Items.AddRange((From x In lst Select New ListItem With {.Value = x.Substring(x.IndexOf("-") + 1), .Text = x.Substring(0, x.IndexOf("-"))}).ToArray)
        ddlEstado.Items.Insert(0, New ListItem With {.Text = "--Todas--", .Value = -1})
    End Sub
    Function obtenerIdsUnidadesSeleccionadas()
        Dim idsUnidadesSeleccionadas As String
        If ddlUnidad.SelectedValue > -1 Then
            idsUnidadesSeleccionadas = ddlUnidad.SelectedValue.ToString
        Else
            idsUnidadesSeleccionadas = String.Join(",", (From x As ListItem In ddlUnidad.Items Where x.Value > -1 Select x.Value).ToArray)
        End If
        Return idsUnidadesSeleccionadas
    End Function
    Function obtenerAnoSeleccionado() As Int64?
        Dim AnoSeleccionado As Int64
        If ddlAno.SelectedValue > -1 Then
            AnoSeleccionado = ddlAno.SelectedValue
        Else
            AnoSeleccionado = Date.Now.Year
        End If
        Return Convert.ToInt64(AnoSeleccionado)
    End Function
    Function obtenerMesSeleccionado() As Int64?
        Dim MesSeleccionado As Int64
        If ddlMes.SelectedValue > -1 Then
            MesSeleccionado = ddlMes.SelectedValue
        Else
            MesSeleccionado = 0
        End If
        Return Convert.ToInt64(MesSeleccionado)
    End Function
    Function obtenerIdGerenteSeleccionados() As Int64?
        Dim gpId As Int64?
        If ddlGerente.SelectedValue > -1 Then
            gpId = CType(ddlGerente.SelectedValue, Int64?)
        Else
            gpId = 0
        End If
        Return gpId
    End Function
    Function obtenerIdCoeSeleccionados() As Int64?
        Dim coeId As Int64?
        If ddlCoe.SelectedValue > -1 Then
            coeId = CType(ddlCoe.SelectedValue, Int64?)
        Else
            coeId = 0
        End If
        Return coeId
    End Function
    Function obtenerIdProcesoSeleccionado() As Int64?
        Dim prId As Int64?
        If procesoId > 0 Then
            prId = procesoId
        ElseIf ddlProceso.SelectedValue > -1 Then
            prId = CType(ddlProceso.SelectedValue, Int64?)
        Else
            prId = 0
        End If
        Return prId
    End Function

    Sub exportarExcel()
        Dim wb As New XLWorkbook
        Dim oLstTareas As List(Of CORE_TrabajosTareas_Get_Result)
        Dim titulosProduccion As String = "Id;NombreTrabajo;JobBook;Proceso;Unidad;GerenteProyectos;IdGerenteProyectos;COE;IdCOE;Tarea;FIniP;FFinP;FIniR;FFinR;Estado;Ano;Mes;FechaRegistro;txtEstadoActual;codEstadoActual;Posicion;"

        Dim ws = wb.Worksheets.Add("ListaTareas")
        insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

        oLstTareas = getlistaTareas(True)
        ws.Cell(2, 1).InsertData(oLstTareas)

        exportarExcel(wb, "ListaTareas")
    End Sub
    Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String, ByVal fila As Integer)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(fila, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub
    Private Sub exportarExcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=" & name & ".xlsx")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub
#End Region
End Class