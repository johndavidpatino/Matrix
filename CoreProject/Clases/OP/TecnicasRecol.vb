
'Imports CoreProject.OP_Cuanti_Model

<Serializable()>
Public Class TecnicasRecol
#Region "Variables Globales"
    Private oMatrixContext As OP_Cuanti
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New OP_Cuanti
    End Sub
#End Region
#Region "Obtener"
    Public Function ObtenerTecnicasPorTipoPY(ByVal tipopy As Int32) As List(Of OP_Tecnicas)
        Return oMatrixContext.OP_Tecnicas.Where(Function(x) x.TecTipo = tipopy).ToList
    End Function

    Public Function ObtenerTecnicaXId(ByVal idTecnica As Int32) As OP_Tecnicas
        Return oMatrixContext.OP_Tecnicas.Where(Function(x) x.id = idTecnica).FirstOrDefault
    End Function
#End Region
End Class
