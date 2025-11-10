

''Imports CoreProject.MatrixModel

Public Class TareasPrevias
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
    Function CORE_TareasPrevias_Get(ByVal tipoHiloId As Int64, ByVal tareaId As Int64, ByVal trabajoId As Int64) As List(Of CORE_WorkFlow_TareasPrevias_Get_Result)
        Return oMatrixContext.CORE_WorkFlow_TareasPrevias_Get(tipoHiloId, tareaId, trabajoId).ToList
    End Function
    Function CORE_Configuracion_Tareas_Previas(ByVal tareaId As Int64, ByVal TipoHiloId As Int64, ByVal Asignada As Boolean?) As List(Of CORE_Configuracion_Tareas_Previas_Result)
        Return oMatrixContext.CORE_Configuracion_Tareas_Previas(tareaId, TipoHiloId, Asignada).ToList
    End Function
#End Region
#Region "Grabar"
    Sub grabar(ByVal tareaId As Int64, ByVal tipoHiloId As Int64, ByVal tareaPreviaId As Int64)
        Dim oCORE_TareasPrevias As New CORE_TareasPrevias
        Dim oCORE_TipoHilo_Tareas As New CORE_TipoHilo_Tareas

        If oMatrixContext.CORE_TipoHilo_Tareas.Where(Function(x) x.TareaId = tareaId AndAlso x.TipoHiloId = tipoHiloId).Count = 0 Then
            oCORE_TipoHilo_Tareas.TareaId = tareaId
            oCORE_TipoHilo_Tareas.TipoHiloId = tipoHiloId
            oMatrixContext.CORE_TipoHilo_Tareas.Add(oCORE_TipoHilo_Tareas)
        End If

        If oMatrixContext.CORE_TipoHilo_Tareas.Where(Function(x) x.TareaId = tareaPreviaId AndAlso x.TipoHiloId = tipoHiloId).Count = 0 Then
            oCORE_TipoHilo_Tareas.TareaId = tareaPreviaId
            oCORE_TipoHilo_Tareas.TipoHiloId = tipoHiloId
            oMatrixContext.CORE_TipoHilo_Tareas.Add(oCORE_TipoHilo_Tareas)
        End If

        oMatrixContext.SaveChanges()

        oCORE_TareasPrevias.TareaId = tareaId
        oCORE_TareasPrevias.TareaPreviaId = tareaPreviaId
        oCORE_TareasPrevias.TipoHiloId = tipoHiloId

        oMatrixContext.CORE_TareasPrevias.Add(oCORE_TareasPrevias)

        oMatrixContext.SaveChanges()

    End Sub
#End Region
#Region "Eliminar"
    Sub Eliminar(ByVal id As Int64)
        Dim oCORE_TareasPrevias As CORE_TareasPrevias
        oCORE_TareasPrevias = oMatrixContext.CORE_TareasPrevias.Where(Function(x) x.id = id).FirstOrDefault
        oMatrixContext.CORE_TareasPrevias.Remove(oCORE_TareasPrevias)
        oMatrixContext.SaveChanges()
    End Sub
#End Region
End Class
