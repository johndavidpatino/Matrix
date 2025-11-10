Imports System.IO
Imports System.Web.Script.Services

Public Class HojasVida
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

	End Sub
	<Services.WebMethod()>
	Shared Function savePerson(idPersonSelected As Int64?, tipoIdentificacion As Int16?, identificacion As String, nombres As String, apellidos As String, edad As UShort?, anosExperiencia As UShort?, nivelIngles As UShort?, numeroCelular As Int64?, correo As String, ciudadResidencia As Int64?, nivelEducativo As UShort?, profesion As UShort?) As Decimal
		Dim hojasVida As New CoreProject.HojasVida()
		Dim personId As Decimal

		If (idPersonSelected.HasValue = False) Then
			personId = hojasVida.agregar(tipoIdentificacion, identificacion, nombres, apellidos, edad, anosExperiencia, nivelIngles, numeroCelular, correo, ciudadResidencia, nivelEducativo, DateTime.UtcNow.AddHours(-5), profesion)
		Else
			hojasVida.actualizarHojasVida(idPersonSelected.Value, tipoIdentificacion, identificacion, nombres, apellidos, edad, anosExperiencia, nivelIngles, numeroCelular, correo, ciudadResidencia, nivelEducativo, profesion)
		End If

		If idPersonSelected.HasValue Then
			personId = idPersonSelected.Value
		End If

		Return personId

	End Function
	<Services.WebMethod()>
	Shared Function addKeyword(idPersonSelected As Int64, keyword As String) As Decimal
		Dim hojasVida As New CoreProject.HojasVida()

		hojasVida.agregarKeyword(idPersonSelected, keyword)

		Return idPersonSelected

	End Function
	<Services.WebMethod()>
	Shared Function addEntrevista(idPersonSelected As Int64, fechaEntrevista As Date, observacion As String) As Decimal
		Dim hojasVida As New CoreProject.HojasVida()

		hojasVida.agregarEntrevista(idPersonSelected, fechaEntrevista, observacion)

		Return idPersonSelected

	End Function
	<Services.WebMethod()>
	Shared Function addExperienciaLaboral(idPersonSelected As Int64, empresa As String, duracionAnos As Single) As Decimal
		Dim hojasVida As New CoreProject.HojasVida()

		Return hojasVida.agregarExperienciaLaboral(idPersonSelected, empresa, duracionAnos)

	End Function

	<Services.WebMethod()>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Shared Function obtenerHojasVida(id As Int64?, nombres As String, apellidos As String, nivelIngles As UShort?, keywords As String, anosExperienciaInicio As UShort?, anosExperienciaFin As UShort?, nivelEducativo As UShort?, ciudadResidencia As UShort?, tieneEntrevista As Boolean?, profesion As UShort?) As List(Of CoreProject.TH_HojasVida_Get_Result)
		Dim hojasVida As New CoreProject.HojasVida()
		Return hojasVida.obtenerHojasVida(nombres, apellidos, nivelIngles, keywords, id, anosExperienciaInicio, anosExperienciaFin, nivelEducativo, ciudadResidencia, tieneEntrevista, profesion)
	End Function
	<Services.WebMethod()>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Shared Function obtenerHojasVidaPorId(id As Int64?) As List(Of CoreProject.TH_HojasVida_Get_Result)
		Dim hojasVida As New CoreProject.HojasVida()
		Return hojasVida.obtenerHojasVidaPorId(id)
	End Function
	<Services.WebMethod()>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Shared Function obtenerEntrevistasHojaVida(hojaVidaId As Int64) As List(Of CoreProject.TH_HojasVida_Entrevistas_Get_Result)
		Dim hojasVida As New CoreProject.HojasVida()
		Return hojasVida.obtenerEntrevistas(hojaVidaId)
	End Function
	<Services.WebMethod()>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Shared Function obtenerExperienciasLaboralesPorHojaVida(hojaVidaId As Int64) As List(Of CoreProject.TH_HojasVida_ExperienciasLaborales_Get_Result)
		Dim hojasVida As New CoreProject.HojasVida()
		Return hojasVida.obtenerExperienciasLaborales(hojaVidaId)
	End Function
	<Services.WebMethod()>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Shared Function getNivelesEducativos() As List(Of CoreProject.TH_NivelesEducativos)
		Dim hojasVida As New CoreProject.RegistroPersonas()
		Return hojasVida.NivelEducativoList
	End Function
	<Services.WebMethod()>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Shared Function getCiudades() As List(Of CoreProject.TH_Ciudades)
		Dim hojasVida As New CoreProject.RegistroPersonas()
		Return hojasVida.CiudadesList
	End Function
	<Services.WebMethod()>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Shared Function getProfesiones() As List(Of CoreProject.TH_HojasVida_Profesiones_Get_Result)
		Dim hojasVida As New CoreProject.HojasVida()
		Return hojasVida.obtenerProfesiones
	End Function
	<Services.WebMethod()>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Shared Sub eliminarEntrevista(id As Int64)
		Dim hojasVida As New CoreProject.HojasVida()
		hojasVida.eliminarEntrevista(id)
	End Sub
	<Services.WebMethod()>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Shared Sub eliminarKeyword(hojasVidaId As Int64, keyword As String)
		Dim hojasVida As New CoreProject.HojasVida()
		hojasVida.eliminarKeyword(hojasVidaId, keyword)
	End Sub
	<Services.WebMethod()>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Shared Sub eliminarExperienciaLaboral(id As Long)
		Dim hojasVida As New CoreProject.HojasVida()
		hojasVida.eliminarExperienciaLaboral(id)
	End Sub

End Class