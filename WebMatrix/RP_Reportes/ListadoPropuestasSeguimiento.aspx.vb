Imports WebMatrix.Util
Imports CoreProject
Imports ClosedXML.Excel
Imports System.IO

Public Class ListadoPropuestasSeguimiento
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarGruposUnidad()
            CargarGerencias()
            CargarEstadoPropuesta()
            'Dim permisos As New Datos.ClsPermisosUsuarios
            'Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            'If permisos.VerificarPermisoUsuario(17, UsuarioID) = False Then
            '    Response.Redirect("../Home.aspx")
            'End If
            'Me.pnlActualizacion.Visible = False
        End If

        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnImgExportar)
    End Sub


#End Region

    Protected Sub gvPropuestas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPropuestas.PageIndexChanging
        gvPropuestas.PageIndex = e.NewPageIndex
        gvPropuestas.DataSource = CargarGvpropuesta()
        gvPropuestas.DataBind()
    End Sub

#Region "Metodos"
    Sub CargarGruposUnidad()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlGrupoUnidades.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(1)
        ddlGrupoUnidades.DataValueField = "id"
        ddlGrupoUnidades.DataTextField = "GrupoUnidad"
        ddlGrupoUnidades.DataBind()
        ddlGrupoUnidades.Items.Insert(0, New ListItem With {.Text = "--Ver todo--", .Value = -1})
    End Sub

    Sub CargarUnidades()
        If ddlGrupoUnidades.SelectedValue = "-1" Or ddlGrupoUnidades.SelectedValue = "" Then
            ddlUnidades.Items.Clear()
        Else
            Dim oUnidades As New US.Unidades
            ddlUnidades.DataSource = oUnidades.ObtenerUnidadCombo(ddlGrupoUnidades.SelectedValue)
            ddlUnidades.DataValueField = "id"
            ddlUnidades.DataTextField = "Unidad"
            ddlUnidades.DataBind()
            ddlUnidades.Items.Insert(0, New ListItem With {.Text = "--Ver todo--", .Value = -1})
        End If
        CargarGerentesCuentas()
    End Sub

    Sub CargarGerencias()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlGerencias.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(2)
        ddlGerencias.DataValueField = "id"
        ddlGerencias.DataTextField = "GrupoUnidad"
        ddlGerencias.DataBind()
        ddlGerencias.Items.Insert(0, New ListItem With {.Text = "--Ver todas--", .Value = -1})
    End Sub

    Public Sub CargarEstadoPropuesta()
        Try
            Dim oAuxiliares As New Auxiliares
            Dim listaEstados = (From lestado In oAuxiliares.DevolverEstadoPropuesta()
                                Select Id = lestado.id, Estado = lestado.Estado).OrderBy(Function(e) e.Id).ToList()
            ddlEstado.DataSource = listaEstados
            ddlEstado.DataValueField = "Id"
            ddlEstado.DataTextField = "Estado"
            ddlEstado.DataBind()
            ddlEstado.Items.Insert(0, New ListItem With {.Text = "--Ver todo--", .Value = -1})
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Sub CargarGerentesCuentas()

        If ddlUnidades.SelectedValue = "-1" Or ddlUnidades.SelectedValue = "" Then
            ddlGerenteCuentas.Items.Clear()
        Else
            Dim o As New Reportes.RP_GerOpe
            ddlGerenteCuentas.DataSource = o.ObtenerUsuariosXUnidadXRol(ddlUnidades.SelectedValue, ListaRoles.GerenteCuentas)
            ddlGerenteCuentas.DataValueField = "id"
            ddlGerenteCuentas.DataTextField = "NombreCompleto"
            ddlGerenteCuentas.DataBind()
            ddlGerenteCuentas.Items.Insert(0, New ListItem With {.Text = "--Ver todo--", .Value = -1})
        End If
    End Sub

    Function CargarGvpropuesta() As List(Of REP_ListadoPropuestasSeguimiento_Result)
        Dim o As New Reportes.RP_GerOpe
        Dim estado As Integer? = Nothing
        Dim probabilidad As Integer? = Nothing
        Dim unidad As Integer? = Nothing
        Dim grupounidad As Long? = Nothing
        Dim gerentecuentas As Long? = Nothing
        Dim gerenciaop As Long? = Nothing

        If Not (ddlEstado.SelectedValue = "-1" Or ddlEstado.SelectedValue = "") Then estado = ddlEstado.SelectedValue
        If Not (ddlProbabilidad.SelectedValue = "-1" Or ddlProbabilidad.SelectedValue = "") Then probabilidad = ddlProbabilidad.SelectedValue
        If Not (ddlGrupoUnidades.SelectedValue = "-1" Or ddlGrupoUnidades.SelectedValue = "") Then unidad = ddlGrupoUnidades.SelectedValue
        If Not (ddlUnidades.SelectedValue = "-1" Or ddlUnidades.SelectedValue = "") Then grupounidad = ddlUnidades.SelectedValue
        If Not (ddlGerenteCuentas.SelectedValue = "-1" Or ddlGerenteCuentas.SelectedValue = "") Then gerentecuentas = ddlGerenteCuentas.SelectedValue
        If Not (ddlGerencias.SelectedValue = "-1" Or ddlGerencias.SelectedValue = "") Then gerenciaop = ddlGerencias.SelectedValue

        Return o.ObtenerListadoPropuestasSeguimiento(gerenciaop, estado, probabilidad, unidad, grupounidad, gerentecuentas)

    End Function
