
'Imports CoreProject.RPMatrixModel

Namespace Reportes
    <Serializable()>
    Public Class DestinatariosCorreos
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
        Function AnuncioAprobacionConValor(ByVal EstudioId As Int64) As List(Of US_Correos_Result)
            Return oMatrixContext.US_CorreosAnuncioAprobacionConValor(EstudioId).ToList
        End Function
        Function AnuncioAprobacionSinValor(ByVal EstudioId As Int64) As List(Of US_Correos_Result)
            Return oMatrixContext.US_CorreosAnuncioAprobacionSinValor(EstudioId).ToList
        End Function
        Function CorreosPorTrabajoGP(ByVal TrabajoId As Int64) As List(Of US_Correos_Result)
            Return oMatrixContext.US_CorreosGerenteOpXTrabajo(TrabajoId).ToList
        End Function
        Function CorreosPorProyectoGP(ByVal ProyectoId As Int64) As List(Of US_Correos_Result)
            Return oMatrixContext.US_CorreosGerenteOpXProyecto(ProyectoId).ToList
        End Function
#End Region
    End Class
End Namespace

