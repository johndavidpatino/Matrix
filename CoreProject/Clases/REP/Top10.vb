
'Imports CoreProject.Reportes
'Imports CoreProject.RPMatrixModel

Namespace Reportes
    <Serializable()>
    Public Class Top10
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

        Public Function ObtenerTopEncuestadoresAnulacion(ByVal Meses As Int32, ByVal Ciudad As Int32?) As List(Of REP_Top10Encuestadores_Result)
            Return oMatrixContext.REP_Top10Encuestadores_Anulacion(Meses, Ciudad).ToList
        End Function

        Public Function ObtenerTopEncuestadoresErrores(ByVal Meses As Int32, ByVal Ciudad As Int32?) As List(Of REP_Top10Encuestadores_Result)
            Return oMatrixContext.REP_Top10Encuestadores_Errores(Meses, Ciudad).ToList
        End Function

        Public Function ObtenerTopEncuestadoresVIP(ByVal Meses As Int32, ByVal Ciudad As Int32?) As List(Of REP_Top10Encuestadores_Result)
            Return oMatrixContext.REP_Top10Encuestadores_VIP(Meses, Ciudad).ToList
        End Function

#End Region


    End Class


End Namespace
