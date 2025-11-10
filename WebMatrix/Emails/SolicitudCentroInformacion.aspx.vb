Imports CoreProject
Imports System.IO
Imports CoreProject.CIEntities
Imports CoreProject.OP
Imports WebMatrix.EnviarCorreo

Public Class SolicitudCentroInformacionMail
    Inherits System.Web.UI.Page


#Region "Propiedades"
    Private _solicitud As CI_SolicitudMedios_Get_Result

    Public Property solicitud() As CI_SolicitudMedios_Get_Result
        Get
            Return _solicitud
        End Get
        Set(ByVal value As CI_SolicitudMedios_Get_Result)
            _solicitud = value
        End Set
    End Property
    
#End Region


#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim IdSolicitud As Decimal
        Dim esQueryValid As Boolean = True
        Dim daCentro As New CentroInformacion

        If Request.QueryString("IdSolicitud") Is Nothing Then
            esQueryValid = False
        ElseIf Int64.TryParse(Request.QueryString("IdSolicitud"), IdSolicitud) = False Then
            esQueryValid = False
        End If

        If esQueryValid = False Then
            Exit Sub
        Else
            solicitud = daCentro.obtenersolicitudxid(IdSolicitud)
            GenerarHtml()
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"

    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)

        Dim oUsuarios As New US.Usuarios
        Dim destinatarios As New List(Of String)

        Me.lblSolicitud.Text = solicitud.Id

        Me.lblMedio.Text = solicitud.IdMedio

        Me.lblUsuarioSolicita.Text = solicitud.UsuarioSolicita

        destinatarios.Add("Juan.Garcia@ipsos.com")
        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"

        'destinatarios.Add("Sammy.Ariza@ipsos.com")
        'destinatarios.Add("John.Patino@ipsos.com")
        'destinatarios.Add("Cesar.Verano@ipsos.com")

        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub

#End Region
End Class