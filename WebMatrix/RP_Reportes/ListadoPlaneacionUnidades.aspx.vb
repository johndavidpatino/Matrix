Imports WebMatrix.Util
Imports CoreProject
Imports ClosedXML.Excel

Public Class REP_ListadoPlaneacionUnidades
	Inherits System.Web.UI.Page

#Region "Eventos"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			CargarGruposUnidad()
			'Dim permisos As New Datos.ClsPermisosUsuarios
			'Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
			'If permisos.VerificarPermisoUsuario(16, UsuarioID) = False Then
			'	Response.Redirect("../Home.aspx")
			'End If
		End If
	End Sub


#End Region

#Region "Metodos"
	Sub CargarGruposUnidad()
		Dim oGruposUnidad As New US.GrupoUnidad
		ddlGrupoUnidades.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(1)
		ddlGrupoUnidades.DataValueField = "id"
		ddlGrupoUnidades.DataTextField = "GrupoUnidad"
		ddlGrupoUnidades.DataBind()
		ddlGrupoUnidades.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
	End Sub

	Sub CargarGrid(grupounidad As Int32?, Optional Unidad As Int32? = Nothing)
		Dim o As New Reportes.Directores
		Me.gvDatos.DataSource = o.ObtenerListadoPlaneacionUnidad(grupounidad, Unidad)
		Me.gvDatos.DataBind()
	End Sub
#End Region


	Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
		If Me.ddlGrupoUnidades.SelectedValue = 0 Or Me.ddlGrupoUnidades.SelectedValue = -1 Then
			ShowNotification("Seleccione primero la unidad antes de continuar", ShowNotifications.ErrorNotification)
			ActivateAccordion(0, EffectActivateAccordion.NoEffect)
			Exit Sub
		End If
		CargarGrid(ddlGrupoUnidades.SelectedValue)
		ActivateAccordion(0, EffectActivateAccordion.NoEffect)
	End Sub

	Private Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
		If Me.ddlGrupoUnidades.SelectedValue = 0 Or Me.ddlGrupoUnidades.SelectedValue = -1 Then
			ShowNotification("Seleccione primero la unidad antes de continuar", ShowNotifications.ErrorNotification)
			ActivateAccordion(0, EffectActivateAccordion.NoEffect)
			Exit Sub
		End If
		CargarGrid(ddlGrupoUnidades.SelectedValue)
		ActivateAccordion(0, EffectActivateAccordion.NoEffect)
		Dim excel As New List(Of Array)
		Dim Titulos As String = "ID;JobBook;Nombre;FechaInicioCampo;FechaFinCampo;Responsable;Tipo"
		Dim DynamicColNames() As String
		Dim lstCambios As List(Of REP_ListadoPlaneacionUnidades_Result)
		Dim workbook = New XLWorkbook()
		Dim worksheet = workbook.Worksheets.Add("CedulasSinProduccion")

		DynamicColNames = Titulos.Split(CChar(";"))
		excel.Add(DynamicColNames)
		Dim o As New Reportes.Directores
		lstCambios = o.ObtenerListadoPlaneacionUnidad(ddlGrupoUnidades.SelectedValue, Nothing)
		For Each x In lstCambios
			excel.Add((x.Id & "|" & x.JobBook & "|" & x.Nombre & "|" & x.FechaInicioCampo & "|" & x.FechaFinCampo & "|" & x.Responsable & "|" & x.Tipo).Split(CChar("|")).ToArray())
		Next
		worksheet.Cell("A1").Value = excel

		worksheet.Columns("A", "G").AdjustToContents(1, 2)
		worksheet.Range("D2:E" & lstCambios.Count + 1).DataType = XLCellValues.DateTime
		worksheet.Range("D2:E" & lstCambios.Count + 1).Style.DateFormat.SetFormat("dd/mm/yyyy")

		'worksheet.Columns("X", "AD").AdjustToContents(1, 2)
		'worksheet.Columns("AE", "AG").AdjustToContents(1, 2)

		Crearexcel(workbook, "ListadoPlaneacionUnidad" & ddlGrupoUnidades.SelectedItem.Text)
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