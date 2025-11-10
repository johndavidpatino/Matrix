Imports System.IO
Imports System.Net
Imports System.Web.Script.Services
Imports System.Web.Services
Imports CoreProject
Imports CoreProject.DesvinculacionEmpleadosDapper
Imports CoreProject.EmpleadosDapper

Public Class DesvinculacionesEmpleadosGestionRRHH
    Inherits System.Web.UI.Page

    Shared empleadosRepository As New EmpleadosDapper()
    Shared DesvinculacionEmpleados As New DesvinculacionEmpleadosDapper()
    Shared emailSender As New EnviarCorreo()

    Private Sub DesvinculacionesEmpleadosGestionRRHH_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())

        If permisos.VerificarPermisoUsuario(154, UsuarioID) = False Then
            Response.Redirect("../Home/Default.aspx")
        End If

    End Sub

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function DesvinculacionesEmpleadosEstatus(pageSize As Integer, pageIndex As Integer, textoBuscado As String) As List(Of TH_DesvinculacionEmpleadosEstatus)
        Try
            Return DesvinculacionEmpleados.DesvinculacionesResumenGeneral(pageIndex:=pageIndex, pageSize:=pageSize, textoBuscado:=textoBuscado)
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function EmpleadosActivos() As List(Of EmpleadosDapper.EmpleadosActivosoResult)
        Try
            empleadosRepository = New EmpleadosDapper()
            Dim empleados = empleadosRepository.EmpleadosActivos()
            Return empleados
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function IniciarProcesoDesvinculacion(ByVal empleadoId As Integer, fechaRetiro As Date, motivosDesvinculacion As String) As String
        If String.IsNullOrWhiteSpace(motivosDesvinculacion) Then
            HttpContext.Current.Response.StatusCode = HttpStatusCode.BadRequest
            Return $"'{NameOf(motivosDesvinculacion)}' cannot be null or whitespace."
        End If

        Dim usuarioActualId As Long = Long.Parse(HttpContext.Current.Session("IDUsuario").ToString())
        Dim desvinculacionEmpleadoId = DesvinculacionEmpleados.DesvinculacionAdd(empleadoId, fechaRetiro, motivosDesvinculacion, Date.UtcNow.AddHours(-5), usuarioActualId)
        emailSender.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/DesvinculacionEmpleadoSolicitudDiligenciamientoAreas.aspx?idProcesoDesvinculacion=" & desvinculacionEmpleadoId)
        Return "Proceso de desvinculación iniciado correctamente."
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function DesvinculacionEmpleadosEstatusEvaluacionesPor(ByVal desvinculacionEmpleadoId As Integer) As IList(Of TH_DesvinculacionEmpleadosEstatusEvaluacionPorDesvinculacion)
        Return DesvinculacionEmpleados.DesvinculacionesEstatusEvaluacionesPor(desvinculacionEmpleadoId)
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function PDFFormato(ByVal desvinculacionEmpleadoId As Integer) As String
        Dim htmlTemplate As String = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Resources/TH_DesvinculacionEmpleados/TemplateFormatoDesvinculacion.html"))
        Dim htmlTemplateEvaluation As String
        Dim infoEmpleado = DesvinculacionEmpleados.InformacionEmpleadoPor(desvinculacionEmpleadoId)

        htmlTemplate = htmlTemplate.Replace("@EmployeeName", infoEmpleado.NombreEmpleadoCompleto)
        htmlTemplate = htmlTemplate.Replace("@IdentificacionNumber", infoEmpleado.EmpleadoId)
        htmlTemplate = htmlTemplate.Replace("@Position", infoEmpleado.Cargo)
        htmlTemplate = htmlTemplate.Replace("@DepartureDate", infoEmpleado.FechaRetiro)

        htmlTemplateEvaluation = "<div class=""evaluation""><div class=""title"">@TitleEvaluation</div><div>Observaciones:</div><div>@Comments</div><div>Evaluador:</div><div>@Evaluator</div><div>Fecha evaluación:</div><div>@DateEvaluation</div></div>"
        Dim evaluaciones = DesvinculacionEmpleados.DesvinculacionesEstatusEvaluacionesPor(desvinculacionEmpleadoId)
        Dim htmlEvaluaciones As String = ""

        For Each evaluacion As TH_DesvinculacionEmpleadosEstatusEvaluacionPorDesvinculacion In evaluaciones
            Dim htmlEvaluacion = htmlTemplateEvaluation
            htmlEvaluacion = htmlEvaluacion.Replace("@TitleEvaluation", evaluacion.NombreArea)
            htmlEvaluacion = htmlEvaluacion.Replace("@Comments", evaluacion.Comentarios)
            htmlEvaluacion = htmlEvaluacion.Replace("@Evaluator", evaluacion.NombreEvaluadorCompleto)
            htmlEvaluacion = htmlEvaluacion.Replace("@DateEvaluation", String.Format("{0:dd/MM/yyyy HH:mm}", evaluacion.FechaDiligenciamiento))
            htmlEvaluaciones += htmlEvaluacion
        Next
        htmlTemplate = htmlTemplate.Replace("@EvaluationsContent", htmlEvaluaciones)
        Dim HTMLToPDFGenerator = New HTMLToPDFGenerator()
        Dim pdfBase64 = HTMLToPDFGenerator.Convert(htmlTemplate)
        Return pdfBase64
    End Function

End Class