#End Region

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        btnImgExportar.Visible = True
        gvPropuestas.DataSource = CargarGvpropuesta()
        gvPropuestas.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlGrupoUnidades_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGrupoUnidades.SelectedIndexChanged
        CargarUnidades()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub ddlUnidades_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUnidades.SelectedIndexChanged
        CargarGerentesCuentas()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub gvPropuestas_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPropuestas.RowCommand
        If e.CommandName = "Actualizar" Then
            Me.hfPropuesta.Value = Me.gvPropuestas.DataKeys(CInt(e.CommandArgument))("Id")
            Dim oPropuesta As New Propuesta
            Dim info = oPropuesta.DevolverxID(hfPropuesta.Value)
            txtFechaInicioCampo.Text = info.FechaInicioCampo
            ddProbabilidadUpdate.SelectedValue = info.ProbabilidadId
            upGerenteAsignar.Update()
            btnUpdate.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End If
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim o As New Reportes.RP_GerOpe
        If Not (IsDate(txtFechaInicioCampo.Text)) Then Exit Sub
        o.ActualizarPropuestaSeguimiento(hfPropuesta.Value, txtFechaInicioCampo.Text, ddProbabilidadUpdate.SelectedValue)
        gvPropuestas.DataSource = CargarGvpropuesta()
        gvPropuestas.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnImgExportar_Click(sender As Object, e As EventArgs) Handles btnImgExportar.Click
        exportarExcel()
    End Sub

#Region "Exportar a Excel Listado Propuestas"
    Sub exportarExcel()
        Dim wb As New XLWorkbook
        Dim oPropuestas As List(Of REP_ListadoPropuestasSeguimiento_Result)
        Dim titulos As String = "No;Titulo;Cliente;FechaEnvio;FechaCampoEstimada;Muestra;Probabilidad;Estado;GerenteCuentas;Grupo"
        oPropuestas = CargarGvpropuesta()
        Dim oExportar = (From x In oPropuestas
                        Select New REP_ListadoPropuestasSeguimiento_Result With {.Id = x.Id, .Titulo = x.Titulo.Replace(Chr(11), ""), .Cliente = x.Cliente, .FechaEnvio = x.FechaEnvio, .FechaInicioCampo = x.FechaInicioCampo, .Muestra = x.Muestra, .Probabilidad = x.Probabilidad, .Estado = x.Estado, .GerenteCuentas = x.GerenteCuentas, .Grupo = x.Grupo}).ToList
        Dim ws = wb.Worksheets.Add("Propuestas")
        insertarNombreColumnasExcelSC(ws, titulos.Split(";"))
        ws.Cell(2, 1).InsertData(oExportar)
        exportarExcel(wb, "Propuestas")
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

End Class