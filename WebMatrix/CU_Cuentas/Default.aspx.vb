Imports CoreProject
Public Class _DefaultCuentas
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			If Not (Session("InfoJobBook") Is Nothing) Then
				LoadInfoJobBook()
			End If
		End If
	End Sub

	Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
		Dim permisos As New Datos.ClsPermisosUsuarios
		If permisos.VerificarPermisoUsuario(22, Session("IDUsuario").ToString()) = False Then
			Response.Redirect("../Home/home.aspx")
		End If
	End Sub

	Protected Sub btnSearch_Click(sender As Object, e As EventArgs)
		Dim oData As New CU_JobBook.DAL
		Dim idPropuesta As Int64?
		If IsNumeric(txtIdPropuestaSearch.Text) Then idPropuesta = txtIdPropuestaSearch.Text
		gvDataSearch.DataSource = oData.InfoJobBookGet(txtTituloSearch.Text, txtJobBookSearch.Text, idPropuesta, Session("IDUsuario").ToString, rbSearch.SelectedValue)
		gvDataSearch.DataBind()
	End Sub

	Protected Sub gvDataSearch_RowCommand(sender As Object, e As GridViewCommandEventArgs)
		Select Case e.CommandName
			Case "Info"
				Dim info As New oJobBook
				Dim oData As New CU_JobBook.DAL
				Dim rData = oData.InfoJobBookGet(idBrief:=Int64.Parse(gvDataSearch.DataKeys(CInt(e.CommandArgument))("IdBrief")), IdPropuesta:=IIf(Int64.Parse(gvDataSearch.DataKeys(CInt(e.CommandArgument))("IdPropuesta")) = 0, Nothing, Int64.Parse(gvDataSearch.DataKeys(CInt(e.CommandArgument))("IdPropuesta"))), idEstudio:=IIf(Int64.Parse(gvDataSearch.DataKeys(CInt(e.CommandArgument))("IdEstudio")) = 0, Nothing, Int64.Parse(gvDataSearch.DataKeys(CInt(e.CommandArgument))("IdEstudio")))).FirstOrDefault
				info.Cliente = rData.Cliente
				info.Estado = rData.Estado
				info.GerenteCuentas = rData.GerenteCuentas
				info.GerenteCuentasID = rData.GerenteCuentasID
				info.IdBrief = rData.IdBrief
				info.IdEstudio = rData.IdEstudio
				info.IdPropuesta = rData.IdPropuesta
				info.IdUnidad = rData.IdUnidad
				info.MarcaCategoria = rData.MarcaCategoria
				info.Titulo = rData.Titulo
				info.Unidad = rData.Unidad
				info.Viabilidad = rData.Viabilidad
				info.NumJobBook = rData.NumJobbook
				If rbSearch.SelectedValue = 1 Or rbSearch.SelectedValue = 2 Then info.GuardarCambios = True Else info.GuardarCambios = False
				If info.GerenteCuentasID = Session("IDUsuario").ToString Then info.GuardarCambios = True
				Session("InfoJobBook") = info
				'If Not (info.IdEstudio = 0) Then Response.Redirect("Estudio.aspx")
				'If Not (info.IdPropuesta = 0) Then Response.Redirect("Propuesta.aspx")
				If Not (info.IdBrief = 0) Then Response.Redirect("Frame.aspx")
			Case "Duplicate"
				hfBriefToDuplicar.Value = Int64.Parse(gvDataSearch.DataKeys(CInt(e.CommandArgument))("IdBrief"))
				CargarUnidades()
				ModalPopupExtenderClonar.Show()
		End Select
	End Sub


	Sub LoadInfoJobBook()
		If Not (Session("InfoJobBook") Is Nothing) Then
			Dim infoJobBook As oJobBook = Session("InfoJobBook")
			lblInfo.Text = infoJobBook.NumJobBook & " | " & infoJobBook.Titulo & " | " & infoJobBook.Cliente & " | " & infoJobBook.IdPropuesta.ToString
		End If
	End Sub

	Protected Sub btnNew_Click(sender As Object, e As EventArgs)
		Session("InfoJobBook") = Nothing
		Response.Redirect("Frame.aspx")
	End Sub

	Private Sub CargarUnidades()
		Dim oUnidades As New CoreProject.US.Unidades
		ddlUnidades.DataSource = oUnidades.ObtenerUnidadesxUsuario(Session("IDUsuario").ToString)
		ddlUnidades.DataTextField = "Unidad"
		ddlUnidades.DataValueField = "id"
		ddlUnidades.DataBind()
	End Sub

	Protected Sub btnOkClone_Click(sender As Object, e As EventArgs)
		If ddlUnidades.Items.Count = 0 Then
			Exit Sub
		End If
		If txtNuevoNombre.Text = "" Then
			Exit Sub
		End If
		Dim oData As New CU_JobBook.DAL
		oData.CloneBrief(hfBriefToDuplicar.Value, Session("IDUsuario").ToString, ddlUnidades.SelectedValue, txtNuevoNombre.Text)
		Dim idPropuesta As Int64?
		If IsNumeric(txtIdPropuestaSearch.Text) Then idPropuesta = txtIdPropuestaSearch.Text
		gvDataSearch.DataSource = oData.InfoJobBookGet(txtTituloSearch.Text, txtJobBookSearch.Text, idPropuesta, Session("IDUsuario").ToString, rbSearch.SelectedValue)
		gvDataSearch.DataBind()
	End Sub
End Class