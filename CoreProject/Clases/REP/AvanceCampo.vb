
'Imports CoreProject.RPMatrixModel

Namespace Reportes
    <Serializable()>
    Public Class AvanceCampo
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
        Public Function ObtenerAvanceGeneralxId(ByVal TrabajoId As Int64) As REP_AvanceCampoGeneral_Result
            Return oMatrixContext.REP_AvanceCampoGeneral(TrabajoId).FirstOrDefault
        End Function

        Public Function ObtenerAvanceXCiudadXId(ByVal TrabajoId As Int64) As List(Of REP_AvanceCampoxCiudad_Result)
            Return oMatrixContext.REP_AvanceCampoxCiudad(TrabajoId).ToList
        End Function

        Public Function ObtenerAvancePorcentualAreasXId(ByVal TrabajoId As Int64) As List(Of REP_AvancePorcentualAreas_Result)
            Return oMatrixContext.REP_AvancePorcentualAreas(TrabajoId).ToList
        End Function

        Public Function ObtenerAvanceAreasRemanentesXid(ByVal TrabajoId As Int64) As List(Of REP_AvanceAreasRemanentes_Result)
            Return oMatrixContext.REP_AvanceAreasRemanentes(TrabajoId).ToList
        End Function

        Public Function ObtenerMatrizCumplimientoXid(ByVal TrabajoId As Int64) As List(Of REP_MatrizEstimacionCumplimiento_Result)
            Return oMatrixContext.REP_MatrizEstimacionCumplimiento(TrabajoId).ToList
        End Function

        Public Function ObtenerMatrizCumplimiento() As List(Of REP_MatrizEstimacionCumplimiento_Result)
            Return oMatrixContext.REP_MatrizEstimacionCumplimientoGeneral.ToList
        End Function

        Public Function ObtenerTraficoAreas(ByVal TrabajoId As Int64, ByVal UnidadId As Int64) As List(Of REP_TraficoAreas_Result)
            Return oMatrixContext.REP_TraficoAreas(TrabajoId, UnidadId).ToList
        End Function
        Public Function ObtenerTraficoAreasGeneral(ByVal FechaInicio As Date, FechaFin As Date, ByVal UnidadId As Int64) As List(Of REP_TraficoAreas_Result)
            Return oMatrixContext.REP_TraficoAreasGeneral(FechaInicio, FechaFin, UnidadId).ToList
        End Function
        Public Function CantidadTraficoEncuentas(ByVal TrabajoId As Int64, ByVal UnidadEnvia As Int64, ByVal UnidadRecibe As Int64, ByVal Recibido As Boolean) As Int32
            Return oMatrixContext.REP_CantidadEnviadaTrafico(TrabajoId, UnidadEnvia, UnidadRecibe, Recibido)(0).Value
        End Function
        Public Function ObtenerEncuestasAnuladas(ByVal TrabajoId As Int64) As List(Of REP_EncuestasAnuladas_Result)
            Return oMatrixContext.REP_EncuestasAnuladas(TrabajoId).ToList
        End Function

        Public Function ObtenerProduccionCampoXfecha(ByVal FechaI As Date, FechaF As Date, Ciudad As Int32?, VerifResultado As Int32?) As List(Of REP_ProduccionCampoxFecha_Result)
            Return oMatrixContext.REP_ProduccionCampoxFecha(FechaI, FechaF, Ciudad, VerifResultado).ToList
        End Function

        Public Function ObtenerEstudiosPlaneacion(ByVal Gerencia As Int64?, ByVal Unidad As Int64?, ByVal Metodologia As Int32?, ByVal Ciudad As Int32?) As List(Of REP_PlaneacionTrabajosGeneral_Result)
            Return oMatrixContext.REP_PlaneacionTrabajosGeneral(Gerencia, Unidad, Metodologia, Ciudad).ToList
        End Function
        Public Function ObtenerEstudiosPlaneacionDetalle(ByVal Gerencia As Int64?, ByVal Unidad As Int64?, ByVal Metodologia As Int32?, ByVal Ciudad As Int32?, Ano As Int32, Semana As Int32) As List(Of REP_PlaneacionTrabajosDetalleSemana_Result)
            Return oMatrixContext.REP_PlaneacionTrabajosDetalleSemana(Gerencia, Unidad, Metodologia, Ciudad, Ano, Semana).ToList
        End Function

        Public Function ObtenerEjecucionxEncuestador(ByVal IdTrabajo As Int64?, ByVal Ciudad As Int64?) As List(Of REP_EjecucionXEncuestador_Result)
            Return oMatrixContext.REP_EjecucionXEncuestador(IdTrabajo, Ciudad).ToList
        End Function

        Public Function ObtenerEjecucionxSupervisor(ByVal IdTrabajo As Int64?, ByVal Ciudad As Int64?) As List(Of REP_EjecucionXSupervisor_Result)
            Return oMatrixContext.REP_EjecucionXSupervisor(IdTrabajo, Ciudad).ToList
        End Function

#End Region
    End Class
End Namespace
