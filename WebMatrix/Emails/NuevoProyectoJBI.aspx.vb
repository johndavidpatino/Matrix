Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo


Public Class Emails_NuevoProyectoJBI
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim script As String = "setTimeout('window.close();',50)"
        Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idProyecto") IsNot Nothing Then
            Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/RE_GT/AsignacionJBI.aspx"
            GenerarHtml()
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        Else
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub CargarElemento(ByVal EstudioId As Long)
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
        Dim od As New Reportes.GerentesOP
        Dim listado = od.ObtenerCorreosNuevoProyecto(estudio)

        Dim destinatarios As New List(Of String)

        For Each li As US_Correos_Result In listado
            destinatarios.Add(li.CORREO)
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