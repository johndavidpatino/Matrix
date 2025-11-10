

'Imports CoreProject.MatrixModel
Imports System.Data.Entity.Core.Objects

Public Class Hilo
#Region "Variables Globales"
    Private oMatrixContext As CORE_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New CORE_Entities
    End Sub
#End Region
#Region "Obtener"
    Function obtenerXId(ByVal id As Int64) As CORE_Hilos
        Return oMatrixContext.CORE_Hilos.Where(Function(x) x.id = id).FirstOrDefault
    End Function

    Function Obtenerhilo(ByVal Id As Int64?) As List(Of CORE_HilosGetxContenedor_Result)
        Return oMatrixContext.CORE_HilosGetxContenedor(Id).ToList
    End Function

    Public Function GuardarHilo(ByVal tipohilo As Int64?, ByVal Contenedor As Int64?) As Decimal

        Dim ID As Decimal = 0
        Dim oResult As ObjectResult(Of Decimal?)
        oResult = oMatrixContext.CORE_Hilo_Add(tipohilo, Contenedor)
        ID = Decimal.Parse(oResult(0))
        Return ID

    End Function

#End Region
End Class
