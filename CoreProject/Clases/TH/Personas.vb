
'Imports CoreProject.TH_Model
'Imports CoreProject
'Imports CoreProject.OP_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class Personas
#Region "Variables Globales"
	Private oMatrixContext As TH_Entities
#End Region
#Region "Constructores"
	'Constructor public 
	Public Sub New()
		oMatrixContext = New TH_Entities
	End Sub
#End Region
#Region "Obtener"

	Public Function ObtenerPersonasxCCNombre(ByVal Cedula As Int64?, ByVal Nombre As String) As List(Of TH_PersonasGET_Result)
		Return oMatrixContext.TH_PersonasGET(Cedula, Nombre).ToList
	End Function

	Public Function ObtenerPersonasxID(ByVal id As Int64?) As List(Of TH_Personas2)
		Return oMatrixContext.TH_Personas.Where(Function(x) x.id = id).ToList
	End Function

	Public Function ObtenerTipoEncuestador(ByVal id As Int64) As OP_Encuestadores4
		Return oMatrixContext.OP_Encuestadores.Where(Function(x) x.id = id).FirstOrDefault
	End Function
	Public Function DevolverTodos() As List(Of TH_Personas_Get_Result)
		Try
			Return TH_Personas_Get(Nothing, Nothing, Nothing)
		Catch ex As Exception
			If (ex.InnerException Is Nothing) Then
				Throw ex
			Else
				Throw ex.InnerException
			End If
		End Try
	End Function
	Public Function DevolverxCargoID(ByVal CargoID As Int32?) As List(Of TH_Personas_Get_Result)
		Try
			Return TH_Personas_Get(Nothing, CargoID, Nothing)
		Catch ex As Exception
			If (ex.InnerException Is Nothing) Then
				Throw ex
			Else
				Throw ex.InnerException
			End If
		End Try
	End Function

	Public Function DevolverxCargoIDTodosLoscampos(ByVal CargoID As Int32?, ByVal TodosLosCampos As String) As List(Of TH_Personas_Get_Result)
		Try
			Return TH_Personas_Get(Nothing, CargoID, TodosLosCampos)
		Catch ex As Exception
			If (ex.InnerException Is Nothing) Then
				Throw ex
			Else
				Throw ex.InnerException
			End If
		End Try
	End Function

	Public Function DevolverxID(ByVal ID As Int64) As TH_Personas_Get_Result
		Try
			Dim oResult As List(Of TH_Personas_Get_Result)
			oResult = TH_Personas_Get(ID, Nothing, Nothing)
			If oResult.Count = 0 Then
				Return New TH_Personas_Get_Result
			Else
				Return oResult.Item(0)
			End If

		Catch ex As Exception
			If (ex.InnerException Is Nothing) Then
				Throw ex
			Else
				Throw ex.InnerException
			End If
		End Try
	End Function
	Public Function DevolverXTodosCampos(ByVal todosCampos As String) As List(Of TH_Personas_Get_Result)
		Try
			Return TH_Personas_Get(Nothing, Nothing, todosCampos)
		Catch ex As Exception
			If (ex.InnerException Is Nothing) Then
				Throw ex
			Else
				Throw ex.InnerException
			End If
		End Try
	End Function
	Private Function TH_Personas_Get(ByVal ID As Int64?, ByVal CargoID As Int64?, ByVal todosCampos As String) As List(Of TH_Personas_Get_Result)
		Try
			Dim oResult As ObjectResult(Of TH_Personas_Get_Result) = oMatrixContext.TH_Personas_Get(ID, CargoID, todosCampos)
			Return oResult.ToList()
		Catch ex As Exception
			If (ex.InnerException Is Nothing) Then
				Throw ex
			Else
				Throw ex.InnerException
			End If
		End Try
	End Function
	Public Function TH_Usuarios_Combo_Get() As List(Of US_Usuarios_Get_Combo_Result)
		Try
			Dim oResult As ObjectResult(Of US_Usuarios_Get_Combo_Result) = oMatrixContext.US_Usuarios_Get_Combo()
			Return oResult.ToList()
		Catch ex As Exception
			If (ex.InnerException Is Nothing) Then
				Throw ex
			Else
				Throw ex.InnerException
			End If
		End Try
	End Function

	Function OPEncuestadoresXid(ByVal id As Int64) As OP_Encuestadores4
		Return oMatrixContext.OP_Encuestadores.Where(Function(x) x.id = id).FirstOrDefault
	End Function

	Function Th_GetLogPersonas(ByVal Fecini As Date?, ByVal Fecfin As Date?, ByVal Cedula As Int64?, ByVal tipoCambio As String)
		Return oMatrixContext.TH_GetLogPersonas(Fecini, Fecfin, Cedula, tipoCambio).ToList
	End Function

	Function Th_REPCambiosContratacion(ByVal Fecini As Date?, ByVal Fecfin As Date?, ByVal Cedula As Int64?)
		Return oMatrixContext.TH_REPCambiosContratacion(Fecini, Fecfin, Cedula).ToList
	End Function

	Function Th_PersonasTipoContratacion(ByVal Cedula As Int64?, ByVal Nombre As String) As List(Of TH_PersonaXTipoContratacionGet_Result)
		Return oMatrixContext.TH_PersonaXTipoContratacionGet(Cedula, Nombre).ToList
	End Function

	Function Th_CarnetXPersona(ByVal PersonaId As Int64) As List(Of TH_CarnetXPersonaGet_Result)
		Return oMatrixContext.TH_CarnetXPersonaGet(PersonaId).ToList
	End Function

	Function TH_CapacitacionGeneralGet(ByVal PersonaId As Int64) As List(Of TH_CapacitacionGeneralGet_Result)
		Return oMatrixContext.TH_CapacitacionGeneralGet(PersonaId).ToList
	End Function
	Function TH_TipoEncuestadorDetalleGet(ByVal PersonaId As Int64) As List(Of TH_DetalleTipoEncuestadorGet_Result)
		Return oMatrixContext.TH_DetalleTipoEncuestadorGet(PersonaId).ToList
	End Function
	Function TH_EvaluacionesGet(ByVal PersonaId As Int64) As List(Of TH_EvaluacionesGet_Result)
		Return oMatrixContext.TH_EvaluacionesGet(PersonaId).ToList
	End Function
	Function Th_STGGet(ByVal PersonaId As Int64) As List(Of TH_STGGet_Result)
		Return oMatrixContext.TH_STGGet(PersonaId).ToList
	End Function
	Function Th_PStSinProduccion(ByVal Fechainicio As Date, ByVal FechaFin As Date) As List(Of TH_ReportePSTSinProduccionXFecha_Result)
		Return oMatrixContext.TH_ReportePSTSinProduccionXFecha(Fechainicio, FechaFin).ToList
	End Function
	Public Function personasActivas() As List(Of TH_Personas2)
		Return oMatrixContext.TH_Personas.Where(Function(x) x.Activo = True AndAlso x.Empresa = 1).ToList
	End Function
	Public Function obtenerBandas() As List(Of TH_Bandas_Get_Result)
		Return oMatrixContext.TH_Bandas_Get.ToList()
	End Function
	Public Function obtenerLevels() As List(Of TH_Levels_Get_Result)
		Return oMatrixContext.TH_Levels_Get.ToList()
	End Function
	Public Function obtenerAreasServicesLines() As List(Of TH_Area_Get_Result)
		Return oMatrixContext.TH_Area_Get(Nothing, Nothing).ToList
	End Function
	Public Function obtenerParentescos() As List(Of TH_Parentescos_Get_Result)
		Return oMatrixContext.TH_Parentescos_Get().ToList
	End Function
	Public Function obtenerHijosPorPersonaId(personaId As Long) As List(Of TH_Hijos_Get_Result)
		Return oMatrixContext.TH_Hijos_Get(personaId).ToList
	End Function
	Public Sub eliminarHijoPorId(id As Long)
		oMatrixContext.TH_Hijos_Del(id)
	End Sub
	Public Function agregarHijo(personaId As Long, nombres As String, apellidos As String, genero As Byte, fechaNacimiento As Date) As Long
		Return oMatrixContext.TH_Hijos_Add(personaId, nombres, apellidos, genero, fechaNacimiento)
	End Function
	Public Function obtenerContactosEmergenciaPorPersonaId(personaId As Long) As List(Of TH_ContactosEmergencia_Get_Result)
		Return oMatrixContext.TH_ContactosEmergencia_Get(personaId).ToList
	End Function
	Public Sub eliminarContactoEmergenciaPorId(id As Long)
		oMatrixContext.TH_ContactosEmergencia_Del(id)
	End Sub
    Public Function obtenerContactosEmergenciaEmpleados() As List(Of TH_ContactosEmergencia_Get_Result)
        Return oMatrixContext.TH_ContactosEmergencia_Get(Nothing).ToList
    End Function
    Public Function obtenerContactosEmergenciaEmpleadosReport() As List(Of TH_ContactosEmergencia_Report_Result)
        Return oMatrixContext.TH_ContactosEmergencia_Report(Nothing).ToList
    End Function
    Public Function agregarContactoEmergencia(personaId As Long, nombres As String, apellidos As String, parentesco As Byte, telefonoFijo As Long?, telefonoCelular As Long?) As Long
		Return oMatrixContext.TH_ContactosEmergencia_Add(personaId, nombres, apellidos, parentesco, telefonoFijo, telefonoCelular)
	End Function
	Public Function obtenerTiposIdentificacion() As List(Of TH_TipoIdentificacion_Get_Result)
		Return oMatrixContext.TH_TipoIdentificacion_Get.ToList
	End Function
	Public Function obtenerTallasCamiseta() As List(Of TH_TallasCamiseta)
		Return oMatrixContext.TH_TallasCamiseta.ToList
	End Function
