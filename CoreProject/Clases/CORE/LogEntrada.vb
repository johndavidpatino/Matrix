'Imports CoreProject.MatrixModel
Public Class LogEntrada
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
    Public Sub Guardar(ByVal UsuarioId As Int64, ByVal Fecha As DateTime, ByVal Ip As String, ByVal NomEquipo As String)
        oMatrixContext.Core_Log_Entrada_Add(UsuarioId, Fecha, Ip, NomEquipo)
    End Sub

#End Region

End Class
