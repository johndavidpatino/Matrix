Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class SolicitudCambioGrossMargin
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("idPropuesta") IsNot Nothing Then
            Dim propuesta As Int64 = Int64.Parse(Request.QueryString("idPropuesta").ToString())
            'CargarElemento(estudio)
            'CargarPresupuestosXEstudio(estudio)
            Dim oUsuarios As New US.Usuarios
            Dim oPropuesta As New Propuesta
            Dim infop = oPropuesta.DevolverxID(propuesta)
            Dim oBrief As New Brief
            Dim infob = oBrief.DevolverxID(infop.Brief)
            Dim infou = oUsuarios.UsuarioGet(infob.GerenteCuentas)
            Me.lblPropuesta.Text = infop.Titulo & " del Gerente de Cuentas " & infou.Nombres & " " & infou.Apellidos
            Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/CAP/Cap_Principal.aspx?IdPropuesta=" & propuesta & "&Alternativa=" & Request.QueryString("Alternativa").ToString() & "&GMU=" & Request.QueryString("GMU").ToString() & "&GMO=" & Request.QueryString("GMO").ToString() & "&TipoCalculo=" & Request.QueryString("TipoCalculo").ToString()
            Me.lblNewGM.Text = Request.QueryString("GMU").ToString()
            If Request.QueryString("Fase") IsNot Nothing Then
                Me.hplLink.NavigateUrl = Me.hplLink.NavigateUrl.ToString & "&Fase=" & Request.QueryString("Fase").ToString
            End If
            If Request.QueryString("Metodologia") IsNot Nothing Then
                Me.hplLink.NavigateUrl = Me.hplLink.NavigateUrl.ToString & "&Metodologia=" & Request.QueryString("Metodologia").ToString
            End If
            GenerarHtml()
        Else
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

        Dim oBrief As New Brief
        Dim propuesta As Int64 = Int64.Parse(Request.QueryString("idPropuesta").ToString())

        Dim oPropuesta As New Propuesta
        Dim infop = oPropuesta.DevolverxID(propuesta)
        Dim infob = oBrief.DevolverxID(infop.Brief)

        Dim oUsuarios As New US.Usuarios

        'Quitar los comentarios para que llegue el correo al gerente de cuentas y a Andrés Lozano
        Dim destinatario As String = oUsuarios.UsuarioGet(infob.GerenteCuentas).Email

        Dim destinatarios As New List(Of String)
        destinatarios.Add(destinatario)
        Dim oAutorizados As New IQ.Consultas
        Dim listAutorizados = oAutorizados.ObtenerAutorizadosCambioGrossMargin(Request.QueryString("Unidad").ToString)
        For i As Integer = 0 To listAutorizados.Count - 1
            destinatarios.Add(oUsuarios.UsuarioGet(listAutorizados(i)).Email)
        Next
        'destinatario = oUsuarios.UsuarioGet(oAutorizados.ObtenerAutorizadoCambioGrossMargin(Request.QueryString("Unidad").ToString)).Email
        'destinatarios.Add(destinatario)

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"

        'destinatarios.Add("John.Patino@ipsos.com")
        'destinatarios.Add("nelmaya@gmail.com")
        'destinatarios.Add("carlos.puentes@ipsos.com")
        
        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region
End Class