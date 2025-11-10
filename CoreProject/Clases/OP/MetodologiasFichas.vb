
'Imports CoreProject.OP_Model

<Serializable()>
Public Class MetodologiasFichas
#Region "Variables Globales"
    Private oMatrixContext As OP_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New OP_Entities
    End Sub
#End Region
#Region "Obtener"
    Public Function ObtenerFichaMetodologiaxId(ByVal Metodologia As Int32)
        Return oMatrixContext.OP_MetodologiaTipoFicha.Where(Function(x) x.MetodologiaId = Metodologia).FirstOrDefault()
    End Function
#End Region
End Class
