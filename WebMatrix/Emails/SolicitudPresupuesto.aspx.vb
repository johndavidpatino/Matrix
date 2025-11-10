Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class Emails_SolicitudPresupuesto
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("TrabajoId") IsNot Nothing Then
            GenerarHtml()
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
        Dim op As New PresupInt

        Dim TrabajoId As Int64 = Int64.Parse(Request.QueryString("TrabajoId").ToString())
        Dim o As New GestionTrabajosOP


        Me.lblObservacion.Text = op.ObservacionPresupuestoGet(TrabajoId).LastOrDefault.Observacion

        Dim oTrabajo As New Trabajo
        Dim info = oTrabajo.obtenerXId(TrabajoId)
        lblAsunto.Text = "Matrix." & " " & info.JobBook & " ID: " & info.id & " " & info.NombreTrabajo & " - Solicitud Presupuesto"
        Me.lblTrabajo.Text = info.NombreTrabajo

        Me.lblcoe.Text = info.NombreCOE

        Me.pnlBody.RenderControl(hw)

        Dim oUsuarios As New US.Usuarios

        Dim destinatarios As New List(Of String)
        For i As Integer = 0 To oUsuarios.UsuarioCorreoPresupuestos(TrabajoId).Count - 1
            destinatarios.Add(oUsuarios.UsuarioCorreoPresupuestos(TrabajoId).Item(i).CORREO)
        Next
        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"

        Dim oEnviarCorreo As New EnviarCorreo

        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region
End Class