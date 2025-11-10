
'Imports CoreProject.US_Model

<Serializable()>
Public Class DestinatariosCorreos
#Region "Variables Globales"
    Private oMatrixContext As US_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New US_Entities
    End Sub
#End Region
#Region "Obtener"
    Public Function DestinatariosAnuncioAprobacion(ByVal EstudioId As Int64) As List(Of US_CorreosAnuncioAprobacion_Result)
        Return oMatrixContext.US_CorreosAnuncioAprobacion(EstudioId).ToList
    End Function

    Public Function DestinatariosCreacionyEspecificacionesTrabajo(ByVal TrabajoId As Int64) As List(Of US_CorreosTrabajos_PreyEsp_Result)
        Return oMatrixContext.US_CorreosTrabajos_PreyEsp(TrabajoId).ToList
    End Function
#End Region
End Class
