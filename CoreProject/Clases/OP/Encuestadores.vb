
'Imports CoreProject.OP_Model

<Serializable()>
Public Class Encuestadores
#Region "Variables Globales"
    Private oMatrixContext As OP_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New OP_Entities
    End Sub
#End Region
#Region "Metodos"
    Function obtenerXId(ByVal id As Int64) As OP_Encuestadores2
        Return oMatrixContext.OP_Encuestadores.Where(Function(x) x.id = id).FirstOrDefault
    End Function
#End Region
End Class
