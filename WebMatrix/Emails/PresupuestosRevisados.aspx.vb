Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo


Public Class PresupuestosRevisados
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim propuestaId As Int64
        Dim alternativa As Int32
        Dim metCodigo As Int32
        Dim parNacional As Int32
        If Request.QueryString("PropuestaId") IsNot Nothing Then
            propuestaId = Int64.Parse(Request.QueryString("PropuestaId").ToString())
        Else
            Exit Sub
        End If

        If Request.QueryString("Alternativa") IsNot Nothing Then
            alternativa = Int32.Parse(Request.QueryString("Alternativa").ToString())
        Else
            Exit Sub
        End If

        If Request.QueryString("MetCodigo") IsNot Nothing Then
            metCodigo = Int32.Parse(Request.QueryString("MetCodigo").ToString())
        Else
            Exit Sub
        End If

        If Request.QueryString("ParNacional") IsNot Nothing Then
            parNacional = Int32.Parse(Request.QueryString("ParNacional").ToString())
        Else
            Exit Sub
        End If

        Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/CU_Cuentas/Propuestas.aspx"
        CargarPresupuesto(propuestaId, alternativa, metCodigo, parNacional)
        GenerarHtml()

    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub CargarPresupuesto(ByVal idPropuesta As Int64, ByVal parAlternativa As Int32, ByVal metCodigo As Int32, ByVal parNacional As Int32)
        Dim _Cati As New IQ.Cati()
        Dim info = _Cati.ObtenerPresupuestosxPropuesta(idPropuesta, parAlternativa, metCodigo, parNacional)
        lblAsunto.Text = lblAsunto.Text
        lblFase.Text = info.Fase
        lblMetodología.Text = info.Metodologia
        lblVentaOPS.Text = FormatCurrency(info.VentaOperaciones, 0)
        lblValorVenta.Text = FormatCurrency(info.ValorVenta, 0)
        lblGross.Text = info.GrossMargin & "%"
    End Sub
    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)

        Dim estudio As Int64 = Int64.Parse(Request.QueryString("PropuestaId").ToString())
        Dim oPropuesta As New Propuesta
        Dim oBrief As New Brief
        Dim info = oPropuesta.DevolverxID(estudio)
        Dim infob = oBrief.DevolverxID(info.Brief)
        Me.lblTitulo.Text = "No. " & info.Id.ToString & " " & info.Titulo
        Me.lblAsunto.Text = Me.lblAsunto.Text & " propuesta No. " & info.Id.ToString & " " & info.Titulo
        Dim oUsuarios As New US.Usuarios

        Dim destinatarios As New List(Of String)

        Dim ousr As New US.Usuarios
        Dim usr As US_Usuarios

        usr = ousr.UsuarioGet(infob.GerenteCuentas)

        destinatarios.Add(usr.Email)

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