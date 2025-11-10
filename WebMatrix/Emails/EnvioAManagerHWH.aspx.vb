Imports System.IO
Imports CoreProject

Public Class EnvioAManagerHWH
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim script As String = "setTimeout('window.close();',50)"
        Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idHWH") IsNot Nothing Then
            Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/TH_TalentoHumano/HWH-Admin.aspx?idHWH=" + Request.QueryString("idHWH")
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

        Dim idHWH As Int64 = Int64.Parse(Request.QueryString("idHWH").ToString())

        Dim teletrabajo As New TeleTrabajoC
        Dim oPersonas As New Personas
        Dim hwh = teletrabajo.BuscarXId(idHWH)

        lblHWHId.Text = "Código HWH: " + hwh.id.ToString + " - Usuario: " + hwh.Nombre_Usuario
        lblUsuario.Text = hwh.Nombre_Usuario
        lblFecha.Text = hwh.Fecha_Programada

        Dim persona As New TH_Personas2
        persona = oPersonas.ObtenerPersonasxID(hwh.Id_Usuario).FirstOrDefault

        Dim u As New US.Usuarios
        Dim destinatarios As New List(Of String)
        Dim JefeInmediato = Nothing

        If hwh.Id_Usuario = persona.JefeInmediato Then
            JefeInmediato = u.obtenerUsuarioXId("79781273")
        Else
            JefeInmediato = u.obtenerUsuarioXId(persona.JefeInmediato)
        End If

        destinatarios.Add(JefeInmediato.Email)

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"

        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub

#End Region


End Class