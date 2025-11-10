Imports CoreProject
Public Class ListadoEncuestadores
	Inherits System.Web.UI.Page

#Region "Eventos"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Dim permisos As New Datos.ClsPermisosUsuarios
		Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
		If permisos.VerificarPermisoUsuario(93, UsuarioID) = False Then
			Response.Redirect("../TH_TalentoHumano/Default.aspx")
		End If
		If Not IsPostBack Then
			cargarPersonas()
		End If
	End Sub

	Private Sub gvContratacion_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvContratacion.RowCommand
		If e.CommandName = "Actualizar" Then
			Dim id As Int64 = gvContratacion.DataKeys(e.CommandArgument)("Id")
			Response.Redirect("FichaEncuestador.aspx?Identificacion=" & id)
		End If
	End Sub

	Private Sub gvContratacion_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvContratacion.PageIndexChanging
		gvContratacion.PageIndex = e.NewPageIndex
		cargarPersonas()
	End Sub
#End Region

#Region "Metodos"
	Sub cargarPersonas()
		Dim oPersonas As New Personas
		gvContratacion.DataSource = oPersonas.DevolverxCargoIDTodosLoscampos(Cargos.TiposCargos.Encuestador, txtBuscar.Text.Trim)
		gvContratacion.DataBind()
	End Sub
#End Region

	Private Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
		cargarPersonas()
	End Sub
End Class