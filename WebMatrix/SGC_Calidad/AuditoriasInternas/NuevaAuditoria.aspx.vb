Imports System.Web.Script.Services
Imports CoreProject.PlanillaModeracionDapper
Imports CoreProject.PlanillaModeracionDapper.SGC_AuditoriasInternasDapper
Imports CoreProject.Usuarios

Public Class NuevaAuditoria
    Inherits System.Web.UI.Page
    Shared usuariosRepository As New UsuariosDapper()
    Shared SGC_AuditoriasInternasRepository As New SGC_AuditoriasInternasDapper()
    Shared userId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HttpContext.Current.Session("IDUsuario") = 80744027
        userId = HttpContext.Current.Session("IDUsuario")
        'userId = 80744027
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function UsuariosXRol(rolId As Integer)
        Dim datos = usuariosRepository.UsuariosXRol(rolId)
        Return datos
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function Nueva(nuevaAuditoria As NuevaAuditoriaModel) As Integer
        Return SGC_AuditoriasInternasRepository.Add(Map(nuevaAuditoria))
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

    Public Class NuevaAuditoriaModel
        Public Property AuditoriId() As Long
        Public Property AreaAuditada() As String
        Public Property ProcesoAuditado() As String
        Public Property FechaLimiteAuditoria As Date
        Public Property TiposAuditoria As List(Of Short)
        Public Property NormativasAAuditar As List(Of Short)
    End Class
End Class