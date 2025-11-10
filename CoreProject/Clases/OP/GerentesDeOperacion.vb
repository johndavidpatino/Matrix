
'Imports CoreProject.OP_Model

<Serializable()>
Public Class GerentesDeOperacion
#Region "Variables Globales"
    Private oMatrixContext As OP_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New OP_Entities
    End Sub
#End Region
#Region "Actualizar"
    Public Sub ActualizarJobBookInternoEnIQ(ByVal TrabajoId As Int64, Fase As Int32, JobBook As String)
        oMatrixContext.IQ_UpdateJobBookInterno(TrabajoId, Fase, JobBook)
    End Sub
    Public Sub AsignarJBI(ByVal PropuestaId As Int64, Alternativa As Int32, Metodologia As Int32, Fase As Int32, JobBook As String)
        oMatrixContext.IQ_AsignarJobBookInterno_GOP(PropuestaId, Alternativa, Metodologia, Fase, JobBook)
    End Sub
#End Region
#Region "Obtener"
    Public Function ListadoTrabajosParaAsignarCoe(ByVal GrupoUnidad As Int32) As List(Of OP_TrabajosParaAsignacionCOE_Result)
        Return oMatrixContext.OP_TrabajosParaAsignacionCOE(GrupoUnidad).ToList
    End Function
    Public Function ListadoFasesJBInterno(ByVal TrabajoId As Int64) As List(Of IQ_ObtenerFasesJB_Result)
        Return oMatrixContext.IQ_ObtenerFasesJB(TrabajoId).ToList
    End Function
#End Region
End Class
