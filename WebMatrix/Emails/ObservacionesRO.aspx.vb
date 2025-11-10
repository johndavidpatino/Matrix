Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class ObservacionesROMail
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim script As String = "setTimeout('window.close();',50)"
        Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idTrabajo") IsNot Nothing Then
            Dim TipoRO As String = Request.QueryString("tipoRO").ToString
            If Request.QueryString("fromgerencia") IsNot Nothing Then
                Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/OP_RO/" & TipoRO & ".aspx?idtrabajo=" & Request.QueryString("idTrabajo")
            Else
                Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/OP_RO/" & TipoRO & ".aspx?idtrabajo=" & Request.QueryString("idTrabajo") & "&fromgerencia=yes"
            End If

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



        Dim estudio As Int64 = Int64.Parse(Request.QueryString("idTrabajo").ToString())
        Dim oTrabajo As New Trabajo
        Dim info = oTrabajo.obtenerXId(estudio)
        Dim oProyecto As New Proyecto
        Dim infop = oProyecto.obtenerXId(info.ProyectoId)

        Dim oUsuarios As New US.Usuarios
        Dim destinatarios As New List(Of String)

        Me.lblTrabajo.Text = info.NombreTrabajo
        Dim TipoRO As String = Request.QueryString("tipoRO").ToString
        If Request.QueryString("fromgerencia") IsNot Nothing Then
            Me.lblAsunto.Text = "Respuesta a Observaciones RO"
            Me.lblBodyRO.Text = "Se han contestado las observaciones al RO " & TipoRO & " del trabajo "
            destinatarios.Add(oUsuarios.UsuarioGet(info.COE).Email)
        Else
            Me.lblAsunto.Text = "Observaciones RO"
            Me.lblBodyRO.Text = "Se han hecho bservaciones al RO " & TipoRO & " del trabajo "
            destinatarios.Add(oUsuarios.UsuarioGet(infop.GerenteProyectos).Email)
        End If

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