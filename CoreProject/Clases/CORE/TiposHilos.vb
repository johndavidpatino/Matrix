

'Imports CoreProject.US_Model
'Imports CoreProject.MatrixModel

Public Class TiposHilos
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
    Function obtenerTodos() As List(Of CORE_TipoHilos)

        Return oMatrixContext.CORE_TipoHilos.ToList

    End Function

    Function obtenerXId(ByVal id As Int64) As CORE_TipoHilos
        Return oMatrixContext.CORE_TipoHilos.Where(Function(x) x.id = id).FirstOrDefault

    End Function

#End Region
End Class
