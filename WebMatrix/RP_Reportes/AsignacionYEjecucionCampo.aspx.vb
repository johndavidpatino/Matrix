Imports WebMatrix.Util
Imports CoreProject
Imports System.IO
Imports ClosedXML.Excel

Public Class AsignacionYEjecucionCampo
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarGerencias()
        End If
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExportar)
    End Sub


#End Region

    Sub CargarGerencias()
        Dim oGruposUnidad As New US.GrupoUnidad
        chbGerencias.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(US.GrupoUnidad.ETiposGruposUnidad.Operativa)
        chbGerencias.DataValueField = "id"
        chbGerencias.DataTextField = "GrupoUnidad"
        chbGerencias.DataBind()
    End Sub

    Protected Sub btnMostrar_Click(sender As Object, e As EventArgs) Handles btnMostrar.Click
        cargarGrillaDatos()
    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        Dim data As New DataTable
        data = obtenerReporteEjecucionCampo()
        exportarExcel(data)
    End Sub

    Private Sub gvDatos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        cargarGrillaDatos()
    End Sub
    Function obtenerReporteEjecucionCampo() As DataTable
        Dim daCampo As New Campo
        Dim dt As New DataTable

        Dim o = chbGerencias.Items

        dt = daCampo.obtenerReporteEjecucionCampo(txtFechaInicio.Text, txtFechaTerminacion.Text, obtenerGerenciasSeleccionadas)
        Return dt
    End Function

    Sub cargarGrillaDatos()
        gvDatos.DataSource = obtenerReporteEjecucionCampo()
        Me.gvDatos.DataBind()
    End Sub

    Function obtenerGerenciasSeleccionadas() As String
        Dim gerenciasSeleccionadas As String = ""
        For Each item As ListItem In chbGerencias.Items
            If item.Selected Then
                gerenciasSeleccionadas &= item.Value & ","
            End If
        Next
        If gerenciasSeleccionadas.Trim <> "" Then
            gerenciasSeleccionadas = gerenciasSeleccionadas.Substring(0, gerenciasSeleccionadas.Length - 1)
        End If

        Return gerenciasSeleccionadas
    End Function

    Sub exportarExcel(ByVal data As DataTable)
        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add("AsigEjecCampo")
        ws.Cell(1, 1).InsertTable(data)
        exportarExcel(wb, "AsigEjecCampo")
    End Sub
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
End Class