Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class Emails_NotificacionPNC
    Inherits System.Web.UI.Page
    Dim idPNC As Decimal
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.hLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/PNC/PNC_Productos.aspx"
        Decimal.TryParse(Request.QueryString("idPNC"), idPNC)
        GenerarHtml()
    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)
        Dim DA As New PNCClass

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"
        Dim destinatarios As New List(Of String)

        destinatarios.AddRange(DA.obtenerUsuariosNotificar(idPNC))

        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, "Nuevo producto no conforme", contenido)
    End Sub
#End Region
End Class