
'Imports CoreProject.PY_Model

Namespace PY
    <Serializable()>
    Public Class Presupuesto
#Region "Variables Globales"
        Private oMatrixContext As PY_Entities
#End Region
#Region "Constructores"
        'Constructor public 
        Public Sub New()
            oMatrixContext = New PY_Entities
        End Sub
#End Region
#Region "Obtener"
        Function obtener(ByVal idEstudio As Long?, ByVal idProyecto As Long?, ByVal Asignado As Boolean?) As List(Of PY_Presupuestos_CU_Estudios_Get_Result)
            Return oMatrixContext.PY_Presupuestos_CU_Estudios_Get(idEstudio, idProyecto, Asignado).ToList
        End Function

#End Region
    End Class
End Namespace