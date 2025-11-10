

'Imports CoreProject.MatrixModel

Public Class Planeacion
#Region "Variables Globales"
    Private oMatrixContext As CORE_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New CORE_Entities
    End Sub
#End Region
#Region "Grabar"
    Function grabar(ByVal core_WorkFlow_Id As Int64, ByVal fechaInicio As Date?, ByVal FechaFin As Date?, ByVal usuario As Int64?, ByVal fechaRegistro As Date?, ByVal observacion As String) As Decimal
        Return oMatrixContext.CORE_Planeacion_Add(core_WorkFlow_Id, fechaInicio, FechaFin, usuario, fechaRegistro, observacion).FirstOrDefault
    End Function
#End Region
#Region "Obtener"
    Function obtenerXWorkFlowId(ByVal id As Int64) As List(Of CORE_Planeacion)
        Return oMatrixContext.CORE_Planeacion.Where(Function(x) x.Core_WorkFlow_Id = id).ToList
    End Function
#End Region
End Class
