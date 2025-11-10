Public Class GestionCalidad
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
    Function obtenerEvaluacionProveedoresCalle74(FechaInicio As Nullable(Of Date), FechaFin As Nullable(Of Date)) As List(Of REP_EvaluacionProveedores_Result)
        Return oMatrixContext.REP_EvaluacionProveedores(FechaInicio, FechaFin).ToList
    End Function
    Function obtenerEvaluacionProveedoresCalle78(FechaInicio As Nullable(Of Date), FechaFin As Nullable(Of Date)) As List(Of REP_EvaluacionProveedores_Ops_Result)
        Return oMatrixContext.REP_EvaluacionProveedores_Ops(FechaInicio, FechaFin).ToList
    End Function
#End Region
End Class