#End Region
#Region "Grabar"
	Sub Grabar(ByVal id As Int64, ByVal nombres As String, ByVal apellidos As String, ByVal cargo As Int64, ByVal ciudadId As Int64, ByVal tipoContratacion As Int16, ByVal fechaIngreso As Date, ByVal fechaNacimiento As Date, ByVal activo As Boolean, ByVal insertarEncuestador As Boolean, ByVal tipoEncuestador As Int16?, Contratista As Int64?)
		Dim oeTH_Persona As New TH_Personas2


		If oMatrixContext.TH_Personas.Where(Function(x) x.id = id).Count > 0 Then
			oeTH_Persona = oMatrixContext.TH_Personas.Where(Function(x) x.id = id).FirstOrDefault
		End If

		oeTH_Persona.id = id
		oeTH_Persona.Nombres = nombres
		oeTH_Persona.Apellidos = apellidos
		oeTH_Persona.Cargo = cargo
		oeTH_Persona.CiudadId = ciudadId
		oeTH_Persona.TipoContratacion = tipoContratacion
		oeTH_Persona.FechaIngreso = fechaIngreso
		oeTH_Persona.FechaNacimiento = fechaNacimiento
		oeTH_Persona.Activo = activo
		oeTH_Persona.Contratista = Contratista

		Using ts As New Transactions.TransactionScope

			If oMatrixContext.TH_Personas.Where(Function(x) x.id = id).Count > 0 Then
				Dim CargoOriginal As Int64 = oeTH_Persona.Cargo
				oMatrixContext.SaveChanges()
				If cargo = Cargos.TiposCargos.Encuestador Then
					If CargoOriginal <> Cargos.TiposCargos.Encuestador Then
						Dim oOP_Encuestadores As New OP_Entities
						If oOP_Encuestadores.OP_Encuestadores.Where(Function(x) x.id = id).Count > 0 Then
							Dim oe As OP_Encuestadores2
							oe = oOP_Encuestadores.OP_Encuestadores.Where(Function(x) x.id = id).FirstOrDefault()
							oe.TipoId = tipoEncuestador
						Else
							oOP_Encuestadores.OP_Encuestadores.Add(New OP_Encuestadores2 With {.id = id, .TipoId = tipoEncuestador})
						End If

						oOP_Encuestadores.SaveChanges()
					Else
						Dim oOP_Encuestadores As New OP_Entities
						Dim oe As OP_Encuestadores2
						oe = oOP_Encuestadores.OP_Encuestadores.Where(Function(x) x.id = id).FirstOrDefault()
						oe.TipoId = tipoEncuestador
						oOP_Encuestadores.SaveChanges()
					End If
				End If
			Else
				oMatrixContext.TH_Personas.Add(oeTH_Persona)
				oMatrixContext.SaveChanges()
				If insertarEncuestador Then
					Dim oOP_Encuestadores As New OP_Entities
					oOP_Encuestadores.OP_Encuestadores.Add(New OP_Encuestadores2 With {.id = id, .TipoId = tipoEncuestador})
					oOP_Encuestadores.SaveChanges()
				End If
			End If


			ts.Complete()
		End Using
	End Sub

	Sub VetarEncuestador(ByVal id As Int64, usuario As Int64, RazonVeto As String)
		Dim ent As New OP_Encuestadores4
		ent = oMatrixContext.OP_Encuestadores.Where(Function(x) x.id = id).FirstOrDefault
		ent.Vetado = True
		ent.VetadoPor = usuario
		ent.RazonVeto = RazonVeto
		ent.FechaVetado = Now
		oMatrixContext.SaveChanges()
	End Sub

	Sub LogPersonas(ByVal Usuario As Int64, ByVal Campo As String, ByVal ValorOriginal As String, ByVal ValorNuevo As String, ByVal Persona As Int64)
		oMatrixContext.TH_LogPersonasAdd(Usuario, Campo, ValorOriginal, ValorNuevo, Persona)
	End Sub

	Sub EntregaCarnetGuardar(ByVal PersonaId As Int64, ByVal FechaEntrega As DateTime, ByVal FechaVencimiento As DateTime, ByVal PersonaEntrega As String)
		oMatrixContext.TH_CarnetAdd(PersonaId, FechaEntrega, FechaVencimiento, PersonaEntrega)
	End Sub

	Sub TH_CapacitacionGeneralAdd(ByVal PersonaId As Int64, ByVal Fecha As DateTime, ByVal Calificacion As Double, ByVal Capacitador As String)
		oMatrixContext.TH_CapacitacionGeneralAdd(PersonaId, Fecha, Calificacion, Capacitador)
	End Sub
	Sub TH_TipoEncuestadorDetalleAdd(ByVal PersonaId As Int64, ByVal Tipo As Int16)
		oMatrixContext.TH_DetalleTipoEncuestadorAdd(PersonaId, Tipo)
	End Sub
	Sub eliminarTiposEncuestador(ByVal personaId As Int64)
		oMatrixContext.TH_DetalleTipoEncuestador_Del(personaId)
	End Sub
	Sub TH_EvaluacionesAdd(ByVal PersonaId As Int64, ByVal Fecha As DateTime, ByVal Calificacion As Double, ByVal Evaluador As String)
		oMatrixContext.TH_EvaluacionesAdd(PersonaId, Fecha, Calificacion, Evaluador)
	End Sub
	Sub Th_STGAdd(ByVal PersonaId As Int64, ByVal Usuario As String, ByVal Clave As String, ByVal FechaBloqueo As DateTime?, ByVal UsuarioBloquea As String)
		oMatrixContext.TH_STGAdd(PersonaId, Usuario, Clave, FechaBloqueo, UsuarioBloquea)
	End Sub

	Sub TH_EvaluacionesActualizar(ByVal PersonaId As Int64, ByVal Fecha As DateTime, ByVal Calificacion As Double, ByVal Evaluador As String)
		oMatrixContext.TH_EvaluacionesUpdate(PersonaId, Fecha, Calificacion, Evaluador)
	End Sub
	Sub TH_CapacitacionGeneralActualizar(ByVal PersonaId As Int64, ByVal Fecha As DateTime, ByVal Calificacion As Double, ByVal Capacitador As String)
		oMatrixContext.TH_CapacitacionGeneralUpdate(PersonaId, Fecha, Calificacion, Capacitador)
	End Sub
	Sub TH_CarnetActualizar(ByVal PersonaId As Int64, ByVal FechaEntrega As DateTime, ByVal FechaVencimiento As DateTime, ByVal PersonaEntrega As String)
		oMatrixContext.TH_CarnetUpdate(PersonaId, FechaEntrega, FechaVencimiento, PersonaEntrega)
	End Sub
	Sub TH_STGActualizar(ByVal PersonaId As Int64, ByVal Usuario As String, ByVal Clave As String, ByVal FechaBloqueo As DateTime?, ByVal UsuarioBloquea As String)
		oMatrixContext.TH_STGUpdate(PersonaId, Usuario, Clave, FechaBloqueo, Usuario)
	End Sub

#End Region
End Class

