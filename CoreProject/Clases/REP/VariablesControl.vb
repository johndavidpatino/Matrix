Public Class VariablesControl
	Private oMatrixContext As RP_Entities
	Public Sub New()
		oMatrixContext = New RP_Entities
	End Sub
	Public Function obtenerReporteVariablesControl(ano As Short?, mes As Byte?, idEvaluado As Long?) As List(Of REP_PY_Variables_Control_Result)
		Return oMatrixContext.REP_PY_Variables_Control(ano, mes, idEvaluado).ToList()
	End Function
	Public Function obtenerReporteVariablesControlPorMes(ano As Short?, mes As Byte?, idEvaluado As Long?) As List(Of REP_PY_Variables_Control_PorMes_Result)
		Return oMatrixContext.REP_PY_Variables_Control_PorMes(ano, mes, idEvaluado).ToList()
	End Function
	Public Function obtenerEmpleadosConEvaluacion() As List(Of REP_PY_VariablesControlEmpleadosConEvaluacion_Result)
		Return oMatrixContext.REP_PY_VariablesControlEmpleadosConEvaluacion.ToList()
	End Function
End Class
