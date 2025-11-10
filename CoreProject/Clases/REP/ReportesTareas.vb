Public Class ReportesTareas
    Public Enum ETiposAgrupacion
        proceso = 1
        unidad = 2
        tarea = 3
        personaAsignada = 4
    End Enum
    Private oMatrixContext As RP_Entities
    Public Sub New()
        oMatrixContext = New RP_Entities
    End Sub
    Function obtenerIndicadoresCumplimientoTareas(mes As Short?, ano As Short?, agruparPor As String, idTarea As Short?, proceso As Short?, grupoUnidad As Short?) As List(Of REP_IndicadoresCumplimientoTareas_Result)
        Return oMatrixContext.REP_IndicadoresCumplimientoTareas(mes, ano, agruparPor, idTarea, proceso, grupoUnidad).ToList()
    End Function

	Function obtenerIndicadoresCumplimientoTareasCOE(mes As Short?, ano As Short?, agruparPor As String, idTarea As Short?, proceso As Short?, grupoUnidad As Short?) As List(Of REP_IndicadoresCumplimientoTareas_COE_Result)
		Return oMatrixContext.REP_IndicadoresCumplimientoTareas_COE(mes, ano, agruparPor, idTarea, proceso, grupoUnidad).ToList()
	End Function

	Function obtenerIndicadoresCumplimientoTareasDetalle(mes As Short?, ano As Short?, agruparPor As String, idTarea As Short?, proceso As Short?, grupoUnidad As Short?, cumple As Short?, usuario As String) As List(Of REP_IndicadoresCumplimientoTareasDetalle_Result)
        Return oMatrixContext.REP_IndicadoresCumplimientoTareasDetalle(mes, ano, agruparPor, idTarea, proceso, grupoUnidad, cumple, usuario).ToList()
    End Function

	Function obtenerIndicadoresCumplimientoTareasDetalleCOE(mes As Short?, ano As Short?, agruparPor As String, idTarea As Short?, proceso As Short?, grupoUnidad As Short?, cumple As Short?, usuario As String) As List(Of REP_IndicadoresCumplimientoTareasDetalle_COE_Result)
		Return oMatrixContext.REP_IndicadoresCumplimientoTareasDetalle_COE(mes, ano, agruparPor, idTarea, proceso, grupoUnidad, cumple, usuario).ToList()
	End Function

	Function ObtenerDiligenciamientoEsquemaAnalisis(ano As Short?, mes As Short?, estado As Short?, usuario As String) As List(Of REP_Diligenciamiento_Esquema_Analisis_Result)
        Return oMatrixContext.REP_Diligenciamiento_Esquema_Analisis(ano, mes, estado, usuario).ToList()
    End Function

    Function ObtenerPorcentajeDiligenciamientoBrief(ano As Short?, mes As Short?, usuario As String) As List(Of REP_Porcentaje_Diligenciamiento_Brief_Result)
        Return oMatrixContext.REP_Porcentaje_Diligenciamiento_Brief(ano, mes, usuario).ToList()
    End Function

	Function ObtenerEnvioPropuestas48Horas(ano As Short?, mes As Short?, estado As Short?, usuario As String) As List(Of REP_Envio_Propuestas_48Horas_Result)
		Return oMatrixContext.REP_Envio_Propuestas_48Horas(ano, mes, estado, usuario).ToList()
	End Function

	Function ObtenerIndicadoresCronogramaPorTrabajoId(idTrabajo As Long?) As List(Of REP_IndicadoresCronogramaTareasByIdTrabajo_Result)
		Return oMatrixContext.REP_IndicadoresCronogramaTareasByIdTrabajo(idTrabajo).ToList()
	End Function

End Class
