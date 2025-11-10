Imports System.IO
Imports System.Net
Imports System.Web.Script.Services
Imports CoreProject.EmpleadosDapper
Imports CoreProject.PlanillaModeracionDapper
Imports CoreProject.PlanillaModeracionDapper.SGC_AuditoriasInternasDapper
Imports CoreProject.Usuarios
Imports WebMatrix.NuevaAuditoria

Public Class Auditor
    Inherits System.Web.UI.Page
    Shared usuariosRepository As New UsuariosDapper()
    Shared SGC_AuditoriasInternasRepository As New SGC_AuditoriasInternasDapper()
    Shared EmpleadosRepository = New EmpleadosDapper()
    Shared userId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        userId = HttpContext.Current.Session("IDUsuario")
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function AuditoriasBy(estadoId As Integer?, anoAuditoria As Integer?, pageIndex As Integer, pageSize As Integer) As List(Of SGC_AuditoriaInternaEntity)
        If UsuarioTieneRolCalidad() Then
            Return SGC_AuditoriasInternasRepository.By(estadoId, Nothing, anoAuditoria, Nothing, pageIndex, pageSize)
        End If

        If UsuarioTieneRolAuditor() Then
            Return SGC_AuditoriasInternasRepository.By(estadoId, userId, anoAuditoria, Nothing, pageIndex, pageSize)
        End If

        Return SGC_AuditoriasInternasRepository.By(estadoId, Nothing, anoAuditoria, userId, pageIndex, pageSize)

    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function EmpleadosActivos() As List(Of EmpleadosDapper.EmpleadosActivosoResult)
        Try
            Return EmpleadosRepository.EmpleadosActivos()
        Catch ex As Exception
            Return New List(Of EmpleadosDapper.EmpleadosActivosoResult)()
        End Try
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Sub AuditoriaInformeAuditorAdd(auditoriaInforme As AuditoriaModel)
        Try
            Dim file = Convert.FromBase64String(auditoriaInforme.ArchivoEvidencia.ArchivoBase64)
            Dim guidFile = Guid.NewGuid().ToString()
            Dim filePath = HttpContext.Current.Server.MapPath("~/Files/SGC_AuditoriasInternas/InformesAuditores/" & guidFile & Path.GetExtension(auditoriaInforme.ArchivoEvidencia.NombreArchivoConExtension))

            System.IO.File.WriteAllBytes(filePath, file)

            SGC_AuditoriasInternasRepository.InformeAuditor(
                auditoriaInforme.AuditoriaId,
                auditoriaInforme.FechaAuditoria,
                auditoriaInforme.Fortalezas,
                auditoriaInforme.Auditados.ToList().Select(Function(x) x.AuditadoId).ToList(),
                auditoriaInforme.Hallazgos.ToList().Select(Function(x) MapHallazgo(x)).ToList(),
                auditoriaInforme.ArchivoEvidencia.NombreArchivoConExtension,
                guidFile,
                file.Length,
                userId,
                DateTime.Now()
                )
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
        End Try
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function Nueva(nuevaAuditoria As NuevaAuditoriaModel) As Integer
        Return SGC_AuditoriasInternasRepository.Add(Map(nuevaAuditoria))
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function UsuarioTieneRolCalidad() As Boolean
        Dim ROL_CALIDAD = 45
        Return usuariosRepository.UsuarioTieneRol(userId, ROL_CALIDAD)
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function InformeAuditorBy(auditoriaId As Integer) As Auditoria_InformeAuditorResultModel
        Try
            Dim informeAuditor = SGC_AuditoriasInternasRepository.InformeAuditorBy(auditoriaId)

            If (informeAuditor Is Nothing) Then
                Return Nothing
            End If

            Dim hallazgos = SGC_AuditoriasInternasRepository.InformeAuditor_HallazgosBy(auditoriaId)
            Dim auditados = SGC_AuditoriasInternasRepository.InformeAuditor_AuditadosBy(auditoriaId)
            Return Map(informeAuditor, hallazgos, auditados)
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
        End Try
    End Function
    Private Shared Function UsuarioTieneRolAuditor() As Boolean
        Dim ROL_AUDITOR = 57
        Return usuariosRepository.UsuarioTieneRol(userId, ROL_AUDITOR)
    End Function
    Private Shared Function Map(informeAuditor As SGC_AI_Auditorias_InformeAuditorEntity, hallazgos As List(Of SGC_AI_HallazgoResult), auditados As List(Of SGC_AI_AuditadoResult)) As Auditoria_InformeAuditorResultModel

        Return New Auditoria_InformeAuditorResultModel With
            {
            .AuditoriaId = informeAuditor.SGC_AI_AuditoriaId,
            .FechaAuditoria = informeAuditor.FechaAuditoria,
            .FechaRegistro = informeAuditor.FechaRegistro,
            .Fortalezas = informeAuditor.Fortalezas,
            .PathFileEvidencia = "/Files/SGC_AuditoriasInternas/InformesAuditores/" & informeAuditor.ArchivoInformeAuditoriaId & ".xlsx",
            .Auditados = auditados.Select(Function(x) New Auditoria_InformeAuditor_AuditadoResultModel With {
            .Id = x.AuditadoId,
            .Nombres = x.Nombres,
            .Apellidos = x.Apellidos,
            .Activo = x.activo
            }).ToList(),
            .Hallazgos = hallazgos.Select(Function(x) New Auditoria_InformeAuditor_HallazgoResultModel With {
            .Id = x.Id,
            .Hallazgo = x.Hallazgo,
            .TipoHallazgoId = x.SGC_AI_TipoHallazgoId,
            .TipoHallazgo = x.TipoHallazgo
            }).ToList()
            }

    End Function
    Private Shared Function Map(auditoria As NuevaAuditoriaModel)
        Return New SGC_AuditoriaInterna() With {
            .AuditorId = auditoria.AuditoriId,
            .AreaAuditada = auditoria.AreaAuditada,
            .ProcesoAuditado = auditoria.ProcesoAuditado,
            .FechaLimiteAuditoria = auditoria.FechaLimiteAuditoria,
            .FechaRegistro = DateTime.Now(),
            .UsuarioRegistraId = userId,
            .TiposAuditoria = auditoria.TiposAuditoria,
            .NormativasAAuditar = auditoria.NormativasAAuditar
            }

    End Function
    Private Shared Function MapHallazgo(hallazgo As HallazgosModel) As SGC_AI_Hallazgo
        Return New SGC_AI_Hallazgo With {.Hallazgo = hallazgo.Hallazgo, .TipoId = hallazgo.TipoId}
    End Function
    Public Class AuditoriaModel
        Property AuditoriaId As Integer
        Property ArchivoEvidencia As ArchivoEvidencia
        Property FechaAuditoria As Date
        Property Fortalezas As String
        Property Auditados As IEnumerable(Of AuditadoModel)
        Property Hallazgos As IEnumerable(Of HallazgosModel)

    End Class
    Public Class AuditadoModel
        Property AuditadoId As Long
    End Class
    Public Class HallazgosModel
        Property Hallazgo As String
        Property TipoId As String
    End Class
    Public Class ArchivoEvidencia
        Property NombreArchivoConExtension As String
        Property ArchivoBase64 As String
    End Class
    Public Class NuevaAuditoriaModel
        Public Property AuditoriId() As Long
        Public Property AreaAuditada() As String
        Public Property ProcesoAuditado() As String
        Public Property FechaLimiteAuditoria As Date
        Public Property TiposAuditoria As List(Of Short)
        Public Property NormativasAAuditar As List(Of Short)
    End Class
    Public Class Auditoria_InformeAuditorResultModel
        Property AuditoriaId As Integer
        Property FechaAuditoria As Date
        Property Fortalezas As String
        Property FechaRegistro As DateTime
        Property PathFileEvidencia As String
        Property Auditados As List(Of Auditoria_InformeAuditor_AuditadoResultModel)

        Property Hallazgos As List(Of Auditoria_InformeAuditor_HallazgoResultModel)
    End Class
    Public Class Auditoria_InformeAuditor_AuditadoResultModel
        Property Id As Integer
        Property Nombres As String
        Property Apellidos As String
        Property Activo As Boolean
    End Class
    Public Class Auditoria_InformeAuditor_HallazgoResultModel
        Property Id As Integer
        Property TipoHallazgoId As Short
        Property TipoHallazgo As String
        Property Hallazgo As String

    End Class
End Class
