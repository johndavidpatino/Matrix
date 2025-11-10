Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class BriefEstadisticaMailCambio
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim script As String = "setTimeout('window.close();',50)"
        Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idBrief") IsNot Nothing Then
            Dim estudio As Int64 = Int64.Parse(Request.QueryString("idBrief").ToString())
            'CargarElemento(estudio)
            'CargarPresupuestosXEstudio(estudio)
            Dim oBrief As New BriefDisenoMuestral
            Dim info = oBrief.DevolverxID(estudio)
            Dim cla = New US.Usuarios
            Dim usuario = cla.UsuarioGet(Session("IDUsuario"))
            lblSolicitante.Text = Trim(info.Gerente.ToUpper)
            Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/ES_Estadistica/BriefDisenoDeMuestra.aspx?idPropuesta=" & info.Propuestaid & "&brieflist"
            GenerarHtml()
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        Else
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub CargarElemento(ByVal EstudioId As Long)
        Dim oAnuncio As New CoreProject.AnuncioAprobacion
        lblAsunto.Text = lblAsunto.Text

    End Sub
    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)

        Me.pnlBody.RenderControl(hw)
        Dim oUsuarios As New US.Usuarios
        Dim listUsuarios = oUsuarios.UsuariosxUnidad(40)

        Dim destinatarios As New List(Of String)

        For Each li As Usuarios_Result In listUsuarios
            destinatarios.Add(li.Email)
        Next

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"

        'destinatarios.Add("John.Patino@ipsos.com")
        'destinatarios.Add("Cesar.Verano@ipsos.com")

        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region
End Class