Imports System.Net.Mail
Imports System.Net
Imports System.Threading.Tasks

Public Class EnviarCorreo

    Public Sub sendMail(ByVal Destinatarios As List(Of String), ByVal Asunto As String, ByVal Contenido As String)

        Dim SmtpServer As New SmtpClient()
        Dim mail As New MailMessage()
        Dim eTo = "", eNuestroCorreo = "", eNuestraContraseña As String = "", eCorreoAutenticacion As String = ""

        'Estos parametros ya viene configurando en el webconfig
        Dim correo As String = System.Configuration.ConfigurationManager.AppSettings("Correo")
        Dim correoAutenticacion As String = System.Configuration.ConfigurationManager.AppSettings("CorreoAutenticacion")
        Dim clave As String = System.Configuration.ConfigurationManager.AppSettings("Clave")
        Dim host As String = System.Configuration.ConfigurationManager.AppSettings("Host")
        Dim puerto As Integer = System.Configuration.ConfigurationManager.AppSettings("Puerto")
        Dim CorreoFrom As String = System.Configuration.ConfigurationManager.AppSettings("CorreoFrom")
        Dim CorreoDisplay As String = System.Configuration.ConfigurationManager.AppSettings("CorreoDisplayName")
        Dim enabledSSL As Boolean = CBool(System.Configuration.ConfigurationManager.AppSettings("EnabledSSL"))

        eTo = String.Join(",", Destinatarios.GroupBy(Function(x) x.Trim).Select(Function(x) x.Key).ToList)

        'Comenzamos el envio del correo
        'controlando si existiera algún error al envío
        Try
            'eNuestroCorreo = "Nuestro correo  configurado en el webconfig"
            eNuestroCorreo = correo
            'Nuestro contraseña 
            eNuestraContraseña = clave
            'Configuración para enviar el correo 

            eCorreoAutenticacion = correoAutenticacion
            SmtpServer.Port = puerto
            SmtpServer.Host = host
            SmtpServer.EnableSsl = enabledSSL
            If Not (correoAutenticacion = "null") Then
                SmtpServer.Credentials = New Net.NetworkCredential _
            (eCorreoAutenticacion, eNuestraContraseña)
            End If
            'Empezamos a configurar el correo a enviar
            'con los datos anteriores que pusimos
            'Le decimos que mail es un nuevo correo
            mail = New MailMessage()
            'Origen del correo
            mail.From = New MailAddress(CorreoFrom, CorreoDisplay)
            'Destinatarios del correo
            mail.To.Add(eTo)
            'Asunto del correo
            mail.Subject = Asunto
            'Texto del Mensaje
            mail.Body = Contenido
            'Texto en HTML del Mensaje
            mail.IsBodyHtml = True
            'Le decimos que envíe el correo
            SmtpServer.Send(mail)
            'Msgbox dando el Ok del envío
            'MsgBox("Correo enviado" + eTo)

        Catch ex As Exception
            Dim ubicacion = System.AppDomain.CurrentDomain.BaseDirectory & "Emails\ErroresEnvio.txt"
            Using sw2 As New IO.StreamWriter(ubicacion, True, System.Text.Encoding.Default)
                sw2.WriteLine("Fecha:" & DateTime.Now.ToString)
                sw2.WriteLine("Asunto:" & Asunto)
                sw2.WriteLine("Error:" & ex.Message)
                sw2.WriteLine("StackTrace:" & ex.StackTrace)
            End Using
        End Try
    End Sub

	Public Sub sendMailWithAttachment(ByVal Destinatarios As List(Of String), ByVal DestinatariosCopia As List(Of String), ByVal Asunto As String, ByVal Contenido As String, Fileattachment As Attachment)

		Dim SmtpServer As New SmtpClient()
		Dim mail As New MailMessage()
		Dim eTo = "", eCCTo = "", eNuestroCorreo = "", eNuestraContraseña As String = "", eCorreoAutenticacion As String = ""

		'Estos parametros ya viene configurando en el webconfig
		Dim correo As String = System.Configuration.ConfigurationManager.AppSettings("Correo")
		Dim correoAutenticacion As String = System.Configuration.ConfigurationManager.AppSettings("CorreoAutenticacion")
		Dim clave As String = System.Configuration.ConfigurationManager.AppSettings("Clave")
		Dim host As String = System.Configuration.ConfigurationManager.AppSettings("Host")
		Dim puerto As Integer = System.Configuration.ConfigurationManager.AppSettings("Puerto")
		Dim CorreoFrom As String = System.Configuration.ConfigurationManager.AppSettings("CorreoFrom")
		Dim CorreoDisplay As String = System.Configuration.ConfigurationManager.AppSettings("CorreoDisplayName")
		Dim enabledSSL As Boolean = CBool(System.Configuration.ConfigurationManager.AppSettings("EnabledSSL"))


		eTo = String.Join(",", Destinatarios.GroupBy(Function(x) x.Trim).Select(Function(x) x.Key).ToList)
		eCCTo = String.Join(",", DestinatariosCopia.GroupBy(Function(x) x.Trim).Select(Function(x) x.Key).ToList)

		'Comenzamos el envio del correo
		'controlando si existiera algún error al envío
		Try
			'eNuestroCorreo = "Nuestro correo  configurado en el webconfig"
			eNuestroCorreo = correo
			'Nuestro contraseña 
			eNuestraContraseña = clave
			'Configuración para enviar el correo 

			eCorreoAutenticacion = correoAutenticacion
			SmtpServer.Port = puerto
			SmtpServer.Host = host
			SmtpServer.EnableSsl = enabledSSL
			If Not (correoAutenticacion = "null") Then
				SmtpServer.Credentials = New Net.NetworkCredential _
			(eCorreoAutenticacion, eNuestraContraseña)
			End If
			'Empezamos a configurar el correo a enviar
			'con los datos anteriores que pusimos
			'Le decimos que mail es un nuevo correo
			mail = New MailMessage()
			'Origen del correo
			mail.From = New MailAddress(CorreoFrom, CorreoDisplay)
			'Destinatarios del correo
			mail.To.Add(eTo)
			mail.CC.Add(eCCTo)
			'Asunto del correo
			mail.Subject = Asunto
			'Texto del Mensaje
			mail.Body = Contenido
			'Texto en HTML del Mensaje
			mail.IsBodyHtml = True
			mail.Attachments.Add(Fileattachment)
			'Le decimos que envíe el correo
			SmtpServer.Send(mail)
			'Msgbox dando el Ok del envío
			'MsgBox("Correo enviado" + eTo)

		Catch ex As Exception
			Dim ubicacion = System.AppDomain.CurrentDomain.BaseDirectory & "Emails\ErroresEnvio.txt"
			Using sw2 As New IO.StreamWriter(ubicacion, True, System.Text.Encoding.Default)
				sw2.WriteLine("Fecha:" & DateTime.Now.ToString)
				sw2.WriteLine("Asunto:" & Asunto)
				sw2.WriteLine("Error:" & ex.Message)
				sw2.WriteLine("StackTrace:" & ex.StackTrace)
			End Using
		End Try
	End Sub

	Public Function enviarCorreo(ByVal strURL As String) As Boolean
        Dim objResponse As WebResponse
        Dim objRequest As HttpWebRequest = HttpWebRequest.Create(strURL)
        Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies(FormsAuthentication.FormsCookieName)
        Dim authenticationCookie As Cookie = New Cookie(FormsAuthentication.FormsCookieName, cookie.Value, cookie.Path, HttpContext.Current.Request.Url.Host)
        objRequest.CookieContainer = New CookieContainer
        objRequest.CookieContainer.Add(authenticationCookie)
        objResponse = objRequest.GetResponse()
    End Function
    Public Function enviarCorreoNoAuthentication(ByVal strURL As String) As Boolean
        Dim objResponse As WebResponse
        Dim objRequest As HttpWebRequest = HttpWebRequest.Create(strURL)
        objResponse = objRequest.GetResponse()
    End Function

    Public Function AsyncEnviarCorreo(ByVal strURL As String) As Boolean
        Dim oEnviarCorreo As New EnviarCorreo
        Dim cookieValue As String = HttpContext.Current.Request.Cookies(FormsAuthentication.FormsCookieName).Value
        Dim cookiePath As String = HttpContext.Current.Request.Cookies(FormsAuthentication.FormsCookieName).Path
        Dim contextHost As String = HttpContext.Current.Request.Url.Host
        Task(Of Boolean).Factory.StartNew(Function() enviar(strURL, cookieValue, cookiePath, contextHost))
    End Function
    Private Function enviar(ByVal strURL As String, ByVal cookieValue As String, ByVal cookiePath As String, ByVal host As String) As Boolean
        Dim objResponse As WebResponse
        Dim objRequest As HttpWebRequest = HttpWebRequest.Create(strURL)
        Dim authenticationCookie As Cookie = New Cookie(FormsAuthentication.FormsCookieName, cookieValue, cookiePath, host)
        objRequest.CookieContainer = New CookieContainer
        objRequest.CookieContainer.Add(authenticationCookie)
        objResponse = objRequest.GetResponse()
        Return True
    End Function
End Class
