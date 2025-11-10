

'Imports CoreProject.MatrixModel

Public Class TipoHilo_Tareas
#Region "Variables Globales"
    Private oMatrixContext As CORE_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New CORE_Entities
    End Sub
#End Region
#Region "Metodos"
    Function obtenerXTipoHiloId(ByVal TipoHiloId As Int64, ByVal Asignadas As Boolean?) As List(Of CORE_Configuracion_TareasXTipoHilo_Get_Result)
        Return oMatrixContext.CORE_Configuracion_TareasXTipoHilo_Get(TipoHiloId, Asignadas).ToList
    End Function
#End Region
#Region "Grabar"
    Sub grabar(ByVal TipoHiloId As Int64, ByVal TareaId As Int64)
        Dim oCORE_TipoHilo_Tareas As New CORE_TipoHilo_Tareas
        oCORE_TipoHilo_Tareas.TareaId = TareaId
        oCORE_TipoHilo_Tareas.TipoHiloId = TipoHiloId
        oMatrixContext.CORE_TipoHilo_Tareas.Add(oCORE_TipoHilo_Tareas)
        oMatrixContext.SaveChanges()
    End Sub
#End Region
#Region "Eliminar"
    Sub Eliminar(ByVal TipoHiloId As Int64, ByVal TareaId As Int64)
        Dim oeCORE_TipoHilo_Tareas As CORE_TipoHilo_Tareas
        oeCORE_TipoHilo_Tareas = oMatrixContext.CORE_TipoHilo_Tareas.Where(Function(x) x.TareaId = TareaId AndAlso x.TipoHiloId = TipoHiloId).FirstOrDefault
        oMatrixContext.CORE_TipoHilo_Tareas.Remove(oeCORE_TipoHilo_Tareas)
        oMatrixContext.SaveChanges()
    End Sub
#End Region
End Class
