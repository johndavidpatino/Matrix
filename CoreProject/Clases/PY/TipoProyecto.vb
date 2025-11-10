
'Imports CoreProject.PY_Model

<Serializable()>
Public Class TipoProyecto
#Region "Variables Globales"
    Private oMatrixContext As PY_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New PY_Entities
    End Sub
#End Region
#Region "Obtener"

    Function obtenerTodos() As List(Of PY_TiposProyectos_Get_Result)
        Return PY_TiposProyectos_Get_Result(Nothing, Nothing)
    End Function

    Private Function PY_TiposProyectos_Get_Result(ByVal id As Int16?, ByVal TipoProyecto As String) As List(Of PY_TiposProyectos_Get_Result)
        Return oMatrixContext.PY_TiposProyectos_Get(id, TipoProyecto).ToList
    End Function
#End Region
End Class
