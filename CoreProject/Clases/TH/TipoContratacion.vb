

'Imports CoreProject.TH_Model
'Imports CoreProject
Namespace TH
    <Serializable()>
    Public Class TipoContratacion
#Region "Variables Globales"
        Private oMatrixContext As TH_Entities
#End Region
#Region "Constructores"
        'Constructor public 
        Public Sub New()
            oMatrixContext = New TH_Entities
        End Sub
#End Region
#Region "Obtener"
		Function obtenerTodos() As List(Of TH_TipoContratacion2)
			Return oMatrixContext.TH_TipoContratacion2.ToList()
		End Function
		Function obtener() As List(Of TH_TiposContratacion_Get_Result)
			Return oMatrixContext.TH_TiposContratacion_Get.ToList
		End Function
#End Region
	End Class
End Namespace

