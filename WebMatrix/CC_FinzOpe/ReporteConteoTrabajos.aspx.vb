Imports CoreProject
Imports WebMatrix.Util
Imports ClosedXML.Excel

Public Class ReporteConteoTrabajos
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Dim eTrabajoOP As New OP_TrabajoConfiguracion
    Private _IDUsuario As Int64
    Public Property IDUsuario() As Int64
        Get
            Return _IDUsuario
        End Get
        Set(ByVal value As Int64)
            _IDUsuario = value
        End Set
    End Property
#End Region



#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(100, UsuarioID) = False Then
            Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
        End If

        If Not IsPostBack Then
            lbtnVolver.PostBackUrl = "~/FI_AdministrativoFinanciero/Default.aspx"
        End If

        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnImgExportar)
    End Sub

    Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
        gvTrabajos.PageIndex = e.NewPageIndex
        CargarReporteConteo()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Sub CargarReporteConteo()
        Dim oTrabajo As New GestionTrabajosFin
        Dim FechaIni As Date? = Nothing
        Dim FechaFin As Date? = Nothing

        FechaIni = If(txtfechainicio.Text = "", CType(Nothing, Date?), CType(txtfechainicio.Text, Date?))
        FechaFin = If(txtfechafin.Text = "", CType(Nothing, Date?), CType(txtfechafin.Text, Date?))

        gvTrabajos.DataSource = oTrabajo.ReporteConteoTrabajos(FechaIni, FechaFin)
        gvTrabajos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        CargarReporteConteo()
    End Sub

    Sub exportarExcel()
        Dim wb As New XLWorkbook
        Dim oLstConteoTrabajos As List(Of REP_ConteoTrabajos_Result)
		Dim titulosProduccion As String = "IdTrabajo;JobBook;NombreTrabajo;Muestra;Metodología;Unidad;COEAsignado;GerenciaOperativa;FechaTentativaInicioCampo;FechaTentativaFinalizacion;Estado;Conteo;DuracionPropuesta;CerradasPropuesta;CerradasMultiplesPropuesta;AbiertasPropuesta;AbiertasMultiplesPropuesta;OtrosPropuesta;DemograficosPropuesta;DuracionReal;CerradasReal;CerradasMultiplesReal;AbiertasReal;AbiertasMultiplesReal;OtrosReal;DemograficosReal;ObservacionReal;DuracionDiferencia;CerradasDiferencia;CerradasMultiplesDiferencia;AbiertasDiferencia;AbiertasMultiplesDiferencia;OtrosDiferencia;DemograficosDiferencia;Fecha Conteo;Usuario Conteo"

		Dim ws = wb.Worksheets.Add("ConteoTrabajos")
        insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

        oLstConteoTrabajos = obtenerReporteConteo()
        ws.Cell(2, 1).InsertData(oLstConteoTrabajos)

        exportarExcel(wb, "ConteoTrabajos")
    End Sub
    Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String, ByVal fila As Integer)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(fila, columna + 1).Value = nombresColumnas(columna)
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

    Function obtenerReporteConteo() As List(Of REP_ConteoTrabajos_Result)
        Dim oTrabajo As New GestionTrabajosFin
        Dim FechaIni As Date? = If(String.IsNullOrEmpty(txtfechainicio.Text), CType(Nothing, Date?), CType(txtfechainicio.Text, Date?))
        Dim FechaFin As Date? = If(String.IsNullOrEmpty(txtfechafin.Text), CType(Nothing, Date?), CType(txtfechafin.Text, Date?))
        Return oTrabajo.ReporteConteoTrabajos(FechaIni, FechaFin)
    End Function

    Protected Sub btnImgExportar_Click(sender As Object, e As ImageClickEventArgs) Handles btnImgExportar.Click
        exportarExcel()
    End Sub
    

#End Region


    
End Class