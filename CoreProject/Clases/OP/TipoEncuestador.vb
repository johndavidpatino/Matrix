
'Imports CoreProject.OP_Model

<Serializable()>
Public Class TipoEncuestador
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
    Function obtenerTodos() As List(Of OP_TipoEncuestador2)
        Return oMatrixContext.OP_TipoEncuestador2.ToList
    End Function
    Function obtenerXId(ByVal id As Int16) As OP_TipoEncuestador2
        Return oMatrixContext.OP_TipoEncuestador2.Where(Function(f) f.id = id).FirstOrDefault
    End Function
#End Region


End Class
