Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class SolicitudAjustesSimulador
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("idSolicitud") IsNot Nothing And Request.QueryString("idPropuesta") IsNot Nothing Then
            Dim oCot As New CoreProject.Cotizador.General
            Dim solicitud As Int64 = Int64.Parse(Request.QueryString("idSolicitud").ToString())
            Dim infoSolicitud = oCot.GetSolicitudesSimulador(solicitud, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault

            lblCliente.Text = infoSolicitud.Cliente
            lblPropuesta.Text = infoSolicitud.Propuesta & " - " & infoSolicitud.NombrePropuesta
            lblSolicitante.Text = infoSolicitud.SolicitadoPor

            GenerarHtml(infoSolicitud.ValorVenta)
        Else
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub CargarElemento(ByVal EstudioId As Long)
        Dim oAnuncio As New CoreProject.AnuncioAprobacion
        lblAsunto.Text = lblAsunto.Text

    End Sub
    Sub GenerarHtml(ByVal valorventa As Long?)
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
        Dim listAutorizados = oAutorizados.ObtenerAutorizadosCambioGrossMarginTable(0, valorventa)

        For i As Integer = 0 To listAutorizados.Count - 1
            destinatarios.Add(oUsuarios.UsuarioGet(listAutorizados(i).IdUsuario).Email)
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