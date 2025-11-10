Public Class PropuestaxBrief
    Private oMatrixContext As RP_Entities
    Public Sub New()
        oMatrixContext = New RP_Entities
    End Sub

    Function obtenerIndicadoresPropuestaxBrief(ano As Integer?, mes As Integer?, unidad As Integer?, gerente As Integer?) As List(Of REP_IndicacionesPropuestayBrief_Result)
        ano = If((ano = 0), Nothing, ano)
        mes = If((mes = 0), Nothing, mes)
        unidad = If((unidad = 0), Nothing, unidad)
        gerente = If((gerente = 0), Nothing, gerente)
        Return oMatrixContext.REP_IndicacionesPropuestayBrief(ano, mes, unidad, gerente).ToList()
    End Function

    Function obtenerIndicadoresPropuestaxBriefxGerente(ano As Integer?, mes As Integer?, unidad As Integer?, gerente As Integer?) As List(Of REP_IndicacionesPropuestayBriefXGerente_Result)
        ano = If((ano = 0), Nothing, ano)
        mes = If((mes = 0), Nothing, mes)
        unidad = If((unidad = 0), Nothing, unidad)
        Return oMatrixContext.REP_IndicacionesPropuestayBriefXGerente(ano, mes, unidad, gerente).ToList()
    End Function

    Function obtenerIndicadoresPropuestaxBriefxUnidades(ano As Integer?, mes As Integer?) As List(Of REP_IndicacionesPropuestayBriefxUnidades_Result)
        ano = If((ano = 0), Nothing, ano)
        mes = If((mes = 0), Nothing, mes)
        Return oMatrixContext.REP_IndicacionesPropuestayBriefxUnidades(ano, mes).ToList()
    End Function
End Class
