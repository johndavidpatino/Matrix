Imports CoreProject
Imports System.IO
Public Class Emails_EncuestaAnuladaNotificacion
    Inherits System.Web.UI.Page
#Region "Propiedades"

    Private _idTrabajo As Int64
    Public Property idTrabajo() As Int64
        Get
            Return _idTrabajo
        End Get
        Set(ByVal value As Int64)
            _idTrabajo = value
        End Set
    End Property
    Private _idNoEncuesta As Int16
    Public Property idNoEncuesta() As Int16
        Get
            Return _idNoEncuesta
        End Get
        Set(ByVal value As Int16)
            _idNoEncuesta = value
        End Set
    End Property
#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.QueryString("IdTrabajo") Is Nothing Then
            Int64.TryParse(Request.QueryString("IdTrabajo"), idTrabajo)
        End If
        If Not Request.QueryString("idNoEncuesta") Is Nothing Then
            Int64.TryParse(Request.QueryString("idNoEncuesta"), idNoEncuesta)
        End If
        parametrizarMensaje()

        Dim page As Page = DirectCast(Context.Handler, Page)
        GenerarHtml()

    End Sub
#End Region
#Region "Metodos"

    Sub parametrizarMensaje()
        Dim o As New CorreoAnulacionEncuestas
        Dim info = o.ContenidoCorreo(idTrabajo, idNoEncuesta)
        lblJobBook.Text = info.JobBook
        lblTrabajoId.Text = idTrabajo
        lblTrabajoNombre.Text = info.NombreTrabajo

        lblAsunto.Text = "Matrix." & " " & info.JobBook & " ID: " & idTrabajo & " " & info.NombreTrabajo & " - " & "Encuesta anulada"

        lblCOE.Text = info.COE
        lblCoordinador.Text = info.Coordinador
        lblAnulador.Text = info.Anulador
        lblUnidad.Text = info.Unidad
        lblNumero.Text = idNoEncuesta
        lblRazon.Text = info.Razon
        lblCiudad.Text = info.Ciudad

    End Sub
    Sub ajustarCuerpo()


    End Sub

    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)
        Dim contenido As String
        Dim oEnviarCorreo As New EnviarCorreo
        Dim destinatarios As New List(Of String)

        Dim o As New CorreoAnulacionEncuestas
        For i As Integer = 0 To o.DestinatarioEncuestas(idTrabajo, idNoEncuesta).Count - 1
            destinatarios.Add(o.DestinatarioEncuestas(idTrabajo, idNoEncuesta).Item(i).CORREO.ToString)
        Next

        If Not destinatarios.Count() = 0 Then

        End If
        Me.pnlBody.RenderControl(hw)
        contenido = sb.ToString
        contenido = contenido & "<br/>"
        
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region


End Class