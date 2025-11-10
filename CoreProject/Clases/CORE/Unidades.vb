

'Imports CoreProject.MatrixModel

Public Class Unidades
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
    Function obtenerTodas() As List(Of CORE_Unidades)
        Return oMatrixContext.CORE_Unidades.ToList()
    End Function
    Function ObtenerUnidadesCore() As List(Of Core_UnidadesAll_Result)
        Return oMatrixContext.Core_UnidadesAll.ToList()
    End Function
#End Region
End Class
