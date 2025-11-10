Public Class RegistroObservaciones

    Public Enum ETiposAgrupacion
        Total = 1
        PersonaAsignada = 2
        Unidad = 3
    End Enum
    Public Enum ETiposAgrupacionTipo
        Tarea = 1
        Tipo = 2
        Persona = 3
    End Enum
    Private oMatrixContext As RP_Entities
    Public Sub New()
        oMatrixContext = New RP_Entities
    End Sub

	Public Function obtenerIndicadoresATiempoAlTiempo(mes As Short?, ano As Short?) As List(Of REP_IndicadoresCumplimientoAlTiempoATiempo_Result)
		Return oMatrixContext.REP_IndicadoresCumplimientoAlTiempoATiempo(mes, ano).ToList()
	End Function

	Public Function obtenerIndicadoresATiempoAlTiempoDetalle(mes As Short?, ano As Short?) As List(Of REP_IndicadoresCumplimientoAlTiempoATiempo_Detalle_Result)
		Return oMatrixContext.REP_IndicadoresCumplimientoAlTiempoATiempo_Detalle(mes, ano).ToList()
	End Function
	Public Function obtenerCalidadErroresRO_GerenteProyectos(idTarea As Short?, mes As Short?, ano As Short?, agruparPor As ETiposAgrupacion, idInstrumento As Short?) As List(Of REP_Calidad_ErroresRO_GerenteProyectos_Result)
		Return oMatrixContext.REP_Calidad_ErroresRO_GerenteProyectos(idTarea, mes, ano, [Enum].GetName(agruparPor.GetType, agruparPor), idInstrumento).ToList()
	End Function
	Public Function obtenerCalidadErroresRO_GerenteProyectosDetalle(idTarea As Short?, mes As Short?, ano As Short?, agruparPor As ETiposAgrupacion, idInstrumento As Short?, usuario As String) As List(Of REP_Calidad_ErroresRO_GerenteProyectos_Detalle_Result)
		Return oMatrixContext.REP_Calidad_ErroresRO_GerenteProyectos_Detalle(idTarea, mes, ano, [Enum].GetName(agruparPor.GetType, agruparPor), idInstrumento, usuario).ToList()
	End Function
	Public Function obtenerCalidadErroresRO_COE(idTarea As Short?, mes As Short?, ano As Short?, agruparPor As ETiposAgrupacion, idInstrumento As Short?) As List(Of REP_Calidad_ErroresRO_COE_Result)
		Return oMatrixContext.REP_Calidad_ErroresRO_COE(idTarea, mes, ano, [Enum].GetName(agruparPor.GetType, agruparPor), idInstrumento).ToList()
	End Function
	Public Function obtenerCalidadErroresRO_COEDetalle(idTarea As Short?, mes As Short?, ano As Short?, agruparPor As ETiposAgrupacion, idInstrumento As Short?, usuario As String) As List(Of REP_Calidad_ErroresRO_COE_Detalle_Result)
		Return oMatrixContext.REP_Calidad_ErroresRO_COE_Detalle(idTarea, mes, ano, [Enum].GetName(agruparPor.GetType, agruparPor), idInstrumento, usuario).ToList()
	End Function
	Public Function obtenerCalidadErroresRO_ResponsableTarea(idTarea As Short?, mes As Short?, ano As Short?, agruparPor As ETiposAgrupacion, idInstrumento As Short?) As List(Of REP_Calidad_ErroresRO_ResponsableTarea_Result)
		Return oMatrixContext.REP_Calidad_ErroresRO_ResponsableTarea(idTarea, mes, ano, [Enum].GetName(agruparPor.GetType, agruparPor), idInstrumento).ToList()
	End Function
	Public Function obtenerCalidadErroresRO_ResponsableTareaDetalle(idTarea As Short?, mes As Short?, ano As Short?, agruparPor As ETiposAgrupacion, idInstrumento As Short?, usuario As String) As List(Of REP_Calidad_ErroresRO_ResponsableTarea_Detalle_Result)
		Return oMatrixContext.REP_Calidad_ErroresRO_ResponsableTarea_Detalle(idTarea, mes, ano, [Enum].GetName(agruparPor.GetType, agruparPor), idInstrumento, usuario).ToList()
	End Function

	Public Function obtenerPorcentajesErrorRegistroObservaciones(idTarea As Short?, mes As Short?, ano As Short?, agruparPor As ETiposAgrupacion, idInstrumento As Short?) As List(Of REP_ErroresRegistroObservaciones_Result)
		Return oMatrixContext.REP_ErroresRegistroObservaciones(idTarea, mes, ano, [Enum].GetName(agruparPor.GetType, agruparPor), idInstrumento).ToList()
	End Function

	Public Function obtenerPorcentajesErrorRegistroObservacionesDetalle(idTarea As Short?, mes As Short?, ano As Short?, agruparPor As ETiposAgrupacion, idInstrumento As Short?, usuario As String) As List(Of REP_ErroresRegistroObservacionesDetalle_Result)
		Return oMatrixContext.REP_ErroresRegistroObservacionesDetalle(idTarea, mes, ano, [Enum].GetName(agruparPor.GetType, agruparPor), idInstrumento, usuario).ToList()
	End Function

    Public Function obtenerRegistroObservacionesInstrumento(trabajoId As Short?, tareaId As Short?, instrumentoId As Short?, observacionId As Short?, ano As Short?, mes As Short?, usuario As Short?) As List(Of REP_RegistroObservacionesInstrumento_Result)
        Return oMatrixContext.REP_RegistroObservacionesInstrumento(trabajoId, tareaId, instrumentoId, observacionId, ano, mes, usuario).ToList()
    End Function

    Public Function obtenerRegistroObservacionesTipo(trabajoId As Short?, tareaId As Short?, instrumentoId As Short?, observacionId As Short?, ano As Short?, mes As Short?, usuario As Short?) As List(Of REP_RegistroObservacionesTipo_Result)
        Return oMatrixContext.REP_RegistroObservacionesTipo(trabajoId, tareaId, instrumentoId, observacionId, ano, mes, usuario).ToList()
    End Function

    Public Function obtenerRegistroObservacionesTarea(trabajoId As Short?, tareaId As Short?, instrumentoId As Short?, observacionId As Short?, ano As Short?, mes As Short?, usuario As Short?) As List(Of REP_RegistroObservacionesTarea_Result)
        Return oMatrixContext.REP_RegistroObservacionesTarea(trabajoId, tareaId, instrumentoId, observacionId, ano, mes, usuario).ToList()
    End Function

    Public Function obtenerRegistroObservacionesUsuario(trabajoId As Short?, tareaId As Short?, instrumentoId As Short?, observacionId As Short?, ano As Short?, mes As Short?, usuario As Short?) As List(Of REP_RegistroObservacionesUsuario_Result)
        Return oMatrixContext.REP_RegistroObservacionesUsuario(trabajoId, tareaId, instrumentoId, observacionId, ano, mes, usuario).ToList()
    End Function

    Public Function obtenerRegistroObservacionesDetalle(trabajoId As Short?, tareaId As Short?, instrumentoId As Short?, observacionId As Short?, ano As Short?, mes As Short?, usuario As Short?) As List(Of REP_RegistroObservaciones_Result)
        Return oMatrixContext.REP_RegistroObservaciones(trabajoId, tareaId, instrumentoId, observacionId, ano, mes, usuario).ToList()
    End Function

    Public Function obtenerRegistroObservacionesConsolidado(tareasId As String, instrumentoId As Short?, usuario As Int64?, ano As Short?, mes As Short?, unidadId As Short?) As List(Of REP_RegistroObservaciones_Consolidado_Result)
        Return oMatrixContext.REP_RegistroObservaciones_Consolidado(tareasId, instrumentoId, usuario, ano, mes, unidadId).ToList()
    End Function
End Class
