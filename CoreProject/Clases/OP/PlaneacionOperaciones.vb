
'Imports CoreProject.OP_Model

<Serializable()>
Public Class PlaneacionOPCuanti
#Region "Variables Globales"
    Private oMatrixContext As OP_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New OP_Entities
    End Sub
#End Region
#Region "Obtener"
    Public Function PlaneacionEncuestas(ByVal Factor As Double) As List(Of OP_PL_Encuestas_Result)
        Return oMatrixContext.OP_PL_Encuestas(Factor).ToList
    End Function
    Public Function PlaneacionEncuestadores(ByVal Factor As Double) As List(Of OP_PL_Encuestadores_Result)
        Return oMatrixContext.OP_PL_Encuestadores(Factor).ToList
    End Function
    Public Function PlaneacionScriptPersonas(ByVal Factor As Double) As List(Of OP_PL_ScriptingPersonas_Result)
        Return oMatrixContext.OP_PL_ScriptingPersonas(Factor).ToList
    End Function
    Public Function PlaneacionScriptTrabajos(ByVal Factor As Double) As List(Of OP_PL_ScriptingTrabajos_Result)
        Return oMatrixContext.OP_PL_ScriptingTrabajos(Factor).ToList
    End Function
    Public Function PlaneacionCriticaPersonas(ByVal Factor As Double) As List(Of OP_PL_CriticaPersonas_Result)
        Return oMatrixContext.OP_PL_CriticaPersonas(Factor).ToList
    End Function
    Public Function PlaneacionCriticaEncuestas(ByVal Factor As Double) As List(Of OP_PL_CriticaEncuestas_Result)
        Return oMatrixContext.OP_PL_CriticaEncuestas(Factor).ToList
    End Function
    Public Function PlaneacionVerificacionPersonas(ByVal Factor As Double) As List(Of OP_PL_VerificacionPersonas_Result)
        Return oMatrixContext.OP_PL_VerificacionPersonas(Factor).ToList
    End Function
    Public Function PlaneacionVerificacionHoras(ByVal Factor As Double) As List(Of OP_PL_VerificacionHoras_Result)
        Return oMatrixContext.OP_PL_VerificacionHoras(Factor).ToList
    End Function
    Public Function PlaneacionCapturaPersonas(ByVal Factor As Double) As List(Of OP_PL_DigitacionPersonas_Result)
        Return oMatrixContext.OP_PL_DigitacionPersonas(Factor).ToList
    End Function
    Public Function PlaneacionCapturaPreguntas(ByVal Factor As Double) As List(Of OP_PL_DigitacionPreguntas_Result)
        Return oMatrixContext.OP_PL_DigitacionPreguntas(Factor).ToList
    End Function
    Public Function PlaneacionCodificacionPersonas(ByVal Factor As Double) As List(Of OP_PL_CodificacionPersonas_Result)
        Return oMatrixContext.OP_PL_CodificacionPersonas(Factor).ToList
    End Function
    Public Function PlaneacionCodificacionPreguntas(ByVal Factor As Double) As List(Of OP_PL_CodificacionPreguntas_Result)
        Return oMatrixContext.OP_PL_CodificacionPreguntas(Factor).ToList
    End Function
    Public Function PlaneacionProcesamientoPersonas(ByVal Factor As Double) As List(Of OP_PL_ProcesamientoPersonas_Result)
        Return oMatrixContext.OP_PL_ProcesamientoPersonas(Factor).ToList
    End Function
    Public Function PlaneacionProcesamientoTrabajos(ByVal Factor As Double) As List(Of OP_PL_ProcesamientoTrabajos_Result)
        Return oMatrixContext.OP_PL_ProcesamientoTrabajos(Factor).ToList
    End Function
    Public Function PlaneacionEncuestasXMetodologia(ByVal Factor As Double, ByVal Metodologia As Int32) As List(Of OP_PL_EncuestasXMetodologia_Result)
        Return oMatrixContext.OP_PL_EncuestasXMetodologia(Factor, Metodologia).ToList
    End Function
    Public Function PlaneacionEncuestasXUnidad(ByVal Factor As Double, ByVal Unidad As Int32) As List(Of OP_PL_EncuestasXUnidad_Result)
        Return oMatrixContext.OP_PL_EncuestasXUnidad(Factor, Unidad).ToList
    End Function
    Public Function PlaneacionEncuestasXCiudad(ByVal Factor As Double, ByVal Ciudad As Int32) As List(Of OP_PL_EncuestasXCiudad_Result)
        Return oMatrixContext.OP_PL_EncuestasXCiudad(Factor, Ciudad).ToList
    End Function
    Public Function PlaneacionEncuestasXGerencia(ByVal Factor As Double, ByVal Gerencia As Int64) As List(Of OP_PL_EncuestasXGerencia_Result)
        Return oMatrixContext.OP_PL_EncuestasXGerencia(Factor, Gerencia).ToList
    End Function
    Public Function PlaneacionEncuestadoresXMetodologia(ByVal Factor As Double, ByVal Metodologia As Int32) As List(Of OP_PL_EncuestadoresXMetodologia_Result)
        Return oMatrixContext.OP_PL_EncuestadoresXMetodologia(Factor, Metodologia).ToList
    End Function
    Public Function PlaneacionEncuestadoresXUnidad(ByVal Factor As Double, ByVal Unidad As Int32) As List(Of OP_PL_EncuestadoresXUnidad_Result)
        Return oMatrixContext.OP_PL_EncuestadoresXUnidad(Factor, Unidad).ToList
    End Function
    Public Function PlaneacionEncuestadoresXCiudad(ByVal Factor As Double, ByVal Ciudad As Int32) As List(Of OP_PL_EncuestadoresXCiudad_Result)
        Return oMatrixContext.OP_PL_EncuestadoresXCiudad(Factor, Ciudad).ToList
    End Function
    Public Function PlaneacionEncuestadoresXGerencia(ByVal Factor As Double, ByVal Gerencia As Int64) As List(Of OP_PL_EncuestadoresXGerencia_Result)
        Return oMatrixContext.OP_PL_EncuestadoresXGerencia(Factor, Gerencia).ToList
    End Function
    Public Function ObtenerMetodologias() As List(Of OP_Metodologias2)
        Return oMatrixContext.OP_Metodologias.Where(Function(x) Not (x.MetGrupoUnidad = 20)).ToList
    End Function

    Public Function PlaneacionPropuestasAltaEncuestas(ByVal Gerencia As Int64?, ByVal Unidad As Int64?, ByVal Metodologia As Int32?, ByVal Ciudad As Int32?) As List(Of OP_PL_PropuestasAltaProbabilidad_Result)
        Return oMatrixContext.OP_PL_PropuestasAltaProbabilidadEncuestas(Gerencia, Unidad, Metodologia, Ciudad).ToList
    End Function

    Public Function PlaneacionPropuestasAltaEncuestadores(ByVal Gerencia As Int64?, ByVal Unidad As Int64?, ByVal Metodologia As Int32?, ByVal Ciudad As Int32?) As List(Of OP_PL_PropuestasAltaProbabilidad_Result)
        Return oMatrixContext.OP_PL_PropuestasAltaProbabilidadRecursos(Gerencia, Unidad, Metodologia, Ciudad).ToList
    End Function

    Public Function PlaneacionEstudiosEncuestas(ByVal Gerencia As Int64?, ByVal Unidad As Int64?, ByVal Metodologia As Int32?, ByVal Ciudad As Int32?) As List(Of OP_PL_PropuestasAltaProbabilidad_Result)
        Return oMatrixContext.OP_PL_EstudiosSinEntregarEncuestas(Gerencia, Unidad, Metodologia, Ciudad).ToList
    End Function

    Public Function PlaneacionEstudiosEncuestadores(ByVal Gerencia As Int64?, ByVal Unidad As Int64?, ByVal Metodologia As Int32?, ByVal Ciudad As Int32?) As List(Of OP_PL_PropuestasAltaProbabilidad_Result)
        Return oMatrixContext.OP_PL_EstudiosSinEntregarRecursos(Gerencia, Unidad, Metodologia, Ciudad).ToList
    End Function

    Public Function PlaneacionPropyEstEncuestas(ByVal Gerencia As Int64?, ByVal Unidad As Int64?, ByVal Metodologia As Int32?, ByVal Ciudad As Int32?) As List(Of OP_PL_PropuestasAltaProbabilidad_Result)
        Return oMatrixContext.OP_PL_PropyEstEncuestas(Gerencia, Unidad, Metodologia, Ciudad).ToList
    End Function

    Public Function PlaneacionPropyEstEncuestadores(ByVal Gerencia As Int64?, ByVal Unidad As Int64?, ByVal Metodologia As Int32?, ByVal Ciudad As Int32?) As List(Of OP_PL_PropuestasAltaProbabilidad_Result)
        Return oMatrixContext.OP_PL_PropyEstRecursos(Gerencia, Unidad, Metodologia, Ciudad).ToList
    End Function

