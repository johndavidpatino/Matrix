Imports System.Data.Entity.Core.Objects
Public Class HojasVida
	Private _Context As TH_Entities
	Sub New()
		_Context = New TH_Entities()
	End Sub
	Function obtenerHojasVida(nombres As String, apellidos As String, nivelIngles As Byte?, keywords As String, id As Int64?, anosExperienciaInicio As UShort?, anosExperienciaFin As UShort?, nivelEducativo As UShort?, ciudadResidencia As UShort?, tieneEntrevista As Boolean?, profesion As UShort?) As List(Of TH_HojasVida_Get_Result)
		Return _Context.TH_HojasVida_Get(nombres, apellidos, nivelIngles, keywords, id, anosExperienciaInicio, anosExperienciaFin, nivelEducativo, ciudadResidencia, tieneEntrevista, profesion).ToList()
	End Function
	Function obtenerHojasVidaPorId(id As Int64) As List(Of TH_HojasVida_Get_Result)
		Return _Context.TH_HojasVida_Get(Nothing, Nothing, Nothing, Nothing, id, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).ToList()
	End Function
	Function obtenerEntrevistas(idHojaVida As Int64) As List(Of TH_HojasVida_Entrevistas_Get_Result)
		Return _Context.TH_HojasVida_Entrevistas_Get(idHojaVida).ToList()
	End Function
	Function agregar(tipoIdentificacion As Byte?, identificacion As String, nombres As String, apellidos As String, edad As Byte?, anosExperiencia As Byte?, nivelIngles As Byte?, numeroCelular As Long?, correo As String, ciudadResidencia As Int16?, nivelEducativo As UShort?, fechaCreacion As DateTime, profesion As UShort?) As Decimal?
		Return _Context.TH_HojasVida_Add(tipoIdentificacion, identificacion, nombres, apellidos, edad, anosExperiencia, nivelIngles, numeroCelular, correo, Nothing, ciudadResidencia, nivelEducativo, fechaCreacion, profesion).FirstOrDefault()
	End Function
	Sub agregarEntrevista(hojasVidaId As Long, fechaEntrevista As Date, observaciones As String)
		_Context.TH_HojasVida_Entrevistas_Add(hojasVidaId, fechaEntrevista, observaciones)
	End Sub
	Sub actualizarHojasVida(id As Long, tipoIdentificacion As Byte?, identificacion As String, nombres As String, apellidos As String, edad As Byte?, anosExperiencia As Byte?, nivelIngles As Byte?, numeroCelular As Long?, correo As String, ciudadResidencia As Int16?, nivelEducativo As UShort?, profesion As UShort?)
		_Context.TH_HojasVida_Update(id, tipoIdentificacion, identificacion, nombres, apellidos, edad, anosExperiencia, nivelIngles, numeroCelular, correo, Nothing, anosExperiencia, ciudadResidencia, nivelEducativo, profesion)
	End Sub
	Sub agregarKeyword(id As Long, keyword As String)
		_Context.TH_HojasVida_Keywords_Add(id, keyword)
	End Sub
	Sub eliminarEntrevista(id As Long)
		_Context.TH_HojasVida_Entrevistas_Delete(id)
	End Sub
	Sub eliminarKeyword(hojasVidaId As Long, keyword As String)
		_Context.TH_HojasVida_Keywords_Delete(hojasVidaId, keyword)
	End Sub
	Function obtenerProfesiones() As List(Of TH_HojasVida_Profesiones_Get_Result)
		Return _Context.TH_HojasVida_Profesiones_Get.ToList()
	End Function
	Function obtenerExperienciasLaborales(hojasVidaId As Long) As List(Of TH_HojasVida_ExperienciasLaborales_Get_Result)
		Return _Context.TH_HojasVida_ExperienciasLaborales_Get(hojasVidaId).ToList
	End Function
	Sub eliminarExperienciaLaboral(id As Long)
		_Context.TH_HojasVida_ExperienciasLaborales_Del(id)
	End Sub
	Function agregarExperienciaLaboral(hojasVidaId As Long, empresa As String, duracionAnos As Single) As Long
		Return _Context.TH_HojasVida_ExperienciasLaborales_Add(hojasVidaId, empresa, duracionAnos).FirstOrDefault
	End Function
End Class
