Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class JBIAsignadoMail
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
        Dim Metodologia As Int32 = Int32.Parse(Request.QueryString("CodMetod").ToString())
        Dim JBI As String = Request.QueryString("JBIInt").ToString()
        Dim oProyecto As New Proyecto
        Dim o As New MetodologiaOperaciones

        Dim info = oProyecto.obtenerXId(estudio)

        Me.lblProyecto.Text = info.JobBook & " - " & info.Nombre
        Me.lblJBI.Text = JBI
        Me.lblMetodologia.Text = o.obtenerXCod(Metodologia).MetNombre
        Dim oEstudio As New Estudio

        Dim oUsuarios As New US.Usuarios


        Dim destinatarios As New List(Of String)

        destinatarios.Add(oUsuarios.UsuarioGet(info.GerenteProyectos).Email)

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"

        'destinatarios.Add("John.Patino@ipsos.com")
        'destinatarios.Add("Cesar.Verano@ipsos.com")
        
        If destinatarios.Count > 0 Then
            Dim oEnviarCorreo As New EnviarCorreo
            oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
        End If

    End Sub
#End Region
End Class