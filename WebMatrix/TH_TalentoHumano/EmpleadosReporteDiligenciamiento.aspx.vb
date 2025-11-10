Imports System.IO
Imports System.Web.Script.Services

Public Class EmpleadosReporteDiligenciamiento
	Inherits System.Web.UI.Page

	<Services.WebMethod()>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Shared Function getReporteDiligenciamiento() As List(Of CoreProject.TH_Empleados_EstadoDiligenciamientoDatos_Get_Result)
		Dim empleados As New CoreProject.Empleados
		Dim context = HttpContext.Current
		If context.Session("IDUsuario") Is Nothing Then
			context.Response.Clear()
			context.Response.StatusCode = 401
			context.Response.End()
		End If
		Return empleados.obtenerReporteDiligenciamiento().OrderBy(Function(x) x.PorcentajeDiligenciamiento).ToList
	End Function
End Class