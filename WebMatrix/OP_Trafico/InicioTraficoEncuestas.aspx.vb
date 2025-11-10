Imports CoreProject

Public Class InicioTraficoEncuestas
    Inherits System.Web.UI.Page
    Dim OP As New OP.TraficoEncuestas

#Region "Funciones y Métodos"
    Public Function ObtenerTE(ByVal TrabajoId) As List(Of OP_TraficoEncuestasCiudad_Result)
        Try
            Return OP.ObtenerTraficoEncuestasXTrabajo(TrabajoId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("TrabajoId") IsNot Nothing Then
                gvDatos.DataSource = ObtenerTE(CInt(Request.QueryString("TrabajoId")))
                gvDatos.DataBind()
            Else
                Response.Redirect("TrabajosProyectos.aspx")
            End If
        End If
    End Sub
#End Region

#Region "Eventos del Control"
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "RMC"
                    Dim Id = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Dim Ciudad = gvDatos.DataKeys(CInt(e.CommandArgument))("Res_Ciudad")
                    Dim Cuenta = gvDatos.DataKeys(CInt(e.CommandArgument))("cuenta")
                    Response.Redirect("RMC.aspx?TrabajoId=" & Id.ToString & "&Ciudad=" & Ciudad & "&Cuenta=" & Cuenta)
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
End Class