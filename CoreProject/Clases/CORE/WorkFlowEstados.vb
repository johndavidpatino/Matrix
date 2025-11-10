

'Imports CoreProject.MatrixModel

Public Class WorkFlowEstados
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
    Function obtenerXId(ByVal id As Int16) As CORE_WorkflowEstados
        Return oMatrixContext.CORE_WorkflowEstados.Where(Function(x) x.id = id).FirstOrDefault
    End Function
    Function obtenerListado() As List(Of CORE_WorkflowEstados)
        Return oMatrixContext.CORE_WorkflowEstados.ToList()
    End Function
#End Region
End Class
