
'Imports CoreProject.OP_Model

<Serializable()>
Public Class CorreoAnulacionEncuestas
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
    Public Function DestinatarioEncuestas(ByVal TrabajoId As Int64, ByVal NumeroEncuestas As Int64) As List(Of US_CorreoAnulacionEncuestas_Result)
        Return oMatrixContext.US_CorreoAnulacionEncuestas(NumeroEncuestas, TrabajoId).ToList
    End Function

    Public Function ContenidoCorreo(ByVal TrabajoId As Int64, ByVal NumeroEncuestas As Int64) As OP_CorreoAnulacionEncuestas_Result
        Return oMatrixContext.OP_CorreoAnulacionEncuestas(NumeroEncuestas, TrabajoId).FirstOrDefault
    End Function
#End Region

End Class
