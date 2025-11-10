
'Imports CoreProject.RPMatrixModel

Namespace Reportes
    <Serializable()>
    Public Class InformeAnuladas
#Region "Variables Globales"
        Private oMatrixContext As RP_Entities
#End Region
#Region "Constructores"
        'Constructor public 
        Public Sub New()
            oMatrixContext = New RP_Entities
        End Sub
#End Region

#Region "Obtener"
        Public Function ObtenerDetalleAnuladas(ByVal FechaInicio As Date, FechaFin As Date) As List(Of REP_InformeAnuladasxCiudad_Result)
            Return oMatrixContext.REP_InformeAnuladasxCiudad(FechaInicio, FechaFin).ToList
        End Function
#End Region
    End Class
End Namespace
