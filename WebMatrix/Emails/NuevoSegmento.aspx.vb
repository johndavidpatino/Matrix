Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class NuevoSegmentoMail
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim script As String = "setTimeout('window.close();',50)"
        'Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idTrabajo") IsNot Nothing Then
            GenerarHtml()
            'ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        Else
            'ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
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



        Dim estudio As Int64 = Int64.Parse(Request.QueryString("idTrabajo").ToString())
        Dim oTrabajo As New Trabajo
        Dim info = oTrabajo.obtenerXId(estudio)

        Me.lblTrabajo.Text = info.NombreTrabajo

        Dim oUsuarios As New US.Usuarios

        Dim destinatarios As New List(Of String)

        destinatarios.Add(oUsuarios.UsuarioGet(info.COE).Email)

        Dim oProyecto As New Proyecto
        Dim infop = oProyecto.obtenerXId(estudio)

        destinatarios.Add(oUsuarios.UsuarioGet(infop.GerenteProyectos).Email)

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"

        'destinatarios.Add("John.Patino@ipsos.com")
        'destinatarios.Add("Cesar.Verano@ipsos.com")
        

        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region
End Class