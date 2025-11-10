Imports CoreProject
Imports DocumentFormat.OpenXml.Wordprocessing

Public Class PresupuestosInternosIndex
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Dim permisos As New Datos.ClsPermisosUsuarios
		Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
		If permisos.VerificarPermisoUsuario(100, UsuarioID) = False Then
			Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
		End If
		If Not IsPostBack Then
			If Not Session("TrabajoId") = Nothing Then
				hfIdTrabajo.Value = Session("TrabajoId").ToString
			End If
		End If
		If Not IsPostBack Then

		End If
	End Sub

	Sub CargarTrabajos()
		Dim oTrabajo As New GestionTrabajosFin
		gvTrabajos.DataSource = oTrabajo.ListadoTrabajoscc(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
		gvTrabajos.DataBind()
	End Sub
	Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
		gvTrabajos.PageIndex = e.NewPageIndex
		CargarTrabajos()
	End Sub
	Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
		If e.CommandName = "Actualizar" Then
			'cargarTrabajo(Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")))
			hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
			Session("TrabajoId") = hfIdTrabajo.Value
		ElseIf e.CommandName = "Presupuestos" Then
			hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
			Response.Redirect("../CC_FinzOpe/PresupuestoInterno.aspx?TrabajoId=" & hfIdTrabajo.Value)
		End If
	End Sub

	Protected Sub btnSearch_Click(sender As Object, e As EventArgs)
		Dim oTrabajo As New GestionTrabajosFin
		Dim id As Int64? = Nothing
		Dim JobBook As String = Nothing
		Dim Nombre As String = Nothing
		Dim NoProp As Int64? = Nothing
		If Not (txtTrabajo.Text = "") Then id = txtTrabajo.Text
		If Not (txtJobBook.Text = "") Then JobBook = txtJobBook.Text
		If Not (txtNombreTrabajo.Text = "") Then Nombre = txtNombreTrabajo.Text
		If Not (txtPropuesta.Text = "") Then NoProp = txtPropuesta.Text
		gvTrabajos.DataSource = oTrabajo.ListadoTrabajoscc(id, Nothing, Nombre, JobBook, Nothing, Nothing, Nothing, Nothing, Nothing, NoProp)
		gvTrabajos.DataBind()
	End Sub
End Class