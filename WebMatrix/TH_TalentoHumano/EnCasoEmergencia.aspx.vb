Imports System.IO
Imports System.Web.Script.Services

Public Class EnCasoEmergencia
	Inherits System.Web.UI.Page
	<Services.WebMethod()>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Shared Function getDatosEmergenciaEmpleados(nombres As String, apellidos As String, areaServiceLine As UShort?, cargo As Byte?, sede As Byte?) As List(Of CoreProject.TH_Empleados_DatosEmergencia_Get_Result)
		Dim empleados As New CoreProject.Empleados
		Return empleados.obtenerDatosEmergencia(nombres, apellidos, areaServiceLine, cargo, sede)
	End Function
End Class