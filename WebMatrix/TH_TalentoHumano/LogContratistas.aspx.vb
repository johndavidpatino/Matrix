Public Class LogContratistas
    Inherits System.Web.UI.Page
    Dim op As New CoreProject.Contratista
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarInformacion(Nothing, Nothing)
        End If
    End Sub

    Sub CargarInformacion(ByVal ContratistaId As Int64?, ByVal Nombre As String)
        If op.LogContratistasGet(ContratistaId, Nombre).Count > 0 Then
            gvlogContratistas.DataSource = op.LogContratistasGet(ContratistaId, Nombre)
            gvlogContratistas.DataBind()
        End If
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If txtIdentificacionBuscar.Text = "" And txtNombreBuscar.Text <> "" Then
            CargarInformacion(Nothing, txtNombreBuscar.Text)
        ElseIf txtNombreBuscar.Text = "" And txtIdentificacionBuscar.Text <> "" Then
            CargarInformacion(txtIdentificacionBuscar.Text, Nothing)
        ElseIf txtNombreBuscar.Text = "" And txtIdentificacionBuscar.Text = "" Then
            CargarInformacion(Nothing, Nothing)
        End If
    End Sub
End Class