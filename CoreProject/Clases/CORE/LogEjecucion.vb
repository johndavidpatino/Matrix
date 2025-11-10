'Imports CoreProject.MatrixModel

Public Class LogEjecucion

#Region "Variables Globales"
    Private oMatrixContext As CORE_Entities

#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New CORE_Entities
    End Sub
#End Region

#Region "Guardar"
    Public Sub Guardar(ByVal FromId As Int64, ByVal IdDoc As Int64?, ByVal Fecha As DateTime, ByVal UsuarioId As Int64, ByVal Accion As Int64)
        If IdDoc Is Nothing Then IdDoc = 0
        oMatrixContext.LogEjecucionAdd(FromId, IdDoc, Fecha, UsuarioId, Accion)
    End Sub

#End Region
End Class
