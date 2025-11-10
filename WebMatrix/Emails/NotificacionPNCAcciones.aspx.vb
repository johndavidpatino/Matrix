Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class Emails_NotificacionPNCAcciones
    Inherits System.Web.UI.Page
    'Dim WJobBook As String
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim script As String = "setTimeout('window.close();',50)"
        'Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idPNCDet") IsNot Nothing Then
            Dim estudio As Int64 = Int64.Parse(Request.QueryString("idPNCDet").ToString())
            lblEstudioId.Text = estudio
            'WJobBook = Request.QueryString("JOBBOOK")
            CargarElemento(estudio)
            'GenerarHtml()
            'ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        Else
            'ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub CargarElemento(ByVal EstudioId As Long)
        Dim o As New CoreProject.PNCClass
        Dim info = o.ObtenerPNCDetalles(lblEstudioId.Text)
        Me.lblAccion.Text = info.Accion
        Me.lblAsunto.Text = Me.lblAsunto.Text & " " & info.Trabajo
        Me.lblDescripcion.Text = info.DescripcionPNC
        If IsDate(info.FechaPlaneada) Then Me.lblFechaPlaneada.Text = CDate(info.FechaPlaneada).ToShortDateString
        Me.lblResponsableAccion.Text = info.ResponsableAccion
        Me.lblResponsableSeguimiento.Text = info.ResponsableSeguiimiento
        Me.lblTipoAccion.Text = info.TipodeAccion
        Dim o2 = o.ObtenerPNCDetallesEntidad(info.Id)
        Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/GD_Documentos/ProductoNoConformeRegistrar.aspx?idPNC=" & o2.IdPNC
        GenerarHtml(info.EmailResponsable, info.EmailSeguimiento)
    End Sub

    Sub GenerarHtml(ByVal emailresponsable As String, emailseguimiento As String)
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"
        Dim destinatarios As New List(Of String)
        'Dim o As New DestinatariosCorreos
        'Dim estudio As Int64 = Int64.Parse(Request.QueryString("idPNC").ToString())
        'For i As Integer = 0 To o.DestinatariosAnuncioAprobacion(estudio).Count - 1
        '    destinatarios.Add(o.DestinatariosAnuncioAprobacion(estudio).Item(i).CORREO)
        'Next
        'If Request.QueryString("idGerenteProyectos") IsNot Nothing Then
        '    destinatarios.Clear()
        '    Dim oUsuarios As New US.Usuarios
        '    destinatarios.Add(oUsuarios.UsuarioGet(Request.QueryString("idGerenteProyectos")).Email)
        'End If
        destinatarios.Add(emailresponsable)
        destinatarios.Add(emailseguimiento)
        'destinatarios.Add("Cesar.Verano@ipsos.com")
        'destinatarios.Add("sistemamatrixtempo@gmail.com")
        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region
End Class