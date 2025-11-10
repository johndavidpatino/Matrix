

'Imports CoreProject.MatrixModel

Public Class Divipola
	Public Enum ECiudades
		Bogota = 11001
	End Enum
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
	Function obtenerXPais(ByVal pais As Int16) As List(Of Divipola_Get_Result)
		Return oMatrixContext.Divipola_Get(pais).ToList
	End Function
#End Region
End Class
