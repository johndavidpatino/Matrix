Imports CoreProject
Public Class TrabajosProyectos
    Inherits System.Web.UI.Page
    Dim PY As New Trabajo

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            gvDatos.DataSource = PY.DevolverxID(Nothing)
            gvDatos.DataBind()
        End If
    End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "ITE"
                    Dim Id = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Response.Redirect("InicioTraficoEncuestas.aspx?TrabajoId=" & Id.ToString)
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
End Class