
'Imports CoreProject.OP_Cuanti_Model

Public Class ActivacionEncuestas
#Region "Variables Globales"
    Private oMatrixContext As OP_Cuanti
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New OP_Cuanti
    End Sub
#End Region
#Region "Eliminar"
    Public Sub Eliminar(ByVal numeroEncuesta As Int64, ByVal IdTrabajo As Int64)
        oMatrixContext.OP_ActivarEncuesta_Del(numeroEncuesta, IdTrabajo)
    End Sub

    Public Sub ActualizarGestionCampo(ByVal trabajoId As Int64, ByVal numeroEncuesta As Int64, ByVal observacion As String, ByVal idUsuario As Decimal)
        oMatrixContext.OP_GestionCampo_ActivarEncuesta(trabajoId, numeroEncuesta, observacion, idUsuario)
    End Sub

#End Region

End Class
