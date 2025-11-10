Imports CoreProject

Public Class ListaActas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarGrid(1)
        End If
    End Sub

    Public Function ConsultarAC() As List(Of SG_ActaComite_Get_Result)
        Dim Data As New SG.ActasComite
        Dim Info As List(Of SG_ActaComite_Get_Result)
        Try
            Info = Data.ObtenerActasComite()
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarGrid(ByVal Opcion As Integer)
        Select Case Opcion
            Case 1
                Session("ListaCargada") = ConsultarAC()
                gvDatos.DataSource = Session("ListaCargada")
                gvDatos.DataBind()
        End Select

    End Sub

    Protected Sub gvDatos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "EditarActaComite"
                    Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Response.Redirect("ActasComite.aspx?ActaId=" & Id.ToString)
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        gvDatos.DataSource = Session("ListaCargada")
        gvDatos.DataBind()
    End Sub

End Class