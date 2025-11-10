Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class DisenoDeMuestraMail
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
            Dim oPropuesta As New Propuesta
            Dim infop = oPropuesta.DevolverxID(info.Propuestaid)
            Me.lblPropuesta.Text = infop.Titulo
            Me.hplLink.NavigateUrl = Me.Request.Url.Scheme & Uri.SchemeDelimiter & Me.Request.Url.Authority & "/ES_Estadistica/DisenoDeMuestra.aspx?idbrief=" & info.id
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

        Dim oBrief As New BriefDisenoMuestral
        Dim estudio As Int64 = Int64.Parse(Request.QueryString("idBrief").ToString())
        Dim info = oBrief.DevolverxID(estudio)

        Dim oBriefC As New Brief
        Dim oPropuesta As New Propuesta
        Dim infop = oPropuesta.DevolverxID(info.Propuestaid)
        Dim infob = oBriefC.DevolverxID(infop.Brief)

        Dim oUsuarios As New US.Usuarios

        Dim destinatario As String = oUsuarios.UsuarioGet(infob.GerenteCuentas).Email
        
        Dim destinatarios As New List(Of String)
        destinatarios.Add(destinatario)
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