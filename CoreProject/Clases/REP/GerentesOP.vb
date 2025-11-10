
'Imports CoreProject.RPMatrixModel

Namespace Reportes
    <Serializable()>
    Public Class GerentesOP
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
        Function ObtenerPresupuestosRevision(ByVal GerenciaOP As Int64?) As List(Of OP_PresupuestosAsignacionJBI_Result)
            Return oMatrixContext.OP_PresupuestosAsignacionJBI(GerenciaOP).ToList
        End Function

        Function ObtenerCorreosNuevoProyecto(ByVal Proyecto As Int64?) As List(Of US_Correos_Result)
            Return oMatrixContext.US_CorreosGerenteOpXProyecto(Proyecto).ToList
        End Function
#End Region
    End Class
End Namespace

