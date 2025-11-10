

'Imports CoreProject.MatrixModel
Public Class CT_Tareas
#Region "Variables Globales"
    Private oMatrixContext As CORE_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New CORE_Entities
    End Sub
#End Region
#Region "Obtener"
    Public Function TareasList(ByVal WFId As Int64?, TrabajoId As Int64?, RolEstima As Int64?, UnidadEjecuta As Int64?, EstadoNot As Int32?, UsuarioAsignado As Int64?, Estado As Int32?, FIniP As Date?, FFinP As Date?, FIniR As Date?, FFinR As Date?) As List(Of CT_TareasList_Result)
        Return oMatrixContext.CT_TareasList(WFId, TrabajoId, RolEstima, UnidadEjecuta, EstadoNot, UsuarioAsignado, Estado, FIniP, FFinP, FIniR, FFinR).ToList

    End Function

    Public Function TareasPrevias(ByVal WFId As Int64?, ByVal TrabajoId As Int64?, ByVal TipoHilo As Int64?) As List(Of CT_TareasList_Result)
        Return oMatrixContext.CT_TareasPrevias(TipoHilo, WFId, TrabajoId).ToList
    End Function

    Public Function ObtenerWorkFlow(ByVal WFId As Int64) As CORE_WorkFlow
        Return oMatrixContext.CORE_WorkFlow.Where(Function(x) x.id = WFId).FirstOrDefault
    End Function

    Public Function TareasTrabajosList(ByVal UsuarioId As Int64, ByVal Estado As Int16?) As List(Of CT_TareasTrabajosList_Result)
        Return oMatrixContext.CT_TareasTrabajosList(UsuarioId, Estado).ToList
    End Function

    Public Function TareasPendientesAsignacion(ByVal WFId As Int64?, ByVal TrabajoId As Int64?, ByVal RolEstima As Int64?, ByVal UnidadEjecuta As Int64?, ByVal EstadoNot As Int32?, ByVal UsuarioAsignado As Int64?, ByVal Estado As Int32?) As List(Of CT_TareasList_Result)
        Return oMatrixContext.CT_TareasListPendientesAsignacion(WFId, TrabajoId, RolEstima, UnidadEjecuta, EstadoNot, UsuarioAsignado, Estado).ToList
    End Function

    Public Function UnidadesAsignacion(ByVal UsuarioId As Int64) As List(Of CT_UnidadesAsignacionXUsuario_Result)
        Return oMatrixContext.CT_UnidadesAsignacionXUsuario(UsuarioId).ToList
    End Function

#End Region

#Region "Guardar"
    Public Sub GuardarWorkFlow(ByRef ent As CORE_WorkFlow)
        oMatrixContext.SaveChanges()
    End Sub

    Public Sub GuardarWorkFlowContext()
        oMatrixContext.SaveChanges()
    End Sub
#End Region

End Class
