Imports CoreProject
Imports WebMatrix.Util
Public Class REP_Top10Encuestadores
    Inherits System.Web.UI.Page
    Private _Identificacion As Int64
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarAnulacion(1, -1)
            CargarErrores(1, -1)
            CargarVIP(1, -1)
        End If
    End Sub
#Region "Metodos"
    Sub CargarAnulacion(ByVal meses As Int32, ByVal Ciudad As Int32?)
        If Ciudad = -1 Then Ciudad = Nothing
        Dim o As New Reportes.Top10
        Me.gvAnulacion.DataSource = o.ObtenerTopEncuestadoresAnulacion(meses, Ciudad)
        Me.gvAnulacion.DataBind()
    End Sub

    Sub CargarErrores(ByVal meses As Int32, ByVal Ciudad As Int32?)
        If Ciudad = -1 Then Ciudad = Nothing
        Dim o As New Reportes.Top10
        Me.gvErrores.DataSource = o.ObtenerTopEncuestadoresErrores(meses, Ciudad)
        Me.gvErrores.DataBind()
    End Sub

    Sub CargarVIP(ByVal meses As Int32, ByVal Ciudad As Int32?)
        If Ciudad = -1 Then Ciudad = Nothing
        Dim o As New Reportes.Top10
        Me.gvVIP.DataSource = o.ObtenerTopEncuestadoresVIP(meses, Ciudad)
        Me.gvVIP.DataBind()
    End Sub

#End Region

#Region "Eventos"
    Private Sub gvAnulacion_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAnulacion.RowCommand
        If e.CommandName = "Actualizar" Then
            Dim id As Int64 = gvAnulacion.DataKeys(e.CommandArgument)("IdEncuestador")
            Response.Redirect("../TH_TalentoHumano/FichaEncuestador.aspx?Identificacion=" & id)
        End If
    End Sub
    Private Sub gvErrores_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvErrores.RowCommand
        If e.CommandName = "Actualizar" Then
            Dim id As Int64 = gvErrores.DataKeys(e.CommandArgument)("IdEncuestador")
            Response.Redirect("../TH_TalentoHumano/FichaEncuestador.aspx?Identificacion=" & id)
        End If
    End Sub
    Private Sub gvVIP_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvVIP.RowCommand
        If e.CommandName = "Actualizar" Then
            Dim id As Int64 = gvVIP.DataKeys(e.CommandArgument)("IdEncuestador")
            Response.Redirect("../TH_TalentoHumano/FichaEncuestador.aspx?Identificacion=" & id)
        End If
    End Sub
#End Region

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        CargarAnulacion(txtMeses.Text, ddlCiudades.SelectedValue)
        CargarErrores(txtMeses.Text, ddlCiudades.SelectedValue)
        CargarVIP(txtMeses.Text, ddlCiudades.SelectedValue)
    End Sub
End Class