

'Imports CoreProject.MatrixModel
Public Class Tarea
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
    Function obtenerXId(ByVal id As Int64) As CORE_Tareas_Get_Result
        Return CORE_Tarea_Get(id).FirstOrDefault
    End Function

    Private Function CORE_Tarea_Get(ByVal id As Int64?) As List(Of CORE_Tareas_Get_Result)
        Return oMatrixContext.CORE_Tareas_Get(id).ToList
    End Function

    Function obtenerTodas() As List(Of CORE_Tareas_Get_Result)
        Return oMatrixContext.CORE_Tareas_Get(Nothing).ToList
    End Function

    Function obtenerXIdUnidadEjecuta(idUnidadEjecuta As Int64) As List(Of CORE_TareasXUnidadEjecuta_Result)
        Return oMatrixContext.CORE_TareasXUnidadEjecuta(idUnidadEjecuta).ToList
    End Function

#End Region
#Region "Grabar"
    Sub grabar(ByVal id As Int64?, ByVal Tarea As String, ByVal NoEmpiezaAntesDe As Int64, ByVal NoTerminaAntesDe As Int64, ByVal TiempoPromedioDias As Int32, ByVal RequiereEstimacion As Boolean, ByVal RolEstima As Int64, ByVal UnidadEjecuta As Int64, ByVal UnidadRecibe As Int64, ByVal RolEjecuta As Int64, ByVal Visible As Boolean)

        Dim oCORE_Tareas As New CORE_Tareas


        If id.HasValue Then
            oCORE_Tareas = oMatrixContext.CORE_Tareas.Where(Function(x) x.id = id).FirstOrDefault
        End If

        oCORE_Tareas.Tarea = Tarea
        oCORE_Tareas.NoEmpiezaAntesDe = NoEmpiezaAntesDe
        oCORE_Tareas.NoTerminaAntesDe = NoTerminaAntesDe
        oCORE_Tareas.TiempoPromedioDias = TiempoPromedioDias
        oCORE_Tareas.RequiereEstimacion = RequiereEstimacion
        oCORE_Tareas.RolEstima = RolEstima
        oCORE_Tareas.UnidadEjecuta = UnidadEjecuta
        oCORE_Tareas.UnidadRecibe = UnidadRecibe
        oCORE_Tareas.RolEjecuta = RolEjecuta
        oCORE_Tareas.Visible = Visible

        If Not id.HasValue Then
            oMatrixContext.CORE_Tareas.Add(oCORE_Tareas)
        End If

        oMatrixContext.SaveChanges()

    End Sub
#End Region
End Class
