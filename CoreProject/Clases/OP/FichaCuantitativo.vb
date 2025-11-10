Imports System.Data.Objects

Public Class FichaCuantitativo
#Region "Variables Globales"
    Private oMatrixContext As OP_Q_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New OP_Q_Entities
    End Sub
#End Region
#Region "Obtener"
    Public Function Todos() As List(Of OP_FichaCuantitativo)
        Return oMatrixContext.OP_FichaCuantitativo().ToList
    End Function

    Public Function ObtenerXID(ByVal id As Long) As OP_FichaCuantitativo
        Return oMatrixContext.OP_FichaCuantitativo.Where(Function(x) x.id = id).FirstOrDefault
    End Function

    Public Function ObtenerXTrabajo(ByVal Trabajoid As Long) As OP_FichaCuantitativo
        Return oMatrixContext.OP_FichaCuantitativo.Where(Function(x) x.TrabajoId = Trabajoid).FirstOrDefault
    End Function
#End Region
#Region "Guardar"

#End Region
End Class
