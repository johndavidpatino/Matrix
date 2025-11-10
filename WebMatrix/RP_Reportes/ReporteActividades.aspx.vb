Imports CoreProject.CC_FinzOpe
Imports WebMatrix.Util
Imports CoreProject
Imports ClosedXML.Excel

Public Class ReporteActividades
    Inherits System.Web.UI.Page
    Dim op As New ProcesosInternos

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Actividades()
        End If
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExportar)
    End Sub
    Sub Actividades()
        ddlactividad.DataSource = op.ActividadesIq
        ddlactividad.DataValueField = "Id"
        ddlactividad.DataTextField = "ActNombre"
        ddlactividad.DataBind()
        ddlactividad.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})

    End Sub

    Sub ActiviadadesTrabajo(ByVal TrabajoId As Int16?, ByVal ActCodigo As Int16?)
        GvActividades.DataSource = op.ObtenerActiviadesxTrabajo(TrabajoId, ActCodigo)
        GvActividades.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub btnbuscar_Click(sender As Object, e As EventArgs) Handles btnbuscar.Click
        ActiviadadesTrabajo(If(String.IsNullOrEmpty(txtidtrabajo.Text), CType(Nothing, Int16?), CType(txtidtrabajo.Text, Int16?)), If(ddlactividad.SelectedValue = -1, CType(Nothing, Int16?), CType(ddlactividad.SelectedValue, Int16?)))
    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        Exportar()
    End Sub

    Protected Sub Exportar()
        Dim vTitulos As String() = "Id;NombreTrabajo;JobBook;Cod;Actividad;PRESUPUESTADO;AUTORIZADO;EJECUTADO;PRODUCCION".Split(";")

        Dim workbook = New XLWorkbook()
        Dim worksheet = workbook.Worksheets.Add("ListadoCambiosPersonas")

        Dim lstCambios = op.ObtenerActiviadesxTrabajo(If(String.IsNullOrEmpty(txtidtrabajo.Text), CType(Nothing, Int16?), CType(txtidtrabajo.Text, Int16?)), If(ddlactividad.SelectedValue = -1, CType(Nothing, Int16?), CType(ddlactividad.SelectedValue, Int16?)))
        worksheet.Cell("A2").InsertData(lstCambios)
        For x = 1 To vTitulos.Count
            worksheet.Cell(1, x).Value = vTitulos(x - 1)
        Next


        Crearexcel(workbook, "ReporteActividades -" & Now.Year & "-" & Now.Month & "-" & Now.Day)
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
End Class