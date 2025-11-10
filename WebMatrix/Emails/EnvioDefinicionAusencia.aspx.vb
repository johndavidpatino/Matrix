Imports System.IO
Imports CoreProject

Public Class EnvioDefinicionAusencia
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim script As String = "setTimeout('window.close();',50)"
        Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idSolicitud") IsNot Nothing Then
            GenerarHtml()
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        Else
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        End If
    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)

        Dim idSolicitud As Int64 = Int64.Parse(Request.QueryString("idSolicitud").ToString())

        Dim o As New TH_Ausencia.DAL
        Dim oPersonas As New Personas
        Dim info = o.RegistrosAusencia(idSolicitud, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)(0)
        Dim info2 = o.GetSolicitudAusencia(idSolicitud)
        lblHWHId.Text = "Código Solicitud: " & info.ID
        lblStatus.Text = info.Estado
        lblTipoAusencia.Text = info.Tipo
        lblFini.Text = info.FInicio.Value
        lblFFin.Text = info.FFin.Value
        lblAsunto.Text = "Matrix: Respuesta a Solicitud de " & info.Tipo
        lblMensajeAdicional.Text = ""

        If info2.Estado = 20 Then
            Select Case info2.Tipo
                Case 1
                    lblMensajeAdicional.Text = ""
                Case 2
                    lblMensajeAdicional.Text = "Apreciamos tu contribución en Ipsos, disfruta de tu plus de vacaciones como más te guste"
                Case 3
                    lblMensajeAdicional.Text = "Keep calm and enjoy your balance day."
                Case 4
                    lblMensajeAdicional.Text = "¡Ipsos te desea un feliz cumpleaños!"
                Case 5
                    lblMensajeAdicional.Text = "Compartimos tu felicidad por alcanzar una meta más!"
                Case 6
                    lblMensajeAdicional.Text = "¡Felicidades por tu matrimonio!, disfruta de este día especial."
                Case 7
                    lblMensajeAdicional.Text = "Celebramos contigo el nacimiento de tu hijo/a… Disfruta este tiempo para compartir en familia."
                Case 8
                    lblMensajeAdicional.Text = "Recuerda traer el soporte"
                Case 9
                    lblMensajeAdicional.Text = ""
                Case 10
                    lblMensajeAdicional.Text = "¡Te deseamos una pronta recuperación! Recuerda hacernos llegar la incapacidad."
                Case 11
                    lblMensajeAdicional.Text = ""
            End Select
        End If

        Dim u As New US.Usuarios
        Dim destinatarios As New List(Of String)

        destinatarios.Add(u.obtenerUsuarioXId(info2.idEmpleado).Email)
        destinatarios.Add("matrix@ipsos.com")
        destinatarios.Add("recursoshumanoscolombia@ipsos.com")

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"

        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub

#End Region


End Class