#End Region

#Region "Factores"
    Public Function PlaneacionEncuestasFactor() As Double
        Return oMatrixContext.OP_PL_EncuestasFactor(0)
    End Function
    Public Function PlaneacionEncuestadoresFactor() As Double
        Return oMatrixContext.OP_PL_EncuestadoresFactor(0)
    End Function
    Public Function PlaneacionScriptPersonasFactor() As Double
        Return oMatrixContext.OP_PL_ScriptingPersonasFactor(0)
    End Function
    Public Function PlaneacionScriptTrabajosFactor() As Double
        Return oMatrixContext.OP_PL_ScriptingTrabajosFactor(0)
    End Function
    Public Function PlaneacionCriticaPersonasFactor() As Double
        Return oMatrixContext.OP_PL_CriticaPersonasFactor(0)
    End Function
    Public Function PlaneacionCriticaEncuestasFactor() As Double
        Return oMatrixContext.OP_PL_CriticaEncuestasFactor(0)
    End Function
    Public Function PlaneacionVerificacionPersonasFactor() As Double
        Return oMatrixContext.OP_PL_VerificacionPersonasFactor(0)
    End Function
    Public Function PlaneacionVerificacionHorasFactor() As Double
        Return oMatrixContext.OP_PL_VerificacionHorasFactor(0)
    End Function
    Public Function PlaneacionCapturaPersonasFactor() As Double
        Return oMatrixContext.OP_PL_DigitacionPersonasFactor(0)
    End Function
    Public Function PlaneacionCapturaPreguntasFactor() As Double
        Return oMatrixContext.OP_PL_DigitacionPreguntasFactor(0)
    End Function
    Public Function PlaneacionCodificacionPersonasFactor() As Double
        Return oMatrixContext.OP_PL_CodificacionPersonasFactor(0)
    End Function
    Public Function PlaneacionCodificacionPreguntasFactor() As Double
        Return oMatrixContext.OP_PL_CodificacionPreguntasFactor(0)
    End Function
    Public Function PlaneacionProcesamientoPersonasFactor() As Double
        Return oMatrixContext.OP_PL_ProcesamientoPersonasFactor(0)
    End Function
    Public Function PlaneacionProcesamientoTrabajosFactor() As Double
        Return oMatrixContext.OP_PL_ProcesamientoTrabajosFactor(0)
    End Function
    Public Function PlaneacionEncuestasXMetodologiaFactor(ByVal Metodologia As Int32) As Double
        Return oMatrixContext.OP_PL_EncuestasXMetodologiaFactor(Metodologia)(0)
    End Function
    Public Function PlaneacionEncuestasXUnidadFactor(ByVal Unidad As Int32) As Double
        Return oMatrixContext.OP_PL_EncuestasXUnidadFactor(Unidad)(0)
    End Function
    Public Function PlaneacionEncuestasXCiudadFactor(ByVal Ciudad As Int32) As Double
        Return oMatrixContext.OP_PL_EncuestasXCiudadFactor(Ciudad)(0)
    End Function
    Public Function PlaneacionEncuestasXGerenciaFactor(ByVal Gerencia As Int64) As Double
        Return oMatrixContext.OP_PL_EncuestasXGerenciaFactor(Gerencia)(0)
    End Function
    Public Function PlaneacionEncuestadoresXMetodologiaFactor(ByVal Metodologia As Int32) As Double
        Return oMatrixContext.OP_PL_EncuestadoresXMetodologiaFactor(Metodologia)(0)
    End Function
    Public Function PlaneacionEncuestadoresXUnidadFactor(ByVal Unidad As Int32) As Double
        Return oMatrixContext.OP_PL_EncuestadoresXUnidadFactor(Unidad)(0)
    End Function
    Public Function PlaneacionEncuestadoresXCiudadFactor(ByVal Ciudad As Int32) As Double
        Return oMatrixContext.OP_PL_EncuestadoresXCiudadFactor(Ciudad)(0)
    End Function
    Public Function PlaneacionEncuestadoresXGerenciaFactor(ByVal Gerencia As Int64) As Double
        Return oMatrixContext.OP_PL_EncuestadoresXGerenciaFactor(Gerencia)(0)
    End Function
#End Region
End Class
