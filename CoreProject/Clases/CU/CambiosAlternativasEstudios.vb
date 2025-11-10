
'Imports CoreProject.RPMatrixModel

Namespace Reportes
    <Serializable()>
    Public Class CambiosAlternativas
#Region "Variables Globales"
        Private oMatrixContext As RP_Entities
#End Region
#Region "Constructores"
        'Constructor public 
        Public Sub New()
            oMatrixContext = New RP_Entities
        End Sub
#End Region

#Region "Obtener"
        Function ObtenerAlternativasNoAsociadas(ByVal EstudioId As Int64) As List(Of CU_AlternativasNoUsadasXEstudio_Result)
            Return oMatrixContext.CU_AlternativasNoUsadasXEstudio(EstudioId).ToList
        End Function
#End Region
#Region "Actualizar"
        Sub CambioAlternativaEstudio(ByVal EstudioId As Int64, NuevaAlternativa As Int64)
            oMatrixContext.CU_CambioAlternativaAprobada(EstudioId, NuevaAlternativa)
        End Sub
#End Region
    End Class
End Namespace

