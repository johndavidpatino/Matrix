Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class RespuestaFeedbackMail
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim script As String = "setTimeout('window.close();',50)"
        Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idFeedBack") IsNot Nothing Then
            GenerarHtml()
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        Else
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub CargarElemento(ByVal EstudioId As Long)
        

    End Sub
    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)

        
        Dim estudio As Int64 = Int64.Parse(Request.QueryString("idFeedback").ToString())

        Dim oFeedback As New CoreProject.Feedback
        Dim ent As New CORE_Retroalimentacion
        ent = oFeedback.ObtenerFeedbackXId(estudio)
        Me.lblMensaje.Text = ent.Mensaje
        Me.lblFecha.Text = ent.Fecha
        Me.lblRespuesta.Text = ent.Respuesta

        Dim destinatarios As New List(Of String)

        destinatarios.Add(ent.US_Usuarios.Email)

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"

        'destinatarios.Add("John.Patino@ipsos.com")
        'destinatarios.Add("Cesar.Verano@ipsos.com")
        destinatarios.Add("sistemamatrixtempo@gmail.com")
        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region
End Class