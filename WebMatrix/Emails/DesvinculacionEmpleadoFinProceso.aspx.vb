Imports System.IO
Imports CoreProject
Imports CoreProject.DesvinculacionEmpleadosDapper
Imports CoreProject.Usuarios

Public Class DesvinculacionEmpleadoFinProceso
    Inherits System.Web.UI.Page

    Private _desvinculacionEmpleadosRepository As DesvinculacionEmpleadosDapper
    Private _emailSender As EnviarCorreo
    Private _usuariosDapper As UsuariosDapper
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        _desvinculacionEmpleadosRepository = New DesvinculacionEmpleadosDapper()
        _emailSender = New EnviarCorreo
        _usuariosDapper = New UsuariosDapper

        If Request.QueryString("idProcesoDesvinculacion") IsNot Nothing Then
            Notificar(Long.Parse(Request.QueryString("idProcesoDesvinculacion")))
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"

    Sub Notificar(desvinculacionEmpleadoId As Long)
        Dim RH_AdminEmpleados = 154
        Dim empleadoDesvincular = _desvinculacionEmpleadosRepository.InformacionEmpleadoPor(desvinculacionEmpleadoId)
        Dim usuariosNotificar = _usuariosDapper.UsuariosAsignadosAlPermiso(RH_AdminEmpleados)

        Dim emails = usuariosNotificar.Select(Function(x) x.Email).ToList()

        Dim contenido As String
        contenido = Html(empleadoDesvincular)
        _emailSender.sendMail(emails, Me.lblAsunto.Text, contenido)

    End Sub
    Private Function Html(empleadoDesvincular As DesvinculacionEmpleadoEmpleadoInfo) As String
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)

        Me.pnlBody.RenderControl(hw)
        Dim contenido As String = sb.ToString

        contenido = contenido.Replace("@NombreEmpleadoADesvincular", empleadoDesvincular.NombreEmpleadoCompleto)
        contenido = contenido & "<br/>"
        Return contenido
    End Function
#End Region
End Class