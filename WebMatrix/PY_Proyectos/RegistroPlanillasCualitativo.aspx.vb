Imports System.Net
Imports System.Web.Script.Services
Imports CoreProject
Imports CoreProject.ExternalServices.BIServices.BIService
Imports CoreProject.PlanillaModeracionDapper
Imports CoreProject.Usuarios
Imports Utilidades

Public Class PlanillaModeracion
    Inherits System.Web.UI.Page

    Shared PlanillaModeracionRepository As New PlanillaModeracionDapper()
    Shared UsuariosRepository As New UsuariosDapper()
    Shared EncriptionService As New Encripcion()
    Shared BIRequestService As New CoreProject.ExternalServices.BIServices.RequestService(ConfigurationManager.AppSettings("BI_API_URLBase"))
    Shared BIService As New CoreProject.ExternalServices.BIServices.BIService(BIRequestService)

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function Tecnicas(TipoTecnica As String) As List(Of TecnicaModel)
        Dim usuario = HttpContext.Current.Session("IDUsuario")
        Try
            Return PlanillaModeracionRepository.GetTecnicas(TipoTecnica)
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function Moderadores() As List(Of ModeradorModel)
        Try
            Return PlanillaModeracionRepository.GetModeradores()
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function GerentesCuentasUU() As List(Of ModeradorModel)
        Const UNIDAD_UU As Int16 = 17
        Const ROL_CUENTAS As Int16 = 5

        Try
            Return UsuariosRepository.UsuariosxUnidadXrol(UNIDAD_UU, ROL_CUENTAS).Select(Of ModeradorModel)(Function(x) Map(x)).ToList()
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function SavePlanillaModeracion(IdJob As String, jobDesc As String, fecha As String, hora As String, tecnica As Integer, tiempo As String, moderador As Integer, rol As String, Observaciones As String, IdCuentasUU As Integer, ServiceLineName As String) As String
        Try
            Dim usuarioActualId As Long = Long.Parse(HttpContext.Current.Session("IDUsuario").ToString())
            Dim planillaModeracion As New PlanillaModeracionModel()
            planillaModeracion.IdJob = IdJob
            planillaModeracion.jobDesc = jobDesc
            planillaModeracion.fecha = Convert.ToDateTime(fecha)
            planillaModeracion.hora = hora
            planillaModeracion.tecnica = tecnica
            planillaModeracion.tiempo = tiempo
            planillaModeracion.moderador = moderador
            planillaModeracion.rol = rol
            planillaModeracion.idUsuarioRegistro = usuarioActualId
            planillaModeracion.Observaciones = Observaciones
            planillaModeracion.IdCuentasUU = IdCuentasUU
            planillaModeracion.BI_WBSL = ServiceLineName

            PlanillaModeracionRepository.SavePlanillaModeracion(planillaModeracion)
            Return "Ok"
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function SaveStatusAprobacionModerarion(idPlanilla As Integer, idJob As String, idEstado As Short, observaciones As String, biStatus As String, biDinero As String) As String
        Try
            Dim usuarioActualId As Long = Long.Parse(HttpContext.Current.Session("IDUsuario").ToString())
            Dim jobBookModeracion = BIService.JobBookModerationInfo(idJob)
            Dim JobEncontradoEnBI As Boolean = False

            If (jobBookModeracion.IsSuccess AndAlso Not jobBookModeracion.Data Is Nothing) Then
                JobEncontradoEnBI = True
            End If
            PlanillaModeracionRepository.UpdatePlanillaModeracion(idPlanilla, idEstado, observaciones, biDinero, biStatus, usuarioActualId, DateTime.UtcNow.AddHours(-5), JobEncontradoEnBI)
            Return "Ok"
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function PlanillaModeracionBy(idPlanilla As Integer) As PlanillaModeracionListModel
        Try
            Dim planilla = PlanillaModeracionRepository.GetPlanillasModeracionBy(idPlanilla)
            Dim jobBook = Long.Parse(planilla.IdJob.Replace("-", ""))

            If Not String.IsNullOrWhiteSpace(planilla.BI_Status) Then
                Return planilla
            End If

            Dim jobBookModeracion = BIService.JobBookModerationInfo(jobBook)

            If (jobBookModeracion.IsSuccess AndAlso Not jobBookModeracion.Data Is Nothing) Then
                planilla.BI_WBSL = jobBookModeracion.Data.ServicelineName
                planilla.BI_3320_Moderacion_DineroDisponible = jobBookModeracion.Data.Budget_3320_Moderation
                planilla.BI_Status = jobBookModeracion.Data.StatusProject
            Else
                planilla.BI_DefaultMessage = ConfigurationManager.AppSettings.Get("BIDefaultMessage")
            End If
            Return planilla
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function SavePlanillaInformes(IdJob As String, jobDesc As String, fecha As String, tecnica As Integer, muestra As String, IdCuentasUU As Integer, analista As Integer, Observaciones As String, ServiceLineName As String) As String
        Try
            Dim usuarioActualId As Long = Long.Parse(HttpContext.Current.Session("IDUsuario").ToString())
            Dim planillaInformes As New PlanillaInformesModel()
            planillaInformes.IdJob = IdJob
            planillaInformes.jobDesc = jobDesc
            planillaInformes.fecha = Convert.ToDateTime(fecha)
            planillaInformes.tecnica = tecnica
            planillaInformes.muestra = muestra
            planillaInformes.IdCuentasUU = IdCuentasUU
            planillaInformes.analista = analista
            planillaInformes.Observaciones = Observaciones
            planillaInformes.idUsuarioRegistro = usuarioActualId
            planillaInformes.ServiceLineName = ServiceLineName

            PlanillaModeracionRepository.SavePlanillaInformes(planillaInformes)
            Return "Ok"
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function PlanillaInformeBy(idPlanilla As Integer) As PlanillaInformesListModel
        Try
            Dim planilla = PlanillaModeracionRepository.GetPlanillasInformesBy(idPlanilla)
            Dim jobBook = Long.Parse(planilla.IdJob.Replace("-", ""))

            If Not String.IsNullOrWhiteSpace(planilla.BI_Status) Then
                Return planilla
            End If

            Dim jobBookModeracion = BIService.JobBookModerationInfo(jobBook)

            If (jobBookModeracion.IsSuccess AndAlso Not jobBookModeracion.Data Is Nothing) Then
                planilla.BI_WBSL = jobBookModeracion.Data.ServicelineName
                planilla.BI_3320_Moderacion_DineroDisponible = jobBookModeracion.Data.Budget_3320_Moderation
                planilla.BI_Status = jobBookModeracion.Data.StatusProject
            Else
                planilla.BI_DefaultMessage = ConfigurationManager.AppSettings.Get("BIDefaultMessage")
            End If
            Return planilla
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function


    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function SaveStatusAprobacionInformes(idPlanilla As Integer, idJob As String, idEstado As Short, observaciones As String, biStatus As String, biDinero As String) As String
        Try
            Dim usuarioActualId As Long = Long.Parse(HttpContext.Current.Session("IDUsuario").ToString())
            Dim jobBookModeracion = BIService.JobBookModerationInfo(idJob)
            Dim JobEncontradoEnBI As Boolean = False

            If (jobBookModeracion.IsSuccess AndAlso Not jobBookModeracion.Data Is Nothing) Then
                JobEncontradoEnBI = True
            End If
            PlanillaModeracionRepository.UpdatePlanillaInformes(idPlanilla, idEstado, observaciones, biDinero, biStatus, usuarioActualId, DateTime.UtcNow.AddHours(-5), JobEncontradoEnBI)
            Return "Ok"
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function


    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function PlanillasGet(pageSize As Integer, pageIndex As Integer, filtroPlanilla As String, idEstado As Short?) As List(Of PlanillaRegistrosListModel)
        Try
            Return PlanillaModeracionRepository.GetPlanillas(pageSize, pageIndex, filtroPlanilla, idEstado)
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function

    <Services.WebMethod()>
    <ScriptMethod()>
    Public Shared Function ExportExcelPlanillasBy(fechaInicio As String, fechaFinal As String, tipoPlanilla As String) As String
        Try
            Dim response As HttpResponse = HttpContext.Current.Response
            Dim nombreColumnas As String
            Dim fechaInicioAsDate As DateTime
            Dim fechaFinalAsDate As DateTime
            Dim fechaFormato As String = "dd/MM/yyyy"
            Dim nombreArchivo As String = ""
            Dim Planillas As IEnumerable(Of Object)

            fechaInicioAsDate = DateTime.ParseExact(fechaInicio, fechaFormato, System.Globalization.CultureInfo.InvariantCulture)
            fechaFinalAsDate = DateTime.ParseExact(fechaFinal, fechaFormato, System.Globalization.CultureInfo.InvariantCulture)

            If tipoPlanilla = "Moderacion" Then
                nombreColumnas = "FechaRegistro;Usuarioregistro;JobBook;Job Description;SL;Estado BI;Dinero BI;Cuentas UU;Tecnica;Puntos;Tiempo;Moderador;Rol;Fecha de actividad;Hora;Observaciones;Observaciones Aprobación;Status"
                Planillas = PlanillaModeracionRepository.GetPlanillasModeracionToExport(fechaInicioAsDate, fechaFinalAsDate).ToList
                nombreArchivo = "ExportacionPlanillas_" + tipoPlanilla + "_del_" + fechaInicio + "_al_" + fechaFinal
            Else
                nombreColumnas = "FechaRegistro;Usuarioregistro;JobBook;Job Description;SL;Estado BI;Dinero BI;Cuentas UU;Tecnica;Puntos;Muestra;Analista;Fecha de actividad;Observaciones;Observaciones Aprobación;Status"
                Planillas = PlanillaModeracionRepository.GetPlanillasInformesToExport(fechaInicioAsDate, fechaFinalAsDate).ToList
                nombreArchivo = "ExportacionPlanillas_" + tipoPlanilla + "_del_" + fechaInicio + "_al_" + fechaFinal
            End If

            Dim excel As New Utilidades.ResponseExcel
            Dim columnasExcel = nombreColumnas.Split(";")

            Dim excelBase64 = excel.CreateExcelToBase64(tipoPlanilla, columnasExcel, Planillas)
            Return excelBase64
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function GetJobsBy(termToSearch As String) As List(Of JobModel)
        Try
            Dim jobs = BIService.JobsBySearchValue(3, 1000, 1, termToSearch)
            Return jobs
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function
    Private Shared Function Map(usuario As Usuarios_Result) As ModeradorModel
        Return New ModeradorModel With {
            .Id = usuario.id,
            .NombreModerador = usuario.Nombres + " " + usuario.Apellidos
        }
    End Function

End Class
