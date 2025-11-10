Imports System.IO
Imports CoreProject

Public Class EnvioAUsuarioHWH
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim script As String = "setTimeout('window.close();',50)"
        Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idHWH") IsNot Nothing Then
            If Request.QueryString("estadoHWH") IsNot Nothing Then
                'Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/TH_TalentoHumano/HWH-Admin.aspx?idHWH=" + Request.QueryString("idHWH")
                GenerarHtml()
                ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
            Else
                ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
            End If
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
        Dim estadoHWH As String = Request.QueryString("estadoHWH").ToString()
        Dim teletrabajo As New TeleTrabajoC
        Dim hwh = teletrabajo.BuscarXId(idHWH)

        lblHWHId.Text = "Código HWH: " + hwh.id.ToString + " - Usuario: " + hwh.Nombre_Usuario
        lblUsuario.Text = hwh.Usuario_Gestion
        lblFecha.Text = hwh.Fecha_Programada
        lblEstado.Text = estadoHWH

        Dim listado = Nothing
        Dim u As New US.Usuarios
        listado = u.obtener(hwh.Id_Usuario, Nothing, Nothing, Nothing, Nothing, Nothing)

        Dim destinatarios As New List(Of String)

        For Each li As US_UsuariosAnyParameter_Get_Result In listado
            destinatarios.Add(li.Email)
        Next

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"

        'destinatarios.Add("John.Patino@ipsos.com")
        'destinatarios.Add("Cesar.Verano@ipsos.com")
        'destinatarios.Add("sistemamatrixtempo@gmail.com")

        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub

#End Region
End Class