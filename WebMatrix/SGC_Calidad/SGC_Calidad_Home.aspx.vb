Imports CoreProject.EmpleadosDapper
Imports CoreProject.PlanillaModeracionDapper
Imports CoreProject.Usuarios

Public Class SGC_Calidad_Home
    Inherits System.Web.UI.Page
    Shared usuariosRepository As New UsuariosDapper()
    Shared SGC_AuditoriasInternasRepository As New SGC_AuditoriasInternasDapper()
    Shared EmpleadosRepository = New EmpleadosDapper()
    Shared userId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class
