Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class NuevoProyectoCoordinacionMail
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim script As String = "setTimeout('window.close();',50)"
        Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idProyecto") IsNot Nothing Then
            Me.hplLink.NavigateUrl = Me.Request.Url.Scheme & Uri.SchemeDelimiter & Me.Request.Url.Authority & "/PY_Proyectos/AsignacionProyectos.aspx"
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



        Dim estudio As Int64 = Int64.Parse(Request.QueryString("idProyecto").ToString())
        Dim oProyecto As New Proyecto
        Dim info = oProyecto.obtenerXId(estudio)

        Me.lblProyecto.Text = info.JobBook & " - " & info.Nombre

        Dim oUsuarios As New US.Usuarios
        Dim listUsuarios = oUsuarios.UsuariosxUnidadXrol(info.UnidadId, ListaRoles.CoordinadorUnidad)

        Dim destinatarios As New List(Of String)

        For Each li As Usuarios_Result In listUsuarios
            destinatarios.Add(li.Email)
        Next

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