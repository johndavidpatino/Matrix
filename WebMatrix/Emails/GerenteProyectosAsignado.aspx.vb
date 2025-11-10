Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class GerenteProyectosMail
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim script As String = "setTimeout('window.close();',50)"
        'Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idProyecto") IsNot Nothing Then
            Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/PY_Proyectos/PY_Proyectos.aspx"
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



        Dim estudio As Int64 = Int64.Parse(Request.QueryString("idProyecto").ToString())
        Dim oProyecto As New Proyecto
        Dim info = oProyecto.obtenerXId(estudio)

        Me.lblProyecto.Text = info.JobBook & " - " & info.Nombre

        Dim oEstudio As New Estudio
        Dim infoe = oEstudio.ObtenerXID(info.EstudioId)
        Dim oBriefC As New Brief
        Dim oPropuesta As New Propuesta
        Dim infop = oPropuesta.DevolverxID(infoe.PropuestaId)
        Dim infob = oBriefC.DevolverxID(infop.Brief)

        Dim oUsuarios As New US.Usuarios

        Me.lblGerenteProyectos.Text = oUsuarios.UsuarioGet(info.GerenteProyectos).Nombres & " " & oUsuarios.UsuarioGet(info.GerenteProyectos).Apellidos
        Me.lblGerenteCuentas.Text = oUsuarios.UsuarioGet(infob.GerenteCuentas).Nombres & " " & oUsuarios.UsuarioGet(infob.GerenteCuentas).Apellidos
        Dim destinatarios As New List(Of String)

        destinatarios.Add(oUsuarios.UsuarioGet(info.GerenteProyectos).Email)
        destinatarios.Add(oUsuarios.UsuarioGet(infob.GerenteCuentas).Email)

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