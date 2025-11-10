Imports CoreProject
Imports WebMatrix.Util
Imports ClosedXML
Imports ClosedXML.Excel
Public Class ListadoTrabajos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btbDescargar)
    End Sub

    Protected Sub btbDescargar_Click(sender As Object, e As EventArgs) Handles btbDescargar.Click
        Exportar()
        ShowNotification("Descarga Finalizada", ShowNotifications.InfoNotification)
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)

    End Sub
#Region "Metodos"

    Protected Sub Exportar()
        Dim excel As New List(Of Array)
        Dim Titulos As String = "id;ProyectoId;OP_MetodologiaId;PresupuestoId;NombreTrabajo;Muestra;FechaTentativaInicioCampo;FechaTentativaFinalizacion;COE;Unidad;JobBook;TipoRecoleccionId;Estado;IdPropuesta;Alternativa;MetCodigo;Fase;NoMedicion;Duracion;FechaCreacionPresupuesto"
        Dim DynamicColNames() As String
        Dim lstCambios As List(Of CC_ListadoTrabajosActivosDescargar_Result)
        Dim workbook = New XLWorkbook()
        Dim worksheet = workbook.Worksheets.Add("ListadoTrabajos")

        DynamicColNames = Titulos.Split(CChar(";"))
        excel.Add(DynamicColNames)
        Dim op As New CC_FinzOpe
        lstCambios = op.CC_ListadoTrabajosActivosDescargar().ToList
        For Each x In lstCambios
            excel.Add((x.id & ";" & x.ProyectoId & ";" & x.OP_MetodologiaId & ";" & x.PresupuestoId & ";" & x.NombreTrabajo & ";" & x.Muestra & ";" & x.FechaTentativaInicioCampo & ";" & x.FechaTentativaFinalizacion & ";" & x.COE & ";" & x.Unidad & ";" & x.JobBook & ";" & x.TipoRecoleccionId & ";" & x.Estado & ";" & x.IdPropuesta & ";" & x.Alternativa & ";" & x.MetCodigo & ";" & x.Fase & ";" & x.NoMedicion & ";" & x.Duracion & ";" & x.FechaCreacion).Split(CChar(";")).ToArray())
        Next
        worksheet.Cell("A1").Value = excel
        Crearexcel(workbook, "ListadoTrabajos -" & Now.Year & "-" & Now.Month & "-" & Now.Day)
    End Sub
    Private Sub Crearexcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)

            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

#End Region
End Class