Imports CoreProject
Imports System.IO
Public Class Emails_CircularCargada
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
#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.QueryString("IdTrabajo") Is Nothing Then
            Int64.TryParse(Request.QueryString("IdTrabajo"), idTrabajo)
        End If
        parametrizarMensaje()

        Dim page As Page = DirectCast(Context.Handler, Page)
        GenerarHtml()

    End Sub
#End Region
#Region "Metodos"

    Sub parametrizarMensaje()
        Dim o As New Trabajo
        Dim info = o.DevolverxID(idTrabajo)
        lblJobBook.Text = info.JobBook
        lblTrabajoId.Text = idTrabajo
        lblTrabajoNombre.Text = info.NombreTrabajo

        lblAsunto.Text = "Matrix." & " " & info.JobBook & " ID: " & idTrabajo & " " & info.NombreTrabajo & " - " & "Nueva Circular"

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

        Dim o As New DestinatariosCorreos
        Dim oUsuarios As New US.Usuarios


        For i As Integer = 0 To o.DestinatariosCreacionyEspecificacionesTrabajo(idTrabajo).Count - 1
            destinatarios.Add(o.DestinatariosCreacionyEspecificacionesTrabajo(idTrabajo).Item(i).CORREO)
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