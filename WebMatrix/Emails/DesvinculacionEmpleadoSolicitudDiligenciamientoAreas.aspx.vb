Imports System.IO
Imports CoreProject
Imports CoreProject.DesvinculacionEmpleadosDapper

Public Class DesvinculacionEmpleadoNotificacionArea
    Inherits System.Web.UI.Page

    Private _desvinculacionEmpleadosRepository As DesvinculacionEmpleadosDapper
    Private _emailSender As EnviarCorreo
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        _desvinculacionEmpleadosRepository = New DesvinculacionEmpleadosDapper()
        _emailSender = New EnviarCorreo

        If Request.QueryString("idProcesoDesvinculacion") IsNot Nothing Then
            Notificar(Long.Parse(Request.QueryString("idProcesoDesvinculacion")))
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"

    Sub Notificar(desvinculacionEmpleadoId As Long)
        Dim areasAprueban = _desvinculacionEmpleadosRepository.DesvinculacionesEstatusEvaluacionesPor(desvinculacionEmpleadoId)
        Dim empleadoDesvincular = _desvinculacionEmpleadosRepository.InformacionEmpleadoPor(desvinculacionEmpleadoId)

        For Each area As TH_DesvinculacionEmpleadosEstatusEvaluacionPorDesvinculacion In areasAprueban
            Dim contenido As String
            contenido = Html(empleadoDesvincular)
            _emailSender.sendMail(area.EmailsEvaluadores.Split(",").ToList(), Me.lblAsunto.Text, contenido)
        Next

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